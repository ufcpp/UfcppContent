---
title: "【C# 11 で再検討】Countable"
source_url: "https://ufcpp.net/blog/2022/1/countable/"
content_type: "BlogEntry"
published_at: "2022-01-04T20:33:50"
updated_at: "2022-01-04T20:37:20"
tags: []
umbraco_id: 2402
parent_id: 2401
sort_order: 0
aliases: []
---

# 【C# 11 で再検討】Countable

[リスト パターン](../../../2021/12/list-pattern/index.md)の実装で、「Count もしくは Length を持った型なら何にでも使える」と説明しました。
C# ではこれを「Countable」と呼んでいます。

この Countable というコンセプト、最初に出て来たのは C# 8.0 の [`Index`](../../../../study/csharp/data/dataranges.md) のときです。
で、リスト パターンや[コレクション リテラル](../../../2021/12/collection-literal/index.md)でも Countable が出て来たところで、
`Index` に対する Countable の扱いも拡張しよう見たいな話がちょこっと出ています。

ちょっと課題もあるので C# 11 時点はおろか、その先でもどうなるかわからないですがとりあえず提案が出ました。

* [[Proposal]: Adding Index support to existing library types (redux) #5596](https://github.com/dotnet/csharplang/issues/5596)

## Countable

`Index` 構文では、`^i` みたいな書き方で「末尾要素から i 個前」みたいなインデックスを表します
(一方、「先頭から」の方は普通に `int` を使います)。
「末尾から」を取るためにはコレクションの要素数を必要としますが、
これは[パターン ベースな構文](../../../../study/csharp/misc/miscpatternbased.md)になっていて、以下の条件を満たす型なら何でも使えます。

* `Index` 型を受け取るインデクサーがある場合は `x[^i]` の `^i` は `Index.FromEnd(i)` として解釈される
* `int` 型を受け取るインデクサーがあって、
    * `int` 型の `Length` プロパティを持っている場合、`x[^i]` を `x[x.Length - i]` に展開する
    * それ以外で、`int` 型の `Count` プロパティを持っている場合、`x[^i]` を `x[x.Count - i]` に展開する

この条件を満たす型を Countable と言います。
リスト パターンも Countable な型に対して使えます。

## Index サポートの拡張

C# 8.0 時点ではインデクサーだけに対応しました。
前述の通り「`x[^i]` を `x[x.Length - i]` に展開する」みたいな処理が掛かります。

一方、`RemoveAt` メソッドとかでも同様に `RemoveAt(^i)` を使いたいという話があります。
C# の文法的に対応しなくても、ライブラリ側で `RemoveAt(Index)` なオーバーロードを足せば今でも `RemoveAt(^i)` と書けます。
ただ、全てのコレクション型に対してオーバーロードを足して回るというのも大変なので、改めて「Countable のサポート範囲の拡張」の話が出てきました。

### 問題1: Add(int) が誤判定しそう

パターン ベースな構文は無節操にやると結構「意図せずパターンを満たしてしまった」と言うことが起こります。
今回の場合も、以下のような書き方ができてしまうという懸念あり。

```csharp
List<int> list = new();
list.Add(^1);
```

これが `list.Add(list.Count - 1)` と解釈されかねないわけで、それが望まれる挙動にはならないでしょう。

一応、この問題は「`Add` などの引数で、引数名が `index` の時にだけこの構文を使える」みたいな制限を掛けることである程度の対処はできそうです。

### 問題2: Range にも似た処理が欲しい？

`Index` の `^i` のサポート範囲を拡張するんなら、
`Range` の `i..j` も拡張したいという話も自然と出てきます。
`list.RemoveRange(1..^1)` みたいなので一定範囲の要素をまとめて削除みたいな感じで。

とはいえ、こちらは今現在デファクトになっているパターンがないですし、
どうせオーバーロードを今から足すのであれば `RemoveRange(Range)` なオーバーロードを足せば、C# の文法の拡張なしでライブラリだけで対応できます。

また、`RemoveRange(1..^1)` の部分を `RemoveRange(1, list.Count - 2)` みたいな感じで、引数の数が増えるような展開が必要になるのでちょっと分かりにくい仕様になります。


## 問題3: オーバーロード解決

新機能を足すたびに問題になるオーバーロード解決なんですが…

`RemoveAt(^i)` みたいなやつでも、下手な実装をすると破壊的変更になりかねません。
例えば今現在、インスタンス メソッドとして `RemoveAt(int)` があって、拡張メソッドとして `RemoveAt(Index)` があったとして、
新構文で `RemoveAt(^i)` を `RemoveAt(Count - i)` に展開するようになると、`RemoveAt(int)` と `RemoveAt(Index)` のどちらを呼ぶかが変わってしまうことがあります。

## 今でも起こりうるパターン ベース問題

`Vector<T>` 型(`System.Numerics` 名前空間)はインデクサーを持っているわけですが、以下のような[リスト パターン](../../../2021/12/list-pattern/index.md)を書きたいかどうかという話もあります。

```csharp
using System.Numerics;

Vector<byte> v = new();

Console.WriteLine(v is [ 0, .., 0 ]);
```

これを書けるようにするには `Vector<T>` を Countable にすればいいんですが…
`Vector<T>` の文脈的に「`Length` と言われても要素数には見えない」という別問題が発生します。
「ベクトルの Length」というと多くの場合はユークリッド距離を指していて、要素数ではありません。

ということで、なんらかの opt-out 手段、すなわち、「Countable になりそうだけど Countable 扱いしてほしくない」というのを表明する手段も必要になるんじゃない？という提案も出ていたりします。

* [Provide a way to opt-out of structural typing. #5278](https://github.com/dotnet/csharplang/discussions/5278)

こないだ [`ImmutableArray` の話](../../../2021/12/immutable-array-init/index.md)で書いたみたいに、
望まれない形でたまたまパターンを満たしてしまった場合、
結構悲惨なことになりますんで…
