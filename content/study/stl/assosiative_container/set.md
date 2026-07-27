---
title: "set, mutiset"
source_url: "https://ufcpp.net/study/stl/assosiative_container/set/"
content_type: "Article"
published_at: "2015-05-06T14:23:39"
updated_at: "2015-05-06T14:23:39"
tags: []
umbraco_id: 1639
parent_id: 1638
sort_order: 0
aliases:
  - "/study/stl/set.html"
---

# set, mutiset

## <a id="sec-generated-title-1"></a> <a id="d26e4"></a>set,multisetとは

<strong id="set" class="keyword">set</strong>は要素の重複を許さない集合、<strong id="multiset" class="keyword">multiset</strong>は要素の重複を許す集合です。
集合には次のような操作があります。
 
A,Bを集合、xを集合の要素の型とすると

1. A∋xかどうか調べる

2. A⊃B,A⊂Bかどうか調べる

3. Aにxを挿入する

4. Aからxを削除する

5. A∪Bを求める

6. A∩Bを求める

7. A-B,B-Aを求める


1,2の操作は要素の検索になります。
特に、集合に対して1の操作は頻繁に行われますから、要素の検索が高速でなければなりません。
3,4は要素の挿入・削除になります。
また、5,6,7の操作をは要素を整列済みの状態で取り出せるなら比較的容易に行うことができます。
 
要素の検索を高速に行えるデータ構造としては2分探索木、ハッシュ、整列済みの配列がありますが、
配列を常に整列された状態に保つためには挿入・削除のさいに手間がかかります。
また、ハッシュでは整列された状態で要素を順番に取り出していくことはできません。
したがってSTLではset,multisetは2分探索木を用いて実装されています。
(要素を整列された状態で取り出す必要がなければハッシュを用いるほうが効率的になります)
 
STLで2分探索木はxtreeというヘッダーファイル中に<code>tree</code>という名前のクラスとして用意されています。
この<code>tree</code>というクラスはset,multiset,map,multimapを実装するために用意されたクラスです。
したがってユーザーが直接このクラスを使う必要はありません。


## <a id="sec-generated-title-2"></a> <a id="d26e60"></a>setの特徴

* 要素の追加、検索、削除が O(log n) で行える

* 要素の重複を許さない

* 値の小さな要素から順にアクセスできる



## <a id="sec-generated-title-3"></a> <a id="d26e75"></a>multisetの特徴

* 要素の追加、検索、削除が O(log n) で行える

* 要素の重複を許す

* 値の小さな要素から順にアクセスできる
