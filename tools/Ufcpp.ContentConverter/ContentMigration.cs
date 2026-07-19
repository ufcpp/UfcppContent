using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Ufcpp.ContentConverter;

public sealed record ValidationReport(
    int NodeCount,
    int MarkdownOutputs,
    int CanonicalPages,
    int StructuralIndexes,
    int IntegratedExercises,
    int MetadataNodes,
    int ExcludedNodes,
    int SitemapUrls,
    int CanonicalSitemapMatches,
    int UnknownMacros,
    int KnownMissingRequiredProperties,
    int UnresolvedInternalLinks,
    int ReferencedAssets,
    int MissingAssets,
    int OutputPathCollisions,
    int InvalidPaths,
    int OriginalPageBreakFiles,
    int OriginalPageBreakMarkers);

public sealed class ContentMigration
{
    private static readonly Regex PageBreakRegex = new(
        @"<!--\s*pageBreak\s*-->",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex HeadingAnchorRegex = new(
        @"(?m)^(?<prefix>#{1,6}\s*)<a\s+[^>]*\bid\s*=\s*(?<quote>[""'])(?<id>.*?)\k<quote>[^>]*>(?<title>[^\r\n]*)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex MissingHeadingAnchorRegex = new(
        @"(?m)^(?<prefix>#{1,6}\s*)(?!<a\s+[^>]*\bid\s*=)(?<title>.+?)\s*#*\s*$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex AnchorAttributeRegex = new(
        @"\b(?:id|name)\s*=\s*[""'](?<id>[^""']+)[""']",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex ProtectedBlockRegex = new(
        @"<pre\b[^>]*>.*?</pre>|<code\b[^>]*>.*?</code>|```.*?```|~~~.*?~~~",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex GeneratedMarkdownLinkRegex = new(
        @"\]\(\s*(?:<)?(?<url>[^\s)>]+)(?:>)?(?:\s+[""'][^""']*[""'])?\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex GeneratedReferenceLinkRegex = new(
        @"(?m)^\s*\[(?!\[)[^\]\r\n]+\]:\s+(?:<)?(?<url>[^\s>]+)(?:>)?",
        RegexOptions.Compiled);

    private static readonly Regex GeneratedHtmlLinkRegex = new(
        @"<(?:a|img|source|video|audio|object|embed|script|link)\b[^>]*?\b(?:href|src|poster|data)\s*=\s*[""'](?<url>.*?)[""']",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex GeneratedAutolinkRegex = new(
        @"<(?<url>(?:https?://|\.\.?/)[^>\s]+)>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>>
        LegacyFragmentAliases =
            new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["/study/csharp/cheatsheet/langversionoption/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["langversion"] = "option",
                        ["new-options"] = "default",
                        ["explict"] = "explicit",
                    },
                ["/study/csharp/datatype/patterns/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["discards"] = "discard",
                    },
                ["/study/csharp/oop/oo_class/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["partial_class"] = "partial",
                    },
                ["/study/csharp/resource/refstruct/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["scoped"] = "scoped-modifier",
                    },
                ["/study/csharp/start/st_string/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["FormatableString"] = "FormattableString",
                    },
                ["/study/csharp/interop/sp_pinvoke/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["COM"] = "com",
                    },
                ["/study/sp/dsp/fourier/"] =
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["covolution"] = "convolution",
                    },
            };

    private readonly MigrationOptions _options;
    private PublishedSnapshot _snapshot = null!;
    private IReadOnlyDictionary<int, string> _canonicalUrls = null!;
    private IReadOnlyDictionary<int, string> _outputPaths = null!;
    private IReadOnlyDictionary<int, IReadOnlyList<string>> _aliases = null!;
    private IReadOnlyDictionary<string, string> _categoryAnchors = null!;
    private MacroExpander _macros = null!;
    private LinkRewriter _links = null!;
    private AssetManager _assets = null!;
    private readonly HashSet<int> _pageBreakNodes = [];
    private int _pageBreakMarkers;

    public ContentMigration(MigrationOptions options)
    {
        _options = options;
    }

    public ValidationReport Run()
    {
        _snapshot = PublishedContentParser.Load(_options.SnapshotPath);
        _canonicalUrls = _snapshot.Nodes.ToDictionary(node => node.Id, ContentPaths.CanonicalUrl);
        _outputPaths = _snapshot.Nodes
            .Select(node => (Node: node, Path: ContentPaths.OutputPath(node)))
            .Where(item => item.Path is not null)
            .ToDictionary(item => item.Node.Id, item => item.Path!);
        ContentPaths.ValidateNoCollisions(_outputPaths.Values);

        var rewriteMaps = RewriteMapCatalog.Load(_options.RewriteMapsPath);
        if (_options.StrictAccounting)
        {
            rewriteMaps.ValidateExpectedCounts();
        }

        _aliases = rewriteMaps.BuildAliases(_snapshot.Nodes, _canonicalUrls);
        _categoryAnchors = BuildCategoryAnchors(_snapshot.Nodes);
        PrepareOutput();
        _assets = new AssetManager(_options.MediaRoot, _options.LegacyRoot, _options.OutputRoot);
        _macros = new MacroExpander(_snapshot.Nodes, _canonicalUrls);
        _links = new LinkRewriter(
            _snapshot.Nodes,
            _canonicalUrls,
            _outputPaths,
            _aliases,
            _categoryAnchors,
            _assets);

        foreach (var node in _snapshot.Nodes
                     .Where(node => _outputPaths.ContainsKey(node.Id))
                     .OrderBy(node => _outputPaths[node.Id], StringComparer.Ordinal))
        {
            WriteMarkdown(node);
        }

        var sitemapUrls = LoadSitemap(_options.SitemapPath);
        var canonicalUrls = _snapshot.Nodes
            .Where(node => ContentPaths.SitemapSnapshotTypes.Contains(node.ContentType))
            .Select(node => "https://ufcpp.net" + _canonicalUrls[node.Id])
            .ToHashSet(StringComparer.Ordinal);
        var sitemapMatches = canonicalUrls.Intersect(sitemapUrls, StringComparer.Ordinal).Count();
        var report = BuildReport(sitemapUrls.Count, sitemapMatches);
        ValidateReport(report, canonicalUrls, sitemapUrls);
        WriteCatalogs(report);
        ValidateGeneratedFiles();
        ValidateGeneratedLinks();
        return report;
    }

    private void PrepareOutput()
    {
        Directory.CreateDirectory(_options.OutputRoot);
        foreach (var directory in new[] { "content", "assets", "catalog" })
        {
            var path = Path.Combine(_options.OutputRoot, directory);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }
    }

    private void WriteMarkdown(ContentNode node)
    {
        var body = BuildBody(node);
        var markdown = TextUtilities.NormalizeMarkdownHeadingSpacing(
            BuildFrontMatter(node) + "\n# " + EscapeMarkdown(PageTitle(node)) + "\n\n" + body);
        var outputPath = Path.Combine(
            _options.OutputRoot,
            _outputPaths[node.Id].Replace('/', Path.DirectorySeparatorChar));
        TextUtilities.WriteText(outputPath, markdown);
    }

    private string BuildBody(ContentNode node)
    {
        string body = node.ContentType switch
        {
            "Article" => ProcessFragment(node.Get("bodyText"), node)
                + ExerciseRenderer.RenderForArticle(node, ProcessFragment),
            "BlogEntry" or "AboutMe" => ProcessFragment(node.Get("bodyText"), node),
            "ExerciseList" => ExerciseRenderer.RenderForList(node, ProcessFragment),
            "Home" => BuildHome(node),
            "StudyTop" => BuildStudyTop(node),
            "Subject" => BuildSubject(node),
            "Chapter" => BuildChapter(node),
            "BlogTop" => BuildBlogTop(node),
            "BlogYear" => BuildBlogYear(node),
            "BlogMonth" => BuildBlogMonth(node),
            "Search" => BuildSearch(node),
            "Sitemap" => BuildSitemap(node),
            _ => throw new InvalidDataException($"No Markdown renderer for '{node.ContentType}'."),
        };

        return TextUtilities.NormalizeNewlines(body).Trim();
    }

    private string ProcessFragment(string value, ContentNode context)
    {
        var isPrimaryArticleBody = context.ContentType == "Article"
            && value.Equals(context.Get("bodyText"), StringComparison.Ordinal);
        value = TextUtilities.NormalizeNewlines(value);
        var pageBreakCount = PageBreakRegex.Matches(value).Count;
        _pageBreakMarkers += pageBreakCount;
        if (pageBreakCount != 0)
        {
            _pageBreakNodes.Add(context.Id);
        }

        value = PageBreakRegex.Replace(value, "<!-- original-page-break -->");
        value = NormalizeHeadingAnchors(value, isPrimaryArticleBody);
        if (isPrimaryArticleBody)
        {
            value = AddLegacyFragmentAliases(value, context);
        }

        value = _macros.Expand(value, context);
        value = _links.Rewrite(value, context);
        return CodeBlockNormalizer.Normalize(value, ContentPaths.CanonicalUrl(context));
    }

    private static string NormalizeHeadingAnchors(string value, bool generateMissingAnchors)
    {
        var protectedBlocks = new List<string>();
        value = ProtectedBlockRegex.Replace(
            value,
            match =>
            {
                var marker = $"\u001A{protectedBlocks.Count}\u001A";
                protectedBlocks.Add(match.Value);
                return marker;
            });
        value = HeadingAnchorRegex.Replace(
            value,
            match => match.Groups["title"].Value.Contains("</a>", StringComparison.OrdinalIgnoreCase)
                ? match.Value
                : $"{match.Groups["prefix"].Value}<a id={match.Groups["quote"].Value}" +
                  $"{match.Groups["id"].Value}{match.Groups["quote"].Value}></a>" +
                  match.Groups["title"].Value.TrimStart());
        if (generateMissingAnchors)
        {
            var generatedAnchor = 0;
            value = MissingHeadingAnchorRegex.Replace(
                value,
                match =>
                    $"{match.Groups["prefix"].Value}<a id=\"sec-generated-title-{++generatedAnchor}\"></a>" +
                    match.Groups["title"].Value);
        }

        for (var index = 0; index < protectedBlocks.Count; index++)
        {
            value = value.Replace($"\u001A{index}\u001A", protectedBlocks[index], StringComparison.Ordinal);
        }

        return value;
    }

    private static string AddLegacyFragmentAliases(string value, ContentNode context)
    {
        if (!LegacyFragmentAliases.TryGetValue(ContentPaths.CanonicalUrl(context), out var aliases))
        {
            return value;
        }

        foreach (var alias in aliases)
        {
            if (AnchorAttributeRegex.Matches(value)
                .Any(match => match.Groups["id"].Value.Equals(alias.Key, StringComparison.Ordinal)))
            {
                continue;
            }

            var target = new Regex(
                $@"(?i:<a\s+[^>]*\bid\s*=\s*)(?<quote>[""']){Regex.Escape(alias.Value)}\k<quote>(?i:[^>]*>)");
            if (!target.IsMatch(value))
            {
                throw new InvalidDataException(
                    $"Legacy fragment alias '{alias.Key}' target '{alias.Value}' is absent in node {context.Id}.");
            }

            value = target.Replace(value, $"<a id=\"{alias.Key}\"></a>$0", 1);
        }

        return value;
    }

    private string BuildFrontMatter(ContentNode node)
    {
        var builder = new StringBuilder();
        builder.AppendLine("---");
        builder.AppendLine($"title: {TextUtilities.YamlQuote(PageTitle(node))}");
        builder.AppendLine($"source_url: {TextUtilities.YamlQuote("https://ufcpp.net" + _canonicalUrls[node.Id])}");
        builder.AppendLine($"content_type: {TextUtilities.YamlQuote(node.ContentType)}");
        builder.AppendLine($"published_at: {TextUtilities.YamlQuote(node.PublishedAt)}");
        builder.AppendLine($"updated_at: {TextUtilities.YamlQuote(node.UpdatedAt)}");
        AppendArray(builder, "tags", node.Tags);
        builder.AppendLine($"umbraco_id: {node.Id}");
        builder.AppendLine($"parent_id: {node.ParentId}");
        builder.AppendLine($"sort_order: {node.SortOrder}");
        AppendArray(builder, "aliases", _aliases[node.Id]);
        builder.AppendLine("---");
        return builder.ToString();
    }

    private static void AppendArray(StringBuilder builder, string name, IReadOnlyList<string> values)
    {
        if (values.Count == 0)
        {
            builder.AppendLine($"{name}: []");
            return;
        }

        builder.AppendLine($"{name}:");
        foreach (var value in values)
        {
            builder.AppendLine($"  - {TextUtilities.YamlQuote(value)}");
        }
    }

    private string BuildHome(ContentNode node)
    {
        var builder = new StringBuilder();
        var description = node.Get("description");
        if (description.Length != 0)
        {
            builder.AppendLine(ProcessFragment(description, node));
            builder.AppendLine();
        }

        builder.AppendLine("## 学習コンテンツ");
        builder.AppendLine();
        foreach (var subject in _snapshot.Nodes
                     .Where(item => item.ContentType == "Subject")
                     .OrderBy(item => item.SortOrder)
                     .ThenBy(item => item.Id))
        {
            builder.AppendLine($"- [{EscapeMarkdown(subject.Title)}]({_canonicalUrls[subject.Id]})");
        }

        builder.AppendLine();
        builder.AppendLine("## その他");
        builder.AppendLine();
        foreach (var item in _snapshot.Nodes
                     .Where(item => item.ContentType is "BlogTop" or "AboutMe")
                     .OrderBy(item => _canonicalUrls[item.Id], StringComparer.Ordinal))
        {
            builder.AppendLine($"- [{EscapeMarkdown(item.Title)}]({_canonicalUrls[item.Id]})");
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildStudyTop(ContentNode node)
    {
        var builder = new StringBuilder();
        builder.AppendLine("公開されている学習分野の一覧です。");
        builder.AppendLine();
        foreach (var subject in _snapshot.Nodes
                     .Where(item => item.ContentType == "Subject")
                     .OrderBy(item => item.SortOrder)
                     .ThenBy(item => item.Id))
        {
            builder.AppendLine($"- [{EscapeMarkdown(subject.Title)}]({_canonicalUrls[subject.Id]})");
            var description = subject.Get("description").Trim();
            if (description.Length != 0)
            {
                builder.AppendLine($"  - {description}");
            }
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildSubject(ContentNode node)
    {
        var builder = new StringBuilder();
        var introduction = node.Get("introduction");
        if (introduction.Length == 0)
        {
            introduction = node.Get("description");
        }

        if (introduction.Length != 0)
        {
            builder.AppendLine(introduction.Trim());
            builder.AppendLine();
        }

        builder.AppendLine("## 章");
        builder.AppendLine();
        foreach (var chapter in node.Children
                     .Where(child => child.ContentType == "Chapter")
                     .OrderBy(child => child.SortOrder)
                     .ThenBy(child => child.Id))
        {
            builder.AppendLine(
                $"### <a id=\"{WebUtility.HtmlEncode(chapter.NodeName)}\"></a>" +
                $"[{EscapeMarkdown(chapter.Title)}]({_canonicalUrls[chapter.Id]})");
            builder.AppendLine();
            foreach (var child in chapter.Children
                         .Where(child => child.ContentType is "Article" or "ExerciseList")
                         .OrderBy(child => child.SortOrder)
                         .ThenBy(child => child.Id))
            {
                builder.AppendLine($"- [{EscapeMarkdown(child.Title)}]({_canonicalUrls[child.Id]})");
            }

            builder.AppendLine();
        }

        AppendRelatedLinks(builder, node);
        return ProcessFragment(builder.ToString(), node);
    }

    private void AppendRelatedLinks(StringBuilder builder, ContentNode node)
    {
        var raw = node.Get("relatedLinks");
        if (string.IsNullOrWhiteSpace(raw) || raw.Trim() == "[]")
        {
            return;
        }

        using var document = JsonDocument.Parse(raw);
        if (document.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException($"Subject {node.Id} relatedLinks must be a JSON array.");
        }

        builder.AppendLine("## 関連ページ");
        builder.AppendLine();
        foreach (var item in document.RootElement.EnumerateArray())
        {
            var caption = item.GetProperty("caption").GetString()
                ?? throw new InvalidDataException($"Subject {node.Id} related link has no caption.");
            var isInternal = item.GetProperty("isInternal").GetBoolean();
            string url;
            if (isInternal)
            {
                var targetId = item.GetProperty("internal").GetInt32();
                if (!_canonicalUrls.TryGetValue(targetId, out url!))
                {
                    throw new InvalidDataException(
                        $"Subject {node.Id} related link targets missing node {targetId}.");
                }
            }
            else
            {
                url = item.GetProperty("link").GetString()
                    ?? throw new InvalidDataException($"Subject {node.Id} external related link has no URL.");
            }

            builder.AppendLine($"- [{EscapeMarkdown(caption)}]({url})");
        }

        builder.AppendLine();
    }

    private string BuildChapter(ContentNode node)
    {
        var builder = new StringBuilder();
        builder.AppendLine("この章の記事一覧です。");
        builder.AppendLine();
        foreach (var article in node.Children
                     .Where(child => child.ContentType is "Article" or "ExerciseList")
                     .OrderBy(child => child.SortOrder)
                     .ThenBy(child => child.Id))
        {
            builder.AppendLine($"- [{EscapeMarkdown(article.Title)}]({_canonicalUrls[article.Id]})");
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildBlogTop(ContentNode node)
    {
        var entries = _snapshot.Nodes
            .Where(item => item.ContentType == "BlogEntry")
            .OrderByDescending(item => item.PublishedAt, StringComparer.Ordinal)
            .ThenBy(item => item.Id)
            .ToArray();
        var builder = new StringBuilder();
        builder.AppendLine("## 最新の投稿");
        builder.AppendLine();
        foreach (var entry in entries.Take(20))
        {
            builder.AppendLine($"- {entry.PublishedAt[..10]} [{EscapeMarkdown(entry.Title)}]({_canonicalUrls[entry.Id]})");
        }

        builder.AppendLine();
        builder.AppendLine("## 年別");
        builder.AppendLine();
        foreach (var year in node.Children
                     .Where(child => child.ContentType == "BlogYear")
                     .OrderByDescending(child => child.NodeName, StringComparer.Ordinal))
        {
            builder.AppendLine($"- [{EscapeMarkdown(year.NodeName + " 年")} ]({_canonicalUrls[year.Id]})");
        }

        builder.AppendLine();
        builder.AppendLine("## カテゴリ");
        builder.AppendLine();
        foreach (var category in _categoryAnchors.OrderBy(item => item.Key, StringComparer.Ordinal))
        {
            var search = _snapshot.Nodes.Single(item => item.ContentType == "Search");
            builder.AppendLine(
                $"- [{EscapeMarkdown(category.Key)}]({_canonicalUrls[search.Id]}?bc={Uri.EscapeDataString(category.Key)})");
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildBlogYear(ContentNode node)
    {
        var builder = new StringBuilder();
        foreach (var month in node.Children
                     .Where(child => child.ContentType == "BlogMonth")
                     .OrderBy(child => ParseNumber(child.NodeName)))
        {
            builder.AppendLine($"## {EscapeMarkdown(month.Title)}");
            builder.AppendLine();
            foreach (var entry in month.Children
                         .Where(child => child.ContentType == "BlogEntry")
                         .OrderBy(child => child.PublishedAt, StringComparer.Ordinal)
                         .ThenBy(child => child.Id))
            {
                builder.AppendLine(
                    $"- {entry.PublishedAt[..10]} [{EscapeMarkdown(entry.Title)}]({_canonicalUrls[entry.Id]})");
            }

            builder.AppendLine();
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildBlogMonth(ContentNode node)
    {
        var builder = new StringBuilder();
        foreach (var entry in node.Children
                     .Where(child => child.ContentType == "BlogEntry")
                     .OrderBy(child => child.PublishedAt, StringComparer.Ordinal)
                     .ThenBy(child => child.Id))
        {
            builder.AppendLine(
                $"- {entry.PublishedAt[..10]} [{EscapeMarkdown(entry.Title)}]({_canonicalUrls[entry.Id]})");
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildSearch(ContentNode node)
    {
        var builder = new StringBuilder();
        builder.AppendLine("静的移行時点のコンテンツ索引です。");
        builder.AppendLine();
        builder.AppendLine("## ブログカテゴリ");
        builder.AppendLine();
        foreach (var category in _categoryAnchors.OrderBy(item => item.Key, StringComparer.Ordinal))
        {
            builder.AppendLine($"### <a id=\"{category.Value}\"></a>{EscapeMarkdown(category.Key)}");
            builder.AppendLine();
            foreach (var entry in _snapshot.Nodes
                         .Where(item => item.ContentType == "BlogEntry"
                             && item.Tags.Contains(category.Key, StringComparer.Ordinal))
                         .OrderByDescending(item => item.PublishedAt, StringComparer.Ordinal)
                         .ThenBy(item => item.Id))
            {
                builder.AppendLine(
                    $"- {entry.PublishedAt[..10]} [{EscapeMarkdown(entry.Title)}]({_canonicalUrls[entry.Id]})");
            }

            builder.AppendLine();
        }

        builder.AppendLine("## 全ページ");
        builder.AppendLine();
        foreach (var item in _snapshot.Nodes
                     .Where(item => ContentPaths.CanonicalSitemapTypes.Contains(item.ContentType))
                     .OrderBy(item => item.Title, StringComparer.Ordinal)
                     .ThenBy(item => item.Id))
        {
            builder.AppendLine($"- [{EscapeMarkdown(item.Title)}]({_canonicalUrls[item.Id]})");
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private string BuildSitemap(ContentNode node)
    {
        var builder = new StringBuilder();
        builder.AppendLine("公開ページの静的サイトマップです。");
        builder.AppendLine();
        foreach (var item in _snapshot.Nodes
                     .Where(item => ContentPaths.CanonicalSitemapTypes.Contains(item.ContentType))
                     .OrderBy(item => _canonicalUrls[item.Id], StringComparer.Ordinal))
        {
            var depth = _canonicalUrls[item.Id].Count(character => character == '/') - 1;
            builder.Append(' ', Math.Max(0, depth) * 2);
            builder.AppendLine($"- [{EscapeMarkdown(item.Title)}]({_canonicalUrls[item.Id]})");
        }

        return ProcessFragment(builder.ToString(), node);
    }

    private ValidationReport BuildReport(int sitemapUrls, int sitemapMatches)
    {
        var generated = _snapshot.Nodes.Count(node => ContentPaths.GeneratedTypes.Contains(node.ContentType));
        return new ValidationReport(
            _snapshot.Nodes.Count,
            generated,
            _snapshot.Nodes.Count(node => ContentPaths.CanonicalSitemapTypes.Contains(node.ContentType)),
            _snapshot.Nodes.Count(node => node.ContentType is "StudyTop" or "Chapter" or "BlogMonth"),
            _snapshot.Nodes.Count(node => node.ContentType == "Exercise"),
            _snapshot.Nodes.Count(node => node.ContentType == "SubjectGroup"),
            _snapshot.Nodes.Count(node => node.ContentType
                is "Settings" or "Rss" or "RssBlog" or "ErrorNotFound" or "ErrorServer"),
            sitemapUrls,
            sitemapMatches,
            0,
            _snapshot.Nodes.Sum(node => node.KnownMissingProperties.Count),
            0,
            _assets.Records.Count,
            0,
            0,
            0,
            _pageBreakNodes.Count,
            _pageBreakMarkers);
    }

    private void ValidateReport(
        ValidationReport report,
        IReadOnlySet<string> canonicalUrls,
        IReadOnlySet<string> sitemapUrls)
    {
        if (!canonicalUrls.SetEquals(sitemapUrls))
        {
            var missing = sitemapUrls.Except(canonicalUrls, StringComparer.Ordinal).Take(10);
            var extra = canonicalUrls.Except(sitemapUrls, StringComparer.Ordinal).Take(10);
            throw new InvalidDataException(
                $"Canonical sitemap mismatch. Missing: {string.Join(", ", missing)}. " +
                $"Extra: {string.Join(", ", extra)}.");
        }

        if (!_options.StrictAccounting)
        {
            return;
        }

        var expected = new ValidationReport(
            1150,
            1107,
            928,
            179,
            34,
            4,
            5,
            928,
            928,
            0,
            1,
            0,
            815,
            0,
            0,
            0,
            21,
            42);
        foreach (var property in typeof(ValidationReport).GetProperties())
        {
            var actualValue = property.GetValue(report);
            var expectedValue = property.GetValue(expected);
            if (!Equals(actualValue, expectedValue))
            {
                throw new InvalidDataException(
                    $"Validation count {property.Name} was {actualValue}; expected {expectedValue}.");
            }
        }
    }

    private void WriteCatalogs(ValidationReport report)
    {
        var entries = _snapshot.Nodes
            .OrderBy(node => node.Id)
            .Select(node =>
            {
                var status = node.ContentType switch
                {
                    "Exercise" => "integrated",
                    "SubjectGroup" => "metadata",
                    "Settings" or "Rss" or "RssBlog" or "ErrorNotFound" or "ErrorServer" => "excluded",
                    _ => "generated",
                };
                string? outputPath = _outputPaths.GetValueOrDefault(node.Id);
                if (node.ContentType == "Exercise")
                {
                    outputPath = node.Parent is null ? null : _outputPaths.GetValueOrDefault(node.Parent.Id);
                }

                return new
                {
                    umbraco_id = node.Id,
                    parent_id = node.ParentId,
                    content_type = node.ContentType,
                    node_name = node.NodeName,
                    title = node.Title,
                    canonical_url = _canonicalUrls[node.Id],
                    output_path = outputPath,
                    status,
                    aliases = _aliases[node.Id],
                    metadata = node.ContentType == "SubjectGroup"
                        ? new Dictionary<string, string> { ["display_name"] = node.Get("displayName") }
                        : null,
                };
            })
            .ToArray();
        TextUtilities.WriteJson(
            Path.Combine(_options.OutputRoot, "catalog", "content-map.json"),
            new { schema_version = 1, entries });

        var assets = _assets.Records
            .OrderBy(asset => asset.OutputPath, StringComparer.Ordinal)
            .Select(asset => new
            {
                original_url = asset.OriginalUrl,
                output_path = asset.OutputPath,
                source_kind = asset.SourceKind,
                source_relative_path = asset.SourceRelativePath,
                bytes = asset.Bytes,
                sha256 = asset.Sha256,
            })
            .ToArray();
        TextUtilities.WriteJson(
            Path.Combine(_options.OutputRoot, "catalog", "asset-map.json"),
            new { schema_version = 1, assets });

        TextUtilities.WriteJson(
            Path.Combine(_options.OutputRoot, "catalog", "source-snapshot.json"),
            new
            {
                schema_version = 1,
                published_xml_sha256 = TextUtilities.Sha256File(_options.SnapshotPath),
                sitemap_sha256 = TextUtilities.Sha256File(_options.SitemapPath),
                rewrite_maps_sha256 = TextUtilities.Sha256File(_options.RewriteMapsPath),
                nodes = _snapshot.Nodes.Count,
                sitemap_urls = report.SitemapUrls,
                media_archive_files = Directory.EnumerateFiles(_options.MediaRoot, "*", SearchOption.AllDirectories).Count(),
            });
        TextUtilities.WriteJson(
            Path.Combine(_options.OutputRoot, "catalog", "validation-report.json"),
            new
            {
                schema_version = 1,
                node_count = report.NodeCount,
                markdown_outputs = report.MarkdownOutputs,
                canonical_pages = report.CanonicalPages,
                structural_indexes = report.StructuralIndexes,
                integrated_exercises = report.IntegratedExercises,
                metadata_nodes = report.MetadataNodes,
                excluded_nodes = report.ExcludedNodes,
                sitemap_urls = report.SitemapUrls,
                canonical_sitemap_matches = report.CanonicalSitemapMatches,
                unknown_macros = report.UnknownMacros,
                known_missing_required_properties = report.KnownMissingRequiredProperties,
                unresolved_internal_links = report.UnresolvedInternalLinks,
                referenced_assets = report.ReferencedAssets,
                missing_assets = report.MissingAssets,
                output_path_collisions = report.OutputPathCollisions,
                invalid_paths = report.InvalidPaths,
                original_page_break_files = report.OriginalPageBreakFiles,
                original_page_break_markers = report.OriginalPageBreakMarkers,
                source_anomalies = _snapshot.Nodes
                    .SelectMany(node => node.KnownMissingProperties.Select(
                        property => new
                        {
                            umbraco_id = node.Id,
                            content_type = node.ContentType,
                            missing_property = property,
                            handling = "Generated with an empty value.",
                        }))
                    .ToArray(),
                sitemap_runtime_urls = new[] { "/404/", "/500/" },
                generated_utility_urls_not_in_sitemap = new[] { "/search/", "/sitemap/" },
            });
    }

    private void ValidateGeneratedFiles()
    {
        foreach (var path in Directory.EnumerateFiles(_options.OutputRoot, "*", SearchOption.AllDirectories))
        {
            if (new FileInfo(path).Length >= 100_000_000)
            {
                throw new InvalidDataException($"Generated file exceeds GitHub's 100 MB limit: '{path}'.");
            }
        }

        foreach (var path in _outputPaths.Values)
        {
            var fullPath = Path.Combine(_options.OutputRoot, path.Replace('/', Path.DirectorySeparatorChar));
            var bytes = File.ReadAllBytes(fullPath);
            _ = TextUtilities.Utf8NoBom.GetString(bytes);
            if (bytes.Length < 4 || Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 4)).StartsWith('\uFEFF'))
            {
                throw new InvalidDataException($"Markdown must be UTF-8 without BOM: '{path}'.");
            }

            var text = TextUtilities.Utf8NoBom.GetString(bytes);
            if (!text.StartsWith("---\n", StringComparison.Ordinal)
                || text.IndexOf("\n---\n", 4, StringComparison.Ordinal) < 0)
            {
                throw new InvalidDataException($"Markdown front matter is malformed: '{path}'.");
            }

            ValidateFrontMatter(path, text);
        }
    }

    private void ValidateGeneratedLinks()
    {
        var outputRoot = Path.GetFullPath(_options.OutputRoot).TrimEnd(Path.DirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        var anchors = _outputPaths.Values.ToDictionary(
            path => Path.GetFullPath(
                Path.Combine(_options.OutputRoot, path.Replace('/', Path.DirectorySeparatorChar))),
            path => AnchorAttributeRegex.Matches(
                    File.ReadAllText(
                        Path.Combine(_options.OutputRoot, path.Replace('/', Path.DirectorySeparatorChar)),
                        TextUtilities.Utf8NoBom))
                .Select(match => WebUtility.HtmlDecode(match.Groups["id"].Value))
                .ToHashSet(StringComparer.Ordinal),
            StringComparer.OrdinalIgnoreCase);
        foreach (var relativePath in _outputPaths.Values)
        {
            var fullPath = Path.Combine(
                _options.OutputRoot,
                relativePath.Replace('/', Path.DirectorySeparatorChar));
            var markdown = File.ReadAllText(fullPath, TextUtilities.Utf8NoBom);
            markdown = ProtectedBlockRegex.Replace(markdown, string.Empty);
            var urls = GeneratedMarkdownLinkRegex.Matches(markdown)
                .Concat(GeneratedReferenceLinkRegex.Matches(markdown))
                .Concat(GeneratedHtmlLinkRegex.Matches(markdown))
                .Concat(GeneratedAutolinkRegex.Matches(markdown))
                .Select(match => WebUtility.HtmlDecode(match.Groups["url"].Value))
                .Distinct(StringComparer.Ordinal);
            foreach (var url in urls)
            {
                ValidateGeneratedLink(outputRoot, relativePath, url, anchors);
            }
        }
    }

    private static void ValidateGeneratedLink(
        string outputRoot,
        string sourcePath,
        string rawUrl,
        IReadOnlyDictionary<string, HashSet<string>> anchors)
    {
        var url = rawUrl.Trim();
        if (url.Length == 0
            || url.StartsWith("//", StringComparison.Ordinal)
            || Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            return;
        }

        var parts = url.Split('#', 2);
        var path = parts[0].Split('?', 2)[0];
        var fragment = parts.Length == 2 ? Uri.UnescapeDataString(parts[1]) : string.Empty;
        if (path.Length == 0)
        {
            if (fragment.Length != 0)
            {
                var sourceFullPath = Path.GetFullPath(
                    Path.Combine(outputRoot, sourcePath.Replace('/', Path.DirectorySeparatorChar)));
                ValidateFragment(sourcePath, rawUrl, sourceFullPath, fragment, anchors);
            }

            return;
        }

        if (path.StartsWith('/'))
        {
            throw new InvalidDataException(
                $"Generated Markdown retains root-relative URL '{rawUrl}' in '{sourcePath}'.");
        }

        var decodedPath = Uri.UnescapeDataString(path);
        var sourceDirectory = Path.GetDirectoryName(
            Path.Combine(outputRoot, sourcePath.Replace('/', Path.DirectorySeparatorChar)))!;
        var target = Path.GetFullPath(
            Path.Combine(sourceDirectory, decodedPath.Replace('/', Path.DirectorySeparatorChar)));
        if (!target.StartsWith(outputRoot, StringComparison.OrdinalIgnoreCase)
            || !File.Exists(target))
        {
            throw new InvalidDataException(
                $"Generated link '{rawUrl}' in '{sourcePath}' does not resolve to a repository file.");
        }

        if (fragment.Length != 0
            && Path.GetExtension(target).Equals(".md", StringComparison.OrdinalIgnoreCase))
        {
            ValidateFragment(sourcePath, rawUrl, target, fragment, anchors);
        }
    }

    private static void ValidateFragment(
        string sourcePath,
        string rawUrl,
        string target,
        string fragment,
        IReadOnlyDictionary<string, HashSet<string>> anchors)
    {
        if (!anchors.TryGetValue(target, out var targetAnchors) || !targetAnchors.Contains(fragment))
        {
            throw new InvalidDataException(
                $"Generated link '{rawUrl}' in '{sourcePath}' targets missing fragment '{fragment}'.");
        }
    }

    private static void ValidateFrontMatter(string path, string text)
    {
        var end = text.IndexOf("\n---\n", 4, StringComparison.Ordinal);
        var lines = text[4..end].Split('\n');
        var scalarNames = new[]
        {
            "title",
            "source_url",
            "content_type",
            "published_at",
            "updated_at",
        };
        foreach (var name in scalarNames)
        {
            var prefix = name + ": ";
            var line = lines.SingleOrDefault(value => value.StartsWith(prefix, StringComparison.Ordinal))
                ?? throw new InvalidDataException($"Front matter in '{path}' is missing '{name}'.");
            try
            {
                _ = JsonSerializer.Deserialize<string>(line[prefix.Length..])
                    ?? throw new JsonException();
            }
            catch (JsonException exception)
            {
                throw new InvalidDataException(
                    $"Front matter string '{name}' in '{path}' is not safely quoted.",
                    exception);
            }
        }

        foreach (var name in new[] { "umbraco_id", "parent_id", "sort_order" })
        {
            var prefix = name + ": ";
            var line = lines.SingleOrDefault(value => value.StartsWith(prefix, StringComparison.Ordinal));
            if (line is null || !int.TryParse(line[prefix.Length..], out _))
            {
                throw new InvalidDataException($"Front matter integer '{name}' in '{path}' is invalid.");
            }
        }

        foreach (var name in new[] { "tags", "aliases" })
        {
            if (!lines.Any(value =>
                    value.Equals(name + ": []", StringComparison.Ordinal)
                    || value.Equals(name + ":", StringComparison.Ordinal)))
            {
                throw new InvalidDataException($"Front matter array '{name}' in '{path}' is invalid.");
            }
        }
    }

    private static HashSet<string> LoadSitemap(string path)
    {
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore,
            XmlResolver = null,
        };
        using var reader = XmlReader.Create(path, settings);
        var document = XDocument.Load(reader);
        var urls = document.Descendants()
            .Where(element => element.Name.LocalName == "loc")
            .Select(element => element.Value.Trim())
            .ToArray();
        var set = urls.ToHashSet(StringComparer.Ordinal);
        if (set.Count != urls.Length)
        {
            throw new InvalidDataException("The sitemap contains duplicate URLs.");
        }

        return set;
    }

    private static IReadOnlyDictionary<string, string> BuildCategoryAnchors(
        IEnumerable<ContentNode> nodes) =>
        nodes.Where(node => node.ContentType == "BlogEntry")
            .SelectMany(node => node.Tags)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToDictionary(
                category => category,
                category => "blog-category-" + TextUtilities.Sha256Text(category)[..12],
                StringComparer.Ordinal);

    private static int ParseNumber(string value) => int.TryParse(value, out var number) ? number : int.MaxValue;

    private string PageTitle(ContentNode node)
    {
        if (node.ContentType != "Home")
        {
            return node.Title;
        }

        var siteTitle = _snapshot.Nodes.SingleOrDefault(item => item.ContentType == "Settings")
            ?.Get("siteTitle");
        return string.IsNullOrWhiteSpace(siteTitle) ? node.Title : siteTitle;
    }

    private static string EscapeMarkdown(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("[", "\\[", StringComparison.Ordinal)
            .Replace("]", "\\]", StringComparison.Ordinal);
}
