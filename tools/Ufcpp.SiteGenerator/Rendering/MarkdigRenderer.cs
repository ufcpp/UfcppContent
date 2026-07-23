using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Net;
using System.Text.RegularExpressions;
using Ufcpp.SiteGenerator.Models;

namespace Ufcpp.SiteGenerator.Rendering;

/// <summary>Renders Markdown content to HTML using Markdig.</summary>
public sealed class MarkdigRenderer
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        // Use individual extensions instead of UseAdvancedExtensions() to exclude
        // EmphasisExtraExtension which misinterprets ++C++ as <ins>C</ins>.
        .UseAbbreviations()
        .UseAutoIdentifiers()
        .UseCitations()
        .UseCustomContainers()
        .UseDefinitionLists()
        .UseFigures()
        .UseFooters()
        .UseFootnotes()
        .UseGridTables()
        .UseMathematics()
        .UseMediaLinks()
        .UsePipeTables()
        .UseListExtras()
        .UseTaskLists()
        .UseDiagrams()
        .UseAutoLinks()
        .UseGenericAttributes()
        .Use(new SyntaxHighlightingExtension())
        .Build();

    // Matches href/src in raw HTML attributes - bounded, no catastrophic backtracking
    private static readonly Regex HtmlAttrRegex = new(
        @"(?<attr>(?:href|src|data))\s*=\s*(?<q>[""'])(?<url>[^""'\r\n]{0,2048})\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex SourceParamRegex = new(
        @"(?<prefix><param\b(?=[^>]{0,2048}\bname\s*=\s*(?<nq>[""'])source\k<nq>)[^>]{0,2048}?\bvalue\s*=\s*)(?<q>[""'])(?<url>[^""'\r\n]{0,2048})\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex LegacyMarkdownElementRegex = new(
        @"<(?<tag>blockquote|div|td|th)\b(?<attributes>(?=[^>]*\bmarkdown\s*=\s*[""']1[""'])[^>]*)>(?<body>(?:(?!<(?:blockquote|div|td|th)\b(?=[^>]*\bmarkdown\s*=\s*[""']1[""']))[\s\S])*?)</\k<tag>\s*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex MarkdownAttributeRegex = new(
        @"\s+\bmarkdown\s*=\s*(?<q>[""'])1\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex RawTableRegex = new(
        @"<table\b[^>]*>.*?</table\s*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex RawTablePlaceholderRegex = new(
        @"<div data-ufcpp-raw-table-placeholder=""(?<index>\d+)""></div>",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex FencedCodePlaceholderRegex = new(
        @"<div data-ufcpp-fenced-code-placeholder=""(?<index>\d+)""></div>",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex LegacyMarkdownPlaceholderRegex = new(
        @"<div data-ufcpp-legacy-markdown-placeholder=""(?<index>\d+)""></div>",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex FenceLineRegex = new(
        @"^ {0,3}(?<marker>`{3,}|~{3,})(?<remainder>.*)$",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex HeadingElementRegex = new(
        @"<h(?<level>[2-4])\b(?<attributes>[^>]{0,2048})>(?<content>.*?)</h\k<level>\s*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex TitleElementRegex = new(
        @"<h1\b[^>]{0,2048}>.*?</h1>\s*",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex KeywordElementRegex = new(
        @"<(?<tag>strong|span)\b(?<attributes>[^>]{0,2048})>(?<content>.*?)</\k<tag>\s*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex AnchorOpeningTagRegex = new(
        @"<a\b(?<attributes>[^>]{0,2048})>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex IdAttributeRegex = new(
        @"(?:^|\s)id\s*=\s*(?<q>[""'])(?<value>[^""']{1,2048})\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex ClassAttributeRegex = new(
        @"(?:^|\s)class\s*=\s*(?<q>[""'])(?<value>[^""']{1,2048})\k<q>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex HtmlTagRegex = new(
        @"<[^>]{1,2048}>",
        RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex WhitespaceRegex = new(
        @"\s+",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private readonly string _contentRootDirectory;
    private readonly string _assetsRootDirectory;

    public MarkdigRenderer(string contentRootDirectory)
        : this(
            contentRootDirectory,
            Path.Combine(contentRootDirectory, "..", "assets"))
    {
    }

    public MarkdigRenderer(
        string contentRootDirectory,
        string assetsRootDirectory)
    {
        _contentRootDirectory = Path.GetFullPath(contentRootDirectory);
        _assetsRootDirectory = Path.GetFullPath(assetsRootDirectory);
    }

    /// <summary>
    /// Renders the Markdown body of the given page to an HTML string,
    /// rewriting internal links to canonical URLs.
    /// </summary>
    public string Render(ContentPage page, IReadOnlyDictionary<string, string> urlMap) =>
        RenderWithMetadata(page, urlMap).Html;

    /// <summary>
    /// Renders the Markdown body and extracts its heading outline and keyword anchors.
    /// </summary>
    public RenderedContent RenderWithMetadata(
        ContentPage page,
        IReadOnlyDictionary<string, string> urlMap)
    {
        var absoluteFilePath = Path.GetFullPath(
            Path.Combine(_contentRootDirectory, page.RelativePath));

        var rewriter = new LinkRewriter(
            _contentRootDirectory,
            _assetsRootDirectory,
            absoluteFilePath,
            urlMap);

        var (markdownWithoutFences, fencedCodeBlocks) =
            ProtectFencedCodeBlocks(page.MarkdownBody);
        var legacyMarkdownBlocks = new List<string>();
        var markdown = RenderLegacyMarkdownElements(
            markdownWithoutFences,
            fencedCodeBlocks,
            legacyMarkdownBlocks);
        var rawTables = new List<string>();
        markdown = RawTableRegex.Replace(
            markdown,
            match =>
            {
                var index = rawTables.Count;
                rawTables.Add(RestoreProtectedBlocks(match.Value, fencedCodeBlocks));
                return $"<div data-ufcpp-raw-table-placeholder=\"{index}\"></div>";
            });
        markdown = RestoreProtectedBlocks(markdown, fencedCodeBlocks);

        // Parse markdown
        var document = Markdown.Parse(markdown, Pipeline);

        // Walk AST and rewrite links in Markdown syntax nodes
        RewriteAstLinks(document, rewriter);

        // Render to HTML
        var sw = new StringWriter();
        var htmlRenderer = new HtmlRenderer(sw);
        Pipeline.Setup(htmlRenderer);
        htmlRenderer.Render(document);
        sw.Flush();
        var html = sw.ToString();
        html = RawTablePlaceholderRegex.Replace(
            html,
            match => rawTables[int.Parse(
                match.Groups["index"].Value,
                System.Globalization.CultureInfo.InvariantCulture)]);
        html = RestoreLegacyMarkdownBlocks(html, legacyMarkdownBlocks);

        // Rewrite links in raw HTML blocks (bounded regex, safe from backtracking)
        html = HtmlAttrRegex.Replace(html, match =>
        {
            var url = match.Groups["url"].Value;
            var rewritten = rewriter.RewriteUrl(url);
            if (rewritten == url)
            {
                return match.Value;
            }

            var attr = match.Groups["attr"].Value;
            var q = match.Groups["q"].Value;
            return attr + "=" + q + rewritten + q;
        });

        html = SourceParamRegex.Replace(html, match =>
        {
            var url = match.Groups["url"].Value;
            var rewritten = rewriter.RewriteUrl(url);
            if (rewritten == url)
            {
                return match.Value;
            }

            return match.Groups["prefix"].Value
                + match.Groups["q"].Value
                + rewritten
                + match.Groups["q"].Value;
        });

        var (htmlWithHeadingIds, tableOfContents) = BuildTableOfContents(html);
        var (htmlWithKeywordTargets, keywords) = ExtractKeywords(htmlWithHeadingIds);
        var (titleHtml, bodyHtml) = ExtractTitle(htmlWithKeywordTargets);
        return new RenderedContent(
            titleHtml,
            bodyHtml,
            tableOfContents,
            keywords);
    }

    private static (string? TitleHtml, string BodyHtml) ExtractTitle(string html)
    {
        var match = TitleElementRegex.Match(html);
        return match.Success
            ? (match.Value, html.Remove(match.Index, match.Length))
            : (null, html);
    }

    private static (
        string Html,
        IReadOnlyList<TableOfContentsItem> Items) BuildTableOfContents(string html)
    {
        var headings = new List<HeadingItem>();
        var usedHeadingIds = new HashSet<string>(StringComparer.Ordinal);
        var firstIdIndexes = GetFirstIdIndexes(html);
        var existingIds = firstIdIndexes.Keys.ToHashSet(StringComparer.Ordinal);
        var generatedId = 0;

        var htmlWithHeadingIds = HeadingElementRegex.Replace(html, headingMatch =>
        {
            var content = headingMatch.Groups["content"].Value;
            var title = ExtractText(content);
            if (string.IsNullOrWhiteSpace(title))
            {
                return headingMatch.Value;
            }

            var id = FindPreferredHeadingId(
                headingMatch.Groups["attributes"].Value,
                content,
                usedHeadingIds,
                firstIdIndexes,
                headingMatch.Index,
                headingMatch.Index + headingMatch.Length);
            var needsGeneratedId = id is null;
            if (id is null)
            {
                do
                {
                    id = $"sec-generated-toc-{++generatedId}";
                }
                while (!existingIds.Add(id));

                usedHeadingIds.Add(id);
            }

            headings.Add(new HeadingItem(
                int.Parse(
                    headingMatch.Groups["level"].Value,
                    System.Globalization.CultureInfo.InvariantCulture),
                id,
                title));

            return needsGeneratedId
                ? SetHeadingId(headingMatch, id)
                : headingMatch.Value;
        });

        var roots = new List<TableOfContentsItem>();
        var levels = new Stack<(int Level, List<TableOfContentsItem> Items)>();
        levels.Push((1, roots));

        foreach (var heading in headings)
        {
            while (levels.Count > 1 && levels.Peek().Level >= heading.Level)
            {
                levels.Pop();
            }

            var children = new List<TableOfContentsItem>();
            levels.Peek().Items.Add(new TableOfContentsItem(
                BuildFragmentUrl(heading.Id),
                heading.Title,
                children));
            levels.Push((heading.Level, children));
        }

        return (htmlWithHeadingIds, roots);
    }

    private static (
        string Html,
        IReadOnlyList<NavigationItem> Items) ExtractKeywords(string html)
    {
        var keywords = new List<NavigationItem>();
        var ids = new HashSet<string>(StringComparer.Ordinal);
        var firstIdIndexes = GetFirstIdIndexes(html);
        var existingIds = firstIdIndexes.Keys.ToHashSet(StringComparer.Ordinal);
        var generatedId = 0;

        var htmlWithKeywordTargets = KeywordElementRegex.Replace(html, keywordMatch =>
        {
            var attributes = keywordMatch.Groups["attributes"].Value;
            var className = GetAttributeValue(ClassAttributeRegex, attributes);
            if (className is null
                || !className
                    .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                    .Contains("keyword", StringComparer.OrdinalIgnoreCase))
            {
                return keywordMatch.Value;
            }

            var id = GetAttributeValue(IdAttributeRegex, attributes);
            var title = ExtractText(keywordMatch.Groups["content"].Value);
            if (string.IsNullOrWhiteSpace(id)
                || string.IsNullOrWhiteSpace(title)
                || !ids.Add(id))
            {
                return keywordMatch.Value;
            }

            var targetId = id;
            var needsGeneratedTarget = firstIdIndexes.TryGetValue(id, out var firstIndex)
                && firstIndex < keywordMatch.Index;
            if (needsGeneratedTarget)
            {
                do
                {
                    targetId = $"sec-generated-keyword-{++generatedId}";
                }
                while (!existingIds.Add(targetId));
            }

            keywords.Add(new NavigationItem(BuildFragmentUrl(targetId), title));
            return needsGeneratedTarget
                ? InsertKeywordTarget(keywordMatch, targetId)
                : keywordMatch.Value;
        });

        return (htmlWithKeywordTargets, keywords);
    }

    private static string BuildFragmentUrl(string id) =>
        "#" + Uri.EscapeDataString(id);

    private static string? FindPreferredHeadingId(
        string headingAttributes,
        string content,
        ISet<string> usedIds,
        IReadOnlyDictionary<string, int> firstIdIndexes,
        int headingStart,
        int headingEnd)
    {
        var explicitIds = new List<string>();
        foreach (Match anchorMatch in AnchorOpeningTagRegex.Matches(content))
        {
            var id = GetAttributeValue(
                IdAttributeRegex,
                anchorMatch.Groups["attributes"].Value);
            if (!string.IsNullOrWhiteSpace(id))
            {
                explicitIds.Add(id);
            }
        }

        var candidates = explicitIds
            .Where(id => !id.StartsWith(
                "sec-generated-title-",
                StringComparison.OrdinalIgnoreCase))
            .ToList();
        var headingId = GetAttributeValue(IdAttributeRegex, headingAttributes);
        if (!string.IsNullOrWhiteSpace(headingId))
        {
            candidates.Add(headingId);
        }

        candidates.AddRange(explicitIds);
        return candidates.FirstOrDefault(candidate =>
            firstIdIndexes.TryGetValue(candidate, out var firstIndex)
            && firstIndex >= headingStart
            && firstIndex < headingEnd
            && usedIds.Add(candidate));
    }

    private static string? GetAttributeValue(Regex regex, string attributes)
    {
        var match = regex.Match(attributes);
        return match.Success
            ? WebUtility.HtmlDecode(match.Groups["value"].Value).Trim()
            : null;
    }

    private static IReadOnlyDictionary<string, int> GetFirstIdIndexes(string html)
    {
        var indexes = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (Match match in IdAttributeRegex.Matches(html))
        {
            var id = WebUtility.HtmlDecode(match.Groups["value"].Value).Trim();
            if (!string.IsNullOrWhiteSpace(id))
            {
                indexes.TryAdd(id, match.Index);
            }
        }

        return indexes;
    }

    private static string SetHeadingId(Match headingMatch, string id)
    {
        var attributesGroup = headingMatch.Groups["attributes"];
        var attributes = attributesGroup.Value;
        var idMatch = IdAttributeRegex.Match(attributes);
        var updatedAttributes = idMatch.Success
            ? attributes[..idMatch.Groups["value"].Index]
                + id
                + attributes[(idMatch.Groups["value"].Index
                    + idMatch.Groups["value"].Length)..]
            : attributes + $" id=\"{id}\"";
        var attributesOffset = attributesGroup.Index - headingMatch.Index;

        return headingMatch.Value[..attributesOffset]
            + updatedAttributes
            + headingMatch.Value[(attributesOffset + attributesGroup.Length)..];
    }

    private static string InsertKeywordTarget(Match keywordMatch, string id)
    {
        var contentGroup = keywordMatch.Groups["content"];
        var contentOffset = contentGroup.Index - keywordMatch.Index;
        return keywordMatch.Value[..contentOffset]
            + $"<span id=\"{id}\"></span>"
            + keywordMatch.Value[contentOffset..];
    }

    private static string ExtractText(string html)
    {
        var withoutTags = HtmlTagRegex.Replace(html, string.Empty);
        return WhitespaceRegex.Replace(
            WebUtility.HtmlDecode(withoutTags),
            " ").Trim();
    }

    private sealed record HeadingItem(int Level, string Id, string Title);

    private static string RenderLegacyMarkdownElements(
        string markdown,
        IReadOnlyList<string> protectedBlocks,
        List<string> renderedBlocks)
    {
        for (var iteration = 0; iteration < 16; iteration++)
        {
            var replaced = LegacyMarkdownElementRegex.Replace(
                markdown,
                match =>
                {
                    var tag = match.Groups["tag"].Value;
                    var attributes = MarkdownAttributeRegex.Replace(
                        match.Groups["attributes"].Value,
                        string.Empty);
                    var renderedBody = RenderMarkdownFragment(
                        Dedent(match.Groups["body"].Value),
                        protectedBlocks);
                    var renderedElement = $"<{tag}{attributes}>{renderedBody}</{tag}>";
                    var index = renderedBlocks.Count;
                    renderedBlocks.Add(renderedElement);
                    return $"<div data-ufcpp-legacy-markdown-placeholder=\"{index}\"></div>";
                });

            if (string.Equals(replaced, markdown, StringComparison.Ordinal))
            {
                return markdown;
            }

            markdown = replaced;
        }

        throw new InvalidDataException(
            "Legacy markdown HTML nesting exceeded the supported depth.");
    }

    private static string RenderMarkdownFragment(
        string markdown,
        IReadOnlyList<string> protectedBlocks)
    {
        var rawTables = new List<string>();
        markdown = RawTableRegex.Replace(
            markdown,
            match =>
            {
                var index = rawTables.Count;
                rawTables.Add(RestoreProtectedBlocks(match.Value, protectedBlocks));
                return $"<div data-ufcpp-raw-table-placeholder=\"{index}\"></div>";
            });

        var renderedFencedBlocks = new Dictionary<int, string>();
        for (var index = 0; index < protectedBlocks.Count; index++)
        {
            var token = $"\u001AFC{index}\u001A";
            if (!markdown.Contains(token, StringComparison.Ordinal))
            {
                continue;
            }

            renderedFencedBlocks[index] = Markdown.ToHtml(
                    protectedBlocks[index],
                    Pipeline)
                .TrimEnd('\r', '\n');
            markdown = markdown.Replace(
                token,
                $"<div data-ufcpp-fenced-code-placeholder=\"{index}\"></div>",
                StringComparison.Ordinal);
        }

        markdown = RestoreProtectedBlocks(markdown, protectedBlocks);
        var html = Markdown.ToHtml(markdown, Pipeline).TrimEnd('\r', '\n');
        html = RawTablePlaceholderRegex.Replace(
            html,
            match => rawTables[int.Parse(
                match.Groups["index"].Value,
                System.Globalization.CultureInfo.InvariantCulture)]);
        return FencedCodePlaceholderRegex.Replace(
            html,
            match => renderedFencedBlocks[int.Parse(
                match.Groups["index"].Value,
                System.Globalization.CultureInfo.InvariantCulture)]);
    }

    private static string RestoreLegacyMarkdownBlocks(
        string html,
        IReadOnlyList<string> renderedBlocks)
    {
        for (var index = renderedBlocks.Count - 1; index >= 0; index--)
        {
            var currentIndex = index;
            html = LegacyMarkdownPlaceholderRegex.Replace(
                html,
                match => int.Parse(
                        match.Groups["index"].Value,
                        System.Globalization.CultureInfo.InvariantCulture)
                    == currentIndex
                    ? renderedBlocks[currentIndex]
                    : match.Value);
        }

        return html;
    }

    private static (string Markdown, IReadOnlyList<string> Blocks)
        ProtectFencedCodeBlocks(string markdown)
    {
        var lines = markdown
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');
        var output = new List<string>(lines.Length);
        var blocks = new List<string>();

        for (var index = 0; index < lines.Length; index++)
        {
            var opening = FenceLineRegex.Match(lines[index]);
            if (!opening.Success
                || !TryFindClosingFence(lines, index + 1, opening, out var closingIndex))
            {
                output.Add(lines[index]);
                continue;
            }

            var blockIndex = blocks.Count;
            blocks.Add(string.Join('\n', lines[index..(closingIndex + 1)]));
            output.Add($"\u001AFC{blockIndex}\u001A");
            index = closingIndex;
        }

        return (string.Join('\n', output), blocks);
    }

    private static bool TryFindClosingFence(
        IReadOnlyList<string> lines,
        int startIndex,
        Match opening,
        out int closingIndex)
    {
        var openingMarker = opening.Groups["marker"].Value;
        for (var index = startIndex; index < lines.Count; index++)
        {
            var candidate = FenceLineRegex.Match(lines[index]);
            if (candidate.Success
                && candidate.Groups["marker"].Value[0] == openingMarker[0]
                && candidate.Groups["marker"].Value.Length >= openingMarker.Length
                && string.IsNullOrWhiteSpace(candidate.Groups["remainder"].Value))
            {
                closingIndex = index;
                return true;
            }
        }

        closingIndex = -1;
        return false;
    }

    private static string RestoreProtectedBlocks(
        string value,
        IReadOnlyList<string> blocks)
    {
        for (var index = 0; index < blocks.Count; index++)
        {
            value = value.Replace(
                $"\u001AFC{index}\u001A",
                blocks[index],
                StringComparison.Ordinal);
        }

        return value;
    }

    private static string Dedent(string value)
    {
        var lines = value
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n')
            .ToList();
        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[0]))
        {
            lines.RemoveAt(0);
        }

        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[^1]))
        {
            lines.RemoveAt(lines.Count - 1);
        }

        if (lines.Count == 0)
        {
            return string.Empty;
        }

        var indentation = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Min(line => line.TakeWhile(char.IsWhiteSpace).Count());
        return string.Join(
            '\n',
            lines.Select(line =>
                line.Length >= indentation ? line[indentation..] : string.Empty));
    }

    private static void RewriteAstLinks(MarkdownDocument document, LinkRewriter rewriter)
    {
        foreach (var node in document.Descendants())
        {
            if (node is LinkInline link && link.Url is not null)
            {
                link.Url = rewriter.RewriteUrl(link.Url);
            }
        }
    }
}
