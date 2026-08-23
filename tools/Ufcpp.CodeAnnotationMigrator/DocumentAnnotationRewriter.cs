namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record DocumentRewriteResult(
    string Content,
    int ReplacementCount);

internal static class DocumentAnnotationRewriter
{
    private static readonly string[] MetadataOrder =
    [
        "title",
        "highlight-lines",
        "highlight-text",
        "highlight-ranges",
    ];

    public static DocumentRewriteResult Rewrite(
        string path,
        string source,
        IReadOnlyList<ReportPlan> plans)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(plans);

        var blocks = CurrentBlockDiscoverer.Discover(source);
        var replacements = new List<DocumentSourceReplacement>();
        foreach (var plan in plans.OrderBy(static plan => plan.CurrentOrdinal))
        {
            if (!string.Equals(plan.Path, path, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Rewrite plan path '{plan.Path}' does not match '{path}'.");
            }

            if (plan.CurrentOrdinal <= 0 || plan.CurrentOrdinal > blocks.Count)
            {
                throw new InvalidDataException(
                    $"Rewrite plan references missing block {plan.CurrentOrdinal} "
                    + $"in '{path}'.");
            }

            var current = blocks[plan.CurrentOrdinal - 1];
            if (!string.Equals(
                    CodeNormalizer.Hash(current.Code),
                    plan.Hash,
                    StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Rewrite plan hash for block {plan.CurrentOrdinal} in "
                    + $"'{path}' is stale.");
            }

            if (current.Kind == CurrentCodeBlockKind.RawPre && current.IsInsideTable)
            {
                replacements.AddRange(
                    RawTableAnnotationRewriter.Plan(path, source, current, plan));
                continue;
            }

            if (current.Kind != CurrentCodeBlockKind.Fenced)
            {
                throw new InvalidDataException(
                    $"Rewrite plan block {plan.CurrentOrdinal} in '{path}' is "
                    + "not a fenced block.");
            }

            var lineStart = FindLineStart(source, current.SourceOffset);
            var lineEnd = FindLineEnd(source, current.SourceOffset);
            var openingLine = source[lineStart..lineEnd];
            var desired = DesiredMetadata(plan.Metadata);
            if (desired.Count == 0)
            {
                continue;
            }

            var metadata = ExistingMetadata(openingLine);
            foreach (var (name, value) in desired)
            {
                if (metadata.TryGetValue(name, out var existing)
                    && !string.Equals(existing, value, StringComparison.Ordinal))
                {
                    throw new InvalidDataException(
                        $"Existing {name} metadata on block {plan.CurrentOrdinal} "
                        + $"in '{path}' conflicts with the migration plan.");
                }

                metadata[name] = value;
            }

            var attributesStart = openingLine.IndexOf('{');
            var prefix = (attributesStart < 0
                    ? openingLine
                    : openingLine[..attributesStart])
                .TrimEnd(' ', '\t');
            var replacement =
                $"{prefix} {{{SerializeMetadata(metadata)}}}";
            if (!string.Equals(openingLine, replacement, StringComparison.Ordinal))
            {
                replacements.Add(
                    new DocumentSourceReplacement(
                        lineStart,
                        lineEnd - lineStart,
                        replacement));
            }
        }

        var rewritten = source;
        foreach (var replacement in replacements
                     .OrderByDescending(static replacement => replacement.Offset))
        {
            rewritten = rewritten.Remove(replacement.Offset, replacement.Length)
                .Insert(replacement.Offset, replacement.Value);
        }

        return new DocumentRewriteResult(rewritten, replacements.Count);
    }

    private static Dictionary<string, string> DesiredMetadata(
        BlockMetadataPlan plan)
    {
        var metadata = new Dictionary<string, string>(StringComparer.Ordinal);
        Add("title", plan.Title);
        Add("highlight-lines", plan.Highlight?.Lines);
        Add("highlight-text", plan.Highlight?.Text);
        Add("highlight-ranges", plan.Highlight?.Ranges);
        return metadata;

        void Add(string name, string? value)
        {
            if (value is not null)
            {
                metadata.Add(name, value);
            }
        }
    }

