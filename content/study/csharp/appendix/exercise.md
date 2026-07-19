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


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Sample
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"岩永信之"</span>);
  }
}
</code></pre>



### <a id="1195-exercise-console2"></a>問題 2


Console.ReadLine を用いて文字列を1行読み込み、
Console.Write を用いて読んだ文字列をそのまま鸚鵡返しするプログラムを作成せよ。

おまけ： 1度読み込んだ文字列を2度ずつ鸚鵡返しするものを作成せよ。


#### 解答例 1


<pre class="source" title="鸚鵡返し" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Sample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">string</span> line = Console.ReadLine();
    Console.Write(line);
  }
}
</code></pre>



#### 解答例 2


<pre class="source" title="鸚鵡返し×2" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Sample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">string</span> line = Console.ReadLine();
    Console.Write(line);
    Console.Write(line);
  }
}
</code></pre>



## <a id="1203"></a>[組込み演算子](../start/st_operator.md)

### <a id="1203-exercise-ope1"></a>問題 1


2つの整数を入力し、
その整数の四則演算(＋, －, ×, ÷)結果を表示するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="整数の四則演算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"input a: "</span>);
    <span class="reserved">int</span> a = <span class="reserved">int</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"input b: "</span>);
    <span class="reserved">int</span> b = <span class="reserved">int</span>.Parse(Console.ReadLine());

    Console.Write(<span class="literal">"{0} + {1} = {2}\n"</span>, a, b, a + b);
    Console.Write(<span class="literal">"{0} - {1} = {2}\n"</span>, a, b, a - b);
    Console.Write(<span class="literal">"{0} * {1} = {2}\n"</span>, a, b, a * b);
    Console.Write(<span class="literal">"{0} / {1} = {2}\n"</span>, a, b, a / b);
  }
}
</code></pre>



### <a id="1203-exercise-ope2"></a>問題 2


前問の「整数の四則演算」の、 double, short 等の他の型を用いた物を作成せよ。


#### 解答例 1


例として double 版を掲載。

<pre class="source" title="実数の四則演算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"input a: "</span>);
    <span class="reserved">double</span> a = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"input b: "</span>);
    <span class="reserved">double</span> b = <span class="reserved">double</span>.Parse(Console.ReadLine());

    Console.Write(<span class="literal">"{0} + {1} = {2}\n"</span>, a, b, a + b);
    Console.Write(<span class="literal">"{0} - {1} = {2}\n"</span>, a, b, a - b);
    Console.Write(<span class="literal">"{0} * {1} = {2}\n"</span>, a, b, a * b);
    Console.Write(<span class="literal">"{0} / {1} = {2}\n"</span>, a, b, a / b);
  }
}
</code></pre>



### <a id="1203-exercise-ope3"></a>問題 3


複素数 x + iy の逆数を求めるプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"実部を入力してください: "</span>);
    <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"虚部を入力してください: "</span>);
    <span class="reserved">double</span> y = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> norm = x * x + y * y;

    Console.Write(<span class="literal">"{0} + i({1}) の逆数は {2} + i({3})\n)"</span>,
      x, y,
      x / norm, -y / norm);
  }
}
</code></pre>



### <a id="1203-exercise-ope4"></a>問題 4


半径を入力し、その半径の円の面積を求めるプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="円の面積を求める" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">double</span> r; <span class="comment">// 半径</span>

    Console.Write(<span class="literal">"半径を入力してください: "</span>);
    r = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> area = r * r * 3.1415926535897932;
    Console.Write(<span class="literal">"面積 = {0}\n"</span>, area);
  }
}
</code></pre>



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

<pre class="console" title="結果の例">
身長[cm] = <span class="input">175.5</span>
体重[kg] = <span class="input">52.4</span>
BMI = 17.0128489216808
</pre>



#### 解答例 1


<pre class="source" title="BMI 値の計算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">double</span> height; <span class="comment">// 身長[cm]</span>
    <span class="reserved">double</span> weight; <span class="comment">// 体重[kg]</span>

    Console.Write(<span class="literal">"身長[cm]: "</span>);
    height = <span class="reserved">double</span>.Parse(Console.ReadLine());
    height *= 0.01; <span class="comment">// cm → m</span>

    Console.Write(<span class="literal">"体重[kg]: "</span>);
    weight = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> bmi = weight / (height * height);
    Console.Write(<span class="literal">"BMI = {0}\n"</span>, bmi);
  }
}
</code></pre>



## <a id="1209"></a>[組込み型変換](../start/st_cast.md)

### <a id="1209-exercise-cast1"></a>問題 1


適当な文字を入力し、その文字コードを表示するプログラムを作成せよ。
（char 型の変数を int 型にキャストすると文字コードが得られます。）


#### 解答例 1


<pre class="source" title="文字コードの表示" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">char</span> c;

    Console.Write(<span class="literal">"文字を入力してください: "</span>);
    c = Console.ReadLine()[0];

    Console.Write(<span class="literal">"文字 {0} の文字コードは {1}\n"</span>, c, (<span class="reserved">int</span>)c);
  }
}
</code></pre>



