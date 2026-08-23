---
title: "Length-based switch dispatch"
source_url: "https://ufcpp.net/blog/2023/8/lengthbasedswitch/"
content_type: "BlogEntry"
published_at: "2023-08-03T22:36:41"
updated_at: "2023-08-03T22:36:41"
tags: []
umbraco_id: 2471
parent_id: 2470
sort_order: 0
aliases: []
---

# Length-based switch dispatch

「そういやブログに書いてなかった」ネタ。
[Pull Request](https://github.com/dotnet/roslyn/pull/66081) が通った
プレビュー版(Visual Studio 16.6 Preview 1)でよければ今年の2月頃から使えてた話です。

文字列に対する `switch` に新しい最適化手法が導入されました。

## <a id="traditional-switch">元々の switch のコスト</a>

例として以下のような `switch` を考えます。

```csharp {title="文字列に対する switch の例"}
static int StringSwitch(string s) => s switch
{
    "abc" => 0,
    "def" => 1,
    "ghi" => 2,
    "01234a" => 3,
    "01234b" => 4,
    "01234c" => 5,
    "aaaaaaaa" => 6,
    _ => -1,
};
```

C# コンパイラー的には、

* `case` 少なければ単に上から順に `if (s == "...")` を並べる
* 多ければ IL の switch 命令を出力

みたいな処理をしていました。

ちなみに、IL の switch 命令は、

1. 各 `case` 文字列に対応するハッシュ値を事前計算
2. `s.GetHashCode()` で `switch` する

みたいなコードを生成するそうです。

`if (s == "...")` を並べる方式はワーストケースでは多数の `==` 比較がかかりますし、
文字列に対する `GetHashCode` の計算は意外と重たい処理です。

## <a id="length-based-switch">新手法 switch</a>

Visual Studio 17.6 (Roslyn 4.6) 以降では、`case` の数が中程度のとき、
以下のような分岐をかけるようになりました
([Trie木](https://ja.wikipedia.org/wiki/%E3%83%88%E3%83%A9%E3%82%A4_(%E3%83%87%E3%83%BC%E3%82%BF%E6%A7%8B%E9%80%A0))的発想の簡易アルゴリズム)。

1. 文字列長でまず分岐
2. どこか1文字だけを使って `char` で `switch`
3. その後に `string` の `==` 判定

Length-based switch dispatch (文字列長ベースの switch 分配)というそうです。

先ほどの例の `switch` だと、おおむね以下のような感じの分岐に置き換わります。
(実際はもうちょっと goto だらけのコードになりますが、
見やすさ優先で変更。)

```csharp {title="length-based switch の例"}
StringSwitch("");

static int StringSwitch(string s) => s.Length switch
{
    3 => s[0] switch
    {
        'a' => s == "abc" ? 0 : -1,
        'd' => s == "def" ? 1 : -1,
        'g' => s == "ghi" ? 2 : -1,
        _ => -1,
    },
    6 => s[5] switch
    {
        'a' => s == "01234a" ? 3 : -1,
        'b' => s == "01234b" ? 4 : -1,
        'c' => s == "01234c" ? 5 : -1,
        _ => -1,
    },
    8 => s == "aaaaaaaa" ? 6 : -1,
    _ => -1,
};
```

これで、どの `case` にも当たらないときには「長さ比較 + 1文字比較」で終わり、
当たった時でもそれに加えて少数の文字列 `==` になります。

例えば .NET の中の人が [HTTP の content type 用の分岐で測った](https://github.com/dotnet/roslyn/issues/56374)感じだと、
5～10倍くらい速いみたいです。
