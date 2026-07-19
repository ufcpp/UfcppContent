---
title: "静電場"
source_url: "https://ufcpp.net/study/physics/em/electrostatic/"
content_type: "Article"
published_at: "2015-05-06T14:20:12"
updated_at: "2015-05-06T14:20:12"
tags: []
umbraco_id: 1565
parent_id: 1561
sort_order: 3
aliases:
  - "/em/electrostatic"
  - "/em/electrostatic.html"
  - "/physics/em/electrostatic/"
  - "/study/em/electrostatic"
  - "/study/em/electrostatic.html"
---

# 静電場

##<a id="sec-generated-title-1"></a> <a id="maxwell"></a>静電場中のマクスウェルの方程式
時間的に変化しない電場を<strong id="static" class="keyword">静電場</strong>といいます。
静電場中では、時間微分に関する項が0になるので、マクスウェルの方程式は

<table summary="">

	<tr>
		<th>微分形</th>
		<th>積分形</th>
		<th>不連続面での境界条件</th>
	</tr>
	<tr>
		<td markdown="1"><div class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = 0
          </div><div class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><span class="vector">∇</span>・<a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <a href="variable.md#rho" title="電荷密度">ρ</a>
          </div></td>
		<td markdown="1"><div class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
            <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>・<span class="normal">d</span><span class="vector">l</span> = 0
          </div><div class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>・<span class="normal">d</span><span class="vector">S</span> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>・<span class="normal">d</span><span class="vector">S</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table><a href="variable.md#rho" title="電荷密度">ρ</a>・<span class="normal">d</span>V
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a>×<span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a><sub>1</sub> − <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = 0
          </div><div class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a>×<span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><sub>1</sub> − <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = <a href="variable.md#xi" title="グザイ">ξ</a>
          </div></td>
	</tr>
</table>


と表せます。


##<a id="sec-generated-title-2"></a> <a id="elepotential"></a>電位
静電場中でのマクスウェルの方程式で、<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>は<span class="math">
        <span class="vector">∇</span>×<a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = 0
      </span>を満たす保存場なので、スカラーポテンシャル<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>が定義できて
<div class="math">
      <em>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </em>
    </div>
この電場のスカラーポテンシャル<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>を<strong id="elepotential" class="keyword">電位</strong>といいます。
スカラーポテンシャルの性質から、点<span class="math">O</span>を基準とした点<span class="math">P</span>での電位<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
        <sub>P</sub>
      </span>は
<div class="math">
      <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      <sub>P</sub> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> O</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">P</td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>・<span class="normal">d</span><span class="vector">l</span>
    </div>
と表せます。


##<a id="sec-generated-title-3"></a> <a id="howtopotential"></a>静電場における電位の求め方
<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>を電場に関するガウスの法則に代入すると
<div class="math">
      <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
      <a href="variable.md#phi" title="スカラーポテンシャル">φ</a> = −<table class="frac" summary="fraction"><tr><td class="num">
          <a href="variable.md#rho" title="電荷密度">ρ</a>
        </td></tr><tr><td>
          <a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table>
    </div>
が得られます。
この方程式を解けば静電場の電位を求めることが出来ます。
具体的には、
<div class="math">
      <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>U = <a href="variable.md#delta" title="デルタ関数">δ</a>
    </div>
の解<span class="math">
        U = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<span class="normal">|</span>
            <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>
          <span class="normal">|</span>
        </td></tr></table>
      </span>と<span class="math">
        −<table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>
          </td></tr><tr><td>
            <a href="variable.md#eps" title="物質中の誘電率">ε</a>
          </td></tr></table>
      </span>
との畳み込み積分
<div class="math">
      <a href="variable.md#phi" title="スカラーポテンシャル">φ</a> = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        <a href="variable.md#eps" title="物質中の誘電率">ε</a>
      </td></tr></table>U*<a href="variable.md#rho" title="電荷密度">ρ</a>
    </div>
つまり、
<div class="math">
      <em>
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>(x,y,z) = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><a href="variable.md#rho" title="電荷密度">ρ</a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)U(x−<a href="variable.md#xi" title="グザイ">ξ</a>,y−<a href="variable.md#eta" title="イータ">η</a>,z−<a href="variable.md#zeta" title="ゼータ">ζ</a>)<a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>
          </td></tr><tr><td>r</td></tr></table><span class="normal">d</span>V
      </em>
    </div>
と表される。
ただし、<span class="math">
        <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a> = (x, y, z), <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>' = (x−<a href="variable.md#xi" title="グザイ">ξ</a>, y−<a href="variable.md#eta" title="イータ">η</a>, z−<a href="variable.md#zeta" title="ゼータ">ζ</a>), r = <span class="normal">|</span>
          <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
        <span class="normal">|</span>
      </span>である。


##<a id="sec-generated-title-4"></a> <a id="howtofield"></a>静電場における電場の求め方
電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>は<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>で与えられ、電位<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>は<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>(x,y,z) = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a>
      </span>によって求められるので、電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>は、
<div class="math">
      <em>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="vector">∇</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="vector">∇</span><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a>
      </em>
    </div><div class="math">
      <em>
        = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)<a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
            <sup>3</sup>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          4π<a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>
            <span class="vector">i</span>
            <sub>r</sub>
          </td></tr><tr><td>
            r<sup>2</sup>
          </td></tr></table><span class="normal">d</span>V
      </em>
    </div>
によって求められる。
ただし、<span class="math">
        <span class="vector">i</span>
        <sub>r</sub>
      </span>は<span class="math">
        <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
      </span>方向を向く単位ベクトル、すなわち<span class="math">
        <span class="vector">i</span><sub>r</sub> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table>
      </span>である。
