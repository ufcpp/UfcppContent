---
title: "C# 7.2 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver7_2/"
content_type: "Article"
published_at: "2017-10-22T00:00:00"
updated_at: "2018-03-25T00:00:00"
tags: []
umbraco_id: 2089
parent_id: 1174
sort_order: 11
aliases:
  - "/csharp/cheatsheet/ap_ver7_2/"
---

# C# 7.2 の新機能

##<a id="sec-generated-title-1"></a> <a id="ver7_2"></a>C# 7.2
<div class="version version7_1">Ver. 7.2</div>

<table>
<tr>
<th>リリース時期</th>
<td>2017/12</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2017 15.5</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>構造体と参照の活用</li>
</ul>
</td>
</tr>
</table>

C# 7.2で追加された機能の多くは「構造体と参照の活用によってパフォーマンス改善」と言った感じのものです。
パフォーマンスが求められるようなライブラリの作者にとっては重要になりますが、
多くのC#開発者にとっては直接利用する機能ではないかもしれません。
ただし、そういった開発者にとっても、
「知らないうちに使っていた」とか「使っているライブラリがなんだか速くなった」というような、
間接的な恩恵が受けられるものです。

また、C# 7.1に引き続いての小さな更新がいくつかあります。

※C# 7.2 は、リリース時点ではバグが多く、その後の更新で修正されたものが結構な数あります。
バグが多いのは主に[参照がらみの機能](#ref)の辺りです。
(具体的なバグについては[昔書いたブログ](../../../blog/2017/12/バグ報告祭り/index.md)があるのでそちらを参照。)
本サイト内で説明している機能がうまく動かなかったときには、一度コンパイラーやVisual Studioのバージョンを挙げてみてください。

##<a id="sec-generated-title-2"></a> <a id="leading-separator"></a>先頭区切り文字
`0b`、`0x`の直後に区切り文字の `_` を入れることができるようになりました。

<pre class="source" title="">
<code><span class="comment">// C# 7.0 から書ける</span>
<span class="reserved">var</span> b1 = 0b1111_0000;
<span class="reserved">var</span> x1 = 0x0001_F408;

<span class="comment">// C# 7.2 から書ける</span>
<span class="comment">// b, x の直後に _ 入れてもOKに</span>
<span class="reserved">var</span> b2 = 0b_1111_0000;
<span class="reserved">var</span> x2 = 0x_0001_F408;
</code></pre>

区切り文字に関しては「[数字区切り文字](../start/stnumber.md#digit-separator)」を参照してください。


##<a id="sec-generated-title-3"></a> <a id="non-trailing-named"></a>非末尾名前付き引数
<h5 class="version version7_1">Ver. 7.2</h5>

前の方の引数を名前付きにできるようになりました。
例えば、以下のような書き方が許されるようになりました。

<pre class="source" title="1つ目の引数だけを名前付きにする">
<code><span class="comment">// C# 7.2</span>
<span class="comment">// 末尾以外でも名前を書けるように</span>
Sum(x: 1, 2, 3);
</code></pre>

詳しくは「[オプション引数・名前付き引数](../structured/sp4_optional.md#non-trailing-named)」で説明します。


##<a id="sec-generated-title-4"></a> <a id="private-protected"></a>private protected
`private protected`というキーワード(語順は自由)で、「`protected`かつ`internal`」なアクセシビリティを指定できるようになりました。

![private protected](../../../../assets/media/1142/accessibilitycs72.png)

詳しくは「[実装の隠蔽](../oop/oo_conceal.md#protected-internal)」で説明します。

##<a id="sec-generated-title-5"></a> <a id="ref"></a>参照の活用
ここから先が、C# 7.2 の大部分を占める「参照の活用」になります。
小さな機能の組み合わせになっているのでそれぞれについて説明します。

###<a id="sec-generated-title-6"></a> <a id="conditional-ref"></a>条件演算子での ref 利用
[条件演算子](../start/st_operator.md#condition)の2項目、3項目を参照にできるようになりました。
以下のような書き方ができます。

<pre class="source" title="条件演算子の中で ref を利用">
<code>x &gt; y ? <span class="reserved">ref</span> x : <span class="reserved">ref</span> y
</code></pre>

詳しくは「[条件演算子での ref 利用](../resource/sp_ref.md#conditional-ref)」で説明します。

###<a id="sec-generated-title-7"></a> <a id="ref-readonly"></a>ref readonly
「参照渡しだけども読み取り専用」というような渡し方ができるようになりました。
読み取り専用参照(ref readonly)と呼ばれています。

引数の場合には`in`修飾子を使って以下のように書きます。

<pre class="source" title="in 引数でコピーを避ける">
<code><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Quaternion</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> W;
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> <span class="reserved">double</span> Z;
    <span class="reserved">public</span> Quaternion(<span class="reserved">double</span> w, <span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z) =&gt; (W, X, Y, Z) = (w, x, y, z);

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Quaternion</span> <span class="reserved">operator</span> *(<span class="reserved"><em>in</em></span> Quaternion a, <span class="reserved"><em>in</em></span> Quaternion b)
        =&gt; <span class="reserved">new</span> Quaternion(
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
}
</code></pre>

`ref`引数や`out`引数とは異なり、`in`引数は以下のような呼び出し方ができます。

- `F(x)` というように、修飾なしで呼ぶ
- `F(10)` というように、リテラルを引数として渡す
- `F(x + y)` というように、右辺値(式の計算結果)を引数として渡す

また、ローカル変数と戻り値の場合は`ref readonly`修飾子を使います。

<pre class="source" title="ref readonly な戻り値、ローカル変数">
<code><span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Max(<span class="reserved">in</span> <span class="reserved">int</span> x, <span class="reserved">in</span> <span class="reserved">int</span> y)
{
    <span class="reserved">ref</span> <span class="reserved">readonly</span> var t = <span class="reserved">ref</span> x;
    <span class="reserved">ref</span> <span class="reserved">readonly</span> var u = <span class="reserved">ref</span> y;

    <span class="reserved">if</span> (t &lt; u) <span class="reserved">return</span> <span class="reserved">ref</span> u;
    <span class="reserved">else</span> <span class="reserved">return</span> <span class="reserved">ref</span> t;
}
</code></pre>

詳しくは「[入力参照引数 (in 引数)](../resource/sp_ref.md#in)」、「[ref readonly](../resource/sp_ref.md#ref-readonly)」で説明します。

####<a id="sec-generated-title-8"></a> <a id="in-operator"></a>演算子のin引数
これまで、[演算子オーバーロード](../oop/oo_operator.md)の引数は値渡しである必要がありました。
C# 7.2では、`in`引数も演算子の引数にできるようになりました。

<pre class="source" title="演算子の in 引数">
<code><span class="reserved">struct</span> <span class="type">Complex</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> Complex(<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// これは OK</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Complex <span class="reserved">operator</span> +(Complex a, Complex b)
        =&gt; <span class="reserved">new</span> Complex(a.X + b.X, a.Y + b.Y);

    <span class="comment">// これはコンパイル エラーになる</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Complex <span class="reserved">operator</span> <span class="error">+</span>(<span class="reserved">ref</span> Complex a, <span class="reserved">ref</span> Complex b)
        =&gt; <span class="reserved">new</span> Complex(a.X + b.X, a.Y + b.Y);

    <span class="comment">// これなら OK</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Complex <span class="reserved">operator</span> +(<span class="reserved"><em>in</em></span> Complex a, <span class="reserved"><em>in</em></span> Complex b)
        =&gt; <span class="reserved">new</span> Complex(a.X + b.X, a.Y + b.Y);
}
</code></pre>

###<a id="sec-generated-title-9"></a> <a id="ref-extensions"></a>参照渡しの拡張メソッド
拡張メソッドの第1引数(`this`が付いている引数)を参照渡し([`ref`](../resource/sp_ref.md#sec-byref)もしくは[`in`](../resource/sp_ref.md#in))で渡せるようになりました。

<pre class="source" title="参照渡しの拡張メソッドの例">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">QuaternionExtensions</span>
{
    <span class="comment">// 構造体の書き換えを拡張メソッドでやりたい場合に ref 引数が使える</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Conjugate(<em><span class="reserved">ref</span> <span class="reserved">this</span></em> Quaternion q)
    {
        <span class="reserved">var</span> norm = q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z;
        q.W = q.W / norm;
        q.X = -q.X / norm;
        q.Y = -q.Y / norm;
        q.Z = -q.Z / norm;
    }

    <span class="comment">// コピーを避けたい場合に in 引数が使える</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Quaternion Rotate(<em><span class="reserved">in</span> <span class="reserved">this</span></em> Quaternion p, <span class="reserved">in</span> Quaternion q)
    {
        <span class="reserved">var</span> qc = q;
        qc.Conjugate();
        <span class="reserved">return</span> q * p * qc;
    }
}
</code></pre>

詳しくは「[参照渡しの拡張メソッド](../functional/sp3_extension.md#ref-extensions)」で説明します。

###<a id="sec-generated-title-10"></a> <a id="readonly-struct"></a>readonly struct
構造体に `readonly` 修飾子を付けることで、以下のような制約を掛けれるようになりました。

- すべてのフィールドに`readonly`を付けることが必須
- `this`参照も`readonly`扱いされて、構造体の書き換えが完全にできなくなる

<pre class="source" title="readonly struct の例">
<code><span class="comment">// 構造体自体に readonly を付ける</span>
<span class="reserved"><em>readonly</em></span> <span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="comment">// フィールドには readonly が必須</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Y;

    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// readonly を付けない場合と違って、以下のような this 書き換えも不可</span>
    <span class="comment">//public void Set(int x, int y) =&gt; this = new Point(x, y);</span>
}
</code></pre>

詳細は「[readonly struct](../resource/readonlyness.md#readonly-struct)」で説明します。

「参照」とは直接は関係ないですが、[in 引数](../resource/sp_ref.md#in)や、ref safety rule (今後追加予定)と関連して必要になった機能です。

###<a id="sec-generated-title-11"></a> <a id="safe-stackalloc"></a>安全な stackalloc
`Span<T>`構造体と併用することで、unsafe なしで [`stackalloc`](../interop/sp_unsafe.md#stackalloc) を使えるようになりました。

<pre class="source" title="ファイル読み込みの一時バッファーに stackalloc を使う例">
<code><span class="reserved">const</span> <span class="reserved">int</span> BufferSize = 128;

<span class="reserved">using</span> (<span class="reserved">var</span> f = <span class="type">File</span>.OpenRead(<span class="string">"test.data"</span>))
{
    <span class="reserved">var</span> rest = (<span class="reserved">int</span>)f.Length;
    <span class="comment">// Span&lt;byte&gt; で受け取ることで、new (配列)を stackalloc (スタック確保)に変更できる</span>
    <em><span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; buffer = <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[BufferSize];</em>

    <span class="reserved">while</span> (<span class="reserved">true</span>)
    {
        <span class="comment">// Read(Span&lt;byte&gt;) が追加された</span>
        <span class="reserved">var</span> read = f.Read(buffer);
        rest -= read;
        <span class="reserved">if</span> (rest == 0) <span class="reserved">break</span>;

        <span class="comment">// buffer に対して何か処理する</span>
    }
}
</code></pre>

`stackalloc`を使っていますがポインターは不要で、ちゃんと範囲チェックも掛かって安全に扱えます。

詳しくは「[`Span<T>`構造体](../resource/span.md#safe-stackalloc)」で説明します。

###<a id="sec-generated-title-12"></a> <a id="span-safety"></a>ref 構造体
C# 7.2 と深く関連する型に[`Span<T>`](../resource/span.md)という構造体があります。
この `Span<T>` は、C#7.2 の主たる目的の「構造体と参照の活用によってパフォーマンス改善」の主役となる構造体です。

この型を安全に使うためにはいくつが制限が必要で、そのために`ref`構造体という構文と、それに対するフロー解析が実装されました。

<pre class="source" title="ref構造体を持てるのはref構造体だけ">
<code><span class="comment">// Span&lt;T&gt; は ref 構造体になっている</span>
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt; { ... }

まず、`Span<T>`を持てるのは`ref`修飾子がついた構造体(`ref`構造体)だけです。

<span class="comment">// ref 構造体を持てるのは ref 構造体だけ</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefStruct</span>
{
    <span class="reserved">private</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; _span; <span class="comment">//OK</span>
}
</code></pre>

`ref`構造体には参照ローカル変数・参照戻りと同じ制限がかかります。


<pre class="source" title="戻り値に返せるかどうか">
<code><span class="comment">// 引数で受け取ったものは戻り値で返せる</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Success(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x) =&gt; x;

<span class="comment">// ローカルで確保したもの変数はダメ</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Error()
{
    <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];
    <span class="reserved">return</span> <span class="error">x</span>;
}
</code></pre>

その他、`ref`構造体には「スタック上になければならない(stack-only)」という制限があり、
その結果、例えば以下のような制限がかかります(一部抜粋)。

<pre class="source" title="ref構造体は stack-only">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="comment">//❌ インターフェイス実装</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefStruct</span> : <span class="type"><span class="error">IDisposable</span></span> { <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { } }

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">//❌ 非同期メソッドの引数</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> Async(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="error">x</span>)
    {
        <span class="comment">//❌ 非同期メソッドのローカル変数</span>
        <span class="error"><span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;</span> local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[10];
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];

        <span class="comment">//❌ クロージャ</span>
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; a1 = () =&gt; <span class="error">local</span>[0];
        <span class="reserved">int</span> F() =&gt; <span class="error">local</span>[0];

        <span class="comment">//❌ 型引数にも渡せない</span>
        <span class="type">List</span>&lt;<span class="error"><span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;</span>&gt; list;
    }
}
</code></pre>

詳しくは「[ref構造体](../resource/refstruct.md)」で説明します。

##<a id="sec-generated-title-13"></a> <a id="minor-change"></a>マイナーな更新
C# の[コンパイラー](https://www.nuget.org/packages/Microsoft.Net.Compilers/)のバージョン 2.7 や、Visual Studio 15.6 というバージョン(2018/3リリース)で、
C# にちょっとした修正が入っています。
かなりマイナーな更新なので、「C# 7.3」とはせず「C# 7.2 fix」(バグ修正扱い、あるいは、バグ修正とまとめてリリースして差し支えない程度の更新)としています。

修正されたのは以下の2点です。

- 参照渡しの拡張メソッド
  - 2.6 時点: `ref this`、`in this` の語順でないとダメ
  - 2.7 から: `this ref`、`this in` の語順でも OK
- `in`引数のメソッド呼び出し/値渡しのメソッドとの呼び分け
  - `void M(T x)`と`void M(in T x)`の両方のメソッドがあるとき
  - 2.6 時点: `M(x)` という呼び出しはエラーになる
  - 2.7 から: `M(x)` だと`void M(T x)`の方が、`M(in x)` だと `void M(in T x)`の方が呼ばれる

あくまで「C# 7.2に対する修正」としてリリースされているので、
新しい(2.7以降の)コンパイラーで、昔の(2.6以前の)挙動にすることはできません。
「できてしかるべきことができていなかったのを、できるようにしただけなので問題は起きないはず」という判断です。
