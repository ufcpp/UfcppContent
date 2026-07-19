---
title: "IEnumerator の別実装"
source_url: "https://ufcpp.net/blog/2018/12/fastenumerator/"
content_type: "BlogEntry"
published_at: "2018-12-19T10:00:28"
updated_at: "2018-12-19T10:05:44"
tags: []
umbraco_id: 2201
parent_id: 2177
sort_order: 18
aliases: []
---

# IEnumerator の別実装

[Devirtualization 最適化](../devirtualization/index.md)の話で仮想呼び出しのコストの話もしました。
そこでもう1つ思い出してほしいのが、[C# 8.0 Async streams](../cs8asyncstreams/index.md)で書いた、
`IAsyncEnumerator<T>`インターフェイスの話。
最終的な決定としては以下のような API を持っています。

<pre class="source" title="最終決定時の IAsyncEnumerator">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="type">T</span> Current { <span class="reserved">get</span>; }
    <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; MoveNextAsync();
}
</code></pre>

一方で、検討段階では以下のような API も考えられていました。

<pre class="source" title="検討段階の IAsyncEnumerator">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; WaitForNextAsync();
    <span class="type">T</span> TryGetNext(<span class="reserved">out</span> <span class="reserved">bool</span> success);
}

</code></pre>

今日はこの後者のメリットについての話。

参考コード: [FastEnumeration](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/PerformanceTips/FastEnumeration)

## IEnumerator 版

同様の話は実は `IEnumerator<T>` にも言えます。
正確に言えば `IAsyncEnumerator<T>` の方が `WaitForNextAsync` を持っている分だけ複雑なんですが、とりあえず単純化のために `IEnumerator<T>` で話を進めます。

`IEnumerator<T>` インターフェイス(`System.Collections.Generic`名前空間)は、
歴史的経緯から非ジェネリックな `IEnumerator` インターフェイス(`System.Collections`名前空間)からの派生になっていますが、
そういう歴史的経緯を抜いて考えれば以下のような API を持つインターフェイスです。

<pre class="source" title="IEnumerator インターフェイスの本質">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="reserved">bool</span> MoveNext();
    <span class="type">T</span> Current { <span class="reserved">get</span>; }
}
</code></pre>

一方、冒頭で上げた検討段階の `IAsyncEnumerator<T>` と同じ考え方で、
以下のような API での実装が考えられます。
(区別のために名前をちょっと変えて、`IFastEnumerator` にしています。)

<pre class="source" title="IEnumerator の構造はこっちの方がよかったかもしれない">
<code><span class="reserved">interface</span> <span class="type">IFastEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="type">T</span> TryMoveNext(<span class="reserved">out</span> <span class="reserved">bool</span> success);
}
</code></pre>

要するに、`MoveNext`と`Current`に分かれている機能を、1つのメソッドにまとめた方がいいのではないかという話です。

(.NET のジェネリクスでは、[out 引数](../../../../study/csharp/resource/sp_ref.md#out)を[共変](../../../../study/csharp/oop/sp4_variance.md#covariance)にできないという嫌な制限があるので、`success` の方が out 引数になっています。
可能であれば `bool TryMoveNext(out T current)` にしたいものです。
とりあえずそこは今回関係なく、あくまで「メソッドを1つにしたい」というところが今回の話の本質です。)

## 仮想呼び出しのコスト

まあ、冒頭で仮想呼び出しのコストの話を振っているのでどういう問題なのか察してもらえると思います。
単純に、そこそこコストが掛かる仮想呼び出しを2回に分けたくないという話です。

`foreach` が含まれるメソッドはたいてい[インライン展開](../../../../study/csharp/structured/miscinlining.md)されません。
そうなると devirtualization 最適化は大体かからなくなるんですが、
なので、`MoveNext`/`Current` には普通に仮想呼び出しのコストが掛かります。
結果、仮想呼び出しが2回。
一方の `IFastEnumerator<T>` であれば、仮想呼び出しは `TryMoveNext` の1回だけです。

ということで、どのくらい変わるか[ベンチマーク](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/FastEnumeration/FastEnumeration)を用意しました。

配列の中身を列挙するだけのクラスを2つ用意します。
片方は `IEnumerator<T>` 実装(本題に関係するところだけ抜き出し)。

<pre class="source" title="IEnumerator 実装">
<code><span class="reserved">class</span> <span class="type">NormalEnumerator</span> : <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span>[] _data;
    <span class="reserved">private</span> <span class="reserved">int</span> _i = -1;
 
    <span class="reserved">public</span> <span class="reserved">int</span> Current =&gt; _data[_i];
    <span class="reserved">public</span> <span class="reserved">bool</span> MoveNext() =&gt; ++_i &lt; _data.Length;
}
</code></pre>

もう一方は `IFastEnumerator<T>` 実装。

<pre class="source" title="IFastEnumerator 実装">
<code><span class="reserved">class</span> <span class="type">FastEnumerator</span> : <span class="type">IFastEnumerator</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span>[] _data;
    <span class="reserved">private</span> <span class="reserved">int</span> _i = -1;
 
    <span class="reserved">public</span> <span class="reserved">int</span> TryMoveNext(<span class="reserved">out</span> <span class="reserved">bool</span> success)
    {
        <span class="reserved">var</span> i = ++_i;
        <span class="reserved">var</span> data = _data;
        <span class="reserved">if</span> ((<span class="reserved">uint</span>)i &lt; (<span class="reserved">uint</span>)data.Length)
        {
            success = <span class="reserved">true</span>;
            <span class="reserved">return</span> data[i];
        }
        <span class="reserved">else</span>
        {
            success = <span class="reserved">false</span>;
            <span class="reserved">return</span> <span class="reserved">default</span>;
        }
    }
}

