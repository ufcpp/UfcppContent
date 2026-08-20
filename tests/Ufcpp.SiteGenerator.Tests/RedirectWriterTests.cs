using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class RedirectWriterTests
{
    [Fact]
    public void Write_ExistingAlias_UpdatesRedirectTarget()
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/first-target/",
            "https://ufcpp.net/first-target/",
            ["/legacy/"],
            tempDirectory.Path);
        RedirectWriter.Write(
            "/second-target/",
            "https://ufcpp.net/second-target/",
            ["/legacy/"],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "legacy",
            "index.html"));
        Assert.Contains("href=\"../second-target/\"", html);
        Assert.DoesNotContain("../first-target/", html);
    }

    [Fact]
    public void Write_AliasEquivalentToCanonicalOutput_PreservesPrimaryPage()
    {
        using var tempDirectory = new TempDirectory();
        var primaryPath = Path.Combine(
            tempDirectory.Path,
            "study",
            "index.html");
        Directory.CreateDirectory(Path.GetDirectoryName(primaryPath)!);
        File.WriteAllText(primaryPath, "primary page");

        RedirectWriter.Write(
            "/study/",
            "https://ufcpp.net/study/",
            ["/study/index.html"],
            tempDirectory.Path);

        Assert.Equal("primary page", File.ReadAllText(primaryPath));
    }

    [Fact]
    public void Write_NoIndexOption_ControlsRobotsMeta()
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/target/",
            "https://ufcpp.net/target/",
            ["/indexable/"],
            tempDirectory.Path);
        RedirectWriter.Write(
            "/target/",
            "https://ufcpp.net/target/",
            ["/noindex/"],
            tempDirectory.Path,
            noIndex: true);

        var indexableHtml = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "indexable",
            "index.html"));
        var noIndexHtml = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "noindex",
            "index.html"));
        Assert.DoesNotContain("<meta name=\"robots\"", indexableHtml);
        Assert.Contains(
            """<meta name="robots" content="noindex, nofollow" />""",
            noIndexHtml);
    }

    [Fact]
    public void Write_Always_PreservesIncomingFragment()
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/study/csharp/oop/oo_interface/",
            "https://ufcpp.net/study/csharp/oop/oo_interface/",
            ["/csharp/oo_interface.html"],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "csharp",
            "oo_interface.html"));

        // Legacy links such as /csharp/oo_interface.html?p=6#static-abstract must keep
        // their anchor when the single-page output is reached.
        Assert.Contains("location.replace(target + location.hash)", html);
        Assert.Contains("""var target = "../study/csharp/oop/oo_interface/";""", html);
        Assert.DoesNotContain("location.search", html);
    }

    [Fact]
    public void Write_Always_KeepsMetaRefreshFallbackForNoScript()
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/target/",
            "https://ufcpp.net/target/",
            ["/legacy/"],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "legacy",
            "index.html"));

        Assert.Contains(
            """<noscript><meta http-equiv="refresh" content="0; url=../target/" /></noscript>""",
            html);
        Assert.Contains(
            """<link rel="canonical" href="https://ufcpp.net/target/" />""",
            html);
    }

    [Fact]
    public void Write_TargetWithHtmlSensitiveCharacters_EscapesBothMarkupAndScript()
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/target/a&b/",
            "https://ufcpp.net/target/a&b/",
            ["/legacy/"],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "legacy",
            "index.html"));

        Assert.Contains("href=\"../target/a&amp;b/\"", html);
        Assert.Contains(
            "href=\"https://ufcpp.net/target/a&amp;b/\"",
            html);
        Assert.Contains("""var target = "../target/a\u0026b/";""", html);
        Assert.DoesNotContain("href=\"../target/a&b/\"", html);
    }

    [Theory]
    [InlineData("/legacy%23part/", "legacy#part")]
    [InlineData("/legacy%25part/", "legacy%part")]
    public void Write_EncodedAlias_KeepsEncodedPublicBasePath(
        string alias,
        string outputDirectoryName)
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/target/",
            "https://ufcpp.net/target/",
            [alias],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            outputDirectoryName,
            "index.html"));
        Assert.Contains("""var target = "../target/";""", html);
    }

    [Theory]
    [InlineData("/legacy/index.html")]
    [InlineData("/legacy/%69ndex.html")]
    public void Write_IndexFileAlias_UsesFileUrlAsRelativeBase(string alias)
    {
        using var tempDirectory = new TempDirectory();

        RedirectWriter.Write(
            "/target/",
            "https://ufcpp.net/target/",
            [alias],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "legacy",
            "index.html"));
        Assert.Contains("""var target = "../target/";""", html);
    }
}
