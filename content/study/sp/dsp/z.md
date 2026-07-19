---
title: "Z変換"
source_url: "https://ufcpp.net/study/sp/dsp/z/"
content_type: "Article"
published_at: "2015-05-06T14:22:08"
updated_at: "2015-05-06T14:22:08"
tags: []
umbraco_id: 1606
parent_id: 1599
sort_order: 6
aliases:
  - "/dsp/z"
  - "/dsp/z.html"
  - "/sp/dsp/z/"
  - "/study/dsp/z"
  - "/study/dsp/z.html"
---

# Z変換

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[フーリエ変換](fourier.md#f-trans)」における微分を表す変数 <span class="math">iω</span> を <span class="math">s</span> と置いて、微分方程式の解析を行うのが「[ラプラス変換](laplace.md#Laplace)」です。
これに対して、
離散関数のフーリエ変換（「[離散関数のフーリエ変換 離散フーリエ変換](dft.md#discrete)」参照）における時間シフトを表す変数 <span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> を <span class="math">z</span> と置いて、差分方程式の解析を行うのが Z 変換です。
 
アナログシステム（連続信号）は微分・積分を用いて表現するのでラプラス変換を用いてシステム解析を行います。
一方、ディジタルシステム（離散信号）は差分・和分を用いて表現するので Z 変換の出番となります。


##<a id="sec-generated-title-2"></a> <a id="definition"></a>Z 変換の定義
離散関数 <span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></span> に対して、
<div class="math">
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝－∞</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
z<sup>－k</sup></div>
で表される変換を<strong id="z-trans" class="keyword">Z 変換</strong>（Z transform）といいます。
 
この式は、離散関数のフーリエ変換の式中の <span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> （<span class="math">T<sub>s</sub></span> は定数（サンプリング周期）、<span class="math">ω</span> は実数変数）の部分に <span class="math">z</span> （<span class="math">z</span> は複素数）を代入したものになっています。
フーリエ変換では、サンプリング周期分の時間シフトは <span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> に変換されます。
すなわち、Z 変換の変数 <span class="math">z</span> は1サンプル分の時間シフトに相当するものです。

「[ラプラス変換](laplace.md#Laplace)」の場合と同様に、因果的な関数のみを対象とし、
級数和の範囲を <span class="math">k ≧ 0</span> に制限する場合もあります。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
z<sup>－k</sup></div>
ラプラス変換のときと同様に、
－∞から始める方を両側 Z 変換、
0 から始めるほうを片側 Z 変換といいます。
 
簡単化のために、以下のように、Z 変換を記号 <span class="math"><span class="cursive">Z</span></span> （筆記体の Z）で表します。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> ＝
<span class="script">Z</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></div>
これもまた、ラプラス変換のときと同様に、
Z 変換後の関数 <span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> を<strong id="d24e162" class="keyword">伝達関数</strong>と呼びます。
ラプラス変換の <span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> も伝達関数と呼ぶため、
この2つを同時使う場合には、
<span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> をアナログ伝達関数、
<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> をディジタル伝達関数などといって区別する場合もあります。
（ディジタル信号処理における用語。）


##<a id="sec-generated-title-3"></a> <a id="property"></a>Z 変換の性質
フーリエ変換の性質から簡単に導き出すことができるので細かい説明は省略しますが、
Z 変換は以下のような性質を持っています。


###<a id="sec-generated-title-4"></a> <a id="linear"></a>線形性
<div class="math">
        <span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
a f<span class="paren" style="font-size:em;">[</span>t<span class="paren" style="font-size:em;">]</span>
＋
b g<span class="paren" style="font-size:em;">[</span>t<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
a
<span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>t<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＋
b
<span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">[</span>t<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>

###<a id="sec-generated-title-5"></a> <a id="timeshift"></a>時間シフト
両側 Z 変換では、時間シフトは <span class="math">z</span> の多項式倍に変換されます。
<div class="math">
        <span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>t ± n<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
z<sup>±n</sup><span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>t<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></div>
<span class="math">f<span class="paren" style="font-size:em;">[</span>t ＋ n<span class="paren" style="font-size:em;">]</span></span>（変換後には <span class="math">z<sup>n</sup></span>）を時間進み、
<span class="math">f<span class="paren" style="font-size:em;">[</span>t － n<span class="paren" style="font-size:em;">]</span></span>（変換後には <span class="math">z<sup>－n</sup></span>）を時間遅れもしくは遅延（delay）と呼びます。
時間進みが生じるようなシステムは実時間処理で実現することはできません。
 
ちなみに、片側 Z 変換においては、
片側ラプラス変換のときと同じような感じで定数項が残ります。
<div class="math">
        <span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k ± n<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
z<sup>±n</sup><span class="script">Z</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">n － 1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝0</td></tr></table>
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
z<sup>n－k</sup></div>

##<a id="sec-generated-title-6"></a> <a id="difference"></a>差分方程式
連続システムは微分方程式を用いて表しますが、
それに対して、離散システムは差分方程式というものを用いて表します。
差分方程式というのは、例えば、
<div class="math">
      <table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝0</td></tr></table>
a<sub>n</sub> x<span class="paren" style="font-size:em;">[</span>k － n<span class="paren" style="font-size:em;">]</span>
＝ y<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span></div>
（<span class="math">a<sub>n</sub></span> は定数）
というように表されます。
（このような形式で表されるものは、
差分方程式の中では単純な部類に属するもので、線形時不変差分方程式と呼ばれます。
まあ、分かりやすくいうと、高校の数学で漸化式として習った奴です。
）
 
ここで、この式を Z 変換してみると以下のようになります。
<div class="math">
      <span class="paren" style="font-size:3em;">(</span>
        <table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝0</td></tr></table>
a<sub>n</sub> z<sup>－n</sup><span class="paren" style="font-size:3em;">)</span>
X<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝ Y<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></div>
この式中の () の中身、なんとなく Z 変換の定義式と同じっぽく見えませんか？
実際、以下のような離散関数
<div class="math">
h<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">a<sub>k</sub></span>  </td><td><span class="paren">(</span><span class="math">0 ≦ k ＜ N</span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">その他</span></span><span class="paren">)</span></td></tr></table></div>
を用意し、<span class="math">H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> ＝ <span class="script">Z</span><span class="paren" style="font-size:1.5em;">[</span>h<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:1.5em;">]</span><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> と置くと、この式は
<div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
X<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
Y<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></div>
となります。
 
ここまで説明すれば大体もう検討が付くかと思いますが、
Z 変換は差分方程式を解く（離散システムを解析する）ために用いられます。
 
ちなみに、差分方程式を簡潔に書き表すための便法として、
遅延演算子というものがあります。
遅延演算子 <span class="math">D</span> は以下のように定義されます。
<div class="math">
D x<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＝ x<span class="paren" style="font-size:em;">[</span>k － 1<span class="paren" style="font-size:em;">]</span></div>
この記法を用いると、先ほどの差分方程式は以下のように表すことができます。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>D<span class="paren" style="font-size:em;">)</span>
x<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝ y<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
,　　
H<span class="paren" style="font-size:em;">(</span>D<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N－1</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝0</td></tr></table>
a<sub>n</sub> D<sup>n</sup></div>
特に説明の必要もないと思いますが、
遅延演算子の Z 変換は <span class="math">z<sup>－1</sup></span> になります。
したがって、<span class="math">H<span class="paren" style="font-size:em;">(</span>D<span class="paren" style="font-size:em;">)</span></span> の Z 変換は <span class="math">H<span class="paren" style="font-size:em;">(</span>z<sup>－1</sup><span class="paren" style="font-size:em;">)</span></span> となります。


##<a id="sec-generated-title-7"></a> <a id="inverse"></a>逆変換
###<a id="sec-generated-title-8"></a> <a id="d24e489"></a>逆変換の式
逆変換の式もラプラス変換と同じように、
逆フーリエ変換の式に <span class="math">z ＝ <span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> という関係式を代入し、
積分区間を変えたような形になります。
具体的には以下のような式になります。
<div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2πi</td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"><span class="normal">|</span>z<span class="normal">|</span> ＝ 1</td></tr></table>
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> z<sup>k － 1</sup><span class="normal">d</span>z
</div>
なんか∮とかいう記号が出てきて、
複素関数解析の分野に慣れていない人にはちょっとわけの分からないことになっていますが、
まあ、分からなくても問題ありません。
ラプラス変換のときと一緒で、この式を直接使うことはあまりありませんので。


###<a id="sec-generated-title-9"></a> <a id="d24e528"></a>変換公式を頼りに逆変換
Z 変換も、部分分数分解と公式だけ使ってたいていのものを逆変換できます。
どうやるかを説明する前に、以下の公式を見てください。
<div class="math">
        <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
r<sup>k</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 － r</td></tr></table></div>
いわゆる、等比無限級数の公式ですね。
では次に、この式に <span class="math">r ＝ a z<sup>－1</sup></span> を代入してみましょう。
<div class="math">
        <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k ＝ 0</td></tr></table>
a<sup>k</sup> z<sup>－k</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 － a z<sup>－1</sup></td></tr></table></div>
この式を見ていれば分かるかと思いますが、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 － a z<sup>－1</sup></td></tr></table></span> の逆 Z 変換は、
<div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＝ 
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">a<sup>k</sup></span>  </td><td><span class="paren">(</span><span class="math">0 ≦ k ＜ N</span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">その他</span></span><span class="paren">)</span></td></tr></table></div>
になります。
 
部分分数分解を使えば <span class="math">z<sup>－1</sup></span> の有理式をこの形に分解できるので、
有理式の逆 Z 変換が可能になります。


###<a id="sec-generated-title-10"></a> <a id="d24e622"></a>級数展開
有理式以外の場合ですが、
ラプラス変換と比べればずいぶんと簡単で、
要は <span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> を
<div class="math">
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝－∞</td></tr></table>
a<sub>n</sub> z<sup>－n</sup></div>
という形に級数展開できれば、
係数 <span class="math">a<sub>n</sub></span> がそのまま
<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> の逆 Z 変換
<span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＝ a<sub>n</sub></span> になります。
 
級数展開というと、テイラー展開や「[ローラン展開](../../math/analysis/residue.md#laurent)」ですね。
まず、<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> が因果的な場合には、
<span class="math">y<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ F<span class="paren" style="font-size:em;">(</span>x<sup>－1</sup><span class="paren" style="font-size:em;">)</span></span> がテイラー展開可能で、
<div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k!</td></tr></table>
y<sup><span class="paren" style="font-size:em;">(</span>k<span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></div>
となります。
（<span class="math">k ＜ 0</span> のときは <span class="math">f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span> ＝ 0</span>。）

<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> が非因果的な場合には、
<span class="math">y<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ F<span class="paren" style="font-size:em;">(</span>x<sup>－1</sup><span class="paren" style="font-size:em;">)</span></span> を「[ローラン展開](../../math/analysis/residue.md#laurent)」することで、
<div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2πi</td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"><span class="normal">|</span>x<span class="normal">|</span>＝1</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">y(x)</td></tr><tr><td>x<sup>k+1</sup></td></tr></table><span class="normal">d</span>x
</div>
となります。
この式も、実用上はあまり使いませんので、
複素解析やローラン展開が分からない人は無視してもらって構いません。
 
ちなみに、このローラン展開の式において、<span class="math">z ＝ x<sup>－1</sup></span> と置いて変数変換すると、
<div class="math">
f<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2πi</td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"><span class="normal">|</span>z<span class="normal">|</span> ＝ 1</td></tr></table>
F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> z<sup>k － 1</sup><span class="normal">d</span>z
</div>
となって、最初に示した公式と一致します。


##<a id="sec-generated-title-11"></a> <a id="stability"></a>安定性
フーリエ変換との間に、
ラプラス変換は <span class="math">s ＝ iω</span>、
Z 変換は <span class="math">z ＝ <span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> という関係を持っています。
この2つの関係式から <span class="math">ω</span> を消去すると、
<div class="math">
z ＝ <span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>s T<sub>s</sub><span class="paren" style="font-size:1em;">)</span></div>
という関係式が得られます。
ラプラス変換は、

* 伝達関数<span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>が複素平面の右半面（実部が正）の範囲に極を1つでも持つとき、解は不安定。

* 伝達関数<span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>の全ての極が左半面（実部が負）のとき、解は定数の収束する。

* 伝達関数<span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>の極が虚軸上（実部が 0）にあるとき、定常応答（sin, cos）が現れる。


という特徴を持っていました。
先ほどの関係式から、
<span class="math">s</span> の実部が正のとき <span class="math"><span class="normal">|</span>z<span class="normal">|</span> ＞ 1</span>、
<span class="math">s</span> の実部が負のとき <span class="math"><span class="normal">|</span>z<span class="normal">|</span> ＜ 1</span> となることが分かるかと思います。
したがって、Z 変換は、

* 伝達関数<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span>が単位円の外側（<span class="math"><span class="normal">|</span>z<span class="normal">|</span> ＞ 1</span>）の範囲に極を1つでも持つとき、解は不安定。

* 伝達関数<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span>の全ての極が単位円内（<span class="math"><span class="normal">|</span>z<span class="normal">|</span> ＜ 1</span>）のとき、解は定数の収束する。

* 伝達関数<span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span>の極が単位円上（<span class="math"><span class="normal">|</span>z<span class="normal">|</span> ＝ 1</span>）にあるとき、定常応答（sin, cos）が現れる。


という特徴を持つことになります。
 
一応、簡単な例を挙げて、
本当にこの条件で解の安定（解が発散）・不安定（解が収束または定常応答になる）が分かれるかどうかを確かめてみましょう。
「[逆変換](#inverse)」で説明したように、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 － a z<sup>－1</sup></td></tr></table></span> の逆 Z 変換は <span class="math">a<sup>k</sup></span> になります。<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 － a z<sup>－1</sup></td></tr></table></span> の極は <span class="math">z ＝ a</span> です。
また、<span class="math">a<sup>k</sup></span> は
<span class="math"><span class="normal">|</span>a<span class="normal">|</span> ＜ 1</span> のとき 0 に収束（安定）、
<span class="math"><span class="normal">|</span>a<span class="normal">|</span> ＝ 1</span> のとき振動（安定）、
<span class="math"><span class="normal">|</span>a<span class="normal">|</span> ＞ 1</span> のとき発散（不安定）します。
この例から、極の絶対値が 1 以上か 1 未満かで安定性が変わることが分かるかと思います。


##<a id="sec-generated-title-12"></a> <a id="spectrum"></a>周波数特性
最初に述べたように、
計算上、Z変換は「[フーリエ変換](fourier.md#f-trans)」の時間シフトを表す変数 <span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> を <span class="math">z</span> で置き換えたものです。
したがって、
「[ラプラス変換](laplace.md#Laplace)」のときと同様に、
Z変換の結果得られた伝達関数 <span class="math">F<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> の
<span class="math">z</span> の部分に
<span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span>
を代入したもの
<span class="math">F<span class="paren" style="font-size:em;">(</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span><span class="paren" style="font-size:em;">)</span></span>
はシステムの周波数特性になります。
 
ラプラス変換とZ変換の関係から、
「<span class="math">z</span> を <span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span> で置き換える」
という操作は、
ラプラス変換における
「<span class="math">s</span> を <span class="math">iω</span> で置き換える」
という操作と同じ意味を持ちます。
（<span class="math">z</span> の絶対値を 1 に固定することになり、
ラプラス変換で <span class="math">s</span> の実部を 0 にすることと同じ意味合いを持ちます。）
すなわち、「過渡解を無視する」ということになります。
 
したがって、ラプラス変換のときと同様に、以下のようなことが言えます。

* Z変換は離散システムの安定性や過渡解の解析に用いる。

* 安定性が保証され、かつ、過渡解が無視できる場合、<span class="math">z ＝ <span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>i T<sub>s</sub>ω<span class="paren" style="font-size:1em;">)</span></span>と置くことで伝達関数から周波数特性が得られる。



##<a id="sec-generated-title-13"></a> <a id="plan"></a>執筆予定
<pre>
z 平面上の安定な領域を図示。


差分方程式に具体例を追加したい。
</pre>
