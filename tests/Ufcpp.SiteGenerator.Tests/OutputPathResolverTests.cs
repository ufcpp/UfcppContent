using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class OutputPathResolverTests
{
    [Theory]
    [InlineData("/", "index.html")]
    [InlineData("/study/csharp/", "study/csharp/index.html")]
    [InlineData("/blog/2025/1/first-class-span/", "blog/2025/1/first-class-span/index.html")]
    [InlineData("/about_me/", "about_me/index.html")]
    [InlineData("/blog/", "blog/index.html")]
    [InlineData("/sitemap/", "sitemap/index.html")]
    [InlineData(
        "/blog/2025/5/%E3%83%95%E3%82%A1%E3%82%A4%E3%83%8A%E3%83%A9%E3%82%A4%E3%82%B6%E3%83%BC/",
        "blog/2025/5/ファイナライザー/index.html")]
    public void Resolve_ReturnsCorrectOutputPath(string canonicalPath, string expectedOutput)
    {
        var result = OutputPathResolver.Resolve(canonicalPath);
        Assert.Equal(expectedOutput, result);
    }

    [Theory]
    [InlineData("/../../outside.html")]
    [InlineData("/%2E%2E/outside.html")]
    [InlineData("/C:/outside.html")]
    [InlineData("/safe/%2Foutside.html")]
    [InlineData("/safe\\outside.html")]
    public void Resolve_UnsafeOutputPath_Throws(string canonicalPath)
    {
        var exception = Assert.Throws<InvalidDataException>(
            () => OutputPathResolver.Resolve(canonicalPath));

        Assert.Contains("Invalid output path", exception.Message);
    }

    [Theory]
    [InlineData("https://ufcpp.net/", "/")]
    [InlineData("https://ufcpp.net/study/csharp/", "/study/csharp/")]
    [InlineData("https://ufcpp.net/blog/2025/1/first-class-span/", "/blog/2025/1/first-class-span/")]
    [InlineData("https://ufcpp.net/about_me/", "/about_me/")]
    [InlineData(
        "https://ufcpp.net/blog/2025/5/%E3%83%95%E3%82%A1%E3%82%A4%E3%83%8A%E3%83%A9%E3%82%A4%E3%82%B6%E3%83%BC/",
        "/blog/2025/5/%E3%83%95%E3%82%A1%E3%82%A4%E3%83%8A%E3%83%A9%E3%82%A4%E3%82%B6%E3%83%BC/")]
    public void ExtractCanonicalPath_ReturnsPathFromUrl(string sourceUrl, string expectedPath)
    {
        var result = OutputPathResolver.ExtractCanonicalPath(sourceUrl);
        Assert.Equal(expectedPath, result);
    }
}
