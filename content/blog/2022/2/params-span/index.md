---
title: "【C# 11 候補】params Span"
source_url: "https://ufcpp.net/blog/2022/2/params-span/"
content_type: "BlogEntry"
published_at: "2022-02-04T23:07:02"
updated_at: "2022-02-06T18:16:53"
tags: []
umbraco_id: 2413
parent_id: 2411
sort_order: 1
aliases: []
---

# 【C# 11 候補】params Span

今日は「low level hackathon」話2個目。

* 1個目: [【C# 11 候補】 ReadOnlySpan 最適化](../span-optimization/index.md)

## 可変長引数

C# の可変長引数は、一時的にデータを詰めておく配列を作ってメソッドに渡す作りになっています。
例えば、`void m(params int[] args)` というメソッドがあったとして、
`m(1, 2, 3);` みたいに呼び出した場合、
`m(new int[] { 1, 2, 3 });` みたいに展開されます。

ここで問題になるのが `new int[]` でヒープ アロケーションが発生する点。
あまりにも数が重なると無視できないコストになってきます。

## params Span

C# 7.2 の頃に [`Span<T>` 構造体](../../../../study/csharp/resource/span.md)が入ったことで、
当然 `params T[]` の `new T[]` によるアロケーションも避けたいという話が出てきます。

つまるところ、

* メソッド定義
  * 今あるもの: `void m(params T[] args)`
  * 欲しいもの: `void m(params Span<T> args)`
* 呼び出し側の展開結果:
  * 今あるもの: `m(new int[] { 1, 2, 3 });` とか
  * 欲しいもの: `m(stackalloc int[] { 1, 2, 3 });` とか

みたいなものが欲しいと。

実際、案自体は結構昔からあります:

[`params Span<T>`](https://github.com/dotnet/csharplang/blob/main/proposals/params-span.md)

## 参照型 stackalloc (没気味)

ただ、`stackalloc` の制限が結構きついので、素直に上記のような展開はできません。

わかりやすい原因は、参照型に対して `stackalloc` を使えない点。
以下のようなコードはコンパイル エラーになります。

<pre class="source" title="参照型の stackalloc は禁止">
<code><span class="type">Span</span>&lt;<span class="reserved"><span class="error">string</span></span>&gt; <span class="variable">span</span> = <span class="reserved">stackalloc</span> <span class="reserved">string</span>[4];
</code></pre>

これは元々ある .NET ランタイムの制限です。

参照型に対する `stackalloc` を下手に認めてしまうとガベージコレクションの参照トラッキングの負担が上がって、GC 発生時のコストまで見た時トータルではかえって遅くなる可能性が高いとのこと。

この制限に対して、low level hachathon で1回、任意の型に対する `stackalloc` をやってみる実験をしたみたいです。

[Experiment with `Unsafe.StackAlloc<T>`](https://github.com/dotnet/runtime/pull/60428)

pull request がそっ閉じされてるんで、
やっぱり上記のような `stackalloc` の問題が許容されなかったんですかね。

## ValueArray

他に、`params Span<T>` に使いたいのであれば固定長配列の類でもいいわけでして。
例えば以下のようなコードで「長さ4固定の配列もどき」を作ることはできます。

<pre class="source" title="長さ固定の配列もどき">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

<span class="type">ValueArray4</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">buffer</span> = <span class="reserved">default</span>;
<span class="type">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">span</span> = <span class="type">MemoryMarshal</span>.<span class="method">CreateSpan</span>(<span class="reserved">ref</span> <span class="variable">buffer</span>.X0, 4);

<span class="reserved">struct</span> <span class="type">ValueArray4</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> X0, X1, X2, X3;
}
</code></pre>

とはいえ、こんなコードを都度手書きはしたくないわけでして。
あと、できれば `ValueArray<string, 4>` みたいな感じで何らかの手段で「長さ」の情報はジェネリクス的に渡したかったりはします。

それに類するものをとりあえず実装してみたという pull request が low level hackathon で出てたりします。

[[hackathon] ValueArray](https://github.com/dotnet/runtime/pull/60519)

「試しにやってみた」実装なのでなかなかにキモイです…
現状の .NET は「ジェネリクスに型引数代わりに整数を渡す」みたいなことができないので、
1 の代わりに `object[]`、2 の代わりに `object[,]`、3 の代わりに `object[,,]`、… みたいな、`object` 配列の次元を整数代わりに使うというすごい実装。
本来であれば `ValueArray<string, 4>` と書きたいところを `ValueArray<string, object[,,,]>` と書くことになります…

<pre class="source" title="object[,,,] でジェネリック整数引数を代用…">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

<span class="type">ValueArray</span>&lt;<span class="reserved">string</span>, <span class="reserved">object</span>[,,,]&gt; <span class="variable">buffer</span> = <span class="reserved">default</span>;
<span class="type">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">span</span> = <span class="variable">buffer</span>.<span class="method">AsSpan</span>();
</code></pre>

これはさすがにあまりにもきもいので没気味。
代替案として、「いったん属性を付けて特殊処理しようか」みたいな話になっています。

[[API Proposal]: InlineArrayAttribute](https://github.com/dotnet/runtime/issues/61135)

こちらだと、いちいち構造体の定義が要るみたいです。

<pre class="source" title="InlineArray 属性">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

<span class="type">ValueArray4</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">buffer</span> = <span class="reserved">default</span>;
<span class="type">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">span</span> = <span class="variable">buffer</span>.<span class="method">AsSpan</span>();

<span class="comment">// この属性を付けた構造体は T 4つ分のメモリを確保する。</span>
[<span class="type">InlineArray</span>(Length = 4)]
<span class="reserved">struct</span> <span class="type">ValueArray4</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">private</span> <span class="type">T</span> _element0;
    <span class="reserved">public</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt; <span class="method">AsSpan</span>() =&gt; <span class="type">MemoryMarshal</span>.<span class="method">CreateSpan</span>(<span class="reserved">ref</span> _element0, 4);
}
</code></pre>

やっぱ、根本的にはジェネリクスに整数を渡せるようにしてほしいところですけどね…
それは結構型システムに手を入れないといけないみたいでちょっと大変みたいです。
