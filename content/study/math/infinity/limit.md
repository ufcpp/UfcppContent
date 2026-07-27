---
title: "極限"
source_url: "https://ufcpp.net/study/math/infinity/limit/"
content_type: "Article"
published_at: "2015-05-06T14:17:54"
updated_at: "2015-05-06T14:17:54"
tags: []
umbraco_id: 1502
parent_id: 1500
sort_order: 1
aliases:
  - "/study/infinity/limit.html"
---

# 極限

## <a id="sec-generated-title-1"></a> <a id="limit"></a>極限

数（整数や実数など）の範囲で考えれば、無限というものは存在しません。
どんなに大きな数であろうと所詮は有限の値です。
 
ですが、極限的な状況においては無限に大きな値になることも考えられます。
（「無限大という値」になるのではなく、あくまで「値が無限に大きくなっていく」という考え方ですが。）


## <a id="sec-generated-title-2"></a> <a id="unlimited"></a>限りなく

極限というのは、数列や関数に対する考え方で、
「整数 <span class="math">n</span> を限りなく大きくしたときに数列 <span class="math">a<sub>n</sub></span> の値がどうなるか」とか、
「実数 <span class="math">x</span> の値を限りなく <span class="math">a</span> に近づけたときに関数 <span class="math">f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> の値がどうなるか」というものです。
 
例えば、整数 <span class="math">n</span> を限りなく大きくしたとき、<span class="math">1 / n</span> は限りなく小さくなっていき、最終的には 0 に限りなく近づいていきます。
<span class="math">n</span> が整数である限り、どんなに値を大きくしたって <span class="math">1 / n</span> が 0 になることはありえないんですが、「限りなく」という言葉をつける場合には <span class="math">n</span> の値を無限に大きくしたものと考えて、<span class="math">1 / n</span> の値も 0 になるものと考えます。
このことを、「0 に収束する」といい、以下のように書き表します。
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">n → ∞</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n</td></tr></table> ＝ 0
</div>
ここで、「∞」という文字が出てきますが、
これは「無限大という数」という意味ではなく、「限りなく大きくする」という操作を表す抽象的な記号だと思ってください。
 
また、次のような例も考えられます。
実数 <span class="math">x</span> を限りなく <span class="math">1</span> に近づけるとき、
関数 <span class="math">f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> ＝ 1 / <span class="paren" style="font-size:em;">(</span>x － 1<span class="paren" style="font-size:em;">)</span><sup>2</sup></span> は限りなく値が大きくなります。
<span class="math">x ＝ 1</span> のときには、0 で割ってはいけないというルールから、<span class="math">f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> の値は定義できないものになってしまいますが、「限りなく近づく」という言い方をすることで、この値を無理やり考えてみようということです。
このことを、「＋∞に発散する」といい、以下のように書きます。
<div class="math">
      <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x → 1</td></tr></table>
      <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>
        <span class="paren" style="font-size:em;">(</span>x － 1<span class="paren" style="font-size:em;">)</span>
        <sup>2</sup>
      </td></tr></table> → ＋∞
</div>
この「∞」も、「値が限りなく大きくなる」という意味です。
何らかの値を示すものではなく、あくまで抽象的概念なので、<span class="math">＝ ∞</span> というように ＝ を使うことは出来ません。
言葉の上でも、「収束」（値が確定する）ではなく「発散」（確定しない）といいます。


## <a id="sec-generated-title-3"></a> <a id="indefinite"></a>∞×0 ＝ ？

とりあえず、極限的な状況下では∞という概念が考えられるわけですが、
±∞に発散するものと、有限の値に収束するものを掛けるとやはり±∞（符号は逆なることもあり）になります。
この性質は 0 に似ています（0 には何を掛けてもやはり 0 になる）。
 
さて、ここまではなんとなく直感的に納得できる話だと思うんですが、
ここからが問題です。
何を掛けても 0 になるものと、何を掛けても∞になるもの、これらを掛けるとどうなるのでしょう。
文字通り矛盾してると思いませんか？
最強の盾（0）と最強の矛（∞）をぶつけるようなもので。
 
こう言うことを矛盾せずにちゃんと定義できるところが数学の面白いところです。
結論から言うと、極限論的な∞や 0 というものには「強さ」があって、強い方の値になります。
（強さが同じなら有限確定値に収束します。）
同じ∞でも、
<span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x</span> と
<span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x<sup>2</sup></span> では後者の方が強力です。
また、
<span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x</span> と
<span class="math"><table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> 2x</span> は強さは同じなんですが、値的には後者の方が2倍の値ということになります。
<div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x</td></tr><tr><td>
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x<sup>2</sup></td></tr></table>
＝ 0
</div><div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x<sup>2</sup></td></tr><tr><td>
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x</td></tr></table>
→ ∞
</div><div class="math">
      <table class="frac" summary="fraction"><tr><td class="num">
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> 2x</td></tr><tr><td>
          <table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x→∞</td></tr></table> x</td></tr></table>
＝ 2
</div>

## <a id="sec-generated-title-4"></a> <a id="d68e214"></a>まとめ

∞は極限的な状況下でのみ存在する概念的なもの。
∞や0にも強さがある。
