using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record MigrationAnalysisInput(
    string SourceCommit,
    string SourcePath,
    string TargetCommit,
    string TargetPath,
    IReadOnlyDictionary<string, string> HistoricalDocuments,
    IReadOnlyDictionary<string, string> CurrentDocuments);

internal sealed record MigrationAnalysisOutcome(
    MigrationReport Report,
    int ExitCode);

internal sealed record MigrationReport(
    int SchemaVersion,
    string Mode,
    ReportSource Source,
    ReportTarget Target,
    string NormalizationPolicy,
    string MatchingPolicy,
    ReportTotals Totals,
    CoverageReport Coverage,
    IReadOnlyList<ReportPlan> Plans,
    IReadOnlyList<ReportDiagnostic> Diagnostics);

internal sealed record ReportSource(string Commit, string Path);

internal sealed record ReportTarget(string Commit, string Path);

internal sealed record ReportTotals(
    int HistoricalDocuments,
    int CurrentDocuments,
    int HistoricalPreBlocks,
    int MalformedHistoricalCases,
    int CurrentFencedBlocks,
    int CurrentRawPreBlocks,
    int CurrentRawTableBlocks);

internal sealed record CoverageCounts(
    int Total,
    int Matched,
    int Ambiguous,
    int Unmatched,
    int Unrepresentable);

internal sealed record CoverageReport(
    CoverageCounts Title,
    CoverageCounts Highlight,
    CoverageCounts Error,
    CoverageCounts Warning,
    CoverageCounts FencedBlocks,
    CoverageCounts RawTableBlocks);

internal sealed record ReportPlan(
    string Path,
    int HistoricalOrdinal,
    int CurrentOrdinal,
    int HistoricalLine,
    int CurrentLine,
    string TargetKind,
    string MatchMethod,
    string Hash,
    BlockMetadataPlan Metadata);

internal sealed record ReportDiagnostic(
    string Code,
    string Severity,
    string Path,
    int? HistoricalOrdinal,
    int? CurrentOrdinal,
    int? HistoricalLine,
    int? CurrentLine,
    string? Hash,
    string? MetadataKind,
    string Message);

