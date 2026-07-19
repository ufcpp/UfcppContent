---
title: "シェルソート"
source_url: "https://ufcpp.net/study/algorithm/sort/sort_shell/"
content_type: "Article"
published_at: "2015-05-06T14:04:38"
updated_at: "2022-10-31T20:29:03"
tags: []
umbraco_id: 1122
parent_id: 1117
sort_order: 4
aliases:
  - "/algorithm/sort/sort_shell/"
  - "/algorithm/sort_shell"
  - "/algorithm/sort_shell.html"
  - "/study/algorithm/sort_shell"
  - "/study/algorithm/sort_shell.html"
---

# シェルソート

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="d24e8" class="keyword">シェルソート</strong>（Shell sort）は、
「[挿入ソート](sort_insert.md#insert)」を改良した物で、
挿入ソートの「概ねソート済みの配列に対しては高速」という性質を最大限生かすアルゴリズムです。
「[不安定](sort.md#unstable)」な「[内部](sort.md#inner)」ソート。

1. 適当な間隔 h を設定して、h 個おきのデータを挿入ソートでソートする。

2. h の間隔を狭めて 1 を繰り返す。


演算量に関しては、理論的に証明するのは非常に難しいけども、
実験的には最良の場合で O(n<sup>1.2</sup>) くらいになるらしい。
最悪の場合は挿入ソートなどと同様の O(n<sup>2</sup>)。

ちなみに、シェルソートの Shell は貝殻とかではなく、人名らしい。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=shell&i=0&s=0&w=300" width="304" height="332"></iframe></div>

## <a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/ShellSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/ShellSort.cs)

<pre class="source" title="シェルソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// シェルソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;</span>
<span class="reserved">public static void</span> ShellSort&lt;T&gt;(T[] a)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">int</span> n = a.Length;
  <span class="reserved">int</span> h;
  <span class="reserved">for</span> (h = 1; h &lt; n / 9; h = h * 3 + 1) ;
  <span class="reserved">for</span> (; h &gt; 0; h /= 3)
    <span class="reserved">for</span> (<span class="reserved">int</span> i = h; i &lt; n; i++)
      <span class="reserved">for</span> (<span class="reserved">int</span> j = i; j &gt;= h &amp;&amp; a[j - h].CompareTo(a[j]) &gt; 0; j -= h)
        Swap(<span class="reserved">ref</span> a[j], <span class="reserved">ref</span> a[j - h]);
}
</code></pre>
