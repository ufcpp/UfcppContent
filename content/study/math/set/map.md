---
title: "写像"
source_url: "https://ufcpp.net/study/math/set/map/"
content_type: "Article"
published_at: "2015-05-06T14:16:58"
updated_at: "2015-05-06T14:16:58"
tags: []
umbraco_id: 1474
parent_id: 1471
sort_order: 2
aliases:
  - "/math/set/map/"
  - "/set/map"
  - "/set/map.html"
  - "/study/set/map"
  - "/study/set/map.html"
---

# 写像

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
数学には関数（function）という概念があります。
関数とは「ある変数に依存して決まる値」の事を指します。
集合論的には、「ある2つの変数の間の対応関係」が関数になります。

通常、関数という言葉は数 → 数の対応関係を指します。
それに対して、一般の集合 → 集合の対応関係を写像（mapping）と呼びます。
（両者の間にはあまり差はありません。ニュアンスの違い程度です。）

集合論における数学的考察の対象は全て集合であるわけですが、
写像というものも集合の1種として定義することが出来ます。

余談ですが、関数という言葉は function を音訳したものです。
（中国語では「関」は「ファン」と読みます。
もともとは「函」と書いていましたが、この文字は常用漢字ではないので、次第に「関」に置き換えられるようになりました。）
古来の日本語には「h」や「f」の音はなく、は行の音は「p」の音で読まれていました。
そのため、「函」は「ハン」や「ファン」ではなく、「クワン」と読まれ、後に「カン」になったそうです。
（現在でも辞書などには「くわん」という読み方が書かれています。）


##<a id="sec-generated-title-2"></a> <a id="ordered"></a>順序対
写像について述べる前に、いくつか下準備が必要になります。
まず最初に、順序対というものについて説明します。

「[対](set.md#pair)」で「[対](set.md#pair)」というものを説明しましたが、
これは順序関係を持っていません。
すなわち、<span class="math">x, y</span> の対は <span class="math">
        <span class="paren" style="font-size:em;">{</span>x, y<span class="paren" style="font-size:em;">}</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>y, x<span class="paren" style="font-size:em;">}</span>
      </span> となります。
これに対し、順序を持った対、つまり、<span class="math">
        x <span class="normal">≠</span> y
      </span> のとき <span class="math">
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
        <span class="normal">≠</span>
        <span class="paren" style="font-size:em;">(</span>y, x<span class="paren" style="font-size:em;">)</span>
      </span> となるようなものを作ることを考えます。

このような集合を作るために、以下のようなものを考えます。
<div class="math">
      <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        x, <span class="paren" style="font-size:em;">{</span>x, y<span class="paren" style="font-size:em;">}</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
このようにして作った <span class="math">x, y</span> の組を<strong id="orderd_pair" class="keyword">順序対</strong>（ordered pair）と呼びます。
順序対は以下のような性質を持ちます。

* <span class="math">
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>y, x<span class="paren" style="font-size:em;">)</span> <span class="normal">⇔</span> x <span class="normal">=</span> y
        </span>

* <span class="math">
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>x', y'<span class="paren" style="font-size:em;">)</span> <span class="normal">⇔</span> x <span class="normal">=</span> x' <span class="normal">∧</span> y <span class="normal">=</span> y'
        </span>


要するに、対（非順序対）とは異なり、中身が順序も含めて一致している場合にのみ同じ集合になります。


##<a id="sec-generated-title-3"></a> <a id="directprod"></a>直積
次に、<strong id="directprod" class="keyword">直積</strong>（direct product）と呼ばれる集合を定義します。
直積とは、<span class="math">a</span> の元 <span class="math">x</span> と <span class="math">b</span> の元 <span class="math">y</span> の順序対 <span class="math">
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
      </span> 全体からなる集合で、<span class="math">a<span class="normal">×</span>b</span> と表します。

例えば、<span class="math">
        a <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>x, y, z<span class="paren" style="font-size:em;">}</span>, b <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
          <span class="normal">0</span>, <span class="normal">1</span>, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">}</span>
      </span> のとき、
