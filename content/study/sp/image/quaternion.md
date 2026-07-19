---
title: "四元数と3次元空間中の回転"
source_url: "https://ufcpp.net/study/sp/image/quaternion/"
content_type: "Article"
published_at: "2008-08-17T00:00:00"
updated_at: "2015-05-06T14:23:00"
tags: []
umbraco_id: 1624
parent_id: 1623
sort_order: 0
aliases:
  - "/image/quaternion"
  - "/image/quaternion.html"
  - "/sp/image/quaternion/"
  - "/study/image/quaternion"
  - "/study/image/quaternion.html"
---

# 四元数と3次元空間中の回転

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

3次元空間中での回転は、回転軸と回転角で表すことができます。
実用上、回転軸
<span class="math">
        <span class="vector">p</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          p<sub>x</sub> ,
          p<sub>y</sub> ,
          p<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>
と回転角
<span class="math">θ</span>
は、以下のような形式に変換して置くことが多いです。
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>x</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>y</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>z</sub> 
      <span class="paren" style="font-size:em;">)</span>
    </div>
これを、以下のように表したりもします。
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        <span class="normal">; </span>
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
    </div>
この形式を四元数といいます。


## <a id="sec-generated-title-2"></a> <a id="quaternion"></a>四元数

四元数なんていうたいそうな名前が付いているは、
元をただせば数学的な意味があるからなんですが、
四元数の数学的意味は、画像処理・3D CG の分野では大して役に立ちません。

応用上は、元々の数学的な意味を知る必要はあまりなく、
回転軸
<span class="math">
        <span class="vector">p</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          p<sub>x</sub> ,
          p<sub>y</sub> ,
          p<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>
と回転角
<span class="math">θ</span>
の情報を、
4次元の単位ベクトルとして、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        <span class="normal">; </span>
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>x</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>y</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>z</sub> 
      <span class="paren" style="font-size:em;">)</span>
    </div>
という形式で持っているものとだけ覚えておけば十分。


## <a id="sec-generated-title-3"></a> <a id="rotation"></a>3次元空間上の回転

まず、3次元空間上の回転を定式化します。

3次元空間上の回転を表すためには、回転軸ベクトル <span class="math">
        <span class="vector">p</span>
      </span> と回転角度 <span class="math">θ</span>が必要になります。
回転軸ベクトルの絶対値は意味を持たないので、<span class="math">
        <span class="normal">|</span>
          <span class="vector">p</span>
        <span class="normal">|</span>
        <span class="normal">=</span>
        <span class="normal">1</span>
      </span> であるものとしてます。

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
          <span class="vector">u</span>
          <span class="normal">⋅</span>
          <span class="vector">p</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">+</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">u</span>
        <span class="normal">⋅</span>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span><span class="vector">p</span>
    </div><div class="math">
      <em>
        <span class="normal">=</span>
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
ちなみに、この式の導出の仕方ですが、下図のようになります。

<figure>
	[![3次元ベクトルの回転](../../../../assets/media/ufcpp2000/sp/rotation3d.png)](../../../../assets/media/ufcpp2000/sp/rotation3d.png)
	<figcaption>3次元ベクトルの回転</figcaption>
</figure>


原点を O、点 A から回転軸におろした垂線の足を H とすると、
<div class="math">
      <Vec>OH</Vec>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">u</span>
        <span class="normal">⋅</span>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="vector">p</span>
    </div><div class="math">
      <Vec>HA</Vec>
      <span class="normal">=</span>
      <span class="vector">u</span>
      <span class="normal">−</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">u</span>
        <span class="normal">⋅</span>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="vector">p</span>
    </div>
となります。
また <span class="math">
        <span class="vector">u</span>
        <span class="normal">×</span>
        <span class="vector">p</span>
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
      <Vec>HA'</Vec> <span class="normal">=</span> <span class="normal">cos</span>θ <Vec>HA</Vec> <span class="normal">+</span> <span class="normal">sin</span>θ <span class="vector">u</span><span class="normal">×</span><span class="vector">p</span>
    </div><div class="math">
      <span class="normal">=</span>
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
    </div>
と表すことができるので、先ほど示した式を導き出すことができます。


## <a id="sec-generated-title-4"></a> <a id="matrix"></a>行列を使った3次元空間上の回転

前節で示した式で3次元空間中の回転が表現できるんですが、
実際には、この式をそのまま使うのではなく、行列演算に変形してから使います。
(行列にすることで、拡大縮小や平行移動などと一緒に、まとめて扱えるため。）

3次元ベクトルの外積計算は以下のように、
<div class="math">
      <span class="vector">p</span>
      <span class="normal">×</span>
      <span class="vector">u</span>

      <span class="normal">=</span>

      <span class="vector">u</span>
      <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
            <span class="normal">0</span>
          </td><td>
            <span class="normal">−</span>p<sub>z</sub>
          </td><td>
            p<sub>y</sub>
          </td></tr><tr><td>
            p<sub>z</sub>
          </td><td>
            <span class="normal">0</span>
          </td><td>
            <span class="normal">−</span>p<sub>x</sub>
          </td></tr><tr><td>
            <span class="normal">−</span>p<sub>y</sub>
          </td><td>
            p<sub>x</sub>
          </td><td>
            <span class="normal">0</span>
          </td></tr></table><span class="paren" style="font-size:4em;">]</span>
    </div>
