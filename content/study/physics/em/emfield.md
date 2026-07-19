---
title: "時間的に変化する電磁場"
source_url: "https://ufcpp.net/study/physics/em/emfield/"
content_type: "Article"
published_at: "2015-05-06T14:20:21"
updated_at: "2015-05-06T14:20:21"
tags: []
umbraco_id: 1569
parent_id: 1561
sort_order: 7
aliases:
  - "/em/emfield"
  - "/em/emfield.html"
  - "/physics/em/emfield/"
  - "/study/em/emfield"
  - "/study/em/emfield.html"
---

# 時間的に変化する電磁場

##<a id="sec-generated-title-1"></a> <a id="dynamic"></a>時間的に変化する電磁場
電荷も電流もない空間上ではマクスウェルの方程式の微分形は以下のようになります。

<table summary="">

	<tr>
		<td markdown="1">(i)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1">(ii)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <a href="variable.md#conductivity" title="電気伝導率">σ</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>+<a href="variable.md#eps" title="物質中の誘電率">ε</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1">(iii)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = 0
          </span></td>
	</tr>
	<tr>
		<td markdown="1">(iv)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = 0
          </span></td>
	</tr>
</table>


ここで、この式を<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>だけ、
もしくは<span class="math">
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
      </span>だけで表すことを考えます。
そのために、まず(i)式の両辺の回転を取ります。
<div class="math">
      <span class="vector">∇</span>×
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">∇</span>×
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      <span class="paren" style="font-size:em;">)</span> = −<a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">∇</span>×<a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
    </div>
ここで、
<span class="math">
        <span class="vector">∇</span>×<span class="paren" style="font-size:em;">(</span>
          <span class="vector">∇</span>×
          <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
        <span class="paren" style="font-size:em;">)</span> = <span class="vector">∇</span><span class="vector">∇</span>・<a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> − <a href="variable.md#Laplace" title="ラプラシアン">Δ</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>
であることおよび、(ii)式、(iii)式を代入すると、
<div class="math">
      <em>
        <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <a href="variable.md#conductivity" title="電気伝導率">σ</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> + <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </em>
    </div>
という関係式が得られます。
同様に、(ii)の回転を取り、(i)式、(iv)式を代入すると、
<div class="math">
      <em>
        <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <a href="variable.md#conductivity" title="電気伝導率">σ</a><a href="variable.md#eps" title="物質中の誘電率">ε</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> + <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
      </em>
    </div>
という関係式が得られます。


##<a id="sec-generated-title-2"></a> <a id="wave"></a>電磁波
特に、<span class="math">
        <a href="variable.md#conductivity" title="電気伝導率">σ</a> = 0
      </span>すなわち、伝導電流が流れないとき、
上式は
<div class="math">
      <em>
        <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </em>
    </div><div class="math">
      <em>
        <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
      </em>
    </div>
となります。
<em>
        この式は<strong id="waveequ" class="keyword">波動方程式</strong>と呼ばれ、速度<span class="math">
          v = <span class="paren" style="font-size:em;">(</span>
            <a href="variable.md#eps" title="物質中の誘電率">ε</a>
            <a href="variable.md#mu" title="物質中の透磁率">μ</a>
          <span class="paren" style="font-size:em;">)</span><sup>−1/2</sup>
        </span>で伝播する波動を表す式です
      </em>。
すなわち、電磁場は波動として空間中を伝播するということになります。
この、電磁場の作る波動を<strong id="wave" class="keyword">電磁波</strong>といいます。

ここで、話を簡単にするために、この微分方程式のもっとも単純な形の解
<div class="math">
      <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <span class="vector">E</span><sub>0</sub><span class="normal">cos</span>(ωt−<span class="vector">k</span><span class="vector">x</span>)
    </div>
を考えます。
ただし、<span class="math">
        <span class="vector">E</span>
        <sub>0</sub>
      </span>は<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>の初期値で、
