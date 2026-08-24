---
title: "基数ソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_radix/"
content_type: "Article"
published_at: "2015-05-06T14:04:48"
updated_at: "2015-07-13T13:28:32"
tags: []
umbraco_id: 1127
parent_id: 1117
sort_order: 9
aliases:
  - "/study/algorithm/sort_radix.html"
---

# 基数ソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[バケットソート](sort_bucket.md#bucket)」は、
値の範囲が限られた整数限定で、計算量 O(n) で極めて高速にソートを行えるアルゴリズムでした。
ですが、「値の範囲が限られた」が曲者で、用途が非常に限定されてしまいます。

<strong id="radix" class="keyword">基数ソート</strong>（radix sort）は、
この欠点を少しでもマシにした、
「[バケットソート](sort_bucket.md#bucket)」の改良版ともいえるアルゴリズムです。

基数（radix）というのは、10進数の10、16進数の16というように、
桁上がりの基準になる数のことです。
基数ソートの発想は、要するに、
「桁ごとに「[バケットソート](sort_bucket.md#bucket)」を繰り返す」というものです。
そうすることで、必要となるバケツの数を基数分（10進数で1桁ずつソートするなら10個で OK）に抑えられます。

基数ソートでも、もちろん、
ソートできる値の桁数に制限が生じますが、
コンピュータ上で扱える整数の桁なんてたかが知れている
（例えば、32ビット整数で、10進数10桁）
ので、
実質上は、整数なら値の範囲を気にせず、計算量 O(n) でソートができます。


## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/RadixSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/RadixSort.cs)

概念説明のために、
まずは基数を10として、3桁までしかソートできない簡易版のソースを示します。

```csharp {title="基数ソート（概念説明用）"}
/// <summary>
/// 基数ソート。
/// 概念説明用の簡易版。
/// 10進数で3桁(0～999)までしかソートできない。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="max">配列 a 中の最大値</param>
public static void RadixSort10(int[] a)
{
  // バケツを用意
  List<int>[] bucket = new List<int>[10];

  for (int d = 0, r = 1; d < 3; ++d, r *= 10)
  {
    // バケツに値を入れる
    for (int i = 0; i < a.Length; ++i)
    {
      int key = (a[i] / r) % 10; // a[i] の d 桁目だけを取り出す。
      if (bucket[key] == null) bucket[key] = new List<int>();
      bucket[key].Add(a[i]);
    }

    // バケツ中の値の結合
    for (int j = 0, i = 0; j < bucket.Length; ++j)
      if (bucket[j] != null)
        foreach (int val in bucket[j])
          a[i++] = val;

    // バケツを一度空にする
    for (int j = 0; j < bucket.Length; ++j)
      bucket[j] = null;
  }
}
```


「バケツに値を入れる」とか「バケツ中の値の結合」の部分は、
「[バケットソート](sort_bucket.md#bucket)」と全く同じ物です。
それを、下の桁から順に3回（＝3桁）繰り返しています。

実装上、除算/剰余算は低速なので使いたくないので、
基数には256などの2の冪を使い、
除算/剰余算の代わりにシフト/マスク演算を使います。
基数を256（＝1バイト）にした場合、
32ビット整数は4桁（＝4バイト）なので、4回の反復で OK です。

```csharp {title="基数ソート"}
/// <summary>
/// 基数ソート。
/// </summary>
/// <param name="a">対象の配列</param>
/// <param name="max">配列 a 中の最大値</param>
public static void RadixSort(int[] a)
{
  // バケツを用意
  List<int>[] bucket = new List<int>[256];

  for (int d = 0, logR = 0; d < 4; ++d, logR += 8)
  {
    // バケツに値を入れる
    for (int i = 0; i < a.Length; ++i)
    {
      int key = (a[i] >> logR) & 255; // a[i] を256進 d 桁目だけを取り出す。
      if (bucket[key] == null) bucket[key] = new List<int>();
      bucket[key].Add(a[i]);
    }

    // バケツ中の値の結合
    for (int j = 0, i = 0; j < bucket.Length; ++j)
      if (bucket[j] != null)
        foreach (int val in bucket[j])
          a[i++] = val;

    // バケツを一度空にする
    for (int j = 0; j < bucket.Length; ++j)
      bucket[j] = null;
  }
}
```


<code>KeyValuePair</code> や <code>List</code> は、
.NET Framework 標準ライブラリの物を使用しています。
