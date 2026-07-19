---
title: "クラス"
source_url: "https://ufcpp.net/study/csharp/oop/oo_class/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2021-03-07T00:00:00"
tags: []
umbraco_id: 1250
parent_id: 1248
sort_order: 1
aliases:
  - "/csharp/oo_class"
  - "/csharp/oo_class.html"
  - "/csharp/oop/oo_class/"
  - "/study/csharp/oo_class"
  - "/study/csharp/oo_class.html"
---

# クラス

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

クラスとはオブジェクトを作るための設計図のようなもので、
オブジェクト指向プログラミングの中心となるものです。


##### <a id="sec-generated-title-2"></a>ポイント

* クラス: オブジェクトを作るための設計図。

* インスタンス: 設計図を基に作られた個々の実体。

* 例えば、
    * `class Point { public int X; public int Y; }` でクラスを作って、

    * `Point p = new Point();` でインスタンスを作る。



* 構造体との違いは「値型」か「参照型」か、継承できるかどうか。



## <a id="sec-generated-title-3"></a> <a id="class"></a>クラスとインスタンス

「[オブジェクト指向とは](oo_about.md)」で述べたように、
操作の対象となるものをオブジェクトといいます。
オブジェクトを作る場合、まず設計図が必要になります。
内部がどういう構造になっているのか、外部からどのような操作をすることが出来るのかを決めてやるわけです。
このようなオブジェクトの設計図のことを<strong id="class" class="keyword">クラス</strong>(class)といいます。
それに対し、設計図を元に作られたオブジェクトの実体のことを<strong id="instance" class="keyword">インスタンス</strong>(instance)といいます。

<table summary="クラスとインスタンスの比喩的例">
	<caption>
		クラスとインスタンスの比喩的例
	</caption>
	<tr>
		<th>クラス</th>
		<th>インスタンス</th>
	</tr>
	<tr>
		<td markdown="1">製品規格</td>
		<td markdown="1">個々の製品</td>
	</tr>
	<tr>
		<td markdown="1">人間</td>
		<td markdown="1">松井君、空知君、畑君、田辺さん・・・</td>
	</tr>
	<tr>
		<td markdown="1">実数全体<span class="math">
            <span class="bold">R</span>
          </span></td>
		<td markdown="1">実数値<span class="math">x, y,</span>・・・</td>
	</tr>
	<tr>
		<td markdown="1">初等関数</td>
		<td markdown="1"><span class="math">
            <span class="normal">sin</span>, <span class="normal">cos</span>, <span class="normal">exp</span>, <span class="normal">log</span>,
          </span>・・・</td>
	</tr>
</table>



## <a id="sec-generated-title-4"></a> <a id="definition"></a>クラス定義

C#では以下のようにしてクラスを定義します。

<pre class="source" title="クラス定義のしかた" lang="">
<code><span class="reserved">class</span> <span class="input">クラス名</span>
{
  <span class="input">クラスの実装</span>
}
</code></pre>

クラスの実装にはメンバ変数の定義とメソッド(メンバー関数)の定義などをします。
メンバー変数とはクラスの内部で宣言される[変数](../../../../assets/st_variable.html)のことで、
メソッドの定義はクラスの内部で宣言される[関数](../../../../assets/st_function.html)のことだと思ってもらって結構です。
以下に例を示します。

<pre class="source" title="クラス定義の例" lang="">
<code><span class="reserved">class</span> Sample
{
  <span class="comment">// メンバー変数の定義 ここから↓</span>
  <span class="reserved">private int</span> x;
  <span class="reserved">private int</span> y;
  <span class="comment">// メンバー変数の定義 ここまで↑</span>

  <span class="comment">// メソッドの定義 ここから↓</span>
  <span class="reserved">public int</span> GetX()
  {
    <span class="reserved">return</span> x;
  }

  <span class="reserved">public int</span> GetY()
  {
    <span class="reserved">return</span> y;
  }

  <span class="reserved">public void</span> Set(<span class="reserved">int</span> a, <span class="reserved">int</span> b)
  {
    x = a;
    y = b;
  }
  <span class="comment">// メソッドの定義 ここまで↑</span>
}
</code></pre>


