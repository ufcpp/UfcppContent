---
title: "C# 12.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver12/"
content_type: "Article"
published_at: "2023-06-21T00:00:00"
updated_at: "2025-01-01T18:49:40"
tags: []
umbraco_id: 2467
parent_id: 1174
sort_order: 17
aliases:
  - "/csharp/cheatsheet/ap_ver12/"
---

# C# 12.0 の新機能

<div class="version version12">Ver. 12.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2023/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 8.0</li>
</td>
</tr>
</table>

## <a id="sec-generated-title-1"></a> <a id="collection-expression">コレクション式</a>

`[]` 記号を使って配列などの初期化ができるようになりました。
配列だけではなく、コレクション(`List<T>` 型など)、`Span<T>` なども全く同じ書き方で初期化できます。
これをコレクション式(collection expression)と言います。

<pre class="source" title="コレクション式">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
</pre>

また、コレクション式中では、`..` を使うことで「別のコレクションの中身の展開」ができます。
これを スプレッド (spread)演算子と言います。

<pre class="source" title="">
<span class="reserved">int</span>[] <span class="variable">array1</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="reserved">int</span>[] <span class="variable">array2</span> <span class="operator">=</span> [<span class="number">4</span>, <span class="number">5</span>, <span class="number">6</span>];

<span class="comment">// 0, 1, 2, 3, 4, 5, 6, 7</span>
<span class="reserved">int</span>[] <span class="variable">combined</span> <span class="operator">=</span> [<span class="number">0</span>, <em>..</em><span class="variable">array1</span>, <em>..</em><span class="variable">array2</span>, <span class="number">7</span>];
</pre>

詳しくは「[コレクション式](../datatype/collection-expression.md)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="primary-constructor">プライマリ コンストラクター</a>

通常のクラス、構造体に対してプライマリ コンストラクターが使えるようになりました。

<pre class="source" title="">
<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; } <span class="operator">=</span> <span class="variable local">x</span>;
}
</pre>

レコード型の方を先に実装してしまったがために混乱があるんですが、
通常クラス・構造体の場合はプライマリ コンストラクター引数からプロパティを自動生成する機能はありません。

また、これに伴い、`class C;` というように、メンバーを1つも持たないでいい場合に `{}` を書く必要がなくなりました。

