namespace Ufcpp.ContentConverter;

/// <summary>
/// Decides which of the aliases collected for a page are published as redirects.
/// </summary>
/// <remarks>
/// <para>
/// The legacy rewrite maps let a page accumulate more aliases than the site actually
/// serves. Two families are purely derived and were never reachable on the original
/// site, so they are kept for internal link resolution but are not published:
/// </para>
/// <list type="number">
/// <item>
/// Paths produced by dropping the <c>/study</c> prefix, such as
/// <c>/csharp/st_basis.html</c> for <c>/study/csharp/st_basis.html</c>.
/// </item>
/// <item>
/// Paths produced by dropping the <c>.html</c> extension, such as
/// <c>/study/csharp/st_basis</c> for <c>/study/csharp/st_basis.html</c>.
/// </item>
/// </list>
/// <para>
/// Genuine legacy URLs that merely happen to sit outside <c>/study/</c>, such as
/// <c>/lecture/index.html</c>, are published because no <c>/study</c>-prefixed
/// counterpart exists on the same page.
/// </para>
/// </remarks>
public static class AliasPolicy
{
    private const string HtmlExtension = ".html";
    private const string StudyPrefix = "/study/";

    /// <summary>
    /// Returns the subset of <paramref name="aliases"/> that is published as redirects
    /// for the page whose canonical URL is <paramref name="canonicalUrl"/>.
    /// </summary>
    public static IReadOnlyList<string> SelectPublished(
        string canonicalUrl,
        IReadOnlyList<string> aliases)
    {
        var routeKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            RouteKey(canonicalUrl),
        };
        foreach (var alias in aliases)
        {
            routeKeys.Add(RouteKey(alias));
        }

        var exactAliases = new HashSet<string>(aliases, StringComparer.OrdinalIgnoreCase);

        return aliases
            .Where(alias =>
                !IsStudyPrefixDropped(alias, routeKeys)
                && !IsHtmlExtensionDropped(alias, exactAliases))
            .Order(StringComparer.Ordinal)
            .ToArray();
    }

    private static bool IsStudyPrefixDropped(string alias, HashSet<string> routeKeys) =>
        !alias.StartsWith(StudyPrefix, StringComparison.OrdinalIgnoreCase)
        && routeKeys.Contains(RouteKey("/study" + alias));

    private static bool IsHtmlExtensionDropped(string alias, HashSet<string> exactAliases) =>
        !alias.EndsWith(HtmlExtension, StringComparison.OrdinalIgnoreCase)
        && exactAliases.Contains(alias + HtmlExtension);

    private static string RouteKey(string path) =>
        path.EndsWith(HtmlExtension, StringComparison.OrdinalIgnoreCase)
            ? path
            : path.TrimEnd('/') + "/";
}
