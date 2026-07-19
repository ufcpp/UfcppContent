---
title: "計量とポテンシャル"
source_url: "https://ufcpp.net/study/physics/dynamics/potential/"
content_type: "Article"
published_at: "2007-04-05T00:00:00"
updated_at: "2007-04-08T00:00:00"
tags: []
umbraco_id: 1557
parent_id: 1554
sort_order: 2
aliases:
  - "/dynamics/potential"
  - "/dynamics/potential.html"
  - "/physics/dynamics/potential/"
  - "/study/dynamics/potential"
  - "/study/dynamics/potential.html"
---

# 計量とポテンシャル

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[最小作用の原理](action.md)」では、
「物体は最短経路上を運動する」というような話をしました。
そこでは、
経路長として
<span class="math"><span class="normal">d</span><span class="vector">s</span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup></span><span class="normal">d</span>t
</span>
の積分を用いて、その最小化問題を考えました。
（表示の都合上、時間微分を ' で表します。）
この式は、いわゆる直交座標系での話なわけですが、
一般の座標系の場合について考え直してみましょう。
 
一般の座標系の場合、
計量（metric）という考え方が出てきます。
計量というのは、空間上の各点における「空間の伸び縮み」、
あるいは「移動に掛かるコスト」を表す量なのですが、
詳細は次節移行で説明していきます。
 
また、物体に力が掛かっているときには、
平方根の中身に、ポテンシャルに相当する項
<span class="math">
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span>
を付け加えて、
<span class="math"><span class="normal">d</span><span class="vector">s</span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup><span class="normal">+</span>
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span><span class="normal">d</span>t
</span>
の積分の最小化を考えました。
この <span class="math">u</span> という項が一体なんなのかということも、
計量の考え方に基づくことで、すっきりとした理解が可能です。
 
さらに、計量の考え方の基、
ベクトルポテンシャルというものも考えてみます。


## <a id="sec-generated-title-2"></a> <a id="general"></a>一般座標系と計量

「[最小作用の原理](action.md)」では、
座標を <span class="math"><span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span> で書きましたが、
このページでは、
<div class="math">
      <span class="vector">x</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
 x<sub><span class="normal">1</span></sub> ,
 x<sub><span class="normal">2</span></sub> ,
 x<sub><span class="normal">3</span></sub> <span class="paren" style="font-size:em;">)</span>
      <sup>t</sup>
    </div>
（右肩の <span class="math">t</span> は転置の意味。）
と書くことにします。
線素 <span class="math"><span class="normal">d</span>s</span> は、
<div class="math">
      <span class="normal">d</span>s
<span class="normal">=</span><span class="normal">|</span><span class="normal">d</span><span class="vector">s</span><span class="normal">|</span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar">
x<sub><span class="normal">1</span></sub>'<sup><span class="normal">2</span></sup><span class="normal">+</span>
x<sub><span class="normal">2</span></sub>'<sup><span class="normal">2</span></sup><span class="normal">+</span>
x<sub><span class="normal">3</span></sub>'<sup><span class="normal">2</span></sup></span><span class="normal">d</span>t
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="vector">x</span>'<sup>t</sup><span class="vector">x</span>'
</span><span class="normal">d</span>t
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span><span class="vector">x</span><sup>t</sup><span class="normal">d</span><span class="vector">x</span></span></div>
となります。

<span class="math">
        <span class="vector">x</span>
      </span>
は直交座標なわけですが、
これに対して、
一般の座標 <span class="math"><span class="vector">r</span></span> を導入します。
<span class="math"><span class="vector">x</span></span>
と
<span class="math"><span class="vector">r</span></span>
の関係を、
<div class="math">
      <span class="vector">r</span>
      <span class="normal">=</span>
      <span class="vector">r</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">x</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
