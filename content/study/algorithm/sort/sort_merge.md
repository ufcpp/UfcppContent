---
title: "マージソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_merge/"
content_type: "Article"
published_at: "2015-05-06T14:04:44"
updated_at: "2022-10-31T20:30:00"
tags: []
umbraco_id: 1125
parent_id: 1117
sort_order: 7
aliases:
  - "/study/algorithm/sort_merge.html"
---

# マージソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

マージ（merge: 併合、吸収）とは、
2つのソート済み配列を、1つのソート済み配列にまとめる操作のことを言います。
そして、<strong id="merge" class="keyword">マージソート</strong>（merge sort）は分割統治法に基づく、
以下のようなアルゴリズムです。

1. 配列を2つに分割。

2. 分けた配列を再帰的にマージソート。

3. 2つのソート済み配列をマージ。


平均・最悪ともに計算量 O(n log n) の高速なソートです。
高速なソートの中では唯一「[安定](sort.md#stable)」なアルゴリズムであるという利点があるのですが、「[外部](sort.md#outer)」ソート（配列長の 1/2 のサイズの余分な領域が必要）になってしまうという欠点もあります。

また、配列にランダムアクセスする（ランダムな順序 <code>i</code> で配列の要素 <code>a[i]</code> にアクセス）必要がないため、
シーケンシャル（前から順番）アクセスしかできない連結リスト構造に対しても使用できるという利点もあります。
連結リストに対してマージソートを用いる場合、
余分な領域を確保する必要がなくなるので、
連結リストに対するソートといえばマージソートになります。

計算量 O(n log n) のソートの中では遅い部類に入り、
安定性が必要な場合や、連結リストに対するソート以外ではあまり利用されません。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=merge&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/MergeSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/MergeSort.cs)

```csharp {title="ソート"}
/// <summary>
/// マージソート。
/// </summary>
/// <param name="a">対象の配列</param>
public static void MergeSort<T>(T[] a)
  where T : IComparable<T>
{
  T[] work = new T[a.Length / 2];
  MergeSort(a, 0, a.Length, work);
}

/// <summary>
/// マージソート → 挿入ソートに切り替える配列長の閾値。
/// </summary>
const int THREASHOLD = 64;

/// <summary>
/// 挿入ソート。
/// 配列のどこからどこまでをソートするかを指定するバージョン。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="first">ソート対象の先頭インデックス</param>
/// <param name="last">ソート対象の末尾インデックス</param>
static void InsertSort<T>(T[] a, int first, int last)
  where T : IComparable<T>
{
  for (int i = first + 1; i <= last; i++)
    for (int j = i; j > first && a[j - 1].CompareTo(a[j]) > 0; --j)
      Swap(ref a[j], ref a[j - 1]);
}

/// <summary>
/// マージソート。
/// 配列のどこからどこまでをソートするかを指定するバージョン。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="begin">ソート対象部分の先頭</param>
/// <param name="end">ソート対象部分の末尾＋1</param>
/// <param name="work">作業領域。a の 1/2 のサイズが必要。</param>
static void MergeSort<T>(T[] a, int begin, int end, T[] work)
  where T : IComparable<T>
{
  if (end - begin < THREASHOLD)
  {
    InsertSort(a, begin, end - 1);
    return;
  }

  int mid = (begin + end) / 2;
  MergeSort(a, begin, mid, work);
  MergeSort(a, mid, end, work);
  Merge(a, begin, mid, end, work);
}

/// <summary>
/// 配列 a の、[begin, mid) の部分と [mid, end) の部分をマージ。
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="a">マージ対象の配列</param>
/// <param name="begin1">aの先頭</param>
/// <param name="mid">aの分割点</param>
/// <param name="end">aの末尾＋1</param>
/// <param name="work">作業領域</param>
static void Merge<T>(T[] a, int begin, int mid, int end, T[] work)
  where T : IComparable<T>
{
  int i, j, k;

  for (i = begin, j = 0; i != mid; ++i, ++j) work[j] = a[i];

  mid -= begin;
  for (j = 0, k = begin; i != end && j != mid; ++k)
  {
    if (a[i].CompareTo(work[j]) < 0)
    {
      a[k] = a[i];
      ++i;
    }
    else
    {
      a[k] = work[j];
      ++j;
    }
  }

  for (; i < end; ++i, ++k) a[k] = a[i];
  for (; j < mid; ++j, ++k) a[k] = work[j];
}
```
