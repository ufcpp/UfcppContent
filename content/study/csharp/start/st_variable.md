---
title: "変数と式"
source_url: "https://ufcpp.net/study/csharp/start/st_variable/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2008-01-05T00:00:00"
tags: []
umbraco_id: 1198
parent_id: 1190
sort_order: 4
aliases:
  - "/csharp/st_variable"
  - "/csharp/st_variable.html"
  - "/csharp/start/st_variable/"
  - "/study/csharp/st_variable"
  - "/study/csharp/st_variable.html"
---

# 変数と式

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

数学では、“整数 <span class="math">n</span>”とか、“実数 <span class="math">x</span>”などといった変数を用意し、
“<span class="math">
        ax<sup><span class="normal">2</span></sup><span class="normal">+</span> bx <span class="normal">+</span> c
      </span>”といった式を用いて計算を行います。
それと同じようにプログラミングでも、まず、変数を用意し、式を並べて計算を行っていきます。


##### <a id="sec-generated-title-2"></a>ポイント

* <code>int x</code>… int が型で、x が変数名。

* <code>int x = 1 + 2;</code>… 式も数学っぽく書ける。

* リテラル: 文字通りの定数。0 とか。



## <a id="sec-generated-title-3"></a> <a id="variable"></a>変数

数学では“整数”とか“実数”といったさまざまな種類の「型」が存在します。
「<span style="font-style:italic;">n</span> は整数に使うことが多い」といったような暗黙の了解も存在しますが、基本的には、「整数 <span style="font-style:italic;">n</span>」といったように、「型」を明示します。

数学の場合と同様に、プログラミング言語にも変数の「<strong id="type" class="keyword">型</strong>（type）」というものが存在します。
特に、<em>C# では各変数は必ず「型」を明示的に指定して宣言してやる必要があります</em>。
(この書き方を<strong id="var-decl" class="keyword">変数宣言</strong>と言います。)
。
以下に例を挙げます。

<pre class="source" title="宣言文の例" lang="">
<code><span class="reserved">bool</span>   b; <span class="comment">// 論理値型の変数 b</span>
<span class="reserved">int</span>    n; <span class="comment">// 整数型の変数 n</span>
<span class="reserved">double</span> x; <span class="comment">// 実数型の変数 x</span>
<span class="reserved">char</span>   c; <span class="comment">// 文字型の変数 c</span>
<span class="reserved">string</span> s; <span class="comment">// 文字列型の変数 s</span>
</code></pre>

左側の <code>bool</code> や <code>int</code> などが変数の「型」で、
その右側の <code>b</code> や <code>n</code> などが変数名になります。
<code>int</code> や <code>double</code>、<code>string</code>などは、C# にもともと用意された変数の「型」(これを<em>組込み型 (embedded type)</em>という)です。

C#の組込み型の型名については「[組込み型](st_embeddedtype.md)」で説明します。

ちなみに、C# には、変数から値を読みだす前に、必ず何らかの値を代入する必要があります。
詳しくは「[[雑記] 明確な代入ルール](definiteassignment.md)」で説明します。

<h5 class="version version7">Ver. 7.0</h5>

C# 6.0 までは、変数宣言は単独で書く必要がありましたが、
C# 7.0 以降、[式](#expression)の途中で変数宣言をできる構文がいくつか追加されています。
詳しくは「[特殊な変数宣言](../datatype/declarationexpressions.md)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="literal"></a>リテラル

<strong id="literal" class="keyword">リテラル</strong>(literal: “文字通りの定数”という意味。直定数などと訳されることもある。)とは、要するに、「10」や「4.56」というように、直接ソースファイル中に値が書かれた定数のことです。

