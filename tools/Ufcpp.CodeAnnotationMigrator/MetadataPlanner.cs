using System.Net;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record SelectionMetadataPlan(
    string? Lines,
    string? Text,
    string? Ranges = null);

internal sealed record BlockMetadataPlan(
    string? Title,
    SelectionMetadataPlan? Highlight,
    SelectionMetadataPlan? Error,
    SelectionMetadataPlan? Warning);

internal sealed record MetadataPlanningDiagnostic(
    string Code,
    AnnotationKind? Kind,
    string Message);

internal sealed record MetadataPlanningResult(
    BlockMetadataPlan Plan,
    IReadOnlyList<MetadataPlanningDiagnostic> Diagnostics);

internal static class MetadataPlanner
{
    public static MetadataPlanningResult Plan(
        HistoricalCodeBlock historical,
        CurrentCodeBlock current)
    {
        ArgumentNullException.ThrowIfNull(historical);
        ArgumentNullException.ThrowIfNull(current);

        var diagnostics = new List<MetadataPlanningDiagnostic>();
        var title = historical.Title;
        if (title?.Any(char.IsControl) == true)
        {
            diagnostics.Add(new MetadataPlanningDiagnostic(
                "UNREPRESENTABLE_TITLE",
                null,
                "The title contains a control character and cannot be emitted safely."));
            title = null;
        }

        var historicalLayout = CodeLayout.Create(historical.Code);
        var currentLayout = CodeLayout.Create(current.Code);
        if (!historicalLayout.Normalized.Equals(
                currentLayout.Normalized,
                StringComparison.Ordinal))
        {
            diagnostics.Add(new MetadataPlanningDiagnostic(
                "MATCHED_CODE_CHANGED",
                null,
                "Metadata planning requires equal normalized historical and current code."));
            return new MetadataPlanningResult(
                new BlockMetadataPlan(title, null, null, null),
                diagnostics);
        }

        if (historicalLayout.NormalizedLineCount != currentLayout.NormalizedLineCount)
        {
            var affectedKinds = historical.Annotations
                .Select(static annotation => (AnnotationKind?)annotation.Kind)
                .Distinct()
                .Order()
                .ToArray();
            if (affectedKinds.Length == 0)
            {
                affectedKinds = [null];
            }

            foreach (var kind in affectedKinds)
            {
                diagnostics.Add(new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_SOURCE_LAYOUT",
                    kind,
                    "Entity decoding changes the physical line layout between "
                    + "the historical and current source blocks."));
            }

            return new MetadataPlanningResult(
                new BlockMetadataPlan(title, null, null, null),
                diagnostics);
        }

        var highlight = PlanSelections(
            AnnotationKind.Highlight,
            historical.Annotations,
            historicalLayout,
            currentLayout,
            diagnostics);
        var error = PlanSelections(
            AnnotationKind.Error,
            historical.Annotations,
            historicalLayout,
            currentLayout,
            diagnostics);
        var warning = PlanSelections(
            AnnotationKind.Warning,
            historical.Annotations,
            historicalLayout,
            currentLayout,
            diagnostics);

