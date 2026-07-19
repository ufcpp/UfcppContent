---
title: "Span<T>構造体"
source_url: "https://ufcpp.net/study/csharp/resource/span/"
content_type: "Article"
published_at: "2017-11-08T00:00:00"
updated_at: "2026-07-01T14:03:03"
tags: []
umbraco_id: 2103
parent_id: 1286
sort_order: 6
aliases:
  - "/csharp/resource/span/"
---

# Span<T>構造体

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<h5 class="version version7">Ver. 7.2</h5>

`Span<T>`構造体(`System`名前空間)は、span (区間、範囲)という名前通り、連続してデータが並んでいるもの([配列](../structured/st_array.md)など)の一定範囲を読み書きするために使う型です。

この型によって、ファイルの読み書きや通信などの際の、生データの読み書きがやりやすくなります。
生データの読み書きを直接行うことは少ないでしょうが、通信ライブラリなどを利用することで間接的に`Span<T>`構造体のお世話になることはこれから多くなるでしょう。

`Span<T>`構造体は、 .NET Core 2.1 からは標準で入ります。それ以前のバージョンや、.NET Framework では、[System.Memory](https://www.nuget.org/packages/System.Memory/)パッケージを参照することで利用できます。

C# 7.2の新機能のうちいくつかは、この型を効率的に・安全に使うために入ったものです。
そこで、言語機能に先立って、この`Span<T>`構造体自体について説明しておきます。

<h5 class="version version14">Ver. 14</h5>

C# 14 では、`Span<T>` 構造体を言語構文的に特別扱いするようになって、より便利に使えるようになりました。
(C# 7.2 から C# 13 までの間、`Span<T>` 構造体はあくまでもあまたある普通の構造体の1つという扱いを脱していませんでした。)

こちらについては「[First-class Span](#first-class-span)」で説明します。

<h5>サンプル コード</h5>

- [サンプル コード](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Resource/Span)

##<a id="sec-generated-title-2"></a> <a id="data-span"></a>連続データの一定範囲の読み書き
「一定範囲の読み書き」の説明に、まずは配列で例を示します。
例えば以下のような書き方で、配列の一部分だけの読み書きができます。

<pre class="source" title="配列の一部分だけを読み書きする例">
<code><span class="comment">// 長さ 8 で配列作成</span>
<span class="comment">// C# の仕様で、全要素 0 で作られる</span>
<span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">int</span>[8];

<span class="comment">// 配列の、2番目(0 始まりなので3要素目)から、3要素分の範囲</span>
<span class="reserved">var</span> span = <span class="reserved">new</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;(array, 2, 3);

<span class="comment">// その範囲だけを 1 に上書き</span>
<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; span.Length; i++)
{
    span[i] = 1;
}

<span class="comment">// ちゃんと、2, 3, 4 番目だけが 1 になってる</span>
<span class="reserved">foreach</span> (var x <span class="reserved">in</span> array)
{
    <span class="type">Console</span>.WriteLine(x); <span class="comment">// 0, 0, 1, 1, 1, 0, 0, 0</span>
}
</code></pre>

このコードで、以下のような書き換えが発生します。

![配列の一部分だけを読み書きする例](../../../../assets/media/1148/span1.png)

`Span<T>`構造体を作る部分は、以下のように、拡張メソッドでも書けます。

<pre class="source" title="配列に対する拡張メソッドで Span を作る">
<code><span class="reserved">var</span> span = array.AsSpan().Slice(2, 3);
</code></pre>

この`AsSpan`は、`System.SpanExtensions`クラスで定義されている拡張メソッドで、
配列全体を指す `Span<T>` を作るものです。
また、`Slice`メソッドは`Span<T>`構造体の、さらに一部分だけを抜き出すメソッドです。

ちなみに、読み書き両方可能な`Span<T>`に加えて、読み取り専用の`ReadOnlySpan<T>`構造体もあります。

<pre class="source" title="読み取り専用の ReadOnlySpan">
<code><span class="comment">// 読み取り専用版</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; r = span;
<span class="reserved">var</span> a = r[0]; <span class="comment">// 読み取りは OK</span>
r[0] = 1;     <span class="comment">// 書き込みは NG</span>
</code></pre>

配列に限って言えば、「配列の一部分を指す型」として、昔から`ArraySegment<T>`構造体(`System`名前空間)がありました。
しかし、以下のような差があります。

- `Span<T>`は、配列だけでなく、いろいろなものを指せる
- `Span<T>`の方が効率的で、読み書きがだいぶ速い

##<a id="sec-generated-title-3"></a> <a id="various-data"></a>いろいろなタイプのメモリ領域を指せる
`Span<T>`は、配列だけでなく、文字列、スタック上の領域、.NET 管理外のメモリ領域などいろいろな場所を指せます。
以下のような使い方ができます。

<pre class="source" title="Span でいろいろな場所を指す">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 配列</span>
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; array = <span class="reserved">new</span> <span class="reserved">int</span>[8].AsSpan().Slice(2, 3);

        <span class="comment">// 文字列</span>
        <span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; str = <span class="string">"abcdefgh"</span>.AsReadOnlySpan().Slice(2, 3);

        <span class="comment">// スタック領域</span>
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; stack = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[8];

        <span class="reserved">unsafe</span>
        {
            <span class="comment">// .NET 管理外メモリ</span>
            <span class="reserved">var</span> p = <span class="type">Marshal</span>.AllocHGlobal(<span class="reserved">sizeof</span>(<span class="reserved">int</span>) * 8);
            <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; unmanaged = <span class="reserved">new</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;((<span class="reserved">int</span>*)p, 8);

            <span class="comment">// 他の言語との相互運用</span>
            <span class="reserved">var</span> q = malloc((<span class="type">IntPtr</span>)(<span class="reserved">sizeof</span>(<span class="reserved">int</span>) * 8));
            <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; interop = <span class="reserved">new</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;((<span class="reserved">int</span>*)q, 8);

            <span class="type">Marshal</span>.FreeHGlobal(p);
            free(q);
        }
    }

    [<span class="type">DllImport</span>(<span class="string">"msvcrt.dll"</span>, CallingConvention = CallingConvention.Cdecl)]
    <span class="reserved">static</span> <span class="reserved">extern</span> IntPtr malloc(IntPtr size);

    [<span class="type">DllImport</span>(<span class="string">"msvcrt.dll"</span>, CallingConvention = CallingConvention.Cdecl)]
    <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> free(IntPtr ptr);
}
</code></pre>

