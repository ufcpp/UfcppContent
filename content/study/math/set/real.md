---
title: "実数"
source_url: "https://ufcpp.net/study/math/set/real/"
content_type: "Article"
published_at: "2015-05-06T14:17:06"
updated_at: "2015-05-06T14:17:06"
tags: []
umbraco_id: 1478
parent_id: 1471
sort_order: 6
aliases:
  - "/math/set/real/"
  - "/set/real"
  - "/set/real.html"
  - "/study/set/real"
  - "/study/set/real.html"
---

# 実数

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

実数は、有理数をユークリッド距離を用いて完備化した体になります。
 
完備（complete）というのは、
「[体](../group/field.md#field)」中の任意の数列の極限値がその体の中に入っているものの事をいいます（正確には距離完備）。
 
要するに、有理数列の極限は必ずしも有理数になりませんが、
その有理数列の極限によって得られる値全体の集合が実数になります。
 
このように、有理数列の極限値として実数を定義する方法は、
カントール（Georg Cantor：ドイツの数学者）によるもので、
ユークリッド距離による距離完備化という概念を用いたものです。
これに対して、
デーデキント（Julius Wilhelm Richard Dedekind：ドイツの数学者）による、
デーデキントの切断と呼ばれる実数の構成方法もあり、
こちらは大小関係による順序完備化という概念を用います。
ここでは前者の距離完備化（カントール流の定義）を用いて説明します。


## <a id="sec-generated-title-2"></a> <a id="sequence"></a>有理数列

有理数列は必ずしも有理数に収束しません。
例えば、<span class="math">a<sub>n</sub> ＝ <table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k!</td></tr></table></span> とすると、<span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table> a<sub>n</sub></span> は無理数になります。
なんでこういう話をするかというと、
大雑把な言い方をすれば、実数は有理数列の収束値として定義します。
 
もう少し正確な実数の定義を説明する前に、有理数列というものについて説明が必要になります。
まず、<em>整数から有理数への写像 <span class="math">S ＝ <span class="bold">Q</span><sup>ω</sup></span> の元 <span class="math">f</span> を有理数列と呼びます</em>。
また、<span class="math">n ∈ ω</span> に対して、<span class="math">f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span> ＝ a<sub>n</sub></span> となるような数列 <span class="math">f</span> を
<span class="math"><span class="paren" style="font-size:em;">{</span>a<sub>n</sub><span class="paren" style="font-size:em;">}</span><sub>n ∈ ω</sub></span> と表します。
<span class="math"><span class="paren" style="font-size:em;">{</span>a<sub>0</sub>, a<sub>1</sub>, a<sub>2</sub>, ・・・<span class="paren" style="font-size:em;">}</span></span> などと表すこともあります。


### <a id="sec-generated-title-3"></a> <a id="Cauchy-sequence"></a>コーシー列・零数列

収束する数列というものをきっちりと書き表す必要があります。
収束する数列というものの表すための条件として以下のようなものがあります。
 
任意の正の有理数 <span class="math">a</span> に対して、
ある自然数 <span class="math">N</span> があって、
<span class="math">n ≧ N</span> となる任意の自然数 <span class="math">n</span> に対して
<span class="math"><span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>N<span class="paren" style="font-size:em;">)</span> － f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span><span class="normal">|</span> ＜ a</span> となる。
 
この条件を満たす数列を（有理数の）<strong id="d47e130" class="keyword">コーシー列</strong>（Cauchy sequnce、コーシーは人名）と呼びます。
この条件のような表現の仕方は、いわゆる「[ε－δ論法](../infinity/epsilon.md#epsilon-delta)」というやつです。
 
例えば、<span class="math"><span class="paren" style="font-size:em;">{</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2<sup>i</sup></td></tr></table><span class="paren" style="font-size:em;">}</span><sub>n ∈ ω</sub></span> はコーシー列になります。
<div class="math">
        <span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>N<span class="paren" style="font-size:em;">)</span> － f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ N</td></tr></table> 2<sup>i</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2<sup>N</sup></td></tr></table> － <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2<sup>n</sup></td></tr></table></td></tr><tr><td>1 － <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2</td></tr></table></td></tr></table>
