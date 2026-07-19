---
title: "【Generic Math】 C# 11 での演算子の新機能"
source_url: "https://ufcpp.net/study/csharp/oop/generic-math-operators/"
content_type: "Article"
published_at: "2022-06-06T00:00:00"
updated_at: "2022-11-25T00:03:03"
tags: []
umbraco_id: 2428
parent_id: 1248
sort_order: 21
aliases:
  - "/csharp/oop/generic-math-operators/"
---

# 【Generic Math】 C# 11 での演算子の新機能

##<a id="sec-generated-title-1"></a> <a id="abstract">概要</a>
<h5 class="version version11">Ver. 11</h5>

C# 11 で、数値型の演算子関連で3つ新機能が追加されています。

* [符号なし右シフト](#unsigned-right-shift)
* [checked 演算子オーバーロード](#checked-operator-overload)
* [シフト演算子の右オペランドの制限撤廃](#relaxing-shift)

##<a id="sec-generated-title-2"></a> <a id="generic-math">背景: Generic Math</a>
C# 11 / .NET 7 でインターフェイスの静的メンバーを仮想・抽象にできる (static abstract members in interfaces)ようになります。
(この機能自体については「[インターフェイスの静的抽象メンバー](oo_interface.md#static-abstract)」で説明しています。)

この機能の一番の用途は、数値型(`int` や `float` など)に対するアルゴリズムを[ジェネリクス](sp2_generics.md)を使って書けるようにすることです。
例えば、以下のようなコードが書けるようになりました。

<pre class="source" title="ジェネリックに「和を取る」コードを書けるように">
<code><span class="reserved">using</span> System.Numerics;

<span class="comment">// よくある「和を取るコード」なものの、</span>
<span class="comment">// これまでだとジェネリックに書く手段がなかった。</span>
<span class="comment">// C# 11 で可能に。</span>
<span class="comment">// (T.Zero や、T に対する + 演算子の定義ができるように)</span>
<span class="reserved">static</span> <span class="type">T</span> <span class="method">sum</span>&lt;<span class="type">T</span>&gt;(<span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; <span class="variable">items</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">INumber</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">var</span> <span class="variable">sum</span> = <span class="type">T</span>.Zero;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">items</span>) <span class="variable">sum</span> += <span class="variable">x</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}

<span class="comment">// いろんな型に対して sum&lt;T&gt; を呼ぶ。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">sum</span>(<span class="reserved">new</span> <span class="reserved">byte</span>[] { 1, 2, 3, 4, 5 }));
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">sum</span>(<span class="reserved">new</span> <span class="reserved">int</span>[] { 1, 2, 3, 4, 5 }));
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">sum</span>(<span class="reserved">new</span> <span class="reserved">float</span>[] { 1, 2, 3, 4, 5 }));
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">sum</span>(<span class="reserved">new</span> <span class="reserved">double</span>[] { 1, 2, 3, 4, 5 }));
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">sum</span>(<span class="reserved">new</span> <span class="reserved">decimal</span>[] { 1, 2, 3, 4, 5 }));
</code></pre>

加減乗除や論理演算はもちろん、`float` などの一部の型は `Math.Sin` などの数学関数も使えます。
コンセプト的に、この新機能を使ったジェネリックな数値処理の事を Generic Math と呼んでいたりします。

また、 .NET 5 以降、数値関連の型がいくつか追加されています。

* [`Half`](https://docs.microsoft.com/ja-jp/dotnet/api/system.half?WT.mc_id=DT-MVP-4028921): 16ビット浮動小数点数
* [`Int128`, `UInt128`](https://github.com/dotnet/runtime/issues/67151): 128ビットの整数
* [`CLong`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.clong?WT.mc_id=DT-MVP-4028921), [`CULong`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.culong?WT.mc_id=DT-MVP-4028921): C/C++ との相互運用のために使う、環境によってビット幅が違う整数
* [`nint`, `nuint`](../cheatsheet/ap_ver9.md#nint): CPU 依存幅の整数

これらの新しい数値型も、Generic Math の対象で、`INumber<T>` などのインターフェイスを実装しています。

この Generic Math と関連して、数値型の演算子関連で細々とした機能がいくつか追加されています。

* 符号なし右シフト
* checked 演算子オーバーロード
* シフト演算子の右オペランドの制限撤廃

##<a id="sec-generated-title-3"></a> <a id="unsigned-right-shift">符号なし右シフト</a>
右シフト演算には符号付き右シフト(算術シフト)と符号なし右シフト(論理シフト)があって、
右シフトしたときに、最上位ビットの 1 が残るかどうかの差になります。

C# の場合、基本的に、

* 符号<em>付き</em>整数の右シフトは符号<em>付き</em>右シフト(算術シフト)
* 符号<em>なし</em>整数の右シフトは符号<em>なし</em>右シフト(論理シフト)

という方式で右シフトの方式を切り替えます。

<pre class="source" title="右シフトの符号のありなし">
<code><span class="comment">// 符号なし (unsigned) の 0xFF = 255</span>
<span class="reserved">byte</span> <span class="variable">u</span> = 0xFF;

<span class="comment">// 符号付き (signed) の 0xFF = -1</span>
<span class="reserved">sbyte</span> <span class="variable">s</span> = (<span class="reserved">sbyte</span>)<span class="variable">u</span>;

<span class="comment">// 符号なしを右シフトすると、左端には 0 が入る。</span>
<span class="comment">// FF → 7F → 3F → 1F → F → 7 → 3 → 1</span>
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 8; <span class="variable">i</span>++)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">u</span>:<span class="string">X</span>}<span class="string">&quot;</span>);
    <span class="variable">u</span> &gt;&gt;= 1;
}

<span class="comment">// 符号なしを右シフトすると、左端のビットが残る。</span>
<span class="comment">// 元が FF だとずっと FF。</span>
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 8; <span class="variable">i</span>++)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">s</span>:<span class="string">X</span>}<span class="string">&quot;</span>);
    <span class="variable">s</span> &gt;&gt;= 1;
}
</code></pre>

右シフトの符号あり/なしを切り替えたい場合、キャストが必要でした。

<pre class="source" title="byte にキャストしてから右シフトすることで論理シフトに">
<code><span class="reserved">sbyte</span> <span class="variable">s</span> = -1;

<span class="comment">// LogicalRightShift を呼んでいるので、符号なし右シフトになる。</span>
<span class="comment">// FF → 7F → 3F → 1F → F → 7 → 3 → 1</span>
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 8; <span class="variable">i</span>++)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">s</span>:<span class="string">X</span>}<span class="string">&quot;</span>);
    <span class="variable">s</span> = <span class="method">LogicalRightShift</span>(<span class="variable">s</span>, 1);
}

<span class="comment">// 右シフトの符号のあり/なしを切り替えたい場合、キャストを挟む。</span>
<span class="reserved">static</span> <span class="reserved">sbyte</span> <span class="method">LogicalRightShift</span>(<span class="reserved">sbyte</span> <span class="variable">s</span>, <span class="reserved">int</span> <span class="variable">bits</span>)
    =&gt; (<span class="reserved">sbyte</span>)((<span class="reserved">byte</span>)<span class="variable">s</span> &gt;&gt; <span class="variable">bits</span>);
</code></pre>

この方式は、Generic Math の導入に伴って1つ問題がありました。
「型引数 `T` に対応する符号なしな型」を取得する手段がありません。

<pre class="source" title="unsinged generic T を取る手段がない">
<code><span class="comment">// 符号なしシフトにしたかったらどうすれば？？？</span>
<span class="reserved">static</span> <span class="type">T</span> <span class="method">LogicalRightShift</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">s</span>, <span class="reserved">int</span> <span class="variable">bits</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IShiftOperators</span>&lt;<span class="type">T</span>,<span class="type">T</span>&gt;
    =&gt; (<span class="type">T</span>)((<span class="comment">/* unsigned T を取得したいけど手段がない */</span>)<span class="variable">s</span> &gt;&gt; bits);
</code></pre>

そこで、C# 11 では普通に「符号なし右シフト演算子」の `>>>` (`>` 3つ)を導入することにしました。
(Java にあるやつです。Java の場合は `uint` などの符号なし整数型がなくて、`>>` か `>>>` で右シフトを切り替えます。)

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

ちなみに、演算子オーバーロードもできます。

<pre class="source" title="&gt;&gt;&gt; 演算子オーバーロードの例">
<code><span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 4; <span class="variable">i</span>++)
{
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">Int2Bit</span>(<span class="variable">i</span>);

    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span><span class="string">for </span>{<span class="variable">x</span>}<span class="string">&quot;</span>);

    <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">j</span> = 0; <span class="variable">j</span> &lt;= 2; <span class="variable">j</span>++)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">j</span>}<span class="string"> bit signed: </span>{<span class="variable">x</span> &gt;&gt; <span class="variable">j</span>}<span class="string">, unsigned: </span>{<span class="variable">x</span> &gt;&gt;&gt; <span class="variable">j</span>}<span class="string">&quot;</span>);
    }
}

