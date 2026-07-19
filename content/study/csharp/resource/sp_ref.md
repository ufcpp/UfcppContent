---
title: "参照渡し"
source_url: "https://ufcpp.net/study/csharp/resource/sp_ref/"
content_type: "Article"
published_at: "2002-12-21T00:00:00"
updated_at: "2018-03-25T00:00:00"
tags:
  - "Ver. 7.0"
umbraco_id: 1290
parent_id: 1286
sort_order: 5
aliases:
  - "/csharp/resource/sp_ref/"
  - "/csharp/sp_ref"
  - "/csharp/sp_ref.html"
  - "/study/csharp/sp_ref"
  - "/study/csharp/sp_ref.html"
---

# 参照渡し

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
プログラミング言語での値の受け渡しの方法には
<strong id="byval" class="keyword">値渡し</strong>（pass by value）と<strong id="byref" class="keyword">参照渡し</strong>（pass by reference）という2つの方法があります。

C# では、値の受け渡しは基本的に値渡しになります。
しかし、<code>ref</code> や <code>out</code> といったキーワードを使うことで参照渡しにすることが出来ます。


##### <a id="sec-generated-title-2"></a>ポイント
* 値渡し： メソッド内で引数の値を書きかえても、呼び出し元には影響しない。

* 参照渡し（ref）： メソッド内での値の書き換えの影響が呼び出し元に伝搬する。

* out： 特殊な参照渡し。戻り値以外にも値を返したいとき（複数の値を返したいとか）に使う。

##<a id="sec-generated-title-3"></a> <a id="pass-by"></a>値の受け渡し
値の受け渡しが発生する場所は何カ所かあります。例えば以下のような場所です。

- 変数から変数
- 変数から引数
- 戻り値から変数

<pre class="source" title="変数から変数への受け渡し">
<code></span><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = x; <span class="comment">// x から y に値を渡す</span>
</code></pre>

<pre class="source" title="変数から引数への値の受け渡し">
<code><span class="reserved">static</span> <span class="reserved">void</span> VariableToParameter()
{
    <span class="reserved">var</span> x = 1;
    F(x); <span class="comment">// 変数 x から、F の引数 x に値を渡す</span>
}

<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span> x)
{
}
</code></pre>

<pre class="source" title=""戻り値から変数への受け渡し>
<code><reserved></span><span class="reserved">static</span> <span class="reserved">void</span> ReturnToVariable()
{
    <span class="reserved">var</span> x = F(); <span class="comment">// F の戻り値から変数 x に値を渡す</span>
}

<span class="reserved">static</span> <span class="reserved">int</span> F() =&gt; 1;
</code></pre>

受け渡しの方法には、以降で説明する[値渡し](#sec-byval)と[参照渡し](#sec-byref)という2種類の受け渡し方法があります。

C#では、通常(特に何もつけないと)、値渡しになります。
一方、以下のようにして、参照渡しを使うこともできます。

- C# 6以前では、引数の受け渡しの際に`ref`もしくは`out`という修飾子を付けることで参照渡しができます
- C# 7以降では、変数間の受け渡しや戻り値でも`ref`修飾子を付けることで参照渡しができます

ちなみに、C#には受け渡しの値渡しと参照渡しの他に、型の区分として[値型と参照型](oo_reference.md)というものもあります。結果的に、「値型の値渡し」、「値型の参照渡し」、「参照型の値渡し」、「参照型の参照渡し」というような組み合わせもできるので注意が必要です。

##<a id="sec-generated-title-4"></a> <a id="sec-byval"></a>値渡し
しばらく、C# 6以前でも使える「引数の受け渡し」で説明して行きましょう。

引数の値渡し(call by value)とは、メソッドを呼び出す際に値のコピーを渡すことを言います。
C# では普通にメソッドを定義すると、その引数は値渡しになります。
例えば、以下のようなプログラムがあったとします。

<pre class="source" title="値渡しの例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">class</span> ByValueTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span> a = 100;
    Console.Write(<span class="literal">"{0} → "</span>, a);
    Test(a);
    Console.Write(<span class="literal">"{0}\n"</span>, a);
  }

  <span class="reserved">static void</span> Test(<span class="reserved">int</span> a)
  {
    a = 10; <span class="comment">// メソッド内で値を書き換える。</span>
  }
}
</code></pre>


<code>Test</code> メソッドの変数 <code>a</code> には <code>Main</code> メソッドの <code>a</code> のコピーが渡されています。
したがって、図1のように、
<code>Test</code> 内で変数 <code>a</code> を書き換えても
<code>Main</code> 内の <code>a</code> の値は変わりません。
そのため、このプログラムの実行結果は以下のようになります。

<pre class="console" title="">
100 → 100
</pre>


<figure>
	[![値型の値渡し](../../../../assets/media/ufcpp2000/csharp/fig/ref1.png)](../../../../assets/media/ufcpp2000/csharp/fig/ref1.png)
	<figcaption>値型の値渡し</figcaption>
</figure>


同様に、参照型の変数を値渡しする場合、図2, 3に示すように、参照情報をコピーして渡すことになります。

<figure>
	[![参照型の値渡し(参照情報の書き換え)](../../../../assets/media/ufcpp2000/csharp/fig/ref2.png)](../../../../assets/media/ufcpp2000/csharp/fig/ref2.png)
	<figcaption>参照型の値渡し(参照情報の書き換え)</figcaption>
</figure>


<figure>
	[![参照型の値渡し(参照先の書き換え)](../../../../assets/media/ufcpp2000/csharp/fig/ref3.png)](../../../../assets/media/ufcpp2000/csharp/fig/ref3.png)
	<figcaption>参照型の値渡し(参照先の書き換え)</figcaption>
</figure>

##<a id="sec-generated-title-5"></a> <a id="sec-byref-param"></a>参照渡しの引数
引数の参照渡し(call by reference)とは、メソッドを呼び出す際に変数の参照情報を渡すことを言います。
C# では、`ref`引数、`in`引数、`out`引数という3種類の参照渡しがあります。

###<a id="sec-generated-title-6"></a> <a id="sec-byref"></a>参照引数(ref 引数)
C# で単に「参照引数」という場合、`ref`引数を指します。
後述する`in`(読み取り専用)や`out`(戻り値的に使う引数)のような制約がなく、読み書き両方できるものです。

以下の例のように、メソッドの引数に <code>ref</code> キーワードを付けることでその引数は参照渡しになります。

<pre class="source" title="参照渡しの例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">class</span> ByReferenceTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span> a = 100;
    Console.Write(<span class="literal">"{0} → "</span>, a);
    Test(<span class="reserved"><em>ref</em></span> a);
    Console.Write(<span class="literal">"{0}\n"</span>, a);
  }

  <span class="reserved">static void</span> Test(<span class="reserved"><em>ref</em> int</span> a)
  {
    a = 10; <span class="comment">// メソッド内で値を書き換える。</span>
  }
}
</code></pre>


<code>Test</code> メソッドの変数 <code>a</code> は <code>Main</code> メソッドの <code>a</code> に対する参照になっています。
したがって、図4のように、
<code>Test</code> 内で変数 <code>a</code> を書き換えた場合、
<code>Main</code> 内の <code>a</code> の値も同時に書き換わります。
そのため、このプログラムの実行結果は以下のようになります。

<pre class="console" title="">
100 → 10
</pre>


<figure>
	[![値型の参照渡し](../../../../assets/media/ufcpp2000/csharp/fig/ref4.png)](../../../../assets/media/ufcpp2000/csharp/fig/ref4.png)
	<figcaption>値型の参照渡し</figcaption>
</figure>


同様に、参照型の変数を値渡しする場合、図5に示すように、参照情報をさらに参照することになります。

<figure>
	[![参照型の参照渡し](../../../../assets/media/ufcpp2000/csharp/fig/ref5.png)](../../../../assets/media/ufcpp2000/csharp/fig/ref5.png)
	<figcaption>参照型の参照渡し</figcaption>
</figure>


ここで1つ注意しなければいけないのは、
<em>メソッドの呼び出し側にも <code>ref</code> キーワードをつける必要がある</em>ということです。
参照渡しを行うと、メソッドの中で値が書き換えられる可能性があります。
(というよりも、書き換える必要があるから参照渡しにする。)
引数が参照渡しであることを知らずにメソッドを呼び出してしまうと、
プログラマの意図しないところで値が書き換わってしまう可能性があり、
これはバグの原因になります。
そのため、呼び出し側でも明示的に <code>ref</code> キーワードを付けなければならいないという制約をつけることによって、
知らないうちに参照渡しのメソッドを呼び出してしまう危険性をなくしています。


##### <a id="sec-generated-title-7"></a>サンプル
<pre class="source" title="ref キーワードのサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> ByRefferanceTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span>[] array = <span class="reserved">new int</span>[]{4, 6, 1, 8, 2, 9, 3, 5, 7};
    BubbleSort(array);
    <span class="reserved">foreach</span>(<span class="reserved">int</span> a <span class="reserved">in</span> array)
    {
      Console.Write(<span class="literal">"{0,3}"</span>, a);
    }
  }

  <span class="comment">/// &lt;summary&gt;
  /// バブルソートを使って配列を整列する
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> BubbleSort(<span class="reserved">int</span>[] array)
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;array.Length-1; ++i)
      <span class="reserved">for</span>(<span class="reserved">int</span> j=array.Length-1; j&gt;i; --j)
        <span class="reserved">if</span>(array[j-1] &gt; array[j])
          Swap(<span class="reserved">ref</span> array[j-1], <span class="reserved">ref</span> array[j]);
  }

  <span class="comment">/// &lt;summary&gt;
  /// a と b の値を入れ替える
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> Swap(<span class="reserved">ref int</span> a, <span class="reserved">ref int</span> b)
  {
    <span class="reserved">int</span> tmp = a;
    a = b;
    b = tmp;
  }
}
</code></pre>


<pre class="console" title="">
  1  2  3  4  5  6  7  8  9
</pre>

###<a id="sec-generated-title-8"></a> <a id="in"></a>入力参照引数 (in 引数)
<h5 class="version version7">Ver. 7.2</h5>

C# 7.2 から、「参照渡しだけども読み取り専用」というような引数の渡し方ができるようになりました。
「入力用」ということを示すように、`in`キーワードを使います。
(`in` を使うのは、C# 1.0の頃からある `out` 引数(次節で説明)との対比もあります。)

<pre class="source" title="in 引数">
<code><span class="reserved">using</span> System;

<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F(<em><span class="reserved">in</span></em> <span class="reserved">int</span> x)
    {
        <span class="comment">// 読み取り可能</span>
        Console.WriteLine(x);

        <span class="comment">// 書き換えようとするとコンパイル エラー</span>
        <span class="error">x</span> = 2;
    }

    <span class="comment">// 補足: in 引数はオプションにもできる</span>
    <span class="reserved">static</span> <span class="reserved">void</span> G(<span class="reserved">in</span> <span class="reserved">int</span> x = 1)
    {
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">int</span> x = 1;

        <span class="comment">// ref 引数と違って修飾不要</span>
        F(x);

        <span class="comment">// 明示的に in と付けてもいい</span>
        F(<span class="reserved">in</span> x);

        <span class="comment">// リテラルに対しても呼べる</span>
        F(10);

        <span class="comment">// 右辺値(式の計算結果)に対しても呼べる</span>
        <span class="reserved">int</span> y = 2;
        F(x + y);
    }
}
</code></pre>

