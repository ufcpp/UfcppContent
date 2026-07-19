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

<pre class="source" title="演算子のオーバーロードの方法" lang="">
<code><span class="reserved">public static</span> <span class="input">戻り値の型</span> <span class="reserved">operator</span><span class="input">演算子</span> (<span class="input">引数リスト</span>)
</code></pre>


例えば、これまでに例としてあげてきた複素数クラスに加算演算子 <code>+</code> を定義したい場合、
以下のように書きます。

<pre class="source" title="Complexクラスの加算演算子" lang="">
<code><span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public static</span> <span class="type">Complex</span> <span class="reserved">operator</span>+ (<span class="type">Complex</span> z, <span class="type">Complex</span> w)
    {
        <span class="reserved">return new</span> <span class="type">Complex</span>(z.Re + w.Re, z.Im + w.Im);
    }
    <span class="comment">// 残りの部分は省略</span>
}
</code></pre>


演算子の定義は必ず public かつ static にする必要があります。

引数リストは、
<code>+</code>, <code>-</code>, <code>*</code>, <code>/</code>
などの2項演算子なら2つ、
<code>++</code>, <code>--</code>, <code>!</code>, <code>~</code>
などの単項演算子なら1つの引数を指定します。

演算子をオーバーロードできるといっても、C# の文法を変えてしまうようなオーバーロードはできません。
たとえば、2項演算子である <code>/</code> 演算子を単項演算子としてオーバーロードすることはできません。

また、引数のうち少なくとも1つの型は演算子を定義するクラス自身である必要があります。

<pre class="source" title="Complexクラスの加算演算子(悪い例2)" lang="">
<code><span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="comment">// ↓この2つはOK。</span>
    <span class="reserved">public static</span> <span class="type">Complex</span> <span class="reserved">operator</span>+ (<span class="type">Complex</span> z, <span class="reserved">double</span> w)
    {
        <span class="reserved">return new</span> <span class="type">Complex</span>(z.Re + w, z.Im);
    }
    <span class="reserved">public static</span> <span class="type">Complex</span> <span class="reserved">operator</span>+ (<span class="reserved">double</span> z, <span class="type">Complex</span> w)
    {
        <span class="reserved">return new</span> <span class="type">Complex</span>(z + w.Re, w.Im);
    }

    <span class="comment">// ↓エラー。引数の少なくともどちらか一方は Complex でないと駄目。</span>
    <span class="reserved">public static</span> <span class="type">Complex</span> <span class="reserved">operator</span>+ (<span class="reserved">double</span> z, <span class="reserved">double</span> w)
    {
        <span class="reserved">return new</span> <span class="type">Complex</span>(z + w, 0);
    }

    <span class="comment">// 残りの部分は省略</span>
}
</code></pre>



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

<pre class="source" title="true, false 演算子" lang="">
<code><span class="reserved">class</span> Bool
{
  <span class="reserved">int</span> i;
  <span class="reserved">public</span> Bool(<span class="reserved">int</span> i){<span class="reserved">this</span>.i = i;}
  <span class="reserved">public static bool operator true</span>(Bool b){<span class="reserved">return</span> b.i != 0;}
  <span class="reserved">public static bool operator false</span>(Bool b){<span class="reserved">return</span> b.i == 0;}
}

<span class="reserved">class</span> OperatorSample
{
  <span class="reserved">static void</span> Main()
  {
    Bool b = <span class="reserved">new</span> Bool(0);

    <span class="reserved">if</span>(b) <span class="comment">// 条件式として利用できる</span>
      Console.Write(<span class="literal">"b == true"</span>);
    <span class="reserved">else</span>
      Console.Write(<span class="literal">"b == false"</span>);
  }
}
</code></pre>


<pre class="console" title="">
b==false
</pre>

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


<pre class="source" title="++, -- 演算子" lang="">
<code><span class="reserved">class</span> Counter
{
  <span class="reserved">int</span> i;
  <span class="reserved">public</span> Counter(<span class="reserved">int</span> i){<span class="reserved">this</span>.i = i;}
  <span class="reserved">public static</span> Counter <span class="reserved">operator</span> ++(Counter c)
  {
    <span class="comment">// c を直接書き換えては駄目。
    // インスタンスのコピーを作る。。</span>
    Counter tmp = <span class="reserved">new</span> Counter(c.i + 1);
    <span class="reserved">return</span> tmp;
  }
  <span class="reserved">public override string</span> ToString(){<span class="reserved">return this</span>.i.ToString();}
}