<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Int2Bit</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">byte</span> Value;
    <span class="reserved">public</span> <span class="type">Int2Bit</span>(<span class="reserved">int</span> <span class="variable">value</span>) =&gt; Value = (<span class="reserved">byte</span>)(<span class="variable">value</span> &amp; 0b11);
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; Value.<span class="method">ToString</span>();

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Int2Bit</span> <span class="reserved">operator</span> &gt;&gt;(<span class="type">Int2Bit</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; <span class="reserved">new</span>(<span class="variable">x</span>.Value &gt;&gt; <span class="variable">y</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Int2Bit</span> <span class="reserved">operator</span> <em>&gt;&gt;&gt;</em>(<span class="type">Int2Bit</span> x, <span class="reserved">int</span> y) =&gt; <span class="reserved">new</span>(ExtendSign(x.Value) &gt;&gt; y);
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">ExtendSign</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> &gt;= 0b10 ? (-4 | <span class="variable">x</span>) : <span class="variable">x</span>;
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="checked-operator-overload">checked 演算子オーバーロード</a>
C# では、[整数演算のオーバーフロー時に何もしないか、それとも例外を投げるかを選べる機能](../start/sp_checked.md)があります。

* `checked` コンパイラー オプション: プログラム全域でオーバーフローを例外にする
* `checked` ブロック: ブロック中のオーバーフローを例外にする
* `checked` 式: `checked()` の `()` の中に書いた式でオーバーフローを例外にする

いずれにせよ `checked` というオプション名/キーワードを使います。
これが付いている状況を「`checked` コンテキスト」と言い、
`checked` コンテキストでの演算(要するに例外が出る演算)を 「`checked` 演算」と言います。

逆に、`unchecked` というキーワードで、
「例外を出さない」状態に戻せて、これを「`unchecked` コンテキスト」、「`unchecked` 演算」と言います。
(何も指定がない場合の既定動作は `unchecked` コンテキストになります。)

ちなみに、投げられる例外は `OverflowException` 型です。

<pre class="source" title="checked 演算の例">
<code><span class="reserved">byte</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">128</span>;
<span class="reserved">byte</span> <span class="variable">y</span> <span class="operator">=</span> <span class="number">128</span>;

<span class="comment">// unchecked 演算。</span>
<span class="comment">// (特にオプション指定がない場合、x + y はこの意味。)</span>
<span class="comment">// 128 + 128 = 256 なものの、オーバーフローして 0 に。</span>
<span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="reserved">unchecked</span>(<span class="variable">x</span> <span class="operator">+</span> <span class="variable">y</span>);

<span class="comment">// checked 演算。</span>
<span class="comment">// Overflow 例外が出る。</span>
<span class="reserved">var</span> <span class="variable">w</span> <span class="operator">=</span> <span class="reserved">checked</span>(<span class="variable">x</span> <span class="operator">+</span> <span class="variable">y</span>);

<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>((<span class="variable">w</span>, <span class="variable">z</span>));
</code></pre>

C# 10 以前では、`checked` な演算ができるのは組み込み整数だけでした。
ユーザー定義で `int` などに準ずる型を作ろうとしても、`cheched`/`unchecked` の切り替えはできません。

「`int` などに準ずる型」をどのくらいの頻度で作るかと言われるとあまりなかったりはするんですが…
ちょうど最近(.NET 7 で)、`Int128`/`UInt128` という型が標準ライブラリに追加されています。

また、generic math でも `checked` を使えるようにしたいしたいです。

<pre class="source" title="generic に checked 演算をやりたい例">
<code><span class="comment">// 例外が出るべき。</span>
<span class="method">CheckedAdd</span>&lt;<span class="reserved">byte</span>&gt;(<span class="number">128</span>, <span class="number">128</span>);

<span class="reserved">static</span> <span class="type param">T</span> <span class="method">CheckedAdd</span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">x</span>, <span class="type param">T</span> <span class="variable local">y</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="reserved">struct</span>, System<span class="operator">.</span>Numerics<span class="operator">.</span><span class="type">IAdditionOperators</span>&lt;<span class="type param">T</span>, <span class="type param">T</span>, <span class="type param">T</span>&gt;
{
    <span class="comment">// 例外を出したい。</span>
    <span class="control">return</span> <span class="reserved">checked</span>(<span class="variable local">x</span> + <span class="variable local">y</span>);
}
</code></pre>

これまでのように、組み込み型でだけ例外を出せるということになってしまうと、

* generic に書き換える手段がなくなる
* 「今現在ライブラリ実装なもの(例えば `Int128`)が将来的に組み込み型になる」みたいなことをやりにくくなる

ということになります。

そこで、C# 11 ではユーザー定義の `checked` 演算子オーバーロードを書けるようにしました。
構文としては、

* `operator` キーワードの<em>後ろ</em>に `checked` を付ける
  * `checked` コンテキストで演算子を書いた時に呼ばれる
  * これを便宜上、「checked 演算子」と呼ぶ
* `unchecked` コンテキストで呼ばれて欲しい方には今まで通り何も付けない(`operator` だけ)
  * 「`checked` 演算子」との区別が必要な場合はわざわざ「普通の(regular)演算子」と呼ぶ

となります。

例えば、前節の符号なし右シフトでも使った「2ビット整数」を例に、とりあえず加算演算を書くなら以下のようになります。

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

`checked` 演算子を定義できるのは算術演算系の演算子だけです。
例えば `+` や `-` は `checked` にできますが、`&` や `!` はできません。

「`checked` だけでなく `unchecked` も明示的に書けるようにするかどうか」みたいなことも検討されたんですが、経験上「ほとんどの人が `unchecked` なコードしか書かない」という事がわかっているので、
「`checked` だけ追加して、何も書かない場合(regular)を `unchecked` 扱い」ということになっています。

###<a id="sec-generated-title-5"></a> <a id="checked-only">注意: checked 演算子のみの定義はエラー</a>
ちなみに、通常演算子なしで `checked` 演算子だけを定義することはできません。

<pre class="source" title="checked のみの定義はコンパイル エラーになる">
<code><span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// OK: 通常演算子のみ</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <span class="type struct">A</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="comment">// OK: 通常演算子、checked 演算子両方</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="operator">-</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <span class="type struct">A</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">-</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <span class="type struct">A</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="comment">// コンパイル エラー: checked 演算子のみ</span>
    <span class="comment">// public static A operator *(A x, A y) =&gt; default; // この行もあれば OK。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator"><span class="error">*</span></span>(<span class="type struct">A</span> <span class="variable local">x</span>, <span class="type struct">A</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
}
</code></pre>

###<a id="sec-generated-title-6"></a> <a id="checked-cast">注意: キャスト演算</a>
[キャスト](oo_operator.md#cast)も `checked` にできます。
ただし、`explicit` (明示的型変換)のみ OK で、`implicit` (暗黙的型変換)には `checked` は使えません。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// OK: explicit キャスト</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">explicit</span> <span class="reserved">operator</span> <span class="type struct">A</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">explicit</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="type struct">A</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="comment">// OK: 通常演算子、checked 演算子両方</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type struct">A</span>(<span class="reserved">float</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>; <span class="comment">// これは大丈夫</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="error"><span class="reserved">checked</span> <span class="type struct">A</span></span>(<span class="reserved">float</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>; <span class="comment">// これはダメ</span>
}
</code></pre>

###<a id="sec-generated-title-7"></a> <a id="checked-responsibility">注意: あくまでユーザー裁量</a>
あくまでユーザー定義なので、悪意を持って実装すれば「通常演算子で例外を投げて、checked 演算子で投げない」みたいなこともできてしまいます。

<pre class="source" title="逆に実装">
<code><span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// なぜかこっちが例外を出して</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">explicit</span> <span class="reserved">operator</span> <span class="type struct">A</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">OverflowException</span>();

    <span class="comment">// こっちが出さない実装をしても別に怒られない…</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">explicit</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="type struct">A</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
}
</code></pre>

