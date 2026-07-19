---
title: "int 型のジェネリック型引数"
source_url: "https://ufcpp.net/blog/2017/4/intgenericparamter/"
content_type: "BlogEntry"
published_at: "2017-04-29T23:12:33"
updated_at: "2017-04-29T23:12:33"
tags: []
umbraco_id: 2058
parent_id: 2050
sort_order: 3
aliases: []
---

# int 型のジェネリック型引数

[昨日のビットフィールド用コード生成](../bitfields/index.md)の副産物の話。

## M～Nビット目

「int型変数のM～Nビットの範囲を取り出す処理をしたい」みたいなときに、「M～Nビット目を表す型」を作りたいときがあったりします。
(まあ、ビットフィールドの利用頻度自体がそんなに高くないので、この型の需要もそこまで大きくはないですが。)

ってことで、書きたいのはこんなコード:

<pre class="source" title="[M, N)ビット目を読み書きする構造体">
<code><span class="comment">// C# では書けない書き方。C++だとこんな感じのことができたりする。</span>
<span class="comment">// [M, N) (N は含まない)の範囲のビットを読み書きする構造体。</span>
<span class="reserved">struct</span> <span class="type">Bit</span>&lt;<span class="type">T</span>, <span class="type">M</span>, <span class="type">N</span>&gt;
{
    <span class="comment">// C# で書けないことその1: 参照フィールド</span>
    <span class="reserved">ref</span> <span class="type">T</span> _r;
    <span class="reserved">public</span> Bit(<span class="reserved">ref</span> <span class="type">T</span> r) =&gt; _r = <span class="reserved">ref</span> r;

    <span class="comment">// C# で書けないことその2: ジェネリック引数が型じゃなくて int</span>
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">int</span> Shift = <span class="type">M</span>;
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="type">T</span> Mask = (1 &lt;&lt; <span class="type">N</span>) - (1 &lt;&lt; <span class="type">M</span>);

    <span class="comment">// C# で書けないことその3: ジェネリックな型に対して静的な処理(&amp; とかの演算子)</span>
    <span class="reserved">public</span> <span class="type">T</span> Value
    {
        <span class="reserved">get</span> =&gt; (r &gt;&gt; Shift) &amp; Mask;
        <span class="reserved">set</span> =&gt; r = (r &amp; (~Mask)) | ((<span class="reserved">value</span> &lt;&lt; Shift) &amp; Mask);
    }
}
</code></pre>

素直にできないことが3つほどあります。

1. 参照フィールド
1. `int` 型なジェネリック引数
1. ジェネリックな型に対して静的な処理(特に、`&` などの演算子)

