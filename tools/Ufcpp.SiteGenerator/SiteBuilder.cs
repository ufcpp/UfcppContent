using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ufcpp.SiteGenerator.Loading;
using Ufcpp.SiteGenerator.Models;
using Ufcpp.SiteGenerator.Output;
using Ufcpp.SiteGenerator.Rendering;
using Ufcpp.SiteGenerator.Templates;
using Ufcpp.SiteGenerator.Validation;

namespace Ufcpp.SiteGenerator;

/// <summary>Orchestrates the full static site generation process.</summary>
public sealed class SiteBuilder
{
    private readonly CliOptions _options;
    private readonly ILogger _logger;

    public SiteBuilder(CliOptions options, ILogger logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task BuildAsync()
    {
        var outputDirectory = new DirectoryInfo(Path.GetFullPath(_options.OutputDirectory));
        var outputParent = outputDirectory.Parent
            ?? throw new InvalidOperationException(
                "The site output directory cannot be a filesystem root.");
        Directory.CreateDirectory(outputParent.FullName);

        var stagingDirectory = Path.Combine(
            outputParent.FullName,
            $".{outputDirectory.Name}.{Guid.NewGuid():N}.tmp");
        var stagingOptions = new CliOptions
        {
            ContentDirectory = _options.ContentDirectory,
            AssetsDirectory = _options.AssetsDirectory,
            OutputDirectory = stagingDirectory,
            SkipValidation = _options.SkipValidation,
            IncludePreviewServer = _options.IncludePreviewServer,
            NoIndex = _options.NoIndex,
        };

        try
        {
            await new SiteBuilder(stagingOptions, _logger).BuildInPlaceAsync();
            ReplaceOutputDirectory(stagingDirectory, outputDirectory.FullName);
        }
        finally
        {
            if (Directory.Exists(stagingDirectory))
            {
                TryDeleteBuildDirectory(stagingDirectory, "staging");
            }
        }

        _logger.LogInformation(
            "Site generation complete. Output: '{OutputDir}'",
            outputDirectory.FullName);
    }

    private async Task BuildInPlaceAsync()
    {
        _logger.LogInformation("Loading pages from '{ContentDir}'...", _options.ContentDirectory);

        var (pages, urlMap) = PageLoader.Load(_options.ContentDirectory);
        _logger.LogInformation("Loaded {Count} pages.", pages.Count);

        ValidateOutputClaims(pages);
        var pagesById = BuildPageIndex(pages);
        var studyNavigationById = BuildStudyPageNavigation(pages, pagesById);
        var knownSiteOutputs = BuildKnownSiteOutputs(pages);

        // Set up Razor HtmlRenderer
        var services = new ServiceCollection();
        services.AddLogging();
        var sp = services.BuildServiceProvider();
        await using var htmlRenderer = new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>());

        var markdigRenderer = new MarkdigRenderer(
            _options.ContentDirectory,
            _options.AssetsDirectory);

        Directory.CreateDirectory(_options.OutputDirectory);

        _logger.LogInformation("Rendering pages...");

        foreach (var page in pages)
        {
            await RenderPageAsync(
                page,
                pagesById,
                studyNavigationById,
                urlMap,
                knownSiteOutputs,
                markdigRenderer,
                htmlRenderer);
        }

        _logger.LogInformation("Copying assets from '{AssetsDir}'...", _options.AssetsDirectory);
        AssetCopier.Copy(_options.AssetsDirectory, _options.OutputDirectory);

        // Copy site CSS
        var cssSourcePath = GetSiteCssPath();
        if (File.Exists(cssSourcePath))
        {
            AssetCopier.CopySiteCss(cssSourcePath, _options.OutputDirectory);
        }
        else
        {
            _logger.LogWarning("Site CSS not found at '{Path}'. Skipping.", cssSourcePath);
        }

        _logger.LogInformation("Writing aliases (redirects)...");
        foreach (var page in pages)
        {
            RedirectWriter.Write(
                page.CanonicalPath,
                page.FrontMatter.SourceUrl,
                page.FrontMatter.Aliases,
                _options.OutputDirectory,
                _options.NoIndex);
        }

        _logger.LogInformation("Writing sitemap.xml...");
        SitemapWriter.Write(pages, _options.OutputDirectory);

        _logger.LogInformation("Writing rssfeed.xml...");
        RssWriter.Write(pages, _options.OutputDirectory);

        if (_options.IncludePreviewServer)
        {
            _logger.LogInformation("Writing server.cs...");
            await PreviewServerWriter.WriteAsync(_options.OutputDirectory);
        }

        if (!_options.SkipValidation)
        {
            _logger.LogInformation("Validating output...");
            var validator = new OutputValidator(_options.OutputDirectory, pages, urlMap);
            validator.Validate();
        }
    }

