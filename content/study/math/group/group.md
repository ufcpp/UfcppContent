---
title: "群"
source_url: "https://ufcpp.net/study/math/group/group/"
content_type: "Article"
published_at: "2015-05-06T14:17:20"
updated_at: "2015-05-06T14:17:20"
tags: []
umbraco_id: 1485
parent_id: 1483
sort_order: 1
aliases:
  - "/group/group"
  - "/group/group.html"
  - "/math/group/group/"
  - "/study/group/group"
  - "/study/group/group.html"
---

# 群

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

まずは、算法を1つ持つ代数系の分類について説明します。
このような代数系の分類として、群・半群などがあります。


## <a id="sec-generated-title-2"></a> <a id="group"></a>群とは

ある代数系<span class="math"><span class="paren" style="font-size:em;">(</span>G,・<span class="paren" style="font-size:em;">)</span></span>に対して、以下の条件を考えます。

1. 「[結合法則](algebraic.md#associative)」が成り立つ。

2. 「[単位元](algebraic.md#unity)」が存在する。

3. 「[逆元](algebraic.md#inverse)」が存在する。


代数系<span class="math">G</span>が
1. を満たすとき、<strong id="semigroup" class="keyword">半群</strong>(semi-group)とよび、
1. 2. を満たすとき、<strong id="monoid" class="keyword">モノイド</strong>(monoid)と呼びます。
また、1.～3. の全てを満たすとき、<span class="math">G</span>を<strong id="group" class="keyword">群</strong>(group)と呼びます。
 
さらに、群(半群、モノイド)の中で、
「[交換法則](algebraic.md#commutative)」を満たすものを可換群(可換半群、可換モノイド)と呼びます。
 
可換群は<strong id="abelian" class="keyword">アーベル群</strong>(abelian group)もしくは加法群(additive group)とも呼ばれ、
その算法は、しばしば <span class="math">＋</span> を用いて表します。
（逆に言うと、<span class="math">＋</span> を用いて表される算法は暗黙的に可換算法であると考えることが多い。）


### <a id="sec-generated-title-3"></a> <a id="d53e73"></a>余談

群という概念は、ニールス・ヘンリック・アーベル（Niels Henrik Abel、ノルウェーの数学者）が
5次方程式の一般解法の存在の有無を調べるために考えたものです。
この当時、アーベルは可換群しか想定していませんでした。
後に、ガロアが群論を構築する際、非可換なものも群として考え、
アーベルの考えた群は可換群として区別するようになりました。
可換群をアーベル群と呼ぶのはこの名残です。


## <a id="sec-generated-title-4"></a> <a id="sample"></a>群の例

半群・群の例をいくつか紹介します。
<h4>自然数</h4>
自然数 <span class="math">ω</span> は加法 <span class="math">＋</span> に関して可換モノイドになります。
<span class="math">m ∈ ω, m ≠ 0</span> に対して、
<span class="math">－m</span> はもはや自然数ではないので、
逆元の存在しない元があり、
群にはなっていません。
 
ただし、<span class="math">m ＋ 0 ＝ m</span> であり、
単位元 0 が存在するのでモノイドになります。
 
また、自然数は乗法 <span class="math">×</span> に関して可換モノイドになります。
（単位元は 1 です。）
<h4>偶数全体</h4>
偶数全体の集合
<span class="math">E ＝ <span class="paren" style="font-size:em;">{</span>x | x ∈ <span class="bold">Z</span> ∧ x ≡ 0 <span class="paren" style="font-size:em;">(</span>mod 2<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></span>
は乗法 <span class="math">×</span> に関して可換半群になります。
（偶数同士の掛け算結果はやはり偶数になるので、代数系になる。）
 
自然数の場合と異なり、
この集合はモノイドではありません。
（1 がないので単位元がない。）
<h4>整数</h4>
整数は <span class="math"><span class="bold">Z</span></span> は加法 <span class="math">＋</span> に関して可換群になります。
単位元は 0、
<span class="math">p ∈ <span class="bold">Z</span></span> の逆元は <span class="math">－p</span> です。
 
また、乗法に関しては、可換モノイドになります。
（整数同士の割り算は整数になるとは限らない。単位元は 1。）


### <a id="sec-generated-title-5"></a> <a id="finite"></a>有限群

自然数や整数などは無限集合ですが、
有限集合になるような群も存在します。
<h4>1 の n 乗根</h4>
1 の <span class="math">n</span> 乗根になるような複素数
<span class="math">
Ω<sub>n</sub>
＝
<span class="paren" style="font-size:em;">{</span>ω | ω ∈ <span class="bold">C</span> ∧ n ∈ <span class="bold">N</span> ∧ ω<sup>n</sup> ＝ 1<span class="paren" style="font-size:em;">}</span></span>
は <span class="math">n</span> 個の元からなる集合になります。
 
この集合は、複素数の乗算に関して可換群になります。
例えば、<span class="math">a, b ∈ Ω<sub>n</sub></span> に対して、
<span class="math">c ＝ a b ＝ b a</span> と置くと、
<span class="math">
c<sup>n</sup>
＝
<span class="paren" style="font-size:em;">(</span>a b<span class="paren" style="font-size:em;">)</span><sup>n</sup>
＝
a<sup>n</sup> b<sup>n</sup>
＝ 1 × 1 ＝ 1
</span>
なので、<span class="math">c ∈ Ω<sub>n</sub></span> になります。
また、
<span class="math">a × a<sup>n － 1</sup> ＝ a<sup>n</sup> ＝ 1</span> なので、
<span class="math">a</span> には必ず逆元 <span class="math">a<sup>n － 1</sup></span> が存在します。
さらに、<span class="math">1 ∈ Ω<sub>n</sub></span> で <span class="math">∀ω ∈ Ω<sub>n</sub>, ω × 1 ＝ ω</span> なので、単位元も存在します。
<h4>置換群</h4>
有限集合に対する「[全単写](../set/map.md#bijection)」を置換（permutation）といいます。
置換という言葉は、
<span class="math"><span class="paren" style="font-size:em;">(</span>a, b, c<span class="paren" style="font-size:em;">)</span></span> を
<span class="math"><span class="paren" style="font-size:em;">(</span>a, c, b<span class="paren" style="font-size:em;">)</span></span> に並べ替えるような操作という意味です。
 
ここでは単純化のため、3つの元に対する置換に付いてのみ説明しますが、
元の数が3以外の場合でも同様のことが成り立ちます。
置換は以下のような書き方で表現します。
<div class="math">
        <span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>1</td><td>2</td><td>0</td></tr></table><span class="paren" style="font-size:2em;">]</span>
      </div>
この例は、0番目の元を1番目の元と、
1番目を2番目と、
2番目を0番目と入れ替えるという意味です。
例として、
<span class="math"><span class="paren" style="font-size:em;">(</span>a, b, c<span class="paren" style="font-size:em;">)</span></span> に対してこの置換を適用すると、
<span class="math"><span class="paren" style="font-size:em;">(</span>b, c, a<span class="paren" style="font-size:em;">)</span></span> になります。
この記法で表すなら、
3つの元に対する置換は以下の6通りになります。
<div class="math">
        <span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>0</td><td>1</td><td>2</td></tr></table><span class="paren" style="font-size:2em;">]</span>
, 
<span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>0</td><td>2</td><td>1</td></tr></table><span class="paren" style="font-size:2em;">]</span>
, 
<span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>2</td><td>1</td><td>0</td></tr></table><span class="paren" style="font-size:2em;">]</span>
, 
<span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>1</td><td>0</td><td>2</td></tr></table><span class="paren" style="font-size:2em;">]</span>
, 
<span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>1</td><td>2</td><td>0</td></tr></table><span class="paren" style="font-size:2em;">]</span>
, 
<span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="matrix"><tr><td>0</td><td>1</td><td>2</td></tr><tr><td>2</td><td>0</td><td>1</td></tr></table><span class="paren" style="font-size:2em;">]</span></div>
置換は、写像の合成に関して群を成していて、
この群を置換群（permutation group）と呼びます。


