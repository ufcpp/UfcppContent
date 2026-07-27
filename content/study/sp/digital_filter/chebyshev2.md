---
title: "逆チェビシェフフィルタ"
source_url: "https://ufcpp.net/study/sp/digital_filter/chebyshev2/"
content_type: "Article"
published_at: "2004-05-02T00:00:00"
updated_at: "2015-05-06T14:22:47"
tags: []
umbraco_id: 1620
parent_id: 1610
sort_order: 9
aliases:
  - "/study/digital_filter/chebyshev2.html"
---

# 逆チェビシェフフィルタ

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="chebyshev2" class="keyword">逆チェビシェフフィルタ</strong>（inverse Chebyshev filter）は、
「[チェビシェフフィルタ](chebyshev.md#chebyshev)」とは逆で、
阻止域で等リプルとなるようなローパスフィルタです。
 
チェビシェフフィルタのことをチェビシェフI型フィルタ、
逆チェビシェフフィルタのことをチェビシェフII型フィルタと呼ぶこともあります。

<figure>

[![阻止域に等リプル](../../../../assets/media/ufcpp2000/sp/chebyshev2_01.png)](../../../../assets/media/ufcpp2000/sp/chebyshev2_01.png)

<figcaption>阻止域に等リプル</figcaption>
</figure>



## <a id="sec-generated-title-2"></a> <a id="property"></a>周波数特性

チェビシェフ特性は以下のような特徴を持っていました。
<div class="math">
      <span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table></div>
このチェビシェフ多項式およびチェビシェフ特性の要点をまとめると、表1のようになります。

<table summary="チェビシェフ特性の要点">
	<caption>
		チェビシェフ特性の要点
	</caption>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
		<td markdown="1"><span class="math">
            <span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
            <sup>2</sup>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ≦ 1</span>のとき</td>
		<td markdown="1"><span class="math">n</span>個の零点を持っていて、<span class="math">－1 ～ 1</span>の間で振動</td>
		<td markdown="1"><span class="math">1 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ε<sup>2</sup></td></tr></table></span>の間で振動</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ＞ 1</span>のとき</td>
		<td markdown="1">急激に単調増加</td>
		<td markdown="1">急激に単調減少</td>
	</tr>
</table>


チェビシェフ特性に対して、
<span class="math">ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span>
を
<span class="math">1 / <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></span>
で置き換えた物が逆チェビシェフ特性です。
<div class="math">
      <span class="normal">|</span>H<sub>I</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:1.5em;">(</span>1 / <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table></div>
<span class="math">1 / <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></span>
および逆チェビシェフ特性の要点は表2のようになります。

<table summary="逆チェビシェフ特性の要点">
	<caption>
		逆チェビシェフ特性の要点
	</caption>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><span class="math">1 / <span class="paren" style="font-size:1.5em;">(</span>C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span></span></td>
		<td markdown="1"><span class="math">
            <span class="normal">|</span>H<sub>I</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
            <sup>2</sup>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ≦ 1</span>のとき</td>
		<td markdown="1">緩やかに単調増加</td>
		<td markdown="1">緩やかに単調減少(1に近い値)</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ＞ 1</span>のとき</td>
		<td markdown="1"><span class="math">n</span>個の極を持っていて、 絶対値が<span class="math">1 ～ ∞</span>の間で振動</td>
		<td markdown="1"><span class="math">0 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ 1/ε<sup>2</sup></td></tr></table></span>の間で振動</td>
	</tr>
</table>


ちなみに、
チェビシェフ特性を <span class="math">H<sub>C</sub></span>、
逆チェビシェフ特性を <span class="math">H<sub>I</sub></span> とすると、
両者の間には、
<span class="math"><span class="normal">|</span>H<sub>I</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup>
＝
1
－
<span class="normal">|</span>H<sub>C</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup></span>
という関係が成り立ちます。
すなわち、
「逆チェビシェフ特性(ローパス) ＝ 1 － チェビシェフ特性(ハイパス)」となっています。

逆チェビシェフ特性を持つようなフィルタを<em>逆チェビシェフフィルタ</em>と呼びます。
チェビシェフフィルタおよび逆チェビシェフフィルタは、
それぞれチェビシェフI型(Chebyshev type I)フィルタ、
チェビシェフII型(Chebyshev type II)フィルタと呼ぶこともあります。
 
以下の図に、例として、3次、5次、9次の逆チェビシェフフィルタの振幅特性を示します。
この例では、リプル幅は 0.1 で設計しています。

<figure>

[![3～9次のチェビシェフフィルタの周波数特性](../../../../assets/media/ufcpp2000/sp/chebyshev2_amp.png)](../../../../assets/media/ufcpp2000/sp/chebyshev2_amp.png)

<figcaption>3～9次のチェビシェフフィルタの周波数特性</figcaption>
</figure>



## <a id="sec-generated-title-3"></a> <a id="analog"></a>アナログプロトタイプの設計（阻止域周波数固定型）

### <a id="sec-generated-title-4"></a> <a id="zp"></a>零極配置

逆チェビシェフフィルタの極は、
チェビシェフフィルタの極の逆数になっています。
 
また、逆チェビシェフフィルタの零点は、
<span class="math">
C<sub>N</sub><span class="paren" style="font-size:em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ω</td></tr></table><span class="paren" style="font-size:em;">)</span> ＝ 0
</span>
の解となります。
この解は、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ω</td></tr></table>
＝
<span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">2k ＋ 1</td></tr><tr><td>2n</td></tr></table>
π
</div>
となります。
ラプラス変数 <span class="math">s ＝ i ω</span> で表すと、
伝達関数の零点 <span class="math">s</span> は以下のようになります。
<em>
        <div class="math">
          <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table>
＝
± i
<span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">n － 2k － 1</td></tr><tr><td>2n</td></tr></table>
π
</div>
      </em>
ただし、<span class="math">k</span>は<span class="math">0～(n－1) / 2</span>までの整数です。


### <a id="sec-generated-title-5"></a> <a id="aptf"></a>アナログプロトタイプ伝達関数

決定された極配置から、チェビシェフフィルタの伝達関数<span class="math">H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>は以下のようになります。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table> － 1</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">γ<sub>k</sub> s<sup>2</sup> ＋ 1</td></tr><tr><td>β<sub>k</sub> s<sup>2</sup> ＋ α<sub>k</sub> s ＋ 1</td></tr></table>
　　　<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>β s ＋ 1</td></tr></table><table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">γ<sub>k</sub> s<sup>2</sup> ＋ 1</td></tr><tr><td>β<sub>k</sub> s<sup>2</sup> ＋ α<sub>k</sub> s ＋ 1</td></tr></table>
　　　<span class="normal">（nが奇数のとき）</span></div>
ただし、<span class="math">α<sub>k</sub>, β<sub>k</sub>, γ<sub>k</sub></span> は以下の式で表される値です。
（<span class="math">α<sub>k</sub>, β<sub>k</sub></span> は（I 型）チェビシェフフィルタのものと同じ。）
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
<span class="normal">sin</span><sup>2</sup><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π<span class="paren" style="font-size:1.5em;">)</span></div><div class="math">
γ<sub>k</sub>
＝
<span class="normal">sin</span><sup>2</sup><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π<span class="paren" style="font-size:1.5em;">)</span></div>

