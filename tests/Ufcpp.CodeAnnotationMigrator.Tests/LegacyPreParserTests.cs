namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class LegacyPreParserTests
{
    [Fact]
    public void Parse_ExtractsDecodedAnnotationsThroughNestedSyntaxMarkup()
    {
        var document = """
            Before <code>inline &lt;code&gt;</code>
            <pre class="source" title="A &amp; B">
            <code><span class="reserved">var</span> <em>value &lt; 2</em>;
            <span class="error">bad <span class="literal">&quot;x&quot;</span></span>
            <span class="warning"><strong>warn</strong></span>
            </code></pre>
            """.ReplaceLineEndings("\n");

        var block = Assert.Single(LegacyPreParser.Parse(document));

        Assert.Equal(1, block.Ordinal);
        Assert.Equal(2, block.SourceLine);
        Assert.False(block.IsInsideTable);
        Assert.Equal("A & B", block.Title);
        Assert.Equal("var value < 2;\nbad \"x\"\nwarn\n", block.Code);
        Assert.Collection(
            block.Annotations,
            annotation =>
            {
                Assert.Equal(AnnotationKind.Highlight, annotation.Kind);
                Assert.Equal("value < 2", annotation.Text);
                Assert.Equal(
                    annotation.Text,
                    block.Code.Substring(annotation.Start, annotation.Length));
            },
            annotation =>
            {
                Assert.Equal(AnnotationKind.Error, annotation.Kind);
                Assert.Equal("bad \"x\"", annotation.Text);
                Assert.Equal(
                    annotation.Text,
                    block.Code.Substring(annotation.Start, annotation.Length));
            },
            annotation =>
            {
                Assert.Equal(AnnotationKind.Warning, annotation.Kind);
                Assert.Equal("warn", annotation.Text);
                Assert.Equal(
                    annotation.Text,
                    block.Code.Substring(annotation.Start, annotation.Length));
            });
    }

    [Fact]
    public void Parse_EnumeratesOnlyPreElementsAndTracksTableContext()
    {
        var document = """
            `<pre>not a block</pre>`
            <code>inline</code>
            <table><tr><td><pre title=" "><code>table &amp; code</code></pre></td></tr></table>
            <pre>plain &lt;text&gt;</pre>
            """.ReplaceLineEndings("\n");

        var blocks = LegacyPreParser.Parse(document);

        Assert.Collection(
            blocks,
            block =>
            {
                Assert.Equal(1, block.Ordinal);
                Assert.True(block.IsInsideTable);
                Assert.Null(block.Title);
                Assert.Equal("table & code", block.Code);
            },
            block =>
            {
                Assert.Equal(2, block.Ordinal);
                Assert.False(block.IsInsideTable);
                Assert.Equal("plain <text>", block.Code);
            });
    }

    [Fact]
    public void Parse_RejectsUnbalancedAnnotationMarkup()
    {
        const string Document = "<pre><code><em>value</code></pre>";

        var exception = Assert.Throws<InvalidDataException>(
            () => LegacyPreParser.Parse(Document));

        Assert.Contains("em", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Parse_SkipsNonTagLessThanWhileFindingPreClose()
    {
        const string Document =
            "<pre><code>if (x < 10)</code></pre><pre>next</pre>";

        var blocks = LegacyPreParser.Parse(Document);

        Assert.Equal(["if (x < 10)", "next"], blocks.Select(block => block.Code));
    }

    [Fact]
    public void Parse_AllowsAnnotationsToCrossIgnoredSyntaxSpanBoundaries()
    {
        const string Document =
            "<pre><code><span class=\"type\">Type<em></span>"
            + "&lt;T&gt;</em></code></pre>";

        var block = Assert.Single(LegacyPreParser.Parse(Document));

        Assert.Equal("Type<T>", block.Code);
        var highlight = Assert.Single(block.Annotations);
        Assert.Equal(AnnotationKind.Highlight, highlight.Kind);
        Assert.Equal("<T>", highlight.Text);
    }

    [Fact]
    public void ParseDetailed_ReportsMalformedBlockAndContinuesWithOriginalOrdinals()
    {
        const string Document =
            "<pre><code>first</code></code></pre>\n"
            + "<pre><code>second</code></pre>";

        var result = LegacyPreParser.ParseDetailed(Document);

        Assert.Equal(2, result.PreBlockCount);
        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal(1, diagnostic.Ordinal);
        Assert.Equal(1, diagnostic.SourceLine);
        var block = Assert.Single(result.Blocks);
        Assert.Equal(2, block.Ordinal);
        Assert.Equal("second", block.Code);
    }

    [Fact]
    public void Parse_IgnoresMalformedSyntaxColorSpanAttributes()
    {
        const string Document =
            "<pre><code><span classbroken\">Assert</span></code></pre>";

        var block = Assert.Single(LegacyPreParser.Parse(Document));

        Assert.Equal("Assert", block.Code);
        Assert.Empty(block.Annotations);
    }

    [Fact]
    public void Parse_PreservesSlashInUnquotedAttributesWithoutInventingClassToken()
    {
        const string Document =
            "<pre title=foo/bar><code>"
            + "<span class=error/warning>value</span>"
            + "</code></pre>";

        var block = Assert.Single(LegacyPreParser.Parse(Document));

        Assert.Equal("foo/bar", block.Title);
        Assert.Equal("value", block.Code);
        Assert.Empty(block.Annotations);
    }

    [Fact]
    public void ParseDetailed_ReportsNestedPreAndStillEnumeratesInnerBlock()
    {
        const string Document = "<pre>outer<pre>inner</pre>tail</pre>";

        var result = LegacyPreParser.ParseDetailed(Document);

        Assert.Equal(2, result.PreBlockCount);
        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal(1, diagnostic.Ordinal);
        Assert.Contains("Nested", diagnostic.Message, StringComparison.Ordinal);
        var inner = Assert.Single(result.Blocks);
        Assert.Equal(2, inner.Ordinal);
        Assert.Equal("inner", inner.Code);
    }

    [Fact]
    public void Parse_DoesNotTreatSlashInUnquotedValueAsSelfClosingSyntax()
    {
        const string Document =
            "<pre><code><span class=error data=x/>value</span></code></pre>";

        var block = Assert.Single(LegacyPreParser.Parse(Document));

        var error = Assert.Single(block.Annotations);
        Assert.Equal(AnnotationKind.Error, error.Kind);
        Assert.Equal("value", error.Text);
    }
}