と表しましょう。
このとき、
微分演算の性質から、
それぞれの導関数には以下のような関係がなりたちます。
<div class="math">
      <span class="normal">d</span>
      <span class="vector">r</span>
      <span class="normal">=</span>
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>
          <span class="vector">r</span>
        </num></td></tr><tr><td><span class="normal">d</span><denom>
          <span class="vector">x</span>
        </denom></td></tr></table>
      <span class="normal">d</span>
      <span class="vector">x</span>
    </div><div class="math">
      <span class="normal">d</span>
      <span class="vector">x</span>
      <span class="normal">=</span>
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>
          <span class="vector">x</span>
        </num></td></tr><tr><td><span class="normal">d</span><denom>
          <span class="vector">r</span>
        </denom></td></tr></table>
      <span class="normal">d</span>
      <span class="vector">r</span>
    </div>
ただし、
記号
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num><span class="vector">x</span></num></td></tr><tr><td><span class="normal">d</span><denom><span class="vector">r</span></denom></td></tr></table></span>
は、
<span class="math">i, j</span> 成分が
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂<num>x<sub>i</sub></num></td></tr><tr><td>∂<denom>r<sub>j</sub></denom></td></tr></table></span>
となるような行列です。
したがって、
<div class="math">
      <span class="normal">d</span>
      <span class="vector">x</span>
      <sup>t</sup>
      <span class="normal">d</span>
      <span class="vector">x</span>
      <span class="normal">=</span>
      <span class="normal">d</span>
      <span class="vector">r</span>
      <sup>t</sup>
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>
          <span class="vector">x</span>
        </num></td></tr><tr><td><span class="normal">d</span><denom>
          <span class="vector">r</span>
        </denom></td></tr></table>
      <sup>t</sup>
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>
          <span class="vector">x</span>
        </num></td></tr><tr><td><span class="normal">d</span><denom>
          <span class="vector">r</span>
        </denom></td></tr></table>
      <span class="normal">d</span>
      <span class="vector">r</span>
    </div>
となります。
ここで、
<span class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num><span class="vector">x</span></num></td></tr><tr><td><span class="normal">d</span><denom><span class="vector">r</span></denom></td></tr></table><sup>t</sup><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num><span class="vector">x</span></num></td></tr><tr><td><span class="normal">d</span><denom><span class="vector">r</span></denom></td></tr></table></span>
の部分を <span class="math"><span class="vector">G</span><span class="normal">=</span><span class="paren" style="font-size:em;">{</span>G<sub>ij</sub><span class="paren" style="font-size:em;">}</span></span>
という行列で書き表すなら、
<div class="math">
      <span class="normal">d</span>s
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span><span class="vector">x</span><sup>t</sup><span class="normal">d</span><span class="vector">x</span></span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span><span class="vector">r</span><sup>t</sup><span class="vector">G</span><span class="normal">d</span><span class="vector">r</span></span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="vector">r</span>'<sup>t</sup><span class="vector">G</span><span class="vector">r</span>'
</span><span class="normal">d</span>t
</div><div class="math">
G<sub>ij</sub><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k<span class="normal">=</span><span class="normal">1</span>, <span class="normal">2</span>, <span class="normal">3</span></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂<num>x<sub>k</sub></num></td></tr><tr><td>∂<denom>r<sub>i</sub></denom></td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂<num>x<sub>k</sub></num></td></tr><tr><td>∂<denom>r<sub>j</sub></denom></td></tr></table></div>
となります。
 
今、
<span class="math"><span class="vector">x</span></span>
と
<span class="math"><span class="vector">r</span></span>
の関係から対称な行列
<span class="math"><span class="vector">G</span></span>
を導出しましたが、
実は、
<span class="math"><span class="vector">x</span></span>
と
<span class="math"><span class="vector">r</span></span>
の関係とかは考えずに、
<span class="math"><span class="vector">G</span></span>
だけ与えるようにしてもかまわないんですね。
<span class="math"><span class="vector">G</span></span>
が与えられれば、
<span class="math"><span class="normal">d</span>s
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span><span class="vector">r</span><sup>t</sup><span class="vector">G</span><span class="normal">d</span><span class="vector">r</span></span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="vector">r</span>'<sup>t</sup><span class="vector">G</span><span class="vector">r</span>'
</span><span class="normal">d</span>t
</span>
の積分で距離が定義できます。
 
