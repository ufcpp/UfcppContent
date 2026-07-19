---
title: "配列リスト"
source_url: "https://ufcpp.net/study/algorithm/collection/col_array/"
content_type: "Article"
published_at: "2015-05-06T14:04:53"
updated_at: "2018-03-25T09:26:18"
tags: []
umbraco_id: 1130
parent_id: 1128
sort_order: 1
aliases:
  - "/algorithm/col_array"
  - "/algorithm/col_array.html"
  - "/algorithm/collection/col_array/"
  - "/study/algorithm/col_array"
  - "/study/algorithm/col_array.html"
---

# 配列リスト

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[配列](../../csharp/structured/st_array.md#array)」も、
同じ型のデータを複数集めた物ですから、
コレクションの一種です。
実際、C# の配列は <code>System.Collections.ICollection</code> という「[インターフェース](../../csharp/oop/oo_interface.md#interface)」を実装していて、コレクションとして扱うことができます。

ですが、単なる配列の場合、
コレクションとして使うには幾分か機能不足な部分があります。

* 最初に配列を作ったときに指定した長さ以上の要素を持てない。

* 新しい要素の挿入・削除ができない。


ということで、
配列に

* 要素数が足りなくなったら自動的に配列を確保しなおす。

* 新しい要素の挿入・削除。


という機能を追加したコレクションクラスを作ることがあります。

このような機能を持つクラスは、ライブラリによって名前がまちまちで、
C++ の STL では 「[vector](../../stl/seq_container/vector.md#vector)」、
C# （.NET Framework）では System.Collections.ArrayList や System.Collections.Generic.List&lt;T&gt; という名前になっています。
ここでは「<strong id="array" class="keyword">配列リスト</strong>」と呼ぶことにしましょう。


## <a id="sec-generated-title-2"></a> <a id="character"></a>特徴

配列リストは以下のような利点を持っています。

* 実装が単純（作るのが楽というだけではなく、動作が高速）。

* 「[インデクサー](../../csharp/oop/oo_indexer.md#indexer)」を使った要素へのランダムアクセスが、配列とほとんど変わらない速度（もちろんオーダーは O(1)）でできる。


しかしながら、所詮中身は配列なので、
以下のような欠点が残っています。

* 末尾以外への要素の挿入・削除が低速（要素数を n として平均 O(n)）。



## <a id="sec-generated-title-3"></a> <a id="impl"></a>実装方法

配列リストは、
単純に配列をラッピングするだけなので、実装が非常に簡単です。
まず、配列と、（確保した長さとは別に）実際に入っている要素の数を保持する変数を用意します。

<pre class="source" title="" lang="">
<code><span class="reserved">class</span> ArrayList&lt;T&gt; : IEnumerable&lt;T&gt;
{
  T[] data;
  <span class="reserved">int</span> count;
}
</code></pre>


配列は、図1に示すように、実際に格納されている要素数（Count）よりも大きめに確保しておきます。

<pre class="source" title="" lang="">
<code><span class="reserved">public</span> ArrayList(<span class="reserved">int</span> capacity)
{
  <span class="reserved">this</span>.data = <span class="reserved">new</span> T[capacity];
  <span class="reserved">this</span>.count = 0;
}
</code></pre>


<figure>
	[![大きめに配列を確保](../../../../assets/media/ufcpp2000/algorithm/fig/col_array0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_array0.png)
	<figcaption>大きめに配列を確保</figcaption>
</figure>


要素を追加していって、
配列の長さが足りなくなったら、配列を確保しなおします。
新たに確保しなおす配列の長さをどうするかは自由に決めれますが、
2倍ずつ長くする場合が多いです。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 配列を確保しなおす。
/// &lt;/summary&gt;
/// &lt;remarks&gt;
/// 配列長は2倍ずつ拡張していきます。
/// &lt;/remarks&gt;</span>
<span class="reserved">void</span> Extend()
{
  T[] data = <span class="reserved">new</span> T[<span class="reserved">this</span>.data.Length * 2];
  <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.data.Length; ++i) data[i] = <span class="reserved">this</span>.data[i];
  <span class="reserved">this</span>.data = data;
}
</code></pre>


中身は単なる配列なので、
末尾以外の位置の要素の挿入・削除は、
要素を1つずつずらして隙間を空ける/埋める作業が必要になります。
したがって、末尾以外への要素の挿入・削除は非常に低速です。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// i 番目の位置に新しい要素を追加。
/// &lt;/summary&gt;
/// &lt;param name="i"&gt;追加位置&lt;/param&gt;
/// &lt;param name="elem"&gt;追加する要素&lt;/param&gt;</span>
<span class="reserved">public void</span> Insert(<span class="reserved">int</span> i, T elem)
{
  <span class="reserved">if</span> (<span class="reserved">this</span>.count &gt;= <span class="reserved">this</span>.data.Length)
    <span class="reserved">this</span>.Extend();

  <span class="reserved">for</span> (<span class="reserved">int</span> n = <span class="reserved">this</span>.count; n &gt; i; --n)
  {
    <span class="reserved">this</span>.data[n] = <span class="reserved">this</span>.data[n - 1];
  }
  <span class="reserved">this</span>.data[i] = elem;
  ++<span class="reserved">this</span>.count;
}

<span class="comment">/// &lt;summary&gt;
/// i 番目の要素を削除。
/// &lt;/summary&gt;
/// &lt;param name="i"&gt;削除位置&lt;/param&gt;</span>
<span class="reserved">public void</span> Erase(<span class="reserved">int</span> i)
{
  <span class="reserved">for</span> (<span class="reserved">int</span> n = i; n &lt; <span class="reserved">this</span>.count - 1; ++n)
  {
    <span class="reserved">this</span>.data[n] = <span class="reserved">this</span>.data[n + 1];
  }
  --<span class="reserved">this</span>.count;
}
</code></pre>



## <a id="sec-generated-title-4"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/ArrayList.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/ArrayList.cs)