### <a id="1209-exercise-cast2"></a>問題 2


整数型（int, short, long）同士の割り算では、結果も整数となり、あまりは切り捨てられます。
切り捨てられると困る場合、浮動小数点数（double, float）にキャストしてから計算する必要があります。

このことを確かめるため、
2つの整数を入力し、
整数のままで割り算した結果（あまり切り捨て）と、
浮動小数点数として割り算した結果を比較するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="整数と浮動小数点数の割り算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"input a: "</span>);
    <span class="reserved">int</span> a = <span class="reserved">int</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"input b: "</span>);
    <span class="reserved">int</span> b = <span class="reserved">int</span>.Parse(Console.ReadLine());

    Console.Write(<span class="literal">"整数: {0} / {1} = {2} … {3}\n"</span>, a, b, a / b, a % b);
    Console.Write(<span class="literal">"実数: {0} / {1} = {2}\n"</span>, a, b, a / (<span class="reserved">double</span>)b);
  }
}
</code></pre>



### <a id="1209-exercise-cast3"></a>問題 3


double → int にキャストすると、値が整数に切り詰められます。
このとき、どのようにして値が切り詰められるのか（切捨てなのか切り上げなのか等）を調べよ。
（正の数だけでなく、負の数も。）


#### 解答例 1


<pre class="source" title="double → int" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// まず、正の数をいくつか確認。</span>
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, 3.8, (<span class="reserved">int</span>)3.8);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, 3.1, (<span class="reserved">int</span>)3.1);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, 2.7, (<span class="reserved">int</span>)2.7);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, 2.4, (<span class="reserved">int</span>)2.4);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, 1.5, (<span class="reserved">int</span>)1.5);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, 0.5, (<span class="reserved">int</span>)0.5);
    <span class="comment">// 負の数も。</span>
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, -3.8, (<span class="reserved">int</span>)-3.8);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, -3.1, (<span class="reserved">int</span>)-3.1);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, -2.7, (<span class="reserved">int</span>)-2.7);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, -2.4, (<span class="reserved">int</span>)-2.4);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, -1.5, (<span class="reserved">int</span>)-1.5);
    Console.Write(<span class="literal">"{0} → {1}\n"</span>, -0.5, (<span class="reserved">int</span>)-0.5);
  }
}
</code></pre>


<pre class="console" title="double → int">
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
</pre>


結果を見ての通り、正の数は切り捨て、負の数は切り上げ（0 に向かって丸め）になります。

正負問わず値を切り捨てたい場合は <code>Math.Floor</code> 関数を、
切り上げたい場合は <code>Math.Ceiling</code> 関数を、
四捨五入したい場合は <code>Math.Round</code> 関数を使用します。


## <a id="1220"></a>[条件分岐](../structured/st_branch.md)

### <a id="1220-exercise-brancheo"></a>問題 1


ユーザから入力された整数が奇数か偶数か判定するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="奇数・偶数の判定" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"整数を入力してください: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="reserved">if</span> (n % 2 == 0) Console.Write(<span class="literal">"{0} は偶数です\n"</span>, n);
    <span class="reserved">else</span>            Console.Write(<span class="literal">"{0} は奇数です\n"</span>, n);
  }
}
</code></pre>



### <a id="1220-exercise-branch0"></a>問題 2


[組込み演算子](../start/st_operator.md)の[問題 5](../start/st_operator.md#exercise-ope5)のプログラムを修正し、
BMI 値から体型(やせ型、普通、やや肥満、肥満、高度肥満)を判定し、
表示するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="BMI 値の計算と体型の判定" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"身長[cm]: "</span>);
    <span class="reserved">double</span> height = <span class="reserved">double</span>.Parse(Console.ReadLine()) * 0.01;
    Console.Write(<span class="literal">"体重[kg]: "</span>);
    <span class="reserved">double</span> weight = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> bmi = weight / (height * height);
    Console.Write(<span class="literal">"BMI = {0}\n"</span>, bmi);

    <span class="reserved">if</span>(bmi &lt; 19.8)      Console.Write(<span class="literal">"やせ型"</span>);
    <span class="reserved">else if</span>(bmi &lt; 24.2) Console.Write(<span class="literal">"普通"</span>);
    <span class="reserved">else if</span>(bmi &lt; 26.4) Console.Write(<span class="literal">"やや肥満（過体重）"</span>);
    <span class="reserved">else if</span>(bmi &lt; 35.0) Console.Write(<span class="literal">"肥満"</span>);
    <span class="reserved">else</span>                Console.Write(<span class="literal">"高度肥満（要治療）"</span>);
    Console.Write(<span class="literal">"です\n"</span>);
  }
}
</code></pre>



### <a id="1220-exercise-branch1"></a>問題 3


switch 文を使って150以下の平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。

ヒント：
要するに、ユーザからの入力が 1, 4, 9, 16, ・・・になっているかどうかを switch 文で判別します。


