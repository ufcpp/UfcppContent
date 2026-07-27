---
title: "STL メソッド一覧"
source_url: "https://ufcpp.net/study/stl/list/methods/"
content_type: "Article"
published_at: "2015-05-06T14:23:48"
updated_at: "2015-05-06T14:23:48"
tags: []
umbraco_id: 1643
parent_id: 1641
sort_order: 1
aliases:
  - "/study/stl/methods.html"
---

# STL メソッド一覧

## <a id="sec-generated-title-1"></a> <a id="d29e4"></a>注意

以下の説明において:

```csharp
n   :負でない整数
t   :要素のインスタンス
i,j :input iterator
p,q :要素のイテレータ
X   :コンテナのクラス名
x   :コンテナのインスタンス
N   :コンテナのサイズ
```



## <a id="sec-generated-title-2"></a> <a id="d29e13"></a>コンテナ内で定義された型

<table summary="">

	<tr>
		<th></th>
		<th>vector,deque</th>
		<th>list</th>
		<th>set,multiset</th>
		<th>map,multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>key_type</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>mapped_type</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>key_compare</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>reference</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>const_reference</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>iterator</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>const_iterator</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>reverse_iterator</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>const_reverse_iterator</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>size_type</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>defference_type</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>value_type</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>value_compare</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>allocator_type</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>pointer</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>const_pointer</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1">イテレータ-の種類</td>
		<td markdown="1">RandomAccess</td>
		<td markdown="1">Bidirectional</td>
		<td markdown="1">Bidirectional</td>
		<td markdown="1">Bidirectional</td>
	</tr>
</table>



## <a id="sec-generated-title-3"></a> <a id="d29e384"></a>・可能な操作とそのオーダー

<h3>コンストラクタ/デストラクタ/コピー/交換</h3>
```csharp
n    :負でない整数
t    :要素のインスタンス
i,j  :input iterator
X    :コンテナのクラス名
x    :コンテナのインスタンス
comp :比較オブジェクト
alloc:アロケーター
```


<table summary="">

	<tr>
		<th></th>
		<th>vector,deque,list</th>
		<th>set,multiset,map,multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>X(alloc)</code></td>
		<td markdown="1">○</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>X(comp,alloc)</code></td>
		<td markdown="1"></td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>X(i,j,alloc)</code></td>
		<td markdown="1">○</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>X(i,j,comp,alloc)</code></td>
		<td markdown="1"></td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>X(x)</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>~X()</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>operator=(x)</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>assign(i,j)</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>assign(n,t)</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>swap(x)</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>get_allocator()</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
</table>

<h3>イテレータ</h3>
すべてのコンテナに共通

<table summary="">

	<tr>
		<td markdown="1"><code>begin()</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>end()</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>rbegin()</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>rend()</code></td>
	</tr>
</table>

<h3>辞書式比較</h3>
すべてのコンテナに共通

```csharp
x:コンテナのインスタンス
```


<table summary="">

	<tr>
		<td markdown="1"><code>operator==(x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>operator!=(x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>operator&lt;(x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>operator&gt;(x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>operator&lt;=(x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>operator&gt;=(x)</code></td>
	</tr>
</table>

<h3>要素数/領域確保</h3>
```csharp
n   :負でない整数
t   :要素のインスタンス
```


<table summary="">

	<tr>
		<th></th>
		<th>vector</th>
		<th>deque,list</th>
		<th>set,multiset,map,multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>size()</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>max_size()</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>empty()</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>resize(n,t)</code></td>
		<td markdown="1">○</td>
		<td markdown="1">○</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>capacity()</code></td>
		<td markdown="1">○</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>reserv(n)</code></td>
		<td markdown="1">○</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
</table>

<h3>参照</h3>
```csharp
n   :負でない整数
N   :コンテナのサイズ
key :連想コンテナのキー
```


<table summary="">

	<tr>
		<th></th>
		<th>vector</th>
		<th>deque</th>
		<th>list</th>
		<th>set</th>
		<th>multiset</th>
		<th>map</th>
		<th>multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>front()</code></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>back()</code></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>at(n)</code></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>x[n]</code></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>x[key]</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">O(logN)</td>
		<td markdown="1"></td>
	</tr>
</table>

<h3>挿入</h3>
```csharp
n   :負でない整数
t   :要素のインスタンス
i,j :input iterator
p,q :要素のイテレータ
N   :コンテナのサイズ
```


<table summary="">

	<tr>
		<th></th>
		<th>vector</th>
		<th>deque</th>
		<th>list</th>
		<th>set</th>
		<th>multiset</th>
		<th>map</th>
		<th>multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>insert(p,t)</code></td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>insert(p,n,t)</code></td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>insert(p,i,j)</code></td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>insert(t)</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>push_front()</code></td>
		<td markdown="1"></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>push_back()</code></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
</table>

<h3>削除</h3>
```csharp
p,q :要素のイテレータ
N   :コンテナのサイズ
key :連想コンテナのキー
```


<table summary="">

	<tr>
		<th></th>
		<th>vector</th>
		<th>deque</th>
		<th>list</th>
		<th>set</th>
		<th>multiset</th>
		<th>map</th>
		<th>multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>erase(p)</code></td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>erase(p,q)</code></td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>erase(key)</code></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>clear()</code></td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
		<td markdown="1">O(N)</td>
	</tr>
	<tr>
		<td markdown="1"><code>pop_front()</code></td>
		<td markdown="1"></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>pop_back()</code></td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1">O(1)</td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1"></td>
	</tr>
</table>

<h3>検索</h3>
```csharp
N   :コンテナのサイズ
key :連想コンテナのキー
```


<table summary="">

	<tr>
		<th></th>
		<th>set,multiset,map,multimap</th>
	</tr>
	<tr>
		<td markdown="1"><code>find(key)</code></td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>count(key)</code></td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>lower_bound(key)</code></td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>upper_bound(key)</code></td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>equallrange(key)</code></td>
		<td markdown="1">O(logN)</td>
	</tr>
	<tr>
		<td markdown="1"><code>key_compare()</code></td>
		<td markdown="1">○</td>
	</tr>
	<tr>
		<td markdown="1"><code>value_compare</code></td>
		<td markdown="1">○</td>
	</tr>
</table>

<h3>リスト操作</h3>
```csharp
t   :要素のインスタンス
i,j :input iterator
p,q :要素のイテレータ
x   :コンテナのインスタンス
pred:述語(真理値を返す関数オブジェクト)
comp:比較オブジェクト
```


<table summary="">

	<tr>
		<td markdown="1"><code>splice(p,x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>splice(p,x,i)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>splice(p,x,i,j)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>remove(t)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>remove(pred)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>unique()</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>unique(pred)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>marge(x)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>marge(x,comp)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>sort()</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>sort(comp)</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>reverse()</code></td>
	</tr>
</table>
