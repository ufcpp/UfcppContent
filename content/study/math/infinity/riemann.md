---
title: "リーマン球面"
source_url: "https://ufcpp.net/study/math/infinity/riemann/"
content_type: "Article"
published_at: "2015-05-06T14:17:59"
updated_at: "2015-05-06T14:17:59"
tags: []
umbraco_id: 1504
parent_id: 1500
sort_order: 3
aliases:
  - "/study/infinity/riemann.html"
---

# リーマン球面

## <a id="sec-generated-title-1"></a> <a id="riemann"></a>リーマン球面

リーマン球面という考え方を用いて、
複素数に ∞ を付け加えることができます。
リーマン球面の概要を説明すると、以下のようになります（図1）。

* 三次元空間上の<span class="math">x-y</span>平面を複素数平面とみなす。

* 三次元空間上の点<span class="math"><span class="paren" style="font-size:em;">(</span>0, 0, 1/2<span class="paren" style="font-size:em;">)</span></span>を中心とする直径 1 の球面を<span class="math">S</span>とする。

* 球面の上端の点<span class="math">n ＝ <span class="paren" style="font-size:em;">(</span>0, 0, 1<span class="paren" style="font-size:em;">)</span></span>をこの球面の北極点と呼ぶ。

* 北極点<span class="math">n</span>と点<span class="math">p ＝ <span class="paren" style="font-size:em;">(</span>x, y, z<span class="paren" style="font-size:em;">)</span></span>を通る直線と、複素数平面の交点を<span class="math">α ＝ u ＋ iv ＝ <span class="paren" style="font-size:em;">(</span>u, v, 0<span class="paren" style="font-size:em;">)</span></span>とする。

* すると、点<span class="math">p</span>と点<span class="math">α</span>は1対1に対応するので、球面上の点<span class="math">p</span>によって複素数を表すことが出来る。

* 北極点<span class="math">n</span>に対応する複素数はないが、この点を∞に相当する点として複素数に加える。


<figure>

[![リーマン球面](../../../../assets/media/ufcpp2000/math/riemann0.emf)](../../../../assets/media/ufcpp2000/math/riemann0.emf)

<figcaption>リーマン球面</figcaption>
</figure>


ここで出てきた球面を<strong id="riemann_sphere" class="keyword">リーマン球面</strong>（Riemann sphere：リーマンはドイツの数学者の名前）と呼びます。
複素数をリーマン球面上の点として考えることで、
複素数に∞に相当する値を付け加えることが出来ます（しかも、空間的に連続に）。
 
このようにして、複素数 <span class="math"><span class="bold">C</span></span> に∞を付け加えたものを拡張複素数と呼び、
<span class="math"><span class="bold">C</span><sup>*</sup></span> とか
<span class="math"><span class="bold">C</span></span> の上にハット（^）を付けたもので表します。
<div class="math">
      <span class="bold">C</span>
      <sup>*</sup>
＝
<span class="bold">C</span>
∪
<span class="paren" style="font-size:em;">{</span>∞<span class="paren" style="font-size:em;">}</span></div>
この定義による∞は、
北極点で定義されることからも明らかなように、
ただ1つの元になります。
すなわち、
∞は、
0 と同じように、
偏角を持っていません。
当然、±の区別も付きません（－∞ ＝ ∞）。
 
余談ですが、
このリーマン球面の考え方のように、ある空間を1次元高い次元から“見下ろす”ことによって、
数学としての表現の幅が広がることがよくあります。


## <a id="sec-generated-title-2"></a> <a id="d70e124"></a>∞の逆数

リーマン球面上に、図2で示すような座標
<span class="math"><span class="paren" style="font-size:em;">(</span>
φ, θ
<span class="paren" style="font-size:em;">)</span></span>
を導入します。

<figure>

[![リーマン球面上の座標 (φ, θ)](../../../../assets/media/ufcpp2000/math/riemann1.emf)](../../../../assets/media/ufcpp2000/math/riemann1.emf)

<figcaption>リーマン球面上の座標 (φ, θ)</figcaption>
</figure>


すると、
複素平面状の点 <span class="math">α</span> は、
<div class="math">
α ＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>i φ<span class="paren" style="font-size:em;">)</span><span class="normal">tan</span>θ
</div>
と表されます。
そして、この逆数<span class="math">α<sup>－1</sup></span>は
<div class="math">
α<sup>－1</sup>
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>－i φ<span class="paren" style="font-size:em;">)</span><span class="normal">cot</span>θ
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>－i φ<span class="paren" style="font-size:em;">)</span><span class="normal">tan</span><span class="paren" style="font-size:em;">(</span>π/2 － θ<span class="paren" style="font-size:em;">)</span></div>
となります。
したがって、
座標
<span class="math"><span class="paren" style="font-size:em;">(</span>φ, θ<span class="paren" style="font-size:em;">)</span></span>
で表される点の逆数の座標は
<span class="math"><span class="paren" style="font-size:em;">(</span>－φ, π/2 － θ<span class="paren" style="font-size:em;">)</span></span>
となります。
 
ところで、この座標を用いた表現では、
複素数平面状の0（＝リーマン球面上の南極点）の座標は
<span class="math"><span class="paren" style="font-size:em;">(</span>φ, 0<span class="paren" style="font-size:em;">)</span></span>
（<span class="math">φ</span> は任意）で、
∞（＝リーマン球面上の北極点）の座標は
<span class="math"><span class="paren" style="font-size:em;">(</span>φ, π/2<span class="paren" style="font-size:em;">)</span></span>
になります。
先ほどの逆数に関するルールと照らし合わせると、
0 の逆数は∞、
∞の逆数は 0、
と言えます。


## <a id="sec-generated-title-3"></a> <a id="d70e207"></a>0 で割っても意味がない

詳しい説明は省きますが、
∞には何をかけてもやはり∞です。
<div class="math">
∀α ∈ <span class="bold">C</span><sup>*</sup> － <span class="paren" style="font-size:em;">{</span>0<span class="paren" style="font-size:em;">}</span>, 
α × ∞
＝
∞ × α
＝
∞
</div>
先ほどの説明の通り、
リーマン球面上では、
∞は 0 の逆数とみなすことができます。
なので、0 で割るというのは、
∞ を掛けると読み替えることが出来るわけです。
ですが、「∞には何をかけても∞」なので、
「0 で割ると何を割っても∞」ということになります。
「何を割ったか」という情報は失われ、
ただ「∞になった」という意味のない結果だけが残ります。


## <a id="sec-generated-title-4"></a> <a id="d70e227"></a>まとめ

リーマン球面という考え方を使うと、
0 の逆数として ∞ を定義できます。
しかしながら、
∞は 0 と同様に特別扱いが必要な数です。
