using ColorCode;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Globalization;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace Ufcpp.SiteGenerator.Rendering;

internal sealed class SyntaxHighlightingExtension : IMarkdownExtension
{
    private readonly Lazy<RoslynCSharpHighlighter> _csharpHighlighter = new();

    private static readonly IReadOnlySet<string> CSharpLanguageNames =
        new HashSet<string>(
            ["csharp", "cs", "c#"],
            StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<string, (string Name, ILanguage Language)>
        LanguagesByName =
            new Dictionary<string, (string Name, ILanguage Language)>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["xml"] = ("xml", Languages.Xml),
                ["html"] = ("html", Languages.Html),
                ["css"] = ("css", Languages.Css),
                ["powershell"] = ("powershell", Languages.PowerShell),
                ["ps1"] = ("powershell", Languages.PowerShell),
                ["cpp"] = ("cpp", Languages.Cpp),
                ["c++"] = ("cpp", Languages.Cpp),
                ["vbnet"] = ("vbnet", Languages.VbDotNet),
                ["vb"] = ("vbnet", Languages.VbDotNet),
                ["fsharp"] = ("fsharp", Languages.FSharp),
                ["fs"] = ("fsharp", Languages.FSharp),
                // ColorCode exposes JSON through lookup but has no Languages.Json property.
                ["json"] = ("json", Languages.FindById("json")),
                ["sql"] = ("sql", Languages.Sql),
                ["java"] = ("java", Languages.Java),
                ["python"] = ("python", Languages.Python),
                ["py"] = ("python", Languages.Python),
                ["javascript"] = ("javascript", Languages.JavaScript),
                ["js"] = ("javascript", Languages.JavaScript),
                ["typescript"] = ("typescript", Languages.Typescript),
                ["ts"] = ("typescript", Languages.Typescript),
            };

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.Insert(
                0,
                new HighlightedCodeBlockRenderer(_csharpHighlighter));
        }
    }

    private sealed class HighlightedCodeBlockRenderer
        : HtmlObjectRenderer<FencedCodeBlock>
    {
        private const string HighlightLinesAttribute = "highlight-lines";
        private const string HighlightTextAttribute = "highlight-text";
        private const string HighlightClassName = "code-highlight";
        private readonly Lazy<RoslynCSharpHighlighter> _csharpHighlighter;

        public HighlightedCodeBlockRenderer(
            Lazy<RoslynCSharpHighlighter> csharpHighlighter)
        {
            _csharpHighlighter = csharpHighlighter;
        }

        protected override void Write(HtmlRenderer renderer, FencedCodeBlock block)
        {
            var code = block.Lines.ToString();
            var highlightSpans = GetHighlightSpans(block, code);
            var languageName = block.Info?
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            if (languageName is not null
                && CSharpLanguageNames.Contains(languageName))
            {
                renderer.Write("<pre><code class=\"language-csharp\">");
                renderer.Write(
                    ApplyHighlights(
                        _csharpHighlighter.Value.Highlight(code),
                        code,
                        highlightSpans));
                renderer.WriteLine("</code></pre>");
                return;
            }

            if (languageName is null
                || !LanguagesByName.TryGetValue(languageName, out var language))
            {
                renderer.Write("<pre><code");
                if (languageName is not null)
                {
                    renderer.Write(" class=\"language-");
                    renderer.WriteEscape(languageName);
                    renderer.Write("\"");
                }

                renderer.Write(">");
                WritePlainCode(renderer, code, highlightSpans);
                renderer.WriteLine("</code></pre>");
                return;
            }

            renderer.Write("<pre><code class=\"language-");
            renderer.WriteEscape(language.Name);
            renderer.Write("\">");

            HighlightedFragment highlightedCode;
            try
            {
                highlightedCode = ExtractHighlightedCode(
                    new HtmlClassFormatter().GetHtmlString(code, language.Language));
            }
            catch (Exception exception) when (
                exception is InvalidDataException
                    or ArgumentException
                    or InvalidOperationException
                    or System.Text.RegularExpressions.RegexMatchTimeoutException)
            {
                WritePlainCode(renderer, code, highlightSpans);
                renderer.WriteLine("</code></pre>");
                return;
            }

            renderer.Write(
                ApplyHighlights(highlightedCode.Html, code, highlightSpans));
            renderer.Write(highlightedCode.TrailingWhitespace);
            renderer.WriteLine("</code></pre>");
        }

        private static IReadOnlyList<SourceSpan> GetHighlightSpans(
            FencedCodeBlock block,
            string code)
        {
            string? highlightedLines = null;
            string? highlightedText = null;
            if (ContainsHighlightMetadata(block.Arguments)
                || ContainsAttachedHighlightMetadata(block.Info))
            {
                throw new InvalidDataException(
                    "The fenced code highlight metadata is malformed.");
            }

            foreach (var attribute in block.TryGetAttributes()?.Properties ?? [])
            {
                if (attribute.Key.Equals(
                        HighlightLinesAttribute,
                        StringComparison.Ordinal))
                {
                    if (highlightedLines is not null)
                    {
                        throw new InvalidDataException(
                            $"The {HighlightLinesAttribute} attribute cannot be repeated.");
                    }

                    highlightedLines = attribute.Value
                        ?? throw new InvalidDataException(
                            $"The {HighlightLinesAttribute} attribute requires a value.");
                }
                else if (attribute.Key.Equals(
                             HighlightTextAttribute,
                             StringComparison.Ordinal))
                {
                    if (highlightedText is not null)
                    {
                        throw new InvalidDataException(
                            $"The {HighlightTextAttribute} attribute cannot be repeated.");
                    }

                    highlightedText = attribute.Value
                        ?? throw new InvalidDataException(
                            $"The {HighlightTextAttribute} attribute requires a value.");
                }
            }

            var spans = new List<SourceSpan>();
            if (highlightedLines is not null)
            {
                AddLineSpans(spans, code, highlightedLines);
            }

            if (highlightedText is not null)
            {
                AddTextSpans(spans, code, highlightedText);
            }

            return MergeSpans(spans);
        }

        private static bool ContainsHighlightMetadata(string? arguments) =>
            arguments is not null
            && (arguments.Contains(
                    HighlightLinesAttribute,
                    StringComparison.Ordinal)
                || arguments.Contains(
                    HighlightTextAttribute,
                    StringComparison.Ordinal));

        private static bool ContainsAttachedHighlightMetadata(string? info) =>
            info is not null
            && (info.Contains(
                    $"{{{HighlightLinesAttribute}",
                    StringComparison.Ordinal)
                || info.Contains(
                    $"{{{HighlightTextAttribute}",
                    StringComparison.Ordinal));

        private static void AddLineSpans(
            ICollection<SourceSpan> spans,
            string code,
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw InvalidLineSyntax();
            }

            var sourceLines = GetSourceLines(code);
            foreach (var item in value.Split(',', StringSplitOptions.None))
            {
                var range = item.Trim().Split('-', StringSplitOptions.None);
                if (range.Length is < 1 or > 2
                    || range.Any(string.IsNullOrWhiteSpace))
                {
                    throw InvalidLineSyntax();
                }

                var startLine = ParseLineNumber(range[0]);
                var endLine = range.Length == 1
                    ? startLine
                    : ParseLineNumber(range[1]);
                if (endLine < startLine)
                {
                    throw InvalidLineSyntax();
                }

                if (endLine > sourceLines.Count)
                {
                    throw new InvalidDataException(
                        $"The {HighlightLinesAttribute} attribute references line "
                        + $"{endLine}, but the code block has {sourceLines.Count} lines.");
                }

                spans.Add(
                    new SourceSpan(
                        sourceLines[startLine - 1].Start,
                        sourceLines[endLine - 1].End));
            }
        }

        private static int ParseLineNumber(string value)
        {
            if (!int.TryParse(
                    value.Trim(),
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out var line)
                || line <= 0)
            {
                throw InvalidLineSyntax();
            }

            return line;
        }

        private static InvalidDataException InvalidLineSyntax() =>
            new(
                $"The {HighlightLinesAttribute} attribute must contain "
                + "comma-separated positive line numbers or inclusive ranges.");

        private static IReadOnlyList<SourceSpan> GetSourceLines(string code)
        {
            var lines = new List<SourceSpan>();
            var lineStart = 0;
            while (lineStart < code.Length)
            {
                var lineFeed = code.IndexOf('\n', lineStart);
                var lineEnd = lineFeed < 0 ? code.Length : lineFeed + 1;
                lines.Add(new SourceSpan(lineStart, lineEnd));
                lineStart = lineEnd;
            }

            return lines;
        }

        private static void AddTextSpans(
            ICollection<SourceSpan> spans,
            string code,
            string value)
        {
            if (value.Length == 0)
            {
                throw new InvalidDataException(
                    $"The {HighlightTextAttribute} attribute requires a non-empty value.");
            }

            var matchStart = code.IndexOf(value, 0, StringComparison.Ordinal);
            if (matchStart < 0)
            {
                throw new InvalidDataException(
                    $"The {HighlightTextAttribute} literal does not occur in the code block.");
            }

            while (matchStart >= 0)
            {
                spans.Add(new SourceSpan(matchStart, matchStart + value.Length));
                var nextStart = matchStart + 1;
                matchStart = nextStart > code.Length - value.Length
                    ? -1
                    : code.IndexOf(value, nextStart, StringComparison.Ordinal);
            }
        }

        private static IReadOnlyList<SourceSpan> MergeSpans(
            IReadOnlyCollection<SourceSpan> spans)
        {
            if (spans.Count == 0)
            {
                return [];
            }

            var ordered = spans
                .OrderBy(static span => span.Start)
                .ThenBy(static span => span.End)
                .ToArray();
            var merged = new List<SourceSpan>(ordered.Length);
            var current = ordered[0];

            foreach (var span in ordered.AsSpan(1))
            {
                if (span.Start <= current.End)
                {
                    current = new SourceSpan(
                        current.Start,
                        Math.Max(current.End, span.End));
                    continue;
                }

                merged.Add(current);
                current = span;
            }

            merged.Add(current);
            return merged;
        }

        private static void WritePlainCode(
            HtmlRenderer renderer,
            string code,
            IReadOnlyList<SourceSpan> highlightSpans)
        {
            if (highlightSpans.Count == 0)
            {
                renderer.WriteEscape(code);
                return;
            }

            renderer.Write(
                ApplyHighlights(
                    WebUtility.HtmlEncode(code),
                    code,
                    highlightSpans));
        }

        private static string ApplyHighlights(
            string highlightedCode,
            string code,
            IReadOnlyList<SourceSpan> highlightSpans)
        {
            if (highlightSpans.Count == 0)
            {
                return highlightedCode;
            }

            XElement root;
            try
            {
                root = XElement.Parse(
                    $"<root>{highlightedCode}</root>",
                    LoadOptions.PreserveWhitespace);
            }
            catch (XmlException exception)
            {
                throw new InvalidDataException(
                    "The syntax highlighter returned a malformed HTML fragment.",
                    exception);
            }

            if (!string.Equals(root.Value, code, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "The syntax highlighter fragment does not map exactly to the source code.");
            }

            InsertMarks(root, code, highlightSpans);
            if (!string.Equals(root.Value, code, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "Applying code highlights changed the source code text.");
            }

            return string.Concat(
                root.Nodes().Select(
                    static node => node.ToString(SaveOptions.DisableFormatting)));
        }

        private static void InsertMarks(
            XElement root,
            string code,
            IReadOnlyList<SourceSpan> highlightSpans)
        {
            var sourcePosition = 0;
            var spanIndex = 0;
            foreach (var textNode in root.DescendantNodes().OfType<XText>().ToArray())
            {
                var text = textNode.Value;
                var nodeEnd = sourcePosition + text.Length;
                var replacementNodes = new List<object>();
                var localPosition = 0;

                while (localPosition < text.Length)
                {
                    var absolutePosition = sourcePosition + localPosition;
                    while (spanIndex < highlightSpans.Count
                           && highlightSpans[spanIndex].End <= absolutePosition)
                    {
                        spanIndex++;
                    }

                    if (spanIndex >= highlightSpans.Count
                        || highlightSpans[spanIndex].Start >= nodeEnd)
                    {
                        replacementNodes.Add(new XText(text[localPosition..]));
                        localPosition = text.Length;
                        continue;
                    }

                    var span = highlightSpans[spanIndex];
                    if (absolutePosition < span.Start)
                    {
                        var plainEnd = Math.Min(span.Start, nodeEnd);
                        replacementNodes.Add(
                            new XText(
                                text[localPosition..(plainEnd - sourcePosition)]));
                        localPosition = plainEnd - sourcePosition;
                        continue;
                    }

                    var highlightedEnd = Math.Min(span.End, nodeEnd);
                    replacementNodes.Add(
                        new XElement(
                            "mark",
                            new XAttribute("class", HighlightClassName),
                            text[localPosition..(highlightedEnd - sourcePosition)]));
                    localPosition = highlightedEnd - sourcePosition;
                }

                if (replacementNodes.Count > 0)
                {
                    textNode.ReplaceWith(replacementNodes.ToArray());
                }

                sourcePosition = nodeEnd;
            }

            if (sourcePosition != code.Length)
            {
                throw new InvalidDataException(
                    "The syntax highlighter fragment ended before the source code.");
            }
        }

        private static HighlightedFragment ExtractHighlightedCode(string html)
        {
            const string OpeningTag = "<pre";
            const string ClosingTag = "</pre>";
            var openingStart = html.IndexOf(OpeningTag, StringComparison.Ordinal);
            var contentStart = openingStart < 0
                ? -1
                : html.IndexOf('>', openingStart) + 1;
            var end = html.LastIndexOf(ClosingTag, StringComparison.Ordinal);
            if (contentStart <= 0 || end < contentStart)
            {
                throw new InvalidDataException(
                    "The syntax highlighter returned an unexpected HTML fragment.");
            }

            if (html.AsSpan(contentStart).StartsWith("\r\n", StringComparison.Ordinal))
            {
                contentStart += 2;
            }
            else if (html.AsSpan(contentStart).StartsWith("\n", StringComparison.Ordinal))
            {
                contentStart++;
            }

            var contentEnd = end;
            var trailingWhitespace = string.Empty;
            if (html.AsSpan(contentStart, contentEnd - contentStart)
                .EndsWith("\r\n", StringComparison.Ordinal))
            {
                contentEnd -= 2;
                trailingWhitespace = "\r\n";
            }
            else if (html.AsSpan(contentStart, contentEnd - contentStart)
                     .EndsWith("\n", StringComparison.Ordinal))
            {
                contentEnd--;
                trailingWhitespace = "\n";
            }

            return new HighlightedFragment(
                html[contentStart..contentEnd],
                trailingWhitespace);
        }

        private readonly record struct HighlightedFragment(
            string Html,
            string TrailingWhitespace);

        private readonly record struct SourceSpan(int Start, int End);
    }
}
