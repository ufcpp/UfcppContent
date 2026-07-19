---
title: "集合の公理系"
source_url: "https://ufcpp.net/study/math/set/axiom/"
content_type: "Article"
published_at: "2015-05-06T14:16:54"
updated_at: "2015-05-06T14:16:54"
tags: []
umbraco_id: 1472
parent_id: 1471
sort_order: 0
aliases:
  - "/math/set/axiom/"
  - "/set/axiom"
  - "/set/axiom.html"
  - "/study/set/axiom"
  - "/study/set/axiom.html"
---

# 集合の公理系

## <a id="sec-generated-title-1"></a> <a id="axiom"></a>公理系

公理とは「数学の理論体系で定理を証明するにあたって、前提として仮定するいくつかの事柄」を指します。
どのような理論体系にも公理、すなわち「前提とする仮定」が存在します。
もちろん、この「仮定」が間違っていれば、理論全体が間違いになりますが、
そもそもの仮定が正しいのかどうかを証明するすべはありません。

正しいかどうか分からないものは少ないに越したことはありません。
したがって、公理と言うものは必要最小限である必要があります。
極力少ない仮定から出発して、より多くの結論を導き出せるものこそが優れた理論であると言えます。

では、自然数や実数などの、数学の集合を構築するためには、
最低限どのような公理が必要になるのでしょうか。
自然数という集合の存在そのものや、1＋1＝2という計算法則は公理として仮定しなければいけないのでしょうか。
それとも、もっとシンプルな公理から出発して証明できるものなのでしょうか。

結論から言うと、自然数などの集合はこれから説明していくような、
たかだか10個程度の公理系から構築することができます。
公理系にもさまざまな決め方がありますが、
現在、一般的に使われている集合の公理系は ZFC と呼ばれる公理系です。


## <a id="sec-generated-title-2"></a> <a id="zfc"></a>ZFC公理系

まず、ZFC公理系の前に、Zermelo、Fraenkelの2名により体系化された<strong id="zf" class="keyword">ZF公理系</strong>というものがあります。

<table summary="">

	<tr>
		<th>名前</th>
		<th>式</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1"><strong id="extensionality" class="keyword">外延性公理</strong>（Axiom of extensionality）</td>
		<td markdown="1">∀a∀b[a=b⇔∀x(x∈a⇔x∈b)]</td>
		<td markdown="1">二つの集合が等しいというのと、それぞれの集合に含まれる元が全て等しいというのは同じである。（等値性の定義）</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="empty" class="keyword">空集合の存在公理</strong>（Axiom of empty set）</td>
		<td markdown="1">∃a∀x[￢(x∈a)]</td>
		<td markdown="1">{} という集合（要素を持たない集合、空集合）が存在する。</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="pair" class="keyword">対の公理</strong>（Axiom of pairing）</td>
		<td markdown="1">∀a∀b∃c∃x(x∈c⇔x=a∨x=b)</td>
		<td markdown="1">x, y が集合であるとき、{x, y} という集合（「対（もしくは非順序対）」と呼ぶ）が存在する。</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="union" class="keyword">合併集合の公理</strong>（Axiom of union）</td>
		<td markdown="1">∀a∃b∀x[x∈b⇔∃c(c∈a∧x∈c)]</td>
		<td markdown="1">A を集合とすると、A の全ての元の合併 B、つまり B の元はすべて A の元の元となるような集合が存在する。</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="infinity" class="keyword">無限集合の公理</strong>（Axiom of infinity）</td>
		<td markdown="1">∃a[φ∈a∧∀x(x∈a⇒x+∈a]</td>
		<td markdown="1">空集合を含み、またある元 x を含むなら、x ∪ {x} も含むような集合が存在する。 x+ = x∪{x} … 「後継ぎ」と呼ぶ。</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="power" class="keyword">ベキ集合の公理</strong>（Axiom of power set）</td>
		<td markdown="1">∀a∃b∀x(x∈b⇔x⊆a)</td>
		<td markdown="1">どんな集合 x に対しても x の部分集合全てからなるような集合が存在する。</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="replacement" class="keyword">置換公理</strong>（Axiom of replacement）</td>
		<td markdown="1">∀x[x∈a⇒∀y∀z(P(a,y)∧P(x,y)⇒y=z)]⇒∃b∀u[u∈b⇔∃x(x∈a∧P(x,y))]</td>
		<td markdown="1">関数の、集合による値域は集合である；関数はここでは、論理式 P(x, y) で、どんな a に対しても「P(a, y) かつ P(a, z) なら y = z」が結論されるようなものである。</td>
	</tr>
	<tr>
		<td markdown="1"><strong id="regularity" class="keyword">正則性の公理</strong>（Axiom of regularity）</td>
		<td markdown="1">∀a[a≠φ⇒∃b(b∈a∧a∩b=φ)]</td>
		<td markdown="1">X が空集合でなければ、ある X の元 Y があって、X ∩ Y = {}（交わらない）である。</td>
	</tr>
</table>


そして、ZF公理系に以下の定理を加えたものを<strong id="zfc" class="keyword">ZFC公理系</strong>(Zermelo-Fraenkel に、公理の名前（Choice）の頭文字を加えたもの)と呼びます。

<table summary="">

	<tr>
		<th>名前</th>
		<th>式</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1"><strong id="choice" class="keyword">選択公理</strong>（Axiom of choice）</td>
		<td markdown="1">∀a∃f[f∈(∪a)^a∧∀x(x∈a∧x≠φ⇒f(x)∈x)]</td>
		<td markdown="1">X をそのどの元も互いに交わらないような空集合でない集合とするとき、X の各元から一つずつとってきたような集合が存在する。</td>
	</tr>
</table>


ZF 公理系のうちで、置換公理は以下に挙げる分出公理の変わりに後から導入されたものです。
（置換公理の方が強い条件で、分出公理は置換公理から導き出すことが出来る。）

<table summary="">

	<tr>
		<th>名前</th>
		<th>式</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1"><strong id="comprehension" class="keyword">分出公理</strong>（Axiom of comprehension）</td>
		<td markdown="1">∀a∃b∀x[x∈b⇔x∈a∧P(x)]</td>
		<td markdown="1">任意の集合 A とある集合に関する性質 P(X) に対して A の元で、P(x) を満たすような x 全体は集合をなす。</td>
	</tr>
</table>
