---
title: "環・体"
source_url: "https://ufcpp.net/study/math/group/field/"
content_type: "Article"
published_at: "2015-05-06T14:17:21"
updated_at: "2020-02-20T19:46:00"
tags: []
umbraco_id: 1486
parent_id: 1483
sort_order: 2
aliases:
  - "/group/field"
  - "/group/field.html"
  - "/math/group/field/"
  - "/study/group/field"
  - "/study/group/field.html"
---

# 環・体

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[群とは](group.md#group)」では算法を1つ持つ代数系の分類について説明しました。
ここでは、加法と乗法の2つを持つ代数系の分類について説明します。
このような代数系の分類として、環・体などがあります。


##<a id="sec-generated-title-2"></a> <a id="field"></a>環・体とは
ある代数系<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          A,<span class="paren" style="font-size:em;">{</span>＋, ×<span class="paren" style="font-size:em;">}</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>に対して、以下の条件を考えます。
（<span class="math">＋</span> を加法、<span class="math">×</span> を乗法と呼びます。）

1. 加法に関して「[アーベル群](group.md#abelian)」をなす。

2. 加法と乗法の間に「[分配法則](algebraic.md#distributive)」が成り立つ。

3. 乗法に関して「[半群](group.md#semigroup)」をなす。

4. 加法に関する「[単位元](algebraic.md#unity)」（零元）を除いて、乗法に関して「[群](group.md#group)」をなす。


代数系<span class="math">A</span>が
1. 2. 3. を満たすとき、<strong id="ring" class="keyword">環</strong>(ring)とよび、
1. 2. 4. を満たすとき、<strong id="field" class="keyword">体</strong>(field)と呼びます。
また、これらは、乗法に関して可換であるとき、可換環・可換体と呼びます。
（ただし、可換なもののみを体と呼び、非可換なものは斜体と呼ぶ流儀もあります。）
慣例的に、環は <span class="math">R</span> で、体は <span class="math">K</span> で表すことが多いです。
（体の K はドイツ語の Körper の頭文字から取ったものらしい。）

加法に関する単位元を<strong id="zero" class="keyword">零元</strong>（zero element）と呼び、<span class="math">0</span> で表します。
また、環・体は、加法もしくは乗法のどちらか一方のみに注目する場合、以下のような書き方をします。
<div class="math">
      A<sup>＋</sup> ＝ <span class="paren" style="font-size:em;">{</span>A, ＋<span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      A<sup>×</sup> ＝ <span class="paren" style="font-size:em;">{</span>A, ×<span class="paren" style="font-size:em;">}</span>
    </div>
さらに、<span class="math">
        A<sup>×</sup>
      </span> から零元を除いたものを <span class="math">
        A<sup>＊</sup>
      </span> と表します。
<div class="math">
      A<sup>＊</sup> ＝ <span class="paren" style="font-size:em;">{</span>
        A － <span class="paren" style="font-size:em;">{</span>0<span class="paren" style="font-size:em;">}</span>, ×
      <span class="paren" style="font-size:em;">}</span>
    </div>
代数系 <span class="math">A</span> が体をなすとき、<span class="math">
        A<sup>＊</sup>
      </span> は群をなします。

環 <span class="math">R</span> の非0の元 <span class="math">x</span> に対して、ある非0元 <span class="math">y</span> があって、
<span class="math">xy ＝ 0</span> を満たすとき、<span class="math">x</span> を<strong id="d54e160" class="keyword">零因子</strong>（zero divisor）と呼びます。
環 <span class="math">R</span> が零因子を持たないとき、<span class="math">R</span> を<strong id="integral" class="keyword">整域</strong>（integral domain）と呼びます。


##<a id="sec-generated-title-3"></a> <a id="easy_sample"></a>環・体の簡単な例
よく知られている集合のうちで、環・体になっているものをいくつか例に挙げて紹介します。
<h4>整数・有理数・実数・複素数</h4>
整数 <span class="math">
        <span class="bold">Z</span>
      </span> は可換環になります。
整数は乗法に関する逆元が存在しない（<span class="math">1 / 2</span> は整数ではない）ので、
体になりません。

有理数 <span class="math">
        <span class="bold">Q</span>
      </span>、
実数 <span class="math">
        <span class="bold">R</span>
      </span>、
複素数 <span class="math">
        <span class="bold">C</span>
      </span>
はいずれも可換体になります。

自然数は加法に関しても逆元が存在しないので、環にもなりません。
<h4>正方行列</h4>
<span class="math">n</span> 次元正方行列 <span class="math">
        M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
      </span> は非可換環になります。

<span class="math">
        a, b, c ∈ M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
      </span> とすると、

* <span class="math">
          a ＋ b ∈ M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </span>

* <span class="math">
          M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </span>の零元は零行列<span class="math">O</span>

* <span class="math">a</span>の加法に関する逆元は<span class="math">
          －a ∈ M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </span>

* <span class="math">
          a × b ∈ M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </span>

* <span class="math">
          M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </span>の単位元は単位行列<span class="math">I</span>

* <span class="math">a × b ≠ b × a</span>

* <span class="math">a</span>の乗法に関する逆元は必ずしも存在しない。 （<span class="math">a</span>が正則な場合に限り、逆元<span class="math">
          a<sup>－1</sup>
        </span>が存在。）

* <span class="math">
          c ×<span class="paren" style="font-size:em;">(</span>a ＋ b<span class="paren" style="font-size:em;">)</span>
          ＝
          c a ＋ c b
          ∈ M<span class="paren" style="font-size:em;">(</span>n<span class="paren" style="font-size:em;">)</span>
        </span>

<h4>線形写像（積として関数の合成を使う）</h4>
（体 → 体 の任意の線形写像でも構わないんですが、話を簡単にするため、）
実数 → 実数 の線形写像 <span class="math">
        F<span class="paren" style="font-size:em;">(</span>
          <span class="bold">R</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> を考えます。
<span class="math">
        f, g ∈ F<span class="paren" style="font-size:em;">(</span>
          <span class="bold">R</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> の加法・乗法を、
<div class="math">
      f + g ＝ f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＋ g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      f × g ＝ f<span class="paren" style="font-size:em;">(</span>
        g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">)</span>　　<span class="normal">（写像の合成）</span>
    </div>
で定めると、これは非可換環になります。
写像の合成は結合法則を満たしますし、
線形写像の場合、
<span class="math">
        f × <span class="paren" style="font-size:em;">(</span>
          g<sub>1</sub> ＋ g<sub>2</sub>
        <span class="paren" style="font-size:em;">)</span>
        ＝
        f<span class="paren" style="font-size:em;">(</span>
          g<sub>1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＋ g<sub>2</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
        ＝
        f<span class="paren" style="font-size:em;">(</span>
          g<sub>1</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
        ＋
        f<span class="paren" style="font-size:em;">(</span>
          g<sub>2</sub><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
        ＝
        f × g<sub>1</sub> ＋ f × g<sub>2</sub>
      </span>
が成り立つので、分配法則も成り立ちます。
また、
零元は <span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ 0
      </span>、
単位元は恒等写像 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ x
      </span> です。
<h4>超関数（積として畳込み積を使う）</h4>
「[超関数](../distribution/distribution-e_distribution.md#distribution)」
<span class="math">f, g</span> に対して加法および乗法を、
<div class="math">
      f + g ＝ f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＋ g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      f × g ＝ f * g
      ＝
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table>f<span class="paren" style="font-size:em;">(</span>ξ<span class="paren" style="font-size:em;">)</span> g<span class="paren" style="font-size:em;">(</span>x － ξ<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>ξ
      　　<span class="normal">（畳込み積）</span>
    </div>
で定義すると、可換体になります。
零元は <span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ 0
      </span>、
単位元はδ関数です。
<h4>正の実数（和として×、積として冪を使う）</h4>
正の実数 <span class="math">
        P ＝ <span class="paren" style="font-size:em;">{</span>
          x | x ∈ <span class="bold">R</span> ∧ x ＞ 0
        <span class="paren" style="font-size:em;">}</span>
      </span> に対して、
加法 <span class="math">
        ＋<sub>P</sub>
      </span> および乗法 <span class="math">
        ×<sub>P</sub>
      </span> を
<div class="math">
      a ＋<sub>P</sub> b ＝ a × b
    </div><div class="math">
      a ×<sub>P</sub> b ＝ a<sup>
        <span class="normal">log</span> b
      </sup>
    </div>
で定義すると可換体となります。

ちなみに、この体
<span class="math">
        <span class="paren" style="font-size:em;">{</span>
          P, <span class="paren" style="font-size:em;">{</span>
            ＋<sub>P</sub>, ×<sub>P</sub>
          <span class="paren" style="font-size:em;">}</span>
        <span class="paren" style="font-size:em;">}</span>
      </span>
は写像 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ <span class="normal">log</span> x
      </span> によって、
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>P<span class="paren" style="font-size:em;">)</span> ＝ <span class="bold">R</span>
    </div><div class="math">
      f<span class="paren" style="font-size:em;">(</span>
        a ＋<sub>P</sub> b
      <span class="paren" style="font-size:em;">)</span>
      ＝
      <span class="normal">log</span><span class="paren" style="font-size:em;">(</span>a × b<span class="paren" style="font-size:em;">)</span>
      ＝
      <span class="normal">log</span><span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
      ＋
      <span class="normal">log</span><span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span>
      ＝
      f<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
      ＋
      f<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      f<span class="paren" style="font-size:em;">(</span>
        a ×<sub>P</sub> b
      <span class="paren" style="font-size:em;">)</span>
      ＝
      <span class="normal">log</span><span class="paren" style="font-size:em;">(</span>
        a<sup>
          <span class="normal">log</span> b
        </sup>
      <span class="paren" style="font-size:em;">)</span>
      ＝
      <span class="normal">log</span> a × <span class="normal">log</span> b
      ＝
      f<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span>
      ×
      f<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span>
    </div>
となるので、実数体
<span class="math">
        <span class="paren" style="font-size:em;">{</span>
          <span class="bold">R</span>, <span class="paren" style="font-size:em;">{</span>＋, ×<span class="paren" style="font-size:em;">}</span>
        <span class="paren" style="font-size:em;">}</span>
      </span>
と同型になります。


##<a id="sec-generated-title-4"></a> <a id="filed_sample"></a>その他の体の例
有理数や実数など以外にも、様々な体が存在します。
このような体の例をいくつか紹介します。


###<a id="sec-generated-title-5"></a> <a id="quaternion"></a>ハミルトンの四元数体
複素数は、<span class="math">
          i<sup>2</sup> ＝ －1
        </span> となる数 <span class="math">i</span> と、2つの実数 <span class="math">a, b</span> を使って <span class="math">a ＋ i b</span> となるような数を作ったものです。
これと同様に、<span class="math">
          j<sup>2</sup> ＝ －1
        </span> となる数 <span class="math">j</span> と、2つの複素数 <span class="math">α, β</span> を使って <span class="math">
          α ＋ jβ<sup>\*</sup>
        </span> となるような数を作ることができます。
（<span class="math">
          x<sup>\*</sup>
        </span> は <span class="math">x</span> の共役複素数。）
このような数はハミルトンの四元数（Hamiltonian quaternion、ハミルトンは人名）、あるいは単に<strong id="quaternion" class="keyword">四元数</strong>（quaternion）と呼ばれています。
<em>四元数は非可換体</em>になります。

四元数は、2つの虚数単位 <span class="math">i, j</span> および4つの実数 <span class="math">a, b, c, d</span> を使って、
<span class="math">a ＋ i b ＋ j c ＋ ij d</span> とも書き表すことができます。
あるいは、<span class="math">k ＝ ij</span> と置いて、
<span class="math">a ＋ i b ＋ j c ＋ k d</span> と書き表します。
<span class="math">i, j, k</span> の間には以下のような関係式が成り立っています。
<div class="math">
        i<sup>2</sup> ＝ j<sup>2</sup> ＝ k<sup>2</sup> ＝ －1
      </div><div class="math">
        ij ＝ k, ki ＝ j, jk = i
      </div><div class="math">
        ji ＝ －k, ik ＝ －j, kj = －i
      </div>
四元数という言葉は、4つの実数（4元）から作られた数という意味です。


####<a id="sec-generated-title-6"></a> <a id="d54e748"></a>ベクトルを使った表現
四元数 <span class="math">a ＋ i b ＋ j c ＋ k d</span> は、
1つのスカラー <span class="math">x ＝ a</span> と
1つのベクトル <span class="math">
            <span class="vector">u</span> ＝ <span class="paren" style="font-size:em;">(</span>b, c, d<span class="paren" style="font-size:em;">)</span>
          </span> を使って、
<span class="math">
            <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> と書き表すことがあります。
<span class="math">x</span> の部分を<em>スカラー部</em>、
<span class="math">
            <span class="vector">u</span>
          </span> の部分を<em>ベクトル部</em>と呼びます。
ベクトル部が <span class="math">
            <span class="vector">0</span>
          </span> のとき、
四元数 <span class="math">
            <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">0</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> を実数と同一視し、
単に <span class="math">x</span> で書き表します。
このような形式を用いることで、以下に述べるように、
加減乗除などの計算が簡単に書き表すことができます。

まず、四元数の加減算は非常に単純で、以下のようになります。
<em>
          <div class="math">
            <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span> ± <span class="paren" style="font-size:em;">(</span>
              y, <span class="vector">v</span>
            <span class="paren" style="font-size:em;">)</span>
            ＝
            <span class="paren" style="font-size:em;">(</span>
              x ± y, <span class="vector">u</span> ± <span class="vector">v</span>
            <span class="paren" style="font-size:em;">)</span>
          </div>
        </em>
次に、乗算ですが、
<div class="math">
          <span class="paren" style="font-size:em;">(</span>a + i b + j c + k d<span class="paren" style="font-size:em;">)</span> × <span class="paren" style="font-size:em;">(</span>e + i f + j g + k h<span class="paren" style="font-size:em;">)</span>
        </div><div class="math">
          ＝
          <span class="paren" style="font-size:em;">(</span>ae － bf － cg － dh<span class="paren" style="font-size:em;">)</span>
          ＋
          i<span class="paren" style="font-size:em;">(</span>af ＋ be ＋ ch － dg<span class="paren" style="font-size:em;">)</span>
        </div><div class="math">
          ＋
          j<span class="paren" style="font-size:em;">(</span>ag ＋ ce ＋ df － bh<span class="paren" style="font-size:em;">)</span>
          ＋
          k<span class="paren" style="font-size:em;">(</span>ah ＋ de ＋ bg － cf<span class="paren" style="font-size:em;">)</span>
        </div>
なので、ベクトル表現では以下のようになります。
<em>
          <div class="math">
            <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span> × <span class="paren" style="font-size:em;">(</span>
              y, <span class="vector">v</span>
            <span class="paren" style="font-size:em;">)</span>
            ＝
            <span class="paren" style="font-size:em;">(</span>
              xy － <span class="vector">u</span> ・ <span class="vector">v</span>,
              x <span class="vector">v</span> ＋ y <span class="vector">u</span> ＋ <span class="vector">u</span> × <span class="vector">v</span>
            <span class="paren" style="font-size:em;">)</span>
          </div>
        </em>
ただし、ベクトル間にある <span class="math">・</span> はベクトルの内積、
<span class="math">×</span> は外積を表します。
<span class="math">
            <span class="vector">u</span> × <span class="vector">v</span>
          </span> の部分が非可換なので、
四元数の積は非可換になります。


####<a id="sec-generated-title-7"></a> <a id="d54e934"></a>絶対値・共役
四元数 <span class="math">
            α ＝ <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> に対して、
実数 <span class="math">
            <span class="normal" style="font-size:em;">√</span><span class="bar">
              x<sup>2</sup> ＋ <span class="normal">|</span>
                <span class="vector">u</span>
              <span class="normal">|</span><sup>2</sup>
            </span>
          </span> を
<span class="math">α</span> の絶対値と呼び、
<span class="math">
            <span class="normal">|</span>α<span class="normal">|</span>
          </span> で表します。

また、四元数 <span class="math">
            α ＝ <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> に対して、
<span class="math">
            <span class="paren" style="font-size:em;">(</span>
              x, －<span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> で表される四元数を、
<em>共役</em>な四元数と呼び、<span class="math">
            α<sup>\*</sup>
          </span> で表します。

四元数 <span class="math">α</span> とその共役四元数を掛け合わせると、
<div class="math">
          αα<sup>\*</sup>
          ＝
          <span class="paren" style="font-size:em;">(</span>
            x, <span class="vector">u</span>
          <span class="paren" style="font-size:em;">)</span> × <span class="paren" style="font-size:em;">(</span>
            x, －<span class="vector">u</span>
          <span class="paren" style="font-size:em;">)</span>
          ＝
          <span class="paren" style="font-size:em;">(</span>
            x<sup>2</sup> － <span class="vector">u</span> ・ <span class="paren" style="font-size:em;">(</span>
              －<span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>,
            x <span class="vector">u</span> － x <span class="vector">u</span> ＋ <span class="vector">u</span> × <span class="paren" style="font-size:em;">(</span>
              －<span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span>
        </div><div class="math">
          ＝
          <span class="paren" style="font-size:em;">(</span>
            x<sup>2</sup> ＋ <span class="normal">|</span>
              <span class="vector">u</span>
            <span class="normal">|</span><sup>2</sup>, <span class="vector">0</span>
          <span class="paren" style="font-size:em;">)</span>
          ＝
          x<sup>2</sup> ＋ <span class="normal">|</span>
            <span class="vector">u</span>
          <span class="normal">|</span><sup>2</sup>
        </div>
というように、絶対値の2乗になります。

このことから、
<div class="math">
          α×<table class="frac" summary="fraction"><tr><td class="num">
              α<sup>\*</sup>
            </td></tr><tr><td>
              <span class="normal">|</span>α<span class="normal">|</span>
              <sup>2</sup>
            </td></tr></table>
          ＝
          <table class="frac" summary="fraction"><tr><td class="num">
              α<sup>\*</sup>
            </td></tr><tr><td>
              <span class="normal">|</span>α<span class="normal">|</span>
              <sup>2</sup>
            </td></tr></table>×α
          ＝
          1
        </div>
となるので、<span class="math">α</span> が非 0 のとき、
必ず逆数が存在し、<span class="math">
            <table class="frac" summary="fraction"><tr><td class="num">
                α<sup>\*</sup>
              </td></tr><tr><td>
                <span class="normal">|</span>α<span class="normal">|</span>
                <sup>2</sup>
              </td></tr></table>
          </span> と表せます。
したがって、四元数は非可換体になります。


####<a id="sec-generated-title-8"></a> <a id="d54e1164"></a>3次元空間上の回転
複素数を使って2次元空間上の（原点中心の）回転を表すことができました。
すなわち、複素数 <span class="math">a ＋ ib</span> を2次元ベクトル <span class="math">a, b</span> とみなし、
<span class="math">
            <span class="normal">cos</span>θ ＋ i<span class="normal">sin</span>θ
          </span> を掛けることで角度 <span class="math">θ</span> の回転計算を行うことができます。
これと同様に、四元数を使うと、3次元空間上の（原点を含む軸中心の）回転を表すことができます。

まず、3次元空間上の回転というものがどういう式で表されるかについて説明します。
3次元空間上の回転を表すためには、回転軸ベクトル <span class="math">
            <span class="vector">p</span>
          </span> と回転角度 <span class="math">θ</span>が必要になります。
回転軸ベクトルの絶対値は意味を持たないので、<span class="math">
            <span class="normal">|</span>
              <span class="vector">p</span>
            <span class="normal">|</span> ＝ 1
          </span> であるものとしてます。

座標ベクトル <span class="math">
            <span class="vector">u</span>
          </span> で表される点 A を、回転軸 <span class="math">
            <span class="vector">p</span>
          </span> を中心に角度 <span class="math">θ</span> 回転した点 A' の座標ベクトル <span class="math">
            <span class="vector">u</span>'
          </span> は、以下のような計算で求めることができます。
<em>
          <div class="math">
            <span class="vector">u</span>'
            ＝
            <span class="normal">sin</span>θ <span class="vector">u</span>×<span class="vector">p</span>
            ＋
            <span class="normal">cos</span>θ <span class="paren" style="font-size:em;">(</span>
              <span class="vector">u</span> － <span class="paren" style="font-size:em;">(</span>
                <span class="vector">u</span>・<span class="vector">p</span>
              <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
            <span class="paren" style="font-size:em;">)</span>
            ＋
            <span class="paren" style="font-size:em;">(</span>
              <span class="vector">u</span>・<span class="vector">p</span>
            <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
          </div>
        </em>
ちなみに、この式の導出の仕方ですが、下図のようになります。

<figure>
	[![3次元ベクトルの回転](../../../../assets/media/ufcpp2000/math/rotation3d.png)](../../../../assets/media/ufcpp2000/math/rotation3d.png)
	<figcaption>3次元ベクトルの回転</figcaption>
</figure>


原点を O、点 A から回転軸におろした垂線の足を H とすると、
<div class="math">
          <Vec>OH</Vec> ＝ <span class="paren" style="font-size:em;">(</span>
            <span class="vector">u</span>・<span class="vector">p</span>
          <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
        </div><div class="math">
          <Vec>HA</Vec> ＝ <span class="vector">u</span> － <span class="paren" style="font-size:em;">(</span>
            <span class="vector">u</span>・<span class="vector">p</span>
          <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
        </div>
となります。
また <span class="math">
            <span class="vector">u</span>×<span class="vector">p</span>
          </span> は、
<span class="math">
            <span class="vector">p</span>
          </span> および <span class="math">
            <Vec>HA</Vec>
          </span> に垂直で、
絶対値が <span class="math">
            <span class="normal">|</span>
              <Vec>HA</Vec>
            <span class="normal">|</span>
          </span> と等しいベクトルになります。
<span class="math">
            <Vec>HA'</Vec>
          </span> は、
<div class="math">
          <Vec>HA'</Vec> ＝ <span class="normal">cos</span>θ <Vec>HA</Vec> ＋ <span class="normal">sin</span>θ <span class="vector">u</span>×<span class="vector">p</span>
        </div>
と表すことができるので、先ほど示した式を導き出すことができます。


####<a id="sec-generated-title-9"></a> <a id="d54e1394"></a>四元数を使った回転
以上のことを踏まえた上で、本題の四元数を使った回転の話に入ります。
まず、絶対値が 1 になるような四元数を用意します。
絶対値が 1 の四元数 <span class="math">Σ</span> は以下のように、
絶対値 1 の3次元ベクトル <span class="math">
            <span class="vector">p</span>
          </span> と角度 <span class="math">θ</span> を用いて表すことができます。
<div class="math">
          Σ ＝
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>,
            <span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table><span class="vector">p</span>
          <span class="paren" style="font-size:em;">)</span>
        </div>
そして、この四元数を以下のようにして他の四元数 <span class="math">
            α ＝ <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> に掛けます。
<div class="math">
          Σ<sup>\*</sup>αΣ
          ＝
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>,
            －<span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table><span class="vector">p</span>
          <span class="paren" style="font-size:em;">)</span>
          ×α×
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>,
            <span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table><span class="vector">p</span>
          <span class="paren" style="font-size:em;">)</span>
        </div>
このままだと少し計算が面倒なので、いったん <span class="math">
            Σ ＝ <span class="paren" style="font-size:em;">(</span>
              y, <span class="vector">v</span>
            <span class="paren" style="font-size:em;">)</span>
          </span> と置いて計算します。
<div class="math">
          Σ<sup>\*</sup>αΣ
          ＝
          <span class="paren" style="font-size:em;">(</span>
            y, －<span class="vector">v</span>
          <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
            x, <span class="vector">u</span>
          <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
            y, <span class="vector">v</span>
          <span class="paren" style="font-size:em;">)</span>
          ＝
          <span class="paren" style="font-size:em;">(</span>
            xy ＋ <span class="vector">u</span>・<span class="vector">v</span>,
            y<span class="vector">u</span> － x<span class="vector">v</span> － <span class="vector">v</span>×<span class="vector">u</span>
          <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
            y, <span class="vector">v</span>
          <span class="paren" style="font-size:em;">)</span>
        </div><div class="math">
          ＝
          <span class="paren" style="font-size:em;">(</span>
            x <span class="paren" style="font-size:em;">(</span>
              y<sup>2</sup> ＋ <span class="normal">|</span>
                <span class="vector">v</span>
              <span class="normal">|</span><sup>2</sup>
            <span class="paren" style="font-size:em;">)</span>,
            2y <span class="vector">u</span>×<span class="vector">v</span>
            ＋ <span class="paren" style="font-size:em;">(</span>
              y<sup>2</sup> － <span class="normal">|</span>
                <span class="vector">v</span>
              <span class="normal">|</span><sup>2</sup>
            <span class="paren" style="font-size:em;">)</span><span class="vector">u</span>
            ＋ 2 <span class="paren" style="font-size:em;">(</span>
              <span class="vector">u</span>・<span class="vector">v</span>
            <span class="paren" style="font-size:em;">)</span><span class="vector">v</span>
          <span class="paren" style="font-size:em;">)</span>
        </div>
この式に、
<div class="math">
          <span class="paren" style="font-size:em;">(</span>
            y<sup>2</sup> ＋ <span class="normal">|</span>
              <span class="vector">v</span>
            <span class="normal">|</span><sup>2</sup>
          <span class="paren" style="font-size:em;">)</span>
          ＝ <span class="normal">sin</span><sup>2</sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>
          ＋ <span class="normal">cos</span><sup>2</sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table> ＝ 1
        </div><div class="math">
          <span class="paren" style="font-size:em;">(</span>
            y<sup>2</sup> － <span class="normal">|</span>
              <span class="vector">v</span>
            <span class="normal">|</span><sup>2</sup>
          <span class="paren" style="font-size:em;">)</span>
          ＝ <span class="normal">cos</span><sup>2</sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>
          － <span class="normal">sin</span><sup>2</sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>
          ＝ <span class="normal">cos</span>θ
        </div><div class="math">
          2 y <span class="vector">v</span>
          ＝
          2 <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table><span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table><span class="vector">p</span>
          ＝
          <span class="normal">sin</span>θ
          <span class="vector">p</span>
        </div><div class="math">
          2 <span class="normal">sin</span><sup>2</sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>2</td></tr></table>
          ＝ 1 － <span class="normal">cos</span>θ
        </div>
などの関係式を代入すると、
<em>
          <div class="math">
            Σ<sup>\*</sup>
            <span class="paren" style="font-size:em;">(</span>
              x, <span class="vector">u</span>
            <span class="paren" style="font-size:em;">)</span>
            Σ
            ＝
            <span class="paren" style="font-size:em;">(</span>
              x,
              <span class="normal">sin</span>θ <span class="vector">u</span>×<span class="vector">p</span>
              ＋
              <span class="normal">cos</span>θ <span class="vector">u</span>
              ＋
              <span class="paren" style="font-size:em;">(</span>
                1 － <span class="normal">cos</span>θ
              <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
                <span class="vector">u</span>・<span class="vector">p</span>
              <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
            <span class="paren" style="font-size:em;">)</span>
          </div>
        </em>
となります。
この式のベクトル部ですが、先ほど説明した3次元ベクトルの回転の式と一致しています。
すなわち、絶対値 1 の四元数 <span class="math">Σ</span> を用意し、
<em>
            <span class="math">
              Σ<sup>\*</sup> α Σ
            </span>
          </em> という計算をすることで、
3次元ベクトルの回転をすることができます。


####<a id="sec-generated-title-10"></a> <a id="appendix1"></a>余談
四元数そのものはそれほど使い道のあるものではないんですが、
四元数の発見は歴史的には大きな意味のあるものだったそうです。

まず、四元数は非可換体です。
四元数が発見される以前、有理数・実数・複素数などの体はいずれも可換なものでした。
また、後述する整数の剰余環なども可換体です。
初めて見つかった非可換な体ということで、そのインパクトは非常に大きなものがあります。

さらに、四元数の研究を通じてさまざまな分野が発展しました。
まず、四元数のベクトル部の研究からベクトル代数・ベクトル解析というものが生まれました。
また、四元数の非可換性の研究から行列式などが派生しました。

ちなみに、複素数から四元数を作った手順と同様の手順で8元数や16元数と呼ばれる数を作ることもできます。
名前の通り、8元数は8つの実数から、16元数は16個の実数からなる数です。
ただし、8元数および16元数は積に関して結合法則がなりたたないため体ではありません。
さらに、16元数は零因子も持っています。


####<a id="sec-generated-title-11"></a> <a id="appendix2"></a>余談2
回転軸と回転角度を指定するんじゃなくて、
球面上の点 <span class="math">
            <span class="vector">a</span>
          </span> を別の点 <span class="math">
            <span class="vector">b</span>
          </span> に移すような回転を考えるなら、以下のような式でできます。

簡単化のため、
<span class="math">
            <span class="vector">a</span>, <span class="vector">b</span>
          </span> は単位球面上の点
（<span class="math">
            <span class="normal">|</span>
              <span class="vector">a</span>
            <span class="normal">|</span> <span class="normal">=</span> <span class="normal">|</span>
              <span class="vector">b</span>
            <span class="normal">|</span> <span class="normal">=</span> 1
          </span>）
とします。
回転軸 <span class="math">
            <span class="vector">p</span>
          </span> は
<div class="math">
          <span class="vector">p</span>
          <span class="normal">=</span>
          <table class="frac" summary="fraction"><tr><td class="num">
              <span class="vector">a</span>
              <span class="normal">×</span>
              <span class="vector">b</span>
            </td></tr><tr><td>
              <span class="normal">|</span>
                <span class="vector">a</span>
                <span class="normal">×</span>
                <span class="vector">b</span>
              <span class="normal">|</span>
            </td></tr></table>
        </div>
回転角 <span class="math">θ</span> は
<div class="math">
          <span class="normal">cos</span> θ <span class="normal">=</span> <span class="vector">a</span> <span class="normal">⋅</span> <span class="vector">b</span>
        </div><div class="math">
          <span class="normal">sin</span> θ <span class="normal">=</span> <span class="normal">|</span>
            <span class="vector">a</span>
            <span class="normal">×</span>
            <span class="vector">b</span>
          <span class="normal">|</span>
        </div>
になるので、結局、
所望の変換 <span class="math">
            <span class="vector">u</span> → <span class="vector">u</span>'
          </span> は、
<div class="math">
          c <span class="normal">=</span> <span class="vector">a</span> <span class="normal">⋅</span> <span class="vector">b</span>
        </div><div class="math">
          <span class="vector">d</span>
          <span class="normal">=</span>
          <span class="vector">a</span>
          <span class="normal">×</span>
          <span class="vector">b</span>
        </div>
とおいて、
<div class="math">
          <span class="vector">u</span>'
          <span class="normal">=</span>
          <span class="vector">u</span>
          <span class="normal">×</span>
          <span class="vector">d</span>
          <span class="normal">+</span>
          c <span class="vector">u</span>
          <span class="normal">+</span>
          <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
            1 <span class="normal">+</span> c
          </td></tr></table><span class="paren" style="font-size:em;">(</span>
            <span class="vector">u</span>
            <span class="normal">⋅</span>
            <span class="vector">d</span>
          <span class="paren" style="font-size:em;">)</span><span class="vector">d</span>
        </div>
になります。


###<a id="sec-generated-title-12"></a> <a id="finite"></a>有限体
整数は環、有理数や実数などは体となりますが、
これらはいずれも無限集合です。
これに対して、有限集合となるような体も存在します。
このような体を<strong id="d54e2069" class="keyword">有限体</strong>（finite field）またはガロア体（Galois field）と呼びます。
（ガロアは人名。現在の群論・体論・代数論の基礎を築いた数学者。
情報系の分野ではガロア体と呼ばれる場合が多い。）

ここでは、この有限体の例として、
整数の剰余環を紹介します。


####<a id="sec-generated-title-13"></a> <a id="d54e2076"></a>整数の剰余環
0 から N－1 までの整数 <span class="math">a, b</span> に対して、以下のようにして加法と乗法を定めます。

* 加法:<span class="math">
              a ＋ b <span class="paren" style="font-size:em;">(</span>
                <span class="normal">mod</span> N
              <span class="paren" style="font-size:em;">)</span>
            </span>

* 乗法:<span class="math">
              a × b <span class="paren" style="font-size:em;">(</span>
                <span class="normal">mod</span> N
              <span class="paren" style="font-size:em;">)</span>
            </span>


mod N は N で割ったあまりを表します。
このようにして作った代数系を<em>整数の剰余環</em>と呼び、
<span class="math">
            <span class="bold">Z</span>/N<span class="bold">Z</span>
          </span> と表します。
（この記法がどういう意味なのかは、剰余体を説明する際に述べます。）

例として、<span class="math">
            <span class="bold">Z</span>/4<span class="bold">Z</span>
          </span> の加算および乗算の結果の表を示します。

<table summary="Z/4Z 加算表">
	<caption>
		Z/4Z 加算表
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>0</th>
		<th>1</th>
		<th>2</th>
		<th>3</th>
	</tr>
	<tr>
		<th>0</th>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<th>1</th>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">3</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
	</tr>
</table>


<table summary="Z/4Z 乗算表">
	<caption>
		Z/4Z 乗算表
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>1</th>
		<th>2</th>
		<th>3</th>
	</tr>
	<tr>
		<th>1</th>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">2</td>
		<td markdown="1">0</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">3</td>
		<td markdown="1">2</td>
		<td markdown="1">1</td>
	</tr>
</table>


<span class="math">
            <span class="bold">Z</span>/N<span class="bold">Z</span>
          </span> は「[環](#ring)」になっています。
加法・乗法ともに「[結合法則](algebraic.md#associative)」が成り立ち、「[単位元](algebraic.md#unity)」が存在しています。
加法に関しては、任意の元 <span class="math">a</span> に対して <span class="math">－a ＝ N － a</span> となり、逆元がただ1つ必ず存在します。
乗法に関しては、必ずしも逆元を持つとは限ず、零因子は持つ場合もあります。
先ほど例に挙げた <span class="math">
            <span class="bold">Z</span>/4<span class="bold">Z</span>
          </span> では、<span class="math">2</span> が零因子になっています（<span class="math">
            2×2 <span class="paren" style="font-size:em;">(</span>
              <span class="normal">mod</span> 4
            <span class="paren" style="font-size:em;">)</span> ＝ 4 <span class="paren" style="font-size:em;">(</span>
              <span class="normal">mod</span> 4
            <span class="paren" style="font-size:em;">)</span> ＝ 0
          </span>）。


####<a id="sec-generated-title-14"></a> <a id="d54e2349"></a>整数の剰余環の逆元
<span class="math">
            <span class="bold">Z</span>/N<span class="bold">Z</span>
          </span> の元 <span class="math">a</span> が逆元を持つための条件を考えるために、まず、以下のような定理を紹介します。

<blockquote markdown="1">
整数係数 <span class="math">a, b, c</span> を持つ不定方程式、
<div class="math">ax ＋ by ＝ c</div>
が整数解を持つための必要十分条件は、
<span class="math">
              <span class="normal">gcd</span><span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span> ＝ c
            </span> となることである。
（ただし、<span class="math">
              <span class="normal">gcd</span>
              <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>
            </span> は <span class="math">a, b</span> の最大公約数。）

</blockquote>
この定理の証明は省略しますが、
<span class="math">a, b</span> がともにある整数 <span class="math">n</span> の倍数のとき、
<span class="math">x, y</span> の値が何であろうと、<span class="math">ax ＋ by</span> の値も必ず <span class="math">n</span> の倍数になるので、直感的になんとなく分かってもらえるのではないかと思います。

ここで、この定理の式を少し書き換えて見ます。
まず、<span class="math">b</span> を <span class="math">N</span> で置き換え、<span class="math">c</span> は1に固定します。
<div class="math">
          ax ＋ Ny ＝ 1
        </div>
そして、両辺、<span class="math">N</span> で割ったあまりを取ります。
<div class="math">
          ax ≡ 1 <span class="paren" style="font-size:em;">(</span>
            <span class="normal">mod</span> N
          <span class="paren" style="font-size:em;">)</span>
        </div>
結果的に何が言えるかというと、
<span class="math">a</span> と <span class="math">N</span> が互いに素（最大公約数が1）のときに限り、
不定方程式 <span class="math">
            ax ≡ 1 <span class="paren" style="font-size:em;">(</span>
              <span class="normal">mod</span> N
            <span class="paren" style="font-size:em;">)</span>
          </span> が解を持つということになります。

この解 <span class="math">x</span> は、<span class="math">
            <span class="bold">Z</span>/N<span class="bold">Z</span>
          </span> における <span class="math">a</span> の乗法に関する逆元ということになりますので、
<em>
            <span class="math">a</span> は <span class="math">N</span> と互いに素な場合にのみ逆元を持つ
          </em>という結論が得られます。


####<a id="sec-generated-title-15"></a> <a id="rasidualfield"></a>整数の剰余体
<span class="math">
            <span class="bold">Z</span>/N<span class="bold">Z</span>
          </span> の元は、<span class="math">N</span> と互いに素な場合にのみ乗法に関する逆元を持ちます。
これは言い換えると、<span class="math">N</span> が素数 <span class="math">p</span> である場合には、
<span class="math">
            <span class="bold">Z</span>/p<span class="bold">Z</span>
          </span> のすべての元が乗法に関する逆元を持つことになります。
すなわち、<span class="math">
            <span class="bold">Z</span>/p<span class="bold">Z</span>
          </span> （<span class="math">p</span> は素数）は体をなします。
このようにして得られた体を整数の<em>剰余体</em>（residual field）と呼びます。

例として、剰余体 <span class="math">
            <span class="bold">Z</span>/3<span class="bold">Z</span>
          </span>、<span class="math">
            <span class="bold">Z</span>/5<span class="bold">Z</span>
          </span>、<span class="math">
            <span class="bold">Z</span>/7<span class="bold">Z</span>
          </span> の乗算結果の表を示します。

<table summary="Z/3Z 乗算表">
	<caption>
		Z/3Z 乗算表
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>1</th>
		<th>2</th>
	</tr>
	<tr>
		<th>1</th>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">2</td>
		<td markdown="1">1</td>
	</tr>
</table>


<table summary="Z/5Z 乗算表">
	<caption>
		Z/5Z 乗算表
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>1</th>
		<th>2</th>
		<th>3</th>
		<th>4</th>
	</tr>
	<tr>
		<th>1</th>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
		<td markdown="1">4</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">2</td>
		<td markdown="1">4</td>
		<td markdown="1">1</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">3</td>
		<td markdown="1">1</td>
		<td markdown="1">4</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<th>4</th>
		<td markdown="1">4</td>
		<td markdown="1">3</td>
		<td markdown="1">2</td>
		<td markdown="1">1</td>
	</tr>
</table>


<table summary="Z/7Z 乗算表">
	<caption>
		Z/7Z 乗算表
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>1</th>
		<th>2</th>
		<th>3</th>
		<th>4</th>
		<th>5</th>
		<th>6</th>
	</tr>
	<tr>
		<th>1</th>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
		<td markdown="1">4</td>
		<td markdown="1">5</td>
		<td markdown="1">6</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">2</td>
		<td markdown="1">4</td>
		<td markdown="1">6</td>
		<td markdown="1">1</td>
		<td markdown="1">3</td>
		<td markdown="1">5</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">3</td>
		<td markdown="1">6</td>
		<td markdown="1">2</td>
		<td markdown="1">5</td>
		<td markdown="1">1</td>
		<td markdown="1">4</td>
	</tr>
	<tr>
		<th>4</th>
		<td markdown="1">4</td>
		<td markdown="1">1</td>
		<td markdown="1">5</td>
		<td markdown="1">2</td>
		<td markdown="1">6</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<th>5</th>
		<td markdown="1">5</td>
		<td markdown="1">3</td>
		<td markdown="1">1</td>
		<td markdown="1">6</td>
		<td markdown="1">4</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<th>6</th>
		<td markdown="1">6</td>
		<td markdown="1">5</td>
		<td markdown="1">4</td>
		<td markdown="1">3</td>
		<td markdown="1">2</td>
		<td markdown="1">1</td>
	</tr>
</table>



####<a id="sec-generated-title-16"></a> <a id="d54e2890"></a>ブール体
2 も素数ですから、<span class="math">
            <span class="bold">Z</span>/2<span class="bold">Z</span>
          </span> も剰余体になります。
<span class="math">
            <span class="bold">Z</span>/2<span class="bold">Z</span>
          </span> の元は 0 と 1 の2つだけで、
加算および乗算は以下のようになります。

<table summary="Z/2Z の加算・乗算">
	<caption>
		Z/2Z の加算・乗算
	</caption>
	<tr>
		<th><span class="math">a</span></th>
		<th><span class="math">b</span></th>
		<th><span class="math">a ＋ b</span></th>
		<th><span class="math">a × b</span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
</table>


情報系の方ならこの表に見覚えがあるんじゃないでしょうか。
1 を真（true）、0 を偽（false）とみなせば、
<span class="math">
            <span class="bold">Z</span>/2<span class="bold">Z</span>
          </span> の加算は XOR、
乗算は AND 演算になっています。

<table summary="XOR と AND">
	<caption>
		XOR と AND
	</caption>
	<tr>
		<th><span class="math">a</span></th>
		<th><span class="math">b</span></th>
		<th><span class="math">a XOR b</span></th>
		<th><span class="math">a AND b</span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
</table>


要するに、論理値 {true, false} に対して、XOR で和を、AND で積を定義した代数系は
剰余体 <span class="math">
            <span class="bold">Z</span>/2<span class="bold">Z</span>
          </span> と同型な体になります。
このような体は、情報分野ではよく使われていて、
<strong id="bool" class="keyword">ブール体</strong>（Boolean field）などと呼ばれ、<span class="math">
            <span class="bold">B</span>
          </span> で表されます。
（論理代数の考案者 George Boole の名から付いた名前。）


##<a id="sec-generated-title-17"></a> <a id="concept"></a>環・体に関する諸概念
###<a id="sec-generated-title-18"></a> <a id="order"></a>位数
群の「[位数](group.md#order)」と同様に、
環・体に対しても、
その元の数（正確には「[濃度](../set/cardinality.md#cardinality)」）を<strong id="order" class="keyword">位数</strong>（order）と呼び、
<span class="math">
          <span class="normal">|</span>K<span class="normal">|</span>
        </span> というように表します。


###<a id="sec-generated-title-19"></a> <a id="isomorphic"></a>同型
これも「[群同型](group.md#g_isomorphic)」と同様、
環・体にも同型の概念があります。
環として同型であることを<strong id="r_isomorphic" class="keyword">環同型</strong>（ring isomorphic）、
体として同型であることを<strong id="f_isomorphic" class="keyword">体同型</strong>（field isomorphic）と呼びます。

同型の条件はもちろん、
集合として同値であり、
さらに、算法の結果にも1対1の対応が取れることです。
すなわち、環の場合には、
二つの環
<span class="math">
          <span class="paren" style="font-size:em;">{</span>
            R, <span class="paren" style="font-size:em;">{</span>
              ＋<sub>R</sub>, ×<sub>R</sub>
            <span class="paren" style="font-size:em;">}</span>
          <span class="paren" style="font-size:em;">}</span>
          ,
          <span class="paren" style="font-size:em;">{</span>
            S, <span class="paren" style="font-size:em;">{</span>
              ＋<sub>S</sub>, ×<sub>S</sub>
            <span class="paren" style="font-size:em;">}</span>
          <span class="paren" style="font-size:em;">}</span>
        </span>
の間に、
「[全単写](../set/map.md#bijection)」<span class="math">f : R → S</span> で、
任意の
<span class="math">
          a, b, c ∈ R
        </span>
に対して、
条件
<div class="math">
        f<span class="paren" style="font-size:em;">(</span>
          a ＋<sub>R</sub> b
        <span class="paren" style="font-size:em;">)</span>
        ＝
        f<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ＋<sub>S</sub> f<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span>
      </div><div class="math">
        f<span class="paren" style="font-size:em;">(</span>
          c ×<sub>R</sub><span class="paren" style="font-size:em;">(</span>
            a ＋<sub>R</sub> b
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
        ＝
        f<span class="paren" style="font-size:em;">(</span>c<span class="paren" style="font-size:em;">)</span>
        ×<sub>S</sub><span class="paren" style="font-size:em;">(</span>
          f<span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ＋<sub>S</sub> f<span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      </div>
を満たすものが存在するとき、2つの環は環同型であるといいます。
体の場合も同様です。


##<a id="sec-generated-title-20"></a> <a id="plan"></a>執筆予定
<pre>
      有限体関係の説明は別ページに移動。
      リンクを張る。
    </pre>