<span class="reserved">class</span> OperatorSample
{
  <span class="reserved">static void</span> Main()
  {
    Counter c = <span class="reserved">new</span> Counter(0);

    Console.Write(c++ + <span class="literal">"\n"</span>);
    <span class="comment">//↑ Counter tmp = c; c = Counter.operator++(c); return tmp;</span>
    Console.Write(c   + <span class="literal">"\n"</span>);
    Console.Write(++c + <span class="literal">"\n"</span>);
    <span class="comment">//↑ c = Counter.operator++(c); return c;</span>
    Console.Write(c   + <span class="literal">"\n"</span>);
  }
}
</code></pre>


<pre class="console" title="">
0
1
2
2
</pre>



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

<pre class="source" title="&amp;&amp;, || 演算子" lang="">
<code><span class="reserved">class</span> Bool
{
  <span class="reserved">int</span> i;
  <span class="reserved">public</span> Bool(<span class="reserved">int</span> i){<span class="reserved">this</span>.i = i==0 ? 0 : 1;}
  <span class="reserved">public static bool operator true</span>(Bool b)
  {
    Console.Write(<span class="literal">"  operator true called\n"</span>);
    <span class="reserved">return</span> b.i != 0;
  }
  <span class="reserved">public static bool operator false</span>(Bool b)
  {
    Console.Write(<span class="literal">"  operator false called\n"</span>);
    <span class="reserved">return</span> b.i == 0;
  }
  <span class="reserved">public static</span> Bool <span class="reserved">operator</span> &amp;(Bool a, Bool b)
  {
    Console.Write(<span class="literal">"  operator &amp; called\n"</span>);
    <span class="reserved">return new</span> Bool(a.i &amp; b.i);
  }
  <span class="reserved">public static</span> Bool <span class="reserved">operator</span> |(Bool a, Bool b)
  {
    Console.Write(<span class="literal">"  operator | called\n"</span>);
    <span class="reserved">return new</span> Bool(a.i | b.i);
  }
}

<span class="reserved">class</span> OperatorSample
{
  <span class="reserved">static void</span> Main()
  {
    Bool a = <span class="reserved">new</span> Bool(1);
    Bool b = <span class="reserved">new</span> Bool(0);

    Bool c;
    Console.Write(<span class="literal">"a &amp;&amp; b\n"</span>);
    c = a &amp;&amp; b;
    Console.Write(<span class="literal">"b &amp;&amp; a\n"</span>);
    c = b &amp;&amp; a;
    Console.Write(<span class="literal">"a || b\n"</span>);
    c = a || b;
    Console.Write(<span class="literal">"b || a\n"</span>);
    c = b || a;
  }
}
</code></pre>


<pre class="console" title="">
a &amp;&amp; b
  operator false called
  operator &amp; called
b &amp;&amp; a
  operator false called
a || b
  operator true called
b || a
  operator true called
  operator | called
</pre>



### <a id="sec-generated-title-9"></a> <a id="assignment"></a>複合代入演算

