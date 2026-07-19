---
title: "べき級数展開・留数"
source_url: "https://ufcpp.net/study/math/analysis/residue/"
content_type: "Article"
published_at: "2015-05-06T14:16:50"
updated_at: "2015-05-06T14:16:50"
tags: []
umbraco_id: 1470
parent_id: 1464
sort_order: 5
aliases:
  - "/analysis/residue"
  - "/analysis/residue.html"
  - "/math/analysis/residue/"
  - "/study/analysis/residue"
  - "/study/analysis/residue.html"
---

# べき級数展開・留数

## <a id="sec-generated-title-1"></a> <a id="int"></a>複素関数の積分

正則関数のところで説明していますが、
関数<span class="math">
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
      </span>が閉路<span class="math">C</span>に囲まれた領域内で正則なとき、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span><span class="normal">0</span>
    </div>
が成り立ちます。

別の見方をすると、<em>
        任意の関数<span class="math">
          f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </span>に対して、
        任意の閉路<span class="math">C</span>上での積分は、
        関数<span class="math">
          f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </span>の正則でない点が閉路<span class="math">C</span>に囲まれているかどうかだけで決まります
      </em>。
例えば、<span class="math">
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>z</td></tr></table>
      </span>は<span class="math">
        z<span class="normal">=</span><span class="normal">0</span>
      </span>以外で正則なので、
<span class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>z</td></tr></table><span class="normal">d</span>z
      </span>の値は、
<span class="math">C</span>が<span class="math">
        z<span class="normal">=</span><span class="normal">0</span>
      </span>を囲むか囲まないかだけで決まります。
そのため、このような積分は計算の容易な経路を適当に選んで計算してやればいいわけです。

例として、<span class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>z</td></tr></table><span class="normal">d</span>z
      </span>の値を計算してみましょう。
積分経路として、<span class="math">
        C<span class="normal">=</span><span class="paren" style="font-size:em;">{</span>
          z | <span class="normal">|</span>z<span class="normal">|</span><span class="normal">=</span>r
        <span class="paren" style="font-size:em;">}</span>
      </span>（<span class="math">r</span>は任意の正の実数）を選んでやると、
<span class="math">
        z<span class="normal">=</span>r e<sup>iθ</sup>, <span class="normal">0</span><span class="normal">≦</span>θ<span class="normal">≦</span><span class="normal">2</span>π
      </span>となりますから、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>z</td></tr></table>
      <span class="normal">d</span>z <span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> 
          <span class="normal">2</span>π
        </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
          <span class="normal">0</span>
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          ir e<sup>iθ</sup><span class="normal">d</span>θ
        </td></tr><tr><td>
          r e<sup>iθ</sup>
        </td></tr></table><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> 
          <span class="normal">2</span>π
        </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
          <span class="normal">0</span>
        </td></tr></table>i<span class="normal">d</span>θ <span class="normal">=</span><span class="normal">2</span>πi
    </div>
となり、したがって、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>z</td></tr></table>
      <span class="normal">d</span>z <span class="normal">=</span><span class="paren" style="font-size:1.5em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">
            <span class="normal">0</span>
          </span>  </td><td><span class="paren">(</span><span class="math">
            C <span class="normal">が</span> z<span class="normal">=</span><span class="normal">0</span> <span class="normal">を囲まないとき</span>
          </span><span class="paren">)</span></td></tr><tr><td><span class="math">
            <span class="normal">2</span>πi
          </span>  </td><td><span class="paren">(</span><span class="math">
            C <span class="normal">が</span> z<span class="normal">=</span><span class="normal">0</span> <span class="normal">を囲むとき</span>
          </span><span class="paren">)</span></td></tr></table>
    </div>
となります。


## <a id="sec-generated-title-2"></a> <a id="order"></a>極と零点の位数

関数<span class="math">f</span>に対して<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">z→a</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span>
      </span>が成り立っているとき、<span class="math">a</span>を<span class="math">f</span>の<strong id="zero" class="keyword">零点</strong>（zero, zero point）といいます。
このとき、<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">z→a</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">
            f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              z<span class="normal">−</span>a
            <span class="paren" style="font-size:em;">)</span>
            <sup>k</sup>
          </td></tr></table>
        <span class="normal">=</span>
        <span class="normal">0</span>
      </span>となる最大の自然数<span class="math">k</span>を<em>
        <span class="math">f</span>の零点<span class="math">a</span>での<strong id="order" class="keyword">位数</strong>
      </em>といいます。

