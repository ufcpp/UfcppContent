---
title: "unsafe"
source_url: "https://ufcpp.net/study/csharp/interop/sp_unsafe/"
content_type: "Article"
published_at: "2003-06-22T00:00:00"
updated_at: "2019-08-12T00:00:00"
tags: []
umbraco_id: 1322
parent_id: 1321
sort_order: 0
aliases:
  - "/csharp/interop/sp_unsafe/"
  - "/csharp/sp_unsafe"
  - "/csharp/sp_unsafe.html"
  - "/study/csharp/sp_unsafe"
  - "/study/csharp/sp_unsafe.html"
---

# unsafe

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# や Java などのプログラミング言語では、
コンピュータのメモリ上の任意の場所に自由にアクセスするための手段、
すなわち、ポインターの利用が禁止もしくは制限されています。

ポインターは、その自由さから、非常に有用であると同時に、
危険なものでもあり、バグの原因になりやすいという問題がありました。
そのため、C# や Java などの言語では、
ポインターの代替となる物を用意し、
必要最小限の機能のみを提供する事によって、
簡単でかつ堅牢なプログラミング環境を提供しています。

ただし、C# では、
C言語などの既存のプログラミング言語との相互運用性や、
プログラムの実行効率向上のために、
ポインターを完全に廃止するのではなく、
<strong id="unsafe" class="keyword">unsafe</strong> コンテキストと呼ばれる特別な場面でのみポインターを利用できるようにしています。


##### <a id="sec-generated-title-2"></a>ポイント

* unsafe キーワードの付いたメソッド内や、unsafe ブロック内限定で、ポインターなどの低レベル機能が使える。

* unsafe を使うためには、コンパイル時に /unsafe オプション(`AllowUnsafeBlocks`)を付ける必要がある。

* C言語などとの相互運用のためにあるもの。それ以外の用途（パフォーマンス向上など）に使うのは最終手段。



## <a id="sec-generated-title-3"></a> <a id="about-pointer"></a>ポインターとは

（
「[コンピュータの基礎知識](../../computer/index.md)」に、
もう少し詳しいポインターの説明を書いたので、
そちらも参照してみてください →
「[メイン・メモリ](../../computer/general/memory.md)」。
）

プログラム中で使用する変数の値はメモリに記憶されています。
図1に示すように、メモリ上には値を格納するための領域が一直線に並んで、
それぞれに<em>アドレス</em>(変数の所在地を示す番号)が付いています。
そして、アドレスを格納するために用いるのが<em>ポインター</em>(アドレスを指し示す変数という意味)です。

<figure>
	[![アドレス](../../../../assets/media/ufcpp2000/csharp/fig/pointer1.png)](../../../../assets/media/ufcpp2000/csharp/fig/pointer1.png)
	<figcaption>アドレス</figcaption>
</figure>


ポインターを説明するために、C# の前身であるC++によるポインターの利用例を示します。
C++では、変数の宣言するとき、
型名の後に `*` を付けるとポインター変数になります。
また、変数の前に <code>&amp;</code> を付けることで、
その変数のアドレスを取り出すことができます。
逆に、ポインターの参照先の値を読み書きするには、
ポインター変数の前に `*` を付けます。

<pre class="source" title="ポインターの宣言、アドレス取り出し(C++)" lang="">
<code><span class="comment">// 注: C++ です。</span>

<span class="reserved">int</span>* p; <span class="comment">// ポインターの宣言</span>
<span class="reserved">int</span> n = 30;

p = &amp;n;     <span class="comment">// ポインター p に n のアドレスを代入</span>
cout &lt;&lt; *p; <span class="comment">// p の参照先 (n) の値 (30) を読み出す</span>
*p = 20;    <span class="comment">// p の参照先 (n) の値を 20 に書き換える</span>
cout &lt;&lt; n;  <span class="comment">// n は 20 に書き換わっている</span>
</code></pre>


このように、ポインターを使うことで他の変数を参照することが出来ます。
さらに、ポインターにはあくまでメモリ上のアドレスがそのまま数値として格納されていて、
<code>+</code>, <code>--</code>, <code>++</code>, <code>--</code> などの演算子を使って値を自由に変更できます。
この自由さのため、ポインターは正しく使用すれば非常に強力な道具になりますが、
ほんのちょっとした不注意からプログラマの意図しない動作を起こすことがあり、
扱いの難しいものとなっていました。

このような問題を解決するため、
C# や Java などのプログラミング言語では、
ポインターの代替となる機能を提供し、
ポインターの使用を禁止もしくは制限しています。

ここでは、ポインターの詳細についてはこれ以上触れませんが、
従来のプログラミング言語においてポインターがどのような場面で使用されいたのかと、
C# においてどのような機能で代替出来るのかだけ、
以下に簡単にまとめます。

<table summary="">

	<tr>
		<th>ポインターを必要とする場面</th>
		<th>代替機能</th>
	</tr>
	<tr>
		<td markdown="1">動的確保</td>
		<td markdown="1">参照型の変数は常に動的に確保される。</td>
	</tr>
	<tr>
		<td markdown="1">動的配列</td>
		<td markdown="1">C# の配列は元々動的。</td>
	</tr>
	<tr>
		<td markdown="1">引数の参照渡し</td>
		<td markdown="1">参照変数を使うもしくは ref、out キーワードを使う。</td>
	</tr>
	<tr>
		<td markdown="1">配列に対する反復処理の効率化</td>
		<td markdown="1">for や foreach に対してコンパイラーが最適化を掛けて、効率のいいコードにする。</td>
	</tr>
</table>



## <a id="sec-generated-title-4"></a> <a id="unsafe"></a>unsafe コード

従来のプログラミング言語でポインターを必要としていた場面のほとんどは、
他の機能で代替することが出来るため、
C# や Java 言語にとってポインターは必須なものではありません。
そのため、Java 言語ではポインターを完全に廃止しています。
しかし、C# ではプログラムの効率化と従来のプログラミング言語との相互運用を目的として、
制限付きながらポインターの使用可能にしてあります。

まず、ポインター使用における制限ですが、
C# では <em>unsafe キーワード</em>を用いて宣言されたメソッドもしくはブロック内(このようなコードを <em>unsafe コード</em>と呼びます)でしかポインターを使用できません。
メソッドに unsafe 修飾子を付けることでそのメソッド内部は unsafe コードとなり、
そのメソッド内でポインターを使用できるようになります。
また、<code>unsafe{}</code> と言うように、ブロックの手前に unsafe キーワードを付けることで、そのブロック内部でのポインター使用が可能になります。

<pre class="source" title="unsafe メソッド、unsafe ブロックの例" lang="">
<code><span class="reserved">unsafe void</span> UnsafeMethod()
{
  <span class="comment">// unsafe メソッド。</span>
  <span class="comment">// ポインターが使用可能。</span>
}

<span class="reserved">void</span> SafeMethod()
{
  <span class="comment">// ポインター使用不可。</span>

  <span class="reserved">unsafe</span>
  {
    <span class="comment">// unsafe ブロック。</span>
    <span class="comment">// ブロック内でのみポインター使用可能。</span>
  }
}
</code></pre>


