using System.Xml;
using System.Xml.Linq;

namespace Ufcpp.ContentConverter;

public sealed class RewriteMapCatalog
{
    private readonly Dictionary<string, List<KeyValuePair<string, string>>> _maps;

    private RewriteMapCatalog(Dictionary<string, List<KeyValuePair<string, string>>> maps)
    {
        _maps = maps;
    }

    public static RewriteMapCatalog Load(string path)
    {
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
            element => element.Elements("add")
                .Select(add => new KeyValuePair<string, string>(
                    RequiredAttribute(add, "key"),
                    RequiredAttribute(add, "value")))
                .ToList(),
            StringComparer.Ordinal);
        return new RewriteMapCatalog(maps);
    }

    public IReadOnlyList<KeyValuePair<string, string>> this[string name] =>
        _maps.TryGetValue(name, out var values) ? values : [];

    public void ValidateExpectedCounts()
    {
        var expected = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["SubjectRedirectsNewSubject"] = 11,
            ["SubjectRedirects"] = 20,
            ["SubjectRewrites"] = 0,
            ["ArticleRedirects"] = 456,
            ["ArticleRewrites"] = 284,
        };
        foreach (var item in expected)
        {
            if (this[item.Key].Count != item.Value)
            {
                throw new InvalidDataException(
                    $"Rewrite map '{item.Key}' has {this[item.Key].Count} entries; expected {item.Value}.");
            }
        }
    }

    public IReadOnlyDictionary<int, IReadOnlyList<string>> BuildAliases(
        IReadOnlyList<ContentNode> nodes,
        IReadOnlyDictionary<int, string> canonicalUrls)
    {
        var byUrl = nodes
            .Where(node => ContentPaths.GeneratedTypes.Contains(node.ContentType))
            .ToDictionary(
                node => ContentPaths.NormalizeSitePath(canonicalUrls[node.Id]),
                node => node,
                StringComparer.OrdinalIgnoreCase);
        var aliases = nodes.ToDictionary(node => node.Id, _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase));

        void AddToNode(ContentNode node, params string[] values)
        {
            foreach (var value in values)
            {
                var alias = "/" + value.Replace('\\', '/').Trim().TrimStart('/');
                if (alias != "/")
                {
                    aliases[node.Id].Add(alias);
                    if (alias.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                    {
                        var extensionless = alias[..^".html".Length];
                        if (!byUrl.ContainsKey(ContentPaths.NormalizeSitePath(extensionless)))
                        {
                            aliases[node.Id].Add(extensionless);
                        }
                    }
                }
            }
        }

        ContentNode? ResolveMovedTarget(string value)
        {
            var path = value.Replace('\\', '/').Trim().Trim('/');
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 0)
            {
                return null;
            }

            var slug = Uri.UnescapeDataString(segments[^1]);
            if (slug.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                slug = slug[..^".html".Length];
            }

            var expectedParent = "/" + string.Join('/', segments.Take(segments.Length - 1)) + "/";
            var parentMatches = nodes.Where(node =>
                    node.ContentType == "Article"
                    && node.Parent is not null
                    && ContentPaths.CanonicalUrl(node.Parent).Equals(
                        expectedParent,
                        StringComparison.OrdinalIgnoreCase)
                    && (node.UrlName.Equals(slug, StringComparison.OrdinalIgnoreCase)
                        || node.UrlName.EndsWith("-" + slug, StringComparison.OrdinalIgnoreCase)
                        || node.UrlName.StartsWith(slug + "-", StringComparison.OrdinalIgnoreCase)))
                .ToArray();
            if (parentMatches.Length == 1)
            {
                return parentMatches[0];
            }

            if (parentMatches.Length > 1)
            {
                throw new InvalidDataException(
                    $"Legacy rewrite slug '{slug}' is ambiguous below '{expectedParent}'.");
            }

            var subjectPrefix = segments.Length >= 2
                ? "/" + string.Join('/', segments.Take(2)) + "/"
                : "/";
            var matches = nodes.Where(node =>
                    node.ContentType == "Article"
                    && (node.UrlName.Equals(slug, StringComparison.OrdinalIgnoreCase)
                        || node.UrlName.EndsWith("-" + slug, StringComparison.OrdinalIgnoreCase)
                        || node.UrlName.StartsWith(slug + "-", StringComparison.OrdinalIgnoreCase))
                    && canonicalUrls[node.Id].StartsWith(subjectPrefix, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            return matches.Length switch
            {
                0 => null,
                1 => matches[0],
                _ => throw new InvalidDataException(
                    $"Legacy rewrite slug '{slug}' is ambiguous below '{subjectPrefix}'."),
            };
        }

        void Add(string target, params string[] values)
        {
            var normalizedTarget = ContentPaths.NormalizeSitePath(target);
            if (!byUrl.TryGetValue(normalizedTarget, out var node))
            {
                node = ResolveMovedTarget(target)
                    ?? throw new InvalidDataException(
                        $"Legacy rewrite target has no canonical node: '{target}'.");
            }

            AddToNode(node, values);
        }

        void AddOptional(string target, params string[] values)
        {
            if (byUrl.TryGetValue(ContentPaths.NormalizeSitePath(target), out var node))
            {
                AddToNode(node, values);
            }
        }

        void AddEither(string first, string second)
        {
            var firstIsTarget = byUrl.ContainsKey(ContentPaths.NormalizeSitePath(first));
            var secondIsTarget = byUrl.ContainsKey(ContentPaths.NormalizeSitePath(second));
            if (firstIsTarget && secondIsTarget)
            {
                throw new InvalidDataException(
                    $"Legacy rewrite pair identifies two canonical targets: '{first}' => '{second}'.");
            }

            if (firstIsTarget)
            {
                Add(first, second);
            }
            else if (secondIsTarget)
            {
                Add(second, first);
            }
            else
            {
                var target = ResolveMovedTarget(first) ?? ResolveMovedTarget(second)
                    ?? throw new InvalidDataException(
                        $"Legacy rewrite pair has no canonical target: '{first}' => '{second}'.");
                AddToNode(target, first, second);
            }
        }

        foreach (var pair in this["SubjectRedirectsNewSubject"])
        {
            Add(pair.Value, $"{pair.Key.TrimEnd('/')}/index.html");
        }

        foreach (var pair in this["SubjectRedirects"])
        {
            Add(pair.Value, pair.Key.TrimEnd('/') + "/", $"{pair.Key.TrimEnd('/')}/index.html");
        }

        foreach (var pair in this["SubjectRewrites"])
        {
            Add(pair.Value, pair.Key);
        }

        foreach (var pair in this["ArticleRedirects"])
        {
            AddEither(pair.Key, pair.Value);
        }

        foreach (var pair in this["ArticleRewrites"])
        {
            AddEither(pair.Key, pair.Value);
        }

        AddOptional("/study/csharp/", "csharp", "csharp/");
        AddOptional("/study/stl/", "stydy/stl", "stydy/stl/");
        AddOptional("/study/xml/", "study/testxsl", "study/testxsl/");
        AddOptional(
            "/study/csharp/datatype/patterns/",
            "study/csharp/datatype/patternmatching",
            "study/csharp/datatype/patternmatching/");
        AddOptional("/study/misc/list/lecture/", "lecture", "lecture/", "lecture/index.html");
        AddOptional("/study/math/hs/m2/", "study/math/elementary", "study/math/elementary/");
        AddOptional(
            "/study/xml/summary/variable/",
            "study/summary/xml/variable",
            "study/summary/xml/variable/");
        AddOptional(
            "/study/csharp/datatype/tuples/",
            "study/csharp/data/tuples",
            "study/csharp/data/tuples/");
        AddOptional(
            "/study/csharp/datatype/deconstruction/",
            "study/csharp/data/deconstruction",
            "study/csharp/data/deconstruction/");
        AddOptional(
            "/study/physics/em/variable/",
            "study/em/physics/variable",
            "study/em/physics/variable/");

        foreach (var node in nodes.Where(node =>
                     ContentPaths.GeneratedTypes.Contains(node.ContentType)
                     && canonicalUrls[node.Id].StartsWith("/study/", StringComparison.OrdinalIgnoreCase)))
        {
            AddToNode(node, canonicalUrls[node.Id]["/study".Length..]);
        }

        foreach (var pair in aliases)
        {
            foreach (var alias in pair.Value
                         .Where(value => value.StartsWith("/study/", StringComparison.OrdinalIgnoreCase))
                         .ToArray())
            {
                pair.Value.Add(alias["/study".Length..]);
            }
        }

        return aliases.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<string>)pair.Value.Order(StringComparer.Ordinal).ToArray());
    }

    private static string RequiredAttribute(XElement element, string name) =>
        element.Attribute(name)?.Value
        ?? throw new InvalidDataException($"Element '{element.Name}' is missing '{name}'.");
}
