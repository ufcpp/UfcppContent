---
title: "queue"
source_url: "https://ufcpp.net/study/stl/container_adaptor/queue/"
content_type: "Article"
published_at: "2015-05-06T14:23:33"
updated_at: "2015-05-06T14:23:33"
tags: []
umbraco_id: 1636
parent_id: 1634
sort_order: 1
aliases:
  - "/stl/container_adaptor/queue/"
  - "/stl/queue"
  - "/stl/queue.html"
  - "/study/stl/queue"
  - "/study/stl/queue.html"
---

# queue

##<a id="sec-generated-title-1"></a> <a id="d24e4"></a>queueとは
queue(キュー)とは、「行列」という意味の単語です。
ただ、「行列」だと、matrixと区別がつかなくなるので、日本語では待ち行列といいます。
(ちなみに、matrixの英語本来の意味は母体・基盤・鋳型)
 
キューは「要素の挿入は末尾から、取り出し・削除は先頭から行うデータ構造」です。
後ろから並んで前に出て行くという意味でこの名前がつきました。
FIFO(first-in first-out)と呼ばれることもあります。
キューでは要素を追加することをenqueue、要素を取り出すことをdequeueといいます。
(ただし、STLでは他のコンテナと名前をあわせるためにpush,popという用語を用います)<br></br>	[![queue.png](../../../../assets/media/ufcpp2000/stl/fig/queue.png)](../../../../assets/media/ufcpp2000/stl/fig/queue.png)

キューは末尾への要素の挿入と先頭からの要素の削除が可能なデータ構造なら何を使っても実装することができます。
普通はリングバッファや双方向循環連結リストを用いて実装します。


##<a id="sec-generated-title-2"></a> <a id="d24e18"></a>STLにおけるqueue
STLのqueueは何を使って実装するかをdeque,listのいずれかから選べます。
キューの要素の型をTとすると、

<table summary="">

	<tr>
		<td markdown="1"><code>queue&lt;T, deque&lt;T&gt; &gt;</code></td>
		<td markdown="1">dequeによるqueue</td>
	</tr>
	<tr>
		<td markdown="1"><code>queue&lt;T, list&lt;T&gt; &gt;</code></td>
		<td markdown="1">listによるqueue</td>
	</tr>
	<tr>
		<td markdown="1"><code>queue&lt;T&gt;</code></td>
		<td markdown="1">queue&lt;T, deque&lt;T&gt; &gt;と同じ意味</td>
	</tr>
</table>


他にも、<code>push_back, pop_front, front, back, size</code>などのメソッドを適当に定義したクラスなら
何でもキューの実装に使えます。