(C# 13 までは) [複合代入演算子](../start/st_operator.md#compound-assignment)は直接オーバーロードすることは出来ませんが、
対応する2項演算子をオーバーロードすることで利用可能になります。

([詳細は後述しますが](#overload-compound)、C# 14 からは複合代入演算子のオーバーロードができるようにないました。)

例えば、<code>+</code> 演算子をオーバーロードした型は、
<code>x += y</code> とすることで、
<code>x = x + y</code> と同じ結果が得られます。

### <a id="sec-generated-title-10"></a> <a id="cast"></a>型変換演算

<strong id="cast" class="keyword">型変換</strong>（cast）演算子は以下のようにして定義します。

<pre class="source" title="型変換演算子の定義の仕方" lang="">
<code><span class="reserved">public static</span> <span class="input"><span class="reserved">explicit</span>または<span class="reserved">implicit</span></span> operator <span class="input">変換先の型</span> (<span class="input">変換元の型</span> <span class="input">引数名</span>)
{
  <span class="comment">// 変換コード</span>
}
</code></pre>


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

<pre class="source" title="型変換演算子" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Counter
{
  <span class="reserved">int</span> i;

  <span class="reserved">public</span> Counter(<span class="reserved">int</span> i){<span class="reserved">this</span>.i=i;}

  <span class="reserved">public static explicit operator</span> Counter (<span class="reserved">int</span> i){<span class="reserved">return new</span> Counter(i);}
  <span class="reserved">public static explicit operator int</span> (Counter c){<span class="reserved">return</span> c.i;}
  <span class="reserved">public override string</span> ToString(){<span class="reserved">return</span> <span class="literal">"count="</span>+<span class="reserved">this</span>.i;}
}

<span class="reserved">class</span> OperatorSample
{
  <span class="reserved">static void</span> Main()
  {
    Counter c = <span class="reserved">new</span> Counter(1);
    Console.Write((<span class="reserved">int</span>)c + <span class="literal">"\n"</span>);
    Console.Write((Counter)2 + <span class="literal">"\n"</span>);
  }
}
</code></pre>


<pre class="console" title="">
1
count=2
</pre>

## <a id="sec-generated-title-11"></a> <a id="parameter"></a>演算子の引数

(C# 7.1 以前では) 演算子の引数は[値渡し](../resource/sp_ref.md#sec-byval)である必要があります。

<pre class="source" title="値渡しはOK。参照渡し(ref引数)はNG">
<code><span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> Complex(<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// これは OK</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Complex <span class="reserved">operator</span> +(Complex a, Complex b)
        =&gt; <span class="reserved">new</span> Complex(a.X + b.X, a.Y + b.Y);

    <span class="comment">// これはコンパイル エラーになる</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Complex <span class="reserved">operator</span> <span class="error">+</span>(<span class="reserved">ref</span> Complex a, <span class="reserved">ref</span> Complex b)
        =&gt; <span class="reserved">new</span> Complex(a.X + b.X, a.Y + b.Y);
}
</code></pre>

<h5 class="version version7">Ver. 7.2</h5>

C# 7.2 で[`in` 引数](../resource/sp_ref.md#in)という機能が入りましたが、
同時に、演算子の引数にこの`in`引数が使えるようになりました。

<pre class="source" title="in であればOK">
<code><span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> Complex(<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// これなら OK</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Complex <span class="reserved">operator</span> +(<span class="reserved">in</span> Complex a, <span class="reserved">in</span> Complex b)
        =&gt; <span class="reserved">new</span> Complex(a.X + b.X, a.Y + b.Y);
}
</code></pre>

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

<pre class="source" title="C# 13 までの演算子オーバーロードのコスト">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// 小さい構造体は + のコストが低い。</span>
<span class="reserved">struct</span> <span class="type struct">SmallStruct</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">SmallStruct</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type struct">SmallStruct</span> <span class="variable local">a</span>, <span class="type struct">SmallStruct</span> <span class="variable local">b</span>)
    {
        <span class="comment">// new SmallStruct(value) のコストも、それを戻り値で返すコストも小さい。</span>
        <span class="control">return</span> <span class="reserved">new</span>(<span class="variable local">a</span><span class="operator">.</span><span class="field">Value</span> <span class="operator">+</span> <span class="variable local">b</span><span class="operator">.</span><span class="field">Value</span>);
    }
}

<span class="comment">// クラスだと new() のコストが問題に。</span>
<span class="reserved">class</span> <span class="type">Class</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Class</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type">Class</span> <span class="variable local">a</span>, <span class="type">Class</span> <span class="variable local">b</span>)
    {
        <span class="comment">// クラスだと new Class(value) のコストが大きい。</span>
        <span class="control">return</span> <span class="reserved">new</span>(<span class="variable local">a</span><span class="operator">.</span><span class="field">Value</span> <span class="operator">+</span> <span class="variable local">b</span><span class="operator">.</span><span class="field">Value</span>);
    }
}

<span class="comment">// int 10個分のフィールドを持つ構造体(大きい)。</span>
<span class="comment">// 大きい構造体ではコピーのコストが高い。</span>
[<span class="type">InlineArray</span>(<span class="number">10</span>)]
<span class="reserved">struct</span> <span class="type struct">LargeStruct</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">LargeStruct</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="reserved">in</span> <span class="type struct">LargeStruct</span> <span class="variable local">a</span>, <span class="reserved">in</span> <span class="type struct">LargeStruct</span> <span class="variable local">b</span>)
    {
        <span class="comment">// int 10個分のスタックを確保して、</span>
        <span class="type struct">LargeStruct</span> <span class="variable">result</span> <span class="operator">=</span> <span class="reserved">default</span>;
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="number">10</span>; <span class="variable">i</span><span class="operator">++</span>)
            <span class="variable">result</span>[<span class="variable">i</span>] <span class="operator">+=</span> <span class="variable local">a</span>[<span class="variable">i</span>] <span class="operator">+</span> <span class="variable local">b</span>[<span class="variable">i</span>];

        <span class="comment">// さらに、戻り値で返す時にコピーが発生。</span>
        <span class="control">return</span> <span class="variable">result</span>;
    }
}
</pre>

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

