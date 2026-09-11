using System.Text;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>Writes the root 404 page used to recover case-mismatched page URLs.</summary>
public static class NotFoundWriter
{
    public const string OutputPath = "404.html";

    public static void Write(string outputDirectory)
    {
        var outputPath = Path.Combine(outputDirectory, OutputPath);
        File.WriteAllText(
            outputPath,
            Html,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private const string Html = """
        <!DOCTYPE html>
        <html lang="ja">
        <head>
        <meta charset="UTF-8" />
        <meta name="robots" content="noindex, nofollow" />
        <script>
        (function () {
          var path = location.pathname;
          var lastSegment = path.substring(path.lastIndexOf("/") + 1);
          var isPagePath = path.endsWith("/") || !lastSegment.includes(".");
          var lowerPath = path.toLowerCase();
          if (isPagePath && lowerPath !== path) {
            location.replace(lowerPath + location.search + location.hash);
          }
        })();
        </script>
        <title>Page not found</title>
        </head>
        <body>
        <h1>Page not found</h1>
        </body>
        </html>
        """;
}
