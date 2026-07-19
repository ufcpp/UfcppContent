---
title: "共変ベクトルと反変ベクトル"
source_url: "https://ufcpp.net/study/math/manifold/m_vector/"
content_type: "Article"
published_at: "2015-05-06T14:18:26"
updated_at: "2015-05-18T17:33:31"
tags: []
umbraco_id: 1517
parent_id: 1515
sort_order: 1
aliases:
  - "/manifold/vector"
  - "/manifold/vector.html"
  - "/math/manifold/m_vector/"
  - "/study/manifold/vector"
  - "/study/manifold/vector.html"
---

# 共変ベクトルと反変ベクトル

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
詳しくは「[数学](../index.md)」を見てもらうことになりますが、
ベクトル解析は正規直交座標を用いて表す限り、非常に美しい理論体系です。
ところが、ベクトル解析は座標変換に弱く、
この美しさは正規直交座標以外の座標系を用いた瞬間に崩れてしまいます。
これは、微分演算子（<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table></span> 等）と座標変換との相性があまりよくないために起こる問題です。
 
直交座標系で美しく表せるのなら直交座標だけ使えばいいと思うかもしれませんが、
微分方程式は変数変換（座標変換）によって解きやすい形に式変形してから解くのが一般的で、
座標変換に強い理論が立てられるならそれに越したことはありません。
 
このような背景から生まれた理論が微分形式です。
微分形式は、ベクトル解析で表現できることを全て表現できるだけでなく、
座標変換に強く、
さらには任意の次元にまで拡張できる非常に美しい理論体系です。
 
多くの解説書では、微分形式の形式的定義を最初に述べ、
そこから出発して、微分形式がベクトル解析や積分方程式の理論をうまく内包していることを示しています。
この過程を初めて目にすると、
あまりにも美しく完成された理論に驚くかと思います。
しかしながら、どんなに美しい理論にも、
完成に至るまでには泥臭い過程があるもので、
理論を理解するためにはその泥臭い過程を知ることも重要です。
 
そこでここでは、微分形式の理論に至るまでの泥臭い過程から説明したいと思います。


##<a id="sec-generated-title-2"></a> <a id="notation"></a>記法に関して
ここでは、座標を <span class="math">u, v</span> 等で表します。
これは、太字で書かれてはいませんが、ベクトルを表しています。
<span class="math">N</span> 次元の座標 <span class="math">u</span> を
<span class="math">u ＝ <span class="paren" style="font-size:em;">(</span>u<sub>1</sub> , u<sub>2</sub> , ・・・, u<sub>N</sub><span class="paren" style="font-size:em;">)</span></span>
あるいは
<span class="math">u ＝ <span class="paren" style="font-size:em;">(</span>u<sup>1</sup>, u<sup>2</sup>, ・・・, u<sup>N</sup><span class="paren" style="font-size:em;">)</span></span>
と言うように表します。
添字は 1 から始めます。
（<span class="math">N － 1</span> と言うように <span class="math">－1</span> を書くのが面倒なため。）
後者は添字が上に書かれていますが、冪ではありません。
添字を上に書くのと下に書くのの違いは後々説明します。
 
また、微分形式の理論では、
<span class="math">
df
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i ＝ 1</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    x<sup>i</sup>
  </denom></td></tr></table>
f
dx<sup>i</sup></span>
というように、積和の形で表される式が頻繁に現れます。
そこで、簡略化のために、
「左右の辺の片側に2つ同じ添字の付いた変数があった場合、
その添字に関して和を取る」
という省略記法があり、
<strong id="einstein" class="keyword">アインシュタインの記法</strong>（Einstein notation）と呼ばれます。
例えば、先ほどの式は
アインシュタインの記法を用いると
<span class="math">
df
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    x<sup>i</sup>
  </denom></td></tr></table>
f
dx<sup>i</sup></span>
と表されます。
先ほどとの違いは Σ が省略されただけですが、
多重に積和を取るような場合には Σ を書くだけでもずいぶん手間がかかるので、
これを省略します。
例えば、Σ が3重に付いたような式
<span class="math">
v<table class="subsup" summary="sub / sup"><tr><td>k</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>i, j</td></tr></table>
＝
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">l ＝ 1</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">m ＝ 1</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n ＝ 1</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂u<sup>l</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂u<sup>m</sup></td></tr><tr><td>∂v<sup>j</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂v<sup>k</sup></td></tr><tr><td>∂u<sup>n</sup></td></tr></table>
u<table class="subsup" summary="sub / sup"><tr><td>n</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>l, m</td></tr></table></span>
は
<span class="math">
v<table class="subsup" summary="sub / sup"><tr><td>k</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>i, j</td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>l</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂u<sup>m</sup></td></tr><tr><td>∂v<sup>j</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂v<sup>k</sup></td></tr><tr><td>∂u<sup>n</sup></td></tr></table>
u<table class="subsup" summary="sub / sup"><tr><td>n</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>l, m</td></tr></table></span>
と略記します。


