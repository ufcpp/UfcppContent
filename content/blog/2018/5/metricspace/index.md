---
title: "C# にも型クラス(Shapes)が欲しい… 距離空間上のアルゴリズム実装"
source_url: "https://ufcpp.net/blog/2018/5/metricspace/"
content_type: "BlogEntry"
published_at: "2018-05-03T16:46:26"
updated_at: "2018-05-04T04:10:15"
tags: []
umbraco_id: 2151
parent_id: 2150
sort_order: 0
aliases: []
---

# C# にも型クラス(Shapes)が欲しい… 距離空間上のアルゴリズム実装

今日は C# で「距離の計算」を汎用的に、かつ、高パフォーマンスでやりたいという話。
というか、やりたいのはやまやまなんだけど、高パフォーマンスを目指すとなかなか大変なことになるという話。
[Shapes](../../../2017/2/pickuproslyn0223/index.md)が来れば楽になるはずだけども、計画上だいぶ先の話なので、待っていると厳しいので大変なのを我慢したという話でもあります。

サンプル コードの全体像: [MetricSpace](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/MetricSpace)

## <a id="distance">距離</a>

2つの何らかの情報の距離を求めたいことは結構あります。

- [近いもの同士でグルーピングしたい](https://ja.wikipedia.org/wiki/%E3%83%87%E3%83%BC%E3%82%BF%E3%83%BB%E3%82%AF%E3%83%A9%E3%82%B9%E3%82%BF%E3%83%AA%E3%83%B3%E3%82%B0)
- [最短経路を求めたい](https://ja.wikipedia.org/wiki/%E6%9C%80%E7%9F%AD%E7%B5%8C%E8%B7%AF%E5%95%8F%E9%A1%8C)
- [一定範囲に入っているものだけを取り出したい](https://docs.unity3d.com/jp/current/Manual/OcclusionCulling.html)

汎用性が要らないなら簡単な話で、以下のようなコードで書けます。

<pre class="source" title="float の配列に対するユークリッド距離">
<code><span class="reserved">class</span> <span class="type">Euclidean</span>
{
    <span class="comment">// a と b の長さが同じとか、いくつか前提を置いちゃってるけども、最低限のコード</span>
    <span class="comment">// a, b を N 次元空間上の点とみなして、その間の距離の2乗、</span>
    <span class="comment">// 要するに「差の2乗和」を求める。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">float</span> DistanceSquared(<span class="reserved">float</span>[] a, <span class="reserved">float</span>[] b)
    {
        <span class="reserved">var</span> d = 0f;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; i++)
        {
            <span class="reserved">var</span> dif = b[i] - a[i];
            d += dif * dif;
        }
        <span class="reserved">return</span> d;
    }
}
</code></pre>

ここで、汎用性を気にすると以下のような要望が出てきます。

- 数値の型: `float`以外にも使いたい
- 数値の「組」の型: 配列じゃなくしたい
- 距離計算の方法: 2乗和(いわゆる[ユークリッド距離](https://ja.wikipedia.org/wiki/%E3%83%A6%E3%83%BC%E3%82%AF%E3%83%AA%E3%83%83%E3%83%89%E8%B7%9D%E9%9B%A2) )だけが距離じゃない

## <a id="arithmetic">数値の型</a>

距離計算に使う演算は、和、差、積程度です。
あと、「同じ点かどうか」は知りたいことが多いので、等値比較はしたいでしょう。
距離を計算したあと、「一定範囲に収まっているかどうか」を判定することが多いので、大小比較くらいは必要です。

とはいえ、`float`に限らず、数値型ならどの型でもこの条件くらいは満たします。
実際例えば、「四角いマス目の上の点」みたいなのを考えると`float`よりも`int`が使いたくなります。
精度が必要な場合には`double`や`decimal`を使いたくなるでしょうし、省メモリ都合で`short`を使いたい場合もあるでしょう。

となったときに問題になるのが、C# では、数値の四則演算を素直にジェネリックにできないこと。
以下のコードはコンパイルできません。

<pre class="source" title="ジェネリックな型には演算子が使えない">
<code><span class="comment">// int や double でも使いたいからと言って、以下のようには書けない。</span>
<span class="comment">// ジェネリックな型 T には +, -, * が定義されていない。</span>
<span class="reserved">class</span> <span class="type">Euclidean</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> DistanceSquared(<span class="type">T</span>[] a, <span class="type">T</span>[] b)
    {
        <span class="type">T</span> d = <span class="error">0</span>;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; i++)
        {
            <span class="reserved">var</span> dif = b[i] <span class="error">-</span> a[i];
            d += dif * dif;
        }
        <span class="reserved">return</span> d;
    }
}
</code></pre>

しょうがなく、以下のように書いたりします。

<pre class="source" title="">
<code><span class="reserved">interface</span> <span class="type">IArithmetic</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span> Zero { <span class="reserved">get</span>; }
    <span class="type">T</span> Add(<span class="type">T</span> a, <span class="type">T</span> b);
    <span class="type">T</span> Subtract(<span class="type">T</span> a, <span class="type">T</span> b);
    <span class="type">T</span> Multiply(<span class="type">T</span> a, <span class="type">T</span> b);
}

<span class="reserved">class</span> <span class="type">Euclidean</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 四則演算用のインターフェイスを外からもらう</span>
    <span class="type">IArithmetic</span>&lt;<span class="type">T</span>&gt; _arithmetic;
    <span class="reserved">public</span> Euclidean(<span class="type">IArithmetic</span>&lt;<span class="type">T</span>&gt; arithmetic) =&gt; _arithmetic = arithmetic;


    <span class="comment">// static にするのはあきらめる</span>
    <span class="reserved">public</span> <span class="type">T</span> DistanceSquared(<span class="type">T</span>[] a, <span class="type">T</span>[] b)
    {
        <span class="reserved">var</span> arith = _arithmetic;
        <span class="comment">// IArithmetic&lt;T&gt; 越しに 0 をもらったり、四則演算したり</span>
        <span class="reserved">var</span> d = arith.Zero;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; i++)
        {
            <span class="reserved">var</span> dif = arith.Subtract(b[i], a[i]);
            <span class="reserved">var</span> sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        <span class="reserved">return</span> d;
    }
}
</code></pre>