<code>private</code>や<code>public</code>といったキーワードについては「[実装の隠蔽](oo_conceal.md)」で解説します。

メンバー変数によってオブジェクトの内部の実装を記述し、メソッドによって外部から行える操作を記述するわけです。
以下、具体的な例を挙げるために複素数を表すクラスを作ってみましょう。
まず、複素数に対する操作には何があるかを列挙してみましょう。

* 実部の取り出し・変更

* 虚部の取り出し・変更

* 絶対値の取り出し・変更

* 偏角の取り出し・変更

* 四則演算

* 共役複素数の計算


次に、複素数を実装する方法を考えて見ましょう。

* 実部と虚部を記憶しておく

* 絶対値と偏角を記憶しておく


いきなりすべてを実装するのは大変ですから、
まず、実部と虚部の取り出し・変更と、絶対値の取り出しを、
実部と虚部を記憶しておく方式で実装してみます。

<pre class="source" title="複素数クラス その1" lang="">
<code><span class="reserved">class</span> Complex
{
  <span class="reserved">public double</span> re; <span class="comment">// 実部を記憶しておく(外部からの読み出し・書き換えも可能)</span>
  <span class="reserved">public double</span> im; <span class="comment">// 虚部を記憶しておく(外部からの読み出し・書き換えも可能)</span>

  <span class="comment">// 絶対値を取り出す</span>
  <span class="reserved">public double</span> Abs()
  {
    <span class="reserved">return</span> Math.Sqrt(re*re + im*im);<span class="comment">// Math.Sqrt は平方根を求める関数</span>
  }
}
</code></pre>


最初ということで、シンプルになるように実装しましたが、今後、徐々にちゃんとした形のものにしていきます。


## <a id="sec-generated-title-5"></a> <a id="use"></a>クラスの利用

クラスを利用するためには、
インスタンスを作成しなければなりません。
そのためにまず、インスタンスを格納するための変数を定義します。
変数定義の仕方は以下のような構文になります。

<pre class="source" title="インスタンスを格納するための変数の定義" lang="">
<code><span class="input">クラス名</span> <span class="input">変数名</span>;
</code></pre>

