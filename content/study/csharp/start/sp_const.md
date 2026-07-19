---
title: "定数"
source_url: "https://ufcpp.net/study/csharp/start/sp_const/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2021-09-18T00:00:00"
tags: []
umbraco_id: 1214
parent_id: 1190
sort_order: 14
aliases:
  - "/csharp/sp_const"
  - "/csharp/sp_const.html"
  - "/csharp/start/sp_const/"
  - "/study/csharp/sp_const"
  - "/study/csharp/sp_const.html"
---

# 定数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

全く変化しない値を、異なる場所で何度も使いたい事があります。
このような場合、「[リテラル](st_variable.md#literal)」を何箇所にも分散させて書くのではなく、<code>const</code> というキーワードを用いて定義した定数を使うべきです。


##### <a id="sec-generated-title-2"></a>ポイント

* const キーワードを使って、定数（値が絶対に変わらない / 変えれない変数）を定義できます。
    * 見た目は変数と同じように使えますが、値の変更はできません。

    * コンパイル結果は「[リテラル](st_variable.md#literal)」を直接書いた場合と同様になります。



* 定数は、宣言時に値をリテラルで初期化できるものにしか使えません。（new できない。）

* より柔軟な初期化ができる readonly （読み取り専用）変数というものもあります。



## <a id="sec-generated-title-3"></a> <a id="constant"></a>変化しない値

例えば、以下のようなコードを見てください。

```csharp
int[] array = new int[5];

for(int i=0; i<5; ++i)
  array[i] = int.Parse(Console.ReadLine());

int sum = 0;
int sq_sum = 0;

for(int i=0; i<5; ++i)
{
  int n = array[i];
  sum += n;
  sq_sum += n*n;
}

double mean = sum / 5;
double var  = sq_sum / 5 - mean*mean;

Console.Write("平均: {0}\n分散: {1}\n", mean, var);
```


値を5つ入力してもらって、その平均と分散を求めるものです。
5 という「[リテラル](st_variable.md#literal)」が4箇所出てきていますね。

さて、ではここで、データの数を5つではなくて6つに変更することになったとします。
5 というリテラルを全部 6 に置き換える必要があるわけですが、
たった4つでも結構面倒です。
まして、もっと数が多かったことを考えてみましょう。
数が増えるにつれて、忘れず全部修正するのが困難になります。

なので、普通はリテラルを直接使うということはしません。
例えば、以下のように、5 と言う値を一度変数に代入して使うことを考えます。

```csharp
int NUM = 5;
int[] array = new int[NUM];

for(int i=0; i<NUM; ++i)
  array[i] = int.Parse(Console.ReadLine());

int sum = 0;
int sq_sum = 0;

for(int i=0; i<NUM; ++i)
{
  int n = array[i];
  sum += n;
  sq_sum += n*n;
}

double mean = sum / (double)NUM;
double var  = sq_sum / (double)NUM - mean*mean;

Console.Write("平均: {0}\n分散: {1}\n", mean, var);
```


これで、もしデータの個数を変更する必要が生じても、
<code>int NUM = 5;</code> の1行だけの修正で解決します。


## <a id="sec-generated-title-4"></a> <a id="const"></a>const

ところが、このコードにもちょっとだけ問題があります。
1つは、定数なのか、途中で値が変わるものなのかが分からないことです。
変数なので、途中で値が書き換えられてしまってもエラーにはなりません。
また、ソースファイルの見易さの観点からも、
定数は定数であることが一目で分かる方が好ましいです。

2つ目の問題は、効率面にあります。
書き換える必要のある変数よりも、
その必要のない定数の方が、プログラムの実行効率が高くなります。
したがって、上述のような方法（一度変数に値を格納）すると、
多少ですが実行効率が悪くなるという欠点があります。

<em>
        そこで、C# では、const というキーワードを用いることで、
        変数のように扱える定数を定義することが出来ます。
      </em>
通常、<strong id="constant" class="keyword">定数</strong>（constant）とだけいうと、
「[リテラル](st_variable.md#literal)」ではなく、
こちらのことを指します。
（リテラルの方は直定数と訳す。）

```csharp
const int NUM = 5;
int[] array = new int[NUM];

for(int i=0; i<NUM; ++i)
  array[i] = int.Parse(Console.ReadLine());

int sum = 0;
int sq_sum = 0;

for(int i=0; i<NUM; ++i)
{
  int n = array[i];
  sum += n;
  sq_sum += n*n;
}

double mean = sum / (double)NUM;
double var  = sq_sum / (double)NUM - mean*mean;

Console.Write("平均: {0}\n分散: {1}\n", mean, var);
```


const を付けて宣言された定数は、宣言文中における初期化時にのみ値を代入できます。
定数というくらいですから、当然、
その他の場所で値を書き換えることは出来ません。

```csharp
const int NUM = 5; // 宣言時の初期化のみ可能
NUM = 6; // ここでエラーになる
```


また、const を付けた定数を用いたソースコードは、
リテラルを使ったソースコードと同等のコンパイル結果になります。
従って、リテラルの直書きと比べて効率が落ちることはありません。

ただし、const キーワードは、int などの数値型、string 型、または列挙型に対してのみ使用できます。
（あと、値が null 限定で参照型にも使える。）
インスタンスを new キーワードで生成するようなものには const キーワードは使えません。


## <a id="sec-generated-title-5"></a> <a id="const_member"></a>const メンバー

const を使った定数は、
メソッド中（ローカル）だけでなく、
クラスのメンバーにする事も出来ます。
（「[クラス](../oop/oo_class.md#class)」に関しては別章参照: 「[クラス](../oop/oo_class.md)」。）

```csharp
class Math
{
  public const double PI = 3.1415926535897932;
}
```


const メンバーはクラスに属します。
（「[静的メンバー](../oop/oo_static.md#static-member)」と同じ扱い。
<code>ClassName.Member</code> という形式で参照。）
例えば、上述の例、<code>PI</code> の場合、<code>Math.PI</code> という形式で参照します。


### <a id="sec-generated-title-6"></a> <a id="versioning"></a>const のバージョニング問題

ちなみに、private な場合は const メンバー変数で問題ないのですが、
public にする場合にはあまり const メンバー変数は使わない方がいいです。

単一のプロジェクトで使っている分には何の問題もありませんが、
もし、ライブラリで定数を定義して、別プロジェクトのアプリから参照するような場合には注意が必要です。
定数は、コンパイル時にリテラルと全く同じように値が展開されてしまうため、
定数を定義しているライブラリの方だけでなく、参照しているアプリ側も再コンパイルしないと、値の変化が反映されません。

このような問題を、バージョン アップしたときの挙動が怪しくなるという意味で、
バージョニング問題（versioning problem）と呼びます。

したがって、
数学の定数である π のように、
値が変わること自体まずありえないような定数なら全然問題ないのですが、
もしも変更がありうる場合には、
たとえ定数であっても static な「[プロパティ](../oop/oo_property.md#property)」にしておく方がいいです。
でないと、値が変わったときに、利用側でも再コンパイルが必要になってしまいます。


## <a id="sec-generated-title-7"></a> <a id="readonly"></a>readonly

クラスのメンバーに対しては、
const 以外に、もう1つ定数のようなものを実現する方法があります。
readonly というキーワードを用いて、<strong id="ro" class="keyword">読取り専用</strong>（read only）の変数を定義できます。
const との違いは以下のようになります。

<table summary="">

	<tr>
		<th>const</th>
		<th>readonly</th>
	</tr>
	<tr>
		<td markdown="1">ローカル変数にも使える</td>
		<td markdown="1">クラスのメンバー変数のみ。</td>
	</tr>
	<tr>
		<td markdown="1">常に静的変数と同じ扱い。</td>
		<td markdown="1">static の有無を変えられる。</td>
	</tr>
	<tr>
		<td markdown="1">宣言時にのみ初期化可能。</td>
		<td markdown="1">コンストラクタ内で値を書き換え可能。</td>
	</tr>
	<tr>
		<td markdown="1">コンパイル結果はリテラルと同等。</td>
		<td markdown="1">コンパイル結果は変数と同等。</td>
	</tr>
	<tr>
		<td markdown="1">インスタンスを new で生成するようなものには使えない。</td>
		<td markdown="1">new 可能。</td>
	</tr>
</table>


```csharp
class A
{
  readonly int num;

  public A(int num)
  {
    this.num = num; // コンストラクタ内では書き換え可能。
  }

  public void Method(int num)
  {
    int x = this.num; // 読み取りは可能。
    this.num = num;   // 書き込み不可。エラー！
  }
}
```

### <a id="sec-generated-title-8"></a> <a id="prefer-readonly"></a>注意: const の問題とreadonlyやプロパティ

ちなみに、const を使った定数は、
（コンパイル結果がリテラルを使った結果と同じく）
プログラム中に直接値が埋め込まれてしまうため、
値を変更した際には、参照側（クラス利用側）のコードも再コンパイルする必要が生じます。
なので、<code>Math.PI</code>（数学定数π）のように、本当に不変で、
絶対に変わることのない値以外は public const なメンバー変数にすべきではありません。
（private なものや、ローカル変数に対する const は OK。）

その代替として、`readonly`なフィールドや、[get-onlyなプロパティ](../oop/oo_property.md#get-only)が使えます。

```csharp
class A
{
    // const はあんまり public にしたくない
    // 今後絶対に値を変更しないという自信がない限りは使わない方がいい
    public const int X = 1;

    // readonly フィールドや、get-only プロパティ越しに公開することを推奨
    // (プロパティの方がより推奨)
    public readonly int Y = 1;
    public int Z => 1;
}
```

### <a id="sec-generated-title-9"></a> <a id="struct-class-readonly"></a>readonly の注意点

現時点では触れられませんが、クラスや構造体を説明した後に、改めて`readonly`に関する注意点があります。
詳細は「[readonly の注意点](../resource/readonlyness.md)」を参照してください。

## <a id="sec-generated-title-10"></a> <a id="constant-expressions"></a>定数にできるもの

C# で `const` を使った定数にできるものには以下のようなものがあります。

* [リテラル](st_embeddedtype.md#literal)
* [列挙型](../structured/st_enum.md)の値
* 他の `const` メンバー
* 上記同士の[式](st_variable.md#expression)(四則演算とか)

例えば、以下のように複数のリテラル、定数を組み合わせたものも再び定数にできます。

```csharp
// enum
const DayOfWeek D1 = DayOfWeek.Friday;
const DayOfWeek D2 = DayOfWeek.Wednesday;

// int のリテラル同士の掛け算。
const int I = 3 * 5;

// 他の定数を参照。
const int J = 2 * I;

// 参照しているものが全部定数なら多少複雑な式でも OK。
const double X = (int)(J % 2 == 0 ? D1 : D2) * 1.25;

// 文字列も const にできる。
const string S = "abc" + "def";

// その他の型では、null だけ const にできる。
const object N = null;
```

ちなみに、特殊なものでは [`nameof` 演算子](st_string.md#nameof-operator)は完全にリテラルと同列の扱いを受けるので、`const` に使えます。

```csharp
const string A = nameof(A); // "A" と同じ結果になる。
const string B = A + nameof(B); // 他の const + リテラル という扱い。
```

### <a id="sec-generated-title-11"></a> <a id="constant-string-interpolation"></a>const 文字列補間

<h5 class="version version10">Ver. 10</h5>

C# 10.0 からは、[文字列補間](st_string.md#string-interpolation)でも、`{}` の中身が `const` 文字列な場合に限り、補完結果も `const` にできます。
例えば以下のような `const` 文字列を作れます。

```csharp
const string A = "Abc";
const string B = "Xyz";
const string C = $"{nameof(A)}: {A}, {nameof(B)}: {B}"; // "A: Abc, B: Xyz"
```

この例のように `nameof` との組み合わせはそれなりに需要があるかと思います。

一方で、`{}` の中身が文字列でない場合、たとえ `const` であっても文字列補間結果は `const` にできなくなります。

```csharp
const int A = 1;
const string C = $"{A}"; // A が文字列じゃないので $"" の結果を const にできない。
```

第一印象としては `const` にできてもよさげに見えるんですが…
これは、文字列補間の結果が一定にならないためです。
例えば、以下のように、浮動小数点数を文字列化した結果は国によって異なります。

```csharp
using System.Globalization;

// 東南アジアの多くの国は . を小数点に使う。
Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-jp");
Console.WriteLine(1.234);

// 大陸ヨーロッパの多くの国は , を小数点に使う。
Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-fr");
Console.WriteLine(1.234);
```

```csharp
1.234
1,234
```
