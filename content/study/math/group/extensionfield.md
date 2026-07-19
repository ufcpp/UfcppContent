---
title: "体の拡大"
source_url: "https://ufcpp.net/study/math/group/extensionfield/"
content_type: "Article"
published_at: "2015-05-06T14:17:26"
updated_at: "2015-05-06T14:17:26"
tags: []
umbraco_id: 1488
parent_id: 1483
sort_order: 4
aliases:
  - "/group/extensionfield"
  - "/group/extensionfield.html"
  - "/math/group/extensionfield/"
  - "/study/group/extensionfield"
  - "/study/group/extensionfield.html"
---

# 体の拡大

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

実数体と有理数体のように、集合として包含関係のある体が存在します。
このとき、その2つの体の関係を、拡大体・部分体という言葉で表します。
また、有理数体から実数体を作るように、自身を部分集合として含む、より大きな体を作ることを体の拡大といいます。
 
実は、体に対して、機械的な操作で体の拡大を行うことが出来ます。


## <a id="sec-generated-title-2"></a> <a id="extension"></a>部分体と拡大体

体 <span class="math">A</span> の部分代数系 <span class="math">B</span> が体になっているとき、
<span class="math">B</span> を <span class="math">A</span> の<strong id="subfield" class="keyword">部分体</strong>（subfield）、
<span class="math">A</span> を <span class="math">B</span> の<strong id="extension" class="keyword">拡大体</strong>（extended field または extension field）と言います。
 
また、体 <span class="math">A, B</span> 間の関係を体の拡大（field extension）と呼び、
<span class="math">A/B</span> と書き表します。
 
分かりやすい例を挙げると、
有理数体 <span class="math"><span class="bold">Q</span></span>、
実数体 <span class="math"><span class="bold">R</span></span> および
複素数体 <span class="math"><span class="bold">C</span></span> の間には、
<span class="math"><span class="bold">C</span> ⊃ <span class="bold">R</span> ⊃ <span class="bold">Q</span></span>
をいう関係があるので、

* 実数は有理数の拡大体。有理数は実数の部分体。

* 複素数は実数の拡大体。実数は複素数の部分体。


になります。


### <a id="sec-generated-title-3"></a> <a id="order"></a>拡大次数

複素数体は実数体上の2次元ベクトルとして考えることも出来るわけですが、
拡大体 <span class="math">E</span> は必ず体 <span class="math">K</span> のベクトル空間になっています。
なぜならば、<span class="math">a, b ∈ K, x, y ∈ E</span> とすると、
<div class="math">
a x ＋ b y ∈ E
</div>
であり、加法と、<span class="math">K</span> の元によるスカラー倍が定義できるからです。
 
このように、体 <span class="math">K</span> の拡大体 <span class="math">E</span> は <span class="math">K</span> 上のベクトル空間になるわけですが、
拡大体 <span class="math">E</span> の <span class="math">K</span> 上のベクトル空間としての次元が <span class="math">n</span> であるとき、
体 <span class="math">E</span> を <span class="math">K</span> の <em><span class="math">n</span> 次拡大体</em>と呼びます。
このとき、<span class="math">n</span> を
拡大 <span class="math">E/K</span> の次数（order of extension）と呼び、
<span class="math"><span class="paren" style="font-size:em;">[</span>E:K<span class="paren" style="font-size:em;">]</span></span> で表します。
 
拡大の次数 <span class="math"><span class="paren" style="font-size:em;">[</span>E:K<span class="paren" style="font-size:em;">]</span></span> が有限のとき、
<span class="math">E/K</span> を有限拡大（finite extension）と呼び、
そうでないとき、無限拡大（infinite extension）と呼びます。
有理数 → 実数 <span class="math"><span class="bold">R</span>/<span class="bold">Q</span></span> は無限拡大、
実数 → 複素数 <span class="math"><span class="bold">C</span>/<span class="bold">R</span></span> は有限（2次）拡大になります。
 
ちなみに、有理数 → 実数 → 複素数というように、
体の拡大は複数段になっている場合もあります。
3つの体 <span class="math">A, B, C</span> があって、
その間に
<span class="math">C/B</span> と <span class="math">B/A</span> いう関係がある場合、
その拡大の次数には、
<em>
        <div class="math">
          <span class="paren" style="font-size:em;">[</span>C:A<span class="paren" style="font-size:em;">]</span>
