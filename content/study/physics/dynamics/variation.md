---
title: "変分学"
source_url: "https://ufcpp.net/study/physics/dynamics/variation/"
content_type: "Article"
published_at: "2007-04-08T00:00:00"
updated_at: "2007-05-01T00:00:00"
tags: []
umbraco_id: 1555
parent_id: 1554
sort_order: 0
aliases:
  - "/dynamics/variation"
  - "/dynamics/variation.html"
  - "/physics/dynamics/variation/"
  - "/study/dynamics/variation"
  - "/study/dynamics/variation.html"
---

# 変分学

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
（書きかけ）
 
「関数の関数」の最小化問題、
すなわち、
関数を与えると値が定まるようなものがあって、
その値を最小にするような関数を求めたい場合があります。
 
例えば、2点間の最短経路を求める問題があります。
空間上の経路というのは関数で表すことができるわけで、
最短経路を求める問題は、
「経路長という値を最小にするような関数を求める」ということになります。
2点間の経路（曲線）を <span class="math">x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> で、
経路長（弧長）を <span class="math">L<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span></span> とあらわすなら、
この <span class="math">L</span> は正に、関数 <span class="math">x</span> の関数。
 
（まあ、まっ平らな空間中では、最短経路は明らかに2点を結ぶ直線です。
でも、曲面上に拘束されてる場合なんかは、複雑な問題になる。
例えば、球面上の場合だったら、2点を結ぶ大円上が最短経路。
）
 
で、こういう、「実関数 → 実数の関数」を汎関数（functional）と呼び、
汎関数の極値問題を変分問題（variation problem）と呼びます。
 
変分問題の取り扱い方に関しては、
かなりしっかりとした理論が出来上がっていて、
<strong id="theory" class="keyword">変分学</strong>などと呼ばれたりします。


##<a id="sec-generated-title-2"></a> <a id="functional"></a>汎関数
改めて書きますが、
「実関数 → 実数の関数」を<strong id="functional" class="keyword">汎関数</strong>（functional）と呼びます。
 
「実関数 → 実数」なら何でもよくて、
極端な例でいうと、
<div class="math">
δ<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span>
x<span class="paren" style="font-size:em;">(</span><span class="normal">0</span><span class="paren" style="font-size:em;">)</span></div>
みたいなのも汎関数です。
 
