---
title: "整数"
source_url: "https://ufcpp.net/study/math/set/integer/"
content_type: "Article"
published_at: "2015-05-06T14:17:02"
updated_at: "2015-05-06T14:17:02"
tags: []
umbraco_id: 1476
parent_id: 1471
sort_order: 4
aliases:
  - "/math/set/integer/"
  - "/set/integer"
  - "/set/integer.html"
  - "/study/set/integer"
  - "/study/set/integer.html"
---

# 整数

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

自然数は加法に関して可換「[半群](../group/group.md#semigroup)」（足し算はできるが引き算はできない）になります。
<span class="math">
        a <span class="normal">+</span> b <span class="normal">=</span> 0
      </span> となるような自然数の組 <span class="math">a, b</span> は <span class="math">
        a <span class="normal">=</span> b <span class="normal">=</span> 0
      </span> 以外に存在しません。

整数は、自然数に対して負の数の概念を付け加ええることで、
加法に関して可換「[群](../group/group.md#group)」となるようにしたものです。
これは、自然数 <span class="math">a, b</span> から“<span class="math">
        a <span class="normal">−</span> b
      </span> という形で表される数”を作ることで実現できます。

“<span class="math">
        a <span class="normal">−</span> b
      </span> という形で表される数”というものをきちんと説明するためには、
まず、同値関係や商集合というものについて説明する必要があります。


## <a id="sec-generated-title-2"></a> <a id="equivalent"></a>同値

### <a id="sec-generated-title-3"></a> <a id="eqrelation"></a>同値関係

集合 <span class="math">A</span> からそれ自身への「[対応](map.md#correspondence)」<span class="math">
          <span class="paren" style="font-size:em;">(</span>
            A<span class="normal">×</span>A, f
          <span class="paren" style="font-size:em;">)</span>
        </span> （または、単にそのグラフ <span class="math">f</span>）を <span class="math">A</span> の上の関係（relation）とも呼びます。
<span class="math">A</span> の元 <span class="math">x, y</span> が <span class="math">
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> f
        </span> のとき、
「<span class="math">x, y</span> は <span class="math">f</span> により関係付けられる」といい、
<span class="math">
          x <table class="sigma" summary="statement under a function"><tr><td><span class="normal">～</span></td></tr><tr><td class="sigmasub">f</td></tr></table> y
        </span> と表します。
（<span class="math">f</span> が自明な場合には、単に <span class="math">x <span class="normal">∼</span> y</span> 表す。）

ある関係 <span class="math">f</span> が以下の条件を満たすとき、
同値関係（equivalence relation）といい、
2つの元 <span class="math">x, y</span> が同値関係を持っているとき、
<span class="math">x, y</span> は互いに<strong id="equivalent" class="keyword">同値</strong>（equivalent）であるといいます。

1. <span class="math">x <span class="normal">∼</span> x</span>（反射律）

2. <span class="math">x <span class="normal">∼</span> y <span class="normal">⇔</span> y <span class="normal">∼</span> x</span>（対称律）

3. <span class="math">x <span class="normal">∼</span> y <span class="normal">∧</span>  y <span class="normal">∼</span> z <span class="normal">⇔</span> x <span class="normal">∼</span> z</span>（推移律）


同値というのは、文字通り、「互いに同じ値であるとみなせる」ということです。
「集合として互いに等しい」、すなわち、<span class="math">
          x <span class="normal">=</span> y
        </span> という関係も同値関係の一種で、
相等関係（equality relation）と呼びます。

例えば、「2で割ったあまりが等しい」という関係は同値関係になります。
<div class="math">
        f <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> <span class="bold">N</span><span class="normal">×</span><span class="bold">N</span> |
          x <span class="normal">≡</span> y <span class="paren" style="font-size:em;">(</span>
            <span class="normal">mod</span>
            <span class="normal">2</span>
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">}</span>
      </div>
逆に、順序関係
<div class="math">
        f <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> <span class="bold">N</span><span class="normal">×</span><span class="bold">N</span> |
          x <span class="normal">≦</span> y
        <span class="paren" style="font-size:em;">}</span>
      </div>
などは同値関係にはなりません。
順序関係は、反射律および推移律は満たしますが、対称律を満たしていません。


### <a id="sec-generated-title-4"></a> <a id="eqclass"></a>同値類

互いに同値関係にある元同士は、全く同じ物であるとみなすことができます。
このとき、全く同じ物とみなせる元同士を代表するような物を1つ選ぶことを考えます。

同値関係 <span class="math">
          <span class="paren" style="font-size:em;">(</span>
            A <span class="normal">×</span> A, f
          <span class="paren" style="font-size:em;">)</span>
        </span> が与えられたとき、集合 <span class="math">A</span> のある元 <span class="math">x</span> に対して、
<span class="math">A</span> の部分集合 <span class="math">
          f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        </span> を <span class="math">x</span> の<strong id="eq_class" class="keyword">同値類</strong>（equivalent class）または代表元と呼びます。
例えば、
元 <span class="math">y, z</span> が <span class="math">x</span> と互いに同値であるとき、
<span class="math">
          <span class="bar">x</span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">{</span>x, y, z<span class="paren" style="font-size:em;">}</span>
        </span> という集合によって、
同値な3つの元 <span class="math">x, y, z</span> を代表させようということです。

具体例を1つ挙げると、0から8までの自然数に対して、
「3で割ったあまりが等しいとき互いに同値」という同値関係を導入したとします。
<div class="math">
        A <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
          <span class="normal">0</span>, <span class="normal">1</span>, <span class="normal">2</span>, <span class="normal">3</span>, <span class="normal">4</span>, <span class="normal">5</span>, <span class="normal">6</span>, <span class="normal">7</span>, <span class="normal">8</span>
        <span class="paren" style="font-size:em;">}</span>
      </div><div class="math">
        f <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> A<span class="normal">×</span>A |
          x <span class="normal">≡</span> y <span class="paren" style="font-size:em;">(</span>
            <span class="normal">mod</span>
            <span class="normal">3</span>
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">}</span>
      </div>
このとき、
<span class="math">
          <span class="normal">0</span> <span class="normal">∼</span> <span class="normal">3</span>
          <span class="normal">∼</span>
          <span class="normal">6</span>
        </span>、
<span class="math">
          <span class="normal">1</span>
          <span class="normal">∼</span>
          <span class="normal">4</span>
          <span class="normal">∼</span>
          <span class="normal">7</span>
        </span>、
<span class="math">
          <span class="normal">2</span>
          <span class="normal">∼</span>
          <span class="normal">5</span>
          <span class="normal">∼</span> <span class="normal">8</span>
      </span> という同値関係が成り立ち、
<span class="math">
          <span class="bar">
            <span class="normal">0</span>
          </span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">{</span>
            <span class="normal">0</span>, <span class="normal">3</span>, <span class="normal">6</span>
          <span class="paren" style="font-size:em;">}</span>
        </span>、
<span class="math">
          <span class="bar">
            <span class="normal">1</span>
          </span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">{</span>
            <span class="normal">1</span>, <span class="normal">4</span>, <span class="normal">7</span>
          <span class="paren" style="font-size:em;">}</span>
        </span>、
<span class="math">
          <span class="bar">
            <span class="normal">2</span>
          </span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">{</span>
            <span class="normal">2</span>, <span class="normal">5</span>, <span class="normal">8</span>
          <span class="paren" style="font-size:em;">}</span>
        </span>
