---
title: "磁気回路"
source_url: "https://ufcpp.net/study/physics/em/circuit/"
content_type: "Article"
published_at: "2015-05-06T14:20:16"
updated_at: "2015-05-06T14:20:16"
tags: []
umbraco_id: 1567
parent_id: 1561
sort_order: 5
aliases:
  - "/study/em/circuit.html"
---

# 磁気回路

## <a id="sec-generated-title-1"></a> <a id="elecircuit"></a>電気回路

本題の磁気回路に入る前にまずは電気回路について簡単にまとめます。

直流電流回路では、
一様な導体中の電場の大きさは一定である(<span class="math">
        <a href="variable.md#voltage" title="電圧">V</a> = El, <a href="variable.md#current" title="電流">I</a> = JS
      </span>)ことと、
各点における電流はその点における電場の強さに比例する(<span class="math">
        <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a> = <a href="variable.md#conductivity" title="電気伝導率">σ</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>)ことを仮定すると、
<div class="math">
      <em>
        <a href="variable.md#current" title="電流">I</a> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#conductivity" title="電気伝導率">σ</a>S
          </td></tr><tr><td>l</td></tr></table><a href="variable.md#voltage" title="電圧">V</a>
      </em>
    </div>
という関係が成り立ちます。
ただし、<span class="math">S</span>は導線の断面積、<span class="math">l</span>は導線の一周の長さ、<span class="math">
        E = <span class="normal">|</span>
          <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
        <span class="normal">|</span>
      </span>は磁場の大きさ、<span class="math">
        J = <span class="normal">|</span>
          <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
        <span class="normal">|</span>
      </span>は電流密度の大きさです。

<em>
        この<span class="math">
          <table class="frac" summary="fraction"><tr><td class="num">
              <a href="variable.md#conductivity" title="電気伝導率">σ</a>S
            </td></tr><tr><td>l</td></tr></table>
        </span>をコンダクタンス(伝導度)(<span class="math">
          <a href="variable.md#conductance" title="コンダクタンス">G</a>
        </span>)といい、その逆数を電気抵抗(<span class="math">
          <a href="variable.md#registance" title="電気抵抗">R</a>
        </span>)といいます
      </em>。
そして、<span class="math">
        <a href="variable.md#voltage" title="電圧">V</a> = <a href="variable.md#registance" title="電気抵抗">R</a><a href="variable.md#current" title="電流">I</a>
      </span>をオームの法則といいます。

次に、定常状態にあるコンデンサでは、
一様な誘電体中の電場の大きさは一定である(<span class="math">
        <a href="variable.md#voltage" title="電圧">V</a> = El, <a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a> = DS
      </span>)ことと、
各点における電束密度はその点における電場の強さに比例する(<span class="math">
        <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>)ことを仮定すると、
<span class="math">
        <a href="variable.md#charge" title="点電荷">Q</a> = <a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a>
      </span>が成り立っているので、
<div class="math">
      <em>
        <a href="variable.md#charge" title="点電荷">Q</a> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#eps" title="物質中の誘電率">ε</a>S
          </td></tr><tr><td>l</td></tr></table><a href="variable.md#voltage" title="電圧">V</a>
      </em>
    </div>
という関係が成り立ちます。
ただし、<span class="math">S</span>は導線の断面積、<span class="math">l</span>は導線の一周の長さ、<span class="math">
        E = <span class="normal">|</span>
          <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
        <span class="normal">|</span>
      </span>は磁場の大きさ、<span class="math">
        D = <span class="normal">|</span>
          <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>
        <span class="normal">|</span>
      </span>は電束密度の大きさ、<span class="math">
        <a href="variable.md#charge" title="点電荷">Q</a>
      </span>はコンデンサに蓄えられた電荷の量です。

<em>
        この<span class="math">
          <table class="frac" summary="fraction"><tr><td class="num">
              <a href="variable.md#eps" title="物質中の誘電率">ε</a>S
            </td></tr><tr><td>l</td></tr></table>
        </span>を電気容量(<span class="math">
          <a href="variable.md#capacitance" title="電気容量">C</a>
        </span>)といいます
      </em>。


