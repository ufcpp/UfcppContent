---
title: "距離"
source_url: "https://ufcpp.net/study/math/set/metric/"
content_type: "Article"
published_at: "2015-05-06T14:17:12"
updated_at: "2015-05-06T14:17:12"
tags: []
umbraco_id: 1481
parent_id: 1471
sort_order: 9
aliases:
  - "/math/set/metric/"
  - "/set/metric"
  - "/set/metric.html"
  - "/study/set/metric"
  - "/study/set/metric.html"
---

# 距離

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

空間上の2点（集合中の2元）の間が遠いか近いか、
連続かどうかなどを論じるためには、
「[位相](topology.md#topology)」
という概念が必要になります。
この位相という概念は、
距離の概念の条件を緩め、より多くの集合の幾何学的な構造を調べるために考えられるものです。
 
この位相という抽象的な概念の話に入る前に、
ここではまず、直感的に想像しやすい距離というものの話をしましょう。


## <a id="sec-generated-title-2"></a> <a id="metric_example"></a>距離の例

最も一般的に使われる距離というと、
ピタゴラスの定理に基づいて定義されるユークリッド距離でしょう。
説明の簡単化のために2次元で説明しますが、
2次元の実空間 <span class="math"><span class="bold">R</span><sup>2</sup></span> 上のユークリッド距離は以下のようにして定義されます。
<div class="math">
      <span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>1</sub> , x<sub>2</sub><span class="paren" style="font-size:em;">)</span>,
