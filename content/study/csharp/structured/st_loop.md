---
title: "反復処理"
source_url: "https://ufcpp.net/study/csharp/structured/st_loop/"
content_type: "Article"
published_at: "2015-05-06T14:08:31"
updated_at: "2019-03-03T19:32:52"
tags: []
umbraco_id: 1225
parent_id: 1217
sort_order: 3
aliases:
  - "/csharp/st_loop"
  - "/csharp/st_loop.html"
  - "/csharp/structured/st_loop/"
  - "/study/csharp/st_loop"
  - "/study/csharp/st_loop.html"
---

# 反復処理

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

プログラム中で、条件が満たされるまで何度も同じ処理を繰り返したい場面がよくあります。
C#ではそういう反復処理のために<code>while</code>、<code>do</code>、<code>for</code>、<code>foreach</code> などのキーワードを用意しています。
(このうち、<code>foreach</code> は「[foreach](../data/sp_foreach.md)」のところでより詳しく説明します。)


##### <a id="sec-generated-title-2"></a>ポイント

* while (条件) 条件が真の間繰り返し

* do 条件にかかわらず、最低1度は実行される while (条件);

* for (初期化; 条件; 更新) 反復したい処理

* foreach (変数 in コレクション) コレクション内の要素の列挙



## <a id="sec-generated-title-3"></a> <a id="while"></a>while 文

<strong id="while" class="keyword">while</strong> 文は以下のような書き方をします。

```csharp
while(条件式)
  繰り返したい文 // 条件式が真の間繰り返される
```


if と同じく、英文法に近い書き方になっています。
while A, B （A の間、B）。

<code>while</code> の後ろの括弧内の条件式が真の間ずっと文が実行されます。

また、ループを途中で抜けたい場合には、<em>
        <code>break</code>
      </em> を、
ループの先頭に戻りたい場合は <em>
        <code>continue</code>
      </em> を使用します。
以下に <code>break</code> と <code>continue</code> の書式を示します。

```csharp
while(true) // 条件式が常に true なので、永久ループになる。
{
  // 何らかの処理

  break;

  // break よりも後ろの処理は実行されない。
}
// break 文が実行されると処理がここに移る。
```


```csharp
while(true) // continue 文が実行されると条件式の判定から処理をやり直す。
{
  // 何らかの処理

  continue;

  // continue よりも後ろの処理は実行されない。
}
```



##### <a id="sec-generated-title-4"></a>サンプル

```csharp
using System;

class WhileSample
{
  static void Main()
  {
    int a, b;

    // 整数を2つ入力してもらう
    Console.Write("1つ目の整数を入力してください : ");
    a = int.Parse(Console.ReadLine());
    Console.Write("2つ目の整数を入力してください : ");
    b = int.Parse(Console.ReadLine());

    Console.Write("{0}と{1}の最大公約数は", a, b);

    // ユークリッド互除法を使ってaとbの最大公約数を求める
    while(b != 0)
    {
      // b が 0 になるまで繰り返し実行される
      int r = a % b;
      a = b;
      b = r;
    }

    // 結果を出力
    Console.Write("{0}", a);
  }
}
```


```console
1つ目の整数を入力してください : 504
2つ目の整数を入力してください : 210
504と210の最大公約数は42
```



## <a id="sec-generated-title-5"></a> <a id="dowhile"></a>do-while 文

<strong id="do" class="keyword">do-while</strong> 文は while 文と似たような書き方をします。

```csharp
do
  繰り返したい文 // 条件式が真の間繰り返される
while(条件式);
```


A while B（B の間、A）。
英語の場合、while みたいな接続詞は本来、後ろにある方が自然なようで。

do-while 文は while 文と異なり、最低1回は文が実行されます。
つまり、while 文は条件式の評価を行ってから文を実行するのに対し、do-while 文は文を実行してから条件式を評価します。


##### <a id="sec-generated-title-6"></a>サンプル

```csharp
using System;

class DoWhileSample
{
  static void Main()
  {
    int n;

    do
    {
      // 数値を入力してもらう
      Console.Write("1～5のいずれかの数値を入力してください : ");
      n = int.Parse(Console.ReadLine());
    }
    while(n < 1 || n > 5); // nの値が1～5の範囲に入るまで繰り返し

    Console.Write("あなたの入力した数値は{0}です", n);
  }
}
```


```console
1～5のいずれかの数値を入力してください : 6
1～5のいずれかの数値を入力してください : -3
1～5のいずれかの数値を入力してください : 2
あなたの入力した数値は2です
```



## <a id="sec-generated-title-7"></a> <a id="for"></a>for 文

