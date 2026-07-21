using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ufcpp.ContentConverter;

public static class CodeBlockNormalizer
{
    private static readonly Regex LegacyPreRegex = new(
        @"<pre\b(?<attributes>[^>]*)>(?<body>.*?)</pre\s*>(?:[ \t]*(?:\n[ \t]*)?</pre\s*>)?(?:[ \t]+(?=\n|$))?(?<following><div\b(?![^\r\n]*<pre\b)[^\r\n]*)?",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex AttributeRegex = new(
        @"\b(?<name>[\w:-]+)\s*=\s*(?<quote>[""'])(?<value>.*?)\k<quote>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex CodeWrapperRegex = new(
        @"^\s*<code\b[^>]*>.*</code\s*>\s*$",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex LegacyHighlightMarkupRegex = new(
        @"</?(?:code|span|em|reserved|comment|attvalue|type|inactive|coe|a)\b[^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex RenderableAnchorRegex = new(
        @"<[A-Za-z][\w:.-]*\b[^>]*\bid\s*=\s*(?<quote>[""'])(?<id>.*?)\k<quote>[^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex FenceLineRegex = new(
        @"^(?<indent> {0,3})(?<marker>`{3,}|~{3,})(?<remainder>.*)$",
        RegexOptions.Compiled);

    private static readonly Regex TitleExtensionRegex = new(
        @"\.(?<extension>cshtml|csproj|psm1|psd1|props|targets|config|xaml|xslt|json|html|razor|aspx|ascx|sln|xsd|xml|xsl|yaml|yml|cpp|cxx|hpp|java|swift|kt|kts|asm|sql|css|php|rb|rs|fsx|csx|ps1|py|mjs|js|ts|fs|vb|cc|go|sh|bash|bat|cmd|c|h)(?![A-Za-z0-9_])",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex PromptMarkupRegex = new(
        @"<span\b[^>]*\bclass\s*=\s*[""'][^""']*\bprompt\b[^""']*[""']",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex ShellCommandRegex = new(
        @"^(?:dotnet|git|nuget|msbuild|csc|vbc|fsc|mono|npm|npx|curl|wget|cszip|cd|mkdir|echo)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex MarkupStartRegex = new(
        @"^<(?<name>[A-Za-z_][\w:.-]*)(?:\s|/?>)",
        RegexOptions.Compiled);

    private static readonly Regex MarkupAttributeRegex = new(
        @"\s[A-Za-z_:][\w:.-]*\s*=\s*[""']",
        RegexOptions.Compiled);

    private static readonly Regex MarkupAnywhereRegex = new(
        @"</?[A-Za-z_][\w:.-]*(?:\s|/?>)",
        RegexOptions.Compiled);

    private static readonly Regex CilRegex = new(
        @"(?m)^\s*(?:\.(?:assembly|class|custom|event|field|locals|method)|IL_[0-9A-Fa-f]{4}:|(?:ldc\.i4|ldarg|ldloc|ldstr|stloc|callvirt|newobj|unbox(?:\.any)?|brtrue|brfalse)(?:\.\w+)*\b)",
        RegexOptions.Compiled);

    private static readonly Regex AssemblyRegex = new(
        @"(?im)^\s*(?:v?mov|fmul|fadd|lea|push|pop)\w*\s+(?:[er]?[abcd]x|[er]?(?:si|di|sp|bp)|[xyz]mm\d+|[dq]word\s+ptr|\w+\s+ptr)",
        RegexOptions.Compiled);

    private static readonly Regex PowerShellRegex = new(
        @"(?im)^\s*(?:\$(?:global:|script:|local:)?[A-Za-z_]\w*\s*=|param\s*\(|function\s+[A-Za-z_]\w*|\[CmdletBinding\b|(?:Get|Set|New|Write|Select|Where|ForEach|Add|Remove|Invoke)-[A-Za-z])",
        RegexOptions.Compiled);

    private static readonly Regex PythonRegex = new(
        @"(?m)^\s*(?:def\s+\w+\s*\(|from\s+\w+(?:\.\w+)*\s+import\s+|import\s+\w+\s*$)",
        RegexOptions.Compiled);

    private static readonly Regex VisualBasicRegex = new(
        @"(?m)^\s*(?:Imports\s+|Sub\s+\w+|End\s+(?:Sub|Class|Module)\b|Dim\s+\w+\s+As\s+|Public\s+Property\s+)",
        RegexOptions.Compiled);

    private static readonly Regex FSharpRegex = new(
        @"(?m)^\s*(?:let\s+\w+.*=|type\s+\w+\s*=|match\s+.+\s+with\s*$|open\s+[A-Z]\w*)",
        RegexOptions.Compiled);

    private static readonly Regex FSharpCaseRegex = new(
        @"(?m)^\s*\|\s*[A-Z]\w*",
        RegexOptions.Compiled);

    private static readonly Regex CppRegex = new(
        @"(?m)^\s*#include\s*[<""]|\b(?:std::|Console::)|\btemplate\s*<|extern\s+""C""",
        RegexOptions.Compiled);

    private static readonly Regex JavaRegex = new(
        @"(?m)^\s*(?:(?:public|private|protected)\s+)?class\s+\w+\s+extends\s+\w+|^\s*(?:public|private|protected)?[^\n;{]+\bthrows\s+[A-Z]\w*|\bSystem\.out\.|\bpublic\s+static\s+void\s+main\s*\(",
        RegexOptions.Compiled);

    private static readonly Regex JavaScriptRegex = new(
        @"(?m)^\s*const\s+[A-Za-z_$]\w*\s*=|^\s*function\s+[A-Za-z_$]\w*\s*\(|\b(?:document|window)\.",
        RegexOptions.Compiled);

    private static readonly Regex SqlRegex = new(
        @"(?is)^\s*(?:SELECT\b.*\bFROM\b|INSERT\s+INTO\b|CREATE\s+TABLE\b|UPDATE\b.*\bSET\b)",
        RegexOptions.Compiled);

    private static readonly Regex CSharpRegex = new(
        @"(?m)^\s*(?:using\s+(?:System|Microsoft)(?:\.[A-Za-z_]\w*)*\s*;|namespace\s+[A-Za-z_][\w.]*\s*[;{]|(?:var|string|bool|byte|sbyte|short|ushort|int|uint|long|ulong|float|double|decimal|char|object)\??\s+[A-Za-z_]\w*\s*=.*;|(?:(?:public|private|protected|internal|static|abstract|sealed|partial|readonly|ref|unsafe)\s+)*(?:class|struct|interface|record|enum|delegate)\s+[A-Za-z_]\w*|(?:for|foreach|while|if|switch|lock|using)\s*\(|[A-Z][A-Za-z0-9_.]*(?:<[^>\n]+>)?\s+[A-Za-z_]\w*\s*=\s*new\b|#(?:define|if|nullable)\b)|\b(?:Console|AppDomain|IEnumerable|Dictionary|JsonSerializer|XDocument)\.",
        RegexOptions.Compiled);

    private static readonly Regex ConsoleOutputRegex = new(
        @"(?m)^\s*(?:Unhandled exception\.|(?:System|Microsoft)\.[A-Za-z.]*Exception:|\bat (?:System|Microsoft)\.|Unknown custom metadata item kind:)",
        RegexOptions.Compiled);

    private static readonly Regex CssRegex = new(
        @"(?m)^[ \t]*(?:[.#]?[A-Za-z][\w-]*|[A-Za-z][\w-]*(?:\.[\w-]+)+)[ \t]*\n[ \t]*\{",
        RegexOptions.Compiled);

    private static readonly Regex RegexPatternRegex = new(
        @"(?:\\[AbBdDsSwWZz]|\(\?<\w+>|(?:^|[^\\])\[[^\]\n]+\][+*?]?)",
        RegexOptions.Compiled);

    private static readonly HashSet<string> LegacyCodeClasses = new(
        ["source", "xsource", "console", "xml"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> HtmlElementNames = new(
        [
            "a", "body", "button", "div", "form", "head", "html", "input", "label", "li", "link",
            "meta", "object", "ol", "option", "p", "script", "select", "span", "style", "table",
            "tbody", "td", "textarea", "tfoot", "th", "thead", "title", "tr", "ul",
        ],
        StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlySet<string> EmptyClasses =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    public static string Normalize(string value, string contextPath)
    {
        var normalized = TextUtilities.NormalizeNewlines(value);
        var source = normalized;
        normalized = LegacyPreRegex.Replace(
            source,
            match => RewriteLegacyBlock(
                match,
                contextPath,
                source,
                IsInsideHtmlTable(source, match.Index)));
        return AddMissingFenceLanguages(normalized, contextPath);
    }

    private static string RewriteLegacyBlock(
        Match match,
        string contextPath,
        string source,
        bool insideHtmlTable)
    {
        var attributes = ParseAttributes(match.Groups["attributes"].Value);
        var classes = SplitClasses(attributes.GetValueOrDefault("class"));
        var body = match.Groups["body"].Value;
        var hasLegacyCodeMarkup =
            classes.Any(LegacyCodeClasses.Contains) || CodeWrapperRegex.IsMatch(body);

        if (PromptMarkupRegex.IsMatch(body))
        {
            classes.Add("console");
        }

        var code = hasLegacyCodeMarkup
            ? LegacyHighlightMarkupRegex.Replace(body, string.Empty)
            : body;
        code = WebUtility.HtmlDecode(code);
        code = TextUtilities.NormalizeNewlines(code).Trim('\n');
        var anchorPrefix = BuildAnchorPrefix(
            insideHtmlTable
                ? body
                : $"<pre{match.Groups["attributes"].Value}>" + body);
        var language = DetectLanguage(
            code,
            attributes.GetValueOrDefault("title") ?? string.Empty,
            attributes.GetValueOrDefault("lang") ?? string.Empty,
            classes,
            contextPath,
            hasLegacyCodeMarkup ? "csharp" : "text");
        if (insideHtmlTable)
        {
            var encodedCode = WebUtility.HtmlEncode(code);
            return anchorPrefix +
                   $"<pre{match.Groups["attributes"].Value}>" +
                   $"<code class=\"language-{language}\">{encodedCode}</code></pre>" +
                   match.Groups["following"].Value.TrimEnd();
        }

        var fence = new string('`', Math.Max(3, LongestRun(code, '`') + 1));
        var prefix = match.Index != 0 && source[match.Index - 1] != '\n' ? "\n" : string.Empty;
        var endIndex = match.Index + match.Length;
        var following = match.Groups["following"].Value;
        var suffix = following.Length != 0
            ? "\n" + following.TrimEnd()
            : endIndex != source.Length && source[endIndex] != '\n' ? "\n" : string.Empty;
        return $"{prefix}{anchorPrefix}{fence}{language}\n{code}\n{fence}{suffix}";
    }

    private static string BuildAnchorPrefix(string value)
    {
        var ids = RenderableAnchorRegex.Matches(value)
            .Select(match => WebUtility.HtmlDecode(match.Groups["id"].Value))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        if (ids.Length == 0)
        {
            return string.Empty;
        }

        return string.Concat(
                   ids.Select(id => $"<a id=\"{WebUtility.HtmlEncode(id)}\"></a>\n"))
               + "\n";
    }

    private static bool IsInsideHtmlTable(string value, int index)
    {
        var lastOpen = value.LastIndexOf("<table", index, StringComparison.OrdinalIgnoreCase);
        var lastClose = value.LastIndexOf("</table", index, StringComparison.OrdinalIgnoreCase);
        return lastOpen > lastClose;
    }

    private static string AddMissingFenceLanguages(string value, string contextPath)
    {
        var lines = value.Split('\n');
        for (var index = 0; index < lines.Length; index++)
        {
            if (!TryGetFence(lines[index], out var opening)
                || !TryFindClosingFence(lines, index + 1, opening, out var closingIndex))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(opening.Remainder))
            {
                var code = string.Join('\n', lines, index + 1, closingIndex - index - 1);
                var language = DetectLanguage(
                    code,
                    string.Empty,
                    string.Empty,
                    EmptyClasses,
                    contextPath,
                    "csharp");
                lines[index] = opening.Indent + opening.Marker + language;
            }

            index = closingIndex;
        }

        return string.Join('\n', lines);
    }

    private static bool TryFindClosingFence(
        IReadOnlyList<string> lines,
        int startIndex,
        Fence opening,
        out int closingIndex)
    {
        for (var index = startIndex; index < lines.Count; index++)
        {
            if (TryGetFence(lines[index], out var candidate)
                && candidate.Marker[0] == opening.Marker[0]
                && candidate.Marker.Length >= opening.Marker.Length
                && string.IsNullOrWhiteSpace(candidate.Remainder))
            {
                closingIndex = index;
                return true;
            }
        }

        closingIndex = -1;
        return false;
    }

    private static bool TryGetFence(string line, out Fence fence)
    {
        var match = FenceLineRegex.Match(line);
        if (!match.Success)
        {
            fence = default;
            return false;
        }

        fence = new Fence(
            match.Groups["indent"].Value,
            match.Groups["marker"].Value,
            match.Groups["remainder"].Value);
        return true;
    }

    private static string DetectLanguage(
        string code,
        string title,
        string explicitLanguage,
        IReadOnlySet<string> classes,
        string contextPath,
        string fallbackLanguage)
    {
        if (!string.IsNullOrWhiteSpace(explicitLanguage))
        {
            return MapLanguageName(explicitLanguage);
        }

        if (classes.Contains("console"))
        {
            return "console";
        }

        var firstLine = code
            .Split('\n')
            .Select(line => line.Trim())
            .FirstOrDefault(line => line.Length != 0) ?? string.Empty;
        if (ShellCommandRegex.IsMatch(firstLine))
        {
            return "shell";
        }

        var extensionLanguage = DetectTitleExtension(title);
        if (extensionLanguage is not null)
        {
            return extensionLanguage;
        }

        var markupLanguage = DetectMarkupLanguage(code, firstLine);
        if (markupLanguage is not null)
        {
            return markupLanguage;
        }

        if (classes.Contains("xsource") || classes.Contains("xml"))
        {
            return "xml";
        }

        if (LooksLikeJson(code))
        {
            return "json";
        }

        if (ConsoleOutputRegex.IsMatch(code))
        {
            return "console";
        }

        if (fallbackLanguage == "text" && CssRegex.IsMatch(code))
        {
            return "css";
        }

        if (fallbackLanguage == "text"
            && contextPath.Contains("/study/xml/", StringComparison.OrdinalIgnoreCase)
            && MarkupAnywhereRegex.IsMatch(code))
        {
            return "xml";
        }

        if (CilRegex.IsMatch(code))
        {
            return "cil";
        }

        if (AssemblyRegex.IsMatch(code))
        {
            return "asm";
        }

        if (CppRegex.IsMatch(code))
        {
            return "cpp";
        }

        if (JavaRegex.IsMatch(code))
        {
            return "java";
        }

        if (VisualBasicRegex.IsMatch(code))
        {
            return "vbnet";
        }

        if (PythonRegex.IsMatch(code))
        {
            return "python";
        }

        if (FSharpRegex.IsMatch(code) && FSharpCaseRegex.IsMatch(code))
        {
            return "fsharp";
        }

        if (SqlRegex.IsMatch(code))
        {
            return "sql";
        }

        if (PowerShellRegex.IsMatch(code))
        {
            return "powershell";
        }

        if (JavaScriptRegex.IsMatch(code))
        {
            return "javascript";
        }

        if (CSharpRegex.IsMatch(code)
            && (fallbackLanguage != "text" || CSharpRegex.IsMatch(firstLine)))
        {
            return "csharp";
        }

        var titleLanguage = DetectTitleLanguage(title, code);
        if (titleLanguage is not null)
        {
            return titleLanguage;
        }

        if (fallbackLanguage == "csharp"
            && contextPath.Contains("/study/powershell/", StringComparison.OrdinalIgnoreCase))
        {
            return "powershell";
        }

        if (Regex.IsMatch(firstLine, @"^[-/][A-Za-z][\w-]*(?::\S+)?$"))
        {
            return "text";
        }

        return fallbackLanguage;
    }

    private static string? DetectTitleExtension(string title)
    {
        var matches = TitleExtensionRegex.Matches(title);
        if (matches.Count == 0)
        {
            return title.Contains(".htaccess", StringComparison.OrdinalIgnoreCase) ? "apache" : null;
        }

        var match = matches[matches.Count - 1];
        return match.Groups["extension"].Value.ToLowerInvariant() switch
        {
            "cs" or "csx" => "csharp",
            "cshtml" or "razor" => "razor",
            "vb" => "vbnet",
            "fs" or "fsx" => "fsharp",
            "cpp" or "cxx" or "cc" or "hpp" or "h" => "cpp",
            "c" => "c",
            "ps1" or "psm1" or "psd1" => "powershell",
            "py" => "python",
            "js" or "mjs" => "javascript",
            "ts" => "typescript",
            "json" => "json",
            "xml" or "xaml" or "xsl" or "xslt" or "xsd" or "csproj" or "props"
                or "targets" or "config" => "xml",
            "html" or "aspx" or "ascx" => "html",
            "css" => "css",
            "sql" => "sql",
            "java" => "java",
            "swift" => "swift",
            "rs" => "rust",
            "kt" or "kts" => "kotlin",
            "go" => "go",
            "sh" or "bash" => "shell",
            "bat" or "cmd" => "batch",
            "asm" => "asm",
            "yaml" or "yml" => "yaml",
            "php" => "php",
            "rb" => "ruby",
            "sln" => "text",
            _ => null,
        };
    }

    private static string? DetectTitleLanguage(string title, string code)
    {
        if (title.Contains("C#", StringComparison.OrdinalIgnoreCase))
        {
            return "csharp";
        }

        if (title.Contains("JavaScript", StringComparison.OrdinalIgnoreCase)
            || title.Contains("Node.js", StringComparison.OrdinalIgnoreCase))
        {
            return "javascript";
        }

        if (title.Contains("TypeScript", StringComparison.OrdinalIgnoreCase))
        {
            return "typescript";
        }

        if (title.Contains("PowerShell", StringComparison.OrdinalIgnoreCase))
        {
            return "powershell";
        }

        if (title.Contains("Python", StringComparison.OrdinalIgnoreCase))
        {
            return "python";
        }

        if (title.Contains("F#", StringComparison.OrdinalIgnoreCase))
        {
            return "fsharp";
        }

        if (title.Contains("C++", StringComparison.OrdinalIgnoreCase))
        {
            return "cpp";
        }

        if (Regex.IsMatch(title, @"(?i)(?:Visual Basic|VB(?:\.NET|\d+)?(?:\W|$))"))
        {
            return "vbnet";
        }

        if (Regex.IsMatch(title, @"(?i)Java(?!Script)"))
        {
            return "java";
        }

        if (title.Contains("Swift", StringComparison.OrdinalIgnoreCase))
        {
            return "swift";
        }

        if (title.Contains("Rust", StringComparison.OrdinalIgnoreCase))
        {
            return "rust";
        }

        if (title.Contains("Kotlin", StringComparison.OrdinalIgnoreCase))
        {
            return "kotlin";
        }

        if (title.Contains("Haskell", StringComparison.OrdinalIgnoreCase))
        {
            return "haskell";
        }

        if (title.Contains("JSON", StringComparison.OrdinalIgnoreCase))
        {
            return "json";
        }

        if (Regex.IsMatch(title, @"(?i)(?:XAML|XML|XSLT?|XSD|MSBuild|csproj|Directory\.Build)"))
        {
            return "xml";
        }

        if (title.Contains("HTML", StringComparison.OrdinalIgnoreCase))
        {
            return "html";
        }

        if (title.Contains("CSS", StringComparison.OrdinalIgnoreCase))
        {
            return "css";
        }

        if (title.Contains("DSL", StringComparison.OrdinalIgnoreCase))
        {
            return "text";
        }

        if (Regex.IsMatch(title, @"(?i)(?:SQL\s*文|SQL\s*クエリ|^\s*SQL\b)"))
        {
            return "sql";
        }

        if (Regex.IsMatch(title, @"(?i)(?:逆アセンブル|生成(?:結果)?の?IL|IL\s*(?:コード|的|$))"))
        {
            return "cil";
        }

        if (Regex.IsMatch(title, @"(?i)(?:x86|x64|Intel\s+Intrinsics|機械語)")
            || title.Contains("アセンブリ言語", StringComparison.OrdinalIgnoreCase))
        {
            return "asm";
        }

        if (title.Contains("正規表現", StringComparison.OrdinalIgnoreCase)
            && RegexPatternRegex.IsMatch(code))
        {
            return "regex";
        }

        return null;
    }

    private static string? DetectMarkupLanguage(string code, string firstLine)
    {
        if (Regex.IsMatch(firstLine, @"^<!DOCTYPE\s+html\b", RegexOptions.IgnoreCase)
            || Regex.IsMatch(firstLine, @"^<html(?:\s|>)", RegexOptions.IgnoreCase))
        {
            return "html";
        }

        if (firstLine.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase)
            || firstLine.StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase))
        {
            return "xml";
        }

        var tag = MarkupStartRegex.Match(firstLine);
        if (tag.Success
            && (code.Contains("</", StringComparison.Ordinal)
                || code.Contains("/>", StringComparison.Ordinal)
                || MarkupAttributeRegex.IsMatch(firstLine)))
        {
            return HtmlElementNames.Contains(tag.Groups["name"].Value)
                ? "html"
                : "xml";
        }

        return null;
    }

    private static bool LooksLikeJson(string code)
    {
        var trimmed = code.Trim();
        if (trimmed.Length < 3 || trimmed[0] is not ('{' or '['))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(trimmed);
            return document.RootElement.ValueKind switch
            {
                JsonValueKind.Object => document.RootElement.EnumerateObject().Any(),
                JsonValueKind.Array => document.RootElement.GetArrayLength() != 0
                    && trimmed.Contains('"'),
                _ => false,
            };
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static string MapLanguageName(string language)
    {
        var mapped = language.Trim().ToLowerInvariant() switch
        {
            "c#" or "cs" => "csharp",
            "vb" or "visual basic" or "visual basic .net" => "vbnet",
            "f#" => "fsharp",
            "c++" => "cpp",
            "text" or "txt" => "text",
            var value => value,
        };
        return Regex.IsMatch(mapped, @"^[a-z0-9+#.-]+$") ? mapped : "text";
    }

    private static Dictionary<string, string> ParseAttributes(string value) =>
        AttributeRegex.Matches(value)
            .ToDictionary(
                match => match.Groups["name"].Value,
                match => WebUtility.HtmlDecode(match.Groups["value"].Value),
                StringComparer.OrdinalIgnoreCase);

    private static HashSet<string> SplitClasses(string? value) =>
        (value ?? string.Empty)
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static int LongestRun(string value, char character)
    {
        var longest = 0;
        var current = 0;
        foreach (var item in value)
        {
            current = item == character ? current + 1 : 0;
            longest = Math.Max(longest, current);
        }

        return longest;
    }

    private readonly record struct Fence(string Indent, string Marker, string Remainder);
}