(ちなみに、[C# 9.0 からは `new` の後ろのクラス名を省略できることがあります](oo_construct.md#target-typed-new)。)

次に、<code>new</code> キーワードでインスタンスを作成し、用意した変数に格納します。

ここで注意すべきことは、C# において、変数というのはただの入れ物であって、
変数を宣言しただけではインスタンスは作成されません。
（空っぽの入れ物だけができる。）
以下のように、<code>new</code> して始めてインスタンスが生成されます。

<pre class="source" title="インスタンスの作成" lang="">
<code><span class="input">変数</span> = <span class="reserved">new</span> <span class="input">クラス名</span>();
</code></pre>

そして、以下のように変数の後に「 <code>.</code> 」で区切ってメンバー名を書くことでメンバー変数やメンバー関数を利用できます。

<pre class="source" title="メンバーの呼び出し" lang="">
<code><span class="input">変数名</span>.<span class="input">メンバー名</span>
</code></pre>


例として先ほど作成した複素数クラスのインスタンスを生成し、利用してみましょう。

<pre class="source" title="インスタンスの作成例" lang="">
<code>Complex z;         <span class="comment">// インスタンスを格納するための変数を定義</span>
z = <span class="reserved">new</span> Complex(); <span class="comment">// new を使ってインスタンスを生成</span>

z.re = 3;             <span class="comment">// 実部の値を変更</span>
z.im = 4;             <span class="comment">// 虚部の値を変更</span>
<span class="reserved">double</span> abs = z.Abs(); <span class="comment">// z の絶対値を取得</span>

Console.Write(<span class="literal">"abs = {0}\n"</span>, abs); <span class="comment">// abs = 5 と表示される</span>
</code></pre>


また、組込み型や配列と同様に変数の宣言と同時にインスタンスを作成して初期化することも出来ます。

<pre class="source" title="宣言時に初期化" lang="">
<code><span class="reserved">int</span> n = 5;
<span class="reserved">string</span> s = <span class="literal">"abcde"</span>;
<span class="reserved">int</span>[] array = <span class="reserved">new int</span>[]{1, 2, 3, 4, 5};
Complex z = <span class="reserved">new</span> Complex();
</code></pre>



##### <a id="sec-generated-title-6"></a>サンプル

<pre class="source" title="クラスのサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 複素数クラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Complex
{
  <span class="reserved">public double</span> re; <span class="comment">// 実部</span>
  <span class="reserved">public double</span> im; <span class="comment">// 虚部

  /// &lt;summary&gt;
  /// 絶対値を返す
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Abs()
  {
    <span class="reserved">return</span> Math.Sqrt(re*re + im*im);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 文字列化する
  /// &lt;/summary&gt;</span>
  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">if</span>(im &gt;0)
      <span class="reserved">return</span> re.ToString() + <span class="literal">"+i"</span> + im.ToString();
    <span class="reserved">if</span>(im &lt; 0)
      <span class="reserved">return</span> re.ToString() + <span class="literal">"-i"</span> + (-im).ToString();
    <span class="reserved">return</span> re.ToString();
  }
}<span class="comment">// class Complex

//================================================</span>
<span class="reserved">class</span> ClassSample
{
  <span class="reserved">static void</span> Main()
  {
    Complex z = <span class="reserved">new</span> Complex();

    z.re = GetDouble(<span class="literal">"実部を入力してください : "</span>);
    z.im = GetDouble(<span class="literal">"虚部を入力してください : "</span>);

    Console.Write(<span class="literal">"|{0}| = {1}\n"</span>, z, z.Abs());
  }

  <span class="comment">// 「関数」のところで作った実数入力用関数</span>
  <span class="reserved">static double</span> GetDouble(<span class="reserved">string</span> message)
  {
    <span class="reserved">double</span> x;
    <span class="reserved">while</span>(<span class="reserved">true</span>)
    {
      <span class="reserved">try</span>
      {
        <span class="comment">// 入力を促すメッセージを表示して、値を入力してもらう</span>
        Console.Write(message);
        x = <span class="reserved">double</span>.Parse(Console.ReadLine());
      }
      <span class="reserved">catch</span>(Exception)
      {
        <span class="comment">// 不正な入力が行われた場合の処理</span>
        Console.Write(
          <span class="literal">"error : 正しい値が入力されませんでした\n入力しなおしてください\n"</span>);
        <span class="reserved">continue</span>;
      }
      <span class="reserved">break</span>;
    }
    <span class="reserved">return</span> x;
  }
}
</code></pre>

## <a id="sec-generated-title-7"></a> <a id="null"></a>null

前節で、クラスを使う際にはまず「`new` キーワードでインスタンスを作る」と説明しましたが、
インスタンスを持たない(作るのを後回しにしたり、使い終わったものを手放したりする)場合の話もしておきます。

C# では、「有効なインスタンスを持っていない」という状態を<strong id="null" class="keyword">null</strong>（ヌル: 空っぽ、0）と呼び、`null` キーワードで表します。

<pre class="source" title="null 値" lang="">
<code><span class="input">変数</span> = <span class="reserved">null</span>;
</code></pre>

よくある用途としては、必要になるまでインスタンス生成を遅らせたり(遅延初期化と言ったりします)です。
[プロパティ](oo_property.md)や[null 合体演算子](../resource/sp2_nullable.md#coalescing)を使うことが多く、今は「こういう書き方がある」くらいの説明しかできませんが、以下のような使い方ができます。

<pre class="source" title="必要になるまでインスタンスを作らない例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.ComponentModel;
<span class="reserved">using</span> System.Reflection;
 
<span class="comment">// System.Type から、自分のプログラムで使う属性とかを抽出するためのクラス</span>
<span class="reserved">class</span> <span class="type">TypeInfo</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">Type</span> _type;
    <span class="reserved">public</span> <span class="type">TypeInfo</span>(<span class="type">Type</span> <span class="variable">type</span>) =&gt; _type = <span class="variable">type</span>;
 
    <span class="comment">// 初期状態で null にしておく。</span>
    <span class="reserved">private</span> <span class="reserved">string</span> _description = <span class="reserved">null</span>;
 
    <span class="comment">// このメソッドの処理はだいぶ重たい。</span>
    <span class="comment">// なので必要になるぎりぎりまで呼びたくない。</span>
    <span class="reserved">private</span> <span class="reserved">string</span> <span class="method">GetDescription</span>() =&gt; _type.<span class="method">GetCustomAttribute</span>&lt;<span class="type">DescriptionAttribute</span>&gt;().Description;
 
    <span class="comment">// 始めてこのプロパティが呼ばれたときに、まだ _description が null のときだけ GetDescription を呼ぶ。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> Description =&gt; _description ??= <span class="method">GetDescription</span>();
}
</code></pre>

また、「有効なインスタンスを取れなかった」ということを表すのに使ったりもします。

<pre class="source" title="有効なものがないことを表すために null を使う例">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// &quot;abcdefg&quot; が条件を満たすのでこれが返ってくる。</span>
        <span class="reserved">var</span> <span class="variable">a</span> = <span class="method">FirstLongString</span>(<span class="reserved">new</span>[] { <span class="string">&quot;a&quot;</span>, <span class="string">&quot;abcd&quot;</span>, <span class="string">&quot;abcdefg&quot;</span>, <span class="string">&quot;abc&quot;</span> });
 
        <span class="comment">// 条件を満たすものがないので null が返ってくる。</span>
        <span class="reserved">var</span> <span class="variable">b</span> = <span class="method">FirstLongString</span>(<span class="reserved">new</span>[] { <span class="string">&quot;a&quot;</span>, <span class="string">&quot;abcd&quot;</span>, <span class="string">&quot;abcd&quot;</span>, <span class="string">&quot;abc&quot;</span> });
    }
 
    <span class="comment">// 配列中から特定の条件を満たす最初のインスタンスを探す。</span>
    <span class="comment">// (例として、長さ 5 以上の文字列を探す。)</span>
    <span class="reserved">static</span> <span class="reserved">string</span> <span class="method">FirstLongString</span>(<span class="reserved">string</span>[] <span class="variable">items</span>)
    {
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">items</span>)
        {
            <span class="control">if</span> (<span class="variable">x</span>.Length &gt;= 5) <span class="control">return</span> <span class="variable">x</span>;
        }
 
        <span class="comment">// 条件を満たすものがなかったことを表すために null を返す。</span>
        <span class="control">return</span> <span class="reserved">null</span>;
    }
}
</code></pre>

