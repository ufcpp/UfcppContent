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
  - "/algorithm/sort/sort_merge/"
  - "/algorithm/sort_merge"
  - "/algorithm/sort_merge.html"
  - "/study/algorithm/sort_merge"
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

<pre class="source" title="ソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// マージソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;</span>
<span class="reserved">public static void</span> MergeSort&lt;T&gt;(T[] a)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  T[] work = <span class="reserved">new</span> T[a.Length / 2];
  MergeSort(a, 0, a.Length, work);
}

<span class="comment">/// &lt;summary&gt;
/// マージソート → 挿入ソートに切り替える配列長の閾値。
/// &lt;/summary&gt;</span>
<span class="reserved">const int</span> THREASHOLD = 64;

<span class="comment">/// &lt;summary&gt;
/// 挿入ソート。
/// 配列のどこからどこまでをソートするかを指定するバージョン。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="first"&gt;ソート対象の先頭インデックス&lt;/param&gt;
/// &lt;param name="last"&gt;ソート対象の末尾インデックス&lt;/param&gt;</span>
<span class="reserved">static void</span> InsertSort&lt;T&gt;(T[] a, <span class="reserved">int</span> first, <span class="reserved">int</span> last)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">for</span> (<span class="reserved">int</span> i = first + 1; i &lt;= last; i++)
    <span class="reserved">for</span> (<span class="reserved">int</span> j = i; j &gt; first &amp;&amp; a[j - 1].CompareTo(a[j]) &gt; 0; --j)
      Swap(<span class="reserved">ref</span> a[j], <span class="reserved">ref</span> a[j - 1]);
}

<span class="comment">/// &lt;summary&gt;
/// マージソート。
/// 配列のどこからどこまでをソートするかを指定するバージョン。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="begin"&gt;ソート対象部分の先頭&lt;/param&gt;
/// &lt;param name="end"&gt;ソート対象部分の末尾＋1&lt;/param&gt;
/// &lt;param name="work"&gt;作業領域。a の 1/2 のサイズが必要。&lt;/param&gt;</span>
<span class="reserved">static void</span> MergeSort&lt;T&gt;(T[] a, <span class="reserved">int</span> begin, <span class="reserved">int</span> end, T[] work)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">if</span> (end - begin &lt; THREASHOLD)
  {
    InsertSort(a, begin, end - 1);
    <span class="reserved">return</span>;
  }

  <span class="reserved">int</span> mid = (begin + end) / 2;
  MergeSort(a, begin, mid, work);
  MergeSort(a, mid, end, work);
  Merge(a, begin, mid, end, work);
}

<span class="comment">/// &lt;summary&gt;
/// 配列 a の、[begin, mid) の部分と [mid, end) の部分をマージ。
/// &lt;/summary&gt;
/// &lt;typeparam name="T"&gt;&lt;/typeparam&gt;
/// &lt;param name="a"&gt;マージ対象の配列&lt;/param&gt;
/// &lt;param name="begin1"&gt;aの先頭&lt;/param&gt;
/// &lt;param name="mid"&gt;aの分割点&lt;/param&gt;
/// &lt;param name="end"&gt;aの末尾＋1&lt;/param&gt;
/// &lt;param name="work"&gt;作業領域&lt;/param&gt;</span>
<span class="reserved">static void</span> Merge&lt;T&gt;(T[] a, <span class="reserved">int</span> begin, <span class="reserved">int</span> mid, <span class="reserved">int</span> end, T[] work)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">int</span> i, j, k;

  <span class="reserved">for</span> (i = begin, j = 0; i != mid; ++i, ++j) work[j] = a[i];

  mid -= begin;
  <span class="reserved">for</span> (j = 0, k = begin; i != end &amp;&amp; j != mid; ++k)
  {
    <span class="reserved">if</span> (a[i].CompareTo(work[j]) &lt; 0)
    {
      a[k] = a[i];
      ++i;
    }
    <span class="reserved">else</span>
    {
      a[k] = work[j];
      ++j;
    }
  }

  <span class="reserved">for</span> (; i &lt; end; ++i, ++k) a[k] = a[i];
  <span class="reserved">for</span> (; j &lt; mid; ++j, ++k) a[k] = work[j];
}
</code></pre>
