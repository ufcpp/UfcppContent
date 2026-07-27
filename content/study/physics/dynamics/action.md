---
title: "最小作用の原理"
source_url: "https://ufcpp.net/study/physics/dynamics/action/"
content_type: "Article"
published_at: "2007-04-04T00:00:00"
updated_at: "2007-05-28T00:00:00"
tags: []
umbraco_id: 1556
parent_id: 1554
sort_order: 1
aliases:
  - "/study/dynamics/action.html"
---

# 最小作用の原理

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

解析力学なんかを習うと、「最小作用の原理」なんて物が出てきます。
ここから出発して、変分法を使うと、
ラグランジュ形式の運動方程式という、
どんな座標系を使っても同じ偏微分方程式で記述できる運動法則が導かれます。
 
でも、作用ってのがなんなのかは書いてないことが多いんですよね。
いや、位置エネルギーと運動エネルギーの差 <span class="math">L <span class="normal">=</span> T <span class="normal">−</span> V</span> が作用（の密度）だという定義式は書いてあるんですが、
なんでそんなものを最小にするのかが良く分からない。
 
もちろん、「<span class="math">L <span class="normal">=</span> T <span class="normal">−</span> V</span> が作用」なんじゃなくて、
「作用になるものを探したら <span class="math">L <span class="normal">=</span> T <span class="normal">−</span> V</span> だった」
というのが正しいんですよ。
でも、じゃあ、一体、作用ってなんなんでしょう？


## <a id="sec-generated-title-2"></a> <a id="shortest"></a>労力的に最短

まず、図1を見てください。

<figure>

[![最短経路 ＝ 直線](../../../../assets/media/ufcpp2000/physics/action01.png)](../../../../assets/media/ufcpp2000/physics/action01.png)

<figcaption>最短経路 ＝ 直線</figcaption>
</figure>


何もない平面に2点 A と B があります。
A から B まで進めといわれたら、
普通は図中の矢印のようにまっすぐ進みますね。
何せそれが最短ですから。
 
中には、ひねくれてて、まっすぐ進まない人もいるかもしれませんが、
物理学の法則によると、
物体は最短経路を進むとされています。
自然の法則は素直です。
いわゆる、「慣性の法則」というやつで、
動いている物質は、特に力とかを受けなければ、
そのままのスピードでまっすぐ進み続けます。
 
じゃあ、次は、平面でない場合を考えて見ましょう。
図2のように、等高線を引いてみます。

<figure>

[![平面じゃなくしてみた](../../../../assets/media/ufcpp2000/physics/action02.png)](../../../../assets/media/ufcpp2000/physics/action02.png)

<figcaption>平面じゃなくしてみた</figcaption>
</figure>


まあ、こうなると、
A から B への行き方は人によるかもしれませんね。
例えば、高さをものともしない人の場合、等高線は無視して相変わらず直線経路をとるかもしれません（図3）。
尾根道を通るのが楽だという人なら、図4のような経路になるでしょう。
あるいは、高さが苦手だと（例えば、上り下りが全くできない乗り物があったとして、それに乗るとすると）図5のような経路を取らざるを得ません。

<figure>

[![高さをものともしない場合](../../../../assets/media/ufcpp2000/physics/action03.png)](../../../../assets/media/ufcpp2000/physics/action03.png)

<figcaption>高さをものともしない場合</figcaption>
</figure>


<figure>

[![尾根道好きの人](../../../../assets/media/ufcpp2000/physics/action04.png)](../../../../assets/media/ufcpp2000/physics/action04.png)

<figcaption>尾根道好きの人</figcaption>
</figure>


<figure>

[![上り下りができない乗り物にでも乗ると](../../../../assets/media/ufcpp2000/physics/action05.png)](../../../../assets/media/ufcpp2000/physics/action05.png)

<figcaption>上り下りができない乗り物にでも乗ると</figcaption>
</figure>


まあ、人によって経路は違いますが、
いずれも、その当人にとっては労力が最小の経路だと思ってください。
進む位置や方向によって移動コストが違う。
で、地図上の最短経路の変わりに、労力的に最短な経路を通る。
 
