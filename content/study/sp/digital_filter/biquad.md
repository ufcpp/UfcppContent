---
title: "双2次フィルタ"
source_url: "https://ufcpp.net/study/sp/digital_filter/biquad/"
content_type: "Article"
published_at: "2004-03-19T00:00:00"
updated_at: "2015-05-06T14:22:36"
tags: []
umbraco_id: 1616
parent_id: 1610
sort_order: 5
aliases:
  - "/study/digital_filter/biquad.html"
---

# 双2次フィルタ

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

分母・分子がともに2次のIIRフィルタを双2次フィルタと呼びます。
双2次フィルタは、以下のような理由から、非常によく利用されています。

* 単純。

* 設計手法が確立している。

* 直列に繋ぐことでさまざまな特性を作ることができる。



## <a id="sec-generated-title-2"></a> <a id="lcr"></a>LCR 回路

以下のような LCR 回路でローパスフィルタが作れます。

<figure>

[![LCR 回路の例](../../../../assets/media/ufcpp2000/sp/biquad01.png)](../../../../assets/media/ufcpp2000/sp/biquad01.png)

<figcaption>LCR 回路の例</figcaption>
</figure>


この回路の伝達特性 <span class="math">T<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> は、
<div class="math">
T<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＝
<table class="frac" summary="fraction"><tr><td class="num">V<sub>1</sub></td></tr><tr><td>V<sub>2</sub></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">1 / sC</td></tr><tr><td>R + sL + 1 / sC</td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">1 / LC</td></tr><tr><td>s<sup>2</sup> ＋ <span class="paren" style="font-size:em;">(</span>R / L<span class="paren" style="font-size:em;">)</span> s + 1 / LC</td></tr></table></div>
となります。
ここで、
<span class="math">ω<sub>0</sub> ＝ <span class="normal" style="font-size:2em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>LC</td></tr></table></span></span>
（無損失（<span class="math">R ＝ 0</span>）時の共振周波数）、
<span class="math">Q ＝ <table class="frac" summary="fraction"><tr><td class="num">L</td></tr><tr><td>R</td></tr></table> ω<sub>0</sub>
 ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>R</td></tr></table><span class="normal" style="font-size:2em;">√</span><span class="bar"><table class="frac" summary="fraction"><tr><td class="num">L</td></tr><tr><td>C</td></tr></table></span></span>
（クオリティファクタ（LCR 回路でコイルの損失率を表すパラメータ））
と置くと、
<div class="math">
T<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">ω<sub>0</sub><sup>2</sup></td></tr><tr><td>s<sup>2</sup> ＋ <span class="paren" style="font-size:em;">(</span>ω<sub>0</sub> / Q<span class="paren" style="font-size:em;">)</span> s + ω<sub>0</sub><sup>2</sup></td></tr></table></div>
と表すことができます。
この伝達関数の周波数特性は、
<span class="math">ω<sub>0</sub></span> を境にして減衰を始めるローパス特性になっています。
すなわち、このような RCL 回路を用いて、ローパスフィルタを作ることが出来ます。
ちなみに、クオリティファクタ <span class="math">Q</span> を変えると、カットオフ特性のなだらかさなどが変化します。
 
ここではローパスフィルタを例に挙げましたが、
RCL の配置をいろいろと変えることで、さまざまな特性を作ることができます。


## <a id="sec-generated-title-3"></a> <a id="biquad"></a>双2次フィルタ

伝達関数の分母・分子ともに2次のフィルタを<strong id="d33e162" class="keyword">双2次フィルタ</strong>（biquadratic filter、あるいは biquad filter）といいます。
（余談ですが、quad- という接頭語は“4”という意味を表します。
quadratic は“四角形の”という意味合いから“2次元の”という意味で使われる言葉です。
ついでに、bi- は“2”を表す接頭語です。
biquadratic は文字通り解釈すると、“2つの2次元”なわけですが、“4次元の”という意味と、“分母・分子ともに2次元”という意味で使われることがあります。
4（quad）なのに2次、
2×4（biquad）でもやっぱり2次だったり。）
 
