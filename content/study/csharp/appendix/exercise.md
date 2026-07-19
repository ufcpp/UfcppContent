---
title: "演習問題集"
source_url: "https://ufcpp.net/study/csharp/appendix/exercise/"
content_type: "ExerciseList"
published_at: "2015-05-06T16:02:28"
updated_at: "2015-05-06T16:02:28"
tags: []
umbraco_id: 1702
parent_id: 1377
sort_order: 6
aliases:
  - "/csharp/appendix/exercise/"
  - "/csharp/exercise"
  - "/csharp/exercise.html"
  - "/study/csharp/exercise"
  - "/study/csharp/exercise.html"
---

# 演習問題集

## <a id="1192"></a>[プログラムの作成・実行](../start/st_compile.md)

### <a id="1192-exercise-compile1"></a>問題 1


「[C#の簡単なプログラム例](../start/st_basis.md#sample)」中のプログラムを実際に作成し、コンパイル・実行してみよ。


## <a id="1195"></a>[値の入出力](../start/st_consoleio.md)

### <a id="1195-exercise-console1"></a>問題 1


Console.Write を用いて、自分の名前を画面に表示せよ。


#### 解答例 1


```csharp
using System;

class Sample
{
  static void Main()
  {
    Console.Write("岩永信之");
  }
}
```



### <a id="1195-exercise-console2"></a>問題 2


Console.ReadLine を用いて文字列を1行読み込み、
Console.Write を用いて読んだ文字列をそのまま鸚鵡返しするプログラムを作成せよ。

おまけ： 1度読み込んだ文字列を2度ずつ鸚鵡返しするものを作成せよ。


#### 解答例 1


```csharp
using System;

class Sample
{
  static void Main()
  {
    string line = Console.ReadLine();
    Console.Write(line);
  }
}
```



#### 解答例 2


```csharp
using System;

class Sample
{
  static void Main()
  {
    string line = Console.ReadLine();
    Console.Write(line);
    Console.Write(line);
  }
}
```



## <a id="1203"></a>[組込み演算子](../start/st_operator.md)

### <a id="1203-exercise-ope1"></a>問題 1


2つの整数を入力し、
その整数の四則演算(＋, －, ×, ÷)結果を表示するプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("input a: ");
    int a = int.Parse(Console.ReadLine());
    Console.Write("input b: ");
    int b = int.Parse(Console.ReadLine());

    Console.Write("{0} + {1} = {2}\n", a, b, a + b);
    Console.Write("{0} - {1} = {2}\n", a, b, a - b);
    Console.Write("{0} * {1} = {2}\n", a, b, a * b);
    Console.Write("{0} / {1} = {2}\n", a, b, a / b);
  }
}
```



### <a id="1203-exercise-ope2"></a>問題 2


前問の「整数の四則演算」の、 double, short 等の他の型を用いた物を作成せよ。


#### 解答例 1


例として double 版を掲載。

```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("input a: ");
    double a = double.Parse(Console.ReadLine());
    Console.Write("input b: ");
    double b = double.Parse(Console.ReadLine());

    Console.Write("{0} + {1} = {2}\n", a, b, a + b);
    Console.Write("{0} - {1} = {2}\n", a, b, a - b);
    Console.Write("{0} * {1} = {2}\n", a, b, a * b);
    Console.Write("{0} / {1} = {2}\n", a, b, a / b);
  }
}
```



### <a id="1203-exercise-ope3"></a>問題 3


複素数 x + iy の逆数を求めるプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("実部を入力してください: ");
    double x = double.Parse(Console.ReadLine());
    Console.Write("虚部を入力してください: ");
    double y = double.Parse(Console.ReadLine());

    double norm = x * x + y * y;

    Console.Write("{0} + i({1}) の逆数は {2} + i({3})\n)",
      x, y,
      x / norm, -y / norm);
  }
}
```



### <a id="1203-exercise-ope4"></a>問題 4