##<a id="sec-generated-title-3"></a> <a id="conversion"></a>座標変換
まず、座標変換について説明します。

<span class="math">N</span> 次元の座標 <span class="math">u</span> で表されている関数や微分方程式等の式を、
別の座標 <span class="math">v</span> で表すことを考えてみます。
<span class="math">u</span> と <span class="math">v</span> の間の関係は、
<span class="math">v ＝ v<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span></span> と表されているものとしましょう。
<span class="math">v<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span></span> は <span class="math"><span class="bold">R</span><sup>N</sup> → <span class="bold">R</span><sup>N</sup></span> の「[同相写像](../set/topology.md#homeomorphism)」で、
微分可能性に関してはとりあえず無限階微分可能と言うことにしておきましょう。

<span class="math">f<span class="paren" style="font-size:em;">(</span>u<span class="paren" style="font-size:em;">)</span></span> みたいな単純なものは、座標変換も簡単で、
単に <span class="math">u ＝ v<sup>－1</sup><span class="paren" style="font-size:em;">(</span>v<span class="paren" style="font-size:em;">)</span></span> を代入するだけで OK です。
ところが、微分が絡むと少し面倒で、
偏微分演算子 <span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table></span> や
微小変分 <span class="math"><span class="normal">d</span>u<sup>i</sup></span> は以下のような変換ルールが必要になります。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
          v<sup>i</sup>
        </denom></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>j</sup>
  </denom></td></tr></table></div><div class="math">
      <span class="normal">d</span>v<sup>i</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>j</sup></td></tr></table><span class="normal">d</span>u<sup>j</sup></div>
高階の偏微分演算子
（<span class="math"><table class="frac" summary="fraction"><tr><td class="num">∂∂</td></tr><tr><td>∂u<sup>i</sup>∂u<sup>j</sup></td></tr></table></span> 等）
や、
重積分中の微小変分
（<span class="math"><span class="normal">d</span>u<sup>i</sup><span class="normal">d</span>u<sup>j</sup></span> 等）
の座標変換はさらに複雑になるであろうことは容易に想像が付くかと思います。
微小変分は、
<div class="math">
      <span class="normal">d</span>z<span class="normal">d</span>w
＝
<span class="normal">det</span><span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td><table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂x</td></tr></table></td><td><table class="frac" summary="fraction"><tr><td class="num">∂z</td></tr><tr><td>∂y</td></tr></table></td></tr><tr><td><table class="frac" summary="fraction"><tr><td class="num">∂w</td></tr><tr><td>∂x</td></tr></table></td><td><table class="frac" summary="fraction"><tr><td class="num">∂w</td></tr><tr><td>∂y</td></tr></table></td></tr></table><span class="paren" style="font-size:4em;">]</span><span class="normal">d</span>x<span class="normal">d</span>y
</div>
というように行列式で表される形になります。
詳細は後述しますが、これは実は、微小変分同士の間にウェッジ積と呼ばれる積を定義することで簡潔な表現が可能になるのですが、
偏微分演算子の方は、
<div class="math">
      <span class="paren" style="font-size:1.5em;">(</span>y <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="paren" style="font-size:1.5em;">)</span>
      <span class="paren" style="font-size:1.5em;">(</span>x <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="paren" style="font-size:1.5em;">)</span>
＝
xy <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table>
＋
y <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table></div><div class="math">
      <span class="paren" style="font-size:1.5em;">(</span>x <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="paren" style="font-size:1.5em;">)</span>
      <span class="paren" style="font-size:1.5em;">(</span>y <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="paren" style="font-size:1.5em;">)</span>
＝
xy <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table>
＋
x <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table></div>
という例からも分かるように、積の微分法則が複雑なため、特に高階の座標変換が困難です。
<pre>
追記予定

微分演算子が座標変換に弱い、というのは、
ベクトル解析で使う、勾配・発散・回転を見てみるもはっきり分かる。
例を挙げて説明。
</pre>

###<a id="sec-generated-title-4"></a> <a id="d80e446"></a>座標変換の有効性
<pre>
執筆予定

