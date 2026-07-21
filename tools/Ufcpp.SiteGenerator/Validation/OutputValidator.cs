using System.Net;
using System.Text.RegularExpressions;
using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Validation;

/// <summary>
/// Validates the generated site output for broken internal links, missing assets,
/// and output path collisions.
/// </summary>
public sealed class OutputValidator
{
    private readonly string _outputDirectory;
    private readonly IReadOnlyList<ContentPage> _pages;
    private readonly IReadOnlyDictionary<string, ContentPage> _targetsByOutputPath;
    private readonly Dictionary<string, HashSet<string>> _anchorsByFile =
        new(StringComparer.OrdinalIgnoreCase);

    public OutputValidator(
        string outputDirectory,
        IReadOnlyList<ContentPage> pages,
        IReadOnlyDictionary<string, string> urlMap)
    {
        _outputDirectory = Path.GetFullPath(outputDirectory);
        _pages = pages;
        _ = urlMap;

        var targets = new Dictionary<string, ContentPage>(StringComparer.OrdinalIgnoreCase);
        foreach (var page in pages)
        {
            targets.TryAdd(NormalizeOutputPath(page.OutputPath), page);
            foreach (var alias in page.FrontMatter.Aliases)
            {
                targets.TryAdd(NormalizeOutputPath(OutputPathResolver.Resolve(alias)), page);
            }
        }

        _targetsByOutputPath = targets;
    }

    /// <summary>
    /// Runs all validation checks. Throws <see cref="AggregateException"/> if any issues
    /// are found.
    /// </summary>
    public void Validate()
    {
        var errors = new List<string>();

        ValidateOutputFiles(errors);
        ValidateInternalLinks(errors);

        if (errors.Count > 0)
        {
            throw new AggregateException(
                $"Site generation validation failed with {errors.Count} error(s).",
                errors.Select(e => new InvalidDataException(e)));
        }
    }

    private void ValidateOutputFiles(List<string> errors)
    {
        // Check that every expected output file was actually written
        foreach (var page in _pages)
        {
            var outputFile = Path.Combine(
                _outputDirectory,
                page.OutputPath.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(outputFile))
            {
                errors.Add($"Expected output file not found: '{page.OutputPath}' (from '{page.RelativePath}').");
            }
        }
    }

    private void ValidateInternalLinks(List<string> errors)
    {
        // Only validate generated pages. HTML files copied beneath assets/ must
        // remain byte-for-byte unchanged and can legitimately contain relative URLs.
        foreach (var page in _pages.OrderBy(
            page => page.OutputPath,
            StringComparer.OrdinalIgnoreCase))
        {
            var outputFile = GetOutputFile(page.OutputPath);
            if (!File.Exists(outputFile))
            {
                continue;
            }

            var html = File.ReadAllText(outputFile, System.Text.Encoding.UTF8);
            ValidateHtmlLinks(page, outputFile, html, errors);
        }
    }

    private void ValidateHtmlLinks(
        ContentPage sourcePage,
        string htmlFile,
        string html,
        List<string> errors)
    {
        var matches = InternalLinkRegex.Matches(html);
        foreach (Match match in matches)
        {
            ValidateUrl(sourcePage, htmlFile, match.Groups["url"].Value, errors);
        }

        foreach (Match match in SourceParamRegex.Matches(html))
        {
            ValidateUrl(sourcePage, htmlFile, match.Groups["url"].Value, errors);
        }
    }

    private void ValidateUrl(
        ContentPage sourcePage,
        string htmlFile,
        string url,
        List<string> errors)
    {
        if (url.Length == 0 || IsExternalUrl(url))
        {
            return;
        }

        var hashIndex = url.IndexOf('#');
        var fragment = hashIndex >= 0 ? url[hashIndex..] : "";
        var pathAndQuery = hashIndex >= 0 ? url[..hashIndex] : url;
        var queryIndex = pathAndQuery.IndexOf('?');
        var path = queryIndex >= 0 ? pathAndQuery[..queryIndex] : pathAndQuery;

        if (path.Length > 0 && !path.StartsWith('/'))
        {
            errors.Add($"Relative URL '{url}' found in generated file '{RelativeTo(htmlFile)}'.");
            return;
        }

        if (path.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase))
        {
            var assetFile = ResolveRootRelativeOutputFile(path);
            if (assetFile is null || !File.Exists(assetFile))
            {
                errors.Add($"Missing asset '{path}' referenced in '{RelativeTo(htmlFile)}'.");
            }

            return;
        }

        ContentPage? targetPage;
        string targetFile;
        string targetPath;