(`int`みたいな型に`in`引数を使ってもメリットは皆無なんですが、サンプルということでご容赦ください。
後述しますが、大き目の構造体に対して使うべき機能です。)

`in`引数は、書き換えできないことがコンパイラーによって保証されています
(書き換えようとするとコンパイル エラーを起こします)。

意図せず書き換わってしまう心配がないので、`ref`引数と違って以下ようなことが認めらています。

- `F(x)` というように、修飾なしで呼ぶ
- `F(10)` というように、リテラルを引数として渡す
  - 既定値を与えて[オプション引数](../structured/sp4_optional.md#optional)にすることもできる
- `F(x + y)` というように、右辺値(式の計算結果)を引数として渡す

ちなみに、
`F(in x)` というように、呼び出し側で `in` 修飾を明示することもできます。
以下のような呼び分けをできるようにするために使います。

<pre class="source" title="値渡しと in 引数の呼び分け">
<code><span class="comment">// 値渡しと in 引数でオーバーロードできる</span>
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span> x) { }
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">in</span> <span class="reserved">int</span> x) { }

<span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="reserved">int</span> x = 1;

    <span class="comment">// (※ 古いバージョンのコンパイラーだとコンパイルできないので注意)</span>
    <span class="comment">// F(int) の方を呼ぶ</span>
    F(x);

    <span class="comment">// F(in int) の方を呼ぶ</span>
    F(<span class="reserved">in</span> x);
}
</code></pre>

※[コンパイラー](https://www.nuget.org/packages/Microsoft.Net.Compilers/)のバージョン2.7以降書けるようになりました。

「書き換えないけども参照で渡す」というのは、
大きめの構造体を使う際に役立ちます。
「[参照渡しの活用](#ref-value-type)」や「[値型の性能](oo_reference.md#performance)」などで触れていますが、
大きめの構造体を値渡し(コピーが発生)すると、結構大きな負担が発生します。
そういう場合に `in` 引数が有用です。

<pre class="source" title="in 引数でコピーを避ける">
<code><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Quaternion</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> W;
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> <span class="reserved">double</span> Z;
    <span class="reserved">public</span> Quaternion(<span class="reserved">double</span> w, <span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z) =&gt; (W, X, Y, Z) = (w, x, y, z);

    <span class="comment">// 足し算4つくらいならインライン展開されて、値渡しでもコピーのコストが掛からない</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Quaternion <span class="reserved">operator</span> +(Quaternion a, Quaternion b)
        =&gt; <span class="reserved">new</span> Quaternion(
            a.W + b.W,
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z);

    <span class="comment">// このくらい中身が大きい(掛け算16個、足し算9個)と、インライン展開されないので in 引数にする効果が結構出る</span>
    <span class="reserved">public</span> <span class="reserved">static</span> Quaternion <span class="reserved">operator</span> *(<span class="reserved"><em>in</em></span> Quaternion a, <span class="reserved"><em>in</em></span> Quaternion b)
        =&gt; <span class="reserved">new</span> Quaternion(
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
}
</code></pre>

ただし、たとえ値渡しでも、[インライン展開](../structured/miscinlining.md)ができるサイズであれば、展開によって値のコピーが消えることがあります。
この例でも、`+` 演算子の方はインライン展開が掛かるため、`in`引数に変えても性能は変わりません(むしろ値渡しの方が速いくらい)。
一方、`*` 演算子の方は中身が大きく、このくらいにあるとインライン展開が掛からないため、`in`引数にした効果が結構現れます。

####<a id="sec-generated-title-9"></a> <a id="in-copy"></a>注意: in 引数を使ってもコピーが発生する場合
詳しくは「[readonly の注意点](readonlyness.md)」で説明しますが、構造体に対して`readonly`を使うと、無駄にコピーが発生してしまうことがあります。
`readonly`なものに対してメソッドを呼ぶ際、呼び出し側は「メソッド内部で値が書き換わっていない」という保証を知る由がないため、
メソッドを呼んだ時点で無条件にコピーを作ります。

この問題は、以下のように、`in`引数でも起こります。[`readonly struct`](readonlyness.md#readonly-struct)を使えば回避できる点も`readonly`フィールドと同様です。

<pre class="source" title="in 引数に対してメソッドを呼ぶとコピーが発せ以することがある">
<code><span class="comment">// 作りとしては readonly を意図しているので、何も書き換えしない</span>
<span class="comment">// でも、struct 自体には readonly が付いていない</span>
<span class="reserved">struct</span> <span class="type">NoReadOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">void</span> M() { }
}

<span class="comment">// NoReadOnly と作りは同じ</span>
<span class="comment">// ちゃんと readonly struct</span>
<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">ReadOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">void</span> M() { }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// in を付けたので readonly 扱い → M を呼ぶ際にコピー発生</span>
    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">in</span> NoReadOnly x) =&gt; x.M();

    <span class="comment">// readonly struct であれば問題なし(コピー回避)</span>
    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">in</span> ReadOnly x) =&gt; x.M();
}
</code></pre>

この、前者(`NoReadOnly`構造体の方)の場合に発生するコピーは、コード上は目に見えません。
だからこそ気づきにくいバグになりがちで、
問題視され、「隠れたコピー」(hidden copy)と呼ばれています。

####<a id="sec-generated-title-10"></a> <a id="ref-readonly-param">ref readonly 引数</a>
<h5 class="version version12">Ver. 12</h5>

[in 引数](#in)では、利便性のため、右辺値を渡せる仕様になっています。

<pre class="source" title="in 引数に右辺値を渡す">
<span class="comment">// in = 参照渡しだけども書き換えはしない。</span>
<span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">in</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }

<span class="comment">// in 引数には右辺値を渡せる。</span>
<span class="method">m</span>(<span class="number">10</span>); <span class="comment">// リテラルとか、</span>

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="reserved">var</span> <span class="variable">b</span> <span class="operator">=</span> <span class="number">2</span>;
<span class="method">m</span>(<span class="variable">a</span> <span class="operator">+</span> <span class="variable">b</span>); <span class="comment">// 式とか。</span>
</pre>

in 引数も参照渡しの一種ですが、本来、参照渡しには「参照先」となる変数が必要です。
in 引数の場合は「書き換えしないのであれば、コンパイラーが作る一時変数を参照しても大丈夫」という前提です。
つまり、さきほどような右辺値を参照する in 引数は、実際には以下のような一時変数が挿入されています。

<pre class="source" title="実際には一時変数が挿入される">
<span class="comment">// in = 参照渡しだけども書き換えはしない。</span>
<span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">in</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="reserved">var</span> <span class="variable">b</span> <span class="operator">=</span> <span class="number">2</span>;

<span class="comment">// 一時変数が作られて、</span>
<span class="reserved">int</span> <span class="variable">temp</span>;

<span class="comment">// その一時変数に値を代入したうえで参照。</span>
<span class="variable">temp</span> <span class="operator">=</span> <span class="number">10</span>;
<span class="method">m</span>(<span class="reserved">in</span> <span class="variable">temp</span>);

<span class="variable">temp</span> <span class="operator">=</span> <span class="variable">a</span> <span class="operator">+</span> <span class="variable">b</span>;
<span class="method">m</span>(<span class="reserved">in</span> <span class="variable">temp</span>);
</pre>

しかし後になって、「書き換えはしないけども、一時変数を渡されると困る」という用途がいくつかあることがわかりました。
例えば `Nullable` 型には .NET 7 から [`GetValueRefOrDefaultRef`](https://learn.microsoft.com/ja-jp/dotnet/api/system.nullable.getvaluerefordefaultref) というメソッドが追加されたんですが、
これが問題になりました。

<pre class="source" title="GetValueRefOrDefaultRef に右辺値を渡せて困った">
<span class="reserved">using</span> System<span class="operator">.</span>Numerics;

<span class="type struct">Quaternion</span><span class="operator">?</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">0</span>);

<span class="comment">// x の中から、x.Value の中身の部分だけを参照。</span>
<span class="comment">// (目的は x.Value のコピーを発生させたくない = パフォーマンス向上。)</span>
<span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">var</span> <span class="variable">v</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="type"><span class="static">Nullable</span></span><span class="operator">.</span><span class="method"><span class="static">GetValueRefOrDefaultRef</span></span>(<span class="reserved">in</span> <span class="variable">x</span>);

<span class="comment">// 一時変数を参照されると…</span>
<span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">var</span> <span class="variable">v1</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="type"><span class="static">Nullable</span></span><span class="operator">.</span><span class="method"><span class="static">GetValueRefOrDefaultRef</span></span>(<span class="warning" title="CS9193"><span class="variable">x</span> + <span class="reserved">new</span> <span class="type struct">Quaternion</span>(<span class="number">1</span>, <span class="operator">-</span><span class="number">1</span>, <span class="number">0</span>, <span class="number">1</span>)</span>);
<span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">var</span> <span class="variable">v2</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="type"><span class="static">Nullable</span></span><span class="operator">.</span><span class="static"><span class="method">GetValueRefOrDefaultRef</span></span>&lt;<span class="type struct">Quaternion</span>&gt;(<span class="warning" title="CS9193"><span class="reserved">new</span>()</span>);
<span class="comment">// v1, v2 は実際にはどこを参照？</span>
<span class="comment">// 一時変数なので消えたり、他で再利用されたりする可能性がある。</span>
</pre>

(問題のある個所に警告が出ていますが、これは C# 12 から出る警告です。
C# 11 時点/ .NET 7 時点では警告が出ません。)

そこで C# 12 では改めて、「書き換えはしないけども、右辺値は受け付けたくない」ということを表す、
ref readonly 引数というものを導入しました。

<pre class="source" title="ref readonly 引数">
<span class="comment">// 冒頭の例から in を ref readonly に変更。</span>
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

用途的に「右辺値は受け付けたくない」という方がレアなので、ref readonly という長ったらしい書き方も許容範囲でしょう。
ほとんどの場合、in 引数を使えばいいと思われます。
(さらにいうと ref 引数や in 引数自体、そもそも利用頻度が低めの機能ですが…)

問題があることがわかっているわけで、ref readonly 引数に右辺値を渡すとエラーにしてもいいくらいですが、警告どまりです。
これは、「一度は in 引数として公開してしまったけどもやっぱり問題があった」というメソッドがあって(前述の `GetValueRefOrDefaultRef` がまさにそう)、
それを ref readonly に変えたいけども、エラーにされると既存コードが困るからだそうです。

また、in 引数と違って `m(ref x)` みたいな呼び出しが許されているのは、
「問題があるから in 引数にできず、本当は書き換えないのに ref 引数にしていた」というメソッドがあるので、
そのメソッドを ref readonly に書き換えた時に、呼び出し側に影響が及ばないようにという配慮です。
(こちらは [`MemoryMarshal.CreateReadOnlySpan`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.memorymarshal.createreadonlyspan) などが該当。)

###<a id="sec-generated-title-11"></a> <a id="out"></a>出力引数 (out 引数)
参照渡しを使うと、メソッド内からメソッド外にある変数を書き換えることができます。
これを、メソッドの戻り値代わりに使うこともできます。
特に、複数の戻り値を返す場合に有効な手段です<sup>※</sup>。
ただ、`ref`修飾子を使った参照引数では、戻り値として使うには以下のようないくつかの問題があります。

<pre class="source" title="参照引数で複数の戻り値を返す(つもり)">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">int</span> a = 0; <span class="comment">// この 0 という値には意味はないけど、必須</span>
        <span class="reserved">int</span> b = 0; <span class="comment">// 同上</span>
        MultipleReturns(<span class="reserved">ref</span> a, <span class="reserved">ref</span> b); <span class="comment">// a, b を</span>
        <span class="type">Console</span>.Write(<span class="string">"{0}\n"</span>, a);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> MultipleReturns(<span class="reserved">ref</span> <span class="reserved">int</span> a, <span class="reserved">ref</span> <span class="reserved">int</span> b)
    {
        a = 10; <span class="comment">// a を初期化</span>
        <span class="comment">// 本当は b も初期化してやらないといけないけど、忘れててバグってる</span>
    }
}
</code></pre>

(<sup>※</sup>C# 6以前では、複数の戻り値を返す唯一の手段でした。C# 7以降ではタプル型というものを使って複数の戻り値を返すことができるようになっています。)

問題を要約すると以下の2点です

- 呼び出し元で、特に意味のない値で変数を初期化しておかなければならない
  - メソッドの中で必ず上書きする想定なので、無駄な初期化になる
- メソッドの中で代入を忘れてしまってもコンパイル エラーにならない

そこで、戻り値として使いたい場合(メソッド内で変数を初期化する予定である場合)、
以下のように <code>out</code> 修飾子を用いて、出力用の参照引数であることを明示してやります。

<pre class="source" title="出力変数の例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">class</span> ByValueTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span> a;
    Test(<span class="reserved"><em>out</em></span> a); <span class="comment">// out を使った場合、変数を初期化しなくてもいい</span>
    Console.Write(<span class="literal">"{0}\n"</span>, a);
  }

  <span class="reserved">static void</span> Test(<span class="reserved"><em>out</em> int</span> a)
  {
    a = 10; <span class="comment">// out を使った場合、メソッド内で必ず値を代入しなければならない</span>
  }
}
</code></pre>


<pre class="console" title="">
10
</pre>

<code>out</code> キーワードを用いて宣言された引数は参照渡しになります。
<code>ref</code> キーワードとの違いは、上述のとおり、

- メソッド呼び出し前に初期化する必要がなくなる
- メソッド内で必ず値を割り当てなければいけない

の2点です。

##### <a id="sec-generated-title-12"></a>サンプル
メソッドで複数の値を返したい場合、
戻り値では1つしか値を返せないので出力変数を使います。

<pre class="source" title="out キーワードのサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> OutTest
{
  <span class="comment">/// &lt;summary&gt;
  /// コンソールから係数を入力して2次方程式の根を計算し、出力する。
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">string</span> line = Console.ReadLine();
    <span class="reserved">string</span>[] token = line.Split(<span class="literal">' '</span>);
    <span class="reserved">double</span> a = <span class="reserved">double</span>.Parse(token[0]);
    <span class="reserved">double</span> b = <span class="reserved">double</span>.Parse(token[1]);
    <span class="reserved">double</span> c = <span class="reserved">double</span>.Parse(token[2]);
    Console.Write(<span class="literal">"{0}x^2 + {1}x + {2} = 0\n"</span>, a, b, c);

    <span class="reserved">double</span> x, y;
    <span class="reserved">int</span> type;
    CalcRoot(a, b, c, <span class="reserved">out</span> type, <span class="reserved">out</span> x, <span class="reserved">out</span> y);
    <span class="reserved">if</span>(type == 0)      Console.Write(<span class="literal">"x = {0}, {1}\n"</span>, x, y);
    <span class="reserved">else if</span>(type == 1) Console.Write(<span class="literal">"x = {0} ±i {1}\n"</span>, x, y);
    <span class="reserved">else</span>               Console.Write(<span class="literal">"x = {0}\n"</span>, x);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 2次方程式 ax^2 + bx + c = 0 の根を求める
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;2次の係数&lt;/param&gt;
  /// &lt;param name="b"&gt;1次の係数&lt;/param&gt;
  /// &lt;param name="c"&gt;定数項&lt;/param&gt;
  /// &lt;param name="type"&gt;根のタイプ。0:実数根2つ、-1:重根1つ、1:虚数根&lt;/param&gt;
  /// &lt;param name="x"&gt;根1(虚数根の場合、根の実部)&lt;/param&gt;
  /// &lt;param name="y"&gt;根2(虚数根の場合、根の虚部)&lt;/param&gt;</span>
  <span class="reserved">static void</span> CalcRoot(
    <span class="reserved">double</span> a, <span class="reserved">double</span> b, <span class="reserved">double</span> c,
    <span class="reserved">out int</span> type, <span class="reserved">out double</span> x, <span class="reserved">out double</span> y)
  {
    b /= 2;
    <span class="reserved">double</span> d = b * b - a * c;

    <span class="reserved">if</span>(d &lt; 0)
    {
      type = 1;
      x = -b / a;
      y = Math.Sqrt(-d) / a;
      <span class="reserved">return</span>;
    }
    
    <span class="reserved">if</span>(d &gt; 0)
    {
      type = 0;
      <span class="reserved">double</span> t1 = -b;
      <span class="reserved">double</span> t2 = Math.Sqrt(d);
      x = (t1 + t2) / a;
      y = (t1 - t2) / a;
      <span class="reserved">return</span>;
    }

    type = -1;
    x = -b / a;
    y = x;
  }
}
</code></pre>


####<a id="sec-generated-title-13"></a> <a id="out-var"></a>出力変数宣言
<h5 class="version version7">Ver. 7</h5>

C# 7で、出力引数を受け取るのと同時に式中で変数を宣言できるようになりました。
これを出力変数宣言(out variable declaration。あるいは、略して out-var)と呼びます。

以前は、出力引数で値を受け取るためには、メソッドなどの呼び出しよりも前に変数を宣言しておく必要がありました。
例えば以下のようになります。

<pre class="source" title="C# 6以前: 出力の受け取りには事前に変数宣言が必要">
<code><span class="reserved">static</span> <span class="reserved">int</span>? ParseOrDefault(<span class="reserved">string</span> s)
{
    <span class="reserved">int</span> x;
    <span class="reserved">return</span> <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> x) ? x : <span class="reserved">default</span>(<span class="reserved">int</span>?);
}
</code></pre>

これに対して、C# 7では、以下のような書き方ができるようになります。
式の中で変数 `x` を宣言しつつ、出力引数の値を受け取っています。

<pre class="source" title="C# 7移行: 出力変数宣言">
<code><span class="reserved">static</span> <span class="reserved">int</span>? ParseOrDefault(<span class="reserved">string</span> s)
{
    <span class="reserved">return</span> <span class="reserved">int</span>.TryParse(s, <em><span class="reserved">out</span> <span class="reserved">int</span> x</em>) ? x : <span class="reserved">default</span>(<span class="reserved">int</span>?);
}
</code></pre>

ちなみに、[`var`](../start/sp3_inference.md#implicit)を使った型推論もできます。

<pre class="source" title="出力変数宣言でもvarによる型推論が使える">
<code><span class="reserved">static</span> <span class="reserved">int</span>? ParseOrDefault(<span class="reserved">string</span> s)
{
    <span class="reserved">return</span> <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> <em><span class="reserved">var</span></em> x) ? x : <span class="reserved">default</span>(<span class="reserved">int</span>?);
}
</code></pre>

この例では、C# 6以前の書き方では、変数宣言ステートメントが必須で、式1つにまとめることができませんでした。
一方、C# 7以降の書き方ならば1つの式で済んでいます。
C# 6で導入された `=>` を使った形式でメソッドを書くことができます。

<pre class="source" title="=&gt; を使う">
<code><span class="reserved">static</span> <span class="reserved">int</span>? ParseOrDefault(<span class="reserved">string</span> s) =&gt; <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> <span class="reserved">var</span> x) ? x : <span class="reserved">default</span>(<span class="reserved">int</span>?);
</code></pre>

