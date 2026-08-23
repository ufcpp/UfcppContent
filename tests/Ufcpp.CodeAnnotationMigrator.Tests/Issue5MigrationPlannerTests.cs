namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class Issue5MigrationPlannerTests
{
    [Fact]
    public void Plan_RewritesTypedRangesAndSecondRunHasNoChanges()
    {
        var historical = new Dictionary<string, string>
        {
            ["sample.md"] =
                "<pre><code>token + <span class=\"error\">token</span></code></pre>",
        };
        var current = new Dictionary<string, string>
        {
            ["sample.md"] = "```text\ntoken + token\n```\n",
        };
        var input = Input(historical, current);
        var first = Issue5MigrationPlanner.Plan(
            input,
            MigrationAnalyzer.Analyze(input).Report,
            []);
        var updated = new Dictionary<string, string>(current)
        {
            ["sample.md"] = first.ChangedDocuments["sample.md"],
        };
        var secondInput = Input(historical, updated);
        var second = Issue5MigrationPlanner.Plan(
            secondInput,
            MigrationAnalyzer.Analyze(secondInput).Report,
            []);

        Assert.Single(first.ChangedDocuments);
        Assert.Contains("error-ranges=", updated["sample.md"]);
        Assert.Equal(
            "token + token",
            Assert.Single(CurrentBlockDiscoverer.Discover(updated["sample.md"])).Code);
        Assert.Empty(second.ChangedDocuments);
    }

    [Fact]
    public void Plan_PreservesAppliedIssue4Metadata()
    {
        const string Historical =
            "<pre title=\"sample\"><code><em>alpha</em> "
            + "<span class=\"warning\">beta</span></code></pre>";
        const string Current =
            "```text {title=\"sample\" highlight-text=\"alpha\"}\nalpha beta\n```\n";
        var input = Input(
            new Dictionary<string, string> { ["sample.md"] = Historical },
            new Dictionary<string, string> { ["sample.md"] = Current });

        var result = Issue5MigrationPlanner.Plan(
            input,
            MigrationAnalyzer.Analyze(input).Report,
            []);

        Assert.Contains(
            "title=\"sample\" highlight-text=\"alpha\" warning-text=\"beta\"",
            result.ChangedDocuments["sample.md"]);
    }

    [Fact]
    public void Plan_UncataloguedEmptySelectionFailsClosed()
    {
        const string Historical =
            "<pre><code>x<span class=\"error\">(<span class=\"error\"></span>)</span>;"
            + "</code></pre>";
        const string Current = "```text\nx();\n```\n";
        var input = Input(
            new Dictionary<string, string> { ["sample.md"] = Historical },
            new Dictionary<string, string> { ["sample.md"] = Current });

        var exception = Assert.Throws<InvalidDataException>(
            () => Issue5MigrationPlanner.Plan(
                input,
                MigrationAnalyzer.Analyze(input).Report,
                []));

        Assert.Contains("EMPTY_ANNOTATION_SELECTION", exception.Message);
    }

    [Fact]
    public void Plan_CataloguedEmptySelectionRestoresVisibleSelectionOnly()
    {
        const string Historical =
            "<pre><code>x<span class=\"error\">(<span class=\"error\"></span>)</span>;"
            + "</code></pre>";
        const string Current = "```text\nx();\n```\n";
        var input = Input(
            new Dictionary<string, string> { ["sample.md"] = Historical },
            new Dictionary<string, string> { ["sample.md"] = Current });
        var exception = new Issue5ExceptionEntry(
            "EX5-EMPTY",
            "EMPTY_ANNOTATION_SELECTION",
            "sample.md",
            1,
            1,
            GitBlobId.Compute(Historical),
            1,
            1,
            [CodeNormalizer.Hash("x();")],
            Issue5ExceptionDisposition.Obsolete,
            AnnotationKind.Error,
            null,
            1,
            0,
            "The empty legacy wrapper has no visible text.");

        var result = Issue5MigrationPlanner.Plan(
            input,
            MigrationAnalyzer.Analyze(input).Report,
            [exception]);

        Assert.Contains("error-text=\"()\"", result.ChangedDocuments["sample.md"]);
        Assert.Equal(exception, Assert.Single(result.Exceptions));
    }

    [Fact]
    public void ExceptionEntry_BuildsGuardedMalformedRangePlan()
    {
        const string Historical = "<pre><code>try</em></code></pre>";
        const string CurrentCode = "try bad";
        const string Current = "```text\ntry bad\n```\n";
        var input = Input(
            new Dictionary<string, string> { ["sample.md"] = Historical },
            new Dictionary<string, string> { ["sample.md"] = Current });
        var entry = new Issue5ExceptionEntry(
            "EX5-MALFORMED",
            "MALFORMED_HISTORICAL_BLOCK",
            "sample.md",
            1,
            1,
            GitBlobId.Compute(Historical),
            1,
            1,
            [CodeNormalizer.Hash(CurrentCode)],
            Issue5ExceptionDisposition.MalformedResolved,
            AnnotationKind.Error,
            "1:5-1:8",
            1,
            1,
            "The exact current token retains the malformed legacy error.",
            HighlightRangePlanner.ComputeHash(CurrentCode),
            "bad");

        var application = entry.CreateApplication(input);

        var plan = Assert.IsType<ReportPlan>(application.Plan);
        Assert.Equal(
            $"sha256:{HighlightRangePlanner.ComputeHash(CurrentCode)};1:5-1:8",
            plan.Metadata.Error?.Ranges);
        Assert.Throws<InvalidDataException>(
            () => entry.CreateApplication(
                input with
                {
                    CurrentDocuments = new Dictionary<string, string>
                    {
                        ["sample.md"] = "```text\nchanged\n```\n",
                    },
                }));
        Assert.Throws<InvalidDataException>(
            () => entry.CreateApplication(
                input with
                {
                    CurrentDocuments = new Dictionary<string, string>
                    {
                        ["sample.md"] = "```text\n try bad\n```\n",
                    },
                }));
    }

    private static MigrationAnalysisInput Input(
        IReadOnlyDictionary<string, string> historical,
        IReadOnlyDictionary<string, string> current) =>
        new(
            "0123456789012345678901234567890123456789",
            "content",
            "9876543210987654321098765432109876543210",
            "content",
            historical,
            current);
}
