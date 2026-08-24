---
title: "小ネタ オブジェクト初期化子"
source_url: "https://ufcpp.net/blog/2016/12/tipsobjectinitializer/"
content_type: "BlogEntry"
published_at: "2016-12-05T01:10:05"
updated_at: "2016-12-05T01:10:05"
tags: []
umbraco_id: 1982
parent_id: 1969
sort_order: 4
aliases: []
---

# 小ネタ オブジェクト初期化子

今日の小ネタは、オブジェクト初期化子について、意外と知られてないらしい話。

## 問い

唐突ですが問題です。以下の3つのコードはそれぞれどういう意味でしょう。

```csharp {title="オブジェクト初期化子パターン1"}
var x = new Line
{
    A = new Point { X = 1, Y = 2 },
    B = new Point { X = 3, Y = 4 },
};
```

```csharp {title="オブジェクト初期化子パターン2"}
var x = new Line
{
    A = { X = 1, Y = 2 },
    B = { X = 3, Y = 4 },
};
```

```csharp {title="オブジェクト初期化子パターン3"}
var x = new Line
{
    A = new { X = 1, Y = 2 },
    B = new { X = 3, Y = 4 },
};
```

ついでに、将来的に認められるようになるかもしれないパターンをもう1つ。

```csharp {title="オブジェクト初期化子パターン4 (将来OKになるかも)"}
var x = new Line
{
    A = new() { X = 1, Y = 2 },
    B = new() { X = 3, Y = 4 },
};
```

## 答え合わせの前に

答えを説明する前に、コード中に出ていた2つの型、`Point`、`Line`について。

まず、`Point`の方は、どのパターンであっても以下のような感じである必要があります。

```csharp {title="Point の例"}
class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

構造体でもいいんですが、その場合、`Line`側に参照戻り値が必要になります。

`Line`の方は、2パターンあります。
1つは、プロパティが書き換え可能なもの。

```csharp {title="書き換え可能な Line の例"}
class Line
{
    public Point A { get; set; }
    public Point B { get; set; }
}
```

もう1つは、getのみのプロパティに対して、コンストラクター、もしくは、プロパティ初期化子で初期値を与えているものです。

```csharp {title="get のみな Line の例"}
class Line
{
    public Point A { get; } = new Point();
    public Point B { get; } = new Point();
}
```

## 答え

### パターン1

パターン1のやつは、一番シンプルというか、多くの方がこれのつもりでオブジェクト初期化子を使っているのではないかと思います。

```csharp {title="パターン1の答え"}
public static void Q()
{
    var x = new Line
    {
        A = new Point { X = 1, Y = 2 },
        B = new Point { X = 3, Y = 4 },
    };
}

public static void A()
{
    var x = new Line();
    x.A = new Point { X = 1, Y = 2 };
    x.B = new Point { X = 3, Y = 4 };
}
```

展開結果を見ての通り、`x.A`や`x.B`に対する代入が発生するので、`A`, `B` は set アクセサーを持つ必要があります。

その結果、書き換え可能な方の `Line` 実装に対してならこの構文を使えますが、
getのみの方の `Line` 実装には使えません。

### パターン2

意外と知られてないのはこいつですね。

```csharp {title="パターン2の答え"}
public static void Q()
{
    var x = new Line
    {
        A = { X = 1, Y = 2 },
        B = { X = 3, Y = 4 },
    };
}

public static void A()
{
    var x = new Line();
    x.A.X = 1;
    x.A.Y = 2;
    x.B.X = 3;
    x.B.Y = 4;
}
```

オブジェクト初期化子は再帰的に書けます。
その場合、この例の `x.A.X` というように、全部展開されて、そこに代入が行われます。

ここで注意が必要なのは、`x.A` の初期化は外からは行われないということです。
もしも、`Line` のコンストラクター内で `A` を初期化していなければ、当然のようにぬるぽります。

つまり、getのみの方の `Line` に対しても使える代わりに、
書き換え可能な方の `Line` 実装みたいにコンストラクター内での初期化をしていないものに対してこの書き方を使うと実行時エラーを起こします。

### パターン3

並べられると、似たようなもので全然違う結果になるので気持ち悪くなりますが、
まあ、個別によく見るとそんなに不思議なものではないと思います。

パターン3は、単に匿名型を代入しているだけ。

```csharp {title="パターン3の答え"}
public static void Q()
{
    // 実はコンパイル エラー
    var x = new Line
    {
        A = new { X = 1, Y = 2 },
        B = new { X = 3, Y = 4 },
    };
}

public static void A()
{
    var x = new Line();
    // この new { } は匿名型。
    // A, B は Point 型なので、匿名型だと型があってない。
    // つまり、コンパイル エラー: 匿名型を暗黙的に Point に変換できません
    x.A = new { X = 1, Y = 2 };
    x.B = new { X = 3, Y = 4 };
}
```

C#だと、コンパイル時にエラーなことがわかるんでそんなに問題はないと思うんですが。
もしも実行してみないとこの差がわからないとか言われたらちょっと殺意を覚えますね…

### パターン4

パターン4は将来の話。今現在はコンパイル エラーになります。
どういう構文が追加されそうかというと、左辺からの型推論です。

```csharp {title="パターン4の答え"}
public static void Q()
{
    var x = new Line
    {
        A = new() { X = 1, Y = 2 },
        B = new() { X = 3, Y = 4 },
    };
}

public static void A()
{
    var x = new Line();
    // new() って書き方で、左辺から型推論してくれる構文が入りそう。
    // この場合、A, B が Point なので、new () は new Point() の意味。
    x.A = new Point { X = 1, Y = 2 };
    x.B = new Point { X = 3, Y = 4 };
}
```

ものすごいほしい型推論機能です。早く実装されないかな…

とはいえ、まあ、こういう、並べると気持ち悪いコードが書けますよ、と。
`{ }`と`new { }`と`new() { }`で全部意味が違うという。