行列とベクトルの積としても書き表わせます。
（ただし、ベクトルは横ベクトルを仮定しています。）

また、
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="vector">u</span>
          <span class="normal">⋅</span>
          <span class="vector">p</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="vector">p</span>
      </span>
という部分も、
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        <span class="vector">u</span>
        <span class="normal">⋅</span>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="vector">p</span>

      <span class="normal">=</span>

      <span class="vector">u</span>
      <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
              p<sub>x</sub><sup><span class="normal">2</span></sup>  
          </td><td>
              p<sub>x</sub>   p<sub>y</sub>  
          </td><td>
              p<sub>z</sub>   p<sub>x</sub>  
          </td></tr><tr><td>
              p<sub>x</sub>   p<sub>y</sub>  
          </td><td>
              p<sub>y</sub><sup><span class="normal">2</span></sup>  
          </td><td>
              p<sub>y</sub>   p<sub>z</sub>  
          </td></tr><tr><td>
              p<sub>z</sub>   p<sub>x</sub>  
          </td><td>
              p<sub>y</sub>   p<sub>z</sub>  
          </td><td>
              p<sub>z</sub><sup><span class="normal">2</span></sup>  
          </td></tr></table><span class="paren" style="font-size:4em;">]</span>
    </div>
と表せます。

これらを使うと、
先ほどの回転の式は、以下のような行列で表すことができます。
<div class="math">
      <span class="vector">u</span>'
      <span class="normal">=</span>
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
    </div><div class="math">
      <span class="normal">=</span>
      <span class="normal">cos</span>θ
      <span class="vector">u</span>

      <span class="normal">+</span>

      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
      <span class="paren" style="font-size:em;">)</span>
      <span class="vector">u</span>
      <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
              p<sub>x</sub><sup><span class="normal">2</span></sup>  
          </td><td>
              p<sub>x</sub>   p<sub>y</sub>  
          </td><td>
              p<sub>z</sub>   p<sub>x</sub>  
          </td></tr><tr><td>
              p<sub>x</sub>   p<sub>y</sub>  
          </td><td>
              p<sub>y</sub><sup><span class="normal">2</span></sup>  
          </td><td>
              p<sub>y</sub>   p<sub>z</sub>  
          </td></tr><tr><td>
              p<sub>z</sub>   p<sub>x</sub>  
          </td><td>
              p<sub>y</sub>   p<sub>z</sub>  
          </td><td>
              p<sub>z</sub><sup><span class="normal">2</span></sup>  
          </td></tr></table><span class="paren" style="font-size:4em;">]</span>

      <span class="normal">+</span>

      <span class="normal">sin</span>θ
      <span class="vector">u</span>
      <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
            <span class="normal">0</span>
          </td><td>
            <span class="normal">−</span>p<sub>z</sub>
          </td><td>
            p<sub>y</sub>
          </td></tr><tr><td>
            p<sub>z</sub>
          </td><td>
            <span class="normal">0</span>
          </td><td>
            <span class="normal">−</span>p<sub>x</sub>
          </td></tr><tr><td>
            <span class="normal">−</span>p<sub>y</sub>
          </td><td>
            p<sub>x</sub>
          </td><td>
            <span class="normal">0</span>
          </td></tr></table><span class="paren" style="font-size:4em;">]</span>
    </div><div class="math">
      <span class="normal">=</span>
      <span class="vector">u</span>

      <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>x</sub><sup><span class="normal">2</span></sup>

            <span class="normal">+</span>
            <span class="normal">cos</span>θ
          </td><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>x</sub>   p<sub>y</sub>

            <span class="normal">−</span>
            p<sub>z</sub>
            <span class="normal">sin</span>θ
          </td><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>z</sub>   p<sub>x</sub>

            <span class="normal">+</span>
            p<sub>y</sub>
            <span class="normal">sin</span>θ
          </td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>x</sub>   p<sub>y</sub>

            <span class="normal">+</span>
            p<sub>z</sub>
            <span class="normal">sin</span>θ
          </td><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>y</sub><sup><span class="normal">2</span></sup>

            <span class="normal">+</span>
            <span class="normal">cos</span>θ
          </td><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>y</sub>   p<sub>z</sub>

            <span class="normal">−</span>
            p<sub>x</sub>
            <span class="normal">sin</span>θ
          </td></tr><tr><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>z</sub>   p<sub>x</sub>

            <span class="normal">−</span>
            p<sub>y</sub>
            <span class="normal">sin</span>θ
          </td><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>y</sub>   p<sub>z</sub>

            <span class="normal">+</span>
            p<sub>x</sub>
            <span class="normal">sin</span>θ
          </td><td>
            <span class="paren" style="font-size:em;">(</span>
              <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
            <span class="paren" style="font-size:em;">)</span>
            p<sub>z</sub><sup><span class="normal">2</span></sup>

            <span class="normal">+</span>
            <span class="normal">cos</span>θ
          </td></tr></table><span class="paren" style="font-size:4em;">]</span>
    </div>

