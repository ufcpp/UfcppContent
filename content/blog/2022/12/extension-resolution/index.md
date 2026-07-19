---
title: "拡張メソッドは暗黙型変換を見ない"
source_url: "https://ufcpp.net/blog/2022/12/extension-resolution/"
content_type: "BlogEntry"
published_at: "2022-12-08T23:07:13"
updated_at: "2022-12-08T23:07:13"
tags: []
umbraco_id: 2443
parent_id: 2438
sort_order: 2
aliases: []
---

# 拡張メソッドは暗黙型変換を見ない

こないだ、C# で [`(stackalloc T[N]).M()`](../stackalloc-natural-type/index.md) とか書けるという話を書いたわけですが。
その過程で出てきた「そういえばこんなのも」話をもう1個。

[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)の拡張メソッド呼びがちょっと変という話になります。

## 拡張メソッドの解決

[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)の存在意義は、
「語順を変更して、`x.M().N()` みたいな呼び出しができる」という点です。
ほとんどの場合は本当に「語順」だけの問題で、通常のメソッド呼び出しの形でも同じコードが書けます。

<pre class="source" title="同じメソッドを通常のメソッド呼び出しと拡張メソッド呼び出しの両方の書き方をする">
<span class="comment">// 拡張メソッド呼び。</span>
<span class="number">1</span><span class="operator">.</span><span class="method">M</span>();

<span class="comment">// 同じものを通常のメソッド呼び出しで書く。</span>
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">_</span>) { }
}
</pre>

ただ、まあ、通常のメソッド呼びと拡張メソッド呼びでは、ちょっとだけ「解決ルール」みたいなやつが違ったりします。

無変換の場合、つまり、
「`1` を `int` 引数に渡す」とか「`""` を `string` 引数に渡す」みたいなときには変な挙動はしないんですが、
問題は型変換が絡む場合です。

## 解決できる例

先に、大丈夫な例から行きます。

親クラスや、実装するインターフェイスへの変換は問題なく行けて、
拡張メソッド呼び出しもできます。

<pre class="source" title="親クラスや、実装しているインターフェイスへの変換">
<span class="comment">// 親クラスや、実装しているインターフェイスへの変換は、拡張メソッド呼び出しできる。</span>
<span class="number">1</span><span class="operator">.</span><span class="method">Object</span>();
<span class="number">1</span><span class="operator">.</span><span class="method">Interface</span>();

<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">Object</span></span>(<span class="number">1</span>);
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="static"><span class="method">Interface</span></span>(<span class="number">1</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Object</span></span>(<span class="reserved">this</span> <span class="reserved">object</span> <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Interface</span></span>(<span class="reserved">this</span> <span class="type">IComparable</span> <span class="variable local">_</span>) { }
}
</pre>

オーバーロードがあるときには
「階層が近い方優先」で、これも通常メソッド呼び・拡張メソッド呼びで共通です。

<pre class="source" title="オーバーロードは階層が近い方優先">
<span class="comment">// どっちも IComparable の方が呼ばれる。</span>
<span class="number">1</span><span class="operator">.</span><span class="method">M</span>();
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="number">1</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">object</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;object&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type">IComparable</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;IComparable&quot;</span>);
}
</pre>

## ユーザー定義の型変換

拡張メソッドの解決時、ユーザー定義の型変換はみません。
一方で、通常のメソッド解決の時には見るので、
「拡張メソッド呼びだけできない」みたいなことがあります。

標準ライブラリでいうと、`DateTime` → `DateTimeOffset` とか、
`Span<T>` → `ReadOnlySpan<T>` とか、
`string` → `ReadOnlySpan<char>` とかがあります。

<pre class="source" title="拡張メソッド呼びできない例: ユーザー定義の型変換">
<span class="comment">// 通常のメソッドとしてなら呼べる。</span>
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="string">&quot;&quot;</span>);                 <span class="comment">// string → ReadOnlySpan&lt;char&gt;</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">stackalloc</span> <span class="reserved">char</span>[<span class="number">1</span>]); <span class="comment">// Span&lt;char&gt; → ReadOnlySpan&lt;char&gt;</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="type struct">DateTime</span><span class="operator">.</span><span class="property"><span class="static">Now</span></span>);       <span class="comment">// DateTime → DateTimeOffset</span>

<span class="comment">// 拡張メソッドでは呼べない…</span>
<span class="string"><span class="error" title="CS1929">&quot;&quot;</span></span><span class="operator">.</span>M();
(<span class="error" title="CS1929"><span class="reserved">stackalloc</span> <span class="reserved">char</span>[<span class="number">1</span>]</span>)<span class="operator">.</span>M();
<span class="error" title="CS1929"><span class="type struct">DateTime</span><span class="operator">.</span><span class="static"><span class="property">Now</span></span></span><span class="operator">.</span>M();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type struct">DateTimeOffset</span> <span class="variable local">_</span>) {}
}
</pre>

これがまあ、[こないだのブログ](../stackalloc-natural-type/index.md)とのつながりでして。
「`(stackalloc char[1]).M()` が呼べない？そうだっけ？」からの、
「`ReadOnly` を削ったら呼べた」ということがありました。

## ターゲットからの型推論

ターゲットからの型推論系の処理も、拡張メソッドでは働きません。

`new()`、`default` 辺りはダメです。

<pre class="source" title="">
<span class="comment">// 通常のメソッドとしてなら呼べる。</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">new</span>());      <span class="comment">// new object()</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">default</span>); <span class="comment">// null</span>

