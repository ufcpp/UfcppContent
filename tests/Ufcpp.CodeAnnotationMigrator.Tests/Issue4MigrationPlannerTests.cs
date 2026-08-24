namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class Issue4MigrationPlannerTests
{
    [Fact]
    public void Plan_RewritesMatchedMetadataAndSecondRunHasNoChanges()
    {
        var historical = new Dictionary<string, string>
        {
            ["sample.md"] =
                "<pre title=\"sample\"><code><em>token</em> + token</code></pre>",
        };
        var current = new Dictionary<string, string>
        {
            ["sample.md"] = "```text\ntoken + token\n```\n",
        };
        var input = Input(historical, current);
        var analysis = MigrationAnalyzer.Analyze(input);

        var first = Issue4MigrationPlanner.Plan(input, analysis.Report, []);
        var updated = new Dictionary<string, string>(current)
        {
            ["sample.md"] = first.ChangedDocuments["sample.md"],
        };
        var secondInput = Input(historical, updated);
        var second = Issue4MigrationPlanner.Plan(
            secondInput,
            MigrationAnalyzer.Analyze(secondInput).Report,
            []);

        Assert.Single(first.ChangedDocuments);
        Assert.Contains("title=\"sample\"", updated["sample.md"]);
        Assert.Contains("highlight-ranges=", updated["sample.md"]);
        Assert.Equal(
            "token + token",
            Assert.Single(CurrentBlockDiscoverer.Discover(updated["sample.md"])).Code);
        Assert.Empty(second.ChangedDocuments);
    }

    [Fact]
    public void Plan_UnresolvedIssue4DiagnosticFailsWithoutMutatingInputs()
    {
        var historical = new Dictionary<string, string>
        {
            ["sample.md"] = "<pre title=\"lost\"><code>old</code></pre>",
        };
        var current = new Dictionary<string, string>
        {
            ["sample.md"] = "```text\nnew\n```\n",
        };
        var input = Input(historical, current);
        var original = current["sample.md"];

        Assert.Throws<InvalidDataException>(
            () => Issue4MigrationPlanner.Plan(
                input,
                MigrationAnalyzer.Analyze(input).Report,
                []));
        Assert.Equal(original, current["sample.md"]);
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
