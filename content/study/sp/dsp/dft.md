---
title: "離散フーリエ変換"
source_url: "https://ufcpp.net/study/sp/dsp/dft/"
content_type: "Article"
published_at: "2015-05-06T14:21:54"
updated_at: "2015-07-07T18:49:39"
tags: []
umbraco_id: 1602
parent_id: 1599
sort_order: 2
aliases:
  - "/dsp/dft"
  - "/dsp/dft.html"
  - "/sp/dsp/dft/"
  - "/study/dsp/dft"
  - "/study/dsp/dft.html"
---

# 離散フーリエ変換

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

アナログとディジタルの違いを大雑把に説明すると、

* アナログは連続量を取り扱う

* ディジタルは離散量を取り扱う


となります。
 
ここでは、連続関数と離散関数の間の関係および離散関数に対するフーリエ変換（離散フーリエ変換）について説明します。


## <a id="sec-generated-title-2"></a> <a id="periodic"></a>周期関数のフーリエ変換

「[フーリエ変換](fourier.md#transform)」では、
非周期関数を、「関数の周期<span class="math">T</span>を<span class="math">T→∞</span>としたものである」とみなすことで、
「[フーリエ級数展開](fourierseries.md#series)」を拡張し、「[フーリエ変換](fourier.md#f-trans)」を導き出しました。
これとは逆、すなわち、フーリエ変換の式に周期関数を代入することでフーリエ級数展開の式を導き出すことを考えてみます。
 
それでは早速、周期<span class="math">T</span>を持つ関数、すなわち、<span class="math">f<span class="paren" style="font-size:em;">(</span>t＋T<span class="paren" style="font-size:em;">)</span> ＝ f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>を満たす関数<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>に対してフーリエ変換を行なってみましょう。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t ＝

<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> <span class="paren" style="font-size:em;">(</span>k+1<span class="paren" style="font-size:em;">)</span>T</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">kT</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>t＋kT<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div>
この式中の<span class="math">t</span>を<span class="math">t＋kT</span>と置いて変数変換すると以下のようになります。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt＋iωkT<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div>
さらに、
<span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωkT<span class="paren" style="font-size:1em;">)</span></span>を積分の前に括りだすことで以下の式が得られます。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<span class="paren" style="font-size:2.5em;">(</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωkT<span class="paren" style="font-size:1em;">)</span><span class="paren" style="font-size:2.5em;">)</span>
×
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div>
ここで、
<span class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωkT<span class="paren" style="font-size:1em;">)</span></span>は、<span class="math">ω＝<table class="frac" summary="fraction"><tr><td class="num">2πn</td></tr><tr><td>T</td></tr></table></span>（<span class="math">n</span>は整数）のときに∞、それ以外の時には0となるような関数になります。
すなわち、「[δ関数](../../math/distribution/distribution-e_distribution.md#delta)」を用いて以下のように表すことが出来ます。
（詳しくは、「[δ関数級数](appendix.md#delta-series)」参照。）
<div class="math">
      <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table>
      <span class="normal">exp</span>
      <span class="paren" style="font-size:1em;">(</span>iωkT<span class="paren" style="font-size:1em;">)</span> ＝
ω<sub>0</sub><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div>
ただし、<span class="math">ω<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T</td></tr></table></span>です。
 
また、<span class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t</span>は、<span class="math">ω ＝ nω<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2πn</td></tr><tr><td>T</td></tr></table></span>のとき、「[複素形フーリエ級数展開](fourierseries.md#complexfourier)」におけるフーリエ係数<span class="math">F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></span>に<span class="math">T</span>を掛けたものと一致します。
 
以上のことから、周期関数に対するフーリエ変換の式は以下のように書き換えることが出来ます。
      <div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
T F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>×ω<sub>0</sub> δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span>
＝
2π<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>×δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div>

このように、周期関数に対するフーリエ変換<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>の結果は、
<span class="math">ω ＝ nω<sub>0</sub></span>（<span class="math">n</span>は整数）という離散的な点でしか値を持たず、
その値はフーリエ級数展開係数<span class="math">F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></span>にδ関数を掛けたものになります。


## <a id="sec-generated-title-3"></a> <a id="discrete"></a>離散関数のフーリエ変換

「[周期関数のフーリエ変換](#periodic)」の結果から、離散関数と連続関数はδ関数を用いて関係付けることで、離散関数に対するフーリエ変換が定義可能ではないかという類推が出来ます。
すなわち、離散関数<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>に対して、
<span class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ 
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>
×δ<span class="paren" style="font-size:em;">(</span>t － kT<sub>s</sub><span class="paren" style="font-size:em;">)</span></span>
と置いて、この連続関数<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>をフーリエ変換したものを、
離散関数<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>のフーリエ変換として定義します。

「[フーリエ変換](fourier.md#f-trans)」の式にこの<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>を代入した結果は以下のようになります。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>δ<span class="paren" style="font-size:em;">(</span>t － kT<sub>s</sub><span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table><span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></div>
この<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>は周期<span class="math">ω<sub>s</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T<sub>s</sub></td></tr></table></span>を持つ周期関数になっています。
そのため、逆変換の式は以下のようになります。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2π</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> <span class="paren" style="font-size:em;">(</span>n+1<span class="paren" style="font-size:em;">)</span>ω<sub>s</sub></td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">nω<sub>s</sub></td></tr></table>
F<span class="paren" style="font-size:em;">(</span>ω＋nω<sub>s</sub><span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>ω
</div><div class="math">　
＝
<span class="paren" style="font-size:2.5em;">(</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>inω<sub>s</sub>t<span class="paren" style="font-size:1em;">)</span><span class="paren" style="font-size:2.5em;">)</span>
×
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ω<sub>s</sub></td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>ω
</div><div class="math">　
＝
<span class="paren" style="font-size:2.5em;">(</span>
T<sub>s</sub><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>t － kT<sub>s</sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2.5em;">)</span>
×
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ω<sub>s</sub></td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>ω
</div>
以上のことをまとめると、離散関数<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>のフーリエ変換は以下のようになります。
<em>
      <div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></div>
      <div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">T<sub>s</sub></td></tr><tr><td>2π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ω<sub>s</sub></td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>ω
</div>
    </em>

## <a id="sec-generated-title-4"></a> <a id="discrete"></a>離散フーリエ変換

これまでの話から、
周期関数のフーリエ変換は離散関数になり、
離散関数のフーリエ変換は周期関数になるということがいえます。
となると、周期離散関数のフーリエ変換は周期離散関数になるということを容易に察することが出来ると思います。
この様子を以下の図に示します。ただし、<span class="math">ω<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T</td></tr></table></span>、<span class="math">ω<sub>s</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T<sub>s</sub></td></tr></table></span>です。


<figure>

[![周期関数のフーリエ変換](../../../../assets/media/ufcpp2000/sp/dft01.png)](../../../../assets/media/ufcpp2000/sp/dft01.png)

<figcaption>周期関数のフーリエ変換</figcaption>
</figure>



<figure>

[![離散関数のフーリエ変換](../../../../assets/media/ufcpp2000/sp/dft02.png)](../../../../assets/media/ufcpp2000/sp/dft02.png)

<figcaption>離散関数のフーリエ変換</figcaption>
</figure>



<figure>

[![周期離散関数のフーリエ変換](../../../../assets/media/ufcpp2000/sp/dft03.png)](../../../../assets/media/ufcpp2000/sp/dft03.png)

<figcaption>周期離散関数のフーリエ変換</figcaption>
</figure>



この周期離散関数のフーリエ変換の公式を導き出して見ましょう。
離散関数<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>が周期<span class="math">N</span>を持つ、
すなわち、<span class="math">f<span class="paren" style="font-size:em;">[</span>k ＋ N<span class="paren" style="font-size:em;">]</span> ＝ f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>が成り立つとき、「[離散関数のフーリエ変換 離散フーリエ変換](#discrete)」の結果から、この関数のフーリエ変換は以下のようになります。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></div><div class="math">　
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ －∞</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k ＋ nN<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－i<span class="paren" style="font-size:em;">(</span>k ＋ nN<span class="paren" style="font-size:em;">)</span>T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></div><div class="math">　
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ －∞</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span>
×
<span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－inNT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></div>
式中の2つの∑は分離することができ、上式は以下のように変形することができます。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span>
×
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ －∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－inNT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></div>
<span class="math">ω<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>N T<sub>s</sub></td></tr></table></span> 
と置くと、右辺の×以降は以下のように書き換えることができます。
（詳しくは、「[δ関数級数](appendix.md#delta-series)」参照。）
<div class="math">
      <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k=－∞</td></tr></table>
      <span class="normal">exp</span>
      <span class="paren" style="font-size:1em;">(</span>inNT<sub>s</sub><span class="paren" style="font-size:1em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>N T<sub>s</sub></td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div>
また、
<span class="math">ω ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>N T<sub>s</sub></td></tr></table>n ＝ n ω<sub>0</sub></span>とおき、
離散関数<span class="math">F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></span>を
<div class="math">
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－i <table class="frac" summary="fraction"><tr><td class="num">2πkn</td></tr><tr><td>N</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
として定義します。
これらを踏まえて、先ほどの式を変形すると以下のようになります。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>N T<sub>s</sub></td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div>
この式を離散関数の逆フーリエ変換の式に代入することで、
以下の式が得られます。
<div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">T<sub>s</sub></td></tr><tr><td>2π</td></tr></table>
×
<table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>N T<sub>s</sub></td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ω<sub>s</sub></td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>ikT<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>ω
</div><div class="math">　
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>N</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ 0</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<sub>0</sub> kn<span class="paren" style="font-size:1em;">)</span></div><div class="math">　
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>N</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ 0</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>i <table class="frac" summary="fraction"><tr><td class="num">2πkn</td></tr><tr><td>N</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
以上のことをまとめると、以下の式が得られます。
<em>
      <div class="math">
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－i <table class="frac" summary="fraction"><tr><td class="num">2πkn</td></tr><tr><td>N</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
      <div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>N</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ 0</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>i <table class="frac" summary="fraction"><tr><td class="num">2πkn</td></tr><tr><td>N</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
    </em>
この式を<strong id="dft" class="keyword">離散フーリエ変換</strong>と呼びます。


## <a id="sec-generated-title-5"></a> <a id="property"></a>離散フーリエ変換の性質

離散フーリエ変換はフーリエ変換に離散信号を代入し、式変形しただけのものなので、
フーリエ変換と同様に以下のような性質を持っています。


### <a id="sec-generated-title-6"></a> <a id="linear"></a>線形性

<div class="math">
        <span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
a f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＋
b g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
a
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＋
b
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></div>

### <a id="sec-generated-title-7"></a> <a id="timeshift"></a>時間シフト

<div class="math">
        <span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t±T<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
      </div><div class="math">　
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>t±T<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div><div class="math">　
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt±T<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div><div class="math">　
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>±iTω<span class="paren" style="font-size:em;">)</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div><div class="math">　
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>±iTω<span class="paren" style="font-size:em;">)</span><span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></div>

### <a id="sec-generated-title-8"></a> <a id="product"></a>積のフーリエ変換

<div class="math">
f＊g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">l＝0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>l<span class="paren" style="font-size:em;">]</span>
g<span class="paren" style="font-size:em;">[</span>k－l<span class="paren" style="font-size:em;">]</span>
　<span class="normal">（ただし、<span class="math">k－l＜0</span>のとき、<span class="math">g<span class="paren" style="font-size:em;">[</span>k－l<span class="paren" style="font-size:em;">]</span>＝g<span class="paren" style="font-size:em;">[</span>k－l＋N<span class="paren" style="font-size:em;">]</span></span>であるものとする）</span></div><div class="math">
        <span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f＊g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
×
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></div>
畳込み積<span class="math">f＊g</span>の定義の仕方が連続関数のフーリエ変換の場合と微妙に異なっていることに注意してください。


## <a id="sec-generated-title-9"></a> <a id="adc"></a>アナログ信号→ディジタル信号

ここでは、アナログ信号（連続関数で表される）からディジタル信号（離散関数）を得る方法について説明します。


### <a id="sec-generated-title-10"></a> <a id="sampling"></a>標本化

通常、連続関数<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>で表されるアナログ信号を一定周期<span class="math">T<sub>s</sub></span>で<strong id="d20e1330" class="keyword">標本化</strong>（サンプリング: sampling）することでディジタル信号（離散関数<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>）を得ます。
<div class="math">
f<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span> ＝ f<span class="paren" style="font-size:em;">(</span>nT<sub>s</sub><span class="paren" style="font-size:em;">)</span></div>
当然、このような方法でディジタル信号（離散関数）を得ると、
標本点の間の情報が抜け落ちてしまいます。
そのため、一般的にはディジタル信号から元のアナログ信号（連続関数）を復元することはできません。
 
しかしながら、一定の条件下では標本化して得た離散関数から元の連続関数を完全に復元することができます。
以下では、このために必要となる条件について説明していきます。


### <a id="sec-generated-title-11"></a> <a id="sampling"></a>標本化関数

「[離散関数のフーリエ変換 離散フーリエ変換](#discrete)」で説明したように、離散関数と連続関数の間はδ関数を用いて関連付けることができます。
そこで、
連続関数<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>
標本化して得た離散関数<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span>を用いて、
以下のような連続関数<span class="math">f'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>を定義します。
<div class="math">
f'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ －∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
δ<span class="paren" style="font-size:em;">(</span>t － nT<sub>s</sub><span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ －∞</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>nT<span class="paren" style="font-size:em;">)</span>
δ<span class="paren" style="font-size:em;">(</span>t － nT<sub>s</sub><span class="paren" style="font-size:em;">)</span>
＝
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ －∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>t － nT<sub>s</sub><span class="paren" style="font-size:em;">)</span>
＝
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
δ<sub>T<sub>s</sub></sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
このことはすなわち、「標本化とは、「[インパルス列](appendix.md#i-series)」を掛け合わせることに相当する」とみなすことができます。

<figure>

[![標本化](../../../../assets/media/ufcpp2000/sp/dft04.png)](../../../../assets/media/ufcpp2000/sp/dft04.png)

<figcaption>標本化</figcaption>
</figure>


「[インパルス列](appendix.md#i-series)」のフーリエ変換が「[インパルス列](appendix.md#i-series)」となることおよび、
積のフーリエ変換が「[畳込み積](fourier.md#convolution)」となることから、
標本化関数のフーリエ変換<span class="math">F'<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>は以下のようになります。
<div class="math">
F'<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
ω<sub>0</sub>
F＊δ<sub>ω<sub>s</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></div>
ちなみに、<span class="math">T<sub>s</sub></span>を<strong id="d20e1482" class="keyword">標本化周期</strong>、
その逆数<span class="math">f<sub>s</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>T<sub>s</sub></td></tr></table></span>を<strong id="d20e1495" class="keyword">標本化周波数</strong>、
標本化周波数に<span class="math">2π</span>を掛けたもの<span class="math">ω<sub>s</sub> ＝ 2πf<sub>s</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T<sub>s</sub></td></tr></table></span>を<strong id="d20e1518" class="keyword">標本化角周波数</strong>といいます。


### <a id="sec-generated-title-12"></a> <a id="shanonn"></a>シャノンの標本化定理

前節で示したように、関数<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>のフーリエ変換<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>と、それを標本化したもの<span class="math">f'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>のフーリエ変換<span class="math">F’<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>の間には以下のような関係が成り立っています。
<div class="math">
F'<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
ω<sub>0</sub>
F＊δ<sub>ω<sub>s</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></div>
δ関数の性質から、この式は以下のように表すことができます。
<div class="math">
F'<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
ω<sub>s</sub><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ －∞</td></tr></table>
F<span class="paren" style="font-size:em;">(</span>ω － nω<sub>s</sub><span class="paren" style="font-size:em;">)</span></div>
この関数<span class="math">F'<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>は離散化前の関数<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>を<span class="math">ω<sub>s</sub></span>おきに複数ならべたものになっています。
そのため、<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>が非0となるような最大の周波数<span class="math">ω<sub>m</sub></span>（<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>が<span class="math">ω<sub>m</sub></span>以下の周波数成分を含まない）が<span class="math">ω<sub>s</sub></span>の半分より小さい場合（<span class="math">2ω<sub>m</sub>＜ω<sub>s</sub></span>）には、
下図のように、元の関数の形が崩れません。
したがって、低周波数成分のみを通過させるようなフィルタ（ローパスフィルタ）を通すことで、元の連続関数を完全に再現することができます。

<figure>

[![標本化関数のフーリエ変換（低周波）](../../../../assets/media/ufcpp2000/sp/dft05.png)](../../../../assets/media/ufcpp2000/sp/dft05.png)

<figcaption>標本化関数のフーリエ変換（低周波）</figcaption>
</figure>


ところが、<span class="math">ω<sub>m</sub></span>がそれ以上の場合（（<span class="math">2ω<sub>m</sub>≧ω<sub>s</sub></span>））、
下図のように、関数の形に歪みが生じます。
そして、この歪みが原因で元の連続関数を再現することができなくなります。
このような関数形の歪みを<strong id="d20e1654" class="keyword">エイリアシング</strong>（aliasing）と呼びます。

<figure>

[![標本化関数のフーリエ変換（高周波）](../../../../assets/media/ufcpp2000/sp/dft06.png)](../../../../assets/media/ufcpp2000/sp/dft06.png)

<figcaption>標本化関数のフーリエ変換（高周波）</figcaption>
</figure>


このような、
「<span class="math">2ω<sub>m</sub>＜ω<sub>s</sub></span>の場合には、
標本化関数から元の連続関数を完全に再現できる」
という結論は、発見者の名前を取ってシャノンの<strong id="d20e1673" class="keyword">標本化定理</strong>と呼ばれています。


<figure>

[![高周波数の関数を標本化すると](../../../../assets/media/ufcpp2000/sp/dft07.png)](../../../../assets/media/ufcpp2000/sp/dft07.png)

<figcaption>高周波数の関数を標本化すると</figcaption>
</figure>




## <a id="sec-generated-title-13"></a> <a id="summay"></a>まとめ

<table summary="離散フーリエ変換の公式">
	<caption>
		離散フーリエ変換の公式
	</caption>
	<tr>
		<td markdown="1">離散フーリエ変換</td>
		<td markdown="1"><div class="math">
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－i <table class="frac" summary="fraction"><tr><td class="num">2πkn</td></tr><tr><td>N</td></tr></table><span class="paren" style="font-size:2em;">)</span></div><div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>N</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ 0</td></tr></table>
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>i <table class="frac" summary="fraction"><tr><td class="num">2πkn</td></tr><tr><td>N</td></tr></table><span class="paren" style="font-size:2em;">)</span></div></td>
	</tr>
</table>


<table summary="離散フーリエ変換の性質">
	<caption>
		離散フーリエ変換の性質
	</caption>
	<tr>
		<td markdown="1">線形性</td>
		<td markdown="1"><div class="math">
            <span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
a f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＋
b g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span>
            <span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
a
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＋
b
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></div></td>
	</tr>
	<tr>
		<td markdown="1">時間シフト</td>
		<td markdown="1"><div class="math">
            <span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t±T<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
            <span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>±iTω<span class="paren" style="font-size:em;">)</span><span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></div></td>
	</tr>
	<tr>
		<td markdown="1">積のフーリエ変換</td>
		<td markdown="1"><div class="math">
f＊g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">l＝0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>l<span class="paren" style="font-size:em;">]</span>
g<span class="paren" style="font-size:em;">[</span>k－l<span class="paren" style="font-size:em;">]</span></div>（ただし、<span class="math">k－l＜0</span>のとき、<span class="math">g<span class="paren" style="font-size:em;">[</span>k－l<span class="paren" style="font-size:em;">]</span>＝g<span class="paren" style="font-size:em;">[</span>k－l＋N<span class="paren" style="font-size:em;">]</span></span>であるものとする）<div class="math"><span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f＊g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
＝
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span>
×
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></div></td>
	</tr>
</table>


<table summary="標本化定理">
	<caption>
		標本化定理
	</caption>
	<tr>
		<td markdown="1">標本化定理</td>
		<td markdown="1">
「<span class="math">2ω<sub>m</sub>＜ω<sub>s</sub></span>の場合には、
標本化関数から元の連続関数を完全に再現できる」
</td>
	</tr>
</table>
