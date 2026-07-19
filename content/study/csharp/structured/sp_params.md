---
title: "可変長引数"
source_url: "https://ufcpp.net/study/csharp/structured/sp_params/"
content_type: "Article"
published_at: "2015-05-06T14:08:53"
updated_at: "2024-08-16T00:00:00"
tags: []
umbraco_id: 1237
parent_id: 1217
sort_order: 9
aliases:
  - "/csharp/sp_params"
  - "/csharp/sp_params.html"
  - "/csharp/structured/sp_params/"
  - "/study/csharp/sp_params"
  - "/study/csharp/sp_params.html"
---

# 可変長引数

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
C# では <em>
        <code>params</code>
      </em> キーワードを用いることでメソッドの引数の数を可変にすることが出来ます。


##### <a id="sec-generated-title-2"></a>ポイント
* 定義側の例：<code>int Sum(params int[] args) { ... }</code>

* 利用側の例：<code>Sum(1, 2, 3, 4, 5);</code>… これで、<code>Sum(new int[] { 1, 2, 3, 4, 5 });</code>と同じ意味。



##<a id="sec-generated-title-3"></a> <a id="params"></a>params キーワード
例えば、可変個の整数のうち最大の整数を求めるメソッドを作りたいとします。
可変長引数を使わずにメソッドを実装すると以下のようになるでしょう。

<pre class="source" title="最大値を求めるメソッド" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> ParamsTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span> a = 314, b = 159, c = 265, d = 358, e  = 979;
    <span class="comment">// ↑こいつらの最大値を探したいとき、</span>

    <span class="reserved">int</span>[] tmp = <span class="reserved">new int</span>[]{a, b, c, d, e};
    <span class="comment">// ↑こんな風に一度配列に格納してから</span>

    <span class="reserved">int</span> max = Max(tmp);
    <span class="comment">// ↑Max メソッドを呼び出す必要がある。</span>

    Console.Write(<span class="literal">"{0}\n"</span>, max);
  }

  <span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> max = a[0];
    <span class="reserved">for</span>(<span class="reserved">int</span> i=1; i&lt;a.Length; ++i)
    {
      <span class="reserved">if</span>(max &lt; a[i])
        max = a[i];
    }
    <span class="reserved">return</span> max;
  }
}
</code></pre>


この方法では、1度値を配列に格納してからメソッドを呼び出すという操作が必要になります。
このメソッドを呼び出すたびに1時的に配列を作成して、
値を格納してという作業を行うのは面倒です。
そこで、この作業を自動化しようというのが C# の可変長引数の考え方です。

C# では <code>params</code> というキーワードを使って可変個の引数を取るメソッドを定義することが出来ます。
例えば、上の例を <code>params</code> キーワードを使って書き直すと以下のようになります。

<pre class="source" title="最大値を求めるメソッド(可変長引数版)" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> ParamsTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span> a = 314, b = 159, c = 265, d = 358, e  = 979;
    <span class="comment">// ↑こいつらの最大値を探したいとき、</span>

    <span class="reserved">int</span> max = Max(<em>a, b, c, d, e</em>);
    <span class="comment">// ↑こうすると、自動的に配列を作って値を格納してくれる。</span>

    Console.Write(<span class="literal">"{0}\n"</span>, max);
  }

  <span class="reserved">static int</span> Max(<span class="reserved"><em>params</em> int</span>[] a)
  {
    <span class="reserved">int</span> max = a[0];
    <span class="reserved">for</span>(<span class="reserved">int</span> i=1; i&lt;a.Length; ++i)
    {
      <span class="reserved">if</span>(max &lt; a[i])
        max = a[i];
    }
    <span class="reserved">return</span> max;
  }
}
</code></pre>


メソッド定義側の変更点は引数 <code>int[] a</code> の前に <code>params</code> キーワードが付いただけです。
呼び出し側では、手動で配列を用意して値を格納しなくても、
可変個の引数を与えてメソッドを呼び出すことが出来ます。


##### <a id="sec-generated-title-4"></a>サンプル
今まで何気なく <code>Console.Write("(x, y) = ({0}, {1})\n", x, y)</code> というような書き方をしていましたが、この Console.Write メソッドは可変長引数の機構を使っています。

ここでは、params の例として、
かなり簡略化したものですが、Console.Write もどきを作ってみます。

