using System.Security.Cryptography;
using System.Text;

namespace Ufcpp.CodeAnnotationMigrator;

internal sealed record Issue4SourceCleanup(
    string Opening,
    string Closing);

internal sealed record Issue4OverrideApplication(
    string CurrentDocument,
    ReportPlan Plan);

internal sealed record Issue4OverrideEntry(
    string Id,
    string Path,
    int HistoricalOrdinal,
    int HistoricalLine,
    string? HistoricalHash,
    string? HistoricalDocumentBlob,
    int CurrentOrdinal,
    int CurrentLine,
    IReadOnlyList<string> CurrentHashes,
    string? Title,
    string? HighlightText,
    Issue4SourceCleanup? Cleanup,
    string Reason = "")
{
    public Issue4OverrideApplication CreateApplication(
        string path,
        string historicalDocument,
        string currentDocument)
    {
        if (!string.Equals(path, Path, StringComparison.Ordinal))
        {
            throw Invalid($"path '{path}' does not match '{Path}'.");
        }

        ValidateHistorical(historicalDocument);
        var current = GetCurrent(currentDocument);
        var hash = CodeNormalizer.Hash(current.Code);
        if (!CurrentHashes.Contains(hash, StringComparer.Ordinal))
        {
            throw Invalid(
                $"current block hash '{hash}' is not an allowed preimage.");
        }

        if (Cleanup is not null)
        {
            currentDocument = ApplyCleanup(currentDocument, current);
            current = GetCurrent(currentDocument);
            hash = CodeNormalizer.Hash(current.Code);
            if (!CurrentHashes.Contains(hash, StringComparer.Ordinal))
            {
                throw Invalid(
                    $"cleaned current block hash '{hash}' is not an allowed postimage.");
            }
        }

        return new Issue4OverrideApplication(
            currentDocument,
            new ReportPlan(
                Path,
                HistoricalOrdinal,
                CurrentOrdinal,
                HistoricalLine,
                current.SourceLine,
                current.Kind == CurrentCodeBlockKind.Fenced
                    ? "fenced"
                    : current.IsInsideTable
                        ? "rawPreInTable"
                        : "rawPre",
                "explicitOverride",
                hash,
                new BlockMetadataPlan(
                    Title,
                    HighlightText is null
                        ? null
                        : new SelectionMetadataPlan(null, HighlightText),
                    null,
                    null)));
    }

    private void ValidateHistorical(string historicalDocument)
    {
        if (HistoricalHash is not null)
        {
            var parsing = LegacyPreParser.ParseDetailed(historicalDocument);
            var block = parsing.Blocks.SingleOrDefault(
                block => block.Ordinal == HistoricalOrdinal);
            if (block is null
                || block.SourceLine != HistoricalLine
                || !string.Equals(
                    CodeNormalizer.Hash(block.Code),
                    HistoricalHash,
                    StringComparison.Ordinal))
            {
                throw Invalid("historical block identity is stale.");
            }

            return;
        }

        if (HistoricalDocumentBlob is null
            || !string.Equals(
                GitBlobId.Compute(historicalDocument),
                HistoricalDocumentBlob,
                StringComparison.Ordinal))
        {
            throw Invalid("malformed historical document blob is stale.");
        }
    }

    private CurrentCodeBlock GetCurrent(string currentDocument)
    {
        var blocks = CurrentBlockDiscoverer.Discover(currentDocument);
        if (CurrentOrdinal <= 0 || CurrentOrdinal > blocks.Count)
        {
            throw Invalid($"current block {CurrentOrdinal} does not exist.");
        }

        var current = blocks[CurrentOrdinal - 1];
        if (current.SourceLine != CurrentLine)
        {
            throw Invalid(
                $"current block line {current.SourceLine} does not match "
                + $"{CurrentLine}.");
        }

        return current;
    }

    private string ApplyCleanup(
        string currentDocument,
        CurrentCodeBlock current)
    {
        var end = FindFencedBlockEnd(currentDocument, current.SourceOffset);
        var opening = currentDocument.IndexOf(
            Cleanup!.Opening,
            current.SourceOffset,
            end - current.SourceOffset,
            StringComparison.Ordinal);
        var closing = currentDocument.IndexOf(
            Cleanup.Closing,
            current.SourceOffset,
            end - current.SourceOffset,
            StringComparison.Ordinal);
        if (opening < 0 && closing < 0)
        {
            return currentDocument;
        }

        if (opening < 0
            || closing < opening + Cleanup.Opening.Length
            || currentDocument.IndexOf(
                Cleanup.Opening,
                opening + Cleanup.Opening.Length,
                end - opening - Cleanup.Opening.Length,
                StringComparison.Ordinal) >= 0
            || currentDocument.IndexOf(
                Cleanup.Closing,
                closing + Cleanup.Closing.Length,
                end - closing - Cleanup.Closing.Length,
                StringComparison.Ordinal) >= 0)
        {
            throw Invalid("literal annotation cleanup guards do not match.");
        }

        return currentDocument.Remove(closing, Cleanup.Closing.Length)
            .Remove(opening, Cleanup.Opening.Length);
    }

    private static int FindFencedBlockEnd(string document, int openingOffset)
    {
        var openingLineEnd = document.IndexOf('\n', openingOffset);
        if (openingLineEnd < 0)
        {
            throw new InvalidDataException("Override fence has no body.");
        }

        var openingLine = document[openingOffset..openingLineEnd].TrimStart(' ', '\t');
        var markerLength = openingLine.TakeWhile(character =>
            character == openingLine[0]).Count();
        var marker = openingLine[0];
        for (var offset = openingLineEnd + 1; offset < document.Length;)
        {
            var lineEnd = document.IndexOf('\n', offset);
            if (lineEnd < 0)
            {
                lineEnd = document.Length;
            }

            var line = document[offset..lineEnd].Trim();
            if (line.Length >= markerLength
                && line.All(character => character == marker))
            {
                return lineEnd;
            }

            offset = lineEnd + 1;
        }

        throw new InvalidDataException("Override fence has no closing marker.");
    }

    private InvalidDataException Invalid(string message) =>
        new($"Issue #4 override {Id}: {message}");
}

