---
title: "クラス"
source_url: "https://ufcpp.net/study/csharp/oop/oo_class/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2021-03-07T00:00:00"
tags: []
umbraco_id: 1250
parent_id: 1248
sort_order: 1
aliases:
  - "/study/csharp/oo_class.html"
---

# クラス

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

クラスとはオブジェクトを作るための設計図のようなもので、
オブジェクト指向プログラミングの中心となるものです。


##### <a id="sec-generated-title-2"></a>ポイント

* クラス: オブジェクトを作るための設計図。

* インスタンス: 設計図を基に作られた個々の実体。

* 例えば、
    * `class Point { public int X; public int Y; }` でクラスを作って、

    * `Point p = new Point();` でインスタンスを作る。



* 構造体との違いは「値型」か「参照型」か、継承できるかどうか。



## <a id="sec-generated-title-3"></a> <a id="class"></a>クラスとインスタンス

「[オブジェクト指向とは](oo_about.md)」で述べたように、
操作の対象となるものをオブジェクトといいます。
オブジェクトを作る場合、まず設計図が必要になります。
内部がどういう構造になっているのか、外部からどのような操作をすることが出来るのかを決めてやるわけです。
このようなオブジェクトの設計図のことを<strong id="class" class="keyword">クラス</strong>(class)といいます。
それに対し、設計図を元に作られたオブジェクトの実体のことを<strong id="instance" class="keyword">インスタンス</strong>(instance)といいます。

<table summary="クラスとインスタンスの比喩的例">
	<caption>
		クラスとインスタンスの比喩的例
	</caption>
	<tr>
		<th>クラス</th>
		<th>インスタンス</th>
	</tr>
	<tr>
		<td markdown="1">製品規格</td>
		<td markdown="1">個々の製品</td>
	</tr>
	<tr>
		<td markdown="1">人間</td>
		<td markdown="1">松井君、空知君、畑君、田辺さん・・・</td>
	</tr>
	<tr>
		<td markdown="1">実数全体<span class="math">
            <span class="bold">R</span>
          </span></td>
		<td markdown="1">実数値<span class="math">x, y,</span>・・・</td>
	</tr>
	<tr>
		<td markdown="1">初等関数</td>
		<td markdown="1"><span class="math">
            <span class="normal">sin</span>, <span class="normal">cos</span>, <span class="normal">exp</span>, <span class="normal">log</span>,
          </span>・・・</td>
	</tr>
</table>



## <a id="sec-generated-title-4"></a> <a id="definition"></a>クラス定義

C#では以下のようにしてクラスを定義します。

```csharp
class クラス名
{
  クラスの実装
}
```

クラスの実装にはメンバ変数の定義とメソッド(メンバー関数)の定義などをします。
メンバー変数とはクラスの内部で宣言される[変数](../../../../assets/st_variable.html)のことで、
メソッドの定義はクラスの内部で宣言される[関数](../../../../assets/st_function.html)のことだと思ってもらって結構です。
以下に例を示します。

```csharp
class Sample
{
  // メンバー変数の定義 ここから↓
  private int x;
  private int y;
  // メンバー変数の定義 ここまで↑

  // メソッドの定義 ここから↓
  public int GetX()
  {
    return x;
  }

  public int GetY()
  {
    return y;
  }

  public void Set(int a, int b)
  {
    x = a;
    y = b;
  }
  // メソッドの定義 ここまで↑
}
```


<code>private</code>や<code>public</code>といったキーワードについては「[実装の隠蔽](oo_conceal.md)」で解説します。

メンバー変数によってオブジェクトの内部の実装を記述し、メソッドによって外部から行える操作を記述するわけです。
以下、具体的な例を挙げるために複素数を表すクラスを作ってみましょう。
まず、複素数に対する操作には何があるかを列挙してみましょう。

* 実部の取り出し・変更

* 虚部の取り出し・変更

* 絶対値の取り出し・変更

* 偏角の取り出し・変更

* 四則演算

* 共役複素数の計算


次に、複素数を実装する方法を考えて見ましょう。

* 実部と虚部を記憶しておく

* 絶対値と偏角を記憶しておく


いきなりすべてを実装するのは大変ですから、
まず、実部と虚部の取り出し・変更と、絶対値の取り出しを、
実部と虚部を記憶しておく方式で実装してみます。

