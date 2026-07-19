---
title: "コレクション概要"
source_url: "https://ufcpp.net/study/algorithm/collection/collection/"
content_type: "Article"
published_at: "2015-05-06T14:04:51"
updated_at: "2015-12-20T00:00:00"
tags: []
umbraco_id: 1129
parent_id: 1128
sort_order: 0
aliases:
  - "/algorithm/collection.html"
  - "/algorithm/collection/collection/"
  - "/study/algorithm/collection.html"
---

# コレクション概要

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「データ構造」と呼ばれるものの代表格というと、
同じ型のデータをたくさん集めたもの、すなわちコレクションと呼ばれるものでしょう。

今の世の中、Java も C# も標準で、
List やら Dictionary やらいろいろなコレクションを持っています。
C++ も、1998年に標準化された STL と呼ばれるライブラリに、
一通りのコレクション（STL の流儀ではコンテナとも呼びます）が揃っています。

昔の人は結構な頻度でこの手のデータ構造を自作していました。
また、標準でその手のコレクションを利用できる現在においても、
どのコレクションの内部構造がどういう風になっているのか概要だけでも知っていれば、
どういうケースでどのコレクションを使うのが最適なのかがすぐに分かるという利点があります。

かなりの部分が STL の説明と被るので、
「単なるC# サンプルプログラム」みたいな扱いになりそうですが、
とにかく、コレクションについての説明をしていきたいと思います。


##<a id="sec-generated-title-2"></a> <a id="cpp"></a>C++ のコレクション
1998年に標準化された STL（Standard Template Library）には多数のコレクションクラスが含まれています。
詳細は「[C++ STL](../../stl/index.md)」で説明しますが、
C++ の STL は非常に高機能で、
現在の C++ では、ほとんどの場合、コレクションクラスを自作する必要がありません。

##<a id="sec-generated-title-3"></a> <a id="cs"></a>C# のコレクション
.NET Framework には、
<code>System.Collections.Generic</code> 名前空間以下にコレクションクラスが用意されています。
C++ の STL と比べると、その機能はシンプルで、
特に需要の高いコレクションのみが揃っています。

