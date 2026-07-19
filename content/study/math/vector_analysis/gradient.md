---
title: "勾配"
source_url: "https://ufcpp.net/study/math/vector_analysis/gradient/"
content_type: "Article"
published_at: "2015-05-06T14:17:42"
updated_at: "2015-05-06T14:17:42"
tags: []
umbraco_id: 1496
parent_id: 1491
sort_order: 4
aliases:
  - "/math/vector_analysis/gradient/"
  - "/study/vector_analysis/gradient"
  - "/study/vector_analysis/gradient.html"
  - "/vector_analysis/gradient"
  - "/vector_analysis/gradient.html"
---

# 勾配

## <a id="sec-generated-title-1"></a> <a id="grad"></a>勾配とは

<em>
        勾配とはスカラー場<span class="math">φ</span>に対して、<span class="math">φ</span>がもっととも大きく変化する方向を向き、その変化量と同じ大きさを持つベクトル
      </em>で、<span class="math">
        <span class="normal">grad</span>φ
      </span>と表します。<span class="math">φ</span>を標高に例えて、ある点にボールを置いたときに自然に転がりだす方向を向き、その坂の傾きと同じ大きさを持つベクトルを<span class="math">φ</span>の<strong id="gradient" class="keyword">勾配</strong>というわけです。

すなわち、ある点Pにおけるスカラー場<span class="math">φ</span>の勾配<span class="math">
        <span class="normal">grad</span>φ
      </span>とは、任意の方向<span class="math">n</span>に対し、その方向を向く単位ベクトルを<span class="math">
        <span class="vector">i</span>
        <sub>n</sub>
      </span>、その方向への方向微分を<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂n</td></tr></table>
      </span>とすると、<span class="math">n</span>の向きによらず常に
<div class="math">
      <span class="normal">grad</span>φ = <span class="vector">i</span><sub>n</sub><table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂n</td></tr></table>
    </div>
が成り立つベクトルです。
また、<em>
        <span class="math">
          <span class="normal">grad</span>φ
        </span>は<span class="math">
          <span class="vector">∇</span>φ
        </span>とも書きます
      </em>。

これだけでは分かりにくいでしょうからもう少し直感的な勾配の意味を言うと、
<em>
        勾配とは線積分（「[線積分とは](lineint.md#lineint)」参照）の逆演算
      </em>で、直行座標を用いてあらわすと
<div class="math">
      <em>
        <span class="vector">∇</span>φ = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </em>
    </div>
と言う形になります。
<span class="math">
        <span class="vector">∇</span>φ
      </span>という書き方をするのは、ナブラベクトル<span class="math">
        <span class="vector">∇</span> = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂</td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </span>と<span class="math">φ</span>の積<span class="math">
        <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </span>が勾配となるからです。


## <a id="sec-generated-title-2"></a> <a id="lineint"></a>線積分との関係

<span class="math">φ</span>は位置<span class="math">
        <span class="vector">r</span>
      </span>に関するスカラー場で、
ある経路上での位置は媒介変数<span class="math">t</span>を用いてあらわされているものとします。
すなわち、
<div class="math">
      φ = φ<span class="paren" style="font-size:em;">(</span>
        <span class="vector">r</span>
      <span class="paren" style="font-size:em;">)</span>, <span class="vector">r</span> = <span class="paren" style="font-size:em;">(</span>
        x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,z<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
がなりたつと仮定します。このとき、<span class="math">φ</span>を媒介変数<span class="math">t</span>に関して微分すると、
全微分の公式から
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">dφ</td></tr><tr><td>dt</td></tr></table> =
      <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂x</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">dx</td></tr><tr><td>dt</td></tr></table> +
      <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂y</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">dy</td></tr><tr><td>dt</td></tr></table> +
      <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂z</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">dz</td></tr><tr><td>dt</td></tr></table> =
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂z</td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span>・<table class="frac" summary="fraction"><tr><td class="num">
          d<span class="vector">r</span>
        </td></tr><tr><td>dt</td></tr></table>
    </div>
よって
<div class="math">
      dφ = <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂z</td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span>・d<span class="vector">r</span>
    </div>
ここで、<span class="math">
        <span class="vector">∇</span>φ = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂x</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂y</td></tr></table>,<table class="frac" summary="fraction"><tr><td class="num">∂φ</td></tr><tr><td>∂z</td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
      </span>とおくと、
<div class="math">
      dφ = <span class="vector">∇</span>φ・d<span class="vector">r</span>
    </div>
となります。

上式を始点<span class="math">P</span>、終点<span class="math">Q</span>の経路<span class="math">C</span>に沿って線積分することを考えます。
上式の<span class="math">
        <span class="vector">r</span>
      </span>を<span class="math">C</span>上の点と考えると、
<span class="math">
        <span class="normal">d</span><span class="vector">l</span> = d<span class="vector">r</span>
      </span>となるので、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> Q</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">P</td></tr></table>dφ = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">∇</span>φ・d<span class="vector">l</span>
    </div><div class="math">
      ∴ φ(Q) − φ(P) = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">∇</span>φ・d<span class="vector">l</span>
    </div>
要するに、勾配とは線積分の逆演算なわけです。


## <a id="sec-generated-title-3"></a> <a id="potential"></a>スカラーポテンシャル

<em>
        ベクトル<span class="math">
          <span class="vector">F</span>
        </span>に対して、<span class="math">
          −<span class="vector">∇</span>φ = <span class="vector">F</span>
        </span>となるようなスカラー<span class="math">φ</span>を<span class="math">
          <span class="vector">F</span>
        </span>の<strong id="scaler" class="keyword">スカラーポテンシャル</strong>といいます
      </em>。

しかし、勾配と線積分の関係から
<div class="math">
      φ(Q) − φ(P) = −<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">F</span>・d<span class="vector">l</span>
    </div>
となるわけで、<span class="math">
        <span class="vector">F</span>
      </span>のスカラーポテンシャルが存在するためには、
<span class="math">
        <span class="vector">F</span>
      </span>の線積分が経路<span class="math">C</span>によらずその始点と終点だけで一意に決まる必要があります。
詳しくは「[回転とは](rotation.md#rot)」で説明しますが、このための必要十分条件は<span class="math">
        <span class="vector">∇</span>×<span class="vector">F</span> = 0
      </span>です。
すなわち、
<em>
        <span class="math">
          <span class="vector">∇</span>×<span class="vector">F</span> = 0
        </span>ならば<span class="math">
          F = −<span class="vector">∇</span>φ
        </span>となるスカラー場<span class="math">φ</span>が存在して、これを<span class="math">
          <span class="vector">F</span>
        </span>のスカラーポテンシャルといいます
      </em>。

ちなみに、<em>
        <span class="math">
          <span class="vector">∇</span>×<span class="vector">F</span> = 0
        </span>を満たすようなベクトル場を「<strong id="conservative" class="keyword">保存場</strong>」といいます
      </em>。また、回転が0という意味で「渦のない場」ということもあります。保存場というのは、エネルギー保存則が成り立つ場という意味で、重力場や静電場みたいな場のことです。
