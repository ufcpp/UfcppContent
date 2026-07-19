---
title: "小ネタ インデックス付き foreach"
source_url: "https://ufcpp.net/blog/2016/12/tipsindexedforeach/"
content_type: "BlogEntry"
published_at: "2016-12-07T00:23:23"
updated_at: "2016-12-06T15:23:49"
tags: []
umbraco_id: 1984
parent_id: 1969
sort_order: 6
aliases: []
---

# 小ネタ インデックス付き foreach

`foreach` ステートメントで、インデックス付きで列挙したいことが時々あります。
今回は、そういうときの対処方法について。
というか、C# 7が待ち遠しくなる話。

配列や`List<T>`であれば以下のようにも書けます。

<pre class="source" title="やむなく for ステートメント">
<code><span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; length; i++)
{
    <span class="reserved">var</span> item = array[i];
    <span class="type">Console</span>.WriteLine(<span class="string">$"index: </span>{i}<span class="string">, value: </span>{item}<span class="string">"</span>);
}
</code></pre>

`IEnumerable<T>`の場合にはこうは書けず、
現状だと、以下のようにループの外側に1個変数を作る必要があったりします。

<pre class="source" title="やむなく foreach ループの外に変数を置く">
<code><span class="reserved">var</span> i = 0;
<span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> items)
{
    <span class="type">Console</span>.WriteLine(<span class="string">$"index: </span>{i}<span class="string">, value: </span>{item}<span class="string">"</span>);
    i++;
}
</code></pre>

ループの外側に変数`i`が漏れるのが嫌なのと、
あと、`continue`が絡むと`i++`するのが大変になったりします。

`Select`のオーバーロードの1つを使って、以下のような書き方も一応できます。

<pre class="source" title="Select のオーバーロードの1つに、インデックスを拾えるものがある">
<code><span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items.Select((item, index) =&gt; <span class="reserved">new</span> { item, index }))
{
    <span class="type">Console</span>.WriteLine(<span class="string">$"index: </span>{x.index}<span class="string">, value: </span>{x.item}<span class="string">"</span>);
}
</code></pre>

ただ、これだと無駄にオブジェクトが`new`されます(匿名型は参照型なのでヒープ確保が発生します)。ループの中でのヒープ確保はできれば避けたい負担です。
それに、`x.item`みたいな書き方がちょっと嫌な感じです。

[C# 7](../../../../study/csharp/cheatsheet/ap_ver7.md)であれば、[タプル](../../../../study/csharp/datatype/tuples.md)を使うのがいいかもしれません。ついでに、[分解構文](../../../../study/csharp/datatype/deconstruction.md)も使えば多少すっきりします。

<pre class="source" title="[C# 7] タプルがあれば">
<code><span class="reserved">foreach</span> (<span class="reserved">var</span> (item, index) <span class="reserved">in</span> items.Select((item, index) =&gt; (item, index)))
{
    <span class="type">Console</span>.WriteLine(<span class="string">$"index: </span>{index}<span class="string">, value: </span>{item}<span class="string">"</span>);
}
</code></pre>

タプルは値型なので、いくらかヒープ確保が減ります。
また、[分解](../../../../study/csharp/datatype/deconstruction.md)があるおかげで`x.`とか書く必要がなくなりました。

でもまだちょっとうっとおしいですね。
`(item, index) => (item, index)`とか毎度書きたくないです。
拡張メソッドを用意しておきたいところ。

<pre class="source" title="Indexed 拡張メソッド">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">TupleEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;(<span class="type">T</span> item, <span class="reserved">int</span> index)&gt; Indexed&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
    {
        <span class="reserved">if</span> (source == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(source));

        <span class="type">IEnumerable</span>&lt;(<span class="type">T</span> item, <span class="reserved">int</span> index)&gt; impl()
        {
            <span class="reserved">var</span> i = 0;
            <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> source)
            {
                <span class="reserved">yield</span> <span class="reserved">return</span> (item, i);
                ++i;
            }
        }

        <span class="reserved">return</span> impl();
    }
}
</code></pre>

これで、以下のように書けます。

<pre class="source" title="Indexed拡張メソッドの使い方">
<code><span class="reserved">foreach</span> (<span class="reserved">var</span> (item, index) <span class="reserved">in</span> items.Indexed())
{
    <span class="type">Console</span>.WriteLine(<span class="string">$"index: </span>{index}<span class="string">, value: </span>{item}<span class="string">"</span>);
}
</code></pre>

これなら、まあ、悪くはなさそうです。
こういうメソッド、そこそこ使うことがありそう。

ちなみに、今回は[イテレーター](../../../../study/csharp/data/sp2_iterator.md)を使って`Indexed`メソッドを実装しましたが、ガチガチに最適化するなら、以下のように、構造体で実装してヒープ確保をなくすべきかもしれません。

- [Gist: index付きforeach](https://gist.github.com/ufcpp/2b3e1a5821169f6b21ded175ad05c752)
