---
title: "2階常微分方程式"
source_url: "https://ufcpp.net/study/math/analysis/diffsecond/"
content_type: "Article"
published_at: "2015-05-06T14:16:41"
updated_at: "2015-05-06T14:16:41"
tags: []
umbraco_id: 1466
parent_id: 1464
sort_order: 1
aliases:
  - "/analysis/diffsecond"
  - "/analysis/diffsecond.html"
  - "/math/analysis/diffsecond/"
  - "/study/analysis/diffsecond"
  - "/study/analysis/diffsecond.html"
---

# 2階常微分方程式

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

一番簡単で、機械的な解法が知られている定数係数線形常微分方程式の中でも、
基礎中の基礎となる2階の斉次常微分方程式
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>u
      <span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u
      <span class="normal">+</span>
      qu
      <span class="normal">=</span><span class="normal">0</span>
    </div>
について説明します。


## <a id="sec-generated-title-2"></a> <a id="first"></a>1階の場合

まずは1階の場合、すなわち
<span class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u <span class="normal">+</span> pu <span class="normal">=</span><span class="normal">0</span>
      </span>
の解法について考えてみましょう。

これはいわゆる変数分離形という奴になっていて、
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>u</td></tr></table><span class="normal">d</span>u <span class="normal">=</span><span class="normal">−</span>p<span class="normal">d</span>t
      </span>
と変形して両辺を積分することで解くことが可能です。
すなわち、積分定数を <span class="math">C</span> として、
<span class="math">
        <span class="normal">log</span>u <span class="normal">=</span><span class="normal">−</span>pt <span class="normal">+</span> C
      </span>
が解です。
よって、
<span class="math">
        u <span class="normal">=</span><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>pt <span class="normal">+</span> C
        <span class="paren" style="font-size:em;">)</span>
      </span>
なわけですが、
<span class="math">
        A <span class="normal">=</span><span class="normal">e</span><sup>C</sup>
      </span> というように積分定数を置き換えると、
結局、
<span class="math">
        u <span class="normal">=</span> A<span class="normal">e</span><sup>
          <span class="normal">−</span>pt
        </sup>
      </span>
と表されます。


## <a id="sec-generated-title-3"></a> <a id="second"></a>2階の場合