が、これだと、

- `IArithmetic<T>`や`Euclidean<T>`のインスタンスを持ちまわすのが大変面倒
- 仮想呼び出しのせいで[インライン化](../../../../study/csharp/structured/miscinlining.md)が効かなくなってものすごく遅い

という問題があります。

で、ちょっとしたトリックなんですが、[値型ジェネリックを使うとインライン化が効く](../../../../study/csharp/oop/sp2_generics.md#pseudo-static)という黒魔術がありまして。
以下のように書けば倍は速くなります。

<pre class="source" title="値型ジェネリックで四則演算">
<code><span class="reserved">class</span> <span class="type">Euclidean</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>&gt;
    <span class="comment">// 構造体にして、型引数で受け取る</span>
    <span class="reserved">where</span> <span class="type">TArithmetic</span> : <span class="reserved">struct</span>, <span class="type">IArithmetic</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> DistanceSquared(<span class="type">T</span>[] a, <span class="type">T</span>[] b)
    {
        <span class="comment">// default を使って IArithmetic&lt;T&gt; を作る</span>
        <span class="reserved">var</span> arith = <span class="reserved">default</span>(<span class="type">TArithmetic</span>);
        <span class="comment">// あとは先ほどと同じ</span>
        <span class="reserved">var</span> d = arith.Zero;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; i++)
        {
            <span class="reserved">var</span> dif = arith.Subtract(b[i], a[i]);
            <span class="reserved">var</span> sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        <span class="reserved">return</span> d;
    }
}

<span class="reserved">struct</span> <span class="type">FloatArithmetic</span> : <span class="type">IArithmetic</span>&lt;<span class="reserved">float</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">float</span> Zero =&gt; 0;
    <span class="reserved">public</span> <span class="reserved">float</span> Add(<span class="reserved">float</span> a, <span class="reserved">float</span> b) =&gt; a + b;
    <span class="reserved">public</span> <span class="reserved">float</span> Multiply(<span class="reserved">float</span> a, <span class="reserved">float</span> b) =&gt; a - b;
    <span class="reserved">public</span> <span class="reserved">float</span> Subtract(<span class="reserved">float</span> a, <span class="reserved">float</span> b) =&gt; a * b;
}

