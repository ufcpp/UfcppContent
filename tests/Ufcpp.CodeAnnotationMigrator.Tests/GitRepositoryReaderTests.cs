namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class GitRepositoryReaderTests
{
    [Fact]
    public async Task LoadAsync_ReadsHistoricalCommitAndCurrentTreeByExactPath()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/nested/a.md", "<pre>historical</pre>");
        repository.Write("content/ignored.txt", "ignored");
        var historicalCommit = repository.Commit("historical");
        repository.Write("content/nested/a.md", "```text\nhistorical\n```");
        repository.Write("content/b.md", "```text\nnew\n```");
        repository.Commit("current");

        var content = await GitRepositoryReader.LoadAsync(
            repository.Root,
            historicalCommit,
            "content",
            "content");

        Assert.Equal(Path.GetFullPath(repository.Root), content.RepositoryRoot);
        Assert.Equal(historicalCommit, content.ResolvedSourceCommit);
        Assert.Equal(
            ["nested/a.md"],
            content.HistoricalDocuments.Keys);
        Assert.Equal(
            ["b.md", "nested/a.md"],
            content.CurrentDocuments.Keys);
        Assert.Equal(
            "<pre>historical</pre>",
            content.HistoricalDocuments["nested/a.md"]);
    }

    [Fact]
    public async Task LoadAsync_RejectsMissingSourceCommit()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "current");
        repository.Commit("current");

        var exception = await Assert.ThrowsAsync<MigrationInputException>(
            () => GitRepositoryReader.LoadAsync(
                repository.Root,
                "0000000000000000000000000000000000000000",
                "content",
                "content"));

        Assert.Contains("commit", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LoadAsync_RejectsMissingHistoricalSourcePath()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "current");
        var commit = repository.Commit("current");

        var exception = await Assert.ThrowsAsync<MigrationInputException>(
            () => GitRepositoryReader.LoadAsync(
                repository.Root,
                commit,
                "missing",
                "content"));

        Assert.Contains("source path", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LoadAsync_RejectsDirtyCurrentContent()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "committed");
        var commit = repository.Commit("current");
        repository.Write("content/untracked.md", "dirty");

        var exception = await Assert.ThrowsAsync<MigrationInputException>(
            () => GitRepositoryReader.LoadAsync(
                repository.Root,
                commit,
                "content",
                "content"));

        Assert.Contains("dirty", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("untracked.md", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LoadAsync_RejectsInvalidRepositoryAndTraversalPaths()
    {
        var directory = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-tests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            await Assert.ThrowsAsync<MigrationInputException>(
                () => GitRepositoryReader.LoadAsync(
                    directory,
                    "HEAD",
                    "content",
                    "content"));

            using var repository = new TemporaryGitRepository();
            repository.Write("content/a.md", "current");
            repository.Commit("current");
            await Assert.ThrowsAsync<MigrationInputException>(
                () => GitRepositoryReader.LoadAsync(
                    repository.Root,
                    "HEAD",
                    "..\\content",
                    "content"));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task LoadAsync_RejectsIgnoredMarkdownInCurrentTree()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write(".gitignore", "**/bin/\n");
        repository.Write("content/a.md", "current");
        var commit = repository.Commit("current");
        repository.Write("content/bin/ignored.md", "local only");

        var exception = await Assert.ThrowsAsync<MigrationInputException>(
            () => GitRepositoryReader.LoadAsync(
                repository.Root,
                commit,
                "content",
                "content"));

        Assert.Contains("ignored.md", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LoadAsync_RejectsTrackedMarkdownSymbolicLink()
    {
        using var repository = new TemporaryGitRepository();
        var externalDirectory = Path.Combine(
            Path.GetTempPath(),
            "ufcpp-code-annotation-linked-source",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(externalDirectory);
        var externalPath = Path.Combine(externalDirectory, "external.md");
        File.WriteAllText(externalPath, "historical");
        var linkPath = Path.Combine(repository.Root, "content", "linked.md");
        Directory.CreateDirectory(Path.GetDirectoryName(linkPath)!);
        try
        {
            try
            {
                File.CreateSymbolicLink(linkPath, externalPath);
            }
            catch (Exception setupException) when (
                setupException is UnauthorizedAccessException
                    or IOException
                    or PlatformNotSupportedException)
            {
                return;
            }

            var commit = repository.Commit("linked");
            File.WriteAllText(externalPath, "changed outside Git");

            var exception = await Assert.ThrowsAsync<MigrationInputException>(
                () => GitRepositoryReader.LoadAsync(
                    repository.Root,
                    commit,
                    "content",
                    "content"));

            Assert.Contains("symbolic", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            File.Delete(linkPath);
            if (File.Exists(externalPath))
            {
                File.Delete(externalPath);
            }

            Directory.Delete(externalDirectory);
        }
    }

    [Fact]
    public async Task LoadAsync_RejectsAssumeUnchangedMarkdown()
    {
        using var repository = new TemporaryGitRepository();
        repository.Write("content/a.md", "committed");
        var commit = repository.Commit("current");
        repository.AssumeUnchanged("content/a.md");
        repository.Write("content/a.md", "hidden change");

        var exception = await Assert.ThrowsAsync<MigrationInputException>(
            () => GitRepositoryReader.LoadAsync(
                repository.Root,
                commit,
                "content",
                "content"));

        Assert.Contains(
            "assume-unchanged",
            exception.Message,
            StringComparison.OrdinalIgnoreCase);
    }
}
