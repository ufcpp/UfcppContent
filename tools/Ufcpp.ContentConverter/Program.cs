using Ufcpp.ContentConverter;

try
{
    var options = CliOptions.Parse(args);
    var report = new ContentMigration(options).Run();
    Console.WriteLine(
        $"Generated {report.MarkdownOutputs} Markdown files and {report.ReferencedAssets} assets " +
        $"from {report.NodeCount} nodes. Sitemap: {report.CanonicalSitemapMatches}/{report.SitemapUrls}.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine($"error: {exception.Message}");
    return 1;
}
