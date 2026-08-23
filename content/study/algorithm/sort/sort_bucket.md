---
title: "バケットソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_bucket/"
content_type: "Article"
published_at: "2015-05-06T14:04:46"
updated_at: "2022-10-31T20:30:19"
tags: []
umbraco_id: 1126
parent_id: 1117
sort_order: 8
aliases:
  - "/study/algorithm/sort_bucket.html"
---

# バケットソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

ソート対象となるデータの種類を仮定しない場合、
（ほとんどの場面で）最速のソートは「[クイックソート](sort_quick.md#quick)」です。
あるいは、「[安定](sort.md#stable)」が必要な場合、
「[マージソート](sort_merge.md#merge)」が使われます。
これらはいずれも計算量 O(n log n) のソートですが、
対象データの種類を仮定しない物ではこの  O(n log n) がオーダーの限界です。

しかしながら、
データの種類を

* 整数（少なくとも、ソート順を決めるキーが整数）

* 整数値の範囲が予め分かっていて、それほど大きくない


という特殊な場合に限定するならば、
計算量 O(n) でソートが可能です。

<strong id="bucket" class="keyword">バケットソート</strong>（bucket sort）は、
このような限定的な条件下で O(n) を実現できるソートの1つです。

バケットソートと呼ばれていますが、bucket というのはバケツ（要は、入れ物）のことです。
ソート対象の整数値が分かっているなら、
まず、その値の数だけバケツを用意します。
そして、

1. 値 x が来たら、x 番目のバケツに入れる。

2. 全ての値を入れ終わったら、バケツに入った値を前から順に繋ぐ。


という操作で、ソートが行えます。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=bucket&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BucketSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BucketSort.cs)

値を本当に整数に限定するなら、
（同じ値の要素が複数ある場合、それぞれに区別がないので）
「値 x が来たら、x 番目のバケツに入れる」という操作は
「値 x の数をカウントする」という操作に置き換えることができます。
従って、バケットソートのプログラムは非常に簡単になり、
以下のようになります。

```csharp {title="バケットソート（int 版）"}
/// <summary>
/// [0, max] の範囲の整数をバケットソート。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="max">配列 a 中の最大値</param>
public static void BucketSort(int[] a, int max)
{
  // バケツを用意
  int[] bucket = new int[max + 1];

  // バケツに値を入れる
  for (int i = 0; i < a.Length; ++i) ++bucket[a[i]];

  // バケツ中の値の結合
  for (int j = 0, i = 0; j < bucket.Length; ++j)
    for (int k = bucket[j]; k != 0; --k, ++i)
      a[i] = j;
}
```


これに対して、例えば、整数をキーとするデータ構造をソートするなら、
（キーが同じ値でも、キー以外のデータが異なる可能性があるので）
「キーの値が x の要素を入れるバケツ」を「[連結リスト](../collection/col_blist.md#linked)」などを使って実装する必要があります。

```csharp {title="バケットソート（任意のデータ構造対象）"}
using System.Collections.Generic;

/// <summary>
/// [0, max] の範囲の整数をキーに持つデータ構造をバケットソート。
/// </summary>
/// <typeparam name="T">値の型</typeparam>
/// <param name="a">対象の配列</param>
/// <param name="max">キーの最大値</param>
public static void BucketSort<T>(KeyValuePair<int, T>[] a, int max)
{
  // バケツを用意
  List<T>[] bucket = new List<T>[max + 1];

  // バケツに値を入れる
  for (int i = 0; i < a.Length; ++i)
  {
    if (bucket[a[i].Key] == null) bucket[a[i].Key] = new List<T>();
    bucket[a[i].Key].Add(a[i].Value);
  }

  // バケツ中の値の結合
  for (int j = 0, i = 0; j < bucket.Length; ++j)
   if(bucket[j] != null)
     foreach (T val in bucket[j])
       a[i++] = new KeyValuePair<int, T>(j, val);
}
```


<code>KeyValuePair</code> や <code>List</code> は、
.NET Framework 標準ライブラリの物を使用しています。
先ほどの整数限定版と比べると、
オーバーヘッドが掛かってしまいますが、
それでもオーダーが O(n) で、他のソートアルゴリズムと比べてかなり高速です。
