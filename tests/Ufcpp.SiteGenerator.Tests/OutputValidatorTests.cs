using System.Text;
using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Validation;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class OutputValidatorTests
{
    [Fact]
    public void Validate_CopiedHtmlAssetWithRelativeLinks_IgnoresContentsAndPreservesBytes()
    {
        using var site = new SiteFixture();
        var copiedHtml = Encoding.UTF8.GetBytes(
            "<!doctype html>\r\n<a href=\"chapter.html\">Chapter</a>\r\n"
            + "<script src=\"legacy.js\"></script>\r\n");
        var copiedPath = site.WriteAssetBytes("archive/manual.html", copiedHtml);
        site.AddPage(
            "/",
            "index.html",
            """
            <a href="/assets/archive/manual.html">Archived manual</a>
            <a href="/missing-control/">Broken control</a>
            """);

        AssertOnlyMissingControl(site.CreateValidator());
        var bytesAfterValidation = File.ReadAllBytes(copiedPath);

        Assert.Equal(copiedHtml, bytesAfterValidation);
    }

    [Fact]
    public void Validate_ProtocolRelativeExternalUrls_DoesNotReportInternalLinks()
    {
        using var site = new SiteFixture();
        site.AddPage(
            "/",
            "index.html",
            """
            <a href="//docs.example.test/guide">External guide</a>
            <script src="//cdn.example.test/site.js?v=2"></script>
            <a href="/missing-control/">Broken control</a>
            """);

        AssertOnlyMissingControl(site.CreateValidator());
    }

    [Fact]
    public void Validate_QueryBearingPageAndAssetUrls_ResolvesPathsWithoutQuery()
    {
        using var site = new SiteFixture();
        site.WriteAsset("css/site.css", "body { color: black; }");
        site.AddPage(
            "/",
            "index.html",
            """
            <a href="/guide/?view=print#details">Guide</a>
            <link rel="stylesheet" href="/assets/css/site.css?v=20260721">
            <a href="/missing-control/">Broken control</a>
            """);
        site.AddPage(
            "/guide/",
            "guide/index.html",
            """<h1 id="details">Details</h1>""");

        AssertOnlyMissingControl(site.CreateValidator());
    }

    [Fact]
    public void Validate_RootRelativeGeneratedFile_ResolvesExactOutputPath()
    {
        using var site = new SiteFixture();
        site.WriteGeneratedFile("rssfeed.xml", "<rss />");
        site.AddPage(
            "/",
            "index.html",
            """
            <a href="/rssfeed.xml">RSS</a>
            <a href="/missing-control/">Broken control</a>
            """);

        AssertOnlyMissingControl(site.CreateValidator());
    }

    [Fact]
    public void Validate_FragmentsMatchingIdOrLegacyName_OnSameAndCrossPage_AreAccepted()
    {
        using var site = new SiteFixture();
        site.AddPage(
            "/",
            "index.html",
            """
            <h1 id="local-id">Home</h1>
            <a name="local-name"></a>
            <a href="#local-id">Local id</a>
            <a href="#local-name">Local legacy name</a>
            <a href="/guide/#remote-id">Remote id</a>
            <a href="/guide/#remote-name">Remote legacy name</a>
            <a href="/missing-control/">Broken control</a>
            """);
        site.AddPage(
            "/guide/",
            "guide/index.html",
            """
            <h2 id="remote-id">Guide</h2>
            <a name="remote-name"></a>
            """);

        AssertOnlyMissingControl(site.CreateValidator());
    }

    [Theory]
    [InlineData("#missing-local", "/", "#missing-local")]
    [InlineData("/guide/#missing-remote", "/guide/", "#missing-remote")]
    public void Validate_MissingFragment_ReportsSourceTargetAndFragment(
        string url,
        string targetPath,
        string fragment)
    {
        using var site = new SiteFixture();
        site.AddPage(
            "/",
            "index.html",
            $"""<a href="{url}">Broken anchor</a>""");
        site.AddPage(
            "/guide/",
            "guide/index.html",
            """<h1 id="present">Guide</h1>""");

        var exception = Assert.Throws<AggregateException>(
            () => site.CreateValidator().Validate());
        var error = Assert.Single(exception.InnerExceptions);

        Assert.IsType<InvalidDataException>(error);
        Assert.Equal(
            $"Missing fragment '{fragment}' in target '{targetPath}' referenced in 'index.html'.",
            error.Message);
    }

    [Theory]
    [InlineData("""<meta name="viewport" content="width=device-width">""", "#viewport")]
    [InlineData("""<param name="source" value="data:application/octet-stream,">""", "#source")]
    [InlineData("""<div data-id="hero"></div>""", "#hero")]
    [InlineData("""<a data-name="legacy"></a>""", "#legacy")]
    public void Validate_NonAnchorNameAttribute_DoesNotSatisfyFragment(
        string namedElement,
        string fragment)
    {
        using var site = new SiteFixture();
        site.AddPage(
            "/",
            "index.html",
            $"""
            {namedElement}
            <a href="{fragment}">Broken anchor</a>
            """);

        var exception = Assert.Throws<AggregateException>(
            () => site.CreateValidator().Validate());
        var error = Assert.Single(exception.InnerExceptions);

        Assert.IsType<InvalidDataException>(error);
        Assert.Equal(
            $"Missing fragment '{fragment}' in target '/' referenced in 'index.html'.",
            error.Message);
    }

    [Fact]
    public void Validate_ExistingObjectAndSourceParamAssets_DoesNotReportMissingResources()
    {
        using var site = new SiteFixture();
        site.WriteAsset("media/player.xap", "player");
        site.WriteAsset("media/application.xap", "application");
        site.AddPage(
            "/",
            "index.html",
            """
            <object data="/assets/media/player.xap" type="application/x-silverlight-2">
              <param name="source" value="/assets/media/application.xap">
            </object>
            <a href="/missing-control/">Broken control</a>
            """);

        AssertOnlyMissingControl(site.CreateValidator());
    }

    [Theory]
    [InlineData(
        "<object data=\"/assets/media/missing-player.xap\"></object>",
        "/assets/media/missing-player.xap")]
    [InlineData(
        "<object data=\"data:application/x-silverlight-2,\"><param name=\"source\" value=\"/assets/media/missing-application.xap\"></object>",
        "/assets/media/missing-application.xap")]
    public void Validate_MissingObjectOrSourceParamAsset_ReportsResource(
        string resourceMarkup,
        string missingPath)
    {
        using var site = new SiteFixture();
        site.AddPage("/", "index.html", resourceMarkup);

        var exception = Assert.Throws<AggregateException>(
            () => site.CreateValidator().Validate());
        var error = Assert.Single(exception.InnerExceptions);

        Assert.IsType<InvalidDataException>(error);
        Assert.Equal(
            $"Missing asset '{missingPath}' referenced in 'index.html'.",
            error.Message);
    }

    [Fact]
    public void Validate_NonResourceParamValue_DoesNotTreatValueAsAssetReference()
    {
        using var site = new SiteFixture();
        site.AddPage(
            "/",
            "index.html",
            """
            <object data="data:application/x-silverlight-2,">
              <param name="background" value="/assets/media/not-a-resource.png">
            </object>
            <a href="/missing-control/">Broken control</a>
            """);

        AssertOnlyMissingControl(site.CreateValidator());
    }

    private static void AssertOnlyMissingControl(OutputValidator validator)
    {
        var exception = Assert.Throws<AggregateException>(validator.Validate);
        var error = Assert.Single(exception.InnerExceptions);

        Assert.IsType<InvalidDataException>(error);
        Assert.Equal(
            "Broken internal link '/missing-control/' in 'index.html'.",
            error.Message);
    }

    private sealed class SiteFixture : IDisposable
    {
        private readonly TempDirectory _tempDirectory = new();
        private readonly List<ContentPage> _pages = [];

        public SiteFixture()
        {
            OutputDirectory = Path.Combine(_tempDirectory.Path, "output");
            Directory.CreateDirectory(OutputDirectory);
        }

        public string OutputDirectory { get; }

        public void AddPage(string canonicalPath, string outputPath, string body)
        {
            WriteOutputFile(outputPath, Encoding.UTF8.GetBytes(CreateHtml(body)));
            _pages.Add(new ContentPage
            {
                FrontMatter = new FrontMatter
                {
                    Title = canonicalPath,
                    SourceUrl = "https://ufcpp.net" + canonicalPath,
                },
                RelativePath = outputPath.Replace("index.html", "source.md"),
                MarkdownBody = "",
                CanonicalPath = canonicalPath,
                OutputPath = outputPath,
            });
        }

        public string WriteAsset(string relativePath, string contents) =>
            WriteAssetBytes(relativePath, Encoding.UTF8.GetBytes(contents));

        public string WriteAssetBytes(string relativePath, byte[] contents) =>
            WriteOutputFile("assets/" + relativePath, contents);

        public string WriteGeneratedFile(string relativePath, string contents) =>
            WriteOutputFile(relativePath, Encoding.UTF8.GetBytes(contents));

        public OutputValidator CreateValidator() => new(
            OutputDirectory,
            _pages,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

        public void Dispose() => _tempDirectory.Dispose();

        private string WriteOutputFile(string relativePath, byte[] contents)
        {
            var path = Path.Combine(
                OutputDirectory,
                relativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllBytes(path, contents);
            return path;
        }

        private static string CreateHtml(string body) => $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="utf-8">
              <title>Validator fixture</title>
            </head>
            <body>
            {body}
            </body>
            </html>
            """;
    }
}