#### 解答例 1


<pre class="source" title="平方数の判別" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"整数を入力してください: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="reserved">switch</span> (n)
    {
      <span class="reserved">case</span> 1:
        Console.Write(<span class="literal">"{0} は平方数です。\n"</span>, n);
        <span class="reserved">break</span>;
      <span class="reserved">case</span> 2 * 2: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 3 * 3: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 4 * 4: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 5 * 5: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 6 * 6: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 7 * 7: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 8 * 8: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 9 * 9: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 10 * 10: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 11 * 11: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 12 * 12: <span class="reserved">goto case</span> 1;
      <span class="reserved">default</span>:
        Console.Write(<span class="literal">"{0} は平方数ではないか、150以上です\n"</span>, n);
        <span class="reserved">break</span>;
    }
  }
}
</code></pre>



### <a id="1220-exercise-branch2"></a>問題 4


数値を3つ入力してもらい、
その3つの値の中の最大値、最小値を求めるプログラムを作成せよ。


#### 解答例 1


単純な条件分岐による方法。

<pre class="source" title="最大値、最小値" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"値1: "</span>);
    <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値2: "</span>);
    <span class="reserved">double</span> y = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値3: "</span>);
    <span class="reserved">double</span> z = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">if</span> (x &gt; y)
    {
      <span class="reserved">if</span> (x &gt; z)
      {
        <span class="reserved">if</span> (y &gt; z) Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, x, y, z);
        <span class="reserved">else</span>       Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, x, z, y);
      }
      <span class="reserved">else</span> Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, z, x, y);
    }
    <span class="reserved">else</span>
    {
      <span class="reserved">if</span> (y &gt; z)
      {
        <span class="reserved">if</span> (x &gt; z) Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, y, x, z);
        <span class="reserved">else</span>       Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, y, z, x);
      }
      <span class="reserved">else</span> Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, z, y, x);
    }
  }
}
</code></pre>



#### 解答例 2


3つの数値をあらかじめ整列してしまう方法。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"値1: "</span>);
    <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値2: "</span>);
    <span class="reserved">double</span> y = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値3: "</span>);
    <span class="reserved">double</span> z = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> tmp;

    <span class="reserved">if</span> (y &lt; z) { tmp = y; y = z; z = tmp; }
    <span class="reserved">if</span> (x &lt; y) { tmp = x; x = y; y = tmp; }
    <span class="reserved">if</span> (y &lt; z) { tmp = y; y = z; z = tmp; }

    Console.Write(<span class="literal">"最大 {0}, 中間{1}, 最小 {2}\n"</span>, x, y, z);
  }
}</code></pre>



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


<pre class="source" title="整数の和" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"n: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());
    <span class="reserved">int</span> sum = 0;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt;= n; ++i)
    {
      sum += i;
    }

    Console.Write(<span class="literal">"loop {0}, formula {1}\n"</span>, sum, n * (n + 1) / 2);
  }
}
</code></pre>



### <a id="1225-exercise-loop1"></a>問題 2


平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。
[条件分岐](../structured/st_branch.md)の[問題 2](../structured/st_branch.md#exercise-branch0)と異なり、判別できる数値に上限は設けない。

ヒント：ループと条件分岐を組み合わせて作る。


#### 解答例 1


<pre class="source" title="平方数の判別" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"整数を入力してください: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());
    <span class="reserved">int</span> i;

    <span class="reserved">for</span> (i = 0; i &lt;= n; ++i)
    {
      <span class="reserved">if</span> (n == i * i) <span class="reserved">break</span>;
    }

    <span class="reserved">if</span> (i &lt;= n) Console.Write(<span class="literal">"{0} = {1} × {1} は平方数です\n"</span>, n, i);
    <span class="reserved">else</span>        Console.Write(<span class="literal">"{0} は平方数ではありません\n"</span>, n);
  }
}
</code></pre>


ちなみに、この for ループの継続条件の部分は、
<code>i &lt;= (int)Math.Sqrt(n)</code> でも OK。
（その下の if 文の条件も変更する必要あり。）
Sqrt は n の平方根を求める関数。


### <a id="1225-exercise-loop2"></a>問題 3


2重ループを使って掛け算の九九表を表示するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="九九表" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt;= 9; ++i)
    {
      <span class="reserved">for</span> (<span class="reserved">int</span> j = 1; j &lt;= 9; ++j)
      {
        Console.Write(<span class="literal">"{0,3}"</span>, i * j);
      }
      Console.Write('\n');
    }
  }
}
</code></pre>



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


<pre class="source" title="数列計算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span>[] a = <span class="reserved">new int</span>[21];
    a[0] = 3;
    a[1] = 1;

    <span class="comment">// 数列を求める。</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 2; i &lt; a.Length; ++i)
    {
      a[i] = 2 * a[i - 1] - 2 * a[i - 2];
    }

    <span class="comment">// 求めた数列を表示。</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      Console.Write(<span class="literal">"{0} "</span>, a[i]);
    }
    Console.Write('\n');
  }
}
</code></pre>