「[1階の場合](#first)」で、
<span class="math">
        u <span class="normal">=</span> A<span class="normal">e</span><sup>
          <span class="normal">−</span>pt
        </sup>
      </span>
という解が得られたので、それをヒントに2階の場合
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>u
      <span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u
      <span class="normal">+</span>
      qu
      <span class="normal">=</span><span class="normal">0</span>
    </div>
を解いてみましょう。

1階の場合の解が指数関数になったので、
2階の場合も同じく指数関数によって解が得られないか試してみましょう。
上述の微分方程式に、
<span class="math">
        u<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> A<span class="normal">e</span><sup>xt</sup>
      </span>
を代入してみます。
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>
      A<span class="normal">e</span><sup>xt</sup><span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>A<span class="normal">e</span><sup>xt</sup><span class="normal">+</span>
      qA<span class="normal">e</span><sup>xt</sup><span class="normal">=</span>
      A<span class="normal">e</span><sup>xt</sup><span class="paren" style="font-size:em;">(</span>
        x<sup><span class="normal">2</span></sup><span class="normal">+</span>
        px
        <span class="normal">+</span>
        q
      <span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span>
    </div>
この等式が常に成り立つようにしたければ、
<span class="math">
        x<sup><span class="normal">2</span></sup><span class="normal">+</span>
        px
        <span class="normal">+</span>
        q
      </span>
が 0 ならばいいわけですから、
結局、この代数方程式を解く問題に帰着されます。
この、
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>
            <sup>k</sup>
          </td></tr><tr><td>
            <span class="normal">d</span>t<sup>k</sup>
          </td></tr></table>u
      </span>
を
<span class="math">
        x<sup>k</sup>
      </span>
置き換えてえられる代数方程式
<span class="math">
        x<sup><span class="normal">2</span></sup><span class="normal">+</span>
        px
        <span class="normal">+</span>
        q
        <span class="normal">=</span><span class="normal">0</span>
      </span>
を、（微分方程式の）特性方程式といいます。

2次方程式の解は2つありますから、
結局、
特性方程式の2つの解を <span class="math">α, β</span> として、
微分方程式の解は
<div class="math">
      <em>
        u
        <span class="normal">=</span>
        A<span class="normal">e</span><sup>αt</sup><span class="normal">+</span>
        B<span class="normal">e</span><sup>βt</sup>
      </em>
    </div>
（<span class="math">A, B</span> は初期値によって決まる定数）
となります。

ちなみに、一般にも、<span class="math">N</span> 階微分方程式の特性方程式は <span class="math">N</span> 次の代数方程式で、
その <span class="math">N</span> 個の解を <span class="math">
        x<sub>i</sub>
      </span> とすると、
微分方程式の解は
<span class="math">
        u
        <span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
            i <span class="normal">=</span> 1
          </td></tr></table>
        A<sub>i</sub><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>
          x<sub>i</sub>t
        <span class="paren" style="font-size:em;">)</span>。
      </span>
となります。


## <a id="sec-generated-title-4"></a> <a id="imaginary"></a>虚数解の場合

特性方程式の解が実数の場合には、前節の通り、
微分方程式の解が指数関数の和になります。
では、虚数解の場合にはどうなるのでしょうか。

オイラーの公式：
<span class="math">
        <span class="normal">exp</span>ix <span class="normal">=</span><span class="normal">cos</span>x <span class="normal">+</span> i <span class="normal">sin</span>x
      </span> を使えば、
虚数解の場合でも前節の方法で解けるんですが、
ここではあえて、オイラーの公式を知らないものとして、
実関数の範囲で解くことを考えます。

まず、簡単な例として、以下の微分方程式を考えてみましょう。
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>
      u
      <span class="normal">=</span><span class="normal">−</span>
      ω<sup><span class="normal">2</span></sup>
      u
    </div>
これの特性方程式の解は <span class="math">
        <span class="normal">±</span>iω
      </span> になります。
三角関数の微分の性質を覚えているなら分かると思いますが、
<span class="math">
        A<span class="normal">sin</span>ωt <span class="normal">+</span> B<span class="normal">cos</span>ωt
      </span>
が解になります。
いわゆる単振動って奴ですね。

で、一般の場合に戻りましょう。
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>u
      <span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u
      <span class="normal">+</span>
      qu
      <span class="normal">=</span><span class="normal">0</span>
    </div>
このときどうするかというと、
1階のときの解が
<span class="math">
        u <span class="normal">=</span> A<span class="normal">e</span><sup>xt</sup>
      </span>
なので、これと先ほどの結果（三角関数が解になる）を組み合わせて、
<span class="math">
        u <span class="normal">=</span> A<span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
      </span>
と置いてみます。すると、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>
      u
      <span class="normal">=</span>
      Aσ<span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
      <span class="normal">−</span>
      Aω<span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
    </div><div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>
      u
      <span class="normal">=</span>
      A<span class="paren" style="font-size:em;">(</span>
        σ<sup><span class="normal">2</span></sup> <span class="normal">−</span> ω<sup><span class="normal">2</span></sup>
      <span class="paren" style="font-size:em;">)</span><span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
      <span class="normal">−</span><span class="normal">2</span>Aωσ<span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
    </div>
なので、
これを微分方程式に代入して、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>u
      <span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u
      <span class="normal">+</span>
      qu
      <span class="normal">=</span>
      A<span class="paren" style="font-size:em;">(</span>
        σ<sup><span class="normal">2</span></sup> <span class="normal">−</span> ω<sup><span class="normal">2</span></sup>
        <span class="normal">+</span>
        pσ
        <span class="normal">+</span>
        q
      <span class="paren" style="font-size:em;">)</span><span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
      <span class="normal">−</span>
      A
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">2</span>ωσ
        <span class="normal">+</span>
        pω
      <span class="paren" style="font-size:em;">)</span><span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
    </div>
が得られます。
ちなみに、cos の代わりに sin を使って
<span class="math">
        u <span class="normal">=</span> A<span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
      </span>
とすると、今度は、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>u
      <span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u
      <span class="normal">+</span>
      qu
      <span class="normal">=</span>
      A<span class="paren" style="font-size:em;">(</span>
        σ<sup><span class="normal">2</span></sup> <span class="normal">−</span> ω<sup><span class="normal">2</span></sup>
        <span class="normal">+</span>
        pσ
        <span class="normal">+</span>
        q
      <span class="paren" style="font-size:em;">)</span><span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
      <span class="normal">+</span>
      A
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">2</span>ωσ
        <span class="normal">+</span>
        pω
      <span class="paren" style="font-size:em;">)</span><span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
    </div>
になります。
いずれにしろ、
<span class="math">
        σ<sup><span class="normal">2</span></sup> <span class="normal">−</span> ω<sup><span class="normal">2</span></sup>
        <span class="normal">+</span>
        pσ
        <span class="normal">+</span>
        q
        <span class="normal">=</span><span class="normal">0</span>
      </span>
かつ
<span class="math">
        <span class="normal">2</span>ωσ
        <span class="normal">+</span>
        pω
        <span class="normal">=</span><span class="normal">0</span>
      </span>