さらに、プログラム内で unsafe キーワードを使用するためには、
<em>
        コンパイル時に <code>/unsafe</code> オプションを付ける必要があります
      </em>。
前節で述べたように、ポインターの使用は危険を伴うため、
C# ではこのような強い制限を設けています。

ちなみに、C# コンパイラーのオプションは `/unsafe` ですが、csproj ファイルに書くタグとしては `AllowUnsafeBlocks` という名前になっています。

<pre class="xsource" title="AllowUnsafeBlocks オプション">
<code><span class="attvalue">&lt;</span><span class="element">Project</span><span class="attvalue"> </span><span class="attribute">Sdk</span><span class="attvalue">=</span>&quot;<span class="attvalue">Microsoft.NET.Sdk</span>&quot;<span class="attvalue">&gt;</span>
 
<span class="attvalue">  &lt;</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">TargetFramework</span><span class="attvalue">&gt;</span>net5.0<span class="attvalue">&lt;/</span><span class="element">TargetFramework</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element"><em>AllowUnsafeBlocks</em></span><span class="attvalue">&gt;</span>true<span class="attvalue">&lt;/</span><span class="element">AllowUnsafeBlocks</span><span class="attvalue">&gt;</span>
<span class="attvalue">  &lt;/</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
 
<span class="attvalue">&lt;/</span><span class="element">Project</span><span class="attvalue">&gt;</span>
</code></pre>

## <a id="sec-generated-title-5"></a> <a id="managed-pointer"></a>補足: managed ポインター

「[参照渡しとポインター](../resource/sp_ref.md#pointer)」などでも触れていますが、
内部的には、[参照渡し](../resource/sp_ref.md#byref)はポインターと同じような処理です。
また、[参照型](../resource/oo_reference.md#reftype)の変数も、内部的にはポインターになっています。

ただし、以下のような差があります。

- 参照型変数や参照渡しは、以下の意味で .NET ランタイムの管理下にある
    - [ガベージ コレクション](../resource/rm_gc.md#garbage-collection)(GC)によってトラッキングされている
    - GC が誤動作を起こさないように、意図しない書き換えができないように厳しく制限されている
- (本項で説明する)ポインターは、.NET ランタイムに管理されていない代わりに自由な読み書きができる

この意味で、参照型変数や参照渡し(が内部で使っているポインター)を、managed ポインターと呼びます。
また、managed ポインターとの区別が必要な場面では、
本項のポインターのことを unmanaged ポインターと呼ぶこともあります。

C# では制限されていますが、[IL](../../il/index.md)のレベルでは実は制限が緩く、
managed ポインターと unmanaged ポインターを相互に変換できたりします。
「GCによるトラッキング」のも兼ねて、実際に変換を行うコードを示しましょう。
(このコードの実行には [Unsafe パッケージ](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)が必要です。)

<pre class="source" title="managed/unmanaged ポインターの強制変換の例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="comment">// C# の参照型が内部的にどうなっているか試してみるために、フィールド1個だけのクラスを用意。</span>
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="comment">// フィールドが1個だけなので、順序に悩む必要なし。</span>
    <span class="comment">// クラスの場合、フィールドが複数あるとき、並び順はコンパイラーが自由に変えていい仕様になってるので注意。</span>
    <span class="comment">// (StructLayout 属性を付けて制御はできる。)</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Value;
}

<span class="reserved">unsafe</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 参照型変数が指す先のヒープのアドレスを取得。</span>
    <span class="comment">// Unsafe クラスは、C# では絶対に書けない処理をやってくれる(中身は IL assebler 実装)。</span>
    <span class="comment">// C# の unsafe コード以上に unsafe なことができるやべーやつ。</span>
    <span class="comment">// IL は案外がばがばで、C# コンパイラーのレベルで安全性を保証してることが結構ある。</span>
    <span class="reserved">static</span> <span class="reserved">ulong</span> AsUnmanaged&lt;<span class="type">T</span>&gt;(<span class="type">T</span> r) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span> =&gt; (<span class="reserved">ulong</span>)<span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="type">IntPtr</span>&gt;(<span class="reserved">ref</span> r);

    <span class="comment">// 同上、ref が指す先のアドレスを取得。</span>
    <span class="reserved">static</span> <span class="reserved">ulong</span> AsUnmanaged&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> r) =&gt; (<span class="reserved">ulong</span>)<span class="type">Unsafe</span>.AsPointer(<span class="reserved">ref</span> r);
        
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// GC 誘発用に無駄オブジェクトを無駄に大量生成。</span>
        <span class="reserved">void</span> GenerageGarbage()
        {
            <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 1000000; i++) { <span class="reserved">var</span> dummy = <span class="reserved">new</span> <span class="reserved">object</span>(); }
        }

        GenerageGarbage();

        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">X</span> { Value = 12345678 };
        <span class="reserved">ref</span> var r = <span class="reserved">ref</span> x.Value;

        <span class="comment">// 通常ではない手段(Unsafe クラス)を使って、managed ポインターを無理やり unmanaged ポインター化。</span>
        <span class="reserved">var</span> addressOfX = AsUnmanaged(x);
        <span class="reserved">var</span> addressOfValue = AsUnmanaged(<span class="reserved">ref</span> r);

        WriteLine((addressOfX, addressOfValue));

        GenerageGarbage();
        <span class="type">GC</span>.Collect(0, <span class="type">GCCollectionMode</span>.Forced);
        WriteLine(<span class="string">"--- ここで GC 発生 ---"</span>);

        <span class="comment">// 無理やり数値化した方のアドレスまでは追えないので、当然、前のアドレスのまま。</span>
        <span class="comment">// もう無効なアドレスなので、ここに対して読み書きするとクラッシュ・セキュリティ ホールの原因になる。</span>
        WriteLine(<span class="string">"unmanaged "</span> + (addressOfX, addressOfValue));

        <span class="comment">// GC 発生後、アドレスが変わってる。</span>
        <span class="comment">// 大体は前に移動しているはずなので、値が小さくなってる。</span>
        WriteLine(<span class="string">"managed   "</span> + (AsUnmanaged(x), AsUnmanaged(<span class="reserved">ref</span> r)));

        <span class="reserved">fixed</span> (<span class="reserved">int</span>* p = &amp;x.Value)
        {
            <span class="comment">// fixed している間はどれだけゴミを出そうが x は移動しない。</span>
            GenerageGarbage();
            <span class="type">GC</span>.Collect(0, <span class="type">GCCollectionMode</span>.Forced);
            WriteLine(<span class="string">"--- ここで GC 発生(fixed) ---"</span>);

            <span class="comment">// fixe 直前と変わってないはず。</span>
            WriteLine(<span class="string">"managed   "</span> + (AsUnmanaged(x), AsUnmanaged(<span class="reserved">ref</span> r)));
        }
    }
}
</code></pre>

実行すると、一例ですが以下のようになります(数値は毎回変わります)。

<pre class="console">
<code>(2349487527640, 2349487527648)
--- ここで GC 発生 ---
unmanaged (2349487527640, 2349487527648)
managed   (2349484335496, 2349484335504)
--- ここで GC 発生(fixed) ---
managed   (2349484335496, 2349484335504)
</code></pre>

