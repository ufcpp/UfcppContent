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
    public void Plan_RepeatedWarningTextUsesExactRangeFallback()
    {
        const string Code = "token + token";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Warning, Code, "token"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:1-1:6"),
            result.Plan.Warning?.Ranges);
        Assert.Null(result.Plan.Warning?.Text);
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
    public void Plan_MultilineErrorUsesExactRangeFallback()
    {
        const string Code = "prefix one\nsecond suffix";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Error, Code, "one\nsecond"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:8-2:7"),
            result.Plan.Error?.Ranges);
        Assert.Null(result.Plan.Error?.Text);
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
    public void Plan_PreservesDifferentlySizedHighlightAndErrorSelections()
    {
        const string Code = "abcdef";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "abcdef"),
            Selection(AnnotationKind.Error, Code, "def"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal("1", result.Plan.Highlight?.Lines);
        Assert.Equal("def", result.Plan.Error?.Text);
    }

    [Fact]
    public void Plan_MultiplePartialErrorsUseOrderedRangeFallback()
    {
        const string Code = "alpha + beta";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Error, Code, "alpha"),
            Selection(AnnotationKind.Error, Code, "beta"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            RangeMetadata(Code, "1:1-1:6,1:9-1:13"),
            result.Plan.Error?.Ranges);
        Assert.Null(result.Plan.Error?.Text);
    }

    [Fact]
    public void Plan_ErrorAndWarningPartialOverlapRemainDistinct()
    {
        const string Code = "abcdef";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Error, Code, "cde"),
            Selection(AnnotationKind.Warning, Code, "abcdef"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal("cde", result.Plan.Error?.Text);
        Assert.Equal("1", result.Plan.Warning?.Lines);
    }

    [Fact]
    public void Plan_DiagnosticIdentitiesPreserveNestedAndDuplicateOccurrences()
    {
        const string Code = "value";
        var historical = Historical(
            Code,
            null,
            new AnnotationSelection(
                AnnotationKind.Error,
                0,
                Code.Length,
                Code,
                "CS1001",
                0),
            new AnnotationSelection(
                AnnotationKind.Error,
                0,
                Code.Length,
                Code,
                "CS1002",
                1),
            new AnnotationSelection(
                AnnotationKind.Error,
                0,
                Code.Length,
                Code,
                "CS1002",
                2));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Empty(result.Diagnostics);
        Assert.Equal(
            DiagnosticMetadata(
                Code,
                "CS1001@1:1-1:6,CS1002@1:1-1:6,CS1002@1:1-1:6"),
            result.Plan.Error?.Diagnostics);
    }

    [Fact]
    public void Plan_EmptySelectionIsDiagnosedWithoutDroppingVisibleSelections()
    {
        const string Code = "x();";
        var historical = Historical(
            Code,
            null,
            new AnnotationSelection(AnnotationKind.Error, 1, 2, "()"),
            new AnnotationSelection(AnnotationKind.Error, 2, 0, string.Empty));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("EMPTY_ANNOTATION_SELECTION", diagnostic.Code);
        Assert.Equal(AnnotationKind.Error, diagnostic.Kind);
        Assert.Equal("()", result.Plan.Error?.Text);
    }

    [Fact]
    public void Plan_NewlineOnlySelectionFailsBeforeEmittingRendererMetadata()
    {
        const string Code = "a\nb";
        var historical = Historical(
            Code,
            null,
            new AnnotationSelection(AnnotationKind.Error, 1, 1, "\n"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("UNREPRESENTABLE_RANGE_PROJECTION", diagnostic.Code);
        Assert.Equal(AnnotationKind.Error, diagnostic.Kind);
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

    private static string DiagnosticMetadata(string code, string entries) =>
        $"sha256:{HighlightRangePlanner.ComputeHash(code)};{entries}";
}
