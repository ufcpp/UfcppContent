---
title: "C# vNext Preview"
source_url: "https://ufcpp.net/blog/2018/5/cs80preview/"
content_type: "BlogEntry"
published_at: "2018-05-12T20:00:42"
updated_at: "2018-05-12T20:05:45"
tags: []
umbraco_id: 2153
parent_id: 2150
sort_order: 2
aliases: []
---

# C# vNext Preview

以下のページで、C# 8.0のプレビュー公開を始めたみたです。

- [vNext Preview](https://github.com/dotnet/csharplang/wiki/vNext-Preview)

## インストール

これまででも、まだ Visual Studio プレビュー版にも組み込まれていないような機能の類も、VSIX (Visual Studio 拡張)や NuGet 参照でコンパイラーだけ差し替えることで使えたりはしました。
[roslyn のデイリー ビルド](https://dotnet.myget.org/gallery/roslyn)を「パッケージ ソース」にして、Microsoft.Net.Compilers パッケージを参照すれば行けます。

ただ、このやり方だと、IDEのC#エディターの IntelliSense は最新版になりません。
ビルドを実行するとコンパイルは通るんですが、エディター上ではエラーの赤線だらけになります。

で、今回公開された[vNext Preview](https://github.com/dotnet/csharplang/wiki/vNext-Preview)は、インストールとアンインストール用のスクリプトが入っています。基本的には中身は VSIX なんですが、たくさんの VSIX が入っていて、依存順が複雑だとかでそれぞれ個別に入れるのは無理っぽい感じです。
なので、

- Visual Studio を全て落とす
- インストール スクリプト(PowerShell)を実行する
- もし、Visual Studio のバージョンアップをする際には、一度 vNext Preview をアンインストールしてから

という手順を踏んでほしいとのこと。

## 少し先の機能

最近だと、Visual Studio 自体が、あるバージョンをリリースしてほとんどすぐに、[次のバージョンのプレビュー](https://www.visualstudio.com/ja/vs/preview/)を公開しています。
インストールも割と簡単で、正式リリース版との共存もできます。

そして、C# もそれでプレビューを体験できたりしました。
Visual Studio のリリース周期は最近3～4か月ごとなので、そのくらい先のものであればそんなに苦労することなく試せます。

一方、今回インストール用スクリプトを用意して提供しているのは、
要するに2バージョン以上先での提供予定のものを早めに試してもらいたいということでしょう。
久々のメジャー バージョンアップですし、ちょっと大き目な機能も入る予定です。

## C# 8.0 プレビューの現状

とはいえ、こないだ公開された「5月4日ビルド版」では、2個しか C# 8.0 の新機能が入っていなかったりはします。
入っているのは、

- 再帰パターン
- ranges

の2つ。

### 再帰パターン

C# 7.0 で[パターン マッチング](../../../../study/csharp/datatype/typeswitch.md)が入ったわけですが、
C# 7.0 時点では、元々計画に挙がってたうちの一部分(型パターン、型スイッチ)だけが実装されています。

C# 8.0 では、7.0 のときに先送りされた再帰パターンが入る予定です。

例えば以下のようなクラスがあったとして、

```csharp {title="例"}
class Base { }
class A : Base
{
    public int X { get; set; }
    public int Y { get; set; }
    public A(int x, int y) => (X, Y) = (x, y);
}
class B : Base
{
    public string Name { get; set; }
    public int Value { get; set; }
    public B(string name, int value) => (Name, Value) = (name, value);
    public void Deconstruct(out string name) => name = Name;
    public void Deconstruct(out string name, out int value) => (name, value) = (Name, Value);
}
```

以下のようなコードなら C# 7.0 でも書けました。

```csharp {title="C# 7.0 の型パターン"}
static int M(Base obj)
{
    switch (obj)
    {
        case A a: return a.X * a.Y;
        case B b when b.Name == "one": return b.Value;
        case B b when b.Name == "two": return 2 * b.Value;
        case B b when b.Name == "three": return 3 * b.Value;
        default: throw new IndexOutOfRangeException();
    }
}
```

C# 8.0 では以下のような、再帰的なパターンが使えるようになります。

```csharp {title="C# 8.0 の再帰パターン"}
static int M(Base obj)
{
    switch (obj)
    {
        case A { X: var x, Y: var y }: return x * y;
        case B ("one") { Value: var v }: return v;
        case B ("two") { Value: var v }: return 2 * v;
        case B ("three") { Value: var v }: return 3 * v;
        default: throw new IndexOutOfRangeException();
    }
}
```

`B("one")` みたいな、`()` の部分は位置指定パターンと言って、`Deconstruct` メソッドが呼ばれています(「[分解](../../../../study/csharp/datatype/deconstruction.md)」と同じ仕組み)。
残りの `{}` の部分はプロパティ パターンと言って、プロパティに対する `X is var x` などに展開されます。

#### switch 式

また、`switch` 式も追加されます。
式です。`=>` の後ろとかにも書けます。
今のところは以下のような構文になる予定。

```csharp {title="switch 式"}
static int M(Base obj)
    => obj switch
    {
        A { X: var x, Y: var y } => x * y,
        B ("one") { Value: var v } => v,
        B ("two") { Value: var v } => 2 * v,
        B ("three") { Value: var v } => 3 * v,
        _ => throw new IndexOutOfRangeException()
    };
```

#### {} パターンで null チェック

ちなみに、プロパティ パターン (`{}` を使ったパターン)には null チェックが伴うそうです。

```csharp {title="{} パターンで null チェック"}
string s = null;

// null は型情報を持ってなかったり。たとえ、静的な型が一致していても is は常に false。
if (s is string) Console.WriteLine("ここは絶対通らない");

// is string x みたいな変数宣言を伴ってても同じ。
if (s is string x) Console.WriteLine("ここも通らない");

// が、var パターンは常に true。見た目 is string に似てるけど、結果が違う。
if (s is var  y) Console.WriteLine("ここは通る");

// で、プロパティ パターンを使って、null チェック付きの var に近いことができる。
if (s is { }) Console.WriteLine("ここは通らない");
```

#### タプル switch

あと、タプルに対する switch では、`()` を1重に省略できます。

```csharp {title="タプル switch"}
static int M(int x, int y)
{
    // 本来は、switch ((x, y))
    switch (x, y)
    {
        case (1, 1): return 1;
        case (1, 2): return 2;
        case (2, 1): return 3;
        case (2, 2): return 4;
        default: return 0;
    }
}
```

### ranges

ranges は、`1..3` みたいな書き方で「1から3まで(ただし3は含まない)のインデックス」みたいな範囲を表す記法です。
`Range`構造体と`Index`構造体に展開される予定で、
[この`range.cs`](https://raw.githubusercontent.com/dotnet/csharplang/master/proposals/ranges.cs)みたいな定義が必要です。

```csharp {title="ranges"}
using System;

class Program
{
    static void Main()
    {
        var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        // 1～4番目 → { 2, 3, 4 }
        Write(data[1..4]);
        // ↑は、↓と同じ結果
        Write(data.AsSpan().Slice(1, 4 - 1));

        // 2～(Length - 2)番目 = 最初と最後の2要素を飛ばす → { 3, 4, 5, 6 }
        Write(data[2..^2]);
        // ↑は、↓と同じ結果
        Write(data.AsSpan().Slice(2, (data.Length - 2) - 2));

        // 5～末尾 → { 6, 7, 8 }
        Write(data[5..]);

        // 先頭～3 → { 1, 2, 3 }
        Write(data[..3]);

        // 全体
        Write(data[..]);
    }

    static void Write(Span<int> s)
    {
        foreach (var x in s)
        {
            Console.Write(x);
            Console.Write(" ");
        }
        Console.WriteLine();
    }
}
```

正直、`Slice(start, length)` みたいな記法との差が少なすぎて、便利さで言うとそこまで大きくはないんですが。
以下のような要件があるので、それなりに必要性はあります。

- `Slice(x, y)` みたいな書き方では、第2引数が「長さ」なのか「終端インデックス」なのかで迷う
- 「末尾から n 番目」みたいなのは`data.Length - n` みたいな書き方が必要でしんどい
- 特に多次元データの時に `data[a..b, c..d, e..f]` みたいに書きたい

現時点では、以下の実装はないみたいです。

1. start, length 型の ranges (「a を始点に長さ b」みたいなやつ)
1. inclusive ranges (「a～b まで(bも<em>含む</em>)」みたいなやつ)
1. ユーザー定義の `..` 演算子

このうち、C# 8.0 正式リリースまでに入るかもしれないのは1の start, length 型 ranges くらい。
残りは「その先また改めて検討」のはずです。

#### 一時的な「拡張インデクサー」

ちなみに、このプレビューでは、「拡張インデクサー」みたいなものが一時的に入っています
(`T[]`、`Span<T>`、`string`に対する`Range`型引数のインデクサーを拡張として追加しています)。
これはほんとに一時的な対処で、C# 8.0でこの文法で「拡張インデクサー」が使えるわけではありません。

正式には、「[Type Class](https://github.com/dotnet/csharplang/issues/110)」という別提案が出ていて、これ待ちです。
もしかしたらこれも C# 8.0 で入るかも。