`AsUnmanaged`メソッドが変換処理に当たります。

このコードを実行すると、managed ポインターの値(アドレス)は、GCが発生する前後で変化しています。
これは、[コンパクション](../../computer/essential-software/memorymanagement.md#compaction)という処理が走ったせいで、
実際にオブジェクトが配置されている場所が変更されています。
.NETランタイムが、この変更に追従して変数の内容を書き換えています。

一方で、unmanaged ポインターは、GCの前後で変わりません。
GCにとってはあずかり知らぬ存在で、コンパクションの結果は反映されません。

これではまずいので、「unmanaged ポインターを使っている間はコンパクションでオブジェクトを移動させないでほしい」という制約を書けるのが、後述する [`fixed` ステートメント](#fixed)です。

## <a id="sec-generated-title-6"></a> <a id="function"></a>unsafe コード限定機能

unsafe コード内では以下の機能が利用可能となります。

* ポインターの使用。

* 配列の静的確保(stackalloc)。

* sizeof 演算子

* アドレス固定(fixed)。


詳しくは次節以降で説明していきます。


### <a id="sec-generated-title-7"></a> <a id="unmanaged-types"></a>アンマネージ型

ちなみに、
[ガベージ コレクション](../../computer/essential-software/memorymanagement.md#garbage-collection)の管理対象(managed)になっている型に対してunsafeなことをされると破滅的な結果を招くので、そういう型に対してはポインターなどの利用を制限する必要があります。

そこで、以下のような条件を満たす型をアンマネージ型(unmanaged type)と呼び、
ポインターなどを利用できるのはこの条件を満たす型のみに限定しています。

* 値型である
* 構造体の場合、再帰的に、アンマネージ型なメンバーしか含まない
* 非ジェネリック<sup>※</sup>

<sup>※</sup> この条件は過剰で、C# 7.3 (「[unmanaged制約](#unmanaged-constraints)」参照) と C# 8.0 ([アンマネージなジェネリック構造体](#unmanaged-generic-struct))で緩和されています。

## <a id="sec-generated-title-8"></a> <a id="pointer"></a>ポインター

C# では、C++ 言語と似た文法でポインターを使用できます。
すなわち、<code>&amp;</code> 演算子を用いてアドレスの取り出し、
<code>*</code> 演算子を用いてポインターの指している先を参照、
<code>+</code>, <code>--</code>, <code>++</code>, <code>--</code> などの演算子を使ってアドレスの値を計算できます。

ちなみに、`&` を アドレス取得式(address-of expression)、`*` を間接参照式(pointer indirection expression)と呼びます。

<pre class="source" title="ポインターの使用例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> UnsafeTest
{
  <span class="reserved">static void</span> Main()
  {
    <em><span class="reserved">unsafe</span></em>
    {
      <span class="reserved">int</span> n;
      <span class="reserved">int</span>* pn = &amp;n;        <span class="comment">// n のアドレスをポインター pn に代入。</span>
      <span class="reserved">byte</span>* p = (<span class="reserved">byte</span>*)pn; <span class="comment">// 違う型のポインターに無理やり代入可能。</span>

      *p = 0x78; <span class="comment">// n の最初の1バイト目に 0x78 を代入</span>
      ++p;
      *p = 0x56; <span class="comment">// n の2バイト目に 0x56 を代入</span>
      ++p;
      *p = 0x34; <span class="comment">// n の3バイト目に 0x34 を代入</span>
      ++p;
      *p = 0x12; <span class="comment">// n の4バイト目に 0x12 を代入</span>

      Console.Write(<span class="literal">"{0:x}\n"</span>, n); <span class="comment">// n の値を16進数で表示。</span>
    }
  }
}
</code></pre>
<pre class="console" title="">
12345678
</pre>

また、ポインターには `->` というポインター専用演算子と、配列と同じ `[]` によるアクセスが使えます。

| 演算子 | 説明 |
|---|---|
| `->` | ポインター メンバー アクセス(pointer member access)。`p->x` で、 `(*p).x` と同じ意味。 |
| `[]` | ポインター要素アクセス(pointer element access)。`p[i]` で `*(p + i)` と同じ意味。 |

例えば以下のように使います。

<pre class="source" title="ポインター用演算子">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">short</span> X;
    <span class="reserved">public</span> <span class="reserved">short</span> Y;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>();

        <span class="comment">// アンマネージ型の変数にはポインターを使える</span>
        <span class="comment">// &amp; でアドレス取得(ポインター化)</span>
        <span class="comment">// 型推論(var)も効く</span>
        <span class="reserved">var</span> pp = &amp;p;

        <span class="comment">// int 型のポインターに無理やり代入</span>
        <span class="comment">// p のある位置に無理やり int の値を書き込み</span>
        <span class="reserved">int</span>* pi = (<span class="reserved">int</span>*)pp;
        *pi = 0x00010002;

        <span class="comment">// -&gt; で構造体のポインターのメンバーにアクセス</span>
        <span class="type">Console</span>.WriteLine(pp-&gt;X); <span class="comment">// (*pp).X と同じ意味 = 2</span>
        <span class="type">Console</span>.WriteLine(pp-&gt;Y); <span class="comment">// (*pp).Y と同じ意味 = 1</span>

        <span class="comment">// byte 型のポインターに無理やり代入</span>
        <span class="reserved">byte</span>* pb = (<span class="reserved">byte</span>*)pp;

        <span class="comment">// ポインターには配列と同じように [] が使える</span>
        <span class="type">Console</span>.WriteLine(pb[0]); <span class="comment">// *(pb + 0) と同じ意味 = 2</span>
        <span class="type">Console</span>.WriteLine(pb[1]); <span class="comment">// *(pb + 1) と同じ意味 = 0</span>
        <span class="type">Console</span>.WriteLine(pb[2]); <span class="comment">// *(pb + 2) と同じ意味 = 1</span>
        <span class="type">Console</span>.WriteLine(pb[3]); <span class="comment">// *(pb + 3) と同じ意味 = 0</span>
    }
}
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="unsafe-using">unsafe 型に対する using エイリアス</a>

<h5 class="version version12">Ver. 12</h5>

C# 12 で [using エイリアスで使える型が増えました](../structured/sp_namespace.md#using-any-type)。
それに伴ってポインターや[関数ポインター](functionpointer.md)に対しても using エイリアスを書けるようになりました。

ただし、これらの型は unsafe な型なので、unsafe 修飾子を必要とします。
そのため、using ディレクティブにも unsafe 修飾を付けます(以下のように、using の後ろに unsafe を書きます)。

<pre class="source" title="unsafe 型に対する using エイリアス">
<span class="reserved">using</span> <span class="reserved">unsafe</span> <span class="type struct">Pointer</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">*</span>;
<span class="reserved">using</span> <span class="reserved">unsafe</span> <span class="type struct">FuncPointer</span> <span class="operator">=</span> <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">int</span>, <span class="reserved">void</span>&gt;;
</pre>

## <a id="sec-generated-title-10"></a> <a id="stackalloc"></a>スタック上への配列の確保(stackalloc)

C# で通常使用している配列はヒープ領域にメモリを確保しています（参考: 「[[雑記] スタックとヒープ](../resource/misc_heap.md)」 ）。
しかしながら、ヒープ領域への読み書きは、スタック領域と比べ、少しですが効率が悪くなります。
そのため、C# では unsafe コード内限定で、配列をスタック上に確保するための構文を用意しています。

スタック上への配列確保は以下に示すように、 <code>stackalloc</code> キーワードを用いて行います。

<pre class="source" title="stackalloc" lang="">
<code><span class="input">型名</span>* <span class="input">変数名</span> = <span class="reserved">stackalloc</span> <span class="input">型名</span>[<span class="input">配列長</span>];
</code></pre>


変数の型が <code>型名[]</code> から <code>型名*</code> に、
インスタンスの作成方法が <code>new 型名[配列長]</code> から <code>stackalloc 型名[配列長]</code> に代わっていますが、通常の配列と似たような構文で使用できます。


##### <a id="sec-generated-title-11"></a>サンプル

<pre class="source" title="stackalloc の例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> UnsafeTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">unsafe</span>
    {
      <span class="reserved">const int</span> N = 10;
      <span class="reserved">const int</span> MAX = 99;
      <span class="reserved">int</span>* x = <em><span class="reserved">stackalloc int</span>[N]</em>; <span class="comment">// 配列をスタック上に確保</span>
      Random rand = <span class="reserved">new</span> Random();

      <span class="comment">// 配列 x に乱数を代入</span>
      <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;N; ++i)
      {
        x[i] = rand.Next(MAX);
        Console.Write(<span class="literal">"{0}, "</span>, x[i]);
      }
      Console.Write('\n');

      <span class="comment">// 配列 x を整列</span>
      <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;N; ++i)
        <span class="reserved">for</span>(<span class="reserved">int</span> j=i+1; j&lt;N; ++j)
          <span class="reserved">if</span>(x[i] &gt; x[j])
          {
            <span class="reserved">int</span> tmp = x[i];
            x[i] = x[j];
            x[j] = tmp;
          }

      <span class="comment">// 整列結果を出力</span>
      <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;N; ++i)
      {
        Console.Write(<span class="literal">"{0}, "</span>, x[i]);
      }
      Console.Write('\n');
    }
  }
}
</code></pre>