internal static class Issue4OverrideCatalog
{
    public static IReadOnlyList<Issue4OverrideEntry> Entries { get; } =
    [
        new(
            "OVR-BLOCK-1",
            "study/csharp/cheatsheet/ap_ver7_2.md",
            10,
            266,
            "2b75ecc97bc061d1ead8ac16d16174d20d9dbcf304f7c7f03c7bfae1ee645601",
            null,
            10,
            277,
            ["a284da6fb4c0b27a02d221416d13df3cfb8a9848bd4332dc89b901bd0e0f50c0"],
            "ref構造体を持てるのはref構造体だけ",
            null,
            null,
            "The legacy block contained an unescaped <T> element that was "
            + "removed from visible text; all other lines and block ordinals "
            + "identify the live current fence exactly."),
        new(
            "OVR-BLOCK-2",
            "study/sp/dsp/frequency.md",
            1,
            553,
            "ec1c524488ee0f7f790624a10ca17c31395845a02b19364a6e821eadad32dd50",
            null,
            1,
            568,
            [
                "717741417ca8f0b58d01d1471b20ef7246782de665d4f7ce7f40b5d87c2895a0",
                "d2ba44afa30030153e1a74aeafafb6c92a1ce22c5ceeaa15da4574319b418ce0",
            ],
            null,
            "比の対数を取る必要がある",
            new Issue4SourceCleanup("<em>", "</em>"),
            "The sole current fence retained the historical <em> tags as "
            + "literal code. Removing that exact pair restores the annotation "
            + "without touching the five unrelated literal <sub> pairs."),
        new(
            "OVR-MALFORMED-1",
            "blog/2016/12/tipsuniontypes/index.md",
            6,
            174,
            null,
            "71be331857f39fcda4508102a3b62936b5156dcc",
            6,
            174,
            ["3d741f1dafc9971c79939d32735d163d8c6537a92a6dce5be03a5754bc779539"],
            "(F#)メソッドを呼び出す側",
            null,
            null,
            "The pinned block has a stray </code> without an opening wrapper; "
            + "its four-line visible body exactly matches current block 6."),
        new(
            "OVR-MALFORMED-2",
            "blog/2022/1/defaultable/index.md",
            2,
            52,
            null,
            "919c6dd1ba0fd29ad96b6d904262cf8777d184ff",
            2,
            52,
            ["43d71c1c40495710629f0a2af08518101e069dcd49a8628512a0c645bea3535f"],
            "default を介して null が紛れ込む例",
            null,
            null,
            "A missing quote and > in a warning span makes the pinned block "
            + "malformed; the guarded current body is otherwise exact."),
        new(
            "OVR-MALFORMED-3",
            "study/csharp/oop/oo_interface.md",
            6,
            361,
            null,
            "be276b5b34506892033f6fa3d9a4107103130865",
            6,
            368,
            ["cbab8381788461e9d7f977959557256acdcc84ae4ccd2612edc56dfeef2cbdcb"],
            "IDisposableの例",
            null,
            null,
            "The pinned wrapper misspells <code> as <coe> and contains a stray "
            + "</span>; the guarded current body is the live replacement."),
        new(
            "OVR-MALFORMED-4",
            "study/csharp/oop/oo_interface.md",
            13,
            623,
            null,
            "be276b5b34506892033f6fa3d9a4107103130865",
            13,
            635,
            ["67e51d0682daa99dd579e178d1280f20cea82e241dc89aad0c25fd4118bc3cd0"],
            "IEnumerableの例",
            "IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
            null,
            "The pinned block has a stray </code>; its guarded current body "
            + "contains the one exact highlighted explicit implementation."),
        new(
            "OVR-MALFORMED-5",
            "study/csharp/structured/oo_exception.md",
            12,
            299,
            null,
            "374eed61f228a01673ba265bbfa97bea718076b7",
            12,
            302,
            ["ae20f5621a04db93b99b2514585b2b767326c8fbda70c5a7cf5566abbb599163"],
            "同じ型のcatchを並べるとエラー",
            null,
            null,
            "The pinned block has an orphan </em> and no opening <em>, so it "
            + "carries a title but no genuine highlight."),
    ];
}

internal static class GitBlobId
{
    public static string Compute(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var header = Encoding.ASCII.GetBytes($"blob {bytes.Length}\0");
        var buffer = new byte[header.Length + bytes.Length];
        header.CopyTo(buffer, 0);
        bytes.CopyTo(buffer, header.Length);
        return Convert.ToHexString(SHA1.HashData(buffer)).ToLowerInvariant();
    }
}
