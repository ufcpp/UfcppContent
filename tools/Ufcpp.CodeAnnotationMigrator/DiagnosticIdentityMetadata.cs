namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record DiagnosticIdentityPlanningResult(
    string? Value,
    string? Error);

internal readonly record struct DiagnosticIdentity(
    string Id,
    int Start,
    int End,
    int Order);

internal static class DiagnosticIdentityPlanner
{
    public static DiagnosticIdentityPlanningResult Plan(
        IReadOnlyList<AnnotationSelection> selections,
        string historicalCode,
        string currentCode)
    {
        if (selections.Count == 0)
        {
            return new DiagnosticIdentityPlanningResult(null, null);
        }

        var entries = new List<string>(selections.Count);
        foreach (var selection in selections)
        {
            if (!DiagnosticCode.IsValid(selection.DiagnosticId))
            {
                return new DiagnosticIdentityPlanningResult(
                    null,
                    $"Diagnostic ID '{selection.DiagnosticId}' is invalid.");
            }

            var projection = HighlightRangePlanner.Plan(
                [selection],
                historicalCode,
                currentCode);
            if (projection.Value is null)
            {
                return new DiagnosticIdentityPlanningResult(
                    null,
                    projection.Error ?? "A diagnostic identity cannot be projected.");
            }

            entries.Add(
                $"{selection.DiagnosticId}@"
                + projection.Value[(projection.Value.IndexOf(';') + 1)..]);
        }

        var value =
            $"sha256:{HighlightRangePlanner.ComputeHash(currentCode)};"
            + string.Join(',', entries);
        try
        {
            _ = DiagnosticIdentityMetadata.Parse(
                currentCode,
                value,
                "diagnostics");
        }
        catch (InvalidDataException exception)
        {
            return new DiagnosticIdentityPlanningResult(null, exception.Message);
        }

        return new DiagnosticIdentityPlanningResult(value, null);
    }
}

internal static class DiagnosticIdentityMetadata
{
    public static IReadOnlyList<DiagnosticIdentity> Parse(
        string code,
        string value,
        string attributeName)
    {
        const string Prefix = "sha256:";
        if (!value.StartsWith(Prefix, StringComparison.Ordinal)
            || value.Length <= Prefix.Length + 65
            || value[Prefix.Length + 64] != ';')
        {
            throw Invalid(attributeName);
        }

        var hash = value.Substring(Prefix.Length, 64);
        if (hash.Any(static character =>
                character is not (>= '0' and <= '9' or >= 'a' and <= 'f'))
            || !string.Equals(
                hash,
                HighlightRangePlanner.ComputeHash(code),
                StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                $"The {attributeName} fingerprint is invalid or stale.");
        }

        var identities = new List<DiagnosticIdentity>();
        var open = new Stack<DiagnosticIdentity>();
        var previousStart = -1;
        foreach (var item in value[(Prefix.Length + 65)..]
                     .Split(',', StringSplitOptions.None))
        {
            var separator = item.IndexOf('@');
            if (separator != 6
                || separator != item.LastIndexOf('@')
                || !DiagnosticCode.IsValid(item[..separator]))
            {
                throw Invalid(attributeName);
            }

            var rangeValue =
                $"{Prefix}{hash};{item[(separator + 1)..]}";
            var range = AssertSingle(
                HighlightRangePlanner.Parse(code, rangeValue),
                attributeName);
            if (range.Start < previousStart)
            {
                throw Noncanonical(attributeName);
            }

            while (open.Count != 0 && range.Start >= open.Peek().End)
            {
                open.Pop();
            }

            if (open.Count != 0 && range.End > open.Peek().End)
            {
                throw Noncanonical(attributeName);
            }

            var identity = new DiagnosticIdentity(
                item[..separator],
                range.Start,
                range.End,
                identities.Count);
            identities.Add(identity);
            open.Push(identity);
            previousStart = range.Start;
        }

        if (identities.Count == 0)
        {
            throw Invalid(attributeName);
        }

        return identities;
    }

    private static HighlightSourceRange AssertSingle(
        IReadOnlyList<HighlightSourceRange> ranges,
        string attributeName) =>
        ranges.Count == 1
            ? ranges[0]
            : throw Invalid(attributeName);

    private static InvalidDataException Invalid(string attributeName) =>
        new(
            $"The {attributeName} attribute must contain a lowercase SHA-256 "
            + "fingerprint and ordered CS#### or CA#### diagnostic ranges.");

    private static InvalidDataException Noncanonical(string attributeName) =>
        new(
            $"The {attributeName} ranges must preserve canonical nested or "
            + "disjoint legacy opening order.");
}