になって欲しいわけですが、
実はこの2式、
<span class="math">
        x
        <span class="normal">=</span>
        σ <span class="normal">+</span> iω
      </span>
とおいたときの、
<span class="math">
        x<sup><span class="normal">2</span></sup><span class="normal">+</span>
        px
        <span class="normal">+</span>
        q
        <span class="normal">=</span><span class="normal">0</span>
      </span>
の実部と虚部になっています。
これは見ての通り、微分方程式の特性方程式です。

結局、
特性方程式
<span class="math">
        x<sup><span class="normal">2</span></sup><span class="normal">+</span>
        px
        <span class="normal">+</span>
        q
        <span class="normal">=</span><span class="normal">0</span>
      </span>
の解
<span class="math">
        x
        <span class="normal">=</span>
        σ <span class="normal">±</span> iω
      </span>
を求めて、その実部・虚部 <span class="math">σ, ω</span> を使って、
<div class="math">
      <em>
        u
        <span class="normal">=</span>
        A<span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
        <span class="normal">+</span>
        B<span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
      </em>
    </div>
（<span class="math">A, B</span> は初期値によって決まる定数）
が微分方程式の解になります。


## <a id="sec-generated-title-5"></a> <a id="Euler"></a>オイラーの公式

<span class="math">a</span> が実数のとき、
<span class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u <span class="normal">=</span> au
      </span>
の解は
<span class="math">
        <span class="normal">exp</span>at
      </span> でした。
これを複素関数に拡張するとするなら、
<span class="math">α</span> を複素数として、
<span class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u <span class="normal">=</span> αu
      </span>
の解をもって複素数の指数関数
<span class="math">
        <span class="normal">exp</span>αt
      </span> で定義するのが自然でしょう。

こうして定義した複素指数関数を使って、
「[2階の場合](#second)」の解き方で
2階微分方程式
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>
          <sup><span class="normal">2</span></sup>
        </td></tr><tr><td>
          <span class="normal">d</span>t<sup><span class="normal">2</span></sup>
        </td></tr></table>u
      <span class="normal">+</span>
      p<table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u
      <span class="normal">+</span>
      qu
      <span class="normal">=</span><span class="normal">0</span>
    </div>
の解を求めると、
特性方程式の解を
<span class="math">
        x
        <span class="normal">=</span>
        σ <span class="normal">±</span> iω
      </span>
として、
<div class="math">
      u
      <span class="normal">=</span>
      A <span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>
        σ <span class="normal">+</span> iω
      <span class="paren" style="font-size:em;">)</span>t
      <span class="normal">+</span>
      B <span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>
        σ <span class="normal">−</span> iω
      <span class="paren" style="font-size:em;">)</span>t
    </div>
