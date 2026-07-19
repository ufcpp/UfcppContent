using System.Text;
using System.Text.Json;

namespace Ufcpp.ContentConverter;

public static class ExerciseRenderer
{
    public static IReadOnlyList<string> ParseAnswers(ContentNode exercise)
    {
        var json = exercise.Get("answerText");
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidDataException(
                    $"Exercise {exercise.Id} answerText must be a JSON array.");
            }

            var answers = new List<string>();
            foreach (var item in document.RootElement.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object
                    || !item.TryGetProperty("value", out var value)
                    || value.ValueKind != JsonValueKind.String)
                {
                    throw new InvalidDataException(
                        $"Exercise {exercise.Id} answerText entries must contain a string value.");
                }

                answers.Add(value.GetString() ?? string.Empty);
            }

            return answers;
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                $"Exercise {exercise.Id} has malformed answerText JSON.", exception);
        }
    }

    public static string RenderForArticle(
        ContentNode article,
        Func<string, ContentNode, string> rewrite)
    {
        var exercises = article.Children
            .Where(node => node.ContentType == "Exercise")
            .OrderBy(node => node.SortOrder)
            .ThenBy(node => node.Id)
            .ToArray();
        if (exercises.Length == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        builder.AppendLine();
        builder.AppendLine("## <a id=\"exercise\"></a>演習問題");
        builder.AppendLine();
        RenderExercises(builder, exercises, string.Empty, rewrite);
        return builder.ToString();
    }

    public static string RenderForList(
        ContentNode exerciseList,
        Func<string, ContentNode, string> rewrite)
    {
        var subject = exerciseList.AncestorsAndSelf()
            .FirstOrDefault(node => node.ContentType == "Subject")
            ?? throw new InvalidDataException($"ExerciseList {exerciseList.Id} is not below a Subject.");
        var groups = subject.Descendants()
            .Where(node => node.ContentType == "Exercise")
            .GroupBy(node => node.Parent
                ?? throw new InvalidDataException($"Exercise {node.Id} has no Article parent."))
            .ToArray();

        var builder = new StringBuilder();
        var introduction = exerciseList.Get("introduction");
        if (!string.IsNullOrWhiteSpace(introduction))
        {
            builder.AppendLine(rewrite(introduction, exerciseList));
            builder.AppendLine();
        }

        foreach (var group in groups)
        {
            var article = group.Key;
            var articleLink = rewrite($"[{EscapeMarkdown(article.Title)}]({ContentPaths.CanonicalUrl(article)})", exerciseList);
            builder.AppendLine($"## <a id=\"{article.Id}\"></a>{articleLink}");
            builder.AppendLine();
            RenderExercises(
                builder,
                group.OrderBy(node => node.SortOrder).ThenBy(node => node.Id).ToArray(),
                article.Id + "-",
                rewrite,
                exerciseList);
        }

        return builder.ToString();
    }

    private static void RenderExercises(
        StringBuilder builder,
        IReadOnlyList<ContentNode> exercises,
        string prefix,
        Func<string, ContentNode, string> rewrite,
        ContentNode? linkContext = null)
    {
        for (var index = 0; index < exercises.Count; index++)
        {
            var exercise = exercises[index];
            var context = linkContext ?? exercise.Parent
                ?? throw new InvalidDataException($"Exercise {exercise.Id} has no parent.");
            builder.AppendLine(
                $"### <a id=\"{prefix}exercise-{exercise.NodeName}\"></a>問題 {index + 1}");
            builder.AppendLine();
            builder.AppendLine(rewrite(exercise.Get("questionText"), context));
            builder.AppendLine();
            var answers = ParseAnswers(exercise);
            for (var answerIndex = 0; answerIndex < answers.Count; answerIndex++)
            {
                builder.AppendLine($"#### 解答例 {answerIndex + 1}");
                builder.AppendLine();
                builder.AppendLine(rewrite(answers[answerIndex], context));
                builder.AppendLine();
            }
        }
    }

    private static string EscapeMarkdown(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("[", "\\[", StringComparison.Ordinal)
            .Replace("]", "\\]", StringComparison.Ordinal);
}
