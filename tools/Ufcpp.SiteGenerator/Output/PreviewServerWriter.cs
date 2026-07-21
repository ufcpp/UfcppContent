using System.Text;

namespace Ufcpp.SiteGenerator.Output;

/// <summary>Writes an optional .NET 10 file-based app for local site preview.</summary>
public static class PreviewServerWriter
{
    public const string OutputPath = "server.cs";

    public static Task WriteAsync(string outputDirectory)
    {
        var outputPath = Path.Combine(outputDirectory, OutputPath);
        return File.WriteAllTextAsync(
            outputPath,
            Source + '\n',
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private const string Source = """
        #:sdk Microsoft.NET.Sdk.Web
        #:property TargetFramework=net10.0
        #:property PublishAot=false

        var siteDirectory = Directory.GetCurrentDirectory();
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = siteDirectory,
            WebRootPath = siteDirectory,
        });

        if (string.IsNullOrWhiteSpace(builder.Configuration["urls"]))
        {
            builder.WebHost.UseUrls("http://localhost:8080");
        }

        var app = builder.Build();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        Console.WriteLine($"Serving static files from: {siteDirectory}");
        await app.RunAsync();
        """;
}
