---
title: "複素数"
source_url: "https://ufcpp.net/study/math/set/complex-s_complex/"
content_type: "Article"
published_at: "2015-05-06T14:17:08"
updated_at: "2015-05-18T16:58:51"
tags: []
umbraco_id: 1479
parent_id: 1471
sort_order: 7
aliases:
  - "/study/math/set/s_complex"
  - "/study/set/complex.html"
---

# 複素数

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

複素数は実数の2次の代数拡大体。
また、複素数は代数的閉体になっている。


## <a id="sec-generated-title-2"></a> <a id="complex"></a>複素数の定義

実数係数の代数方程式の根は必ずしも実数とはなりません。
簡単な例を挙げると、
<span class="math">
        x<sup><span class="normal">2</span></sup> <span class="normal">+</span> <span class="normal">1</span> <span class="normal">=</span> 0
      </span> の根は実数の範囲にはありません。
そこで、
実数に <span class="math">
        x<sup><span class="normal">2</span></sup> <span class="normal">+</span> <span class="normal">1</span> <span class="normal">=</span> 0
      </span> の根を付け加えたような集合を作ろうというのが複素数の発想です。

要するに、実数 <span class="math">
        <span class="bold">R</span>
      </span> に、
<span class="math">
        i<sup><span class="normal">2</span></sup> <span class="normal">=</span> <span class="normal">−</span><span class="normal">1</span>
      </span> を満たすような特別な元 <span class="math">i</span> を付け加えた集合
<div class="math">
      <span class="bold">C</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        <span class="bold">R</span>, i
      <span class="paren" style="font-size:em;">}</span>
    </div>
