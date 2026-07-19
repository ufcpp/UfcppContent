---
title: "C# 9.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver9/"
content_type: "Article"
published_at: "2020-05-09T00:00:00"
updated_at: "2021-05-02T00:00:00"
tags: []
umbraco_id: 2294
parent_id: 1174
sort_order: 14
aliases:
  - "/csharp/cheatsheet/ap_ver9/"
---

# C# 9.0 の新機能

<div class="version version9">Ver. 9.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2020/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 5.0</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>レコード型</li>
</ul>
</td>
</tr>
</table>

## <a id="sec-generated-title-1"></a> <a id="record"></a>レコード型

C# 9.0 で、レコード型(records)という新しい種類の型が追加されました。
record (記録)という名前通り、データの読み書きに使うことを意図した型です。
例えば以下のような書き方で、「`Name` という文字列と `Birthday` という日付」を読み書きできます。

<pre class="source" title="record の例">
<code><span class="reserved">using</span> System;
 
<span class="reserved">record</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable">Name</span>, <span class="type">DateTime</span> <span class="variable">birthday</span>);
</code></pre>

詳しくは「[レコード型](../datatype/record.md)」で説明します。

### <a id="sec-generated-title-2"></a> <a id="init-only"></a>init-only プロパティ

以下のように `init` という新しいアクセサーを使って、「オブジェクト初期化子までは書き換え可能で、それ以降は書き換えできないプロパティ」を作れるようになりました。

<pre class="source" title="オブジェクト初期化子でだけ書き換え可能">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
<span class="error"><span class="variable">p</span>.X</span> = 3; <span class="comment">// ダメ。</span>
 
<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved"><em>init</em></span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved"><em>init</em></span>; }
}
</code></pre>

