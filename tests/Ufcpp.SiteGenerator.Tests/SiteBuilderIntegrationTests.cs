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
    public async Task BuildAsync_NoIndexOption_ControlsRobotsMeta()
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

        var normalOutput = site.GetOutputDirectory("indexable");
        await site.BuildAsync(normalOutput);
        var normalHead = Assert.Single(
            LoadHtmlDocument(Path.Combine(normalOutput, "index.html")).Root!.Elements("head"));
        Assert.Empty(normalHead.Elements("meta").Where(
            element => (string?)element.Attribute("name") == "robots"));

        var noIndexOutput = site.GetOutputDirectory("noindex");
        await site.BuildAsync(noIndexOutput, noIndex: true);
        var noIndexHead = Assert.Single(
            LoadHtmlDocument(Path.Combine(noIndexOutput, "index.html")).Root!.Elements("head"));
        var robots = Assert.Single(noIndexHead.Elements("meta").Where(
            element => (string?)element.Attribute("name") == "robots"));
        Assert.Equal("noindex, nofollow", (string?)robots.Attribute("content"));
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
        Assert.Matches(@"--color-code-keyword\s*:\s*#0000e1\s*;", css);
        Assert.Matches(@"--color-code-string\s*:\s*#a31515\s*;", css);
        Assert.Matches(@"\.site-main\s*\{[^}]*width\s*:\s*100%\s*;", css);
        Assert.DoesNotContain(".site-sidebar", css);
        Assert.DoesNotContain("--sidebar-width", css);
        Assert.DoesNotContain("grid-template-areas", css);
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
            @"\.content pre code \.keyword\s*\{[^}]*"
            + @"color\s*:\s*var\(--color-code-keyword\)\s*;"
            + @"[^}]*background\s*:\s*transparent\s*;",
            css);
        Assert.Matches(
            @"\.content pre code \.xmlName\s*\{[^}]*"
            + @"color\s*:\s*var\(--color-code-string\)\s*;",
            css);
        Assert.Matches(
            @"\.content pre code \.powershellCommand\s*\{[^}]*color\s*:\s*#000080\s*;",
            css);
        Assert.Matches(
            @"\.version\s*\{[^}]*display\s*:\s*block\s*;[^}]*"
            + @"background\s*:\s*transparent\s*;[^}]*"
            + @"color\s*:\s*var\(--color-heading\)\s*;",
            css);
        Assert.Matches(
            @"@media print\s*\{[\s\S]*?\.content pre code span\s*\{"
            + @"[^}]*color\s*:\s*#000000(?:\s*!important)?\s*;",
            css);
    }

    [Fact]
    public async Task BuildAsync_LegacyMath_WritesFormulaStyles()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "fraction.md",
            "Fraction",
            "/fraction/",
            "Article",
            102,
            -1,
            0,
            """
            <span class="math">x = <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>y</td></tr></table></span>
            <div class="math"><table class="sigma"><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i=0</td></tr></table></div>
            <div class="math"><span class="integral">∫</span><table class="integral"><tr><td class="intsup">1</td></tr><tr><td class="intsub">0</td></tr></table></div>
            <div class="math"><span class="paren">(</span><table class="matrix"><tr><td>x</td><td>y</td></tr></table><span class="paren">)</span></div>
            """));
        var output = site.GetOutputDirectory("fraction");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(output, "fraction", "index.html"));
        var fraction = Assert.Single(
            document.Descendants("table"),
            element => HasClassToken(element, "frac"));
        Assert.Equal(["1", "y"], fraction.Descendants("td").Select(cell => cell.Value));

        var css = await File.ReadAllTextAsync(Path.Combine(
            output,
            "assets",
            "css",
            "site.css"));
        Assert.Matches(
            @"\.content table\.frac,[^{]*\{[^}]*display\s*:\s*inline-table\s*;"
            + @"[^}]*vertical-align\s*:\s*middle\s*;",
            css);
        Assert.Matches(
            @"\.content table\.frac td,[^{]*\{[^}]*border\s*:\s*0\s*;"
            + @"[^}]*text-align\s*:\s*center\s*;",
            css);
        Assert.Matches(
            @"\.content table\.frac td\.num\s*\{[^}]*"
            + @"border-bottom\s*:\s*1pt solid currentColor\s*;",
            css);
        Assert.Contains(".content table.sigma", css);
        Assert.Contains(".content table.integral", css);
        Assert.Contains(".content table.matrix", css);
        Assert.Contains(".content table.branch", css);
        Assert.Contains(".content table.subsup", css);
        Assert.Contains(".content .math span.normal", css);
        Assert.Contains(".content .math span.vector", css);
        Assert.Contains(".content .math span.bar", css);
    }

    [Fact]
    public async Task BuildAsync_StudyArticle_OmitsRelatedPagesSidebarAndUsesFullWidthArticle()
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
        AssertStableDocumentStructure(
            document,
            "https://ufcpp.net/study/csharp/start/current/",
            "article");
        Assert.Empty(document.Descendants("aside"));
        Assert.Empty(document.Descendants().Where(element =>
            (string?)element.Attribute("aria-label") == "関連ページ"));

        var css = await File.ReadAllTextAsync(Path.Combine(
            output,
            "assets",
            "css",
            "site.css"));
        Assert.Matches(@"\.site-main\s*\{[^}]*width\s*:\s*100%\s*;", css);
        Assert.DoesNotContain(".site-sidebar", css);
    }

    [Fact]
    public async Task BuildAsync_StudyArticle_WritesBreadcrumbsTocAndKeywords()
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "index.md",
            "Home",
            "/",
            "Home",
            100,
            -1,
            0,
            "# Home"));
        site.AddPage(new(
            "study/index.md",
            "study",
            "/study/",
            "StudyTop",
            101,
            100,
            0,
            "# study"));
        site.AddPage(new(
            "study/csharp/index.md",
            "C# Guide",
            "/study/csharp/",
            "Subject",
            102,
            101,
            0,
            "# C# Guide"));
        site.AddPage(new(
            "study/csharp/start/index.md",
            "Basics",
            "/study/csharp/start/",
            "Chapter",
            103,
            102,
            0,
            "# Basics"));
        site.AddPage(new(
            "study/csharp/start/current.md",
            "Current Topic",
            "/study/csharp/start/current/",
            "Article",
            104,
            103,
            0,
            """
            # Current Topic

            ## <a id="sec-generated-title-1"></a> <a id="overview"></a>Overview

            ### <a id="details"></a>Details

            <strong id="term" class="keyword">Important term</strong>

            ## Page toc title

            <span id="page-keywords-title" class="keyword">Label collision</span>
            """,
            "2000-12-24T00:00:00",
            "2008-01-05T00:00:00"));
        var output = site.GetOutputDirectory("context-navigation");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(
            output,
            "study",
            "csharp",
            "start",
            "current",
            "index.html"));
        var article = Assert.Single(document.Descendants("article"));

        var breadcrumbs = Assert.Single(
            article.Elements("nav"),
            element => HasClassToken(element, "breadcrumbs"));
        var articleElements = article.Elements().ToList();
        Assert.True(
            articleElements.IndexOf(Assert.Single(article.Elements("h1")))
            < articleElements.IndexOf(breadcrumbs),
            "The page title must precede contextual article navigation.");
        var breadcrumbItems = breadcrumbs.Descendants("li").ToArray();
        Assert.Equal(
            ["TOP", "C# Guide", "Basics", "Current Topic"],
            breadcrumbItems.Select(item => NormalizeWhitespace(item.Value)));
        Assert.Equal(
            ["/", "/study/csharp/", "/study/csharp/start/"],
            breadcrumbItems
                .Take(3)
                .Select(item => (string?)Assert.Single(item.Elements("a")).Attribute("href")));
        var currentBreadcrumb = breadcrumbItems[^1];
        Assert.Empty(currentBreadcrumb.Elements("a"));
        Assert.Equal(
            "page",
            (string?)Assert.Single(currentBreadcrumb.Elements("span"))
                .Attribute("aria-current"));

        var articleMetadata = Assert.Single(
            article.Elements("div"),
            element => HasClassToken(element, "article-meta"));
        Assert.Equal(
            "2000/12/24 (Last updated:2008/01/05)",
            NormalizeWhitespace(articleMetadata.Value));
        Assert.Equal(
            ["2000-12-24T00:00:00", "2008-01-05T00:00:00"],
            articleMetadata
                .Descendants("time")
                .Select(time => (string?)time.Attribute("datetime")));

        var tableOfContents = Assert.Single(
            article.Elements("nav"),
            element => HasClassToken(element, "toc"));
        Assert.Equal(
            ["#overview", "#details", "#page-toc-title"],
            tableOfContents
                .Descendants("a")
                .Select(link => (string?)link.Attribute("href")));
        Assert.Equal(
            "目次",
            (string?)tableOfContents.Attribute("aria-label"));
        var overviewItem = Assert.Single(tableOfContents.Elements("ul"))
            .Elements("li")
            .First();
        Assert.Single(overviewItem.Elements("ul"));

        var keywords = Assert.Single(
            article.Elements("section"),
            element => HasClassToken(element, "keywords"));
        Assert.Equal(
            "キーワード",
            (string?)keywords.Attribute("aria-label"));
        var keyword = Assert.Single(
            keywords.Descendants("a"),
            link => (string?)link.Attribute("href") == "#term");
        Assert.Equal("#term", (string?)keyword.Attribute("href"));
        Assert.Equal("Important term", keyword.Value);
        Assert.Single(
            article.DescendantsAndSelf(),
            element => (string?)element.Attribute("id") == "page-toc-title");
        Assert.Single(
            article.DescendantsAndSelf(),
            element => (string?)element.Attribute("id") == "page-keywords-title");
        var articleBody = Assert.Single(
            article.Elements("div"),
            element => HasClassToken(element, "article-body"));
        Assert.True(
            articleElements.IndexOf(breadcrumbs) < articleElements.IndexOf(articleMetadata)
            && articleElements.IndexOf(articleMetadata) < articleElements.IndexOf(tableOfContents)
            && articleElements.IndexOf(tableOfContents) < articleElements.IndexOf(keywords)
            && articleElements.IndexOf(keywords) < articleElements.IndexOf(articleBody),
            "Original article metadata and indexes must precede the framed body.");
        Assert.Empty(document.Descendants("aside"));

        var css = await File.ReadAllTextAsync(Path.Combine(
            output,
            "assets",
            "css",
            "site.css"));
        Assert.Contains(".content .breadcrumbs", css);
        Assert.Contains(".content .toc", css);
        Assert.Contains(".content .keywords", css);
        Assert.Contains("viewBox='0 0 1696 1600'", css);
        Assert.DoesNotContain(@"\1F511", css);
        Assert.Matches(
            @"\.content \.sub-info-section\s*\{[^}]*padding\s*:\s*0\s*;[^}]*border\s*:\s*0\s*;",
            css);
        Assert.Matches(
            @"\.content\.article\s*\{[^}]*padding\s*:\s*0\s*;[^}]*border\s*:\s*0\s*;[^}]*background\s*:\s*transparent\s*;",
            css);
        Assert.DoesNotContain(".site-sidebar", css);
    }

    [Theory]
    [InlineData("2025-07-20T12:34:56", "2025-07-20T12:34:56", null)]
    [InlineData("2025-07-20T00:00:00", "2025-07-20T12:34:56", "2025/07/20")]
    [InlineData(
        "2025-07-20T12:34:56",
        "2025-07-21T12:34:56",
        "2025/07/20 (Last updated:2025/07/21)")]
    public async Task BuildAsync_StudyArticle_MatchesOriginalDateDisplay(
        string publishedAt,
        string updatedAt,
        string? expectedText)
    {
        using var site = new SiteFixture();
        site.AddPage(new(
            "study/page.md",
            "Dated article",
            "/study/page/",
            "Article",
            1,
            -1,
            0,
            "# Dated article",
            publishedAt,
            updatedAt));
        var output = site.GetOutputDirectory("article-dates");

        await site.BuildAsync(output);

        var document = LoadHtmlDocument(Path.Combine(
            output,
            "study",
            "page",
            "index.html"));
        var metadata = document
            .Descendants("div")
            .Where(element => HasClassToken(element, "article-meta"))
            .ToArray();
        if (expectedText is null)
        {
            Assert.Empty(metadata);
        }
        else
        {
            Assert.Equal(
                expectedText,
                NormalizeWhitespace(Assert.Single(metadata).Value));
        }
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
            <h2 id="blog-heading">Blog heading</h2>
            <strong id="blog-keyword" class="keyword">Blog keyword</strong>
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
        var title = Assert.Single(structure.Article.Elements("h1"));
        var breadcrumbs = Assert.Single(
            structure.Article.Elements("nav"),
            element => HasClassToken(element, "breadcrumbs"));
        var articleElements = structure.Article.Elements().ToList();
        Assert.True(
            articleElements.IndexOf(title) < articleElements.IndexOf(breadcrumbs)
            && articleElements.IndexOf(breadcrumbs) < articleElements.IndexOf(metadata)
            && articleElements.IndexOf(metadata) < articleElements.IndexOf(body),
            "Title, breadcrumbs, entry metadata, and body must remain in that order.");
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
        Assert.Empty(
            structure.Article.Descendants("nav").Where(
                element => HasClassToken(element, "toc")));
        Assert.Empty(
            structure.Article.Descendants("section").Where(
                element => HasClassToken(element, "keywords")));
        Assert.Empty(
            structure.Article.Descendants().Where(
                element => HasClassToken(element, "article-meta")));

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

    private static (XElement Layout, XElement Article)
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
        Assert.Equal(
            ["main"],
            layout.Elements().Select(element => element.Name.LocalName).ToArray());
        var article = Assert.Single(main.Elements("article"));
        Assert.Contains("content", GetClassTokens(article));
        Assert.Contains(expectedContentTypeClass, GetClassTokens(article));
        var footer = Assert.Single(body.Elements("footer"));

        var shell = body.Elements().ToList();
        Assert.True(
            shell.IndexOf(header) < shell.IndexOf(layout)
            && shell.IndexOf(layout) < shell.IndexOf(footer),
            "The durable page shell must be ordered header, content layout, footer.");

        return (layout, article);
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
            bool includePreviewServer = false,
            bool noIndex = false)
        {
            var options = new CliOptions
            {
                ContentDirectory = ContentDirectory,
                AssetsDirectory = AssetsDirectory,
                OutputDirectory = outputDirectory,
                IncludePreviewServer = includePreviewServer,
                NoIndex = noIndex,
                // These partial fixtures intentionally omit global navigation targets.
                SkipValidation = true,
            };

            return new SiteBuilder(options, NullLogger.Instance).BuildAsync();
        }

        public void Dispose() => _tempDirectory.Dispose();
    }
}
