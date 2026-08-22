namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record SelectionMetadataPlan(string? Lines, string? Text);

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

        var overlappingKinds = FindOverlappingKinds(historical.Annotations);
        foreach (var kind in overlappingKinds)
        {
            diagnostics.Add(new MetadataPlanningDiagnostic(
                "UNREPRESENTABLE_OVERLAPPING_KINDS",
                kind,
                $"The {kind.ToString().ToLowerInvariant()} selection overlaps "
                + "a differently sized selection of another metadata kind."));
        }

        var highlight = overlappingKinds.Contains(AnnotationKind.Highlight)
            ? null
            : PlanSelections(
                AnnotationKind.Highlight,
                historical.Annotations,
                historicalLayout,
                currentLayout,
                diagnostics);
        var error = overlappingKinds.Contains(AnnotationKind.Error)
            ? null
            : PlanSelections(
                AnnotationKind.Error,
                historical.Annotations,
                historicalLayout,
                currentLayout,
                diagnostics);
        var warning = overlappingKinds.Contains(AnnotationKind.Warning)
            ? null
            : PlanSelections(
                AnnotationKind.Warning,
                historical.Annotations,
                historicalLayout,
                currentLayout,
                diagnostics);

        return new MetadataPlanningResult(
            new BlockMetadataPlan(title, highlight, error, warning),
            diagnostics);
    }

    private static IReadOnlySet<AnnotationKind> FindOverlappingKinds(
        IReadOnlyList<AnnotationSelection> annotations)
    {
        var kinds = new SortedSet<AnnotationKind>();
        for (var leftIndex = 0; leftIndex < annotations.Count; leftIndex++)
        {
            var left = annotations[leftIndex];
            var leftEnd = left.Start + left.Length;
            for (var rightIndex = leftIndex + 1;
                 rightIndex < annotations.Count;
                 rightIndex++)
            {
                var right = annotations[rightIndex];
                if (left.Kind == right.Kind
                    || left.Start == right.Start && left.Length == right.Length)
                {
                    continue;
                }

                var rightEnd = right.Start + right.Length;
                if (Math.Max(left.Start, right.Start) < Math.Min(leftEnd, rightEnd))
                {
                    kinds.Add(left.Kind);
                    kinds.Add(right.Kind);
                }
            }
        }

        return kinds;
    }

    private static SelectionMetadataPlan? PlanSelections(
        AnnotationKind kind,
        IReadOnlyList<AnnotationSelection> annotations,
        CodeLayout historical,
        CodeLayout current,
        ICollection<MetadataPlanningDiagnostic> diagnostics)
    {
        var selections = annotations.Where(item => item.Kind == kind).ToArray();
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

        if (partial.Count > 1)
        {
            diagnostics.Add(new MetadataPlanningDiagnostic(
                "UNREPRESENTABLE_MULTIPLE_TEXT",
                kind,
                $"The {kind.ToString().ToLowerInvariant()} metadata has "
                + $"{partial.Count} partial selections; the contract supports one."));
            return null;
        }

        string? text = null;
        if (partial.Count == 1)
        {
            text = NormalizeNewlines(partial[0].Text);
            if (text.Length == 0)
            {
                diagnostics.Add(new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_EMPTY_TEXT",
                    kind,
                    $"The {kind.ToString().ToLowerInvariant()} selection is empty."));
                return null;
            }

            if (text.Contains('\n', StringComparison.Ordinal))
            {
                diagnostics.Add(new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_MULTILINE_TEXT",
                    kind,
                    $"The {kind.ToString().ToLowerInvariant()} partial selection "
                    + "crosses a line boundary."));
                return null;
            }

            var occurrenceCount = CountOccurrences(current.Canonical, text);
            if (occurrenceCount == 0)
            {
                diagnostics.Add(new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_MISSING_TEXT",
                    kind,
                    $"The {kind.ToString().ToLowerInvariant()} selected text "
                    + "does not occur in the current block."));
                return null;
            }

            if (occurrenceCount > 1)
            {
                diagnostics.Add(new MetadataPlanningDiagnostic(
                    "UNREPRESENTABLE_REPEATED_TEXT",
                    kind,
                    $"The {kind.ToString().ToLowerInvariant()} selected text "
                    + $"occurs {occurrenceCount} times in the current block."));
                return null;
            }
        }

        return new SelectionMetadataPlan(
            lines.Count == 0 ? null : FormatLineRanges(lines),
            text);
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
