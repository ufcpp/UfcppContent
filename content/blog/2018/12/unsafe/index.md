---
title: "System.Runtime.CompilerServices.Unsafe"
source_url: "https://ufcpp.net/blog/2018/12/unsafe/"
content_type: "BlogEntry"
published_at: "2018-12-14T09:35:46"
updated_at: "2018-12-14T09:35:46"
tags: []
umbraco_id: 2195
parent_id: 2177
sort_order: 13
aliases: []
---

# System.Runtime.CompilerServices.Unsafe

昨日から始まった在庫一掃処分セール的なブログなんですが、結構な頻度で「`Unsafe` クラス」ってのが出てきます。

以下のパッケージに含まれているもので、こいつをを参照すれば、通常の C# では書けないようなどぎつい unsafe な真似がし放題になります。

- [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)

これの登場はもう結構前なんですけども、そういえばちゃんとした説明をしたことなかったなと。

## .NET の IL は意外とやりたい放題

上記パッケージにはパッケージ名と同じ`Unsafe`というクラスが入っています。
この`Unsafe`クラス、ソースコードはこんな感じ:

- [System.Runtime.CompilerServices.Unsafe.il](https://github.com/dotnet/corefx/blob/64c6d9fe5409be14bdc3609d73ffb3fea1f35797/src/System.Runtime.CompilerServices.Unsafe/src/System.Runtime.CompilerServices.Unsafe.il)

ILアセンブリ実装です。

C# では書けなくても、IL なら何も特別なことをしなくてもやりたい放題。
要するに、.NET における「安全」は、結構 C# のレベルで保証しています。

とはいえ、unsafe でもいいので、C# でできないのは困るということで提供されるようになったのがこの`Unsafe`クラスです。
C# の文法を拡張するよりは、こういう IL 実装なクラスを提供する方が手っ取り早かったのでこんなことになりました。

## ポインターの方がまだマシ疑惑

とはいえ、この`Unsafe`クラスをフル活用すると、こんなコードになります。

<pre class="source" title="Unsafe クラス">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> UnsafeClass(<span class="reserved">int</span>[] array)
    {
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">ref</span> <span class="reserved">var</span> begin = <span class="reserved">ref</span> array[0];
        <span class="reserved">ref</span> <span class="reserved">var</span> p = <span class="reserved">ref</span> <span class="type">Unsafe</span>.As&lt;<span class="reserved">int</span>, <span class="reserved">byte</span>&gt;(<span class="reserved">ref</span> begin);
        <span class="reserved">var</span> length = array.Length * 4;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; length; i++, p = <span class="reserved">ref</span> <span class="type">Unsafe</span>.Add(<span class="reserved">ref</span> p, 1))
            sum += p;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

ちなみに、普通に C# で unsafe コードを使って同じものを書くと以下のようになります。

<pre class="source" title="unsafe コードで同じもの">
<code><span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">int</span> UnsafeContext(<span class="reserved">int</span>[] array)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">fixed</span> (<span class="reserved">int</span>* begin = &amp;array[0])
    {
        <span class="reserved">var</span> p = (<span class="reserved">byte</span>*)begin;
        <span class="reserved">var</span> length = array.Length * 4;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; length; i++, p++)
            sum += *p;
    }
    <span class="reserved">return</span> sum;
}
</code></pre>

見た目に関しては、ポインターを使った後者の方がまだマシなんじゃないでしょうか。

だったら素直に unsafe コードを使う方がいいんじゃないかという話になるとは思いますが、
いくつか、`Unsafe`クラスでしかできないことがあります。

- ポインターの代わりに [`ref`](../../../../study/csharp/resource/sp_ref.md) で操作できる
- ジェネリックな型をポインター化できる

### ポンターの代わりに ref

