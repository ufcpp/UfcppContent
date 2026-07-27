---
title: "チェビシェフフィルタ"
source_url: "https://ufcpp.net/study/sp/digital_filter/chebyshev/"
content_type: "Article"
published_at: "2004-04-07T00:00:00"
updated_at: "2015-05-06T14:22:44"
tags: []
umbraco_id: 1619
parent_id: 1610
sort_order: 8
aliases:
  - "/study/digital_filter/chebyshev.html"
---

# チェビシェフフィルタ

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="chebyshev" class="keyword">チェビシェフフィルタ</strong>（Chebyshev filter）は、
通過域で等リプルとなるようなローパスフィルタです。
リプルを許容することで急峻なカットオフ特性を得ることができます。

<figure>

[![リプルの許容](../../../../assets/media/ufcpp2000/sp/chebyshev01.png)](../../../../assets/media/ufcpp2000/sp/chebyshev01.png)

<figcaption>リプルの許容</figcaption>
</figure>



## <a id="sec-generated-title-2"></a> <a id="idea"></a>基本アイディア

「[バターワースフィルタ](butterworth.md#butterworth)」は通過域で平坦な周波数特性を示すローパスフィルタでした。
これに対して、通過域での平坦性を犠牲にし、リプルを許容することによって、
少ない誤差および急峻なカットオフ特性を持つフィルタを得ることができます。

このようなフィルタの設計手法について説明する前に
まず、バターワースフィルタの振幅特性式を以下のように一般化して考えてみます。
<div class="math">
      <span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><sup>2</sup></td></tr></table></div>
<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝ ω<sup>n</sup></span>の場合がバターワースフィルタにあたります。
バターワースフィルタでは、
<span class="math">ω<sup>n</sup></span>が<span class="math">ω＝1</span>を境にして急激に増加するため、
フィルタの振幅特性<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ω<sup>2n</sup></td></tr></table></span>は
<span class="math">ω＜1</span>のとき1に近い値を、
<span class="math">ω＞1</span>のとき0に近い値を取りました。
これと同様に、
<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>として、
<span class="math">ω＜1</span>のときに極力小さい値を、
<span class="math">ω＞1</span>のときに極力大きい値を持つような多項式を選べば、
<span class="math"><span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup></span>がローパス特性になります。


## <a id="sec-generated-title-3"></a> <a id="cheb-poly"></a>チェビシェフ多項式

前節で述べたローパス特性において、
<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>として、
<span class="math">ω＜1</span>の範囲での最大値が最も小さくなるような多項式を選ぶことで、
<span class="math"><span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup></span>の通過域（<span class="math">ω＜1</span>）における誤差を最小にすることができます。
すなわち、最高次の係数が1であるようなn次多項式<span class="math">p<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>のうちで、
<span class="math"><table class="sigma" summary="statement under a function"><tr><td><span class="normal">max</span></td></tr><tr><td class="sigmasub">x＜1</td></tr></table>
p<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
となるようなものを選び、
<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝ a p<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>（<span class="math">a</span>は定数）とします。
 
このような性質をもつ多項式として、<strong id="chebyshev-poly" class="keyword">チェビシェフ多項式</strong>（Chebyshev polynomial）というものがあります。
n次のチェビシェフ多項式<span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>は、
<span class="math">x ≦ 1</span>に対して以下のように定義されます。
<em>
      <div class="math">
C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">cos</span><sup>－1</sup> x<span class="paren" style="font-size:em;">)</span></div>
    </em>
この式は一見すると複雑に見えますが、
<span class="math">x ＝ <span class="normal">cos</span>θ</span>とおくと、
<div class="math">
C<sub>n＋1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＋
C<sub>n－1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cos</span><span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>n ＋ 1<span class="paren" style="font-size:em;">)</span>θ<span class="paren" style="font-size:em;">)</span>
＋
<span class="normal">cos</span><span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>n － 1<span class="paren" style="font-size:em;">)</span>θ<span class="paren" style="font-size:em;">)</span></div><div class="math">　
＝
2
<span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nθ<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>θ<span class="paren" style="font-size:em;">)</span>
＝
2 x <span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">cos</span><sup>－1</sup> x<span class="paren" style="font-size:em;">)</span>
＝
2 x C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></div>
となるので、以下の漸化式で表されるn次の多項式になります。
<em>
      <div class="math">
