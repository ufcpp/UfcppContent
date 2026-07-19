---
title: "電磁ポテンシャル"
source_url: "https://ufcpp.net/study/physics/em/p_potential/"
content_type: "Article"
published_at: "2015-05-06T14:20:24"
updated_at: "2015-05-06T14:20:24"
tags: []
umbraco_id: 1570
parent_id: 1561
sort_order: 8
aliases:
  - "/em/potential"
  - "/em/potential.html"
  - "/physics/em/p_potential/"
  - "/study/em/potential"
  - "/study/em/potential.html"
---

# 電磁ポテンシャル

##<a id="sec-generated-title-1"></a> <a id="potential"></a>電磁ポテンシャル
<table summary="">

	<tr>
		<td markdown="1">(i)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1">(ii)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a> = <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a> + <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>
          </span></td>
	</tr>
	<tr>
		<td markdown="1">(iii)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>・
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#rho" title="電荷密度">ρ</a>
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


(iv)より<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>はソレノイダル場なので、<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>となる<strong id="vectorpotential" class="keyword">ベクトルポテンシャル</strong><span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>が存在します。
これを(i)に代入すると、
<div class="math">
      <span class="vector">∇</span>×
      <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<span class="vector">∇</span>×<span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table>
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      <span class="paren" style="font-size:1.5em;">)</span>
    </div><div class="math">
      <span class="vector">∇</span>×
      <span class="paren" style="font-size:1.5em;">(</span>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> + <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      <span class="paren" style="font-size:1.5em;">)</span> = 0
    </div>
が得られます。
この式より<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> + <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>は保存場になっているので、
<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> + <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>となる<strong id="scalerpotential" class="keyword">スカラーポテンシャル</strong><span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>が存在します。
以上のことをまとめると、電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>および<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>電束密度<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>はスカラーポテンシャル<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>および<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>を用いて
<table class="layout" summary="レイアウト用テーブル">
<tr><td><div class="math">
            <em>
              <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> − <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
            </em>
          </div><div class="math">
            <em>
              <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
            </em>
          </div></td></tr></table>


と表せることが分かります。


##<a id="sec-generated-title-2"></a> <a id="metaphysics"></a>電磁ポテンシャルの物理的意味
スカラーポテンシャル<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>はその勾配が電場となり、
電場は電荷にかかる力です。
位置エネルギーの勾配は物質にはたらく力となりますから、
そのことと対比させて考えると、
<em>
        スカラーポテンシャル<span class="math">
          <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
        </span>は電荷の位置エネルギー
      </em>と考えられます。

同様に、ベクトルポテンシャル<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>はその時間微分が電場となります。
運動量の変化は力積となりますから、
そのことと対比させて考えると、
<em>
        ベクトルポテンシャル<span class="math">
          <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
        </span>は電場中の電荷を運動させたときに
        運動量が変化する量を表します
      </em>。


##<a id="sec-generated-title-3"></a> <a id="gauge"></a>ゲージ変換
<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>
      </span>および<span class="math">
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>に対して、任意関数<span class="math">χ</span>を用いた
<div class="math">
      <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>' = <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> − <span class="vector">∇</span>χ
    </div><div class="math">
      <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>' = <a href="variable.md#phi" title="スカラーポテンシャル">φ</a> + <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table>χ
    </div>
という変換を考えると、
<div class="math">
      −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a>' − <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>' = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a>−<span class="vector">∇</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table>χ−<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>+<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">∇</span>χ = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> − <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
    </div><div class="math">
      <span class="vector">∇</span>×
      <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>' = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>−<span class="vector">∇</span>×<span class="paren" style="font-size:em;">(</span>
        <span class="vector">∇</span>χ
      <span class="paren" style="font-size:em;">)</span> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
    </div>
となります。
すなわち、<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>',<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>'
      </span>は<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>,<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>と同じ<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>,<a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>を与えます。
この<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>,<a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
      </span>を変えない<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>,<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>の変換を<strong id="gauge" class="keyword">ゲージ変換</strong>といいます。

このように、<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>,<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>は任意関数<span class="math">χ</span>の分だけ不定性を持ちます。
そのため、<span class="math">
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a> = 0
      </span>となるように<span class="math">χ</span>を選んだりすることも出来きます。


##<a id="sec-generated-title-4"></a> <a id="lorentz"></a>ローレンツ条件
<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = −<span class="vector">∇</span><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> − <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>および<span class="math">
        <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <span class="vector">∇</span>×<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      </span>を(ii)および(iii)に代入することで
<div class="math">
      <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
      <a href="variable.md#phi" title="スカラーポテンシャル">φ</a> + <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">∇</span>・<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = −<table class="frac" summary="fraction"><tr><td class="num">
          <a href="variable.md#rho" title="電荷密度">ρ</a>
        </td></tr><tr><td>
          <a href="variable.md#eps" title="物質中の誘電率">ε</a>
        </td></tr></table>
    </div><div class="math">
      <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
      <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> − <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> − <span class="vector">∇</span><span class="paren" style="font-size:1.5em;">(</span>
        <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> + <span class="vector">∇</span>・<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
      <span class="paren" style="font-size:1.5em;">)</span> = −<a href="variable.md#mu" title="物質中の透磁率">μ</a><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
    </div>
となります。
ここで、<em>
        <span class="math">
          <a href="variable.md#phi" title="スカラーポテンシャル">φ</a>,<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a>
        </span>を<span class="math">
          <span class="vector">∇</span>・<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> + <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> = 0
        </span>を満たすように選んでやれば
      </em>、
<div class="math">
      <em>
        <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
        <a href="variable.md#phi" title="スカラーポテンシャル">φ</a> − <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> = −<table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#rho" title="電荷密度">ρ</a>
          </td></tr><tr><td>
            <a href="variable.md#eps" title="物質中の誘電率">ε</a>
          </td></tr></table>
      </em>
    </div><div class="math">
      <em>
        <a href="variable.md#Laplace" title="ラプラシアン">Δ</a>
        <a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> − <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table><a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> = −<a href="variable.md#mu" title="物質中の透磁率">μ</a><a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
      </em>
    </div>
という関係式が得られます。
この条件<span class="math">
        <span class="vector">∇</span>・<a href="variable.md#A" title="ベクトルポテンシャル"><span class="vector">A</span></a> + <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#mu" title="物質中の透磁率">μ</a><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#phi" title="スカラーポテンシャル">φ</a> = 0
      </span>を<strong id="lorentzcond" class="keyword">ローレンツ条件</strong>といい、
ローレンツ条件が満たされるようにゲージ変換することを<strong id="lorentzguage" class="keyword">ローレンツ・ゲージ変換</strong>といいます。
