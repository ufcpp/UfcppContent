---
title: "主成分分析"
source_url: "https://ufcpp.net/study/math/linear/principal/"
content_type: "Article"
published_at: "2015-05-06T14:16:36"
updated_at: "2015-05-06T14:16:36"
tags: []
umbraco_id: 1463
parent_id: 1458
sort_order: 4
aliases:
  - "/linear/principal"
  - "/linear/principal.html"
  - "/math/linear/principal/"
  - "/study/linear/principal"
  - "/study/linear/principal.html"
---

# 主成分分析

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[固有値問題](eigen.md#eigen_problem)」の応用の1つとして、主成分分析を紹介。


## <a id="sec-generated-title-2"></a> <a id="preparation"></a>記号の準備

これから説明する主成分分析は、
<span class="math">N</span> 個のベクトル
<span class="math"><span class="vector">x</span><sub>1</sub> , <span class="vector">x</span><sub>2</sub> , 
・・・,
<span class="vector">x</span><sub>N</sub></span>
に対して、その線形結合
<div class="math">
      <span class="vector">y</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝1</td></tr></table>
a<sub>i</sub> <span class="vector">x</span><sub>i</sub></div>
を考えます。
<span class="math">N</span> 個の線形結合を考えて、
<div class="math">
      <span class="vector">y</span>
      <sub>i</sub>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">j＝1</td></tr></table>
a<sub>i j</sub> <span class="vector">x</span><sub>j</sub></div>
として考える場合もあります。
ベクトル
<span class="math"><span class="vector">x</span><sub>i</sub></span>
の次元は特に限定しませんが、
内積が定義できるベクトル空間の元であることを仮定します。

<span class="math">N</span> 個のベクトル列
<span class="math"><span class="vector">x</span><sub>1</sub> , <span class="vector">x</span><sub>2</sub> , 
・・・,
<span class="vector">x</span><sub>N</sub></span>
を、1まとめにして、
<div class="math">
X
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">x</span><sub>1</sub> <span class="vector">x</span><sub>2</sub> 
・・・ <span class="vector">x</span><sub>N</sub><span class="paren" style="font-size:em;">)</span></div>
と表現します。
<span class="math"><span class="vector">x</span><sub>i</sub></span>
が <span class="math">M</span> 次の縦ベクトルの場合には、
これは <span class="math">M</span>×<span class="math">N</span> 行列に相当します。
また、係数 <span class="math">a<sub>i j</sub></span> も行列として表現します。
<div class="math">
      <span class="vector">a</span>
      <sub>i</sub>
＝
<span class="paren" style="font-size:em;">(</span>
a<sub>i 1</sub> , 
a<sub>i 2</sub> , 
・・・,
a<sub>i N</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></div><div class="math">
A
＝
<span class="paren" style="font-size:em;">{</span>a<sub>i j</sub><span class="paren" style="font-size:em;">}</span>
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub> , 
・・・,
<span class="vector">a</span><sub>N</sub><span class="paren" style="font-size:em;">)</span></div>
この記法に従うなら、先ほどの線形結合の式は、
<div class="math">
      <span class="vector">y</span>
      <sub>i</sub>
＝
X <span class="vector">a</span><sub>i</sub></div><div class="math">
Y
＝
X A
　　
<span class="normal">ただし、</span>
Y
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>1</sub> <span class="vector">y</span><sub>2</sub> 
・・・ <span class="vector">y</span><sub>N</sub><span class="paren" style="font-size:em;">)</span></div>
となります。
 
また、2つのベクトル
<span class="math"><span class="vector">x</span><sub>i</sub></span>, 
<span class="math"><span class="vector">x</span><sub>j</sub></span>
の内積を
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">x</span>
        <sub>i</sub>
         , <span class="vector">x</span><sub>j</sub><span class="paren" style="font-size:em;">)</span>
    </div>
