---
title: "ピックアップRoslyn 3/21: Design Notes一斉アップロード祭り"
source_url: "https://ufcpp.net/blog/2018/3/pickuproslyn0321/"
content_type: "BlogEntry"
published_at: "2018-03-22T00:13:56"
updated_at: "2018-03-22T00:51:10"
tags: []
umbraco_id: 2139
parent_id: 2134
sort_order: 3
aliases: []
---

# ピックアップRoslyn 3/21: Design Notes一斉アップロード祭り

昨日なんですけども、2018年に入ってからのC# Language Design Meetingの議事録(design notes)が一斉にアップロードされました。

- [C# Language Design Notes for 2018](https://github.com/dotnet/csharplang/tree/master/meetings/2018)

読むの大変だった… 春分の日でよかった…

一通りなんとなくは目を通したんですけど、ブログ1回の内容じゃなさすぎるので、少しずつネタにしていこうかと。

## ここ数時の状況

2週間前に[Visual Studio 15.6が正式リリース](../vs15_6/index.md)されて、
その後ほどなくして15.7のプレビュー1もリリースされたわけですけども。

このプレビュー1の時点では 15.7 に C# 7.3 は入っていなかったわけですけども、
[roslynリポジトリの15.7マイルストーン](https://github.com/dotnet/roslyn/milestone/32)を見るとだいぶC# 7.3がらみの作業がマージされている状況です。
作業進捗を表す[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)のページもつい5日前に更新されて、C# 7.3のところの大半の機能が Merged になりました。

要するに、15.7向けのC# 7.3の最低限の作業が完了したんでしょうね。
あとは正式リリースに向けてバグ出し・バグ修正するフェーズに。
おそらく近々15.7プレビューにC# 7.3対応が来るのではないかと思われます。

ちなみに、[roslynのナイトリービルド](https://dotnet.myget.org/gallery/roslyn)に挙がっているVSIXやNuGetパッケージをインストールすれば、[結構ちゃんとC# 7.3が使えていました](https://gist.github.com/ufcpp/b4b505077f589235afb169652e10ee8d)。

そして、作業が落ち着いたタイミングで毎度やってくる「一斉投稿」が昨日来たと…

## 最近採用が決まった提案

とりあえず今日はこの話題のみ。
[3/19のDesign Note](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-03-19.md)で今後取り組む作業の選別をしたようで、
いろんな提案issueが新たにChampioned(将来取り組むこと自体は決定)に昇格しています。

取り組み時期はたいてい「8.X」。「8.0ですらなくさらにその後」という意味で、実際のところ「未定」と大差ないやつです。
そんな状態のものなので、具体的な文法はこれからまだだいぶ変わると思います。

### default in deconstruction

- [Champion: allow 'default' in deconstruction #1394](https://github.com/dotnet/csharplang/issues/1394)

↓みたいな書き方を認めてほしいというもの。

```csharp
int x;
int y;
(x, y) = default; // x = default; y = default; と同じ意味
```

### and, or, and not パターン

- [Champion "and, or, and not patterns" #1350](https://github.com/dotnet/csharplang/issues/1350)

↓みたいに、パターン マッチングで条件のところに and, or, not を書けるようにしたいとのこと。

```csharp
switch (o)
{
    case 1 or 2:
    case Point(0, 0) or null:
    case Point(var x, var y) and var p:
    case not string _:
}
```

### 型引数の部分的な型推論

- [Champion: "Partial Type Inference" #1349](https://github.com/dotnet/csharplang/issues/1349)

いくつか文法案は出ているものの、そのうちの1つで書くと、↓みたいな感じ。

```csharp
M<int, >(args); // 2個目の型引数だけは args から推論できて、1個目は無理な時、こう書けるようにしたい
```

### 制約なしの型引数に対して is null を認めたい

- [Champion "permit `t is null` for unconstrained type parameter" #1284](https://github.com/dotnet/csharplang/issues/1284)

ちょっと説明しにくいんですけど、以下のような感じ。`where T : class`なしの`T t`に対して、`t is null`を認めたい。

```csharp
void M(string s)
{
    if (s is null) { } // OK。クラスだし、null チェックしたい
}

void M(int x)
{
    if (x == null) { } // 警告は出るけど別にエラーにはならない。常にfalse
    // ↑あんまり良い話ではないけど、default とかジェネリクスがなかった頃の名残っぽい
}

void M1<T>(T t)
    where T : class
{
    if (t is null) { } // OK。クラス制約あるし。
}

void M2<T>(T t)
{
    if (t is null) { } // 今は NG。
    // とはいえ、構造体の == null が OK なんだから別にこれも認めていいでしょ。常にfalseで
}
```

### 暗黙的なスコープのusingステートメント

- [Champion "Implicitly scoped using statement" #1174](https://github.com/dotnet/csharplang/issues/1174)

以下のような、`using`したいリソースがたくさんあるときのネスト問題への対処。

```csharp
using (var d = SomeDisposable())
{
    // ここのネストが1段深くなるのがしんどい時がある
}

// 特に、多段の時。最後の1個以外は {} を省略できるとはいえ
using (var d1 = SomeDisposable())
using (var d2 = SomeDisposable())
using (var d3 = SomeDisposable())
using (var d4 = SomeDisposable())
using (var d5 = SomeDisposable())
{
}
```

以下のような書き方を予定。

```csharp
{
    // 変数宣言の前に using を付けることで、その変数のスコープを using のスコープにする
    using var d1 = SomeDisposable();
    using var d2 = SomeDisposable();
    using var d3 = SomeDisposable();
    using var d4 = SomeDisposable();
    using var d5 = SomeDisposable();

    // Dispose が走るのは、変数がスコープを抜ける時
    // = このブロックから抜けるとき
}
```

### defer ステートメント

- [Champion "defer statement" #1398](https://github.com/dotnet/csharplang/issues/1398)

Swift にあるやつ。

```csharp
static void Main()
{
    defer
    {
        Console.WriteLine("関数を抜ける時に呼ばれる"); // 例外があっても常に
    }

    Console.WriteLine("こっちの方が先に表示される");
}
```

「一回り外側のブロックに影響する」っていう点が気持ち悪くて据え置きになっていたんですが…
前節の`using var`を認めてしまった以上、それを理由にリジェクトできなくなった感じ。

前節の`using var`を使って、以下のように代用できないこともないんですが、
これだとラムダ式のオーバーヘッド(デリゲートのヒープ確保とインライン展開の阻害)が掛かるのが嫌だそうです。

```csharp
    using var d = new ActionDisposable(() =>
    {
        Console.WriteLine("関数を抜ける時に呼ばれる");
    });

    Console.WriteLine("こっちの方が先に表示される");
```

### ユーザー定義の位置指定パターン(positional patterns)

- [Champion "User-defined Positional Patterns" #1047](https://github.com/dotnet/csharplang/issues/1047)

[パターン マッチング](https://github.com/dotnet/csharplang/blob/master/proposals/patterns.md)、C# 7.0時点では「型パターン」とか「定数パターン」とか、一部分だけが実装されました。
残りの大部分は、現状、C# 8.0で提供する予定になっています。

そんな中、さらに C# 8.0からも外れて「8.X」にしようという風に外されたのがこいつ。

```csharp
struct Cartesian
{
    public double X;
    public double Y;
    public Cartesian(double x, double y) => (X, Y) = (x, y);

    // こいつを使った positional パターンは C# 8.0 で入る予定
    public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
}

class Polar
{
    // こんな感じの定義を書くことで、Cartesian p を p is Polar(var r, var t) みたいなパターンに掛けることができる仕様がある
    // が、こいつは C# 8.0 では入らない
    public static bool operator is(Cartesian p, out double radius, out double theta)
    {
        radius = Math.Sqrt(p.X * p.X + p.Y * p.Y);
        theta = Math.Atan2(p.Y, p.X);
    }
}
```
