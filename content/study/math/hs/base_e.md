---
title: "自然対数の底"
source_url: "https://ufcpp.net/study/math/hs/base_e/"
content_type: "Article"
published_at: "2007-07-14T00:00:00"
updated_at: "2015-05-06T14:16:21"
tags: []
umbraco_id: 1455
parent_id: 1445
sort_order: 9
aliases:
  - "/hs/base_e"
  - "/hs/base_e.html"
  - "/math/hs/base_e/"
  - "/study/hs/base_e"
  - "/study/hs/base_e.html"
---

# 自然対数の底

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

自然対数の底 e に関して。
 
e は、
オイラー数（世紀の大数学者 Leonhard Euler の名前から）とかネイピア数（対数表を作った人、 John Napier の名前から）とか呼ばれる場合もあります。
（オイラーの業績は多岐にわたりすぎていて、
「オイラー数」っていうと「どのオイラー数だよ」ってことになるんで、
ネイピア数の方が一般的です。
というか、「自然対数の底」って呼ぶ方が一般的ですけど。）


## <a id="sec-generated-title-2"></a> <a id="definition"></a>定義

自然対数の底 e は、
以下のいずれかで定義されることが多いです。
（どちらの定義でも出てくる結論は同じ。）

1. <span class="math">
          <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>x</td></tr></table>
          <span class="normal">log</span>
          <sub>
            <span class="normal">e</span>
          </sub> x <span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table></span>となるような定数 e を自然対数の底と呼ぶ。

2. <span class="math">
          <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>x</td></tr></table>
          <span class="normal">e</span>
          <sup>x</sup>
          <span class="normal">=</span>
          <span class="normal">e</span>
          <sup>x</sup>
        </span>となるような定数 e を自然対数の底と呼ぶ。


1. の方の、対数の方の定義の方が説明が簡単なので、
対数の方を出発点にします。
 
微分の定義に立ち返って、底がある定数 e の 対数 <span class="math"><span class="normal">log</span><sub><span class="normal">e</span></sub></span> を微分してみると以下のようになります。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>x</td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">h <span class="normal">→</span><span class="normal">0</span></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">log</span>
          <sub>
            <span class="normal">e</span>
          </sub>
          <span class="paren" style="font-size:em;">(</span>x <span class="normal">+</span> h<span class="paren" style="font-size:em;">)</span>
          <span class="normal">−</span>
          <span class="normal">log</span>
          <sub>
            <span class="normal">e</span>
          </sub> x
 </td></tr><tr><td>h</td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">h <span class="normal">→</span><span class="normal">0</span></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>h</td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="paren" style="font-size:2em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">x <span class="normal">+</span> h</td></tr><tr><td>x</td></tr></table>
      <span class="paren" style="font-size:2em;">)</span>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">h <span class="normal">→</span><span class="normal">0</span></td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="paren" style="font-size:2em;">(</span>
        <span class="normal">1</span>
        <span class="normal">+</span>
        <table class="frac" summary="fraction"><tr><td class="num">h</td></tr><tr><td>x</td></tr></table>
      <span class="paren" style="font-size:2em;">)</span>
      <sup>
        <table class="frac" summary="fraction"><tr><td class="num">x</td></tr><tr><td>h</td></tr></table>
      </sup>
    </div>
lim 中の <span class="math"><table class="frac" summary="fraction"><tr><td class="num">h</td></tr><tr><td>x</td></tr></table></span> の部分を <span class="math">t</span> とでも置いて、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>x</td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">t <span class="normal">→</span><span class="normal">0</span></td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="paren" style="font-size:2em;">(</span>
        <span class="normal">1</span>
        <span class="normal">+</span> t
<span class="paren" style="font-size:2em;">)</span>
      <sup>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>t</td></tr></table>
      </sup>
    </div>
とするか、
逆数にして、
<span class="math">n <span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>t</td></tr></table></span> として、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>x</td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n <span class="normal">→</span><span class="normal">∞</span></td></tr></table>
      <span class="normal">log</span>
      <sub>
        <span class="normal">e</span>
      </sub>
      <span class="paren" style="font-size:2em;">(</span>
        <span class="normal">1</span>
        <span class="normal">+</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table>
      <span class="paren" style="font-size:2em;">)</span>
      <sup>
n
</sup>
    </div>