### <a id="sec-generated-title-6"></a> <a id="spec"></a>設計仕様

透過域/阻止域の周波数/リプル
（<span class="math">A<sub>p</sub>, r<sub>s</sub>, ω<sub>p</sub></span>）
を仕様として与えたとき、
仕様を満たす最小の次数 <span class="math">N</span> とパラメータ <span class="math">ε</span> を求める方法について説明します。
（<span class="math">ω<sub>s</sub></span> は 1 で固定。）

<figure>

[![逆チェビシェフフィルタの設計仕様1](../../../../assets/media/ufcpp2000/sp/chebyshev2_spec.png)](../../../../assets/media/ufcpp2000/sp/chebyshev2_spec.png)

<figcaption>逆チェビシェフフィルタの設計仕様1</figcaption>
</figure>


まず、阻止域リプルから <span class="math">ε</span> を決定します。
図4に示すように、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num">
ε<sup>2</sup></td></tr><tr><td>
1 ＋ ε<sup>2</sup></td></tr></table>
＝
r<sub>s</sub><sup>2</sup></span>
なので、
<em>
        <div class="math">
ε
＝
<span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num">r<sub>s</sub><sup>2</sup></td></tr><tr><td>
1
－
r<sub>s</sub><sup>2</sup></td></tr></table></span></div>
      </em>
が得られます。
 
