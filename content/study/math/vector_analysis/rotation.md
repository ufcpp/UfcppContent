---
title: "回転"
source_url: "https://ufcpp.net/study/math/vector_analysis/rotation/"
content_type: "Article"
published_at: "2015-05-06T14:17:46"
updated_at: "2015-05-06T14:17:46"
tags: []
umbraco_id: 1498
parent_id: 1491
sort_order: 6
aliases:
  - "/math/vector_analysis/rotation/"
  - "/study/vector_analysis/rotation"
  - "/study/vector_analysis/rotation.html"
  - "/vector_analysis/rotation"
  - "/vector_analysis/rotation.html"
---

# 回転

##<a id="sec-generated-title-1"></a> <a id="rot"></a>回転とは
<em>
        <strong id="rotation" class="keyword">回転</strong>とはある点における単位面積あたりの渦（「[](v_field.md#vortex)」参照）の強さの量です。
      </em>
言い換えると、渦を起こす何らかの物があると考えて、それの密度が回転です。
流体の中で何かを回転させるとそこに渦が出来ますから、ベクトル場の渦を作るこの力のことを回転と言うわけです。

すなわち、ある点Pにおけるベクトル場<span class="math">
        <span class="vector">F</span>
      </span>の回転<span class="math">
        <span class="normal">rot</span>
        <span class="vector">F</span>
      </span>とは、点Pを通る任意の曲面を<span class="math">S</span>、その外周を<span class="math">l</span>、<span class="math">S</span>の点Pにおける法線を<span class="math">
        <span class="vector">n</span>
      </span>とすると、<span class="math">S</span>の取り方によらず常に
<div class="math">
      <em>
        <span class="normal">rot</span>
        <span class="vector">F</span>・<span class="vector">n</span> = <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">S→0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">l</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span>
          </td></tr><tr><td>S</td></tr></table>
      </em>
    </div>
が成り立つベクトルです。
また、<em>
        <span class="math">
          <span class="normal">rot</span>φ
        </span>は<span class="math">
          <span class="vector">∇</span>×φ
        </span>とも書きます
      </em>。

これだけでは分かりにくいでしょうからもう少し直感的な回転の意味を言うと、
回転とは<em>
        線積分（「[線積分とは](lineint.md#lineint)」参照）と面積分（「[面積分とは](surfaceint.md#surfaceint)」参照）を関係付ける微分演算
      </em>で、直交座標を用いて表すと
<div class="math">
      <em>
        <span class="vector">∇</span>×
        <span class="vector">F</span> = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂y</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂z</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂z</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂x</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂y</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </em>
    </div>
となります。
ただし、<span class="math">
        <span class="vector">F</span> = <span class="paren" style="font-size:em;">(</span>
          F<sub>x</sub>, F<sub>y</sub>, F<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>です。
<span class="math">
        <span class="vector">∇</span>×
        <span class="vector">F</span>
      </span>という書き方をするのは、ナブラベクトル<span class="math">
        <span class="vector">∇</span> = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </span>と<span class="math">
        <span class="vector">F</span>
      </span>の外積を取ったものが回転となるからです。


##<a id="sec-generated-title-2"></a> <a id="green"></a>グリーンの定理
いきなり3次元の線積分と面積分を関係付ける公式を出すよりも、2次元で考えたほうが分かりやすいので、まずは2次元で考えます。

図1のような積分経路を考えると、ベクトル場<span class="math">
        <span class="vector">F</span>
      </span>の線積分の値は
<div class="math">
      <span class="paren" style="font-size:em;">{</span>
        F<sub>y</sub>(x+<span class="normal">d</span>x,y)−F<sub>y</sub>(x+,y)
      <span class="paren" style="font-size:em;">}</span>
      <span class="normal">d</span>y − <span class="paren" style="font-size:em;">{</span>
        F<sub>x</sub>(x,y+<span class="normal">d</span>y)−F<sub>x</sub>(x+,y)
      <span class="paren" style="font-size:em;">}</span><span class="normal">d</span>x
    </div>
となります。

<figure>
	[![積分経路](../../../../assets/media/ufcpp2000/math/rot1.png)](../../../../assets/media/ufcpp2000/math/rot1.png)
	<figcaption>積分経路</figcaption>
</figure>


ここで、<span class="math">
        F<sub>y</sub>(x,y)−F<sub>y</sub>(x+<span class="normal">d</span>x,y) = <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂x</td></tr></table><span class="normal">d</span>x
      </span>であることを用いると、この式の第一項は
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂x</td></tr></table><span class="normal">d</span>x<span class="normal">d</span>y
      </span>
となります。
同様に第二項も
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂y</td></tr></table><span class="normal">d</span>x<span class="normal">d</span>y
      </span>
となりますので、この線積分の値は
<div class="math">
      <span class="paren" style="font-size:1.5em;">{</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂x</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>x</sub>
          </td></tr><tr><td>∂y</td></tr></table>
      <span class="paren" style="font-size:1.5em;">}</span>
      <span class="normal">d</span>x<span class="normal">d</span>y
    </div>
となります。
すなわち、
<div class="math">
      <em>
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">l</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span> = <span class="integral">∫<span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂x</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂y</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>x<span class="normal">d</span>y
      </em>
    </div>
が成り立ちます。
この式をグリーンの定理といいます。


##<a id="sec-generated-title-3"></a> <a id="stokes"></a>ストークスの定理
回転は単位面積あたりの渦の強さなわけですから、回転の面積分の値は渦の強さに等しくなります。
つまり、
<div class="math">
      <em>
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">l</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="vector">∇</span>×<span class="vector">F</span><span class="normal">d</span><span class="vector">S</span>
      </em>
    </div>
この式をストークスの定理といい、線積分と面積分を関係付ける公式です。

この面積分の値をx方向成分、y方向成分、ｚ方向成分に分けて考えます。
x方向成分は曲面<span class="math">S</span>をy−z平面に投影したものを考えればいいので、
2次元の場合と同様にグリーンの定理を使って
<span class="math">
        <span class="integral">∫<span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂y</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>y<span class="normal">d</span>z
      </span>
とあらわすことができます。
同様に、y方向成分、z方向成分はそれぞれ
<span class="math">
        <span class="integral">∫<span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂z</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂x</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>z<span class="normal">d</span>x
      </span>、
<span class="math">
        <span class="integral">∫<span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂x</td></tr></table>−<table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂y</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>x<span class="normal">d</span>z
      </span>
となります。
これらを足し合わせたものが面積分の値になりますので、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">l</td></tr></table>
      <span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span> =
      <span class="integral">∫<span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">[</span>
        <span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂y</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>y<span class="normal">d</span>z
        ＋
        <span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂z</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂x</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>z<span class="normal">d</span>x
        ＋
        <span class="paren" style="font-size:1.5em;">{</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂x</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂y</td></tr></table>
        <span class="paren" style="font-size:1.5em;">}</span><span class="normal">d</span>x<span class="normal">d</span>z
      <span class="paren" style="font-size:1.5em;">]</span>
    </div><div class="math">
      <em>
        ∴
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">l</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span> =
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂y</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂z</td></tr></table>
          ,
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂z</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂x</td></tr></table>
          ,
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂x</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂y</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
        ・<span class="normal">d</span><span class="vector">S</span>
      </em>
    </div>
となります。そして回転の定義より、
<div class="math">
      <em>
        <span class="vector">∇</span>×
        <span class="vector">F</span>
        ＝
        <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂y</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂z</td></tr></table>
          ,
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂z</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>z</sub>
            </td></tr><tr><td>∂x</td></tr></table>
          ,
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>y</sub>
            </td></tr><tr><td>∂x</td></tr></table>
          －
          <table class="frac" summary="fraction"><tr><td class="num">
              ∂F<sub>x</sub>
            </td></tr><tr><td>∂y</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </em>
    </div>
となります。
