using Ufcpp.SiteGenerator.Rendering;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class LinkRewriterTests
{
    private static LinkRewriter CreateRewriter(
        TempDirectory tempDirectory,
        Dictionary<string, string>? urlMap = null)
    {
        var contentRoot = GetContentPath(tempDirectory);
        var currentFile = GetContentPath(
            tempDirectory,
            "study",
            "csharp",
            "async",
            "misc_asyncflow.md");

        urlMap ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [currentFile] = "/study/csharp/async/misc_asyncflow/",
            [GetContentPath(tempDirectory, "study", "csharp", "index.md")] = "/study/csharp/",
            [GetContentPath(tempDirectory, "blog", "2025", "1", "first-class-span", "index.md")] =
                "/blog/2025/1/first-class-span/",
        };

        return new LinkRewriter(contentRoot, currentFile, urlMap);
    }

    private static string GetContentPath(
        TempDirectory tempDirectory,
        params string[] relativePathSegments)
    {
        var path = Path.Combine(tempDirectory.Path, "content");
        foreach (var segment in relativePathSegments)
        {
            path = Path.Combine(path, segment);
        }

        return Path.GetFullPath(path);
    }

    [Fact]
    public void RewriteUrl_ExternalLink_ReturnsUnchanged()
    {
        using var tempDir = new TempDirectory();
        var rewriter = CreateRewriter(tempDir);
        Assert.Equal("https://example.com/", rewriter.RewriteUrl("https://example.com/"));
    }

    [Fact]
    public void RewriteUrl_FragmentOnly_ReturnsUnchanged()
    {
        using var tempDir = new TempDirectory();
        var rewriter = CreateRewriter(tempDir);
        Assert.Equal("#section", rewriter.RewriteUrl("#section"));
    }

    [Fact]
    public void RewriteUrl_AbsolutePath_ReturnsUnchanged()
    {
        using var tempDir = new TempDirectory();
        var rewriter = CreateRewriter(tempDir);
        Assert.Equal("/some/path/", rewriter.RewriteUrl("/some/path/"));
    }

    [Fact]
    public void RewriteUrl_RelativeMdLink_ResolvesToCanonicalUrl()
    {
        using var tempDir = new TempDirectory();

        // From content/study/csharp/async/misc_asyncflow.md,
        // a link to ../asyncvariation.md should resolve if in URL map
        var urlMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [GetContentPath(tempDir, "study", "csharp", "async", "misc_asyncflow.md")] =
                "/study/csharp/async/misc_asyncflow/",
            [GetContentPath(tempDir, "study", "csharp", "async", "asyncvariation.md")] =
                "/study/csharp/async/asyncvariation/",
        };
        var r = CreateRewriter(tempDir, urlMap);
        Assert.Equal("/study/csharp/async/asyncvariation/", r.RewriteUrl("asyncvariation.md"));
    }

    [Fact]
    public void RewriteUrl_AssetLink_ReturnsAbsoluteAssetPath()
    {
        using var tempDir = new TempDirectory();

        // From content/study/csharp/async/misc_asyncflow.md,
        // ../../../../assets/media/foo.zip should become /assets/media/foo.zip
        var urlMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var rewriter = CreateRewriter(tempDir, urlMap);

        var result = rewriter.RewriteUrl("../../../../assets/media/ufcpp2000/csharp/source/ShowDialogAsyncSample.zip");
        Assert.Equal("/assets/media/ufcpp2000/csharp/source/ShowDialogAsyncSample.zip", result);
    }

    [Fact]
    public void RewriteUrl_RelativeMdLinkWithFragment_ResolvesToCanonicalUrlWithFragment()
    {
        using var tempDir = new TempDirectory();

        var urlMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [GetContentPath(tempDir, "study", "csharp", "async", "misc_asyncflow.md")] =
                "/study/csharp/async/misc_asyncflow/",
            [GetContentPath(tempDir, "study", "csharp", "async", "asyncvariation.md")] =
                "/study/csharp/async/asyncvariation/",
        };
        var r = CreateRewriter(tempDir, urlMap);
        var result = r.RewriteUrl("asyncvariation.md#section1");
        Assert.Equal("/study/csharp/async/asyncvariation/#section1", result);
    }

    [Fact]
    public void RewriteUrl_ExistingRootRelativeLegacyAsset_RewritesAssetAndKeepsSiteUrl()
    {
        using var tempDir = new TempDirectory();

        var assetPath = Path.GetFullPath(Path.Combine(
            tempDir.Path,
            "assets",
            "media",
            "ufcpp2000",
            "csharp",
            "slide",
            "WcfDemo.pptx"));
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath)!);
        File.WriteAllText(assetPath, "test asset");

        var rewriter = CreateRewriter(
            tempDir,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

        var rewrittenAsset = rewriter.RewriteUrl("/media/ufcpp2000/csharp/slide/WcfDemo.pptx");
        var unchangedSiteUrl = rewriter.RewriteUrl("/study/csharp/");

        Assert.Equal(
            ("/assets/media/ufcpp2000/csharp/slide/WcfDemo.pptx", "/study/csharp/"),
            (rewrittenAsset, unchangedSiteUrl));
    }

    [Theory]
    [InlineData("asyncvariation.md?p=6#section1", "/study/csharp/async/asyncvariation/#section1")]
    [InlineData("asyncvariation.md?P=6", "/study/csharp/async/asyncvariation/")]
    [InlineData("asyncvariation.md?p=6&x=1", "/study/csharp/async/asyncvariation/?x=1")]
    [InlineData("/study/csharp/oo_interface.html?p=6#x", "/study/csharp/oo_interface.html#x")]
    public void RewriteUrl_LegacyPageQuery_IsDroppedAndFragmentKept(
        string input,
        string expected)
    {
        using var tempDir = new TempDirectory();

        var urlMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [GetContentPath(tempDir, "study", "csharp", "async", "misc_asyncflow.md")] =
                "/study/csharp/async/misc_asyncflow/",
            [GetContentPath(tempDir, "study", "csharp", "async", "asyncvariation.md")] =
                "/study/csharp/async/asyncvariation/",
        };
        var rewriter = CreateRewriter(tempDir, urlMap);

        Assert.Equal(expected, rewriter.RewriteUrl(input));
    }
}
