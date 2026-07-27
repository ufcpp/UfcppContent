---
title: "行列と線形写像"
source_url: "https://ufcpp.net/study/math/linear/matrix/"
content_type: "Article"
published_at: "2015-05-06T14:16:30"
updated_at: "2015-05-18T10:31:57"
tags: []
umbraco_id: 1460
parent_id: 1458
sort_order: 1
aliases:
  - "/study/linear/matrix.html"
---

# 行列と線形写像

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

行列とは何なのかといわれると、いろいろな表現の仕方があるのですが、
大まかに言うと以下の2つになります。

* 行列 ＝ 1次方程式を表現するための便法、数の一般化

* 行列 ＝ 線形写像



## <a id="sec-generated-title-2"></a> <a id="matrix"></a>行列

まず、行列というのは1次方程式を簡潔に表現するための便法だと考えることができます。
例えば、
<div class="math">
a<sub>11</sub> x<sub>1</sub>
＋
a<sub>12</sub> x<sub>2</sub>
＝
b<sub>1</sub></div><div class="math">
a<sub>21</sub> x<sub>1</sub>
＋
a<sub>22</sub> x<sub>2</sub>
＝
b<sub>2</sub></div>
というような連立1次方程式を、
<div class="math">
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>11</sub></td><td>a<sub>12</sub></td></tr><tr><td>a<sub>21</sub></td><td>a<sub>22</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>x<sub>1</sub></td></tr><tr><td>x<sub>2</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
＝
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>b<sub>1</sub></td></tr><tr><td>b<sub>2</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
と表す。
あるいは、
<div class="math">
A ＝
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>11</sub></td><td>a<sub>12</sub></td></tr><tr><td>a<sub>21</sub></td><td>a<sub>22</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
,
x ＝
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>x<sub>1</sub></td></tr><tr><td>x<sub>2</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
,
b ＝
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>b<sub>1</sub></td></tr><tr><td>b<sub>2</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
と置いて、
<div class="math">
A x ＝ b
</div>
と表すことができます。
 
こうすることで、
1変数の場合と同じ記法で他変数の1次方程式を記述できます。
1変数のときに、
<span class="math">a x ＝ y, b y ＝ c</span> ならば
<span class="math">a b x ＝ c</span> で、
<span class="math">x ＝ b<sup>－1</sup> a<sup>－1</sup> c</span> と表せるように、
他変数の場合でも行列を用いて、
<span class="math">A x ＝ y, B y ＝ c</span> を <span class="math">x</span> について解いたものを
<span class="math">x ＝ B<sup>－1</sup> A<sup>－1</sup> c</span> と表せます。
 
行列を用いて1変数のときと同じ記法で他変数の場合を表現できるのは、
1次方程式の解だけじゃなくて他にもいろいろなものがあります。
以下に、いくつか例を挙げます。
<h4>線形漸化式</h4>
1変数のとき、
<div class="math">
x<sub>n ＋ 1</sub>
＝
a
x<sub>n</sub>
　⇒　
x<sub>n</sub>
＝
a<sup>n</sup>
x<sub>0</sub></div>
多変数でも、
<div class="math">
x<sub>n ＋ 1</sub>
＝
A
x<sub>n</sub>
　⇒　
x<sub>n</sub>
＝
A<sup>n</sup>
x<sub>0</sub></div><h4>線形微分方程式</h4>
1変数のとき、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
a
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
　⇒　
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
e<sup>at</sup>
x<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></div>
多変数でも、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
A
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
　⇒　
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>At<span class="paren" style="font-size:em;">)</span>
x<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></div>
ただし、行列 <span class="math">A</span> の指数関数
<span class="math"><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>A<span class="paren" style="font-size:em;">)</span></span> は冪級数で定義。
 
これらの例に示すように、
行列というのは、数というものを多変数・高次元の場合に一般化したものだと考えることができます。


## <a id="sec-generated-title-3"></a> <a id="linear_map"></a>線形写像

行列の持つもう1つの側面として、線形写像という考え方があります。
 
ベクトルに対して、線形性という性質にのみ着目して、線形空間として抽象化したように、
行列も線形写像として抽象化されます。
 
前節の説明で、行列というのは、以下のような1次方程式を表現するための便法だと説明しました。
<div class="math">
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>11</sub></td><td>a<sub>12</sub></td></tr><tr><td>a<sub>21</sub></td><td>a<sub>22</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>x<sub>1</sub></td></tr><tr><td>x<sub>2</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
＝
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>y<sub>1</sub></td></tr><tr><td>y<sub>2</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
ベクトル・行列を使って表すと、
<div class="math">
Ax ＝ y
</div>
となりますが、この式は、
「ベクトル <span class="math">x</span> に対して行列を掛けると、別のベクトル <span class="math">y</span> が得られる」
と考えることもできます。
すなわち、行列はベクトル → ベクトルへの変換・写像だとみなせます。
 
これに対して、
一般の線形空間 → 線形空間の写像で以下の性質を満たすものを線形写像といいます。

<span class="math">U, V</span> は体 <span class="math">K</span> 上の線形空間で、
<span class="math">f</span> は <span class="math">U</span> → <span class="math">V</span> の写像とします。
<span class="math">x, y ∈ U, 　a, b ∈ K</span> のとき、
<div class="math">
f<span class="paren" style="font-size:em;">(</span>ax ＋ by<span class="paren" style="font-size:em;">)</span>
＝
a f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＋
b f<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span></div>
となるとき、<span class="math">f</span> を <span class="math">U</span> → <span class="math">V</span> の線形写像といいます。

