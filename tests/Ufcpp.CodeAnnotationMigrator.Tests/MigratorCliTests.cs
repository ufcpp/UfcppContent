using System.Text;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class MigratorCliTests
{
    [Fact]
    public void Parse_UsesPinnedDryRunDefaultsAndExplicitOverrides()
    {
        var defaults = MigratorCliOptionsParser.Parse([], @"C:\repository");
        Assert.Equal(@"C:\repository", defaults.RepositoryRoot);
        Assert.Equal(
            "eacf0d470a684771524fb04f710951d38a60cc74",
            defaults.SourceCommit);
        Assert.Equal("content", defaults.SourcePath);
        Assert.Equal("content", defaults.CurrentPath);
        Assert.Equal("-", defaults.ReportPath);
        Assert.Null(defaults.Issue);
        Assert.Equal(MigratorOutputFormat.Report, defaults.Format);

        var explicitOptions = MigratorCliOptionsParser.Parse(
            [
                "--dry-run",
                "--repo-root",
                @"C:\other",
                "--source-commit",
                "HEAD~1",
                "--source-path",
                "archive",
                "--current-path",
                "docs",
                "--report",
                "report.json",
                "--issue",
                "4",
                "--format",
                "patch",
            ],
            @"C:\repository");
        Assert.Equal(@"C:\other", explicitOptions.RepositoryRoot);
        Assert.Equal("HEAD~1", explicitOptions.SourceCommit);
        Assert.Equal("archive", explicitOptions.SourcePath);
        Assert.Equal("docs", explicitOptions.CurrentPath);
        Assert.Equal("report.json", explicitOptions.ReportPath);
        Assert.Equal(4, explicitOptions.Issue);
        Assert.Equal(MigratorOutputFormat.Patch, explicitOptions.Format);

        var issue5 = MigratorCliOptionsParser.Parse(
            ["--issue", "5", "--format", "patch"],
            @"C:\repository");
        Assert.Equal(5, issue5.Issue);
        Assert.Equal(MigratorOutputFormat.Patch, issue5.Format);
    }

    [Theory]
    [InlineData("--apply")]
    [InlineData("--unknown")]
    [InlineData("--source-commit")]
    public void Parse_RejectsApplyUnknownAndMissingValues(string argument)
    {
        Assert.Throws<MigrationInputException>(
            () => MigratorCliOptionsParser.Parse([argument], Environment.CurrentDirectory));
    }

    [Fact]
    public async Task RunAsync_WritesDryRunReportWithoutMutatingContent()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write(
            "content/a.md",
            "<pre title=\"sample\"><code><em>value</em></code></pre>");
        var historicalCommit = repository.Commit("historical");
        repository.Write(
            "content/a.md",
            "```text\nvalue\n```");
        repository.Commit("current");
        var before = File.ReadAllBytes(Path.Combine(repository.Root, "content", "a.md"));
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                historicalCommit,
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(0, exitCode);
        Assert.Equal(string.Empty, error.ToString());
        Assert.Contains(
            "\"schemaVersion\": 3",
            Encoding.UTF8.GetString(output.ToArray()),
            StringComparison.Ordinal);
        Assert.Equal(
            before,
            File.ReadAllBytes(Path.Combine(repository.Root, "content", "a.md")));
        Assert.Equal(string.Empty, repository.Status());
    }

    [Fact]
    public async Task RunAsync_ReturnsInputErrorForMissingCommit()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "current");
        repository.Commit("current");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                "missing",
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.Empty(output.ToArray());
        Assert.Contains("commit", error.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RunAsync_Issue4PatchIsDeterministicAndDoesNotMutateContent()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write(
            "content/a.md",
            "<pre title=\"sample\"><code><em>token</em> + token</code></pre>");
        var historicalCommit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\ntoken + token\n```\n");
        repository.Commit("current");
        var before = File.ReadAllBytes(Path.Combine(repository.Root, "content", "a.md"));

        var first = await RunIssue4PatchAsync(repository, historicalCommit);
        var second = await RunIssue4PatchAsync(repository, historicalCommit);

        Assert.Equal(0, first.ExitCode);
        Assert.Equal(string.Empty, first.Error);
        Assert.Equal(first.Output, second.Output);
        var patch = Encoding.UTF8.GetString(first.Output);
        Assert.Contains("highlight-ranges=", patch);
        Assert.Contains("title=\"sample\"", patch);
        Assert.Equal(before, File.ReadAllBytes(Path.Combine(
            repository.Root,
            "content",
            "a.md")));
        Assert.Equal(string.Empty, repository.Status());
    }

    [Fact]
    public async Task RunAsync_Issue4PatchFailsClosedWithoutPartialOutput()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write(
            "content/a.md",
            "<pre title=\"lost\"><code>historical</code></pre>");
        var historicalCommit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\ncurrent\n```\n");
        repository.Commit("current");

        var result = await RunIssue4PatchAsync(repository, historicalCommit);

        Assert.Equal(3, result.ExitCode);
        Assert.Empty(result.Output);
        Assert.Contains("blocked", result.Error, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(string.Empty, repository.Status());
    }

    [Fact]
    public async Task RunAsync_Issue5PatchIsDeterministicAndDoesNotMutateContent()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write(
            "content/a.md",
            "<pre><code>token + <span class=\"error\">token</span></code></pre>");
        var historicalCommit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\ntoken + token\n```\n");
        repository.Commit("current");
        var before = File.ReadAllBytes(Path.Combine(repository.Root, "content", "a.md"));

        var first = await RunIssue5Async(
            repository,
            historicalCommit,
            MigratorOutputFormat.Patch);
        var second = await RunIssue5Async(
            repository,
            historicalCommit,
            MigratorOutputFormat.Patch);

        Assert.Equal(0, first.ExitCode);
        Assert.Equal(string.Empty, first.Error);
        Assert.Equal(first.Output, second.Output);
        Assert.Contains(
            "error-ranges=",
            Encoding.UTF8.GetString(first.Output),
            StringComparison.Ordinal);
        Assert.Equal(before, File.ReadAllBytes(Path.Combine(
            repository.Root,
            "content",
            "a.md")));
        Assert.Equal(string.Empty, repository.Status());
    }

    [Fact]
    public async Task RunAsync_Issue5ReportIncludesAcceptanceAndRepresentationCounts()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write(
            "content/a.md",
            "<pre><code>alpha "
            + "<span class=\"warning\" title=\"CS0219\">beta</span>"
            + "</code></pre>");
        var historicalCommit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\nalpha beta\n```\n");
        repository.Commit("current");

        var result = await RunIssue5Async(
            repository,
            historicalCommit,
            MigratorOutputFormat.Report);
        var report = Encoding.UTF8.GetString(result.Output);

        Assert.Equal(0, result.ExitCode);
        Assert.Equal(string.Empty, result.Error);
        Assert.Contains("\"mode\": \"issue5-plan\"", report, StringComparison.Ordinal);
        Assert.Contains("\"restoredWarningBlocks\": 1", report, StringComparison.Ordinal);
        Assert.Contains("\"warningTextBlocks\": 1", report, StringComparison.Ordinal);
        Assert.Contains("\"historicalOccurrences\": 1", report, StringComparison.Ordinal);
        Assert.Contains("\"restoredOccurrences\": 1", report, StringComparison.Ordinal);
        Assert.Contains("\"restoredWarningOccurrences\": 1", report, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RunAsync_RejectsReportInsideCurrentContent()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\nvalue\n```");
        repository.Commit("current");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                commit,
                "--report",
                "content/report.json",
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.False(File.Exists(Path.Combine(
            repository.Root,
            "content",
            "report.json")));
        Assert.Contains("report", error.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RunAsync_RejectsExplicitFileReportWithoutWriting()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\nvalue\n```");
        repository.Commit("current");
        var reportDirectory = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-reports",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(reportDirectory);
        var reportPath = Path.Combine(reportDirectory, "report.json");
        try
        {
            await using var output = new MemoryStream();
            using var error = new StringWriter();
            var exitCode = await MigratorCli.RunAsync(
                [
                    "--repo-root",
                    repository.Root,
                    "--source-commit",
                    commit,
                    "--report",
                    reportPath,
                ],
                output,
                error,
                repository.Root);

            Assert.Equal(2, exitCode);
            Assert.False(File.Exists(reportPath));
            Assert.Empty(output.ToArray());
            Assert.Contains(
                "standard output",
                error.ToString(),
                StringComparison.OrdinalIgnoreCase);
            Assert.Equal(string.Empty, repository.Status());
        }
        finally
        {
            Directory.Delete(reportDirectory);
        }
    }

    [Fact]
    public async Task RunAsync_RejectsReportPathThroughDirectoryLink()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        var reportDirectory = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-links",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(reportDirectory);
        var linkPath = Path.Combine(reportDirectory, "linked-content");
        try
        {
            Directory.CreateSymbolicLink(
                linkPath,
                Path.Combine(repository.Root, "content"));

            await using var output = new MemoryStream();
            using var error = new StringWriter();
            var exitCode = await MigratorCli.RunAsync(
                [
                    "--repo-root",
                    repository.Root,
                    "--source-commit",
                    commit,
                    "--report",
                    Path.Combine(linkPath, "report.json"),
                ],
                output,
                error,
                repository.Root);

            Assert.Equal(2, exitCode);
            Assert.False(File.Exists(Path.Combine(
                repository.Root,
                "content",
                "report.json")));
            Assert.Contains(
                "standard output",
                error.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (Directory.Exists(linkPath))
            {
                Directory.Delete(linkPath);
            }

            Directory.Delete(reportDirectory);
        }
    }

    [Fact]
    public async Task RunAsync_RejectsExtendedPathAliasIntoContent()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        var reportPath = @"\\?\"
            + Path.Combine(repository.Root, "content", "report.json");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                commit,
                "--report",
                reportPath,
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.False(File.Exists(Path.Combine(
            repository.Root,
            "content",
            "report.json")));
    }

    [Fact]
    public async Task RunAsync_RejectsReportInsideCommonGitDirectory()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        var linkedWorktree = repository.CreateLinkedWorktree();
        var reportPath = Path.Combine(repository.CommonGitDirectory(), "report.json");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                linkedWorktree,
                "--source-commit",
                commit,
                "--report",
                reportPath,
            ],
            output,
            error,
            linkedWorktree);

        Assert.Equal(2, exitCode);
        Assert.False(File.Exists(reportPath));
        Assert.Contains(
            "standard output",
            error.ToString(),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RunAsync_RejectsReportInsideAssociatedWorktree()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        var linkedWorktree = repository.CreateLinkedWorktree();
        var reportPath = Path.Combine(linkedWorktree, "report.json");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                commit,
                "--report",
                reportPath,
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.False(File.Exists(reportPath));
        Assert.Contains(
            "standard output",
            error.ToString(),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RunAsync_AtomicReplaceDoesNotWriteThroughHardLink()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("historical");
        repository.Write("content/a.md", "```text\nvalue\n```");
        repository.Commit("current");
        var contentPath = Path.Combine(repository.Root, "content", "a.md");
        var originalContent = File.ReadAllBytes(contentPath);
        var reportDirectory = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-hardlink",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(reportDirectory);
        var reportPath = Path.Combine(reportDirectory, "report.json");
        TestFileSystemLinks.CreateHardLink(reportPath, contentPath);
        try
        {
            await using var output = new MemoryStream();
            using var error = new StringWriter();
            var exitCode = await MigratorCli.RunAsync(
                [
                    "--repo-root",
                    repository.Root,
                    "--source-commit",
                    commit,
                    "--report",
                    reportPath,
                ],
                output,
                error,
                repository.Root);

            Assert.Equal(2, exitCode);
            Assert.Equal(originalContent, File.ReadAllBytes(contentPath));
            Assert.Equal(originalContent, File.ReadAllBytes(reportPath));
            Assert.Equal(string.Empty, repository.Status());
        }
        finally
        {
            File.Delete(reportPath);
            Directory.Delete(reportDirectory);
        }
    }

    [Fact]
    public async Task RunAsync_RejectsOrFailsClosedForLocalhostAdminShareIntoContent()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        var root = Path.GetPathRoot(repository.Root)
            ?? throw new InvalidOperationException("Repository has no path root.");
        var relativeReportPath = Path.Combine(
            Path.GetRelativePath(root, repository.Root),
            "content",
            "report.json");
        var reportPath =
            $@"\\localhost\{char.ToUpperInvariant(root[0])}$\{relativeReportPath}";
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                commit,
                "--report",
                reportPath,
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.False(File.Exists(Path.Combine(
            repository.Root,
            "content",
            "report.json")));
        Assert.Contains("Input error", error.ToString(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("//?/C:/report.json")]
    [InlineData("//./C:/report.json")]
    [InlineData("\\??\\C:\\report.json")]
    public async Task RunAsync_RejectsAllWindowsDeviceNamespaceSpellings(
        string reportPath)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                commit,
                "--report",
                reportPath,
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.Contains(
            "standard output",
            error.ToString(),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RunAsync_NonWindowsFileReportFailsClosed()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "<pre>value</pre>");
        var commit = repository.Commit("current");
        var reportPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");
        await using var output = new MemoryStream();
        using var error = new StringWriter();

        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                commit,
                "--report",
                reportPath,
            ],
            output,
            error,
            repository.Root);

        Assert.Equal(2, exitCode);
        Assert.False(File.Exists(reportPath));
        Assert.Contains("standard output", error.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<(int ExitCode, byte[] Output, string Error)>
        RunIssue4PatchAsync(
            TemporaryGitRepository repository,
            string historicalCommit)
    {
        await using var output = new MemoryStream();
        using var error = new StringWriter();
        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                historicalCommit,
                "--issue",
                "4",
                "--format",
                "patch",
            ],
            output,
            error,
            repository.Root);
        return (exitCode, output.ToArray(), error.ToString());
    }

    private static async Task<(int ExitCode, byte[] Output, string Error)>
        RunIssue5Async(
            TemporaryGitRepository repository,
            string historicalCommit,
            MigratorOutputFormat format)
    {
        await using var output = new MemoryStream();
        using var error = new StringWriter();
        var exitCode = await MigratorCli.RunAsync(
            [
                "--repo-root",
                repository.Root,
                "--source-commit",
                historicalCommit,
                "--issue",
                "5",
                "--format",
                format == MigratorOutputFormat.Patch ? "patch" : "report",
            ],
            output,
            error,
            repository.Root);
        return (exitCode, output.ToArray(), error.ToString());
    }
}