同様に、関数<span class="math">f</span>に対して<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">z→b</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </td></tr></table>
        <span class="normal">=</span>
        <span class="normal">0</span>
      </span>が成り立っているとき、<span class="math">b</span>を<span class="math">f</span>の<strong id="pole" class="keyword">極</strong>（pole）といいます。
このとき、<span class="math">
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">z→b</td></tr></table>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="paren" style="font-size:em;">(</span>
              z<span class="normal">−</span>b
            <span class="paren" style="font-size:em;">)</span>
            <sup>k</sup>
          </td></tr><tr><td>
            f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
          </td></tr></table>
        <span class="normal">=</span>
        <span class="normal">0</span>
      </span>となる最大の自然数<span class="math">k</span>を<em>
        <span class="math">f</span>の極<span class="math">b</span>での位数
      </em>といいます。
すなわち、<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>f</td></tr></table>
      </span>の零点を<span class="math">f</span>の極といい、<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>f</td></tr></table>
      </span>の零点の位数を<span class="math">f</span>の極の位数といいます。

また、位数が<span class="math">N</span>である零点を「<span class="math">N</span>位の零点」といい、
位数が<span class="math">N</span>である極を「<span class="math">N</span>位の極」といいます。


## <a id="sec-generated-title-3"></a> <a id="z"></a>整式の積分

べき級数展開や留数の話をする前に、<span class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>z<sup>n</sup><span class="normal">d</span>z
      </span>（<span class="math">C</span>は原点を囲む閉路で、<span class="math">n</span>は整数）の計算結果を知っていると留数を理解しやすくなりますので、まずこのことについて述べます。

<span class="math">n</span>が<span class="math">
        <span class="normal">0</span>
      </span>以上の時には、<span class="math">
        z<sup>n</sup>
      </span>はすべての複素数<span class="math">z</span>に対して正則なので、閉路上での積分の値は常に<span class="math">
        <span class="normal">0</span>
      </span>になります。
また、<span class="math">
        n<span class="normal">=</span><span class="normal">−</span><span class="normal">1</span>
      </span>の時の結果は先ほど求めたように、<span class="math">
        <span class="normal">2</span>πi
      </span>になります。

それでは残る<span class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          z<sup>n</sup>
        </td></tr></table><span class="normal">d</span>z
      </span>（<span class="math">
        n<span class="normal">≧</span><span class="normal">2</span>
      </span>）の場合を考えてみましょう。
先ほどと同じように、積分経路として<span class="math">
        C<span class="normal">=</span><span class="paren" style="font-size:em;">{</span>
          z | <span class="normal">|</span>z<span class="normal">|</span><span class="normal">=</span>r
        <span class="paren" style="font-size:em;">}</span>
      </span>（<span class="math">r</span>は任意の正の実数）を選んでやると、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>z</td></tr></table>
      <span class="normal">d</span>z <span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> 
          <span class="normal">2</span>π
        </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
          <span class="normal">0</span>
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          ir e<sup>iθ</sup><span class="normal">d</span>θ
        </td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>
            r e<sup>iθ</sup>
          <span class="paren" style="font-size:em;">)</span>
          <sup>n</sup>
        </td></tr></table><span class="normal">=</span><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> 
          <span class="normal">2</span>π
        </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
          <span class="normal">0</span>
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          i<span class="normal">d</span>θ
        </td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>
            r e<sup>iθ</sup>
          <span class="paren" style="font-size:em;">)</span>
          <sup>
            n<span class="normal">−</span><span class="normal">1</span>
          </sup>
        </td></tr></table>
    </div>
となります。
ここで、この積分の値は<span class="math">r</span>の値によらず一定なわけですから、<span class="math">r → ∞</span>としてもこの積分の値は変わりません。
<span class="math">
        n<span class="normal">≧</span><span class="normal">2</span>
      </span>ですから、このとき<span class="math">
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          r<sup>
            n<span class="normal">−</span><span class="normal">1</span>
          </sup>
        </td></tr></table> → <span class="normal">0</span>
      </span>となります。
