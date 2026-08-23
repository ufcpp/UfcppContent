---
title: "プロパティ"
source_url: "https://ufcpp.net/study/csharp/oop/oo_property/"
content_type: "Article"
published_at: "2015-05-06T14:09:28"
updated_at: "2022-09-22T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 3.0"
  - "Ver. 6.0"
umbraco_id: 1255
parent_id: 1248
sort_order: 4
aliases:
  - "/study/csharp/oo_property.html"
---

# プロパティ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="property" class="keyword">プロパティ</strong>（property：所有物、特性）とは、JavaやC++にはない(Visual Basicにはある)機能で、
クラス外部から見るとメンバー変数のように振る舞い、
クラス内部から見るとメソッドのように振舞うものです。

JavaやC++がこの機能を持ってないことからも分かると思いますが、
プロパティはオブジェクト指向言語に必須の機能ではありません。
しかし、これから説明していくように、あると便利なものです。


##### <a id="sec-generated-title-2"></a>ポイント

* プロパティ: 中（実装側）からはメソッドのように扱え、外（利用側）からはメンバー変数のように見えるもの。

* 実装の隠蔽（カプセル化）の原則を崩すことなく、 アクセサー関数の煩雑さを解消。



## <a id="sec-generated-title-3"></a> <a id="about"></a>プロパティとは

「[実装の隠蔽](oo_conceal.md)」で、
メンバー変数はクラス外部から直接アクセス出来ないようにして、
オブジェクトの状態の変更はすべてメソッドを通して行うべきだと書きました。
これを忠実に実行すると、クラスを利用する側のコードは以下の例のように少々見栄えの悪いものになってしまいます。

```csharp {title="「実装の隠蔽」で作った複素数クラスその2の利用例"}
using System;

// 「実装の隠蔽」で作った複素数クラス
class Complex
{
  // 実装は外部から隠蔽(privateにしておく)
  private double re; // 実部を記憶しておく
  private double im; // 虚部を記憶しておく

  public double Re(){return this.re;}    // 実部を取り出す
  public void Re(double x){this.re = x;} // 実部を書き換え

  public double Im(){return this.im;}    // 虚部を取り出す
  public void Im(double y){this.im = y;} // 虚部を書き換え

  public double Abs(){return Math.Sqrt(re*re + im*im);}  // 絶対値を取り出す
}

// クラス利用側
class ConcealSample
{
  static void Main()
  {
    // x = 5 + 1i
    Complex x = new Complex();
    x.Re(5);  // x.re = 5
    x.Im(1);  // x.im = 1

    // y = -2 + 3i
    Complex y = new Complex();
    y.Re(-2); // y.re = -2
    y.Im( 3); // y.im =  3

    Complex z = new Complex();
    z.Re(x.Re() + y.Re()); // z.re = x.re + y.re
    z.Im(x.Im() + y.Im()); // z.im = x.im + y.im

    Console.Write("|{0} + {1}i| = {2}\n", z.Re(), z.Im(), z.Abs());
    // |3 + 4i| = 5 と表示される
  }
}
```


<code>void Re(double x)</code>、<code>double Re()</code>などの、
メンバー変数の値の取得・変更を行うためのメソッドのことを<strong id="accessor" class="keyword">アクセサー</strong>(accessor)といいます。
C++やJavaなどの言語では、下手をすると<em>メンバー変数の数だけアクセサーが存在する</em>という状態になることもあります。
C++やJavaではアクセサーのメソッド名は<code>void SetRe(double x)</code>、<code>double GetRe()</code>というように、メンバー変数名に Set/Get をつけた物を使うことが多く、<em>メンバ変数の数だけ Set/Get で始まるメソッドのペアができ、ちょっと見苦しいものになります</em>。
（参考： 「[Set / Get とプロパティ](../../miscprog/list/accessor.md)」）

また、クラス作成側からすると、オブジェクトの状態の取得・変更はすべてメソッドを通して行ったほうがいいのですが、
クラス利用側からすると、メンバー変数に値を直接代入するほうが見た目がすっきりします。

このような理由から、
C#では
<em>
        クラス内部から見るとメソッドのように振る舞い、
        クラス利用側から見るとメンバー変数のように振舞う
      </em>
プロパティという機能を用意しました。
プロパティの定義の仕方は以下のような書式になります。

```csharp
アクセスレベル 型名 プロパティ名
{
    set
    {
        // setアクセサー（setter とも言う）
        //  ここに値の変更時の処理を書く。
        //  value という名前の変数に代入された値が格納される。
    }
    get
    {
        // getアクセサー （getter とも言う）
        //  ここに値の取得時の処理を書く。
        //  メソッドの場合と同様に、値はreturnキーワードを用いて返す。
    }
}
```


set 以降のブロックに値の変更用の処理を、
get 以降のに値の取得用の処理を書きます。
これらを、set アクセサー、get アクセサーと呼びます。
あるいは、通称では <strong id="setter" class="keyword">setter</strong>、<strong id="getter" class="keyword">getter</strong> と呼んだりします。

例えば先ほどの複素数クラスのアクセサーをプロパティを使って書き換えると以下のようになります。

