namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class DocumentAnnotationRewriterTests
{
    [Fact]
    public void Rewrite_CanonicalizesMetadataWithoutChangingCodeText()
    {
        const string Code = "var value = \"<tag>\";";
        var source =
            "# Heading\n\n"
            + "```csharp\n"
            + Code
            + "\n```\n\nTail\n";
        var plan = Plan(
            Code,
            new BlockMetadataPlan(
                """A "quoted" & {value}""",
                new SelectionMetadataPlan(
                    "1",
                    "value",
                    "sha256:"
                    + HighlightRangePlanner.ComputeHash(Code)
                    + ";1:5-1:10"),
                null,
                null));

        var result = DocumentAnnotationRewriter.Rewrite(
            "sample.md",
            source,
            [plan]);

        Assert.Equal(1, result.ReplacementCount);
        Assert.Contains(
            "```csharp {title=\"A &quot;quoted&quot; &amp; {value}\" "
            + "highlight-lines=\"1\" highlight-text=\"value\" "
            + $"highlight-ranges=\"sha256:{HighlightRangePlanner.ComputeHash(Code)};"
            + "1:5-1:10\"}\n",
            result.Content);
        Assert.Equal(
            Code,
            Assert.Single(CurrentBlockDiscoverer.Discover(result.Content)).Code);
        Assert.Equal(
            source.Replace("```csharp", result.Content.Split('\n')[2], StringComparison.Ordinal),
            result.Content);
    }

    [Fact]
    public void Rewrite_SecondRunIsIdempotent()
    {
        const string Code = "alpha + beta";
        var source = $"```text\n{Code}\n```\n";
        var plan = Plan(
            Code,
            new BlockMetadataPlan(
                "sample",
                new SelectionMetadataPlan(null, "alpha"),
                null,
                null));

        var first = DocumentAnnotationRewriter.Rewrite("sample.md", source, [plan]);
        var second = DocumentAnnotationRewriter.Rewrite(
            "sample.md",
            first.Content,
            [plan]);

        Assert.Equal(1, first.ReplacementCount);
        Assert.Equal(0, second.ReplacementCount);
        Assert.Equal(first.Content, second.Content);
    }

    [Fact]
    public void Rewrite_RejectsConflictingExistingMetadata()
    {
        const string Code = "value";
        var source = $"```text {{title=\"different\"}}\n{Code}\n```\n";
        var plan = Plan(
            Code,
            new BlockMetadataPlan("expected", null, null, null));

        var exception = Assert.Throws<InvalidDataException>(
            () => DocumentAnnotationRewriter.Rewrite("sample.md", source, [plan]));

        Assert.Contains("conflicts", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Rewrite_EncodesMetadataValueContainingBothQuoteCharacters()
    {
        const string Code = "value";
        var source = $"```text\n{Code}\n```\n";
        var plan = Plan(
            Code,
            new BlockMetadataPlan("""both " and ' quotes with `tick`""", null, null, null));

        var result = DocumentAnnotationRewriter.Rewrite("sample.md", source, [plan]);

        Assert.Contains(
            "title=\"both &quot; and ' quotes with &#96;tick&#96;\"",
            result.Content);
    }

    [Fact]
    public void Rewrite_RawTableRangeWrapsWholeEntityAndPreservesVisibleCode()
    {
        const string Code = "alpha < beta";
        const string Source =
            "<table><tr><td><pre class=\"source\" title=\"sample\">"
            + "<code>alpha &lt; beta</code></pre></td></tr></table>\n";
        var ranges =
            $"sha256:{HighlightRangePlanner.ComputeHash(Code)};1:7-1:8";
        var plan = Plan(
            Code,
            new BlockMetadataPlan(
                "sample",
                new SelectionMetadataPlan(null, null, ranges),
                null,
                null),
            targetKind: "rawPreInTable");

        var first = DocumentAnnotationRewriter.Rewrite("sample.md", Source, [plan]);
        var second = DocumentAnnotationRewriter.Rewrite(
            "sample.md",
            first.Content,
            [plan]);

        Assert.Contains(
            "<mark class=\"code-highlight\">&lt;</mark>",
            first.Content);
        Assert.Equal(
            Code,
            Assert.Single(CurrentBlockDiscoverer.Discover(first.Content)).Code);
        Assert.Equal(0, second.ReplacementCount);
        Assert.Equal(first.Content, second.Content);
    }

    [Fact]
    public void Rewrite_RawTableUniqueTextUsesPermanentMarkElement()
    {
        const string Code = "alpha beta";
        const string Source =
            "<table><tr><td><pre><code>alpha beta</code></pre></td></tr></table>\n";
        var plan = Plan(
            Code,
            new BlockMetadataPlan(
                null,
                new SelectionMetadataPlan(null, "beta"),
                null,
                null),
            targetKind: "rawPreInTable");

        var result = DocumentAnnotationRewriter.Rewrite("sample.md", Source, [plan]);

        Assert.Contains(
            "alpha <mark class=\"code-highlight\">beta</mark>",
            result.Content);
    }

    private static ReportPlan Plan(string code, BlockMetadataPlan metadata) =>
        Plan(code, metadata, "fenced");

    private static ReportPlan Plan(
        string code,
        BlockMetadataPlan metadata,
        string targetKind) =>
        new(
            "sample.md",
            1,
            1,
            1,
            1,
            targetKind,
            "ordinalAndHash",
            CodeNormalizer.Hash(code),
            metadata);
}
