using System.Text.RegularExpressions;

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
    /// ufcpp.net hides <c>.expand-panel</c> and reveals it with JavaScript. This
    /// site ships no JavaScript, so porting that rule verbatim would hide the
    /// content permanently.
    /// </summary>
    [Fact]
    public void ExpandPanel_StaysVisibleWithoutScripting()
    {
        var declarations = RuleBody(@":where\(\.content\)\s*\.expand-panel");

        Assert.NotNull(declarations);
        Assert.DoesNotContain("display: none", declarations);
    }

    /// <summary>
    /// Nothing in the article body may look clickable when it is not: without
    /// JavaScript the expand buttons and tab strips are inert labels.
    /// </summary>
    [Theory]
    [InlineData(@":where\(\.content\)\s*\.expand-button")]
    [InlineData(@":where\(\.content\)\s*\.tab-container\s*>\s*ul\s+li")]
    public void InertLegacyControl_HasNoInteractiveAffordance(string selectorPattern)
    {
        var css = SiteCss.Value;
        var declarations = RuleBody(selectorPattern);

        Assert.NotNull(declarations);
        Assert.DoesNotContain("cursor:", declarations);
        Assert.DoesNotMatch(new Regex(selectorPattern + @"[^{]*:hover"), css);
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