###<a id="sec-generated-title-4"></a> <a id="by-reference"></a>部分参照
`Span<T>`は、配列や文字列の一部分を直接参照しています。

例えば、`string`の`Substring`メソッドを使うと、部分文字列をコピーした新しい別の`string`が生成されて、ちょっと非効率です。
これに対して、`Span<char>`と`Slice`を使えば、コピーなしで部分文字列を参照できます。

例えば以下のようなコードを書いたとします。

<pre class="source" title="部分文字列の取り出し">
<code><span class="reserved">var</span> s = <span class="string">"abcあいう亜以宇"</span>;

<span class="reserved">var</span> sub = s.Substring(3, 3);
<span class="reserved">var</span> span = s.AsReadOnlySpan().Slice(3, 3);

<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 3; i++)
{
    <span class="type">Console</span>.WriteLine((sub[i], span[i])); <span class="comment">// あ、い、う が2つずつ表示される</span>
}
</code></pre>

`sub` (`Substring`メソッドを利用)と`span` (`Slice`メソッドを利用)はいずれも、「3番目から3つ分」の部分文字列を取り出しています。
しかし、以下のように、`sub`ではコピーが発生し、`span`では発生しません。

![Substring と Span の差](../../../../assets/media/1149/span2.png)

###<a id="sec-generated-title-5"></a> <a id="pointer-and-array"></a>配列とポインターに両対応
`Span<T>`を使う利点は、配列とポインターの両方に、1つの型で対応できることです。

[ネイティブ コードとの相互運用](../interop/sp_pinvoke.md)で有用なのはもちろん、
C# だけでプログラムを作るにしてもポインターを使いたいことが稀にあります
(主に、パフォーマンスが非常に重要になる場面で)。

例えば以下のようなコードを考えます。
[unsafe](../interop/sp_unsafe.md) を使うと速い処理の典型例として、一定範囲を 0 クリアする処理を、ポインターを使って書いています。

<pre class="source" title="unsafe を使うと速い処理の典型例">
<code><span class="comment">// unsafe を使うと速い処理の典型例として、一定範囲を 0 クリアする処理</span>
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 作る側</span>
    <span class="comment">// ライブラリを作る側としては別に unsafe コードがあっても不都合はそこまでない</span>
    <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> Clear(<span class="reserved">byte</span>* p, <span class="reserved">int</span> length)
    {
        <span class="reserved">var</span> last = p + length;
        <span class="reserved">while</span> (p + 7 &lt; last)
        {
            *(<span class="reserved">ulong</span>*)p = 0;
            p += 8;
        }
        <span class="reserved">if</span> (p + 3 &lt; last)
        {
            *(<span class="reserved">uint</span>*)p = 0;
            p += 4;
        }
        <span class="reserved">while</span> (p &lt; last)
        {
            *p = 0;
            ++p;
        }
    }

    <span class="comment">// 使う側</span>
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">byte</span>[256];

        <span class="comment">// array をいろいろ書き換えた後、全要素 0 にクリアしたいとして</span>

        <span class="comment">// ライブラリを使う側に unsafe が必要なのは怖いし面倒</span>
        <span class="reserved">unsafe</span>
        {
            <span class="reserved">fixed</span> (<span class="reserved">byte</span>* p = array)
                Clear(p, array.Length);
        }
    }
}
</code></pre>

コード中にも書いていますが、ここで問題になるのは、使う側に unsafe コードを強要する点です。
ライブラリを作る側は作る人の責任で多少危険なコードも書けますが、
どういう人が使うかはコントロールできないので、使う側に unsafe を求めるのはつらいです。
また、見ての通り、`unsafe`や`fixed`などのブロックで囲う処理は面倒です。

そこで、通常、以下のようにいくつかのオーバーロードを増やすことになります。