## <a id="sec-generated-title-5"></a> <a id="q_to_m"></a>四元数から回転行列を計算

ここで話を四元数に戻します。

四元数は、
回転軸
<span class="math">
        <span class="vector">p</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          p<sub>x</sub> ,
          p<sub>y</sub> ,
          p<sub>z</sub>
        <span class="paren" style="font-size:em;">)</span>
      </span>
と回転角
<span class="math">θ</span>
の情報を、以下の形式で保持するものです。
<div class="math">
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        <span class="normal">; </span>
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        <span class="vector">p</span>
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>x</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>y</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>z</sub> 
      <span class="paren" style="font-size:em;">)</span>
    </div>
ここで、sin や cos を何度も書きたくないので、
以下のように書き表わすことにします。
<div class="math">
      <span class="vector">q</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        q<sub>w</sub> ,
        q<sub>x</sub> ,
        q<sub>y</sub> ,
        q<sub>z</sub> ,
      <span class="paren" style="font-size:em;">)</span>
      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">cos</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>x</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>y</sub> ,
        <span class="normal">sin</span>
        <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
            <span class="normal">2</span>
          </td></tr></table>
        p<sub>z</sub> 
      <span class="paren" style="font-size:em;">)</span>
    </div>
ここで、三角関数の倍角の公式を使うと、以下の等式が導かれます。
（* と † の位置には、<span class="math">x, y, z</span> のいずれかが入ります。）
<div class="math">
      <span class="normal">2</span>
      q<sub>*</sub>
      q<sub>
        <span class="normal">†</span>
      </sub>

      <span class="normal">=</span>

      <span class="normal">2</span>
      <span class="normal">sin</span><sup><span class="normal">2</span></sup>
      <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table>
      p<sub>*</sub>
      p<sub>
        <span class="normal">†</span>
      </sub>

      <span class="normal">=</span>
      <span class="paren" style="font-size:em;">(</span>
        <span class="normal">1</span> <span class="normal">−</span> <span class="normal">cos</span>θ
      <span class="paren" style="font-size:em;">)</span>
      p<sub>*</sub>
      p<sub>
        <span class="normal">†</span>
      </sub>
    </div><div class="math">
      <span class="normal">2</span>
      q<sub>w</sub>
      q<sub>*</sub>

      <span class="normal">=</span>

      <span class="normal">2</span>
      <span class="normal">sin</span>
      <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table>
      <span class="normal">cos</span>
      <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table>
      p<sub>*</sub>

      <span class="normal">=</span>
      <span class="normal">sin</span>θ
      p<sub>*</sub>
    </div><div class="math">
      <span class="normal">2</span>
      q<sub>w</sub><sup><span class="normal">2</span></sup>

      <span class="normal">=</span>

      <span class="normal">2</span>
      <span class="normal">cos</span><sup><span class="normal">2</span></sup>
      <table class="frac" summary="fraction"><tr><td class="num">θ</td></tr><tr><td>
          <span class="normal">2</span>
        </td></tr></table>

      <span class="normal">=</span>
      <span class="normal">1</span>
      <span class="normal">+</span>
      <span class="normal">cos</span>θ
    </div>
