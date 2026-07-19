---
title: "セット"
source_url: "https://ufcpp.net/study/algorithm/collection/col_set/"
content_type: "Article"
published_at: "2015-05-06T14:05:19"
updated_at: "2015-07-13T13:35:30"
tags: []
umbraco_id: 1140
parent_id: 1128
sort_order: 11
aliases:
  - "/algorithm/col_set"
  - "/algorithm/col_set.html"
  - "/algorithm/collection/col_set/"
  - "/study/algorithm/col_set"
  - "/study/algorithm/col_set.html"
---

# セット

## <a id="sec-generated-title-1"></a> <a id="set"></a>セット

数学などにおいては、
「集合（set）」というと、
要素を包含するかどうかだけが問題で、
挿入された順序等は意味を持ちません。

ということで、要素の順序は関係ないという状況下で、
要素の挿入・削除・検索を高速で行えるようなコレクションを<strong id="set" class="keyword">セット</strong>（set）と呼びましょう。
（「集合」だとコレクションと紛らわしいので、英語のままにしておきます。
ちなみに、数学用語的には、collection は「集まり」と訳します。）

「[ソート済み配列](col_sorted.md#sorted)」、
「[ハッシュテーブル](col_hash.md#hashtable)」、
「[2分探索木](col_tree.md#bintree)」等は全てこの要件を満たしています。
要するに、これらはいずれもセットと呼ぶに値する機能を持っています。
そこで、セットは、以下のような「[インターフェース](../../csharp/oop/oo_interface.md#interface)」として定義します。

```csharp
/// <summary>
/// セット。
/// 数学で「集合」と呼ぶ奴。
/// 要素の順序には意味がなくて、要素が含まれているかどうかだけが問題。
/// </summary>
/// <typeparam name="T">要素の型</typeparam>
interface ISet<T> : IEnumerable<T>
{
  /// <summary>
  /// 新しい要素の挿入。
  /// </summary>
  /// <param name="elem">新しい要素</param>
  void Insert(T elem);

  /// <summary>
  /// 要素の削除。
  /// </summary>
  /// <param name="elem">削除したい要素</param>
  void Erase(T elem);

  /// <summary>
  /// 要素を含むかどうか。
  /// </summary>
  /// <param name="elem">検索したい要素</param>
  /// <returns>見つかった場合 true</returns>
  bool Contains(T elem);
}
```


「[ソート済み配列](col_sorted.md#sorted)」、
「[ハッシュテーブル](col_hash.md#hashtable)」、
「[2分探索木](col_tree.md#bintree)」は、
この ISet インターフェースを実装します。


## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Set.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Set.cs)