半径を入力し、その半径の円の面積を求めるプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    double r; // 半径

    Console.Write("半径を入力してください: ");
    r = double.Parse(Console.ReadLine());

    double area = r * r * 3.1415926535897932;
    Console.Write("面積 = {0}\n", area);
  }
}
```



### <a id="1203-exercise-ope5"></a>問題 5


体重と身長を入力し、BMIを求めるプログラムを作成せよ。

<blockquote markdown="1">
BMIは、WHO（世界保健機関）が推奨しているもので、Body Mass Indexの略称で、肥満度指数とも呼ばれています。 BMIは肥満度の基準として、広く使用されている測定方法です。
計算式は、下記のとおりで比較的簡単に計算できることも特徴です。

BMI = 体重(kg)÷{身長(m)×身長(m)}

BMIの値が22のときに病気になる可能性が最も低く、BMIが26を超えると糖尿病など生活習慣病になるリスクが高まると言われています。

<table summary="">

	<tr>
		<td markdown="1">BMI 値</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1">19.8未満</td>
		<td markdown="1">やせ型</td>
	</tr>
	<tr>
		<td markdown="1">19.8～24.2未満</td>
		<td markdown="1">普通</td>
	</tr>
	<tr>
		<td markdown="1">24.2～26.4未満</td>
		<td markdown="1">やや肥満（過体重）</td>
	</tr>
	<tr>
		<td markdown="1">26.4～35.0未満</td>
		<td markdown="1">肥満</td>
	</tr>
	<tr>
		<td markdown="1">35.0以上</td>
		<td markdown="1">高度肥満（要治療）</td>
	</tr>
</table>


</blockquote>
以下にプログラムの実行結果の例を示す。

```console
身長[cm] = 175.5
体重[kg] = 52.4
BMI = 17.0128489216808
```



#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    double height; // 身長[cm]
    double weight; // 体重[kg]

    Console.Write("身長[cm]: ");
    height = double.Parse(Console.ReadLine());
    height *= 0.01; // cm → m

    Console.Write("体重[kg]: ");
    weight = double.Parse(Console.ReadLine());

    double bmi = weight / (height * height);
    Console.Write("BMI = {0}\n", bmi);
  }
}
```



## <a id="1209"></a>[組込み型変換](../start/st_cast.md)

### <a id="1209-exercise-cast1"></a>問題 1


適当な文字を入力し、その文字コードを表示するプログラムを作成せよ。
（char 型の変数を int 型にキャストすると文字コードが得られます。）


#### 解答例 1


```csharp
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



### <a id="1209-exercise-cast2"></a>問題 2


整数型（int, short, long）同士の割り算では、結果も整数となり、あまりは切り捨てられます。
切り捨てられると困る場合、浮動小数点数（double, float）にキャストしてから計算する必要があります。

このことを確かめるため、
2つの整数を入力し、
整数のままで割り算した結果（あまり切り捨て）と、
浮動小数点数として割り算した結果を比較するプログラムを作成せよ。


#### 解答例 1


```csharp
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



### <a id="1209-exercise-cast3"></a>問題 3


double → int にキャストすると、値が整数に切り詰められます。
このとき、どのようにして値が切り詰められるのか（切捨てなのか切り上げなのか等）を調べよ。
（正の数だけでなく、負の数も。）


#### 解答例 1


```csharp
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


```console
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


## <a id="1220"></a>[条件分岐](../structured/st_branch.md)

### <a id="1220-exercise-brancheo"></a>問題 1


ユーザから入力された整数が奇数か偶数か判定するプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("整数を入力してください: ");
    int n = int.Parse(Console.ReadLine());

    if (n % 2 == 0) Console.Write("{0} は偶数です\n", n);
    else            Console.Write("{0} は奇数です\n", n);
  }
}
```



### <a id="1220-exercise-branch0"></a>問題 2


[組込み演算子](../start/st_operator.md)の[問題 5](../start/st_operator.md#exercise-ope5)のプログラムを修正し、
BMI 値から体型(やせ型、普通、やや肥満、肥満、高度肥満)を判定し、
表示するプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("身長[cm]: ");
    double height = double.Parse(Console.ReadLine()) * 0.01;
    Console.Write("体重[kg]: ");
    double weight = double.Parse(Console.ReadLine());

    double bmi = weight / (height * height);
    Console.Write("BMI = {0}\n", bmi);

    if(bmi < 19.8)      Console.Write("やせ型");
    else if(bmi < 24.2) Console.Write("普通");
    else if(bmi < 26.4) Console.Write("やや肥満（過体重）");
    else if(bmi < 35.0) Console.Write("肥満");
    else                Console.Write("高度肥満（要治療）");
    Console.Write("です\n");
  }
}
```



