---
title: "関数"
source_url: "https://ufcpp.net/study/csharp/structured/st_function/"
content_type: "Article"
published_at: "2001-11-17T00:00:00"
updated_at: "2016-10-25T00:00:00"
tags:
  - "Ver. 4.0"
  - "Ver. 6.0"
umbraco_id: 1233
parent_id: 1217
sort_order: 5
aliases:
  - "/csharp/st_function"
  - "/csharp/st_function.html"
  - "/csharp/structured/st_function/"
  - "/study/csharp/st_function"
  - "/study/csharp/st_function.html"
---

# 関数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

同じプログラムコードを複数の場所で何度も利用したい場合があります。
例えば、今まで説明してきた中で、たびたび「入力を促すメッセージを出力して、整数を入力してもらう」という場面が出てきました。
そのために何度も同じようなソースコードを書いてきました。

「[反復処理](st_loop.md)」のところでも説明しましたが、
同じコードを複数の箇所に書くのはプログラムを管理していく上で好ましくありません。
そこで、こういう頻繁に使われる機能をまとめて、何度も呼び出せるようにしたのが<strong id="function" class="keyword">関数</strong>（function）です。

(追記したい)
何度も出てくる処理でなくても、
処理に名前が付く単位で関数化すべき。
明確な名前を付ける(名前が付く単位で区切る)のがよいコードを書くコツ。


##### <a id="sec-generated-title-2"></a>ポイント

* 何度も出てくる処理は関数化する。

* 数学の「関数」から取った名前。プログラミング用語的には、他に、サブルーチン、プロシージャ（手続き）、メソッド等といった呼び方がある。

* C# の機能の呼び名としては、このページで説明しているものは正確には「メソッド」という。
    * とりあえず現時点では、メソッド ＝ 関数 と思っておいて OK。

    * C# の場合、関数的な挙動をするものがいくつかあって、そのうち、一番「関数らしい関数」がメソッド。





##### <a id="sec-generated-title-3"></a>サンプル

[https://github.com/ufcpp/UfcppSample/tree/master/Chapters/StructuredProgramming/Function](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/StructuredProgramming/Function)


## <a id="sec-generated-title-4"></a> <a id="sec-function-member"></a>補足: 関数メンバー

C# の場合、このページで説明するような「関数」的な動作、
すなわち、何らかの値を受け取って、処理して、結果の値を返すような挙動をするものがいくつかあります。
具体的には、以下のようなものがあります。

* メソッド
    * 拡張メソッド(参考:「[拡張メソッド](../functional/sp3_extension.md)」)



* コンストラクター(参考:「[コンストラクター](../oop/oo_construct.md)」)

* プロパティ(参考:「[プロパティ](../oop/oo_property.md)」)

* インデクサー(参考:「[インデクサー](../oop/oo_indexer.md)」)

* イベント(参考:「[イベント](../functional/sp_event.md)」)

* 演算子(参考:「[演算子のオーバーロード](../oop/oo_operator.md)」)

* ユーザー定義の型変換(参考:「[演算子のオーバーロード](../oop/oo_operator.md)」)

* ファイナライザー(参考: 「[ファイナライザー](../resource/rm_destructor.md)」)


これらを合わせて、<strong id="function-member" class="keyword">関数メンバー</strong>(function member)と呼びます。
このページで説明しているものは、C# の機能名としては、正確には<strong id="method" class="keyword">メソッド</strong>といいます。

また、数学の関数を引き合いに出して説明していますが、
数学の関数のイメージと一番合うのは、「静的メソッド」というものです
(メソッドにもインスタンス メソッドと静的メソッドの2種類があります)。
これについては、「[静的メンバー](../oop/oo_static.md)」で説明します。

このページでの説明(つまり、メソッドに対する説明)の多くは、メソッド以外の関数メンバーにも当てはまります。
引数の書き方、関数本体の書き方、戻り値の返し方などは、すべての関数メンバーで共通です。
(引数や戻り値が指定できない関数メンバーもありますが、指定できる場合には、書き方が同じです。)


## <a id="sec-generated-title-5"></a> <a id="definition"></a>関数定義

C# では、以下のようにして関数(C# 用語としては、正確にはメソッド)を定義します。

<pre class="source" title="関数の書式" lang="">
<code><span class="input">戻り値の型</span> <span class="input">関数名</span>(<span class="input">引数一覧</span>)
{
    <span class="input">関数本体(具体的な処理)</span>
}
</code></pre>


<h5 class="version version6">Ver. 6</h5>
また、C# 6 では、関数本体の部分が1つの式だけからなる場合、以下のような書き方をすることができるようになりました。
これを、expression-bodied (本体が式の)関数と呼びます(詳細は後述)。

<pre class="source" title="関数の書式" lang="">
<code><span class="input">戻り値の型</span> <span class="input">関数名</span>(<span class="input">引数一覧</span>) =&gt; <span class="input">関数本体の式</span>
</code></pre>


関数という名前は、数学用語の関数からきています。
数学の関数は、ある値を入力すると、一定のルールに従った出力が得られます。
また、入力することの出来る値の範囲や、出力として得られる値の範囲はしっかりと決められています。

C#の関数も同じように、入力出来る値と、出力される値の型をあらかじめ決めておかなければ行けません。
例えば、実数(浮動小数点数)を入力して、その値のsinを求めるような関数を作りたい場合、
以下のようにして関数を作ることが出来ます。

<pre class="source" title="関数の例 sin関数" lang="">
<code><span class="comment">// sin x を求める関数。
// テイラー展開を利用。
// かなり適当に作ってるので、この方法ではそんなに精度はよくない。</span>
<span class="reserved">double</span> Sin(<span class="reserved">double</span> x)
{
  <span class="reserved">double</span> xx = -x * x;
  <span class="reserved">double</span> fact = 1;
  <span class="reserved">double</span> sin = x;
  <span class="reserved">for</span>(<span class="reserved">int</span> i=1; i&lt;100;)
  {
    fact *= i; ++i; fact *= i; ++i;
    x *= xx;
    sin += x / fact;
  }
  <span class="reserved">return</span> sin;
}
</code></pre>


まず、コメントの部分を除いて1番最初の、「<code>
                <span class="reserved">double</span> Sin(<span class="reserved">double</span> x)
            </code>」の部分を細かく見ていくと、
先頭の「<code>
                <span class="reserved">double</span>
            </code>」が関数の出力(これを<strong id="return" class="keyword">戻り値</strong>という)の型、
次の「<code>Sin</code>」が関数の名前、
<code>()</code>の中にある「<code>
                <span class="reserved">double</span> x
            </code>」が入力の型と入力された値を保持するための変数(これを<strong id="paramter" class="keyword">引数</strong>(paramter)といいます)です。

その後に続く<code>{}</code>の中身が関数の内部で行う処理です。
そして、出力にしたい値(これを<strong id="return-value" class="keyword">戻り値</strong>(return value)といいます)は<strong id="return" class="keyword">return</strong>というキーワードの後ろに書きます。

作成した関数を呼び出すには、

<pre class="source" title="関数の書式" lang="">
<code><span class="input">変数</span> = <span class="input">関数名</span>(<span class="input">入力</span>)
</code></pre>


というように書きます。
以下に関数を呼び出す例を挙げます。


##### <a id="sec-generated-title-6"></a>サンプル

<pre class="source" title="sin関数を使ったサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> SinSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;10; ++i)
    {
      <span class="reserved">double</span> x = 0.01 * i;

<em>      <span class="reserved">double</span> y = Sin(x); <span class="comment">// 関数呼び出し</span></em>

      Console.Write(<span class="literal">"sin({0:f2}) = {1:f6}\n"</span>, x, y);
    }
  }

  <span class="comment">/// &lt;summary&gt;
  /// sin(x) の値を求める。
  /// 実装は割りと適当。
  /// &lt;/summary&gt;</span>
