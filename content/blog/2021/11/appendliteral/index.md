---
title: "【C# 10.0】 AppendLiteral(\" \")"
source_url: "https://ufcpp.net/blog/2021/11/appendliteral/"
content_type: "BlogEntry"
published_at: "2021-11-20T21:32:05"
updated_at: "2021-11-20T21:32:05"
tags: []
umbraco_id: 2372
parent_id: 2363
sort_order: 4
aliases: []
---

# 【C# 10.0】 AppendLiteral(" ")

C# 10.0 で、[文字列補間に対するパフォーマンス改善](../../../../study/csharp/cheatsheet/ap_ver10.md#improved-string-interpolation)が入りました。
例えば、以下のようなコードがあったとして、

<pre class="source" title="文字列補間の例">
<code><span class="reserved">static</span> <span class="reserved">string</span> <span class="method">A</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; <span class="string">$&quot;</span><span class="string">(</span>{<span class="variable">x</span>}<span class="string">, </span>{<span class="variable">y</span>}<span class="string">)</span><span class="string">&quot;</span>;
<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">B</span>(<span class="reserved">int</span> <span class="variable">a</span>, <span class="reserved">int</span> <span class="variable">b</span>, <span class="reserved">int</span> <span class="variable">c</span>) =&gt; <span class="string">$&quot;</span>{<span class="variable">a</span>}<span class="string">.</span>{<span class="variable">b</span>}<span class="string">.</span>{<span class="variable">c</span>}<span class="string">&quot;</span>;
</code></pre>

C# 10.0 では `$""` の部分がそれぞれ以下のように展開されます。

<pre class="source" title="文字列補間の C# 10.0 での展開結果">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">A</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>)
{
    <span class="type">DefaultInterpolatedStringHandler</span> <span class="variable">h</span> = <span class="reserved">new</span>(4, 2);
    <span class="variable">h</span>.<span class="method">AppendLiteral</span>(<span class="string">&quot;(&quot;</span>);
    <span class="variable">h</span>.<span class="method">AppendFormatted</span>(<span class="variable">x</span>);
    <span class="variable">h</span>.<span class="method">AppendLiteral</span>(<span class="string">&quot;, &quot;</span>);
    <span class="variable">h</span>.<span class="method">AppendFormatted</span>(<span class="variable">y</span>);
    <span class="variable">h</span>.<span class="method">AppendLiteral</span>(<span class="string">&quot;)&quot;</span>);
    <span class="control">return</span> <span class="variable">h</span>.<span class="method">ToStringAndClear</span>();
}

<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">B</span>(<span class="reserved">int</span> <span class="variable">a</span>, <span class="reserved">int</span> <span class="variable">b</span>, <span class="reserved">int</span> <span class="variable">c</span>)
{
    <span class="type">DefaultInterpolatedStringHandler</span> <span class="variable">h</span> = <span class="reserved">new</span>(4, 2);
    <span class="variable">h</span>.<span class="method">AppendFormatted</span>(<span class="variable">a</span>);
    <span class="variable">h</span>.<span class="method">AppendLiteral</span>(<span class="string">&quot;.&quot;</span>);
    <span class="variable">h</span>.<span class="method">AppendFormatted</span>(<span class="variable">b</span>);
    <span class="variable">h</span>.<span class="method">AppendLiteral</span>(<span class="string">&quot;.&quot;</span>);
    <span class="variable">h</span>.<span class="method">AppendFormatted</span>(<span class="variable">c</span>);
    <span class="control">return</span> <span class="variable">h</span>.<span class="method">ToStringAndClear</span>();
}
</code></pre>

今日の話はこの `AppendLiteral` のところの最適化の話。

## インライン展開

上記の展開結果を最初に見た時の感想は「`AppendLiteral(char)` はなくていいの？」でした。
C# 的に、文字 (`'.'`) は単なる数値(2バイトの値型)なのに対して、文字列(`"."`) は参照型(ヒープ アロケーションが掛かる)なので、効率が悪いんじゃないかと。

実際、例えば類似のメソッドとして、`StringBuilder.Append` なんかは「文字列じゃなくて文字のオーバーロードを使え」というコード解析が出てきたりします。

![文字列じゃなくて文字のオーバーロードを使え](../../../../../assets/media/1194/ca1834.png)

何も対処しないと確かに問題になるっぽいんですが、これに対して、`DefaultInterpolatedStringHandler.AppendLiteral` の実装を工夫して、効率を落とさないようにしているそうです。

今現在(2021/11/7)の `DefaultInterpolatedStringHandler.AppendLiteral` の中身は以下のような感じ。

[DefaultInterpolatedStringHandler.cs#L136](https://github.com/dotnet/runtime/blob/f54ab52d24ee524a246e463d754e526832850d4a/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/DefaultInterpolatedStringHandler.cs#L136)

まんまコメントが書かれていますが、

> AppendLiteral is expected to always be called by compiler-generated code with a literal string.
> By inlining it, the method body is exposed to the constant length of that literal, allowing the JIT to
> prune away the irrelevant cases.  This effectively enables multiple implementations of AppendLiteral,
> special-cased on and optimized for the literal's length.  We special-case lengths 1 and 2 because
> they're very common, e.g.
>
>     1: ' ', '.', '-', '\t', etc.
>     2: ", ", "0x", "=>", ": ", etc.
>
> but we refrain from adding more because, in the rare case where AppendLiteral is called with a non-literal,
> there is a lot of code here to be inlined.

文字列長が1文字と2文字のときの特殊分岐を書いた上で、
`AggressiveInlining` を付けています。
要点だけを抜き出すと以下のようなコード。

<pre class="source" title="AppendLiteral 中の特殊分岐">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;

[<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.AggressiveInlining)]
<span class="reserved">void</span> <span class="method">AppendLiteral</span>(<span class="reserved">string</span> <span class="variable">value</span>)
{
    <span class="control">if</span> (<span class="variable">value</span>.Length == 1)
    {
        <span class="comment">// value[0] しか参照しないコード</span>
        <span class="control">return</span>;
    }

    <span class="control">if</span> (<span class="variable">value</span>.Length == 2)
    {
        <span class="comment">// value[0], value[1] しか参照しないコード</span>
        <span class="control">return</span>;
    }

    <span class="comment">// 汎用ロジック</span>
    <span class="method">AppendStringDirect</span>(<span class="variable">value</span>);
}
</code></pre>

`AppendLiteral` には文字通りリテラルしか渡ってこないという前提ありきですが、
これで1文字の場合と2文字の場合はかなり速くなるとのこと。

JIT 時最適化で、
1文字の文字列リテラルが渡ってきたときには `if (value.Length) == 1)`、
2文字のが渡ってきたときには `if (value.Length) == 2)` の中身しか残らないそうです。
