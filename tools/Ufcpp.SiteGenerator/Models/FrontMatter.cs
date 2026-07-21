namespace Ufcpp.SiteGenerator.Models;

/// <summary>YAML front matter found at the top of every content Markdown file.</summary>
public sealed class FrontMatter
{
    public string Title { get; init; } = "";
    public string SourceUrl { get; init; } = "";
    public string ContentType { get; init; } = "";
    public string PublishedAt { get; init; } = "";
    public string UpdatedAt { get; init; } = "";
    public List<string> Tags { get; init; } = [];
    public int UmbracoId { get; init; }
    public int ParentId { get; init; }
    public int SortOrder { get; init; }
    public List<string> Aliases { get; init; } = [];
}
