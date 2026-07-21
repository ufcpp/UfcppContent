using System.Text;
using System.Xml;
using Ufcpp.SiteGenerator.Models;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>Generates an XML sitemap for the site.</summary>
public static class SitemapWriter
{
    private const string SitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private static readonly HashSet<string> PublicContentTypes = new(
        [
            "Home",
            "AboutMe",
            "Subject",
            "Article",
            "BlogTop",
            "BlogYear",
            "BlogEntry",
            "ExerciseList",
        ],
        StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Writes <c>sitemap.xml</c> to the output directory containing entries for all
    /// canonical pages that have a real <c>source_url</c>.
    /// </summary>
    public static void Write(IEnumerable<ContentPage> pages, string outputDirectory)
    {
        var entries = pages
            .Where(p => PublicContentTypes.Contains(p.FrontMatter.ContentType))
            .OrderBy(p => p.CanonicalPath, StringComparer.Ordinal)
            .ToList();

        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\n",
        };

        var destFile = Path.Combine(outputDirectory, "sitemap.xml");
        using var fs = File.Open(destFile, FileMode.Create, FileAccess.Write);
        using var writer = XmlWriter.Create(fs, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("urlset", SitemapNs);

        foreach (var page in entries)
        {
            writer.WriteStartElement("url", SitemapNs);
            writer.WriteElementString("loc", SitemapNs, page.FrontMatter.SourceUrl);
            writer.WriteElementString("lastmod", SitemapNs,
                page.FrontMatter.UpdatedAt.Length >= 10
                    ? page.FrontMatter.UpdatedAt[..10]
                    : page.FrontMatter.UpdatedAt);
            writer.WriteEndElement(); // url
        }

        writer.WriteEndElement(); // urlset
        writer.WriteEndDocument();
    }
}
