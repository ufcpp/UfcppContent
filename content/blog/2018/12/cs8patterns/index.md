---
title: "C# 8.0 パターン マッチング"
source_url: "https://ufcpp.net/blog/2018/12/cs8patterns/"
content_type: "BlogEntry"
published_at: "2018-12-10T10:00:09"
updated_at: "2018-12-10T10:05:09"
tags: []
umbraco_id: 2191
parent_id: 2177
sort_order: 9
aliases: []
---

# C# 8.0 パターン マッチング

今日はパターン マッチングの話。
昨日の[`switch`式](../cs8switchexpr/index.md)に引き続き、
真っ先に実装されてそうなものなのに Preview 1 には入っていなかったやつ。
というか、`switch`式自体、このパターン マッチングの一部として提案されているものです。

## パターン マッチング “完全版”

パターン マッチングは、元々は C# 7.0 で検討されていたものの、
結局、一部分だけが C# 7.0 に入り、複雑なものは C# 8.0 に回りました。

| パターン | C# のバージョン | 概要 | 例 |
| --- | --- | --- | ------------- |
| discard | C# 7.0 | 何にでもマッチ・無視 | `_` |
| var | C# 7.0 | 何にでもマッチ・引数で受け取り | `var x` |
| 定数パターン | C# 7.0 | 定数との比較 | `null`、`1` |
| 型パターン | C# 7.0 | 型の判定 | `int i`、`string s` |
| 位置パターン | C# 8.0 | [分解](../../../../study/csharp/datatype/deconstruction.md)と同じ要領で、`Deconstruct`を元に(引数の位置に応じて)再帰的にマッチングする | `(1, var i, _)` |
| プロパティ パターン | C# 8.0 | プロパティに対して再帰的にマッチングする | `{ A: 1, B: var i }` |

要するに、再帰的に使える下の2つが C# 8.0 での新機能になります。

