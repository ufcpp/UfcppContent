using Ufcpp.SiteGenerator.Output;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class SiteUrlResolverTests
{
    [Theory]
    [InlineData("/", "/assets/css/site.css", "assets/css/site.css")]
    [InlineData("/study/csharp/async/", "/assets/css/site.css", "../../../assets/css/site.css")]
    [InlineData(
        "/study/csharp/current/",
        "/study/csharp/sibling/?view=print#details",
        "../sibling/?view=print#details")]
    [InlineData(
        "/csharp/oo_interface.html",
        "/study/csharp/oop/oo_interface/",
        "../study/csharp/oop/oo_interface/")]
    [InlineData("/", "/", "./")]
    [InlineData("/study/page/", "/study/page/#details", "#details")]
    [InlineData(
        "/study/%E6%97%A5%E6%9C%AC/",
        "/study/%E6%97%A5%E6%9C%AC/next/",
        "next/")]
    public void MakeRelative_RootRelativeTarget_ReturnsPageRelativeUrl(
        string sourcePath,
        string targetUrl,
        string expected)
    {
        Assert.Equal(expected, SiteUrlResolver.MakeRelative(sourcePath, targetUrl));
    }

    [Theory]
    [InlineData("https://example.com/")]
    [InlineData("//cdn.example.com/site.css")]
    [InlineData("#details")]
    [InlineData("?view=print")]
    [InlineData("../already-relative/")]
    public void MakeRelative_NonRootRelativeTarget_ReturnsUnchanged(string targetUrl)
    {
        Assert.Equal(
            targetUrl,
            SiteUrlResolver.MakeRelative("/study/page/", targetUrl));
    }
}