## <a id="sec-generated-title-6"></a> <a id="concept"></a>群に関する諸概念

### <a id="sec-generated-title-7"></a> <a id="order"></a>位数

群の元の数を<strong id="order" class="keyword">位数</strong>（order）といいます。
集合の元の数と同様に、群 <span class="math">G</span> の位数を <span class="math"><span class="normal">|</span>G<span class="normal">|</span></span> と表します。
「[群の例](#sample)」で出てきた例に関しては、
整数や偶数全体の集合の位数は無限、
1 の <span class="math">n</span> 乗根の位数は <span class="math">n</span>、
n 元に対する置換群の位数は <span class="math">n!</span> になります。


### <a id="sec-generated-title-8"></a> <a id="isomorphic"></a>同型

二つの群
<span class="math"><span class="paren" style="font-size:em;">{</span>G, ・<sub>G</sub><span class="paren" style="font-size:em;">}</span>
, 
<span class="paren" style="font-size:em;">{</span>H, ・<sub>H</sub><span class="paren" style="font-size:em;">}</span></span>
の間に、
「[全単写](../set/map.md#bijection)」<span class="math">f : G → H</span> で、
任意の
<span class="math">
a, b ∈ G
</span>
に対して、
条件
<div class="math">
f<span class="paren" style="font-size:em;">(</span>a ・<sub>G</sub> b<span class="paren" style="font-size:em;">)</span>
＝
f<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ・<sub>H</sub> f<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span></div>
を満たすものが存在するとき、
この写像 <span class="math">f</span> を群同型写像（group isomorphism）と呼び、
2つの群は互いに<strong id="g_isomorphic" class="keyword">群同型</strong>である（group isomorphic）と言います。
（群であることが明白である場合、単に<strong id="isomorphic" class="keyword">同型</strong>であると言います。）
 
すなわち、群同型とは、
2つの群が集合として「[同値](../set/map.md#equivalent)」であり、
さらに、算法の結果にも1対1の対応が取れることを言います。
 
例として、以下のような2つの群
<div class="math">
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">{</span>0, 1, 2<span class="paren" style="font-size:em;">}</span>, ＋<span class="paren" style="font-size:em;">}</span>
      </div><div class="math">
0 ＋ 1 ＝ 1 ＋ 0 ＝ 2 ＋ 2 ＝ 1
</div><div class="math">
0 ＋ 2 ＝ 2 ＋ 0 ＝ 1 ＋ 1 ＝ 2
</div><div class="math">
1 ＋ 2 ＝ 2 ＋ 1 ＝ 0 ＋ 0 ＝ 0
</div>
と
<div class="math">
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">{</span>a, b, c<span class="paren" style="font-size:em;">}</span>, ・<span class="paren" style="font-size:em;">}</span>
      </div><div class="math">
a ・ b ＝ b ・ a ＝ c ・ c ＝ b
</div><div class="math">
a ・ c ＝ c ・ a ＝ b ・ b ＝ c
</div><div class="math">
b ・ c ＝ c ・ b ＝ a ・ a ＝ a
</div>
を考えます。
まあ、この例は単純なので、ぱっと見で自明だと思いますが、
<span class="math">
f<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span> ＝ a, 
f<span class="paren" style="font-size:em;">(</span>1<span class="paren" style="font-size:em;">)</span> ＝ b, 
f<span class="paren" style="font-size:em;">(</span>2<span class="paren" style="font-size:em;">)</span> ＝ c
</span>
となるような写像 <span class="math">f</span> を用意すれば、同型になります。
 
要するに、同型という概念は、
外面だけ見ると、<span class="math">0, 1, 2</span> と <span class="math">a, b, c</span> と言うように
異なる群のように見えるかもしれないけれども、
実質的には同じ構造をになっているようなもののことを言います。


### <a id="sec-generated-title-9"></a> <a id="subgroup"></a>部分群

群 <span class="math"><span class="paren" style="font-size:em;">{</span>G, ・<span class="paren" style="font-size:em;">}</span></span> があるとき、
<span class="math">G</span> の部分集合 <span class="math">H</span> が <span class="math">・</span> に関して群を成しているとき、
群 <span class="math"><span class="paren" style="font-size:em;">{</span>H, ・<span class="paren" style="font-size:em;">}</span></span> を
<span class="math"><span class="paren" style="font-size:em;">{</span>G, ・<span class="paren" style="font-size:em;">}</span></span> の<strong id="subgroup" class="keyword">部分群</strong>（sub group）と呼びます。
 
例えば、整数は加法に関して有理数の部分群になります。
 
また、整数 <span class="math"><span class="bold">Z</span></span> に対して、
偶数全体の集合や、
3の倍数の集合は加法に関して整数の部分群になっています。
（偶数同士を足すとやはり偶数になるし、
3の倍数同士もやはり3の倍数。）
より一般的にいうと、
<em>ある整数 <span class="math">N</span> の倍数全体の集合は整数の部分群になります</em>。
 
さらに言うと、
「[環](field.md#ring)」<span class="math">R</span> に対して、<span class="math">R</span> の元のうちからの1つ <span class="math">a</span> を選んだとき、
<div class="math">
G ＝ <span class="paren" style="font-size:em;">{</span>x | ∃y ∈ R, x ＝ a y<span class="paren" style="font-size:em;">}</span></div>
と表せるような（形式的には <span class="math">a</span> の倍数になっているような）集合 <span class="math">G</span> は <span class="math">R</span> の部分群になります。
（詳しくは「[環・体](field.md)」で述べますが、実際は部分環になります。）
このような形の部分群を <span class="math">aR</span> と書き表します。
（この記法を用いると、
偶数全体の集合は <span class="math">2<span class="bold">Z</span></span>、
3の倍数全体の集合は <span class="math">3<span class="bold">Z</span></span> となります。）
 
注： <span class="math">G</span> が非可換群の場合には、
<span class="math">g</span> を左からかけた場合 <span class="math">gG</span> と、右から書けた場合 <span class="math">Gg</span> の2つが考えられます。


### <a id="sec-generated-title-10"></a> <a id="residual"></a>剰余群

群 <span class="math">G</span> とその部分群 <span class="math">H</span> があるときに、以下のようにして新しい集合を定義することを考えます。

* <span class="math">a, b ∈ G</span>

* <span class="math">a b<sup>－</sup> ∈ H</span>のとき、<span class="math">a</span>と<span class="math">b</span>は「[同値](../set/integer.md#equivalent)」であるものとする。

* この同値関係を使って「[商集合](../set/integer.md#quotient_set)」<span class="math">G/H</span>を作る。

* <span class="math">G/H</span>の元に対して、<span class="math">G</span>と同じ算法を適用する。


このようにして作った商集合 <span class="math">G/H</span> は、
<span class="math">G</span> と同じ算法に関して群になります。
このような群を<strong id="residual" class="keyword">剰余群</strong>（residual group）もしくは商群（quotient group）と呼びます。
<h4>整数の剰余群</h4>
簡単な例として、剰余群 <span class="math"><span class="bold">Z</span>/3<span class="bold">Z</span></span> を挙げてみます。
上述の手順において、
<span class="math">G ＝ <span class="bold">Z</span></span>、
<span class="math">H ＝ 3<span class="bold">Z</span></span> として、
商集合を作ると、
<span class="math">G/H ＝ <span class="paren" style="font-size:em;">{</span><span class="bar">0</span>, <span class="bar">1</span>, <span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span>
になります。
<span class="math">G/H</span> の元に対して、
<span class="math">G</span> の加法をそのまま適用すると、
<div class="math">
        <span class="bar">0</span> ＋ <span class="bar">0</span> ＝ <span class="bar">0 ＋ 0</span> ＝ <span class="bar">0</span></div><div class="math">
        <span class="bar">0</span> ＋ <span class="bar">1</span> ＝ <span class="bar">0 ＋ 1</span> ＝ <span class="bar">1</span></div><div class="math">
        <span class="bar">0</span> ＋ <span class="bar">2</span> ＝ <span class="bar">0 ＋ 2</span> ＝ <span class="bar">2</span></div><div class="math">
        <span class="bar">1</span> ＋ <span class="bar">1</span> ＝ <span class="bar">1 ＋ 1</span> ＝ <span class="bar">2</span></div><div class="math">
        <span class="bar">1</span> ＋ <span class="bar">2</span> ＝ <span class="bar">1 ＋ 2</span> ＝ <span class="bar">3</span> ＝ <span class="bar">0</span></div><div class="math">
        <span class="bar">2</span> ＋ <span class="bar">2</span> ＝ <span class="bar">2 ＋ 2</span> ＝ <span class="bar">4</span> ＝ <span class="bar">1</span></div>
となります。
単位元は <span class="math"><span class="bar">0</span></span> で、
<span class="math"><span class="bar">1</span></span> の逆元は
<span class="math"><span class="bar">2</span></span> になります。
<h4>1 の n 乗根の剰余群</h4>
「[有限群](#finite)」で述べたように、
1 の <span class="math">n</span> 乗根で作った集合は複素数の乗法に関して群を成します。
実は、<span class="math">n</span> が素数ではないとき、この群は部分群を持ちます。
 
ここでは例として、<span class="math">n ＝ 6</span> の場合を挙げます。
まず、1 の3乗根のうちで、偏角の最も小さいものを <span class="math">ω</span> としましょう。
そうすると、1 の6乗根は、
<span class="math">Ω<sub>6</sub> ＝ <span class="paren" style="font-size:em;">{</span>1, ω, ω<sup>2</sup>, －1, －ω, －ω<sup>2</sup><span class="paren" style="font-size:em;">}</span></span>
の6つになります。
見るからに明らかですが、この6つのうちの
<span class="math"><span class="paren" style="font-size:em;">{</span>1, ω, ω<sup>2</sup><span class="paren" style="font-size:em;">}</span></span>
の3つは 1 の3乗根<span class="math">Ω<sub>3</sub></span>ですので、群になっています。
すなわち、
<span class="math">Ω<sub>3</sub></span> は
<span class="math">Ω<sub>6</sub></span> の部分群になります。
ちなみに、<span class="math">Ω<sub>2</sub> ＝ <span class="paren" style="font-size:em;">{</span>1, －1<span class="paren" style="font-size:em;">}</span></span> も
<span class="math">Ω<sub>6</sub></span> の部分群です。
 
さて、それでは、これらを使って剰余群を作ってみましょう。
まずは、<span class="math">Ω<sub>3</sub></span> を使った剰余群ですが、
<div class="math">
ω/1 ＝ ω ∈Ω<sub>3</sub>, 
ω<sup>2</sup>/1 ＝ ω<sup>2</sup> ∈Ω<sub>3</sub>, 
ω<sup>2</sup>/ω ＝ ω ∈Ω<sub>3</sub>, 
</div><div class="math">
－ω/－1 ＝ ω ∈Ω<sub>3</sub>, 
－ω<sup>2</sup>/－1 ＝ ω<sup>2</sup> ∈Ω<sub>3</sub>, 
－ω<sup>2</sup>/－ω ＝ ω ∈Ω<sub>3</sub>, 
</div>
なので、「[同値類](../set/integer.md#eq_class)」は
<div class="math">
        <span class="bar">1</span> ＝ <span class="paren" style="font-size:em;">{</span>1, ω, ω<sup>2</sup><span class="paren" style="font-size:em;">}</span>, 
<span class="bar">－1</span> ＝ <span class="paren" style="font-size:em;">{</span>－1, －ω, －ω<sup>2</sup><span class="paren" style="font-size:em;">}</span></div>
の2つなので、剰余群は
<div class="math">
Ω<sub>6</sub> / Ω<sub>3</sub>
＝
<span class="paren" style="font-size:em;">{</span><span class="bar">1</span>, <span class="bar">－1</span><span class="paren" style="font-size:em;">}</span></div>
になります。
まあ、見るからにそうなんですが、
これは <span class="math">Ω<sub>2</sub></span> と「[群同型](#g_isomorphic)」になります。
同様に、<span class="math">Ω<sub>2</sub></span> を使った剰余群は、
<div class="math">
1/－1 ＝ －1 ∈Ω<sub>2</sub>, 
ω/－ω ＝ －1 ∈Ω<sub>2</sub>, 
ω<sup>2</sup>/－ω<sup>2</sup> ＝ －1 ∈Ω<sub>3</sub>, 
</div>
なので、同値類は
<div class="math">
        <span class="bar">1</span> ＝ <span class="paren" style="font-size:em;">{</span>1, －1<span class="paren" style="font-size:em;">}</span>, 
<span class="bar">ω</span> ＝ <span class="paren" style="font-size:em;">{</span>ω, －ω<span class="paren" style="font-size:em;">}</span>, 
<span class="bar">ω<sup>2</sup></span> ＝ <span class="paren" style="font-size:em;">{</span>ω<sup>2</sup>, －ω<sup>2</sup><span class="paren" style="font-size:em;">}</span></div>
の3つになり、
<div class="math">
Ω<sub>6</sub> / Ω<sub>2</sub>
＝
<span class="paren" style="font-size:em;">{</span><span class="bar">1</span>, <span class="bar">ω</span>, <span class="bar">ω<sup>2</sup></span><span class="paren" style="font-size:em;">}</span></div>
になります。
これも見るからに、<span class="math">Ω<sub>3</sub></span> と群同型です。
要するに、群同型を記号 <span class="math">～</span> で表すと、
<em>
        <div class="math">
Ω<sub>6</sub> / Ω<sub>2</sub>
～
Ω<sub>3</sub></div>
        <div class="math">
Ω<sub>6</sub> / Ω<sub>3</sub>
～
Ω<sub>2</sub></div>
      </em>
となります。
実はこの逆もまた叱りで、
<span class="math">Ω<sub>2</sub></span> と <span class="math">Ω<sub>3</sub></span> の直積集合
<div class="math">
Ω<sub>2</sub> × Ω<sub>3</sub>
＝
<span class="paren" style="font-size:em;">{</span><span class="paren" style="font-size:em;">(</span>1, 1<span class="paren" style="font-size:em;">)</span>, 
 <span class="paren" style="font-size:em;">(</span>1, ω<span class="paren" style="font-size:em;">)</span>, 
 <span class="paren" style="font-size:em;">(</span>1, ω<sup>2</sup><span class="paren" style="font-size:em;">)</span>, 
 <span class="paren" style="font-size:em;">(</span>－1, 1<span class="paren" style="font-size:em;">)</span>, 
 <span class="paren" style="font-size:em;">(</span>－1, ω<span class="paren" style="font-size:em;">)</span>, 
 <span class="paren" style="font-size:em;">(</span>－1, ω<sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></div>
に対して、
<span class="math"><span class="paren" style="font-size:em;">(</span>1, 1<span class="paren" style="font-size:em;">)</span> → 1</span>、
<span class="math"><span class="paren" style="font-size:em;">(</span>1, ω<span class="paren" style="font-size:em;">)</span>,  → ω</span>、
<span class="math"><span class="paren" style="font-size:em;">(</span>1, ω<sup>2</sup><span class="paren" style="font-size:em;">)</span>,  → ω<sup>2</sup></span>、
<span class="math"><span class="paren" style="font-size:em;">(</span>－1, 1<span class="paren" style="font-size:em;">)</span>,  → －1</span>、
<span class="math"><span class="paren" style="font-size:em;">(</span>－1, ω<span class="paren" style="font-size:em;">)</span>,  → －ω</span>、
<span class="math"><span class="paren" style="font-size:em;">(</span>－1, ω<sup>2</sup><span class="paren" style="font-size:em;">)</span> → －ω<sup>2</sup></span>
というように同一視して考えると、
<em>
        <div class="math">
Ω<sub>6</sub>
～
Ω<sub>2</sub> × Ω<sub>3</sub></div>
      </em>
となります。


### <a id="sec-generated-title-11"></a> <a id="s_to_g"></a>半群から群を機械的に作る

実は、半群から群を機械的に作ることが出来ます。
最も分かりやすい例は、自然数から整数を定義する手順なんですが、
これに関しては「[整数の定義](../set/integer.md#integer)」で説明しています。
 
商集合が群になることの証明はここでは省略しますが、
この手順を任意の半群 <span class="math">S</span> に対して一般化すると、以下のようになります。

* 半群<span class="math">S</span>の対<span class="math"><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span> ∈ S×S</span>を用意する。

* 2つの対<span class="math">m ＝ <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>, n ＝ <span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span></span>に対して、「<span class="math">ad ＝ bc</span>のとき互いに同値」という同値関係<span class="math">～</span>を定める。

* この同値関係を使って商集合<span class="math">S×S / ～</span>を作る。

* この商集合は群になる。

* 形式的に、対<span class="math"><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span></span>を<span class="math">ab<sup>－1</sup></span>と書く。

* <span class="math">m</span>と<span class="math">n</span>との間の算法は<span class="math">mn ＝ <span class="paren" style="font-size:em;">(</span>ac, bd<span class="paren" style="font-size:em;">)</span></span>で定める。



## <a id="sec-generated-title-12"></a> <a id="plan"></a>執筆予定

<pre>
・部分群

・巡回群とかの説明もここ？

ある元 a があって、
全ての元が a の冪で書き表せるとき、その群を巡回群という。
a のことは生成元という。
</pre>
