---
title: "[雑記] 型の決定"
source_url: "https://ufcpp.net/study/csharp/start/misctyperesolution/"
content_type: "Article"
published_at: "2019-12-08T00:00:00"
updated_at: "2021-01-02T00:00:00"
tags: []
umbraco_id: 2275
parent_id: 1190
sort_order: 18
aliases:
  - "/csharp/start/misctyperesolution/"
---

# \[雑記\] 型の決定

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
C# の[変数](st_variable.md#variable)や[式](st_variable.md#expression)はそれぞれが「型」を持っています。
例えば `int x;` として宣言した変数 `x` は `int` 型になりますし、`"abc"` という式(文字列[リテラル](st_variable.md#literal)も式の一種)は `string` 型になります。

そして、代入(`=`)などの処理では、左右両辺の型が一致しないとコンパイル時にエラーを起こします。
例えば以下のコードはコンパイルできません。

<pre class="source" title="型の不一致でのコンパイル エラー">
<code><span class="reserved">int</span> <span class="variable">x</span> = <span class="error"><span class="string">&quot;abc&quot;</span></span>;
</code></pre> 

どうせ左右で型を合わせる必要があるわけで、片方からもう片方の型を自動決定する構文もいくつかあります。
[ローカル変数の型推論(`var` 変数宣言)](sp3_inference.md#type-inference)が代表例で、
例えば以下のような書き方をすると、「右辺の型に合わせて `x` の型が自動的に `string` になる」という挙動になります。

<pre class="source" title="ローカル変数の型推論">
<code><span class="reserved">var</span> <span class="variable">x</span> = <span class="string">&quot;abc&quot;</span>;
</code></pre>

逆に、反対側の辺を見ないと型が決定できないようなものもいくつかあります。
[デリゲート](../functional/sp_delegate.md)や[匿名関数](../functional/fun_localfunctions.md#anonymous-function)が代表例で、
例えば以下のコードは「型が決定できなくてコンパイル エラー」になります。

<pre class="source" title="左辺の型が必須な構文">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="error"><span class="variable">m</span> = Main</span>;
        <span class="reserved">var</span> <span class="error"><span class="variable">f</span> = () =&gt; { }</span>;
 
        <span class="comment">// 以下の書き方ならコンパイル可能。左辺の型が必須。</span>
        <span class="type">Action</span> <span class="variable">m1</span> = <span class="method">Main</span>;
        <span class="type">Action</span> <span class="variable">f1</span> = () =&gt; { };
    }
}
</code></pre>

本項では、こういった「型の決定」について説明していきます。

##<a id="sec-generated-title-2"></a> <a id="source-target"></a>型決定の「向き」
型の決定には「向き」があります。
概要で話した通り、型決定の代表例は代入処理で、`=` 演算子の左右を指して「左辺」(left hand side)、「右辺」(right hand side)と言ったりします。
ただ、同様の型決定は、必ずしも「左右」になっていない構文でも発生します。
例えば、メソッド呼び出し([オーバーロード解決](../structured/miscoverloadresolution.md))の場合は「左右」というよりは「内外」といった方がいいかもしれません。

<pre class="source" title="型決定の「左右」、「内外」">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 右辺の &quot;abc&quot; から左辺の s の型が string に決定。</span>
        <span class="reserved">var</span> <span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>;
 
        <span class="comment">// 内側の 1 から外側の X (の引数)の型が int に決定(X(int x) が呼ばれる)。</span>
        <span class="method">X</span>(1);
 
        <span class="comment">// 左辺の Action から右辺の () =&gt; { } の型が決定。</span>
        <span class="type">Action</span> <span class="variable">a</span> = () =&gt; { };
 
        <span class="comment">// 外側の Y (の引数の Action)から内側の () =&gt; { } の型が決定。</span>
        <span class="method">Y</span>(() =&gt; { });
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">X</span>(<span class="reserved">int</span> <span class="variable">x</span>) { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">X</span>(<span class="reserved">string</span> <span class="variable">x</span>) { }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Y</span>(<span class="type">Action</span> <span class="variable">a</span>) { }
}
</code></pre>

どちらの場合でも、「値の出所」と「値を受け取る側」に分かれます。
そして、出所の方を<strong id="source-type" class="keyword">ソース型</strong>(source type)、受け取る側を<strong id="target-type" class="keyword">ターゲット型</strong>(target type)と言います。

| 型の「向き」 | 例 |
| --- | --- |
| ソース | 代入の右辺、メソッド呼び出しの実引数 |
| ターゲット | 代入の左辺、メソッドの仮引数 |

元々 C# ではソース型の方を明示的に指定して、ターゲット型の方を自動決定することが多いです。
なので、単に推論(inference)とか解決(resolution)という場合、この向き(ソース型からの決定)なことが多いです。

<pre class="source" title="ソース型からの型決定">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 変数の型推論(type inference)はソース型からの型決定</span>
        <span class="reserved">var</span> <span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>;
 
        <span class="comment">// オーバーロード解決(overload resolution)はソース型からの型決定。</span>
        <span class="method">X</span>(1);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">X</span>(<span class="reserved">int</span> <span class="variable">x</span>) { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">X</span>(<span class="reserved">string</span> <span class="variable">x</span>) { }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Y</span>(<span class="type">Action</span> <span class="variable">a</span>) { }
}
</code></pre>