という3つの同値類に分類されることになります。


## <a id="sec-generated-title-5"></a> <a id="quotientset"></a>商集合

集合 <span class="math">A</span> と、<span class="math">A</span> 上の同値関係 <span class="math">f</span> が与えられたとき、
<span class="math">A</span> 上の <span class="math">f</span> による同値類の集合を、
<span class="math">A</span> の <span class="math">f</span> による<strong id="quotient_set" class="keyword">商集合</strong>（quotient set）と呼び、<span class="math">A / f</span> と表します。
すなわち、商集合とは、
<div class="math">
      A / f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        X <span class="normal">∈</span> <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>A<span class="paren" style="font-size:em;">)</span> |
        <span class="normal">∃</span> x 
        <span class="paren" style="font-size:em;">(</span>
          x <span class="normal">∈</span> A <span class="normal">∧</span> X <span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
と定義される集合です。

例えば、先ほどの例であげた集合および同値関係
<div class="math">
      A <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        <span class="normal">0</span>, <span class="normal">1</span>, <span class="normal">2</span>, <span class="normal">3</span>, <span class="normal">4</span>, <span class="normal">5</span>, <span class="normal">6</span>, <span class="normal">7</span>, <span class="normal">8</span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> A<span class="normal">×</span>A |
        x <span class="normal">≡</span> y <span class="paren" style="font-size:em;">(</span>
          <span class="normal">mod</span>
          <span class="normal">3</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
を用いて商集合を作ると、
<div class="math">
      A / f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="bar">
          <span class="normal">0</span>
        </span>, <span class="bar">
          <span class="normal">1</span>
        </span>, <span class="bar">
          <span class="normal">2</span>
        </span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
となります。
ただし、
<span class="math">
        <span class="bar">
          <span class="normal">0</span>
        </span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="normal">0</span>, <span class="normal">3</span>, <span class="normal">6</span>
        <span class="paren" style="font-size:em;">}</span>
      </span>、
<span class="math">
        <span class="bar">
          <span class="normal">1</span>
        </span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="normal">1</span>, <span class="normal">4</span>, <span class="normal">7</span>
        <span class="paren" style="font-size:em;">}</span>
      </span>、
