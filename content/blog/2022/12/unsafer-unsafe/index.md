---
title: "Unsafe クラスの敗北 (関数ポインター)"
source_url: "https://ufcpp.net/blog/2022/12/unsafer-unsafe/"
content_type: "BlogEntry"
published_at: "2022-12-18T22:11:56"
updated_at: "2022-12-18T22:11:56"
tags: []
umbraco_id: 2445
parent_id: 2438
sort_order: 4
aliases: []
---

# Unsafe クラスの敗北 (関数ポインター)

[Gist](https://gist.github.com/ufcpp) に書き捨ててたコードの供養ブログ シリーズ、
今日のは特に人を選ぶやつ。

今日は C# 9 で入った [関数ポインター](../../../../study/csharp/cheatsheet/ap_ver9.md#function-pointer) がらみの話です。

## <a id="unsafe-class">Unsafe クラス</a>

C# の unsafe 機能、例えばポインターとかは、なかなか制限がきついです。
そのため、「実は .NET の型システム的にはできる」というものでも、
C# で書くことはできないことが結構あります。

それに対して、
.NET Core 以降、
[`Unsafe`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.unsafe?view=net-7.0) (`System.Runtime.CompilerServices` 名前空間)とかいう名前からして unsafe なクラスがあって、
内部的に IL を使ったり、
runtime intrinsics (JIT コンパイラーの特別扱い)で実装したりして、
元々 C# では書けなかったようなコードを、普通の C# で書けるようにしました。

この `Unsafe` クラスは、
[unsafe コンテキスト](../../../../study/csharp/interop/sp_unsafe.md)なしで、
普通の unsafe コードよりもよっぽど unsafe なことができちゃうという意味で良くも悪くも凶悪です。

ということで、皆様ご存じの通り<sup>[※](#as-you-all-known)</sup>、
`Unsafe` クラスを使えば C# でも C++ 的な遊びがいろいろと楽しめます。

<pre class="source" title="Unsafe.As">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>(<span class="number">123</span>);

<span class="comment">// readonly struct なので、↓はエラー。</span>
<span class="comment">//a.Value = 999;</span>

<span class="comment">// Unsafe.As を使えば、</span>
<span class="comment">// C++ でいう reinterpret_cast 的に何でもかんでも変換可能。</span>
<span class="comment">// (メモリレイアウトが想定通りかは利用者の自己責任。)</span>
<span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="static"><span class="type">Unsafe</span></span><span class="operator">.</span><span class="method"><span class="static">As</span></span>&lt;<span class="type struct">A</span>, <span class="reserved">int</span>&gt;(<span class="reserved">ref</span> <span class="variable">a</span>);
<span class="variable">x</span> <span class="operator">=</span> <span class="number">999</span>;

<span class="comment">// a.Value が 999 に書き変わってる。</span>
<span class="comment">// A { Value = 999 }</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>);

<span class="reserved">readonly</span> <span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">A</span>(<span class="reserved">int</span> <span class="variable local">Value</span>);
</pre>

<sup><a id="as-you-all-known">※</a></sup>どの方面に向かって「皆」と言っているのかは不明。

`Unsafe` クラスを使ったこの手の処理、
誤用すると盛大にクラッシュさせれるくらい安全性皆無になるので利用には注意が必要ですが、
パフォーマンス改善につながることが多くて、
一部界隈では結構多用されます。

## <a id="ref-struct">ref struct の制限</a>

ところが、`Unsafe` クラスでもできないことがありまして。
というか、`Unsafe` クラスはおろか、現状では[ポインター](../../../../study/csharp/interop/sp_unsafe.md#pointer)を使っても解決できないものがありまして。

というのも、[ref struct](../../../../study/csharp/resource/refstruct.md)はジェネリック型引数にもできないし、ポインターにもできません。

なので、先ほどと同じノリで ref struct に対して `Unsafe.As` (とか、それ相当の unsafe コード)を書こうとしてもうまくいきません。

<pre class="source" title="Span には Unsafe.As が使えない">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">span</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="reserved">int</span>[] { <span class="number">0xDE</span>, <span class="number">0xAD</span>, <span class="number">0xBE</span>, <span class="number">0xEF</span> });
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>(<span class="variable">span</span>);

<span class="reserved">var</span> <span class="variable">spanFromA</span> <span class="operator">=</span> <span class="type"><span class="static">Unsafe</span></span><span class="operator">.</span><span class="error" title="CS0306"><span class="error" title="CS0306"><span class="static"><span class="method">As</span></span>&lt;<span class="type struct">A</span>, <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;&gt;</span></span>(<span class="reserved">ref</span> <span class="variable">a</span>);

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// private なので通常、この _span を取り出す方法ない。</span>
    <span class="comment">// なんならリフレクションを使っても無理。</span>
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">_span</span>;
    <span class="reserved">public</span> <span class="type struct">A</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>) <span class="operator">=&gt;</span> <span class="field">_span</span> <span class="operator">=</span> <span class="variable local">span</span>; 
}
</pre>

## <a id="function-pointer">関数ポインター</a>

そんな時には[関数ポインター](../../../../study/csharp/cheatsheet/ap_ver9.md#function-pointer)を使えばいいらしいですよ。

C# 9 で入った関数ポインター、
`delegate*<T1, T2, ...>` みたいな、ジェネリクスに似た記法を使う割に、
この `T1` とか `T2` のところには `ref` も書けるし ref struct も書けるしで、相当自由みたいです。

要するに、`Span<T>` (ref struct)に対して、
`Span<T>*` (直接その型のポインター)は書けないし、
`Unsafe.As<A, Span<T>>` (型引数)も書けませんが、
`delegate*<ref A, ref Span<T>>` (関数ポインターの引数)なら書けます。

これを使えば、以下のように、ref struct に対しても `Unsafe.As` 的なことができるようになったりします。

<pre class="source" title="関数ポインターは制限がゆるくて、現状これでしかできないことができちゃう">
<span class="reserved">var</span> <span class="variable">span</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="reserved">int</span>[] { <span class="number">0xDE</span>, <span class="number">0xAD</span>, <span class="number">0xBE</span>, <span class="number">0xEF</span> });
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>(<span class="variable">span</span>);

<span class="reserved">unsafe</span>
{
    <span class="comment">// function pointer の引数なら ref RefStruct も行ける。</span>
    <span class="reserved">var</span> <span class="variable">f</span> <span class="operator">=</span> (<span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">ref</span> <span class="type struct">A</span>, <span class="reserved">ref</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;&gt;)(<span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">nint</span>, <span class="reserved">nint</span>&gt;)<span class="operator">&amp;</span><span class="method"><span class="static">id</span></span>;

    <span class="comment">// 晴れて A の中から _span を抜き出し。</span>
    <span class="reserved">var</span> <span class="variable">spanFromA</span> <span class="operator">=</span> <span class="variable">f</span>(<span class="reserved">ref</span> <span class="variable">a</span>);

    <span class="comment">// span と同じ内容。</span>
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">spanFromA</span>) <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">x</span>:<span class="string">X2</span>}<span class="string">&quot;</span>);
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>();

    <span class="comment">// span が書き変わる。</span>
    <span class="variable">spanFromA</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="number">1</span>;
    <span class="variable">spanFromA</span>[<span class="number">1</span>] <span class="operator">=</span> <span class="number">2</span>;
    <span class="variable">spanFromA</span>[<span class="number">2</span>] <span class="operator">=</span> <span class="number">3</span>;
    <span class="variable">spanFromA</span>[<span class="number">3</span>] <span class="operator">=</span> <span class="number">4</span>;
}

<span class="comment">// 上書きされた 01020304。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">span</span>) <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">x</span>:<span class="string">X2</span>}<span class="string">&quot;</span>);
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>();

<span class="comment">// nint 素通しメソッド。</span>
<span class="comment">// nint = unsafe コンテキスト内なら任意のポインター、任意の ref T を通せる。</span>
<span class="reserved">static</span> <span class="reserved">nint</span> <span class="method"><span class="static">id</span></span>(<span class="reserved">nint</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span>;

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// private なので通常、この _span を取り出す方法ない。</span>
    <span class="comment">// なんならリフレクションを使っても無理。</span>
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">_span</span>;
    <span class="reserved">public</span> <span class="type struct">A</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>) <span class="operator">=&gt;</span> <span class="field">_span</span> <span class="operator">=</span> <span class="variable local">span</span>;
}
</pre>

native interop でしか使い道がないと思っていた関数ポインター、
こんなところで「これでしかできないこと」があるとは…