・座標変換
座標変換（あるいは変数変換）の重要性を先に説明しよう
- 座標変換、変数変換の有用性

不定積分にたいして、解析的に原始関数を求めるとき、
置換積分と呼ばれる手法を使うことが多々ある
→ 置換積分はある意味、変数変換＝座標変換の一種。

微分方程式を解析的に解くのにもよく変数変換を用いる。

線形変換も、座標変換によって簡単化できる。
  参考： 「[Jordan の標準形](/study/math/linear/eigen?key=jordan)」

→ このように、座標変換によって式変形することで、
問題を解きやすくする事ができる。


・高次元化
あと、独立変数の数を増やす代わりに、
方程式の次数や微分の階数なんかを減らす手法があることも示そう

ラグランジュの未定乗数法
高階線形微分方程式 → 連立1階線形微分方程式
解析力学のラグランジュの方程式 → ハミルトンの正準方程式

→ 3次元にしか使えないベクトル解析では不十分
3次元で定式化しているつもりが、
未定乗数で1次元増えたり、
正準変数の導入で6次元になったり。
</pre>

##<a id="sec-generated-title-5"></a> <a id="total"></a>全微分
前節で、微分が絡むと座標変換が難しくなると書きましたが、
微分が絡んでいても、ある条件下では座標変換が簡潔に表されます。
その最たる例が全微分です。
 
座標に関する関数 <span class="math">f</span> の全微分 <span class="math"><span class="normal">d</span>f</span> は、
どんな座標系を用いようとも同じ形式で表されます。
例えば、<span class="math">u</span> と <span class="math">v</span> という2つの座標で <span class="math"><span class="normal">d</span>f</span> を表すと以下のようになります。
<div class="math">
      <span class="normal">d</span>f
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table>
f
<span class="normal">d</span>u<sup>i</sup>
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    v<sup>i</sup>
  </denom></td></tr></table>
f
<span class="normal">d</span>v<sup>i</sup></div>
これはどういうことかと言うと、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table></span>
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table></span>
と
<span class="math"><span class="normal">d</span>u<sup>i</sup></span>
の変換規則が真逆なため、打ち消しあうことで結局の所元の形が維持されます。
実際に計算してみると、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    v<sup>i</sup>
  </denom></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>j</sup>
  </denom></td></tr></table>
, 
<span class="normal">d</span>v<sup>i</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>j</sup></td></tr></table><span class="normal">d</span>u<sup>j</sup></span>
なので、
<div class="math">
      <span class="normal">d</span>f
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    v<sup>i</sup>
  </denom></td></tr></table>
f
<span class="normal">d</span>v<sup>i</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>j</sup>
  </denom></td></tr></table>
f
<table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>k</sup></td></tr></table><span class="normal">d</span>u<sup>k</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>k</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>j</sup>
  </denom></td></tr></table>
f
<span class="normal">d</span>u<sup>k</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂u<sup>k</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>j</sup>
  </denom></td></tr></table>
f
<span class="normal">d</span>u<sup>k</sup>
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
    u<sup>i</sup>
  </denom></td></tr></table>
f
<span class="normal">d</span>u<sup>i</sup></div>
となります。
最後の式変形は、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂u<sup>k</sup></td></tr></table>
＝
δ<sub>jk</sub></span>
（<span class="math">δ<sub>jk</sub></span> はクロネッカーのδ）
であることを利用しています。
真逆な2つの変換規則が互いに打ち消しあっているというのがポイントなわけですが、
このことから次のような発想が生まれます。
 
ベクトル解析では、積分形で表される法則、
例えば、
<span class="math"><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="vector">E</span>・<span class="normal">d</span><span class="vector">l</span> ＝ －<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="vector">B</span>・<span class="normal">d</span><span class="vector">S</span></span>
等を、
微分形で
<span class="math"><span class="vector">∇</span>×<span class="vector">E</span> ＝ －<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="vector">B</span></span>
というように表し、
<span class="math"><span class="normal">d</span>x</span> 等の微小変分を省略していましたが、
これが間違いだったのではないでしょうか。
<span class="math"><span class="normal">d</span>x</span> 等を付けっぱなしにすることで、
全微分のように座標変換に対して不変な記述ができたのではないか、
という発想です。
 
微分形式というのは、この発想から生まれてくる概念です。


