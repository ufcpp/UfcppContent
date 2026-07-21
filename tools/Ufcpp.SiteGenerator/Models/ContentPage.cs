namespace Ufcpp.SiteGenerator.Models;

/// <summary>A fully-loaded content page ready for HTML generation.</summary>
public sealed class ContentPage
{
    /// <summary>Parsed YAML front matter.</summary>
    public required FrontMatter FrontMatter { get; init; }

    /// <summary>
    /// Path of the source Markdown file relative to the content root directory
    /// (e.g. <c>study/csharp/index.md</c>).
    /// </summary>
    public required string RelativePath { get; init; }

    /// <summary>Markdown body text after the front matter block.</summary>
    public required string MarkdownBody { get; init; }

    /// <summary>
    /// Canonical site path extracted from <see cref="FrontMatter.SourceUrl"/>
    /// (e.g. <c>/study/csharp/</c>).
    /// </summary>
    public required string CanonicalPath { get; init; }

    /// <summary>
    /// Output file path relative to the site output directory
    /// (e.g. <c>study/csharp/index.html</c>).
    /// </summary>
    public required string OutputPath { get; init; }
}
