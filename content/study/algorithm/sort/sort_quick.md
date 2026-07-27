---
title: "クイックソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_quick/"
content_type: "Article"
published_at: "2015-05-06T14:04:40"
updated_at: "2022-10-31T20:29:24"
tags: []
umbraco_id: 1123
parent_id: 1117
sort_order: 5
aliases:
  - "/study/algorithm/sort_quick.html"
---

# クイックソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="quick" class="keyword">クイックソート</strong>（quick sort）は、
名前に quick なんて単語を入れるだけあって、
大半の状況下で最速となるソートアルゴリズムです。
「[不安定](sort.md#unstable)」な「[内部](sort.md#inner)」ソート。

いわゆる、分割統治法的な考え方に基づいて、
大まかにソート → 配列を2つに分割という処理を再帰的に繰り返します。

1. 配列の中からある適当な数（pivot: 中心、軸。枢軸と訳す。）を選ぶ。

2. 配列を左右両端から見ていって、左側では枢軸よりも値の大きい物を、右側では枢軸よりも小さい物を探す。

3. 2 で探した左の値と右の値を入れ替える。

4. 3 を繰り返し適用し終えた時点で、配列の左側には枢軸以下の要素が、右側にはそれ以上の要素が集まる。

5. 左側の部分と右側の部分に対して、再帰的に同様の処理を繰り返す。


平均計算量的には O(n log n) で、
他の O(n log n) ソートアルゴリズムに比べてもかなり高速ですが、
ワーストケースでは計算量が O(n<sup>2</sup>) になってしまうという欠点もあります。
枢軸要素の選び方次第では、
ソート済みの配列に対してクイックソートアルゴリズムを適用するという分かりやすい状況下でワーストケースに陥ってしまいます。

しかしながら、長年の歴史を経て、
ワーストケースに陥らないための問題回避策も研究されていて、
ほぼどんなデータに対しても O(n log n) に近い性能が発揮できるような改良版が考案されています。

また、分割がある程度短くなったときに、「[挿入ソート](sort_insert.md#insert)」（O(n<sup>2</sup>) だけど、短い配列に対してはきわめて高速）に切り替えるという手法により、更なる高速化が図れます。
このような様々な工夫を施すことで、
安定性を気にする必要のない場面においては最高速のソートが得られます。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=quick&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/QuickSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/QuickSort.cs)

```csharp
/// <summary>
/// クイックソート。
/// </summary>
/// <param name="a">対象の配列</param>
public static void QuickSort<T>(T[] a)
  where T : IComparable<T>
{
  QuickSort(a, 0, a.Length - 1);
}

/// <summary>
/// クイックソート → 挿入ソートに切り替える配列長の閾値。
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
/// クイックソート本体。
/// 配列のどこからどこまでをソートするかを指定するバージョン。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="first">ソート対象の先頭インデックス</param>
/// <param name="last">ソート対象の末尾インデックス</param>
static void QuickSort<T>(T[] a, int first, int last)
  where T : IComparable<T>
{
  // 要素数が少なくなってきたら挿入ソートに切り替え
  if (last - first < THREASHOLD)
  {
    InsertSort(a, first, last);
    return;
  }

  // 枢軸決定（配列の先頭、ど真ん中、末尾の3つの値の中央値を使用。）
  T pivot = Median(a[first], a[(first + last) / 2], a[last]);

  // 左右分割
  int l = first;
  int r = last;

  while(l <= r)
  {
    while (l < last && a[l].CompareTo(pivot) < 0) l++;
    while (r > first && a[r].CompareTo(pivot) >= 0) r--;
    if (l > r) break;
    Swap(ref a[l], ref a[r]);
    l++; r--;
  }

  // 再帰呼び出し
  QuickSort(a, first, l-1);
  QuickSort(a, l, last);
}

/// <summary>
/// 3つの値の中央値を求める。
/// </summary>
/// <param name="a">オペランドa</param>
/// <param name="b">オペランドb</param>
/// <param name="c">オペランドc</param>
/// <returns>中央値</returns>
static T Median<T>(T a, T b, T c)
  where T : IComparable<T>
{
  if (a.CompareTo(b) > 0) Swap(ref a, ref b);
  if (a.CompareTo(c) > 0) Swap(ref a, ref c);
  if (b.CompareTo(c) > 0) Swap(ref b, ref c);
  return b;
}
```
