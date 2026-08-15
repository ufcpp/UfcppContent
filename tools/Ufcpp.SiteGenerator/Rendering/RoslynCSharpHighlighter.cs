using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Ufcpp.SiteGenerator.Rendering;

internal sealed class RoslynCSharpHighlighter
{
    private const string CssClassPrefix = "roslyn-";
    private const string ConsoleImplicitGlobalUsings = """
        global using System;
        global using System.IO;
        global using System.Collections.Generic;
        global using System.Linq;
        global using System.Net.Http;
        global using System.Threading;
        global using System.Threading.Tasks;
        """;
    private readonly Project _baseProject;
    private readonly ConcurrentDictionary<string, string> _highlightedCodeCache =
        new(StringComparer.Ordinal);

    public RoslynCSharpHighlighter()
    {
        var workspace = new AdhocWorkspace();
        var project = workspace
            .AddProject("C# syntax highlighting", LanguageNames.CSharp)
            .WithParseOptions(
                CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview))
            .WithCompilationOptions(
                new CSharpCompilationOptions(OutputKind.ConsoleApplication))
            .AddMetadataReferences(CreatePlatformReferences());
        _baseProject = project
            .AddDocument(
                "GlobalUsings.g.cs",
                SourceText.From(ConsoleImplicitGlobalUsings, Encoding.UTF8))
            .Project;

        if (_baseProject.GetCompilationAsync().GetAwaiter().GetResult() is null)
        {
            throw new InvalidOperationException(
                "The C# classification project could not create a compilation.");
        }
    }

    public string Highlight(string code) =>
        _highlightedCodeCache.GetOrAdd(code, HighlightCore);

    private string HighlightCore(string code)
    {
        var sourceText = SourceText.From(code, Encoding.UTF8);
        var document = _baseProject.AddDocument("Snippet.cs", sourceText);
        var classifiedSpans = Classifier
            .GetClassifiedSpansAsync(
                document,
                new TextSpan(0, sourceText.Length))
            .GetAwaiter()
            .GetResult();

        return RenderClassifiedCode(code, classifiedSpans);
    }

    private static IEnumerable<MetadataReference> CreatePlatformReferences()
    {
        string[] referencePaths =
        {
            typeof(object).Assembly.Location,
            typeof(Console).Assembly.Location,
            typeof(Enumerable).Assembly.Location,
            typeof(ImmutableArray).Assembly.Location,
            typeof(ObservableCollection<>).Assembly.Location,
            typeof(INotifyPropertyChanged).Assembly.Location,
            typeof(DataTable).Assembly.Location,
            typeof(HttpClient).Assembly.Location,
            typeof(BigInteger).Assembly.Location,
            typeof(JsonSerializer).Assembly.Location,
            typeof(Regex).Assembly.Location,
            typeof(XDocument).Assembly.Location,
            typeof(SyntaxTree).Assembly.Location,
            typeof(CSharpSyntaxTree).Assembly.Location,
        };
        if (referencePaths.Any(string.IsNullOrWhiteSpace))
        {
            throw new InvalidOperationException(
                "A runtime assembly is unavailable for C# classification.");
        }

        return referencePaths
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase)
            .Select(static path => MetadataReference.CreateFromFile(path));
    }

    private static string RenderClassifiedCode(
        string code,
        IEnumerable<ClassifiedSpan> classifiedSpans)
    {
        var ranges = new List<ClassificationRange>();
        var boundaries = new SortedSet<int> { 0, code.Length };

        foreach (var classifiedSpan in classifiedSpans)
        {
            var span = classifiedSpan.TextSpan;
            if (span.Length == 0
                || classifiedSpan.ClassificationType == ClassificationTypeNames.WhiteSpace)
            {
                continue;
            }

            if (span.Start < 0 || span.End > code.Length)
            {
                throw new InvalidDataException(
                    $"Roslyn returned an out-of-range classification span {span}.");
            }

            ranges.Add(
                new ClassificationRange(
                    span.Start,
                    span.End,
                    GetCssClass(classifiedSpan.ClassificationType)));
            boundaries.Add(span.Start);
            boundaries.Add(span.End);
        }

        var starts = ranges.ToLookup(static range => range.Start);
        var ends = ranges.ToLookup(static range => range.End);
        var activeClasses = new SortedDictionary<string, int>(StringComparer.Ordinal);
        var positions = boundaries.ToArray();
        var html = new StringBuilder(code.Length * 2);
        string? openClasses = null;

        for (var index = 0; index < positions.Length - 1; index++)
        {
            var position = positions[index];
            var nextPosition = positions[index + 1];

            foreach (var range in ends[position])
            {
                RemoveActiveClass(activeClasses, range.CssClass);
            }

            foreach (var range in starts[position])
            {
                activeClasses[range.CssClass] =
                    activeClasses.GetValueOrDefault(range.CssClass) + 1;
            }

            var classes = activeClasses.Count == 0
                ? null
                : string.Join(' ', activeClasses.Keys);
            if (!string.Equals(openClasses, classes, StringComparison.Ordinal))
            {
                if (openClasses is not null)
                {
                    html.Append("</span>");
                }

                if (classes is not null)
                {
                    html.Append("<span class=\"");
                    html.Append(classes);
                    html.Append("\">");
                }

                openClasses = classes;
            }

            html.Append(
                HtmlEncoder.Default.Encode(code[position..nextPosition]));
        }

        if (openClasses is not null)
        {
            html.Append("</span>");
        }

        return html.ToString();
    }

    private static void RemoveActiveClass(
        IDictionary<string, int> activeClasses,
        string cssClass)
    {
        var count = activeClasses[cssClass];
        if (count == 1)
        {
            activeClasses.Remove(cssClass);
        }
        else
        {
            activeClasses[cssClass] = count - 1;
        }
    }

    private static string GetCssClass(string classificationType)
    {
        var cssClass = new StringBuilder(CssClassPrefix);
        var previousWasSeparator = true;

        foreach (var character in classificationType)
        {
            if (char.IsAsciiLetterOrDigit(character))
            {
                cssClass.Append(char.ToLowerInvariant(character));
                previousWasSeparator = false;
            }
            else if (!previousWasSeparator)
            {
                cssClass.Append('-');
                previousWasSeparator = true;
            }
        }

        if (previousWasSeparator && cssClass.Length > CssClassPrefix.Length)
        {
            cssClass.Length--;
        }

        if (cssClass.Length == CssClassPrefix.Length)
        {
            throw new InvalidDataException(
                $"Roslyn returned an invalid classification type '{classificationType}'.");
        }

        return cssClass.ToString();
    }

    private sealed record ClassificationRange(
        int Start,
        int End,
        string CssClass);
}
