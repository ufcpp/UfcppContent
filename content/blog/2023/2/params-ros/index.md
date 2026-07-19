---
title: "【C# 12 候補】params Span、改め、params ReadOnlySpan"
source_url: "https://ufcpp.net/blog/2023/2/params-ros/"
content_type: "BlogEntry"
published_at: "2023-02-12T15:41:58"
updated_at: "2023-02-12T15:41:58"
tags: []
umbraco_id: 2456
parent_id: 2455
sort_order: 0
aliases: []
---

# 【C# 12 候補】params Span、改め、params ReadOnlySpan

今回は [params](../../../../study/csharp/structured/sp_params.md#params) の話。

* Working Group 議事録
    * [2022/10/25](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/params-improvements/PI-2022-10-25.md)
    * [2022/11/3](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/params-improvements/PI-2022-11-03.md)

params の改善話は紆余曲折ありまして。
[去年の時点では `params Span<T>` で検討されていました](../../../2022/2/params-span/index.md)。
ちょこっとだけマイナーチェンジされまして、現在は `params ReadOnlySpan<T>` です。

## いろんな型で params 案(没)

現在の C# の params (可変長引数)は、`params T[]` (引数の型は配列)しか書けません。
これに対して、任意のコレクション型を使って、`params List<T>` とか `params IEnumerable<T>` とか書きたいという要望が長らくありました。

<pre class="source" title="過去の params 改善案">
<span class="comment">// (あくまでも過去の案)</span>
<span class="method"><span class="static">M1</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
<span class="method"><span class="static">M2</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
<span class="static"><span class="method">M3</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M1</span></span>(<span class="reserved">params</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">items</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M2</span></span>(<span class="reserved">params</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">items</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M3</span></span>(<span class="reserved">params</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">items</span>) { }
</pre>

「この類の何かを書きたい」という要望は今でもあるんですが、
ただ、ここにきて[コレクション リテラル](../../1/collection-literal/index.md)という提案が出ています。
コレクション リテラルがあれば、別に params がなくても以下のように書くことができます。

<pre class="source" title="コレクション リテラルがあれば別にいいのでは…">
<span class="comment">// 呼び出し側をコレクション リテラルにしてしまう。</span>
<span class="comment">// 元の params 案との差は [] の2文字だけ。</span>
<span class="method"><span class="static">M1</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);
<span class="method"><span class="static">M2</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);
<span class="static"><span class="method">M3</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);

<span class="comment">// params で任意のコレクションを扱うのはやめちゃう。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M1</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">items</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M2</span></span>(<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">items</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M3</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">items</span>) { }
</pre>

`[]` の2文字程度ならさぼらず書いてもいいんじゃないかという感じがします。
なので、「params の汎用化」という目的においてはもう別にやらなくてもいいんじゃないかという雰囲気になっています。

## params ReadOnlySpan

「params の汎用化」が没り気味な一方で、
「既存の `params T[]` 利用個所のパフォーマンスを改善したい」という要件は残っています。
そこで出てくるのが `params ReadOnlySpan<T>` になります。

すなわち、

* params に使えそうな中で一番パフォーマンス的に有利な `ReadOnlySpan` だけを残す
* 既存の `params T[]` よりも、`params ReadOnlySpan<T>` の方がオーバーロード解決優先順位を上にする
    * 既存のメソッドに `params ReadOnlySpan<T>` なオーバーロードを足せば、利用側は再コンパイルするだけでパフォーマンス改善になる

という方針で進めるようです。

ちなみに、`params ReadOnlySpan<T>` で定義した引数は常に [scoped](../../../../study/csharp/resource/refstruct.md#scoped) みたいです。
ReadOnly で受け取っているので書き換えできず、scoped なのでメソッドの外には漏らせません。
その結果、呼び出し側で `M(a, b, c)` みたいな書き方から「`a`, `b`, `c` を含む `ReadOnlySpan`」を作るときの最適化がしやすくなっています
(DLL のデータ領域を直接参照したり、複数回呼び出されるときに同じバッファーを使いまわしたり)。

## 固定長バッファー

以下のようなコードを書いたとき、

<pre class="source" title="params ReadOnlySpan">
<span class="method"><span class="static">M</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
<span class="method"><span class="static">M</span></span>(<span class="string">"a"</span>, <span class="string">"b"</span>, <span class="string">"c"</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type">T</span>&gt;(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type">T</span>&gt; <span class="variable local">items</span>) { }
</pre>

概念的には、以下のように「スタック割り当て」をしたいです。

<pre class="source" title="">
<span class="comment">// int の場合はこれで問題ない。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">temp1</span> <span class="operator">=</span> <span class="reserved">stackalloc</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
<span class="method"><span class="static">M</span></span>(<span class="variable">temp1</span>);

<span class="comment">// 現状、参照型の stackalloc はできないので、何らかの対処が必要。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">temp2</span> <span class="operator">=</span> <span class="error" title="CS0208"><span class="reserved">stackalloc</span>[] { <span class="string">&quot;a&quot;</span>, <span class="string">&quot;b&quot;</span>, <span class="string">&quot;c&quot;</span> }</span>;
<span class="static"><span class="method">M</span></span>(<span class="variable">temp2</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type">T</span>&gt;(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type">T</span>&gt; <span class="variable local">items</span>) { }
</pre>

そこで、参照型にも使えるスタック割り当て手段を必要とするわけですが。
[去年の時点で](../../../2022/2/params-span/index.md)、
「[Experiment with 'Unsafe.StackAlloc<T>'](https://github.com/dotnet/runtime/pull/60428)」とか「[[hackathon] ValueArray](https://github.com/dotnet/runtime/pull/60519)」みたいなプロトタイプもあったんですが、あんまり筋はよくなかったようで没っています。

そして現状、「特殊な属性を1個用意して、それをつけると .NET ランタイムが特殊対応して固定長バッファーを生成する」みたいな案で進んでいるようです。

* [[API Proposal]: InlineArrayAttribute #61135](https://github.com/dotnet/runtime/issues/61135)

ちなみに、`params ReadOnlySpan<T>` と `ReadOnlySpan<T>` に対する[コレクション リテラル](../../1/collection-literal/index.md)は同じ戦略をとるそうで
(やることは同じなので2重実装は避ける)、
「params の改善」と「コレクション リテラル」は2つ合わせて同時に進めるということになりました。