### <a id="1229-exercise-array1"></a>問題 2


int 型の配列に格納されている値の最大値、最小値および平均値を求めよ。
できれば、配列の長さ n および n 個の整数値をユーザに入力してもらうようにすること。


#### 解答例 1


<pre class="source" title="配列の最大値、最小値、平均値" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// 配列長の入力</span>
    Console.Write(<span class="literal">"配列の長さ: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="comment">// 配列の値の入力</span>
    <span class="reserved">int</span>[] a = <span class="reserved">new int</span>[n];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; n; ++i)
    {
      Console.Write(<span class="literal">"{0}: "</span>, i);
      a[i] = <span class="reserved">int</span>.Parse(Console.ReadLine());
    }

    <span class="comment">// 最大値、最小値、平均値の計算</span>
    <span class="reserved">int</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">int</span> min = <span class="reserved">int</span>.MaxValue;
    <span class="reserved">double</span> average = 0;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; n; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
      <span class="reserved">if</span> (min &gt; a[i]) min = a[i];
      average += a[i];
    }
    average /= n;

    Console.Write(
<span class="literal">@"
最大値: {0}
最小値: {1}
平均値: {2}
"</span>
    , max, min, average);
  }
}
</code></pre>



### <a id="1229-exercise-array2"></a>問題 3


double 型の2次元配列を行列に見立てて、行列の掛け算を行うプログラムを作成せよ。


#### 解答例 1


行列の次元は任意だけども、例として2×2行列の場合を示す。

<pre class="source" title="行列の積" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">double</span>[,] a = <span class="reserved">new double</span>[,]
    {
      {1, 1},
      {1, 0},
    };
    <span class="reserved">double</span>[,] b = <span class="reserved">new double</span>[,]
    {
      {1, 2},
      {3, 4},
    };

    <span class="comment">// ここより下は、a, b のサイズが任意の場合でも正しく動作する。</span>
    <span class="reserved">double</span>[,] c = <span class="reserved">new double</span>[a.GetLength(0), b.GetLength(1)];

    <span class="comment">// a×b を計算</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.GetLength(0); ++i)
      <span class="reserved">for</span> (<span class="reserved">int</span> j = 0; j &lt; b.GetLength(1); ++j)
        <span class="reserved">for</span> (<span class="reserved">int</span> k = 0; k &lt; a.GetLength(1); ++k)
          c[i, j] += a[i, k] * b[k, j];

    <span class="comment">// a を表示</span>
    Console.Write(<span class="literal">"a =\n"</span>);
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.GetLength(0); ++i)
    {
      <span class="reserved">for</span> (<span class="reserved">int</span> j = 0; j &lt; a.GetLength(1); ++j)
        Console.Write(<span class="literal">"{0, 4} "</span>, a[i, j]);
      Console.Write('\n');
    }

    <span class="comment">// b を表示</span>
    Console.Write(<span class="literal">"b =\n"</span>);
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; b.GetLength(0); ++i)
    {
      <span class="reserved">for</span> (<span class="reserved">int</span> j = 0; j &lt; b.GetLength(1); ++j)
        Console.Write(<span class="literal">"{0, 4} "</span>, b[i, j]);
      Console.Write('\n');
    }

    <span class="comment">// a×b を表示</span>
    Console.Write(<span class="literal">"a×b =\n"</span>);
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; c.GetLength(0); ++i)
    {
      <span class="reserved">for</span> (<span class="reserved">int</span> j = 0; j &lt; c.GetLength(1); ++j)
        Console.Write(<span class="literal">"{0, 4} "</span>, c[i, j]);
      Console.Write('\n');
    }
  }
}
</code></pre>



## <a id="1233"></a>[関数](../structured/st_function.md)

### <a id="1233-exercise-func0"></a>問題 1


int 型の配列に格納されている値の最大値、最小値および平均値を求める関数をそれぞれ作成せよ。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 配列中の値の最大値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;returns&gt;最大値&lt;/returns&gt;</span>
<span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)

<span class="comment">/// &lt;summary&gt;
/// 配列中の値の最小値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;returns&gt;最小値&lt;/returns&gt;</span>
<span class="reserved">static int</span> Min(<span class="reserved">int</span>[] a)

<span class="comment">/// &lt;summary&gt;
/// 配列中の値の平均値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;returns&gt;平均値&lt;/returns&gt;</span>
<span class="reserved">static double</span> Average(<span class="reserved">int</span>[] a)
</code></pre>



