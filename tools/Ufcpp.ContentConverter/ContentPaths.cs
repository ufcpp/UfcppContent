using System.Text;

namespace Ufcpp.ContentConverter;

public static class ContentPaths
{
    public static readonly HashSet<string> GeneratedTypes =
    [
        "Home",
        "StudyTop",
        "Subject",
        "Chapter",
        "Article",
        "ExerciseList",
        "BlogTop",
        "BlogYear",
        "BlogMonth",
        "BlogEntry",
        "AboutMe",
        "Search",
        "Sitemap",
    ];

    public static readonly HashSet<string> CanonicalSitemapTypes =
    [
        "Home",
        "AboutMe",
        "Subject",
        "Article",
        "ExerciseList",
        "BlogTop",
        "BlogYear",
        "BlogEntry",
        "Search",
        "Sitemap",
    ];

    public static readonly HashSet<string> SitemapSnapshotTypes =
    [
        "Home",
        "AboutMe",
        "Subject",
        "Article",
        "ExerciseList",
        "BlogTop",
        "BlogYear",
        "BlogEntry",
        "ErrorNotFound",
        "ErrorServer",
    ];

    private static readonly HashSet<string> IndexTypes =
    [
        "Home",
        "StudyTop",
        "Subject",
        "Chapter",
        "BlogTop",
        "BlogYear",
        "BlogMonth",
    ];

    private static readonly HashSet<string> ReservedNames =
        new(
            new[] { "CON", "PRN", "AUX", "NUL" }
                .Concat(Enumerable.Range(1, 9).Select(number => $"COM{number}"))
                .Concat(Enumerable.Range(1, 9).Select(number => $"LPT{number}")),
            StringComparer.OrdinalIgnoreCase);

    public static string CanonicalUrl(ContentNode node)
    {
        if (node.ContentType == "Home")
        {
            return "/";
        }

        var segments = node.AncestorsAndSelf()
            .Reverse()
            .Where(ancestor => ancestor.ContentType != "Home")
            .Select(ancestor => Uri.EscapeDataString(ancestor.UrlName));
        return $"/{string.Join('/', segments)}/";
    }

    public static string? OutputPath(ContentNode node)
    {
        if (!GeneratedTypes.Contains(node.ContentType))
        {
            return null;
        }

        if (node.ContentType == "Home")
        {
            return "content/index.md";
        }

        var segments = node.AncestorsAndSelf()
            .Reverse()
            .Where(ancestor => ancestor.ContentType != "Home")
            .Select(ancestor => ValidateSegment(ancestor.UrlName))
            .ToList();

        if (node.ContentType == "BlogEntry")
        {
            segments.Add("index.md");
        }
        else if (IndexTypes.Contains(node.ContentType))
        {
            segments.Add("index.md");
        }
        else
        {
            segments[^1] += ".md";
        }

        return $"content/{string.Join('/', segments)}";
    }

    public static string ValidateSegment(string value)
    {
        if (value.Length == 0 || value is "." or "..")
        {
            throw new InvalidDataException($"Invalid empty or relative output path segment '{value}'.");
        }

        if (!value.IsNormalized(NormalizationForm.FormC))
        {
            throw new InvalidDataException($"Output path segment is not Unicode NFC: '{value}'.");
        }

        if (value.EndsWith(' ') || value.EndsWith('.'))
        {
            throw new InvalidDataException($"Output path segment has a trailing space or period: '{value}'.");
        }

        if (value.Any(character => character < 32 || "<>:\"/\\|?*".Contains(character)))
        {
            throw new InvalidDataException($"Output path segment contains a Windows-invalid character: '{value}'.");
        }

        var stem = value.Split('.')[0];
        if (ReservedNames.Contains(stem))
        {
            throw new InvalidDataException($"Output path segment uses reserved Windows name '{value}'.");
        }

        return value;
    }

    public static string NormalizeSitePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        path = path.Replace('\\', '/').Trim();
        var queryIndex = path.IndexOfAny(['?', '#']);
        if (queryIndex >= 0)
        {
            path = path[..queryIndex];
        }

        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(segment => Uri.EscapeDataString(Uri.UnescapeDataString(segment)));
        var normalized = "/" + string.Join('/', segments);
        return normalized == "/" ? normalized : normalized.TrimEnd('/') + "/";
    }

    public static void ValidateNoCollisions(IEnumerable<string> paths)
    {
        var seen = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var path in paths)
        {
            var key = path.Normalize(NormalizationForm.FormC);
            if (seen.TryGetValue(key, out var existing))
            {
                throw new InvalidDataException($"Output path collision: '{existing}' and '{path}'.");
            }

            seen.Add(key, path);
        }
    }
}
