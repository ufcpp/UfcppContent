using System.Buffers;
using System.Globalization;
using System.Text;

namespace Ufcpp.SiteGenerator.Rendering;

internal static class AnnotationRangeMetadata
{
    public static IReadOnlyList<(int Start, int End)> Parse(
        string code,
        string value,
        string attributeName)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(attributeName);

        var lines = GetSourceLines(code, attributeName);
        var spans = new List<(int Start, int End)>();
        var canonical = new List<string>();
        foreach (var serializedRange in value.Split(
                     ',',
                     StringSplitOptions.None))
        {
            var separator = serializedRange.IndexOf('-');
            if (separator <= 0
                || separator != serializedRange.LastIndexOf('-')
                || separator == serializedRange.Length - 1)
            {
                throw InvalidSyntax(attributeName);
            }

            var startPosition = ParsePosition(
                serializedRange[..separator],
                lines,
                attributeName);
            var endPosition = ParsePosition(
                serializedRange[(separator + 1)..],
                lines,
                attributeName);
            if (endPosition.Offset <= startPosition.Offset)
            {
                throw InvalidSyntax(attributeName);
            }

            if (!code.AsSpan(
                    startPosition.Offset,
                    endPosition.Offset - startPosition.Offset)
                .ContainsAnyExcept('\r', '\n'))
            {
                throw new InvalidDataException(
                    $"A {attributeName} selection must contain code text.");
            }

            if (spans.Count > 0 && startPosition.Offset <= spans[^1].End)
            {
                throw new InvalidDataException(
                    $"The {attributeName} selections must be strictly ordered, "
                    + "disjoint, and non-adjacent.");
            }

            spans.Add((startPosition.Offset, endPosition.Offset));
            canonical.Add(
                $"{startPosition.Line}:{startPosition.Column}"
                + $"-{endPosition.Line}:{endPosition.Column}");
        }

        if (spans.Count == 0
            || !string.Equals(
                value,
                string.Join(',', canonical),
                StringComparison.Ordinal))
        {
            throw InvalidSyntax(attributeName);
        }

        return spans;
    }

    private static SourcePosition ParsePosition(
        string value,
        IReadOnlyList<SourceLine> lines,
        string attributeName)
    {
        var separator = value.IndexOf(':');
        if (separator <= 0
            || separator != value.LastIndexOf(':')
            || separator == value.Length - 1)
        {
            throw InvalidSyntax(attributeName);
        }

        var line = ParsePositiveDecimal(value[..separator], attributeName);
        var column = ParsePositiveDecimal(value[(separator + 1)..], attributeName);
        if (line > lines.Count
            || column > lines[line - 1].ScalarBoundaries.Count)
        {
            throw new InvalidDataException(
                $"The {attributeName} position {line}:{column} is outside "
                + "the code block.");
        }

        return new SourcePosition(
            line,
            column,
            lines[line - 1].ScalarBoundaries[column - 1]);
    }

    private static int ParsePositiveDecimal(string value, string attributeName)
    {
        if (value.Length == 0
            || value.Length > 1 && value[0] == '0'
            || !int.TryParse(
                value,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var number)
            || number <= 0)
        {
            throw InvalidSyntax(attributeName);
        }

        return number;
    }

    private static IReadOnlyList<SourceLine> GetSourceLines(
        string code,
        string attributeName)
    {
        var lines = new List<SourceLine>();
        for (var lineStart = 0; lineStart < code.Length;)
        {
            var contentEnd = lineStart;
            while (contentEnd < code.Length && code[contentEnd] is not '\r' and not '\n')
            {
                contentEnd++;
            }

            lines.Add(
                new SourceLine(
                    GetScalarBoundaries(
                        code,
                        lineStart,
                        contentEnd,
                        attributeName)));
            if (contentEnd == code.Length)
            {
                break;
            }

            lineStart = contentEnd + 1;
            if (code[contentEnd] == '\r'
                && lineStart < code.Length
                && code[lineStart] == '\n')
            {
                lineStart++;
            }
        }

        return lines;
    }

    private static IReadOnlyList<int> GetScalarBoundaries(
        string code,
        int start,
        int end,
        string attributeName)
    {
        var boundaries = new List<int> { start };
        for (var offset = start; offset < end;)
        {
            var status = Rune.DecodeFromUtf16(
                code.AsSpan(offset, end - offset),
                out var rune,
                out var consumed);
            if (status != OperationStatus.Done)
            {
                throw new InvalidDataException(
                    "Code blocks with unpaired UTF-16 surrogates cannot use "
                    + $"{attributeName}.");
            }

            offset += consumed;
            boundaries.Add(offset);
        }

        return boundaries;
    }

    private static InvalidDataException InvalidSyntax(string attributeName) =>
        new(
            $"The {attributeName} attribute must contain canonical 1-based, "
            + "end-exclusive line:column ranges.");

    private sealed record SourceLine(IReadOnlyList<int> ScalarBoundaries);

    private readonly record struct SourcePosition(
        int Line,
        int Column,
        int Offset);
}