したがって、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        z<sup>n</sup>
      </td></tr></table>
      <span class="normal">d</span>z <span class="normal">=</span><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">r→∞</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> 
          <span class="normal">2</span>π
        </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
          <span class="normal">0</span>
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          i<span class="normal">d</span>θ
        </td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>
            r e<sup>iθ</sup>
          <span class="paren" style="font-size:em;">)</span>
          <sup>
            n<span class="normal">−</span><span class="normal">1</span>
          </sup>
        </td></tr></table><span class="normal">=</span><span class="normal">0</span>
    </div>
となります。
すなわち、<span class="math">
        n ≦ <span class="normal">−</span><span class="normal">2</span>
      </span> のとき、
原点を囲む閉路上での <span class="math">
        z<sup>n</sup>
      </span> の積分は <span class="math">
        <span class="normal">0</span>
      </span> になります。

以上の結果をまとめると、
<div class="math">
      <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>z<sup>n</sup><span class="normal">d</span>z <span class="normal">=</span><span class="paren" style="font-size:1.5em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">
            <span class="normal">0</span>
          </span>  </td><td><span class="paren">(</span><span class="math">
            n<span class="normal">≠</span><span class="normal">−</span><span class="normal">1</span>
          </span><span class="paren">)</span></td></tr><tr><td><span class="math">
            <span class="normal">2</span>πi
          </span>  </td><td><span class="paren">(</span><span class="math">
            n<span class="normal">=</span><span class="normal">−</span><span class="normal">1</span>
          </span><span class="paren">)</span></td></tr></table>
    </div>
となります。

また、今までは簡素化のため、原点を囲む閉路に限定して考えてきましたが、
任意の閉路について考えるために、
<span class="math">z</span>を<span class="math">
        z<span class="normal">−</span>ζ
      </span>に置き換えます。
その結果、変数変換の公式から直ちに、
<div class="math">
      <em>
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
        <span class="paren" style="font-size:em;">(</span>
          z<span class="normal">−</span>ζ
        <span class="paren" style="font-size:em;">)</span>
        <sup>n</sup>
        <span class="normal">d</span>z <span class="normal">=</span><span class="paren" style="font-size:1.5em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">
              <span class="normal">0</span>
            </span>  </td><td><span class="paren">(</span><span class="math">
              n<span class="normal">≠</span><span class="normal">−</span><span class="normal">1</span>
            </span><span class="paren">)</span></td></tr><tr><td><span class="math">
              <span class="normal">2</span>πi
            </span>  </td><td><span class="paren">(</span><span class="math">
              n<span class="normal">=</span><span class="normal">−</span><span class="normal">1</span>
            </span><span class="paren">)</span></td></tr></table>
      </em>
    </div>
という式が得られます。
ただし、 <span class="math">C</span>は<span class="math">ζ</span>を囲む任意の経路です。


## <a id="sec-generated-title-4"></a> <a id="Laurent"></a>ローラン展開

<span class="math">
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>
        <span class="paren" style="font-size:em;">(</span>
          z<span class="normal">−</span>ζ
        <span class="paren" style="font-size:em;">)</span>
        <sup>n</sup>
        <span class="normal">d</span>z
      </span>の値を計算するのと同じ方法
（<span class="math">
        z <span class="normal">=</span> r <span class="normal">e</span><sup>iθ</sup>
      </span> と置いて置換積分）
で、
<span class="math">f</span>が領域<span class="math">
        <span class="normal">0</span><span class="normal">&lt;</span><span class="normal">|</span>
          z<span class="normal">−</span>ζ
        <span class="normal">|</span><span class="normal">&lt;</span>R
      </span>上で正則であるとき、
<span class="math">
        <span class="normal">0</span><span class="normal">&lt;</span>r<span class="normal">&lt;</span>R
      </span>である任意の実数<span class="math">r</span>に対して、