```csharp {title="複素数クラス その3" highlight-lines="11,23"}
using System;

// クラス定義
class Complex
{
    // 実装は外部から隠蔽(privateにしておく)
    private double re; // 実部を記憶しておく
    private double im; // 虚部を記憶しておく

    // 実部の取得・変更用のプロパティ
    public double Re
    {
        set { this.re = value; }
        get { return this.re; }
    }
    /* ↑のコードは意味的には以下のコードと同じ。
    public void SetRe(double value){this.re = value;}
    public double GetRe(){return this.re;}
    メソッドと同じ感覚で使える。
    */

    // 実部の取得・変更用のプロパティ
    public double Im
    {
        set { this.im = value; }
        get { return this.im; }
    }

    // 絶対値の取得用のプロパティ
    public double Abs
    {
        // 読み取り専用プロパティ。
        // setブロックを書かない。
        get { return Math.Sqrt(re * re + im * im); }
    }
}

// クラス利用側
class PropertySample
{
    static void Main()
    {
        Complex c = new Complex();
        c.Re = 4; // Reプロパティのsetアクセサーが呼び出される。
        c.Im = 3; // Imプロパティのsetアクセサーが呼び出される。
        Console.Write("|{0} + ", c.Re); // Reプロパティのgetアクセサーが呼び出される。
        Console.Write("{0}i| =", c.Im); // Imプロパティのgetアクセサーが呼び出される。
        Console.Write(" {0}\n", c.Abs); // Absプロパティのgetアクセサーが呼び出される。
    }
}
```


「[実装の隠蔽](oo_conceal.md)」のときと同様に、
このコードの実装方法を
「実部と虚部をメンバー変数に記憶しておく」方法から
「絶対値と偏角をメンバー変数に記憶しておく」方法に変更しても、
以下のように、クラス利用側のコードに手を加える必要は一切ありません。

```csharp {title="複素数クラスその3の実装を変更" highlight-lines="53-54"}
using System;

// クラス定義
class Complex
{
    // 実装は外部から隠蔽(privateにしておく)
    private double abs; // 絶対値を記憶しておく
    private double arg; // 偏角を記憶しておく

    // 実部の取得・変更用のプロパティ
    public double Re
    {
        set
        {
            double im = this.abs * Math.Sin(this.arg);
            this.abs = Math.Sqrt(value * value + im * im);
            this.arg = Math.Atan2(im, value);
        }
        get
        {
            return this.abs * Math.Cos(this.arg);
        }
    }

    // 実部の取得・変更用のプロパティ
    public double Im
    {
        set
        {
            double re = this.abs * Math.Cos(this.arg);
            this.abs = Math.Sqrt(value * value + re * re);
            this.arg = Math.Atan2(value, re);
        }
        get
        {
            return this.abs * Math.Sin(this.arg);
        }
    }

    // 絶対値の取得用のプロパティ
    public double Abs
    {
        get { return this.abs; }
    }
}

// クラス利用側
class PropertySample
{
    static void Main()
    {
        Complex c = new Complex();
        c.Re = 4; // クラス利用側は一切変更せず
        c.Im = 3;
        Console.Write("|{0} + ", c.Re);
        Console.Write("{0}i| =", c.Im);
        Console.Write(" {0}\n", c.Abs);
    }
}
```



## <a id="sec-generated-title-4"></a> <a id="level"></a>set/get で異なるアクセスレベルを設定

<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 の新機能で、
プロパティの set/get アクセサーそれぞれ異なるアクセスレベルを設定できるようになりました。

```csharp {title="異なるアクセスレベル" highlight-text="protected"}
class A
{
  private int n;

  public int N
  {
    get{ return this.n; }
    protected set{ this.n = value; }
  }
}
```



## <a id="sec-generated-title-5"></a> <a id="auto"></a>自動プロパティ

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では、プロパティの get/set の中身の省略もできるようになりました。
この機能を<strong id="auto_prop" class="keyword">自動プロパティ</strong>（auto-property, auto-implemented property）といいます。

例えば、

```csharp {title="プロパティの set/get の省略"}
public string Name { get; set; }
```


というように、
<code>get; set;</code> とだけ書いておくと、

```csharp {title="set/get の自動生成結果"}
private string __name;
public string Name
{
  get { return this.__name; }
  set { this.__name = value; }
}
```


というようなコードに相当するものが自動的に生成されます。
（説明のため <code>__name</code> という名前で書いていますが、
実際のコンパイル結果はプログラマが参照できない記号入りの名前で生成されます。）
ちなみに、このコンパイラーによって生成されるフィールド(この例で言うと __name)は、バッキング フィールド(baking field: 裏打ち、裏付け、後援みたいな意味)と呼ばれます。

C# プログラミングでは、
この手のコード（メンバー変数 name をプロパティ Name で覆う）は定型文的によく使います。
また、クラス内からであっても、private のメンバー変数には直接アクセスせず、
プロパティを通してアクセスする方が後々の保守がしやすかったりします。
ということで、自動プロパティのような省略記法が導入されました。

複素数の例でも、直交座標による実装のものは、以下のようにだいぶシンプルに書けるようになります。

```csharp {title="自動プロパティを使った複素数クラス定義"}
using System;

class Complex
{
    public double Re { get; set; }
    public double Im { get; set; }

    public double Abs
    {
        get { return Math.Sqrt(Re * Re + Im * Im); }
    }
}
```