<pre class="console" title="">
56, 67, 82, 23, 86, 78, 27, 92, 39, 13,
13, 23, 27, 39, 56, 67, 78, 82, 86, 92,
</pre>

### <a id="sec-generated-title-12"></a> <a id="safe-stackalloc"></a>安全な stackalloc

<h5 class="version version7">Ver. 7.2</h5>

C# 7.2から、[`Span<T>`構造体](../resource/span.md)を使うことで、
unsafe なしで `stackalloc` 演算子を使うことができるようになりました。
といっても、unsafe なしで危険なことができるわけではありません。
安全性は`Span<T>`構造体が保証してくれます。

<pre class="source" title="安全な stackalloc">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; s = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[5];
    }
}
</code></pre>

詳しくは、「[安全な stackalloc](../resource/span.md#safe-stackalloc)」で説明します。

### <a id="sec-generated-title-13"></a> <a id="stackalloc-initializer"></a>stackalloc 初期化子

<h5 class="version version7">Ver. 7.3</h5>

C# 7.3から、`stackalloc`に対して、配列と同じような初期化子を使えるようになりました。
配列同様、初期化子中の要素の型からの推論も効きます。

<pre class="source" title="">
<code><span class="comment">// 初期化子。{ } を使って初期値を与えられる。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x1 = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[3] { 0xEF, 0xBB, 0xBF };

<span class="comment">// 初期化子があるとき、サイズは省略可能。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x2 = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[] { 0xEF, 0xBB, 0xBF };

<span class="comment">// 初期化子から推論できるときは型名も省略可能。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x3 = <span class="reserved">stackalloc</span>[] { 0xEF, 0xBB, 0xBF };
</code></pre>

### <a id="sec-generated-title-14"></a> <a id="loop"></a>注意: ループ中の stackalloc

`stackalloc` で確保したスタック領域は、実は関数を抜けるまで解放されません。
例えば以下のようにループ中で `stackalloc` を使うと結構あっさり stack overflow (要はメモリ不足)を起こします。

<pre class="source" title="ループ中の stackalloc が原因で stack overflow">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 1000; <span class="variable">i</span>++)
        {
            <span class="comment">// ループの中でしか使ってないけど、実際に解放されるのは Main を抜けるタイミングだったり。</span>
            <span class="comment">// (確保は毎ループで起きる。)</span>
            <span class="comment">// ループを繰り返してるうちに stack overflow を起こす。</span>
            <span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_</span> = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[10000];
        }
    }
}
</code></pre>

特に、C# 8.0 では[式中の stackalloc](../resource/span.md#nested-stackalloc) が認められて気軽に書きやすくなったので注意が必要です。

解決方法ですが、関数を抜ければ解放されるので、以下のようにローカル関数を1個挟むだけでよかったりします。

<pre class="source" title="ループ中で stackalloc を使いたい場合は別関数(ローカル関数可)を挟む">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 1000; <span class="variable">i</span>++)
        {
            <span class="comment">// 別関数を挟めば大丈夫(ローカル関数でも可)</span>
            <span class="method">loopBody</span>();
 
            <span class="reserved">void</span> <span class="method">loopBody</span>()
            {
                <span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_</span> = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[10000];
            }
        }
    }
}
</code></pre>

## <a id="sec-generated-title-15"></a> <a id="sizeof"></a>sizeof 演算子

unsafe コード内では、sizeof 演算子で構造体の領域サイズを取得できます。
（通常（unsafe コードの外では）、sizeof 演算子でサイズを取得できるのは int や char など、C# の規格上サイズが決まっている数値型のみです。）

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">struct</span> <span class="type">X</span>
    {
        <span class="reserved">byte</span> x;
        <span class="reserved">int</span> y;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">unsafe</span>
        {
            <span class="type">Console</span>.WriteLine(<span class="reserved">sizeof</span>(<span class="type">X</span>));
        }
    }
}
</code></pre>


<pre class="console" title="">
8
</pre>


構造体のメンバーのレイアウトは、必ずしも隙間なく並ぶわけではなく、
メンバーのアドレスが4の倍数になるように隙間が作られたりします。
（32ビット CPU では、4バイト単位で値を読み書きするため、その方が実行効率がいい。
CPU の種類によって、最適な間隔は変わります。）
上記の例でも、1バイトの x と、4バイトの y の間に隙間が空いて、X 構造体全体では8バイトの領域を占めています。


## <a id="sec-generated-title-16"></a> <a id="fixed"></a>アドレス固定(fixed)

