---
title: "C# 14 の破壊的変更点(First-class Span)"
source_url: "https://ufcpp.net/blog/2025/10/first-class-span-breaking-change/"
content_type: "BlogEntry"
published_at: "2025-10-22T21:33:30"
updated_at: "2025-10-22T21:33:30"
tags: []
umbraco_id: 2517
parent_id: 2516
sort_order: 0
aliases: []
---

# C# 14 の破壊的変更点(First-class Span)

C# 14 で導入された [First-class Span](../../../../study/csharp/resource/span.md#first-class-span) は破壊的変更を伴っています。

例えば標準ライブラリの範囲内の拡張メソッド呼び出しでも以下のような差が生じます。

<pre class="source" title="C# 14 にすると挙動が変わる拡張メソッド呼び出しの例">
<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// C# 13 まで: System.Linq.Enumerable.Contains (IEnumerable 引数) が呼ばれる</span>
<span class="comment">// C# 14 から: System.MemoryExtensions.Contains (ReadOnlySpan 引数) が呼ばれる</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">array</span><span class="operator">.</span><span class="method">Contains</span>(<span class="number">2</span>));
</pre>

ほとんどの場合、「パフォーマンスが上がるだけで得られる結果は同じ」な実装ばかりなのでそんなに問題にはならないだろうということで、
「許容できる範囲内」・「破壊的変更を受け入れるメリットの方が大きい」という判定を受けています。
公式の[.NET 10 での破壊的変更に関するドキュメント](https://learn.microsoft.com/ja-jp/dotnet/core/compatibility/core-libraries/10.0/csharp-overload-resolution)では、
「式ツリーで使っていた場合に問題が起きうる」くらいしか書かれていません。

ただ、これのせいで問題を起こしそうだった拡張メソッドとして `Reverse` があったりします。

<pre class="source" title="Reverse は危うかった">
<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// C# 13 まで: Enumerable.Reverse だったから問題なし。</span>
<span class="comment">// C# 14 から: MemoryExtensions.Reverse を呼んじゃいそう…</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="error" title="CS1579"><span class="variable">array</span><span class="operator">.</span><span class="method">Reverse</span>()</span>)
{
}

<span class="comment">// デモ用に同じシグネチャの拡張メソッドをローカル実装。</span>
<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">A</span></span>
{
    <span class="comment">// System.Linq.Enumerable にあるのは「新しい IEnumerable インスタンスを作って返す」タイプ。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type param">TSource</span>&gt; <span class="method"><span class="static">Reverse</span></span>&lt;<span class="type param">TSource</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type param">TSource</span>&gt; <span class="variable local">source</span>) <span class="operator">=&gt;</span> <span class="reserved">null</span><span class="operator">!</span>;

    <span class="comment">// MemoryExtensions にあるのは「Span に対する自己書き換え」タイプ。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Reverse</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>) { }
}
</pre>

ちなみに、この問題は「[`TSource[]` 引数の `Reverse`](https://learn.microsoft.com/ja-jp/dotnet/api/system.linq.enumerable.reverse?view=net-10.0#system-linq-enumerable-reverse-1(-0())) を足す」という方法で解決しています。
`Span<T>` (first-class とはいえ型変換を挟む)よりも `T[]` (無変換)の方が優先度が高いので、
`array.Reverse()` は `Reverse(T[])` が優先的に呼ばれます。

ここまではあくまで「標準ライブラリの範囲内で」の話。
「自作の LINQ もどき」とかを持っているともう少しいろいろと問題を踏みます。
というか、自分が踏んだという話…
それを2つほど紹介。

## Where(Span)

1個目は以下のような `Where` メソッドです。

<pre class="source" title="Span 自己書き換え Where">
<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">InPlaceLinq</span></span>
{
    <span class="comment">// Reverse の例と同様、「Span 相手は自己書き換えでいいだろ」的なメソッド。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="method"><span class="static">Where</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>, <span class="type">Func</span>&lt;<span class="type param">T</span>, <span class="reserved">bool</span>&gt; <span class="variable local">predicate</span>)
    {
        <span class="reserved">int</span> <span class="variable">count</span> <span class="operator">=</span> <span class="number">0</span>;
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="variable local">span</span><span class="operator">.</span><span class="property">Length</span>; <span class="variable">i</span><span class="operator">++</span>)
        {
            <span class="control">if</span> (<span class="variable local">predicate</span>(<span class="variable local">span</span>[<span class="variable">i</span>]))
            {
                <span class="variable local">span</span>[<span class="variable">count</span><span class="operator">++</span>] <span class="operator">=</span> <span class="variable local">span</span>[<span class="variable">i</span>];
            }
        }
        <span class="control">return</span> <span class="variable local">span</span>[..<span class="variable">count</span>];
    }
}
</pre>

先ほどの `Reverse` の例同様、自己書き換え。

自己書き換えな時点で用途はかなり限定的で、
本来は以下のような利用を想定しています。

<pre class="source" title="本来の「自己書き換え Where」利用">
<span class="reserved">struct</span> <span class="type struct">SomeItem</span>
{
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="property">Flag</span> { <span class="reserved">get</span>; }
}

<span class="reserved">class</span> <span class="type">SomeRepository</span>
{
    <span class="reserved">public</span> <span class="type struct">SomeItem</span>[] <span class="method">Filter</span>(<span class="type">Func</span>&lt;<span class="type struct">SomeItem</span>, <span class="reserved">bool</span>&gt; <span class="variable local">predicate</span>)
    {
        <span class="comment">// 個数の上限がある程度わかってる &amp; 小さいので stackalloc でバッファー確保。</span>
        <span class="type struct">Span</span>&lt;<span class="type struct">SomeItem</span>&gt; <span class="variable">buffer</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="type struct">SomeItem</span>[<span class="number">32</span>];

        <span class="comment">// 一覧を取る時点では Where 出来ず一度バッファーに書き込みが必要なメソッドを呼ぶ。</span>
        <span class="reserved">var</span> <span class="variable">written</span> <span class="operator">=</span> <span class="method">GetItems</span>(<span class="variable">buffer</span>);

        <span class="comment">// 最終的には ToArray して返す。</span>
        <span class="control">return</span> [.. <span class="variable">buffer</span>[..<span class="variable">written</span>]<span class="operator">.</span><span class="method">Where</span>(<span class="variable local">predicate</span>)];
    }

    <span class="reserved">private</span> <span class="reserved">int</span> <span class="method">GetItems</span>(<span class="type struct">Span</span>&lt;<span class="type struct">SomeItem</span>&gt; <span class="variable local">destination</span>)
    {
        <span class="comment">// 本来は以下の類のコード</span>
        <span class="comment">// destination[count++] = ...</span>
        <span class="comment">// return count;</span>
        <span class="control">return</span> <span class="number">0</span>;
    }
}
</pre>

ところが、first-class Span が入ったことで、配列に対して `Enumerable.Where` よりも優先度が高くなってしまい…
意図しないところで呼ばれてしまうことに…

<pre class="source" title="意図せず「自己書き換え Where」の方が呼ばれてしまった例">
<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// C# 13 まで: System.Linq.Enumerable.Where が呼ばれる。</span>
<span class="comment">// C# 14 から: InPlaceLinq.Where が呼ばれる。自己書き換え…</span>
<span class="reserved">var</span> <span class="variable">result</span> <span class="operator">=</span> <span class="variable">array</span><span class="operator">.</span><span class="method">Where</span>(<span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">%</span> <span class="number">2</span> <span class="operator">==</span> <span class="number">0</span>);

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;result&quot;</span>);
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">result</span>)
{
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);
}

