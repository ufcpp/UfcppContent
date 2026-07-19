---
title: "[雑記] LINQ と遅延評価"
source_url: "https://ufcpp.net/study/csharp/data/sp3_lazylist/"
content_type: "Article"
published_at: "2015-05-06T14:11:21"
updated_at: "2015-08-26T09:17:16"
tags:
  - "Ver. 3.0"
umbraco_id: 1306
parent_id: 1298
sort_order: 7
aliases:
  - "/csharp/data/sp3_lazylist/"
  - "/csharp/sp3_lazylist"
  - "/csharp/sp3_lazylist.html"
  - "/study/csharp/sp3_lazylist"
  - "/study/csharp/sp3_lazylist.html"
---

# \[雑記\] LINQ と遅延評価

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# 2.0 の「[イテレーター](sp2_iterator.md#iterator)」や、
C# 3.0 の「[LINQ](../cheatsheet/ap_ver3.md#linq)」を使うと、lazy list のようなことが実現可能。


## <a id="sec-generated-title-2"></a> <a id="linq"></a>LINQ の動作概要

例えば、以下のようなコードを見てみましょう。

<pre class="source" title="LINQ の例" lang="">
<code><span class="reserved">int</span>[] x = {-<span class="literal">15</span>, -<span class="literal">10</span>, -<span class="literal">5</span>, <span class="literal">0</span>, <span class="literal">5</span>, <span class="literal">10</span>, <span class="literal">15</span>};
<span class="reserved">int</span> min = -<span class="literal">10</span>;
<span class="reserved">int</span> max = <span class="literal">10</span>;
<span class="reserved">var</span> y =
  <span class="reserved">from</span> p <span class="reserved">in</span> x
  <span class="reserved">where</span> min &lt;= p &amp;&amp; p &lt;= max
  <span class="reserved">select</span> p * p;

<span class="reserved">foreach</span>(<span class="reserved">var</span> p <span class="reserved">in</span> y)
{
  <span class="type">Console</span>.Write(<span class="literal">"{0}\n"</span>, p);
}
</code></pre>


int の配列 x から、
<span class="math">
        <span class="paren" style="font-size:em;">[</span>
          <span class="normal">−</span><span class="normal">10</span>, <span class="normal">10</span>
        <span class="paren" style="font-size:em;">]</span>
      </span> の範囲に入っている物だけを取り出し、
さらに、値の2乗を返しています。

<pre class="console" title="LINQ の例、実行結果">
100
25
0
25
100
</pre>


非常に抽象度の高いコードですが、
これが具体的にどういうことをしているのかを考えてみましょう。
実は、このコードの from, where, select の部分は以下のように展開されます。

<pre class="source" title="from, where, selet の展開結果" lang="">
<code>  <span class="reserved">var</span> y = x.Where(p =&gt; min &lt;= p &amp;&amp; p &lt;= max)
    .Select(q =&gt; q * q);
</code></pre>