出力変数宣言で作った変数のスコープは、概ね、その式を囲っているブロック内になります。
つまり、式の直前に変数を宣言したのと同じスコープになります。

<pre class="source" title="出力変数宣言で作った変数を使える範囲">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="reserved">void</span> GetCoordinate(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y)
    {
        x = X;
        y = Y;
    }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// x, y のスコープはこのブロック内</span>
        <span class="comment">// この辺りで x, y という名前の変数は作れない</span>

        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
        p.GetCoordinate(<span class="reserved">out</span> <span class="reserved">var</span> x, <span class="reserved">out</span> <span class="reserved">var</span> y);

        <span class="comment">// 以下のような書き方をしたのと同じ</span>
        <span class="comment">// int x, y;</span>
        <span class="comment">// p.GetCoordinate(out x, out y);</span>

        <span class="comment">// この行から下で x, y を使える</span>

        <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);
    }
}
</code></pre>

正確にいうともう少し複雑なルールになっていますが、詳細については「[式の中で変数宣言](../start/st_scope.md#declaration-expressions)」を参照してください。

###<a id="sec-generated-title-14"></a> <a id="ref-in-out"></a>in も out も内部的には ref
C# コンパイラーとしては`in`引数や`out`引数を`ref`引数と区別していますが、
.NET の型システムのレベルでは実は区別がありません。
.NET 的には`in`引数も`out`引数も`ref`引数扱いになっています。
そのため、以下のような不便があります。

- オーバーロードの区別に使えない
- 共変・反変にできない

まず、`ref`、`in`、`out`だけの違いのオーバーロードは作れません。
例えば以下のコードでは`F`、`G`、`H`のいずれもコンパイル エラーになります。

<pre class="source" title="ref/in/out 違いのオーバーロードは不可">
<code><span class="reserved">void</span> F(<span class="reserved">ref</span> <span class="reserved">int</span> x) { }
<span class="reserved">void</span> <span class="error">F</span>(<span class="reserved">in</span> <span class="reserved">int</span> x) { }

<span class="reserved">void</span> G(<span class="reserved">ref</span> <span class="reserved">int</span> x) { }
<span class="reserved">void</span> <span class="error">G</span>(<span class="reserved">out</span> <span class="reserved">int</span> x) =&gt; x = 0;

<span class="reserved">void</span> H(<span class="reserved">in</span> <span class="reserved">int</span> x) { }
<span class="reserved">void</span> <span class="error">H</span>(<span class="reserved">out</span> <span class="reserved">int</span> x) =&gt; x = 0;
</code></pre>

もう1つは、`in`引数や`out`引数を持つ[インターフェイス](../oop/oo_interface.md)や[デリゲート](../functional/sp_delegate.md)には[変性](../oop/sp4_variance.md)を指定しません。

入力にしか使わない型引数は[反変(`in`制約)](../oop/sp4_variance.md#contravariance)に、
出力にしか使わない型引数は[共変(`out`制約)](../oop/sp4_variance.md#covariance)にできます。
この条件に沿って考えるなら本来、`in`引数は反変、`out`引数は共変にできるはずです。
ところが、 .NET の型システム上は`in`引数・`out`引数は`ref`引数と同等のものなので、
「入力/出力にしか使わない」という判定ができません。
以下のようなコードはコンパイル エラーになります。

<pre class="source" title="in/out引数を使うと、in/out型制約が付けられない">
<code><span class="reserved">interface</span> <span class="type">Contravariance</span>&lt;<span class="reserved">in</span> <span class="type">T</span>&gt;
{
    <span class="comment">// 普通の引数は共変</span>
    <span class="reserved">void</span> M(<span class="type">T</span> x);

    <span class="comment">// 本来できてもいいはずなものの、.NET 的には無理</span>
    <span class="reserved">void</span> M(<span class="reserved">in</span> <span class="type"><span class="error">T</span></span> x);
}

<span class="reserved">interface</span> <span class="type">Covariance</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="comment">// 普通の戻り値は反変</span>
    <span class="type">T</span> M();

    <span class="comment">// 本来できてもいいはずなものの、.NET 的には無理</span>
    <span class="reserved">void</span> M(<span class="reserved">out</span> <span class="type"><span class="error">T</span></span> x);
}
</code></pre>

ちなみに、最新のコンパイラーで`in`引数を使ったメソッドを作って、
それを古いコンパイラー(Visual Studio 2017 15.4以前)で使おうとすると`ref`引数扱いされます。
(実際のところ、`in`引数は、`ref`引数に`IsReadOnly`属性が付いているだけ。)

###<a id="sec-generated-title-15"></a> <a id="byref-param-restriction"></a>参照引数の制限
[別項](refstruct.md#stack-only)で少し話していますが、参照はスタック上でしか使えません。
参照引数もこの制限に引っかかります。
その結果、参照引数(`ref`、`in`、`out`いずれも)には以下のような制限があります。

- [クロージャ](../functional/fun_localfunctions.md#closure)にキャプチャできない
- [イテレーター](../data/sp2_iterator.md)や[非同期メソッド](../async/sp5_async.md)の引数には使えない

例えば以下のコードはコンパイル エラーになります。

<pre class="source" title="参照引数の制限">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">void</span> M(<span class="reserved">ref</span> <span class="reserved">int</span> x)
    {
        <span class="comment">// クロージャに使えない</span>
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; a = i =&gt; <span class="error">x</span> = i;
        <span class="reserved">void</span> f(<span class="reserved">int</span> i) =&gt; <span class="error">x</span> = i;
    }

    <span class="comment">// イテレーターの引数に使えない</span>
    <span class="type">IEnumerable</span> Iterator(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="error">x</span>)
    {
        <span class="reserved">yield</span> <span class="reserved">break</span>;
    }

    <span class="comment">// 非同期メソッドの引数に使えない</span>
    <span class="reserved">async</span> <span class="type">Task</span> Async(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="error">x</span>)
    {
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
    }
}
</code></pre>

<!-- original-page-break -->

##<a id="sec-generated-title-16"></a> <a id="ref-returns"></a>参照戻り値と参照ローカル変数
<h5 class="version version7">Ver. 7</h5>

- [サンプル](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Resource/RefReturns)

C# 7から、戻り値とローカル変数でも参照渡しを使えるようになりました。
書き方はほぼ参照引数と同じです。
戻り値の型の前、値を渡す側、値を受ける側それぞれに`ref`修飾子を付けます。

例として、配列のi番目の要素を参照で返してみましょう。以下のようになります。

<pre class="source" title="参照戻り値">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span>[] { -1, -1, -1, -1, -1 };

        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; x.Length; i++)
        {
            <span class="comment">// 戻り値を書き換えてる</span>
            <span class="comment">// 実際書き換わってるのは参照先の配列 x</span>
            Ref(x, i) = i;
        }

        <span class="comment">// ↑のループで書き換えたので、結果は 0, 1, 2, 3, 4</span>
        <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="string">", "</span>, x));
    }

    <span class="comment">// 配列の i 番目の要素を参照</span>
    <span class="reserved">static</span> <em><span class="reserved">ref</span> <span class="reserved">int</span></em> Ref(<span class="reserved">int</span>[] array, <span class="reserved">int</span> i) =&gt; <em><span class="reserved">ref</span></em> array[i];
}
</code></pre>

