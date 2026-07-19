---
title: "[雑記] 内部イテレータと外部イテレータ"
source_url: "https://ufcpp.net/study/csharp/data/sp2_itpattern/"
content_type: "Article"
published_at: "2007-09-29T00:00:00"
updated_at: "2015-08-26T09:08:49"
tags:
  - "Ver. 2.0"
umbraco_id: 1301
parent_id: 1298
sort_order: 2
aliases:
  - "/csharp/data/sp2_itpattern/"
  - "/csharp/sp2_itpattern"
  - "/csharp/sp2_itpattern.html"
  - "/study/csharp/sp2_itpattern"
  - "/study/csharp/sp2_itpattern.html"
---

# \[雑記\] 内部イテレータと外部イテレータ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

コレクションの要素の列挙・反復の方法には、
内部イテレータと呼ばれる方式と外部イテレータと呼ばれる2つの方式（デザインパターン）があります。

ちなみに、列挙（enumerate）と反復（iterate）ってのは、
この分野において、
「コレクション内の全要素に対して処理する」って意味ではほぼ同義語です。
ただし、
C++ の iterator が順方向・逆方向・双方向・ランダムアクセス可能なのに対して、
C# の Enumerator が順方向アクセスしかできないので、
そのイメージに引きずられて意味を使い分ける場合もあります。


## <a id="sec-generated-title-2"></a> <a id="in_out"></a>内部イテレータと外部イテレータ

百聞は一見にしかずということで、具体例を出しつつ内部イテレータと外部イテレータについて説明しましょう。

例ということで、余計な機能は一切省いた以下のような低機能リストを考えます。

<pre class="source" title="低機能リスト" lang="">
<code><span class="reserved">public class</span> List
{
  <span class="reserved">int</span>[] items;

  <span class="reserved">public</span> List(<span class="reserved">params int</span>[] items)
  {
    <span class="reserved">this</span>.items = items;
  }

  <span class="input">後略</span>
}
</code></pre>


で、この items の中身を列挙したい場合に、アプローチが2つあると。


### <a id="sec-generated-title-3"></a> <a id="inner"></a>内部イテレータ

1つ目が内部イテレータ。
まず、List クラス内に以下のようなメソッドを用意。

<pre class="source" title="内部イテレータ的アプローチ" lang="">
<code><span class="reserved">public delegate void</span> ForEachAction(<span class="reserved">int</span> x);

<span class="reserved">public class</span> List
{
  <span class="input">前略</span>

  <span class="comment">/// &lt;summary&gt;
  /// 内部イテレータ的に、
  /// リストの各要素にたいして action を適用する。
  /// &lt;/summary&gt;
  /// &lt;param name="action"&gt;適用したい動作&lt;/param&gt;</span>
  <span class="reserved">public void</span> ForEach(ForEachAction action)
  {
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.items.Length; ++i)
    {
      action(<span class="reserved">this</span>.items[i]);
    }
  }
}
</code></pre>


で、以下のようにして使います。

<pre class="source" title="内部イテレータで反復処理" lang="">
<code>List l = <span class="reserved">new</span> List(1, 2, 3, 4, 5);

<span class="reserved">int</span> sum = 0;
l.ForEach(<span class="reserved">delegate</span>(<span class="reserved">int</span> x)
{
  sum += x;
}
);
Console.Write(<span class="literal">"sum = {0}\n"</span>, sum);
</code></pre>


要するに、反復の仕方は List クラスの中の、ForEach メソッドの中に書いて、
要素ごとに行いたい処理をデリゲートとして渡します。

ForEach の実装は簡単なんですけども、
利用側がちょっと美しくないです。
また、この方法だと、break とか continue が使えなかったりします。


### <a id="sec-generated-title-4"></a> <a id="outer"></a>外部イテレータ

もう1つが外部イテレータ。
.NET Framework がとってるアプローチはこちらです。

こちらは、実装がちょっと面倒になります。
（C# 2.0 からの新機能である「[イテレーター](sp2_iterator.md#iterator)」を使えば簡単に書けるようになりますが、ここでは説明ということで IEnumerator を自前で実装します。）

