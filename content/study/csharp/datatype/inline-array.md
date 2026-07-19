---
title: "[雑記] InlineArray"
source_url: "https://ufcpp.net/study/csharp/datatype/inline-array/"
content_type: "Article"
published_at: "2023-09-20T00:00:00"
updated_at: "2025-02-15T15:49:32"
tags: []
umbraco_id: 2472
parent_id: 1940
sort_order: 7
aliases:
  - "/csharp/datatype/inline-array/"
---

# \[雑記\] InlineArray

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

<h5 class="version version12">Ver. 12</h5>

.NET 8 で、
[`InlineArray` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.inlinearrayattribute) (`System.Runtime.CompilerServices` 名前空間) というものが入りました。

基本的には .NET ランタイム側の機能ですが、
いくつか、C# 側にもこの `InlineArray` 向けの特殊対応が入っています。

ちなみに、この機能は現状、
[コレクション式](https://github.com/ufcpp/UfcppSample/issues/447)の内部実装にこそ使っていますが、
本稿で書いているようなコードを直接書く必要はほぼありません。
(実質、本稿はコレクション式の内部実装(の一部)の説明みたいなものです。)

## <a id="sec-generated-title-2"></a> <a id="inline-array-attribute">InlineArray 属性</a>

.NET 8 から、
以下のように、構造体に属性を付けると構造体のサイズが変わります。

<pre class="source" title="InlineArray 属性">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// この属性を付けると、 .NET ランタイムが特別扱いして、構造体のサイズを拡大する。</span>
<span class="comment">// (コンストラクター引数で Length 指定。)</span>
[<span class="type">InlineArray</span>(<span class="number">3</span>)]
<span class="reserved">struct</span> <span class="type struct">FixedBuffer</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// フィールドを1個だけ書く。</span>
    <span class="comment">// (2個以上書くとコンパイル エラーになる。)</span>
    <span class="comment">// 構造体のサイズが sizeof(T) × Length になる。</span>
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value</span>;
}
</pre>

inline array という名前通り、「埋め込み配列」として使います。
(長さ N の配列代わりに、長さ N 個分のサイズを持った構造体を作ります。
C# の配列はヒープに割り当てられるのに対して、この inline array であればスタック上に値を持てます。)

要は、以下のような「N 個のフィールドを並べる」みたいな構造体を、ランタイム側で自動的に作ってくれる機能です。

<pre class="source" title="N 個のフィールドを手書きで並べた例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="reserved">struct</span> <span class="type struct">FixedBuffer3</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// 所望の個数フィールドを書く。</span>
    <span class="comment">// (3要素くらいならいいけども、数十とか数百になるときつい。)</span>
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value0</span>;
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field"><span class="warning" title="CS0169">_value1</span></span>;
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field"><span class="warning" title="CS0169">_value2</span></span>;

    <span class="comment">// 変換とかも自前で書く。</span>
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="method">AsSpan</span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">MemoryMarshal</span></span><span class="operator">.</span><span class="static"><span class="method">CreateSpan</span></span>(<span class="reserved">ref</span> <span class="field">_value0</span>, <span class="number">3</span>);

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type param">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="method">AsSpan</span>()[<span class="variable local">index</span>];
}
</pre>

## <a id="sec-generated-title-3"></a> <a id="vs-stackalloc">stackalloc との違い</a>