まあ、これだとあんまり面白くなくて、
実際よく問題になるのは、
以下のように関数 <span class="math">x</span>（とその導関数 <span class="math">x<sup><span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span></sup></span>）の定積分で書かれるタイプ。
<div class="math">
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
f
<span class="paren" style="font-size:1.2em;">(</span>
 x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">2</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 <span class="normal">⋯</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">d</span>t
</div>
このタイプで、一番簡単な例というと、
重み関数 <span class="math">w<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> との内積。
<div class="math">
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
w<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div>
経路長の問題なんかも、この範疇に入る。
<div class="math">
      <span class="cursive">L</span>
      <span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <span class="normal">|</span>
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <span class="normal">d</span>t
</div>

##<a id="sec-generated-title-3"></a> <a id="euler"></a>オイラー・ラグランジュ方程式
端点（<span class="math">x<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>, x<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span></span> の値）が固定された定積分形の汎関数
<div class="math">
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
f
<span class="paren" style="font-size:1.2em;">(</span>
 x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">2</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 <span class="normal">⋯</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">d</span>t
</div><div class="math">
x<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> x<sub>a</sub> , 
x<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> x<sub>b</sub></div>
の極値問題（固定端変分問題）を、微分方程式の問題に帰着。
 
何階までの導関数が含まれてても一般論があるんだけど、
ここでは簡単化のため、
1階導関数まで含むものに限定して考える。
<div class="math">
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
f
<span class="paren" style="font-size:1.2em;">(</span>
 x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">d</span>t
</div>
関数の微分に相当するものを考える。
<div class="math">
δI<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span>
I<span class="paren" style="font-size:em;">[</span>x <span class="normal">+</span> δx<span class="paren" style="font-size:em;">]</span><span class="normal">−</span>
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span></div><div class="math">
　
<span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="paren" style="font-size:1.2em;">(</span>
f
<span class="paren" style="font-size:em;">(</span>
 x <span class="normal">+</span> δx, 
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="normal">+</span> δx<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="normal">−</span>
f
<span class="paren" style="font-size:em;">(</span>
 x, 
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">d</span>t
</div>
これを<strong id="variation" class="keyword">変分</strong>（variation）と呼ぶ。
 
で、積分の中身をテイラー展開して、1次の項まで取ると、
<div class="math">
δI<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span>
δx
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup></denom></td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span>
δx<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:2em;">)</span><span class="normal">d</span>t
</div>
2項目（<span class="math">x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup></span>に関する項）を部分積分して、
<div class="math">
δI<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup></denom></td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span>
δx
<span class="normal">d</span>t

<span class="normal">+</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span>
δx
<span class="paren" style="font-size:2em;">]</span><table class="subsup" summary="sub / sup"><tr><td>b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>a</td></tr></table></div>
端点固定の問題なので、
<span class="math">δx<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> δx<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span></span>
で、
<div class="math">
δI<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup></denom></td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span>
δx
<span class="normal">d</span>t
</div>
「実関数の極値 ＝ 微分が 0」だったのに対して、
「汎関数の極値 ＝ 変分が 0」。
これが任意の <span class="math">δx</span> に対してなりたつには、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup></denom></td></tr></table>
f<span class="paren" style="font-size:em;">(</span>x, x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span></div>
すなわち、
固定端の汎関数
<span class="math">
I<span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
f
<span class="paren" style="font-size:1.2em;">(</span>
 x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">2</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
 <span class="normal">⋯</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">d</span>t
</span>
の極値問題（変分問題）と、
微分方程式
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table>
f
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup></denom></td></tr></table>
f
<span class="normal">=</span><span class="normal">0</span></span>
は同値。
これを、（変分問題に対する）オイラー・ラグランジュ方程式という。


##<a id="sec-generated-title-4"></a> <a id="example"></a>変分問題の例
例:
2点間の最短経路
 
例えば、球面上に拘束されてる場合の最短経路問題を解いてみる。
 
もう1つ、有名な例として、最速降下曲線 (brachistochrone curve)


##<a id="sec-generated-title-5"></a> <a id="energy"></a>弧長とエネルギー
曲線 <span class="math">x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> の弧長は
<span class="math"><span class="cursive">L</span><span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal">|</span>
 x<sup><span class="paren" style="font-size:em;">(</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><span class="normal">d</span>t
</span>
例えば、ユークリッド空間上の曲線
<span class="math">
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="paren" style="font-size:em;">(</span>
 x<sub><span class="normal">1</span></sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, 
 x<sub><span class="normal">2</span></sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, 
 x<sub><span class="normal">3</span></sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span></span>
の場合だと、
（表示の都合上、時間微分を ' で表すと）
<div class="math">
      <span class="cursive">L</span>
      <span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <span class="normal" style="font-size:1.2em;">√</span><span class="bar">
 x<sub><span class="normal">1</span></sub>'<sup><span class="normal">2</span></sup><span class="normal">+</span>
 x<sub><span class="normal">2</span></sub>'<sup><span class="normal">2</span></sup><span class="normal">+</span>
 x<sub><span class="normal">3</span></sub>'<sup><span class="normal">2</span></sup></span>
      <span class="normal">d</span>t
</div>
これに対して、以下のようなものを定義。
<div class="math">
      <span class="cursive">E</span>
      <span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <span class="paren" style="font-size:1.2em;">(</span>
 x<sub><span class="normal">1</span></sub>'<sup><span class="normal">2</span></sup><span class="normal">+</span>
 x<sub><span class="normal">2</span></sub>'<sup><span class="normal">2</span></sup><span class="normal">+</span>
 x<sub><span class="normal">3</span></sub>'<sup><span class="normal">2</span></sup><span class="paren" style="font-size:1.2em;">)</span>
      <span class="normal">d</span>t
</div>
力学で、運動エネルギーが <span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>mv<sup><span class="normal">2</span></sup></span> になることの類推から、
この <span class="math"><span class="cursive">E</span></span> をエネルギーと呼んだりする。
 
平方根がうっとうしいし、
弧長 <span class="math"><span class="cursive">L</span></span> の最小化問題をエネルギー <span class="math"><span class="cursive">E</span></span> の最小化問題にできないか考えてみる。
<h4>
      <span class="math">
        <span class="cursive">L</span>
      </span> と <span class="math"><span class="cursive">E</span></span> の関係</h4>
以後、簡単化のために、
平方根の中身を <span class="math">V</span> と書いて、
<span class="math"><span class="cursive">L</span><span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><span class="normal">d</span>t
</span>
,
<span class="math"><span class="cursive">E</span><span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
V
<span class="normal">d</span>t
</span>
としておく。
 
いったん
<span class="math"><span class="cursive">L</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><span class="normal">d</span>t
</span>
,
<span class="math">
α
<table class="frac" summary="fraction"><tr><td class="num"><span class="cursive">E</span></td></tr><tr><td>b <span class="normal">−</span> a</td></tr></table></span>
と置いて、
積分
<span class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="paren" style="font-size:1.2em;">(</span><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><span class="normal">−</span>
α
<span class="paren" style="font-size:1.2em;">)</span><sup><span class="normal">2</span></sup><span class="normal">d</span>t
</span>
を考えることで、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <span class="paren" style="font-size:1.2em;">(</span>
        <span class="normal" style="font-size:em;">√</span><span class="bar">V</span>
        <span class="normal">−</span>
α
<span class="paren" style="font-size:1.2em;">)</span>
      <sup><span class="normal">2</span></sup>
      <span class="normal">d</span>t

<span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
V
<span class="normal">d</span>t
<span class="normal">−</span><span class="normal">2</span>α
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><span class="normal">d</span>t
<span class="normal">−</span>
α<sup><span class="normal">2</span></sup><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal">d</span>t

<span class="normal">=</span><span class="cursive">E</span><span class="normal">−</span>
α<sup><span class="normal">2</span></sup><span class="paren" style="font-size:em;">(</span>b <span class="normal">−</span> a<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="cursive">E</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="cursive">L</span><sup><span class="normal">2</span></sup></td></tr><tr><td>b <span class="normal">−</span> a</td></tr></table></div>
一番左の辺が、二乗の積分（＝ 常に正）なので、
<span class="math"><span class="cursive">E</span><span class="normal">≧</span><table class="frac" summary="fraction"><tr><td class="num"><span class="cursive">L</span><sup><span class="normal">2</span></sup></td></tr><tr><td>b <span class="normal">−</span> a</td></tr></table></span>
。
等号は、<span class="math">V <span class="normal">≡</span> α</span>、
ようするに、
<span class="math">V</span> が積分変数 <span class="math">t</span> によらず一定のとき成立。
<h4>弧長パラメータ</h4>
時間パラメータ <span class="math">t</span> を適当に変数変換して、
<span class="math"><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><span class="normal">d</span>t
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table><span class="normal">d</span>s
</span>
としたとき、
<span class="math"><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table></span>
が定数になれば、
前節の結果から、
この変数 <span class="math">s</span> を使う限り、
<span class="math"><span class="cursive">E</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="cursive">L</span><sup><span class="normal">2</span></sup></td></tr><tr><td>b <span class="normal">−</span> a</td></tr></table></span>
。

<span class="math">
        <span class="normal" style="font-size:em;">√</span><span class="bar">V</span>
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table>
      </span>
が定数ということで、
その値を 1 にしておくと、
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>s</td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar">V</span></span>
で、結局、
<div class="math">
s<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> t</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">V</span><span class="normal">d</span>t
</div>
これを、
<span class="math"><span class="normal" style="font-size:em;">√</span><span class="bar">V</span></span> の弧長パラメータと呼ぶ。
<h4>
      <span class="math">
        <span class="normal" style="font-size:em;">√</span><span class="bar">V</span>
      </span> の座標変換不変性</h4>
<span class="math">
V
<span class="normal">=</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">1</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">2</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">3</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup></span>
なので、
<div class="math">
      <span class="normal" style="font-size:em;">√</span><span class="bar">V</span>
      <span class="normal">d</span>t
<span class="normal">=</span><span class="normal" style="font-size:2em;">√</span><span class="bar"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">1</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">2</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">3</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup></span><span class="normal">d</span>t
<span class="normal">=</span><span class="normal" style="font-size:2em;">√</span><span class="bar"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">1</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">2</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">3</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><sup><span class="normal">2</span></sup></span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table><span class="normal">d</span>s
</div><div class="math">
　
<span class="normal">=</span><span class="normal" style="font-size:2em;">√</span><span class="bar"><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">1</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup><span class="normal">2</span></sup><span class="normal">+</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">2</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup><span class="normal">2</span></sup><span class="normal">+</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>t</td></tr><tr><td><span class="normal">d</span>s</td></tr></table><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span>x<sub><span class="normal">3</span></sub></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup><span class="normal">2</span></sup></span><span class="normal">d</span>s
<span class="normal">=</span><span class="normal" style="font-size:2em;">√</span><span class="bar"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>x<sub><span class="normal">1</span></sub></num></td></tr><tr><td><span class="normal">d</span>s</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>x<sub><span class="normal">2</span></sub></num></td></tr><tr><td><span class="normal">d</span>s</td></tr></table><sup><span class="normal">2</span></sup><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>x<sub><span class="normal">3</span></sub></num></td></tr><tr><td><span class="normal">d</span>s</td></tr></table><sup><span class="normal">2</span></sup></span><span class="normal">d</span>s
</div>
パラメータ変数を何に取ろうが形が一緒。
もちろん、弧長パラメータを使っても一緒。
→ 
<span class="math"><span class="cursive">E</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="cursive">L</span><sup><span class="normal">2</span></sup></td></tr><tr><td>b <span class="normal">−</span> a</td></tr></table></span>
。
 
結局、
弧長 <span class="math"><span class="cursive">L</span></span> の最小化問題とエネルギー <span class="math"><span class="cursive">E</span></span> の最小化問題は同値。


##<a id="sec-generated-title-6"></a> <a id="summary"></a>まとめ
汎関数 ＝ 実関数→実数 の写像
 
変分問題 ＝ 汎関数の極値問題
 
変分問題の取り扱い方に関しては、
かなりしっかりとした理論が出来上がってる → 変分学。
 
結局、変分問題は微分方程式に帰着させられる。
 
弧長の変分問題とエネルギーの変分問題は同値。
<h4>further reading</h4>
「変分問題は微分方程式に帰着させて解くことができる」といっても、
微分方程式を解析的に解くのはそれはそれで難問。
というか、解けない場合の方が多い。
 
微分方程式は、結局、数値計算で近時解を求めたりするけども、
数値計算するなら、変分問題の方が解きやすかったりすることもある。
なので、むしろ、
逆に微分方程式を変分問題に直して数値計算したりもする。
