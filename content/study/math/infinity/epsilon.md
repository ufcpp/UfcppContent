---
title: "ε-δ"
source_url: "https://ufcpp.net/study/math/infinity/epsilon/"
content_type: "Article"
published_at: "2015-05-06T14:17:57"
updated_at: "2015-05-06T14:17:57"
tags: []
umbraco_id: 1503
parent_id: 1500
sort_order: 2
aliases:
  - "/study/infinity/epsilon.html"
---

# ε-δ

## <a id="sec-generated-title-1"></a> <a id="d69e4"></a>限りなく？

「[極限](limit.md#limit)」では極限という考え方について簡単に触れました。
極限では、「限りなく大きくなる」とか「限りなく近づく」とかいう言葉を使いますが、
じゃあ、この「限りなく」というのはどういうことなのでしょうか。
 
「限りなく近づく」なんて言い方だと、「近似的に成り立つ」とか「漸近的に成り立つ」などという言葉と大差がない感じがします。
こういう曖昧な言葉を使った定義をそのままにしておくと、
議論が広がる余地がなくなるのでいいことではありません。
そこで、この「限りなく」というものを厳密に定める必要があります。


## <a id="sec-generated-title-2"></a> <a id="d69e10"></a>ε‐δ論法

「<span class="math">x</span> がある値 <span class="math">a</span> に限りなく近づく」というのは、
「<span class="math"><span class="normal">|</span>x － a<span class="normal">|</span>＜δ</span> としたとき、<span class="math">δ</span>の値がどんどん小さくなる」と表すことができます。
「<span class="math">f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> がある値 <span class="math">b</span> に限りなく近づく」というのも、
「<span class="math"><span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － b<span class="normal">|</span> ＜ε</span> としたとき、<span class="math">ε</span>の値がどんどん小さくなる」
と表します。
そして、
「<span class="math">x</span> を限りなく <span class="math">a</span> に近づけたとき、
<span class="math">f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> が <span class="math">b</span> に限りなく近づく」というのは、
式としては
「<span class="math"><span class="normal">|</span>x － a<span class="normal">|</span> ＜ δ ⇒ <span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － b<span class="normal">|</span> ＜ε</span>」
と表して、
<span class="math">δ</span> も <span class="math">ε</span> もどんどん小さくするということになります。
 
ここで、「<span class="math">δ</span> を限りなく小さくしたときに、<span class="math">ε</span> も限りなく小さくなる」
という言い方をしたのでは、結局「限りなくって何？」という疑問は解決できません。
 
ここで発想を逆転して、
<span class="math">δ</span> よりも先に <span class="math">ε</span> の方を先に固定してみます。
すなわち、
「<span class="math">ε</span> を任意に小さくできるような <span class="math">δ</span> が存在する」
と言い換えてみましょう。
これなら定式化できます。
<em><div class="math">
∀ε＞0 ∃δ, <span class="normal">|</span>x － a<span class="normal">|</span> ＜ δ ⇒ <span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － b<span class="normal">|</span> ＜ε
</div></em>
右側にある <span class="math">ε</span> が「任意の」で、
左側にある <span class="math">δ</span> が「存在する」なのがポイントです。
すなわち、
<span class="math">δ</span> は <span class="math">ε</span> の値によって決まる従属変数です。
任意の正の実数 <span class="math">ε</span> に対して、
<div class="math"><span class="normal">|</span>x － a<span class="normal">|</span> ＜ δ<span class="paren" style="font-size:em;">(</span>ε<span class="paren" style="font-size:em;">)</span> ⇒ <span class="normal">|</span>f<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> － b<span class="normal">|</span> ＜ε
</div>
という命題が恒等的に真となるような関数 <span class="math">δ<span class="paren" style="font-size:em;">(</span>ε<span class="paren" style="font-size:em;">)</span></span> が存在すると言う感じで考えてもらってOKです。
 
この考え方を、
<span class="math">ε</span> の方を先に考えて、<span class="math">δ</span> はその従属変数という意味で、
<strong id="epsilon-delta" class="keyword">ε－δ論法</strong>と呼びます。
 
数列 <span class="math">a<sub>n</sub></span> の極限なんかもこれと同じように、
<div class="math">
∀ε＞0 ∃N, n＞N ⇒ <span class="normal">|</span>a<sub>n</sub> － α<span class="normal">|</span> ＜ε
</div>
で定義します。
これも、
<span class="math">ε</span> の方が「任意の」で <span class="math">N</span> の方が「存在する」です。


## <a id="sec-generated-title-3"></a> <a id="d69e184"></a>まとめ

この考え方の下では∞というものは存在しません。
∞という概念抜きで「限りなく大きく」や「限りなく近づく」と言うものを表現しています。
