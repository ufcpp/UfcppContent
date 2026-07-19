---
title: "楕円フィルタ"
source_url: "https://ufcpp.net/study/sp/digital_filter/elliptic/"
content_type: "Article"
published_at: "2004-05-02T00:00:00"
updated_at: "2015-05-06T14:22:49"
tags: []
umbraco_id: 1621
parent_id: 1610
sort_order: 10
aliases:
  - "/digital_filter/elliptic"
  - "/digital_filter/elliptic.html"
  - "/sp/digital_filter/elliptic/"
  - "/study/digital_filter/elliptic"
  - "/study/digital_filter/elliptic.html"
---

# 楕円フィルタ

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[チェビシェフフィルタ](chebyshev.md#chebyshev)」や「[逆チェビシェフフィルタ](chebyshev2.md#chebyshev2)」は、
通過域または阻止域のどちらか一方で等リプルとなるようすることで、
「[バターワースフィルタ](butterworth.md#butterworth)」よりも急峻なカットオフ特性を得ていました。
 
それに対して、
通過域と阻止域の両方で等リプルとなるようにすることで、
より急峻なカットオフ特性を得ようという考え方の基に作られたのが
<strong id="elliptic" class="keyword">楕円フィルタ</strong>（elliptic filter）です。
 
このような特性を持ったフィルタを設計するためには、
楕円関数の知識を要するので、楕円フィルタという名前で呼ばれています。
また、楕円フィルタは、
チェビシェフ特性と逆チェビシェフ特性の考え方を連立して考えるという意味で、
連立チェビシェフフィルタと呼ばれることや、
考案者の名前を取って、Cauerフィルタと呼ばれることもあります。

<figure>
	[![透過域と阻止域の両方にリプル](../../../../assets/media/ufcpp2000/sp/elliptic01.png)](../../../../assets/media/ufcpp2000/sp/elliptic01.png)
	<figcaption>透過域と阻止域の両方にリプル</figcaption>
</figure>



##<a id="sec-generated-title-2"></a> <a id="idea"></a>基本アイディア
チェビシェフ多項式 <span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> や、
その逆 <span class="math">1 / C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span></span> の代わりに、
表1に示すような特徴を持つ<span class="math">n</span>次有理式 <span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> を考えます。

<table summary="チェビシェフ多項式の拡張">
	<caption>
		チェビシェフ多項式の拡張
	</caption>
	<tr>
		<td markdown="1"></td>
		<td markdown="1"><span class="math">C<sub>n</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
		<td markdown="1"><span class="math">1 / C<sub>n</sub><span class="paren" style="font-size:em;">(</span>1/ω<span class="paren" style="font-size:em;">)</span></span></td>
		<td markdown="1"><span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ≦ 1</span>のとき</td>
		<td markdown="1"><span class="math">n</span>個の零点を持っていて、<span class="math">－1 ～ 1</span>の間で振動</td>
		<td markdown="1">緩やかに単調増加</td>
		<td markdown="1"><span class="math">n</span>個の零点を持っていて、<span class="math">－1 ～ 1</span>の間で振動</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ＞ 1</span>のとき</td>
		<td markdown="1">急激に単調増加</td>
		<td markdown="1"><span class="math">n</span>個の極を持っていて、 絶対値が<span class="math">1 ～ ∞</span>の間で振動</td>
		<td markdown="1"><span class="math">n</span>個の極を持っていて、 絶対値が<span class="math">1/k<sub>1</sub> ～ ∞</span>の間で振動 （<span class="math">k<sub>1</sub></span>は定数）</td>
	</tr>
</table>


このような有理式 <span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> を使って、
以下のような周波数特性を作ってやれば、
通過域と阻止域の両方で等リプルとなるよな特性が得られます。
<div class="math">
      <span class="normal">|</span>H<sub>E</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:1.5em;">(</span>ε R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><sup>2</sup></td></tr></table></div>
このようにして得られる周波数特性を<em>楕円特性</em>と呼び、
楕円特性 <span class="math">H<sub>E</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> は、
チェビシェフ特性 <span class="math">H<sub>C</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> や逆チェビシェフ特性 <span class="math">H<sub>I</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> と比べると、表2に示すような特性になります。

<table summary="楕円特性の要点">
	<caption>
		楕円特性の要点
	</caption>
	<tr>
		<td markdown="1"></td>
		<td markdown="1">チェビシェフ特性<span class="math">H<sub>C</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
		<td markdown="1">逆チェビシェフ特性<span class="math">H<sub>I</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
		<td markdown="1">楕円特性<span class="math">H<sub>E</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ≦ 1</span>のとき</td>
		<td markdown="1"><span class="math">1 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ε<sup>2</sup></td></tr></table></span>の間で振動</td>
		<td markdown="1">緩やかに単調減少(1に近い値)</td>
		<td markdown="1"><span class="math">1 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ ε<sup>2</sup></td></tr></table></span>の間で振動</td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">ω ＞ 1</span>のとき</td>
		<td markdown="1">急激に単調減少</td>
		<td markdown="1"><span class="math">0 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ 1/ε<sup>2</sup></td></tr></table></span>の間で振動</td>
		<td markdown="1"><span class="math">0 ～ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>1 ＋ <span class="paren" style="font-size:em;">(</span>ε/k<sub>1</sub><span class="paren" style="font-size:em;">)</span><sup>2</sup></td></tr></table></span>の間で振動</td>
	</tr>
</table>


楕円フィルタの発想は極めて単純ですが、
上述のような特長を持つ有理式 <span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> をどのようにして作るかが問題となります。
このような有理式の作り方を次節で説明します。


##<a id="sec-generated-title-3"></a> <a id="chebyshev_rational"></a>チェビシェフ有理関数
以下のようにして有理式 <span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> を定義することで、
「[基本アイディア](#idea)」で述べたような特徴を持つ有利式が得られ、
この有理式を「[チェビシェフ有理関数](ellipticrational.md#elliptic_rational)」 と呼びます。
<div class="math">
R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cd</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table><span class="normal">cd</span><sup>－1</sup><span class="paren" style="font-size:em;">(</span>
  x,
  k
 <span class="paren" style="font-size:em;">)</span>
 ,
 k<sub>1</sub><span class="paren" style="font-size:2em;">)</span></div>
チェビシェフ有理関数は一見すると複雑に見えますが、
<span class="math">n</span>個の零点と<span class="math">n</span>個の極を持ち、
以下のような<span class="math">n</span>次の有理式で表すことが出来ます。
<div class="math">
      <span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
＝
x
<table class="sigma" summary="product"><tr><td class="sigmasub">(n － 1) / 2</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">m ＝ 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">k<sup>2</sup> ω<sub>m, n</sub><sup>2</sup> － 1</td></tr><tr><td>1 － ω<sub>m, n</sub><sup>2</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">x<sup>2</sup> － ω<sub>m, n</sub><sup>2</sup></td></tr><tr><td>k<sup>2 </sup>ω<sub>m, n</sub><sup>2</sup> x<sup>2</sup> － 1</td></tr></table>
　　　<span class="normal">… n が奇数のとき</span></div><div class="math">
      <span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span>
＝
<table class="sigma" summary="product"><tr><td class="sigmasub">n / 2</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">m ＝ 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">k<sup>2</sup> ω<sub>m, n</sub><sup>2</sup> － 1</td></tr><tr><td>1 － ω<sub>m, n</sub><sup>2</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">x<sup>2</sup> － ω<sub>m, n</sub><sup>2</sup></td></tr><tr><td>k<sup>2 </sup>ω<sub>m, n</sub><sup>2</sup> x<sup>2</sup> － 1</td></tr></table>
　　　<span class="normal">… n が偶数のとき</span></div>
ただし、
<span class="math">
ω<sub>m, n</sub> ＝
<span class="normal">sn</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2m － 1</td></tr><tr><td>n</td></tr></table>K,
  k
<span class="paren" style="font-size:2em;">)</span></span>
です。
 
チェビシェフ有理関数の詳細について説明すると長くなりますので、
別項 「[チェビシェフ有理関数](ellipticrational.md#abstract)」 で説明します。
上式中の <span class="math">k, k<sub>1</sub></span> などのパラメータの意味についても
「[チェビシェフ有理関数](ellipticrational.md#abstract)」 を参照してください。


##<a id="sec-generated-title-4"></a> <a id="property"></a>周波数特性
以下の図に、例として、3次、5次、9次の楕円フィルタの振幅特性を示します。
この例では、リプル幅は 0.1 で設計しています。

<figure>
	[![3～9次の楕円フィルタの周波数特性](../../../../assets/media/ufcpp2000/sp/elliptic_amp.png)](../../../../assets/media/ufcpp2000/sp/elliptic_amp.png)
	<figcaption>3～9次の楕円フィルタの周波数特性</figcaption>
</figure>



##<a id="sec-generated-title-5"></a> <a id="analog"></a>アナログプロトタイプの設計
###<a id="sec-generated-title-6"></a> <a id="zero"></a>零点配置
<span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> の極が
<span class="math">H<sub>E</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> の零点になります。
<span class="math"><span class="normal">cd</span><span class="paren" style="font-size:em;">(</span>u, k<span class="paren" style="font-size:em;">)</span></span> の極は
<span class="math"><span class="paren" style="font-size:em;">(</span>2 m ＋ 1<span class="paren" style="font-size:em;">)</span> K ＋ i K<sub>1</sub>'</span> なので、
<span class="math">R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> の極は、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table>
        <span class="normal">cd</span>
        <sup>－1</sup>
        <span class="paren" style="font-size:em;">(</span>ω, k<span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:em;">(</span>2 m ＋ 1<span class="paren" style="font-size:em;">)</span> K<sub>1</sub> ＋ i K<sub>1</sub>'
　　　
<span class="normal">（m は任意の自然数。）</span></div>
の解になります。
これを解くことにより、
<div class="math">
        <span class="normal">cd</span>
        <sup>－1</sup>
        <span class="paren" style="font-size:em;">(</span>ω, k<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">2 m ＋ 1</td></tr><tr><td>n</td></tr></table> K
＋ i K'
</div><div class="math">
ω
＝
<span class="normal">cd</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">2 m ＋ 1</td></tr><tr><td>n</td></tr></table> K
＋ i K'
, k
<span class="paren" style="font-size:1.5em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
k
<span class="normal">cd</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">2 m ＋ 1</td></tr><tr><td>n</td></tr></table> K
, k
<span class="paren" style="font-size:1.5em;">)</span></td></tr></table></div>
が得られます。
ラプラス変数 <span class="math">s ＝ i ω</span> で表すと、
伝達関数の零点 <span class="math">s</span> は以下のようになります。
<em>
        <div class="math">
s
＝
± i
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
k
<span class="normal">sn</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2 m － 1</td></tr><tr><td>n</td></tr></table> K
, k
<span class="paren" style="font-size:1.5em;">)</span></td></tr></table></div>
      </em>
ただし、<span class="math">k</span>は<span class="math">0～(n－1) / 2</span>までの整数です。


###<a id="sec-generated-title-7"></a> <a id="pole"></a>極配置
<span class="math">H<sub>E</sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span></span> の極は、
<div class="math">
R<sub>n, k<sub>1</sub></sub><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">cd</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table><span class="normal">cd</span><sup>－1</sup><span class="paren" style="font-size:em;">(</span>
  ω,
  k
 <span class="paren" style="font-size:em;">)</span>
 ,
 k<sub>1</sub><span class="paren" style="font-size:2em;">)</span>
＝
±i <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></div>
を解くことで得られます。
<span class="math"><span class="normal">cd</span><sup>－1</sup><span class="paren" style="font-size:em;">(</span>ω, k<span class="paren" style="font-size:em;">)</span>
＝
u ＋ i v
</span>
と置くと、
<div class="math">
        <span class="normal">cd</span>
        <span class="paren" style="font-size:2em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table>
          <span class="paren" style="font-size:em;">(</span>u ＋ i v<span class="paren" style="font-size:em;">)</span>
 ,
 k<sub>1</sub><span class="paren" style="font-size:2em;">)</span>
＝
±i <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></div>
となります。
ここで、
<span class="math">
sn ＝
<span class="normal">sn</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table> u － K<sub>1</sub> , k<sub>1</sub><span class="paren" style="font-size:1.5em;">)</span>,
　
sn' ＝
<span class="normal">sn</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table> v , k<sub>1</sub>'
<span class="paren" style="font-size:1.5em;">)</span></span>
などと置くと、ヤコビの楕円関数の加法定理より、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
sn dn' + i cn dn sn' cn'
</td></tr><tr><td>
cn'<sup>2</sup> ＋ k<sub>1</sub><sup>2</sup> sn<sup>2</sup> sn'<sup>2</sup></td></tr></table>
＝
±i <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></div>
となります。
右辺の実部が0、かつ、
<span class="math">dn'</span> は常に非0なので、
<div class="math">
sn ＝
<span class="normal">sn</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table> u － K<sub>1</sub> , k<sub>1</sub><span class="paren" style="font-size:1.5em;">)</span>
＝
0
</div>
が得られ、したがって、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table> u － K<sub>1</sub>
＝
2 m K<sub>1</sub>
　　　
<span class="normal">（m は任意の自然数。）</span></div><div class="math">
∴
u
＝
<table class="frac" summary="fraction"><tr><td class="num">2 m ＋ 1</td></tr><tr><td>n</td></tr></table> K
</div>
が得られます。
このとき、<span class="math">cn ＝ dn ＝ 0</span> なので、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
sn' cn'
</td></tr><tr><td>
cn'<sup>2</sup></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">
sn'
</td></tr><tr><td>
cn'
</td></tr></table>
＝
±<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></div>
よって、
<div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">n K<sub>1</sub></td></tr><tr><td>K</td></tr></table> v
＝
±<span class="normal">sc</span><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="paren" style="font-size:1.5em;">)</span></div><div class="math">
∴
v
＝
±
<table class="frac" summary="fraction"><tr><td class="num">K</td></tr><tr><td>n K<sub>1</sub></td></tr></table><span class="normal">sc</span><sup>－1</sup><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="paren" style="font-size:1.5em;">)</span></div>
これらの結果をら、
<span class="math"><span class="normal">cd</span><sup>－1</sup><span class="paren" style="font-size:em;">(</span>ω, k<span class="paren" style="font-size:em;">)</span>
＝
u ＋ i v
</span>
に代入することで、解 <span class="math">ω</span> が得られます。
<span class="math">
sn ＝
<span class="normal">sn</span><span class="paren" style="font-size:em;">(</span>u, k<span class="paren" style="font-size:em;">)</span>,
　
sn' ＝
<span class="normal">sn</span><span class="paren" style="font-size:em;">(</span>v, k'<span class="paren" style="font-size:em;">)</span>,
</span>
などと置くと、
<div class="math">
ω
＝
<table class="frac" summary="fraction"><tr><td class="num">
sn dn' ± i cn dn sn' cn'
</td></tr><tr><td>
1 － dn<sup>2</sup> sn'<sup>2</sup></td></tr></table></div>
となります。
この中から安定極のみを選ぶことで、伝達関数の極 <span class="math">s</span> は以下のようになります
<em>
        <div class="math">
s
＝
<table class="frac" summary="fraction"><tr><td class="num">
－ cn dn sn' cn'
± i sn dn'
</td></tr><tr><td>
1 － dn<sup>2</sup> sn'<sup>2</sup></td></tr></table></div>
      </em><div class="math">
sn
＝
<span class="normal">sn</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">n － 2m － 1</td></tr><tr><td>n</td></tr></table>
K
,
k
<span class="paren" style="font-size:2em;">)</span>
　　　
<span class="normal">（</span>cn, dn <span class="normal">も同様）</span></div><div class="math">
sn'
＝
<span class="normal">sn</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">K</td></tr><tr><td>n K<sub>1</sub></td></tr></table><span class="normal">sc</span><sup>－1</sup><span class="paren" style="font-size:1.5em;">(</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table><span class="paren" style="font-size:1.5em;">)</span>
,
k'
<span class="paren" style="font-size:2em;">)</span>
　　　
<span class="normal">（</span>cn', dn' <span class="normal">も同様）</span></div>
ただし、<span class="math">k</span>は<span class="math">0～(n－1) / 2</span>までの整数です。


