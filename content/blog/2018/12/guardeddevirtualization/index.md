---
title: "Guarded Devirtualization"
source_url: "https://ufcpp.net/blog/2018/12/guardeddevirtualization/"
content_type: "BlogEntry"
published_at: "2018-12-18T09:32:09"
updated_at: "2018-12-18T21:28:28"
tags: []
umbraco_id: 2200
parent_id: 2177
sort_order: 17
aliases: []
---

# Guarded Devirtualization

今日はちょっと将来の話。
提案ドキュメントとか予備実験的な実装はあるんですが、
リリースされる時期については未定のものです。

Guarded Devirtualization という最適化手法。

- 参考: [JIT: see if guarded devirtualization for EqualityComparer methods pays off](https://github.com/dotnet/coreclr/issues/14223)

(余談ですが、[この提案に当たっての調査レポート](https://github.com/AndyAyersMS/coreclr/blob/d35d4dd64a4e1b1ba67f3c804cabcfaa1094b678/Documentation/design-docs/GuardedDevirtualization.md)、ものすごく丁寧で良い内容です。
何かを提案する際の理想形。)

## Devirtualization の実情

昨日の[Devirtualization 最適化](../devirtualization/index.md)の話で書きましたが、
仮想呼び出しを通常のメソッド呼び出しに置き換える最適化があって、これを devirtualization といいます。

ただ、devirtualization できる状況はかなり限られています。
coreclr 内で統計を取ってみたところ、クラスの仮想メソッドの呼び出ししているところのうち15%程度しか、devirtualization 最適化が掛からないそうです。
インターフェイスを介しているものについてはもっときつくて、5%程度だそうです。

なんせ、devirtualization が有効になるためには、「メソッド内をさかのぼれば静的な型が1つに確定している」という状態でないといけない。
それに対して、実際のところ多い状況は、
「ほとんどの場合には決まったある1つの型のが来るものの、まれに別の型が来る」というものです。

## if + Devirtualization

そこで、最頻で来てそうな1つ(あるいはせいぜい数個)の型に対してだけ `if` を挟んでしまうという最適化が考えられます。

例えば、以下のようないくつかの型があったとして

<pre class="source">
<code><span class="reserved">interface</span> <span class="type">I</span> { <span class="reserved">void</span> M(); }
<span class="reserved">struct</span> <span class="type">A1</span> : <span class="type">I</span> { <span class="reserved">public</span> <span class="reserved">void</span> M() { } }
<span class="reserved">struct</span> <span class="type">A2</span> : <span class="type">I</span> { <span class="reserved">public</span> <span class="reserved">void</span> M() { } }
<span class="reserved">struct</span> <span class="type">A3</span> : <span class="type">I</span> { <span class="reserved">public</span> <span class="reserved">void</span> M() { } }
<span class="reserved">struct</span> <span class="type">A4</span> : <span class="type">I</span> { <span class="reserved">public</span> <span class="reserved">void</span> M() { } }
</code></pre>

以下のような呼び出しを考えます。

<pre class="source">
<code><span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">I</span>[] items)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> items)
    {
        i.M();
    }
}
</code></pre>

何の前提もないと、このコードは最適化のやりようがないんですが、
例えば、
「ほとんどの場合に`A1`～`A4`の構造体が来る。他の型が来る率は低い」、
「その中でも`A1`の頻度が特に高い」みたいな前提が入ると、
以下のようなコードが速くなったりします。

<pre class="source">
<code><span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">I</span>[] items)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> items)
    {
        <span class="reserved">if</span> (i.GetType() == <span class="reserved">typeof</span>(<span class="type">A1</span>)) ((<span class="type">A1</span>)i).M();
        <span class="reserved">else</span> <span class="reserved">if</span> (i.GetType() == <span class="reserved">typeof</span>(<span class="type">A2</span>)) ((<span class="type">A2</span>)i).M();
        <span class="reserved">else</span> <span class="reserved">if</span> (i.GetType() == <span class="reserved">typeof</span>(<span class="type">A3</span>)) ((<span class="type">A3</span>)i).M();
        <span class="reserved">else</span> <span class="reserved">if</span> (i.GetType() == <span class="reserved">typeof</span>(<span class="type">A4</span>)) ((<span class="type">A4</span>)i).M();
        <span class="reserved">else</span> i.M();
    }
}
</code></pre>

数個程度の `if` 分岐であれば仮想呼び出しのコストよりも安くなります。
特に、発生確率に偏りがある場合には分岐予測が効くので、
「ほとんどが`A1`」みたいな状況では分岐のコストがほぼ消えます。
また、メソッド `M` の実装がインライン展開可能なものだった場合、
インライン展開の効果でかなり速くなります。

[ベンチマーク](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2018/GuardedDevirt/GuardedDevirt/CallBenchmark.cs#L94)を取ってみた感じ、
普通に `i.M()` で仮想呼び出しするよりも、3倍くらい高速です。

ということで、こういう「よく来る型」を実行時に検出して、上記のような`if`分岐を生成するような最適化を CoreCLR に入れたいみたいです。
「ほとんどが `A1`」という予想が外れたときのための“防護策”(guard)として`if`挿入するので、
Guarded Devirtualization と呼ばれます。
