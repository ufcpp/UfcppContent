---
title: "C# 13.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver13/"
content_type: "Article"
published_at: "2024-07-06T00:00:00"
updated_at: "2024-07-13T00:00:00"
tags: []
umbraco_id: 2499
parent_id: 1174
sort_order: 18
aliases:
  - "/csharp/cheatsheet/ap_ver13/"
---

# C# 13.0 の新機能

<div class="version version13">Ver. 13.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2024/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 9.0</li>
<li>Visual Studio 2022 17.12</li>
</td>
</tr>
</table>

執筆予定: [C# 13.0 トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/462)

##<a id="sec-generated-title-1"></a> <a id="params-collections">params コレクション</a>
[コレクション式](../datatype/collection-expression.md)で使える型であれば何でも `params` にできるようになりました。

<pre class="source" title="任意のコレクションに対して params を付ける例">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M1</span></span>(<span class="reserved">params</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M2</span></span>(<span class="reserved">params</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M3</span></span>(<span class="reserved">params</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M4</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }

<span class="method"><span class="static">M1</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="static"><span class="method">M2</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="method"><span class="static">M3</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="static"><span class="method">M4</span></span>(<span class="number">1</span>, <span class="number">2</span>);
</pre>

需要が高いのは `ReadOnlySpan` で、
`params T[]` を `params ReadOnlySpan<T>` に変更すればそれだけでパフォーマンスの改善が見込めます。

実際、 .NET 9 では、`string.Join` や `Task.WhenAll` などのメソッドに
`params ReadOnlySpan<T>` なオーバーロードが増えています。

<pre class="source" title="params ReadOnlySpan オーバーロードが増えている例">
<span class="comment">// .NET 8 以前なら Join(string, string[])</span>
<span class="comment">// .NET 9 以降なら Join(string, ReadOnlySpan&lt;string&gt;)</span>
<span class="reserved">var</span> <span class="variable">joiend</span> <span class="operator">=</span> <span class="reserved">string</span><span class="operator">.</span><span class="method"><span class="static">Join</span></span>(<span class="string">&quot;,&quot;</span>, <span class="string">&quot;a&quot;</span>, <span class="string">&quot;b&quot;</span>, <span class="string">&quot;c&quot;</span>);
</pre>

このため、自分で `params` を使わない場合でも、
「.NET 9 にアップグレードして再コンパイルするだけでアプリのパフォーマンスがちょっと改善する」という間接的なメリットがあります。

詳しくは「[`params` コレクション](../structured/sp_params.md#params-collections)」で説明しています。

##<a id="sec-generated-title-2"></a> <a id="partial-property">部分プロパティ</a>
プロパティとインデクサーも `partial` にできるようになりました。

例えば、C# 13 と同世代の .NET 9 では、[`GeneratedRegex`](https://learn.microsoft.com/ja-jp/dotnet/api/system.text.regularexpressions.generatedregexattribute) をプロパティにできるようになりました。

<pre class="source" title="GeneratedRegex をプロパティに付けれるようになった">
<span class="reserved">using</span> System<span class="operator">.</span>Text<span class="operator">.</span>RegularExpressions;

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">MyPatterns</span>
{
    [<span class="type">GeneratedRegex</span>(<span class="string">@&quot;\d{4}&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="type">Regex</span> <span class="property"><span class="static">FourDigits</span></span> { <span class="reserved">get</span>; } <span class="comment">// プロパティになった。</span>
}
</pre>

詳しくは「[部分プロパティ](../misc/partial-type.md#partial_property)」で説明します。

##<a id="sec-generated-title-3"></a> <a id="ref-struct-interface">ref 構造体のインターフェイス実装</a>
ref 構造体にインターフェイスを実装できるようになりました。
また、このインターフェイスのメンバーを呼び出すために、
ジェネリック型引数に ref 構造体を渡せるようにする仕組みとして `allows ref struct` アンチ制約が追加されました。

<pre class="source" title="allows ref struct なジェネリック メソッドを介して、ref 構造体のインターフェイス実装を呼ぶ">
<span class="type struct">S</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span>(); <span class="comment">// S は IFormattable を実装してる。</span>

<span class="comment">// これはボックス化を起こすから C# 13 でもエラーになる。</span>
<span class="type">IFormattable</span> <span class="variable">f</span> <span class="operator">=</span> <span class="variable"><span class="error" title="CS0029">x</span></span>;
<span class="variable">f</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X&quot;</span>, <span class="reserved">null</span>);

<span class="comment">// allows ref struct なジェネリックメソッドを介して、</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">f</span>) <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IFormattable</span>, <span class="reserved">allows</span> <span class="reserved">ref</span> <span class="reserved">struct</span>
    <span class="operator">=&gt;</span> <span class="variable local">f</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X&quot;</span>, <span class="reserved">null</span>);

<span class="comment">// こうやって IFormattable.ToString を呼べば大丈夫になった。</span>
<span class="method"><span class="static">M</span></span>(<span class="variable">x</span>);

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span> : <span class="type">IFormattable</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="method">ToString</span>(<span class="reserved">string</span><span class="operator">?</span> <span class="variable local">format</span>, <span class="type">IFormatProvider</span><span class="operator">?</span> <span class="variable local">formatProvider</span>) <span class="operator">=&gt;</span> <span class="string">&quot;&quot;</span>;
}
</pre>

詳しくは「[ref 構造体のインターフェイス実装](../resource/refstruct.md#ref-struct-interface)」で説明します。
また、「アンチ制約」という言葉については「[アンチ制約](../oop/sp2_generics.md#anti-constraint)」で説明しています。

##<a id="sec-generated-title-4"></a> <a id="overload-resolution-priority">OverloadResolutionPriority</a>
C# 13 で、オーバーロードの解決優先度を属性を付けて明示できる機能が入りました。

<pre class="source" title="オーバーロード解決の優先度を変更する例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// IEnumerable&lt;char&gt; の方が選ばれる。</span>
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M1</span></span>(<span class="string">&quot;&quot;</span>);
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M2</span></span>(<span class="string">&quot;&quot;</span>);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 通常、インターフェイスよりも具体的な型の方が優先。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M1</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) { }

    <span class="comment">// 属性を付けて優先度を上げる。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M1</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) { }

    <span class="comment">// 属性を付けて優先度を下げる。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M2</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M2</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

詳しくは「[オーバーロード解決](../structured/miscoverloadresolution.md#overload-resolution-priority)」で説明します。

トラッキングissue: [#478](https://github.com/ufcpp/UfcppSample/issues/478)

##<a id="sec-generated-title-5"></a> <a id="lock-class">Lock クラスに対する lock</a>
.NET 9 で `Lock` クラス(`System.Threading` 名前空間)という新しい lock 用の型が追加されたことに伴って、
`lock` ステートメントでこの `Lock` クラスを特別扱いするようになりました。
既存の `lock` (`Monitor.Enter` に展開される)と異なり、以下のようなコードに展開されます。

<pre class="source" title="lock (x) は using (x.EnterSceop()) になる">
<span class="reserved">var</span> <span class="variable">syncObject</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Lock</span>();

<span class="comment">// lock (syncObject)</span>
<span class="reserved">using</span> (<span class="variable">syncObject</span><span class="operator">.</span><span class="method">EnterScope</span>())
{
}
</pre>

詳しくは「[Lock クラス](../async/sp_thread.md#lock-class)」で説明しています。

##<a id="sec-generated-title-6"></a> <a id="ref-in-async">ref/unsafe をイテレーター/非同期メソッド中に書けるように</a>
[ref ローカル変数](../resource/sp_ref.md#ref-returns)、
[ref 構造体](../resource/refstruct.md)の変数、
[unsafe](../interop/sp_unsafe.md#unsafe) ブロックを、
[イテレーター](../data/sp2_iterator.md)と[非同期メソッド](../async/sp5_async.md)内で使えるようになりました。

イテレーターと非同期メソッドは内部の仕組み的に非常に似ているにも関わらず、
この2者で微妙に制限のかかり方が違ったんですが、
それも C# 13 でそろいました。

以下のコードで、行末コメントで ⭕ を付けている部分が C# 13 で新たにコンパイルできるようになったコードです。

<pre class="source" title="ref/unsafe をイテレーター/非同期メソッド中に書けるように">
<span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method">Enumerate</span>()
{
    <span class="reserved">unsafe</span> { } <span class="comment">// ⭕</span>

    <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;

    <span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">data</span> <span class="operator">=</span> [];

    <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;

    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>; <span class="comment">// ⭕</span>
}

<span class="reserved">async</span> <span class="type">Task</span> <span class="method">GetAsync</span>()
{
    <span class="reserved">unsafe</span> { }

    <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="static"><span class="method">Yield</span></span>();

    <span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">data</span> <span class="operator">=</span> []; <span class="comment">// ⭕</span>

    <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>();

    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>; <span class="comment">// ⭕</span>
}

<span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method">EnumerateAsync</span>()
{
    <span class="reserved">unsafe</span> { } <span class="comment">// ⭕</span>

    <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;

    <span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">data</span> <span class="operator">=</span> []; <span class="comment">// ⭕</span>

    <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;

    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>; <span class="comment">// ⭕</span>
}
</pre>

元々、原理的にはこう書いても問題ないことはわかっていたんですが、
正しく判定するのにコストがかかる割に、需要は低いだろうということでエラーにしていました。
C# 13 で書けるようになったのは、前述の[`Lock` クラスに対する `lock`](#lock-class) のついでだそうです。
(`Lock` クラスの `EnterScope` が ref 構造体を使っています。)

ただし、これは `yield` や `await` をまたがない場合に限って許されます。
例えば以下のコードは C# 13 でもコンパイル エラーを起こします。

<pre class="source" title="C# 13 でもエラーになる書き方の例">
<span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method">Enumerate</span>()
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>;
    <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;
    <span class="error" title="CS9217"><span class="variable">r</span></span> <span class="operator">=</span> <span class="number">456</span>;
}

<span class="reserved">async</span> <span class="type">Task</span> <span class="method">GetAsync</span>()
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>;
    <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="static"><span class="method">Yield</span></span>();
    <span class="error" title="CS9217"><span class="variable">r</span></span> <span class="operator">=</span> <span class="number">456</span>;
}
</pre>

##<a id="sec-generated-title-7"></a> <a id="escape-escape">\e (エスケープ文字のエスケープ シーケンス)</a>
文字・文字列リテラル中の[エスケープ シーケンス](../start/st_embeddedtype.md#escape-sequence)に `\e` (U+001B、エスケープ文字)が追加されました。

例えば、コンソール アプリで以下のように書くことで、文字列の色を変えたり装飾したりできます。

<pre class="source" title="\e の利用例">
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;\e[31mred text&quot;</span>);
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;\e[4munderlined text&quot;</span>);
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;\e[0mreset style&quot;</span>);
</pre>

![\e エスケープ シーケンス](../../../../assets/media/1217/escapeescape.png)

機能追加の背景などについてはブログ記事「[\e (エスケープ文字のエスケープ シーケンス)](../../../blog/2023/12/escape-escape/index.md)」で説明しています。


##<a id="sec-generated-title-8"></a> <a id="interceptor">インターセプター</a>
(書きかけ。予定地。)

トラッキングissue: [#456](https://github.com/ufcpp/UfcppSample/issues/456)

##<a id="sec-generated-title-9"></a> <a id="other">その他</a>
その他、ほぼバグ修正レベルの機能がいくつかあります。

###<a id="sec-generated-title-10"></a> <a id="index-in-object-initializer">オブジェクト初期化子中の ^ 演算子</a>
以下のように、オブジェクト初期化子中の `[]` の中で[インデックスの `^` 演算子](../data/dataranges.md)を使えるようになりました。

<pre class="source" title="">
<span class="comment">// これが C# 12 以前はコンパイル エラーを起こしてた。</span>
<span class="reserved">var</span> <span class="variable">c</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">C</span> { [<span class="operator">^</span><span class="number">1</span>] <span class="operator">=</span> <span class="number">1</span> };

<span class="comment">// これなら昔からコンパイルできる。</span>
<span class="comment">// (オブジェクト初期化子はこれと同じコードに展開されるはずなのに。)</span>
<span class="variable">c</span>[<span class="operator">^</span><span class="number">1</span>] <span class="operator">=</span> <span class="number">1</span>;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// インデクサーと Length さえ持っていれば c[^i] と書けるようになる。</span>
    <span class="comment">// c[c.Length - i] 扱い。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Length</span> <span class="operator">=&gt;</span> <span class="number">1</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">i</span>] { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="variable local">i</span>; <span class="reserved">set</span> { } }
}
</pre>

###<a id="sec-generated-title-11"></a> <a id="method-group-natrural-type">デリゲートの自然な型の改善</a>
[デリゲートの自然な型](../functional/sp_delegate.md#natural-type)の決定の際、
メソッド グループに対する型決定がちょっと賢くなったそうです。
同名のインスタンス メソッドと拡張メソッドがあるとき、インスタンス メソッドを優先的に見るようになりました。

例えば以下のようなクラスがあったとします。

<pre class="source" title="同名のインスタンス メソッドと拡張メソッド">
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() { } <span class="comment">// インスタンス メソッド M と、</span>
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">E</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type">C</span> <span class="variable local">c</span>, <span class="reserved">object</span> <span class="variable local">o</span>) { } <span class="comment">// 同名の拡張メソッド。</span>
}
</pre>

この `C` 型のインスタンス `x` に対して `x.M` と書いたとき、
C# 12 までは自然な型を決定できなかったのに対して、
C# 13 ではインスタンスメソッドを優先的に見ます。

<pre class="source" title="C# 13 の新ルール">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">C</span>();

<span class="comment">// オーバーロード解決ではインスタンスメソッド優先。</span>
<span class="variable">x</span><span class="operator">.</span><span class="method">M</span>();      <span class="comment">// C.M()</span>
<span class="variable">x</span><span class="operator">.</span><span class="method">M</span>(<span class="string">&quot;&quot;</span>); <span class="comment">// E.M(C, object)</span>

<span class="comment">// 型の明示があると昔から大丈夫だった。</span>
<span class="type">Action</span> <span class="variable">a</span> <span class="operator">=</span> <span class="variable">x</span><span class="operator">.</span><span class="method">M</span>;         <span class="comment">// C.M()</span>
<span class="type">Action</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">b</span> <span class="operator">=</span> <span class="variable">x</span><span class="operator">.</span><span class="method">M</span>; <span class="comment">// E.M(C, object)</span>

<span class="comment">// var を使う。</span>
<span class="comment">// これが C# 13 から行けるように。</span>
<span class="comment">// インスタンス メソッド優先で、Action 型になる。</span>
<span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="variable">x</span><span class="operator">.</span><span class="method">M</span>;
</pre>

###<a id="sec-generated-title-12"></a> <a id="collection-expression13">コレクション式の改善</a>
[コレクション式](ap_ver12.md#collection-expression)にも微妙な修正が2つ入っています。

1つは、`Add` メソッドが拡張メソッドでも大丈夫になりました。
(こちらは最新のコンパイラーにすると `LangVersion` 12 にしても元の挙動(= コンパイル エラー)にはなりません。)

<pre class="source" title="コレクション式も拡張メソッドの Add を見てくれるように">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="type">C</span> <span class="variable">c</span> <span class="operator">=</span> [<span class="string">'a'</span>];

<span class="reserved">class</span> <span class="type">C</span> : <span class="type">IEnumerable</span>
{
    <span class="reserved">public</span> <span class="type">IEnumerator</span> <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Extensions</span></span>
{
    <span class="comment">// C# 12 の頃はこの拡張メソッドを見てくれずエラーになっていた。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Add</span></span>(<span class="reserved">this</span> <span class="type">C</span> <span class="variable local">a</span>, <span class="reserved">char</span> <span class="variable local">_</span>) { }
}
</pre>

もう1つは、[params コレクション](#params-collections)との兼ね合いで、オーバーロード解決ルールが変わっています。
以下のように、要素の型違いのオーバーロードがあるとき、要素の[自然な型](../../../blog/2022/12/stackalloc-natural-type/index.md)を見るようになりました。
(この変更は言語バージョンを見て分岐しているようで、
最新のコンパイラーでも [`LangVersion`](langversionoption.md#langversion) を12以前に戻すと古い挙動になります。)

<pre class="source" title="要素の自然な型優先">
<span class="comment">// C# 12 では以下の2つとも解決不能(コンパイル エラー)になってた。</span>

<span class="comment">// C# 13 では int の方になる。</span>
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([<span class="number">1</span>]);

<span class="comment">// C# 13 では string の方になる。</span>
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>([<span class="string">$&quot;</span><span class="string">&quot;</span>]);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">List</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">_</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">List</span>&lt;<span class="reserved">string</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">List</span>&lt;<span class="type">IFormattable</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

ただ、この結果、ちょっとした破壊的変更も起きています。
C# 12 から C# 13 にアップデートすると、以下のような場合にオーバーロード解決先が変わります。

<pre class="source" title="コレクション式のオーバーロード解決の破壊的変更">
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>([<span class="number">1</span>, <span class="number">2</span>]);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// C# 12 だとこっちが呼ばれる。</span>
    <span class="comment">// (ReadOnlySpan 優先。)</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">data</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;ReadOnlySpan&lt;byte&gt;&quot;</span>);

    <span class="comment">// C# 13 だとこっちが呼ばれる。</span>
    <span class="comment">// (中身の自然な型(整数リテラルは int になる)優先。)</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">data</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Span&lt;int&gt;&quot;</span>);
}
</pre>
