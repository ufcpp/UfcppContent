---
title: "電場と電束密度"
source_url: "https://ufcpp.net/study/physics/em/electro/"
content_type: "Article"
published_at: "2015-05-06T14:20:06"
updated_at: "2015-05-18T17:40:53"
tags: []
umbraco_id: 1563
parent_id: 1561
sort_order: 1
aliases:
  - "/em/electro"
  - "/em/electro.html"
  - "/physics/em/electro/"
  - "/study/em/electro"
  - "/study/em/electro.html"
---

# 電場と電束密度

## <a id="sec-generated-title-1"></a> <a id="electro"></a>電場と電束密度

電荷の作る場には、電場と電束密度という2つのものがあります。
どっちも似たようなものなのに、なぜ2つあるのかって疑問をもたれるかたもいるかと思います。
あと、似ているようで微妙に違ってて混乱したりも。
そういうわけでここでは<strong id="elefield" class="keyword">電場</strong>と<strong id="eledensity" class="keyword">電束密度</strong>について解説していきます。

<em>電場というのは、2つの電荷の間に働く力を近接作用で考える(つまり、電荷が存在するところには何らかの場が発生して、その場が他の電荷に力を加えると考える)際に利用する場</em>です。
すなわち、電荷の周りには電場が発生し、電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>の中に電荷<span class="math">q</span>をおくと、
<div class="math">
      <span class="vector">F</span> = q<a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
    </div>
という力が発生するするというものです。