この対称行列
<span class="math"><span class="vector">G</span></span>
を<strong id="metric" class="keyword">計量</strong>（metric）と呼びます。
（ここでは行列という言い方をしていますが、
本当は、座標変換のことまで考えると、
「2階の共変テンソル」という方が正確。
テンソルについては、「[数学](../../math/index.md)」あたりで説明予定。
）
計量というのは、空間上の各点における「長さの尺度」という意味です。
<span class="math"><span class="vector">r</span></span> が直交座標ではないので、
各点の尺度が違うと考える。


## <a id="sec-generated-title-3"></a> <a id="scalar"></a>スカラーポテンシャルと4次元計量

「[歪んだ空間での最短経路](action.md#curve)」では、
<span class="math"><span class="normal">d</span><span class="vector">s</span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar">
x'<sup><span class="normal">2</span></sup><span class="normal">+</span>
y'<sup><span class="normal">2</span></sup></span><span class="normal">d</span>t
</span>
の平方根の中身にスカラーポテンシャルに相当する項
<span class="math">
u<span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span></span>
を加えました。
前節で導入した記法に従って書き直すなら、
<div class="math">
      <span class="normal">d</span>s
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="vector">r</span>'<sup>t</sup><span class="vector">G</span><span class="vector">r</span>'
<span class="normal">+</span>
u
</span><span class="normal">d</span>t
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span><span class="vector">r</span><sup>t</sup><span class="vector">G</span><span class="normal">d</span><span class="vector">r</span><span class="normal">+</span><span class="normal">d</span>t 
u
<span class="normal">d</span>t
</span></div>
となります。
特に、一番右の辺を見てください。
なんだか、さらに綺麗にまとめられそうな気がします。
時間変数 <span class="math">t</span> と空間座標 <span class="math"><span class="vector">r</span></span> をまとめて4次元ベクトル <span class="math">q</span> を、
スカラーポテンシャル <span class="math">u</span> と3次元計量 <span class="math"><span class="vector">G</span></span>をまとめて4次元計量 <span class="math">g</span> を作ります。
<div class="math">
q
<span class="normal">=</span><span class="paren" style="font-size:em;">(</span>t, <span class="vector">r</span><span class="paren" style="font-size:em;">)</span><sup>t</sup></div><div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>u</td><td><span class="normal">0</span></td></tr><tr><td><span class="normal">0</span></td><td><span class="vector">G</span></td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
すると、先ほどの線素の式は、
<div class="math">
      <span class="normal">d</span>s
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span>q<sup>t</sup>
g
<span class="normal">d</span>q
</span></div>
という非常にシンプルな形に落ち着きます。
要するに、3次元空間に時間を加えて、
4次元時空として考えた方が式が綺麗にまとまる。
そして、ポテンシャルのつもりで導入した <span class="math">u</span> は、
4次元時空の計量の時間 <span class="math">t</span> に関係する成分だという解釈ができます。


## <a id="sec-generated-title-4"></a> <a id="vector"></a>ベクトルポテンシャル

前節で導入した4次元計量 <span class="math">g</span> は、
<div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>u</td><td><span class="normal">0</span></td></tr><tr><td><span class="normal">0</span></td><td><span class="vector">G</span></td></tr></table><span class="paren" style="font-size:3em;">]</span></div>
という形をしていました。
で、これの 0 になっている部分に何か値を入れてみましょう。
（というか、0 のままにしておきたくても、
座標変換の仕方によっては 0 でなくなってしまう場合があります。）
すなわち、
<div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td>u</td><td><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">a</span><sup>t</sup></td></tr><tr><td><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">a</span></td><td><span class="vector">G</span></td></tr></table><span class="paren" style="font-size:5em;">]</span></div>
とします。
 
