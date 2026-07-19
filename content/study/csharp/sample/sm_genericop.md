---
title: "[サンプル] ジェネリックな複素数型"
source_url: "https://ufcpp.net/study/csharp/sample/sm_genericop/"
content_type: "Article"
published_at: "2008-03-14T00:00:00"
updated_at: "2015-05-06T14:13:20"
tags: []
umbraco_id: 1366
parent_id: 1359
sort_order: 6
aliases:
  - "/csharp/sample/sm_genericop/"
  - "/csharp/sm_genericop"
  - "/csharp/sm_genericop.html"
  - "/study/csharp/sm_genericop"
  - "/study/csharp/sm_genericop.html"
---

# \[サンプル\] ジェネリックな複素数型

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/GenericOperator.zip)



事の発端は↓から。

* [あらあらあら - 東方算程譚](http://blogs.wankuma.com/episteme/archive/2009/02/04/167390.aspx)

* [コンプレックスと戦う - Garbage Collection](http://blogs.wankuma.com/izmktr/archive/2009/02/05/167489.aspx)

* [複素数型を作る。 - HIRASE CONNECTION WK](http://blogs.wankuma.com/hirase/archive/2009/02/05/167505.aspx)

* [Expression trees と .NET 風メタプログラミング - NyaRuRuの日記](http://d.hatena.ne.jp/NyaRuRu/20090205/p1)


ちょうど「[ジェネリック](../oop/sp2_generics.md)」に「[C++ や Java の template/generics との違い](../oop/sp2_generics.md#compare)」を足した時にこういう話題を見かけたので食いついてみた。

ちょっと考えてみた結果、以下のようなクラスを作ると便利なんじゃないかという考えに至る。

<pre class="source" title="動的にジェネリック型 T の加減乗除関数を作る" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq.Expressions;

<span class="reserved">namespace</span> GenericOperator
{
    <span class="reserved">using</span> Binary = Func&lt;ParameterExpression, ParameterExpression, BinaryExpression&gt;;
    <span class="reserved">using</span> Unary = Func&lt;ParameterExpression, UnaryExpression&gt;;

    <span class="comment">/// &lt;summary&gt;
    /// 動的にジェネリック型 T の加減乗除関数を作る。
    /// &lt;/summary&gt;
    /// &lt;typeparam name="T"&gt;対象となる型。&lt;/typeparam&gt;</span>
    <span class="reserved">public static class</span> Operator&lt;T&gt;
    {
        <span class="reserved">static readonly</span> ParameterExpression x = Expression.Parameter(<span class="reserved">typeof</span>(T), <span class="literal">"x"</span>);
        <span class="reserved">static readonly</span> ParameterExpression y = Expression.Parameter(<span class="reserved">typeof</span>(T), <span class="literal">"y"</span>);

        <span class="reserved">public static readonly</span> Func&lt;T, T, T&gt; Add = Lambda(Expression.Add);
        <span class="reserved">public static readonly</span> Func&lt;T, T, T&gt; Subtract = Lambda(Expression.Subtract);
        <span class="reserved">public static readonly</span> Func&lt;T, T, T&gt; Multiply = Lambda(Expression.Multiply);
        <span class="reserved">public static readonly</span> Func&lt;T, T, T&gt; Divide = Lambda(Expression.Divide);
        <span class="reserved">public static readonly</span> Func&lt;T, T&gt; Plus = Lambda(Expression.UnaryPlus);
        <span class="reserved">public static readonly</span> Func&lt;T, T&gt; Negate = Lambda(Expression.Negate);

        <span class="reserved">public static</span> Func&lt;T, T, T&gt; Lambda(Binary op)
        {
            <span class="reserved">return</span> Expression.Lambda&lt;Func&lt;T, T, T&gt;&gt;(op(x, y), x, y).Compile();
        }

        <span class="reserved">public static</span> Func&lt;T, T&gt; Lambda(Unary op)
        {
            <span class="reserved">return</span> Expression.Lambda&lt;Func&lt;T, T&gt;&gt;(op(x), x).Compile();
        }
    }
}
</code></pre>



## <a id="sec-generated-title-2"></a> <a id="template"></a>C++ template と C# genrics の違い

「[C++ や Java の template/generics との違い](../oop/sp2_generics.md#compare)」に書いたように、
C++ の template と C# のジェネリクスには色々違いがありますが、
このページで問題にするのは以下の点。

* C++ の template では、 メソッド呼び出しは「[ダックタイピング](../appendix/ap_term.md#ducktype)」で行う。

* C# のジェネリクスでは、 メソッド呼び出しはインターフェースを使った型制約で行う。


C# の generics において、メソッド呼び出しにインターフェースを使うということは、
以下のような制約が生じます。

* 型引数がどういうインターフェースを実装しているべきか、コンパイル時に分かっている必要がある。

* 静的メソッドを呼べない。 ということは、operator を使えない。


特に、operator を使えないというのが結構問題で、複素数クラスのジェネリック版を作るのにすら苦労することになります。

例えば、C++ ならば以下のような書き方ができます。

<pre class="source" title="C++ で複素数に + 演算子を定義" lang="">
<code>template&lt;typename T&gt;
Complex&lt;T&gt; operator +(Complex&lt;T&gt; x, Complex&lt;T&gt; y)
{
  //↓ T の型がなんであれ、+ 演算子を持っているものならコンパイル可能。
  T re = x.re + y.re;
  T im = x.im + y.im;
  return Complex&lt;T&gt;(re, im);
}
</code></pre>


ところが、同じことを C# でやろうとすると、コンパイルエラーになります。

<pre class="source" title="C# で複素数に + 演算子を定義（失敗例）" lang="">
<code><span class="reserved">public static</span> Complex&lt;T&gt; <span class="reserved">operator</span> +(Complex&lt;T&gt; x, Complex&lt;T&gt; y)
{
  <span class="comment">//↓ エラー： 演算子 '+' を 'T' と 'T' 型のオペランドに適用することはできません。</span>
  T re = x.re + y.re;
  T im = x.im + y.im;
  <span class="reserved">return new</span> Complex&lt;T&gt;(re, im);
}
</code></pre>


（
C# のジェネリクスは、
異なる型引数で（例えば List&lt;int&gt; と List&lt;string&gt; で）可能な限り生成されるコードを共通化する方針を取ったためこういう仕様になっています。
C++ の方針の方が自由は効きますが、こちらはこちらで別の問題も抱えているのでどちらがいいとも言えないです。
）


## <a id="sec-generated-title-3"></a> <a id="howto"></a>C# ではどうすればいいか

C# ジェネリクスでできないことも、動的コード生成などを使うことで何とかできることが多いです。

1. <h5 class="version version4">Ver. 4.0</h5>C# 4.0 で導入される dynamic キーワードを使う。

2. 「[リフレクション](../dynamic/sp_reflection.md#reflection)」を使う。

3. 「[式木](../functional/sp3_lambda.md#exp_tree)」を使う。


1つ目の dynamic を使えば、まさに「[ダックタイピング](../appendix/ap_term.md#ducktype)」が可能です。
でも、「キャスト不要」とか「実行効率がいい」というようなジェネリクスの利点とは相反するものなので、
あまりジェネリクスと組み合わせて使いたいものではないです。

2つ目のリフレクションを使えば、例えば以下のようなことができます。

<pre class="source" title="ジェネリクスとリフレクションの組み合わせ" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Text;

<span class="reserved">class</span> Test
{
    <span class="reserved">static void</span> Main()
    {
        <span class="reserved">var</span> mem = <span class="reserved">new</span> MemoryStream();
        <span class="reserved">var</span> writer = <span class="reserved">new</span> BinaryWriter(mem);
<em>
        Serializer.Serialize(writer, 10);
        Serializer.Serialize(writer, 10.0);
        Serializer.Serialize(writer, (<span class="reserved">byte</span>)10);
        Serializer.Serialize(writer, <span class="literal">"10"</span>);
</em>
        mem.Seek(0, SeekOrigin.Begin);

        <span class="reserved">var</span> reader = <span class="reserved">new</span> BinaryReader(mem);
<em>
        Console.WriteLine(Serializer.Deserialize&lt;<span class="reserved">int</span>&gt;(reader));
        Console.WriteLine(Serializer.Deserialize&lt;<span class="reserved">double</span>&gt;(reader));
        Console.WriteLine(Serializer.Deserialize&lt;<span class="reserved">byte</span>&gt;(reader));
        Console.WriteLine(Serializer.Deserialize&lt;<span class="reserved">string</span>&gt;(reader));
</em>
    }
}

<span class="reserved">static class</span> Serializer
{
    <span class="reserved">public static void</span> Serialize&lt;T&gt;(BinaryWriter writer, T value)
    {
        <span class="comment">// string だけ特殊処理。</span>
        <span class="reserved">if</span> (<span class="reserved">typeof</span>(T) == <span class="reserved">typeof</span>(<span class="reserved">string</span>))
        {
            <span class="reserved">var</span> s = value <span class="reserved">as string</span>;
            <span class="reserved">var</span> b = Encoding.UTF8.GetBytes(s);
            writer.Write(b.Length);
            writer.Write(b);
            <span class="reserved">return</span>;
        }

        <span class="comment">// BinaryWriter.Write のオーバーロードがあるものはこれを呼び出す。</span>
        <span class="reserved">var</span> write = <span class="reserved">typeof</span>(BinaryWriter).GetMethod(<span class="literal">"Write"</span>, <span class="reserved">new</span>[] { <span class="reserved">typeof</span>(T) });
        System.Diagnostics.Debug.Assert(write != <span class="reserved">null</span>);
        write.Invoke(writer, <span class="reserved">new object</span>[] { value });
    }

    <span class="reserved">public static</span> T Deserialize&lt;T&gt;(BinaryReader reader)
    {
        <span class="comment">// string だけ特殊処理。</span>
        <span class="reserved">if</span> (<span class="reserved">typeof</span>(T) == <span class="reserved">typeof</span>(<span class="reserved">string</span>))
        {
            <span class="reserved">var</span> count = reader.ReadInt32();
            <span class="reserved">var</span> b = reader.ReadBytes(count);
            <span class="reserved">return</span> (T)(<span class="reserved">object</span>)Encoding.UTF8.GetString(b);
        }

        <span class="comment">// BinaryReader.Read*** があるものはこれを呼び出す。</span>
        <span class="reserved">var</span> read = <span class="reserved">typeof</span>(BinaryReader).GetMethod(<span class="literal">"Read"</span> + <span class="reserved">typeof</span>(T).Name, <span class="reserved">new</span> Type[0]);
        System.Diagnostics.Debug.Assert(read != <span class="reserved">null</span>);
        <span class="reserved">return</span> (T)read.Invoke(reader, <span class="reserved">new object</span>[0]);
    }
}
</code></pre>


これも、特殊な用途で使うものであって、
キャストが必要だし、実行効率もあまりよくないです。
（この場合、ジェネリクスを使う理由は型推論の利用であって、実行効率ではない。）

それに、この方法だと、int などの組み込み数値型に対する四則演算は呼び出せなかったりします。
（C# では組み込み数値型の演算は特別扱いされてて、リフレクションでは呼び出せない。）

で、今回問題になっている operator 呼び出しには、3つ目の式木を使うのがいいと思います。


## <a id="sec-generated-title-4"></a> <a id="expression"></a>式木で動的コード生成

ということで、前節で出てきた3つ目、式木を使った動的コード生成について説明します。

.NET Framework 3.0 で導入された式木を使うと、以下のようなことができます。

<pre class="source" title="式木を使ってジェネリックに加算" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq.Expressions;

<span class="reserved">class</span> Test
{
    <span class="reserved">static void</span> Main()
    {
        <span class="reserved">var</span> add = CreateAdder&lt;<span class="reserved">int</span>&gt;();

        Console.WriteLine(add(10, 20)); <span class="comment">// 10 + 20 で 30 が表示される。</span>
    }

    <span class="comment">/// &lt;summary&gt;
    /// (T x, T y) =&gt; x + y; に相当する匿名デリゲートを生成する。
    /// &lt;/summary&gt;
    /// &lt;typeparam name="T"&gt;オペランドの型。&lt;/typeparam&gt;
    /// &lt;returns&gt;加算デリゲート。&lt;/returns&gt;</span>
    <span class="reserved">static</span> Func&lt;T, T, T&gt; CreateAdder&lt;T&gt;()
    {
        <span class="reserved">var</span> x = Expression.Parameter(<span class="reserved">typeof</span>(T), <span class="literal">"x"</span>);
        <span class="reserved">var</span> y = Expression.Parameter(<span class="reserved">typeof</span>(T), <span class="literal">"y"</span>);

        <span class="reserved">var</span> expression = Expression.Lambda&lt;Func&lt;T, T, T&gt;&gt;(
            Expression.Add(x, y),
            x, y);

        <span class="reserved">return</span> expression.Compile();
    }
}
</code></pre>


汎用化するために、このページの冒頭で載せたようなクラスを定義。
要点だけ抜粋すると、以下のような感じ。

<pre class="source" title="動的にジェネリック型 T の加減乗除関数を作る" lang="">
<code><span class="reserved">using</span> Binary = Func&lt;ParameterExpression, ParameterExpression, BinaryExpression&gt;;

<span class="reserved">public static class</span> Operator&lt;T&gt;
{
    <em><span class="reserved">public static readonly</span> Func&lt;T, T, T&gt; Add = Lambda(Expression.Add);</em>

    <span class="reserved">public static</span> Func&lt;T, T, T&gt; Lambda(Binary op)
    {
        <span class="reserved">return</span> Expression.Lambda&lt;Func&lt;T, T, T&gt;&gt;(
            op(x, y),
            x, y).Compile();
    }
}
</code></pre>


これを使えば、ジェネリックな複素数の加算を以下のような感じで作れます。

<pre class="source" title="C# で複素数に + 演算子を定義" lang="">
<code><span class="reserved">static</span> T Add(T x, T y) { <span class="reserved">return</span> Operator&lt;T&gt;.Add(x, y); }

<span class="reserved">public static</span> Complex&lt;T&gt; <span class="reserved">operator</span> +(Complex&lt;T&gt; x, Complex&lt;T&gt; y)
{
    T re = Add(x.re, y.re);
    T im = Add(x.im, y.im);
    <span class="reserved">return new</span> Complex&lt;T&gt;(re, im);
}
</code></pre>


<code>x.re + y.re</code> と書けなくて不格好なのと、
デリゲート呼び出しが1段挟まって多少まだ効率が悪いので、
C++ の template には少々及びませんが、
これでジェネリックな複素数クラスを作るという目的は果たせそうです。

完成品は以下の通り。

* 
[Operator&lt;T&gt; クラス](../../../../assets/media/ufcpp2000/csharp/source/Operator.cs)


* 
[Complex&lt;T&gt; クラス](../../../../assets/media/ufcpp2000/csharp/source/Complex.cs)


* 
[下記サンプルで使っている有理数クラス](../../../../assets/media/ufcpp2000/csharp/source/Rational.cs)


* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/GenericOperator.zip)



利用例としては以下のような感じ。

<pre class="source" title="Complex&lt;T&gt; クラスの利用例" lang="">
<code><span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
{
    ShowFourOperations(
        4 .I(5),
        2 .I(4));

    ShowFourOperations(
        4.0 .I(5.0),
        2.0 .I(4.0));

    ShowFourOperations(
        1.Over(2) .I(1.Over(3)),
        2.Over(3) .I(3.Over(4)));
}

<span class="reserved">static void</span> ShowFourOperations&lt;T&gt;(Complex&lt;T&gt; x, Complex&lt;T&gt; y)
    <span class="reserved">where</span> T: IComparable&lt;T&gt;
{
    Console.WriteLine(<span class="reserved">typeof</span>(T).Name);
    Console.WriteLine(<span class="literal">"({0}) + ({1}) = {2}"</span>, x, y, x + y);
    Console.WriteLine(<span class="literal">"({0}) - ({1}) = {2}"</span>, x, y, x - y);
    Console.WriteLine(<span class="literal">"({0}) * ({1}) = {2}"</span>, x, y, x * y);
    Console.WriteLine(<span class="literal">"({0}) / ({1}) = {2}"</span>, x, y, x / y);
}
</code></pre>


<pre class="console" title="実行結果">
Int32
(4 + i5) + (2 + i4) = 6 + i9
(4 + i5) - (2 + i4) = 2 + i1
(4 + i5) * (2 + i4) = -12 + i26
(4 + i5) / (2 + i4) = 0 + i0
Double
(4 + i5) + (2 + i4) = 6 + i9
(4 + i5) - (2 + i4) = 2 + i1
(4 + i5) * (2 + i4) = -12 + i26
(4 + i5) / (2 + i4) = 1.75 - i0.375
Rational
((1/2) + i(1/3)) + ((2/3) + i(3/4)) = (7/6) + i(13/12)
((1/2) + i(1/3)) - ((2/3) + i(3/4)) = (-1/6) - i(5/12)
((1/2) + i(1/3)) * ((2/3) + i(3/4)) = (1/12) + i(43/72)
((1/2) + i(1/3)) / ((2/3) + i(3/4)) = (7/12) - i(11/72)
</pre>
