---
title: "組込み型変換"
source_url: "https://ufcpp.net/study/csharp/start/st_cast/"
content_type: "Article"
published_at: "2015-05-06T14:07:58"
updated_at: "2021-09-22T16:44:12"
tags: []
umbraco_id: 1209
parent_id: 1190
sort_order: 12
aliases:
  - "/study/csharp/st_cast.html"
---

# 組込み型変換

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# には <code>int</code> 型から <code>double</code> 型というように、
ある型から別の型に変換する機能があります。

型変換は、代入などの操作を行うだけで暗黙的に行われる変換と、
変換先の型を明示的に指定して行う変換があります。
また、いずれの変換も、変換できる型・できない型が予め決まっています。


##### <a id="sec-generated-title-2"></a>ポイント

* C# では、ほとんどの場合、型変換は自動的には行われません

* short x = (short)5; というように、明示的に型変換します

* short → int のように、変換しても値の精度が失われない物についてのみ、暗黙的な型変換が用意されています



## <a id="sec-generated-title-3"></a> <a id="implicit"></a>暗黙的な型変換

暗黙的(implicit)な型変換とは、ある型の変数を別の型の変数に代入するだけで自動的に型を変換してくれる機能です。
以下に暗黙的な型変換の例を挙げます。

```csharp {title="暗黙的な型変換の例"}
short   m = 365;
long    n = m; // short から long への暗黙的な型変換。
double  x = n; // long から double への暗黙的な型変換。
```


C# で定義されている組込み型同士の間の暗黙的な型変換は以下のとおりです。

<table summary="">

	<tr>
		<th>変換元</th>
		<th>変換先</th>
	</tr>
	<tr>
		<td markdown="1"><code>sbyte</code></td>
		<td markdown="1"><code>short, int, long, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>byte</code></td>
		<td markdown="1"><code>short, ushort, int, uint, long, ulong, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>short</code></td>
		<td markdown="1"><code>int, long, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>ushort</code></td>
		<td markdown="1"><code>int, uint, long, ulong, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>int</code></td>
		<td markdown="1"><code>long, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>uint</code></td>
		<td markdown="1"><code>long, ulong, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>long</code></td>
		<td markdown="1"><code>float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>ulong</code></td>
		<td markdown="1"><code>float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>char</code></td>
		<td markdown="1"><code>ushort, int, uint, long, ulong, float, double, decimal</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>float</code></td>
		<td markdown="1"><code>double</code></td>
	</tr>
</table>


C# における暗黙的な型変換の基本的なルールは以下のとおりです。

* 符号なし整数から符号付き整数への変換は暗黙的に行える。

* 表現できる値の範囲が広い型への変換は暗黙的に行える。


<em>要するに、C# では型を変換しても値の大きさが失われない場合にのみ暗黙的な型変換が行えます</em>。
ただし、<code>int, uint, long</code> から <code>float</code> への変換、および <code>long</code> から <code>double</code> への変換では有効桁数が落ちる場合があります。


## <a id="sec-generated-title-4"></a> <a id="explicit"></a>明示的な型変換

暗黙的に変換を行えない型同士の変換は以下のように明示的(explicit)に行う必要があります。

```csharp {title="明示的な型変換"}
int     i = 365;
short   j = (short)i; // int から short への明示的な型変換。
int     m = 365;
byte    n = (byte)m;  // int から byte への明示的な型変換。
                      // byte は 0～255 までの範囲しか表現できないので、
                      // n の値は 365 mod 256 = 109 になる。
double  x = 3.14159;
long    y = (long)x; // double から long への明示的な型変換。
                      // 値は小数点以下切捨てられて 3 になる。
```


この、「<code>
        (<span class="input">変換後の型名</span>)<span class="input">変数名</span>
      </code>」という形の式のことを「<strong id="cast" class="keyword">キャスト</strong>（cast）式」と呼びます。

C# では、すべての数値型同士の間で明示的な型変換を行うことが出来ます。
しかしながら、文字列から値型への変換は、キャストでは行えません。
文字列を数値化する場合には、<code>Parse</code> というメソッドを使用します。

## <a id="exercise"></a>演習問題

### <a id="exercise-cast1"></a>問題 1


適当な文字を入力し、その文字コードを表示するプログラムを作成せよ。
（char 型の変数を int 型にキャストすると文字コードが得られます。）


#### 解答例 1


```csharp {title="文字コードの表示"}
using System;

class Exercise
{
  static void Main()
  {
    char c;

    Console.Write("文字を入力してください: ");
    c = Console.ReadLine()[0];

    Console.Write("文字 {0} の文字コードは {1}\n", c, (int)c);
  }
}
```



### <a id="exercise-cast2"></a>問題 2


整数型（int, short, long）同士の割り算では、結果も整数となり、あまりは切り捨てられます。
切り捨てられると困る場合、浮動小数点数（double, float）にキャストしてから計算する必要があります。

このことを確かめるため、
2つの整数を入力し、
整数のままで割り算した結果（あまり切り捨て）と、
浮動小数点数として割り算した結果を比較するプログラムを作成せよ。


#### 解答例 1


```csharp {title="整数と浮動小数点数の割り算"}
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("input a: ");
    int a = int.Parse(Console.ReadLine());
    Console.Write("input b: ");
    int b = int.Parse(Console.ReadLine());

    Console.Write("整数: {0} / {1} = {2} … {3}\n", a, b, a / b, a % b);
    Console.Write("実数: {0} / {1} = {2}\n", a, b, a / (double)b);
  }
}
```



### <a id="exercise-cast3"></a>問題 3


double → int にキャストすると、値が整数に切り詰められます。
このとき、どのようにして値が切り詰められるのか（切捨てなのか切り上げなのか等）を調べよ。
（正の数だけでなく、負の数も。）


#### 解答例 1


```csharp {title="double → int"}
using System;

class Exercise
{
  static void Main()
  {
    // まず、正の数をいくつか確認。
    Console.Write("{0} → {1}\n", 3.8, (int)3.8);
    Console.Write("{0} → {1}\n", 3.1, (int)3.1);
    Console.Write("{0} → {1}\n", 2.7, (int)2.7);
    Console.Write("{0} → {1}\n", 2.4, (int)2.4);
    Console.Write("{0} → {1}\n", 1.5, (int)1.5);
    Console.Write("{0} → {1}\n", 0.5, (int)0.5);
    // 負の数も。
    Console.Write("{0} → {1}\n", -3.8, (int)-3.8);
    Console.Write("{0} → {1}\n", -3.1, (int)-3.1);
    Console.Write("{0} → {1}\n", -2.7, (int)-2.7);
    Console.Write("{0} → {1}\n", -2.4, (int)-2.4);
    Console.Write("{0} → {1}\n", -1.5, (int)-1.5);
    Console.Write("{0} → {1}\n", -0.5, (int)-0.5);
  }
}
```


```console {title="double → int"}
3.8 → 3
3.1 → 3
2.7 → 2
2.4 → 2
1.5 → 1
0.5 → 0
-3.8 → -3
-3.1 → -3
-2.7 → -2
-2.4 → -2
-1.5 → -1
-0.5 → 0
```


結果を見ての通り、正の数は切り捨て、負の数は切り上げ（0 に向かって丸め）になります。

正負問わず値を切り捨てたい場合は <code>Math.Floor</code> 関数を、
切り上げたい場合は <code>Math.Ceiling</code> 関数を、
四捨五入したい場合は <code>Math.Round</code> 関数を使用します。