[前述の通り](#managed-pointer)、GC 管理下にあるオブジェクトは、GC 発生時に異動する可能性があります。
そして、GC 管理下にあるオブジェクトに対してポインターを使いたい場合には、
しばらくの間オブジェクトの移動を停止してもらう(<em>アドレスを固定する</em>)処理が必要になります。
そのための構文として、C# には `fixed` ステートメントというものがあります。
`fixed` ステートメントは以下のような形で書かれます。

<pre class="source" title="fixed ステートメント" lang="">
<code><span class="reserved">fixed</span>(<span class="input">型名</span>* <span class="input">変数名</span> = <span class="input">アドレス取得式</span>) <span class="input">実行したい文</span>
</code></pre>


`fixed` ステートメント中でアドレスを取得したオブジェクトは GC で移動されなくなり、
アドレスが変化しないことが保証されます。
例えば、参照型のメンバーのアドレスをポインターに代入する場合、以下のようにします。

<pre class="source" title="fixed ステートメントの例" lang="">
<code><span class="comment">// Complex クラスは re, im というdouble 型のメンバーを持っているものとする。</span>
<span class="type">Complex</span> c = new <span class="type">Complex</span>(1, 0);
<em><span class="reserved">fixed</span>(<span class="reserved">double</span>* p = &amp;c.re)</em>
{
  *p = 10;
}
<span class="type">Console</span>.Write(<span class="literal">"({0}, {1})\n"</span>, c.re, c.im); <span class="comment">// (10, 0) と表示される</span>
</code></pre>

ちなみに、C# では、配列と文字列に対して、`fixed`ステートメントを使うことで、
配列・文字列の先頭要素・文字のアドレスを取得することができます。

### <a id="sec-generated-title-17"></a> <a id="array"></a>配列

`fixed`ステートメント中で、
配列をポインターに暗黙的に変換することができます。

<pre class="source" title="ポインターを介して配列を操作" lang="">
<code><span class="reserved">int</span>[] array = <span class="reserved">new int</span>[10];
<span class="reserved">fixed</span> (<span class="reserved">int</span>* p = array)
{
}
</code></pre>

例えば以下のように、ポインター`px`を介して配列 `array` の内容を書き換えられます。

<pre class="source" title="fixed で配列をポインター越しに書き換える例">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> array = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };

        <span class="comment">// 配列 x をポインター px に代入する。</span>
        <span class="reserved">fixed</span> (<span class="reserved">int</span>* px = array)
        {
            <span class="comment">// ポインターを介して配列 x の内容を変更。</span>
            <span class="reserved">for</span> (<span class="reserved">int</span>* p = px; p != px + array.Length; ++p)
                *p = (*p) * (*p);
        }

        <span class="comment">// 結果出力。</span>
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; array.Length; ++i)
            System.<span class="type">Console</span>.Write(<span class="string">"{0} "</span>, array[i]);
        <span class="comment">// 1 4 9 16 25 と表示される。</span>
    }
}
</code></pre>

この場合、`&`演算子は必要ありません。
ほぼ `&array[0]` (先頭要素のアドレスの取得)と同じ意味ですが、1点だけ、空配列の時に以下のような差があります。

- `array`からの変換の場合、空配列を渡すと 0 (null ポインター)が得られる
- `&array[0]`の場合、空配列を渡すと IndexOutOfRange 例外が発生する

<pre class="source" title="空配列に対する fixed">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">int</span>[0]; <span class="comment">// 空っぽ</span>

        <span class="reserved">fixed</span> (<span class="reserved">int</span>* p = array) WriteLine((<span class="reserved">ulong</span>)p); <span class="comment">// 0</span>

        <span class="reserved">try</span>
        {
            <span class="comment">// この書き方だと今度は例外になる。</span>
            <span class="reserved">fixed</span> (<span class="reserved">int</span>* p = &amp;array[0]) WriteLine((<span class="reserved">ulong</span>)p);
        }
        <span class="reserved">catch</span> (<span class="type">IndexOutOfRangeException</span>)
        {
            WriteLine(<span class="string">"IndexOutOfRangeException"</span>);
        }
    }
}
</code></pre>

### <a id="sec-generated-title-18"></a> <a id="string"></a>文字列

配列と同様に、文字列に対しても `fixed` ステートメントが使えます。
この場合は先頭1文字の場所のアドレスが得られます。

<pre class="source" title="string に対する fixed の例">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">unsafe</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> s = <span class="string">"abcde"</span>;
        <span class="reserved">fixed</span> (<span class="reserved">char</span>* p = s)
        {
            <span class="comment">// 1行に1文字ずつ a b c d e が表示される</span>
            <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; s.Length; i++)
                WriteLine(s[i]);
        }

        <span class="comment">// ちなみに、string の場合は空文字列でも有効なアドレスが返ってくる</span>
        <span class="reserved">var</span> empty = <span class="string">""</span>;
        <span class="reserved">fixed</span> (<span class="reserved">char</span>* p = empty)
        {
            WriteLine((<span class="reserved">ulong</span>)p);  <span class="comment">// 非 0</span>
            WriteLine((<span class="reserved">int</span>)p[0]); <span class="comment">// 常に '\0' が入ってる</span>
        }
    }
}
</code></pre>

ちなみに、.NET の文字列は、内部的には以下のような構造になっています。

![string の内部](../../../../assets/media/1162/stringinternal.png)

先ほどの例で、空っぽの文字列を `fixed` でポインター化しても有効なアドレスが返ってくると言いましたが、
これは、文字列の末尾に常に 0 が入っていて、
その場所のアドレスを返しても安全だからです。
(C 言語で書かれたコードとの相互運用のために、常にこの無駄な 0 が入っています。)

また、見ての通り、文字列の本当の先頭と、1文字目(`a`)が入っている場所は、
実際には12バイトずれています。
文字列に対して`fixed`ステーメントを使うと、この12バイトを足す処理が C# コンパイラーによって追加されています。
(実際には、何バイトずれているかはOSなどの環境によって異なります。
[`OffsetToStringData`](https://msdn.microsoft.com/ja-jp/library/system.runtime.compilerservices.runtimehelpers.offsettostringdata.aspx)というプロパティから実際のバイト数が取れるので、このプロパティからの読み出しコードも追加されています。
)

#### <a id="sec-generated-title-19"></a> <a id="mutate-string"></a>文字列の書き換え

.NET の文字列(`string`)は、通常は書き換えできません。
しかし、unsafe を使ってポインター越しになら書き換えできてしまいます。

<pre class="source" title="ポインターを使った文字列の書き換え">
<code><span class="reserved">unsafe</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> s = <span class="string">"abcde"</span>;

        <span class="reserved">fixed</span>(<span class="reserved">char</span>* p = s)
        {
            <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
                p[i] = (<span class="reserved">char</span>)(i + <span class="string">'1'</span>);
        }

        System.<span class="type">Console</span>.WriteLine(s); <span class="comment">// 12345</span>
    }
}
</code></pre>

ほとんどの場合、文字列を書き換えるのはバグの原因にこそなれど何の利益もないんですが、
「桁数がわかっている数値を整形して文字列化したい」といったときなど、新規に文字列を作るときに利用価値があったりします。

### <a id="sec-generated-title-20"></a> <a id="custom-fixed"></a>ユーザー定義型での fixed ステートメント利用

<h5 class="version version7">Ver. 7.3</h5>

