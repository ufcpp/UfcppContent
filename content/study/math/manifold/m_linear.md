---
title: "線形な座標系"
source_url: "https://ufcpp.net/study/math/manifold/m_linear/"
content_type: "Article"
published_at: "2015-05-06T14:18:24"
updated_at: "2015-05-18T17:23:57"
tags: []
umbraco_id: 1516
parent_id: 1515
sort_order: 0
aliases:
  - "/study/manifold/linear.html"
---

# 線形な座標系

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

多様体の話に入る前に、
座標系というものの復習をしましょう
そのために、ここではまず、
複雑なものは考えず、単純な線形座標系について説明したいと思います。
 
それも、特に単純な場合として、
2次元の実数ベクトル <span class="math"><span class="bold">R</span><sup>2</sup></span> を中心に説明しますが、
ここでの説明は一般の体 <span class="math">K</span> の任意の次数のベクトル空間 <span class="math">K<sup>n</sup></span> に対して成り立ちます。
 
ちなみに、非線形な座標系でも、
各点の近傍を微視的な視点で見ると線形になっているので、
まずは線形な座標系のイメージを掴んでもらうと、
非線形な場合の理解も早まるかと思います。


## <a id="sec-generated-title-2"></a> <a id="coordinate"></a>ベクトルの座標

最初に述べたように、2次元の実数ベクトル <span class="math"><span class="bold">R</span><sup>2</sup></span> に関して説明します。

<span class="math">
        <span class="bold">R</span>
        <sup>2</sup>
      </span> 上のある点 <span class="math">p</span> が
2つのベクトル <span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub></span> の線形結合
<div class="math">
x <span class="vector">a</span><sub>1</sub> ＋ y <span class="vector">a</span><sub>2</sub></div>
で表されるとき、
<span class="math"><span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><sup>T</sup></span> を点 <span class="math">p</span> の
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">}</span></span> に関する座標（coordinate）といいます。
（右肩の <span class="math">T</span> の記号は転置を表します。）

<span class="math">
        <span class="bold">R</span>
        <sup>2</sup>
      </span> 上の全ての点は
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">}</span></span> に関する座標で表すことが出来るわけで、
このとき、<span class="math"><span class="bold">R</span><sup>2</sup></span> に<strong id="system" class="keyword">座標系</strong>（coordinate system）が導入されると言います。
（正確には、座標系の原点も指定する必要がありますが、特に断りのない場合、
<span class="math"><span class="paren" style="font-size:em;">(</span>0, 0<span class="paren" style="font-size:em;">)</span><sup>T</sup></span> を原点とします。）
 
また、
この2つのベクトル
<span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub></span> 
を、座標系の<strong id="base" class="keyword">基底</strong>（base）あるいは基と言います。
基底の集合
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">}</span></span> 
自体のことを座標系という場合もあります。

<span class="math">
        <span class="vector">
          <span class="vector">a</span>
          <sub>1</sub> ＝ e</span>
        <sub>x</sub> ＝ <span class="paren" style="font-size:em;">(</span>1, 0<span class="paren" style="font-size:em;">)</span><sup>T</sup>,
</span>
<span class="math">
        <span class="vector">
          <span class="vector">a</span>
          <sub>2</sub> ＝ e</span>
        <sub>y</sub> ＝ <span class="paren" style="font-size:em;">(</span>0, 1<span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
の場合がいわゆる正規直交座標です。
一般には、
<span class="math"><span class="vector">a</span><sub>1</sub></span>
と
<span class="math"><span class="vector">a</span><sub>2</sub></span> 
が平行でさえなければ任意のベクトルを使ってかまいません。
 
先ほどの式は、
<span class="math"><span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><sup>T</sup>, </span><span class="math"><span class="vector">A</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">)</span></span>
と置くと、
<div class="math">
      <span class="vector">A</span>
      <span class="vector">x</span>
    </div>
というように、行列の形で表すことができます。


## <a id="sec-generated-title-3"></a> <a id="transform"></a>座標変換

座標の取り方は一通りではなく、
同じ空間を表すのに複数の座標系が存在します。
ここでは <span class="math"><span class="bold">R</span><sup>2</sup></span> を表す2つの座標を考え、
それらの基底を
<span class="math"><span class="vector">A</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">)</span>, </span><span class="math"><span class="vector">B</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">b</span><sub>1</sub> , <span class="vector">b</span><sub>2</sub><span class="paren" style="font-size:em;">)</span></span>
で表します。
 
