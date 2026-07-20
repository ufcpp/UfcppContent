using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ufcpp.ContentConverter;

public static class TextUtilities
{
    private static readonly Regex AtxHeadingRegex = new(
        @"^ {0,3}#{1,6}(?:[ \t]+|$)",
        RegexOptions.Compiled);

    private static readonly Regex LegacyAtxHeadingRegex = new(
        @"^(?<indent> {0,3})(?<marker>#{1,6})(?!#)(?<content><a\b.*)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex MarkdownFenceRegex = new(
        @"^ {0,3}(?<marker>`{3,}|~{3,})(?<remainder>.*)$",
        RegexOptions.Compiled);

    private static readonly Regex LinkedMarkdownImageRegex = new(
        @"^\[!\[[^\r\n]*\]\([^\r\n]*\)\]\([^\r\n]*\)$",
        RegexOptions.Compiled);

    private static readonly Regex ProtectedHtmlBlockStartRegex = new(
        @"<(?<tag>pre|code|script|style)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static readonly UTF8Encoding Utf8NoBom = new(false, true);

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static string NormalizeNewlines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');

    public static string NormalizeMarkdownFigureImages(string value)
    {
        var normalized = NormalizeNewlines(value);
        var lines = normalized.Split('\n');
        var output = new List<string>(lines.Length);
        var inFence = false;
        var fenceMarker = '\0';
        var fenceLength = 0;
        var inHtmlComment = false;
        string? htmlEndTag = null;

        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            if (inFence)
            {
                output.Add(line);
                if (TryGetFence(line, out var marker, out var length, out var remainder)
                    && marker == fenceMarker
                    && length >= fenceLength
                    && string.IsNullOrWhiteSpace(remainder))
                {
                    inFence = false;
                }

                continue;
            }

            if (inHtmlComment)
            {
                output.Add(line);
                if (line.Contains("-->", StringComparison.Ordinal))
                {
                    inHtmlComment = false;
                }

                continue;
            }

            if (htmlEndTag is not null)
            {
                output.Add(line);
                if (line.Contains(htmlEndTag, StringComparison.OrdinalIgnoreCase))
                {
                    htmlEndTag = null;
                }

                continue;
            }

            if (TryGetFence(line, out fenceMarker, out fenceLength, out _))
            {
                inFence = true;
                output.Add(line);
                continue;
            }

            var commentStart = line.IndexOf("<!--", StringComparison.Ordinal);
            if (commentStart >= 0
                && line.IndexOf("-->", commentStart + 4, StringComparison.Ordinal) < 0)
            {
                inHtmlComment = true;
                output.Add(line);
                continue;
            }

            var htmlMatch = ProtectedHtmlBlockStartRegex.Match(line);
            if (htmlMatch.Success)
            {
                var tag = htmlMatch.Groups["tag"].Value;
                var endTag = $"</{tag}>";
                if (line.IndexOf(
                        endTag,
                        htmlMatch.Index + htmlMatch.Length,
                        StringComparison.OrdinalIgnoreCase) < 0)
                {
                    htmlEndTag = endTag;
                }

                output.Add(line);
                continue;
            }

            if (line == "<figure>"
                && index + 2 < lines.Length
                && IsLinkedMarkdownImage(lines[index + 1]))
            {
                output.Add(line);
                output.Add(string.Empty);
                output.Add(lines[index + 1].Trim());
                output.Add(string.Empty);

                var nextIndex = index + 2;
                while (nextIndex < lines.Length && string.IsNullOrWhiteSpace(lines[nextIndex]))
                {
                    nextIndex++;
                }

                if (nextIndex < lines.Length && IsFigureCaption(lines[nextIndex]))
                {
                    output.Add(lines[nextIndex].TrimStart());
                    index = nextIndex;
                }
                else
                {
                    index = nextIndex - 1;
                }

                continue;
            }

            output.Add(line);
        }

        return string.Join('\n', output);
    }

