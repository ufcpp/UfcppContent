---
title: "線形位相と最小位相"
source_url: "https://ufcpp.net/study/sp/dsp/phase/"
content_type: "Article"
published_at: "2015-05-06T14:22:11"
updated_at: "2015-05-06T14:22:11"
tags: []
umbraco_id: 1607
parent_id: 1599
sort_order: 7
aliases:
  - "/dsp/phase"
  - "/dsp/phase.html"
  - "/sp/dsp/phase/"
  - "/study/dsp/phase"
  - "/study/dsp/phase.html"
---

# 線形位相と最小位相

##<a id="sec-generated-title-1"></a> <a id="pahse-variation"></a>位相の変化
<pre>
・振幅は同じだけど位相の異なる2つの信号
時間領域で波形を見ると、全然違う見た目になるけど、
例えば音として聞くと同じように聞こえたりする。
(少なくとも定常音の場合、かなり聞き分けは困難。)

↓
・分野によっては位相はあまり重要な意味を持たない。
(振幅特性と比べると重要性が低いけど、全く意味がないわけではない。
 音にしても、定常音の場合位相のずれの聞き分けは難しいけど、
 音の立ち上がりの部分の位相差は結構分かる。)

こういう分野では、フーリエ変換後、位相特性はあまり使わず、
振幅特性だけを見ることが多い。

振幅特性だけが重要な場合、
位相はどうしておくのがいいかというと、
できる限り遅延が少ない ＝ できる限り位相が小さい
方が好まれる。
→ 最小遅延。


・逆に、元の波形を崩したくない ＝ 位相を保ちたい分野もある。
例えば、画像なんかでは位相がずれると画像の見た目が崩れて使い物にならない。

こういう分野では、位相の線形性（周波数によらず遅延が一定）を保証する必要がある
→ 線形位相。

あるいは、フーリエ変換の亜種の、コサイン変換やサイン変換というものを使う。
コサイン変換では、そもそも位相を得られないし、変化させれない。
</pre>

##<a id="sec-generated-title-2"></a> <a id="allpass"></a>オールパス特性
<pre>
「振幅は同じだけど位相の異なる2つの信号」を作ろうと思ったら、
「振幅特性が周波数によらず1で固定で、位相だけ変化する特性」を考えればいい。
こういう特性を持つフィルタを作って、それに信号を通せば
振幅を変えずに位相だけ変えれる。

このような、
「振幅特性が周波数によらず1で固定で、位相だけ変化する特性」を
オールパス特性と呼ぶ。
全ての周波数帯域の信号の振幅を減衰させずに通すという意味。

作り方は簡単で、
s 領域（アナログ）なら
s － a
-------
s ＋ a*
（a* は a の複素共役）
、
z 領域（ディジタル）なら
1 － αz^{-1}
-------------
α* － z^{-1}
という特性を作るだけ。

これが実際に、振幅特性＝1になっていることは、
s ＝ jω
または
z ＝ exp(jTω) ＝ cos Tω ＋ j sin Tω
を代入して絶対値を取ることで簡単に確認できる。

この事実から、
任意の伝達関数 g(s) または g(z) に対して、
(s － a) g(s) と (s ＋ a*) g(s)、
(1 － αz^{-1}) g(z) と (α* － z^{-1}) g(z) は
それぞれ振幅特性が等しく、位相だけが異なる伝達関数になる。
</pre>

##<a id="sec-generated-title-3"></a> <a id="plan"></a>線形位相
<strong id="linear_phase" class="keyword">線形位相</strong>
<pre>
周波数によらず遅延が一定ならば、位相は線形（φ ＝ kω … ωの1次式）になる。
遅延が一定ということは、
時間領域波形が崩れないということ。
↑
振幅特性が同じでも、位相がずれると時間領域における波形は崩れる。

振幅特性のみを再現すればいい場合、
位相の線形性は問題にならない。

逆に、時間領域波形を保つ必要がある場合、
位相は線形でなければならない。

位相が線形なシステムや伝達関数のことを線形位相であるという。

インパルス応答が左右対称なシステムは、線形位相を持つ。
∵
- 原点遇対称な信号の周波数特性は実関数
- T 時間シフト → exp jTω
↓
ある時刻 T を中心として遇対称な信号の周波数特性は、
実関数 × exp jTω ＝ 位相 Tω ＝ 線形位相
</pre>

##<a id="sec-generated-title-4"></a> <a id="plan"></a>最小位相
<strong id="minimum_phase" class="keyword">最小位相</strong>
<pre>
1. 最小位相とは何か

