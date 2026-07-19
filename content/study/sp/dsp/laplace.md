---
title: "ラプラス変換"
source_url: "https://ufcpp.net/study/sp/dsp/laplace/"
content_type: "Article"
published_at: "2015-05-06T14:22:05"
updated_at: "2022-07-10T18:19:24"
tags: []
umbraco_id: 1605
parent_id: 1599
sort_order: 5
aliases:
  - "/dsp/laplace"
  - "/dsp/laplace.html"
  - "/sp/dsp/laplace/"
  - "/study/dsp/laplace"
  - "/study/dsp/laplace.html"
---

# ラプラス変換

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

ラプラス変換とは、「[フーリエ変換](fourier.md#f-trans)」とよく似た式で表される積分変換
（積分の形で表される、関数→関数の変換）の一種です。
 
ラプラス変換は簡単に言うと、フーリエ変換において <span class="math">iω</span> となっていた部分に <span class="math">s</span> を代入したもので、
フーリエ変換を拡張したものになっています。
 
フーリエ変換が主に周波数解析（定常応答解析）に使われるのに対して、
ラプラス変換は過渡応答や安定性の解析に使われます。


## <a id="sec-generated-title-2"></a> <a id="definition"></a>ラプラス変換の定義

連続関数 <span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> に対して、
<div class="math">
F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－st<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div>
で表される積分変換を<strong id="Laplace" class="keyword">ラプラス変換</strong>（Laplace Transform、ラプラスは人名（Pierre-Simon Laplace））といいます。
 
この式は、「[フーリエ変換](fourier.md#f-trans)」の式中の <span class="math">iω</span> （<span class="math">ω</span> は実数）の部分に <span class="math">s</span> （<span class="math">s</span> は複素数）を代入したものになっています。
フーリエ変換では、微分演算子は <span class="math">iω</span> に、積分は <span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>iω</td></tr></table></span> に変換されます。
すなわち、ラプラス変換の変数 <span class="math">s</span> は微分演算子に相当するものです。
 
ラプラス変換では、<span class="math">iω</span> を <span class="math">s</span> で置き換えたことによって、
→∞ 方向に非常に強い収束性を持つようになります。
フーリエ変換では、<span class="math"><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－st<span class="paren" style="font-size:1em;">)</span></span> という周期関数を掛け合わせているため、<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> 自信が →∞ において収束する必要があったのですが、
ラプラス変換では指数関数を掛け合わせているため、<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> が →∞ で発散するような関数でもラプラス変換した結果が意味を持ちます。
（まあ、ちょっと難しい話をすると、現代のフーリエ変換の定義では、→∞ で発散する関数（正確には緩増加関数）も超関数的な意味合いでフーリエ変換可能です。）
 
しかしながら、逆向き（→－∞）方向に対しては強く発散してしまうため、
ラプラス変換は因果的な関数に対してのみしか適用できません。
<em>因果的</em>な関数というのは、ある時刻 <span class="math">T</span> 以前では値を持たない関数です。
すなわち、ある値 <span class="math">T</span> に対して、<span class="math">t＜T</span> となるような任意の <span class="math">t</span> において <span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ 0</span> となるような関数です。
 
ラプラス変換は因果的な関数にしか適用できないので、
あらかじめ積分範囲を [T, ∞) に絞って考えるのが一般的です。
通常は <span class="math">T ＝ 0</span> として考えるので、ラプラス変換の式は以下のようになります。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>－st<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t
</div>
工学系応用分野ではこの式こそがラプラス変換の定義だと思って構いません。
上述の式をこちらを区別するために、前者を<em>両側ラプラス変換</em>、後者を<em>片側ラプラス変換</em>と呼ぶこともあります。
このページは工学的な応用を中心に解説していますので、片側ラプラス変換について説明していきます。
 
簡単化のために、以下のように、ラプラス変換を記号 <span class="math"><span class="cursive">L</span></span> （筆記体の L）で表します。
<div class="math">
F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＝
<span class="normal">ℒ</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
また、<span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> のように、ラプラス変換後の関数を<strong id="d23e198" class="keyword">伝達関数</strong>と呼びます。
（制御やシステムなどの分野で使う用語。言葉の意味は「システム」で説明します。）


## <a id="sec-generated-title-3"></a> <a id="example"></a>ラプラス変換の例

いくつか代表的な初等関数のラプラス変換の例を挙げておきます。

<table summary="初等関数のラプラス変換結果">
	<caption>
		初等関数のラプラス変換結果
	</caption>
	<tr>
		<th>関数</th>
		<th>ラプラス変換結果</th>
	</tr>
	<tr>
		<td markdown="1"><span class="math">c</span>（定数）</td>
		<td markdown="1"><span class="math">
            <table class="frac" summary="fraction"><tr><td class="num">c</td></tr><tr><td>s</td></tr></table>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">t<sup>n</sup></span>（多項式、<span class="math">n</span>は自然数。）</td>
		<td markdown="1"><span class="math">
            <table class="frac" summary="fraction"><tr><td class="num">n!</td></tr><tr><td>s<sup>n ＋ 1</sup></td></tr></table>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="normal">e</span>
            <sup>a t</sup>
          </span>（指数関数）</td>
		<td markdown="1"><span class="math">
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s － a</td></tr></table>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="normal">cos</span>ωt</span></td>
		<td markdown="1"><span class="math">
            <table class="frac" summary="fraction"><tr><td class="num">s</td></tr><tr><td>s<sup>2</sup> ＋ ω<sup>2</sup></td></tr></table>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><span class="math">
            <span class="normal">sin</span>ωt</span></td>
		<td markdown="1"><span class="math">
            <table class="frac" summary="fraction"><tr><td class="num">ω</td></tr><tr><td>s<sup>2</sup> ＋ ω<sup>2</sup></td></tr></table>
          </span></td>
	</tr>
</table>


これらは、ラプラス変換の定義から割りと簡単に計算することができるので、一度自分の手で計算して見てください。
（多項式は部分積分を繰り返して、sin, cos は <span class="math"><span class="normal">sin</span> ＝ <span class="script">Im</span><span class="normal">exp</span>, <span class="normal">cos</span> ＝ <span class="script">Re</span><span class="normal">exp</span></span> であることを利用して計算すると楽です。）


## <a id="sec-generated-title-4"></a> <a id="property"></a>ラプラス変換の性質

ラプラス変換は、以下に示すように、フーリエ変換と非常によく似た性質を持っています。
（「[フーリエ変換の性質](fourier.md#property)」を参照。）
積分範囲を片側に限ってしまったために、ところどころ性質が異なっているので注意が必要です。


### <a id="sec-generated-title-5"></a> <a id="linear"></a>線形性

<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
a f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＋
b g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
a
<span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＋
b
<span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>

### <a id="sec-generated-title-6"></a> <a id="differential"></a>微分 ⇔ 多項式

微分演算子は <span class="math">s</span> に変換されます。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
          <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
s
<span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
－ f<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></div>
片側ラプラス変換では、定数項 <span class="math">f<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></span> が現れるので注意が必要です。
 
また、n階微分は <span class="math">s<sup>n</sup></span> に変換されます。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
          <span class="paren" style="font-size:2em;">(</span>
            <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
          <span class="paren" style="font-size:2em;">)</span>
          <sup>n</sup> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
s<sup>n</sup><span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
－ s<sup>n－1</sup> f<sup></sup><span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span>
－ s<sup>n－2</sup> f<sup><span class="paren" style="font-size:em;">(</span>1<span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span>
－ ・・・
－ s f<sup><span class="paren" style="font-size:em;">(</span>n－2<span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span>
－ f<sup><span class="paren" style="font-size:em;">(</span>n－1<span class="paren" style="font-size:em;">)</span></sup><span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></div>
こちらも定数項が現れるので注意が必要です。
 
逆に、多項式倍は微分演算子に変換されます。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
t<sup>n</sup> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:2em;">(</span>
－
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>s</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>n</sup><span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
ただし、微分→<span class="math">s</span>のときと違って、－ 符号が付くので注意。


### <a id="sec-generated-title-7"></a> <a id="integral"></a>積分

積分は <span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table></span> に変換されます。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
          <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> t</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>τ<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>τ
<span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table><span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
積分範囲が「[フーリエ変換の性質](fourier.md#property)」のときと異なるので注意してください。
 
逆に、<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>t</td></tr></table></span> は積分に変換されます。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
          <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>t</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">s</td></tr></table><sup>n</sup><span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
積分→<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table></span> のときとは積分範囲が逆なので注意。


### <a id="sec-generated-title-8"></a> <a id="timeshift"></a>時間シフト

時間シフトは指数関数倍に変換されます。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t±a<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>±a s<span class="paren" style="font-size:em;">)</span><span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
逆に、指数関数倍はシフトになります。
<div class="math">
        <span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
          <span class="normal">exp</span>
          <span class="paren" style="font-size:em;">(</span>a t<span class="paren" style="font-size:em;">)</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">ℒ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t － a<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
シフト→指数関数倍のときとは <span class="math">a</span> の符号が逆になるので注意。


### <a id="sec-generated-title-9"></a> <a id="convolution"></a>畳み込み積

フーリエ変換と同様に、畳み込み積のラプラス変換はただの積になります。
<div class="math">
        <span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f＊g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span>
        <span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
×
<span class="normal">ℱ</span><span class="paren" style="font-size:2em;">[</span>
g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:2em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
ただし、フーリエ変換の場合と、畳み込み積の定義における積分範囲が少し異なります。
（－∞ が 0 になっています。）
<div class="math">
f＊g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>τ<span class="paren" style="font-size:em;">)</span>
g<span class="paren" style="font-size:em;">(</span>t－τ<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>τ
</div>

## <a id="sec-generated-title-10"></a> <a id="inverse"></a>逆変換

### <a id="sec-generated-title-11"></a> <a id="d23e763"></a>逆変換の式

基本的に、ラプラス変換はフーリエ変換の式において <span class="math">iω ＝ s</span> としたものなので、
逆ラプラス変換も逆フーリエ変換の式に <span class="math">iω ＝ s</span> を入れて OK かというと、
実はそうもいきません。
詳細は説明しませんが、逆ラプラス変換の公式は以下のようになります。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">T → ∞</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> σ ＋ iT</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">σ － iT</td></tr></table> F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>st<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>s
</div>
まあ、積分の中身は確かに、逆フーリエ変換の式を <span class="math">iω ＝ s</span> としたものなんですが、
積分範囲が異なります。
虚軸に平行な線に沿っての積分になっています。
 
なんだか難しそうですが、実際には、この式そのものを使って逆変換を行うことはあまりないので安心してください。
では、実際どうするかというのをこれから説明していきます。


### <a id="sec-generated-title-12"></a> <a id="d23e805"></a>変換公式を頼りに逆変換

「[ラプラス変換の例](#example)」で説明したように、
<span class="math"><span class="normal">e</span><sup>a t</sup></span> → <span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s － a</td></tr></table></span>
などといった変換公式が成り立ちます。
この公式を逆にたどって、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s － a</td></tr></table></span> → <span class="math"><span class="normal">e</span><sup>a t</sup></span>
というように逆ラプラス変換を行うことができます。
 
こういうやりかただと、公式が適用できない場合にはどうするんだ？という疑問があるかと思いますが、
そのときはそのときであきらめます。
といっても、実用上よく使う関数はたいてい、公式だけで逆変換できますので安心してください。
 
例えば、多項式、指数関数、三角関数およびその積・微分・積分は、
ラプラス変換すれば全て <span class="math">s</span> の有理式になります。
（時間シフトも絡むと、それに指数関数を掛けたものになる。これも公式に当てはめて逆変換可能。）
なので、有理式の逆ラプラス変換さえできれば実用上結構有益だといえます。
 
さて、公式通りに逆変換できるのは以下のような形の関数です。

* <span class="math">
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s</td></tr></table>
          </span>→<span class="math">1</span>

* <span class="math">
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s ± a</td></tr></table>
          </span>→<span class="math"><span class="normal">e</span><sup>±a t</sup></span>

* <span class="math">
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>s<sup>n</sup></td></tr></table>
          </span>→<span class="math"><table class="frac" summary="fraction"><tr><td class="num">t<sup>n － 1</sup></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>n － 1<span class="paren" style="font-size:em;">)</span>!</td></tr></table></span>

* <span class="math">
            <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
              <span class="paren" style="font-size:em;">(</span>s ± a<span class="paren" style="font-size:em;">)</span>
              <sup>n</sup>
            </td></tr></table>
          </span>→<span class="math"><table class="frac" summary="fraction"><tr><td class="num">t<sup>n － 1</sup></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>n － 1<span class="paren" style="font-size:em;">)</span>!</td></tr></table><span class="normal">e</span><sup>±a t</sup></span>

* <table class="frac" summary="fraction"><tr><td class="num">s ＋ σ</td></tr><tr><td>
              <span class="paren" style="font-size:em;">(</span>s ＋ σ<span class="paren" style="font-size:em;">)</span>
              <sup>2</sup> ＋ ω<sup>2</sup></td></tr></table>→<span class="math"><span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt</span>

* <span class="math">
            <table class="frac" summary="fraction"><tr><td class="num">ω</td></tr><tr><td>
                <span class="paren" style="font-size:em;">(</span>s ＋ σ<span class="paren" style="font-size:em;">)</span>
                <sup>2</sup> ＋ ω<sup>2</sup></td></tr></table>
          </span>→<span class="math"><span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt</span>


これだけ分かっていれば、任意の有理式を逆ラプラス変換することができます。
任意の有理式は、部分分数分解により以下のように変形することができます。
<div class="math">
c
＋
<table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">j</td></tr></table>
c<sub>i, j</sub><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="paren" style="font-size:em;">(</span>s － a<sub>i</sub><span class="paren" style="font-size:em;">)</span><sup>j</sup></td></tr></table></div>
ただし、<span class="math">c, c<sub>i, j</sub>, a<sub>i</sub></span> は定数です。
元の有理式が実係数なら、これらの定数は実数または互いに共役な複素数のペアになります。
<span class="math">c<sub>i, j</sub>, a<sub>i</sub></span> が共役複素数の場合には、以下のように書き直すことができます。
<div class="math">
        <table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">j</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">α<sub>i</sub><span class="paren" style="font-size:em;">(</span>s ＋ σ<sub>i</sub><span class="paren" style="font-size:em;">)</span> ＋ β<sub>i</sub> ω<sub>i</sub></td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>s ＋ σ<sub>i</sub><span class="paren" style="font-size:em;">)</span>
            <sup>2</sup> ＋ ω<sub>i</sub><sup>2</sup></td></tr></table>
      </div>
<span class="math">
α<sub>i</sub>, β<sub>i</sub>, σ<sub>i</sub>, ω<sub>i</sub></span>
はいずれも実数の定数になります。
ここまでくればあとはこれらを先ほどの公式に当てはめて逆変換するだけです。
複雑な式になると、部分分数分解がちょっと面倒な作業になりますが、理論上はどんな有理式でも逆ラプラス変換することが可能です。


### <a id="sec-generated-title-13"></a> <a id="d23e1076"></a>留数を使った逆変換

最初に述べた逆ラプラス変換の式の計算は、
「[留数](../../math/analysis/residue.md#residue)」というものを使うと多少楽に計算することができます。
これもここでは公式を示すのみにとどめ、詳細な説明はしません。
留数を使った逆ラプラス変換の公式は以下のようになります。
 
関数 <span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> の極を <span class="math">s<sub>i</sub></span> （<span class="math">i ＝ 1, 2, ・・・, N</span>）とすると、<span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> の逆ラプラス変換結果 <span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> は以下のようになる。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝1</td></tr></table><span class="normal">Res</span><span class="paren" style="font-size:em;">[</span><span class="normal">e</span><sup>s t</sup>
F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>,
s<sub>i</sub><span class="paren" style="font-size:em;">]</span></div>
有理式に対しても、この式を使えばものすごく簡単に逆ラプラス変換を求められるような感じがしますが、
有理式に対する留数計算も結局の所、部分分数分解を用いて求めたりするので、
先ほど説明した公式による手法とかかる手間はあまり変わらなかったりします。


## <a id="sec-generated-title-14"></a> <a id="differential"></a>微分方程式への応用

ラプラス変換を用いることで、線形時不変常微分方程式を簡単に解くことができます。
 
線形時不変常微分方程式は、一般に以下のように書き表されます。
<div class="math">
      <span class="paren" style="font-size:3em;">(</span>
        <table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ 1</td></tr></table>
 a<sub>i</sub><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>i</sup><span class="paren" style="font-size:3em;">)</span>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
これは、
<span class="math">
H<span class="paren" style="font-size:em;">(</span>D<span class="paren" style="font-size:em;">)</span> ＝ 
 <table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ 1</td></tr></table>
 a<sub>i</sub>
 D<sup>i</sup></span>
と置いて、
<div class="math">
H<span class="paren" style="font-size:em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:em;">)</span> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
と書き表せます。
ラプラス変換の性質から、
この式を両辺それぞれラプラス変換すると、以下の式が得られます。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> － F<sub>0</sub><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＝ X<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></div>
ただし、<span class="math">F, X</span> はそれぞれ <span class="math">f, x</span> をラプラス変換したもの、
<span class="math">F<sub>0</sub></span> は <span class="math">f</span> の初期値によって定まる関数です。
したがって、<span class="math">F</span>、<span class="math">x</span> および <span class="math">f</span> の初期値が与えられたとき、
<div class="math">
F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>  ＝
<table class="frac" summary="fraction"><tr><td class="num">X<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＋ F<sub>0</sub><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></td></tr><tr><td>H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></td></tr></table></div>
という式で <span class="math">F</span> を求めることができ、
さらにこれを逆ラプラス変換することで <span class="math">f</span> が求まります。


## <a id="sec-generated-title-15"></a> <a id="stability"></a>安定性

線形時不変微分方程式の解 <span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> は、基本的に
<div class="math">
c t<sup>n</sup><span class="normal">e</span><sup>s<sub>0</sub> t</sup></div>
という形（の関数の線形結合）になります。
<span class="math">c, s<sub>0</sub></span> は複素数の定数です。
 
あるいは、<span class="math">s<sub>0</sub> ＝ σ<sub>0</sub> ＋ i ω<sub>0</sub></span> と置くと、
<div class="math">
t<sup>n</sup><span class="normal">e</span><sup>σ<sub>0</sub> t</sup><span class="paren" style="font-size:em;">(</span>
a <span class="normal">cos</span>ω<sub>0</sub> t
＋
b <span class="normal">sin</span>ω<sub>0</sub> t
<span class="paren" style="font-size:em;">)</span></div>
と表せます。
 
ここで、時間が経過する（t が増加する）につれこの式がどうなるかを考えてみましょう。
まあ、式を見れば分かると思いますが、<span class="math">σ<sub>0</sub></span> の正負によって結果が変わります。

* <span class="math">σ<sub>0</sub></span>が正のとき、発散する。

* <span class="math">σ<sub>0</sub></span>が負のとき、0 に収束する。

* <span class="math">σ<sub>0</sub> ＝ 0</span>（かつ<span class="math">n ＝ 0</span>）のとき、sin, cos の項が残る。


解が発散するとき（<span class="math">σ<sub>0</sub></span> が正のとき）、
解が<em>不安定</em>であるといい、
それ以外のときは<em>安定</em>であるといいます。
解が発散すると非常にまずいので、安定性の解析は重要な問題になります。
 
また、解が安定な場合でも、最後まで 0 にならずに残る sin, cos の項を<em>定常解</em>と呼び、
それ以外の部分を<em>過渡解</em>と呼びます。
（分野によっては定常応答、過渡応答と呼ぶ。）
<span class="math">s<sub>0</sub> ＝ σ<sub>0</sub> ＋ i ω<sub>0</sub></span> の
実部 <span class="math">σ<sub>0</sub></span> は過渡解を表す部分であり、
虚部 <span class="math">ω<sub>0</sub></span> は定常解を表す部分であるといえます。
 
さて、ここでラプラス変換に話を戻します。
<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> をラプラス変換した伝達関数 <span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span>
が点 <span class="math">s<sub>0</sub></span> に
n 位の「[極](../../math/analysis/residue.md#pole)」を持っていると、
<span class="math">c t<sup>n</sup><span class="normal">e</span><sup>s<sub>0</sub> t</sup></span>
という項が現れます。
先ほどの安定性の話とあわせて考えると、
伝達関数の極を調べれば解の安定性が分かることになります。
すなわち、以下のようなことが言えます。

* 伝達関数が複素平面の右半面（実部が正）の範囲に極を1つでも持つとき、解は不安定。

* 伝達関数の全ての極が左半面（実部が負）のとき、解は定数の収束する。

* 伝達関数の極が虚軸上（実部が 0）にあるとき、定常応答（sin, cos）が現れる。



## <a id="sec-generated-title-16"></a> <a id="spectrum"></a>周波数特性

最初に述べたように、
計算上、ラプラス変換は「[フーリエ変換](fourier.md#f-trans)」の <span class="math">iω</span> を
<span class="math">s</span> で置き換えたものです。
逆に言うと、ラプラス変換の結果得られた伝達関数 <span class="math">F<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> の
<span class="math">s</span> の部分に <span class="math">iω</span> を代入したもの
<span class="math">F<span class="paren" style="font-size:em;">(</span>iω<span class="paren" style="font-size:em;">)</span></span>
はシステムの周波数特性になります。
 
では、この「<span class="math">s</span> を <span class="math">iω</span> で置き換える」という操作は一体どういう意味を持つのかを考えて見ましょう。
ラプラス変数 <span class="math">s ＝ σ ＋ iω</span> の実部 <span class="math">σ</span> はシステムの過渡解を、
虚部 <span class="math">iω</span> はシステムの定常解を表すものでした。
「<span class="math">s</span> を <span class="math">iω</span> で置き換える」というのは、
「実部 <span class="math">σ</span> を 0 にする」ことに相当し、
「過渡解を無視する」ということになります。
（したがって、フーリエ変換では過渡解や安定性の解析はできなくなります。）
 
前節で説明したように、伝達関数が安定（伝達関数の極の実部が全て負）な場合、
過渡解は 0 に収束します。
すなわち、十分な時間が経過すると、定常項のみが残ると考えて差支えがないということです。
これらのことから、以下のようなことが言えます。

* ラプラス変換はシステムの安定性や過渡解の解析に用いる。

* 安定性が保証され、かつ、過渡解が無視できる場合、<span class="math">s ＝ iω</span>と置くことで伝達関数から周波数特性が得られる。



## <a id="sec-generated-title-17"></a> <a id="plan"></a>執筆予定

<pre>
s 平面上の安定な領域を図示。


微分方程式への応用に具体例を追加したい。


最終値の定理とかも追加。


定数係数 線形 時不変 という言葉の意味も説明しときたい。
微分方程式の説明ページを別に作った方がいいかも。
→ システムのページに説明を書いたんで、ref をつける。
</pre>