先ほど示したように、RCL 回路を用いてアナログの双2次フィルタを作ることができます。
分母・分子ともに2次という制限の元でも結構いろんなことができます（図2）。
また、双2次フィルタの組み合わせ（直列に繋ぐ）によって、より複雑な特性を作り出すことも出来ます。

<figure>

[![双2次元フィルタの特性いろいろ](../../../../assets/media/ufcpp2000/sp/biquad02.png)](../../../../assets/media/ufcpp2000/sp/biquad02.png)

<figcaption>双2次元フィルタの特性いろいろ</figcaption>
</figure>


フィルタ設計が容易であることや、設計手法が確立されたものであることなどから、
双2次フィルタは非常によく利用されています。
 
Robert Bristow-Johnson という方が、
ネット上で（オーディオ用に）双2次フィルタの設計時に使う公式をまとめたものを公開しています
（通称、RBJ Audio-EQ-Cookbook、参考 URL: http://www.harmony-central.com/Computer/Programming/Audio-EQ-Cookbook.txt）。
以下、この RBJ Cookbook の内容について簡単に説明します（和訳しただけの部分も多いですが）。
<h4>設計手法概要</h4>
双2次フィルタの設計では、
まず、アナログの双2次伝達関数を作ります。
このとき、フィルタの特性周波数を 1 として伝達関数を作ります。
そして、「[周波数変換](transform.md)」で説明したような手法を用いて、周波数変換およびアナログ→ディジタル変換を行います。
双1次変換を用いてアナログ→ディジタル変換を行うと、
フィルタの次数は変わらないので、ディジタルの双2次フィルタを作ることができます。
 
双2次フィルタを設計する際には、以下のようなパラメータを用います。

* <span class="math">F<sub>s</sub></span>… ディジタル信号のサンプリング周波数。

* <span class="math">f<sub>0</sub></span>… 特性周波数。要するに、ローパスフィルタならカットオフ周波数、ピーキングフィルタなら中心周波数。

* <span class="math">gain</span>… ピーキングフィルタやシェルフフィルタの利得[dB]。

* <span class="math">Q</span>… RCL 回路で説明したクオリティファクタに相当するパラメータ。ローパス・ハイパスフィルタのカットオフ特性や、ピーキングフィルタのピークの鋭さが変わる。


<span class="math">Q</span> に関しては、ピーキングフィルタではピークの幅 <span class="math">BW</span> を、シェルフフィルタではシェルフスロープに関するパラメータ <span class="math">S, a</span> を与えて <span class="math">Q</span> を計算することも多いです。
 
また、中間的なパラメータとして、以下のようなものを用います。

* <span class="math">ω<sub>0</sub> ＝ 2 π f<sub>0</sub> / F<sub>s</sub></span>… 特性周波数を2πで正規化したもの。

* <span class="math">
          <span class="normal">sin</span> ω<sub>0</sub> , <span class="normal">cos</span> ω<sub>0</sub></span>… 下の説明参照

* <span class="math">α ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span> ω<sub>0</sub></td></tr><tr><td>Q</td></tr></table></span>

* <span class="math">A ＝ <span class="normal" style="font-size:em;">√</span><span class="bar">10<sup>gain / 20</sup></span></span>… ピーキングフィルタやシェルフフィルタの利得[dB]をリニア値にしたものの平方根。


以下のような式を用いると、周波数変換と双1次変換を同時に行うことができます。
<div class="math">
s
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">tan</span> ω<sub>0</sub>/2</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">1 － z<sup>－1</sup></td></tr><tr><td>1 ＋ z<sup>－1</sup></td></tr></table></div>
ただし、<span class="math">ω<sub>0</sub></span> は特性周波数 <span class="math">f<sub>0</sub></span>を正規化した角周波数です。
<span class="math"><span class="normal">tan</span><table class="frac" summary="fraction"><tr><td class="num">ω<sub>0</sub></td></tr><tr><td>2</td></tr></table></span> およびその2乗は、
<span class="math"><span class="normal">sin</span> ω<sub>0</sub> , <span class="normal">cos</span> ω<sub>0</sub></span> を使って書き換えることができるので、
中間パラメータとして <span class="math"><span class="normal">sin</span> ω<sub>0</sub> , <span class="normal">cos</span> ω<sub>0</sub></span> が頻繁に出てくることになります。
 
