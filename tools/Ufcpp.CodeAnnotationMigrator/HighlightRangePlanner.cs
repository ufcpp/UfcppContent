using System.Buffers;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record HighlightRangePlanningResult(
    string? Value,
    string? Error);

internal readonly record struct HighlightSourceRange(int Start, int End);

internal static class HighlightRangePlanner
{
    public static HighlightRangePlanningResult Plan(
        IReadOnlyList<AnnotationSelection> selections,
        string historicalCode,
        string currentCode)
    {
        ArgumentNullException.ThrowIfNull(selections);
        ArgumentNullException.ThrowIfNull(historicalCode);
        ArgumentNullException.ThrowIfNull(currentCode);

        NormalizedProjection historical;
        NormalizedProjection current;
        try
        {
            historical = NormalizedProjection.Create(historicalCode);
            current = NormalizedProjection.Create(currentCode);
        }
        catch (InvalidDataException exception)
        {
            return new HighlightRangePlanningResult(null, exception.Message);
        }

        if (!string.Equals(
                historical.Normalized,
                current.Normalized,
                StringComparison.Ordinal)
            || historical.Tokens.Count != current.Tokens.Count)
        {
            return new HighlightRangePlanningResult(
                null,
                "Range planning requires equal normalized scalar sequences.");
        }

        var mapped = new List<HighlightSourceRange>();
        foreach (var selection in selections)
        {
            var selectionEnd = selection.Start + selection.Length;
            if (selection.Start < 0
                || selection.Length <= 0
                || selectionEnd > historicalCode.Length)
            {
                return new HighlightRangePlanningResult(
                    null,
                    "A historical highlight selection is outside its code block.");
            }

            var historicalStarts = historical.FindBoundaryIndexes(selection.Start);
            var historicalEnds = historical.FindBoundaryIndexes(selectionEnd);
            var candidates = new HashSet<HighlightSourceRange>();
            var expected = NormalizeNewlines(WebUtility.HtmlDecode(selection.Text));
            foreach (var startIndex in historicalStarts)
            {
                foreach (var endIndex in historicalEnds.Where(index => index > startIndex))
                {
                    if (!string.Equals(
                            historical.GetText(startIndex, endIndex),
                            expected,
                            StringComparison.Ordinal))
                    {
                        continue;
                    }

                    foreach (var currentStart in current.Boundaries[startIndex])
                    {
                        foreach (var currentEnd in current.Boundaries[endIndex]
                                     .Where(offset => offset > currentStart))
                        {
                            var sourceText = NormalizeNewlines(
                                WebUtility.HtmlDecode(
                                    currentCode[currentStart..currentEnd]));
                            if (string.Equals(sourceText, expected, StringComparison.Ordinal))
                            {
                                candidates.Add(
                                    new HighlightSourceRange(currentStart, currentEnd));
                            }
                        }
                    }
                }
            }

            if (candidates.Count == 0
                && TryMapTrailingWhitespace(
                    selection,
                    historicalCode,
                    currentCode,
                    out var trailingRange))
            {
                candidates.Add(trailingRange);
            }

            if (candidates.Count != 1)
            {
                return new HighlightRangePlanningResult(
                    null,
                    candidates.Count == 0
                        ? "A highlight boundary does not project to an exact current "
                            + "source boundary."
                        : "A highlight selection has multiple exact current source "
                            + "projections.");
            }

            mapped.Add(candidates.Single());
        }

        mapped.Sort(static (left, right) =>
        {
            var start = left.Start.CompareTo(right.Start);
            return start != 0 ? start : left.End.CompareTo(right.End);
        });
        var merged = new List<HighlightSourceRange>();
        foreach (var range in mapped)
        {
            if (merged.Count != 0 && range.Start <= merged[^1].End)
            {
                merged[^1] = new HighlightSourceRange(
                    merged[^1].Start,
                    Math.Max(merged[^1].End, range.End));
            }
            else
            {
                merged.Add(range);
            }
        }

        var coordinates = SourceCoordinates.Create(currentCode);
        var serialized = new List<string>(merged.Count);
        foreach (var range in merged)
        {
            if (!coordinates.TryGetPosition(range.Start, out var start)
                || !coordinates.TryGetPosition(range.End, out var end))
            {
                return new HighlightRangePlanningResult(
                    null,
                    "A projected highlight boundary is not an addressable "
                    + "line/column position.");
            }

            serialized.Add($"{start}-{end}");
        }

        return new HighlightRangePlanningResult(
            $"sha256:{ComputeHash(currentCode)};{string.Join(',', serialized)}",
            null);
    }