<em>  <span class="reserved">static double</span> Sin(<span class="reserved">double</span> x) <span class="comment">// 関数定義</span></em>
  {
    <span class="reserved">double</span> xx = -x * x;
    <span class="reserved">double</span> fact = 1;
    <span class="reserved">double</span> sin = x;
    <span class="reserved">for</span>(<span class="reserved">int</span> i=1; i&lt;100;)
    {
      fact *= i; ++i; fact *= i; ++i;
      x *= xx;
      sin += x / fact;
    }
    <span class="reserved">return</span> sin;
  }
}
</code></pre>


<pre class="console" title="">
sin(0.00) = 0.000000
sin(0.01) = 0.010000
sin(0.02) = 0.019999
sin(0.03) = 0.029996
sin(0.04) = 0.039989
sin(0.05) = 0.049979
sin(0.06) = 0.059964
sin(0.07) = 0.069943
sin(0.08) = 0.079915
sin(0.09) = 0.089879
</pre>


<code>Sin</code> 関数の定義の部分の前についている <code>
                <span class="reserved">static</span>
            </code> というキーワードについては、
「[静的メンバー](../oop/oo_static.md)」で説明します。

もう一つ違う例を挙げて見ましょう。
今まで、実数の入力は以下のようにして行っていました。

<pre class="source" title="値の入力" lang="">
<code>Console.Write(<span class="literal">"ユーザーに入力を促すメッセージ"</span>);
x = <span class="reserved">double</span>.Parse(Console.ReadLine());
</code></pre>


実数を入力する必要のある場面ごとにこのようなコードを書くのは面倒ですし、
これを関数化して見ましょう。
まず、単純に関数化した結果を以下に示します。

<pre class="source" title="値を入力する部分を関数化" lang="">
<code><span class="reserved">double</span> GetDouble(<span class="reserved">string</span> message)
{
  Console.Write(message);
  <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
  <span class="reserved">return</span> x;
}
</code></pre>


今までずっと無視してきていたのですが、実はこのままでは、実数に出来ない文字列を入力してしまうとエラーが発生して、以下のようなエラーメッセージを表示してプログラムが途中で止まってしまいます。

<pre class="console" title="不正な値が入力されたときのエラーメッセージ">
未処理の例外 : System.FormatException: 入力文字列の形式が正しくありません。
  at System.Number.ParseDouble(String s, NumberStyles style, NumberFormatInfo info)
  at System.Double.Parse(String s, NumberStyles style, IFormatProvider provider)
  at StatementSample2.Main()
</pre>


本当は例外処理(後述)というものを行ってこのようなエラーが出たときの対処を行わないといけません。
そこで、この例外処理を行うように変更を加えてみましょう。
(例外処理については「[例外処理](oo_exception.md)」参照。
<code>try</code>と<code>catch</code>は例外処理を行うための構文です。)

<pre class="source" title="値を入力する関数に例外処理を追加" lang="">
<code><span class="reserved">double</span> GetDouble(<span class="reserved">string</span> message)
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
</code></pre>


この修正した関数を用いて「[変数と式](../start/st_variable.md)」の最後で示したサンプルを書き換えてみましょう。


##### <a id="sec-generated-title-7"></a>サンプル

