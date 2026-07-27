---
title: "実数の0除算"
source_url: "https://ufcpp.net/study/math/infinity/div0/"
content_type: "Article"
published_at: "2015-05-06T14:17:52"
updated_at: "2015-05-06T14:17:52"
tags: []
umbraco_id: 1501
parent_id: 1500
sort_order: 0
aliases:
  - "/study/infinity/div0.html"
---

# 実数の0除算

## <a id="sec-generated-title-1"></a> <a id="d67e4"></a>0で割る

まず、実数の範囲で0で割るという操作を行ってはいけないということについて説明します。
 
「0で割ってはいけない」というのは、数学の知識がある程度ある人にとっては常識かと思います。
でも、「なぜ0で割ってはいけないのか」という質問への答え方となると常識とはいえないようで、
答えに詰まる人もちらほら見かけます。
 
この質問に答えるためには、そもそも割り算とは何なのかをしっかりと把握する必要があります。
割り算とは、掛け算の逆演算として定義されるものです。
すなわち、3つの実数 <span class="math">a, b, c</span> が <span class="math">a × c ＝ b</span> を満たすとき、
<span class="math">c ＝ b ÷ a</span> として割り算を定義します。
 
では、0 で割るということについて考えて見ましょう。
0 というのはかなり特殊な数で、任意の実数 <span class="math">c</span> に対して、
<span class="math">0 × c ＝ 0</span> となります。
したがって、0 で割るという操作を認めてしまうと、
<div class="math">0 ÷ 0 ＝ <span class="normal">任意の実数</span></div>
となってしまいます。
ちなみに、このような結果は「0÷0 は任意」とは言わず、
「0÷0 は不確定」といいます。
（当然、不確定なものは数としては認められません。）
 
また、<span class="math">b ≠ 0</span> となる任意の実数に対して、
<span class="math">a × 0 ≠ b</span> なので、
<div class="math">
      <span class="normal">任意の実数（非0）</span> ÷ 0 ≠ <span class="normal">任意の実数</span></div>
となり、0 で割った結果は実数の範囲では存在し得ないことが分かります。


## <a id="sec-generated-title-2"></a> <a id="d67e63"></a>0除算を認めると

0 で割るという操作を認めてしまうと変なことがおきます。
例えば、以下のような話を聞いたことがありませんか？

<blockquote markdown="1">
以下の手順で「任意の実数は 0 に等しい」ということが証明できる。

* <span class="math">a</span>を任意の実数とし、<span class="math">a ＝ b</span>とする。

* 両辺に<span class="math">a</span>をかけ、移項する。

* <span class="math">a<sup>2</sup> － ab ＝ 0</span>

* <span class="math">a (a － b) ＝ 0</span>

* 両辺を<span class="math">a － b</span>で割る。

* <span class="math">a ＝ 0</span>

* よって、任意の実数は0に等しい。


</blockquote>
この話は、数学をよく知らない人を騙すのに使われたりもしますが、
「0で割るという操作はしてはいけない」ということを教えるための例です。
この証明では、<span class="math">a － b</span> で割ってるのがそもそもの間違いです。
<span class="math">a ＝ b</span> なので、<span class="math">a － b ＝ 0</span>。
したがって、0 で割ってはいけないというルールに反しているので、
最後に得られる <span class="math">a ＝ 0</span> という結論は間違いになります。


## <a id="sec-generated-title-3"></a> <a id="d67e139"></a>結論

0 で割っちゃ駄目。
∞は実数としては存在しない。
