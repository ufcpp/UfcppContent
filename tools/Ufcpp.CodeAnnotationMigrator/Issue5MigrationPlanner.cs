namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record Issue5MigrationResult(
    IReadOnlyDictionary<string, string> ChangedDocuments,
    IReadOnlyList<ReportPlan> Plans,
    IReadOnlyList<Issue5ExceptionEntry> Exceptions);

internal enum Issue5ExceptionDisposition
{
    MalformedResolved,
    Obsolete,
    Unrelated,
}

internal sealed record Issue5ExceptionApplication(ReportPlan? Plan);

internal sealed record Issue5ExceptionEntry(
    string Id,
    string DiagnosticCode,
    string Path,
    int? HistoricalOrdinal,
    int HistoricalLine,
    string HistoricalDocumentBlob,
    int? CurrentOrdinal,
    int? CurrentLine,
    IReadOnlyList<string> CurrentHashes,
    Issue5ExceptionDisposition Disposition,
    AnnotationKind? Kind,
    string? RangeCoordinates,
    int HistoricalSelections,
    int RestoredSelections,
    string Reason,
    string? CurrentRangeHash = null,
    string? ExpectedRangeText = null)
{
    public bool Matches(ReportDiagnostic diagnostic) =>
        string.Equals(diagnostic.Code, DiagnosticCode, StringComparison.Ordinal)
        && string.Equals(diagnostic.Path, Path, StringComparison.Ordinal)
        && diagnostic.HistoricalOrdinal == HistoricalOrdinal
        && diagnostic.HistoricalLine == HistoricalLine;

    public Issue5ExceptionApplication CreateApplication(MigrationAnalysisInput input)
    {
        if (!input.HistoricalDocuments.TryGetValue(Path, out var historicalDocument)
            || !string.Equals(
                GitBlobId.Compute(historicalDocument),
                HistoricalDocumentBlob,
                StringComparison.Ordinal))
        {
            throw Invalid("historical document blob is stale.");
        }

        if (CurrentOrdinal is null)
        {
            if (CurrentLine is not null
                || CurrentHashes.Count != 0
                || RangeCoordinates is not null)
            {
                throw Invalid("has incomplete current target guards.");
            }

            return new Issue5ExceptionApplication(null);
        }

        if (CurrentLine is null
            || CurrentHashes.Count == 0
            || !input.CurrentDocuments.TryGetValue(Path, out var currentDocument))
        {
            throw Invalid("references a missing current target.");
        }

        var blocks = CurrentBlockDiscoverer.Discover(currentDocument);
        if (CurrentOrdinal <= 0 || CurrentOrdinal > blocks.Count)
        {
            throw Invalid($"current block {CurrentOrdinal} does not exist.");
        }

        var current = blocks[CurrentOrdinal.Value - 1];
        var hash = CodeNormalizer.Hash(current.Code);
        if (current.SourceLine != CurrentLine
            || !CurrentHashes.Contains(hash, StringComparer.Ordinal))
        {
            throw Invalid("current block identity is stale.");
        }

        if (RangeCoordinates is null)
        {
            return new Issue5ExceptionApplication(null);
        }

        if (HistoricalOrdinal is null || Kind is null)
        {
            throw Invalid("a restored range requires historical identity and kind.");
        }

        var rangeValue =
            $"sha256:{HighlightRangePlanner.ComputeHash(current.Code)};{RangeCoordinates}";
        if (CurrentRangeHash is null
            || ExpectedRangeText is null
            || !string.Equals(
                HighlightRangePlanner.ComputeHash(current.Code),
                CurrentRangeHash,
                StringComparison.Ordinal))
        {
            throw Invalid("exact range code fingerprint is stale.");
        }

        var range = AssertSingle(HighlightRangePlanner.Parse(current.Code, rangeValue));
        if (!string.Equals(
                current.Code[range.Start..range.End],
                ExpectedRangeText,
                StringComparison.Ordinal))
        {
            throw Invalid("exact range text is stale.");
        }

        var selection = new SelectionMetadataPlan(null, null, rangeValue);
        return new Issue5ExceptionApplication(
            new ReportPlan(
                Path,
                HistoricalOrdinal.Value,
                CurrentOrdinal.Value,
                HistoricalLine,
                current.SourceLine,
                current.Kind == CurrentCodeBlockKind.Fenced
                    ? "fenced"
                    : current.IsInsideTable
                        ? "rawPreInTable"
                        : "rawPre",
                "explicitOverride",
                hash,
                new BlockMetadataPlan(
                    null,
                    null,
                    Kind == AnnotationKind.Error ? selection : null,
                    Kind == AnnotationKind.Warning ? selection : null)));

        static HighlightSourceRange AssertSingle(
            IReadOnlyList<HighlightSourceRange> ranges) =>
            ranges.Count == 1
                ? ranges[0]
                : throw new InvalidDataException(
                    "An Issue #5 exception range must contain one interval.");
    }

    private InvalidDataException Invalid(string message) =>
        new($"Issue #5 exception {Id}: {message}");
}