<pre class="source" title="値の入力部分を関数化したサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> StatementSample2
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">double</span> x, y, z; <span class="comment">// 変数を宣言。

    // x, y にユーザーの入力した値を代入。</span>
    x = GetDouble(<span class="literal">"input x : "</span>);
    y = GetDouble(<span class="literal">"input y : "</span>);

    <span class="comment">// 入力された値を元に計算</span>
    z = x * x + y * y; <span class="comment">// z に x と y の二乗和を代入</span>
    x /=  z;           <span class="comment">// x =  x / z; と同じ。</span>
    y /= -z;           <span class="comment">// y = -y / z; と同じ。

    // 計算結果を出力</span>
    Console.Write(<span class="literal">"({0}, {1})"</span>, x, y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 入力を促すメッセージを表示して、実数を入力してもらう。
  /// 正しく実数として解釈できる文字が入力されるまで繰り返す。
  /// &lt;param name="message"&gt; 入力を促すメッセージ &lt;/param&gt;
  /// &lt;return&gt; 入力された値 &lt;/return&gt;
  /// &lt;/summary&gt;</span>
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

### <a id="sec-generated-title-8"></a> <a id="return-statement"></a>returnの場所・数

これまでの例ではすべて、関数の最後に1つだけ`return`を書いていますが、C# には別にそういう縛りはありません。
`return` は関数の途中にも書けますし、1つの関数内に複数書けます。

複数個の`return`を書きたくなる1番の例は[条件分岐](st_branch.md)でしょう。
以下のように、条件を満たすときと満たさない時で別の値を返したい場合などです。

<pre class="source" title="条件ごとに異なる値をreturn">
<code><span class="reserved">static</span> <span class="reserved">int</span> Max(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
{
    <span class="reserved">if</span> (x &gt; y) <span class="reserved">return</span> x;
    <span class="reserved">else</span> <span class="reserved">return</span> y;
}
</code></pre>

分岐なしで関数の途中に`return`を書くこともできますが、この場合は、`return`よりも後ろは実行されません。

<pre class="source" title="returnの後ろは実行されない">
<code><span class="reserved">static</span> <span class="reserved">int</span> F(<span class="reserved">int</span> x)
{
    <span class="type">Console</span>.WriteLine(<span class="string">"ここは実行される"</span>);
    <span class="reserved">return</span> x;

    <span class="type">Console</span>.WriteLine(<span class="string">"<em>ここは実行されない</em>"</span>);
}
</code></pre>

## <a id="sec-generated-title-9"></a> <a id="arity"></a>引数・戻り値の数

引数や戻り値は、なくてもよかったり、複数書けたりします。

### <a id="sec-generated-title-10"></a> <a id="multiple-params"></a>引数が複数ある関数、

数学の関数では、例えば「f(x, y、z) = x<sup>2</sup>+y<sup>2</sup>+z<sup>2</sup>」といったように、入力が複数ある場合があります。
C#の関数でもこのように引数が複数ある関数を作れます。
引数を複数使いたい場合、数学の関数と同じように、関数を定義する際に、以下のように複数の引数を <code>,</code> で区切って並べます。
このように、引数を <code>,</code> で区切って並べたものを<em>引数リスト</em>といいます。

<pre class="source" title="引数が複数ある関数の例" lang="">
<code><span class="reserved">double</span> Norm(<span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z)
{
  <span class="comment">// ノルムの計算</span>
  <span class="reserved">return</span> x*x + y*y + z*z;
}
</code></pre>

### <a id="sec-generated-title-11"></a> <a id="no-param"></a>引数のない関数

数学ではあまり考えられませんが、C#では引数のない関数も定義できます。
引数のない関数は、以下のように、引数リストを空にして定義します。

<pre class="source" title="引数のない関数の例" lang="">
<code><span class="reserved">ulong</span> seed = 4275646295673547UL;
<span class="reserved">ulong</span> Random()
{
  <span class="comment">// 線形合同法による疑似乱数の生成</span>
  <span class="reserved">unchecked</span>{seed = seed * 1566083941UL + 1;}
  <span class="reserved">return</span> seed;
}
</code></pre>

### <a id="sec-generated-title-12"></a> <a id="void"></a>戻り値のない関数

同様に戻り値のない関数も定義できます。
戻り値のない関数は、以下のように、戻り値の型を <em>
                <code>void</code>
            </em> (「空の、何もない」という意味)というものにしておきます。

<pre class="source" title="戻り値のない関数の例" lang="">
<code><span class="reserved">void</span> WriteArray(<span class="reserved">int</span>[] array)
{
  Console.Write(<span class="literal">"{"</span>);
  <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;array.Length-1; ++i)
  {
    Console.Write(<span class="literal">"{0}, "</span>, array[i]);
  }
  Console.Write(array[array.Length-1] + <span class="literal">"}\n"</span>);
}
</code></pre>

戻り値のない物でも「関数」と呼ぶのはC言語やC++言語から受け継いだ習慣です。
その他の言語では、
戻り値のないものは「<em>サブルーチン</em>」とか「<em>プロシージャ</em>」といって関数と区別する場合もあります。

ちなみに、戻り値がない(`void`)の場合、`return`は書けますが、`return`の後ろには何も値を書かず、関数を途中で抜ける意味だけ持ちます。

<pre class="source" title="voidの時にはreturnの後ろに値を書かない">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span> x)
{
    <span class="reserved">if</span> (x &lt;= 0) <span class="reserved">return</span>;

    <span class="type">Console</span>.WriteLine(<span class="string">"x が正の時だけ実行される"</span>);
}
</code></pre>

### <a id="sec-generated-title-13"></a> <a id="unit"></a>補足1: void だけ特別扱いは不便

戻り値が`void`の関数は`return`の後ろに値を書けません。
これは`void`の場合だけの特別な書き方になります。
そして、特別に書き方を変えないといけないというのが面倒になることがあります。

例えば、C# では関数を変数に格納して使うことができるんですが、
戻り値がある場合は`Func`、ない場合は`Action`と、別の型に代入して使うことになります。