        if (path.Length == 0)
        {
            targetPage = sourcePage;
            targetFile = htmlFile;
            targetPath = sourcePage.CanonicalPath;
        }
        else
        {
            var exactFile = ResolveRootRelativeOutputFile(path);
            if (exactFile is not null && File.Exists(exactFile))
            {
                var exactOutputPath = NormalizeOutputPath(RelativeTo(exactFile));
                _targetsByOutputPath.TryGetValue(exactOutputPath, out targetPage);
                targetFile = targetPage is null
                    ? exactFile
                    : GetOutputFile(targetPage.OutputPath);
                targetPath = targetPage?.CanonicalPath ?? path;
            }
            else
            {
                var requestedOutputPath = NormalizeOutputPath(OutputPathResolver.Resolve(path));
                _targetsByOutputPath.TryGetValue(requestedOutputPath, out targetPage);

                var requestedFile = GetOutputFile(requestedOutputPath);
                if (!File.Exists(requestedFile))
                {
                    errors.Add($"Broken internal link '{url}' in '{RelativeTo(htmlFile)}'.");
                    return;
                }

                targetFile = targetPage is null
                    ? requestedFile
                    : GetOutputFile(targetPage.OutputPath);
                targetPath = targetPage?.CanonicalPath ?? NormalizeSitePath(path);
            }
        }

        if (fragment.Length <= 1)
        {
            return;
        }

        var anchor = DecodeUrlComponent(fragment[1..]);
        if (!GetAnchors(targetFile).Contains(anchor))
        {
            errors.Add(
                $"Missing fragment '{fragment}' in target '{targetPath}' referenced in '{RelativeTo(htmlFile)}'.");
        }
    }

    private HashSet<string> GetAnchors(string htmlFile)
    {
        if (_anchorsByFile.TryGetValue(htmlFile, out var cached))
        {
            return cached;
        }

        var anchors = new HashSet<string>(StringComparer.Ordinal);
        if (File.Exists(htmlFile))
        {
            var html = File.ReadAllText(htmlFile, System.Text.Encoding.UTF8);
            foreach (Match match in IdAnchorRegex.Matches(html))
            {
                anchors.Add(WebUtility.HtmlDecode(match.Groups["value"].Value));
            }

            foreach (Match match in LegacyNamedAnchorRegex.Matches(html))
            {
                anchors.Add(WebUtility.HtmlDecode(match.Groups["value"].Value));
            }
        }

        _anchorsByFile[htmlFile] = anchors;
        return anchors;
    }

    private string GetOutputFile(string outputPath) =>
        Path.Combine(
            _outputDirectory,
            NormalizeOutputPath(outputPath).Replace('/', Path.DirectorySeparatorChar));

    private string? ResolveRootRelativeOutputFile(string path)
    {
        var relativePath = DecodeUrlComponent(path.TrimStart('/'))
            .Replace('/', Path.DirectorySeparatorChar);
        var candidate = Path.GetFullPath(Path.Combine(_outputDirectory, relativePath));
        var relativeToOutput = Path.GetRelativePath(_outputDirectory, candidate);
        if (Path.IsPathRooted(relativeToOutput)
            || relativeToOutput == ".."
            || relativeToOutput.StartsWith(
                ".." + Path.DirectorySeparatorChar,
                StringComparison.Ordinal))
        {
            return null;
        }

        return candidate;
    }

    private static bool IsExternalUrl(string url) =>
        url.StartsWith("//", StringComparison.Ordinal)
        || SchemeRegex.IsMatch(url);

    private static string DecodeUrlComponent(string value)
    {
        try
        {
            return Uri.UnescapeDataString(value);
        }
        catch (UriFormatException)
        {
            return value;
        }
    }

    private static string NormalizeOutputPath(string path) =>
        path.Replace('\\', '/').TrimStart('/');

    private static string NormalizeSitePath(string path)
    {
        var normalized = path.StartsWith('/') ? path : "/" + path;
        return normalized.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
            ? normalized
            : normalized.TrimEnd('/') + "/";
    }

    private string RelativeTo(string absolutePath) =>
        Path.GetRelativePath(_outputDirectory, absolutePath).Replace('\\', '/');

    private static readonly Regex InternalLinkRegex = new(
        @"(?:href|src|data)\s*=\s*(?<q>[""'])(?<url>[^""'\r\n]*?)\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex SourceParamRegex = new(
        @"<param\b(?=[^>]{0,2048}\bname\s*=\s*(?<nq>[""'])source\k<nq>)[^>]{0,2048}?\bvalue\s*=\s*(?<q>[""'])(?<url>[^""'\r\n]{0,2048})\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex IdAnchorRegex = new(
        @"(?<![\w-])id\s*=\s*(?<q>[""'])(?<value>[^""'\r\n]+)\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex LegacyNamedAnchorRegex = new(
        @"<a\b[^>]{0,2048}?(?<![\w-])name\s*=\s*(?<q>[""'])(?<value>[^""'\r\n]+)\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex SchemeRegex = new(
        @"^[a-z][a-z0-9+.-]*:",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));
}