これまでも [`stackalloc`](../interop/sp_unsafe.md#safe-stackalloc) という機能を使えば、
一応、スタック上に配列上のデータを置くことはできました。
ただ、`stackalloc` には結構強い制限があって使いづらいです。

一番きつい制限は、参照型、もしくは、参照を含む型に対して使えないことです
(これを認めようとすると[ガベコレ](../../computer/essential-software/memorymanagement.md#garbage-collection)の負担が上がって、パフォーマンス的にかえって不利になるそうです)。
例えば以下のコードでは、`string` 以下の型に対してコンパイル エラーになります。

<pre class="source" title="参照を含むときには stackalloc は使えない">
<span class="comment">// 構造体に対しては使える。</span>
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">i</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">100</span>];
<span class="type struct">Span</span>&lt;<span class="type struct">DateTimeOffset</span>&gt; <span class="variable">d</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="type struct">DateTimeOffset</span>[<span class="number">100</span>];

<span class="comment">// クラスに対しては使えない。</span>
<span class="comment">// (コンパイル エラーになる。)</span>
<span class="type struct">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved"><span class="error" title="CS0208">string</span></span>[<span class="number">100</span>];

<span class="comment">// クラスや参照を含む構造体に対しても使えない。</span>
<span class="comment">// (コンパイル エラーになる。)</span>
<span class="type struct">Span</span>&lt;<span class="type struct">ContainsRefType</span>&gt; <span class="variable">r1</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="type struct"><span class="error" title="CS0208">ContainsRefType</span></span>[<span class="number">100</span>];
<span class="type struct">Span</span>&lt;<span class="type struct"><span class="error" title="CS0306">ContainsRefField</span></span>&gt; <span class="variable">r2</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="error" title="CS0208"><span class="type struct">ContainsRefField</span></span>[<span class="number">100</span>];

<span class="reserved">struct</span> <span class="type struct">ContainsRefType</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning" title="CS0649"><span class="field">String</span></span>;
}

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">ContainsRefField</span>
{
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="warning" title="CS0649"><span class="field">Ref</span></span>;
}
</pre>

また、`stackalloc` で確保したスタック領域は、メソッドを抜けるまで解放されません。
このせいで、ループの内側で間違って `stackalloc` を使ってしまうと簡単にスタック オーバーフロー(要はメモリ不足)を引き起こします
(一般に、スタックはヒープよりもだいぶサイズが小さいです。Windows の場合は 1MB 程度)。
例えば以下のコードを Windows で実行するとスタック オーバーフローします
(1000 とか 200 とか、そこまで大きくない数字ですら簡単にスタック オーバーフローになります)。

<pre class="source" title="">
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="number">1000</span>; <span class="variable">i</span><span class="operator">++</span>)
{
    <span class="reserved">_</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">long</span>[<span class="number">200</span>];
}
</pre>

## <a id="sec-generated-title-4"></a> <a id="special-syntax">C# 側特殊対応</a>

一応、C# 側にもこの InlineArray に対する特殊対応が入っています。
(一応、C# 12 の新機能。)

まず、属性を付けた型に対するチェックが働いています。
すでに前述の例でも書いていますが、
`InlineArray` 属性を付けた型にフィールドが2つ以上あるとコンパイル エラーになります。

<pre class="source" title="InlineArray 属性を付けた型に対するチェック">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

[<span class="type">InlineArray</span>(<span class="number">3</span>)]
<span class="reserved">struct</span> <span class="type struct">FixedBuffer</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// フィールドを2個以上書くとコンパイル エラーになるのは一応「C# の新機能」。</span>
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value</span>;
}
</pre>

また、この型を使う側に、以下のような特殊対応が入っています。

* インデクサーを直接書ける
* `Span<T>`/`ReadOnlySpan<T>` に暗黙的に変換できる
* `foreach` で列挙できる

<pre class="source" title="InlineArray 型利用側の特殊対応">
<span class="type struct">FixedBuffer</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">buffer</span> <span class="operator">=</span> <span class="reserved">new</span>();

<span class="comment">// InlineArray に対して直接インデクサーを書ける。</span>
<span class="variable">buffer</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="string">&quot;zero&quot;</span>;
<span class="variable">buffer</span>[<span class="number">1</span>] <span class="operator">=</span> <span class="string">&quot;one&quot;</span>;

<span class="comment">// Span/ReadOnlySpan に暗黙的に変換できる。</span>
<span class="type struct">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="variable">buffer</span>;
<span class="variable">span</span>[<span class="number">2</span>] <span class="operator">=</span> <span class="string">&quot;two&quot;</span>;

<span class="comment">// foreach で列挙できる。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">buffer</span>)
{
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);
}
</pre>

## <a id="sec-generated-title-5"></a> <a id="collection-expressions">コレクション式と InlineArray</a>