    public static string NormalizeMarkdownHeadingSpacing(string value)
    {
        var normalized = NormalizeNewlines(value);
        var lines = normalized.Split('\n');
        var builder = new StringBuilder(normalized.Length);
        var inFrontMatter = lines.Length != 0 && lines[0] == "---";
        var inFence = false;
        var fenceMarker = '\0';
        var fenceLength = 0;
        var inHtmlComment = false;
        string? htmlEndTag = null;

        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            var isProtected = inFrontMatter || inFence || inHtmlComment || htmlEndTag is not null;
            if (!isProtected)
            {
                line = LegacyAtxHeadingRegex.Replace(
                    line,
                    "${indent}${marker} ${content}",
                    1);
            }

            var isHeading = !isProtected && AtxHeadingRegex.IsMatch(line);

            if (inFrontMatter)
            {
                if (index != 0 && line is "---" or "...")
                {
                    inFrontMatter = false;
                }
            }
            else if (inFence)
            {
                if (TryGetFence(line, out var marker, out var length, out var remainder)
                    && marker == fenceMarker
                    && length >= fenceLength
                    && string.IsNullOrWhiteSpace(remainder))
                {
                    inFence = false;
                }
            }
            else if (inHtmlComment)
            {
                if (line.Contains("-->", StringComparison.Ordinal))
                {
                    inHtmlComment = false;
                }
            }
            else if (htmlEndTag is not null)
            {
                if (line.Contains(htmlEndTag, StringComparison.OrdinalIgnoreCase))
                {
                    htmlEndTag = null;
                }
            }
            else if (TryGetFence(line, out fenceMarker, out fenceLength, out _))
            {
                inFence = true;
            }
            else
            {
                var commentStart = line.IndexOf("<!--", StringComparison.Ordinal);
                if (commentStart >= 0
                    && line.IndexOf("-->", commentStart + 4, StringComparison.Ordinal) < 0)
                {
                    inHtmlComment = true;
                }

                if (!inHtmlComment)
                {
                    var htmlMatch = ProtectedHtmlBlockStartRegex.Match(line);
                    if (htmlMatch.Success)
                    {
                        var tag = htmlMatch.Groups["tag"].Value;
                        var endTag = $"</{tag}>";
                        if (line.IndexOf(
                                endTag,
                                htmlMatch.Index + htmlMatch.Length,
                                StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            htmlEndTag = endTag;
                        }
                    }
                }
            }

            builder.Append(line);
            if (index == lines.Length - 1)
            {
                continue;
            }

            builder.Append('\n');
            if (isHeading && !string.IsNullOrWhiteSpace(lines[index + 1]))
            {
                builder.Append('\n');
            }
        }

        return builder.ToString();
    }

    public static string YamlQuote(string value) =>
        JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });

    public static string Sha256File(string path)
    {
        using var stream = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }

    public static string Sha256Text(string value) =>
        Convert.ToHexString(SHA256.HashData(Utf8NoBom.GetBytes(value))).ToLowerInvariant();

    public static void WriteText(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, NormalizeNewlines(content).TrimEnd() + "\n", Utf8NoBom);
    }

    public static void WriteJson(string path, object value) =>
        WriteText(path, JsonSerializer.Serialize(value, JsonOptions));

    private static bool IsLinkedMarkdownImage(string line)
    {
        var trimmed = line.Trim();
        return LinkedMarkdownImageRegex.IsMatch(trimmed);
    }

    private static bool IsFigureCaption(string line)
    {
        var trimmed = line.TrimStart();
        const string tag = "<figcaption";
        return trimmed.StartsWith(tag, StringComparison.OrdinalIgnoreCase)
            && trimmed.Length > tag.Length
            && (trimmed[tag.Length] == '>' || char.IsWhiteSpace(trimmed[tag.Length]));
    }

    private static bool TryGetFence(
        string line,
        out char marker,
        out int length,
        out string remainder)
    {
        var match = MarkdownFenceRegex.Match(line);
        if (!match.Success)
        {
            marker = '\0';
            length = 0;
            remainder = string.Empty;
            return false;
        }

        var markerText = match.Groups["marker"].Value;
        marker = markerText[0];
        length = markerText.Length;
        remainder = match.Groups["remainder"].Value;
        return true;
    }
}
