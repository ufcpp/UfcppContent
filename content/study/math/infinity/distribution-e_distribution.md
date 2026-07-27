---
title: "超関数"
source_url: "https://ufcpp.net/study/math/infinity/distribution-e_distribution/"
content_type: "Article"
published_at: "2015-05-06T14:18:05"
updated_at: "2015-05-06T14:18:05"
tags: []
umbraco_id: 1507
parent_id: 1500
sort_order: 6
aliases:
  - "/study/infinity/distribution.html"
  - "/study/math/infinity/distribution"
---

# 超関数

## <a id="sec-generated-title-1"></a> <a id="distribution"></a>超関数

詳しくは別項「[超関数](distribution-e_distribution.md)」で述べますが、
超関数という考え方では1点だけで値が∞になるような関数（を拡張したもの）を定義できます。
基本的なアイディアとしては、

* 1点で値が∞になっても、その1点を含む任意の区間での積分の値が有限ならば OK ということにする。

* 2つの超関数<span class="math">f, g</span>の等値性は、任意の積分区間で<span class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x ＝ <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x</span>が成り立つかどうかで決める。


という感じです。
 
これで∞の値を持つ関数（のようなもの）を定義できるわけですが、
これには欠点もあります。
積分によって関数の等値性を決めるため、
1点で値が異なっていても、積分値が同じならば区別がつかないことになります。
1点でも値が異なれば、関数としては別のものになりますが、
超関数としては区別がつきません。
 
例えば、以下の2つの関数<span class="math">f, g</span>は関数論的には相異なるものですが、超関数論的には同じものになります。
（1点に限らず、可算無限個の点で異なる値を持つ2つの関数は超関数論的には同等。）
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ t
</div><div class="math">
g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">t</span>  </td><td><span class="paren">(</span><span class="math">t≠0</span><span class="paren">)</span></td></tr><tr><td><span class="math">1</span>  </td><td><span class="paren">(</span><span class="math">t＝0</span><span class="paren">)</span></td></tr></table></div>

## <a id="sec-generated-title-2"></a> <a id="delta"></a>δ関数

以下のような超関数を考えて見ましょう。
<div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>δ<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x
＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">1</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">区間</span><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span><span class="normal">が 0 を含む</span></span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">区間</span><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span><span class="normal">が 0 を含まない</span></span><span class="paren">)</span></td></tr></table></div>
このような超関数は、通常の関数の極限として作ることが出来て、
例えば、
以下のようなにして作ることが出来ます。
<div class="math">
f<sub>a</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:3em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>a</td></tr></table></span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">－a/2 ＜ x ＜ a/2</span></span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">それ以外</span></span><span class="paren">)</span></td></tr></table></div><div class="math">
δ<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">a → 0</td></tr></table>
f<sub>a</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></div>
この関数は、
<span class="math">x ＝ 0</span> に置いて∞の値を持ちます。


## <a id="sec-generated-title-3"></a> <a id="d73e136"></a>まとめ

超関数という概念を導入することで、
1点でだけ∞になるような関数を考えることが出来ます。
ただし、全くのノーリスクで∞の概念を得られるわけではなく、
関数として1点で値が異なっていても、超関数としては区別がつかないという欠点もあります。