とします。
ここで、
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>x</td></tr></table><span class="normal">log</span><sub><span class="normal">e</span></sub> x <span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table></span>
が定数 e の定義なので、
こうなるためには、結局、
<div class="math">
      <span class="normal">e</span>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n <span class="normal">→</span><span class="normal">∞</span></td></tr></table>
      <span class="paren" style="font-size:2em;">(</span>
        <span class="normal">1</span>
        <span class="normal">+</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table>
      <span class="paren" style="font-size:2em;">)</span>
      <sup>
n
</sup>
    </div>
となります。


## <a id="sec-generated-title-3"></a> <a id="convergence"></a>e の収束性

さて、ここまでは教科書通りです。
ところで、高校の教科書の e に関する記述には少しごまかしがあります。
教科書曰く、

<blockquote markdown="1">
<span class="math">
          <span class="normal">e</span>
          <span class="normal">=</span>
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n <span class="normal">→</span><span class="normal">∞</span></td></tr></table>
          <span class="paren" style="font-size:2em;">(</span>
            <span class="normal">1</span>
            <span class="normal">+</span>
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table>
          <span class="paren" style="font-size:2em;">)</span>
          <sup>
n
</sup>
        </span>
は収束する。

</blockquote>
となっています。
要するに、e の収束性は既知のものとなっています。
 