<span class="math">
        <span class="bar">
          <span class="normal">2</span>
        </span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="normal">2</span>, <span class="normal">5</span>, <span class="normal">8</span>
      <span class="paren" style="font-size:em;">}</span>
      </span> です。


## <a id="sec-generated-title-6"></a> <a id="integer"></a>整数の定義

<strong id="integer" class="keyword">整数</strong>（integer number）は以下のような手順で定義します。

* 自然数の対<span class="math">
          <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> ω<span class="normal">×</span>ω
        </span>を用意する。

* 2つの対<span class="math">
          m <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>, n <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span>
        </span>に対して、「<span class="math">
          a <span class="normal">+</span> d <span class="normal">=</span> b <span class="normal">+</span> c
        </span>のとき互いに同値」という同値関係を定める。

* この同値関係を使って商集合<span class="math">
          <span class="bold">Z</span>
        </span>を作る。

* この<span class="math">
          <span class="bold">Z</span>
        </span>を整数と呼ぶ。


すなわち、
<div class="math">
      ω<sup><span class="normal">2</span></sup> <span class="normal">=</span> ω<span class="normal">×</span>ω
    </div><div class="math">
      f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="paren" style="font-size:em;">(</span>m, n<span class="paren" style="font-size:em;">)</span>
        <span class="normal">∈</span> ω<sup><span class="normal">2</span></sup> <span class="normal">×</span> ω<sup><span class="normal">2</span></sup>
        |
        m <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>,
        n <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span>,
        a <span class="normal">+</span> d <span class="normal">=</span> b <span class="normal">+</span> c
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      <span class="bold">Z</span> <span class="normal">=</span> ω<sup><span class="normal">2</span></sup> / f
    </div>
となります。

このとき、自然数の対 <span class="math">
        <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>
      </span> を <em>
        <span class="math">
          a <span class="normal">−</span> b
        </span>
      </em> とも書きます。
同値関係 <span class="math">f</span> は、要するに「自然数の対の差が等しければ同値」ということになります。
具体例を挙げると、
<span class="math">
        <span class="normal">0</span> <span class="normal">−</span>
        <span class="normal">3</span>
      </span> と <span class="math">
        <span class="normal">1</span> <span class="normal">−</span> <span class="normal">4</span>
      </span> と <span class="math">
        <span class="normal">2</span> <span class="normal">−</span>
        <span class="normal">5</span>
      </span> は互いに同値ということで、
直感的な整数のイメージ通りのものになっています。

同値類 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>
          a <span class="normal">−</span> <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> は自然数 <span class="math">a</span> と1対1に対応するので、
これを <span class="math">a</span> と同一視することができ、
<em>自然数は整数の部分集合である</em>とみなすことができます。
そこで、同値類 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>
          a <span class="normal">−</span> <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> を単に自然数 <span class="math">a</span> で表します。