≦
<table class="frac" summary="fraction"><tr><td class="num"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2<sup>N</sup></td></tr></table></td></tr><tr><td>1 － <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2</td></tr></table></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2<sup>N</sup> － 2<sup>N － 1</sup></td></tr></table></div>
となるので、任意の有理数 <span class="math">a</span> に対して、<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2<sup>N</sup> － 2<sup>N － 1</sup></td></tr></table> ＜ a</span> となるような自然数 <span class="math">N</span> を取ることでコーシー列の条件を満たすことができます。
 
他の例としては、定数列、すなわち、有理数 <span class="math">a</span> を与えたとき
<span class="math"><span class="paren" style="font-size:em;">{</span>a<span class="paren" style="font-size:em;">}</span><sub>n ∈ ω</sub></span> となる数列（<span class="math">n</span> によらず常に <span class="math">a</span> という値をとる数列）もコーシー列です。
（<span class="math"><span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>N<span class="paren" style="font-size:em;">)</span> － f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span><span class="normal">|</span></span> は恒等的に 0。）
 
また、0 に収束する数列を<strong id="d47e257" class="keyword">零数列</strong>と呼びます。
厳密に書くならば、
任意の有理数 <span class="math">a</span> に対して、
ある自然数 <span class="math">N</span> があり、
<span class="math">n ≧ N</span> となるような自然数 <span class="math">n</span> に対して、
<span class="math"><span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span><span class="normal">|</span> ＜ a</span> となる数列が零数列です。


## <a id="sec-generated-title-4"></a> <a id="real"></a>実数の定義

最初に述べたように、概念的には有理数列の収束値として<strong id="real" class="keyword">実数</strong>（real number）を定義します。
といっても、実際にはもう少し回りくどい定義の仕方が必要で、簡単に言うと以下のようになります。

* 2つのコーシー列<span class="math">x ＝ <span class="paren" style="font-size:em;">{</span>x<sub>n</sub><span class="paren" style="font-size:em;">}</span>, y ＝ <span class="paren" style="font-size:em;">{</span>y<sub>n</sub><span class="paren" style="font-size:em;">}</span></span>に対して、<span class="math">x － y</span>が零数列のとき（<span class="math">x</span>と<span class="math">y</span>が同じ値に収束するとき）互いに同値であるものとする。

* この同値関係を使ってコーシー列の商集合<span class="math"><span class="bold">R</span></span>を作る。

* この<span class="math"><span class="bold">R</span></span>を実数と定義する。


