using System.Globalization;
using System.Text;
using System.Xml.Linq;
using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class RssWriterTests
{
    [Fact]
    public void Write_OffsetlessTimestamp_InterpretsItAsUtc()
    {
        using var tempDirectory = new TempDirectory();
        var page = CreatePage(
            "Offset-less timestamp",
            "/blog/offset-less/",
            "2025-07-20T12:34:56");

        RssWriter.Write([page], tempDirectory.Path);

        var document = XDocument.Load(Path.Combine(tempDirectory.Path, "rssfeed.xml"));
        var item = Assert.Single(document.Descendants("item"));
        var actual = (
            Title: item.Element("title")?.Value,
            Link: item.Element("link")?.Value,
            Guid: item.Element("guid")?.Value,
            PubDate: item.Element("pubDate")?.Value);

        Assert.Equal(
            (
                Title: "Offset-less timestamp",
                Link: "https://ufcpp.net/blog/offset-less/",
                Guid: "https://ufcpp.net/blog/offset-less/",
                PubDate: "Sun, 20 Jul 2025 12:34:56 GMT"),
            actual);
    }

    [Fact]
    public void Write_OrdersAndLimitsBlogItems_WithCultureIndependentBytes()
    {
        using var tempDirectory = new TempDirectory();
        var pages = new[]
        {
            CreatePage("Oldest", "/blog/oldest/", "2025-07-19T08:00:00Z", ["old"]),
            CreatePage(
                "Newest",
                "/blog/newest/",
                "2025-07-21T10:00:00Z",
                ["release", "csharp"]),
            CreatePage("Middle", "/blog/middle/", "2025-07-20T09:00:00Z", ["middle"]),
            CreatePage(
                "Not a blog entry",
                "/article/newer/",
                "2099-01-01T00:00:00Z",
                contentType: "Article"),
        };
        var enUsDirectory = Path.Combine(tempDirectory.Path, "en-US");
        var frFrDirectory = Path.Combine(tempDirectory.Path, "fr-FR");

        var enUsBytes = WriteUnderCulture(pages, enUsDirectory, "en-US", maxEntries: 2);
        var frFrBytes = WriteUnderCulture(pages, frFrDirectory, "fr-FR", maxEntries: 2);

        Assert.Equal(enUsBytes, frFrBytes);

        var document = XDocument.Parse(Encoding.UTF8.GetString(enUsBytes));
        var actualItems = document
            .Descendants("item")
            .Select(item => (
                Title: item.Element("title")?.Value,
                Link: item.Element("link")?.Value,
                Guid: item.Element("guid")?.Value,
                PubDate: item.Element("pubDate")?.Value,
                Categories: string.Join(
                    ",",
                    item.Elements("category").Select(category => category.Value))))
            .ToArray();

        Assert.Equal(
            [
                (
                    Title: "Newest",
                    Link: "https://ufcpp.net/blog/newest/",
                    Guid: "https://ufcpp.net/blog/newest/",
                    PubDate: "Mon, 21 Jul 2025 10:00:00 GMT",
                    Categories: "release,csharp"),
                (
                    Title: "Middle",
                    Link: "https://ufcpp.net/blog/middle/",
                    Guid: "https://ufcpp.net/blog/middle/",
                    PubDate: "Sun, 20 Jul 2025 09:00:00 GMT",
                    Categories: "middle"),
            ],
            actualItems);
    }

    private static byte[] WriteUnderCulture(
        IReadOnlyList<ContentPage> pages,
        string outputDirectory,
        string cultureName,
        int maxEntries)
    {
        Directory.CreateDirectory(outputDirectory);
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);
            RssWriter.Write(pages, outputDirectory, maxEntries);
            return File.ReadAllBytes(Path.Combine(outputDirectory, "rssfeed.xml"));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    private static ContentPage CreatePage(
        string title,
        string canonicalPath,
        string publishedAt,
        IReadOnlyList<string>? tags = null,
        string contentType = "BlogEntry") =>
        new()
        {
            FrontMatter = new FrontMatter
            {
                Title = title,
                SourceUrl = "https://ufcpp.net" + canonicalPath,
                ContentType = contentType,
                PublishedAt = publishedAt,
                UpdatedAt = publishedAt,
                Tags = tags?.ToList() ?? [],
            },
            RelativePath = title + ".md",
            MarkdownBody = "",
            CanonicalPath = canonicalPath,
            OutputPath = OutputPathResolver.Resolve(canonicalPath),
        };
}
