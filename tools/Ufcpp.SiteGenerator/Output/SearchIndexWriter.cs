using System.Text.Json;
using Ufcpp.SiteGenerator.Models;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>Writes a deterministic search index for static-site clients.</summary>
public static class SearchIndexWriter
{
    private static readonly HashSet<string> ExcludedContentTypes = new(
        ["Search", "Sitemap"],
        StringComparer.OrdinalIgnoreCase);

    public static void Write(IEnumerable<ContentPage> pages, string outputDirectory)
    {
        var entries = pages
            .Where(page => !ExcludedContentTypes.Contains(page.FrontMatter.ContentType))
            .OrderBy(page => page.FrontMatter.SourceUrl, StringComparer.Ordinal)
            .ToArray();

        var outputPath = Path.Combine(outputDirectory, "search-index.json");
        using var stream = File.Open(outputPath, FileMode.Create, FileAccess.Write);
        using var writer = new Utf8JsonWriter(
            stream,
            new JsonWriterOptions { Indented = true });

        writer.WriteStartArray();
        foreach (var page in entries)
        {
            writer.WriteStartObject();
            writer.WriteString("url", page.FrontMatter.SourceUrl);
            writer.WriteString("title", page.FrontMatter.Title);
            writer.WriteString("contentType", page.FrontMatter.ContentType);

            writer.WriteStartArray("tags");
            foreach (var tag in page.FrontMatter.Tags)
            {
                writer.WriteStringValue(tag);
            }

            writer.WriteEndArray();
            writer.WriteString("text", page.MarkdownBody);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.Flush();
    }
}
