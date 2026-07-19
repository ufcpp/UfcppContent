---
title: "曲面上の運動"
source_url: "https://ufcpp.net/study/physics/dynamics/surface/"
content_type: "Article"
published_at: "2007-04-30T00:00:00"
updated_at: "2015-05-06T14:19:59"
tags: []
umbraco_id: 1560
parent_id: 1554
sort_order: 5
aliases:
  - "/dynamics/surface"
  - "/dynamics/surface.html"
  - "/physics/dynamics/surface/"
  - "/study/dynamics/surface"
  - "/study/dynamics/surface.html"
---

# 曲面上の運動

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[ラグランジュ形式](lagrange.md)」や「[ハミルトン形式](hamilton.md)」で解説したような、
座標系の取り方によらない運動方程式に関して、
曲面上に拘束された物体の運動について考えてみます。
 
また、具体例として、単位球面上の運動を考えて、
数値計算により物体の軌跡を求めてみます。


## <a id="sec-generated-title-2"></a> <a id="surface"></a>曲面上のラグランジアン

正規直交座標系 <span class="math">x, y, z</span> を用いると、
ラグランジアンは
<span class="math">
L
<span class="normal">=</span>
T <span class="normal">−</span> V
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>m
<span class="paren" style="font-size:1.2em;">(</span>
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">+</span>
z'<sup><span class="normal">2</span></sup><span class="paren" style="font-size:1.2em;">)</span><span class="normal">−</span>
φ
<span class="paren" style="font-size:em;">(</span>x, y, z<span class="paren" style="font-size:em;">)</span></span>
と表されます。
（ただし、変数に付いた ' は時間微分を表すものとする。）
これが、曲面上ではどう表されるかを考えてみましょう。
 
3次元空間上の曲面は、
2つの媒介変数
<span class="math">q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub></span>
を用いて、
<div class="math">
      <span class="paren" style="font-size:1.2em;">(</span>
x<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span>,
y<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span>,
z<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.2em;">)</span>
    </div>
と表すことができます。
で、物体がこの曲面上に垂直抗力で拘束されているものとします。
 
垂直抗力は仕事をしない（＝ ポテンシャルには影響しない）ので、
まず、<span class="math">φ</span> の方は、
単純に <span class="math">x <span class="normal">=</span> x<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span></span> 等を代入して、
<div class="math">
φ<sub>q</sub><span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
φ
<span class="paren" style="font-size:em;">(</span>
x<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span>,
y<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span>,
z<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span></div>
と表すことができます。
（座標 <span class="math">q</span> で表したポテンシャルであることを明示するために
<span class="math">φ<sub>q</sub></span> と書きましたが、
文脈で分かる場合には、
<span class="math">
φ<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
φ
<span class="paren" style="font-size:em;">(</span>
x<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span>,
y<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span>,
z<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span></span>
と書いたりもします。）
 
一方、運動エネルギーの方は、
微分の座標変換法則が必要になります。
全微分公式から、
<div class="math">
x'
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂x</td></tr><tr><td>∂<denom>q<sub><span class="normal">1</span></sub></denom></td></tr></table> q<sub><span class="normal">1</span></sub>'
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂x</td></tr><tr><td>∂<denom>q<sub><span class="normal">2</span></sub></denom></td></tr></table> q<sub><span class="normal">2</span></sub>'
</div><div class="math">
y'
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂y</td></tr><tr><td>∂<denom>q<sub><span class="normal">1</span></sub></denom></td></tr></table> q<sub><span class="normal">1</span></sub>'
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂y</td></tr><tr><td>∂<denom>q<sub><span class="normal">2</span></sub></denom></td></tr></table> q<sub><span class="normal">2</span></sub>'
</div><div class="math">
z'
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂z</td></tr><tr><td>∂<denom>q<sub><span class="normal">1</span></sub></denom></td></tr></table> q<sub><span class="normal">1</span></sub>'
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂z</td></tr><tr><td>∂<denom>q<sub><span class="normal">2</span></sub></denom></td></tr></table> q<sub><span class="normal">2</span></sub>'
</div>
となるので、
運動エネルギーはこれらの二乗和になるんですが、
二乗和をいちいち書くのも面倒なので、
以下のような記号を用意します。
<div class="math">
q
<span class="normal">=</span><span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></div><div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>g<sub><span class="normal">11</span></sub></td><td>g<sub><span class="normal">12</span></sub></td></tr><tr><td>g<sub><span class="normal">21</span></sub></td><td>g<sub><span class="normal">22</span></sub></td></tr></table><span class="paren" style="font-size:3em;">]</span></div><div class="math">
g<sub><span class="normal">ij</span></sub><span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂x</td></tr><tr><td>∂<denom>q<sub>i</sub></denom></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂x</td></tr><tr><td>∂<denom>q<sub>j</sub></denom></td></tr></table><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂y</td></tr><tr><td>∂<denom>q<sub>i</sub></denom></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂y</td></tr><tr><td>∂<denom>q<sub>j</sub></denom></td></tr></table><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂z</td></tr><tr><td>∂<denom>q<sub>i</sub></denom></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂z</td></tr><tr><td>∂<denom>q<sub>j</sub></denom></td></tr></table></div>
ちなみに、<span class="math">g</span> は対称行列になっています。
この行列 <span class="math">g</span> を曲面上の計量といいます。
また、<span class="math">g</span> の各要素は <span class="math">q</span> の関数になっているので、
これを明示するために、ここでは <span class="math">g<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></span> と書きましょう。
すると、運動エネルギー <span class="math">T</span> は以下のように表すことができます。
<div class="math">
T
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>m q'<sup>T</sup> g<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span> q'
</div>
したがって、ラグランジアンは以下のようになります。
<div class="math">
L
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>m q'<sup>T</sup> g<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span> q'
<span class="normal">−</span>
φ<sub>q</sub><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></div>

## <a id="sec-generated-title-3"></a> <a id="d23e373"></a>曲面上のハミルトン形式

ラグランジアンの形が分かれば、
ラグランジュ形式やハミルトン形式の運動方程式が立てられます。
まずは、
<span class="math">q <span class="normal">=</span><span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
の共役運動量
<span class="math">p <span class="normal">=</span><span class="paren" style="font-size:em;">(</span>p<sub><span class="normal">1</span></sub>, p<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
を求めると、以下の通り。
<div class="math">
p
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂L</td></tr><tr><td>∂<denom>q'</denom></td></tr></table><span class="normal">=</span>
m g<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span> q'
</div>
したがって、ハミルトニアンは、
<div class="math">
H
<span class="normal">=</span>
p<sup>T</sup> q'
<span class="normal">−</span>
L
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>m q'<sup>T</sup> g<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span> q
<span class="normal">+</span>
φ<sub>q</sub><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span>m</td></tr></table> p<sup>T</sup> g<sup><span class="normal">−1</span></sup><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span> p
<span class="normal">+</span>
φ<sub>q</sub><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></div>
となります。
<span class="math">g<sup><span class="normal">−1</span></sup><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></span> は <span class="math">g<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></span> の逆行列なんですが、
一応、要素を書き下しておくと、以下の通りです。
<div class="math">
g<sup><span class="normal">−1</span></sup><span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>g<sup><span class="normal">11</span></sup></td><td>g<sup><span class="normal">12</span></sup></td></tr><tr><td>g<sup><span class="normal">21</span></sup></td><td>g<sup><span class="normal">22</span></sup></td></tr></table><span class="paren" style="font-size:3em;">]</span></div><div class="math">
d
<span class="normal">=</span>
g<sub><span class="normal">11</span></sub>g<sub><span class="normal">22</span></sub><span class="normal">−</span>
g<sub><span class="normal">12</span></sub><sup><span class="normal">2</span></sup></div><div class="math">
g<sup><span class="normal">11</span></sup><span class="normal">=</span> g<sub><span class="normal">22</span></sub><span class="normal">/</span> d
</div><div class="math">
g<sup><span class="normal">22</span></sup><span class="normal">=</span> g<sub><span class="normal">11</span></sub><span class="normal">/</span> d
</div><div class="math">
g<sup><span class="normal">12</span></sup><span class="normal">=</span>
g<sup><span class="normal">21</span></sup><span class="normal">=</span><span class="normal">−</span>g<sub><span class="normal">12</span></sub><span class="normal">/</span> d
</div>
で、ハミルトン形式の運動方程式を立てると、以下の通り。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>q
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
H
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span>m</td></tr></table>
g<sup><span class="normal">−1</span></sup><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span> p
</div><div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>p
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
H
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span>m</td></tr></table>
p<sup>T</sup><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>g<sup><span class="normal">−1</span></sup><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span> p
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>φ<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></div>

## <a id="sec-generated-title-4"></a> <a id="example"></a>具体例: 単位球面上の運動

前節までで、曲面上のハミルトン形式を導出したわけですが、
まだ「よく分からない式」だと思うので、
具体例を挙げてみましょう。
 
簡単な例ということで、
高さに比例したポテンシャル <span class="math">φ <span class="normal">=</span> mGz</span>
（<span class="math">m</span> は質量で、<span class="math">G</span> は重力定数）
が働いているときの、
単位球面上の運動を考えてみます。


### <a id="sec-generated-title-5"></a> <a id="sphere"></a>ハミルトン形式の導出

単位球面は、
<div class="math">
x<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">cos</span> q<sub><span class="normal">1</span></sub><span class="normal">sin</span> q<sub><span class="normal">2</span></sub></div><div class="math">
y<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">sin</span> q<sub><span class="normal">1</span></sub><span class="normal">sin</span> q<sub><span class="normal">2</span></sub></div><div class="math">
z<span class="paren" style="font-size:em;">(</span>q<sub><span class="normal">1</span></sub>, q<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">−</span><span class="normal">cos</span> q<sub><span class="normal">2</span></sub></div>
という式で表すことができます。
（要するに、<span class="math">q<sub><span class="normal">1</span></sub></span> が水平角で、
<span class="math">q<sub><span class="normal">2</span></sub></span> が仰角。
<span class="math">q<sub><span class="normal">2</span></sub><span class="normal">=</span><span class="normal">0</span></span> 付近が一番下で、
<span class="math">q<sub><span class="normal">2</span></sub><span class="normal">=</span> π</span> 付近が一番上。）
 
微分すると、
<div class="math">
        <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>x'</td></tr><tr><td>y'</td></tr><tr><td>z'</td></tr></table><span class="paren" style="font-size:4em;">]</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
              <span class="normal">−</span>
              <span class="normal">sin</span> q<sub><span class="normal">1</span></sub><span class="normal">sin</span> q<sub><span class="normal">2</span></sub></td><td>
              <span class="normal">cos</span> q<sub><span class="normal">1</span></sub><span class="normal">cos</span> q<sub><span class="normal">2</span></sub></td></tr><tr><td>
              <span class="normal">cos</span> q<sub><span class="normal">1</span></sub><span class="normal">sin</span> q<sub><span class="normal">2</span></sub></td><td>
              <span class="normal">sin</span> q<sub><span class="normal">1</span></sub><span class="normal">cos</span> q<sub><span class="normal">2</span></sub></td></tr><tr><td>
              <span class="normal">0</span>
            </td><td>
              <span class="normal">sin</span> q<sub><span class="normal">2</span></sub></td></tr></table><span class="paren" style="font-size:4em;">]</span>
        <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>q<sub><span class="normal">1</span></sub>'</td></tr><tr><td>q<sub><span class="normal">2</span></sub>'</td></tr></table><span class="paren" style="font-size:3em;">]</span>
      </div>
で、計量 <span class="math">g</span> は、
<div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td><span class="normal">sin</span><sup><span class="normal">2</span></sup> q<sub><span class="normal">2</span></sub></td><td><span class="normal">0</span></td></tr><tr><td><span class="normal">0</span></td><td><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:3em;">]</span>
,   
g<sup><span class="normal">−1</span></sup><span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td><span class="normal">1</span><span class="normal">/</span><span class="normal">sin</span><sup><span class="normal">2</span></sup> q<sub><span class="normal">2</span></sub></td><td><span class="normal">0</span></td></tr><tr><td><span class="normal">0</span></td><td><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
となります。
また、
<span class="math">g<sup><span class="normal">−1</span></sup>, φ <span class="normal">=</span><span class="normal">−</span>mG <span class="normal">cos</span> q<sub><span class="normal">2</span></sub></span> を<span class="math">q</span> で微分すると、
<div class="math">
        <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q<sub><span class="normal">1</span></sub></denom></td></tr></table>
g<sup><span class="normal">−1</span></sup><span class="normal">=</span>
0
,    <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q<sub><span class="normal">2</span></sub></denom></td></tr></table>
g<sup><span class="normal">−1</span></sup><span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td><span class="normal">−</span>2<span class="normal">cos</span> q<sub><span class="normal">2</span></sub><span class="normal">/</span><span class="normal">sin</span><sup><span class="normal">3</span></sup> q<sub><span class="normal">2</span></sub></td><td><span class="normal">0</span></td></tr><tr><td><span class="normal">0</span></td><td><span class="normal">0</span></td></tr></table><span class="paren" style="font-size:3em;">]</span></div><div class="math">
        <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q<sub><span class="normal">1</span></sub></denom></td></tr></table>
φ
<span class="normal">=</span>
0
,    <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q<sub><span class="normal">2</span></sub></denom></td></tr></table>
φ
<span class="normal">=</span>
mG <span class="normal">sin</span> q<sub><span class="normal">2</span></sub></div>
が得られます。
これらを前節の結果に代入すると、以下の微分方程式が得られます。
<div class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>q<sub><span class="normal">1</span></sub><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">p<sub><span class="normal">1</span></sub></td></tr><tr><td>m <span class="normal">sin</span><sup><span class="normal">2</span></sup> q<sub><span class="normal">2</span></sub></td></tr></table></div><div class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>q<sub><span class="normal">1</span></sub><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">p<sub><span class="normal">2</span></sub></td></tr><tr><td>m</td></tr></table></div><div class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>p<sub><span class="normal">1</span></sub><span class="normal">=</span><span class="normal">0</span></div><div class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>p<sub><span class="normal">2</span></sub><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">p<sub><span class="normal">1</span></sub><sup><span class="normal">2</span></sup><span class="normal">cos</span> q<sub><span class="normal">2</span></sub></td></tr><tr><td>m <span class="normal">sin</span><sup><span class="normal">3</span></sup> q<sub><span class="normal">2</span></sub></td></tr></table><span class="normal">−</span>
mG <span class="normal">sin</span> q<sub><span class="normal">2</span></sub></div>

### <a id="sec-generated-title-6"></a> <a id="numerical"></a>数値計算

微分方程式を導出したからといって、
厳密解を解析的に求められるわけではないんですが、
ハミルトン形式にしてしまえば、
数値計算は簡単にできます。
 
ハミルトン形式のように、
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x <span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
という形になっている微分方程式は、
例えば、以下のような反復計算で近似解が得られます。
<div class="math">
x<span class="paren" style="font-size:em;">(</span>t <span class="normal">+</span> Δt<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>
f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span>
Δt
</div>
この方法はオイラー法と呼ばれているもので、
<span class="math">f</span> のテイラー展開に関して1次程度の近似度の数値計算法です。
（式自体は簡単だけど、精度はあまり高くない。）
 
では、オイラー法を用いて、球面上の物体の運動を数値計算してみましょう。
プログラム例を C# 3.0 で示すと、以下のようになります。
（参考：
「[C# によるプログラミング入門](../../csharp/index.md)」「[C# 3.0 の新機能](../../csharp/cheatsheet/ap_ver3.md)」）

```csharp
using Func2 = System.Linq.Func<double, double, double>;
using Func4 = System.Linq.Func<double, double, double, double, double>;

static void Simulate()
{
  const double M = 0.1;
  const double G = 10;

  Func2 x = (q1_, q2_) => (Math.Cos(q1_) * Math.Sin(q2_));
  Func2 y = (q1_, q2_) => (Math.Sin(q1_) * Math.Sin(q2_));
  Func2 z = (q1_, q2_) => (-Math.Cos(q2_));

  Func4 fq1 =
    (q1_, q2_, p1_, p2_) => (p1_ / (M * Math.Sin(q2_)));
  Func4 fq2 =
    (q1_, q2_, p1_, p2_) => (p2_ / M);
  Func4 fp1 =
    (q_1, q2_, p1_, p2_) => (0);
  Func4 fp2 =
    (q1_, q2_, p1_, p2_) => (
      (p1_ * p1_ * Math.Cos(q2_))
        / (M * Math.Sin(q2_) * Math.Sin(q2_) * Math.Sin(q2_))
      - M * G * Math.Sin(q2_)
      );

  double q1 = 0;
  double q2 = Math.PI / 2;
  double p1 = 0.1;
  double p2 = 0;

  const double dt = 0.01;
  const double t_end = 10;
  const int DISPLAY_INTERVAL = 5;

  Console.Write("t,x,y,z\n");

  int n = 0;
  for (double t = 0; t < t_end; t += dt, ++n)
  {
    q1 += dt * fq1(q1, q2, p1, p2);
    q2 += dt * fq2(q1, q2, p1, p2);
    p1 += dt * fp1(q1, q2, p1, p2);
    p2 += dt * fp2(q1, q2, p1, p2);

    if (n == DISPLAY_INTERVAL)
    {
      Console.Write("{0},{1},{2},{3}\n",
        t,
        x(q1, q2), y(q1, q2), z(q1, q2)));
    }
  }
}
```


ちなみに、
もう少し込み入ったことをしてる（精度の高い数値計算法を使ったり）サンプルプログラムも作ったので、
置いておきます →

[surface.cs](../../../../assets/media/ufcpp2000/physics/surface.cs)
。
（GUI 版、.NET Framework 3.0 必須 → [ソースファイル一式](../../../../assets/surface.zip)。）
 
別ページにてバージョンアップ →
その1：「[曲面上の物体の運動シミュレーション](../../dotnet/appendix/sample.md#dynamics)」、
その2：「[Expression Tree ＋ CodeDom ＋ WPF](../../csharp/sample/sp3_expressionsample.md#dynamics)」。