C<sub>n＋1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
2 x C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
－
C<sub>n－1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></div>
      <div class="math">
C<sub>1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
x
</div>
      <div class="math">
C<sub>0</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
1
</div>
    </em>
<span class="math">x ＞ 1</span>に対しては、
<div class="math">
C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cosh</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">cosh</span><sup>－1</sup> x<span class="paren" style="font-size:em;">)</span></div>
と定義することでまったく同じ漸化式が得られます。
 
チェビシェフ多項式<span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>は、
先ほど述べた
<span class="math"><table class="sigma" summary="statement under a function"><tr><td><span class="normal">max</span></td></tr><tr><td class="sigmasub">x＜1</td></tr></table>
p<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
を満たすようなn次多項式<span class="math">p<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>を<span class="math">2<sup>n － 1</sup></span>倍したものになります。
また、定義から分かるように、
チェビシェフ多項式は<span class="math">x ≦ 1</span>のとき、<span class="math">－1 ≦ C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ≦ 1</span>の範囲で振動し、<span class="math">x ＞ 1</span>のとき、単調増加します。
以下に2次～7次までのチェビシェフ多項式の係数およびグラフを示します。
<div class="math">
C<sub>2</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
2 x<sup>2</sup> － 1
</div><div class="math">
C<sub>3</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
4 x<sup>3</sup> － 3 x
</div><div class="math">
C<sub>4</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
8 x<sup>4</sup> － 8 x<sup>2</sup> ＋ 1
</div><div class="math">
C<sub>5</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
16 x<sup>5</sup> － 20 x<sup>3</sup> ＋ 5 x
</div><div class="math">
C<sub>6</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
32 x<sup>6</sup> － 48 x<sup>4</sup> ＋ 18 x<sup>2</sup> － 1
</div><div class="math">
C<sub>7</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
64 x<sup>7</sup> － 112 x<sup>5</sup> ＋ 56 x<sup>3</sup> － 7 x
</div>
<figure>

[![チェビシェフ多項式のグラフ](../../../../assets/media/ufcpp2000/sp/chebyshev03.png)](../../../../assets/media/ufcpp2000/sp/chebyshev03.png)

<figcaption>チェビシェフ多項式のグラフ</figcaption>
</figure>


この式を見ると、
<span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
の最高次の係数は <span class="math">2<sup>n － 1</sup></span> になっていることが分かります。
すなわち、<span class="math">x</span> の値が大きいとき、
<span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
の増加率は、
<span class="math">x<sup>n</sup></span>
の <span class="math">2<sup>n － 1</sup></span> になります。


## <a id="sec-generated-title-4"></a> <a id="property"></a>周波数特性

「[基本アイディア](#idea)」で述べたローパス特性の式中の<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>を、チェビシェフ多項式<span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>と任意定数<span class="math">ε</span>を用いて
<span class="math">F<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span> ＝ ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>
と置いたものを<em>チェビシェフフィルタ</em>と呼びます。
 
すなわち、チェビチェフフィルタは以下のような周波数特性を持つローパスフィルタです。
<div class="math">
      <span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table></div>
チェビシェフ多項式の性質から、
この特性は<span class="math">ω ≦ 1</span>のとき<span class="math">1 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ε<sup>2</sup></td></tr></table></span>の間で振動し、
<span class="math">ω＞1</span>のときバターワースフィルタの<span class="math">ε 2<sup>n－1</sup></span>倍のペースで急激に減衰していきます。
<span class="math">ε</span>の値が小さければ、リプルが大きいい代わりに急峻な減衰を示し、
逆に、<span class="math">ε</span>の値が大きければ、リプルが小さく、減衰が緩やかになります。
また、リプルの高さが<span class="math">1 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ε<sup>2</sup></td></tr></table></span>の間で一定なので、
<em>等リプルフィルタ</em>（equiripple filter）とも呼ばれています。
 
図3に、例として、3次、5次、9次のチェビシェフフィルタの振幅特性を示します。
この例では、リプル幅は 0.1 で設計しています。

<figure>

[![3～9次のチェビシェフフィルタの周波数特性](../../../../assets/media/ufcpp2000/sp/chebyshev_amp.png)](../../../../assets/media/ufcpp2000/sp/chebyshev_amp.png)

<figcaption>3～9次のチェビシェフフィルタの周波数特性</figcaption>
</figure>



## <a id="sec-generated-title-5"></a> <a id="analog"></a>アナログプロトタイプの設計

### <a id="sec-generated-title-6"></a> <a id="zp"></a>極配置

バターワースフィルタのときと同様に、
<span class="math"><span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup></span>の分母は<span class="math">2n</span>次の多項式であり、
<span class="math">2n</span>個の極の中から安定な<span class="math">n</span>個の極を選び、
<span class="math">H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>の極にすることでフィルタを設計します。

<span class="math">
          <span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
          <sup>2</sup>
        </span>の極は
<div class="math">
        <span class="paren" style="font-size:2em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">)</span>
        <sup>2</sup> ＝ －1
