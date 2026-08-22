using System.Text.RegularExpressions;
using Ufcpp.SiteGenerator.Rendering;

namespace Ufcpp.SiteGenerator.Tests;

/// <summary>
/// Guards the CSS parity between the migrated content and ufcpp.net.
///
/// The migrated Markdown keeps the original site's raw HTML, so it still carries
/// legacy class names. These tests fail when content starts using a class the
/// stylesheet does not cover, and they pin the declarations whose cascade the
/// legacy markup depends on. See docs/css-parity.md for the reconciliation
/// procedure and for the differences that are intentional.
///
/// Nothing here touches the network: the reference stylesheet is reconciled
/// offline by tools/css-class-reconciliation.ps1.
/// </summary>
public sealed class SiteCssParityTests
{
    /// <summary>
    /// Classes that appear in markup but are deliberately left unstyled. Every
    /// one of these is also unstyled on ufcpp.net, so it is not a parity gap.
    /// </summary>
    private static readonly Dictionary<string, string> UnstyledByDesign = new(StringComparer.Ordinal)
    {
        ["color"] = "Empty <span class=\"color\"> spacers inside ASCIIMath samples; unstyled upstream too.",
        ["speakerdeck-embed"] = "Third-party embed hook styled by SpeakerDeck's own script.",
        ["twitter-tweet"] = "Third-party embed hook styled by Twitter's own script.",
        ["silverlightControlHost"] = "Legacy Silverlight embed; sized by its inline style attribute.",
        ["subject"] = "Legacy hook with no bare .subject rule upstream (only ul.subject-menu).",
        ["language-console"] = "Code-fence language hook; highlighting is emitted as token spans instead.",
        ["language-xml"] = "Code-fence language hook; highlighting is emitted as token spans instead.",
        ["key-file-local-type"] = "Anchor marker on a <strong>; carries no styling upstream.",
        ["site-footer-links"] = "Our own footer wrapper; .site-footer p / .site-footer a already cover it.",
        ["version11*"] = "Content typo for \"version11\" in study/csharp/start/st_operator.md; broken upstream too.",
        ["xsource"] = "Content typo for \"source\" in study/dotnet/silverlight/devmodel.md; unstyled upstream too.",
    };

    /// <summary>
    /// The version marker palette, copied from ufcpp.net's bundle.min.css.
    /// </summary>
    public static TheoryData<string, string, string, string?> VersionMarkers() => new()
    {
        { "version2", "#ff0000", "#ff8e8e", null },
        { "version3", "#00cc00", "#8ecc8e", null },
        { "version4", "#0000cc", "#8e8ecc", null },
        { "version5", "#cc00cc", "#dd8ecc", null },
        { "version6", "#bbbb00", "#dddd66", null },
        { "version7", "#00aacc", "#66ccdd", null },
        { "version7_1", "#0000ff", "#66ccdd", null },
        { "version7_2", "#00cc00", "#66ccdd", null },
        { "version7_3", "#aa0000", "#66ccdd", null },
        { "version8", "#ff0000", "#ff8e8e", "ridge" },
        { "version9", "#00cc00", "#8ecc8e", "ridge" },
        { "version10", "#0000cc", "#8e8ecc", "ridge" },
        { "version11", "#cc00cc", "#dd8ecc", "ridge" },
        { "version12", "#bbbb00", "#dddd66", "ridge" },
        { "version13", "#00aacc", "#66ccdd", "ridge" },
        { "version14", "#ff0000", "#ff8e8e", "double" },
        { "version15", "#00cc00", "#8ecc8e", "double" },
        { "version16", "#0000cc", "#8e8ecc", "double" },
        { "version17", "#cc00cc", "#dd8ecc", "double" },
        { "version18", "#bbbb00", "#dddd66", "double" },
        { "version19", "#00aacc", "#66ccdd", "double" },
    };