これらを、前節で求めた行列に代入すると、
以下の結果が得られます。
<div class="math">
      <span class="vector">u</span>'
      <span class="normal">=</span>
      <span class="vector">u</span>

      <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>x</sub><sup><span class="normal">2</span></sup>
              <span class="normal">+</span>
              q<sub>w</sub><sup><span class="normal">2</span></sup>
            <span class="paren" style="font-size:em;">)</span>

            <span class="normal">−</span>
            <span class="normal">1</span>
          </td><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>x</sub>
               
              q<sub>y</sub>
              <span class="normal">−</span>
              q<sub>z</sub>
               
              q<sub>w</sub>
            <span class="paren" style="font-size:em;">)</span>
          </td><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>x</sub>
               
              q<sub>z</sub>
              <span class="normal">+</span>
              q<sub>y</sub>
               
              q<sub>w</sub>
            <span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>x</sub>
               
              q<sub>y</sub>
              <span class="normal">+</span>
              q<sub>z</sub>
               
              q<sub>w</sub>
            <span class="paren" style="font-size:em;">)</span>
          </td><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>y</sub><sup><span class="normal">2</span></sup>
              <span class="normal">+</span>
              q<sub>w</sub><sup><span class="normal">2</span></sup>
            <span class="paren" style="font-size:em;">)</span>

            <span class="normal">−</span>
            <span class="normal">1</span>
          </td><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>y</sub>
               
              q<sub>z</sub>
              <span class="normal">−</span>
              q<sub>x</sub>
               
              q<sub>w</sub>
            <span class="paren" style="font-size:em;">)</span>
          </td></tr><tr><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>x</sub>
               
              q<sub>y</sub>
              <span class="normal">−</span>
              q<sub>y</sub>
               
              q<sub>w</sub>
            <span class="paren" style="font-size:em;">)</span>
          </td><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>y</sub>
               
              q<sub>z</sub>
              <span class="normal">+</span>
              q<sub>x</sub>
               
              q<sub>w</sub>
            <span class="paren" style="font-size:em;">)</span>
          </td><td>
            <span class="normal">2</span>
            <span class="paren" style="font-size:em;">(</span>
              q<sub>z</sub><sup><span class="normal">2</span></sup>
              <span class="normal">+</span>
              q<sub>w</sub><sup><span class="normal">2</span></sup>
            <span class="paren" style="font-size:em;">)</span>

            <span class="normal">−</span>
            <span class="normal">1</span>
          </td></tr></table><span class="paren" style="font-size:4em;">]</span>
    </div>
あるいは、
<span class="math">
        <span class="normal">|</span>
          <span class="vector">q</span>
        <span class="normal">|</span>
        <span class="normal">=</span>
        <span class="normal">1</span>
      </span>
なことを利用して、以下のようにも書けます。
<div class="math">
      <span class="vector">u</span>'
      <span class="normal">=</span>
      <span class="vector">u</span>
      <em>
        <span class="paren" style="font-size:4em;">[</span><table class="matrix" summary="matrix"><tr><td>
              <span class="normal">1</span>
              <span class="normal">−</span>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>y</sub><sup><span class="normal">2</span></sup>
                <span class="normal">+</span>
                q<sub>z</sub><sup><span class="normal">2</span></sup>
              <span class="paren" style="font-size:em;">)</span>
            </td><td>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>x</sub>
                 
                q<sub>y</sub>
                <span class="normal">−</span>
                q<sub>z</sub>
                 
                q<sub>w</sub>
              <span class="paren" style="font-size:em;">)</span>
            </td><td>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>x</sub>
                 
                q<sub>z</sub>
                <span class="normal">+</span>
                q<sub>y</sub>
                 
                q<sub>w</sub>
              <span class="paren" style="font-size:em;">)</span>
            </td></tr><tr><td>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>x</sub>
                 
                q<sub>y</sub>
                <span class="normal">+</span>
                q<sub>z</sub>
                 
                q<sub>w</sub>
              <span class="paren" style="font-size:em;">)</span>
            </td><td>
              <span class="normal">1</span>
              <span class="normal">−</span>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>z</sub><sup><span class="normal">2</span></sup>
                <span class="normal">+</span>
                q<sub>x</sub><sup><span class="normal">2</span></sup>
              <span class="paren" style="font-size:em;">)</span>
            </td><td>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>y</sub>
                 
                q<sub>z</sub>
                <span class="normal">−</span>
                q<sub>x</sub>
                 
                q<sub>w</sub>
              <span class="paren" style="font-size:em;">)</span>
            </td></tr><tr><td>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>x</sub>
                 
                q<sub>y</sub>
                <span class="normal">−</span>
                q<sub>y</sub>
                 
                q<sub>w</sub>
              <span class="paren" style="font-size:em;">)</span>
            </td><td>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>y</sub>
                 
                q<sub>z</sub>
                <span class="normal">+</span>
                q<sub>x</sub>
                 
                q<sub>w</sub>
              <span class="paren" style="font-size:em;">)</span>
            </td><td>
              <span class="normal">1</span>
              <span class="normal">−</span>
              <span class="normal">2</span>
              <span class="paren" style="font-size:em;">(</span>
                q<sub>x</sub><sup><span class="normal">2</span></sup>
                <span class="normal">+</span>
                q<sub>y</sub><sup><span class="normal">2</span></sup>
              <span class="paren" style="font-size:em;">)</span>
            </td></tr></table><span class="paren" style="font-size:4em;">]</span>
      </em>
    </div>
ちなみに、こうやって作られた行列は、必ず直交行列になります。
（実際、計算してみてもらうと、各列が正規直行していることがわかります。）
回転しかしていないんだから、長さも角度も変わらない変換（＝ 直交行列による1次変換）になっていないとおかしいはずです。
