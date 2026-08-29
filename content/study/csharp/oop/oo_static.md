---
title: "静的メンバー"
source_url: "https://ufcpp.net/study/csharp/oop/oo_static/"
content_type: "Article"
published_at: "2015-05-06T14:09:31"
updated_at: "2015-01-18T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 3.0"
  - "Ver. 6.0"
umbraco_id: 1257
parent_id: 1248
sort_order: 5
aliases:
  - "/study/csharp/oo_static.html"
---

# 静的メンバー

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="static-member" class="keyword">静的メンバー</strong>（static member）とは、
特定のインスタンスにではなく、クラスに属するフィールドやメソッドのことです。
そのため、静的変数のとこを<em>クラス メンバー</em>とも呼びます。
(クラス変数という呼び名の方が意味合い的には正しいのですが、
C言語から派生したというC#の歴史的な背景のため、静的変数という呼び方をします。)

「静的」という言葉は、各種メンバー（フィールド、メソッド、プロパティなど）それぞれに対して、<strong id="stfield" class="keyword">静的フィールド</strong>、<strong id="stmethod" class="keyword">静的メソッド</strong>、静的プロパティ、… などという使い方もします。
また、静的メンバーとの区別を明確にしたい場合には、通常のメンバー変数のことを<em>インスタンス メンバー</em>と呼びます。


##### <a id="sec-generated-title-2"></a>ポイント

* 静的メンバー: この呼び方は歴史的なもので、実際にはクラス メンバー（クラス メソッド、クラス フィールド）と呼ぶ方がいいかも。
    * static キーワードをつけると静的メンバーになる。



* クラス メンバー: クラスに属するもの。全インスタンスで共有されるもの。

* インスタンス メンバー: 今まで説明してきたメンバー。インスタンスごとに別の値になる。



## <a id="sec-generated-title-3"></a> <a id="use"></a>静的メンバーの使い方

クラスのメンバー（フィールドやメソッド）を定義する際に、<em><code>static</code></em>キーワードを付けることで、
その変数は静的メンバー変数・静的メソッドになります。
例えば、静的フィールドであれば以下のように書きます。

```csharp {title="static 変数の定義"}
static 型名 フィールド名
```


静的メンバーはクラスごとに唯一つの実体を持ち、すべてのインスタンスの間で共有されます。

例として、人間について考えてみましょう。
この場合、特定のインスタンスとは個人個人のこと、
クラスとは人間という種別そのもののことになるわけですが、
名前や年齢などは各個人ごとに異なります。
一方で、人という種の学名「Homo sapiens」などのように個体によらない共通のものもあります。
したがって、人間をあらわす<code>Person</code>というクラスを作成した場合、
<code>name</code>(名前)や<code>age</code>(年齢)といったメンバー変数を作りたい場合はインスタンス フィールドに、
<code>scientificName</code>(学名)などのクラス全体で共有すべき変数を作りたい場合は静的フィールドにすべきです。
(実際には学名などの普遍な値は定数(<code>const</code>)として定義すべきですが、
ここでは説明のためということでご容赦を。
定数の定義については後ほど説明します。)

```csharp {title="インスタンス フィールドと静的フィールドの例" highlight-text="static"}
class Person
{
  public string name; // 名前。個体ごとに違うので、インスタンス フィールドに。
  public int age;     // 年齢。同上、インスタンス フィールドに。

  public static string scientificName;
  // 学名。個体じゃなくて種によって決まるものなので、静的フィールドに。
}
```


静的メンバーはクラスに属する値なので、値を参照するには、変数を介してではなく、以下のようにします。

```csharp {title="静的変数の参照" highlight-text="静的フィールドは [クラス名.フィールド名] で参照する。"}
Person p = new Person()

p.name = "野上冴子"; // インスタンス フィールドは [インスタンス名.フィールド名] で参照する。
p.age  = 40;

Person.scientificName = "Homo Sapiens";
// 静的フィールドは [クラス名.フィールド名] で参照する。
```


また、メソッドに対して static を付けると、
クラスに属するメソッドになります。
（静的メンバーにしかアクセスできなくなります。
メソッドからインスタンス フィールドなどにアクセスする必要が特にない場合には、静的メソッドにしておく方が実行効率がいい。）

数学関数や数学定数などのように、そもそもインスタンスを持つ必要のないものもあります。
この場合にも、静的メソッド・静的フィールド（あるいは別項で説明する「定数」）を使います。

```csharp {title="インスタンスを持たない関数の例" highlight-ranges="6:10-6:16"}
using System;

class MyMath
{
  // sin x を求める関数。
  public static double Sin(double x)
  {
    double xx = -x * x;
    double fact = 1;
    double sin = x;
    for(int i=2; i<100;)
    {
      fact *= i; ++i; fact *= i; ++i;
      x *= xx;
      sin += x / fact;
    }
    return sin;
  }
}

class StaticSample
{
  static void Main()
  {
    Console.Write(MyMath.Sin(1));
  }
}
```


標準ライブラリの <code>Math.Sin</code> や <code>Console.Write</code> などは静的メソッドです。

```csharp
using System;

class Program
{
    static void Main()
    {
        var pi = 2 * Math.Asin(1); // 静的クラス Math の静的メソッド Asin を参照
        Console.WriteLine(Math.PI == pi); // 静的クラス Math の定数 PI を参照
    }
}
```



##### <a id="sec-generated-title-4"></a>補足: 関数

オブジェクト指向ではない（クラス的な概念を持たない）プログラミング言語でいう「関数」に一番近いのは、この静的メソッドです。

ちなみに、「[関数メンバー](../structured/st_function.md#function-member)」で説明していますが、
C# には「関数」的な動作をするメンバーとして、コンストラクター、プロパティ、インデクサーなどがあって、
これらの総称として「関数メンバー」という呼び方をします。


## <a id="sec-generated-title-5"></a> <a id="ctor"></a>静的コンストラクター

静的フィールドの初期化には、通常のコンストラクターではなく、<strong id="stconst" class="keyword">静的コンストラクター</strong>（static constructor）というものを使います。
静的コンストラクターの定義の仕方は、コンストラクターの前に <code>static</code> キーワードを付ける以外は通常のコンストラクターの定義の仕方と同じです。
例えば、先ほどの <code>Person</code> クラスを例に挙げると以下のようになります。

```csharp {title="静的コンストラクターの例" highlight-ranges="16:3-16:9"}
class Person
{
  string name; // 名前。インスタンス フィールド。
  int age;     // 年齢。インスタンス フィールド。

  static string scientificName; // 学名。静的フィールド。

  // 通常のコンストラクター
  public Person(string name, int age)
  {
    this.name = name;
    this.age  = age;
  }

  // 静的コンストラクター
  static Person()
  {
    Person.scientificName = "Homo sapiens";
  }
}
```


通常のコンストラクターが新しいインスタンスが生成されるたびに呼び出されるのに対して、
静的コンストラクターは1度だけ呼び出されます
（呼び出されるタイミングは、そのクラスの何らかのメンバーに初めてアクセスしたときです）。


##### <a id="sec-generated-title-6"></a>サンプル

```csharp {title="静的フィールドのサンプル"}
using System;

// 1台ごとに固有のIDが振られるような何らかの製品。
class Product
{
  static int id_generator;
  int id;

  static Product()
  {
    // 最初に1度だけ呼ばれ、id_generator を 0 に初期化。
    id_generator = 0;
  }

  public Product()
  {
    // 新しい製品が製造されるたびに新しい id を振る。
    id = id_generator;
    id_generator++;
  }

  /// <summary>
  /// その製品のIDを取得する。
  /// </summary>
  public int ID
  {
    get{return id;}
  }
}

class StaticSample
{
  static void Main()
  {
    for(int i=0; i<10; i++)
    {
      Product p = new Product();

      Console.Write("ID: {0}\n", p.ID);
    }
  }
}
```


```console
ID: 0
ID: 1
ID: 2
ID: 3
ID: 4
ID: 5
ID: 6
ID: 7
ID: 8
ID: 9
```



## <a id="sec-generated-title-7"></a> <a id="class"></a>静的クラス

<h5 class="version version2">Ver. 2.0</h5>

標準ライブラリ中の <code>Math</code> クラスのように、
静的なメンバーしか持たないクラスがあります。
<code>Math</code> クラスに限らず、
static メンバーのみを持ち、インスタンスの作成が不可能なクラスを作りたいことがしばしばあります。

C# 1.0 では、private なコンストラクタを持つ sealed クラスとしてこのようなクラスを作成していました。
このような方法で、「インスタンスが作成不可能」という制約は満たすことが出来ますが、
非 static なメンバーを定義することができてしまうという問題がありました。
(決してアクセスすることの出来ない無駄なメンバーになってしまいます。)

それに対して、C# 2.0 では、
クラス定義時に <code>static</code> をつけることで、
静的メンバーしか定義できないクラスを作ることが出来ます。
このようなクラスを<strong id="stclass" class="keyword">静的クラス</strong>（static class）と呼びます。

```csharp {title="静的クラスの例" highlight-ranges="1:1-1:7"}
static class MyMath
{
  // double x; というような、非 static な変数・メソッドは定義できない。

  // sin x を求める関数。
  public static double Sin(double x)
  {
    double xx = -x * x;
    double fact = 1;
    double sin = x;
    for(int i=2; i<100;)
    {
      fact *= i; ++i; fact *= i; ++i;
      x *= xx;
      sin += x / fact;
    }
    return sin;
  }
}
```


ちなみに、他のプログラミング言語には、こういう静的メンバーしか定義しない型のことを「モジュール」(module)と呼んでクラスと区別するものもあったりします。
（例えば Visual Basic なんかがそう。）


## <a id="sec-generated-title-8"></a> <a id="extension"></a>拡張メソッド

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では、（本来、前置き記法である）静的メソッドを、
インスタンスメソッドと同様に後置き記法で書くことのできる、
拡張メソッドという機能が追加されました。

すなわち、
今までなら、

```csharp {title="静的メソッド"}
int x = int.Parse("1"); // "1" よりも Parse が前
```


と書いていたものを、

```csharp {title="拡張メソッドの定義"}
static class Extensions
{
    public static int Parse(this string str)
    {
        return int.Parse(str);
    }
}
```


というような静的メソッドを用意することで、
以下のような構文で呼び出せるようになります。

```csharp {title="拡張メソッドの利用"}
int x = "1".Parse(); // Parse が後に
```


詳細は「[拡張メソッド](../functional/sp3_extension.md)」で説明します。


## <a id="sec-generated-title-9"></a> <a id="using-static"></a>using static

<h5 class="version version6">Ver. 6</h5>

名前空間(参考: 「[名前空間](../structured/sp_namespace.md)」)に対しては、「[using ディレクティブ](../structured/sp_namespace.md#using)」を使うことで、
利用側で長い名前空間を省略して書けるようになります。

C# 6 で、これと同じようなことが、静的メソッドに対してもできるようになりました。
<strong id="key-using-static" class="keyword">using static</strong> ディレクティブを書くことで、クラス名を省略して、直接静的メソッドを呼べるようになります。
例えば、Math クラス(System 名前空間)中のメソッド呼び出しであれば、以下のように書けます。

```csharp {highlight-ranges="2:1-2:25,8:22-8:29,9:27-9:29"}
using System;
using static System.Math;

class Program
{
    static void Main()
    {
        var pi = 2 * Asin(1);
        Console.WriteLine(PI == pi);
    }
}
```

ちなみに、using static は任意のクラスに対して使えます(静的クラスでないとダメとかの制限はありません)。
たとえば以下の例では、`TimeSpan`構造体や`Task`クラスを using static していますが、これらは static 修飾子がついていない普通のクラスです。

```csharp {title="static 修飾子がつかないクラスを using static"}
using System.Threading.Tasks;
using static System.Threading.Tasks.Task;
using static System.TimeSpan;

class UsingStaticNormalClass
{
    public async Task XAsync()
    {
        // TimeSpan.FromSeconds
        var sec = FromSeconds(1);

        // Task.Delay
        await Delay(sec);
    }
}
```

### <a id="sec-generated-title-10"></a> <a id="using-static-enum"></a>using staticと列挙型

列挙型のメンバーも静的なので、using staticを使って、型名を省略して参照できます。

```csharp {title="using static と列挙型"}
using static Color;

class UsingStaticEnum
{
    public void X()
    {
        // enum のメンバーも using static で参照できる
        var cyan = Blue | Green;
        var purple = Red | Blue;
        var yellow = Red | Green;
    }
}

enum Color
{
    Red = 1,
    Green = 2,
    Blue = 4,
}
```

### <a id="sec-generated-title-11"></a> <a id="using-static-extensions"></a>using staticと拡張メソッド

using static を使う場合でも、そのクラス中の[拡張メソッド](../functional/sp3_extension.md)はあくまで拡張メソッドとしてだけ使えます。
using static だけでは、拡張メソッドを普通の静的メソッドと同じ呼び方で呼べません。

```csharp {title="拡張メソッドと using static"}
using static System.Linq.Enumerable;

class UsingStaticSample
{
    public void X()
    {
        // 普通の静的メソッド
        // Enumerable.Range が呼ばれる
        var input = Range(0, 10);

        // 拡張メソッド
        // Enumerable.Select が呼ばれる
        var output1 = input.Select(x => x * x);

        // 拡張メソッドを普通の静的メソッドとして呼ぼうとすると
        // コンパイル エラー
        var output2 = Select(input, x => x * x);
    }
}
```

### <a id="sec-generated-title-12"></a>補足: 名前空間の using と違う理由

ちなみに、名前空間の using ディレクティブと区別を付けるために、「using static クラス名;」というように、static キーワードが必要です。
名前空間の using と静的クラスの using の区別がつかないと結構ひどいコードが書けてしまう問題があったので、この文法に落ち着きました。
static キーワードを付けなくてよい場合、以下のように、名前空間と同名のクラスを後から足すことで、既存のコードを壊せます。

```csharp {title="using クラス名; ではなく、using static クラス名; な理由"}
// 正式な C# 6 ではコンパイルできない
// プレビュー版のころにコンパイルできて問題になったコード

using System;
using System.Linq;

// ↑ 静的クラスの方の Linq が参照される。
// 本来の LINQ (System.Linq.Enumerable クラス内の拡張メソッド)は呼べなくなるわ、
// nameof の意味が下記の Linq クラスの nameof 静的メソッドで上書きされてしまうわ、
// ろくなことにならない。

public class Program
{
    public static void Main()
    {
        var name = nameof(Main); // 下記の System.Linq クラスの nameof 静的メソッドが呼ばれる。
        Console.WriteLine(name);
    }
}

namespace System
{
    public static class Linq
    {
        public static string nameof(Action x) => "";
    }
}
```


nameof も C# 6 で追加された新文法です。詳しくは「[特殊な文字列リテラル](../start/st_string.md)」を参照。
## <a id="exercise"></a>演習問題

### <a id="exercise-static1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体に、
2点間の距離を求める static メソッド <code>GetDistance</code> を追加せよ。

```csharp {title="GetDistance"}
/// <summary>
/// A-B 間の距離を求める。
/// </summary>
/// <param name="a">点A</param>
/// <param name="b">点B</param>
/// <returns>距離AB</returns>
public static double GetDistance(Point a, Point b)
```


また、<code>GetDistance</code> を用いて、
<code>Triangle</code> クラスに三角形の周を求めるメソッド
<code>GetPerimeter</code> を追加せよ。

```csharp {title="GetPerimeter"}
/// <summary>
/// 三角形の周の長さを求める。
/// </summary>
/// <returns>周</returns>
public double GetPerimeter()
```



#### 解答例 1


```csharp {title="Point/Triangle"}
using System;

/// <summary>
/// 2次元の点をあらわす構造体
/// </summary>
struct Point
{
  double x; // x 座標
  double y; // y 座標

  #region 初期化

  /// <summary>
  /// 座標値 (x, y) を与えて初期化。
  /// </summary>
  /// <param name="x">x 座標値</param>
  /// <param name="y">y 座標値</param>
  public Point(double x, double y)
  {
    this.x = x;
    this.y = y;
  }

  #endregion
  #region プロパティ

  /// <summary>
  /// x 座標。
  /// </summary>
  public double X
  {
    get { return this.x; }
    set { this.x = value; }
  }

  /// <summary>
  /// y 座標。
  /// </summary>
  public double Y
  {
    get { return this.y; }
    set { this.y = value; }
  }

  #endregion

  /// <summary>
  /// A-B 間の距離を求める。
  /// </summary>
  /// <param name="a">点A</param>
  /// <param name="b">点B</param>
  /// <returns>距離AB</returns>
  public static double GetDistance(Point a, Point b)
  {
    double x = a.x - b.x;
    double y = a.y - b.y;
    return Math.Sqrt(x * x + y * y);
  }

  public override string ToString()
  {
    return "(" + x + ", " + y + ")";
  }
}

/// <summary>
/// 2次元空間上の三角形をあらわす構造体
/// </summary>
class Triangle
{
  Point a;
  Point b;
  Point c;

  #region 初期化

  /// <summary>
  /// 3つの頂点の座標を与えて初期化。
  /// </summary>
  /// <param name="a">頂点A</param>
  /// <param name="b">頂点B</param>
  /// <param name="c">頂点C</param>
  public Triangle(Point a, Point b, Point c)
  {
    this.a = a;
    this.b = b;
    this.c = c;
  }

  #endregion
  #region プロパティ

  /// <summary>
  /// 頂点A。
  /// </summary>
  public Point A
  {
    get { return a; }
    set { a = value; }
  }

  /// <summary>
  /// 頂点B。
  /// </summary>
  public Point B
  {
    get { return b; }
    set { b = value; }
  }

  /// <summary>
  /// 頂点C。
  /// </summary>
  public Point C
  {
    get { return c; }
    set { c = value; }
  }

  #endregion

  /// <summary>
  /// 三角形の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public double GetArea()
  {
    double abx, aby, acx, acy;
    abx = b.X - a.X;
    aby = b.Y - a.Y;
    acx = c.X - a.X;
    acy = c.Y - a.Y;
    return 0.5 * Math.Abs(abx * acy - acx * aby);
  }

  /// <summary>
  /// 三角形の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public double GetPerimeter()
  {
    double l = Point.GetDistance(this.a, this.b);
    l += Point.GetDistance(this.a, this.c);
    l += Point.GetDistance(this.b, this.c);
    return l;
  }
}

/// <summary>
/// Class1 の概要の説明です。
/// </summary>
class Class1
{
  static void Main()
  {
    Triangle t = new Triangle(
      new Point(0, 0),
      new Point(3, 4),
      new Point(4, 3));

    Console.Write("{0}\n", t.GetArea());
    Console.Write("{0}\n", t.GetPerimeter());
  }
}
```
