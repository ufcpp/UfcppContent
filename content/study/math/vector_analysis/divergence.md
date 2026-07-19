---
title: "発散"
source_url: "https://ufcpp.net/study/math/vector_analysis/divergence/"
content_type: "Article"
published_at: "2015-05-06T14:17:44"
updated_at: "2015-05-18T17:09:19"
tags: []
umbraco_id: 1497
parent_id: 1491
sort_order: 5
aliases:
  - "/math/vector_analysis/divergence/"
  - "/study/vector_analysis/divergence"
  - "/study/vector_analysis/divergence.html"
  - "/vector_analysis/divergence"
  - "/vector_analysis/divergence.html"
---

# 発散

##<a id="sec-generated-title-1"></a> <a id="div"></a>発散とは
<em>
        <strong id="divergence" class="keyword">発散</strong>とはある点における単位体積あたりのベクトル場の湧き出し（「[流束](v_field.md#flux)」参照）の量、つまり湧き出しの密度
      </em>です。
発散という言葉は、ある点から湧き出したベクトル場が外に向かって広がっていくという意味で使われています。

すなわち、ある点Pにおけるベクトル場<span class="math">
        <span class="vector">F</span>
      </span>の発散<span class="math">
        <span class="normal">div</span>
        <span class="vector">F</span>
      </span>とは、点Pを囲む任意の閉曲面を<span class="math">S</span>、その内部の体積を<span class="math">V</span>とすると、
<div class="math">
      <em>
        <span class="normal">div</span>
        <span class="vector">F</span>
        = <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">V→0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span>
          </td></tr><tr><td>V</td></tr></table>
      </em>
    </div>
で定義されます。
また、<em>
        <span class="math">
          <span class="normal">div</span>
          <span class="vector">F</span>
        </span>は<span class="math">
          <span class="vector">∇</span>・
          <span class="vector">F</span>
        </span>とも書きます
      </em>。

これだけでは分かりにくいでしょうからもう少し直感的な発散の意味を言うと、
発散とは<em>
        面積分（「[面積分とは](surfaceint.md#surfaceint)」参照）と体積積分（「[体積積分とは](volumeint.md#volumeint)」参照）を関係付ける微分演算
      </em>で、直交座標を用いて表すと
<div class="math">
      <em>
        <span class="vector">∇</span>・
        <span class="vector">F</span> = <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>x</sub>
          </td></tr><tr><td>∂x</td></tr></table>+<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂y</td></tr></table>+<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>z</sub>
          </td></tr><tr><td>∂z</td></tr></table>
      </em>
    </div>
となります。
ただし、<span class="math">
        <span class="vector">F</span> = <span class="paren" style="font-size:em;">(</span>
          F<sub>x</sub>, F<sub>y</sub>, F<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>です。
<span class="math">
        <span class="vector">∇</span>・
        <span class="vector">F</span>
      </span>という書き方をするのは、ナブラベクトル<span class="math">
        <span class="vector">∇</span> = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </span>と<span class="math">
        <span class="vector">F</span>
      </span>の内積を取ったものが発散となるからです。


##<a id="sec-generated-title-2"></a> <a id="gaus"></a>ガウスの定理
発散は湧き出しの密度なわけですから発散を体積積分したものは湧き出しに等しくなります。
つまり、
<div class="math">
      <em>
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
        <span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table><span class="vector">∇</span>・<span class="vector">F</span><span class="normal">d</span>V
      </em>
    </div>
この式をガウスの定理といい、面積分と体積積分を関係付ける公式です。

図1のような微小体積を考えると、その表面を貫いて外に出て行くベクトル場<span class="math">
        <span class="vector">F</span>
      </span>の流束は
<div class="math">
      <span class="paren" style="font-size:em;">{</span>
        F<sub>x</sub>(x,y,z)−F<sub>x</sub>(x+<span class="normal">d</span>x,y,z)
      <span class="paren" style="font-size:em;">}</span>
      <span class="normal">d</span>y<span class="normal">d</span>z+<span class="paren" style="font-size:em;">{</span>
        F<sub>y</sub>(x,y,z)−F<sub>y</sub>(x,y+<span class="normal">d</span>y,z)
      <span class="paren" style="font-size:em;">}</span><span class="normal">d</span>z<span class="normal">d</span>x+<span class="paren" style="font-size:em;">{</span>
        F<sub>z</sub>(x,y,z)−F<sub>z</sub>(x,y,z+<span class="normal">d</span>z)
      <span class="paren" style="font-size:em;">}</span><span class="normal">d</span>x<span class="normal">d</span>y
    </div>
となります。

<figure>
	[![微小体積](../../../../assets/media/ufcpp2000/math/div1.png)](../../../../assets/media/ufcpp2000/math/div1.png)
	<figcaption>微小体積</figcaption>
</figure>


ここで、<span class="math">
        F<sub>x</sub>(x,y,z)−F<sub>x</sub>(x+<span class="normal">d</span>x,y,z) = <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>x</sub>
          </td></tr><tr><td>∂x</td></tr></table><span class="normal">d</span>x
      </span>であることを用いると、この式の第一項は
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>x</sub>
          </td></tr><tr><td>∂x</td></tr></table>
        <span class="normal">d</span>V
      </span>
となります(<span class="math">
        <span class="normal">d</span>V=<span class="normal">d</span>x<span class="normal">d</span>y<span class="normal">d</span>z
      </span>)。
同様に第二項、第三項も
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂y</td></tr></table><span class="normal">d</span>V, <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>z</sub>
          </td></tr><tr><td>∂z</td></tr></table><span class="normal">d</span>V
      </span>
となりますので、左図の微小体積の表面を貫く<span class="math">
        <span class="vector">F</span>
      </span>の流束は
<div class="math">
      <span class="paren" style="font-size:2em;">{</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>x</sub>
          </td></tr><tr><td>∂x</td></tr></table>+<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂y</td></tr></table>+<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>z</sub>
          </td></tr><tr><td>∂z</td></tr></table>
      <span class="paren" style="font-size:2em;">}</span>
      <span class="normal">d</span>V
    </div>
となります。
そして発散の定義から、
<div class="math">
      <em>
        <span class="vector">∇</span>・
        <span class="vector">F</span> = <table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>x</sub>
          </td></tr><tr><td>∂x</td></tr></table>+<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>y</sub>
          </td></tr><tr><td>∂y</td></tr></table>+<table class="frac" summary="fraction"><tr><td class="num">
            ∂F<sub>z</sub>
          </td></tr><tr><td>∂z</td></tr></table>
      </em>
    </div>
となります。