<pre class="source" title="unsafe を避けるためのオーバーロードいろいろ">
<code><span class="comment">// 使う側に unsafe を求めないために要するオーバーロードいろいろ</span>
<span class="reserved">static</span> <span class="reserved">void</span> Clear(<span class="type">ArraySegment</span>&lt;<span class="reserved">byte</span>&gt; segment) =&gt; Clear(segment.Array, segment.Offset, segment.Count);
<span class="reserved">static</span> <span class="reserved">void</span> Clear(<span class="reserved">byte</span>[] array, <span class="reserved">int</span> offset = 0) =&gt; Clear(array, offset, array.Length - offset);
<span class="reserved">static</span> <span class="reserved">void</span> Clear(<span class="reserved">byte</span>[] array, <span class="reserved">int</span> offset, <span class="reserved">int</span> length)
{
    <span class="reserved">unsafe</span>
    {
        <span class="reserved">fixed</span> (<span class="reserved">byte</span>* p = array)
        {
            Clear(p + offset, length);
        }
    }
}
</code></pre>

1セットくらいなら別にまだ平気なんですが、例えばコピー処理(コピー元とコピー先の2セット必要)とか、引数が増えるとかなり大変なことになります。

<pre class="source" title="コピー元とコピー先の2つになることで面倒になる例">
<code><span class="comment">// Clear は1つしか引数がないのでまだマシ。</span>
<span class="comment">// コピー(コピー元とコピー先)とか、2つになるとだいぶ面倒に。</span>

<span class="reserved">static</span> <span class="reserved">void</span> Copy(<span class="type">ArraySegment</span>&lt;<span class="reserved">byte</span>&gt; source, <span class="type">ArraySegment</span>&lt;<span class="reserved">byte</span>&gt; destination)
    =&gt; Copy(source.Array, source.Offset, destination.Array, destination.Offset, source.Count);
<span class="reserved">static</span> <span class="reserved">void</span> Copy(<span class="reserved">byte</span>[] source, <span class="reserved">int</span> sourceOffset, <span class="reserved">byte</span>[] destination, <span class="reserved">int</span> destinationOffset)
    =&gt; Copy(source, sourceOffset, destination, destinationOffset, source.Length - sourceOffset);
<span class="reserved">static</span> <span class="reserved">void</span> Copy(<span class="reserved">byte</span>[] source, <span class="reserved">int</span> sourceOffset, <span class="reserved">byte</span>[] destination, <span class="reserved">int</span> destinationOffset, <span class="reserved">int</span> length)
{
    <span class="reserved">unsafe</span>
    {
        <span class="reserved">fixed</span> (<span class="reserved">byte</span>* s = source)
        <span class="reserved">fixed</span> (<span class="reserved">byte</span>* d = destination)
        {
            Copy(s + sourceOffset, d + destinationOffset, length);
        }
    }
}
<span class="comment">// 他にも、利便性を求めるなら、</span>
<span class="comment">// source, destination の片方だけが ArraySegment のパターンとか</span>
<span class="comment">// 片方だけがポインターのパターンとか(組み合わせなのでパターンが多くなる)</span>

<span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> <span class="method">Copy</span>(<span class="reserved">byte</span>* <span class="variable">source</span>, <span class="reserved">byte</span>* <span class="variable">destination</span>, <span class="reserved">int</span> <span class="variable">length</span>)
{
    <span class="reserved">var</span> <span class="variable">last</span> = <span class="variable">source</span> + <span class="variable">length</span>;
    <span class="control">while</span> (<span class="variable">source</span> + 7 &lt; <span class="variable">last</span>)
    {
        *(<span class="reserved">ulong</span>*)<span class="variable">destination</span> = *(<span class="reserved">ulong</span>*)<span class="variable">source</span>;
        <span class="variable">source</span> += 8;
        <span class="variable">destination</span> += 8;
    }
    <span class="control">if</span> (<span class="variable">source</span> + 3 &lt; <span class="variable">last</span>)
    {
        *(<span class="reserved">uint</span>*)<span class="variable">destination</span> = *(<span class="reserved">uint</span>*)<span class="variable">source</span>;
        <span class="variable">source</span> += 4;
        <span class="variable">destination</span> += 4;
    }
    <span class="control">while</span> (<span class="variable">source</span> &lt; <span class="variable">last</span>)
    {
        *<span class="variable">destination</span> = *<span class="variable">source</span>;
        ++<span class="variable">source</span>;
        ++<span class="variable">destination</span>;
    }
}
</code></pre>

この問題に対して、`Span<T>`であれば、この構造体1つで配列でもポインターでも、その全体でも一部分でも受け取れるので、
オーバーロードは1つで十分です。

<pre class="source" title="Spanを使ってオーバーロードを減らす">
<code><span class="comment">// 作る側</span>
<span class="comment">// Span&lt;T&gt; なら配列でもポインターでも、その全体でも一部分でも受け取れる</span>
<span class="reserved">static</span> <span class="reserved">void</span> Clear(<span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; span)
{
    <span class="reserved">unsafe</span>
    {
        <span class="comment">// 結局内部的には unsafe にしてポインターを使った方が速い場合あり</span>
        <span class="reserved">fixed</span> (<span class="reserved">byte</span>* pin = &amp;span.GetPinnableReference())
        <span class="comment">// 注: C# 7.3 からは以下の書き方ができる</span>
        <span class="comment">// fixed (byte* pin = span)</span>
        {
            <span class="reserved">var</span> p = pin;
            <span class="reserved">var</span> last = p + span.Length;
            <span class="reserved">while</span> (p + 7 &lt; last)
            {
                *(<span class="reserved">ulong</span>*)p = 0;
                p += 8;
            }
            <span class="reserved">if</span> (p + 3 &lt; last)
            {
                *(<span class="reserved">uint</span>*)p = 0;
                p += 4;
            }
            <span class="reserved">while</span> (p &lt; last)
            {
                *p = 0;
                ++p;
            }
        }
    }
}