<pre class="source" title="外部イテレータ的アプローチ" lang="">
<code><span class="reserved">public class</span> List
{
  <span class="input">前略</span>

  <span class="comment">/// &lt;summary&gt;
  /// 外部イテレータ用の IEnumerator 実装クラス。
  /// &lt;/summary&gt;</span>
  <span class="reserved">class</span> Enumerator : IEnumerator&lt;<span class="reserved">int</span>&gt;
  {
    List l;
    <span class="reserved">int</span> n;

    <span class="reserved">internal</span> Enumerator(List l)
    {
      <span class="reserved">this</span>.l = l;
      <span class="reserved">this</span>.n = -1;
    }

    <span class="reserved">public int</span> Current
    {
      <span class="reserved">get</span> { <span class="reserved">return</span> l.items[n]; }
    }

    <span class="reserved">void</span> IDisposable.Dispose() { }

    <span class="reserved">object</span> System.Collections.IEnumerator.Current
    {
      <span class="reserved">get</span> { <span class="reserved">return this</span>.Current; }
    }

    <span class="reserved">bool</span> System.Collections.IEnumerator.MoveNext()
    {
      ++n;
      <span class="reserved">return</span> n != l.items.Length;
    }

    <span class="reserved">void</span> System.Collections.IEnumerator.Reset()
    {
      n = -1;
    }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 外部イテレータを返す。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;イテレータ&lt;/returns&gt;</span>
  <span class="reserved">public</span> IEnumerator&lt;<span class="reserved">int</span>&gt; GetEnumerator()
  {
    <span class="reserved">return new</span> Enumerator(<span class="reserved">this</span>);
  }
}
</code></pre>


で、利用側は以下のような感じ。

<pre class="source" title="外部イテレータで反復処理" lang="">
<code>List l = <span class="reserved">new</span> List(1, 2, 3, 4, 5);

<span class="reserved">int</span> sum = 0;
IEnumerator&lt;<span class="reserved">int</span>&gt; e = l.GetEnumerator();
<span class="reserved">while</span> (e.MoveNext())
{
  sum += e.Current;
}
Console.Write(<span class="literal">"sum = {0}\n"</span>, sum);
</code></pre>


要するに、Enumerator という別のクラスを通して items 中の要素を1つずつ取り出します。

MoveNext とか Current とかいちいち書くのが面倒ではありますが、
while を使っていて、この方が反復処理らしくはあります。
あと、ちゃんと break や continue も使えます。

ただ、IEnumerator を実装するのがものすごい面倒な作業になります。

（
あと、内部イテレータの方では、ループ1回に付き、action デリゲートが1回呼ばれるだけなのに対して、
こちらの場合はループ1回に付き MoveNext と Current （の getter）という2回のメソッド呼び出しがあります。
なので、こっちのアプローチの方が、実はほんのちょっとだけ遅い。）


## <a id="sec-generated-title-5"></a> <a id="foreach"></a>C# の foreach

前節のとおり、外部イテレータ的アプローチには2つ面倒なところがあります。

1. MoveNext とか Current とかいちいち書くのがめんどくさい

2. IEnumerator の実装がものすごい面倒


このうち、1つ目の面倒ごとを解決してくれるのが、C# の foreach 文です。
実は、前節の時点ですでに、List クラスに foreach 文を使うために必要なコードの大半を書いているので、
あとは、以下のように、IEnumerable インターフェースを実装するだけです。

<pre class="source" title="IEnumerable を実装" lang="">
<code><span class="reserved">public class</span> List : IEnumerable&lt;<span class="reserved">int</span>&gt;
{
  <span class="input">前略</span>

  IEnumerator&lt;<span class="reserved">int</span>&gt; IEnumerable&lt;<span class="reserved">int</span>&gt;.GetEnumerator()
  {
    <span class="reserved">return this</span>.GetEnumerator();
  }

  System.Collections.IEnumerator
    System.Collections.IEnumerable.GetEnumerator()
  {
    <span class="reserved">return this</span>.GetEnumerator();
  }
}
</code></pre>


これで、C# の List クラスの要素を foreach 文で列挙できるようになります。

<pre class="source" title="foreach 文" lang="">
<code>List l = <span class="reserved">new</span> List(1, 2, 3, 4, 5);

<span class="reserved">int</span> sum = 0;
<span class="reserved">foreach</span> (<span class="reserved">int</span> x <span class="reserved">in</span> l)
{
  sum += x;
}
Console.Write(<span class="literal">"sum = {0}\n"</span>, sum);
</code></pre>


まあ、利用側の見た目をすっきりさせるための構文糖（便法）みたいなもんで、
コンパイル時に、
while (e.MoveNext())
に相当するコードに変換されます。


## <a id="sec-generated-title-6"></a> <a id="iterator"></a>C# のイテレータ構文

外部イテレータの2つ目の面倒ごと、すなわち、
「IEnumerator の実装がものすごい面倒」という問題を解決するために導入されたのが、
C# 2.0 の「[イテレーター](sp2_iterator.md#iterator)」構文です。

「[外部イテレータ](#outer)」で示した GetEnumerator メソッドを、
以下のように書き換えることで、
Enumerator クラスに相当するものを自動的に生成してくれます。

<pre class="source" title="イテレータ構文を使った GetEnumerator" lang="">
<code>  <span class="comment">/// &lt;summary&gt;
  /// イテレータ構文を使って外部イテレータを自動生成。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;イテレータ&lt;/returns&gt;</span>
  <span class="reserved">public</span> IEnumerator&lt;<span class="reserved">int</span>&gt; GetEnumerator()
  {
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.items.Length; ++i)
    {
      <span class="reserved">yield return this</span>.items[i];
    }
  }
</code></pre>


見た目的には、
「[内部イテレータ](#inner)」で書いた Foreach メソッドとほとんど一緒です。
（action デリゲート呼び出しの部分が yield return に変わっただけ。）

要するに、C# 2.0 のイテレータ構文というのは、
内部イテレータ的な書き方で、
外部イテレータを自動生成するものです。
（なので、他の言語では、
これと似たような構文のことをジェネレータ（genrator: 生成するもの）と呼んだりします。
）