ポインターと `ref` は内部的には似たようなものです。
大体同じ命令を使って間接参照します。
ですが、
1つ決定的に違うのが、`ref`なら[ガベコレ](../../../../study/csharp/resource/rm_gc.md#garbage-collection)が追えるという点があります。

<pre class="source" title="ref とガベコレ">
<code><span class="comment">// ref 戻り値ならこんなコードを書いても平気。</span>
<span class="comment">// 戻り値が「参照」されている限り、配列自体の参照がガベコレにトラッキングされる。</span>
<span class="reserved">ref</span> <span class="reserved">int</span> X()
{
    <span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">int</span>[1];
    <span class="reserved">return</span> <span class="reserved">ref</span> array[0];
}
 
<span class="comment">// 一方、これはダメ。</span>
<span class="comment">// ガベコレが走ったら、もはやポインターが有効な場所を指さなくなる。</span>
<span class="reserved">unsafe</span> <span class="reserved">int</span>* Y()
{
    <span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">int</span>[1];
    <span class="reserved">fixed</span> (<span class="reserved">int</span>* p = array)
        <span class="reserved">return</span> p;
}
</code></pre>

ということで、`ref`を使って unsafe なことをしたいときに使うのが
`Unsafe` クラスです。

例としては[`Span<T>`構造体](../../../../study/csharp/resource/span.md)があります。
(というか、`Unsafe`クラスを導入するに至った最初の動機は`Span<T>`構造体を作るためでした。)

`Span<T>`は、以下のように、配列でもポインターでも統一的に扱える型です。

<pre class="source" title="Span&lt;T&gt;">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 配列</span>
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; array = <span class="reserved">new</span> <span class="reserved">int</span>[8].AsSpan().Slice(2, 3);
 
        <span class="comment">// 文字列</span>
        <span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; str = <span class="string">&quot;abcdefgh&quot;</span>.AsSpan().Slice(2, 3);
 
        <span class="comment">// スタック領域</span>
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; stack = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[8];
 
        <span class="reserved">unsafe</span>
        {
            <span class="comment">// ガベコレ管理外メモリ</span>
            <span class="reserved">var</span> p = <span class="type">Marshal</span>.AllocHGlobal(<span class="reserved">sizeof</span>(<span class="reserved">int</span>) * 8);
            <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; unmanaged = <span class="reserved">new</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;((<span class="reserved">int</span>*)p, 8);
 
            <span class="comment">// 他の言語との相互運用</span>
            <span class="reserved">var</span> q = malloc((<span class="type">IntPtr</span>)(<span class="reserved">sizeof</span>(<span class="reserved">int</span>) * 8));
            <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; interop = <span class="reserved">new</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;((<span class="reserved">int</span>*)q, 8);
 
            <span class="type">Marshal</span>.FreeHGlobal(p);
            free(q);
        }
    }
 
    [<span class="type">DllImport</span>(<span class="string">&quot;msvcrt.dll&quot;</span>, CallingConvention = <span class="type">CallingConvention</span>.Cdecl)]
    <span class="reserved">static</span> <span class="reserved">extern</span> <span class="type">IntPtr</span> malloc(<span class="type">IntPtr</span> size);
 
    [<span class="type">DllImport</span>(<span class="string">&quot;msvcrt.dll&quot;</span>, CallingConvention = <span class="type">CallingConvention</span>.Cdecl)]
    <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> free(<span class="type">IntPtr</span> ptr);
}

</code></pre>

こういう型を作ろうと思うと、通常なら unsafe コードだらけ・ポインターだらけになるんですが、
`Span<T>`構造体はその代わりに [`Unsafe` クラスだらけ・`ref`だらけ](https://source.dot.net/#System.Private.CoreLib/shared/System/Span.Fast.cs,d2517139cac388e8)です。

### ジェネリックな型をポインター化

C# の unsafe コードの仕様では、ジェネリックな型はポインター化できません。
とはいえ、この制限は実はちょっと厳しすぎです。

<pre class="source" title="ジェネリック構造体とポインター">
<code><span class="comment">// 値型しか含まない構造体はポインター化 (A*) できる。</span>
<span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
}
 
<span class="comment">// 1つでも参照型を含んでいる場合、ポインター化されるとガベコレが追えなくなって困る。</span>
<span class="comment">// なので、ポインター化できない仕様もやむなし。</span>
<span class="reserved">struct</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> X;
}
 
<span class="comment">// ならこのジェネリックな場合はどうか。</span>
<span class="comment">// T に値型を渡したとき、値型しか含まない構造体になり得る。</span>
<span class="comment">// T 次第でポインター化できるかどうか変えてもよかったのではないか。</span>
<span class="comment">// (現状は無条件にポインター化 (C&lt;int&gt;* とかも) 不可)</span>
<span class="reserved">struct</span> <span class="type">C</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span> X;
}
</code></pre>

C# 7.3 で [unmanaged 制約](../../../../study/csharp/interop/sp_unsafe.md#unmanaged-types)が入って、
多少は制限が緩和したんですが、いまだこの例の `C<T>` のような型はポインター化できません。
(C# 8.0 で緩和される可能性あり。遅くとも C# 8.x の間には緩和されると思われます。)

が、`Unsafe`クラスを使えば(今でも)そんな制限をガン無視できます。

<pre class="source" title="ジェネリック構造体からポインターを取得">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">struct</span> <span class="type">C</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> X;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> c = <span class="reserved">new</span> <span class="type">C</span>&lt;<span class="reserved">int</span>&gt;();
        <span class="reserved">int</span>* p = (<span class="reserved">int</span>*)<span class="type">Unsafe</span>.AsPointer(<span class="reserved">ref</span> c);
        *p = 1;
        <span class="type">Console</span>.WriteLine(c.X); <span class="comment">// 1</span>
    }
}
</code></pre>

## Unsafe クラスを safe なところから呼べる

もちろん、`Unsafe` クラス悪用すると、unsafe コード以上に unsafe になります。

にもかかわらず、`Unsafe`クラスのメソッドの引数・戻り値は大半が `ref` になっているので、unsafe コードなしで呼び出せます。
ある意味、これが一番の欠陥で、言語機能の不足を感じます
(「ポインターは使っていないけども unsafe コードからしか呼べない」みたいな制約を付けれる機能が欲しい)。
(実際、corefx/coreclr 内でも度々そういう話題は上がっています。
そもそも利用頻度が低いクラスなので需要はあんまりありませんが…)
