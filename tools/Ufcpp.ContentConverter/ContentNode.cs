using System.Globalization;

namespace Ufcpp.ContentConverter;

public sealed class ContentNode
{
    public required int Id { get; init; }

    public required int ParentId { get; init; }

    public required int Level { get; init; }

    public required int SortOrder { get; init; }

    public required string NodeName { get; init; }

    public required string UrlName { get; init; }

    public required string ContentType { get; init; }

    public required string CreateDate { get; init; }

    public required string UpdateDate { get; init; }

    public ContentNode? Parent { get; internal set; }

    public List<ContentNode> Children { get; } = [];

    public Dictionary<string, string> Properties { get; } =
        new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<string> KnownMissingProperties => _knownMissingProperties;

    private readonly List<string> _knownMissingProperties = [];

    internal void RecordKnownMissingProperty(string name) => _knownMissingProperties.Add(name);

    public string Get(string name) =>
        Properties.TryGetValue(name, out var value) ? value : string.Empty;

    public string Title
    {
        get
        {
            var title = Get("title").Trim();
            return title.Length == 0 ? NodeName : title;
        }
    }

    public IEnumerable<ContentNode> AncestorsAndSelf()
    {
        for (ContentNode? current = this; current is not null; current = current.Parent)
        {
            yield return current;
        }
    }

    public IEnumerable<ContentNode> Descendants() =>
        Children.SelectMany(child => new[] { child }.Concat(child.Descendants()));

    public IReadOnlyList<string> Tags
    {
        get
        {
            var raw = ContentType == "BlogEntry" ? Get("categories") : Get("tags");
            return raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(value => value.Length != 0)
                .Distinct(StringComparer.Ordinal)
                .Order(StringComparer.Ordinal)
                .ToArray();
        }
    }

    public string PublishedAt =>
        NormalizeDate(ContentType switch
        {
            "Article" when Get("sinceSet").Length != 0 => Get("sinceSet"),
            "BlogEntry" when Get("firstPublishedDate").Length != 0 => Get("firstPublishedDate"),
            _ => CreateDate,
        });

    public string UpdatedAt =>
        NormalizeDate(ContentType == "Article" && Get("lastUpdatedSet").Length != 0
            ? Get("lastUpdatedSet")
            : UpdateDate);

    private static string NormalizeDate(string value)
    {
        if (!DateTime.TryParse(
                value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind,
                out var date))
        {
            throw new InvalidDataException($"Invalid content date '{value}'.");
        }

        return date.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
    }
}

public sealed record PublishedSnapshot(
    ContentNode Home,
    IReadOnlyList<ContentNode> Nodes,
    IReadOnlyDictionary<int, ContentNode> ById);
