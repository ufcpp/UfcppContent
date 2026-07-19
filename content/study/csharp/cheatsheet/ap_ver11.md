---
title: "C# 11.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver11/"
content_type: "Article"
published_at: "2022-05-04T00:00:00"
updated_at: "2022-09-22T00:00:00"
tags: []
umbraco_id: 2423
parent_id: 1174
sort_order: 16
aliases:
  - "/csharp/cheatsheet/ap_ver11/"
---

# C# 11.0 の新機能

<div class="version version11">Ver. 11.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2022/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 7.0</li>
<li>Visual Studio 2022 17.4</li>
</td>
</tr>
</table>

執筆予定: [C# 11.0 トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/387)

## <a id="sec-generated-title-1"></a> <a id="utf8-literal"></a>UTF-8 リテラル

`"abc"u8` みたいに、文字列リテラルの後ろに u8 接尾辞を付けることで、UTF-8 な byte 列を文字列リテラルの形で書けるようになりました。

<pre class="source" title="u8 リテラルの例">
<code><span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">hex</span> <span class="operator">=</span> <span class="string">&quot;0123456789ABCDEF&quot;u8</span>;
</code></pre>

以下のような byte 列とほぼ同じ意味になります。

<pre class="source" title="u8 リテラルの展開結果の例">
<code><span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">97</span>, <span class="number">98</span>, <span class="number">99</span> };
</code></pre>

詳しくは「[UTF-8 リテラル](../start/st_string.md#utf8-literal)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="raw-string">生文字列リテラル</a>

C# 11 で、3つ以上の連続した `"` を使うことで、「一切エスケープが必要ない文字列リテラル」を書けるようになりました。

<pre class="source" title="raw string literal">
<code><span class="comment">// &quot;&quot;&quot; から始まる文字列リテラル(raw string, 生文字列)。</span>
<span class="reserved">var</span> <span class="variable">quote</span> = <span class="string">&quot;&quot;&quot;
    &quot; はそのまま &quot; として使われて、
    \ も \ のままの意味。
    \\ は \ が2個。
    {} とかも特別な解釈はされない。
    &quot;&quot;&quot;</span>;
</code></pre>

この `"""` を使った書き方で、さらに文字列補間をすることもできます。

<pre class="source" title="$ を2個にすれば、{ 1個はエスケープなしで書ける">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">format</span>(123, <span class="string">&quot;abc&quot;</span>));

<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">format</span>(<span class="reserved">int</span> <span class="variable">id</span>, <span class="reserved">string</span> <span class="variable">name</span>) =&gt; <span class="string">$$&quot;&quot;&quot;
</span><span class="string">    {
      &quot;id&quot;: </span>{{<span class="variable">id</span> <span class="comment">/* ここは補間 */</span> }}<span class="string">,
      &quot;name&quot;: &quot;</span>{{<span class="variable">name</span> <span class="comment">/* ここも補間 */</span>}}<span class="string">&quot;
    }</span><span class="string">
    &quot;&quot;&quot;</span>;
</code></pre>

詳しくは「[生文字列リテラル](../start/st_string.md#raw-string)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="required">required メンバー</a>

プロパティとフィールドに対する `required` 修飾子というものが追加されました。
これを使うと、[オブジェクト初期化子](../oop/oo_construct.md#member_initializer)で何らかの値を代入することを義務付けられます。
例えば以下のようなコードを書いたとき、`a1` 以外の `new A` はエラーになります。
(警告ではなくエラーにします。)

<pre class="source" title="required 修飾子">
<span class="reserved">var</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span> { <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span> };

<span class="reserved">var</span> <span class="variable">a2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035">A</span></span> { <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span> }; <span class="comment">// Y を代入していないのでエラー。</span>
<span class="reserved">var</span> <span class="variable">a3</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035">A</span></span> { <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span> };   <span class="comment">// X を代入していないのでエラー。</span>
<span class="reserved">var</span> <span class="variable">a4</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035"><span class="error" title="CS9035">A</span></span></span>();             <span class="comment">// X も Y も代入していないのでエラー。</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <em><span class="reserved">required</span></em> <span class="reserved">string</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <em><span class="reserved">required</span></em> <span class="reserved">int</span> <span class="property">Y</span>;
}
</pre>

詳しくは「[required メンバー](../oop/oo_property.md#required)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="list">リスト パターン</a>

C# 11で、`[]` を使ってリスト(配列や `List<T>` など)に対するパターン マッチングができるようになりました。
例えば以下のような `switch` を書けます。

<pre class="source" title="リスト パターンの例">
<code><span class="reserved">static</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="method">removeBom</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">utf8</span>)
    <span class="operator">=&gt;</span> <span class="variable local">utf8</span> <span class="reserved">is</span> [<span class="number">0xEF</span>, <span class="number">0xBB</span>, <span class="number">0xBF</span>, .. <span class="reserved">var</span> noBom] <span class="operator">?</span> <span class="variable">noBom</span> <span class="operator">:</span> <span class="variable local">utf8</span>;

<span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">palindrome</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">list</span>) <span class="operator">=&gt;</span> <span class="variable local">list</span> <span class="control">switch</span>
{
    [] <span class="reserved">or</span> [<span class="reserved">_</span>] <span class="operator">=&gt;</span> <span class="reserved">true</span>,
    [<span class="reserved">var</span> first, .. <span class="reserved">var</span> rest, <span class="reserved">var</span> last] <span class="operator">=&gt;</span> <span class="variable">first</span> <span class="operator">==</span> <span class="variable">last</span> <span class="operator">&amp;&amp;</span> <span class="method">palindrome</span>(<span class="variable">rest</span>),
};
</code></pre>

詳しくは「[リスト パターン](../datatype/patterns.md#list)」で説明します。

## <a id="sec-generated-title-5"></a> <a id="generic-math">Generic Math</a>

インターフェイスの静的メンバーを仮想・抽象にできるようになりました。

この機能の一番の用途は、数値型(`int` や `float` など)に対するアルゴリズムを[ジェネリクス](../oop/sp2_generics.md)を使って書けるようにすることです。
この最大用途にちなんで、
インターフェイスの静的メンバーなどを含む一連の機能を「generic math」と呼んだりしていました。
(コンセプト的な呼び名で、具体的に generic math という名前の文法やライブラリが追加されたわけではありません。)

generic math 関連で、数値型の演算子関連で3つ新機能が追加されています。

* [符号なし右シフト](#unsigned-right-shift)
* [checked 演算子オーバーロード](#checked-operator-overload)
* [シフト演算子の右オペランドの制限撤廃](#relaxing-shift)

### <a id="sec-generated-title-6"></a> <a id="static-abstract">インターフェイスの静的抽象メンバー</a>

まず、インターフェイスの静的メンバーについてですが、
例えば以下のようなコードが書けるようになりました。

<pre class="source" title="ジェネリックな Sum メソッド">
<span class="reserved">using</span> System<span class="operator">.</span>Numerics;

<span class="comment">// よくある「和を取るコード」なものですら、これまでだとジェネリックに書く手段がなかった。</span>
<span class="comment">// C# 11 で可能に。</span>
<span class="reserved">static</span> <span class="type param">T</span> <span class="static"><span class="method">Sum</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">items</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">INumber</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="type param">T</span><span class="operator">.</span><span class="property"><span class="static">Zero</span></span>;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">items</span>) <span class="variable">sum</span> += <span class="variable">x</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}

<span class="comment">// いろんな型に対して sum&lt;T&gt; を呼ぶ。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="method">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">float</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="static"><span class="type">Console<span class="operator"></span></span>.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">double</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">decimal</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
</pre>

(詳しくは「[インターフェイスの静的抽象メンバー](../oop/oo_interface.md#static-abstract)」で説明します。)

### <a id="sec-generated-title-7"></a> <a id="unsigned-right-shift">符号なし右シフト</a>

符号付き整数(`int` とか `sbyte` とか)でも符号なし整数(`uint` とか `byte` とか)でも無関係に、
常に「符号なし右シフト(論理シフト)」をするための `>>>`演算子 (`>` の数が3つ)が追加されました。

<pre class="source" title="C# にも符号なし右シフト演算子を導入">
<code><span class="reserved">using</span> System.Numerics;

<span class="reserved">sbyte</span> <span class="variable">s</span> = -1;

<span class="comment">// ちゃんと符号なし右シフトに。</span>
<span class="comment">// FF → 7F → 3F → 1F → F → 7 → 3 → 1</span>
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 8; <span class="variable">i</span>++)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">s</span>:<span class="string">X</span>}<span class="string">&quot;</span>);
    <span class="variable">s</span> = <span class="method">LogicalRightShift</span>(<span class="variable">s</span>, 1);
}

<span class="comment">// &gt;&gt;&gt; でどの型に対しても符号なし右シフト。</span>
<span class="reserved">static</span> <span class="type">T</span> <span class="method">LogicalRightShift</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">s</span>, <span class="reserved">int</span> <span class="variable">bits</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IShiftOperators</span>&lt;<span class="type">T</span>,<span class="type">T</span>&gt;
    =&gt; <span class="variable">s</span> <em>&gt;&gt;&gt;</em> <span class="variable">bits</span>;
</code></pre>

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#unsigned-right-shift)」で説明します。

### <a id="sec-generated-title-8"></a> <a id="checked-operator-overload">checked 演算子オーバーロード</a>

`operator` キーワードの後ろに `checked` を付けることで、
「`checked` 演算子」を定義できるようになりました。
これにより、ユーザー定義の演算子オーバーロードでも `checked`(オーバーフロー時に例外を投げる)と `unchecked` (オーバーフローしても例外を投げない)を切り替えられるようになります。

<pre class="source" title="checked 演算子オーバーロードの例">
<code><span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type struct">Int2Bit</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">byte</span> <span class="field">Value</span>;
    <span class="reserved">public</span> <span class="type struct">Int2Bit</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">Value</span> <span class="operator">=</span> (<span class="reserved">byte</span>)(<span class="variable local">value</span> <span class="operator">&amp;</span> <span class="number">0b11</span>);
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="field">Value</span><span class="operator">.</span><span class="method">ToString</span>();

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">Int2Bit</span> <span class="method">Checked</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="variable local">value</span> <span class="reserved">is</span> <span class="operator">&lt;</span> <span class="number">2</span> <span class="reserved">and</span> <span class="operator">&gt;=</span> <span class="number">0</span> <span class="operator">?</span> <span class="reserved">new</span>(<span class="variable local">value</span>) <span class="operator">:</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">OverflowException</span>();

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">Int2Bit</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type struct">Int2Bit</span> <span class="variable local">x</span>, <span class="type struct">Int2Bit</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">x</span><span class="operator">.</span><span class="field">Value</span> <span class="operator">+</span> <span class="variable local">y</span><span class="operator">.</span><span class="field">Value</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">Int2Bit</span> <span class="reserved">operator</span> <span class="reserved"><em>checked</em></span> <span class="operator">+</span>(<span class="type struct">Int2Bit</span> <span class="variable local">x</span>, <span class="type struct">Int2Bit</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="method">Checked</span>(<span class="variable local">x</span><span class="operator">.</span><span class="field">Value</span> <span class="operator">+</span> <span class="variable local">y</span><span class="operator">.</span><span class="field">Value</span>);
}
</code></pre>

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#checked-operator-overload)」で説明します。

### <a id="sec-generated-title-9"></a> <a id="relaxing-shift">シフト演算子の右オペランドの制限撤廃</a>

シフト演算子の右オペランドに `int` 以外の型を使えるようになりました。

<pre class="source" title="C# 11 で operator &lt;&lt;(A x, A y) とかが書けるように">
<code><span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// C# 10 以前でも書けるオーバーロード。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="comment">// C# 11 以降でだけ書けるオーバーロード。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <em><span class="type struct">A</span> <span class="variable local">y</span></em>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
}
</code></pre>

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#relaxing-shift)」で説明します。

## <a id="sec-generated-title-10"></a> <a id="file-local"></a>file ローカル型

`file` という修飾子を使って「書いたファイル内からだけアクセスできる型」を作れるようになりました。

<pre class="source" title="file 修飾付きの型を使う例">
<span class="number">1</span><span class="operator">.</span><span class="method">M</span>();

<em><span class="reserved">file</span></em> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=></span> <span class="type"><span class="static">Console</span><span class="operator">.<span class="method"><span class="static">WriteLine</span></span>(<span class="variable local">x</span>);
}
</pre>

これと同じプロジェクト内の別のファイルに以下のようなコードを書いてもエラーにはなりません。

<pre class="source" title="別のファイルに同名の file 修飾付きの型を定義">
<em><span class="reserved">file</span></em> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=></span> <span class="type"><span class="static">Console</span><span class="operator">.<span class="method"><span class="static">WriteLine</span></span>(<span class="string">"別ファイルの file-local Extensions"</span>);
}
</pre>

詳しくは「[file ローカル型](../misc/file-local.md)」で説明します。

## <a id="sec-generated-title-11"></a> <a id="ref-field">ref フィールド</a>

[ref 構造体](../resource/refstruct.md#key-refstruct)のフィールドを [`ref` (参照渡し)](../resource/sp_ref.md#byref)で持てるようになりました。

ref フィールドの書き方は参照引数や参照戻り値と同じく、型の前に `ref` 修飾を付けます。

<pre class="source" title="ref フィールド">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">ByReference</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type param">T</span> <span class="field">Value</span>;
}
</pre>

詳しくは「[ref フィールド](../resource/refstruct.md#ref-field)」で説明します。

## <a id="sec-generated-title-12"></a> <a id="others">その他</a>

### <a id="sec-generated-title-13"></a> <a id="span">ReadOnlySpan に対するパターンマッチ</a>

C# 11 で、`ReadOnlySpan<char>` に対して[文字列リテラルによる定数パターン](../datatype/patterns.md#span)が使えるようになりました。

<pre class="source" title="">
<span class="comment">// string を渡せたところには ReadOnlySpan&lt;char&gt; を渡せるように。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">ReadLine</span></span>();

<span class="comment">// is も</span>
<span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="string">&quot;a&quot;</span>) { }

<span class="comment">// switch ステートメントも</span>
<span class="control">switch</span> (<span class="variable">s</span>)
{
    <span class="control">case</span> <span class="string">&quot;b&quot;</span>:
        <span class="control">break</span>;
}

<span class="comment">// switch 式も OK。</span>
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="variable">s</span> <span class="control">switch</span>
{
    <span class="string">&quot;c&quot;</span> <span class="operator">=&gt;</span> <span class="number">1</span>,
    <span class="reserved">_</span> <span class="operator">=&gt;</span> <span class="number">2</span>,
};
</pre>

### <a id="sec-generated-title-14"></a> <a id="nameof-parameter"></a>nameof(引数) のスコープ変更

[`nameof`](../start/st_string.md#nameof-parameter) にちょっとだけ変更が掛かりました。
以下のように、メソッドに対する属性の中で、そのメソッドの引数の名前が参照できるようになりました。

<pre class="source" title="nameof(引数名)">
<code><span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="comment">// C# 10 までこの属性、 NotNullIfNotNull(&quot;x&quot;) と書かないといけなくて割かしつらかった。</span>
[<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="reserved">nameof</span>(x))]
<span class="reserved">static</span> <span class="reserved">string</span><span class="operator">?</span> <span class="method">m</span>(<span class="reserved">string</span><span class="operator">?</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span>;
</code></pre>

### <a id="sec-generated-title-15"></a> <a id="auto-default">構造体のフィールドの既定値初期化</a>

C# 11 では、構造体でもフィールドの明示的な初期化が不要になりました。
クラスと同じく、明示的に代入しなかったフィールド・自動プロパティには既定値が入ります。

<pre class="source" title="構造体のフィールドが自動的に 0 初期化されるように">
<code><span class="reserved">struct</span> <span class="type struct">Sample</span>
{
    <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">int</span> <span class="field">_y</span>;
    <span class="reserved">int</span> <span class="field">_z</span>;

    <span class="reserved">public</span> <span class="type struct">Sample</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>)
    {
        <span class="method">M</span>(); <span class="comment">// C# 11 では初期化よりも先に読んでも平気。_x, _y にもこの時点でいったん 0 が入ってる。</span>

        <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">x</span>;
        <span class="field">_y</span> <span class="operator">=</span> <span class="variable local">y</span>;
        <span class="comment">// C# 11 では _z に 0 が自動で入る。</span>
    }

    <span class="reserved">void</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="field">_x</span>}<span class="string">, </span>{<span class="field">_y</span>}<span class="string">, </span>{<span class="field">_z</span>}<span class="string">&quot;</span>);
}
</code></pre>

詳しくは「[構造体](../resource/rm_struct.md#auto-default)」や「[既定値](../resource/rm_default.md#auto-default)」で説明します。


### <a id="sec-generated-title-16"></a> <a id="generic-attribute">ジェネリックな属性</a>

[属性をジェネリック クラスにできるようになりました](../dynamic/sp_attribute.md#generic-attribute
)。

<pre class="source" title="C# 11 以降">
<code><span class="comment">// 属性クラスをジェネリックにできるように。</span>
<span class="reserved">class</span> <span class="type">TypeConverter</span>&lt;<span class="type">T</span>&gt; : <span class="type">Attribute</span> { }

<span class="comment">// &lt;&gt; で型引数を指定できる。</span>
[<span class="type">TypeConverter</span>&lt;<span class="type">MyConverter</span>&gt;]
<span class="reserved">class</span> <span class="type">MyClass</span> { }
</code></pre>

### <a id="sec-generated-title-17"></a> <a id="newline-in-interpolation">文字列補間中の改行</a>

[文字列補間](../start/st_string.md#string-interpolation)で、以下のようなコードが書けるようになりました
(`{}` の中で改行を入れれるようになりました)。

<pre class="source" title="{} の中の改行">
<code><span class="reserved">var</span> <span class="variable">a</span> = 1;
<span class="reserved">var</span> <span class="variable">b</span> = 2;
<span class="reserved">var</span> <span class="variable">s</span> = <span class="string">$&quot;</span><span class="string">a: </span>{
    <span class="variable">a</span> <span class="comment">// ここで改行できるのは C# 11 から</span>
    }<span class="string">, b: </span>{<span class="variable">b</span>}<span class="string">&quot;</span>;
</code></pre>

ちなみに、以下のように、`$@` (文字列補間、かつ、逐語的文字列リテラル)を使う場合には C# 10.0 以前でも以下のようなコードが普通に書けました。

<pre class="source" title="$@ なら10以前でもOK">
<code><span class="reserved">var</span> <span class="variable">a</span> = 1;
<span class="reserved">var</span> <span class="variable">b</span> = 2;
<span class="reserved">var</span> <span class="variable">s</span> = <span class="string">$@&quot;</span><span class="string">a: </span>{
    <span class="variable">a</span> <span class="comment">// $@ の場合は C# 10.0 以前でも OK</span>
    }<span class="string">, b: </span>{<span class="variable">b</span>}<span class="string">&quot;</span>;
</code></pre>

「`$""` の場合だけダメだった理由は今となっては思い出せない」というレベルだそうで、
仕様漏れ・バグ修正の類にギリギリの「新機能」になります。

### <a id="sec-generated-title-18"></a> <a id="numeric-intptr">Numeric IntPtr</a>

「C# の新機能」と言っていいのかどうか微妙なラインですが、
[`nint`](ap_ver9.md#nint) に関してちょっとした変更がありました。

C# 9.0 の頃には、`IntPtr`、`UIntPtr` 型に算術演算子の定義がなく、
`nint`、`nuint` に対する演算は C# コンパイラーが特別扱いすることで実装していました。
そのため、
「`nint`、`nuint` は内部的には `IntPtr`、`UIntPtr` としてコンパイルするけども、
`NativeInteger` 属性を付けて `nint`、`nuint` か `IntPtr`、`UIntPtr` を区別する」
みたいなことをしていました。

ところが、 .NET 7 (C# 11 と同世代)では、[generic math](#generic-math) 導入に伴って、
`IntPtr`、`UIntPtr` にも算術演算子が導入されました。
その結果、C# 9.0 時代のような「特別扱い」が不要になったそうです。
そこで C# 11 では、

* .NET 7 移行をターゲットにした場合、`NativeInteger` 属性を付けない
    * 正確に言うと、[`RuntimeFeature` クラス](../../../blog/2018/12/runtimefeature/index.md)を見て分岐
* `NativeInteger` 属性がなくても `nint`、`nuint` と同じ扱いをする

みたいな変更が掛かっています。

一応これが既存のコードに対する破壊的変更になる可能性があって、
例えば、以下のようなコードはこれまで例外が絶対に出なかったのが、C# 11 以降は例外が出る可能性があります。

<pre class="source" title="Numeric IntPtr 関連の破壊的変更">
<span class="reserved">unsafe</span> <span class="reserved">void</span> <span class="method"><span class="warning" title="CS8321">M</span></span>(<span class="reserved">void</span><span class="operator">*</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>)
{
    <span class="reserved">var</span> <span class="variable">p</span> <span class="operator">=</span> <span class="reserved">checked</span>((<span class="type struct">IntPtr</span>)<span class="variable local">x</span>); <span class="comment">// unsigned → singed 変換扱い</span>
    <span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="reserved">checked</span>(<span class="variable">p</span> + <span class="variable local">y</span>);
}
</pre>

### <a id="sec-generated-title-19"></a> <a id="cache-static-method-group">静的メソッドをデリゲート化するときのキャッシュ化</a>

[Numeric IntPtr](#numeric-intptr) の話以上に「C# の新機能と言っていいのかどうか微妙」な話(文法的には何も変わっていないし、挙動も大差ない)ですが、
`Func<int, int> f = Method;` みたいな書き方をしたときに、デリゲートのインスタンスをキャッシュするようになりました。

例えば以下のようなコードを考えます。

<pre class="source" title="ラムダ式と、メソッド グループからのデリゲート化の例">
<span class="comment">// この X と</span>
<span class="reserved">int</span> <span class="method">X</span>(<span class="reserved">int</span>[] <span class="variable local">data</span>) <span class="operator">=&gt;</span> <span class="variable local">data</span><span class="operator">.</span><span class="method">Sum</span>(<span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>);

<span class="comment">// この Y、やってることは一緒。</span>
<span class="reserved">int</span> <span class="method">Y</span>(<span class="reserved">int</span>[] <span class="variable local">data</span>) <span class="operator">=&gt;</span> <span class="variable local">data</span><span class="operator">.</span><span class="method">Sum</span>(<span class="static"><span class="method">square</span></span>);
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">square</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>;
</pre>

C# 10 までは、おおむね以下のようなコードに展開されていました。

<pre class="source" title="C# 10 までの展開結果">
<span class="comment">// ラムダ式だと導入当初からキャッシュが効いてた。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable">_anonymous1</span> <span class="operator">=</span> <span class="reserved">null</span>;

<span class="reserved">int</span> <span class="method">X</span>(<span class="reserved">int</span>[] <span class="variable local">data</span>)
{
    <span class="comment">// こんな感じのコードに展開されてて、 new Func&lt;int, int&gt;() のアロケーションは1回限り。</span>
    <span class="variable">_anonymous1</span> <span class="operator">??=</span> <span class="reserved">new</span> <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(<span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>);
    <span class="control">return</span> <span class="variable local">data</span><span class="operator">.</span><span class="method">Sum</span>(<span class="variable">_anonymous1</span>);
}

<span class="comment">// ところが、メソッド グループを直接渡した場合、都度 new Func&lt;int, int&gt;() してた(C# 10 まで)。</span>
<span class="reserved">int</span> <span class="method">Y</span>(<span class="reserved">int</span>[] <span class="variable local">data</span>)
{
    <span class="comment">// おおむねこういうコードと同じ。</span>
    <span class="reserved">var</span> <span class="variable">f</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(<span class="static"><span class="method">square</span></span>);
    <span class="control">return</span> <span class="variable local">data</span><span class="operator">.</span><span class="method">Sum</span>(<span class="variable">f</span>);
}
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">square</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>;
</pre>

メソッド グループをデリゲート化するとき(`Y` の側)、常に `new Func<int, int>()` のコストがかかっていました。
これが、C# 11 からは以下のような感じのコードに展開されます。

<pre class="source" title="C# 11 からの展開結果">
<span class="comment">// C# 11 で、メソッド グループの場合でも、static なものはキャッシュするようになった。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable">_square</span> <span class="operator">=</span> <span class="reserved">null</span>;

<span class="reserved">int</span> <span class="method">Y</span>(<span class="reserved">int</span>[] <span class="variable local">data</span>)
{
    <span class="comment">// この類のコードになった。ラムダ式の場合のものと一緒。</span>
    <span class="variable">_square</span> <span class="operator">??=</span> <span class="reserved">new</span> <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(<span class="method"><span class="static">square</span></span>);
    <span class="control">return</span> <span class="variable local">data</span><span class="operator">.</span><span class="method">Sum</span>(<span class="variable">_square</span>);
}
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">square</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>;
</pre>

### <a id="sec-generated-title-20"></a> <a id="CS9029">補足: required, scoped, file キーワードと型名</a>

これまでずっと、C# に新しいキーワードを足したいときには、文脈キーワード(特定の状況下でだけキーワード扱いを受ける)にしてきました。

C# 11 で追加される `required`, `scoped`, `file` の3つも文脈キーワードです。
ただ、これらのキーワードは型名と競合しやすい位置に書くことになるので、
型名として使えてしまうと文脈からの弁別が難しくなるようで、
型名として使えなくしたようです。
以下のようにコンパイル エラーになります。

<pre class="source" title="文脈キーワードな型名">
<span class="comment">// 古めの文脈キーワードはクラス名にしても警告にしかならない。</span>
<span class="comment">// 警告の出方も、古いやつは「小文字始まり ASCII のみの型名はやめて欲しい」の CS8981</span>
<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8981">async</span></span> { }
<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8981">await</span></span> { }
<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8981">dynamic</span></span> { }

<span class="comment">// record に関しては専用の警告。CS8860。</span>
<span class="comment">// 今となっては、これもエラーでよかった説はある。</span>
<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8860">record</span></span> { }

<span class="comment">// 最近の文脈キーワードはクラス名にするとエラーにするようにしたみたい。</span>
<span class="reserved">class</span> <span class="error" title="CS9029"><span class="type">required</span></span> { }
<span class="reserved">class</span> <span class="error" title="CS9062"><span class="type">scoped</span></span> { }
<span class="reserved">class</span> <span class="error" title="CS9056"><span class="type">file</span></span> { }

<span class="comment">// ちなみに、この辺りのクラス名をあえて使いたいときは @ を付けとけば OK。</span>
<span class="comment">// (警告にもならない。)</span>
<span class="reserved">class</span> <span class="type">@required</span> { }

<span class="comment">// まあ、@ を付ければ、文脈によらない通常キーワードですら名前に使えるので。</span>
<span class="reserved">class</span> <span class="type">@class</span> { }
</pre>

### <a id="sec-generated-title-21"></a> <a id="pointer-of-managed-types">マネージ型のポインター</a>

C# 11 から、マネージ型のポインターを使えるようになりました。

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

詳しくは「[unsafe](../interop/sp_unsafe.md#pointer-of-managed-types)」で説明します。
