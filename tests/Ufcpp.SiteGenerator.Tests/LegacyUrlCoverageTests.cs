using System.Xml;
using System.Xml.Linq;
using Ufcpp.SiteGenerator.Loading;
using Ufcpp.SiteGenerator.Models;

namespace Ufcpp.SiteGenerator.Tests;

/// <summary>
/// Guards issue #28: every URL declared in the archived IIS rewrite maps must still be
/// served by the generated site, and canonical URLs must not regress to legacy HTML paths.
/// </summary>
public sealed class LegacyUrlCoverageTests
{
    private const string HtmlExtension = ".html";
    private const string StudyPrefix = "/study/";

    private static readonly string[] RewriteMapNames =
    [
        "SubjectRedirectsNewSubject",
        "SubjectRedirects",
        "SubjectRewrites",
        "ArticleRedirects",
        "ArticleRewrites",
    ];

    private static readonly IReadOnlyDictionary<string, int> ExpectedRewriteMapCounts =
        new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["SubjectRedirectsNewSubject"] = 11,
            ["SubjectRedirects"] = 20,
            ["SubjectRewrites"] = 0,
            ["ArticleRedirects"] = 456,
            ["ArticleRewrites"] = 284,
        };

    private static readonly string RepoRoot = FindRepoRoot();

    private static readonly Lazy<IReadOnlyList<ContentPage>> Pages = new(
        () => PageLoader.Load(Path.Combine(RepoRoot, "content")).Pages);

    private static readonly Lazy<IReadOnlyDictionary<string, IReadOnlyList<KeyValuePair<string, string>>>>
        RewriteMaps = new(LoadRewriteMaps);

    [Fact]
    public void CanonicalUrlsAreNeverLegacyHtmlPaths()
    {
        var offenders = Pages.Value
            .Where(page => page.CanonicalPath.EndsWith(HtmlExtension, StringComparison.OrdinalIgnoreCase))
            .Select(page => page.RelativePath)
            .ToArray();

        Assert.Empty(offenders);
    }

    [Fact]
    public void EveryRewriteMapUrlIsServed()
    {
        var served = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var page in Pages.Value)
        {
            served.Add(RouteKey(page.CanonicalPath));
            foreach (var alias in page.FrontMatter.Aliases)
            {
                served.Add(RouteKey(alias));
            }
        }

        var missing = RewriteMapNames
            .SelectMany(name => RewriteMaps.Value[name])
            .SelectMany(entry => new[] { entry.Key, entry.Value })
            .Select(RouteKey)
            .Where(route => !served.Contains(route))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Empty(missing);
    }

    [Fact]
    public void PublishedAliasesContainNoDerivedForms()
    {
        var offenders = Pages.Value
            .Where(page => !page.FrontMatter.Aliases.SequenceEqual(
                SelectPublishedAliases(page.CanonicalPath, page.FrontMatter.Aliases),
                StringComparer.Ordinal))
            .Select(page => page.RelativePath)
            .ToArray();

        Assert.Empty(offenders);
    }

    private static IReadOnlyDictionary<string, IReadOnlyList<KeyValuePair<string, string>>>
        LoadRewriteMaps()
    {
        var path = Path.Combine(
            RepoRoot,
            "tests",
            "Ufcpp.SiteGenerator.Tests",
            "data",
            "rewrite_rewritemaps.config");
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore,
            XmlResolver = null,
        };
        using var reader = XmlReader.Create(path, settings);
        var root = XDocument.Load(reader).Root
            ?? throw new InvalidDataException("Rewrite map configuration has no root.");
        var maps = root.Elements("rewriteMap").ToDictionary(
            element => RequiredAttribute(element, "name"),
            element => (IReadOnlyList<KeyValuePair<string, string>>)element.Elements("add")
                .Select(add => new KeyValuePair<string, string>(
                    RequiredAttribute(add, "key"),
                    RequiredAttribute(add, "value")))
                .ToArray(),
            StringComparer.Ordinal);

        foreach (var expected in ExpectedRewriteMapCounts)
        {
            if (!maps.TryGetValue(expected.Key, out var entries) || entries.Count != expected.Value)
            {
                var actual = entries?.Count ?? 0;
                throw new InvalidDataException(
                    $"Rewrite map '{expected.Key}' has {actual} entries; expected {expected.Value}.");
            }
        }

        return maps;
    }

    private static IReadOnlyList<string> SelectPublishedAliases(
        string canonicalUrl,
        IReadOnlyList<string> aliases)
    {
        var routeKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            RouteKey(canonicalUrl),
        };
        foreach (var alias in aliases)
        {
            routeKeys.Add(RouteKey(alias));
        }

        var exactAliases = new HashSet<string>(aliases, StringComparer.OrdinalIgnoreCase);
        return aliases
            .Where(alias =>
                !IsStudyPrefixDropped(alias, routeKeys)
                && !IsHtmlExtensionDropped(alias, exactAliases))
            .Order(StringComparer.Ordinal)
            .ToArray();
    }

    private static bool IsStudyPrefixDropped(string alias, HashSet<string> routeKeys) =>
        !alias.StartsWith(StudyPrefix, StringComparison.OrdinalIgnoreCase)
        && routeKeys.Contains(RouteKey("/study" + alias));

    private static bool IsHtmlExtensionDropped(string alias, HashSet<string> exactAliases) =>
        !alias.EndsWith(HtmlExtension, StringComparison.OrdinalIgnoreCase)
        && exactAliases.Contains(alias + HtmlExtension);

    private static string RouteKey(string value)
    {
        var path = "/" + value.Replace('\\', '/').Trim().TrimStart('/');
        return path.EndsWith(HtmlExtension, StringComparison.OrdinalIgnoreCase)
            ? path
            : path.TrimEnd('/') + "/";
    }

    private static string RequiredAttribute(XElement element, string name) =>
        element.Attribute(name)?.Value
        ?? throw new InvalidDataException($"Element '{element.Name}' is missing '{name}'.");

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "UfcppContent.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException(
            $"Could not locate the repository root from '{AppContext.BaseDirectory}'.");
    }
}
