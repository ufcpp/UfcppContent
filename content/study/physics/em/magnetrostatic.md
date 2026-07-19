---
title: "静磁場"
source_url: "https://ufcpp.net/study/physics/em/magnetrostatic/"
content_type: "Article"
published_at: "2015-05-06T14:20:14"
updated_at: "2015-05-06T14:20:14"
tags: []
umbraco_id: 1566
parent_id: 1561
sort_order: 4
aliases:
  - "/em/magnetrostatic"
  - "/em/magnetrostatic.html"
  - "/physics/em/magnetrostatic/"
  - "/study/em/magnetrostatic"
  - "/study/em/magnetrostatic.html"
---

# 静磁場

## <a id="sec-generated-title-1"></a> <a id="maxwell"></a>静磁場中のマクスウェルの方程式

時間的に変化しない磁場を<strong id="static" class="keyword">静磁場</strong>といいます。
静磁場中では、時間微分に関する項が0になるので、マクスウェルの方程式は

<table summary="">

	<tr>
		<th>微分形</th>
		<th>積分形</th>
		<th>不連続面での境界条件</th>
	</tr>
	<tr>
		<td markdown="1"><div class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
          </div><div class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <a href="variable.md#mu" title="物質中の透磁率">μ</a><span class="vector">∇</span>・<a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = 0
          </div></td>
		<td markdown="1"><div class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
            <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>・<span class="normal">d</span><span class="vector">l</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>・<span class="normal">d</span><span class="vector">S</span>
          </div><div class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
            <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>・<span class="normal">d</span><span class="vector">S</span> = <a href="variable.md#mu" title="物質中の透磁率">μ</a><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>・<span class="normal">d</span><span class="vector">S</span> = 0
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a>×<span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a><sub>1</sub> − <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = <a href="variable.md#K" title="面電流ベクトル"><span class="vector">K</span></a>
          </div><div class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a>×<span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><sub>1</sub> − <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = 0
          </div></td>
	</tr>
</table>


と表せます。


## <a id="sec-generated-title-2"></a> <a id="vectorpotential"></a>ベクトルポテンシャル

静磁場中でのマクスウェルの方程式で、<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>は<span class="math">
        <span class="vector">∇</span>・<a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = 0
      </span>を満たすソレノイダル場なので、<strong id="vectorpotential" class="keyword">ベクトルポテンシャル</strong><span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>が定義でき、
<div class="math">
      <em>
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </em>
    </div>
と表せます。
電磁気学ではベクトルポテンシャルを使うのは磁場だけなので、ただ単にベクトルポテンシャルと言えば磁場のベクトルポテンシャルのことを指します。
また、電束密度<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>が２つのベクトルポテンシャル<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>1</sub>, <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>2</sub>
      </span>によって表されているとすると
<div class="math">
      <span class="vector">∇</span>×
      <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      <sub>1</sub> = <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>e2</sub>
    </div><div class="math">
      <span class="vector">∇</span>×(<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>1</sub> − <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>2</sub>) = 0
    </div>
となり、<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>1</sub> − <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>2</sub>
      </span>は保存場となっているので
<span class="math">
        <span class="vector">∇</span>ψ = <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>1</sub> − <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a><sub>2</sub>
      </span>
となるスカラー場<span class="math">ψ</span>が定義でき、ベクトルポテンシャルには
このスカラー場<span class="math">ψ</span>の勾配の分だけの不定性が残るということになります。
このように、ベクトルポテンシャルには任意性があるので、<em>
        <span class="math">
          <span class="vector">∇</span>・<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = 0
        </span>を満たすようにすることが出来ます
      </em>。
通常、静磁場のベクトルポテンシャルはこのように発散が<span class="math">0</span>になるように取ります。


## <a id="sec-generated-title-3"></a> <a id="howtopotential"></a>静磁場におけるベクトルポテンシャルの求め方

<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>, <span class="vector">∇</span>・<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = 0
      </span>をアンペアマクスウェルの法則に代入すると