結論から先に言ってしまうと、
<span class="math">u</span> がスカラーポテンシャル（の定数倍、逆符号）に相当する項なのに対して、
<span class="math"><span class="vector">a</span></span> はベクトルポテンシャルに相当します。
これから、そのことを示していくことにします。
 
まず、この計量 <span class="math">g</span> 線素 <span class="math"><span class="normal">d</span>s</span> の式に代入します。
すると、
<div class="math">
      <span class="normal">d</span>s
<span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span>q<sup>t</sup>
g
<span class="normal">d</span>q
</span><span class="normal">=</span><span class="normal" style="font-size:em;">√</span><span class="bar"><span class="normal">d</span><span class="vector">r</span><sup>t</sup><span class="vector">G</span><span class="normal">d</span><span class="vector">r</span><span class="normal">+</span><span class="normal">d</span>t 
u
<span class="normal">d</span>t
<span class="normal">+</span><span class="normal">d</span>t <span class="vector">a</span><span class="normal">⋅</span><span class="normal">d</span><span class="vector">r</span></span></div><div class="math">
      <span class="normal">=</span>
      <span class="normal" style="font-size:em;">√</span><span class="bar">
        <span class="vector">r</span>'<sup>t</sup><span class="vector">G</span><span class="vector">r</span>'
<span class="normal">+</span>
u
<span class="normal">+</span><span class="vector">a</span><span class="normal">⋅</span><span class="vector">r</span>'
</span>
      <span class="normal">d</span>t </div>
となります。
そこで、
作用密度
<span class="math">
L
<span class="normal">=</span><span class="vector">r</span>'<sup>t</sup><span class="vector">G</span><span class="vector">r</span>'
<span class="normal">+</span>
u
<span class="normal">+</span><span class="vector">a</span><span class="normal">⋅</span><span class="vector">r</span>'
</span>
と置いて変分問題を解くと、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>r'</denom></td></tr></table>
L
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂r</td></tr></table>
L
<span class="normal">=</span><span class="normal">0</span></div><div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      <span class="paren" style="font-size:2em;">(</span>
        <span class="normal">2</span>
        <span class="vector">G</span>
        <span class="vector">r</span>'
 <span class="normal">+</span><span class="vector">a</span><span class="paren" style="font-size:2em;">)</span>
      <span class="normal">−</span>
      <span class="paren" style="font-size:2em;">(</span>
        <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂r</td></tr></table>
 u
 <span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂r</td></tr></table><span class="vector">a</span><span class="normal">⋅</span><span class="vector">r</span>'
<span class="paren" style="font-size:2em;">)</span>
      <span class="normal">=</span>
      <span class="normal">0</span>
    </div>
<span class="math">
        <span class="vector">r</span>
      </span> が直交座標 <span class="math"><span class="vector">x</span></span> の時には、
<span class="math"><span class="vector">G</span></span> が単位行列となって、
<div class="math">
      <span class="normal">2</span>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup></td></tr></table>
      <span class="vector">x</span>
      <span class="normal">=</span>
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
          <span class="vector">x</span>
        </denom></td></tr></table>
 u
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom><span class="vector">x</span></denom></td></tr></table><span class="vector">a</span><span class="normal">⋅</span><span class="vector">x</span>'
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="vector">a</span></div>
となります。
最後の2項は、
頑張って展開して計算すると、
ベクトル解析の記法で書くなら（参考: 「[数学](../../math/index.md)」）、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>
          <span class="vector">x</span>
        </denom></td></tr></table>
      <span class="vector">a</span>
      <span class="normal">⋅</span>
      <span class="vector">x</span>'
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="vector">a</span><span class="normal">=</span><span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">a</span><span class="normal">−</span><span class="vector">x</span>'
<span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">∇</span><span class="normal">×</span><span class="vector">a</span><span class="paren" style="font-size:em;">)</span></div>
となります。
すなわち、
<div class="math">
      <span class="normal">2</span>
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num>
          <sup><span class="normal">2</span></sup>
        </num></td></tr><tr><td><span class="normal">d</span><denom>t<sup><span class="normal">2</span></sup></denom></td></tr></table>
      <span class="vector">x</span>
      <span class="normal">=</span>
      <span class="normal">∇</span>u
