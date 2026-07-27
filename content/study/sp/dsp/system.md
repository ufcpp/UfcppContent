---
title: "システム"
source_url: "https://ufcpp.net/study/sp/dsp/system/"
content_type: "Article"
published_at: "2015-05-06T14:21:57"
updated_at: "2015-05-06T14:21:57"
tags: []
umbraco_id: 1603
parent_id: 1599
sort_order: 3
aliases:
  - "/study/dsp/system.html"
---

# システム

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

```text
入力→[システム]→出力
```
システムあるいは系（system）
信号 <span class="math">x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> を入力して、
<span class="math">y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> という信号を得るようなブラックボックス。


## <a id="sec-generated-title-2"></a> <a id="expression"></a>システムの数学的表現

通常、システムの入出力の関係は、微分方程式を用いて表します。
ある関数 <span class="math">H</span> を用いて、
<div class="math">
y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
H<span class="paren" style="font-size:2em;">(</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
t,
<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
などと言うように表すことができます。
 
特に、入出力の関係を
<div class="math">
y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
H<span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
</div>
という形で表せるとき、
関数 <span class="math">H</span> を<strong id="tf" class="keyword">伝達関数</strong>（transfer function）と呼びます。
信号の伝達経路の特性を表す関数という意味です。
詳細は後ほど説明しますが、
線形時不変なシステムはこのような形で表すことができます。


## <a id="sec-generated-title-3"></a> <a id="class"></a>分類

システムは、その性質からいくつかの種類に分類できます。
 
この節では、システムの分類について説明していきますが、
便宜上、以下の文章では記号の意味を以下のように定義します。
 
まず、システム <span class="math">S</span> に対して、
「信号 <span class="math">x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> をシステム <span class="math">S</span> に入力したとき、
出力信号 <span class="math">y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> が得られた」
というのを、
<div class="math">
y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
S<span class="paren" style="font-size:1.5em;">[</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">]</span></div>
で表します。
 
また、入力信号を
<span class="math">x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>, 
<span class="math">x<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>, 
<span class="math">x<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> ・・・
など、文字 <span class="math">x</span> で表し、
出力信号を
<span class="math">y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>, 
<span class="math">y<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>, 
<span class="math">y<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> ・・・
など、文字 <span class="math">y</span> で表します。
このとき、特に断りがない場合、
暗黙的に
<span class="math">y<sub>n</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>
はシステムに
<span class="math">x<sub>n</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>
を入力したときの出力であるものとします。
 
さらに、特に断りのない限り、
<span class="math">a, b, c</span> などの文字は（<span class="math">t</span> によらない）定数をさすものとします。


### <a id="sec-generated-title-4"></a> <a id="linear"></a>線形性

システム <span class="math">S</span> の入力と出力が以下のような関係を持つとき、
システムは線形である（linear）、あるいは、<strong id="linear" class="keyword">線形性</strong>（linearity）を持つと言います。
（その逆は非線形（non-linear）と言う。）
<div class="math">
S<span class="paren" style="font-size:1.5em;">[</span>
a x<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＋
b x<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">]</span>
＝
a y<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＋
b y<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
線形なシステムは、
<span class="math">x<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> に対する出力
<span class="math">y<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> と、
<span class="math">x<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> に対する出力
<span class="math">y<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> を別個に求めることで、
2つの入力信号の線形結合
<span class="math">
a x<sub>1</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＋
b x<sub>2</sub><span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>
に対する出力を簡単に計算することができます。
 
入出力の関係式を
<div class="math">
y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
H<span class="paren" style="font-size:2em;">(</span>
t,
<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
と表せるとき、このシステムは線形になります。


### <a id="sec-generated-title-5"></a> <a id="time_invariant"></a>時不変性

システム <span class="math">S</span> の入力と出力が以下のような関係を持つとき、
システムは時不変である（time-invariant）、あるいは、<strong id="ti" class="keyword">時不変性</strong>（time-invariance）を持つと言います。
（その逆は時変（time-variant）と言う。）
<div class="math">
S<span class="paren" style="font-size:1.5em;">[</span>
x<span class="paren" style="font-size:em;">(</span>t ＋ T<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.5em;">]</span>
＝
y<span class="paren" style="font-size:em;">(</span>t ＋ T<span class="paren" style="font-size:em;">)</span></div>
ただし、<span class="math">T</span> は定数です。
時不変とは、システムの特性が時間によって不変であるということです。
（入力の時刻がずれると、出力も同じ時間分ずれるだけ。）
 
入出力の関係式を
<div class="math">
y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
H<span class="paren" style="font-size:2em;">(</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
と表せるとき、このシステムは時不変になります。


### <a id="sec-generated-title-6"></a> <a id="lti"></a>線形時不変

システムが線形性と時不変性を両方持っているとき、
システムは線形時不変であるといいます。
線形時不変なシステムは、
定数係数線形微分方程式を用いて表すことができ、
システムの振る舞いを解析的に調べることが容易です。
 
線形時不変システムでは、入出力の関係式を
<div class="math">
y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
H<span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span>
x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></div>
と表すことができます。
このとき、関数 <span class="math">H</span> をシステムの伝達関数と呼びます。


### <a id="sec-generated-title-7"></a> <a id="stable"></a>安定性

任意の時刻 <span class="math">t</span> において、
信号 <span class="math">x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span> が有限の値を持つとき、
その信号は安定（stable）であると言います。
システムに対して安定な信号を入力したときに、
出力信号が必ず安定になるとき、
そのシステムは安定である、あるいは、<strong id="d21e367" class="keyword">安定性</strong>（stability）を持つといいます。
（その逆は不安定（unstable）。）
 
不安定なシステムでは、出力信号が発振したりします。
例えば、スピーカがハウリングを起こすのは、
（マイク→アンプ→スピーカという音の伝達経路が）不安定になっているということです。


### <a id="sec-generated-title-8"></a> <a id="causal"></a>因果性

システムのインパルス応答が時刻 <span class="math">t ＜ 0</span> において 0。
 
現実には因果的でないシステムは作れない。


## <a id="sec-generated-title-9"></a> <a id="plan"></a>執筆予定

```text
  通常、微分方程式(離散信号の場合、差分方程式)で表される。

  線形時不変システムに対しては、フーリエ変換を用いた解析が極めて有効。
  （線形時不変システムは定数係数線形微分方程式で表される。）

  x(t) →[H(d/dt)]→ y(t) = H(d/dt)x(t)
  ↓F
  X(ω) →[H(iω)]→ Y(ω) = H(iω)X(ω)
  ↓F^-1
  x(t) →[* h(t)]→ y(t) = h*x(t)

    ↓
  周波数領域で表現すれば、
    ただの多項式になったりする。
    ただの積で現せる。

  H(iω) を周波数特性とか周波数応答と呼ぶ。

アナログ回路
  能動素子(オペアンプなど)と抵抗・コイル・コンデンサで回路を構成。
  連続信号をそのまま処理。
  微分・積分が回路構成の基本 → s領域で回路設計。(「ラプラス変換」参照)

ディジタル回路
  連続信号を標本化して離散信号に。
  差分が基本 → z領域で回路設計。(「Z変換」参照)

↑
ラプラス変換領域や、Z変換領域で表した H(s) や H(z) などを伝達関数と呼ぶ。
```
