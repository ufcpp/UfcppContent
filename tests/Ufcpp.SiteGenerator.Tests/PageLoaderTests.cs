using System.IO;
using Ufcpp.SiteGenerator.Loading;
using Ufcpp.SiteGenerator.Rendering;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class PageLoaderTests
{
    [Fact]
    public void Load_ParsesFrontMatterCorrectly()
    {
        using var tempDir = new TempDirectory();
        var mdContent = """
            ---
            title: "C# によるプログラミング入門"
            source_url: "https://ufcpp.net/study/csharp/"
            content_type: "Subject"
            published_at: "2015-05-06T14:06:20"
            updated_at: "2015-05-14T10:42:03"
            tags: []
            umbraco_id: 1169
            parent_id: 1115
            sort_order: 0
            aliases:
              - "/csharp"
              - "/csharp/"
            ---

            # C# によるプログラミング入門

            Some content here.
            """;
        File.WriteAllText(Path.Combine(tempDir.Path, "csharp.md"), mdContent);

        var (pages, urlMap) = PageLoader.Load(tempDir.Path);

        Assert.Single(pages);
        var page = pages[0];
        Assert.Equal("C# によるプログラミング入門", page.FrontMatter.Title);
        Assert.Equal("https://ufcpp.net/study/csharp/", page.FrontMatter.SourceUrl);
        Assert.Equal("Subject", page.FrontMatter.ContentType);
        Assert.Equal("/study/csharp/", page.CanonicalPath);
        Assert.Equal("study/csharp/index.html", page.OutputPath);
        Assert.Equal(2, page.FrontMatter.Aliases.Count);
        Assert.Contains("/csharp", page.FrontMatter.Aliases);
        Assert.Contains("/csharp/", page.FrontMatter.Aliases);
        Assert.Contains("# C# によるプログラミング入門", page.MarkdownBody);
    }

    [Fact]
    public void Load_ThrowsOnOutputPathCollision()
    {
        using var tempDir = new TempDirectory();

        var md = (string slug, string url) => $"""
            ---
            title: "Page {slug}"
            source_url: "{url}"
            content_type: "Article"
            published_at: "2015-01-01T00:00:00"
            updated_at: "2015-01-01T00:00:00"
            tags: []
            umbraco_id: 1
            parent_id: -1
            sort_order: 0
            aliases: []
            ---
            Body.
            """;

        // Both have source_url /study/csharp/ → same output path
        File.WriteAllText(Path.Combine(tempDir.Path, "a.md"), md("a", "https://ufcpp.net/study/csharp/"));
        File.WriteAllText(Path.Combine(tempDir.Path, "b.md"), md("b", "https://ufcpp.net/study/csharp/"));

        Assert.Throws<InvalidDataException>(() => PageLoader.Load(tempDir.Path));
    }

    [Fact]
    public void Load_ReturnsUrlMapKeyedByAbsoluteFilePath()
    {
        using var tempDir = new TempDirectory();

        var md = """
            ---
            title: "Test"
            source_url: "https://ufcpp.net/study/csharp/"
            content_type: "Subject"
            published_at: "2015-01-01T00:00:00"
            updated_at: "2015-01-01T00:00:00"
            tags: []
            umbraco_id: 1
            parent_id: -1
            sort_order: 0
            aliases: []
            ---
            Body.
            """;
        var filePath = Path.Combine(tempDir.Path, "csharp.md");
        File.WriteAllText(filePath, md);

        var (_, urlMap) = PageLoader.Load(tempDir.Path);
        var absPath = Path.GetFullPath(filePath);

        Assert.True(urlMap.ContainsKey(absPath));
        Assert.Equal("/study/csharp/", urlMap[absPath]);
    }
}

/// <summary>Creates and auto-deletes a temporary directory.</summary>
internal sealed class TempDirectory : IDisposable
{
    public string Path { get; } = System.IO.Path.Combine(
        System.IO.Path.GetTempPath(),
        System.IO.Path.GetRandomFileName());

    public TempDirectory() => Directory.CreateDirectory(Path);

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, recursive: true);
        }
    }
}
