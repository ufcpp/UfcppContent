using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Rendering;

/// <summary>
/// Resolves links found in a Markdown file to deployment-independent site URLs.
/// Converts <c>.md</c> file references to canonical site paths and
/// emits internal targets relative to the current public page path.
/// </summary>
public sealed class LinkRewriter
{
    private readonly string _contentRootDirectory;
    private readonly string _assetsRootDirectory;
    private readonly string _currentFileDirectory;
    private readonly string _currentSitePath;
    private readonly IReadOnlyDictionary<string, string> _urlMap;
    private readonly IReadOnlySet<string> _knownSiteOutputs;

    /// <summary>
    /// Initialises the rewriter for a specific page.
    /// </summary>
    /// <param name="contentRootDirectory">Absolute path to the content directory root.</param>
    /// <param name="currentFilePath">Absolute path of the Markdown file being processed.</param>
    /// <param name="currentSitePath">Root-relative public path of the current page.</param>
    /// <param name="urlMap">Map from absolute file path → canonical site path.</param>
    public LinkRewriter(
        string contentRootDirectory,
        string currentFilePath,
        string currentSitePath,
        IReadOnlyDictionary<string, string> urlMap,
        IReadOnlySet<string>? knownSiteOutputs = null)
        : this(
            contentRootDirectory,
            Path.Combine(contentRootDirectory, "..", "assets"),
            currentFilePath,
            currentSitePath,
            urlMap,
            knownSiteOutputs)
    {
    }

    /// <summary>Initialises the rewriter with an explicit asset source directory.</summary>
    public LinkRewriter(
        string contentRootDirectory,
        string assetsRootDirectory,
        string currentFilePath,
        string currentSitePath,
        IReadOnlyDictionary<string, string> urlMap,
        IReadOnlySet<string>? knownSiteOutputs = null)
    {
        _contentRootDirectory = Path.GetFullPath(contentRootDirectory);
        _assetsRootDirectory = Path.GetFullPath(assetsRootDirectory);
        _currentFileDirectory = Path.GetDirectoryName(Path.GetFullPath(currentFilePath))
            ?? _contentRootDirectory;
        _currentSitePath = currentSitePath;
        _urlMap = urlMap;
        _knownSiteOutputs = knownSiteOutputs
            ?? urlMap.Values
                .Select(OutputPathResolver.Resolve)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
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

        if (TryGetKnownAbsoluteSiteUrl(rawUrl, out var siteUrl))
        {
            rawUrl = siteUrl;
        }
        else if (rawUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
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
        var suffix = suffixIndex >= 0 ? NormalizeSuffix(rawUrl[suffixIndex..]) : "";

        // Existing root-relative legacy asset URLs (for example /media/...) map
        // to files beneath the emitted /assets/ tree. All internal targets are then
        // made relative to the current public page.
        if (urlPath.StartsWith('/'))
        {
            if (urlPath.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase))
            {
                return MakeSiteRelative(urlPath + suffix);
            }

            var assetRelativePath = Uri.UnescapeDataString(urlPath.TrimStart('/'));
            var assetFile = Path.GetFullPath(Path.Combine(
                _assetsRootDirectory,
                assetRelativePath.Replace('/', Path.DirectorySeparatorChar)));

            if (IsWithinDirectory(assetFile, _assetsRootDirectory) && File.Exists(assetFile))
            {
                return MakeSiteRelative("/assets/" + urlPath.TrimStart('/') + suffix);
            }

            return MakeSiteRelative(urlPath + suffix);
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
            return MakeSiteRelative(canonicalPath + suffix);
        }

        // Also try without .md extension (in case link lacks it)
        if (!urlPath.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            var withMd = resolved + ".md";
            if (_urlMap.TryGetValue(withMd, out canonicalPath))
            {
                return MakeSiteRelative(canonicalPath + suffix);
            }
        }

        // Check if this resolves to within the assets directory
        if (IsWithinDirectory(resolved, _assetsRootDirectory))
        {
            var assetRelative = Path.GetRelativePath(_assetsRootDirectory, resolved)
                .Replace('\\', '/');
            return MakeSiteRelative("/assets/" + assetRelative + suffix);
        }

        // Return original (might be an external relative URL or something else)
        return urlPath + suffix;
    }

    private string MakeSiteRelative(string targetUrl) =>
        SiteUrlResolver.MakeRelative(_currentSitePath, targetUrl);

    private bool TryGetKnownAbsoluteSiteUrl(string rawUrl, out string siteUrl)
    {
        siteUrl = "";
        if (!Uri.TryCreate(rawUrl, UriKind.Absolute, out var uri)
            || uri.Scheme is not ("http" or "https")
            || !uri.IsDefaultPort
            || !IsSiteHost(uri.Host)
            || !IsKnownSiteTarget(uri.AbsolutePath))
        {
            return false;
        }

        siteUrl = uri.AbsolutePath + uri.Query + uri.Fragment;
        return true;
    }

    private bool IsKnownSiteTarget(string path)
    {
        try
        {
            if (_knownSiteOutputs.Contains(OutputPathResolver.Resolve(path)))
            {
                return true;
            }
        }
        catch (InvalidDataException)
        {
            return false;
        }

        var assetPath = path.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase)
            ? path["/assets/".Length..]
            : path.TrimStart('/');
        var assetFile = Path.GetFullPath(Path.Combine(
            _assetsRootDirectory,
            Uri.UnescapeDataString(assetPath)
                .Replace('/', Path.DirectorySeparatorChar)));
        return IsWithinDirectory(assetFile, _assetsRootDirectory)
            && File.Exists(assetFile);
    }

    private static bool IsSiteHost(string host) =>
        host.Equals("ufcpp.net", StringComparison.OrdinalIgnoreCase)
        || host.Equals("www.ufcpp.net", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Drops the legacy Umbraco page-number query (<c>?p=</c>) that used to select one page of
    /// a paginated article. The archive renders each article as a single page, so only the
    /// fragment identifies a position within it.
    /// </summary>
    private static string NormalizeSuffix(string suffix)
    {
        if (!suffix.StartsWith('?'))
        {
            return suffix;
        }

        var fragmentIndex = suffix.IndexOf('#');
        var fragment = fragmentIndex >= 0 ? suffix[fragmentIndex..] : "";
        var query = fragmentIndex >= 0 ? suffix[1..fragmentIndex] : suffix[1..];

        var retained = query
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(item => !IsLegacyPageParameter(item))
            .ToArray();

        return retained.Length == 0
            ? fragment
            : "?" + string.Join('&', retained) + fragment;
    }

    private static bool IsLegacyPageParameter(string queryItem)
    {
        var equals = queryItem.IndexOf('=');
        var key = equals >= 0 ? queryItem[..equals] : queryItem;
        return key.Equals("p", StringComparison.OrdinalIgnoreCase);
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
