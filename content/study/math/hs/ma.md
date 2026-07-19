---
title: "数学A"
source_url: "https://ufcpp.net/study/math/hs/ma/"
content_type: "Article"
published_at: "2015-05-06T14:16:10"
updated_at: "2015-05-06T14:16:10"
tags: []
umbraco_id: 1450
parent_id: 1445
sort_order: 4
aliases:
  - "/hs/ma"
  - "/hs/ma.html"
  - "/math/hs/ma/"
  - "/study/hs/ma"
  - "/study/hs/ma.html"
---

# 数学A

##<a id="sec-generated-title-1"></a> <a id="figure"></a>平面図形
* 三角形の性質

* 円の性質


高校数学のほとんどは、17～18世紀の数学なんですが、
ここだけ時代が古い。
ギリシャ数学の範疇だったり。
 
これの役割は中学と高校の数学の間の橋渡し。
あと、三角関数がらみの定理の証明にいくつかの幾何学の定理を使うくらい。


###<a id="sec-generated-title-2"></a> <a id="coordinate"></a>座標系の導入
数IIの「[図形と方程式](m2.md#geometry)」で習うように、
座標系を導入してしまえば、方程式やベクトルの問題に帰着されてしまう問題がほとんど。
まあ、もちろん、幾何学の定理を覚えてる方が解くの簡単だけど。
 
ちなみに、数Bで習うベクトルに、矢印による表現（幾何ベクトル）と数値の組による表現（数ベクトル）があるのは、
要するに、幾何的な図形である矢印に座標系を導入したような物。


###<a id="sec-generated-title-3"></a> <a id="kika"></a>“幾何”
数学の用語ってちゃんと意訳されている語が多くて、
漢字の字面を見ればある程度その言葉の意味が分かるんですが、
「幾何学」は例外。
幾何学って言葉は珍しく、音訳語なんですよね。
geometry の geo- の部分（土地を表すギリシャ語を起源とする接頭語）の音訳。
（「幾何」の中国語読みは「ちーほー」。）
geometory という原語の意味は、geo- （土地）＋metry（測定学）。
 
まあ、だから、高校の教科書では幾何学に相当する所の章タイトルが「図形」とかになってるんですけど。
確かにこの方が分かりやすくていいと思う。
いっそのこと、geometry とか geometric の訳語を図形学とか図形的にしてしまう方がいいかも。


##<a id="sec-generated-title-4"></a> <a id="d22e42"></a>集合と論理
* 集合と要素の個数

* 命題と証明



###<a id="sec-generated-title-5"></a> <a id="set"></a>集合
現代数学的視点でみると、
自然数とか複素数とかも集合だし、
微分可能な実関数全体とかも集合。
数学的考察の対象となるものは大半が集合で、
全ての数学の基礎となる重要な概念。
 
といっても、高校ではそんな複雑なことは習いませんけど。
気をつけないといけないのは
集合の要素は順序を持たないのと、重複を認めない点くらい？
要するに、
<span class="math"><span class="paren" style="font-size:em;">{</span>a, b<span class="paren" style="font-size:em;">}</span></span>
と
<span class="math"><span class="paren" style="font-size:em;">{</span>b, a<span class="paren" style="font-size:em;">}</span></span>
は区別が付かないし、
<span class="math"><span class="paren" style="font-size:em;">{</span>a<span class="paren" style="font-size:em;">}</span></span>
と
<span class="math"><span class="paren" style="font-size:em;">{</span>a, a<span class="paren" style="font-size:em;">}</span></span>
も区別が付かない。
 
参考：「[数学](../index.md)」。


###<a id="sec-generated-title-6"></a> <a id="sufficient"></a>必要性と十分性
<span class="math">a <span class="normal">⇒</span> b</span> といわれた場合、
命題 <span class="math">a</span> が真の時には <span class="math">b</span> は確実に真なんですが、
<span class="math">a</span> が偽だった時のことは何も分かりません。
<span class="math">a</span> が偽だった時は、<span class="math">b</span> 真だろうが偽だろうが知ったこっちゃない。
 
このことを逆に言うと、
<span class="math">b</span> が真でも、<span class="math">a</span> が偽のこともありうる。
でも、<span class="math">b</span> が偽なのに <span class="math">a</span> が真ということはあり得ない。

<table summary="a→b">
	<caption>
		a→b
	</caption>
	<tr>
		<th><span class="math">a</span>の真偽</th>
		<th><span class="math">b</span>の真偽</th>
		<th>可能性</th>
	</tr>
	<tr>
		<td markdown="1">真</td>
		<td markdown="1">真</td>
		<td markdown="1"><span class="math">a</span>が真の時点で<span class="math">b</span>が真なことが確定。</td>
	</tr>
	<tr>
		<td markdown="1">真</td>
		<td markdown="1">偽</td>
		<td markdown="1">ありえない。</td>
	</tr>
	<tr>
		<td markdown="1">偽</td>
		<td markdown="1">真</td>
		<td markdown="1">別にこうなっても OK。</td>
	</tr>
	<tr>
		<td markdown="1">偽</td>
		<td markdown="1">偽</td>
		<td markdown="1">別にこれも OK。</td>
	</tr>
</table>


「<span class="math">a</span> が真なら100％ <span class="math">b</span> も真」なわけですが、
この「100％」というのをさして、
<span class="math">a</span> を <span class="math">b</span> の「十分条件（sufficient condition）」と言い表します。
 
「逆に、<span class="math">a</span> を真にしたければ、少なくとも <span class="math">b</span> は真でないと駄目」ということになりますが、
この「少なくとも」というのをさして、
<span class="math">b</span> を <span class="math">a</span> の「必要条件（necessary condition）」といいます。
 
必要性と十分性、どっちがどっちだったか結構迷いやすいんですが、
「100％ ＝ 十分」、「少なくとも ＝ 必要」と考えれば、多少は迷いにくくなると思います。


##<a id="sec-generated-title-7"></a> <a id="probability"></a>場合の数と確率
* 順列・組み合わせ

* 確率とその基本的な法則

* 独立な試行と確率



###<a id="sec-generated-title-8"></a> <a id="combinational"></a>組み合わせ論的確率
高校で習う確率というと「組み合わせ論的確率」。
 
これの問題はもう、正直、法則もクソもなくて、
「猿になったつもりでひたすら数える」のが基本。
部分的に、順列とか組み合わせの数の公式を使って楽できるけども、
やっぱり最終的にはひたすら数え上げるしかない。


###<a id="sec-generated-title-9"></a> <a id="metrical"></a>測度論的確率
組み合わせ論的な確率ばっかりやるもんだから、
大学とか入って、確率変数が実数になったとたんにわけ分からなくなることあり。
でも、実数みたいな連続なものに対する確率をまじめに考えようとすると、
微分積分に対する結構深い知識が必要で難しい。
 
ほんとに厳密に考えようとすると、
「測度」とか「ルベーグ積分」というものの知識が必要。
（大学でも、測度とかまで習う学科は少ないかも。）
 
でも、組み合わせ論が重要でないというわけではなくて、
コンピュータの進歩に伴って、これはこれで重要。
