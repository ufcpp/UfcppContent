---
title: "マクスウェルの方程式"
source_url: "https://ufcpp.net/study/physics/em/maxwell/"
content_type: "Article"
published_at: "2015-05-06T14:20:04"
updated_at: "2015-05-06T14:20:04"
tags: []
umbraco_id: 1562
parent_id: 1561
sort_order: 0
aliases:
  - "/em/maxwell"
  - "/em/maxwell.html"
  - "/physics/em/maxwell/"
  - "/study/em/maxwell"
  - "/study/em/maxwell.html"
---

# マクスウェルの方程式

## <a id="sec-generated-title-1"></a> <a id="maxwell"></a>マクスウェルの方程式

<table summary="">

	<tr>
		<th>名前</th>
		<th>積分形 / 微分形 / 不連続面での境界条件</th>
		<th>式の意味</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">ファラデー・マクスウェルの法則</td>
		<td markdown="1"><span class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
            <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">l</span> = <span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span></td>
		<td markdown="1" rowspan="3"><span class="math">
            <a href="variable.md#voltage" title="電圧">V</a> = <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">l</span>
          </span>…曲面<span class="math">S</span>の外周<span class="math">C</span>上に生じた起電力<br></br><span class="math">
            <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span>…曲面<span class="math">S</span>を貫く磁束<br></br><span class="math">
            <a href="variable.md#voltage" title="電圧">V</a> = <span class="normal">−</span> <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a>
          </span><br></br>閉路上に生じる起電力はその閉路を貫く磁束の変化に等しい</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a><span class="normal">×</span><span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a><sub>1</sub>
              <span class="normal">−</span> <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = 0
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">アンペア・マクスウェルの法則</td>
		<td markdown="1"><span class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
            <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">l</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span></td>
		<td markdown="1" rowspan="3"><span class="math">
            <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">l</span>
          </span>…曲面<span class="math">S</span>の外周<span class="math">C</span>上に生じた起磁力<br></br><span class="math">
            <a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span>…曲面<span class="math">S</span>を貫く電束<br></br><span class="math">
            I = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span>…曲面<span class="math">S</span>を貫く電流<br></br><span class="math">
            <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = I <span class="normal">+</span> <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a>
          </span><br></br>電流が流れるか、電束が変化するとその周囲に磁界が発生する</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a> <span class="normal">+</span> <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a><span class="normal">×</span><span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a><sub>1</sub>
              <span class="normal">−</span>
              <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = <a href="variable.md#K" title="面電流ベクトル"><span class="vector">K</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">電束に関するガウスの法則</td>
		<td markdown="1"><span class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table><a href="variable.md#rho" title="電荷密度">ρ</a><span class="normal">⋅</span><span class="normal">d</span>V
          </span></td>
		<td markdown="1" rowspan="3"><span class="math">
            <a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a> = <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span>…閉曲面<span class="math">S</span>を貫く電束<br></br><span class="math">
            Q = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table><a href="variable.md#rho" title="電荷密度">ρ</a><span class="normal">⋅</span><span class="normal">d</span>V
          </span>…閉曲面<span class="math">S</span>の内部<span class="math">V</span>にある電荷の総和<br></br><span class="math">
            <a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a> = Q
          </span><br></br>平曲面を貫く電束はその内部にある電化の総和に等しい</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#rho" title="電荷密度">ρ</a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a><span class="normal">⋅</span><span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><sub>1</sub>
              <span class="normal">−</span>
              <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = <a href="variable.md#xi" title="グザイ">ξ</a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">磁束に関するガウスの法則</td>
		<td markdown="1"><span class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table>
            <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span> = 0
          </span></td>
		<td markdown="1" rowspan="3"><span class="math">
            <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><span class="normal">⋅</span><span class="normal">d</span><span class="vector">S</span>
          </span>…閉曲面<span class="math">S</span>を貫く磁束<br></br><br></br><span class="math">
            <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = 0
          </span><br></br>平曲面を貫く磁束は常に0</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = 0
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <a href="variable.md#n" title="法線ベクトル"><span class="vector">n</span></a><span class="normal">⋅</span><span class="paren" style="font-size:em;">(</span>
              <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><sub>1</sub>
              <span class="normal">−</span>
              <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a><sub>2</sub>
            <span class="paren" style="font-size:em;">)</span> = 0
          </span></td>
	</tr>
</table>
