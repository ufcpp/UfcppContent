---
title: "(没) UTF-8 文字列補間"
source_url: "https://ufcpp.net/blog/2024/12/u8-string-interpolation/"
content_type: "BlogEntry"
published_at: "2024-12-19T22:13:12"
updated_at: "2024-12-19T22:13:12"
tags: []
umbraco_id: 2503
parent_id: 2501
sort_order: 1
aliases: []
---

# (没) UTF-8 文字列補間

今日のは、C# 言語機能としては否決されたものの、ほぼ同等のものがライブラリと JIT 時最適化で実現されたという話になります。

ちなみに今日のこの話は .NET 8 の頃の話で、「そういえば去年書いてなかった」ネタになります。

## UTF-8 リテラルがあるなら

C# 11 で [UTF-8 リテラル](../../../../study/csharp/start/st_string.md#utf8-literal)が入って、
C# プログラム中に UTF-8 なバイト列を `ReadOnlySpan<byte>` で直接埋め込めるようになりました。

```csharp
ReadOnlySpan<byte> hex = "0123456789ABCDEF"u8;
```

[割かし何度も書いてたりはしますが](../../../../study/csharp/start/st_string.md#history)、
もう今となっては世の中の多くの文字列が UTF-8 でやり取りされています。
.NET の `string` が UTF-16 ベースなので、UTF-16 ⇔ UTF-8 の変換がそれなりのコストになっていたりします。

そこで、いろんなものを直接 UTF-8 で書き込んだりするようになりました。
今、 .NET のプリミティブ型にも [`IUtf8SpanFormattable` インターフェイス](https://learn.microsoft.com/ja-jp/dotnet/api/system.iutf8spanformattable)とかが実装されていて、UTF-8 文字列化を直接できるようになっています。

そうなると欲しくなるのが UTF-8 に直接書き込める[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)。
以下のようなことをできるといいなぁという要望があります。

```csharp
static byte[] Format(int x, int y) => $"(X: {x:X2}, Y: {y:X2})"u8;
```

要は、文字列補間の `$""` にも `u8` を付けて、直接 UTF-8 を書き込むというもの。

この提案、一瞬本気で検討はされてたんですけども:

* [[Proposal]: u8 string interpolation #7072](https://github.com/dotnet/csharplang/issues/7072)

結論がどうなったかというと、「普通の文字列補間を使って、パフォーマンスを落とさずに UTF-8 補間できるように JIT 最適化したからいいや」という感じで、C# 言語機能は不要ということになりました。

## Utf8.TryWrite

前節の UTF-8 文字列補間(案)に類することをやるために、 .NET 8 移行、以下のような書き方ができます。

```csharp
using System.Text.Unicode;

static byte[] Format(int x, int y)
{
    var temp = (stackalloc byte[64]);

    Utf8.TryWrite(temp, $"(X: {x:X2}, Y: {y:X2})", out var written);

    return temp[..written].ToArray();
}
```

`Utf8` クラスの `TryWrite` メソッドを使って直接 UTF-8 なバイト列を作れます。

ただ、文字列補間の部分は普通の `$""` で書きます。
そして、この文字列補間の展開結果は以下の通り。

```csharp
using System.Text.Unicode;

static byte[] Format(int x, int y)
{
    var temp = (stackalloc byte[64]);

    Utf8.TryWriteInterpolatedStringHandler handler = new(9, 2, temp, out var shouldAppend);

    if (shouldAppend
        && handler.AppendLiteral("X: ")
        && handler.AppendFormatted(x, "X2")
        && handler.AppendLiteral(", Y: ")
        )  handler.AppendFormatted(y, "X2");

    Utf8.TryWrite(temp, ref handler, out var written);

    return temp[..written].ToArray();
}
```

## 定数文字列の最適化

前節、UTF-8 補間に、普通の文字列補間構文を使っているため、
`AppendLiteral("X: ")` とかで、普通の文字列リテラル(UTF-16)が発生しています。
当然ここで「そんなことしたら UTF-16 から UTF-8 への変換コストが発生したりするんじゃないか？」という話になるんですが、
そこが今日の本題。
.NET 8 移行、定数文字列の最適化がものすごい進んだみたいでして。
結果、この変換コストはほとんど発生しないそうです。

`AppendLiteral` の中身はこんな感じ:

* [Utf8.cs#L399C25-L419](https://github.com/dotnet/runtime/blob/4695f3f0e2fcfdd94602a21d6ef0670fc85f6d2a/src/libraries/System.Private.CoreLib/src/System/Text/Unicode/Utf8.cs#L399C25-L419)

内部的に `ReadUtf8` ってのを呼んでるんですけども、これはこちら:

* [UTF8Encoding.Sealed.cs#L158-L171](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Text/UTF8Encoding.Sealed.cs#L158-L171)

説明文に、

> Same as Encoding.UTF8.TryGetBytes, except with refs, returning the number of bytes written (or -1 if the operation fails), and optimized for a constant input.

(`Encoding.UTF8.TryGetBytes` とほとんど一緒で、違いは、ref を使っていて書き込んだバイト数を返すのと、**定数入力に対して最適化される**。)

と書かれていますし、`Intrinsic` 属性が付いています。
この属性が付いているメソッドは「JIT が特別な最適化したものに置き換える」というマーカーでして、
定数文字列の UTF-16 → UTF-8 変換は JIT がやっちゃいます。
この最適化の Pull Request は以下の通り:

* [JIT: Intrinsify UTF16->UTF8 conversion for string literals (Encoding.UTF8.TryGetBytes) #85328](https://github.com/dotnet/runtime/pull/85328)

これのおかげで、
`AppendLiteral("X: ")` のコストがほぼ `AppendLiteral("X: "u8)` (みたいな `ReadOnlySpan<byte>` 引数オーバーロードを足すの)と同レベルになるそうで。

## $""u8 要る？

そこで本題に戻りまして、UTF-8 文字列補間用の構文、すなわち、`$""u8` みたいなものは必要かどうか。

ここまでで説明したように、`Utf8.TryWrite(temp, $"(X: {x:X2}, Y: {y:X2})", out var written);` みたいに普通の文字列補間(C# のコンパイル結果的には UTF-16)で書いても、JIT 時最適化で UTF-16 から UTF-8 の変換コストがかなり低コストになっています。

この事実を踏まえて、[改めて C# コンパイラー チーム内で検討した結果](https://github.com/dotnet/csharplang/blob/227c84c027dee993934d39800aaa6ba8331ea1ea/meetings/2023/LDM-2023-10-16.md#u8-string-interpolation)、
backlog (未処理・未完成品、C# チーム用語的には「よっぽど斬新なアイディアが出てこない限りやらない」)行きになりました。

まあ、確かに、`Utf8.TryWrite` で事足りた感じはあります。