</div>
すなわち、
<div class="math">
        <span class="normal">cos</span> n <span class="normal">cos</span><sup>－1</sup>ω ＝ ±i <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></div>
を満たします。
この式を満たす<span class="math">ω</span>を求めるために、
まず、<span class="math"><span class="normal">cos</span>ω ＝ w ＝ u ＋ i v</span> と置き、
<span class="math">u, v</span> に付いて解きます。
<span class="math"><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>x ＋ i y<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cos</span>x <span class="normal">cosh</span>y － i <span class="normal">sin</span>x <span class="normal">sinh</span>y
</span>
となるので、
上式は以下のように表すことができます。
<div class="math">
        <span class="paren" style="font-size:3em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">
              <span class="normal">cos</span> n u <span class="normal">cosh</span> n v ＝ 0</span>  </td><td><span class="paren">(</span><span class="math"></span><span class="paren">)</span></td></tr><tr><td><span class="math">
              <span class="normal">sin</span> n u <span class="normal">sinh</span> n v ＝ ±<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></span>  </td><td><span class="paren">(</span><span class="math"></span><span class="paren">)</span></td></tr></table>
      </div>
第一式において、<span class="math"><span class="normal">cosh</span> n v</span>は常に<span class="math"><span class="normal">cosh</span> n v ＞ 1</span>を満たすので、<span class="math"><span class="normal">cos</span> n u ＝ 0</span>になり、これを解くことで、
<div class="math">
u = <table class="frac" summary="fraction"><tr><td class="num">2 k ＋ 1</td></tr><tr><td>2 n</td></tr></table>π
</div>
が得られます。（ただし、<span class="math">k</span>は<span class="math">0 ～ 2 n － 1</span>の範囲の整数。）
このとき、<span class="math"><span class="normal">sin</span> n u ＝ ±1</span>であり、
<span class="math"><span class="normal">sinh</span> n v ＝ ±<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></span>となります。
これを解くことにより、
<div class="math">
v = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="normal">sinh</span><sup>－1</sup><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
が得られます。
 
以上のことから、<span class="math">ω</span>は
<div class="math">
ω ＝ <span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>u ＋ i v<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cosh</span> v <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">2 k ＋ 1</td></tr><tr><td>2 n</td></tr></table>π
± i
<span class="normal">sinh</span> v <span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">2 k ＋ 1</td></tr><tr><td>2 n</td></tr></table>π
</div>
となります。
 
この中から安定極のみを選ぶことで、伝達関数の極 <span class="math">s ＝ σ ＋ i ω</span> は以下のようになります。
<em>
        <div class="math">
s ＝ σ ＋ i ω
＝
－
<span class="normal">sinh</span> v <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π
± i
<span class="normal">cosh</span> v <span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π
</div>
      </em>
ただし、<span class="math">k</span>は<span class="math">0～(n－1) / 2</span>までの整数です。
 
三角関数の性質から、
<div class="math">
        <span class="paren" style="font-size:2em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">σ</td></tr><tr><td>
              <span class="normal">sinh</span> v</td></tr></table>
        <span class="paren" style="font-size:2em;">)</span>
        <sup>2</sup>
