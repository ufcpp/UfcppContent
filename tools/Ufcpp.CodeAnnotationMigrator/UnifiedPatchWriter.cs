using System.Text;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record DocumentChange(string Before, string After);

internal static class UnifiedPatchWriter
{
    public static byte[] Write(
        string targetCommit,
        IReadOnlyDictionary<string, DocumentChange> changes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetCommit);
        ArgumentNullException.ThrowIfNull(changes);
        if (changes.Count == 0)
        {
            return [];
        }

        var patch = new StringBuilder();
        patch.Append("# Ufcpp Code Annotation Migrator target ")
            .Append(targetCommit)
            .Append('\n');
        foreach (var (path, change) in changes.OrderBy(
                     static item => item.Key,
                     StringComparer.Ordinal))
        {
            if (string.Equals(change.Before, change.After, StringComparison.Ordinal))
            {
                continue;
            }

            var beforeLines = GetLines(change.Before);
            var afterLines = GetLines(change.After);
            if (beforeLines.Values.Count != afterLines.Values.Count)
            {
                throw new InvalidDataException(
                    $"Issue #4 patch for '{path}' changes the line count.");
            }

            var changedLines = Enumerable.Range(0, beforeLines.Values.Count)
                .Where(index =>
                    !string.Equals(
                        beforeLines.Values[index],
                        afterLines.Values[index],
                        StringComparison.Ordinal)
                    || index == beforeLines.Values.Count - 1
                    && beforeLines.EndsWithNewline != afterLines.EndsWithNewline)
                .ToArray();
            if (changedLines.Length == 0)
            {
                throw new InvalidDataException(
                    $"Issue #4 patch for '{path}' has no changed line.");
            }

            patch.Append("diff --git a/").Append(path).Append(" b/").Append(path).Append('\n')
                .Append("index ").Append(GitBlobId.Compute(change.Before))
                .Append("..").Append(GitBlobId.Compute(change.After))
                .Append(" 100644\n")
                .Append("--- a/").Append(path).Append('\n')
                .Append("+++ b/").Append(path).Append('\n');
            foreach (var hunk in GetHunks(changedLines, beforeLines.Values.Count))
            {
                patch.Append("@@ -")
                    .Append(hunk.Start + 1)
                    .Append(',')
                    .Append(hunk.End - hunk.Start)
                    .Append(" +")
                    .Append(hunk.Start + 1)
                    .Append(',')
                    .Append(hunk.End - hunk.Start)
                    .Append(" @@\n");
                for (var index = hunk.Start; index < hunk.End; index++)
                {
                    var isFinal = index == beforeLines.Values.Count - 1;
                    if (string.Equals(
                            beforeLines.Values[index],
                            afterLines.Values[index],
                            StringComparison.Ordinal)
                        && (!isFinal
                            || beforeLines.EndsWithNewline
                                == afterLines.EndsWithNewline))
                    {
                        AppendPatchLine(
                            ' ',
                            beforeLines.Values[index],
                            isFinal && !beforeLines.EndsWithNewline);
                    }
                    else
                    {
                        AppendPatchLine(
                            '-',
                            beforeLines.Values[index],
                            isFinal && !beforeLines.EndsWithNewline);
                        AppendPatchLine(
                            '+',
                            afterLines.Values[index],
                            isFinal && !afterLines.EndsWithNewline);
                    }
                }
            }
        }

        return Encoding.UTF8.GetBytes(patch.ToString());

        void AppendPatchLine(char prefix, string value, bool noFinalNewline)
        {
            patch.Append(prefix).Append(value).Append('\n');
            if (noFinalNewline)
            {
                patch.Append("\\ No newline at end of file\n");
            }
        }
    }

    private static FileLines GetLines(string value)
    {
        if (value.Contains('\r', StringComparison.Ordinal))
        {
            throw new InvalidDataException(
                "Issue #4 patch input must use LF newlines.");
        }

        var lines = value.Split('\n').ToList();
        if (value.EndsWith('\n') && lines.Count != 0)
        {
            lines.RemoveAt(lines.Count - 1);
        }

        return new FileLines(lines, value.EndsWith('\n'));
    }

    private static IReadOnlyList<Hunk> GetHunks(
        IReadOnlyList<int> changedLines,
        int lineCount)
    {
        var groups = new List<(int First, int Last)>();
        var first = changedLines[0];
        var last = first;
        foreach (var line in changedLines.Skip(1))
        {
            if (line - last <= 6)
            {
                last = line;
            }
            else
            {
                groups.Add((first, last));
                first = line;
                last = line;
            }
        }

        groups.Add((first, last));
        return groups.Select(group =>
            new Hunk(
                Math.Max(0, group.First - 3),
                Math.Min(lineCount, group.Last + 4))).ToArray();
    }

    private readonly record struct Hunk(int Start, int End);

    private sealed record FileLines(
        IReadOnlyList<string> Values,
        bool EndsWithNewline);
}
