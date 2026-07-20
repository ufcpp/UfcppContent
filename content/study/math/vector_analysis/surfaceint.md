---
title: "面積分"
source_url: "https://ufcpp.net/study/math/vector_analysis/surfaceint/"
content_type: "Article"
published_at: "2015-05-06T14:17:38"
updated_at: "2015-05-06T14:17:38"
tags: []
umbraco_id: 1494
parent_id: 1491
sort_order: 2
aliases:
  - "/math/vector_analysis/surfaceint/"
  - "/study/vector_analysis/surfaceint"
  - "/study/vector_analysis/surfaceint.html"
  - "/vector_analysis/surfaceint"
  - "/vector_analysis/surfaceint.html"
---

# 面積分

## <a id="sec-generated-title-1"></a> <a id="surfaceint"></a>面積分とは

ある曲面<span class="math">S</span>上で定義されるスカラー場<span class="math">F</span>に対し、
<div class="math">
      <em>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>F<span class="normal">d</span>S
      </em>
    </div>
を<span class="math">F</span>の<span class="math">S</span>上での<strong id="surfaceint" class="keyword">面積分</strong>といいます。
ここで、<span class="math">
        <span class="normal">d</span><span class="vector">S</span>
      </span>は<span class="math">S</span>上の微小面積素です。
曲面<span class="math">S</span>が閉曲面の場合、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>F<span class="normal">d</span>S
    </div>
と書きます。


## <a id="sec-generated-title-2"></a> <a id="vector"></a>ベクトル場の面積分

ベクトル場<span class="math">
        <span class="vector">F</span>
      </span>を考えます。
曲面<span class="math">S</span>の単位法線ベクトルを<span class="math">
        <span class="vector">n</span>
      </span>とすると
<span class="math">
        <span class="vector">F</span>・<span class="vector">n</span>
      </span>はスカラー場となります。
これを<span class="math">F</span>とおいて面積分すると、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>F<span class="normal">d</span>S = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="vector">F</span>・<span class="vector">n</span><span class="normal">d</span>S
    </div>
となります。
ここで、<span class="math">
        <span class="normal">d</span><span class="vector">S</span> = <span class="vector">n</span><span class="normal">d</span>S
      </span>とおくと上の式は
<div class="math">
      <em>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span>
      </em>
    </div>
となります。
この<span class="math">
        <span class="normal">d</span><span class="vector">S</span>
      </span>を面積素ベクトルといいます。

物理学ではこのように曲面を貫くベクトル場の法線方向成分のみが問題となる場合が多いので、
<span class="math">
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span>
      </span>という形の式を、
単にベクトル場<span class="math">
        <span class="vector">F</span>
      </span>の曲面<span class="math">S</span>上での面積分ということもあります。


## <a id="sec-generated-title-3"></a> <a id="cartesian"></a>面積分の直交座標系での表現

曲面<span class="math">S</span>上の点<span class="math">
        <span class="vector">r</span>
      </span>は媒介変数を持ちいて<span class="math">
        <span class="vector">r</span><span class="paren" style="font-size:em;">(</span>u,v<span class="paren" style="font-size:em;">)</span>=<span class="paren" style="font-size:em;">(</span>
          x<span class="paren" style="font-size:em;">(</span>u,v<span class="paren" style="font-size:em;">)</span>, y<span class="paren" style="font-size:em;">(</span>u,v<span class="paren" style="font-size:em;">)</span>, z<span class="paren" style="font-size:em;">(</span>u,v<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>と言う風に表されます。ここで、<span class="math">u</span>がごく小さい値<span class="math">
        <span class="normal">d</span>u
      </span>だけ変化したとき、<span class="math">
        <span class="vector">r</span>
      </span>は
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          ∂<span class="vector">r</span>
        </td></tr><tr><td>∂u</td></tr></table>
      <span class="normal">d</span>u
    </div>
だけ変化します。同様に<span class="math">v</span>が<span class="math">
        <span class="normal">d</span>v
      </span>だけ変化したとき、<span class="math">
        <span class="vector">r</span>
      </span>は
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          ∂<span class="vector">r</span>
        </td></tr><tr><td>∂v</td></tr></table>
      <span class="normal">d</span>v
    </div>
だけ変化します。
このとき、<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂u</td></tr></table><span class="normal">d</span>u
      </span>および<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂v</td></tr></table><span class="normal">d</span>v
      </span>は<span class="math">S</span>の接ベクトルになっています。

図1のように<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂u</td></tr></table><span class="normal">d</span>u
      </span>および<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂v</td></tr></table><span class="normal">d</span>v
      </span>は<span class="math">S</span>と<span class="math">
        <span class="normal">d</span>S, <span class="vector">n</span>
      </span>の間には
<div class="math">
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂u</td></tr></table>
        <span class="normal">d</span>u
      <span class="paren" style="font-size:1.5em;">)</span>×<span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂v</td></tr></table><span class="normal">d</span>v
      <span class="paren" style="font-size:1.5em;">)</span> = <span class="vector">n</span><span class="normal">d</span>S
    </div>