しかし徐々にターゲット型の方を明示的に指定する構文が増えています。
後入りな構文が多いせいか、こちらは「ターゲット型からの(target typed)」という形容をすることが多いです。
例えば C# 7.1 で入った [`default` 式](../resource/rm_default.md#default-expr)は「target typed default」などと呼ばれることがあります。
また、C# 8.0 で入った [`switch` 式はターゲットからの型決定](../datatype/typeswitch.md#target-typed)をしていますが、
こちらも「target typed switch」と言われたりします。

<pre class="source" title="ターゲット型からの型決定">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="comment">// 以前の C# では default(DateTime) と書く必要があった。</span>
    <span class="comment">// C# 7.1 から、ターゲットからの型推論で default だけで書けるようになった。</span>
    <span class="type">DateTime</span> <span class="variable">t</span> = <span class="reserved">default</span>;
 
    <span class="comment">// C# の型決定機構では「1 と null の共通型が何かわからない」ということでコンパイルできなかった。</span>
    <span class="comment">// switch 式ではターゲット型(この場合 int?)を見て、switch の型を決めて、1 と null を受け付けできるようにした。</span>
    <span class="reserved">int</span>? <span class="variable">x</span> = <span class="variable">b</span> <span class="control">switch</span>
    {
        <span class="reserved">true</span> =&gt; 1,
        <span class="reserved">false</span> =&gt; <span class="reserved">null</span>
    };
}
</code></pre>

#####<a id="sec-generated-title-3"></a> <a id="source-typed"></a>ソース型からの決定
ソース型によって挙動が決まる構文として以下のようなものがあります。