<span class="normal">−</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">a</span><span class="normal">−</span><span class="vector">x</span>'
<span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">∇</span><span class="normal">×</span><span class="vector">a</span><span class="paren" style="font-size:em;">)</span></div>
ベクトル解析の知識とてらし合わせるなら、
スカラーポテンシャルを <span class="math">φ</span>、
ベクトルポテンシャルを <span class="math"><span class="vector">A</span></span>
とすると、
<div class="math">
m
<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span><num><sup><span class="normal">2</span></sup></num></td></tr><tr><td><span class="normal">d</span><denom>t<sup><span class="normal">2</span></sup></denom></td></tr></table><span class="vector">x</span><span class="normal">=</span><span class="normal">−</span><span class="normal">∇</span>φ
<span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><span class="vector">A</span><span class="normal">+</span><span class="vector">x</span>'
<span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">∇</span><span class="normal">×</span><span class="vector">A</span><span class="paren" style="font-size:em;">)</span></div>
となるはずなので、
2つの式を比較して、
<div class="math">
u
<span class="normal">=</span><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>m</td></tr></table>
φ
</div><div class="math">
      <span class="vector">a</span>
      <span class="normal">=</span>
      <span class="normal">−</span>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">2</span>
        </td></tr><tr><td>m</td></tr></table>
      <span class="vector">A</span>
    </div>
となって、定数倍の違い（特に符号が±逆）を除けば、
<span class="math">u, <span class="vector">a</span></span> がポテンシャルに相当するものであることが分かります。


## <a id="sec-generated-title-5"></a> <a id="summary"></a>まとめ

「最小作用の原理」 ≒ 「物体は最短距離を動く」という考え方のもと、
計量という概念を導入しました。
 
3次元空間に時間変数を加えて、
4次元時空で考えると、
ポテンシャルを計量の一部として考えることができます。
 
3次元空間の計量を <span class="math"><span class="vector">G</span></span> として、
ベクトル解析で言う所の
スカラーポテンシャルを <span class="math">φ</span>、
ベクトルポテンシャルを <span class="math"><span class="vector">A</span></span>
とすると、
4次元時空の計量は、
<div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">2</span></td></tr><tr><td>m</td></tr></table>
  φ
 </td><td><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>m</td></tr></table><span class="vector">A</span><sup>t</sup></td></tr><tr><td><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>m</td></tr></table><span class="vector">A</span></td><td><span class="vector">G</span></td></tr></table><span class="paren" style="font-size:5em;">]</span></div>
あるいは定数の掛け方を変えて、
<div class="math">
g
<span class="normal">=</span><span class="paren" style="font-size:5em;">[</span><table class="matrix" summary="matrix"><tr><td>
  φ
 </td><td><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">A</span><sup>t</sup></td></tr><tr><td><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">A</span></td><td><span class="normal">−</span><table class="frac" summary="fraction"><tr><td class="num">m</td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">G</span></td></tr></table><span class="paren" style="font-size:5em;">]</span></div><h4>further reading</h4>
* 距離を計量を使って表す → リーマン幾何学。

* ラグランジュの方程式の変わりに、 測地線（geodesic）の方程式というものが出てくる。

* 4次元計量を使って力学を構築 → 一般相対性理論に繋がる。
