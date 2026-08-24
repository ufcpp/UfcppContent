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
    private const string ErrorOpening = "<span class=\"error\">";
    private const string WarningOpening = "<span class=\"warning\">";
    private const string SpanClosing = "</span>";

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

        var plannedSelections =
            new Dictionary<RawAnnotationKind, SelectionMetadataPlan>();
        AddPlannedSelection(RawAnnotationKind.Highlight, plan.Metadata.Highlight);
        AddPlannedSelection(RawAnnotationKind.Error, plan.Metadata.Error);
        AddPlannedSelection(RawAnnotationKind.Warning, plan.Metadata.Warning);
        if (plannedSelections.Count == 0)
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
        var cleanBody = RemoveExistingAnnotations(path, current, originalBody);
        var existingAnnotations = GetExistingAnnotations(
            path,
            current,
            originalBody);
        var map = RawVisibleMap.Create(cleanBody);
        if (!string.Equals(map.Text, current.Code, StringComparison.Ordinal))
        {
            throw Invalid(
                path,
                current,
                "does not map exactly to its discovered visible code.");
        }

        var rawRanges = new List<TypedSourceRange>();
        var rawIdentities = new List<TypedDiagnosticSourceRange>();
        foreach (var kind in Enum.GetValues<RawAnnotationKind>()
                     .Where(static kind => kind != RawAnnotationKind.None))
        {
            var existing =
                existingAnnotations.VisualRanges.GetValueOrDefault(kind) ?? [];
            IReadOnlyList<HighlightSourceRange> codeRanges;
            IReadOnlyList<DiagnosticIdentity> codeIdentities;
            if (plannedSelections.TryGetValue(kind, out var selection))
            {
                codeRanges = GetCodeRanges(current.Code, selection);
                if (existing.Count != 0 && !existing.SequenceEqual(codeRanges))
                {
                    throw Invalid(
                        path,
                        current,
                        $"has conflicting existing {KindName(kind)} markup.");
                }

                codeIdentities = selection.Diagnostics is null
                    ? []
                    : DiagnosticIdentityMetadata.Parse(
                        current.Code,
                        selection.Diagnostics,
                        $"{KindName(kind)} diagnostics");
                var existingIdentities =
                    existingAnnotations.DiagnosticIdentities.GetValueOrDefault(kind) ?? [];
                if (existingIdentities.Count != 0
                    && !SameIdentities(existingIdentities, codeIdentities))
                {
                    throw Invalid(
                        path,
                        current,
                        $"has conflicting existing {KindName(kind)} titles.");
                }
            }
            else
            {
                codeRanges = existing;
                codeIdentities =
                    existingAnnotations.DiagnosticIdentities.GetValueOrDefault(kind) ?? [];
            }

            foreach (var range in codeRanges)
            {
                var mapped = MapRawRange(range, kind);
                rawRanges.Add(new TypedSourceRange(kind, mapped.Start, mapped.End));
            }

            foreach (var identity in codeIdentities)
            {
                var mapped = MapRawRange(
                    new HighlightSourceRange(identity.Start, identity.End),
                    kind);
                rawIdentities.Add(
                    new TypedDiagnosticSourceRange(
                        kind,
                        identity.Id,
                        mapped.Start,
                        mapped.End,
                        identity.Order));
            }
        }

        var desiredBody = ApplyAnnotations(
            cleanBody,
            rawRanges,
            rawIdentities);

        if (!string.Equals(originalBody, desiredBody, StringComparison.Ordinal))
        {
            replacements.Add(
                new DocumentSourceReplacement(
                    bodyStart,
                    originalBody.Length,
                    desiredBody));
        }

        return replacements;

        void AddPlannedSelection(
            RawAnnotationKind kind,
            SelectionMetadataPlan? selection)
        {
            if (selection is not null)
            {
                plannedSelections.Add(kind, selection);
            }
        }

        HighlightSourceRange MapRawRange(
            HighlightSourceRange range,
            RawAnnotationKind kind)
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
                    $"has no unique exact raw source range for {KindName(kind)}.");
            }

            return candidates.Single();
        }
    }

    private static RawExistingAnnotations GetExistingAnnotations(
            string path,
            CurrentCodeBlock current,
            string body)
    {
        var annotationBody = RewriteExistingAnnotationTags(
            path,
            current,
            body,
            marksAsEmphasis: true);
        HistoricalCodeBlock block;
        try
        {
            block = AssertSingle(
                LegacyPreParser.Parse(
                    $"<pre><code>{annotationBody}</code></pre>"));
        }
        catch (InvalidDataException exception)
        {
            throw Invalid(
                path,
                current,
                $"contains malformed existing annotation markup: {exception.Message}");
        }

        if (!string.Equals(block.Code, current.Code, StringComparison.Ordinal))
        {
            throw Invalid(
                path,
                current,
                "existing annotations do not map to the discovered visible code.");
        }

        var visual = block.Annotations
            .GroupBy(static annotation => ToRawKind(annotation.Kind))
            .ToDictionary(
                static group => group.Key,
                static group => (IReadOnlyList<HighlightSourceRange>)Merge(
                    group.Select(static annotation => new HighlightSourceRange(
                        annotation.Start,
                        annotation.Start + annotation.Length))));
        var identities = block.Annotations
            .Where(static annotation => annotation.DiagnosticId is not null)
            .GroupBy(static annotation => ToRawKind(annotation.Kind))
            .ToDictionary(
                static group => group.Key,
                static group => (IReadOnlyList<DiagnosticIdentity>)group
                    .OrderBy(static annotation => annotation.Order)
                    .Select((annotation, order) => new DiagnosticIdentity(
                        annotation.DiagnosticId!,
                        annotation.Start,
                        annotation.Start + annotation.Length,
                        order))
                    .ToArray());
        return new RawExistingAnnotations(visual, identities);

        static HistoricalCodeBlock AssertSingle(
            IReadOnlyList<HistoricalCodeBlock> blocks) =>
            blocks.Count == 1
                ? blocks[0]
                : throw new InvalidDataException(
                    "Existing raw annotations do not form one code block.");
    }

    private static RawAnnotationKind ToRawKind(AnnotationKind kind) =>
        kind switch
        {
            AnnotationKind.Highlight => RawAnnotationKind.Highlight,
            AnnotationKind.Error => RawAnnotationKind.Error,
            AnnotationKind.Warning => RawAnnotationKind.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

    private static bool SameIdentities(
        IReadOnlyList<DiagnosticIdentity> left,
        IReadOnlyList<DiagnosticIdentity> right) =>
        left.Count == right.Count
        && left.Zip(right).All(pair =>
            pair.First.Id == pair.Second.Id
            && pair.First.Start == pair.Second.Start
            && pair.First.End == pair.Second.End);

    private static string RemoveExistingAnnotations(
        string path,
        CurrentCodeBlock current,
        string body) =>
        RewriteExistingAnnotationTags(
            path,
            current,
            body,
            marksAsEmphasis: false);

    private static string RewriteExistingAnnotationTags(
        string path,
        CurrentCodeBlock current,
        string body,
        bool marksAsEmphasis)
    {
        var output = new StringBuilder(body.Length);
        var open = new Stack<RawWrapper>();
        for (var index = 0; index < body.Length;)
        {
            if (body[index] != '<')
            {
                output.Append(body[index++]);
                continue;
            }

            if (body.AsSpan(index).StartsWith("<!--", StringComparison.Ordinal))
            {
                var commentEnd = body.IndexOf("-->", index + 4, StringComparison.Ordinal);
                if (commentEnd < 0)
                {
                    throw Invalid(path, current, "contains an unclosed HTML comment.");
                }

                var end = commentEnd + 3;
                output.Append(body, index, end - index);
                index = end;
                continue;
            }

            var tagEnd = FindTagEnd(body, index);
            var tag = body[index..(tagEnd + 1)];
            if (TryParseDiagnosticSpanOpening(
                    tag,
                    out var diagnosticKind,
                    out var diagnosticId))
            {
                Open(diagnosticKind, diagnosticId);
                if (marksAsEmphasis)
                {
                    output.Append(tag);
                }

                index = tagEnd + 1;
                continue;
            }

            switch (tag)
            {
                case MarkOpening:
                    Open(RawAnnotationKind.Highlight, null);
                    if (marksAsEmphasis)
                    {
                        output.Append("<em>");
                    }
                    break;
                case MarkClosing:
                    Close(RawAnnotationKind.Highlight);
                    if (marksAsEmphasis)
                    {
                        output.Append("</em>");
                    }
                    break;
                case SpanClosing:
                    if (open.Count == 0
                        || open.Peek().Kind is not (
                            RawAnnotationKind.Error or RawAnnotationKind.Warning))
                    {
                        throw Invalid(
                            path,
                            current,
                            "contains an unmatched annotation </span>.");
                    }

                    open.Pop();
                    if (marksAsEmphasis)
                    {
                        output.Append(tag);
                    }
                    break;
                default:
                    if (Regex.IsMatch(
                            tag,
                            """^</?(?:mark|span)(?:\s|>)""",
                            RegexOptions.IgnoreCase))
                    {
                        throw Invalid(
                            path,
                            current,
                            "contains unsupported or noncanonical annotation markup.");
                    }

                    output.Append(tag);
                    break;
            }

            index = tagEnd + 1;
        }

        if (open.Count != 0)
        {
            throw Invalid(path, current, "contains unclosed annotation markup.");
        }

        return output.ToString();

        void Open(RawAnnotationKind kind, string? diagnosticId)
        {
            var sameKindNesting = open.Count != 0
                && open.Peek().Kind == kind
                && diagnosticId is not null
                && open.Peek().DiagnosticId is not null;
            if (open.Any(wrapper => wrapper.Kind == kind) && !sameKindNesting
                || open.Count != 0
                && Rank(kind) < Rank(open.Peek().Kind))
            {
                throw Invalid(
                    path,
                    current,
                    "contains noncanonical annotation nesting.");
            }

            open.Push(new RawWrapper(kind, diagnosticId, open.Count));
        }

        void Close(RawAnnotationKind kind)
        {
            if (open.Count == 0 || open.Pop().Kind != kind)
            {
                throw Invalid(
                    path,
                    current,
                    "contains crossing or unmatched annotation markup.");
            }
        }

        static int Rank(RawAnnotationKind kind) =>
            kind switch
            {
                RawAnnotationKind.Highlight => 1,
                RawAnnotationKind.Error => 2,
                RawAnnotationKind.Warning => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
            };
    }

    private static bool TryParseDiagnosticSpanOpening(
        string tag,
        out RawAnnotationKind kind,
        out string? diagnosticId)
    {
        var match = Regex.Match(
            tag,
            """^<span class="(?<kind>error|warning)"(?: title="(?<id>(?:CS|CA)\d{4})")?>$""",
            RegexOptions.CultureInvariant);
        if (!match.Success)
        {
            kind = RawAnnotationKind.None;
            diagnosticId = null;
            return false;
        }

        kind = match.Groups["kind"].Value == "error"
            ? RawAnnotationKind.Error
            : RawAnnotationKind.Warning;
        diagnosticId = match.Groups["id"].Success
            ? match.Groups["id"].Value
            : null;
        return true;
    }

    private static IReadOnlyList<HighlightSourceRange> GetCodeRanges(
        string code,
        SelectionMetadataPlan selection)
    {
        var ranges = new List<HighlightSourceRange>();
        if (selection.Lines is not null)
        {
            ranges.AddRange(ParseLineRanges(code, selection.Lines));
        }

        if (selection.Text is not null)
        {
            var start = code.IndexOf(selection.Text, StringComparison.Ordinal);
            if (start < 0
                || code.IndexOf(
                    selection.Text,
                    start + 1,
                    StringComparison.Ordinal) >= 0)
            {
                throw new InvalidDataException(
                    "A raw-table annotation text value must occur exactly once.");
            }

            ranges.Add(new HighlightSourceRange(start, start + selection.Text.Length));
        }

        if (selection.Ranges is not null)
        {
            ranges.AddRange(HighlightRangePlanner.Parse(code, selection.Ranges));
        }

        return Merge(ranges);
    }

    private static string ApplyAnnotations(
        string source,
        IReadOnlyList<TypedSourceRange> ranges,
        IReadOnlyList<TypedDiagnosticSourceRange> identities)
    {
        var boundaries = new SortedSet<int> { 0, source.Length };
        foreach (var range in ranges)
        {
            boundaries.Add(range.Start);
            boundaries.Add(range.End);
        }
        foreach (var identity in identities)
        {
            boundaries.Add(identity.Start);
            boundaries.Add(identity.End);
        }

        var positions = boundaries.ToArray();
        var output = new StringBuilder(
            source.Length
            + (ranges.Count + identities.Count)
            * (ErrorOpening.Length + SpanClosing.Length));
        var active = new List<RawWrapper>();
        for (var index = 0; index < positions.Length - 1; index++)
        {
            var start = positions[index];
            var end = positions[index + 1];
            var next = GetActiveWrappers(start, end);
            var common = 0;
            while (common < active.Count
                   && common < next.Count
                   && active[common] == next[common])
            {
                common++;
            }

            for (var close = active.Count - 1; close >= common; close--)
            {
                output.Append(Closing(active[close]));
            }

            active.RemoveRange(common, active.Count - common);
            foreach (var wrapper in next.Skip(common))
            {
                output.Append(Opening(wrapper));
                active.Add(wrapper);
            }
            output.Append(source, start, end - start);
        }

        for (var close = active.Count - 1; close >= 0; close--)
        {
            output.Append(Closing(active[close]));
        }

        return output.ToString();

        IReadOnlyList<RawWrapper> GetActiveWrappers(int start, int end)
        {
            var wrappers = new List<RawWrapper>();
            foreach (var kind in new[]
                     {
                         RawAnnotationKind.Highlight,
                         RawAnnotationKind.Error,
                         RawAnnotationKind.Warning,
                     })
            {
                var visual = ranges.Any(range =>
                    range.Kind == kind
                    && range.Start <= start
                    && end <= range.End);
                var activeIdentities = identities
                    .Where(identity =>
                        identity.Kind == kind
                        && identity.Start <= start
                        && end <= identity.End)
                    .OrderBy(static identity => identity.Order)
                    .ToArray();
                if (activeIdentities.Length != 0)
                {
                    wrappers.AddRange(
                        activeIdentities.Select(identity => new RawWrapper(
                            kind,
                            identity.DiagnosticId,
                            identity.Order)));
                }
                else if (visual)
                {
                    wrappers.Add(new RawWrapper(kind, null, -1));
                }
            }

            return wrappers;
        }

        static string Opening(RawWrapper wrapper) =>
            wrapper.Kind switch
            {
                RawAnnotationKind.Highlight => MarkOpening,
                RawAnnotationKind.Error or RawAnnotationKind.Warning =>
                    $"<span class=\"{(wrapper.Kind == RawAnnotationKind.Error ? "error" : "warning")}\""
                    + (wrapper.DiagnosticId is null
                        ? ">"
                        : $" title=\"{wrapper.DiagnosticId}\">"),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(wrapper),
                    wrapper,
                    null),
            };

        static string Closing(RawWrapper wrapper) =>
            wrapper.Kind switch
            {
                RawAnnotationKind.Highlight => MarkClosing,
                RawAnnotationKind.Error or RawAnnotationKind.Warning => SpanClosing,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(wrapper),
                    wrapper,
                    null),
            };
    }

    private static string KindName(RawAnnotationKind kind) =>
        kind switch
        {
            RawAnnotationKind.Highlight => "a highlight",
            RawAnnotationKind.Error => "an error annotation",
            RawAnnotationKind.Warning => "a warning annotation",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

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

    [Flags]
    private enum RawAnnotationKind
    {
        None = 0,
        Highlight = 1,
        Error = 2,
        Warning = 4,
    }

    private readonly record struct TypedSourceRange(
        RawAnnotationKind Kind,
        int Start,
        int End);

    private readonly record struct TypedDiagnosticSourceRange(
        RawAnnotationKind Kind,
        string DiagnosticId,
        int Start,
        int End,
        int Order);

    private readonly record struct RawWrapper(
        RawAnnotationKind Kind,
        string? DiagnosticId,
        int Order);

    private sealed record RawExistingAnnotations(
        IReadOnlyDictionary<RawAnnotationKind, IReadOnlyList<HighlightSourceRange>>
            VisualRanges,
        IReadOnlyDictionary<RawAnnotationKind, IReadOnlyList<DiagnosticIdentity>>
            DiagnosticIdentities);

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
