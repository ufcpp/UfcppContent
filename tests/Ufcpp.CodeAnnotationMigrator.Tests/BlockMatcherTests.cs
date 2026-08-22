namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class BlockMatcherTests
{
    [Fact]
    public void Match_UsesOrdinalAndHashForAlignedBlocks()
    {
        var historical = new[]
        {
            Historical(1, "alpha"),
            Historical(2, "beta"),
        };
        var current = new[]
        {
            Current(1, "alpha"),
            Current(2, "beta"),
        };

        var result = BlockMatcher.Match(historical, current);

        Assert.Equal(2, result.Historical.Count);
        Assert.All(
            result.Historical,
            match =>
            {
                Assert.Equal(BlockMatchStatus.Matched, match.Status);
                Assert.Equal(BlockMatchMethod.OrdinalAndHash, match.Method);
                Assert.Equal(match.Historical.Ordinal, match.Current?.Ordinal);
            });
        Assert.Empty(result.CurrentOnly);
    }

    [Fact]
    public void Match_UsesUniqueHashFallbackAfterOrderShift()
    {
        var historical = new[]
        {
            Historical(1, "alpha"),
            Historical(2, "beta"),
        };
        var current = new[]
        {
            Current(1, "inserted"),
            Current(2, "alpha"),
            Current(3, "beta"),
        };

        var result = BlockMatcher.Match(historical, current);

        Assert.Equal(
            [2, 3],
            result.Historical.Select(match => match.Current?.Ordinal));
        Assert.All(
            result.Historical,
            match => Assert.Equal(BlockMatchMethod.UniqueHashFallback, match.Method));
        Assert.Equal(1, Assert.Single(result.CurrentOnly).Ordinal);
    }

    [Fact]
    public void Match_AcceptsDuplicatesOnlyWhenAllOrdinalsAlign()
    {
        var historical = new[]
        {
            Historical(1, "same"),
            Historical(2, "same"),
        };
        var current = new[]
        {
            Current(1, "same"),
            Current(2, "same"),
        };

        var result = BlockMatcher.Match(historical, current);

        Assert.Equal(2, result.Historical.Count);
        Assert.All(
            result.Historical,
            match =>
            {
                Assert.Equal(BlockMatchStatus.Matched, match.Status);
                Assert.Equal(BlockMatchMethod.OrdinalAndHash, match.Method);
            });
    }

    [Fact]
    public void Match_RejectsShiftedDuplicateHashesAsAmbiguous()
    {
        var historical = new[]
        {
            Historical(1, "same"),
            Historical(2, "same"),
        };
        var current = new[]
        {
            Current(1, "inserted"),
            Current(2, "same"),
            Current(3, "same"),
        };

        var result = BlockMatcher.Match(historical, current);

        Assert.All(
            result.Historical,
            match =>
            {
                Assert.Equal(BlockMatchStatus.Ambiguous, match.Status);
                Assert.Null(match.Current);
                Assert.Null(match.Method);
                Assert.Equal([2, 3], match.CandidateCurrentOrdinals);
            });
    }

    [Fact]
    public void Match_ReportsHashMismatchAsUnmatched()
    {
        var result = BlockMatcher.Match(
            [Historical(1, "historical")],
            [Current(1, "current")]);

        var match = Assert.Single(result.Historical);
        Assert.Equal(BlockMatchStatus.Unmatched, match.Status);
        Assert.Empty(match.CandidateCurrentOrdinals);
        Assert.Equal(1, Assert.Single(result.CurrentOnly).Ordinal);
    }

    [Fact]
    public void Match_PlainPreIndentationMatchesConvertedFence()
    {
        var historicalDocument = """
            <pre class="source" title="sample">
                case 'e':
                    ch = '\u001b';
                    break;
            </pre>
            """.ReplaceLineEndings("\n");
        var currentDocument = """
            ```csharp
                case 'e':
                    ch = '\u001b';
                    break;
            ```
            """.ReplaceLineEndings("\n");

        var result = BlockMatcher.Match(
            LegacyPreParser.Parse(historicalDocument),
            CurrentBlockDiscoverer.Discover(currentDocument));

        var match = Assert.Single(result.Historical);
        Assert.Equal(BlockMatchStatus.Matched, match.Status);
        Assert.Equal(BlockMatchMethod.OrdinalAndHash, match.Method);
    }

    private static HistoricalCodeBlock Historical(int ordinal, string code) =>
        new(ordinal, ordinal * 10, ordinal, false, null, code, []);

    private static CurrentCodeBlock Current(int ordinal, string code) =>
        new(
            ordinal,
            ordinal * 10,
            ordinal,
            CurrentCodeBlockKind.Fenced,
            false,
            code);
}
