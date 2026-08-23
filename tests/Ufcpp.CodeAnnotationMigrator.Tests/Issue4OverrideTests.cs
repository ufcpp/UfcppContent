namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class Issue4OverrideTests
{
    [Fact]
    public void CreateApplication_RequiresExactPathOrdinalsLinesAndHashes()
    {
        const string HistoricalCode = "historical";
        const string CurrentCode = "current";
        const string HistoricalDocument =
            "<pre title=\"restored\"><code>historical</code></pre>";
        const string CurrentDocument = "```text\ncurrent\n```";
        var entry = new Issue4OverrideEntry(
            "OVR-TEST",
            "sample.md",
            1,
            1,
            CodeNormalizer.Hash(HistoricalCode),
            null,
            1,
            1,
            [CodeNormalizer.Hash(CurrentCode)],
            "restored",
            null,
            null);

        var application = entry.CreateApplication(
            "sample.md",
            HistoricalDocument,
            CurrentDocument);

        Assert.Equal(CurrentDocument, application.CurrentDocument);
        Assert.Equal("restored", application.Plan.Metadata.Title);
        Assert.Equal(CodeNormalizer.Hash(CurrentCode), application.Plan.Hash);
        Assert.Throws<InvalidDataException>(
            () => entry.CreateApplication(
                "sample.md",
                HistoricalDocument,
                "```text\nchanged\n```"));
    }

    [Fact]
    public void CreateApplication_GuardsMalformedSourceByGitBlobHash()
    {
        const string HistoricalDocument = "<pre title=\"restored\">bad</code></pre>";
        const string CurrentDocument = "```text\nbad\n```";
        var entry = new Issue4OverrideEntry(
            "OVR-MALFORMED",
            "sample.md",
            1,
            1,
            null,
            GitBlobId.Compute(HistoricalDocument),
            1,
            1,
            [CodeNormalizer.Hash("bad")],
            "restored",
            null,
            null);

        Assert.Equal(
            "restored",
            entry.CreateApplication(
                    "sample.md",
                    HistoricalDocument,
                    CurrentDocument)
                .Plan.Metadata.Title);
        Assert.Throws<InvalidDataException>(
            () => entry.CreateApplication(
                "sample.md",
                HistoricalDocument + " ",
                CurrentDocument));
    }

    [Fact]
    public void CreateApplication_RemovesOnlyExactLegacyHighlightTagsIdempotently()
    {
        const string HistoricalDocument =
            "<pre><code>prefix <em>selected</em> suffix</code></pre>";
        const string CurrentCode = "prefix <em>selected</em> suffix";
        const string CleanCode = "prefix selected suffix";
        var currentDocument = $"```text\n{CurrentCode}\n```";
        var entry = new Issue4OverrideEntry(
            "OVR-LITERAL-EM",
            "sample.md",
            1,
            1,
            CodeNormalizer.Hash(CleanCode),
            null,
            1,
            1,
            [CodeNormalizer.Hash(CurrentCode), CodeNormalizer.Hash(CleanCode)],
            null,
            "selected",
            new Issue4SourceCleanup("<em>", "</em>"));

        var first = entry.CreateApplication(
            "sample.md",
            HistoricalDocument,
            currentDocument);
        var second = entry.CreateApplication(
            "sample.md",
            HistoricalDocument,
            first.CurrentDocument);

        Assert.DoesNotContain("<em>", first.CurrentDocument);
        Assert.Equal(first.CurrentDocument, second.CurrentDocument);
        Assert.Equal("selected", second.Plan.Metadata.Highlight?.Text);
        Assert.Equal(CodeNormalizer.Hash(CleanCode), second.Plan.Hash);
    }
}