<pre class="source" title="複合代入演算子のオーバーロードの例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">struct</span> <span class="type struct">SmallStruct</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="type struct">SmallStruct</span> <span class="variable local">a</span>)
    {
        <span class="comment">// 自己書き換え。</span>
        <span class="field">Value</span> <span class="operator">+=</span> <span class="variable local">a</span><span class="operator">.</span><span class="field">Value</span>;
    }
}

<span class="reserved">class</span> <span class="type">Class</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="type">Class</span> <span class="variable local">a</span>)
    {
        <span class="comment">// 自己書き換えならクラスでもコスト低め。</span>
        <span class="field">Value</span> <span class="operator">+=</span> <span class="variable local">a</span><span class="operator">.</span><span class="field">Value</span>;
    }
}

<span class="comment">// int 10個分のフィールドを持つ構造体(大きい)。</span>
<span class="comment">// 大きい構造体ではコピーのコストが高い。</span>
[<span class="type">InlineArray</span>(<span class="number">10</span>)]
<span class="reserved">struct</span> <span class="type struct">LargeStruct</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">in</span> <span class="type struct">LargeStruct</span> <span class="variable local">a</span>)
    {
        <span class="comment">// 自己書き換えなら大きめの構造体でもコスト低め。</span>
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="number">10</span>; <span class="variable">i</span><span class="operator">++</span>)
            <span class="reserved">this</span>[<span class="variable">i</span>] <span class="operator">+=</span> <span class="variable local">a</span>[<span class="variable">i</span>];
    }
}
</pre>

ちなみに、「`+` があれば `+=` 利用可能」だった二項演算子のオーバーロードと違って、
`+=` だけあっても `+` は使えません。

<pre class="source" title="+= だけあっても + は使えない">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">X</span>(<span class="number">5</span>);

<span class="comment">// += はできる。</span>
<span class="variable">x</span> += <span class="number">10</span>;

<span class="comment">// 二項演算の + はダメ。</span>
<span class="variable">x</span> <span class="operator">=</span> <span class="error" title="CS0019"><span class="variable">x</span> <span class="operator">+</span> <span class="number">10</span></span>;

<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>;
}
</pre>

`+=`, `-=`, `*=`, `/=`, `%=`, `&=`, `|=`, `^=`, `<<=`, `>>=`, `>>>=` のオーバーロードが可能です。
このうち、`+=`, `-=`, `*=`, `/=` は [`checked`](generic-math-operators.md#checked-operator-overload) オーバーロードもできます。

<pre class="source" title="オーバーロード可能な複合代入演算子">
<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">-=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">-=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">*=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">*=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">/=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">/=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">%=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">%=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&amp;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&amp;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">|=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">|=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">^=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">^=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&lt;&lt;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&gt;&gt;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&gt;&gt;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&gt;&gt;&gt;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&gt;&gt;&gt;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">-=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">*=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">/=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
}
</pre>

また、同じく自己書き換えなので、インクリメント `++` とデクリメント `--` もインスタンス メンバーとしてオーバーロードできるようになりました
(これらも [`checked`](generic-math-operators.md#checked-operator-overload) にできます)。

<pre class="source" title="インクリメントとデクリメントの自己書き換えオーバーロード">
<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">++</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">++</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">--</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">--</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">++</span>() { <span class="reserved">checked</span> { <span class="property">Value</span><span class="operator">++</span>; } }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">--</span>() { <span class="reserved">checked</span> { <span class="property">Value</span><span class="operator">--</span>; } }
}
</pre>

ただ、この自己書き換え版のインクリメント/デクリメントは後起き版(書き換える前の値を残す必要がある)の利用に難があります。
基本的には後起きインクリメント/デクリメントには使えません。

<pre class="source" title="後起きインクリメントはダメ">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">X</span>(<span class="number">1</span>);

<span class="comment">// 前置きはどこでも書ける。</span>
<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> ++<span class="variable">x</span>;

<span class="comment">// 後起きはダメ。コンパイル エラーになる。</span>
<span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="error" title="CS0023"><span class="variable">x</span><span class="operator">++</span></span>;

<span class="comment">// ただし… 単文で書くときは後起きでも問題ない。</span>
<span class="comment">// (書き換え前の値を残す必要がないのでセーフ。)</span>
++<span class="variable">x</span>;
<span class="variable">x</span>++;

<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">++</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">++</span>;
}
</pre>

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