### <a id="1220-exercise-branch1"></a>問題 3


switch 文を使って150以下の平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。

ヒント：
要するに、ユーザからの入力が 1, 4, 9, 16, ・・・になっているかどうかを switch 文で判別します。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("整数を入力してください: ");
    int n = int.Parse(Console.ReadLine());

    switch (n)
    {
      case 1:
        Console.Write("{0} は平方数です。\n", n);
        break;
      case 2 * 2: goto case 1;
      case 3 * 3: goto case 1;
      case 4 * 4: goto case 1;
      case 5 * 5: goto case 1;
      case 6 * 6: goto case 1;
      case 7 * 7: goto case 1;
      case 8 * 8: goto case 1;
      case 9 * 9: goto case 1;
      case 10 * 10: goto case 1;
      case 11 * 11: goto case 1;
      case 12 * 12: goto case 1;
      default:
        Console.Write("{0} は平方数ではないか、150以上です\n", n);
        break;
    }
  }
}
```



### <a id="1220-exercise-branch2"></a>問題 4


数値を3つ入力してもらい、
その3つの値の中の最大値、最小値を求めるプログラムを作成せよ。


#### 解答例 1


単純な条件分岐による方法。

```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("値1: ");
    double x = double.Parse(Console.ReadLine());
    Console.Write("値2: ");
    double y = double.Parse(Console.ReadLine());
    Console.Write("値3: ");
    double z = double.Parse(Console.ReadLine());

    if (x > y)
    {
      if (x > z)
      {
        if (y > z) Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", x, y, z);
        else       Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", x, z, y);
      }
      else Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", z, x, y);
    }
    else
    {
      if (y > z)
      {
        if (x > z) Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", y, x, z);
        else       Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", y, z, x);
      }
      else Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", z, y, x);
    }
  }
}
```



#### 解答例 2


3つの数値をあらかじめ整列してしまう方法。

```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("値1: ");
    double x = double.Parse(Console.ReadLine());
    Console.Write("値2: ");
    double y = double.Parse(Console.ReadLine());
    Console.Write("値3: ");
    double z = double.Parse(Console.ReadLine());

    double tmp;

    if (y < z) { tmp = y; y = z; z = tmp; }
    if (x < y) { tmp = x; x = y; y = tmp; }
    if (y < z) { tmp = y; y = z; z = tmp; }

    Console.Write("最大 {0}, 中間{1}, 最小 {2}\n", x, y, z);
  }
}
```



## <a id="1225"></a>[反復処理](../structured/st_loop.md)

### <a id="1225-exercise-loop0"></a>問題 1


ユーザに整数 n を入力してもらい、
1 から n までの整数の和を求めるプログラムを作成せよ。
ただし、
ループを使って和を求めたものと、
和の公式 <span class="math">
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table> n <span class="paren" style="font-size:em;">(</span>n <span class="normal">+</span> <span class="normal">1</span><span class="paren" style="font-size:em;">)</span>
          </span> の結果を比較せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("n: ");
    int n = int.Parse(Console.ReadLine());
    int sum = 0;

    for (int i = 1; i <= n; ++i)
    {
      sum += i;
    }

    Console.Write("loop {0}, formula {1}\n", sum, n * (n + 1) / 2);
  }
}
```



### <a id="1225-exercise-loop1"></a>問題 2