<pre class="source" title="FuncとActionを区別">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Action</span> a1 = A1; <span class="comment">// Func&lt;void&gt; とは書けない</span>
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; a2 = A2;
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f1 = F1; <span class="comment">// Action と Func が別</span>
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f2 = F2;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> A1() { } <span class="comment">// 戻り値がないと、=&gt; 記法も使えない</span>
    <span class="reserved">static</span> <span class="reserved">void</span> A2(<span class="reserved">int</span> x) { }
    <span class="reserved">static</span> <span class="reserved">int</span> F1() =&gt; 0;
    <span class="reserved">static</span> <span class="reserved">int</span> F2(<span class="reserved">int</span> x) =&gt; x;
}
</code></pre>

そこで、以下のように、空っぽの値を用意して、`void`の代わりに使うことですべて「戻り値あり」で統一する手法を時々使ったりします。

<pre class="source" title="空っぽの値でvoidを代用">
<code><span class="reserved">using</span> System;

<span class="comment">// 空っぽの型を1個用意</span>
<span class="reserved">struct</span> <span class="type">Unit</span> { }

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// void の代わりに Unit を使うことで、全部 Func に統一</span>
        <span class="type">Func</span>&lt;<span class="type">Unit</span>&gt; a1 = A1;
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="type">Unit</span>&gt; a2 = A2;
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f1 = F1;
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f2 = F2;
    }

    <span class="reserved">static</span> <span class="type">Unit</span> A1() =&gt; <span class="reserved">default</span>(<span class="type">Unit</span>); <span class="comment">// 空っぽの値を返しておく</span>
    <span class="reserved">static</span> <span class="type">Unit</span> A2(<span class="reserved">int</span> x) =&gt; <span class="reserved">default</span>(<span class="type">Unit</span>);
    <span class="reserved">static</span> <span class="reserved">int</span> F1() =&gt; 0;
    <span class="reserved">static</span> <span class="reserved">int</span> F2(<span class="reserved">int</span> x) =&gt; x;
}
</code></pre>

不格好なので積極的に使うものでもありませんが、統一のためにやむを得ないこともあったりします。

#### <a id="sec-generated-title-14"></a> <a id="why-unit"></a>Unitという名前

ちなみに、先ほどの例では、空っぽの型の名前をunit (単位元)にしていますが、
一応意味があってこの名前を使っています。
プログラミング用語しても使われるので、C#に限らず、たまに目にする言葉かもしれません。

この名前は数学用語に由来します。
数学では、以下のような表現をすることがあります。

- 0 = { }  … 0とは空っぽ(0要素)の集合である
- 1 = { 0 } … 1とは0のみを持つ(1要素の)集合である

unitというのはこの意味での「1」を指します。
先ほどの例では`defautl(Unit)`という意味のない値を返していますが、`Unit`型は、この意味のない値を1つだけ持つ型ということになります。

### <a id="sec-generated-title-15"></a> <a id="tuple"></a>補足2: 複数の戻り値(タプル)

C#では、基本的には戻り値は1つだけ返せます。

複数の値(多値)を返したいこともありますが、その場合、C# 6以前では[複合型](st_struct.md#about)を1つ作って返していました。

<pre class="source" title="多値戻り値のための複合型追加">
<code><span class="reserved">struct</span> <span class="type">SumCount</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> sum;
    <span class="reserved">public</span> <span class="reserved">int</span> count;
}

<span class="reserved">static</span> <span class="type">SumCount</span> Tally(<span class="reserved">int</span>[] items)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">var</span> count = 0;
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
    {
        sum += x;
        count++;
    }
    <span class="reserved">return</span> <span class="reserved">new</span> <span class="type">SumCount</span> { sum = sum, count = count };
}
</code></pre>

<h5 class="version version7">Ver. 7</h5>

この型に本当にちゃんとした意味があればいいんですが、この例の場合は見るからに大した意味がありません。
「和(sum)と個数(count)を表す`SumCount`型」なんていわれなくても、`sum`と`count`を見ればわかります。

そこで、C# 7では、以下のように書けるようになりました。

<pre class="source" title="C# 7で導入されたタプルを使って多値戻り値を返す">
<code><span class="reserved">static</span> (<span class="reserved">int</span> sum, <span class="reserved">int</span> count) Tally(<span class="reserved">int</span>[] items)
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">var</span> count = 0;
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
    {
        sum += x;
        count++;
    }
    <span class="reserved">return</span> (sum, count);
}
</code></pre>

複数の戻り値を返しているような書き心地になります。

これは実際には、`(int sum, int count)`という「名前のない型」(これをタプルと呼びます)を1つ作って、その値を返しています。
詳細は[名前のない複合型](st_anonymoustype.md)で説明します。

##### <a id="sec-generated-title-16"></a>サンプル

<pre class="source" title="さまざまな関数のサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> FunctionSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span>[] array = <span class="reserved">new int</span>[3];

    <span class="comment">// 乱数を使って値を生成</span>
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;array.Length; ++i)
    {
      array[i] = (<span class="reserved">int</span>)(Random() &gt;&gt; 58); <span class="comment">// [0,63] の整数乱数生成</span>
    }

    <span class="comment">// ノルムを計算</span>
    <span class="reserved">double</span> norm = Norm(array[0], array[1], array[2]);

    <span class="comment">// 値の出力</span>
    WriteArray(array);
    Console.Write(<span class="literal">"norm = {0}\n"</span>, norm);
  }

  <span class="reserved">static ulong</span> seed = 4275646293455673547UL;
  <span class="comment">/// &lt;summary&gt;
  /// 線形合同法による乱数の生成
  /// &lt;/summary&gt;</span>
  <span class="reserved">static ulong</span> Random()
  {
    <span class="reserved">unchecked</span>{seed = seed * 1566083941UL + 1;}
    <span class="reserved">return</span> seed;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 入力した3つの値のノルムを計算
  /// &lt;summary&gt;</span>
  <span class="reserved">static double</span> Norm(<span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z)
  {
    <span class="reserved">return</span> x*x + y*y + z*z;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列を , で各要素を区切って、{}で括った形式で出力
  /// &lt;summary&gt;</span>
  <span class="reserved">static void</span> WriteArray(<span class="reserved">int</span>[] array)
  {
    Console.Write(<span class="literal">"{"</span>);
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;array.Length-1; ++i)
    {
      Console.Write(<span class="literal">"{0}, "</span>, array[i]);
    }
    Console.Write(array[array.Length-1] + <span class="literal">"}\n"</span>);
  }
}
</code></pre>


