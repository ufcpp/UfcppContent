---
title: "C# 7.1 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver7_1/"
content_type: "Article"
published_at: "2017-06-11T00:00:00"
updated_at: "2017-08-15T00:00:00"
tags: []
umbraco_id: 2073
parent_id: 1174
sort_order: 10
aliases:
  - "/csharp/cheatsheet/ap_ver7_1/"
---

# C# 7.1 の新機能

##<a id="sec-generated-title-1"></a> <a id="ver7_1"></a>C# 7.1
<div class="version version7_1">Ver. 7.1</div>

<table>
<tr>
<th>リリース時期</th>
<td>2017/8</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2017 Update 3 (15.3)</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>C# 7.0のちょっとした改善</li>
</ul>
</td>
</tr>
</table>

2017年8月、すなわち、C# 7.0のリリース(2017年2月)から半年足らずで C# 7.1 がリリースされました。

C# 7.0の頃から、目標としては C# のリリース サイクルの短縮を考えていました。
多くの機能を2・3年に1度一気にリリースするよりも、細かく出せるものに関しては短いリリース サイクルで出したいという意図です。
今回、(実質的に<sup>※</sup>)初の「マイナー バージョンアップ」となる C# 7.1 が誕生しました。

(<sup>※</sup> 一応、[C# 1.1](ap_ver1.md#sec-generated-title-2)があったんですが、ほとんど使われない機能が2つ追加されただけなので、1.1があったこと自体あまり認知されていないものです。)

C# 7.1 は、Visual Studio 2017のリリース時期に間に合わなかった C# 7.0 の積み残しと言った感じの、小さい機能が4つほど追加されています。

##<a id="sec-generated-title-2"></a> <a id="async-Main"></a>非同期Main
`Main`メソッドの戻り値に`Task`クラス(`System.Threading.Tasks`名前空間)を使えるようになりました。
以下のいずれかのオーバーロードであればエントリーポイントとして認識されます。

<pre class="source" title="非同期Main(C# 7.1 から)">
<code><span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; Main()
<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; Main(<span class="reserved">string</span>[] args)
<span class="reserved">static</span> <span class="type">Task</span> Main()
<span class="reserved">static</span> <span class="type">Task</span> Main(<span class="reserved">string</span>[] args)
</code></pre>

詳しくは、「[非同期Main](../structured/miscentrypoint.md#async-main)」で説明します。

##<a id="sec-generated-title-3"></a> <a id="default-expr"></a>default 式
これまでも[既定値](../resource/rm_default.md)を作るために、`default(T)`という構文がありましたが、
型名`T`の指定が煩雑でした。
特に、名前の長い型に対して`default(T)`を使うと、かなりのうっとおしさがあります。

既定値を結構使って、かつ、名前が長い型というと、例えば`CancellationToken`構造体(`System.Threading`名前空間)とかです。
以下のようなコードを書いたりします。

<pre class="source" title="CancellationTokenの規定値をdefault(T)で作る例">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> DefaultExpression(<span class="type">CancellationToken</span> c = <span class="reserved">default</span>(<span class="type">CancellationToken</span>))
{
    <span class="reserved">while</span> (c != <span class="reserved">default</span>(<span class="type">CancellationToken</span>) &amp;&amp; !c.IsCancellationRequested)
    {
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        <span class="type">Console</span>.WriteLine(<span class="string">"."</span>);
    }
}
</code></pre>

これに対して、C# 7.1では、左辺(代入先)から推論できる場合に、`(T)`を省略して`default`だけで既定値を作れるようになりました。
例えば先ほどのコードは以下のように書き直せます。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> DefaultExpression(<span class="type">CancellationToken</span> c = <em><span class="reserved">default</span></em>)
{
    <span class="reserved">while</span> (c != <em><span class="reserved">default</span></em> &amp;&amp; !c.IsCancellationRequested)
    {
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        <span class="type">Console</span>.WriteLine(<span class="string">"."</span>);
    }
}
</code></pre>

既定値自体や、`default(T)`の説明は「[既定値](../resource/rm_default.md)」を参照してください。

##<a id="sec-generated-title-4"></a> <a id="infer-tuple-name"></a>タプル要素名の推論
タプルの要素名が、タプル構築時に渡した変数から推論できるようになりました。
例えば以下のように、`(x, y)` と書くだけで、1要素目に`x`、2要素目に `y` という名前が付きます。
(これまでだと、`(x: x, y: y)` と書く必要があった。)

<pre class="source" title="タプル要素名の推論の例">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
<span class="reserved">var</span> t = (x, y);

<span class="comment">// C# 7.0。t の要素には名前が付かない</span>
<span class="type">Console</span>.WriteLine(t.Item1);
<span class="type">Console</span>.WriteLine(t.Item2);

<span class="comment">// C# 7.1。(x, y) で (x: x, y: y) 扱い</span>
<span class="comment">// t の要素に x, y という名前が付く</span>
<span class="type">Console</span>.WriteLine(t.x);
<span class="type">Console</span>.WriteLine(t.y);
</code></pre>

詳しくは「[タプル](../datatype/tuples.md#infer-tuple-name)」で説明します。

##<a id="sec-generated-title-5"></a> <a id="generic-type-switch"></a>ジェネリック型に対するパターン マッチング(型スイッチ)
C# 7.0で[`is`や`switch`で型を見ての分岐](../datatype/typeswitch.md)ができるようになりました。
しかし、[ジェネリクス](../oop/sp2_generics.md)が絡む場合、
例えば以下のようなコードはC# 7.0ではコンパイル エラーになっていました。

<pre class="source" title="C# 7.0ではコンパイルできないswitchの例">
<code><span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x)
{
    <span class="reserved">switch</span> (x)
    {
        <span class="reserved">case</span> <span class="reserved">int</span> i:
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="reserved">string</span> s:
            <span class="reserved">break</span>;
    }
}
</code></pre>

「`T`を`int`や`string`として処理できない」と言った旨のコンパイル エラーが出ます。

さらにいうと、以下のような需要が結構ありそうな場面でも、C# 7.0ではコンパイル エラーになりました。

<pre class="source" title="C# 7.0ではコンパイルできないswitchの例(型制約付き)">
<code><span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived1</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived2</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived3</span> : <span class="type">Base</span> { }

<span class="comment">// こういう、型制約付きのやつですら 7.0 ではダメだった</span>
<span class="reserved">static</span> <span class="reserved">void</span> N&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">Base</span>
{
    <span class="reserved">switch</span> (x)
    {
        <span class="reserved">case</span> <span class="type">Derived1</span> d:
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="type">Derived2</span> d:
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="type">Derived3</span> d:
            <span class="reserved">break</span>;
    }
}
</code></pre>

C# 7.0でも、以下のように、`as`演算子を使った場合にはちゃんとコンパイルできます。
型スイッチは、内部的には`as`演算子に展開される機能で、`as`演算子にできて型スイッチにできないことがあるのは不自然です。

<pre class="source" title="as 演算子での置き換え">
<code><span class="reserved">static</span> <span class="reserved">void</span> N&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">Base</span>
{
    { <span class="reserved">var</span> d = x <span class="reserved">as</span> <span class="type">Derived1</span>; <span class="reserved">if</span> (d != <span class="reserved">null</span>) { <span class="reserved">return</span>; } }
    { <span class="reserved">var</span> d = x <span class="reserved">as</span> <span class="type">Derived2</span>; <span class="reserved">if</span> (d != <span class="reserved">null</span>) { <span class="reserved">return</span>; } }
    { <span class="reserved">var</span> d = x <span class="reserved">as</span> <span class="type">Derived3</span>; <span class="reserved">if</span> (d != <span class="reserved">null</span>) { <span class="reserved">return</span>; } }
}
</code></pre>

そこで、C# 7.1では、上記コードのような、ジェネリックな型に対する型スイッチを使えるようになりました。
(新機能というよりは、仕様漏れ・バグ修正の類です。)

パターンマッチング(型スイッチ)自体の説明に関しては「[型スイッチ](../datatype/typeswitch.md)」を参照してください。
