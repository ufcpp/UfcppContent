---
title: "微分形式"
source_url: "https://ufcpp.net/study/math/manifold/difform/"
content_type: "Article"
published_at: "2015-05-06T14:18:30"
updated_at: "2015-05-06T14:18:30"
tags: []
umbraco_id: 1519
parent_id: 1515
sort_order: 3
aliases:
  - "/manifold/difform"
  - "/manifold/difform.html"
  - "/math/manifold/difform/"
  - "/study/manifold/difform"
  - "/study/manifold/difform.html"
---

# 微分形式

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

ようやく本題の微分形式の話に。


## <a id="sec-generated-title-2"></a> <a id="vector_analysis"></a>勾配、発散、回転 再考

「[数学](../index.md)」で説明した、
勾配、発散、回転と積分の関係式、
いわゆるガウスの定理やストークスの定理と呼ばれるものを改めて列挙してみましょう。
<div class="math">
φ<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span> － φ<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">∇</span>φ・<span class="normal">d</span><span class="vector">l</span></div><div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂S</td></tr></table>
      <span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="vector">∇</span>×<span class="vector">F</span><span class="normal">d</span><span class="vector">S</span></div><div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂V</td></tr></table>
      <span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table><span class="vector">∇</span>・<span class="vector">F</span><span class="normal">d</span>V</div>
第1式の
<span class="math">a, b</span> は曲線 <span class="math">C</span> の始点および終点なわけですが、
これを他の2式とあわせるために形式的に <span class="math">∂C</span> と書いて、
<span class="math">
φ<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span> － φ<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span></span>
も
<span class="math"><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂C</td></tr></table> φ
</span>
と書いてしまいましょう。
 
また、このページの説明にあわせて、
<span class="math">x, y, z</span> を使う代わりに
<span class="math">
 u<sup>1</sup> , 
 u<sup>2</sup> , 
 u<sup>3</sup> </span>
を使って、
<span class="math"><span class="normal">d</span><span class="vector">l</span>
＝
<span class="paren" style="font-size:em;">(</span><span class="normal">d</span>u<sup>1</sup> , <span class="normal">d</span>u<sup>2</sup> , <span class="normal">d</span>u<sup>3</sup> <span class="paren" style="font-size:em;">)</span></span>
などと表しましょう。
そして、
アインシュタインの記法や、
<span class="math">
∂u<sub>i</sub>
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u
    <sup>i</sup>
  </denom></td></tr></table>
</span>
という略記法なども使って上記の3式を書き直すと、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂C</td></tr></table> φ
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
∂u<sub>i</sub> φ <span class="normal">d</span>u<sup>i</sup></div><div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂S</td></tr></table> F<sub>i</sub><span class="normal">d</span>u<sup>i</sup>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="paren" style="font-size:em;">(</span>
 ∂u<sub>2</sub> F<sub>3</sub> － ∂u<sub>3</sub> F<sub>2</sub><span class="paren" style="font-size:em;">)</span><span class="normal">d</span>u<sup>2</sup><span class="normal">d</span>u<sup>3</sup>
＋
<span class="paren" style="font-size:em;">(</span>
 ∂u<sub>3</sub> F<sub>1</sub> － ∂u<sub>1</sub> F<sub>3</sub><span class="paren" style="font-size:em;">)</span><span class="normal">d</span>u<sup>3</sup><span class="normal">d</span>u<sup>1</sup>
＋
<span class="paren" style="font-size:em;">(</span>
 ∂u<sub>1</sub> F<sub>2</sub> － ∂u<sub>2</sub> F<sub>1</sub><span class="paren" style="font-size:em;">)</span><span class="normal">d</span>u<sup>1</sup><span class="normal">d</span>u<sup>2</sup></div><div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂V</td></tr></table>