＋
<span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">ω</td></tr><tr><td><span class="normal">cosh</span> v</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>2</sup>
＝
1
</div>
となります。
すなわち、チェビシェフフィルタの極は複素数平面の楕円上に分布しています。


### <a id="sec-generated-title-7"></a> <a id="aptf"></a>アナログプロトタイプ伝達関数

決定された極配置から、チェビシェフフィルタの伝達関数<span class="math">H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>は以下のようになります。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table> － 1</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">β<sub>k</sub></td></tr><tr><td>s<sup>2</sup> ＋ α<sub>k</sub> s ＋ β<sub>k</sub></td></tr></table>
　　　<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">β</td></tr><tr><td>s ＋ β</td></tr></table><table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝1</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">β<sub>k</sub></td></tr><tr><td>s<sup>2</sup> ＋ α<sub>k</sub> s ＋ β<sub>k</sub></td></tr></table>
　　　<span class="normal">（nが奇数のとき）</span></div>
ただし、<span class="math">α<sub>k</sub>, β<sub>k</sub></span> は以下の式で表される値です。
<div class="math">
β
＝
<span class="normal">sinh</span> v
＝
<span class="normal">sinh</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table><span class="normal">sinh</span><sup>－1</sup><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="paren" style="font-size:2em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">t<sup>2</sup> － 1</td></tr><tr><td>2 t</td></tr></table>
,
t ＝ <span class="paren" style="font-size:2.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">1＋<span class="paren" style="font-size:em;">(</span>ε<sup>2</sup> ＋ 1<span class="paren" style="font-size:em;">)</span><sup>1/2</sup></td></tr><tr><td>ε</td></tr></table><span class="paren" style="font-size:2.5em;">)</span><sup>1/n</sup></div><div class="math">
α<sub>k</sub>
＝
2 β
<span class="normal">cos</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π<span class="paren" style="font-size:1.5em;">)</span></div><div class="math">
β<sub>k</sub>
＝
β<table class="subsup" summary="sub / sup"><tr><td>2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td></td></tr></table>
＋
<span class="normal">sin</span><sup>2</sup><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π<span class="paren" style="font-size:1.5em;">)</span></div>

### <a id="sec-generated-title-8"></a> <a id="spec"></a>設計仕様

透過域/阻止域の周波数/リプル
（<span class="math">A<sub>p</sub>, r<sub>s</sub>, ω<sub>s</sub></span>）
を仕様として与えたとき、
仕様を満たす最小の次数 <span class="math">N</span> とパラメータ <span class="math">ε</span> を求める方法について説明します。
（<span class="math">ω<sub>p</sub></span> は 1 で固定。）

<figure>

[![チェビシェフフィルタの設計仕様](../../../../assets/media/ufcpp2000/sp/chebyshev_spec.png)](../../../../assets/media/ufcpp2000/sp/chebyshev_spec.png)

<figcaption>チェビシェフフィルタの設計仕様</figcaption>
</figure>


まず、透過域リプルから <span class="math">ε</span> を決定します。
図4に示すように、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
1 ＋ ε<sup>2</sup></td></tr></table>
＝
A<sub>p</sub><sup>2</sup></span>
なので、
<em>
        <div class="math">
ε
＝
<span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>A<sub>p</sub><sup>2</sup></td></tr></table>
－
1
</span></div>
      </em>
が得られます。
 
また、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
1
＋
<span class="paren" style="font-size:1.5em;">(</span>
ε C<sub>N</sub><span class="paren" style="font-size:em;">(</span>ω<sub>s</sub><span class="paren" style="font-size:em;">)</span><sup>2</sup><span class="paren" style="font-size:1.5em;">)</span></td></tr></table>
≦
r<sub>s</sub><sup>2</sup></span>
より、
<span class="math">
C<sub>N</sub><span class="paren" style="font-size:em;">(</span>ω<sub>s</sub><span class="paren" style="font-size:em;">)</span>
≧
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>r<sub>s</sub><sup>2</sup></td></tr></table> － 1
  </span></td></tr><tr><td>
  ε
 </td></tr></table></span>
