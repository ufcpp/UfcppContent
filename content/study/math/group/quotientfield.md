---
title: "商体・剰余体"
source_url: "https://ufcpp.net/study/math/group/quotientfield/"
content_type: "Article"
published_at: "2015-05-06T14:17:24"
updated_at: "2015-05-06T14:17:24"
tags: []
umbraco_id: 1487
parent_id: 1483
sort_order: 3
aliases:
  - "/group/quotientfield"
  - "/group/quotientfield.html"
  - "/math/group/quotientfield/"
  - "/study/group/quotientfield"
  - "/study/group/quotientfield.html"
---

# 商体・剰余体

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[有理数の定義](../set/rational.md#rational)」において、整数から有理数を作ったように、
ある特定の条件を満たす「[環](field.md#ring)」から「[体](field.md#field)」を機械的に作る方法があります。
 
体を作る方法には大きく分けて2つあり、
商体と呼ばれるものと剰余体と呼ばれるものがあります。


##<a id="sec-generated-title-2"></a> <a id="quotient"></a>商体
体の構成方法の1つ目は<strong id="quotient" class="keyword">商体</strong>（quotient field）と呼ばれるものです。
整数から有理数を定義（「[有理数の定義](../set/rational.md#rational)」参照）するときのように、
環 <span class="math">R</span> の元 <span class="math">m, n</span> に対して、
形式的に <span class="math">m / n</span> に相当するような元を作ることで体を構成します。
 
この手順は、半群から群を作るときの手順（「[半群から群を機械的に作る](group.md#s_to_g)」参照）とほとんど同様になります。
具体的な手順としては、

* 環<span class="math">R</span>の元の対<span class="math"><span class="paren" style="font-size:em;">(</span>m, n<span class="paren" style="font-size:em;">)</span> ∈ R×R</span>を用意する。

* 2つの対<span class="math">p ＝ <span class="paren" style="font-size:em;">(</span>k, l<span class="paren" style="font-size:em;">)</span>, q ＝ <span class="paren" style="font-size:em;">(</span>m, n<span class="paren" style="font-size:em;">)</span></span>に対して、「<span class="math">lm ＝ kn</span>のとき互いに同値」という同値関係<span class="math">～</span>を定める。

* この同値関係を使って商集合<span class="math">K ＝ R×R / ～</span>を作る。

* 形式的に、対<span class="math"><span class="paren" style="font-size:em;">(</span>m, n<span class="paren" style="font-size:em;">)</span></span>を<span class="math">mn<sup>－1</sup></span>あるいは<span class="math">m / n</span>と書く。

* <span class="math">p, q ∈ K</span>の乗法を<span class="math">p q ＝ <span class="paren" style="font-size:em;">(</span>k m, l n<span class="paren" style="font-size:em;">)</span></span>で定める。

* <span class="math">p, q ∈ K</span>の加法を<span class="math">p ＋ q ＝ <span class="paren" style="font-size:em;">(</span>k n ＋ l m, m n<span class="paren" style="font-size:em;">)</span></span>で定める。


<em>
このようにして定義した代数系 <span class="math">K</span> は、
環 <span class="math">R</span> が「[整域](field.md#integral)」のとき、
体になります。
</em>
（<span class="math">R</span> が整域でない（零因子を持つ）場合には、
<span class="math">K</span> も零因子を持つ（乗法の逆元がただ1つに確定しない）ことになるので、
体にはなりません。）


###<a id="sec-generated-title-3"></a> <a id="quotient_sample"></a>商体の例
商体の典型的な例というと、
これまでの説明でも出てきたように「[有理数](../set/rational.md#rational)」になります。
ですが、当然のことながら、有理数以外にもさまざまな商体が考えられます。
とにかく、任意の「[整域](field.md#integral)」があるとき、
機械的に体が構成できます。
 
例えば、実数上の多項式は整域になるので、
上述の手順で商体を構成できます。
要するに、実数上の有理式になるんですが、
この実数上の有理式は体を成します。


####<a id="sec-generated-title-4"></a> <a id="polynomial"></a>多項式環と有理式体
一般的に、体 <span class="math">K</span> 上の多項式は整域になり、
したがって、<span class="math">K</span> 上の有理式は体を成します。
 
体 <span class="math">K</span> 上の多項式 <span class="math">K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> というのは、
<span class="math">K</span> に対して、<span class="math">K</span> に含まれない新たな元 <span class="math">X</span> を加えて、
<div class="math">
f<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ∈ ω, k<sub>i</sub> ∈ K</td></tr></table>
k<sub>i</sub> X<sup>i</sup></div>
という形で表すことの出来る集合です。
<span class="math">K</span> に含まれない新たな元は1つである必要はなく、
<span class="math">N</span> この元 <span class="math">X<sub>1</sub>, X<sub>2</sub>, ・・・, X<sub>N</sub></span> を加えた多項式環 <span class="math">K<span class="paren" style="font-size:em;">[</span>X<sub>1</sub>, X<sub>2</sub>, ・・・, X<sub>N</sub><span class="paren" style="font-size:em;">]</span></span> を作ることも出来き、
<span class="math">K</span> 上の <span class="math">N</span> 変数多項式と呼びます。

<span class="math">K</span> 上の多項式は、変数の数 <span class="math">N</span> によらず、常に整域となり、
商体を作ることができます。
この商体を、<span class="math">K</span> 上の有理式体と呼び、
<span class="math">Q<span class="paren" style="font-size:em;">(</span>K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">)</span></span> とか、
<span class="math">K<span class="paren" style="font-size:em;">(</span>X<span class="paren" style="font-size:em;">)</span></span> とかいうように書き表します。
 
ちなみに、有理式体 <span class="math">K<span class="paren" style="font-size:em;">(</span>X<span class="paren" style="font-size:em;">)</span></span> は体 <span class="math">K</span> の「[拡大体](extensionfield.md#extension)」になっています。


##<a id="sec-generated-title-5"></a> <a id="residual"></a>剰余体
もう1つの体の構成方法として、
剰余体と呼ばれるものがあります。
こちらは、「[整数の剰余体](field.md#rasidualfield)」で説明した、
整数の剰余体と同じ手法で体を構成します。
 
剰余体について話をする前に、
イデアルなどの予備知識が必要ですので、
まずはそちらの説明から始めます。


###<a id="sec-generated-title-6"></a> <a id="ideal"></a>イデアル
環 <span class="math">R</span> の空でない部分集合 <span class="math">A</span> が

1. <span class="math">A</span>は加法について閉じていて、<span class="math">A</span>は<span class="math">R</span><sup>＋</sup>の部分群になっている

2. <span class="math">R</span>の任意の元<span class="math">x</span>、<span class="math">A</span>の任意の元<span class="math">a</span>に対して<span class="math">ax ∈ A, xa ∈ A</span>


という2つの条件を満たすとき、
<span class="math">A</span> を <span class="math">R</span> の<strong id="ideal" class="keyword">イデアル</strong>（ideal）と言います。
（<span class="math">ax ∈ A, xa ∈ A</span> のうちどちらか一方のみを満たすものを
左側イデアルもしくは右側イデアル、
両方を満たすものを両側イデアルと言ったりもします。）
 
例えば、ある整数 <span class="math">n</span> の倍数 <span class="math">n<span class="bold">Z</span></span>は、
整数 <span class="math"><span class="bold">Z</span></span> のイデアルになります。
<span class="math">n</span> の倍数全体の集合はもちろん整数全体の集合の部分集合で、
<span class="math">n</span> の倍数同士を足すと<span class="math">n</span> の倍数になりますし、
<span class="math">n</span> の倍数に対して整数をかけるとやはり <span class="math">n</span> の倍数になるので、
<span class="math">n<span class="bold">Z</span> ⊂ <span class="bold">Z</span></span> かつ
<span class="math">a, b ∈ n<span class="bold">Z</span> ⇒ a ＋ b ∈ n<span class="bold">Z</span></span> かつ
<span class="math">a ∈ n<span class="bold">Z</span>, x ∈ <span class="bold">Z</span> ⇒ ax ＝ xa ∈ n<span class="bold">Z</span></span>
であり、イデアルの条件を満たしています。
 
ちなみに、
環 <span class="math">R</span> 自身と、
<span class="math">R</span> の零元のみの集合 <span class="math"><span class="paren" style="font-size:em;">{</span>0<span class="paren" style="font-size:em;">}</span></span> は必ず <span class="math">R</span> のイデアルになります。
この2つは自明なイデアルと呼ばれます。


####<a id="sec-generated-title-7"></a> <a id="d55e425"></a>余談
イデアルの概念は、理想数（ideal number）という別の概念から派生したものらしい。
この理想数の概念を考えたのも、
それを整理して今のイデアルの概念を作ったのもドイツの数学者なので、英語の ideal に相当するドイツ語 ideale の読みで「イデアル」というらしい。


###<a id="sec-generated-title-8"></a> <a id="generator"></a>生成元
整数環における <span class="math">n</span> の倍数のように、
環 <span class="math">R</span> のある元の倍数の形で表されるイデアルがあります。
 
環 <span class="math">R</span> の <span class="math">N</span> の元
<span class="math">a<sub>1</sub> , a<sub>2</sub> , ・・・, a<sub>N</sub></span>
に対して、
<div class="math">
A
＝
<span class="paren" style="font-size:em;">{</span>
a<sub>1</sub> x<sub>1</sub> ＋ a<sub>2</sub> x<sub>2</sub>
＋ ・・・ ＋
a<sub>N</sub> x<sub>N</sub>
|
x<sub>i</sub> ∈ R <span class="paren" style="font-size:em;">(</span>i ＝ 1, 2, ・・・, N<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></div>
という形で表される集合 <span class="math">A</span> は <span class="math">R</span> のイデアルになります。
このとき、この <span class="math">N</span> 個の元
<span class="math">a<sub>1</sub> , a<sub>2</sub> , ・・・, a<sub>N</sub></span>
を <span class="math">A</span> の<strong id="generator" class="keyword">生成元</strong>（generating element, generator）と呼びます。
 
有限個の生成元から上述のようにして作られるイデアルを有限生成イデアルと呼びます。
特に、ただ1つの元 <span class="math">a</span> から作られるイデアル
<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>ax | x ∈ R<span class="paren" style="font-size:em;">}</span></span>
を単項イデアルと呼びます。
 
整数環のイデアルはすべて単項イデアルになります。
2つの元 <span class="math">m, n</span> からイデアルを生成したものは、
結局の所 <span class="math">m</span> と <span class="math">n</span> の最大公約数から生成したイデアルと一致します。
例えば、
<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>4x ＋ 6y | x, y ∈ R<span class="paren" style="font-size:em;">}</span></span>
と
<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>2x | x ∈ R<span class="paren" style="font-size:em;">}</span></span>
は同じ集合になります。
整数環と同じように、イデアルが全て単項イデアルになるような整域を
単項イデアル整域と呼びます。
 
逆に、単項イデアル整域とはならない例を挙げると、
整数上の多項式環 <span class="math"><span class="bold">Z</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> は単項イデアル整域ではありません。
例えば、
<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>2x ＋ Xy | x, y ∈ R<span class="paren" style="font-size:em;">}</span></span>
というイデアルは、<span class="math">2</span> と <span class="math">X</span> が線形独立なので、
これ以上生成元を減らすことが出来ません。


###<a id="sec-generated-title-9"></a> <a id="prime_ideal"></a>素イデアル
環 <span class="math">R</span> のイデアル <span class="math">A</span> があるとき、
<span class="math">R</span> の任意の元 <span class="math">a, b</span> について、
<span class="math">ab ∈ A ⇒ a∈A ∨ b∈A</span> が成り立つとき、
<span class="math">A</span> を<strong id="p_ideal" class="keyword">素イデアル</strong>（prime ideal）と言います。

<em>
整数のイデアル <span class="math">n<span class="bold">Z</span></span>は、
「[生成元](#generator)」<span class="math">n</span> が素数のとき、素イデアルになります。
</em>
例えば、<span class="math">n</span> が 6 ＝ 2×3 と言うように素数でない場合には、
<span class="math">4, 9 ∈ <span class="bold">Z</span>, 4・9 ＝ 36 ＝ 6・6 ∈ 6<span class="bold">Z</span></span>
ですが、
4、9 は 6 の倍数ではないので、<span class="math">6<span class="bold">Z</span></span> に含まれません。
一方、<span class="math">n</span> が素数 <span class="math">p</span> の場合、
<span class="math">ab ∈ p<span class="bold">Z</span></span> ならば、
<span class="math">a</span> か <span class="math">b</span> のどちらか片方は必ず <span class="math">p</span> の倍数となるので、
素イデアルの条件を満たします。


###<a id="sec-generated-title-10"></a> <a id="maximal_ideal"></a>極大イデアル
環 <span class="math">R</span> のイデアル <span class="math">A</span> が、
<span class="math">R</span> の任意のイデアル <span class="math">B</span> に対して
<span class="math">A ⊆ B ⊆ R ⇒ A＝B ∨ B＝R</span>
を満たすとき、
<span class="math">A</span> を<strong id="m_ideal" class="keyword">極大イデアル</strong>（maximal ideal）と言います。
 
要するに、極大イデアルは、
<span class="math">A</span> よりも大きな（<span class="math">A</span> を真部分集合として含むような）イデアルは <span class="math">R</span> 自身以外にないようなイデアルになります。
 
整数環の場合、素イデアル（生成元が素数）ならば必ず極大イデアルになります。
しかしながら、一般的には、素イデアルは必ずしも極大イデアルにはなりません。
ただし、その逆は常に成り立ち、
極大イデアルは常に素イデアルになります。


###<a id="sec-generated-title-11"></a> <a id="residual_ring"></a>剰余環
環 <span class="math">R</span> のイデアル <span class="math">A</span> が与えられたとき、
「<span class="math">x － y ∈ A</span> のとき <span class="math">x</span> と <span class="math">y</span> が同値である」と定め、
この同値関係を使って「[商集合](../set/integer.md#quotient_set)」を作ったものを
<span class="math">R/A</span> と書きます。

<span class="math">R/A</span> の元 <span class="math">x</span> の「[同値類](../set/integer.md#eq_class)」を
<span class="math"><span class="paren" style="font-size:em;">{</span>x<span class="paren" style="font-size:em;">}</span><sub>A</sub></span> で表し、
<span class="math">R/A</span> の元 <span class="math">x, y</span> の間の加法および乗法をそれぞれ
<div class="math">
        <span class="paren" style="font-size:em;">{</span>x<span class="paren" style="font-size:em;">}</span>
        <sub>A</sub> ＋ <span class="paren" style="font-size:em;">{</span>y<span class="paren" style="font-size:em;">}</span><sub>A</sub>
＝
<span class="paren" style="font-size:em;">{</span>x ＋ y<span class="paren" style="font-size:em;">}</span><sub>A</sub></div><div class="math">
        <span class="paren" style="font-size:em;">{</span>x<span class="paren" style="font-size:em;">}</span>
        <sub>A</sub> × <span class="paren" style="font-size:em;">{</span>y<span class="paren" style="font-size:em;">}</span><sub>A</sub>
＝
<span class="paren" style="font-size:em;">{</span>x × y<span class="paren" style="font-size:em;">}</span><sub>A</sub></div>
で表すと、
<span class="math">R/A</span> は環になります。
 
このような環を整数 <span class="math"><span class="bold">Z</span></span> と
そのイデアル <span class="math">n<span class="bold">Z</span></span> で作ったものが、
「[有限体](field.md#finite)」で説明したような整数の剰余環（整数を <span class="math">n</span> で割ったあまりの集合）になります。
すなわち、

* 整数<span class="math"><span class="bold">Z</span></span>とイデアル<span class="math">n<span class="bold">Z</span></span>の商集合<span class="math"><span class="bold">Z</span>/n<span class="bold">Z</span></span>は 0 から<span class="math">n － 1</span>までの<span class="math">n</span>個の元からなる集合と同等。

* 条件<span class="math">x － y ∈ n<span class="bold">Z</span></span>は<span class="math">x ≡ y <span class="paren" style="font-size:em;">(</span>mod n<span class="paren" style="font-size:em;">)</span></span>と同等。

* したがって、商集合<span class="math"><span class="bold">Z</span>/n<span class="bold">Z</span></span>に対して、<span class="math"><span class="paren" style="font-size:em;">(</span>x ＋ y<span class="paren" style="font-size:em;">)</span> mod n</span>、<span class="math"><span class="paren" style="font-size:em;">(</span>x × y<span class="paren" style="font-size:em;">)</span> mod n</span>で加法・乗法を定義したものはここで説明したような剰余環になる。


ということになります。
 
では逆に、このような考え方を任意の剰余環に当てはめてみましょう。
環 <span class="math">R</span> の元 <span class="math">k</span> を生成元とする単項イデアルを
<div class="math">
kR
＝
<span class="paren" style="font-size:em;">{</span>
kx
|
x ∈ R
<span class="paren" style="font-size:em;">}</span></div>
とし、このイデアル <span class="math">kR</span> を使って剰余環 <span class="math">R/kR</span> を作ることができます。
このとき、大雑把な言い方をすると、この環 <span class="math">R/kR</span> はもとの環 <span class="math">R</span> を元 <span class="math">k</span> で割ったあまりの集合だとみなすことができます。


###<a id="sec-generated-title-12"></a> <a id="residual_field"></a>剰余体
環 <span class="math">R</span> のイデアル <span class="math">A</span> による剰余環 <span class="math">R/A</span> は、
<span class="math">A</span> が「[素イデアル](#p_ideal)」のとき整域に、
<span class="math">A</span> が「[極大イデアル](#m_ideal)」のとき体になります。
剰余環 <span class="math">R/A</span> が体になっているとき（すなわち、<span class="math">A</span> が極大イデアルのとき）、
<span class="math">R/A</span> を<strong id="residual" class="keyword">剰余体</strong>（residual field）と呼びます。
（詳しい証明は省略。興味のある方は体論の教科書等を参照してください。）
 
例えば、整数のイデアル <span class="math">n<span class="bold">Z</span></span> は <span class="math">n</span> が素数のときに極大イデアルになるので、
<em>
素数 <span class="math">p</span> を生成元とするイデアル <span class="math">p<span class="bold">Z</span></span> を使って作った剰余環 <span class="math"><span class="bold">Z</span>/p<span class="bold">Z</span></span> は体になります。
</em>
また、体 <span class="math">K</span> 上の多項式 <span class="math">K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> に対して、
既約多項式（因数分解の出来ない多項式） <span class="math">f</span> を生成元とするイデアル <span class="math">f K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> は極大イデアルになり、
剰余環 <span class="math">K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>/f K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> は体になります。
（すなわち、<em>多項式環を既約多項式で割ったあまりの集合は体になります</em>。）
 
例えば、実数上の多項式 <span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> に付いて考えてみましょう。
実数上の多項式で（実数の範囲で）因数分解の出来ない多項式というと、<span class="math">X<sup>2</sup> ＋ 1</span> があります。
そこで、<span class="math">X<sup>2</sup> ＋ 1</span> を生成元とするイデアル <span class="math"><span class="paren" style="font-size:em;">(</span>X<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> を使って剰余環 <span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>/<span class="paren" style="font-size:em;">(</span>X<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> を作ります。
<span class="math"><span class="bold">Z</span>/p<span class="bold">Z</span></span> が「整数を素数 <span class="math">p</span> で割ったあまりの集合」であるのと同様に、
このようにして作った剰余環<span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>/<span class="paren" style="font-size:em;">(</span>X<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> は、以下のように解釈する事ができます。

* 実数上の多項式<span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>を既約既約多項式<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>で割ったあまりの集合を作る。


これは以下のような解釈の仕方をしてもかまいません。

* 実数上の多項式<span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>の変数<span class="math">X</span>に対して、条件<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>を付ける。

* 実数<span class="math"><span class="bold">R</span></span>に対して、条件<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>を満たす新しい元<span class="math">X</span>を加える。


1番下のような解釈をした場合、
このような元 <span class="math">X</span> を <span class="math">i</span> で表すとどこかで見たことのあるような集合になるかと思います。
すなわち、
<div class="math">
        <span class="bold">C</span> ＝ <span class="paren" style="font-size:em;">{</span><span class="bold">R</span>, i<span class="paren" style="font-size:em;">}</span></div><div class="math">
i<sup>2</sup> ＝ －1
</div>
であり、要するにこれは複素数の定義になります。
したがって、
剰余環 <span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>/<span class="paren" style="font-size:em;">(</span>X<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> は、
複素数 <span class="math"><span class="bold">C</span></span> と同型な集合であり、
体になります。
