---
title: "2分探索木"
source_url: "https://ufcpp.net/study/algorithm/collection/col_tree/"
content_type: "Article"
published_at: "2015-05-06T14:05:11"
updated_at: "2015-07-13T13:33:31"
tags: []
umbraco_id: 1136
parent_id: 1128
sort_order: 7
aliases:
  - "/algorithm/col_tree"
  - "/algorithm/col_tree.html"
  - "/algorithm/collection/col_tree/"
  - "/study/algorithm/col_tree"
  - "/study/algorithm/col_tree.html"
---

# 2分探索木

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
一般に木構造というと、循環のない有向グラフのことなんですが、
そういう一般論はまた別の機会に話をしましょう。

ここでは、要素の挿入・削除・検索を高速に行うことの出来るコレクションのデータ構造として、
<strong id="bintree" class="keyword">2分探索木</strong>（binary search tree）というものを紹介します。
2分探索木は、以下のような特徴を持つ木構造です（図1）。

* 2分木（各ノードは最大で2本の子を持つ）。

* 全ての要素が「左の子＜親≦右の子」（あるいは「左の子≦親＜右の子」）という大小関係を満たす。


<figure>
	[![2分探索木](../../../../assets/media/ufcpp2000/algorithm/fig/col_tree0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_tree0.png)
	<figcaption>2分探索木</figcaption>
</figure>


要素の挿入・削除・検索は、
木の根から葉までの経路を1つ探索することになるので、
木の高さ分に比例する計算量が必要です。
理想的には、木のバランスが均等に整っていれば、
要素数を n として計算量は O(log n) になります。
しかしながら、逆に、
図2に示すように、木が左右どちらかに偏っている場合、
計算量は O(n) になります。

<figure>
	[![偏った2分探索木](../../../../assets/media/ufcpp2000/algorithm/fig/col_tree1.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_tree1.png)
	<figcaption>偏った2分探索木</figcaption>
</figure>



##<a id="sec-generated-title-2"></a> <a id="character"></a>特徴
2分探索木は以下のような利点を持っています。

* 理想的には、要素の挿入・削除・検索が O(log n) で行える。

* 「[ハッシュテーブル](col_hash.md#hashtable)」のように、メモリを多めに確保しておく必要がない。

* また、ハッシュ関数の作り方に悩む必要もない。

* 要素を整列された順序で取り出せる。


ただし、以下のような欠点もあります。

* 木の高さがバランスを保っていないと検索などが O(n) になる。平衡化機構が必要。


ちなみに、2分探索木への、平衡化機構の組込み方にはいくつか種類があります。
実装が簡単なのでよく用いられる物としては、
赤黒木あるいは2色木（red-black tree）と呼ばれるものがあります。


##<a id="sec-generated-title-3"></a> <a id="btimpl"></a>2分探索木の実装
まず、2分探索木も構造的には2分木なので、
以下のような左右の子を持つノードを定義します。

<pre class="source" title="" lang="">
<code><span class="reserved">public class</span> Node
{
  <span class="reserved">#region</span> フィールド

  <span class="reserved">internal</span> T val;
  <span class="reserved">internal</span> Node left, right, parent;

  <span class="reserved">internal</span> Node() : <span class="reserved">this</span>(<span class="reserved">default</span>(T), <span class="reserved">null</span>) { }

  <span class="reserved">internal</span> Node(T val, Node parent)
  {
    <span class="reserved">this</span>.val = val;
    <span class="reserved">this</span>.parent = parent;
    <span class="reserved">this</span>.left = <span class="reserved">this</span>.right = <span class="reserved">null</span>;
  }
}
</code></pre>


「[連結リスト](col_flist.md#linked)」と同様に、
<code>left</code>、 <code>right</code> 等の「[アクセスレベル](../../csharp/oop/oo_conceal.md#level)」は internal にしておきます。

そして、2分探索木には、木の根に当たるノードを持つための変数を用意します。

<pre class="source" title="" lang="">
<code><span class="reserved">class</span> BinaryTree&lt;T&gt; : IEnumerable&lt;T&gt;
  <span class="reserved">where</span> T: IComparable&lt;T&gt;
{
  Node root;
}
</code></pre>


前節で説明したような条件を満たす2分探索木中の要素を検索するには、以下のようにします。
要するに、値の大小を見て、左の子を見るか右の子を見るか決めて、
木を根から葉に向かってたどります。

<pre class="source" title="" lang="">
<code><span class="reserved">public</span> Node Find(T elem)
{
  Node n = <span class="reserved">this</span>.root;
  <span class="reserved">while</span> (n != <span class="reserved">null</span>)
  {
    <span class="reserved">if</span> (n.val.CompareTo(elem) &gt; 0) n = n.left;
    <span class="reserved">else if</span> (n.val.CompareTo(elem) &lt; 0) n = n.right;
    <span class="reserved">else break</span>;
  }
  <span class="reserved">return</span> n;
}
</code></pre>


次に、要素の挿入ですが、
とりあえず、平衡化することは考えなければ、
実装方法は簡単で、検索のときと同じ要領で木の中を探索し、
新しい葉を作ります。

<pre class="source" title="" lang="">
<code><span class="reserved">public void</span> Insert(T elem)
{
  <span class="reserved">if</span> (<span class="reserved">this</span>.root == <span class="reserved">null</span>)
  {
    <span class="reserved">this</span>.root = <span class="reserved">new</span> Node(elem, <span class="reserved">null</span>);
    <span class="reserved">return</span>;
  }

  Node n = <span class="reserved">this</span>.root;
  Node p = <span class="reserved">null</span>;
  <span class="reserved">while</span> (n != <span class="reserved">null</span>)
  {
    p = n;
    <span class="reserved">if</span> (n.val.CompareTo(elem) &gt; 0) n = n.left;
    <span class="reserved">else</span> n = n.right;
  }

  n = <span class="reserved">new</span> Node(elem, p);
  <span class="reserved">if</span> (p.val.CompareTo(elem) &gt; 0) p.left = n;
  <span class="reserved">else</span> p.right = n;
}
</code></pre>


ノードの削除も、平衡化のことを考えなければ、
以下のようにして簡単に行えます。

* 左の子が null なら、自身の位置に右の子ノードを繋ぎなおす。

* 右の子が null なら、自身の位置に左の子ノードを繋ぎなおす。

* 両方の子が非 null なら、自身の次に大きな値を持つノード（右の部分木の左端）で自身を置き換える。


<pre class="source" title="" lang="">
<code><span class="reserved">public void</span> Erase(Node n)
{
  <span class="reserved">if</span> (n == <span class="reserved">null</span>) <span class="reserved">return</span>;

  <span class="reserved">if</span> (n.left == <span class="reserved">null</span>) <span class="reserved">this</span>.Replace(n, n.right);
  <span class="reserved">else if</span>(n.right == <span class="reserved">null</span>) <span class="reserved">this</span>.Replace(n, n.left);
  <span class="reserved">else</span>
  {
    Node m = n.right.Min;
    n.Value = m.Value;
    <span class="reserved">this</span>.Replace(m, m.right);
  }
}

<span class="comment">/// &lt;summary&gt;
/// n の片方の子は null、もう片方の子は m という前提の元で、
/// ノード n の位置を子ノード m で置き換える。
/// &lt;/summary&gt;
/// &lt;param name="n"&gt;削除するノード&lt;/param&gt;
/// &lt;param name="m"&gt;置き換える子ノード&lt;/param&gt;</span>
<span class="reserved">void</span> Replace(Node n, Node m)
{
  Node p = n.parent;
  <span class="reserved">if</span> (m != <span class="reserved">null</span>) m.parent = p;
  <span class="reserved">if</span> (n == <span class="reserved">this</span>.root) <span class="reserved">this</span>.root = m;
  <span class="reserved">else if</span> (p.left == n) p.left = m;
  <span class="reserved">else</span> p.right = m;
}

<span class="reserved">public class</span> Node
{
  <span class="comment">/// &lt;summary&gt;
  /// このノード以下の部分木中で、最小の要素を持つノード（＝左端ノード）を返す。
  /// &lt;/summary&gt;</span>
  <span class="reserved">internal</span> Node Min
  {
    <span class="reserved">get</span>
    {
      Node n = <span class="reserved">this</span>;
      <span class="reserved">for</span> (; n.left != <span class="reserved">null</span>; n = n.left) ;
      <span class="reserved">return</span> n;
    }
  }
}
</code></pre>



##<a id="sec-generated-title-4"></a> <a id="sample"></a>サンプルソース
C# サンプルソースを示します。
まずは、平衡化機構のないものです。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/BinaryTree.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/BinaryTree.cs)

（ISet 「[インターフェース](../../csharp/oop/oo_interface.md#interface)」 は、
[Set.cs](../../../../assets/src/Set.cs) で定義しています。）


##<a id="sec-generated-title-5"></a> <a id="plan"></a>執筆予定
木構造の平衡化、赤黒木。