internal static class Issue5ExceptionCatalog
{
    public static IReadOnlyList<Issue5ExceptionEntry> Entries { get; } =
    [
        FromIssue4(
            "EX5-MALFORMED-TIPSUNION",
            "OVR-MALFORMED-1",
            Issue5ExceptionDisposition.Unrelated,
            null,
            null,
            0,
            0,
            "The malformed block contains no error or warning annotation."),
        FromIssue4(
            "EX5-OBSOLETE-DEFAULTABLE-WARNING",
            "OVR-MALFORMED-2",
            Issue5ExceptionDisposition.Obsolete,
            AnnotationKind.Warning,
            null,
            1,
            0,
            "The malformed warning selected null in new(null), while the exact "
            + "current block intentionally uses new() and no longer contains the token."),
        FromIssue4(
            "EX5-MALFORMED-INTERFACE-6",
            "OVR-MALFORMED-3",
            Issue5ExceptionDisposition.Unrelated,
            null,
            null,
            0,
            0,
            "The malformed IDisposable block contains no Issue #5 annotation."),
        FromIssue4(
            "EX5-MALFORMED-INTERFACE-13",
            "OVR-MALFORMED-4",
            Issue5ExceptionDisposition.Unrelated,
            null,
            null,
            0,
            0,
            "The malformed IEnumerable block contains an Issue #4 highlight only."),
        FromIssue4(
            "EX5-RECOVER-EXCEPTION-ERROR",
            "OVR-MALFORMED-5",
            Issue5ExceptionDisposition.MalformedResolved,
            AnnotationKind.Error,
            "7:7-7:22",
            1,
            1,
            "The second FormatException remains at the exact guarded current "
            + "position despite the unrelated orphan </em>.",
            "ae20f5621a04db93b99b2514585b2b767326c8fbda70c5a7cf5566abbb599163",
            "FormatException"),
        new(
            "EX5-ORPHAN-PRE-CLOSE",
            "ORPHAN_HISTORICAL_PRE_CLOSE",
            "blog/2026/3/sourcegeneratordemo/index.md",
            null,
            117,
            "d0a640a09c53cefa394a1d2be1337a8430ac610e",
            null,
            null,
            [],
            Issue5ExceptionDisposition.Unrelated,
            null,
            null,
            0,
            0,
            "The orphan closing tag carries no annotation and represents no block."),
        new(
            "EX5-OBSOLETE-EMPTY-ERROR",
            "EMPTY_ANNOTATION_SELECTION",
            "study/misc/list/test.md",
            2,
            144,
            "265988cfbae6d7e5484b6d0504ba3daaa39fedaf",
            2,
            150,
            ["f487f56f66d6f2266cc42ba8e1d63e01cdf78097726f506299241709ea5ae690"],
            Issue5ExceptionDisposition.Obsolete,
            AnnotationKind.Error,
            null,
            1,
            0,
            "The empty CS1525 wrapper has no visible text or underline; the four "
            + "non-empty nested errors in the same block remain range-mapped."),
    ];

    private static Issue5ExceptionEntry FromIssue4(
        string id,
        string issue4Id,
        Issue5ExceptionDisposition disposition,
        AnnotationKind? kind,
        string? rangeCoordinates,
        int historicalSelections,
        int restoredSelections,
        string reason,
        string? currentRangeHash = null,
        string? expectedRangeText = null)
    {
        var source = Issue4OverrideCatalog.Entries.Single(
            entry => string.Equals(entry.Id, issue4Id, StringComparison.Ordinal));
        return new Issue5ExceptionEntry(
            id,
            "MALFORMED_HISTORICAL_BLOCK",
            source.Path,
            source.HistoricalOrdinal,
            source.HistoricalLine,
            source.HistoricalDocumentBlob
                ?? throw new InvalidDataException(
                    $"Issue #4 exception {issue4Id} has no document blob guard."),
            source.CurrentOrdinal,
            source.CurrentLine,
            source.CurrentHashes,
            disposition,
            kind,
            rangeCoordinates,
            historicalSelections,
            restoredSelections,
            reason,
            currentRangeHash,
            expectedRangeText);
    }
}

internal static class Issue5MigrationPlanner
{
    public static Issue5MigrationResult Plan(
        MigrationAnalysisInput input,
        MigrationReport report,
        IReadOnlyList<Issue5ExceptionEntry>? exceptions = null)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(report);
        exceptions ??= Issue5ExceptionCatalog.Entries;

        if (!string.Equals(input.TargetCommit, report.Target.Commit, StringComparison.Ordinal)
            || !string.Equals(input.TargetPath, report.Target.Path, StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "Issue #5 migration report does not describe the supplied target.");
        }

