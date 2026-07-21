using Microsoft.Extensions.Logging.Abstractions;
using Ufcpp.SiteGenerator.Loading;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class OutputCollisionTests
{
    [Theory]
    [InlineData("alias-primary", "beta/index.html")]
    [InlineData("alias-alias", "legacy/index.html")]
    public void Load_DistinctCanonicalTargetsClaimSameOutput_Throws(
        string collisionKind,
        string expectedOutputPath)
    {
        using var site = new SiteFixture();

        switch (collisionKind)
        {
            case "alias-primary":
                site.AddPage("a.md", "/alpha/", "/beta");
                site.AddPage("b.md", "/beta/");
                break;
            case "alias-alias":
                site.AddPage("a.md", "/alpha/", "/legacy");
                site.AddPage("b.md", "/beta/", "/legacy/");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(collisionKind));
        }

        var exception = Assert.Throws<InvalidDataException>(
            () => PageLoader.Load(site.ContentDirectory));

        Assert.Contains("collision", exception.Message.ToLowerInvariant());
        Assert.Contains(
            expectedOutputPath,
            exception.Message.Replace('\\', '/'),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task BuildAsync_EquivalentAliasesForSameCanonicalTarget_WritesOneRedirect()
    {
        using var site = new SiteFixture();
        site.AddPage("article.md", "/article/", "/legacy", "/legacy/");

        await site.BuildAsync();

        var redirectDirectory = Path.Combine(site.OutputDirectory, "legacy");
        var redirectFile = Assert.Single(
            Directory.EnumerateFiles(redirectDirectory, "*.html", SearchOption.AllDirectories));
        var html = File.ReadAllText(redirectFile);

        Assert.Contains("""<link rel="canonical" href="/article/" />""", html);
        Assert.Contains("""<meta http-equiv="refresh" content="0; url=/article/" />""", html);
    }

    [Fact]
    public async Task BuildAsync_AliasNeedsDirectoryAtCopiedAssetFilePath_ThrowsCollision()
    {
        using var site = new SiteFixture();
        site.AddPage("article.md", "/article/", "/assets/archive.zip/");
        site.AddAsset("archive.zip", [0x50, 0x4b, 0x03, 0x04]);

        var exception = await Assert.ThrowsAsync<InvalidDataException>(site.BuildAsync);

        Assert.Contains("collision", exception.Message.ToLowerInvariant());
        Assert.Contains(
            "assets/archive.zip",
            exception.Message.Replace('\\', '/'),
            StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/sitemap.xml")]
    [InlineData("/rssfeed.xml")]
    public async Task BuildAsync_PrimaryPageClaimsGeneratedArtifactPath_ThrowsCollision(
        string generatedPath)
    {
        using var site = new SiteFixture();
        site.AddPage("collision.md", generatedPath);

        var exception = await Assert.ThrowsAsync<InvalidDataException>(site.BuildAsync);

        Assert.Contains("collision", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(generatedPath.TrimStart('/'), exception.Message);
    }

    [Fact]
    public async Task BuildAsync_PrimaryPageClaimsEnabledPreviewServerPath_ThrowsCollision()
    {
        using var site = new SiteFixture();
        site.AddPage("collision.md", "/server.cs");

        var exception = await Assert.ThrowsAsync<InvalidDataException>(
            () => site.BuildAsync(includePreviewServer: true));

        Assert.Contains("collision", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("server.cs", exception.Message);
    }

    private sealed class SiteFixture : IDisposable
    {
        private readonly TempDirectory _tempDirectory = new();

        public SiteFixture()
        {
            ContentDirectory = Path.Combine(_tempDirectory.Path, "content");
            AssetsDirectory = Path.Combine(_tempDirectory.Path, "assets");
            OutputDirectory = Path.Combine(_tempDirectory.Path, "output");
            Directory.CreateDirectory(ContentDirectory);
            Directory.CreateDirectory(AssetsDirectory);
        }

        public string ContentDirectory { get; }

        public string AssetsDirectory { get; }

        public string OutputDirectory { get; }

        public void AddPage(string relativePath, string canonicalPath, params string[] aliases)
        {
            var aliasesYaml = aliases.Length == 0
                ? "aliases: []"
                : "aliases:\n" + string.Join(
                    '\n',
                    aliases.Select(alias => $"  - \"{alias}\""));
            var markdown = $$"""
                ---
                title: "{{relativePath}}"
                source_url: "https://ufcpp.net{{canonicalPath}}"
                content_type: "Article"
                published_at: "2025-07-20T12:34:56"
                updated_at: "2025-07-20T12:34:56"
                tags: []
                umbraco_id: 1
                parent_id: -1
                sort_order: 0
                {{aliasesYaml}}
                ---
                Body.
                """;
            var path = Path.Combine(
                ContentDirectory,
                relativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, markdown);
        }

        public void AddAsset(string relativePath, byte[] bytes)
        {
            var path = Path.Combine(
                AssetsDirectory,
                relativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllBytes(path, bytes);
        }

        public Task BuildAsync() => BuildAsync(includePreviewServer: false);

        public Task BuildAsync(bool includePreviewServer)
        {
            var options = new CliOptions
            {
                ContentDirectory = ContentDirectory,
                AssetsDirectory = AssetsDirectory,
                OutputDirectory = OutputDirectory,
                IncludePreviewServer = includePreviewServer,
                SkipValidation = true,
            };

            return new SiteBuilder(options, NullLogger.Instance).BuildAsync();
        }

        public void Dispose() => _tempDirectory.Dispose();
    }
}
