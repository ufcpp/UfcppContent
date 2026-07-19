---
title: "演算子のオーバーロード"
source_url: "https://ufcpp.net/study/csharp/oop/oo_operator/"
content_type: "Article"
published_at: "2015-05-06T14:09:35"
updated_at: "2017-11-04T00:00:00"
tags: []
umbraco_id: 1259
parent_id: 1248
sort_order: 7
aliases:
  - "/csharp/oo_operator"
  - "/csharp/oo_operator.html"
  - "/csharp/oop/oo_operator/"
  - "/study/csharp/oo_operator"
  - "/study/csharp/oo_operator.html"
---

# 演算子のオーバーロード

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

オブジェクト指向言語ではクラスを定義することで自分の思い通りの「型」を作ることが出来ます。
このような自作の型は、<code>int</code> や <code>double</code> などの組込み型と区別するため、
<strong id="udt" class="keyword">ユーザー定義型</strong>と呼ばれています。
ユーザー定義型の理想は、組込み型とまったく同じように扱えることです。

ユーザー定義型をあたかも組込み型であるかのように扱えるようにするため、
C#には<strong id="opoverload" class="keyword">演算子のオーバーロード</strong>というものが用意されています。
C#の組込み型には <code>+</code> や <code>-</code> などの演算子が用意されていますが、
演算子のオーバーロードを行うことで、
ユーザー定義型にも自分で演算子を定義することが出来、
組込み型と同じように扱うことができます。

このように、演算子のオーバーロードによってユーザ定義型に追加された演算子のことを<strong id="udo" class="keyword">ユーザ定義演算子</strong>と呼びます。


##### <a id="sec-generated-title-2"></a>ポイント

* 組み込み型（int や string など）とユーザー定義型（クラスや構造体）の区別をなくそう。

* ユーザー定義型にも、組み込み型と同じように<code>+</code>や<code>-</code>などの演算子が定義できます。

* 書き方は、T operator+ (T x, T y) { ... }



### <a id="sec-generated-title-3"></a> <a id="shouldnot"></a>注意: 乱用は非推奨

演算子のオーバーロードの最大の目的は、ユーザー定義型と組み込み型の差をなくすことです。

逆に言うと、オーバーロードした演算子は、組み込み型と似たような挙動をすべきです。
<code>+</code> 演算子なら加算、
<code>&gt;</code> 演算子なら大なり比較というように、
元の意味と同じ、あるいは、少なくとも似ている操作であるべきです。
この範囲を超えての乱用は避けるべきでしょう。

このように考えると、演算子のオーバーロードが有用な場面は限られます。
かろうじて、<code>+</code> 演算子は文字列やデリゲートなど、結合にも使われるので用途も広がります。
しかし、他の演算子に関しては、複素数型のように数学で使うような「数」を表す型など、ごく限られた型でしかまず使いません。

## <a id="sec-generated-title-4"></a> <a id="overload"></a>演算子のオーバーロードの方法

