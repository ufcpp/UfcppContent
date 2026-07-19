---
title: "循環バッファ"
source_url: "https://ufcpp.net/study/algorithm/collection/col_circular/"
content_type: "Article"
published_at: "2015-05-06T14:04:55"
updated_at: "2015-07-13T13:30:18"
tags: []
umbraco_id: 1131
parent_id: 1128
sort_order: 2
aliases:
  - "/algorithm/col_circular"
  - "/algorithm/col_circular.html"
  - "/algorithm/collection/col_circular/"
  - "/study/algorithm/col_circular"
  - "/study/algorithm/col_circular.html"
---

# 循環バッファ

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[配列リスト](col_array.md#array)」は、
要素へのランダムアクセスが非常に高速という利点がある一方、
末尾以外への要素の挿入が極端に遅いという欠点がありました。
用途によってはこれでも十分なのですが、
ある用途においては、末尾だけでなく先頭にも要素の挿入・削除を行いたい場合があります。

そこで、先頭と末尾の要素の挿入・削除を高速に（オーダー O(1) で）行えるデータ構造として、
<strong id="circular" class="keyword">循環バッファ</strong>（circular buffer）という物が考えられています。
循環バッファはリングバッファ（ring buffer）等とも呼ばれます。

循環バッファは、その名前の通り、
配列の先頭と末尾を繋いだ環のようなイメージのデータ構造です（図1）。

<figure>
	[![循環バッファ](../../../../assets/media/ufcpp2000/algorithm/fig/col_circular0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_circular0.png)
	<figcaption>循環バッファ</figcaption>
</figure>



##<a id="sec-generated-title-2"></a> <a id="character"></a>特徴
循環バッファは以下のような利点を持っています。

* 「[配列リスト](col_array.md#array)」と同様に、高速な（オーダー O(1)）ランダムアクセスが可能。

* 先頭および末尾への要素の追加・削除が一定時間（O(1)）で行える。


ただし、以下のような欠点もあります。

* オーダーは O(1) でも、オーバーヘッドが生じるため、「[配列リスト](col_array.md#array)」と比べると少しランダムアクセスが遅い。

* 先頭と末尾以外への要素の挿入・削除は相変わらず遅い。



##<a id="sec-generated-title-3"></a> <a id="impl"></a>実装方法
配列の先頭と末尾を環のように繋ぐというのをどうやって実装するかと言うと、
答えは簡単で、

<pre class="source" title="" lang="">
<code>data[i % data.Length]
</code></pre>


というように、
配列長による剰余演算でアクセスする位置 <code>i</code> をクリッピングします。

そして、先頭と末尾の両方への要素の挿入・削除を高速に行うために、
要素が入っている先頭位置を表すメンバー変数 <code>top</code> と、
末尾位置を表す <code>bottom</code> を用意します。

<pre class="source" title="" lang="">
<code>T[] data;
<span class="reserved">int</span> top, bottom;
</code></pre>


先頭から i 番目の要素へのアクセスは以下のようにして行います。

<pre class="source" title="" lang="">
<code><span class="reserved">public</span> T <span class="reserved">this</span>[<span class="reserved">int</span> i]
{
  <span class="reserved">get</span>
  {
    <span class="reserved">return this</span>.data[(i + <span class="reserved">this</span>.top) % <span class="reserved">this</span>.data.Length];
  }
  <span class="reserved">set</span>
  {
    <span class="reserved">this</span>.data[(i + <span class="reserved">this</span>.top) % <span class="reserved">this</span>.data.Length] = value;
  }
}
</code></pre>


ただ、一般に、剰余演算は遅い演算（四則演算の中ではダントツで遅い）なので、
極力避けたいものです。
配列長 <code>data.Length</code> が2の冪（<span class="math">
        2<sup>n</sup>
      </span> の形で表される数）のときには、以下のように、
剰余演算を論理 AND 演算に置き換えることができるので、
配列長を2の冪に限定してこの方法を使って循環バッファを実装するのが一般的です。

<pre class="source" title="" lang="">
<code><span class="reserved">public</span> CircularBuffer(<span class="reserved">int</span> capacity)
{
  capacity = Pow2((<span class="reserved">uint</span>)capacity);
  <span class="reserved">this</span>.data = <span class="reserved">new</span> T[capacity];
  <span class="reserved">this</span>.top = <span class="reserved">this</span>.bottom = 0;
  <span class="reserved">this</span>.mask = capacity - 1;
}

<span class="reserved">static int</span> Pow2(<span class="reserved">uint</span> n)
{
  --n;
  <span class="reserved">int</span> p = 0;
  <span class="reserved">for</span> (; n != 0; n &gt;&gt;= 1) p = (p &lt;&lt; 1) + 1;
  <span class="reserved">return</span> p + 1;
}

<span class="reserved">public</span> T <span class="reserved">this</span>[<span class="reserved">int</span> i]
{
  <span class="reserved">get</span>
  {
    <span class="reserved">return this</span>.data[(i + <span class="reserved">this</span>.top) &amp; <span class="reserved">this</span>.mask];
  }
  <span class="reserved">set</span>
  {
    <span class="reserved">this</span>.data[(i + <span class="reserved">this</span>.top) &amp; <span class="reserved">this</span>.mask] = value;
  }
}
</code></pre>


先頭への要素の挿入・削除は以下のように行います。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 先頭に新しい要素を追加。
/// &lt;/summary&gt;
/// &lt;param name="elem"&gt;追加する要素&lt;/param&gt;</span>
<span class="reserved">public void</span> InsertFirst(T elem)
{
  <span class="reserved">if</span> (<span class="reserved">this</span>.Count &gt;= <span class="reserved">this</span>.data.Length - 1)
    <span class="reserved">this</span>.Extend();

  <span class="reserved">this</span>.top = (<span class="reserved">this</span>.top - 1) &amp; <span class="reserved">this</span>.mask;
  <span class="reserved">this</span>.data[<span class="reserved">this</span>.top] = elem;
}

<span class="comment">/// &lt;summary&gt;
/// 先頭の要素を削除。
/// &lt;/summary&gt;</span>
<span class="reserved">public void</span> EraseFirst()
{
  <span class="reserved">this</span>.top = (<span class="reserved">this</span>.top + 1) &amp; <span class="reserved">this</span>.mask;
}
</code></pre>


末尾に関しては以下の通りです。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 末尾に新しい要素を追加。
/// &lt;/summary&gt;
/// &lt;param name="elem"&gt;追加する要素&lt;/param&gt;</span>
<span class="reserved">public void</span> InsertLast(T elem)
{
  <span class="reserved">if</span> (<span class="reserved">this</span>.Count &gt;= <span class="reserved">this</span>.data.Length - 1)
    <span class="reserved">this</span>.Extend();

  <span class="reserved">this</span>.data[<span class="reserved">this</span>.bottom] = elem;
  <span class="reserved">this</span>.bottom = (<span class="reserved">this</span>.bottom + 1) &amp; <span class="reserved">this</span>.mask;
}

<span class="comment">/// &lt;summary&gt;
/// 末尾の要素を削除。
/// &lt;/summary&gt;</span>
<span class="reserved">public void</span> EraseLast()
{
  <span class="reserved">this</span>.bottom = (<span class="reserved">this</span>.bottom - 1) &amp; <span class="reserved">this</span>.mask;
}
</code></pre>


見ての通り、いずれも要素数に関係なく、一定時間で挿入・削除が可能です。


##<a id="sec-generated-title-4"></a> <a id="sample"></a>サンプルソース
C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/CircularBuffer.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/CircularBuffer.cs)
