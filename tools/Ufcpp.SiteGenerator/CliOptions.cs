namespace Ufcpp.SiteGenerator;

/// <summary>CLI options for the site generator.</summary>
public sealed class CliOptions
{
    /// <summary>Path to the content directory containing Markdown files.</summary>
    public required string ContentDirectory { get; init; }

    /// <summary>Path to the assets directory to copy to the output.</summary>
    public required string AssetsDirectory { get; init; }

    /// <summary>Path to the site output directory.</summary>
    public required string OutputDirectory { get; init; }

    /// <summary>When true, skips post-generation link and asset validation.</summary>
    public bool SkipValidation { get; init; }

    /// <summary>When true, writes a .NET 10 file-based static preview server.</summary>
    public bool IncludePreviewServer { get; init; }

    /// <summary>When true, prevents generated HTML from being indexed by search engines.</summary>
    public bool NoIndex { get; init; }
}