##<a id="sec-generated-title-6"></a> <a id="vector"></a>ベクトル
前節で、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
と
<span class="math"><span class="normal">d</span>u<sup>i</sup></span>
という、
変換規則が真逆な2つの物があることを説明しました。
<span class="math">N</span> 次元座標 <span class="math">u<sup>i</sup></span>（<span class="math">i ＝ 1～N</span>）を考えると、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
と
<span class="math"><span class="normal">d</span>u<sup>i</sup></span>
はいずれも <span class="math">N</span> 個ずつ存在することになります。
これら2組を基底として、
2つの <span class="math">N</span> 次元ベクトル空間
<span class="math">
a<sup>i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
と
<span class="math">
f<sub>i</sub><span class="normal">d</span>u<sup>i</sup></span>
を作ることが出来ます。
 
物理的なイメージは次節以降で説明していくことになりますが、
本節ではまず、形式的にこれらのベクトル空間の座標変換について説明します。
 
では、これらのベクトルが座標変換によってどう変化するかを考えてみましょう。
とりあえず、まずは
<span class="math">
a<sup>i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
の方について考えます。
基底を明示するために、
基底 <span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
を用いた時の成分を
<span class="math">a<sup>u i</sup></span>、
基底 <span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table></span>
を用いた時の成分を
<span class="math">a<sup>v i</sup></span>、
で表してみます。
すると、
<span class="math">
a<sup>u i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table>
＝
a<sup>v i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table></span>
となればいいわけですが、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
の座標変換規則
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>j</sup></denom></td></tr></table></span>
を考えると、
<div class="math">
a<sup>u i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table>
＝
a<sup>u j</sup><table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>j</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table>
＝
a<sup>v i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table></div>
となるわけで、
<div class="math">
a<sup>v i</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>j</sup></td></tr></table>
a<sup>u j</sup></div>
と表されます。
数ベクトル＆行列的に表現するなら、
<span class="math">
a<sup>u</sup>
＝
<span class="paren" style="font-size:em;">(</span>
 a<sup>u 1</sup>, 
 a<sup>u 2</sup>,
  ・・・
 a<sup>u N</sup><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
, 
<span class="math">
a<sup>u</sup>
＝
<span class="paren" style="font-size:em;">(</span>
 a<sup>u 1</sup>, 
 a<sup>u 2</sup>,
  ・・・
 a<sup>u N</sup><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>
, 
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>v</td></tr><tr><td><span class="normal">d</span>u</td></tr></table>
＝
<span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>j</sup></td></tr></table><span class="paren" style="font-size:2em;">)</span></span>
と置いて、
<span class="math">
a<sup>v</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>v</td></tr><tr><td><span class="normal">d</span>u</td></tr></table>
a<sup>u</sup></span>
と表すことが出来ます。
 
同様に、
<span class="math">
f<sub>i</sub><span class="normal">d</span>u<sup>i</sup></span>
の方も、
基底を明示するために、
基底 <span class="math"><span class="normal">d</span>u<sup>i</sup></span>
を用いた時の成分を
<span class="math">
f<sub>u</sub>
＝
<span class="paren" style="font-size:em;">(</span>
 f<sub>u 1</sub> , 
 f<sub>u 2</sub> ,
  ・・・
 f<sub>u N</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>、
基底 <span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table></span>
を用いた時の成分を
<span class="math">
f<sub>v</sub>
＝
<span class="paren" style="font-size:em;">(</span>
 f<sub>v 1</sub> , 
 f<sub>v 2</sub> ,
  ・・・
 f<sub>v N</sub><span class="paren" style="font-size:em;">)</span><sup>T</sup></span>、
で表すと、
<span class="math">
f<sub>v</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>u</td></tr><tr><td><span class="normal">d</span>v</td></tr></table>
f<sub>u</sub></span>
と表されることになります。

<span class="math">
a<sup>i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
の方の成分
<span class="math">
a<sup>i</sup></span>
は、座標変換に対して
<span class="math">
a<sup>v</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>v</td></tr><tr><td><span class="normal">d</span>u</td></tr></table>
a<sup>u</sup></span>
という式で変化し、
これは、実は、<span class="math"><span class="normal">d</span>u<sup>i</sup></span>
と同じ変換規則になっています。
一方、
<span class="math">
f<sub>i</sub><span class="normal">d</span>u<sup>i</sup></span>
の方の成分
<span class="math">
f<sub>i</sub></span>
は、座標変換に対して
<span class="math">
f<sub>v</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>u</td></tr><tr><td><span class="normal">d</span>v</td></tr></table>
f<sub>u</sub></span>
という式で変化し、
これは、実は、<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
と同じ変換規則になっています。
（いずれも、基底と反対の変換規則になる。）


###<a id="sec-generated-title-7"></a> <a id="co_contravariant"></a>共変ベクトルと反変ベクトル
座標変数の微小差分 <span class="math"><span class="normal">d</span>u<sup>i</sup></span> と同じ変換規則という意味で、
<span class="math"><span class="normal">d</span>u<sup>i</sup></span>
の方を<strong id="d80e1238" class="keyword">共変ベクトル</strong>（covariant vevtor）、
<span class="math"><span class="normal">d</span>u<sup>i</sup></span> と反対の変換規則という意味で、
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span>
の方を<strong id="d80e1255" class="keyword">反変ベクトル</strong>（contravariant vevtor）と呼びます。
 
共変ベクトル・反変ベクトルという呼び方は、
座標変換の変化の仕方という視点で見た場合の呼び名になります。
次節以降では、
これらのベクトルの物理的なイメージについて説明していくことになりますが、
その際、視点の違いによっていくつか異なる呼び方をします。
 
反変ベクトルの方は、多様体論などでは接ベクトルと呼ばれていますし、
力学などでは単にベクトル場と呼べば反変ベクトルのことを指します。
 
共変ベクトルの方はというと、
多様体論などでは余接ベクトルと呼ばれています。
これは、接ベクトルの「[双対空間](m_linear.md#dualspace)」になっているためです。
また、本稿の主題である微分形式は共変ベクトル（を含む概念）です。
 
ちなみに、ここまでの説明で、
添字が上に付いているものと下についているものがあることにお気づきでしょうか。
基本的に、共変ベクトルの添字は上に、
反変ベクトルの添字は下につけます。
 
これはどういうことかと言うと、
上に添字の付いているものと下に添字の付いているものとの間で和を取る形になっています。
「[記法に関して](#notation)」で説明したように、
「[アインシュタインの記法](#einstein)」によって和を意味する Σ を省略しているわけですが、
このとき、上に付く添字と下に付く添字で同じ文字があれば和を取るんだと思ってください。
基本的に、上に付く添字同士、下に付く添字同士で和を取ることはありません。

<table summary="2つのベクトル">
	<caption>
		2つのベクトル
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>反変ベクトル</th>
		<th>共変ベクトル</th>
	</tr>
	<tr>
		<th>表現方法</th>
		<td markdown="1"><span class="math">a<sup>i</sup><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>i</sup></denom></td></tr></table></span></td>
		<td markdown="1"><span class="math">f<sub>i</sub><span class="normal">d</span>u<sup>i</sup></span></td>
	</tr>
	<tr>
		<th>座標変換規則<br></br>（基底）</th>
		<td markdown="1"><span class="math">
              <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>v<sup>i</sup></denom></td></tr></table>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂u<sup>j</sup></td></tr><tr><td>∂v<sup>i</sup></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>u<sup>j</sup></denom></td></tr></table></span></td>
		<td markdown="1"><span class="math">
              <span class="normal">d</span>v<sup>i</sup>
＝
<table class="frac" summary="fraction"><tr><td class="num">∂v<sup>i</sup></td></tr><tr><td>∂u<sup>j</sup></td></tr></table><span class="normal">d</span>u<sup>j</sup></span></td>
	</tr>
	<tr>
		<th>その他の呼び方</th>
		<td markdown="1">接ベクトル、ベクトル場</td>
		<td markdown="1">余接ベクトル、微分形式</td>
	</tr>
</table>



###<a id="sec-generated-title-8"></a> <a id="scalar"></a>スカラー
アインシュタインの記法では、
上に付く添字と下に付く添字で同じ文字があれば和を取ると説明しました。
また、共変ベクトルの添字は上、
反変ベクトルの添字は下だという説明もしました。
要するに、
添字が上のものと下のもので座標変換規則が逆なんですね。
なので、
添字が上のものと下のものの積和を取ると、
座標変換に対して不変なものができます。
 
ということは、
共変ベクトルの成分 <span class="math">a<sup>i</sup></span> と
反変ベクトルの成分 <span class="math">f<sub>i</sub></span> の
積和
<span class="math">a<sup>i</sup> f<sub>i</sub></span>
も座標変換に対して不変です。
（座標に対して不変な量なので、基底を省略して書いても OK。）
多様体論などの分野では、
このような座標変換に対して不変な量を<strong id="scaler" class="keyword">スカラー</strong>（scaler）と呼びます。
例え1次元の量（向きを持たない量）であっても、
座標変換に対して不変でないものはスカラーとは呼びません。