internal static class MigrationAnalyzer
{
    public static MigrationAnalysisOutcome Analyze(MigrationAnalysisInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var diagnostics = new List<ReportDiagnostic>();
        var plans = new List<ReportPlan>();
        var currentBlocksByPath = ParseCurrentDocuments(
            input.CurrentDocuments,
            diagnostics);
        var titleCoverage = new CoverageAccumulator();
        var highlightCoverage = new CoverageAccumulator();
        var errorCoverage = new CoverageAccumulator();
        var warningCoverage = new CoverageAccumulator();
        var fencedCoverage = new CoverageAccumulator();
        var tableCoverage = new CoverageAccumulator();
        var historicalBlockCount = 0;
        var malformedHistoricalCaseCount = 0;

        foreach (var (path, document) in input.HistoricalDocuments
                     .OrderBy(static item => item.Key, StringComparer.Ordinal))
        {
            HistoricalParseResult parsing;
            try
            {
                parsing = LegacyPreParser.ParseDetailed(document);
            }
            catch (InvalidDataException exception)
            {
                malformedHistoricalCaseCount++;
                diagnostics.Add(Diagnostic(
                    "MALFORMED_HISTORICAL_MARKUP",
                    "error",
                    path,
                    message: exception.Message));
                continue;
            }

            var historicalBlocks = parsing.Blocks;
            historicalBlockCount += parsing.PreBlockCount;
            malformedHistoricalCaseCount += parsing.Diagnostics.Count;
            foreach (var parseDiagnostic in parsing.Diagnostics)
            {
                if (parseDiagnostic.CountsAsBlock)
                {
                    (parseDiagnostic.IsInsideTable
                        ? tableCoverage
                        : fencedCoverage).Add(CoverageDisposition.Unrepresentable);
                }

                diagnostics.Add(Diagnostic(
                    parseDiagnostic.Code,
                    "error",
                    path,
                    historicalOrdinal: parseDiagnostic.Ordinal,
                    historicalLine: parseDiagnostic.SourceLine,
                    message: parseDiagnostic.Message));
            }

            var currentBlocks = currentBlocksByPath.GetValueOrDefault(path) ?? [];
            var matches = BlockMatcher.Match(historicalBlocks, currentBlocks);
            foreach (var match in matches.Historical)
            {
                var blockCoverage = match.Historical.IsInsideTable
                    ? tableCoverage
                    : fencedCoverage;
                var hasTitle = match.Historical.Title is not null;
                var hasHighlight = HasAnnotation(
                    match.Historical,
                    AnnotationKind.Highlight);
                var hasError = HasAnnotation(match.Historical, AnnotationKind.Error);
                var hasWarning = HasAnnotation(
                    match.Historical,
                    AnnotationKind.Warning);

                if (match.Status == BlockMatchStatus.Ambiguous)
                {
                    blockCoverage.Add(CoverageDisposition.Ambiguous);
                    AddMetadataCoverage(
                        CoverageDisposition.Ambiguous,
                        hasTitle,
                        hasHighlight,
                        hasError,
                        hasWarning,
                        titleCoverage,
                        highlightCoverage,
                        errorCoverage,
                        warningCoverage);
                    diagnostics.Add(Diagnostic(
                        "AMBIGUOUS_BLOCK",
                        "error",
                        path,
                        match,
                        message: "The normalized hash has multiple current candidates: "
                            + string.Join(", ", match.CandidateCurrentOrdinals)
                            + $". Affected metadata: "
                            + $"{AffectedMetadata(match.Historical)}."));
                    continue;
                }

                if (match.Status == BlockMatchStatus.Unmatched)
                {
                    blockCoverage.Add(CoverageDisposition.Unmatched);
                    AddMetadataCoverage(
                        CoverageDisposition.Unmatched,
                        hasTitle,
                        hasHighlight,
                        hasError,
                        hasWarning,
                        titleCoverage,
                        highlightCoverage,
                        errorCoverage,
                        warningCoverage);
                    diagnostics.Add(Diagnostic(
                        "UNMATCHED_BLOCK",
                        "error",
                        path,
                        match,
                        message: "No current block in the exact path has this "
                            + "normalized code hash. Affected metadata: "
                            + $"{AffectedMetadata(match.Historical)}."));
                    continue;
                }

                var current = match.Current
                    ?? throw new InvalidDataException(
                        "A matched block has no current block.");
                if (!HasExpectedTargetKind(match.Historical, current))
                {
                    blockCoverage.Add(CoverageDisposition.Unrepresentable);
                    AddMetadataCoverage(
                        CoverageDisposition.Unrepresentable,
                        hasTitle,
                        hasHighlight,
                        hasError,
                        hasWarning,
                        titleCoverage,
                        highlightCoverage,
                        errorCoverage,
                        warningCoverage);
                    diagnostics.Add(Diagnostic(
                        "UNEXPECTED_TARGET_KIND",
                        "error",
                        path,
                        match,
                        current,
                        message: match.Historical.IsInsideTable
                            ? "A historical table block must match a current raw "
                                + "<pre> inside a table."
                            : "A historical non-table block must match a current "
                                + "fenced code block."));
                    continue;
                }

                var planning = MetadataPlanner.Plan(match.Historical, current);
                var blockDisposition = planning.Diagnostics.Count == 0
                    ? CoverageDisposition.Matched
                    : CoverageDisposition.Unrepresentable;
                blockCoverage.Add(blockDisposition);
                AddPlannedMetadataCoverage(
                    planning,
                    hasTitle,
                    hasHighlight,
                    hasError,
                    hasWarning,
                    titleCoverage,
                    highlightCoverage,
                    errorCoverage,
                    warningCoverage);
                foreach (var diagnostic in planning.Diagnostics)
                {
                    diagnostics.Add(Diagnostic(
                        diagnostic.Code,
                        "error",
                        path,
                        match,
                        current,
                        diagnostic.Kind,
                        diagnostic.Message));
                }

                if (HasRepresentableMetadata(planning.Plan))
                {
                    plans.Add(new ReportPlan(
                        path,
                        match.Historical.Ordinal,
                        current.Ordinal,
                        match.Historical.SourceLine,
                        current.SourceLine,
                        TargetKind(current),
                        MatchMethod(match.Method),
                        match.Hash,
                        planning.Plan));
                }
            }

            foreach (var currentOnly in matches.CurrentOnly)
            {
                diagnostics.Add(Diagnostic(
                    "CURRENT_ONLY_BLOCK",
                    "warning",
                    path,
                    currentOrdinal: currentOnly.Ordinal,
                    currentLine: currentOnly.SourceLine,
                    message: "The current block has no historical normalized-code hash."));
            }
        }

        foreach (var path in input.CurrentDocuments.Keys
                     .Except(input.HistoricalDocuments.Keys, StringComparer.Ordinal)
                     .Order(StringComparer.Ordinal))
        {
            diagnostics.Add(Diagnostic(
                "CURRENT_ONLY_DOCUMENT",
                "warning",
                path,
                message: "The current Markdown document does not exist in the "
                    + "pinned historical tree."));
        }

        plans.Sort(ComparePlans);
        diagnostics.Sort(CompareDiagnostics);
        var allCurrentBlocks = currentBlocksByPath.Values.SelectMany(static blocks => blocks);
        var report = new MigrationReport(
            1,
            "dry-run",
            new ReportSource(input.SourceCommit, input.SourcePath),
            new ReportTarget(input.TargetCommit, input.TargetPath),
            "html-decode+lf+trim-line-end+trim-blank-frame+common-indent-v1",
            "path+sha256+ordinal+unique-hash+target-kind-v1",
            new ReportTotals(
                input.HistoricalDocuments.Count,
                input.CurrentDocuments.Count,
                historicalBlockCount,
                malformedHistoricalCaseCount,
                allCurrentBlocks.Count(static block =>
                    block.Kind == CurrentCodeBlockKind.Fenced),
                allCurrentBlocks.Count(static block =>
                    block.Kind == CurrentCodeBlockKind.RawPre
                    && !block.IsInsideTable),
                allCurrentBlocks.Count(static block =>
                    block.Kind == CurrentCodeBlockKind.RawPre
                    && block.IsInsideTable)),
            new CoverageReport(
                titleCoverage.ToReport(),
                highlightCoverage.ToReport(),
                errorCoverage.ToReport(),
                warningCoverage.ToReport(),
                fencedCoverage.ToReport(),
                tableCoverage.ToReport()),
            plans,
            diagnostics);
        return new MigrationAnalysisOutcome(
            report,
            diagnostics.Any(static diagnostic => diagnostic.Severity == "error")
                ? 3
                : 0);
    }

