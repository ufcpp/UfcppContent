---
title: "vector"
source_url: "https://ufcpp.net/study/stl/seq_container/vector/"
content_type: "Article"
published_at: "2015-05-06T14:23:20"
updated_at: "2015-05-06T14:23:20"
tags: []
umbraco_id: 1631
parent_id: 1630
sort_order: 0
aliases:
  - "/study/stl/vector.html"
---

# vector

## <a id="sec-generated-title-1"></a> <a id="d20e4"></a>vectorとは

<strong id="vector" class="keyword">vector</strong>とは一言で言うと可変長配列です。
データが確保した記憶領域に収まりきらなくなったら自動で領域を拡張してくれ、
配列の長さを自動で管理してくれるます。
 
内部的な実装では、単純に配列を使って実装しています。
配列なので、末尾以外への要素の挿入には時間がかかりますが、
<code>operator[]</code>を使って自由に要素にアクセスできます。
 
vectorという言葉は数学に出てくるベクトルと同じvectorなのですが、
STLのvectorはメモリの許す限り、次元を拡張できるn次元ベクトルだと思ってください。
(まあ、むしろ数学のベクトルは想像しないほうがいいです。)


## <a id="sec-generated-title-2"></a> <a id="d20e24"></a>vectorの特徴

* ランダムアクセス(<code>[]</code>を使って添え字を指定してのアクセス)が O(1) で行え、もっとも高速

* 末尾への要素の追加、削除は O(1) で行える

* それ以外への場所の要素の追加は O(n) かかる