<span class="comment">// 使う側</span>
<span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">byte</span>[256];

    <span class="comment">// array をいろいろ書き換えた後、全要素 0 にクリアしたいとして</span>

    <span class="comment">// 呼ぶのがだいぶ楽</span>
    Clear(array);
}
</code></pre>

###<a id="sec-generated-title-6"></a> <a id="safe-stackalloc"></a>安全な stackalloc
C# の速度最適化のコツの1つに、「[ガベージ コレクション](rm_gc.md#garbage-collection)を避ける」というのがあります。
要は、可能であれば、クラスや配列の `new` を避けろという話になります。
(割かし「言うは易し」で、なかなか`new`を避けるのが大変なことはよくありますが。)

例えば、ファイルからデータを読み出しつつ、何か処理をしたいとします。
データは一気に全体を見る必要はなく、一定サイズずつ(仮にここでは128バイトずつ)読んでは捨ててを繰り返せるものとします。
これまでであれば、以下のように、そのサイズ分の配列を `new` して使うことになります。

<pre class="source" title="読み出し用の一時配列を new する例">
<code><span class="reserved">const</span> <span class="reserved">int</span> BufferSize = 128;

<span class="reserved">using</span> (<span class="reserved">var</span> f = <span class="type">File</span>.OpenRead(<span class="string">"test.data"</span>))
{
    <span class="reserved">var</span> rest = (<span class="reserved">int</span>)f.Length;
    <span class="reserved">var</span> buffer = <span class="reserved">new</span> <span class="reserved">byte</span>[BufferSize];

    <span class="reserved">while</span> (<span class="reserved">true</span>)
    {
        <span class="reserved">var</span> read = f.Read(buffer, 0, Math.Min(rest, BufferSize));
        rest -= read;

        <span class="comment">// buffer に対して何か処理する</span>

        <span class="reserved">if</span> (rest == 0) <span class="reserved">break</span>;
    }
}
</code></pre>

