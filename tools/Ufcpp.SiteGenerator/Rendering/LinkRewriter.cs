namespace Ufcpp.SiteGenerator.Rendering;

/// <summary>
/// Resolves relative links found in a Markdown file to canonical site URLs.
/// Converts <c>.md</c> file references to canonical site paths and
/// relative <c>assets/</c> paths to root-relative <c>/assets/</c> URLs.
/// </summary>
public sealed class LinkRewriter
{
    private readonly string _contentRootDirectory;
    private readonly string _assetsRootDirectory;
    private readonly string _currentFileDirectory;
    private readonly IReadOnlyDictionary<string, string> _urlMap;

    /// <summary>
    /// Initialises the rewriter for a specific page.
    /// </summary>
    /// <param name="contentRootDirectory">Absolute path to the content directory root.</param>
    /// <param name="currentFilePath">Absolute path of the Markdown file being processed.</param>
    /// <param name="urlMap">Map from absolute file path → canonical site path.</param>
    public LinkRewriter(
        string contentRootDirectory,
        string currentFilePath,
        IReadOnlyDictionary<string, string> urlMap)
        : this(
            contentRootDirectory,
            Path.Combine(contentRootDirectory, "..", "assets"),
            currentFilePath,
            urlMap)
    {
    }

    /// <summary>Initialises the rewriter with an explicit asset source directory.</summary>
    public LinkRewriter(
        string contentRootDirectory,
        string assetsRootDirectory,
        string currentFilePath,
        IReadOnlyDictionary<string, string> urlMap)
    {
        _contentRootDirectory = Path.GetFullPath(contentRootDirectory);
        _assetsRootDirectory = Path.GetFullPath(assetsRootDirectory);
        _currentFileDirectory = Path.GetDirectoryName(Path.GetFullPath(currentFilePath))
            ?? _contentRootDirectory;
        _urlMap = urlMap;
    }

    /// <summary>Resolves a single URL string, returning the rewritten URL or the original.</summary>
    public string RewriteUrl(string rawUrl)
    {
        if (string.IsNullOrWhiteSpace(rawUrl))
        {
            return rawUrl;
        }

        // Keep fragment-only links
        if (rawUrl.StartsWith('#'))
        {
            return rawUrl;
        }

        // Keep external links, including protocol-relative URLs.
        if (rawUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("//", StringComparison.Ordinal)
            || rawUrl.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("tel:", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
        {
            return rawUrl;
        }

        var suffixIndex = rawUrl.IndexOfAny(['?', '#']);
        var urlPath = suffixIndex >= 0 ? rawUrl[..suffixIndex] : rawUrl;
        var suffix = suffixIndex >= 0 ? rawUrl[suffixIndex..] : "";

        // Existing root-relative legacy asset URLs (for example /media/...) map
        // to files beneath the emitted /assets/ tree. Other site URLs stay intact.
        if (urlPath.StartsWith('/'))
        {
            if (urlPath.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase))
            {
                return rawUrl;
            }

            var assetRelativePath = Uri.UnescapeDataString(urlPath.TrimStart('/'));
            var assetFile = Path.GetFullPath(Path.Combine(
                _assetsRootDirectory,
                assetRelativePath.Replace('/', Path.DirectorySeparatorChar)));

            if (IsWithinDirectory(assetFile, _assetsRootDirectory) && File.Exists(assetFile))
            {
                return "/assets/" + urlPath.TrimStart('/') + suffix;
            }

            return rawUrl;
        }

        if (string.IsNullOrEmpty(urlPath))
        {
            return suffix;
        }

        // Resolve relative path against the current file's directory
        var resolved = Path.GetFullPath(Path.Combine(
            _currentFileDirectory,
            Uri.UnescapeDataString(urlPath).Replace('/', Path.DirectorySeparatorChar)));

        // Check if this is an .md link
        if (_urlMap.TryGetValue(resolved, out var canonicalPath))
        {
            return canonicalPath + suffix;
        }

        // Also try without .md extension (in case link lacks it)
        if (!urlPath.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            var withMd = resolved + ".md";
            if (_urlMap.TryGetValue(withMd, out canonicalPath))
            {
                return canonicalPath + suffix;
            }
        }

        // Check if this resolves to within the assets directory
        if (IsWithinDirectory(resolved, _assetsRootDirectory))
        {
            var assetRelative = Path.GetRelativePath(_assetsRootDirectory, resolved)
                .Replace('\\', '/');
            return "/assets/" + assetRelative + suffix;
        }

        // Return original (might be an external relative URL or something else)
        return rawUrl;
    }

    private static bool IsWithinDirectory(string path, string directory)
    {
        var relative = Path.GetRelativePath(directory, path);
        return !Path.IsPathRooted(relative)
            && relative != ".."
            && !relative.StartsWith(
                ".." + Path.DirectorySeparatorChar,
                StringComparison.Ordinal);
    }
}
