using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Ufcpp.ContentConverter;

public static class TextUtilities
{
    public static readonly UTF8Encoding Utf8NoBom = new(false, true);

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static string NormalizeNewlines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');

    public static string YamlQuote(string value) =>
        JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });

    public static string Sha256File(string path)
    {
        using var stream = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }

    public static string Sha256Text(string value) =>
        Convert.ToHexString(SHA256.HashData(Utf8NoBom.GetBytes(value))).ToLowerInvariant();

    public static void WriteText(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, NormalizeNewlines(content).TrimEnd() + "\n", Utf8NoBom);
    }

    public static void WriteJson(string path, object value) =>
        WriteText(path, JsonSerializer.Serialize(value, JsonOptions));
}
