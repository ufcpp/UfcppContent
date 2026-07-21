using System.Net;
using System.Text.RegularExpressions;

namespace Ufcpp.ContentConverter;

public sealed class LinkRewriter
{
    private static readonly IReadOnlyDictionary<string, (string Path, string Fragment)> LegacyLinkCorrections =
        new Dictionary<string, (string Path, string Fragment)>(StringComparer.OrdinalIgnoreCase)
        {
            ["/blog/2021/12/notorious-compat-char/#apple-log"] =
                ("/blog/2021/12/ninjacatdies/", "apple-log"),
            ["/study/math/group/field/#vortex"] =
                ("/study/math/vector_analysis/v_field/", "vortex"),
            ["/study/csharp/start/misc_unicode/#katakana-middle-dothttp://"] =
                ("/study/csharp/start/misc_unicode/", "katakana-middle-dot"),
        };

    private static readonly Regex HtmlAttributeRegex = new(
        @"(?<prefix><(?:a|img|source|video|audio|object|embed|script|link)\b[^>]*?\b(?:href|src|poster|data)\s*=\s*)(?<quote>[""'])(?<url>.*?)(\k<quote>)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex MarkdownDestinationRegex = new(
        @"(?<prefix>\]\(\s*)(?<angle><)?(?<url>[^\s)>]+)(?(angle)>)(?<suffix>(?:\s+[""'][^""']*[""'])?\s*\))",
        RegexOptions.Compiled);

    private static readonly Regex ReferenceLinkRegex = new(
        @"(?m)(?<prefix>^\s*\[(?!\[)[^\]\r\n]+\]:\s+)(?<angle><)?(?<url>[^\s>]+)(?(angle)>)",
        RegexOptions.Compiled);