#### 解答例 1


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// 配列長の入力</span>
    Console.Write(<span class="literal">"配列の長さ: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="comment">// 配列の値の入力</span>
    <span class="reserved">int</span>[] a = <span class="reserved">new int</span>[n];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; n; ++i)
    {
      Console.Write(<span class="literal">"{0}: "</span>, i);
      a[i] = <span class="reserved">int</span>.Parse(Console.ReadLine());
    }

    Console.Write(
<span class="literal">@"
最大値: {0}
最小値: {1}
平均値: {2}
"</span>
    , Max(a), Min(a), Average(a));
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最大値&lt;/returns&gt;</span>
  <span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> max = <span class="reserved">int</span>.MinValue;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
    }

    <span class="reserved">return</span> max;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最小値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最小値&lt;/returns&gt;</span>
  <span class="reserved">static int</span> Min(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> min = <span class="reserved">int</span>.MaxValue;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (min &gt; a[i]) min = a[i];
    }

    <span class="reserved">return</span> min;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;平均値&lt;/returns&gt;</span>
  <span class="reserved">static double</span> Average(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">double</span> average = 0;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      average += a[i];
    }

    <span class="reserved">return</span> average / a.Length;
  }
}
</code></pre>



### <a id="1233-exercise-func1"></a>問題 2


double 型の値 x の整数冪を求める関数 Power を作成せよ。

<pre class="source" title="Power の仕様" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// x の整数冪を求める。
/// &lt;/summary&gt;
/// &lt;param name="x"&gt;仮数 x&lt;/param&gt;
/// &lt;param name="n"&gt;指数 n&lt;/param&gt;
/// &lt;returns&gt;x の n 乗&lt;/returns&gt;</span>
<span class="reserved">static double</span> Power(
  <span class="reserved">double</span> x,
  <span class="reserved">int</span> n)
</code></pre>



#### 解答例 1


<pre class="source" title="Power の実装とテスト" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">const double</span> x = 3;
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, 4));
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, -1));
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, -2));
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, 0));
  }

  <span class="comment">/// &lt;summary&gt;
  /// x の整数冪を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;仮数 x&lt;/param&gt;
  /// &lt;param name="n"&gt;指数 n&lt;/param&gt;
  /// &lt;returns&gt;x の n 乗&lt;/returns&gt;</span>
  <span class="reserved">static double</span> Power(
    <span class="reserved">double</span> x,
    <span class="reserved">int</span> n)
  {
    <span class="reserved">if</span> (n == 0)
      <span class="reserved">return</span> 1;

    <span class="reserved">if</span> (n &lt; 0)
    {
      x = 1.0 / x;
      n = -n;
    }

    <span class="reserved">double</span> y = x;
    <span class="reserved">while</span> (--n &gt; 0)
    {
      y *= x;
    }

    <span class="reserved">return</span> y;
  }
}
</code></pre>



### <a id="1233-exercise-func2"></a>問題 3


配列に格納されている値の最大値を求める関数を、
<code>int[]</code> に対するものと
<code>double[]</code> に対するものの2種類作成せよ。


#### 解答例 1


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span>[]    ai = <span class="reserved">new int</span>[]    { 1, 3, 9, 2, 5, 6, 4 };
    <span class="reserved">double</span>[] ad = <span class="reserved">new double</span>[] { 1, 3, 9, 2, 5, 6, 4 };

    Console.Write(<span class="literal">"{0}, {1}\n"</span>, Max(ai), Max(ad));
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最大値&lt;/returns&gt;</span>
  <span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
    }
    <span class="reserved">return</span> max;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最大値&lt;/returns&gt;</span>
  <span class="reserved">static double</span> Max(<span class="reserved">double</span>[] a)
  {
    <span class="reserved">double</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
    }
    <span class="reserved">return</span> max;
  }
}
</code></pre>


見ての通り、型が変わっただけで、処理自体は全く同じものになっています。
このように、型と無関係に同じ処理で実現できるものは、
「[ジェネリック](../oop/sp2_generics.md#generics)」を使うことで実装の手間を軽減できます。


## <a id="1242"></a>[データの構造化(複合型)](../structured/st_struct.md)

### <a id="1242-exercise-str1"></a>問題 1


サンプル中の Point 構造体を使って、三角形を表す構造体 <code>Triangle</code> を作成せよ。
（3つの頂点を a, b, c 等のメンバー変数として持つ。）

また、作成した構造体に、三角形の面積を求めるメンバー関数 <code>GetArea</code>を追加せよ。

<pre class="source" title="GetArea 仕様" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 三角形の面積を求める。
/// &lt;/summary&gt;
/// &lt;returns&gt;面積&lt;/returns&gt;</span>
<span class="reserved">public double</span> GetArea()
</code></pre>



#### 解答例 1


<pre class="source" title="Triangle 構造体" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">public double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">public double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Triangle
{
  <span class="reserved">public</span> Point a;
  <span class="reserved">public</span> Point b;
  <span class="reserved">public</span> Point c;

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> abx, aby, acx, acy;
    abx = b.x - a.x;
    aby = b.y - a.y;
    acx = c.x - a.x;
    acy = c.y - a.y;
    <span class="reserved">return</span> 0.5 * Math.Abs(abx * acy - acx * aby);
  }
}

<span class="reserved">class</span> Test
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t;
    t.a.x = 0;
    t.a.y = 0;
    t.b.x = 3;
    t.b.y = 4;
    t.c.x = 4;
    t.c.y = 3;
    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
  }
}
</code></pre>



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

