using System.Diagnostics;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

internal sealed class TemporaryGitRepository : IDisposable
{
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

    public void Dispose()
    {
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
