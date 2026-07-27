---
title: "正則関数"
source_url: "https://ufcpp.net/study/math/analysis/regular/"
content_type: "Article"
published_at: "2015-05-06T14:16:48"
updated_at: "2015-05-18T10:38:18"
tags: []
umbraco_id: 1469
parent_id: 1464
sort_order: 4
aliases:
  - "/study/analysis/regular.html"
---

# 正則関数

## <a id="sec-generated-title-1"></a> <a id="regular"></a>正則関数

<strong id="regular_func" class="keyword">正則関数</strong>（regular function）とは、
定義域中の任意の点で微分可能である関数のことを言います。
この正則関数についての説明を行う前に、
まず、複素関数が微分可能とはどういうことなのかを次節「[複素関数の微分可能性](#differentiability)」で説明します。


## <a id="sec-generated-title-2"></a> <a id="differentiability"></a>複素関数の微分可能性

複素関数<span class="math">
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
      </span>の微分は実数の場合と同じように、
<div class="math">
      <em>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>f
          </td></tr><tr><td>
            <span class="normal">d</span>z
          </td></tr></table>
        <span class="normal">=</span>
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">
          <span class="normal">|</span>Δz<span class="normal">|</span>→<span class="normal">0</span>
        </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">
            f<span class="paren" style="font-size:em;">(</span>
              z<span class="normal">+</span>Δz
            <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>Δz</td></tr></table>
      </em>
    </div>
で定義され、<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>f
          </td></tr><tr><td>
            <span class="normal">d</span>z
          </td></tr></table>
      </span>を<span class="math">f</span>の<em>導関数</em>または<em>微分係数</em>といいます。

ところで、複素数とは複素平面上の点と考えることができますから、
2変数関数の場合と同じように、微分を行う向きによって導関数の値が変わってしまう可能性があります。
当然、そのような状態は好ましくありませんので、微分を行う向きに拠らず導関数が一通りに決まるときだけ、微分可能であると定義します。

ここで、複素関数が微分可能であるための条件を調べたいと思います。
微分を行う向きに拠らず導関数が一通りに決まるわけですから、
当然、実軸方向および虚軸方向に沿っての微分係数も一致するはずです。
そのためにまず、実軸方向および虚軸方向に沿っての微分を考えます。
<span class="math">
        z<span class="normal">=</span>x<span class="normal">+</span>iy, f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> u<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span> iv<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
      </span>と置くと、
実軸方向に沿った微分は
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">
        Δx→<span class="normal">0</span>
      </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          f<span class="paren" style="font-size:em;">(</span>
            z<span class="normal">+</span>Δx
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δx</td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">
        Δx→<span class="normal">0</span>
      </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          u<span class="paren" style="font-size:em;">(</span>
            x<span class="normal">+</span>Δx,y
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>u<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span> i<span class="paren" style="font-size:em;">(</span>
            v<span class="paren" style="font-size:em;">(</span>
              x<span class="normal">+</span>Δx,y
            <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>v<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δx</td></tr></table>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num">∂u</td></tr><tr><td>∂x</td></tr></table>
      <span class="normal">+</span>
      i<table class="frac" summary="fraction"><tr><td class="num">∂v</td></tr><tr><td>∂x</td></tr></table>
    </div>
となり、虚軸方向に沿った微分は
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">
        Δy→<span class="normal">0</span>
      </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          f<span class="paren" style="font-size:em;">(</span>
            z<span class="normal">+</span>iΔy
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>iΔy</td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">
        Δy→<span class="normal">0</span>
      </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          v<span class="paren" style="font-size:em;">(</span>
            x,y<span class="normal">+</span>Δy
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>v<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">−</span> i<span class="paren" style="font-size:em;">(</span>
            u<span class="paren" style="font-size:em;">(</span>
              x,y<span class="normal">+</span>Δy
            <span class="paren" style="font-size:em;">)</span><span class="normal">−</span>u<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δy</td></tr></table>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num">∂v</td></tr><tr><td>∂y</td></tr></table>
      <span class="normal">−</span>
      i<table class="frac" summary="fraction"><tr><td class="num">∂u</td></tr><tr><td>∂y</td></tr></table>
    </div>
となります。
これらが一致していてほしいわけですから、微分可能な複素関数は
<span class="math">u,v</span>が微分可能でかつ、
<div class="math">
      <em>
        <table class="frac" summary="fraction"><tr><td class="num">∂u</td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂v</td></tr><tr><td>∂y</td></tr></table>
      </em>
    </div><div class="math">
      <em>
        <table class="frac" summary="fraction"><tr><td class="num">∂v</td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">=</span>
        <span class="normal">−</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂u</td></tr><tr><td>∂y</td></tr></table>
      </em>
    </div>
という関係式が成り立っているはずです。
この関係式を<em>コーシー・リーマンの関係式</em>といいます。
逆に、この関係式が成り立つならば微分可能であることも証明できますので、教科書などを調べてみてください。

コーシー・リーマンの関係式が成り立つとき、すなわち微分可能であるとき、その関数は「正則である」といいます。
また、定義域中の任意の点でコーシー・リーマンの関係式が成り立つ関数のことを「正則関数」といいます。


## <a id="sec-generated-title-3"></a> <a id="property"></a>正則関数の性質

前節で述べたように、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">∂u</td></tr><tr><td>∂x</td></tr></table>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num">∂v</td></tr><tr><td>∂y</td></tr></table>
    </div><div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">∂v</td></tr><tr><td>∂x</td></tr></table>
      <span class="normal">=</span>
      <span class="normal">−</span>
      <table class="frac" summary="fraction"><tr><td class="num">∂u</td></tr><tr><td>∂y</td></tr></table>
    </div>
を満たす関数<span class="math">
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> u<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span> iv<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
      </span>は正則であるといいます。
関数<span class="math">f</span>が正則であるとき、以下の定理が成り立ちます。


### <a id="sec-generated-title-4"></a> <a id="differential"></a>微分可能

<span class="math">f</span>は微分可能です。

関数が正則であることは、その関数が微分可能であることの必要十分条件です。
そして、正則な関数の微分はすべて実数変数のときと同じように行えます。
例えば、<span class="math">
          z<sup>n</sup>
        </span>の導関数は<span class="math">
          nz<sup>
            n<span class="normal">−</span><span class="normal">1</span>
          </sup>
        </span>ですし、<span class="math">
          e<sup>z</sup>
        </span>の導関数は<span class="math">
          e<sup>z</sup>
        </span>です。

ちなみに、ここでは証明は省略しますが、<em>正則関数は必ず無限階微分可能</em>です。
実数関数の場合、<span class="math">N</span> 階微分できたからと言って、<span class="math">
          N <span class="normal">+</span><span class="normal">1</span>
        </span> 回目の微分もできるとは限りませんでしたが、
複素関数の場合は、1 階微分できれば必ず無限階微分可能になります。


### <a id="sec-generated-title-5"></a> <a id="integral"></a>定積分が経路によらない

複素平面状の任意の経路<span class="math">C</span>に対して<span class="math">
          <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span><span class="normal">0</span>
        </span> になります。

<span class="math">
          <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z
        </span>に対してグリーンの定理（「[グリーンの定理](../vector_analysis/rotation.md#green)」参照）を用いると、
<div class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="paren" style="font-size:em;">(</span>
          u<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>iv<span class="paren" style="font-size:em;">(</span>x.y<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
          <span class="normal">d</span>x<span class="normal">+</span>i<span class="normal">d</span>y
        <span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="paren" style="font-size:em;">(</span>
          u<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>iv<span class="paren" style="font-size:em;">(</span>x.y<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x <span class="normal">+</span><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>v<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>iu<span class="paren" style="font-size:em;">(</span>x.y<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span><span class="normal">d</span>y
      </div><div class="math">
        <span class="normal">=</span>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
        <span class="paren" style="font-size:1.2em;">{</span>
          <span class="paren" style="font-size:em;">(</span>
            v<sub>x</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">−</span>iu<sub>x</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">−</span>
          <span class="paren" style="font-size:em;">(</span>
            u<sub>y</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>iv<sub>y</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:1.2em;">}</span>
        <span class="normal">d</span>x<span class="normal">d</span>y
      </div><div class="math">
        <span class="normal">=</span>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
        <span class="paren" style="font-size:1.2em;">{</span>
          <span class="paren" style="font-size:em;">(</span>
            v<sub>x</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">−</span>u<sub>y</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">−</span>i<span class="paren" style="font-size:em;">(</span>
            v<sub>y</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>u<sub>x</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:1.2em;">}</span>
        <span class="normal">d</span>x<span class="normal">d</span>y
      </div>
となります。
この式にコーシー・リーマンの関係式を代入すると、
<div class="math">
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
        <span class="paren" style="font-size:1.2em;">{</span>
          <span class="paren" style="font-size:em;">(</span>
            v<sub>x</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">−</span>u<sub>y</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">−</span>i<span class="paren" style="font-size:em;">(</span>
            v<sub>y</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span><span class="normal">+</span>u<sub>x</sub><span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:1.2em;">}</span>
        <span class="normal">d</span>x<span class="normal">d</span>y <span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="normal">0</span><span class="normal">d</span>x<span class="normal">d</span>y <span class="normal">=</span><span class="normal">0</span>
      </div>
となり、したがって、
<div class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span><span class="normal">0</span>
      </div>
が成り立ちます。

この式は、閉路上の積分が常に 0 になるということですが、
これはすなわち、
正則関数の積分はその経路に拠らず、両端の点のみで決まるということを示しています。
つまり、
始点が<span class="math">α</span>、終点が<span class="math">β</span>であるような任意の積分経路<span class="math">C</span>に対して、正則関数の積分は
<div class="math">
        <em>
          <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> β</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">α</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span> F<span class="paren" style="font-size:em;">(</span>β<span class="paren" style="font-size:em;">)</span><span class="normal">−</span> F<span class="paren" style="font-size:em;">(</span>α<span class="paren" style="font-size:em;">)</span>
        </em>
      </div>
とあらわすことができるということです。
（途中の経路がどうであれ、始点と終点のみで値を表せる。）
ここで、<span class="math">F</span>は<span class="math">
          <table class="frac" summary="fraction"><tr><td class="num">
              <span class="normal">d</span>F
            </td></tr><tr><td>
              <span class="normal">d</span>z
            </td></tr></table><span class="normal">=</span> f
        </span>のとなる関数で、実数関数のときと同じく、<em>原始関数</em>と呼びます。


### <a id="sec-generated-title-6"></a> <a id="conjugate"></a>共役変数を含まない

<span class="math">f</span>は<span class="math">
          z<sup>\*</sup>
        </span>を含みません。

複素関数<span class="math">f</span>は引数<span class="math">z</span>の実部<span class="math">x</span>および虚部<span class="math">y</span>を用いて<span class="math">
          f<span class="paren" style="font-size:em;">(</span>x,y<span class="paren" style="font-size:em;">)</span>
        </span>とあらわせます。
<span class="math">
          z<span class="normal">=</span>x<span class="normal">+</span>iy, z<sup>\*</sup><span class="normal">=</span>x<span class="normal">−</span>iy
        </span>
（
<span class="math">
          z<sup>\*</sup>
        </span>は<span class="math">z</span>の共役複素数
）ですから、
<div class="math">
        x<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table><span class="paren" style="font-size:em;">(</span>
          z <span class="normal">+</span>z<sup>*</sup>
        <span class="paren" style="font-size:em;">)</span>
      </div><div class="math">
        y<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>i
        </td></tr></table><span class="paren" style="font-size:em;">(</span>
          z <span class="normal">−</span>z<sup>*</sup>
        <span class="paren" style="font-size:em;">)</span>
      </div>
となりますので、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂z</td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>∂z</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">+</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>∂z</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂y</td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">+</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>i
        </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂y</td></tr></table>
      </div><div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>
            ∂z<sup>*</sup>
          </td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂x</td></tr><tr><td>
            ∂z<sup>*</sup>
          </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">+</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂y</td></tr><tr><td>
            ∂z<sup>*</sup>
          </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂y</td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">−</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>i
        </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂y</td></tr></table>
      </div>
という関係式が成り立ちます。
ここで、コーシー・リーマンの関係式から
<div class="math">
        i<table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂x</td></tr></table><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂y</td></tr></table>
      </div>
が成り立ちますので、
<div class="math">
        <em>
          <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂z</td></tr></table>
          <span class="normal">=</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂x</td></tr></table>
          <span class="normal">=</span>
          <span class="normal">−</span>i<table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>∂y</td></tr></table>
        </em>
      </div><div class="math">
        <em>
          <table class="frac" summary="fraction"><tr><td class="num">∂f</td></tr><tr><td>
              ∂z<sup>*</sup>
            </td></tr></table>
          <span class="normal">=</span>
          <span class="normal">0</span>
        </em>
      </div>
となります。
すなわち、<em>
          正則な関数<span class="math">f</span>は<span class="math">
            z<sup>\*</sup>
          </span>を含みません
        </em>。
例えば、<span class="math">
          z<sup>n</sup>
        </span>や<span class="math">
          e<sup>z</sup>
        </span>などは正則ですが、<span class="math">
          <span class="normal">|</span>z<span class="normal">|</span><sup>
            <span class="normal">2</span>
          </sup><span class="normal">=</span> zz<sup>\*</sup>
        </span>や<span class="math">
          Re<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">
              z <span class="normal">+</span>z<sup>\*</sup>
            </td></tr><tr><td>
              <span class="normal">2</span>
            </td></tr></table>
        </span>などは正則ではありません。

無限回微分可能な実数関数 <span class="math">
          f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        </span> の実変数 <span class="math">x</span> を複素変数 <span class="math">z</span> で置き換えたものは、当然、<span class="math">
          z<sup>\*</sup>
        </span> を含みませんから、正則関数になります。


### <a id="sec-generated-title-7"></a> <a id="summary"></a>まとめ

一般に複素関数は実数関数と同じようには扱えません。
しかし、コーシー・リーマンの関係式を満たす正則な複素関数は、
微分・不定積分を定義でき、しかも実数のときとまったく同じように扱えます。
また、微分可能な実数関数<span class="math">
          f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        </span>の<span class="math">x</span>を複素数<span class="math">z</span>に置き換えた複素関数<span class="math">
          f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </span>は正則になります。