で表します。
そして、ベクトル列 <span class="math">X</span> の<strong id="correlation" class="keyword">相関行列</strong>（correlation matrix） <span class="math">R<sub>X</sub></span> を
<div class="math">
R<sub>X</sub>
＝
X<sup>T</sup> X
＝
<span class="paren" style="font-size:em;">{</span><span class="paren" style="font-size:em;">(</span><span class="vector">x</span><sub>i</sub> , <span class="vector">x</span><sub>j</sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></div>
と定義します。
（ベクトル/行列の右肩の <span class="math"><sup>T</sup></span> は転置を表します。）
<span class="math">X</span> の各ベクトル <span class="math"><span class="vector">x</span><sub>i</sub></span> の次元がなんであれ、
<span class="math">R<sub>X</sub></span> は
<span class="math">N</span> 次の正定値で対称な正方行列になります。
 
ちなみに、<span class="math">Y ＝ X A</span> のとき、
<div class="math">
R<sub>Y</sub>
＝
Y<sup>T</sup> Y
＝
A<sup>T</sup> X<sup>T</sup> X A
＝
A<sup>T</sup> R<sub>X</sub> A
</div>
と表すことができます。


## <a id="sec-generated-title-3"></a> <a id="principal"></a>主成分分析

主成分分析にはいくつか表現のしかたがあります。
（結局は同じ式、同じ結果に帰着。）


### <a id="sec-generated-title-4"></a> <a id="d34e402"></a>線形結合結果の絶対値最大化

1つ目は、ベクトル列 <span class="math">X</span> の線形結合
<div class="math">
        <span class="vector">y</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝1</td></tr></table>
a<sub>i</sub> <span class="vector">x</span><sub>i</sub>
＝
X <span class="vector">a</span></div>
に対して、
<span class="math"><span class="normal">|</span><span class="vector">a</span><span class="normal">|</span><sup>2</sup> ＝ 1</span>
という制約条件化で、
<span class="math"><span class="vector">y</span></span>
の絶対値
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="vector">y</span>, <span class="vector">y</span><span class="paren" style="font-size:em;">)</span></span>
の値を最大化したいというものです。
 
その結果得られるベクトル <span class="math"><span class="vector">y</span></span> は、
<span class="math">N</span> 個のベクトル列
<span class="math">
X ＝
<span class="vector">x</span><sub>1</sub> , <span class="vector">x</span><sub>2</sub> , 
・・・,
<span class="vector">x</span><sub>N</sub></span>
を最もよく代表する1本のベクトルと考えることができます。
「最もよく代表するベクトル」という意味で、
<span class="math"><span class="vector">y</span></span> をベクトル列 <span class="math">X</span> の（第一）<em>主成分</em>と呼びます。
 
主成分の求め方ですか、
条件付極値問題になりますので、
ルジャンドルの未定乗数法を使って、
<div class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="vector">y</span>, <span class="vector">y</span><span class="paren" style="font-size:em;">)</span>
－
λ
<span class="paren" style="font-size:em;">(</span><span class="normal">|</span><span class="vector">a</span><span class="normal">|</span><sup>2</sup> － 1
<span class="paren" style="font-size:em;">)</span>
＝
<span class="vector">a</span><sup>T</sup>
R<sub>X</sub> <span class="vector">a</span>
－
λ
<span class="paren" style="font-size:em;">(</span><span class="normal">|</span><span class="vector">a</span><span class="normal">|</span><sup>2</sup> － 1
<span class="paren" style="font-size:em;">)</span></div>
の極値問題に帰着されます。
したがって、この式を、<span class="math"><span class="vector">a</span></span> の各要素で微分することで、
<div class="math">
R<sub>X</sub> <span class="vector">a</span>
＝
λ
<span class="vector">a</span>
,
　　　<span class="normal">|</span><span class="vector">a</span><span class="normal">|</span><sup>2</sup> ＝ 1
</div>
という条件が得られ、
相関行列 <span class="math">R<sub>X</sub></span> の固有値問題に帰着されます。

<span class="math">N</span> 次正方行列の固有値問題の解は <span class="math">N</span> 個ありますが、
このうち、
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="vector">y</span>, <span class="vector">y</span><span class="paren" style="font-size:em;">)</span></span>
を最大化するという条件を満たすのは、
絶対値が最大の固有値に属する固有ベクトルです。
 
ちなみに、
最大固有値以外の固有ベクトルを用いても同様に
<span class="math"><span class="vector">y</span></span> に相当する物が得られるわけですが、
これを、固有値が大きい順に
第一主成分、第二主成分・・・と呼びます。


### <a id="sec-generated-title-5"></a> <a id="d34e599"></a>ベクトル列の直交化

