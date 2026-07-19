---
title: "超関数"
source_url: "https://ufcpp.net/study/math/distribution/distribution-e_distribution/"
content_type: "Article"
published_at: "2015-05-06T14:18:11"
updated_at: "2015-05-18T17:21:21"
tags: []
umbraco_id: 1510
parent_id: 1509
sort_order: 0
aliases:
  - "/distribution/distribution"
  - "/distribution/distribution.html"
  - "/math/distribution/distribution-e_distribution/"
  - "/math/distribution/e_distribution"
  - "/study/distribution/distribution"
  - "/study/distribution/distribution.html"
  - "/study/math/distribution/e_distribution"
---

# 超関数

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

簡単にまとめます。
詳細はいずれ。

<strong id="distribution" class="keyword">超関数</strong>は、関数を拡張した概念で、大雑把な言い方をすると以下のような特徴があります。

* 可算個の点で無限大の値を持つ物も定義できる。

* 滑らかでない関数でも無理やり微分できる。



## <a id="sec-generated-title-2"></a> <a id="idea"></a>基本アイディア

「無限大の値」なんてものは実際には存在しないので、
2つの超関数の等値性を以下のように定義します。
 
「2つの超関数<span class="math">f, g</span>が、任意の区間<span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>で
<span class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝ <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t</span>
という関係が成り立っているとき、互いに等しいものとする。」
 
このように定義することで、ある点で無限大の値を持つ（要するに、値が発散する）関数でも、積分値さえ有限ならばちゃんとした意味を持つことになります。
 
ただし、このように定義したことによって、関数としては異なるものであっても、超関数としては同じものになってしまうこともあります。
例えば、以下の2つの関数<span class="math">f, g</span>は関数論的には相異なるものですが、超関数論的には同じものになります。
（このような関数以外にも、可算無限個の点で異なる値を持つ2つの関数は超関数論的には同等。）
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ t
</div><div class="math">
g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">t</span>  </td><td><span class="paren">(</span><span class="math">t≠0</span><span class="paren">)</span></td></tr><tr><td><span class="math">1</span>  </td><td><span class="paren">(</span><span class="math">t＝0</span><span class="paren">)</span></td></tr></table></div>

## <a id="sec-generated-title-3"></a> <a id="dirac"></a>ディラックのδ関数

超関数の中で最も有名なものとして、ディラックの<strong id="delta" class="keyword">δ関数</strong>があげられます。
δ関数は、（口語的に述べると）以下のような性質を持つ超関数です。

* <span class="math">δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>は<span class="math">t≠0</span>のとき0。

* <span class="math">δ<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></span>は無限大。


「[基本アイディア](#idea)」で述べた定義を使ってこの言葉を置き換えると、
δ関数<span class="math"><span class="math">δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span></span>は以下の条件を満たすような超関数になります。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">1</span>  </td><td><span class="paren">(</span><span class="math">a＜0＜b</span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">otherwise</span></span><span class="paren">)</span></td></tr></table></div>
δ関数は、通常の関数の極限として定義することも出来ます。
定義の仕方は1通りではありませんが、以下に代表的なものをいくつか列挙します。
<div class="math">
δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">σ→0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2πσ</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－<table class="frac" summary="fraction"><tr><td class="num">x<sup>2</sup></td></tr><tr><td>σ<sup>2</sup></td></tr></table><span class="paren" style="font-size:2em;">)</span></div><div class="math">
δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">ε→0</td></tr></table><span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>ε</td></tr></table></span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">|</span>t<span class="normal">|</span>＜<table class="frac" summary="fraction"><tr><td class="num">ε</td></tr><tr><td>2</td></tr></table></span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math"><span class="normal">otherwise</span></span><span class="paren">)</span></td></tr></table></div><div class="math">
δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝
<table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>2π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:1em;">(</span>iωt<span class="paren" style="font-size:1em;">)</span><span class="normal">d</span>t ＝
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">ω→∞</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span>ωt</td></tr><tr><td>πω</td></tr></table></div>
1つ目は正規分布の密度関数に対して分散を0に限りなく近づけたもの、
2つ目は面積1の矩形を限りなく細くしていったもの、
3つ目はフーリエ変換を用いてδ関数を表したものになっています。


## <a id="sec-generated-title-4"></a> <a id="d75e251"></a>厳密な定義

詳細は省略します（余裕があれば追加します）。
いくつか異なる定義の仕方がありますが、
詳しく知りたい人は以下のキーワードで検索してみてください。

* Schwartzの超関数（分布: distribution）

* 超分布（ultradistribution）

* 佐藤の超関数（hyper function）


ここでは、この中では比較的理解の容易な Schwartz の超関数について概説します。
大雑把に説明すると以下のような感じで定義します。

* （導関数が滑らかでないものも含め）関数を無理やり微分出来るように微分演算を拡張する。

* 線形な汎関数（実連続関数→実数への写像）として定義する。


