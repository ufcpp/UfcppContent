---
title: "数列と漸化式"
source_url: "https://ufcpp.net/study/math/hs/sequence/"
content_type: "Article"
published_at: "2015-05-06T14:16:23"
updated_at: "2015-05-06T14:16:23"
tags: []
umbraco_id: 1456
parent_id: 1445
sort_order: 10
aliases:
  - "/study/hs/sequence.html"
---

# 数列と漸化式

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

高校でならう数列と漸化式は、基本的にパターンを覚えるだけ。
高校のとき先生が、漸化式のパターンとその解き方一覧みたなプリントを1枚くれたんですけど、
それがすごくよかったです。


## <a id="sec-generated-title-2"></a> <a id="convergence"></a>収束値

まあ、パターン丸暗記以外に、いくつか覚えるこつはあって、
そのうちの1つは収束値という考え方。
例えば、以下のような漸化式を考えてみます。
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table> a<sub>n</sub><span class="normal">+</span><span class="normal">3</span><span class="normal">=</span><span class="normal">0</span></div>
解き方のパターンとしては、
<span class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
a<sub>n</sub><span class="normal">=</span>
x
</span>
と置いて、<span class="math">x</span> の値を求めろって言われます。
まあ、意味はよく分からないけども、それで解けることが経験的に知られているんで。
でも、
<span class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
a<sub>n</sub><span class="normal">=</span>
x
</span>
と置くことに意味がないこともない。
 
もし、この漸化式によって定まる数列 <span class="math">a<sub>n</sub></span> が、
<span class="math">n <span class="normal">→</span><span class="normal">∞</span></span> のときにある値に収束するとすると、
<span class="math">a<sub>n</sub></span> だけでなくて、
<span class="math">a<sub>n <span class="normal">+</span><span class="normal">1</span></sub></span> も同じ値に収束するわけです。
この値を <span class="math">x</span> とでも置くと、
元の漸化式から、
<div class="math">
x <span class="normal">+</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table> x <span class="normal">+</span><span class="normal">3</span><span class="normal">=</span><span class="normal">0</span></div>
ですから、これを解いて、<span class="math">x <span class="normal">=</span><span class="normal">−</span><span class="normal">2</span></span> になります。
<span class="math">a<sub>n</sub></span> はこの値に収束するわけですから、
<span class="math">a<sub>n</sub><span class="normal">+</span><span class="normal">2</span></span> は、0 に収束します。
 
