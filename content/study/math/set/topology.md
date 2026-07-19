---
title: "位相"
source_url: "https://ufcpp.net/study/math/set/topology/"
content_type: "Article"
published_at: "2015-05-06T14:17:14"
updated_at: "2015-05-06T14:17:14"
tags: []
umbraco_id: 1482
parent_id: 1471
sort_order: 10
aliases:
  - "/math/set/topology/"
  - "/set/topology"
  - "/set/topology.html"
  - "/study/set/topology"
  - "/study/set/topology.html"
---

# 位相

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

数学では位相とはtopologyのこと。
物理で言う所の位相（phase）は相と言うことが多い。
 
位相というのは、空間上の2点（集合中の2元）の間が遠いか近いか、
連続かどうかなどを論じるための概念です。
例えば、距離空間（2点間の距離を定義できる空間）は位相空間の一種になります。
ただし、位相の概念は距離の概念よりも条件が緩く、広い概念です。
 
位相は、
関数の連続性や微分積分などのいわゆる「解析学」の下地となる基本的な概念になります。


## <a id="sec-generated-title-2"></a> <a id="plan"></a>執筆予定

```text
・位相
「距離が定義できる」というのは実はかなり厳しい条件。
もっと緩い条件でも連続性の議論が出来ないかという発想の元に生まれたのが位相空間。


実は、「f(x) が開集合→ x も開集合」となるような写像 f を
連続写像として定義しても連続性の議論ができる。
ということは、開集合とそうでない集合の区別さえ付けば OK。
（この条件は「距離が定義できる」よりもはるかに緩い）

ということで、ある集合 S と、その部分集合の集合 O∈P(S) （P は冪集合）を用意して、
S の部分集合 S1 が O に含まれるなら S1 は開集合であると考える。

S と O の対 (S, O) を位相空間、O を<strong id="topology" class="keyword">位相</strong>と呼ぶ。


位相は上述のような「開集合となるような集合」以外にも、
閉包関数（閉包作用素）というものを用いても定義可能。
閉包関数とは、ある集合 A に対して、A を含む最小の閉集合を対応させる関数。

閉包関数 Cl に対して、Cl(A) ＝ A となるような集合を閉集合として定義でき、
閉集合の補集合として開集合を定義できるので、
閉包関数を定めれば、「開集合となるような集合」も定まる。


距離が定義できれば位相も定義可能。
（距離空間は位相空間である）
その逆はいえない。
（「距離が定義できる」方がより厳しい条件）

2つの位相空間が集合として「[同値](/study/math/set/map?key=equivalent)」で、
かつ、位相構造も保つとき、2つの位相空間は同相であるといいます。
すなわち、
(A, O<sub>A</sub>) と (B, O<sub>B</sub>) に対して、
A → B の「[全単写](/study/math/set/map?key=bijection)」 f で、
f 自身も f の逆関数も連続写像になるようなものがあるとき、
この写像 f を<strong id="homeomorphism" class="keyword">同相写像</strong>（homeomorphism）と呼び、
2つの位相空間を同相である（homeomorphic）といいます。
```
