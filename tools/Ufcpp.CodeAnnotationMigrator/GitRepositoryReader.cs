using System.Diagnostics;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed class MigrationInputException(string message) : Exception(message);

internal sealed record RepositoryContent(
    string RepositoryRoot,
    string ResolvedSourceCommit,
    string SourcePath,
    string CurrentPath,
    IReadOnlyDictionary<string, string> HistoricalDocuments,
    IReadOnlyDictionary<string, string> CurrentDocuments);

internal static class GitRepositoryReader
{
    public static async Task<RepositoryContent> LoadAsync(
        string repositoryRoot,
        string sourceCommit,
        string sourcePath,
        string currentPath,
        CancellationToken cancellationToken = default)
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

        var commit = await RunGitAsync(
            root,
            ["rev-parse", "--verify", "--end-of-options", $"{sourceCommit}^{{commit}}"],
            cancellationToken);
        if (commit.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Source commit '{sourceCommit}' is not available in local Git history.");
        }

        var resolvedCommit = commit.Output.Trim();
        var sourceType = await RunGitAsync(
            root,
            ["cat-file", "-t", $"{resolvedCommit}:{normalizedSourcePath}"],
            cancellationToken);
        if (sourceType.ExitCode != 0
            || !sourceType.Output.Trim().Equals("tree", StringComparison.Ordinal))
        {
            throw new MigrationInputException(
                $"Historical source path '{normalizedSourcePath}' is not a tree "
                + $"at commit '{resolvedCommit}'.");
        }

        var currentDirectory = Path.GetFullPath(
            Path.Combine(
                root,
                normalizedCurrentPath.Replace('/', Path.DirectorySeparatorChar)));
        if (!Directory.Exists(currentDirectory))
        {
            throw new MigrationInputException(
                $"Current path does not exist: '{normalizedCurrentPath}'.");
        }

        var dirty = await RunGitAsync(
            root,
            [
                "status",
                "--porcelain=v1",
                "-z",
                "--untracked-files=all",
                "--",
                normalizedCurrentPath,
            ],
            cancellationToken);
        if (dirty.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to inspect current path '{normalizedCurrentPath}': "
                + dirty.Error.Trim());
        }

        if (dirty.Output.Length != 0)
        {
            var entries = dirty.Output
                .Split('\0', StringSplitOptions.RemoveEmptyEntries)
                .Select(static entry => entry.Trim())
                .Order(StringComparer.Ordinal);
            throw new MigrationInputException(
                $"Current content tree is dirty: {string.Join(", ", entries)}.");
        }

        var historicalDocuments = await ReadHistoricalDocumentsAsync(
            root,
            resolvedCommit,
            normalizedSourcePath,
            cancellationToken);
        var currentDocuments = await ReadCurrentDocumentsAsync(
            root,
            normalizedCurrentPath,
            currentDirectory,
            cancellationToken);
        return new RepositoryContent(
            root,
            resolvedCommit,
            normalizedSourcePath,
            normalizedCurrentPath,
            historicalDocuments,
            currentDocuments);
    }

    private static async Task<IReadOnlyDictionary<string, string>>
        ReadHistoricalDocumentsAsync(
            string root,
            string commit,
            string sourcePath,
            CancellationToken cancellationToken)
    {
        var tree = await RunGitAsync(
            root,
            ["ls-tree", "-r", "-z", "--name-only", commit, "--", sourcePath],
            cancellationToken);
        if (tree.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to enumerate historical source path '{sourcePath}': "
                + tree.Error.Trim());
        }

        var prefix = sourcePath + "/";
        var repositoryPaths = tree.Output
            .Split('\0', StringSplitOptions.RemoveEmptyEntries)
            .Where(static path => path.EndsWith(
                ".md",
                StringComparison.OrdinalIgnoreCase))
            .Order(StringComparer.Ordinal)
            .ToArray();
        var documents = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var repositoryPath in repositoryPaths)
        {
            if (!repositoryPath.StartsWith(prefix, StringComparison.Ordinal))
            {
                throw new MigrationInputException(
                    $"Historical path '{repositoryPath}' escaped '{sourcePath}'.");
            }

            var blob = await RunGitAsync(
                root,
                ["cat-file", "blob", $"{commit}:{repositoryPath}"],
                cancellationToken);
            if (blob.ExitCode != 0)
            {
                throw new MigrationInputException(
                    $"Unable to read historical Markdown '{repositoryPath}': "
                    + blob.Error.Trim());
            }

            documents.Add(repositoryPath[prefix.Length..], blob.Output);
        }

        return documents;
    }

    private static async Task<IReadOnlyDictionary<string, string>>
        ReadCurrentDocumentsAsync(
            string root,
            string currentPath,
            string currentDirectory,
            CancellationToken cancellationToken)
    {
        var tracked = await RunGitAsync(
            root,
            ["ls-files", "-z", "--", currentPath],
            cancellationToken);
        if (tracked.ExitCode != 0)
        {
            throw new MigrationInputException(
                $"Unable to enumerate tracked current Markdown: "
                + tracked.Error.Trim());
        }

        var prefix = currentPath + "/";
        var trackedMarkdown = tracked.Output
            .Split('\0', StringSplitOptions.RemoveEmptyEntries)
            .Where(static path => path.EndsWith(
                ".md",
                StringComparison.OrdinalIgnoreCase))
            .Select(path => path.StartsWith(prefix, StringComparison.Ordinal)
                ? path[prefix.Length..]
                : throw new MigrationInputException(
                    $"Tracked path '{path}' escaped '{currentPath}'."))
            .ToHashSet(StringComparer.Ordinal);
        var files = Directory
            .EnumerateFiles(currentDirectory, "*", SearchOption.AllDirectories)
            .Where(static path => Path.GetExtension(path).Equals(
                ".md",
                StringComparison.OrdinalIgnoreCase))
            .Select(path => (
                Path: path,
                RelativePath: Path.GetRelativePath(currentDirectory, path)
                    .Replace('\\', '/')))
            .OrderBy(static item => item.RelativePath, StringComparer.Ordinal)
            .ToArray();
        var localOnly = files
            .Where(file => !trackedMarkdown.Contains(file.RelativePath))
            .Select(static file => file.RelativePath)
            .ToArray();
        if (localOnly.Length != 0)
        {
            throw new MigrationInputException(
                "Current content tree contains untracked or ignored Markdown: "
                + string.Join(", ", localOnly)
                + ".");
        }

        var linked = files
            .Where(static file =>
                (File.GetAttributes(file.Path) & FileAttributes.ReparsePoint) != 0)
            .Select(static file => file.RelativePath)
            .ToArray();
        if (linked.Length != 0)
        {
            throw new MigrationInputException(
                "Current Markdown must be regular files, not symbolic links: "
                + string.Join(", ", linked)
                + ".");
        }

        var documents = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var file in files)
        {
            documents.Add(
                file.RelativePath,
                await File.ReadAllTextAsync(file.Path, cancellationToken));
        }

        return documents;
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
        var startInfo = new ProcessStartInfo("git")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = new UTF8Encoding(false),
            StandardErrorEncoding = new UTF8Encoding(false),
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        startInfo.ArgumentList.Add("-C");
        startInfo.ArgumentList.Add(root);
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

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

    private sealed record GitResult(int ExitCode, string Output, string Error);
}