STL と比べて欠けているのは、
「[deque](../../stl/seq_container/deque.md#deque)」、
「[priority_queue](../../stl/container_adaptor/priority_queue.md#priority)」、
「[set](../../stl/assosiative_container/set.md#set)」 および
「[multiset](../../stl/assosiative_container/set.md#multiset)」 に相当する物がないのと、
STL の iterator に相当する enumerator が、
forward iterator 相当の機能しか持っていないという点です。
これで困る場面はそれほど多くもないですが、
元々 C++ を使いこなしていた方には少々不満かもしれません。

C++/CLI （.NET Framework 向けに拡張された C++）専用で、
[STL.NET](http://www.microsoft.com/japan/msdn/vs05/visualc/stl-netprimer.aspx) というライブラリもありますが、
残念ながら C++/CLI からしか利用できません。


##<a id="sec-generated-title-4"></a> <a id="inpl"></a>実装方法による分類
###<a id="sec-generated-title-5"></a> <a id="sequence"></a>順序構造を保つコレクション
<table summary="">

	<tr>
		<th>名前</th>
		<th>C++ STL</th>
		<th>C# System.Collections</th>
	</tr>
	<tr>
		<td markdown="1">「[配列リスト](col_array.md)」</td>
		<td markdown="1">[`vector`](http://cpprefjp.github.io/reference/vector.html)</td>
		<td markdown="1">[`List`](https://msdn.microsoft.com/ja-jp/library/6sh2ey19.aspx)</td>
	</tr>
	<tr>
		<td markdown="1">「[循環バッファ](col_circular.md)」</td>
		<td markdown="1">[`deque`](http://cpprefjp.github.io/reference/deque.html)</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">「[片方向連結リスト](col_flist.md)」</td>
		<td markdown="1">[`forward_list`](http://cpprefjp.github.io/reference/forward_list.html)</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">「[双方向連結リスト](col_blist.md)」</td>
		<td markdown="1">[`list`](http://cpprefjp.github.io/reference/list.html)</td>
		<td markdown="1">[`LinkedList`](https://msdn.microsoft.com/ja-jp/library/he2s3bh7.aspx)</td>
	</tr>
</table>



###<a id="sec-generated-title-6"></a> <a id="set"></a>検索が高速なコレクション
<table summary="">

	<tr>
		<th>名前</th>
		<th>C++ STL</th>
		<th>C# System.Collections</th>
	</tr>
	<tr>
		<td markdown="1">「[ソート済み配列](col_sorted.md)」</td>
		<td markdown="1">　</td>
		<td markdown="1">[`SortedList`](https://msdn.microsoft.com/ja-jp/library/ms132319.aspx)</td>
	</tr>
	<tr>
		<td markdown="1">「[ハッシュテーブル](col_hash.md)」</td>
		<td markdown="1">[`unordered_set`](http://cpprefjp.github.io/reference/unordered_set.html), [`unordered_map`](http://cpprefjp.github.io/reference/unordered_map.html)</td>
		<td markdown="1">[`Dictionary`](https://msdn.microsoft.com/ja-jp/library/xfhwa508.aspx), [`HashSet`](https://msdn.microsoft.com/ja-jp/library/bb359438.aspx)</td>
	</tr>
	<tr>
		<td markdown="1">「[2分探索木](col_tree.md)」</td>
		<td markdown="1">[`set`](http://cpprefjp.github.io/reference/set.html), [`map`](http://cpprefjp.github.io/reference/map.html)</td>
		<td markdown="1">[`SortedDictionary`](https://msdn.microsoft.com/ja-jp/library/ms132289.aspx), [`SortedSet`](https://msdn.microsoft.com/ja-jp/library/dd412070.aspx)</td>
	</tr>
</table>



##<a id="sec-generated-title-7"></a> <a id="usage"></a>用途による分類
###<a id="sec-generated-title-8"></a> <a id="buffer"></a>要素の挿入・削除を一定ルールで行うコレクション
<table summary="">

	<tr>
		<th>名前</th>
		<th>C++ STL</th>
		<th>C# System.Collections</th>
	</tr>
	<tr>
		<td markdown="1">「[スタック](col_stack.md)」</td>
		<td markdown="1">[`stack`](http://cpprefjp.github.io/reference/stack.html)</td>
		<td markdown="1">[`Stack`](https://msdn.microsoft.com/ja-jp/library/3278tedw.aspx)</td>
	</tr>
	<tr>
		<td markdown="1">「[待ち行列](col_queue.md)」</td>
		<td markdown="1">[`queue`](http://cpprefjp.github.io/reference/queue.html)</td>
		<td markdown="1">[`Queue`](https://msdn.microsoft.com/ja-jp/library/7977ey2c.aspx)</td>
	</tr>
	<tr>
		<td markdown="1">「[優先度付き待ち行列](col_heap.md)」</td>
		<td markdown="1">[`priority_queue`](http://cpprefjp.github.io/reference/queue/priority_queue.html)</td>
		<td markdown="1">　</td>
	</tr>
</table>



###<a id="sec-generated-title-9"></a> <a id="assoc"></a>連想コレクション
<table summary="">

	<tr>
		<th>名前</th>
		<th>C++ STL</th>
		<th>C# System.Collections</th>
	</tr>
	<tr>
		<td markdown="1">「[セット](col_set.md)」</td>
		<td markdown="1">[`set`](http://cpprefjp.github.io/reference/set.html),  [`multiset`](http://cpprefjp.github.io/reference/set/multiset.html), [`unordered_set`](http://cpprefjp.github.io/reference/unordered_set.html), [`unordered_muliset`](http://cpprefjp.github.io/reference/unordered_set/unordered_set.html)</td>
		<td markdown="1">[`HashSet`](https://msdn.microsoft.com/ja-jp/library/bb359438.aspx),　[`SortedSet`](https://msdn.microsoft.com/ja-jp/library/dd412070.aspx)</td>
	</tr>
	<tr>
		<td markdown="1">「[辞書](col_dic.md)」</td>
		<td markdown="1">[`map`](http://cpprefjp.github.io/reference/map.html), [`multimap`](http://cpprefjp.github.io/reference/map/multimap.html), [`unordered_map`](http://cpprefjp.github.io/reference/unordered_map.html), [`unordered_multimap`](http://cpprefjp.github.io/reference/unordered_map/unordered_multimap.html)</td>
		<td markdown="1">[`Dictionary`](https://msdn.microsoft.com/ja-jp/library/xfhwa508.aspx), [`SortedDictionary`](https://msdn.microsoft.com/ja-jp/library/ms132289.aspx), [`SortedList`](https://msdn.microsoft.com/ja-jp/library/ms132319.aspx)</td>
	</tr>
</table>
