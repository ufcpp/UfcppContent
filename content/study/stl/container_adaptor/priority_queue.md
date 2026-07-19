---
title: "priority_queue"
source_url: "https://ufcpp.net/study/stl/container_adaptor/priority_queue/"
content_type: "Article"
published_at: "2015-05-06T14:23:35"
updated_at: "2015-05-06T14:23:35"
tags: []
umbraco_id: 1637
parent_id: 1634
sort_order: 2
aliases:
  - "/stl/container_adaptor/priority_queue/"
  - "/stl/priority_queue"
  - "/stl/priority_queue.html"
  - "/study/stl/priority_queue"
  - "/study/stl/priority_queue.html"
---

# priority_queue

##<a id="sec-generated-title-1"></a> <a id="d25e4"></a>priority_queue とは
<strong id="priority" class="keyword">priority_queue</strong>とは優先度つき待ち行列と呼ばれるもので、
挿入された順序どおりに要素の取り出しを行うのではなく、
優先度の高い要素から先に取り出す待ち行列。
 
例えば、<code>int</code>型を格納するpriority_queueで、整数の値をそのまま優先度として用いると、値の大きな整数から順に取り出される待ち行列になります。
 
priority_queueはヒープと呼ばれるデータ構造が使われます。
このヒープは、完全な2分木の形をしていて、
木の各ノードは自身より下にの要素よりも大きな値を持ちます。
そのため、最大の値を持つ要素は常に木の根に含まれていることになります。
この木の根にある要素を取り出すことで常に最大の値を持つ要素を取り出します。
 
ヒープの実装は、ランダムアクセス(<code>[]</code>を使った添字によるアクセス)のできるデータ構造を使って行えます。


##<a id="sec-generated-title-2"></a> <a id="d25e28"></a>STL におけるpriority_queue
STLのpriority_queueは何を使って実装するかをvector,queueのいずれかから選べます。<br></br>
キューの要素の型をTとすると、

<table summary="">

	<tr>
		<td markdown="1"><code>priority_queue&lt;T, vector&lt;T&gt; &gt;</code></td>
		<td markdown="1">vectorによるpriority_queue</td>
	</tr>
	<tr>
		<td markdown="1"><code>priority_queue&lt;T, deque&lt;T&gt; &gt;</code></td>
		<td markdown="1">dequeによるpriority_queue</td>
	</tr>
	<tr>
		<td markdown="1"><code>priority_queue&lt;T, vector&lt;T&gt;, greater&lt;T&gt; &gt;</code></td>
		<td markdown="1">vectorによるpriority_queue (値の小さな要素から取り出す)</td>
	</tr>
</table>


他にも、<code>push_back, pop_front, front, back, size</code>などのメソッドを適当に定義したクラスなら
何でもキューの実装に使えます。