ちなみに、null は[参照型](../resource/oo_reference.md#reftype)の[既定値](../resource/rm_default.md)になります。

詳しくは「[null の取り扱い](../resource/rm_nullusage.md)」というページもあるのでこちらも参照してみてください。

<h5 class="version version2">Ver. 2.0</h5>

元々は参照型にしかなかった概念ですが、
C# 2.0 からは[null許容値型](../resource/sp2_nullable.md)という機能を使うことで値型でも null を使えるようになりました。

<h5 class="version version8">Ver. 8.0</h5>

また、C# 8.0 では「nullが本当に必要かどうか」を明示的に指定できるように、
[null許容参照型](../resource/nullablereferencetype.md)という機能が入りました。

## <a id="sec-generated-title-8"></a> <a id="this-access"></a>this アクセス

クラス中では、`this`というキーワードが特別な意味を持ちます。
`this`は、英単語の意味(これ、この)通り「このインスタンス自身」を表す特別な変数になります。

通常はあってもなくてもいいものなんですが、
例えば、ローカル変数と同名のフィールドがあったときに、フィールドの方を参照するために使えます。

<pre class="source" title="this でフィールドとローカル変数を弁別">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="comment">// 小文字 x, y でフィールドを定義</span>
    <span class="reserved">int</span> x;
    <span class="reserved">int</span> y;

    <span class="comment">// 同じ x で引数を定義</span>
    <span class="comment">// y の方は名前を変えてみる</span>
    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> a)
    {
        <span class="comment">// this. が付いている方はフィールド</span>
        <span class="comment">// ついていない方は引数</span>
        <span class="reserved">this</span>.x = x;

        <span class="comment">// y の方は this. を付けなくても、他に候補がないのでフィールドの y</span>
        y = a;

        <span class="comment">// この場合、this. を付けても y と同じ意味</span>
        <span class="reserved">var</span> b = <span class="reserved">this</span>.y;
    }
}
</code></pre>

