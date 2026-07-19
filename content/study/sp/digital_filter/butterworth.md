---
title: "バターワースフィルタ"
source_url: "https://ufcpp.net/study/sp/digital_filter/butterworth/"
content_type: "Article"
published_at: "2004-04-03T00:00:00"
updated_at: "2015-05-06T14:22:40"
tags: []
umbraco_id: 1618
parent_id: 1610
sort_order: 7
aliases:
  - "/digital_filter/butterworth"
  - "/digital_filter/butterworth.html"
  - "/sp/digital_filter/butterworth/"
  - "/study/digital_filter/butterworth"
  - "/study/digital_filter/butterworth.html"
---

# バターワースフィルタ

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<strong id="butterworth" class="keyword">バターワースフィルタ</strong>（Butterworth filter）は、
通過域で最大平坦な振幅特性を示すローパスフィルタです。
位相特性も線形に近いという特徴があります。

<figure>
	[![バターワースフィルタの周波数特性](../../../../assets/media/ufcpp2000/sp/butterworth01.png)](../../../../assets/media/ufcpp2000/sp/butterworth01.png)
	<figcaption>バターワースフィルタの周波数特性</figcaption>
</figure>



##<a id="sec-generated-title-2"></a> <a id="property"></a>周波数特性
バターワースフィルタは以下のような周波数特性を持ちます。
<div class="math"><span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ω<sup>2n</sup></td></tr></table></div>
この特性は<span class="math">ω＞1</span>で急激に減衰していきます。
<span class="math">ω</span>の変わりに<span class="math">ω/ω<sub>c</sub></span>を代入することでカットオフ周波数<span class="math">ω<sub>c</sub></span>を任意に設定できます。
また、この伝達関数は1階から<span class="math">2n－1</span>階までの全ての導関数が<span class="math">ω ＝ 0</span>において0であるという性質を持っています。
この性質を最大平坦と呼びます。
 
図2に、例として、3次、5次、9次のバターワースフィルタの振幅特性を示します。

<figure>
	[![3～9次のバターワースフィルタの周波数特性](../../../../assets/media/ufcpp2000/sp/butterworth_amp.png)](../../../../assets/media/ufcpp2000/sp/butterworth_amp.png)
	<figcaption>3～9次のバターワースフィルタの周波数特性</figcaption>
</figure>



##<a id="sec-generated-title-3"></a> <a id="analog"></a>アナログプロトタイプの設計
###<a id="sec-generated-title-4"></a> <a id="zp"></a>極配置
<span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup>の分母は<span class="math">2n</span>次の多項式なので、
<span class="normal">|</span>H<span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup>は<span class="math">2n</span>個の極を持ちます。
この<span class="math">2n</span>個の極は、
<span class="math">ω<sup>2n</sup> ＝ －1</span>より、以下のようになります。
<div class="math">
ω ＝
<span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>i<table class="frac" summary="fraction"><tr><td class="num">2k ＋ 1</td></tr><tr><td>2n</td></tr></table>π<span class="paren" style="font-size:2em;">)</span></div>
この<span class="math">2n</span>個の中から安定な<span class="math">n</span>個の極を選び、
バターワースフィルタの極 <span class="math">s</span> にします。
<em><div class="math">
s ＝
－
<span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">n － 2k － 1</td></tr><tr><td>2n</td></tr></table>π
± i
<span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">n － 2k － 1</td></tr><tr><td>2n</td></tr></table>π
</div></em>
ただし、<span class="math">k</span>は<span class="math">0～(n－1) / 2</span>までの整数です。


###<a id="sec-generated-title-5"></a> <a id="aptf"></a>アナログプロトタイプ伝達関数
決定された極配置から、バターワースフィルタの伝達関数<span class="math">H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>は以下のようになります。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table> － 1</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>s<sup>2</sup> ＋ α<sub>k</sub> s ＋ 1</td></tr></table>
　　　<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>s ＋ 1</td></tr></table><table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>s<sup>2</sup> ＋ α<sub>k</sub> s ＋ 1</td></tr></table>
　　　<span class="normal">（nが奇数のとき）</span></div>
ただし、<span class="math">α<sub>k</sub></span> は以下の式で表される値です。
<div class="math">
α<sub>k</sub>
＝
2
<span class="normal">cos</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 k － 1</td></tr><tr><td>2 n</td></tr></table>π<span class="paren" style="font-size:1.5em;">)</span></div>

