using System.Globalization;
using System.Text;
using System.Xml;
using Ufcpp.SiteGenerator.Models;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>Generates an RSS 2.0 feed for recent blog entries.</summary>
public static class RssWriter
{
    private const int DefaultMaxEntries = 30;

    /// <summary>
    /// Writes <c>rssfeed.xml</c> to the output directory containing the most recent
    /// blog entries ordered by publication date.
    /// </summary>
    public static void Write(IEnumerable<ContentPage> pages, string outputDirectory, int maxEntries = DefaultMaxEntries)
    {
        var entries = pages
            .Where(p => p.FrontMatter.ContentType == "BlogEntry")
            .OrderByDescending(p => ParsePublishedAt(p.FrontMatter.PublishedAt))
            .ThenBy(p => p.CanonicalPath, StringComparer.Ordinal)
            .Take(maxEntries)
            .ToList();

        const string SiteBaseUrl = "https://ufcpp.net";
        const string FeedTitle = "++C++; // 未確認飛行 C";
        const string FeedDescription = "C# を中心としたプログラミング・情報工学ブログ";

        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\n",
        };

        var destFile = Path.Combine(outputDirectory, "rssfeed.xml");
        using var fs = File.Open(destFile, FileMode.Create, FileAccess.Write);
        using var writer = XmlWriter.Create(fs, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");
        writer.WriteAttributeString("xmlns", "atom", null, "http://www.w3.org/2005/Atom");

        writer.WriteStartElement("channel");
        writer.WriteElementString("title", FeedTitle);
        writer.WriteElementString("link", SiteBaseUrl + "/blog/");
        writer.WriteElementString("description", FeedDescription);
        writer.WriteElementString("language", "ja");

        writer.WriteStartElement("atom", "link", "http://www.w3.org/2005/Atom");
        writer.WriteAttributeString("href", SiteBaseUrl + "/rssfeed.xml");
        writer.WriteAttributeString("rel", "self");
        writer.WriteAttributeString("type", "application/rss+xml");
        writer.WriteEndElement();

        foreach (var entry in entries)
        {
            var entryUrl = SiteBaseUrl + entry.CanonicalPath;
            writer.WriteStartElement("item");
            writer.WriteElementString("title", entry.FrontMatter.Title);
            writer.WriteElementString("link", entryUrl);
            writer.WriteElementString("guid", entryUrl);
            var pubDate = ParsePublishedAt(entry.FrontMatter.PublishedAt);
            if (pubDate != DateTimeOffset.MinValue)
            {
                writer.WriteElementString(
                    "pubDate",
                    pubDate.ToString("R", CultureInfo.InvariantCulture));
            }

            if (entry.FrontMatter.Tags.Count > 0)
            {
                foreach (var tag in entry.FrontMatter.Tags)
                {
                    writer.WriteElementString("category", tag);
                }
            }

            writer.WriteEndElement(); // item
        }

        writer.WriteEndElement(); // channel
        writer.WriteEndElement(); // rss
        writer.WriteEndDocument();
    }

    private static DateTimeOffset ParsePublishedAt(string value) =>
        DateTimeOffset.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var result)
            ? result
            : DateTimeOffset.MinValue;
}
