---
title: "互換性の維持"
source_url: "https://ufcpp.net/study/csharp/misc/ap_compatibility/"
content_type: "Article"
published_at: "2013-11-10T00:00:00"
updated_at: "2016-09-22T00:00:00"
tags: []
umbraco_id: 1342
parent_id: 1338
sort_order: 4
aliases:
  - "/csharp/ap_compatibility"
  - "/csharp/ap_compatibility.html"
  - "/csharp/misc/ap_compatibility/"
  - "/study/csharp/ap_compatibility"
  - "/study/csharp/ap_compatibility.html"
---

# 互換性の維持

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# は後方互換性に非常に注意を払っています。
C# 自身についても、C# のバージョンを上げることで動かなくなるコードが出ないように気を付けて機能追加をしていますし、
C# で書かれたライブラリについても、ライブラリ内の改修がライブラリ利用側で問題になりにくいように気を使って文法を決めています。


## <a id="sec-generated-title-2"></a> <a id="lang"></a>C# 自体の後方互換性

プログラミング言語に機能を追加する際、既存のプログラム コードがそのままコンパイルできるように互換性を保つことは非常に重要です。


### <a id="sec-generated-title-3"></a> <a id="contextual-keyword"></a>文脈キーワード

C# 2.0 以降で追加されたキーワードは、全て文脈キーワード(contexual keyword) というものになっています。通常のキーワードとは違って、特定の文脈でしかキーワード扱いされません。

文脈キーワードの作り方にも数パターンがありますが、いくつか例を挙げて見ましょう。

- `yield`: 2単語で初めてキーワード扱い
- `var`: 変数宣言できる場所で、かつ、`var`という名前の型が存在しない時だけキーワード扱い
- `await`: `async`修飾子が付いたメソッドの中でだけキーワード扱い
- `nameof`: `nameof`という名前のメソッドがない時に限りキーワード扱い

#### <a id="sec-generated-title-4"></a> <a id="yield"></a>yield

1つ目はC# 2.0で追加された「[イテレーター](../data/sp2_iterator.md#iterator)」に関する <code>yield</code> キーワードです。
<code>yield</code> は、 <code>yield return</code> もしくは <code>yield break</code> という2単語並んだ状態でしかキーワード扱いされません。

ですので、C# 1.0時代に以下のようなコードを書いてた人がいたとしても、C# 2.0 以降でも問題なくコンパイルできます。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">void</span> Calc(<span class="reserved">decimal</span> dividends, <span class="reserved">decimal</span> price)
{
    <span class="comment">// yield には歩留まりとか出来高みたいな意味があって、</span>
    <span class="comment">// こういう変数名を使う人がいてもおかしくはない</span>
    <span class="reserved">decimal</span> yield = dividends / price;
    <span class="type">Console</span>.WriteLine(yield);
}
</code></pre>

極端な話、キーワードの`yield` (`yield return`や`yield break`)と並べて、型名や変数名でも`yield`という識別子を使えます。

<pre class="source" title="yield returnの2単語でキーワード">
<code><span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="type">IEnumerator</span>&lt;<span class="type">yield</span>&gt; F()
    {
        <span class="comment">// 「yield return」の2単語で初めてキーワードになる</span>
        <span class="comment">// 青いところだけがキーワード。</span>
        <span class="comment">// 水色が型名、黒が変数名。</span>

        <span class="type">yield</span> yield = 1;
        <span class="reserved">yield</span> <span class="reserved">return</span> yield;
    }

    <span class="reserved">struct</span> <span class="type">yield</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> value;
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">yield</span>(<span class="reserved">int</span> n) =&gt; <span class="reserved">new</span> <span class="type">yield</span> { value = n };
    }
}
</code></pre>

#### <a id="sec-generated-title-5"></a> <a id="var"></a>var

もう1つ、C# 3.0で導入された「[型推論](../start/sp3_inference.md#type-inference)」に関する <code>var</code> キーワードは、変数宣言出来る文脈でだけキーワード扱いされます。
以下のようなコードも C# 3.0 でコンパイルできます。

<pre class="source" title="var 変数" lang="">
<code><span class="reserved">static</span> <span class="reserved">double</span> Calc(<span class="type">IEnumerable</span>&lt;<span class="reserved">double</span>&gt; data)
{
    <span class="reserved">int</span> count = 0;
    <span class="reserved">double</span> sum = 0;
    <span class="reserved">double</span> sqSum = 0;
 
    <span class="reserved">foreach</span> (<span class="reserved">double</span> x <span class="reserved">in</span> data)
    {
        ++count;
        sum += x;
        sqSum += x * x;
    }
 
    <span class="comment">// 分散(variance)。ローカル変数だし略して var って名前つける人はいる</span>
    <span class="reserved">double</span> <em>var</em> = (sum * sum - sqSum) / count;
    <span class="reserved">return</span> var;
}
</code></pre>