<span class="comment">// IntArithmetic, DoubleArithmetic, ...</span>
<span class="comment">// 使いたい型の分だけ同じ IArithmetic&lt;T&gt; を書く</span>

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// FloatArithmetic の時点で T は float で確定なんだけど、残念ながら型推論はされない</span>
        <span class="comment">// 常にこの2つの型引数をペアで渡さないといけない</span>
        Euclidean&lt;<span class="reserved">float</span>, <span class="type">FloatArithmetic</span>&gt;.DistanceSquared(<span class="reserved">new</span>[] { 1f, 2f }, <span class="reserved">new</span>[] { 3f, 4f });
    }
}
</code></pre>

一応これで、最初の `float` 専用で書いたコードに近いパフォーマンスになります。
まあ、面倒も多々あって、特に大変なのが、型引数を常にペアで渡さないと行けなくなる部分です。
この先、さらにどんどん面倒になって行くんですが、もうすでにこの時点で相当面倒です…

## <a id="fixed-array">数値の「組」の型</a>

次の課題は、配列を避けたいという点。
前述の例でも、メソッド呼び出しの際に `new[] { 1f, 2f }` とか書いていますが、
配列は[ヒープ](../../../../study/computer/essential-software/memorymanagement.md#heap)を使ってしまうので、
今回のような用途ではパフォーマンス上、あまり好ましくありません。

今回の用途だと、

- 要素数が常に固定
- しかも、よく使うのはせいぜい2次元か3次元

ということで、配列の代わりに以下のような構造体を使いたくなったりします。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Array1</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Item1;
}

<span class="reserved">struct</span> <span class="type">Array2</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Item1;
    <span class="reserved">public</span> <span class="type">T</span> Item2;
}

<span class="reserved">struct</span> <span class="type">Array3</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Item1;
    <span class="reserved">public</span> <span class="type">T</span> Item2;
    <span class="reserved">public</span> <span class="type">T</span> Item3;
}

<span class="comment">// 以下、必要なだけ ArrayN を用意</span>
</code></pre>

で、以下の理由から、こいつに対しても先ほどと同様の「値型ジェネリックを使ったトリック」が必要になります。

- 固定長の配列なんだから、長さを静的に取得したい
- 構造体は、自身のフィールドを `ref` 戻り値で返せない

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Array2</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Item1;
    <span class="reserved">public</span> <span class="type">T</span> Item2;

    <span class="comment">// これをジェネリックに使いたければトリックが必要</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> Length =&gt; 2;

    <span class="comment">// ただでさえ、safe にインデックス アクセスを実現する方法はないんだけど…</span>
    <span class="comment">// そもそも、C# の構造体は ref Item1 したものを、ref 戻り値では返せない仕様</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> <span class="type">Unsafe</span>.Add&lt;T&gt;(<span class="reserved">ref</span> <span class="error">Item1</span>, index);
}
</code></pre>

その結果、行きつく先は以下のようなコードになります。

<pre class="source" title="値型ジェネリックを使った固定長配列">
<code><span class="comment">// 配列自体用。これは大して意味は持ってない。誤用防止程度</span>
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IFixedArray</span>&lt;<span class="type">T</span>&gt; { }

<span class="comment">// 値型ジェネリック トリック用</span>
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IFixedArrayAccessor</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
    <span class="reserved">where</span> <span class="type">TArray</span> : <span class="reserved">struct</span>, <span class="type">IFixedArray</span>&lt;T&gt;
{
    <span class="type">TArray</span> New();
    <span class="reserved">ref</span> <span class="type">T</span> At(<span class="reserved">ref</span> <span class="type">TArray</span> array, <span class="reserved">int</span> i);
    <span class="reserved">int</span> Length { <span class="reserved">get</span>; }
}