<span class="math">
        <span class="vector">∇</span>×(<span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>) = <span class="vector">∇</span>・(<span class="vector">∇</span><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>) − <a href="variable.md#Laplace" title="ラプラシアン">Δ</a><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = − <a href="variable.md#Laplace" title="ラプラシアン">Δ</a><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>より、
<div class="math">
      <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
      <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = −<a href="variable.md#mu" title="物質中の透磁率">μ</a><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
    </div>
が得られます。
この方程式を解けば静磁場の電位を求めることが出来ます。
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
        −<a href="variable.md#mu" title="物質中の透磁率">μ</a><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
      </span>
との畳み込み積分
<div class="math">
      <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = −<a href="variable.md#mu" title="物質中の透磁率">μ</a>U*<a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
    </div>
つまり、
<div class="math">
        <em>
          <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>(x,y,z) = −<a href="variable.md#mu" title="物質中の透磁率">μ</a><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)U(x−<a href="variable.md#xi" title="グザイ">ξ</a>,y−<a href="variable.md#eta" title="イータ">η</a>,z−<a href="variable.md#zeta" title="ゼータ">ζ</a>)<a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#mu" title="物質中の透磁率">μ</a>
          </td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#mu" title="物質中の透磁率">μ</a>
          </td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
          </td></tr><tr><td>r</td></tr></table><span class="normal">d</span>V
        </em>
      </div>
と表される。
ただし、<span class="math">
        <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a> = (x, y, z), <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>' = (x−<a href="variable.md#xi" title="グザイ">ξ</a>, y−<a href="variable.md#eta" title="イータ">η</a>, z−<a href="variable.md#zeta" title="ゼータ">ζ</a>), r = <span class="normal">|</span>
          <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
        <span class="normal">|</span>
      </span>である。


## <a id="sec-generated-title-4"></a> <a id="howtofield"></a>静磁場における磁場の求め方

磁場<span class="math">
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
      </span>は<span class="math">
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <a href="variable.md#mu" title="物質中の透磁率">μ</a>
        </td></tr></table><a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <a href="variable.md#mu" title="物質中の透磁率">μ</a>
        </td></tr></table><span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>で与えられ、ベクトルポテンシャル<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>は<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>(x,y,z) = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a>
      </span>によって求められるので、磁束密度<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>は、
<div class="math">
      <em>
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>4π</td></tr></table><span class="vector">∇</span>×<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="vector">∇</span>×<table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a>
      </em>
    </div><div class="math">
      <em>
        = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>(<a href="variable.md#xi" title="グザイ">ξ</a>,<a href="variable.md#eta" title="イータ">η</a>,<a href="variable.md#zeta" title="ゼータ">ζ</a>)×<a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
          </td></tr><tr><td>
            <span class="normal">|</span>
              <a href="variable.md#r" title="位置ベクトル"><span class="vector">r</span></a>'
            <span class="normal">|</span>
            <sup>3</sup>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#xi" title="グザイ">ξ</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#eta" title="イータ">η</a><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><a href="variable.md#zeta" title="ゼータ">ζ</a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>×<span class="vector">i</span><sub>r</sub>
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

特に導線に電流が流れているときには<span class="math">
        <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a><span class="normal">d</span>V = <a href="variable.md#current" title="電流">I</a><span class="normal">d</span><span class="vector">l</span> = <a href="variable.md#current" title="電流">I</a><span class="vector">i</span><sub>l</sub><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>l
      </span>となるので、この式は
<div class="math">
      <em>
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>4π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#current" title="電流">I</a><span class="vector">i</span><sub>l</sub>×<span class="vector">i</span><sub>r</sub>
          </td></tr><tr><td>
            r<sup>2</sup>
          </td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>l
      </em>
    </div>
となる。
ただし、この線積分は導線に沿った経路上での線積分で、<span class="math">
        <span class="vector">i</span>
        <sub>l</sub>
      </span>は電流の流れる向きを向く単位ベクトルである。
この式を<em>ビオ・サバールの法則</em>という。
