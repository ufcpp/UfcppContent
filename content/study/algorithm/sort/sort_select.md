---
title: "選択ソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_select/"
content_type: "Article"
published_at: "2015-05-06T14:04:32"
updated_at: "2022-10-31T20:28:18"
tags: []
umbraco_id: 1120
parent_id: 1117
sort_order: 2
aliases:
  - "/study/algorithm/sort_select.html"
---

# 選択ソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="select" class="keyword">選択ソート</strong>（selection sort）は、
以下のような手順でソートを行うアルゴリズムです。
「[安定](sort.md#stable)」な「[内部](sort.md#inner)」ソート。

1. 配列の中で最小の要素を探して、先頭の要素と交換する。

2. 未整列の部分に対して、1の処理を繰り返す。


比較の回数は「[バブルソート](sort_bubble.md#bubble)」と同様に多い部類に入りますが、
要素の交換の回数は常に一定して少ないという特徴があります。
そのため、「比較は簡単だけど、要素の交換は遅い」と言うようなデータ構造に対しては比較的高速になります。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=select&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/SelectSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/SelectSort.cs)

```csharp {title="選択ソート"}
/// <summary>
/// 選択ソート。
/// </summary>
/// <param name="a">対象の配列</param>
public static void SelectSort<T>(T[] a)
  where T : IComparable<T>
{
  int n = a.Length;
  for (int i = 0; i < n; i++)
  {
    int min = i;
    for (int j = i + 1; j < n; j++)
      if (a[min].CompareTo(a[j]) > 0)
        min = j;
    Swap(ref a[i], ref a[min]);
}
```