<pre class="source" title="Point クラスコンストラクター" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 座標値 (x, y) を与えて初期化。
/// &lt;/summary&gt;
/// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
/// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
<span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
</code></pre>


<pre class="source" title="Triangle クラスコンストラクター" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 3つの頂点の座標を与えて初期化。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;頂点A&lt;/param&gt;
/// &lt;param name="b"&gt;頂点B&lt;/param&gt;
/// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
<span class="reserved">public</span> Triangle(Point a, Point b, Point c)
</code></pre>



#### 解答例 1


<pre class="source" title="Point/Triangle クラス" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">public double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">public double</span> y; <span class="comment">// y 座標

  /// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle
{
  <span class="reserved">public</span> Point a;
  <span class="reserved">public</span> Point b;
  <span class="reserved">public</span> Point c;

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> abx, aby, acx, acy;
    abx = b.x - a.x;
    aby = b.y - a.y;
    acx = c.x - a.x;
    acy = c.y - a.y;
    <span class="reserved">return</span> 0.5 * Math.Abs(abx * acy - acx * aby);
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
  }
}
</code></pre>



## <a id="1255"></a>[プロパティ](../oop/oo_property.md)

### <a id="1255-exercise-prop1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスの各メンバー変数に対して、
プロパティを使って実装の隠蔽を行え。


#### 解答例 1


<pre class="source" title="Point/Triangle" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> abx, aby, acx, acy;
    abx = b.X - a.X;
    aby = b.Y - a.Y;
    acx = c.X - a.X;
    acy = c.Y - a.Y;
    <span class="reserved">return</span> 0.5 * Math.Abs(abx * acy - acx * aby);
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
  }
}
</code></pre>



## <a id="1257"></a>[静的メンバー](../oop/oo_static.md)

### <a id="1257-exercise-static1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体に、
2点間の距離を求める static メソッド <code>GetDistance</code> を追加せよ。

<pre class="source" title="GetDistance" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// A-B 間の距離を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;点A&lt;/param&gt;
/// &lt;param name="b"&gt;点B&lt;/param&gt;
/// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
<span class="reserved">public static double</span> GetDistance(Point a, Point b)
</code></pre>


また、<code>GetDistance</code> を用いて、
<code>Triangle</code> クラスに三角形の周を求めるメソッド
<code>GetPerimeter</code> を追加せよ。

<pre class="source" title="GetPerimeter" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 三角形の周の長さを求める。
/// &lt;/summary&gt;
/// &lt;returns&gt;周&lt;/returns&gt;</span>
<span class="reserved">public double</span> GetPerimeter()
</code></pre>



#### 解答例 1


<pre class="source" title="Point/Triangle" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// A-B 間の距離を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
  <span class="reserved">public static double</span> GetDistance(Point a, Point b)
  {
    <span class="reserved">double</span> x = a.x - b.x;
    <span class="reserved">double</span> y = a.y - b.y;
    <span class="reserved">return</span> Math.Sqrt(x * x + y * y);
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> abx, aby, acx, acy;
    abx = b.X - a.X;
    aby = b.Y - a.Y;
    acx = c.X - a.X;
    acy = c.Y - a.Y;
    <span class="reserved">return</span> 0.5 * Math.Abs(abx * acy - acx * aby);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">double</span> l = Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.b);
    l += Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.c);
    l += Point.GetDistance(<span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
    <span class="reserved">return</span> l;
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
    Console.Write(<span class="literal">"{0}\n"</span>, t.GetPerimeter());
  }
}
</code></pre>



## <a id="1259"></a>[演算子のオーバーロード](../oop/oo_operator.md)

### <a id="1259-exercise-opeover1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Point</code> 構造体を2次元ベクトルとみなして、
ベクトルの和・差を計算する演算子 <code>+</code> および <code>-</code> を追加せよ。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// ベクトル和
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;点A&lt;/param&gt;
/// &lt;param name="b"&gt;点B&lt;/param&gt;
/// &lt;returns&gt;和&lt;/returns&gt;</span>
<span class="reserved">public static</span> Point <span class="reserved">operator</span> +(Point a, Point b)

<span class="comment">/// &lt;summary&gt;
/// ベクトル差
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;点A&lt;/param&gt;
/// &lt;param name="b"&gt;点B&lt;/param&gt;
/// &lt;returns&gt;和&lt;/returns&gt;</span>
<span class="reserved">public static</span> Point <span class="reserved">operator</span> -(Point a, Point b)
</code></pre>



#### 解答例 1


<pre class="source" title="Point/Triangle" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion
  #region</span> 演算子

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル和
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> +(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x + b.x, a.y + b.y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル差
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> -(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x - b.x, a.y - b.y);
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// A-B 間の距離を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
  <span class="reserved">public static double</span> GetDistance(Point a, Point b)
  {
    <span class="reserved">double</span> x = a.x - b.x;
    <span class="reserved">double</span> y = a.y - b.y;
    <span class="reserved">return</span> Math.Sqrt(x * x + y * y);
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    <span class="reserved">return</span> 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">double</span> l = Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.b);
    l += Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.c);
    l += Point.GetDistance(<span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
    <span class="reserved">return</span> l;
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
    Console.Write(<span class="literal">"{0}\n"</span>, t.GetPerimeter());
  }
}
</code></pre>



