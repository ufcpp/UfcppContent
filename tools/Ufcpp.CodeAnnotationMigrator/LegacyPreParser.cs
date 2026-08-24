using System.Net;
using System.Text;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Ufcpp.CodeAnnotationMigrator;

internal static class LegacyPreParser
{
    public static IReadOnlyList<HistoricalCodeBlock> Parse(string document)
    {
        var result = ParseDetailed(document);
        if (result.Diagnostics.Count != 0)
        {
            throw new InvalidDataException(result.Diagnostics[0].Message);
        }

        return result.Blocks;
    }

    public static HistoricalParseResult ParseDetailed(string document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var excludedSpans = Markdown.Parse(document)
            .Descendants()
            .Where(static node => node is CodeBlock or CodeInline)
            .Select(static node => (node.Span.Start, node.Span.End))
            .Concat(FindInlineCodeSpans(document))
            .OrderBy(static span => span.Start)
            .ToArray();
        var blocks = new List<HistoricalCodeBlock>();
        var diagnostics = new List<HistoricalParseDiagnostic>();
        var tableDepth = 0;
        var preBlockCount = 0;
        var suppressedRecoveryClosings = 0;

        for (var index = 0; index < document.Length;)
        {
            var excluded = excludedSpans.FirstOrDefault(
                span => index >= span.Start && index <= span.End);
            if (excluded != default)
            {
                index = excluded.End + 1;
                continue;
            }

            if (document[index] != '<'
                || !TryReadTag(document, index, out var tag))
            {
                index++;
                continue;
            }

            if (tag.Name.Equals("table", StringComparison.OrdinalIgnoreCase))
            {
                tableDepth = tag.IsClosing
                    ? Math.Max(0, tableDepth - 1)
                    : tableDepth + (tag.IsSelfClosing ? 0 : 1);
                index = tag.End + 1;
                continue;
            }

            if (!tag.IsClosing
                && tag.Name.Equals("pre", StringComparison.OrdinalIgnoreCase))
            {
                preBlockCount++;
                var sourceLine = SourceText.GetLineNumber(document, tag.Start);
                var isInsideTable = tableDepth > 0;
                HtmlTag? closingTag = null;
                try
                {
                    closingTag = FindClosingTag(document, tag, "pre");
                    var attributes = ParseAttributes(tag.Attributes);
                    var title = attributes.GetValueOrDefault("title");
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        title = null;
                    }

                    var (code, annotations) = ParsePreBody(
                        document,
                        tag.End + 1,
                        closingTag.Value.Start);
                    blocks.Add(new HistoricalCodeBlock(
                        preBlockCount,
                        tag.Start,
                        sourceLine,
                        isInsideTable,
                        title,
                        code,
                        annotations));
                }
                catch (Exception exception) when (
                    exception is InvalidDataException or NestedPreException)
                {
                    if (exception is NestedPreException)
                    {
                        suppressedRecoveryClosings++;
                    }

                    diagnostics.Add(new HistoricalParseDiagnostic(
                        "MALFORMED_HISTORICAL_BLOCK",
                        preBlockCount,
                        sourceLine,
                        isInsideTable,
                        true,
                        exception.Message));
                }

                index = closingTag?.End + 1 ?? tag.End + 1;
                continue;
            }

            if (tag.IsClosing
                && tag.Name.Equals("pre", StringComparison.OrdinalIgnoreCase))
            {
                if (suppressedRecoveryClosings > 0)
                {
                    suppressedRecoveryClosings--;
                }
                else
                {
                    diagnostics.Add(new HistoricalParseDiagnostic(
                        "ORPHAN_HISTORICAL_PRE_CLOSE",
                        null,
                        SourceText.GetLineNumber(document, tag.Start),
                        tableDepth > 0,
                        false,
                        "Orphan </pre> closing tag has no matching opening tag."));
                }
            }

            index = tag.End + 1;
        }

