---
title: "ピックアップRoslyn 6/30: Working with Data"
source_url: "https://ufcpp.net/blog/2018/6/pickuproslyn0630/"
content_type: "BlogEntry"
published_at: "2018-06-30T19:38:40"
updated_at: "2018-06-30T19:38:40"
tags: []
umbraco_id: 2158
parent_id: 2157
sort_order: 0
aliases: []
---

# ピックアップRoslyn 6/30: Working with Data

C# 6.0 くらいの頃から脈々とずっとテーマに挙がっている「データ」関連の機能で、2つほど提案が挙がっています。

- [
Proposal: "data" classes for C# #1667](https://github.com/dotnet/csharplang/pull/1667)
- [Proposal: Named tuples #1673](https://github.com/dotnet/csharplang/pull/1673)

新しいものが出たというよりは、プライマリ コンストラクターとかレコード型とか言われていたものを、コンパクトに分割した感じのものです。

長らく先延ばしになっていた機能ですが、C# 8.0 でいよいよ実装しようといことで、
詳細を詰めた結果2つに分かれたという感じだと思われます。

## data クラス/構造体

1つ目は、data クラス/構造体 と言われるもので、

- `class`/`struct` の前に `data` 修飾子を付ける
- public なフィールド、自動プロパティから、以下のものを自動生成
  - `GetHashCode`
  - `Equals`
  - `==`, `!=` 演算子
  - `ToString`

というようなもの。

印象としては、[匿名型](../../../../study/csharp/oop/oo_class.md#anonymous)の延長で、ちゃんとしたクラス・構造体に昇格させたいとい時に使うものな感じです。

```csharp
using System;

// class に data 修飾子を付ける
data class Point
{
    public int X { get; }
    public int Y { get; }
}

class Program
{
    static void Main()
    {
        // 匿名型
        var p1 = new { X = 1, Y = 2 };

        // data クラス
        var p2 = new Point { X = 1, Y = 2 };

        // 比較とかが自動的に作られる
        Console.WriteLine(p2 == new Point { X = 1, Y = 2 });
    }
}
```

### 対象となるフィールド/プロパティ

基本的には、比較やハッシュ値計算に使われるのは public なフィールドと自動プロパティだけです。
private なものや、自動実装でないものは除外されます。
(「データから計算で得られる値を、1回だけ計算してキャッシュしておきたい」みたいなとき、そのキャッシュを比較・ハッシュ値計算に使うことはあまりないので。)

ただ、自動実装でないプロパティでも、`DataMember`属性を付ければ、比較・ハッシュ値計算の対象にできます。

### immutable データに対してオブジェクト初期化子

また、data クラス/構造体では、immutable なデータ(get-only なプロパティ)に対してもオブジェクト初期化子が使えます。
(これまでの C# だと、匿名型で特別扱いで認められてた。通常のクラスだと、オブジェクト初期化子が使えるのは書き換え可能なフィールド/プロパティだけ。)

例えば上記の例では、`X`, `Y` の2つのプロパティは get-only ですが、`new Point { X = 1, Y = 2 }` という書き方が許されます。
これを認めるために、get-only プロパティを、実際には以下のようにコード生成する予定だそうです。

```csharp
class Point
{
    // <> から始まる名前は、通常の C# コードでは書けない。
    // 通常は使えない名前を使うことで、C# コードからは読み書きさせない。
    // (コンパイラー生成のコードからだけ読み書きする。)
    private int <>X;
    public int X => <>X;

    private int <>Y;
    public int Y => <>Y;

    // 以下、Equals や GetHashCode なども生成
}

class Program
{
    static void Main()
    {
        // コンパイラーはオブジェクト初期化子を以下のように展開
        var p2 = new Point();
        p2.<>X = 1;
        p2.<>Y = 2;
    }
}
```

## 名前付きタプル

一方で、[タプル](../../../../study/csharp/datatype/tuples.md)の延長で、ちゃんとしたクラス・構造体に昇格させるみたいな構文も追加。名前付きタプルと呼ぶそうです。

以下のように、クラス名に続けてタプルみたいなものを書くことで、タプルに名前が付きます。

```csharp
using System;

// 型名の後ろにタプル的なものを書く
class Point(int X, int Y);

class Program
{
    static void Main()
    {
        // タプル
        var p1 = (X: 1, Y: 2);

        // 名前付きタプル
        var p2 = new Point(1, 2);
        Console.WriteLine(p2.X);
        Console.WriteLine(p2.Y);
    }
}
```

見ての通り、コンストラクターとプロパティが生成されます。
また、タプルと同様、比較、ハッシュ値計算や、`Deconstruct` メソッドなども生成されるそうです。

この例だと `class Point(int X, int Y);` だけ書きましたが、クラスの中身も持てるそうです。
(昔あったレコード型の提案に結構近い。)

また、`class Point(int, int);` と言うように、メンバー名は省略できます。
この場合、タプルと同様、`Item1`, `Item2` というような番号付きのメンバーが生成されます。
