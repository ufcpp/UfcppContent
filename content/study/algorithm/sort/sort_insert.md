---
title: "挿入ソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_insert/"
content_type: "Article"
published_at: "2015-05-06T14:04:35"
updated_at: "2022-10-31T20:28:56"
tags: []
umbraco_id: 1121
parent_id: 1117
sort_order: 3
aliases:
  - "/algorithm/sort/sort_insert/"
  - "/algorithm/sort_insert"
  - "/algorithm/sort_insert.html"
  - "/study/algorithm/sort_insert"
  - "/study/algorithm/sort_insert.html"
---

# 挿入ソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="insert" class="keyword">挿入ソート</strong>（insertion sort）は、
以下のような手順でソートを行うアルゴリズムです。
「[安定](sort.md#stable)」な「[内部](sort.md#inner)」ソート。

1. ソート済みの配列に対して要素を1つ挿入することを考える。
* 元の配列の末尾に新しい要素を付け加える。

* 配列の後ろの要素から見ていって、新しい要素よりも値が大きければ、新しい要素と順序を交換していく。

* 順序交換が必要なくなるところまで進めれば、結果もソート済みの配列になる。



2. 1の処理を、前2つの要素だけ、次は3つ、その次は4つ・・・と繰り返す。


人間が手作業で物を並び替えるのにもっともなじみやすいアルゴリズムだと言われています。
また、シンプルでかつ O(n<sup>2</sup>) のソートの中では高速な部類に入るので、
非常によく使われます。

概ねソート済みの配列に対しては高速ですが、
逆順に並んだ配列に対してはかなり低速になります。
「概ねソート済みのものに対して高速」という性質のため、
他のソートアルゴリズム（特にクイックソート）で大まかにソートして、
最後は挿入ソートを行うという使い方をされたりします。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=insert&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/InsertSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/InsertSort.cs)

<pre class="source" title="挿入ソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 挿入ソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;</span>
<span class="reserved">public static void</span> InsertSort&lt;T&gt;(T[] a)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">int</span> n = a.Length;
  <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt; n; i++)
    <span class="reserved">for</span> (<span class="reserved">int</span> j = i; j &gt;= 1 &amp;&amp; a[j - 1].CompareTo(a[j]) &gt; 0; --j )
      Swap(<span class="reserved">ref</span> a[j], <span class="reserved">ref</span> a[j - 1]);
}
</code></pre>
