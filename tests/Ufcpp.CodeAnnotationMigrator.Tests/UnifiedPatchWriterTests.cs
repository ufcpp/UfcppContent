using System.Text;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

public sealed class UnifiedPatchWriterTests
{
    [Fact]
    public void Write_IsDeterministicGuardedUtf8AndContextual()
    {
        const string Before = "zero\none\ntwo\nthree\nfour\n";
        const string After = "zero\none changed\ntwo\nthree\nfour changed\n";
        var changes = new Dictionary<string, DocumentChange>
        {
            ["content/日本語.md"] = new(Before, After),
        };

        var first = UnifiedPatchWriter.Write(
            "0123456789012345678901234567890123456789",
            changes);
        var second = UnifiedPatchWriter.Write(
            "0123456789012345678901234567890123456789",
            changes);
        var patch = Encoding.UTF8.GetString(first);

        Assert.Equal(first, second);
        Assert.NotEqual(0xEF, first[0]);
        Assert.Contains(
            $"index {GitBlobId.Compute(Before)}..{GitBlobId.Compute(After)} 100644",
            patch);
        Assert.Contains("--- a/content/日本語.md", patch);
        Assert.Contains("+++ b/content/日本語.md", patch);
        Assert.Contains("-one", patch);
        Assert.Contains("+one changed", patch);
        Assert.Contains("-four", patch);
        Assert.Contains("+four changed", patch);
    }

    [Fact]
    public void Write_EmptyChangeSetProducesNoBytes()
    {
        Assert.Empty(
            UnifiedPatchWriter.Write(
                "0123456789012345678901234567890123456789",
                new Dictionary<string, DocumentChange>()));
    }

    [Fact]
    public void Write_EmitsGitMarkersForMissingFinalNewline()
    {
        const string Before = "intro\n```text\nvalue\n```";
        const string After = "intro\n```text {title=\"sample\"}\nvalue\n```";

        var patch = Encoding.UTF8.GetString(
            UnifiedPatchWriter.Write(
                "0123456789012345678901234567890123456789",
                new Dictionary<string, DocumentChange>
                {
                    ["content/sample.md"] = new(Before, After),
                }));

        Assert.EndsWith(
            " ```\n\\ No newline at end of file\n",
            patch,
            StringComparison.Ordinal);
    }
}
