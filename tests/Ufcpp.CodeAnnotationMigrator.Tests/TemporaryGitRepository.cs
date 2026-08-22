using System.Diagnostics;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

internal sealed class TemporaryGitRepository : IDisposable
{
    private readonly List<string> _linkedWorktrees = [];

    public TemporaryGitRepository()
    {
        Root = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-tests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Root);
        Run("init", "--quiet");
        Run("config", "user.name", "Test User");
        Run("config", "user.email", "test@example.invalid");
    }

    public string Root { get; }

    public void Write(string relativePath, string content)
    {
        var path = Path.Combine(Root, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content, new UTF8Encoding(false));
    }

    public string Commit(string message)
    {
        Run("add", "--all");
        Run("commit", "--quiet", "-m", message);
        return Run("rev-parse", "HEAD").Trim();
    }

    public string Status() => Run("status", "--porcelain=v1");

    public void AssumeUnchanged(string relativePath) =>
        Run("update-index", "--assume-unchanged", "--", relativePath);

    public void ReplaceObject(string objectToReplace, string replacementObject) =>
        Run("replace", objectToReplace, replacementObject);

    public string CreateLinkedWorktree()
    {
        var path = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-linked-worktrees",
            Guid.NewGuid().ToString("N"));
        AddLinkedWorktree(path);
        return path;
    }

    public void AddLinkedWorktree(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        Run("worktree", "add", "--quiet", "--detach", path, "HEAD");
        _linkedWorktrees.Add(path);
    }

    public string GitDirectory() => Run("rev-parse", "--absolute-git-dir").Trim();

    public string CommonGitDirectory() =>
        Run("rev-parse", "--path-format=absolute", "--git-common-dir").Trim();

    public void Dispose()
    {
        foreach (var worktree in _linkedWorktrees)
        {
            Run("worktree", "remove", "--force", worktree);
        }

        if (Directory.Exists(Root))
        {
            foreach (var file in Directory.EnumerateFiles(
                         Root,
                         "*",
                         SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            Directory.Delete(Root, true);
        }
    }

    private string Run(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            WorkingDirectory = Root,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start Git.");
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"git {string.Join(' ', arguments)} failed: {error}");
        }

        return output;
    }
}