<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Fixed2</span>&lt;<span class="type">T</span>&gt; : <span class="type">IFixedArrayAccessor</span>&lt;<span class="type">T</span>, <span class="type">Fixed2</span>&lt;T&gt;.<span class="type">Array</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Array</span> : <span class="type">IFixedArray</span>&lt;<span class="type">T</span>&gt;
    {
        <span class="reserved">public</span> <span class="type">T</span> Item1; <span class="reserved">public</span> <span class="type">T</span> Item2;
        <span class="reserved">public</span> Array(<span class="type">T</span> item1, <span class="type">T</span> item2) =&gt; (Item1, Item2) = (item1, item2);
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> Array((<span class="type">T</span>, <span class="type">T</span>) value) =&gt; <span class="reserved">new</span> <span class="type">Array</span>(value.Item1, value.Item2);
    }

    <span class="reserved">public</span> <span class="type">Array</span> New() =&gt; <span class="reserved">default</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> Length =&gt; 2;
    <span class="reserved">public</span> <span class="reserved">unsafe</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt; AsSpan(<span class="reserved">ref</span> <span class="type">Array</span> array) =&gt; <span class="reserved">new</span> Span&lt;<span class="type">T</span>&gt;(Unsafe.AsPointer(<span class="reserved">ref</span> array.Item1), 2);
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> At(<span class="reserved">ref</span> <span class="type">Array</span> array, <span class="reserved">int</span> i) =&gt; <span class="reserved">ref</span> AsSpan(<span class="reserved">ref</span> array)[i];
    <span class="comment">// 範囲チェックをさぼる(危険でいい)なら以下の書き方でも OK</span>
    <span class="comment">//public ref T At(ref Array array, int i) =&gt; ref Unsafe.Add(ref array.Item1, i);</span>
}
</code></pre>

この時点で結構悩ましいコードですが、されにこれを距離計算に組み込むと以下のようになります。

<pre class="source" title="固定長配列を距離計算に組み込み">
<code><span class="reserved">class</span> <span class="type">Euclidean</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>, <span class="type">TArray</span>, <span class="type">TArrayAccessor</span>&gt;
    <span class="reserved">where</span> <span class="type">TArithmetic</span> : <span class="reserved">struct</span>, <span class="type">I/OArithmetic</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">TArray</span> : <span class="reserved">struct</span>, <span class="type">IFixedArray</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">TArrayAccessor</span> : <span class="reserved">struct</span>, <span class="type">IFixedArrayAccessor</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> DistanceSquared(<span class="type">TArray</span> a, <span class="type">TArray</span> b)
    {
        <span class="reserved">var</span> arith = <span class="reserved">default</span>(<span class="type">TArithmetic</span>);
        <span class="reserved">var</span> accessor = <span class="reserved">default</span>(<span class="type">TArrayAccessor</span>);
        <span class="reserved">var</span> d = arith.Zero;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; accessor.Length; i++)
        {
            <span class="reserved">var</span> dif = arith.Subtract(accessor.At(<span class="reserved">ref</span> b, i), accessor.At(<span class="reserved">ref</span> a, i));
            <span class="reserved">var</span> sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        <span class="reserved">return</span> d;
    }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// これも、Fixed2&lt;float&gt; を使う時点で残りの型引数確定なんだけど、残念ながら型推論はされない</span>
        <span class="comment">// 常にこの4つの型引数が必要</span>
        <span class="type">Euclidean</span>&lt;<span class="reserved">float</span>, <span class="type">FloatArithmetic</span>, <span class="type">Fixed2</span>&lt;<span class="reserved">float</span>&gt;.<span class="type">Array</span>, <span class="type">Fixed2</span>&lt;<span class="reserved">float</span>&gt;&gt;.DistanceSquared((1, 2), (3, 4));
    }
}
</code></pre>

型引数が4つに増えました。
しかし、実際のところ意味がある情報は、`float`、「2次元」の2つだけです。
気持ち的には `Euclidean<float, 2>` とだけ書きたいですが、C# では叶いません。

## <a id="metric">距離計算の方法</a>

最後に、距離の計算自体も汎用化してみましょう。

