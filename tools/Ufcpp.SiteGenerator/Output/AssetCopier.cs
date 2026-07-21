namespace Ufcpp.SiteGenerator.Output;

/// <summary>Copies static assets from the source assets directory to the site output directory.</summary>
public static class AssetCopier
{
    /// <summary>
    /// Copies all files from <paramref name="assetsSourceDirectory"/> to
    /// <c>assets/</c> inside <paramref name="outputDirectory"/>, preserving bytes exactly.
    /// </summary>
    public static void Copy(string assetsSourceDirectory, string outputDirectory)
    {
        var destination = Path.Combine(outputDirectory, "assets");
        Directory.CreateDirectory(destination);

        foreach (var sourceFile in Directory
            .EnumerateFiles(assetsSourceDirectory, "*", SearchOption.AllDirectories)
            .OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
        {
            var relative = Path.GetRelativePath(assetsSourceDirectory, sourceFile);
            var destFile = Path.Combine(destination, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
            File.Copy(sourceFile, destFile, overwrite: true);
        }
    }

    /// <summary>
    /// Copies the site CSS file to <c>assets/css/site.css</c> in the output directory.
    /// </summary>
    public static void CopySiteCss(string cssSourcePath, string outputDirectory)
    {
        var destDir = Path.Combine(outputDirectory, "assets", "css");
        Directory.CreateDirectory(destDir);
        File.Copy(cssSourcePath, Path.Combine(destDir, "site.css"), overwrite: true);
    }
}