## <a id="sec-generated-title-2"></a> <a id="magcircuit"></a>磁気回路

環状の磁性体にコイルを巻き、コイルに電流を流すと、磁性体中に磁場が発生します。
このとき、コイルの巻き数を <span class="math">N</span>、コイルに流れる電流の強さを <span class="math">
        <a href="variable.md#current" title="電流">I</a>
      </span> とすると、
アンペアマクスウェルの法則から
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>・<span class="normal">d</span><span class="vector">l</span> = N<a href="variable.md#current" title="電流">I</a>
    </div>
という関係式が成り立ちます。
一様な磁性体中では磁場の強さは一定だと仮定すると、この式は
<div class="math">
      Hl = N<a href="variable.md#current" title="電流">I</a>
    </div>
ただし、<span class="math">
        H=<span class="normal">|</span>
          <a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
        <span class="normal">|</span>
      </span>は磁場の強さで、
<span class="math">l</span>は環状磁性体の一周の長さです。
また、磁場が一様という仮定の基では磁性体の断面を貫く磁束<span class="math">
        <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a>
      </span>は
<div class="math">
      <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = BS
    </div>
となります。
ただし、<span class="math">
        B = <span class="normal">|</span>
          <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a>
        <span class="normal">|</span>
      </span>は磁束密度の大きさで、<span class="math">
        B = <a href="variable.md#mu" title="物質中の透磁率">μ</a>H
      </span>がなりたちます。
また、<span class="math">S</span>は磁性体の断面積です。
これらの式をあわせると、
<div class="math">
      <em>
        <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#mu" title="物質中の透磁率">μ</a>S
          </td></tr><tr><td>l</td></tr></table>・N<a href="variable.md#current" title="電流">I</a>
      </em>
    </div>
となります。

この関係式はオームの法則の式によく似ています。そこで、起磁力<span class="math">
        <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a>
      </span>および磁気抵抗(リラクタンス:気が向かないこと、嫌気という意味。)<span class="math">
        <a href="variable.md#reluctance" title="磁気抵抗">R<sub>m</sub></a>
      </span>、パーミアンス(浸透するという意味。磁気浸透度とでも訳してもいいかも。)<span class="math">
        <a href="variable.md#permeance" title="パーミアンス">Λ</a>
      </span>を
<div class="math">
      <em>
        <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = N<a href="variable.md#current" title="電流">I</a>
      </em>
    </div><div class="math">
      <em>
        <a href="variable.md#reluctance" title="磁気抵抗">R<sub>m</sub></a> = <table class="frac" summary="fraction"><tr><td class="num">l</td></tr><tr><td>
            <a href="variable.md#mu" title="物質中の透磁率">μ</a>S
          </td></tr></table>
      </em>
    </div><div class="math">
      <em>
        <a href="variable.md#permeance" title="パーミアンス">Λ</a> = <table class="frac" summary="fraction"><tr><td class="num">
            <a href="variable.md#mu" title="物質中の透磁率">μ</a>S
          </td></tr><tr><td>l</td></tr></table>
      </em>
    </div>
と定義すると、
<em>
        <span class="math">
          <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = <a href="variable.md#reluctance" title="磁気抵抗">R<sub>m</sub></a><a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a>, <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = <a href="variable.md#permeance" title="パーミアンス">Λ</a><a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a>
        </span>
      </em>
という関係式が成り立つ


## <a id="sec-generated-title-3"></a> <a id="compare"></a>電気回路と磁気回路の対比