平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。
[条件分岐](../structured/st_branch.md)の[問題 2](../structured/st_branch.md#exercise-branch0)と異なり、判別できる数値に上限は設けない。

ヒント：ループと条件分岐を組み合わせて作る。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("整数を入力してください: ");
    int n = int.Parse(Console.ReadLine());
    int i;

    for (i = 0; i <= n; ++i)
    {
      if (n == i * i) break;
    }

    if (i <= n) Console.Write("{0} = {1} × {1} は平方数です\n", n, i);
    else        Console.Write("{0} は平方数ではありません\n", n);
  }
}
```


ちなみに、この for ループの継続条件の部分は、
<code>i &lt;= (int)Math.Sqrt(n)</code> でも OK。
（その下の if 文の条件も変更する必要あり。）
Sqrt は n の平方根を求める関数。


### <a id="1225-exercise-loop2"></a>問題 3


2重ループを使って掛け算の九九表を表示するプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    for (int i = 1; i <= 9; ++i)
    {
      for (int j = 1; j <= 9; ++j)
      {
        Console.Write("{0,3}", i * j);
      }
      Console.Write('\n');
    }
  }
}
```



## <a id="1229"></a>[配列](../structured/st_array.md)

### <a id="1229-exercise-array0"></a>問題 1


for 文を使って以下の漸化式の一般項 <span class="math">
            a<sub>n</sub>
          </span> を20項目まで求めるプログラムを作成せよ。 (<span class="math">
            a<sub>n</sub>
          </span> を配列で表す。)
<div class="math">
          a<sub>n ＋ 2</sub> ＝ 2 a<sub>n ＋ 1</sub> － 2 a<sub>n</sub>
        </div><div class="math">
          a<sub>0</sub> ＝ 3
        </div><div class="math">
          a<sub>1</sub> ＝ 1
        </div>

#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    int[] a = new int[21];
    a[0] = 3;
    a[1] = 1;

    // 数列を求める。
    for (int i = 2; i < a.Length; ++i)
    {
      a[i] = 2 * a[i - 1] - 2 * a[i - 2];
    }

    // 求めた数列を表示。
    for (int i = 0; i < a.Length; ++i)
    {
      Console.Write("{0} ", a[i]);
    }
    Console.Write('\n');
  }
}
```



### <a id="1229-exercise-array1"></a>問題 2


int 型の配列に格納されている値の最大値、最小値および平均値を求めよ。
できれば、配列の長さ n および n 個の整数値をユーザに入力してもらうようにすること。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    // 配列長の入力
    Console.Write("配列の長さ: ");
    int n = int.Parse(Console.ReadLine());

    // 配列の値の入力
    int[] a = new int[n];
    for (int i = 0; i < n; ++i)
    {
      Console.Write("{0}: ", i);
      a[i] = int.Parse(Console.ReadLine());
    }

    // 最大値、最小値、平均値の計算
    int max = int.MinValue;
    int min = int.MaxValue;
    double average = 0;

    for (int i = 0; i < n; ++i)
    {
      if (max < a[i]) max = a[i];
      if (min > a[i]) min = a[i];
      average += a[i];
    }
    average /= n;

    Console.Write(
@"
最大値: {0}
最小値: {1}
平均値: {2}
"
    , max, min, average);
  }
}
```



### <a id="1229-exercise-array2"></a>問題 3


double 型の2次元配列を行列に見立てて、行列の掛け算を行うプログラムを作成せよ。


#### 解答例 1


行列の次元は任意だけども、例として2×2行列の場合を示す。

```csharp
using System;

class Exercise
{
  static void Main()
  {
    double[,] a = new double[,]
    {
      {1, 1},
      {1, 0},
    };
    double[,] b = new double[,]
    {
      {1, 2},
      {3, 4},
    };

    // ここより下は、a, b のサイズが任意の場合でも正しく動作する。
    double[,] c = new double[a.GetLength(0), b.GetLength(1)];

    // a×b を計算
    for (int i = 0; i < a.GetLength(0); ++i)
      for (int j = 0; j < b.GetLength(1); ++j)
        for (int k = 0; k < a.GetLength(1); ++k)
          c[i, j] += a[i, k] * b[k, j];

    // a を表示
    Console.Write("a =\n");
    for (int i = 0; i < a.GetLength(0); ++i)
    {
      for (int j = 0; j < a.GetLength(1); ++j)
        Console.Write("{0, 4} ", a[i, j]);
      Console.Write('\n');
    }

    // b を表示
    Console.Write("b =\n");
    for (int i = 0; i < b.GetLength(0); ++i)
    {
      for (int j = 0; j < b.GetLength(1); ++j)
        Console.Write("{0, 4} ", b[i, j]);
      Console.Write('\n');
    }

    // a×b を表示
    Console.Write("a×b =\n");
    for (int i = 0; i < c.GetLength(0); ++i)
    {
      for (int j = 0; j < c.GetLength(1); ++j)
        Console.Write("{0, 4} ", c[i, j]);
      Console.Write('\n');
    }
  }
}
```



