using System.Text;
using System.Text.Json;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record Issue4MigrationReport(
    int SchemaVersion,
    string Mode,
    ReportSource Source,
    ReportTarget Target,
    Issue4AcceptanceCounts Acceptance,
    Issue4RepresentationCounts Representations,
    IReadOnlyList<string> ChangedDocuments,
    IReadOnlyList<Issue4OverrideResolution> ExceptionResolutions,
    IReadOnlyList<ReportPlan> Plans);

internal sealed record Issue4AcceptanceCounts(
    int BaselineTitles,
    int SupplementalMalformedTitles,
    int RestoredTitles,
    int BaselineHighlights,
    int SupplementalMalformedHighlights,
    int RestoredHighlights,
    int Obsolete,
    int Blocked);

internal sealed record Issue4RepresentationCounts(
    int TitleBlocks,
    int HighlightBlocks,
    int HighlightLineBlocks,
    int HighlightTextBlocks,
    int HighlightRangeBlocks,
    int RawTableHighlightBlocks);

internal sealed record Issue4OverrideResolution(
    string Id,
    string Path,
    int HistoricalOrdinal,
    int CurrentOrdinal,
    string Disposition,
    string Reason);

internal static class Issue4MigrationReportWriter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static byte[] Serialize(
        MigrationReport baseline,
        Issue4MigrationResult migration)
    {
        var malformedTitles = migration.Overrides.Count(
            static entry => entry.HistoricalDocumentBlob is not null
                && entry.Title is not null);
        var malformedHighlights = migration.Overrides.Count(
            static entry => entry.HistoricalDocumentBlob is not null
                && entry.HighlightText is not null);
        var report = new Issue4MigrationReport(
            2,
            "issue4-plan",
            baseline.Source,
            baseline.Target,
            new Issue4AcceptanceCounts(
                baseline.Coverage.Title.Total,
                malformedTitles,
                migration.Plans.Count(static plan => plan.Metadata.Title is not null),
                baseline.Coverage.Highlight.Total,
                malformedHighlights,
                migration.Plans.Count(static plan => plan.Metadata.Highlight is not null),
                0,
                0),
            new Issue4RepresentationCounts(
                migration.Plans.Count(static plan => plan.Metadata.Title is not null),
                migration.Plans.Count(static plan => plan.Metadata.Highlight is not null),
                migration.Plans.Count(static plan =>
                    plan.Metadata.Highlight?.Lines is not null),
                migration.Plans.Count(static plan =>
                    plan.Metadata.Highlight?.Text is not null),
                migration.Plans.Count(static plan =>
                    plan.Metadata.Highlight?.Ranges is not null),
                migration.Plans.Count(static plan =>
                    plan.TargetKind == "rawPreInTable"
                    && plan.Metadata.Highlight is not null)),
            migration.ChangedDocuments.Keys.Order(StringComparer.Ordinal).ToArray(),
            migration.Overrides
                .OrderBy(static entry => entry.Id, StringComparer.Ordinal)
                .Select(entry => new Issue4OverrideResolution(
                    entry.Id,
                    entry.Path,
                    entry.HistoricalOrdinal,
                    entry.CurrentOrdinal,
                    entry.HistoricalDocumentBlob is null
                        ? "mapped"
                        : "malformed-resolved",
                    entry.Reason))
                .ToArray(),
            migration.Plans);
        return Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(report, Options).ReplaceLineEndings("\n") + "\n");
    }
}