＝
<span class="paren" style="font-size:em;">[</span>C:B<span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">[</span>B:A<span class="paren" style="font-size:em;">]</span></div>
      </em>
という関係が成り立ちます。
<span class="math">
n ＝ <span class="paren" style="font-size:em;">[</span>C:B<span class="paren" style="font-size:em;">]</span> , 
m ＝ <span class="paren" style="font-size:em;">[</span>B:A<span class="paren" style="font-size:em;">]</span></span>
として、
<span class="math">B</span> の <span class="math">A</span> 上のベクトル空間としての基底を
<span class="math"><span class="paren" style="font-size:em;">{</span>a<sub>i</sub> | i ＝ 0 ～ m － 1<span class="paren" style="font-size:em;">}</span></span>、
<span class="math">C</span> の <span class="math">B</span> 上のベクトル空間としての基底を
<span class="math"><span class="paren" style="font-size:em;">{</span>b<sub>j</sub> | j ＝ 0 ～ n － 1<span class="paren" style="font-size:em;">}</span></span>
と置くと、
<span class="math">C</span> の <span class="math">A</span> 上のベクトル空間としての基底は
<span class="math"><span class="paren" style="font-size:em;">{</span>
a<sub>i</sub> b<sub>j</sub> |
 i ＝ 0 ～ m － 1,
 j ＝ 0 ～ n － 1
<span class="paren" style="font-size:em;">}</span></span>
の <span class="math">mn</span> 個になります。
 
ちなみに、
<span class="math">C/B</span> と <span class="math">B/A</span> の <span class="math">B</span> のように、
他の2つの体の間に挟まっている体を中間体（intermediate field）と呼びます。


### <a id="sec-generated-title-4"></a> <a id="example"></a>拡大体の例

複素数、実数、有理数などの間の関係は分かりやすい例ですが、
その他にもいくつか、拡大体の例を挙げます。
 
有理数体 <span class="math"><span class="bold">Q</span></span> に、無理数 <span class="math"><span class="normal" style="font-size:em;">√</span><span class="bar">2</span></span> を加えた集合
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span> を考えて見ましょう。
この集合は、<span class="math">a, b ∈ <span class="bold">Q</span></span> として、
<div class="math">
a ＋ b <span class="normal" style="font-size:em;">√</span><span class="bar">2</span></div>
と表すことができ、有理数体 <span class="math"><span class="bold">Q</span></span> 上の2次元ベクトル空間になっています。
<span class="math">
a ＋ b <span class="normal" style="font-size:em;">√</span><span class="bar">2</span>, 
c ＋ d <span class="normal" style="font-size:em;">√</span><span class="bar">2</span> 
∈
<span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span>
に対して、
<div class="math">
        <span class="paren" style="font-size:em;">(</span>a ＋ b <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">)</span>
±
<span class="paren" style="font-size:em;">(</span>c ＋ d <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:em;">(</span>a ± c<span class="paren" style="font-size:em;">)</span> ＋ <span class="paren" style="font-size:em;">(</span>b ± d<span class="paren" style="font-size:em;">)</span><span class="normal" style="font-size:em;">√</span><span class="bar">2</span>
∈
<span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></div><div class="math">
        <span class="paren" style="font-size:em;">(</span>a ＋ b <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">)</span>
×
<span class="paren" style="font-size:em;">(</span>c ＋ d <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:em;">(</span>ac ＋ 2bd<span class="paren" style="font-size:em;">)</span> ＋ <span class="paren" style="font-size:em;">(</span>ad ＋ bc<span class="paren" style="font-size:em;">)</span><span class="normal" style="font-size:em;">√</span><span class="bar">2</span>
∈
<span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></div><div class="math">
1 ÷
<span class="paren" style="font-size:em;">(</span>a ＋ b <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">a</td></tr><tr><td>a<sup>2</sup> － 2 b<sup>2</sup></td></tr></table>
－
<table class="frac" summary="fraction"><tr><td class="num">b</td></tr><tr><td>a<sup>2</sup> － 2 b<sup>2</sup></td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">2</span>
∈
<span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></div>
が成り立っているので、
この集合 <span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span> は体になっています。
もちろん、<span class="math"><span class="bold">Q</span> ⊂ <span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span> なので、
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span> は
<em>有理数体 <span class="math"><span class="bold">Q</span></span> の2次拡大体</em>になります。
 
同様に、
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span><span class="paren" style="font-size:em;">}</span></span> も有理数体 <span class="math"><span class="bold">Q</span></span> の2次拡大体になる事が証明できます。
さらに、
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span><span class="paren" style="font-size:em;">}</span></span> という集合を作ると、
これは有理数体 <span class="math"><span class="bold">Q</span></span> の4次拡大体になります。
（<span class="math">1, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">6</span></span> が一次独立なので、4次元ベクトル空間になる。）
また、<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span><span class="paren" style="font-size:em;">}</span></span> は、
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span> の2次拡大体でも、
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span><span class="paren" style="font-size:em;">}</span></span> の2次拡大体でもあります。
<table class="layout" summary="レイアウト用テーブル">
<tr><td><span class="bold">Q</span></td><td>→</td><td>2次拡大</td><td>→</td><td><span class="math">
              <span class="paren" style="font-size:em;">{</span>
                <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span>
            </span></td></tr><tr><td>↓</td><td>＼</td><td></td><td></td><td>↓</td></tr><tr><td>2次拡大</td><td></td><td>4次拡大</td><td></td><td>2次拡大</td></tr><tr><td>↓</td><td></td><td></td><td>＼</td><td>↓</td></tr><tr><td><span class="math">
              <span class="paren" style="font-size:em;">{</span>
                <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span><span class="paren" style="font-size:em;">}</span>
            </span></td><td>→</td><td>2次拡大</td><td>→</td><td><span class="math">
              <span class="paren" style="font-size:em;">{</span>
                <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">3</span><span class="paren" style="font-size:em;">}</span>
            </span></td></tr></table>



## <a id="sec-generated-title-5"></a> <a id="extend"></a>体の拡大方法

冒頭でも述べましたが、体に対して、機械的な操作で体の拡大を行うことが出来ます。
 
例えば、ある体 <span class="math">K</span> 上の有理式 <span class="math">K<span class="paren" style="font-size:em;">(</span>X<span class="paren" style="font-size:em;">)</span></span> は体になりますが、有理式体 <span class="math">K<span class="paren" style="font-size:em;">(</span>X<span class="paren" style="font-size:em;">)</span></span> は体 <span class="math">K</span> を部分集合として含みます。
したがって、
体 <span class="math">K</span> から、有理式体 <span class="math">K<span class="paren" style="font-size:em;">(</span>X<span class="paren" style="font-size:em;">)</span></span> を作ることで、
機械的に体 <span class="math">K</span> の拡大体を作れることになります。
 
その他にも機械的に体を拡大する方法があるわけですが、
特に重要な体の拡大方法として、
代数拡大というものと、
完備拡大というものがあります。
以下のセクションではこの2つに関して説明していきます。


## <a id="sec-generated-title-6"></a> <a id="algebraic"></a>代数拡大

1つ目の体の拡大方法は代数拡大と呼ばれるものです。
「[拡大体の例](#example)」で説明したような、
有理数体 <span class="math"><span class="bold">Q</span></span> から
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span> への拡大や、
実数 → 複素数の拡大はこの方法を使ったものになります。
 
代数方程式 <span class="math">X<sup>2</sup> － 2 ＝ 0</span> は、
全ての係数が有理数であるにも関らず、
その解は有理数にはなりません
（<span class="math">X ＝ ±<span class="normal" style="font-size:em;">√</span><span class="bar">2</span></span> は無理数）。
この有理数の範囲では解けない方程式
<span class="math">X<sup>2</sup> － 2 ＝ 0</span>
の解を有理数に付け加えたのが、
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span>
という拡大体です。
 
実数 → 複素数も同様で、
代数方程式 <span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span> は、
全ての係数が実数であるにも関らず、
解は実数にはなりません。
この方程式の解、いわゆる虚数単位 <span class="math">i</span> を実数に付け加えたのが複素数です。
 
すなわち、
ある体 <span class="math">K</span> があるとき、
体 <span class="math">K</span> を係数とする代数方程式の解が
<span class="math">K</span> 上に存在するとは限らないわけですが、
その「解けない方程式」の解を形式的に用意して、
体 <span class="math">K</span> に付け加えることで拡大体を作ることができます。
このような手順で拡大体を作ることを<strong id="algebraic" class="keyword">代数拡大</strong>（algebraic extension）とよび、
作られた拡大体のことを代数拡大体と呼びます。


### <a id="sec-generated-title-7"></a> <a id="residual"></a>多項式環の剰余体

実は、
体 <span class="math">K</span> の代数拡大体というのは、
<span class="math">K</span> 上の多項式環 <span class="math">K<span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span> の「[剰余体](quotientfield.md#residual)」になっています。
 
再び実数 → 複素数を例にして説明すると、
実数に虚数単位 <span class="math">i</span> を加えるという操作は、以下のような解釈をすることができます。

* 実数<span class="math"><span class="bold">R</span></span>の範囲では解けない代数方程式<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>の解を実数に付け加える。

* 実数<span class="math"><span class="bold">R</span></span>に対して、条件<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>を満たす新しい元<span class="math">X</span>を加える。

* 実数上の多項式<span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>の変数<span class="math">X</span>に対して、条件<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>を付ける。

* 実数上の多項式<span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>を既約多項式<span class="math">X<sup>2</sup> ＋ 1 ＝ 0</span>で割ったあまりの集合を作る。

* <span class="math">X<sup>2</sup> ＋ 1</span>を生成元とするイデアル<span class="math"><span class="paren" style="font-size:em;">(</span>X<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>を使って剰余環<span class="math"><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>/<span class="paren" style="font-size:em;">(</span>X<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><span class="bold">R</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>を作る。


この5つの解釈は結局の所同じことを言っているのですが、
最後の1つはまさに多項式環の剰余体のことを言っています。
すなわち、
代数拡大とは、
多項式環の剰余体を作る操作と同じことになります。
 
実数 → 複素数の場合と同様に、
有理数体 <span class="math"><span class="bold">Q</span></span> の拡大体
<span class="math"><span class="paren" style="font-size:em;">{</span><span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">2</span><span class="paren" style="font-size:em;">}</span></span>
は、
有理数体上の多項式環
<span class="math"><span class="bold">Q</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>
の剰余体
<span class="math"><span class="bold">Q</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span>
/
<span class="paren" style="font-size:em;">(</span>X<sup>2</sup> － 2<span class="paren" style="font-size:em;">)</span><span class="bold">Q</span><span class="paren" style="font-size:em;">[</span>X<span class="paren" style="font-size:em;">]</span></span>
と同型な体になります。
 
ちなみに、
この代数拡大体を作る際に使用する
「解けない方程式」すなわち既約多項式を、
拡大体の<strong id="d56e945" class="keyword">生成多項式</strong>と呼びます。


### <a id="sec-generated-title-8"></a> <a id="closed"></a>代数的閉体

要するに、任意の体 <span class="math">K</span> に対して、
「解けない方程式」さえあれば代数拡大体を作ることが出来ます。
 
ですが、逆に言うと、
体 <span class="math">K</span> 上の任意の代数方程式が、
<span class="math">K</span> 内に解を持つ場合、
その体はそれ以上代数拡大することができません。
例えば、複素数がその代表例なのですが、
複素数係数の任意の代数方程式は、必ず複素数の解を持ちます（それも一意に定まる）。
したがって、複素数はこれ以上代数拡大することができません。
 
このように、代数拡大することのできない体が存在するわけですが、
このような体を<strong id="closedfield" class="keyword">代数的閉体</strong>（algebraic closed field）といいます。


### <a id="sec-generated-title-9"></a> <a id="finite"></a>有限体の代数拡大

「解けない方程式」さえあれば（すなわち代数的閉体でなければ）代数拡大体を作ることが出来るわけですが、
ここでもう1つ拡大体の例を挙として、有限体（整数の剰余体）<span class="math"><span class="bold">Z</span>/p<span class="bold">Z</span></span> の代数拡大について説明します。
（整数の剰余体に関しては、「[整数の剰余体](field.md#rasidualfield)」を参照してください。）
 
とりあえず、最も簡単な有限体ということで、
「[ブール体](field.md#bool)」<span class="math"><span class="bold">B</span></span> の代数拡大を例として挙げます。
<span class="math"><span class="bold">B</span></span> の既約多項式（解けない方程式）はいくらでもあって、
例えば、以下のようなものがあります。

* <span class="math">X<sup>2</sup> ＋ X ＋ 1</span>

* <span class="math">X<sup>3</sup> ＋ X ＋ 1</span>

* <span class="math">X<sup>3</sup> ＋ X<sup>2</sup> ＋ 1</span>

* <span class="math">X<sup>4</sup> ＋ X ＋ 1</span>


これらの多項式は、<span class="math">X</span> に 1 を代入しても 0 を代入しても 1 にしかなりません。
（ブール体では、<span class="math">1 ＋ 1 ＝ 0, 1 × 1 ＝ 1</span>。）
すなわち、ちゃんと既約多項式になっているわけで、
これらの多項式を使ってブール体を代数拡大することができます。
<h4>例1（2次拡大）</h4>
それではまず、1番次数の低い <span class="math">X<sup>2</sup> ＋ X ＋ 1</span> を使ったブール体の代数拡大を見てみましょう。

<span class="math">X<sup>2</sup> ＋ X ＋ 1 ＝ 0</span> という条件は、
<span class="math">X<sup>2</sup> ＝ X ＋ 1</span> とも書けますので、
<span class="math">X</span> の項が2次以上の場合、この条件を使って <span class="math">X</span> の次数を1次以下に下げることができます。
したがって、この体は <span class="math">0, 1, X, X ＋ 1</span> の4つの元からなる体になります。
 
ちなみに、これは、<span class="math">a ＋ bX</span>（<span class="math">a, b ⊂ <span class="bold">B</span> ＝ <span class="paren" style="font-size:em;">{</span>0, 1<span class="paren" style="font-size:em;">}</span></span>）と書くこともでき、
ブール体 <span class="math"><span class="bold">B</span></span> の2次元ベクトル空間になっていることがわかるので、
この代数拡大は2次の拡大になります。
 
さて、それでは、この集合 <span class="math"><span class="paren" style="font-size:em;">{</span>0, 1, X, X ＋ 1<span class="paren" style="font-size:em;">}</span></span> がちゃんと体になっていることを確かめて見ましょう。
そのために、表1 に示すようにこれらの中の 0 以外の3つの元の冪乗を計算してみましょう。

<table summary="ブール体の2次拡大体の元の冪乗">
	<caption>
		ブール体の2次拡大体の元の冪乗
	</caption>
	<tr>
		<th>元</th>
		<th>2乗</th>
		<th>3乗</th>
		<th>4乗</th>
	</tr>
	<tr>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＝ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＝ 1</span></td>
		<td markdown="1"><span class="math">X</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1 ＝ X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＝ 1</span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
	</tr>
</table>


見ての通り、
3乗すると必ず 1 になり、4乗すると元に戻ります。
従って、2乗したもの <span class="math">x<sup>2</sup></span> が
乗法に関する逆元 <span class="math">x<sup>－1</sup></span> になります。
ちなみに、ブール体の性質から、加法に関する逆元はそれ自身になります。
（ <span class="math">x ＋ x ＝ 0</span> 。）
加法に関しても乗法に関しても全ての元が逆元を持っていますので、
この集合が体を成していることが分かります。
<h4>例2（3次拡大）</h4>
では次に、3次の既約多項式を使った代数拡大を見てみましょう。
3次の既約多項式は2つありますので、
それぞれを区別するために、変数の文字を変え、
<div class="math">
X<sup>3</sup> ＋ X ＋ 1
</div><div class="math">
Y<sup>3</sup> ＋ Y<sup>2</sup> ＋ 1
</div>
と表しましょう。
いずれの既約多項式を使った場合でも、
代数拡大は3次の代数拡大となり、
その元は、
<span class="math">a, b, c ⊂ <span class="bold">B</span> ＝ <span class="paren" style="font-size:em;">{</span>0, 1<span class="paren" style="font-size:em;">}</span></span>
とすると、
<span class="math">a ＋ bX ＋ cX<sup>2</sup></span> または
<span class="math">a ＋ bY ＋ cY<sup>2</sup></span> と表すことが出来ます。
従って、元の数は <span class="math">2<sup>3</sup> ＝ 8</span> 個あるわけですが、
例1のときと同様に
そのうちの 0 ではない7つの元に対して、冪乗がどうなっているかを見てみましょう。
その結果を表2および3に示します。

<table summary="ブール体の3次拡大体の元の冪乗（X）">
	<caption>
		ブール体の3次拡大体の元の冪乗（X）
	</caption>
	<tr>
		<th>元</th>
		<th>2乗</th>
		<th>3乗</th>
		<th>4乗</th>
		<th>5乗</th>
		<th>6乗</th>
		<th>7乗</th>
	</tr>
	<tr>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
</table>


<table summary="ブール体の3次拡大体の元の冪乗（Y）">
	<caption>
		ブール体の3次拡大体の元の冪乗（Y）
	</caption>
	<tr>
		<th>元</th>
		<th>2乗</th>
		<th>3乗</th>
		<th>4乗</th>
		<th>5乗</th>
		<th>6乗</th>
		<th>7乗</th>
	</tr>
	<tr>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
</table>


いずれの生成多項式を使った場合でも、
7乗したものは必ず <span class="math">1</span> になっています。
従って、ある元 <span class="math">x</span> を6乗したもの <span class="math">x<sup>6</sup></span> は
情報に関する逆元 <span class="math">x<sup>－1</sup></span> になります。
2次拡大のときと同様に、<span class="math">x</span> の加法に関する逆元はそれ自身 <span class="math">x</span> であり、
加法・乗法共に逆元を持ち、この集合は体になっている事が分かります。
<h4>2つの3次拡大は互いに体同型</h4>
2つの生成多項式
<span class="math">
X<sup>3</sup> ＋ X ＋ 1
</span>
と
<span class="math">
Y<sup>3</sup> ＋ Y<sup>2</sup> ＋ 1
</span>
を使って、一見すると異なる2つの拡大体を作りましたが、
実はこの2つの体は互いに同型になります。
 
なぜかというと、
1つ目の生成多項式
<span class="math">
X<sup>3</sup> ＋ X ＋ 1
</span>
に、
<span class="math">X ＝ Y ＋ 1</span>
を代入すると、
<div class="math">
X<sup>3</sup> ＋ X ＋ 1
＝
<span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span><sup>3</sup> ＋ <span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span> ＋ 1
</div><div class="math">
　＝
Y<sup>3</sup> ＋
Y<sup>2</sup> ＋
Y ＋ 1 ＋
Y ＋ 1 ＋
1
＝
Y<sup>3</sup> ＋
Y<sup>2</sup> ＋
1
</div>
となり、2つ目の生成多項式になるからです。
確認のために、表2の <span class="math">X</span> の行に <span class="math">Y ＋ 1</span> を代入して、
表3の <span class="math">Y ＋ 1</span> の行と比較してみましょう。

<table summary="X ＝ Y ＋ 1">
	<caption>
		X ＝ Y ＋ 1
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>元</th>
		<th>2乗</th>
		<th>3乗</th>
		<th>4乗</th>
		<th>5乗</th>
		<th>6乗</th>
		<th>7乗</th>
	</tr>
	<tr>
		<th>表2（元）</th>
		<td markdown="1"><span class="math">X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup></span></td>
		<td markdown="1"><span class="math">X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ X ＋ 1</span></td>
		<td markdown="1"><span class="math">X<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<th>表2（代入）</th>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">
              <span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span>
              <sup>2</sup> ＝</span><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">
              <span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span> ＋ 1 ＝</span><span class="math">Y</span></td>
		<td markdown="1"><span class="math">
              <span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span>
              <sup>2</sup>
            </span><span class="math"> ＋ <span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span> ＝</span><span class="math">Y<sup>2</sup> ＋ 1</span><span class="math"> ＋ Y ＋ 1 ＝</span><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">
              <span class="paren" style="font-size:em;">(</span>Y ＋ 1<span class="paren" style="font-size:em;">)</span>
              <sup>2</sup> ＋ 1 ＝</span><span class="math">Y<sup>2</sup> ＋ 1 ＋ 1 ＝</span><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
	<tr>
		<th>表3</th>
		<td markdown="1"><span class="math">Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ 1</span></td>
		<td markdown="1"><span class="math">Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup> ＋ Y ＋ 1</span></td>
		<td markdown="1"><span class="math">Y<sup>2</sup></span></td>
		<td markdown="1"><span class="math">1</span></td>
	</tr>
</table>


ちなみに、
<span class="math">
X<sup>3</sup> ＋ X ＋ 1
</span>
を使った方の元を、
<span class="math">a ＋ bX ＋ cX<sup>2</sup></span>
（<span class="math">a, b, c ⊂ <span class="bold">B</span></span> ）
で、
<span class="math">
Y<sup>3</sup> ＋ Y<sup>2</sup> ＋ 1
</span>
を使った方の元を、
<span class="math">d ＋ eY ＋ fY<sup>2</sup></span>
（<span class="math">d, e, f ⊂ <span class="bold">B</span></span> ）
で表すと、
<div class="math">
        <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>d</td></tr><tr><td>e</td></tr><tr><td>f</td></tr></table><span class="paren" style="font-size:4em;">]</span>
＝
<span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>1</td><td>1</td><td>1</td></tr><tr><td>0</td><td>1</td><td>0</td></tr><tr><td>0</td><td>0</td><td>1</td></tr></table><span class="paren" style="font-size:4em;">]</span><span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>a</td></tr><tr><td>b</td></tr><tr><td>c</td></tr></table><span class="paren" style="font-size:4em;">]</span></div>
という線形写像で関係付けることが出来ます。


## <a id="sec-generated-title-10"></a> <a id="plan"></a>執筆予定

```text
・<strong id="completed" class="keyword">完備拡大</strong>（completed extension）
	完備化（completion）するとも言う。
	体上の数列の極限値。
	極限を定義する際のノルムのとり方によって色々な拡大の仕方が出来る。
	例:
		有理数→実数
		{有理数, 虚数単位}→複素数
		p進体


・代数拡大

ちなみに、
E/K で、α ∈ E － K のとき、
E/K(α), K(α)/K
になる。
特に、
E ＝ K(α)
のとき、E/F を単純拡大（simple）という。

αが K の超越的数のとき、
K(α) は K 上の有理式体 K(X) と同型。

αが K の代数的数のとき、
E 上で f[α] ＝ 0 となり、K 上で既約になる多項式 f[α] が必ず存在し、
K(α) は剰余体 K[X]/f[X]K[X] と同型。

n 次代数拡大というのは、
代数的な n 個の元 αi（i ＝ 0 ～ n－1）を使って
n 回単純拡大を繰り返したもの。



・代数的閉包

有限次の拡大は常に代数拡大。
E/K, α ∈ E － K, [E:K] ＝ n のとき、
1, α, α<sup>2</sup>, ・・・, α<sup>n</sup> は線形従属
（E が K 上の n 次元ベクトル空間なので、n＋1 個の元は必ず線形従属）
なので、
∃ci ∈ K （i ＝ 0 ～ n）, Σ ci α<sup>i</sup> ＝ 0
で、αは K 上で代数的。


逆に言うと、[K(α):K] ＜ ∞ となるような元 α は K 上で代数的。

E/K に対して、
~K ＝ {α ∈ E | [K(α):K] ＜ ∞}
（E の中で、K 上の代数的な元全体の集合）
を（E 中における） K の代数的閉包（algegraic closure）と呼ぶ。

特に、複素数 <span class="bold">C</span> 中における有理数 <span class="bold">Q</span> の代数的閉包を
代数的数（algebraic number）と呼ぶ。


・有限体(ガロア体)の性質

有限体の説明は別ページに移動。
そちらにリンクを張る。
```
