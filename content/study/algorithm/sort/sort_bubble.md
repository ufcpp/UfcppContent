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
  - "/algorithm/sort/sort_bubble/"
  - "/algorithm/sort_bubble"
  - "/algorithm/sort_bubble.html"
  - "/study/algorithm/sort_bubble"
  - "/study/algorithm/sort_bubble.html"
---

# バブルソート

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<strong id="bubble" class="keyword">バブルソート</strong>（bubble sort）というのは、
ソートの中でも最も単純な部類に入るアルゴリズムで、
たいていの教科書ではソートの章の1番最初に出てきます。
プログラムは単純ですが、比較回数・要素の交換回数ともに多く、低速です。
「[安定](sort.md#stable)」な「[内部](sort.md#inner)」ソート。

空気の泡が水中をゆっくり登っていくように、
値の小さい要素から順に配列の前の方に移動していくさまからこのような名前が付いています。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=bubble&i=0&s=0&w=300" width="304" height="332"></iframe></div>

##<a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース
[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BubbleSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BubbleSort.cs)

<pre class="source" title="バブルソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// バブルソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;</span>
<span class="reserved">public static void</span> BubbleSort&lt;T&gt;(T[] a)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">int</span> n = a.Length;
  <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; n - 1; i++)
    <span class="reserved">for</span> (<span class="reserved">int</span> j = n - 1; j &gt; i; j--)
      <span class="reserved">if</span> (a[j].CompareTo(a[j - 1]) &lt; 0)
        Swap(<span class="reserved">ref</span> a[j], <span class="reserved">ref</span> a[j - 1]);
}
</code></pre>