- 応答・逆応答がどちらも因果的
か
- 伝達関数の零点と極が全て安定
  (s 領域なら実部が負、z 領域なら単位円内に入っている)

↑
ちなみに、この2つの定義は同値。
安定な極を持つ特性
1/(1 － bz^-1)       (b は |b|＜1)
は
Σ_{k=0}^∞ (bz^-1)^k
と展開できて、z の正の冪（＝時間進み）を含まないので因果的。

伝達関数の零点も極もどちらも安定なら、
応答も逆応答も安定で、したがって、応答も逆応答も因果的。


2. 位相が最小とはどういうことか

同じ振幅応答を持つ信号の中では、
最小位相フィルタが一番位相が小さく、
信号のエネルギーが時刻 0 付近に集まる

位相というか、正確には群遅延特性が最小らしい。

「信号のエネルギーが時刻 0 付近に集まる」ということに関しては、
このページ末尾の「[付録 最小位相特性のエネルギー](/study/sp/dsp/phase?sec=apendix)」を参照。

この結果、
- 遅延が少ないシステムが作れる。
- FIR フィルタで実現する場合、少ない次数でよい特性近似ができる。


3. どうやって最小位相化するか

最小位相 … 最初に述べた定義では、
応答・逆応答共に安定 ＝ 零点も極も Re(s)＜0 or |z|＜1

で、このとき、実は、
応答・逆応答ともに因果的になる。
→ 最小位相 ＝ 応答・逆応答ともに因果的


因果的な信号 a(t) があったとして、その周波数特性を A(ω) とします。
すると、
exp(A(ω)) で作られる周波数応答は最小位相になります。

A が因果的 → －A も因果的
A が因果的 → A の冪級数も因果的 → exp(A) も因果的
－A が因果的 → exp(－A) ＝ 1/exp(A) ＝ 逆応答も因果的
という感じ。

で、周波数特性 X(ω) に対して、
X ＝ exp(log|X| ＋ j arg(X))
なわけですが、
exp の中身の実部は log|X| のままで、因果的になるような信号を作れば、
振幅特性は |X| のままで、最小位相を持つ周波数特性が得られます。

因果的な信号をフーリエ変換すると、
実部 R(ω) と虚部 I(ω) の間に
I ＝ R のヒルベルト変換
という関係が成り立ちます。
したがって、
振幅特性 |X| の信号の最小位相というのは、
log|X| のヒルベルト変換で求まります。

ヒルベルト変換なんですが、
- 実部だけの信号を逆 FFT する
- 逆変換結果の、負の時刻に当たる部分を全て 0 で埋める
- FFT で周波数領域に戻す
という操作をすると、
実部は元のままで、虚部には実部のヒルベルト変換結果が得られます。

要するに、最小位相は
- log|X| を IFFT
- IFFT 結果の負の時刻の部分を全て 0 で埋める
  (実際には、離散でやるので、時系列の後ろ半分を 0 にする)
- FFT
として、虚部に現れる信号が最小位相です。

(計算手順的にはケプストラム解析に似ている。)
</pre>

##<a id="sec-generated-title-5"></a> <a id="apendix"></a>付録
###<a id="sec-generated-title-6"></a> <a id="apendix"></a>最小位相特性のエネルギー
最小位相特性のエネルギーが「時刻 0 付近に集まる」というのは、
任意の時刻 <span class="math">k ＞ 0</span> に対して、
<div class="math">
        <table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table>
        <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span>
        <sup>2</sup>
≧
<table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup></div><div class="math">
h<sub>m</sub><span class="normal"> … 最小位相特性を持つ時系列</span></div><div class="math">
h <span class="normal"> … </span>h<sub>min</sub><span class="normal"> と同じ振幅を持つが、非最小位相な時系列</span></div>
という意味です。

<span class="math">h<sub>m</sub></span> と <span class="math">h</span> は、
振幅特性が同じなので、パーセバルの定理から、
<div class="math">
        <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table>
        <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span>
        <sup>2</sup>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup></div>
という等式が成り立ちます。
上述の不等式は、
この等式のうち <span class="math">k ＞ i</span> の範囲の部分和だけを見ると、
<span class="math">k</span> によらず常に左辺の <span class="math">h<sub>m</sub></span> の方がエネルギーが大きくなるという意味です。
すなわち、 <span class="math">h<sub>m</sub></span> の方が時刻 0 付近にエネルギーが集まっているということになります。
<h4>証明</h4>
このことは、以下のような手順で証明できます。

