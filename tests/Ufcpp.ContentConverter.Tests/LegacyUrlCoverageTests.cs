using System.Text;
using System.Text.Json;
using Ufcpp.ContentConverter;

namespace Ufcpp.ContentConverter.Tests;

/// <summary>
/// Guards issue #28: every legacy URL declared in the IIS rewrite maps must still be
/// served by the generated site, and <c>/study/大カテゴリー/カテゴリー/記事名/</c> must be
/// the canonical URL for every page. Runs entirely offline against the committed
/// <c>content/</c> tree.
/// </summary>
public class LegacyUrlCoverageTests
{
    private static readonly string[] RewriteMapNames =
    [
        "SubjectRedirectsNewSubject",
        "SubjectRedirects",
        "SubjectRewrites",
        "ArticleRedirects",
        "ArticleRewrites",
    ];

    private static readonly Lazy<IReadOnlyList<ContentPage>> Pages = new(LoadPages);

    [Fact]
    public void CanonicalUrlsAreNeverLegacyHtmlPaths()
    {
        var offenders = Pages.Value
            .Where(page => page.CanonicalUrl.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
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
            served.Add(RouteKey(page.CanonicalUrl));
            foreach (var alias in page.Aliases)
            {
                served.Add(RouteKey(alias));
            }
        }

        var catalog = RewriteMapCatalog.Load(
            Path.Combine(RepoRoot, "tools", "Ufcpp.ContentConverter", "data", "rewrite_rewritemaps.config"));
        catalog.ValidateExpectedCounts();

        var missing = RewriteMapNames
            .SelectMany(name => catalog[name])
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
            .Where(page => !page.Aliases.SequenceEqual(
                AliasPolicy.SelectPublished(page.CanonicalUrl, page.Aliases),
                StringComparer.Ordinal))
            .Select(page => page.RelativePath)
            .ToArray();

        Assert.Empty(offenders);
    }

    private static string RouteKey(string value)
    {
        var path = "/" + value.Replace('\\', '/').Trim().TrimStart('/');
        return path.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
            ? path
            : path.TrimEnd('/') + "/";
    }

    private static string RepoRoot { get; } = FindRepoRoot();

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

        throw new InvalidOperationException("Could not locate the repository root from the test output directory.");
    }

    private static IReadOnlyList<ContentPage> LoadPages()
    {
        var contentRoot = Path.Combine(RepoRoot, "content");
        var pages = new List<ContentPage>();
        foreach (var file in Directory
            .EnumerateFiles(contentRoot, "*.md", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal))
        {
            var lines = TextUtilities
                .NormalizeNewlines(File.ReadAllText(file, new UTF8Encoding(false)))
                .Split('\n');
            var relativePath = Path.GetRelativePath(RepoRoot, file).Replace('\\', '/');

            var sourceUrlIndex = Array.FindIndex(
                lines,
                line => line.StartsWith("source_url: ", StringComparison.Ordinal));
            var aliasesIndex = Array.FindIndex(lines, line => line is "aliases:" or "aliases: []");
            if (sourceUrlIndex < 0 || aliasesIndex < 0)
            {
                throw new InvalidDataException($"'{relativePath}' has no source_url/aliases front matter.");
            }

            var sourceUrl = ParseYamlString(lines[sourceUrlIndex]["source_url: ".Length..]);
            var aliases = new List<string>();
            for (var index = aliasesIndex + 1;
                index < lines.Length && lines[index].StartsWith("  - ", StringComparison.Ordinal);
                index++)
            {
                aliases.Add(ParseYamlString(lines[index]["  - ".Length..]));
            }

            pages.Add(new ContentPage(relativePath, new Uri(sourceUrl).AbsolutePath, aliases));
        }

        return pages;
    }

    private static string ParseYamlString(string value) =>
        JsonSerializer.Deserialize<string>(value)
            ?? throw new InvalidDataException($"'{value}' is not a quoted string.");

    private sealed record ContentPage(
        string RelativePath,
        string CanonicalUrl,
        IReadOnlyList<string> Aliases);
}
