using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator;

internal static class CodeNormalizer
{
    public static string Normalize(string code)
    {
        ArgumentNullException.ThrowIfNull(code);

        var lines = WebUtility.HtmlDecode(code)
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n')
            .Select(static line => line.TrimEnd(' ', '\t'))
            .ToList();
        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[0]))
        {
            lines.RemoveAt(0);
        }

        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[^1]))
        {
            lines.RemoveAt(lines.Count - 1);
        }

        var nonEmptyLines = lines.Where(static line => line.Length != 0).ToArray();
        if (nonEmptyLines.Length == 0)
        {
            return string.Empty;
        }

        var commonIndent = LeadingWhitespace(nonEmptyLines[0]);
        foreach (var line in nonEmptyLines.Skip(1))
        {
            var indentation = LeadingWhitespace(line);
            var commonLength = 0;
            while (commonLength < commonIndent.Length
                   && commonLength < indentation.Length
                   && commonIndent[commonLength] == indentation[commonLength])
            {
                commonLength++;
            }

            commonIndent = commonIndent[..commonLength];
            if (commonIndent.Length == 0)
            {
                break;
            }
        }

        if (commonIndent.Length != 0)
        {
            for (var index = 0; index < lines.Count; index++)
            {
                if (lines[index].StartsWith(commonIndent, StringComparison.Ordinal))
                {
                    lines[index] = lines[index][commonIndent.Length..];
                }
            }
        }

        return string.Join('\n', lines);
    }

    public static string Hash(string code)
    {
        var bytes = Encoding.UTF8.GetBytes(Normalize(code));
        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }

    private static string LeadingWhitespace(string value)
    {
        var length = 0;
        while (length < value.Length && value[length] is ' ' or '\t')
        {
            length++;
        }

        return value[..length];
    }
}