となります。
これと、
「[虚数解の場合](#imaginary)」の解き方で求めた解
<div class="math">
      u
      <span class="normal">=</span>
      C<span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
      <span class="normal">+</span>
      D<span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
    </div>
とは、初期値が同じなら一致しているはずです。
（<span class="math">n</span> 階の微分方程式の解は、
任意定数をちょうど <span class="math">n</span> 個だけ含む。
また、初期値が同じなら、全体で解が一致する。）
なので、初期値が等しいとしたときの
2組の定数 <span class="math">A, B</span> と <span class="math">C, D</span> の関係を求めるために、
<span class="math">
        u<span class="paren" style="font-size:em;">(</span>
          <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
と
<span class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u<span class="paren" style="font-size:em;">(</span>
          <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
の値を比べてみましょう。
まず、
<span class="math">
        u<span class="paren" style="font-size:em;">(</span>
          <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
とすると、
<div class="math">
      A <span class="normal">+</span> B <span class="normal">=</span> C
    </div>
一方、
<span class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>u<span class="paren" style="font-size:em;">(</span>
          <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
とすると、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        σ <span class="normal">+</span> iω
      <span class="paren" style="font-size:em;">)</span>A
      <span class="normal">+</span><span class="paren" style="font-size:em;">(</span>
        σ <span class="normal">−</span> iω
      <span class="paren" style="font-size:em;">)</span>B
      <span class="normal">=</span>
      σC <span class="normal">+</span> ωD
    </div>
これに対して、
<span class="math">
        A <span class="normal">+</span> B <span class="normal">=</span> C
      </span>
であることを使うと、結局、
<div class="math">
      i
      <span class="paren" style="font-size:em;">(</span>
        A
        <span class="normal">−</span>
        B
      <span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
      D
    </div>
となります。
これを元の式に代入すると、
<div class="math">
      A <span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>
        σ <span class="normal">+</span> iω
      <span class="paren" style="font-size:em;">)</span>t
      <span class="normal">+</span>
      B <span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>
        σ <span class="normal">−</span> iω
      <span class="paren" style="font-size:em;">)</span>t
      <span class="normal">=</span>
      A <span class="normal">exp</span>σt <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>ωt <span class="normal">+</span> i<span class="normal">sin</span>ωt
      <span class="paren" style="font-size:em;">)</span><span class="normal">+</span>
      B <span class="normal">exp</span>σt <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>ωt <span class="normal">−</span> i<span class="normal">sin</span>ωt
      <span class="paren" style="font-size:em;">)</span>
    </div>
となりますが、
ここで、
<span class="math">
        <span class="normal">exp</span>iωt
        <span class="normal">=</span><span class="normal">cos</span>ωt <span class="normal">+</span> i<span class="normal">sin</span>ωt
      </span>
とおくなら、
この等式が常に成り立つことが分かります。
この、
<div class="math">
      <em>
        <span class="normal">exp</span>iωt
        <span class="normal">=</span><span class="normal">cos</span>ωt <span class="normal">+</span> i<span class="normal">sin</span>ωt
      </em>
    </div>
という式を、オイラーの公式と呼びます。

余談：
オイラーとかガウスは業績が多すぎて、
「オイラー/ガウスの定理/公式」とか言われても、
「どのオイラー/ガウスの定理/公式？」って感じではあるんですが。

余談2：
このオイラーの公式は、
指数関数や三角関数のテイラー展開式からも導けます。


## <a id="sec-generated-title-6"></a> <a id="double"></a>重解の場合

証明とかはなしで事実だけ述べますが、
<span class="math">n</span> 階の線形微分方程式の解は、<span class="math">n</span> 個の任意定数を含みます。
（定数の値は初期値などの条件によって決まります。
<span class="math">n</span> 個の定数を決めるには、
<span class="math">
        n<span class="normal">−</span><span class="normal">1</span>
      </span> 階までの微分の初期値を指定したりします。）

では、2階の場合で、重解を持つ場合はどうしましょう。
特性方程式の解を <span class="math">α, β</span> として、
2階の微分方程式の解は
<span class="math">
        u
        <span class="normal">=</span>
        A<span class="normal">e</span><sup>αt</sup><span class="normal">+</span>
        B<span class="normal">e</span><sup>βt</sup>
      </span>
になるわけですが、
重解（<span class="math">
        α <span class="normal">=</span> β
      </span>）の場合、
<span class="math">
        u
        <span class="normal">=</span>
        A<span class="normal">e</span><sup>αt</sup>
      </span>
となってしまって、任意定数が1つしか出てきません。

経験的に知られている結果だけ言ってしまうなら、
特性方程式が重解を持つ場合の解は、
<div class="math">
        <em>
          u
          <span class="normal">=</span>
        A<span class="normal">e</span><sup>αt</sup><span class="normal">+</span>
        B t<span class="normal">e</span><sup>αt</sup>
        </em>
      </div>
になります。
実際、微分方程式に
<span class="math">
        t<span class="normal">e</span><sup>αt</sup>
      </span> を代入すると、ちゃんと 0 になるので、一度計算してみてください。

ここでは経験則に基づく結果だけになってしまいましたが、
「「[ラプラス変換](../../sp/dsp/laplace.md#Laplace)」」とか行列による解法について学べば、
もう少し納得の行く過程を知ることができます。


## <a id="sec-generated-title-7"></a> <a id="summary"></a>まとめ

特性方程式を使う。

* 実数解：<span class="math">
          A<span class="normal">e</span><sup>αt</sup><span class="normal">+</span>
          B <span class="normal">e</span><sup>βt</sup>
        </span>

* 重解：<span class="math">
          A<span class="normal">e</span><sup>αt</sup><span class="normal">+</span>
          B t<span class="normal">e</span><sup>αt</sup>
        </span>

* 複素数解：<span class="math">
          A<span class="normal">e</span><sup>σt</sup><span class="normal">cos</span>ωt
          <span class="normal">+</span>
          B<span class="normal">e</span><sup>σt</sup><span class="normal">sin</span>ωt
        </span>



## <a id="sec-generated-title-8"></a> <a id="plan"></a>執筆予定

<pre>
2階の線形微分方程式になる例を

・速度抵抗付きのバネ
- 粘性のある液体の中にバネを
- ダンパー付きの扉

・RCL 回路
↑の例だと、液体の粘性を自由に制御したりってのは難しいけど、
こっちだと R の抵抗値を自由に変えれるんで、
特性方程式の解によって電圧の波形がどう変わるかを観測しやすくて面白い。

R の値に応じて、
単振動 → 減衰振動 → 過減衰 → 減衰
と変化する様が観測できる。
    </pre>
