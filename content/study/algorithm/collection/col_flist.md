---
title: "片方向連結リスト"
source_url: "https://ufcpp.net/study/algorithm/collection/col_flist/"
content_type: "Article"
published_at: "2015-05-06T14:04:58"
updated_at: "2015-07-13T13:30:49"
tags: []
umbraco_id: 1132
parent_id: 1128
sort_order: 3
aliases:
  - "/algorithm/col_flist"
  - "/algorithm/col_flist.html"
  - "/algorithm/collection/col_flist/"
  - "/study/algorithm/col_flist"
  - "/study/algorithm/col_flist.html"
---

# 片方向連結リスト

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[配列リスト](col_array.md#array)」や「[循環バッファ](col_circular.md#circular)」を使う場合、
要素の追加に柔軟に対応するためには、
実際に格納している要素の数よりも多めにメモリを確保しておく必要があります。
これに対して、<strong id="linked" class="keyword">連結リスト</strong>と呼ばれるデータ構造ならば、
常に必要な分のメモリだけ確保しておけば十分です。

連結リストには、大きく分けて片方向連結リストと両方向連結リストと呼ばれるものがあり、
ここで説明するのは前者の<strong id="flist" class="keyword">片方向連結リスト</strong>（forward linked list）です。

片方向連結リストは、図1に示すように、
ノード（node: 節）と呼ばれるデータ構造を繋いで出来たものです。
各ノードは、格納したい値 <code>Value</code> と、
次のノードへの参照（C/C++ 的にはポインター） <code>Next</code> を持っています。

<figure>
	[![片方向連結リスト](../../../../assets/media/ufcpp2000/algorithm/fig/col_flist0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_flist0.png)
	<figcaption>片方向連結リスト</figcaption>
</figure>


片方向連結リストはメモリ効率がいいという利点はあるのですが、
制約も多く、使い道は限定されています。


## <a id="sec-generated-title-2"></a> <a id="character"></a>特徴

片方向連結リストは以下のような利点を持っています。

* 「[配列リスト](col_array.md#array)」や「[循環バッファ](col_circular.md#circular)」のように、予め大きめのメモリを確保しておく必要がなく、常に要素数分のメモリだけ確保しておける。

* あるノードの直後に新しい要素を挿入する場合、一定時間（O(1)）で行える。

* あるノードの直後のノード（そのノード自身ではなく、直後）の削除を一定時間（O(1)）で行える。


ただし、以下のような欠点もあります。

* リスト中の要素にランダムアクセスできない。

* 先頭から順にしか要素にアクセスできない（逆順参照すらできない）。

* 「あるノードの直後への挿入・削除が O(1)」といっても、そのノードを探してくる操作自体は O(n)。従って、リストの先頭以外への要素の挿入・削除はそれほど得意ではない。


結局の所、「リスト先頭への要素の追加・削除が、計算量・メモリ量の両面で効率よくできる」というのが片方向連結リストの唯一の強みになります。
（この特徴から、スタックというデータ構造の実装にはよく使われます。）


## <a id="sec-generated-title-3"></a> <a id="impl"></a>実装方法

まず、ノードを実装します。
最初に説明したように、
ノードは要素を格納しておくための変数と、
次のノードを指す参照変数を持っています。

<pre class="source" title="" lang="">
<code><span class="reserved">public class</span> Node&lt;T&gt;
{
  T val;
  Node next;

  <span class="reserved">internal</span> Node(T val, Node next)
  {
    <span class="reserved">this</span>.val = val;
    <span class="reserved">this</span>.next = next;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 格納している要素を取得。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> T Value
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.val; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.val = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 次のノード。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Node Next
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.next; }
    <span class="reserved">internal set</span> { <span class="reserved">this</span>.next = value; }
  }
}
</code></pre>


ここで1つ注意しておくべきことは、
次のノードを指す「[プロパティ](../../csharp/oop/oo_property.md#property)」の <code>Next</code> の「[アクセスレベル](../../csharp/oop/oo_conceal.md#level)」は internal にしておきます。
また、ノードのコンストラクタも internal にします。
これは、ノードの連結構造を変更できるのは片方向連結リスト内からのみにするためです。
もし、リスト外からノードの連結構造を変更できた場合、
利用者の誤った操作でデータ構造が壊れてしまう可能性があります。

そして、片方向連結リスト本体ですが、
リストの先頭ノードを格納するための変数を1つ持ちます。

<pre class="source" title="" lang="">
<code><span class="reserved">public class</span> ForwardLinkedList&lt;T&gt; : IEnumerable&lt;T&gt;
{
  Node first;
}
</code></pre>


そして、リストの先頭への要素の追加・削除は以下のように行います。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 先頭に新しい要素を追加。
/// &lt;/summary&gt;
/// &lt;param name="elem"&gt;新しい要素&lt;/param&gt;
/// &lt;returns&gt;新しく挿入されたノード&lt;/returns&gt;</span>
<span class="reserved">public</span> Node InsertFirst(T elem)
{
  Node m = <span class="reserved">new</span> Node(elem, <span class="reserved">this</span>.first);
  <span class="reserved">this</span>.first = m;
  <span class="reserved">return</span> m;
}

<span class="comment">/// &lt;summary&gt;
/// 先頭の要素を削除。
/// &lt;/summary&gt;</span>
<span class="reserved">public void</span> EraseFirst()
{
  <span class="reserved">if</span> (<span class="reserved">this</span>.first != <span class="reserved">null</span>)
    <span class="reserved">this</span>.first = <span class="reserved">this</span>.first.Next;
}
</code></pre>


また、以下のようにして、ノードを与えてそのノードの直後の要素を追加・削除できます。

<pre class="source" title="" lang="">
<code>  <span class="comment">/// &lt;summary&gt;
  /// ノード n の後ろに新しい要素を追加。
  /// &lt;/summary&gt;
  /// &lt;param name="n"&gt;要素の挿入位置&lt;/param&gt;
  /// &lt;param name="elem"&gt;新しい要素&lt;/param&gt;
  /// &lt;returns&gt;新しく挿入されたノード&lt;/returns&gt;</span>
  <span class="reserved">public</span> Node InsertAfter(Node n, T elem)
{
  Node m = <span class="reserved">new</span> Node(elem, n.Next);
  n.Next = m;
  <span class="reserved">return</span> m;
}

<span class="comment">/// &lt;summary&gt;
/// ノード n の後ろの要素を削除。
/// &lt;/summary&gt;
/// &lt;param name="n"&gt;要素の削除位置&lt;/param&gt;</span>
<span class="reserved">public void</span> EraseAfter(Node n)
{
  <span class="reserved">if</span> (n.Next != <span class="reserved">null</span>)
    n.Next = n.Next.Next;
}
</code></pre>


見ての通り、これらの操作は常に一定の時間で実行可能です。
しかしながら、ノード自身を削除する場合、
そのノードの直前のノードを探す作業を行う必要があります。

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// ノード n の自身を削除。
/// &lt;/summary&gt;
/// &lt;param name="n"&gt;要素の削除位置&lt;/param&gt;
/// &lt;returns&gt;削除した要素の次のノード&lt;/returns&gt;</span>
<span class="reserved">public</span> Node Erase(Node n)
{
  Node prev = <span class="reserved">this</span>.first;
  <span class="reserved">for</span> (; prev != <span class="reserved">null</span> &amp;&amp; prev.Next != n; prev = prev.Next) ;
  <span class="reserved">if</span> (prev == <span class="reserved">this</span>.first)
  {
    <span class="reserved">this</span>.first = <span class="reserved">null</span>;
    <span class="reserved">return null</span>;
  }
  <span class="reserved">if</span> (prev != <span class="reserved">null</span>)
  {
    <span class="reserved">this</span>.EraseAfter(prev);
    <span class="reserved">return</span> prev.Next;
  }
  <span class="reserved">return null</span>;
}
</code></pre>


また、リストに含まれている要素数を求めるのも、
前から順にノードをたどって数えるしかありません。
（要素数を保持しておく変数を別に用意しておくという手はあります。）

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 要素の個数。
/// &lt;/summary&gt;</span>
<span class="reserved">public int</span> Count
{
  <span class="reserved">get</span>
  {
    <span class="reserved">int</span> i = 0;
    <span class="reserved">for</span>(Node n = <span class="reserved">this</span>.first; n!=<span class="reserved">null</span>; n=n.Next)
      ++i;
    <span class="reserved">return</span> i;
  }
}
</code></pre>


ちなみに、末尾のノードを指す変数も持っておくことで、
末尾への要素の挿入・削除も高速に行うこともできます。
ですが、先頭と末尾の両方に要素の挿入・削除をしたいときには、
片方向ではなく、両方向連結リストを使う場合が多いので、
この方法はあまり使われません。


## <a id="sec-generated-title-4"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/ForwardLinkedList.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/ForwardLinkedList.cs)
