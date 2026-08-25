using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text.RegularExpressions;

namespace Ufcpp.SiteGenerator.Tests;

public sealed class ContentFormatTests
{
    private static readonly Regex PreElementRegex = new(
        @"<pre\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex TableTagRegex = new(
        @"<(?<closing>/)?(?<name>table|td|th)\b(?<attributes>[^>]*)>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex MarkdownAttributeRegex = new(
        @"\bmarkdown\s*=\s*(?<quote>[""'])1\k<quote>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    private static readonly Regex FenceLineRegex = new(
        @"^(?<indent>[ \t]*)(?<marker>`{3,}|~{3,})(?<remainder>.*)$",
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(5));

    [Fact]
    public void MarkdownContent_DoesNotContainLegacyPreElements()
    {
        var failures = new List<string>();
        foreach (var path in Directory.EnumerateFiles(
                     Path.Combine(RepoRoot, "content"),
                     "*.md",
                     SearchOption.AllDirectories))
        {
            var document = Markdown.Parse(File.ReadAllText(path));
            var hasLegacyPre = document.Descendants().Any(node =>
                node switch
                {
                    HtmlBlock block => PreElementRegex.IsMatch(block.Lines.ToString()),
                    HtmlInline inline => PreElementRegex.IsMatch(inline.Tag),
                    _ => false,
                });
            if (hasLegacyPre)
            {
                failures.Add(Path.GetRelativePath(RepoRoot, path));
            }
        }

        Assert.True(
            failures.Count == 0,
            "Legacy <pre> elements must be fenced Markdown code blocks:"
            + Environment.NewLine
            + string.Join(Environment.NewLine, failures));
    }

    [Fact]
    public void MarkdownContent_TableFencesUseSupportedMarkdownCells()
    {
        var failures = new List<string>();
        foreach (var path in Directory.EnumerateFiles(
                     Path.Combine(RepoRoot, "content"),
                     "*.md",
                     SearchOption.AllDirectories))
        {
            foreach (var line in FindUnsupportedTableFences(File.ReadAllText(path)))
            {
                failures.Add($"{Path.GetRelativePath(RepoRoot, path)}:{line}");
            }
        }

        Assert.True(
            failures.Count == 0,
            "Fenced code inside td/th must use markdown=\"1\", safe indentation, "
            + "and a cell-scoped closing fence:"
            + Environment.NewLine
            + string.Join(Environment.NewLine, failures));
    }

    [Fact]
    public void FindUnsupportedTableFences_TracksCellsAndIgnoresCodeContent()
    {
        const string MissingAttribute =
            "<table>\n<td>\n```csharp\nvar value = 1;\n```\n</td>\n</table>";
        const string EnabledCell =
            "<table>\n<td markdown='1'>\n```csharp\nvar value = 1;\n```\n</td>\n</table>";
        const string IndentedMissingAttribute =
            "<table>\n<td>\n    ```csharp\n    var value = 1;\n    ```\n</td>\n</table>";
        const string HtmlInsideCode =
            "```html\n<table><tr><td>\n```\n\n```csharp\nvar value = 1;\n```";
        const string MalformedClosedTable =
            "<table>\n<th>heading</ht>\n</table>\n\n```csharp\nvar value = 1;\n```";
        const string UnclosedEnabledCell =
            "<table>\n<td markdown=\"1\">\n```csharp\nvar first = 1;\n</td>\n"
            + "<td>\n```csharp\nvar second = 2;\n```\n</td>\n</table>";
        const string IndentedHtmlFence =
            "<table>\n<td markdown=\"1\">\n    ```html\n    </td>\n    ```\n"
            + "</td>\n</table>";
        const string ProtectedHtmlFence =
            "<table>\n<td markdown=\"1\">\n```html\n</td>\n```\n</td>\n</table>";
        const string IndentedClosingFence =
            "<table>\n<td markdown=\"1\">\n```csharp\nvar value = 1;\n    ```\n"
            + "</td>\n</table>";
        const string TabIndentedClosingFence =
            "<table>\n<td markdown=\"1\">\n```csharp\nvar value = 1;\n\t```\n"
            + "</td>\n</table>";

        Assert.Equal([3], FindUnsupportedTableFences(MissingAttribute));
        Assert.Equal([3], FindUnsupportedTableFences(IndentedMissingAttribute));
        Assert.Empty(FindUnsupportedTableFences(EnabledCell));
        Assert.Empty(FindUnsupportedTableFences(HtmlInsideCode));
        Assert.Empty(FindUnsupportedTableFences(MalformedClosedTable));
        Assert.Equal([3, 7], FindUnsupportedTableFences(UnclosedEnabledCell));
        Assert.Equal([3], FindUnsupportedTableFences(IndentedHtmlFence));
        Assert.Empty(FindUnsupportedTableFences(ProtectedHtmlFence));
        Assert.Equal([3, 5], FindUnsupportedTableFences(IndentedClosingFence));
        Assert.Equal([3, 5], FindUnsupportedTableFences(TabIndentedClosingFence));
    }