距離にもいろいろあります。
ぶっちゃけ、「非負」「三角不等式が成り立つ」の2点だけ満たしていれば何でも距離です。
[ユークリッド距離](https://ja.wikipedia.org/wiki/%E3%83%A6%E3%83%BC%E3%82%AF%E3%83%AA%E3%83%83%E3%83%89%E8%B7%9D%E9%9B%A2)以外でそこそこよく使うものだと以下のようなものがあります。

- [マンハッタン距離](https://ja.wikipedia.org/wiki/%E3%83%9E%E3%83%B3%E3%83%8F%E3%83%83%E3%82%BF%E3%83%B3%E8%B7%9D%E9%9B%A2)
  - 絶対値の和
  - 京都やマンハッタンの街みたいに碁盤の目になっている都市での2点間の距離
  - 「一定距離にある点」をつなぐと、ダイアモンド型になる
- [チェビシェフ距離](https://ja.wikipedia.org/wiki/%E3%83%81%E3%82%A7%E3%83%93%E3%82%B7%E3%82%A7%E3%83%95%E8%B7%9D%E9%9B%A2)
  - 絶対値の最大値
  - チェスや将棋みたいに、斜めにも動ける駒にとっての盤面の距離
  - 「一定距離にある点」をつなぐと、四角になる

これも、汎用化するだけならインターフェイスを1個用意するだけなんですが、
パフォーマンスを考えると値型ジェネリックを使うことになります。
行きつく先が以下のようなコード。

<pre class="source" title="距離もジェネリック化">
<code><span class="reserved">interface</span> <span class="type">IMetric</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
    <span class="reserved">where</span> <span class="type">TArray</span> : <span class="reserved">struct</span>, <span class="type">IFixedArray</span>&lt;T&gt;
{
    <span class="type">T</span> DistanceSquared(<span class="type">TArray</span> a, <span class="type">TArray</span> b);
}

<span class="reserved">struct</span> <span class="type">EuclideanMetric</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>, <span class="type">TArray</span>, <span class="type">TArrayAccessor</span>&gt; : <span class="type">IMetric</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
    <span class="reserved">where</span> <span class="type">TArithmetic</span> : <span class="reserved">struct</span>, <span class="type">IArithmetic</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">TArray</span> : <span class="reserved">struct</span>, <span class="type">IFixedArray</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">TArrayAccessor</span> : <span class="reserved">struct</span>, <span class="type">IFixedArrayAccessor</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> DistanceSquared(<span class="type">TArray</span> a, <span class="type">TArray</span> b)
    {
        <span class="reserved">var</span> arith = <span class="reserved">default</span>(<span class="type">TArithmetic</span>);
        <span class="reserved">var</span> accessor = <span class="reserved">default</span>(<span class="type">TArrayAccessor</span>);
        <span class="reserved">var</span> d = arith.Zero;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; accessor.Length; i++)
        {
            <span class="reserved">var</span> dif = arith.Subtract(accessor.At(<span class="reserved">ref</span> b, i), accessor.At(<span class="reserved">ref</span> a, i));
            <span class="reserved">var</span> sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        <span class="reserved">return</span> d;
    }
}

<span class="comment">// Manhattan とか Chebychev とかも同様に作る</span>

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 近い方の点を求める</span>
    <span class="reserved">static</span> <span class="type">TArray</span> Nearest&lt;<span class="type">T</span>, <span class="type">TArray</span>, <span class="type">TMetric</span>&gt;(<span class="type">TArray</span> origin, <span class="type">TArray</span> a, <span class="type">TArray</span> b)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IComparable</span>&lt;<span class="type">T</span>&gt;
        <span class="reserved">where</span> <span class="type">TArray</span> : <span class="reserved">struct</span>, <span class="type">IFixedArray</span>&lt;<span class="type">T</span>&gt;
        <span class="reserved">where</span> <span class="type">TMetric</span> : <span class="reserved">struct</span>, <span class="type">IMetric</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
    {
        <span class="reserved">var</span> metric = <span class="reserved">default</span>(<span class="type">TMetric</span>);

        <span class="reserved">var</span> da = metric.DistanceSquared(origin, a);
        <span class="reserved">var</span> db = metric.DistanceSquared(origin, b);

        <span class="reserved">return</span> da.CompareTo(db) &lt;= 0 ? a : b;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 型引数は3つと思いきや、Euclidean がさらに4つ求めるので合計7つ</span>
        <span class="comment">// 常にこの7つの型引数が必要</span>
        <span class="reserved">var</span> n = Nearest&lt;<span class="reserved">float</span>, <span class="type">Fixed2</span>&lt;<span class="reserved">float</span>&gt;.<span class="type">Array</span>, <span class="type">EuclideanMetric</span>&lt;<span class="reserved">float</span>, <span class="type">FloatArithmetic</span>, <span class="type">Fixed2</span>&lt;<span class="reserved">float</span>&gt;.<span class="type">Array</span>, <span class="type">Fixed2</span>&lt;<span class="reserved">float</span>&gt;&gt;&gt;(
            (0, 0), (1, 2), (3, 4));

        <span class="type">Console</span>.WriteLine((n.Item1, n.Item2));
    }
}
</code></pre>

型引数だけで画面の横幅目いっぱい使うようなメソッドができました…
もちろん、意味がある部分は`float`, `2`, `Euclidean`だけで、残りは冗長です。

## <a id="instantiation">ごまかし</a>

[というようなコードを書くことに最近迫られまして](https://github.com/ufcpp/KdTree/)。
(元々公開されていたリポジトリからフォークして、上記のようなトリックを仕込んでパフォーマンス向上する作業をした。)
汎用化を捨てたり、パフォーマンスをあきらめてもよかったんですが。
なんとなくきっちりやっちゃいまして。

最初はしょうがなく7つの冗長な型引数を書いてたんですが、
やっぱりすぐにつらくなって断念。
代わりに、以下のようなごまかしコードを書くことになりました。

<pre class="source" title="派生でごまかす">
<code><span class="comment">// ジェネリックな型を1個用意しておいて、派生で型引数を与えておく</span>
<span class="comment">// 数値の型</span>
<span class="reserved">class</span> <span class="type">FloatPoint</span> : <span class="type">Point</span>&lt;<span class="reserved">float</span>, <span class="type">FloatArithmetic</span>&gt; { }
<span class="reserved">class</span> <span class="type">DoublePoint</span> : <span class="type">Point</span>&lt;<span class="reserved">double</span>, <span class="type">DoubleArithmetic</span>&gt; { }
<span class="reserved">class</span> <span class="type">IntPoint</span> : <span class="type">Point</span>&lt;<span class="reserved">int</span>, <span class="type">IntArithmetic</span>&gt; { }
<span class="reserved">class</span> <span class="type">ShortPoint</span> : <span class="type">Point</span>&lt;<span class="reserved">short</span>, <span class="type">ShortArithmeti</span>c&gt; { }

<span class="reserved">class</span> <span class="type">Point</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>&gt;
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IComparable</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">TArithmetic</span> : <span class="reserved">struct</span>, <span class="type">IArithmetic</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 数値の「組」の型</span>
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">_1</span> : <span class="type">Dimension</span>&lt;<span class="type">Fixed1</span>&lt;<span class="type">T</span>&gt;.<span class="type">Array</span>, <span class="type">Fixed1</span>&lt;<span class="type">T</span>&gt;&gt; { }
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">_2</span> : <span class="type">Dimension</span>&lt;<span class="type">Fixed2</span>&lt;<span class="type">T</span>&gt;.<span class="type">Array</span>, <span class="type">Fixed2</span>&lt;<span class="type">T</span>&gt;&gt; { }
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">_3</span> : <span class="type">Dimension</span>&lt;<span class="type">Fixed3</span>&lt;<span class="type">T</span>&gt;.<span class="type">Array</span>, <span class="type">Fixed3</span>&lt;<span class="type">T</span>&gt;&gt; { }
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">_4</span> : <span class="type">Dimension</span>&lt;<span class="type">Fixed4</span>&lt;<span class="type">T</span>&gt;.<span class="type">Array</span>, <span class="type">Fixed4</span>&lt;<span class="type">T</span>&gt;&gt; { }

    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Dimension</span>&lt;<span class="type">TArray</span>, <span class="type">TArrayAccessor</span>&gt;
        <span class="reserved">where</span> <span class="type">TArray</span> : <span class="reserved">struct</span>, <span class="type">IFixedArray</span>&lt;<span class="type">T</span>&gt;
        <span class="reserved">where</span> <span class="type">TArrayAccessor</span> : <span class="reserved">struct</span>, <span class="type">IFixedArrayAccessor</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
    {
        <span class="comment">// 距離計算の方法</span>
        <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Euclidean</span> : <span class="type">Metric</span>&lt;<span class="type">EuclideanMetric</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>, <span class="type">TArray</span>, <span class="type">TArrayAccessor</span>&gt;&gt; { }
        <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Manhattan</span> : <span class="type">Metric</span>&lt;<span class="type">ManhattanMetric</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>, <span class="type">TArray</span>, <span class="type">TArrayAccessor</span>&gt;&gt; { }
        <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Chebyshev</span> : <span class="type">Metric</span>&lt;<span class="type">ChebyshevMetric</span>&lt;<span class="type">T</span>, <span class="type">TArithmetic</span>, <span class="type">TArray</span>, <span class="type">TArrayAccessor</span>&gt;&gt; { }

        <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Metric</span>&lt;<span class="type">TMetric</span>&gt;
            <span class="reserved">where</span> <span class="type">TMetric</span> : <span class="reserved">struct</span>, <span class="type">IMetric</span>&lt;<span class="type">T</span>, <span class="type">TArray</span>&gt;
        {
            <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">TArray</span> Nearest(<span class="type">TArray</span> origin, <span class="type">TArray</span> a, <span class="type">TArray</span> b)
            {
                <span class="reserved">var</span> metric = <span class="reserved">default</span>(<span class="type">TMetric</span>);

                <span class="reserved">var</span> da = metric.DistanceSquared(origin, a);
                <span class="reserved">var</span> db = metric.DistanceSquared(origin, b);

                <span class="reserved">return</span> da.CompareTo(db) &lt;= 0 ? a : b;
            }

            <span class="comment">// その他、距離空間に対するアルゴリズムをこの中に書く</span>
        }
    }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 使う側に関してはだいぶ短く書けた</span>
        <span class="reserved">var</span> n = <span class="type">FloatPoint</span>.<span class="type">_2</span>.<span class="type">Euclidean</span>.<span class="type">Nearest</span>(
            (0, 0), (1, 2), (3, 4));

        <span class="type">Console</span>.WriteLine((n.Item1, n.Item2));
    }
}
</code></pre>