こういう場合に、これまでも、unsafe コードを使えば配列の `new` を避ける手段がありました。
[`stackalloc`](../interop/sp_unsafe.md#stackalloc)というものを使って、スタック上に一時領域を確保できます。
(スタックはガベージ コレクションの負担になりません。)
ただ、これだけのために unsafe コードを必要とするもの、ちょっとしんどいものがあります。

これに対して、C# 7.2では、`Span<T>`構造体と併用することで、unsafe なしで `stackalloc`を使えるようになりました。

例えば先ほどのコードは、以下のように書き直せます。
このコードはunsafeなしでコンパイルできます。
(※ .NET Core 2.1 で実行するか、他の環境では最新の [System.IO パッケージ](https://www.nuget.org/packages/System.IO/)の参照が必要です。現状ではプレビュー版のみ。)

<pre class="source" title="Span を使って一時バッファーを stackalloc に変更">
<code><span class="reserved">const</span> <span class="reserved">int</span> BufferSize = 128;

<span class="reserved">using</span> (<span class="reserved">var</span> f = <span class="type">File</span>.OpenRead(<span class="string">"test.data"</span>))
{
    <span class="reserved">var</span> rest = (<span class="reserved">int</span>)f.Length;
    <span class="comment">// Span&lt;byte&gt; で受け取ることで、new (配列)を stackalloc (スタック確保)に変更できる</span>
    <em><span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; buffer = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[BufferSize];</em>

    <span class="reserved">while</span> (<span class="reserved">true</span>)
    {
        <span class="comment">// Read(Span&lt;byte&gt;) が追加された</span>
        <span class="reserved">var</span> read = f.Read(buffer);
        rest -= read;
        <span class="reserved">if</span> (rest == 0) <span class="reserved">break</span>;

        <span class="comment">// buffer に対して何か処理する</span>
    }
}
</code></pre>

ただし、`Span<T>`相手であっても、`stackalloc`が使える型は[アンマネージ型](../interop/sp_unsafe.md#unmanaged-types)に限られます。
クラスなどに対しては使えません。

<pre class="source" title="">
<code><span class="comment">// これはOK。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; i = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[4];

<span class="comment">// こっちはダメ。</span>
<span class="comment">// Span&lt;string&gt; は大丈夫だけど、stackalloc string はダメ。</span>
<span class="type">Span</span>&lt;<span class="reserved">string</span>&gt; s = <span class="reserved">stackalloc</span> <span class="reserved"><span class="error">string</span></span>[4];
</code></pre>

ちなみに、スタック上の領域確保は、あんまり大きなサイズにはできません。
一般的には、多くても数キロバイト程度くらいまでしか使いません。
そのため、確保したいバッファーのサイズに応じて、`stackalloc`と配列の`new`を切り替えたいと言ったこともあります。
そこでC# 7.2 では、以下のように、条件演算子で`stackalloc`を使うこともできるようになっています。

<pre class="source" title="条件 stackalloc">
<code><span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; buffer = bufferSize &lt;= 128 ? <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[bufferSize] : <span class="reserved">new</span> <span class="reserved">byte</span>[bufferSize];
</code></pre>

また、unsafeが不要なことからもわかる通り、`Span<T>`との併用であれば`stackalloc`は安全です。
以下のように、範囲チェックが掛かって、確保した分を越えての読み書きはできないようになっています。

<pre class="source" title="Span + stackalloc は安全">
<code><span class="comment">// Span 版 = safe</span>
<span class="reserved">static</span> <span class="reserved">void</span> Safe()
{
    <span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; span = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[8];

    <span class="reserved">try</span>
    {
        <span class="comment">// 8バイトしか確保していないのに、9要素目に書き込み</span>
        span[8] = 1;
    }
    <span class="reserved">catch</span>(IndexOutOfRangeException)
    {
        <span class="comment">// ちゃんと例外が発生してここに来る</span>
        Console.WriteLine(<span class="string">"span[8] はダメ"</span>);
    }
}

<span class="comment">// ポインター版 = unsafe</span>
<span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> Unsafe()
{
    <span class="reserved">byte</span>* p = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[8];

    <span class="reserved">try</span>
    {
        <span class="comment">// 8バイトしか確保していないのに、9要素目に書き込み</span>
        p[8] = 1;
    }
    <span class="reserved">catch</span> (Exception)
    {
        <span class="comment">// ここには来ない！</span>
        <span class="comment">// 結果、不正な場所に 1 が書き込まれてるはず(かなり危険)</span>
        <span class="comment">// それも、エラーを拾う手段がないので気づきにくい</span>
        <span class="reserved">throw</span>;
    }
}
</code></pre>

###<a id="sec-generated-title-7"></a> <a id="nested-stackalloc"></a>式中の stackalloc
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、式中の任意の場所に `stackalloc` を書けるようになりました。
例えば以下のような書き方ができます。

<pre class="source" title="式中の任意の場所に stackalloc">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// Span を受け取る適当なメソッドを用意。</span>
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">buf</span>) =&gt; 0;
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">len</span>)
    {
        <span class="comment">// if の条件式中</span>
        <span class="control">if</span> (<span class="reserved">stackalloc</span> <span class="reserved">byte</span>[1] <span class="method">==</span> <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[1]) ;
        <span class="method">M</span>(<span class="reserved">stackalloc</span> <span class="reserved">byte</span>[1]);
 
        <span class="comment">// でもこれが今まではダメだった。</span>
        <span class="comment">// C# 8.0 ではコンパイルできる。</span>
        <span class="method">M</span>(<span class="variable">len</span> &gt; 512 ? <span class="reserved">new</span> <span class="reserved">byte</span>[<span class="variable">len</span>] : <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="variable">len</span>]);
 
        <span class="comment">// こういう書き方は C# 8.0 以前からできてた。条件演算子だけ特別扱いしてたらしい。</span>
        <span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">buf</span> = <span class="variable">len</span> &gt; 512 ? <span class="reserved">new</span> <span class="reserved">byte</span>[<span class="variable">len</span>] : <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="variable">len</span>];
    }
 
    <span class="comment">// フィールド初期化子の中でも書ける。</span>
    <span class="reserved">int</span> a = <span class="method">M</span>(<span class="reserved">stackalloc</span> <span class="reserved">byte</span>[8]);
 
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">MAsync</span>()
    {
        <span class="comment">// こういう入れ子の stackalloc の場合、非同期メソッド中でも書ける。</span>
        <span class="method">M</span>(<span class="reserved">stackalloc</span> <span class="reserved">byte</span>[1]);
 
        <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Yield</span>();
 
        {
            <span class="comment">// これは C# 8.0 でもダメ。</span>
            <span class="comment">// { } でくくってて(await をまたがない状態)もダメ。</span>
            <span class="error"><span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt;</span> <span class="variable">buf</span> = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[1];
        }
    }
}
</code></pre>

ただし、対象の型が `Span<T>` である必要があります。
ポインターに対する `stackalloc` にはこれまで通り `T* p = stackalloc T[len]` の形でしか書けません。

`Span<T> span = stackalloc T[len]` なら元々書けたので、
それと区別して「入れ子コンテキストでの `stackalloc`」(`stackalloc` in nested context)と言ったりします。

C# 7.3 時点でも、条件演算子の中でだけは `stackalloc` を書けましたが、
これは条件演算子だけ特別扱いしていたみたいです。
それに対して、C# 8.0 では本当にどこにでもかけます。

どうも、[再帰パターン](../datatype/patterns.md#recursive)を実装するついでにこの機能が入ったそうです。
(再帰パターン中に[参照](sp_ref.md#ref-returns)や[`ref`構造体](refstruct.md)が出てきても、戻り値に返していいものかどうかをちゃんと解析しないとまずくて、それが解析できるんなら`stackalloc`の安全性も解析できるとのこと。)

##<a id="sec-generated-title-8"></a> <a id="span-internal"></a>Span の内部的な話
前節では`Span<T>`構造体の用途を見てきましたが、続いて、その中身がどうなっているかについて説明しておきます。

`ArraySegment<T>`よりも`Span<T>`の方が高速な理由でもありますが、
`Span<T>`の中身は参照になっています。

比較のために`ArraySegment<T>`の中身から説明しましょう。
`ArraySegment<T>`は以下のようなメンバーを持った構造体です。

<pre class="source" title="ArraySegment の中身">
<code><span class="reserved">struct</span> <span class="type">ArraySegment</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span>[] Array;
    <span class="reserved">int</span> Offset;
    <span class="reserved">int</span> Count;
}
</code></pre>

![ArraySegmentの中身](../../../../assets/media/1150/arraysegmentinternal.png)

一方で、`Span<T>`構造体は、論理的には以下のようなメンバーを持った構造体です。
(「論理的には」と断っているのは、これをそのまま書くことはできないため。)

<pre class="source" title="Spanの中身(疑似コード。これをそのままは書けない)">
<code><span class="reserved">struct</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">ref</span> <span class="type">T</span> Reference;
    <span class="reserved">int</span> Length;
}
</code></pre>

![Spanの中身](../../../../assets/media/1151/spaninternal.png)

要するに、以下のような点が `Span<T>` の特徴になります。
(この他、`Span<T>`は .NET ランタイムが特別扱いしていくつか特殊な最適化を掛けてくれるため高速になります。)

- 必要な範囲の先頭を直接参照しているので、`+ Offset`分の計算が省ける
- `Array`と`Offset`と分けて持つ必要がないので、1メンバー分省サイズ
- 配列に限らずどこでも(ポインターでも)参照できる

###<a id="sec-generated-title-9"></a> <a id="two-implementations"></a>slow Span と fast Span
先ほど、`Span<T>`の中身には「論理的には」`ref T`なフィールドがあるという話をしました。
ただ、 .NET の型システム上、フィールドに `ref` を付けることはできませんでした(.NET 6 以前)。
実のところ、`Span<T>`はこういう「参照フィールド」を実現するためにちょっと特殊なことをしていました。

####<a id="sec-generated-title-10"></a> <a id="fast-span"></a>fast Span (.NET Core 2.1 以降向けの Span<T>)
.NET Core 2.1 では、ランタイム側で特殊処理を入れて、「参照フィールド」に相当する機能を使えるようにしました。
.NET Core 2.1 以降向けの `Span<T>` は以下のような構造になっています。
([coreclr レポジトリ内にソースコードがあります](https://github.com/dotnet/coreclr/blob/aae414026671e3dc1ccf0f308d351ac04cc746a4/src/mscorlib/shared/System/Span.cs#L29)。)

<pre class="source" title="fast Span の中身">
<code><span class="reserved">struct</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">ByReference</span>&lt;<span class="type">T</span>&gt; _pointer;
    <span class="reserved">int</span> _length;
}
</code></pre>

`ByReference<T>` が特殊対応部分です。
ランタイム側で「この型は参照フィールドとして扱う」という特別扱いをすることで、所望の動作を得ています。

####<a id="sec-generated-title-11"></a> <a id="fast-span7"></a>.NET 7 以降の fast Span
.NET 7 / C# 11 で、晴れて [ref フィールド](refstruct.md#ref-field)を持てるようになりました。
その結果、`Span<T>` は「普通の」ref 構造体になりました。
おおむね以下のような内容の構造体です。

<pre class="source" title=".NET 7 以降の Span 構造体">
<span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="type param">T</span> <span class="field">_reference</span>;
    <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="field">_length</span>;
}
</pre>

####<a id="sec-generated-title-12"></a> <a id="slow-span"></a>slow Span (旧来のランタイム向けの Span<T>)
「.NET Core 2.1以降でしか使えません」ということになると使い勝手が悪すぎるため、
旧来のランタイム向けの「ちょっと遅い」`Span<T>`実装もあります。
(こちらは[corefx リポジトリ内にソースコードがあります](https://github.com/dotnet/corefx/blob/8d212b41126baff94fc025e4438d6f4e8cbff7e9/src/System.Memory/src/System/Span.cs#L448)。)

こちらは、概ね以下のような構造です。

<pre class="source" title="slow Span の中身">
<code><span class="reserved">struct</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">Pinnable</span>&lt;<span class="type">T</span>&gt; _pinnable;
    <span class="type">IntPtr</span> _byteOffset;
    <span class="reserved">int</span> _length;
}
</code></pre>

`Pinnable<T>`はただのクラスです。
ガベージ コレクション管理下の参照と、管理外の参照を同列に扱えないからこういう構造になっています。
管理メモリ(配列)は `_pinnable` (ただのクラス)で扱い、管理外メモリ(相互運用で得たポインターや`stackalloc`で確保したメモリ)は `_byteOffset` に直接ポインター値を入れて扱います。

結果的に、管理下/管理外で条件分岐が必要だったり、構造体のサイズが大きくなるせいで、少し動作が遅くなります。
ただし、それでも、`ArraySegment<T>`を使うよりはだいぶ高速です。

###<a id="sec-generated-title-13"></a> <a id="ref-fields"></a>参照フィールド
要するに、`Span<T>`構造体は、論理的には「参照フィールドと、長さのペア」です。
実際、「fast Span」な実装では、参照フィールドに相当するものを、ランタイム側の特殊対応で実現しています。

となると、`Span<T>`の取り扱いには少し注意が必要になります。
「[参照戻り値と参照ローカル変数](sp_ref.md#ref-returns)」で説明していますが、
参照渡しでは、参照先が必ず有効であることを保証するために、いくつかの制限を掛けています。
それと同じ制限が`Span<T>`型の引数・変数・戻り値にも掛からなければいけません。

正確な条件などについては次節の「[ref 構造体](refstruct.md)」で説明します。


##<a id="sec-generated-title-14"></a> <a id="first-class-span">First-class Span</a>
<h5 class="version version14">Ver. 14</h5>

C# 14 では `Span<T>`/`ReadOnlySpan<T>` 構造体を言語構文的に特別扱いするようなりました。

###<a id="sec-generated-title-15"></a> <a id="before-first-class-span">C# 13 までの問題</a>
C# 7.2 の頃に `Span<T>` や `ReadOnlySpan<T>` が導入されて以来、
これらの型を使った高パフォーマンスな実装がたくさん提供されています。

以前なら `IEnumerable<T>` インターフェイスなどを使って実装していたものを、
C# 7.2 以降は `ReadOnlySpan<T>` 構造体を使って実装することが増えました。
例えば以下のように、引数の型を `IEnumerable<T>` から `ReadOnlySpan<T>` に書き換えるだけで高速になるということが多々あります。

<pre class="source" title="ReadOnlySpan 構造体を使うと高速になる">
<span class="reserved">class</span> <span class="type">Overloads</span>
{
    <span class="comment">// 昔からある伝統的な書き方。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>)
    {
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">values</span>)
        {
            <span class="comment">// 何か</span>
        }
    }

    <span class="comment">// C# 7.2 以降、全く同じ処理ならこっちの方が高速。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>)
    {
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">values</span>)
        {
            <span class="comment">// 同じ何か</span>
        }
    }
}
</pre>

.NET の標準ライブラリでは、例えば [`string.Join`](https://learn.microsoft.com/ja-jp/dotnet/api/system.string.join) メソッドなどがそうで、
.NET 9 (C# 13 世代)くらいで `ReadOnlySpan<T>` 引数のオーバーロードが追加されたものが多いです。

ただ、`Span<T>` や `ReadOnlySpan<T>` 引数のメソッドには使い勝手の問題がありました。
配列や文字列から `Span<T>` や `ReadOnlySpan<T>` への変換が「普通の構造体に定義されたユーザー定義の型変換」だったせいなんですが、[型推論](../start/sp3_inference.md)や[オーバーロード解決](../structured/miscoverloadresolution.md)ができなくなる場面が多かったです。

以下に3つほど例を挙げますが、C# 13 までは、いずれの例でもメソッド `M` の呼び出しがコンパイル エラーになっていました(後述するように、これが C# 14 ではコンパイルできるようになります)。

1つ目、は拡張メソッド呼び出し:

<pre class="source" title="拡張メソッド呼び出しができなかった例(C# 14 で解決)">
<span class="error" title="CS1929"><span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">0</span>]</span><span class="operator">.</span><span class="method">M</span>();
<span class="error" title="CS1929"><span class="string">&quot;&quot;</span></span><span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) { }
}</pre>

2つ目、ユーザー定義の型変換を介した呼び出し:

<pre class="source" title="ユーザー定義の型変換ができなかった例(C# 14 で解決)">
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="error" title="CS1503"><span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">0</span>]</span>);
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="error" title="CS1503"><span class="string">&quot;&quot;</span></span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">A</span> <span class="variable local">_</span>) { }
}

<span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type struct">A</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type struct">A</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
}
</pre>

3つ目、ジェネリック型引数の型推論:

<pre class="source" title="ジェネリック型推論ができなかった例(C# 14 で解決)">
<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="method"><span class="static"><span class="error" title="CS0411">M</span></span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">0</span>]);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

また、単独ではエラーにならなくても、`IEnumerable<T>` 引数との混在でオーバーロード解決できなくなる例もあります。

<pre class="source" title="IEnumerable と ReadOnlySpan の解決ができなかった例(C# 14 で解決)">
<span class="reserved">int</span>[] <span class="variable">data</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="type">Overloads</span><span class="operator">.</span><span class="method"><span class="static"><span class="error" title="CS0121">M</span></span></span>(<span class="variable">data</span>); <span class="comment">// 呼び分けができなくてコンパイル エラー(C# 13 まで)。</span>
<span class="type">Overloads</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">data</span><span class="operator">.</span><span class="method">AsSpan</span>()); <span class="comment">// ReadOnlySpan&lt;int&gt; 版を呼びたければこう書く。</span>

<span class="reserved">class</span> <span class="type">Overloads</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) { } <span class="comment">// こっちの方が高速</span>
}
</pre>

ちなみに、[C# 12 で入ったコレクション式](../datatype/collection-expression.md#priority)や、
[C# 13 で入った `params` コレクション](../structured/sp_params.md#params-collections)では、
`T[]` や `IEnumerable<T>` よりも `Span<T>` や `ReadOnlySpan<T>` を優先的に選ぶように特別な処理が入っています。

<pre class="source" title="コレクション式や params では ReadOnlySpan の優先度が高い">
<span class="comment">// int[] を経由すると解決不能になるものの、</span>
<span class="comment">// コレクション式や params を使った場合は ReadOnlySpan の優先度が高い扱い。</span>
<span class="type">Overloads</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);
<span class="type">Overloads</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);

<span class="reserved">class</span> <span class="type">Overloads</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">params</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>)
    {
        <span class="comment">// 何か</span>
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">values</span>)
    {
        <span class="comment">// 同じ何か</span>
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) <span class="comment">// これが最優先。</span>
    {
        <span class="comment">// 同じ何か</span>
    }
}
</pre>

この C# 13 で入ったコレクション式での特別扱いでもわかるように、
今や `Span<T>` や `ReadOnlySpan<T>` が重要な地位を占めていて、
C# の言語構文上も徐々に特別扱いされるようになって来ています。

###<a id="sec-generated-title-16"></a> <a id="after-first-class-span">C# 14 からの Span/ReadOnlySpan 特別扱い</a>
前節の不便は
あくまで `Span<T>` や `ReadOnlySpan<T>` が「ただの構造体」ということに起因します。
配列 `T[]` の変数を `Span<T>` や `ReadOnlySpan<T>` 型の変数/引数に渡せるのもあくまで
「`Span<T>` や `ReadOnlySpan<T>` 構造体に定義されたユーザー定義型変換」を経由しています。
C# 言語組み込みの型変換と比べて、ユーザー定義型変換は1段下扱いで、色々な不便があります。

この問題は、コレクション式の例からもわかる通り、
`Span<T>` や `ReadOnlySpan<T>` を言語組み込みにする(コンパイラーで特別扱いする)ことで解決します。
これを、「ただの構造体」扱いから「(`int` などと同列の)言語組み込みな型」扱いに格上げするという意味で、
「first-class (第一級、一流)化する」と言ったりします。

first-class になったことで、まず、
前述の `IEnumerable<T>` との呼び分けができない問題も、C# 14 にするだけで解消して、
`ReadOnlySpan<T>` 側が呼ばれるようになります。

<pre class="source" title="C# 14 では ReadOnlySpan オーバーロードの優先度が上がった">
<span class="reserved">int</span>[] <span class="variable">data</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="type">Overloads</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">data</span>); <span class="comment">// C# 14 であればエラーにならない。</span>

<span class="reserved">class</span> <span class="type">Overloads</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) { }

    <span class="comment">// こっちの方が高速。</span>
    <span class="comment">// C# 14 からオーバーロード解決で優先されるようになった。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) { }
}
</pre>