あるいは、メソッドの引数に自分自身を渡したりするときに使います。

<pre class="source" title="this で、メソッドの引数に自分自身を渡す">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="comment">// 前略</span>

    <span class="comment">// Point を引数として受け取るメソッドがあったとして、</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Write(<span class="type">Point</span> p)
    {
        System.<span class="type">Console</span>.WriteLine(<span class="string">$"(</span>{p.x}<span class="string">, </span>{p.y}<span class="string">)"</span>);
    }

    <span class="comment">// そのメソッドに「自分自身」を渡す</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Write() =&gt; Write(<span class="reserved">this</span>);
}
</code></pre>

その他、[インデクサー](oo_indexer.md)や[拡張メソッド](../functional/sp3_extension.md)など、`this`を常に必要とする構文があります。

<pre class="source" title="this を常に必要とする構文">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X, Y;

    <span class="reserved">public</span> <span class="reserved">void</span> M()
    {
        <span class="reserved">var</span> x = <span class="reserved">this</span>[0]; <span class="comment">// インデクサーの呼び出し</span>
        <span class="reserved">var</span> l = <span class="reserved">this</span>.LengthSquared(); <span class="comment">// 拡張メソッドの呼び出し</span>
    }

    <span class="comment">// インデクサー</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> i]
        =&gt; i == 0 ? X
        : i == 1 ? Y
        : <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">IndexOutOfRangeException</span>();
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">PointExtensions</span>
{
    <span class="comment">// 拡張メソッド</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> LengthSquared(<span class="reserved">this</span> <span class="type">Point</span> p) =&gt; p.X * p.X + p.Y * p.Y;
}
</code></pre>

## <a id="sec-generated-title-9"></a> <a id="struct"></a>クラスと構造体

