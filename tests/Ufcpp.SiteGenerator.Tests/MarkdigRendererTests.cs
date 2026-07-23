using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Rendering;

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
    [InlineData("cs", "public string Value = \"<tag>\";", "csharp", "keyword")]
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
            <div markdown="1">

            ```csharp
            if (left < right) return;
            ```

            </div>
            """);

        Assert.Contains(
            "<pre><code class=\"language-csharp\"><span class=\"keyword\">if</span>",
            html);
        Assert.DoesNotContain("&lt;span class=&quot;keyword&quot;&gt;", html);
        Assert.DoesNotContain("```csharp", html);
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

    private static string Render(string markdown, Action<string>? arrange = null)
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

        return new MarkdigRenderer(contentRoot).Render(
            page,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
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