また、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num">
ε C<sub>N</sub><span class="paren" style="font-size:em;">(</span>1 / ω<sub>p</sub><span class="paren" style="font-size:em;">)</span><sup>2</sup></td></tr><tr><td>
1
＋
<span class="paren" style="font-size:1.5em;">(</span>
ε C<sub>N</sub><span class="paren" style="font-size:em;">(</span>1 / ω<sub>p</sub><span class="paren" style="font-size:em;">)</span><sup>2</sup><span class="paren" style="font-size:1.5em;">)</span></td></tr></table>
≦
A<sub>p</sub><sup>2</sup></span>
より、
<span class="math">
C<sub>N</sub><span class="paren" style="font-size:em;">(</span>1 / ω<sub>p</sub><span class="paren" style="font-size:em;">)</span>
≧
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num">
  A<sub>p</sub><sup>2</sup></td></tr><tr><td>
  1
  －
  A<sub>p</sub><sup>2</sup></td></tr></table></span></span>
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
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">cosh</span><sup>－1</sup><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num">
  A<sub>p</sub><sup>2</sup></td></tr><tr><td>
  1
  －
  A<sub>p</sub><sup>2</sup></td></tr></table></span></td></tr><tr><td><span class="normal">cosh</span><sup>－1</sup>
  1 / ω<sub>p</sub></td></tr></table></div>
      </em>
が得られます。
この式を満たすような最小の N を選びます。


## <a id="sec-generated-title-7"></a> <a id="analog2"></a>アナログプロトタイプの設計（透過域周波数固定型）

「[アナログプロトタイプの設計（阻止域周波数固定型）](#analog)」で説明した伝達関数では、
阻止域周波数が <span class="math">1</span> で固定になります。
透過域周波数の方を固定にして設計するために、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table></span>
という伝達関数の代わりに、
<div class="math">
      <span class="normal">|</span>H<sub>I</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:1.5em;">(</span>C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<sub>s</sub>/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr><tr><td><span class="paren" style="font-size:1.5em;">(</span>C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<sub>s</sub>/ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup>
 ＋
 <span class="paren" style="font-size:1.5em;">(</span>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<sub>s</sub><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table></div>
という式を使う手法もあります。


### <a id="sec-generated-title-8"></a> <a id="zp2"></a>零極配置

阻止域周波数固定型の「[零極配置](#zp)」を元に、
<span class="math">ε</span> および零点・極を以下のように置き換えます。

<span class="math">ε</span>
→
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<sub>s</sub><span class="paren" style="font-size:em;">)</span></td></tr></table></span>
零点・極
→
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ω<sub>s</sub></td></tr></table></span>
倍する


### <a id="sec-generated-title-9"></a> <a id="aptf2"></a>アナログプロトタイプ伝達関数

阻止域周波数固定型の「[アナログプロトタイプ伝達関数](#aptf)」を元に、
<span class="math">α<sub>k</sub>, β<sub>k</sub>, γ<sub>k</sub></span> を以下のように置き換えます。

<span class="math">α<sub>k</sub>, β</span>
→
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ω<sub>s</sub></td></tr></table></span>
倍する

<span class="math">β<sub>k</sub>, γ<sub>k</sub></span>
→
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ω<sub>s</sub><sup>2</sup></td></tr></table></span>
倍する


### <a id="sec-generated-title-10"></a> <a id="spec2"></a>設計仕様

透過域/阻止域の周波数/リプル
（<span class="math">A<sub>p</sub>, r<sub>s</sub>, ω<sub>s</sub></span>）
を仕様として与えたとき、
仕様を満たす最小の次数 <span class="math">N</span> とパラメータ <span class="math">ε</span> を求める方法について説明します。
（<span class="math">ω<sub>p</sub></span> は 1 で固定。）

<figure>

[![逆チェビシェフフィルタの設計仕様2](../../../../assets/media/ufcpp2000/sp/chebyshev2s_spec.png)](../../../../assets/media/ufcpp2000/sp/chebyshev2s_spec.png)

<figcaption>逆チェビシェフフィルタの設計仕様2</figcaption>
</figure>


図4より、
透過域周波数固定型の逆チェビシェフフィルタの設計仕様は、
（I型）チェビシェフフィルタの設計仕様（「[設計仕様](chebyshev.md#spec)」参照）と同じになります。
すなわち、
<span class="math">ε</span> は、
<em>
        <div class="math">
ε
＝
<span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>A<sub>p</sub><sup>2</sup></td></tr></table>
－
1
</span></div>
      </em>
となり、
<span class="math">N</span> は、
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
を満たすような最小のものを選びます。
