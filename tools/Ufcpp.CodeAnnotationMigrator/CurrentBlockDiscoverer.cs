using Markdig;
using Markdig.Syntax;

namespace Ufcpp.CodeAnnotationMigrator;

internal static class CurrentBlockDiscoverer
{
    public static IReadOnlyList<CurrentCodeBlock> Discover(string document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var candidates = Markdown.Parse(document)
            .Descendants()
            .OfType<FencedCodeBlock>()
            .Select(block => CreateFencedCandidate(document, block))
            .Concat(LegacyPreParser.Parse(document).Select(block =>
                new CurrentBlockCandidate(
                    block.SourceOffset,
                    block.SourceLine,
                    CurrentCodeBlockKind.RawPre,
                    block.IsInsideTable,
                    block.Code)))
            .OrderBy(static block => block.SourceOffset)
            .ToArray();

        return candidates
            .Select((block, index) => new CurrentCodeBlock(
                index + 1,
                block.SourceOffset,
                block.SourceLine,
                block.Kind,
                block.IsInsideTable,
                block.Code))
            .ToArray();
    }

    private static CurrentBlockCandidate CreateFencedCandidate(
        string document,
        FencedCodeBlock block)
    {
        if (block.IsOpen)
        {
            throw new InvalidDataException(
                $"Fenced code block at line "
                + $"{SourceText.GetLineNumber(document, block.Span.Start)} "
                + "has no valid closing fence.");
        }

        return new CurrentBlockCandidate(
            block.Span.Start,
            SourceText.GetLineNumber(document, block.Span.Start),
            CurrentCodeBlockKind.Fenced,
            false,
            block.Lines.ToString());
    }

    private sealed record CurrentBlockCandidate(
        int SourceOffset,
        int SourceLine,
        CurrentCodeBlockKind Kind,
        bool IsInsideTable,
        string Code);
}