また、`var`という名前の型が存在していた場合は、型推論よりも優先的にその`var`型が使われます。

<pre class="source" title="型推論よりも、var型優先">
<code><span class="reserved">class</span> <span class="type">Inferred</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F()
    {
        <span class="comment">// この場合は型推論で Int 型の変数 var になる</span>
        <span class="reserved">var</span> var = 1;
    }
}

<span class="reserved">class</span> <span class="type">SuccessfullyCompiled</span>
{
    <span class="reserved">struct</span> <span class="type">var</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> value;
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">var</span>(<span class="reserved">int</span> n) =&gt; <span class="reserved">new</span> <span class="type">var</span> { value = n };
    }

    <span class="reserved">static</span> <span class="reserved">void</span> F()
    {
        <span class="comment">// この場合は ↑ の var 構造体型の変数 var になる</span>
        <span class="type">var</span> var = 1;
    }
}

<span class="reserved">class</span> <span class="type">Erroneous</span>
{
    <span class="reserved">struct</span> <span class="type">var</span> { }

    <span class="reserved">static</span> <span class="reserved">void</span> F()
    {
        <span class="comment">// この場合は ↑ の var 構造体型になるけども、1 を代入できなくてコンパイル エラー</span>
        <span class="type">var</span> var = 1;
    }
}
</code></pre>

C#では型名を小文字始まりにする習慣があまりないのでめったなことではこういう状態になりませんが、
もし万が一、C# 2.0以前に`var`型を作っていた人がいてもちゃんとコンパイルできます。

逆に、あまり褒められた手法ではないですが、この仕様を逆手にとって、「このプロジェクトでは型推論を使わせない」というコーディング規約を遵守させるためにわざと`var`型を定義しておく人もいるそうです。

#### <a id="sec-generated-title-6"></a> <a id="await"></a>await

C# 5.0 で導入された非同期メソッド用の <code>await</code> キーワードは、
「<code>async</code> 修飾子がついているメソッドの中でだけキーワード扱いされる」という方法で文脈キーワードになっています
（<code>async</code> はメソッドの手前でだけキーワード扱い）。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> X()
{
    <span class="reserved">var</span> async = 2; <span class="comment">// OK</span>

    <span class="comment">// 匿名関数の中などはまた別文脈</span>
    <span class="comment">// 匿名関数に async を付けているので、この中では await がキーワード</span>
    <span class="type">Func</span>&lt;<span class="type">Task</span>&lt;<span class="reserved">int</span>&gt;&gt; f = <span class="reserved">async</span> () =&gt; { <span class="reserved">await</span> <span class="type">Task</span>.Delay(3); <span class="reserved">return</span> async; };

    <span class="reserved">var</span> await = 5; <span class="comment">// OK</span>
    <span class="reserved">return</span> await * f().Result;
}

<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; XAsync()
{
    <span class="reserved">var</span> async = 2;
    <span class="type">Func</span>&lt;<span class="type">Task</span>&lt;<span class="reserved">int</span>&gt;&gt; f = <span class="reserved">async</span> () =&gt; { <span class="reserved">await</span> <span class="type">Task</span>.Delay(3); <span class="reserved">return</span> async; };
    <span class="reserved">var</span> await = 5; <span class="comment">// コンパイル エラー。キーワード扱いなので変数名に使えない。</span>
    <span class="reserved">return</span> <span class="reserved">await</span> * <span class="reserved">await</span> f();
}
</code></pre>

非同期メソッドの場合、前述の`yield`や`var`とは違い、もしも`await`という名前の型が存在していても、非同期メソッド内では`await`はキーワードです。むしろ、`await`型の方を使うのにエスケープが必要です。

<pre class="source" title="非同期メソッド内ではawait型を使えない">
<code><span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">await</span> { }

    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; XAsync()
    {
        <span class="comment">// async が付いたメソッド内では ↑ の await 型は使えない</span>
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">await</span>(); <span class="comment">// コンパイル エラー</span>

        <span class="comment">// どうしても使いたかったら @ を付けてエスケープ</span>
        <span class="reserved">var</span> y = <span class="reserved">new</span> <span class="type">@await</span>(); <span class="comment">// これならコンパイルできる</span>
    }
}
</code></pre>

ちなみに、C# 4.0以前には非同期メソッド自体がなかったので、これで破壊的変更になるソースコードはこの世に存在しないはずです。

また、`async`に関してもメソッド戻り値の手前でだけキーワード扱いされるので、例えば以下のようなコードでもちゃんとコンパイルできます。

