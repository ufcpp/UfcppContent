---
title: "共変配列事故"
source_url: "https://ufcpp.net/blog/2022/11/covariantarrayincident/"
content_type: "BlogEntry"
published_at: "2022-11-24T23:50:02"
updated_at: "2022-11-24T23:50:02"
tags: []
umbraco_id: 2435
parent_id: 2434
sort_order: 0
aliases: []
---

# 共変配列事故

またちょっと Gist に書き捨ててたコードが増えてきたので供養ブログをしばらく書いていこうかと。

(今年はまだ少な目。一人アドベントカレンダーな量にはならず。)

## 配列の共変性

悪名高いんですが、C# のというか、.NET の配列は[共変](../../../../study/csharp/oop/sp4_variance.md#covariant-array)だったりします。

<pre class="source" title="配列の共変性">
<span class="comment">// ↓.NET 的に許されていはいるものの、 items[0] = new Base(); が例外を起こすので今となってはあんまり使いたくない機能。</span>
<span class="comment">// 意図的に使うことはめったにないものの…</span>
<span class="type">Base</span>[] <span class="variable">items</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Derived</span>[<span class="number">1</span>];

<span class="comment">// これは問題ない</span>
<span class="variable">items</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Derived</span>();

<span class="comment">// これも問題ない。 Base に Derived を代入するのは安全。</span>
<span class="type">Base</span> <span class="variable">item</span> <span class="operator">=</span> <span class="variable">items</span>[<span class="number">0</span>];

<span class="comment">// これがダメ。</span>
<span class="comment">// 実行時例外が出る。</span>
<span class="variable">items</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Base</span>();

<span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span> { }
</pre>

実行時例外出ることわかってるんだからコンパイル時に禁止しろと…
(みんな言ってる。何度でも言ってる。)

`IEnumerable<T>` や `ReadOnlySpan<T>` がある現在では本当に意味不明な仕様なんですが、
まあ、 .NET の最初期(.NET Framework 1.0)の頃はジェネリクスすらなかったので、
やむなくこんな仕様を入れたんだと思います。

ちなみに、実のところ Java も配列が共変で、.NET はそれに右に倣えな感じは多少あります(初期にジェネリクスがなかったのも共通)。

## 事故発生

まあ、この仕様は昔の名残丸出しの気持ち悪い仕様なので、意図的に使うことはほとんどないんですが。
時々事故るんですよねぇ。

`Base[] items = new Derived[1];` とかいうわかりやすいコードならやらないのであって、
型推論が絡むと時々間違っちゃう。

<pre class="source" title="型推論の過程でやらかし">
<span class="comment">// 配列の型推論はソース側(右辺側)からしかやらない。</span>
<span class="comment">// となると…</span>
<span class="type">Base</span>[] <span class="variable">items</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="reserved">new</span> <span class="type">Derived</span>() };

<span class="comment">// 1. new[]{} の中身が Derived である</span>
<span class="comment">// 2. 中身からの型推論で、右辺の型は Derived[] になる</span>
<span class="comment">// 3. Base[] に Derive[] を代入(共変)している</span>

<span class="comment">// はい、アウト。実行時例外が出る。</span>
<span class="variable">items</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Base</span>();

<span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span> { }
</pre>

数年に1度はやっちゃう…

ちなみに今年やったのはもうちょっと複雑で、要点を抜き出すと以下のようなコードでした。

<pre class="source" title="気づきにくい共変配列バグ">
<span class="reserved">var</span> <span class="variable">testData</span> <span class="operator">=</span> <span class="reserved">new</span>[]
{
    <span class="comment">// たくさん new A() が並んでる。</span>
    <span class="reserved">new</span> <span class="type">A</span>
    {
        <span class="field">Child</span> <span class="operator">=</span> <span class="reserved">new</span>()
        {
            <span class="field">Items</span> <span class="operator">=</span> <span class="reserved">new</span>[] <span class="comment">// これの推論結果は Base[] なのでセーフ。</span>
            {
                <span class="reserved">new</span> <span class="type">Base</span>(), <span class="reserved">new</span> <span class="type">Derived</span>(),
            },
        },
    },
    <span class="reserved">new</span> <span class="type">A</span>
    {
        <span class="field">Child</span> <span class="operator">=</span> <span class="reserved">new</span>()
        {
            <span class="field">Items</span> <span class="operator">=</span> <span class="reserved">new</span>[] <span class="comment">// これが Derived[] になってアウト。</span>
            {
                <span class="reserved">new</span> <span class="type">Derived</span>(), <span class="reserved">new</span> <span class="type">Derived</span>(),
            },
        },
    },
    <span class="comment">// たくさん new A() が並んでる。</span>
};

<span class="comment">// いろいろあって最終的に B.Items が Deserialize に渡る。</span>
<span class="comment">// こっちは平気だけど…</span>
<span class="type">Serializer</span><span class="operator">.</span><span class="static"><span class="method">Deserialize</span></span>(<span class="variable">testData</span>[<span class="number">0</span>]<span class="operator">.</span><span class="field">Child</span><span class="operator">!</span><span class="operator">.</span><span class="field">Items</span><span class="operator">!</span>);

<span class="comment">// こっちは実行時例外起こす。</span>
<span class="type">Serializer</span><span class="operator">.</span><span class="static"><span class="method">Deserialize</span></span>(<span class="variable">testData</span>[<span class="number">1</span>]<span class="operator">.</span><span class="field">Child</span><span class="operator">!</span><span class="operator">.</span><span class="field">Items</span><span class="operator">!</span>);

<span class="reserved">class</span> <span class="type">Serializer</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Deserialize</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span>[] <span class="variable local">value</span>)
    {
        <span class="comment">// ちなみに、共変配列が来てるとここの Span へのキャストのタイミングで実行時例外。</span>
        <span class="static"><span class="method">Deserialize</span></span>((<span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt;)<span class="variable local">value</span>);
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Deserialize</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">value</span>)
    {
        <span class="control">foreach</span> (<span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">value</span>)
        {
            <span class="comment">// x = ...</span>
        }
    }
}

<span class="reserved">class</span> <span class="type">A</span> { <span class="reserved">public</span> <span class="type">B</span><span class="operator">?</span> <span class="field">Child</span>; }
<span class="reserved">class</span> <span class="type">B</span> { <span class="reserved">public</span> <span class="type">Base</span>[]<span class="operator">?</span> <span class="field">Items</span>; }

<span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span> { }
</pre>

来年には入るかもと目されている[コレクション リテラル](../../../2021/12/collection-literal/index.md)ではこんな問題起こさないように設計されていそうで。
この時ばかりはかなり本気で、一刻も早くコレクション リテラルに来てほしいと思いました。