<pre class="source" title="Console.Write もどき" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> TestParams
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="reserved">double</span> x = 3.14;
    <span class="reserved">int</span>    n = 99;
    <span class="reserved">string</span> s = <span class="literal">"test string"</span>;
    <span class="reserved">bool</span>   b = <span class="reserved">true</span>;

    Write(<span class="literal">"x = {0}, n = {1}, s = {2}, b = {3}\n"</span>, x, n, s, b);
  }

  <span class="comment">/// &lt;summary&gt;
  /// Console.Write もどき。
  /// {0:d5} のような書式指定は出来ません。
  /// &lt;/summary&gt;
  /// &lt;param name="format"&gt;書式指定文字列&lt;/param&gt;
  /// &lt;param name="args"&gt;format を使用して書き込むオブジェクトの配列&lt;/param&gt;</span>
  <span class="reserved">static void</span> Write(<span class="reserved">string</span> format, <span class="reserved">params object</span>[] args)
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;args.Length; ++i)
    {
      format = format.Replace(<span class="literal">"{"</span> + i.ToString() + <span class="literal">"}"</span>, args[i].ToString());
    }
    Console.Write(format);
  }
}
</code></pre>


<pre class="console" title="Console.Write もどき">
x = 3.14, n = 99, s = test string, b = True
</pre>

##<a id="sec-generated-title-5"></a> <a id="params-collections">params コレクション</a>
<h5 class="version version13">Ver. 13</h5>

<!-- 昔この id のセクションがあったのでアンカーだけは残す -->
<a id="params-IEnumerable"></a>

C# 13 で、配列以外にも `params` にできる型が増えました。
[コレクション式](../datatype/collection-expression.md)で使える型であれば何でも `params` にできます。
例えば、以下のコードの `M1`～`M4` のようなコードを書けます。

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

俗称として、このような機能を「`params` コレクション」と言います。

昔から `IEnumerable<T>` を使いたいという要望は多くありました。
また、[C# 7.2](../cheatsheet/ap_ver7_2.md#span-safety) 以降では [`Span<T>` や `ReadOnlySpan<T>`](../resource/span.md) を使いたいという要望も出てきました。
どちらも、「具体的に何の型のインスタンスを作って渡すのがいいか」を決めかねていたり、
オーバーロード解決をどうするかという課題があって、今の今まで実装されてきませんでした。
ただ、これらの課題はコレクション式でも全く同じものを抱えます。
つまるところ、コレクション式(当然課題を解決済み)が [C# 12](../cheatsheet/ap_ver12.md#collection-expression) で入ったのであれば、「コレクション式と同じ解決方法をとる」だけで `params` コレクションを実装できます。

そうなると今度は、「コレクション式だけもう十分なのでは？」という話になります。
なんせ、コレクション式のおかげで、`params` がなくても `[]` のたった2文字の追加だけでほぼ同様のことができます。

<pre class="source" title="params の価値とは…">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">A</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">B</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }

<span class="comment">// params</span>
<span class="static"><span class="method">A</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);

<span class="comment">// params がなくても、[] を足すだけ。</span>
<span class="comment">// params の価値とは…</span>
<span class="static"><span class="method">B</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);
</pre>

なので、「パフォーマンス的に明らかに有利な `params ReadOnlySpan<T>` 以外は要らないのではないか」という話も出ました。
実際、需要があるのはこれと、あとはせいぜい `params Span<T>` くらいな可能性があります。
あくまで、「コレクション式が先に実装されている以上、あえて `ReadOnlySpan<T>` だけに制限する理由がない」という感じです。

既存の `params T[]` なメソッドがあったとして、
このメソッドを `params ReadOnlySpan<T>` に置き換えれば、
メソッドを呼んでいる側のコードは書き換えることなく、パフォーマンスが改善します。

例えば元コードとして以下のようなものがあったとします。

<pre class="source" title="params T[] な元コード">
<span class="comment">// 初期状態。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">A</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) { }

<span class="comment">// これはコンパイル結果的には</span>
<span class="comment">// A(new int[] { 1, 2, 3 });</span>
<span class="comment">// になる。</span>
<span class="comment">// この new int[3] がそこそこ重たい。</span>
<span class="static"><span class="method">A</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
</pre>

これが、以下のように、メソッド定義側だけの書き換えで、利用側はノータッチでパフォーマンス改善が見込めます。

<pre class="source" title="params ReadOnlySpan に書き換え">
<span class="comment">// メソッド定義側だけ ReadOnlySpan に変更。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">A</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }

<span class="comment">// 呼び出し側はノータッチ。</span>
<span class="comment">// (C# 13 で再コンパイルだけ必要。)</span>
<span class="comment">// 何もせず、 new int[3] のアロケーションが消える。</span>
<span class="static"><span class="method">A</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
</pre>

利用個所が非常に多い場合、
「コレクション式があるから `[]` の2文字を足して回るだけ」というのもそんなに簡単な話ではないので、
`params ReadOnlySpan<T>` にはそれなりの需要が出てきます。

実際、 .NET 9 では、`string.Join` や `Task.WhenAll` などのメソッドに
`params ReadOnlySpan<T>` なオーバーロードが増えています。

