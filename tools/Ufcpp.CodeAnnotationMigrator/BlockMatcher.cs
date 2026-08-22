namespace Ufcpp.CodeAnnotationMigrator;

internal enum BlockMatchStatus
{
    Matched,
    Ambiguous,
    Unmatched,
}

internal enum BlockMatchMethod
{
    OrdinalAndHash,
    UniqueHashFallback,
}

internal sealed record BlockMatch(
    HistoricalCodeBlock Historical,
    CurrentCodeBlock? Current,
    string Hash,
    BlockMatchStatus Status,
    BlockMatchMethod? Method,
    IReadOnlyList<int> CandidateCurrentOrdinals);

internal sealed record DocumentBlockMatches(
    IReadOnlyList<BlockMatch> Historical,
    IReadOnlyList<CurrentCodeBlock> CurrentOnly);

internal static class BlockMatcher
{
    public static DocumentBlockMatches Match(
        IReadOnlyList<HistoricalCodeBlock> historical,
        IReadOnlyList<CurrentCodeBlock> current)
    {
        ArgumentNullException.ThrowIfNull(historical);
        ArgumentNullException.ThrowIfNull(current);

        var historicalByHash = historical
            .Select(block => new HashedHistorical(block, CodeNormalizer.Hash(block.Code)))
            .GroupBy(static block => block.Hash, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.OrderBy(item => item.Block.Ordinal).ToArray(),
                StringComparer.Ordinal);
        var currentByHash = current
            .Select(block => new HashedCurrent(block, CodeNormalizer.Hash(block.Code)))
            .GroupBy(static block => block.Hash, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.OrderBy(item => item.Block.Ordinal).ToArray(),
                StringComparer.Ordinal);
        var matches = new List<BlockMatch>(historical.Count);
        var referencedCurrentOrdinals = new HashSet<int>();

        foreach (var (hash, historicalGroup) in historicalByHash)
        {
            if (!currentByHash.TryGetValue(hash, out var currentGroup))
            {
                matches.AddRange(historicalGroup.Select(item => new BlockMatch(
                    item.Block,
                    null,
                    hash,
                    BlockMatchStatus.Unmatched,
                    null,
                    [])));
                continue;
            }

            foreach (var candidate in currentGroup)
            {
                referencedCurrentOrdinals.Add(candidate.Block.Ordinal);
            }

            if (historicalGroup.Length > 1 || currentGroup.Length > 1)
            {
                var historicalOrdinals = historicalGroup
                    .Select(static item => item.Block.Ordinal);
                var currentOrdinals = currentGroup.Select(static item => item.Block.Ordinal);
                if (historicalGroup.Length == currentGroup.Length
                    && historicalOrdinals.SequenceEqual(currentOrdinals))
                {
                    matches.AddRange(historicalGroup.Zip(
                        currentGroup,
                        (source, target) => Matched(
                            source.Block,
                            target.Block,
                            hash,
                            BlockMatchMethod.OrdinalAndHash)));
                }
                else
                {
                    var candidates = currentGroup
                        .Select(static item => item.Block.Ordinal)
                        .ToArray();
                    matches.AddRange(historicalGroup.Select(item => new BlockMatch(
                        item.Block,
                        null,
                        hash,
                        BlockMatchStatus.Ambiguous,
                        null,
                        candidates)));
                }

                continue;
            }

            var historicalBlock = historicalGroup[0].Block;
            var currentBlock = currentGroup[0].Block;
            matches.Add(Matched(
                historicalBlock,
                currentBlock,
                hash,
                historicalBlock.Ordinal == currentBlock.Ordinal
                    ? BlockMatchMethod.OrdinalAndHash
                    : BlockMatchMethod.UniqueHashFallback));
        }

        matches.Sort(static (left, right) =>
            left.Historical.Ordinal.CompareTo(right.Historical.Ordinal));
        var currentOnly = current
            .Where(block => !referencedCurrentOrdinals.Contains(block.Ordinal))
            .OrderBy(static block => block.Ordinal)
            .ToArray();
        return new DocumentBlockMatches(matches, currentOnly);
    }

    private static BlockMatch Matched(
        HistoricalCodeBlock historical,
        CurrentCodeBlock current,
        string hash,
        BlockMatchMethod method) =>
        new(
            historical,
            current,
            hash,
            BlockMatchStatus.Matched,
            method,
            []);

    private sealed record HashedHistorical(
        HistoricalCodeBlock Block,
        string Hash);

    private sealed record HashedCurrent(
        CurrentCodeBlock Block,
        string Hash);
}