<span class="vector">y</span> ＝ <span class="paren" style="font-size:em;">(</span>y<sub>1</sub> , y<sub>2</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
d<span class="paren" style="font-size:em;">(</span><span class="vector">x</span>, <span class="vector">y</span><span class="paren" style="font-size:em;">)</span>
＝
<span class="normal" style="font-size:em;">√</span><span class="bar"><span class="paren" style="font-size:em;">(</span>x<sub>1</sub> － y<sub>1</sub><span class="paren" style="font-size:em;">)</span><sup>2</sup>
＋
<span class="paren" style="font-size:em;">(</span>x<sub>2</sub> － y<sub>2</sub><span class="paren" style="font-size:em;">)</span><sup>2</sup></span></div>
しかしながら、距離というのはこのユークリッド距離以外にも定義できます。
例えば、碁盤の目状にしか移動できないという制約のある空間を考えます。
日本的に言うと、京都の街のように道路が碁盤の目状に通っている所を歩くようなものです。
アメリカ的にはマンハッタンの街を考えるようです（といっても、アメリカの都会はどこも碁盤の目状に道路が整備されているんですが）。
このような空間上で2点間の最短経路長はどうなるかというと、
以下の式のようになります。
<div class="math">
d<span class="paren" style="font-size:em;">(</span><span class="vector">x</span>, <span class="vector">y</span><span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">|</span>x<sub>1</sub> － y<sub>1</sub><span class="normal">|</span>
＋
<span class="normal">|</span>x<sub>2</sub> － y<sub>2</sub><span class="normal">|</span></div>
この式によって定義される距離をマンハッタン距離と呼びます。


## <a id="sec-generated-title-3"></a> <a id="general_metric"></a>距離の一般化

前節で述べたように、
距離の定義の仕方にもいろいろあります。
また、先ほどは実空間上の距離の例しか挙げませんでしたが、
任意の集合に対する距離も定義したいところです。
 
でも、どんな定義の仕方でもいいというわけではなくて、
最低限満たしていて欲しい条件というものがあります。
距離の満たすべき条件を考える上で、
参考としてユークリッド距離の満たす条件を挙げてみましょう。
ユークリッド距離 <span class="math">d</span> は、任意の実数 <span class="math">x, y, z</span> に対して、

1. <span class="math">d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ≧ 0</span>

2. <span class="math">x ＝ y ⇔ d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＝ 0</span><span class="math"></span>

3. <span class="math">d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＝ d<span class="paren" style="font-size:em;">(</span>y, x<span class="paren" style="font-size:em;">)</span></span>

4. <span class="math">d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＋ d<span class="paren" style="font-size:em;">(</span>y, z<span class="paren" style="font-size:em;">)</span> ≧ d<span class="paren" style="font-size:em;">(</span>x, z<span class="paren" style="font-size:em;">)</span></span>


を満たします。
そこで、これを参考にして、
集合 <span class="math">X</span> の距離を以下のように定義します。
 
直積 <span class="math">X × X</span> から実数 <span class="math"><span class="bold">R</span></span> への写像 <span class="math">d</span> で、
<span class="math">X</span> の任意の元 <span class="math">x, y, z</span> に対して、

1. <span class="math">d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ≧ 0</span>

2. <span class="math">x ＝ y ⇔ d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＝ 0</span><span class="math"></span>

3. <span class="math">d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＝ d<span class="paren" style="font-size:em;">(</span>y, x<span class="paren" style="font-size:em;">)</span></span>

4. <span class="math">d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＋ d<span class="paren" style="font-size:em;">(</span>y, z<span class="paren" style="font-size:em;">)</span> ≧ d<span class="paren" style="font-size:em;">(</span>x, z<span class="paren" style="font-size:em;">)</span></span>


を満たすようなものを <span class="math">X</span> の距離または<strong id="metric" class="keyword">計量</strong>（distance または metric）と呼びます。
集合 <span class="math">X</span> と計量 <span class="math">d</span> をセットにして（順序対にして）、
<span class="math"><span class="paren" style="font-size:em;">(</span>X, d<span class="paren" style="font-size:em;">)</span></span>
を<strong id="metric_space" class="keyword">距離空間</strong>（metric space）と呼び、
<span class="math">X</span> に計量が導入されたといいます。
 
このようにして距離の概念が抽象化されたことによって、
図形的なイメージの沸かないような集合に対しても距離が導入できるようになります。
例えば、有解な実関数 <span class="math">f, g</span> に対して、
<div class="math">
d<span class="paren" style="font-size:em;">(</span>f, g<span class="paren" style="font-size:em;">)</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table><span class="normal">|</span>
 f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup><span class="normal">d</span>x
</div>
という写像 <span class="math">d</span> を定義すると、
これは距離の条件を満たし、
実関数空間に距離を導入できます。


### <a id="sec-generated-title-4"></a> <a id="d50e309"></a>ノルム

ユークリッド距離やマンハッタン距離の例を見ても明らかですが、
同じ集合に対して異なる距離を導入することが出来ます。
ところで、この2つの距離ですが、
ユークリッド距離は2乗和、
マンハッタン距離は絶対値和になっています。
 
このことをより一般化して、
任意の n 次元ベクトル
<div class="math">
        <span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>1</sub> , x<sub>2</sub>, ・・・, x<sub>n</sub><span class="paren" style="font-size:em;">)</span></div>
に対して、絶対値の <span class="math">n</span> 乗和、
<div class="math">
||x||<sub>n</sub>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ 1</td></tr></table><span class="normal">|</span>x<sub>i</sub><span class="normal">|</span><sup>n</sup></div>
あるいは、絶対値の <span class="math">n</span> 乗和の <span class="math">n</span> 乗根を考えます。
<div class="math">
|x|<sub>n</sub>
＝
<span class="paren" style="font-size:2em;">(</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ 1</td></tr></table><span class="normal">|</span>x<sub>i</sub><span class="normal">|</span><sup>n</sup><span class="paren" style="font-size:2em;">)</span><sup>1/n</sup></div>
<span class="math">|x|<sub>n</sub></span> を
<span class="math">l<sup>n</sup></span> ノルム（norm：基準、規範）と呼びます。

<span class="math">l<sup>n</sup></span> ノルムにおいて、
<span class="math">n → ∞</span> で極限を取ると、
<div class="math">
|x|<sub>∞</sub>
＝
<span class="normal">max</span><span class="normal">|</span>x<sub>i</sub><span class="normal">|</span></div>
になりますが、
これを
<span class="math">l<sup>∞</sup></span> ノルムと呼びます。
 
実関数に対しても、
和を積分に変え、
<div class="math">
||f||<sub>n</sub>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table><span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>n</sup></div>
によって定義されるものを、
<span class="math">L<sup>n</sup></span> ノルムと呼びます。
 
これらのノルムを用いて、
2つの元 <span class="math">x, y</span> から実数への写像 <span class="math">d</span> を
<div class="math">
d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
＝
|x － y|<sub>n</sub></div>
と定義すると、
これは計量の条件を満たします。
（<span class="math">l<sup>n</sup></span> ノルムは n 乗根を取ったものでも、取らないものでもどちらでも計量の条件を満たします。）
 
実ベクトル空間に対して、
<span class="math">l<sup>2</sup></span> ノルムを使った計量はユークリッド距離、
<span class="math">l<sup>1</sup></span> ノルムを使った計量はマンハッタン距離と一致します。


## <a id="sec-generated-title-5"></a> <a id="form"></a>双線形形式と2次形式

実ベクトル空間には内積やノルムの概念があります。
また簡単化のために2次元ベクトルで説明しますが、
実ベクトルの内積、ノルム（正確には <span class="math">L<sup>2</sup></span> ノルム）は以下のようなものです。
<div class="math">
      <span class="vector">x</span> ＝ <span class="paren" style="font-size:em;">(</span>x<sub>1</sub> , x<sub>2</sub><span class="paren" style="font-size:em;">)</span>,
<span class="vector">y</span> ＝ <span class="paren" style="font-size:em;">(</span>y<sub>1</sub> , y<sub>2</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
      <span class="vector">x</span>・<span class="vector">y</span>
＝
<span class="paren" style="font-size:em;">(</span>x<sub>1</sub> y<sub>1</sub><span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>x<sub>2</sub> , y<sub>2</sub><span class="paren" style="font-size:em;">)</span>
　<span class="normal">・・・（内積）</span></div><div class="math">
||<span class="vector">x</span>||
＝
<span class="vector">x</span>・<span class="vector">x</span>
＝
x<sub>1</sub><sup>2</sup>
＋
x<sub>2</sub><sup>2</sup>
　<span class="normal">・・・（ノルム）</span></div>
これらを一般化しましょう。
まず、内積の一般化ですが、
実数上の任意のベクトル空間 <span class="math">V</span> に対して、
<span class="math">V × V</span> から実数 <span class="math"><span class="bold">R</span></span> への写像 <span class="math">B</span> で、
<span class="math">u, v ∈ V</span>、<span class="math">a ∈ <span class="bold">R</span></span>
1. <span class="math">B<span class="paren" style="font-size:em;">(</span>u, v<span class="paren" style="font-size:em;">)</span> ＝ B<span class="paren" style="font-size:em;">(</span>v, u<span class="paren" style="font-size:em;">)</span></span>

2. <span class="math">B<span class="paren" style="font-size:em;">(</span>u＋v, w<span class="paren" style="font-size:em;">)</span> ＝ B<span class="paren" style="font-size:em;">(</span>u, w<span class="paren" style="font-size:em;">)</span> ＋ B<span class="paren" style="font-size:em;">(</span>v, w<span class="paren" style="font-size:em;">)</span></span>

3. <span class="math">B<span class="paren" style="font-size:em;">(</span>au, v<span class="paren" style="font-size:em;">)</span> ＝ aB<span class="paren" style="font-size:em;">(</span>u, v<span class="paren" style="font-size:em;">)</span></span>


を満たすものを
<span class="math">V</span> 上の
対称双線形形式（symmetric bilinear form）あるいは双1次形式と呼びます。
（1を対称性、2, 3 を合わせて双線形性と呼びます。）
実ベクトル空間の内積は対称双線形形式になっています。
すなわち、対称双線形形式は内積を一般化した概念です。
 
次に、ノルムですが、
<span class="math">V</span> から <span class="math"><span class="bold">R</span></span> への写像 <span class="math">Q</span> で、

1. <span class="math">Q<span class="paren" style="font-size:em;">(</span>au<span class="paren" style="font-size:em;">)</span> ＝ a Q<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span></span>

2. <span class="math">Q<span class="paren" style="font-size:em;">(</span>u ＋ v<span class="paren" style="font-size:em;">)</span> － Q<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span> － Q<span class="paren" style="font-size:em;">(</span>v<span class="paren" style="font-size:em;">)</span></span>が<span class="math">V</span>上の双線形形式になる。


を満たすものを
<span class="math">V</span> 上の
2次形式（quadratic form）と呼びます。
実ベクトル空間のノルムは2次形式になっています。
 
対称双線形形式 <span class="math">B</span> に対して、
<span class="math">Q<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span> ＝ B<span class="paren" style="font-size:em;">(</span>u, u<span class="paren" style="font-size:em;">)</span></span> という写像を作ると、
2次形式になり、
このようにして得られる2次形式を <span class="math">B</span> の同伴（associated）2次形式と呼びます。
逆に、
2次形式 <span class="math">Q</span> に対して、
<span class="math">B<span class="paren" style="font-size:em;">(</span>u, v<span class="paren" style="font-size:em;">)</span> ＝ <span class="paren" style="font-size:1.5em;">(</span>Q<span class="paren" style="font-size:em;">(</span>u ＋ v<span class="paren" style="font-size:em;">)</span> － Q<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span> － Q<span class="paren" style="font-size:em;">(</span>v<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span> / 2</span> 
を <span class="math">Q</span> の同伴双線形形式と呼びます。
実ベクトルの内積とノルムは互いに同伴な関係にあります。
 
ところで、任意のベクトル <span class="math">u ∈ V</span> に対して、
<span class="math">Q<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span> ≦ 0</span> を満たすような2次形式を
半正定であるといいます。
（ちなみに、＜ 0 なら正定、＞ 0 なら負定、 ≧ 0 なら半負定。）
半正定な2次形式 <span class="math">Q</span> を導入できるベクトル空間は、
<div class="math">
d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
＝
Q<span class="paren" style="font-size:em;">(</span>x － y<span class="paren" style="font-size:em;">)</span><sup>1/2</sup></div>
によって計量を導入できます。
証明は省略しますが、
このようにして定義された <span class="math">d</span> は距離の条件を満たしています。
 
ちなみに、n 次元実ベクトル空間の2次形式は、
n 次対称正方行列 <span class="math">A</span> を用いて、
<div class="math">
Q<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
x<sup>T</sup> A x
</div>
（T は転置を表す記号。）
と表すことが出来ます。
この2次形式が半正定になるための条件は、
行列 <span class="math">A</span> の全ての固有値が非負になることです。


## <a id="sec-generated-title-6"></a> <a id="valuation"></a>付値

もう1つ、体の絶対値の概念を一般化することを考えましょう。
このような概念を<strong id="valuation" class="keyword">付値</strong>（valuation）と呼びます。
 
絶対値というものに求められる条件ということで以下のようなものを考えます。
「[体](../group/field.md#field)」<span class="math">K</span> から実数 <span class="math"><span class="bold">R</span></span> への写像 <span class="math">v</span>で、
任意の元 <span class="math">x, y ∈ K</span> に対して、

1. <span class="math">v<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ≧ 0</span>

2. <span class="math">v ＝ 0 ⇔ v<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ 0</span>

3. <span class="math">v<span class="paren" style="font-size:em;">(</span>x y<span class="paren" style="font-size:em;">)</span> ＝ v<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> v<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span></span>

4. <span class="math">v<span class="paren" style="font-size:em;">(</span>x ＋ y<span class="paren" style="font-size:em;">)</span> ≦ v<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＋ v<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span></span>


を満たすものを <span class="math">K</span> の付値と呼びます。
実数や複素数の絶対値はこの条件を満たしています。
 
体 <span class="math">K</span> に付値 <span class="math">v</span> が定義されるとき、
<span class="math">v</span> を用いて
<div class="math">
d<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
＝
v<span class="paren" style="font-size:em;">(</span>x － y<span class="paren" style="font-size:em;">)</span></div>
によって計量を導入出来ます。


## <a id="sec-generated-title-7"></a> <a id="continuous"></a>連続写像

距離空間の概念によって、
写像の連続性というものを議論することができます。
写像の連続性は、「[ε－δ論法](../infinity/epsilon.md#epsilon-delta)」を用いて定義します。
 
距離空間
<span class="math"><span class="paren" style="font-size:em;">(</span>A, d<sub>A</sub><span class="paren" style="font-size:em;">)</span></span>
から
<span class="math"><span class="paren" style="font-size:em;">(</span>B, d<sub>B</sub><span class="paren" style="font-size:em;">)</span></span>
への写像 <span class="math">f</span> が、
点 <span class="math">x ∈ A</span> において、
<div class="math">
∀ε＞0, ∃δ＞0, 
d<sub>A</sub><span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> ＜ δ
→
d<sub>B</sub><span class="paren" style="font-size:em;">(</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>, f<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span> ＜ ε
</div>
を満たすとき、
<span class="math">f</span> は <span class="math">x</span> において<strong id="d50e960" class="keyword">連続</strong>（continuous）であるといいます。
<span class="math">δ</span> は <span class="math">ε</span> および <span class="math">x</span> の2つによって定まる従属変数だと考えてください。
 
より強い条件として、
この命題が <span class="math">x, y</span> と無関係に成り立つような
<span class="math">δ</span> が存在するとき、
すなわち、
<span class="math">δ</span> が <span class="math">x</span> には依存せず、
<span class="math">ε</span> のみの従属変数に出来るとき、
<span class="math">f</span> は一様連続（uniform continuous）であるといいます。


## <a id="sec-generated-title-8"></a> <a id="plan"></a>執筆予定

```text
ε近傍
ε近傍を使った連続性の定義

・数列の極限
p 進付値
p 進付値による極限
p 進体
```