また、同値類 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>
          <span class="normal">0</span> <span class="normal">−</span> a
        <span class="paren" style="font-size:em;">)</span>
      </span> を <span class="math">
        <span class="normal">−</span>a
      </span> と表します。
<span class="math">
        <span class="normal">−</span>a
      </span> は <span class="math">a</span> の加法に関する逆元になります。
すなわち、<span class="math">
        a <span class="normal">+</span> <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>a
        <span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="normal">0</span>
      </span> が成り立ちます。


## <a id="sec-generated-title-7"></a> <a id="operation"></a>整数の間の関係・演算

### <a id="sec-generated-title-8"></a> <a id="order"></a>整数の順序

整数の順序関係は
<div class="math">
        a <span class="normal">−</span> b <span class="normal">∈</span> <span class="bold">N</span> <span class="normal">⇔</span> a <span class="normal">&gt;</span> b
      </div><div class="math">
        a <span class="normal">−</span> b <span class="normal">=</span> <span class="normal">0</span> <span class="normal">⇔</span> a <span class="normal">=</span> b
      </div><div class="math">
        b <span class="normal">−</span> a <span class="normal">∈</span> <span class="bold">N</span> <span class="normal">⇔</span> a <span class="normal">&lt;</span> b
      </div>
で定義します。
この順序関係は、自然数の順序関係の自然な拡張になっていて、以下の命題が成り立ちます。

* <span class="math">
            m <span class="normal">≦</span> n <span class="normal">∧</span> n <span class="normal">≦</span> m <span class="normal">⇒</span> m <span class="normal">=</span> n
          </span>

* <span class="math">l <span class="normal">≦</span> m <span class="normal">∧</span> m <span class="normal">≦</span> n <span class="normal">⇒</span> l <span class="normal">≦</span> n</span>

* 任意の整数<span class="math">m, n</span>に関して、<span class="math">
            m <span class="normal">&lt;</span> n, m <span class="normal">=</span> n, m <span class="normal">&gt;</span> n
          </span>のいずれか1つが必ず、そしてただ1つのみが成り立つ。



### <a id="sec-generated-title-9"></a> <a id="sum"></a>整数の和・積

2つの整数 <span class="math">
          m <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>, n <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>c, d<span class="paren" style="font-size:em;">)</span>
        </span> の間の和・積を、
<div class="math">
        m <span class="normal">+</span> n <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>
          a <span class="normal">+</span> c, b <span class="normal">+</span> d
        <span class="paren" style="font-size:em;">)</span>
      </div><div class="math">
        m <span class="normal">×</span> n <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>
          ac <span class="normal">+</span> bd, ad <span class="normal">+</span> bc
        <span class="paren" style="font-size:em;">)</span>
      </div>
で定義します。
これらは自然数の和・積の自然な拡張であり、結合法則・交換法則・分配法則などが成り立ちます。


### <a id="sec-generated-title-10"></a> <a id="algebra"></a>代数系としての整数

整数は、和に関して可換「[群](../group/group.md#group)」、
積に関して可換「[半群](../group/group.md#semigroup)」になります。
また、和と積の間に分配法則が成り立つので、整数は「[環](../group/field.md#ring)」となります。
環であることを明示的に表すために、整数を<em>整数環</em>と呼ぶこともあります。


### <a id="sec-generated-title-11"></a> <a id="misc"></a>余談

ときどき、<span class="math">
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">−</span>
            <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">×</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">−</span>
            <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
        </span> はなぜ 1 になるのかという疑問の声を耳にしますが、
これも上の定義を使うと自明になります。
<span class="math">
          <span class="normal">−</span>
          <span class="normal">1</span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">0</span>, <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
        </span>
なので、
<span class="math">
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">−</span>
            <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">×</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">−</span>
            <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">0</span>, <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">×</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">0</span>, <span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">0</span><span class="normal">×</span><span class="normal">0</span><span class="normal">+</span><span class="normal">1</span><span class="normal">×</span><span class="normal">1</span>, <span class="normal">0</span><span class="normal">×</span><span class="normal">1</span><span class="normal">+</span><span class="normal">0</span><span class="normal">×</span><span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">=</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">1</span>, <span class="normal">0</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">=</span>
          <span class="normal">1</span>
        </span>