## <a id="1233"></a>[関数](../structured/st_function.md)

### <a id="1233-exercise-func0"></a>問題 1


int 型の配列に格納されている値の最大値、最小値および平均値を求める関数をそれぞれ作成せよ。

```csharp
/// <summary>
/// 配列中の値の最大値を求める。
/// </summary>
/// <param name="a">対象の配列</param>
/// <returns>最大値</returns>
static int Max(int[] a)

/// <summary>
/// 配列中の値の最小値を求める。
/// </summary>
/// <param name="a">対象の配列</param>
/// <returns>最小値</returns>
static int Min(int[] a)

/// <summary>
/// 配列中の値の平均値を求める。
/// </summary>
/// <param name="a">対象の配列</param>
/// <returns>平均値</returns>
static double Average(int[] a)
```



#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    // 配列長の入力
    Console.Write("配列の長さ: ");
    int n = int.Parse(Console.ReadLine());

    // 配列の値の入力
    int[] a = new int[n];
    for (int i = 0; i < n; ++i)
    {
      Console.Write("{0}: ", i);
      a[i] = int.Parse(Console.ReadLine());
    }

    Console.Write(
@"
最大値: {0}
最小値: {1}
平均値: {2}
"
    , Max(a), Min(a), Average(a));
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最大値</returns>
  static int Max(int[] a)
  {
    int max = int.MinValue;

    for (int i = 0; i < a.Length; ++i)
    {
      if (max < a[i]) max = a[i];
    }

    return max;
  }

  /// <summary>
  /// 配列中の値の最小値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最小値</returns>
  static int Min(int[] a)
  {
    int min = int.MaxValue;

    for (int i = 0; i < a.Length; ++i)
    {
      if (min > a[i]) min = a[i];
    }

    return min;
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>平均値</returns>
  static double Average(int[] a)
  {
    double average = 0;

    for (int i = 0; i < a.Length; ++i)
    {
      average += a[i];
    }

    return average / a.Length;
  }
}
```



### <a id="1233-exercise-func1"></a>問題 2


double 型の値 x の整数冪を求める関数 Power を作成せよ。

```csharp
/// <summary>
/// x の整数冪を求める。
/// </summary>
/// <param name="x">仮数 x</param>
/// <param name="n">指数 n</param>
/// <returns>x の n 乗</returns>
static double Power(
  double x,
  int n)
```



#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    const double x = 3;
    Console.Write("{0}\n", Power(x, 4));
    Console.Write("{0}\n", Power(x, -1));
    Console.Write("{0}\n", Power(x, -2));
    Console.Write("{0}\n", Power(x, 0));
  }

  /// <summary>
  /// x の整数冪を求める。
  /// </summary>
  /// <param name="x">仮数 x</param>
  /// <param name="n">指数 n</param>
  /// <returns>x の n 乗</returns>
  static double Power(
    double x,
    int n)
  {
    if (n == 0)
      return 1;

    if (n < 0)
    {
      x = 1.0 / x;
      n = -n;
    }

    double y = x;
    while (--n > 0)
    {
      y *= x;
    }

    return y;
  }
}
```



### <a id="1233-exercise-func2"></a>問題 3


