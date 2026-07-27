---
title: "データの構造化(複合型)"
source_url: "https://ufcpp.net/study/csharp/structured/st_struct/"
content_type: "Article"
published_at: "2015-05-06T14:09:03"
updated_at: "2021-07-22T15:03:11"
tags: []
umbraco_id: 1242
parent_id: 1217
sort_order: 14
aliases:
  - "/study/csharp/st_struct.html"
---

# データの構造化(複合型)

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# では、複数の異なるデータ型の変数を1まとめにして管理するため、クラスや構造体と呼ばれるものを定義して使うことが出来ます。

##### <a id="sec-generated-title-2"></a>ポイント

* 複合型: 複数のデータを1つにまとめて使うための型
* C# の複合型にはクラスと構造体の2種類ある
  * クラス: `class 型名 { メンバー定義 }`
  * 構造体: `struct 型名 { メンバー定義 }`
  * 大体の場合はクラスを使う
  * (C# 9.0 ではもう1つ[レコード型](../datatype/record.md)というものも追加)
* 例えば、「2次元中の点」を表す複合型なら `class Point { public int X; public int Y; }`

## <a id="sec-generated-title-3"></a> <a id="about"></a>複合型

今まで <code>int</code> や <code>double</code> などの組込み型だけを使ってきましたが、
組込み型だけでは複雑なデータを扱うことはできません。
例えば、名簿を管理するプログラムを作りたいとします。
説明を簡単にするために、名前と年齢と住所のみを考えましょう。
この、名前と年齢と住所を一まとめにして管理できるような型があれば便利だとは思いませんか？
要するに、以下のような「データをまとめた型」を使いたいことがあります。

- `個人情報 = { 名前, 年齢, 住所 }`

このような、「データをまとめた型」を複合型（complex type）と呼びます。

C# の場合、複合型には
<strong id="class" class="keyword">クラス</strong>（class）と
<strong id="struct" class="keyword">構造体</strong>（structure type）の2種類あり、
以下のように定義します。
(C# 9.0 では[レコード型](../datatype/record.md)というものが追加されました。これはクラスの亜種みたいなものです。)

まず、クラスの定義は以下の通り。

```csharp
class クラス名
{
  クラスのメンバー
}
```

一方、構造体の定義は以下のようなります。`class`キーワードの代わりに`struct`キーワードを使う以外はクラスとほぼ同じです。

```csharp
struct 構造体名
{
  構造体のメンバー
}
```

クラスと構造体の違いについては別項([構造体](../resource/rm_struct.md))で説明します。
当面は、<em>ほとんどの場合クラスを使っておけばいい</em>とだけ覚えておいてください。

クラスや構造体のメンバーとして書けるものは色々ありますが、詳細は今後少しずつ説明していきます。
本項の主題、つまり、データをまとめる意図で使うのは、データを保持するためのメンバーです。
これを<strong id="field" class="keyword">フィールド</strong>(field: 作業領域)と呼びます。

(C#ではフィールドという呼び方が一般的ですが、
他のプログラミング言語(特に古めの言語)では<em>メンバー変数</em>(member variable)と読んだりもします。)

フィールドは、以下のように、クラスや構造体の中に変数宣言を書くような書式で定義します。

```csharp
class クラス名
{
    フィールドの型 フィールド名;
}
```

例えば、先ほどの例の名前と年齢と住所を一まとめにした構造体を定義したければ、以下のように書きます。
構造体の名前は Person にでもしておきましょう。

```csharp
class Person
{
  public string name;    // 名前
  public uint   age;     // 年齢
  public string address; // 住所
}
```

<code>public</code> というキーワードについては「[実装の隠蔽](../oop/oo_conceal.md)」で説明します。

このクラスを利用するときには以下のようにします。

```csharp
Person p = new Person(); // string とか配列と同じような感じで宣言＆初期化

// 「 . 」 を使って各メンバーにアクセスする
// 構造体変数名.メンバー名
p.name    = "ちゆ";
p.age     = 12;
p.address = "http://www.tiyu.to";
```

複合型、特にクラスの機能の詳細については、「[オブジェクト指向](../index.md#oop)」で説明して行きます。

##### <a id="sec-generated-title-4"></a>サンプル

```csharp
using System;

/// <summary>
/// 2次元の点をあらわすクラス
/// </summary>
class Point
{
  public double x; // x 座標
  public double y; // y 座標

  public override string ToString()
  {
    return "(" + x + ", " + y + ")";
  }
}

class StructSample
{
  static void Main()
  {
    Point p1 = new Point();
    Point p2 = new Point();

    p1.x = 100;
    p1.y = 0;

    p2.x = 400;
    p2.y = 400;

    Console.Write("{0} と {1} の間の距離は {2}", p1, p2, Distance(p1, p2));
  }

  /// <summary>
  /// 2点間の距離を求める
  /// </summary>
  static double Distance(Point p1, Point p2)
  {
    double dx = p1.x - p2.x;
    double dy = p1.y - p2.y;
    return Math.Sqrt(dx*dx + dy*dy);
  }
}
```


```console
(100, 0) と (400, 400) の間の距離は 500
```

## <a id="sec-generated-title-5"></a> <a id="anonymous"></a>匿名の複合型

<h5 class="version version3">Ver. 3.0</h5>
<h5 class="version version7">Ver. 7.0</h5>

C# 3.0 からは[匿名型](../start/sp3_inference.md#anonymous)、
C# 7.0 からは[タプル](../datatype/tuples.md)という機能が追加されて、
`struct`や`class`などを定義しなくても複合型を書けるようになりました。

```csharp
// 匿名型
// new { } 内に値を並べる
var x = new { p.name, p.age };
```

```csharp
var p = new Point();

// タプル
// () 内に値を並べる
var q = (Math.Sqrt(p.X * p.X + p.Y * p.Y), Math.Atan2(p.Y, p.X));
```

匿名型とタプルの違いについては「[名前のない複合型](st_anonymoustype.md)」で説明します。
## <a id="exercise"></a>演習問題

### <a id="exercise-str1"></a>問題 1


サンプル中の Point 構造体を使って、三角形を表す構造体 <code>Triangle</code> を作成せよ。
（3つの頂点を a, b, c 等のメンバー変数として持つ。）

また、作成した構造体に、三角形の面積を求めるメンバー関数 <code>GetArea</code>を追加せよ。

```csharp
/// <summary>
/// 三角形の面積を求める。
/// </summary>
/// <returns>面積</returns>
public double GetArea()
```



#### 解答例 1


```csharp
using System;

/// <summary>
/// 2次元の点をあらわす構造体
/// </summary>
struct Point
{
  public double x; // x 座標
  public double y; // y 座標

  public override string ToString()
  {
    return "(" + x + ", " + y + ")";
  }
}

/// <summary>
/// 2次元空間上の三角形をあらわす構造体
/// </summary>
struct Triangle
{
  public Point a;
  public Point b;
  public Point c;

  /// <summary>
  /// 三角形の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public double GetArea()
  {
    double abx, aby, acx, acy;
    abx = b.x - a.x;
    aby = b.y - a.y;
    acx = c.x - a.x;
    acy = c.y - a.y;
    return 0.5 * Math.Abs(abx * acy - acx * aby);
  }
}

class Test
{
  static void Main()
  {
    Triangle t;
    t.a.x = 0;
    t.a.y = 0;
    t.b.x = 3;
    t.b.y = 4;
    t.c.x = 4;
    t.c.y = 3;
    Console.Write("{0}\n", t.GetArea());
  }
}
```
