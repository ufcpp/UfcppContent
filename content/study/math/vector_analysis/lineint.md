---
title: "線積分"
source_url: "https://ufcpp.net/study/math/vector_analysis/lineint/"
content_type: "Article"
published_at: "2015-05-06T14:17:36"
updated_at: "2015-05-06T14:17:36"
tags: []
umbraco_id: 1493
parent_id: 1491
sort_order: 1
aliases:
  - "/math/vector_analysis/lineint/"
  - "/study/vector_analysis/lineint"
  - "/study/vector_analysis/lineint.html"
  - "/vector_analysis/lineint"
  - "/vector_analysis/lineint.html"
---

# 線積分

## <a id="sec-generated-title-1"></a> <a id="lineint"></a>線積分とは

<figure>
	[![線積分](../../../../assets/media/ufcpp2000/math/lineint1.png)](../../../../assets/media/ufcpp2000/math/lineint1.png)
	<figcaption>線積分</figcaption>
</figure>


ある経路<span class="math">C</span>上で定義されるスカラー場<span class="math">F</span>に対し、
<div class="math">
        <em>
          <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>F <span class="normal">d</span>l
        </em>
      </div>
を<span class="math">F</span>の<span class="math">C</span>上での<strong id="lineint" class="keyword">線積分</strong>といいます。
ここで、<span class="math">
        <span class="normal">d</span>l
      </span>は<span class="math">C</span>上の微小線素です(線素を<span class="math">
        <span class="normal">d</span>s
      </span>と書く人も多いです(stringのs？))。
経路<span class="math">C</span>が閉路の場合、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>F <span class="normal">d</span>l
    </div>
と書きます。


## <a id="sec-generated-title-2"></a> <a id="vector"></a>ベクトル場の線積分

ベクトル場<span class="math">
        <span class="vector">F</span>
      </span>を考えます。
経路<span class="math">C</span>の単位接線ベクトルを<span class="math">
        <span class="vector">t</span>
      </span>とすると
<span class="math">
        <span class="vector">F</span>・<span class="vector">t</span>
      </span>はスカラー場となります。
これを<span class="math">F</span>とおいて線積分すると、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>F <span class="normal">d</span>l = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">F</span>・<span class="vector">t</span><span class="normal">d</span>l
    </div>
となります。
ここで、<span class="math">
        <span class="normal">d</span><span class="vector">l</span> = <span class="vector">t</span><span class="normal">d</span>l
      </span>とおくと上の式は
<div class="math">
      <em>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span>
      </em>
    </div>
となります。
この<span class="math">
        <span class="normal">d</span><span class="vector">l</span>
      </span>を線素ベクトルといいます。

物理学ではこのようにベクトル場の接線方向成分のみが問題となる場合が多いので、
<span class="math">
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span>
      </span>という形の式を、
単にベクトル場<span class="math">
        <span class="vector">F</span>
      </span>の曲線<span class="math">C</span>上での線積分ということもあります。


## <a id="sec-generated-title-3"></a> <a id="cartesian"></a>線積分の直交座標系での表現

<span class="math">
        <span class="normal">d</span><span class="vector">l</span>
      </span>は接線方向を向き、微小線分の長さと同じ絶対値を持つベクトルですから、
直行座標を用いてあらわすと
<div class="math">
      <span class="normal">d</span><span class="vector">l</span> = <span class="paren" style="font-size:em;">(</span>
        <span class="normal">d</span>x,<span class="normal">d</span>y,<span class="normal">d</span>z
      <span class="paren" style="font-size:em;">)</span>
    </div>
となります。
よって、<span class="math">
        <span class="vector">F</span>=<span class="paren" style="font-size:em;">(</span>
          F<sub>x</sub>,F<sub>y</sub>,F<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>とおくと、
線積分は
<div class="math">
      <em>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="paren" style="font-size:em;">(</span>
          F<sub>x</sub><span class="normal">d</span>x+F<sub>y</sub><span class="normal">d</span>y+F<sub>z</sub><span class="normal">d</span>z
        <span class="paren" style="font-size:em;">)</span>
      </em>
    </div>
となります。


## <a id="sec-generated-title-4"></a> <a id="root"></a>積分経路

線積分は一般には始点<span class="math">P</span>と終点<span class="math">Q</span>が同じでも経路が異なればまったく異なる値となります。
しかし、詳しくは「[回転とは](rotation.md#rot)」で説明しますが、<em>
        <span class="math">
          <span class="vector">F</span>
        </span>が<span class="math">
          <span class="vector">∇</span>×<span class="vector">F</span> = 0
        </span>を満たすときには線積分の値は始点と終点が同じならば経路によらず常に一定の値を持ちます。
      </em>
このとき、
<div class="math">
      φ(Q) − φ(P) = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span>
    </div>
という関係を満たすスカラー場<span class="math">φ</span>が存在します。