配列に格納されている値の最大値を求める関数を、
<code>int[]</code> に対するものと
<code>double[]</code> に対するものの2種類作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    int[]    ai = new int[]    { 1, 3, 9, 2, 5, 6, 4 };
    double[] ad = new double[] { 1, 3, 9, 2, 5, 6, 4 };

    Console.Write("{0}, {1}\n", Max(ai), Max(ad));
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最大値</returns>
  static int Max(int[] a)
  {
    int max = int.MinValue;
    for (int i = 0; i < a.Length; ++i)
    {
      if (max < a[i]) max = a[i];
    }
    return max;
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最大値</returns>
  static double Max(double[] a)
  {
    double max = int.MinValue;
    for (int i = 0; i < a.Length; ++i)
    {
      if (max < a[i]) max = a[i];
    }
    return max;
  }
}
```


見ての通り、型が変わっただけで、処理自体は全く同じものになっています。
このように、型と無関係に同じ処理で実現できるものは、
「[ジェネリック](../oop/sp2_generics.md#generics)」を使うことで実装の手間を軽減できます。


## <a id="1242"></a>[データの構造化(複合型)](../structured/st_struct.md)

### <a id="1242-exercise-str1"></a>問題 1


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



## <a id="1250"></a>[クラス](../oop/oo_class.md)

### <a id="1250-exercise-str1"></a>問題 1


「[データの構造化](../structured/st_struct.md)」の[データの構造化](../structured/st_struct.md)の[問題 1](../structured/st_struct.md#exercise-str1)で作成した <code>Triangle</code> 構造体をクラスで作り直せ。
（<code>Point</code> 構造体は構造体のままで OK。）

注1：現時点では、
単に struct が class に変わるだけで、特にメリットはありませんが、
今後、
「[継承](../oop/oo_inherit.md)」」や「[多態性](../oop/oo_polymorphism.md)」を通して、
クラスのメリットを徐々に加えていく予定です。

注2：
クラスにした場合、メンバー変数をきちんと初期化してやらないと正しく動作しません。
（構造体でもメンバー変数の初期化はきちんとする方がいいんですが。）
初期化に関しては、次節の「[コンストラクターとデストラクター](../oop/oo_construct.md)」で説明します。


## <a id="1252"></a>[コンストラクター](../oop/oo_construct.md)

### <a id="1252-exercise-str1"></a>問題 1


前節[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスに、
以下のようなコンストラクターを追加せよ。

```csharp
/// <summary>
/// 座標値 (x, y) を与えて初期化。
/// </summary>
/// <param name="x">x 座標値</param>
/// <param name="y">y 座標値</param>
public Point(double x, double y)
```


```csharp
/// <summary>
/// 3つの頂点の座標を与えて初期化。
/// </summary>
/// <param name="a">頂点A</param>
/// <param name="b">頂点B</param>
/// <param name="c">頂点C</param>
public Triangle(Point a, Point b, Point c)
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
  public Point a;
  public Point b;
  public Point c;

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



## <a id="1255"></a>[プロパティ](../oop/oo_property.md)

### <a id="1255-exercise-prop1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスの各メンバー変数に対して、
プロパティを使って実装の隠蔽を行え。


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



## <a id="1257"></a>[静的メンバー](../oop/oo_static.md)

### <a id="1257-exercise-static1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体に、
2点間の距離を求める static メソッド <code>GetDistance</code> を追加せよ。

```csharp
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

```csharp
/// <summary>
/// 三角形の周の長さを求める。
/// </summary>
/// <returns>周</returns>
public double GetPerimeter()
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



## <a id="1259"></a>[演算子のオーバーロード](../oop/oo_operator.md)

### <a id="1259-exercise-opeover1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体を2次元ベクトルとみなして、
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



## <a id="1263"></a>[多態性](../oop/oo_polymorphism.md)

### <a id="1263-exercise-polim1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Triangle</code> クラスを元に、
以下のような継承構造を持つクラスを作成せよ。

まず、三角形や円等の共通の基底クラスとなる <code>Shape</code> クラスを以下のように作成。

```csharp
/// <summary>
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// </summary>
class Shape
{
  virtual public double GetArea() { return 0; }
  virtual public double GetPerimeter() { return 0; }
}
```


そして、<code>Shape</code> クラスを継承して、
三角形 <code>Triangle</code> クラスと
円 <code>Circle</code> クラスを作成。

