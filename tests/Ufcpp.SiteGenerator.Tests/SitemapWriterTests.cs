using System.Xml.Linq;
using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class SitemapWriterTests
{
    [Fact]
    public void Write_UsesPublicCatalogTypesAndCanonicalPathOrder()
    {
        using var tempDirectory = new TempDirectory();
        var pages = new[]
        {
            CreatePage("Sitemap", "/sitemap/", "2025-01-13"),
            CreatePage("BlogEntry", "/blog-entry/", "2025-01-05"),
            CreatePage("Chapter", "/chapter/", "2025-01-10"),
            CreatePage("Home", "/", "2025-01-01"),
            CreatePage("StudyTop", "/study/", "2025-01-09"),
            CreatePage("Article", "/article/", "2025-01-04"),
            CreatePage("Search", "/search/", "2025-01-12"),
            CreatePage("BlogMonth", "/blog-month/", "2025-01-11"),
            CreatePage("AboutMe", "/about/", "2025-01-02"),
            CreatePage("ExerciseList", "/exercise-list/", "2025-01-08"),
            CreatePage("BlogYear", "/blog-year/", "2025-01-07"),
            CreatePage("Subject", "/subject/", "2025-01-03"),
            CreatePage("BlogTop", "/blog-top/", "2025-01-06"),
        };

        SitemapWriter.Write(pages, tempDirectory.Path);

        var document = XDocument.Load(Path.Combine(tempDirectory.Path, "sitemap.xml"));
        XNamespace sitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var actual = document
            .Root!
            .Elements(sitemapNamespace + "url")
            .Select(url => (
                Location: url.Element(sitemapNamespace + "loc")?.Value,
                LastModified: url.Element(sitemapNamespace + "lastmod")?.Value))
            .ToArray();

        Assert.Equal(
            [
                (Location: "https://ufcpp.net/", LastModified: "2025-01-01"),
                (Location: "https://ufcpp.net/about/", LastModified: "2025-01-02"),
                (Location: "https://ufcpp.net/article/", LastModified: "2025-01-04"),
                (Location: "https://ufcpp.net/blog-entry/", LastModified: "2025-01-05"),
                (Location: "https://ufcpp.net/blog-top/", LastModified: "2025-01-06"),
                (Location: "https://ufcpp.net/blog-year/", LastModified: "2025-01-07"),
                (Location: "https://ufcpp.net/exercise-list/", LastModified: "2025-01-08"),
                (Location: "https://ufcpp.net/subject/", LastModified: "2025-01-03"),
            ],
            actual);
        Assert.DoesNotContain(
            "https://ufcpp.net/search/",
            actual.Select(entry => entry.Location));
        Assert.DoesNotContain(
            "https://ufcpp.net/sitemap/",
            actual.Select(entry => entry.Location));
    }

    private static ContentPage CreatePage(
        string contentType,
        string canonicalPath,
        string updatedAt) =>
        new()
        {
            FrontMatter = new FrontMatter
            {
                Title = contentType,
                SourceUrl = "https://ufcpp.net" + canonicalPath,
                ContentType = contentType,
                PublishedAt = updatedAt + "T00:00:00",
                UpdatedAt = updatedAt + "T00:00:00",
            },
            RelativePath = contentType + ".md",
            MarkdownBody = "",
            CanonicalPath = canonicalPath,
            OutputPath = OutputPathResolver.Resolve(canonicalPath),
        };
}