<pre class="console" title="参照戻り値の結果">
<code>0, 1, 2, 3, 4
</code></pre>

また、ローカル変数に対しても、`ref`修飾子を付けることで参照渡しができます。

<pre class="source" title="参照ローカル変数">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> a = 10;

         <em><span class="reserved">ref</span> <span class="reserved">var</span></em> b = <em><span class="reserved">ref</span></em> a; <span class="comment">// 参照ローカル変数。宣言側にも、値を渡す側にも ref</span>

        <span class="reserved">var</span> c = b;         <span class="comment">// これは普通に値渡し(コピー)。この時点の a の値 = 10 が入る</span>
        <em><span class="reserved">ref</span> <span class="reserved">var</span></em> d = <em><span class="reserved">ref</span></em> b; <span class="comment">// さらに参照渡しで、結局 a を参照</span>

        d = 1; <span class="comment">// d = b = a を書き換え</span>

        <em><span class="reserved">ref</span> <span class="reserved">var</span></em> e = <em><span class="reserved">ref</span></em> Ref(<span class="reserved">ref</span> c); <span class="comment">// 参照戻り値越しに、c を参照</span>
        <span class="reserved">var</span> f = Ref(<span class="reserved">ref</span> c);         <span class="comment">// これは結局、値渡し(コピー)</span>

        ++e;   <span class="comment">// e = c を +1。元が10なので、11に</span>
        f = 0; <span class="comment">// f は普通に値渡しで作った新しい変数なので他に影響なし</span>

        <span class="comment">// 結果は 1, 1, 11, 1, 11, 0</span>
        <span class="comment">// a, b, d が同じ場所を参照してて 1</span>
        <span class="comment">// 同上、c, e が 11</span>
        <span class="comment">// f が 0</span>
        <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="string">", "</span>, a, b, c, d, e, f));
    }

    <span class="comment">// 引数を素通し</span>
    <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Ref(<span class="reserved">ref</span> <span class="reserved">int</span> x) =&gt; <span class="reserved">ref</span> x;
}
</code></pre>

<pre class="console" title="参照ローカル変数の結果">
<code>1, 1, 11, 1, 11, 0
</code></pre>

`ref`だらけになってしまいますが、渡す側、受け取る側の両側に`ref`修飾子が必要なのは参照引数と同様です。
元の変数がどこか遠くの知らない場所で書き換えられるかもしれないというのはそれなりに危険なことなので、あえて面倒な構文になっています。

上記の例でも、参照引数を参照戻り値で返して、それをさらに参照ローカル変数で受け取るものもあります。
ここだけ抜き出すと以下のような感じです。

<pre class="source" title="参照引数を参照戻り値で返して、参照ローカル変数で受ける">
<code><reserved></span><span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="reserved">var</span> x = 10;
    <span class="reserved">ref</span> <span class="reserved">var</span> y = <span class="reserved">ref</span> Ref(<span class="reserved">ref</span> x);
    y = 0; <span class="comment">// y は巡り巡って x を参照。x も 0 に</span>

    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>); <span class="comment">// 0, 0</span>
}

<span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Ref(<span class="reserved">ref</span> <span class="reserved">int</span> x) =&gt; <span class="reserved">ref</span> x;
</code></pre>

これで、下図のような状態になっています。これくらい単純な例でも、結局どこが書き換わるのかそこそこわかりづらくなるので注意が必要です。

![参照引数を参照戻り値で返して、参照ローカル変数で受ける](../../../../assets/media/1078/multiref.png)