```csharp
class Complex
{
  public double re; // 実部を記憶しておく(外部からの読み出し・書き換えも可能)
  public double im; // 虚部を記憶しておく(外部からの読み出し・書き換えも可能)

  // 絶対値を取り出す
  public double Abs()
  {
    return Math.Sqrt(re*re + im*im);// Math.Sqrt は平方根を求める関数
  }
}
```


最初ということで、シンプルになるように実装しましたが、今後、徐々にちゃんとした形のものにしていきます。


## <a id="sec-generated-title-5"></a> <a id="use"></a>クラスの利用

クラスを利用するためには、
インスタンスを作成しなければなりません。
そのためにまず、インスタンスを格納するための変数を定義します。
変数定義の仕方は以下のような構文になります。

```csharp
クラス名 変数名;
```

(ちなみに、[C# 9.0 からは `new` の後ろのクラス名を省略できることがあります](oo_construct.md#target-typed-new)。)

次に、<code>new</code> キーワードでインスタンスを作成し、用意した変数に格納します。

ここで注意すべきことは、C# において、変数というのはただの入れ物であって、
変数を宣言しただけではインスタンスは作成されません。
（空っぽの入れ物だけができる。）
以下のように、<code>new</code> して始めてインスタンスが生成されます。

```csharp
変数 = new クラス名();
```

そして、以下のように変数の後に「 <code>.</code> 」で区切ってメンバー名を書くことでメンバー変数やメンバー関数を利用できます。

```csharp
変数名.メンバー名
```


例として先ほど作成した複素数クラスのインスタンスを生成し、利用してみましょう。

```csharp
Complex z;         // インスタンスを格納するための変数を定義
z = new Complex(); // new を使ってインスタンスを生成

z.re = 3;             // 実部の値を変更
z.im = 4;             // 虚部の値を変更
double abs = z.Abs(); // z の絶対値を取得

Console.Write("abs = {0}\n", abs); // abs = 5 と表示される
```


また、組込み型や配列と同様に変数の宣言と同時にインスタンスを作成して初期化することも出来ます。

```csharp
int n = 5;
string s = "abcde";
int[] array = new int[]{1, 2, 3, 4, 5};
Complex z = new Complex();
```



##### <a id="sec-generated-title-6"></a>サンプル

```csharp
using System;

/// <summary>
/// 複素数クラス
/// </summary>
class Complex
{
  public double re; // 実部
  public double im; // 虚部

  /// <summary>
  /// 絶対値を返す
  /// </summary>
  public double Abs()
  {
    return Math.Sqrt(re*re + im*im);
  }

  /// <summary>
  /// 文字列化する
  /// </summary>
  public override string ToString()
  {
    if(im >0)
      return re.ToString() + "+i" + im.ToString();
    if(im < 0)
      return re.ToString() + "-i" + (-im).ToString();
    return re.ToString();
  }
}// class Complex

//================================================
class ClassSample
{
  static void Main()
  {
    Complex z = new Complex();

    z.re = GetDouble("実部を入力してください : ");
    z.im = GetDouble("虚部を入力してください : ");

    Console.Write("|{0}| = {1}\n", z, z.Abs());
  }

  // 「関数」のところで作った実数入力用関数
  static double GetDouble(string message)
  {
    double x;
    while(true)
    {
      try
      {
        // 入力を促すメッセージを表示して、値を入力してもらう
        Console.Write(message);
        x = double.Parse(Console.ReadLine());
      }
      catch(Exception)
      {
        // 不正な入力が行われた場合の処理
        Console.Write(
          "error : 正しい値が入力されませんでした\n入力しなおしてください\n");
        continue;
      }
      break;
    }
    return x;
  }
}
```

## <a id="sec-generated-title-7"></a> <a id="null"></a>null

前節で、クラスを使う際にはまず「`new` キーワードでインスタンスを作る」と説明しましたが、
インスタンスを持たない(作るのを後回しにしたり、使い終わったものを手放したりする)場合の話もしておきます。

C# では、「有効なインスタンスを持っていない」という状態を<strong id="null" class="keyword">null</strong>（ヌル: 空っぽ、0）と呼び、`null` キーワードで表します。

```csharp
変数 = null;
```

よくある用途としては、必要になるまでインスタンス生成を遅らせたり(遅延初期化と言ったりします)です。
[プロパティ](oo_property.md)や[null 合体演算子](../resource/sp2_nullable.md#coalescing)を使うことが多く、今は「こういう書き方がある」くらいの説明しかできませんが、以下のような使い方ができます。

```csharp
using System;
using System.ComponentModel;
using System.Reflection;
 
// System.Type から、自分のプログラムで使う属性とかを抽出するためのクラス
class TypeInfo
{
    private readonly Type _type;
    public TypeInfo(Type type) => _type = type;
 
    // 初期状態で null にしておく。
    private string _description = null;
 
    // このメソッドの処理はだいぶ重たい。
    // なので必要になるぎりぎりまで呼びたくない。
    private string GetDescription() => _type.GetCustomAttribute<DescriptionAttribute>().Description;
 
    // 始めてこのプロパティが呼ばれたときに、まだ _description が null のときだけ GetDescription を呼ぶ。
    public string Description => _description ??= GetDescription();
}
```

また、「有効なインスタンスを取れなかった」ということを表すのに使ったりもします。

```csharp
class Program
{
    static void Main()
    {
        // "abcdefg" が条件を満たすのでこれが返ってくる。
        var a = FirstLongString(new[] { "a", "abcd", "abcdefg", "abc" });
 
        // 条件を満たすものがないので null が返ってくる。
        var b = FirstLongString(new[] { "a", "abcd", "abcd", "abc" });
    }
 
    // 配列中から特定の条件を満たす最初のインスタンスを探す。
    // (例として、長さ 5 以上の文字列を探す。)
    static string FirstLongString(string[] items)
    {
        foreach (var x in items)
        {
            if (x.Length >= 5) return x;
        }
 
        // 条件を満たすものがなかったことを表すために null を返す。
        return null;
    }
}
```

ちなみに、null は[参照型](../resource/oo_reference.md#reftype)の[既定値](../resource/rm_default.md)になります。

詳しくは「[null の取り扱い](../resource/rm_nullusage.md)」というページもあるのでこちらも参照してみてください。

<h5 class="version version2">Ver. 2.0</h5>

元々は参照型にしかなかった概念ですが、
C# 2.0 からは[null許容値型](../resource/sp2_nullable.md)という機能を使うことで値型でも null を使えるようになりました。

<h5 class="version version8">Ver. 8.0</h5>

また、C# 8.0 では「nullが本当に必要かどうか」を明示的に指定できるように、
[null許容参照型](../resource/nullablereferencetype.md)という機能が入りました。

## <a id="sec-generated-title-8"></a> <a id="this-access"></a>this アクセス

クラス中では、`this`というキーワードが特別な意味を持ちます。
`this`は、英単語の意味(これ、この)通り「このインスタンス自身」を表す特別な変数になります。

通常はあってもなくてもいいものなんですが、
例えば、ローカル変数と同名のフィールドがあったときに、フィールドの方を参照するために使えます。

```csharp
class Point
{
    // 小文字 x, y でフィールドを定義
    int x;
    int y;

    // 同じ x で引数を定義
    // y の方は名前を変えてみる
    public Point(int x, int a)
    {
        // this. が付いている方はフィールド
        // ついていない方は引数
        this.x = x;

        // y の方は this. を付けなくても、他に候補がないのでフィールドの y
        y = a;

        // この場合、this. を付けても y と同じ意味
        var b = this.y;
    }
}
```

あるいは、メソッドの引数に自分自身を渡したりするときに使います。

```csharp
class Point
{
    // 前略

    // Point を引数として受け取るメソッドがあったとして、
    public static void Write(Point p)
    {
        System.Console.WriteLine($"({p.x}, {p.y})");
    }

    // そのメソッドに「自分自身」を渡す
    public void Write() => Write(this);
}
```

その他、[インデクサー](oo_indexer.md)や[拡張メソッド](../functional/sp3_extension.md)など、`this`を常に必要とする構文があります。

```csharp
using System;

class Point
{
    public int X, Y;

    public void M()
    {
        var x = this[0]; // インデクサーの呼び出し
        var l = this.LengthSquared(); // 拡張メソッドの呼び出し
    }

    // インデクサー
    public int this[int i]
        => i == 0 ? X
        : i == 1 ? Y
        : throw new IndexOutOfRangeException();
}

static class PointExtensions
{
    // 拡張メソッド
    public static int LengthSquared(this Point p) => p.X * p.X + p.Y * p.Y;
}
```

## <a id="sec-generated-title-9"></a> <a id="struct"></a>クラスと構造体

ここまでの説明を見て、
「[クラス](#class)」と「[構造体](../structured/st_struct.md#struct)」の類似性に気付いた方もいるかと思います。
実際、メンバー変数やメソッドの定義は構造体でもできます。

クラスと構造体の違いを説明するためには、
継承や多態性などのオブジェクト指向の概念や、
値型と参照型というプログラミングの概念の理解が必要になります。
これらの概念の詳細は、
「[継承](oo_inherit.md)」、
「[多態性](oo_polymorphism.md)」、
「[値型と参照型](../resource/oo_reference.md)」
などで説明することにして、
ここでは簡単に概要だけを表にまとめます。

<table summary="クラスと構造体">
	<caption>
		クラスと構造体
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>クラス</th>
		<th>構造体</th>
	</tr>
	<tr>
		<th>型の分類</th>
		<td markdown="1">参照型</td>
		<td markdown="1">値型</td>
	</tr>
	<tr>
		<th>継承</th>
		<td markdown="1">できる</td>
		<td markdown="1">できない</td>
	</tr>
	<tr>
		<th>多態性</th>
		<td markdown="1">使える</td>
		<td markdown="1">使えない</td>
	</tr>
</table>


迷うようならクラスにしておけばいいと思います。
構造体は、以下の条件がそろっている場合にのみ使います。

* データのサイズが小さい（目安としては16バイト程度以下）

* 絶対に継承しないと分かっている

* 変数への代入がコピーを生むというのが許容できる



## <a id="sec-generated-title-10"></a> <a id="partial_class"></a><a id="partial"></a>クラスの分割定義

<a id="partial_method"></a>
<a id="partial_method-side-effect"></a>
<a id="extended_partial_method"></a>
<a id="contextual-partial-keyword"></a>
ページを分割しました:

* [型の分割定義 (partial)](../misc/partial-type.md)

## <a id="sec-generated-title-11"></a> <a id="anonymous"></a>匿名型

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では<strong id="anonytype" class="keyword">匿名型</strong>（anonymous type）を作成できるようになりました。
匿名型の作り方は以下の通りです。

```csharp
var x = new { FamilyName = "糸色", FirstName="望"};
```


このようなコードから、自動的に、以下のような型が生成されます。

```csharp
// ↓この __Anonymous という名前はプログラマが参照できるわけではない。
class __Anonymous1
{
  private string f1;
  private string f2;
  
  public __Anonymous1(string f1, string f2)
  {
    this.f1 = f1;
    this.f2 = f2;
  }

  public string FamilyName
  {
    get { return this.f1}
  };
  public string FirstName
  {
    get { return this.f2}
  };
  
  // あと、Equals, GetHashCode, ToString も実装
}
```


この機能的は「[LINQ](../data/sp3_linq.md#linq)」とともに利用することで真価を発揮します。
単体で使う場面はそれほど多くないと思いますが、
例えば、以下のような書き方ができます。

```csharp
var rectangle = new { Width = 2, Height = 3 };

Console.Write("幅  : {0}\n高さ: {1}\n面積: {2}\n",
  rectangle.Width,
  rectangle.Height,
  rectangle.Width * rectangle.Height);
```
## <a id="exercise"></a>演習問題

### <a id="exercise-str1"></a>問題 1


「[データの構造化](../structured/st_struct.md)」の[データの構造化](../structured/st_struct.md)の[問題 1](../structured/st_struct.md#exercise-str1)で作成した <code>Triangle</code> 構造体をクラスで作り直せ。
（<code>Point</code> 構造体は構造体のままで OK。）

注1：現時点では、
単に struct が class に変わるだけで、特にメリットはありませんが、
今後、
「[継承](oo_inherit.md)」」や「[多態性](oo_polymorphism.md)」を通して、
クラスのメリットを徐々に加えていく予定です。

注2：
クラスにした場合、メンバー変数をきちんと初期化してやらないと正しく動作しません。
（構造体でもメンバー変数の初期化はきちんとする方がいいんですが。）
初期化に関しては、次節の「[コンストラクターとデストラクター](oo_construct.md)」で説明します。
