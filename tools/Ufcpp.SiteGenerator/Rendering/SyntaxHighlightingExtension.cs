using ColorCode;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Globalization;
using System.Net;
using System.Text;
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
        private const string HighlightRangesAttribute = "highlight-ranges";
        private const string HighlightTextAttribute = "highlight-text";
        private const string ErrorLinesAttribute = "error-lines";
        private const string ErrorRangesAttribute = "error-ranges";
        private const string ErrorTextAttribute = "error-text";
        private const string ErrorDiagnosticsAttribute = "error-diagnostics";
        private const string WarningLinesAttribute = "warning-lines";
        private const string WarningRangesAttribute = "warning-ranges";
        private const string WarningTextAttribute = "warning-text";
        private const string WarningDiagnosticsAttribute = "warning-diagnostics";
        private const string TitleAttribute = "title";
        private static readonly AnnotationDefinition[] AnnotationDefinitions =
        [
            new(
                AnnotationKind.Highlight,
                HighlightLinesAttribute,
                HighlightTextAttribute,
                HighlightRangesAttribute,
                "mark",
                "code-highlight",
                false,
                null),
            new(
                AnnotationKind.Error,
                ErrorLinesAttribute,
                ErrorTextAttribute,
                ErrorRangesAttribute,
                "span",
                "error",
                true,
                ErrorDiagnosticsAttribute),
            new(
                AnnotationKind.Warning,
                WarningLinesAttribute,
                WarningTextAttribute,
                WarningRangesAttribute,
                "span",
                "warning",
                true,
                WarningDiagnosticsAttribute),
        ];
        private readonly Lazy<RoslynCSharpHighlighter> _csharpHighlighter;

        public HighlightedCodeBlockRenderer(
            Lazy<RoslynCSharpHighlighter> csharpHighlighter)
        {
            _csharpHighlighter = csharpHighlighter;
        }

        protected override void Write(HtmlRenderer renderer, FencedCodeBlock block)
        {
            var code = block.Lines.ToString();
            var languageName = block.Info?
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            var metadata = GetMetadata(block, code, languageName);
            var preClassName = languageName?.Equals(
                "console",
                StringComparison.OrdinalIgnoreCase) == true
                    ? "console"
                    : null;
            if (languageName is not null
                && CSharpLanguageNames.Contains(languageName))
            {
                WriteOpeningTags(
                    renderer,
                    preClassName,
                    "csharp",
                    metadata.Title);
                renderer.Write(
                    ApplyAnnotations(
                        _csharpHighlighter.Value.Highlight(code),
                        code,
                        metadata.AnnotationSpans));
                renderer.WriteLine("</code></pre>");
                return;
            }

            if (languageName is null
                || !LanguagesByName.TryGetValue(languageName, out var language))
            {
                WriteOpeningTags(
                    renderer,
                    preClassName,
                    languageName,
                    metadata.Title);
                WritePlainCode(renderer, code, metadata.AnnotationSpans);
                renderer.WriteLine("</code></pre>");
                return;
            }

            WriteOpeningTags(
                renderer,
                preClassName,
                language.Name,
                metadata.Title);

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
                WritePlainCode(renderer, code, metadata.AnnotationSpans);
                renderer.WriteLine("</code></pre>");
                return;
            }

            renderer.Write(
                ApplyAnnotations(
                    highlightedCode.Html,
                    code,
                    metadata.AnnotationSpans));
            renderer.Write(highlightedCode.TrailingWhitespace);
            renderer.WriteLine("</code></pre>");
        }

        private static void WriteOpeningTags(
            HtmlRenderer renderer,
            string? preClassName,
            string? languageName,
            string? title)
        {
            renderer.Write("<pre");
            if (preClassName is not null)
            {
                renderer.Write(" class=\"");
                renderer.WriteEscape(preClassName);
                renderer.Write("\"");
            }

            if (title is not null)
            {
                renderer.Write(" title=\"");
                renderer.WriteEscape(title);
                renderer.Write("\"");
            }

            renderer.Write("><code");
            if (languageName is not null)
            {
                renderer.Write(" class=\"language-");
                renderer.WriteEscape(languageName);
                renderer.Write("\"");
            }

            renderer.Write(">");
        }

        private static CodeBlockMetadata GetMetadata(
            FencedCodeBlock block,
            string code,
            string? languageName)
        {
            var annotationValues = new Dictionary<string, string>(
                StringComparer.Ordinal);
            string? title = null;
            if (!string.IsNullOrWhiteSpace(block.Arguments)
                || ContainsAttachedMetadata(block.Info))
            {
                throw new InvalidDataException(
                    "The fenced code metadata is malformed.");
            }

            var attributes = block.TryGetAttributes();
            if (block.GetType() == typeof(FencedCodeBlock))
            {
                if (!string.IsNullOrEmpty(attributes?.Id))
                {
                    throw UnsupportedMetadata(
                        propertyName: "id",
                        languageName: languageName);
                }

                var generatedLanguageClass = languageName is null
                    ? null
                    : $"language-{languageName}";
                var classes = attributes?.Classes;
                var unsupportedClass = classes is null
                    ? null
                    : classes.FirstOrDefault(
                        className => !string.Equals(
                            className,
                            generatedLanguageClass,
                            StringComparison.Ordinal));
                if (unsupportedClass is not null)
                {
                    throw UnsupportedMetadata(
                        propertyName: "class",
                        languageName: languageName);
                }
            }

            foreach (var attribute in attributes?.Properties ?? [])
            {
                if (AnnotationDefinitions.Any(
                        definition => definition.AttributeNames.Contains(
                            attribute.Key,
                            StringComparer.Ordinal)))
                {
                    var value = attribute.Value
                        is { } encoded
                            ? WebUtility.HtmlDecode(encoded)
                            : throw new InvalidDataException(
                                $"The {attribute.Key} attribute requires a value.");
                    if (!annotationValues.TryAdd(attribute.Key, value))
                    {
                        throw new InvalidDataException(
                            $"The {attribute.Key} attribute cannot be repeated.");
                    }
                }
                else if (attribute.Key.Equals(
                             TitleAttribute,
                             StringComparison.Ordinal))
                {
                    if (title is not null)
                    {
                        throw new InvalidDataException(
                            $"The {TitleAttribute} attribute cannot be repeated.");
                    }

                    title = attribute.Value
                        is { } value
                            ? WebUtility.HtmlDecode(value)
                            : throw new InvalidDataException(
                                $"The {TitleAttribute} attribute requires a value.");
                    ValidateTitle(title);
                }
                else
                {
                    throw UnsupportedMetadata(
                        propertyName: attribute.Key,
                        languageName: languageName);
                }
            }

            var annotationChannels =
                new Dictionary<AnnotationKind, AnnotationChannel>();
            foreach (var definition in AnnotationDefinitions)
            {
                var lines = annotationValues.GetValueOrDefault(
                    definition.LinesAttribute);
                var text = annotationValues.GetValueOrDefault(
                    definition.TextAttribute);
                var ranges = annotationValues.GetValueOrDefault(
                    definition.RangesAttribute);
                var diagnostics = definition.DiagnosticsAttribute is null
                    ? null
                    : annotationValues.GetValueOrDefault(
                        definition.DiagnosticsAttribute);
                if (text is not null
                    && ranges is not null
                    && definition.Kind != AnnotationKind.Highlight)
                {
                    throw new InvalidDataException(
                        $"The {definition.TextAttribute} and "
                        + $"{definition.RangesAttribute} attributes are mutually exclusive.");
                }

                var spans = new List<SourceSpan>();
                if (lines is not null)
                {
                    AddLineSpans(spans, code, lines, definition.LinesAttribute);
                }

                if (text is not null)
                {
                    AddTextSpans(
                        spans,
                        code,
                        text,
                        definition.TextAttribute,
                        definition.RequiresUniqueText);
                }

                if (ranges is not null)
                {
                    spans.AddRange(
                        AnnotationRangeMetadata.Parse(
                                code,
                                ranges,
                                definition.RangesAttribute)
                            .Select(static span => new SourceSpan(span.Start, span.End)));
                }

                var identities = diagnostics is null
                    ? []
                    : DiagnosticIdentityMetadata.Parse(
                        code,
                        diagnostics,
                        definition.DiagnosticsAttribute!);
                var mergedSpans = MergeSpans(spans);
                if (identities.Any(identity => !mergedSpans.Any(
                        span => span.Start <= identity.Start
                            && identity.End <= span.End)))
                {
                    throw new InvalidDataException(
                        $"Every {definition.DiagnosticsAttribute} identity must be "
                        + "covered by its visual annotation metadata.");
                }

                annotationChannels.Add(
                    definition.Kind,
                    new AnnotationChannel(mergedSpans, identities));
            }

            return new CodeBlockMetadata(title, annotationChannels);
        }

        private static bool ContainsAttachedMetadata(string? info) =>
            info?.Contains('{', StringComparison.Ordinal) == true;

        private static InvalidDataException UnsupportedMetadata(
            string? languageName = null,
            string? propertyName = null)
        {
            return new InvalidDataException(
                "Fenced code metadata supports only title and highlight/error/"
                + "warning line, text, range, and diagnostic properties; "
                + "found unsupported "
                + $"'{propertyName}' metadata "
                + $"for language '{languageName ?? string.Empty}'.");
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidDataException(
                    $"The {TitleAttribute} attribute requires a non-empty value.");
            }

            if (title.Any(char.IsControl))
            {
                throw new InvalidDataException(
                    $"The {TitleAttribute} attribute cannot contain control characters.");
            }
        }

        private static void AddLineSpans(
            ICollection<SourceSpan> spans,
            string code,
            string value,
            string attributeName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw InvalidLineSyntax(attributeName);
            }

            var sourceLines = GetSourceLines(code);
            foreach (var item in value.Split(',', StringSplitOptions.None))
            {
                var range = item.Trim().Split('-', StringSplitOptions.None);
                if (range.Length is < 1 or > 2
                    || range.Any(string.IsNullOrWhiteSpace))
                {
                    throw InvalidLineSyntax(attributeName);
                }

                var startLine = ParseLineNumber(range[0], attributeName);
                var endLine = range.Length == 1
                    ? startLine
                    : ParseLineNumber(range[1], attributeName);
                if (endLine < startLine)
                {
                    throw InvalidLineSyntax(attributeName);
                }

                if (endLine > sourceLines.Count)
                {
                    throw new InvalidDataException(
                        $"The {attributeName} attribute references line "
                        + $"{endLine}, but the code block has {sourceLines.Count} lines.");
                }

                spans.Add(
                    new SourceSpan(
                        sourceLines[startLine - 1].Start,
                        GetLineContentEnd(code, sourceLines[endLine - 1])));
            }
        }

        private static int GetLineContentEnd(string code, SourceSpan line)
        {
            var end = line.End;
            if (end > line.Start && code[end - 1] == '\n')
            {
                end--;
                if (end > line.Start && code[end - 1] == '\r')
                {
                    end--;
                }
            }

            return end;
        }

        private static int ParseLineNumber(string value, string attributeName)
        {
            if (!int.TryParse(
                    value.Trim(),
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out var line)
                || line <= 0)
            {
                throw InvalidLineSyntax(attributeName);
            }

            return line;
        }

        private static InvalidDataException InvalidLineSyntax(string attributeName) =>
            new(
                $"The {attributeName} attribute must contain "
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
            string value,
            string attributeName,
            bool requiresUniqueText)
        {
            if (value.Length == 0)
            {
                throw new InvalidDataException(
                    $"The {attributeName} attribute requires a non-empty value.");
            }

            var matchStart = code.IndexOf(value, 0, StringComparison.Ordinal);
            if (matchStart < 0)
            {
                throw new InvalidDataException(
                    $"The {attributeName} literal does not occur in the code block.");
            }

            if (requiresUniqueText
                && matchStart <= code.Length - value.Length - 1
                && code.IndexOf(
                    value,
                    matchStart + 1,
                    StringComparison.Ordinal) >= 0)
            {
                throw new InvalidDataException(
                    $"The {attributeName} literal must occur exactly once "
                    + "in the code block.");
            }

            while (matchStart >= 0)
            {
                spans.Add(new SourceSpan(matchStart, matchStart + value.Length));
                if (requiresUniqueText)
                {
                    break;
                }

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
            IReadOnlyDictionary<AnnotationKind, AnnotationChannel>
                annotationChannels)
        {
            if (!annotationChannels.Values.Any(static channel =>
                    channel.VisualSpans.Count != 0
                    || channel.DiagnosticIdentities.Count != 0))
            {
                renderer.WriteEscape(code);
                return;
            }

            renderer.Write(
                ApplyAnnotations(
                    WebUtility.HtmlEncode(code),
                    code,
                    annotationChannels));
        }

        private static string ApplyAnnotations(
            string highlightedCode,
            string code,
            IReadOnlyDictionary<AnnotationKind, AnnotationChannel>
                annotationChannels)
        {
            if (!annotationChannels.Values.Any(static channel =>
                    channel.VisualSpans.Count != 0
                    || channel.DiagnosticIdentities.Count != 0))
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

            InsertAnnotations(root, code, annotationChannels);
            if (!string.Equals(root.Value, code, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "Applying code highlights changed the source code text.");
            }

            return SerializeNodes(root.Nodes());
        }

        private static string SerializeNodes(IEnumerable<XNode> nodes)
        {
            var output = new StringBuilder();
            foreach (var node in nodes)
            {
                WriteNode(node);
            }

            return output.ToString();

            void WriteNode(XNode node)
            {
                if (node is XText text)
                {
                    output.Append(WebUtility.HtmlEncode(text.Value));
                    return;
                }

                if (node is not XElement element)
                {
                    throw new InvalidDataException(
                        "The highlighted fragment contains an unsupported node.");
                }

                output.Append('<').Append(element.Name.LocalName);
                foreach (var attribute in element.Attributes())
                {
                    output.Append(' ')
                        .Append(attribute.Name.LocalName)
                        .Append("=\"")
                        .Append(WebUtility.HtmlEncode(attribute.Value))
                        .Append('"');
                }

                output.Append('>');
                foreach (var child in element.Nodes())
                {
                    WriteNode(child);
                }

                output.Append("</")
                    .Append(element.Name.LocalName)
                    .Append('>');
            }
        }

        private static void InsertAnnotations(
            XElement root,
            string code,
            IReadOnlyDictionary<AnnotationKind, AnnotationChannel>
                annotationChannels)
        {
            var segments = new List<RenderedSegment>();
            var sourcePosition = 0;
            foreach (var node in root.Nodes().ToArray())
            {
                string text;
                XElement? element = null;
                if (node is XText textNode)
                {
                    text = textNode.Value;
                }
                else if (node is XElement candidate
                         && !candidate.Descendants().Any())
                {
                    element = candidate;
                    text = candidate.Value;
                }
                else
                {
                    throw new InvalidDataException(
                        "The syntax highlighter returned unsupported nested markup.");
                }

                var nodeEnd = sourcePosition + text.Length;
                var boundaries = new SortedSet<int> { 0, text.Length };
                foreach (var span in annotationChannels.Values.SelectMany(
                             static channel => channel.VisualSpans))
                {
                    if (span.End <= sourcePosition || span.Start >= nodeEnd)
                    {
                        continue;
                    }

                    boundaries.Add(Math.Max(span.Start, sourcePosition) - sourcePosition);
                    boundaries.Add(Math.Min(span.End, nodeEnd) - sourcePosition);
                }
                foreach (var identity in annotationChannels.Values.SelectMany(
                             static channel => channel.DiagnosticIdentities))
                {
                    if (identity.End <= sourcePosition || identity.Start >= nodeEnd)
                    {
                        continue;
                    }

                    boundaries.Add(
                        Math.Max(identity.Start, sourcePosition) - sourcePosition);
                    boundaries.Add(
                        Math.Min(identity.End, nodeEnd) - sourcePosition);
                }

                var positions = boundaries.ToArray();
                for (var index = 0; index < positions.Length - 1; index++)
                {
                    var start = positions[index];
                    var end = positions[index + 1];
                    if (start == end)
                    {
                        continue;
                    }

                    var absoluteStart = sourcePosition + start;
                    var absoluteEnd = sourcePosition + end;
                    var value = text[start..end];
                    XNode renderedNode = element is null
                        ? new XText(value)
                        : new XElement(
                            element.Name,
                            element.Attributes().Select(
                                static attribute => new XAttribute(attribute)),
                            value);
                    segments.Add(
                        new RenderedSegment(
                            renderedNode,
                            GetActiveWrappers(
                                annotationChannels,
                                absoluteStart,
                                absoluteEnd)));
                }

                sourcePosition = nodeEnd;
            }

            if (sourcePosition != code.Length)
            {
                throw new InvalidDataException(
                    "The syntax highlighter fragment ended before the source code.");
            }

            root.RemoveNodes();
            var activeWrappers = new List<AnnotationWrapper>();
            var activeElements = new List<XElement>();
            XElement parent = root;
            foreach (var segment in segments)
            {
                var common = 0;
                while (common < activeWrappers.Count
                       && common < segment.ActiveWrappers.Count
                       && activeWrappers[common] == segment.ActiveWrappers[common])
                {
                    common++;
                }

                while (activeWrappers.Count > common)
                {
                    activeWrappers.RemoveAt(activeWrappers.Count - 1);
                    activeElements.RemoveAt(activeElements.Count - 1);
                }

                parent = activeElements.Count == 0 ? root : activeElements[^1];
                foreach (var wrapper in segment.ActiveWrappers.Skip(common))
                {
                    var element = new XElement(
                        wrapper.ElementName,
                        new XAttribute("class", wrapper.ClassName));
                    if (wrapper.DiagnosticId is not null)
                    {
                        element.Add(new XAttribute("title", wrapper.DiagnosticId));
                    }

                    parent.Add(element);
                    parent = element;
                    activeWrappers.Add(wrapper);
                    activeElements.Add(element);
                }

                parent.Add(segment.Node);
            }
        }

        private static IReadOnlyList<AnnotationWrapper> GetActiveWrappers(
            IReadOnlyDictionary<AnnotationKind, AnnotationChannel> channels,
            int start,
            int end)
        {
            var wrappers = new List<AnnotationWrapper>();
            foreach (var definition in AnnotationDefinitions)
            {
                var channel = channels[definition.Kind];
                var visual = channel.VisualSpans.Any(
                    span => span.Start <= start && end <= span.End);
                var identities = channel.DiagnosticIdentities
                    .Where(identity => identity.Start <= start && end <= identity.End)
                    .OrderBy(static identity => identity.Order)
                    .ToArray();
                if (identities.Length != 0)
                {
                    wrappers.AddRange(
                        identities.Select(identity => new AnnotationWrapper(
                            definition.ElementName,
                            definition.ClassName,
                            identity.Id,
                            identity.Order)));
                }
                else if (visual)
                {
                    wrappers.Add(
                        new AnnotationWrapper(
                            definition.ElementName,
                            definition.ClassName,
                            null,
                            -1));
                }
            }

            return wrappers;
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

        private readonly record struct CodeBlockMetadata(
            string? Title,
            IReadOnlyDictionary<AnnotationKind, AnnotationChannel>
                AnnotationSpans);

        private readonly record struct RenderedSegment(
            XNode Node,
            IReadOnlyList<AnnotationWrapper> ActiveWrappers);

        private readonly record struct AnnotationChannel(
            IReadOnlyList<SourceSpan> VisualSpans,
            IReadOnlyList<DiagnosticIdentity> DiagnosticIdentities);

        private readonly record struct AnnotationWrapper(
            string ElementName,
            string ClassName,
            string? DiagnosticId,
            int Order);

        [Flags]
        private enum AnnotationKind
        {
            None = 0,
            Highlight = 1,
            Error = 2,
            Warning = 4,
        }

        private sealed record AnnotationDefinition(
            AnnotationKind Kind,
            string LinesAttribute,
            string TextAttribute,
            string RangesAttribute,
            string ElementName,
            string ClassName,
            bool RequiresUniqueText,
            string? DiagnosticsAttribute)
        {
            public IReadOnlyList<string> AttributeNames { get; } =
                DiagnosticsAttribute is null
                    ? [LinesAttribute, TextAttribute, RangesAttribute]
                    : [
                        LinesAttribute,
                        TextAttribute,
                        RangesAttribute,
                        DiagnosticsAttribute,
                    ];
        }

        private readonly record struct SourceSpan(int Start, int End);
    }
}
