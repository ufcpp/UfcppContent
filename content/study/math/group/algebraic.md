---
title: "代数系"
source_url: "https://ufcpp.net/study/math/group/algebraic/"
content_type: "Article"
published_at: "2015-05-06T14:17:17"
updated_at: "2015-05-06T14:17:17"
tags: []
umbraco_id: 1484
parent_id: 1483
sort_order: 0
aliases:
  - "/group/algebraic"
  - "/group/algebraic.html"
  - "/math/group/algebraic/"
  - "/study/group/algebraic"
  - "/study/group/algebraic.html"
---

# 代数系

## <a id="sec-generated-title-1"></a> <a id="algebraic"></a>代数系

集合<span class="math">S</span>に対して、<span class="math">S<sup>S×S</sup></span>の元(<span class="math">S×S</span>から<span class="math">S</span>への写像)を<span class="math">S</span>の<strong id="d52e26" class="keyword">算法</strong>(operation)もしくは演算と呼び、
その性質に応じて、<span class="math">×, ＋, ・</span>などで表します。
(算法は、2つの元の間に働くものであることを明示するため、2項演算(binary operation)などと呼ぶこともあります。)
「集合<span class="math">S</span>が算法<span class="math">・</span>を持つ」とは、
すなわち、集合<span class="math">S</span>の元<span class="math">x, y</span>に対して、別の元<span class="math">x ・ y∈S</span>が存在することをさします。
 
集合<span class="math">A</span>と、その算法の族<span class="math">O</span>に対して、順序対<span class="math"><span class="paren" style="font-size:em;">(</span>A, O<span class="paren" style="font-size:em;">)</span></span>を<strong id="algebraic" class="keyword">代数系</strong>(algebraic system)と呼び、<span class="math">A</span>をその台(Support)、<span class="math">O</span>をその算法族と呼びます。
算法族の元が唯一(<span class="math">O=<span class="paren" style="font-size:em;">{</span>・<span class="paren" style="font-size:em;">}</span></span>)であるときには、<span class="math"><span class="paren" style="font-size:em;">(</span>A, <span class="paren" style="font-size:em;">{</span>・<span class="paren" style="font-size:em;">}</span><span class="paren" style="font-size:em;">)</span></span>を単に<span class="math"><span class="paren" style="font-size:em;">(</span>A, ・<span class="paren" style="font-size:em;">)</span></span>と書きます。
また、算法族を省略して、代数系を単に<span class="math">A</span>で表すこともあります。
 
例えば、自然数全体の集合を<span class="math">ω</span>、その加法を<span class="math">＋</span>、乗法を<span class="math">×</span>とすると、<span class="math"><span class="paren" style="font-size:em;">(</span>ω, <span class="paren" style="font-size:em;">{</span>＋, ×<span class="paren" style="font-size:em;">}</span><span class="paren" style="font-size:em;">)</span></span>は代数系となります。


## <a id="sec-generated-title-2"></a> <a id="d52e111"></a>代数系の性質

代数系が与えられたとき、その性質に応じて代数系を分類することが出来ます。
ここでは、代数系を分類する際に用いるいくつかの性質について説明します。


### <a id="sec-generated-title-3"></a> <a id="d52e116"></a>結合法則

代数系<span class="math"><span class="paren" style="font-size:em;">(</span>A, ・<span class="paren" style="font-size:em;">)</span></span>が与えられたとき、
<span class="math">A</span>の元<span class="math">x, y, z</span>について、
<div class="math">(x ・ y) ・ z = x ・ (y ・ ｚ)</div>
が成り立つとき、<span class="math">・</span>について<strong id="associative" class="keyword">結合法則</strong>(associative low)が成り立つといい、
<span class="math">A</span>は<span class="math">・</span>について結合的(associative)であるといいます。


### <a id="sec-generated-title-4"></a> <a id="d52e150"></a>交換法則

<span class="math">A</span>の元<span class="math">x, y</span>について、
<div class="math">x ・ y = y ・ x</div>
が成り立つとき、<span class="math">・</span>について<strong id="commutative" class="keyword">交換法則</strong>(commutative low)が成り立つといい、
<span class="math">A</span>は<span class="math">・</span>について可換(commutative)であるといいます。
 
結合法則および交換法則を満たす算法は<span class="math">＋</span>で表し、加法と呼ぶことが多いです。


### <a id="sec-generated-title-5"></a> <a id="d52e186"></a>分配法則

2つの算法<span class="math">＋, ×</span>を持つ代数系<span class="math"><span class="paren" style="font-size:em;">(</span>A, <span class="paren" style="font-size:em;">{</span>＋, ×<span class="paren" style="font-size:em;">}</span><span class="paren" style="font-size:em;">)</span></span>が与えられたとき、
<span class="math">A</span>の元<span class="math">x, y, z</span>について、
<div class="math">(x ＋ y) × z = (x × z) ＋ (y × z)</div><div class="math">z × (x ＋ y) = (z × x) ＋ (z × y)</div>
が成り立つとき、<strong id="distributive" class="keyword">分配法則</strong>(distributive low)が成り立つといいます。
 
分配法則が成り立つとき、算法<span class="math">＋</span>を加法、<span class="math">×</span>を乗法と呼びます。


### <a id="sec-generated-title-6"></a> <a id="d52e228"></a>単位元

<span class="math">A</span>の元<span class="math">e</span>で、
任意の元<span class="math">x∈A</span>に対して
<div class="math">e ・ x = x ・ e = x</div>
が成り立つようなものがあれば、<span class="math">e</span>を<span class="math">・</span>に関する<strong id="unity" class="keyword">単位元</strong>(unity)と呼びます。
また、<span class="math">A</span>が単位元を持つとき、<span class="math">A</span>は単位的(unitary)であるといいます。
 
単位元が存在するならば、それは一意的に定まります。
すなわち、<span class="math">e' ・ x = x ・ e' = x</span>を満たすような元<span class="math">e'</span>があれば、<span class="math">e = e ・ e' = e'</span>となります。
 
単位元は<span class="math">e</span>の他に、<span class="math">i, 1</span>などの記号で表すことも多い。
また、加法<span class="math">＋</span>に関する単位元は、<span class="math">0</span>で表し、零元と呼ぶこともあります。


### <a id="sec-generated-title-7"></a> <a id="d52e292"></a>逆元

<span class="math">A</span>の元<span class="math">x</span>に対して、
<div class="math">y ・ x = x ・ y = e</div>
を満たすような<span class="math">A</span>の元<span class="math">y</span>が存在するとき、<span class="math">x</span>は<strong id="regular" class="keyword">正則</strong>(regular)であるといいます。ただし、<span class="math">e</span>は<span class="math">A</span>の単位元です。

<span class="math">x</span>が正則元であるとき、上式を満たすような元<span class="math">y</span>は一意的に定まり、このような元を<span class="math">x</span>の<strong id="inverse" class="keyword">逆元</strong>(inverse)と呼びます。
一般的に、乗法<span class="math">×</span>に関する<span class="math">x</span>の逆元は<span class="math">x<sup>－1</sup></span>と表し、
加法<span class="math">＋</span>に関する逆元は<span class="math">－x</span>と表すことが多いです。
 
また、<span class="math">x</span>が正則元であるとき、<span class="math">x<sup>－1</sup></span>も正則となり、<span class="math">(x<sup>－1</sup>)<sup>－1</sup> = x</span>となります。
