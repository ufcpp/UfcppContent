---
title: "First-class な Span 型"
source_url: "https://ufcpp.net/blog/2025/1/first-class-span/"
content_type: "BlogEntry"
published_at: "2025-01-03T21:01:48"
updated_at: "2025-01-03T21:01:48"
tags: []
umbraco_id: 2509
parent_id: 2506
sort_order: 2
aliases: []
---

# First-class な Span 型

「Rosly の [Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) に並んでいるもののうち、すでに preview 提供済みのものシリーズ第2段。

* field キーワード
* First-class Span ← 今日はこれ
* nameof(T<>)

すでに今、[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) に `preview` を指定すれば利用可能です。

今日は First-class Span。
([これも昔1回取り上げてるんですが](../../../2024/2/first-class-span/index.md)、案外書くことあり。)

### <a id="first-class-span">First-class Span</a>

C# 7.2 の頃に `Span<T>` や `ReadOnlySpan<T>` が導入されて以来、
これらの型を使った高パフォーマンスな API がたくさん提供されています。
また、[C# 12 で入ったコレクション式](../../../../study/csharp/datatype/collection-expression.md#priority)や、
[C# 13 で入った `params` コレクション](../../../../study/csharp/structured/sp_params.md#params-collections)では、
`T[]` や `IEnumerable<T>` よりも `Span<T>` や `ReadOnlySpan<T>` を優先的に選ぶように特別な処理が入っています。
この例からもわかるように、今や `Span<T>` や `ReadOnlySpan<T>` が重要な地位を占めています。

ところが、コレクション式などの一部の文脈を除いて、
`Span<T>` や `ReadOnlySpan<T>` は「ただの構造体」で、
配列からの型変換も「`Span<T>` や `ReadOnlySpan<T>` 構造体に定義されたユーザー定義型変換」です。
C# 言語組み込みの型変換と比べて、ユーザー定義型変換は1段下扱いで、色々な不便があります。

そこで、[`Span<T>` や `ReadOnlySpan<T>` を言語組み込み(= first-class、一級市民)にしたい](https://github.com/dotnet/csharplang/blob/main/proposals/first-class-span-types.md)という提案があって、
これもすでに実装があり、
Visual Studio 17.13.0 Preview 1 (.NET 9 の正式リリースと同時)で merge 済みです。

わかりやすいのは拡張メソッドの呼び出しで、
ユーザー定義型変換を挟む拡張メソッド呼び出しはできません。
例えば、以下のコードは C# 13 でコンパイル エラーだったものが、preview ではコンパイルできるようになっています。

<pre class="source" title="配列に対して Span や ReadOnlySpan の拡張メソッドは呼べない">
<span class="comment">// 拡張メソッドの呼び出しはユーザー定義の型変換を見ない。</span>
<span class="comment">// Span の特別扱いがないと拡張メソッドは呼べない。</span>
<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">1</span>]<span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

また、
「`Span<T>` や `ReadOnlySpan<T>` 引数を使った方がパフォーマンスがいいのでこちらを呼んでほしい」という要望があるんですが、
これまでは `IEnumerable<T>` なオーバーロードがあるとそっちが優先されるという問題もありました。

<pre class="source" title="IEnumerable よりも Span/ReadOnlySpan を優先する特別扱い">
<span class="comment">// ユーザー定義の型変換よりも、「派生・実装クラスだから変換可能」の方が優先度が高い。</span>
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">1</span>]);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { } <span class="comment">// C# 13 ではこっち。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { } <span class="comment">// preview ではこっち。Span の特別扱いがないとこっちは呼んでもらえない。</span>
}
</pre>

また、ユーザー定義の型変換では「型引数の[共変性](../../../../study/csharp/oop/sp4_variance.md#variance)」を表現できないという問題があります。
`ReadOnlySpan<string>` を `ReadOnlySpan<object>` に代入できてもいいはずなのに、
これが C# 13 まではできませんでしたが、preview にすると受け付けます。

<pre class="source" title="ReadOnlySpan の共変性">
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">s</span> <span class="operator">=</span> [];
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="variable">s</span>; <span class="comment">// C# 13 ではエラー。</span>
</pre>

ちなみに、`Span<T>` と `ReadOnlySpan<T>` の両方のオーバーロードがある場合、
`ReadOnlySpan<T>` の方が優先されます。

<pre class="source" title="ReadOnlySpan 優先">
<span class="reserved">string</span>[] <span class="variable">s</span> <span class="operator">=</span> [];

<span class="comment">// ReadOnlySpan の方が優先。</span>
<span class="variable">s</span><span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

target-typed で生成される型自体が変わるコレクション式と違って、
一度配列を作っちゃってるので `ReadOnlySpan<T>` を優先しても別にパフォーマンス的なメリットは少ないんですけども。
じゃあどうしてこういう仕様にしたかというと…
こうしておかないとまた「[配列の共変性の地雷](../../../2022/11/covariantarrayincident/index.md)を踏むから」とのこと。

<pre class="source" title="配列の共変性は結構な地雷">
<span class="reserved">string</span>[] <span class="variable">s</span> <span class="operator">=</span> [];
<span class="reserved">object</span>[] <span class="variable">o</span> <span class="operator">=</span> <span class="variable">s</span>; <span class="comment">// C# の配列は共変。</span>

<span class="comment">// Span を優先するとこれが例外を起こしちゃう。</span>
<span class="comment">// ReadOnlySpan&lt;object&gt; x = s; は合法。</span>
<span class="comment">// Span&lt;object&gt; x = s; は実行時例外。</span>
<span class="variable">o</span><span class="operator">.</span><span class="method">M</span>(); <span class="comment">// ReadOnlySpan&lt;object&gt; を優先しないとここで例外が出る。</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="reserved">object</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">object</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

以上、とりあえず、C# 14 (現状 `LangVersion` preview)では `Span<T>`、`ReadOnlySpan<T>` が特別扱いされて、オーバーロードの解決順位が変わります。
おおむね便利な方向に変わるはずですが、もしかすると何らかの問題を起こす可能性もあります。
もしも「`Span<T>` オーバーロードを呼ばれるとまずい」みたいなことがあれば、[`OverloadResolutionPriority`](../../../../study/csharp/cheatsheet/ap_ver13.md#overload-resolution-priority)とかでの対処を考えてみてください。
