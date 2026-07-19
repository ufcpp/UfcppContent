---
title: "C# 10.0 の補間文字列の改善"
source_url: "https://ufcpp.net/study/csharp/start/improvedinterpolatedstring/"
content_type: "Article"
published_at: "2021-09-22T00:00:00"
updated_at: "2021-09-23T00:00:00"
tags: []
umbraco_id: 2362
parent_id: 1190
sort_order: 10
aliases:
  - "/csharp/start/improvedinterpolatedstring/"
---

# C# 10.0 の補間文字列の改善

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<h5 class="version version10">Ver. 10</h5>

C# 10.0 で、補間文字列(interpolated string)のコンパイル結果に変更が掛かって、
これまでよりもかなり高速化されました。
詳細は気にせず単に高速化の恩恵だけを受けたい場合、
言語バージョン、SDK バージョンを C# 10.0/.NET  6.0 にアップデートして再コンパイルするだけで速くなります。

一方、本項では、
C# 9.0 までの補間文字列の問題点と、
C# 10.0 から補間文字列がどのように展開されるかについて説明します。
仕組みがわかれば、補間文字列の解釈を結構自由にカスタマイズすることができます。

サンプル コード: [InterpolatedStrings](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2021/Csharp10/InterpolatedStrings)

## <a id="sec-generated-title-2"></a> <a id="csharp9"></a>C# 9.0 までの補間文字列

例えば以下のようなコードがあったとします。

<pre class="source" title="補間文字列の例">
<code><span class="reserved">static</span> <span class="reserved">string</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c, <span class="reserved">int</span> d) =&gt; <span class="string">$"</span>{a}<span class="string">.</span>{b}<span class="string">.</span>{c}<span class="string">.</span>{d}<span class="string">"</span>;
</code></pre>

C# 9.0 までは、このコードは以下のように展開されていました。

<pre class="source" title="string.Format への展開">
<code><span class="reserved">static</span> <span class="reserved">string</span> m(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c, <span class="reserved">int</span> d) =&gt; <span class="reserved">string</span>.<span class="method">Format</span>(<span class="string">"{0}.{1}.{2}.{3}"</span>, a, b, c, d);
</code></pre>

要は `string.Format` メソッド呼び出しへの展開でした。
ちなみに、ここで呼ばれている `Format` メソッドは以下のようなオーバーロードです。

<pre class="source" title="Format(format, args)">
<code><span class="reserved">public static string</span> <span class="method">Format</span>(<span class="reserved">string</span> format, <span class="reserved">params</span> <span class="reserved">object</span>?[] args)
</code></pre>

この展開方法では以下のようなコストがどうしても避けられず、用途によっては使うのがためらわれていました。

* [`params`](../structured/sp_params.md) を介していて、`new object[4]` のコストが発生する
* `object` を介していて、`int` などの値を渡すと[ボックス化](../resource/rmboxing.md) のコストが発生する
* (ログレベルの変更などで)実際には文字列を全く使わない状況でも必ず文字列インスタンスが作られる
* [`Span` 構造体](../resource/span.md)を渡せない

そこで、C# 10.0 では以下のように、`AppendLiteral`, `AppendFormatted` メソッドを何度も呼び出す方針に変更されました。

<pre class="source" title="C# 10.0 での文字列補間の展開結果の例">
<code><span class="type">DefaultInterpolatedStringHandler</span> handler = <span class="reserved">new</span> <span class="type">DefaultInterpolatedStringHandler</span>(3, 4);
handler.<span class="method">AppendFormatted</span>(a);
handler.<span class="method">AppendLiteral</span>(<span class="string">"."</span>);
handler.<span class="method">AppendFormatted</span>(b);
handler.<span class="method">AppendLiteral</span>(<span class="string">"."</span>);
handler.<span class="method">AppendFormatted</span>(c);
handler.<span class="method">AppendLiteral</span>(<span class="string">"."</span>);
handler.<span class="method">AppendFormatted</span>(d);
<span class="reserved">string</span> s = handler.<span class="method">ToStringAndClear</span>();
</code></pre>

## <a id="sec-generated-title-3"></a> <a id="handler-pattern"></a>ハンドラー パターン

