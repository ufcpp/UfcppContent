---
title: "C# 13 でのコレクション式 - ディクショナリ式"
source_url: "https://ufcpp.net/blog/2024/3/dictionary-expressions/"
content_type: "BlogEntry"
published_at: "2024-03-16T18:17:03"
updated_at: "2024-03-16T18:17:03"
tags: []
umbraco_id: 2494
parent_id: 2490
sort_order: 3
aliases: []
---

# C# 13 でのコレクション式 - ディクショナリ式

C# 13でのコレクション式関連、量が多いのでちょっとずつ取り上げシリーズ。

* [[Proposal]: Collection Expressions Next (C#13 and beyond)](https://github.com/dotnet/csharplang/issues/7913)

今日はディクショナリ式の話を。

* ディクショナリ式 ← 今日はこれ
* 自然な型
* インラインなコレクション式
* コレクションに対する拡張メソッド
* 現状でコレクション式に対応してない型
* 非ジェネリックなコレクションのサポート
* [制限の緩和](../colexpr13-relax-restriction/index.md)

## ディクショナリ式

C# 12 でコレクション式が入りましたが、`Dictionary<TKey, TValue>` などのディクショナリ系の型に対しては使えませんでした。

```csharp
// C# 12 でも空っぽのディクショナリは作れるのに…
Dictionary<string, int> d = [];

// 要素があるものは書く手段がない(以下はいずれもエラー)。
// スケジュールの都合で意図的に「C# 13 でやる」計画。

// KeyValuePair とかタプルも受け付けず。
Dictionary<string, int> d1 = [KeyValuePair.Create("", 1)];
Dictionary<string, int> d2 = [("", 1)];

// コレクション初期化子/オブジェクト初期化子みたいな構文も受け付けず。
Dictionary<string, int> d3 = [{"", 1}];
Dictionary<string, int> d4 = [[""] = 1];
```

C# 12 時点で[コレクション式](../../../../study/csharp/datatype/collection-expression.md)に対する背景と同じく、
ディクショナリについても以下の需要が見込まれます。

* 簡素に書きたい
* いろいろな種類のディクショナリ系の型に対して共通で使える構文にしたい
* 特に、既存のコレクション初期化子では使えない immutable な型にも対応したい

まあ、
GitHub を軽くクロールしてみて利用頻度を調べると、
リストや配列と比べたらディクショナリの利用率は10%くらいらしいです。
とはいえ、10%もそこそこ大きな数字。
C# 12 時点では後回しになりましたが、13候補としては有力です。

提案ドキュメント、関連デザインミーティング等:

* [[Proposal]: Dictionary expressions #7822](https://github.com/dotnet/csharplang/issues/7822)
* [Dictionary Expressions](https://github.com/dotnet/csharplang/blob/main/proposals/dictionary-expressions.md)
* [C# Language Design Meeting for March 11th, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-03-11.md)

まあ検討が始まったばかりなので、まだまだ結論の出ていない検討事項も多数。
とりあえず今日は3月11日のミーティング議事録をベースにした話を書こうかと思います。

## 構文の候補

まだ構文をどうするかも決定ではないんですが、現状の最有力候補は `[key: value]` みたいな書き方です。

```csharp
// 「ディクショナリ式」の最有力候補の文法:
Dictionary<string, int> d = [
    "one": 1,
    "two": 2,
    ];
```

もちろん、「JavaScript では `{}` を使うけども」みたいな別案もあるんですが、
まあ、C# 12 のコレクション式に合わせて `[]` になると思われます。

ちなみに、最初期には「`[]` の外でも `key: value` で `KeyValuePair` を作れるようにするべきか？」みたいな見当もありましたが、
現状それには否定的で、 `[]` の中限定の構文になりそうです。

```csharp
// 没案「KeyValuePair 式」。
KeyValuePair<string, int> kvp = "one": 1;
```

## 検討事項1: KeyValuePair を並べる

ディクショナリ式中では、`key: value` みたいな形式のみを受け付けるか、それとも、`KeyValuePair` であれば直接書けるようにするかという話があります。

```csharp
// key: value のみ。これは問題ない。
Dictionary<string, int> d = ["one": 1];

var kvp = KeyValuePair.Create("two", 2);

// KeyValuePair をいちいち展開する必要はあるかどうか。
Dictionary<string, int> d1 = ["one": 1, kvp.Key: kvp.Value];

// こう書きたい需要はある。
Dictionary<string, int> d2 = ["one": 1, kvp];
```

`["one": 1, kvp]` と書けるようにする案には肯定的な人が多く、承認されそうです。

## 検討事項2: KeyValuePair のリストを Spread する

検討事項1と似たような話ですが、`IEnumerable<KeyValuePair<TKey, TValue>>` とかをディクショナリ式中に含められるかという話もあります。

```csharp
var kvps = new[] { KeyValuePair.Create("two", 2) };

// .. で展開すると KeyValuePair になるわけで、
// KeyValuePair を認めるのなら、 ..(KeyValuePair のリスト) も認めたい。
Dictionary<string, int> d1 = [..kvps];

// 混在も需要あり。
Dictionary<string, int> d2 = ["one": 1, ..>kvps];

// 特に、「複数のディクショナリのマージ」みたいな用途で以下のように書きたい。
var kvps1 = new[] { KeyValuePair.Create("three", 3) };
Dictionary<string, int> d3 = [..kvps, ..kvps1];
```

これも認める方向で検討されています。

## 検討事項3: ディクショナリじゃなくて KeyValuePair のリスト

`[]` 中の `key: value` は「`KeyValuePair` を作るための簡易記法」みたいなものになっているわけですが、
だったら以下のような「ディクショナリじゃないただのコレクションに対して使えるか」という話が出てきます。

```csharp
// 「ディクショナリ式」の最有力候補の文法:
List<KeyValuePair<string, int>> d = [
    "one": 1,
    "two": 2,
    ];
```

これも需要がそれなりにありそうです。
.NET の BCL とか、 Roslyn 中のコードでもオプションとかをディクショナリではなくて `IEnumerable<KeyValuePair<TKey, TValue>>` 引数で受け取っているものがそれなりにあるそうで。

それに、先ほどの `Dictionary<string, int> d3 = [..kvps, ..kvps1];` みたいなもので、マージ元になる `kvps` などはディクショナリではなくて `KeyValuePair` のリストということは十分ありそうな話です。

ということで、これも承認されそうです。

## 検討事項4: KeyValuePair 以外の要素は認められるか

`Dictionary<TValue, TKey>` とかでは要素の列挙などに `KeyValuePair<TValue, TKey>` を使うことが多いですが、
ディクショナリ式を作るにあたって `KeyValuePair` だけに絞るか、それとも他の型も使えるようにするかという問題もあります。

例えば、タプル導入時にも、`(TKey key, TValue value)` はほぼ `KeyValuePair<TKey, TValue>` と同等」みたいなことを言われています。
割かし最近 BCL に追加された [`PriorityQueue`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.generic.priorityqueue-2) なんかは、`(TElement Element, TPriority Priority)` で要素とその優先度の列挙をします。
[`Zip`](https://learn.microsoft.com/ja-jp/dotnet/api/system.linq.enumerable.zip?view=net-8.0#system-linq-enumerable-zip-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1)))) なんかも `(TFirst First, TSecond Second)` で結果を列挙します。
こういうものを直接 `[]` の中で `..` で展開したかったりはします。

あとは、`KeyValuePair` を特別扱いするとしても、暗黙の型変換を認めるかどうか。

```csharp
struct Pair<X, Y>(X x, Y y)
{
    public static implicit operator KeyValuePair<X, Y>(Pair<X, Y> pair) => ...;
}

Dictionary<string, int> d = 
[
    new Pair("one", 1),
    .. new[] { new Pair("two", 2) }
];
```

これについては結論はまだ出ていないみたいです。

## 検討事項5: Add か、インデクサーか

まず、ディクショナリ式ではキーの重複を認めるかどうかという話があります。
例えば、`ToDictionary` なんかでは、キーが重複していると例外を出します。

```csharp
var d = new[] { (1, 10), (1, 20) }
    .ToDictionary(x => x.Item1); // ArgumentException
```

が、まあ、前述の2個のディクショナリをマージするようなケースでは重複を認める方がよかったりします。
オプション指定とかだと「デフォルト設定と、ユーザーごとの設定をマージ、後で追加した方を優先」みたいな使い方を結構しますし。

ただ、「重複を認めるかどうか」という観点だと、結局は「ターゲットにする型によって挙動が違う」ということになります。
例えば、以下のような感じ。

* `Dictionary<TKey, TValue>` の `Add` は重複を認めていない
* `ImmutableDictionary<TKey, TValue>` の `Add` は上書き(上書きした新しいインスタンスを作って返す)
* `FrozenDictionary<TKey, TValue>` の `Add` (`ICollection` インターフェイス越しに呼べちゃう) は `NotSupported` 例外を出す

なので結局は「どういう動作にするか」は決めれなくて、「`Add` とインデクサーのどちらを使うか」という話になります。

```csharp
// Add で初期化。
Dictionary<string, int> d1 = new();
d1.Add("a", 1);
d1.Add("b", 2);

// インデクサで初期化。
Dictionary<string, int> d2 = new();
d2["a"] = 1;
d2["b"] = 2;
```

ちなみにこれらは、現状のコレクション初期化子・オブジェクト初期化子を使うと以下のように書けるやつです。

```csharp
// Add での初期化になるコレクション初期化子。
Dictionary<string, int> d1 = new()
{
    { "a", 1 },
    { "b", 2 }
};

// インデクサでの初期化になるオブジェクト初期化子。
Dictionary<string, int> d2 = new()
{
    ["a"] = 1,
    ["b"] = 2
};
```

`["a": 1, "b": 2]` はどちらになるかという話なわけですが、
現状はインデクサー案が有力みたいです。
コレクション初期化子(`Add` になる)と食い違うという懸念もありますが、
インデクサーの方が都合がよさどうという判断になっています。
例えば先ほど例に挙げた `[..defaultSettings, ..userSettings]` みたいなケースで重複を認めている方がよさそうで、
`Dictionary<TKey, TValue>` の場合は「`Add` は重複不可、インデクサーは可」ですし。