<pre class="source" title="async 型">
<code><span class="reserved">using</span> <span class="type">async</span> = System.Threading.Tasks.<span class="type">Task</span>;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 原理的には C# 4.0 時代にあり得るコード</span>
    <span class="comment">// ちゃんとコンパイル可能</span>
    <span class="comment">// この async は Task クラスのエイリアス</span>
    <span class="reserved">static</span> <span class="type">async</span> F()
    {
        <span class="reserved">return</span> <span class="type">async</span>.Delay(1);
    }

    <span class="comment">// ちゃんと、1つ目の async がキーワード、2つ目の async は型名</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">async</span> G()
    {
        <span class="reserved">await</span> <span class="type">async</span>.Delay(1);
    }
}
</code></pre>

#### <a id="sec-generated-title-7"></a> <a id="nameof"></a>nameof

C# 6で導入された`nameof`演算子は、同名のメソッドがない場合に限ってキーワード扱いされます。

<pre class="source" title="nameofメソッド">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">NoMethod</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F()
    {
        <span class="comment">// nameof メソッドが存在しないのでこれはキーワード</span>
        <span class="reserved">var</span> x = 1;
        <span class="type">Console</span>.WriteLine(<span class="reserved">nameof</span>(x)); <span class="comment">// x</span>
    }
}

<span class="reserved">class</span> <span class="type">SuccessfullyCompiled</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F()
    {
        <span class="comment">// nameof メソッドがあるのでそちらが呼ばれてしまう</span>
        <span class="reserved">var</span> x = 1;
        <span class="type">Console</span>.WriteLine(nameof(x)); <span class="comment">// abc</span>
    }

    <span class="reserved">static</span> <span class="reserved">string</span> nameof(<span class="reserved">int</span> n) =&gt; <span class="string">"abc"</span>;
}

<span class="reserved">class</span> <span class="type">Erroneous</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F()
    {
        <span class="comment">// nameof メソッドがある上に、型が合わない</span>
        <span class="comment">// コンパイル エラーになる</span>
        <span class="reserved">var</span> x = 1;
        <span class="type">Console</span>.WriteLine(nameof(x));
    }

    <span class="reserved">static</span> <span class="reserved">string</span> nameof(<span class="reserved">string</span> s) =&gt; <span class="string">""</span>;
}
</code></pre>

メソッド名も、C#の習慣では大文字始まりで書くものなので、`nameof`メソッド(小文字始まり)を作って使っていた人はほとんどいないでしょう。
それでも万が一いたとしても、ちゃんとC# 6でコンパイルできます。

この仕様のため、1つ気を付けなければならないことがあります。
互換性的な問題ではないですが、[`using static`](../oop/oo_static.md#using-static)との組み合わせで、
知らず知らずのうちに`nameof`メソッドが呼ばれる可能性があります。

<pre class="source" title="using staticとnameofの組み合わせ">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> <span class="reserved">static</span> <span class="type">MyExtensions</span>;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 一見、nameof メソッドはなさそうに見えるけども…</span>
        <span class="comment">// using static MyExtensions; のせいで、MyExtensions.nameof が参照される</span>
        <span class="reserved">var</span> x = 1;
        <span class="type">Console</span>.WriteLine(nameof(x)); <span class="comment">// abc</span>
    }
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span> nameof(<span class="reserved">object</span> x) =&gt; <span class="string">"abc"</span>;
}
</code></pre>

悪意を持ってわざとやらない限り書かれることはないであろうコードですが、一応注意してください。

#### <a id="sec-generated-title-8"></a> <a id="on-context"></a>余談1： 文脈依存の大変さ

文脈キーワードには、過去のバージョンとの互換性を取りやすいというだけでなく、識別子（変数名など）に使える単語が減らないという利点があります。
その一方で、キーワードかどうかをプログラム的に判別するのが難しくなり、例えば、ブログとかでのキーワードの色付け表示がしづらかったりします。
単にキーワードに色を付けるためだけでも単純な文字列マッチングではできず、C# の文法を理解する必要があります。


#### <a id="sec-generated-title-9"></a> <a id="yield-or-await"></a>余談2： yield と await

いくつか紹介してきたように、文脈キーワードの作り方は一種類ではありません。
似たような機能であっても、文脈の作り方が異なる場合もあります。

例えば前述の通り、イテレーター用の <code>yield</code> は、2単語の複合キーワードにすることで文脈キーワードになっています。
一方で、`await`は`async`修飾子が付いたメソッド内では単独でキーワードになります。

<pre class="source" title="yieldとawaitの方針の差">
<code><span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Yield()
{
    <span class="reserved">var</span> yield = 1; <span class="comment">// OK</span>
    <span class="reserved">yield</span> <span class="reserved">return</span> yield;
}

