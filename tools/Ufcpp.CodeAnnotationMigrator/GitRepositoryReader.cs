using System.Diagnostics;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed class MigrationInputException(string message) : Exception(message);

internal sealed record RepositoryContent(
    string RepositoryRoot,
    string ResolvedSourceCommit,
    string ResolvedCurrentCommit,
    string SourcePath,
    string CurrentPath,
    IReadOnlyDictionary<string, string> HistoricalDocuments,
    IReadOnlyDictionary<string, string> CurrentDocuments);

internal sealed record RepositoryReadHooks(
    Func<Task>? AfterInitialValidation = null);

internal static class GitRepositoryReader
{
    public static Task<RepositoryContent> LoadAsync(
        string repositoryRoot,
        string sourceCommit,
        string sourcePath,
        string currentPath,
        CancellationToken cancellationToken = default) =>
        LoadAsync(
            repositoryRoot,
            sourceCommit,
            sourcePath,
            currentPath,
            hooks: null,
            cancellationToken);

    internal static async Task<RepositoryContent> LoadAsync(
        string repositoryRoot,
        string sourceCommit,
        string sourcePath,
        string currentPath,
        RepositoryReadHooks? hooks,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(repositoryRoot)
            || !Directory.Exists(repositoryRoot))
        {
            throw new MigrationInputException(
                $"Repository root does not exist: '{repositoryRoot}'.");
        }

        var root = Path.GetFullPath(repositoryRoot);
        var normalizedSourcePath = NormalizeRelativePath(sourcePath, "source path");
        var normalizedCurrentPath = NormalizeRelativePath(currentPath, "current path");
        var topLevel = await RunGitAsync(
            root,
            ["rev-parse", "--show-toplevel"],
            cancellationToken);
        if (topLevel.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Repository root is not a Git worktree: '{root}'.");
        }

