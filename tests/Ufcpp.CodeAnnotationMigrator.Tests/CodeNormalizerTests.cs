namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class CodeNormalizerTests
{
    [Fact]
    public void Normalize_DecodesEntitiesAndCanonicalizesFramingWhitespace()
    {
        const string Code =
            "\r\n\t  alpha &lt; beta  \r\n\t  \tgamma\t \r\n\r\n";

        var normalized = CodeNormalizer.Normalize(Code);

        Assert.Equal("alpha < beta\n\tgamma", normalized);
    }

    [Fact]
    public void Normalize_ConvertsBareCarriageReturnsAndPreservesInternalBlankLines()
    {
        const string Code = "  first\r  \r  second\r";

        var normalized = CodeNormalizer.Normalize(Code);

        Assert.Equal("first\n\nsecond", normalized);
    }

    [Fact]
    public void Normalize_DoesNotTreatTabsAsEquivalentToSpaces()
    {
        const string Code = " \talpha\n  beta";

        var normalized = CodeNormalizer.Normalize(Code);

        Assert.Equal("\talpha\n beta", normalized);
    }

    [Fact]
    public void Hash_UsesLowercaseSha256OfNormalizedUtf8()
    {
        var left = CodeNormalizer.Hash("\r\n  value &amp; more  \r\n");
        var right = CodeNormalizer.Hash("value & more");

        Assert.Equal(right, left);
        Assert.Matches("^[0-9a-f]{64}$", left);
    }
}
