---
title: "保存場とソレノイダル場"
source_url: "https://ufcpp.net/study/math/vector_analysis/conservation/"
content_type: "Article"
published_at: "2015-05-06T14:17:48"
updated_at: "2015-05-06T14:17:48"
tags: []
umbraco_id: 1499
parent_id: 1491
sort_order: 7
aliases:
  - "/math/vector_analysis/conservation/"
  - "/study/vector_analysis/conservation"
  - "/study/vector_analysis/conservation.html"
  - "/vector_analysis/conservation"
  - "/vector_analysis/conservation.html"
---

# 保存場とソレノイダル場

##<a id="sec-generated-title-1"></a> <a id="conservation"></a>保存場とソレノイダル場
<em>
        回転が0のベクトル場を<strong id="conservative" class="keyword">保存場</strong>といい、発散が0のベクトル場を<strong id="solenoidal" class="keyword">ソレノイダル場</strong>といいます。
      </em>

回転が0のベクトル場を保存場と呼ぶのは、閉路上の線積分が必ず0になり、<strong id="conservation" class="keyword">エネルギー保存則</strong>が成り立つからです。
また、発散が0のベクトル場をソレノイダル場と呼ぶのは、ベクトル場の流線が渦を巻いたような形状をしているからです。
以下に保存場およびソレノイダル場の特徴を挙げます。

<table summary="保存場とソレノイダル場">
	<caption>
		保存場とソレノイダル場
	</caption>
	<tr>
		<th>保存場</th>
		<th>ソレノイダル場</th>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <span class="vector">F</span> = 0
          </span>(回転が常に0)</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>・
            <span class="vector">F</span> = 0
          </span>(発散が常に0)</td>
	</tr>
	<tr>
		<td markdown="1">任意の閉路<span class="math">C</span>に対して<span class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">l</span> = 0
          </span>（閉路上の線積分が常に0)</td>
		<td markdown="1">任意の閉曲面<span class="math">S</span>に対して<span class="math">
            <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table><span class="vector">F</span>・<span class="normal">d</span><span class="vector">S</span> = 0
          </span>(平曲面を貫く流束が常に0)</td>
	</tr>
	<tr>
		<td markdown="1">線積分の値は経路によらず、その始点と終点のみによって決まる</td>
		<td markdown="1">面積分の値は曲面の取り方によらず、曲面の外周の形状のみによって決まる</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            −<span class="vector">∇</span>φ = <span class="vector">F</span>
          </span>となるスカラー場<span class="math">
            φ
          </span>(<strong id="scaler" class="keyword">スカラーポテンシャル</strong>)が必ず存在する</td>
		<td markdown="1"><span class="math">
            <span class="vector">∇</span>×
            <span class="vector">A</span> = <span class="vector">F</span>
          </span>となるベクトル場<span class="math">
            <span class="vector">A</span>
          </span>(<strong id="vector" class="keyword">ベクトルポテンシャル</strong>)が必ず存在する</td>
	</tr>
</table>


ちなみに、任意のベクトル場は発散が0の部分と回転が0の部分に分割することが出来ます。
すなわち、任意のベクトル場<span class="math">
        <span class="vector">F</span>
      </span>は
<span class="math">
        <span class="vector">∇</span>×<span class="vector">F</span><sub>1</sub> = 0
      </span>を満たすベクトル場<span class="math">
        <span class="vector">F</span>
        <sub>1</sub>
      </span>と、<span class="math">
        <span class="vector">∇</span>・<span class="vector">F</span><sub>2</sub> = 0
      </span>を満たすベクトル場<span class="math">
        <span class="vector">F</span>
        <sub>2</sub>
      </span>を用いて
<div class="math">
      <span class="vector">F</span> = <span class="vector">F</span><sub>1</sub> + <span class="vector">F</span><sub>2</sub>
    </div>
と言う風に分割できます。
これは言い換えると、<span class="math">
        <span class="vector">F</span>
      </span>はあるスカラー場<span class="math">φ</span>とあるベクトル場<span class="math">
        <span class="vector">A</span>
      </span>を用いて、
<div class="math">
      <span class="vector">F</span> = −<span class="vector">∇</span>φ + <span class="vector">∇</span>×<span class="vector">A</span>
    </div>
とあらわすことが出来るということです。