## <a id="1263"></a>[多態性](../oop/oo_polymorphism.md)

### <a id="1263-exercise-polim1"></a>問題 1


[クラス](../oop/oo_class.md)の[問題 1](../oop/oo_class.md#exercise-str1)の <code>Triangle</code> クラスを元に、
以下のような継承構造を持つクラスを作成せよ。

まず、三角形や円等の共通の基底クラスとなる <code>Shape</code> クラスを以下のように作成。

<pre class="source" title="Shape" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Shape
{
  <span class="reserved">virtual public double</span> GetArea() { <span class="reserved">return</span> 0; }
  <span class="reserved">virtual public double</span> GetPerimeter() { <span class="reserved">return</span> 0; }
}
</code></pre>


そして、<code>Shape</code> クラスを継承して、
三角形 <code>Triangle</code> クラスと
円 <code>Circle</code> クラスを作成。

<pre class="source" title="Triangle" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle : Shape
</code></pre>


<pre class="source" title="Circle" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の円をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Circle : Shape
</code></pre>



#### 解答例 1


<pre class="source" title="Shape、Triangle、Circle" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion
  #region</span> 演算子

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル和
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> +(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x + b.x, a.y + b.y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル差
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> -(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x - b.x, a.y - b.y);
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// A-B 間の距離を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
  <span class="reserved">public static double</span> GetDistance(Point a, Point b)
  {
    <span class="reserved">double</span> x = a.x - b.x;
    <span class="reserved">double</span> y = a.y - b.y;
    <span class="reserved">return</span> Math.Sqrt(x * x + y * y);
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Shape
{
  <span class="reserved">virtual public double</span> GetArea() { <span class="reserved">return</span> 0; }
  <span class="reserved">virtual public double</span> GetPerimeter() { <span class="reserved">return</span> 0; }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の円をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Circle : Shape
{
  Point center;
  <span class="reserved">double</span> radius;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 半径を指定して初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="r"&gt;半径。&lt;/param&gt;</span>
  <span class="reserved">public</span> Circle(Point center, <span class="reserved">double</span> r)
  {
    <span class="reserved">this</span>.center = center;
    <span class="reserved">this</span>.radius = r;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 円の中心。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point Center
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.center; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.center = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の半径。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Radius
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.radius; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.radius = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 円の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetArea()
  {
    <span class="reserved">return</span> Math.PI * <span class="reserved">this</span>.radius * <span class="reserved">this</span>.radius;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetPerimeter()
  {
    <span class="reserved">return</span> 2 * Math.PI * <span class="reserved">this</span>.radius;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (c = {0}, r = {1})"</span>,
      <span class="reserved">this</span>.center, <span class="reserved">this</span>.radius);
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle : Shape
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    <span class="reserved">return</span> 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetPerimeter()
  {
    <span class="reserved">double</span> l = Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.b);
    l += Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.c);
    l += Point.GetDistance(<span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
    <span class="reserved">return</span> l;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (a = {0}, b = {1}, c = {2})"</span>,
      <span class="reserved">this</span>.a, <span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Circle c = <span class="reserved">new</span> Circle(
      <span class="reserved">new</span> Point(0, 0), 3);

    Show(t);
    Show(c);
  }

  <span class="reserved">static void</span> Show(Shape f)
  {
    Console.Write(<span class="literal">"{0}\n"</span>, f);
    Console.Write(<span class="literal">"{0}\n"</span>, f.GetArea());
    Console.Write(<span class="literal">"{0}\n"</span>, f.GetPerimeter());
  }
}
</code></pre>



## <a id="1267"></a>[抽象メソッド、抽象クラス](../oop/oo_abstract.md)

### <a id="1267-exercise-abst1"></a>問題 1


[多態性](../oop/oo_polymorphism.md)の[問題 1](../oop/oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスを抽象クラス化せよ。


#### 解答例 1


必要な箇所（Shape クラスの部分）だけ抜粋。

<pre class="source" title="Shape" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// &lt;/summary&gt;</span>
abstract <span class="reserved">class</span> Shape
{
  <span class="reserved">public</span> abstract <span class="reserved">double</span> GetArea();
  <span class="reserved">public</span> abstract <span class="reserved">double</span> GetPerimeter();
}
</code></pre>



## <a id="1269"></a>[インターフェース](../oop/oo_interface.md)

### <a id="1269-exercise-if1"></a>問題 1


[多態性](../oop/oo_polymorphism.md)の[問題 1](../oop/oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスをインターフェース化せよ。

<code>Triangle</code> や <code>Shape</code> 関係の例題は一応、これで完成形。

余力があれば、楕円、長方形、平行四辺形、（任意の頂点の）多角形等、さまざまな図形クラスを作成せよ。
また、これらの図形の面積と周の比を計算するプログラムを作成せよ。


#### 解答例 1


三角形、円に加え、多角形を実装した物を示します。

<pre class="source" title="さまざまな図形" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion
  #region</span> 演算子

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル和
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> +(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x + b.x, a.y + b.y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル差
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> -(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x - b.x, a.y - b.y);
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// A-B 間の距離を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
  <span class="reserved">public static double</span> GetDistance(Point a, Point b)
  {
    <span class="reserved">double</span> x = a.x - b.x;
    <span class="reserved">double</span> y = a.y - b.y;
    <span class="reserved">return</span> Math.Sqrt(x * x + y * y);
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">interface</span> Shape
{
  <span class="reserved">double</span> GetArea();
  <span class="reserved">double</span> GetPerimeter();
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の円をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Circle : Shape
{
  Point center;
  <span class="reserved">double</span> radius;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 半径を指定して初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="r"&gt;半径。&lt;/param&gt;</span>
  <span class="reserved">public</span> Circle(Point center, <span class="reserved">double</span> r)
  {
    <span class="reserved">this</span>.center = center;
    <span class="reserved">this</span>.radius = r;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 円の中心。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point Center
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.center; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.center = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の半径。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Radius
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.radius; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.radius = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 円の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">return</span> Math.PI * <span class="reserved">this</span>.radius * <span class="reserved">this</span>.radius;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">return</span> 2 * Math.PI * <span class="reserved">this</span>.radius;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (c = {0}, r = {1})"</span>,
      <span class="reserved">this</span>.center, <span class="reserved">this</span>.radius);
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle : Shape
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    <span class="reserved">return</span> 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">double</span> l = Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.b);
    l += Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.c);
    l += Point.GetDistance(<span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
    <span class="reserved">return</span> l;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (a = {0}, b = {1}, c = {2})"</span>,
      <span class="reserved">this</span>.a, <span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
  }
}

<span class="comment">/// &lt;summary&gt;
/// 自由多角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Polygon : Shape
{
  Point[] verteces; <span class="comment">// 頂点</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="verteces"&gt;頂点の座標の入った配列&lt;/param&gt;</span>
  <span class="reserved">public</span> Polygon(<span class="reserved">params</span> Point[] verteces)
  {
    <span class="reserved">this</span>.verteces = verteces;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点の集合。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point[] Verteces
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.verteces; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.verteces = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> area = 0;
    Point p = <span class="reserved">this</span>.verteces[<span class="reserved">this</span>.verteces.Length - 1];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.verteces.Length; ++i)
    {
      Point q = <span class="reserved">this</span>.verteces[i];
      area += p.X * q.Y - q.X * p.Y;
      p = q;
    }
    <span class="reserved">return</span> 0.5 * Math.Abs(area);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">double</span> perimeter = 0;
    Point p = <span class="reserved">this</span>.verteces[<span class="reserved">this</span>.verteces.Length - 1];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.verteces.Length; ++i)
    {
      Point q = <span class="reserved">this</span>.verteces[i];
      perimeter += Point.GetDistance(p, q);
      p = q;
    }
    <span class="reserved">return</span> perimeter;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    System.Text.StringBuilder sb = <span class="reserved">new</span> System.Text.StringBuilder();
    sb.AppendFormat(<span class="literal">"Polygon ({0}"</span>, <span class="reserved">this</span>.verteces[0]);
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt; <span class="reserved">this</span>.verteces.Length; ++i)
    {
      sb.AppendFormat(<span class="literal">", {0}"</span>, <span class="reserved">this</span>.verteces[i]);
    }
    sb.Append(<span class="literal">")"</span>);

    <span class="reserved">return</span> sb.ToString();
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Circle c = <span class="reserved">new</span> Circle(
      <span class="reserved">new</span> Point(0, 0), 3);

    Polygon p1 = <span class="reserved">new</span> Polygon(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Polygon p2 = <span class="reserved">new</span> Polygon(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(0, 2),
      <span class="reserved">new</span> Point(2, 2),
      <span class="reserved">new</span> Point(2, 0));

    Show(t);
    Show(c);
    Show(p1);
    Show(p2);
  }

  <span class="reserved">static void</span> Show(Shape f)
  {
    Console.Write(<span class="literal">"図形 {0}\n"</span>, f);
    Console.Write(<span class="literal">"面積/周 = {0}\n"</span>, f.GetArea() / f.GetPerimeter());
  }
}
</code></pre>



## <a id="1353"></a>[グラフィック](../lib/lib_drawing.md)

### <a id="1353-exercise-draw1"></a>問題 1

[GUI 雛形プログラム（Graphic 用）](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/DrawImage)をベースに、
画面上を何か図形が動き回るようなプログラムを作成せよ。
Windows のスクリーンセーバー「ライン アート」のような物を目指すとよい。

#### 解答例 1


[GUI 雛形プログラム（Graphic 用）](../../../../assets/source/DrawImage.zip)自体が1つの回答例。