一方で、<em>
        電束密度は自由電荷を湧出点とする「[流束](../../math/vector_analysis/v_field.md#flux)」
      </em>です。
つまり、自由電荷密度分布が<span class="math">
        <a href="variable.md#rho" title="電荷密度">ρ</a>
      </span>のとき、
<div class="math">
      <span class="vector">∇</span>・
      <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#rho" title="電荷密度">ρ</a>
    </div>
を満たすような場<span class="math">
        <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>
      </span>が電束密度です。

そして、この2つの量の間には比例関係が成り立ちますから、比例係数<span class="math">
        <a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a>
      </span>を定義して
<div class="math">
      <em>
        <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </em>
    </div>
という関係が成り立ちます。
ここで用いた比例係数<span class="math">
        <a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a>
      </span>を<em>
        真空中の<strong id="permittivity" class="keyword">誘電率</strong>
      </em>といいます。


## <a id="sec-generated-title-2"></a> <a id="dielectric"></a>誘電体

誘電体とは、電場をかけると分極を起こして分極電荷の現れる物質のことです。

<figure>

[![分極](../../../../assets/media/ufcpp2000/physics/electro4.png)](../../../../assets/media/ufcpp2000/physics/electro4.png)

<figcaption>分極</figcaption>
</figure>


分極とは、誘電体中に負電荷(電子)と正電荷(原子核)の位置が微妙にずれたもの(これを電気双極子という。左図参照。)が分布した状態のことを言います。
誘電体に電場をかけることで、原子核と電子の位置がずれ、このような状態が生じます。
このような、電気双極子の分布によって電荷の分布が生じます。
この分極によって生じる電荷分布を分極電荷といい、
分極の変化によって生じる電流密度分布を分極電流密度といいます。

電気双極子に対して、<strong id="moment" class="keyword">電気双極子モーメント</strong><span class="math">
        <a href="variable.md#dipole_e" title="電気双極子モーメント"><span class="vector">p</span></a>
      </span>というものを定義し、
<div class="math">
      <em>
        <a href="variable.md#dipole_e" title="電気双極子モーメント"><span class="vector">p</span></a> = q<span class="vector">d</span>
      </em>
    </div>
で表します。
ここで、<span class="math">q</span>は双極子を作る電荷の電気量で、<span class="math">
        <span class="vector">d</span>
      </span>は正負の電荷の間の位置の差を表すベクトルです。
なぜこの式で電気双極子を表すかというと、このような電気双極子に対して電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>をかけると、電気双極子を電場と同じ向きに向けようとするトルク(偶力)<span class="math">
        <span class="vector">T</span>
      </span>がはたらき、<em>
        <span class="math">
          <span class="vector">T</span> = <a href="variable.md#dipole_e" title="電気双極子モーメント"><span class="vector">p</span></a>×<a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
        </span>
      </em>と表せるからです。

次に、分極は電気双極子の密度ですから、電気双極子の分布密度を<span class="math">N</span>とすると、
<strong id="polarization" class="keyword">分極密度</strong><span class="math">
        <a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a>
      </span>は
<div class="math">
      <a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a> = N<a href="variable.md#dipole_e" title="電気双極子モーメント"><span class="vector">p</span></a>
    </div>
となります。

そして、分極によって生じる分極電荷<span class="math">
        <a href="variable.md#rho" title="電荷密度">ρ</a>
        <sub>p</sub>
      </span>は
<div class="math">
      <em>
        <a href="variable.md#rho" title="電荷密度">ρ</a>
        <sub>p</sub> = -<span class="vector">∇</span>・<a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a>
      </em>
    </div>
となり、分極の変化によって生じる分極電流密度<span class="math">
        <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
        <sub>p</sub>
      </span>は
<div class="math">
      <em>
        <a href="variable.md#J" title="電流密度ベクトル"><span class="vector">J</span></a>
        <sub>p</sub> = <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table><a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a>
      </em>
    </div>
となります。(電束密度と電荷、電流の関係とは符号が逆なのに注意。)


## <a id="sec-generated-title-3"></a> <a id="in_material"></a>誘電体中の電場・電束密度

電束密度<span class="math">
        <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a>
      </span>は自由電荷(つまり、誘電体に生じた分極電荷は考慮に入れない)から湧き出してくる流束です。
要するに、<em>電束密度は誘電体中でもその値は変わりません</em>。
また、誘電体に電場をかけると分極<span class="math">
        <a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a>
      </span>が生じます。
この様子を下図1、2に示します。

一方、電場<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </span>は、電荷にかかる正味の力をあらわすものですから、
誘電体に生じた分極の分まで考慮に入れる必要があります。
すなわち、<em>誘電体中では電場は変化し、</em>
<div class="math">
      <em>
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
          <a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a>
        </td></tr></table><span class="paren" style="font-size:em;">(</span>
          <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> − <a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a>
        <span class="paren" style="font-size:em;">)</span>
      </em>
    </div>
という関係が成り立ちます。
この様子を下図に示します。
<table class="layout" summary="レイアウト用テーブル">
<tr><td>
<figure>

[![電荷から生じる電束密度](../../../../assets/media/ufcpp2000/physics/electro1.png)](../../../../assets/media/ufcpp2000/physics/electro1.png)

<figcaption>電荷から生じる電束密度</figcaption>
</figure>

</td><td>
<figure>

[![誘電体をはさんだ場合](../../../../assets/media/ufcpp2000/physics/electro2.png)](../../../../assets/media/ufcpp2000/physics/electro2.png)

<figcaption>誘電体をはさんだ場合</figcaption>
</figure>

</td><td>
<figure>

[![同じく、電場](../../../../assets/media/ufcpp2000/physics/electro3.png)](../../../../assets/media/ufcpp2000/physics/electro3.png)

<figcaption>同じく、電場</figcaption>
</figure>

</td></tr></table>


さて、ここで分極密度が誘電体にかけた電場に等方線形時不変で比例すると仮定します(この仮定はどんな誘電体に対しても出来るわけではありませんが、たいていは出来るものとみなして大丈夫です)。
この仮定の元、<span class="math">
        <a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <a href="variable.md#chi_e" title="分極率">χ<sub>e</sub></a><a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a><a href="variable.md#P" title="分極ベクトル"><span class="vector">P</span></a>
      </span>と置くと、
<div class="math">
      <em>
        <a href="variable.md#D" title="電束密度ベクトル"><span class="vector">D</span></a> = <a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a><span class="paren" style="font-size:em;">(</span>
          1+<a href="variable.md#chi_e" title="分極率">χ<sub>e</sub></a>
        <span class="paren" style="font-size:em;">)</span><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a> = <a href="variable.md#eps" title="物質中の誘電率">ε</a><a href="variable.md#E" title="電場ベクトル"><span class="vector">E</span></a>
      </em>
    </div>
となります。
ここで用いた比例係数<span class="math">
        <a href="variable.md#chi_e" title="分極率">χ<sub>e</sub></a>
      </span>を<strong id="polarizability" class="keyword">分極率</strong>といい、<span class="math">
        <a href="variable.md#eps" title="物質中の誘電率">ε</a>
      </span>を(誘電体中の)<em>誘電率</em>といいます。
また、<span class="math">
        <a href="variable.md#eps_r" title="比誘電率">ε<sub>r</sub></a> = 1+<a href="variable.md#chi_e" title="分極率">χ<sub>e</sub></a>
      </span>というものを定義し(<span class="math">
        <a href="variable.md#eps" title="物質中の誘電率">ε</a> = <a href="variable.md#eps_r" title="比誘電率">ε<sub>r</sub></a><a href="variable.md#eps0" title="真空中の誘電率">ε<sub>0</sub></a>
      </span>)、これを<em>比誘電率</em>といいます。