<pre class="source" title="リテラルの例" lang="">
<code><span class="reserved">bool</span>   b = <span class="reserved">true</span>;    <span class="comment">// 論理値リテラル</span>
<span class="reserved">int</span>    n = 26983;   <span class="comment">// 整数リテラル</span>
<span class="reserved">double</span> x = 10.362;  <span class="comment">// 実数リテラル</span>
<span class="reserved">char</span>   c = <span class="literal">'a'</span>;     <span class="comment">// 文字リテラル</span>
<span class="reserved">string</span> s = <span class="literal">"文字列"</span>; <span class="comment">// 文字列リテラル</span>
</code></pre>


ここで使われている <code>=</code> は代入を意味し、左辺の変数に右辺の値を代入するものです。
詳しくは次節の式と文で説明します。

リテラルの書き方の詳細は「[組込み型](st_embeddedtype.md)」で説明します。

ちなみに、定数（constant）というとリテラルとは別の意味で使われます
（参考: 「[定数](sp_const.md)」）。

## <a id="sec-generated-title-5"></a> <a id="updates"></a>いろいろな変数宣言の仕方

C# のアップデートで、いろいろな変数宣言の仕方が追加されています。

### <a id="sec-generated-title-6"></a> <a id="infer"></a>型推論

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 から、<strong id="var" class="keyword">var</strong> キーワードを使って、型を明示せずに変数を定義できるようになりました。

<pre class="source" title="var キーワードによる暗黙的型付け" lang="">
<code><span class="reserved">var</span> b = <span class="reserved">true</span>;    <span class="comment">// 論理値</span>
<span class="reserved">var</span> n = 26983;   <span class="comment">// 整数</span>
<span class="reserved">var</span> x = 10.362;  <span class="comment">// 実数</span>
<span class="reserved">var</span> c = <span class="literal">'a'</span>;     <span class="comment">// 文字</span>
<span class="reserved">var</span> s = <span class="literal">"文字列"</span>; <span class="comment">// 文字列</span>
</code></pre>


このとき、変数の型は右辺の値から推論されます。
この例の場合、b は論理値ですし、n は整数になります。

var キーワードを使った変数の定義は、あくまで型推論です。
右辺値がない（推論の手がかりがない）場合には var を使うことは出来ません。
また、「どんな型でも代入できる変数」を作れるわけではありません。

<pre class="source" title="var はあくまで型推論" lang="">
<code><span class="reserved">var</span> x; <span class="comment">// これはコンパイルエラー</span>
x = 1;
</code></pre>


また、var で宣言した変数の型が途中で変わることもりません。

<pre class="source" title="途中で型は変えられない" lang="">
<code><span class="reserved">var</span> n = 0; <span class="comment">// この時点で n は int になるので、</span>
n = <span class="literal">""</span>;    <span class="comment">// これはコンパイルエラー</span>
</code></pre>


ちなみに、var の利用には賛否両論あったりします。
参考：「[[雑記] 型推論の是非](sp3_var.md)」。


### <a id="sec-generated-title-7"></a> <a id="dynamic"></a>dynamic

<h5 class="version version4">Ver. 4.0</h5>

C# 4.0 では、動的な型の扱いもできるようになりました。
（詳しくは「[dynamic](../dynamic/sp4_dynamic.md)」参照。）

ただし、動的な型は C# において必要とされる場面はあまりなく、dynamic の利用場面はそれほど多くありません。
以下のような場合に利用することになるでしょう。

