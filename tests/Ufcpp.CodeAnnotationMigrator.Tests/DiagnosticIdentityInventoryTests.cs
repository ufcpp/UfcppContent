using System.Text.Json;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class DiagnosticIdentityInventoryTests
{
    [Fact]
    public async Task PinnedRepository_ReconcilesExactDiagnosticIdentityInventory()
    {
        var root = FindRepositoryRoot();
        var repository = await GitRepositoryReader.LoadAsync(
            root,
            MigratorCliOptionsParser.PinnedSourceCommit,
            "content",
            "content");
        var input = new MigrationAnalysisInput(
            repository.ResolvedSourceCommit,
            repository.SourcePath,
            repository.ResolvedCurrentCommit,
            repository.CurrentPath,
            repository.HistoricalDocuments,
            repository.CurrentDocuments);
        var analysis = MigrationAnalyzer.Analyze(input);
        var migration = Issue5MigrationPlanner.Plan(input, analysis.Report);
        using var report = JsonDocument.Parse(
            Issue5MigrationReportWriter.Serialize(input, analysis.Report, migration));
        var identities = report.RootElement.GetProperty("diagnosticIdentities");

        Assert.Equal(382, identities.GetProperty("historicalOccurrences").GetInt32());
        Assert.Equal(298, identities.GetProperty("historicalErrorOccurrences").GetInt32());
        Assert.Equal(84, identities.GetProperty("historicalWarningOccurrences").GetInt32());
        Assert.Equal(381, identities.GetProperty("restoredOccurrences").GetInt32());
        Assert.Equal(297, identities.GetProperty("restoredErrorOccurrences").GetInt32());
        Assert.Equal(84, identities.GetProperty("restoredWarningOccurrences").GetInt32());
        Assert.Equal(1, identities.GetProperty("obsoleteOccurrences").GetInt32());
        Assert.Equal(152, identities.GetProperty("distinctIds").GetInt32());
        Assert.Equal(56, identities.GetProperty("documents").GetInt32());
        Assert.Equal(174, identities.GetProperty("blocks").GetInt32());
        Assert.Equal(30, identities.GetProperty("multipleDistinctIdDocuments").GetInt32());
        Assert.Equal(51, identities.GetProperty("multipleDistinctIdBlocks").GetInt32());
        Assert.Equal(35, identities.GetProperty("sameKindMergeGroups").GetInt32());
        Assert.Equal(31, identities.GetProperty("multipleIdMergeGroups").GetInt32());
        Assert.Equal(13, identities.GetProperty("multipleIdMergeDocuments").GetInt32());
        Assert.Equal(18, identities.GetProperty("multipleIdMergeBlocks").GetInt32());
        Assert.Equal(7, identities.GetProperty("rawTableOccurrences").GetInt32());
        Assert.Equal(174, identities.GetProperty("metadataBlocks").GetInt32());
    }

    private static string FindRepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
             directory is not null;
             directory = directory.Parent)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git"))
                || File.Exists(Path.Combine(directory.FullName, ".git")))
            {
                return directory.FullName;
            }
        }

        throw new DirectoryNotFoundException("Repository root was not found.");
    }
}
