using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Ufcpp.ContentConverter;

public sealed class MacroExpander
{
    private static readonly Regex MacroRegex = new(
        @"<\?UMBRACO_MACRO\s+macroAlias=""(?<alias>[^""]+)""\s*/>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex ResidualMacroRegex = new(
        @"<\?\s*UMBRACO_MACRO\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex KeywordRegex = new(
        @"<strong\s+[^>]*id=[""'](?<id>[^""']+)[""'][^>]*>(?<text>.*?)</strong>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex HtmlTagRegex = new("<[^>]+>", RegexOptions.Compiled);

    private readonly IReadOnlyList<ContentNode> _nodes;
    private readonly IReadOnlyDictionary<int, string> _canonicalUrls;

    public MacroExpander(
        IReadOnlyList<ContentNode> nodes,
        IReadOnlyDictionary<int, string> canonicalUrls)
    {
        _nodes = nodes;
        _canonicalUrls = canonicalUrls;
    }

    public string Expand(string body, ContentNode current)
    {
        var expanded = MacroRegex.Replace(body, match => match.Groups["alias"].Value switch
        {
            "CsharpIndexVersionRelease" => VersionRelease(),
            "CsharpIndexByVersion" => ArticlesByVersion(),
            "KeywordSummary" => KeywordSummary(current),
            var alias => throw new InvalidDataException(
                $"Unknown Umbraco macro '{alias}' in node {current.Id}."),
        });
        if (ResidualMacroRegex.IsMatch(expanded))
        {
            throw new InvalidDataException($"Unrecognized Umbraco macro syntax in node {current.Id}.");
        }

        return expanded;
    }

    private static string VersionRelease() =>
        """
        <ul>
        <li><a href="/study/csharp/ap_ver2.html">C# 2.0 の機能</a></li>
        <li><a href="/study/csharp/ap_ver3.html">C# 3.0 の機能</a></li>
        <li><a href="/study/csharp/ap_ver4.html">C# 4.0 の機能</a></li>
        <li><a href="/study/csharp/ap_ver5.html">C# 5.0 の機能</a></li>
        <li><a href="/study/csharp/ap_ver6.html">C# 6 の機能</a></li>
        <li><a href="/study/csharp/cheatsheet/ap_ver7/">C# 7 の機能</a></li>
        </ul>
        """;

    private string ArticlesByVersion()
    {
        var builder = new StringBuilder();
        for (var version = 2; version <= 7; version++)
        {
            var tag = $"Ver. {version}.0";
            var articles = _nodes
                .Where(node => node.ContentType == "Article" && node.Tags.Contains(tag, StringComparer.Ordinal))
                .OrderBy(node => _canonicalUrls[node.Id], StringComparer.Ordinal)
                .ToArray();
            if (articles.Length == 0)
            {
                continue;
            }

            builder.AppendLine($"## C# {version}{(version < 6 ? ".0" : string.Empty)} の機能");
            builder.AppendLine();
            foreach (var article in articles)
            {
                builder.AppendLine($"- [{EscapeMarkdown(article.Title)}]({_canonicalUrls[article.Id]})");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }

    private string KeywordSummary(ContentNode current)
    {
        var subject = current.AncestorsAndSelf().FirstOrDefault(node => node.ContentType == "Subject")
            ?? throw new InvalidDataException($"KeywordSummary node {current.Id} is not below a Subject.");
        var builder = new StringBuilder();
        builder.AppendLine("<table>");
        builder.AppendLine("<thead><tr><th>タイトル</th><th>キーワード</th></tr></thead>");
        builder.AppendLine("<tbody>");
        foreach (var chapter in subject.Children
                     .Where(node => node.ContentType == "Chapter")
                     .OrderBy(node => node.SortOrder)
                     .ThenBy(node => node.Id))
        {
            builder.AppendLine(
                $"<tr><td colspan=\"2\"><span id=\"{chapter.Id}\">{WebUtility.HtmlEncode(chapter.Title)}</span></td></tr>");
            foreach (var article in chapter.Children
                         .Where(node => node.ContentType == "Article")
                         .OrderBy(node => node.SortOrder)
                         .ThenBy(node => node.Id))
            {
                var keywords = KeywordRegex.Matches(article.Get("bodyText"))
                    .Select(match => new
                    {
                        Id = match.Groups["id"].Value,
                        Text = WebUtility.HtmlDecode(
                            HtmlTagRegex.Replace(match.Groups["text"].Value, string.Empty)).Trim(),
                    })
                    .Where(keyword => keyword.Id.Length != 0 && keyword.Text.Length != 0)
                    .DistinctBy(keyword => keyword.Id, StringComparer.Ordinal)
                    .ToArray();
                if (keywords.Length == 0)
                {
                    continue;
                }

                builder.AppendLine(
                    $"<tr><th><a href=\"{_canonicalUrls[article.Id]}\" id=\"{article.Id}\">" +
                    $"{WebUtility.HtmlEncode(article.Title)}</a></th><td><ul>");
                foreach (var keyword in keywords)
                {
                    builder.AppendLine(
                        $"<li><a href=\"{_canonicalUrls[article.Id]}?key={Uri.EscapeDataString(keyword.Id)}\">" +
                        $"{WebUtility.HtmlEncode(keyword.Text)}</a></li>");
                }

                builder.AppendLine("</ul></td></tr>");
            }
        }

        builder.AppendLine("</tbody>");
        builder.AppendLine("</table>");
        return builder.ToString();
    }

    private static string EscapeMarkdown(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("[", "\\[", StringComparison.Ordinal)
            .Replace("]", "\\]", StringComparison.Ordinal);
}