を作ります。
このような手順で新しい集合を作ることを
「[代数拡大](../group/extensionfield.md#algebraic)」
といい、
作られた集合は「[体](../group/field.md#field)」
になります（加減乗除が定義できる）。
（詳しくは、群・環・体で説明します。）
この体 <span class="math">
        <span class="bold">C</span>
      </span> を<strong id="complex" class="keyword">複素数</strong>（complex number）と呼びます。

ここで新しく定義した元 <span class="math">i</span> を虚数単位といいます。
工学系の分野では、習慣的に <span class="math">i</span> は電流を表すための文字なので、
これと区別するために虚数単位を <span class="math">j</span> で表すこともあります。

<span class="math">
        <span class="bold">C</span>
      </span> の任意の元 <span class="math">α</span> は、
2つの実数 <span class="math">x, y</span> と虚数単位 <span class="math">i</span> を用いて
<div class="math">
      α <span class="normal">=</span> x <span class="normal">+</span> iy
    </div>
と表すことができます。
すなわち、<span class="math">
        <span class="normal">1</span>, i</span> を基底ベクトルとする2次元ベクトル空間になっています。
（これも代数拡大体の性質の1つです。詳しくは群・環・体で。）
このため、
複素数は単純に大小比較することは出来ません。
（ベクトルは大小比較が出来ない。）

ちなみに、上式において、
（<span class="math">i</span> の付かない） <span class="math">x</span> の部分を実部（real part）、
（<span class="math">i</span> の付いている） <span class="math">y</span> の部分を虚部（imaginary part）と呼びます。

虚部が 0、すなわち、<span class="math">y <span class="normal">=</span> <span class="normal">0</span>
      </span> となるような複素数は、実数と1対1に対応するので、
<em>実数は複素数の部分集合である</em>とみなすことができます。

また、複素数の中で、実数ではないもの、すなわち、虚部を持つ（<span class="math">y ≠ <span class="normal">0</span>
    </span>）ものを虚数（imaginary number）と呼びます。
特に、実部を持たず（<span class="math">x <span class="normal">=</span> <span class="normal">0</span>
      </span>）、虚部のみを持つ虚数を、
純虚数（pure imaginary number, purely imaginary number）と呼びます。


## <a id="sec-generated-title-3"></a> <a id="operation"></a>複素数の間の関係・演算

### <a id="sec-generated-title-4"></a> <a id="sum"></a>複素数の加減算・乗算

2つの複素数 <span class="math">α <span class="normal">=</span> x <span class="normal">+</span> iy, β <span class="normal">=</span> w <span class="normal">+</span> iz</span> の和・差は
<div class="math">
        α <span class="normal">±</span> β <span class="normal">=</span> x <span class="normal">±</span> w <span class="normal">+</span> i<span class="paren" style="font-size:em;">(</span>y <span class="normal">±</span> z<span class="paren" style="font-size:em;">)</span>
      </div>
となります。

また、<span class="math">α, β</span> の積は、
実数の積および <span class="math">
          i<sup><span class="normal">2</span></sup> <span class="normal">=</span> <span class="normal">−</span> <span class="normal">1</span>
        </span> という性質を使うと、
<div class="math">
        α <span class="normal">×</span> β <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>x <span class="normal">+</span> iy<span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span>w <span class="normal">+</span> iz<span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        xw <span class="normal">+</span> iyw <span class="normal">+</span> ixz <span class="normal">+</span> i<sup><span class="normal">2</span></sup>yz
        <span class="normal">=</span>
        xw <span class="normal">−</span> yz <span class="normal">+</span> i<span class="paren" style="font-size:em;">(</span>yw <span class="normal">+</span> xz<span class="paren" style="font-size:em;">)</span>
      </div>
となります。


### <a id="sec-generated-title-5"></a> <a id="conjugate"></a>複素数の絶対値、共役

複素数 <span class="math">α <span class="normal">=</span> x <span class="normal">+</span> iy</span> に対して、
実数 <span class="math">
          <span class="normal" style="font-size:em;">√</span><span class="bar">
            x<sup><span class="normal">2</span></sup> <span class="normal">+</span> y<sup><span class="normal">2</span></sup>
          </span>
        </span> を
<span class="math">α</span> の絶対値と呼び、
<span class="math">
          <span class="normal">|</span>α<span class="normal">|</span>
        </span> で表します。

また、複素数 <span class="math">α <span class="normal">=</span> x <span class="normal">+</span> iy</span> に対して、
<span class="math">x <span class="normal">−</span> iy</span> で表される複素数を、
<strong id="conjugate" class="keyword">共役</strong>な複素数と呼び、<span class="math">
          α<sup>*</sup>
        </span> で表します。

複素数 <span class="math">α</span> とその共役複素数を掛け合わせると、
<div class="math">
        αα<sup>*</sup>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>x <span class="normal">+</span> iy<span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span>x <span class="normal">−</span> iy<span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        x<sup><span class="normal">2</span></sup> <span class="normal">−</span> ixy <span class="normal">+</span> ixy <span class="normal">−</span> i<sup><span class="normal">2</span></sup>y<sup><span class="normal">2</span></sup>
        <span class="normal">=</span>
        x<sup><span class="normal">2</span></sup> <span class="normal">+</span> i<sup><span class="normal">2</span></sup>y<sup><span class="normal">2</span></sup>
      </div>
というように、絶対値の2乗になります。

したがって、
<div class="math">
        α<span class="normal">×</span><table class="frac" summary="fraction"><tr><td class="num">
            α<sup>*</sup>
          </td></tr><tr><td>
            <span class="normal">|</span>α<span class="normal">|</span>
            <sup><span class="normal">2</span></sup>
          </td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            α<sup>*</sup>
          </td></tr><tr><td>
            <span class="normal">|</span>α<span class="normal">|</span>
            <sup><span class="normal">2</span></sup>
          </td></tr></table><span class="normal">×</span>α
        <span class="normal">=</span>
        <span class="normal">1</span>
      </div>
となるので、<span class="math">α</span> が非 0 のとき、
必ず逆数が存在し、<span class="math">
          <table class="frac" summary="fraction"><tr><td class="num">
              α<sup>*</sup>
            </td></tr><tr><td>
              <span class="normal">|</span>α<span class="normal">|</span>
              <sup><span class="normal">2</span></sup>
            </td></tr></table>
        </span> と表せます。
したがって、複素数は体になります。


## <a id="sec-generated-title-6"></a> <a id="algebra"></a>代数系としての複素数

ここまでで説明してきたように、複素数は体になります。
体であることを明示的に表すために、複素数を<em>複素数体</em>と呼ぶこともあります。

複素数は、実数や有理数を部分体として含む体となります。
最初にも述べていますが、
実数→複素数のように、解を持たない代数方程式の根を付け加えることで体を拡大する方法を「[代数拡大](../group/extensionfield.md#algebraic)」と呼びます。
また、複素数は実数上の2次元ベクトル空間にもなっているわけですが、
このことを<em>複素数は実数の2次の代数拡大体である</em>といいます。


## <a id="sec-generated-title-7"></a> <a id="misc"></a>余談

複素数以外にも代数拡大によって作れる体はいくらでもあります。
例えば、有理数体 <span class="math">
        <span class="bold">Q</span>
      </span> に <span class="math">
        <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      </span>を加えた集合
<span class="math">
        <span class="paren" style="font-size:em;">{</span>
          <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">
            <span class="normal">3</span>
          </span>
        <span class="paren" style="font-size:em;">}</span>
      </span>
を作ると体になります。

<span class="math">
        p, q, r, s <span class="normal">∈</span> <span class="bold">Q</span>
      </span> として、
<span class="math">
        α <span class="normal">=</span> p + <span class="paren" style="font-size:em;">(</span>
          <span class="normal" style="font-size:em;">√</span><span class="bar">
            <span class="normal">3</span>
          </span>
        <span class="paren" style="font-size:em;">)</span> q, β <span class="normal">=</span> r + <span class="paren" style="font-size:em;">(</span>
          <span class="normal" style="font-size:em;">√</span><span class="bar">
            <span class="normal">3</span>
          </span>
        <span class="paren" style="font-size:em;">)</span> s
      </span>
とすると、
<div class="math">
      α, β <span class="normal">∈</span> <span class="paren" style="font-size:em;">{</span>
        <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      α<span class="normal">+</span>β <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>p<span class="normal">+</span>r<span class="paren" style="font-size:em;">)</span>    <span class="normal">+</span> <span class="paren" style="font-size:em;">(</span>
        <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>q<span class="normal">+</span>s<span class="paren" style="font-size:em;">)</span>
      <span class="normal">∈</span> <span class="paren" style="font-size:em;">{</span>
        <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      α<span class="normal">×</span>β <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>pr<span class="normal">+</span><span class="normal">3</span>qs<span class="paren" style="font-size:em;">)</span> <span class="normal">+</span> <span class="paren" style="font-size:em;">(</span>
        <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>ps<span class="normal">+</span>rq<span class="paren" style="font-size:em;">)</span>
      <span class="normal">∈</span> <span class="paren" style="font-size:em;">{</span>
        <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      <span class="normal">−</span>α   <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span><span class="normal">−</span>p<span class="paren" style="font-size:em;">)</span>     <span class="normal">+</span> <span class="paren" style="font-size:em;">(</span>
        <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
        <span class="normal">−</span>q
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">∈</span> <span class="paren" style="font-size:em;">{</span>
        <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      α<sup>
        <span class="normal">−</span>
        <span class="normal">1</span>
      </sup> <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>p/d<span class="paren" style="font-size:em;">)</span>     <span class="normal">+</span> <span class="paren" style="font-size:em;">(</span>
        <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span>q/d<span class="paren" style="font-size:em;">)</span>
      <span class="normal">∈</span> <span class="paren" style="font-size:em;">{</span>
        <span class="bold">Q</span>, <span class="normal" style="font-size:em;">√</span><span class="bar">
          <span class="normal">3</span>
        </span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
（ただし、<span class="math">
        d <span class="normal">=</span> p<sup><span class="normal">2</span></sup> <span class="normal">−</span> <span class="normal">3</span> q<sup><span class="normal">2</span></sup> <span class="normal">∈</span> <span class="bold">Q</span>
      </span>）
となり、
四則演算が定義できることが分かると思います。


## <a id="sec-generated-title-8"></a> <a id="plan"></a>執筆予定

```text
      複素数係数の代数方程式は必ず複素数根を持つ。
      このような性質を「代数的に閉じている」という。
      代数的に閉じた体を「代数的閉体」と呼ぶ。
      （複素数体は代数的閉体）

      ↑
      代数学の基本定理。
      代数的に証明しようとすると結構面倒。
      複素関数論とかを使って解析的に証明すると（複素解析が分かっていれば）割と簡単。
    
```