<span class="comment">// C# 14 だと自己書き換えやっちゃってるんで当然…</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;array&quot;</span>);
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">array</span>)
{
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>); <span class="comment">// 2, 4, 3, 4 になっちゃう。</span>
}
</pre>

この例の特に厳しいところは、コンパイル エラーにはならずに実行できてしまうものの、
実行結果が破滅的に意図しない挙動になるところです…

一応、`Reverse` の例同様、`Where(TSource[], Func<TSource, bool>)` オーバーロードを足して `Enumerable.Where` に流すようにしてしまえば解決できるはずです。

また、自己書き換えな拡張メソッドが非破壊なものと同名なのが問題だったという反省もあり、
メソッド名を変更してしまうべきとう気もします。
(実際、この路線で修正。`WhereInPlace` というあえての長ったらしい名前に変更。)

## Index(ReadOnlySpan)

もう1個は以下のような `Index` メソッド。

<pre class="source" title="">
<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">SpanExtensions</span></span>
{
    <span class="comment">// 要は「Span 相手にも Enumerable.Index みたいなものが欲しい」。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">IndexEnumerable</span>&lt;<span class="type param">T</span>&gt; <span class="static"><span class="method">Index</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">span</span>);

    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">IndexEnumerable</span>&lt;<span class="type param">T</span>&gt;(<span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>)
    {
        <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="field">_span</span> <span class="operator">=</span> <span class="variable local">span</span>;
        <span class="reserved">public</span> <span class="type struct">IndexEnumerator</span>&lt;<span class="type param">T</span>&gt; <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="field">_span</span>);
    }

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">IndexEnumerator</span>&lt;<span class="type param">T</span>&gt;(<span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>)
    {
        <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="field">_span</span> <span class="operator">=</span> <span class="variable local">span</span>;
        <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_index</span> <span class="operator">=</span> <span class="operator">-</span><span class="number">1</span>;
        <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() <span class="operator">=&gt;</span> <span class="operator">++</span><span class="field">_index</span> <span class="operator">&lt;</span> <span class="field">_span</span><span class="operator">.</span><span class="property">Length</span>;
        <span class="reserved">public</span> <span class="reserved">readonly</span> (<span class="reserved">int</span> Index, <span class="type param">T</span> Item) <span class="property">Current</span> <span class="operator">=&gt;</span> (<span class="field">_index</span>, <span class="field">_span</span>[<span class="field">_index</span>]);
    }
}
</pre>