もう1つの考え方としては、
ベクトル列 <span class="math">X</span> を、
直交変換 <span class="math">A</span> によって直交したベクトル列 <span class="math">Y</span> に変換するという考え方です。
要するに、
<div class="math">
Y ＝ X A
</div>
というベクトル列 <span class="math">X</span> → ベクトル列 <span class="math">Y</span> の変換において、
<span class="math">A<sup>T</sup> A ＝ I</span> （<span class="math">I</span> は <span class="math">N</span> 次単位行列）
という制約の元、
<span class="math">Y<sup>T</sup> Y ＝ R<sub>Y</sub></span> を対角化したいというものです。
（対角化の問題は、結局の所、固有値問題になります。）
 
まず、<span class="math">R<sub>Y</sub></span> が対角行列になって欲しいわけで、
これを
<div class="math">
R<sub>Y</sub>
＝
<span class="normal">diag</span><span class="paren" style="font-size:em;">{</span><span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>i</sub> , <span class="vector">y</span><sub>i</sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></div>
と書きます。
（ただし、<span class="math"><span class="normal">diag</span><span class="paren" style="font-size:em;">{</span>λ<sub>i</sub><span class="paren" style="font-size:em;">}</span></span> は、
<span class="math">i</span> 行 <span class="math">i</span> 列の対角成分が <span class="math">λ<sub>i</sub></span> であるような対角行列。）
このとき、
<span class="math">Y ＝ X A</span> のとき、
<span class="math">R<sub>Y</sub> ＝ A<sup>T</sup> R<sub>X</sub> A</span>
なので、
<div class="math">
        <span class="normal">diag</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="vector">y</span>
            <sub>i</sub>
             , <span class="vector">y</span><sub>i</sub><span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">}</span>
＝ A<sup>T</sup> R<sub>X</sub> A
</div>
となります。
また、<span class="math">A</span> が直交変換、
すなわち、<span class="math">A<sup>－1</sup> ＝ A<sup>T</sup></span>
という条件から、
<div class="math">
R<sub>X</sub> A
＝
A
<span class="normal">diag</span><span class="paren" style="font-size:em;">{</span><span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>i</sub> , <span class="vector">y</span><sub>i</sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></div>
が得られます。
これを、<span class="math">A</span> に関して列ごとに書き下すと、
<div class="math">
R<sub>X</sub> <span class="vector">a</span><sub>i</sub>
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>i</sub> , <span class="vector">y</span><sub>i</sub><span class="paren" style="font-size:em;">)</span> <span class="vector">a</span><sub>i</sub>
,
　　　<span class="normal">|</span><span class="vector">a</span><sub>i</sub><span class="normal">|</span><sup>2</sup> ＝ 1
</div>
となり、
やはり、
相関行列 <span class="math">R<sub>X</sub></span> の固有値問題に帰着されます。
（このとき、
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>i</sub> , <span class="vector">y</span><sub>i</sub><span class="paren" style="font-size:em;">)</span></span>
は <span class="math">R<sub>X</sub></span> の固有値になる。）
ただし、固有値が 0 のときは <span class="math"><span class="vector">a</span><sub>i</sub> ＝ 0</span> とします。
 
この際に得られる、
固有ベクトルは、固有値の大きい物から順にならべておく習慣があります。
すなわち、
<div class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="vector">y</span>
          <sub>1</sub>
           , <span class="vector">y</span><sub>1</sub><span class="paren" style="font-size:em;">)</span>
＞
<span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>2</sub> , <span class="vector">y</span><sub>2</sub><span class="paren" style="font-size:em;">)</span>
＞
・・・
＞
<span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>N</sub> , <span class="vector">y</span><sub>N</sub><span class="paren" style="font-size:em;">)</span></div>
となるように、
<span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub> , 
・・・,
<span class="vector">a</span><sub>N</sub> </span>
を並べます。


### <a id="sec-generated-title-6"></a> <a id="conclusion"></a>まとめ

要するに、以下のような手順でベクトル列 <span class="math">X</span> を
ベクトル列 <span class="math">Y</span> に変換する手法を
<strong id="principal" class="keyword">主成分分析</strong>（principal component analysis）と呼びます。

