---
title: "付録"
source_url: "https://ufcpp.net/study/sp/dsp/appendix/"
content_type: "Article"
published_at: "2015-05-06T14:22:17"
updated_at: "2015-05-06T14:22:17"
tags: []
umbraco_id: 1609
parent_id: 1599
sort_order: 9
aliases:
  - "/dsp/appendix"
  - "/dsp/appendix.html"
  - "/sp/dsp/appendix/"
  - "/study/dsp/appendix"
  - "/study/dsp/appendix.html"
---

# 付録

## <a id="sec-generated-title-1"></a> <a id="delta-fourier"></a>δ関数のフーリエ変換

δ関数および指数関数のフーリエ変換は以下のようになります。
<div class="math">
      <span class="normal">ℱ</span><span class="paren" style="font-size:em;">[</span>δ<span class="paren" style="font-size:em;">(</span>t ± T<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span>
      <span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>±i Tω<span class="paren" style="font-size:em;">)</span></div><div class="math">
      <span class="normal">ℱ</span><span class="paren" style="font-size:em;">[</span>
        <span class="normal">exp</span>
        <span class="paren" style="font-size:em;">(</span>±i ω<sub>0</sub> t<span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">]</span>
      <span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝
2π δ<span class="paren" style="font-size:em;">(</span>ω ± ω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div>

## <a id="sec-generated-title-2"></a> <a id="delta-series"></a>δ関数級数

離散関数を取り扱う際、以下のようにδ関数を等間隔で並べた級数<span class="math">δ<sub>T</sub></span>がしばしば用いられます。
<div class="math">
δ<sub>T</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝－∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>t － kT<span class="paren" style="font-size:em;">)</span></div>
この級数<span class="math">δ<sub>T</sub></span>は、周期<span class="math">T</span>の周期関数となるので、フーリエ級数展開可能です。
<span class="math">
c<sub>n</sub> ＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table>
δ<sub>T</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>－iω<sub>0</sub>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝ 1
</span>
となるので、
級数<span class="math">δ<sub>T</sub></span>のフーリエ級数展開は以下のようになります。
<div class="math">
δ<sub>T</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝－∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>t － kT<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>T</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝－∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>iω<sub>0</sub> n t<span class="paren" style="font-size:em;">)</span></div>
ただし、<span class="math">ω<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T</td></tr></table></span> です。
 
また、この結果と指数関数のフーリエ変換の公式から、
級数<span class="math">δ<sub>T</sub></span>のフーリエ変換を求めると以下のようになります。
<div class="math">
      <span class="normal">ℱ</span><span class="paren" style="font-size:em;">[</span>
δ<sub>T</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span>
      <span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>T</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝－∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>iω<sub>0</sub> n t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">2π</td></tr><tr><td>T</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝－∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span>
＝
ω<sub>0</sub><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝－∞</td></tr></table>
δ<span class="paren" style="font-size:em;">(</span>ω － nω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div>
信号処理の分野では、δ関数で表される信号を<strong id="impulse" class="keyword">インパルス</strong>、
級数<span class="math">δ<sub>T</sub></span>で表される信号を<strong id="i-series" class="keyword">インパルス列</strong>と呼びます。


## <a id="sec-generated-title-3"></a> <a id="sinc-function"></a>sinc関数

以下のようにして定義された関数<span class="math"><span class="normal">sinc</span> x</span> を <strong id="sinc" class="keyword">sinc</strong> 関数と呼びます。
<div class="math">
      <span class="normal">sinc</span> x ＝ 
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span> x</td></tr><tr><td>x</td></tr></table></div>
sinc 関数は以下のような性質を持ちます。

* <span class="math">
          <span class="normal">sinc</span> 0 ＝ 1</span>

* <span class="math">n</span>を非0整数とすると、<span class="math"><span class="normal">sinc</span> πn ＝ 0</span>

* <span class="math">
          <span class="normal">ℱ</span><span class="paren" style="font-size:em;">[</span>
            <span class="normal">sinc</span> ω<sub>0</sub> t<span class="paren" style="font-size:em;">]</span>
          <span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">1</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">|</span>ω<span class="normal">|</span>≦ω<sub>0</sub></span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">|</span>ω<span class="normal">|</span>＞ω<sub>0</sub></span><span class="paren">)</span></td></tr></table></span>

* <span class="math">
          <span class="normal">sinc</span> ω<sub>0</sub> t
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ω<sub>0</sub></td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－ω<sub>0</sub></td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>iωt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>ω
</span>

* <span class="math">
δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">ω<sub>0</sub>→∞</td></tr></table><span class="normal">sinc</span> ω<sub>0</sub> t
</span>