        var issue4 = Issue4MigrationPlanner.Plan(input, report);
        if (issue4.ChangedDocuments.Count != 0)
        {
            throw new InvalidDataException(
                "Issue #5 requires an idempotent Issue #4 migration base.");
        }

        var plans = report.Plans
            .Where(static plan =>
                plan.Metadata.Error is not null || plan.Metadata.Warning is not null)
            .Select(static plan => plan with
            {
                Metadata = new BlockMetadataPlan(
                    null,
                    null,
                    plan.Metadata.Error,
                    plan.Metadata.Warning),
            })
            .ToList();
        var appliedExceptions = new List<Issue5ExceptionEntry>();
        foreach (var entry in exceptions.OrderBy(static entry => entry.Id, StringComparer.Ordinal))
        {
            var hasHistorical = input.HistoricalDocuments.ContainsKey(entry.Path);
            var hasCurrent = input.CurrentDocuments.ContainsKey(entry.Path);
            if (!hasHistorical && !hasCurrent)
            {
                continue;
            }

            if (!hasHistorical)
            {
                throw new InvalidDataException(
                    $"Issue #5 exception {entry.Id} references a missing "
                    + "historical document.");
            }

            var application = entry.CreateApplication(input);
            if (application.Plan is not null)
            {
                plans.Add(application.Plan);
            }

            appliedExceptions.Add(entry);
        }

        EnsureNoUnresolvedIssue5Diagnostics(report, appliedExceptions);
        plans.Sort(static (left, right) =>
        {
            var path = string.Compare(left.Path, right.Path, StringComparison.Ordinal);
            return path != 0
                ? path
                : left.CurrentOrdinal.CompareTo(right.CurrentOrdinal);
        });

        var duplicate = plans
            .GroupBy(static plan => (plan.Path, plan.CurrentOrdinal))
            .FirstOrDefault(static group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidDataException(
                $"Issue #5 has duplicate plans for '{duplicate.Key.Path}' "
                + $"block {duplicate.Key.CurrentOrdinal}.");
        }

        var changed = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var group in plans.GroupBy(static plan => plan.Path, StringComparer.Ordinal))
        {
            if (!input.CurrentDocuments.TryGetValue(group.Key, out var source))
            {
                throw new InvalidDataException(
                    $"Issue #5 plan references missing current document '{group.Key}'.");
            }

            var rewritten = DocumentAnnotationRewriter.Rewrite(
                group.Key,
                source,
                group.ToArray()).Content;
            ValidatePlanTargets(group.Key, rewritten, group);
            if (!string.Equals(source, rewritten, StringComparison.Ordinal))
            {
                changed.Add(group.Key, rewritten);
            }
        }

        return new Issue5MigrationResult(changed, plans, appliedExceptions);
    }

    private static void EnsureNoUnresolvedIssue5Diagnostics(
        MigrationReport report,
        IReadOnlyList<Issue5ExceptionEntry> exceptions)
    {
        foreach (var diagnostic in report.Diagnostics.Where(
                     static diagnostic => diagnostic.Severity == "error"))
        {
            if (exceptions.Any(entry => entry.Matches(diagnostic)))
            {
                continue;
            }

            var affectsIssue5 =
                diagnostic.MetadataKind is "error" or "warning"
                || diagnostic.Code.StartsWith("MALFORMED_", StringComparison.Ordinal)
                || diagnostic.Code == "ORPHAN_HISTORICAL_PRE_CLOSE"
                || diagnostic.Code is "AMBIGUOUS_BLOCK" or "UNMATCHED_BLOCK"
                    && (diagnostic.Message.Contains("error", StringComparison.Ordinal)
                        || diagnostic.Message.Contains("warning", StringComparison.Ordinal));
            if (affectsIssue5)
            {
                throw new InvalidDataException(
                    $"Unresolved Issue #5 diagnostic {diagnostic.Code} at "
                    + $"'{diagnostic.Path}' historical block "
                    + $"{diagnostic.HistoricalOrdinal?.ToString() ?? "n/a"}: "
                    + diagnostic.Message);
            }
        }
    }

    private static void ValidatePlanTargets(
        string path,
        string document,
        IEnumerable<ReportPlan> plans)
    {
        var blocks = CurrentBlockDiscoverer.Discover(document);
        foreach (var plan in plans)
        {
            if (plan.CurrentOrdinal <= 0
                || plan.CurrentOrdinal > blocks.Count
                || !string.Equals(
                    CodeNormalizer.Hash(blocks[plan.CurrentOrdinal - 1].Code),
                    plan.Hash,
                    StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Issue #5 rewrite changed code for '{path}' block "
                    + $"{plan.CurrentOrdinal}.");
            }
        }
    }
}
