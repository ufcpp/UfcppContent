using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class NotFoundWriterTests
{
    [Fact]
    public void Write_Always_RetriesMixedCasePagePathInLowercase()
    {
        using var tempDirectory = new TempDirectory();

        NotFoundWriter.Write(tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            NotFoundWriter.OutputPath));
        Assert.Contains("var lowerPath = path.toLowerCase();", html);
        Assert.Contains("if (isPagePath && lowerPath !== path)", html);
        Assert.Contains(
            "location.replace(lowerPath + location.search + location.hash);",
            html);
    }

    [Fact]
    public void Write_Always_LimitsFallbackToPagePaths()
    {
        using var tempDirectory = new TempDirectory();

        NotFoundWriter.Write(tempDirectory.Path);

        var html = File.ReadAllText(Path.Combine(
            tempDirectory.Path,
            NotFoundWriter.OutputPath));
        Assert.Contains(
            """var isPagePath = path.endsWith("/") || !lastSegment.includes(".");""",
            html);
        Assert.Contains(
            """<meta name="robots" content="noindex, nofollow" />""",
            html);
    }
}