以下、具体的な双2次フィルタの話になりますが、
元となるアナログ伝達関数 <span class="math">H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span></span> と、
ディジタルフィルタの係数 <span class="math">a<sub>0</sub> , a<sub>1</sub> , a<sub>2</sub> , b<sub>0</sub> , b<sub>1</sub> , b<sub>2</sub></span> のみを示します。
係数の意味は以下の通りです。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> ＝
<table class="frac" summary="fraction"><tr><td class="num">b<sub>0</sub> ＋ b<sub>1</sub> z<sup>－1</sup> ＋ b<sub>2</sub> z<sup>－2</sup></td></tr><tr><td>a<sub>0</sub> ＋ a<sub>1</sub> z<sup>－1</sup> ＋ a<sub>2</sub> z<sup>－2</sup></td></tr></table></div>
ちなみに、
次節以降で結果だけ示す伝達関数の導出方法ですが、
例えばローパスフィルタなら、
<span class="math">f<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> 1, </span><span class="math">f'<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> 0, </span><span class="math">f<span class="paren" style="font-size:em;">(</span><span class="normal">∞</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span> 0, </span><span class="math">f'<span class="paren" style="font-size:em;">(</span><span class="normal">∞</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span> 1</span>
というような条件から係数を計算します。
<h4>ローパスフィルタ（low-pass filter）</h4>
低域透過フィルタ。
低周波数信号のみを通すフィルタ。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num">1 － <span class="normal">cos</span> ω<sub>0</sub></td></tr><tr><td>2</td></tr></table></div><div class="math">
b<sub>1</sub>
＝
1 － <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
b<sub>2</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num">1 － <span class="normal">cos</span> ω<sub>0</sub></td></tr><tr><td>2</td></tr></table></div><div class="math">
a<sub>0</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α
</div><h4>ハイパスフィルタ（high-pass filter）</h4>
高域透過フィルタ。
高周波数信号のみを通すフィルタ。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup></td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num">1 ＋ <span class="normal">cos</span> ω<sub>0</sub></td></tr><tr><td>2</td></tr></table></div><div class="math">
b<sub>1</sub>
＝
－<span class="paren" style="font-size:em;">(</span>1 ＋ <span class="normal">cos</span> ω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
b<sub>2</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num">1 ＋ <span class="normal">cos</span> ω<sub>0</sub></td></tr><tr><td>2</td></tr></table></div><div class="math">
a<sub>0</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α
</div><h4>バンドパスフィルタ（band-pass filter）</h4>
帯域透過フィルタ。
特定の帯域信号のみを通すフィルタ。
<span class="math">Q</span> によって透過域の利得が変わる（帯域幅一定）ものと、
帯域幅が変わる（利得は <span class="math">Q</span> で一定）のものがある。
 
帯域幅一定版
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span> ω<sub>0</sub></td></tr><tr><td>2</td></tr></table>
＝
Q α
</div><div class="math">
b<sub>1</sub>
＝
0
</div><div class="math">
b<sub>2</sub>
＝
－
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span> ω<sub>0</sub></td></tr><tr><td>2</td></tr></table>
＝
－Q α
</div><div class="math">
a<sub>0</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α
</div>
利得一定版
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
α
</div><div class="math">
b<sub>1</sub>
＝
0
</div><div class="math">
b<sub>2</sub>
＝
－α
</div><div class="math">
a<sub>0</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α
</div><h4>バンドストップフィルタ（band-stop filter）</h4>
帯域阻止フィルタ。
特定の帯域信号のみを通さないフィルタ。
ノッチ（notch）フィルタともいう。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup> ＋ 1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
1
</div><div class="math">
b<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
b<sub>2</sub>
＝
1
</div><div class="math">
a<sub>0</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α
</div><h4>オールパスフィルタ（all-pass filter）</h4>
全域透過フィルタ。
振幅特性はそのまま（全域透過、all-pass）で、
位相特性のみを変化させるフィルタ。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup> － <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
1 － α
</div><div class="math">
b<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
b<sub>2</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>0</sub>
＝
1 ＋ α
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α
</div><h4>ピーキングフィルタ（peaking filter）</h4>
振幅特性にピークやディップ（山や谷）を作るフィルタ。
周波数ごとの音量調整に使えるので、音響分野では特によく使われる（いわゆる、イコライザ）。
なので、分野によってはピーキングイコライザ（peaking equalizer）と呼ぶ。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num">A</td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>A Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
1 ＋ α A
</div><div class="math">
b<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
b<sub>2</sub>
＝
1 － α A
</div><div class="math">
a<sub>0</sub>
＝
1 ＋ α A
</div><div class="math">
a<sub>1</sub>
＝
－2 <span class="normal">cos</span> ω<sub>0</sub></div><div class="math">
a<sub>2</sub>
＝
1 － α A
</div>
あるいは、
<span class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num">A<sup>2</sup></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></span>
または
<span class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>A<sup>2</sup> Q</td></tr></table>s ＋ 1</td></tr></table></span>
というように、
<span class="math">A</span> を分母または分子に集中させる場合もある。
（通常、
<span class="math">A</span> が1以上（<span class="math">gain</span> [dB] が正）のとき分母に、
<span class="math">A</span> が1以下（<span class="math">gain</span> [dB] が負）のとき分子に集中させる。）
<h4>ローシェルフフィルタ（low-shelf filter, low-shelving filter）</h4>
振幅特性の低域に棚状の利得をかける。
ローブースト（low-boost）フィルタともいう。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
A
<table class="frac" summary="fraction"><tr><td class="num">s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal" style="font-size:em;">√</span><span class="bar">A</span></td></tr><tr><td>Q</td></tr></table>s ＋ A</td></tr><tr><td>A s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal" style="font-size:em;">√</span><span class="bar">A</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
A
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
－
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
＋
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
<span class="paren" style="font-size:em;">)</span></div><div class="math">
b<sub>1</sub>
＝
－2 A
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span>
－
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
b<sub>2</sub>
＝
A
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
－
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
－
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
<span class="paren" style="font-size:em;">)</span></div><div class="math">
a<sub>0</sub>
＝
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
＋
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
</div><div class="math">
a<sub>1</sub>
＝
2
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
a<sub>2</sub>
＝
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
－
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
</div><h4>ハイシェルフフィルタ（high-shelf filter, high-shelving filter）</h4>
振幅特性の高域に棚状の利得をかける。
ハイブースト（high-boost）フィルタともいう。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>
＝
A
<table class="frac" summary="fraction"><tr><td class="num">A s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal" style="font-size:em;">√</span><span class="bar">A</span></td></tr><tr><td>Q</td></tr></table>s ＋ 1</td></tr><tr><td>s<sup>2</sup> ＋ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal" style="font-size:em;">√</span><span class="bar">A</span></td></tr><tr><td>Q</td></tr></table>s ＋ A</td></tr></table></div><div class="math">
b<sub>0</sub>
＝
A
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
＋
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
<span class="paren" style="font-size:em;">)</span></div><div class="math">
b<sub>1</sub>
＝
2 A
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
b<sub>2</sub>
＝
A
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
＋
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
－
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
<span class="paren" style="font-size:em;">)</span></div><div class="math">
a<sub>0</sub>
＝
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
－
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
＋
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
</div><div class="math">
a<sub>1</sub>
＝
－2
<span class="paren" style="font-size:em;">(</span><span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span>
－
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub><span class="paren" style="font-size:em;">)</span></div><div class="math">
a<sub>2</sub>
＝
<span class="paren" style="font-size:em;">(</span>A ＋ 2<span class="paren" style="font-size:em;">)</span>
－
<span class="paren" style="font-size:em;">(</span>A － 2<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span> ω<sub>0</sub>
－
2 <span class="normal" style="font-size:em;">√</span><span class="bar">A</span> α
</div>