そこの禁止まではしてないので注意してください。


### <a id="sec-generated-title-8"></a>コンパイル結果
(`>>>` のとこにも同様の話を)

通常演算子は `op_Addition` みたいな名前のメソッドになってる。

checked 演算子は `op_AdditionChecked` みたいに、通常演算子の後ろに `Checked` が付いた名前に

##<a id="sec-generated-title-9"></a> <a id="relaxing-shift">シフト演算子の右オペランドの制限撤廃</a>
C# ではこれまで、シフト演算子の右オペランド(何ビットシフトするかを決める方)には `int` しか使えないという制限がありました。
`<<` や `>>` という記号をシフト以外の意味で使わせるつもりはないのと、
であれば、シフトの右オペランドに `int` 以外のものを使いたい場面がほとんどないためです。

例えば、以下のコードはコンパイル エラーになります。
「1.1 ビットのシフト」とか言われても意味が解らないので、まあこれは妥当な制限でしょう。

<pre class="source" title="右オペランドが整数じゃないのでエラー">
<code><span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span> <span class="error"><span class="operator">&lt;&lt;</span> <span class="number">1.1</span></span>;
</code></pre>

ただ、以下のようなコードもコンパイル エラーになります。
右オペランドが `uint` や `long` の場合ですら制限されていて、
ちょっと厳しい感じがします。