こちらは `Enumerable.Index(this IEnumerable<T>)` と同じ挙動を `ReadOnlySpan<T>` 引数で実装したものです。

まあ、`Span`/`ReadOnlySpan` を使いたいくらいパフォーマンスを気にする場面なら `for (var i = 0; i < span.Length; ++i)` を使えという話はありつつも…

同じ挙動なので、配列に対して呼ばれても問題ないはずでめでたしめでたし(?)

<pre class="source" title="">
<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// C# 13 まで: System.Linq.Enumerable.Index が呼ばれる。</span>
<span class="comment">// C# 14 から: 自作の SpanExtensions.Index が呼ばれる。</span>
<span class="comment">//</span>
<span class="comment">// 挙動は同じなので特に問題ない。</span>
<span class="comment">// むしろパフォーマンス上がるのではないかと。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> (<span class="variable">index</span>, <span class="variable">item</span>) <span class="control">in</span> <span class="variable">array</span><span class="operator">.</span><span class="method">Index</span>())
{
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span>{<span class="variable">index</span>}<span class="string">: </span>{<span class="variable">item</span>}<span class="string">&quot;</span>);
}
</pre>

問題は `IndexEnumerable` が `ref struct` な点で、
`foreach` 中に `yield` や `await` があるとエラーになります。

<pre class="source" title="">
<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// C# 14 で 自作の SpanExtensions.Index が呼ばれようになると…</span>
<span class="error" title="CS4007"><span class="control">foreach</span> (<span class="reserved">var</span> (<span class="variable">index</span>, <span class="variable">item</span>) <span class="control">in</span> <span class="variable">array</span><span class="operator">.</span><span class="method">Index</span>())</span>
{
    <span class="comment">// さっきとの差は await を含んでることだけ。</span>
    <span class="control">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Delay</span></span>(<span class="number">1</span>);
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span>{<span class="variable">index</span>}<span class="string">: </span>{<span class="variable">item</span>}<span class="string">&quot;</span>);
}
</pre>

「 'SpanExtensions.IndexEnumerator&lt;int&gt;' 型のインスタンスは、'await' または 'yield' 境界を越えて保持することはできません。」というコンパイル エラーが出るはずです。

これはまあ、ほとんどの場合は `Index(ReadOnlySpan)` が呼ばれた方が好ましい中、
少数の `yield`/`await` を含むケースでだけ問題になるので、
コンパイル エラーが出た場所を `Enumerable.Index` (拡張メソッドをやめて静的メソッド呼び)に書き換えるなどで対処しました。
(それか、名前空間の内側に `using static System.Linq.Enumerable;` を書いて `Enumerable.Index` の優先度を上げるという手もあります。)

あとこれも一応、配列用のオーバーロード追加でも問題解消するはずです。
