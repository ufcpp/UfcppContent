---
title: "式木（Expression Trees）"
source_url: "https://ufcpp.net/study/csharp/dynamic/sp3_expression/"
content_type: "Article"
published_at: "2008-03-09T00:00:00"
updated_at: "2019-08-03T19:27:58"
tags:
  - "Ver. 3.0"
umbraco_id: 1315
parent_id: 1312
sort_order: 2
aliases:
  - "/csharp/dynamic/sp3_expression/"
  - "/csharp/sp3_expression"
  - "/csharp/sp3_expression.html"
  - "/study/csharp/sp3_expression"
  - "/study/csharp/sp3_expression.html"
---

# 式木（Expression Trees）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version3">Ver. 3.0</h5>

「[ラムダ式](../functional/sp3_lambda.md#lambda)」は、Expression 型の変数に代入すると、
匿名デリゲート（実行可能なコード）ではなく<strong id="expressiontree" class="keyword">式木</strong>（式の意味を表す木構造データ）としてコンパイルされます。 
例えば、以下の2つのコードは同じ意味になります。

<pre class="source" title="ラムダ式から式木を作る" lang="">
<code><span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt; e = x =&gt; x + <span class="literal">5</span>
</code></pre>


<pre class="source" title="直接式木を作る" lang="">
<code><span class="reserved">var</span> x = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">int</span>), <span class="literal">"x"</span>);
<span class="reserved">var</span> e = 
  <span class="type">Expression</span>.Lambda&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt;(
    <span class="type">Expression</span>.Add(x, <span class="type">Expression</span>.Constant(<span class="literal">5</span>)),
    x);
</code></pre>


ここでは、
どういうラムダ式を書くと、どういう式木が得られるのかを簡単に説明していきます。

サンプルコード →

[TypesForTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/TypesForTest.cs)
、

[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
。


## <a id="sec-generated-title-2"></a> <a id="constraints"></a>式木にできるラムダ式の条件

まず先に、式木を使う上での制約について。
ラムダ式ならば何でも式木にできるというわけではありません。

ラムダ式には、以下に例示するような2つの記法、
1文だけのタイプとブロックを持つタイプがあります。

<pre class="source" title="1文だけのラムダ式（式木にできる）" lang="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f = x =&gt; x + <span class="literal">5</span>
</code></pre>


<pre class="source" title="ブロックタイプのラムダ式（式木にできない）" lang="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f = x =&gt;
  {
    <span class="reserved">int</span> p = <span class="literal">1</span>;
    <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="literal">0</span>; i &lt; x; ++i)
      p *= x;
    <span class="reserved">return</span> p;
  }
</code></pre>


前者は、ただ1つだけの式からなっていて、 {} や return を省略できます。
後者は、{} ブロック内に複数の文を並べてかけます。

このうち、<em>式木にできるのは前者（1文だけのラムダ式）だけです</em>。

そうなると、結構強い制約がかかってきます。
例えば、for, while, switch などの制御構文や、x = 0 といったような代入式は式木にできません。
あと、インクリメント・デクリメントも、実質的には加減算＋代入なので、式木にできません。
また、ラムダ式内でローカル変数を定義できません。

一方、C# 3.0 で導入されたオブジェクト初期化子（object initializer）（参考：「[初期化子](../functional/sp3_lambda.md#init)」）を使えば、結構複雑な式も書けたりします。
例えば以下のような感じ。

<pre class="source" title="初期化子を使ったラムダ式" lang="">
<code><span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="type">LineSegment</span>&gt;&gt; e = () =&gt; 
  <span class="reserved">new</span> <span class="type">LineSegment</span>
  {
    Start = { X = <span class="literal">0</span>, Y = <span class="literal">0</span> },
    End   = { X = <span class="literal">1</span>, Y = <span class="literal">1</span> },
  };
</code></pre>



## <a id="sec-generated-title-3"></a> <a id="Expression"></a>Expression 型

前節の例でちょこっと出てきた Expression.Lambda や Expression.Add メソッドによって生成されるのは、
LambdaExpression 型や BinaryExpression 型の変数になりますが、
これらは全て、Expression 型の派生クラスになります。

Expression 型の派生クラスは、直接 new することはできず、
Expression 型の static メソッド（Lambda や Add）を使って生成します。

Expression 型は NodeType というプロパティを持っていて、
例えば、加算なら NodeType == ExpressionType.Add になります。

生成用の static メソッド、
具体的な型、
NodeType がそれぞればらばらで、少し複雑なんですが、
いくつか先に例を示します。

<table summary="Expression 型の例">
	<caption>
		Expression 型の例
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>NodeType</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">+</td>
		<td markdown="1">Add</td>
		<td markdown="1">Add</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">new</td>
		<td markdown="1">New</td>
		<td markdown="1">New</td>
		<td markdown="1">NewExpression</td>
	</tr>
	<tr>
		<td markdown="1">() =&gt; 0</td>
		<td markdown="1">Lambda&lt;Func&lt;int&gt;&gt;</td>
		<td markdown="1">Lambda</td>
		<td markdown="1">LambdaExpression&lt;Func&lt;int&gt;&gt;</td>
	</tr>
</table>


実装上、ほとんどのものが、生成メソッドの名前と、NodeType 列挙子の名前はそろえてあるようです。
（条件演算子とメンバーアクセスだけ例外。
条件演算子は Expression.Condition で生成するけど、NodeType は Conditional。
メンバーアクセスは Expression.MakeMemberAccess で生成するけど、NodeType は MemberAccess。）


## <a id="sec-generated-title-4"></a> <a id="prepare"></a>下準備

百聞は一見にしかずということで、
次節以降では、ラムダ式と式木の対応関係を実例を挙げて紹介していきます。
それに先立って、いくつか補助関数や変数を用意しておきます。

まず、Expression 型を作りやすくするために
（型推論が働きやすくするために）、
以下のような補助関数を用意します。

<pre class="source" title="Expression&lt;T&gt; 型の型推論のための補助関数" lang="">
<code><span class="reserved">static partial class</span> <span class="type">Make</span>
{
    <span class="reserved">public static</span> <span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;TR&gt;&gt; Expression&lt;TR&gt;(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;TR&gt;&gt; e)
    {
        <span class="reserved">return</span> e;
    }

    <span class="reserved">public static</span> <span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, TR&gt;&gt; Expression&lt;T1, TR&gt;(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, TR&gt;&gt; e)
    {
        <span class="reserved">return</span> e;
    }

    <span class="reserved">public static</span> <span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, T2, TR&gt;&gt; Expression&lt;T1, T2, TR&gt;(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, T2, TR&gt;&gt; e)
    {
        <span class="reserved">return</span> e;
    }

    <span class="reserved">public static</span> <span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, T2, T3, TR&gt;&gt; Expression&lt;T1, T2, T3, TR&gt;(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, T2, T3, TR&gt;&gt; e)
    {
        <span class="reserved">return</span> e;
    }

    <span class="reserved">public static</span> <span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, T2, T3, T4, TR&gt;&gt; Expression&lt;T1, T2, T3, T4, TR&gt;(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;T1, T2, T3, T4, TR&gt;&gt; e)
    {
        <span class="reserved">return</span> e;
    }
}</code></pre>


また、（簡易的にではありますが、）
2つの式木が一致するかどうかを判定する関数を用意します。

<pre class="source" title="2つの式木が一致性判定" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 式木の構造が一致してれば、少なくとも ToString の結果は一致するので、
/// それで2つの式木の一致性を判定。
/// &lt;/summary&gt;</span>
<span class="reserved">static void</span> SimpleCheck(<span class="type">Expression</span> e1, <span class="type">Expression</span> e2)
{
    <span class="reserved">if</span> (e1.ToString() != e2.ToString())
    {
        <span class="type">Console</span>.Write(<span class="literal">"not match: {0}, {1}\n"</span>, e1, e2);
    }
}
</code></pre>


さらに、Expression.Parameter は頻繁に出てくるものなので、
あらかじめ Parameter を作って変数に代入しておきます。

<pre class="source" title="ParameterExpression を事前に準備" lang="">
<code><span class="reserved">static</span> <span class="type">ParameterExpression</span> intX = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">int</span>), <span class="literal">"x"</span>);
<span class="reserved">static</span> <span class="type">ParameterExpression</span> intY = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">int</span>), <span class="literal">"y"</span>);
<span class="reserved">static</span> <span class="type">ParameterExpression</span> boolX = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">bool</span>), <span class="literal">"x"</span>);
<span class="reserved">static</span> <span class="type">ParameterExpression</span> boolY = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">bool</span>), <span class="literal">"y"</span>);
</code></pre>


それから、テスト用に、Point, LineSegment, Polyline などの型を定義します →

[TypesForTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/TypesForTest.cs)
。


## <a id="sec-generated-title-5"></a> <a id="lambda"></a>ラムダ式

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の Lambda() メソッド。

ラムダ式そのものは LambdaExpression 型か、
Expression&lt;T&gt; ジェネリック型（LambdaExpression のサブクラス）になります。

Lambda メソッドに、ラムダ式の本体（Body）とパラメータリスト（Paramters）を渡して生成します。
ちなみに、パラメータと定数はそれぞれ、Parameter、Constant メソッドで生成します。

（以後、サンプルコード中では、
SimpleCheck メソッドの1つ目の引数と2つ目の引数が同じ式木になっています。）

<pre class="source" title="Lambda" lang="">
<code><span class="type">SimpleCheck(
  Make</span>.Expression((<span class="reserved">int</span> x) =&gt; <span class="literal">0</span>),
  <span class="type">Expression</span>.Lambda&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt;(
    <span class="type">Expression</span>.Constant(<span class="literal">0</span>), <span class="comment">// Body</span>
    intX) <span class="comment">// Paremters[0]</span>
  );
</code></pre>


ちなみに、ラムダ式中にさらに式木が含まれていた場合、
その式木は Quote で囲まれます。

<pre class="source" title="Quote" lang="">
<code><span class="type">SimpleCheck(
  Make</span>.Expression(() =&gt;
    (<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt;&gt;)(() =&gt; <span class="literal">0</span>)
  ).Body,
  <span class="type">Expression</span>.Convert(
    <span class="type">Expression</span>.Quote(
      (<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt;&gt;)(() =&gt; <span class="literal">0</span>)),
  <span class="reserved">typeof</span>(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt;&gt;))
);
</code></pre>


<table summary="Lambda, Paramter, Constant, Quote">
	<caption>
		Lambda, Paramter, Constant, Quote
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">ラムダ式</td>
		<td markdown="1">Lambda</td>
		<td markdown="1">LambdaExpression（とその派生クラス）</td>
	</tr>
	<tr>
		<td markdown="1">定数</td>
		<td markdown="1">Constant</td>
		<td markdown="1">ConstantExpression</td>
	</tr>
	<tr>
		<td markdown="1">パラメータ</td>
		<td markdown="1">Parameter</td>
		<td markdown="1">ParameterExpression</td>
	</tr>
	<tr>
		<td markdown="1">式木</td>
		<td markdown="1">Quote</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-6"></a> <a id="arithmetic"></a>算術演算

`+` や `-` などの C# 組込み演算子には、それぞれ対応する式木があります。


### <a id="sec-generated-title-7"></a> <a id="unaryarithmetic"></a>単項演算

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の ArithmeticUnaryOperator() メソッド。

算術演算には、オーバーフローのチェックを行うかどうかで2つのバージョンがあります。

<pre class="source" title="checked/unchecked" lang="">
<code><span class="type">SimpleCheck(
  Make</span>.Expression((<span class="reserved">int</span> x) =&gt; -x).Body,
  <span class="type">Expression</span>.Negate(intX)
);
SimpleCheck(
  <span class="type">Make</span>.Expression((<span class="reserved">int</span> x) =&gt; <span class="reserved">checked</span>(-x)).Body,
  <span class="type">Expression</span>.NegateChecked(intX)
);
</code></pre>


int などに単項 + を適用すると、最適化されて + が消えてしまうので注意。
ユーザ定義型の + の場合はちゃんと + が残ります。

<pre class="source" title="単項 +" lang="">
<code><span class="comment">// ↓これは最適化がかかって +x が x になる。</span>
<span class="type">SimpleCheck(
  Make</span>.Expression((<span class="reserved">int</span> x) =&gt; +x).Body,
  intX
);
SimpleCheck(
  <span class="type">Make</span>.Expression((CustomUnaryPlus x) =&gt; +x).Body,
  <span class="type">Expression</span>.UnaryPlus(<span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(CustomUnaryPlus), <span class="literal">"x"</span>))
);
</code></pre>