まあ、級数の収束性の判定は、
正確にやるためには
「[ε－δ論法](../infinity/epsilon.md#epsilon-delta)」が必要で、
高校の範囲では説明のしようがなかったりもするんですが。
 
結局、ここでも少しごまかしごまかし説明することになるんですが、
数列の収束性判定法として、以下のようなものが知られています。
（これの証明にも「[ε－δ論法](../infinity/epsilon.md#epsilon-delta)」が必要。
ここではこれ以上踏み込むのは避けます。）

<blockquote markdown="1">
有界単調増加列は収束する。

</blockquote>
有界単調増加列というのは、
値に上限があって、かつ、単調増加な数列のことで、
もう少し正確に書くと以下のようになります。

<blockquote markdown="1">
以下のような条件を満たす数列 <span class="math">a<sub>n</sub></span> を有界単調増加列と呼ぶ。

1. すべての自然数<span class="math">n</span>について、<span class="math">a<sub>n</sub><span class="normal">&lt;</span> M</span>となるような実数<span class="math">M</span>が存在する。

2. すべての自然数<span class="math">n</span>について、<span class="math">a<sub>n</sub><span class="normal">&lt;</span> a<sub>n <span class="normal">+</span><span class="normal">1</span></sub></span>が成り立つ。


</blockquote>
ということで、ここでは、
e の定義中に出てくる数列
<span class="math">
e<sub>n</sub><span class="normal">=</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">+</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>
n
</sup></span>
の有界性と単調増加性についての話をします。


### <a id="sec-generated-title-4"></a> <a id="increase"></a>単調増加性

数列
<span class="math">
e<sub>n</sub><span class="normal">=</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">+</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>
n
</sup></span>
は、2項展開することで、以下のように書き表すことができます。
<div class="math">
e<sub>n</sub><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><sub>n</sub>C<sub>k</sub><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>k</sup></div>
ただし、
組み合わせの数は高校流に 
<span class="math"><sub>n</sub>C<sub>k</sub></span> 
で表すものとします。
<div class="math">
        <sub>n</sub>C<sub>k</sub><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">n<span class="normal">!</span></td></tr><tr><td>
  k<span class="normal">!</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">−</span> k<span class="paren" style="font-size:em;">)</span><span class="normal">!</span></td></tr></table><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">
  n
  <span class="normal">⋅</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">−</span> k <span class="normal">+</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="normal">1</span><span class="normal">⋅</span><span class="normal">2</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span>
  k
 </td></tr></table></div>
<span class="math">
e<sub>n</sub><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><sub>n</sub>C<sub>k</sub><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>k</sup></span>
という式中の
<span class="math"><sub>n</sub>C<sub>k</sub></span> 
を展開してしまうと以下のようになります。
<div class="math">
e<sub>n</sub><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">1</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">k <span class="normal">−</span><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
ここで、
<span class="math">
e<sub>n</sub></span>
と
<span class="math">
e<sub>n<span class="normal">+</span><span class="normal">1</span></sub></span>
の大小を比べてみましょう。
<div class="math">
e<sub>n<span class="normal">+</span><span class="normal">1</span></sub><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n<span class="normal">+</span><span class="normal">1</span></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">1</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">k <span class="normal">−</span><span class="normal">1</span></td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
なわけですが、まず、∑の中身は正なので、
1項削ると値が小さくなります。
よって、以下の不等式が成り立ちます。
<div class="math">
e<sub>n<span class="normal">+</span><span class="normal">1</span></sub><span class="normal">&gt;</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">1</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">k <span class="normal">−</span><span class="normal">1</span></td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
また、
任意の正の数 <span class="math">m</span> に対して、
<span class="math"><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">m</td></tr><tr><td>n</td></tr></table><span class="normal">&lt;</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">m</td></tr><tr><td>n<span class="normal">+</span><span class="normal">1</span></td></tr></table></span>
が成り立つので、
<div class="math">
e<sub>n<span class="normal">+</span><span class="normal">1</span></sub><span class="normal">&gt;</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">1</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">k <span class="normal">−</span><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">=</span>
e<sub>n</sub></div>
が成り立ち、
<span class="math">
e<sub>n</sub></span>
の単調増加性が示されます。


### <a id="sec-generated-title-5"></a> <a id="bounded"></a>有界性

次は有界性に関してですが、
これもまた、
以下の式から出発して考えてみます。
<div class="math">
e<sub>n</sub><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">1</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">k <span class="normal">−</span><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
<span class="math">n</span> 以下の自然数 <span class="math">m</span> に関して、
<span class="math"><span class="normal">0</span><span class="normal">&lt;</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">m</td></tr><tr><td>n</td></tr></table><span class="normal">&lt;</span><span class="normal">1</span></span>
なので、
<div class="math">
e<sub>n</sub><span class="normal">&lt;</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table></div>
となります。
また、
1 以上の整数 <span class="math">k</span> に関して、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">&lt;</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span><sup>k <span class="normal">−</span><span class="normal">1</span></sup></td></tr></table></span>
が成り立つので、
<div class="math">
e<sub>n</sub><span class="normal">&lt;</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">&lt;</span><span class="normal">1</span><span class="normal">+</span><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">1</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span><sup>k <span class="normal">−</span><span class="normal">1</span></sup></td></tr></table><span class="normal">&lt;</span><span class="normal">3</span></div>
が成り立ち、
有界性が示されます。


### <a id="sec-generated-title-6"></a> <a id="value"></a>収束値

以上で、e の収束性が示されました。
といっても、具体的な値が何になるかは分かりません。
 
でも、
重要なのは「何らかの値に収束する」ということ自体であって、
具体的な値はさほど重要ではありません。
e の収束性さえ分かれば指数・対数の微分・積分に関する議論が可能ですし、
実用上も、e の値が 2 と 3 の間くらいにあるということが分かるだけで割と色々な結果が得られます。
 
まあ、「収束性自体が重要」と前置きしつつも、
具体的な値に関する話をしたいと思います。

<span class="math">
e<sub>n</sub></span>
は以下のように表すことができました。
<div class="math">
        <span class="normal">e</span>
        <span class="normal">=</span>
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n <span class="normal">→</span><span class="normal">∞</span></td></tr></table>
e<sub>n</sub><span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n <span class="normal">→</span><span class="normal">∞</span></td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table><span class="normal">1</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">⋅</span><span class="normal">⋯</span><span class="normal">⋅</span><span class="paren" style="font-size:2em;">(</span><span class="normal">1</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">k <span class="normal">−</span><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
さて、この式で、<span class="math">n <span class="normal">→</span><span class="normal">∞</span></span>
とすると、
以下の式に収束しそうだと予想が付くと思います。
<div class="math">
        <span class="normal">e</span>
        <span class="normal">=</span>
        <table class="sigma" summary="sum"><tr><td class="sigmasub">
            <span class="normal">∞</span>
          </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table>
      </div>
厳密には、
<span class="math">n <span class="normal">→</span><span class="normal">∞</span></span> としたときにちゃんとこの値に収束するのかとか、
<span class="math"><table class="sigma" summary="sum"><tr><td class="sigmasub"><span class="normal">∞</span></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">0</span></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table></span>
という級数自体の収束性とか、
考えなければいけないことは残っているんで、
高校の知識でこの予想の成否を示すことはできません。
ただ、まあ、細かい話抜きで結果だけ言うと、
この予想は合っています。
 
e の具体的な値の 2.718281828… というのは、
この式を使って計算できます。
階乗関数は極めて早く値が大きくなる
（なので <span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k<span class="normal">!</span></td></tr></table></span> は極めて早く 0 に近づく）ので、
この級数は収束の早い（＝ 数値計算向きな）級数です。
（2.718281828 程度の精度でよければ、
<span class="math">k <span class="normal">=</span><span class="normal">12</span></span> くらいの項まで足せば十分。）