<span class="math">h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span></span> を最小位相特性を持つ時系列信号、
<span class="math">H<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> をその「[Z 変換](z.md#z-trans)」とします。
すると、<span class="math">H<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> は、
別の、1次低い次数の最小位相特性
<span class="math">G<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span>
を用いて、
<div class="math">
H<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> ＝
 <span class="paren" style="font-size:em;">(</span>1 － αz<sup>－1</sup><span class="paren" style="font-size:em;">)</span>
 G<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>

　　<span class="paren" style="font-size:em;">(</span><span class="normal">|</span>α<span class="normal">|</span> ＜ 1<span class="paren" style="font-size:em;">)</span></div>
と表すことができます。
ここで、<span class="math">H<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> と同じ振幅特性を持つ
非最小位相特性として、以下のようなものを考えます。
<div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> ＝
 <span class="paren" style="font-size:em;">(</span>α<sup>*</sup> － z<sup>－1</sup><span class="paren" style="font-size:em;">)</span>
 G<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></div>
このような特性 <span class="math">H</span> を持つ信号に対して証明すれば、
帰納的な議論により、任意の非最小位相特性について定理を証明できます。
なので、この特性 <span class="math">H</span> についてのみ証明を与えます。
 
上式のような形で表された
<span class="math">H<sub>m</sub></span> および <span class="math">H</span> を時間領域に戻すと、
（<span class="math">h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span></span> を <span class="math">H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> の時系列信号、
同様に<span class="math">g<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span></span> を <span class="math">G<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></span> の時系列信号として）
<div class="math">
h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span>
 ＝
 g<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span> － α g<span class="paren" style="font-size:em;">[</span>i － 1<span class="paren" style="font-size:em;">]</span></div><div class="math">
h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span>
 ＝
 α<sup>*</sup>g<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span> － g<span class="paren" style="font-size:em;">[</span>i － 1<span class="paren" style="font-size:em;">]</span></div>
となります。
これらに対して、以下の式を計算します。
<div class="math">
      <table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table>
      <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span>
      <sup>2</sup>
－
<table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>
 g<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span> － α g<span class="paren" style="font-size:em;">[</span>i － 1<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup>
－
<table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>α<sup>*</sup>g<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span> － g<span class="paren" style="font-size:em;">[</span>i － 1<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup></div>
これを展開してまとめなおすと、
<div class="math">
      <table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table>
      <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span>
      <sup>2</sup>
－
<table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup>
＝
<span class="paren" style="font-size:em;">(</span>1 － <span class="normal">|</span>α<span class="normal">|</span><sup>2</sup><span class="paren" style="font-size:em;">)</span><span class="normal">|</span>g<span class="paren" style="font-size:em;">[</span>k<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup></div>
となり、
<span class="math"><span class="normal">|</span>α<span class="normal">|</span> ＜ 1</span> という条件から、
この式は常に正になります。
したがって、
<div class="math">
      <table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table>
      <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span>
      <sup>2</sup>
≧
<table class="sigma" summary="sum"><tr><td class="sigmasub">k</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i＝0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">[</span>i<span class="paren" style="font-size:em;">]</span><span class="normal">|</span><sup>2</sup></div>
が示されました。
<h4>連続信号の場合</h4>
連続信号に対しても同様の証明が可能です。
連続信号の場合は、先ほどの式中の Σ が積分に置き換わり、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> x</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
      <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
      <span class="normal">d</span>t
≧
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> x</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup><span class="normal">d</span>t
</div>
となります。
証明も同様に、
<div class="math">
H<sub>m</sub><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span> ＝
 <span class="paren" style="font-size:em;">(</span>s － a<span class="paren" style="font-size:em;">)</span>
 G<sub>m</sub><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>

　　<span class="paren" style="font-size:em;">(</span><span class="script">Re</span>α ＜ 0<span class="paren" style="font-size:em;">)</span></div><div class="math">
H<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> ＝
 <span class="paren" style="font-size:em;">(</span>s ＋ a<sup>*</sup><span class="paren" style="font-size:em;">)</span>
 G<sub>m</sub><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span></div><div class="math">
h<sub>m</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
 ＝
 g'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> － a g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div><div class="math">
h<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
 ＝
 g'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＋ a<sup>*</sup> g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
等と置くと、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> x</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
      <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
      <span class="normal">d</span>t
－
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> x</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup><span class="normal">d</span>t
＝
－<span class="script">Re</span>a <span class="normal">|</span>g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup></div>
という式を導くことができ、
<span class="math"><span class="script">Re</span>α ＜ 0</span>
であることから、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> x</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>
      <span class="normal">|</span>h<sub>m</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span>
      <sup>2</sup>
      <span class="normal">d</span>t
≧
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> x</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table><span class="normal">|</span>h<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">|</span><sup>2</sup><span class="normal">d</span>t
</div>
が示されます。