前述の通り、[配列](#array)と[文字列](#string)に対して`fixed`ステートメントを使うと、
ちょっと特殊な処理が掛かっています。

そして、その他にもいくつかの型で、同様の「`fixed`ステートメントでの特殊処理」をしたいことがあります。
例えば以下のようなものがあります。

- [`Span<T>`構造体](../resource/span.md) … 配列や文字列の一部分を指したりする型なので、配列や文字列と同様にポインターを使いたいことがある
- [`ImmutableArray<T>`構造体](https://source.dot.net/#System.Collections.Immutable/System/Collections/Immutable/ImmutableArray_1.cs,570249d040af2b99) … 内部的には配列。その内部の配列に対してポインター操作したいことがある

そこで、C# 7.3では、所定のパターンを満たす型に対して `fixed` ステートメントが使えるようになりました。
以下のように、`GetPinnableReference`という名前のメソッドを用意すれば使えます。

<pre class="source" title="ユーザー定義型に対する fixed ステートメント">
<code><span class="comment">// ただの配列のラッパー</span>
<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Array</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">T</span>[] _array;
    <span class="reserved">public</span> Array(<span class="reserved">int</span> length) =&gt; _array = <span class="reserved">new</span> <span class="type">T</span>[length];
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> _array[index];
    <span class="reserved">public</span> <span class="reserved">int</span> Length =&gt; _array.Length;

    <span class="comment">// このメソッドがあれば fixed ステートメントを使えるようになる</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> GetPinnableReference() =&gt; <span class="reserved">ref</span> _array[0];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">Array</span>&lt;<span class="reserved">int</span>&gt;(5);

        <span class="reserved">unsafe</span>
        {
            <span class="comment">// fixed (int* p = &amp;a.GetPinnableReference()) に展開される。</span>
            <span class="reserved">fixed</span> (<span class="reserved">int</span>* p = a)
            {
                <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
                    p[i] = i;
            }
        }

        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
            System.<span class="type">Console</span>.WriteLine(a[i]);
    }
}
</code></pre>

配列や文字列に対しても、C# コンパイラーが特殊処理するのではなく、
`GetPinnableReference`メソッドを用意して上記のパターンに展開したいという話もあります。

ちなみに、[配列の時の特殊処理](#array)、すなわち「空配列だったら 0 (null ポインター)を返す」と同じ結果にしたければ、`GetPinnableReference`を以下のように書く必要があります。
(現状、[`Unsafe`クラス](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)が必須です。)

<pre class="source" title="null ポインター相当の ref を返す方法">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="comment">// ただの配列のラッパー</span>
<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Array</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 中略</span>

    <span class="reserved">public</span> <span class="reserved">unsafe</span> <span class="reserved">ref</span> <span class="type">T</span> GetPinnableReference()
    {
        <span class="reserved">var</span> a = _array;
        <span class="reserved">if</span> (a.Length == 0) <span class="reserved">return</span> <span class="reserved">ref</span> <span class="type">Unsafe</span>.AsRef&lt;<span class="type">T</span>&gt;(<span class="reserved">null</span>);
        <span class="reserved">else</span> <span class="reserved">return</span> <span class="reserved">ref</span> a[0];
    }
}
</code></pre>

## <a id="sec-generated-title-21"></a> <a id="fixed-buffer"></a>固定長バッファ

<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 で、unsafe な構造体のメンバーとして、
C 言語の配列風の固定長バッファを定義できるようになりました。

固定長バッファは、以下のように、fixed キーワードを用いて定義します。
通常の C# の配列と異なり、型名[] ではなく、
変数名[要素数] と書きます。

<pre class="source" title="固定長バッファ" lang="">
<code>[System.Runtime.InteropServices.StructLayout(
  System.Runtime.InteropServices.LayoutKind.Sequential,
  Pack=1)]
<span class="reserved">unsafe struct</span> Header
{
  <span class="reserved">public</span> Int16 Source;
  <span class="reserved">public</span> Int16 Destination;
  <span class="reserved">public</span> Byte  Type;
  <em><span class="reserved">fixed byte</span> reserved[3];</em>
  <span class="reserved">public</span> Int32 Data;
}
</code></pre>


固定長配列は、主に unmanaged コードとの相互運用時に用いられます。
その名前の通り、バッファ長は固定で、実行時にサイズを決めたり、
サイズを変えたりすることはできません。


## <a id="sec-generated-title-22"></a> <a id="cppcli"></a>余談： C++/CLI

C# の unsafe の目的は、実行効率の向上と既存言語との相互運用性ですが、
この目的のためなら、C# で unsafe を使う以外に、
<strong id="cppcli" class="keyword">C++/CLI</strong> を使うという選択肢もあります。

C++/CLI は、C++ を .NET Framework 向けに修正したもので、
Microsoft の提供する .NET 言語の中で唯一、
native コードと managed コードを混在させてのプログラミングができる言語です。
C++/CLI を使うことで、既存の（native の） C++ 資源を .NET Framework から利用することも割と容易です。

native なコードは、通常の C++ と同じ構文で記述でき、
managed な部分に関しては、
ref、delegate、property などのいくつかの拡張キーワードや、
TypeName^ や TypeName^% などの追加の記号を使って記述します。

C# と比べるとお世辞にも書きやすいとは言えない言語ですが、
native / managed 混在プログラムを書きたい場合には最良の選択肢となるでしょう。

## <a id="sec-generated-title-23"></a> <a id="how-unsafe"></a>unsafeコードはどのくらいunsafeか

unsafeコードが名前通りunsafe(安全でない)なところを、一例出しておきます。

通常、C#の文字列(`string`型)は書き換えできません。
書き換えれないようなすることで、同じメモリ領域を複数の場所から参照しても安全に使えます(どこか別のあずかり知らぬところで書き換わってる心配がない)。

ですが、unsafeコードを使うと、文字列を書き換えれてしまいます。
例えば以下のようなコードが書けます。

<pre class="source" title="unsafeコードで文字列の書き換え">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// C# の string は書き換えできないはず</span>
        <span class="reserved">var</span> s1 = <span class="string">"-----"</span>;

        <span class="comment">// 参照型なので、同じインスタンスを見てる</span>
        <span class="comment">// 書き換えれないからこそ、インスタンスの共有が安全</span>
        <span class="reserved">var</span> s2 = s1;

        <span class="comment">// 実際には、C# の string は書き換えれる</span>
        <span class="reserved">unsafe</span>
        {
            <span class="reserved">fixed</span> (<span class="reserved">char</span>* c = s1)
            {
                c[2] = <span class="string">'X'</span>;
            }
        }

        <span class="type">Console</span>.WriteLine(s1); <span class="comment">// --X--</span>
        <span class="type">Console</span>.WriteLine(s2); <span class="comment">// 同じものを見てるので、こちらにも書き換えの影響が出てて --X--</span>
    }
}
</code></pre>

無制限にやられてしますと結構怖いコードです。
このように、unsafeコードの利用には注意が必要です。|

## <a id="sec-generated-title-24"></a> <a id="unmanaged-constraints"></a>unmanaged制約

<h5 class="version version7">Ver. 7.3</h5>