が得られます。
これに、チェビシェフ多項式の定義
<span class="math">
C<sub>n</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>n <span class="normal">cos</span><sup>－1</sup> x<span class="paren" style="font-size:em;">)</span></span>
を代入することで、
<em>
        <div class="math">
N
≧
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">cosh</span><sup>－1</sup><table class="frac" summary="fraction"><tr><td class="num"><span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>r<sub>s</sub><sup>2</sup></td></tr></table> － 1
  </span></td></tr><tr><td>
  ε
 </td></tr></table></td></tr><tr><td><span class="normal">cosh</span><sup>－1</sup>
  ω<sub>s</sub></td></tr></table></div>
      </em>
が得られます。
この式を満たすような最小の N を選びます。


## <a id="sec-generated-title-9"></a> <a id="digital"></a>ディジタルフィルタ設計

「[アナログプロトタイプの設計](#analog)」で得られた式を、
カットオフ周波数<span class="math">ω<sub>c</sub></span>で双1次変換することで以下の式が得られる。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝1</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
b<sub>k</sub><span class="paren" style="font-size:em;">(</span>
1 ＋ 2 z<sup>－1</sup> ＋ z<sup>－2</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td>
a<sub>k,0</sub>
＋
a<sub>k,1</sub>
z<sup>－1</sup>
＋
a<sub>k,2</sub>
z<sup>－2</sup></td></tr></table>
　　
<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">
b<sub>0</sub><span class="paren" style="font-size:em;">(</span>
1 ＋ z<sup>－1</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td>
a<sub>0,0</sub>
＋
a<sub>0,1</sub>
z<sup>－1</sup></td></tr></table><table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝1</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
b<sub>k</sub><span class="paren" style="font-size:em;">(</span>
1 ＋ 2 z<sup>－1</sup> ＋ z<sup>－2</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td>
a<sub>k,0</sub>
＋
a<sub>k,1</sub>
z<sup>－1</sup>
＋
a<sub>k,2</sub>
z<sup>－2</sup></td></tr></table>
　　
<span class="normal">（nが奇数のとき）</span></div>
ただし、<span class="math">a<sub>k,0</sub> , a<sub>k,1</sub> , a<sub>k,2</sub> , b</span> は以下の式で表される値です。 
<div class="math">
c ＝ <span class="normal">cos</span> ω<sub>c</sub> , 
s ＝ <span class="normal">sin</span> ω<sub>c</sub></div><div class="math">
a<sub>0,0</sub>
＝
β
s
＋
<span class="paren" style="font-size:em;">(</span>1 ＋ c<span class="paren" style="font-size:em;">)</span></div><div class="math">
a<sub>0,1</sub>
＝
β
s
－
<span class="paren" style="font-size:em;">(</span>1 ＋ c<span class="paren" style="font-size:em;">)</span></div><div class="math">
b<sub>0</sub>
＝
β
s
</div><div class="math">
a<sub>k,0</sub>
＝
<span class="paren" style="font-size:em;">(</span>1 ＋ c<span class="paren" style="font-size:em;">)</span>
＋
β<sub>k</sub><span class="paren" style="font-size:em;">(</span>1 － c<span class="paren" style="font-size:em;">)</span>
＋
α<sub>k</sub> s
</div><div class="math">
a<sub>k,1</sub>
＝
2
<span class="paren" style="font-size:1.5em;">(</span><span class="paren" style="font-size:em;">(</span>1 ＋ c<span class="paren" style="font-size:em;">)</span>
 －
 β<sub>k</sub><span class="paren" style="font-size:em;">(</span>1 － c<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></div><div class="math">
a<sub>k,2</sub>
＝
 <span class="paren" style="font-size:em;">(</span>1 ＋ c<span class="paren" style="font-size:em;">)</span>
 ＋
 β<sub>k</sub><span class="paren" style="font-size:em;">(</span>1 － c<span class="paren" style="font-size:em;">)</span>
 －
 α<sub>k</sub> s
</div><div class="math">
b<sub>k</sub>
＝
β<sub>k</sub><span class="paren" style="font-size:em;">(</span>1－c<span class="paren" style="font-size:em;">)</span></div>
