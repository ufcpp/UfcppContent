---
title: "ファーストクラスな Span 型"
source_url: "https://ufcpp.net/blog/2024/2/first-class-span/"
content_type: "BlogEntry"
published_at: "2024-02-13T23:12:43"
updated_at: "2024-02-15T10:16:24"
tags: []
umbraco_id: 2485
parent_id: 2480
sort_order: 4
aliases: []
---

# ファーストクラスな Span 型

今日は「`Span<T>`、`ReadOnlySpan<T>` をコンパイラーで特別扱いしたい」という話。

C# 7.2 の頃、[`Span<T>` 型](../../../../study/csharp/resource/span.md)が追加されて、
安全性を損なわずに unsafe コード並みにパフォーマンスのよいコードが書けるようになりました。
それ以来、.NET の標準ライブラリでもいろんな場面で`Span<T>` 型が活用されています。

いまや結構重要なポジションを担う型なわけですが、
現状の扱いはあくまで「普通の構造体の1つ」です。
そのため微妙にオーバーロード解決とかで困り気味。

例えば直近では、C# 12 でコレクション式を導入するにあたって「[普通にやってたら使い勝手が悪いので `Span` を特別扱い](../../../../study/csharp/datatype/collection-expression.md#priority)」みたいなことをやっています。

<pre class="source" title="コレクション式の Span 特別対応">
<span class="comment">// 普通にやると IEnumerable と Span の優先度はつかなくてコンパイルエラー。</span>
<span class="type">EnumerableVsSpan</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121"><span class="static">M</span></span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">5</span>]);

<span class="comment">// コレクション式は Span を優先する。</span>
<span class="type">EnumerableVsSpan</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);

<span class="comment">// Span を優先しちゃう(パフォーマンス的に好ましくない)。</span>
<span class="type">SpanVsReadOnlySpan</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">5</span>]);

<span class="comment">// ReadOnlySpan を優先するよう特別扱い。</span>
<span class="type">SpanVsReadOnlySpan</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);

<span class="reserved">class</span> <span class="type">EnumerableVsSpan</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}

<span class="reserved">class</span> <span class="type">SpanVsReadOnlySpan</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

また、いわゆる共変性の辺りが微妙だったりします。
以下のように、コンパイルできてほしくないのに実行時エラーになるのが1件、
コンパイルできてほしいのにできないのが1件。

<pre class="source" title="ReadOnlySpan の共変性">
<span class="reserved">var</span> <span class="variable">strArray</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">string</span>[<span class="number">5</span>];

<span class="comment">// 行ける。 Span に implicit operator が定義されているので。</span>
<span class="type struct">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">strSpan</span> <span class="operator">=</span> <span class="variable">strArray</span>;

<span class="comment">// なぜか行ける…</span>
<span class="comment">// 配列に共変性(object[] objArray = strArray; が合法という負の遺産)があるせい。</span>
<span class="comment">// が、実行時例外起こす。</span>
<span class="type struct">Span</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">objSpan</span> <span class="operator">=</span> <span class="variable">strArray</span>;

<span class="comment">// 行ける。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">strRos1</span> <span class="operator">=</span> <span class="variable">strArray</span>;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">strRos2</span> <span class="operator">=</span> <span class="variable">strSpan</span>;

<span class="comment">// これも行ける。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">objRos1</span> <span class="operator">=</span> <span class="variable">objSpan</span>;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">objRos2</span> <span class="operator">=</span> <span class="variable">strArray</span>;

<span class="comment">// ダメ…</span>
<span class="comment">// (できても問題ないけど、ReadOnlySpan を特別扱いしないとコンパイラーにはそれがわからない。)</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">objRos3</span> <span class="operator">=</span> <span class="variable"><span class="error" title="CS0029">strSpan</span></span>;
</pre>

ということで、まあ、
「`Span<T>`、`ReadOnlySpan<T>` をコンパイラーで特別扱いしたい」という話になります。

* ドキュメント追加の PR: [Add first-class span types proposal](https://github.com/dotnet/csharplang/pull/7904)
* トラッキング issue: [[Proposal]: First-Class Span Types](https://github.com/dotnet/csharplang/issues/7905)

「配列から `IEnumerable<T>` への変換」とかが元からそうなんで、その辺りに並べて `Span<T>`、`ReadOnlySpan<T>` がらみの仕様を入れるとのこと。

提案は[2月5日の Language Design Meeting であっさり了承されてる](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-05.md)し、
割かしコレクション式の取り組みからの流れっぽい感じもするので C# 13 に入りそうな感じがしますね。
懸念として、ちょっとした(めったに起こらなさそうな)破壊的変更があり得るので、
そのリスクがどう評価されるか次第。
