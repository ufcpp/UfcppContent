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
  - "/algorithm/sort/sort_quick/"
  - "/algorithm/sort_quick"
  - "/algorithm/sort_quick.html"
  - "/study/algorithm/sort_quick"
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

<pre class="source" title="クイックソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// クイックソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;</span>
<span class="reserved">public static void</span> QuickSort&lt;T&gt;(T[] a)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  QuickSort(a, 0, a.Length - 1);
}

<span class="comment">/// &lt;summary&gt;
/// クイックソート → 挿入ソートに切り替える配列長の閾値。
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
/// クイックソート本体。
/// 配列のどこからどこまでをソートするかを指定するバージョン。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="first"&gt;ソート対象の先頭インデックス&lt;/param&gt;
/// &lt;param name="last"&gt;ソート対象の末尾インデックス&lt;/param&gt;</span>
<span class="reserved">static void</span> QuickSort&lt;T&gt;(T[] a, <span class="reserved">int</span> first, <span class="reserved">int</span> last)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="comment">// 要素数が少なくなってきたら挿入ソートに切り替え</span>
  <span class="reserved">if</span> (last - first &lt; THREASHOLD)
  {
    InsertSort(a, first, last);
    <span class="reserved">return</span>;
  }

  <span class="comment">// 枢軸決定（配列の先頭、ど真ん中、末尾の3つの値の中央値を使用。）</span>
  T pivot = Median(a[first], a[(first + last) / 2], a[last]);

  <span class="comment">// 左右分割</span>
  <span class="reserved">int</span> l = first;
  <span class="reserved">int</span> r = last;

  <span class="reserved">while</span>(l &lt;= r)
  {
    <span class="reserved">while</span> (l &lt; last &amp;&amp; a[l].CompareTo(pivot) &lt; 0) l++;
    <span class="reserved">while</span> (r &gt; first &amp;&amp; a[r].CompareTo(pivot) &gt;= 0) r--;
    <span class="reserved">if</span> (l &gt; r) <span class="reserved">break</span>;
    Swap(<span class="reserved">ref</span> a[l], <span class="reserved">ref</span> a[r]);
    l++; r--;
  }

  <span class="comment">// 再帰呼び出し</span>
  QuickSort(a, first, l-1);
  QuickSort(a, l, last);
}

<span class="comment">/// &lt;summary&gt;
/// 3つの値の中央値を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;オペランドa&lt;/param&gt;
/// &lt;param name="b"&gt;オペランドb&lt;/param&gt;
/// &lt;param name="c"&gt;オペランドc&lt;/param&gt;
/// &lt;returns&gt;中央値&lt;/returns&gt;</span>
<span class="reserved">static</span> T Median&lt;T&gt;(T a, T b, T c)
  <span class="reserved">where</span> T : IComparable&lt;T&gt;
{
  <span class="reserved">if</span> (a.CompareTo(b) &gt; 0) Swap(<span class="reserved">ref</span> a, <span class="reserved">ref</span> b);
  <span class="reserved">if</span> (a.CompareTo(c) &gt; 0) Swap(<span class="reserved">ref</span> a, <span class="reserved">ref</span> c);
  <span class="reserved">if</span> (b.CompareTo(c) &gt; 0) Swap(<span class="reserved">ref</span> b, <span class="reserved">ref</span> c);
  <span class="reserved">return</span> b;
}
</code></pre>