<div class="math">
      <em>
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>πi
        </td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
            <span class="normal">|</span>
              z<span class="normal">−</span>ζ
            <span class="normal">|</span><span class="normal">=</span>r
          </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            f<span class="paren" style="font-size:em;">(</span>ζ<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            ζ<span class="normal">−</span>z
          </td></tr></table><span class="normal">d</span>ζ
      </em>
    </div>
という式が得られます。
（前節までの説明とは、<span class="math">z</span> と <span class="math">ζ</span> の位置が逆なので注意。）

また、この式を<span class="math">z</span>に関して <span class="math">n</span> 階微分することで
<div class="math">
      <em>
        f<sup>
          <span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </sup><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">
            n<span class="normal">!</span>
          </td></tr><tr><td>
            <span class="normal">2</span>πi
          </td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
            <span class="normal">|</span>
              z<span class="normal">−</span>ζ
            <span class="normal">|</span><span class="normal">=</span>r
          </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            f<span class="paren" style="font-size:em;">(</span>ζ<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              ζ<span class="normal">−</span>z
            <span class="paren" style="font-size:em;">)</span>
            <sup>
              n<span class="normal">+</span><span class="normal">1</span>
            </sup>
          </td></tr></table><span class="normal">d</span>ζ
      </em>
    </div>
という式が得られます。
ただし、<span class="math">
        f<sup>
          <span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </sup>
      </span>は<span class="math">f</span>を<span class="math">n</span>階微分したものです。

ところで、複素関数<span class="math">f</span>は、正則な<span class="math">
        z<span class="normal">=</span>ζ
      </span>の周りでテイラー展開
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          n<span class="normal">=</span><span class="normal">0</span>
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          f<sup>
            <span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
          </sup><span class="paren" style="font-size:em;">(</span>ζ<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>
          n<span class="normal">!</span>
        </td></tr></table><span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span><sup>n</sup>
    </div>
が行えます。
この式に、<span class="math">
        f<sup>
          <span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </sup><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num">
            n<span class="normal">!</span>
          </td></tr><tr><td>
            <span class="normal">2</span>πi
          </td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
            <span class="normal">|</span>
              z<span class="normal">−</span>ζ
            <span class="normal">|</span><span class="normal">=</span>r
          </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            f<span class="paren" style="font-size:em;">(</span>ζ<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              ζ<span class="normal">−</span>z
            <span class="paren" style="font-size:em;">)</span>
            <sup>
              n<span class="normal">+</span><span class="normal">1</span>
            </sup>
          </td></tr></table><span class="normal">d</span>ζ
      </span>を代入すると、
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          n<span class="normal">=</span><span class="normal">0</span>
        </td></tr></table>a<sub>n</sub><span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span><sup>n</sup>
    </div><div class="math">
      a<sub>n</sub><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        <span class="normal">2</span>πi
      </td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
          <span class="normal">|</span>
            ζ<span class="normal">−</span>z
          <span class="normal">|</span><span class="normal">=</span>r
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
        </td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>
            z<span class="normal">−</span>ζ
          <span class="paren" style="font-size:em;">)</span>
          <sup>
            n<span class="normal">+</span><span class="normal">1</span>
          </sup>
        </td></tr></table><span class="normal">d</span>z
    </div>
となります。

<span class="math">n</span> 階微分
<span class="math">
        f<sup>
          <span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </sup><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
      </span>は、<span class="math">
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
      </span>が<span class="math">
        z<span class="normal">=</span>ζ
      </span>で正則なときにしか定義できませんでしたが、この<span class="math">
        a<sub>n</sub>
      </span>なら<span class="math">
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
      </span>が<span class="math">
        z<span class="normal">=</span>ζ
      </span>で正則でなくても、<span class="math">
        z<span class="normal">=</span>ζ
      </span>の近傍で正則（このような点を<em>孤立特異点</em>といいます）なら定義することができます。
そこで、テーラー展開を拡張して、
<div class="math">
      <em>
        f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
            n<span class="normal">=</span><span class="normal">−</span>∞
          </td></tr></table>a<sub>n</sub><span class="paren" style="font-size:em;">(</span>
          z<span class="normal">−</span>ζ
        <span class="paren" style="font-size:em;">)</span><sup>n</sup>
      </em>
    </div><div class="math">
      <em>
        a<sub>n</sub><span class="normal">=</span><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>πi
        </td></tr></table><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
            <span class="normal">|</span>
              ζ<span class="normal">−</span>z
            <span class="normal">|</span><span class="normal">=</span>r
          </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
            f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              z<span class="normal">−</span>ζ
            <span class="paren" style="font-size:em;">)</span>
            <sup>
              n<span class="normal">+</span><span class="normal">1</span>
            </sup>
          </td></tr></table><span class="normal">d</span>z
      </em>
    </div>
