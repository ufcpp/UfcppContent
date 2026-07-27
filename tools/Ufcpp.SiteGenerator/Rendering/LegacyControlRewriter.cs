using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Ufcpp.SiteGenerator.Rendering;

/// <summary>
/// Rewrites the two ufcpp.net components that only worked with JavaScript into
/// markup that works without it.
///
/// <para>
/// The migrated Markdown still carries the original site's raw HTML: a
/// <c>&lt;span class="expand-button"&gt;</c> label followed by a
/// <c>&lt;div class="expand-panel"&gt;</c> that started hidden, and a
/// <c>&lt;div class="tab-container"&gt;</c> whose <c>&lt;ul&gt;</c> and panels were wired
/// together at runtime. This site ships no JavaScript, so the pair becomes
/// <c>&lt;details&gt;</c>/<c>&lt;summary&gt;</c> and each tab set gets one radio button per
/// tab that <c>site.css</c> switches with <c>:checked ~</c>.
/// </para>
///
/// <para>
/// The rewrite runs on the Markdown source after fenced code blocks have been
/// replaced by placeholders, so HTML that only appears inside a code sample is
/// never touched. Anything that does not match the expected shape is left
/// exactly as it was, which keeps the previous static rendering as the fallback.
/// </para>
/// </summary>
public static class LegacyControlRewriter
{
    /// <summary>
    /// Largest tab count <c>site.css</c> can switch. Sets with more tabs keep the
    /// static rendering, because a panel with no matching <c>:checked ~</c> rule
    /// would be permanently hidden.
    /// </summary>
    public const int MaxSwitchableTabs = 6;

    private const string TabIdPrefix = "ufcpp-tab-";

