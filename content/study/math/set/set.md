---
title: "集合"
source_url: "https://ufcpp.net/study/math/set/set/"
content_type: "Article"
published_at: "2015-05-06T14:16:55"
updated_at: "2015-05-06T14:16:55"
tags: []
umbraco_id: 1473
parent_id: 1471
sort_order: 1
aliases:
  - "/math/set/set/"
  - "/set/set"
  - "/set/set.html"
  - "/study/set/set"
  - "/study/set/set.html"
---

# 集合

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[ZFC公理系](axiom.md#zfc)」を満たす数学的思考の対象を<strong id="set" class="keyword">集合</strong>(set)といいます。
自然数や実数などの集合も、ZFC公理系から出発して構築していくことが出来ます。
 
ZFC公理系を満たすもの以外にも、
数学的思考の対象（object）の集まり(collection)を考えることは出来ますが、
集合論ではそのような集まりは議論の対象から外します。
これは、何でもかんでも扱おうとして、理論が破綻しないようにするためです。
（何でもかんでも扱おうとすると生じてしまう矛盾の例として、
ラッセルの背理(Russell's paradox)というものがあります。
興味があれば調べてみてください。）


## <a id="sec-generated-title-2"></a> <a id="d42e18"></a>集合とは

「[概要](#abstract)」でも述べましたが、
集合論ではZFC公理系を満たすような物を集合と呼びます。
 
集合を現すのに、<span class="math">a, b, c, …, A, B, C, …, α, β, γ</span> などの文字が使われます。


### <a id="sec-generated-title-3"></a> <a id="d42e31"></a>元

集合という名前が示すとおり、集合は何らかの対象が集まったものです。
集合論で取り扱われる数学的対象は全て集合なので、集合の中身はやはり集合です。
集合の中身のことを<strong id="elem" class="keyword">元</strong>（element）または要素といい、
集合 <span class="math">a</span> が集合 <span class="math">b</span> の元であるとき、
<div class="math">
a ∈ b
</div>
と書き表します。


### <a id="sec-generated-title-4"></a> <a id="d42e52"></a>等しい集合

まず、「2つの集合が互いに等しい」というのがどういうことなのかを定義する公理が「[外延性公理](axiom.md#extensionality)」です。
<div class="math">
∀a∀b<span class="paren" style="font-size:em;">[</span>a=b⇔∀x<span class="paren" style="font-size:em;">(</span>x∈a⇔x∈b<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span></div>
これは簡単に言うと、「含まれる全ての元が等しい2つの集合は互いに等しい」ということです。
 
外延（extention）という言葉は内包（intention）という言葉の対義語なんですが、
これらの用語は、哲学や論理学で用いられる場合には、
内包は「ある事物が充たすべき条件」であり、
外延は「ある条件を充たす事物の集合」のことを指します。
例えば、「偶数」の場合、
内包は「2で割り切れる数」、
外延は「2, 4, 6, 8, …」となります。
 
集合論的には、集合の表し方として内包的記法と外延的記法という2つの方法があります。
内包的記法は、
<span class="math">S ＝ <span class="paren" style="font-size:em;">{</span>x | P(x)<span class="paren" style="font-size:em;">}</span></span> というように、集合 <span class="math">S</span> の元が満たすべき条件 <span class="math">P</span> を使って表す方法です。
一方、外延的記法は
<span class="math">S ＝ <span class="paren" style="font-size:em;">{</span>a, b, c, d, …<span class="paren" style="font-size:em;">}</span></span> というように、「集合 <span class="math">S</span> の元としての条件を満たすものを列挙する方法です。
再び「偶数」を例に挙げるなら、
偶数全体の集合 <span class="math">E</span> は、
内包的記法では <span class="math">E ＝ <span class="paren" style="font-size:em;">{</span>x | ∃n ∈ <span class="bold">N</span>, x ＝ 2 n<span class="paren" style="font-size:em;">}</span></span>、
外延的記法では <span class="math">E ＝ <span class="paren" style="font-size:em;">{</span>2, 4, 6, 8, …<span class="paren" style="font-size:em;">}</span></span> となります。
 
まあ、難しい話を抜きにして、外延性公理に関する部分だけ説明すると、
「中身が全部同じ ⇔ 同じ集合」というような考え方のことを外延的等価性といいます。


### <a id="sec-generated-title-5"></a> <a id="subset"></a>部分集合

<span class="math">∀x<span class="paren" style="font-size:em;">(</span>x∈a⇔x∈b<span class="paren" style="font-size:em;">)</span></span> というのが「等しい集合」の条件でした。
この ⇔ を一方通行に変えたものは集合の包含関係を表すものになります。

<span class="math">∀x<span class="paren" style="font-size:em;">(</span>x∈a→x∈b<span class="paren" style="font-size:em;">)</span></span> が成り立つとき、
「<span class="math">a</span> は <span class="math">b</span> の<strong id="sub" class="keyword">部分集合</strong>（subset）である」といい、
<span class="math">a ⊆ b</span> と表します。
また、<span class="math">a ⊆ b ∧ a ≠ b</span> のとき、
「<span class="math">a</span> は <span class="math">b</span> の<strong id="propsub" class="keyword">真部分集合</strong>（proper subset）である」といい、
<span class="math">a ⊂ b</span> と表します。
（流儀によっては、部分集合⊆を⊂で表し、真部分集合⊂を⊂の下に≠を付けた物で表すこともあります。）
 
集合の包含関係については以下のような命題が成り立ちます。
<div class="math">
a ⊆ a
</div><div class="math">
a ⊆ b ∧ b ⊆ a → a ＝ b
</div><div class="math">
a ⊆ b ∧ b ⊆ c → a ⊆ c
</div>

### <a id="sec-generated-title-6"></a> <a id="empty"></a>空集合

空集合と呼ばれる特殊な集合の存在を仮定するのが「[空集合の存在公理](axiom.md#empty)」です。
<div class="math">
∃a∀x<span class="paren" style="font-size:em;">[</span>￢ <span class="paren" style="font-size:em;">(</span>x∈a<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span></div>
これは、「全く元を含まない集合が存在する」ということです。
この「全く元を含まない集合」を<strong id="empty" class="keyword">空集合</strong>（empty set）とよび、
φで表します。
（本来、空集合は 0 （数字のゼロ）に斜め線を入れた記号で表すものですが、
写植やフォントの都合から、φ（ギリシャ文字のファイ）を使うことがよくあります。）
（あるいは、中身が空っぽの括弧 <span class="math"><span class="paren" style="font-size:em;">{</span><span class="paren" style="font-size:em;">}</span></span> で空集合を表すこともあります。）
 
ちなみに、証明は省略しますが、空集合はただ1つに確定します。
すなわち、2つの集合<span class="math">a, b</span> がともに「[空集合の存在公理](axiom.md#empty)」を満たすとき、
<span class="math">a ＝ b</span> となります。


## <a id="sec-generated-title-7"></a> <a id="d42e213"></a>集合に対する操作

### <a id="sec-generated-title-8"></a> <a id="pair"></a>対

2つの集合 <span class="math">a, b</span> から、これら2つを要素として持つ集合 <span class="math">c ＝ <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span> を作ることが考えられます。
このような操作が出来る（このような集合が存在する）ということを仮定するのが「[対の公理](axiom.md#pair)」です。
<div class="math">
∀a∀b∃c∃x<span class="paren" style="font-size:em;">(</span>x∈c ⇔ x＝a∨x＝b<span class="paren" style="font-size:em;">)</span></div>
このようにして得られる集合 <span class="math"><span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span> を<strong id="pair" class="keyword">対</strong>（pair）と呼びます。
このとき、<span class="math">a</span> と <span class="math">b</span> の順番は関係ありません。
すなわち、<span class="math"><span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span> と <span class="math"><span class="paren" style="font-size:em;">{</span>b, a<span class="paren" style="font-size:em;">}</span></span> はどちらも同じものになります。
順序が関係ないということを明示するために、対を非順序対（unordered pair）と呼ぶこともあります。
 
また、<span class="math">a ＝ b</span> の場合、対 <span class="math"><span class="paren" style="font-size:em;">{</span>a, a<span class="paren" style="font-size:em;">}</span></span> を単に
<span class="math"><span class="paren" style="font-size:em;">{</span>a<span class="paren" style="font-size:em;">}</span></span> と書き、<span class="math">a</span> の<strong id="singleton" class="keyword">シングルトン</strong>（singleton）と呼びます。
<span class="math">a</span> と <span class="math"><span class="paren" style="font-size:em;">{</span>a<span class="paren" style="font-size:em;">}</span></span> は全く別の集合になります。


### <a id="sec-generated-title-9"></a> <a id="union"></a>合併

「[合併集合の公理](axiom.md#union)」により、合併を作ることが出来ます。
<div class="math">
∀a∃b∀x<span class="paren" style="font-size:em;">[</span>x∈b ⇔ ∃c<span class="paren" style="font-size:em;">(</span>c∈a∧x∈c<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span></div>
これは「全ての元が集合 <span class="math">a</span> の元の元になるような集合 <span class="math">b</span> を作ることが出来る」ということです。
<span class="math">b</span> は <span class="math">a</span> の元の合併になります。
このような集合を、
<span class="math">b ＝ <table class="sigma" summary="statement under a function"><tr><td><span class="normal">∪</span></td></tr><tr><td class="sigmasub">c ∈ a</td></tr></table> c</span> または
<span class="math">b ＝ <span class="normal">∪</span>a</span> と表します。
 
この合併という操作は、「集合の中の壁を取り払う」操作になります。
例えば、<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span><span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>, <span class="paren" style="font-size:em;">{</span>c, d, e<span class="paren" style="font-size:em;">}</span>, <span class="paren" style="font-size:em;">{</span>f<span class="paren" style="font-size:em;">}</span><span class="paren" style="font-size:em;">}</span></span> のとき、内側の（<span class="math"><span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span> とかの）括弧を取り払って、<span class="math">∪A ＝ <span class="paren" style="font-size:em;">{</span>a, b, c, d, e, f<span class="paren" style="font-size:em;">}</span></span> という集合を作るということです。

<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span> と <span class="math">B ＝ <span class="paren" style="font-size:em;">{</span>c, d<span class="paren" style="font-size:em;">}</span></span> というような集合から
<span class="math">C ＝ <span class="paren" style="font-size:em;">{</span>a, b, c, d<span class="paren" style="font-size:em;">}</span></span> というような集合を作るためには、
一度 <span class="math">A</span> と <span class="math">B</span> の対を作ってから、
「[合併集合の公理](axiom.md#union)」を適用します。
<div class="math">
        <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>, <span class="paren" style="font-size:em;">{</span>c, d<span class="paren" style="font-size:em;">}</span></div>
↓「[対の公理](axiom.md#pair)」<div class="math">
        <span class="paren" style="font-size:em;">{</span>
          <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>, <span class="paren" style="font-size:em;">{</span>c, d<span class="paren" style="font-size:em;">}</span><span class="paren" style="font-size:em;">}</span>
      </div>
↓「[合併集合の公理](axiom.md#union)」<div class="math">
        <span class="paren" style="font-size:em;">{</span>a, b, c, d<span class="paren" style="font-size:em;">}</span>
      </div>
このような手順で得られた集合 <span class="math">C ＝ <span class="paren" style="font-size:em;">{</span>a, b, c, d<span class="paren" style="font-size:em;">}</span></span> を、
<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span> と <span class="math">B ＝ <span class="paren" style="font-size:em;">{</span>c, d<span class="paren" style="font-size:em;">}</span></span> の<strong id="union" class="keyword">合併</strong>（union）と呼び、<span class="math">C ＝ A ∪ B</span> と書きます。
合併は和集合（sum set）ということもあります。
合併には以下のような命題が成り立ちます。

* <span class="math">a ∪ a ＝ a</span>（冪等律）

* <span class="math">a ∪ b ＝ b ∪ a</span>（交換律）

* <span class="math">a ∪ <span class="paren" style="font-size:em;">(</span>b ∪ c<span class="paren" style="font-size:em;">)</span> ＝ <span class="paren" style="font-size:em;">(</span>a ∪ b <span class="paren" style="font-size:em;">)</span> ∪ c</span>（結合律）

* <span class="math">a ⊆ b ⇔ a ∪ b ＝ b</span>

* <span class="math">a ∪ φ ＝ a</span>


ちなみに、
<span class="math">a, b</span> が互いに「[共通部分](#intersection)」を持たないとき、
合併 <span class="math">a ∪ b</span> のことを<strong id="disjoint" class="keyword">直和</strong>（disjoint union, disjoint sum：互いに素な合併・和）集合と呼びます。
直和は ∪ という記号の代わりに、
＋ を ○ で囲った記号や、
Π を上下逆さまにした記号を使って表します。
（この記号が出せるフォントはあまりありませんが。）


### <a id="sec-generated-title-10"></a> <a id="intersection"></a>共通部分

「[分出公理](axiom.md#comprehension)」
集合 <span class="math">a</span> の元で、特定の条件（<span class="math">P<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> という命題）を満たすものを集めて作ったものもまた集合になることを主張しています。
<div class="math">
∀a∃b∀x<span class="paren" style="font-size:em;">[</span>x∈b ⇔ x∈a∧P<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span></div>
このような、「<span class="math">a</span> の中で、<span class="math">P<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span></span> を満たす元を集めて作った集合 <span class="math">b</span>」を
<span class="math">b ＝ <span class="paren" style="font-size:em;">{</span>x∈a | P<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></span> と表します。
 
特に、命題 <span class="math">P</span> が、<span class="math">x ∈ b</span> のとき、
すなわち、<span class="math">c ＝ <span class="paren" style="font-size:em;">{</span>x∈a | x∈b<span class="paren" style="font-size:em;">}</span></span> のとき、
<span class="math">c</span> を <span class="math">a</span> と <span class="math">b</span> の<strong id="intersection" class="keyword">共通部分</strong>（intersection）と呼び、<span class="math">C ＝ A ∩ B</span> と書きます。
共通部分は積集合（product set）ということもあります。
共通部分には以下のような命題が成り立ちます。

* <span class="math">a ∩ a ＝ a</span>（冪等律）

* <span class="math">a ∩ b ＝ b ∩ a</span>（交換律）

* <span class="math">a ∩ <span class="paren" style="font-size:em;">(</span>b ∩ c<span class="paren" style="font-size:em;">)</span> ＝ <span class="paren" style="font-size:em;">(</span>a ∩ b <span class="paren" style="font-size:em;">)</span> ∩ c</span>（結合律）

* <span class="math">a ⊆ b ⇔ a ∩ b ＝ a</span>

* <span class="math">a ∩ φ ＝ φ</span>


また、集合の合併との間に以下のような関係（分配律）が成り立ちます。

* <span class="math">a ∪ <span class="paren" style="font-size:em;">(</span>b ∩ c<span class="paren" style="font-size:em;">)</span> ＝ <span class="paren" style="font-size:em;">(</span>a ∪ b <span class="paren" style="font-size:em;">)</span> ∩ <span class="paren" style="font-size:em;">(</span>a ∪ c <span class="paren" style="font-size:em;">)</span></span>

* <span class="math">a ∩ <span class="paren" style="font-size:em;">(</span>b ∪ c<span class="paren" style="font-size:em;">)</span> ＝ <span class="paren" style="font-size:em;">(</span>a ∩ b <span class="paren" style="font-size:em;">)</span> ∪ <span class="paren" style="font-size:em;">(</span>a ∩ c <span class="paren" style="font-size:em;">)</span></span>


<span class="math">b ∈ a</span> に対して、
<span class="math"><span class="bar">b</span> ＝ <span class="paren" style="font-size:em;">{</span>x∈b | ∀y <span class="paren" style="font-size:em;">(</span>y∈あ → x∈y<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">}</span></span> という集合を作ると、<span class="math"><span class="bar">b</span></span> は <span class="math">a</span> の全ての元の共通部分になります。
このような集合を、
<span class="math">b ＝ <table class="sigma" summary="statement under a function"><tr><td><span class="normal">∩</span></td></tr><tr><td class="sigmasub">c ∈ a</td></tr></table> c</span> または
<span class="math">b ＝ <span class="normal">∩</span>a</span> と表します。


### <a id="sec-generated-title-11"></a> <a id="d42e676"></a>その他の操作

共通部分とは逆に、<span class="math">c ＝ <span class="paren" style="font-size:em;">{</span>x∈a | ￢ x∈b<span class="paren" style="font-size:em;">}</span></span> という集合を作ることが出来ます。
このような集合 <span class="math">c</span> を<strong id="diff" class="keyword">差</strong>（difference）と呼び、<span class="math">C ＝ A － B</span> と書きます。
自然数などの差とは性質がかなり異なっているので、区別するために集合論的差という言い方をする場合が多いです。
また、和集合・積集合などにあわせて、差集合と呼ぶこともあります。
 
集合 <span class="math">U, a</span> について、<span class="math">a ⊆ U</span> であるとき、
集合 <span class="math">U － a</span> を <span class="math">a</span> の <span class="math">U</span> に対する<strong id="complement" class="keyword">補集合</strong>（complement）と呼びます。
特に、<span class="math">U</span> がどういう集合か明らかであり、明示する必要がない場合には、
<span class="math">a</span> の補集合を <span class="math">a<sup>c</sup></span> と表します。
補集合には以下のような命題が成り立ちます。

* <span class="math">
            <span class="paren" style="font-size:em;">(</span>a<sup>c</sup><span class="paren" style="font-size:em;">)</span>
            <sup>c</sup> ＝ a</span>

* <span class="math">U<sup>c</sup> ＝ φ</span>

* <span class="math">φ<sup>c</sup> ＝ U</span>

* <span class="math">a ∩ a<sup>c</sup> ＝ φ</span>

* <span class="math">a ∪ a<sup>c</sup> ＝ U</span>

* <span class="math">a ⊆ b ⇔ a<sup>c</sup> ⊇ b<sup>c</sup></span>

* <span class="math">（a ∩ b）<sup>c</sup> ＝ a<sup>c</sup> ∪ b<sup>c</sup></span>

* <span class="math">（a ∪ b）<sup>c</sup> ＝ a<sup>c</sup> ∩ b<sup>c</sup></span>


最後の2つの命題は de Morgan の法則と呼ばれています。


## <a id="sec-generated-title-12"></a> <a id="d42e826"></a>冪集合

集合 <span class="math">a</span> から <span class="math">a</span> の部分集合全体からなる集合 <span class="math">b</span> を作ることを考えます。
このような集合が存在することを保証するのが「[ベキ集合の公理](axiom.md#power)」です。
<div class="math">
∀a∃b∀x<span class="paren" style="font-size:em;">(</span>x∈b ⇔ x⊆a<span class="paren" style="font-size:em;">)</span></div>
このような集合 <span class="math">b</span> を<strong id="power" class="keyword">冪集合</strong>（power set）とよび、
<span class="math">b ＝ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span></span> と表します。
（<span class="math"><span class="cursive">P</span></span> は筆記体の大文字の P。）
 
例えば、<span class="math">A ＝ <span class="paren" style="font-size:em;">{</span>a, b, c<span class="paren" style="font-size:em;">}</span></span> のとき、
<div class="math">
      <span class="cursive">P</span>
      <span class="paren" style="font-size:em;">(</span>A<span class="paren" style="font-size:em;">)</span> ＝
<span class="paren" style="font-size:em;">{</span>
 φ,
 <span class="paren" style="font-size:em;">{</span>a<span class="paren" style="font-size:em;">}</span>,
 <span class="paren" style="font-size:em;">{</span>b<span class="paren" style="font-size:em;">}</span>,
 <span class="paren" style="font-size:em;">{</span>c<span class="paren" style="font-size:em;">}</span>,
 <span class="paren" style="font-size:em;">{</span>b, c<span class="paren" style="font-size:em;">}</span>,
 <span class="paren" style="font-size:em;">{</span>c, a<span class="paren" style="font-size:em;">}</span>,
 <span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span>,
 <span class="paren" style="font-size:em;">{</span>a, b, c<span class="paren" style="font-size:em;">}</span><span class="paren" style="font-size:em;">}</span></div>
となります。

<span class="math">a ⊆ b</span> （<span class="math">a</span> は <span class="math">b</span> の部分集合である）という関係は、
<span class="math">a ∈ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span></span> と書き表すことも出来ます。
（<span class="math">a ⊂ b</span>（真部分集合）は <span class="math">a ∈ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span> － <span class="paren" style="font-size:em;">{</span>b<span class="paren" style="font-size:em;">}</span></span>。）
また、冪集合について以下のような命題が成り立ちます。

* <span class="math">a ⊆ b ⇔ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ⊆ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span></span>

* <span class="math">
          <span class="cursive">P</span>
          <span class="paren" style="font-size:em;">(</span>a∩b<span class="paren" style="font-size:em;">)</span> ＝ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ∩ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span></span>

* <span class="math">
          <span class="cursive">P</span>
          <span class="paren" style="font-size:em;">(</span>a∪b<span class="paren" style="font-size:em;">)</span> ⊇ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>a<span class="paren" style="font-size:em;">)</span> ∪ <span class="cursive">P</span><span class="paren" style="font-size:em;">(</span>b<span class="paren" style="font-size:em;">)</span></span>