<pre class="source" title="U や L がついてもダメ">
<code><span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="error"><span class="number">1</span> <span class="operator">&lt;&lt;</span> <span class="number">1U</span></span>;
<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="error"><span class="number">1</span> <span class="operator">&lt;&lt;</span> <span class="number">1L</span></span>;
</code></pre>

必要かと言われると別に要らないので、厳しかろうと誰も文句は言わなかったんですが。

ところがここに来て、generic math が入りました。
generic math で使えるメソッドの中にはシフト演算の右オペランドで使えそうなものがいくつかあったりします。
例えば、`LeadingZeroCount` や `TrailingZeroCount` などが代表例でが、
これらの戻り値は `int` ではなく、`TSelf` (型引数になっている型)です。

<pre class="source" title="シフト演算の右オペランドに使えそうな値を返す generic math メソッド">
<code><span class="reserved">using</span> System<span class="operator">.</span>Numerics;

<span class="method">M</span>&lt;<span class="reserved">byte</span>&gt;(<span class="number">0x8</span>);
<span class="method">M</span>&lt;<span class="reserved">byte</span>&gt;(<span class="number">0xF</span>);
<span class="method">M</span>&lt;<span class="reserved">byte</span>&gt;(<span class="number">0x10</span>);
<span class="method">M</span>&lt;<span class="reserved">byte</span>&gt;(<span class="number">0x30</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">x</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IBinaryInteger</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// pop count = 1 になっているビットの個数を求める関数。</span>
    <span class="type param">T</span> <span class="variable">count</span> <span class="operator">=</span> <span class="type param">T</span><span class="operator">.</span><span class="method">PopCount</span>(<span class="variable local">x</span>);

    <span class="comment">// leading zero count = 上位ビットに 0 が何個並んでいるか。</span>
    <span class="type param">T</span> <span class="variable">leading</span> <span class="operator">=</span> <span class="type param">T</span><span class="operator">.</span><span class="method">LeadingZeroCount</span>(<span class="variable local">x</span>);

    <span class="comment">// trailing zero count = 下位ビットに 0 が何個並んでいるか。</span>
    <span class="type param">T</span> <span class="variable">trailing</span> <span class="operator">=</span> <span class="type param">T</span><span class="operator">.</span><span class="method">TrailingZeroCount</span>(<span class="variable local">x</span>);

    <span class="comment">// これらの戻り値が int ではなく T (ジェネリック)。</span>
    <span class="comment">// こういう「ビット数」系の値はシフト演算の右オペランドで使うことがある。</span>

    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>((<span class="variable">count</span>, <span class="variable">leading</span>, <span class="variable">trailing</span>));
}
</code></pre>