前述の通り、
`InlineArray` 属性には `[EditorBrowsable(Never)]` が付いていて、
開発者が直接使う想定はあまりありません。

ただ、この機能は C# 12 時点で、コレクション式の最適化のために使われています。
`Span<T>` や `ReadOnlySpan<T>` 型に対してコレクション式を使うと、
`InlineArray` に展開されます。
例えば以下のようなコードの場合、

<pre class="source" title="Span/ReadOnlySpan に対するコレクション式の例">
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">i</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>];

<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">s</span> <span class="operator">=</span> [<span class="string">&quot;a&quot;</span>, <span class="string">&quot;abc&quot;</span>, <span class="string">&quot;&quot;</span>];
</pre>

以下のようなコードとほぼ同じ挙動になります。

<pre class="source" title="上記のコレクション式は InlineArray に展開される">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">i0</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">FixedArray5</span>&lt;<span class="reserved">int</span>&gt;();
<span class="variable">i0</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="number">1</span>;
<span class="variable">i0</span>[<span class="number">1</span>] <span class="operator">=</span> <span class="number">2</span>;
<span class="variable">i0</span>[<span class="number">2</span>] <span class="operator">=</span> <span class="number">3</span>;
<span class="variable">i0</span>[<span class="number">3</span>] <span class="operator">=</span> <span class="number">4</span>;
<span class="variable">i0</span>[<span class="number">4</span>] <span class="operator">=</span> <span class="number">5</span>;
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">i</span> <span class="operator">=</span> <span class="variable">i0</span>;

<span class="reserved">var</span> <span class="variable">s0</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">FixedArray3</span>&lt;<span class="reserved">string</span>&gt;();
<span class="variable">s0</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="string">&quot;a&quot;</span>;
<span class="variable">s0</span>[<span class="number">1</span>] <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>;
<span class="variable">s0</span>[<span class="number">2</span>] <span class="operator">=</span> <span class="string">&quot;&quot;</span>;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="variable">s0</span>;

[<span class="type">InlineArray</span>(<span class="number">3</span>)]
<span class="reserved">struct</span> <span class="type struct">FixedArray3</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value</span>;
}

[<span class="type">InlineArray</span>(<span class="number">5</span>)]
<span class="reserved">struct</span> <span class="type struct">FixedArray5</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value</span>;
}
</pre>

## <a id="sec-generated-title-6"></a> <a id="future">将来展望</a>

現状では、先ほどの例でいうと `FixedArray3<T>` と `FixedArray5<T>` があるように、
長さごとに別の型を用意せざるを得ない状態です。
「N 個のフィールドを並べる」コードを手書きするよりはマシですが、
まだ一時しのぎ的な実装になっていることは否めません。

根本的に大工事して型システムを改善するなら、
例えば、以下のように「整数型引数」を導入して、これを使って `InlineArray` を作りたいという話もなくはないです。

<pre class="source" title="「整数型引数」で InlineArray">
<span class="comment">// ※仮定の文法</span>
<span class="reserved">namespace</span> System;

<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type struct">InlineArray</span>&lt;<span class="type param">T</span>, <span class="reserved">int</span> <span class="variable">N</span>&gt;;
</pre>

こういう「public にできる(一時しのぎではないちゃんとした) `InlineArray` 型」があるのなら、
C# 側でももう少し踏み込んだ文法を導入したかったみたいです。
候補として挙がっていたのは、`int[N]` という書き方で「長さ N の `InlineArray`」を書けるようにするというものです。

<pre class="source" title="T[N]">
<span class="comment">// ※仮定の文法</span>
<span class="reserved">var</span> <span class="variable">c</span> = <span class="reserved">new</span> <span class="type">C</span>();

<span class="reserved">int</span>[3] <span class="reserved">values</span> = <span class="variable">c</span>.Values;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span>[3] _values;
    <span class="reserved">public</span> <span class="reserved">int</span>[3] Values =&gt; _values;
}
</pre>

前述の `InlineArray<T, int N>` みたいな書き方をできるようにするのは結構大変で、
短期的には実現しそうになく、
それに依存しそうな `int[N]` という書き方も残念ながらしばらく実現の見込みはありません。