    private static readonly Regex ExpandControlRegex = new(
        """(?m)^[ \t]*<span\b(?=[^>]{0,2048}\bclass\s*=\s*"(?:[^"]{0,2048}\s)?expand-button(?:\s[^"]{0,2048})?")[^>]{0,2048}>(?<label>.{0,2048}?)</span\s*>[ \t]*\r?\n(?:[ \t]*\r?\n)?(?<panel><div\b(?=[^>]{0,2048}\bclass\s*=\s*"(?:[^"]{0,2048}\s)?expand-panel(?:\s[^"]{0,2048})?")[^>]{0,2048}>)""",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex TabContainerRegex = new(
        """<div\b(?=[^>]{0,2048}\bclass\s*=\s*"(?:[^"]{0,2048}\s)?tab-container(?:\s[^"]{0,2048})?")[^>]{0,2048}>""",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex IdAttributeRegex = new(
        """(?:^|\s)id\s*=\s*(?<q>["'])(?<value>[^"']{1,2048})\k<q>""",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    /// <summary>
    /// Rewrites every expand panel and language tab set found in
    /// <paramref name="markdown"/>.
    /// </summary>
    public static string Rewrite(string markdown) =>
        RewriteTabContainers(RewriteExpandPanels(markdown));

    private static string RewriteExpandPanels(string markdown)
    {
        var builder = new StringBuilder(markdown.Length);
        var copied = 0;

        foreach (Match match in ExpandControlRegex.Matches(markdown))
        {
            if (match.Index < copied)
            {
                continue;
            }

            var panel = match.Groups["panel"];
            var bodyStart = panel.Index + panel.Length;
            if (!TryFindClosingTag(markdown, "div", bodyStart, markdown.Length, out var body))
            {
                continue;
            }

            builder.Append(markdown, copied, match.Index - copied);
            builder.Append("<details class=\"expand-panel\">\n<summary class=\"expand-button\">");
            builder.Append(match.Groups["label"].Value.Trim());
            builder.Append("</summary>\n<div class=\"expand-panel-body\" markdown=\"1\">");
            builder.Append(markdown, bodyStart, body.Start - bodyStart);
            builder.Append("</div>\n</details>");
            copied = body.End;
        }

        builder.Append(markdown, copied, markdown.Length - copied);
        return builder.ToString();
    }

    private static string RewriteTabContainers(string markdown)
    {
        var takenIds = CollectIds(markdown);
        var builder = new StringBuilder(markdown.Length);
        var copied = 0;
        var setNumber = 0;

        foreach (Match match in TabContainerRegex.Matches(markdown))
        {
            if (match.Index < copied)
            {
                continue;
            }

            var contentStart = match.Index + match.Length;
            if (!TryFindClosingTag(markdown, "div", contentStart, markdown.Length, out var container)
                || !TryReadTabSet(markdown, contentStart, container.Start, out var tabs))
            {
                continue;
            }

            var name = ReserveTabName(takenIds, tabs.Count, ref setNumber);

            builder.Append(markdown, copied, tabs.ListStart - copied);
            for (var index = 0; index < tabs.Count; index++)
            {
                builder.Append("<input type=\"radio\" name=\"")
                    .Append(name)
                    .Append("\" id=\"")
                    .Append(TabId(name, index))
                    .Append('"')
                    .Append(index == 0 ? " checked" : string.Empty)
                    .Append(">\n");
            }

            var position = tabs.ListStart;
            for (var index = 0; index < tabs.Count; index++)
            {
                var label = tabs.Labels[index];
                builder.Append(markdown, position, label.Start - position);
                builder.Append("<label for=\"").Append(TabId(name, index)).Append("\">");
                builder.Append(markdown, label.Start, label.End - label.Start);
                builder.Append("</label>");
                position = label.End;
            }

            builder.Append(markdown, position, container.End - position);
            copied = container.End;
        }

        builder.Append(markdown, copied, markdown.Length - copied);
        return builder.ToString();
    }

    private static string TabId(string name, int index) =>
        name + "-" + (index + 1).ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Picks the next group name whose generated ids are all free, so a tab set
    /// never steals an id a legacy anchor already claims.
    /// </summary>
    private static string ReserveTabName(HashSet<string> takenIds, int tabCount, ref int setNumber)
    {
        while (true)
        {
            var name = TabIdPrefix + (++setNumber).ToString(CultureInfo.InvariantCulture);
            var ids = Enumerable.Range(0, tabCount).Select(index => TabId(name, index)).ToList();
            if (ids.All(id => !takenIds.Contains(id)))
            {
                takenIds.UnionWith(ids);
                return name;
            }
        }
    }

    private static HashSet<string> CollectIds(string markdown) =>
        IdAttributeRegex.Matches(markdown)
            .Select(match => match.Groups["value"].Value.Trim())
            .Where(value => value.Length > 0)
            .ToHashSet(StringComparer.Ordinal);

    /// <summary>
    /// Reads the tab strip of one container: the <c>&lt;li&gt;</c> contents and the
    /// panel count. Returns false unless the container has the shape the CSS
    /// expects — a single <c>&lt;ul&gt;</c> followed by one panel per tab.
    /// </summary>
    private static bool TryReadTabSet(string markdown, int contentStart, int contentEnd, out TabSet tabs)
    {
        tabs = default;

        var listStart = FindOpeningTag(markdown, "ul", contentStart, contentEnd);
        if (listStart is null
            || !TryFindClosingTag(markdown, "ul", TagEnd(markdown, listStart.Value), contentEnd, out var list))
        {
            return false;
        }

        var labels = new List<Span>();
        var position = TagEnd(markdown, listStart.Value);
        while (FindOpeningTag(markdown, "li", position, list.Start) is { } itemStart)
        {
            var itemContentStart = TagEnd(markdown, itemStart);
            if (!TryFindClosingTag(markdown, "li", itemContentStart, list.Start, out var item))
            {
                return false;
            }

            labels.Add(new Span(itemContentStart, item.Start));
            position = item.End;
        }

        var panels = 0;
        position = list.End;
        while (FindOpeningTag(markdown, "div", position, contentEnd) is { } panelStart)
        {
            if (!TryFindClosingTag(markdown, "div", TagEnd(markdown, panelStart), contentEnd, out var panel))
            {
                return false;
            }

            panels++;
            position = panel.End;
        }

        if (labels.Count == 0 || labels.Count > MaxSwitchableTabs || labels.Count != panels)
        {
            return false;
        }

        tabs = new TabSet(listStart.Value, labels);
        return true;
    }

    private static int TagEnd(string markdown, int tagStart) =>
        markdown.IndexOf('>', tagStart) + 1;

    /// <summary>
    /// Index of the next <c>&lt;tag …&gt;</c> between <paramref name="start"/> and
    /// <paramref name="limit"/>, or null when there is none.
    /// </summary>
    private static int? FindOpeningTag(string markdown, string tag, int start, int limit)
    {
        for (var index = start; index < limit;)
        {
            var next = markdown.IndexOf('<', index);
            if (next < 0 || next >= limit)
            {
                return null;
            }

            if (IsTagAt(markdown, next, tag, closing: false))
            {
                return markdown.IndexOf('>', next) >= 0 ? next : null;
            }

            index = next + 1;
        }

        return null;
    }

    /// <summary>
    /// Finds the closing tag that matches an already-consumed opening tag,
    /// counting nested openings of the same name.
    /// </summary>
    private static bool TryFindClosingTag(
        string markdown,
        string tag,
        int contentStart,
        int limit,
        out Span closing)
    {
        var depth = 1;
        for (var index = contentStart; index < limit;)
        {
            var next = markdown.IndexOf('<', index);
            if (next < 0 || next >= limit)
            {
                break;
            }

            var isClosing = IsTagAt(markdown, next, tag, closing: true);
            if (!isClosing && !IsTagAt(markdown, next, tag, closing: false))
            {
                index = next + 1;
                continue;
            }

            var end = markdown.IndexOf('>', next);
            if (end < 0 || end >= limit)
            {
                break;
            }

            if (isClosing)
            {
                if (--depth == 0)
                {
                    closing = new Span(next, end + 1);
                    return true;
                }
            }
            else if (markdown[end - 1] != '/')
            {
                depth++;
            }

            index = end + 1;
        }

        closing = default;
        return false;
    }

    private static bool IsTagAt(string markdown, int index, string tag, bool closing)
    {
        var nameStart = index + (closing ? 2 : 1);
        if (closing && (index + 1 >= markdown.Length || markdown[index + 1] != '/'))
        {
            return false;
        }

        if (nameStart + tag.Length >= markdown.Length
            || string.Compare(
                markdown,
                nameStart,
                tag,
                0,
                tag.Length,
                StringComparison.OrdinalIgnoreCase) != 0)
        {
            return false;
        }

        var after = markdown[nameStart + tag.Length];
        return after == '>' || after == '/' || char.IsWhiteSpace(after);
    }

    private readonly record struct Span(int Start, int End);

    private readonly record struct TabSet(int ListStart, IReadOnlyList<Span> Labels)
    {
        public int Count => Labels.Count;
    }
}
