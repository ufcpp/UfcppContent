using System.Text;

namespace Ufcpp.ContentConverter;

public sealed record AssetRecord(
    string OriginalUrl,
    string OutputPath,
    string SourceKind,
    string SourceRelativePath,
    long Bytes,
    string Sha256);

public sealed class AssetManager
{
    private static readonly IReadOnlyDictionary<string, string> LegacyUrlCorrections =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["/media/ufcpp2000/dsl/ClientBin/StackMachine.xap"] =
                "/media/ufcpp2000/csharp/ClientBin/StackMachine.xap",
        };

    private readonly string _mediaRoot;
    private readonly string _legacyRoot;
    private readonly string _outputRoot;
    private readonly Dictionary<string, AssetRecord> _records =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> _outputSources =
        new(StringComparer.OrdinalIgnoreCase);

    public AssetManager(string mediaRoot, string legacyRoot, string outputRoot)
    {
        _mediaRoot = Path.GetFullPath(mediaRoot);
        _legacyRoot = Path.GetFullPath(legacyRoot);
        _outputRoot = Path.GetFullPath(outputRoot);
    }

    public IReadOnlyCollection<AssetRecord> Records => _records.Values;

    public bool LooksLikeAsset(string sitePath)
    {
        var path = Uri.UnescapeDataString(sitePath);
        return path.StartsWith("/media/", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/images/", StringComparison.OrdinalIgnoreCase)
            || Path.HasExtension(path);
    }

    public string ResolveAndCopy(string sitePath)
    {
        var normalizedUrl = NormalizeAssetUrl(sitePath);
        if (_records.TryGetValue(normalizedUrl, out var existing))
        {
            return existing.OutputPath;
        }

        var decoded = Uri.UnescapeDataString(normalizedUrl).TrimStart('/');
        var segments = decoded.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            throw new InvalidDataException($"Invalid asset URL '{sitePath}'.");
        }

        foreach (var segment in segments)
        {
            ContentPaths.ValidateSegment(segment);
        }

        var source = FindSource(decoded);
        if (source is null)
        {
            throw new FileNotFoundException($"Referenced internal asset was not found: '{sitePath}'.");
        }

        var outputPath = "assets/" + string.Join('/', segments);
        var collisionKey = outputPath.Normalize(NormalizationForm.FormC);
        if (_outputSources.TryGetValue(collisionKey, out var collisionSource)
            && !string.Equals(collisionSource, source.Value.Path, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException(
                $"Asset output collision at '{outputPath}' from '{collisionSource}' and '{source.Value.Path}'.");
        }

        _outputSources[collisionKey] = source.Value.Path;
        var destination = Path.Combine(_outputRoot, outputPath.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        File.Copy(source.Value.Path, destination, true);

        var info = new FileInfo(destination);
        var record = new AssetRecord(
            normalizedUrl,
            outputPath,
            source.Value.Kind,
            source.Value.RelativePath.Replace('\\', '/'),
            info.Length,
            TextUtilities.Sha256File(destination));
        _records.Add(normalizedUrl, record);
        return outputPath;
    }

    private (string Path, string Kind, string RelativePath)? FindSource(string decoded)
    {
        if (decoded.StartsWith("media/", StringComparison.OrdinalIgnoreCase))
        {
            var mediaRelative = decoded["media/".Length..].Replace('/', Path.DirectorySeparatorChar);
            var mediaPath = SafeCombine(_mediaRoot, mediaRelative);
            if (File.Exists(mediaPath))
            {
                return (mediaPath, "media-archive", decoded["media/".Length..]);
            }

            var recovered = FindRecoveredMedia(decoded);
            if (recovered is not null)
            {
                return recovered;
            }

            return null;
        }

        var relative = decoded.Replace('/', Path.DirectorySeparatorChar);
        var archiveMatches = Directory.EnumerateFiles(
                _mediaRoot,
                Path.GetFileName(relative),
                SearchOption.AllDirectories)
            .Where(path => Path.GetRelativePath(_mediaRoot, path)
                .Replace('\\', '/')
                .EndsWith(decoded, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (archiveMatches.Length == 1)
        {
            return (
                archiveMatches[0],
                "media-archive",
                Path.GetRelativePath(_mediaRoot, archiveMatches[0]).Replace('\\', '/'));
        }

        if (archiveMatches.Length > 1)
        {
            throw new InvalidDataException($"Referenced asset path is ambiguous: '{decoded}'.");
        }

        var candidates = new[]
        {
            (
                Root: Path.Combine(_legacyRoot, "Liszt", "Liszt.Umbraco"),
                Relative: relative,
                Kind: "legacy-web-root"),
            (
                Root: Path.Combine(_legacyRoot, "元", "ufcpp.net"),
                Relative: relative,
                Kind: "legacy-site-root"),
            (
                Root: Path.Combine(_legacyRoot, "元", "ufcpp.net", "study", "algorithm"),
                Relative: relative,
                Kind: "legacy-algorithm-root"),
            (
                Root: Path.Combine(_legacyRoot, "元", "ufcpp.net", "study", "csharp"),
                Relative: relative,
                Kind: "legacy-csharp-root"),
            (
                Root: Path.Combine(_legacyRoot, "元", "ufcpp.net", "study", "dotnet"),
                Relative: relative,
                Kind: "legacy-dotnet-root"),
            (
                Root: Path.Combine(_legacyRoot, "Tools", "media", "ufcpp2000", "algorithm"),
                Relative: relative,
                Kind: "legacy-media-root"),
            (
                Root: Path.Combine(_legacyRoot, "Tools", "media", "ufcpp2000", "csharp"),
                Relative: relative,
                Kind: "legacy-media-root"),
        };
        foreach (var candidate in candidates)
        {
            var path = SafeCombine(candidate.Root, candidate.Relative);
            if (File.Exists(path))
            {
                return (path, candidate.Kind, decoded);
            }
        }

        return null;
    }

    private (string Path, string Kind, string RelativePath)? FindRecoveredMedia(string decoded)
    {
        var mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["media/ufcpp2000/en/source/Differential.zip"] =
                "元/ufcpp.net/study/csharp/source/Differential.zip",
            ["media/ufcpp2000/xml/vsxml.jpg"] =
                "元/ufcpp.net/study/testxsl/vsxml.jpg",
            ["media/ufcpp2000/xml/xslfiles/nomenu.xsl"] =
                "元/ufcpp.net/study/testxsl/nomenu.xsl",
        };
        if (!mappings.TryGetValue(decoded, out var relative))
        {
            return null;
        }

        var path = SafeCombine(_legacyRoot, relative.Replace('/', Path.DirectorySeparatorChar));
        return File.Exists(path) ? (path, "legacy-recovery", relative) : null;
    }

    private static string SafeCombine(string root, string relative)
    {
        var fullRoot = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        var fullPath = Path.GetFullPath(Path.Combine(fullRoot, relative));
        if (!fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException($"Asset path escapes its source root: '{relative}'.");
        }

        return fullPath;
    }

    private static string NormalizeAssetUrl(string path)
    {
        path = path.Replace('\\', '/');
        var suffix = path.IndexOfAny(['?', '#']);
        if (suffix >= 0)
        {
            path = path[..suffix];
        }

        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        return LegacyUrlCorrections.GetValueOrDefault(path, path);
    }
}