<pre class="console" title="">
{40, 31, 39}
norm = 4082
</pre>



### <a id="sec-generated-title-17"></a> <a id="state"></a>補足: 状態

この例の `Random` は、数学の関数と違って、「状態」(state)を持っています。
一般に、数学の関数は、引数と戻り値の関係だけを説明していて、同じ引数を与えた場合、常に同じ戻り値が返ります。
一方、この例は、関数の外にある `seed` という変数に値を記録・書き換え(これを「状態を持つ」という)していて、呼ぶたびに状態が変わり、違う戻り値を返します。

ちなみに、この関数の外にある変数のことは、C# 的にはフィールド(field)と呼びます。詳しくは「[データの構造化](st_struct.md)」で説明します。


## <a id="sec-generated-title-18"></a> <a id="parameter"></a>補足: 引数

引数についてはいくつか補足があります。


### <a id="sec-generated-title-19"></a> <a id="default-parameter"></a>オプション引数と名前付き引数

<h5 class="version version4">Ver. 4.0</h5>

C# 4 から、引数に規定値(default value)を与えて、呼び出し時に省略できたり(optional)、名前付き(named)で引数を書けるようになりました。

<pre class="source" title="規定値付きのメソッド定義" lang="">
<code><span class="reserved">class</span> OptionalParameterSample
{
    <span class="reserved">public static void</span> Sample()
    {
        <span class="comment">// 引数の省略(optional parameter)</span>
        <span class="reserved">var</span> s1 = Sum();     <span class="comment">// Sum(0, 0, 0); と同じ意味。</span>
        <span class="reserved">var</span> s2 = Sum(<span class="literal">1</span>);    <span class="comment">// Sum(1, 0, 0); と同じ意味。</span>
        <span class="reserved">var</span> s3 = Sum(<span class="literal">1</span>, <span class="literal">2</span>); <span class="comment">// Sum(1, 2, 0); と同じ意味。</span>

        <span class="comment">// 名前付きで引数を与える(named parameter)</span>
        <span class="reserved">var</span> s4 = Sum(x: 1, y: 2, z: 3); <span class="comment">// Sum(1, 2, 3); と同じ意味。</span>
        <span class="reserved">var</span> s5 = Sum(z: 3);             <span class="comment">// Sum(0, 0, 3); と同じ意味。</span>
    }

    <span class="reserved">static int</span> Sum(<span class="reserved">int</span> x = <span class="literal">0</span>, <span class="reserved">int</span> y = <span class="literal">0</span>, <span class="reserved">int</span> z = <span class="literal">0</span>)
    {
        <span class="reserved">return</span> x + y + z;
    }
}
</code></pre>


詳しくは、「[オプション引数・名前付き引数](sp4_optional.md)」で説明します。