<pre class="source" title="二項演算子と複合代入演算子に整合性がないと変なことになる例">
<span class="reserved">var</span> <span class="variable">x1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">X</span>(<span class="number">1</span>);
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(++<span class="variable">x1</span>); <span class="comment">// インスタンス ++ が呼ばれる。</span>

<span class="reserved">var</span> <span class="variable">x2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">X</span>(<span class="number">1</span>);
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">x2</span>++; <span class="comment">// static ++ が呼ばれる。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x2</span>);

<span class="reserved">var</span> <span class="variable">x3</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">X</span>(<span class="number">1</span>);
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x3</span> += <span class="number">1</span>); <span class="comment">// インスタンス += が呼ばれる。</span>

<span class="reserved">var</span> <span class="variable">x4</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">X</span>(<span class="number">1</span>);
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x4</span> + <span class="number">1</span>); <span class="comment">// static + が呼ばれる。</span>

<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">++</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">++</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">X</span> <span class="reserved">operator</span> <span class="operator">++</span>(<span class="type struct">X</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">x</span><span class="operator">.</span><span class="property">Value</span> <span class="operator">-</span> <span class="number">1</span>); <span class="comment">// わざと変な実装。</span>

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">v</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">v</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">X</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type struct">X</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">v</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">x</span><span class="operator">.</span><span class="property">Value</span> <span class="operator">-</span> <span class="variable local">v</span>); <span class="comment">// わざと変な実装。</span>
}
</pre>

<pre class="console">
X { Value = 2 }
X { Value = 0 }
X { Value = 2 }
X { Value = 0 }
</pre>

不整合を避けるために、以下のように、
複合代入演算子を先に実装して、二項演算子の方は「コピー + 複合代入」で実装するのがいいのではないかと思われます。

<pre class="source" title="複合代入演算子を先に実装して、二項演算子からはそれを呼ぶ">
<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">++</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">++</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">X</span> <span class="reserved">operator</span> <span class="operator">++</span>(<span class="type struct">X</span> <span class="variable local">x</span>) <span class="comment">// 後起き ++ 用。</span>
    {
        <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// コピー。</span>
        ++<span class="variable">y</span>; <span class="comment">// インスタンス ++ を呼び出す。</span>
        <span class="control">return</span> <span class="variable">y</span>;
    }

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">v</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">v</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">X</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type struct">X</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">v</span>)
    {
        <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// コピー。</span>
        <span class="variable">y</span> += <span class="variable local">v</span>; <span class="comment">// インスタンス += を呼び出す。</span>
        <span class="control">return</span> <span class="variable">y</span>;
    }
}
</pre>

### <a id="sec-generated-title-16"></a> <a id="compound-virtual">余談: virtual</a>

複合代入演算子のオーバーロードはインスタンス メンバーなので、一応、`virtual` や `abstract` にできます。

<pre class="source" title="複合代入演算子は virtual/abstract にできる">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();
<span class="method"><span class="static">SumTo5</span></span>(<span class="variable">x</span>);
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span><span class="operator">.</span><span class="field">Value</span>); <span class="comment">// 15</span>

<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">B</span>();
<span class="static"><span class="method">SumTo5</span></span>(<span class="variable">y</span>);
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">y</span><span class="operator">.</span><span class="field">Value</span>); <span class="comment">// 120</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">SumTo5</span></span>(<span class="type">Base</span> <span class="variable local">x</span>)
{
    <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">1</span>; <span class="variable">i</span> <span class="operator">&lt;=</span> <span class="number">5</span>; <span class="variable">i</span><span class="operator">++</span>) <span class="variable local">x</span> += <span class="variable">i</span>;
}

<span class="comment">// += の実装を派生クラスごとに変えれる。</span>
<span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span>;
    <span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="reserved">operator</span><span class="operator">+=</span> (<span class="reserved">int</span> <span class="variable local">value</span>);
}

<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Base</span>
{
    <span class="comment">// 普通に和にする。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>;
}

<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Base</span>
{
    <span class="comment">// + を積にしてしまう。</span>
    <span class="reserved">public</span> <span class="type">B</span>() <span class="operator">=&gt;</span> <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">Value</span> <span class="operator">*=</span> <span class="variable local">value</span>;
}
</pre>

(書きかけ)
## <a id="exercise"></a>演習問題

### <a id="exercise-opeover1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体を2次元ベクトルとみなして、
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