ちなみに、元々 C# 2.0 以前でも、
「プロパティの「[デリゲート](../functional/sp_delegate.md#delegate)」版」にあたる「[イベント](../functional/sp_event.md#event)」では自動プロパティを同じような省略が可能でした。
（デリゲート、イベントについては後述。
参考： 「[デリゲート](../functional/sp_delegate.md)」、「[イベント](../functional/sp_event.md)」。）
その省略機能がプロパティにも実装されたということになります。


## <a id="sec-generated-title-6"></a> <a id="get-only"></a>get-only プロパティ

<h5 class="version version6">Ver. 6</h5>

C# 6 では、get アクセサーだけのプロパティを定義できるようになりました。
この場合、コンストラクターでだけ値を代入できて、以降は書き換え不能になります。

```csharp {title="get-only なプロパティ" highlight-ranges="sha256:5cce1aa043a02d3e9541a1f127de137a61e7026b744d974525cab4159ce39090;5:24-5:28,6:24-6:28"}
using System;

class Complex
{
    public double Re { get; }
    public double Im { get; }

    public Complex(double re, double im)
    {
        // コンストラクター内でだけ代入可能。
        Re = re;
        Im = im;
    }
}
```

このように `get` アクセサーのみを持つプロパティは通称 <strong id="key-get-only" class="keyword">get-only プロパティ</strong>(get-only property)と呼ばれています。

「コンストラクターでだけ値を代入できる」という挙動は [readonly フィールド](../start/sp_const.md#readonly)と同じです。
実際、上記の get-only プロパティからは以下のように、readonly なバッキング フィールドが作られます。

```csharp {title="get-only なプロパティから生成されるコード"}
using System;

class Complex
{
    public double Re { get { return _re; } }
    private readonly double _re;
    public double Im { get { return _im; } }
    private readonly double _im;

    public Complex(double re, double im)
    {
        // コンストラクター内でだけ代入可能。
        _re = re;
        _im = im;
    }
}
```

## <a id="sec-generated-title-7"></a> <a id="property-initializer"></a>プロパティ初期化子

<h5 class="version version6">Ver. 6</h5>

同じくC# 6.0から、自動プロパティに対して初期化子を与えられるようになりました。

```csharp {highlight-ranges="sha256:9722a7311933e59eb28a0f30572455e368db90523deced1a0d84ebaacd9e95e0;3:31-3:37,4:32-4:37"}
class Point
{
    public int X { get; set; } = 10;
    public int Y { get; set; } = 20;
}
```

これで、コンストラクターを書かなくてもプロパティに対して初期値を与えることができます。

## <a id="sec-generated-title-8"></a> <a id="expression-bodied"></a>expression-bodied なプロパティ

get-only のプロパティに限りますが、他のいくつかの関数メンバーと同様に、expression-bodied (本体が式の)形式でプロパティを定義できます。
(参考: 「[expression-bodied な関数](../structured/st_function.md#sec-expression-bodied)」)

先ほどから例に挙げている複素数クラスでいうと、Abs プロパティの定義が楽になります。

```csharp {highlight-lines="8"}
using static System.Math;

class Complex
{
    public double Re { get; set; }
    public double Im { get; set; }

    public double Abs => Sqrt(Re * Re + Im * Im);
}
```



## <a id="sec-generated-title-9"></a> <a id="indexed"></a>余談: C# にインデックス付きプロパティはありません

VB にはある「インデックス付きプロパティ」は、C# にはありません。
C# の流儀的には、「インデックス付きプロパティ」よりも、「コレクションクラスを返す普通のプロパティ」推奨です。
（その方が、foreach が使えたり、色々便利だから。）

```csharp {title="ダメな例： インデックス付きプロパティ"}
int[] x;
// ↓これは文法違反。
public int X[int i]
{
    get { return x[i]; }
    private set { x[i] = value; }
}
```


```csharp {title="一応、可能： 配列を返すプロパティ"}
int[] x;
// ↓これなら OK。
public int[] X
{
    get { return x; }
}
```


C# 2.0 や C# 3.0 を見こすなら、以下のように、配列や ICollection ではなく、IEnumerable を返すようにする方がいいかもしれません。
（詳細は「[イテレーター](../data/sp2_iterator.md)」参照。）

```csharp {title="C# 2.0 的には： イテレーターを使って IEnumerable で返す"}
int[] x;
public IEnumerable<int> X
{
    get { foreach (var item in x) yield return item; }
}
```


ちなみに、VB にはあることからわかるように、.NET 的にはインデックス付きプロパティを認めています。
C# から呼び出す場合は、get_*** というような名前のメソッド呼び出しになります。
例えば、VB で X と言う名前で、int を引数にとるインデックス付きプロパティを定義した場合、
C# からは get_X(0) というように呼び出します。

さらに特殊事情として、対 COM の場合だけ、普通に X[0] というような呼び出し方が認められます。
詳しくは「[COM 相互運用時の特別処理](../interop/sp4_cominterop.md)」を参照。

## <a id="sec-generated-title-10"></a> <a id="init-only"></a>init-only プロパティ

<h5 class="version version9">Ver. 9</h5>

C# 9.0 では、`set` に代わって、`init` という名前のアクセサーを定義できるようになりました。
例えば以下のように書けます(ちなみに、`set` と `init` は同時には書けません。排他です)。

```csharp {title="init アクセサー" highlight-ranges="sha256:636c732609e9ab5ea4adda8c122b8b072f7ad0e1d15994242948a6bb10807115;3:29-3:33,4:29-4:33"}
class Complex
{
    public double Re { get; init; }
    public double Im { get; init; }
}
```

`init` アクセサーを持っているプロパティは通称 <strong id="key-get-only" class="keyword">init-only プロパティ</strong>(init-only property)と呼ばれます。

用途としては [get-only プロパティ](#get-only) や [`readonly` フィールド](../start/sp_const.md#readonly)とほとんど同じです。
ただ、`readonly` の制限が厳しすぎるので、問題ない範囲でちょっとだけ制限を緩めたものが `init` アクセサーです。
(歴史的経緯で `init` という新キーワードが使われていますが、もし C# をフルスクラッチで作り直せるなら `readonly` が最初から `init` 相当の仕様になっていたと思います。)

まず、`readonly` と同じ点として、コンストラクター内での書き換えはできます。

```csharp {title="init はコンストラクター内から書き換え可能"}
class Complex
{
    public double Re { get; init; }
    public double Im { get; init; }
 
    public Complex(double re, double im)
    {
        // この2行は OK。
        Re = re;
        Im = im;
    }
}
```

一方、`readonly` では認められてないことで、`init` であればできることが3つあります。

- [オブジェクト初期化子](oo_construct.md#member_initializer)での書き換え
- 他の `init` アクセサー内での書き換え
- [`with` 式での書き換え](../datatype/record.md#with)

例えば、以下のコード(get-only プロパティを利用)はコンパイルできませんが、

```csharp {title="get-only プロパティはオブジェクト初期化子を使えない"}
var p = new Point { X = 1, Y = 2 };
 
class Point
{
    public int X { get; }
    public int Y { get; }
}
```

以下のように init-only プロパティに書き換えるとコンパイルできます。

```csharp {title="init-only プロパティならオブジェクト初期化子を使える" highlight-ranges="sha256:b341cabc7933534ba4e5870561633ae577dad42607ce5d1394a2160830041325;5:25-5:29,6:25-6:29"}
var p = new Point { X = 1, Y = 2 };
 
class Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
```

初期化子の外で書き換えようとすると、`readonly`と同じくコンパイル エラーになります。

```csharp
var p = new Point { X = 1, Y = 2 };
p.X = 3; // ダメ。
```

`with` 式については別途解説予定(トラッキング issue: [C# 9.0](https://github.com/ufcpp/UfcppSample/issues/297))ですが、
例えば以下のようなコードが書けます。

```csharp {title="with 式で init-only プロパティを書き換え"}
var p0 = new Point(1, 2);
var p1 = p0 with { X = 3 }; // p0 のクローンを作った上で、X だけ 3 で上書き。
 
record Point(int X, int Y);
```

他の `init` アクセサーからの書き換えは、例えば以下のようなコードを書けます。

```csharp {title="他の init アクセサーからの書き換え"}
using System;
 
var x = new Squared { ValueSquared = 4 };
Console.WriteLine(x.Value); // 2
 
class Squared
{
    public double Value { get; init; }
 
    public double ValueSquared
    {
        get => Value * Value;
        init => Value = Math.Sqrt(value);
    }
}
```

ちなみに、`init` アクセサー内では `readonly` フィールドも書き換え可能です。

```csharp {title="init アクセサー内で readonly フィールドを書き換え"}
class Squared
{
    public readonly double Value;
 
    public double ValueSquared
    {
        get => Value * Value;
        init => Value = Math.Sqrt(value); // OK。
    }
}
```

### <a id="sec-generated-title-11"></a> <a id="init-only-internal">init-only プロパティの中身</a>

ちなみに、init-only プロパティコンパイル結果としては単に `public` な `set` アクセサーと `readonly` フィールドになっています。
C# コンパイラーのレベルで「初期化子など以外からの書き換えを禁止する」というような解析をしています。

この解析に対応していない古い C# コンパイラーから `set` を呼ばれるとかなりまずい(本来書き換えられないはずの `readonly` フィールドが書き換わる)ので、それを禁止するために modreq という修飾機能を使っています。

modreq については別途説明予定です。トラッキング issue:

- [新機能の実装方法(modreq + RuntimeFeature)](https://github.com/ufcpp/UfcppSample/issues/295)
- [modreq って何？](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/4)


<!-- original-page-break -->

## <a id="sec-generated-title-12"></a> <a id="required"></a>required メンバー

<h5 class="version version11">Ver. 11</h5>

C# 11 でプロパティとフィールドに対する `required` 修飾子というものが追加されました。
これを使うと、[オブジェクト初期化子](oo_construct.md#member_initializer)で何らかの値を代入することを義務付けられます。
例えば以下のようなコードを書いたとき、`a1` 以外の `new A` はエラーになります。
(警告ではなくエラーにします。)

```csharp {title="required 修飾子" highlight-ranges="sha256:b2ab16f5c01f49b3579d37408c360d5558ab23736f76afb1e0feede18550d81f;9:12-9:20,10:12-10:20"}
var a1 = new A { X = "abc", Y = 123 };

var a2 = new A { X = "abc" }; // Y を代入していないのでエラー。
var a3 = new A { Y = 123 };   // X を代入していないのでエラー。
var a4 = new A();             // X も Y も代入していないのでエラー。

class A
{
    public required string X { get; init; }
    public required int Y;
}
```

この機能を指して、<strong id="key-required" class="keyword">required メンバー</strong> (required members)と言います。

### <a id="sec-generated-title-13"></a> <a id="required-needs">required の必要性</a>

C# のオブジェクトの初期化には以下の2種類の構文があります。

* `new A(x, y)`: コンストラクターに引数で値を与える
    * 引数を並べる順序に意味があって、渡す先に仮引数名は指定しないので「位置指定」(positional)初期化と呼ぶ
* `new A { X = x, Y = y }`: オブジェクト初期化子でプロパティに値を与える
    * 順序に意味がなくて、プロパティ名は指定するので「名前指定」(nominal)初期化と呼ぶ

元々の C# にはコンストラクター(位置指定初期化)しかなかったのに対して、C# 3 でオブジェクト初期化子が導入されて名前指定初期化ができるようになりました。
C# 3 当時は名前指定初期化という考え方もなくて、あくまでコンストラクターの補助的な立ち位置でしたが、今となってはコンストラクターと対を成すような扱いを受けています。

クラスを作っている側で手間を惜しまないのであれば、普通にコンストラクターがある方が、使う側にとっては便利なことが多かったりします。
ただ、作る側の面倒は結構多いです。

まず、単にコンストラクターが増えるだけで手間。
よく言われる話ですが、プロパティ1個に対して同じような文字列を4回は繰り返す必要が出ます。

```csharp {title="コンストラクターを用意する手間"}
var a = new A("abc", 123); // 使う側は簡潔。

class A
{
    public string X { get; } // ここに X を書いて
    public int Y { get; }

    public A(string x, int y) // ここにも x
    {
        X = x; // ここに至っては2個の X
        Y = y;
    }
}
```

さらに、このクラス `A` を継承して、もう1個 `Z` プロパティを持った型 `B` を作ることを考えます。
以下のように、さらに追加で2か所同じ文字列を追加する必要があります。

```csharp {title="継承するとさらにかかる手間"}
class A
{
    // A の中身はさっきと一緒。
}

// 派生クラスで1プロパティ増やしたくなった時
class B : A
{
    public bool Z { get; }

    public B(string x, int y, bool z) // さらにここと、
        : base(x, y) // ここにも x が必要。
    {
        Z = z;
    }
}
```

これに対して、名前指定初期化の場合はプロパティだけ書けばいいのでずいぶんと楽です。

```csharp {title="名前指定初期化はクラス定義側が楽"}
// 使う側は多少長いものの、名前を明示してる分読みやすいかも。
var a = new B
{
    X = "abc",
    Y = 123,
    Z = true,
};

// クラス定義側は簡素に。
class A
{
    public string X { get; init; }
    public int Y { get; init; }
}

class B : A
{
    public bool Z { get; init; }
}
```

ところがこれには1つ問題があります。
このコードの例で、`X` プロパティのところに警告(CS8618)が出てしまっています。
この警告は [null 許容参照型](../resource/nullablereferencetype.md)を有効化してるときにだけ発生するんですが、要するに、
「`X` の型は (非 null な) `string` なのに、有効な初期値を与えていない」というものです。
非 null な以上、何も値を与えない(勝手に null に初期化される)わけにはいきません。

そこで `required` が導入されました。
「名前指定にはしたいけど、明示的な初期化も義務付けたい」という要件です。

```csharp {title="名前指定にはしたいけど、明示的な初期化も義務付けたいときには required"}
var a = new A
{
    X = "abc", // 非 null に初期化される保証がこの行でできる.
    Y = 123,
};

// 明示的な初期化を義務付けたいプロパティ/フィールドには required を付ける。
// これを使えば null 許容参照型での問題も回避可能。
class A
{
    public required string X { get; init; }
    public required int Y { get; init; }
}
```

ちなみに、null 許容参照型は「わかりやすい需要の例」ではありますが、
別にその他の場面でも `required` は使えます。
とにかく「初期化を明示させたい」というものなので、値型や null 許容型でも使えます。

```csharp {title="とにかく「初期化を明示させたい」"}
// 全部 0 か null なので、別に new A() でも結果は同じものの、明示させたいという意図があるなら required。
var a1 = new A { X = null, Y = 0, Z = null };

var a2 = new A { X = null, Y = 0 }; // Z がないのでエラー。

class A
{
    // default 値(0 や null)でもいいけども、とにかく明示はさせたい。
    public required string? X { get; init; }
    public required int Y { get; init; }
    public required int? Z { get; init; }
}
```

### <a id="sec-generated-title-14"></a> <a id="applicable">required の適用範囲</a>

`required` は、`virtual` や `abstract` なプロパティに対しても使えます。
ただし、基底クラス側が `required` なものは派生クラス側にも `required` を付ける必要があります。

```csharp {title="派生と required"}
abstract class A
{
    public required abstract int X { get; init; }
    public required virtual int Y { get; init; }
    public virtual int Z { get; init; }
}

class B : A
{
    // 基底クラス側が required なら、こっちも required でないとダメ。
    public override required int X { get; init; }

    // 逆は大丈夫。基底クラスになくても、派生クラス側だけ required を足すことはできる。
    public override required int Z { get; init; }
}

class C : A
{
    // 派生側で required を取ってしまうとコンパイル エラー。
    public override int X { get; init; }
}
```

そして、`required` はオブジェクト初期化で使うことが前提なので、
`new` できないインターフェイスに対しては使えません。

```csharp {title="インターフェイスには required を使えない"}
interface I
{
    // エラー。
    required int X { get; init; }
}
```

また、オブジェクト初期化子で値を渡せるように、
プロパティ/フィールドのアクセシビリティは、それを含む型よりも広い必要があります。
例えば、`internal` クラスの `internal` プロパティには使えますが、
`public` クラスの `protected` プロパティには使えません。

```csharp {title="required メンバーのアクセシビリティの制限"}
internal class A
{
    // internal クラスの internal プロパティなので OK。
    internal required int X { get; init; }
}

public class B
{
    // public 未満のアクセシビリティは全部不可。以下は全部エラー。
    protected required int X1 { get; init; }
    internal required int X2 { get; init; }
    internal protected required int X3 { get; init; }
    protected private required int X4 { get; init; }
    private required int X5;
}
```

### <a id="sec-generated-title-15"></a> <a id="SetsRequiredMembers">SetsRequiredMembers</a>

`required` メンバーをコンストラクター内で初期化するのであれば、
呼び出し元のオブジェクト初期化子では必ずしも初期化の必要がない場合があります。
こういう場合にエラーを出されても困るので、
`SetsRequiredMembers` という属性(`System.Diagnostics.CodeAnalysis` 名前空間)を使って「このコンストラクターを呼んだ場合は `required` メンバーの初期化をする必要はない」
という指定もできます。

```csharp {title="SetsRequiredMembers 属性の例"}
using System.Diagnostics.CodeAnalysis;

// required メンバーは A() (引数なしコンストラクター)で初期化するので、
// この場合は { X = "" } とかがなくてもエラーにならない。
var a = new A();

class A
{
    public required string X { get; init; }
    public int Y { get; init; }

    [SetsRequiredMembers]
    public A()
    {
        X = "abc";
        Y = 123;
    }
}
```

ただ、この `SetsRequiredMembers` は、利用側(呼び出した側)のエラーはなくしてくれる一方で、
作る側(コンストラクターの実装側)では特に何もしてくれません。
単にエラーを消します。

```csharp {title="自称 SetsRequiredMembers"}
using System.Diagnostics.CodeAnalysis;

// 自称 SetsRequiredMembers を信じてエラーは出さない。
var a = new A();

Console.WriteLine(a.X); // null

class A
{
    public required string X { get; init; }
    public int Y { get; init; }

    [SetsRequiredMembers]
    public A()
    {
        // 「requierd メンバーをセットする」と自称しているくせに、実際は何もしない。
        // X に関しては nullability のフロー解析で、null 許容参照型警告が出るけども、全くの別件。
        // Y に関しては一切何もチェックが働かない。
        // 少なくとも C# 11 リリース時点では「仕様」(問題はわかっているものの、実装が大変なので妥協)。
        // 現状の SetsRequiredMembers は「使う側はコンパイラーが守るけど、作る側は自分で頑張って」という姿勢。
    }
}
```

### <a id="sec-generated-title-16"></a> <a id="required-internal">required メンバーの中身</a>

required メンバーを含む型は、内部的には属性を付けて表現しているようです。
例えば、以下のようなクラスがあったとします。

```csharp {title="シンプルな required メンバーの例"}
class A
{
    public required int X { get; init; }
}
```

これをコンパイルすると、以下のようなコードに展開されます。

```csharp {title="上記の例の展開結果"}
using System.Runtime.CompilerServices;

[RequiredMember]
class A
{
    [RequiredMember]
    public int X { get; init; }

    [Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
    [CompilerFeatureRequired("RequiredMembers")]
    public A() { }
}
```

型と、required メンバー自体には `RequiredMember` 属性(`System.Runtime.CompilerServices` 名前空間)が付いていて、これで required かどうかを判断しています。

そして、引数なしコンストラクターが追加されて、
そこに `Obsolete` と `CompilerFeatureRequired` 属性が付きます。
これらは required メンバーに未対応の古いコンパイラーでこのクラスを使ったときにエラーにするための属性です。
これは本来どちらか片方でいいんですが、それぞれ以下のような用途です。

* 既存の仕組みでエラーにできるように `Obsolete` 属性を付けている
    * required メンバーに対応しているコンパイラーの場合、「所定のメッセージの場合は無視してエラーにしない」みたいな特殊対応をしている
* `Obsolete` による対処は気持ち悪いので、「未対応ならエラー」のために新しい `CompilerFeatureRequired` 属性を作った
    * こちらは素直に、`featureName` 引数に与えた文字列を見て対応できるかどうかを判定
    * `CompilerFeatureRequired` に対応していないコンパイラーのサポートが切れるくらいの頃に `Obsolete` は消したい

[`init` の場合](#init-only-internal)とは違って、modreq (属性よりも強い制約でコンパイル エラーにできる機構)は使わない方針です。
以下のような状況を考えると、制約が強い modreq は使いにくいそうです。
(不意に、コンパイラーが裏で勝手に作るコンストラクターが増えることがある。
不意に増えるものに使うには modreq は強すぎる。)

```csharp
using System.Diagnostics.CodeAnalysis;

class A
{
    public required int X { get; init; }

    // SetsRequiredMembers なコンストラクターを明示。
    // この場合、Obsolete, CompilerFeatureRequired 付きのコンストラクターはコンパイラー生成されない。
    // もし、このコンストラクターを消すと…
    // コンパイラーが裏で Obsolete, CompilerFeatureRequired 付きを作ってしまう。
    [SetsRequiredMembers]
    public A() { }
}
```

## <a id="sec-generated-title-17"></a> <a id="field-keyword">field キーワード</a>

<h5 class="version version14">Ver. 14</h5>

[自動プロパティ](#auto)ではバッキング フィールドへの値の素通しが行われます。
これに対して、ちょこっとだけ実装をいじりたいことが結構あります。
特によくあるのが「バッキング フィールドの生成は自動でやってほしいけど、`get`/`set` の中身は自分で書きたい」という状況で、例えば下のような例があります。

```csharp {title="惜しくも自動にならないプロパティ"}
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

class FieldBackedProperties : INotifyPropertyChanged
{
    // 遅延初期化: 最初のアクセス時にインスタンスを生成。
    private string? _x;
    public string X => _x ??= "";

    // set 側だけ null 許容(get 側で ?? で非 null 化)。
    private string? _y;

    [AllowNull]
    public string Y
    {
        get => _y ?? "";
        set => _y = value;
    }

    // INotifyPropertyChanged の実装: get 側だけ素通し。
    private string? _z;

    public string? Z
    {
        get => _z;
        set
        {
            if (_x != value)
            {
                _z = value;
                PropertyChanged?.Invoke(this, new(nameof(Z)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
```

これに対して C# 14 では、 `field` キーワードというものを追加しました。
プロパティの `get`/`set` の中に `field` と書くと、
バッキング フィールドを生成した上で、そのフィールドの読み書きができます。
例えば前述の例を `field` を使って書き直すと以下のようになります。

```csharp {title="field キーワードを使って書き直し"}
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

class FieldBackedProperties : INotifyPropertyChanged
{
    // 遅延初期化: 最初のプロパティ アクセス時にインスタンスを生成。
    public string X => field ??= "";

    // set 側だけ null 許容(get 側で ?? で非 null 化)。
    [AllowNull]
    public string Y
    {
        get => field ?? "";
        set;
    }

    // INotifyPropertyChanged の実装: get 側だけ素通し。
    public string? Z
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                PropertyChanged?.Invoke(this, new(nameof(Z)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
```

`field` キーワードには以下のようなメリットがあります。

* 重複を避けれる
  * この例の場合は `_x` みたいな短い名前なものの、プロパティ名はもっと長いことが多いので繰り返したくない
  * プロパティの型も、型名が長いことが多々ある
* 他のプロパティから参照されるのを避けれる
  * ほとんどの場合「`_x` は `X` 内でしか使わない」みたいなことになるのに、`_x` が他のメソッドやプロパティから見えてしまっていた

ちなみに(この例で既に使っていますが)、自動実装(空っぽの `get`/`set`)との併用もできます。
`get;` は `get => field;` と、
`set;` は `set => field = value;` と同じ意味になります。

### <a id="sec-generated-title-18"></a> <a id="field-backed-property">自動プロパティとの共通点</a>

既存の自動プロパティと、 C# 14 で追加された `field` キーワードを使ったプロパティは
「バッキング フィールドが自動生成される」という意味で共通しているわけですが、
これらを合わせて field-baked プロパティ(フィールドで裏付けされたプロパティ)と呼びます。
ひとくくりにする言葉が用意されているくらいにはこの2つは扱われ方が似ています。

以下は一例ですが、「`get` だけ書くと [get-only プロパティ](#get-only)になる」という挙動は完全に一致します。

```csharp {title="field-backed プロパティの get-only 化"}
class GetOnly
{
    // 元々ある get-only プロパティ。
    public int X { get; }

    // get => field; と get; は全く同じ意味で、これも get-only プロパティになる。
    public int Y { get => field; }

    // 何ならこれも get => field; の省略形なので get-only プロパティになる。
    public int Z => field;

    // 中身をカスタマイズしても、field キーワードを使っている時点で get-only プロパティ。
    public int W => field + 1;

    public GetOnly(int x, int y, int z, int w)
    {
        // なので set; を省略していても、コンストラクター内に限り値の代入が可能。
        // (バッキング フィールドへの直代入扱い。)
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
}
```

他の例として、`ref` 付きのバッキング フィールドは作れないという制限も共通です。

```csharp {title="ref 付きのプロパティは field-backed プロパティにできない"}
ref struct RefField
{
    // ref 付きのプロパティは自動実装にできない。
    public ref int X { get; }

    // 同じく field キーワードは使えない。
    public ref int Y => ref field;

    // 参考: これなら書ける。(警告は別件。)
    private ref int _z;
    public ref int Z => ref _z;
}
```

### <a id="sec-generated-title-19"></a> <a id="field-contextual-keyword">文脈キーワード</a>

`field` 「キーワード」とは言っていますが、
他の例にもれず `field` は[文脈キーワード](../misc/ap_compatibility.md#contextual-keyword)です。
プロパティの `get`/`set` 内でだけキーワード扱いされます。

```csharp {title="field は文脈キーワード"}
class A
{
    // これは普通にフィールド。
    private int field;

    public int M()
    {
        // これは普通にローカル変数。
        var field = 123;
        return field;
    }

    // これは文脈キーワードの field。
    // (ちなみにこの例では「同名のフィールドがあるけど大丈夫？」と警告される。)
    public int X => field;
}

// これも警告は出るものの合法。普通に型名。
// (「小文字アルファベット始まるの型名は将来の文脈キーワードと被る可能性が高いからやめてほしい」という警告。)
class field;

class B
{
    // こんなのすら合法。
    public field field(field field) => field;
}
```

この例のような「`field` という名前のフィールド」は元々書けていたわけで、
`field` キーワードの追加はたとえ文脈キーワードだとしても破壊的変更です。
以下のコードは C# 13 と 14 で解釈が異なります。

```csharp {title="field キーワードの追加は破壊的変更"}
class A
{
    private int field;

    // C# 13: field フィールドを参照。
    // C# 14: X のバッキング フィールドが自動生成されて、それを参照。
    //        (field フィールドとは別のフィールドが生成される。)
    public int X => field;

    // 以前の挙動を得るためには:

    // @ を付けるとキーワードではなくなる。この名前のフィールドを参照。
    public int Y => @field;

    // this. を付けてもフィールド参照にできる。
    public int Z => this.field;
}
```

### <a id="sec-generated-title-20"></a> <a id="field-keyword-initializer">プロパティ初期化子</a>

プロパティ初期化子を使う場合ちょっと注意が必要になります。
初期化子で値を渡す場合、プロパティの `set` アクセサー呼び出しではなく、バッキング フィールドへの直代入になります。

```csharp {title="プロパティ初期化子では set が呼ばれない"}
var x = new PropertyInitializer(10);

// x.X は 10 になる。
// set が呼ばれていなくて、バッキング フィールドに直接 10 が渡る。
Console.WriteLine(x.X);

class PropertyInitializer(int x)
{
    public int X
    {
        get;
        set => field = value + 1; // 値を1ずらす
    } = x;
}
```

コンストラクターの場合はこんなことはなくて、ちゃんと `set` アクセサーが呼ばれます。

```csharp {title="コンストラクター内で初期化するとちゃんと set が呼ばれる"}
var x = new Constructor(10);

// x.X は 11 になる。
// ちゃんと set 経由でバッキング フィールドの初期化が行われる。
Console.WriteLine(x.X);

class Constructor
{
    public int X
    {
        get;
        set => field = value + 1; // 値を1ずらす
    }

    public Constructor(int x)
    {
        X = x; // この場合は set アクセサーが呼ばれる。
    }
}
```

変な挙動ではありますが、これは初期化子やコンストラクターの実行順序に関係しています。
「[コンストラクター](oo_construct.md#initializer-order)」や
「[継承](oo_inherit.md#ctor)」で説明していますが、フィールド初期化子やプロパティ初期化子でインスタンス メソッドを呼べてしまうと、未初期化のフィールドを読んでしまう可能性があります。
プロパティのアクセサーの実態はメソッドとほぼ同じなので同様の問題があり得て、
初期化子で `set` アクセサーは呼んではいけないということになります。
そのため仕方なく、プロパティ初期化子ではフィールドへの直代入する仕様になっています。

### <a id="sec-generated-title-21"></a> <a id="backing-field-nullability">バッキング フィールドの null 許容性</a>

プロパティが参照型のとき、そのバッキング フィールドの [null 許容性](../resource/nullablereferencetype.md)はどうあるべきでしょうか？
本節冒頭の例でも挙げたように、`field` キーワードの用途の1つに遅延初期化があります。
この場合、「`T` 型のプロパティのバッキング フィールドは `T?` の方が都合がいい」ということになります。

```csharp {title="T 型の遅延初期化では T? が都合がいい"}
class LazyInit
{
    // field は string? でも大丈夫。
    // 一方で、field が string だとすると「コンストラクターで非 null に初期化しろ」警告が出るはず。
    // つまり、field は string? の方が都合がいい。
    public string X => field ?? "";
}
```

かといって常に `T?` にすればいいというものでもなく、`T` でないとまずい場合もあります。 
ちょっと複雑な例ですが、以下のコードを見てください。

```csharp {title="string プロパティのバッキング フィールドは string か string? か"}
using System.Diagnostics.CodeAnalysis;

class AllowNullSetter
{
    // AllowNull を付けると set 側だけ nullable になる。
    // obj.X = null; を渡せて、でも、var x = obj.X; は null にならない。

    // フィールドは string? であってほしい例: 
    [AllowNull]
    public string X
    {
        get => field ?? ""; // こっちで非 null を保証。
        set => field = value;
    }

    // フィールドは string であってほしい例: 
    [AllowNull]
    public string Y
    {
        get => field;
        set => field = value ?? ""; // こっちで非 null を保証。
    } = "";
}
```

これをコンパイラーが正しく判断できるように、`get`/`set` 両方合わせてフロー解析する仕様になっています
(通常、null 許容性のフロー解析は2つ以上のメソッドをまたいで行いません。
`get`/`set` の中身はそれぞれ独立したメソッドなので、ここだけの特殊処理になります)。
`get` 側で `field` が `T?` だと思ってフロー解析してみて警告にならなかった場合、
`set` 側も `field` が `T?` かもしれない前提でフロー解析します。

```csharp {title="get の解析結果を踏まえて set をフロー解析"}
class Nullability
{
    public string X
    {
        get => field ?? ""; // field は string? でも問題ない。
        set
        {
            // string? 扱いでフロー解析。
            string x = field; // ここで警告。
        }
    }

    public string Y // ここに「非 null 初期化しろ」警告が出る。
    {
        get => field; // field は string でないとおかしい。
        set
        {
            // string 扱いでフロー解析。
            string x = field; // 警告なし。
        }
    }

    public string Z
    {
        get => field ?? "";
        set
        {
            // string? 扱いでフロー解析するとしても、
            // value が string なのでここより後ろでは field は非 null。
            field = value;
            string x = field; // 警告なし。
        }
    }

    public string W
    {
        set
        {
            // ちなみに get を省略すると field は string? 扱いになる。
            string x = field; // ここで警告。
        }
    }
}
```

ちなみにこの挙動はあくまで [null 許容参照型](../resource/nullablereferencetype.md)に対するものです。
[null 許容値型](../resource/sp2_nullable.md)の場合は「`T` 型プロパティのバッキング フィールドは常に `T`」になります。
`int X => field ??= 1;` などと書くとエラー(`field` は `int?` にはならず `int`。`int` に対して `??` は使えない)になります。
## <a id="exercise"></a>演習問題

### <a id="exercise-prop1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスの各メンバー変数に対して、
プロパティを使って実装の隠蔽を行え。


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
  }
}
```