```csharp
/// <summary>
/// 2次元空間上の三角形をあらわすクラス
/// </summary>
class Triangle : Shape
```


```csharp
/// <summary>
/// 2次元空間上の円をあらわすクラス
/// </summary>
class Circle : Shape
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
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// </summary>
class Shape
{
  virtual public double GetArea() { return 0; }
  virtual public double GetPerimeter() { return 0; }
}

/// <summary>
/// 2次元空間上の円をあらわすクラス
/// </summary>
class Circle : Shape
{
  Point center;
  double radius;

  #region 初期化

  /// <summary>
  /// 半径を指定して初期化。
  /// </summary>
  /// <param name="r">半径。</param>
  public Circle(Point center, double r)
  {
    this.center = center;
    this.radius = r;
  }

  #endregion
  #region プロパティ

  /// <summary>
  /// 円の中心。
  /// </summary>
  public Point Center
  {
    get { return this.center; }
    set { this.center = value; }
  }

  /// <summary>
  /// 円の半径。
  /// </summary>
  public double Radius
  {
    get { return this.radius; }
    set { this.radius = value; }
  }

  #endregion
  #region 面積・周

  /// <summary>
  /// 円の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public override double GetArea()
  {
    return Math.PI * this.radius * this.radius;
  }

  /// <summary>
  /// 円の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public override double GetPerimeter()
  {
    return 2 * Math.PI * this.radius;
  }

  #endregion

  public override string ToString()
  {
    return string.Format(
      "Circle (c = {0}, r = {1})",
      this.center, this.radius);
  }
}

/// <summary>
/// 2次元空間上の三角形をあらわすクラス
/// </summary>
class Triangle : Shape
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
  #region 面積・周

  /// <summary>
  /// 三角形の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public override double GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    return 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  /// <summary>
  /// 三角形の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public override double GetPerimeter()
  {
    double l = Point.GetDistance(this.a, this.b);
    l += Point.GetDistance(this.a, this.c);
    l += Point.GetDistance(this.b, this.c);
    return l;
  }

  #endregion

  public override string ToString()
  {
    return string.Format(
      "Circle (a = {0}, b = {1}, c = {2})",
      this.a, this.b, this.c);
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

    Circle c = new Circle(
      new Point(0, 0), 3);

    Show(t);
    Show(c);
  }

  static void Show(Shape f)
  {
    Console.Write("{0}\n", f);
    Console.Write("{0}\n", f.GetArea());
    Console.Write("{0}\n", f.GetPerimeter());
  }
}
```



## <a id="1267"></a>[抽象メソッド、抽象クラス](../oop/oo_abstract.md)

### <a id="1267-exercise-abst1"></a>問題 1