<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; Await()
{
    <span class="comment">//var await = 1; // これはコンパイル エラー</span>
    <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
    <span class="reserved">return</span> 1;
}
</code></pre>

似たような機能にも拘わらず異なる設計になっているのは、C# 2.0の時に導入したイテレーター構文にいくつか不満・不便があったからだそうです。

* <code>yield return</code>というように、2単語書くのがめんどくさい。
    * （<code>await</code>は1単語。）
* 「[匿名関数](../functional/sp_delegate.md#anonymous-func)」内で<code>yield</code>を使えない（匿名関数をイテレーター化できない<sup>※</sup>）。
    * （非同期な匿名関数は作れる。）
* メソッド内に<code>yield</code>が含まれるかどうかによって、メソッド内部のコンパイル結果がまるっきり変わる（のが少し不気味）。
    * （一方、非同期メソッドの方は、<code>await</code>演算子を使わない限り<code>async</code>修飾子を付けても付けなくてもコンパイル結果が同じという気持ち悪さはあります。）

（<sup>※</sup>やってできなくはないものの、コンパイラーの保守コストが跳ね上がって割に合わない。）

いまさら変更はできないんですが（もちろん互換性維持のため）、もしかすると、イテレーターも以下のように、別のキーワードで修飾するような文法の方がよかったかもしれません。

<pre class="source" title="イテレーターの、ありえたかもしれない別構文" lang="">
<code><span class="reserved">static <em>iterator</em></span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Range(<span class="reserved">int</span> from, <span class="reserved">int</span> to)
{
    <span class="reserved">for</span> (<span class="reserved">var</span> i = from; i &lt; to; i++)
        <span class="reserved">yield</span> i;
}
</code></pre>



## <a id="sec-generated-title-10"></a> <a id="library"></a>C# で書かれたコードの互換性

C# の開発者は互換性に対して非常に多くの注意を払っています。
C# という言語自体の互換性だけでなく、
C# を使って書いたライブラリが互換性を保って利用してもらいやすいように C# の文法を決めています。


### <a id="sec-generated-title-11"></a> <a id="dependency"></a>依存関係と、コード修正の影響

C# の文法の話をする前に、ライブラリの互換性維持について少し説明しておきましょう。
シンプルな例ですが、図1に、ライブラリの開発体制としてありがちな状況を示します。

<figure>
	[![ライブラリの開発体制の例](../../../../assets/media/ufcpp2000/csharp/fig/compatibility-lib.png)](../../../../assets/media/ufcpp2000/csharp/fig/compatibility-lib.png)
	<figcaption>ライブラリの開発体制の例</figcaption>
</figure>


例えばこれで、自分は真ん中の「自社製ライブラリ」の開発に関わっていることを想像してください。
自分たちが依存している他のライブラリもありますし、自分たちの作ったライブラリを利用しているアプリもあります。
直接ソースコードを修正できるのは自分たちの作っている「自社製ライブラリ」だけで、依存先の「他社製ライブラリ」は問題を見つけたとしてフィードバックをしてもすぐに修正される保証はありません。
利用者の「アプリ」に至っては、どこの誰が使っているのかさえわからない場合もあります（たとえ社内であったとしても部署が違えばよくある話）。

そして、「他社ライブラリ」中の親クラス（Base）を継承して、「自社ライブラリ」で子クラス（Derived）を作り、その子クラスを「アプリ」が使うというようなことも考えられます。
例えば以下のような状況です（わかりやすくするために1つにまとめていますが、Base、Derived、Program はそれぞれ別ファイル・別プロジェクトにあって、別の人が保守しているものと考えてください）。

<pre class="source" title="保守担当" lang="">
<code><span class="reserved">using</span> System;
 
<span class="comment">// X さんが保守</span>
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
}
 
<span class="comment">// Y さんが保守</span>
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> B() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
}
 
<span class="comment">// Z さんが保守</span>
<span class="comment">// X さん、Y さん、Z さんは互いに全く面識なし。</span>
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Derived</span>();
        x.A();
        x.B();
        <span class="type">Base</span> y = x;
        y.A();
    }
}
</code></pre>


この状況下で、Base や Derived クラスに対する修正がどういう影響を及ぼすかを考える必要があります。


### <a id="sec-generated-title-12"></a> <a id="add"></a>基底クラスへの追加

当然ですが、public になっている部分を「変更」すると、利用側のコードが動かなくなります。
これはわかりやすい互換性の問題なので、たいていの開発者は細心の注意を払うと思います（変更したくてもしないとか、互換性を破棄する旨をあらかじめ伝えるとか）。