<div class="math">
      a <span class="normal">×</span> b <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">)</span>,　
        <span class="paren" style="font-size:em;">(</span>
          y, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          y, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          y, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">)</span>,　
        <span class="paren" style="font-size:em;">(</span>
          z, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          z, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          z, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
になります。

上述の直積 <span class="math">a<span class="normal">×</span>b</span> の定義は、集合論的には以下のような表し方になります。
<div class="math">
      a<span class="normal">×</span>b <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        u <span class="normal">∈</span> <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>
          <span class="cursive">P</span>
          <span class="paren" style="font-size:em;">(</span>
            a <span class="normal">∪</span> b
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span> |
        <span class="normal">∃</span>x<span class="normal">∃</span>y
        <span class="paren" style="font-size:em;">(</span>
          x <span class="normal">∈</span> a <span class="normal">∧</span> y <span class="normal">∈</span> b <span class="normal">∧</span> u <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
（順序対 <span class="math">
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
      </span> 全体の集合は、<span class="math">
        <span class="cursive">P</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="cursive">P</span>
          <span class="paren" style="font-size:em;">(</span>
            a <span class="normal">∪</span> b
          <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> の部分集合になります。）

直積は以下のような性質を持っています。

* <span class="math">
          <span class="normal">∅</span><span class="normal">×</span>b  <span class="normal">=</span> a<span class="normal">×</span><span class="normal">∅</span> <span class="normal">=</span> <span class="normal">∅</span>
        </span>

* <span class="math">
          <span class="paren" style="font-size:em;">(</span>
            a <span class="normal">∩</span> b
          <span class="paren" style="font-size:em;">)</span><span class="normal">×</span>c <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>a<span class="normal">×</span>c<span class="paren" style="font-size:em;">)</span> <span class="normal">∩</span> <span class="paren" style="font-size:em;">(</span>b<span class="normal">×</span>c<span class="paren" style="font-size:em;">)</span>
        </span>

* <span class="math">
          <span class="paren" style="font-size:em;">(</span>
            a <span class="normal">∪</span> b
          <span class="paren" style="font-size:em;">)</span><span class="normal">×</span>c <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>a<span class="normal">×</span>c<span class="paren" style="font-size:em;">)</span> <span class="normal">∪</span> <span class="paren" style="font-size:em;">(</span>b<span class="normal">×</span>c<span class="paren" style="font-size:em;">)</span>
        </span>

* <span class="math">
          a ⊆ c <span class="normal">∧</span> b ⊆ d <span class="normal">→</span> a<span class="normal">×</span>b ⊆ c<span class="normal">×</span>d
        </span>



##<a id="sec-generated-title-4"></a> <a id="relation"></a>対応
直積を使って2つの集合 <span class="math">a, b</span> の間の元の対応関係を定義することが出来ます。

直積 <span class="math">a<span class="normal">×</span>b</span> とその部分集合 <span class="math">f</span> の順序対 <span class="math">
        <span class="paren" style="font-size:em;">(</span>a<span class="normal">×</span>b, f<span class="paren" style="font-size:em;">)</span>
      </span> のことを <span class="math">a</span> から <span class="math">b</span> への<strong id="correspondence" class="keyword">対応</strong>（correspondence）とよび、<span class="math">f</span> を対応 <span class="math">
        <span class="paren" style="font-size:em;">(</span>a<span class="normal">×</span>b, f<span class="paren" style="font-size:em;">)</span>
      </span> のグラフ（graph）と呼びます。
対応は、グラフ <span class="math">f</span> と <span class="math">a, b</span> の3つ組 <span class="math">
        <span class="paren" style="font-size:em;">(</span>f, a, b<span class="paren" style="font-size:em;">)</span>
      </span> （順序対の順序対 <span class="math">
        <span class="paren" style="font-size:em;">(</span>
          f, <span class="paren" style="font-size:em;">(</span>a, b<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>）で定義する流儀もあります。

ちなみに、<span class="math">a</span> から <span class="math">a</span> 自身への対応を<strong id="relation" class="keyword">関係</strong>（relation）と言います。
（集合 <span class="math">a</span> の2つの元の間の関係を表すのに使う。
例えば、順序関係等。）

対応  <span class="math">
        <span class="paren" style="font-size:em;">(</span>a<span class="normal">×</span>b, f<span class="paren" style="font-size:em;">)</span>
      </span> は、
便宜上、<span class="math">
        f : a <span class="normal">→</span> b
      </span> と表すこともよくあります。
<span class="math">a, b</span> があらかじめ与えられており、これらの表示を省略しても分かる場合には、
単に「対応 <span class="math">f</span>」ということもあります。

<span class="math">
        x <span class="normal">∈</span> a
      </span> のとき、
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        y <span class="normal">∈</span> b |
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> f
      <span class="paren" style="font-size:em;">}</span>
    </div>
を <span class="math">x</span> の <span class="math">f</span> による<strong id="image" class="keyword">像</strong>（image）といいます。

例えば、<span class="math">
        a <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>w, x, y, z<span class="paren" style="font-size:em;">}</span>, b <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
          <span class="normal">0</span>, <span class="normal">1</span>, <span class="normal">2</span>, <span class="normal">3</span>
        <span class="paren" style="font-size:em;">}</span>
      </span> のとき、
対応 <span class="math">f</span> を
<div class="math">
      f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          y, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          z, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
と定義すれば、
<div class="math">
      f<span class="paren" style="font-size:em;">(</span>w<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="normal">∅</span>
    </div><div class="math">
      f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        <span class="normal">0</span>, <span class="normal">1</span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      f<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        <span class="normal">2</span>
      <span class="paren" style="font-size:em;">}</span>
    </div><div class="math">
      f<span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
        <span class="normal">0</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
となる。
像が「[シングルトン](set.md#singleton)」になるとき、
すなわち、<span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>y<span class="paren" style="font-size:em;">}</span>
      </span> のようになる場合には、
これを <span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> y
      </span> と略記することもあります。

また、<span class="math">a</span> の部分集合 <span class="math">a' ⊆ a</span> に対して、
像 <span class="math">
        f<span class="paren" style="font-size:em;">[</span>a'<span class="paren" style="font-size:em;">]</span>
      </span> を
<div class="math">
      f<span class="paren" style="font-size:em;">[</span>a'<span class="paren" style="font-size:em;">]</span> <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        y <span class="normal">∈</span> b |
        <span class="normal">∃</span>x
        <span class="paren" style="font-size:em;">(</span>
          x <span class="normal">∈</span> a' <span class="normal">∧</span> <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> f
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
で定義します。
これは、<span class="math">
        f<span class="paren" style="font-size:em;">[</span>a'<span class="paren" style="font-size:em;">]</span> <span class="normal">=</span> <table class="sigma" summary="statement under a function"><tr><td><span class="normal">∩</span></td></tr><tr><td class="sigmasub">
          x <span class="normal">∈</span> a'
        </td></tr></table> f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
      </span> と定義するのと等しくなります。
また、<span class="math">
        f<span class="paren" style="font-size:em;">[</span>a<span class="paren" style="font-size:em;">]</span>
      </span> を <span class="math">f</span> の像または<strong id="range" class="keyword">値域</strong>（range または range of values）と呼び、<span class="math">
        <span class="normal">Im</span> f
      </span> と表します。
さらに、<span class="math">
        <span class="paren" style="font-size:em;">{</span>
          x <span class="normal">∈</span> a | f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">≠</span> <span class="normal">∅</span>
        <span class="paren" style="font-size:em;">}</span>
      </span> （<span class="math">f</span> 像が<span class="math">
        <span class="normal">∅</span>
      </span>にならないような元全体）を<strong id="domain" class="keyword">定義域</strong>（domain または domain of definition）と呼び、<span class="math">
        <span class="normal">Dom</span> f
      </span> と表します。

先ほどの例では、
<span class="math">f</span> の値域は <span class="math">
        <span class="normal">Im</span> f <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
          <span class="normal">0</span>, <span class="normal">1</span>, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">}</span>
      </span>
（<span class="math">
        <span class="normal">3</span>
      </span> という値をとることはないので）に、
定義域は <span class="math">
        <span class="normal">Dom</span> f <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>x, y, z<span class="paren" style="font-size:em;">}</span>
      </span>
（<span class="math">
        f<span class="paren" style="font-size:em;">(</span>w<span class="paren" style="font-size:em;">)</span>
      </span> は<span class="math">
        <span class="normal">∅</span>
      </span>なので）になります。
この様子を図1に示します。

<figure>
	[![像の値域と定義域](../../../../assets/media/ufcpp2000/math/map01.png)](../../../../assets/media/ufcpp2000/math/map01.png)
	<figcaption>像の値域と定義域</figcaption>
</figure>



##<a id="sec-generated-title-5"></a> <a id="morph"></a>写像
対応 <span class="math">
        f : a <span class="normal">→</span> b
      </span> のうちで、
<span class="math">
        <span class="normal">Dom</span> f <span class="normal">=</span> a
      </span> であり、
さらに、全ての値 <span class="math">
        f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
      </span> が<span class="math">b</span> の「[シングルトン](set.md#singleton)」となるものを <span class="math">a</span> から <span class="math">b</span> への<strong id="mapping" class="keyword">写像</strong>（mapping）と呼びます。
要するに、<span class="math">a</span> 全ての元に対して、ちょうど1つずつ値が割り当てられているような対応のことを写像といいます。

<figure>
	[![写像](../../../../assets/media/ufcpp2000/math/map02.png)](../../../../assets/media/ufcpp2000/math/map02.png)
	<figcaption>写像</figcaption>
</figure>


例えば、<span class="math">
        a <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>w, x, y, z<span class="paren" style="font-size:em;">}</span>, b <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
          <span class="normal">0</span>, <span class="normal">1</span>, <span class="normal">2</span>, <span class="normal">3</span>
        <span class="paren" style="font-size:em;">}</span>
      </span> のとき、
<div class="math">
      f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="paren" style="font-size:em;">(</span>
          w, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          y, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          z, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
は写像になります。
（<span class="math">w, x, y, z</span> に対して、それぞれ1つずつ値が割り当たっている。）
一方、
<div class="math">
      f <span class="normal">=</span>
      <span class="paren" style="font-size:em;">{</span>
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          x, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          y, <span class="normal">2</span>
        <span class="paren" style="font-size:em;">)</span>,
        <span class="paren" style="font-size:em;">(</span>
          z, <span class="normal">0</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">}</span>
    </div>
は対応ですが、写像ではありません。
（<span class="math">w</span> に値がない。<span class="math">x</span> が値を2つ持っている。）


###<a id="sec-generated-title-6"></a> <a id="spectial"></a>特殊な写像
いくつか特殊な写像の例を挙げます。

集合 <span class="math">a</span> に対して、
<span class="math">
          Δ<sub>a</sub> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
            <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> a<span class="normal">×</span>a | x <span class="normal">=</span> y
          <span class="paren" style="font-size:em;">}</span>
        </span> を対角集合（diagonal set）と呼びます。
グラフが対角集合であるような写像 <span class="math">
          id<sub>a</sub> <span class="normal">=</span> <span class="paren" style="font-size:em;">{</span>
            a<span class="normal">×</span>a, Δ<sub>a</sub>
          <span class="paren" style="font-size:em;">}</span>
        </span> は、<span class="math">a</span> からそれ自身への写像となり、<span class="math">
          f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> x
        </span> となります。
このような写像を <span class="math">a</span> の<strong id="identity" class="keyword">恒等写像</strong>（identity mapping）と呼びます。

また、直積 <span class="math">a<span class="normal">×</span>b</span> から <span class="math">a</span> への写像 <span class="math">f</span> を
<div class="math">
        f <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>, z
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">∈</span> <span class="paren" style="font-size:em;">(</span>a<span class="normal">×</span>b<span class="paren" style="font-size:em;">)</span><span class="normal">×</span>a |
          x <span class="normal">=</span> z
        <span class="paren" style="font-size:em;">}</span>
      </div>
で定義すると、
<span class="math">
          f<span class="paren" style="font-size:em;">(</span>
            <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
          <span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> x
        </span> となります。
（要するに、<span class="math">y</span> の値を無視して <span class="math">x</span> のみを取り出す写像。）
このような写像を <span class="math">a<span class="normal">×</span>b</span> から <span class="math">a</span> への<strong id="canonical" class="keyword">標準的射影</strong>（canonical projection）と呼び、
<span class="math">
          <span class="normal">proj</span>
          <sub>a</sub>
        </span> と表したりします。
（同様に、<span class="math">y</span> のみを取り出すような写像 <span class="math">
          <span class="normal">proj</span>
          <sub>b</sub>
        </span> も定義できます。）


###<a id="sec-generated-title-7"></a> <a id="surjection"></a>全写・単写
写像 <span class="math">
          f : a <span class="normal">→</span> b
        </span> が、<span class="math">
          <span class="normal">Im</span> f <span class="normal">=</span> b
        </span> を満たすとき、
<span class="math">f</span> は <span class="math">a</span> から <span class="math">b</span> への<strong id="surjection" class="keyword">全写</strong>（surjection）、
もしくは<span class="math">a</span> から <span class="math">b</span> の上への写像（onto mapping）といいます。

<figure>
	[![全写](../../../../assets/media/ufcpp2000/math/map03.png)](../../../../assets/media/ufcpp2000/math/map03.png)
	<figcaption>全写</figcaption>
</figure>


また、<span class="math">a</span> の任意の2元 <span class="math">x, y</span> について、
<span class="math">
          f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <span class="normal">=</span> f<span class="paren" style="font-size:em;">(</span>y<span class="paren" style="font-size:em;">)</span> <span class="normal">→</span> x <span class="normal">=</span> y
        </span> を満たすとき、
<span class="math">f</span> は <span class="math">a</span> から <span class="math">b</span> への<strong id="injection" class="keyword">単写</strong>（injection）、
もしくは1対1の写像（1:1 mapping）といいます。

<figure>
	[![単写](../../../../assets/media/ufcpp2000/math/map04.png)](../../../../assets/media/ufcpp2000/math/map04.png)
	<figcaption>単写</figcaption>
</figure>


写像 <span class="math">f</span> が全写かつ単写のとき、
<span class="math">a</span> から <span class="math">b</span> への<strong id="bijection" class="keyword">全単写</strong>（bijection）、
もしくは上への1対1の写像（1:1 onto mapping）といいます。

<figure>
	[![全単写](../../../../assets/media/ufcpp2000/math/map05.png)](../../../../assets/media/ufcpp2000/math/map05.png)
	<figcaption>全単写</figcaption>
</figure>


2つの集合の間に全単写が存在するとき、
その2つの集合の全ての元の間には1対1の対応があります。
このとき、2つの集合は<strong id="equivalent" class="keyword">同値</strong>（equivalent）（対等、等価などと訳す場合もあり）であるといいます。
互いに同値な集合というのは、集合的に完全に対等な関係にあると考えることが出来ます。


###<a id="sec-generated-title-8"></a> <a id="inverse"></a>逆写像
対応 <span class="math">
          f : a <span class="normal">→</span> b
        </span> が与えられたとき、
以下のような対応 <span class="math">
          f<sup><span class="normal">−1</span></sup> : b <span class="normal">→</span> a
        </span> が定義できます。
<div class="math">
        f<sup><span class="normal">−1</span></sup> <span class="normal">=</span>
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">(</span>y, x<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> b<span class="normal">×</span>a |
          <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span> <span class="normal">∈</span> f
        <span class="paren" style="font-size:em;">}</span>
      </div>
要するに、対応関係の向きを逆にしたものなんですが、
このようは対応 <span class="math">
          f<sup><span class="normal">−1</span></sup>
        </span> を <span class="math">f</span> の逆対応と呼びます。

写像は対応の一種ですから、
写像の逆対応を作ることが出来ます。
ただし、一般には写像の逆対応は写像とはなりません。

写像 <span class="math">f</span> の逆対応 <span class="math">
          f<sup><span class="normal">−1</span></sup>
        </span> が写像になるためには、
<span class="math">f</span> が全単写である必要があります。
<span class="math">f</span> が全単写である場合に限り、
その逆対応 <span class="math">
          f<sup><span class="normal">−1</span></sup>
        </span> もまた写像となり、しかも全単写になります。
このような写像 <span class="math">
          f<sup><span class="normal">−1</span></sup>
        </span> を <span class="math">f</span> の<strong id="inverse" class="keyword">逆写像</strong>（inverse mapping）と呼びます。


###<a id="sec-generated-title-9"></a> <a id="whole"></a>写像全体の集合
<span class="math">a</span> から <span class="math">b</span> への写像 <span class="math">
          f: a <span class="normal">→</span> b
        </span> のグラフ全体の集合を
<div class="math">
        b<sup>a</sup>
      </div>
で表します。


##<a id="sec-generated-title-10"></a> <a id="num"></a>元の個数
ある集合 <span class="math">a</span> に対して同値となるような自然数 <span class="math">n</span> が存在するとき、
集合 <span class="math">a</span> を<strong id="finite" class="keyword">有限集合</strong>（finite set）と呼びます。
逆に、そのような自然数が存在しないとき集合は<strong id="infinite" class="keyword">無限集合</strong>（infinite set）と呼びます。

このような自然数が存在するならば、その自然数はただ1つ確定します。
すなわち、<span class="math">a</span> と自然数 <span class="math">m</span> が同値でかつ <span class="math">a</span> と自然数 <span class="math">n</span> が成り立つとき、<span class="math">
        m <span class="normal">=</span> n
      </span> となります。
この1つに確定する自然数 <span class="math">n</span> を <span class="math">a</span> の<strong id="num" class="keyword">元の個数</strong>（number）と呼び、
<span class="math">
        <span class="normal">|</span>a<span class="normal">|</span>
      </span> と表します。

元の個数というと、有限集合にしか使えない概念ですが、
この概念は無限集合にも適用できるように拡張することができます。
この無限集合の元の個数に相当する拡張概念を位数（order）、濃度（power, cardinality）または基数（cardinal number）などと呼びますが、この概念はまた後ほど説明します。
