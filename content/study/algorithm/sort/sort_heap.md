---
title: "ヒープソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_heap/"
content_type: "Article"
published_at: "2015-05-06T14:04:42"
updated_at: "2022-10-31T20:29:42"
tags: []
umbraco_id: 1124
parent_id: 1117
sort_order: 6
aliases:
  - "/algorithm/sort/sort_heap/"
  - "/algorithm/sort_heap"
  - "/algorithm/sort_heap.html"
  - "/study/algorithm/sort_heap"
  - "/study/algorithm/sort_heap.html"
---

# ヒープソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

詳しくは「[優先度付き待ち行列](../collection/col_heap.md)」で説明しますが、
ヒープというのは、
常に最大の要素を取り出せる状態に保たれているデータ構造です。

常に最大の要素を取り出せるなら、当然それをソートアルゴリズムに転用できます。
ということで、
そのヒープ構造を使ったソートアルゴリズムを<strong id="heap" class="keyword">ヒープソート</strong>（heap sort）と言います。
「[不安定](sort.md#unstable)」な「[内部](sort.md#inner)」ソート。

しかしながら、平均的なケースにおいては、クイックソートの方が高速ですが、
平均計算量も最悪計算量もどちらも O(n log n) となり、
常に安定して高速です。
この点に関しては「[クイックソート](sort_quick.md#quick)」よりも優れています。

ただし、クイックソートも、最悪のケースに陥らないような改良策がいろいろ考えられているので、
ヒープソートの方がクイックソート（の改良版）よりも高速になる場面はそれほどありません。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=heap&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/HeapSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/HeapSort.cs)

```csharp
/// <summary>
/// ヒープソート。
/// </summary>
/// <param name="a">対象の配列</param>
public static void HeapSort<T>(T[] a)
  where T : IComparable<T>
{
  for (int i = 1; i < a.Length; ++i)
    MakeHeap(a, i);
  for (int i = a.Length - 1; i >= 0; --i)
    a[i] = PopHeap(a, i);
}

/// <summary>
/// 配列をヒープ化する。
/// n - 1 番目までの要素は既にヒープ化されていることを仮定して、
/// n 番目の要素をヒープに追加。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="n">要素数</param>
static void MakeHeap<T>(T[] a, int n)
  where T : IComparable<T>
{
  while (n != 0)
  {
    int i = (n - 1) / 2;
    if (a[n].CompareTo(a[i]) > 0) Swap(ref a[n], ref a[i]);
    n = i;
  }
}

/// <summary>
/// ヒープから最大値を取り出す。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="n">要素数 - 1</param>
/// <returns>取り出した最大値</returns>
static T PopHeap<T>(T[] a, int n)
  where T : IComparable<T>
{
  T max = a[0];

  a[0] = a[n];

  for (int i=0, j; (j = 2 * i + 1) < n; )
  {
    if ((j != n - 1) && (a[j].CompareTo(a[j + 1]) < 0)) j++;
    if (a[i].CompareTo(a[j]) < 0) Swap(ref a[i], ref a[j]);
    i = j;
  }

  return max;
}
```
