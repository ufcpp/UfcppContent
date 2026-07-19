---
title: "連続信号と離散信号の比較"
source_url: "https://ufcpp.net/study/sp/dsp/da/"
content_type: "Article"
published_at: "2015-05-06T14:22:14"
updated_at: "2015-05-06T14:22:14"
tags: []
umbraco_id: 1608
parent_id: 1599
sort_order: 8
aliases:
  - "/dsp/da"
  - "/dsp/da.html"
  - "/sp/dsp/da/"
  - "/study/dsp/da"
  - "/study/dsp/da.html"
---

# 連続信号と離散信号の比較

## <a id="sec-generated-title-1"></a> <a id="d26e4"></a>連続信号と離散信号の比較

<table summary="">

	<tr>
		<td markdown="1" width="15%"></td>
		<th width="40%">連続</th>
		<th width="40%">離散</th>
	</tr>
	<tr>
		<td markdown="1">信号の表記</td>
		<td markdown="1">連続信号<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>、<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
		<td markdown="1">離散信号<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>、<span class="math">F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">演算</td>
		<td markdown="1">微分<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>f</td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
＝
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">f<span class="paren" style="font-size:em;">(</span>t ＋ Δt<span class="paren" style="font-size:em;">)</span> － f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></td></tr><tr><td>Δt</td></tr></table></span></td>
		<td markdown="1">差分<span class="math">
Δf<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
f<span class="paren" style="font-size:em;">[</span>k ＋ 1<span class="paren" style="font-size:em;">]</span> － f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span></td>
	</tr>
	<tr>
		<td markdown="1">積分<span class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</span></td>
		<td markdown="1">和分<span class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table> f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span></td>
	</tr>
	<tr>
		<td markdown="1"></td>
		<td markdown="1">遅延<span class="math">
D f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
f<span class="paren" style="font-size:em;">[</span>k － 1<span class="paren" style="font-size:em;">]</span></span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="8">方程式</td>
		<td markdown="1">微分方程式<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＋ a f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ 0
</span></td>
		<td markdown="1">差分方程式<span class="math">
D f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＋ a f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＝ 0
</span></td>
	</tr>
	<tr>
		<td markdown="1">定係数線形微分方程式</td>
		<td markdown="1">定係数線形差分方程式</td>
	</tr>
	<tr>
		<td markdown="1"><div class="math">
            <table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>i</sup>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝ 0
</div></td>
		<td markdown="1"><div class="math">
            <table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub>
D<sup>i</sup>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝ 0
</div></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ A <span class="normal">e</span><sup>x t</sup></span>と置いて、<div class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>i</sup>
A <span class="normal">e</span><sup>x t</sup>
＝
A <span class="normal">e</span><sup>x t</sup><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub>
x<sup>i</sup>
＝ 0
</div><div class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub>
x<sup>i</sup>
＝ 0
</div>となり、代数方程式に帰着。</td>
		<td markdown="1"><span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＝ A x<sup>k</sup></span>と置いて、<div class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub>
D<sup>i</sup>
A x<sup>k</sup>
＝
A x<sup>k</sup><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub>
x<sup>－i</sup>
＝ 0
</div><div class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub"></td></tr></table>
a<sub>i</sub>
x<sup>－i</sup>
＝ 0
</div>となり、代数方程式に帰着。</td>
	</tr>
	<tr>
		<td markdown="1">多元連立1階微分方程式</td>
		<td markdown="1">多元連立1階差分方程式</td>
	</tr>
	<tr>
		<td markdown="1"><div class="math">
            <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
            <span class="vector">f</span>
            <span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<span class="vector">A</span><span class="vector">f</span><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div><span class="math">
            <span class="vector">f</span>
            <span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
          </span>は n 次元縦ベクトル<br></br><span class="math"><span class="vector">A</span></span>は n 次正方行列<br></br></td>
		<td markdown="1"><div class="math">
D <span class="vector">f</span><span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<span class="vector">A</span><span class="vector">f</span><span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></div><span class="math">
            <span class="vector">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>
          </span>は n 次元縦ベクトル<br></br><span class="math"><span class="vector">A</span></span>は n 次正方行列<br></br></td>
	</tr>
	<tr>
		<td markdown="1">↑の解は、<div class="math"><span class="vector">f</span><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">Exp</span><span class="paren" style="font-size:em;">(</span><span class="vector">A</span> t
<span class="paren" style="font-size:em;">)</span><span class="vector">f</span><sub>0</sub></div><span class="math"><span class="vector">f</span><sub>0</sub></span>は n 次元縦ベクトル<br></br><span class="math"><span class="vector">A</span></span>は n 次正方行列</td>
		<td markdown="1">↑の解は、<div class="math"><span class="vector">f</span><span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<span class="vector">A</span><sup>k</sup><span class="vector">f</span><sub>0</sub></div><span class="math"><span class="vector">f</span><sub>0</sub></span>は n 次元縦ベクトル<br></br><span class="math"><span class="vector">A</span></span>は n 次正方行列</td>
	</tr>
	<tr>
		<td markdown="1">ただし、<span class="math"><span class="normal">Exp</span><span class="vector">A</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n!</td></tr></table><span class="vector">A</span><sup>n</sup></span></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">伝達関数解析</td>
		<td markdown="1">ラプラス変換</td>
		<td markdown="1">Z 変換</td>
	</tr>
	<tr>
		<td markdown="1"><div class="math">
F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－st<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div></td>
		<td markdown="1"><div class="math">
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
z<sup>－k</sup></div></td>
	</tr>
	<tr>
		<td markdown="1"><div class="math">
            <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table> → s
</div><div class="math">
            <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table> → <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table></div></td>
		<td markdown="1"><div class="math">
D → z<sup>－1</sup></div></td>
	</tr>
</table>