F<sub>1</sub><span class="normal">d</span>u<sup>2</sup><span class="normal">d</span>u<sup>3</sup>
＋
F<sub>2</sub><span class="normal">d</span>u<sup>3</sup><span class="normal">d</span>u<sup>1</sup>
＋
F<sub>3</sub><span class="normal">d</span>u<sup>1</sup><span class="normal">d</span>u<sup>2</sup>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table>
∂u<sub>i</sub> F<sub>i</sub><span class="normal">d</span>u<sup>1</sup><span class="normal">d</span>u<sup>2</sup><span class="normal">d</span>u<sup>3</sup></div>
もう少しきれいな形になりそうでならなくてもどかしい感じです。
これをきれいな形で書けるようにするためには、
少し道具の整備が必要になります。


## <a id="sec-generated-title-3"></a> <a id="wedge"></a>ウェッジ積

ここで一旦少し話を変えて、
<span class="math"><span class="normal">d</span>u<sup>1</sup><span class="normal">d</span>u<sup>2</sup><span class="normal">d</span>u<sup>3</sup></span>
などの意味について考えてみましょう。
 
もし、<span class="math">x, y, z</span> が直交座標の場合、
その積 <span class="math">x y z</span> は幅 x、高さ y、奥行き z の直方体の体積になります。
したがって、<span class="math"><span class="normal">d</span>x <span class="normal">d</span>y <span class="normal">d</span>z</span> も微小な体積と考えることができます。
<span class="math"><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="normal">d</span>x <span class="normal">d</span>y <span class="normal">d</span>z</span> は微小な体積を寄せ集めて大きな図形の体積を計算するものです。
ただし、ここでは、負の体積も認めます。
幅 x が原点よりも右側に伸びている場合は正、
左側の場合は負だと考えます。
 
さて、直交座標系でない場合はどう考えましょう。
「[体積](../linear/determinant.md#volume)」で説明するように、
直方体の体積というのは、直方体の辺を成すベクトルに関して多重線形性と交代性を満たす関数です。
そこで、
<span class="math"><span class="normal">d</span>u<sup>1</sup>, <span class="normal">d</span>u<sup>2</sup>, <span class="normal">d</span>u<sup>3</sup></span>
に対して、多重線形性と交代性を満たす積、<span class="math">∧</span> を以下のように定義します。

* 多重線形性：<span class="math">
a <span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup>
＝
a <span class="paren" style="font-size:em;">(</span><span class="normal">d</span>u<sup>j</sup> ∧ <span class="normal">d</span>u<sup>i</sup><span class="paren" style="font-size:em;">)</span></span>、<span class="math"><span class="normal">d</span>u<sup>i</sup> ∧ b <span class="normal">d</span>u<sup>j</sup>
＝
b <span class="paren" style="font-size:em;">(</span><span class="normal">d</span>u<sup>j</sup> ∧ <span class="normal">d</span>u<sup>i</sup><span class="paren" style="font-size:em;">)</span></span>

* 交代性：<span class="math"><span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup>
＝
－ <span class="normal">d</span>u<sup>j</sup> ∧ <span class="normal">d</span>u<sup>i</sup></span>


内積の・、外積の×をそれぞれドット積、クロス積と呼ぶように、
<span class="math">∧</span> は<strong id="wedge" class="keyword">ウェッジ積</strong>（wedge product: wedge は楔の意味）と呼びます。
ちなみに、
交代性により、同じもの同士のウェッジ積は 0 になります。
（<span class="math"><span class="normal">d</span>x ∧ <span class="normal">d</span>x ＝ 0</span>）
 
さて、ここで、2つの座標
<span class="math">x, y</span> と <span class="math">u, v</span> の微分の間に、
<div class="math">
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>
            <span class="normal">d</span>u</td></tr><tr><td>
            <span class="normal">d</span>v</td></tr></table><span class="paren" style="font-size:3em;">]</span>
＝
A
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td><span class="normal">d</span>x</td></tr><tr><td><span class="normal">d</span>y</td></tr></table><span class="paren" style="font-size:3em;">]</span>
＝
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a</td><td>c</td></tr><tr><td>b</td><td>d</td></tr></table><span class="paren" style="font-size:3em;">]</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td><span class="normal">d</span>x</td></tr><tr><td><span class="normal">d</span>y</td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
という関係がある場合、
面積 <span class="math"><span class="normal">d</span>x <span class="normal">d</span>y</span> と <span class="math"><span class="normal">d</span>u <span class="normal">d</span>v</span> の間には、
<div class="math">
      <span class="normal">d</span>u <span class="normal">d</span>v