ある点 <span class="math">p</span> を、この2つの基底を使って表した座標をそれぞれ
<span class="math"><span class="vector">x</span><sub>a</sub> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>a</sub> , y<sub>a</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup>, </span><span class="math"><span class="vector">x</span><sub>b</sub> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>b</sub> , y<sub>b</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
としましょう。
この2つが同じ点を表すわけですから、当然、
<div class="math">
      <span class="vector">A</span>
      <span class="vector">x</span>
      <sub>a</sub>
＝
<span class="vector">B</span><span class="vector">x</span><sub>b</sub></div>
が成り立ちます。
したがって、2つの座標の間には、以下の関係式が成り立っています。
<div class="math">
      <span class="vector">x</span>
      <sub>b</sub>
＝
<span class="vector">B</span><sup>－1</sup><span class="vector">A</span><span class="vector">x</span><sub>a</sub></div>
この式により、
ある座標系から他の座標系への変換が可能です。
このような変換を座標変換（cooridinate transformation, cooridinate conversion, cooridinate change）と言います。


## <a id="sec-generated-title-4"></a> <a id="dualspace"></a>双対空間

体 <span class="math">K</span> 上の線形空間 <span class="math">V</span> に対し、
<span class="math">V → K</span> の線形写像全体の空間
<span class="math">
V<sup>*</sup>
＝
<span class="paren" style="font-size:em;">{</span>f : V → K | f <span class="normal">は線形写像</span><span class="paren" style="font-size:em;">}</span></span>
を <span class="math">V</span> の<strong id="dualspace" class="keyword">双対空間</strong>（dual space）と言います。
 
実数ベクトル <span class="math"><span class="bold">R</span><sup>n</sup></span> の場合、
線形写像 <span class="math"><span class="bold">R</span><sup>n</sup> → <span class="bold">R</span></span> は
ベクトルの内積で表されます。
例えば、2次元の場合、
<span class="math"><span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ∈ <span class="bold">R</span><sup>2</sup></span> に対して、
内積
<span class="math">a x ＋ b y</span>（<span class="math">a, b ∈ <span class="bold">R</span></span>）
を取ると1つの実数値が得られ、
これは <span class="math"><span class="bold">R</span><sup>2</sup> → <span class="bold">R</span></span> の線形写像になっています。
 
内積の形で表されることからも分かるように、
双対空間の双対空間は元の空間と一致します（<span class="math">V<sup>**</sup> ＝ V</span>）。
双対（dual：2つ組の）という言葉はこのことに由来します。
 
2次元実数ベクトルの話に戻りますが、先ほどの線形写像は2次元のベクトル
<span class="math"><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span></span>
で表されています。
すなわち、
<span class="math"><span class="bold">R</span><sup>2</sup></span>
の双対空間は
<span class="math"><span class="bold">R</span><sup>2</sup></span>
自身と同型な空間になります。
 
詳しい証明はしませんが、
このことは一般の線形空間についても成り立ち、
体 <span class="math">K</span> 上の <span class="math">n</span> 次元線形空間 <span class="math">K<sup>n</sup></span> の双対空間は、
やはり <span class="math">K</span> 上の <span class="math">n</span> 次元線形空間になります。
 
では、なぜ、結局元と同じ空間になるのにわざわざ双対空間などと言うのかですが、
それは以下で説明する双対座標の座標変換を見ていただければはっきりすると思います。


## <a id="sec-generated-title-5"></a> <a id="dualcoordinate"></a>双対座標

先ほどの説明では、座標系の話はあいまいにぼかして説明していましたが、
ここでは双対空間に座標系を導入することを考えて見ましょう。
 
