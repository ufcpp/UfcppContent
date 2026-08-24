namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record Issue4MigrationResult(
    IReadOnlyDictionary<string, string> ChangedDocuments,
    IReadOnlyList<ReportPlan> Plans,
    IReadOnlyList<Issue4OverrideEntry> Overrides);

internal static class Issue4MigrationPlanner
{
    public static Issue4MigrationResult Plan(
        MigrationAnalysisInput input,
        MigrationReport report,
        IReadOnlyList<Issue4OverrideEntry>? overrides = null)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(report);
        overrides ??= Issue4OverrideCatalog.Entries;

        if (!string.Equals(input.TargetCommit, report.Target.Commit, StringComparison.Ordinal)
            || !string.Equals(input.TargetPath, report.Target.Path, StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "Issue #4 migration report does not describe the supplied target.");
        }

        var currentDocuments = input.CurrentDocuments.ToDictionary(
            static item => item.Key,
            static item => item.Value,
            StringComparer.Ordinal);
        var plans = report.Plans
            .Where(static plan =>
                plan.Metadata.Title is not null || plan.Metadata.Highlight is not null)
            .Select(static plan => plan with
            {
                Metadata = new BlockMetadataPlan(
                    plan.Metadata.Title,
                    plan.Metadata.Highlight,
                    null,
                    null),
            })
            .ToList();

        var appliedOverrides = new List<Issue4OverrideEntry>();
        foreach (var entry in overrides.OrderBy(static entry => entry.Id, StringComparer.Ordinal))
        {
            var hasHistorical = input.HistoricalDocuments.TryGetValue(
                entry.Path,
                out var historicalDocument);
            var hasCurrent = currentDocuments.TryGetValue(
                entry.Path,
                out var currentDocument);
            if (!hasHistorical && !hasCurrent)
            {
                continue;
            }

            if (!hasHistorical || !hasCurrent)
            {
                throw new InvalidDataException(
                    $"Issue #4 override {entry.Id} references a missing document.");
            }

            var application = entry.CreateApplication(
                entry.Path,
                historicalDocument!,
                currentDocument!);
            currentDocuments[entry.Path] = application.CurrentDocument;
            plans.Add(application.Plan);
            appliedOverrides.Add(entry);
        }

        EnsureNoUnresolvedIssue4Diagnostics(report, appliedOverrides);
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
                $"Issue #4 has duplicate plans for '{duplicate.Key.Path}' "
                + $"block {duplicate.Key.CurrentOrdinal}.");
        }

        var changed = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var group in plans.GroupBy(static plan => plan.Path, StringComparer.Ordinal))
        {
            if (!currentDocuments.TryGetValue(group.Key, out var source))
            {
                throw new InvalidDataException(
                    $"Issue #4 plan references missing current document '{group.Key}'.");
            }

            var rewritten = DocumentAnnotationRewriter.Rewrite(
                group.Key,
                source,
                group.ToArray()).Content;
            ValidatePlanTargets(group.Key, rewritten, group);
            currentDocuments[group.Key] = rewritten;
        }

        foreach (var (path, postimage) in currentDocuments)
        {
            var preimage = input.CurrentDocuments[path];
            if (!string.Equals(preimage, postimage, StringComparison.Ordinal))
            {
                changed.Add(path, postimage);
            }
        }

        return new Issue4MigrationResult(changed, plans, appliedOverrides);
    }

    private static void EnsureNoUnresolvedIssue4Diagnostics(
        MigrationReport report,
        IReadOnlyList<Issue4OverrideEntry> overrides)
    {
        var overrideKeys = overrides
            .Select(static entry => (entry.Path, (int?)entry.HistoricalOrdinal))
            .ToHashSet();
        foreach (var diagnostic in report.Diagnostics.Where(
                     static diagnostic => diagnostic.Severity == "error"))
        {
            var resolvedByOverride = overrideKeys.Contains(
                (diagnostic.Path, diagnostic.HistoricalOrdinal));
            var isBenignOrphan =
                diagnostic.Code == "ORPHAN_HISTORICAL_PRE_CLOSE"
                && diagnostic.Path == "blog/2026/3/sourcegeneratordemo/index.md"
                && diagnostic.HistoricalLine == 117;
            if (resolvedByOverride || isBenignOrphan)
            {
                continue;
            }

            var affectsIssue4 =
                diagnostic.MetadataKind == "highlight"
                || diagnostic.Code is "UNEXPECTED_TARGET_KIND"
                || diagnostic.Code.StartsWith("MALFORMED_", StringComparison.Ordinal)
                || diagnostic.Code == "ORPHAN_HISTORICAL_PRE_CLOSE"
                || diagnostic.Code is "AMBIGUOUS_BLOCK" or "UNMATCHED_BLOCK"
                    && (diagnostic.Message.Contains("title", StringComparison.Ordinal)
                        || diagnostic.Message.Contains("highlight", StringComparison.Ordinal));
            if (affectsIssue4)
            {
                throw new InvalidDataException(
                    $"Unresolved Issue #4 diagnostic {diagnostic.Code} at "
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
                    $"Issue #4 rewrite changed code for '{path}' block "
                    + $"{plan.CurrentOrdinal}.");
            }
        }
    }
}
