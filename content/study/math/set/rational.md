---
title: "有理数"
source_url: "https://ufcpp.net/study/math/set/rational/"
content_type: "Article"
published_at: "2015-05-06T14:17:04"
updated_at: "2020-07-04T22:25:34"
tags: []
umbraco_id: 1477
parent_id: 1471
sort_order: 5
aliases:
  - "/math/set/rational/"
  - "/set/rational"
  - "/set/rational.html"
  - "/study/set/rational"
  - "/study/set/rational.html"
---

# 有理数

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
有理数は整数環から作った商体です。
自然数から整数を作る際、<span class="math">a － b</span> という形で表される数を考えましたが、
それと同様に、有理数は、
2つの整数 <span class="math">m, n</span> を用いて <span class="math">m/n</span> という形で表される数として定義します。


##<a id="sec-generated-title-2"></a> <a id="rational"></a>有理数の定義
<strong id="rational" class="keyword">有理数</strong>（rational number）は以下のような手順で定義します。

* 整数の対<span class="math"><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span> ∈ <span class="bold">Z</span>×<span class="bold">Z</span></span>を用意する。

* 2つの対<span class="math">p ＝ <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>, q ＝ <span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span></span>に対して、「<span class="math">a × d ＝ b × c</span>のとき互いに同値」という同値関係を定める。

* この同値関係を使って商集合<span class="math"><span class="bold">Q</span></span>を作る。

* この<span class="math"><span class="bold">Q</span></span>を有理数と呼ぶ。


要するに、自然数から整数を作る過程で加法に関して行ったような事を、
乗法に関しても行うことで有理数を作ります。
 
整数のときと同じく、整数の対 <span class="math"><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span></span> を <em><span class="math">a/b</span></em> とも書きます。
また、同値類 <span class="math">f<span class="paren" style="font-size:em;">(</span>a/1<span class="paren" style="font-size:em;">)</span></span> は整数 <span class="math">a</span> と1対1に対応するので、
これを <span class="math">a</span> と同一視することができ、
<em>整数は有理数の部分集合である</em>とみなすことができます。
 
同値類 <span class="math">f<span class="paren" style="font-size:em;">(</span>a/1<span class="paren" style="font-size:em;">)</span></span> を単に整数 <span class="math">a</span> で表します。
また、同値類 <span class="math">f<span class="paren" style="font-size:em;">(</span>1/a<span class="paren" style="font-size:em;">)</span></span> を <span class="math">a<sup>－1</sup></span> と表します。
<span class="math">a<sup>－1</sup></span> は <span class="math">a</span> の乗法に関する逆元になります。
すなわち、<span class="math">a × <span class="paren" style="font-size:em;">(</span>a<sup>－1</sup><span class="paren" style="font-size:em;">)</span> ＝ 1</span> が成り立ちます。


##<a id="sec-generated-title-3"></a> <a id="operation"></a>有理数の間の関係・演算
###<a id="sec-generated-title-4"></a> <a id="order"></a>有理数の順序
有理数 <span class="math">p ＝ a/b, q</span> の順序関係は
<div class="math">
ab ＞ 0 ⇔ a/b <span class="normal">は正の有理数</span></div><div class="math">
ab ＜ 0 ⇔ a/b <span class="normal">は負の有理数</span></div>
として有理数の正負を定め、
<div class="math">
p － q <span class="normal">が正</span> ⇔ p ＞ q
</div>
で定義します。


###<a id="sec-generated-title-5"></a> <a id="sum"></a>有理数の和・積
2つの有理数 <span class="math">p ＝ <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>, q ＝ <span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span></span> の間の和・積を、
<div class="math">
p ＋ q ＝ <span class="paren" style="font-size:em;">(</span>ad ＋ bc, bd<span class="paren" style="font-size:em;">)</span></div><div class="math">
p × q ＝ <span class="paren" style="font-size:em;">(</span>ac, bd<span class="paren" style="font-size:em;">)</span></div>
で定義します。


###<a id="sec-generated-title-6"></a> <a id="algebra"></a>代数系としての有理数
有理数は、和に関しても積に関しても可換「[群](../group/group.md#group)」となり、
和と積の間に分配法則が成り立つので、「[体](../group/field.md#field)」となります。
体であることを明示的に表すために、有理数を<em>有理数体</em>と呼ぶこともあります。
 
ちなみに、整数から有理数を作ったときと同様の手順（<span class="math">a, b</span> という元から <span class="math">a/b</span> という形で表される数の集合を作る）で、
任意の「[環](../group/field.md#ring)」から「[体](../group/field.md#field)」を作る事ができます。
このような手順で作った体を「[商体](../group/quotientfield.md#quotient)」といいます。