では、「追加」ならどうでしょう。以下のような場合がありえます。

<table summary="">

	<tr>
		<th>変更前</th>
		<th>変更後</th>
	</tr>
	<tr>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> B() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
}
</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
 
    <span class="comment">// 派生クラスに B メソッドがあることなんて知らないから足してしまった</span>
    <span class="reserved">public</span> <span class="reserved">void</span> B() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.B"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// エラーにはならない。ただし、警告あり。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> B() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
 
    <span class="comment">// （別に問題ない場合）警告を消すためには public new void B() とする</span>
    <span class="comment">// （たいていは問題になったりするので早めにメソッド名を変えてしまえる方がいいのだけども）</span>
}
</code></pre>

</td>
	</tr>
</table>


Base 側に Derived 側と同じ名前のメソッドを追加してしまいました。
Base 側開発者は Derived 側のことを何も知らないので、悪意なく起こりえる話です。
この場合でも、C# はエラーを起こさないようにしています。


### <a id="sec-generated-title-13"></a> <a id="override"></a>new 修飾子、override 修飾子

やむなく変更が必要な場合も考えてみましょう。
例えば以下の例を見てください。

<table summary="">

	<tr>
		<th>変更前</th>
		<th>変更後</th>
	</tr>
	<tr>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// 意図して Base と同じ名前のメソッドを定義</span>
    <span class="reserved">public</span> <span class="reserved">new</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
}
</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> ARenamed() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// 警告が出る。基底クラスに A がないのに new 修飾。</span>
    <span class="comment">// 少なくとも、Base 側の変更に気づきはする。</span>
    <span class="reserved">public</span> <span class="reserved">new</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
}
</code></pre>

</td>
	</tr>
</table>


Derived 側で A メソッドをわざわざ再定義（new）しているわけで、何らかの事情があったりします
（普通はこういうコードは避ける。意図して（new 修飾子を付けてまで）やっている時点で何か事情がある）。
この場合だと、new 修飾子が不要なのについている状態になって、Derived 側開発者が Base 側の「事情が変わった」ということに気づけるようになっています。

