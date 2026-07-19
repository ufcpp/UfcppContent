---
title: "[サンプル] 式木からクエリ式の再構築"
source_url: "https://ufcpp.net/study/csharp/sample/sp3_linqreconstruct/"
content_type: "Article"
published_at: "2007-07-13T00:00:00"
updated_at: "2015-05-06T14:13:16"
tags: []
umbraco_id: 1364
parent_id: 1359
sort_order: 4
aliases:
  - "/csharp/sample/sp3_linqreconstruct/"
  - "/csharp/sp3_linqreconstruct"
  - "/csharp/sp3_linqreconstruct.html"
  - "/study/csharp/sp3_linqreconstruct"
  - "/study/csharp/sp3_linqreconstruct.html"
---

# \[サンプル\] 式木からクエリ式の再構築

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「[標準クエリ演算子（クエリ式関係）](../data/sp3_stdquery.md)」などで説明しているように、
LINQ クエリ式はメソッド（あるいは拡張メソッド）呼び出しに変換されます。
例えば、以下のような式は、

<pre class="source" title="クエリ式の例" lang="">
<code><span class="reserved">var</span> q =
  <span class="reserved">from</span> x <span class="reserved">in</span> list
  <span class="reserved">where</span> x &gt; 0
  <span class="reserved">select</span> x;
</code></pre>


以下のようなメソッド呼び出しに変換されます。

<pre class="source" title="その変換結果" lang="">
<code><span class="reserved">var</span> q = list.Where(x =&gt; x &gt; 0);
</code></pre>


ここでは、この逆をやってみようという話をします。
すなわち、
list.Where(...) から from x in list where ... というクエリ式を再構築します。


##<a id="sec-generated-title-2"></a> <a id="cause"></a>ことの発端
まず、ことの発端は以下のような問題を LINQ で書いてみようという話題がネット上で起きたこと。

<blockquote markdown="1">
* Baker, Cooper, Fletcher, MillerとSmithは五階建てアパートの異なる階に住んでいる。

* Bakerは最上階に住むのではない。

* Cooperは最下階に住むのではない。

* Fletcherは最上階にも最下階にも住むのではない。

* MillerはCooperより上の階に住んでいる。

* SmithはFletcherの隣の階に住むのではない。

* FletcherはCooperの隣の階に住むのではない。


それぞれはどの階に住んでいるか。

</blockquote>
これは総当たりで解く問題です。
要するに、総当たり問題を LINQ でどう書くかという話題。

この問題の場合、総当たりの対象が5人いるので、
5重ループ（クエリ式の場合には5重の from）が必要です。

まず、多少なりとも結果の式の見栄えを良くするために、
以下のような補助関数を用意。

<pre class="source" title="補助関数" lang="">
<code><span class="comment">// 1～5</span>
<span class="reserved">static</span> IEnumerable&lt;<span class="reserved">int</span>&gt; five = Enumerable.Range(1, 5);

<span class="comment">// x の要素に重複がないとき true</span>
<span class="reserved">static bool</span> Distinct(<span class="reserved">params int</span>[] x)
{
  <span class="reserved">return</span> x.Distinct().Count() == x.Length;
}

