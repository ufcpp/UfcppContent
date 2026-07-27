---
title: "ハミルトン形式"
source_url: "https://ufcpp.net/study/physics/dynamics/hamilton/"
content_type: "Article"
published_at: "2007-04-08T00:00:00"
updated_at: "2007-05-01T00:00:00"
tags: []
umbraco_id: 1559
parent_id: 1554
sort_order: 4
aliases:
  - "/study/dynamics/hamilton.html"
---

# ハミルトン形式

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[ラグランジュの運動方程式](lagrange.md#equation)」は、
以下のように、<span class="math">t, q, q'</span> に関する微分を含む2階の微分方程式になっています。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
L
<span class="normal">=</span><span class="normal">0</span></div>
これに対して、
微分方程式を解くときの常套手段の1つなんですが、
変数を増やす代わりに階数を減らすことで、
微分方程式を解きやすい形に変形することがあります。
 
結果だけ先に書いてしまうなら、
<div class="math">
p
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>q, p<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
p q' <span class="normal">−</span> L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div>
と置くことで、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
q
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
H
</div><div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
p
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
H
</div>
が得られます。
1階の微分方程式になっていますし、
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x <span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> という、
定性的な解析や、数値計算がしやすい形式になっています。


## <a id="sec-generated-title-2"></a> <a id="derivation"></a>ハミルトン形式の導出

というわけで、
<div class="math">
p
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div>
と置いてみます。
まあ、要するに、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table></span> の部分を消したいんだから、
ここを別変数においてみようということです。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
p
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
L
</div>
となって、一応、1階の微分方程式になっています。
（ただし、変数 <span class="math">p</span> が増えた。）
あとは、<span class="math">p</span> の定義式の方から <span class="math">p</span> と <span class="math">q'</span> の関係を逆にして、
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
q
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
L
</span>
みたいにできれば（<span class="math">q'</span> が消えて）完璧なんですけど、
流石にこれは <span class="math">L</span> のままでは無理で、もう少し変形が必要です。
 
そこで、<span class="math">L</span> とは別の関数 <span class="math">H<span class="paren" style="font-size:em;">(</span>q, q', p<span class="paren" style="font-size:em;">)</span></span> を導入します。
適当な条件を付けて、運動方程式の形が簡単になる <span class="math">H</span> を求めたいわけですが、
まあ、まずは、
さっき「<span class="math">L</span> のままでは無理」と言った以下の条件を付けます。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
q
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
H
</div>
で、もう一つ、q' が邪魔なんだから
（元々 q と q' という2変数で表現できてたんだから、q と p の2つで十分なはず）、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
H
<span class="normal">=</span><span class="normal">0</span></div>
という条件も付けて、<span class="math">H</span> が <span class="math">q'</span> を明示的に含まないようにします。
まず、最初の条件から、
<span class="math">p</span> を含まないある関数 <span class="math">f<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></span> を用いて、
<div class="math">
H<span class="paren" style="font-size:em;">(</span>q, q', p<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
p q' <span class="normal">+</span> f<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div>
と書けることがわかります。
これを2つ目の条件に代入して、
<div class="math">
p
<span class="normal">+</span>
f<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span></div>
で、
<span class="math">
p
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></span>
なんだから、
結局、
<span class="math">f <span class="normal">=</span><span class="normal">−</span>L</span> になって、
<div class="math">
H<span class="paren" style="font-size:em;">(</span>q, p<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
p q' <span class="normal">−</span> L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div>
という結果が得られます。
（<span class="math">q'</span> は右辺中から消えるはず。）
で、さらに、これを両辺、<span class="math">q</span> で偏微分すれば、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
H
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
L
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
p
</div>
となります。


## <a id="sec-generated-title-3"></a> <a id="equation"></a>ハミルトンの運動方程式

まとめると、
<div class="math">
p
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>q, p<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
p q' <span class="normal">−</span> L<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></div>
と置くと、ラグランジュの運動方程式から、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
q
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
H
</div><div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
p
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
H
</div>
という微分方程式が得られます。
 
この微分方程式を、
ハミルトン形式の運動方程式、あるいは単に、
<strong id="equation" class="keyword">ハミルトンの運動方程式</strong>と呼びます。
（ハミルトンはアイルランドの物理学者。William Rowan Hamilton。）
 
ここで、
正規直交座標系を使った場合には
<span class="math">p <span class="normal">=</span> m<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x</span>、
すなわち、<span class="math">p</span> は運動量になります。
これとの類推で、
一般の座標系 <span class="math">q</span> に対して、
<span class="math">p</span> を<strong id="d22e365" class="keyword">一般化運動量</strong>と呼びます。
 
また、
<span class="math">H</span> を<strong id="hamiltonian" class="keyword">ハミルトニアン</strong>と呼びます。
正規直交座標系を使った場合には、
<span class="math">H <span class="normal">=</span> T <span class="normal">+</span> V</span> 
（運動エネルギーと位置エネルギーの和）になって、
系の全エネルギーを表す物理量になります。
一般の座標系を用いた場合でも、
次節で述べるように、
全エネルギーに相当する（保存則が成り立つ）物理量です。


### <a id="sec-generated-title-4"></a> <a id="merit"></a>ハミルトン形式の利点

ハミルトン形式は、
ラグランジュ形式と比べて形がシンプルです。
（「自励系」と呼ばれる、微分方程式の中でもかなりシンプルな部類に入る形になってる。）
 
まあ、式変形の結果得られただけのものなので、
ラグランジュ形式とハミルトン形式は本質的には同じものを表しています。
したがって、微分方程式が解析的に解けるのなら、
ラグランジュ形式で解こうがハミルトン形式で解こうが得られる解曲線は同じで、
2つの形式に大した違いはありません。
 
でも、微分方程式は解析的に解けない場合が多くて、
数値的に解いたり、定性的なことを調べるだけにする場合が多々あります。
例えば、具体的な解曲線の軌跡はわからなくても、
周期性を持つかどうかとか、発散するかどうかとかだけは調べられたりします。
そういう場合、
ラグランジュ形式よりも形がシンプルな分、ハミルトン形式の方が解析がしやすいという利点があります。


## <a id="sec-generated-title-5"></a> <a id="preserve"></a>一般の物理量とハミルトニアンの保存則

<span class="math">q<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, p<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> をハミルトンの運動方程式の解曲線として、
（<span class="math">t</span> を明示的に含まず、）
<span class="math">q, p</span> に依存する物理量
<span class="math">A<span class="paren" style="font-size:em;">(</span>q, p<span class="paren" style="font-size:em;">)</span></span>
を考えます。
これを時間微分すると、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
A
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
A
<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>q
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
A
<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>p
</div>
となるわけですが、
これにハミルトンの運動方程式を代入すると、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
A
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
A
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
H
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
A
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
H
</div>
となります。
ここで、以下のような記号を導入します。
<div class="math">
      <span class="paren" style="font-size:em;">{</span>f, g<span class="paren" style="font-size:em;">}</span>
      <span class="normal">=</span>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
f
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
g
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂p</td></tr></table>
f
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
g
</div>
この記号を、<strong id="poisson" class="keyword">ポアソン括弧</strong>（Poisson bracket）といいます。
ポアソン括弧を使うと、先ほどの式は
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>A
<span class="normal">=</span><span class="paren" style="font-size:em;">{</span>A, H<span class="paren" style="font-size:em;">}</span></div>
と書くことができます。


### <a id="sec-generated-title-6"></a> <a id="preserve"></a>ハミルトニアンの保存則

ハミルトニアン
<span class="math">H</span>
自身も <span class="math">q, p</span> に依存する物理量なわけで、
もし、<span class="math">t</span> を明示的に含まないなら、
<div class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>H
<span class="normal">=</span><span class="paren" style="font-size:em;">{</span>H, H<span class="paren" style="font-size:em;">}</span><span class="normal">=</span><span class="normal">0</span></div>
となります。
（2つの関数が同じものなとき、ポアソン括弧は 0 になります。）
すなわち、ハミルトニアン <span class="math">H</span> は時間的に不変な量（＝ 保存則が成り立つ）になります。


## <a id="sec-generated-title-7"></a> <a id="summary"></a>まとめ

* ラグランジュの運動方程式に対して、独立変数を増やす代わりに次数を下げて、<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x <span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>という、 定性的な解析や、数値計算がしやすい形式に変形。

* ラグランジュ形式から導出するので、任意の座標変数に対して同じ微分方程式が成り立つ。

* ただし、一般化運動量変数<span class="math">p</span>に関しては、いちいちラグランジュ形式に立ち返って計算しなおす必要あり。



## <a id="sec-generated-title-8"></a> <a id="plan"></a>執筆予定

正準変換