    private static Dictionary<string, string> ExistingMetadata(string openingLine)
    {
        var metadata = new Dictionary<string, string>(StringComparer.Ordinal);
        var opening = openingLine.IndexOf('{');
        if (opening < 0)
        {
            return metadata;
        }

        var closing = openingLine.LastIndexOf('}');
        if (closing <= opening
            || !string.IsNullOrWhiteSpace(openingLine[(closing + 1)..]))
        {
            throw new InvalidDataException(
                "Fenced block contains malformed metadata braces.");
        }

        var value = openingLine.AsSpan(opening + 1, closing - opening - 1);
        for (var offset = 0; offset < value.Length;)
        {
            while (offset < value.Length && char.IsWhiteSpace(value[offset]))
            {
                offset++;
            }

            if (offset == value.Length)
            {
                break;
            }

            var nameStart = offset;
            while (offset < value.Length
                   && !char.IsWhiteSpace(value[offset])
                   && value[offset] != '=')
            {
                offset++;
            }

            var name = value[nameStart..offset].ToString();
            while (offset < value.Length && char.IsWhiteSpace(value[offset]))
            {
                offset++;
            }

            if (name.Length == 0 || offset >= value.Length || value[offset++] != '=')
            {
                throw new InvalidDataException(
                    "Fenced block contains malformed metadata.");
            }

            while (offset < value.Length && char.IsWhiteSpace(value[offset]))
            {
                offset++;
            }

            if (offset >= value.Length || value[offset] is not ('"' or '\''))
            {
                throw new InvalidDataException(
                    "Fenced block metadata values must be quoted.");
            }

            var quote = value[offset++];
            var contentStart = offset;
            while (offset < value.Length && value[offset] != quote)
            {
                offset++;
            }

            if (offset >= value.Length)
            {
                throw new InvalidDataException(
                    "Fenced block metadata has an unclosed quote.");
            }

            var content = System.Net.WebUtility.HtmlDecode(
                value[contentStart..offset++].ToString());
            if (!MetadataOrder.Contains(name, StringComparer.Ordinal)
                || !metadata.TryAdd(name, content))
            {
                throw new InvalidDataException(
                    "Fenced block contains unsupported or duplicate metadata.");
            }
        }

        return metadata;
    }

    private static string SerializeMetadata(
        IReadOnlyDictionary<string, string> metadata) =>
        string.Join(
            ' ',
            MetadataOrder
                .Where(metadata.ContainsKey)
                .Select(name => $"{name}={Quote(metadata[name])}"));

    private static string Quote(string value)
    {
        var encoded = value
            .Replace("&", "&amp;", StringComparison.Ordinal)
            .Replace("\"", "&quot;", StringComparison.Ordinal)
            .Replace("<", "&lt;", StringComparison.Ordinal)
            .Replace(">", "&gt;", StringComparison.Ordinal)
            .Replace("`", "&#96;", StringComparison.Ordinal);
        return $"\"{encoded}\"";
    }

    private static int FindLineStart(string source, int offset)
    {
        var lineFeed = source.LastIndexOf('\n', Math.Max(0, offset - 1));
        var carriageReturn = source.LastIndexOf('\r', Math.Max(0, offset - 1));
        return Math.Max(lineFeed, carriageReturn) + 1;
    }

    private static int FindLineEnd(string source, int offset)
    {
        var lineFeed = source.IndexOf('\n', offset);
        var carriageReturn = source.IndexOf('\r', offset);
        if (lineFeed < 0)
        {
            return carriageReturn < 0 ? source.Length : carriageReturn;
        }

        return carriageReturn < 0 ? lineFeed : Math.Min(lineFeed, carriageReturn);
    }

}