という関係があります。よって<span class="math">
        <span class="normal">d</span><span class="vector">S</span>
      </span>は、
<div class="math">
      <span class="normal">d</span><span class="vector">S</span> = <table class="frac" summary="fraction"><tr><td class="num">
          ∂<span class="vector">r</span>
        </td></tr><tr><td>∂u</td></tr></table>×<table class="frac" summary="fraction"><tr><td class="num">
          ∂<span class="vector">r</span>
        </td></tr><tr><td>∂v</td></tr></table><span class="normal">d</span>u<span class="normal">d</span>v
    </div>
となります。

<figure>

[![dS と du, dv の関係](../../../../assets/media/ufcpp2000/math/surfaceint1.png)](../../../../assets/media/ufcpp2000/math/surfaceint1.png)

<figcaption>dS と du, dv の関係</figcaption>
</figure>


ここで<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂u</td></tr></table>×<table class="frac" summary="fraction"><tr><td class="num">
            ∂<span class="vector">r</span>
          </td></tr><tr><td>∂v</td></tr></table>
      </span>を座標成分ごとにあらわすと
<div class="math">
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂u</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂v</td></tr></table> −
        <table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂v</td></tr></table>,
        <table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂v</td></tr></table> −
        <table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂v</td></tr></table>,
        <table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂v</td></tr></table> −
        <table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂v</td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span>
    </div>
となることと、
<div class="math">
      <span class="normal">d</span>x<span class="normal">d</span>y =
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂v</td></tr></table> −
        <table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂v</td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span><span class="normal">d</span>u<span class="normal">d</span>v
    </div><div class="math">
      <span class="normal">d</span>y<span class="normal">d</span>z =
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂v</td></tr></table> −
        <table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂v</td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span><span class="normal">d</span>u<span class="normal">d</span>v
    </div><div class="math">
      <span class="normal">d</span>z<span class="normal">d</span>x =
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂v</td></tr></table> −
        <table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂u</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂v</td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span><span class="normal">d</span>u<span class="normal">d</span>v
    </div>
であることから、結局<span class="math">
        <span class="normal">d</span><span class="vector">S</span>
      </span>は
<div class="math">
      <span class="normal">d</span><span class="vector">S</span> = <span class="paren" style="font-size:em;">(</span>
        <span class="normal">d</span>y<span class="normal">d</span>z,<span class="normal">d</span>z<span class="normal">d</span>x,<span class="normal">d</span>x<span class="normal">d</span>y
      <span class="paren" style="font-size:em;">)</span>
    </div>
と表すことができます。
よって、<span class="math">
        <span class="vector">F</span>=<span class="paren" style="font-size:em;">(</span>
          F<sub>x</sub>,F<sub>y</sub>,F<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>とおくと、
線積分は
<div class="math">
      <em>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span> = <span class="integral">∫<span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="paren" style="font-size:em;">(</span>
          F<sub>x</sub><span class="normal">d</span>y<span class="normal">d</span>z+F<sub>y</sub><span class="normal">d</span>z<span class="normal">d</span>x+F<sub>z</sub><span class="normal">d</span>x<span class="normal">d</span>y
        <span class="paren" style="font-size:em;">)</span>
      </em>
    </div>
となります。