        return new HistoricalParseResult(blocks, diagnostics, preBlockCount);
    }

    private static IEnumerable<(int Start, int End)> FindInlineCodeSpans(
        string document)
    {
        for (var lineStart = 0; lineStart < document.Length;)
        {
            var lineEnd = document.IndexOfAny(['\r', '\n'], lineStart);
            if (lineEnd < 0)
            {
                lineEnd = document.Length;
            }

            for (var index = lineStart; index < lineEnd;)
            {
                if (document[index] != '`'
                    || index > lineStart && document[index - 1] == '\\')
                {
                    index++;
                    continue;
                }

                var markerLength = 1;
                while (index + markerLength < lineEnd
                       && document[index + markerLength] == '`')
                {
                    markerLength++;
                }

                var closing = index + markerLength;
                while (closing < lineEnd)
                {
                    closing = document.IndexOf('`', closing, lineEnd - closing);
                    if (closing < 0)
                    {
                        break;
                    }

                    var closingLength = 1;
                    while (closing + closingLength < lineEnd
                           && document[closing + closingLength] == '`')
                    {
                        closingLength++;
                    }

                    if (closingLength == markerLength)
                    {
                        yield return (index, closing + closingLength - 1);
                        index = closing + closingLength;
                        break;
                    }

                    closing += closingLength;
                }

                if (closing < 0 || closing >= lineEnd)
                {
                    index += markerLength;
                }
            }

            lineStart = lineEnd;
            if (lineStart < document.Length && document[lineStart] == '\r')
            {
                lineStart++;
            }

            if (lineStart < document.Length && document[lineStart] == '\n')
            {
                lineStart++;
            }
        }
    }

    private static (string Code, IReadOnlyList<AnnotationSelection> Annotations)
        ParsePreBody(string document, int bodyStart, int bodyEnd)
    {
        var wrapperStart = bodyStart;
        var wrapperEnd = bodyEnd;
        while (wrapperStart < wrapperEnd && char.IsWhiteSpace(document[wrapperStart]))
        {
            wrapperStart++;
        }

        while (wrapperEnd > wrapperStart && char.IsWhiteSpace(document[wrapperEnd - 1]))
        {
            wrapperEnd--;
        }

        if (wrapperStart < wrapperEnd
            && document[wrapperStart] == '<'
            && TryReadTag(document, wrapperStart, out var openingCode)
            && !openingCode.IsClosing
            && openingCode.Name.Equals("code", StringComparison.OrdinalIgnoreCase))
        {
            var closingCode = FindClosingTag(document, openingCode, "code");
            if (closingCode.Start <= bodyEnd
                && IsOnlyWhitespace(document, closingCode.End + 1, bodyEnd))
            {
                return ParseAnnotatedText(
                    document,
                    openingCode.End + 1,
                    closingCode.Start);
            }
        }

        return ParseAnnotatedText(document, bodyStart, bodyEnd);
    }

    private static (string Code, IReadOnlyList<AnnotationSelection> Annotations)
        ParseAnnotatedText(string document, int start, int end)
    {
        var code = new StringBuilder(end - start);
        var annotations = new List<AnnotationSelection>();
        var openEmphasis = new List<OpenSelection>();
        var openSpans = new List<OpenSelection>();
        var openCodeCount = 0;

        for (var index = start; index < end;)
        {
            if (document[index] != '<'
                || !TryReadTag(document, index, out var tag)
                || tag.End >= end)
            {
                var nextTag = document.IndexOf(
                    '<',
                    document[index] == '<' ? index + 1 : index);
                if (nextTag < 0 || nextTag >= end)
                {
                    nextTag = end;
                }

                var text = document[index..nextTag];
                code.Append(WebUtility.HtmlDecode(text));
                index = nextTag;
                continue;
            }

            if (tag.Name.Equals("br", StringComparison.OrdinalIgnoreCase)
                && !tag.IsClosing)
            {
                code.Append('\n');
                index = tag.End + 1;
                continue;
            }

            if (tag.IsClosing)
            {
                if (tag.Name.Equals("em", StringComparison.OrdinalIgnoreCase))
                {
                    CloseSelection(
                        "em",
                        code,
                        openEmphasis,
                        annotations,
                        required: true);
                }
                else if (tag.Name.Equals("span", StringComparison.OrdinalIgnoreCase))
                {
                    CloseSelection(
                        "span",
                        code,
                        openSpans,
                        annotations,
                        required: false);
                }
                else if (tag.Name.Equals("code", StringComparison.OrdinalIgnoreCase))
                {
                    if (openCodeCount == 0)
                    {
                        throw new InvalidDataException(
                            "Unbalanced </code> markup in a <pre> block.");
                    }

                    openCodeCount--;
                }
            }
            else if (!tag.IsSelfClosing)
            {
                if (tag.Name.Equals("em", StringComparison.OrdinalIgnoreCase))
                {
                    openEmphasis.Add(new OpenSelection(
                        AnnotationKind.Highlight,
                        code.Length));
                }
                else if (tag.Name.Equals("span", StringComparison.OrdinalIgnoreCase))
                {
                    openSpans.Add(new OpenSelection(
                        GetAnnotationKind(tag),
                        code.Length));
                }
                else if (tag.Name.Equals("code", StringComparison.OrdinalIgnoreCase))
                {
                    openCodeCount++;
                }
            }

            index = tag.End + 1;
        }

        if (openEmphasis.Count != 0)
        {
            throw new InvalidDataException(
                "Unbalanced <em> markup in a <pre> block.");
        }

        if (openSpans.Any(static span => span.Kind is not null))
        {
            throw new InvalidDataException(
                "Unbalanced annotated <span> markup in a <pre> block.");
        }

        if (openCodeCount != 0)
        {
            throw new InvalidDataException(
                "Unbalanced <code> markup in a <pre> block.");
        }

        annotations.Sort(static (left, right) =>
        {
            var startComparison = left.Start.CompareTo(right.Start);
            if (startComparison != 0)
            {
                return startComparison;
            }

            var lengthComparison = left.Length.CompareTo(right.Length);
            return lengthComparison != 0
                ? lengthComparison
                : left.Kind.CompareTo(right.Kind);
        });
        return (code.ToString(), annotations);
    }

    private static void CloseSelection(
        string tagName,
        StringBuilder code,
        List<OpenSelection> openSelections,
        List<AnnotationSelection> annotations,
        bool required)
    {
        if (openSelections.Count == 0)
        {
            if (required)
            {
                throw new InvalidDataException(
                    $"Unbalanced </{tagName}> markup in a <pre> block.");
            }

            return;
        }

        var opening = openSelections[^1];
        openSelections.RemoveAt(openSelections.Count - 1);
        if (opening.Kind is not { } kind)
        {
            return;
        }

        var length = code.Length - opening.Start;
        annotations.Add(new AnnotationSelection(
            kind,
            opening.Start,
            length,
            code.ToString(opening.Start, length)));
    }

    private static AnnotationKind? GetAnnotationKind(HtmlTag tag)
    {
        if (tag.Name.Equals("em", StringComparison.OrdinalIgnoreCase))
        {
            return AnnotationKind.Highlight;
        }

        if (!tag.Name.Equals("span", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var classes = ParseAttributes(tag.Attributes)
            .GetValueOrDefault("class")?
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase)
            ?? [];
        var isError = classes.Contains("error");
        var isWarning = classes.Contains("warning");
        if (isError && isWarning)
        {
            throw new InvalidDataException(
                "A legacy span cannot be both error and warning metadata.");
        }

        return isError
            ? AnnotationKind.Error
            : isWarning
                ? AnnotationKind.Warning
                : null;
    }

    private static HtmlTag FindClosingTag(
        string document,
        HtmlTag openingTag,
        string name)
    {
        var depth = 1;
        for (var index = openingTag.End + 1; index < document.Length;)
        {
            var next = document.IndexOf('<', index);
            if (next < 0)
            {
                break;
            }

            if (!TryReadTag(document, next, out var tag))
            {
                index = next + 1;
                continue;
            }

            if (tag.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                if (tag.IsClosing)
                {
                    depth--;
                    if (depth == 0)
                    {
                        return tag;
                    }
                }
                else if (!tag.IsSelfClosing)
                {
                    if (name.Equals("pre", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new NestedPreException(
                            $"Nested <pre> element at line "
                            + $"{SourceText.GetLineNumber(document, tag.Start)}.");
                    }

                    depth++;
                }
            }

            index = tag.End + 1;
        }

        throw new InvalidDataException(
            $"Unclosed <{name}> element at line "
            + $"{SourceText.GetLineNumber(document, openingTag.Start)}.");
    }

    private static IReadOnlyDictionary<string, string?> ParseAttributes(
        string attributes)
    {
        var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        for (var index = 0; index < attributes.Length;)
        {
            while (index < attributes.Length && char.IsWhiteSpace(attributes[index]))
            {
                index++;
            }

            if (index >= attributes.Length)
            {
                break;
            }

            if (attributes[index] == '/'
                && attributes[(index + 1)..].All(char.IsWhiteSpace))
            {
                break;
            }

            var nameStart = index;
            while (index < attributes.Length
                   && !char.IsWhiteSpace(attributes[index])
                   && attributes[index] is not '=' and not '/' and not '>')
            {
                index++;
            }

            if (index == nameStart)
            {
                var snippetLength = Math.Min(40, attributes.Length - index);
                throw new InvalidDataException(
                    "Malformed HTML attribute near '"
                    + attributes.Substring(index, snippetLength)
                    + "'.");
            }

            var name = attributes[nameStart..index];
            while (index < attributes.Length && char.IsWhiteSpace(attributes[index]))
            {
                index++;
            }

            string? value = null;
            if (index < attributes.Length && attributes[index] == '=')
            {
                index++;
                while (index < attributes.Length && char.IsWhiteSpace(attributes[index]))
                {
                    index++;
                }

                if (index >= attributes.Length)
                {
                    throw new InvalidDataException(
                        $"HTML attribute '{name}' requires a value.");
                }

                if (attributes[index] is '"' or '\'')
                {
                    var quote = attributes[index++];
                    var valueStart = index;
                    while (index < attributes.Length && attributes[index] != quote)
                    {
                        index++;
                    }

                    if (index >= attributes.Length)
                    {
                        throw new InvalidDataException(
                            $"HTML attribute '{name}' has no closing quote.");
                    }

                    value = attributes[valueStart..index++];
                }
                else
                {
                    var valueStart = index;
                    while (index < attributes.Length
                           && !char.IsWhiteSpace(attributes[index])
                           && attributes[index] != '>')
                    {
                        index++;
                    }

                    value = attributes[valueStart..index];
                }
            }

            if (!values.TryAdd(name, WebUtility.HtmlDecode(value)))
            {
                throw new InvalidDataException(
                    $"HTML attribute '{name}' cannot be repeated.");
            }
        }

        return values;
    }

    private static bool TryReadTag(
        string document,
        int start,
        out HtmlTag tag)
    {
        tag = default;
        if (start < 0 || start >= document.Length || document[start] != '<')
        {
            return false;
        }

        if (document.AsSpan(start).StartsWith("<!--", StringComparison.Ordinal))
        {
            var commentEnd = document.IndexOf("-->", start + 4, StringComparison.Ordinal);
            if (commentEnd < 0)
            {
                throw new InvalidDataException("Unclosed HTML comment.");
            }

            tag = new HtmlTag(start, commentEnd + 2, "!--", false, true, string.Empty);
            return true;
        }

        var index = start + 1;
        var isClosing = index < document.Length && document[index] == '/';
        if (isClosing)
        {
            index++;
        }

        if (index >= document.Length || !IsTagNameStart(document[index]))
        {
            return false;
        }

        var nameStart = index;
        while (index < document.Length && IsTagNameCharacter(document[index]))
        {
            index++;
        }

        var name = document[nameStart..index];
        var attributesStart = index;
        var quote = '\0';
        var awaitingAttributeValue = false;
        var inUnquotedAttributeValue = false;
        var selfClosingSlash = false;
        for (; index < document.Length; index++)
        {
            var character = document[index];
            if (quote != '\0')
            {
                if (character == quote)
                {
                    quote = '\0';
                }

                continue;
            }

            if (inUnquotedAttributeValue)
            {
                if (char.IsWhiteSpace(character))
                {
                    inUnquotedAttributeValue = false;
                    continue;
                }

                if (character != '>')
                {
                    continue;
                }
            }

            if (awaitingAttributeValue)
            {
                if (char.IsWhiteSpace(character))
                {
                    continue;
                }

                awaitingAttributeValue = false;
                if (character is '"' or '\'')
                {
                    quote = character;
                    continue;
                }

                if (character != '>')
                {
                    inUnquotedAttributeValue = true;
                    continue;
                }
            }

            if (character == '>')
            {
                var attributes = document[attributesStart..index];
                tag = new HtmlTag(
                    start,
                    index,
                    name,
                    isClosing,
                    selfClosingSlash,
                    attributes);
                return true;
            }

            if (char.IsWhiteSpace(character))
            {
                continue;
            }

            if (character == '/')
            {
                selfClosingSlash = true;
                continue;
            }

            selfClosingSlash = false;
            if (character == '=')
            {
                awaitingAttributeValue = true;
            }
        }

        throw new InvalidDataException(
            $"Unclosed <{name}> tag at line "
            + $"{SourceText.GetLineNumber(document, start)}.");
    }

    private static bool IsOnlyWhitespace(string value, int start, int end)
    {
        for (var index = start; index < end; index++)
        {
            if (!char.IsWhiteSpace(value[index]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsTagNameStart(char value) =>
        char.IsAsciiLetter(value) || value is '!' or '?';

    private static bool IsTagNameCharacter(char value) =>
        char.IsAsciiLetterOrDigit(value) || value is '-' or ':' or '!' or '?';

    private readonly record struct HtmlTag(
        int Start,
        int End,
        string Name,
        bool IsClosing,
        bool IsSelfClosing,
        string Attributes);

    private sealed record OpenSelection(
        AnnotationKind? Kind,
        int Start);

    private sealed class NestedPreException(string message)
        : Exception(message);
}
