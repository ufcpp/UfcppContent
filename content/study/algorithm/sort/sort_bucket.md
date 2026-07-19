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
  - "/algorithm/sort/sort_bucket/"
  - "/algorithm/sort_bucket"
  - "/algorithm/sort_bucket.html"
  - "/study/algorithm/sort_bucket"
  - "/study/algorithm/sort_bucket.html"
---

# バケットソート

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
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

##<a id="sec-generated-title-2"></a> <a id="sample"></a>サンプルソース
[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BucketSort.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Sort/BucketSort.cs)

値を本当に整数に限定するなら、
（同じ値の要素が複数ある場合、それぞれに区別がないので）
「値 x が来たら、x 番目のバケツに入れる」という操作は
「値 x の数をカウントする」という操作に置き換えることができます。
従って、バケットソートのプログラムは非常に簡単になり、
以下のようになります。

<pre class="source" title="バケットソート（int 版）" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// [0, max] の範囲の整数をバケットソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="max"&gt;配列 a 中の最大値&lt;/param&gt;</span>
<span class="reserved">public static void</span> BucketSort(<span class="reserved">int</span>[] a, <span class="reserved">int</span> max)
{
  <span class="comment">// バケツを用意</span>
  <span class="reserved">int</span>[] bucket = <span class="reserved">new int</span>[max + 1];

  <span class="comment">// バケツに値を入れる</span>
  <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i) ++bucket[a[i]];

  <span class="comment">// バケツ中の値の結合</span>
  <span class="reserved">for</span> (<span class="reserved">int</span> j = 0, i = 0; j &lt; bucket.Length; ++j)
    <span class="reserved">for</span> (<span class="reserved">int</span> k = bucket[j]; k != 0; --k, ++i)
      a[i] = j;
}
</code></pre>


これに対して、例えば、整数をキーとするデータ構造をソートするなら、
（キーが同じ値でも、キー以外のデータが異なる可能性があるので）
「キーの値が x の要素を入れるバケツ」を「[連結リスト](../collection/col_blist.md#linked)」などを使って実装する必要があります。

<pre class="source" title="バケットソート（任意のデータ構造対象）" lang="">
<code><span class="reserved">using</span> System.Collections.Generic;

<span class="comment">/// &lt;summary&gt;
/// [0, max] の範囲の整数をキーに持つデータ構造をバケットソート。
/// &lt;/summary&gt;
/// &lt;typeparam name="T"&gt;値の型&lt;/typeparam&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="max"&gt;キーの最大値&lt;/param&gt;</span>
<span class="reserved">public static void</span> BucketSort&lt;T&gt;(KeyValuePair&lt;<span class="reserved">int</span>, T&gt;[] a, <span class="reserved">int</span> max)
{
  <span class="comment">// バケツを用意</span>
  List&lt;T&gt;[] bucket = <span class="reserved">new</span> List&lt;T&gt;[max + 1];

  <span class="comment">// バケツに値を入れる</span>
  <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
  {
    <span class="reserved">if</span> (bucket[a[i].Key] == <span class="reserved">null</span>) bucket[a[i].Key] = <span class="reserved">new</span> List&lt;T&gt;();
    bucket[a[i].Key].Add(a[i].Value);
  }

  <span class="comment">// バケツ中の値の結合</span>
  <span class="reserved">for</span> (<span class="reserved">int</span> j = 0, i = 0; j &lt; bucket.Length; ++j)
   <span class="reserved">if</span>(bucket[j] != <span class="reserved">null</span>)
     <span class="reserved">foreach</span> (T val <span class="reserved">in</span> bucket[j])
       a[i++] = <span class="reserved">new</span> KeyValuePair&lt;<span class="reserved">int</span>, T&gt;(j, val);
}
</code></pre>


<code>KeyValuePair</code> や <code>List</code> は、
.NET Framework 標準ライブラリの物を使用しています。
先ほどの整数限定版と比べると、
オーバーヘッドが掛かってしまいますが、
それでもオーダーが O(n) で、他のソートアルゴリズムと比べてかなり高速です。
