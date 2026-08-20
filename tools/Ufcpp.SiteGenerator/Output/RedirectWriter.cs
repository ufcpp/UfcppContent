using System.Net;
using System.Text.Json;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>
/// Generates redirect HTML files for page aliases so that old URLs continue to work.
/// </summary>
public static class RedirectWriter
{
    /// <summary>
    /// Writes a redirect HTML page at each alias output path that redirects to the target
    /// canonical URL. The redirect carries the incoming fragment across, so legacy links such
    /// as <c>/csharp/oo_interface.html?p=6#static-abstract</c> still land on their anchor in
    /// the single-page output. Legacy runtime query strings such as <c>?p=</c> are dropped.
    /// A meta-refresh fallback keeps the redirect working without scripting.
    /// </summary>
    /// <param name="canonicalPath">The target canonical site path (e.g. <c>/study/csharp/</c>).</param>
    /// <param name="canonicalUrl">The target's absolute public canonical URL.</param>
    /// <param name="aliases">The alias site paths to generate redirects for.</param>
    /// <param name="outputDirectory">The root output directory of the site.</param>
    /// <param name="noIndex">Whether to prevent generated redirects from being indexed.</param>
    public static void Write(
        string canonicalPath,
        string canonicalUrl,
        IEnumerable<string> aliases,
        string outputDirectory,
        bool noIndex = false)
    {
        var canonicalOutputPath = OutputPathResolver.Resolve(canonicalPath);
        foreach (var alias in aliases)
        {
            var outputPath = OutputPathResolver.Resolve(alias);
            if (string.Equals(
                outputPath,
                canonicalOutputPath,
                StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var destFile = Path.Combine(outputDirectory, outputPath.Replace('/', Path.DirectorySeparatorChar));

            var destDir = Path.GetDirectoryName(destFile);
            if (destDir is null)
            {
                throw new InvalidDataException(
                    $"Redirect output path has no parent directory: '{outputPath}'.");
            }

            if (File.Exists(destDir))
            {
                throw new InvalidDataException(
                    $"Redirect output directory is an existing file: '{destDir}'.");
            }

            Directory.CreateDirectory(destDir);

            var aliasPublicPath = GetPublicPath(alias, outputPath);
            var targetUrl = SiteUrlResolver.MakeRelative(aliasPublicPath, canonicalPath);
            var html = BuildRedirectHtml(targetUrl, canonicalUrl, noIndex);
            File.WriteAllText(destFile, html, System.Text.Encoding.UTF8);
        }
    }

    private static string GetPublicPath(string alias, string outputPath)
    {
        var normalized = outputPath.Replace('\\', '/').TrimStart('/');
        if (!string.Equals(normalized, "index.html", StringComparison.OrdinalIgnoreCase)
            && !normalized.EndsWith(
                "/index.html",
                StringComparison.OrdinalIgnoreCase))
        {
            return alias;
        }

        const string IndexFile = "index.html";
        return IsIndexFileAlias(alias, IndexFile)
            ? alias
            : alias.TrimEnd('/') + "/";
    }

    private static bool IsIndexFileAlias(string alias, string indexFile)
    {
        var slashIndex = alias.LastIndexOf('/');
        var lastSegment = alias[(slashIndex + 1)..];
        try
        {
            return Uri.UnescapeDataString(lastSegment)
                .Equals(indexFile, StringComparison.OrdinalIgnoreCase);
        }
        catch (UriFormatException)
        {
            return false;
        }
    }

    private static string BuildRedirectHtml(
        string targetUrl,
        string canonicalUrl,
        bool noIndex)
    {
        var robotsMeta = noIndex
            ? """<meta name="robots" content="noindex, nofollow" />"""
            : string.Empty;
        var encodedTarget = WebUtility.HtmlEncode(targetUrl);
        var encodedCanonical = WebUtility.HtmlEncode(canonicalUrl);
        var scriptTarget = JsonSerializer.Serialize(targetUrl);

        return $$"""
            <!DOCTYPE html>
            <html lang="ja">
            <head>
            <meta charset="UTF-8" />
            {{robotsMeta}}
            <link rel="canonical" href="{{encodedCanonical}}" />
            <script>
            (function () {
              var target = {{scriptTarget}};
              location.replace(target + location.hash);
            })();
            </script>
            <noscript><meta http-equiv="refresh" content="0; url={{encodedTarget}}" /></noscript>
            <title>Redirecting...</title>
            </head>
            <body>
            <p><a href="{{encodedTarget}}">Redirecting...</a></p>
            </body>
            </html>
            """;
    }
}