###<a id="sec-generated-title-8"></a> <a id="spec"></a>設計仕様
透過域/阻止域の周波数/リプル
（<span class="math">A<sub>p</sub>, r<sub>s</sub>, ω<sub>s</sub></span>）
を仕様として与えたとき、
仕様を満たす最小の次数 <span class="math">N</span> とパラメータ <span class="math">ε, k<sub>1</sub></span> を求める方法について説明します。
（<span class="math">ω<sub>p</sub></span> は 1 で固定。）

<figure>
	[![楕円フィルタの設計仕様](../../../../assets/media/ufcpp2000/sp/elliptic_spec.png)](../../../../assets/media/ufcpp2000/sp/elliptic_spec.png)
	<figcaption>楕円フィルタの設計仕様</figcaption>
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
 
次に、阻止域リプルから <span class="math">k<sub>1</sub></span> を決定します。
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
1 ＋
<table class="frac" summary="fraction"><tr><td class="num">
ε<sup>2</sup></td></tr><tr><td>
k<sub>1</sub><sup>2</sup></td></tr></table></td></tr></table>
＝
r<sub>s</sub><sup>2</sup></span>
なので、
<em>
        <div class="math">
k<sub>1</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num">ε</td></tr><tr><td><span class="normal" style="font-size:em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
r<sub>s</sub><sup>2</sup></td></tr></table>
－
1
</span></td></tr></table></div>
      </em>
が得られます。
 
また、
<span class="math">ω<sub>s</sub> ≦ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>k</td></tr></table></span>
および
楕円積分の性質
（<span class="math">K<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> は単調増加、
<span class="math">K' <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> は単調減少）を用いると、
<em>
        <div class="math">
N
＝
<table class="frac" summary="fraction"><tr><td class="num">K<sub>1</sub>' K</td></tr><tr><td>K<sub>1</sub> K'</td></tr></table>
≧
<table class="frac" summary="fraction"><tr><td class="num">K<sub>1</sub>' K<span class="paren" style="font-size:em;">(</span>1/ω<sub>s</sub><span class="paren" style="font-size:em;">)</span></td></tr><tr><td>K<sub>1</sub> K' <span class="paren" style="font-size:em;">(</span>1/ω<sub>s</sub><span class="paren" style="font-size:em;">)</span></td></tr></table></div>
      </em>
となり、
この式を満たすような最小の N を選びます。
 
そして最後に、
<span class="math">
N
＝
<table class="frac" summary="fraction"><tr><td class="num">K<sub>1</sub>' K</td></tr><tr><td>K<sub>1</sub> K'</td></tr></table></span>
となるような <span class="math">k</span> を求めます。