＝
<span class="normal">|</span>A<span class="normal">|</span><span class="normal">d</span>x <span class="normal">d</span>y
＝
<span class="paren" style="font-size:em;">(</span>a d － b c<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x <span class="normal">d</span>y
</div>
という関係があります。
実はこれも、ウェッジ積を使うことで簡単に表すことができます。
<div class="math">
      <span class="normal">d</span>u ∧ <span class="normal">d</span>v
＝
<span class="paren" style="font-size:em;">(</span>a <span class="normal">d</span>x ＋ b <span class="normal">d</span>y<span class="paren" style="font-size:em;">)</span>
∧
<span class="paren" style="font-size:em;">(</span>c <span class="normal">d</span>x ＋ d <span class="normal">d</span>y<span class="paren" style="font-size:em;">)</span></div><div class="math">
＝
a c <span class="normal">d</span>x ∧ <span class="normal">d</span>x
＋
a d <span class="normal">d</span>x ∧ <span class="normal">d</span>y
＋
b c <span class="normal">d</span>y ∧ <span class="normal">d</span>x
＋
b d <span class="normal">d</span>y ∧ <span class="normal">d</span>y
</div><div class="math">
＝
a d <span class="normal">d</span>x ∧ <span class="normal">d</span>y
＋
b c <span class="normal">d</span>y ∧ <span class="normal">d</span>x
＝
a d <span class="normal">d</span>x ∧ <span class="normal">d</span>y
－
b c <span class="normal">d</span>x ∧ <span class="normal">d</span>y
</div><div class="math">
＝
<span class="paren" style="font-size:em;">(</span>a d － b c<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x ∧ <span class="normal">d</span>y
</div>
ウェッジ積を使うことによって、
微小な面積に関する公式が機械的な計算で求まるわけです。
これは、2次元の場合だけでなく、高次元の場合でも成り立ちます。
（
行列式もウェッジ積も、体積というものの持つ多重線形交代性に着目して定義されるものなので、
ウェッジ積を使った計算の結果に行列式が現れるのは不思議なことではありません。
）


## <a id="sec-generated-title-4"></a> <a id="dif_int"></a>微分したものを積分

発散、回転、勾配の話に戻りましょう。
「[勾配、発散、回転 再考](#vector_analysis)」で列挙した式、
すなわち、ガウスの定理やストークスの定理は、
基本的には微分積分学の基本定理、
すなわち、
（連続微分可能な関数に対して）
微分したものを積分すると元に戻るという事実から導き出される定理です。
 
説明を簡単化するために、
積分範囲を
<span class="math">
V
＝
<span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>×
<span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span>×
<span class="paren" style="font-size:em;">(</span>e, f<span class="paren" style="font-size:em;">)</span></span>
という直方体に限定して説明すると、
例えば、ガウスの定理は、
（座標を <span class="math">x, y, z</span>、<span class="math"><span class="vector">F</span> ＝ <span class="paren" style="font-size:em;">(</span>F, G, H<span class="paren" style="font-size:em;">)</span></span> として）
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table>
      <span class="vector">∇</span>・
      <span class="vector">F</span>
      <span class="normal">d</span>V
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal">d</span>x
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> d</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">c</td></tr></table><span class="normal">d</span>y
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> f</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">e</td></tr></table><span class="normal">d</span>z
<span class="paren" style="font-size:em;">(</span>
∂x F
＋
∂y G
＋
∂x H
<span class="paren" style="font-size:em;">)</span></div><div class="math">
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> d</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">c</td></tr></table><span class="normal">d</span>y
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> f</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">e</td></tr></table><span class="normal">d</span>z
<span class="paren" style="font-size:2em;">(</span>
 F<span class="paren" style="font-size:em;">(</span>b, y, z<span class="paren" style="font-size:em;">)</span> － F<span class="paren" style="font-size:em;">(</span>a, y, z<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span></div><div class="math">
＋
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> f</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">e</td></tr></table><span class="normal">d</span>z
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal">d</span>x
<span class="paren" style="font-size:2em;">(</span>
 G<span class="paren" style="font-size:em;">(</span>x, d, z<span class="paren" style="font-size:em;">)</span> － G<span class="paren" style="font-size:em;">(</span>x, c, z<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span></div><div class="math">
＋
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal">d</span>x
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> d</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">c</td></tr></table><span class="normal">d</span>y
<span class="paren" style="font-size:2em;">(</span>
 H<span class="paren" style="font-size:em;">(</span>x, y, f<span class="paren" style="font-size:em;">)</span> － H<span class="paren" style="font-size:em;">(</span>x, y, e<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span></div><div class="math">
＝
<span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂V</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span></div>
となることから得られる定理です。
微分したものを積分すると元に戻るというのは、
要するに、
<div class="math">
φ<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span> － φ<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
∂x
φ
<span class="normal">d</span>x
</div>
あるいは、不定積分で表すなら、
<span class="math">C</span> を積分定数として、
<div class="math">
φ<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － C
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
∂x
φ
<span class="normal">d</span>x
</div>
ということです。
多次元の場合には、いわゆる全微分公式というものになって、
以下のように表されます。
<div class="math">
φ<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span> － C
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
∂u<sub>i</sub>
φ
<span class="normal">d</span>u<sup>i</sup>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="normal">d</span>φ
</div>
全微分
<span class="math"><span class="normal">d</span>φ
＝
∂u<sub>i</sub>
φ
<span class="normal">d</span>u<sup>i</sup></span>
も「微分して積分すると元に戻る」っていう発想の一種なわけです。
ガウスの定理やストークスの定理も、
「微分して積分」の一種なので、
全微分を拡張した何らかの微分操作で表現できないでしょうか。
そういう発想のもと考えられたのが次節で説明する外微分です。


## <a id="sec-generated-title-5"></a> <a id="outer_dif"></a>外微分

さて、それでは<strong id="outer_dif" class="keyword">外微分</strong>（outer differentiation）を定義していきましょう。
 
まず、普通の関数 <span class="math">f</span> に対しては、外微分 ＝ 全微分
<span class="math"><span class="normal">d</span>f
＝
∂u<sub>i</sub>
f
<span class="normal">d</span>u<sup>i</sup></span>
とします。
 
次に、1階微分
<span class="math">f<sub>i</sub><span class="normal">d</span>u<sup>i</sup></span>
に関しては、
<div class="math">
      <span class="normal">d</span>
      <span class="paren" style="font-size:em;">(</span>f<sub>i</sub><span class="normal">d</span>u<sup>i</sup><span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">d</span>f
∧
<span class="normal">d</span>u<sup>i</sup>
＝
∂u<sub>j</sub>
f<sub>i</sub><span class="normal">d</span>u<sup>j</sup>
∧
<span class="normal">d</span>u<sup>i</sup></div>
で外微分を定義します。
同様に、
2階微分
<span class="math">f<sub>i j</sub><span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup></span>
に対しても、
<div class="math">
      <span class="normal">d</span>
      <span class="paren" style="font-size:em;">(</span>f<sub>i j</sub><span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup><span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">d</span>f
∧
<span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup>
＝
∂u<sub>k</sub>
f<sub>i j</sub><span class="normal">d</span>u<sup>k</sup>
∧
<span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup></div>
としていきます。
 
機械的に書くと、
何階でも外微分できそうな感じがしますが、
ウェッジ積の交代性（同じ文字同士のウェッジ積は <span class="math"><span class="normal">d</span>x ∧ <span class="normal">d</span>x ＝ 0</span>）
があるので、
n 次元ならば n 階しか外微分できません。
 
イメージをつかんでいただくために、
3次元で、座標 <span class="math">x, y, z</span> を使った場合を具体的に計算してみましょう。
<div class="math">
      <span class="normal">d</span>f ＝ ∂x f <span class="normal">d</span>x ＋∂y f <span class="normal">d</span>y ＋∂z f <span class="normal">d</span>z
</div><div class="math">
      <span class="normal">d</span>
      <span class="paren" style="font-size:em;">(</span>f<span class="normal">d</span>x ＋ g<span class="normal">d</span>y ＋ h<span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:em;">(</span>∂x f <span class="normal">d</span>x ＋∂y f <span class="normal">d</span>y ＋∂z f <span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x
</div><div class="math">
　　＋
<span class="paren" style="font-size:em;">(</span>∂x g <span class="normal">d</span>x ＋∂y g <span class="normal">d</span>y ＋∂z g <span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>y
</div><div class="math">
　　＋
<span class="paren" style="font-size:em;">(</span>∂x h <span class="normal">d</span>x ＋∂y h <span class="normal">d</span>y ＋∂z h <span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z
</div><div class="math">
　＝
∂y f <span class="normal">d</span>y<span class="normal">d</span>x ＋∂z f <span class="normal">d</span>z<span class="normal">d</span>x
＋
∂z g <span class="normal">d</span>z<span class="normal">d</span>y ＋∂x g <span class="normal">d</span>x<span class="normal">d</span>y
＋
∂x h <span class="normal">d</span>x<span class="normal">d</span>z ＋∂y h <span class="normal">d</span>y<span class="normal">d</span>z
</div><div class="math">
　＝
<span class="paren" style="font-size:em;">(</span>∂y h － ∂z g<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>y<span class="normal">d</span>z
＋
<span class="paren" style="font-size:em;">(</span>∂z f － ∂x h<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z<span class="normal">d</span>x
＋
<span class="paren" style="font-size:em;">(</span>∂x g － ∂y f<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x<span class="normal">d</span>y
</div><div class="math">
      <span class="normal">d</span>
      <span class="paren" style="font-size:em;">(</span>f<span class="normal">d</span>y<span class="normal">d</span>x ＋ g<span class="normal">d</span>z<span class="normal">d</span>x ＋ h<span class="normal">d</span>x<span class="normal">d</span>y<span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
　＝
<span class="paren" style="font-size:em;">(</span>∂x f <span class="normal">d</span>x ＋∂y f <span class="normal">d</span>y ＋∂z f <span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>y<span class="normal">d</span>x
</div><div class="math">
　　＋
<span class="paren" style="font-size:em;">(</span>∂x g <span class="normal">d</span>x ＋∂y g <span class="normal">d</span>y ＋∂z g <span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z<span class="normal">d</span>x
</div><div class="math">
　　＋
<span class="paren" style="font-size:em;">(</span>∂x h <span class="normal">d</span>x ＋∂y h <span class="normal">d</span>y ＋∂z h <span class="normal">d</span>z<span class="paren" style="font-size:em;">)</span> h<span class="normal">d</span>x<span class="normal">d</span>y
</div><div class="math">
　＝
<span class="paren" style="font-size:em;">(</span>∂x f ＋∂y g ＋∂z gh<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x<span class="normal">d</span>y<span class="normal">d</span>z
</div>
1つ目はいわゆる全微分なわけですが、
2つ目、3つ目の式も、
よく見てみると回転と発散になっていることが分かるでしょうか。
 
発想としては、
ガウスの定理もストークスの定理も、「微分して積分」で説明が付く
→
「微分して積分」は全微分に相当する
→
全微分を拡張した外微分というものを定義する
→
ガウスの定理もストークスの定理も外微分で表現できる、
といった感じになります。


## <a id="sec-generated-title-6"></a> <a id="stokes"></a>ストークスの定理

外微分の計算過程に回転や発散の式が現れることが分かりました。
だいぶ遠回りをしましたが、
ようやくガウスの定理やストークスの定理を簡潔に表すことができそうです。

<span class="math">
χ ＝ F<span class="normal">d</span>x ＋ G<span class="normal">d</span>y ＋ H<span class="normal">d</span>z
</span>
、
<span class="math">
ψ ＝ F<span class="normal">d</span>y∧<span class="normal">d</span>z ＋ G<span class="normal">d</span>z∧<span class="normal">d</span>x ＋ H<span class="normal">d</span>x∧<span class="normal">d</span>y
</span>
とおくと、
ストークスの定理、ガウスの定理はそれぞれ、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂S</td></tr></table> χ
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="normal">d</span>χ
</div><div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂V</td></tr></table> ψ
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table><span class="normal">d</span>ψ
</div>
となります。
見ての通り、まったく同じ形をしています。
全微分公式も形式的に、
曲線 <span class="math">C</span> の始点・終点 <span class="math">a, b</span> を <span class="math">∂C</span>、
<span class="math"><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂C</td></tr></table> φ ＝ φ<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span> － φ<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span></span>
と書くことで、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂C</td></tr></table> φ
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="normal">d</span>φ
</div>
と表現するなら同じ形になります。
外微分という道具を用意することで、
勾配も発散も回転もまったく同じ形式で書くことができるようになったわけです。
 
微分形式の分野では、
これらを合わせてストークスの定理と呼びます。
これは、ベクトル解析のストークスの定理の自然な拡張になっています。


## <a id="sec-generated-title-7"></a> <a id="form"></a>微分形式

これだけ説明すれば、
微分形式ももはや「形式的に定義した、よく分からないけどつじつまの合う道具」ではなく、
具体的なイメージを伴うものになると思います。
それでは、ほんとうに前置きが長くなりましたが、
<strong id="difform" class="keyword">微分形式</strong>（differential form）というのは以下のようにして定義されるものです。

* 通常の関数<span class="math">f</span>を微分 0 形式と呼ぶ。

* 反変ベクトル<span class="math">a<sub>i</sub><span class="normal">d</span>u<sup>i</sup></span>を微分 1 形式と呼ぶ。

* ウェッジ積を用いて、<span class="math">a<sub>i j</sub><span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup></span>というようにして作ったものを微分 2 形式、<span class="math">a<sub>i j k</sub><span class="normal">d</span>u<sup>i</sup> ∧ <span class="normal">d</span>u<sup>j</sup> ∧ <span class="normal">d</span>u<sup>k</sup></span>というようなのを微分 3 形式と呼ぶ。 以下、逐次、微分 k 形式も定義できる。


<span class="math">
        <span class="normal">d</span>u<sup>i</sup></span>
などは、
「反変ベクトルの基底を表す抽象的な記号」と言ってしまってもいいんですが、
ウェッジ積導入の動機などを思い出していただけば、
これが線素、面積素、体積素（要するに、いわゆる微小差分）を自然に拡張したもの
であることが分かると思います。
 
一見すると、n 変数の微分 k 形式は <span class="math">n<sup>k</sup></span> 次元のベクトルになるように見えますが、
実際には、ウェッジ積の交代性から、
微分 0～n 形式までしか定義できず、
<span class="math"><sub>n</sub><span class="normal">C</span><sub>k</sub></span> 次元
（C は組み合わせの数）のベクトルになります。
 
例えば、3次元で直交座標 <span class="math">x, y, z</span> を使う場合、

* 微分 0 形式： 通常の関数<span class="math">f</span>

* 微分 1 形式：<span class="math">f <span class="normal">d</span>x ＋ g <span class="normal">d</span>y ＋ h <span class="normal">d</span>z</span>

* 微分 2 形式：<span class="math">f <span class="normal">d</span>y∧<span class="normal">d</span>z ＋ g <span class="normal">d</span>z∧<span class="normal">d</span>x ＋ h <span class="normal">d</span>x∧<span class="normal">d</span>y</span>

* 微分 3 形式：<span class="math">f <span class="normal">d</span>x∧<span class="normal">d</span>y∧<span class="normal">d</span>z</span>


ベクトル解析では、0 形式と 3 形式、1 形式と 2 形式の区別が付きませんでしたが、
実は別物です。
3 形式は、ベクトル解析的な書き方では
（<span class="math"><span class="normal">d</span>x∧<span class="normal">d</span>y∧<span class="normal">d</span>z</span> を明示しないので）
見た目ではスカラーと区別がつきませんが、
座標変換に対して不変ではないので、擬スカラーなどと呼ばれます。
1 形式と 2 形式はどちらも3次元ベクトルではあるんですが、
これも座標変換のルールが違うので、区別するためにそれぞれ、
極性ベクトルと軸性ベクトルと呼ばれたりします。
```text
執筆予定

- 外微分
- 座標変換
- 微分形式の積分
```
