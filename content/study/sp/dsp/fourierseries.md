---
title: "フーリエ級数展開"
source_url: "https://ufcpp.net/study/sp/dsp/fourierseries/"
content_type: "Article"
published_at: "2015-05-06T14:21:48"
updated_at: "2015-05-06T14:21:48"
tags: []
umbraco_id: 1600
parent_id: 1599
sort_order: 0
aliases:
  - "/dsp/fourierseries"
  - "/dsp/fourierseries.html"
  - "/sp/dsp/fourierseries/"
  - "/study/dsp/fourierseries"
  - "/study/dsp/fourierseries.html"
---

# フーリエ級数展開

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

フーリエ級数展開の基本となる概念は19世紀の前半にフランスの数学者 フーリエ（Fourier、1764-1830）が熱伝導問題の解析の過程で考え出したものです。
そして、その基本アイディアは「<em>任意の周期関数は三角関数の和で表される</em>」というものです。
 
フーリエ級数展開（および、フーリエ変換）について詳細に説明しようとすると、それだけで本が1冊書けるほどになってしまいます。
そのため、ディジタル信号処理などの工学的な応用に必要になる部分に絞って説明していきたいと思います。


## <a id="sec-generated-title-2"></a> <a id="idea"></a>基本アイディア

フーリエは「任意の周期関数は三角関数の和で表される」という仮定の下で、
周期関数を三角関数を使って級数展開する方法(<strong id="f-series" class="keyword">フーリエ級数展開</strong>と呼ばれています)を考案しました。
すなわち、周期<span class="math">T</span>の関数<span class="math">f(t)</span>は
<div class="math">
f(t) ＝ 
a<sub>0</sub> ＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=1</td></tr></table><span class="paren" style="font-size:2em;">(</span>
 a<sub>n</sub><span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table> ＋
 b<sub>n</sub><span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
というように、三角関数の和で表すことができると主張し、
係数<span class="math">a<sub>n</sub>, b<sub>n</sub></span>を求める方法を導き出したわけです。
 
もちろん、厳密には「任意の周期関数は三角関数の和で表される」という仮定が正しいかどうかをまず議論する必要がありますが、この議論には少し難しい知識が必要とされます。
一方、厳密な議論は後回しにして、とりあえずこの仮定が正しいとした上で話を進めるなら、高校レベルの知識でも十分に理解できます。
また、工学的な応用に用いる限りには厳密な議論は後回しにしても全く差し支えありません。
 
実際、歴史的にも、厳密な議論よりも物理学への応用が先になされ、
その後から「任意の周期関数は三角関数の和で表される」という仮定に関する厳密な議論が行なわれました。
 
以上のことから、ここでは厳密な議論は抜きにして（知りたい人は専門書を読んで自分で勉強してもらうものとして）説明していきます。


## <a id="sec-generated-title-3"></a> <a id="orthogonal"></a>三角関数の直交性

三角関数の性質として、任意の自然数<span class="math">m, n</span>に対して以下の式が成り立つというものがあります。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
      <span class="normal">sin</span>
      <span class="paren" style="font-size:em;">(</span>mt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">d</span>t ＝ 0
</div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
      <span class="normal">cos</span>
      <span class="paren" style="font-size:em;">(</span>mt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">d</span>t ＝ 0
</div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
      <span class="normal">sin</span>
      <span class="paren" style="font-size:em;">(</span>mt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">sin</span>
      <span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">d</span>t ＝ 
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">π</span>  </td><td><span class="paren">(</span><span class="math">m ＝ n のとき</span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math">m ≠ n のとき</span><span class="paren">)</span></td></tr></table></div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
      <span class="normal">cos</span>
      <span class="paren" style="font-size:em;">(</span>mt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">cos</span>
      <span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">d</span>t ＝ 
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">π</span>  </td><td><span class="paren">(</span><span class="math">m ＝ n のとき</span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math">m ≠ n のとき</span><span class="paren">)</span></td></tr></table></div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
      <span class="normal">sin</span>
      <span class="paren" style="font-size:em;">(</span>mt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">cos</span>
      <span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span>
      <span class="normal">d</span>t ＝ 0
</div>
すなわち、三角関数の積分値は、
<span class="math"><span class="normal">sin</span></span>どうし、または<span class="math"><span class="normal">cos</span></span>どうしを掛けた物で、
<span class="math">m ＝ n</span>の場合にのみ非0となり、
その他の場合には必ず0になります。
このような性質は<em>三角関数の直交性</em>と呼ばれています。


## <a id="sec-generated-title-4"></a> <a id="series"></a>フーリエ級数展開

説明を単純化するため、まずは周期<span class="math">2π</span>の関数に絞って説明していきたいと思います。
このとき、「[基本アイディア](#idea)」で示した式は以下のようになります。
<div class="math">
f(t) ＝ 
a<sub>0</sub> ＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=1</td></tr></table><span class="paren" style="font-size:1em;">(</span>
 a<sub>n</sub><span class="normal">cos</span> nt ＋
 b<sub>n</sub><span class="normal">sin</span> nt
<span class="paren" style="font-size:1em;">)</span></div>
「[三角関数の直交性](#orthogonal)」で示した式から、この両辺を<span class="math">－π～π</span>の範囲で積分すると、<span class="math">a<sub>0</sub></span>の項だけが残ります。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> a<sub>0</sub><span class="normal">d</span>t ＝ 2π a<sub>0</sub></div>
同様に、
両辺に<span class="math"><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span></span> を掛けてから積分すると<span class="math">a<sub>m</sub></span>の項だけが、
<span class="math"><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span></span> を掛けてから積分すると<span class="math">b<sub>m</sub></span>の項だけがのこります。
<div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> a<sub>m</sub><span class="normal">cos</span><sup>2</sup><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝ π a<sub>n</sub></div><div class="math">
      <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> b<sub>m</sub><span class="normal">sin</span><sup>2</sup><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t ＝ π b<sub>n</sub></div>
したがって、以下の計算式で係数<span class="math">a<sub>n</sub>, b<sub>n</sub></span>を計算できます。
<div class="math">
a<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div><div class="math">
a<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div><div class="math">
b<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div>
このようにして得られた級数
<span class="math">
f(t) = 
a<sub>0</sub> ＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=1</td></tr></table><span class="paren" style="font-size:1em;">(</span>
 a<sub>n</sub><span class="normal">cos</span> t ＋
 b<sub>n</sub><span class="normal">sin</span> t
<span class="paren" style="font-size:1em;">)</span></span>
をフーリエ級数、係数<span class="math">a<sub>n</sub>, b<sub>n</sub></span>をフーリエ係数などといいます。
また、このように、周期関数をフーリエ級数に展開することをフーリエ級数展開といいます。
 
周期<span class="math">T</span>が<span class="math">2π</span>以外の関数に関しては、変数<span class="math">t</span>を<span class="math"><table class="frac" summary="fraction"><tr><td class="num">2πt</td></tr><tr><td>T</td></tr></table></span>で置き換えることにより、
<em>
      <div class="math">
a<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div>
      <div class="math">
a<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2</td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">d</span>t
</div>
      <div class="math">
b<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">2</td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">sin</span><span class="paren" style="font-size:2em;">(</span><table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">d</span>t
</div>
      <div class="math">
f(t) ＝ 
a<sub>0</sub> ＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=1</td></tr></table><span class="paren" style="font-size:2em;">(</span>
 a<sub>n</sub><span class="normal">cos</span><table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table> ＋
 b<sub>n</sub><span class="normal">sin</span><table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
    </em>
となる。


## <a id="sec-generated-title-5"></a> <a id="d18e647"></a>複素形フーリエ級数展開

三角関数と指数関数の間には、
<div class="math">
      <span class="normal">e</span>
      <sup>ix</sup> ＝ <span class="normal">cos</span>x ＋ i <span class="normal">sin</span>x
</div><div class="math">
      <span class="normal">cos</span>x ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">e</span><sup>ix</sup> ＋ <span class="normal">e</span><sup>－ix</sup></td></tr><tr><td>2</td></tr></table></div><div class="math">
      <span class="normal">sin</span>x ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">e</span><sup>ix</sup> － <span class="normal">e</span><sup>－ix</sup></td></tr><tr><td>2i</td></tr></table></div>
という関係式があります。
この関係式を用いて、先ほどのフーリエ級数展開の式を以下のように書き換えることが出来ます。
<div class="math">
c<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－i<table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">d</span>t
</div><div class="math">
f(t) ＝ 
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
 c<sub>n</sub><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span> i<table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
この式を<strong id="complexfourier" class="keyword">複素形フーリエ級数展開</strong>、係数<span class="math">c<sub>n</sub></span>を複素フーリエ係数などと呼びます。
(フーリエ級数展開という呼称で複素形の方をさす場合もあります。)
複素形では、複素数が出てきてしまう代わりに、式をシンプルに書き表すことが出来ます。
 
ちなみに、この係数<span class="math">c<sub>n</sub></span>と先ほどの係数<span class="math">a<sub>n</sub>, b<sub>n</sub></span>との間には、以下のような関係が成り立っています。
<div class="math">
c<sub>0</sub> ＝ a<sub>0</sub></div><div class="math">
c<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">a<sub>n</sub> － i b<sub>n</sub></td></tr><tr><td>2</td></tr></table></div><div class="math">
c<sub>－n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num">a<sub>n</sub> ＋ i b<sub>n</sub></td></tr><tr><td>2</td></tr></table></div>
また、この係数<span class="math">c<sub>n</sub></span>を、整数から複素数への写像(離散関数)とみなして<span class="math">F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></span>と書き表すこともあります。
<div class="math">
F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span> ＝ <table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－i<table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">d</span>t
</div><div class="math">
f(t) ＝ 
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
 F<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span> i<table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span></div>
以後、特に断りのない限り、
<span class="math">f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span></span>のように()付き表記の関数は連続関数を、
<span class="math">f<span class="paren" style="font-size:em;">[</span>n<span class="paren" style="font-size:em;">]</span></span>のように[]付き表記の関数は離散関数を表すものとします。


## <a id="sec-generated-title-6"></a> <a id="exsample"></a>フーリエ級数展開の例

いくつか、フーリエ級数展開の例を挙げます。


### <a id="sec-generated-title-7"></a> <a id="d18e897"></a>矩形波

以下のような周期関数のフーリエ変換を考えてみましょう。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">－1</span>  </td><td><span class="paren">(</span><span class="math">π≦x＜0</span><span class="paren">)</span></td></tr><tr><td><span class="math">1</span>  </td><td><span class="paren">(</span><span class="math">0≦x＜π</span><span class="paren">)</span></td></tr></table>
,
f(t＋2π) ＝ f(t)
</div>
この周期関数で表されるような信号は（周期πの）矩形波と呼ばれ、下図のような波形を示します。

<figure>

[![矩形波の波形](../../../../assets/media/ufcpp2000/sp/fourier02.png)](../../../../assets/media/ufcpp2000/sp/fourier02.png)

<figcaption>矩形波の波形</figcaption>
</figure>


矩形波のフーリエ係数は以下のようになります。
<div class="math">
a<sub>n</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
＝ 0
</div><div class="math">
b<sub>n</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
＝
<table class="frac" summary="fraction"><tr><td class="num">2</td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">0</td></tr></table><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
＝
<span class="paren" style="font-size:3em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math"><table class="frac" summary="fraction"><tr><td class="num">4</td></tr><tr><td>π n</td></tr></table></span>  </td><td><span class="paren">(</span><span class="math">n<span class="normal">が奇数</span></span><span class="paren">)</span></td></tr><tr><td><span class="math">0</span>  </td><td><span class="paren">(</span><span class="math">n<span class="normal">が偶数</span></span><span class="paren">)</span></td></tr></table></div>
したがって、矩形波のフーリエ級数展開は以下のようになります。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
<table class="frac" summary="fraction"><tr><td class="num">4</td></tr><tr><td>π</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2k ＋ 1</td></tr></table><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>2k ＋ 1<span class="paren" style="font-size:em;">)</span>t
</div>
この式は無限級数になっていますが、
実用上は級数を途中までで打ち切って近似式として利用します（フーリエ級数近似）。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
≒
<table class="frac" summary="fraction"><tr><td class="num">4</td></tr><tr><td>π</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">K</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">k＝0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2k ＋ 1</td></tr></table><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>2k ＋ 1<span class="paren" style="font-size:em;">)</span>t
</div>
<span class="math">K</span> の値が大きいほど近似の精度は高くなりますが、
計算手間が増大します。
以下に<span class="math">K ＝ 0, 1, 3, 7, 15</span>の場合のフーリエ級数近似の1周期分のグラフを示します。

<figure>

[![矩形波のフーリエ級数近似](../../../../assets/media/ufcpp2000/sp/fourier03.png)](../../../../assets/media/ufcpp2000/sp/fourier03.png)

<figcaption>矩形波のフーリエ級数近似</figcaption>
</figure>



### <a id="sec-generated-title-8"></a> <a id="d18e1077"></a>鋸波

以下の周期関数で表される信号を（周期πの）鋸（のこぎり）波と呼びます。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
t,
f(t＋2π) ＝ f(t)
</div>
<figure>

[![鋸波の波形](../../../../assets/media/ufcpp2000/sp/fourier04.png)](../../../../assets/media/ufcpp2000/sp/fourier04.png)

<figcaption>鋸波の波形</figcaption>
</figure>


鋸波のフーリエ係数は以下のようになります。
<div class="math">
a<sub>n</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
t
<span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
＝ 0
</div><div class="math">
b<sub>n</sub>
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table>
t
<span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
＝
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="paren" style="font-size:2em;">[</span>
－x
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nx<span class="paren" style="font-size:em;">)</span></td></tr><tr><td>n</td></tr></table><span class="paren" style="font-size:2em;">]</span><table class="subsup" summary="sub / sup"><tr><td>π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>－π</td></tr></table>
＋
<table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nx<span class="paren" style="font-size:em;">)</span></td></tr><tr><td>n</td></tr></table><span class="normal">d</span>t
＝
<span class="paren" style="font-size:em;">(</span>－1<span class="paren" style="font-size:em;">)</span><sup>n＋1</sup><table class="frac" summary="fraction"><tr><td class="num">2</td></tr><tr><td>π</td></tr></table></div>
したがって、鋸波のフーリエ級数近似式は以下のようになります。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
≒
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝0</td></tr></table><span class="paren" style="font-size:em;">(</span>－1<span class="paren" style="font-size:em;">)</span><sup>n＋1</sup><table class="frac" summary="fraction"><tr><td class="num">2</td></tr><tr><td>n</td></tr></table><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span></div>
以下に<span class="math">N ＝ 1, 3, 7, 15, 31</span>の場合のフーリエ級数近似の1周期分のグラフを示します。

<figure>

[![矩形波のフーリエ級数近似](../../../../assets/media/ufcpp2000/sp/fourier05.png)](../../../../assets/media/ufcpp2000/sp/fourier05.png)

<figcaption>矩形波のフーリエ級数近似</figcaption>
</figure>



### <a id="sec-generated-title-9"></a> <a id="d18e1222"></a>インパルス列

以下の周期関数で表される信号を（周期πの）インパルス列と呼びます。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
＝
δ<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>,
f(t＋2π) ＝ f(t)
</div>
<figure>

[![インパルス列](../../../../assets/media/ufcpp2000/sp/fourier06.png)](../../../../assets/media/ufcpp2000/sp/fourier06.png)

<figcaption>インパルス列</figcaption>
</figure>


δ関数の性質から、インパルス列の複素形フーリエ係数は全て1となり、
フーリエ級数近似式は以下のようになります。
<div class="math">
f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
≒
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝－N</td></tr></table><span class="normal">exp</span><span class="paren" style="font-size:em;">(</span>－i n t<span class="paren" style="font-size:em;">)</span>
＝
1＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n＝0</td></tr></table>
2 <span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>n t<span class="paren" style="font-size:em;">)</span></div>
以下に<span class="math">N ＝ 1, 3, 7, 15, 31</span>の場合のフーリエ級数近似の1周期分のグラフを示します。

<figure>

[![矩形波のフーリエ級数近似](../../../../assets/media/ufcpp2000/sp/fourier07.png)](../../../../assets/media/ufcpp2000/sp/fourier07.png)

<figcaption>矩形波のフーリエ級数近似</figcaption>
</figure>



## <a id="sec-generated-title-10"></a> <a id="summay"></a>まとめ

<table summary="フーリエ級数展開の公式">
	<caption>
		フーリエ級数展開の公式
	</caption>
	<tr>
		<td markdown="1">フーリエ級数展開</td>
		<td markdown="1"><div class="math">
a<sub>0</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>2π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div><div class="math">
a<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">cos</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div><div class="math">
b<sub>n</sub> ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>π</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> π</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－π</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">sin</span><span class="paren" style="font-size:em;">(</span>nt<span class="paren" style="font-size:em;">)</span><span class="normal">d</span>t
</div><div class="math">
f(t) ＝ 
a<sub>0</sub> ＋
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=1</td></tr></table><span class="paren" style="font-size:1em;">(</span>
 a<sub>n</sub><span class="normal">cos</span> nt ＋
 b<sub>n</sub><span class="normal">sin</span> nt
<span class="paren" style="font-size:1em;">)</span></div></td>
	</tr>
	<tr>
		<td markdown="1">フーリエ級数展開（複素形）</td>
		<td markdown="1"><div class="math">
c<sub>n</sub> ＝
<table class="frac" summary="fraction"><tr><td class="num">1</td></tr><tr><td>T</td></tr></table><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> T/2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">－T/2</td></tr></table> f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span>－i<table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span><span class="normal">d</span>t
</div><div class="math">
f(t) ＝ 
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=－∞</td></tr></table>
 c<sub>n</sub><span class="normal">exp</span><span class="paren" style="font-size:2em;">(</span> i<table class="frac" summary="fraction"><tr><td class="num">2πnt</td></tr><tr><td>T</td></tr></table><span class="paren" style="font-size:2em;">)</span></div></td>
	</tr>
</table>
