using System.Text;
using Markdig;
using Markdig.Syntax;

namespace Ufcpp.CodeAnnotationMigrator;

internal static class CurrentBlockDiscoverer
{
    public static IReadOnlyList<CurrentCodeBlock> Discover(string document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var sanitized = SanitizeBlankLines(document);
        var candidates = Markdown.Parse(sanitized.Text)
            .Descendants()
            .OfType<FencedCodeBlock>()
            .Select(block => CreateFencedCandidate(document, sanitized, block))
            .Concat(LegacyPreParser.Parse(document).Select(block =>
                new CurrentBlockCandidate(
                    block.SourceOffset,
                    block.SourceLine,
                    CurrentCodeBlockKind.RawPre,
                    block.IsInsideTable,
                    block.Code)))
            .OrderBy(static block => block.SourceOffset)
            .ToArray();

        return candidates
            .Select((block, index) => new CurrentCodeBlock(
                index + 1,
                block.SourceOffset,
                block.SourceLine,
                block.Kind,
                block.IsInsideTable,
                block.Code))
            .ToArray();
    }

    private static CurrentBlockCandidate CreateFencedCandidate(
        string document,
        SanitizedDocument sanitized,
        FencedCodeBlock block)
    {
        var sourceOffset = sanitized.GetOriginalOffset(block.Span.Start);
        if (block.IsOpen)
        {
            throw new InvalidDataException(
                $"Fenced code block at line "
                + $"{SourceText.GetLineNumber(document, sourceOffset)} "
                + "has no valid closing fence.");
        }

        return new CurrentBlockCandidate(
            sourceOffset,
            SourceText.GetLineNumber(document, sourceOffset),
            CurrentCodeBlockKind.Fenced,
            false,
            block.Lines.ToString());
    }

    private static SanitizedDocument SanitizeBlankLines(string document)
    {
        var text = new StringBuilder(document.Length);
        var originalOffsets = new List<int>(document.Length);
        char? fenceMarker = null;
        var fenceLength = 0;
        for (var lineStart = 0; lineStart < document.Length;)
        {
            var contentEnd = lineStart;
            while (contentEnd < document.Length
                   && document[contentEnd] is not '\r' and not '\n')
            {
                contentEnd++;
            }

            var line = document.AsSpan(lineStart, contentEnd - lineStart);
            if (fenceMarker is null)
            {
                if (TryReadFence(line, closingOnly: false, out var marker, out var length))
                {
                    fenceMarker = marker;
                    fenceLength = length;
                }
            }
            else if (TryReadFence(line, closingOnly: true, out var marker, out var length)
                     && marker == fenceMarker
                     && length >= fenceLength)
            {
                fenceMarker = null;
                fenceLength = 0;
            }

            if (fenceMarker is not null || !line.IsWhiteSpace())
            {
                Append(lineStart, contentEnd);
            }

            if (contentEnd < document.Length)
            {
                var separatorEnd = contentEnd + 1;
                if (document[contentEnd] == '\r'
                    && separatorEnd < document.Length
                    && document[separatorEnd] == '\n')
                {
                    separatorEnd++;
                }

                Append(contentEnd, separatorEnd);
                lineStart = separatorEnd;
            }
            else
            {
                lineStart = contentEnd;
            }
        }

        return new SanitizedDocument(text.ToString(), originalOffsets, document.Length);

        void Append(int start, int end)
        {
            for (var index = start; index < end; index++)
            {
                text.Append(document[index]);
                originalOffsets.Add(index);
            }
        }
    }

    private static bool TryReadFence(
        ReadOnlySpan<char> line,
        bool closingOnly,
        out char marker,
        out int length)
    {
        var offset = 0;
        while (true)
        {
            var beforeQuote = offset;
            while (offset < line.Length
                   && offset - beforeQuote < 3
                   && line[offset] == ' ')
            {
                offset++;
            }

            if (offset >= line.Length || line[offset] != '>')
            {
                break;
            }

            offset++;
            if (offset < line.Length && line[offset] is ' ' or '\t')
            {
                offset++;
            }
        }

        var indentationStart = offset;
        while (offset < line.Length
               && offset - indentationStart < 3
               && line[offset] == ' ')
        {
            offset++;
        }

        marker = offset < line.Length ? line[offset] : '\0';
        if (marker is not '`' and not '~')
        {
            length = 0;
            return false;
        }

        var markerStart = offset;
        while (offset < line.Length && line[offset] == marker)
        {
            offset++;
        }

        length = offset - markerStart;
        if (length < 3)
        {
            return false;
        }

        var remainder = line[offset..];
        return closingOnly
            ? remainder.IsWhiteSpace()
            : marker != '`' || !remainder.Contains('`');
    }

    private sealed record CurrentBlockCandidate(
        int SourceOffset,
        int SourceLine,
        CurrentCodeBlockKind Kind,
        bool IsInsideTable,
        string Code);

    private sealed record SanitizedDocument(
        string Text,
        IReadOnlyList<int> OriginalOffsets,
        int OriginalLength)
    {
        public int GetOriginalOffset(int sanitizedOffset) =>
            sanitizedOffset == OriginalOffsets.Count
                ? OriginalLength
                : OriginalOffsets[sanitizedOffset];
    }
}