- [ローカル変数の型推論](sp3_inference.md#implicit)
- [配列の暗黙的型付け](sp3_inference.md#impl_array)
- [オーバーロード解決](../structured/miscoverloadresolution.md)
  - 特に、[ジェネリック型引数の推論](../oop/sp2_generics.md#method)

#####<a id="sec-generated-title-4"></a> <a id="source-typed"></a>ターゲット型からの決定
ターゲット型によって挙動が決まる構文として以下のようなものがあります。

単に「ターゲットを見るまで型が確定しない」程度のものもあります。
暗黙的型変換の一種として考えることができます(未確定の型から確定した型への変換)。

- [整数リテラル](st_embeddedtype.md#intl)
- [null](../oop/oo_class.md#null)
- [`default` 式](../resource/rm_default.md#default-expr)
- [`new` 演算子](../oop/oo_construct.md#target-typed-new)
- [デリゲート](../functional/sp_delegate.md)

もう少し積極的にターゲット型の情報を使う構文もあります。

- [`switch` 式](../datatype/typeswitch.md#target-typed)
- [条件演算子](../structured/st_branch.md#terget-typed-conditional)

また、ターゲットの型によってまるっきり異なる挙動になるものもあります。

 - [文字列補間](st_string.md#string-interpolation)
  - [`IFormattable` 型に渡すとき](st_string.md#FormattableString)
 - [ラムダ式](../functional/sp3_lambda.md)
  - [`Expression` 型に渡すとき](../functional/sp3_lambda.md#expression)

ちなみに、組み合わせも行けます。
以下のように、`switch` 式中の条件演算子中の `new` みたいな入れ子になった状況でもターゲット型推論が働きます。

<pre class="source" title="ターゲット型推論の入れ子">
<code><span class="reserved">using</span> System;
 
<span class="comment">// target-typed switch 式中の</span>
<span class="comment">// target-typed 条件演算子中の</span>
<span class="comment">// target-typed new 式。</span>
<span class="type">TimeSpan</span> <span class="method">X</span>(<span class="reserved">int</span> <span class="variable">i</span>, <span class="reserved">bool</span> <span class="variable">b</span>) =&gt; <span class="variable">i</span> <span class="control">switch</span>
{
    &lt; 0 =&gt; <span class="variable">b</span> ? <span class="reserved">new</span>(0) : <span class="reserved">new</span>(1),
    0 =&gt; <span class="variable">b</span> ? <span class="reserved">new</span>(2) : <span class="reserved">new</span>(6),
    &gt; 0 =&gt; <span class="variable">b</span> ? <span class="reserved">new</span>(4) : <span class="reserved">new</span>(5),
};
</code></pre>

##<a id="sec-generated-title-5"></a> <a id="history"></a>自動型決定の歴史
ソース/ターゲットのいずれか一方だけの型を指定して他方を自動決定するというのは、
2000年代頃から増え始めたものです。
20世紀の(1990年代以前の)プログラミング言語では少数派でしたし、
C# でも、C# 2.0 や 3.0 から導入された構文が多いです。
例えば、C# 1.0 (2000年発表、2002年正式リリース)時代には以下のような書き方はできませんでした。

<pre class="source" title="C# 1.0 時代">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
{
    <span class="comment">// C# 2.0 から。</span>
    <span class="comment">// C# 1.0 時代は Action m = new Action(Main); と書く必要あり。</span>
    <span class="type">Action</span> <span class="variable">m</span> = <span class="method">Main</span>;
 
    <span class="comment">// C# 3.0 から。</span>
    <span class="comment">// C# 1.0 時代は string s = &quot;abc&quot;; と書く必要あり。</span>
    <span class="reserved">var</span> <span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>;
}
</code></pre>

ただ、明確に型推論(type inference)という言葉が出始めたのは C# 3.0 の頃からですが、
それ以前でも、推論に類するものはありました。
例えばメソッドのオーバーロード解決は「ソース型からの型決定」に類するものですし、
整数リテラルや null などは実は「ターゲット型からの型決定」をしています
(正確に言うと「暗黙的型変換」なんですが、いずれにせよターゲット型が決まるまで解釈が確定しません)。

<pre class="source" title="整数リテラルの型決定">
<code><span class="comment">// byte リテラルや short リテラルは存在していなくて、「整数リテラル」の暗黙的型変換で代用している。</span>
<span class="reserved">byte</span> <span class="variable">a</span> = 1;  <span class="comment">// この 1 は byte (に代入可能)</span>
<span class="reserved">short</span> <span class="variable">b</span> = 1; <span class="comment">// この 1 は short (に代入可能)</span>

<span class="comment">// 変数だと int から byte や short への暗黙的変換は認められていない。コンパイル エラーに。</span>
<span class="reserved">int</span> <span class="variable">i</span> = 1;
<span class="variable">a</span> = <span class="error"><span class="variable">i</span></span>;
 
<span class="comment">// ちゃんと精度チェックが入る。byte に代入できない大きさの整数リテラルはコンパイル エラーを起こす。</span>
<span class="variable">a</span> = <span class="error">256</span>;

<span class="comment">// ただし、var に対して使った時は int 扱い。</span>
<span class="reserved">var</span> <span class="variable">c</span> = 1; <span class="comment">// c の型は int</span>
 
<span class="comment">// 配列初期化子はターゲット型を見ているのでこの書き方は OK。</span>
<span class="reserved">byte</span>[] <span class="variable">d</span> = { 1, 2 }; <span class="comment">// この 1, 2 は byte 扱い</span>
 
<span class="comment">// でも、配列の型推論はソース型からの型決定なので NG。</span>
<span class="comment">// 右辺は int[] 扱いで、左辺の byte[] と型が合わなくてコンパイル エラーになる。</span>
<span class="reserved">byte</span>[] <span class="variable">e</span> = <span class="error"><span class="reserved">new</span>[] { 1, 2 }</span>;
</code></pre>

##<a id="sec-generated-title-6"></a> <a id="conflict"></a>自動型決定の競合
ソースから型決定する構文とターゲットから型決定する構文は、当然ですが両立はできません。
どちらもあいまいでは型決定できません。片方は明示的な型指定が必要になります。

<pre class="source" title="推論に推論は重ねられない">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// OK。引数の側の型を明示。</span>
        <span class="method">X</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">a</span>);
 
        <span class="comment">// OK。メソッドの側の型を明示。</span>
        <span class="method">X</span>&lt;<span class="reserved">int</span>&gt;(<span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">b</span>);
 
        <span class="comment">// NG。(型引数の)推論と(ローカル変数の)推論が重なって型決定不可。</span>
        <span class="error">X</span>(<span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">c</span>);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">X</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">out</span> <span class="type">T</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> = <span class="reserved">default</span>;
}
</code></pre>

ちなみに、型推論は「後から」競合を起こす原因になり得たりもします。
例えば以下のようなコードはコンパイルできるコードなんですが、

<pre class="source" title="有効な型推論">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">DateTime</span>? x) { }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="method">M</span>(<span class="reserved">null</span>);
        <span class="method">M</span>(<span class="reserved">default</span>);
        <span class="method">M</span>(<span class="reserved">new</span>());
    }
}
</code></pre>

ここに1行、オーバーロードを増やすとどちらを呼ぶべきか決定できなくてコンパイル エラーになります。

<pre class="source" title="オーバーロードの追加が破壊的変更になり得る">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span>? x) { } <span class="comment">// この行を追加</span>
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">DateTime</span>? x) { }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="error">M</span>(<span class="reserved">null</span>);
        <span class="error">M</span>(<span class="reserved">default</span>);
        <span class="error">M</span>(<span class="reserved">new</span>());
    }
}
</code></pre>

##<a id="sec-generated-title-7"></a> <a id="priority"></a>優先度付きのターゲットからの型決定
ターゲットの型を見て挙動が変わりはするものの、
未指定の場合の既定の挙動が決まっていて、ソース型からの型推論と競合しないものもあります。

整数リテラルなどがそうで、整数リテラルはターゲットの型によって挙動を変えますが、
型推論に対しては `int` 扱いになりますし、
オーバーロード解決では `int` が最優先になります。

<pre class="source" title="整数リテラルの既定の型は int">
<code><span class="comment">// ターゲットの型を見ている。</span>
<span class="reserved">byte</span> <span class="variable">a</span> = 1;  <span class="comment">// この 1 は byte 扱い</span>
<span class="reserved">short</span> <span class="variable">b</span> = 1; <span class="comment">// この 1 は short 扱い</span>
 
<span class="comment">// ターゲット型が決まっていない場合は int になる。</span>
<span class="reserved">var</span> <span class="variable">c</span> = 1; <span class="comment">// 1 は int で、そこからの型推論で c の型は int</span>

<span class="reserved">void</span> <span class="method">f</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>) { }
<span class="method">f</span>(1); <span class="comment">// 型引数の推論でも int 扱い</span>
</code></pre>

[文字列補間](st_string.md#string-interpolation)にも優先度があります。
文字列補間は `IFormattable` 型よりも `string` が優先です。

<pre class="source" title="文字列補間は IFormattable より string が優先">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>)
    {
        <span class="comment">// var に対して文字列補間を使うと string 扱い。</span>
        <span class="reserved">var</span> <span class="variable">s</span> = <span class="string">$&quot;abc </span>{<span class="variable">x</span>}<span class="string">&quot;</span>;
 
        <span class="comment">// M(string) が優先的に呼ばれる。</span>
        <span class="method">M</span>(<span class="string">$&quot;abc </span>{<span class="variable">x</span>}<span class="string">&quot;</span>);
 
        <span class="comment">// M(IFormattable) の方を呼びたければキャストが必要。</span>
        <span class="method">M</span>((<span class="type">IFormattable</span>)<span class="string">$&quot;abc </span>{<span class="variable">x</span>}<span class="string">&quot;</span>);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>) { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">IFormattable</span> <span class="variable">s</span>) { }
}
</code></pre>

##<a id="sec-generated-title-8"></a> <a id="cost"></a>自動型決定のコスト
ソースとターゲットのどちらか片方から他方を決定できるといっても、
その推論が低コストなものと、意外と高コストなものがあったりします。

例えば、ローカル変数の型推論はほとんどコストがかからないそうです。
なんせ左辺と右辺が1対1ですし、元々「型が合うかどうか」の判定のために左右どちらにも明確な型を求めています。
単に片方を他方に伝搬させるだけなので低コストです。

一方、オーバーロード解決は結構高コストです。
多数の候補の中から1つを選ばないといけないので単純に検索コストがかかります。
例えば、以下のようなコードでは `Parse("")` で呼び出せるメソッドの候補が4つあります。

<pre class="source" title="複数の候補があるメソッド呼び出しの例">
<code><span class="comment">// この1行によって DateTime.Parse(string) がオーバーロード解決候補に入る。</span>
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">DateTime</span>;
 
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="comment">// 基底クラスにも同名のメソッド。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Parse</span>(<span class="reserved">string</span> <span class="variable">x</span>) { }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// Parse(&quot;&quot;) で呼び出せる候補が複数。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Parse</span>(<span class="reserved">object</span> <span class="variable">x</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Parse</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>) { }
 
    <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="method">Parse</span>(<span class="string">&quot;abc&quot;</span>);
    }
}
</code></pre>

オーバーロードの数や引数の数が多くなれば多くなるほど複雑になることは容易に想像できるかと思います。
標準ライブラリ中にも多数のオーバーロードを持つメソッドは多く、容易に複雑化します。
実は、C# ソースコードのコンパイル時間のうち数割程度はオーバーロード解決が占めているといわれています。

<pre class="source" title="いかにも複雑そうなオーバーロード解決の例">
<code><span class="comment">// 以下の5つの Parse が候補に</span>
<span class="comment">// int Parse(ReadOnlySpan&lt;char&gt; s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)</span>
<span class="comment">// int Parse(string s)</span>
<span class="comment">// int Parse(string s, NumberStyles style)</span>
<span class="comment">// int Parse(string s, NumberStyles style, IFormatProvider provider)</span>
<span class="comment">// int Parse(string s, IFormatProvider provider)</span>
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Int32</span>;
 
<span class="comment">// 以下の4つの Parse が候補に</span>
<span class="comment">// DateTime Parse(ReadOnlySpan&lt;char&gt; s, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)</span>
<span class="comment">// DateTime Parse(string s)</span>
<span class="comment">// DateTime Parse(string s, IFormatProvider provider)</span>
<span class="comment">// DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)</span>
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">DateTime</span>;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 2引数で、第2引数が NumberStyles なものは1個しかないので、</span>
        <span class="comment">// int Parse(string, NumberStyles)</span>
        <span class="method">Parse</span>(<span class="reserved">null</span>, System.Globalization.<span class="type">NumberStyles</span>.HexNumber);
 
        <span class="comment">// stackalloc によって第1引数の型が ReadOnlySpan&lt;char&gt; に決定</span>
        <span class="comment">// 第3引数が DateTimeStyles なので、</span>
        <span class="comment">// DateTime Parse(ReadOnlySpan&lt;char&gt;, IFormatProvider, DateTimeStyles)</span>
        <span class="method">Parse</span>(<span class="reserved">stackalloc</span> <span class="reserved">char</span>[0], <span class="reserved">null</span>, System.Globalization.<span class="type">DateTimeStyles</span>.None);
    }
}
</code></pre>

ちなみに、C# は、ローカル変数に対しては型推論(`var` 変数宣言)を認めていますが、
メンバー(フィールド、プロパティやメソッド)の引数・戻り値に対しては認めていません。
これは、作法的な問題(メンバーの型は明示すべきという思想)もありますが、
簡単に高コストになりうるから認められないという問題もあるそうです。

<pre class="source" title="メンバーの型推論は認めない">
<code><span class="comment">// 以下、仮定的な構文。C# では認めていない(おそらく今後も認めない)。</span>
 
<span class="comment">// 再帰しているので当然型決定が不可能。</span>
<span class="comment">// この例はまだ単純なものの、「再帰の検知」も十分複雑になりえる。</span>
<span class="reserved">static</span> <span class="reserved">var</span> a = b;
<span class="reserved">static</span> <span class="reserved">var</span> b = a;
 
<span class="comment">// ただでさえ複雑なオーバーロード解決と組み合わせると悲惨なことに…</span>
<span class="reserved">static</span> (<span class="type">T</span>, <span class="type">U</span>) <span class="method">M</span>&lt;<span class="type">T</span>, <span class="type">U</span>&gt;(<span class="type">T</span> <span class="variable">t</span>, <span class="type">U</span> <span class="variable">u</span>) =&gt; (<span class="variable">t</span>, <span class="variable">u</span>);
<span class="reserved">static</span> <span class="reserved">short</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">string</span> <span class="variable">y</span>) =&gt; 0;
<span class="reserved">static</span> <span class="reserved">float</span> <span class="method">M</span>(<span class="reserved">double</span> <span class="variable">x</span>, <span class="reserved">string</span> <span class="variable">y</span>) =&gt; 0;
 
<span class="reserved">static</span> <span class="reserved">var</span> <span class="method">A</span>() =&gt; M(<span class="method">B</span>(), <span class="method">C</span>());
<span class="reserved">static</span> <span class="reserved">var</span> <span class="method">B</span>() =&gt; M(<span class="method">D</span>(), <span class="method">E</span>());
<span class="reserved">static</span> <span class="reserved">var</span> <span class="method">C</span>() =&gt; M(<span class="method">F</span>(), <span class="string">&quot;&quot;</span>);
<span class="reserved">static</span> <span class="reserved">var</span> <span class="method">D</span>() =&gt; <span class="method">M</span>(1, 1.2);
<span class="reserved">static</span> <span class="reserved">var</span> <span class="method">E</span>() =&gt; <span class="method">M</span>(1, <span class="string">&quot;&quot;</span>);
<span class="reserved">static</span> <span class="reserved">var</span> <span class="method">F</span>() =&gt; <span class="method">M</span>(1.2, <span class="reserved">new</span> <span class="reserved">object</span>());
</code></pre>

##<a id="sec-generated-title-9"></a> <a id="nest"></a>入れ子
いくつかの構文では、多段に中身を追って型決定してくれます。
例えば、以下のように、多重のラムダ式からオーバーロード解決することができます。
(ただし制限あり。)

<pre class="source" title="多重ラムダ式からのオーバーロード解決">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 非ジェネリックなオーバーロードなら、多段ラムダ式でも解決可能。</span>
        <span class="comment">// (ただし、この解決ができるのは C# 6.0 以降)</span>
        <span class="method">M</span>(() =&gt; () =&gt; 1);   <span class="comment">// M(Func&lt;Func&lt;int&gt;&gt; x)</span>
        <span class="method">M</span>(() =&gt; () =&gt; 1.0); <span class="comment">// M(Func&lt;Func&lt;double&gt;&gt; x)</span>
 
        <span class="comment">// ただ、ジェネリックなものについては無理。コンパイル エラーに。</span>
        <span class="comment">// M&lt;string&gt; は呼んでもらえない。</span>
        M(() =&gt; () =&gt; <span class="string">&quot;&quot;</span>);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">Func</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt;&gt; <span class="variable">x</span>) { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">Func</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">double</span>&gt;&gt; <span class="variable">x</span>) { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">Func</span>&lt;<span class="type">Func</span>&lt;<span class="type">T</span>&gt;&gt; <span class="variable">x</span>) { }
}
</code></pre>

暗黙の型変換の類は[タプル](../datatype/tuples.md)の中でも働きますし、この場合、タプルが入れ子になっていても大丈夫です。

<pre class="source" title="入れ子のタプル">
<code><span class="comment">// 整数リテラルはターゲット型を見て暗黙的に変換がかかる。</span>
<span class="comment">// たとえ入れ子のタプルになっていてもこの仕組みは働く。</span>
(<span class="reserved">byte</span>, (<span class="reserved">short</span>, <span class="reserved">long</span>)) <span class="variable">t</span> = (1, (2, 3));
 
<span class="comment">// ちなみに、以下のコードだとコンパイル エラー。</span>
<span class="comment">// リテラル以外では、int から byte, short への変換は暗黙的にできない。</span>
(<span class="reserved">int</span>, (<span class="reserved">int</span>, <span class="reserved">int</span>)) <span class="variable">i</span> = (1, (2, 3));
<span class="variable">t</span> = <span class="variable">i</span>;
</code></pre>

[target-typed な`switch` 式](../datatype/typeswitch.md#target-typed)も、入れ子になっていても平気です。

<pre class="source" title="入れ子の swtich 式">
<code><span class="reserved">static</span> <span class="reserved">byte</span>? <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>) =&gt; <span class="variable">obj</span> <span class="control">switch</span>
{
    <span class="reserved">string</span> <span class="variable">s</span> =&gt; <span class="variable">s</span>.Length <span class="control">switch</span>
    {
        0 =&gt; 1,    <span class="comment">// 2重の switch の中でも byte に変換できる</span>
        <span class="reserved">_</span> =&gt; <span class="reserved">null</span>, <span class="comment">// 同、null を byte? 扱いできる</span>
    },
    <span class="reserved">byte</span> <span class="variable">i</span> =&gt; <span class="variable">i</span>,
    <span class="reserved">_</span> =&gt; <span class="reserved">null</span>,
};
</code></pre>

一方で、条件演算子や配列の型推論、ジェネリック型引数などは、ターゲット型からの型推論に対応していなくて、
以下のコードはコンパイル エラーになります。
(ただし、条件演算子については C# 9.0 でターゲット型推論を導入する予定があります。)

<pre class="source" title="ターゲット型からの型推論を持っていない構文">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Fail</span>()
{
    <span class="comment">// 以下のいずれもコンパイル エラー。</span>
    <span class="reserved">byte</span>? <span class="variable">a</span> = <span class="error"><span class="reserved">true</span> ? 1 : <span class="reserved">null</span></span>;
    <span class="reserved">byte</span>?[] <span class="variable">b</span> = <span class="reserved">new</span>[] { 1, <span class="error"><span class="reserved">null</span></span> };
    <span class="reserved">byte</span>? <span class="variable">c</span> = M(1, <span class="error"><span class="reserved">null</span></span>);
}
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Success</span>()
{
    <span class="comment">// ターゲット型推論に頼らない書き方が求められる。</span>
    <span class="comment">// 以下の書き方ならソースからだけで型決定できる。</span>
    <span class="reserved">byte</span>? <span class="variable">a</span> = <span class="reserved">true</span> ? (<span class="reserved">byte</span>?)1 : <span class="reserved">null</span>;
    <span class="reserved">byte</span>?[] <span class="variable">b</span> = <span class="reserved">new</span>[] { (<span class="reserved">byte</span>?)1, <span class="reserved">null</span> };
    <span class="reserved">byte</span>? <span class="variable">c</span> = <span class="method">M</span>((<span class="reserved">byte</span>?)1, <span class="reserved">null</span>);
}
 
<span class="reserved">static</span> <span class="type">T</span> <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>, <span class="type">T</span> <span class="variable">y</span>) =&gt; <span class="variable">x</span>;
</code></pre>

##<a id="sec-generated-title-10"></a> <a id="common-type"></a>共通型
`switch` 式や条件演算子など、いくつかの「枝」を持つ構文では、枝ごとの型の「共通の型」(common type)を探す作業を一応行います。
ただ、C# 8.0 時点では制約がきつく、「枝のうちいずれか1つ」しか選ばれません。

ちょっとわかりにくいと思うので具体例を挙げます。
まず、以下のようなクラスを用意します。

<pre class="source" title="型階層の例">
<code><span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Base</span> { }
</code></pre>

このクラスと、あと、int を使って共通型を決定できるかどうかの例を示します。

<pre class="source" title="共通型の決定">
<code><span class="comment">// 型の候補は A, B。それぞれお互いには変換不可なので、共通型の決定不可。</span>
<span class="reserved">var</span> <span class="variable">ng1</span> = <span class="error"><span class="variable">x</span> ? <span class="reserved">new</span> <span class="type">A</span>() : <span class="reserved">new</span> <span class="type">B</span>()</span>;
 
<span class="comment">// 型の候補は int。null は int に変換不可なので、共通型の決定不可。</span>
<span class="reserved">var</span> <span class="variable">ng2</span> = <span class="error"><span class="variable">x</span> ? 1 : <span class="reserved">null</span></span>;
 
<span class="comment">// 型の候補は Base, A。A から Base に変換可能なので、共通型は Base に決定。</span>
<span class="reserved">var</span> <span class="variable">ok1</span> = <span class="variable">x</span> ? <span class="reserved">new</span> <span class="type">Base</span>() : <span class="reserved">new</span> <span class="type">A</span>();
 
<span class="comment">// 型の候補は int?。null は int? に変換可能なので、共通型は int?。</span>
<span class="reserved">var</span> <span class="variable">ok2</span> = <span class="variable">x</span> ? (<span class="reserved">int</span>?)1 : <span class="reserved">null</span>;
 
<span class="comment">// 型の候補は int, int?。int は int? に変換可能なので、共通型は int?。</span>
<span class="reserved">var</span> <span class="variable">ok3</span> = <span class="variable">x</span> ? 1 : <span class="reserved">default</span>(<span class="reserved">int</span>?);
</code></pre>

`int` と null の共通型は `int?` だとわかりそうなものですが、少なくとも C# 8.0 ではそういう自動判定はしません。
枝のいずれか1つが `int?` でないと共通型判定できません。

ちなみに、「提案が出ている」という程度の状態で実現するかはわかりませんが、
値型 `T`と null が並んでいた場合、共通の型として `T?` を選ぶようにするという案もあります。

また、クラスの場合も共通の基底クラス(`A` と `B` の場合 `Base`)を共通型として選ぶかどうかも検討されています。
こちらは「基底クラスに限る」(共通インターフェイスの場合は相変わらず)という条件付きです。
インターフェイスが絡むと以下のように多段派生があったり複雑なのでおそらく認められません。

<pre class="source" title="共通型の決定が難しい例">
<code><span class="comment">// 型 D と F の「共通型」といわれると何？</span>
<span class="comment">// インターフェイス J？ それともクラス A？</span>
<span class="reserved">interface</span> <span class="type">I</span> { }
<span class="reserved">interface</span> <span class="type">J</span> { }
<span class="reserved">class</span> <span class="type">A</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>, <span class="type">I</span> { }
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">A</span> { }
<span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>, <span class="type">J</span> { }
<span class="reserved">class</span> <span class="type">E</span> : <span class="type">B</span> { }
<span class="reserved">class</span> <span class="type">F</span> : <span class="type">C</span>, <span class="type">J</span> { }
</code></pre>
