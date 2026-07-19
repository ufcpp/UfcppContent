---
title: "微小差分"
source_url: "https://ufcpp.net/study/math/analysis/infinitesimal/"
content_type: "Article"
published_at: "2015-05-06T14:16:46"
updated_at: "2015-05-06T14:16:46"
tags: []
umbraco_id: 1468
parent_id: 1464
sort_order: 3
aliases:
  - "/analysis/infinitesimal"
  - "/analysis/infinitesimal.html"
  - "/math/analysis/infinitesimal/"
  - "/study/analysis/infinitesimal"
  - "/study/analysis/infinitesimal.html"
---

# 微小差分

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「微分」という言葉には2つの意味があります。
というのも、日本語の「分」には「分割する」という操作を示す意味と、
「部分」という物を表す意味があるためです。
すなわち、微小に分割する操作「微分演算」と、
微小な部分「微小差分」という物を表す2つの意味です。

ここで話をするのは後者、すなわち「微小差分」の話。
微分方程式や積分の式の中に出てくる <span class="math">
        <span class="normal">d</span>t
      </span> という形をしたやつです。
最初、微分という物を習った時、
微分演算は <span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            x<span class="paren" style="font-size:em;">(</span>
              t <span class="normal">+</span> Δt
            <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>Δt</td></tr></table>
      </span> という比の極限なので、
微小差分（<span class="math">
        x<span class="paren" style="font-size:em;">(</span>
          t <span class="normal">+</span> Δt
        <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x <span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> の極限である <span class="math">
        <span class="normal">d</span>x
      </span>）は
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>x
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table>
      </span> というように、比の形でしか意味をなさないというように習ったと思います。

なのに、今度は積分という物を習う時に、
<span class="math">
        <span class="normal">d</span>t
      </span> とかが単品で出てきたりしますよね。
さらには、微分方程式を習うと、
<span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x <span class="normal">=</span> g<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
      </span> なんて式まで出てきます。
この <span class="math">
        <span class="normal">d</span>t
      </span> っていうやつを微小差分などと呼ぶわけですが、
ここでは、なぜわざわざこんな書き方をするのかという辺りの説明をしていきます。


## <a id="sec-generated-title-2"></a> <a id="differential"></a>微小差分

概要で軽く触れましたが、<strong id="difop" class="keyword">微分演算</strong>（derivation, differentiation）の定義は以下の通りです。
<div class="math">
      f'<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          x<span class="paren" style="font-size:em;">(</span>
            t <span class="normal">+</span> Δt
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δt</td></tr></table>
    </div>
微分演算の結果得られる関数 <span class="math">f'</span> を<strong id="deriv" class="keyword">導関数</strong>（derivative, derived function）と呼びます。
そして、
<strong id="diff" class="keyword">微分</strong>（differential） <span class="math">
        <span class="normal">d</span>t, <span class="normal">d</span>x
      </span> というのは、あえて言うなら、以下のような物になります。
<div class="math">
      <span class="normal">d</span>t
      <span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      Δt
    </div><div class="math">
      <span class="normal">d</span>x
      <span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      x<span class="paren" style="font-size:em;">(</span>
        t <span class="normal">+</span> Δt
      <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x <span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
    </div>
日本語では「微分」という単語だけで微分演算をさす場合もありますし、
英語でもやっぱり differential だと「微分の」という意味合いに取られるので、
<strong id="infinitesimal" class="keyword">微小差分</strong>（infinitesimal difference）あるいは微小変化（infinitesimal change）ということもあります。
（infinitesimal の訳語としては「無限小」の方が適切。
「微分」という言葉とニュアンスが近いので、ここではあえて微小と訳します。）

もちろん、微小差分は通常の文脈では 0 にしかなりません。
これら微小差分は、あくまで比の形 <span class="math">
        <span class="normal">d</span>x / <span class="normal">d</span>t
      </span> あるいは積分中でのみ意味を持ちます。


## <a id="sec-generated-title-3"></a> <a id="reduce"></a>合成関数の微分（微小差分の約分）

比の形でしか意味を持たないものをなぜわざわざ分母と分子に分けて書くかというと、
微小差分同士で形式的に約分ができるからです。

このことを説明するために、まず合成関数の微分を見てみましょう。
合成関数、要するに関数 <span class="math">
        y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> の中に別の関数
<span class="math">
        x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> が入ったような関数
<span class="math">
        y<span class="paren" style="font-size:em;">(</span>
          x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> の微分は以下のようになります。
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          y<span class="paren" style="font-size:em;">(</span>
            x<span class="paren" style="font-size:em;">(</span>
              t <span class="normal">+</span> Δt
            <span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> y<span class="paren" style="font-size:em;">(</span>
            x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δt</td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          y<span class="paren" style="font-size:em;">(</span>
            x<span class="paren" style="font-size:em;">(</span>
              t <span class="normal">+</span> Δt
            <span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> y<span class="paren" style="font-size:em;">(</span>
            x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>
          x<span class="paren" style="font-size:em;">(</span>
            t <span class="normal">+</span> Δt
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          x<span class="paren" style="font-size:em;">(</span>
            t <span class="normal">+</span> Δt
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δt</td></tr></table>
    </div>
ここで、<span class="math">
        x<span class="paren" style="font-size:em;">(</span>
          t <span class="normal">+</span> Δt
        <span class="paren" style="font-size:em;">)</span><span class="normal">=</span> x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">+</span> Δx
      </span> と置くと、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δx → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          y<span class="paren" style="font-size:em;">(</span>
            x <span class="normal">+</span> Δx
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> y<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δx</td></tr></table>
      <span class="normal">×</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          x<span class="paren" style="font-size:em;">(</span>
            t <span class="normal">+</span> Δt
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>Δt</td></tr></table>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
    </div>
となります。
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>y
          </td></tr><tr><td>
            <span class="normal">d</span>x
          </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>x
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>y
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table>
      </span>
ですので、<em>
        一見すると <span class="math">
          <span class="normal">d</span>x
        </span> が約分されて消えているように見えますよね
      </em>。
これ、実際に約分してるんです。
極限には、<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub"></td></tr></table>
      </span> を取る操作と、約分（乗除算）の順番を入れ替えることが出来るという性質がありますので、こういうことが可能なわけです。


## <a id="sec-generated-title-4"></a> <a id="inverse"></a>逆関数の微分（微小差分の逆数）

逆関数の微分も、合成関数の微分と似たような式になります。
先ほどの式に、<span class="math">
        y <span class="normal">=</span> t
      </span> を代入すると、
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>t
          </td></tr><tr><td>
            <span class="normal">d</span>x
          </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>x
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>t
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table><span class="normal">=</span>
        1
      </span>
となります。
要するに、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>t
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <span class="normal">=</span>
      1 ÷
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
    </div>
これも、微分の定義式に立ち返ってみてみると、以下のようになります。
まず、<span class="math">x</span> と <span class="math">t</span> の間に <span class="math">
        x <span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> という関係式があるとき、
<span class="math">
        t <span class="normal">=</span> f<sup>
          <span class="normal">−</span>1
        </sup><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
      </span> なので、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>t
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δx → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          f<sup>
            <span class="normal">−</span>1
          </sup><span class="paren" style="font-size:em;">(</span>
            x <span class="normal">+</span> Δx
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> f<sup>
            <span class="normal">−</span>1
          </sup><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>
          x <span class="normal">+</span> Δx <span class="normal">−</span> x
        </td></tr></table>
    </div>
ここで、<span class="math">
        t <span class="normal">=</span> f<sup>
          <span class="normal">−</span>1
        </sup><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>, Δt <span class="normal">=</span> f<sup>
          <span class="normal">−</span>1
        </sup><span class="paren" style="font-size:em;">(</span>
          x <span class="normal">+</span> Δx
        <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> f<sup>
          <span class="normal">−</span>1
        </sup><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
      </span> と置くと、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>t
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δx → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">Δt</td></tr><tr><td>
          f<span class="paren" style="font-size:em;">(</span>
            t <span class="normal">+</span> Δt
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">Δt</td></tr><tr><td>Δx</td></tr></table>
    </div>
合成関数の微分の場合と同じく、極限を取る操作と逆数を取る操作は順序を入れ替えることが出来るので、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>t
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">Δt</td></tr><tr><td>Δx</td></tr></table>
      <span class="normal">=</span>
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      1÷
      <table class="frac" summary="fraction"><tr><td class="num">Δx</td></tr><tr><td>Δt</td></tr></table><span class="normal">=</span>
      1÷
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">Δx</td></tr><tr><td>Δt</td></tr></table><span class="normal">=</span>
      1÷
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
    </div>
となるわけです。


## <a id="sec-generated-title-5"></a> <a id="definite"></a>定積分（積分中の微小差分）

<strong id="definite" class="keyword">定積分</strong>（definite integral）の定義（正確にはリーマン積分（Riemann integral）と呼ばれる定義）は以下のようになります。
まず、区間 <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>
      </span> の間を <span class="math">n</span> 個に分割し、
分割点の座標を <span class="math">
        t<sub>0</sub><span class="normal">=</span> a, t<sub>1</sub>, … , t<sub>
          n <span class="normal">−</span>1
        </sub>, t<sub>n</sub><span class="normal">=</span> b
      </span> とします。
このとき、関数 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> の <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>
      </span> における積分を
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table>
      <table class="sigma" summary="sum"><tr><td class="sigmasub">
          n <span class="normal">−</span> 1
        </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          i <span class="normal">=</span> 0
        </td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>
        t<sub>i</sub>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
        t<sub>
          i <span class="normal">+</span> 1
        </sub><span class="normal">−</span> t<sub>i</sub>
      <span class="paren" style="font-size:em;">)</span>
    </div>
<span class="math">
        Δt<sub>i</sub><span class="normal">=</span> t<sub>
          i <span class="normal">+</span> 1
        </sub><span class="normal">−</span> t<sub>i</sub>
      </span> と置くと、
より簡単に書け、
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table>
      <table class="sigma" summary="sum"><tr><td class="sigmasub">
          n <span class="normal">−</span> 1
        </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          i <span class="normal">=</span> 0
        </td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>
        t<sub>i</sub>
      <span class="paren" style="font-size:em;">)</span>
      Δt<sub>i</sub>
    </div>
となります。
（注：リーマン積分の正確な定義はもう少し複雑です。
<span class="math">
        f<span class="paren" style="font-size:em;">(</span>
          t<sub>i</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span> の部分を、
<span class="math">
        <table class="sigma" summary="statement under a function"><tr><td><span class="normal">sup</span></td></tr><tr><td class="sigmasub">
          t<sub>i</sub><span class="normal">&lt;</span> t <span class="normal">&lt;</span> t<sub>i</sub>
        </td></tr></table>
        f<span class="paren" style="font-size:em;">(</span>
          t<sub>i</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span> にしたものを上積分、
<span class="math">
        <table class="sigma" summary="statement under a function"><tr><td><span class="normal">inf</span></td></tr><tr><td class="sigmasub">
          t<sub>i</sub><span class="normal">&lt;</span> t <span class="normal">&lt;</span> t<sub>i</sub>
        </td></tr></table>
        f<span class="paren" style="font-size:em;">(</span>
          t<sub>i</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span> にしたものを下積分と呼んで、
この2つが一致する時に限り、積分可能であるとし、その値を積分値にします。
このとき、上述の定義による積分値も上積分および下積分と一致します。）

この式、<span class="math">n</span> を大きくする（→∞）ということは、
分割数をどんどん増やすということです。
このとき、分割した各区間幅 <span class="math">
        Δt<sub>i</sub>
      </span> はどんどん小さくなります。
したがって、以下のように表すこともできます。
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
      <table class="sigma" summary="sum"><tr><td class="sigmasub">
          n <span class="normal">−</span> 1
        </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          i <span class="normal">=</span> 0
        </td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>
        t<sub>i</sub>
      <span class="paren" style="font-size:em;">)</span>
      Δt<sub>i</sub>
    </div>
この、
<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
        <table class="sigma" summary="sum"><tr><td class="sigmasub">
            n <span class="normal">−</span> 1
          </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
            i <span class="normal">=</span> 0
          </td></tr></table>
      </span>
の部分を
<span class="math">
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      </span>
で、
<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>
        Δt
      </span>
の部分を
<span class="math">
        <span class="normal">d</span>t
      </span>
で表そうというのが、積分の記号
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
    </div>
の意味です。
要するに、<em>
        積分記号中の <span class="math">
          <span class="normal">d</span>t
        </span> の意味は、
        <span class="math">
          <span class="normal">d</span>t <span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δt → 0</td></tr></table>Δt
        </span> です
      </em>。
見ての通り、微分演算の時に説明した微小差分の意味と同じになります。


## <a id="sec-generated-title-6"></a> <a id="indefinite"></a>定積分と不定積分（微分演算中と積分中の微小差分）

高校では、定積分と不定積分を特に区別して教えませんが、
この2つは定義上、全く異なるものです。
定積分は、前節で説明したような、関数の和の極限を取る操作のことを言います。
一方、<strong id="indefinite" class="keyword">不定積分</strong>（antiderivative, indefinite integral）というのは、微分演算の逆演算として定義されます（英語では antiderivative というあたり、本当に微分の逆演算）。
すなわち、<span class="math">
        <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>F<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> となるような関数 <span class="math">F</span> を関数 <span class="math">f</span> の<strong id="primitive" class="keyword">原始関数</strong>（primitive）と呼び、
<span class="math">f</span> の原始関数を求めることを不定積分と呼びます。

定積分と不定積分の関係を調べるために、
導関数の定積分がどうなるかを見てみましょう。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>f
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">d</span>t
    </div>
という式は、定義式に当てはめてみると、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>f
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">d</span>t
      <span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">
          n <span class="normal">−</span> 1
        </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          i <span class="normal">=</span> 0
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          f<span class="paren" style="font-size:em;">(</span>
            t<sub>
              i <span class="normal">+</span> 1
            </sub>
          <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> f<span class="paren" style="font-size:em;">(</span>
            t<sub>i</sub>
          <span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>
          t<sub>
            i <span class="normal">+</span> 1
          </sub><span class="normal">−</span> t<sub>i</sub>
        </td></tr></table><span class="normal">×</span><span class="paren" style="font-size:em;">(</span>
        t<sub>
          i <span class="normal">+</span> 1
        </sub><span class="normal">−</span> t<sub>i</sub>
      <span class="paren" style="font-size:em;">)</span>
    </div>
というように書けます。
これも、極限操作と通分の順序を入れ替えると、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>f
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">d</span>t
      <span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">
          n <span class="normal">−</span> 1
        </td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          i <span class="normal">=</span> 0
        </td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>
        t<sub>
          i <span class="normal">+</span> 1
        </sub>
      <span class="paren" style="font-size:em;">)</span><span class="normal">−</span> f<span class="paren" style="font-size:em;">(</span>
        t<sub>i</sub>
      <span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table><span class="normal">d</span>f
      <span class="normal">=</span>
      f<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span><span class="normal">−</span> f<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
    </div>
という結果が得られます。
<em>微分法で出てくる微小差分と定積分中に出てくる微小差分も通分して消すことが可能</em>なわけです。

ここで、<span class="math">f</span> は
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>f
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table>
      </span>
の不定積分になっているので、
この式は（元々全然別物として定義されている）定積分と不定積分を関係付ける式であるといえます。
定積分を定義式どおりに計算するよりも、
原始関数を求めてから計算する方が簡単なので、
この関係式は非常に有益なものとなっています。
そのため、この関係式は微分積分学の基本定理（fundamental theorem of calculus）と呼ばれています。

この基本定理により、定積分と不定積分が関係付けられるため、
不定積分も記号 <span class="math">
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      </span> で表します。
<div class="math">
      F<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
      <span class="normal">+</span>
      C
    </div>
ただし、原始関数には定数分の不定性が残りますので、
その不定性を表すための未知定数 <span class="math">C</span> を付けて表します。
（この未知定数 <span class="math">C</span> を積分定数と呼びます。）


## <a id="sec-generated-title-7"></a> <a id="separation"></a>変数分離形の微分方程式（微分方程式中の微小差分）

解析的に解くことのできる微分方程式のパターンの1つに、変数分離形（separation of variables）というものがあります。
変数分離形の微分方程式は以下のような式で表されます。
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <span class="normal">=</span>
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
      g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
    </div>
これは以下のようにして、不定積分することで解きます。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
        </td></tr></table>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x
    </div>
要するに、
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>y
          </td></tr><tr><td>
            <span class="normal">d</span>x
          </td></tr></table><span class="normal">=</span>
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
      </span>
の両辺を
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>x
          </td></tr><tr><td>
            g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
          </td></tr></table>
      </span>
倍してから不定積分しているわけです。

ここで、
両辺 <span class="math">
        <span class="normal">d</span>x
      </span> 倍するっていう操作はしていいの？という疑問が生じるかと思います。
これに関しては、以下のように考えることもできます。

まず、<span class="math">x, y</span> が別の変数 <span class="math">t</span> に従属する変数 <span class="math">
        x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, y<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
      </span> だと考えて、
この式の左辺を
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
      <span class="normal">=</span>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>t
        </td></tr><tr><td>
          <span class="normal">d</span>x
        </td></tr></table>
    </div>
と2つに分けます。
そして、式の両辺を
<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
        </td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>x
          </td></tr><tr><td>
            <span class="normal">d</span>t
          </td></tr></table>
      </span>
倍して、
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
      </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">=</span>
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
    </div>
最後に両辺を <span class="math">t</span> で不定積分します。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
      </td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table>
      <span class="normal">d</span>t
      <span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>x
        </td></tr><tr><td>
          <span class="normal">d</span>t
        </td></tr></table><span class="normal">d</span>t
    </div>
こうすると、どうせ <span class="math">
        <span class="normal">d</span>t
      </span> は通分されて消えてしまい、
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">d</span>y
        </td></tr><tr><td>
          g<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span>
        </td></tr></table>
      <span class="normal">=</span>
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>x
    </div>
となります。

要するに、微分方程式中に単品で出てくる微小差分（<span class="math">
        <span class="normal">d</span>x, <span class="normal">d</span>y
      </span> など）には、
最終的に不定積分時に消えてしまう別の微小差分 <span class="math">
        <span class="normal">d</span>t
      </span> が含まれていて、
表記上省略されていると考えることができます。

まあ、実際はこんな回りくどい考え方をしなくても、
微分方程式を解くときはどうせ最終的に不定積分するんだから、
<span class="math">
        <span class="normal">d</span>x, <span class="normal">d</span>y
      </span> が（比の形ではなく）単品で出てきてもいいんだよ、
と開き直るのもありだったりするんですが。


## <a id="sec-generated-title-8"></a> <a id="summary"></a>最後に

これまで散々、微小変分（<span class="math">
        <span class="normal">d</span>t
      </span> など）は比の形あるいは積分中でしか意味を成さない、
単品で出てきた場合にも最終的に積分されることが前提か、
分母の <span class="math">
        <span class="normal">d</span>t
      </span> 暗黙的に省略されているかであると話してきました。
古典的な微分積分学（calculus）上ではこれは事実で、単品の <span class="math">
        <span class="normal">d</span>t
      </span> は意味を持ちません。

しかしながら、現在では 0 に限りなく近い非 0 無限小を取り扱うことのできる理論「超準解析」や、
微小変分を拡張したような概念「測度」「微分形式」などがあって、
このような分野においては <span class="math">
        <span class="normal">d</span>t
      </span> 単品でもちゃんとした意味を持っています。
これらに関しては、このページの趣旨とは違ってきますので、
ここでは説明しませんが、いずれ別ページで説明するかもしれません。
興味があれば「超準解析」「測度」「微分形式」などをキーワードにして調べてみてください。