<pre class="source" title="params ReadOnlySpan オーバーロードが増えている例">
<span class="comment">// .NET 8 以前なら Join(string, string[])</span>
<span class="comment">// .NET 9 以降なら Join(string, ReadOnlySpan&lt;string&gt;)</span>
<span class="reserved">var</span> <span class="variable">joiend</span> <span class="operator">=</span> <span class="reserved">string</span><span class="operator">.</span><span class="method"><span class="static">Join</span></span>(<span class="string">&quot;,&quot;</span>, <span class="string">&quot;a&quot;</span>, <span class="string">&quot;b&quot;</span>, <span class="string">&quot;c&quot;</span>);
</pre>

ちなみに、この理屈でのパフォーマンス改善のためには、
コンパイラーを C# 13 にアップグレードした後、1度は再コンパイルが必要です。
再コンパイルしないままだと(以前コンパイルした dll のまま .NET 9 環境に持って行って動かしても)、`params T[]` の方を参照したままになります。

また、こういう「以前コンパイルした dll をそのまま使う」という利用形態がある以上、
`params T[]` なオーバーロードを消すことは破壊的変更になるためためらわれます。
メソッド作者と利用者が同じなら `params T[]` を単に `params ReadOnlySpan<T>` に書き換えてもいいですが、
誰が利用するかわからないメソッドの場合には実質的には `params ReadOnlySpan<T>` オーバーロードの追加(`params T[]` も残す)しかできません。

幸い、[コレクション式の時点でこの辺りは考慮していて](../datatype/collection-expression.md#priority)、`params` でも同様に配列よりも `ReadOnlySpan<T>` (パフォーマンス的に有利)の方が優先度が高い仕様になっています。

<pre class="source" title="ReadOnlySpan が優先">
<span class="comment">// ReadOnlySpan の方が呼ばれる。</span>
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// int[] と ReadOnlySpan&lt;int&gt; の両方ある。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}</pre>

こういった背景から、基本的に、コレクション式と `params` コレクションでは、どちらからも生成されるコードはほぼ同じになります。

<pre class="source" title="コレクション式と params コレクション">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M1</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) { }

<span class="comment">// どちらで呼んでも new int[] { 1 } 生成。</span>
<span class="method"><span class="static">M1</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="method"><span class="static">M1</span></span>([<span class="number">1</span>, <span class="number">2</span>]);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M2</span></span>(<span class="reserved">params</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }

<span class="comment">// どちらで呼んでも <a href="https://ufcpp.net/study/csharp/datatype/inline-array/">InlineArray</a> に展開。</span>
<span class="method"><span class="static">M2</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="static"><span class="method">M2</span></span>([<span class="number">1</span>, <span class="number">2</span>]);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M3</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }

<span class="comment">// どちらで呼んでも<a href="https://ufcpp.net/study/csharp/datatype/collection-expression/#static-data">静的データ最適化</a>が掛かる。</span>
<span class="static"><span class="method">M3</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="static"><span class="method">M3</span></span>([<span class="number">1</span>, <span class="number">2</span>]);
</pre>

#### <a id="sec-generated-title-6"></a> <a id="diff-from-collection-expr">余談:  コレクション式との差</a>
ただ、実装都合でどうしても「全く同じ」にはできないこともあるそうで、ちょっとだけ差があります。
例えば以下のようなコードの場合、`[]` の有無で呼ばれるオーバーロード解決ルールが変わるそうです。

<pre class="source" title="コレクション式と params コレクション利用時で結果がちょっと変わる珍しい例">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121"><span class="static">M</span></span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]); <span class="comment">// こちらは解決できなくてエラーに。</span>
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>); <span class="comment">// こちらは int[] 側に解決。</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">params</span> <span class="reserved">long</span>[] <span class="variable local">_</span>) { }
}
</pre>

#### <a id="sec-generated-title-7"></a> <a id="params-ref-struct">余談:  params ref 構造体</a>
ref 構造体 (`Span<T>` や `ReadOnlySpan<T>` など)に `params` を付けた場合、
暗黙的に [`scoped`](../resource/refstruct.md#scoped-modifier) 扱い(`scoped` 修飾子を付けた場合と同じルールで解析)になるそうです。

<pre class="source" title="params を付けた場合、暗黙的に scoped">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 普通の ReadOnlySpan 引数は、戻り値に素通し可能。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="static"><span class="method">M1</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span>;

    <span class="comment">// scoped を付けると外に漏らせなくなる。</span>
    <span class="comment">// 戻り値に返そうとするとコンパイル エラー。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="static"><span class="method">M2</span></span>(<span class="reserved">scoped</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local"><span class="error" title="CS8352">x</span></span>;

    <span class="comment">// params を付けると自動的に scoped 扱い。</span>
    <span class="comment">// 戻り値に返そうとするとコンパイル エラー。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="method"><span class="static">M3</span></span>(<span class="reserved">params</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="error" title="CS8352"><span class="variable local">x</span></span>;
}
</pre>