1つ目の参照フィールドに関しては、unsafeでよければポインターを使って近いことはできます。
(ポインターなので、使う際に[`fixed`](../../../../study/csharp/interop/sp_unsafe.md#fixed)必須だったり、参照先が残っている保証がなかったり、いろいろ問題あり。)
今、[`ByReference<T>`型](https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/ByReference.cs)とかいう、ランタイムで特殊処理して「参照フィールドっぽいもの」を実現しようとしている動きもあります。

まあ、今日の話は残りの2つ、特に、2つ目の`int`型ジェネリック引数です。

## ジェネリクスに静的な処理を

素直な手段では、C#で静的な処理に対してジェネリクスを使う手段がなかったりします。
だいたいインターフェイス越しにメソッドを呼ぶことになっちゃって、仮想呼び出しになる。
仮想呼び出しなので、インライン展開とかも効かず、あんまり実行性能はよろしくない。

ですが、まあ、ちょっと煩雑なコードを書いていいなら、ジェネリック、かつ、高性能なコードを書く手段は一応あります。

参考:

- [静的メソッド代わり](../../../../study/csharp/oop/sp2_generics.md#pseudo-static)
- [値型ジェネリックを使った最適化](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2017/InlineExpansion)
  - (この辺りもそのうちちゃんとした説明書きたい)

こういうトリックを使って、「M～Nビット目を表す型」を作ってみたのが以下のコード:

- [型定義: `Bits<T, TOp, LowBit, HighBit>`型](https://github.com/ufcpp/BitFields/blob/master/experimental/TypeClassBits/Bits_M_N.cs)
- [利用例](https://github.com/ufcpp/BitFields/blob/master/experimental/ConsoleApp1/TypeClassBits/DoubleView.cs)

## `int` 型なジェネリック引数の需要

まあ、上記の[`Bits<T, TOp, LowBit, HighBit>`型](https://github.com/ufcpp/BitFields/blob/master/experimental/TypeClassBits/Bits_M_N.cs)だと、やってることが多すぎてちょっと細かく見て行くのしんどいかなぁとか思って、「`int` 型なジェネリック引数」もどきを実現する部分だけを抜き出したのがこちら:

- [C++ の template<int N> もどきを C# で](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2017/IntTemplateParameter/IntTemplateParameter)

この手の要件に対してよく例として使われる[ガロアの有限体](https://ja.wikipedia.org/wiki/%E6%9C%89%E9%99%90%E4%BD%93)を実装しています。

またちょっと、数学わからない人には申し訳ない感じの例ですが…
要するに、Nが素数のとき、N個の値から成る「四則演算全部まっとうにできる型」が作れるって話です。
(整数の部分集合なのに、整数÷整数で、同じ範囲の整数値に収まる。任意の値 a に対して、a × b = 1 となる b がただ1つ定まります。)

2値からなるF<sub>2</sub>と、3値からなるF<sub>3</sub>は当然「別の型」なわけで、
プログラムを書く場合にも、型引数にして2と3で「別の具象型」を作れる仕組みが欲しくなります。

## `GaloisField<N>`

ということで、それをC#で、[`GaloisField<N>`って書き方で書けるようにしたのが](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2017/IntTemplateParameter/IntTemplateParameter)今日のポイント。

必要な個所だけ抜き出すと、以下のような感じ。

<pre class="source" title="C# でintジェネリック引数">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IConstant</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span> Value { <span class="reserved">get</span>; }
}

<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">GaloisField</span>&lt;<span class="type">N</span>&gt;
    <span class="reserved">where</span> <span class="type">N</span> : <span class="reserved">struct</span>, <span class="type">IConstant</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="comment">// ガロアの有限体を作るにあたって必要なのは、拡張ユークリッド互除法っていうアルゴリズム</span>
    <span class="comment">// https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/IntTemplateParameter/IntTemplateParameter/GaloisField_N.cs#L43</span>

    <span class="comment">// で、その中で、「i を N で割った余り」を使う</span>
    <span class="comment">// (C# の % だと、i が負の時に結果が負になるので、その場合には N を足して正にする。)</span>
    <span class="reserved">static</span> <span class="reserved">int</span> Mod(<span class="reserved">int</span> i)
    {
        <span class="comment">// 定数「N」の代わりに、default(N).Value って書く</span>
        i %= <span class="reserved">default</span>(<span class="type">N</span>).Value;
        <span class="reserved">if</span> (i &lt; 0) i += <span class="reserved">default</span>(<span class="type">N</span>).Value;
        <span class="reserved">return</span> i;
    }
}
</code></pre>

ただし、定数代わりに、以下のような構造体を作っておく必要あり。

<pre class="source" title="定数もどきの構造体">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">ConstantInt</span>
{
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">_0</span> : <span class="type">IConstant</span>&lt;<span class="reserved">int</span>&gt; { <span class="reserved">public</span> <span class="reserved">int</span> Value =&gt; 0; }
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">_1</span> : <span class="type">IConstant</span>&lt;<span class="reserved">int</span>&gt; { <span class="reserved">public</span> <span class="reserved">int</span> Value =&gt; 1; }
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">_2</span> : <span class="type">IConstant</span>&lt;<span class="reserved">int</span>&gt; { <span class="reserved">public</span> <span class="reserved">int</span> Value =&gt; 2; }
    <span class="comment">//...</span>
}
</code></pre>

いちいち必要な分だけ構造体を定義しておかないといけないので、定数の置き換えとしては不完全ですけども、
実行性能的には結構いい結果になるはずです。
(ちゃんと静的に展開されて、インライン化もされる状態になります。)
