using Ufcpp.SiteGenerator.Loading;
using Ufcpp.SiteGenerator.Rendering;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class PageLoaderLinkRewriterIntegrationTests
{
    [Fact]
    public void Load_UrlMapPassedToLinkRewriter_ResolvesSiblingLinkWithFragment()
    {
        using var tempDir = new TempDirectory();
        var contentRoot = Path.GetFullPath(Path.Combine(tempDir.Path, "content"));
        var articleDirectory = Path.Combine(contentRoot, "study", "csharp");
        Directory.CreateDirectory(articleDirectory);

        var currentFile = Path.GetFullPath(Path.Combine(articleDirectory, "current.md"));
        var siblingFile = Path.GetFullPath(Path.Combine(articleDirectory, "sibling.md"));
        File.WriteAllText(
            currentFile,
            CreateMarkdown("Current page", "https://ufcpp.net/study/csharp/current/"));
        File.WriteAllText(
            siblingFile,
            CreateMarkdown("Sibling page", "https://ufcpp.net/study/csharp/sibling/"));

        var (pages, urlMap) = PageLoader.Load(contentRoot);
        var rewriter = new LinkRewriter(contentRoot, currentFile, urlMap);

        Assert.Equal(2, pages.Count);
        Assert.Equal("/study/csharp/sibling/", urlMap[siblingFile]);
        Assert.Equal(
            "/study/csharp/sibling/#details",
            rewriter.RewriteUrl("sibling.md#details"));
    }

    private static string CreateMarkdown(string title, string sourceUrl) => $"""
        ---
        title: "{title}"
        source_url: "{sourceUrl}"
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
}
