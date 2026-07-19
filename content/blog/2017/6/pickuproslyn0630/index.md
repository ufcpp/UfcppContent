---
title: "ピックアップ Roslyn 6/30"
source_url: "https://ufcpp.net/blog/2017/6/pickuproslyn0630/"
content_type: "BlogEntry"
published_at: "2017-06-30T14:13:39"
updated_at: "2017-06-30T14:13:39"
tags: []
umbraco_id: 2074
parent_id: 2070
sort_order: 1
aliases: []
---

# ピックアップ Roslyn 6/30

[csharplang](https://github.com/dotnet/csharplang)に出てる提案の整理をしたみたいで、
いくつかの提案に「Proposal Champion」ラベルが付き始めたようです。

「Proposal Champion」は、Proposal(提案が出てるだけ)とChanpion(C#チームの誰かがオーナーになって進めることが決まった段階)の間くらいのつもりっぽく。
「いつかは取り組むけども、今すぐとはいかない」くらいみたいです。

以下、いくつか紹介。

## 末尾以外で名前付き引数

- [Champion "Non-trailing named arguments" #570](https://github.com/dotnet/csharplang/issues/570)

末尾以外の場所にある引数でも名前付き引数を使いたいという話。

どうもこれはプロトタイプ実装がすでに始まってるっぽい。

```csharp
static void Main()
{
    // よく言われる話で、bool なフラグがたくさん並ぶとどれがどれかわからない
    M(true, true, true);

    // 名前付き引数には「オプションな引数を省略可能にする」という意味もあるけども
    M(isC: true);

    // どのフラグが何だったかを明記する意味でも使う
    M(isA: true, isB: true, isC: true);

    // (これまで) 末尾の引数だけを名前付きにすることならできた
    M(true, true, isC: true);

    // (提案) それ以外の位置でも、一部分だけ名前付きにできるようにしたい
    M(isA: true, true, true);
}

private static void M(bool isA = false, bool isB = false, bool isC = false)
{
}
```

## どこででも拡張メソッドを定義

- [Champion "Support extension methods everywhere" #301](https://github.com/dotnet/csharplang/issues/301)

これまでだと拡張メソッドは、トップレベル(名前空間直下)の静的クラスの中でしか定義できませんでした。

でも、「あるクラスの中でだけ使う拡張メソッドのために、外に1クラス作るのは嫌」というような話はたびたび出てくるわけで、
「静的でないクラスでも拡張メソッドを定義したい」とか、
「入れ子のクラス内で拡張メソッドを定義したい」とかいう要望はよく見ます。

ということで、制限を緩めようという話がついに検討に上がったみたいです。

## 0b, 0x の直後に _ 区切り

- [Champion "allow digit separator after 0b or 0x" #65](https://github.com/dotnet/csharplang/issues/65)

C# 7で、数値リテラルの数字と数字の間を `_` で区切れるようになりました
([数字区切り文字](../../../../study/csharp/start/stnumber.md#digit-separator))。

で、数字区切り(digit separator)の名前通り、ほんとに数字と数字の間にしか `_` を書けません。

```csharp
var a = 1_2_3; // OK
var b = 0b1111_1111; // OK
var c = 0b_1111_1111; // エラー。0b の直後に _ を書くのはダメ
var d = 0xab_cd; // OK
var e = 0x_ab_cd; // エラー。0x の直後に _ を書くのはダメ
```

元々、`0b` や `0x` の直後にも `_` を書きたいという要望は多々あったんですが、
「名前通りにしたい」、「数字区切りなのに数字でないところで区切れるようにはあんまりしたくない」みたいな雰囲気で、
C# 7ではこういう仕様になっています。

まあでも、やっぱり要望が大きく、`0b` や `0x` の直後の `_` (上記サンプルの`c`、`e`みたいな書き方)も認めようという流れになりつつあるみたいです。

## 分解にタプルは要らないのではないか

- [Surprising System.ValueTuple requirement for deconstruction #18629](https://github.com/dotnet/roslyn/issues/18629)

「提案」ってわけではないんですが。

C# 7で[分解](../../../../study/csharp/datatype/deconstruction.md)構文(deconstruction)が入りましたが、例えば以下のような場合を考えてみます。

```csharp
class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) => (X, Y) = (x, y);
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}

class Program
{
    static void Main()
    {
        var (x, y) = (1, 2);

        // ↑現状は一度 ValueTuple を作ってから、それぞれ x, yに代入するようなコードに展開される
        //var t = new ValueTuple<int, int>(1, 2);
        //var x = t.Item1;
        //var y = t.Item2;

        // でも、仕様上は以下のような状態に展開する最適化を認めてるし、実際将来的にそういうコード生成する可能性がある
        //var x = 1;
        //var y = 2;

        // だったら、var (x, y) = (1, 2); に ValueTuple は不要なはず

        var (a, b) = new Point(1, 2);

        // ↑この場合は完全にタプルは要らないはず
        // 以下のようなコードに展開される(どこにも ValueTuple は出てこない)
        //var p = new Point(1, 2);
        //int a, b;
        //p.Deconstruct(out a, out b);

        // にもかかわらず、現状、分解を使っただけで ValueTuple (.NET 4.7以上、もしくは、System.ValueTuple パッケージの参照)が求められる
    }
}
```

現状だと、不要なはずの `ValueTuple` が求められます。

これは、将来的に以下のような構文を認めるために、分解とタプル構築の内部表現を統一したのの余波です。

```csharp
var t = (var x, var y) = (1, 2);

// ↑これは、↓これと同じ意味
// (var x, var y) = (1, 2);
// var t = (x, y);
```

ですが、まあ、利用者としては不要なはずのパッケージ参照が必要になるというのは気分がいいものではありません。
実際「バグ報告」扱いで報告が入ってしまっている状況。

で、まあ、修正するようです。
本当に `ValueTuple` が必要になるまでは、パッケージ参照を求めないよう修正済み。
C# 7.2辺りに入れる予定みたいです。
