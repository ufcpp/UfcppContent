---
title: "【C# 11 候補】リスト パターン【VS 17.1 p2 で追加予定】"
source_url: "https://ufcpp.net/blog/2021/12/list-pattern/"
content_type: "BlogEntry"
published_at: "2021-12-29T17:27:45"
updated_at: "2021-12-29T17:31:19"
tags: []
umbraco_id: 2396
parent_id: 2375
sort_order: 10
aliases: []
---

# 【C# 11 候補】リスト パターン【VS 17.1 p2 で追加予定】

C# に[パターン](../../../../study/csharp/datatype/patterns.md)がまた1個増えます。
今回はリスト。`is [..]` とかで配列や `List<T>` にマッチ。
これをリスト パターンと言います。

Roslyn 化(C# コンパイラーを C# で書き直し)した初期の頃から、C# の進化の長期テーマになってる ["Programming With Data"](https://github.com/dotnet/csharplang/discussions/3107) の続きです。
以下の表の赤丸を付けたところ。

![リスト パターンの立ち位置](../../../../../assets/media/1205/listpattern.png)

ちなみにこのリスト パターンは Visual Studio 17.1 Preview 2 向けですでに merge 済み。近々動くコンパイラーを実際に触れるはずです。

## <a id="square-bracket">角括弧</a>

リスト パターンには `[]` を使うことになりました。

当初予定は `{}` (プロパティ パターンと被る)とか `[]{}` (これはこれでキモイ)とかも検討されていたんですが。
配列初期化子とかコレクション初期化子との対称性のためでしたが、
構文解析的にきつくて断念。

<pre class="source" title="当初案(没)">
<code><span class="reserved">var</span> <span class="variable">array</span> = <span class="reserved">new</span>[] { 1, 2 };

<span class="comment">// 当初案1:</span>
<span class="comment">// int[] array = { 1, 2 }; との対比。</span>
<span class="comment">// { Length: &gt; 0 } とかとの区別が付かなくて断念。</span>
<span class="control">if</span> (<span class="variable">array</span> <span class="reserved">is</span> { })
{
}

<span class="comment">// 当初案2:</span>
<span class="comment">// var array = new[] { 1, 2 }; との対比。</span>
<span class="comment">// まだ {} の部分がきついのと、length を必要としないときに [] を付けるのがだいぶつらい。</span>
<span class="reserved">const</span> <span class="reserved">int</span> length = 2;
<span class="control">if</span> (<span class="variable">array</span> <span class="reserved">is</span> [length] { 1, _ })
{
}
</code></pre>

結果的に、`[]` だけにすることに。

<pre class="source" title="[] でリスト パターンを表現">
<code><span class="reserved">var</span> <span class="variable">array</span> = <span class="reserved">new</span>[] { 1, 2 };

<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> []); <span class="comment">// 長さ0マッチ。false。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [_, _]); <span class="comment">// 長さ2マッチ。true。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [ .. ]); <span class="comment">// 任意長さマッチ。true。</span>

<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [ 1 ]); <span class="comment">// 長さ1マッチ。false。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [ 1, .. ]); <span class="comment">// 1で開始、任意長さ。true。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [ .., 2 ]); <span class="comment">// 2で終了、任意長さ。true。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [ 1, .., 2 ]); <span class="comment">// 1で開始、2で終了、任意長さ。true。</span>
</code></pre>

基本的には「長さピッタリ」にだけマッチします。
任意長さとマッチさせたい場合は `..` を挟むという仕様です。

## <a id="slice-pattern">..パターン</a>

ちなみに、 `..` の後ろには入れ子でパターンを書けます。
主に [var パターン](../../../../study/csharp/datatype/patterns.md#var)で「マッチ結果の一部分」を抜き出すのに使います。

<pre class="source" title="..var">
<code><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };

<span class="control">if</span> (<span class="variable">a</span> <span class="reserved">is</span> [1, ..<span class="reserved">var</span> <span class="variable">middle</span>, 5])
{
    <span class="type">Console</span>.WriteLine(<span class="variable">middle</span>.Length); <span class="comment">// 2, 3, 4 で長さ3</span>
}
</code></pre>

あんまり意味はないですが、`[..[]]` とかも書けます。

<pre class="source" title=".. の後ろに再度 []">
<code><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
<span class="type">Console</span>.WriteLine(<span class="variable">a</span> <span class="reserved">is</span> [1, ..[2, 3, 4], 5]); <span class="comment">// true</span>
</code></pre>

`[1, ..[2, 3, 4], 5]` と `[1, 2, 3, 4, 5]` が同じ意味になるので、
ある意味スプレッド演算([JavaScript とかにある配列を展開するやつ](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Spread_syntax))です。

## <a id="lowering">展開結果</a>

リスト パターンは、`Length` チェックと[インデックス・範囲処理](../../../../study/csharp/data/dataranges.md)を使ったようなコードに展開されます。

例えば先ほどの `a is [1, ..var middle, 5]` であれば、以下のようなコードと同じ結果になります。

<pre class="source" title="[1, ..var middle, 5] を展開">
<code><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };

<span class="control">if</span> (<span class="variable">a</span>.Length &gt;= 2 &amp;&amp; <span class="variable">a</span>[0] == 1)
{
    <span class="reserved">var</span> <span class="variable">middle</span> = <span class="variable">a</span>[1..^1];
    <span class="control">if</span> (<span class="variable">a</span>[^1] == 5)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">middle</span>.Length);
    }
}
</code></pre>

`^` と `..` もさらに展開すると以下のコードと同じ意味。

<pre class="source" title="^ と .. も展開">
<code><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };

<span class="reserved">var</span> <span class="variable">length</span> = <span class="variable">a</span>.Length;
<span class="control">if</span> (<span class="variable">length</span> &gt;= 2 &amp;&amp; <span class="variable">a</span>[0] == 1)
{
    <span class="reserved">var</span> <span class="variable">middle</span> = <span class="variable">a</span>.<span class="method">Slice</span>(1, <span class="variable">length</span> - 1 - 1);
    <span class="control">if</span> (<span class="variable">a</span>[<span class="variable">length</span> - 1] == 5)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">middle</span>.Length);
    }
}
</code></pre>

ちなみに、`Length` か `Count` プロパティとインデクサーを持っている型に対してリスト パターンを使えます。

## <a id="collection-literal">[] リテラル (C# 11 より後かも)</a>

`new[] {}` との対称性をあきらめてパターン側を `[]` にしたわけですが、
ここで逆の発想が出て来たみたいです。
配列・コレクションの初期化の方も `[]` リテラルでやる案。

<pre class="source" title="[] でコレクション初期化">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="reserved">int</span>[] <span class="variable">array</span> = [ 1, 2 ];
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = [ 1, 2 ];
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> = [ 1, 2 ];
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> = [ 1, 2 ];
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> = [1, 2];
</code></pre>

これの話はまた回を改めて書くと思いますが、`ImmutableArray` の初期化も視野に入れています。 (`ImmutableArray` は今の `new() { 1, 2 }` だと望まれる動作にならない。)