体 <span class="math">K</span> 上の
<span class="math">n</span> 次元線形空間 <span class="math">V</span> が基底
<span class="math"><span class="vector">v</span><sub>i</sub></span>（<span class="math">i ＝ 1 ～ n</span>）
の座標系を持っているものとしましょう。
そして、<span class="math">V</span> の双対空間を <span class="math">V<sup>*</sup></span> で表します。
先ほど説明したように、
<span class="math">V<sup>*</sup></span> も <span class="math">n</span> 次元線形空間になります。
したがって、<span class="math">V<sup>*</sup></span> にも <span class="math">n</span> 個の基底
<span class="math"><span class="vector">f</span><sub>i</sub></span>（<span class="math">i ＝ 1 ～ n</span>）
を用意して座標系を導入できます。

<span class="math">
        <span class="vector">f</span>
        <sub>i</sub>
      </span> は <span class="math">V</span> の元 <span class="math"><span class="vector">x</span></span>に対する線形写像で、
写像の値 <span class="math"><span class="vector">f</span><sub>i</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> は体 <span class="math">K</span> の元になります。
以下、
<span class="math"><span class="vector">f</span><sub>i</sub><span class="paren" style="font-size:em;">(</span><span class="vector">x</span><span class="paren" style="font-size:em;">)</span></span> の括弧は省略して、
単に
<span class="math"><span class="vector">f</span><sub>i</sub><span class="vector">x</span></span>
と書き表します。
 
基底 <span class="math"><span class="vector">f</span><sub>i</sub></span> は任意に選ぶことができるため、
元の空間の基底 <span class="math"><span class="vector">v</span><sub>i</sub></span> との関係が明瞭になるように恣意的なものを選びます。
<span class="math"><span class="vector">f</span><sub>i</sub></span> と
<span class="math"><span class="vector">v</span><sub>i</sub></span> の関係が最も明瞭なのは、
以下の条件を満たすように選んだ時でしょう。
<div class="math">
      <span class="vector">f</span>
      <sub>i</sub>
      <span class="vector">v</span>
      <sub>j</sub>
＝
δ<sub>ij</sub></div>
<span class="math">
δ<sub>ij</sub></span>
はクロネッカーのδで、
<span class="math">i ＝ j</span> のときだけ1、それ以外の時には 0 を表す記号です。
このような条件を満たす基底 <span class="math"><span class="vector">f</span><sub>i</sub></span> を、
<span class="math"><span class="vector">v</span><sub>i</sub></span> の双対基底（dual base）と言います。
そして、双対基底による双対空間上の座標を<strong id="dualcoordinate" class="keyword">双対座標</strong>（dual coordinate）と呼びます。
 
それぞれの基底を行列で表し、
<span class="math"><span class="vector">V</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">v</span><sub>1</sub>, ・・・ , <span class="vector">v</span><sub>n</sub><span class="paren" style="font-size:em;">)</span>, 
</span><span class="math"><span class="vector">F</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">f</span><sub>1</sub>, ・・・ , <span class="vector">f</span><sub>n</sub><span class="paren" style="font-size:em;">)</span>, 
</span>
とすると、
<div class="math">
      <span class="vector">F</span> ＝ 
<span class="vector">V</span><sup>－1・T</sup></div>
という関係が成り立ちます。
 
元の空間上の点 <span class="math">p</span> は、
座標 <span class="math"><span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>1</sub>, ・・・ , x<sub>n</sub><span class="paren" style="font-size:em;">)</span></span>
により、
<div class="math">
p
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝1</td></tr></table>
x<sub>i</sub><span class="vector">v</span><sub>i</sub>
＝
<span class="vector">V</span><span class="vector">x</span></div>
で、
双対空間上の点 <span class="math">q</span> は
座標 <span class="math"><span class="vector">y</span> ＝ <span class="paren" style="font-size:em;">(</span>y<sub>1</sub>, ・・・ , y<sub>n</sub><span class="paren" style="font-size:em;">)</span></span>
により、
<div class="math">
q
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝1</td></tr></table>
y<sub>i</sub><span class="vector">f</span><sub>i</sub>
＝
<span class="vector">F</span><span class="vector">y</span></div>
で表されることになりますが、
<span class="math">p</span> と <span class="math">q</span> の内積をとると、
<div class="math">
q<sup>T</sup> p
＝ 
<span class="paren" style="font-size:em;">(</span><span class="vector">F</span><span class="vector">y</span><span class="paren" style="font-size:em;">)</span><sup>T</sup><span class="vector">V</span><span class="vector">x</span>
＝ 
<span class="vector">y</span><sup>T</sup><span class="vector">V</span><sup>－1</sup><span class="vector">V</span><span class="vector">x</span>
＝ 
<span class="vector">y</span><sup>T</sup><span class="vector">x</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝1</td></tr></table>
x<sub>i</sub>
y<sub>i</sub></div>
となり、それぞれの座標の値を、数ベクトルの内積と同じルールで積和したものと一致します。


