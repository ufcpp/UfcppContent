namespace Ufcpp.SiteGenerator.Rendering;

internal readonly record struct DiagnosticIdentity(
    string Id,
    int Start,
    int End,
    int Order);

internal static class DiagnosticIdentityMetadata
{
    public static IReadOnlyList<DiagnosticIdentity> Parse(
        string code,
        string value,
        string attributeName)
    {
        var identities = new List<DiagnosticIdentity>();
        var open = new Stack<DiagnosticIdentity>();
        var previousStart = -1;
        foreach (var item in value.Split(',', StringSplitOptions.None))
        {
            var separator = item.IndexOf('@');
            if (separator != 6
                || separator != item.LastIndexOf('@')
                || !IsDiagnosticId(item[..separator]))
            {
                throw Invalid(attributeName);
            }

            var range = AssertSingle(
                AnnotationRangeMetadata.Parse(
                    code,
                    item[(separator + 1)..],
                    attributeName),
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

    private static bool IsDiagnosticId(string value) =>
        value is { Length: 6 }
        && value[0] == 'C'
        && value[1] is 'S' or 'A'
        && value.AsSpan(2).IndexOfAnyExceptInRange('0', '9') < 0;

    private static (int Start, int End) AssertSingle(
        IReadOnlyList<(int Start, int End)> ranges,
        string attributeName) =>
        ranges.Count == 1
            ? ranges[0]
            : throw Invalid(attributeName);

    private static InvalidDataException Invalid(string attributeName) =>
        new(
            $"The {attributeName} attribute must contain ordered CS#### or "
            + "CA#### diagnostic ranges.");

    private static InvalidDataException Noncanonical(string attributeName) =>
        new(
            $"The {attributeName} ranges must preserve canonical nested or "
            + "disjoint legacy opening order.");
}
