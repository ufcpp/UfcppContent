---
title: "楕円積分"
source_url: "https://ufcpp.net/study/math/elliptic/integral/"
content_type: "Article"
published_at: "2015-05-06T14:18:14"
updated_at: "2015-05-06T14:18:14"
tags: []
umbraco_id: 1512
parent_id: 1511
sort_order: 0
aliases:
  - "/elliptic/integral"
  - "/elliptic/integral.html"
  - "/math/elliptic/integral/"
  - "/study/elliptic/integral"
  - "/study/elliptic/integral.html"
---

# 楕円積分

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

以下のような形式の積分を考えます。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>φ<span class="paren" style="font-size:em;">(</span>x, √p<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x
</div>
ただし、
<span class="math">φ</span> は有理関数、
<span class="math">p<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> は4次の多項式です。
このような形式の積分の代表例として、
楕円の弧長計算が挙げられます。
そのため、この形式の積分を<strong id="d76e30" class="keyword">楕円積分</strong>（elliptic integral）と呼びます。
 
同じような形式の積分でも、
<span class="math">p</span> が2次の多項式の場合には、
<span class="math">x ＝ <span class="normal">sin</span>θ</span> と置いて変数変換することが出来ます。
 
また、<span class="math">p</span> が3次の多項式の場合には、
<span class="math">p</span> の実根のうちの1つを <span class="math">α</span> として、
<span class="math">x － α ＝ y<sup>2</sup></span> と置いて変数変換することで、
4次の場合に帰着することが出来ます。
 
このような形式の積分は、
<span class="math">p</span> が2次の場合には簡単に解析的に計算できますが、
3次・4次（楕円積分）になると、解析的には解けず、その性質も非常に複雑です。
そのため、楕円積分の性質に関する理論は、それだけで本1つになるほどのものです。


## <a id="sec-generated-title-2"></a> <a id="plan"></a>執筆予定

```text
一般形
式変形を繰り返すと、楕円積分は以下のいずれかの形式に帰着
```
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> u</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x</td></tr><tr><td>
 √<span class="paren" style="font-size:1.5em;">(</span><span class="paren" style="font-size:em;">(</span>1 － x<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>1 － k<sup>2</sup>x<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></td></tr></table>
    </div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> u</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">x<sup>2</sup><span class="normal">d</span>x</td></tr><tr><td>
 √<span class="paren" style="font-size:1.5em;">(</span><span class="paren" style="font-size:em;">(</span>1 － x<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>1 － k<sup>2</sup>x<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></td></tr></table>
    </div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> u</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x</td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>x － a<span class="paren" style="font-size:em;">)</span>
 √<span class="paren" style="font-size:1.5em;">(</span><span class="paren" style="font-size:em;">(</span>1 － x<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>1 － k<sup>2</sup>x<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></td></tr></table>
    </div>
ただし、<span class="math"><span class="normal">|</span>k<span class="normal">|</span> ＜ 1</span>。
 
これらは、<span class="math">x ＝ <span class="normal">sin</span>φ, Δ<span class="paren" style="font-size:em;">(</span>φ<span class="paren" style="font-size:em;">)</span> ＝ √<span class="paren" style="font-size:em;">(</span>1 － k <span class="normal">sin</span><sup>2</sup>φ<span class="paren" style="font-size:em;">)</span></span> と置くと、以下の3つのパターンのいずれかに帰着します。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> φ</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>φ</td></tr><tr><td>Δ<span class="paren" style="font-size:em;">(</span>φ<span class="paren" style="font-size:em;">)</span></td></tr></table>
    </div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> φ</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
Δ<span class="paren" style="font-size:em;">(</span>φ<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>φ
</div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> φ</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>φ</td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>1 ＋ n <span class="normal">sin</span>φ<span class="paren" style="font-size:em;">)</span>
 Δ<span class="paren" style="font-size:em;">(</span>φ<span class="paren" style="font-size:em;">)</span></td></tr></table>
    </div>
これらを上から順に、
第1種不完全楕円積分 <span class="math">F<span class="paren" style="font-size:em;">(</span>φ, k<span class="paren" style="font-size:em;">)</span></span> （incomplete elliptic integral）、
第2種不完全楕円積分 <span class="math">E<span class="paren" style="font-size:em;">(</span>φ, k<span class="paren" style="font-size:em;">)</span></span>、
第3種不完全楕円積分 <span class="math">Π<span class="paren" style="font-size:em;">(</span>φ, k<span class="paren" style="font-size:em;">)</span></span> と呼びます。
 
特に、<span class="math">φ ＝ π/2</span> のとき、完全楕円積分（complete elliptic integral）と呼ぶ。
第1種完全楕円積分は <span class="math">K<span class="paren" style="font-size:em;">(</span>k<span class="paren" style="font-size:em;">)</span> ＝ F<span class="paren" style="font-size:em;">(</span>φ, k<span class="paren" style="font-size:em;">)</span></span> と書き表す。
