using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record Issue5MigrationReport(
    int SchemaVersion,
    string Mode,
    ReportSource Source,
    ReportTarget Target,
    Issue5AcceptanceCounts Acceptance,
    Issue5RepresentationCounts Representations,
    Issue5OverlapCounts Overlaps,
    IReadOnlyList<string> ChangedDocuments,
    IReadOnlyList<Issue5ExceptionResolution> ExceptionResolutions,
    IReadOnlyList<ReportPlan> Plans);

internal sealed record Issue5AcceptanceCounts(
    int BaselineErrorBlocks,
    int SupplementalMalformedErrorBlocks,
    int RestoredErrorBlocks,
    int BaselineWarningBlocks,
    int SupplementalMalformedWarningCandidates,
    int RestoredWarningBlocks,
    int ParsedErrorSelections,
    int SupplementalErrorSelections,
    int RestoredErrorSelections,
    int ParsedWarningSelections,
    int SupplementalWarningSelections,
    int RestoredWarningSelections,
    int ObsoleteSelections,
    int Blocked);

internal sealed record Issue5RepresentationCounts(
    int ErrorLineBlocks,
    int ErrorTextBlocks,
    int ErrorRangeBlocks,
    int ErrorRangeIntervals,
    int WarningLineBlocks,
    int WarningTextBlocks,
    int WarningRangeBlocks,
    int WarningRangeIntervals,
    int RawTableErrorBlocks,
    int RawTableWarningBlocks);

internal sealed record Issue5OverlapCounts(
    int ErrorHighlightSameBlock,
    int WarningHighlightSameBlock,
    int ErrorWarningSameBlock,
    int AllThreeSameBlock,
    int ErrorHighlightIntersections,
    int WarningHighlightIntersections,
    int ErrorWarningIntersections,
    int AllThreeIntersections);

internal sealed record Issue5ExceptionResolution(
    string Id,
    string Path,
    int? HistoricalOrdinal,
    string Disposition,
    AnnotationKind? Kind,
    int HistoricalSelections,
    int RestoredSelections,
    string Reason);

