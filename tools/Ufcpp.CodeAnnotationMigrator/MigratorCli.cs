namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record MigratorCliOptions(
    string RepositoryRoot,
    string SourceCommit,
    string SourcePath,
    string CurrentPath,
    string ReportPath);

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
                case "--apply":
                    throw new MigrationInputException(
                        "--apply is unavailable; PR 1 supports dry run only.");
                default:
                    throw new MigrationInputException(
                        $"Unknown option or positional argument '{argument}'.");
            }
        }

        return new MigratorCliOptions(
            repositoryRoot,
            sourceCommit,
            sourcePath,
            currentPath,
            reportPath);
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
                workingDirectory,
                repository);
            var outcome = MigrationAnalyzer.Analyze(
                new MigrationAnalysisInput(
                    repository.ResolvedSourceCommit,
                    repository.SourcePath,
                    repository.CurrentPath,
                    repository.HistoricalDocuments,
                    repository.CurrentDocuments));
            var bytes = MigrationReportWriter.Serialize(outcome.Report);
            if (reportPath is null)
            {
                await standardOutput.WriteAsync(bytes, cancellationToken);
                await standardOutput.FlushAsync(cancellationToken);
            }
            else
            {
                await WriteReportFileAsync(reportPath, bytes, cancellationToken);
            }

            return outcome.ExitCode;
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
        string workingDirectory,
        RepositoryContent repository)
    {
        if (value == "-")
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new MigrationInputException("Report path cannot be empty.");
        }

        if (OperatingSystem.IsWindows()
            && (value.StartsWith(@"\\?\", StringComparison.Ordinal)
                || value.StartsWith(@"\\.\", StringComparison.Ordinal)))
        {
            throw new MigrationInputException(
                "Report path cannot use a Windows device-path alias.");
        }

        var reportPath = Path.GetFullPath(value, workingDirectory);
        var parent = Path.GetDirectoryName(reportPath);
        if (parent is null || !Directory.Exists(parent))
        {
            throw new MigrationInputException(
                $"Report directory does not exist: '{parent}'.");
        }

        if (Directory.Exists(reportPath))
        {
            throw new MigrationInputException(
                $"Report path names a directory: '{reportPath}'.");
        }

        if (IsWithin(reportPath, repository.RepositoryRoot)
            || FileSystemPathSafety.IsDirectoryWithin(
                parent,
                repository.RepositoryRoot))
        {
            throw new MigrationInputException(
                "Report path must be outside the repository worktree.");
        }

        RejectReparsePoints(reportPath);
        return reportPath;
    }

    private static async Task WriteReportFileAsync(
        string reportPath,
        byte[] bytes,
        CancellationToken cancellationToken)
    {
        var parent = Path.GetDirectoryName(reportPath)
            ?? throw new MigrationInputException("Report path has no parent directory.");
        var temporaryPath = Path.Combine(
            parent,
            $".{Path.GetFileName(reportPath)}.{Guid.NewGuid():N}.tmp");
        try
        {
            await File.WriteAllBytesAsync(temporaryPath, bytes, cancellationToken);
            RejectReparsePoints(reportPath);
            File.Move(temporaryPath, reportPath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private static void RejectReparsePoints(string path)
    {
        var current = File.Exists(path) || Directory.Exists(path)
            ? path
            : Path.GetDirectoryName(path);
        while (!string.IsNullOrEmpty(current))
        {
            if ((File.GetAttributes(current) & FileAttributes.ReparsePoint) != 0)
            {
                throw new MigrationInputException(
                    $"Report path cannot traverse a symbolic link or junction: "
                    + $"'{current}'.");
            }

            var parent = Path.GetDirectoryName(
                current.TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar));
            if (string.Equals(parent, current, StringComparison.Ordinal))
            {
                break;
            }

            current = parent;
        }
    }

    private static bool IsWithin(string path, string directory)
    {
        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        var normalizedDirectory = directory.TrimEnd(
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar);
        return path.Equals(normalizedDirectory, comparison)
            || path.StartsWith(
                normalizedDirectory + Path.DirectorySeparatorChar,
                comparison)
            || Path.AltDirectorySeparatorChar != Path.DirectorySeparatorChar
            && path.StartsWith(
                normalizedDirectory + Path.AltDirectorySeparatorChar,
                comparison);
    }
}