まあ、C# 7.0 のやつだと「“パターン”って言うほど複雑なマッチングしてない」感がありました。
(実際、なので C# 7.0 リリース当時は「型スイッチ」みたいな呼び方もされていました。
結局、まあ、C# 8.0 を見越してあくまで「パターン マッチングのうち、型パターンだけは先にリリース」みたいな感じでアナウンスされています。)

例えば以下のような感じ。

```csharp
public class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) => (X, Y) = (x, y);
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
 
public class C
{
    static int M(object obj)
        => obj switch
        {
            0 => 1,
            int i => 2,
            Point(1, _) => 4, // new!
            Point { X: 2, Y: var y } => y, // new!
            _ => 0
        };
}
```

## 何に使うかと言われると

まあ、再帰パターンなわけで、再帰的なデータ構造相手なら便利そうではあります。
(再起データ構造自体どのくらいの頻度で使われるかという話を置いておけば…)

例えば、`x + 1` みたいな式を `Add(Variable(x), Const(1))` みたいなツリー構造で表す奴とか。
そのツリーに対して、`x + 0` は `x` と等しいとか、`x * 1` は `x` と等しいとかその手の簡単化をするやつは、以下のように書けるようになります。

```csharp
public static Node Simplify(this Node n)
    => n switch
        {
            Add(var l, var r) => (l.Simplify(), r.Simplify()) switch
            {
                (Const(0), var r1) => r1,
                (var l1, Const(0)) => l1,
                (var l1, var r1) => new Add(l1, r1)
            },
            Mul(var l, var r) => (l.Simplify(), r.Simplify()) switch
            {
                (Const(0) c, _) => c,
                (_, Const(0) c) => c,
                (Const(1), var r1) => r1,
                (var l1, Const(1)) => l1,
                (var l1, var r1) => new Mul(l1, r1)
            },
            _ => n
        };
```

([コード全体はGist上に](https://gist.github.com/ufcpp/37702d99a7c0148b3b0d0f8b82e46414))

ちなみに、単に「複数の値を同時にマッチング」という使い方もできます。
以下のように、`(x, y) switch { }` でスイッチ。

```csharp
static int Compare(int? x, int? y)
    => (x, y) switch
    {
        (null, null) => 0,
        (null, _) => -1,
        (_, null) => 1,
        ({} ix, {} iy) => ix.CompareTo(iy)
    };
```

要するに、このコードは「タプルに対する位置パターン」なんですが、
それが「`x`, `y` に対して多値マッチング」っぽく使えます。

(あと、`{}`は後述しますが、「プロパティ パターン」(の、中身空っぽ)です。)

ちなみに、`switch` ステートメントでも以下のような書き方ができます。

```csharp
static int Compare(int? x, int? y)
{
    switch (x, y)
    {
        case (null, null): return 0;
        case (null, _): return -1;
        case (_, null): return 1;
        case ({ } ix, { } iy): return ix.CompareTo(iy);
        }
    }
}
```

先ほど書いた通り、これは実際には「タプルに対する位置パターン」なんですが、
だったら、本来は `switch ((x, y))` という書き方(内側の`()`がタプル構築、外側の`()`が`switch`ステートメントのもの)をする必要があります。
これも C# 8.0 の新機能で、「タプルだったら`()`を1個省略して、多値 `switch` っぽく書けるようにした」というものです。

## 非 null マッチング

ちなみに、プロパティ パターンの `{}` は、
プロパティを調べる前に本体が null ではないことをチェックします。
中身が空っぽのプロパティ パターンでも null チェックだけは挿入されるので、
`x is {}`で、「`x`はnullではない」の意味で使えます。

C# 7.0 までのパターンだと、null チェックを楽に書く手段がなかったです。

```csharp
struct LongLongNamedStruct { }
 
void M1(LongLongNamedStruct? x)
{
    // こういう書き方だと null チェックになる。
    if (x is LongLongNamedStruct nonNull)
    {
        // obj が null じゃない時だけここが実行される。
        // でも、x の型が既知なのに、長いクラス名をわざわざ書くのはしんどい…
    }
}
 
void M2(LongLongNamedStruct? x)
{
    // が、var パターンは null にもマッチしちゃう。
    // (var は「何にでもマッチ」。null でも true になっちゃう。)
    if (x is var nullable)
    {
        // obj が null でもここが実行される。
    }
}
```

もちろん、単に null チェックだけなら `!(x is null)` とか `x.HasValue` でいいんですけども、
値を使いたければその後ろで `var nonNull = x.GetValueOrDefault();` を書かないと行けないのがしんどく。

そこで、プロパティ パターンが使えます。
以下のように、「空のプロパティ パターン」を書けば、「非 null のときだけ」判定ができます。

```csharp
void M3(LongLongNamedStruct? x)
{
    // (C# 8.0) プロパティ パターンであれば、null チェックを含む。
    if (x is {} nonNull)
    {
        // obj が null じゃない時だけここが実行される。
    }
}
```

ちょっと「知ってないと使えない仕様」ですけども…
覚えておくと便利です。

## 対称性

C# 7.0 の時、タプルとか分解の構文を決めるにあたって、C# チームは結構「対称性」を気にしていました。

まず、タプルは「引数と対になるもの」として考えられています。

```csharp
// タプル型宣言と引数宣言は同じような見た目。
(int x, int y) tup0;
int method(int x, int y) => x + y;
 
// タプル構築はメソッド呼び出しみたいな書き方になる。
// 位置指定:
var tup1 = (1, 2);
var ret1 = method(1, 2);
 
// 名前指定:
var tup2 = (x: 1, y: 2);
var ret2 = method(x: 1, y: 2);
 
// タプル戻り値は、引数と同じような書き方に。
(int x, int y) swap(int x, int y) => (y, x);
```

また、分解は「コンストラクターと対になるもの」です。

```csharp
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
 
    // 複数の値を組み合わせて1つの型にまとめるのが構築(construct)。
    public Point(int x = 0, int y = 0) => (X, Y) = (x, y);
 
    // 1つにまとまっている値をバラバラに戻すのが分解(deconstruct)。
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
```

```csharp
var p = new Point(1, 2); // contruct
var (x, y) = p;          // deconstruct
```

C# 8.0 の再帰パターンもこの話の延長にあります。

```csharp
// 位置指定で構築できるんなら、位置指定でマッチングできるべき
var p1 = new Point(1, 2);
var r1 = p1 is (1, 2);
 
// 名前指定で構築できるんなら、名前指定でマッチングできるべき
var p2 = new Point(x: 1, y: 2);
var r2 = p2 is (x: 1, y: 2);
 
// 初期化子でプロパティ指定できるんなら、プロパティ指定でマッチングできるべき
var p3 = new Point { X = 1, Y = 2 };
var r3 = p3 is { X: 1, Y: 2 };
 
// 混在構築できるんなら、混在マッチングできるべき
var p4 = new Point(x: 1) { Y = 2 };
var r4 = p4 is (1, _) { Y: 2 };
```

## 最近の変更

冒頭で言ったように、再帰パターンも元々は C# 7.0 で考えられていました。
それに、C# 8.0 機能の中では相当早い段階から実装済みで、
確か今年の初めくらいにはすでに実装がありました。

(なので、[sharplab.io](https://sharplab.io/#v2:EYLgZgpghgLgrgJwgZwLRIMaOQSwG4SoAOsMECAdsgD4ACADAAS0CMA3ALABQ3tAzMxYA2ZgCZGAYW4Bvbo3nMBrETgoxJUADZZNsCAAoYACxzJGAOQD2AEwiMKAGkar1ADwCUchd4C8APntGZAB3HBgMIy9veVkuaPjGADUoBEY8Rn9GVwcohPkJSyp1DAyAjAA6ZM04CBy4vIUAQWtrfTwUxk0ndtSEd1LO8oktHT19D0YAakYEIZG4XTJx9zqGhQBZBbaOrrSOvoHNOe0FsYmAKhnj0aWPVbXGAH0B4wRLYPsID4BJCnbNHDWADyRHIsBwhQAoq4MBAiDAIRR9J56vEAL6cHioxSCERWWyMADKOAAtkQAWAAJ6GExmfF2CgovKZChBULhSLY7yxB6MZqtHqdbr7fqZfRHYlkinUlZXSXknBU5H9EJhCK5PI83kKfQFIr6eiywUIFiigIm+7axjbVKaFhOPXIGAG9xmzr2jUPG3u4W9U0DChfPktcX2mamz3oy0NTaab27Y1u8XleXS5FOWapxUylXs9VchJaq26wpOl2MDBOR5uyuRhr6R4O0vOw0VmvR3kl/Wm33ht0Wut5eNhx3O01uu0dr2Cyd7P1uwMfWOhjMRgtRwcKZ4susY7i5ZTMAAsjHWUFUyNyReigtcGQsNgg5QAGpi8qwAJzjRiXO/TU1vgkn76CBLA/lkUyMEwv79JcpopqSCpKq6gHxMBIF/owoiwdamF8K61ynEs46oYwaLcORWL8IwUDAE6CBQBg6i0OI9IyAeSjCDM0DWIUmiUg+BLPvei5JCkyKkdRh6IQCGBhIwliggxMCWKk9L6C4ezVBAC5BqO2zae4kmcXij4KUpsCqZM+j0p0EBgDATi2QgOAAOZGDAukfPy4r2Y5MxuR5Rkcbigl2IpYIqQg5w2WZmh+U5Zkue5nkBkGy7xQ5GaBZ5mKUbwSjiMkqQgGFjDSDilgEAgLkEqwTAACqWISMAuRQrnIgMABErhdWwZH7lwUniKOjClWxXDXtRmlVDU5WMK5EAwP1lHeNR+maf8NT9BVs12D4Wk1CtIW0CeAAiEAYM2CBwEx+iWHA6ibVo20DFt+1idpxkKdVtV2PVjBNS1bUdW6e3lEDrWqKDeWDcNwbWGNYXsdi1G2QAMn582LctA2owItkAEo5djS3HfjCOxQSmX+c5OVJpjWWMMTKWitaNPZaz32nYwF1XUUN13Q96i2RzCmPWVyVBQMvlM1LqUHfojP+SzQXfVV5B/YIjXNVD7WdZkAAkXX6NIysQ7rIPImikHSKrMAW8D0PW+4fUUXDhWngsSMTVNBNmcrpO46tCho2Z9tB+Ta0CMuosJZL9My8rTj22zsv+fLwUUzzfPXbdzrC2VYuF3TrMy2Lmf3kr8ep+rv2Av9LA607+tusbZt+Y7eugzblx2zlXdW+4aJu1waJAA==)で割かし安定して試せたりします。
Visual Studio 2019 Preview 1 で実装されていなくても割と細かくブログを書けるのはこれのおかげ。)

個人的には「C# 7.4 があってもよかったんじゃ… 再帰パターンだけのリリース」とかもちょっと思ったり。
C# チーム的には「マイナー リリースで出すほど小さい機能ではない」とのことで、C# 8.0 での追加になります。

ということで、大半の機能はだいぶ前から試せる状態にあったんですが、
割と最近にもいくつか細かい追加・変更がありました。

- `switch (x, y)` の「`()` を1段省略」は割と最近の採用
- プロパティ パターンの構文は `{ X is pattern }` か `{ X = pattern }` か `{ X: pattern }` のどれがいいか
  - `:` になったのは割と最近
- `var (x)` みたいな、「1引数 `Deconstruct`」
  - キャストや、「`(1)`は単なる`1`と同じ意味」という既存の構文との弁別の問題があるものの、`var`の後ろなら弁別できるので認めようということに最近なった
- 同じく、「0引数`Desonctruct`」に対する`var ()`パターンも