もう1例、似たような話ですが、「[仮想メソッド](../oop/oo_polymorphism.md#virtual_method)」の場合も見てみましょう。

<table summary="">

	<tr>
		<th>変更前</th>
		<th>変更後</th>
	</tr>
	<tr>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
}
</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual void</span> ARenamed() { <span class="type">Console</span>.WriteLine(<span class="literal">"Base.A"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// この場合はコンパイル エラー。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> A() { <span class="type">Console</span>.WriteLine(<span class="literal">"Derived.B"</span>); }
}
</code></pre>

</td>
	</tr>
</table>


仮想メソッドの場合、基底クラスと同じメソッドの別実装を与えることが目的なので、
そもそも基底クラスにないメソッドに override 修飾子がついているというのは明らかに何かのミスがあります。
なので、この場合は、コンパイル エラーを起こします（Base 側の変更に合わせて Derived 側も直す必要がある）。


### <a id="sec-generated-title-14"></a> <a id="overload"></a>オーバーロードの解決ルール

もう少し複雑な例を。
C# では、同じ名前で引数の型だけが違うメソッドを定義できます（「[オーバーロード](../structured/st_function.md#overload)」）。
複数の候補がある場合には、もっとも型の一致度の高いものが選ばれます。
例えば以下のように、型がぴったり一致するオーバーロードがあればそちらが呼ばれます。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A(<span class="reserved">object</span> x) { <span class="type">Console</span>.WriteLine(<span class="literal">"object"</span>); }
    <span class="reserved">public</span> <span class="reserved">void</span> A(<span class="reserved">string</span> x) { <span class="type">Console</span>.WriteLine(<span class="literal">"string"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Sample</span>();
        x.A(<span class="literal">""</span>); <span class="comment">// A(string x) の方が呼ばれる</span>
    }
}
</code></pre>


ここでまた、基底クラスへのメソッド追加を考えてみましょう。

<table summary="">

	<tr>
		<th>変更前</th>
		<th>変更後</th>
	</tr>
	<tr>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Base</span>
{
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A(<span class="reserved">object</span> x) { <span class="type">Console</span>.WriteLine(<span class="literal">"object"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Derived</span>();
        x.A(<span class="literal">""</span>); <span class="comment">// 1個しかないので当然 A(object x) が呼ばれる</span>
    }
}
</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A(<span class="reserved">string</span> x) { <span class="type">Console</span>.WriteLine(<span class="literal">"string"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> A(<span class="reserved">object</span> x) { <span class="type">Console</span>.WriteLine(<span class="literal">"object"</span>); }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Derived</span>();
        x.A(<span class="literal">""</span>); <span class="comment">// 型の一致よりも、Derived にあることが優先されて、A(object) が呼ばれる</span>
    }
}
</code></pre>

</td>
	</tr>
</table>


通常のルールとは異なり、引数の型の一致度よりも、Derived 側で定義されているということの方が優先されます。

最初に説明した通り、Base 側と Derived 側は全く別の、それも面識のない開発者が保守している可能性があって、
Derived 側の事情はお構いなしに Base 側が変更される場合がありえます。
この場合、Base 側に後からメソッドを追加しても、元々の挙動が変わらないようにした結果として、こういうオーバーロードの解決順になっています。


## <a id="sec-generated-title-15"></a> <a id="breaking-change"></a>C# における破壊的変更

注意は払っているといっても、C# にも破壊的変更（breaking change: 互換性を損ねる変更）がなくはないです。

粗を探せば結構な数があるものの、既存コード（マイクロソフト社内でのコードや、オープンソース プロジェクトのコード）を使って、問題のあるコードがほとんどないことを確認しているそうです。
実際、著者の知る範囲で、昔書いた C# コードが最新のコンパイラーでコンパイルして問題が起きたという体験をしたこと/聞いたことは1度もありません。

ここでは、一応、どんな破壊的変更があったのかを紹介しておきましょう
（おそらく影響あるのは、ほとんどの人は思いつかないし、思いついても書かないようなコードだと思います）。


### <a id="sec-generated-title-16"></a> <a id="generic"></a>ジェネリクスの導入

<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 で「[ジェネリック](../oop/sp2_generics.md)」が導入されました。

ジェネリクスはほぼ上位互換な機能追加でしたが、
一応、やろうと思えば 1.0 でしかコンパイルできないようなコードが書けたりします。
以下のコードは、C# 1.0 でしかコンパイルできません。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">int</span> x = 1;
        <span class="reserved">int</span> y = 2;
        <span class="reserved">int</span> z = 3;
        M(x &lt; y, z &gt; (0));
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">bool</span> a, <span class="reserved">bool</span> b) { <span class="type">Console</span>.WriteLine(<span class="literal">"{0}, {1}"</span>, a, b); }
}
</code></pre>


C# 1.0 では、このコードは2つの大小比較 <code>x &lt; y</code> と <code>z &gt; (0)</code> を引数に与えるメソッド M 呼び出しとみなされます。

一方で、C# 2.0 以降の場合、<code>x&lt;y, z&gt;</code> というジェネリックなメソッドの呼び出しとみなされて、
「x はメソッドじゃない」「y, z という型はない」という理由でコンパイル エラーになります。


### <a id="sec-generated-title-17"></a> <a id="is"></a>ジェネリックの変性と is 演算子

<h5 class="version version4">Ver. 4.0</h5>

is 演算子は、例外を出さずにキャストできるかどうかを判定する演算子です（参考: 「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」）。

C# 4.0 で<sup>※</sup>、ジェネリックに「[変性注釈](../oop/sp4_variance.md#variance-annotation)」を持たせれるようになったため、
キャストできるかどうかの結果が変わり、場合によっては互換性を失うコードがあります。
例えば以下のコードは、C# 4.0 以降では True、3.0 以前では False と表示されます。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">IEnumerable</span>&lt;<span class="reserved">string</span>&gt; x = <span class="reserved">new</span> <span class="reserved">string</span>[0];
        <span class="type">Console</span>.WriteLine(x <span class="reserved">is</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">object</span>&gt;);
    }
}
</code></pre>


<sup>※</sup> 実際に変性注釈を持てるようになったのは、「[CLI](../abstract/ab_dotnet.md#cli)」のレベルでは 2.0 の頃からでした（C# の文法に組み込まれたのが 4.0 から）。
ただし、IEnumerable&lt;T&gt; インターフェイスに out 修飾子が付いたのは .NET Framework 4 からなので、
このコードは、.NET 4 以降で実行するか、.NET 3.5 以前で実行するかによって結果が変わることになります（コンパイルに使った C# のバージョンでなく、実行に使う .NET Framework の方のバージョンに依存）。


### <a id="sec-generated-title-18"></a> <a id="event"></a>自動実装 event

<h5 class="version version4">Ver. 4.0</h5>

C# 4.0 では、ひそかに自動実装イベント（add/remove アクセサーを持たないイベント。「[イベント](../functional/sp_event.md#event)」参照）の内部実装方法が変更されました。 
C# の仕様上、イベントの自動実装はスレッド安全であることを要請しています。
しかし、スレッド安全性を保証する方法はいくつかあり、C# 4.0 では、より安全でパフォーマンスもいい実装方法に変更されたという流れです。

例えば、以下のようなイベントがあったとします。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">EventHandler</span>&lt;<span class="reserved">string</span>&gt; A;
}
</code></pre>