詳しくは「[プライマリ コンストラクター](../oop/oo_construct.md#primary-constructor)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="using-any-type">using エイリアスに任意の型を書けるように</a>

C# 11 ではエラーになっていた以下のようなコードをコンパイルできるようになりました。

<pre class="source" title="C# 12 から書ける using エイリアス">
<span class="reserved">using</span> <span class="type struct">Primitive</span> <span class="operator">=</span> <span class="reserved">int</span>;
<span class="reserved">using</span> <span class="type">Array</span> <span class="operator">=</span> <span class="reserved">int</span>[];
<span class="reserved">using</span> <span class="type struct">Nullable</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">?</span>;
<span class="reserved">using</span> <span class="type struct">Tuple</span> <span class="operator">=</span> (<span class="reserved">int</span>, <span class="reserved">int</span>);
</pre>

詳しくは「[任意の型に対する using エイリアス](../structured/sp_namespace.md#using-any-type)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="lambda-default">ラムダ式のデフォルト引数</a>

ラムダ式の引数に[オプション引数](../structured/sp4_optional.md#optional)にできる(既定値を与えられる)ようになりました。
また、[params 引数](../structured/sp_params.md)も使えるようになりました。

<pre class="source" title="ラムダ式の引数の既定値と params 引数">
<span class="comment">// オプション引数(既定値値指定)。</span>
<span class="reserved">var</span> <span class="variable">f1</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="comment">// params 引数。</span>
<span class="reserved">var</span> <span class="variable">f2</span> <span class="operator">=</span> (<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="comment">// 混在も OK。</span>
<span class="reserved">var</span> <span class="variable">f3</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>, <span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;
</pre>

詳しくは「[ラムダ式のオプション引数(既定値)と params 引数](../functional/fun_localfunctions.md#lambda-default)」で説明します。

## <a id="sec-generated-title-5"></a> <a id="ref-readonly-param">ref readonly 引数</a>

ref 引数、in 引数の亜種として、
「書き換えはしないけども、右辺値は受け付けたくない」ということを表す ref readonly 引数というものを導入しました。

<pre class="source" title="ref readonly 引数">
<span class="comment">// in 引数の代わりに ref readonly 引数。</span>
<span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }

<span class="method">m</span>(<span class="warning" title="CS9193"><span class="number">10</span></span>); <span class="comment">// リテラルは警告に。</span>

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="reserved">var</span> <span class="variable">b</span> <span class="operator">=</span> <span class="number">2</span>;
<span class="method">m</span>(<span class="warning" title="CS9193"><span class="variable">a</span> <span class="operator">+</span> <span class="variable">b</span></span>); <span class="comment">// 式も警告に。</span>

<span class="comment">// in や ref を付けないのも警告。</span>
<span class="method">m</span>(<span class="variable"><span class="warning" title="CS9192">a</span></span>);

<span class="comment">// in を付けると警告が出ない。</span>
<span class="method">m</span>(<span class="reserved">in</span> <span class="variable">a</span>);

<span class="comment">// in 引数と違って、ref 修飾でも OK。</span>
<span class="method">m</span>(<span class="reserved">ref</span> <span class="variable">a</span>);
</pre>

ちなみに、呼び出し側の書き方が変わる以外に差はなく、コンパイル結果の挙動は in 引数と全く同じです。
呼び出し側の差は以下の通りです。

| 呼び方 | in | ref readonly |
| --- | --- | --- |
| `m(ref x)` | 警告 | OK |
| `m(in x)`  | OK | OK |
| `m(x)`, `m(x + y)`, `m(123)`     | OK | 警告 |

詳しくは「[ref readonly 引数](../resource/sp_ref.md#ref-readonly-param)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="other"></a>その他

### <a id="sec-generated-title-7"></a> <a id="inline-array">InlineArray</a>

.NET 8 で、[`InlineArray` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.inlinearrayattribute) (`System.Runtime.CompilerServices` 名前空間) というものが入って、「値型の固定長配列」みたいなものを作れるようになりました。

<pre class="source" title="InlineArray 属性">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// この属性を付けると、 .NET ランタイムが特別扱いして、構造体のサイズを拡大する。</span>
<span class="comment">// (コンストラクター引数で Length 指定。)</span>
[<span class="type">InlineArray</span>(<span class="number">3</span>)]
<span class="reserved">struct</span> <span class="type struct">FixedBuffer</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value</span>;
}
</pre>

基本的には .NET ランタイム側の機能ですが、
いくつか、C# 側にもこの `InlineArray` 向けの特殊対応が入っています。

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

詳しくは「[[雑記] InlineArray](../datatype/inline-array.md)」で説明します。

### <a id="sec-generated-title-8"></a> <a id="nameof-instance-menbers"></a>nameof の微修正

[`nameof` 演算子](../start/st_string.md#nameof-operator)にちょっとした修正が入りました。

C# 11 以前だと、以下の例の最後の行のように、
静的メンバー内から「インスタンス メンバーのインスタンス メンバー」みたいな名前の参照ができなかったようです。

<pre class="source" title="C# 11 まではエラーになっていたコードの例">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Instance</span> { <span class="reserved">get</span>; }

    <span class="comment">// これは元から行けた。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="method">InstanceM</span>() <span class="operator">=&gt;</span> <span class="reserved">nameof</span>(<span class="property">Instance</span><span class="operator">.</span><span class="property">Length</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span> <span class="static"><span class="method">StaticM1</span></span>() <span class="operator">=&gt;</span> <span class="reserved">nameof</span>(<span class="reserved">string</span><span class="operator">.</span><span class="property">Length</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span> <span class="method"><span class="static">StaticM2</span></span>() <span class="operator">=&gt;</span> <span class="reserved">nameof</span>(<span class="property">Instance</span>);

    <span class="comment">// これが今までダメだったらしい。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span> <span class="static"><span class="method">StaticM</span></span>() <span class="operator">=&gt;</span> <span class="reserved">nameof</span>(<span class="property"><span class="error" title="CS0120">Instance</span></span><span class="operator">.</span><span class="property">Length</span>);
}
</pre>

これが、C# 12 ではコンパイルできるようになりました。

正直、バグ修正扱い(最新コンパイラーを使うと C# 11 以下でもコンパイルが通るようになる)でもいいレベルだとは思いますが、一応は C# 12 以上限定です。
