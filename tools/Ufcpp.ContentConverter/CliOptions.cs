namespace Ufcpp.ContentConverter;

public sealed record MigrationOptions(
    string SnapshotPath,
    string MediaRoot,
    string SitemapPath,
    string RewriteMapsPath,
    string LegacyRoot,
    string OutputRoot,
    bool StrictAccounting = true);

public static class CliOptions
{
    private static readonly string[] RequiredNames =
    [
        "--snapshot",
        "--media",
        "--sitemap",
        "--rewrite-maps",
        "--legacy-root",
        "--output",
    ];

    public static MigrationOptions Parse(string[] args)
    {
        if (args.Length == 0 || args.Contains("--help", StringComparer.Ordinal))
        {
            throw new ArgumentException(
                "Usage: Ufcpp.ContentConverter --snapshot <published.xml> --media <media-root> " +
                "--sitemap <sitemap.xml> --rewrite-maps <config> --legacy-root <source-repo> --output <repo-root>");
        }

        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var index = 0; index < args.Length; index += 2)
        {
            if (index + 1 >= args.Length || !args[index].StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Expected a value after '{args[index]}'.");
            }

            if (!RequiredNames.Contains(args[index], StringComparer.Ordinal))
            {
                throw new ArgumentException($"Unknown argument '{args[index]}'.");
            }

            if (!values.TryAdd(args[index], Path.GetFullPath(args[index + 1])))
            {
                throw new ArgumentException($"Argument '{args[index]}' was supplied more than once.");
            }
        }

        var missing = RequiredNames.Where(name => !values.ContainsKey(name)).ToArray();
        if (missing.Length != 0)
        {
            throw new ArgumentException($"Missing required arguments: {string.Join(", ", missing)}.");
        }

        RequireFile(values["--snapshot"]);
        RequireDirectory(values["--media"]);
        RequireFile(values["--sitemap"]);
        RequireFile(values["--rewrite-maps"]);
        RequireDirectory(values["--legacy-root"]);

        return new MigrationOptions(
            values["--snapshot"],
            values["--media"],
            values["--sitemap"],
            values["--rewrite-maps"],
            values["--legacy-root"],
            values["--output"]);
    }

    private static void RequireFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Input file does not exist: {path}");
        }
    }

    private static void RequireDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Input directory does not exist: {path}");
        }
    }
}
