---
title: "小ネタ 構文糖衣と、そうではない構文と"
source_url: "https://ufcpp.net/blog/2016/12/tipsgeneratedil/"
content_type: "BlogEntry"
published_at: "2016-12-18T09:55:07"
updated_at: "2016-12-18T09:55:07"
tags: []
umbraco_id: 2001
parent_id: 1969
sort_order: 17
aliases: []
---

# 小ネタ 構文糖衣と、そうではない構文と

## 構文糖衣が多い言語

C#は構文糖衣が結構多い言語です。
構文糖衣(syntax sugar)っていうのは、要するに、「定型的な長くて面倒なコードにはなるけども、原理的にはその構文がなくても全く同じ意味のコードが書ける」というような類の機能です。

例えば[クエリ式](../../../../study/csharp/data/sp3_linq.md#query)がわかりやすいですが、以下の3つの式は全く同じ意味になります。

<pre class="source" title="クエリ式">
<code><span class="reserved">from</span> x <span class="reserved">in</span> data <span class="reserved">where</span> x &gt; 2 <span class="reserved">select</span> x * x
</code></pre>

<pre class="source" title="クエリ式 → メソッド呼び出しに展開">
<code>data.Where(x =&gt; x &gt; 2).Select(x =&gt; x * x)
</code></pre>

<pre class="source" title="拡張メソッド → 静的メソッドに展開">
<code><span class="type">Enumerable</span>.Select(<span class="type">Enumerable</span>.Where(data, x =&gt; x &gt; 2), x =&gt; x * x)
</code></pre>

ということで、C#の機能を説明するとき、結構、「こういうコードと同じ意味になります」というような文章を書くことは多いです。

特に、ここで例に挙げたクエリ式は、式 → メソッド呼び出し → 必要があれば拡張メソッドの静的メソッドへの展開 と、2段階の変換を行う機能です。
いうなれば、「2段階」構文糖衣。

## 構文糖衣でない文法

逆に、C#に対して後から追加された構文の全部が全部単なる構文糖衣ではなく、コンパイル結果まで覗いてみる必要のものもちらほらあります。

もちろん、[C# 2.0の時のジェネリックの追加](../../../../study/csharp/cheatsheet/ap_ver2.md#generics)のように、.NET Frameworkのランタイム自体に手を入れることで実現した機能もあります。

一方で、「ランタイムのレベル(型システムや中間言語(IL: Intermediate Language)の仕様レベル)では元々機能としてあるけども、
C# からは使えなかったものを使えるようにした」 というような機能もいくつかあります。
代表的なのは以下の2つでしょう。

- C# 4.0から: [ジェネリックの変性](../../../../study/csharp/oop/sp4_variance.md)(variance)
- C# 7から: [参照戻り値](../../../../study/csharp/resource/sp_ref.md#ref-returns)

## ジェネリックの変性

[ジェネリックの変性](../../../../study/csharp/oop/sp4_variance.md)(variance)は割かしわかりやすい例でしょうか。
C#上の構文糖衣でどうこうできるわけではなくて、.NETのメタデータのレベルで処理されています。

例えばC# 4.0以降で以下のようなコードを書いたとしましょう。型引数`T`に、共変であることを示す`out`修飾子を付けています。

<pre class="source" title="共変なジェネリック型引数T">
<code><span class="reserved">interface</span> <span class="type">IWrapper</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="type">T</span> Value { <span class="reserved">get</span>; }
}
</code></pre>

逆アセンブルしてみると以下のようになっています。
ILのレベルでも、`T`の前に`+`という記号が入っていますが、これが共変を表すフラグです。

<pre class="source" title="">
<code>.class <span class="reserved">interface</span> <span class="reserved">private</span> <span class="reserved">abstract</span> <span class="reserved">auto</span> <span class="reserved">ansi</span> IWrapper`1&lt;+ T&gt;
{
} <span class="comment">// end of class IWrapper`1
</code></pre>

このフラグは、IL的には .NET 2.0の頃からありましたが、
C#からこのフラグをいじれるようになったのはC# 4.0(.NET 4と同世代)からです。

まあ、何にしても「以前のC#であればこう展開されます」的な説明はできない機能です。

## 参照戻り値

最近だと[参照戻り値](../../../../study/csharp/resource/sp_ref.md#ref-returns)なんかもそうです。

ILって実は安全性を損なうきわどい書き方をやりたい放題で、
C#のレベルで制限を掛けて安全性を担保していることが結構あります。
参照戻り値は、

- これまで安全性を担保するための解析がしんどかったから使えなかった、
- C# 7では、C#コンパイラーが賢くなったからその解析ができるようになって、認められるようになった

というものなわけです。
ILはそんな安全性とか気にしないので、昔から参照戻り値を使えました。

例えば、C# 7で以下のようなコードを書いたとします。

<pre class="source" title="参照戻り値を使ったC#コード">
<code><span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> RefMax(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">ref</span> <span class="reserved">int</span> y)
{
    <span class="reserved">if</span> (x &gt;= y) <span class="reserved">return</span> <span class="reserved">ref</span> x;
    <span class="reserved">else</span> <span class="reserved">return</span> <span class="reserved">ref</span> y;
}
</code></pre>

コンパイル結果は以下の通り。`&`が参照を表す記号です。

<pre class="source" title="IL的には参照は &amp;">
<code>.method <span class="reserved">private</span> <span class="reserved">hidebysig</span> <span class="reserved">static</span> <span class="reserved">int32</span>&amp;  RefMax(<span class="reserved">int32</span>&amp; x,
                                                <span class="reserved">int32</span>&amp; y) <span class="reserved">cil</span> <span class="reserved">managed</span>
{
  <span class="comment">// コード サイズ       10 (0xa)
</span>  .maxstack  8
  IL_0000:  ldarg.0
  IL_0001:  ldind.i4
  IL_0002:  ldarg.1
  IL_0003:  ldind.i4
  IL_0004:  blt.s      IL_0008
  IL_0006:  ldarg.0
  IL_0007:  ret
  IL_0008:  ldarg.1
  IL_0009:  ret
} <span class="comment">// end of method Program::RefMax
</span>
</code></pre>

このコードは、.NET 1.0の頃から書けました。
もっとも、`int32&` (C# 7でいう`ref int`)の戻り値を受け取る手段がなかったので、実際のところ書いてもC#からは使えないコードになります(例えばこのコードをVisual Studio 2017でコンパイルして、その結果のDLLをVisual Studio 2015から参照した場合、このメソッドはクラスのメンバー一覧情報のところに表示されません)。

ちなみに、参照に対する操作は、実のところポインターに対する操作とまったく同じになります。
例えば、以下のようなunsafeなコードを書いてみます。
ポインターになっただけで、やっていることは先ほどの参照を使ったコードとまったく同じです。

<pre class="source" title="ポインターを使ったC#コード">
<code><span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">int</span>* RefMax(<span class="reserved">int</span>* x, <span class="reserved">int</span>* y)
{
    <span class="reserved">if</span> (*x &gt;= *y) <span class="reserved">return</span> x;
    <span class="reserved">else</span> <span class="reserved">return</span> y;
}
</code></pre>

こちらのコンパイル結果は以下のようになります。`&`が`*`に変わった以外の部分は一字一句たがわず、先ほどのコードと完全に一致しています。

<pre class="source" title="IL的にはポインターは*">
<code>.method <span class="reserved">private</span> <span class="reserved">hidebysig</span> <span class="reserved">static</span> <span class="reserved">int32</span>*  RefMax(<span class="reserved">int32</span>* x,
                                                <span class="reserved">int32</span>* y) <span class="reserved">cil</span> <span class="reserved">managed</span>
{
  <span class="comment">// コード サイズ       10 (0xa)
</span>  .maxstack  8
  IL_0000:  ldarg.0
  IL_0001:  ldind.i4
  IL_0002:  ldarg.1
  IL_0003:  ldind.i4
  IL_0004:  blt.s      IL_0008
  IL_0006:  ldarg.0
  IL_0007:  ret
  IL_0008:  ldarg.1
  IL_0009:  ret
} <span class="comment">// end of method Program::RefMax
</span>
</code></pre>

`ldind`はload indirect (間接ロード)の略で、
ポインターや参照ごしに値を取ってくる命令です。
ポインターと参照でまったく同じ命令を使います。

## おまけ: `Unsafe`

余談となりますが、ILを使えば、自己責任で安全でないコード書き放題だという例にも触れておきます。

今、[System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)とかいう名前からしてunsafeなライブラリがあったりします。

このパッケージ中にある`Unafe`クラスは、[3日目](../tipscorefxlab/index.md)に紹介した`System.Memory`の中で使われています。
というか、元々は`System.Memory`パッケージ内にあったコードを、これ単体で使えるだろうと切り出した結果が`System.Runtime.CompilerServices.Unsafe`パッケージです。

ソースコードも以下のGitHubリポジトリで公開されているので中身を覗いてみると…

- [dotnet/corefx: System.Runtime.CompilerServices.Unsafe](https://github.com/dotnet/corefx/blob/master/src/System.Runtime.CompilerServices.Unsafe)

[完全にILで書かれています](https://github.com/dotnet/corefx/blob/master/src/System.Runtime.CompilerServices.Unsafe/src/System.Runtime.CompilerServices.Unsafe.il)。

ポインターと参照を相互に変換するメソッドなんかもあったりするんですが、
以下のような感じで、実はほぼ素通しです。
ロード(`ldarg`)して、即リターン(`ret`)。

<pre class="source" title="参照とポインターは実はほぼ素通しで変換可能">
<code>.method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">static</span> <span class="reserved">void</span>* AsPointer&lt;T&gt;(!!T&amp; 'value') <span class="reserved">cil</span> <span class="reserved">managed</span> aggressiveinlining
  {
        .custom <span class="reserved">instance</span> <span class="reserved">void</span> System.Runtime.Versioning.NonVersionableAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 )
        .maxstack 1
        ldarg.0
        conv.u
        ret
  } <span class="comment">// end of method Unsafe::AsPointer
</span>
    .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">static</span> !!T&amp; AsRef&lt;T&gt;(<span class="reserved">void</span>* source) <span class="reserved">cil</span> <span class="reserved">managed</span> aggressiveinlining
  {
        .custom <span class="reserved">instance</span> <span class="reserved">void</span> System.Runtime.Versioning.NonVersionableAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 )
        .maxstack 1
        ldarg.0
        ret
  } <span class="comment">// end of method Unsafe::AsRef
</span>
</code></pre>

ちなみに、`conv.u`命令は、ネイティブ(CPUの種類に応じて32bitか64bitか切り替わる)符号なし整数への変換命令です。
ポインター = ネイティブ符号なし整数。
