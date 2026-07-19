---
title: "関数ポインター"
source_url: "https://ufcpp.net/study/csharp/interop/functionpointer/"
content_type: "Article"
published_at: "2023-04-01T00:00:00"
updated_at: "2023-04-01T20:42:18"
tags: []
umbraco_id: 2461
parent_id: 1321
sort_order: 5
aliases:
  - "/csharp/interop/functionpointer/"
---

# 関数ポインター

##<a id="sec-generated-title-1"></a> <a id="abstract">概要</a>
<h5 class="version version9">Ver. 9</h5>

関数ポインターとは、メモリ上でメソッドなどの命令列が入ってるアドレスを指すポインターで、
「そのアドレスにジャンプすることでメソッド呼び出しが実現されている」みたいなものです。

.NET の内部的にはこれまでも関数ポインターがあったんですが、
それを C# から効率的に呼ぶ手段がありませんでした。
これに対して、C# 9 では `delegate*` という記法で関数ポインターを扱えるようになりました。
([unsafe コンテキスト](sp_unsafe.md#unsafe)内限定で使えます。)

##<a id="sec-generated-title-2"></a> <a id="since1.0">以前からある関数ポインター</a>
関数ポインター自体は .NET には昔からあって、
例えば、関数ポインターの値を `IntPtr` (`nint`) で取得する手段は .NET Framework 1.0 (初代。2002年リリース)の頃からありました。

ただ、関数ポインターを使ったメソッド呼び出しの側は、C# には関連機能が一切なく、
一度デリゲート化するひと手間が必要でした。

<pre class="source" title="GetFunctionPointer で関数ポインター取得">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="reserved">var</span> <span class="variable">m</span> <span class="operator">=</span> <span class="reserved">typeof</span>(<span class="type">A</span>)<span class="operator">.</span><span class="method">GetMethod</span>(<span class="string">&quot;M&quot;</span>)<span class="operator">!</span>;

<span class="comment">// GetFunctionPointer で、メソッド M の関数ポインターが取れる。</span>
<span class="reserved">nint</span> <span class="variable">ptr</span> <span class="operator">=</span> <span class="variable">m</span><span class="operator">.</span><span class="property">MethodHandle</span><span class="operator">.</span><span class="method">GetFunctionPointer</span>();

<span class="comment">// かつてはこれを直接呼ぶ手段はなくて、デリゲート化のひと手間が必要だった。</span>
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="type"><span class="static">Marshal</span></span><span class="operator">.</span><span class="method"><span class="static">GetDelegateForFunctionPointer</span></span>&lt;<span class="type">Action</span>&gt;(<span class="variable">ptr</span>);

<span class="comment">// これで A.M を間接的に呼べる。</span>
<span class="variable">a</span>();

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;A.M&quot;</span>);
}
</pre>

##<a id="sec-generated-title-3"></a> <a id="pinvoke">ネイティブ コード呼び出し</a>
まあ、C# で完結している分には役に立ちません。
C# で書いたメソッドを C# のデリゲートで受け取るんなら、
直接代入するだけでデリゲート化できます。
前節の例も、単に以下のように書けます。

<pre class="source" title="C# で完結している分には関数ポインターの出番なし">
<span class="comment">// C# で書いたメソッドを C# のデリゲートで受け取るんなら、単に代入でできるわけで、</span>
<span class="comment">// 関数ポインターを介する意味は全くなく。</span>
<span class="type">Action</span> <span class="variable">a</span> <span class="operator">=</span> <span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>;

<span class="comment">// これで A.M を間接的に呼べる。</span>
<span class="variable">a</span>();

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;A.M&quot;</span>);
}
</pre>

実際に関数ポインターを使う場面があるのは[ネイティブ コード呼び出し](sp_pinvoke.md)になります。

ネイティブ コード呼び出しも、`DllImport` 属性(.NET 7 以降であれば `LibraryImport` 属性)を使えば普通の、安全な C# コードだけで呼び出し可能ではあります。
例えば、`LibraryImport` 属性を使って kernel32.dll 中の `Beep` メソッドを呼ぶコードは以下のように書けます。