一応、使う側のコードはだいぶ短くなり、許容範囲になったかなと思います。
ただ、これはこれで、以下のような問題があって、妥協的です。

- 使いたい型、固定長配列の次元、距離計算の方法が増えるたびに、この派生クラスも追加しないといけない
- クラスの中に入っているので、他のアセンブリで型を追加できない

根本解決できるようなプログラミング言語を求めるなら、
ジェネリック型引数の推論をもっと頑張ってもらうとか、`float`に対する`FloatArithmetic`みたいなもののペアリングとかの仕様が必要になります。

型の推論は、かなり頑張っている言語もあって、そういう言語ではもうちょっと手短にコードを書けるんですが、
その代わりにコンパイル時間が指数的に跳ね上がったり、
コンパイル エラーが出たときにエラー メッセージが読めた代物じゃなくなったりという弊害があったりします…

`float`に対する`FloatArithmetic`、
`TArray`に対する`TArrayAccessor`みたいなものは、ShapeとかType Classとか呼ばれたりするんですが、
こいつは[将来的に C# に入りそうな雰囲気](../../../2017/2/pickuproslyn0223/index.md)があります。

これが来てくれればだいぶこの手の作業は楽になるんですが。
現状は明確なマイルストーンが切られておらず、「7.X はおろか、8.X でも無理」という扱いです。
最短で、9.0 とかで入ると仮定しても2年以上は先でしょうか…
なので待ってられないので、しょうがなくこんなコードを書くことに…
