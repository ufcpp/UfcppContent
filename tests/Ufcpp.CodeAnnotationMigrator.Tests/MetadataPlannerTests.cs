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
    public void Plan_RejectsMultiplePartialSelectionsOfOneKind()
    {
        const string Code = "alpha + beta";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "alpha"),
            Selection(AnnotationKind.Highlight, Code, "beta"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Equal(
            "UNREPRESENTABLE_MULTIPLE_TEXT",
            Assert.Single(result.Diagnostics).Code);
        Assert.Null(result.Plan.Highlight);
    }

    [Fact]
    public void Plan_RejectsDifferentlySizedSelectionsThatOverlapAcrossKinds()
    {
        const string Code = "abcdef";
        var historical = Historical(
            Code,
            null,
            Selection(AnnotationKind.Highlight, Code, "abcdef"),
            Selection(AnnotationKind.Error, Code, "def"));

        var result = MetadataPlanner.Plan(historical, Current(Code));

        Assert.Equal(2, result.Diagnostics.Count);
        Assert.All(
            result.Diagnostics,
            diagnostic => Assert.Equal(
                "UNREPRESENTABLE_OVERLAPPING_KINDS",
                diagnostic.Code));
        Assert.Equal(
            [AnnotationKind.Highlight, AnnotationKind.Error],
            result.Diagnostics.Select(diagnostic => diagnostic.Kind));
        Assert.Null(result.Plan.Highlight);
        Assert.Null(result.Plan.Error);
    }

    [Fact]
    public void Plan_RejectsPartialTextThatExistsOnlyAfterEntityDecoding()
    {
        const string HistoricalCode = "value < limit";
        const string CurrentCode = "value &lt; limit";
        var historical = Historical(
            HistoricalCode,
            null,
            Selection(AnnotationKind.Highlight, HistoricalCode, "<"));

        var result = MetadataPlanner.Plan(historical, Current(CurrentCode));

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("UNREPRESENTABLE_MISSING_TEXT", diagnostic.Code);
        Assert.Null(result.Plan.Highlight);
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
}