###<a id="sec-generated-title-6"></a> <a id="spec"></a>設計仕様
透過域/阻止域の周波数/リプル
（<span class="math">A<sub>p</sub>, r<sub>s</sub>, ω<sub>p</sub>, ω<sub>s</sub></span>）
を仕様として与えたとき、
仕様を満たす最小の次数 <span class="math">N</span> を求める方法について説明します。

<figure>
	[![バターワースフィルタの設計仕様](../../../../assets/media/ufcpp2000/sp/butterworth_spec.png)](../../../../assets/media/ufcpp2000/sp/butterworth_spec.png)
	<figcaption>バターワースフィルタの設計仕様</figcaption>
</figure>


このような次数 <span class="math">N</span> を求めるためには、
図3に示すように、
<div class="math">
A<sub>p</sub><sup>2</sup>
≦
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
1
＋
ω<sub>p</sub><sup>2 N</sup></td></tr></table>
, 　
r<sub>p</sub><sup>2</sup>
≧
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
1
＋
ω<sub>s</sub><sup>2 N</sup></td></tr></table></div>
という条件を満たすような最小の <span class="math">N</span> を求めます。
これらの式から、
<div class="math">
N ≧
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">log</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>A<sub>p</sub><sup>2</sup></td></tr></table>
   －
   1
  <span class="paren" style="font-size:1.5em;">)</span></td></tr><tr><td>
  2
  <span class="normal">log</span>
  ω<sub>p</sub></td></tr></table>
, 　
N ≧
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">log</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>r<sub>s</sub><sup>2</sup></td></tr></table>
   －
   1
  <span class="paren" style="font-size:1.5em;">)</span></td></tr><tr><td>
  2
  <span class="normal">log</span>
  ω<sub>s</sub></td></tr></table></div>
という式が得られるので、
これらの最大値を選ぶことにより、
<em><div class="math">
N ≧
<span class="normal">Max</span><span class="paren" style="font-size:3em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">log</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>A<sub>p</sub><sup>2</sup></td></tr></table>
   －
   1
  <span class="paren" style="font-size:1.5em;">)</span></td></tr><tr><td>
  2
  <span class="normal">log</span>
  ω<sub>p</sub></td></tr></table>
, 
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">log</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>r<sub>s</sub><sup>2</sup></td></tr></table>
   －
   1
  <span class="paren" style="font-size:1.5em;">)</span></td></tr><tr><td>
  2
  <span class="normal">log</span>
  ω<sub>s</sub></td></tr></table><span class="paren" style="font-size:3em;">)</span></div></em>
という式が得られます。
この不等式を満たすような最小の <span class="math">N</span> を選びます。