1つ目の方法に関しては、例えば、
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">t<sup>2</sup></span>  </td><td><span class="paren">(</span><span class="math">t≧0</span><span class="paren">)</span></td></tr><tr><td><span class="math">－t<sup>2</sup></span>  </td><td><span class="paren">(</span><span class="math">t＜0</span><span class="paren">)</span></td></tr></table></div>
とすると（この<span class="math">f</span>は滑らか）、その導関数は形式的に以下のようになります。
<div class="math">
f'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ 2<span class="normal">|</span>t<span class="normal">|</span></div><div class="math">
f''<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ 4h<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> - 2
</div><div class="math">
f'''<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span> ＝ 4δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
（ただし、<span class="math">h<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>、<span class="math">δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>はそれぞれヘヴィサイドの単位階段関数およびディラックのδ関数。）
このような微分が許容されるように、微分演算を拡張してしまおうというのが1つ目の方法の発想です。
 
一方、2つ目の方法に関しては、例えば、
任意の関数<span class="math">φ</span>に対して、
<div class="math">
T<span class="paren" style="font-size:em;">(</span>f<span class="paren" style="font-size:em;">)</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－∞</td></tr></table>φ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div>
というような汎関数（関数<span class="math">f</span>から実数値<span class="math">T<span class="paren" style="font-size:em;">(</span>f<span class="paren" style="font-size:em;">)</span></span>を得る関数）が定義できます。
この逆の発想で、先に汎関数<span class="math">T</span>を定義して、それに対応する関数<span class="math">φ</span>を形式的に考えることで、
通常の関数の概念を拡張しようというのが2つ目の方法です。
（この発想が Schwartz の超関数。）
 
例を挙げると、
<div class="math">
H<span class="paren" style="font-size:em;">(</span>f<span class="paren" style="font-size:em;">)</span> = <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> ∞</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div><div class="math">
D<span class="paren" style="font-size:em;">(</span>f<span class="paren" style="font-size:em;">)</span> = f<span class="paren" style="font-size:em;">(</span>0<span class="paren" style="font-size:em;">)</span></div>
というような汎関数<span class="math">H, D</span>がそれぞれヘヴィサイドの単位階段関数およびディラックのδ関数になります。


## <a id="sec-generated-title-5"></a> <a id="plan"></a>執筆予定

<pre>
・δ関数
関数の極限として作れたりも。

∫δ(t - T)f(t)dt = f(T)
δ(at) = 1/|a| δ(t) とか。

・不連続な関数の微分
微分を拡張
↓
積分の性質として ∫f'φ ＝ [fφ] - ∫fφ' というのが
台がコンパクトでかつ無限回微分可能な関数φを使えば、
∫f'φ ＝ -∫fφ'
fが微分できなくても、-∫fφ' は定義可能。
これを f の微分の代わりに使う。

・台がコンパクトな関数の例
ガウス関数 G(x) = exp(－x^2)

A(x) = 0 (x＜0のとき), 1－G(x) (x≧0のとき)
この関数は無限階微分可能。
また、x＞0のとき常に正。

B(x) = ∫_－∞^x A(x)dx
同じく無限階微分可能で、
x＜0で0、x＞0で単調増加。

C(x) = B(x) × B(ε－x)
同じく無限階微分可能で、
(0, ε) でのみ非0(正)。

D(x) = ∫_－∞^x C(x)dx
x≦0 で 0
0＜x＜ε で単調増加
ε≦x で 定数。

D'(x) = D(x) / D(ε)
x≦0 で 0
0＜x＜ε で単調増加
ε≦x で 1。

E(x) = D'(x－(a－ε)) × D'(b＋ε－x)
区間(a, b)で1
(a－ε, a), (b, b＋ε)で連続に単調減少
それ以外で0
しかも、無限階微分可能。

・線形汎関数として定義

F(f) ＝ ∫φfdt
で distribution を定義。
汎関数 → 関数空間の双対空間

φが超関数というよりは、∫dtφ の部分が超関数。
なので、実は関数の一般化というよりは、測度の一般化だったりする。

そのせいで、個々の点における個性は失ってる。
可算個の点で異なる値を持つ2つの関数は、Schwartzの超関数的には区別が付かない。


・佐藤の超関数

こっちは英語でも hyper function。

佐藤幹夫氏は Schwartz の超関数が個々の点において個性を失う
という事実に納得がいかなかったようで、
個々の点における個性を持ちつつ、無限大の値を取れるような
本当の意味で関数の上位概念になるような理論を構築したかったらしい。
その結果生まれたのが hyper function。

Schwartz 超関数だと、実軸上だけで孤立特異点の性質を見ているので、
∫_a^b fφdt
といように、幅を持った区間で積分しないと超関数の性質が見えてこない。
これが、個々の点で個性を失う原因。

で、複素数平面にまで視野を広げてやると、
留数定理やローラン展開なんかを思い出してもらえば分かるように、
孤立特異点を囲む任意の閉路での積分によって、
孤立特異点の性質を調べることが出来る。

∮_C f dt
C は孤立特異点を1つだけ囲んでいるという条件さえ満たせばどんな経路でもいい。
無限に小さな経路でもOK。

この発想から出発して、個々の点における個性も持ちつつ、
無限大の値を持つ微分可能な関数の上位概念が作れるんじゃないか
というのが佐藤の超関数。
</pre>
