---
title: "忘れがちなカルチャー依存問題"
source_url: "https://ufcpp.net/blog/2023/3/string-order/"
content_type: "BlogEntry"
published_at: "2023-03-18T17:35:11"
updated_at: "2023-03-18T17:35:11"
tags: []
umbraco_id: 2460
parent_id: 2457
sort_order: 2
aliases: []
---

# 忘れがちなカルチャー依存問題

今日は、「[Globalization Invariant Mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md)」に変更したら、意外と忘れがちなところで差が出たみたいな話。

## Globalization Invariant Mode

[以前に1回ブログに書いてる](../../../2021/8/invariantculture/index.md)んですが、 .NET の文字列 API にはカルチャー依存なものが多くて、
例えば `1.2.ToString()` すらカルチャー依存です。
大陸ヨーロッパだと小数点を `,` にすることが多く、そのあたりの OS でこの `ToString` を実行すると `"1,2"` になります。

一方で、カルチャーごとの書式情報みたいなのは結構データ量が多いので、
WebAssembly みたいなフットプリントを小さくしたい環境では「そのデータを除外したい」要件があったりします。

そこで導入されたのが[Globalization Invariant Mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md)。
`CultureInfo.CurrentCulture` を呼んでも、`InvariantCulture` が返ってくるというモードです。
`1.2.ToString()` も常に `"1.2"` に。

## 文字列比較

日本 (`ja-JP` カルチャー)の場合、`InvariantCulture` (実質 `en-US` カルチャー)との差は日付がらみ(米国は `MM/dd/yyyy`)くらいなので、
大した影響もないはず(フォーマット未指定で日付系の型を `ToString` することがあんまりない)なので早々に Globalization Invariant Mode を有効化しようとしたところ、
`Order`/`OrderBy` が影響を受けていました。

[Globalization Invariant Mode の解説](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md)をよく見てみれば原因は明白で、

> String operations like Compare, IndexOf and LastIndexOf are always performed as ordinal and not linguistic operations regardless of the string comparing options passed to the APIs.

> 文字列操作、例えば Compare, IndexOf, LastIndesOf などは常に ordinal で実行され、言語的な操作はしません。文字列比較のオプションに何を渡しても関係なく。

とのこと。

`CurrentCulture` が `InvariantCulture` に化ける他に、
文字列比較が常に ordinal (文字コード順)になるそうです。

例えば以下のようなコードを実行すると、

```csharp {title="Order がカルチャー依存"}
Console.OutputEncoding = System.Text.Encoding.UTF8;

// InvariantGlobalization true のときはこの行が例外になるのでコメントアウトして実行。
Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");

var data = new[]
{
    // だいたいのカルチャーで、大文字・小文字、アクセント記号違いの文字が並ぶようにソートされる。
    "a",
    "A",
    "b",
    "B",
    "À",
    // トルコ語の時に順序変わるやつ
    "i",
    "I",
    "ı",
    "İ",
};

var @default = data.Order().ToArray(); // 実行結果を見ればわかるものの、未指定は CurrentCulture。
var current = data.Order(StringComparer.CurrentCulture).ToArray();
var invariant = data.Order(StringComparer.InvariantCulture).ToArray();
var ordinal = data.Order(StringComparer.Ordinal).ToArray();

for (int i = 0; i < @default.Length; i++)
{
    Console.WriteLine($"{@default[i]} {current[i]} {invariant[i]} {ordinal[i]}");
}
```

InvariantGlobalization が false なら以下のような結果になります。
無指定が `CurrentCulture` と一緒。
`CurrentCulture` と `InvariantCulture` では一部の並びに変化がありますが、
「大文字・小文字が並ぶ」くらいはどのカルチャーでも共通です。

```console
a a a A
A A A B
À À À I
b b b a
B B B b
ı ı i i
I I I À
i i İ İ
İ İ ı ı
```

そして、InvariantGlobalization を true 以下のような結果に変化します。
無指定と `CurrentCulture` の列だけが変わるかと思いきや、
`InvariantCulture` の列も含めて全部が `Ordinal` とそろいます。
完全に文字コード順なので、ASCII アルファベットは大文字が先で、小文字がまとめて後ろに。
`À` は non-ASCII の文字なのでさらに後ろ。

```console
A A A A
B B B B
I I I I
a a a a
b b b b
i i i i
À À À À
İ İ İ İ
ı ı ı ı
```

ちなみに、日本語でもひらがな・カタカナの並びが変わります。
(カルチャーあり: あ、ア、い、イ。
Ordinal: あ、い、ア、イ。)

## 実は遅い Order()

カルチャー依存のテーブルを引いて順序を決めるのと、
単に文字コードの数値を見てソートするのとではどちらが高速か明白です。

`Order`/`OrderBy` もカルチャー依存ということは…
ベンチマークを取ってみましょう…

```csharp {title="Order のベンチマーク取ってみる"}
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<StringCompareBenchmark>();

public class StringCompareBenchmark
{
    private static readonly string[] _data = """
        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
        """.Split(' ');

    [Benchmark]
    public string[] Order() => _data.Order().ToArray();

    [Benchmark]
    public string[] OrderCurrent() => _data.Order(StringComparer.CurrentCulture).ToArray();

    [Benchmark]
    public string[] OrderInvariant() => _data.Order(StringComparer.InvariantCulture).ToArray();

    [Benchmark]
    public string[] OrderOrdinal() => _data.Order(StringComparer.Ordinal).ToArray();
}
```

結果、うちの環境だと以下のような感じでした。

|         Method |     Mean |     Error |    StdDev |
|--------------- |---------:|----------:|----------:|
|          Order | 8.348 us | 0.0778 us | 0.0727 us |
|   OrderCurrent | 8.399 us | 0.0313 us | 0.0277 us |
| OrderInvariant | 8.453 us | 0.0569 us | 0.0532 us |
|   OrderOrdinal | 2.593 us | 0.0125 us | 0.0111 us |

Ordinal 指定すると3倍以上速くなります。

ちなみに、自分の用途ではどこで何度実行しようと同じ順になりさえすればよくて、
順序は a A b B でも A B a b でもどちらでもよかったので、
もっと早くから Ordinal 指定しておくべき事案でした。
