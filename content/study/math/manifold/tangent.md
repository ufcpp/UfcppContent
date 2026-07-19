---
title: "接ベクトル"
source_url: "https://ufcpp.net/study/math/manifold/tangent/"
content_type: "Article"
published_at: "2015-05-06T14:18:28"
updated_at: "2015-05-06T14:18:28"
tags: []
umbraco_id: 1518
parent_id: 1515
sort_order: 2
aliases:
  - "/manifold/tangent"
  - "/manifold/tangent.html"
  - "/math/manifold/tangent/"
  - "/study/manifold/tangent"
  - "/study/manifold/tangent.html"
---

# 接ベクトル

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

共変ベクトル ＝ 接ベクトルの話を。


## <a id="sec-generated-title-2"></a> <a id="tangent"></a>接ベクトル

本題の微分形式に入る前に、
先に
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table></span>
に関する話をしましょう。
前節で、共変ベクトル
<span class="math">a<sup>i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table></span>
は接ベクトルとも呼ばれるという話をしました。
ここではその意味について説明します。

<span class="math">
        <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
            u<sup>i</sup>
          </denom></td></tr></table>
      </span>
は書くのが大変なので、
<span class="math">∂u<sub>i</sub></span>
と略記。
添え字が下付きになっていることに注意。


## <a id="sec-generated-title-3"></a> <a id="plan"></a>執筆予定

<pre>
・接ベクトル

まず、3次元空間上の曲面で説明
x ＝ (x(s, t), y(s, t), z(s, t))
と表される曲面に対して、
∂x/∂s と ∂x/∂t はこの曲面の接線の1つになる。

その線形結合
a (∂x/∂s) ＋ b (∂x/∂t)
も接平面上のベクトルになる。

なので、いっそのこと、
a(∂/∂s) ＋ b(∂/∂t)
という微分演算子を接ベクトルと呼んでしまう。

一般に、N 次元多様体 x を考える場合でも、
座標 u に対して、微分演算子の線形結合

f<sup>i</sup>(∂/∂u<sup>i</sup>)

を接ベクトルと呼ぶ。

f<sup>i</sup> は u の関数ね。
x の各点 u に対して1つのベクトル f<sup>i</sup> が定まる。
位置に関する関数ということで、これを接ベクトル場と呼んだりする。
あるいは、単に「ベクトル場」というと接ベクトル場のこと。

接ベクトル場は、要するに ∂/∂u<sup>i</sup> を基底とするベクトル空間になるわけだけど、
∂/∂u<sup>i</sup> の座標変換規則を考えると、接ベクトル場には

f<sup>i</sup>∂/∂u<sup>i</sup>
＝
f<sup>i</sup>(∂v<sup>j</sup>/∂u<sup>i</sup>)∂/∂v<sup>j</sup>

という座標変換規則がある。

- ベクトル場と積分曲線

V(u) ＝ V<sup>i</sup>(u) (∂/∂u<sup>i</sup>) を接ベクトル場として、
du/dt ＝ V(u)
という微分方程式の解を V の積分曲線という。

この微分方程式の解によって、ベクトル場 V に沿った曲線が描かれる。

運動方程式とか、多くの微分方程式がこの形に帰着される。

v ＝ φ(u) のとき、
du/dt ＝ V<sup>i</sup>∂/∂u<sup>i</sup>
の積分曲線を u(t) とすると、
dv/dt ＝ V<sup>i</sup>(∂v<sup>j</sup>/∂u<sup>i</sup>)∂/∂v<sup>j</sup>
の積分曲線を v(t) で、
v(t) ＝ φ(u(t)) となるものが必ず存在する。
</pre>
