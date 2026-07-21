using Microsoft.Extensions.Logging;
using Ufcpp.SiteGenerator;

var contentDir = GetArg("--content", "content");
var assetsDir = GetArg("--assets", "assets");
var outputDir = GetArg("--output", "_site");
var skipValidation = args.Contains("--skip-validation");
var includePreviewServer = args.Contains("--include-preview-server");

using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddSimpleConsole(o =>
    {
        o.SingleLine = true;
        o.TimestampFormat = "HH:mm:ss ";
    }));

var logger = loggerFactory.CreateLogger("SiteGenerator");

// Resolve relative paths against current working directory
contentDir = Path.GetFullPath(contentDir);
assetsDir = Path.GetFullPath(assetsDir);
outputDir = Path.GetFullPath(outputDir);

if (!Directory.Exists(contentDir))
{
    logger.LogError("Content directory not found: '{ContentDir}'", contentDir);
    return 1;
}

if (!Directory.Exists(assetsDir))
{
    logger.LogError("Assets directory not found: '{AssetsDir}'", assetsDir);
    return 1;
}

var options = new CliOptions
{
    ContentDirectory = contentDir,
    AssetsDirectory = assetsDir,
    OutputDirectory = outputDir,
    SkipValidation = skipValidation,
    IncludePreviewServer = includePreviewServer,
};

try
{
    var builder = new SiteBuilder(options, logger);
    await builder.BuildAsync();
    return 0;
}
catch (AggregateException ex)
{
    logger.LogError("Generation failed:");
    foreach (var inner in ex.InnerExceptions)
    {
        logger.LogError("  {Message}", inner.Message);
    }

    return 1;
}
catch (Exception ex)
{
    logger.LogError(ex, "Unexpected error during site generation.");
    return 1;
}

string GetArg(string flag, string defaultValue)
{
    var index = Array.IndexOf(args, flag);
    return index >= 0 && index + 1 < args.Length ? args[index + 1] : defaultValue;
}
