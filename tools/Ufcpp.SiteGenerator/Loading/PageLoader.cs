using System.Text.RegularExpressions;
using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Output;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Ufcpp.SiteGenerator.Loading;

/// <summary>
/// Loads all Markdown files from the content directory, parses front matter,
/// and builds a URL map for link resolution.
/// </summary>
public sealed class PageLoader
{
    private static readonly Regex FrontMatterRegex = new(
        @"^---\s*\r?\n(?<yaml>.*?)\r?\n---\s*\r?\n?(?<body>.*)",
        RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    /// <summary>Loads all Markdown files in the given content directory.</summary>
    /// <param name="contentDirectory">Absolute path to the content directory.</param>
    /// <returns>
    /// A list of loaded pages, plus a URL map from absolute file path to canonical site path.
    /// </returns>
    public static (IReadOnlyList<ContentPage> Pages, IReadOnlyDictionary<string, string> UrlMap)
        Load(string contentDirectory)
    {
        var pages = new List<ContentPage>();
        var urlMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var outputClaims = new Dictionary<string, OutputClaim>(StringComparer.OrdinalIgnoreCase);

        var files = Directory
            .EnumerateFiles(contentDirectory, "*.md", SearchOption.AllDirectories)
            .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var file in files)
        {
            var text = File.ReadAllText(file, System.Text.Encoding.UTF8);
            var match = FrontMatterRegex.Match(text);
            if (!match.Success)
            {
                throw new InvalidDataException($"Missing or malformed YAML front matter in '{file}'.");
            }

            var yaml = match.Groups["yaml"].Value;
            var body = match.Groups["body"].Value;

            FrontMatter frontMatter;
            try
            {
                frontMatter = YamlDeserializer.Deserialize<FrontMatter>(yaml);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Failed to parse YAML front matter in '{file}': {ex.Message}", ex);
            }

            if (string.IsNullOrWhiteSpace(frontMatter.SourceUrl))
            {
                throw new InvalidDataException($"Missing 'source_url' in front matter of '{file}'.");
            }

            var canonicalPath = OutputPathResolver.ExtractCanonicalPath(frontMatter.SourceUrl);
            var outputPath = OutputPathResolver.Resolve(canonicalPath);

            ClaimOutputPath(
                outputClaims,
                outputPath,
                canonicalPath,
                isPrimary: true,
                file);

            foreach (var alias in frontMatter.Aliases)
            {
                ClaimOutputPath(
                    outputClaims,
                    OutputPathResolver.Resolve(alias),
                    canonicalPath,
                    isPrimary: false,
                    file);
            }

            var relativePath = Path.GetRelativePath(contentDirectory, file)
                .Replace('\\', '/');

            var page = new ContentPage
            {
                FrontMatter = frontMatter,
                RelativePath = relativePath,
                MarkdownBody = body,
                CanonicalPath = canonicalPath,
                OutputPath = outputPath,
            };

            pages.Add(page);

            // Register canonical path in URL map (keyed by absolute file path)
            urlMap[Path.GetFullPath(file)] = canonicalPath;
        }

        return (pages, urlMap);
    }

    private static void ClaimOutputPath(
        Dictionary<string, OutputClaim> claims,
        string outputPath,
        string canonicalPath,
        bool isPrimary,
        string sourceFile)
    {
        var normalizedOutputPath = outputPath.Replace('\\', '/').TrimStart('/');
        if (claims.TryGetValue(normalizedOutputPath, out var existing))
        {
            var equivalentAlias = string.Equals(
                    existing.CanonicalPath,
                    canonicalPath,
                    StringComparison.OrdinalIgnoreCase)
                && !(existing.IsPrimary && isPrimary);

            if (equivalentAlias)
            {
                return;
            }

            throw new InvalidDataException(
                $"Output path collision: '{normalizedOutputPath}' is claimed by "
                + $"'{existing.SourceFile}' for '{existing.CanonicalPath}' and "
                + $"'{sourceFile}' for '{canonicalPath}'.");
        }

        claims.Add(
            normalizedOutputPath,
            new OutputClaim(canonicalPath, isPrimary, sourceFile));
    }

    private sealed record OutputClaim(
        string CanonicalPath,
        bool IsPrimary,
        string SourceFile);
}
