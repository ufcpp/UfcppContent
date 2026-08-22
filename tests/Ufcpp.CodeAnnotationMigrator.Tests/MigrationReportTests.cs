using System.Text;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class MigrationReportTests
{
    [Fact]
    public void Analyze_ReportsStableCoveragePlansAndDiagnostics()
    {
        var historical = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["d.md"] = "<pre><code>missing</code></pre>",
            ["c.md"] =
                "<table><tr><td><pre><code>table</code></pre></td></tr></table>",
            ["b.md"] =
                "<pre><code>repeat <span class=\"error\">repeat</span></code></pre>",
            ["a.md"] =
                "<pre title=\"sample.cs\"><code>alpha <em>token</em></code></pre>",
        };
        var current = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["z.md"] = "```text\nnew\n```",
            ["c.md"] =
                "<table><tr><td><pre><code>table</code></pre></td></tr></table>",
            ["b.md"] = "```text\nrepeat repeat\n```",
            ["a.md"] = "```text\nalpha token\n```",
        };

        var outcome = MigrationAnalyzer.Analyze(
            new MigrationAnalysisInput(
                "0123456789012345678901234567890123456789",
                "content",
                "content",
                historical,
                current));

        Assert.Equal(3, outcome.ExitCode);
        Assert.Equal(
            new ReportTotals(4, 4, 4, 0, 3, 0, 1),
            outcome.Report.Totals);
        Assert.Equal(new CoverageCounts(1, 1, 0, 0, 0), outcome.Report.Coverage.Title);
        Assert.Equal(
            new CoverageCounts(1, 1, 0, 0, 0),
            outcome.Report.Coverage.Highlight);
        Assert.Equal(
            new CoverageCounts(1, 0, 0, 0, 1),
            outcome.Report.Coverage.Error);
        Assert.Equal(
            new CoverageCounts(3, 1, 0, 1, 1),
            outcome.Report.Coverage.FencedBlocks);
        Assert.Equal(
            new CoverageCounts(1, 1, 0, 0, 0),
            outcome.Report.Coverage.RawTableBlocks);

        var plan = Assert.Single(outcome.Report.Plans);
        Assert.Equal("a.md", plan.Path);
        Assert.Equal("sample.cs", plan.Metadata.Title);
        Assert.Equal("token", plan.Metadata.Highlight?.Text);
        Assert.Equal(
            [
                "UNREPRESENTABLE_REPEATED_TEXT",
                "UNMATCHED_BLOCK",
                "CURRENT_ONLY_DOCUMENT",
            ],
            outcome.Report.Diagnostics.Select(diagnostic => diagnostic.Code));
    }

    [Fact]
    public void Serialize_IsByteIdenticalAndUsesStableOrdering()
    {
        var input = new MigrationAnalysisInput(
            "0123456789012345678901234567890123456789",
            "content",
            "content",
            new Dictionary<string, string>
            {
                ["b.md"] = "<pre title=\"B\">beta</pre>",
                ["a.md"] = "<pre title=\"A\">alpha</pre>",
            },
            new Dictionary<string, string>
            {
                ["a.md"] = "```text\nalpha\n```",
                ["b.md"] = "```text\nbeta\n```",
            });

        var first = MigrationReportWriter.Serialize(MigrationAnalyzer.Analyze(input).Report);
        var second = MigrationReportWriter.Serialize(MigrationAnalyzer.Analyze(input).Report);

        Assert.Equal(first, second);
        Assert.Equal((byte)'\n', first[^1]);
        var json = Encoding.UTF8.GetString(first);
        Assert.DoesNotContain("generatedAt", json, StringComparison.OrdinalIgnoreCase);
        Assert.True(json.IndexOf("\"a.md\"", StringComparison.Ordinal)
            < json.IndexOf("\"b.md\"", StringComparison.Ordinal));
    }

    [Fact]
    public void Analyze_ReturnsSuccessWhenAllBlocksAndMetadataAreSafe()
    {
        var outcome = MigrationAnalyzer.Analyze(
            new MigrationAnalysisInput(
                "0123456789012345678901234567890123456789",
                "content",
                "content",
                new Dictionary<string, string>
                {
                    ["safe.md"] = "<pre><code><em>whole line</em></code></pre>",
                },
                new Dictionary<string, string>
                {
                    ["safe.md"] = "```text\nwhole line\n```",
                }));

        Assert.Equal(0, outcome.ExitCode);
        Assert.Empty(outcome.Report.Diagnostics);
        Assert.Equal(
            new CoverageCounts(1, 1, 0, 0, 0),
            outcome.Report.Coverage.Highlight);
    }

    [Fact]
    public void Analyze_ReportsMalformedBlockAndStillAnalyzesLaterBlocks()
    {
        var outcome = MigrationAnalyzer.Analyze(
            new MigrationAnalysisInput(
                "0123456789012345678901234567890123456789",
                "content",
                "content",
                new Dictionary<string, string>
                {
                    ["mixed.md"] =
                        "<pre><code>first</code></code></pre>\n"
                        + "<pre title=\"good\"><code>second</code></pre>",
                },
                new Dictionary<string, string>
                {
                    ["mixed.md"] =
                        "```text\nfirst\n```\n"
                        + "```text\nsecond\n```",
                }));

        Assert.Equal(3, outcome.ExitCode);
        Assert.Equal(2, outcome.Report.Totals.HistoricalPreBlocks);
        Assert.Equal(1, outcome.Report.Totals.MalformedHistoricalBlocks);
        Assert.Equal(
            new CoverageCounts(2, 1, 0, 0, 1),
            outcome.Report.Coverage.FencedBlocks);
        Assert.Contains(
            outcome.Report.Diagnostics,
            diagnostic =>
                diagnostic.Code == "MALFORMED_HISTORICAL_BLOCK"
                && diagnostic.HistoricalOrdinal == 1);
        var plan = Assert.Single(outcome.Report.Plans);
        Assert.Equal(2, plan.HistoricalOrdinal);
        Assert.Equal(2, plan.CurrentOrdinal);
    }

    [Theory]
    [InlineData(
        "<pre><code>value</code></pre>",
        "<pre><code>value</code></pre>",
        "fencedBlocks")]
    [InlineData(
        "<table><pre><code>value</code></pre></table>",
        "```text\nvalue\n```",
        "rawTableBlocks")]
    public void Analyze_RejectsMatchedCodeInWrongTargetKind(
        string historical,
        string current,
        string coverageName)
    {
        var outcome = MigrationAnalyzer.Analyze(
            new MigrationAnalysisInput(
                "0123456789012345678901234567890123456789",
                "content",
                "content",
                new Dictionary<string, string> { ["kind.md"] = historical },
                new Dictionary<string, string> { ["kind.md"] = current }));

        Assert.Equal(3, outcome.ExitCode);
        var diagnostic = Assert.Single(outcome.Report.Diagnostics);
        Assert.Equal("UNEXPECTED_TARGET_KIND", diagnostic.Code);
        var coverage = coverageName == "fencedBlocks"
            ? outcome.Report.Coverage.FencedBlocks
            : outcome.Report.Coverage.RawTableBlocks;
        Assert.Equal(new CoverageCounts(1, 0, 0, 0, 1), coverage);
    }
}