<span class="comment">// x, y が隣り合う数字でないとき true</span>
<span class="reserved">static bool</span> Discrete(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
{
  <span class="reserved">return</span> Math.Abs(<span class="reserved">checked</span>(x - y)) != 1;
}
</code></pre>


これを使って先ほどの問題を解くクエリ式を書くと、以下のような感じ。

<pre class="source" title="クエリ式で総当たり探索" lang="">
<code><span class="reserved">var</span> answers1 =
  <span class="reserved">from</span> baker <span class="reserved">in</span> five
  <span class="reserved">from</span> cooper <span class="reserved">in</span> five
  <span class="reserved">from</span> fletcher <span class="reserved">in</span> five
  <span class="reserved">from</span> miller <span class="reserved">in</span> five
  <span class="reserved">from</span> smith <span class="reserved">in</span> five
  <span class="reserved">where</span> Distinct(baker, cooper, fletcher, miller, smith)
  <span class="reserved">where</span> baker != 5
  <span class="reserved">where</span> cooper != 1
  <span class="reserved">where</span> fletcher != 1 &amp;&amp; fletcher != 5
  <span class="reserved">where</span> miller &gt; cooper
  <span class="reserved">where</span> Discrete(smith, fletcher)
  <span class="reserved">where</span> Discrete(fletcher, cooper)
  <span class="reserved">select new</span> { baker, cooper, fletcher, miller, smith };
</code></pre>



##<a id="sec-generated-title-3"></a> <a id="multifrom"></a>余談：多重 from の展開結果
ここでちょっと話がそれますが、
前節で書いたクエリ式は、以下のようなメソッド呼び出しに展開されます。

<pre class="source" title="answers1 のクエリ式と等価なクエリ演算" lang="">
<code><span class="reserved">var</span> answers0 = five
  .SelectMany(x =&gt; five, (baker, cooper) =&gt; <span class="reserved">new</span> { baker, cooper })
  .SelectMany(x =&gt; five, (x, fletcher) =&gt; <span class="reserved">new</span> { x, fletcher })
  .SelectMany(x =&gt; five, (x, miller) =&gt; <span class="reserved">new</span> { x, miller })
  .SelectMany(x =&gt; five, (x, smith) =&gt; <span class="reserved">new</span> { x, smith })
  .Where(x =&gt; Distinct(x.x.x.x.baker, x.x.x.x.cooper, x.x.x.fletcher, x.x.miller, x.smith))
  .Where(x =&gt; x.x.x.x.baker != 5)
  .Where(x =&gt; x.x.x.x.cooper != 1)
  .Where(x =&gt; x.x.x.fletcher != 1 &amp;&amp; x.x.x.fletcher != 5)
  .Where(x =&gt; x.x.miller &gt; x.x.x.x.cooper)
  .Where(x =&gt; Discrete(x.smith, x.x.x.fletcher))
  .Where(x =&gt; Discrete(x.x.x.fletcher, x.x.x.x.cooper))
  .Select(x =&gt; <span class="reserved">new</span> { x.x.x.x.baker, x.x.x.x.cooper, x.x.x.fletcher, x.x.miller, x.smith });
</code></pre>


.x だらけで泣けてきます。
こういう、元のクエリ式には存在しない（コンパイラによって生成された）変数を「[透過識別子](../data/sp3_stdquery.md#transparent)」と言います。

ここでは見やすさ優先で x で書いていますが、
実際にコンパイラが生成する変数名はもっと長ったらしい変な名前です。
結構な悲惨さで、難読化ツールなんて使わなくても十分難読。

多少なりとも透過識別子を整理すると以下のような感じ。

<pre class="source" title="answers0 の透過識別子をちょっと整理" lang="">
<code><span class="reserved">var</span> answers01 = five
  .SelectMany(x =&gt; five, (baker, cooper) =&gt; <span class="reserved">new</span> { baker, cooper })
  .SelectMany(x =&gt; five, (x, fletcher) =&gt; <span class="reserved">new</span> { x.baker, x.cooper, fletcher })
  .SelectMany(x =&gt; five, (x, miller) =&gt; <span class="reserved">new</span> { x.baker, x.cooper, x.fletcher, miller })
  .SelectMany(x =&gt; five, (x, smith) =&gt; <span class="reserved">new</span> { x.baker, x.cooper, x.fletcher, x.miller, smith })
  .Where(x =&gt; Distinct(x.baker, x.cooper, x.fletcher, x.miller, x.smith))
  .Where(x =&gt; x.baker != 5)
  .Where(x =&gt; x.cooper != 1)
  .Where(x =&gt; x.fletcher != 1 &amp;&amp; x.fletcher != 5)
  .Where(x =&gt; x.miller &gt; x.cooper)
  .Where(x =&gt; Discrete(x.smith, x.fletcher))
  .Where(x =&gt; Discrete(x.fletcher, x.cooper));
</code></pre>


幾分かはマシに。
多重 from が必要な場合、クエリ式のありがたみが身にしみます。


##<a id="sec-generated-title-4"></a> <a id="iterator"></a>イテレータに置き換え
C# 3.0 のクエリ式は、ラムダ式とかIQueryableを駆使して色々と面白いことができるんで、
高いポテンシャルを秘めてるんですけども、ここで話すのはもうちょっと単純な場合について考えてみます。

クエリ式は、LINQ to object（単純な IEnumerable に対するクエリ）に話を限定して、
from, where select くらいしか使わないような単純な場合には、
foreach, if, yield return ですべて置き換え可能なんですよね。

例えば、以下のような単純なクエリ式を考えてみます。

<pre class="source" title="IEnumerable に対するクエリ式" lang="">
<code><span class="reserved">var</span> points =
  <span class="reserved">from</span> x <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">from</span> y <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">where</span> x % 2 != 0
  <span class="reserved">where</span> y % 3 != 0
  <span class="reserved">select new</span> { x, y };
</code></pre>


このクエリ式は、「[イテレーター](../data/sp2_iterator.md#iterator)」構文を使って、
以下のように書き換えることができます。

<pre class="source" title="イテレータで置き換え" lang="">
<code><span class="reserved">static</span> IEnumerable&lt;Point&gt; Points()
{
  <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">foreach</span> (<span class="reserved">var</span> y <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">if</span> (x % 2 != 0)
  <span class="reserved">if</span> (y % 3 != 0)
  <span class="reserved">yield return new</span> Point(x, y);
}
</code></pre>


イテレータは匿名メソッドで書けない（＝ クロージャにできないし、匿名型を使えない）っていう欠点はありますが、
パフォーマンス的にはほんのちょっとだけ良くなったりします。
ほんの数％でもいいからパフォーマンスが欲しければ、
イテレータを使って書き変えましょう。


##<a id="sec-generated-title-5"></a> <a id="order"></a>クエリの順序変更
IEnumerable に対する単純なクエリ式が、foreach, if, yield return を使って書き変えれることがわかったところで、
パフォーマンスに関して古くから言われている以下の格言を思い出してみましょう。

<blockquote markdown="1">
条件分岐はループの外に出せ。

</blockquote>
要するに、

<pre class="source" title="if が foreach の内側" lang="">
<code><span class="reserved">static</span> IEnumerable&lt;Point&gt; Points()
{
  <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">foreach</span> (<span class="reserved">var</span> y <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">if</span> (x % 2 != 0)
  <span class="reserved">if</span> (y % 3 != 0)
  <span class="reserved">yield return new</span> Point(x, y);
}
</code></pre>


というコードよりも、

<pre class="source" title="if (x % 2 != 0) を foreach の外側に移動" lang="">
<code><span class="reserved">static</span> IEnumerable&lt;Point&gt; Points()
{
  <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">if</span> (x % 2 != 0)
  <span class="reserved">foreach</span> (<span class="reserved">var</span> y <span class="reserved">in</span> Enumerable.Range(0, 100)
  <span class="reserved">if</span> (y % 3 != 0)
  <span class="reserved">yield return new</span> Point(x, y);
}
</code></pre>


と書く方がパフォーマンスがよくなります。
たいていの場合、これだけで数割ほどパフォーマンスよくなったりします。
ループがもっと深いと下手すると何倍もよくなることも。

で、多重 from のあるクエリ式も、多重 foreach ループと似たようなもので、
from と where の順序を入れ替えるだけでパフォーマンスがよくなります。
例えば、

<pre class="source" title="where が from の後ろ" lang="">
<code><span class="reserved">var</span> answers1 =
  <span class="reserved">from</span> baker <span class="reserved">in</span> five
  <span class="reserved">from</span> cooper <span class="reserved">in</span> five
  <span class="reserved">from</span> fletcher <span class="reserved">in</span> five
  <span class="reserved">from</span> miller <span class="reserved">in</span> five
  <span class="reserved">from</span> smith <span class="reserved">in</span> five
  <span class="reserved">where</span> Distinct(baker, cooper, fletcher, miller, smith)
  <span class="reserved">where</span> baker != 5
  <span class="reserved">where</span> cooper != 1
  <span class="reserved">where</span> fletcher != 1 &amp;&amp; fletcher != 5
  <span class="reserved">where</span> miller &gt; cooper
  <span class="reserved">where</span> Discrete(smith, fletcher)
  <span class="reserved">where</span> Discrete(fletcher, cooper)
  <span class="reserved">select new</span> { baker, cooper, fletcher, miller, smith };
</code></pre>


と書くよりも、

<pre class="source" title="where を from の前に移動" lang="">
<code><span class="reserved">var</span> answers2 =
  <span class="reserved">from</span> baker <span class="reserved">in</span> five
  <span class="reserved">where</span> baker != 5
  <span class="reserved">from</span> cooper <span class="reserved">in</span> five
  <span class="reserved">where</span> cooper != 1
  <span class="reserved">from</span> fletcher <span class="reserved">in</span> five
  <span class="reserved">where</span> fletcher != 1 &amp;&amp; fletcher != 5
  <span class="reserved">where</span> Discrete(fletcher, cooper)
  <span class="reserved">from</span> miller <span class="reserved">in</span> five
  <span class="reserved">where</span> miller &gt; cooper
  <span class="reserved">from</span> smith <span class="reserved">in</span> five
  <span class="reserved">where</span> Discrete(smith, fletcher)
  <span class="reserved">where</span> Distinct(baker, cooper, fletcher, miller, smith)
  <span class="reserved">select new</span> { baker, cooper, fletcher, miller, smith };
</code></pre>


と書く方がパフォーマンスはいいわけです。
この例の場合、ループが深いし、Distinct 関数の負荷が結構重たいので、
この最適化で10倍以上高速になります。
→ 
[計測用ソース](../../../../assets/media/ufcpp2000/csharp/source/Comprehension.cs)
。

このように、
クエリ式は from と where の順序を工夫することでかなりパフォーマンスが変ることがあります。
が、やってて思ったんですが、
from が前に固まってないだけで思った以上に式が見づらい。

なので、クエリの順序最適化を自動的にやってくれるようなライブラリが欲しいなぁ、
と思うわけです。


##<a id="sec-generated-title-6"></a> <a id="reconst"></a>クエリ式の再構築
ということで、クエリ式の順序最適化をしたいなぁと思うわけですが、
そのためにはまず、
クエリ式をデータとして扱える必要があります。

「[ラムダ式](../functional/sp3_lambda.md#lambda)」を使えば、
匿名デリゲートと同じ記法で「[式木](../dynamic/sp3_expression.md#expressiontree)」が作れます。
ですが、クエリ式の場合には、
C# の仕様上、メソッド呼び出しに変換されてしまうわけです。
要するに、以下のようなクエリ式は、

<pre class="source" title="クエリ式の例" lang="">
<code><span class="reserved">var</span> q =
  <span class="reserved">from</span> x <span class="reserved">in</span> list
  <span class="reserved">where</span> x &gt; 0
  <span class="reserved">select</span> x;
</code></pre>


以下のようなメソッド呼び出しに変換されます。

<pre class="source" title="その変換結果" lang="">
<code><span class="reserved">var</span> q = list.Where(x =&gt; x &gt; 0);
</code></pre>


ということで、メソッド呼び出しの形になっている式木から、
元のクエリ式を復元するようなプログラムを書いてみました。

* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/ExpressionTree.zip)



ソース中の QueryExpression.cs が件の処理をしてる部分。

以下のようなコードで、

<pre class="source" title="式木からクエリ式の再構築" lang="">
<code><span class="reserved">var</span> q0 = Make.Expression((IEnumerable&lt;<span class="reserved">int</span>&gt; five) =&gt;
  <span class="reserved">from</span> baker <span class="reserved">in</span> five
  <span class="reserved">from</span> cooper <span class="reserved">in</span> five
  <span class="reserved">from</span> fletcher <span class="reserved">in</span> five
  <span class="reserved">from</span> miller <span class="reserved">in</span> five
  <span class="reserved">from</span> smith <span class="reserved">in</span> five
  <span class="reserved">where</span> Distinct(baker, cooper, fletcher, miller, smith)
  <span class="reserved">where</span> baker != 5
  <span class="reserved">where</span> cooper != 1
  <span class="reserved">where</span> fletcher != 1 &amp;&amp; fletcher != 5
  <span class="reserved">where</span> miller &gt; cooper
  <span class="reserved">where</span> Discrete(smith, fletcher)
  <span class="reserved">where</span> Discrete(fletcher, cooper)
  <span class="reserved">select new</span> { baker, cooper, fletcher, miller, smith }
  );

<span class="reserved">var</span> q = <span class="reserved">new</span> QueryExpression(q0);

<span class="reserved">foreach</span> (<span class="reserved">var</span> l <span class="reserved">in</span> q.Queries)
{
  Console.Write(<span class="literal">"{0}\n"</span>, l);
}
</code></pre>


以下のような出力を得ます。

<pre class="console" title="出力結果">
from baker in five
from cooper in five
from fletcher in five
from miller in five
from smith in five
where Distinct(new [] {baker, cooper, fletcher, miller, smith})
where (baker != 5)
where (cooper != 1)
where ((fletcher != 1) &amp;&amp; (fletcher != 5))
where (miller &gt; cooper)
where Discrete(smith, fletcher)
where Discrete(fletcher, cooper)
select new &lt;&gt;f__AnonymousType5`5(baker, cooper, fletcher, miller, smith)
</pre>