ちなみに、参照ローカル変数では、
「参照先を読み書きする」という操作の他に、
「どこを参照するか自体を書き換え」という操作が考えられます。
[後述](#ref-reassignment)しますが、この参照の書き換えはC# 7.3からできるようになっています
(逆に、C# 7.0～7.2 ではこの機能は使えません)。

###<a id="sec-generated-title-17"></a> <a id="flow-analysis"></a>参照戻り値で返せるもの
もし何の制限も掛かっていないなら、参照渡しでは参照をたどった先の大元が消えしまっている可能性があって危険です。
C#の参照渡しでは、そうならないように、参照できるものを制限しています。

(他のプログラミング言語では、参照渡しが必ずしも安全でなかったり(不正なメモリ操作につながる)、逆に参照渡しの機能を提供していないものもあります。
.NETも、[IL](../../il/index.md)のレベルでは安全でない参照もできたりします。
C#は、コンパイラーが厳しめにチェックして、安全でない参照ができないようにしています。)

- 通常のメソッドの参照引数は常に安全です
  - なので、これはC# 1.0の頃から認められています
  - [非同期メソッド](../async/sp5_async.md)や[イテレーター](../data/sp2_iterator.md)では安全性を保障できないので、これらのタイプのメソッドでは参照引数を認めていません
- 参照戻り値の場合、返しても安全かどうかを判定して、安全でない可能性があるならコンパイル エラーになります
  - 参照引数は参照戻り値で返せます
  - 通常の引数やローカル変数は返せません
  - 参照ローカル変数などを挟んで、多段に参照している場合、コードをたどって大元が安全かどうかまで調べます

例えば、以下のようなコードは、赤色の下線で強調表示しているところがコンパイル エラーになります。

<pre class="source" title="">
<code><comment></span><span class="comment">// 参照引数は参照戻り値で返せる</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Success1(<span class="reserved">ref</span> <span class="reserved">int</span> x) =&gt; <span class="reserved">ref</span> x;

<span class="comment">// 値渡しの引数はダメ</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Error1(<span class="reserved">int</span> x) =&gt; <span class="reserved">ref</span> <span class="error">x</span>;

<span class="comment">// ローカル変数はダメ</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Error2()
{
    <span class="reserved">var</span> x = <span class="reserved">int</span>.Parse(<span class="type">Console</span>.ReadLine());
    <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">x</span>;
}

<span class="comment">// 多段の場合も元をたどって出所を調べてくれる</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Success1(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">ref</span> <span class="reserved">int</span> y)
{
    <span class="reserved">ref</span> <span class="reserved">int</span> r1 = <span class="reserved">ref</span> x;
    <span class="reserved">ref</span> <span class="reserved">int</span> r2 = <span class="reserved">ref</span> y;
    <span class="reserved">ref</span> <span class="reserved">int</span> r3 = <span class="reserved">ref</span> Max(<span class="reserved">ref</span> r1, <span class="reserved">ref</span> r2);

    <span class="comment">// r3 は出所をたどると引数の x か y の参照</span>
    <span class="comment">// x も y も参照引数なので大丈夫</span>
    <span class="reserved">return</span> <span class="reserved">ref</span> r3;
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Error1(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">int</span> y)
{
    <span class="reserved">ref</span> <span class="reserved">int</span> r1 = <span class="reserved">ref</span> x;
    <span class="reserved">ref</span> <span class="reserved">int</span> r2 = <span class="reserved">ref</span> y;
    <span class="reserved">ref</span> <span class="reserved">int</span> r3 = <span class="reserved">ref</span> Max(<span class="reserved">ref</span> r1, <span class="reserved">ref</span> r2);

    <span class="comment">// y が値渡しなのでダメ</span>
    <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">r3</span>;
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Error2(<span class="reserved">ref</span> <span class="reserved">int</span> x)
{
    <span class="reserved">var</span> y = <span class="reserved">int</span>.Parse(<span class="type">Console</span>.ReadLine());
    <span class="reserved">ref</span> <span class="reserved">int</span> r1 = <span class="reserved">ref</span> x;
    <span class="reserved">ref</span> <span class="reserved">int</span> r2 = <span class="reserved">ref</span> y;
    <span class="reserved">ref</span> <span class="reserved">int</span> r3 = <span class="reserved">ref</span> Max(<span class="reserved">ref</span> r1, <span class="reserved">ref</span> r2);

    <span class="comment">// y がローカル変数なのでダメ</span>
    <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">r3</span>;
}
</code></pre>

C# 7では、コンパイラーが賢くなって、この「大元をたどって調べる」という仕事ができるようになったので、参照戻り値や参照ローカル変数が使えるようになったということです。
こういうコンパイラーの努力を<strong id="key-escape-analysis" class="keyword">エスケープ解析</strong>(escape analysis: 逃がしてはいけないものが漏れ出ていないかの解析)といいます。

ただし、C# 7でも、あくまでメソッド内で完結できる範囲でしか「たどって調べる」ということができません。
例えば、以下のようなコードはコンパイルできません。

<pre class="source" title="メソッドをまたいだ解析まではできない">
<code><span class="comment">// あまり意味のないメソッドなものの…</span>
<span class="comment">// 第1引数しか参照しない</span>
<span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> X(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">ref</span> <span class="reserved">int</span> y) =&gt; <span class="reserved">ref</span> x;

<span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Y(<span class="reserved">ref</span> <span class="reserved">int</span> x)
{
    <span class="reserved">int</span> local = 1;

    <span class="comment">// X の中身まで追えば、実のところ local は参照していないものの、そこまでは追えない</span>
    <span class="comment">// あくまで、「local を参照で渡してしまった以上、X の戻り値に local が含まれている可能性あり」と判定する</span>
    <span class="comment">// 結果的に、このコードはコンパイル エラーになる</span>
    <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">X(<span class="reserved">ref</span> x, <span class="reserved">ref</span> local)</span>;
}
</code></pre>

このコードは、もし仮に、`X`を`Y`の中で展開してしまえば、ローカル変数`local`の参照を戻り値として返さないということがわかるんですが、
コンパイラーはそこまでは追ってくれません。
(こういう`X`の中身次第で変わる挙動を認めてしまうと、`X`の変更の影響が`X`利用側(この例の場合`Y`)に及び過ぎるため問題があります。
「追ってくれない」というより、意図的に「追わない」という面もあります。)

####<a id="sec-generated-title-18"></a> <a id="struct-this"></a>構造体のフィールドの参照(戻り値にできない)
C# コンパイラーが行う「参照戻り値に返して安全かどうか」の判定で、
1つ注意が必要な点があります。
構造体の場合、フィールドの参照を返せません。
(ただし、C# 7.2 では、[`ref`引数拡張メソッドを救済策として使えます](../functional/sp3_extension.md#struct-field)。)

例えば、以下のコードはコンパイル エラーになります。

<pre class="source" title="構造体のフィールドは参照戻り値で返せない">
<code><span class="reserved">struct</span> <span class="type">Struct</span>
{
    <span class="reserved">int</span> _v;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> Value =&gt; <span class="reserved">ref</span> <span class="error">_v</span>; <span class="comment">// ダメ</span>
}

<span class="reserved">class</span> <span class="type">Class</span>
{
    <span class="reserved">int</span> _v;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> Value =&gt; <span class="reserved">ref</span> _v; <span class="comment">// クラスの場合はOK</span>
}
</code></pre>

ちなみに、エラーになるのは構造体のフィールドの参照を直接返している場合だけです。
以下のように、フィールドを介していても、参照型の中の参照を返すことはできます。

<pre class="source" title="構造体でも、参照型の中の参照は返せる">
<code><span class="reserved">struct</span> <span class="type">ArrayOffset</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span>[] _array;
    <span class="reserved">int</span> _offset;
    <span class="reserved">public</span> ArrayOffset(<span class="type">T</span>[] array, <span class="reserved">int</span> offset) =&gt; (_array, _offset) = (array, offset);

    <span class="comment">// フィールドの参照を直接返しているわけではなく、</span>
    <span class="comment">// 配列 T[] (参照型)の中の参照を返しているのでOK</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> i] =&gt; <span class="reserved">ref</span> _array[i + _offset];
}
</code></pre>

構造体内では、フィールドの読み書きのために、実は`this`が参照扱いになっています。
そのせいで、「大元をたどって参照を返せるかどうかを調べる」という作業が難しく、
結局「構造体はフィールドの参照(`this`が絡む参照)を返せない」という制限を掛けたそうです。

この仕様は、少し詳しい人であれば何か釈然としないものがあるかもしれません。
例えば以下のように、[拡張メソッド](../functional/sp3_extension.md)的に([静的メソッド](../oop/oo_static.md)で)書けば似たようなことが実現できます。

<pre class="source" title="静的メソッドで同じようなことを書けば可能">
<code><span class="reserved">struct</span> <span class="type">Struct</span>
{
    <span class="reserved">internal</span> <span class="reserved">int</span> _v;

    <span class="comment">// ↓これはダメ(なのでコメントアウト)</span>
    <span class="comment">// public ref int V() =&gt; ref _v;</span>
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="comment">// Struct.V() と、実のところやっていることは同じ</span>
    <span class="comment">// (構造体内では、this は参照扱いになっている)</span>
    <span class="comment">// Struct.V() ではダメなのに、同じことを静的メソッドでやるとできる</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> V(<span class="reserved">ref</span> <span class="type">Struct</span> @this) =&gt; <span class="reserved">ref</span> @this._v;
}
</code></pre>

実のところ、「`this`が参照扱いになっている」というのはこのコードと似たような状態で、
このコードが許されるのに通常のメソッドでは許されないというのは少し不思議です。

正確には、「以下の2つのうちどちらか片方を選ぶ必要があり、前者を選んだ」ということだそうです。

- 構造体はフィールドの参照を返せない(C# 7で選んだ仕様)
- 構造体の関数メンバーを呼ぶ際には、常に`this`参照が引数として渡っている前提で安全性を調べる(選ばなかった仕様)

要するに、以下の例の、`Ok`メソッドのようなものを認めるためには前者の仕様が必要です。

<pre class="source" title="「構造体はフィールドの参照を返せない」という仕様を必要とするコード">
<code><span class="reserved">struct</span> <span class="type">ArrayOffset</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 拡張メソッドから参照するために internal</span>
    <span class="reserved">internal</span> <span class="type">T</span>[] _array;
    <span class="reserved">internal</span> <span class="reserved">int</span> _offset;
    <span class="reserved">public</span> ArrayOffset(<span class="type">T</span>[] array, <span class="reserved">int</span> offset) =&gt; (_array, _offset) = (array, offset);

    <span class="comment">// OK</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> i] =&gt; <span class="reserved">ref</span> _array[i + _offset];
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="comment">// ArrayOffset のインデクサーと同じことを静的メソッドで書く</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="type">T</span> Get&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">ArrayOffset</span>&lt;<span class="type">T</span>&gt; @this, <span class="reserved">int</span> i) =&gt; <span class="reserved">ref</span> @this._array[i + @this._offset];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Ok()
    {
        <span class="comment">// a はローカル変数なので、こいつが絡む参照は戻り値にしてはいけない</span>
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">ArrayOffset</span>&lt;<span class="reserved">int</span>&gt;(<span class="reserved">new</span>[] { 1, 2, 3 }, 1);

        <span class="comment">// 構造体の関数メンバーはフィールドの参照を返さないという仕様なので、</span>
        <span class="comment">// この ref には a 絡みの参照は絶対にない</span>
        <span class="reserved">return</span> <span class="reserved">ref</span> a[1];
    }

    <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Ng()
    {
        <span class="comment">// 同上、a 絡みの参照は返せない</span>
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">ArrayOffset</span>&lt;<span class="reserved">int</span>&gt;(<span class="reserved">new</span>[] { 1, 2, 3 }, 1);

        <span class="comment">// a が参照引数にわたっている以上、Get の戻り値には a 絡みの参照が含まれる可能性がある</span>
        <span class="comment">// コンパイル エラーになる</span>
        <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error"><span class="type">Extensions</span>.Get(<span class="reserved">ref</span> a, 1)</span>;
    }
}
</code></pre>

あと、以下のように、[ジェネリクス](../oop/sp2_generics.md)絡みの問題を避けるためにもこの仕様を選ぶ必要があったそうです。

<pre class="source" title="構造体がフィールドの参照を返せるとジェネリクス絡みで困る">
<code><span class="reserved">using</span> System;

<span class="reserved">interface</span> <span class="type">IReference</span>
{
    <span class="reserved">ref</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }
}

<span class="reserved">class</span> <span class="type">ReferenceClass</span> : <span class="type">IReference</span>
{
    <span class="reserved">int</span> _value;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> Value =&gt; <span class="reserved">ref</span> _value;
}

<span class="reserved">struct</span> <span class="type">ReferenceStruct</span> : <span class="type">IReference</span>
{
    <span class="reserved">int</span> _value;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> Value =&gt; <span class="reserved">ref</span> <span class="error">_value</span>; <span class="comment">// 認められていない。もし認めると…</span>
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">ref</span> <span class="reserved">var</span> r = <span class="reserved">ref</span> X&lt;<span class="type">ReferenceClass</span>&gt;();
        r = 1;
        <span class="type">Console</span>.WriteLine(1);
    }

    <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> X&lt;<span class="type">T</span>&gt;()
        <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IReference</span>, <span class="reserved">new</span>()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">T</span>();
        <span class="reserved">return</span> <span class="reserved">ref</span> x.Value; <span class="comment">// T が構造体だと、返してはいけないはずの参照が返る</span>
    }
}
</code></pre>

###<a id="sec-generated-title-19"></a> <a id="conditional-ref"></a>条件演算子での ref 利用
<h5 class="version version7">Ver. 7.2</h5>

C# 7.2から、[条件演算子](../start/st_operator.md#condition)の2項目、3項目を参照にできるようになりました。
以下のような書き方ができます。

<pre class="source" title="条件演算子の中で ref を利用">
<code>x &gt; y ? <span class="reserved">ref</span> x : <span class="reserved">ref</span> y
</code></pre>

これを、さらに参照ローカル変数や参照戻り値で受けたい場合には、条件演算子の前にも `ref` が必要です。

<pre class="source" title="条件演算子の前にも ref">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;

<span class="comment">// 条件演算子自体は ref を返すものの、その前に ref を付けていない</span>
<span class="comment">// v の型は int になる</span>
<span class="reserved">var</span> v = x &gt; y ? <span class="reserved">ref</span> x : <span class="reserved">ref</span> y; ;

v = 10; <span class="comment">// 書き換えても x, y に影響なし</span>
Console.WriteLine((x, y)); <span class="comment">// (1, 2)</span>

<span class="comment">// 条件演算子の前にも ref を付ける</span>
<span class="comment">// v の型は ref int になる</span>
<span class="reserved">ref var</span> r = <span class="reserved">ref</span> x &gt; y ? <span class="reserved">ref</span> x : <span class="reserved">ref</span> y; ;

r = 10; <span class="comment">// y が書き換わる</span>
Console.WriteLine((x, y)); <span class="comment">// (1, 10)</span>
</code></pre>

この「条件 ref」は、左辺にも使えます。
例えば以下のように、「条件付きで `x` と `y` のどちらかを書き換える」みたいなことができます。

<pre class="source" title="左辺に条件 ref を書く">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;

<span class="comment">// y が書き換わる</span>
(x &gt; y ? <span class="reserved">ref</span> x : <span class="reserved">ref</span> y) = 10;

Console.WriteLine((x, y)); <span class="comment">// (1, 10)</span>
</code></pre>

ただし、この例の通り、左辺に `()` が必要です。
(`ref` に限った話ではなく、単に演算子の優先度の問題です。
代入と条件演算子が並んでいる場合、右から順に結合するので、`()`がなければ代入が先に解釈されます。)

###<a id="sec-generated-title-20"></a> <a id="ref-readonly"></a>ref readonly
<h5 class="version version7">Ver. 7.2</h5>

[`in`引数](#in)と併せてC# 7.2で、
参照戻り値と参照ローカル変数でも「参照渡しだけども読み取り専用」という渡し方ができるようになりました。
以下のように、`ref readonly`で修飾します。

<pre class="source" title="ref readonly な戻り値、ローカル変数">
<code><span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Max(<span class="reserved">in</span> <span class="reserved">int</span> x, <span class="reserved">in</span> <span class="reserved">int</span> y)
{
    <span class="reserved">ref</span> <span class="reserved">readonly</span> var t = <span class="reserved">ref</span> x;
    <span class="reserved">ref</span> <span class="reserved">readonly</span> var u = <span class="reserved">ref</span> y;

    <span class="reserved">if</span> (t &lt; u) <span class="reserved">return</span> <span class="reserved">ref</span> u;
    <span class="reserved">else</span> <span class="reserved">return</span> <span class="reserved">ref</span> t;
}
</code></pre>

`ref readonly`と書く必要があるのは型名の側だけで、受け渡しする側(上記コードで言うと`ref x`や`ref y`)の方は`ref`だけ書きます。

ちなみに、引数の`in`と、ローカル変数・戻り値の `ref readonly` は全く同じ意味です。
提案当初は引数でも`ref readonly`と書かせる案もありましたが、`out`引数との対称性がきれいだったため、最終的には`in`の方が採用されました。

###<a id="sec-generated-title-21"></a> <a id="ref-reassignment"></a>ref再代入
<h5 class="version version7">Ver. 7.3</h5>

C# 7.3で、参照引数、参照ローカル変数のref再代入(ref reassignment)というものができるようになりました。
参照先の値の書き換えではなく、「どこを参照しているか」自体を書き換える機能です。

以下のように、参照ローカル変数への代入時に、右辺に`ref`を付けることでref再代入になります。

<pre class="source" title="ref 再代入">
<code><span class="reserved">int</span> x = 1;
<span class="reserved">int</span> y = 2;

<span class="comment">// x を参照。</span>
<span class="reserved">ref</span> var r = <span class="reserved">ref</span> x;

<span class="comment">// このとき、r に対する代入は x に反映される。</span>
r = 10; <span class="comment">// x が 10 になる。</span>

<span class="comment">// これが ref 再代入。</span>
<span class="comment">// r が y を参照するようになる。</span>
r = <span class="reserved"><em>ref</em></span> y;

<span class="comment">// 今度は、r に対する代入が y に反映される。</span>
r = 20; <span class="comment">// y が 20 になる。</span>

<span class="type">Console</span>.WriteLine((x, y)); <span class="comment">// (10, 20)</span>
</code></pre>

ちなみに、参照引数に対しても使えます。

<pre class="source" title="参照引数のref再代入">
<code><span class="reserved">static</span> <span class="reserved">void</span> M1(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">ref</span> <span class="reserved">int</span> y)
{
    x = <span class="reserved">ref</span> y;
}

<span class="reserved">static</span> <span class="reserved">void</span> M2(<span class="reserved">in</span> <span class="reserved">int</span> x, <span class="reserved">ref</span> <span class="reserved">int</span> y)
{
    x = <span class="reserved">ref</span> y;
    <span class="comment">// y = ref x; ←逆は当然ダメ</span>
}

<span class="reserved">static</span> <span class="reserved">void</span> M3(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y)
{
    y = 0; <span class="comment">// 先に値を与えないとダメ</span>
    x = <span class="reserved">ref</span> y;
    y = <span class="reserved">ref</span> x;
}
</code></pre>

この機能の用途はそんなに広くはありませんが、
例えば、配列中のデータの探索などで、この機能を使うとシンプルに書けて速度的にも有利なことがあります。
以下の例は、`int`の配列中の最大値になっているところを参照戻り値で返す処理ですが、
都度インデックス アクセスするよりも、ref再代入を使ったコードの方が少しだけ有利です。

<pre class="source" title="ref再代入の利用例">
<code><span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> RefMaxOld(<span class="reserved">int</span>[] array)
{
    <span class="reserved">if</span> (array.Length == 0) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();

    <span class="comment">// これまでこんな感じでインデックスで持って、</span>
    <span class="reserved">var</span> maxIndex = 0;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt; array.Length; i++)
    {
        <span class="comment">// 毎度毎度、配列のインデックス アクセスするようなコードを書くことがたまに。</span>
        <span class="comment">// array[maxIndex] で配列の中身を取り直すのがちょっともったいない。</span>
        <span class="reserved">if</span> (array[maxIndex] &lt; array[i])
        {
            maxIndex = i;
        }
    }

    <span class="reserved">return</span> <span class="reserved">ref</span> array[maxIndex];
}

<span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> RefMax(<span class="reserved">int</span>[] array)
{
    <span class="reserved">if</span> (array.Length == 0) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();

    <span class="comment">// それを、こんな風に参照ローカル変数に変えて、</span>
    <span class="reserved">ref</span> var max = <span class="reserved">ref</span> array[0];

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt; array.Length; i++)
    {
        <span class="comment">// ref 再代入で済ませるように。</span>
        <span class="reserved">ref</span> var x = <em><span class="reserved">ref</span> array[i]</em>;
        <span class="comment">// array (の先頭)に maxIndex を足す作業が減る。</span>
        <span class="reserved">if</span> (max &lt; x) max = <span class="reserved">ref</span> x;
    }

    <span class="reserved">return</span> <span class="reserved">ref</span> max;
}
</code></pre>

###<a id="sec-generated-title-22"></a> <a id="ref-for"></a>for/foreach のループ変数を参照に
<h5 class="version version7">Ver. 7.3</h5>

C# 7.3から、`for`ステートメントや`foreach`ステートメントのループ変数も、参照ローカル変数にできるようにないました。

`for`の方は分かりやすいでしょう。単に、`for (初期化式; 条件式; 更新式)`の初期化式内で参照ローカル変数を定義できるようになっただけです。

<pre class="source" title="ref for">
<code><span class="reserved">var</span> array = <span class="reserved">new</span>[] { 1, 3, 5, 2, 4 };

<span class="reserved">var</span> x = 0;

<span class="reserved">for</span> (<span class="reserved">ref</span> <span class="reserved">int</span> i = <span class="reserved">ref</span> x; i &lt; array.Length; i++)
{
    <span class="reserved">if</span> (array[i] == 5) <span class="reserved">break</span>;
}

<span class="type">Console</span>.WriteLine(x); <span class="comment">// break した時点の i の値 = 2</span>
</code></pre>

用途はそんなに思い浮かびませんが、例えば、C++でよくやるような、[ポインター風の配列列挙](https://gist.github.com/ufcpp/b84e39371ba04ae2c07fbe0b874a6d1e)に使えるかもしれません。

`foreach`の方も、[通常の`foreach`と同じパターン](../data/sp_foreach.md#foreach)で、`MoveNext`や`Current`の呼び出しに展開されるだけです。
`Current`が参照戻り値を返すとき、それをrefループ変数で受け取ることができます。

<pre class="source" title="ref foreach">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> array = <span class="reserved">new</span> <span class="reserved">int</span>[10];
        <span class="reserved">foreach</span> (<span class="reserved"><em>ref</em></span> var x <span class="reserved">in</span> array.AsRef())
        {
            <span class="comment">// ちゃんとこれで、配列の各要素を書き換えられる。</span>
            x = 1;
        }

        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
        {
            <span class="comment">// 全要素 1 になってる。</span>
            <span class="type">Console</span>.WriteLine(x);
        }
    }
}

<span class="comment">// 標準で ref 戻り値になっている Enumerable はないので自作。</span>
<span class="reserved">struct</span> <span class="type">RefArrayEnumerable</span>&lt;<span class="type">T</span>&gt;
{
    T[] _array;
    <span class="reserved">public</span> RefArrayEnumerable(<span class="type">T</span>[] array) =&gt; _array = array;
    <span class="reserved">public</span> <span class="type">RefArrayEnumerator</span>&lt;<span class="type">T</span>&gt; GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">RefArrayEnumerator</span>&lt;<span class="type">T</span>&gt;(_array);
}

<span class="reserved">struct</span> <span class="type">RefArrayEnumerator</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">int</span> _index;
    <span class="type">T</span>[] _array;
    <span class="reserved">public</span> RefArrayEnumerator(<span class="type">T</span>[] array) =&gt; (_index, _array) = (-1, array);
    <span class="comment">// Current が ref 戻り値になっているのがポイント。</span>
    <span class="reserved">public</span> <span class="reserved"><em>ref</em></span> <span class="type">T</span> Current =&gt; <span class="reserved">ref</span> _array[_index];
    <span class="reserved">public</span> <span class="reserved">bool</span> MoveNext() =&gt; ++_index &lt; _array.Length;
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">RefExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> RefArrayEnumerable&lt;T&gt; AsRef&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> T[] array) =&gt; <span class="reserved">new</span> <span class="type">RefArrayEnumerable</span>&lt;T&gt;(array);
}
</code></pre>

この例でもコメントに書いていますが、
言語機能として認められたと言っても、現状はこのパターン通りの列挙子がほとんどないので、
この機能の恩恵はなかなか受けづらくはあります。
また、「`IEnumerable<T>`のref版」のようなインターフェイスもありません。

ただ、.NET Core 2.1 から導入された[`Span<T>`](span.md)であれば、 `Enumerator` が `ref` 戻り値な `Current` を持っています。`AsSpan`拡張メソッドで配列を`Span<T>`にできるので、以下のようなコードが書けます。

<pre class="source" title="AsSpan で参照ループ変数利用">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">array</span> = <span class="reserved">new</span> <span class="reserved">int</span>[10];
        <span class="control">foreach</span> (<span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">array</span>.<span class="method">AsSpan</span>())
        {
            <span class="comment">// ちゃんとこれで、配列の各要素を書き換えられる。</span>
            <span class="variable">x</span> = 1;
        }
 
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">array</span>)
        {
            <span class="comment">// 全要素 1 になってる。</span>
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
        }
    }
}
</code></pre>

### <a id="sec-generated-title-23"></a>余談(将来の話): let や readonly 引数・ローカル変数
ローカル変数に対して `ref readonly var x`というように書くのは長ったらしくて多少しんどいものがあります。

`ref readonly`だけが先に入ることになりましたが、(参照ではなく単に) `readonly` な引数やローカル変数も今後入る予定です。
その際、`readonly var`の省略形として`let`など1単語を使った書き方ができるようになる予定です。
(`let`はもう少し高度な機能として提供される予定ですが、“`readonly var`としても”使えます。)

<pre class="source" title="readonly 引数・ローカル変数">
<code><span class="comment">// (将来の予定)</span>
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">readonly</span> <span class="reserved">int</span> x)
{
    <span class="reserved">readonly</span> <span class="reserved">int</span> a = 1;
    <span class="reserved">readonly</span> var b = 1;
    let c = 1;

    <span class="comment">// 以下、いずれもコンパイル エラー</span>
    x = 1;
    a = 2;
    b = 3;
    c = 3;
}
</code></pre>

ちなみに、`ref readonly`の語順がこの順になっている理由も、この仕様を見越してのことです。
将来的には、以下のような使い分けを考えています。

- `ref`: 「再参照」も「参照先の値の書き換え」もできる
- `readonly`: 「値の書き換え」ができない
- `readonly ref`: 「再参照」できない
- `readonly ref readonly`: 「再参照」も「参照先の値の書き換え」もできない


<!-- original-page-break -->

##<a id="sec-generated-title-24"></a> <a id="value-type"></a>値型の参照渡し
- [サンプル](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Resource/RefReturns)

最後に、参照渡しの活用場面について説明します。

C#には、値渡し・参照渡しと、値型・参照型という区別があって、組み合わせると以下の4つが考えられます。

- 値型の値渡し
- 参照型の値渡し
- 値型の参照渡し
- 参照型の参照渡し

正直、参照型の参照渡しを使いたい場面は、[出力引数](#out)くらいでしょう。
通常の参照引数(`ref`引数)や参照戻り値は、ほぼ値型に対して使うものです。
ここでは、どうして値型の場合は参照渡しが必要になるかについて説明して行きましょう。

###<a id="sec-generated-title-25"></a> <a id="mutate-value"></a>値型の部分書き換えに関する問題
前述の通り、値渡しをすると、値のコピーが発生します。
結果として、値の書き換えは変数ごとに独立になります。

例えば、以下のようなコードを書いたとしましょう。
2つの変数`p`と`q`がありますが、それぞれ別コピーになっていて、片方の書き換えは他方に影響しません。

<pre class="source" title="値渡しの場合、書き換えは変数ごとに独立">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">int</span> Y;

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString() =&gt; <span class="string">$"(</span>{X}<span class="string">, </span>{Y}<span class="string">)"</span>;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };

        <span class="comment">// p のコピーが作られる</span>
        <span class="reserved">var</span> q = p;

        <span class="comment">// コピー側の書き換えなので、p には影響なし</span>
        q.X = 3;
        <span class="type">Console</span>.WriteLine(p); <span class="comment">// 1, 2</span>
        <span class="type">Console</span>.WriteLine(q); <span class="comment">// 3, 2</span>

        <span class="comment">// 同じく、p を書き換えても q に影響なし</span>
        p.Y = 4;
        <span class="type">Console</span>.WriteLine(p); <span class="comment">// 1, 4</span>
        <span class="type">Console</span>.WriteLine(q); <span class="comment">// 3, 2</span>
    }
}
</code></pre>

