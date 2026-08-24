namespace Ufcpp.CodeAnnotationMigrator;

internal enum AnnotationKind
{
    Highlight,
    Error,
    Warning,
}

internal sealed record AnnotationSelection(
    AnnotationKind Kind,
    int Start,
    int Length,
    string Text,
    string? DiagnosticId = null,
    int Order = 0);

internal static class DiagnosticCode
{
    public static bool IsValid(string? value) =>
        value is { Length: 6 }
        && value[0] is 'C'
        && value[1] is 'S' or 'A'
        && value.AsSpan(2).IndexOfAnyExceptInRange('0', '9') < 0;
}

internal sealed record HistoricalCodeBlock(
    int Ordinal,
    int SourceOffset,
    int SourceLine,
    bool IsInsideTable,
    string? Title,
    string Code,
    IReadOnlyList<AnnotationSelection> Annotations);

internal sealed record HistoricalParseDiagnostic(
    string Code,
    int? Ordinal,
    int SourceLine,
    bool IsInsideTable,
    bool CountsAsBlock,
    string Message);

internal sealed record HistoricalParseResult(
    IReadOnlyList<HistoricalCodeBlock> Blocks,
    IReadOnlyList<HistoricalParseDiagnostic> Diagnostics,
    int PreBlockCount);

internal enum CurrentCodeBlockKind
{
    Fenced,
    RawPre,
}

internal sealed record CurrentCodeBlock(
    int Ordinal,
    int SourceOffset,
    int SourceLine,
    CurrentCodeBlockKind Kind,
    bool IsInsideTable,
    string Code);