    private async Task RenderPageAsync(
        ContentPage page,
        IReadOnlyDictionary<int, ContentPage> pagesById,
        IReadOnlyDictionary<int, PageNavigation> studyNavigationById,
        IReadOnlyDictionary<string, string> urlMap,
        IReadOnlySet<string> knownSiteOutputs,
        MarkdigRenderer markdigRenderer,
        HtmlRenderer htmlRenderer)
    {
        var renderedContent = markdigRenderer.RenderWithMetadata(
            page,
            urlMap,
            knownSiteOutputs);

        var pageTitle = BuildPageTitle(page);
        var contentTypeClass = GetContentTypeClass(page.FrontMatter.ContentType);
        var showRss = page.FrontMatter.ContentType is "BlogTop" or "BlogYear" or "BlogMonth";
        var breadcrumbs = BuildBreadcrumbs(page, pagesById);
        var isArticle = page.FrontMatter.ContentType == "Article";
        var isBlogEntry = page.FrontMatter.ContentType == "BlogEntry";
        studyNavigationById.TryGetValue(
            page.FrontMatter.UmbracoId,
            out var studyNavigation);

        var fullHtml = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                [nameof(SiteLayout.PageTitle)] = pageTitle,
                [nameof(SiteLayout.CurrentPath)] = page.CanonicalPath,
                [nameof(SiteLayout.CanonicalUrl)] = page.FrontMatter.SourceUrl,
                [nameof(SiteLayout.TitleHtml)] = renderedContent.TitleHtml,
                [nameof(SiteLayout.BodyHtml)] = renderedContent.BodyHtml,
                [nameof(SiteLayout.ContentTypeClass)] = contentTypeClass,
                [nameof(SiteLayout.ShowRssFeed)] = showRss,
                [nameof(SiteLayout.NoIndex)] = _options.NoIndex,
                [nameof(SiteLayout.IsArticle)] = isArticle,
                [nameof(SiteLayout.Breadcrumbs)] = breadcrumbs,
                [nameof(SiteLayout.TableOfContents)] = isArticle
                    ? renderedContent.TableOfContents
                    : [],
                [nameof(SiteLayout.Keywords)] = isArticle
                    ? renderedContent.Keywords
                    : [],
                [nameof(SiteLayout.PreviousPage)] = studyNavigation?.Previous,
                [nameof(SiteLayout.NextPage)] = studyNavigation?.Next,
                [nameof(SiteLayout.PublishedAt)] = isArticle || isBlogEntry
                    ? page.FrontMatter.PublishedAt
                    : null,
                [nameof(SiteLayout.UpdatedAt)] = isArticle || isBlogEntry
                    ? page.FrontMatter.UpdatedAt
                    : null,
                [nameof(SiteLayout.Tags)] = isBlogEntry
                    ? page.FrontMatter.Tags
                    : [],
            });

            var output = await htmlRenderer.RenderComponentAsync<SiteLayout>(parameters);
            return output.ToHtmlString();
        });

        var destFile = Path.Combine(
            _options.OutputDirectory,
            page.OutputPath.Replace('/', Path.DirectorySeparatorChar));

        Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
        await File.WriteAllTextAsync(destFile, fullHtml, System.Text.Encoding.UTF8);
    }

    private static string BuildPageTitle(ContentPage page)
    {
        const string SiteName = "++C++; // 未確認飛行 C";

        if (page.FrontMatter.ContentType == "Home")
        {
            return SiteName;
        }

        var title = page.FrontMatter.Title;
        if (string.IsNullOrWhiteSpace(title))
        {
            return SiteName;
        }

        return $"{title} | {SiteName}";
    }

    private static string GetContentTypeClass(string contentType) =>
        contentType.ToLowerInvariant() switch
        {
            "article" => "article",
            "blogentry" => "blog-entry",
            "blogtop" or "blogyear" or "blogmonth" => "blog-index",
            "subject" => "subject",
            "chapter" => "chapter",
            "studytop" => "study-top",
            "home" => "home",
            "search" => "search",
            "sitemap" => "sitemap",
            "aboutme" => "about",
            _ => "",
        };

    private static IReadOnlyDictionary<int, ContentPage> BuildPageIndex(
        IReadOnlyList<ContentPage> pages)
    {
        var pagesById = new Dictionary<int, ContentPage>();
        foreach (var page in pages)
        {
            if (!pagesById.TryAdd(page.FrontMatter.UmbracoId, page))
            {
                throw new InvalidDataException(
                    $"Duplicate Umbraco ID {page.FrontMatter.UmbracoId} in '{page.RelativePath}'.");
            }
        }

        return pagesById;
    }

    private static IReadOnlyDictionary<int, PageNavigation> BuildStudyPageNavigation(
        IReadOnlyList<ContentPage> pages,
        IReadOnlyDictionary<int, ContentPage> pagesById)
    {
        var childrenByParentId = pages
            .GroupBy(page => page.FrontMatter.ParentId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderBy(page => page.FrontMatter.SortOrder)
                    .ThenBy(
                        page => page.RelativePath,
                        StringComparer.OrdinalIgnoreCase)
                    .ThenBy(page => page.RelativePath, StringComparer.Ordinal)
                    .ToArray());
        var navigationById = new Dictionary<int, PageNavigation>();

        foreach (var subject in pages
                     .Where(page => page.FrontMatter.ContentType == "Subject")
                     .OrderBy(
                         page => page.RelativePath,
                         StringComparer.OrdinalIgnoreCase)
                     .ThenBy(page => page.RelativePath, StringComparer.Ordinal))
        {
            var studyPages = EnumerateNavigableStudyPages(
                    subject,
                    childrenByParentId)
                .ToArray();

            for (var index = 0; index < studyPages.Length; index++)
            {
                var current = studyPages[index];
                var previous = index > 0
                    ? BuildStudyPageNavigationItem(
                        current,
                        studyPages[index - 1],
                        pagesById)
                    : null;
                var next = index < studyPages.Length - 1
                    ? BuildStudyPageNavigationItem(
                        current,
                        studyPages[index + 1],
                        pagesById)
                    : null;

                navigationById.Add(
                    current.FrontMatter.UmbracoId,
                    new PageNavigation(previous, next));
            }
        }

        return navigationById;
    }

    private static IEnumerable<ContentPage> EnumerateNavigableStudyPages(
        ContentPage subject,
        IReadOnlyDictionary<int, ContentPage[]> childrenByParentId)
    {
        var ancestors = new HashSet<int>();

        foreach (var page in EnumerateChildren(subject))
        {
            yield return page;
        }

        IEnumerable<ContentPage> EnumerateChildren(ContentPage parent)
        {
            var parentId = parent.FrontMatter.UmbracoId;
            if (!ancestors.Add(parentId))
            {
                throw new InvalidDataException(
                    $"Content hierarchy cycle detected while building study page navigation for '{subject.RelativePath}'.");
            }

            try
            {
                if (!childrenByParentId.TryGetValue(parentId, out var children))
                {
                    yield break;
                }

                foreach (var child in children)
                {
                    if (child.FrontMatter.ContentType == "Subject")
                    {
                        continue;
                    }

                    if (child.FrontMatter.ContentType is "Article" or "ExerciseList")
                    {
                        yield return child;
                    }

                    foreach (var page in EnumerateChildren(child))
                    {
                        yield return page;
                    }
                }
            }
            finally
            {
                ancestors.Remove(parentId);
            }
        }
    }

    private static NavigationItem BuildStudyPageNavigationItem(
        ContentPage current,
        ContentPage target,
        IReadOnlyDictionary<int, ContentPage> pagesById)
    {
        var title = target.FrontMatter.Title;
        if (current.FrontMatter.ParentId != target.FrontMatter.ParentId)
        {
            if (!pagesById.TryGetValue(target.FrontMatter.ParentId, out var parent))
            {
                throw new InvalidDataException(
                    $"Parent ID {target.FrontMatter.ParentId} for '{target.RelativePath}' does not exist.");
            }

            title = $"【{parent.FrontMatter.Title}】 {title}";
        }

        return new NavigationItem(target.CanonicalPath, title);
    }

    private static IReadOnlySet<string> BuildKnownSiteOutputs(
        IReadOnlyList<ContentPage> pages) =>
        pages
            .SelectMany(page =>
                new[] { page.CanonicalPath }.Concat(page.FrontMatter.Aliases))
            .Append("/sitemap.xml")
            .Append("/rssfeed.xml")
            .Select(OutputPathResolver.Resolve)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static IReadOnlyList<NavigationItem> BuildBreadcrumbs(
        ContentPage page,
        IReadOnlyDictionary<int, ContentPage> pagesById)
    {
        var pages = new Stack<ContentPage>();
        var visitedIds = new HashSet<int>();
        var current = page;

        while (true)
        {
            var currentId = current.FrontMatter.UmbracoId;
            if (!visitedIds.Add(currentId))
            {
                throw new InvalidDataException(
                    $"Content hierarchy cycle detected while building breadcrumbs for '{page.RelativePath}'.");
            }

            pages.Push(current);
            var parentId = current.FrontMatter.ParentId;
            if (parentId == -1)
            {
                break;
            }

            var child = current;
            if (!pagesById.TryGetValue(parentId, out var parent))
            {
                throw new InvalidDataException(
                    $"Parent ID {parentId} for '{child.RelativePath}' does not exist.");
            }

            current = parent;
        }

        return pages
            .Where(candidate =>
                ReferenceEquals(candidate, page)
                || candidate.FrontMatter.ContentType != "StudyTop")
            .Select(candidate => new NavigationItem(
                candidate.CanonicalPath,
                candidate.FrontMatter.ContentType == "Home"
                    ? "TOP"
                    : candidate.FrontMatter.Title))
            .ToArray();
    }

    private string GetSiteCssPath()
    {
        // Look for site.css relative to the tool's location
        var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        return Path.Combine(assemblyDir, "wwwroot", "css", "site.css");
    }

    private void ValidateOutputClaims(IReadOnlyList<ContentPage> pages)
    {
        var contentOutputs = pages
            .SelectMany(page => new[] { page.OutputPath }
                .Concat(page.FrontMatter.Aliases.Select(OutputPathResolver.Resolve)))
            .Select(NormalizeOutputPath)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var reservedOutputs = new List<string>
        {
            "sitemap.xml",
            "rssfeed.xml",
        };
        if (_options.IncludePreviewServer)
        {
            reservedOutputs.Add(PreviewServerWriter.OutputPath);
        }

        foreach (var contentOutput in contentOutputs)
        {
            var reserved = reservedOutputs.FirstOrDefault(path =>
                PathsConflict(contentOutput, path));
            if (reserved is not null)
            {
                throw new InvalidDataException(
                    $"Output path collision: '{contentOutput}' conflicts with generated artifact '{reserved}'.");
            }
        }

        var sourceAssetOutputs = Directory
            .EnumerateFiles(_options.AssetsDirectory, "*", SearchOption.AllDirectories)
            .Select(path => "assets/" + Path
                .GetRelativePath(_options.AssetsDirectory, path)
                .Replace('\\', '/'))
            .ToArray();

        if (sourceAssetOutputs.Contains(
            "assets/css/site.css",
            StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidDataException(
                "Output path collision: 'assets/css/site.css' is claimed by both a source asset and the site stylesheet.");
        }

        var assetOutputs = sourceAssetOutputs
            .Append("assets/css/site.css")
            .ToArray();

        foreach (var contentOutput in contentOutputs)
        {
            var conflictingAsset = assetOutputs.FirstOrDefault(assetOutput =>
                PathsConflict(contentOutput, assetOutput));
            if (conflictingAsset is not null)
            {
                throw new InvalidDataException(
                    $"Output path collision: '{contentOutput}' conflicts with asset '{conflictingAsset}'.");
            }
        }
    }

    private static bool PathsConflict(string contentOutput, string assetOutput) =>
        string.Equals(contentOutput, assetOutput, StringComparison.OrdinalIgnoreCase)
        || contentOutput.StartsWith(
            assetOutput.TrimEnd('/') + "/",
            StringComparison.OrdinalIgnoreCase)
        || assetOutput.StartsWith(
            contentOutput.TrimEnd('/') + "/",
            StringComparison.OrdinalIgnoreCase);

    private static string NormalizeOutputPath(string path) =>
        path.Replace('\\', '/').TrimStart('/');

    private void ReplaceOutputDirectory(
        string stagingDirectory,
        string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.Move(stagingDirectory, outputDirectory);
            return;
        }

        var output = new DirectoryInfo(outputDirectory);
        var backupDirectory = Path.Combine(
            output.Parent!.FullName,
            $".{output.Name}.{Guid.NewGuid():N}.bak");

        Directory.Move(outputDirectory, backupDirectory);
        try
        {
            Directory.Move(stagingDirectory, outputDirectory);
        }
        catch
        {
            Directory.Move(backupDirectory, outputDirectory);
            throw;
        }

        TryDeleteBuildDirectory(backupDirectory, "backup");
    }

    private void TryDeleteBuildDirectory(string path, string kind)
    {
        try
        {
            Directory.Delete(path, recursive: true);
        }
        catch (IOException exception)
        {
            _logger.LogWarning(
                exception,
                "Could not delete the site build {Kind} directory '{Path}'.",
                kind,
                path);
        }
        catch (UnauthorizedAccessException exception)
        {
            _logger.LogWarning(
                exception,
                "Could not delete the site build {Kind} directory '{Path}'.",
                kind,
                path);
        }
    }

    private sealed record PageNavigation(
        NavigationItem? Previous,
        NavigationItem? Next);
}
