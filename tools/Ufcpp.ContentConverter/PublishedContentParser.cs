using System.Xml;
using System.Xml.Linq;

namespace Ufcpp.ContentConverter;

public static class PublishedContentParser
{
    private static readonly HashSet<string> KnownContentTypes =
    [
        "AboutMe",
        "Article",
        "BlogEntry",
        "BlogMonth",
        "BlogTop",
        "BlogYear",
        "Chapter",
        "ErrorNotFound",
        "ErrorServer",
        "Exercise",
        "ExerciseList",
        "Home",
        "Rss",
        "RssBlog",
        "Search",
        "Settings",
        "Sitemap",
        "StudyTop",
        "Subject",
        "SubjectGroup",
    ];

    private static readonly IReadOnlyDictionary<string, string[]> RequiredProperties =
        new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            ["AboutMe"] = ["bodyText"],
            ["Article"] = ["bodyText"],
            ["BlogEntry"] = ["bodyText"],
            ["Exercise"] = ["questionText"],
        };

    public static PublishedSnapshot Load(string path)
    {
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore,
            XmlResolver = null,
        };

        using var reader = XmlReader.Create(path, settings);
        var document = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
        var xmlRoot = document.Root ?? throw new InvalidDataException("The published snapshot has no root element.");
        var homeElements = xmlRoot.Elements().Where(IsDocumentNode).ToArray();
        if (homeElements.Length != 1)
        {
            throw new InvalidDataException($"Expected one published root node, found {homeElements.Length}.");
        }

        var nodes = new List<ContentNode>();
        var home = ParseNode(homeElements[0], null, nodes);
        Validate(home, nodes);
        return new PublishedSnapshot(home, nodes, nodes.ToDictionary(node => node.Id));
    }

    private static ContentNode ParseNode(XElement element, ContentNode? parent, List<ContentNode> nodes)
    {
        var contentType = RequiredAttribute(element, "nodeTypeAlias");
        if (!KnownContentTypes.Contains(contentType))
        {
            throw new InvalidDataException($"Unknown document type '{contentType}'.");
        }

        var node = new ContentNode
        {
            Id = ParseInt(element, "id"),
            ParentId = ParseInt(element, "parentID"),
            Level = ParseInt(element, "level"),
            SortOrder = ParseInt(element, "sortOrder"),
            NodeName = RequiredAttribute(element, "nodeName"),
            UrlName = RequiredAttribute(element, "urlName"),
            ContentType = contentType,
            CreateDate = RequiredAttribute(element, "createDate"),
            UpdateDate = RequiredAttribute(element, "updateDate"),
            Parent = parent,
        };

        foreach (var property in element.Elements().Where(child => !IsDocumentNode(child)))
        {
            if (!node.Properties.TryAdd(property.Name.LocalName, property.Value))
            {
                throw new InvalidDataException(
                    $"Node {node.Id} contains duplicate property '{property.Name.LocalName}'.");
            }
        }

        if (RequiredProperties.TryGetValue(node.ContentType, out var requiredProperties))
        {
            foreach (var property in requiredProperties)
            {
                if (!node.Properties.ContainsKey(property))
                {
                    if (IsKnownMissingProperty(node, property))
                    {
                        node.RecordKnownMissingProperty(property);
                        continue;
                    }

                    throw new InvalidDataException(
                        $"Node {node.Id} ({node.ContentType}) is missing required property '{property}'.");
                }
            }
        }

        nodes.Add(node);
        if (parent is not null)
        {
            parent.Children.Add(node);
        }

        foreach (var child in element.Elements().Where(IsDocumentNode))
        {
            ParseNode(child, node, nodes);
        }

        return node;
    }

    private static bool IsKnownMissingProperty(ContentNode node, string property) =>
        property.Equals("bodyText", StringComparison.Ordinal)
        && node.Id == 1410
        && node.ParentId == 1408
        && node.ContentType == "Article"
        && node.UrlName == "keywords"
        && node.Properties.TryGetValue("title", out var title)
        && title == "重要語句一覧";

    private static void Validate(ContentNode home, IReadOnlyCollection<ContentNode> nodes)
    {
        var duplicate = nodes.GroupBy(node => node.Id).FirstOrDefault(group => group.Count() != 1);
        if (duplicate is not null)
        {
            throw new InvalidDataException($"Duplicate node ID {duplicate.Key}.");
        }

        var byId = nodes.ToDictionary(node => node.Id);
        foreach (var node in nodes)
        {
            if (node == home)
            {
                if (node.ParentId != -1 || node.ContentType != "Home")
                {
                    throw new InvalidDataException("The root content node must be Home with parent ID -1.");
                }

                continue;
            }

            if (!byId.TryGetValue(node.ParentId, out var declaredParent))
            {
                throw new InvalidDataException($"Node {node.Id} has missing parent {node.ParentId}.");
            }

            if (!ReferenceEquals(declaredParent, node.Parent))
            {
                throw new InvalidDataException(
                    $"Node {node.Id} is nested below {node.Parent?.Id} but declares parent {node.ParentId}.");
            }
        }

        var visiting = new HashSet<int>();
        var visited = new HashSet<int>();
        Visit(home, visiting, visited);
        if (visited.Count != nodes.Count)
        {
            throw new InvalidDataException("The hierarchy contains disconnected nodes.");
        }
    }

    private static void Visit(ContentNode node, HashSet<int> visiting, HashSet<int> visited)
    {
        if (!visiting.Add(node.Id))
        {
            throw new InvalidDataException($"The hierarchy contains a cycle at node {node.Id}.");
        }

        foreach (var child in node.Children)
        {
            Visit(child, visiting, visited);
        }

        visiting.Remove(node.Id);
        visited.Add(node.Id);
    }

    private static bool IsDocumentNode(XElement element) => element.Attribute("nodeTypeAlias") is not null;

    private static string RequiredAttribute(XElement element, string name) =>
        element.Attribute(name)?.Value
        ?? throw new InvalidDataException($"Element '{element.Name}' is missing attribute '{name}'.");

    private static int ParseInt(XElement element, string name)
    {
        if (!int.TryParse(RequiredAttribute(element, name), out var value))
        {
            throw new InvalidDataException($"Attribute '{name}' on '{element.Name}' is not an integer.");
        }

        return value;
    }
}
