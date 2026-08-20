namespace Ufcpp.SiteGenerator.Output;

/// <summary>
/// Converts site-root-relative URLs to URLs relative to the page that contains them.
/// </summary>
public static class SiteUrlResolver
{
    private static readonly Uri SiteOrigin = new("https://ufcpp.invalid/", UriKind.Absolute);

    /// <summary>
    /// Makes a root-relative internal URL portable across arbitrary deployment base paths.
    /// Non-root-relative URLs are returned unchanged.
    /// </summary>
    public static string MakeRelative(string sourceSitePath, string targetUrl)
    {
        if (!IsRootRelative(targetUrl))
        {
            return targetUrl;
        }

        _ = OutputPathResolver.Resolve(sourceSitePath);

        var sourceUri = new Uri(SiteOrigin, sourceSitePath);
        var targetUri = new Uri(SiteOrigin, targetUrl);
        if (!string.Equals(targetUri.Scheme, SiteOrigin.Scheme, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(targetUri.Host, SiteOrigin.Host, StringComparison.OrdinalIgnoreCase)
            || targetUri.Port != SiteOrigin.Port)
        {
            throw new InvalidDataException($"Invalid internal site URL '{targetUrl}'.");
        }

        var relativeUrl = sourceUri.MakeRelativeUri(targetUri).OriginalString;
        return relativeUrl.Length == 0
            ? GetSelfReference(sourceUri.AbsolutePath)
            : relativeUrl;
    }

    private static bool IsRootRelative(string url) =>
        url.StartsWith('/')
        && !url.StartsWith("//", StringComparison.Ordinal);

    private static string GetSelfReference(string sourcePath)
    {
        if (sourcePath.EndsWith('/'))
        {
            return "./";
        }

        var slashIndex = sourcePath.LastIndexOf('/');
        return sourcePath[(slashIndex + 1)..];
    }
}
