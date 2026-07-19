---
title: "タプル要素名の大文字・小文字"
source_url: "https://ufcpp.net/blog/2018/12/tupleelementcase/"
content_type: "BlogEntry"
published_at: "2018-12-02T15:55:43"
updated_at: "2018-12-02T15:58:33"
tags: []
umbraco_id: 2179
parent_id: 2177
sort_order: 1
aliases: []
---

# タプル要素名の大文字・小文字

タプルの要素名は`(int x, int y)` みたいに camelCase (先頭小文字) で書くべきか、
`(int X, int Y)` みたいに PascalCase (先頭大文字) で書くべきか、
割かし最近、この問題が再燃してたりしました。

## 背景1: C# のコーディング規約

大体のプログラミングでは、別に大文字・小文字に意味があるわけではなく、
`x` と書こうが `X` と書こうが原理的には自由です。
しかし、実践的には、「その言語の標準ライブラリ辺りに合わせる」というのが一般的かと思います。
現在の C# であれば、「[Naming Guidelines](https://docs.microsoft.com/ja-jp/dotnet/standard/design-guidelines/naming-guidelines)」辺りに合わせるのが無難でしょう。

(ちなみに、上記の規約は public な部分にしか言及していません。
割かし「private なところは自由にしたらいいんじゃないか」な文化です。
[corefx](https://github.com/dotnet/corefx)なんかは独自に[C# Coding Style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md)を持っていたりしますが、
これはあくまで corefx に pull request を出すときに気を付けるべき規約であって、
全ての C# 利用者に対して強制するものではありません。
)

今日の主題は casing/capitalization (変数などの先頭を大文字にするか小文字にするか)になります。
今日の話に関係するのは以下の2つ。

- メンバー名は大体 PascalCase
  - フィールドであっても、public なものは PascalCase
- 引数名・ローカル変数名は camelCase

```csharp
public class Class
{
    // フィールドも、public なものは大文字始まり
    public int PublicField;

    // private なものについては割かし自由
    // ほとんどの人は小文字始まり。
    // 先頭に _ を付けるかどうかは好みが分かれるものの、 _ を付ける人の方がちょっと多い印象。
    private int _privateField;

    // 引数は小文字始まり
    public void M(int parameter)
    {
        // ローカル変数は小文字始まり
        int localVariable = parameter;
    }
}
```

## 背景2: タプル戻り値

やりたかったことは、要するに LINQ の `Zip` メソッドにオーバーロードを足したいというもの。
今あるオーバーロードだと、以下のような書き方をよくやると思います。

```csharp
var x = new[] { 1, 2, 3 };
var y = new[] { "one", "two", "three" };
var zip = x.Zip(y, (i, s) => (i, s));
```

今の `Zip` は最後の引数で必ずデリゲートを1個渡す必要がありますが、
大抵の場合はこの例のように「単にタプルで返したい」で十分です。
そこで、以下のようなオーバーロードを足す流れになりました。

```csharp
static class Enumearble
{
    public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(
        this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        => first.Zip(second, (x, y) => (x, y));
        // 実際には効率化のためにもうちょっと複雑な実装。返す値自体はこれと同じ。
}
```

## 問題: タプル戻り値の casing

先ほどの `Zip` の新オーバーロードですが、戻り値が
`IEnumerable<(TFirst First, TSecond Second)>`です。
問題となるのは `First`、`Second` の部分。

タプルの要素名の casing をどうするべきかは結構意見が割れます。
なぜかというと、

- タプルは構造体の一種なんで、各要素は public なフィールドである
  - だとすると、一般的な規約では PascalCase (先頭大文字)
- タプルは「変数をペアリングしたもの」あるいは「引数リストの一般化」である
  - だとすると、camelCase (先頭小文字)

前者の立場(構造体のフィールド派)は以下のような感じ。

```csharp
// 立場1: 構造体のフィールド派
static void A()
{
    // 以下の2行の差は「匿名か、名前付きか」だけ。
    var p = new Point(1, 2);
    var q = (1, 2);

    // タプルは「名前がない型」なだけで、各要素はフィールドみたいなもの。

    // 大体、上記のように要素名を省略した場合、Item1, Item2 と、PacalCase な名前で値を参照することになる。
    System.Console.WriteLine(q.Item1);

    // 以下のように、PascalCase にしておいた方が「名前付き」の場合とそろってていい。
    // このタプル(無名の型)から、自動リファクタリングで Point 型みたいなものを生成することもあり得えて、その場合 PascalCase の方が自然
    var r = (X: 1, Y: 2);
    var x1 = p.X;
    var x2 = r.X;
}
```

後者の立場(引数の一般化派)は以下の通り。

```csharp
// 立場2: 引数の一般化派
static void B()
{
    var p = new Point(1, 2);
    var q = (1, 2);

    // タプルは「引数リストだけがむき出しになっている」という状態。

    // 「名前付き引数」を使うと以下のような書き方になるわけで、それに合わせる方が自然。
    var r = new Point(x: 1, y: 2);
    var s = (x: 1, y: 2);

    // s.x という書き方も、「変数 x, y が s によってペアリングされてる」程度の認識。
    var sum = s.x + s.y;
}

// 引数リストとタプル戻り値がほぼ同じ書き方。
// タプルは引数の対となるもの。
static (int x, int y) Swap(int x, int y) => (y, x);
```

## どちらが有力か

C# チームは「引数の一般化派」です。
なので、タプル(C# 7.0)がリリースされてからの2年ほど、
大抵の場所で camelCase が使われてきました。

それに対して、今回、2年越しで corefx 方面に「構造体のフィールド派」が多かったみたいです。
その結果、前述の`Zip`の戻り値は PascalCase で作られて、
一度それの pull request はマージされました。

ここで、両者の齟齬が発覚。
もめ始めて、

- いったん `Zip` のやつは revert
- camelCase か PascalCase かちゃんと決めて、規約に書いとこうぜ
- 決めかねる。というか、タプルを public なところに使うこと自体よくない
  - `Zip` みたいに「2要素で確定」みたいなものならともかく、通常は、要素(フィールド)を増やすだけで破壊的変更になってしまうようなものは public には使いにくい
- 規約に書くなら「タプルは public なところに使うものじゃない」がいい

みたいな流れに。

結局、前述の `Zip` 新オーバーロードは、以下のような構造体を返す作りにおそらく変更されることになりそうです。

```csharp
public readonly struct ZipResult<TFirst, TSecond>
{
    public ZipResult(TFirst first, TSecond second) => (First, Second) = (first, second);
    public TFirst First { get; }
    public TSecond Second { get; }
    public void Deconstruct(out TFirst first, out TSecond second) => (first, second) = (First, Second);
}
```

規約に関しては、タプル要素名の casing についてはやっぱり両論あって決まらなさそう。
その代わり、corefx 内では「DO NOT: タプルは public API で使うな」規約を足しそうな雰囲気になっています。
(これももちろん、あくまで corefx 内の話です。corefx は「破壊的変更を起こしそうなもの」を特に強く避ける文化なのでかなり保守的。)