`readonly` の制限が厳しすぎるので、問題ない範囲でちょっとだけ制限を緩めたもです。
(歴史的経緯で `init` という新キーワードが使われていますが、もし C# をフルスクラッチで作り直せるなら `readonly` が最初から `init` 相当の仕様になっていたと思います。)

詳しくは「[init-only プロパティ](../oop/oo_property.md#init-only)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="top-level-statements"></a>トップ レベル ステートメント

トップ レベル(top-leve: クラスや名前空間よりも外側、ファイル直下)に[ステートメント](../start/st_variable.md#statement)を直接書けるようになりました。

例えばよくある「Hello World」であれば、単に以下のように書けるようになります。

<pre class="source" title="トップ レベルに直接「Hello World」">
<code><span class="reserved">using</span> System;
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
</code></pre>

詳しくは「[トップ レベル ステートメント](../misc/miscentrypoint.md#top-level-statements)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="pattern-v3"></a>パターンの追加

[C# 7.0](ap_ver7.md)から脈々と改善されてきた[パターン マッチング](../datatype/patterns.md)ですが、
C# 9.0 でもいくつかのパターンが追加されています。

<pre class="source" title="C# 9.0 でのパターン追加">
<code><span class="comment">// not, and, or や、 &lt;, &lt;=, &gt;, &gt;= などのパターンが増えた。</span>
<span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">uint</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    0 <span class="reserved">or</span> 2 <span class="reserved">or</span> 4 <span class="reserved">or</span> 6 <span class="reserved">or</span> 8 =&gt; 0,
    1 <span class="reserved">or</span> 3 <span class="reserved">or</span> 5 <span class="reserved">or</span> 7 <span class="reserved">or</span> 9 =&gt; 1,
    &gt;= 10 =&gt; -1,
};
</code></pre>

3世代かけてようやく当初予定(C# に追加すること自体は最初から決めていた機能)が全て入りました。
当初から予定に入ってたのは、既存のいくつかのプログラミング言語が同様の文法を持っていて、
[網羅性](../datatype/patterns.md#exhaustive)や[重複](../datatype/patterns.md#case-duplicate)の検査を含めて実装手段が既知で、検討コストが低いからです。
それでも、需要が高いものから少しずつ実装した結果、3世代に分かれました。
3世代目なことを指して「パターン v3」(patterns v3)という俗称があったりもします。

詳しくは「[パターン マッチング](../datatype/patterns.md)」で説明します。
C# 9.0 で追加されているのは以下の3つです。

- [型パターンの簡単化](../datatype/patterns.md#simplified-type-pattern)
- [パターンの組み合わせ](../datatype/patterns.md#pattern-combintor)
- [関係演算パターン](../datatype/patterns.md#relational-patterns)

## <a id="sec-generated-title-5"></a> <a id="target-typed-inference"></a>ターゲット型推論の強化

### <a id="sec-generated-title-6"></a> <a id="target-typed-new"></a>ターゲットからの new 型推論

ターゲット型からの推論が効く場合に、`new T()` の `T` の部分を省略できるようになりました。
(target-typed new とか呼ばれたりします。)

特に、[`var`](../start/sp3_inference.md#type-inference) が使えず、
型名が長い特に便利です。

<pre class="source" title="フィールド初期化子で特に便利">
<code><span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="comment">// フィールドに対しては var が使えない。
    // 代わりに new 型推論を使うと便利なことがある(特に、型名が長い時)。</span>
    <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="type">List</span>&lt;(<span class="reserved">int</span> x, <span class="reserved">int</span> y)&gt;&gt; _cache = <span class="reserved">new</span>();
}
</code></pre>

詳しくは「[ターゲットからの new 型推論](../oop/oo_construct.md#target-typed-new)」で説明します。

### <a id="sec-generated-title-7"></a> <a id="target-typed-conditional"></a>条件演算子のターゲット型推論

条件演算子の第2・第3項がターゲット型からの型推論するようになりました。


<pre class="source" title="">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="comment">// int? を明示するとコンパイルできる(var だとダメ)。</span>
    <span class="reserved">int</span>? <span class="variable">i</span> = <span class="variable">b</span> ? 1 : <span class="reserved">null</span>;
 
    <span class="comment">// Base を明示するとコンパイルできる(var だとダメ)</span>
    <span class="type">Base</span> <span class="variable">c</span> = <span class="variable">b</span> ? <span class="reserved">new</span> <span class="type">A</span>() : <span class="reserved">new</span> <span class="type">B</span>();
}
 
<span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Base</span> { }
</code></pre>

詳しくは「[条件演算子のターゲット型推論](../structured/st_branch.md#terget-typed-conditional)」で説明します。
「[型の決定](../start/misctyperesolution.md)」も参考にしてください。

## <a id="sec-generated-title-8"></a> <a id="class-covariant-returns"></a>クラスの共変戻り値

仮想メソッドの戻り値に共変性が認められるようになりました。
(機能名の俗称としては、「クラスの共変戻り値」と言ったりします。)

例えば以下のようなコードを書けるようになります。

<pre class="source" title="仮想メソッド戻り値の共変性">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">Base</span> <span class="method">Clone</span>() =&gt; <span class="reserved">new</span> <span class="type">Base</span>();
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// これの戻り値が Base じゃなくてもよくなった。</span>
    <span class="comment">// Derived は常に Base に安全に変換可能なので、 Base Clone() の override として Derived Clone() を使える。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="type">Derived</span> <span class="method">Clone</span>() =&gt; <span class="reserved">new</span> <span class="type">Derived</span>();
}
</code></pre>

詳しくは「[多態性/戻り値の共変性](../oop/oo_polymorphism.md#covariance)」で説明します。

## <a id="sec-generated-title-9"></a> <a id="unsafe"></a>unsafe/ネイティブ相互運用向け機能

[C# 7.2](ap_ver7_2.md)の辺りから、
言語の方向性として生産性や安全性を優先する C# でも、
パフォーマンス改善を目的とするような言語機能が結構増えてきました。

また、 クロスプラットフォーム化が進んだことで、ネイティブ相互運用関連の機能も増えています。

この手の機能は一般的な開発者が直接触れることは少ないですが、
.NET ランタイム自体や、大規模に使われているライブラリのパフォーマンス改善につながり、
間接的にすべての C# 開発者が恩恵を受けるものになります。

### <a id="sec-generated-title-10"></a> <a id="skip-locals-init"></a>ローカル変数の0初期化抑止

`/unsafe` オプション指定時限定ですが、ローカル変数の0初期化を抑止できるようになりました。

<pre class="source" title="SkipLocalsInit 属性で0初期化抑止">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Text.Unicode;
 
<span class="method">m</span>(<span class="string">&quot;aあ</span><span style="color:#b776fb;">😀</span><span class="string">&quot;</span>);
 
<span class="comment">// この属性を付けると stackalloc の要素の0初期化がなくなる。</span>
[<span class="type">SkipLocalsInit</span>]
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

詳しくは「[ローカル変数の0初期化抑止](../interop/sp_unsafe.md#skip-locals-init)」で説明します。

### <a id="sec-generated-title-11"></a> <a id="function-pointer"></a>関数ポインター

C# で関数ポインターを書けるようになりました。

.NET の内部的にはこれまでも関数ポインターがあったんですが、 それを C# から効率的に呼ぶ手段がありませんでした。 これに対して、C# 9 では delegate* という記法で関数ポインターを扱えるようになりました。

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

詳しくは「[関数ポインター](../interop/functionpointer.md)」で説明します。

## <a id="sec-generated-title-12"></a> <a id="nint"></a>native int

`nint` と `nuint` というキーワードで、「CPU 依存の一番高速に扱える整数」が使えるようになりました。
`nint` が符号付、`nuint` が符号なしです。

<pre class="source" title="CPU 依存幅整数">
<code><span class="reserved">nint</span> <span class="variable">x</span> = 0x1_0000;
<span class="variable">x</span> = <span class="variable">x</span> * <span class="variable">x</span>;

<span class="comment">// 32ビット CPU だと 0、64ビット CPU だと 100000000 になるはず。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">x</span>:<span class="string">X</span>}<span class="string">&quot;</span>);

<span class="reserved">unsafe</span>
{
    <span class="comment">// 32ビット CPU だと 4、64ビット CPU だと 8 になるはず。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">sizeof</span>(<span class="reserved">nint</span>));
}
</code></pre>

ちなみに、内部的には `IntPtr`、`UIntPtr` (いずれも `System` 名前空間)にコンパイルされています。
そのため、以下のようなコードはコンパイル エラーになります(引数の型が同じ同名のメソッドが2つあるため)。

<pre class="source" title="IntPtr と nint でのオーバーロードはできない">
<code><span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">void</span> <span class="method">M</span>(<span class="type">IntPtr</span> <span class="variable">x</span>) { }
    <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">nint</span> <span class="variable">x</span>) { }
}
</code></pre>

ただ、単純に 「`IntPtr`、`UIntPtr` に別名が付いた」というわけではなく、`+` などの演算子の挙動が違います。
(※ C# 10 までの話。 [C# 11](ap_ver11.md#numeric-intptr) 以降は「`nint`、`nuint` は `IntPtr`、`UIntPtr` の別名 」という扱いになりました。)

`IntPtr` の場合は `operator +(IntPtr pointer, int offset)` しか持っていませんが、
`nint` の場合は普通に整数としての四則演算が全て行えます。 
ちなみに、コンパイラーが `IntPtr` と `nint` を区別するため、`nint` だった場合は `NativeInteger` 属性(`System.Runtime.CompilerServices` 名前空間)が付きます。

今更こんな機能が入った背景には、パフォーマンス改善やネイティブ相互運用の強化があります。
例えば、以下のような場面で `nint` を使っています。

* [ネイティブ コード側が CPU 依存幅の整数になっている場合の相互運用](https://github.com/dotnet/runtime/blob/7984b32774916c98ab7c85c244c9e40581e4cdf5/src/libraries/Common/src/Interop/OSX/Interop.libobjc.cs#L11-L17)
* [配列のインデックス アクセスは `nint` を使った方が速い](https://github.com/dotnet/runtime/blob/4017327955f1d8ddc43980eb1848c52fbb131dfc/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Char.cs#L30) (C++ でいう `size_t` な処理)

## <a id="sec-generated-title-13"></a> <a id="other"></a>その他

### <a id="sec-generated-title-14"></a> <a id="nrt"></a>null 許容参照型の改善

C# 8.0 で入った [null 許容参照型](../resource/nullablereferencetype.md)に対してちょっと改善が入っています。
主に以下の2点です。

- [制約なしジェネリック型引数に `?` を付けれるようになった](../resource/nullablereferencetype.md#unconstrained-generics)
- [アノテーション属性](../resource/nullablereferencetype.md#annotation-attributes)に `MemberNotNull` と `MemberNotNullWhen` が増えた

### <a id="sec-generated-title-15"></a> <a id="lambda-discard"></a>ラムダ式の引数を破棄

ラムダ式の引数で、`_` を使った値の破棄ができるようになりました。

<pre class="source" title="ラムダ式の引数で _ を破棄扱い">
<code><span class="reserved">static</span> <span class="reserved">void</span> Subscribe(<span class="type">INotifyPropertyChanged</span> source)
{
    <span class="comment">// _ を破棄扱いして、2個以上並べられる</span>
    source.PropertyChanged += (<span class="reserved">_</span>, <span class="reserved">_</span>) =&gt; { };
}
</code></pre>

詳細は「[値の破棄 - ラムダ式の引数](../datatype/declarationexpressions.md#lambda-discard)」で説明します。

### <a id="sec-generated-title-16"></a> <a id="static-anonymous-function"></a>静的匿名関数

匿名関数に対しても `static` 修飾子を付けれるようになりました。
「外部の変数を捕獲しない」という意味になります。

<pre class="source" title="静的匿名関数">
<code><span class="reserved">using</span> System;
 
<span class="reserved">int</span> <span class="variable">a</span> = 0;
 
<span class="comment">// 以下の行は自身の引数しか使っていないので static にしても怒られない。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">ok</span> = <span class="reserved"><em>static</em></span> <span class="variable">x</span> =&gt; <span class="variable">x</span> * <span class="variable">x</span>;
 
<span class="comment">// 以下の行は外側のローカル変数 a を使ってしまったのでコンパイル エラー。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">ng</span> = <span class="reserved"><em>static</em></span> <span class="variable">x</span> =&gt; <span class="variable"><span class="error">a</span></span> * <span class="variable">x</span>;
</code></pre>

詳しくは「[静的匿名関数](../functional/fun_localfunctions.md#static-local-function)」で説明します。

### <a id="sec-generated-title-17"></a> <a id="local-function-attribute"></a>ローカル関数への属性適用

[ローカル関数](../functional/fun_localfunctions.md#local-function)に属性を付けられるようになりました。

<pre class="source" title="ローカル関数に属性を付ける">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Diagnostics.CodeAnalysis;
 
<span class="method">m</span>(<span class="string">&quot;&quot;</span>, <span class="string">&quot;&quot;</span>);
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span>? <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>)
{
    <span class="comment">// C# 9.0 からローカル関数に属性を付けれる。</span>
    <span class="comment">// C# 8.0 の null 許容参照型がらみで特に有用。</span>
    [<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="string">&quot;s&quot;</span>)]
    <span class="reserved">string</span>? <span class="method">toLower</span>(<span class="reserved">string</span>? <span class="variable">s</span>) =&gt; <span class="variable">s</span>?.<span class="method">ToLower</span>();
 
    <span class="control">if</span> (<span class="variable">a</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span> &amp;&amp; <span class="variable">b</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
    {
        <span class="comment">// a, b の null 許容性が、NotNullIfNotNull 属性のおかげで al, bl に伝搬。</span>
        <span class="reserved">string</span> <span class="variable">al</span> = <span class="method">toLower</span>(<span class="variable">a</span>);
        <span class="reserved">string</span> <span class="variable">bl</span> = <span class="method">toLower</span>(<span class="variable">b</span>);
 
        <span class="comment">// a, b が非 null なので、al, bl は非 null で確定済み。改めてのチェック不要。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">al</span>.<span class="method">GetHashCode</span>());
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">bl</span>.<span class="method">GetHashCode</span>());
    }
}
</code></pre>

### <a id="sec-generated-title-18"></a> <a id="extension-getenumerator"></a>拡張メソッドでの GetEnumerator 実装

[パターン ベース](../misc/miscpatternbased.md#index)な [`foreach`](../data/sp_foreach.md#extension-getenumerator)、[`await foreach`](../async/asyncstream.md#await-foreach)で、拡張メソッドによる実装ができるようになりました。

## <a id="sec-generated-title-19"></a> <a id="source-generator"></a>ソースコード生成

正確には C# という言語の機能ではなく、「C# 9.0 と同時期に実装された」というだけですが、
C# 9.0 世代の C# コンパイラーにはソースコード生成(source generator)プラグインの作成機能が追加されました。
詳細は「[コード解析とコード生成](../misc/analyzer-generator.md)」で説明しています。

これと同時に、ソースコード生成を前提とした文法もいくつか実装されました。

### <a id="sec-generated-title-20"></a> <a id="extended_partial_method"></a>部分メソッドの拡張

[ソースコード生成](../misc/analyzer-generator.md)では、手書きでは不完全な C# コードを書いて、
それをソースコード生成で埋めてもらうという状況があり得ます。
C# 9.0 ではそのための文法として、[`partial` キーワード](../oop/oo_class.md#partial_method)を再利用することにしました。

<pre class="source" title="ソースコード生成で埋めてもらう前提の不完全なメソッドの例">
<code><span class="comment">// (1) 手書き前提のコード</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        System.<span class="type">Console</span>.<span class="method">WriteLine</span>(
            <span class="string">&quot;PreGeneratedMethod が呼ばれた直後&quot;</span>
            + <span class="method">WantSourceGenerated</span>());
    }
 
    <span class="comment">// C# コードが先にあって、これを元にソースコード生成してほしいメソッド。</span>
    <span class="reserved">private</span> <span class="reserved">partial</span> <span class="reserved">string</span> <span class="method">WantSourceGenerated</span>();
}
 
<span class="comment">// (2) C# からのソースコード生成が前提のコード</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">private</span> <span class="reserved">partial</span> <span class="reserved">string</span> <span class="method">WantSourceGenerated</span>() =&gt; <span class="string">&quot;手書きはしづらしくて、ソースコード生成なら楽な文字列&quot;</span>;
}
</code></pre>

C# 2.0 の頃からある部分メソッドとの差は[アクセシビリティ](../oop/oo_conceal.md#level)修飾子の有無です。
`private` などを付けるかどうかで「コード生成が先」か「手書きが先」かの用途が逆になります。

詳しくは「[部分メソッドの拡張](../oop/oo_class.md#extended_partial_method)」で説明します。

### <a id="sec-generated-title-21"></a> <a id="module-initializer"></a>モジュール初期化子

プログラムの実行時、最初に1回だけ呼び出したい処理が必要になることがあります。
「[静的コンストラクター](../oop/oo_static.md#ctor)」で説明しているように、この静的コンストラクターという機能を使っても「最初に1回だけ呼ばれる」ということができますが、C# 9.0 ではモジュール初期化子という書き方もできるようになりました。

以下のように、`ModuleInitilizer` 属性(`System.Runtime.CompilerServices` 名前空間)を付けた[静的メソッド](../oop/oo_static.md#stmethod)を書くと、それが必ず1回呼び出されるようになります。

<pre class="source" title="ModuleInitialize 属性">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;必ず1回だけ呼ばれる&quot;</span>);
    }
}
</code></pre>

これをモジュール初期化子(module initializer)と呼びます。

ソースコード生成と組み合わせて、これまでなら[リフレクション](../dynamic/sp_reflection.md)に頼らざるを得なかったような処理を、コンパイル時コード生成に置き換えたりできます。
(他にも使い道はありますが、モジュール初期化子導入の最大のモチベーションはこれです。)

詳しくは「[モジュール初期化子](../oop/moduleinitializer.md)」で説明します。