* <span class="math">X</span>の「[相関行列](#correlation)」<span class="math">R<sub>X</sub></span>の固有値・固有ベクトルを求める。

* 求めた固有ベクトルを、固有値が大きい順に<span class="math"><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub> , 
・・・,
<span class="vector">a</span><sub>N</sub> </span>と並べる。 このとき、各固有ベクトルは絶対値を 1 に正規化しておく。 （<span class="math"><span class="normal">|</span><span class="vector">a</span><sub>i</sub><span class="normal">|</span> ＝ 1</span>）

* <span class="math">X</span>を、行列<span class="math">
A
＝
<span class="paren" style="font-size:em;">(</span><span class="vector">a</span><sub>1</sub> , <span class="vector">a</span><sub>2</sub> , 
・・・,
<span class="vector">a</span><sub>N</sub><span class="paren" style="font-size:em;">)</span></span>を使って変換する。<span class="math">Y ＝ X A</span>。


このとき、以下のことが成り立ちます。

* <span class="math">A</span>は直交変換。 すなわち、<span class="math">i ≠ j</span>のとき、<span class="math"><span class="vector">a</span><sub>i</sub><sup>T</sup><span class="vector">a</span><sub>j</sub> ＝ 0</span>。 また、逆変換は<span class="math">X ＝ Y A<sup>T</sup></span>と表せる。

* <span class="math">Y</span>の各列<span class="math"><span class="vector">y</span><sub>i</sub></span>は互いに直交。 すなわち、<span class="math">i ≠ j</span>のとき、<span class="math"><span class="paren" style="font-size:em;">(</span><span class="vector">y</span><sub>i</sub> , <span class="vector">y</span><sub>j</sub><span class="paren" style="font-size:em;">)</span>
＝ 0</span>。

* <span class="math">
            <span class="vector">y</span>
            <sub>i</sub>
          </span>はベクトル列<span class="math">X</span>の第 i 主成分。


ちなみに、ベクトル列 <span class="math">X</span> が互いに強い相関を持っているような場合、
<span class="math"><span class="vector">y</span><sub>i</sub></span> の絶対値は、
極端に第一主成分 <span class="math"><span class="vector">y</span><sub>1</sub></span> に偏ります。
逆に、<span class="math">X</span> が互いに弱い相関を持つ場合、
<span class="math"><span class="vector">y</span><sub>i</sub></span> の絶対値には偏りが少なくなります。
 
また、<span class="math">N</span> 個のベクトルのうち、
線形独立なベクトルが <span class="math">M</span> （<span class="math">M ＜ N</span>）本しかない場合、
<span class="math"><span class="vector">y</span><sub>i</sub></span> は <span class="math">M</span> 個目までが非0で、
残りは零ベクトルになります。


## <a id="sec-generated-title-7"></a> <a id="probability"></a>確率変数の主成分分析

主成分分析の考え方は、
内積、あるいは、内積に類する性質の演算を定義できる線形空間なら何にでも適用できます。
 
その一例として、確率変数の主成分分析について少し触れておきます。
ベクトルの内積の代わりに、確率変数に対しては共分散を用います。
（ベクトルの直交性は、確率変数の場合は相関性にあたる。直交 ＝ 無相関。）
2つの確率変数 <span class="math">x</span>, <span class="math">y</span> の共分散
<div class="math">
      <span class="normal">Cov</span>
      <span class="paren" style="font-size:em;">[</span>x, y<span class="paren" style="font-size:em;">]</span>
＝
<span class="normal">E</span><span class="paren" style="font-size:em;">[</span>xy<span class="paren" style="font-size:em;">]</span>
－
<span class="normal">E</span><span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span><span class="normal">E</span><span class="paren" style="font-size:em;">[</span>y<span class="paren" style="font-size:em;">]</span></div>
（<span class="math"><span class="normal">E</span><span class="paren" style="font-size:em;">[</span>x<span class="paren" style="font-size:em;">]</span></span> は <span class="math">x</span> の期待値。）
を用いて、
N 個の確率変数の列
<span class="math">
x<sub>1</sub> , 
x<sub>2</sub> , 
・・・,
x<sub>N</sub></span>
の共分散行列 <span class="math">C<sub>X</sub></span> を以下のように定義します。
<div class="math">
      <span class="math">C<sub>X</sub></span>
＝
<span class="paren" style="font-size:em;">{</span><span class="normal">Cov</span><span class="paren" style="font-size:em;">[</span>x<sub>i</sub> , x<sub>j</sub><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">}</span></div>
そして、
共分散行列 <span class="math">C<sub>X</sub></span> の固有値問題を解くことで、
前節で述べたベクトルの主成分分析と同様の議論が可能です。
このような手法を確率変数の主成分分析といいます。