演算子は <code>operator</code> キーワードを用いることで、
クラスの「[静的メソッド](oo_static.md#stmethod)」として以下のようにして定義することが出来ます。

```csharp
public static 戻り値の型 operator演算子 (引数リスト)
```


例えば、これまでに例としてあげてきた複素数クラスに加算演算子 <code>+</code> を定義したい場合、
以下のように書きます。

```csharp
class Complex
{
    public static Complex operator+ (Complex z, Complex w)
    {
        return new Complex(z.Re + w.Re, z.Im + w.Im);
    }
    // 残りの部分は省略
}
```


演算子の定義は必ず public かつ static にする必要があります。

引数リストは、
<code>+</code>, <code>-</code>, <code>*</code>, <code>/</code>
などの2項演算子なら2つ、
<code>++</code>, <code>--</code>, <code>!</code>, <code>~</code>
などの単項演算子なら1つの引数を指定します。

演算子をオーバーロードできるといっても、C# の文法を変えてしまうようなオーバーロードはできません。
たとえば、2項演算子である <code>/</code> 演算子を単項演算子としてオーバーロードすることはできません。

また、引数のうち少なくとも1つの型は演算子を定義するクラス自身である必要があります。

```csharp
class Complex
{
    // ↓この2つはOK。
    public static Complex operator+ (Complex z, double w)
    {
        return new Complex(z.Re + w, z.Im);
    }
    public static Complex operator+ (double z, Complex w)
    {
        return new Complex(z + w.Re, w.Im);
    }

    // ↓エラー。引数の少なくともどちらか一方は Complex でないと駄目。
    public static Complex operator+ (double z, double w)
    {
        return new Complex(z + w, 0);
    }

    // 残りの部分は省略
}
```



## <a id="sec-generated-title-5"></a> <a id="able"></a>オーバーロード可能な演算子

演算子の一覧とオーバーロード可能かどうかを以下に示します。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>オーバーロード可能かどうか</th>
	</tr>
	<tr>
		<td markdown="1"><code>+, -, !, ~, ++, --, true, false</code></td>
		<td markdown="1">これらの単項演算子はオーバーロード可能です。</td>
	</tr>
	<tr>
		<td markdown="1"><code>+, -, *, /, %, &amp;, |, ^, &lt;&lt;, &gt;&gt;, &gt;&gt;&gt;</code></td>
		<td markdown="1">これらの2項演算子はオーバーロード可能です。</td>
	</tr>
	<tr>
		<td markdown="1"><code>==, !=, &lt;, &gt;, &lt;=, &gt;=</code></td>
		<td markdown="1">これらの比較演算子はオーバーロード可能です。</td>
	</tr>
	<tr>
		<td markdown="1"><code>&amp;&amp;, ||</code></td>
		<td markdown="1">これらの条件 AND/OR 演算子は直接オーバーロードすることは出来ませんが、<code>&amp;, |, true, false</code>をオーバーロードすることで利用可能になります。</td>
	</tr>
	<tr>
		<td markdown="1"><code>[]</code></td>
		<td markdown="1">配列の添字演算子はインデクサとして定義することが出来ます。 詳しくは「[インデクサー](oo_indexer.md)」で説明します。</td>
	</tr>
	<tr>
		<td markdown="1">キャスト</td>
		<td markdown="1">キャストは型変換演算子として定義することが出来ます。</td>
	</tr>
	<tr>
		<td markdown="1"><code>+=, -=, *=, /=, %=, &amp;=, |=, ^=, &lt;&lt;=, &gt;&gt;=</code></td>
		<td markdown="1">(C# 13 まで) これらの代入演算子は直接オーバーロードすることは出来ませんが、 対応する2項演算子をオーバーロードすることで利用可能になります。
([C# 14 からはオーバーロード可能](#overload-compound))
</td>
	</tr>
	<tr>
		<td markdown="1"><code>=, ., ?:, -&gt;, new, is, sizeof, typeof</code></td>
		<td markdown="1">これらの演算子はオーバーロード出来ません。</td>
	</tr>
</table>


<code>+</code> などの演算子は特に説明は必要ないと思います。
ここでは、説明の必要になりそうな演算子のみをとりあげます。

### <a id="sec-generated-title-6"></a> <a id="true-false"></a>true, false 演算子

<code>true, false</code> 演算子が定義された型のオブジェクトは
<code>if</code> や <code>while, for, ?:</code> などで条件式として利用することが出来ます。

<code>true, false</code> 演算子のどちらか一方を定義する場合、もう一方も定義する必要があります。
また、<code>true, false</code> 演算子の戻り値の型は <code>bool</code> でなければなりません。

```csharp
class Bool
{
  int i;
  public Bool(int i){this.i = i;}
  public static bool operator true(Bool b){return b.i != 0;}
  public static bool operator false(Bool b){return b.i == 0;}
}

class OperatorSample
{
  static void Main()
  {
    Bool b = new Bool(0);

    if(b) // 条件式として利用できる
      Console.Write("b == true");
    else
      Console.Write("b == false");
  }
}
```


```console
b==false
```

### <a id="sec-generated-title-7"></a> <a id="increment"></a>インクリメント・デクリメント

インクリメント・デクリメント演算子は一度インスタンスをコピーし、
コピー後のインスタンスの値を変更し、戻り値とします。
前置き(<code>++x</code>)と後置き(<code>x++</code>)の2つの形式がありますが、
それぞれ以下のような手順で呼び出されます。

前置き

* x を引数として<code>++, --</code>演算子を呼び出し、その結果を x に代入する。

* x をそのまま戻り値として返す。


後置き

* x を別の場所に保存する。

* x を引数として<code>++, --</code>演算子を呼び出し、その結果を x に代入する。

* 別の場所に保存しておいた、 x の変更前の値を戻り値として返す。


```csharp
class Counter
{
  int i;
  public Counter(int i){this.i = i;}
  public static Counter operator ++(Counter c)
  {
    // c を直接書き換えては駄目。
    // インスタンスのコピーを作る。。
    Counter tmp = new Counter(c.i + 1);
    return tmp;
  }
  public override string ToString(){return this.i.ToString();}
}

class OperatorSample
{
  static void Main()
  {
    Counter c = new Counter(0);

    Console.Write(c++ + "\n");
    //↑ Counter tmp = c; c = Counter.operator++(c); return tmp;
    Console.Write(c   + "\n");
    Console.Write(++c + "\n");
    //↑ c = Counter.operator++(c); return c;
    Console.Write(c   + "\n");
  }
}
```


```console
0
1
2
2
```



### <a id="sec-generated-title-8"></a> <a id="conditional-and-or"></a>条件 AND/OR 演算子

<code>&amp;&amp;, ||</code> 演算子は直接オーバーロードすることは出来ませんが、
<code>&amp;, |</code> 演算子および <code>true, false</code> 演算子をオーバーロードすることで利用可能になります。

<code>T</code> 型の変数 <code>x, y</code> に対して、
<code>x &amp;&amp; y</code> は
<code>T.operator false(x) ? x : T.operator &amp;(x, y)</code> として評価されます。
すなわち、<code>x</code> が <code>false</code> として評価された場合、<code>y</code> は評価されません。

同様に、
<code>x || y</code> は
<code>T.operator true(x) ? x : T.operator |(x, y)</code> として評価されます。

```csharp
class Bool
{
  int i;
  public Bool(int i){this.i = i==0 ? 0 : 1;}
  public static bool operator true(Bool b)
  {
    Console.Write("  operator true called\n");
    return b.i != 0;
  }
  public static bool operator false(Bool b)
  {
    Console.Write("  operator false called\n");
    return b.i == 0;
  }
  public static Bool operator &(Bool a, Bool b)
  {
    Console.Write("  operator & called\n");
    return new Bool(a.i & b.i);
  }
  public static Bool operator |(Bool a, Bool b)
  {
    Console.Write("  operator | called\n");
    return new Bool(a.i | b.i);
  }
}

class OperatorSample
{
  static void Main()
  {
    Bool a = new Bool(1);
    Bool b = new Bool(0);

    Bool c;
    Console.Write("a && b\n");
    c = a && b;
    Console.Write("b && a\n");
    c = b && a;
    Console.Write("a || b\n");
    c = a || b;
    Console.Write("b || a\n");
    c = b || a;
  }
}
```


```console
a && b
  operator false called
  operator & called
b && a
  operator false called
a || b
  operator true called
b || a
  operator true called
  operator | called
```



### <a id="sec-generated-title-9"></a> <a id="assignment"></a>複合代入演算

(C# 13 までは) [複合代入演算子](../start/st_operator.md#compound-assignment)は直接オーバーロードすることは出来ませんが、
対応する2項演算子をオーバーロードすることで利用可能になります。

([詳細は後述しますが](#overload-compound)、C# 14 からは複合代入演算子のオーバーロードができるようにないました。)

例えば、<code>+</code> 演算子をオーバーロードした型は、
<code>x += y</code> とすることで、
<code>x = x + y</code> と同じ結果が得られます。

### <a id="sec-generated-title-10"></a> <a id="cast"></a>型変換演算

<strong id="cast" class="keyword">型変換</strong>（cast）演算子は以下のようにして定義します。

```csharp
public static explicitまたはimplicit operator 変換先の型 (変換元の型 引数名)
{
  // 変換コード
}
```


<code>explicit</code> を指定して型変換演算子を定義した場合、
明示的にキャストを行わなければ型変換を行いません
(明示的型変換)。
一方、
<code>implicit</code> を指定して型変換演算子を定義した場合、
型変換が必要になった時に自動的に型変換を行います
(暗黙的型変換)。

<code>implicit</code> を指定した場合、
意図しないところで勝手に型変換が行われてしまう可能性があるので、
出来る限り <code>explicit</code> を指定しましょう。

また、変換先の型と変換元の型の少なくともどちらか一方は型変換演算子を定義するクラス自身である必要があります。

```csharp
using System;

class Counter
{
  int i;

  public Counter(int i){this.i=i;}

  public static explicit operator Counter (int i){return new Counter(i);}
  public static explicit operator int (Counter c){return c.i;}
  public override string ToString(){return "count="+this.i;}
}

class OperatorSample
{
  static void Main()
  {
    Counter c = new Counter(1);
    Console.Write((int)c + "\n");
    Console.Write((Counter)2 + "\n");
  }
}
```


```console
1
count=2
```

## <a id="sec-generated-title-11"></a> <a id="parameter"></a>演算子の引数

(C# 7.1 以前では) 演算子の引数は[値渡し](../resource/sp_ref.md#sec-byval)である必要があります。

```csharp
class Complex
{
    public double X;
    public double Y;
    public Complex(double x, double y) => (X, Y) = (x, y);

    // これは OK
    public static Complex operator +(Complex a, Complex b)
        => new Complex(a.X + b.X, a.Y + b.Y);

    // これはコンパイル エラーになる
    public static Complex operator +(ref Complex a, ref Complex b)
        => new Complex(a.X + b.X, a.Y + b.Y);
}
```

<h5 class="version version7">Ver. 7.2</h5>

C# 7.2 で[`in` 引数](../resource/sp_ref.md#in)という機能が入りましたが、
同時に、演算子の引数にこの`in`引数が使えるようになりました。

```csharp
class Complex
{
    public double X;
    public double Y;
    public Complex(double x, double y) => (X, Y) = (x, y);

    // これなら OK
    public static Complex operator +(in Complex a, in Complex b)
        => new Complex(a.X + b.X, a.Y + b.Y);
}
```

## <a id="sec-generated-title-12"></a> <a id="checked"></a>checked 演算子

<h5 class="version version11">Ver. 11</h5>

オーバーロード可能な演算子のうち、`++`, `--`, `+`, `-`, `*`, `/` および キャスト演算子は `checked` キーワードを付けて、`checked` 演算子オーバーロードすることができます。

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](generic-math-operators.md#checked-operator-overload)」で説明します。

## <a id="sec-generated-title-13"></a> <a id="overload-compound">複合代入演算子のオーバーロード</a>

<h5 class="version version14">Ver. 14</h5>

C# 13 まで、「`+` をオーバーロードしたら `+=` も使える。`x += y` は `x = x + y` と解釈する」というように、
単独の二項演算子が先にあって、それを使って複合代入が行われていました。
このやり方だと、`+` のたびに値のコピーが必要になります。
この路線は、コピーのコストが低い「小さい構造体」(具体的にはおおむね8バイト以下)なら問題にならないんですが、クラスや大きい構造体の時に問題になります。

```csharp
using System.Runtime.CompilerServices;

// 小さい構造体は + のコストが低い。
struct SmallStruct(int value)
{
    public int Value = value;
    public static SmallStruct operator +(SmallStruct a, SmallStruct b)
    {
        // new SmallStruct(value) のコストも、それを戻り値で返すコストも小さい。
        return new(a.Value + b.Value);
    }
}

// クラスだと new() のコストが問題に。
class Class(int value)
{
    public int Value = value;
    public static Class operator +(Class a, Class b)
    {
        // クラスだと new Class(value) のコストが大きい。
        return new(a.Value + b.Value);
    }
}

// int 10個分のフィールドを持つ構造体(大きい)。
// 大きい構造体ではコピーのコストが高い。
[InlineArray(10)]
struct LargeStruct
{
    private int _value;

    public static LargeStruct operator +(in LargeStruct a, in LargeStruct b)
    {
        // int 10個分のスタックを確保して、
        LargeStruct result = default;
        for (int i = 0; i < 10; i++)
            result[i] += a[i] + b[i];

        // さらに、戻り値で返す時にコピーが発生。
        return result;
    }
}
```

演算子をオーバーロードするような型は大部分が小さい構造体だったりするので、
かつてはそれほど問題視されていませんでした。
ところが、最近はコピーにコストがかかる代数型(四則演算を持つような型)がちらほらあったりします。

* [`Matrix4x4`](https://learn.microsoft.com/ja-jp/dotnet/api/system.numerics.matrix4x4): `float` 16個分のサイズの大きい構造体(64バイト)
* [`BigInteger`](https://learn.microsoft.com/ja-jp/dotnet/api/system.numerics.biginteger): これ自体は構造体なものの、中身に `uint` の配列を含んでいてそのクローンのコストが高い
* [`Tensor<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.numerics.tensors.tensor-1): クラス実装

そこで C# 14 では、
自己書き換えな `+=` などの複合代入演算子を直接オーバーロードできるようになりました。
必ず静的メンバーとして実装する必要があった二項演算子と違い、
こちらは必ずインスタンス メンバーになります。

```csharp
using System.Runtime.CompilerServices;

struct SmallStruct(int value)
{
    public int Value = value;
    public void operator +=(SmallStruct a)
    {
        // 自己書き換え。
        Value += a.Value;
    }
}

class Class(int value)
{
    public int Value = value;
    public void operator +=(Class a)
    {
        // 自己書き換えならクラスでもコスト低め。
        Value += a.Value;
    }
}

// int 10個分のフィールドを持つ構造体(大きい)。
// 大きい構造体ではコピーのコストが高い。
[InlineArray(10)]
struct LargeStruct
{
    private int _value;

    public void operator +=(in LargeStruct a)
    {
        // 自己書き換えなら大きめの構造体でもコスト低め。
        for (int i = 0; i < 10; i++)
            this[i] += a[i];
    }
}
```

ちなみに、「`+` があれば `+=` 利用可能」だった二項演算子のオーバーロードと違って、
`+=` だけあっても `+` は使えません。

```csharp
var x = new X(5);

// += はできる。
x += 10;

// 二項演算の + はダメ。
x = x + 10;

record struct X(int Value)
{
    public void operator +=(int value) => Value += value;
}
```

`+=`, `-=`, `*=`, `/=`, `%=`, `&=`, `|=`, `^=`, `<<=`, `>>=`, `>>>=` のオーバーロードが可能です。
このうち、`+=`, `-=`, `*=`, `/=` は [`checked`](generic-math-operators.md#checked-operator-overload) オーバーロードもできます。

```csharp
record struct X(int Value)
{
    public void operator +=(int value) => Value += value;
    public void operator -=(int value) => Value -= value;
    public void operator *=(int value) => Value *= value;
    public void operator /=(int value) => Value /= value;
    public void operator %=(int value) => Value %= value;
    public void operator &=(int value) => Value &= value;
    public void operator |=(int value) => Value |= value;
    public void operator ^=(int value) => Value ^= value;
    public void operator <<=(int value) => Value <<= value;
    public void operator >>=(int value) => Value >>= value;
    public void operator >>>=(int value) => Value >>>= value;
    public void operator checked +=(int value) { checked { Value += value; }; }
    public void operator checked -=(int value) { checked { Value += value; }; }
    public void operator checked *=(int value) { checked { Value += value; }; }
    public void operator checked /=(int value) { checked { Value += value; }; }
}
```

また、同じく自己書き換えなので、インクリメント `++` とデクリメント `--` もインスタンス メンバーとしてオーバーロードできるようになりました
(これらも [`checked`](generic-math-operators.md#checked-operator-overload) にできます)。

```csharp
record struct X(int Value)
{
    public void operator ++() => Value++;
    public void operator --() => Value--;
    public void operator checked ++() { checked { Value++; } }
    public void operator checked --() { checked { Value--; } }
}
```

ただ、この自己書き換え版のインクリメント/デクリメントは後起き版(書き換える前の値を残す必要がある)の利用に難があります。
基本的には後起きインクリメント/デクリメントには使えません。

```csharp
var x = new X(1);

// 前置きはどこでも書ける。
var y = ++x;

// 後起きはダメ。コンパイル エラーになる。
var z = x++;

// ただし… 単文で書くときは後起きでも問題ない。
// (書き換え前の値を残す必要がないのでセーフ。)
++x;
x++;

record struct X(int Value)
{
    public void operator ++() => Value++;
}
```

### <a id="sec-generated-title-14"></a> <a id="compound-metadata">余談: コンパイル結果</a>

[IL](../../il/index.md) の仕様上は演算子というものはなく、
例えば `+` 演算子であれば `op_Addition` という名前の静的メソッドになっていたりします。
(`x + y` なども `X.op_Addition(x, y)` みたいなコードが生成されています。)

これに対して、複合代入演算子は `op_AdditionAssignment` みたいな名前になっています。
どの演算子も「元の演算子名」の後ろに `Assignment` が付いたものになります。
「自己書き換えのインクリメント/デクリメント」も `op_IncrementAssignment`/`op_DecrementAssignment` という名前です。
さらに、`checked` 版は `op_CheckedAdditionAssignment` というような名前になります。

### <a id="sec-generated-title-15"></a> <a id="both-binary-and-compound">注意: 両方をオーバーロード</a>

旧来の静的な二項演算子と C# 14 からのインスタンスの複合代入演算子は両方ユーザー定義できます。
この場合当然、その2種の整合性を取るのは実装する人の責任になるので、
変な実装をしてしまわないように気を付けましょう。
やろうと思えば以下のようなコードも書けてしまいます。

```csharp
var x1 = new X(1);
Console.WriteLine(++x1); // インスタンス ++ が呼ばれる。

var x2 = new X(1);
_ = x2++; // static ++ が呼ばれる。
Console.WriteLine(x2);

var x3 = new X(1);
Console.WriteLine(x3 += 1); // インスタンス += が呼ばれる。

var x4 = new X(1);
Console.WriteLine(x4 + 1); // static + が呼ばれる。

record struct X(int Value)
{
    public void operator ++() => Value++;
    public static X operator ++(X x) => new(x.Value - 1); // わざと変な実装。

    public void operator +=(int v) => Value += v;
    public static X operator +(X x, int v) => new(x.Value - v); // わざと変な実装。
}
```

```console
X { Value = 2 }
X { Value = 0 }
X { Value = 2 }
X { Value = 0 }
```

不整合を避けるために、以下のように、
複合代入演算子を先に実装して、二項演算子の方は「コピー + 複合代入」で実装するのがいいのではないかと思われます。

```csharp
record struct X(int Value)
{
    public void operator ++() => Value++;
    public static X operator ++(X x) // 後起き ++ 用。
    {
        var y = x; // コピー。
        ++y; // インスタンス ++ を呼び出す。
        return y;
    }

    public void operator +=(int v) => Value += v;
    public static X operator +(X x, int v)
    {
        var y = x; // コピー。
        y += v; // インスタンス += を呼び出す。
        return y;
    }
}
```

### <a id="sec-generated-title-16"></a> <a id="compound-virtual">余談: virtual</a>

複合代入演算子のオーバーロードはインスタンス メンバーなので、一応、`virtual` や `abstract` にできます。

```csharp
var x = new A();
SumTo5(x);
Console.WriteLine(x.Value); // 15

var y = new B();
SumTo5(y);
Console.WriteLine(y.Value); // 120

static void SumTo5(Base x)
{
    for (int i = 1; i <= 5; i++) x += i;
}

// += の実装を派生クラスごとに変えれる。
abstract class Base
{
    public int Value;
    public abstract void operator+= (int value);
}

class A : Base
{
    // 普通に和にする。
    public override void operator +=(int value) => Value += value;
}

class B : Base
{
    // + を積にしてしまう。
    public B() => Value = 1;
    public override void operator +=(int value) => Value *= value;
}
```

(書きかけ)
## <a id="exercise"></a>演習問題

### <a id="exercise-opeover1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体を2次元ベクトルとみなして、
ベクトルの和・差を計算する演算子 <code>+</code> および <code>-</code> を追加せよ。

```csharp
/// <summary>
/// ベクトル和
/// </summary>
/// <param name="a">点A</param>
/// <param name="b">点B</param>
/// <returns>和</returns>
public static Point operator +(Point a, Point b)

/// <summary>
/// ベクトル差
/// </summary>
/// <param name="a">点A</param>
/// <param name="b">点B</param>
/// <returns>和</returns>
public static Point operator -(Point a, Point b)
```



#### 解答例 1


```csharp
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
  #region 演算子

  /// <summary>
  /// ベクトル和
  /// </summary>
  /// <param name="a">点A</param>
  /// <param name="b">点B</param>
  /// <returns>和</returns>
  public static Point operator +(Point a, Point b)
  {
    return new Point(a.x + b.x, a.y + b.y);
  }

  /// <summary>
  /// ベクトル差
  /// </summary>
  /// <param name="a">点A</param>
  /// <param name="b">点B</param>
  /// <returns>和</returns>
  public static Point operator -(Point a, Point b)
  {
    return new Point(a.x - b.x, a.y - b.y);
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
    Point ab = b - a;
    Point ac = c - a;
    return 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
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
