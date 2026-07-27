using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Loading;
using Ufcpp.SiteGenerator.Rendering;
using System.Xml.Linq;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class MarkdigRendererTests
{
    public static TheoryData<string, string> RawHtmlCases => new()
    {
        {
            """<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>+</mo><mn>1</mn></math>""",
            "&lt;math"
        },
        {
            """<script async src="//speakerdeck.example/assets/embed.js"></script>""",
            "&lt;script"
        },
        {
            """<iframe src="https://example.test/embed" title="Demo"></iframe>""",
            "&lt;iframe"
        },
        {
            """<object data="data:application/x-silverlight-2," type="application/x-silverlight-2"></object>""",
            "&lt;object"
        },
        {
            """<pre class="source"><code class="language-csharp"><span class="reserved">if</span> (left &lt; right) return;</code></pre>""",
            "&lt;pre"
        },
    };

    [Fact]
    public void Render_PipeTable_EmitsTableHeaderAndCells()
    {
        var html = Render("""
            | Language | Version |
            | --- | --- |
            | C# | 13 |

            ++C++
            """);

        Assert.Contains("<table>", html);
        Assert.Contains("<thead>", html);
        Assert.Contains("<tbody>", html);
        Assert.Contains("<th>Language</th>", html);
        Assert.Contains("<th>Version</th>", html);
        Assert.Contains("<td>C#</td>", html);
        Assert.Contains("<td>13</td>", html);
        Assert.Contains("++C++", html);
        Assert.DoesNotContain("| Language | Version |", html);
        Assert.DoesNotContain("<ins>C</ins>", html);
    }

    [Theory]
    [InlineData("cs", "public string Value = \"<tag>\";", "csharp", "roslyn-keyword")]
    [InlineData("XML", "<root attr=\"value\" />", "xml", "xmlName")]
    [InlineData("ps1", "$value = Get-Item \"path\"", "powershell", "powershellCommand")]
    public void Render_SupportedFencedCode_NormalizesAndHighlights(
        string language,
        string code,
        string normalizedLanguage,
        string tokenClass)
    {
        var html = Render($"```{language}\n{code}\n```");

        Assert.Contains(
            $"<pre><code class=\"language-{normalizedLanguage}\">",
            html);
        Assert.Contains($"<span class=\"{tokenClass}\">", html);
        Assert.DoesNotContain("<tag>", html);
    }

    [Theory]
    [InlineData("cs")]
    [InlineData("csharp")]
    [InlineData("c#")]
    [InlineData("CSHARP")]
    public void Render_CSharpAliases_UseRoslyn(string language)
    {
        var html = Render($"```{language}\nrecord R;\n```");

        Assert.Contains("<pre><code class=\"language-csharp\">", html);
        Assert.Contains(
            "<span class=\"roslyn-keyword\">record</span>",
            html);
        Assert.Contains(
            "<span class=\"roslyn-record-class-name\">R</span>",
            html);
    }

    [Fact]
    public void Render_CSharpFence_ClassifiesModernSyntaxAndSymbols()
    {
        var html = Render(
            """
            ```cs
            using System.Diagnostics.CodeAnalysis;
            using System.Numerics;

            record struct X(int A)
            {
                public int B { readonly get => field; set => field = value; }
                public readonly bool TryGetValue([NotNullWhen(true)] out int? value) => (value = A) != 0;
            }

            static class Y
            {
                extension<T>(ref T x)
                    where T : struct, IIncrementOperators<T>
                {
                    public void operator +=(int count)
                    {
                        for (int i = 0; i < count; i++) x++;
                    }
                }
            }
            ```
            """);

        Assert.Contains("<span class=\"roslyn-keyword\">record</span>", html);
        Assert.Contains("<span class=\"roslyn-keyword\">field</span>", html);
        Assert.Contains(
            "<span class=\"roslyn-record-struct-name\">X</span>",
            html);
        Assert.Contains("<span class=\"roslyn-property-name\">B</span>", html);
        Assert.Contains(
            "<span class=\"roslyn-method-name\">TryGetValue</span>",
            html);
        Assert.Contains(
            "<span class=\"roslyn-parameter-name\">value</span>",
            html);
        Assert.Contains(
            "<span class=\"roslyn-interface-name\">IIncrementOperators</span>",
            html);
        Assert.Contains("<span class=\"roslyn-local-name\">i</span>", html);
        Assert.Contains("&lt;", html);
    }

    [Fact]
    public void Render_CSharpFence_PreservesTextAcrossMultipleClassifications()
    {
        const string Code = """
            C.Write("<tag>");
            static class C
            {
                public static void Write(string value) { }
            }
            """;

        var html = Render($"```csharp\n{Code}\n```");

        Assert.Contains(
            "<span class=\"roslyn-method-name roslyn-static-symbol\">Write</span>",
            html);
        Assert.DoesNotContain("<tag>", html);
        Assert.Equal(
            Code,
            ExtractRenderedCodeText(html).TrimEnd('\r', '\n'));
    }

    [Theory]
    [InlineData("")]
    [InlineData("unknown-language")]
    public void Render_UnsupportedFencedCode_FallsBackToEscapedPlainCode(
        string language)
    {
        var html = Render($"```{language}\n<tag attr=\"value\">& text</tag>\n```");

        Assert.Contains("&lt;tag attr=&quot;value&quot;&gt;&amp; text&lt;/tag&gt;", html);
        Assert.DoesNotContain("<span", html);
    }

    [Fact]
    public void Render_SupportedFencedCode_IsDeterministic()
    {
        const string Markdown = "```csharp\nvar answer = 42;\n```";

        Assert.Equal(Render(Markdown), Render(Markdown));
    }

    [Theory]
    [MemberData(nameof(RawHtmlCases))]
    public void Render_RawHtml_PreservesMarkupUnescaped(
        string rawHtml,
        string escapedOpeningTag)
    {
        var html = Render(rawHtml);

        Assert.Contains(rawHtml, html);
        Assert.DoesNotContain(escapedOpeningTag, html);
    }

    [Fact]
    public void Render_LegacyHtmlTableWithOpeningBlankLine_PreservesRowsAndAnchors()
    {
        var html = Render(
            """
            <table summary="">

            \t<tr>
            \t\t<td><strong id="entry">Entry</strong></td>
            \t</tr>
            </table>
            """.Replace("\\t", "\t", StringComparison.Ordinal));

        Assert.Contains("<table summary=\"\">", html);
        Assert.Contains("<tr>", html);
        Assert.Contains("<td><strong id=\"entry\">Entry</strong></td>", html);
        Assert.DoesNotContain("<pre><code>", html);
        Assert.DoesNotContain("&lt;strong", html);
    }

    [Fact]
    public void Render_LegacyMarkdownTableCell_RendersMarkdownInsideRawTable()
    {
        var html = Render(
            """
            <table summary="">

            \t<tr>
            \t\t<td markdown="1">
            * [Guide](/guide/)
            * **Important**
            </td>
            \t</tr>
            </table>
            """.Replace("\\t", "\t", StringComparison.Ordinal));

        Assert.Contains("<table summary=\"\">", html);
        Assert.Contains("<td>", html);
        Assert.Contains("<ul>", html);
        Assert.Contains("<a href=\"/guide/\">Guide</a>", html);
        Assert.Contains("<strong>Important</strong>", html);
        Assert.DoesNotContain("markdown=\"1\"", html);
        Assert.DoesNotContain("* [Guide]", html);
        Assert.DoesNotContain("<pre><code>", html);
    }

    [Fact]
    public void Render_NestedLegacyMarkdownElements_RendersInnerTableBeforeOuterBlock()
    {
        var html = Render(
            """
            <blockquote markdown="1">
            Intro

            <table summary="">

            \t<tr>
            \t\t<td markdown="1">[Guide](/guide/)</td>
            \t</tr>
            </table>
            </blockquote>
            """.Replace("\\t", "\t", StringComparison.Ordinal));

        Assert.Contains("<blockquote>", html);
        Assert.Contains("<p>Intro</p>", html);
        Assert.Contains("<table summary=\"\">", html);
        Assert.Contains("<td><p><a href=\"/guide/\">Guide</a></p></td>", html);
        Assert.DoesNotContain("markdown=\"1\"", html);
        Assert.DoesNotContain("<pre><code>", html);
    }

    [Fact]
    public void Render_LegacyMarkdownContainer_PreservesHighlightedFencedCode()
    {
        var html = Render(
            """
            <div class="expand-panel" markdown="1" title="（古いコード（Windows Forms））">

            ```csharp
            if (left < right) return;
            ```

            </div>
            """);

        Assert.Contains(
            "<pre><code class=\"language-csharp\"><span class=\"roslyn-keyword-control\">if</span>",
            html);
        Assert.DoesNotContain(
            "&lt;span class=&quot;roslyn-keyword-control&quot;&gt;",
            html);
        Assert.DoesNotContain("```csharp", html);
    }

    [Fact]
    public void Render_StBasisPage_DoesNotEscapeHighlightedCode()
    {
        var contentRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\..\\content"));
        var (pages, urlMap) = PageLoader.Load(contentRoot);
        var page = pages.Single(
            candidate => candidate.RelativePath == "study/csharp/start/st_basis.md");

        var html = new MarkdigRenderer(contentRoot).Render(page, urlMap);

        Assert.DoesNotContain("&lt;span class=&quot;", html);
    }

    [Fact]
    public void RenderWithMetadata_ExtractsNestedOutlineAndKeywordAnchors()
    {
        var rendered = RenderWithMetadata(
            """
            # Article title

            ## <a id="sec-generated-title-1"></a> <a id="overview"></a>Overview

            ### <a id="details"></a>Details

            #### <a id="deep-detail"></a>Deep detail

            ##### Point

            ## Generated heading

            ## <a id="仮想メモリ"></a>仮想メモリ

            <strong class="term keyword" id="type">Type &amp; member</strong>
            <span id="alias" class="keyword">Alias</span>
            <strong id="type" class="keyword">Duplicate</strong>
            """);

        Assert.Equal(3, rendered.TableOfContents.Count);

        var overview = rendered.TableOfContents[0];
        Assert.Equal("#overview", overview.Url);
        Assert.Equal("Overview", overview.Title);

        var details = Assert.Single(overview.Children);
        Assert.Equal("#details", details.Url);
        var deepDetail = Assert.Single(details.Children);
        Assert.Equal("#deep-detail", deepDetail.Url);

        var generated = rendered.TableOfContents[1];
        Assert.Equal("#generated-heading", generated.Url);
        Assert.Equal("Generated heading", generated.Title);

        var unicodeAnchor = rendered.TableOfContents[2];
        Assert.Equal(
            "#%E4%BB%AE%E6%83%B3%E3%83%A1%E3%83%A2%E3%83%AA",
            unicodeAnchor.Url);
        Assert.Equal("仮想メモリ", unicodeAnchor.Title);
        Assert.DoesNotContain(
            rendered.TableOfContents.SelectMany(Flatten),
            item => item.Title == "Point");

        Assert.Equal(
            ["#type", "#alias"],
            rendered.Keywords.Select(keyword => keyword.Url));
        Assert.Equal(
            ["Type & member", "Alias"],
            rendered.Keywords.Select(keyword => keyword.Title));
    }

    [Fact]
    public void RenderWithMetadata_DuplicateLegacyHeadingId_UsesUniqueFallbackAnchor()
    {
        var rendered = RenderWithMetadata(
            """
            ## <a id="sec-generated-title-1"></a> <a id="shared"></a>First

            ## <a id="sec-generated-title-2"></a> <a id="shared"></a>Second
            """);

        Assert.Equal(
            [("#shared", "First"), ("#second", "Second")],
            rendered.TableOfContents
                .Select(item => (item.Url, item.Title))
                .ToArray());
        Assert.Contains("id=\"second\"", rendered.Html);
    }

    [Fact]
    public void RenderWithMetadata_UnanchoredRawHeading_GeneratesResolvableAnchor()
    {
        var rendered = RenderWithMetadata(
            """
            <span id="sec-generated-toc-1"></span>
            <h2>Raw heading</h2>
            """);

        var item = Assert.Single(rendered.TableOfContents);
        Assert.Equal("#sec-generated-toc-2", item.Url);
        Assert.Equal("Raw heading", item.Title);
        Assert.Contains(
            "<h2 id=\"sec-generated-toc-2\">Raw heading</h2>",
            rendered.Html);
    }

    [Fact]
    public void RenderWithMetadata_CollidingHeadingAndKeywordIds_UseExactTargets()
    {
        var rendered = RenderWithMetadata(
            """
            <strong id="before" class="keyword">Before</strong>

            ## <a id="before"></a>Later heading

            ## <a id="after"></a>Earlier heading

            <strong id="after" class="keyword">After</strong>
            """);

        Assert.Equal(
            [("#later-heading", "Later heading"), ("#after", "Earlier heading")],
            rendered.TableOfContents
                .Select(item => (item.Url, item.Title))
                .ToArray());
        Assert.Equal(
            [("#before", "Before"), ("#sec-generated-keyword-1", "After")],
            rendered.Keywords
                .Select(item => (item.Url, item.Title))
                .ToArray());
        Assert.Contains(
            "<span id=\"sec-generated-keyword-1\"></span>After",
            rendered.Html);
    }

    [Fact]
    public void Render_ObjectDataAndSourceParam_RewritesOnlyExistingLegacyAssets()
    {
        var html = Render(
            """
            <object data="/media/demo/player.xap" type="application/x-silverlight-2">
              <param name="source" value="/media/demo/application.xap">
              <param name="background" value="/media/demo/not-a-resource.png">
            </object>
            """,
            tempRoot =>
            {
                WriteAsset(tempRoot, "media/demo/player.xap");
                WriteAsset(tempRoot, "media/demo/application.xap");
            });

        Assert.Contains("data=\"/assets/media/demo/player.xap\"", html);
        Assert.Contains("name=\"source\" value=\"/assets/media/demo/application.xap\"", html);
        Assert.Contains("name=\"background\" value=\"/media/demo/not-a-resource.png\"", html);
        Assert.DoesNotContain("data=\"/media/demo/player.xap\"", html);
        Assert.DoesNotContain(
            "name=\"background\" value=\"/assets/media/demo/not-a-resource.png\"",
            html);
    }

    [Fact]
    public void Render_OriginalPageBreakMarkers_KeepsSinglePageWithAllAnchors()
    {
        var rendered = RenderWithMetadata(
            """
            # インターフェース

            ## <a id="abst"></a>概要

            最初のページの本文。

            <!-- original-page-break -->

            ## <a id="static-abstract"></a>インターフェイスの静的抽象メンバー

            旧 6 ページ目の本文。

            <!-- original-page-break -->

            ## <a id="last"></a>最後の節
            """);

        // The legacy pageBreak markers must not split the article: every anchor that used to
        // live on a separate ?p=N page stays reachable within the single generated page.
        Assert.Contains("id=\"abst\"", rendered.Html);
        Assert.Contains("id=\"static-abstract\"", rendered.Html);
        Assert.Contains("id=\"last\"", rendered.Html);
        Assert.Contains("旧 6 ページ目の本文。", rendered.Html);
        Assert.Equal(
            ["#abst", "#static-abstract", "#last"],
            rendered.TableOfContents.Select(item => item.Url).ToArray());
    }

    [Fact]
    public void Render_LegacyPageQueryInLinks_KeepsOnlyTheFragment()
    {
        var html = Render(
            """
            [静的抽象メンバー](oo_interface.md?p=6#static-abstract)

            <a href="oo_interface.md?p=6#static-abstract">静的抽象メンバー</a>
            """);

        Assert.DoesNotContain("?p=", html);
        Assert.Equal(
            2,
            html.Split("\"oo_interface.md#static-abstract\"").Length - 1);
    }

    private static string Render(string markdown, Action<string>? arrange = null)
        => RenderWithMetadata(markdown, arrange).Html;

    private static string ExtractRenderedCodeText(string html)
    {
        var codeTagStart = html.IndexOf("<code", StringComparison.Ordinal);
        Assert.True(codeTagStart >= 0);
        var contentStart = html.IndexOf('>', codeTagStart) + 1;
        var contentEnd = html.IndexOf("</code>", contentStart, StringComparison.Ordinal);
        Assert.True(contentStart > 0);
        Assert.True(contentEnd >= contentStart);

        return XElement.Parse(
            $"<root>{html[contentStart..contentEnd]}</root>",
            LoadOptions.PreserveWhitespace).Value;
    }

    private static RenderedContent RenderWithMetadata(
        string markdown,
        Action<string>? arrange = null)
    {
        using var tempDirectory = new TempDirectory();
        var contentRoot = Path.Combine(tempDirectory.Path, "content");
        Directory.CreateDirectory(Path.Combine(contentRoot, "study"));
        arrange?.Invoke(tempDirectory.Path);

        var page = new ContentPage
        {
            FrontMatter = new FrontMatter { Title = "Renderer fixture" },
            RelativePath = "study/page.md",
            MarkdownBody = markdown,
            CanonicalPath = "/study/page/",
            OutputPath = "study/page/index.html",
        };

        return new MarkdigRenderer(contentRoot).RenderWithMetadata(
            page,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
    }

    private static IEnumerable<TableOfContentsItem> Flatten(
        TableOfContentsItem item)
    {
        yield return item;
        foreach (var child in item.Children.SelectMany(Flatten))
        {
            yield return child;
        }
    }

    private static void WriteAsset(string tempRoot, string relativePath)
    {
        var path = Path.Combine(
            tempRoot,
            "assets",
            relativePath.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "fixture");
    }
}
