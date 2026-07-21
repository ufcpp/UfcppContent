using System.Text;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>Converts canonical site paths to output file paths.</summary>
public static class OutputPathResolver
{
    private static readonly HashSet<string> ReservedNames =
        new(
            new[] { "CON", "PRN", "AUX", "NUL" }
                .Concat(Enumerable.Range(1, 9).Select(number => $"COM{number}"))
                .Concat(Enumerable.Range(1, 9).Select(number => $"LPT{number}")),
            StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Converts a canonical site path such as <c>/study/csharp/</c> to the
    /// corresponding output file path <c>study/csharp/index.html</c>.
    /// Also handles paths that already end with <c>.html</c>.
    /// </summary>
    public static string Resolve(string canonicalPath)
    {
        if (canonicalPath == "/")
        {
            return "index.html";
        }

        if (string.IsNullOrWhiteSpace(canonicalPath)
            || !canonicalPath.StartsWith('/')
            || canonicalPath.StartsWith("//", StringComparison.Ordinal)
            || canonicalPath.Contains('\\')
            || canonicalPath.Contains('?')
            || canonicalPath.Contains('#'))
        {
            throw InvalidPath(canonicalPath);
        }

        var encodedPath = canonicalPath.Trim('/');
        if (encodedPath.Length == 0)
        {
            return "index.html";
        }

        var encodedSegments = encodedPath.Split('/');
        var segments = encodedSegments.Select(DecodeAndValidateSegment).ToArray();
        var outputPath = string.Join('/', segments);

        return !canonicalPath.EndsWith('/')
               && segments[^1].EndsWith(".html", StringComparison.OrdinalIgnoreCase)
            ? outputPath
            : outputPath + "/index.html";
    }

    /// <summary>
    /// Returns the canonical site path extracted from a full source URL such as
    /// <c>https://ufcpp.net/study/csharp/</c>.
    /// </summary>
    public static string ExtractCanonicalPath(string sourceUrl)
    {
        if (Uri.TryCreate(sourceUrl, UriKind.Absolute, out var uri))
        {
            return uri.AbsolutePath;
        }

        // Fallback: treat as a path directly
        return sourceUrl.StartsWith('/') ? sourceUrl : "/" + sourceUrl;
    }

    private static string DecodeAndValidateSegment(string encodedSegment)
    {
        if (encodedSegment.Length == 0 || HasInvalidPercentEncoding(encodedSegment))
        {
            throw InvalidPath(encodedSegment);
        }

        string segment;
        try
        {
            segment = Uri.UnescapeDataString(encodedSegment);
        }
        catch (UriFormatException)
        {
            throw InvalidPath(encodedSegment);
        }

        if (segment.Length == 0
            || segment is "." or ".."
            || !segment.IsNormalized(NormalizationForm.FormC)
            || segment.EndsWith(' ')
            || segment.EndsWith('.')
            || segment.Any(character =>
                character < 32 || "<>:\"/\\|?*".Contains(character)))
        {
            throw InvalidPath(encodedSegment);
        }

        var stem = segment.Split('.')[0];
        if (ReservedNames.Contains(stem))
        {
            throw InvalidPath(encodedSegment);
        }

        return segment;
    }

    private static bool HasInvalidPercentEncoding(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            if (value[index] != '%')
            {
                continue;
            }

            if (index + 2 >= value.Length
                || !Uri.IsHexDigit(value[index + 1])
                || !Uri.IsHexDigit(value[index + 2]))
            {
                return true;
            }

            index += 2;
        }

        return false;
    }

    private static InvalidDataException InvalidPath(string path) =>
        new($"Invalid output path '{path}'.");
}