    private static IReadOnlyList<int> FindUnsupportedTableFences(string markdown)
    {
        var cells = new Stack<Cell>();
        var failures = new List<int>();
        var tableDepth = 0;
        var lines = markdown
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            var fence = FenceLineRegex.Match(lines[index]);
            if (fence.Success)
            {
                var cell = cells.TryPeek(out var currentCell)
                    && currentCell.TableDepth > 0
                        ? currentCell
                        : (Cell?)null;
                var safelyProtected = IsSafelyProtectedFence(fence);
                var closingIndex = FindClosingFence(
                    lines,
                    index + 1,
                    fence.Groups["marker"].Value,
                    cell?.Name,
                    requireSafeIndent: safelyProtected);
                if (cell is not null
                    && (!cell.Value.MarkdownEnabled
                        || !safelyProtected
                        || closingIndex < 0))
                {
                    failures.Add(index + 1);
                }

                if (closingIndex >= 0)
                {
                    index = closingIndex;
                    continue;
                }
            }

            foreach (Match tag in TableTagRegex.Matches(lines[index]))
            {
                var name = tag.Groups["name"].Value;
                var closing = tag.Groups["closing"].Success;
                if (name.Equals("table", StringComparison.OrdinalIgnoreCase))
                {
                    if (!closing)
                    {
                        tableDepth++;
                        continue;
                    }

                    while (cells.TryPeek(out var cell)
                           && cell.TableDepth >= tableDepth)
                    {
                        cells.Pop();
                    }

                    tableDepth--;
                    if (tableDepth < 0)
                    {
                        throw new InvalidDataException(
                            "Unbalanced </table> while checking table code.");
                    }

                    continue;
                }

                if (!closing)
                {
                    cells.Push(new Cell(
                        name,
                        MarkdownAttributeRegex.IsMatch(
                            tag.Groups["attributes"].Value),
                        tableDepth));
                    continue;
                }

                if (cells.TryPeek(out var opening)
                    && string.Equals(
                        opening.Name,
                        name,
                        StringComparison.OrdinalIgnoreCase))
                {
                    cells.Pop();
                }
            }
        }

        return failures;
    }

    private static int FindClosingFence(
        IReadOnlyList<string> lines,
        int startIndex,
        string openingMarker,
        string? cellName,
        bool requireSafeIndent)
    {
        var sawCellClose = false;
        for (var index = startIndex; index < lines.Count; index++)
        {
            var candidate = FenceLineRegex.Match(lines[index]);
            if (candidate.Success
                && (!requireSafeIndent || IsSafelyProtectedFence(candidate))
                && candidate.Groups["marker"].Value[0] == openingMarker[0]
                && candidate.Groups["marker"].Value.Length >= openingMarker.Length
                && string.IsNullOrWhiteSpace(candidate.Groups["remainder"].Value))
            {
                return index;
            }

            if (cellName is null)
            {
                continue;
            }

            foreach (Match tag in TableTagRegex.Matches(lines[index]))
            {
                var name = tag.Groups["name"].Value;
                var closing = tag.Groups["closing"].Success;
                if (name.Equals("table", StringComparison.OrdinalIgnoreCase))
                {
                    if (closing && sawCellClose)
                    {
                        return -1;
                    }

                    continue;
                }

                if (closing
                    && name.Equals(cellName, StringComparison.OrdinalIgnoreCase))
                {
                    sawCellClose = true;
                }
                else if (!closing && sawCellClose)
                {
                    return -1;
                }
            }
        }

        return -1;
    }

    private static bool IsSafelyProtectedFence(Match fence)
    {
        var indent = fence.Groups["indent"].Value;
        return indent.All(static character => character == ' ')
            && indent.Length <= 3;
    }

    private readonly record struct Cell(
        string Name,
        bool MarkdownEnabled,
        int TableDepth);

    private static readonly string RepoRoot = FindRepoRoot();

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "UfcppContent.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException(
            $"Could not locate the repository root from '{AppContext.BaseDirectory}'.");
    }
}