    private static readonly Regex AutolinkRegex = new(
        @"<(?<url>https?://(?:www\.)?ufcpp\.net/[^>\s]+)>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex CssUrlRegex = new(
        @"(?<prefix>\burl\(\s*)(?<quote>[""']?)(?<url>[^)""']+)(\k<quote>)(?<suffix>\s*\))",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex SourceParamRegex = new(
        @"(?<prefix><param\b(?=[^>]{0,2048}\bname\s*=\s*(?<nq>[""'])source\k<nq>)[^>]{0,2048}?\bvalue\s*=\s*)(?<quote>[""'])(?<url>[^""'\r\n]{0,2048})\k<quote>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex MalformedLinkRegex = new(
        @"\]\(\((?<url>(?:https?://|/)[^\s)]+)\){1,2}",
        RegexOptions.Compiled);

    private static readonly Regex ProtectedBlockRegex = new(
        @"<pre\b[^>]*>.*?</pre>|<code\b[^>]*>.*?</code>|```.*?```|~~~.*?~~~",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private readonly IReadOnlyDictionary<string, ContentNode> _routes;
    private readonly IReadOnlyDictionary<int, string> _outputPaths;
    private readonly IReadOnlyDictionary<string, string> _blogCategoryAnchors;
    private readonly AssetManager _assets;

    public LinkRewriter(
        IReadOnlyList<ContentNode> nodes,
        IReadOnlyDictionary<int, string> canonicalUrls,
        IReadOnlyDictionary<int, string> outputPaths,
        IReadOnlyDictionary<int, IReadOnlyList<string>> aliases,
        IReadOnlyDictionary<string, string> blogCategoryAnchors,
        AssetManager assets)
    {
        _outputPaths = outputPaths;
        _blogCategoryAnchors = blogCategoryAnchors;
        _assets = assets;
        var routes = new Dictionary<string, ContentNode>(StringComparer.OrdinalIgnoreCase);
        foreach (var node in nodes.Where(node => outputPaths.ContainsKey(node.Id)))
        {
            AddRoute(routes, canonicalUrls[node.Id], node);
            foreach (var alias in aliases[node.Id])
            {
                AddRoute(routes, alias, node);
            }
        }

        _routes = routes;
    }

    public int RewrittenInternalLinks { get; private set; }

    public string Rewrite(string markdown, ContentNode current)
    {
        markdown = markdown.Replace(
            "[1ch.tv](/$root/memo/2007/02.html#16_1)",
            "1ch.tv",
            StringComparison.Ordinal);
        var protectedBlocks = new List<string>();
        markdown = ProtectedBlockRegex.Replace(
            markdown,
            match =>
            {
                var marker = $"\u001A{protectedBlocks.Count}\u001A";
                protectedBlocks.Add(match.Value);
                return marker;
            });
        markdown = MalformedLinkRegex.Replace(
            markdown,
            match => "](" + match.Groups["url"].Value + ")");

        string Replace(Match match)
        {
            var rewritten = RewriteUrl(WebUtility.HtmlDecode(match.Groups["url"].Value), current);
            var encoded = match.Value.Contains("&amp;", StringComparison.Ordinal)
                ? WebUtility.HtmlEncode(rewritten).Replace("&#39;", "'", StringComparison.Ordinal)
                : rewritten;
            var value = match.Value;
            var group = match.Groups["url"];
            var relativeStart = group.Index - match.Index;
            return value[..relativeStart] + encoded + value[(relativeStart + group.Length)..];
        }

        markdown = HtmlAttributeRegex.Replace(markdown, Replace);
        markdown = SourceParamRegex.Replace(markdown, Replace);
        markdown = MarkdownDestinationRegex.Replace(markdown, Replace);
        markdown = ReferenceLinkRegex.Replace(markdown, Replace);
        markdown = AutolinkRegex.Replace(markdown, Replace);
        markdown = CssUrlRegex.Replace(markdown, Replace);
        for (var index = 0; index < protectedBlocks.Count; index++)
        {
            markdown = markdown.Replace($"\u001A{index}\u001A", protectedBlocks[index], StringComparison.Ordinal);
        }

        return markdown;
    }

    public string RewriteUrl(string rawUrl, ContentNode current)
    {
        if (rawUrl.Equals("System.Threading.Tasks.Extensions", StringComparison.Ordinal))
        {
            return "https://www.nuget.org/packages/System.Threading.Tasks.Extensions/";
        }

        if (!rawUrl.Contains('/')
            && !rawUrl.Contains('?')
            && !rawUrl.Contains('#'))
        {
            var heading = Regex.Match(
                current.Get("bodyText"),
                $@"<a\s+[^>]*id=[""'](?<id>[^""']+)[""'][^>]*>\s*{Regex.Escape(rawUrl)}",
                RegexOptions.IgnoreCase);
            if (heading.Success)
            {
                return "#" + Uri.EscapeDataString(heading.Groups["id"].Value);
            }
        }

        if (!rawUrl.Contains('/')
            && !rawUrl.Contains('?')
            && !rawUrl.Contains('#')
            && Regex.IsMatch(
                current.Get("bodyText"),
                $@"\bid\s*=\s*[""']{Regex.Escape(rawUrl)}[""']",
                RegexOptions.IgnoreCase))
        {
            return "#" + Uri.EscapeDataString(rawUrl);
        }

        if (string.IsNullOrWhiteSpace(rawUrl)
            || rawUrl.StartsWith("(http://", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("(https://", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("tel:", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
        {
            return rawUrl;
        }

        var internalUrl = ParseInternal(rawUrl, current);
        if (internalUrl is null)
        {
            return rawUrl;
        }

        var path = internalUrl.Value.Path;
        var query = ParseQuery(internalUrl.Value.Query);
        var fragment = internalUrl.Value.Fragment;
        var silverlightDemo = Regex.Match(
            path,
            @"^/sl_(?<name>[A-Za-z0-9_-]+)\.html$",
            RegexOptions.IgnoreCase);
        if (silverlightDemo.Success)
        {
            path = "/media/ufcpp2000/csharp/ClientBin/"
                + silverlightDemo.Groups["name"].Value
                + ".xap";
        }
        else if (rawUrl.StartsWith("/../xsd/", StringComparison.OrdinalIgnoreCase))
        {
            path = "/media/ufcpp2000/xsd/xsd.zip";
        }

        query.RemoveAll(item => item.Key.Equals("p", StringComparison.OrdinalIgnoreCase));
        var runtimeFragments = new List<string>();
        foreach (var item in query.ToArray())
        {
            if (item.Key.Equals("key", StringComparison.OrdinalIgnoreCase)
                || item.Key.Equals("sec", StringComparison.OrdinalIgnoreCase))
            {
                runtimeFragments.Add(item.Value);
                query.Remove(item);
            }
            else if (item.Key.Equals("exercise", StringComparison.OrdinalIgnoreCase))
            {
                runtimeFragments.Add("exercise-" + item.Value);
                query.Remove(item);
            }
            else if (item.Key.Equals("list", StringComparison.OrdinalIgnoreCase) && !item.HasEquals)
            {
                runtimeFragments.Add("list");
                query.Remove(item);
            }
            else if (item.Key.Equals("bc", StringComparison.OrdinalIgnoreCase))
            {
                if (!_blogCategoryAnchors.TryGetValue(item.Value, out var anchor))
                {
                    throw new InvalidDataException($"Unknown blog category query value '{item.Value}'.");
                }

                runtimeFragments.Add(anchor);
                query.Remove(item);
            }
        }

        if (query.Count != 0)
        {
            throw new InvalidDataException(
                $"Unsupported internal runtime query in '{rawUrl}': " +
                string.Join(", ", query.Select(item => item.Key)));
        }

        if (runtimeFragments.Count > 1
            && runtimeFragments.Distinct(StringComparer.Ordinal).Count() != 1)
        {
            throw new InvalidDataException($"Conflicting runtime fragments in '{rawUrl}'.");
        }

        if (runtimeFragments.Count != 0)
        {
            fragment = runtimeFragments[0];
        }

        fragment = Uri.UnescapeDataString(fragment);
        var correctionKey = ContentPaths.NormalizeSitePath(path) + "#" + fragment;
        if (LegacyLinkCorrections.TryGetValue(correctionKey, out var correction))
        {
            path = correction.Path;
            fragment = correction.Fragment;
        }

        if (TryRoute(path, out var target))
        {
            var canonicalCorrectionKey = ContentPaths.CanonicalUrl(target) + "#" + fragment;
            if (LegacyLinkCorrections.TryGetValue(canonicalCorrectionKey, out correction))
            {
                path = correction.Path;
                fragment = correction.Fragment;
                if (!TryRoute(path, out target))
                {
                    throw new InvalidDataException(
                        $"Corrected legacy route '{path}' has no generated target.");
                }
            }

            RewrittenInternalLinks++;
            return RelativeLink(current, target, fragment);
        }

        if (path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
            && TryRoute(path[..^".md".Length], out target))
        {
            RewrittenInternalLinks++;
            return RelativeLink(current, target, fragment);
        }

        if (path.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
            && TryRoute(path[..^".html".Length], out target))
        {
            RewrittenInternalLinks++;
            return RelativeLink(current, target, fragment);
        }

        if (_assets.LooksLikeAsset(path))
        {
            var assetPath = _assets.ResolveAndCopy(path);
            RewrittenInternalLinks++;
            var currentDirectory = Path.GetDirectoryName(_outputPaths[current.Id])!;
            var relative = Path.GetRelativePath(
                    currentDirectory.Replace('/', Path.DirectorySeparatorChar),
                    assetPath.Replace('/', Path.DirectorySeparatorChar))
                .Replace('\\', '/');
            return relative + FormatFragment(fragment);
        }

        throw new InvalidDataException($"Unresolved internal URL '{rawUrl}' in node {current.Id}.");
    }

    private string RelativeLink(ContentNode current, ContentNode target, string fragment)
    {
        if (current.Id == target.Id && fragment.Length != 0)
        {
            return FormatFragment(fragment);
        }

        var currentDirectory = Path.GetDirectoryName(_outputPaths[current.Id])!;
        var relative = Path.GetRelativePath(
                currentDirectory.Replace('/', Path.DirectorySeparatorChar),
                _outputPaths[target.Id].Replace('/', Path.DirectorySeparatorChar))
            .Replace('\\', '/');
        return relative + FormatFragment(fragment);
    }

    private bool TryRoute(string path, out ContentNode node)
    {
        var normalized = ContentPaths.NormalizeSitePath(path);
        if (_routes.TryGetValue(normalized, out node!))
        {
            return true;
        }

        var raw = "/" + path.Replace('\\', '/').Trim().TrimStart('/');
        return _routes.TryGetValue(raw, out node!);
    }

    private static void AddRoute(Dictionary<string, ContentNode> routes, string path, ContentNode node)
    {
        foreach (var key in new[]
                 {
                     ContentPaths.NormalizeSitePath(path),
                     "/" + path.Replace('\\', '/').Trim().TrimStart('/'),
                 }.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (routes.TryGetValue(key, out var existing) && existing.Id != node.Id)
            {
                throw new InvalidDataException(
                    $"URL route collision '{key}' between nodes {existing.Id} and {node.Id}.");
            }

            routes[key] = node;
        }
    }

    private static (string Path, string Query, string Fragment)? ParseInternal(
        string rawUrl,
        ContentNode current)
    {
        rawUrl = rawUrl.Trim();
        if (rawUrl.StartsWith("study/", StringComparison.OrdinalIgnoreCase)
            || rawUrl.StartsWith("blog/", StringComparison.OrdinalIgnoreCase))
        {
            rawUrl = "/" + rawUrl;
        }

        if (rawUrl.StartsWith("//", StringComparison.Ordinal))
        {
            if (!Uri.TryCreate("https:" + rawUrl, UriKind.Absolute, out var protocolRelative)
                || !IsInternalHost(protocolRelative.Host))
            {
                return null;
            }

            return (
                Uri.UnescapeDataString(protocolRelative.AbsolutePath),
                protocolRelative.Query,
                protocolRelative.Fragment.TrimStart('#'));
        }

        if (Uri.TryCreate(rawUrl, UriKind.Absolute, out var absolute))
        {
            if (absolute.Scheme is not ("http" or "https") || !IsInternalHost(absolute.Host))
            {
                return null;
            }

            return (
                Uri.UnescapeDataString(absolute.AbsolutePath),
                absolute.Query,
                absolute.Fragment.TrimStart('#'));
        }

        var relativePath = rawUrl.Split('?', '#')[0];
        var baseNode = relativePath.Length != 0
            && !relativePath.StartsWith('/')
            && (relativePath.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                || current.ContentType is "BlogEntry" or "Article")
            && current.Parent is not null
                ? current.Parent
                : current;
        var baseUri = new Uri("https://ufcpp.net" + ContentPaths.CanonicalUrl(baseNode));
        if (!Uri.TryCreate(baseUri, rawUrl, out var resolved))
        {
            throw new InvalidDataException($"Malformed URL '{rawUrl}' in node {current.Id}.");
        }

        return (
            Uri.UnescapeDataString(resolved.AbsolutePath),
            resolved.Query,
            resolved.Fragment.TrimStart('#'));
    }

    private static bool IsInternalHost(string host) =>
        host.Equals("ufcpp.net", StringComparison.OrdinalIgnoreCase)
        || host.Equals("www.ufcpp.net", StringComparison.OrdinalIgnoreCase);

    private static List<QueryItem> ParseQuery(string query)
    {
        if (query.StartsWith('?'))
        {
            query = query[1..];
        }

        if (query.Length == 0)
        {
            return [];
        }

        return query.Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(part =>
            {
                var equals = part.IndexOf('=');
                var key = equals < 0 ? part : part[..equals];
                var value = equals < 0 ? string.Empty : part[(equals + 1)..];
                return new QueryItem(DecodeQuery(key), DecodeQuery(value), equals >= 0);
            })
            .ToList();
    }

    private static string DecodeQuery(string value) =>
        Uri.UnescapeDataString(value.Replace('+', ' '));

    private static string FormatFragment(string fragment) =>
        fragment.Length == 0 ? string.Empty : "#" + Uri.EscapeDataString(Uri.UnescapeDataString(fragment));

    private sealed record QueryItem(string Key, string Value, bool HasEquals);
}