<pre class="source" title="LibraryImport 属性を使ったネイティブ コード呼び出しの例(ビープ音を鳴らす)">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="comment">// 呼び出し側。</span>
<span class="type">Native</span><span class="operator">.</span><span class="static"><span class="method">Beep</span></span>(<span class="number">440</span>, <span class="number">1000</span>);

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Native</span>
{
    <span class="comment">// こんな感じで属性を付けておけば、 .NET ランタイム内でなんかよろしくやってくれてネイティブ コードを呼べる。</span>
    [<span class="type">LibraryImport</span>(<span class="string">&quot;kernel32.dll&quot;</span>)]
    [<span class="reserved">return</span>: <span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span><span class="operator">.</span>Bool)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">bool</span> <span class="method"><span class="static">Beep</span></span>(<span class="reserved">uint</span> <span class="variable local">frequency</span>, <span class="reserved">uint</span> <span class="variable local">duration</span>);
}
</pre>

というか、かつてはネイティブ コードの関数ポインターを取る手段がありませんでした。
(上記の `Native.Beep` に対して `GetFunctionPointer` すると、
取れるのはあくまで「ネイティブ コード呼び出しを内部的によろしくやってくれる C# のメソッド」の関数ポインターになります。)

##<a id="sec-generated-title-4"></a> <a id="NativeLibrary">NativeLibrary クラス</a>
「関数ポインターを取る手段がないから使い道がない」と
「関数ポインターが指す先を呼び出す手段がないから取れてもしょうがない」で卵が先か鶏が先かみたいな話になるんですが、
C# に関数ポインターは必要ありませんでした。

ところが、 .NET Core 3.0 (C# 8.0 と同世代)で、[`NativeLibary`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.nativelibrary) (`System.Runtime.InteropServices` 名前空間)というクラスが入って、
ネイティブ コードの関数ポインターを取得する手段が提供されるようになりました。

<pre class="source" title="NativeLibrary を使ったネイティブ コード呼び出しの例(ビープ音を鳴らす)">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="comment">// DLL のロード。</span>
<span class="reserved">nint</span> <span class="variable">kernel32</span> <span class="operator">=</span> <span class="static"><span class="type">NativeLibrary</span></span><span class="operator">.</span><span class="static"><span class="method">Load</span></span>(<span class="string">&quot;kernel32.dll&quot;</span>);

<span class="comment">// 所望の関数の関数ポインターを取得。</span>
<span class="reserved">nint</span> <span class="variable">p</span> <span class="operator">=</span> <span class="type"><span class="static">NativeLibrary</span></span><span class="operator">.</span><span class="static"><span class="method">GetExport</span></span>(<span class="variable">kernel32</span>, <span class="string">&quot;Beep&quot;</span>);

<span class="comment">// ただ、C# 8.0 時点だと呼び出しには一度デリゲート化する必要あり。</span>
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="type"><span class="static">Marshal</span></span><span class="operator">.</span><span class="static"><span class="method">GetDelegateForFunctionPointer</span></span>&lt;<span class="type">BeepDelegate</span>&gt;(<span class="variable">p</span>);
<span class="variable">a</span>(<span class="number">440</span>, <span class="number">1000</span>);

<span class="comment">// ちなみに、 NativeLibrary の利点として、DLL のアンロードが可能。</span>
<span class="static"><span class="type">NativeLibrary</span></span><span class="operator">.</span><span class="static"><span class="method">Free</span></span>(<span class="variable">kernel32</span>);

<span class="comment">// GetDelegateForFunctionPointer にはジェネリックな型は渡せないらしく、</span>
<span class="comment">// Func&lt;uint, uint, int&gt; が使えないので同じ引数・戻り値のデリゲートを定義。</span>
<span class="reserved">delegate</span> <span class="reserved">int</span> <span class="type">BeepDelegate</span>(<span class="reserved">uint</span> <span class="variable local">frequencey</span>, <span class="reserved">uint</span> <span class="variable local">duration</span>);
</pre>

`NativeLibary` は、
`DllImport` や `LibararyImoprt` と比べると煩雑ではありますが、
動的にロード・アンロードしたりといった細やかな制御が可能です。

この例のように、C# 8.0 時点では一度デリゲート化する必要があります。
ただ、このデリゲートを介する部分がペナルティになって、
`DllImport` よりも低速になっていました。

##<a id="sec-generated-title-5"></a> <a id="function-pointer">関数ポインター構文</a>
<h5 class="version version9">Ver. 9</h5>

問題は `IntPtr` (`nint`)でポインターを取れても、
引数や戻り値に関する情報がなくなっていて、
どうやって引数を渡して、どうやって戻り値を受け取ればいいかがわからないことです。

`NativeLibary` クラスも入ったことだし、C# でも関数ポインターを扱える構文が欲しいということになり、C# 9 で実際に導入されることになりました。
記法としては `delegate*` を使います。
先ほどの `NativeLibrary` を使った `Beep` 呼び出しの例を関数ポインターで書き換えると以下のようになります。

<pre class="source" title="関数ポインター構文の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="comment">// 関数ポインターを nint で取得。</span>
<span class="reserved">nint</span> <span class="variable">kernel32</span> <span class="operator">=</span> <span class="type"><span class="static">NativeLibrary</span></span><span class="operator">.</span><span class="static"><span class="method">Load</span></span>(<span class="string">&quot;kernel32.dll&quot;</span>);
<span class="reserved">nint</span> <span class="variable">p</span> <span class="operator">=</span> <span class="type"><span class="static">NativeLibrary</span></span><span class="operator">.</span><span class="static"><span class="method">GetExport</span></span>(<span class="variable">kernel32</span>, <span class="string">&quot;Beep&quot;</span>);

<span class="reserved">unsafe</span>
{
    <span class="comment">// 「関数ポインター型」にキャストして使う。</span>
    <span class="comment">// 構文的には delegate* から初めて、 &lt;&gt; の中に引数を戻り値の型を並べる。</span>
    <span class="comment">// (戻り値の型が最後。Func&lt;&gt; 風。)</span>
    <span class="reserved">var</span> <span class="variable">fp</span> <span class="operator">=</span> (<span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">uint</span>, <span class="reserved">uint</span>, <span class="reserved">int</span>&gt;)<span class="variable">p</span>;
    <span class="variable">fp</span>(<span class="number">440</span>, <span class="number">1000</span>);
}
</pre>

`delegate*` から書き始めて、`<>` の中に引数と戻り値の型を並べます。

`<>` の中身は、最後の1個が必ず戻り値です。
`Func<>` と `Action<>` のように、戻り値の有無で型を分ける必要はなく、
「戻り値がない場合は最後の1個を `void` にする」という仕様です。

<pre class="source" title="戻り値がないときは void を書く">
<span class="reserved">unsafe</span>
{
    <span class="comment">// 引数 int, 戻り値 int</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">pf</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="method"><span class="static">f</span></span>;

    <span class="comment">// 引数 int, 戻り値なし(void)</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">int</span>, <span class="reserved">void</span>&gt; <span class="variable">pa</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="static"><span class="method">a</span></span>;
}

<span class="comment">// 同じようなコードでも、デリゲートだと Func/Action の分岐が必要。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">df</span> <span class="operator">=</span> <span class="static"><span class="method">f</span></span>;
<span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">da</span> <span class="operator">=</span> <span class="static"><span class="method">a</span></span>;

<span class="comment">// (こっちも普通に delegate&lt;int, void&gt; とか書きたい気持ちあるものの、現状、そういう仕様はない。)</span>

<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">f</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>;
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">a</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
</pre>

ちなみに、[IL](../../il/index.md) には .NET Framework 1.0 の頃から関数ポインターの仕様がちゃんとあって、「引数が `uint` 2つ、戻り値が `int`」みたいなのを指定して関数ポインターが指す先を呼び出す命令([`calli`](https://learn.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.calli))がありました。
あくまで C# 8 以前には `calli` を出力する能力がなかっただけです。

###<a id="sec-generated-title-6"></a> <a id="and-operator">& 演算子</a>
前節の例ですでに使っていますが、
C# で書いたメソッドに対して `&` 演算子を使えます。
`&` 演算子で、`GetFunctionPointer` などのリフレクション介さずにメソッドから直接関数ポインターを得ることができます。

<pre class="source" title="&amp; 演算子">
<span class="reserved">unsafe</span>
{
    <span class="comment">// &amp; で A.M の関数ポインターを取得。</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>;

    <span class="comment">// ちゃんと呼べる。</span>
    <span class="variable">p</span>();
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;A.M&quot;</span>);
}
</pre>

ただし、`&` 演算子で関数ポインターを取れるのは静的メソッドだけです。

<pre class="source" title="&amp; で関数ポインターを取れるのは静的メソッドのみ">
<span class="reserved">unsafe</span>
{
    <span class="comment">// 静的メソッドは OK。</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p1</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">Static</span></span>;

    <span class="comment">// インスタンス メソッドは A.Instance みたいな参照の仕方はできないし、</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p2</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="error" title="CS8759"><span class="type">A</span><span class="operator">.</span><span class="method">Instance</span></span>;

    <span class="comment">// デリゲートみたいに「インスタンス.メソッド」での参照も不可。</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p3</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="error" title="CS8759"><span class="reserved">new</span> <span class="type">A</span>()<span class="operator">.</span><span class="method">Instance</span></span>;

    <span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p4</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="error" title="CS8759"><span class="variable">a</span><span class="operator">.</span><span class="method">Instance</span></span>;
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>() { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Instance</span>() { }
}
</pre>

ちなみに、取れる値(関数ポインターが指すアドレス)自体は、`GetFunctionPointer` と同じになります。
ただし、`Type` 型や `MethodInfo` 型を介さなくていい分、`&` 演算子を使う方がパフォーマンスはいいそうです。

<pre class="source" title="GetFunctionPointer と同じ値">
<span class="reserved">var</span> <span class="variable">p1</span> <span class="operator">=</span> <span class="reserved">typeof</span>(<span class="type">A</span>)<span class="operator">.</span><span class="method">GetMethod</span>(<span class="string">&quot;M&quot;</span>)<span class="operator">!</span><span class="operator">.</span><span class="property">MethodHandle</span><span class="operator">.</span><span class="method">GetFunctionPointer</span>();
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">p1</span>);

<span class="reserved">unsafe</span>
{
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p2</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>;

    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>((<span class="reserved">nint</span>)<span class="variable">p2</span>); <span class="comment">// p1 と同じ値が取れる。</span>
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">p1</span> <span class="operator">==</span> (<span class="reserved">nint</span>)<span class="variable">p2</span>); <span class="comment">// true。</span>
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>() { }
}
</pre>

###<a id="sec-generated-title-7"></a> <a id="arguments">引数・戻り値の型</a>
`delegate*<T>` という、一見するとジェネリック型(`Func<T>` とか `Action<T>` とか)と似たような構文ですが、関数ポインターの `<>` の中に書ける型は、ジェネリック型引数よりもだいぶ制限が緩いです。
現状ではジェネリック型引数には書けない以下のような型も、関数ポインターの `<>` には普通に書けます。

* `ref T`, `out T`, `in T`
* ポインター型 `T*`
* `ref struct` な型
* `void`

<pre class="source" title="ジェネリック型引数よりもだいぶ緩い制約">
<span class="reserved">unsafe</span>
{
    <span class="comment">// in, out, ref が書ける</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">in</span> <span class="reserved">int</span>, <span class="reserved">out</span> <span class="reserved">int</span>, <span class="reserved">ref</span> <span class="reserved">string</span>&gt; <span class="variable">p1</span> <span class="operator">=</span> <span class="reserved">null</span>;

    <span class="comment">// ポインターが書ける</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">ref</span> <span class="reserved">int</span>, <span class="reserved">int</span><span class="operator">*</span>&gt; <span class="variable">p2</span> <span class="operator">=</span> <span class="reserved">null</span>;

    <span class="comment">// ref struct も書けるし、それのさらに ref も書ける</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;, <span class="reserved">ref</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;&gt; <span class="variable">p3</span> <span class="operator">=</span> <span class="reserved">null</span>;

    <span class="comment">// 前述のとおり、戻り値がないときは void</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt; <span class="variable">p4</span> <span class="operator">=</span> <span class="reserved">null</span>;
}
</pre>

関数ポインターの入れ子も可能です。

<pre class="source" title="">
<span class="reserved">unsafe</span>
{
    <span class="comment">// 入れ子</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">int</span>, <span class="reserved">void</span>&gt;, <span class="reserved">int</span>, <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">void</span>&gt;&gt; <span class="variable">p1</span> <span class="operator">=</span> <span class="reserved">null</span>;
}
</pre>

ちなみに、書ける型の制限が緩いので、Unsafe クラスですらできないことが関数ポインター使えば書けたり。

###<a id="sec-generated-title-8"></a> <a id="calling-convention">呼び出し規約</a>
複数のプログラミング言語をまたいでやり取りする場合、
呼び出し規約(calling convention)というものを気にする必要があります。

呼び出し規約は、引数や戻り値の受け渡しの仕方を呼ぶ側・呼ばれる側でそろえるためのルールです。
1つのプログラミング言語で完結している分にはコンパイラー任せで大丈夫ですが、
言語をまたぐときには明示が必要になります。
(「C# から Windows API を呼ぶ分にはデフォルトの規約が同じ」みたいな理由で省略可能なことはあります。)

`DllImport` では `CallingConvention` プロパティで、
`LibraryImport` では `UnmanagedCallConv` 属性で指定します。

<pre class="source" title="DllImport, LibraryImport での呼び出し規約の指定">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="type">LibraryImports</span><span class="operator">.</span><span class="method"><span class="static">Beep</span></span>(<span class="number">440</span>, <span class="number">1000</span>);
<span class="type">DllImports</span><span class="operator">.</span><span class="method"><span class="static">Beep</span></span>(<span class="number">440</span>, <span class="number">1000</span>);

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">LibraryImports</span>
{
    <span class="comment">// LibraryImport では UnmanagedCallConv 属性を付ける。</span>
    [<span class="type">LibraryImport</span>(<span class="string">&quot;kernel32.dll&quot;</span>)]
    [<span class="type">UnmanagedCallConv</span>(<span class="field">CallConvs</span>  <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="reserved">typeof</span>(<span class="type">CallConvCdecl</span>) })]
    [<span class="reserved">return</span>: <span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span><span class="operator">.</span>Bool)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">bool</span> <span class="method"><span class="static">Beep</span></span>(<span class="reserved">uint</span> <span class="variable local">frequency</span>, <span class="reserved">uint</span> <span class="variable local">duration</span>);
}

<span class="reserved">class</span> <span class="type">DllImports</span>
{
    <span class="comment">// DllImport では CallingConvention プロパティを指定する。</span>
    [<span class="type">DllImport</span>(<span class="string">&quot;kernel32.dll&quot;</span>, <span class="field">CallingConvention</span> <span class="operator">=</span> <span class="type">CallingConvention</span><span class="operator">.</span>Cdecl)]
    [<span class="reserved">return</span>: <span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span><span class="operator">.</span>Bool)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">bool</span> <span class="method"><span class="static">Beep</span></span>(<span class="reserved">uint</span> <span class="variable local">frequency</span>, <span class="reserved">uint</span> <span class="variable local">duration</span>);
}
</pre>

関数ポインターでは、`delegate*` と `<>` の間に、
`managed` もしくは `unmanaged[]` という修飾を付けます。

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="reserved">nint</span> <span class="variable">kernel32</span> <span class="operator">=</span> <span class="static"><span class="type">NativeLibrary</span></span><span class="operator">.</span><span class="static"><span class="method">Load</span></span>(<span class="string">&quot;kernel32.dll&quot;</span>);
<span class="reserved">nint</span> <span class="variable">p</span> <span class="operator">=</span> <span class="static"><span class="type">NativeLibrary</span></span><span class="operator">.</span><span class="method"><span class="static">GetExport</span></span>(<span class="variable">kernel32</span>, <span class="string">&quot;Beep&quot;</span>);

<span class="reserved">unsafe</span>
{
    <span class="comment">// 規約を省略。省略時のデフォルトは managed。</span>
    <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">p1</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="type">A</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>;

    <span class="comment">// managed 規約。C# で書いた普通のメソッドを呼ぶときに使う。</span>
    <span class="comment">// 要は「.NET ランタイム任せ」。</span>
    <span class="reserved">delegate</span><span class="operator">*</span> <span class="reserved">managed</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">p2</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>;

    <span class="comment">// unmanaged のみ指定。</span>
    <span class="comment">// 呼び出し規約はプラットフォーム依存で、</span>
    <span class="comment">// Windows では stdcall、他のプラットフォームでは cdecl になるっぽい。</span>
    <span class="reserved">var</span> <span class="variable">p3</span> <span class="operator">=</span> (<span class="reserved">delegate</span><span class="operator">*</span> <span class="reserved">unmanaged</span>&lt;<span class="reserved">uint</span>, <span class="reserved">uint</span>, <span class="reserved">int</span>&gt;)<span class="variable">p</span>;

    <span class="comment">// unmanaged[] で呼び出し規約を明示。</span>
    <span class="reserved">var</span> <span class="variable">p4</span> <span class="operator">=</span> (<span class="reserved">delegate</span><span class="operator">*</span> <span class="reserved">unmanaged</span>[Stdcall]&lt;<span class="reserved">uint</span>, <span class="reserved">uint</span>, <span class="reserved">int</span>&gt;)<span class="variable">p</span>;
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">y</span>;
}
</pre>