以下の図のような状態になっているわけです。

![値渡しの場合、書き換えは変数ごとに独立](../../../../assets/media/1079/writevalue.png)

この例はローカル変数への代入に関するものですが、同様の「コピー」は、引数や戻り値でも起こります。
ここで注意が必要なのはプロパティとインデクサーです。
プロパティやインデクサーは、フィールドや配列に対する読み書きに似た呼び出し方になりますが、
実際には関数呼び出しになっています。
値を直接読み書きしているように見えて、実際には引数・戻り値越しの読み書きになります。
そのため、値型のプロパティやインデクサーには注意が必要です。

例えば、フィールドや配列を直接読み書きするのであれば、以下のような書き方ができます。

<pre class="source" title="フィールドや配列を直接書き換え">
<code><span class="reserved">class</span> <span class="type">RawData</span>
{
    <span class="comment">// フィールドを直接公開</span>
    <span class="reserved">public</span> <span class="type">Point</span> P;

    <span class="comment">// 配列を公開</span>
    <span class="reserved">public</span> <span class="type">Point</span>[] Items { <span class="reserved">get</span>; } = <span class="reserved">new</span> <span class="type">Point</span>[3];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> raw = <span class="reserved">new</span> <span class="type">RawData</span>();
        raw.P.X = 1;        <span class="comment">// フィールドは直接書き換え可能</span>
        raw.Items[0].X = 1; <span class="comment">// 配列の要素の直接書き換え可能</span>
    }
}
</code></pre>