    internal static string ComputeHash(string code)
    {
        var normalized = NormalizeNewlines(code);
        return Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(normalized)))
            .ToLowerInvariant();
    }

    internal static IReadOnlyList<HighlightSourceRange> Parse(
        string code,
        string value)
    {
        const string Prefix = "sha256:";
        if (!value.StartsWith(Prefix, StringComparison.Ordinal)
            || value.Length <= Prefix.Length + 65
            || value[Prefix.Length + 64] != ';')
        {
            throw new InvalidDataException("Invalid highlight-ranges syntax.");
        }

        var hash = value.Substring(Prefix.Length, 64);
        if (hash.Any(static character =>
                character is not (>= '0' and <= '9' or >= 'a' and <= 'f'))
            || !string.Equals(hash, ComputeHash(code), StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "The highlight-ranges fingerprint is invalid or stale.");
        }

        var coordinates = SourceCoordinates.Create(code);
        var serialized = value[(Prefix.Length + 65)..];
        var canonical = new List<string>();
        var ranges = new List<HighlightSourceRange>();
        foreach (var item in serialized.Split(',', StringSplitOptions.None))
        {
            var separator = item.IndexOf('-');
            if (separator <= 0
                || separator != item.LastIndexOf('-')
                || separator == item.Length - 1)
            {
                throw new InvalidDataException("Invalid highlight-ranges syntax.");
            }

            var start = ParsePosition(item[..separator]);
            var end = ParsePosition(item[(separator + 1)..]);
            if (!coordinates.TryGetOffset(start, out var startOffset)
                || !coordinates.TryGetOffset(end, out var endOffset)
                || endOffset <= startOffset
                || ranges.Count != 0 && startOffset <= ranges[^1].End)
            {
                throw new InvalidDataException(
                    "The highlight-ranges coordinates are out of bounds, "
                    + "unordered, overlapping, or adjacent.");
            }

            ranges.Add(new HighlightSourceRange(startOffset, endOffset));
            canonical.Add($"{start}-{end}");
        }

        if (ranges.Count == 0
            || !string.Equals(serialized, string.Join(',', canonical), StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "The highlight-ranges value is not canonical.");
        }

        return ranges;
    }

    private static string NormalizeNewlines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');

    private static bool TryMapTrailingWhitespace(
        AnnotationSelection selection,
        string historicalCode,
        string currentCode,
        out HighlightSourceRange range)
    {
        range = default;
        if (selection.Text.Length == 0
            || selection.Text.Any(static character => character is not ' ' and not '\t'))
        {
            return false;
        }

        var historicalLines = GetPhysicalLines(historicalCode);
        var historicalLineIndex = historicalLines.FindIndex(line =>
            selection.Start >= line.Start
            && selection.Start + selection.Length <= line.End);
        if (historicalLineIndex < 0)
        {
            return false;
        }

        var historicalLine = historicalLines[historicalLineIndex];
        if (selection.Start + selection.Length != historicalLine.End
            || !historicalCode.AsSpan(selection.Start, selection.Length)
                .SequenceEqual(selection.Text))
        {
            return false;
        }

        var currentLines = GetPhysicalLines(currentCode);
        var historicalLeading = historicalLines
            .TakeWhile(line => string.IsNullOrWhiteSpace(
                historicalCode[line.Start..line.End].TrimEnd(' ', '\t')))
            .Count();
        var currentLeading = currentLines
            .TakeWhile(line => string.IsNullOrWhiteSpace(
                currentCode[line.Start..line.End].TrimEnd(' ', '\t')))
            .Count();
        var normalizedLine = historicalLineIndex - historicalLeading;
        var currentLineIndex = currentLeading + normalizedLine;
        if (normalizedLine < 0
            || currentLineIndex < 0
            || currentLineIndex >= currentLines.Count)
        {
            return false;
        }

        var currentLine = currentLines[currentLineIndex];
        var currentStart = currentLine.End - selection.Length;
        if (currentStart < currentLine.Start
            || !currentCode.AsSpan(currentStart, selection.Length)
                .SequenceEqual(selection.Text))
        {
            return false;
        }

        range = new HighlightSourceRange(currentStart, currentLine.End);
        return true;
    }

    private static List<PhysicalLine> GetPhysicalLines(string code)
    {
        var lines = new List<PhysicalLine>();
        for (var start = 0; start < code.Length;)
        {
            var end = start;
            while (end < code.Length && code[end] is not '\r' and not '\n')
            {
                end++;
            }

            lines.Add(new PhysicalLine(start, end));
            if (end == code.Length)
            {
                break;
            }

            start = end + 1;
            if (code[end] == '\r' && start < code.Length && code[start] == '\n')
            {
                start++;
            }
        }

        return lines;
    }

    private readonly record struct SourcePosition(int Line, int Column)
    {
        public override string ToString() =>
            $"{Line.ToString(CultureInfo.InvariantCulture)}"
            + $":{Column.ToString(CultureInfo.InvariantCulture)}";
    }

    private readonly record struct PhysicalLine(int Start, int End);

    private sealed class SourceCoordinates
    {
        private readonly IReadOnlyDictionary<int, SourcePosition> _positions;
        private readonly IReadOnlyDictionary<SourcePosition, int> _offsets;

        private SourceCoordinates(
            IReadOnlyDictionary<int, SourcePosition> positions,
            IReadOnlyDictionary<SourcePosition, int> offsets)
        {
            _positions = positions;
            _offsets = offsets;
        }

        public static SourceCoordinates Create(string code)
        {
            var positions = new Dictionary<int, SourcePosition>();
            var line = 1;
            for (var lineStart = 0; lineStart < code.Length; line++)
            {
                var contentEnd = lineStart;
                while (contentEnd < code.Length
                       && code[contentEnd] is not '\r' and not '\n')
                {
                    contentEnd++;
                }

                var column = 1;
                positions.Add(lineStart, new SourcePosition(line, column));
                for (var offset = lineStart; offset < contentEnd;)
                {
                    var status = Rune.DecodeFromUtf16(
                        code.AsSpan(offset, contentEnd - offset),
                        out _,
                        out var consumed);
                    if (status != OperationStatus.Done)
                    {
                        throw new InvalidDataException(
                            "Unpaired UTF-16 surrogate in current code.");
                    }

                    offset += consumed;
                    positions.Add(offset, new SourcePosition(line, ++column));
                }

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

            return new SourceCoordinates(
                positions,
                positions.ToDictionary(static item => item.Value, static item => item.Key));
        }

        public bool TryGetPosition(int offset, out SourcePosition position) =>
            _positions.TryGetValue(offset, out position);

        public bool TryGetOffset(SourcePosition position, out int offset) =>
            _offsets.TryGetValue(position, out offset);
    }

    private static SourcePosition ParsePosition(string value)
    {
        var separator = value.IndexOf(':');
        if (separator <= 0
            || separator != value.LastIndexOf(':')
            || separator == value.Length - 1)
        {
            throw new InvalidDataException("Invalid highlight-ranges position.");
        }

        return new SourcePosition(
            ParsePositiveDecimal(value[..separator]),
            ParsePositiveDecimal(value[(separator + 1)..]));
    }

    private static int ParsePositiveDecimal(string value)
    {
        if (value.Length == 0
            || value.Length > 1 && value[0] == '0'
            || !int.TryParse(
                value,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var result)
            || result <= 0)
        {
            throw new InvalidDataException("Invalid highlight-ranges number.");
        }

        return result;
    }

    private sealed class NormalizedProjection
    {
        private NormalizedProjection(
            IReadOnlyList<ProjectedToken> tokens,
            IReadOnlyList<IReadOnlyList<int>> boundaries)
        {
            Tokens = tokens;
            Boundaries = boundaries;
            Normalized = string.Concat(tokens.Select(static token => token.Value));
        }

        public string Normalized { get; }

        public IReadOnlyList<ProjectedToken> Tokens { get; }

        public IReadOnlyList<IReadOnlyList<int>> Boundaries { get; }

        public static NormalizedProjection Create(string source)
        {
            var decoded = Decode(source);
            var canonical = NormalizeNewlineTokens(decoded);
            var lines = SplitLines(canonical);
            foreach (var line in lines)
            {
                while (line.Content.Count != 0
                       && line.Content[^1].Value is " " or "\t")
                {
                    line.Content.RemoveAt(line.Content.Count - 1);
                }
            }

            var first = 0;
            while (first < lines.Count && IsBlank(lines[first].Content))
            {
                first++;
            }

            var last = lines.Count - 1;
            while (last >= first && IsBlank(lines[last].Content))
            {
                last--;
            }

            if (last < first)
            {
                return new NormalizedProjection([], [[]]);
            }

            var retained = lines[first..(last + 1)];
            var indentation = CommonIndent(retained);
            var normalized = new List<ProjectedToken>();
            for (var index = 0; index < retained.Count; index++)
            {
                var content = retained[index].Content;
                normalized.AddRange(content.Skip(Math.Min(indentation.Count, content.Count)));
                if (index + 1 < retained.Count)
                {
                    normalized.Add(
                        retained[index].Separator
                        ?? throw new InvalidDataException(
                            "A retained logical line has no separator."));
                }
            }

            var expected = CodeNormalizer.Normalize(source);
            var actual = string.Concat(normalized.Select(static token => token.Value));
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "The range projection does not reproduce matching normalization.");
            }

            var boundaries = new List<IReadOnlyList<int>>(normalized.Count + 1);
            for (var index = 0; index <= normalized.Count; index++)
            {
                var candidates = new SortedSet<int>();
                if (index > 0 && normalized[index - 1].BoundaryAfter is { } after)
                {
                    candidates.Add(after);
                }

                if (index < normalized.Count
                    && normalized[index].BoundaryBefore is { } before)
                {
                    candidates.Add(before);
                }

                boundaries.Add(candidates.ToArray());
            }

            return new NormalizedProjection(normalized, boundaries);
        }

        public IReadOnlyList<int> FindBoundaryIndexes(int sourceOffset) =>
            Boundaries
                .Select((candidates, index) => (candidates, index))
                .Where(item => item.candidates.Contains(sourceOffset))
                .Select(static item => item.index)
                .ToArray();

        public string GetText(int start, int end) =>
            string.Concat(Tokens.Skip(start).Take(end - start)
                .Select(static token => token.Value));

        private static List<ProjectedToken> Decode(string source)
        {
            var tokens = new List<ProjectedToken>();
            for (var offset = 0; offset < source.Length;)
            {
                if (source[offset] == '&'
                    && TryDecodeEntity(source, offset, out var decoded, out var entityEnd))
                {
                    var runes = decoded.EnumerateRunes().ToArray();
                    for (var index = 0; index < runes.Length; index++)
                    {
                        tokens.Add(new ProjectedToken(
                            runes[index].ToString(),
                            index == 0 ? offset : null,
                            index == runes.Length - 1 ? entityEnd : null));
                    }

                    offset = entityEnd;
                    continue;
                }

                var status = Rune.DecodeFromUtf16(
                    source.AsSpan(offset),
                    out var rune,
                    out var consumed);
                if (status != OperationStatus.Done)
                {
                    throw new InvalidDataException(
                        "Unpaired UTF-16 surrogate in annotation source.");
                }

                tokens.Add(new ProjectedToken(
                    rune.ToString(),
                    offset,
                    offset + consumed));
                offset += consumed;
            }

            var decodedSource = WebUtility.HtmlDecode(source);
            if (!string.Equals(
                    decodedSource,
                    string.Concat(tokens.Select(static token => token.Value)),
                    StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    "HTML entity decoding cannot be projected exactly.");
            }

            return tokens;
        }

        private static bool TryDecodeEntity(
            string source,
            int start,
            out string decoded,
            out int end)
        {
            end = source.IndexOf(';', start + 1);
            if (end < 0 || end - start > 64)
            {
                decoded = string.Empty;
                end = start;
                return false;
            }

            end++;
            var encoded = source[start..end];
            decoded = WebUtility.HtmlDecode(encoded);
            if (string.Equals(encoded, decoded, StringComparison.Ordinal))
            {
                end = start;
                return false;
            }

            return true;
        }

        private static List<ProjectedToken> NormalizeNewlineTokens(
            IReadOnlyList<ProjectedToken> tokens)
        {
            var normalized = new List<ProjectedToken>(tokens.Count);
            for (var index = 0; index < tokens.Count; index++)
            {
                var token = tokens[index];
                if (token.Value == "\r")
                {
                    var following = index + 1 < tokens.Count ? tokens[index + 1] : null;
                    if (following?.Value == "\n")
                    {
                        normalized.Add(new ProjectedToken(
                            "\n",
                            token.BoundaryBefore,
                            following.BoundaryAfter));
                        index++;
                    }
                    else
                    {
                        normalized.Add(token with { Value = "\n" });
                    }
                }
                else
                {
                    normalized.Add(token);
                }
            }

            return normalized;
        }

        private static List<ProjectedLine> SplitLines(
            IReadOnlyList<ProjectedToken> tokens)
        {
            var lines = new List<ProjectedLine>();
            var content = new List<ProjectedToken>();
            foreach (var token in tokens)
            {
                if (token.Value == "\n")
                {
                    lines.Add(new ProjectedLine(content, token));
                    content = [];
                }
                else
                {
                    content.Add(token);
                }
            }

            lines.Add(new ProjectedLine(content, null));
            return lines;
        }

        private static bool IsBlank(IReadOnlyList<ProjectedToken> tokens) =>
            string.Concat(tokens.Select(static token => token.Value))
                .All(char.IsWhiteSpace);

        private static IReadOnlyList<string> CommonIndent(
            IReadOnlyList<ProjectedLine> lines)
        {
            string[]? common = null;
            foreach (var line in lines.Where(static line => line.Content.Count != 0))
            {
                var indentation = line.Content
                    .TakeWhile(static token => token.Value is " " or "\t")
                    .Select(static token => token.Value)
                    .ToArray();
                if (common is null)
                {
                    common = indentation;
                    continue;
                }

                var length = 0;
                while (length < common.Length
                       && length < indentation.Length
                       && common[length] == indentation[length])
                {
                    length++;
                }

                common = common[..length];
            }

            return common ?? [];
        }

        private sealed record ProjectedLine(
            List<ProjectedToken> Content,
            ProjectedToken? Separator);
    }

    private sealed record ProjectedToken(
        string Value,
        int? BoundaryBefore,
        int? BoundaryAfter);
}