としたものを<strong id="laurent" class="keyword">ローラン展開</strong>（Laurent expansion、ローランは人名）といいます。
ただし、<span class="math">r</span>は<span class="math">f</span>が<span class="math">
        <span class="normal">0</span><span class="normal">&lt;</span><span class="normal">|</span>
          z<span class="normal">−</span>ζ
        <span class="normal">|</span><span class="normal">&lt;</span>R
      </span>で正則となるような実数<span class="math">R</span>に対して、<span class="math">
        <span class="normal">0</span><span class="normal">&lt;</span>r<span class="normal">&lt;</span>R
      </span>となる任意の実数です。

ローラン展開は、<span class="math">f</span>の正則点ではテーラー展開と一致します。また、<span class="math">
        z<span class="normal">=</span>ζ
      </span>が<span class="math">f</span>の<span class="math">N</span>位の極であるとき、<span class="math">
        n≦<span class="normal">−</span>N
      </span>ならば<span class="math">
        a<sub>n</sub><span class="normal">=</span><span class="normal">0</span>
      </span>となります。
（ここではローラン級数の収束性や一意性について厳密な話は取り扱いません。詳しくは教科書などをご覧ください。）


## <a id="sec-generated-title-5"></a> <a id="residue"></a>留数

<span class="math">f</span>が領域<span class="math">
        <span class="normal">0</span><span class="normal">&lt;</span><span class="normal">|</span>
          z<span class="normal">−</span>ζ
        <span class="normal">|</span><span class="normal">&lt;</span>R
      </span>上で正則であるとき、
<span class="math">
        <span class="normal">0</span><span class="normal">&lt;</span>r<span class="normal">&lt;</span>R
      </span>となる任意の実数<span class="math">r</span>に対して、
<div class="math">
      <em>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="normal">2</span>πi
        </td></tr></table>
        <span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">
            <span class="normal">|</span>
              z<span class="normal">−</span>ζ
            <span class="normal">|</span>
            <span class="normal">=</span>r
          </td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z
      </em>
    </div>
の値を<em>
        <span class="math">f</span>の<span class="math">
          z<span class="normal">=</span>ζ
        </span>における<strong id="residue" class="keyword">留数</strong>
      </em>といい、
<em>
        <span class="math">
          <span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>f,ζ<span class="paren" style="font-size:em;">]</span>
        </span>
      </em>と書きます。

留数は、
点 <span class="math">ζ</span> 上を除いて正則な関数の、閉路上での積分ですから、
点 <span class="math">ζ</span> で非正則な場合（すなわち、極である場合）にのみ 0 でない値になります。

また、<span class="math">f</span>の任意の閉路<span class="math">C</span>上での積分の値は、留数を用いて
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>z <span class="normal">=</span><span class="normal">2</span>πi<table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i</td></tr></table><span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>
        f,ζ<sub>i</sub>
      <span class="paren" style="font-size:em;">]</span>
    </div>
と表すことができます。
ただし、
<span class="math">
        ζ<sub>i</sub>
      </span>は閉路<span class="math">C</span>に囲まれた領域内にある<span class="math">f</span>の極です。
この式から、
閉路中の全ての極における留数を調べることで、
任意の閉路上の積分の値を求めることが出来ます。

留数の求め方について考える前に、留数とローラン展開の関係について触れて起きます。
ローラン展開の係数の式と、留数の定義式を比べれば即座に分かりますが、
留数はローラン展開の<span class="math">
        <span class="normal">−</span>
        <span class="normal">1</span>
      </span>次の項の係数に一致します。
（前節で述べた式に <span class="math">
        n <span class="normal">=</span><span class="normal">−</span><span class="normal">1</span>
      </span> を代入すると、分母が消えて留数の定義式に一致します。）
すなわち、<span class="math">f</span>が
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          n<span class="normal">=</span><span class="normal">−</span>∞
        </td></tr></table>a<sub>n</sub><span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span><sup>n</sup>
    </div>
とローラン展開できるとき、
<div class="math">
      <em>
        <span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>f,ζ<span class="paren" style="font-size:em;">]</span>
        <span class="normal">=</span> a<sub>
          <span class="normal">−</span>
          <span class="normal">1</span>
        </sub>
      </em>
    </div>
