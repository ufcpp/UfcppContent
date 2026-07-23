namespace Ufcpp.SiteGenerator.Models;

/// <summary>Rendered article HTML and navigation metadata derived from it.</summary>
public sealed record RenderedContent(
    string? TitleHtml,
    string BodyHtml,
    IReadOnlyList<TableOfContentsItem> TableOfContents,
    IReadOnlyList<NavigationItem> Keywords)
{
    public string Html => (TitleHtml ?? string.Empty) + BodyHtml;
}