これが、プロパティやインデクサーを介すると、以下のように書き換えが面倒になります。

<pre class="source" title="値型のプロパティやインデクサーには注意が必要">
<code><span class="reserved">class</span> <span class="type">CapsuledData</span>
{
    <span class="comment">// プロパティで公開</span>
    <span class="reserved">public</span> <span class="type">Point</span> P { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="comment">// インデクサーで公開</span>
    <span class="reserved">public</span> <span class="type">Point</span> <span class="reserved">this</span>[<span class="reserved">int</span> i]
    {
        <span class="reserved">get</span> { <span class="reserved">return</span> _items[i]; }
        <span class="reserved">set</span> { _items[i] = <span class="reserved">value</span>; }
    }
    <span class="reserved">private</span> <span class="type">Point</span>[] _items = <span class="reserved">new</span> <span class="type">Point</span>[3];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
<span class="inactive">#if false</span>
        <span class="reserved">var</span> cap = <span class="reserved">new</span> <span class="type">CapsuledData</span>();
        cap.P.X = 1;  <span class="comment">// プロパティの戻り値(コピー品)の書き換えはコンパイル エラーに</span>
        cap[0].X = 1; <span class="comment">// インデクサーの戻り値も同様、コンパイル エラーに</span>
<span class="inactive">#else</span>
        <span class="comment">// こんな書き方が必須になる</span>
        <span class="reserved">var</span> cap = <span class="reserved">new</span> <span class="type">CapsuledData</span>();
        <span class="reserved">var</span> p = cap.P; <span class="comment">// 一旦ローカル変数に全体をコピー</span>
        p.X = 1;       <span class="comment">// ローカル変数を部分書き換え</span>
        cap.P = p;     <span class="comment">// 全体を渡しなおし</span>
        <span class="reserved">var</span> q = cap[0];
        q.X = 1;
        cap[0] = q;
<span class="inactive">#endif</span>
    }
}
</code></pre>

この例を見ての通り、部分書き換えができなくなります。
一旦コピーして、ローカル変数に対して部分書き換えをして、その結果を全体を渡しなおす必要があります。

####<a id="sec-generated-title-26"></a> <a id="immutable-value-type"></a>補足: 「構造体は書き換え不能に作れ」ガイドライン
プロパティやインデクサーを通して部分書き換えできないというのが意外と罠になるので、
構造体は最初から部分書き換え不能に作る方がいいというガイドラインもあるくらいです。
このガイドライン通りに`Point`構造体を作るなら、以下のようになります。

<pre class="source" title="書き換えできないように構造体を作る例">
<code><span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Y;

    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { X = x;  Y = y; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString() =&gt; <span class="string">$"(</span>{X}<span class="string">, </span>{Y}<span class="string">)"</span>;
}
</code></pre>

ただし、この方針は、パフォーマンス的には不利になることが多いです。
`X`, `Y`のどちらかだけを書き換えたい場合でも、`X`, `Y`両方のコピーが発生するためです。
特に、構造体のサイズが大きくなると、コピーの負担が結構深刻になってきます。

###<a id="sec-generated-title-27"></a> <a id="ref-value-type"></a>参照渡しの活用
補足で説明したような部分書き換えできない型を作る実装方法は、バグを減らす意味では有効です。
しかしその一方で、パフォーマンス的には不利になります。

先ほどの例の`Point`構造体(`int`型2つでせいぜい8バイト)くらいならいいんですが、
全体のコピーのコストが問題になる場合もあります。
別項の「[値型の性能](oo_reference.md#performance)」で少し触れていますが、
構造体のサイズによってはパフォーマンスに数倍の差が出たりします。

このコピーのコストが許容できない場面で、参照戻り値が役立つことがあります。
例えば先ほどの例を以下のような書き換えてみましょう。
値渡しの時と違って、構造体の部分書き換えができるようになります。

<pre class="source" title="参照戻り値を使って構造体を部分書き換え">
<code><span class="reserved">class</span> <span class="type">RefData</span>
{
    <span class="comment">// 参照戻り値のプロパティで公開</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">Point</span> P =&gt; <span class="reserved">ref</span> _p;
    <span class="reserved">private</span> <span class="type">Point</span> _p;

    <span class="comment">// 参照戻り値のインデクサーで公開</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">Point</span> <span class="reserved">this</span>[<span class="reserved">int</span> i] =&gt; <span class="reserved">ref</span> _items[i];
    <span class="reserved">private</span> <span class="type">Point</span>[] _items = <span class="reserved">new</span> <span class="type">Point</span>[3];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> raw = <span class="reserved">new</span> <span class="type">RefData</span>();
        raw.P.X = 1; <span class="comment">// プロパティ越しに、参照先のフィールドを書き換え可能</span>
        raw[0].X = 1; <span class="comment">// インデクサー越しに、参照先の配列を書き換え可能</span>
    }
}
</code></pre>

