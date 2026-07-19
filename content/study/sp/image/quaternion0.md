---
title: "四元数の数学的意味"
source_url: "https://ufcpp.net/study/sp/image/quaternion0/"
content_type: "Article"
published_at: "2008-08-17T00:00:00"
updated_at: "2015-05-06T14:23:04"
tags: []
umbraco_id: 1625
parent_id: 1623
sort_order: 1
aliases:
  - "/image/quaternion0"
  - "/image/quaternion0.html"
  - "/sp/image/quaternion0/"
  - "/study/image/quaternion0"
  - "/study/image/quaternion0.html"
---

# 四元数の数学的意味

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[四元数と3次元空間中の回転](quaternion.md)」の付録。

四元数の数学的な側面について説明します。

はっきり言って、画像処理の分野では不要な知識。
画像処理（主に 3D CG）の分野では、とりあえず、
「四元数とは、回転の軸と角度を表わすために使うデータの形式」
とだけ覚えておけば OK。

ここで話す内容は要するに、
「なんでそれを四元数と呼ぶんだろう」という疑問に答えるものです。

ちなみに、「[ハミルトンの四元数体](../../math/group/field.md#quaternion)」の内容の焼き直しだったりします。
より深く理解するためには、
「[群](../../math/group/group.md#group)」、
「[環](../../math/group/field.md#ring)」、
「[体](../../math/group/field.md#field)」などについて調べることをお勧めします。


## <a id="sec-generated-title-2"></a> <a id="complex"></a>その前に・・・ 複素数についておさらい

四元数の説明に入る前に、
少し複素数についておさらいしておきます。
簡単に言うと、複素数ってのは以下のようなものです。

* 実数に、<span class="math">
          i<sup><span class="normal">2</span></sup> <span class="normal">=</span> <span class="normal">−</span><span class="normal">1</span>
        </span>となる元<span class="math">i</span>を追加したもの。

* 実数上の2次元ベクトルともみなせる。

* 曲形式<span class="math">
          α <span class="normal">=</span> r <span class="paren" style="font-size:em;">(</span>
            <span class="normal">cos</span>θ <span class="normal">+</span> i <span class="normal">sin</span>θ
          <span class="paren" style="font-size:em;">)</span>
        </span>で表現できる。

* 複素数同士の掛け算で、2次元の回転を表現できる。

* 四則演算に関して閉じている。


で、四元数はこれと似た性質を持っています。
（というか、複素数をさらに拡張した数になっています。）

* 複素数に、虚数単位<span class="math">i</span>に加えてさらに、<span class="math">
          j<sup><span class="normal">2</span></sup> <span class="normal">=</span> <span class="normal">−</span><span class="normal">1</span>
        </span>となる元<span class="math">j</span>を追加したもの。

* すなわち、<span class="math">i, j</span>と<span class="math">
          k <span class="normal">=</span> ij
        </span>を使って、<span class="math">
          a <span class="normal">+</span> i b <span class="normal">+</span> j c <span class="normal">+</span> k d
        </span>と表わされる（<span class="math">a, b, c, d</span>は実数）。

* 複素数上の2次元ベクトル、実数上の4次元ベクトルとみなせる。

* 回転軸となる単位ベクトル<span class="math">
          <span class="paren" style="font-size:em;">(</span>x, y, z<span class="paren" style="font-size:em;">)</span>
        </span>と角度<span class="math">θ</span>を使って、<span class="math">
          q <span class="normal">=</span> r <span class="paren" style="font-size:em;">(</span>
            <span class="normal">sin</span>θ,
            x <span class="normal">cos</span>θ,
            y <span class="normal">cos</span>θ,
            z <span class="normal">cos</span>θ
          <span class="paren" style="font-size:em;">)</span>
        </span>と表現したりする。

* 四元数の掛け算を使って、3次元の回転を表現できる。

* 四則演算に関して閉じている。ただし、複素数と違って、積は非可換。



## <a id="sec-generated-title-3"></a> <a id="quaternion"></a>四元数

複素数は、実数に対して
<span class="math">
        i<sup></sup> <span class="normal">=</span> <span class="normal">−</span><span class="normal">1</span>
      </span>
となる数 <span class="math">i</span> を付け足したもので、
2つの実数 <span class="math">a, b</span> を使って
<span class="math">
        a <span class="normal">+</span> i b
      </span>
と書ける数です。

これと同様に、今度は複素数に対して、
<span class="math">
        j<sup></sup> <span class="normal">=</span> <span class="normal">−</span><span class="normal">1</span>
      </span>
となる数 <span class="math">j</span> をさらに付け足したものが<strong id="quaternion" class="keyword">四元数</strong>（quaternion）です。
2つの複素数 <span class="math">α, β</span> を使って、
<div class="math">
      α <span class="normal">+</span> jβ<sup>*</sup>
    </div>
と書くか、あるいは、
<span class="math">
        k  <span class="normal">=</span> ij
      </span>
と置いて、
4つの実数 <span class="math">a, b, c, d</span> を使って、
<div class="math">
      a <span class="normal">+</span> i b <span class="normal">+</span> j c <span class="normal">+</span> k d
    </div>
と書けます。
四元数という名前は、見ての通り、4つの実数から成る数という意味です。

ちなみに、
<span class="math">i, j, k</span>
の間には、以下のような関係が成り立ちます。
<div class="math">
      i<sup><span class="normal">2</span></sup> <span class="normal">=</span> j<sup><span class="normal">2</span></sup> <span class="normal">=</span> k<sup><span class="normal">2</span></sup> <span class="normal">=</span> <span class="normal">−</span><span class="normal">1</span>
    </div><div class="math">
      ij <span class="normal">=</span> k, ki <span class="normal">=</span> j, jk <span class="normal">=</span> i
    </div><div class="math">
      ji <span class="normal">=</span> <span class="normal">−</span>k, ik <span class="normal">=</span> <span class="normal">−</span>j, kj <span class="normal">=</span> <span class="normal">−</span>i
    </div>
四元数は、発見者の名前を取ってハミルトンの四元数（Hamilton's quaternion）とも呼ばれます。


## <a id="sec-generated-title-4"></a> <a id="re_im"></a>実部と虚部

複素数では実部が実数1つ、虚部も実数1つでしたが、
四元数では虚部が3つになっています。
この虚部の3つの実数は1セットで意味を持っていたりするので、
四元数、
<div class="math">
      a <span class="normal">+</span> i b <span class="normal">+</span> j c <span class="normal">+</span> k d
    </div>
を、
実部 <span class="math">
        x <span class="normal">=</span> a
      </span> と、
虚部
<span class="math">
        <span class="vector">u</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>b, c, d<span class="paren" style="font-size:em;">)</span>
      </span>
に分けて、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        x<span class="normal">; </span> <span class="vector">u</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
と表したりします。
実部・虚部をそれぞれスカラー部・ベクトル部と呼んだりもします。

また、ベクトル部が 0 ベクトルのとき、
四元数
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="vector">
            <span class="normal">0</span>
          </span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
を実数と同一視し、
単に <span class="math">x</span> で書き表します。

このような形式を用いることで、以下に述べるように、
加減乗除などの計算が簡単に書き表すことができます。


##### <a id="sec-generated-title-5"></a>加減算

まず、四元数の加減算は非常に単純で、以下のようになります。
<div class="math">
      <em>
        <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">±</span>
        <span class="paren" style="font-size:em;">(</span>
          y<span class="normal">; </span> <span class="vector">v</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          x <span class="normal">±</span> y<span class="normal">; </span> <span class="vector">u</span> <span class="normal">±</span> <span class="vector">v</span>
        <span class="paren" style="font-size:em;">)</span>
      </em>
    </div>

##### <a id="sec-generated-title-6"></a>乗算

次に、乗算ですが、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        a <span class="normal">+</span> i b <span class="normal">+</span> j c <span class="normal">+</span> k d
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">×</span>
      <span class="paren" style="font-size:em;">(</span>
        e <span class="normal">+</span> i f <span class="normal">+</span> j g <span class="normal">+</span> k h
      <span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        ae <span class="normal">−</span> bf <span class="normal">−</span> cg <span class="normal">−</span> dh
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">+</span>
      i<span class="paren" style="font-size:em;">(</span>
        af <span class="normal">+</span> be <span class="normal">+</span> ch <span class="normal">−</span> dg
      <span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      <span class="normal">+</span>
      j<span class="paren" style="font-size:em;">(</span>
        ag <span class="normal">+</span> ce <span class="normal">+</span> df <span class="normal">−</span> bh
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">+</span>
      k<span class="paren" style="font-size:em;">(</span>
        ah <span class="normal">+</span> de <span class="normal">+</span> bg <span class="normal">−</span> cf
      <span class="paren" style="font-size:em;">)</span>
    </div>
なので、ベクトル表現では以下のようになります。
<div class="math">
      <em>
        <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">×</span>
        <span class="paren" style="font-size:em;">(</span>
          y<span class="normal">; </span> <span class="vector">v</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          xy <span class="normal">−</span> <span class="vector">u</span> <span class="normal">⋅</span> <span class="vector">v</span>
          <span class="normal">; </span>
          x <span class="vector">v</span> <span class="normal">+</span> y <span class="vector">u</span> <span class="normal">+</span> <span class="vector">u</span> <span class="normal">×</span> <span class="vector">v</span>
        <span class="paren" style="font-size:em;">)</span>
      </em>
    </div>
ただし、
2つのベクトル
<span class="math">
        <span class="vector">u</span>, <span class="vector">v</span>
      </span>
に対する
<span class="math">
        <span class="vector">u</span>
        <span class="normal">⋅</span>
        <span class="vector">v</span>
      </span>
、
<span class="math">
        <span class="vector">u</span>
        <span class="normal">×</span>
        <span class="vector">v</span>
      </span>
はそれぞれ、ベクトルの内積・外積です。

複素数の場合と違って、
<span class="math">
        <span class="vector">u</span>
        <span class="normal">×</span>
        <span class="vector">v</span>
      </span>
の部分が非可換なので、
四元数の積は非可換になります。


## <a id="sec-generated-title-7"></a> <a id="conjugate"></a>共役と逆元

四元数
<span class="math">
        q
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
に対して、
実数
<span class="math">
        <span class="normal" style="font-size:em;">√</span><span class="bar">
          x<sup><span class="normal">2</span></sup> <span class="normal">+</span>
          <span class="normal">|</span>
            <span class="vector">u</span>
          <span class="normal">|</span><sup><span class="normal">2</span></sup>
        </span>
      </span>
を
<span class="math">q</span>
の<em>絶対値</em>と呼び、
<span class="math">
        <span class="normal">|</span>q<span class="normal">|</span>
      </span>
で表します。

また、四元数
<span class="math">
        q
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
に対して、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="normal">−</span><span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
で表される四元数を、
<em>共役</em>な四元数と呼び、
<span class="math">
        q<sup>*</sup>
      </span>
で表します。
（あるいは、
<span class="math">
        <span class="bar">q</span>
      </span>
と表したりもします。）

四元数
<span class="math">q</span>
とその共役四元数を掛け合わせると、
<div class="math">
      qq<sup>*</sup>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        x<span class="normal">; </span> <span class="vector">u</span>
      <span class="paren" style="font-size:em;">)</span> <span class="normal">×</span> <span class="paren" style="font-size:em;">(</span>
        x<span class="normal">; </span> <span class="normal">−</span><span class="vector">u</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        x<sup><span class="normal">2</span></sup> <span class="normal">−</span> <span class="vector">u</span> <span class="normal">⋅</span> <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>
          <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span><span class="normal">; </span>
        x <span class="vector">u</span> <span class="normal">−</span> x <span class="vector">u</span> <span class="normal">+</span> <span class="vector">u</span>
        <span class="normal">×</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>
          <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
      <span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        x<sup><span class="normal">2</span></sup> <span class="normal">+</span> <span class="normal">|</span>
          <span class="vector">u</span>
        <span class="normal">|</span><sup><span class="normal">2</span></sup><span class="normal">; </span> <span class="vector">
          <span class="normal">0</span>
        </span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      x<sup><span class="normal">2</span></sup> <span class="normal">+</span> <span class="normal">|</span>
        <span class="vector">u</span>
      <span class="normal">|</span><sup><span class="normal">2</span></sup>
    </div>
というように、
<span class="math">q</span>
の絶対値の2乗になります。

このことから、
<div class="math">
      <em>
        q<span class="normal">×</span><table class="frac" summary="fraction"><tr><td class="num">
            q<sup>*</sup>
          </td></tr><tr><td>
            <span class="normal">|</span>q<span class="normal">|</span>
            <sup><span class="normal">2</span></sup>
          </td></tr></table>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            q<sup>*</sup>
          </td></tr><tr><td>
            <span class="normal">|</span>q<span class="normal">|</span>
            <sup><span class="normal">2</span></sup>
          </td></tr></table><span class="normal">×</span>q
        <span class="normal">=</span>
        <span class="normal">1</span>
      </em>
    </div>
となるので、<span class="math">q</span> が非 0 のとき、
必ず逆元が存在し、
<span class="math">
        q<sup><span class="normal">−1</span></sup>
        <span class="normal">=</span>
        <table class="frac" summary="fraction"><tr><td class="num">
            q<sup>*</sup>
          </td></tr><tr><td>
            <span class="normal">|</span>q<span class="normal">|</span>
            <sup><span class="normal">2</span></sup>
          </td></tr></table>
      </span> と表せます。
したがって、四元数は非可換体になります。
（四則演算がすべて問題なく行える。ただし、積は非可換（左右入れ替えると値が変わる）。）


## <a id="sec-generated-title-8"></a> <a id="rotation"></a>四元数を使った回転

「[3次元空間上の回転](quaternion.md#rotation)」で説明したように、
座標ベクトル <span class="math">
        <span class="vector">u</span>
      </span> で表される点 A を、回転軸 <span class="math">
        <span class="vector">p</span>
      </span> を中心に角度 <span class="math">θ</span> 回転した点 A' の座標ベクトル <span class="math">
        <span class="vector">u</span>'
      </span> は、以下のような計算で求めることができます。
<div class="math">
      <span class="vector">u</span>'
      <span class="normal">=</span>
      <span class="normal">sin</span>θ <span class="vector">u</span>
      <span class="normal">×</span>
      <span class="vector">p</span>
      <span class="normal">+</span>
      <span class="normal">cos</span>θ <span class="paren" style="font-size:em;">(</span>
        <span class="vector">u</span>
        <span class="normal">−</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="vector">u</span><span class="normal">⋅</span><span class="vector">p</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">+</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">u</span><span class="normal">⋅</span><span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
    </div><div class="math">
      <span class="normal">=</span>
      <em>
        <span class="normal">sin</span>θ <span class="vector">u</span>
        <span class="normal">×</span>
        <span class="vector">p</span>
        <span class="normal">+</span>
        <span class="normal">cos</span>θ
        <span class="vector">u</span>
        <span class="normal">+</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
        <span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="vector">u</span>
          <span class="normal">⋅</span>
          <span class="vector">p</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="vector">p</span>
      </em>
    </div>
このことを踏まえた上で、本題の四元数を使った回転の話に入ります。
まず、絶対値が 1 になるような四元数を用意します。
絶対値が 1 の四元数 <span class="math">Σ</span> は以下のように、
絶対値 1 の3次元ベクトル <span class="math">
        <span class="vector">p</span>
      </span> と角度 <span class="math">θ</span> を用いて表すことができます。
<div class="math">
      Σ <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
        <span class="normal">; </span>
        <span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
そして、この四元数とその共役を使って、
以下のようにして他の四元数
<span class="math">
        q <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>
          x<span class="normal">; </span> <span class="vector">u</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> を挟み込むように掛けます。
<div class="math">
      Σ<sup>*</sup>qΣ
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
        <span class="normal">; </span>
        <span class="normal">−</span><span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">×</span>q<span class="normal">×</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
        <span class="normal">; </span>
        <span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
このままだと少し計算が面倒なので、いったん <span class="math">
        Σ <span class="normal">=</span> <span class="paren" style="font-size:em;">(</span>
          y<span class="normal">; </span> <span class="vector">v</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> と置いて計算します。
<div class="math">
      Σ<sup>*</sup>qΣ
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        y<span class="normal">; </span> <span class="normal">−</span><span class="vector">v</span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
        x<span class="normal">; </span> <span class="vector">u</span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
        y<span class="normal">; </span> <span class="vector">v</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        xy <span class="normal">+</span> <span class="vector">u</span><span class="normal">⋅</span><span class="vector">v</span>,
        y<span class="vector">u</span> <span class="normal">−</span> x<span class="vector">v</span> <span class="normal">−</span> <span class="vector">v</span><span class="normal">×</span><span class="vector">u</span>
      <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
        y<span class="normal">; </span> <span class="vector">v</span>
      <span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        x <span class="paren" style="font-size:em;">(</span>
          y<sup><span class="normal">2</span></sup> <span class="normal">+</span> <span class="normal">|</span>
            <span class="vector">v</span>
          <span class="normal">|</span><sup><span class="normal">2</span></sup>
        <span class="paren" style="font-size:em;">)</span><span class="normal">; </span>
        <span class="normal">2</span>y <span class="vector">u</span><span class="normal">×</span><span class="vector">v</span>
        <span class="normal">+</span> <span class="paren" style="font-size:em;">(</span>
          y<sup><span class="normal">2</span></sup> <span class="normal">−</span> <span class="normal">|</span>
            <span class="vector">v</span>
          <span class="normal">|</span><sup><span class="normal">2</span></sup>
        <span class="paren" style="font-size:em;">)</span><span class="vector">u</span>
        <span class="normal">+</span> <span class="normal">2</span> <span class="paren" style="font-size:em;">(</span>
          <span class="vector">u</span><span class="normal">⋅</span><span class="vector">v</span>
        <span class="paren" style="font-size:em;">)</span><span class="vector">v</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
この式に、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        y<sup><span class="normal">2</span></sup> <span class="normal">+</span> <span class="normal">|</span>
          <span class="vector">v</span>
        <span class="normal">|</span><sup><span class="normal">2</span></sup>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span> <span class="normal">sin</span><sup><span class="normal">2</span></sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
      <span class="normal">+</span> <span class="normal">cos</span><sup><span class="normal">2</span></sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table> <span class="normal">=</span>
      <span class="normal">1</span>
    </div><div class="math">
      <span class="paren" style="font-size:em;">(</span>
        y<sup><span class="normal">2</span></sup> <span class="normal">−</span> <span class="normal">|</span>
          <span class="vector">v</span>
        <span class="normal">|</span><sup><span class="normal">2</span></sup>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span> <span class="normal">cos</span><sup><span class="normal">2</span></sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
      <span class="normal">−</span> <span class="normal">sin</span><sup><span class="normal">2</span></sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
      <span class="normal">=</span> <span class="normal">cos</span>θ
    </div><div class="math">
      <span class="normal">2</span> y <span class="vector">v</span>
      <span class="normal">=</span>
      <span class="normal">2</span> <span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table><span class="vector">p</span>
      <span class="normal">=</span>
      <span class="normal">sin</span>θ
      <span class="vector">p</span>
    </div><div class="math">
      <span class="normal">2</span> <span class="normal">sin</span><sup><span class="normal">2</span></sup><table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td><span class="normal">2</span></td></tr></table>
      <span class="normal">=</span> <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
    </div>
などの関係式を代入すると、
<div class="math">
      Σ<sup>*</sup>
      <span class="paren" style="font-size:em;">(</span>
        x<span class="normal">; </span> <span class="vector">u</span>
      <span class="paren" style="font-size:em;">)</span>
      Σ
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        x<span class="normal">; </span>
        <em>
          <span class="normal">sin</span>θ <span class="vector">u</span><span class="normal">×</span><span class="vector">p</span>
          <span class="normal">+</span>
          <span class="normal">cos</span>θ <span class="vector">u</span>
          <span class="normal">+</span>
          <span class="paren" style="font-size:em;">(</span>
            <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
          <span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">(</span>
            <span class="vector">u</span>
            <span class="normal">⋅</span>
            <span class="vector">p</span>
          <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
        </em>
      <span class="paren" style="font-size:em;">)</span>
    </div>
となります。
この式のベクトル部ですが、先ほど説明した3次元ベクトルの回転の式と一致しています。
すなわち、絶対値 1 の四元数 <span class="math">Σ</span> を用意し、
<em>
        <span class="math">
          Σ<sup>*</sup> q Σ
        </span>
      </em> という計算をすることで、
3次元ベクトルの回転をすることができます。
