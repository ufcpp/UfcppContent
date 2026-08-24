using System.Buffers;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record DocumentSourceReplacement(
    int Offset,
    int Length,
    string Value);

internal static partial class RawTableAnnotationRewriter
{
    private const string MarkOpening = "<mark class=\"code-highlight\">";
    private const string MarkClosing = "</mark>";

    [GeneratedRegex(
        """(?:^|\s)title\s*=\s*(?<quote>["'])(?<value>.*?)\k<quote>""",
        RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex TitleAttributeRegex();

    public static IReadOnlyList<DocumentSourceReplacement> Plan(
        string path,
        string source,
        CurrentCodeBlock current,
        ReportPlan plan)
    {
        var openingEnd = FindTagEnd(source, current.SourceOffset);
        var closingStart = source.IndexOf(
            "</pre>",
            openingEnd + 1,
            StringComparison.OrdinalIgnoreCase);
        if (closingStart < 0)
        {
            throw Invalid(path, current, "has no closing </pre> tag.");
        }

        var replacements = new List<DocumentSourceReplacement>();
        var opening = source[current.SourceOffset..(openingEnd + 1)];
        if (plan.Metadata.Title is { } title)
        {
            var match = TitleAttributeRegex().Match(opening);
            if (match.Success)
            {
                var existing = WebUtility.HtmlDecode(match.Groups["value"].Value);
                if (!string.Equals(existing, title, StringComparison.Ordinal))
                {
                    throw Invalid(path, current, "has a conflicting title.");
                }
            }
            else
            {
                var encoded = WebUtility.HtmlEncode(title);
                var updated = opening.Insert(opening.Length - 1, $" title=\"{encoded}\"");
                replacements.Add(
                    new DocumentSourceReplacement(
                        current.SourceOffset,
                        opening.Length,
                        updated));
            }
        }

        if (plan.Metadata.Highlight is not { } highlight)
        {
            return replacements;
        }

        var codeOpening = source.IndexOf(
            "<code",
            openingEnd + 1,
            closingStart - openingEnd - 1,
            StringComparison.OrdinalIgnoreCase);
        if (codeOpening < 0)
        {
            throw Invalid(path, current, "has no structural <code> wrapper.");
        }

        var codeOpeningEnd = FindTagEnd(source, codeOpening);
        var codeClosing = source.LastIndexOf(
            "</code>",
            closingStart,
            closingStart - codeOpeningEnd,
            StringComparison.OrdinalIgnoreCase);
        if (codeClosing < 0)
        {
            throw Invalid(path, current, "has no closing </code> wrapper.");
        }

        var bodyStart = codeOpeningEnd + 1;
        var originalBody = source[bodyStart..codeClosing];
        var cleanBody = RemoveExistingMarks(path, current, originalBody);
        var map = RawVisibleMap.Create(cleanBody);
        if (!string.Equals(map.Text, current.Code, StringComparison.Ordinal))
        {
            throw Invalid(
                path,
                current,
                "does not map exactly to its discovered visible code.");
        }

        var codeRanges = GetCodeRanges(current.Code, highlight);
        var rawRanges = new List<HighlightSourceRange>(codeRanges.Count);
        foreach (var range in codeRanges)
        {
            var candidates = new HashSet<HighlightSourceRange>();
            foreach (var start in map.GetRawBoundaries(range.Start))
            {
                foreach (var end in map.GetRawBoundaries(range.End)
                             .Where(end => end > start))
                {
                    var slice = cleanBody[start..end];
                    if (slice.Contains('<', StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (string.Equals(
                            WebUtility.HtmlDecode(slice),
                            current.Code[range.Start..range.End],
                            StringComparison.Ordinal))
                    {
                        candidates.Add(new HighlightSourceRange(start, end));
                    }
                }
            }

            if (candidates.Count != 1)
            {
                throw Invalid(
                    path,
                    current,
                    "has no unique exact raw source range for a highlight.");
            }

            rawRanges.Add(candidates.Single());
        }

        var merged = Merge(rawRanges);
        var desiredBody = cleanBody;
        foreach (var range in merged.OrderByDescending(static range => range.Start))
        {
            desiredBody = desiredBody.Insert(range.End, MarkClosing)
                .Insert(range.Start, MarkOpening);
        }

        if (!string.Equals(originalBody, desiredBody, StringComparison.Ordinal))
        {
            replacements.Add(
                new DocumentSourceReplacement(
                    bodyStart,
                    originalBody.Length,
                    desiredBody));
        }

        return replacements;
    }

    private static string RemoveExistingMarks(
        string path,
        CurrentCodeBlock current,
        string body)
    {
        if (body.Contains("<mark", StringComparison.OrdinalIgnoreCase)
            && !body.Contains(MarkOpening, StringComparison.Ordinal))
        {
            throw Invalid(path, current, "contains an unsupported existing <mark>.");
        }

        var openingCount = CountOccurrences(body, MarkOpening);
        var closingCount = CountOccurrences(body, MarkClosing);
        if (openingCount != closingCount)
        {
            throw Invalid(path, current, "contains unbalanced highlight marks.");
        }

        return body.Replace(MarkOpening, string.Empty, StringComparison.Ordinal)
            .Replace(MarkClosing, string.Empty, StringComparison.Ordinal);
    }

    private static IReadOnlyList<HighlightSourceRange> GetCodeRanges(
        string code,
        SelectionMetadataPlan highlight)
    {
        var ranges = new List<HighlightSourceRange>();
        if (highlight.Lines is not null)
        {
            ranges.AddRange(ParseLineRanges(code, highlight.Lines));
        }

        if (highlight.Text is not null)
        {
            var start = code.IndexOf(highlight.Text, StringComparison.Ordinal);
            if (start < 0
                || code.IndexOf(
                    highlight.Text,
                    start + 1,
                    StringComparison.Ordinal) >= 0)
            {
                throw new InvalidDataException(
                    "A raw-table highlight-text value must occur exactly once.");
            }

            ranges.Add(new HighlightSourceRange(start, start + highlight.Text.Length));
        }

        if (highlight.Ranges is not null)
        {
            ranges.AddRange(HighlightRangePlanner.Parse(code, highlight.Ranges));
        }

        return Merge(ranges);
    }

    private static IReadOnlyList<HighlightSourceRange> ParseLineRanges(
        string code,
        string value)
    {
        var lines = GetLines(code);
        var ranges = new List<HighlightSourceRange>();
        foreach (var item in value.Split(',', StringSplitOptions.None))
        {
            var values = item.Split('-', StringSplitOptions.None);
            if (values.Length is < 1 or > 2
                || !int.TryParse(values[0], out var startLine)
                || values.Length == 2 && !int.TryParse(values[1], out _))
            {
                throw new InvalidDataException("Invalid raw-table highlight-lines value.");
            }

            var endLine = values.Length == 1 ? startLine : int.Parse(values[1]);
            if (startLine <= 0 || endLine < startLine || endLine > lines.Count)
            {
                throw new InvalidDataException(
                    "Raw-table highlight-lines value is out of bounds.");
            }

            ranges.Add(
                new HighlightSourceRange(
                    lines[startLine - 1].Start,
                    lines[endLine - 1].End));
        }

        return ranges;
    }

    private static IReadOnlyList<HighlightSourceRange> GetLines(string code)
    {
        var lines = new List<HighlightSourceRange>();
        for (var start = 0; start < code.Length;)
        {
            var end = start;
            while (end < code.Length && code[end] is not '\r' and not '\n')
            {
                end++;
            }

            lines.Add(new HighlightSourceRange(start, end));
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

    private static IReadOnlyList<HighlightSourceRange> Merge(
        IEnumerable<HighlightSourceRange> ranges)
    {
        var ordered = ranges
            .OrderBy(static range => range.Start)
            .ThenBy(static range => range.End)
            .ToArray();
        var merged = new List<HighlightSourceRange>();
        foreach (var range in ordered)
        {
            if (range.End <= range.Start)
            {
                throw new InvalidDataException("A raw-table highlight range is empty.");
            }

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

        return merged;
    }

    private static int FindTagEnd(string source, int start)
    {
        var quote = '\0';
        for (var index = start; index < source.Length; index++)
        {
            if (quote == '\0' && source[index] is '"' or '\'')
            {
                quote = source[index];
            }
            else if (source[index] == quote)
            {
                quote = '\0';
            }
            else if (quote == '\0' && source[index] == '>')
            {
                return index;
            }
        }

        throw new InvalidDataException("Unclosed raw HTML tag.");
    }

    private static int CountOccurrences(string value, string text)
    {
        var count = 0;
        for (var offset = 0; offset <= value.Length - text.Length;)
        {
            var found = value.IndexOf(text, offset, StringComparison.Ordinal);
            if (found < 0)
            {
                break;
            }

            count++;
            offset = found + text.Length;
        }

        return count;
    }

    private static InvalidDataException Invalid(
        string path,
        CurrentCodeBlock current,
        string message) =>
        new(
            $"Raw block {current.Ordinal} in '{path}' {message}");

    private sealed class RawVisibleMap
    {
        private readonly IReadOnlyDictionary<int, IReadOnlyList<int>> _boundaries;

        private RawVisibleMap(
            string text,
            IReadOnlyDictionary<int, IReadOnlyList<int>> boundaries)
        {
            Text = text;
            _boundaries = boundaries;
        }

        public string Text { get; }

        public static RawVisibleMap Create(string source)
        {
            var text = new StringBuilder(source.Length);
            var boundaries = new Dictionary<int, SortedSet<int>>
            {
                [0] = [0],
            };
            for (var offset = 0; offset < source.Length;)
            {
                if (source[offset] == '<')
                {
                    var end = FindTagEnd(source, offset) + 1;
                    var tag = source[offset..end];
                    AddBoundary(text.Length, offset);
                    if (Regex.IsMatch(
                            tag,
                            """^<br(?:\s[^>]*)?/?>$""",
                            RegexOptions.IgnoreCase))
                    {
                        text.Append('\n');
                    }

                    AddBoundary(text.Length, end);
                    offset = end;
                    continue;
                }

                if (source[offset] == '&')
                {
                    var end = source.IndexOf(';', offset + 1);
                    if (end >= 0 && end - offset <= 64)
                    {
                        end++;
                        var encoded = source[offset..end];
                        var decoded = WebUtility.HtmlDecode(encoded);
                        if (!string.Equals(encoded, decoded, StringComparison.Ordinal))
                        {
                            AddBoundary(text.Length, offset);
                            text.Append(decoded);
                            AddBoundary(text.Length, end);
                            offset = end;
                            continue;
                        }
                    }
                }

                var status = Rune.DecodeFromUtf16(
                    source.AsSpan(offset),
                    out var rune,
                    out var consumed);
                if (status != OperationStatus.Done)
                {
                    throw new InvalidDataException(
                        "Unpaired UTF-16 surrogate in raw code.");
                }

                AddBoundary(text.Length, offset);
                text.Append(rune.ToString());
                offset += consumed;
                AddBoundary(text.Length, offset);
            }

            return new RawVisibleMap(
                text.ToString(),
                boundaries.ToDictionary(
                    static item => item.Key,
                    static item => (IReadOnlyList<int>)item.Value.ToArray()));

            void AddBoundary(int visibleOffset, int rawOffset)
            {
                if (!boundaries.TryGetValue(visibleOffset, out var candidates))
                {
                    candidates = [];
                    boundaries.Add(visibleOffset, candidates);
                }

                candidates.Add(rawOffset);
            }
        }

        public IReadOnlyList<int> GetRawBoundaries(int visibleOffset) =>
            _boundaries.GetValueOrDefault(visibleOffset) ?? [];
    }
}