これにより、「シフト演算の右オペランドは `int` だけでいい」という前提が崩れました。

まあ、元が厳しすぎたという話なので、C# 11 で制限を撤廃することになりました。
以下のようなコードが認められるようになっています。

<pre class="source" title="C# 11 で operator &lt;&lt;(A x, A y) とかが書けるように">
<code><span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// C# 10 以前でも書けるオーバーロード。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="comment">// C# 11 以降でだけ書けるオーバーロード。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">A</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;</span>(<span class="type struct">A</span> <span class="variable local">x</span>, <em><span class="type struct">A</span> <span class="variable local">y</span></em>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
}
</code></pre>

###<a id="sec-generated-title-10"></a> <a id="shift-guideline">注意: シフト以外の用途で << を使わせたくはない</a>
思想的な話でいうと、
「`<<` や `>>` という記号をシフト以外の意味で使わせるつもりはない」という方針はこれまで通りです。

ただ、構文的な制限はなくなったので、
思想に反するコードも書けるようになっています。
例えば以下のように、悪名高い「`<<` を "write" とか "append" 的な意味で使う」みたいなこともできます。

<pre class="source" title="某言語的な &lt;&lt;">
<code><span class="reserved">using</span> <span class="reserved">static</span> <span class="type">Iostream</span>;

