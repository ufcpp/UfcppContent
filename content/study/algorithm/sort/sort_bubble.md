---
title: "バブルソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_bubble/"
content_type: "Article"
published_at: "2015-05-06T14:04:29"
updated_at: "2022-10-31T20:27:52"
tags: []
umbraco_id: 1119
parent_id: 1117
sort_order: 1
aliases:
  - "/study/algorithm/sort_bubble.html"
---

# バブルソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="bubble" class="keyword">バブルソート</strong>（bubble sort）というのは、
ソートの中でも最も単純な部類に入るアルゴリズムで、
たいていの教科書ではソートの章の1番最初に出てきます。
プログラムは単純ですが、比較回数・要素の交換回数ともに多く、低速です。
「[安定](sort.md#stable)」な「[内部](sort.md#inner)」ソート。

空気の泡が水中をゆっくり登っていくように、
値の小さい要素から順に配列の前の方に移動していくさまからこのような名前が付いています。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=bubble&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BubbleSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BubbleSort.cs)

```csharp {title="バブルソート"}
/// <summary>
/// バブルソート。
/// </summary>
/// <param name="a">対象の配列</param>
public static void BubbleSort<T>(T[] a)
  where T : IComparable<T>
{
  int n = a.Length;
  for (int i = 0; i < n - 1; i++)
    for (int j = n - 1; j > i; j--)
      if (a[j].CompareTo(a[j - 1]) < 0)
        Swap(ref a[j], ref a[j - 1]);
}
```