[拡張メソッド](../functional/sp3_extension.md)の場合はオーバーロード解決のルールがちょっと違うんですが、
「`ReadOnlySpan<T>` の方が有利なのに呼んでもらえなかった/呼べなかった」という問題はこちらにもありました。
これも、first-class 化したことで解決しています。
これまで、配列 `T[]` から `ReadOnlySpan<T>` への変換は
「ユーザー定義の変換なので拡張メソッドの解決に寄与しない」という扱いだったのが、
C# 14 からは「コンパイラーが保証している変換で、優先的に拡張メソッドの解決に使われる」という扱いになります。

<pre class="source" title="拡張メソッドでも ReadOnlySpan が特別扱いされるように">
<span class="reserved">int</span>[] <span class="variable">data</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// C# 13 まで: IEnumerable の方が呼ばれる。</span>
<span class="comment">//             (というか ReadOnlySpan の方しかないとコンパイル エラーになる。)</span>
<span class="comment">// C# 14 から: ReadOnlySpan の方が呼ばれる。</span>
<span class="variable">data</span><span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">values</span>) { } <span class="comment">// こっちが高速なのでこっちを読んでほしい。</span>
}
</pre>

#####<a id="sec-generated-title-17"></a> <a id="covariance">ReadOnlySpan の共変性</a>
また、[ユーザー定義の型変換](../oop/oo_operator.md#cast)では「型引数の[共変性](../oop/sp4_variance.md#variance)」を表現できないという問題があります。
`ReadOnlySpan<string>` を `ReadOnlySpan<object>` に代入できてもいいはずなのに、
これが C# 13 まではできませんでした。
C# 14 からはこれを受け付けます。

<pre class="source" title="ReadOnlySpan の共変性">
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">s</span> <span class="operator">=</span> [];
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="variable">s</span>; <span class="comment">// C# 13 ではエラー。</span>
</pre>

#####<a id="sec-generated-title-18"></a> <a id="read-only-span-over-span">Span よりも ReadOnlySpan の方を優先</a>
ちなみに、`Span<T>` と `ReadOnlySpan<T>` の両方のオーバーロードがある場合、
`ReadOnlySpan<T>` の方が優先されます。

<pre class="source" title="ReadOnlySpan 優先">
<span class="reserved">string</span>[] <span class="variable">s</span> <span class="operator">=</span> [];

<span class="comment">// ReadOnlySpan の方が優先。</span>
<span class="variable">s</span><span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">_</span>) { } <span class="comment">// こちらが呼ばれる。</span>
}
</pre>

これはパフォーマンス(どちらが高速か)の問題ではなく、
こうしておかないとまた「[配列の共変性の地雷](../../../blog/2022/11/covariantarrayincident/index.md)を踏むから」という理由だそうです。

<pre class="source" title="配列の共変性は結構な地雷">
<span class="reserved">string</span>[] <span class="variable">s</span> <span class="operator">=</span> [];
<span class="reserved">object</span>[] <span class="variable">o</span> <span class="operator">=</span> <span class="variable">s</span>; <span class="comment">// C# の配列は共変(歴史的経緯)。</span>

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