<span class="comment">// C# の思想的には書かせたくないコードの例。</span>
<span class="comment">// 書けてしまうように。</span>
<span class="reserved">_</span> <span class="operator">=</span> <em><span class="field">cout</span> &lt;&lt; <span class="string">&quot;Hellow World!&quot;</span> &lt;&lt; <span class="field">endl</span></em>;

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Iostream</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type struct">ConsoleOut</span> <span class="field">cout</span> <span class="operator">=</span> <span class="reserved">new</span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type struct">ConsoleEndLine</span> <span class="field">endl</span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type struct">ConsoleOut</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">ConsoleOut</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;</span>(<span class="type struct">ConsoleOut</span> <span class="variable local">x</span>, <span class="reserved">string</span> <span class="variable local">value</span>) { <span class="type">Console</span><span class="operator">.</span><span class="method">Write</span>(<span class="variable local">value</span>); <span class="control">return</span> <span class="variable local">x</span>; }
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">ConsoleOut</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;</span>(<span class="type struct">ConsoleOut</span> <span class="variable local">x</span>, <span class="type struct">ConsoleEndLine</span> <span class="variable local">_</span>) { <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(); <span class="control">return</span> <span class="variable local">x</span>; }
    }

    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type struct">ConsoleEndLine</span> { }
}
</code></pre>

ただ、まあこういうコードは推奨されていないというのは今となっては割と有名な話ですし、
いわゆるガイドラインとかベストプラクティス集みたいなドキュメントで「やるべきではない」と書いておけば十分だろいうという判断が下されました。

なので、今回のシフト演算子の制限緩和でも、「`INumber<T>` インターフェイスを実装した型に限る」みたいな制限は掛けない(緩めるのであれば一切の制限をしない)ことになりました。