* .NET 以前の古いコード（COM など）との連携（参考:「[COM](../interop/sp_pinvoke.md#COM)」

* 動的言語との連携


この例のように、dynamic は主に「連携」のための機能です。

### <a id="sec-generated-title-8"></a> <a id="tuple"></a>タプル

<h5 class="version version7">Ver. 7.0</h5>

C# 7.0 で[タプル](../datatype/tuples.md)という機能が追加されて、
複数の変数をまとめたり、同時に宣言したりできるようになりました。

<pre class="source" title="タプル">
<code>var (x, y) = (1, 2);
var (sum, dif) = (x + y, x - y);
</code></pre>

`()`の中に複数の値を並べている部分がタプルです。
この例のように、`x`, `y` などの変数を同時に宣言したり、
`x + y`, `x - y` の計算を一緒に行ったりできます。

## <a id="sec-generated-title-9"></a> <a id="identifier"></a>識別子名

変数など、プログラマが自由に名前を付けることの出来るものを<strong id="identifier" class="keyword">識別子</strong>(identifier)と呼びます。

詳細は「[[雑記] 識別子名に使える文字](misc_identifier.md)」にて説明しますが、
識別子の名前に使える文字には制限があります。
細かいことは抜きにして実用の範囲で考えると、
<em>
        先頭には <code>\_</code> もしくはアルファベット(ソースファイルを Unicode で保存すれば仮名漢字も)、
        先頭以外にはそれに加えて数字を使える
      </em>と覚えておけば問題ありません。
（ちなみに、Unicode で保存しても全角の記号文字（＊とか＄とか）は変数名には使えません。）

ただし、int や if というような、C# のキーワードになっているものは、
そのままでは識別子として利用できません。


##### <a id="sec-generated-title-10"></a>逐語的識別子

先頭に @ を付けることで、
キーワードも識別子として利用可能になります。
たとえば、<code>@this</code> や <code>@for</code> と書くことで、それぞれ this、for という名前の変数を作ることができます。
この <code>@</code> 付きの識別子を逐語的識別子（verbatim identifier）と呼びます。

C# ではキーワードになっていても、他のプログラミング言語ではキーワードでない場合があります。
逐語的識別子は、このような、他のプログラミング言語との連携を想定したものです。
例えば、this という単語が識別子でない言語の場合、this という名前の変数を作ることができます。
この変数を C# から参照したければ、@this というように書きます（C# では、this はキーワード）。

ちなみに、この先頭の @ 記号は、識別子名の一部としては認識されません。
例えば、x と @x は全く同じ識別子名として認識されます。


## <a id="sec-generated-title-11"></a> <a id="expression"></a>式と演算子

変数の用意が出来たら式を立てて計算を行っていきます。
それでは具体的な例を見ながら説明して行くことにしましょう。

<pre class="source" title="式と代入の例" lang="">
<code><span class="reserved">int</span> a = 3, b = 5, c, d; <span class="comment">// 整数型の変数を4つ用意</span>
c = (a + b) / 2;        <span class="comment">// c に a と b の平均値を代入</span>
d = a * b;              <span class="comment">// d に a と b の積を代入</span>
</code></pre>


この、<code>c = (a + b) / 2</code> とか <code>d = a * b</code> という部分が<strong id="expression" class="keyword">式</strong>（expression）です。
式は、変数、リテラル、演算子などで構成されます。

<strong id="operator" class="keyword">演算子</strong>（operator）とは、数学でよく使う ＋－×÷ などのことです。
(ただし、C# では、<em>
        掛け算のために <code>*</code> を、割り算のために <code>/</code> を使います
      </em>。また、代入のための <code>=</code> も演算子の一種です。)
C# では、数学などで使う記法とほぼ同じ書き方で四則演算などが行えます。
加減算よりも乗除算のほうが計算の優先順位が高いのも数学と同じです。

<pre class="source" title="演算子の優先順位の例" lang="">
<code><span class="reserved">int</span> a = 5 * 2 + 3 * 4; <span class="comment">// 掛け算が先。a の値は (5×2) ＋ (3×4) で 22 になる。</span>
</code></pre>


C# の演算子の一覧と優先順位は「[組込み演算子](st_operator.md)」で説明します。


## <a id="sec-generated-title-12"></a> <a id="statement"></a>文

<strong id="statement" class="keyword">文</strong>(statement)とはプログラムの処理の単位のことです。

<pre class="source" title="文" lang="">
<code><span class="reserved">int</span> c, d;         <span class="comment">// 宣言文: 変数を用意。</span>
<span class="reserved">int</span> a = 3, b = 5; <span class="comment">// 宣言文: 変数の宣言と同時に初期化もできる。</span>
c = (a + b) / 2;  <span class="comment">// 代入文: c に a と b の平均値を代入</span>
d = a * b;        <span class="comment">// 代入文: d に a と b の積を代入</span>
</code></pre>


例えば上の例では、4つの文があります。
C# では、文と文は <code>;</code> (セミコロン)で区切られます。
最初の2つの文は変数の準備と初期化を、残り2つの文は計算を行ってその結果の代入を行います。

また、複数の文を <code>{}</code> で括ることで一塊の文とみなすことができます。

<pre class="source" title="複文" lang="">
<code>{
  c = (a + b) / 2;
  d = a * b;
} <span class="comment">// 2つの文を1つのグループに</span>
</code></pre>


このようにグループ化された文を<em>複文</em>または<strong id="block" class="keyword">ブロック</strong>(block)といいます。


##### <a id="sec-generated-title-13"></a>サンプル

<pre class="source" title="変数と式のサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> StatementSample
{
   <span class="reserved">static void</span> Main()
   {
      <span class="reserved">double</span> x, y, z;  <span class="comment">// 変数を宣言。

      // xにユーザーの入力した値を代入。</span>
      Console.Write(<span class="literal">"input x : "</span>);
      x = <span class="reserved">double</span>.Parse(Console.ReadLine());

      <span class="comment">// yにユーザーの入力した値を代入。</span>
      Console.Write(<span class="literal">"input y : "</span>);
      y = <span class="reserved">double</span>.Parse(Console.ReadLine());

      <span class="comment">// 入力された値を元に計算</span>
      z = x * x + y * y; <span class="comment">// z に x と y の二乗和を代入</span>
      x /=  z;           <span class="comment">// x =  x / z; と同じ。</span>
      y /= -z;           <span class="comment">// y = -y / z; と同じ。

      // 計算結果を出力</span>
      Console.Write(<span class="literal">"({0}, {1})"</span>, x, y);
   }
}
</code></pre>


<pre class="console" title="">
input x : <span class="input">3</span>
input y : <span class="input">4</span>
(0.12, -0.16)
</pre>



### <a id="sec-generated-title-14"></a> <a id="sentencse"></a>余談: 平叙文

statement という単語の訳語が「文」なのは少し不正確だったりします。
（なので、カタカナ語でステートメントと表現することも多いです。）
一般に、「文」というと、英単語としては sentense になりますが、sentense に対する分類として、以下のようなものがあります。

* 平叙文（statement）

* 疑問文（question）

* 命令文（command）

* 感嘆文（exclamation）


プログラミング言語における statement はこの意味の statement で、平叙文、すなわち、「普通に何かを述べる文」という意味です。

「何かを表明する」という意味合いが強くて、
例えば、政策や企業理念などを端的に表す言葉を statement と呼んだりします。

## <a id="sec-generated-title-15"></a> <a id="var-expression"></a>式の中での変数宣言

<h5 class="version version7">Ver. 7</h5>

変数の宣言は、前述の通り、以下のような書き方が必要でした。
これらは宣言文(declaration statement)と言って、ステートメントの一種です。

<pre class="source" title="宣言文の例" lang="">
<code><span class="reserved">bool</span>   b; <span class="comment">// 論理値型の変数 b</span>
<span class="reserved">int</span>    n; <span class="comment">// 整数型の変数 n</span>
<span class="reserved">double</span> x; <span class="comment">// 実数型の変数 x</span>
<span class="reserved">char</span>   c; <span class="comment">// 文字型の変数 c</span>
<span class="reserved">string</span> s; <span class="comment">// 文字列型の変数 s</span>
</code></pre>

ステートメントは、式(`x + y`みたいなやつ)と比べると、書ける場所が限られていて、使い勝手が悪いです。

これに対して、C# 7で、2つ、式の途中で変数宣言できる構文が追加されました。

- [is式](../datatype/typeswitch.md#is)
- [出力変数宣言](../resource/sp_ref.md#out-var)

詳細はそれぞれのリンク先で説明しています。
また、今後(C# 7よりもさらに先)、変数宣言できる場所がもっと増える可能性があります。
