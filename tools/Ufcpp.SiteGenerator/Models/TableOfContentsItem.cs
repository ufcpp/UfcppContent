namespace Ufcpp.SiteGenerator.Models;

/// <summary>A link in an article table of contents, with any nested headings.</summary>
public sealed record TableOfContentsItem(
    string Url,
    string Title,
    IReadOnlyList<TableOfContentsItem> Children);