これなら、多少は C# 2.0 の頃に見慣れたコードに近づきました。
Where や Select に引数として与えている部分は、
「[ラムダ式](../functional/sp3_lambda.md#lambda)」という機能で、
まあ、「[匿名メソッド式](../functional/sp_delegate.md#anonymous)」を簡素化したようなものです。

また、var キーワードは、右辺値の型から左辺の変数の型を推論してくれる機能です。
（参考： 「[型推論](../start/sp3_inference.md#implicit)」。）

ところで、int[] に Where や Select という名前のメソッドはありましたっけ？
実はこれらは、
「[拡張メソッド](../functional/sp3_extension.md#exmethod)」という機能を使って、
int[]に（正確に言うと、IEnumerable&lt;int&gt; に）“追加した”メソッドです。
（定義場所は System.Query.Sequence クラス内。）


## <a id="sec-generated-title-3"></a> <a id="list"></a>リスト → リスト

前節の Where や Select はリストを変形して別のリストを得るメソッドと考えることができます。
Where は特定の条件を満たす要素だけを取り出したリストを、
Select はリスト内の各要素にある操作を施したリストを作るメソッドです。

で、実際の LINQ の実装（System.Query.Sequence クラス内の実装）では、
Where や Select は IEnumerable → IEnumerable を得るメソッドなんですが、
ここでは、
比較のために、IList で同等の機能を実装してみましょう。

以下のような感じになります。
（第1引数の前に this とか付いてるのが「[拡張メソッド](../functional/sp3_extension.md#exmethod)」です。）

<pre class="source" title="Where, Select の実装（IList 版）" lang="">
<code><span class="reserved">public static class</span> <span class="type">Extensions</span>
{
  <span class="reserved">public static</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; Where(
    <span class="reserved">this</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; x,
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; f)
  {
    <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; y = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
    <span class="reserved">for</span> (<span class="reserved">int</span> i=<span class="literal">0</span>; i&lt;x.Count; ++i)
      <span class="reserved">if</span> (f(x[i]))
        y.Add(x[i]);
    <span class="reserved">return</span> y;
  }

  <span class="reserved">public static</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; Select(
    <span class="reserved">this</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; x,
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f)
  {
    <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; y = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
    <span class="reserved">for</span> (<span class="reserved">int</span> i=<span class="literal">0</span>; i&lt;x.Count; ++i)
      y.Add(f(x[i]));
    <span class="reserved">return</span> y;
  }
}
</code></pre>


で、これの動作を見るために、
以下のようなテストコードを書いてみます。
まあ、先ほどと同じ処理内容なんですが、
select の所に“重たい処理”を挟んだ上で、
実行時間を計測しています。

<pre class="source" title="Where, Select（IList 版）のテスト" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Query;

<span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static int</span> HeavyFunction(<span class="reserved">int</span> p)
  {
    <span class="comment">// 非常に重たい処理を装うために、スリープを挟んでみる</span>
    System.Threading.<span class="type">Thread</span>.Sleep(<span class="literal">100</span>);
    <span class="reserved">return</span> p * p;
  }

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="reserved">int</span>[] x = {-<span class="literal">15</span>, -<span class="literal">10</span>, -<span class="literal">5</span>, <span class="literal">0</span>, <span class="literal">5</span>, <span class="literal">10</span>, <span class="literal">15</span>};
    <span class="reserved">int</span> min = -<span class="literal">10</span>;
    <span class="reserved">int</span> max = <span class="literal">10</span>;
    <span class="type">DateTime</span> t = <span class="type">DateTime</span>.Now;

    <span class="reserved">var</span> y =
      <span class="reserved">from</span> p <span class="reserved">in</span> x
      <span class="reserved">where</span> min &lt;= p &amp;&amp; p &lt;= max
      <span class="reserved">select</span> HeavyFunction(p);

    <span class="type">TimeSpan</span> ts = <span class="type">DateTime</span>.Now - t;
    <span class="type">Console</span>.Write(<span class="literal">"{0}\n"</span>, ts.Ticks);
    t = <span class="type">DateTime</span>.Now;

    <span class="reserved">foreach</span>(<span class="reserved">var</span> p <span class="reserved">in</span> y)
    {
      ts = <span class="type">DateTime</span>.Now - t;
      <span class="type">Console</span>.Write(<span class="literal">"{0}: {1}\n"</span>, ts.Ticks, p);
      t = <span class="type">DateTime</span>.Now;
    }
  }
}

<span class="reserved">public static class</span> <span class="type">Extensions</span>
{
  <span class="reserved">public static</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; Where(
    <span class="reserved">this</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; x,
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; f)
  {
    <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; y = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
    <span class="reserved">for</span> (<span class="reserved">int</span> i=<span class="literal">0</span>; i&lt;x.Count; ++i)
      <span class="reserved">if</span> (f(x[i]))
        y.Add(x[i]);
    <span class="reserved">return</span> y;
  }

  <span class="reserved">public static</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; Select(
    <span class="reserved">this</span> <span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; x,
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f)
  {
    <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; y = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
    <span class="reserved">for</span> (<span class="reserved">int</span> i=<span class="literal">0</span>; i&lt;x.Count; ++i)
      y.Add(f(x[i]));
    <span class="reserved">return</span> y;
  }
}
</code></pre>


Extensions クラスで Where や Select を自前で定義したので、
LINQ 標準で用意されている System.Query.Sequence 内の方ではなく、
Extensions の方の Where, Select が呼び出されます。

実行結果は以下の通りです。

<pre class="console" title="Where, Select（IList 版）のテストの実行結果">
5468750
0: 100
0: 25
0: 0
0: 25
0: 100
</pre>


リスト処理なんで当たり前なんですが、
最初のリスト x から別のリスト y を得る部分で実行時間の大部分を占めています。


## <a id="sec-generated-title-4"></a> <a id="delay"></a>遅延評価

前節では、IList を使って Where および Select を実装してみました。
その結果は、元のリスト x から変形後のリスト y を得る部分で一気に処理をしていて、
残りの foreach の部分ではほとんど何もしていないという状態になりました。

これではあんまり好ましくない場合があります。
まず、この方式では、無限に続くシーケンスは処理できません。
無限に続くシーケンスというのは、IEnumerable と、
C# 2.0 の「[イテレーター](sp2_iterator.md#iterator)」機能を用いて、
例えば以下のようにして実現できる物です。

<pre class="source" title="無限シーケンス" lang="">
<code><span class="reserved">public static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; CountUp()
{
  <span class="reserved">for</span>(<span class="reserved">long</span> i = <span class="literal">0</span>; ; ++i)
    <span class="reserved">yield return</span> i;
}
</code></pre>


これで、<code>foreach(var i in CountUp())</code> とかやった日には、
冗談抜きで永久ループになります。
（foreach 中で break を入れて使えば、適当な所で処理を止めれる。）

もう1つは、
foreach で IEnumerator の MoveNext が呼ばれるたびに処理をすることで負荷を分散したい場合もあって、
これは IList 版の実装では実現できません。
IEnumerable を用いた実装が必要となります。

このことを示すために、IEnumerable 版の Where, Select を実装して、
先ほどと同様に処理の実行時間を測ってみましょう。
この IEnumerable 版実装ですが、以下のようになります。
（ちなみに、System.Query.Sequence クラス内の実装もこんな感じのはず。）

<pre class="source" title="Where, Select の実装（IEnumerable 版）" lang="">
<code><span class="reserved">public static class</span> <span class="type">Extensions</span>
{
  <span class="reserved">public static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Where(
    <span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; x,
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; f)
  {
    <span class="reserved">foreach</span> (<span class="reserved">int</span> p <span class="reserved">in</span> x)
      <span class="reserved">if</span> (f(p))
        <span class="reserved">yield return</span> p;
  }

  <span class="reserved">public static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Select(
    <span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; x,
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f)
  {
    <span class="reserved">foreach</span> (<span class="reserved">int</span> p <span class="reserved">in</span> x)
      <span class="reserved">yield return</span> f(p);
  }
}
</code></pre>


先ほどの IList 版とほとんど変わりませんね。
いかに「[イテレーター](sp2_iterator.md#iterator)」が便利な機能なのかが分かります。
イテレータなしだと、結構面倒な記述が必要になります。

で、早速実行時間を測ってみましょう。
測定用のテストコードは、Extensions クラスの中身を差し替えるだけで OK なので、
コード全体の再掲は割愛します。
実行結果は以下の通り。

<pre class="console" title="Where, Select（IList 版）のテストの実行結果">
0
1093750: 100
1093750: 25
1093750: 0
1093750: 25
1093750: 100
</pre>


この通り、見事に負荷が分散してますね。
ちゃんと、IEnumerator の MoveNext が呼ばれるたびに HeavyFunction メソッドが実行されている証拠です。
（実行時間も見事に5分の1。DateTime.Ticks ごときの精度では差が出ないみたい。）

さて、こういう風に、
必要になった要素から、必要になった分だけ計算する方法を、
<strong id="delayed" class="keyword">遅延評価</strong>（delayed evaluation）もしくは lazy な評価（lazy evaluation）といいます。
lazy というのは、不精・怠惰という意味で、
「必要になるまでやらない」という姿勢を例えたものです。

そして、遅延評価機能を持ったリストのことを lazy list （そのまま横文字で読むか、遅延リストと訳す場合が多い）と呼びます。
C# では、IEnumerable インターフェースと「[イテレーター](sp2_iterator.md#iterator)」を使って、
lazy list に似たような機能が実現できるわけです。
そして、C# 3.0 では、
「[拡張メソッド](../functional/sp3_extension.md#exmethod)」や「[クエリ式](sp3_linq.md#query)」を用いることで、
lazy list 風の操作がより簡便に行えるようになりました。


### <a id="sec-generated-title-5"></a> <a id="side_effect"></a>おまけ：値のキャッシュ

一般に、関数型言語などの言語で実装されている遅延評価機能は、
「必要になるまで計算を実行しない」というのに加えて、
「1度計算したら計算結果をキャッシュする」というような機構も持っていることが多いです。

このページで説明したような、IEnumerator を使った操作は、
「必要になるまで計算を実行しない」という意味では遅延評価的な動作になるんですが、
キャッシュ機構までは持っていません。

具体的にどういうことかというと、
2度 foreach すると2度実行されます。
特に、クエリ式中で副作用のあるコードを書く場合にはちょっと注意が必要です。
例えば、以下のようなコードを書いたとします。

<pre class="source" title="2度 foreach すると2度同じ処理が行われる" lang="">
<code><span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
{
  <span class="reserved">var</span> a =
    <span class="reserved">from</span> n <span class="reserved">in</span> <span class="type">Enumerable</span>.Range(<span class="literal">0</span>, <span class="literal">3</span>)
    <span class="reserved">select</span> SideEffect(n);

  <span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> a) ;
  <span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> a) ;
}

<span class="reserved">static int</span> SideEffect(<span class="reserved">int</span> n)
{
  <span class="type">Console</span>.Write(n);
  <span class="reserved">return</span> n;
}
</code></pre>


<pre class="console" title="実行結果">
012012
</pre>


foreach 1回につき 012 が1回表示されます。

2度も同じことが実行されるのがいやなら、
以下のようにすればいいんですが、
ToList() が呼ばれた時点で処理が実行され、
「必要になるまで計算を実行しない」というのはできなくなります。

<pre class="source" title="ToList()" lang="">
<code><span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
{
  <span class="reserved">var</span> a =
    <span class="reserved">from</span> n <span class="reserved">in</span> <span class="type">Enumerable</span>.Range(<span class="literal">0</span>, <span class="literal">3</span>)
    <span class="reserved">select</span> SideEffect(n);
  <span class="reserved">var</span> b = a.ToList();

  <span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> b) ;
  <span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> b) ;
}

<span class="reserved">static int</span> SideEffect(<span class="reserved">int</span> n)
{
  <span class="type">Console</span>.Write(n);
  <span class="reserved">return</span> n;
}
</code></pre>


<pre class="console" title="実行結果">
012
</pre>


C# で、キャッシュ機構まで持つ遅延評価がしたければ、
以下のページで紹介されているようなクラスを書く必要があります。

[Lazy Computation in C#](http://msdn2.microsoft.com/ja-jp/vcsharp/bb870976.aspx)（英語）。