C# 3.0 以前では、以下のような MethodImpl 属性を使ったコードに展開されていました。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">EventHandler</span> A
    {
        [<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.Synchronized)]
        <span class="reserved">add</span> { a = (<span class="type">EventHandler</span>)<span class="type">Delegate</span>.Combine(a, <span class="reserved">value</span>); }
        [<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.Synchronized)]
        <span class="reserved">remove</span> { a = (<span class="type">EventHandler</span>)<span class="type">Delegate</span>.Remove(a, <span class="reserved">value</span>); }
    }
    <span class="reserved">private</span> <span class="reserved">event</span> <span class="type">EventHandler</span> a;
}
</code></pre>


かつてはこれでよいと思われていたものの、今となっては、MethodImplOptions.Synchronized による同期にはいくつか問題が指摘されています
（メソッド全体に lock(this) がかかるので、安全性的にもパフォーマンス的にもいまいち）。
そこで、C# 4.0 から、以下のようなコードが生成されるように変更されました。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">EventHandler</span> A
    {
        <span class="reserved">add</span>
        {
            <span class="type">EventHandler</span> a1, a2 = a;
            <span class="reserved">do</span>
            {
                a1 = a2;
                <span class="reserved">var</span> a3 = (<span class="type">EventHandler</span>)<span class="type">Delegate</span>.Combine(a1, <span class="reserved">value</span>);
                a2 = <span class="type">Interlocked</span>.CompareExchange(<span class="reserved">ref</span> a, a3, a1);
            }
            <span class="reserved">while</span> (a2 != a1);
        }
        <span class="reserved">remove</span>
        {
            <span class="type">EventHandler</span> a1, a2 = a;
            <span class="reserved">do</span>
            {
                a1 = a2;
                <span class="reserved">var</span> a3 = (<span class="type">EventHandler</span>)<span class="type">Delegate</span>.Remove(a1, <span class="reserved">value</span>);
                a2 = <span class="type">Interlocked</span>.CompareExchange(<span class="reserved">ref</span> a, a3, a1);
            }
            <span class="reserved">while</span> (a2 != a1);
        }
    }
    <span class="reserved">private</span> <span class="reserved">event</span> <span class="type">EventHandler</span> a;
}
</code></pre>


これは、lock ステートメント（それなりに負担が大きい機構）を使わずにスレッド安全性を保証する方法として知られているパターンの一種です。
基本的にはパフォーマンスがよくなっただけなので、変更といえど問題はほとんど起こりません。

問題が出る極端な場合を紹介すると、
[Mono](http://www.mono-project.com/Main_Page) 2.10 以前のバージョンを使っていて、iOS 上で実行しようとした場合には、
CompareExchange メソッドが正しく動かないという問題があって、上記のコードが実行時エラーを起こします。
（あくまで、C# 4.0（Visual Studio 2010）以上を使って作った DLL を古いバージョンの Mono 経由で iOS 上で使おうとするという状況下でだけ起きる問題です。）


### <a id="sec-generated-title-19"></a> <a id="foreach"></a>foreach の変数スコープ

<h5 class="version version5">Ver. 5.0</h5>

C# 5.0 で、foreach の仕様に変更がありました（参考「[foreach の仕様変更](../cheatsheet/ap_ver5.md#foreach)」）。
以下のコードを実行すると、C# 4.0 以前と 5.0 以降で結果が変わります。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Action</span> a = <span class="reserved">null</span>;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 })
        {
            a += () =&gt; <span class="type">Console</span>.WriteLine(x);
        }
        a();
    }
}
</code></pre>


<table summary="">

	<tr>
		<td markdown="1">4.0 以前</td>
		<td markdown="1">5.0 以降</td>
	</tr>
	<tr>
		<td markdown="1">
<pre class="console" title="">
5
5
5
5
5
</pre>

</td>
		<td markdown="1">
<pre class="console" title="">
1
2
3
4
5
</pre>

</td>
	</tr>
</table>


4.0 以前では、あまりにも使い勝手が悪く、こういうコードを意図して書いている人はほぼいなくて、特に問題にならないという判断で仕様変更が行われました。

実際、4.0 からのバージョンアップで困ることはまずないでしょう。
ただし、その逆、C# 5.0 で作ったコードを古い環境に持っていってコンパイルしなおす場合には注意が必要です。
環境が混在している場合には特に注意しましょう。

### <a id="sec-generated-title-20"></a> <a id="unicode"></a>C#と文字コード(カタカナ中点・)

<h5 class="version version6">Ver. 6</h5>