ここまでの説明を見て、
「[クラス](#class)」と「[構造体](../structured/st_struct.md#struct)」の類似性に気付いた方もいるかと思います。
実際、メンバー変数やメソッドの定義は構造体でもできます。

クラスと構造体の違いを説明するためには、
継承や多態性などのオブジェクト指向の概念や、
値型と参照型というプログラミングの概念の理解が必要になります。
これらの概念の詳細は、
「[継承](oo_inherit.md)」、
「[多態性](oo_polymorphism.md)」、
「[値型と参照型](../resource/oo_reference.md)」
などで説明することにして、
ここでは簡単に概要だけを表にまとめます。

<table summary="クラスと構造体">
	<caption>
		クラスと構造体
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>クラス</th>
		<th>構造体</th>
	</tr>
	<tr>
		<th>型の分類</th>
		<td markdown="1">参照型</td>
		<td markdown="1">値型</td>
	</tr>
	<tr>
		<th>継承</th>
		<td markdown="1">できる</td>
		<td markdown="1">できない</td>
	</tr>
	<tr>
		<th>多態性</th>
		<td markdown="1">使える</td>
		<td markdown="1">使えない</td>
	</tr>
</table>


迷うようならクラスにしておけばいいと思います。
構造体は、以下の条件がそろっている場合にのみ使います。

* データのサイズが小さい（目安としては16バイト程度以下）

* 絶対に継承しないと分かっている

* 変数への代入がコピーを生むというのが許容できる



## <a id="sec-generated-title-10"></a> <a id="partial_class"></a><a id="partial"></a>クラスの分割定義

<a id="partial_method"></a>
<a id="partial_method-side-effect"></a>
<a id="extended_partial_method"></a>
<a id="contextual-partial-keyword"></a>
ページを分割しました:

* [型の分割定義 (partial)](../misc/partial-type.md)

## <a id="sec-generated-title-11"></a> <a id="anonymous"></a>匿名型

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では<strong id="anonytype" class="keyword">匿名型</strong>（anonymous type）を作成できるようになりました。
匿名型の作り方は以下の通りです。

<pre class="source" title="匿名型" lang="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> { FamilyName = <span class="literal">"糸色"</span>, FirstName=<span class="literal">"望"</span>};
</code></pre>


このようなコードから、自動的に、以下のような型が生成されます。

<pre class="source" title="匿名型によって自動生成されるクラス" lang="">
<code><span class="comment">// ↓この __Anonymous という名前はプログラマが参照できるわけではない。</span>
<span class="reserved">class</span> <span class="type">__Anonymous1</span>
{
  <span class="reserved">private string</span> f1;
  <span class="reserved">private string</span> f2;
  
  <span class="reserved">public</span> __Anonymous1(<span class="reserved">string</span> f1, <span class="reserved">string</span> f2)
  {
    <span class="reserved">this</span>.f1 = f1;
    <span class="reserved">this</span>.f2 = f2;
  }

  <span class="reserved">public string</span> FamilyName
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.f1}
  };
  <span class="reserved">public string</span> FirstName
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.f2}
  };
  
  <span class="comment">// あと、Equals, GetHashCode, ToString も実装</span>
}
</code></pre>


この機能的は「[LINQ](../data/sp3_linq.md#linq)」とともに利用することで真価を発揮します。
単体で使う場面はそれほど多くないと思いますが、
例えば、以下のような書き方ができます。

<pre class="source" title="匿名型の利用" lang="">
<code><span class="reserved">var</span> rectangle = <span class="reserved">new</span> { Width = 2, Height = 3 };

<span class="type">Console</span>.Write(<span class="literal">"幅  : {0}\n高さ: {1}\n面積: {2}\n"</span>,
  rectangle.Width,
  rectangle.Height,
  rectangle.Width * rectangle.Height);
</code></pre>
## <a id="exercise"></a>演習問題

### <a id="exercise-str1"></a>問題 1


「[データの構造化](../structured/st_struct.md)」の[データの構造化](../structured/st_struct.md)の[問題 1](../structured/st_struct.md#exercise-str1)で作成した <code>Triangle</code> 構造体をクラスで作り直せ。
（<code>Point</code> 構造体は構造体のままで OK。）

注1：現時点では、
単に struct が class に変わるだけで、特にメリットはありませんが、
今後、
「[継承](oo_inherit.md)」」や「[多態性](oo_polymorphism.md)」を通して、
クラスのメリットを徐々に加えていく予定です。

注2：
クラスにした場合、メンバー変数をきちんと初期化してやらないと正しく動作しません。
（構造体でもメンバー変数の初期化はきちんとする方がいいんですが。）
初期化に関しては、次節の「[コンストラクターとデストラクター](oo_construct.md)」で説明します。