## <a id="sec-generated-title-6"></a> <a id="dualtransform"></a>双対座標の座標変換

「[座標変換](#transform)」では、
同じ点 <span class="math">p</span> の、
2つの基底
<span class="math"><span class="vector">A</span> ＝ <span class="paren" style="font-size:em;">(</span>a<sub>1</sub> , a<sub>2</sub><span class="paren" style="font-size:em;">)</span></span> と
<span class="math"><span class="vector">B</span> ＝ <span class="paren" style="font-size:em;">(</span>b<sub>1</sub> , b<sub>2</sub><span class="paren" style="font-size:em;">)</span></span>
を使って表した座標をそれぞれ
<span class="math"><span class="vector">x</span><sub>a</sub> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>a</sub> , y<sub>a</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup>, </span><span class="math"><span class="vector">x</span><sub>b</sub> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>b</sub> , y<sub>b</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
とすると、
<div class="math">
      <span class="vector">x</span>
      <sub>b</sub>
＝
<span class="vector">B</span><sup>－1</sup><span class="vector">A</span><span class="vector">x</span><sub>a</sub></div>
という変換法則が成り立つことを説明しました。
これに対して、双対座標の場合にはどのような変換法則が成り立つのかを見てみましょう。

<span class="math">
        <span class="vector">A</span>, <span class="vector">B</span></span> の双対基底をそれぞれ
<span class="math"><span class="vector">A</span><sup>*</sup> ＝ A<sup>－1・T</sup>, </span><span class="math"><span class="vector">B</span><sup>*</sup> ＝ B<sup>－1・T</sup></span>
と表します。
双対空間上の点 <span class="math">q</span> の、
これらの基底を使った座標をベクトル表現でそれぞれ
<span class="math"><span class="vector">y</span><sub>a</sub> , <span class="vector">y</span><sub>b</sub></span>
で表すと、
<div class="math">
      <span class="vector">A</span>
      <sup>*</sup>
      <span class="vector">y</span>
      <sub>a</sub>
＝
<span class="vector">B</span><sup>*</sup><span class="vector">y</span><sub>b</sub></div><div class="math">
∴
<span class="vector">y</span><sub>b</sub>
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">B</span><sup>*</sup><span class="paren" style="font-size:em;">)</span><sup>－1</sup><span class="vector">A</span><sup>*</sup><span class="vector">y</span><sub>a</sub></div>
になるわけですが、
ここに元の基底と双対基底の関係式を代入することにより、
以下のようになります。
<div class="math">
      <span class="vector">y</span>
      <sub>b</sub>
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">B</span><span class="vector">A</span><sup>－1</sup><span class="paren" style="font-size:em;">)</span><sup>T</sup><span class="vector">y</span><sub>a</sub></div>
この式が双対座標の座標変換の式であり、
元の座標の変換式とは異なる形になります。
 
双対空間は、
線形空間としての構造は元の空間と同じ
（<span class="math">K<sup>n</sup></span> の双対空間はやはり <span class="math">K<sup>n</sup></span>）ですが、
このように、
双対座標系を導入しその座標変換を考えると
元の空間の座標変換と異なる変換法則が得られます。


## <a id="sec-generated-title-7"></a> <a id="variance"></a>反変座標と共変座標

これまでの話を少し違った視点から見てみましょう。
基底 <span class="math"><span class="vector">A</span></span> を用いて、
<span class="math">
p ＝ <span class="vector">A</span><span class="vector">x</span></span>
の形で点 <span class="math">p</span> を表す座標 <span class="math"><span class="vector">x</span></span> を単に座標と呼び、
<span class="math">
p ＝ <span class="vector">A</span><sup>－1・T</sup><span class="vector">y</span></span>
の形で表す座標 <span class="math"><span class="vector">y</span></span> を双対座標と呼ぶ、
というように考えることも出来ます。


### <a id="sec-generated-title-8"></a> <a id="contravariance"></a>反変座標

まず、前者、
<span class="math">
p ＝ <span class="vector">A</span><span class="vector">x</span></span>
で表される座標 <span class="math"><span class="vector">x</span></span> について考えてみましょう。
 
単純化のため、2次元の場合について考えます。
<span class="math"><span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><sup>T</sup>, </span><span class="math"><span class="vector">A</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">)</span></span>
とすると、
この式は、
基底
<span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub></span>
の線形結合を表しています。
この表現方法は、図的に考えると図1のようになります。

<figure>

[![基底の線形結合による座標表現](../../../../assets/media/ufcpp2000/math/linear0.emf)](../../../../assets/media/ufcpp2000/math/linear0.emf)

<figcaption>基底の線形結合による座標表現</figcaption>
</figure>


ところで、この方式を取ると、
基底
<span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub></span> がそれぞれ
<span class="math">g</span> 倍、<span class="math">h</span> 倍になると、
座標 <span class="math">x, y</span> はそれぞれ
<span class="math">1/g</span> 倍、<span class="math">1/h</span> 倍になります。
これは以下の式から示されます。
<div class="math">
x <span class="vector">a</span><sub>1</sub>
＋
y <span class="vector">a</span><sub>2</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num">x</td></tr><tr><td>g</td></tr></table><span class="paren" style="font-size:em;">(</span>g <span class="vector">a</span><sub>1</sub><span class="paren" style="font-size:em;">)</span>
＋
<table class="frac" summary="fraction"><tr><td class="num">y</td></tr><tr><td>h</td></tr></table><span class="paren" style="font-size:em;">(</span>h <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">)</span></div>
そのため、この方式で表された座標、
すなわち、先ほどまで単に座標と呼んでいたものを、
基底と反対の変化をすると言う意味で、
<strong id="contravariant" class="keyword">反変座標</strong>（contravariant coordinage）とも呼びます。


### <a id="sec-generated-title-9"></a> <a id="covariance"></a>共変座標

では、後者の
<span class="math">
p ＝ <span class="vector">A</span><sup>－1・T</sup><span class="vector">x</span></span>
で表される座標について考えてみましょう。
 
このままではちょっと分かりづらいんですが、
先ほどと同様に、
<span class="math"><span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><sup>T</sup>, </span><span class="math"><span class="vector">A</span> ＝ <span class="paren" style="font-size:em;">(</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub><span class="paren" style="font-size:em;">)</span></span>
とすると、
<span class="math">x, y</span> は、
<div class="math">
x ＝ <span class="vector">a</span><sub>1</sub><sup>T</sup> p
</div><div class="math">
y ＝ <span class="vector">a</span><sub>2</sub><sup>T</sup> p
</div>
になります。
すなわち、基底との内積の値を座標にしていることになります。
反変座標と比べると、少しイメージが沸きづらいですが、
少々無理やりに、図的に表すと図2のようになります。
（少しごまかしあり。あくまでイメージを表す図です。）

<figure>

[![基底の内積による座標表現](../../../../assets/media/ufcpp2000/math/linear1.emf)](../../../../assets/media/ufcpp2000/math/linear1.emf)

<figcaption>基底の内積による座標表現</figcaption>
</figure>


さて、この方式を取った場合には、
基底
<span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub></span> がそれぞれ
<span class="math">g</span> 倍、<span class="math">h</span> 倍になると、
座標 <span class="math">x, y</span> もそれぞれ
<span class="math">g</span> 倍、<span class="math">h</span> 倍になります。
<div class="math">
g x ＝ g <span class="vector">a</span><sub>1</sub><sup>T</sup> p
</div><div class="math">
h y ＝ h <span class="vector">a</span><sub>2</sub><sup>T</sup> p
</div>
そのため、この方式、すなわち、双対座標を、
基底と同じ変化をすると言う意味で、
<strong id="covariant" class="keyword">共変座標</strong>（covariant coordinate）とも呼びます。
 
ちなみに、定義から明らかですが、
正規直交座標を用いる場合には反変座標と共変座標は一致します。