リンク先の余談、「[余談： なんでいまさら？](sp4_optional.md#fyi)」 で説明していますが、引数の規定値には「後から値を変えにくい」という問題があるので、使用する際には注意が必要です。
とはいえ、後から値を変えることもそう多くなく、便利に使える機能です。


### <a id="sec-generated-title-20"></a> <a id="actual-formal"></a>実引数と仮引数

引数という言葉は、「引数として渡す値」と「引数を受け取るための変数」という2つの意味で使われます。
区別が必要な場合、前者を<strong id="actual-parameter" class="keyword">実引数</strong>(actual parameter)、後者を<strong id="formal parameter" class="keyword">仮引数</strong>(formal parameter)と呼びます。

例えば、先ほどの例で言うと、以下のように、Norm 関数に渡す 3, 4, 5 などの数値が実引数、
Norm 関数の定義側にある x, y, z などの変数が仮引数です。

<pre class="source" title="" lang="">
<code><span class="reserved">static void</span> Main()
{
    <span class="reserved">var</span> norm = Norm(3, 4, 5); <span class="comment">// 3, 4, 5 が実引数</span>
}

<span class="reserved">static double</span> Norm(<span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z) <span class="comment">// x, y, z が仮引数</span>
{
    <span class="reserved">return</span> x * x + y * y + z * z;
}
</code></pre>



##### <a id="sec-generated-title-21"></a>余談: paramter と argument

少し余談になりますが、引数は、英語だと parameter 以外に、argument (ここでは「独立変数」の意味。数学用語としての argument)という単語を使うこともあります。
(ちなみに、parameter も、数学用語としては「媒介変数」という訳語になります。)

流儀によっては、実引数の意味で argument、仮引数の意味で parameter といように単語で呼び分けることもあるようです。
(どちらがどちらだったかわからなくなるので、あまり推奨はされません。結局、actual argument, formal argument という言い方もします。)


### <a id="sec-generated-title-22"></a> <a id="special-case"></a>特殊な引数

C# には、いくつか特殊な引数があります。
詳しくは別項で説明しています。

* ref, out:「[引数の参照渡し](../resource/sp_ref.md)」

* params:「[可変長引数](sp_params.md)」

* this:「[拡張メソッド](../functional/sp3_extension.md)」



## <a id="sec-generated-title-23"></a> <a id="overload"></a>関数のオーバーロード

関数を作る際、関数の名前が同じで引数リストだけが異なる関数を複数作ることが出来ます。
例えば、以下のように同じ名前の関数を作成することが出来ます。

<pre class="source" title="引数リストだけが異なる同じ名前の関数の例" lang="">
<code><span class="reserved">void</span> WriteTypeAndValue(<span class="reserved">string</span> s)
{
  Console.Write(<span class="literal">"文字列 : {0}\n"</span>, s);
}

<span class="reserved">void</span> WriteTypeAndValue(<span class="reserved">int</span> n)
{
  Console.Write(<span class="literal">"整数   : {0}\n"</span>, n);
}

<span class="reserved">void</span> WriteTypeAndValue(<span class="reserved">double</span> x)
{
  Console.Write(<span class="literal">"実数   : {0}\n"</span>, x);
}
</code></pre>


このように、引数リストだけが異なる関数を作ることを関数の<strong id="overload" class="keyword">オーバーロード</strong>(overload : 過負荷、上積み)といいます。

##### <a id="sec-generated-title-24"></a>サンプル

<pre class="source" title="関数のオーバーロードのサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> OverloadSample
{
  <span class="reserved">static void</span> Main()
  {
    WriteTypeAndValue(<span class="literal">"サンプル"</span>); <span class="comment">// WriteTypeAndValue(string) が呼ばれる</span>
    WriteTypeAndValue(13);         <span class="comment">// WriteTypeAndValue(int)    が呼ばれる</span>
    WriteTypeAndValue(3.14159265); <span class="comment">// WriteTypeAndValue(double) が呼ばれる</span>
  }

  <span class="comment">/// &lt;summary&gt;
  /// 型名と値を出力する(string 版)。
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> WriteTypeAndValue(<span class="reserved">string</span> s)
  {
    Console.Write(<span class="literal">"文字列 : {0}\n"</span>, s);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 型名と値を出力する(int 版)。
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> WriteTypeAndValue(<span class="reserved">int</span> n)
  {
    Console.Write(<span class="literal">"整数   : {0}\n"</span>, n);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 型名と値を出力する(double 版)。
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> WriteTypeAndValue(<span class="reserved">double</span> x)
  {
    Console.Write(<span class="literal">"実数   : {0}\n"</span>, x);
  }
}</code></pre>


<pre class="console" title="">
文字列 : サンプル
整数   : 13
実数   : 3.14159265
</pre>

### <a id="sec-generated-title-25"></a> <a id="non-ovarloadable"></a>オーバーロードできない例

C# のメソッドのオーバーロードにはいくつか制限があります。

引数の型違いのオーバーロードはできますが、引数名だけが違うオーバーロードは作れません。

<pre class="source" title="引数名違いのオーバーロードは無理">
<code><span class="comment">// F は、引数の型が違うので大丈夫</span>
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span> x) { }
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">string</span> x) { }

<span class="comment">// G は、引数の型まで一緒で、名前だけ違う。これはコンパイル エラー</span>
<span class="reserved">static</span> <span class="reserved">void</span> G(<span class="reserved">int</span> x) { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="error">G</span>(<span class="reserved">int</span> y) { }
</code></pre>

また、戻り値だけ違うオーバーロードも作れません。

<pre class="source" title="戻り値違いのオーバーロードも無理">
<code><span class="comment">// H は、引数が一致していて、戻り値だけ違う。これもコンパイル エラー</span>
<span class="reserved">static</span> <span class="reserved">int</span> H() =&gt; 1;
<span class="reserved">static</span> <span class="reserved">string</span> <span class="error">H</span>() =&gt; <span class="string">""</span>;
</code></pre>

あと、「C# としては区別しているように見えるけども、内部的には同じ扱いになっていて区別できないのでオーバーロードにも使えない」という型がいくつかあります。

- [`dynamic`](../dynamic/sp4_dynamic.md)は[内部的には`object`扱い](../dynamic/sp4_callsite.md)
- [`in`、`ref`、`out`は内部的には同じ扱い](../resource/sp_ref.md#ref-in-out)

例えば以下のようなオーバーロードは作れません。

<pre class="source" title="dynamic の扱い">
<code><span class="reserved">void</span> D(<span class="reserved">object</span> x) { }
<span class="reserved">void</span> D(<span class="reserved">dynamic</span> x) { }
</code></pre>

<pre class="source" title="in, ref, out の扱い">
<code><span class="reserved">void</span> F(<span class="reserved">ref</span> <span class="reserved">int</span> x) { }
<span class="reserved">void</span> <span class="error">F</span>(<span class="reserved">in</span> <span class="reserved">int</span> x) { }

<span class="reserved">void</span> G(<span class="reserved">ref</span> <span class="reserved">int</span> x) { }
<span class="reserved">void</span> <span class="error">G</span>(<span class="reserved">out</span> <span class="reserved">int</span> x) =&gt; x = 0;

<span class="reserved">void</span> H(<span class="reserved">in</span> <span class="reserved">int</span> x) { }
<span class="reserved">void</span> <span class="error">H</span>(<span class="reserved">out</span> <span class="reserved">int</span> x) =&gt; x = 0;
</code></pre>

### <a id="sec-generated-title-26"></a> <a id="signature"></a>シグネチャ

オーバーロードがある以上、関数は、複数ある関数のうちのどれを呼ぶか、名前だけ特定することができません。
特定には、関数名と、引数の型が必要になります。
こういう、関数の呼び分けに必要な情報のことを<strong id="key-signature" class="keyword">シグネチャ</strong>(signature: 署名、サイン)と呼びます。

前述の通り、C#の場合は引数名や戻り値の型はオーバロード解決には使えないので、これらはシグネチャには含まれません。
例えば、`int F(int x, int y)`というようなメソッドがあった場合、このメソッド`F`のシグネチャは`F(int, int)`です(引数名と戻り値の型が消える)。

参考までに他の言語の例を上げておくと、
C++やJavaはC#と同様です。
C言語やGoは、関数のオーバーロード自体認めていない(呼び分けには名前自体を変えないといけない)ので、関数名 = シグネチャです。
Swiftでは、引数名違いや戻り値の型違いのオーバーロードができるので、`func x(x: Int) -> Int`というような関数があった場合、`x(x: Int) -> Int`全体がシグネチャです。

### <a id="sec-generated-title-27"></a> <a id="method-group"></a>メソッド グループ

関数の中でも[メソッド](#method)の場合は、[デリゲート](../functional/sp_delegate.md)への代入の際に `Action a = M;` みたいな引数なしな書き方ができます。

この引数なしな書き方では、`M`だけではなくてその周りまで見ないと「どの`M`か」が確定しません。
すなわち、「いくつかあるメソッド`M`のうちのいずれか」という状態です。
この状態の`M`を<strong id="key-method-group" class="keyword">メソッド グループ</strong>(method group)と呼びます。

<pre class="source" title="メソッド グループの例">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// M(int) という「メソッド」</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x) { }

    <span class="comment">// M(string) という「メソッド」</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">string</span> x) { }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 右辺だけ見ると M は「M(int) か M(string) のどちらか」という状態</span>
        <span class="comment">// この状態の M をメソッド グループという</span>
        <span class="comment">// 左辺の Action&lt;int&gt; を見て初めてどちらなのかが確定する</span>
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; a = M;
    }
}
</code></pre>

## <a id="sec-generated-title-28"></a> <a id="sec-expression-bodied"></a>expression-bodied な関数

<h5 class="version version6">Ver. 6</h5>
C# 6 では、関数本体の部分が1つの式だけからなる場合、 `=>` 記号を使って以下のような書き方をすることができるようになりました。
これを、<strong id="expression-bodied" class="keyword">expression-bodied (本体が式の)関数</strong>(expression-bodied function)と呼びます。

例えば、先ほど例に出した2つの関数、Random と Norm は以下のように書くこともできます。

<pre class="source" title="" lang="">
<code><span class="reserved">static ulong</span> Random() =&gt; <span class="reserved">unchecked</span>(seed = seed * 1566083941UL +  1 );

<span class="reserved">static double</span> Norm(<span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z) =&gt; x * x + y * y + z * z;
</code></pre>

C# 6時点では、メソッド、演算子、プロパティとインデクサー(get-only)を `=>` 記号で書けます。

<pre class="source" title="C# 6 時点で =&gt; を使って書ける関数メンバー">
<code><span class="reserved">class</span> <span class="type">Csharp6</span>
{
    <span class="comment">// メソッド</span>
    <span class="reserved">int</span> Method(<span class="reserved">int</span> x) =&gt; x * x;

    <span class="comment">// 演算子</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Csharp6</span> <span class="reserved">operator</span> +(<span class="type">Csharp6</span> x) =&gt; x;

    <span class="comment">// プロパティ(get-only)</span>
    <span class="reserved">int</span> X =&gt; 0;

    <span class="comment">// インデクサー(get-only)</span>
    <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; index;
}
</code></pre>

また、C# 7では、コンストラクター、ファイナライザー、プロパティとインデクサー(get/set それぞれ)、イベント(add/removeそれぞれ)も `=>` 記号で書けるようになりました。

<pre class="source" title="C# 7 で追加された =&gt; を使って書ける関数メンバー">
<code><span class="reserved">class</span> <span class="type">Csharp7</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> x;

    <span class="comment">// コンストラクター</span>
    Csharp7() =&gt; x++;

    <span class="comment">// ファイナライザー</span>
    ~Csharp7() =&gt; x--;

    <span class="comment">// プロパティ(get/set)</span>
    <span class="reserved">int</span> X
    {
        <span class="reserved">get</span> =&gt; x++;
        <span class="reserved">set</span> =&gt; x--;
    }

    <span class="comment">// インデクサー(get/set)</span>
    <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index]
    {
        <span class="reserved">get</span> =&gt; x += index;
        <span class="reserved">set</span> =&gt; x -= index;
    }

    <span class="comment">// イベント(add/remove)</span>
    <span class="reserved">event</span> <span class="type">Action</span> E
    {
        <span class="reserved">add</span> =&gt; x++;
        <span class="reserved">remove</span> =&gt; x--;
    }
}
</code></pre>

## <a id="sec-generated-title-29"></a> <a id="sec-local"></a>ローカル関数

<h5 class="version version7">Ver. 7</h5>

C# 7では、関数の中で別の関数を定義して使うことができます。
関数の中でしか使えないため、<strong id="key-local">ローカル関数</strong>(local function: その場所でしか使えない関数)と呼びます。

例えば以下のように書けます。

<pre class="source" title="ローカル関数の例">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// Main 関数の中で、ローカル関数 f を定義</span>
        <em><span class="reserved">int</span> f(<span class="reserved">int</span> n) =&gt; n &gt;= 1 ? n * f(n - 1) : 1;</em>

        <span class="type">Console</span>.WriteLine(f(10));
    }
}
</code></pre>