前述の通り、C# 10.0 からは補間文字列(`$""`)を`AppendFormatted`や`AppendLiteral`メソッドに展開します。
これは[パターン ベース](../misc/miscpatternbased.md#key-pattern-based)になっていて、
所定のパターンを満たしていればどんな型であっても可能です。

まず、以下の条件を満たす型を補間文字列ハンドラー (interpolated string handler)と呼びます。
(以下、このページ内では単に「ハンドラー型」と呼びます。)

* `InterpolatedStringHandler` 属性(`System.Runtime.CompilerServices`名前空間)が付いている
* 最低限、以下の引数を持つコンストラクターを持つ
    * `int literalLength`: 補間文字列のリテラル部分(`$""` の中から `{}` を除いた部分)の文字列長
    * `int formattedCount`: `{}` (interpolation hole: 補間穴)の個数
    * 追加で、`out bool` なアウト引数を持てる
    * `InterpolatedStringHandlerArgument` 属性と組み合わせ得て、追加で任意の引数を足せる
* リテラル部分を書き込むための `AppendLiteral(string)` メソッドを持つ
    * `void` か `bool` 戻り値(後述)
* `{}` の部分を書き込むための `AppendFormatted(T)' メソッドを持つ
    * `void` か `bool` 戻り値(後述)
    * 追加で `int alignment` 引数(フォーマット時の幅指定)を持てる
    * 追加で `string format` 引数(フォーマット指定文字列)を持てる

最低ライン必要なメンバーをそろえた型を作ると以下のようになります。
(本当に「コンパイルが通る」レベルで、中身が何もないので `Dummy` という名前にしてあります。)

<pre class="source" title="補間文字列ハンドラーに必要な最低限だけ持った型の例">
<code>
[System.Runtime.CompilerServices.<span class="type">InterpolatedStringHandler</span>]
<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">DummyHandler</span>
{
    <span class="reserved">public</span> <span class="type">DummyHandler</span>(<span class="reserved">int</span> literalLength, <span class="reserved">int</span> formattedCount) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendLiteral</span>(<span class="reserved">string</span> s) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) { }
}
</code></pre>

### <a id="sec-generated-title-4"></a> <a id="assign-to-handler"></a>ハンドラー型への直接代入

まず、補間文字列をハンドラー型に直接渡す場合、
コンストラクター、`AppendLiteral`、`AppendFormatted` メソッドの呼び出しに展開されます。

例えば以下のようなコードがあるとき、

<pre class="source" title="補間文字列をハンドラー型に直接渡す例">
<code><span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b)
{
    <span class="type">DummyHandler</span> h = <span class="string">$"</span>{a}<span class="string"> / </span>{b}"</span>;
}
</code></pre>

以下のように展開されます。

<pre class="source" title="補間文字列をハンドラー型に直接渡す例の展開結果">
<code><span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b)
{
    <span class="type">DummyHandler</span> temp = <span class="reserved">new</span>(3, 2);
    temp.<span class="method">AppendFormatted</span>(a);
    temp.<span class="method">AppendLiteral</span>(<span class="string">" / "</span>);
    temp.<span class="method">AppendFormatted</span>(b);
    <span class="type">DummyHandler</span> h = temp;
}
</code></pre>

### <a id="sec-generated-title-5"></a> <a id="assign-to-string"></a>string への代入

`string` 型は特殊で、補間文字列を `string` 型に渡す場合、
以下のような展開が行われます。

* `DefaultInterpolatedStringHandler` 型(`System.Runtime.CompilerServices` 名前空間)が利用可能な場合
    * まず、この型に対する代入処理と同様に `AppendLiteral`、`AppendFormatted` メソッドを呼び出す
    * 最後に `DefaultInterpolatedStringHandler.ToStringAndClear` メソッドを呼んで文字列化する
* 利用できない場合、`string.Format` に展開する(C# 9.0 までの挙動と同じ)

`DefaultInterpolatedStringHandler` 型が存在するならほとんどの場合はこれを利用可能です。
そして、この型は .NET 6.0 からは標準ライブラリに入っています。
例えば以下のようなコードを書いて .NET 6.0 向けにコンパイルした場合、

<pre class="source" title="補間文字列を string 型に渡す例">
<code><span class="reserved">string</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b) =&gt; <span class="string">$"</span>{a}<span class="string"> / </span>{b}<span class="string">"</span>;
</code></pre>

以下のように展開されます。
(`DefaultInterpolatedStringHandler` 型への代入の展開結果 + `ToStringAndClear` 呼び出しみたいなコードになります。)

<pre class="source" title="補間文字列を string 型に渡す例の展開結果">
<code><span class="reserved">string</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b)
{
    <span class="type">DefaultInterpolatedStringHandler</span> h = <span class="reserved">new</span>(3, 2);
    h.<span class="method">AppendFormatted</span>(a);
    h.<span class="method">AppendLiteral</span>(<span class="string">" / "</span>);
    h.<span class="method">AppendFormatted</span>(b);
    <span class="reserved">return</span> <em>h.<span class="method">ToStringAndClear</span>()</em>;
}
</code></pre>

`DefaultInterpolatedStringHandler` 型自体は存在するのに補間文字列として利用できない状況は、
補間穴(`{}`)の中に [`await`](../async/sp5_async.md#async) を含む場合などです。
`DefaultInterpolatedStringHandler` 型は [ref 構造体](../resource/refstruct.md)なので、`await` と共存できません。
例えば以下のようなコードを書くと `string.Format` に展開されます。

<pre class="source" title="DefaultInterpolatedStringHandler に展開できない補間文字列の例">
<code><span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; <span class="method">m</span>(<span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; a) =&gt; <span class="string">$"result: </span>{<em><span class="reserved">await</span> a</em>}<span class="string">"</span>;
</code></pre>

ちなみに、`DefaultInterpolatedStringHandler` 型は標準ライブラリ中のものでなくても構いません。
もし .NET 5.0 以前をターゲットにした場合でも同様の最適化が掛かって欲しいなら、
`DefaultInterpolatedStringHandler` 型を移植すれば可能です。
.NET 6.0 にしかない機能をちらほら使っているので 5.0 以前への移植は[多少面倒ですが、できなくはないレベルかと思います](https://github.com/ufcpp/UfcppSample/issues/355#issuecomment-916822451)。

### <a id="sec-generated-title-6"></a> <a id="AppendFormatted-overload"></a>AppendFormatted メソッドのオーバーロード

ハンドラー型を作る際、`AppendFormatted` メソッドはいくつオーバーロードがあっても構いません。
よく使いそうなのは、ジェネリック型引数として使えない `ReadOnlySpan<char>` や、
その他最適化のために具象型を直接受け取りたい場合(`string` など)用のオーバーロードなどです。

<pre class="source" title="AppendFormatted のオーバーロードを増やす例">
<code><span class="type">DummyHandler</span> h = <span class="string">$"</span>{123}<span class="string">, </span>{<span class="string">"abc"</span>}<span class="string">, </span>{<span class="reserved">stackalloc</span> <span class="reserved">char</span>[1]}<span class="string">"</span>;

[System.Runtime.CompilerServices.<span class="type">InterpolatedStringHandler</span>]
<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">DummyHandler</span>
{
    <span class="reserved">public</span> <span class="type">DummyHandler</span>(<span class="reserved">int</span> literalLength, <span class="reserved">int</span> formattedCount) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendLiteral</span>(<span class="reserved">string</span> s) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"(literal)"</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"ジェネリック版"</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>(<span class="reserved">string</span> x) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"string 版"</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>(<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; x) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"ReadOnlySpan 版"</span>);
}
</code></pre>

<pre class="console" title="AppendFormatted のオーバーロードを増やす例">
<code>ジェネリック版
(literal)
string 版
(literal)
ReadOnlySpan 版
</code></pre>

### <a id="sec-generated-title-7"></a> <a id="formatting"></a>書式指定

補間文字列の `{}` の中では[書式指定](st_string.md#formatting)ができます。
(ハンドラー型が使える状況下で)書式指定した場合、`AppendFormatted` メソッドの第2、第3引数に書式が渡ります。
例えば以下のようなコードを書いた場合、

<pre class="source" title="書式指定付きの補間文字列の例">
<code><span class="reserved">string</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c) =&gt; <span class="string">$"(</span>{a<em>, 8:<span class="string">X</span></em>}<span class="string">) (</span>{b<em>:<span class="string">X</span></em>}<span class="string">) (</span>{c<em>,4</em>}<span class="string">)"</span>;
</code></pre>

以下のように展開されます。

<pre class="source" title="書式指定付きの補間文字列の例の展開結果">
<code><span class="reserved">string</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c)
{
    <span class="type">DefaultInterpolatedStringHandler</span> h = <span class="reserved">new</span>(8, 3);
    h.<span class="method">AppendLiteral</span>(<span class="string">"("</span>);
    h.<span class="method">AppendFormatted</span>(a, <em>8, <span class="string">"X"</span></em>);
    h.<span class="method">AppendLiteral</span>(<span class="string">") ("</span>);
    h.<span class="method">AppendFormatted</span>(b, <em><span class="string">"X"</span></em>);
    h.<span class="method">AppendLiteral</span>(<span class="string">") ("</span>);
    h.<span class="method">AppendFormatted</span>(c, <em>4</em>);
    h.<span class="method">AppendLiteral</span>(<span class="string">")"</span>);
    <span class="reserved">return</span> h.ToStringAndClear();
}
</code></pre>

ハンドラー型を自作する場合、`AppendFormatted` メソッドの引数は、
以下のようにオーバーロードをいくつか用意しても構いませんし、

<pre class="source" title="AppendFormatted メソッドの引数の例(オーバーロードをいくつか用意)">
<code>    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="reserved">int</span> alignment) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="reserved">string</span> format) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="reserved">int</span> alignment, <span class="reserved">string</span> format) { }
</code></pre>

以下のようにオプション引数で1つのメソッドにまとめても構いません。

<pre class="source" title="AppendFormatted メソッドの引数の例(オプション引数)">
<code>    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="reserved">int</span>? alignment = <span class="reserved">null</span>, <span class="reserved">string</span>? format = <span class="reserved">null</span>) { }
</code></pre>

### <a id="sec-generated-title-8"></a> <a id="bool-return"></a>bool 戻り値

ハンドラー型のコンストラクターでは第3引数に `out bool` を、
`AppendLiteral`、`AppendFormatted` メソッドでは戻り値として `bool` を返すことができます。
この場合、false が返ってきたら処理を途中で打ち切るようなコードに展開されます。
例えば以下のようなハンドラー型があったとします。

<pre class="source" title="bool 戻り値を持つ補間文字列ハンドラー型の例">
<code>[<span class="type">InterpolatedStringHandler</span>]
<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">DummyHandler</span>
{
    <span class="reserved">public</span> <span class="type">DummyHandler</span>(<span class="reserved">int</span> literalLength, <span class="reserved">int</span> formattedCount, <span class="reserved">out</span> <span class="reserved">bool</span> result) =&gt; result = <span class="reserved">true</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">AppendLiteral</span>(<span class="reserved">string</span> s) =&gt; <span class="reserved">true</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">AppendFormatted</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) =&gt; <span class="reserved">true</span>;
}
</code></pre>

このハンドラー型に対して、例えば以下のように補間文字列を渡した場合、

<pre class="source" title="bool 戻り値を持つ補間文字列ハンドラー型の利用例">
<code><span class="type">DummyHandler</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c, <span class="reserved">int</span> d) =&gt; <span class="string">$"</span>{a}<span class="string">.</span>{b}<span class="string">.</span>{c}<span class="string">.</span>{d}<span class="string">"</span>;
</code></pre>

以下のような展開結果になります。

<pre class="source" title="bool 戻り値を持つ補間文字列ハンドラー型の利用例の展開結果">
<code><span class="type">DummyHandler</span> <span class="method">m</span>(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c, <span class="reserved">int</span> d)
{
    <span class="type">DummyHandler</span> h = <span class="reserved">new</span>(3, 4, <span class="reserved">out</span> <span class="reserved">var</span> result);
    <span class="reserved">if</span> (result
        &amp;&amp; h.<span class="method">AppendFormatted</span>(a)
        &amp;&amp; h.<span class="method">AppendLiteral</span>(<span class="string">"."</span>)
        &amp;&amp; h.<span class="method">AppendFormatted</span>(b)
        &amp;&amp; h.<span class="method">AppendLiteral</span>(<span class="string">"."</span>)
        &amp;&amp; h.<span class="method">AppendFormatted</span>(c)
        &amp;&amp; h.<span class="method">AppendLiteral</span>(<span class="string">"."</span>))
        h.<span class="method">AppendFormatted</span>(d);
    <span class="reserved">return</span> h;
}
</code></pre>

これを使って、例えば、「一定文字数を超えたらそこで処理を打ち切り」とか、
「ログ レベル的に全く文字列化処理が必要ない場合、 `AppendLiteral`/`AppendFormatted` を一切呼ばない」とかができます。

### <a id="sec-generated-title-9"></a> <a id="InterpolatedStringHandlerArgument"></a>InterpolatedStringHandlerArgument 属性

`InterpolatedStringHandlerArgument` 属性(`System.Runtime.CompilerServices` 名前空間)を使って、
ハンドラー型のコンストラクターに追加の引数を渡すことができます。
例えば以下のような使い方をします。
(実際、`DefaultInterpolatedStringHandler` がそういう使い方をしています。)

* カルチャー指定して文字列を作りたいとき用に、引数で `IFormatProvider` を渡す
* 文字列を作る際に使うバッファーとして外から `Span<char>` を渡す

これを使うためにはまず、以下のようにコンストラクターに追加の引数を持ったハンドラー型を作ります。

<pre class="source" title="コンストラクターに追加の引数を持ったハンドラー型">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;

[<span class="type">InterpolatedStringHandler</span>]
<span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">DummyHandler</span>
{
    <span class="reserved">public</span> <span class="type">DummyHandler</span>(<span class="reserved">int</span> literalLength, <span class="reserved">int</span> formattedCount) : <span class="reserved">this</span>(literalLength, formattedCount, <span class="reserved">null</span>, <span class="reserved">default</span>) { }

    <span class="comment">// 追加の引数持ち</span>
    <span class="reserved">public</span> <span class="type">DummyHandler</span>(<span class="reserved">int</span> literalLength, <span class="reserved">int</span> formattedCount, <span class="type">IFormatProvider</span>? provider)
        : <span class="reserved">this</span>(literalLength, formattedCount, provider, <span class="reserved">default</span>) { }

    <span class="reserved">public</span> <span class="type">DummyHandler</span>(<span class="reserved">int</span> literalLength, <span class="reserved">int</span> formattedCount, <span class="type">IFormatProvider</span>? provider, <span class="type">Span</span>&lt;<span class="reserved">char</span>&gt; initialBuffer)
    <span class="comment">// 以下略</span>
}
</code></pre>

次に、以下のように、`InterpolatedStringHandlerArgument` 属性を使って、メソッドの引数とハンドラー型のコンストラクター引数の結び付けるメソッドを書きます。

<pre class="source" title="InterpolatedStringHandlerArgument 属性を使った引数の結び付け">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Formatter</span>
{
    <span class="comment">// 追加の引数なし。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Format</span>(<span class="type">DummyHandler</span> handler)
    <span class="comment">// 省略</span>

    <span class="comment">// provider を追加。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Format</span>(
        <span class="type">IFormatProvider</span> provider,
        [<span class="type">InterpolatedStringHandlerArgument</span>(<span class="string">"provider"</span>)] <span class="type">DummyHandler</span> handler)
        =&gt; <span class="method">Format</span>(handler);

    <span class="comment">// provider と initialBuffer を追加。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Format</span>(
        <span class="type">IFormatProvider</span> provider, <span class="type">Span</span>&lt;<span class="reserved">char</span>&gt; initialBuffer,
        [<span class="type">InterpolatedStringHandlerArgument</span>(<span class="string">"provider"</span>, <span class="string">"initialBuffer"</span>)] <span class="type">DummyHandler</span> handler)
        =&gt; <span class="method">Format</span>(handler);
}
</code></pre>

そしてこれらのメソッドを呼ぶと、ハンドラー型に追加の引数が渡るようになります。

<pre class="source" title="ハンドラー型に引数を渡す例">
<code><span class="reserved">using</span> System.Globalization;

<span class="comment">// Format(DummyHandler) を呼んでて、</span>
<span class="comment">// new DummyHandler(5, 2) が作られる。</span>
<span class="type">Formatter</span>.<span class="method">Format</span>(<span class="string">$"abc </span>{1}<span class="string"> </span>{2}<span class="string">"</span>);

<span class="comment">// Format(IFormatProvider, DummyHandler) を呼んでて、</span>
<span class="comment">// new DummyHandler(5, 2, CultureInfo.InvariantCulture) が作られる。</span>
<span class="type">Formatter</span>.<span class="method">Format</span>(<span class="type">CultureInfo</span>.InvariantCulture, <span class="string">$"abc </span>{1}<span class="string"> </span>{2}<span class="string">"</span>);

<span class="comment">// Format(IFormatProvider, Span&lt;char&gt;, DummyHandler) を呼んでて、</span>
<span class="comment">// new DummyHandler(5, 2, CultureInfo.InvariantCulture, stackalloc char[128]) が作られる。</span>
<span class="type">Formatter</span>.<span class="method">Format</span>(<span class="type">CultureInfo</span>.InvariantCulture, <span class="reserved">stackalloc</span> <span class="reserved">char</span>[128], <span class="string">$"abc </span>{1}<span class="string"> </span>{2}<span class="string">"</span>);
</code></pre>

## <a id="sec-generated-title-10"></a> <a id="overload-resolution"></a>オーバーロード解決

C# 10.0 でハンドラー型の仕様が追加され、
C# 9.0 まででも [FormattableString](st_string.md#FormattableString) の仕様があるので、
補間文字列を受け取る候補となるメソッドを3つ同時に定義できます。

<pre class="source" title="補間文字列を受け取る候補となる3つのメソッド">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">DefaultInterpolatedStringHandler</span> _) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"handler"</span>);
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> _) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"string"</span>);
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">IFormattable</span> _) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"formattable"</span>);
</code></pre>

こういう状況では、ハンドラー型 > `string` 型 > FormattableString 
(ハンドラー型が一番呼ばれやすい) という優先順位になります。

<pre class="source" title="ハンドラー型 &gt; string &gt; FormattableString">
<code><span class="comment">// ハンドラー型最優先。</span>
<span class="method">M</span>(<span class="string">$"</span>{1}<span class="string">"</span>); <span class="comment">// handler</span>

<span class="comment">// ただの文字列の場合は string に行く。</span>
<span class="method">M</span>(<span class="string">"abc"</span>); <span class="comment">// string</span>

<span class="comment">// ちょっと混乱しそうなのが、const になる場合に限り、 $ がついてても string 行き。</span>
<span class="method">M</span>(<span class="string">$""</span>); <span class="comment">// string</span>
<span class="method">M</span>(<span class="string">$"abc </span>{<span class="string">"abc"</span>}<span class="string"> abc"</span>); <span class="comment">// string</span>

<span class="comment">// もちろん、キャストしてしまえば任意に呼び分け可能。</span>
<span class="method">M</span>(<span class="string">$"</span>{1}<span class="string">"</span>); <span class="comment">// handler</span>
<span class="method">M</span>((<span class="reserved">string</span>)<span class="string">$"</span>{1}<span class="string">"</span>); <span class="comment">// string</span>
<span class="method">M</span>((<span class="type">IFormattable</span>)<span class="string">$"</span>{1}<span class="string">"</span>); <span class="comment">// formattable</span>
</code></pre>

`string` 型が真ん中なのがちょっと不思議な仕様ですが、
これは FormattableString のときの反省からです。
FormattableString を優先してほしいのに優先してもらえなくて困るので、
[`RawString`](st_string.md#FormattableString-overload)みたいな「`string` 型を覆った別の型」を1段挟むことで無理やり FormattableString 優先になるようにする手法が知られていました。
ハンドラー型では同じ轍を踏まないよう、最初からハンドラー型優先になっています。

ちなみに、ハンドラーの条件を満たす型が複数あって、
それでオーバーロードした場合、オーバーロード解決できません。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Caller()
{
    <span class="comment">// 優先度は付かないので不明瞭エラーを起こす。</span>
    <span class="error">M</span>(<span class="string">$""</span>);

    <span class="comment">// 明示的にキャストすれば呼び分け可能。</span>
    <span class="method">M</span>((<span class="type">Handler1</span>)<span class="string">$""</span>);
    <span class="method">M</span>((<span class="type">Handler2</span>)<span class="string">$""</span>);
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">Handler1</span> _) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"Handler1"</span>);
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">Handler2</span> _) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"Handler2"</span>);
</code></pre>

## <a id="sec-generated-title-11"></a> <a id="api-in-net6"></a>.NET 6.0 で追加された API

ここまで補間文字列ハンドラーの説明してきましたが、
実際のところ、ハンドラー型を自作することは少ないでしょう。
一方で、標準ライブラリ中に存在するハンドラー型(を使っているメソッド)を使うことで、
補間文字列のパフォーマンス改善によって間接的な利益になる場面は多々あると思います。

C# 10.0 と同時に出た .NET 6.0 ではハンドラー型や、それを使ったメソッドがいくつか追加されています。
本項では最後に、.NET 6.0 で追加されたいくつかのメソッドを紹介して終わりにしたいと思います。

### <a id="sec-generated-title-12"></a> <a id="string.Create"></a>string.Create

`string.Create` に以下の2つのオーバーロードが追加されています。

* [`Create(IFormatProvider, DefaultInterpolatedStringHandler)`](https://docs.microsoft.com/ja-jp/dotnet/api/system.string.create?view=net-6.0#System_String_Create_System_IFormatProvider_System_Runtime_CompilerServices_DefaultInterpolatedStringHandler__)
* [`Create(IFormatProvider, Span<Char>, DefaultInterpolatedStringHandler)`](https://docs.microsoft.com/ja-jp/dotnet/api/system.string.create?view=net-6.0#System_String_Create_System_IFormatProvider_System_Span_System_Char__System_Runtime_CompilerServices_DefaultInterpolatedStringHandler__)

「[InterpolatedStringHandlerArgument 属性](improvedinterpolatedstring.md)」で例に挙げた通り、カルチャー指定で文字列補間するための引数と、初期バッファーを渡すための引数です。

#### <a id="sec-generated-title-13"></a> <a id="string.Create-culture"></a>カルチャー指定

C# の補間文字列はカルチャー依存で、何も指定しないと [`CurrentCulture`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.cultureinfo.currentculture) が使われます。
その結果、手元の環境で実行すると日本式のフォーマットになるけど、
サーバー上で実行すると米国式のフォーマットになったりすることがあります。

<pre class="source" title="カルチャー依存で文字列補間の結果が変わる例">
<code><span class="reserved">using</span> System.Globalization;

<span class="comment">// サンプルなので明示的に指定。</span>
<span class="comment">// 手元の環境が ja-jp カルチャーだとして…</span>
<span class="type">Thread</span>.CurrentThread.CurrentCulture = <span class="type">CultureInfo</span>.<span class="method">GetCultureInfo</span>(<span class="string">"ja-jp"</span>);

<span class="comment">// 日本式。</span>
<span class="comment">// yyyy/MM/dd hh:mm:ss</span>
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{<span class="type">DateTime</span>.Now}<span class="string">"</span>);

<span class="comment">// 一方、サーバーとかで別カルチャーだったりすると…</span>
<span class="comment">// (最近、データ量削減のために「CurrentCulture が常に InvariantCulture」みたいなモードがあったりする。)</span>
<span class="type">Thread</span>.CurrentThread.CurrentCulture = <span class="type">CultureInfo</span>.InvariantCulture;

<span class="comment">// .NET の InvariantCulture は Invariant (不変)と言いつつ、米国基準。</span>
<span class="comment">// MM/dd/yyyy hh:mm:ss</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$"</span>{<span class="type">DateTime</span>.Now}<span class="string">"</span>);
</code></pre>

<pre class="console" title="カルチャー依存で文字列補間の結果が変わる例">
<code>2021/09/23 22:39:39
09/23/2021 22:39:39
</code></pre>

`CurrentCulture` 依存が怖いなら、`string.Create` メソッドを使ってカルチャーを明示します。

<pre class="source" title="string.Create でカルチャーを明示する例">
<code><span class="reserved">using</span> System.Globalization;

<span class="comment">// どこか日本でも Invariant でもない適当なカルチャー。</span>
<span class="type">Thread</span>.CurrentThread.CurrentCulture = <span class="type">CultureInfo</span>.GetCultureInfo(<span class="string">"fr-fr"</span>);

<span class="comment">// これは CurrentCulture 依存。</span>
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{<span class="type">DateTime</span>.Now}<span class="string">"</span>);

<span class="comment">// string.Create を使ってカルチャーを明示すれば CurrentCulture 依存はなくなる。</span>
<span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Create(<span class="type">CultureInfo</span>.InvariantCulture, <span class="string">$"</span>{<span class="type">DateTime</span>.Now}<span class="string">"</span>));
</code></pre>

<pre class="console" title="string.Create でカルチャーを明示する例">
<code>23/09/2021 22:39:39
09/23/2021 22:39:39
</code></pre>

ちなみに[サンプル コード](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2021/Csharp10/InterpolatedStrings/InvariantGlobalization)では、以下のようなハンドラー型を提供していたりします。

* [`Invariant`](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2021/Csharp10/InterpolatedStrings/InvariantGlobalization/Invariant.cs): 常に `InvariantCulture` で文字列補間する型
* [`Iso8601`](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2021/Csharp10/InterpolatedStrings/InvariantGlobalization/Iso8601.cs): 常に `InvariantCulture` を使いつつ、日付だけは MM/dd/yyyy を許さず、ISO 8601 形式で文字列補間する型

#### <a id="sec-generated-title-14"></a> <a id="string.Create-buffer"></a>初期バッファー指定

冒頭での説明通り、C# 10.0 で再コンパイルするだけで文字列補間は高速化されます。
ただ、パフォーマンスを求めるのであれば、素の `$""` を使うよりも、
`string.Create` で初期バッファーを与える方がいいです。
特に、補間結果の文字数がある程度わかっている場合には初期バッファーの指定でパフォーマンスが劇的に改善することがあります。

例えば[サンプル コードのベンチマーク](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2021/Csharp10/InterpolatedStrings/Benchmark/Program.cs)では以下のようなもののパフォーマンス比較を行っています。

* OldStyle: C# 9.0 までの展開結果である `string.Format` を使ったコード
* Improved: C# 10.0 の文字列補間に任せる(`DefaultInterpolatedStringHandler` が使われる)
* InitialBuffer: `string.Create(_currentCulture, stackalloc char[InitialBufferSize], $"{a}.{b}.{c}.{d}")` で初期バッファー指定

手元の環境でベンチマーク計測した結果、これらは以下のような実行結果になりました。

|                                      Method |     Mean |   Error |  StdDev |    Gen 0 | Allocated |
|-------------------------------------------- |---------:|--------:|--------:|---------:|----------:|
|                                    OldStyle | 978.2 us | 0.97 us | 0.76 us | 228.5156 |  1,875 KB |
|                                    Improved | 530.8 us | 0.77 us | 0.64 us |  46.8750 |    391 KB |
|                               InitialBuffer | 377.2 us | 0.73 us | 0.61 us |  47.3633 |    391 KB |

### <a id="sec-generated-title-15"></a> <a id="StringBuilder.Append"></a>StringBuilder.Append

これまで `StringBuilder` (`System.Text` 名前空間)に対して
`builder.Append($"{1} {2} {3}");` みたいなコードを書くと、
一度 `string.Format` で文字列インスタンスを作った上で、それを `Append` していました。

一方、C# 10.0/.NET 6.0 では、[`Append(AppendInterpolatedStringHandler)`](https://docs.microsoft.com/ja-jp/dotnet/api/system.text.stringbuilder.append?view=net-6.0#System_Text_StringBuilder_Append_System_Text_StringBuilder_AppendInterpolatedStringHandler__) というオーバーロードが追加されています。
このオーバーロードを呼ぶと、
`builder.Append($"{1} {2} {3}");` を、以下のようなコードとそん色ないパフォーマンスで呼ぶことができます。

<pre class="source" title="$&quot;{1} {2} {3}&quot; 相当コード">
<code>builder.<span class="method">Append</span>(1);
builder.<span class="method">Append</span>(<span class="string">" "</span>);
builder.<span class="method">Append</span>(2);
builder.<span class="method">Append</span>(<span class="string">" "</span>);
builder.<span class="method">Append</span>(3);
</code></pre>

### <a id="sec-generated-title-16"></a> <a id="MemoryExtensions.TryWrite"></a>MemoryExtensions.TryWrite

`MemoryExtensions` (`System` 名前空間)に [`TryWrite`](https://docs.microsoft.com/ja-jp/dotnet/api/system.memoryextensions.trywrite?view=net-6.0#System_MemoryExtensions_TryWrite_System_Span_System_Char__System_MemoryExtensions_TryWriteInterpolatedStringHandler__System_Int32__) と言う名前で、
`Span<char>` バッファーに直接書き込みするメソッドも追加されています。
`string.Create` の場合は最終的に必ず1個は `new string()` が発生しますが、
`MemoryExtensions.TryWrite` なら完全にアロケーションなしで文字列補間ができます。
バッファー管理がちょっと大変ですが、一応、最速を目指すならこのメソッドを使うことになります。

<pre class="source" title="TryWrite">
<code><span class="reserved">void</span> m(<span class="reserved">int</span> a,<span class="reserved">int</span> b,<span class="reserved">int</span> c,<span class="reserved">int</span> d)
{
    <span class="type">Span</span>&lt;<span class="reserved">char</span>&gt; buffer = <span class="reserved">stackalloc</span> <span class="reserved">char</span>[128];
    buffer.<span class="method">TryWrite</span>(<span class="string">$"</span>{a}<span class="string">.</span>{b}<span class="string">.</span>{c}<span class="string">.</span>{d}<span class="string">"</span>, <span class="reserved">out</span> <span class="reserved">var</span> charsWritten);

    <span class="comment">// デモ用なので ToString しちゃってるけども…</span>
    <span class="comment">// 工夫次第ではこの ToString 負担も避けれる。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(buffer[..charsWritten].ToString());
}
</code></pre>

### <a id="sec-generated-title-17"></a> <a id="Debug.Assert"></a>Debug.Assert

`Debug.Assert` (`System.Diagnostics` 名前空間)に[ハンドラー型を受け取るオーバーロード](https://docs.microsoft.com/ja-jp/dotnet/api/system.diagnostics.debug.assert?view=net-6.0#System_Diagnostics_Debug_Assert_System_Boolean_System_Diagnostics_Debug_AssertInterpolatedStringHandler__)が増えています。

このオーバーロードを使うと、`condition` 引数が `false` の時だけ `AppendLiteral`/`AppendFormatted` を呼び出します。

<pre class="source" title="Debug.Assert">
<code><span class="reserved">using</span> System.Diagnostics;

<span class="type">Debug</span>.<span class="method">Assert</span>(<span class="reserved">true</span>, <span class="string">$@"condition が true な限り、Append は全く呼ばれない。
(Assert の condition はバグがない限り true になっている想定でコードを書く物なので、めったに通らない。)
なので重たい処理を書いても割かし平気。</span>
{<span class="type">DateTime</span>.Now}
{<span class="type">Environment</span>.StackTrace}
{<span class="type">Environment</span>.UserName}
<span class="string">"</span>);
</code></pre>