<table summary="単項算術演算">
	<caption>
		単項算術演算
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">単項 +</td>
		<td markdown="1">UnaryPlus</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">単項 -</td>
		<td markdown="1">Negate</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">checked(-x)</td>
		<td markdown="1">NegateChecked</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
</table>



### <a id="sec-generated-title-8"></a> <a id="binaryarithmetic"></a>2項演算

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の ArithmeticBinaryOperator() メソッド。

単項 - と同じく、+, -, * にはオーバーフローをチェックするかどうかで2バージョンあります。

ちなみに、C# の言語仕様では、オーバーフローのチェックを行うのは整数に対してのみです。
double などの浮動小数点数では、たとえ checked がついていても、オーバーフローのチェックは行われません。

<pre class="source" title="浮動小数点数は checked にならない" lang="">
<code><span class="comment">// たとえ checked がついていても、
// double 同士の演算はオーバーフローをチェックしない</span>
SimpleCheck(
  <span class="type">Make</span>.Expression((<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; <span class="reserved">checked</span>(x + y)).Body,
  <span class="type">Expression</span>.Add(
    <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">double</span>), <span class="literal">"x"</span>),
    <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">double</span>), <span class="literal">"y"</span>))
);
</code></pre>


あと、C# には、べき乗算子はありませんが、
式木にはべき乗を表す Power ノードがあります。
（VB などではべき乗演算子があるため。）
（ユーザ定義型で、べき乗の意味で ^ 演算子をオーバーロードしても、
^ の式木への変換結果は ExclusiveOr になります。）

<table summary="2項算術演算（unchecked）">
	<caption>
		2項算術演算（unchecked）
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">加算 +</td>
		<td markdown="1">Add</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">減算 -</td>
		<td markdown="1">Subtract</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">乗算 *</td>
		<td markdown="1">Multiply</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">除算 /</td>
		<td markdown="1">Divide</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">剰余 %</td>
		<td markdown="1">Modulo</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">べき乗（C# には対応する演算子なし）</td>
		<td markdown="1">Power</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
</table>


<table summary="2項算術演算（checked）">
	<caption>
		2項算術演算（checked）
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">checked(+)</td>
		<td markdown="1">AddChecked</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">checked(-)</td>
		<td markdown="1">SubtractChecked</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">checked(*)</td>
		<td markdown="1">MultiplyChecked</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-9"></a> <a id="comparison"></a>比較演算

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の ComparisonOperator() メソッド。

<table summary="比較演算">
	<caption>
		比較演算
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">==</td>
		<td markdown="1">Equal</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">!=</td>
		<td markdown="1">NotEqual</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">&lt;</td>
		<td markdown="1">LessThan</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">&lt;=</td>
		<td markdown="1">LessThanOrEqual</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">&gt;</td>
		<td markdown="1">GreaterThan</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">&gt;=</td>
		<td markdown="1">GreaterThanOrEqual</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-10"></a> <a id="logical"></a>論理演算

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の LogicalOperator() メソッド。

通常の &amp;, |, ^ がそれぞれ And, Or, ExclusiveOr で、
「[短絡評価](../start/st_operator.md#shortcircuit)」版 &amp;&amp;, || がそれぞれ AndAlso, OrElse です。

bool に対する論理否定 ! と、整数型に対するビット反転 ^ はいずれも Not になります。

<table summary="">
	<caption>
		
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">論理積 &amp;</td>
		<td markdown="1">And</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">論理和 |</td>
		<td markdown="1">Or</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">排他的論理和 ^</td>
		<td markdown="1">ExclusiveOr</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">論理否定 !・ビット反転 ^</td>
		<td markdown="1">Not</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">短絡評価 And &amp;&amp;</td>
		<td markdown="1">AndAlso</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">短絡評価 Or ||</td>
		<td markdown="1">OrElse</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-11"></a> <a id="misc"></a>その他の2項・3項演算

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の OtherOperator() メソッド。

ヌル結合演算子 a ?? b は、a != null ? a : b には展開されるわけではなく、
ちゃんと Coalesce という式木ノードがあります。

大半の演算子は 生成メソッド名と NodeType の名前が一致するのに、
条件演算子は微妙に違うので注意。

<table summary="その他の2項・3項演算">
	<caption>
		その他の2項・3項演算
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>NodeType</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">左シフト &lt;&lt;</td>
		<td markdown="1">LeftShift</td>
		<td markdown="1">LeftShift</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">右シフト &gt;&gt;</td>
		<td markdown="1">RightShift</td>
		<td markdown="1">RightShift</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">ヌル結合演算 ??</td>
		<td markdown="1">Coalesce</td>
		<td markdown="1">Coalesce</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">条件演算子 ? :</td>
		<td markdown="1">Condition</td>
		<td markdown="1">Conditional</td>
		<td markdown="1">ConditionalExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-12"></a> <a id="type"></a>型変換・判定

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の () メソッド。

int から short にキャストする際などには、オーバーフローが発生する可能性があるので、
キャストには算術演算と同様に checked 版と unchecked 版があります。
（as 演算子はそういう挙動はしないので、checked 版なし。）

<table summary="型変換・判定">
	<caption>
		型変換・判定
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">as</td>
		<td markdown="1">TypeAs</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">is</td>
		<td markdown="1">TypeIs</td>
		<td markdown="1">TypeBinaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">キャスト</td>
		<td markdown="1">Convert</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">checked キャスト</td>
		<td markdown="1">ConvertChecked</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-13"></a> <a id="memberaccess"></a>メンバー参照

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の MemberAccess() メソッド。

フィールド（メンバー変数）・プロパティの参照が MemberAcess、
配列の長さの参照が ArrayLength、
配列の要素参照が ArrayIndex です。

配列の長さ参照は、C# では Length プロパティの参照で表しますが、
言語によっては配列長参照演算子があるからか、ArrayLength というノードタイプが用意されています。
（配列の Length プロパティの参照は、MemberAccess ではなく ArrayLength になります。）

<table summary="">
	<caption>
		
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>NodeType</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">フィールド・プロパティ参照</td>
		<td markdown="1">MakeMemberAccess</td>
		<td markdown="1">MemberAccess</td>
		<td markdown="1">MemberExpression</td>
	</tr>
	<tr>
		<td markdown="1">配列長参照</td>
		<td markdown="1">ArrayLength</td>
		<td markdown="1">ArrayLength</td>
		<td markdown="1">UnaryExpression</td>
	</tr>
	<tr>
		<td markdown="1">配列要素参照</td>
		<td markdown="1">ArrayIndex</td>
		<td markdown="1">ArrayIndex</td>
		<td markdown="1">BinaryExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-14"></a> <a id="new"></a>インスタンス生成

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の New() メソッド。

new Point(1, 2) みたいな普通のコンストラクタ呼び出しは New になります。

new int[] { 1, 2 } のような形式の配列生成は ArrayNewInit、
new int[2] のような形式のものは ArrayBounds です。

new Point { X = 1, Y = 2 } のような、初期化子を使った初期化は MemberInit になります。
MemberInit ノードは New プロパティと Bindings プロパティを持っていて、
New がコンストラクタ呼び出し、Bindings が初期化子ーによるメンバー初期化を表します。

<table summary="インスタンス生成">
	<caption>
		インスタンス生成
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">コンストラクタ呼び出し</td>
		<td markdown="1">New</td>
		<td markdown="1">NewExpression</td>
	</tr>
	<tr>
		<td markdown="1">配列（要素指定）</td>
		<td markdown="1">NewArrayInit</td>
		<td markdown="1">NewArrayExpression</td>
	</tr>
	<tr>
		<td markdown="1">配列（配列長指定）</td>
		<td markdown="1">NewArrayBounds</td>
		<td markdown="1">NewArrayInit</td>
	</tr>
	<tr>
		<td markdown="1">初期化子による初期化</td>
		<td markdown="1">MemberInit</td>
		<td markdown="1">MemberInitExpression</td>
	</tr>
</table>


MemberInit の Bindings は、
以下のような単純なものは MemberAssingment（Expressin.Bind メソッドで生成）、

<pre class="source" title="MemberAssingment" lang="">
<code><span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">2</span> }
</code></pre>


以下のような、再帰構造を持つものは MemberMemberBinding（Expression.MemberBind で生成）、

<pre class="source" title="MemberMemberBinding" lang="">
<code><span class="reserved">new</span> <span class="type">LineSegment</span>
{
  Start = { X = <span class="literal">1</span>, Y = <span class="literal">1</span> },
  End = { X = <span class="literal">2</span>, Y = <span class="literal">2</span> }
}
</code></pre>


以下のようなリスト形式のものは ListBinding（Expression.ListBind で生成）

<pre class="source" title="" lang="">
<code><span class="reserved">new</span> Polyline
{
  Vertices = {
    <span class="reserved">new</span> <span class="type">Point</span>{ X = <span class="literal">1</span>, Y = <span class="literal">1</span> },
    <span class="reserved">new</span> <span class="type">Point</span>{ X = <span class="literal">2</span>, Y = <span class="literal">2</span> },
  }
}
</code></pre>


になります。


## <a id="sec-generated-title-15"></a> <a id="call"></a>メソッド・デリゲート呼び出し

サンプル： 
[ExpressionTest.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/ExpressionTest.cs)
 中の Call() メソッド。

メソッドの呼び出しは Call、デリゲート・ラムダ式の呼び出しは Invoke になります。

<table summary="">
	<caption>
		
	</caption>
	<tr>
		<th>対応するコード</th>
		<th>生成メソッド</th>
		<th>型</th>
	</tr>
	<tr>
		<td markdown="1">メソッド呼び出し</td>
		<td markdown="1">Call</td>
		<td markdown="1">MethodCallExpression</td>
	</tr>
	<tr>
		<td markdown="1">デリゲート呼び出し</td>
		<td markdown="1">Invoke</td>
		<td markdown="1">InvocationExpression</td>
	</tr>
</table>



## <a id="sec-generated-title-16"></a> <a id="ast"></a>式木 4.0（構文木）

<h5 class="version version4">Ver. 4.0</h5>

.NET Framework 4 で、式木が大幅にバージョンアップしました。
式木と言いつつ（Expression クラスではあるものの）、実際には、
複文、条件分岐、ループなども使えるようになっています。

要するに、構文木（syntax tree）相当の機能は揃っています。
これまでとの互換性から式木（expression tree）を名乗っているだけで、
実際には DLR で使っている構文木の全機能を備えています。

以下に、.NET 4 の式木の例を示します。

<pre class="source" title=".NET Framework 4 での式木の例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq.Expressions;

<span class="reserved">public class</span> <span class="type">Program</span>
{
    <span class="reserved">public static void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">int</span>), <span class="literal">"x"</span>);
        <span class="reserved">var</span> i = <span class="type">Expression</span>.Parameter(<span class="reserved">typeof</span>(<span class="reserved">int</span>), <span class="literal">"i"</span>);
        <span class="reserved">var</span> endLoop = <span class="type">Expression</span>.Label(<span class="literal">"EndLoop"</span>);

        <span class="reserved">var</span> body = <span class="type">Expression</span>.Block(
            <span class="reserved">typeof</span>(<span class="reserved">int</span>),
            <span class="reserved">new</span>[] { x },
            <span class="type">Expression</span>.Assign(x, <span class="type">Expression</span>.Constant(<span class="literal">0</span>)),
            <span class="type">Expression</span>.Loop(
                <span class="type">Expression</span>.Block(
                    <span class="type">Expression</span>.AddAssign(x, i),
                    <span class="type">Expression</span>.SubtractAssign(i, <span class="type">Expression</span>.Constant(<span class="literal">1</span>)),
                    <span class="type">Expression</span>.IfThen(
                        <span class="type">Expression</span>.LessThan(i, <span class="type">Expression</span>.Constant(<span class="literal">0</span>)),
                        <span class="type">Expression</span>.Break(endLoop))),
                endLoop),
            x);

        <span class="reserved">var</span> e = <span class="type">Expression</span>.Lambda&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt;(body, i);

        <span class="reserved">var</span> f = e.Compile();

        <span class="type">Console</span>.WriteLine(f(<span class="literal">2</span>));
        <span class="type">Console</span>.WriteLine(f(<span class="literal">4</span>));
        <span class="type">Console</span>.WriteLine(f(<span class="literal">6</span>));
    }
}
</code></pre>


これで、以下のコードに相当する式木が作れます。

<pre class="source" title="ループ持ちのラムダ式" lang="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f = i =&gt;
    {
        <span class="reserved">int</span> x = <span class="literal">0</span>;
        <span class="reserved">for</span> (; ;)
        {
            x += i;
            i -= <span class="literal">1</span>;
            <span class="reserved">if</span> (i &lt;= <span class="literal">0</span>) <span class="reserved">break</span>;
        }
        <span class="reserved">return</span> x;
    };
</code></pre>


（ループは永久ループに相当する LoopExpression しかなくて、for や while 相当のコードを書くには、上記のように if と break を使います。）

ただし、このラムダ式を <code>Expression&lt;Func&lt;int, int&gt;&gt;</code> に代入することはできません。
C# の仕様自体は C# 3.0 の時から変わっていなくて、
単文のラムダ式しか式木にできません。


## <a id="sec-generated-title-17"></a> <a id="link"></a>式木の利用例（リンク）

このサイト内にある式木関連のサンプルにリンク：

* 「[[サンプル] 式木の利用例](../sample/sp3_expressionsample.md)」

* 「[[サンプル] 式木を WPF で GUI 表示](../sample/sm_treeview.md)」

* 「[[サンプル] 式木からクエリ式の再構築](../sample/sp3_linqreconstruct.md)」

* 「[[サンプル] クエリ式とリスト内包](../sample/sp3_comprehensions.md)」
