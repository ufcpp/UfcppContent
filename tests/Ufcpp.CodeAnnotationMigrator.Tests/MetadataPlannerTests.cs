using System.Security.Cryptography;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class MetadataPlannerTests
{
    [Fact]
    public void Plan_UsesWholeLinesAndOneUniquePartialTextPerKind()
    {
        const string Code = "first\n  whole\npartial token\nlast\n";
        var historical = Historical(
            Code,
            "sample.cs",
            Selection(AnnotationKind.Highlight, Code, "  whole"),
            Selection(AnnotationKind.Highlight, Code, "token"),
            Selection(AnnotationKind.Error, Code, "last"),
            Selection(AnnotationKind.Warning, Code, "irs"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal("sample.cs", result.Plan.Title);
        Assert.Equal(
            new SelectionMetadataPlan("2", "token"),
            result.Plan.Highlight);
        Assert.Equal(
            new SelectionMetadataPlan("4", null),
            result.Plan.Error);
        Assert.Equal(
            new SelectionMetadataPlan(null, "irs"),
            result.Plan.Warning);
    }

    [Fact]
    public void Plan_CollapsesAdjacentAndDisjointWholeLineSelections()
    {
        const string Code = "zero\none\ntwo\nthree\nfour\n";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "zero"),
            Selection(AnnotationKind.Highlight, Code, "one\ntwo"),
            Selection(AnnotationKind.Highlight, Code, "four"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal("1-3,5", result.Plan.Highlight?.Lines);
        Assert.Null(result.Plan.Highlight?.Text);
    }

    [Fact]
    public void Plan_MapsWholeLinesAcrossFramingIndentationAndNewlines()
    {
        const string HistoricalCode = "\r\n\tvalue < 2  \r\n";
        const string CurrentCode = "  value &lt; 2\n";
        var historical = Historical(
            HistoricalCode,
            null,
            Selection(
                AnnotationKind.Error,
                HistoricalCode,
                "\tvalue < 2  "));

        var result = MetadataPlanner.Plan(historical, Current(CurrentCode));

        Assert.Empty(result.Diagnostics);
        Assert.Equal("1", result.Plan.Error?.Lines);
    }

    [Fact]
    public void Plan_RejectsRepeatedPartialText()
    {
        const string Code = "token + token";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Warning, Code, "token"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("UNREPRESENTABLE_REPEATED_TEXT", diagnostic.Code);
        Assert.Equal(AnnotationKind.Warning, diagnostic.Kind);
        Assert.Null(result.Plan.Warning);
    }

    [Fact]
    public void Plan_RepeatedHighlightTextUsesExactRangeFallback()
    {
        const string Code = "token + token";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "token"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:1-1:6"),
            result.Plan.Highlight?.Ranges);
        Assert.Null(result.Plan.Highlight?.Text);
    }

    [Fact]
    public void Plan_RejectsMultilinePartialText()
    {
        const string Code = "prefix one\nsecond suffix";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Error, Code, "one\nsecond"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Equal(
            "UNREPRESENTABLE_MULTILINE_TEXT",
            Assert.Single(result.Diagnostics).Code);
        Assert.Null(result.Plan.Error);
    }

    [Fact]
    public void Plan_MultilineHighlightUsesExactRangeFallback()
    {
        const string Code = "prefix one\nsecond suffix";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "one\nsecond"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:8-2:7"),
            result.Plan.Highlight?.Ranges);
    }

    [Fact]
    public void Plan_MultiplePartialHighlightsUseOrderedRangeFallback()
    {
        const string Code = "alpha + beta";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "alpha"),
            Selection(AnnotationKind.Highlight, Code, "beta"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:1-1:6,1:9-1:13"),
            result.Plan.Highlight?.Ranges);
    }

    [Fact]
    public void Plan_AdjacentPartialHighlightsMergeIntoCanonicalRange()
    {
        const string Code = "alphabeta";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "alpha"),
            Selection(AnnotationKind.Highlight, Code, "beta"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:1-1:10"),
            result.Plan.Highlight?.Ranges);
    }

    [Fact]
    public void Plan_RangeProjectionMapsIndentEntitiesAndUnicodeScalarsExactly()
    {
        const string HistoricalCode = "\ta😀<b";
        const string CurrentCode = "  a😀&lt;b";
        var historical = Historical(
            HistoricalCode,
            null,
            Selection(AnnotationKind.Highlight, HistoricalCode, "😀<"));

        var result = MetadataPlanner.Plan(historical, Current(CurrentCode));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(CurrentCode, "1:4-1:9"),
            result.Plan.Highlight?.Ranges);
    }

    [Fact]
    public void Plan_TrailingWhitespaceHighlightUsesGuardedLineEndAnchor()
    {
        const string Code = "value    \n    next";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "    "));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:6-1:10"),
            result.Plan.Highlight?.Ranges);
    }

    [Fact]
    public void Plan_RestoresHighlightWhenIssueFiveKindOverlaps()
    {
        const string Code = "abcdef";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "abcdef"),
            Selection(AnnotationKind.Error, Code, "def"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("UNREPRESENTABLE_OVERLAPPING_KINDS", diagnostic.Code);
        Assert.Equal(AnnotationKind.Error, diagnostic.Kind);
        Assert.Equal("1", result.Plan.Highlight?.Lines);
        Assert.Null(result.Plan.Error);
    }

    [Fact]
    public void Plan_EntityEncodedSelectionUsesExactRangeFallback()
    {
        const string HistoricalCode = "value < limit";
        const string CurrentCode = "value &lt; limit";
        var historical = Historical(
            HistoricalCode,
            null,
            Selection(AnnotationKind.Highlight, HistoricalCode, "<"));

        var result = MetadataPlanner.Plan(historical, Current(CurrentCode));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(CurrentCode, "1:7-1:11"),
            result.Plan.Highlight?.Ranges);
    }

    [Fact]
    public void Plan_RejectsEntityDecodedNewlineThatChangesPhysicalLayout()
    {
        const string HistoricalCode = "a\nb";
        const string CurrentCode = "a&#10;b";
        var historical = Historical(
            HistoricalCode,
            null,
            Selection(AnnotationKind.Error, HistoricalCode, "b"));

        var result = MetadataPlanner.Plan(historical, Current(CurrentCode));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("UNREPRESENTABLE_SOURCE_LAYOUT", diagnostic.Code);
        Assert.Null(result.Plan.Error);
    }

    [Fact]
    public void Plan_RangeFallbackPreservesSemanticOccurrence()
    {
        const string HistoricalCode = "< <";
        const string CurrentCode = "&lt; <";
        var historical = Historical(
            HistoricalCode,
            null,
            Selection(AnnotationKind.Highlight, HistoricalCode, "<"));

        var result = MetadataPlanner.Plan(historical, Current(CurrentCode));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(CurrentCode, "1:1-1:5"),
            result.Plan.Highlight?.Ranges);
    }

    private static HistoricalCodeBlock Historical(
        string code,
        string? title,
        params AnnotationSelection[] annotations) =>
        new(1, 0, 1, false, title, code, annotations);

    private static CurrentCodeBlock Current(string code) =>
        new(1, 0, 1, CurrentCodeBlockKind.Fenced, false, code);

    private static AnnotationSelection Selection(
        AnnotationKind kind,
        string code,
        string text)
    {
        var start = code.IndexOf(text, StringComparison.Ordinal);
        Assert.True(start >= 0, $"Selection '{text}' was not found.");
        return new AnnotationSelection(kind, start, text.Length, text);
    }

    private static string RangeMetadata(string code, string ranges)
    {
        var normalized = code
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');
        var hash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(normalized)))
            .ToLowerInvariant();
        return $"sha256:{hash};{ranges}";
    }
}