0 に収束する数列と言うと、ぱっと思いつくのは等比数列で、
実際、この場合、<span class="math">a<sub>n</sub><span class="normal">+</span><span class="normal">2</span></span> は等比数列になっています。
<div class="math">
      <span class="paren" style="font-size:em;">(</span>a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><span class="normal">2</span><span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="normal">−</span>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        <span class="normal">2</span>
      </td></tr></table>
      <span class="paren" style="font-size:em;">(</span>a<sub>n</sub><span class="normal">+</span><span class="normal">2</span><span class="paren" style="font-size:em;">)</span>
    </div>
<span class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
a<sub>n</sub><span class="normal">=</span>
x
</span>
と置くのは、要するに、収束値を先に求めちゃってるわけです。
ただまあ、同様の解き方が、収束しない数列の場合にも使えちゃうということですね。
例えば、
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><span class="normal">3</span> a<sub>n</sub><span class="normal">+</span><span class="normal">4</span><span class="normal">=</span><span class="normal">0</span></div>
は、同じく（収束を仮定した場合の）収束値は <span class="math">x <span class="normal">=</span><span class="normal">−</span><span class="normal">1</span></span> になるわけで、
<span class="math">a<sub>n</sub><span class="normal">+</span><span class="normal">1</span></span> が等比数列になるんですが、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="normal">−</span>
      <span class="normal">3</span>
      <span class="paren" style="font-size:em;">(</span>a<sub>n</sub><span class="normal">+</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span>
    </div>
となって、これは収束はしない数列になります。
（この場合は、本当は収束値ではなくて、振動の中心点が <span class="math">x <span class="normal">=</span><span class="normal">−</span><span class="normal">1</span></span> になる。）
でもまあ、漸化式を解くことはできます。


## <a id="sec-generated-title-3"></a> <a id="homogeneous"></a>斉次式

前項で、数列が収束しない場合でも、収束値に相当する値を先に求めてしまうことで漸化式が解けると説明しました。
収束しないんだから収束値を先に求めているという考え方はできませんが、
この場合は斉次式という物を使って説明できます。
（斉次式のよみは「せいじしき」。次数がそろってるという意味。同次式という言い方も。）
先ほど例に挙げた漸化式を再び考えて見ます。
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><span class="normal">3</span> a<sub>n</sub><span class="normal">+</span><span class="normal">4</span><span class="normal">=</span><span class="normal">0</span></div>
このうち、定数項を除いた部分の
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><span class="normal">3</span> a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
を元の漸化式の斉次式（あるいは斉次項）といいます。
添え字の部分の違いを除けば、全部の項に <span class="math">a</span> がそろって出てきているのでこう呼びます。
 
ここで、<span class="math">a<sub>n</sub></span> を、斉次式の解 <span class="math">a'<sub>n</sub></span> とその他の部分 <span class="math">x</span> に分けて考えてみましょう。
<span class="math">a<sub>n</sub><span class="normal">=</span> a'<sub>n</sub><span class="normal">+</span> x</span> と置いて、漸化式に代入すると、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>a'<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span><span class="normal">3</span> a'<sub>n</sub><span class="paren" style="font-size:em;">)</span>
      <span class="normal">+</span>
      <span class="paren" style="font-size:em;">(</span>x <span class="normal">+</span><span class="normal">3</span> x <span class="normal">+</span><span class="normal">4</span><span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="normal">0</span>
    </div>
と、斉次式と、
<span class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
a<sub>n</sub><span class="normal">=</span>
x
</span>
と置いて得られた式に分解されます。
 
要するに、この手の非斉次項が定数の漸化式は、

1. 斉次式の解を求める。

2. <span class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
a<sub>n</sub><span class="normal">=</span>
x
</span>と置いたときの<span class="math">x</span>の値を求める。


という2段階で解けることになります。


## <a id="sec-generated-title-4"></a> <a id="characteristic"></a>特性方程式

ちなみに、定数係数の斉次な漸化式（元から斉次項しかない漸化式）の解は、
漸化式の階数によらず冪関数になることが知られています。
ここでは、2階の漸化式（高校風に言うと、3項間漸化式）を例にとって説明してみましょう。
2階の斉次漸化式、
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">2</span></sub><span class="normal">+</span>
p a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span>
q a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
を考えてみましょう。
（ただし、<span class="math">p, q</span> は定数。）
最初に言ったように、これの解は冪関数 <span class="math">a<sub>n</sub><span class="normal">=</span> A x<sup>n</sup></span>
（<span class="math">x, A</span> は <span class="math">n</span> によらない定数。）
というような形の数列になることが知られています。
これを元の漸化式に代入してみると、
<div class="math">
A
<span class="paren" style="font-size:em;">(</span>
x<sup>n <span class="normal">+</span><span class="normal">2</span></sup><span class="normal">+</span>
p x<sup>n <span class="normal">+</span><span class="normal">1</span></sup><span class="normal">+</span>
q x<sup>n</sup><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span></div>
となりますが、これを両辺 <span class="math">A x<sup>n</sup></span> で割ると、
<div class="math">
x<sup><span class="normal">2</span></sup><span class="normal">+</span>
p x
<span class="normal">+</span>
q
<span class="normal">=</span><span class="normal">0</span></div>
となります。
これは、元の漸化式の
<span class="math">a<sub>n <span class="normal">+</span> k</sub></span> の部分を <span class="math">x<sup>k</sup></span> に置き換えた物になります。
この式を、漸化式の特性方程式と呼びます。
見ての通り、定数係数の斉次漸化式を解く問題は、
代数方程式を解く問題に帰着されます。
 
2階漸化式の場合、特性方程式は2次式になりますので、当然、答えも2つ出てきます。
したがって、正確には、漸化式の解は、特性方程式の2つの解をそれぞれ <span class="math">α, β</span> として、
<span class="math">A α<sup>n</sup><span class="normal">+</span> B β<sup>n</sup></span>
（<span class="math">A, B</span> は初期値によって決まる定数）
という形になります。
（ちなみに、重解の場合は
<span class="math">A α<sup>n</sup><span class="normal">+</span> B n α<sup>n</sup></span>
）
 
ちなみに、漸化式から特性方程式を得る操作は、
「Z 変換」という物を使って説明することもできます。
参考： 「[Z変換](../../sp/dsp/z.md)」。


## <a id="sec-generated-title-5"></a> <a id="imaginary"></a>虚数解の場合

漸化式の特性方程式が虚数解を持つ場合、
その一般解となる数列は周期性を持ちます。
例えば、以下のような漸化式を考えてみましょう。
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">2</span></sub><span class="normal">+</span>
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span>
a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
特性方程式を解いてみる前に、
ちょっと別の解法をみてみましょう。
一工夫必要なんですが、
添え字 <span class="math">n</span> を1つずらした物を用意して、
元の漸化式との差を取ります。
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">3</span></sub><span class="normal">+</span>
a<sub>n <span class="normal">+</span><span class="normal">2</span></sub><span class="normal">+</span>
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">−</span><span class="paren" style="font-size:em;">(</span>
a<sub>n <span class="normal">+</span><span class="normal">2</span></sub><span class="normal">+</span>
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span>
a<sub>n</sub><span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
a<sub>n <span class="normal">+</span><span class="normal">3</span></sub><span class="normal">−</span>
a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
はい、見ての通り、
<span class="math">
a<sub>n <span class="normal">+</span><span class="normal">3</span></sub><span class="normal">=</span>
a<sub>n</sub></span>
となりますんで、
3項に1回、同じ値が出てくる周期を持った数列になります。
もちろん、最初の2項が実数ならば、全部の項が実数です。
 
それでは、これを特性方程式の考え方を使って解いてみましょう。
この漸化式の特性方程式は
<span class="math">x<sup><span class="normal">2</span></sup><span class="normal">+</span> x <span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">0</span></span>
で、解は1の3乗根になります。
なので、1の3乗根（の1以外の2つ）を <span class="math">ω, <span class="bar">ω</span></span> （文字の上のラインは共役複素数を表すものとする）と置くと、
一般解は、
<div class="math">
a<sub>n</sub><span class="normal">=</span>
A ω<sup>n</sup><span class="normal">+</span>
B <span class="bar">ω</span><sup>n</sup></div>
定数 <span class="math">A, B</span> は最初の2項の値から求めます。
<div class="math">
A ω
<span class="normal">+</span>
B <span class="bar">ω</span><span class="normal">=</span>
a<sub><span class="normal">1</span></sub></div><div class="math">
A <span class="bar">ω</span><span class="normal">+</span>
B ω
<span class="normal">=</span>
a<sub><span class="normal">2</span></sub></div>
これらの式から、
<span class="math">a<sub><span class="normal">1</span></sub>, a<sub><span class="normal">2</span></sub></span>
が実数ならば、
<span class="math">B <span class="normal">=</span><span class="bar">A</span></span> という関係が成り立つことが分かります。
したがって、
<div class="math">
a<sub>n</sub><span class="normal">=</span>
A ω<sup>n</sup><span class="normal">+</span><span class="bar">A</span><span class="bar">ω</span><sup>n</sup><span class="normal">=</span>
A ω<sup>n</sup><span class="normal">+</span><span class="bar">A ω<sup>n</sup></span><span class="normal">=</span><span class="normal">2</span><span class="script">Re</span><span class="paren" style="font-size:em;">(</span>
A ω<sup>n</sup><span class="paren" style="font-size:em;">)</span></div>
となって、ちゃんと全ての項が実数になります。
しかも、<span class="math">ω</span> は1の3乗根なので、
<span class="math">ω<sup>n</sup></span> は3回に1回同じ値になるわけで、
上述の、一工夫して解いた結果とは矛盾しません。
（というよりも、上の工夫は、
「特性方程式の解が1の3乗根になるんだから、周期3の数列になるはず」
→
「じゃあ、
<span class="math">
a<sub>n <span class="normal">+</span><span class="normal">3</span></sub><span class="normal">=</span>
a<sub>n</sub></span>
になるはずだから、そうなるように式を変形しよう」
という発想から来ます。）


## <a id="sec-generated-title-6"></a> <a id="matrix"></a>行列

2つの数列が同時に出てくるような漸化式だって考えられます。
例えば、以下のようなもの。
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
p a<sub>n</sub><span class="normal">+</span>
q b<sub>n</sub></div><div class="math">
b<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
r a<sub>n</sub><span class="normal">+</span>
s b<sub>n</sub></div>
数Cで習う「行列」を使うと、以下のようにも書けます。
<div class="math">
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>n <span class="normal">+</span><span class="normal">1</span></sub></td></tr><tr><td>b<sub>n <span class="normal">+</span><span class="normal">1</span></sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>p</td><td>q</td></tr><tr><td>r</td><td>s</td></tr></table><span class="paren" style="font-size:3em;">]</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>n</sub></td></tr><tr><td>b<sub>n</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
    </div>
これは、
<div class="math">
      <span class="vector">a</span>
      <sub>n</sub>
      <span class="normal">=</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>n</sub></td></tr><tr><td>b<sub>n</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
    </div><div class="math">
A
<span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>p</td><td>q</td></tr><tr><td>r</td><td>s</td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
（ベクトルを太字アルファベットで表します。）
というように、ベクトルの数列
<span class="math"><span class="vector">a</span><sub>n</sub></span> と、
行列の係数 <span class="math">A</span> を使って、
<div class="math">
      <span class="vector">a</span>
      <sub>n <span class="normal">+</span><span class="normal">1</span></sub>
      <span class="normal">=</span>
A <span class="vector">a</span><sub>n</sub></div>
とも書けるわけですが、
これの一般解は、
<div class="math">
      <span class="vector">a</span>
      <sub>n</sub>
      <span class="normal">=</span>
A<sup>n</sup><span class="vector">a</span><sub><span class="normal">1</span></sub></div>
と書けて、
結局、行列の n 乗問題に帰着されます。
 
数列の数が増えても同様です。
何個だろうと、定数係数で斉次な限り同じ論法で解けます。
（ただし、3×3 以上の正方行列の n 乗計算には、
大学1年レベルの知識が必要。
参考： 「[固有値](../linear/eigen.md)」。）
 
ちなみに、前節の定数係数斉次漸化式
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">2</span></sub><span class="normal">+</span>
p a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span>
q a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
も、
<span class="math">
b<sub>n</sub><span class="normal">=</span>
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub></span>
と置くと、
<div class="math">
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>n <span class="normal">+</span><span class="normal">1</span></sub></td></tr><tr><td>b<sub>n <span class="normal">+</span><span class="normal">1</span></sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>
            <span class="normal">0</span>
          </td><td>
            <span class="normal">1</span>
          </td></tr><tr><td>
            <span class="normal">−</span>p</td><td>
            <span class="normal">−</span>q</td></tr></table><span class="paren" style="font-size:3em;">]</span>
      <span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a<sub>n</sub></td></tr><tr><td>b<sub>n</sub></td></tr></table><span class="paren" style="font-size:3em;">]</span>
    </div>
と変形できるので、
結局、行列の n 乗問題として解くこともできます。


## <a id="sec-generated-title-7"></a> <a id="linear"></a>定数係数線形漸化式

（書きかけ）

<span class="math">a<sub>n</sub></span> から
<span class="math">a<sub>n <span class="normal">+</span><span class="normal">1</span></sub></span> を作る操作を
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span>
z
a<sub>n</sub></div>
と書きましょう。
<span class="math">z</span> は変数ではなく、数列に対して添え字を1つずらす演算子になります。
この記法を使うなら、
漸化式
<div class="math">
a<sub>n <span class="normal">+</span><span class="normal">2</span></sub><span class="normal">+</span>
p a<sub>n <span class="normal">+</span><span class="normal">1</span></sub><span class="normal">+</span>
q a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
は、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
z<sup><span class="normal">2</span></sup><span class="normal">+</span>
p z
<span class="normal">+</span>
q
<span class="paren" style="font-size:em;">)</span>
a<sub>n</sub><span class="normal">=</span><span class="normal">0</span></div>
となります。
このとき、括弧の中身は、
特性方程式に演算子 <span class="math">z</span> を代入したものになっています。
 
