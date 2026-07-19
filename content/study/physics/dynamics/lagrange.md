---
title: "ラグランジュ形式"
source_url: "https://ufcpp.net/study/physics/dynamics/lagrange/"
content_type: "Article"
published_at: "2007-04-08T00:00:00"
updated_at: "2007-05-01T00:00:00"
tags: []
umbraco_id: 1558
parent_id: 1554
sort_order: 3
aliases:
  - "/dynamics/lagrange"
  - "/dynamics/lagrange.html"
  - "/physics/dynamics/lagrange/"
  - "/study/dynamics/lagrange"
  - "/study/dynamics/lagrange.html"
---

# ラグランジュ形式

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[最小作用の原理](action.md#minaction)」を出発点として、
座標系によらない力学法則を導出することができます。


## <a id="sec-generated-title-2"></a> <a id="equation"></a>ラグランジュの運動方程式

「[最小作用の原理](action.md)」では、「物体は労力的にみて最短な経路を通ろうとする」という話をしました。
<span class="math">
L
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>
m
<span class="paren" style="font-size:1.5em;">(</span>
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="paren" style="font-size:1.5em;">)</span><span class="normal">−</span>
V<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span>
と置いて、
<span class="math">
I
<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>x, y, x', y'<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</span>
が物体の移動に掛かる労力（＝ 作用）で、
これを最小にするような経路を求めることで、
物体の運動の軌跡が分かります
（「[最小作用の原理](action.md#minaction)」）。

「[変分学](variation.md)」の知識を使って、
この作用 <span class="math">I</span> の変分問題から以下のような微分方程式が得られます。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x'</denom></td></tr></table>
L
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>
L
<span class="normal">=</span><span class="normal">0</span></div>
これをラグランジュ形式の運動方程式、
あるいは単に、<strong id="equation" class="keyword">ラグランジュの運動方程式</strong>といいます。
（ラグランジュはフランスの物理学者。Joseph Louis Lagrange。）
 
ちなみに、物理の分野では、ここで出てきた <span class="math">L</span> を<strong id="lagrangean" class="keyword">ラグランジアン</strong>（Lagrangian）と呼びます。
分野によってラグランジアンという言葉の意味・ニュアンスが違ったりするみたいで、
以下のような2つの使い方をするようなんですが、
物理学では通常、1. の方を使います。

1. 力学： 運動エネルギー<span class="math">T</span>と位置エネルギー<span class="math">V</span>の差<span class="math">L <span class="normal">=</span> T <span class="normal">−</span> V</span>を（1つの物理量とみなして）ラグランジアンと呼ぶ。

2. 変分学： 汎関数<span class="math">I<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>が、 積分形で<span class="math">
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
L
<span class="paren" style="font-size:1.2em;">(</span>
 x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">2</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 <span class="normal">⋯</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">d</span>t
</span>と表されるとき、<span class="math">I</span>をラグランジアン、<span class="math">L</span>をラグランジアン密度と呼ぶ。


「ラグランジュの作用積分」、「ラグランジュの作用密度」とかいう言い方すれば、
意味がはっきりするんですけどね。


## <a id="sec-generated-title-3"></a> <a id="coordinate"></a>座標に依存しない力学

ラグランジュ形式の運動方程式は、
元々が作用
<span class="math">
I
<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>x, x'<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</span>
の変分問題なので、
変数変換で式の形が変わりません。
 
これはどういうことかというと、
座標変数 <span class="math">x</span> を別の変数 <span class="math">q</span> に変換するなら、
<span class="math">
L<sub>q</sub><span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
L<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span>, x'<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span></span>
とでもおいて、
作用は
<div class="math">
I
<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
L<span class="paren" style="font-size:em;">(</span>x, x'<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
<span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
L<sub>q</sub><span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div>
と書き表されます。
この変分問題を微分方程式に直すなら、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L<sub>q</sub><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
L<sub>q</sub><span class="normal">=</span><span class="normal">0</span></div>
という式になって、見ての通り、座標変数が <span class="math">x</span> のときと全く同じ式になります。
（
もちろん、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table></span>
とか
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table></span>
とかの座標変換法則から、
微分方程式の方をがんばって式変形しても同様の結論に至ります。
）
 
どんな座標系を使っても同じ式になるということで、
直交座標系のイメージの強い <span class="math">x</span> の文字は避けて、
変数 <span class="math">q</span> を使って運動方程式を記述します。
 
現代科学的には、
「自然の法則は座標系の取り方には依存しない」という信念みたいなものがあって、
その信念に基づけば、ニュートンの運動方程式よりも、
変数（座標系）の取り方に依存しないラグランジュの運動方程式の方が自然の本質に近づいた式といえます。


## <a id="sec-generated-title-4"></a> <a id="vector"></a>ベクトルポテンシャル

位置エネルギー <span class="math">V</span> の部分が <span class="math">q'</span> にも依存する場合についても考えてみます。
例えば、
保存場（スカラーポテンシャル <span class="math">U</span> のみを持つ場）じゃなくて
ベクトルポテンシャル <span class="math"><span class="vector">A</span></span> がある場合、
物体に働く力 <span class="math">f</span> は、
<div class="math">
f
<span class="normal">=</span><span class="normal">−</span><span class="normal">∇</span>U
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">A</span><span class="normal">+</span>
v<span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">∇</span><span class="normal">×</span><span class="vector">A</span><span class="paren" style="font-size:em;">)</span></div>
になります。
<span class="math">
L
<span class="normal">=</span>
T
<span class="normal">−</span>
V
<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span></span>
から導出される力 <span class="math">f</span> が上式を満たすようにしたければ、
<span class="math">
V
<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
U<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">+</span><span class="vector">A</span><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">⋅</span>
q'
</span>
とすればいいことが、
（頑張って計算してみれば）分かります。
要するに、
ベクトルポテンシャルが存在する場合のラグランジアンは
<div class="math">
L
<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
T<span class="paren" style="font-size:em;">(</span>q'<span class="paren" style="font-size:em;">)</span><span class="normal">−</span>
U<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">−</span><span class="vector">A</span><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">⋅</span>
q'
</div>
という形になります。
（
ちなみに、
この形式のラグランジアンや、
ベクトルポテンシャルの物理的な意味に関しては、
「[計量とポテンシャル](potential.md)」を参照。）
 
電磁場の場合は、電荷が運動するだけで磁場というベクトルポテンシャル持った場が生じます。
では、重力場の場合はどうかというと、
座標の方が動いたりする（例えば、自分の乗ってる乗り物が加速する）と、
慣性力とかコリオリの力が生じるますが、
これをベクトルポテンシャルによって生じる力だと考えて式を立てることができます。


## <a id="sec-generated-title-5"></a> <a id="summary"></a>まとめ

運動エネルギーを <span class="math">T<span class="paren" style="font-size:em;">(</span>q'<span class="paren" style="font-size:em;">)</span></span>、
スカラーポテンシャルを <span class="math">U<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></span>、
ベクトルポテンシャルを <span class="math"><span class="vector">A</span><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span></span> として、
<div class="math">
L
<span class="paren" style="font-size:em;">(</span>q, q'<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
T<span class="paren" style="font-size:em;">(</span>q'<span class="paren" style="font-size:em;">)</span><span class="normal">−</span>
U<span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">−</span><span class="vector">A</span><span class="paren" style="font-size:em;">(</span>q<span class="paren" style="font-size:em;">)</span><span class="normal">⋅</span>
q'
</div>
と置いて、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>q'</denom></td></tr></table>
L
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂q</td></tr></table>
L
<span class="normal">=</span><span class="normal">0</span></div>
が物体の運動を記述する方程式。

* 「[最小作用の原理](action.md#minaction)」から導出。

* 元が作用積分の変分問題なので、座標変数の取り方によらず式の形が同じ。

* 現代科学的には、「自然法則は座標系の取り方に依存しない」という信念みたいなものがある。