internal static class Issue5MigrationReportWriter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public static byte[] Serialize(
        MigrationAnalysisInput input,
        MigrationReport baseline,
        Issue5MigrationResult migration)
    {
        var parsedErrorSelections = CountParsedSelections(
            input,
            AnnotationKind.Error);
        var parsedWarningSelections = CountParsedSelections(
            input,
            AnnotationKind.Warning);
        var malformedError = migration.Exceptions.Count(
            static entry =>
                entry.DiagnosticCode == "MALFORMED_HISTORICAL_BLOCK"
                && entry.Kind == AnnotationKind.Error);
        var malformedWarning = migration.Exceptions.Count(
            static entry =>
                entry.DiagnosticCode == "MALFORMED_HISTORICAL_BLOCK"
                && entry.Kind == AnnotationKind.Warning);
        var supplementalErrorSelections = migration.Exceptions
            .Where(static entry =>
                entry.DiagnosticCode == "MALFORMED_HISTORICAL_BLOCK"
                && entry.Kind == AnnotationKind.Error)
            .Sum(static entry => entry.HistoricalSelections);
        var supplementalWarningSelections = migration.Exceptions
            .Where(static entry =>
                entry.DiagnosticCode == "MALFORMED_HISTORICAL_BLOCK"
                && entry.Kind == AnnotationKind.Warning)
            .Sum(static entry => entry.HistoricalSelections);
        var parsedObsoleteErrorSelections = migration.Exceptions
            .Where(static entry =>
                entry.DiagnosticCode != "MALFORMED_HISTORICAL_BLOCK"
                && entry.Disposition == Issue5ExceptionDisposition.Obsolete
                && entry.Kind == AnnotationKind.Error)
            .Sum(static entry => entry.HistoricalSelections);
        var parsedObsoleteWarningSelections = migration.Exceptions
            .Where(static entry =>
                entry.DiagnosticCode != "MALFORMED_HISTORICAL_BLOCK"
                && entry.Disposition == Issue5ExceptionDisposition.Obsolete
                && entry.Kind == AnnotationKind.Warning)
            .Sum(static entry => entry.HistoricalSelections);
        var restoredSupplementalErrorSelections = migration.Exceptions
            .Where(static entry =>
                entry.DiagnosticCode == "MALFORMED_HISTORICAL_BLOCK"
                && entry.Kind == AnnotationKind.Error)
            .Sum(static entry => entry.RestoredSelections);
        var restoredSupplementalWarningSelections = migration.Exceptions
            .Where(static entry =>
                entry.DiagnosticCode == "MALFORMED_HISTORICAL_BLOCK"
                && entry.Kind == AnnotationKind.Warning)
            .Sum(static entry => entry.RestoredSelections);
        var obsoleteSelections = migration.Exceptions
            .Where(static entry =>
                entry.Disposition == Issue5ExceptionDisposition.Obsolete)
            .Sum(static entry => entry.HistoricalSelections);
        var overlaps = CountOverlaps(input, baseline, migration);
        var report = new Issue5MigrationReport(
            3,
            "issue5-plan",
            baseline.Source,
            baseline.Target,
            new Issue5AcceptanceCounts(
                baseline.Coverage.Error.Total,
                malformedError,
                migration.Plans.Count(static plan => plan.Metadata.Error is not null),
                baseline.Coverage.Warning.Total,
                malformedWarning,
                migration.Plans.Count(static plan => plan.Metadata.Warning is not null),
                parsedErrorSelections,
                supplementalErrorSelections,
                parsedErrorSelections
                    - parsedObsoleteErrorSelections
                    + restoredSupplementalErrorSelections,
                parsedWarningSelections,
                supplementalWarningSelections,
                parsedWarningSelections
                    - parsedObsoleteWarningSelections
                    + restoredSupplementalWarningSelections,
                obsoleteSelections,
                0),
            new Issue5RepresentationCounts(
                CountBlocks(migration.Plans, AnnotationKind.Error, Form.Lines),
                CountBlocks(migration.Plans, AnnotationKind.Error, Form.Text),
                CountBlocks(migration.Plans, AnnotationKind.Error, Form.Ranges),
                CountRangeIntervals(migration.Plans, AnnotationKind.Error),
                CountBlocks(migration.Plans, AnnotationKind.Warning, Form.Lines),
                CountBlocks(migration.Plans, AnnotationKind.Warning, Form.Text),
                CountBlocks(migration.Plans, AnnotationKind.Warning, Form.Ranges),
                CountRangeIntervals(migration.Plans, AnnotationKind.Warning),
                migration.Plans.Count(static plan =>
                    plan.TargetKind == "rawPreInTable"
                    && plan.Metadata.Error is not null),
                migration.Plans.Count(static plan =>
                    plan.TargetKind == "rawPreInTable"
                    && plan.Metadata.Warning is not null)),
            overlaps,
            migration.ChangedDocuments.Keys.Order(StringComparer.Ordinal).ToArray(),
            migration.Exceptions
                .OrderBy(static entry => entry.Id, StringComparer.Ordinal)
                .Select(static entry => new Issue5ExceptionResolution(
                    entry.Id,
                    entry.Path,
                    entry.HistoricalOrdinal,
                    DispositionName(entry.Disposition),
                    entry.Kind,
                    entry.HistoricalSelections,
                    entry.RestoredSelections,
                    entry.Reason))
                .ToArray(),
            migration.Plans);
        return Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(report, Options).ReplaceLineEndings("\n") + "\n");
    }

    private static int CountParsedSelections(
        MigrationAnalysisInput input,
        AnnotationKind kind) =>
        input.HistoricalDocuments.Values
            .Select(LegacyPreParser.ParseDetailed)
            .SelectMany(static result => result.Blocks)
            .SelectMany(static block => block.Annotations)
            .Count(annotation => annotation.Kind == kind);

    private static int CountBlocks(
        IEnumerable<ReportPlan> plans,
        AnnotationKind kind,
        Form form) =>
        plans.Count(plan => GetSelection(plan.Metadata, kind) is { } selection
            && GetForm(selection, form) is not null);

    private static int CountRangeIntervals(
        IEnumerable<ReportPlan> plans,
        AnnotationKind kind) =>
        plans
            .Select(plan => GetSelection(plan.Metadata, kind)?.Ranges)
            .OfType<string>()
            .Sum(static ranges =>
                ranges[(ranges.IndexOf(';', StringComparison.Ordinal) + 1)..]
                    .Split(',', StringSplitOptions.None)
                    .Length);

    private static Issue5OverlapCounts CountOverlaps(
        MigrationAnalysisInput input,
        MigrationReport baseline,
        Issue5MigrationResult migration)
    {
        var baselinePlans = baseline.Plans.ToDictionary(
            static plan => (plan.Path, plan.CurrentOrdinal));
        var sameErrorHighlight = 0;
        var sameWarningHighlight = 0;
        var sameErrorWarning = 0;
        var sameAll = 0;
        var intersectErrorHighlight = 0;
        var intersectWarningHighlight = 0;
        var intersectErrorWarning = 0;
        var intersectAll = 0;
        foreach (var plan in migration.Plans)
        {
            var block = CurrentBlockDiscoverer.Discover(
                input.CurrentDocuments[plan.Path])[plan.CurrentOrdinal - 1];
            baselinePlans.TryGetValue(
                (plan.Path, plan.CurrentOrdinal),
                out var baselinePlan);
            var highlight = GetSpans(
                block.Code,
                baselinePlan?.Metadata.Highlight);
            var error = GetSpans(block.Code, plan.Metadata.Error);
            var warning = GetSpans(block.Code, plan.Metadata.Warning);
            var hasHighlight = highlight.Count != 0;
            var hasError = error.Count != 0;
            var hasWarning = warning.Count != 0;
            sameErrorHighlight += hasError && hasHighlight ? 1 : 0;
            sameWarningHighlight += hasWarning && hasHighlight ? 1 : 0;
            sameErrorWarning += hasError && hasWarning ? 1 : 0;
            sameAll += hasError && hasWarning && hasHighlight ? 1 : 0;
            intersectErrorHighlight += Intersects(error, highlight) ? 1 : 0;
            intersectWarningHighlight += Intersects(warning, highlight) ? 1 : 0;
            intersectErrorWarning += Intersects(error, warning) ? 1 : 0;
            intersectAll += Intersects(error, warning, highlight) ? 1 : 0;
        }

        return new Issue5OverlapCounts(
            sameErrorHighlight,
            sameWarningHighlight,
            sameErrorWarning,
            sameAll,
            intersectErrorHighlight,
            intersectWarningHighlight,
            intersectErrorWarning,
            intersectAll);
    }

    private static IReadOnlyList<HighlightSourceRange> GetSpans(
        string code,
        SelectionMetadataPlan? selection)
    {
        if (selection is null)
        {
            return [];
        }

        var spans = new List<HighlightSourceRange>();
        if (selection.Lines is not null)
        {
            var lines = GetLines(code);
            foreach (var item in selection.Lines.Split(',', StringSplitOptions.None))
            {
                var parts = item.Split('-', StringSplitOptions.None);
                var start = int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                var end = parts.Length == 1
                    ? start
                    : int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                spans.Add(
                    new HighlightSourceRange(
                        lines[start - 1].Start,
                        lines[end - 1].End));
            }
        }

        if (selection.Text is not null)
        {
            for (var offset = 0; offset <= code.Length - selection.Text.Length;)
            {
                var found = code.IndexOf(selection.Text, offset, StringComparison.Ordinal);
                if (found < 0)
                {
                    break;
                }

                spans.Add(
                    new HighlightSourceRange(found, found + selection.Text.Length));
                offset = found + 1;
            }
        }

        if (selection.Ranges is not null)
        {
            spans.AddRange(HighlightRangePlanner.Parse(code, selection.Ranges));
        }

        return Merge(spans);
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
        IEnumerable<HighlightSourceRange> spans)
    {
        var merged = new List<HighlightSourceRange>();
        foreach (var span in spans
                     .OrderBy(static span => span.Start)
                     .ThenBy(static span => span.End))
        {
            if (merged.Count != 0 && span.Start <= merged[^1].End)
            {
                merged[^1] = new HighlightSourceRange(
                    merged[^1].Start,
                    Math.Max(merged[^1].End, span.End));
            }
            else
            {
                merged.Add(span);
            }
        }

        return merged;
    }

    private static bool Intersects(
        IReadOnlyList<HighlightSourceRange> left,
        IReadOnlyList<HighlightSourceRange> right) =>
        left.Any(first => right.Any(second =>
            Math.Max(first.Start, second.Start) < Math.Min(first.End, second.End)));

    private static bool Intersects(
        IReadOnlyList<HighlightSourceRange> first,
        IReadOnlyList<HighlightSourceRange> second,
        IReadOnlyList<HighlightSourceRange> third) =>
        first.Any(left => second.Any(middle => third.Any(right =>
            Math.Max(left.Start, Math.Max(middle.Start, right.Start))
                < Math.Min(left.End, Math.Min(middle.End, right.End)))));

    private static SelectionMetadataPlan? GetSelection(
        BlockMetadataPlan metadata,
        AnnotationKind kind) =>
        kind switch
        {
            AnnotationKind.Highlight => metadata.Highlight,
            AnnotationKind.Error => metadata.Error,
            AnnotationKind.Warning => metadata.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

    private static string? GetForm(SelectionMetadataPlan selection, Form form) =>
        form switch
        {
            Form.Lines => selection.Lines,
            Form.Text => selection.Text,
            Form.Ranges => selection.Ranges,
            _ => throw new ArgumentOutOfRangeException(nameof(form), form, null),
        };

    private static string DispositionName(Issue5ExceptionDisposition disposition) =>
        disposition switch
        {
            Issue5ExceptionDisposition.MalformedResolved => "malformed-resolved",
            Issue5ExceptionDisposition.Obsolete => "obsolete",
            Issue5ExceptionDisposition.Unrelated => "unrelated",
            _ => throw new ArgumentOutOfRangeException(
                nameof(disposition),
                disposition,
                null),
        };

    private enum Form
    {
        Lines,
        Text,
        Ranges,
    }
}