差分演算子 <span class="math">Δ <span class="normal">=</span> z <span class="normal">−</span><span class="normal">1</span></span>。
漸化式の斉次項の階数が1つ増えるけど、
非斉次項に <span class="math">n<sup>k</sup></span> があった場合、これの次数を1つ下げれる。


## <a id="sec-generated-title-8"></a> <a id="general"></a>一般的には

係数とか非斉次項が定数の場合には、これまでに説明したような特性方程式とかの考え方を使って、
確実に漸化式から数列の一般解を得ることができます。
でも、
係数が定数でなかったり、
非斉次項が複雑な形をしていたり、
<span class="math">a<sub>n</sub><sup><span class="normal">2</span></sup></span>
というように非線形（1次じゃない）項があるものになると、
どんな場合でも一般解を得られるような万能の方法はありません。
 
もちろん、高校の数学の問題として出てくるようなものは解けるものが出てくるわけですが、
それは幸運なケースです。


## <a id="sec-generated-title-9"></a> <a id="other"></a>見方を変えて

数列は、無限次元のベクトルと考えることも可能。
<span class="math">N</span> 次元ベクトルの要素を、
<span class="math">x<sub>i</sub></span>（<span class="math">i <span class="normal">=</span> 1 <span class="normal">∼</span> N</span>）
と書くのの、<span class="math">i</span> の範囲の制約を取り払った物。
 
でも、<span class="math">N</span> 階漸化式の解になると、
最初の <span class="math">N</span> 項までの値で数列全体が決まるので、
<span class="math">N</span> 次元ベクトル。
 
あと、自然数 → 実数の関数とみなすことも可能。
 
こういう風に、視点を変えたり、制約条件・前提条件が変わったりすると、
同じ物が別の見え方をするということは、数学にはよくあります。
その逆もまたしかりで、
見かけ上全く別の物が、見方一つで同じ物とみなせる場合も多々あります。
