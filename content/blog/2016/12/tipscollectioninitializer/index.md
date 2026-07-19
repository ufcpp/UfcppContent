---
title: "小ネタ コレクション初期化子"
source_url: "https://ufcpp.net/blog/2016/12/tipscollectioninitializer/"
content_type: "BlogEntry"
published_at: "2016-12-06T00:00:49"
updated_at: "2016-12-05T15:01:13"
tags: []
umbraco_id: 1983
parent_id: 1969
sort_order: 5
aliases: []
---

# 小ネタ コレクション初期化子

昨日のオブジェクト初期化子に続き、今日はコレクション初期化子の話。

コレクション初期化子ってのは、例えば以下のようなやつのことです。

<pre class="source" title="コレクション初期化子">
<code><span class="comment">// この、{} の部分がコレクション初期化子。</span>
<span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { 1, 2, 3, 4, 5 };
</code></pre>

このコレクション初期化を使える条件は、`Add` メソッドを持っていて、かつ、 `IEnumerable` を実装していることです。

最低限の実装をしてみると、以下のような感じ。

<pre class="source" title="コレクション初期化子の最低限の条件を満たす例">
<code><span class="reserved">class</span> <span class="type">MyList</span> : <span class="type">IEnumerable</span>
{
    <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; _list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
    <span class="reserved">public</span> <span class="reserved">void</span> Add(<span class="reserved">int</span> value) =&gt; _list.Add(value);
    <span class="reserved">public</span> <span class="type">IEnumerator</span> GetEnumerator() =&gt; _list.GetEnumerator();
}

<span class="reserved">static</span> <span class="reserved">void</span> ListSample()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">MyList</span> { 1, 2, 3, 4, 5 };

    <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> x)
        <span class="type">Console</span>.WriteLine(item);
}
</code></pre>

この、コレクション初期化子は以下のように展開されます。

<pre class="source" title="コレクション初期化子の展開結果">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">MyList</span>();
x.Add(1);
x.Add(2);
x.Add(3);
x.Add(4);
x.Add(5);
</code></pre>

ここで生じる疑問があります: `IEnumerable` の実装、要るの？

## 依存は避けれるなら避けるべきもの

だって、`Add`メソッドしか使ってなくない？`IEnumerable`は何にも使ってないよね？

だいたい、C#の文法が`IEnumerable`に依存しちゃうの？
例えば、`foreach`であれば`GetEnumeartor`メソッドさえ持っていれば、別に`IEnumerable`インターフェイスを実装していない型であっても使えます。
LINQもそうで、`Select`や`Where`など、所定のメソッドさえ持っていれば、クエリ式を使えます。

最近、Build Insiderで[Task-likeの話](http://www.buildinsider.net/column/iwanaga-nobuyuki/009)とかも書きましたけど、
言語の文法が何かの型に依存するというのはリスクを持ちます。
可能なら避けるべきものです。

で、コレクション初期化子、`IEnumerable` 要るの？

## たぶん、誤用の防止

まあ、問題になるとすると以下のような例ですかね。

<pre class="source" title="コレクション初期化子の誤用の例">
<code><span class="reserved">struct</span> <span class="type">Adder</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Add(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; x + y;
}

<span class="reserved">static</span> <span class="reserved">void</span> AdderSample()
{
    <span class="comment">// こういう誤用を防ぎたかったのかなという気はする</span>
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Adder</span>
    {
        { 2, 1 },
        { 3, 4 },
        { 5, 9 },
    };
}
</code></pre>

`Add`メソッドだけを条件にしてしまうと、こういうコードが書けてしまう。
で、この`Add`の呼ばれ方だと、何の役にも立たないわけです。
`Adder`の内部状態を変えたいわけじゃなてく、単なるオペレーターなわけでして。

もちろん、`IEnumerable`の実装を義務付けたところで、あえて濫用することはできます。
例えば、以下のような書き方なら現在の仕様でもできます。

<pre class="source" title="GetEnumeratorを空実装して無理やりコレクション初期化子を使う例">
<code><span class="reserved">class</span> <span class="type">Accumulator</span> : <span class="type">IEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Sum { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Add(<span class="reserved">int</span> value) =&gt; Sum += value;

    <span class="comment">// 空実装してしまえば、コレクション初期化子の乱用可能</span>
    <span class="reserved">public</span> <span class="type">IEnumerator</span> GetEnumerator() =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">NotSupportedException</span>();
}

<span class="reserved">static</span> <span class="reserved">void</span> AccumulatorSample()
{
    <span class="comment">// コレクションでもないんでもないけど、コレクション初期化子を使える</span>
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Accumulator</span> { 1, 2, 3, 4, 5 };
    <span class="type">Console</span>.WriteLine(x.Sum); <span class="comment">// 15</span>
}
</code></pre>

とりあえず空実装。

まあ、意図的にやってるので大して問題にはならないんですが。
`Adder`みたいなのが意図せずコレクション初期化子で使われるのだけは防止したかったんですかね…
そのために`GetEnumerator`の空実装しろと…