これまでに、ポインターなどの機能を使えるのは[アンマネージ型](#unmanaged-types)に限るという話をしました。
アンマネージというのは、「参照型など、[ガベージ コレクション](../resource/rm_gc.md#garbage-collection)のトラッキング対象になっている型を含まない」という意味です。
このことを保証するために、これまでは、「ジェネリックな型を使えない」という制限が掛かっていました。

しかし、C# 7.3では、`unmanaged`という型制約が増えて、
ジェネリック型引数に対してもポインターなどを使えるようになりました。

<pre class="source" title="">
<code><span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> MemSet0&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> T x)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved"><em>unmanaged</em></span>
{
    <span class="comment">// 今まではこの T* が許されなかった。</span>
    <span class="comment">// たとえ、Point みたいにポインター化可能な型で MemSet0&lt;Point&gt; を呼んだとしてもダメ。</span>
    <span class="comment">// unmanaged  制約のおかげで、ポインター化可能になった。</span>
    <span class="reserved">fixed</span> (T* p = &amp;x)
    {
        <span class="reserved">var</span> b = (<span class="reserved">byte</span>*)p;
        <span class="reserved">var</span> size = <span class="reserved">sizeof</span>(<span class="type">T</span>);
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; size; i++)
        {
            b[i] = 0;
        }
    }
}
</code></pre>

`Span<T>`構造体を使った安全な`stackalloc`でも同様に、`unmanaged`制約が有効です。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">SafeStackalloc</span>&lt;<span class="type">T</span>&gt;()
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">unmanaged</span>
{
    <span class="comment">// unmanaged 制約必須。</span>
    <span class="type">Span</span>&lt;<span class="type">T</span>&gt; span = <span class="reserved">stackalloc</span> <span class="type">T</span>[4];
}
</code></pre>

これらは、ちゃんと呼び出し側で制約のチェックが行われます。

<pre class="source" title="unmanaged 制約">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">SafeStackalloc</span>&lt;<span class="type">T</span>&gt;()
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">unmanaged</span>
    {
        <span class="type">Span</span>&lt;<span class="type">T</span>&gt; span = <span class="reserved">stackalloc</span> <span class="type">T</span>[4];
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// OK</span>
        <span class="method">SafeStackalloc</span>&lt;<span class="reserved">int</span>&gt;(); <span class="comment">// 値型</span>
        <span class="method">SafeStackalloc</span>&lt;<span class="type">DateTime</span>&gt;(); <span class="comment">// 値型だけを含む構造体</span>

        <span class="comment">// 以下はNG</span>
        <span class="error"><span class="method">SafeStackalloc</span>&lt;<span class="reserved">string</span>&gt;</span>(); <span class="comment">// 参照型</span>

        <span class="comment">// 残念なことに C# 7.3 以前ではジェネリックな型が NG (8.0 で改善)</span>
        <span class="error"><span class="method">SafeStackalloc</span>&lt;<span class="type">KeyValuePair</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt;</span>();
        <span class="error"><span class="method">SafeStackalloc</span>&lt;<span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;&gt;</span>();
    }

    <span class="reserved">struct</span> <span class="type">Wrap</span>&lt;<span class="type">T</span>&gt;
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">unmanaged</span>
    {
        <span class="reserved">public</span> <span class="type">T</span> Value;
    }
}
</code></pre>

1つ注意すべきは、ジェネリックな型を再帰的に追えるようになるのは C# 8.0 以降です。
C# 7.3 では `Wrap<int>` みたいな、`unmanaged` 制約を満たしているはずの型であってもアンマネージ型と認識できません。

## <a id="sec-generated-title-25"></a> <a id="unmanaged-generic-struct"></a>アンマネージなジェネリック構造体

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では、ジェネックな構造体に対して再帰的にアンマネージ型かどうかの判定するようになりました。
型引数全てがアンマネージであれば、その構造体もアンマネージ扱いを受けるようになります。

前節の例の末尾2行もコンパイルできるようになっています。

<pre class="source" title="unmanaged 制約の緩和">
<code>        <span class="comment">// C# 7.3 ではダメだったけど、8.0 では OK</span>
        <span class="method">SafeStackalloc</span>&lt;<span class="type">KeyValuePair</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt;();
        <span class="method">SafeStackalloc</span>&lt;<span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;&gt;();
</code></pre>

以下のように、ポインターも使えます。

<pre class="source" title="ジェネリックな構造体に対するポインター">
<code><span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">kv</span> = <span class="reserved">new</span> <span class="type">KeyValuePair</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(1, 2);
        <span class="type">KeyValuePair</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;* <span class="variable">pkv</span> = &amp;<span class="variable">kv</span>;
 
        <span class="reserved">var</span> <span class="variable">wi</span> = <span class="reserved">new</span> <span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt; { Value = 1 };
        <span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;* <span class="variable">pwi</span> = &amp;<span class="variable">wi</span>;
    }
 
    <span class="reserved">struct</span> <span class="type">Wrap</span>&lt;<span class="type">T</span>&gt;
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">unmanaged</span>
    {
        <span class="reserved">public</span> <span class="type">T</span> Value;
    }
}
</code></pre>

何段入れ子になっていても大丈夫です。
ちゃんと、すべてがアンマネージかどうかを調べてくれます。

<pre class="source" title="">
<code><span class="comment">// 何段入れ子になっていても大丈夫</span>
<span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">KeyValuePair</span>&lt;(<span class="reserved">float</span>, <span class="reserved">bool</span>), <span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;&gt;((1, <span class="reserved">true</span>), <span class="reserved">new</span> <span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;());
<span class="reserved">var</span> <span class="variable">px</span> = &amp;<span class="variable">x</span>;
 
<span class="comment">// ただし、その中に1つでもマネージな型(参照型)が含まれているとダメ</span>
<span class="reserved">var</span> <span class="variable">y</span> = <span class="reserved">new</span> <span class="type">KeyValuePair</span>&lt;(<span class="reserved">float</span>, <span class="reserved"><em>string</em></span>), <span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;&gt;((1, <span class="string">&quot;&quot;</span>), <span class="reserved">new</span> <span class="type">Wrap</span>&lt;<span class="reserved">int</span>&gt;());
<span class="reserved">var</span> <span class="variable">py</span> = <span class="error">&amp;<span class="variable">y</span></span>;
</code></pre>

## <a id="sec-generated-title-26"></a> <a id="skip-locals-init"></a>ローカル変数の0初期化抑止

<h5 class="version version9">Ver. 9.0</h5>

C# では通常、[未初期化](../resource/rm_default.md#uninitialized)のままの変数を読むことはできません。

<pre class="source" title="未初期化エラー">
<code><span class="reserved">using</span> System;
 
<span class="comment">// ローカル変数には初期化が必須。</span>
<span class="reserved">int</span> <span class="variable">x</span> = 1;
 
<span class="comment">// 初期化されていないものを読もうとするとコンパイル エラー。</span>
<span class="reserved">int</span> <span class="variable">y</span>;
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="error"><span class="variable">y</span></span>);
</code></pre>