    private static Dictionary<string, IReadOnlyList<CurrentCodeBlock>>
        ParseCurrentDocuments(
            IReadOnlyDictionary<string, string> documents,
            ICollection<ReportDiagnostic> diagnostics)
    {
        var blocksByPath = new Dictionary<string, IReadOnlyList<CurrentCodeBlock>>(
            StringComparer.Ordinal);
        foreach (var (path, document) in documents
                     .OrderBy(static item => item.Key, StringComparer.Ordinal))
        {
            try
            {
                blocksByPath.Add(path, CurrentBlockDiscoverer.Discover(document));
            }
            catch (InvalidDataException exception)
            {
                blocksByPath.Add(path, []);
                diagnostics.Add(Diagnostic(
                    "MALFORMED_CURRENT_MARKUP",
                    "error",
                    path,
                    message: exception.Message));
            }
        }

        return blocksByPath;
    }

    private static void AddMetadataCoverage(
        CoverageDisposition disposition,
        bool hasTitle,
        bool hasHighlight,
        bool hasError,
        bool hasWarning,
        CoverageAccumulator title,
        CoverageAccumulator highlight,
        CoverageAccumulator error,
        CoverageAccumulator warning)
    {
        if (hasTitle)
        {
            title.Add(disposition);
        }

        if (hasHighlight)
        {
            highlight.Add(disposition);
        }

        if (hasError)
        {
            error.Add(disposition);
        }

        if (hasWarning)
        {
            warning.Add(disposition);
        }
    }

    private static void AddPlannedMetadataCoverage(
        MetadataPlanningResult planning,
        bool hasTitle,
        bool hasHighlight,
        bool hasError,
        bool hasWarning,
        CoverageAccumulator title,
        CoverageAccumulator highlight,
        CoverageAccumulator error,
        CoverageAccumulator warning)
    {
        if (hasTitle)
        {
            title.Add(IsUnrepresentable(planning, null)
                ? CoverageDisposition.Unrepresentable
                : CoverageDisposition.Matched);
        }

        if (hasHighlight)
        {
            highlight.Add(IsUnrepresentable(planning, AnnotationKind.Highlight)
                ? CoverageDisposition.Unrepresentable
                : CoverageDisposition.Matched);
        }

        if (hasError)
        {
            error.Add(IsUnrepresentable(planning, AnnotationKind.Error)
                ? CoverageDisposition.Unrepresentable
                : CoverageDisposition.Matched);
        }

        if (hasWarning)
        {
            warning.Add(IsUnrepresentable(planning, AnnotationKind.Warning)
                ? CoverageDisposition.Unrepresentable
                : CoverageDisposition.Matched);
        }
    }

    private static bool IsUnrepresentable(
        MetadataPlanningResult planning,
        AnnotationKind? kind) =>
        planning.Diagnostics.Any(diagnostic =>
            diagnostic.Kind == kind
            || diagnostic.Code == "MATCHED_CODE_CHANGED"
            || kind is null && diagnostic.Code == "UNREPRESENTABLE_TITLE");

