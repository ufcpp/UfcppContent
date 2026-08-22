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
    string Text);

internal sealed record HistoricalCodeBlock(
    int Ordinal,
    int SourceOffset,
    int SourceLine,
    bool IsInsideTable,
    string? Title,
    string Code,
    IReadOnlyList<AnnotationSelection> Annotations);

internal sealed record HistoricalParseDiagnostic(
    int Ordinal,
    int SourceLine,
    bool IsInsideTable,
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