が成り立ちます。

また、点 <span class="math">ζ</span> が複素関数 <span class="math">f</span> の <span class="math">N</span> 位の極であるとき、
<span class="math">f</span> のローラン係数 <span class="math">
        a<sub>n</sub>
      </span> は <span class="math">
        n <span class="normal">&lt;</span> <span class="normal">−</span>N
      </span> である全ての <span class="math">n</span> に対して 0 になります。
すなわち、<span class="math">f</span> のローラン展開は以下のようになります。
（Σ の範囲に注意。）
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          n<span class="normal">=</span><span class="normal">−</span>N
        </td></tr></table>a<sub>n</sub><span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span><sup>n</sup>
    </div>
ここで、まずこの<span class="math">f</span>に<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          z<span class="normal">−</span>ζ
        <span class="paren" style="font-size:em;">)</span>
        <sup>N</sup>
      </span>をかけてやると、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span>
      <sup>N</sup>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          n<span class="normal">=</span><span class="normal">0</span>
        </td></tr></table>a<sub>
        n<span class="normal">−</span>N
      </sub><span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span><sup>n</sup>
    </div>
となり、さらにこれを<span class="math">
        N<span class="normal">−</span><span class="normal">1</span>
      </span>階微分してやると、
<div class="math">
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>
          </td></tr><tr><td>
            <span class="normal">d</span>z
          </td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span>
      <sup>
        N<span class="normal">−</span><span class="normal">1</span>
      </sup>
      <span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span>
      <sup>N</sup>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">
          n<span class="normal">=</span><span class="normal">0</span>
        </td></tr></table><table class="frac" summary="fraction"><tr><td class="num">
          <span class="paren" style="font-size:em;">(</span>
            n<span class="normal">+</span>N<span class="normal">−</span><span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">!</span>
        </td></tr><tr><td>
          n<span class="normal">!</span>
        </td></tr></table>a<sub>
        n<span class="normal">−</span><span class="normal">1</span>
      </sub><span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span><sup>n</sup>
    </div>
となります。ここで、<span class="math">z → ζ</span> の極限を取ると、<span class="math">
        n<span class="normal">=</span><span class="normal">0</span>
      </span>の項だけが残るので、
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">z→ζ</td></tr></table>
      <span class="paren" style="font-size:1.5em;">(</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            <span class="normal">d</span>
          </td></tr><tr><td>
            <span class="normal">d</span>z
          </td></tr></table>
      <span class="paren" style="font-size:1.5em;">)</span>
      <sup>
        N<span class="normal">−</span><span class="normal">1</span>
      </sup>
      <span class="paren" style="font-size:em;">(</span>
        z<span class="normal">−</span>ζ
      <span class="paren" style="font-size:em;">)</span>
      <sup>N</sup>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="paren" style="font-size:em;">(</span>
        N<span class="normal">−</span><span class="normal">1</span>
      <span class="paren" style="font-size:em;">)</span><span class="normal">!</span>a<sub>
        <span class="normal">−</span>
        <span class="normal">1</span>
      </sub>
    </div>
が得られます。
<span class="math">
        <span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>f,ζ<span class="paren" style="font-size:em;">]</span><span class="normal">=</span> a<sub>
          <span class="normal">−</span>
          <span class="normal">1</span>
        </sub>
      </span>ですから、結局、<span class="math">N</span>位の極<span class="math">ζ</span>での<span class="math">f</span>の留数は
<div class="math">
      <em>
        <span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>f,ζ<span class="paren" style="font-size:em;">]</span>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <span class="paren" style="font-size:em;">(</span>
            N<span class="normal">−</span><span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">!</span>
        </td></tr></table>
        <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">z→ζ</td></tr></table>
        <span class="paren" style="font-size:1.5em;">(</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              <span class="normal">d</span>
            </td></tr><tr><td>
              <span class="normal">d</span>z
            </td></tr></table>
        <span class="paren" style="font-size:1.5em;">)</span>
        <sup>
          N<span class="normal">−</span><span class="normal">1</span>
        </sup>
        <span class="paren" style="font-size:em;">(</span>
          z<span class="normal">−</span>ζ
        <span class="paren" style="font-size:em;">)</span>
        <sup>N</sup>f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
      </em>
    </div>
によって求めることができます。