反復処理を行うとき、多くの場合、反復前の初期化、条件式の評価、反復ごとに変数を更新という3つの作業を行います。
例えば、10回同じ処理を繰り返したい場合、
整数型の変数 <code>i</code> を用意し、
反復前に <code>i</code> を0にセット(初期化)、
<code>i</code> が10未満の間(条件式の評価)、<code>i</code> を1ずつ増加させる(更新)といった処理を行います。

C# には、この3つの作業を行うために<strong id="for" class="keyword">for</strong> 文というものが用意されています。

```csharp
for(初期化式; 条件式; 更新式)
  反復を行いたい文
```


「1 から n までの整数 i に対して A が成り立つ」みたいな文章は、英語では "A for integer i from 1 to n" と言ったりします。

for 文では、反復処理に入る前に1度だけ初期化式が実行されます。
その後、条件式を評価し、条件を満たさなければループを抜けます。
そして、1回の反復が終わるたびに更新式が実行され、次の反復に移ります。
これと同様のことを while 文を用いて行うと以下のようになります。

```csharp
初期化式;
while(条件式)
{
  反復を行いたい文

FOR_END: // continue の代わりに goto FOR_END とする必要あり。
  更新式
}
```



##### <a id="sec-generated-title-8"></a>サンプル

```csharp
using System;

class WhileSample
{
  static void Main()
  {
    //九九表を作成
    for(int x=1; x<=9; ++x) // xを1～9まで、1ずつ増やして繰り返し
    {
      for(int y=1; y<=9; ++y) // yを1～9まで、1ずつ増やして繰り返し
      {
        // xy の値を、幅をそろえて表示
        Console.Write((x*y).ToString().PadLeft(3, ' '));
      }
      Console.Write("\n");
    }
  }
}
```


```console
  1  2  3  4  5  6  7  8  9
  2  4  6  8 10 12 14 16 18
  3  6  9 12 15 18 21 24 27
  4  8 12 16 20 24 28 32 36
  5 10 15 20 25 30 35 40 45
  6 12 18 24 30 36 42 48 54
  7 14 21 28 35 42 49 56 63
  8 16 24 32 40 48 56 64 72
  9 18 27 36 45 54 63 72 81
```



## <a id="sec-generated-title-9"></a> <a id="foreach"></a>foreach文

最も頻繁に使われる反復処理は配列の全ての要素に対して読み書きを行うことです。
(配列と言うものについては「[配列](st_array.md)」で詳しく説明します。)
例えば、配列に格納された値の平均値を求める場合、以下のようにします。

```csharp
double Average(double[] a)
{
  double y = 0;
  for(int i=0; i<a.Length; ++i)
  {
    y += a[i];
  }
  return y / a.Length;
}
```


C#には配列の全ての要素にアクセスするための専用の <strong id="foreach" class="keyword">foreach</strong> 文という構文があります。
foreach とは、
"for each element in an array" (配列中のそれぞれの要素に対して処理を行う)という意味です。
foreach 文は以下のよな書き方をします。

```csharp
foreach(変数宣言 in 配列名)
  繰り返したい文 // 配列中の各要素に対して1回ずつ処理が行われる。
```


上述の例を foreach 文を使って書き直すと以下のようになります。

```csharp
double Average(double[] a)
{
  double y = 0;
  foreach(double x in a)
  {
    y += x;
  }
  return y / a.Length;
}
```


ちなみに、foreach 文は配列だけでなく、任意のコレクションクラス(リストや辞書など、複数の要素をひとつにまとめるクラスのこと)に対して使用することが出来ます。
(詳細は「[foreach](../data/sp_foreach.md)」で説明します。)

### <a id="sec-generated-title-10"></a> <a id="query"></a>クエリ式/LINQ

C# 3.0 以降には、「`foreach` の[式](miscexpressions.md#term)版」とも言えるクエリ式という構文もあります。
式なので戻り値が必須なのと、内部的な挙動は実はだいぶ`foreach`とは異なるんですが、似たような結果を得られます。

```csharp
using System.Linq;
 
class Program
{
    static void Main()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
 
        // foreach で奇数の2乗の和
        var sum1 = 0;
        foreach (var x in array)
            if (x % 2 == 1)
                sum1 += x * x;
 
        // クエリ式で奇数の2乗の和
        var sum2 = (
            from x in array
            where x % 2 == 1
            select x * x
            ).Sum();
 
        // sum2 と同じ処理を、単にメソッド呼び出しで実装
        var sum3 = array
            .Where(x => x % 2 == 1)
            .Sum(x => x * x);
    }
}
```

詳しくは「[LINQ](../data/sp3_linq.md)」で説明します。
## <a id="exercise"></a>演習問題

### <a id="exercise-loop0"></a>問題 1


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



### <a id="exercise-loop1"></a>問題 2


平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。
[条件分岐](st_branch.md)の[問題 2](st_branch.md#exercise-branch0)と異なり、判別できる数値に上限は設けない。

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


### <a id="exercise-loop2"></a>問題 3


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