詳細は「[ローカル関数と匿名関数](../functional/fun_localfunctions.md)」で説明しています。

## <a id="sec-generated-title-30"></a> <a id="anonymous"></a>匿名関数

<h5 class="version version2">Ver. 2.0</h5>

もう1つ、関数の中に関数を書く方法として、<strong id="key-anonymous" class="keyword">匿名関数</strong>(anonymous function)というものがあります。

以下のような書き方をします。

<pre class="source" title="匿名関数の例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> input = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
        <span class="reserved">var</span> output = input
            .Where(<em>n =&gt; n &gt; 3</em>)
            .Select(<em>n =&gt; n * n</em>);

        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> output)
        {
            <span class="type">Console</span>.WriteLine(x);
        }
    }
}
</code></pre>

強調表示している部分が匿名関数です。
通常の関数もローカル関数も名前を持っていますが、匿名関数は、その名前通り、無名です。

匿名関数は、ローカル関数と比べて制限も多いですが、その代わり、どこにでも書けるという利点があります。
(正確には、[式](../start/st_variable.md#expression)が書ける場所ならどこにでも書けます。
一方、ローカル関数は[ステートメント](../start/st_variable.md#statement)です。)

詳細は「[ローカル関数と匿名関数](../functional/fun_localfunctions.md)」で説明しています。
## <a id="exercise"></a>演習問題

### <a id="exercise-func0"></a>問題 1


int 型の配列に格納されている値の最大値、最小値および平均値を求める関数をそれぞれ作成せよ。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 配列中の値の最大値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;returns&gt;最大値&lt;/returns&gt;</span>
<span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)

<span class="comment">/// &lt;summary&gt;
/// 配列中の値の最小値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;returns&gt;最小値&lt;/returns&gt;</span>
<span class="reserved">static int</span> Min(<span class="reserved">int</span>[] a)

<span class="comment">/// &lt;summary&gt;
/// 配列中の値の平均値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;returns&gt;平均値&lt;/returns&gt;</span>
<span class="reserved">static double</span> Average(<span class="reserved">int</span>[] a)
</code></pre>



#### 解答例 1


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// 配列長の入力</span>
    Console.Write(<span class="literal">"配列の長さ: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="comment">// 配列の値の入力</span>
    <span class="reserved">int</span>[] a = <span class="reserved">new int</span>[n];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; n; ++i)
    {
      Console.Write(<span class="literal">"{0}: "</span>, i);
      a[i] = <span class="reserved">int</span>.Parse(Console.ReadLine());
    }

    Console.Write(
<span class="literal">@"
最大値: {0}
最小値: {1}
平均値: {2}
"</span>
    , Max(a), Min(a), Average(a));
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最大値&lt;/returns&gt;</span>
  <span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> max = <span class="reserved">int</span>.MinValue;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
    }

    <span class="reserved">return</span> max;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最小値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最小値&lt;/returns&gt;</span>
  <span class="reserved">static int</span> Min(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> min = <span class="reserved">int</span>.MaxValue;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (min &gt; a[i]) min = a[i];
    }

    <span class="reserved">return</span> min;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;平均値&lt;/returns&gt;</span>
  <span class="reserved">static double</span> Average(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">double</span> average = 0;

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      average += a[i];
    }

    <span class="reserved">return</span> average / a.Length;
  }
}
</code></pre>