プロパティ/インデクサーのsetアクセサーを介する場合と比べると自由度は減ります(set時に値の検証などの処理が挟めない)。
しかし、フィールドや配列を直接公開するよりは自由な処理が書けます(少なくともget時の処理は挟める)。
例えば以下のような利用例が考えられるでしょう。getアクセサーに少しだけ処理が挟まっています。

<pre class="source" title="getアクセサーに少し処理を挟む例">
<code><span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> 循環バッファー。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;typeparam name="</span><span class="type">T</span><span class="inactive">"&gt;</span><span class="comment">要素の型。</span><span class="inactive">&lt;/typeparam&gt;</span>
<span class="reserved">class</span> <span class="type">CircularBuffer</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">private</span> <span class="reserved">int</span> _startIndex;
    <span class="reserved">private</span> <span class="type">T</span>[] _data;

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> 容量を指定して初期化。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;param name="</span>capacity<span class="inactive">"&gt;</span><span class="comment">容量。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="reserved">public</span> CircularBuffer(<span class="reserved">int</span> capacity)
    {
        _startIndex = 0;
        _data = <span class="reserved">new</span> <span class="type">T</span>[capacity];
    }

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> 値を追加。</span>
    <span class="inactive">///</span><span class="comment"> 容量を超えた分は古いものから削除。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;param name="</span>item<span class="inactive">"&gt;</span><span class="comment">新しい値。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Push(<span class="type">T</span> item)
    {
        _data[_startIndex] = item;
        _startIndex++;
        <span class="reserved">if</span> (_startIndex &gt;= _data.Length) _startIndex = 0;
    }

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> 先頭要素。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> Head =&gt; <span class="reserved">ref</span> _data[_startIndex];

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> 先頭から </span><span class="inactive">&lt;paramref name="</span>index<span class="inactive">"/&gt;</span><span class="comment"> 先の要素。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;param name="</span>index<span class="inactive">"&gt;</span><span class="comment">先頭からの位置。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;returns&gt;&lt;/returns&gt;</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> _data[(_startIndex + index) % _data.Length];
}
</code></pre>

### <a id="sec-generated-title-28"></a>補足: 配列のインデクサー
本節で挙げた例で、配列のインデクサーはユーザー定義のインデクサーと挙動が違うことにお気づきでしょうか。
実は、配列のインデクサーは参照を返しています。

C# 6までは参照戻り値のための構文がなく、ユーザー定義のインデクサーでは参照を返す手段はありませんでした。
しかし、配列は特別扱いを受けていて、インデクサーが参照になっています。
例えば、以下のようなコードを書くと、配列の方だけ正常にコンパイルできます。

<pre class="source" title="配列のインデクサーは最初から参照を返してる">
<code><span class="reserved">var</span> array = <span class="reserved">new</span>[]
{
    <span class="reserved">new</span> <span class="type">Point</span>(),
    <span class="reserved">new</span> <span class="type">Point</span>(),
};
<span class="comment">// 配列のインデクサーは要素への参照になってる</span>
<span class="comment">// 値型の要素の書き換え可能</span>
array[0].X = 1; <span class="comment">// OK</span>

<span class="reserved">var</span> list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="type">Point</span>&gt;
{
    <span class="reserved">new</span> <span class="type">Point</span>(),
    <span class="reserved">new</span> <span class="type">Point</span>(),
};
<span class="comment">// これまで、ユーザー定義のインデクサーは参照返せなかった</span>
<span class="comment">// 当然、C# 6以前からあるクラスのインデクサーは値型の要素の書き換え不能</span>
list[0].X = 1; <span class="comment">// コンパイル エラー</span>
</code></pre>

<!-- original-page-break -->

##<a id="sec-generated-title-29"></a> <a id="pointer"></a>参照渡しとポインター
少し内部的な話もしておきましょう。
内部的には、参照渡しとポインターは似たようなものです。

もちろん、型システム上の扱いとしては、以下のような差があります。

| 参照渡し | ポインター |
| ---- | ---- |
| 通常のコンテキスト内で使える代わりに、制限がきつい | [unsafe](../interop/sp_unsafe.md#unsafe)コンテキストでしか使えない代わりに、自由が利く |
| 基本的に、有効なオブジェクトしか参照できない | どこでも参照できる。`p + 1`など、数値との加減算して隣接するメモリを参照できる |
| どんな型でも参照できる | 「[アンマネージ型](../interop/sp_unsafe.md#function)」と呼ばれる一部の型しか参照できない |

しかし、読み書きに使われる命令的には参照渡しとポインターは全く同じだったりします。
例えば、以下の2つのメソッドを見てみましょう。

<pre class="source" title="参照渡しとポインターの比較の例">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Max(<span class="reserved">ref</span> <span class="reserved">int</span> x, <span class="reserved">ref</span> <span class="reserved">int</span> y)
{
    <span class="reserved">if</span> (x &gt;= y) <span class="reserved">return</span> <span class="reserved">ref</span> x;
    <span class="reserved">else</span> <span class="reserved">return</span> <span class="reserved">ref</span> y;
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">int</span>* Max(<span class="reserved">int</span>* x, <span class="reserved">int</span>* y)
{
    <span class="reserved">if</span> (*x &gt;= *y) <span class="reserved">return</span> x;
    <span class="reserved">else</span> <span class="reserved">return</span> y;
}
</code></pre>

やっていることは全く同じで、ただ型的に参照渡しかポインターかが違います。
このコードのコンパイル結果は、下図のように、ほとんど同じになります。

![参照渡しとポインターを使ったコードのコンパイル結果](../../../../assets/media/1127/refandpointer.png)

型としては、引数と戻り値のところを見ての通り、`&`と`*`の差があります(`&`が参照渡しで、`*`がポインターです)。
一方で、メソッドの中身に関しては一字一句たがわず同じです。

`ldind`はload indirect (間接ロード)の略で、 ポインターや参照ごしに値を取ってくる命令ですが、 ポインターと参照でまったく同じ命令を使います。

###<a id="sec-generated-title-30"></a> <a id="as-pointer"></a>参照渡しとポインターの相互変換
命令上互換性があるわけで、やろうと思えば参照渡しとポインターの間で相互変換が可能です。
C#を使って書けるコードではありませんが、[IL](../../il/index.md)を使えば書けます。

そのILで書かれたライブラリを参照すれば、C#からも参照渡し⇔ポインターの変換ができます。
[CoreFX](https://github.com/dotnet/corefx)による公式実装があって、以下のように、NuGetパッケージとして公開されています。

- [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)

このパッケージ中にある`Unsafe`クラスを使うと、以下のようなコードが書けます。

<pre class="source" title="Unsafeクラスを使って参照渡しとポインターを変換する例">
<code><span class="reserved">unsafe</span>
{
    <span class="reserved">int</span> x = 1;
    <span class="reserved">void</span>* pointer = <span class="type">Unsafe</span>.AsPointer(<span class="reserved">ref</span> x);
    *(<span class="reserved">int</span>*)pointer = 2;

    <span class="type">Console</span>.WriteLine(x); <span class="comment">// 2 になってる</span>

    <span class="reserved">ref</span> <span class="reserved">int</span> r = <span class="reserved">ref</span> <span class="type">Unsafe</span>.AsRef&lt;<span class="reserved">int</span>&gt;(pointer);
    r = 3;

    <span class="type">Console</span>.WriteLine(*(<span class="reserved">int</span>*)pointer); <span class="comment">// 3 になってる</span>
}
</code></pre>

これで何がうれしいかというと、以下のように、タイプが異なるいろんなメモリ領域を統一的に扱えたりすることです。
また、ポインターを使う部分にはunsafeコンテキストが必要ですが、作られたクラスを使うだけなら、使う側にはunsafeを求めません。

<pre class="source" title="いろんなメモリ領域を統一的に扱う例">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Runtime.InteropServices;

<span class="reserved">struct</span> <span class="type">ManagedBuffer</span>
{
    <span class="reserved">int</span>[] _array;
    <span class="reserved">public</span> ManagedBuffer(<span class="reserved">int</span> length) { _array = <span class="reserved">new</span> <span class="reserved">int</span>[length]; }

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> _array[index];
}

<span class="reserved">unsafe</span> <span class="reserved">struct</span> <span class="type">UnsafeBuffer</span>
{
    <span class="reserved">void</span>* _pointer;
    <span class="reserved">public</span> UnsafeBuffer(<span class="reserved">int</span>* pointer) { _pointer = pointer; }

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> <span class="type">Unsafe</span>.AsRef&lt;<span class="reserved">int</span>&gt;(_pointer);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 配列と</span>
        <span class="reserved">var</span> b1 = <span class="reserved">new</span> <span class="type">ManagedBuffer</span>(10);
        b1[0] = 1;

        <span class="comment">// スタック領域と</span>
        <span class="reserved">var</span> stack = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[10];
        <span class="reserved">var</span> b2 = <span class="reserved">new</span> <span class="type">UnsafeBuffer</span>(stack);
        b2[0] = 1;

        <span class="comment">// アンマネージなメモリとを同じように触れる</span>
        <span class="reserved">var</span> p = <span class="type">Marshal</span>.AllocHGlobal(10 * <span class="reserved">sizeof</span>(<span class="reserved">int</span>));
        <span class="reserved">var</span> b3 = <span class="reserved">new</span> <span class="type">UnsafeBuffer</span>((<span class="reserved">int</span>*)p);
        b3[0] = 1;

        <span class="type">Marshal</span>.Release(p);
    }
}
</code></pre>

特に、C# の管理外の世界からもらったアンマネージなメモリ領域を手軽に参照できるのは、パフォーマンスの改善に大きく寄与します。

一方で、もちろん、unsafeコンテキストを経由するので、通常のC#の感覚からするとおかしなこともできます。
例えば、本節の冒頭の表で「参照渡しは有効なオブジェクトしか参照できない」という説明をしましたが、
この制約を破ることができます。
例えば、以下のようなコードで、「参照渡しのnull」を作れます。

<pre class="source" title="参照渡しのnull">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">NullReference</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="type">T</span> Null&lt;<span class="type">T</span>&gt;() =&gt; <span class="reserved">ref</span> <span class="type">Unsafe</span>.AsRef&lt;<span class="type">T</span>&gt;((<span class="reserved">void</span>*)0);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> IsNull&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> x) =&gt; <span class="type">Unsafe</span>.AsPointer(<span class="reserved">ref</span> x) == (<span class="reserved">void</span>*)0;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">ref</span> var x = <span class="reserved">ref</span> <span class="type">NullReference</span>.Null&lt;<span class="reserved">int</span>&gt;();
        <span class="type">Console</span>.WriteLine(<span class="type">NullReference</span>.IsNull(<span class="reserved">ref</span> x)); <span class="comment">// true</span>
        <span class="type">Console</span>.WriteLine(x); <span class="comment">// 実行時エラー。NullReferenceException 発生</span>
    }
}
</code></pre>

注意して使いましょう。
