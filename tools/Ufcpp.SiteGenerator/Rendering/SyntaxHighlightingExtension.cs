using ColorCode;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Ufcpp.SiteGenerator.Rendering;

internal sealed class SyntaxHighlightingExtension : IMarkdownExtension
{
    private static readonly IReadOnlyDictionary<string, (string Name, ILanguage Language)>
        LanguagesByName =
            new Dictionary<string, (string Name, ILanguage Language)>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["csharp"] = ("csharp", Languages.CSharp),
                ["cs"] = ("csharp", Languages.CSharp),
                ["c#"] = ("csharp", Languages.CSharp),
                ["xml"] = ("xml", Languages.Xml),
                ["html"] = ("html", Languages.Html),
                ["css"] = ("css", Languages.Css),
                ["powershell"] = ("powershell", Languages.PowerShell),
                ["ps1"] = ("powershell", Languages.PowerShell),
                ["cpp"] = ("cpp", Languages.Cpp),
                ["c++"] = ("cpp", Languages.Cpp),
                ["vbnet"] = ("vbnet", Languages.VbDotNet),
                ["vb"] = ("vbnet", Languages.VbDotNet),
                ["fsharp"] = ("fsharp", Languages.FSharp),
                ["fs"] = ("fsharp", Languages.FSharp),
                // ColorCode exposes JSON through lookup but has no Languages.Json property.
                ["json"] = ("json", Languages.FindById("json")),
                ["sql"] = ("sql", Languages.Sql),
                ["java"] = ("java", Languages.Java),
                ["python"] = ("python", Languages.Python),
                ["py"] = ("python", Languages.Python),
                ["javascript"] = ("javascript", Languages.JavaScript),
                ["js"] = ("javascript", Languages.JavaScript),
                ["typescript"] = ("typescript", Languages.Typescript),
                ["ts"] = ("typescript", Languages.Typescript),
            };

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.Insert(0, new HighlightedCodeBlockRenderer());
        }
    }

    private sealed class HighlightedCodeBlockRenderer
        : HtmlObjectRenderer<FencedCodeBlock>
    {
        protected override void Write(HtmlRenderer renderer, FencedCodeBlock block)
        {
            var languageName = block.Info?
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            if (languageName is null
                || !LanguagesByName.TryGetValue(languageName, out var language))
            {
                renderer.Write("<pre><code");
                if (languageName is not null)
                {
                    renderer.Write(" class=\"language-");
                    renderer.WriteEscape(languageName);
                    renderer.Write("\"");
                }

                renderer.Write(">");
                renderer.WriteEscape(block.Lines.ToString());
                renderer.WriteLine("</code></pre>");
                return;
            }

            renderer.Write("<pre><code class=\"language-");
            renderer.WriteEscape(language.Name);
            renderer.Write("\">");
            var code = block.Lines.ToString();

            try
            {
                renderer.Write(ExtractHighlightedCode(
                    new HtmlClassFormatter().GetHtmlString(code, language.Language)));
            }
            catch (Exception exception) when (
                exception is InvalidDataException
                    or ArgumentException
                    or InvalidOperationException
                    or System.Text.RegularExpressions.RegexMatchTimeoutException)
            {
                renderer.WriteEscape(code);
            }

            renderer.WriteLine("</code></pre>");
        }

        private static string ExtractHighlightedCode(string html)
        {
            const string OpeningTag = "<pre>\n";
            const string ClosingTag = "\n</pre>";
            var start = html.IndexOf(OpeningTag, StringComparison.Ordinal);
            var end = html.LastIndexOf(ClosingTag, StringComparison.Ordinal);
            if (start < 0 || end < start)
            {
                throw new InvalidDataException(
                    "The syntax highlighter returned an unexpected HTML fragment.");
            }

            return html[(start + OpeningTag.Length)..end];
        }
    }
}
