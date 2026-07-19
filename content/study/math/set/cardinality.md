---
title: "濃度"
source_url: "https://ufcpp.net/study/math/set/cardinality/"
content_type: "Article"
published_at: "2015-05-06T14:17:10"
updated_at: "2015-05-06T14:17:10"
tags: []
umbraco_id: 1480
parent_id: 1471
sort_order: 8
aliases:
  - "/math/set/cardinality/"
  - "/set/cardinality"
  - "/set/cardinality.html"
  - "/study/set/cardinality"
  - "/study/set/cardinality.html"
---

# 濃度

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[濃度](#cardinality)」とは、
有限集合で言う所の“集合の元の数”に相当する概念です。
無限集合に対して「元の数」というと少しおかしいので、濃度という言い方をします。

「[元の個数](map.md#num)」で説明したように、
有限集合 <span class="math">S</span> の場合、
<span class="math">S</span> と「[同値](map.md#equivalent)」な自然数がただひとつ定まるので、
その自然数によって <span class="math">S</span> の元の数を定義しました。

これに対し、無限集合の元の数は自然数で表すことができないので、
数の概念を拡張する必要があります。
無限集合の濃度を表すための概念として、
「[基数](#cardinal)」とも呼ばれるものがあります。

この基数の概念を説明するには、
その下準備としてまず、集合の大小、すなわち「[順序関係](#order_relation)」についての説明が必要になります。
そして次に、自然数の和・積・大小関係を無限集合に対して拡張した「[順序数](#ordinal)」というものについて説明します。
そして、集合 <span class="math">a</span> の基数、あるいは濃度（cardinality）と言うものを、
<span class="math">a</span> と「[同値](map.md#equivalent)」な最小の順序数として定義します。


##<a id="sec-generated-title-2"></a> <a id="ordered_set"></a>順序集合
集合 <span class="math">a</span> の2つの元の間の「[関係](map.md#relation)」<span class="math">f</span> が以下の条件を満たすとき、<span class="math">f</span> を<strong id="order_relation" class="keyword">順序関係</strong>（order relation）と言います。

1. <span class="math">
          f <span class="normal">⊃</span> Δ<sub>a</sub>
        </span>

2. <span class="math">
          f ∩ f<sup><span class="normal">−1</span></sup> <span class="normal">=</span> Δ<sub>a</sub>
        </span>

3. <span class="math">f <span class="normal">∘</span> f <span class="normal">⊂</span> f</span>


ただし、
<span class="math">
        Δ<sub>a</sub>
      </span> は <span class="math">a <span class="normal">×</span> a</span> の対角集合
<span class="math">
        Δ<sub>a</sub>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>x, x<span class="paren" style="font-size:em;">)</span> | x <span class="normal">∈</span> a
        <span class="paren" style="font-size:em;">}</span>
      </span>
です。
3行目の <span class="math">
        f <span class="normal">∘</span> f</span> は関係の合成を表します。

説明の補足のために具体例を挙げると、
自然数の大小関係 <span class="math"><span class="normal">≦</span>
      </span> はこの条件を満たしています。
1. の対角集合を含むというのは、<span class="math">x <span class="normal">≦</span> y</span> という関係が <span class="math">x <span class="normal">=</span> y</span> という関係を含むという意味で、
2. は <span class="math">x <span class="normal">≦</span> y</span> かつ <span class="math">x <span class="normal">≧</span> y</span> ⇔ <span class="math">x <span class="normal">=</span> y</span> と言う意味です。
3. は <span class="math">x <span class="normal">≦</span> y</span> かつ <span class="math">y <span class="normal">≦</span> z</span> ⇒ <span class="math">x <span class="normal">≦</span> z</span> です。

集合 <span class="math">a</span> と関係 <span class="math">f</span> を合わせて（「[順序対](map.md#orderd_pair)」を作って） <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> を<strong id="ordered_set" class="keyword">順序集合</strong>（ordered set）と呼びます。
このとき、<span class="math">a</span> は <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> の<strong id="support" class="keyword">台集合</strong>（support）といいます。
順序関係 <span class="math">f</span> が明らかな場合には、
省略して、単に順序集合 <span class="math">a</span> と表すこともあります。

<span class="math">x, y <span class="normal">∈</span> a</span> に対して、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> f
      </span> のとき、
<span class="math">
        x <span class="normal">≦</span><sub>f</sub> y
      </span> と表すことにします。
（本来は、図1のように、＜ を少し凹ませたような記号を使いますが、
フォントの都合でこのように整数の大小関係の記号で代用します。
<span class="math">f</span> を書き入れる場所も、本来は演算子の真下ですが、
表示の都合上、右下に表記します。
ちなみに、＜ を凹ませたほうを precedes、＞ の方を succeedes と言ったりするようです。
）

<figure>
	[![関係演算子](../../../../assets/media/ufcpp2000/math/prec_succ.emf)](../../../../assets/media/ufcpp2000/math/prec_succ.emf)
	<figcaption>関係演算子</figcaption>
</figure>


追記：ページを Unicode にしたらprecedes, succeedes 記号表示できるみたい →
<span class="math">
      <span class="normal">≺</span>, <span class="normal">≻</span>
    </span>

また、これもここでのみの記法ですが、
<span class="math">
        x <span class="normal">≦</span><sub>f</sub> y
      </span> かつ <span class="math">x <span class="normal">≠</span> y</span> のとき、
<span class="math">
        x <span class="normal">&lt;</span><sub>f</sub> y
      </span> で表します。
（本来は、図1の preceding 記号の下に <span class="math"><span class="normal">≠</span>
      </span> を書いた記号を使います。）

大小関係以外にも、
任意の集合間の包含関係や、
整数の整除関係（「<span class="math">x</span> が <span class="math">y</span> を割り切る」という関係）
も順序関係になります。


##<a id="sec-generated-title-3"></a> <a id="well_ordered_set"></a>整列集合
前節で定義した順序集合は、
「順序関係によって順番に並べられた集合」なわけですが、
必ずしも「綺麗に並べられた集合」にはなりません。
綺麗ではないというのはどういうことかというと、
まあ、実例を挙げてみるなら、
包含関係や整除関係による順序には以下のような問題があります。

まず、包含関係を <span class="math">I</span> で表すと、
<span class="math">
        <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span> <span class="normal">≦</span><sub>I</sub><span class="paren" style="font-size:em;">{</span>a, b, c<span class="paren" style="font-size:em;">}</span>
      </span>
や
<span class="math">
        <span class="paren" style="font-size:em;">{</span>b, c<span class="paren" style="font-size:em;">}</span> <span class="normal">≦</span><sub>I</sub><span class="paren" style="font-size:em;">{</span>a, b, c<span class="paren" style="font-size:em;">}</span>
      </span>
などはいいのですが、
<span class="math">
        <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>
      </span>
と
<span class="math">
        <span class="paren" style="font-size:em;">{</span>b, c<span class="paren" style="font-size:em;">}</span>
      </span>
の間には
<span class="math">
        <span class="paren" style="font-size:em;">{</span>b, c<span class="paren" style="font-size:em;">}</span> <span class="normal">≦</span><sub>I</sub><span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>
      </span>
という関係も
<span class="math">
        <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span> <span class="normal">≦</span><sub>I</sub><span class="paren" style="font-size:em;">{</span>b, c<span class="paren" style="font-size:em;">}</span>
      </span>
という関係も成り立ちません。
この順序は判別できないことになり、
したがって、
<span class="math">
        <span class="paren" style="font-size:1.5em;">{</span>
          <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>,
          <span class="paren" style="font-size:em;">{</span>b, c<span class="paren" style="font-size:em;">}</span>,
          <span class="paren" style="font-size:em;">{</span>a, b, c<span class="paren" style="font-size:em;">}</span>
        <span class="paren" style="font-size:1.5em;">}</span>
      </span>
という集合には、包含関係による順序関係を使う限り、
最小値が存在しないことになります。

整数の整除関係に関しても同様で、
整除関係を <span class="math">E</span> で表すと、
<span class="math">
        <span class="normal">1</span> <span class="normal">≦</span><sub>E</sub> <span class="normal">2</span> <span class="normal">≦</span><sub>E</sub> 6
      </span>,
<span class="math">
        <span class="normal">1</span> <span class="normal">≦</span><sub>E</sub> <span class="normal">3</span> <span class="normal">≦</span><sub>E</sub> 6
      </span>,
などは成り立ちますが、
2 と 3 の間には
<span class="math">
        <span class="normal">2</span> <span class="normal">≦</span><sub>E</sub>
        <span class="normal">3</span>
      </span> も
<span class="math">
        <span class="normal">3</span> <span class="normal">≦</span><sub>E</sub> <span class="normal">2</span>
      </span> も成り立ちません。

これに対して、
順序集合 <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> の
任意の2つの元 <span class="math">x, y</span> に対して、
<span class="math">
        x <span class="normal">≦</span><sub>f</sub> y
      </span> か
<span class="math">
        x <span class="normal">≧</span><sub>f</sub> y
      </span> の少なくとも一方が成り立つとき、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> を<strong id="totalorder" class="keyword">全順序集合</strong>（total ordered set）と呼びます。

全順序集合であっても、
整数全体の集合のように、
（－∞ まで続くいたりして）最小値を持たない集合も存在します。
そこで、順序集合 <span class="math">a</span> の任意の部分集合 <span class="math">s</span> が最小値を持つという条件を考えます。
この条件を満たすとき、
順序集合 <span class="math">a</span> を<strong id="well-ordered" class="keyword">整列集合</strong>（well-orderd set）と呼びます。
整列集合は全順序集合になりますが、
整数全体の集合が反例となっている通り、
その逆は成り立ちません。


##<a id="sec-generated-title-4"></a> <a id="order_isomorphic"></a>順序同型
2つの順序集合
<span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span>,
<span class="math">
        <span class="paren" style="font-size:em;">(</span>b, g<span class="paren" style="font-size:em;">)</span>
      </span>
が、集合として同値なだけでなく、順序まで含めて同等な関係を持つとき、
2つの順序集合は順序同型であるといいます。

正確には、
<span class="math">a</span> から <span class="math">b</span> への「[写像](map.md#mapping)」<span class="math">F: a → b</span> が、
任意の <span class="math">x, y <span class="normal">∈</span> a</span> に対して、
<div class="math">
      x <span class="normal">≦</span><sub>f</sub> y
      <span class="normal">⇒</span>
      F<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">≦</span><sub>g</sub> F<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
    </div>
を満たすとき、
<span class="math">F</span> を単調写像（monotonous mapping）といいます。
さらに、もし <span class="math">F</span> が「[全単写](map.md#bijection)」ならば、
<span class="math">F</span> を順序同型写像（order isomorphism）といい、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> と
<span class="math">
        <span class="paren" style="font-size:em;">(</span>b, g<span class="paren" style="font-size:em;">)</span>
      </span>
は<strong id="isomorphic" class="keyword">順序同型</strong>（order isomorphic）であるといいます。


##<a id="sec-generated-title-5"></a> <a id="ordinal_number"></a>順序数
「[自然数](natural.md)」で説明した通り、
「[自然数](natural.md#natural)」は、
<div class="math">
      <span class="normal">1</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>0<span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      <span class="normal">2</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>0, <span class="normal">1</span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      <span class="normal">3</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        0, <span class="normal">1</span>, <span class="normal">2</span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      <span class="normal">4</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        0, <span class="normal">1</span>, <span class="normal">2</span>, <span class="normal">3</span>
    <span class="paren" style="font-size:em;">}</span>
    </div>
などという形で表されます。
見ての通り、任意の自然数 <span class="math">n</span> は、
<span class="math">n</span> より小さい全ての自然数を含む集合です。

これを一般化して、
「[整列集合](#well-ordered)」<span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> の任意の元 <span class="math">x</span> が
<span class="math">x</span> 以下の元を全て含む、すなわち、
<span class="math">
        s<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          y <span class="normal">∈</span> a | y <span class="normal">&lt;</span><sub>f</sub> x
        <span class="paren" style="font-size:em;">}</span>
      </span>
と置いて、
<div class="math">
      x <span class="normal">=</span> s<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
    </div>
た成り立つとき、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> を<strong id="ordinal" class="keyword">順序数</strong>（ordinal number）と言います。

整列集合 <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> があるとき、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span> と順序同型な順序数 <span class="math">α</span> がただ1つ定まります。
この順序数を
<span class="math">
        α <span class="normal">=</span> <span class="normal">ord</span><span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
      </span>
あるいは単に
<span class="math">
        α <span class="normal">=</span> <span class="normal">ord</span> a
      </span> で書き表し、
<span class="math">a</span> の順序数と呼びます。

あるいは、
<span class="math">
        <span class="normal">ord</span> a
      </span> は順序型（order type）とも呼ばれます。
すなわち、
2つの整列集合が順序同型なとき、
2つは同じ順序型を持つといい、
その「型」を <span class="math">
        <span class="normal">ord</span> a
      </span> という記号で表すわけです。

さて、この順序数というものには、
次節以降で述べる方法で和、積、順序関係を定義できます。
少々複雑ですが、
要点だけ先に述べてしまうと、以下のような集合が作れるということです。

* 自然数の和・積・大小関係を、無限集合に対して自然に拡張したものになっている。

* 順序数が有限集合の場合、その演算・関係は自然数のものと一致する。

* 無限集合の場合、積の交換法則は成り立たず、和と積の間の分配法則も成り立たない。



###<a id="sec-generated-title-6"></a> <a id="sum"></a>順序数の和
2つの順序集合
<span class="math">
          <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
        </span>,
<span class="math">
          <span class="paren" style="font-size:em;">(</span>b, g<span class="paren" style="font-size:em;">)</span>
        </span>
を考えます。
これらの「[台集合](#support)」<span class="math">a, b</span> に対して、
<div class="math">
        a <span class="normal">⊕</span> b <span class="normal">=</span> a <span class="normal">×</span> <span class="paren" style="font-size:em;">{</span>0<span class="paren" style="font-size:em;">}</span> <span class="normal">∪</span> b <span class="normal">×</span> <span class="paren" style="font-size:em;">{</span>
          <span class="normal">1</span>
        <span class="paren" style="font-size:em;">}</span>
      </div>
（ただし、<span class="math"><span class="normal">⊕</span></span> という記号は「[直和](set.md#disjoint)」を表すものです。
また、<span class="math"><span class="normal">×</span></span> は「[直積](map.md#directprod)」を表します。）
という集合を作ると、
<span class="math">a, b</span> それぞれの元が何であれ、
別々の元だとみなして直和を取ったものを作れます。

この直和集合に対してさらに、
（<span class="math">x, y <span class="normal">∈</span> a <span class="normal">∪</span> b</span>, <span class="math">m, n <span class="normal">∈</span> 2</span> として、）
<div class="math">
        <span class="paren" style="font-size:em;">(</span>x, m<span class="paren" style="font-size:em;">)</span>
        <span class="normal">≦</span>
        <span class="paren" style="font-size:em;">(</span>y, n<span class="paren" style="font-size:em;">)</span>
        ⇔
        <span class="paren" style="font-size:1.5em;">[</span>
          m <span class="normal">&lt;</span> n
          <span class="normal">∨</span>
          <span class="paren" style="font-size:em;">(</span>
            m <span class="normal">=</span> n <span class="normal">=</span> 0 <span class="normal">∧</span> x <span class="normal">≦</span><sub>f</sub> y
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">∨</span>
          <span class="paren" style="font-size:em;">(</span>
            m <span class="normal">=</span> n <span class="normal">=</span> <span class="normal">1</span> <span class="normal">∧</span> x <span class="normal">≦</span><sub>g</sub> y
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:1.5em;">]</span>
      </div>
という順序関係 <span class="math"><span class="normal">≦</span></span> を導入すると、
<span class="math">
          <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
        </span>,
<span class="math">
          <span class="paren" style="font-size:em;">(</span>b, g<span class="paren" style="font-size:em;">)</span>
        </span>
が共に整列集合ならば、
<span class="math">
          <span class="paren" style="font-size:em;">(</span>a <span class="normal">⊕</span> b, <span class="normal">≦</span><span class="paren" style="font-size:em;">)</span>
        </span>
も整列集合になります。

この整列集合を使って、
順序数
<span class="math">
          α <span class="normal">=</span> <span class="normal">ord</span> a
        </span>,
<span class="math">
          β <span class="normal">=</span> <span class="normal">ord</span> b
        </span> の和を
<div class="math">
        α <span class="normal">⊕</span> β
        <span class="normal">=</span>
        <span class="normal">ord</span><span class="paren" style="font-size:em;">(</span>a <span class="normal">⊕</span> b<span class="paren" style="font-size:em;">)</span>
      </div>
で定義します。

<span class="math">a, b</span> が自然数の場合、
この定義は自然数の和に一致します。
また、順序数 α が無限集合ならば、
0 以外の任意の自然数 <span class="math">n</span> に対して
<span class="math">n <span class="normal">⊕</span> α <span class="normal">=</span> α</span> が成り立ちます。


###<a id="sec-generated-title-7"></a> <a id="prod"></a>順序数の積
同じく2つの整列集合
<span class="math">
          <span class="paren" style="font-size:em;">(</span>a, f<span class="paren" style="font-size:em;">)</span>
        </span>,
<span class="math">
          <span class="paren" style="font-size:em;">(</span>b, g<span class="paren" style="font-size:em;">)</span>
        </span>
に対し、今度はその「[直積](map.md#directprod)」を考えます。
直積 <span class="math">a <span class="normal">×</span> b</span> の元
<span class="math">
          <span class="paren" style="font-size:em;">(</span>x, z<span class="paren" style="font-size:em;">)</span>
        </span>,
<span class="math">
          <span class="paren" style="font-size:em;">(</span>y, w<span class="paren" style="font-size:em;">)</span>
        </span>
に対して、
逆辞書式順序と呼ばれる
<div class="math">
        <span class="paren" style="font-size:em;">(</span>x, z<span class="paren" style="font-size:em;">)</span>
        <span class="normal">≦</span>
        <span class="paren" style="font-size:em;">(</span>y, w<span class="paren" style="font-size:em;">)</span>
        ⇔
        <span class="paren" style="font-size:1.5em;">[</span>
          z <span class="normal">≦</span><sub>g</sub> w
          <span class="normal">∨</span>
          <span class="paren" style="font-size:em;">(</span>
            z <span class="normal">=</span> w <span class="normal">∧</span> x <span class="normal">≦</span><sub>f</sub> y
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:1.5em;">]</span>
      </div>
という順序関係を導入すると、
この直積集合も整列集合になります。

この整列集合を使って、
順序数
<span class="math">
          α <span class="normal">=</span> <span class="normal">ord</span> a
        </span>,
<span class="math">
          β <span class="normal">=</span> <span class="normal">ord</span> b
        </span> の積を
<div class="math">
        α <span class="normal">⊗</span> β
        <span class="normal">=</span>
        <span class="normal">ord</span><span class="paren" style="font-size:em;">(</span>a <span class="normal">×</span> b<span class="paren" style="font-size:em;">)</span>
      </div>
で定義します。

<span class="math">a, b</span> が自然数の場合、
この定義は自然数の和に一致し、交換法則もなりたちます。

しかしながら、
一般的には順序数の積は交換法則を満たしません。
例えば、詳しい説明は省略しますが、
<span class="math">
          ω <span class="normal">⊗</span> <span class="normal">2</span> <span class="normal">&gt;</span> ω
        </span> という関係は成り立ちますが、
その逆は
<span class="math">
          <span class="normal">2</span> <span class="normal">⊗</span> ω <span class="normal">=</span> ω
        </span> であり、
<span class="math">
          ω <span class="normal">⊗</span> <span class="normal">2</span> <span class="normal">≠</span> <span class="normal">2</span> <span class="normal">⊗</span> ω
        </span> になります。
また、<span class="math"><span class="normal">⊕</span></span> と <span class="math"><span class="normal">⊗</span>
        </span> の間には分配法則も成り立ちません。


##<a id="sec-generated-title-8"></a> <a id="carginal"></a>基数
有限集合の場合、元の数が等しければ互いに同値でした。
ある集合と同値な自然数があればそれを元の数と呼びます。
これに習って、無限集合の場合、
集合と同値な順序数をもって元の数（の拡張概念）としたい所なのですが、
1つ問題があります。

「[順序数](#ordinal)」の説明で述べましたが、
順序数の大小関係としては、
<span class="math">ω <span class="normal">⊗</span> <span class="normal">2</span> <span class="normal">&gt;</span> <span class="normal">2</span> <span class="normal">⊗</span> ω <span class="normal">=</span> ω</span> という関係が成り立ちます。
しかしながら、集合の同値関係としては、（同値を ～ で表すと）
<span class="math">ω <span class="normal">⊗</span> <span class="normal">2</span> <span class="normal">∼</span> <span class="normal">2</span> <span class="normal">⊗</span> ω <span class="normal">∼</span> ω</span>
が成り立ちます。
この例からも分かるように、
ある集合と同値な順序数というのは1つに確定しません。

しかしながら、
証明は省きますが、
ある集合 <span class="math">a</span>と同値な順序数の中で最小の順序数ならばただ1つ確定します。
そして、このような順序数を
<span class="math">a</span> の<strong id="cardinal" class="keyword">基数</strong>（cardinal number）と呼び、
<span class="math">
        <span class="normal">|</span>a<span class="normal">|</span>
      </span> または
<span class="math">
        <span class="normal">card</span> a
      </span> で表します。


##<a id="sec-generated-title-9"></a> <a id="carginality"></a>濃度
「[基数](#cardinal)」の考え方により、
互いに同値な集合に対してただ1つ定まる「数」が定義されました。
そこで、集合 <span class="math">a</span> の基数を
「互いに同値な集合に共通の基本的な性質」という意味で、
<span class="math">a</span> の<strong id="cardinality" class="keyword">濃度</strong>（cardinality：基本的な性質）と呼びます。

有限集合の場合には、濃度は元の数に一致します。
それでは、無限集合の場合にはどうなるでしょうか。


###<a id="sec-generated-title-10"></a> <a id="countable"></a>可算濃度
自然数 <span class="math">ω</span> は最小の無限集合になります。
この自然数 <span class="math">ω</span> の濃度を<strong id="countable" class="keyword">可算濃度</strong>（countable cardinality）と呼び、
<span class="math">
          ‭א<sub>0</sub>
        </span> （アレフ0と読みます）で表します。
また、可算濃度を持つ集合を<strong id="countable" class="keyword">可算無限</strong>集合（countable infinite set）と呼びます。

自然数と同型になるような集合は 1, 2, 3, .... と番号を振っていくことが出来るので、
自然数で数え上げることが可能 ＝ 可算（countable）ということです。

余談になりますが、
この記号 <span class="math">
          ‭א
        </span> は、
ヘブライ文字の1文字目で、ギリシャ文字のα、ローマンアルファベットの a の元になった文字です。
無限基数の中で小さいものから順に、
<span class="math">
          ‭א<sub>0</sub>
        </span>,
<span class="math">
          ‭א<sub>1</sub>
        </span>,
<span class="math">
          ‭א<sub>2</sub>
        </span>,
・・・
と表します。
昔は、
無限基数を小さいものから順に、
ヘブライ文字の第 n 文字目で表していました
（aleph, beth, gimel, daleth, ・・・）が、
読めないし、写植の上でもなかなか表示できないので、
アレフの右下に添字を付ける今の表記法になりました。

それでは、自然数以外のよく知られた無限集合の濃度はどうなるでしょうか。


##### 
        <a id="sec-generated-title-11"></a>の濃度
自然数 <span class="math">n</span> と自然数全体の集合の直和
<span class="math">ω <span class="normal">⊕</span> n</span> は互いに同値です。
（可算無限集合に、有限個の元を加えても可算無限集合のまま。）
例えば、以下のような写像を考えると、
<span class="math">ω <span class="normal">⊕</span> n → ω</span> の全単斜になります。
<div class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">x</span>  </td><td><span class="paren">(</span><span class="math">x <span class="normal">∈</span> n</span><span class="paren">)</span></td></tr><tr><td><span class="math">x <span class="normal">+</span> n</span>  </td><td><span class="paren">(</span><span class="math">x <span class="normal">∈</span> ω</span><span class="paren">)</span></td></tr></table>
      </div>
したがって、
可算濃度 <span class="math">
          ‭א<sub>0</sub>
        </span> は、
任意の自然数 <span class="math">n</span> に対して、
<div class="math">
        n <span class="normal">+</span> ‭א<sub>0</sub> <span class="normal">=</span> ‭א<sub>0</sub>
      </div>
です。

また、
自然数全体の集合 <span class="math">ω</span> と、その直積集合 <span class="math">
          ω<sup><span class="normal">2</span></sup> <span class="normal">=</span> ω<span class="normal">×</span>ω
        </span> の間には、
図2に示すように1対1の関係を作ることが出来ます。

<figure>
	[![ωとω2の対応](../../../../assets/media/ufcpp2000/math/card00.emf)](../../../../assets/media/ufcpp2000/math/card00.emf)
	<figcaption>ωとω2の対応</figcaption>
</figure>


要するに、
<span class="math">
          f<span class="paren" style="font-size:em;">(</span>m, n<span class="paren" style="font-size:em;">)</span>
          <span class="normal">=</span>
          <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table><span class="paren" style="font-size:em;">(</span>m <span class="normal">+</span> n<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>m <span class="normal">+</span> n <span class="normal">+</span> <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span> <span class="normal">+</span> n
        </span>
という写像
<span class="math">
          f: ω<span class="normal">×</span>ω → ω
        </span>
を作ると、
これは
<span class="math">
          ω<span class="normal">×</span>ω → ω
        </span>
の全単斜になっています。

したがって、集合としては
<span class="math">ω</span> と <span class="math">
          ω<sup><span class="normal">2</span></sup>
        </span> は「[同値](map.md#equivalent)」になり、
これらの濃度は等しくなります。
さらに、
0 以外の任意の自然数 <span class="math">n</span> に対して、
<span class="math">ω</span> と <span class="math">
          ω<sup>n</sup>
        </span> も同値になり、
これらの濃度も等しくなります。

このことから、
可算濃度 <span class="math">
          ‭א<sub>0</sub>
        </span> は、
<div class="math">
        ‭א<sub>0</sub>
        <sup>n</sup>
        <span class="normal">=</span>
        ‭א<sub>0</sub>
      </div>
です。

これらの性質は無限集合独特のものです。
有限集合の場合、
真部分集合の濃度は元の集合の濃度よりも小さくなりますし、
直積集合の濃度は元の集合の濃度よりも大きくなりますが、
無限集合の場合にはこれが成り立ちません。


##### <a id="sec-generated-title-12"></a>整数の濃度
整数 <span class="math">
          <span class="bold">Z</span>
        </span> は、
集合的には
（同値な集合は互いに等しいものとすると）
<span class="math">
          <span class="bold">N</span>
          <span class="normal">⊂</span>
          <span class="bold">Z</span>
          <span class="normal">⊂</span>
          <span class="bold">N</span>
          <span class="normal">×</span>
          <span class="bold">N</span>
        </span>
になります。
（整数の定義の仕方は「[整数](integer.md)」を参照。）

このことと、
<span class="math">
          <span class="bold">N</span>
        </span>
と
<span class="math">
          <span class="bold">N</span>
          <span class="normal">×</span>
          <span class="bold">N</span>
        </span>
が同値なことから想像がつくように、
<span class="math">
          <span class="bold">N</span>
        </span>
と
<span class="math">
          <span class="bold">Z</span>
        </span>
も同値であることが示せます。
（2つの集合 <span class="math">a, b</span> に対して、
<span class="math">
          <span class="normal">|</span>a<span class="normal">|</span>
          <span class="normal">⊂</span>
          <span class="normal">|</span>b<span class="normal">|</span>
          <span class="normal">∧</span>
          <span class="normal">|</span>b<span class="normal">|</span>
          <span class="normal">⊂</span>
          <span class="normal">|</span>a<span class="normal">|</span>
          <span class="normal">⇒</span>
          <span class="normal">|</span>b<span class="normal">|</span>
          <span class="normal">=</span>
          <span class="normal">|</span>a<span class="normal">|</span>
        </span>
という定理がある（Bernstein の定理）。）
したがって、整数 <span class="math">
          <span class="bold">Z</span>
        </span> の濃度も可算濃度になります。
<div class="math">
        <span class="normal">|</span>
          <span class="bold">Z</span>
        <span class="normal">|</span>
        <span class="normal">=</span>
        <span class="normal">|</span>
          <span class="bold">N</span>
        <span class="normal">|</span>
        <span class="normal">=</span>
        ‭א<sub>0</sub>
      </div>

##### <a id="sec-generated-title-13"></a>有理数の濃度
整数のときと同じで、
有理数 <span class="math">
          <span class="bold">Q</span>
        </span> は、
集合的には
（同値な集合は互いに等しいものとすると）
<span class="math">
          <span class="bold">Z</span>
          <span class="normal">⊂</span>
          <span class="bold">Q</span>
          <span class="normal">⊂</span>
          <span class="bold">Z</span>
          <span class="normal">×</span>
          <span class="bold">Z</span>
        </span>
になります。
（有理数の定義の仕方は「[有理数](rational.md)」を参照。）

さらに、
<span class="math">
          <span class="bold">Z</span>
        </span>
と
<span class="math">
          <span class="bold">Z</span>
          <span class="normal">×</span>
          <span class="bold">Z</span>
        </span>
は同値であり、
したがって、有理数 <span class="math">
          <span class="bold">Q</span>
        </span> の濃度も可算濃度になります。
<div class="math">
        <span class="normal">|</span>
          <span class="bold">Q</span>
        <span class="normal">|</span>
        <span class="normal">=</span>
        <span class="normal">|</span>
          <span class="bold">Z</span>
        <span class="normal">|</span>
        <span class="normal">=</span>
        ‭א<sub>0</sub>
      </div>

###<a id="sec-generated-title-14"></a> <a id="infinite"></a>無限濃度に関する性質
まず、可算濃度 <span class="math">
          ‭א<sub>0</sub>
        </span> よりも大きな無限濃度が存在することについて説明します。
（といっても、ところどころ証明は省き、概要説明だけになりますが。）

（有限・無限を問わず）
ある集合 <span class="math">a</span> に対して、
その「[冪集合](set.md#power)」<span class="math">
          <span class="cursive">P</span>
          <span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
        </span> の濃度は
<span class="math">
          <span class="normal">2</span><sup>
            <span class="normal">|</span>a<span class="normal">|</span>
          </sup>
        </span>
になります。
そして、証明は省きますが、
<span class="math">a</span> と <span class="math">
          <span class="cursive">P</span>
          <span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
        </span> の間には全単写が存在しない
（＝ 同値にはならない）
ので、
<span class="math">
          <span class="normal">|</span>a<span class="normal">|</span> <span class="normal">&lt;</span>
          <span class="normal">2</span><sup>
            <span class="normal">|</span>a<span class="normal">|</span>
          </sup>
        </span>
になります。

ここで、<span class="math">a</span> の部分に自然数全体の集合 <span class="math">ω</span> を入れると、
<span class="math">
          ‭א<sub>0</sub> <span class="normal">&lt;</span>
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
        </span>
となり、可算濃度よりも大きな濃度が存在することが分かります。
可算濃度よりも大きな濃度を持つ集合を
非可算集合（uncountable set）と呼びます。
また、この式から、無限濃度がいくらでも作れることが分かります。

証明は省略しますが、
無限濃度に関して、
<span class="math">α</span> を無限濃度、
<span class="math">β</span> を濃度として、
以下の定理が成り立ちます。

* <span class="math">
            β <span class="normal">≦</span> α <span class="normal">⇒</span> α <span class="normal">+</span> β <span class="normal">=</span> α
          </span>

* <span class="math">
            <span class="normal">0</span> <span class="normal">&lt;</span> β <span class="normal">≦</span> α <span class="normal">⇒</span> αβ <span class="normal">=</span> α
          </span>

* <span class="math">n</span>を非0の自然数として、<span class="math">
            α<sup>n</sup> <span class="normal">=</span> α
          </span>

* <span class="math">
            α <span class="normal">&lt;</span> <span class="normal">2</span><sup>α</sup>
          </span>

* <span class="math">
            <span class="normal">2</span> <span class="normal">≦</span> β <span class="normal">≦</span> α <span class="normal">⇒</span> <span class="normal">2</span><sup>α</sup> <span class="normal">=</span> β<sup>α</sup>
          </span>


まとめると、
<span class="math">
          <span class="normal">2</span> <span class="normal">≦</span> β <span class="normal">≦</span> α
        </span> であるような濃度 <span class="math">β</span> に対し、
<div class="math">
        α <span class="normal">+</span> β <span class="normal">=</span> αβ <span class="normal">=</span> α
        <span class="normal">&lt;</span>
        <span class="normal">2</span><sup>α</sup>
        <span class="normal">=</span>
        <span class="normal">3</span><sup>α</sup>
        <span class="normal">=</span> <span class="normal">⋯</span> <span class="normal">=</span>
        β<sup>α</sup>
      </div>
となり、
見ての通り、
有限濃度（＝ 自然数）の場合と大きく異なります。


###<a id="sec-generated-title-15"></a> <a id="real"></a>連続濃度
自然数、整数、有理数の濃度はいずれも
<span class="math">
          ‭א<sub>0</sub>
        </span>
でした。
しかし、実数の濃度は
<span class="math">
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup> <span class="normal">&gt;</span> ‭א<sub>0</sub>
        </span>
になります。
このことは以下のようにして示されます。

「[実数の定義](real.md#real)」で説明したように、
実数は有理数のコーシー列の零数列による剰余体です。
このことから、
集合的には実数は有利数列 <span class="math">
          <span class="bold">Q</span>
          <sup>ω</sup>
        </span> の部分集合であり、
<span class="math">
          <span class="normal">|</span>
            <span class="bold">R</span>
          <span class="normal">|</span>
          <span class="normal">≦</span>
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
        </span>
が示されます。
<div class="math">
        <span class="bold">R</span>
        <span class="normal">⊂</span>
        <span class="bold">Q</span>
        <sup>ω</sup>
      </div><div class="math">
        ∴
        <span class="normal">|</span>
          <span class="bold">R</span>
        <span class="normal">|</span>
        <span class="normal">≦</span>
        <span class="normal">|</span>
          <span class="bold">Q</span>
          <sup>ω</sup>
        <span class="normal">|</span>
        <span class="normal">=</span>
        ‭א<sub>0</sub><sup>
          ‭א<sub>0</sub>
        </sup>
        <span class="normal">=</span>
        <span class="normal">2</span><sup>
          ‭א<sub>0</sub>
        </sup>
      </div>
なので、あとはこの逆
<span class="math">
          <span class="normal">|</span>
            <span class="bold">R</span>
          <span class="normal">|</span>
          <span class="normal">≧</span>
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
        </span>
を示す必要があるわけですが、
そのために、
<span class="math">
          <span class="paren" style="font-size:em;">{</span>
            <span class="normal">0</span>, <span class="normal">1</span>
          <span class="paren" style="font-size:em;">}</span>
        </span> の数列
<span class="math">
          <span class="normal">2</span><sup>ω</sup>
        </span>
（<span class="math">
          ω → <span class="normal">2</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
            <span class="normal">0</span>, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">}</span>
        </span> の写像）
を考えます。
数列 <span class="math">
          a<sub>n</sub> <span class="normal">∈</span> <span class="normal">2</span><sup>ω</sup>
        </span>
に対して、
以下のようなものを作ります。
<div class="math">
        <table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i <span class="normal">∈</span> ω</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">
            a<sub>i</sub>
          </td></tr><tr><td>
            <span class="normal">3</span><sup>i</sup>
          </td></tr></table>
      </div>
分母は 2 より大きい実数ならなんでもいいんですが、
この和は実数の値に収束します。
したがって、
数列全体の集合
<span class="math">
          <span class="normal">2</span><sup>ω</sup>
        </span>
は実数の部分集合と同値になり、
<span class="math">
          <span class="normal">|</span>
            <span class="bold">R</span>
          <span class="normal">|</span>
          <span class="normal">≧</span>
          <span class="normal">|</span>
            <span class="normal">2</span><sup>ω</sup>
          <span class="normal">|</span>
          <span class="normal">=</span>
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
        </span>
が示されます。

以上のことから、
<span class="math">
          <span class="normal">|</span>
            <span class="bold">R</span>
          <span class="normal">|</span>
          <span class="normal">=</span>
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
        </span>
が示されました。
「[無限濃度に関する性質](#infinite)」で説明したとおり、
<span class="math">
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
          <span class="normal">&gt;</span>
          ‭א<sub>0</sub>
        </span>
であり、
実数の濃度は可算濃度よりも大きいことになります。
この実数の濃度を連続の濃度あるいは<strong id="continuum" class="keyword">連続体濃度</strong>（cardinality of continuum）といい、
<span class="math">
          ‭א
          <span class="normal">=</span>
          <span class="normal">2</span><sup>
            ‭א<sub>0</sub>
          </sup>
        </span>
（添字なしのアレフ）で表します。


##<a id="sec-generated-title-16"></a> <a id="continuum"></a>連続体仮説
これまでに、
<span class="math">
        ‭א<sub>0</sub>
        <span class="normal">&lt;</span>
        <span class="normal">2</span><sup>
          ‭א<sub>0</sub>
        </sup>
      </span>
すなわち、
「[可算濃度可算無限](#countable)」
＜
「[連続体濃度](#continuum)」
であることを述べました。
ここで1つ別の疑問が生じます。
それは、
<span class="math">
        ‭א<sub>0</sub>
      </span>
と
<span class="math">
        <span class="normal">2</span><sup>
          ‭א<sub>0</sub>
        </sup>
      </span>
の間に位置する濃度が存在するのかどうかという疑問です。

無限濃度は無数にあり、
また、濃度には順序がありますので、
それを小さい方から
<span class="math">
        ‭א<sub>0</sub> ,
        ‭א<sub>1</sub> ,
        ‭א<sub>2</sub> ,
        <span class="normal">⋯</span>
      </span>
と添字を付けて表します。
この記法に従うと、
先ほどの疑問は
「
<span class="math">
        ‭א<sub>1</sub>
        <span class="normal">=</span>
        <span class="normal">2</span><sup>
          ‭א<sub>0</sub>
        </sup>
      </span>
」
が成り立つかどうかということになります。
この命題を「連続体仮説」と呼びます。
より一般化すると、
任意の自然数 <span class="math">n</span> に対して、
「
<span class="math">
        ‭א<sub>n＋1</sub>
        <span class="normal">=</span>
        <span class="normal">2</span><sup>
          ‭א<sub>n</sub>
        </sup>
      </span>
」
が成り立つかどうかという命題になり、
これを「一般連続体仮説」と呼びます。

さて、この疑問に対する答えですが、
実は、
「[ZFC公理系](axiom.md#zfc)」
の範囲では否定も肯定も出来ません。
すなわち、
「ZFC が無矛盾だとすると、それに連続体仮説を加えた公理系も無矛盾」
と
「ZFC が無矛盾だとすると、それに連続体仮説の否定を加えた公理系も無矛盾」
がどちらも証明されています。
