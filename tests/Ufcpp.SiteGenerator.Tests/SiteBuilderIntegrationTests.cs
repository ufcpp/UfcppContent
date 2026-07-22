using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class SiteBuilderIntegrationTests
{
    [Fact]
    public async Task BuildAsync_SearchPage_WritesAccessibleGoogleSiteSearchForm()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "search.md",
            "サイト内検索",
            "/search/",
            "Search",
            101,
            -1,
            0,
            """
            # サイト内検索

            <form class="site-search-form" action="https://www.google.com/search" method="get">
              <label for="site-search-query">検索キーワード</label>
              <input id="site-search-query" name="q" type="search" required="required">
              <input name="as_sitesearch" type="hidden" value="ufcpp.net">
              <button type="submit">Google で検索</button>
            </form>
            """));
        var output = site.GetOutputDirectory("search");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(output, "search", "index.html"));
        var form = Assert.Single(document.Descendants("form"));
        Assert.Equal("https://www.google.com/search", (string?)form.Attribute("action"));
        Assert.Equal("get", (string?)form.Attribute("method"));

        var label = Assert.Single(form.Elements("label"));
        Assert.Equal("site-search-query", (string?)label.Attribute("for"));
        Assert.Equal("検索キーワード", label.Value);

        var query = Assert.Single(
            form.Descendants("input"),
            input => (string?)input.Attribute("name") == "q");
        Assert.Equal("site-search-query", (string?)query.Attribute("id"));
        Assert.Equal("search", (string?)query.Attribute("type"));
        Assert.NotNull(query.Attribute("required"));

        var siteRestriction = Assert.Single(
            form.Descendants("input"),
            input => (string?)input.Attribute("name") == "as_sitesearch");
        Assert.Equal("hidden", (string?)siteRestriction.Attribute("type"));
        Assert.Equal("ufcpp.net", (string?)siteRestriction.Attribute("value"));
        Assert.Equal("Google で検索", Assert.Single(form.Elements("button")).Value);
        Assert.False(File.Exists(Path.Combine(output, "search-index.json")));
    }

    [Fact]
    public async Task BuildAsync_Page_WritesUfcppBrandShellAndPalette()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "index.md",
            "Home",
            "/",
            "Home",
            1,
            -1,
            0,
            "# Home"));
        var output = site.GetOutputDirectory("brand");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(output, "index.html"));
        var head = Assert.Single(document.Root!.Elements("head"));
        var themeColor = Assert.Single(
            head.Elements("meta"),
            element => (string?)element.Attribute("name") == "theme-color");
        Assert.Equal("#ccccff", (string?)themeColor.Attribute("content"));

        var header = Assert.Single(document.Root.Elements("body").Elements("header"));
        var logo = Assert.Single(
            header.Descendants("img"),
            element => HasClassToken(element, "site-logo"));
        Assert.Equal("/assets/images/sitelogo_l.jpg", (string?)logo.Attribute("src"));
        Assert.Equal("++C++; // 未確認飛行 C", (string?)logo.Attribute("alt"));

        var css = await File.ReadAllTextAsync(Path.Combine(
            output,
            "assets",
            "css",
            "site.css"));
        Assert.Matches(@"--color-page-bg\s*:\s*#f3f3f3\s*;", css);
        Assert.Matches(@"--color-brand-lavender\s*:\s*#ccccff\s*;", css);
        Assert.Matches(@"--color-brand-navy\s*:\s*#2a3869\s*;", css);
        Assert.Matches(@"--color-content-bg\s*:\s*#ffffff\s*;", css);
        Assert.Matches(@"--color-content-link\s*:\s*#a35951\s*;", css);
        Assert.Matches(
            @"\.site-body\s*\{[^}]*grid-template-areas\s*:\s*""sidebar main""\s*;",
            css);
        Assert.Matches(
            @"@media\s*\(\s*max-width\s*:\s*1024px\s*\)[^{]*\{[\s\S]*?"
            + @"\.site-body\s*\{[^}]*grid-template-areas\s*:\s*""main""\s*""sidebar""\s*;",
            css);
        Assert.Matches(
            @"\.content h1\s*\{[^}]*overflow-wrap\s*:\s*anywhere\s*;",
            css);
        Assert.Matches(
            @"@media\s*\(\s*max-width\s*:\s*640px\s*\)[^{]*\{[\s\S]*?"
            + @"\.site-title\s*\{[^}]*width\s*:\s*100%\s*;",
            css);
        Assert.Matches(
            @"@media\s*\(\s*max-width\s*:\s*640px\s*\)[^{]*\{[\s\S]*?"
            + @"\.site-body\s*\{[^}]*width\s*:\s*auto\s*;",
            css);
        Assert.Matches(
            @"\.content pre code \.keyword\s*\{[^}]*color\s*:\s*#0000e1\s*;"
            + @"[^}]*background\s*:\s*transparent\s*;",
            css);
        Assert.Matches(
            @"\.content pre code \.xmlName\s*\{[^}]*color\s*:\s*#a31515\s*;",
            css);
        Assert.Matches(
            @"\.content pre code \.powershellCommand\s*\{[^}]*color\s*:\s*#000080\s*;",
            css);
        Assert.Matches(
            @"@media print\s*\{[\s\S]*?\.content pre code span\s*\{"
            + @"[^}]*color\s*:\s*#000000(?:\s*!important)?\s*;",
            css);
    }

    [Fact]
    public async Task BuildAsync_StudyArticle_WritesOrderedSemanticResponsiveSidebar()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "study/csharp/start/index.md",
            "Start Chapter",
            "/study/csharp/start/",
            "Chapter",
            200,
            -1,
            0,
            "# Start Chapter"));
        site.AddPage(new(
            "study/csharp/start/a-last.md",
            "Advanced Concepts",
            "/study/csharp/start/advanced/",
            "Article",
            203,
            200,
            30,
            "# Advanced Concepts"));
        site.AddPage(new(
            "study/csharp/start/current.md",
            "Current Topic",
            "/study/csharp/start/current/",
            "Article",
            202,
            200,
            20,
            "# Current Topic"));
        site.AddPage(new(
            "study/csharp/start/z-first.md",
            "Getting Started",
            "/study/csharp/start/getting-started/",
            "Article",
            201,
            200,
            10,
            "# Getting Started"));
        var output = site.GetOutputDirectory("study");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(
            output,
            "study",
            "csharp",
            "start",
            "current",
            "index.html"));
        var structure = AssertStableDocumentStructure(
            document,
            "https://ufcpp.net/study/csharp/start/current/",
            "article");
        var aside = AssertAsideAfterMain(structure.Layout, structure.Main);
        var navigation = Assert.Single(aside.Descendants("nav"));
        Assert.False(string.IsNullOrWhiteSpace((string?)navigation.Attribute("aria-label")));

        var links = navigation.Descendants("a").ToList();
        var parentIndex = AssertSingleLink(
            links,
            "/study/csharp/start/",
            "Start Chapter");
        var firstSiblingIndex = AssertSingleLink(
            links,
            "/study/csharp/start/getting-started/",
            "Getting Started");
        var lastSiblingIndex = AssertSingleLink(
            links,
            "/study/csharp/start/advanced/",
            "Advanced Concepts");
        Assert.True(
            parentIndex < firstSiblingIndex && firstSiblingIndex < lastSiblingIndex,
            "The parent link must precede sibling links ordered by sort_order.");

        var css = await File.ReadAllTextAsync(Path.Combine(
            output,
            "assets",
            "css",
            "site.css"));
        AssertResponsiveLayoutRule(css, structure.Layout, aside);
    }

    [Fact]
    public async Task BuildAsync_BlogEntry_WritesCanonicalTimesAndTagsOutsideBody()
    {
        const string PublishedAt = "2025-07-20T09:30:00+09:00";
        const string UpdatedAt = "2025-07-21T18:45:00+09:00";

        using var site = new SiteFixture();
        site.AddPage(new(
            "blog/2025/7/index.md",
            "2025 July",
            "/blog/2025/7/",
            "BlogMonth",
            300,
            -1,
            7,
            "# 2025 July"));
        site.AddPage(new(
            "blog/2025/7/phase-four/index.md",
            "Phase Four Metadata",
            "/blog/2025/7/phase-four/",
            "BlogEntry",
            301,
            300,
            2,
            """
            <section id="fixture-body">
            <h1>Phase Four Metadata</h1>
            <p>Body marker only.</p>
            </section>
            """,
            PublishedAt,
            UpdatedAt,
            ["csharp", "release"]));
        var output = site.GetOutputDirectory("blog");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(
            output,
            "blog",
            "2025",
            "7",
            "phase-four",
            "index.html"));
        var structure = AssertStableDocumentStructure(
            document,
            "https://ufcpp.net/blog/2025/7/phase-four/",
            "blog-entry");
        var body = Assert.Single(
            structure.Article.Descendants("section"),
            element => (string?)element.Attribute("id") == "fixture-body");
        var metadata = Assert.Single(
            structure.Article.Descendants(),
            element => HasClassToken(element, "entry-meta"));
        var articleElements = structure.Article.DescendantsAndSelf().ToList();
        Assert.True(
            articleElements.IndexOf(metadata) < articleElements.IndexOf(body),
            "Entry metadata must precede the rendered body.");
        Assert.DoesNotContain(metadata, body.AncestorsAndSelf());

        Assert.Equal(
            [PublishedAt, UpdatedAt],
            metadata
                .Descendants("time")
                .Select(time => (string?)time.Attribute("datetime"))
                .ToArray());

        var tagText = metadata
            .DescendantNodes()
            .OfType<XText>()
            .Select(text => NormalizeWhitespace(text.Value))
            .Where(text => text is "csharp" or "release")
            .ToArray();
        Assert.Equal(["csharp", "release"], tagText);

        AssertAsideAfterMain(structure.Layout, structure.Main);
    }

    [Fact]
    public async Task BuildAsync_CustomAssetsDirectory_RewritesAndCopiesLegacyAsset()
    {
        using var site = new SiteFixture();
        site.WriteAsset("media/demo/image.png", "fixture image");
        site.AddPage(new(
            "index.md",
            "Home",
            "/",
            "Home",
            1,
            -1,
            0,
            """<img src="/media/demo/image.png" alt="Demo">"""));
        var output = site.GetOutputDirectory("custom-assets");

        await site.BuildAsync(output);

        var html = await File.ReadAllTextAsync(Path.Combine(output, "index.html"));
        var copiedAsset = Path.Combine(
            output,
            "assets",
            "media",
            "demo",
            "image.png");
        Assert.Contains("src=\"/assets/media/demo/image.png\"", html);
        Assert.Equal("fixture image", await File.ReadAllTextAsync(copiedAsset));
    }

    [Fact]
    public async Task BuildAsync_PreviewServerOption_ControlsProjectlessDotNet10Host()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "index.md",
            "Home",
            "/",
            "Home",
            1,
            -1,
            0,
            "# Home"));
        var output = site.GetOutputDirectory("preview-server");
        var serverPath = Path.Combine(output, "server.cs");

        await site.BuildAsync(output);
        Assert.False(File.Exists(serverPath));

        await site.BuildAsync(output, includePreviewServer: true);
        var serverSource = await File.ReadAllTextAsync(serverPath);
        Assert.Contains("#:sdk Microsoft.NET.Sdk.Web", serverSource);
        Assert.Contains("#:property TargetFramework=net10.0", serverSource);
        Assert.Contains("app.UseDefaultFiles();", serverSource);
        Assert.Contains("app.UseStaticFiles();", serverSource);
        Assert.Contains("await app.RunAsync();", serverSource);
        Assert.False(Directory.EnumerateFiles(output, "*.csproj").Any());

        await site.BuildAsync(output);
        Assert.False(File.Exists(serverPath));
    }

    [Fact]
    public async Task BuildAsync_Rebuild_RemovesDeletedPagesAliasesAndAssets()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "index.md",
            "Home",
            "/",
            "Home",
            1,
            -1,
            0,
            "# Home"));
        site.AddPage(new(
            "obsolete.md",
            "Obsolete",
            "/obsolete/",
            "Article",
            2,
            -1,
            0,
            "# Obsolete",
            Aliases: ["/old-obsolete/"]));
        site.WriteAsset("obsolete.txt", "obsolete");
        var output = site.GetOutputDirectory("rebuild");

        await site.BuildAsync(output);

        var obsoletePage = Path.Combine(output, "obsolete", "index.html");
        var obsoleteAlias = Path.Combine(output, "old-obsolete", "index.html");
        var obsoleteAsset = Path.Combine(output, "assets", "obsolete.txt");
        Assert.True(File.Exists(obsoletePage));
        Assert.True(File.Exists(obsoleteAlias));
        Assert.True(File.Exists(obsoleteAsset));

        site.RemovePage("obsolete.md");
        site.RemoveAsset("obsolete.txt");
        await site.BuildAsync(output);

        Assert.True(File.Exists(Path.Combine(output, "index.html")));
        Assert.False(File.Exists(obsoletePage));
        Assert.False(File.Exists(obsoleteAlias));
        Assert.False(File.Exists(obsoleteAsset));
    }

    [Fact]
    public async Task BuildAsync_FailedRebuild_PreservesLastSuccessfulOutput()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "index.md",
            "Home",
            "/",
            "Home",
            1,
            -1,
            0,
            "# Home"));
        var output = site.GetOutputDirectory("failed-rebuild");

        await site.BuildAsync(output);

        var homePath = Path.Combine(output, "index.html");
        var successfulOutput = await File.ReadAllBytesAsync(homePath);
        site.AddPage(new(
            "duplicate.md",
            "Duplicate Home",
            "/",
            "Home",
            2,
            -1,
            0,
            "# Duplicate"));

        await Assert.ThrowsAsync<InvalidDataException>(() => site.BuildAsync(output));

        Assert.Equal(successfulOutput, await File.ReadAllBytesAsync(homePath));
    }

    private static (XElement Layout, XElement Main, XElement Article)
        AssertStableDocumentStructure(
            XDocument document,
            string expectedCanonicalUrl,
            string expectedContentTypeClass)
    {
        Assert.NotNull(document.DocumentType);
        Assert.Equal("html", document.DocumentType!.Name, ignoreCase: true);

        var html = Assert.Single(document.Elements("html"));
        Assert.Equal("ja", (string?)html.Attribute("lang"));
        Assert.Equal(
            ["head", "body"],
            html.Elements().Select(element => element.Name.LocalName).ToArray());

        var head = Assert.Single(html.Elements("head"));
        var canonical = Assert.Single(
            head.Elements("link"),
            element => string.Equals(
                (string?)element.Attribute("rel"),
                "canonical",
                StringComparison.OrdinalIgnoreCase));
        Assert.Equal(expectedCanonicalUrl, (string?)canonical.Attribute("href"));

        var body = Assert.Single(html.Elements("body"));
        var header = Assert.Single(body.Elements("header"));
        Assert.Single(header.Descendants("nav"));
        var layout = Assert.Single(
            body.Elements(),
            element => HasClassToken(element, "site-body"));
        var main = Assert.Single(layout.Elements("main"));
        var article = Assert.Single(main.Elements("article"));
        Assert.Contains("content", GetClassTokens(article));
        Assert.Contains(expectedContentTypeClass, GetClassTokens(article));
        var footer = Assert.Single(body.Elements("footer"));

        var shell = body.Elements().ToList();
        Assert.True(
            shell.IndexOf(header) < shell.IndexOf(layout)
            && shell.IndexOf(layout) < shell.IndexOf(footer),
            "The durable page shell must be ordered header, content layout, footer.");

        return (layout, main, article);
    }

    private static XElement AssertAsideAfterMain(XElement layout, XElement main)
    {
        var aside = Assert.Single(layout.Elements("aside"));
        var columns = layout.Elements().ToList();
        Assert.True(
            columns.IndexOf(main) < columns.IndexOf(aside),
            "The semantic sidebar must follow the primary content.");
        return aside;
    }

    private static int AssertSingleLink(
        IList<XElement> links,
        string expectedHref,
        string expectedText)
    {
        var link = Assert.Single(
            links,
            candidate => (string?)candidate.Attribute("href") == expectedHref);
        Assert.Equal(expectedText, NormalizeWhitespace(link.Value));
        return links.IndexOf(link);
    }

    private static void AssertResponsiveLayoutRule(
        string css,
        XElement layout,
        XElement aside)
    {
        var layoutClasses = GetClassTokens(layout)
            .Concat(GetClassTokens(aside))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var hasResponsiveRule = EnumerateNarrowMediaBlocks(css).Any(block =>
            block.MaxWidth <= 800
            && layoutClasses.Any(className => Regex.IsMatch(
                block.Rules,
                $@"\.{Regex.Escape(className)}(?![\w-])",
                RegexOptions.IgnoreCase))
            && Regex.IsMatch(
                block.Rules,
                @"\b(?:display|width|grid-template(?:-columns|-areas)?|flex-direction|flex-flow|order)\s*:",
                RegexOptions.IgnoreCase));

        Assert.True(
            hasResponsiveRule,
            "Expected a max-width media query that changes the generated layout or sidebar.");
    }

    private static IEnumerable<(int MaxWidth, string Rules)> EnumerateNarrowMediaBlocks(
        string css)
    {
        var mediaHeaders = Regex.Matches(
            css,
            @"@media[^{]*\(\s*max-width\s*:\s*(?<width>\d+)px\s*\)[^{]*\{",
            RegexOptions.IgnoreCase);

        foreach (Match mediaHeader in mediaHeaders)
        {
            var openBrace = mediaHeader.Index + mediaHeader.Length - 1;
            var depth = 1;
            for (var index = openBrace + 1; index < css.Length; index++)
            {
                depth += css[index] switch
                {
                    '{' => 1,
                    '}' => -1,
                    _ => 0,
                };

                if (depth == 0)
                {
                    yield return (
                        int.Parse(
                            mediaHeader.Groups["width"].Value,
                            CultureInfo.InvariantCulture),
                        css[(openBrace + 1)..index]);
                    break;
                }
            }
        }
    }

    private static IEnumerable<string> EnumerateJsonStrings(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    yield return property.Name;
                    foreach (var value in EnumerateJsonStrings(property.Value))
                    {
                        yield return value;
                    }
                }

                break;
            case JsonValueKind.Array:
                foreach (var item in element.EnumerateArray())
                {
                    foreach (var value in EnumerateJsonStrings(item))
                    {
                        yield return value;
                    }
                }

                break;
            case JsonValueKind.String:
                yield return element.GetString()!;
                break;
        }
    }

    private static XDocument LoadHtmlDocument(string path)
    {
        var html = Regex.Replace(
            File.ReadAllText(path),
            @"<(area|base|br|col|embed|hr|img|input|link|meta|param|source|track|wbr)(\b[^<>]*?)(?<!/)>",
            "<$1$2 />",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
            .Replace("&copy;", "&#169;", StringComparison.Ordinal)
            .Replace("&ndash;", "&#8211;", StringComparison.Ordinal)
            .Replace("&ensp;", "&#8194;", StringComparison.Ordinal)
            .Replace("&nbsp;", "&#160;", StringComparison.Ordinal)
            .Replace("&mdash;", "&#8212;", StringComparison.Ordinal);
        return XDocument.Parse(html, LoadOptions.PreserveWhitespace);
    }

    private static bool HasClassToken(XElement element, string expected) =>
        GetClassTokens(element).Contains(expected, StringComparer.Ordinal);

    private static string[] GetClassTokens(XElement element) =>
        ((string?)element.Attribute("class") ?? "")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

    private static string NormalizeWhitespace(string value) =>
        Regex.Replace(value, @"\s+", " ").Trim();

    private sealed record PageFixture(
        string RelativePath,
        string Title,
        string CanonicalPath,
        string ContentType,
        int UmbracoId,
        int ParentId,
        int SortOrder,
        string Body,
        string PublishedAt = "2025-07-20T12:34:56",
        string UpdatedAt = "2025-07-20T12:34:56",
        string[]? Tags = null,
        string[]? Aliases = null);

    private sealed class SiteFixture : IDisposable
    {
        private readonly TempDirectory _tempDirectory = new();

        public SiteFixture()
        {
            ContentDirectory = Path.Combine(_tempDirectory.Path, "content");
            AssetsDirectory = Path.Combine(_tempDirectory.Path, "custom-assets");
            Directory.CreateDirectory(ContentDirectory);
            Directory.CreateDirectory(AssetsDirectory);
        }

        public string ContentDirectory { get; }

        public string AssetsDirectory { get; }

        public void WriteAsset(string relativePath, string contents)
        {
            var path = Path.Combine(
                AssetsDirectory,
                relativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, contents, new UTF8Encoding(false));
        }

        public void RemoveAsset(string relativePath) =>
            File.Delete(Path.Combine(
                AssetsDirectory,
                relativePath.Replace('/', Path.DirectorySeparatorChar)));

        public void AddPage(PageFixture page)
        {
            var tags = page.Tags is { Length: > 0 }
                ? "tags:\n" + string.Join(
                    '\n',
                    page.Tags.Select(tag => $"  - {JsonSerializer.Serialize(tag)}"))
                : "tags: []";
            var aliases = page.Aliases is { Length: > 0 }
                ? "aliases:\n" + string.Join(
                    '\n',
                    page.Aliases.Select(alias => $"  - {JsonSerializer.Serialize(alias)}"))
                : "aliases: []";
            var markdown = $"""
                ---
                title: {JsonSerializer.Serialize(page.Title)}
                source_url: {JsonSerializer.Serialize("https://ufcpp.net" + page.CanonicalPath)}
                content_type: {JsonSerializer.Serialize(page.ContentType)}
                published_at: {JsonSerializer.Serialize(page.PublishedAt)}
                updated_at: {JsonSerializer.Serialize(page.UpdatedAt)}
                {tags}
                umbraco_id: {page.UmbracoId}
                parent_id: {page.ParentId}
                sort_order: {page.SortOrder}
                {aliases}
                ---
                {page.Body}
                """;
            var path = Path.Combine(
                ContentDirectory,
                page.RelativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, markdown, new UTF8Encoding(false));
        }

        public void RemovePage(string relativePath) =>
            File.Delete(Path.Combine(
                ContentDirectory,
                relativePath.Replace('/', Path.DirectorySeparatorChar)));

        public string GetOutputDirectory(string name) =>
            Path.Combine(_tempDirectory.Path, name);

        public Task BuildAsync(
            string outputDirectory,
            bool includePreviewServer = false)
        {
            var options = new CliOptions
            {
                ContentDirectory = ContentDirectory,
                AssetsDirectory = AssetsDirectory,
                OutputDirectory = outputDirectory,
                IncludePreviewServer = includePreviewServer,
                // These partial fixtures intentionally omit global navigation targets.
                SkipValidation = true,
            };

            return new SiteBuilder(options, NullLogger.Instance).BuildAsync();
        }

        public void Dispose() => _tempDirectory.Dispose();
    }
}
