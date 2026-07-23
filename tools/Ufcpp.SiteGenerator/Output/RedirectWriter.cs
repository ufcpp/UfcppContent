namespace Ufcpp.SiteGenerator.Output;

/// <summary>
/// Generates redirect HTML files for page aliases so that old URLs continue to work.
/// </summary>
public static class RedirectWriter
{
    /// <summary>
    /// Writes a redirect HTML page at each alias output path that performs an immediate
    /// meta-refresh and canonical-link redirect to the target canonical URL.
    /// </summary>
    /// <param name="canonicalPath">The target canonical site path (e.g. <c>/study/csharp/</c>).</param>
    /// <param name="aliases">The alias site paths to generate redirects for.</param>
    /// <param name="outputDirectory">The root output directory of the site.</param>
    /// <param name="noIndex">Whether to prevent generated redirects from being indexed.</param>
    public static void Write(
        string canonicalPath,
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

            var html = BuildRedirectHtml(canonicalPath, noIndex);
            File.WriteAllText(destFile, html, System.Text.Encoding.UTF8);
        }
    }

    private static string BuildRedirectHtml(string targetUrl, bool noIndex)
    {
        var robotsMeta = noIndex
            ? """<meta name="robots" content="noindex, nofollow" />"""
            : string.Empty;

        return $"""
            <!DOCTYPE html>
            <html lang="ja">
            <head>
            <meta charset="UTF-8" />
            {robotsMeta}
            <link rel="canonical" href="{targetUrl}" />
            <meta http-equiv="refresh" content="0; url={targetUrl}" />
            <title>Redirecting...</title>
            </head>
            <body>
            <p><a href="{targetUrl}">Redirecting...</a></p>
            </body>
            </html>
            """;
    }
}
