---
title: "stack"
source_url: "https://ufcpp.net/study/stl/container_adaptor/stack/"
content_type: "Article"
published_at: "2015-05-06T14:23:30"
updated_at: "2015-05-06T14:23:30"
tags: []
umbraco_id: 1635
parent_id: 1634
sort_order: 0
aliases:
  - "/stl/container_adaptor/stack/"
  - "/stl/stack"
  - "/stl/stack.html"
  - "/study/stl/stack"
  - "/study/stl/stack.html"
---

# stack

##<a id="sec-generated-title-1"></a> <a id="d23e4"></a>stackとは
スタックは末尾(または先頭)に限って要素の追加削除を行うことのできるデータ構造です。
末尾からしか要素を取り出せないので、
必然的に「後に入れたものから先に取り出さなくてなはならない」ということになります。
そのため、スタックはLIFO(last-in first-out)とも呼ばれます。
スタックに新しい要素を挿入することをpush、要素を取り出すことをpopといいます。<br></br>	[![stack.png](../../../../assets/media/ufcpp2000/stl/fig/stack.png)](../../../../assets/media/ufcpp2000/stl/fig/stack.png)

スタックは末尾への要素の挿入・削除ができるデータ構造なら何を使っても実装することができます。
普通は配列や単方向連結リストを用いて実装します。


##<a id="sec-generated-title-2"></a> <a id="d23e17"></a>STLにおけるstack
STLのstackは何を使って実装するかをvector,deque,listの中から選べます。
スタックの要素の型をTとすると、

<table summary="">

	<tr>
		<td markdown="1"><code>stack&lt;T, vector&lt;T&gt; &gt;</code></td>
		<td markdown="1">vectorによるstack</td>
	</tr>
	<tr>
		<td markdown="1"><code>stack&lt;T, deque&lt;T&gt; &gt;</code></td>
		<td markdown="1">dequeによるstack</td>
	</tr>
	<tr>
		<td markdown="1"><code>stack&lt;T, list&lt;T&gt; &gt;</code></td>
		<td markdown="1">listによるstack</td>
	</tr>
	<tr>
		<td markdown="1"><code>stack&lt;T&gt;</code></td>
		<td markdown="1">stack&lt;T, vector&lt;T&gt; &gt;と同じ意味</td>
	</tr>
</table>


他にも、<code>push_back, pop_back, back, size</code>などのメソッドを適当に定義したクラスなら
何でもスタックの実装に使えます。
