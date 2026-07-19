---
title: "STL コンテナの特徴一覧"
source_url: "https://ufcpp.net/study/stl/list/characteristics/"
content_type: "Article"
published_at: "2015-05-06T14:23:46"
updated_at: "2015-05-06T14:23:46"
tags: []
umbraco_id: 1642
parent_id: 1641
sort_order: 0
aliases:
  - "/stl/characteristics"
  - "/stl/characteristics.html"
  - "/stl/list/characteristics/"
  - "/study/stl/characteristics"
  - "/study/stl/characteristics.html"
---

# STL コンテナの特徴一覧

##<a id="sec-generated-title-1"></a> <a id="d28e4"></a>STL コンテナの特徴一覧
STLに標準で用意されているコンテナの特徴の一覧です。

<table summary="">

	<tr>
		<td markdown="1" colspan="3">要素を挿入する順番に意味がある</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">ランダムアクセス可能</td>
		<td markdown="1" colspan="1">ランダムアクセス不可</td>
	</tr>
	<tr>
		<td markdown="1">末尾への挿入・削除のみ高速</td>
		<td markdown="1">先頭・末尾への挿入・削除のみ高速</td>
		<td markdown="1">任意の位置に挿入・削除が高速</td>
	</tr>
	<tr>
		<td markdown="1"><code>vector</code></td>
		<td markdown="1"><code>deque</code></td>
		<td markdown="1"><code>list</code></td>
	</tr>
</table>


<table summary="">

	<tr>
		<td markdown="1" colspan="3">要素の挿入と取り出しのみを行う</td>
	</tr>
	<tr>
		<td markdown="1">後から入れた要素を先に取り出す(FILO)</td>
		<td markdown="1">先に入れた要素を先に取り出す(FIFO)</td>
		<td markdown="1">優先度の高い物から先に取り出す</td>
	</tr>
	<tr>
		<td markdown="1"><code>stack</code></td>
		<td markdown="1"><code>queue</code></td>
		<td markdown="1"><code>priority_queue</code></td>
	</tr>
</table>


<table summary="">

	<tr>
		<td markdown="1" colspan="4">要素を挿入する順番には意味がない</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">値のみを持つ</td>
		<td markdown="1" colspan="2">キーと値を持つ</td>
	</tr>
	<tr>
		<td markdown="1">値の重複を許さない</td>
		<td markdown="1">値の重複を許す</td>
		<td markdown="1">キーの重複を許さない</td>
		<td markdown="1">キーの重複を許す</td>
	</tr>
	<tr>
		<td markdown="1"><code>set</code></td>
		<td markdown="1"><code>multiset</code></td>
		<td markdown="1"><code>map</code></td>
		<td markdown="1"><code>multimap</code></td>
	</tr>
</table>
