namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class CurrentBlockDiscovererTests
{
    [Fact]
    public void Discover_MergesFencesAndRawPreInDocumentOrder()
    {
        var document = """
            `<pre>inline literal</pre>`

            ```csharp
            <pre>fenced literal</pre>
            ```

            <table><tr><td><pre><code>table &amp; raw</code></pre></td></tr></table>

            ~~~~text
            tail
            ~~~~

            <pre>outside raw</pre>
            """.ReplaceLineEndings("\n");

        var blocks = CurrentBlockDiscoverer.Discover(document);

        Assert.Collection(
            blocks,
            block =>
            {
                Assert.Equal(1, block.Ordinal);
                Assert.Equal(3, block.SourceLine);
                Assert.Equal(CurrentCodeBlockKind.Fenced, block.Kind);
                Assert.False(block.IsInsideTable);
                Assert.Equal("<pre>fenced literal</pre>", block.Code);
            },
            block =>
            {
                Assert.Equal(2, block.Ordinal);
                Assert.Equal(7, block.SourceLine);
                Assert.Equal(CurrentCodeBlockKind.RawPre, block.Kind);
                Assert.True(block.IsInsideTable);
                Assert.Equal("table & raw", block.Code);
            },
            block =>
            {
                Assert.Equal(3, block.Ordinal);
                Assert.Equal(9, block.SourceLine);
                Assert.Equal(CurrentCodeBlockKind.Fenced, block.Kind);
                Assert.False(block.IsInsideTable);
                Assert.Equal("tail", block.Code);
            },
            block =>
            {
                Assert.Equal(4, block.Ordinal);
                Assert.Equal(13, block.SourceLine);
                Assert.Equal(CurrentCodeBlockKind.RawPre, block.Kind);
                Assert.False(block.IsInsideTable);
                Assert.Equal("outside raw", block.Code);
            });
    }

    [Fact]
    public void Discover_ExcludesRawPreInsideIndentedAndInlineCode()
    {
        var document = """
                <pre>indented literal</pre>

            Text `<pre>inline literal</pre>`.
            """.ReplaceLineEndings("\n");

        Assert.Empty(CurrentBlockDiscoverer.Discover(document));
    }

    [Theory]
    [InlineData("```")]
    [InlineData("```text\nvalue")]
    [InlineData("```text\nvalue\n    ```")]
    public void Discover_RejectsUnclosedFence(string document)
    {
        var exception = Assert.Throws<InvalidDataException>(
            () => CurrentBlockDiscoverer.Discover(document));

        Assert.Contains("closing", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Discover_AcceptsClosedFenceInsideBlockQuote()
    {
        var document = """
            > ```text
            > value
            > ```
            """.ReplaceLineEndings("\n");

        var block = Assert.Single(CurrentBlockDiscoverer.Discover(document));

        Assert.Equal(CurrentCodeBlockKind.Fenced, block.Kind);
        Assert.Equal("value", block.Code);
    }
}