ただ、[`stackalloc`](#stackalloc)を使った場合、その要素までは初期化が必須にはなりません。
この時、通常は、未初期化領域を参照してしまわないように、 .NET ランタイムが「規定値(0)で埋める」という処理を行っています。

<pre class="source" title="stackalloc の中身の0初期化">
<code><span class="reserved">using</span> System;
 
<span class="comment">// スタック上に4要素の int を確保。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[4];
 
<span class="comment">// 4要素すべて0で自動的に初期化されている状態になる。</span>
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; <span class="variable">span</span>.Length; <span class="variable">i</span>++)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">span</span>[<span class="variable">i</span>]); <span class="comment">// 0</span>
}
</code></pre>

本項で説明している unsafe 機能の目的は、「パフォーマンス優先で、安全性はプログラマーが頑張るからコンパイラーは余計なことをしないでくれ」というものです。
なので、この `stackalloc` に対する0初期化も「余計なコスト」になるので避けたいことがあります。
例えば、以下のように、プログラマーにとっては必ず上書きされることがわかってる
(かつ、何かミスがあったときに未初期化領域を参照してしまうのは「自己責任」と割り切れる)
場合、0初期化は無駄です。

<pre class="source" title="0初期化が「余計なお世話」な状況">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Text.Unicode;
 
<span class="method">m</span>(<span class="string">&quot;aあ</span><span style="color:#b776fb;">😀</span><span class="string">&quot;</span>);
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> <span class="variable">s</span>)
{
    <span class="comment">// UTF-16 の文字数に大して、UTF-8 のバイト数は最大でも3倍以内。</span>
    <span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">buffer</span> = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="variable">s</span>.Length * 3];
    <span class="type">Utf8</span>.<span class="method">FromUtf16</span>(<span class="variable">s</span>, <span class="variable">buffer</span>, <span class="reserved">out</span> <span class="reserved">_</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">bytesWritten</span>);
 
    <span class="comment">// FromUtf16 の仕様上、bytesWritten バイト目までは必ず上書きされる。</span>
    <span class="comment">// 上書きされた部分だけを使う分には0初期化は「余計なお世話」。</span>
    <span class="reserved">var</span> <span class="variable">written</span> = <span class="variable">buffer</span>[..<span class="variable">bytesWritten</span>];
 
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">b</span> <span class="control">in</span> <span class="variable">written</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">b</span>);
    }
}
</code></pre>

C# 9.0 で、unsafe 限定で、この0初期化をスキップできるようになりました。
メソッドに `SkipLocalsInit` 属性 (`System.Runtime.CompilerServices` 名前空間)を付けるだけです。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Text.Unicode;
 
<span class="method">m</span>(<span class="string">&quot;aあ</span><span style="color:#b776fb;">😀</span><span class="string">&quot;</span>);
 
[<span class="type">SkipLocalsInit</span>]
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> <span class="variable">s</span>)
{
    <span class="comment">// この buffer は中身が0初期化されない。</span>
    <span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">buffer</span> = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="variable">s</span>.Length * 3];
 
    <span class="comment">// 以下、先ほどと同じコードは省略。</span>
}
</code></pre>

ただ、本当に unsafe なので、`/unsafe` オプション(`AllowUnsafeBlock`)が必須です。
現状、ポインターや[ネイティブ相互運用](sp_pinvoke.md)を使わずに未初期化領域を参照できてしまう唯一の機能になります。
例えば、以下のようなコードを書くと「不定な値」が返ってきます。

<pre class="source" title="SkipLocalsInit で不定な値を取得">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="method">m</span>();
 
[<span class="type">SkipLocalsInit</span>]
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>()
{
    <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[4];
 
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">span</span>)
    {
        <span class="comment">// ここで何の値が表示されるかは未定義。</span>
        <span class="comment">// Debug ビルドだと0が返ってきたりするものの、Release ビルドだと毎回違う値が返ってきたり。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
    }
}
</code></pre>

ちなみに、`SkipLocalsInit` 属性はクラスやモジュールに付けることができて、
その場合、クラス内・モジュール内全部のメソッドの0初期化が抑止されます。

この0初期化をケチるほど安全性よりもパフォーマンスを優先することは低頻度で多くの開発者にとっては無縁ではありますが、パフォーマンス最優先な場面は全くないわけではありません。
例えば、 .NET の基本クラス ライブラリ自身がこの属性を使ってパフォーマンスを改善していたりします。
(C# 9.0 以前は、特殊なビルド処理を掛けることで同様の処理を無理やり実現していたようですが、
C# 9.0 (.NET 5.0) からはこの機能を使うようになったようです。)

## <a id="sec-generated-title-27"></a> <a id="function-pointer"></a>関数ポインター

<h5 class="version version9">Ver. 9</h5>

C# 9.0 で、関数ポインターも使えるようになりました。

詳しくは「[関数ポインター](functionpointer.md)」で説明します。

## <a id="sec-generated-title-28"></a> <a id="pointer-of-managed-types">マネージ型のポインター</a>

C# 11 から、マネージ型のポインターを使えるようになりました。
すなわち、
参照型 `T` や [ref 構造体](../resource/refstruct.md) `R` に対して、
`T*` や `R*` みたいなポインター型を書いたり、
それらの変数 `x` に対して `&x` でアドレス取得できるようになりました。

<pre class="source" title="マネージ型のポインター型/アドレス取得">
<span class="reserved">unsafe</span>
{
    <span class="reserved">string</span> <span class="variable">s</span> <span class="operator">=</span> <span class="string">&quot;&quot;</span>;
    <span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="number">4</span>];

    <span class="comment">// 以下のような型、アドレス取得はこれまではエラーになっていた。</span>
    <span class="comment">// (C# 11 以降も警告にはなる。多少の緩和があった。)</span>
    <span class="warning" title="CS8500"><span class="reserved">string</span><span class="operator">*</span></span> <span class="variable">ps</span> <span class="operator">=</span> <span class="warning" title="CS8500"><span class="operator">&amp;</span><span class="variable">s</span></span>;
    <span class="warning" title="CS8500"><span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt;<span class="operator">*</span></span> <span class="variable">px</span> <span class="operator">=</span> <span class="warning" title="CS8500"><span class="operator">&amp;</span><span class="variable">x</span></span>;
}
</pre>

C# 11以降でも警告になることからわかる通り、割かし危険度の高い機能です。
(コンパイラーチェックが効かないプログラマー裁量での)安全性を保つためにはかなりの注意を要するため、
あまり利用は推奨しません。

ただ、こういう「推奨はされないけども最終手段として使える回避策」が必要になることもまれにあります。
これまでのようにエラーになってしまうと、もっと醜悪でもっと危険な手段を取らないと最終手段の回避策すら取れません。
(例: [Unsafe クラスその1](../../../blog/2018/12/unsafe/index.md)、
[その2](../../../blog/2018/12/unsafegarantee/index.md)、
[その3](../../../blog/2018/12/unsafenogarantee/index.md))

それと比べれば、[`unsafe` コンテキスト](sp_unsafe.md)は C# 1.0 の頃から使われてきている安定の最終手段なので、まだマシといえます。
そのため、「気乗りはしないけどもしょうがなさげ」くらいのノリで、
「マネージ型のポインター」が認められることになりました。

ちなみに、[ref フィールド](../cheatsheet/ap_ver11.md#ref-field)がらみの作業をしている途中で必要になったようです。
(C# 11 のタイミングで重い腰を挙げて実装されたのはそのせい。)