    private static bool HasAnnotation(
        HistoricalCodeBlock block,
        AnnotationKind kind) =>
        block.Annotations.Any(annotation => annotation.Kind == kind);

    private static string AffectedMetadata(HistoricalCodeBlock block)
    {
        var kinds = new List<string>();
        if (block.Title is not null)
        {
            kinds.Add("title");
        }

        foreach (var kind in Enum.GetValues<AnnotationKind>())
        {
            if (HasAnnotation(block, kind))
            {
                kinds.Add(kind.ToString().ToLowerInvariant());
            }
        }

        return kinds.Count == 0 ? "none" : string.Join(", ", kinds);
    }

    private static bool HasExpectedTargetKind(
        HistoricalCodeBlock historical,
        CurrentCodeBlock current) =>
        historical.IsInsideTable
            ? current.Kind == CurrentCodeBlockKind.RawPre && current.IsInsideTable
            : current.Kind == CurrentCodeBlockKind.Fenced;

    private static bool HasRepresentableMetadata(BlockMetadataPlan plan) =>
        plan.Title is not null
        || plan.Highlight is not null
        || plan.Error is not null
        || plan.Warning is not null;

    private static string TargetKind(CurrentCodeBlock block) =>
        block.Kind == CurrentCodeBlockKind.Fenced
            ? "fenced"
            : block.IsInsideTable
                ? "rawPreInTable"
                : "rawPre";

    private static string MatchMethod(BlockMatchMethod? method) =>
        method switch
        {
            BlockMatchMethod.OrdinalAndHash => "ordinalAndHash",
            BlockMatchMethod.UniqueHashFallback => "uniqueHashFallback",
            _ => throw new InvalidDataException("A matched block has no match method."),
        };

    private static ReportDiagnostic Diagnostic(
        string code,
        string severity,
        string path,
        BlockMatch? match = null,
        CurrentCodeBlock? current = null,
        AnnotationKind? kind = null,
        string? message = null,
        int? historicalOrdinal = null,
        int? historicalLine = null,
        int? currentOrdinal = null,
        int? currentLine = null) =>
        new(
            code,
            severity,
            path,
            match?.Historical.Ordinal ?? historicalOrdinal,
            current?.Ordinal ?? currentOrdinal,
            match?.Historical.SourceLine ?? historicalLine,
            current?.SourceLine ?? currentLine,
            match?.Hash,
            kind?.ToString().ToLowerInvariant(),
            message ?? string.Empty);

    private static int ComparePlans(ReportPlan left, ReportPlan right)
    {
        var path = string.Compare(left.Path, right.Path, StringComparison.Ordinal);
        return path != 0
            ? path
            : left.HistoricalOrdinal.CompareTo(right.HistoricalOrdinal);
    }

    private static int CompareDiagnostics(
        ReportDiagnostic left,
        ReportDiagnostic right)
    {
        var comparison = string.Compare(left.Path, right.Path, StringComparison.Ordinal);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Nullable.Compare(left.HistoricalOrdinal, right.HistoricalOrdinal);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Nullable.Compare(left.CurrentOrdinal, right.CurrentOrdinal);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = string.Compare(left.Code, right.Code, StringComparison.Ordinal);
        return comparison != 0
            ? comparison
            : string.Compare(
                left.MetadataKind,
                right.MetadataKind,
                StringComparison.Ordinal);
    }

    private enum CoverageDisposition
    {
        Matched,
        Ambiguous,
        Unmatched,
        Unrepresentable,
    }

    private sealed class CoverageAccumulator
    {
        private int _matched;
        private int _ambiguous;
        private int _unmatched;
        private int _unrepresentable;

        public void Add(CoverageDisposition disposition)
        {
            switch (disposition)
            {
                case CoverageDisposition.Matched:
                    _matched++;
                    break;
                case CoverageDisposition.Ambiguous:
                    _ambiguous++;
                    break;
                case CoverageDisposition.Unmatched:
                    _unmatched++;
                    break;
                case CoverageDisposition.Unrepresentable:
                    _unrepresentable++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(disposition),
                        disposition,
                        null);
            }
        }

        public CoverageCounts ToReport() =>
            new(
                _matched + _ambiguous + _unmatched + _unrepresentable,
                _matched,
                _ambiguous,
                _unmatched,
                _unrepresentable);
    }
}

internal static class MigrationReportWriter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public static byte[] Serialize(MigrationReport report) =>
        Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(report, Options).ReplaceLineEndings("\n") + "\n");
}