<span class="math">ω</span>はこの波動の角周波数、<span class="math">
        <span class="vector">k</span>
      </span>は波数で、
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">|</span>
              <span class="vector">k</span>
            <span class="normal">|</span>
            <sup>2</sup>
          </td></tr><tr><td>
            ω<sup>2</sup>
          </td></tr></table> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          v<sup>2</sup>
        </td></tr></table> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a>
      </span>
という関係式が成り立ちます。

この式は、進行波と呼ばれるもので、<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>は常に<span class="math">
        <span class="vector">E</span>
        <sub>0</sub>
      </span>と同じ方向(もしくは、その逆向き)を向き、<span class="math">
        <span class="vector">k</span>
      </span>の方向に速度<span class="math">v</span>で伝播していきます。

さて、このとき、(i)式から磁場<span class="math">
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
      </span>を求めると、
<div class="math">
      <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        <a href="variable.md#mu" title="物質中の透磁率">μ</a>
      </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="vector">∇</span>×<span class="vector">E</span><sub>0</sub><span class="normal">cos</span>(ωt−<span class="vector">k</span><span class="vector">x</span>)<a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>t = −<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        <a href="variable.md#mu" title="物質中の透磁率">μ</a>
      </td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="vector">k</span>×<span class="vector">E</span><sub>0</sub><span class="normal">sin</span>(ωt−<span class="vector">k</span><span class="vector">x</span>)<a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>t = <table class="frac" summary="fraction"><tr><td class="num">
          <span class="vector">k</span>×<span class="vector">E</span><sub>0</sub>
        </td></tr><tr><td>
          <a href="variable.md#mu" title="物質中の透磁率">μ</a>ω
        </td></tr></table><span class="normal">cos</span>(ωt−<span class="vector">k</span><span class="vector">x</span>)
    </div>
ここで、
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">|</span>
              <span class="vector">k</span>
            <span class="normal">|</span>
            <sup>2</sup>
          </td></tr><tr><td>
            ω<sup>2</sup>
          </td></tr></table> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a>
      </span>
であることを用いると、
<div class="math">
      <em>
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <a href="variable.md#intrinsic_impedance" title="固有インピーダンス">η</a>
        </td></tr></table><span class="vector">i</span><sub>k</sub>×<span class="vector">E</span><sub>0</sub><span class="normal">cos</span>(ωt−<span class="vector">k</span><span class="vector">x</span>)
      </em>
    </div>
という関係式が得られます。
ただし、<span class="math">
        <span class="vector">i</span>
        <sub>k</sub>
      </span>は<span class="math">
        <span class="vector">k</span>
      </span>の方向(すなわち波動の進行方向)を向く単位ベクトルで、<span class="math">
        <a href="variable.md#intrinsic_impedance" title="固有インピーダンス">η</a>
      </span>は<span class="math">
        <a href="variable.md#intrinsic_impedance" title="固有インピーダンス">η</a> = <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              <a href="variable.md#mu" title="物質中の透磁率">μ</a>
            </td></tr><tr><td>
              <a href="variable.md#eps" title="物質中の誘電率">ε</a>
            </td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span><sup>1/2</sup>
      </span>で、<strong id="impedance" class="keyword">固有インピーダンス</strong>と呼ばれています。

同様に
<span class="math">
        <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <span class="vector">H</span><sub>0</sub><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>
          ωt−<span class="vector">k</span><span class="vector">x</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
とおいて、(ii)式から電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>を求めると、
<div class="math">
      <em>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<a href="variable.md#intrinsic_impedance" title="固有インピーダンス">η</a><span class="vector">i</span><sub>k</sub>×<span class="vector">H</span><sub>0</sub><span class="normal">cos</span>(ωt−<span class="vector">k</span><span class="vector">x</span>)
      </em>
    </div>
という関係式が得られます。
そして、この2つの式から<em>
        <span class="math">
          <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>, <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>, <span class="vector">k</span>
        </span>は互いに直行している
      </em>ことが分かります。