要するに、実数はコーシー列全体の集合 <span class="math">C</span> の零数列全体の集合 <span class="math">N</span> による「[剰余体](../group/quotientfield.md#residual)」<span class="math">C/N</span> になります。
 
定数列 <span class="math"><span class="paren" style="font-size:em;">{</span>a<span class="paren" style="font-size:em;">}</span></span> を代表する実数の元は有理数と1対1に対応するので、
これを有理数 <span class="math">a</span> と同一視することができ、
<em>有理数は実数の部分集合である</em>とみなすことができます。


## <a id="sec-generated-title-5"></a> <a id="operation"></a>実数の間の関係・演算

### <a id="sec-generated-title-6"></a> <a id="lim"></a>極限

実数の順序の話をする前に、コーシー列の極限というものについて説明します。
 
ある有理数列 <span class="math">f</span> に対して、<span class="math">f</span> を代表する実数の元を <span class="math">f</span> の<strong id="d47e377" class="keyword">極限</strong>と呼び、<span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table>f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span></span> または単に <span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>f</span> で表します。
<span class="math">f, g ∈ C</span> に対し、
<div class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>f ＋ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>g ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table><span class="paren" style="font-size:em;">(</span>f ＋ g<span class="paren" style="font-size:em;">)</span></div><div class="math">
        <span class="paren" style="font-size:em;">(</span>
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>f<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">(</span>
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>g<span class="paren" style="font-size:em;">)</span> ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table><span class="paren" style="font-size:em;">(</span>fg<span class="paren" style="font-size:em;">)</span></div>
が成り立ちます。


### <a id="sec-generated-title-7"></a> <a id="order"></a>実数の順序

詳細は省きますが、任意のコーシー列 <span class="math">f</span> には、
<span class="math">n ≧ N</span> となるような自然数 <span class="math">n</span> に対して
<span class="math">f<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span></span> の符号が全て同じになるような自然数 <span class="math">N</span> が存在します。
このときの符号を実数 <span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>f</span> の符号として定義します。
そして、2つの実数 <span class="math">x, y</span> の順序関係は
<div class="math">
x － y <span class="normal">が正</span> ⇔ x ＞ y
</div><div class="math">
x － y <span class="normal">が負</span> ⇔ x ＜ y
</div>
で定義します。


### <a id="sec-generated-title-8"></a> <a id="sum"></a>実数の加減乗除

2つの実数 <span class="math">x ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>f, y ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>g</span> の和・差・積・商は
<div class="math">
x ± y ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table><span class="paren" style="font-size:em;">(</span>f ± g<span class="paren" style="font-size:em;">)</span></div><div class="math">
xy ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table><span class="paren" style="font-size:em;">(</span>fg<span class="paren" style="font-size:em;">)</span></div><div class="math">
x / y ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table><span class="paren" style="font-size:em;">(</span>f / g<span class="paren" style="font-size:em;">)</span></div>
で定義します。


## <a id="sec-generated-title-9"></a> <a id="algebra"></a>代数系としての実数

定義から明らかなように、実数は体になります。
体であることを明示的に表すために、実数を<em>実数体</em>と呼ぶこともあります。
 
有理数も体になりますが、実数は有理数体を部分体として含む体ということになります。
体からより大きな体を作ることを<em>体の拡大</em>と呼びます。
特に、有理数→実数のように極限を用いて体の拡大を行う方法を「[完備拡大](../group/extensionfield.md#completed)」と呼びます。


## <a id="sec-generated-title-10"></a> <a id="plan"></a>執筆予定

```text
・有理数と実数の関係
有理数 ⊂ 実数
（有理数は実数の真部分集合）

<span class="bold">R</span>－<span class="bold">Q</span> の元を無理数（irrational number）と呼ぶ。


・連続性
写像 F<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>: <span class="bold">R</span> → <span class="bold">R</span> の連続性

<span class="math">F<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ＝ b</span> のとき、
<span class="math">∀ε＞0 ∃δ, <span class="normal">|</span>x － a<span class="normal">|</span> ＜ δ ⇒ <span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － b<span class="normal">|</span> ＜ε</span>


・完備性
実数のコーシー列は実数に収束する。
（⇔ 有理数のコーシー列は必ずしも有理数の範囲には収束しない）

「完備（completion）である」とはある集合のコーシー列の極限が、
その集合の範囲内に存在することを指す。
（有理数は完備ではなく、実数は完備）

ノルム空間とか完備化とかの話もどこかに書きたい。


余談:
極限の定義の仕方によって色々な完備拡大が出来る。
（コーシー列の定義のところで、<span class="normal">|</span>***<span class="normal">|</span> ＜ a というように絶対値を使っているが、
この絶対値の部分を別の定義に置き換える。）

実際、有理数体の拡大体には、実数体の他に p-進体 というものがある。
（絶対値（いわゆるユークリッドノルム）の代わりにp進ノルムというものを使う。）

詳しくは群・環・体で説明。
```
