namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record MigratorCliOptions(
    string RepositoryRoot,
    string SourceCommit,
    string SourcePath,
    string CurrentPath,
    string ReportPath,
    int? Issue,
    MigratorOutputFormat Format);

internal enum MigratorOutputFormat
{
    Report,
    Patch,
}

internal static class MigratorCliOptionsParser
{
    public const string PinnedSourceCommit =
        "eacf0d470a684771524fb04f710951d38a60cc74";

    public static MigratorCliOptions Parse(
        IReadOnlyList<string> arguments,
        string workingDirectory)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        if (string.IsNullOrWhiteSpace(workingDirectory))
        {
            throw new MigrationInputException("Working directory is required.");
        }

        var repositoryRoot = workingDirectory;
        var sourceCommit = PinnedSourceCommit;
        var sourcePath = "content";
        var currentPath = "content";
        var reportPath = "-";
        int? issue = null;
        var format = MigratorOutputFormat.Report;
        var seen = new HashSet<string>(StringComparer.Ordinal);

        for (var index = 0; index < arguments.Count; index++)
        {
            var argument = arguments[index];
            if (!seen.Add(argument))
            {
                throw new MigrationInputException(
                    $"Option '{argument}' cannot be repeated.");
            }

            switch (argument)
            {
                case "--dry-run":
                    break;
                case "--repo-root":
                    repositoryRoot = ReadValue(arguments, ref index, argument);
                    break;
                case "--source-commit":
                    sourceCommit = ReadValue(arguments, ref index, argument);
                    break;
                case "--source-path":
                    sourcePath = ReadValue(arguments, ref index, argument);
                    break;
                case "--current-path":
                    currentPath = ReadValue(arguments, ref index, argument);
                    break;
                case "--report":
                    reportPath = ReadValue(arguments, ref index, argument);
                    break;
                case "--issue":
                    var issueValue = ReadValue(arguments, ref index, argument);
                    issue = issueValue == "4"
                        ? 4
                        : throw new MigrationInputException(
                            "Only Issue #4 is supported by this migration mode.");
                    break;
                case "--format":
                    format = ReadValue(arguments, ref index, argument) switch
                    {
                        "report" => MigratorOutputFormat.Report,
                        "patch" => MigratorOutputFormat.Patch,
                        var value => throw new MigrationInputException(
                            $"Unknown output format '{value}'."),
                    };
                    break;
                case "--apply":
                    throw new MigrationInputException(
                        "--apply is unavailable; PR 1 supports dry run only.");
                default:
                    throw new MigrationInputException(
                        $"Unknown option or positional argument '{argument}'.");
            }
        }

        if (format == MigratorOutputFormat.Patch && issue != 4)
        {
            throw new MigrationInputException(
                "Patch format requires --issue 4.");
        }

        return new MigratorCliOptions(
            repositoryRoot,
            sourceCommit,
            sourcePath,
            currentPath,
            reportPath,
            issue,
            format);
    }

    private static string ReadValue(
        IReadOnlyList<string> arguments,
        ref int index,
        string option)
    {
        if (index + 1 >= arguments.Count
            || arguments[index + 1].StartsWith("--", StringComparison.Ordinal))
        {
            throw new MigrationInputException(
                $"Option '{option}' requires a value.");
        }

        return arguments[++index];
    }
}

internal static class MigratorCli
{
    public static async Task<int> RunAsync(
        IReadOnlyList<string> arguments,
        Stream standardOutput,
        TextWriter standardError,
        string workingDirectory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(standardOutput);
        ArgumentNullException.ThrowIfNull(standardError);

        try
        {
            var options = MigratorCliOptionsParser.Parse(arguments, workingDirectory);
            var repository = await GitRepositoryReader.LoadAsync(
                options.RepositoryRoot,
                options.SourceCommit,
                options.SourcePath,
                options.CurrentPath,
                cancellationToken);
            var reportPath = ResolveReportPath(
                options.ReportPath,
                workingDirectory);
            var input = new MigrationAnalysisInput(
                repository.ResolvedSourceCommit,
                repository.SourcePath,
                repository.ResolvedCurrentCommit,
                repository.CurrentPath,
                repository.HistoricalDocuments,
                repository.CurrentDocuments);
            var outcome = MigrationAnalyzer.Analyze(input);
            byte[] bytes;
            var exitCode = outcome.ExitCode;
            if (options.Issue == 4)
            {
                Issue4MigrationResult migration;
                try
                {
                    migration = Issue4MigrationPlanner.Plan(input, outcome.Report);
                }
                catch (InvalidDataException exception)
                {
                    await standardError.WriteLineAsync(
                        $"Issue #4 migration blocked: {exception.Message}");
                    return 3;
                }

                bytes = options.Format == MigratorOutputFormat.Patch
                    ? UnifiedPatchWriter.Write(
                        repository.ResolvedCurrentCommit,
                        migration.ChangedDocuments.ToDictionary(
                            item => JoinGitPath(repository.CurrentPath, item.Key),
                            item => new DocumentChange(
                                repository.CurrentDocuments[item.Key],
                                item.Value),
                            StringComparer.Ordinal))
                    : Issue4MigrationReportWriter.Serialize(
                        outcome.Report,
                        migration);
                exitCode = 0;
            }
            else
            {
                bytes = MigrationReportWriter.Serialize(outcome.Report);
            }

            if (reportPath is null)
            {
                await standardOutput.WriteAsync(bytes, cancellationToken);
                await standardOutput.FlushAsync(cancellationToken);
            }

            return exitCode;
        }
        catch (MigrationInputException exception)
        {
            await standardError.WriteLineAsync($"Input error: {exception.Message}");
            return 2;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            await standardError.WriteLineAsync(
                $"Unexpected migration failure: {exception.Message}");
            return 1;
        }
    }

    private static string? ResolveReportPath(
        string value,
        string workingDirectory)
    {
        if (value == "-")
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new MigrationInputException("Report path cannot be empty.");
        }

        _ = Path.GetFullPath(value, workingDirectory);
        throw new MigrationInputException(
            "File report output is disabled because filesystem aliases and "
            + "concurrent topology changes cannot be proven safe without writes "
            + "or locks; use --report - and capture standard output.");
    }

    private static string JoinGitPath(string root, string path) =>
        string.IsNullOrEmpty(root) || root == "."
            ? path
            : root.TrimEnd('/') + "/" + path;
}