[多態性](../oop/oo_polymorphism.md)の[問題 1](../oop/oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスを抽象クラス化せよ。


#### 解答例 1


必要な箇所（Shape クラスの部分）だけ抜粋。

```csharp
/// <summary>
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// </summary>
abstract class Shape
{
  public abstract double GetArea();
  public abstract double GetPerimeter();
}
```



## <a id="1269"></a>[インターフェース](../oop/oo_interface.md)

### <a id="1269-exercise-if1"></a>問題 1


[多態性](../oop/oo_polymorphism.md)の[問題 1](../oop/oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスをインターフェース化せよ。

<code>Triangle</code> や <code>Shape</code> 関係の例題は一応、これで完成形。

余力があれば、楕円、長方形、平行四辺形、（任意の頂点の）多角形等、さまざまな図形クラスを作成せよ。
また、これらの図形の面積と周の比を計算するプログラムを作成せよ。


#### 解答例 1


三角形、円に加え、多角形を実装した物を示します。

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
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// </summary>
interface Shape
{
  double GetArea();
  double GetPerimeter();
}

/// <summary>
/// 2次元空間上の円をあらわすクラス
/// </summary>
class Circle : Shape
{
  Point center;
  double radius;

  #region 初期化

  /// <summary>
  /// 半径を指定して初期化。
  /// </summary>
  /// <param name="r">半径。</param>
  public Circle(Point center, double r)
  {
    this.center = center;
    this.radius = r;
  }

  #endregion
  #region プロパティ

  /// <summary>
  /// 円の中心。
  /// </summary>
  public Point Center
  {
    get { return this.center; }
    set { this.center = value; }
  }

  /// <summary>
  /// 円の半径。
  /// </summary>
  public double Radius
  {
    get { return this.radius; }
    set { this.radius = value; }
  }

  #endregion
  #region 面積・周

  /// <summary>
  /// 円の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public double GetArea()
  {
    return Math.PI * this.radius * this.radius;
  }

  /// <summary>
  /// 円の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public double GetPerimeter()
  {
    return 2 * Math.PI * this.radius;
  }

  #endregion

  public override string ToString()
  {
    return string.Format(
      "Circle (c = {0}, r = {1})",
      this.center, this.radius);
  }
}

/// <summary>
/// 2次元空間上の三角形をあらわすクラス
/// </summary>
class Triangle : Shape
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
  #region 面積・周

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

  #endregion

  public override string ToString()
  {
    return string.Format(
      "Circle (a = {0}, b = {1}, c = {2})",
      this.a, this.b, this.c);
  }
}

/// <summary>
/// 自由多角形をあらわすクラス
/// </summary>
class Polygon : Shape
{
  Point[] verteces; // 頂点

  #region 初期化

  /// <summary>
  /// 座標を与えて初期化。
  /// </summary>
  /// <param name="verteces">頂点の座標の入った配列</param>
  public Polygon(params Point[] verteces)
  {
    this.verteces = verteces;
  }

  #endregion
  #region プロパティ

  /// <summary>
  /// 頂点の集合。
  /// </summary>
  public Point[] Verteces
  {
    get { return this.verteces; }
    set { this.verteces = value; }
  }

  #endregion
  #region 面積・周

  /// <summary>
  /// 三角形の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public double GetArea()
  {
    double area = 0;
    Point p = this.verteces[this.verteces.Length - 1];
    for (int i = 0; i < this.verteces.Length; ++i)
    {
      Point q = this.verteces[i];
      area += p.X * q.Y - q.X * p.Y;
      p = q;
    }
    return 0.5 * Math.Abs(area);
  }

  /// <summary>
  /// 三角形の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public double GetPerimeter()
  {
    double perimeter = 0;
    Point p = this.verteces[this.verteces.Length - 1];
    for (int i = 0; i < this.verteces.Length; ++i)
    {
      Point q = this.verteces[i];
      perimeter += Point.GetDistance(p, q);
      p = q;
    }
    return perimeter;
  }

  #endregion

  public override string ToString()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    sb.AppendFormat("Polygon ({0}", this.verteces[0]);
    for (int i = 1; i < this.verteces.Length; ++i)
    {
      sb.AppendFormat(", {0}", this.verteces[i]);
    }
    sb.Append(")");

    return sb.ToString();
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

    Circle c = new Circle(
      new Point(0, 0), 3);

    Polygon p1 = new Polygon(
      new Point(0, 0),
      new Point(3, 4),
      new Point(4, 3));

    Polygon p2 = new Polygon(
      new Point(0, 0),
      new Point(0, 2),
      new Point(2, 2),
      new Point(2, 0));

    Show(t);
    Show(c);
    Show(p1);
    Show(p2);
  }

  static void Show(Shape f)
  {
    Console.Write("図形 {0}\n", f);
    Console.Write("面積/周 = {0}\n", f.GetArea() / f.GetPerimeter());
  }
}
```



## <a id="1353"></a>[グラフィック](../lib/lib_drawing.md)

### <a id="1353-exercise-draw1"></a>問題 1

[GUI 雛形プログラム（Graphic 用）](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/DrawImage)をベースに、
画面上を何か図形が動き回るようなプログラムを作成せよ。
Windows のスクリーンセーバー「ライン アート」のような物を目指すとよい。

#### 解答例 1


[GUI 雛形プログラム（Graphic 用）](../../../../assets/source/DrawImage.zip)自体が1つの回答例。
