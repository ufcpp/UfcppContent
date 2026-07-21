using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
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

    private static readonly Regex FenceLineRegex = new(
        @"^ {0,3}(?<marker>`{3,}|~{3,})(?<remainder>.*)$",
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
    public string Render(ContentPage page, IReadOnlyDictionary<string, string> urlMap)
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
        var markdown = RenderLegacyMarkdownElements(
            markdownWithoutFences,
            fencedCodeBlocks);
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

        return html;
    }

    private static string RenderLegacyMarkdownElements(
        string markdown,
        IReadOnlyList<string> protectedBlocks)
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
                    return $"<{tag}{attributes}>{renderedBody}</{tag}>";
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
        markdown = RestoreProtectedBlocks(markdown, protectedBlocks);
        var html = Markdown.ToHtml(markdown, Pipeline).TrimEnd('\r', '\n');
        return RawTablePlaceholderRegex.Replace(
            html,
            match => rawTables[int.Parse(
                match.Groups["index"].Value,
                System.Globalization.CultureInfo.InvariantCulture)]);
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
