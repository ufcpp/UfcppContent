---
title: "ソート概要"
source_url: "https://ufcpp.net/study/algorithm/sort/sort/"
content_type: "Article"
published_at: "2015-05-06T14:04:28"
updated_at: "2022-10-31T20:32:15"
tags: []
umbraco_id: 1118
parent_id: 1117
sort_order: 0
aliases:
  - "/algorithm/sort.html"
  - "/algorithm/sort/sort/"
  - "/study/algorithm/sort.html"
---

# ソート概要

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

まあ、結局の所、今現在世の中で使われているソートアルゴリズムの大半は、マージソートか「[クイックソート](sort_quick.md#quick)」をベースにしたものです。
（基本的にこの2つのソートを使い、途中から「[挿入ソート](sort_insert.md#insert)」というソートに切り替えるという手法が有名。）

ですが、そこに至るまでの道筋には先人達の試行錯誤があったわけで、
その試行錯誤の中で生まれ、現在に至るまでその名を残すアルゴリズムは結構な種類存在します。
そして、アルゴリズム入門書籍・ウェブサイトでは、
その手のソートアルゴリズムが必ずといっていいほど頻繁に取り上げられています。

これは、以下のような理由で、アルゴリズム入門として記事にしやすいからでしょう。

* いろんな種類のアルゴリズムがある

* それぞれの特徴が分かりやすい・説明しやすい

* オーダーの違うアルゴリズムの圧倒的差を体感できる


ということで、このページでも様々なソートアルゴリズムについて説明したいと思います。



## <a id="sec-generated-title-2"></a> <a id="demo"></a>デモ

説明に入る前に、先にソートの様子を視覚化したデモをお見せしておきましょう。

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?i=0&s=0&w=150" width="780" height="500"></iframe></div>

([ソースコード](https://github.com/ufcpp/StaticWebApps/tree/main/BlazorWasm/SortVisualizer))


ソートの途中で、比較や入れ替えがどうおこなわれているのかという経過を表示しています。
これで大まかなイメージをつかんでから説明を読んでもらうと、理解が深まるかと思います。

## <a id="sec-generated-title-3"></a> <a id="common"></a>はじめに

まずはじめにいくつか留意点を。

「[アルゴリズムとデータ構造](../index.md)」インデックスページでも書いたように、
サンプルプログラムには 「[C#](../../csharp/abstract/ab_csharp.md#cs)」 を用います。
また、C# 2.0 の機能である「[ジェネリック](../../csharp/oop/sp2_generics.md#generics)」を使用します。

それから、ソートでは、アルゴリズムの種類を問わず、
2つの要素を入れ替えるスワップという処理をよく使用します。
なので、スワップは以下のように関数化しておきます。

<pre class="source" title="Swap" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// a と b の中身を入れ替える。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;オペランドa&lt;/param&gt;
/// &lt;param name="b"&gt;オペランドb&lt;/param&gt;</span>
<span class="reserved">public static void</span> Swap&lt;T&gt;(<span class="reserved">ref</span> T a, <span class="reserved">ref</span> T b)
{
  T c = a; a = b; b = c;
}
</code></pre>


ref というキーワードに関しては「[引数の参照渡し](../../csharp/resource/sp_ref.md)」を、
&lt;T&gt; という部分に関しては「[ジェネリック](../../csharp/oop/sp2_generics.md)」を参照してください。


## <a id="sec-generated-title-4"></a> <a id="stable"></a>安定性

数あるソートアルゴリズムを分類する方法の1つに、
安定性の有無があります。

<strong id="stable" class="keyword">安定</strong>なソート（stable sort）とは、
順序的に同等な要素が複数あったときに、その並びが元のまま保たれるもののことを言います。
そうでない場合は<strong id="unstable" class="keyword">不安定</strong>（unstable）。

整数などの、単純な数値型の配列を比較する場合には安定性の有無は問題になってきませんが、
以下のようなケースを考えて見ましょう。

具体的な例として、年齢と名前のペアを作り、その配列で名簿管理みたいなことをしたいとします。
そして、年齢の大小でソートすることを考えて見ましょう。
まず、年齢と名前のペアは Entry と言う名前で以下のように実装します。

<pre class="source" title="年齢と名前のペア" lang="">
<code><span class="reserved">class</span> Entry : IComparable&lt;Entry&gt;
{
  <span class="reserved">public int</span> age;
  <span class="reserved">public string</span> name;

  <span class="reserved">public</span> Entry(<span class="reserved">int</span> age, <span class="reserved">string</span> name)
  {
    <span class="reserved">this</span>.age = age;
    <span class="reserved">this</span>.name = name;
  }

  <span class="reserved">int</span> IComparable&lt;Entry&gt;.CompareTo(Entry other)
  {
    <span class="reserved">return this</span>.age.CompareTo(other.age);
  }
}
</code></pre>


これを使って、以下のようなリストを作ります。

<pre class="source" title="リスト" lang="">
<code>Entry[] list = <span class="reserved">new</span> Entry[]{
  <span class="reserved">new</span> Entry(10, <span class="literal">"a"</span>),
  <span class="reserved">new</span> Entry(11, <span class="literal">"b"</span>),
  <span class="reserved">new</span> Entry(12, <span class="literal">"c"</span>),
  <span class="reserved">new</span> Entry(11, <span class="literal">"d"</span>),
  <span class="reserved">new</span> Entry(13, <span class="literal">"e"</span>),
  <span class="reserved">new</span> Entry(10, <span class="literal">"f"</span>),
  <span class="reserved">new</span> Entry(12, <span class="literal">"g"</span>),
  <span class="reserved">new</span> Entry(14, <span class="literal">"h"</span>),
};
</code></pre>


この状態では、名前順に並んでいますね。
そして、10歳、11歳、12歳のエントリーがそれぞれ複数含まれています。
これを、Array.Sort メソッドを使ってソートしてみましょう。

<pre class="source" title="リストのソート" lang="">
<code>Array.Sort(list);
<span class="reserved">foreach</span> (Entry entry <span class="reserved">in</span> list)
{
  Console.Write(<span class="literal">"{0}, {1}\n"</span>, entry.age, entry.name);
} 
</code></pre>


結果は以下のようになります。

<pre class="console" title="不安定なソート結果">
10, f
10, a
11, d
11, b
12, g
12, c
13, e
14, h
</pre>


名前の順序がばらばらになっていることが分かります。
Array.Sort は、おそらく「[クイックソート](sort_quick.md#quick)」を使っている物と思われます。
クイックソートには安定性はなく、名前の順序を元のまま保てません。
もしも、これを安定なソートアルゴリズムを使ってソートするならば、
結果は以下のようになります。

<pre class="console" title="安定なソート結果">
10, a
10, f
11, d
11, b
12, c
12, g
13, e
14, h
</pre>



## <a id="sec-generated-title-5"></a> <a id="outer"></a>外部記憶の必要性

配列をソートする際に、
配列内の要素の交換だけでソートできる物を<strong id="inner" class="keyword">内部</strong>ソートと呼びます。
逆に、ソートしたい配列の他に、余分に記憶領域を確保して、
そちらに一時的にデータを保存しなければならない物を<strong id="outer" class="keyword">外部</strong>ソートと呼びます。

大半のソートアルゴリズムは内部ソートだったりします。
有名どころのうちで、例外はマージソートのみ。


## <a id="sec-generated-title-6"></a> <a id="order"></a>オーダー

ソートに限らず、アルゴリズムの良し悪しの判断基準として最も重要なのは計算量でしょう。
計算量の評価は、厳密に行うのは難しい場合も多く、
大まかな見積もりにとどめる場合もあります。

計算量の大まかな見積もり指標の1つが<strong id="order" class="keyword">オーダー</strong>です。


## <a id="sec-generated-title-7"></a> <a id="simple"></a>単純なソート

O(n<sup>2</sup>)の物。

* 「[バブルソート](sort_bubble.md)」

* 「[選択ソート](sort_select.md)」

* 「[挿入ソート](sort_insert.md)」


ちょっと高速な物。

* 「[シェルソート](sort_shell.md)」



## <a id="sec-generated-title-8"></a> <a id="rapid"></a>高速なソート

O(n lon n)の物。

* 「[クイックソート](sort_quick.md)」

* 「[ヒープソート](sort_heap.md)」

* 「[マージソート](sort_merge.md)」



## <a id="sec-generated-title-9"></a> <a id="integer"></a>整数限定のソート

範囲が予め分かっている整数に限って、
O(n) で計算できる物。
制限が強いけども、超高速。

* 「[バケットソート](sort_bucket.md)」

* 「[基数ソート](sort_radix.md)」



## <a id="sec-generated-title-10"></a> <a id="src"></a>ソースファイル

紹介したソートプログラムのソースファイルを置いておきます。

[ソースファイル](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/algorithm/src/Sort.cs)
