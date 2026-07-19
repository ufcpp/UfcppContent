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

<pre class="source" title="ヒープソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// ヒープソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;</span>
<span class="reserved">public static void</span> HeapSort&lt;T&gt;(T[] a)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt; a.Length; ++i)
    MakeHeap(a, i);
  <span class="reserved">for</span> (<span class="reserved">int</span> i = a.Length - 1; i &gt;= 0; --i)
    a[i] = PopHeap(a, i);
}

<span class="comment">/// &lt;summary&gt;
/// 配列をヒープ化する。
/// n - 1 番目までの要素は既にヒープ化されていることを仮定して、
/// n 番目の要素をヒープに追加。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="n"&gt;要素数&lt;/param&gt;</span>
<span class="reserved">static void</span> MakeHeap&lt;T&gt;(T[] a, <span class="reserved">int</span> n)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">while</span> (n != 0)
  {
    <span class="reserved">int</span> i = (n - 1) / 2;
    <span class="reserved">if</span> (a[n].CompareTo(a[i]) &gt; 0) Swap(<span class="reserved">ref</span> a[n], <span class="reserved">ref</span> a[i]);
    n = i;
  }
}

<span class="comment">/// &lt;summary&gt;
/// ヒープから最大値を取り出す。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="n"&gt;要素数 - 1&lt;/param&gt;
/// &lt;returns&gt;取り出した最大値&lt;/returns&gt;</span>
<span class="reserved">static</span> T PopHeap&lt;T&gt;(T[] a, <span class="reserved">int</span> n)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  T max = a[0];

  a[0] = a[n];

  <span class="reserved">for</span> (<span class="reserved">int</span> i=0, j; (j = 2 * i + 1) &lt; n; )
  {
    <span class="reserved">if</span> ((j != n - 1) &amp;&amp; (a[j].CompareTo(a[j + 1]) &lt; 0)) j++;
    <span class="reserved">if</span> (a[i].CompareTo(a[j]) &lt; 0) Swap(<span class="reserved">ref</span> a[i], <span class="reserved">ref</span> a[j]);
    i = j;
  }

  <span class="reserved">return</span> max;
}
</code></pre>