で、物理法則上、物体はこの
「労力が最小の経路」を進むわけです。
ここでは口語的に「労力」って言い方しましたが、
物理用語としては、作用（action）と言います。
これが「<strong id="minaction" class="keyword">最小作用の原理</strong>」。
物体はオーバーアクションせずに、最小のアクションで動くんですね。
実に省エネです。


## <a id="sec-generated-title-3"></a> <a id="line"></a>平面上の最短経路 ＝ 直線

物体は最小作用の原理に従う、
すなわち、
労力的に最短な経路を通ります。
例えば、何の力も働いてないまっ平らな空間だと、直線になるわけですね。
だから物体はまっすぐ進む（慣性の法則）。
 
で、まずはこの「平面上の最短経路 ＝ 直線」という話を、
数学的に考察しなおしてみましょう。
経路長というのは、解析学（微分・積分）の言葉で書くなら、
<div class="math">
      <span class="cursive">L</span>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <span class="normal">d</span>s
</div>
となるので、
これを最小化する問題になります。
簡単化のため2次元で考えて、
物体が経路 <span class="math"><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">)</span></span> 上を動くものとした場合、
<span class="math">x, y</span> の時間微分を ' で表すものとして
（時間微分は文字の上に・を書く記法の方が一般的ですが、
表示上の都合でここでは ' を使います）、
<div class="math">
      <span class="normal">d</span>
      <span class="vector">s</span>
      <span class="normal">=</span>
      <span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup></span>
      <span class="normal">d</span>t
</div>
なので、
<div class="math">
      <span class="cursive">L</span>
      <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup></span>
      <span class="normal">d</span>t
</div>
を最小にする <span class="math">t</span> の関数 <span class="math">x, y</span> を求めることになります。

<span class="math">x, y</span> がただの変数じゃなくて、
<span class="math">t</span> の関数だということに注意してください。
こういう問題は、変分問題と呼ばれていて、
かなりしっかりと解き方が研究されています（「[変分学](variation.md)」参照）。
で、この変分学の知識を使えば、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup></td></tr></table>
x
<span class="normal">=</span><span class="normal">0</span>
,
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span><sup><span class="normal">2</span></sup></td></tr><tr><td><span class="normal">d</span>t<sup><span class="normal">2</span></sup></td></tr></table>
y
<span class="normal">=</span><span class="normal">0</span></div>
という条件に書き直すことが出来て、
これを解くと、結局、直線が最短経路な事が分かります。


## <a id="sec-generated-title-4"></a> <a id="curve"></a>歪んだ空間での最短経路

前節での説明どおり、
何の力も働いていない平面上では、物体は
<span class="math"><span class="cursive">L</span><span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup></span><span class="normal">d</span>t
</span>
を最小にするような経路を通ります。
 
ところが、力が働いていると、空間が歪みます。
平面の場合には、
物体が空間上のどこにいても、移動には同じだけのコスト
<span class="math"><span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup></span></span>
掛かっていたわけですが、
これに空間の歪みが加わって、
<span class="math"><span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">+</span>
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span></span>
になると思ってください。
（関数 <span class="math">u</span> の具体的な形は後々求めます。）
すなわち、
<div class="math">
      <span class="cursive">L</span>
      <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">+</span>
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span>
      <span class="normal">d</span>t
</div>
の最小化問題。
ところで、この式中、平方根がちょっとうっとうしいですね。
平方根がからむと、計算が面倒になります。
 