「[ベクトルと線形空間](l_vector.md)」の「[基底](l_vector.md#base)」で触れましたが、
任意の線形空間は、適当な座標系を与えることで、数ベクトルとして表現可能です。
なので、任意の線形写像は、定義域・値域の両方に適当な座標系を与えることで、
（有限次元ならば）行列として表現可能です。
 
例えば、文字 <span class="math">t</span> に関する3次多項式は線形空間、
それに対する微分演算は線形写像になりますが、
3次多項式の基底として、<span class="math"><span class="paren" style="font-size:em;">{</span>1, t, t<sup>2</sup>, t<sup>3</sup><span class="paren" style="font-size:em;">}</span></span> をとるなら、
微分演算 <span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table></span> は
<div class="math">
      <span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>0</td><td>0</td></tr><tr><td>0</td><td>0</td><td>2</td><td>0</td></tr><tr><td>0</td><td>0</td><td>0</td><td>3</td></tr><tr><td>0</td><td>0</td><td>0</td><td>0</td></tr></table><span class="paren" style="font-size:5em;">]</span>
    </div>
という行列で表されます。
ただし、行列での表し方は、座標系の取り方に依存しています。
今の例において、基底の取り方をチェビシェフ多項式 <span class="math"><span class="paren" style="font-size:em;">{</span>1, t, 2t<sup>2</sup> － 1, 4t<sup>3</sup> － 3t<span class="paren" style="font-size:em;">}</span></span> に変えると、
<div class="math">
      <span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>0</td><td>3</td></tr><tr><td>0</td><td>0</td><td>4</td><td>0</td></tr><tr><td>0</td><td>0</td><td>0</td><td>6</td></tr><tr><td>0</td><td>0</td><td>0</td><td>0</td></tr></table><span class="paren" style="font-size:5em;">]</span>
    </div>
となりますし、
ルジャンドル多項式 <span class="math"><span class="paren" style="font-size:em;">{</span>1, t, <table class="frac" summary="fraction"><tr><td class="num">3</td></tr><tr><td>2</td></tr></table>t<sup>2</sup> － <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>2</td></tr></table>, <table class="frac" summary="fraction"><tr><td class="num">5</td></tr><tr><td>2</td></tr></table>t<sup>3</sup> － <table class="frac" summary="fraction"><tr><td class="num">3</td></tr><tr><td>2</td></tr></table>t<span class="paren" style="font-size:em;">}</span></span> に変えると、
<div class="math">
      <span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>0</td><td>1</td></tr><tr><td>0</td><td>0</td><td>3</td><td>0</td></tr><tr><td>0</td><td>0</td><td>0</td><td>5</td></tr><tr><td>0</td><td>0</td><td>0</td><td>0</td></tr></table><span class="paren" style="font-size:5em;">]</span>
    </div>
となります。
 
ということで、線形写像 ≒ 行列 と思ってもいいわけですが、
抽象的に定義しておく方が扱える対象が広がって都合がいいので（無限次元も扱えるし）、
抽象化します。
もし必要なら、必要が生じたときに始めて、
その時々に最も適切な座標系を選んで、行列表現を得ます。


## <a id="sec-generated-title-4"></a> <a id="coordinate"></a>座標変換

同じ線形空間上の同じ元でも、座標系の取り方によって異なる数ベクトルで表される。
ある座標系から別の座標系に変換することを座標変換と言う。
座標変換も線形写像になる ＝ 行列で表される。
 
例として、前節で例示した <span class="math">t</span> に関する3次多項式の座標変換を考えてみましょう。
多項式 <span class="math">a ＋ b t ＋ c t<sup>2</sup> ＋ d t<sup>3</sup></span> は、
座標系 <span class="math"><span class="paren" style="font-size:em;">{</span>1, t, t<sup>2</sup>, t<sup>3</sup><span class="paren" style="font-size:em;">}</span></span> を用いるなら、
<span class="math"><span class="paren" style="font-size:em;">(</span>a, b, c, d<span class="paren" style="font-size:em;">)</span></span> となります。
一方で、
チェビシェフ多項式を用いた座標系
<span class="math"><span class="paren" style="font-size:em;">{</span>1, t, 2t<sup>2</sup> － 1, 4t<sup>3</sup> － 3t<span class="paren" style="font-size:em;">}</span></span>
を用いるなら、
<span class="math"><span class="paren" style="font-size:em;">(</span>a ＋ 1/2 c, b ＋ 3/4 d, 1/2 c, 1/4 d<span class="paren" style="font-size:em;">)</span></span>
となります。
 
したがって、
<span class="math"><span class="paren" style="font-size:em;">{</span>1, t, t<sup>2</sup>, t<sup>3</sup><span class="paren" style="font-size:em;">}</span></span>
から
<span class="math"><span class="paren" style="font-size:em;">{</span>1, t, 2t<sup>2</sup> － 1, 4t<sup>3</sup> － 3t<span class="paren" style="font-size:em;">}</span></span>
への座標変換は以下のような行列で表現できます。
<div class="math">
      <span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td>1</td><td>0</td><td>1/2</td><td>0</td></tr><tr><td>0</td><td>1</td><td>0</td><td>3/4</td></tr><tr><td>0</td><td>0</td><td>1/2</td><td>0</td></tr><tr><td>0</td><td>0</td><td>0</td><td>1/4</td></tr></table><span class="paren" style="font-size:5em;">]</span>
    </div>