</code></pre>

これに対して以下のようなループを回します。
(`IEnumerator` 版の方は、まんま、`foreach` の展開結果です。)

<pre class="source" title="インターフェイスを介して呼ぶと MoveNext/Current のコストが気になる">
<code><span class="reserved">static</span> <span class="reserved">int</span> VirtualSum(<span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt; e)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">while</span> (e.MoveNext())
    {
        <span class="reserved">var</span> x = e.Current;
        sum += x;
    }
    <span class="reserved">return</span> sum;
}

<span class="comment">// IFastEnumerator の方が1.5倍くらい速い。</span>
<span class="reserved">static</span> <span class="reserved">int</span> VirtualSum(<span class="type">IFastEnumerator</span>&lt;<span class="reserved">int</span>&gt; e)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">while</span> (<span class="reserved">true</span>)
    {
        <span class="reserved">var</span> x = e.TryMoveNext(<span class="reserved">out</span> <span class="reserved">var</span> success);
        <span class="reserved">if</span> (!success) <span class="reserved">break</span>;
        sum += x;
    }
    <span class="reserved">return</span> sum;
}

</code></pre>

これの結果は、`IFastEnumerator` 版の方が1.5倍くらい高速です。
`TryMoveNext` の方が複雑なコードになりがちなので最適化も効きにくいんですが、
それ以上に仮想呼び出しのコストが高くて、`TryMoveNext` の方が速くなります。

## インターフェイスをやめてしまえば…

ちなみに、あくまでこれは仮想呼び出しのコストの問題なので、
以下のように、インターフェイスを介さず具象クラスで呼ぶと、
むしろ `MoveNext`/`Current` 型の方が速くなります。

<pre class="source" title="具象型で呼ぶと別に MoveNext/Current のコストは気にならない">
<code><span class="comment">// さっきとの違いは引数の型だけ。</span>
<span class="comment">// IEnumerator インターフェイスだったのを、NormalEnumerator クラスに変えただけ。</span>
<span class="comment">// この場合は普通にこっちの方が速い。</span>
<span class="reserved">static</span> <span class="reserved">int</span> NonVirtualSum(<span class="type">NormalEnumerator</span> e)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">while</span> (e.MoveNext())
    {
        <span class="reserved">var</span> x = e.Current;
        sum += x;
    }
    <span class="reserved">return</span> sum;
}
 
<span class="comment">// 同じく、IFastEnumerator インターフェイスを FastEnumerator クラスに変えただけ。</span>
<span class="reserved">static</span> <span class="reserved">int</span> NonVirtualSum(<span class="type">FastEnumerator</span> e)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">while</span> (<span class="reserved">true</span>)
    {
        <span class="reserved">var</span> x = e.TryMoveNext(<span class="reserved">out</span> <span class="reserved">var</span> success);
        <span class="reserved">if</span> (!success) <span class="reserved">break</span>;
        sum += x;
    }
    <span class="reserved">return</span> sum;
}
</code></pre>

とはいえ、汎用性がなくなるので具象クラスで受け渡しするのはちょっとつらいです。
ジェネリクスを使えば多少緩和はされるんですが…

<pre class="source" title="ジェネリック版">
<code><span class="comment">// ジェネリクスを使えば、構造体の時には仮想呼び出しが消える。</span>
<span class="comment">// (構造体限定。クラスの時は別に仮想呼び出しは消えない。)</span>
<span class="reserved">static</span> <span class="reserved">int</span> GenericSum&lt;<span class="type">T</span>&gt;(<span class="type">T</span> e)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">while</span> (e.MoveNext())
    {
        <span class="reserved">var</span> x = e.Current;
        sum += x;
    }
    <span class="reserved">return</span> sum;
}
</code></pre>

速くなる(仮想呼び出しが消える)のは構造体限定です。
しかもなお悪いことに、`foreach` は、`GetEnumerator` を介する構造なので、
普通にやるとどうやっても仮想呼び出しが消えません。

<pre class="source" title="foreach はどうやっても仮想呼び出しが残る">
<code><span class="reserved">static</span> <span class="reserved">int</span> Sum&lt;<span class="type">T</span>&gt;(<span class="type">T</span> items)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items) sum += x;
    <span class="reserved">return</span> sum;
} 
<span class="comment">// ↑は↓みたいに展開される</span>
<span class="reserved">static</span> <span class="reserved">int</span> Sum_&lt;<span class="type">T</span>&gt;(<span class="type">T</span> items)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">var</span> sum = 0;
 
    <span class="comment">// この GetEnumerator の仮想呼び出しは消える可能性があるものの…</span>
    <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt; e = items.GetEnumerator();
    <span class="reserved">try</span>
    {
        <span class="comment">// 結局ここの MoveNext/Current はインターフェイス越し。</span>
        <span class="comment">// 必ず仮想呼び出しになる。</span>
        <span class="reserved">while</span> (e.MoveNext())
        {
            <span class="reserved">var</span> x = e.Current;
            sum += x;
        }
    }
    <span class="reserved">finally</span>
    {
        <span class="reserved">if</span> (e <span class="reserved">is</span> <span class="type">IDisposable</span> d) d.Dispose();
    }
    <span class="reserved">return</span> sum;
}
</code></pre>

ということで、`foreach` 中の `MoveNext`/`Current` の仮想呼び出しはなかなか消せなかったりします。
なのでなおのこと、1回で済む `TryMoveNext` 型のインターフェイスがよかったかもしれない、という話になったりします。