実はこの平方根はなくすことができます。
簡単化のために平方根の中身を <span class="math">L</span> と書くとき、
<span class="math">L</span> がある良好な性質を持っているとき、
<span class="math"><span class="cursive">L</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="normal" style="font-size:em;">√</span><span class="bar">
L
</span><span class="normal">d</span>t
</span>
の最小化問題と、
<span class="math">
I
<span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
L
<span class="normal">d</span>t
</span>
の最小化問題は同値な事が示せます。
（「[弧長とエネルギー](variation.md#energy)」。）
したがって、
<div class="math">
I
<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="paren" style="font-size:1.5em;">(</span>
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">+</span>
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">)</span><span class="normal">d</span>t
</div>
の最小化問題を考えることになります。
で、この
<span class="math">
I
</span>
を<strong id="action" class="keyword">作用</strong>（action）、
<span class="math">L</span> を<strong id="actiond" class="keyword">作用密度</strong>（action density）と呼びます。
 
さて、変分学の知識から、
この変分問題は、以下のような微分方程式に置き換えられます。
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">∂</span>
        </td></tr><tr><td>
          <span class="normal">∂</span>x'</td></tr></table>
L
<span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">∂</span></td></tr><tr><td><span class="normal">∂</span>x</td></tr></table>
L
<span class="normal">=</span><span class="normal">0</span></div>
（<span class="math">y</span> についても同様。）
これに、
<span class="math">
L
<span class="normal">=</span>
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">+</span>
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span>
を代入すれば、
以下の式が得られます。
<div class="math">
      <span class="normal">2</span>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup></td></tr></table>
x
<span class="normal">+</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">∂</span></td></tr><tr><td><span class="normal">∂</span>x</td></tr></table>
u
<span class="normal">=</span><span class="normal">0</span></div>
（これも、<span class="math">y</span> についても同様。）
ニュートンの運動方程式
<span class="math">
m
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span><sup><span class="normal">2</span></sup></td></tr><tr><td><span class="normal">d</span>t<sup><span class="normal">2</span></sup></td></tr></table>
x
<span class="normal">=</span>
f
</span>
と比べると、
<span class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">∂</span></td></tr><tr><td><span class="normal">∂</span>x</td></tr></table>
u
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>m</td></tr></table>f
</span>
とすれば、
上述の変分問題で運動の法則を表せることが分かります。
特に、<span class="math">f</span> が保存場の場合、
ポテンシャル <span class="math">V</span> が存在して、
<span class="math">
u
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>m</td></tr></table>
V
</span>
となります。
 
要するに、
<span class="math">
L
<span class="normal">=</span>
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>m</td></tr></table>
V
</span>
。
ちょこっと定数倍して、
<span class="math">
L
<span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table>
m
<span class="paren" style="font-size:1.5em;">(</span>
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="paren" style="font-size:1.5em;">)</span><span class="normal">−</span>
V
</span>
としてもいい。
よく見てみると、これは、
運動エネルギーを <span class="math">T</span> とすると、
<span class="math">
L
<span class="normal">=</span>
T
<span class="normal">−</span>
V
</span>
になっています。
 
結局、
「
<span class="math">
L
<span class="normal">=</span>
T
<span class="normal">−</span>
V
</span>
を作用密度とする変分問題（作用最小化問題）を解くと、
ニュートンの運動方程式を解いたのと同じ結果が得られる」
ということになります。
 
あと、この考察は同時に、ポテンシャルという物をどう解釈すればいいのかも示唆しています。
ここで示した内容から察するに、
ポテンシャルというのは、
空間の歪みによる「移動に掛かるコスト」の変化なわけです。


## <a id="sec-generated-title-5"></a> <a id="summary"></a>まとめ

要するに、

* 作用というのは物体が運動するときにかかる労力。

* 「最小作用の原理」というのは、「物体は労力的にみて最短な経路を通ろうとする」ということ。

* ニュートンの運動法則を満たす作用密度を探してみたら、<span class="math">
L
<span class="normal">=</span>
T
<span class="normal">−</span>
V
</span>だった。


ということになります。
 
ちなみに、ここで書いた内容、
歴史的に見ると順序が逆なんですけどね。
最初は、「仮想仕事の原理」というのから出発して作用積分が導出されていました。
そのころは「最小作用 ＝ 距離の最短化」という明確な認識はなかったと思います。
あとになって、作用積分の意味を考えてみた結果、
距離の類似概念として捉えるとうまくいくことが分かった。
 
あと、本当のことをいうと、“最小”作用というのは不正確だったりします。
実際には微分・変分が 0 というだけ、
要するに、極大・極小・鞍点（合わせて停留点と言う）の可能性もある。
中には、「停留作用の原理と呼ぶべきなのかもしれない」なんて言う人も。