        var resolvedTopLevel = Path.GetFullPath(topLevel.Output.Trim());
        var pathComparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!string.Equals(
                resolvedTopLevel.TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar),
                root.TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar),
                pathComparison))
        {
            throw new MigrationInputException(
                $"Repository root must be the worktree root '{resolvedTopLevel}'.");
        }

        ValidateNoReparsePoints(root, normalizedCurrentPath);
        var resolvedSourceCommit = await ResolveCommitAsync(
            root,
            sourceCommit,
            "Source",
            cancellationToken);
        var resolvedCurrentCommit = await ResolveCommitAsync(
            root,
            "HEAD",
            "Current HEAD",
            cancellationToken);
        await ValidateTreeAsync(
            root,
            resolvedSourceCommit,
            normalizedSourcePath,
            "Historical source",
            cancellationToken);
        await ValidateTreeAsync(
            root,
            resolvedCurrentCommit,
            normalizedCurrentPath,
            "Current",
            cancellationToken);

        await ValidateCurrentCheckoutAsync(
            root,
            normalizedCurrentPath,
            resolvedCurrentCommit,
            changedDuringRead: false,
            cancellationToken);
        if (hooks?.AfterInitialValidation is { } afterInitialValidation)
        {
            await afterInitialValidation();
        }

        var historicalDocuments = await ReadDocumentsAtCommitAsync(
            root,
            resolvedSourceCommit,
            normalizedSourcePath,
            "historical",
            cancellationToken);
        var currentDocuments = await ReadDocumentsAtCommitAsync(
            root,
            resolvedCurrentCommit,
            normalizedCurrentPath,
            "current",
            cancellationToken);
        await ValidateCurrentCheckoutAsync(
            root,
            normalizedCurrentPath,
            resolvedCurrentCommit,
            changedDuringRead: true,
            cancellationToken);
        return new RepositoryContent(
            root,
            resolvedSourceCommit,
            resolvedCurrentCommit,
            normalizedSourcePath,
            normalizedCurrentPath,
            historicalDocuments,
            currentDocuments);
    }

    internal static ProcessStartInfo CreateGitStartInfo(
        string root,
        IReadOnlyList<string> arguments)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = new UTF8Encoding(false),
            StandardErrorEncoding = new UTF8Encoding(false),
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        startInfo.Environment["GIT_NO_REPLACE_OBJECTS"] = "1";
        startInfo.Environment["GIT_OPTIONAL_LOCKS"] = "0";
        startInfo.Environment["GIT_NO_LAZY_FETCH"] = "1";
        startInfo.Environment["GIT_TERMINAL_PROMPT"] = "0";
        foreach (var configuration in new[]
                 {
                     "core.fsmonitor=false",
                     "maintenance.auto=false",
                     "fetch.writeCommitGraph=false",
                 })
        {
            startInfo.ArgumentList.Add("-c");
            startInfo.ArgumentList.Add(configuration);
        }

        startInfo.ArgumentList.Add("-C");
        startInfo.ArgumentList.Add(root);
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        return startInfo;
    }

    private static async Task<string> ResolveCommitAsync(
        string root,
        string revision,
        string description,
        CancellationToken cancellationToken)
    {
        var result = await RunGitAsync(
            root,
            ["rev-parse", "--verify", "--end-of-options", $"{revision}^{{commit}}"],
            cancellationToken);
        if (result.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"{description} commit '{revision}' is not available in local "
                + "Git history without replacement objects or lazy fetching.");
        }

        return result.Output.Trim();
    }

    private static async Task ValidateTreeAsync(
        string root,
        string commit,
        string path,
        string description,
        CancellationToken cancellationToken)
    {
        var result = await RunGitAsync(
            root,
            ["cat-file", "-t", $"{commit}:{path}"],
            cancellationToken);
        if (result.ExitCode != 0
            || !result.Output.Trim().Equals("tree", StringComparison.Ordinal))
        {
            throw new MigrationInputException(
                $"{description} path '{path}' is not a tree at commit '{commit}', "
                + "or its objects are unavailable locally.");
        }
    }

    private static async Task ValidateCurrentCheckoutAsync(
        string root,
        string currentPath,
        string expectedCommit,
        bool changedDuringRead,
        CancellationToken cancellationToken)
    {
        var liveCommit = await ResolveCommitAsync(
            root,
            "HEAD",
            "Current HEAD",
            cancellationToken);
        if (!liveCommit.Equals(expectedCommit, StringComparison.Ordinal))
        {
            throw new MigrationInputException(
                "Current HEAD changed during immutable snapshot capture.");
        }

        var dirty = await RunGitAsync(
            root,
            [
                "status",
                "--porcelain=v1",
                "-z",
                "--untracked-files=all",
                "--",
                currentPath,
            ],
            cancellationToken);
        if (dirty.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to inspect current path '{currentPath}': "
                + dirty.Error.Trim());
        }

        if (dirty.Output.Length != 0)
        {
            var entries = dirty.Output
                .Split('\0', StringSplitOptions.RemoveEmptyEntries)
                .Select(static entry => entry.Trim())
                .Order(StringComparer.Ordinal);
            var prefix = changedDuringRead
                ? "Current content tree changed during immutable snapshot capture"
                : "Current content tree is dirty";
            throw new MigrationInputException(
                $"{prefix}: {string.Join(", ", entries)}.");
        }

        await RejectUnsafeIndexFlagsAsync(root, currentPath, cancellationToken);
        await RejectIgnoredMarkdownAsync(root, currentPath, cancellationToken);
        ValidateNoReparsePoints(root, currentPath);
    }

    private static async Task RejectUnsafeIndexFlagsAsync(
        string root,
        string currentPath,
        CancellationToken cancellationToken)
    {
        var flagged = await RunGitAsync(
            root,
            ["ls-files", "-v", "-z", "--", currentPath],
            cancellationToken);
        if (flagged.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to inspect current Git index flags: "
                + flagged.Error.Trim());
        }

        var unsafePaths = flagged.Output
            .Split('\0', StringSplitOptions.RemoveEmptyEntries)
            .Where(static entry =>
                entry.Length > 2
                && (char.IsLower(entry[0]) || entry[0] == 'S')
                && entry[2..].EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            .Select(static entry => entry[2..])
            .Order(StringComparer.Ordinal)
            .ToArray();
        if (unsafePaths.Length != 0)
        {
            throw new MigrationInputException(
                "Current Markdown cannot use assume-unchanged or skip-worktree "
                + $"index flags: {string.Join(", ", unsafePaths)}.");
        }
    }

    private static async Task RejectIgnoredMarkdownAsync(
        string root,
        string currentPath,
        CancellationToken cancellationToken)
    {
        var ignored = await RunGitAsync(
            root,
            [
                "ls-files",
                "--others",
                "--ignored",
                "--exclude-standard",
                "-z",
                "--",
                currentPath,
            ],
            cancellationToken);
        if (ignored.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to inspect ignored current Markdown: "
                + ignored.Error.Trim());
        }

        var ignoredMarkdown = ignored.Output
            .Split('\0', StringSplitOptions.RemoveEmptyEntries)
            .Where(static path => path.EndsWith(
                ".md",
                StringComparison.OrdinalIgnoreCase))
            .Order(StringComparer.Ordinal)
            .ToArray();
        if (ignoredMarkdown.Length != 0)
        {
            throw new MigrationInputException(
                "Current content tree contains ignored Markdown: "
                + string.Join(", ", ignoredMarkdown)
                + ".");
        }
    }

    private static async Task<IReadOnlyDictionary<string, string>>
        ReadDocumentsAtCommitAsync(
            string root,
            string commit,
            string treePath,
            string description,
            CancellationToken cancellationToken)
    {
        var tree = await RunGitAsync(
            root,
            ["ls-tree", "-r", "-z", commit, "--", treePath],
            cancellationToken);
        if (tree.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to enumerate {description} path '{treePath}' at "
                + $"'{commit}' without fetching: {tree.Error.Trim()}");
        }

        var prefix = treePath + "/";
        var entries = tree.Output
            .Split('\0', StringSplitOptions.RemoveEmptyEntries)
            .Select(ParseTreeEntry)
            .Where(static entry => entry.Path.EndsWith(
                ".md",
                StringComparison.OrdinalIgnoreCase))
            .OrderBy(static entry => entry.Path, StringComparer.Ordinal)
            .ToArray();
        var documents = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var entry in entries)
        {
            if (!entry.Path.StartsWith(prefix, StringComparison.Ordinal))
            {
                throw new MigrationInputException(
                    $"{description} path '{entry.Path}' escaped '{treePath}'.");
            }

            if (entry.Type != "blob" || entry.Mode is not ("100644" or "100755"))
            {
                throw new MigrationInputException(
                    $"{description} Markdown '{entry.Path}' must be a regular "
                    + $"Git blob, not mode '{entry.Mode}' type '{entry.Type}'.");
            }

            var blob = await RunGitAsync(
                root,
                ["cat-file", "blob", entry.ObjectId],
                cancellationToken);
            if (blob.ExitCode != 0)
            {
                throw new MigrationInputException(
                    $"Unable to read {description} Markdown '{entry.Path}' "
                    + $"from local object '{entry.ObjectId}': {blob.Error.Trim()}");
            }

            documents.Add(entry.Path[prefix.Length..], blob.Output);
        }

        return documents;
    }

    private static GitTreeEntry ParseTreeEntry(string value)
    {
        var tab = value.IndexOf('\t');
        if (tab < 0)
        {
            throw new MigrationInputException(
                $"Git returned a malformed tree entry: '{value}'.");
        }

        var metadata = value[..tab].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (metadata.Length != 3)
        {
            throw new MigrationInputException(
                $"Git returned malformed tree metadata: '{value[..tab]}'.");
        }

        return new GitTreeEntry(metadata[0], metadata[1], metadata[2], value[(tab + 1)..]);
    }

    private static void ValidateNoReparsePoints(string root, string relativePath)
    {
        var current = root;
        foreach (var segment in relativePath.Split('/').Prepend(string.Empty))
        {
            if (segment.Length != 0)
            {
                current = Path.Combine(current, segment);
            }

            if (!Directory.Exists(current))
            {
                throw new MigrationInputException(
                    $"Current path component does not exist: '{current}'.");
            }

            if ((File.GetAttributes(current) & FileAttributes.ReparsePoint) != 0)
            {
                throw new MigrationInputException(
                    $"Current path cannot traverse a symbolic link or junction: "
                    + $"'{current}'.");
            }
        }
    }

    private static string NormalizeRelativePath(string path, string description)
    {
        if (string.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path))
        {
            throw new MigrationInputException(
                $"The {description} must be a non-empty relative path.");
        }

        var normalized = path.Replace('\\', '/').TrimEnd('/');
        var segments = normalized.Split('/', StringSplitOptions.None);
        if (segments.Any(static segment =>
                segment.Length == 0 || segment is "." or ".."))
        {
            throw new MigrationInputException(
                $"The {description} cannot contain empty, '.' or '..' segments.");
        }

        return normalized;
    }

    private static async Task<GitResult> RunGitAsync(
        string root,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = CreateGitStartInfo(root, arguments);
        try
        {
            using var process = Process.Start(startInfo)
                ?? throw new MigrationInputException("Unable to start Git.");
            var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);
            await process.WaitForExitAsync(cancellationToken);
            return new GitResult(
                process.ExitCode,
                await outputTask,
                await errorTask);
        }
        catch (System.ComponentModel.Win32Exception exception)
        {
            throw new MigrationInputException(
                $"Unable to run Git: {exception.Message}");
        }
    }

    private sealed record GitTreeEntry(
        string Mode,
        string Type,
        string ObjectId,
        string Path);

    private sealed record GitResult(int ExitCode, string Output, string Error);
}