### <a id="exercise-func1"></a>問題 2


double 型の値 x の整数冪を求める関数 Power を作成せよ。

<pre class="source" title="Power の仕様" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// x の整数冪を求める。
/// &lt;/summary&gt;
/// &lt;param name="x"&gt;仮数 x&lt;/param&gt;
/// &lt;param name="n"&gt;指数 n&lt;/param&gt;
/// &lt;returns&gt;x の n 乗&lt;/returns&gt;</span>
<span class="reserved">static double</span> Power(
  <span class="reserved">double</span> x,
  <span class="reserved">int</span> n)
</code></pre>



#### 解答例 1


<pre class="source" title="Power の実装とテスト" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">const double</span> x = 3;
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, 4));
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, -1));
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, -2));
    Console.Write(<span class="literal">"{0}\n"</span>, Power(x, 0));
  }

  <span class="comment">/// &lt;summary&gt;
  /// x の整数冪を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;仮数 x&lt;/param&gt;
  /// &lt;param name="n"&gt;指数 n&lt;/param&gt;
  /// &lt;returns&gt;x の n 乗&lt;/returns&gt;</span>
  <span class="reserved">static double</span> Power(
    <span class="reserved">double</span> x,
    <span class="reserved">int</span> n)
  {
    <span class="reserved">if</span> (n == 0)
      <span class="reserved">return</span> 1;

    <span class="reserved">if</span> (n &lt; 0)
    {
      x = 1.0 / x;
      n = -n;
    }

    <span class="reserved">double</span> y = x;
    <span class="reserved">while</span> (--n &gt; 0)
    {
      y *= x;
    }

    <span class="reserved">return</span> y;
  }
}
</code></pre>



### <a id="exercise-func2"></a>問題 3


配列に格納されている値の最大値を求める関数を、
<code>int[]</code> に対するものと
<code>double[]</code> に対するものの2種類作成せよ。


#### 解答例 1


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">int</span>[]    ai = <span class="reserved">new int</span>[]    { 1, 3, 9, 2, 5, 6, 4 };
    <span class="reserved">double</span>[] ad = <span class="reserved">new double</span>[] { 1, 3, 9, 2, 5, 6, 4 };

    Console.Write(<span class="literal">"{0}, {1}\n"</span>, Max(ai), Max(ad));
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最大値&lt;/returns&gt;</span>
  <span class="reserved">static int</span> Max(<span class="reserved">int</span>[] a)
  {
    <span class="reserved">int</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
    }
    <span class="reserved">return</span> max;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 配列中の値の最大値を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
  /// &lt;returns&gt;最大値&lt;/returns&gt;</span>
  <span class="reserved">static double</span> Max(<span class="reserved">double</span>[] a)
  {
    <span class="reserved">double</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">if</span> (max &lt; a[i]) max = a[i];
    }
    <span class="reserved">return</span> max;
  }
}
</code></pre>


見ての通り、型が変わっただけで、処理自体は全く同じものになっています。
このように、型と無関係に同じ処理で実現できるものは、
「[ジェネリック](../oop/sp2_generics.md#generics)」を使うことで実装の手間を軽減できます。