##<a id="sec-generated-title-7"></a> <a id="digital"></a>ディジタルフィルタ設計
「[アナログプロトタイプの設計](#analog)」で得られた式を、
カットオフ周波数<span class="math">ω<sub>c</sub></span>で双1次変換することで以下の式が得られます。
<div class="math">
c ＝ <span class="normal">cos</span> ω<sub>c</sub> , 
s ＝ <span class="normal">sin</span> ω<sub>c</sub></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table>－1</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">1－c</td></tr><tr><td>2</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="paren" style="font-size:em;">(</span>
1 ＋ 2 z<sup>－1</sup> ＋ z<sup>－2</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>1＋α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
－
2 c z<sup>－1</sup>
＋
<span class="paren" style="font-size:em;">(</span>1－α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
z<sup>－2</sup></td></tr></table>
, 
　　
<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s <span class="paren" style="font-size:em;">(</span>1 ＋ z<sup>－1</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>s ＋ c + 1<span class="paren" style="font-size:em;">)</span> ＋ <span class="paren" style="font-size:em;">(</span>s － c － 1<span class="paren" style="font-size:em;">)</span> z<sup>－1</sup></td></tr></table>
×
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">1－c</td></tr><tr><td>2</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="paren" style="font-size:em;">(</span>
1 ＋ 2 z<sup>－1</sup> ＋ z<sup>－2</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>1＋α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
－
2 c z<sup>－1</sup>
＋
<span class="paren" style="font-size:em;">(</span>1－α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
z<sup>－2</sup></td></tr></table>
　　
<span class="normal">（nが奇数のとき）</span></div>
ただし、<span class="math">α<sub>k, n</sub></span> は以下の式で表される値です。 
<div class="math">
α<sub>k, n</sub>
＝
s <span class="normal">cos</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2k ＋ 1</td></tr><tr><td>2n</td></tr></table>π<span class="paren" style="font-size:2em;">)</span></div>

##<a id="sec-generated-title-8"></a> <a id="highpass"></a>ハイパスフィルタ
「[周波数変換](transform.md)」で説明したように、
ローパスフィルタのアナログ伝達関数の変数 <span class="math">s</span> を <span class="math">s → <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table></span> に入れ替えると、
ハイパスフィルタになります。
したがって、
ハイパスバターワースフィルタのアナログプロトタイプ伝達関数は以下のようになります。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table> － 1</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup></td></tr><tr><td>s<sup>2</sup> ＋ α<sub>k</sub> s ＋ 1</td></tr></table>
　　　<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s</td></tr><tr><td>s ＋ 1</td></tr></table><table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup></td></tr><tr><td>s<sup>2</sup> ＋ α<sub>k</sub> s ＋ 1</td></tr></table>
　　　<span class="normal">（nが奇数のとき）</span></div>
この式の分母は、ローパス伝達関数の分母と全く同じものになっています。
（バターワースフィルタの伝達関数は、分母の定数項と2次の係数が同じなのでこうなる。）
その結果、ディジタルフィルタ伝達関数も、以下のように、
ローパス伝達関数と分母が全く同じものになります。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n</td></tr><tr><td>2</td></tr></table>－1</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">1＋c</td></tr><tr><td>2</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="paren" style="font-size:em;">(</span>
1 － 2 z<sup>－1</sup> ＋ z<sup>－2</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>1＋α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
－
2 c z<sup>－1</sup>
＋
<span class="paren" style="font-size:em;">(</span>1－α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
z<sup>－2</sup></td></tr></table>
　　
<span class="normal">（nが偶数のとき）</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:em;">(</span>1<em>＋</em>c<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>1 <em>－</em> z<sup>－1</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>s ＋ c + 1<span class="paren" style="font-size:em;">)</span> ＋ <span class="paren" style="font-size:em;">(</span>s － c － 1<span class="paren" style="font-size:em;">)</span> z<sup>－1</sup></td></tr></table>
×
<table class="sigma" summary="product"><tr><td class="sigmasub"><table class="frac" summary="fraction"><tr><td class="num">n－1</td></tr><tr><td>2</td></tr></table></td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">1<em>＋</em>c</td></tr><tr><td>2</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="paren" style="font-size:em;">(</span>
1 <em>－</em> 2 z<sup>－1</sup> ＋ z<sup>－2</sup><span class="paren" style="font-size:em;">)</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>1＋α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
－
2 c z<sup>－1</sup>
＋
<span class="paren" style="font-size:em;">(</span>1－α<sub>k, n</sub><span class="paren" style="font-size:em;">)</span>
z<sup>－2</sup></td></tr></table>
　　
<span class="normal">（nが奇数のとき）</span></div>
この式を、ローパス伝達関数と見比べてみると、
分母が同じなだけでなく、
分子も似たものになっています。
式中で強調表示している部分の符号が異なっているだけです。
このことから、同じカットオフ周波数のローパス・ハイパスフィルタは、
その処理の大部分を共通化することができます。
