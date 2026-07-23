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
            ["/legacy/"],
            tempDirectory.Path);
        RedirectWriter.Write(
            "/second-target/",
            ["/legacy/"],
            tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            "legacy",
            "index.html"));
        Assert.Contains("href=\"/second-target/\"", html);
        Assert.DoesNotContain("/first-target/", html);
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
            ["/indexable/"],
            tempDirectory.Path);
        RedirectWriter.Write(
            "/target/",
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
}