        return new MetadataPlanningResult(
            new BlockMetadataPlan(title, highlight, error, warning),
            diagnostics);
    }

    private static SelectionMetadataPlan? PlanSelections(
        AnnotationKind kind,
        IReadOnlyList<AnnotationSelection> annotations,
        CodeLayout historical,
        CodeLayout current,
        ICollection<MetadataPlanningDiagnostic> diagnostics)
    {
        var selections = annotations
            .Where(item => item.Kind == kind && item.Length > 0)
            .ToArray();
        var emptySelectionCount = annotations.Count(
            item => item.Kind == kind && item.Length == 0);
        if (emptySelectionCount != 0)
        {
            diagnostics.Add(new MetadataPlanningDiagnostic(
                "EMPTY_ANNOTATION_SELECTION",
                kind,
                $"The {kind.ToString().ToLowerInvariant()} metadata contains "
                + $"{emptySelectionCount} empty selection"
                + (emptySelectionCount == 1 ? "." : "s.")));
        }

        if (selections.Length == 0)
        {
            return null;
        }

        var lines = new SortedSet<int>();
        var partial = new List<AnnotationSelection>();
        foreach (var selection in selections)
        {
            if (TryMapWholeLines(selection, historical, current, lines))
            {
                continue;
            }

            partial.Add(selection);
        }

        string? text = null;
        MetadataPlanningDiagnostic? textDiagnostic = null;
        if (partial.Count == 1)
        {
            text = NormalizeNewlines(partial[0].Text);
            if (text.Length == 0)
            {
                textDiagnostic = new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_EMPTY_TEXT",
                    kind,
                    $"The {kind.ToString().ToLowerInvariant()} selection is empty.");
            }
            else if (text.Contains('\n', StringComparison.Ordinal))
            {
                textDiagnostic = new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_MULTILINE_TEXT",
                    kind,
                    $"The {kind.ToString().ToLowerInvariant()} partial selection "
                    + "crosses a line boundary.");
            }
            else
            {
                var occurrenceCount = CountOccurrences(current.Canonical, text);
                if (occurrenceCount == 0)
                {
                    textDiagnostic = new MetadataPlanningDiagnostic(
                        "UNREPRESENTABLE_MISSING_TEXT",
                        kind,
                        $"The {kind.ToString().ToLowerInvariant()} selected text "
                        + "does not occur in the current block.");
                }
                else if (occurrenceCount > 1)
                {
                    textDiagnostic = new MetadataPlanningDiagnostic(
                        "UNREPRESENTABLE_REPEATED_TEXT",
                        kind,
                        $"The {kind.ToString().ToLowerInvariant()} selected text "
                        + $"occurs {occurrenceCount} times in the current block.");
                }
                else
                {
                    var currentOffset = current.Canonical.IndexOf(
                        text,
                        StringComparison.Ordinal);
                    if (!MapsToSameSemanticOccurrence(
                            partial[0],
                            historical,
                            current,
                            text,
                            currentOffset))
                    {
                        textDiagnostic = new MetadataPlanningDiagnostic(
                            "UNREPRESENTABLE_POSITIONAL_TEXT",
                            kind,
                            $"The {kind.ToString().ToLowerInvariant()} selected text "
                            + "maps to a different occurrence after entity normalization.");
                    }
                }
            }

            if (textDiagnostic is not null)
            {
                text = null;
            }
        }

        if (partial.Count != 0 && text is null)
        {
            var rangePlan = HighlightRangePlanner.Plan(
                partial,
                historical.Original,
                current.Original);
            if (rangePlan.Value is not null)
            {
                return new SelectionMetadataPlan(
                    lines.Count == 0 ? null : FormatLineRanges(lines),
                    null,
                    rangePlan.Value);
            }

            diagnostics.Add(new MetadataPlanningDiagnostic(
                "UNREPRESENTABLE_RANGE_PROJECTION",
                kind,
                rangePlan.Error ?? "The highlight cannot be projected exactly."));
            return null;
        }

        return new SelectionMetadataPlan(
            lines.Count == 0 ? null : FormatLineRanges(lines),
            text);
    }

    private static bool MapsToSameSemanticOccurrence(
        AnnotationSelection selection,
        CodeLayout historical,
        CodeLayout current,
        string rawText,
        int currentRawOffset)
    {
        if (currentRawOffset < 0
            || selection.Start < 0
            || selection.Start > historical.Original.Length)
        {
            return false;
        }

        var historicalCanonicalOffset = NormalizeNewlines(
            historical.Original[..selection.Start]).Length;
        if (historicalCanonicalOffset > historical.Canonical.Length)
        {
            return false;
        }

        var semanticText = WebUtility.HtmlDecode(rawText);
        var historicalSemantic = WebUtility.HtmlDecode(historical.Canonical);
        var currentSemantic = WebUtility.HtmlDecode(current.Canonical);
        var historicalSemanticOffset = WebUtility.HtmlDecode(
            historical.Canonical[..historicalCanonicalOffset]).Length;
        var currentSemanticOffset = WebUtility.HtmlDecode(
            current.Canonical[..currentRawOffset]).Length;
        return HasTextAt(historicalSemantic, semanticText, historicalSemanticOffset)
            && HasTextAt(currentSemantic, semanticText, currentSemanticOffset)
            && GetOccurrenceOrdinal(
                historicalSemantic,
                semanticText,
                historicalSemanticOffset)
            == GetOccurrenceOrdinal(
                currentSemantic,
                semanticText,
                currentSemanticOffset);
    }

    private static bool HasTextAt(string value, string text, int offset) =>
        offset >= 0
        && offset + text.Length <= value.Length
        && value.AsSpan(offset, text.Length).SequenceEqual(text);

    private static int GetOccurrenceOrdinal(
        string value,
        string text,
        int selectedOffset)
    {
        var ordinal = 0;
        for (var offset = 0; offset <= selectedOffset;)
        {
            var found = value.IndexOf(text, offset, StringComparison.Ordinal);
            if (found < 0 || found > selectedOffset)
            {
                break;
            }

            ordinal++;
            offset = found + 1;
        }

        return ordinal;
    }

    private static bool TryMapWholeLines(
        AnnotationSelection selection,
        CodeLayout historical,
        CodeLayout current,
        ISet<int> targetLines)
    {
        if (selection.Start < 0
            || selection.Length < 0
            || selection.Start + selection.Length > historical.Original.Length)
        {
            return false;
        }

        var selectionEnd = selection.Start + selection.Length;
        var startLine = historical.Lines.FindIndex(
            line => line.Start == selection.Start);
        var endLine = historical.Lines.FindIndex(
            line => line.End == selectionEnd);
        if (startLine < 0 || endLine < startLine)
        {
            return false;
        }

        var normalizedStart = startLine - historical.LeadingBlankLines;
        var normalizedEnd = endLine - historical.LeadingBlankLines;
        if (normalizedStart < 0
            || normalizedEnd >= historical.NormalizedLineCount
            || normalizedEnd >= current.NormalizedLineCount)
        {
            return false;
        }

        for (var line = normalizedStart; line <= normalizedEnd; line++)
        {
            targetLines.Add(current.LeadingBlankLines + line + 1);
        }

        return true;
    }

    private static int CountOccurrences(string value, string text)
    {
        var count = 0;
        for (var index = 0; index <= value.Length - text.Length;)
        {
            var found = value.IndexOf(text, index, StringComparison.Ordinal);
            if (found < 0)
            {
                break;
            }

            count++;
            index = found + 1;
        }

        return count;
    }

    private static string FormatLineRanges(IEnumerable<int> lines)
    {
        var values = lines.ToArray();
        var ranges = new List<string>();
        for (var index = 0; index < values.Length;)
        {
            var start = values[index];
            var end = start;
            while (index + 1 < values.Length && values[index + 1] == end + 1)
            {
                end = values[++index];
            }

            ranges.Add(start == end ? start.ToString() : $"{start}-{end}");
            index++;
        }

        return string.Join(',', ranges);
    }

    private static string NormalizeNewlines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');

    private sealed record CodeLayout(
        string Original,
        string Canonical,
        string Normalized,
        List<LineRange> Lines,
        int LeadingBlankLines,
        int NormalizedLineCount)
    {
        public static CodeLayout Create(string code)
        {
            var canonical = NormalizeNewlines(code);
            var lines = GetLines(code);
            var canonicalLines = canonical
                .Split('\n')
                .Select(static line => line.TrimEnd(' ', '\t'))
                .ToArray();
            var leading = canonicalLines
                .TakeWhile(static line => string.IsNullOrWhiteSpace(line))
                .Count();
            var trailing = canonicalLines
                .Reverse()
                .TakeWhile(static line => string.IsNullOrWhiteSpace(line))
                .Count();
            var normalizedLineCount = Math.Max(
                0,
                canonicalLines.Length - leading - trailing);
            return new CodeLayout(
                code,
                canonical,
                CodeNormalizer.Normalize(code),
                lines,
                leading,
                normalizedLineCount);
        }

        private static List<LineRange> GetLines(string value)
        {
            var lines = new List<LineRange>();
            var lineStart = 0;
            for (var index = 0; index < value.Length;)
            {
                if (value[index] is not '\r' and not '\n')
                {
                    index++;
                    continue;
                }

                lines.Add(new LineRange(lineStart, index));
                if (value[index] == '\r'
                    && index + 1 < value.Length
                    && value[index + 1] == '\n')
                {
                    index++;
                }

                index++;
                lineStart = index;
            }

            lines.Add(new LineRange(lineStart, value.Length));
            return lines;
        }
    }

    private sealed record LineRange(int Start, int End);
}
