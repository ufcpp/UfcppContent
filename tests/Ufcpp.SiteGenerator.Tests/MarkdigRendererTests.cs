using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Loading;
using Ufcpp.SiteGenerator.Rendering;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Classification;

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
            "<span class=\"roslyn-record-class-name roslyn-type-name\">R</span>",
            html);
    }

    [Fact]
    public void Render_CSharpFence_ResolvesConsoleTemplateImplicitUsings()
    {
        var html = Render(
            """
            ```csharp
            Console.WriteLine("Hello");
            await Task.Delay(1);
            ```
            """);

        Assert.Contains(
            "<span class=\"roslyn-class-name roslyn-static-symbol roslyn-type-name\">Console</span>",
            html);
        Assert.Contains(
            "<span class=\"roslyn-class-name roslyn-type-name\">Task</span>",
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
            "<span class=\"roslyn-record-struct-name roslyn-type-name\">X</span>",
            html);
        Assert.Contains("<span class=\"roslyn-property-name\">B</span>", html);
        Assert.Contains(
            "<span class=\"roslyn-method-name\">TryGetValue</span>",
            html);
        Assert.Contains(
            "<span class=\"roslyn-parameter-name\">value</span>",
            html);
        Assert.Contains(
            "<span class=\"roslyn-interface-name roslyn-type-name\">IIncrementOperators</span>",
            html);
        Assert.Contains("<span class=\"roslyn-local-name\">i</span>", html);
        Assert.Contains("&lt;", html);
    }

    /// <summary>
    /// Every kind of type carries the shared <c>roslyn-type-name</c> class on top
    /// of its specific one, so the stylesheet can color types once and override
    /// only the kinds that differ.
    /// </summary>
    [Theory]
    [InlineData("class C { }", "roslyn-class-name roslyn-type-name", "C")]
    [InlineData("record R;", "roslyn-record-class-name roslyn-type-name", "R")]
    [InlineData("struct S { }", "roslyn-struct-name roslyn-type-name", "S")]
    [InlineData("record struct RS;", "roslyn-record-struct-name roslyn-type-name", "RS")]
    [InlineData("interface I { }", "roslyn-interface-name roslyn-type-name", "I")]
    [InlineData("delegate void D();", "roslyn-delegate-name roslyn-type-name", "D")]
    [InlineData("enum E { A }", "roslyn-enum-name roslyn-type-name", "E")]
    [InlineData("class C<TItem> { }", "roslyn-type-name roslyn-type-parameter-name", "TItem")]
    public void Render_CSharpFence_MarksEveryTypeKindAsAType(
        string code,
        string classes,
        string name)
    {
        var html = Render($"```csharp\n{code}\n```");

        Assert.Contains($"<span class=\"{classes}\">{name}</span>", html);
    }

    /// <summary>
    /// The shared class must not leak onto members and locals: they are colored
    /// by their own classes, and picking up the type color would be wrong.
    /// </summary>
    [Fact]
    public void Render_CSharpFence_LeavesNonTypesUnmarked()
    {
        var html = Render(
            """
            ```csharp
            class C
            {
                int _field;
                const int Constant = 1;
                int Property { get; set; }
                event Action? Event;
                void Method(int parameter) { int local = parameter; }
            }
            ```
            """);

        Assert.Contains("<span class=\"roslyn-field-name\">_field</span>", html);
        Assert.Contains(
            "<span class=\"roslyn-constant-name roslyn-static-symbol\">Constant</span>",
            html);
        Assert.Contains("<span class=\"roslyn-property-name\">Property</span>", html);
        Assert.Contains("<span class=\"roslyn-event-name\">Event</span>", html);
        Assert.Contains("<span class=\"roslyn-method-name\">Method</span>", html);
        Assert.Contains("<span class=\"roslyn-parameter-name\">parameter</span>", html);
        Assert.Contains("<span class=\"roslyn-local-name\">local</span>", html);
    }

    /// <summary>
    /// Roslyn exposes no list of the classifications that denote a type, so the
    /// highlighter carries its own. This checks that list against every
    /// classification Roslyn declares: when a compiler release adds a kind of
    /// type (C# 15's unions, for one), this fails and the new name has to be
    /// sorted into <see cref="RoslynCSharpHighlighter.TypeClassificationTypeNames"/>
    /// or into the non-type list below.
    /// </summary>
    [Fact]
    public void TypeClassifications_CoverEveryClassificationRoslynDeclares()
    {
        string[] nonTypeNames =
        [
            ClassificationTypeNames.ConstantName,
            ClassificationTypeNames.EnumMemberName,
            ClassificationTypeNames.EventName,
            ClassificationTypeNames.ExtensionMethodName,
            ClassificationTypeNames.FieldName,
            ClassificationTypeNames.LabelName,
            ClassificationTypeNames.LocalName,
            ClassificationTypeNames.MethodName,
            ClassificationTypeNames.NamespaceName,
            ClassificationTypeNames.ParameterName,
            ClassificationTypeNames.PropertyName,
            ClassificationTypeNames.XmlDocCommentAttributeName,
            ClassificationTypeNames.XmlDocCommentName,
            ClassificationTypeNames.XmlLiteralAttributeName,
            ClassificationTypeNames.XmlLiteralName,
        ];

        var declared = typeof(ClassificationTypeNames)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null)!)
            .Where(name => name.EndsWith("name", StringComparison.Ordinal))
            .ToArray();

        var classified = RoslynCSharpHighlighter.TypeClassificationTypeNames
            .Concat(nonTypeNames)
            .ToHashSet(StringComparer.Ordinal);

        Assert.NotEmpty(declared);
        Assert.Empty(declared.Where(name => !classified.Contains(name)));
        Assert.Empty(
            RoslynCSharpHighlighter.TypeClassificationTypeNames
                .Where(name => !declared.Contains(name)));
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
    public void Render_FencedCodeTitle_IsEscapedAndWrittenOnlyToPre()
    {
        const string Title = """A "quoted" <tag> & value""";
        var html = Render(
            """
            ```unknown-language {title='A "quoted" <tag> & value'}
            <unsafe>
            ```
            """);

        var pre = Assert.Single(ExtractRenderedPreElements(html));
        var code = Assert.Single(pre.Elements("code"));
        Assert.Equal(Title, pre.Attribute("title")?.Value);
        Assert.Null(code.Attribute("title"));
        Assert.Equal("<unsafe>", code.Value);
        Assert.Contains(
            "title=\"A &quot;quoted&quot; &lt;tag&gt; &amp; value\"",
            html);
        Assert.DoesNotContain("<tag>", html);
        Assert.DoesNotContain("<unsafe>", html);
    }

    [Fact]
    public void Render_FencedCodeMetadata_DecodesCanonicalEntitiesOnce()
    {
        var html = Render(
            """
            ```text {title="A &quot;quoted&quot; &#96;tick&#96; &amp; &amp;lt;" highlight-text="&amp;lt;"}
            A &lt;
            ```
            """);

        var pre = Assert.Single(ExtractRenderedPreElements(html));
        Assert.Equal("""A "quoted" `tick` & &lt;""", pre.Attribute("title")?.Value);
        Assert.Equal(
            "&lt;",
            Assert.Single(pre.Descendants("mark")).Value);
    }

    [Fact]
    public void Render_FencedCodeWithoutTitle_OmitsTitleAttribute()
    {
        var html = Render(
            """
            ```text
            plain
            ```
            """);

        Assert.Null(
            Assert.Single(ExtractRenderedPreElements(html)).Attribute("title"));
    }

    [Fact]
    public void Render_FencedCodeMetadata_RejectsPropertiesOutsideAllowlist()
    {
        Assert.Throws<InvalidDataException>(
            () => Render(
                """
                ```text {onclick="alert(1)"}
                plain
                ```
                """));
    }

    [Theory]
    [InlineData("""{#code-block}""")]
    [InlineData("""{.source}""")]
    public void Render_FencedCodeMetadata_RejectsGenericIdsOrClasses(string metadata)
    {
        Assert.Throws<InvalidDataException>(
            () => Render($"```text {metadata}\nplain\n```"));
    }

    [Fact]
    public void Render_MathBlock_AllowsExtensionGeneratedGenericClass()
    {
        var html = Render(
            """
            $$
            x + y
            $$
            """);

        Assert.Contains("x + y", html);
    }

    [Fact]
    public void Render_FencedCodeTitle_RejectsRepeatedAttribute()
    {
        Assert.Throws<InvalidDataException>(
            () => Render(
                """
                ```text {title="first" title="second"}
                plain
                ```
                """));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Render_FencedCodeTitle_RejectsEmptyValue(string title)
    {
        var markdown = $"```text {{title=\"{title}\"}}\nplain\n```";

        Assert.Throws<InvalidDataException>(() => Render(markdown));
    }

    [Theory]
    [InlineData("\t")]
    [InlineData("\u007F")]
    public void Render_FencedCodeTitle_RejectsControlCharacters(string title)
    {
        var markdown = $"```text {{title=\"before{title}after\"}}\nplain\n```";

        Assert.Throws<InvalidDataException>(() => Render(markdown));
    }

    [Fact]
    public void Render_FencedCode_HighlightsLinesAndInclusiveRanges()
    {
        var html = Render(
            """
            ```csharp {highlight-lines="2,4-5"}
            int zero = 0;
            int one = 1;
            int two = 2;
            int three = 3;
            int four = 4;
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        Assert.Equal(
            [
                "int one = 1;",
                "int three = 3;\nint four = 4;",
            ],
            GetHighlightedRegions(code));
        Assert.NotEmpty(code.Descendants("span"));
        Assert.All(
            code.Descendants("mark"),
            mark => Assert.Equal("code-highlight", mark.Attribute("class")?.Value));
    }

    [Fact]
    public void Render_HighlightText_HighlightsEveryOrdinalOccurrenceInSupportedCode()
    {
        var html = Render(
            """
            ```csharp {highlight-text="Value"}
            var Value = "Value";
            var value = 0;
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        Assert.Equal("ValueValue", ExtractHighlightedCodeText(code));
        Assert.Contains(
            code.Descendants("mark"),
            mark => mark.Descendants("span").Any());
    }

    [Fact]
    public void Render_HighlightText_PreservesColorCodeTokenSpans()
    {
        var html = Render(
            """
            ```xml {highlight-text="value"}
            <root attr="value" />
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        Assert.Equal("<root attr=\"value\" />\n", code.Value);
        Assert.Equal("value", ExtractHighlightedCodeText(code));
        Assert.Contains(
            code.Descendants("mark"),
            mark => mark.Descendants("span").Any());
        Assert.Contains(code.Descendants("span"), span => span.HasAttributes);
    }

    [Fact]
    public void Render_HighlightText_HighlightsPartialPlainCode()
    {
        var html = Render(
            """
            ```console {highlight-text="/target:winexe"}
            csc /target:winexe Program.cs
            ```
            """);

        var pre = Assert.Single(ExtractRenderedPreElements(html));
        var code = Assert.Single(pre.Elements("code"));
        var mark = Assert.Single(code.Descendants("mark"));
        Assert.Equal("console", pre.Attribute("class")?.Value);
        Assert.Equal("/target:winexe", mark.Value);
        Assert.Empty(code.Descendants("span"));
        Assert.DoesNotContain("highlight-text", html);
    }

    [Fact]
    public void Render_OverlappingHighlightSelections_AreMerged()
    {
        var html = Render(
            """
            ```text {highlight-lines="1" highlight-text="aa"}
            aaa
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        var mark = Assert.Single(code.Descendants("mark"));
        Assert.Equal("aaa", mark.Value);
        Assert.Empty(mark.Descendants("mark"));
    }

    [Fact]
    public void Render_HighlightText_EscapesCodeAndDoesNotEmitMetadata()
    {
        var html = Render(
            """
            ```csharp {highlight-text="<img src=x>"}
            var text = "<img src=x>";
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        Assert.Equal("var text = \"<img src=x>\";", code.Value);
        Assert.Equal("<img src=x>", ExtractHighlightedCodeText(code));
        Assert.DoesNotContain("<img src=x>", html);
        Assert.DoesNotContain("highlight-text", html);
    }

    [Fact]
    public void Render_UnsupportedLanguage_HighlightsEscapedFallback()
    {
        var html = Render(
            """
            ```unknown-language {highlight-lines="1"}
            <tag attr="value">& text</tag>
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        Assert.Equal("language-unknown-language", code.Attribute("class")?.Value);
        Assert.Equal("<tag attr=\"value\">& text</tag>", code.Value);
        Assert.Equal(code.Value, ExtractHighlightedCodeText(code));
        Assert.Empty(code.Descendants("span"));
        Assert.DoesNotContain("<tag attr=", html);
    }

    [Fact]
    public void Render_HighlightRanges_HighlightsMultilinePartialSelection()
    {
        const string Code = "alpha beta\nsecond line";
        var html = Render(
            $"```text {{highlight-ranges=\"{RangeMetadata(Code, "1:7-2:7")}\"}}\n"
            + $"{Code}\n```");

        var code = ExtractRenderedCodeElement(html);
        Assert.Equal(Code, code.Value);
        Assert.Equal(["beta\nsecond"], GetHighlightedRegions(code));
        Assert.DoesNotContain("highlight-ranges", html);
    }

    [Fact]
    public void Render_HighlightRanges_UsesUnicodeScalarColumns()
    {
        const string Code = "a😀b";
        var html = Render(
            $"```text {{highlight-ranges=\"{RangeMetadata(Code, "1:2-1:3")}\"}}\n"
            + $"{Code}\n```");

        Assert.Equal(
            ["😀"],
            GetHighlightedRegions(ExtractRenderedCodeElement(html)));
    }

    [Fact]
    public void Render_HighlightRanges_SplitsAcrossCSharpSyntaxSpans()
    {
        const string Code = "if (value == \"x\") return;";
        var html = Render(
            $"```csharp {{highlight-ranges=\"{RangeMetadata(Code, "1:1-1:15")}\"}}\n"
            + $"{Code}\n```");

        var mark = Assert.Single(ExtractRenderedCodeElement(html).Descendants("mark"));
        Assert.Equal("if (value == \"", mark.Value);
        Assert.True(mark.Descendants("span").Count() > 1);
    }

    [Fact]
    public void Render_HighlightRanges_NormalizesCrLfForFingerprintAndCoordinates()
    {
        const string Code = "one\ntwo";
        var markdown =
            $"```text {{highlight-ranges=\"{RangeMetadata(Code, "2:1-2:4")}\"}}\r\n"
            + "one\r\ntwo\r\n```";

        Assert.Equal(
            ["two"],
            GetHighlightedRegions(ExtractRenderedCodeElement(Render(markdown))));
    }

    [Fact]
    public void Render_HighlightRanges_FingerprintUsesExactMarkdigCodeValue()
    {
        const string WithoutFinalBlankLine =
            "sha256:8f434346648f6b96df89dda901c5176b10a6d83961dd3c1ac88b59b2dc327aa4;1:1-1:3";
        const string WithFinalBlankLine =
            "sha256:98ea6e4f216f2fb4b69fff9b3a44842c38686ca685f3f55dc48c5d3fb1107be4;1:1-1:3";
        const string Utf8WithoutPreamble =
            "sha256:77710aedc74ecfa33685e33a6c7df5cc83004da1bdcef7fb280f5c2b2e97e0a5;1:1-1:4";

        var ordinary = Render(
            $"```text {{highlight-ranges=\"{WithoutFinalBlankLine}\"}}\r\n"
            + "hi\r\n```");
        var finalBlankLine = Render(
            $"```text {{highlight-ranges=\"{WithFinalBlankLine}\"}}\r\n"
            + "hi\r\n\r\n```");
        var unicode = Render(
            $"```text {{highlight-ranges=\"{Utf8WithoutPreamble}\"}}\n"
            + "日本語\n```");

        Assert.Equal("hi", ExtractRenderedCodeElement(ordinary).Value);
        Assert.Equal("hi\n", ExtractRenderedCodeElement(finalBlankLine).Value);
        Assert.Equal("日本語", ExtractRenderedCodeElement(unicode).Value);
    }

    [Fact]
    public void Render_HighlightRanges_PreservesEncodedEntitySpelling()
    {
        const string Code = "value &lt; limit";
        var html = Render(
            $"```text {{highlight-ranges=\"{RangeMetadata(Code, "1:7-1:11")}\"}}\n"
            + $"{Code}\n```");

        var rendered = ExtractRenderedCodeElement(html);
        Assert.Equal(Code, rendered.Value);
        Assert.Equal(["&lt;"], GetHighlightedRegions(rendered));
    }

    [Fact]
    public void Render_HighlightRanges_RendersIntentionalTrailingSpaces()
    {
        const string Code = "value    ";
        var html = Render(
            $"```text {{highlight-ranges=\"{RangeMetadata(Code, "1:6-1:10")}\"}}\n"
            + $"{Code}\n```");

        Assert.Equal(
            ["    "],
            GetHighlightedRegions(ExtractRenderedCodeElement(html)));
    }

    [Fact]
    public void Render_HighlightRanges_PreservesColorCodeSpansAtIntersections()
    {
        const string Code = """<root attr="value" />""";
        var html = Render(
            $"```xml {{highlight-ranges=\"{RangeMetadata(Code, "1:7-1:19")}\"}}\n"
            + $"{Code}\n```");

        var mark = Assert.Single(ExtractRenderedCodeElement(html).Descendants("mark"));
        Assert.Equal("attr=\"value\"", mark.Value);
        Assert.True(mark.Descendants("span").Count() > 1);
    }

    [Fact]
    public void Render_HighlightLines_PreservesEncodedQuotesInColorCodeTextNodes()
    {
        var html = Render(
            """
            ```html {highlight-lines="1"}
            <%@ Register TagPrefix="local" Src="~/ShowXml.ascx" %>
            ```
            """);

        Assert.Contains("Src=&quot;~/ShowXml.ascx&quot;", html);
        Assert.DoesNotContain("Src=\"~/ShowXml.ascx\"", html);
        Assert.Equal(
            """<%@ Register TagPrefix="local" Src="~/ShowXml.ascx" %>""",
            ExtractRenderedCodeElement(html).Value.TrimEnd('\r', '\n'));
    }

    [Fact]
    public void Render_HighlightRanges_ComposesWithWholeLineSelection()
    {
        const string Code = "whole\npartial value";
        var html = Render(
            $"```text {{highlight-lines=\"1\" "
            + $"highlight-ranges=\"{RangeMetadata(Code, "2:9-2:14")}\"}}\n"
            + $"{Code}\n```");

        Assert.Equal(
            ["whole", "value"],
            GetHighlightedRegions(ExtractRenderedCodeElement(html)));
    }

    [Fact]
    public void Render_HighlightTextAndRanges_RemainBackwardCompatible()
    {
        const string Code = "alpha beta";
        var html = Render(
            $"```text {{highlight-text=\"alpha\" "
            + $"highlight-ranges=\"{RangeMetadata(Code, "1:7-1:11")}\"}}\n"
            + $"{Code}\n```");

        Assert.Equal(
            ["alpha", "beta"],
            GetHighlightedRegions(ExtractRenderedCodeElement(html)));
    }

    [Fact]
    public void Render_TypedAnnotations_UseDeterministicWrapperOrderInsideSyntax()
    {
        const string Code = "int value = 0;";
        var html = Render(
            $"```csharp {{highlight-lines=\"1\" "
            + $"error-ranges=\"{RangeMetadata(Code, "1:1-1:10")}\" "
            + "warning-text=\"value\"}\n"
            + $"{Code}\n```");

        var code = ExtractRenderedCodeElement(html);
        var warning = Assert.Single(
            code.Descendants("span").Where(
                element => element.Attribute("class")?.Value == "warning"));
        var error = Assert.IsType<XElement>(warning.Parent);
        var highlight = Assert.IsType<XElement>(error.Parent);

        Assert.Equal("value", warning.Value);
        Assert.Equal("error", error.Attribute("class")?.Value);
        Assert.Equal("mark", highlight.Name.LocalName);
        Assert.Equal("code-highlight", highlight.Attribute("class")?.Value);
        Assert.Contains(
            warning.Descendants("span"),
            element => element.Attribute("class")?.Value.Contains(
                "roslyn-local-name",
                StringComparison.Ordinal) == true);
        Assert.Equal(Code, code.Value.TrimEnd('\r', '\n'));
        Assert.DoesNotContain("error-ranges", html);
        Assert.DoesNotContain("warning-text", html);
    }

    [Fact]
    public void Render_TypedAnnotations_RenderLineAndTextChannelsIndependently()
    {
        var html = Render(
            """
            ```text {error-lines="1" warning-text="warn"}
            bad
            good warn
            ```
            """);

        var code = ExtractRenderedCodeElement(html);
        var error = Assert.Single(
            code.Descendants("span").Where(
                element => element.Attribute("class")?.Value == "error"));
        var warning = Assert.Single(
            code.Descendants("span").Where(
                element => element.Attribute("class")?.Value == "warning"));

        Assert.Equal("bad", error.Value);
        Assert.Equal("warn", warning.Value);
        Assert.Empty(error.Descendants("span"));
        Assert.Empty(warning.Descendants("span"));
        Assert.Equal("bad\ngood warn", code.Value);
    }

    [Fact]
    public void Render_TypedAnnotations_SplitPartialOverlapsWithoutCrossingTags()
    {
        const string Code = "abcdef";
        var html = Render(
            $"```text {{error-ranges=\"{RangeMetadata(Code, "1:1-1:5")}\" "
            + $"warning-ranges=\"{RangeMetadata(Code, "1:3-1:7")}\"}}\n"
            + $"{Code}\n```");

        var code = ExtractRenderedCodeElement(html);
        var errors = code.Descendants("span")
            .Where(element => element.Attribute("class")?.Value == "error")
            .ToArray();
        var warnings = code.Descendants("span")
            .Where(element => element.Attribute("class")?.Value == "warning")
            .ToArray();

        Assert.Equal(["ab", "cd"], errors.Select(static element => element.Value));
        Assert.Equal(["cd", "ef"], warnings.Select(static element => element.Value));
        Assert.Equal("error", warnings[0].Parent?.Attribute("class")?.Value);
        Assert.Equal(Code, code.Value);
    }

    [Fact]
    public void Render_TypedAnnotations_RetainAllKindsAtEqualBoundaries()
    {
        const string Code = "value";
        var range = RangeMetadata(Code, "1:1-1:6");
        var html = Render(
            $"```text {{highlight-ranges=\"{range}\" error-ranges=\"{range}\" "
            + $"warning-ranges=\"{range}\"}}\n{Code}\n```");

        var code = ExtractRenderedCodeElement(html);
        var mark = Assert.Single(code.Elements("mark"));
        var error = Assert.Single(mark.Elements("span"));
        var warning = Assert.Single(error.Elements("span"));

        Assert.Equal("code-highlight", mark.Attribute("class")?.Value);
        Assert.Equal("error", error.Attribute("class")?.Value);
        Assert.Equal("warning", warning.Attribute("class")?.Value);
        Assert.Equal(Code, warning.Value);
    }

    [Theory]
    [InlineData("error-text")]
    [InlineData("warning-text")]
    public void Render_TypedAnnotationText_RejectsAmbiguousLiteral(string attribute)
    {
        Assert.Throws<InvalidDataException>(
            () => Render($"```text {{{attribute}=\"token\"}}\ntoken + token\n```"));
    }

    [Theory]
    [InlineData("error")]
    [InlineData("warning")]
    public void Render_TypedAnnotationTextAndRanges_AreMutuallyExclusive(string kind)
    {
        const string Code = "value";
        var range = RangeMetadata(Code, "1:1-1:6");

        Assert.Throws<InvalidDataException>(
            () => Render(
                $"```text {{{kind}-text=\"value\" {kind}-ranges=\"{range}\"}}\n"
                + $"{Code}\n```"));
    }

    [Theory]
    [InlineData("error-ranges")]
    [InlineData("warning-ranges")]
    public void Render_TypedRanges_ReportTheirOwnAttributeOnStaleFingerprint(
        string attribute)
    {
        const string Code = "value";
        var valid = RangeMetadata(Code, "1:1-1:6");
        var stale = new string('0', 64) + valid[64..];

        var exception = Assert.Throws<InvalidDataException>(
            () => Render($"```text {{{attribute}=\"{stale}\"}}\n{Code}\n```"));

        Assert.Contains(attribute, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Render_WarningRanges_UseUnicodeScalarsAcrossCrLf()
    {
        const string CanonicalCode = "a😀\nsecond";
        var markdown =
            $"```text {{warning-ranges=\"{RangeMetadata(CanonicalCode, "1:2-2:4")}\"}}\r\n"
            + "a😀\r\nsecond\r\n```";

        var warning = Assert.Single(
            ExtractRenderedCodeElement(Render(markdown))
                .Descendants("span")
                .Where(element => element.Attribute("class")?.Value == "warning"));

        Assert.Equal("😀\nsec", warning.Value);
    }

    [Fact]
    public void Render_TypedAnnotations_PreserveColorCodeSpansAtIntersections()
    {
        const string Code = """<root attr="value" />""";
        var html = Render(
            $"```xml {{error-ranges=\"{RangeMetadata(Code, "1:7-1:19")}\" "
            + "warning-text=\"value\"}\n"
            + $"{Code}\n```");

        var code = ExtractRenderedCodeElement(html);
        var warning = Assert.Single(
            code.Descendants("span").Where(
                element => element.Attribute("class")?.Value == "warning"));

        Assert.Equal("error", warning.Parent?.Attribute("class")?.Value);
        Assert.Equal("value", warning.Value);
        Assert.Contains(warning.Descendants("span"), static span => span.HasAttributes);
        Assert.Equal(Code, code.Value.TrimEnd('\r', '\n'));
    }

    [Theory]
    [InlineData("console")]
    [InlineData("unknown-language")]
    public void Render_TypedAnnotations_WorkOnEscapedPlainCode(string language)
    {
        var html = Render(
            $"```{language} {{error-text=\"<tag>\"}}\n"
            + "<tag> & value\n```");

        var code = ExtractRenderedCodeElement(html);
        var error = Assert.Single(
            code.Descendants("span").Where(
                element => element.Attribute("class")?.Value == "error"));

        Assert.Equal("<tag>", error.Value);
        Assert.DoesNotContain("<tag>", html);
        Assert.Empty(error.Descendants("span"));
    }

    [Fact]
    public void Render_SameKindSelectionsMergeWithoutNestedWrappers()
    {
        var html = Render(
            """
            ```text {error-lines="1" error-text="alpha"}
            alpha beta
            ```
            """);

        var error = Assert.Single(
            ExtractRenderedCodeElement(html)
                .Descendants("span")
                .Where(element => element.Attribute("class")?.Value == "error"));

        Assert.Equal("alpha beta", error.Value);
        Assert.Empty(
            error.Descendants("span").Where(
                element => element.Attribute("class")?.Value == "error"));
    }

    [Fact]
    public void Render_TypedText_DecodesEntityMetadataOnce()
    {
        var html = Render(
            """
            ```text {warning-text="&amp;lt;"}
            `&lt;`
            ```
            """);

        var warning = Assert.Single(
            ExtractRenderedCodeElement(html)
                .Descendants("span")
                .Where(element => element.Attribute("class")?.Value == "warning"));

        Assert.Equal("&lt;", warning.Value);
    }

    [Fact]
    public void Render_TypedRanges_RejectUnpairedSurrogate()
    {
        const string Code = "a\uD800b";
        var markdown =
            $"```text {{error-ranges=\"{RangeMetadata(Code, "1:1-1:2")}\"}}\n"
            + $"{Code}\n```";

        var exception = Assert.Throws<InvalidDataException>(() => Render(markdown));

        Assert.Contains("error-ranges", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Render_HighlightRanges_RejectsRepeatedAttribute()
    {
        const string Code = "value";
        var value = RangeMetadata(Code, "1:1-1:6");

        Assert.Throws<InvalidDataException>(
            () => Render(
                $"```text {{highlight-ranges=\"{value}\" "
                + $"highlight-ranges=\"{value}\"}}\n{Code}\n```"));
    }

    [Theory]
    [InlineData("1:1-1:1")]
    [InlineData("1:1-1:3,1:2-1:4")]
    [InlineData("1:1-1:2,1:2-1:3")]
    [InlineData("01:1-1:2")]
    [InlineData("1:1-3:1")]
    [InlineData("2:4-2:5")]
    public void Render_HighlightRanges_RejectsInvalidOrNonCanonicalRanges(
        string ranges)
    {
        const string Code = "one\ntwo";
        var markdown =
            $"```text {{highlight-ranges=\"{RangeMetadata(Code, ranges)}\"}}\n"
            + $"{Code}\n```";

        Assert.Throws<InvalidDataException>(() => Render(markdown));
    }

    [Fact]
    public void Render_HighlightRanges_RejectsStaleOrNonCanonicalFingerprint()
    {
        const string Code = "value";
        var valid = RangeMetadata(Code, "1:1-1:6");
        var stale = new string('0', 64) + valid[64..];
        var uppercase = valid.ToUpperInvariant();

        Assert.Throws<InvalidDataException>(
            () => Render($"```text {{highlight-ranges=\"{stale}\"}}\n{Code}\n```"));
        Assert.Throws<InvalidDataException>(
            () => Render($"```text {{highlight-ranges=\"{uppercase}\"}}\n{Code}\n```"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("1-")]
    [InlineData("2-1")]
    [InlineData("1,,1")]
    public void Render_HighlightLines_RejectsMalformedSyntax(string value)
    {
        var markdown = $"```text {{highlight-lines=\"{value}\"}}\nonly\n```";

        Assert.Throws<InvalidDataException>(() => Render(markdown));
    }

    [Theory]
    [InlineData("""{highlight-lines=}""")]
    [InlineData("""{highlight-text=}""")]
    [InlineData("""{title=}""")]
    [InlineData("""{highlight-lines="1" """)]
    [InlineData("""highlight-text="only" """)]
    [InlineData("""title="only" """)]
    public void Render_FencedCodeMetadata_RejectsMalformedAttribute(string metadata)
    {
        var markdown = $"```text {metadata}\nonly\n```";

        Assert.Throws<InvalidDataException>(() => Render(markdown));
    }

    [Theory]
    [InlineData("""text{highlight-lines=}""")]
    [InlineData("""text{highlight-text="only" """)]
    public void Render_FencedCodeMetadata_RejectsMalformedAttributeAttachedToLanguage(
        string info)
    {
        var markdown = $"```{info}\nonly\n```";

        Assert.Throws<InvalidDataException>(() => Render(markdown));
    }

    [Fact]
    public void Render_HighlightLines_RejectsOutOfRangeLine()
    {
        Assert.Throws<InvalidDataException>(
            () => Render(
                """
                ```text {highlight-lines="2"}
                only
                ```
                """));
    }

    [Fact]
    public void Render_HighlightText_RejectsMissingLiteral()
    {
        Assert.Throws<InvalidDataException>(
            () => Render(
                """
                ```text {highlight-text="missing"}
                present
                ```
                """));
    }

    [Fact]
    public void Render_HighlightText_RejectsEmptyLiteral()
    {
        Assert.Throws<InvalidDataException>(
            () => Render(
                """
                ```text {highlight-text=""}
                present
                ```
                """));
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
    public void Render_RawTable_PreservesTitleAndPermanentHighlightMarkup()
    {
        const string RawTable =
            "<table><tr><td><pre title=\"sample\"><code>"
            + "alpha <mark class=\"code-highlight\">&lt;</mark> beta"
            + "</code></pre></td></tr></table>";

        var html = Render(RawTable);

        var pre = Assert.Single(ExtractRenderedPreElements(html));
        var code = Assert.Single(pre.Elements("code"));
        var mark = Assert.Single(code.Elements("mark"));
        Assert.Equal("sample", pre.Attribute("title")?.Value);
        Assert.Equal("code-highlight", mark.Attribute("class")?.Value);
        Assert.Equal("alpha < beta", code.Value);
        Assert.Contains(RawTable, html);
    }

    [Fact]
    public void Render_RawTable_PreservesTypedAnnotationNesting()
    {
        const string RawTable =
            "<table><tr><td><pre><code>"
            + "<mark class=\"code-highlight\"><span class=\"error\">"
            + "<span class=\"warning\">value</span></span></mark>"
            + "</code></pre></td></tr></table>";

        var html = Render(RawTable);

        var code = Assert.Single(ExtractRenderedPreElements(html)).Element("code");
        var mark = Assert.Single(Assert.IsType<XElement>(code).Elements("mark"));
        var error = Assert.Single(mark.Elements("span"));
        var warning = Assert.Single(error.Elements("span"));
        Assert.Equal("code-highlight", mark.Attribute("class")?.Value);
        Assert.Equal("error", error.Attribute("class")?.Value);
        Assert.Equal("warning", warning.Attribute("class")?.Value);
        Assert.Equal("value", warning.Value);
        Assert.Contains(RawTable, html);
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
        Assert.Contains("<a href=\"../../guide/\">Guide</a>", html);
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
        Assert.Contains("<td><p><a href=\"../../guide/\">Guide</a></p></td>", html);
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
    public void Render_ExpandPanelPair_BecomesNativeDisclosure()
    {
        var html = Render(
            """
            <span class="expand-button">（古いバージョンの例）</span>

            <div class="expand-panel" markdown="1">

            Legacy *body*.

            ```csharp
            if (left < right) return;
            ```

            </div>
            """);

        Assert.Contains("<details class=\"expand-panel\">", html);
        Assert.Contains(
            "<summary class=\"expand-button\">（古いバージョンの例）</summary>",
            html);
        Assert.Contains("<div class=\"expand-panel-body\">", html);
        Assert.Contains("</details>", html);
        Assert.Contains("<em>body</em>", html);
        Assert.Contains(
            "<pre><code class=\"language-csharp\"><span class=\"roslyn-keyword-control\">if</span>",
            html);
        Assert.DoesNotContain("class=\"expand-button\">（", html.Replace(
            "<summary class=\"expand-button\">（古いバージョンの例）</summary>",
            string.Empty));
        Assert.DoesNotContain("markdown=\"1\"", html);
    }

    [Fact]
    public void Render_ExpandPanelWithoutButton_IsLeftAlone()
    {
        var html = Render(
            """
            <div class="expand-panel" markdown="1">

            Body only.

            </div>
            """);

        Assert.DoesNotContain("<details", html);
        Assert.Contains("<div class=\"expand-panel\">", html);
    }

    [Fact]
    public void Render_TabContainer_GainsRadiosAndLabels()
    {
        var html = Render(
            """
            <div class="tab-container">
            <ul>
            	<li>C#</li>
            	<li>VB</li>
            </ul>
            <div>

            C# panel.

            </div>
            <div>

            VB panel.

            </div>
            </div>
            """);

        Assert.Contains(
            "<input type=\"radio\" name=\"ufcpp-tab-1\" id=\"ufcpp-tab-1-1\" checked>",
            html);
        Assert.Contains(
            "<input type=\"radio\" name=\"ufcpp-tab-1\" id=\"ufcpp-tab-1-2\">",
            html);
        Assert.Contains("<li><label for=\"ufcpp-tab-1-1\">C#</label></li>", html);
        Assert.Contains("<li><label for=\"ufcpp-tab-1-2\">VB</label></li>", html);

        // Exactly one tab starts selected.
        Assert.Single(Regex.Matches(html, " checked>"));

        // The radios must precede the <ul> so `:checked ~` can reach both the
        // tab strip and the panels.
        Assert.True(
            html.IndexOf("ufcpp-tab-1-2\">", StringComparison.Ordinal)
                < html.IndexOf("<ul>", StringComparison.Ordinal));
    }

    [Fact]
    public void Render_MultipleTabContainers_GetDistinctNames()
    {
        var html = Render(
            """
            <div class="tab-container">
            <ul>
            	<li>C#</li>
            </ul>
            <div>

            One.

            </div>
            </div>

            <div class="tab-container">
            <ul>
            	<li>VB</li>
            </ul>
            <div>

            Two.

            </div>
            </div>
            """);

        Assert.Contains("name=\"ufcpp-tab-1\" id=\"ufcpp-tab-1-1\"", html);
        Assert.Contains("name=\"ufcpp-tab-2\" id=\"ufcpp-tab-2-1\"", html);
    }

    [Fact]
    public void Render_TabContainerBeyondStyledLimit_IsLeftAlone()
    {
        var tabs = string.Join(
            "\n",
            Enumerable
                .Range(1, LegacyControlRewriter.MaxSwitchableTabs + 1)
                .Select(index => $"\t<li>Lang{index}</li>"));
        var panels = string.Join(
            "\n",
            Enumerable
                .Range(1, LegacyControlRewriter.MaxSwitchableTabs + 1)
                .Select(index => $"<div>\n\nPanel {index}.\n\n</div>"));

        var html = Render(
            $"""
            <div class="tab-container">
            <ul>
            {tabs}
            </ul>
            {panels}
            </div>
            """);

        Assert.DoesNotContain("<input type=\"radio\"", html);
        Assert.DoesNotContain("<label", html);
    }

    [Fact]
    public void Render_LegacyControlMarkupInsideCodeSample_IsNotRewritten()
    {
        var html = Render(
            """
            ```html
            <span class="expand-button">Label</span>
            <div class="expand-panel">Body</div>
            <div class="tab-container"><ul><li>C#</li></ul><div>x</div></div>
            ```
            """);

        Assert.DoesNotContain("<details", html);
        Assert.DoesNotContain("<input type=\"radio\"", html);
        Assert.Contains("expand-button", html);
    }

    [Fact]
    public void Render_TabContainer_AvoidsCollidingWithExistingIds()
    {
        var html = Render(
            """
            <a id="ufcpp-tab-1-1"></a>

            <div class="tab-container">
            <ul>
            	<li>C#</li>
            </ul>
            <div>

            Panel.

            </div>
            </div>
            """);

        Assert.Contains("name=\"ufcpp-tab-2\" id=\"ufcpp-tab-2-1\"", html);
        Assert.DoesNotContain("name=\"ufcpp-tab-1\"", html);
    }

    [Fact]
    public void Render_StBasisPage_DoesNotEscapeHighlightedCode()
    {
        var contentRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "content"));
        var (pages, urlMap) = PageLoader.Load(contentRoot);
        var page = pages.Single(
            candidate => candidate.RelativePath == "study/csharp/start/st_basis.md");

        var html = new MarkdigRenderer(contentRoot).Render(page, urlMap);

        Assert.DoesNotContain("&lt;span class=&quot;", html);
    }

    [Fact]
    public void Render_LibFormsPage_RestoresAllEditorialCodeHighlights()
    {
        var contentRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "content"));
        var (pages, urlMap) = PageLoader.Load(contentRoot);
        var page = pages.Single(
            candidate => candidate.RelativePath == "study/csharp/lib/lib_forms.md");

        var html = new MarkdigRenderer(contentRoot).Render(page, urlMap);
        var codeBlocks = ExtractRenderedPreElements(html);
        Assert.Equal(
            [
                "最小の GUI アプリケーション",
                "target:winexe",
                "幅・高さとタイトル文字を設定",
                "Form をサブクラス化",
                "Button",
                "Form に Button を追加",
                "Click イベントハンドラを追加",
            ],
            codeBlocks.Select(block => block.Attribute("title")?.Value));

        var highlightedBlocks = codeBlocks
            .Where(block => block.Descendants("mark").Any())
            .ToArray();

        Assert.Equal(3, highlightedBlocks.Length);
        Assert.Equal(
            [
                "target:winexe",
                "Form に Button を追加",
                "Click イベントハンドラを追加",
            ],
            highlightedBlocks.Select(block => block.Attribute("title")?.Value));

        var console = highlightedBlocks.Single(
            block => block.Attribute("title")?.Value == "target:winexe");
        Assert.Equal(
            "/target:winexe",
            ExtractHighlightedCodeText(Assert.Single(console.Elements("code"))));

        var addControl = highlightedBlocks.Single(
            block => block.Attribute("title")?.Value == "Form に Button を追加");
        Assert.Equal(
            ["this.Controls.Add(this.button1);"],
            GetHighlightedRegions(Assert.Single(addControl.Elements("code"))));

        var handler = highlightedBlocks.Single(
            block => block.Attribute("title")?.Value == "Click イベントハンドラを追加");
        Assert.Equal(
            [
                "this.button1.Click += new EventHandler(this.Button1_Click);",
                "  void Button1_Click(object sender, EventArgs e)\n"
                    + "  {\n"
                    + "    this.count++;\n"
                    + "    this.button1.Text = this.count.ToString();\n"
                        + "  }",
            ],
            GetHighlightedRegions(Assert.Single(handler.Elements("code"))));
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

        Assert.Contains("data=\"../../assets/media/demo/player.xap\"", html);
        Assert.Contains(
            "name=\"source\" value=\"../../assets/media/demo/application.xap\"",
            html);
        Assert.Contains("name=\"background\" value=\"/media/demo/not-a-resource.png\"", html);
        Assert.DoesNotContain("data=\"../../media/demo/player.xap\"", html);
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
        => ExtractRenderedCodeElement(html).Value;

    private static XElement ExtractRenderedCodeElement(string html)
    {
        var match = Regex.Match(
            html,
            @"<code(?:\s[^>]*)?>.*?</code>",
            RegexOptions.Singleline);
        Assert.True(match.Success);

        return XElement.Parse(match.Value, LoadOptions.PreserveWhitespace);
    }

    private static IReadOnlyList<XElement> ExtractRenderedCodeElements(string html) =>
        Regex.Matches(
                html,
                @"<code(?:\s[^>]*)?>.*?</code>",
                RegexOptions.Singleline)
            .Cast<Match>()
            .Select(
                match => XElement.Parse(
                    match.Value,
                    LoadOptions.PreserveWhitespace))
            .ToArray();

    private static IReadOnlyList<XElement> ExtractRenderedPreElements(string html) =>
        Regex.Matches(
                html,
                @"<pre(?:\s[^>]*)?>.*?</pre>",
                RegexOptions.Singleline)
            .Cast<Match>()
            .Select(
                match => XElement.Parse(
                    match.Value,
                    LoadOptions.PreserveWhitespace))
            .ToArray();

    private static string ExtractHighlightedCodeText(XElement code) =>
        string.Concat(code.Descendants("mark").Select(static mark => mark.Value));

    private static IReadOnlyList<string> GetHighlightedRegions(XElement code) =>
        code.Descendants("mark").Select(static mark => mark.Value).ToArray();

    private static string RangeMetadata(string code, string ranges)
    {
        var normalized = code
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');
        var hash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(normalized)))
            .ToLowerInvariant();
        return $"sha256:{hash};{ranges}";
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