<span class="comment">// 拡張メソッド前に型推論は働かない。</span>
<span class="comment">// エラーに。</span>
<span class="error" title="CS8754"><span class="reserved">new</span>()</span><span class="operator">.</span>M();
<span class="reserved"><span class="error" title="CS8716">default</span></span><span class="operator">.</span>M();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">object</span><span class="operator">?</span> <span class="variable local">_</span>) {}
}
</pre>

## ターゲットからの型推論 + 自然な型

ターゲットからの型推論は効かないものの、
自然な型を持っているやつはどうなるかというと…

基本的に、自然な型の時だけは拡張メソッド呼びもできます。

<pre class="source" title="自然な型の時なら拡張メソッドが呼べる">
<span class="comment">// 通常のメソッドとして、当然呼べる。</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>);
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span>);

<span class="comment">// 整数リテラルの自然な型は int で、 int の拡張メソッドなら呼べる。</span>
<span class="number">1</span><span class="operator">.</span><span class="method">M</span>();

<span class="comment">// 同、string の拡張メソッドなら呼べる。</span>
<span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span><span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">_</span>) {}
}
</pre>

例えば整数リテラルは `short` や `byte` 型に変換できますし、
文字列補間 `$""` は `IFormattable` や文字列補間ハンドラーに変換できます。
ところが、こういう場合は拡張メソッド呼びできません。

<pre class="source" title="ターゲットからの型判定がかかるような例では拡張メソッドは呼べない">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// 通常のメソッドとしてなら呼べる。</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>);
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span>);

<span class="comment">// ターゲットからの型判定がかかるような例では拡張メソッドは呼べない。</span>
<span class="number"><span class="error" title="CS1929">1</span></span><span class="operator">.</span>M();
<span class="error" title="CS1929"><span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span></span><span class="operator">.</span>M();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">byte</span> <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type struct">DefaultInterpolatedStringHandler</span> <span class="variable local">_</span>) {}
}
</pre>

### ラムダ式の自然な型

ちなみに、自然な型決定できるようになったにもかかわらず、
ラムダ式は自然な型に対しても拡張メソッド呼びはできません。
これは意図的で、`() => {}.M()` みたいな文法を認めたくなかったみたいです。
`(() => {}).M()` でもダメ。 

<pre class="source" title="ラムダ式のときは自然な型に対しても拡張メソッド呼び不可">
<span class="comment">// これは行ける。</span>
<span class="comment">// 何なら Delegate とか object 引数相手でもこう書ける。</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(() <span class="operator">=&gt;</span> { });

<span class="comment">// これはダメ。</span>
<span class="comment">// 自然な型は Action なはずだけど。</span>
<span class="error" title="CS0023">(() <span class="operator">=&gt;</span> { })<span class="operator">.</span>M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="type">Action</span> <span class="variable local">_</span>) {}
}
</pre>

### 特殊なオーバーロード解決順序

文字列補間のオーバーロード解決順序はちょっと特殊です。

C# 10 でパフォーマンス改善のために[ハンドラー パターン](../../../../study/csharp/start/improvedinterpolatedstring.md)を導入したわけですが、
その時に検討された内容:

* たいていのクラスがすでに `string` のオーバーロードを持っている
* 普通に考えれば `$""` の自然な型は `string` で、オーバーロード解決でも `string` 引数が優先されるべき
* ところが `string` オーバーロードが呼ばれたらパフォーマンス改善されない
* 何なら C# 6 で[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)を導入したときにも、`IFormattable` オーバーロードが呼ばれなくて困った

このような背景がありまして。
結果的に、「文字列補間ハンドラーがあれば、それを優先的に使う」という特殊処理が挟まっています。

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// ハンドラー優先の特殊処理が働く。</span>
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span>); <span class="comment">// interpolation の方が呼ばれる</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;string&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">DefaultInterpolatedStringHandler</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;interpolation&quot;</span>);
}
</pre>

これは本当に特殊処理です。
例えば、整数リテラルの場合は普通に int が優先されます。

<pre class="source" title="int 優先">
<span class="comment">// 普通に考えれば「自然な型」優先。</span>
<span class="comment">// 実際、整数リテラルは int 優先。</span>
<span class="static"><span class="type">Ex</span></span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="number">1</span>); <span class="comment">// int が呼ばれる</span>

<span class="comment">// int におさまらない桁のリテラルを書くと long リテラルになって、long オーバーロードが呼ばれるのに。</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">0x1_0000_0000</span>); <span class="comment">// long が呼ばれる。</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;int&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">byte</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;byte&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">long</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;long&quot;</span>);
}
</pre>

で、この `$""` に対する特殊処理が、拡張メソッド解決の際には働かないということは…
以下のように、通常メソッド呼びと拡張メソッド呼びで呼ばれるオーバーロードが変わるという症状を起こします。

<pre class="source" title="$&quot;&quot; の拡張メソッド呼び">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// ハンドラー優先の特殊処理が働く。</span>
<span class="type"><span class="static">Ex</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span>); <span class="comment">// interpolation の方が呼ばれる</span>

<span class="comment">// そしてその特殊処理は、拡張メソッド解決時には働かない！</span>
<span class="string">$&quot;</span>{<span class="number">1</span>}<span class="string">&quot;</span><span class="operator">.</span><span class="method">M</span>(); <span class="comment">// string の方が呼ばれる！</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Ex</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;string&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type struct">DefaultInterpolatedStringHandler</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;interpolation&quot;</span>);
}
</pre>

特殊処理が挟まった背景を知らないと意味が分からない仕様ですよね。
一応、バグじゃなくて仕様通りです。