`scoped` が付いていると、メソッド定義側での自由が減る代わりに、呼び出し側の自由が増えます。
`params` の用途的に、定義側が `scoped` 困ることもなく、呼び出し側は `scoped` でないと困ることがありそうということでこういう仕様になりました。


##<a id="sec-generated-title-8"></a> <a id="no-param"></a>余談: 可変長引数を引数なしで呼ぶ
可変長引数にしたメソッドは、引数なしで呼ぶこともできます。
この場合、呼び出された側のメソッドには、空配列(長さ0の配列)が渡ります。

<pre class="source" title="可変長引数メソッドを引数なしで呼ぶと空配列が渡る">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> x = Sum();
        <span class="type">Console</span>.WriteLine(x); <span class="comment">// 0</span>
    }

    <span class="reserved">static</span> <span class="reserved">int</span> Sum(<span class="reserved">params</span> <span class="reserved">int</span>[] source)
    {
        <span class="comment">// 引数なしで呼ばれた場合、source には空配列が入る</span>
        <span class="comment">// source が null にはならない</span>
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> source) sum += x;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

ちなみに、空配列の作られ方ですが、
.NET Frameworkのバージョンによって変化します。
.NET Framework 4.6以降/.NET Coreでは、`Array.Empty`という空配列を作るためのメソッドが用意されています。
これがある(つまり、.NET Framework 4.6以降か、.NET Coreがターゲットになっている)場合、このメソッドが呼ばれます。
なければ、`new T[0]`で空配列を作ります。

つまり、上記の`var x = Sum()`は、.NET Framework 4.5以前であれば以下のように解釈されます。

<pre class="source" title=".NET 4.5以前での空配列の作り方">
<code><span class="comment">// .NET Framework 4.5 以前はこういう扱い</span>
<span class="reserved">var</span> x = Sum(<span class="reserved">new</span> <span class="reserved">int</span>[0]);
</code></pre>

一方、.NET Framework 4.6以降であれば以下のように解釈されます。

<pre class="source" title=".NET 4.6以降での空配列の作り方">
<code><span class="comment">// .NET Framework 4.6 以降はこういう扱い</span>
<span class="reserved">var</span> x = Sum(<span class="type">Array</span>.Empty&lt;<span class="reserved">int</span>&gt;());
</code></pre>

これらの差・変更の理由は単純で、`Array.Empty`を使う方がパフォーマンスが良いです。
`new int[0]`だと、メソッド呼び出しのために新しい配列のインスタンスが作られますが、
`Array.Empty`は最初に作った1つのインスタンスをキャッシュしてずっと使いまわします。

一応、昔からあるプログラムの挙動が変わる可能性がある破壊的変更なので注意してください。
狙ってやらないと起こせないような珍しい問題ですが、
例えば以下のようなコードの挙動は、.NET Framework のバージョンによって変化します。

<pre class="source" title="破壊的変更になる例">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> x = IsCached();
        <span class="type">Console</span>.WriteLine(x);
        <span class="reserved">var</span> y = IsCached();
        <span class="type">Console</span>.WriteLine(y); <span class="comment">// ターゲットによって結果が変わる</span>
    }

    <span class="reserved">static</span> <span class="reserved">int</span>[] prev;

    <span class="reserved">static</span> <span class="reserved">bool</span> IsCached(<span class="reserved">params</span> <span class="reserved">int</span>[] source)
    {
        <span class="comment">// .NET 4.5 以前だと、毎回違う配列がnewされて渡ってくる</span>
        <span class="comment">// .NET 4.6 以降だと、毎回同じインスタンスが使いまわされる</span>
        <span class="reserved">if</span> (prev == source) <span class="reserved">return</span> <span class="reserved">true</span>;

        prev = source;
        <span class="reserved">return</span> <span class="reserved">false</span>;
    }
}
</code></pre>

##<a id="sec-generated-title-9"></a> <a id="arglist"></a>__arglist
ちなみに、仕様書にない隠し機能ではあるんですが、
マイクロソフト製や、Mono 製の C# コンパイラーには、可変長引数のための構文として、もう1つ、__arglist というものがあります。
詳しくは「[型付き参照](../interop/sp_makeref.md)」で説明します。

この隠し機能は主に C# 以外のプログラミング言語との相互運用にあるためのものです。
実際のところ、あまり性能はよくない(params を使ったものと比べると1桁は余裕で遅い)ので、わざわざ使うものではないでしょう。