<table summary="">

	<tr>
		<td markdown="1"></td>
		<th>電気回路(抵抗)</th>
		<th>電気回路(コンデンサ)</th>
		<th>磁気回路</th>
	</tr>
	<tr>
		<td markdown="1">起電力/起磁力</td>
		<td markdown="1"><div class="math">
            <a href="variable.md#voltage" title="電圧">V</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#voltage" title="電圧">V</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = N<a href="variable.md#current" title="電流">I</a>
          </div></td>
	</tr>
	<tr>
		<td markdown="1">電流/電束/磁束</td>
		<td markdown="1"><div class="math">
            <a href="variable.md#current" title="電流">I</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#charge" title="点電荷">Q</a>=<a href="variable.md#Phi_e" title="電束">Φ<sub>e</sub></a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a>
          </div></td>
	</tr>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#voltage" title="電圧">V</a> = <a href="variable.md#registance" title="電気抵抗">R</a><a href="variable.md#current" title="電流">I</a>
          </div></td>
		<td markdown="1"></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = <a href="variable.md#reluctance" title="磁気抵抗">R<sub>m</sub></a><a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a>
          </div></td>
	</tr>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#current" title="電流">I</a> = <a href="variable.md#conductance" title="コンダクタンス">G</a><a href="variable.md#voltage" title="電圧">V</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#charge" title="点電荷">Q</a> = <a href="variable.md#capacitance" title="電気容量">C</a><a href="variable.md#voltage" title="電圧">V</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a> = <a href="variable.md#permeance" title="パーミアンス">Λ</a><a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a>
          </div></td>
	</tr>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a> = <a href="variable.md#conductivity" title="電気伝導率">σ</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#B" title="磁束密度ベクトル"><span class="vector">B</span></a> = <a href="variable.md#mu" title="物質中の透磁率">μ</a><a href="variable.md#H" title="磁場ベクトル"><span class="vector">H</span></a>
          </div></td>
	</tr>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#current" title="電流">I</a> = <table class="frac" summary="fraction"><tr><td class="num">
                <a href="variable.md#conductivity" title="電気伝導率">σ</a>S
              </td></tr><tr><td>l</td></tr></table><a href="variable.md#voltage" title="電圧">V</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#charge" title="点電荷">Q</a> = <table class="frac" summary="fraction"><tr><td class="num">
                <a href="variable.md#eps" title="物質中の誘電率">ε</a>S
              </td></tr><tr><td>l</td></tr></table><a href="variable.md#voltage" title="電圧">V</a>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#V_m" title="起磁力">V<sub>m</sub></a> = <table class="frac" summary="fraction"><tr><td class="num">
                <a href="variable.md#mu" title="物質中の透磁率">μ</a>S
              </td></tr><tr><td>l</td></tr></table><a href="variable.md#Phi_m" title="磁束">Φ<sub>m</sub></a>
          </div></td>
	</tr>
	<tr>
		<td markdown="1">伝導度/容量</td>
		<td markdown="1"><div class="math">
            <a href="variable.md#conductance" title="コンダクタンス">G</a> = <table class="frac" summary="fraction"><tr><td class="num">
                <a href="variable.md#conductivity" title="電気伝導率">σ</a>S
              </td></tr><tr><td>l</td></tr></table>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#capacitance" title="電気容量">C</a> = <table class="frac" summary="fraction"><tr><td class="num">
                <a href="variable.md#eps" title="物質中の誘電率">ε</a>S
              </td></tr><tr><td>l</td></tr></table>
          </div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#permeance" title="パーミアンス">Λ</a> = <table class="frac" summary="fraction"><tr><td class="num">
                <a href="variable.md#mu" title="物質中の透磁率">μ</a>S
              </td></tr><tr><td>l</td></tr></table>
          </div></td>
	</tr>
	<tr>
		<td markdown="1">抵抗</td>
		<td markdown="1"><div class="math">
            <a href="variable.md#registance" title="電気抵抗">R</a> = <table class="frac" summary="fraction"><tr><td class="num">l</td></tr><tr><td>
                <a href="variable.md#conductivity" title="電気伝導率">σ</a>S
              </td></tr></table>
          </div></td>
		<td markdown="1"><div class="math"></div></td>
		<td markdown="1"><div class="math">
            <a href="variable.md#reluctance" title="磁気抵抗">R<sub>m</sub></a> = <table class="frac" summary="fraction"><tr><td class="num">l</td></tr><tr><td>
                <a href="variable.md#mu" title="物質中の透磁率">μ</a>S
              </td></tr></table>
          </div></td>
	</tr>
</table>