    [Fact]
    public void EveryClassUsedInMarkup_IsStyledOrExplicitlyUnstyled()
    {
        var defined = DefinedClasses();

        var undocumented = UsedClasses()
            .Where(entry => !defined.Contains(entry.Key))
            .Where(entry => !UnstyledByDesign.ContainsKey(entry.Key))
            .OrderBy(entry => entry.Key, StringComparer.Ordinal)
            .Select(entry => $"  .{entry.Key} (e.g. {entry.Value})")
            .ToList();

        Assert.True(
            undocumented.Count == 0,
            "These classes are used in markup but site.css does not style them. Either port the "
                + "rule from ufcpp.net or add the class to UnstyledByDesign with a reason:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, undocumented));
    }

    [Fact]
    public void UnstyledByDesignList_HasNoStaleEntries()
    {
        var used = UsedClasses();
        var defined = DefinedClasses();

        var stale = UnstyledByDesign.Keys
            .Where(name => !used.ContainsKey(name) || defined.Contains(name))
            .OrderBy(name => name, StringComparer.Ordinal)
            .Select(name => used.ContainsKey(name)
                ? $"  .{name} is now styled by site.css"
                : $"  .{name} no longer appears in any markup")
            .ToList();

        Assert.True(
            stale.Count == 0,
            "UnstyledByDesign has entries that no longer apply; remove them:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, stale));
    }

    [Theory]
    [MemberData(nameof(VersionMarkers))]
    public void VersionMarker_MatchesOriginalPalette(
        string className,
        string color,
        string borderColor,
        string? borderStyle)
    {
        var declarations = RuleBody($@":where\(\.content\)\s*\.{Regex.Escape(className)}");

        Assert.NotNull(declarations);
        Assert.Contains($"color: {color};", declarations);
        Assert.Contains($"border-left-color: {borderColor};", declarations);

        if (borderStyle is null)
        {
            Assert.DoesNotContain("border-left-style", declarations);
        }
        else
        {
            Assert.Contains($"border-left-style: {borderStyle};", declarations);
        }
    }

    [Fact]
    public void VersionBase_KeepsTheColouredBarAndIndent()
    {
        var declarations = RuleBody(@":where\(\.content\)\s*\.version");

        Assert.NotNull(declarations);
        Assert.Contains("margin-left: 8px;", declarations);
        Assert.Contains("padding-left: 8px;", declarations);
        Assert.Contains("border-left: 8px solid #b0b0c0;", declarations);
    }

    /// <summary>
    /// The legacy markers rely on inheriting their left margin from
    /// <c>.version</c> at specificity (0,1,0). Writing the heading margin as the
    /// <c>margin</c> shorthand would also declare <c>margin-left: 0</c> at
    /// (0,1,1) and silently flatten every version bar back against the text.
    /// ufcpp.net uses the longhands for exactly this reason.
    /// </summary>
    [Theory]
    [InlineData("h3")]
    [InlineData("h4")]
    [InlineData("h5")]
    [InlineData("h6")]
    public void ContentHeading_UsesMarginLonghands(string tag)
    {
        var bodies = RuleBodiesFor($".content {tag}");

        Assert.NotEmpty(bodies);
        Assert.Contains(bodies, body => body.Contains("margin-top: 16px;", StringComparison.Ordinal));
        Assert.Contains(bodies, body => body.Contains("margin-bottom: 16px;", StringComparison.Ordinal));
        Assert.DoesNotContain(bodies, body => MarginShorthand.IsMatch(body));
    }

    /// <summary>
    /// The current ufcpp.net colors syntax keywords without changing their
    /// inherited weight. Generated and legacy token classes must do the same.
    /// </summary>
    [Theory]
    [InlineData(".content pre code .keyword")]
    [InlineData(".content pre code .roslyn-keyword")]
    [InlineData(".content pre code .roslyn-preprocessor-keyword")]
    [InlineData(".content pre code .roslyn-keyword-control")]
    [InlineData(".reserved")]
    [InlineData(".language-csharp .reserved")]
    public void CodeKeyword_InheritsTheBaseCodeWeight(string selector)
    {
        var bodies = RuleBodiesFor(selector);

        Assert.NotEmpty(bodies);
        Assert.DoesNotContain(
            bodies,
            body => Regex.IsMatch(body, @"(^|;)\s*font-weight\s*:", RegexOptions.Multiline));
    }

    /// <summary>
    /// The code token palette.
    ///
    /// The values follow Visual Studio's current C# colors rather than the ones
    /// baked into ufcpp.net's bundle.min.css: several of those predate a Visual
    /// Studio release that later introduced a dedicated color, and the site owner
    /// wants the newer ones. See docs/css-parity.md for the per-token rationale.
    ///
    /// Tokens whose color is defined as "the same as another token" carry a
    /// <c>var()</c> reference instead of a literal, so the two stay in step when
    /// one of them is retuned.
    /// </summary>
    public static TheoryData<string, string, string> CodeTokenPalette() => new()
    {
        { "--color-code-keyword", "#0000ff", "Visual Studio keyword." },
        { "--color-code-control", "#8f08c4", "ufcpp.net .control; Visual Studio leaves it as a keyword." },
        { "--color-code-preprocessor", "#808080", "Visual Studio preprocessor keyword." },
        { "--color-code-excluded", "#808080", "Visual Studio excluded code." },
        { "--color-code-comment", "#008000", "Visual Studio comment." },
        { "--color-code-string", "#a31515", "Visual Studio string." },
        { "--color-code-number", "#098658", "VS Code light theme; Visual Studio paints numbers as plain text." },
        { "--color-code-type", "#2b91af", "Visual Studio class name; the default for every kind of type." },
        { "--color-code-struct", "#007acc", "Visual Studio struct name." },
        { "--color-code-method", "#74531f", "Visual Studio method name." },
        { "--color-code-field", "#004080", "Visual Studio field name." },
        { "--color-code-property", "var(--color-code-field)", "Same color as a field, separate class." },
        { "--color-code-event", "var(--color-code-property)", "An event is closest to a property." },
        { "--color-code-enum-member", "var(--color-code-field)", "An enum member is a static field." },
        { "--color-code-constant", "#0070c1", "VS Code Visual Studio Light constant name." },
        { "--color-code-variable", "#000080", "Visual Studio local and parameter name." },
        { "--color-code-operator", "#004080", "Visual Studio paints operators as plain text; this site tints them." },
    };

    /// <summary>
    /// Which palette entry each token class has to read. The Roslyn highlighter
    /// splits C# far more finely than the legacy one did, and these categories
    /// are colored separately, so the classes must not be collapsed back into one
    /// shared color.
    ///
    /// Every kind of type is covered by the single <c>.roslyn-type-name</c> entry
    /// the highlighter adds alongside the specific class, so only the kinds that
    /// deviate from the default type color are listed here.
    /// </summary>
    public static TheoryData<string, string> CodeTokenSelectors() => new()
    {
        { ".content pre code .roslyn-keyword", "--color-code-keyword" },
        { ".content pre code .roslyn-keyword-control", "--color-code-control" },
        { ".content pre code .roslyn-preprocessor-keyword", "--color-code-preprocessor" },
        { ".content pre code .roslyn-excluded-code", "--color-code-excluded" },
        { ".content pre code .roslyn-comment", "--color-code-comment" },
        { ".content pre code .roslyn-string", "--color-code-string" },
        { ".content pre code .roslyn-number", "--color-code-number" },
        { ".content pre code .roslyn-type-name", "--color-code-type" },
        { ".content pre code .roslyn-type-name.roslyn-struct-name", "--color-code-struct" },
        { ".content pre code .roslyn-type-name.roslyn-record-struct-name", "--color-code-struct" },
        { ".content pre code .roslyn-method-name", "--color-code-method" },
        { ".content pre code .roslyn-extension-method-name", "--color-code-method" },
        { ".content pre code .roslyn-field-name", "--color-code-field" },
        { ".content pre code .roslyn-property-name", "--color-code-property" },
        { ".content pre code .roslyn-event-name", "--color-code-event" },
        { ".content pre code .roslyn-enum-member-name", "--color-code-enum-member" },
        { ".content pre code .roslyn-constant-name", "--color-code-constant" },
        { ".content pre code .roslyn-local-name", "--color-code-variable" },
        { ".content pre code .roslyn-parameter-name", "--color-code-variable" },
        { ".content pre code .roslyn-operator", "--color-code-operator" },
        { ".content pre code .roslyn-operator-overloaded", "--color-code-operator" },
        { ".content pre code .keyword", "--color-code-keyword" },
        { ".content pre code .preprocessorKeyword", "--color-code-preprocessor" },
        { ".reserved", "--color-code-keyword" },
    };

    /// <param name="variable">The custom property that carries the color.</param>
    /// <param name="color">
    /// The literal color, or a <c>var()</c> reference when the token is defined
    /// as following another one.
    /// </param>
    /// <param name="source">Where the value comes from, recorded so it can be re-checked.</param>
    [Theory]
    [MemberData(nameof(CodeTokenPalette))]
    public void CodeTokenColor_MatchesOriginalPalette(
        string variable,
        string color,
        string source)
    {
        Assert.NotEmpty(source);

        var declarations = RuleBody(":root");

        Assert.NotNull(declarations);
        Assert.Contains($"{variable}: {color};", declarations);
    }

    /// <summary>
    /// A type kind that carries no color of its own has to fall back to the
    /// shared type color, so that a kind the language adds later (C# 15's unions,
    /// for one) is styled as a type without anyone touching the stylesheet.
    /// Giving such a kind its own rule would silently break that fallback.
    /// </summary>
    [Theory]
    [InlineData("roslyn-class-name")]
    [InlineData("roslyn-record-class-name")]
    [InlineData("roslyn-interface-name")]
    [InlineData("roslyn-delegate-name")]
    [InlineData("roslyn-enum-name")]
    [InlineData("roslyn-module-name")]
    [InlineData("roslyn-type-parameter-name")]
    public void UncoloredTypeKind_HasNoRuleOfItsOwn(string cssClass)
    {
        var selectors = Regex.Replace(SiteCss.Value, @"\{[^{}]*\}", " ", RegexOptions.Singleline);

        Assert.DoesNotContain(cssClass, selectors, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(CodeTokenSelectors))]
    public void CodeToken_ReadsItsOwnPaletteEntry(string selector, string variable)
    {
        var bodies = RuleBodiesFor(selector);

        Assert.NotEmpty(bodies);
        Assert.All(bodies, body => Assert.Contains($"color: var({variable});", body));
    }

    /// <summary>
    /// Fields, properties, constants, locals and parameters used to share a
    /// single <c>--color-code-symbol</c> taken from Visual Studio's default
    /// theme. ufcpp.net gives them four different colors, so the merged variable
    /// must stay gone.
    /// </summary>
    [Fact]
    public void CodePalette_HasNoMergedSymbolColor()
    {
        Assert.DoesNotContain("--color-code-symbol", SiteCss.Value, StringComparison.Ordinal);
    }

    /// <summary>
    /// The generator rewrites the legacy expand markup into a native
    /// <c>&lt;details&gt;</c>, so the body has to be hidden by the element's own
    /// semantics rather than by a CSS rule that no script can undo.
    /// </summary>
    [Fact]
    public void ExpandPanelBody_IsNotHiddenByCss()
    {
        var panel = RuleBody(@":where\(\.content\)\s*\.expand-panel");
        var body = RuleBody(@":where\(\.content\)\s*\.expand-panel-body");

        Assert.NotNull(panel);
        Assert.NotNull(body);
        Assert.DoesNotContain("display: none", panel);
        Assert.DoesNotContain("display: none", body);

        // ufcpp.net's grey box moves to the inner wrapper so the summary is not
        // painted over.
        Assert.Contains("background-color: #f3f3f3", body);
    }

    /// <summary>
    /// <c>display</c> on a <c>&lt;summary&gt;</c> overrides
    /// <c>display: list-item</c> and removes the disclosure triangle, which is
    /// the only affordance left now that no icon font is served.
    /// </summary>
    [Fact]
    public void ExpandButton_KeepsTheDisclosureMarker()
    {
        var declarations = RuleBody(@":where\(\.content\)\s*\.expand-button");

        Assert.NotNull(declarations);
        Assert.DoesNotContain("display:", declarations);
    }

    /// <summary>
    /// Both legacy controls are operable again, so they have to look operable —
    /// pointer cursor plus the hover feedback ufcpp.net draws.
    /// </summary>
    [Theory]
    [InlineData(
        @":where\(\.content\)\s*\.expand-button",
        @":where\(\.content\)\s*\.expand-button:hover")]
    [InlineData(
        @":where\(\.content\)\s*\.tab-container\s*>\s*ul\s+li\s+label",
        @":where\(\.content\)\s*\.tab-container\s*>\s*ul\s+li:hover")]
    public void InteractiveLegacyControl_HasAffordance(
        string selectorPattern,
        string hoverPattern)
    {
        var declarations = RuleBody(selectorPattern);

        Assert.NotNull(declarations);
        Assert.Contains("cursor: pointer", declarations);
        Assert.NotNull(RuleBody(hoverPattern));
    }

    /// <summary>
    /// Tab panels are hidden by default and revealed by the sibling radio, so
    /// every index the rewriter can emit needs a matching pair of rules —
    /// otherwise a panel would be unreachable.
    /// </summary>
    [Fact]
    public void TabContainer_SwitchesEveryIndexTheRewriterCanEmit()
    {
        var panel = RuleBody(@":where\(\.content\)\s*\.tab-container\s*>\s*div");

        Assert.NotNull(panel);
        Assert.Contains("display: none", panel);

        var css = SiteCss.Value;
        for (var index = 1; index <= LegacyControlRewriter.MaxSwitchableTabs; index++)
        {
            Assert.Matches(
                new Regex(
                    $@"\.tab-container\s*>\s*input:nth-of-type\({index}\):checked\s*~\s*div:nth-of-type\({index}\)"),
                css);
            Assert.Matches(
                new Regex(
                    $@"\.tab-container\s*>\s*input:nth-of-type\({index}\):checked\s*~\s*ul\s+li:nth-child\({index}\)"),
                css);
        }
    }

    /// <summary>
    /// The radio is clipped out of view, so keyboard focus has to be visible on
    /// the tab it drives.
    /// </summary>
    [Fact]
    public void TabContainer_KeepsKeyboardFocusVisible()
    {
        var radio = RuleBody(@":where\(\.content\)\s*\.tab-container\s*>\s*input");

        Assert.NotNull(radio);
        Assert.DoesNotContain("display: none", radio);
        Assert.DoesNotContain("visibility: hidden", radio);

        Assert.Matches(
            new Regex(
                @"\.tab-container\s*>\s*input:nth-of-type\(1\):focus-visible\s*~\s*ul\s+li:nth-child\(1\)"),
            SiteCss.Value);
    }

    private static readonly char[] RazorExpressionChars = ['@', '{', '}', '(', ')'];

    private static readonly Regex MarginShorthand = new(@"(^|;)\s*margin\s*:", RegexOptions.Multiline);

    /// <summary>
    /// Declaration blocks of every rule that lists <paramref name="selector"/>,
    /// including grouped rules and rules inside media queries. Checking all of
    /// them matters: a later rule can undo an earlier one.
    /// </summary>
    private static List<string> RuleBodiesFor(string selector) =>
        Regex.Matches(SiteCss.Value, @"([^{}]+)\{([^{}]*)\}")
            .Where(rule => rule.Groups[1].Value
                .Split(',')
                .Any(part => part.Trim() == selector))
            .Select(rule => rule.Groups[2].Value)
            .ToList();

    private static string? RuleBody(string selectorPattern)
    {
        var match = Regex.Match(SiteCss.Value, selectorPattern + @"\s*\{([^}]*)\}");
        return match.Success ? match.Groups[1].Value : null;
    }

    private static HashSet<string> DefinedClasses()
    {
        // Remove declaration blocks so only selector text is left to scan.
        var selectors = Regex.Replace(SiteCss.Value, @"\{[^{}]*\}", " ", RegexOptions.Singleline);

        return Regex.Matches(selectors, @"\.(-?[_a-zA-Z][_a-zA-Z0-9-]*)")
            .Select(match => match.Groups[1].Value)
            .ToHashSet(StringComparer.Ordinal);
    }

    /// <summary>
    /// Maps each class used in markup to one file that uses it.
    /// </summary>
    private static Dictionary<string, string> UsedClasses()
    {
        var sources = Directory
            .EnumerateFiles(Path.Combine(RepoRoot, "content"), "*.md", SearchOption.AllDirectories)
            .Select(path => (Path: path, StripProse: true))
            .Concat(Directory
                .EnumerateFiles(
                    Path.Combine(RepoRoot, "tools", "Ufcpp.SiteGenerator", "Templates"),
                    "*.razor",
                    SearchOption.AllDirectories)
                .Select(path => (Path: path, StripProse: false)));

        var used = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var (path, stripProse) in sources)
        {
            var text = File.ReadAllText(path);
            if (stripProse)
            {
                text = RemoveNonMarkupText(text);
            }

            var relativePath = Path.GetRelativePath(RepoRoot, path).Replace('\\', '/');

            foreach (Match attribute in Regex.Matches(text, @"\bclass\s*=\s*""([^""]*)""", RegexOptions.IgnoreCase))
            {
                foreach (var token in attribute.Groups[1].Value.Split(
                    (char[]?)null,
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    // Razor expressions such as class="content @ContentTypeClass".
                    if (token.IndexOfAny(RazorExpressionChars) >= 0)
                    {
                        continue;
                    }

                    used.TryAdd(token, relativePath);
                }
            }
        }

        return used;
    }

    /// <summary>
    /// Drops fenced code blocks, inline code spans and entity-escaped markup, so
    /// classes that are only ever shown as sample text are not treated as used.
    /// </summary>
    private static string RemoveNonMarkupText(string text)
    {
        text = Regex.Replace(
            text,
            @"^[ \t]*(`{3,}|~{3,}).*?^[ \t]*\1[ \t]*$",
            "\n",
            RegexOptions.Multiline | RegexOptions.Singleline);
        text = Regex.Replace(text, @"(`+)(?!`).*?\1", " ", RegexOptions.Singleline);
        text = Regex.Replace(text, @"&lt;.*?&gt;", " ", RegexOptions.Singleline);
        return text;
    }

    private static readonly string RepoRoot = FindRepoRoot();

    private static readonly Lazy<string> SiteCss = new(() => StripComments(File.ReadAllText(
        Path.Combine(RepoRoot, "tools", "Ufcpp.SiteGenerator", "wwwroot", "css", "site.css"))));

    private static string StripComments(string css) =>
        Regex.Replace(css, @"/\*.*?\*/", " ", RegexOptions.Singleline);

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "UfcppContent.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException(
            $"Could not locate the repository root from '{AppContext.BaseDirectory}'.");
    }
}