C# 6で、コンパイラーを1から作り直した影響もあって、C#コンパイラーが参照しているUnicodeのバージョンが変わりました。

ほとんどの場合、Unicodeのバージョンアップは文字の追加なので、破壊的変更になることはありません。しかし、1文字だけ、文字カテゴリーが変わって、今まで変数名につかえていたのに、C# 6からは変数名に使えなくなった文字があります。

詳しくは「[注意: カタカナ中点](../start/misc_unicode.md#katakana-middle-dot)」で説明していますが、カタカナ中点(なかぐろ)「・」(katakana middle dot、U+30FB)がその問題となる文字です。

ちなみに、C#的には、C#のどのバージョンがUnicodeのどのバージョンを使うかは特に明記せず、「とりあえずその時点で最新のUnicodeバージョンを使う」という方針になります。
(これまではマイクロソフト製C#コンパイラーは基本的にWindows上で動かすものだったので特に気にされることはありませんでしたが、C# 6以降の世代では、C#コンパイラーも.NETもオープンソース化、マルチプラットフォーム化した影響で、プラットフォームごとに多少、使える文字が変わる可能性があります。)

### <a id="sec-generated-title-21"></a> <a id="infer-tuple-name"></a>タプル要素名の推論

<h5 class="version version7_1">Ver. 7.1</h5>

C# 7.0で入ったタプルですが、[C# 7.1で少し機能追加がありました](../cheatsheet/ap_ver7_1.md#sec-generated-title-3)。
タプルの要素名を、タプル構築時に与えた変数名から推論する機能なんですが、この機能のせいで、拡張メソッドが絡んだ時の挙動がちょっと変わりました。

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> y(<span class="reserved">this</span> (<span class="reserved">int</span>, <span class="type">Action</span>) t) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"拡張メソッド y"</span>);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">int</span> x = 1;
        <span class="type">Action</span> y = () =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"変数 y"</span>);

        <span class="comment">// C# 7.0 では、(int, Action) 扱い</span>
        <span class="comment">// C# 7.1 では、(int x, Action y) 扱い</span>
        <span class="reserved">var</span> t = (x, y);

        <span class="comment">// C# 7.0 での挙動: 拡張メソッドの y が呼ばれる</span>
        <span class="comment">//     ↑ 正確にいうと、昔の C# コンパイラーではこういう挙動だった</span>
        <span class="comment">//     今の C# コンパイラーでは「その機能を使うには7.1以上を使え」的なコンパイル エラーになる</span>
        <span class="comment">// C# 7.1 での挙動: タプル要素の y が呼ばれる</span>
        t.y();
    }
}
</code></pre>

この「要素名の推論」は、[匿名型](../start/sp3_inference.md#anonymous)であれば当初から使えた機能です。
匿名型と比較されることの多いタプルでも、当然、最初から検討はされていました。
しかし、匿名型には必ず要素名が必要なのに対して、
タプルの場合は名前なしのもの(`(int, Action)`)があり得るので、推論のせいで以下のような状況があり得ます。

<pre class="source" title="">
<code><span class="comment">// 元々こういうコードだったとして、</span>
<span class="comment">//var t = (1, 2);</span>

<span class="comment">// リファクタリングでこう書き換えたとする</span>
<span class="reserved">const</span> <span class="reserved">int</span> M = 1;
<span class="reserved">const</span> <span class="reserved">int</span> N = 2;
<span class="reserved">var</span> t = (M, N);

<span class="comment">// 元々の書き方だと t.Item1 と書かざるを得ない</span>
<span class="comment">// それが、書き換えた方だと「M に書き換えませんか？」と提案される</span>
<span class="comment">// 通常、これは警告にもならないけども、設定変更で警告とかエラーにもできる</span>
<span class="reserved">var</span> x = t.Item1;
</code></pre>

そこで、「C# 7.0の時点では先送りして、必要であれば7.1で推論を導入する」ということになっていたんですが、
その結果、上記の拡張メソッドでの破壊的変更を生んでしまうことに気付いて慌てたようです。

### <a id="sec-generated-title-22"></a> <a id="others"></a>その他

その他細々と、破壊的変更に関する情報のまとめページを以下に掲載して起きます。

* [Visual C# 2008 Breaking Changes](http://msdn.microsoft.com/en-us/library/cc713578.aspx)

* [Visual C# 2010 Breaking Changes](http://msdn.microsoft.com/en-us/library/vstudio/ee855831.aspx)

* [Visual C# Breaking Changes in Visual Studio 2012](http://msdn.microsoft.com/en-us/library/hh678682(v=vs.110).aspx)


これまでに説明してきたような大きなもの以外では、バグっぽかったり仕様漏れだったものを直したものが多いです。
