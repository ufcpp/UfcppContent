---
title: "コレクション"
source_url: "https://ufcpp.net/study/dotnet/bcl/bcl_collection/"
content_type: "Article"
published_at: "2012-01-30T00:00:00"
updated_at: "2015-05-06T14:14:11"
tags: []
umbraco_id: 1389
parent_id: 1385
sort_order: 3
aliases:
  - "/dotnet/bcl/bcl_collection/"
  - "/dotnet/bcl_collection"
  - "/dotnet/bcl_collection.html"
  - "/study/dotnet/bcl_collection"
  - "/study/dotnet/bcl_collection.html"
---

# コレクション

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

コレクション（collection: データの「集まり」）を管理する方法には色々な種類があって、
それぞれ一長一短あります（目的に応じて使い分けます）。

.NET Framework の標準ライブラリにも、色々なコレクションを表すクラスが用意されています。
ここでは、どのコレクションをどういう場面で使えばいいのか、
ある程度勘所をつかめるように、
それぞれの挙動について簡単に説明していきます。


## <a id="sec-generated-title-2"></a> <a id="classification"></a>大まかな分類

コレクションには、大きく分けると3系統のものがあります。

ここで紹介するコレクションは、いずれも System.Collections.Generic 名前空間で定義されています。

* 「[リスト系](#sec-list)」

* 「[セット系](#sec-set)」

* 「[辞書系](#sec-dictionary)」



## <a id="sec-generated-title-3"></a> <a id="sec-list"></a>リスト系

挿入した順序に意味があるコレクションです。
インデックスを使った（i 番目の要素の）読み書きができたり、
挿入した順序通りに要素を取り出せたりするタイプです。

List&lt;T&gt;, LinkedList&lt;T&gt;, Stack&lt;T&gt;, Queue&lt;T&gt; があります。


### <a id="sec-generated-title-4"></a> <a id="List"></a>List&lt;T&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* インデックスを使って要素を読み書きする場合に使います
    * 特に、ランダム アクセスが必要な場合



</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* インデックスを使った読み書きは O(1)

* 末尾以外への要素の挿入や削除は O(n)

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[配列リスト](bcl_collection_algorithm.md#array-list)」です
    * C++から来た人は名前で混乱しがちですが、C++でいうとvectorです



* あらかじめ大き目の配列を確保しておきます

* 要素が増えて、サイズが足りなくなったらより大きな配列を確保しなおします

</td>
	</tr>
</table>



### <a id="sec-generated-title-5"></a> <a id="LinkedList"></a>LinkedList&lt;T&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* 事前に大き目の領域を確保したくない場合や、要素数が大きく変わるので事前確保の量を決めにくい場合に使います

</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* このノードの後ろに要素を追加したいというような、位置が事前にわかっている場合の挿入や削除はどこでも O(1)

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[連結リスト](bcl_collection_algorithm.md#linked-list)」です

* {値、前のノードへの参照、後ろのノードへの参照} という情報を持つ「ノード」（node: 節）をつないで作ります

</td>
	</tr>
</table>



### <a id="sec-generated-title-6"></a> <a id="Stack"></a>Stack&lt;T&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* いわゆる LIFO（last in first out）です
    * 後に入れた要素を先に出す

    * 要素を上に積み上げていく（一番上をどけないと、次の要素を取り出せない）ので、 スタック（stack: 積み荷）と呼ぶ



</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 要素の出し入れ（末尾にしかできません）は O(1)

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[配列リスト](bcl_collection_algorithm.md#array-list)」です

</td>
	</tr>
</table>



### <a id="sec-generated-title-7"></a> <a id="Queue"></a>Queue&lt;T&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* いわゆる FIFO（first in first out）です
    * 先に入れた要素を先に出す

    * 後ろに並んで、前から出ていくので、 キュー（queue: 待ち行列）と呼ぶ



</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 要素の出し入れ（末尾への挿入、先頭の削除）は O(1)

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[循環バッファー](bcl_collection_algorithm.md#circular-buffer)」です

* 事前に確保した領域で足りなくなった場合に配列の再確保が発生する点は、「[配列リスト](bcl_collection_algorithm.md#array-list)」と同じです

</td>
	</tr>
</table>



## <a id="sec-generated-title-8"></a> <a id="sec-set"></a>セット系

数学的な意味での集合（set）は、順序に意味を持ちません。
要素を含んでいるかどうかということにだけ意味があります。

プログラミング的にいうと、順序は狂っても構わないので、
要素の検索だけ高速にできてほしい場合があって、
そういうコレクションのことをセット（set: もちろん、数学的な意味での集合のこと）と呼びます。

HashSet&lt;T&gt;, SortedSet&lt;T&gt; があります。


### <a id="sec-generated-title-9"></a> <a id="HashSet"></a>HashSet&lt;T&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* メモリに余裕がある場合にはもっとも高速なセットです

* 要素の型は、GetHashCode メソッドを正しく定義している必要があります

* 要素の順序保証はありません

</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 事前に大きな領域を確保できれば、ほぼ O(1) で挿入・検索・削除可能

* 逆に、確保した領域目いっぱいに要素が詰まり出すと、最悪の場合、O(n) になります

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[ハッシュ テーブル](bcl_collection_algorithm.md#hash-table)」です

</td>
	</tr>
</table>



### <a id="sec-generated-title-10"></a> <a id="SortedSet"></a>SortedSet&lt;T&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* 事前に大き目の領域を確保したくない場合や、要素数が大きく変わるので事前確保の量を決めにくい場合に使います

* 要素の大小によって整列した状態で要素を列挙できます

</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 要素の挿入・検索・削除は O(log n) です

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[二分探索ツリー](bcl_collection_algorithm.md#binary-tree)」です

</td>
	</tr>
</table>



## <a id="sec-generated-title-11"></a> <a id="sec-dictionary"></a>辞書系

キーを指定して値を検索する必要がある場合に使うコレクションです。

「キーと値のペア」を要素とするセットを作ればいいので、内部的なアルゴリズムはセットと同じになります。
逆にいうと、セットは、値のない（キーのみの）辞書と考えることもできます。

Dictionary&lt;TKey, TValue&gt;, SortedDictionary&lt;TKey, TValue&gt;, SortedList&lt;TKey, TValue&gt; があります。


### <a id="sec-generated-title-12"></a> <a id="Dictionary"></a>Dictionary&lt;TKey, TValue&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* HashSet の辞書版

* メモリに余裕がある場合にはもっとも高速なセットです

* 要素の型は、GetHashCode メソッドを正しく定義している必要があります

* キーによる順序保証はありません

</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 事前に大きな領域を確保できれば、ほぼ O(1) で挿入・検索・削除可能

* 逆に、確保した領域目いっぱいに要素が詰まり出すと、最悪の場合、O(n) になります

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[ハッシュ テーブル](bcl_collection_algorithm.md#hash-table)」です

</td>
	</tr>
</table>



### <a id="sec-generated-title-13"></a> <a id="SortedDictionary"></a>SortedDictionary&lt;TKey, TValue&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* SortedSet の辞書版

* 事前に大き目の領域を確保したくない場合や、要素数が大きく変わるので事前確保の量を決めにくい場合に使います

* 要素の大小によって整列した状態で要素を列挙できます

</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 要素の挿入・検索・削除は O(log n) です

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 「[二分探索ツリー](bcl_collection_algorithm.md#binary-tree)」です

</td>
	</tr>
</table>



### <a id="sec-generated-title-14"></a> <a id="SortedList"></a>SortedList&lt;TKey, TValue&gt;

<table summary="">

	<tr>
		<th>用途</th>
		<td markdown="1">
* 要素の挿入・削除よりも、検索の方が圧倒的に多い場合に使います

</td>
	</tr>
	<tr>
		<th>計算量</th>
		<td markdown="1">
* 要素の挿入・削除は O(n)、検索は O(log n)

</td>
	</tr>
	<tr>
		<th>内部実装</th>
		<td markdown="1">
* 整列済み配列の「[二分探索](bcl_collection_algorithm.md#binary-search)」です

* 配列は、「[配列リスト](bcl_collection_algorithm.md#array-list)」と同様の再確保を行います

</td>
	</tr>
</table>



## <a id="sec-generated-title-15"></a> <a id="plan"></a>追記予定

その他、特殊系
```text
BitArray

Concurrent 系

ObservableCollection も？
    
```
