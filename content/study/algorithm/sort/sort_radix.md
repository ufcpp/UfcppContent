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
  - "/algorithm/sort/sort_radix/"
  - "/algorithm/sort_radix"
  - "/algorithm/sort_radix.html"
  - "/study/algorithm/sort_radix"
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

<pre class="source" title="基数ソート（概念説明用）" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 基数ソート。
/// 概念説明用の簡易版。
/// 10進数で3桁(0～999)までしかソートできない。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="max"&gt;配列 a 中の最大値&lt;/param&gt;</span>
<span class="reserved">public static void</span> RadixSort10(<span class="reserved">int</span>[] a)
{
  <span class="comment">// バケツを用意</span>
  List&lt;<span class="reserved">int</span>&gt;[] bucket = <span class="reserved">new</span> List&lt;<span class="reserved">int</span>&gt;[10];

  <span class="reserved">for</span> (<span class="reserved">int</span> d = 0, r = 1; d &lt; 3; ++d, r *= 10)
  {
    <span class="comment">// バケツに値を入れる</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">int</span> key = (a[i] / r) % 10; <span class="comment">// a[i] の d 桁目だけを取り出す。</span>
      <span class="reserved">if</span> (bucket[key] == <span class="reserved">null</span>) bucket[key] = <span class="reserved">new</span> List&lt;<span class="reserved">int</span>&gt;();
      bucket[key].Add(a[i]);
    }

    <span class="comment">// バケツ中の値の結合</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> j = 0, i = 0; j &lt; bucket.Length; ++j)
      <span class="reserved">if</span> (bucket[j] != <span class="reserved">null</span>)
        <span class="reserved">foreach</span> (<span class="reserved">int</span> val <span class="reserved">in</span> bucket[j])
          a[i++] = val;

    <span class="comment">// バケツを一度空にする</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> j = 0; j &lt; bucket.Length; ++j)
      bucket[j] = <span class="reserved">null</span>;
  }
}
</code></pre>


「バケツに値を入れる」とか「バケツ中の値の結合」の部分は、
「[バケットソート](sort_bucket.md#bucket)」と全く同じ物です。
それを、下の桁から順に3回（＝3桁）繰り返しています。

実装上、除算/剰余算は低速なので使いたくないので、
基数には256などの2の冪を使い、
除算/剰余算の代わりにシフト/マスク演算を使います。
基数を256（＝1バイト）にした場合、
32ビット整数は4桁（＝4バイト）なので、4回の反復で OK です。

<pre class="source" title="基数ソート" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 基数ソート。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;対象の配列&lt;/param&gt;
/// &lt;param name="max"&gt;配列 a 中の最大値&lt;/param&gt;</span>
<span class="reserved">public static void</span> RadixSort(<span class="reserved">int</span>[] a)
{
  <span class="comment">// バケツを用意</span>
  List&lt;<span class="reserved">int</span>&gt;[] bucket = <span class="reserved">new</span> List&lt;<span class="reserved">int</span>&gt;[256];

  <span class="reserved">for</span> (<span class="reserved">int</span> d = 0, logR = 0; d &lt; 4; ++d, logR += 8)
  {
    <span class="comment">// バケツに値を入れる</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; a.Length; ++i)
    {
      <span class="reserved">int</span> key = (a[i] &gt;&gt; logR) &amp; 255; <span class="comment">// a[i] を256進 d 桁目だけを取り出す。</span>
      <span class="reserved">if</span> (bucket[key] == <span class="reserved">null</span>) bucket[key] = <span class="reserved">new</span> List&lt;<span class="reserved">int</span>&gt;();
      bucket[key].Add(a[i]);
    }

    <span class="comment">// バケツ中の値の結合</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> j = 0, i = 0; j &lt; bucket.Length; ++j)
      <span class="reserved">if</span> (bucket[j] != <span class="reserved">null</span>)
        <span class="reserved">foreach</span> (<span class="reserved">int</span> val <span class="reserved">in</span> bucket[j])
          a[i++] = val;

    <span class="comment">// バケツを一度空にする</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> j = 0; j &lt; bucket.Length; ++j)
      bucket[j] = <span class="reserved">null</span>;
  }
}
</code></pre>


<code>KeyValuePair</code> や <code>List</code> は、
.NET Framework 標準ライブラリの物を使用しています。
