---
title: "【C# 11 候補】 UTF-8 リテラル"
source_url: "https://ufcpp.net/blog/2021/12/utf8-literal/"
content_type: "BlogEntry"
published_at: "2021-12-28T14:45:05"
updated_at: "2024-02-08T20:34:04"
tags: []
umbraco_id: 2394
parent_id: 2375
sort_order: 9
aliases: []
---

# 【C# 11 候補】 UTF-8 リテラル

.NET の UTF-8 対応がらみの続報。

## byte でやりくり

元々、`string` (UTF-16 でデータを持ってる)に加えて `Utf8String` みたいな名前で UTF-8 な型を追加しようか何て話もあったんですが。
`string` と `Utf8String` の2重管理がしんどいだろう、これだけ `string` 前提で .NET エコシステムが確立された状況で追加は無理だろうという雰囲気になっています。

`string` の中身を UTF-8 に変更した方が建設的かもしれないという話も出るくらいですが、さすがにそれをやりだすと大工事過ぎて短期では無理でしょう。
著者個人的にも「10年先ならわからないけども」くらいのお気持ちになりつつあります。

そうこうしているうちに、「生 byte 列で UTF-8 を扱う」と言うのが .NET エコシステム内でデファクトスタンダード化してしまいました(今ここ)。
例えば `System.Text.Unicode` 名前空間中のメソッドは以下のような感じになっています。

```csharp {highlight-ranges="8:36-8:58,12:9-12:34"}
using System.Buffers;

namespace System.Text.Unicode;

public static class Utf8
{
    public static OperationStatus FromUtf16(
        ReadOnlySpan<char> source, Span<byte> destination, out int charsRead, out int bytesWritten,
        bool replaceInvalidSequences = true, bool isFinalBlock = true);

    public static OperationStatus ToUtf16(
        ReadOnlySpan<byte> source, Span<char> destination, out int bytesRead, out int charsWritten,
        bool replaceInvalidSequences = true, bool isFinalBlock = true);
}
```

`Span<byte>` と `ReadOnlySpan<byte>` で UTF-8 文字列を扱っています。

文字なのかその他のバイナリ形式なのかがわからなくなるんであんまり親切設計ではないんですが…
型変換やオーバーロードをあんまり増やすのもしんどく、
「生 byte 列で UTF-8 を扱う」は結構定着しちゃうんじゃないかという感じ。

## リテラル問題

とはいえ。
UTF-8 扱いで `Span<byte>` とかを使うにあたって困るのが文字列リテラル。
今だと以下のように `byte` 定数的に `new byte[]` するしか方法がありません。

```csharp {title="UTF-8 代わりの byte 定数"}
ReadOnlySpan<byte> _true = new byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
ReadOnlySpan<byte> _false = new byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
ReadOnlySpan<byte> _null = new byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
```

一応、これ、最適化はされて `new byte[]` のヒープ アロケーションは発生せず、
直接 DLL 中のデータ領域からデータが読まれます。

この3つくらいならいいんですけども、極まってくるとありとあらゆる文字列リテラルを UTF-8 byte 列化したくなり…

* [HTTP のヘッダーとかで使う文字列](https://github.com/dotnet/aspnetcore/blob/8b30d862de6c9146f466061d51aa3f1414ee2337/src/Servers/Kestrel/Core/src/Internal/Http2/Http2Connection.Generated.cs)
* [HTTP のステータス コード](https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Shared/runtime/Http2/Hpack/StatusCodes.cs)

とかを見てもらえるとなかなかにつらみを感じてもらえるのではないかと思います。

`"100"` みたいなものすら `new byte[] { (byte)'1', (byte)'0', (byte)'0' }`。

## UTF-8 文字列リテラル

と言うことで着地点として、[リテラルだけ UTF-8 なものを用意しようか](https://github.com/dotnet/csharplang/blob/main/proposals/utf8-string-literals.md)という雰囲気になっています。

* `Span<byte>` や `ReadSpan<byte>` に対して文字列リテラルを渡すと自動的に上記のような UTF-8 byte 列を生成する
* オーバーロード解決や `var` 型推論用に `u8` 接尾辞を用意

例えば以下のように書けるようになります。

暗黙的変換:

```csharp {title="byte[] とかへの文字列リテラル代入は UTF-8 byte 列扱い"}
byte[] array = "hello";             // new byte[] { 0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20 }
Span<byte> span = "dog";            // new byte[] { 0x64, 0x6f, 0x67 }
ReadOnlySpan<byte> span = "cat";    // new byte[] { 0x63, 0x61, 0x74 }
```

`u8` 接尾辞:

```csharp {title="u8 を付けると UTF-8 リテラルに"}
string s1 = "hello"u8;      // エラー。型が合ってない。
var s2 = "hello"u8;         // Ok。型は ReadOnlySpan<byte>。
Span<byte> s3 = "hello"u8;  // Ok。
byte[] s4 = "hello"u8;      // Ok。
```

UTF-8 として不正になる文字列リテラルはコンパイル エラーにするそうです。
.NET の文字列は UTF-16 というか実際には「古き良き Unicode」(2バイト固定長で行けると思ってた頃の Unicode)なので、「[サロゲート ペア](https://codezine.jp/article/detail/1592)の片割れ」みたいな今となってはダメなやつを受け付けてしまうので。

```csharp {title="不正な UTF-16 はコンパイル エラーに"}
byte[] array = "\uD801"; // ハイ サロゲートのみ。コンパイル エラーにする。
```

ちなみに、`const string` から UTF-8 リテラルも作れるし、
「不正な UTF-16 を `+` でつないで、その結果が有効な UTF-8 になるなら OK」だそうです。

```csharp {title="不正 UTF-16 な const string 2つ → 有効な UTF-8 リテラル"}
const string first = "\uD83D";  // ハイ サロゲート。
const string second = "\uDE00"; // ロー サロゲート。
ReadOnlySpan<byte> span = first + second; // これは OK
```

## Utf8String 型の可能性

前述の通り、今の `string` を置き換えるような `Utf8String` 型が追加される可能性はかなり低くなってきたんですが。

一応まだ可能性 0 とは断じない方がいいので、一応この仮定的な `Utf8String` の存在は考慮しているそうです。

もしも `Utf8String` が積極的に使いたい「良い型」になったとしても、
たぶん、`""` から `byte[]`、`Span<byte>`、`ReadOnlySpan<byte>` への暗黙的変換は対して問題にならなさそう。

後悔するとしたら `u8` 接尾辞の「自然な型」を `ReadOnlySpan<byte>` にしてしまう点で、これに関しては「やっぱり `Utf8String` に変えたい」となっても変えれるものではなくなります。
とはいえ、「なので今は自然な型を決めるのはやめておこう」と思うほどのものではない(ので、`ReadOnlySpan<byte>` な方針でいく)でしょう。
