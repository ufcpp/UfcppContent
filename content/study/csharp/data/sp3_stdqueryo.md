---
title: "標準クエリ演算子（その他）"
source_url: "https://ufcpp.net/study/csharp/data/sp3_stdqueryo/"
content_type: "Article"
published_at: "2008-02-19T00:00:00"
updated_at: "2019-01-29T19:43:15"
tags:
  - "Ver. 3.0"
umbraco_id: 1305
parent_id: 1298
sort_order: 6
aliases:
  - "/csharp/data/sp3_stdqueryo/"
  - "/csharp/sp3_stdqueryo"
  - "/csharp/sp3_stdqueryo.html"
  - "/study/csharp/sp3_stdqueryo"
  - "/study/csharp/sp3_stdqueryo.html"
---

# 標準クエリ演算子（その他）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version3">Ver. 3.0</h5>

LINQ は、元々はシーケンス（IEnumerable 実装クラス）やデータベーステーブルに対するメソッド群としてのみ提供される予定だったそうです。
（要するに、<code>from x in list</code> のようなクエリ式を導入する予定はなくて、
.Select などのメソッドのみを提供するつもりだった。）

でも、メソッド提供だけでは、
join や let などがどうしてもきれいに表現できなかったので、
やむなく SQL 風のクエリ式を導入したそうです。
（プログラミング言語の中に別の言語を埋め込むというのはデメリットも大きくて、
言語制作者にとっては結構ためらわれる行為。）
（join や let をきれいに書くためには、
どうしても「[透過識別子](sp3_stdquery.md#transparent)」のような考え方が必要だった。）

というような背景から、
標準クエリ演算子と呼ばれるメソッド群は、
クエリ式の形で書けるもの以外にも多数
（というか、むしろクエリ式で書けないものの方が多数）あります。


## <a id="sec-generated-title-2"></a> <a id="list"></a>その他の標準クエリ演算子

クエリ式で書けるもの以外にも、
メソッド呼び出しの形でだけ利用できる標準クエリ演算子として、以下のようなものもあります。

「[パーティション分割演算子](#partition)」：
Take、Skip、TakeWhile、SkipWhile

「[連結演算子](#concat)」：
Concat

「[順序付け演算子](#reverse)」：
Reverse

「[セット演算子](#set)」：
Distinct、Union、Intersect、Except

「[変換演算子](#cast)」：
AsEnumerable、ToArray、ToList、ToDictionary、ToLookup、OfType、Cast

「[等価演算子](#equal)」：
SequenceEqual

「[要素演算子](#element)」：
First、FirstOrDefault、Last、LastOrDefault、Single、SingleOrDefault、ElementAt、ElementAtOrDefault、DefaultIfEmpty

「[生成演算子](#generate)」：
Range、Repeat、Empty

「[限定子](#quantifier)」：
Any、All、Contains

「[集計演算子](#aggregate)」：
Count、LongCount、Sum、Min、Max、Average、Aggregate

これらの説明は次節以降で行っていきます。
その際、例として以下のようなデータを使います。

<pre class="source" title="サンプルデータ" lang="">
<code><span class="reserved">var</span> a = <span class="reserved">new</span>[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
<span class="reserved">var</span> b = <span class="reserved">new</span>[] { 0, 2, 4, 6, 8, 10, 12 };
</code></pre>


また、結果の出力用に、以下のような補助関数を使います。

<pre class="source" title="出力用の補助関数" lang="">
<code><span class="reserved">static void</span> Show&lt;T&gt;(IEnumerable&lt;T&gt; a)
{
  <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> a)
    Console.Write(<span class="literal">"{0} "</span>, x);
  Console.Write(<span class="literal">"\n"</span>);
}
</code></pre>



## <a id="sec-generated-title-3"></a> <a id="partition"></a>パーティション分割演算子

シーケンスを部分的に区切るため、
Take、Skip、TakeWhile、SkipWhile
の4つのメソッドがあります。

<table summary="">

	<tr>
		<td markdown="1">Take</td>
		<td markdown="1">先頭 n 個のみ取り出す</td>
	</tr>
	<tr>
		<td markdown="1">Skip</td>
		<td markdown="1">先頭 n 個を読み飛ばす</td>
	</tr>
	<tr>
		<td markdown="1">TakeWhile</td>
		<td markdown="1">先頭から、条件を満たす間だけ取り出す</td>
	</tr>
	<tr>
		<td markdown="1">SkipWhile</td>
		<td markdown="1">先頭から、条件を満たす間だけ読み飛ばす</td>
	</tr>
</table>


使用例を以下に示します。

<pre class="source" title="パーティション分割演算子の例" lang="">
<code>Show(a.Take(5));
Show(a.Skip(5));
Show(a.TakeWhile(x =&gt; x != 2));
Show(a.SkipWhile(x =&gt; x != 2));
</code></pre>


<pre class="console" title="実行結果">
0 0 1 1 2
2 3 3 4 4
0 0 1 1
2 2 3 3 4 4
</pre>



## <a id="sec-generated-title-4"></a> <a id="concat"></a>連結演算子

Concat で、2つのシーケンスを連結できます。

<pre class="source" title="連結演算子の例" lang="">
<code>Show(a.Concat(b));
</code></pre>


<pre class="console" title="実行結果">
0 0 1 1 2 2 3 3 4 4 0 2 4 6 8 10 12
</pre>


ちなみに、Concat や、後述する Union などは拡張メソッドなので、
<code>Concat(a, b)</code> という書き方も可能です。
<code>a.Concat(b)</code> と書いて a と b の間の2項演算とみなすか、
後者の書き方をして英語的に concatenate a and b と読むか、
ちょっと悩みますが、お好きな方をご利用ください。


## <a id="sec-generated-title-5"></a> <a id="reverse"></a>順序付け演算子

Reverse で、シーケンスの中身の順序を真逆にできます。

<pre class="source" title="順序付け演算子の例" lang="">
<code>Show(a.Reverse());
</code></pre>


<pre class="console" title="実行結果">
4 4 3 3 2 2 1 1 0 0
</pre>



## <a id="sec-generated-title-6"></a> <a id="set"></a>セット演算子

Distinct、Union、Intersect、Except の4つの
セット（set： 数学の集合論でいうところの集合。Collection と区別するために横文字にしておきます）演算子があります。

<table summary="">

	<tr>
		<td markdown="1">Distinct</td>
		<td markdown="1">コレクションから重複を取り除きます。</td>
	</tr>
	<tr>
		<td markdown="1">Union</td>
		<td markdown="1">合併（和集合）を求めます。</td>
	</tr>
	<tr>
		<td markdown="1">Intersect</td>
		<td markdown="1">共通部分（積集合）を求めます。</td>
	</tr>
	<tr>
		<td markdown="1">Except</td>
		<td markdown="1">a から b に含まれる要素を取り除きます（差集合）。</td>
	</tr>
</table>


<pre class="source" title="セット演算子の例" lang="">
<code>Show(a.Distinct());
Show(a.Union(b));
Show(a.Intersect(b));
Show(a.Except(b));
</code></pre>


<pre class="console" title="実行結果">
0 1 2 3 4
0 1 2 3 4 6 8 10 12
0 2 4
1 3
</pre>


注： 数学的な意味での集合は要素の重複を認めません。
セット演算子の結果は重複が除かれたものになります。


## <a id="sec-generated-title-7"></a> <a id="cast"></a>変換演算子

型の変換のための演算子がいくつかあります。


##### <a id="sec-generated-title-8"></a>シーケンス → シーケンス

まず、AsEnumerable、ToArray、ToList は、
シーケンスをそれぞれ、
IEnumeragle&lt;T&gt;、配列、List&lt;T&gt; に変換します。

<pre class="source" title="型変換" lang="">
<code><span class="reserved">var</span> a = <span class="reserved">new</span>[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
IEnumerable&lt;<span class="reserved">int</span>&gt; a1 = a.Distinct().AsEnumerable();
<span class="reserved">int</span>[] a2            = a.Distinct().ToArray();
List&lt;<span class="reserved">int</span>&gt; a3        = a.Distinct().ToList();
</code></pre>


AsEnumerable は、この例のような場合だとあまり役に立ちませんが、
IQueryable（LINQ to SQL などで使う）を IEnumerable に変換したりする場合に使います。

AsEnumerable が as なのに、ToArray や ToList が to を使っているのには理由があって、
as の方は遅延評価、to の方はその場での評価になります。
例えば、以下のようなコードを実行したとします。

<pre class="source" title="As 系と To 系の違い" lang="">
<code>Func&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; hook = x =&gt;
{
  Console.Write(<span class="literal">"{0}"</span>, x);
  <span class="reserved">return</span> x;
};

Console.Write(<span class="literal">"AsEnumerable\n"</span>);
Console.Write(<span class="literal">"before "</span>);
<span class="reserved">var</span> a1 = a.Select(hook).AsEnumerable();
Console.Write(<span class="literal">" middle "</span>);
<span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> a1) ;
Console.Write(<span class="literal">" after\n\n"</span>);

Console.Write(<span class="literal">"ToList\n"</span>);
Console.Write(<span class="literal">"before "</span>);
<span class="reserved">var</span> a2 = a.Select(hook).ToList();
Console.Write(<span class="literal">" middle "</span>);
<span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> a2) ;
Console.Write(<span class="literal">" after\n\n"</span>);
</code></pre>


上半分と下半分は、AsEnumerable と ToList の部分以外はほぼ同じコードですが、
実行結果は以下のように変わります。
前者は foreach の行で初めて hook が実行され、
後者は ToList の時点で実行されます。

<pre class="console" title="実行結果">
AsEnumerable
before  middle <em>0011223344</em> after

ToList
before <em>0011223344</em> middle  after
</pre>



##### <a id="sec-generated-title-9"></a>シーケンス → 辞書

ToDictionary と ToLookup は、シーケンスを辞書（キーと値のペア）化します。
ToDictionary は Dictionary（1つのキーに対して1つの値を持つ）を、
ToLookup は Lookup型（1つのキーに対して複数の値（1つの IEnumerable）を持つ辞書）の値を返します。

<pre class="source" title="辞書化の例" lang="">
<code><span class="reserved">var</span> list = <span class="reserved">new</span>[] {
  <span class="reserved">new</span> { Name = <span class="literal">"糸色望"</span>, CV = <span class="literal">"神谷浩史"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"風浦可符香"</span>, CV = <span class="literal">"野中藍"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"大草麻菜実"</span>, CV = <span class="literal">"井上喜久子"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"音無芽留"</span>, CV = <span class="literal">"？？？？"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"加賀愛"</span>, CV = <span class="literal">"後藤沙緒里"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"木津千里"</span>, CV = <span class="literal">"井上麻里奈"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"木村カエレ"</span>, CV = <span class="literal">"小林ゆう"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"小節あびる"</span>, CV = <span class="literal">"後藤邑子"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"小森霧"</span>, CV = <span class="literal">"谷井あすか"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"関内・マリア・太郎"</span>, CV = <span class="literal">"沢城みゆき"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"常月まとい"</span>, CV = <span class="literal">"真田アサミ"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"日塔奈美"</span>, CV = <span class="literal">"新谷良子"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"藤吉晴美"</span>, CV = <span class="literal">"松来未祐"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"三珠真夜"</span>, CV = <span class="literal">"谷井あすか"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"久藤准"</span>, CV = <span class="literal">"水島大宙"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"新井智恵"</span>, CV = <span class="literal">"矢島晶子"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"臼井影郎"</span>, CV = <span class="literal">"上田陽司"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"隣の女子大生"</span>, CV = <span class="literal">"野中藍"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"万世橋わたる"</span>, CV = <span class="literal">"上田陽司"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"甚六先生"</span>, CV = <span class="literal">"上田陽司"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"糸色景"</span>, CV = <span class="literal">"子安武人"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"糸色命"</span>, CV = <span class="literal">"神谷浩史"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"糸色倫"</span>, CV = <span class="literal">"矢島晶子"</span> },
  <span class="reserved">new</span> { Name = <span class="literal">"糸色交"</span>, CV = <span class="literal">"矢島晶子"</span> },
};

<span class="reserved">var</span> dicByName = list.ToDictionary(x =&gt; x.Name);
Console.Write(<span class="literal">"{0}\n"</span>, dicByName[<span class="literal">"日塔奈美"</span>].CV);
Console.Write(<span class="literal">"{0}\n"</span>, dicByName[<span class="literal">"小節あびる"</span>].CV);

<span class="reserved">var</span> lookupByCV = list.ToLookup(x =&gt; x.CV);
Show(lookupByCV[<span class="literal">"矢島晶子"</span>].Select(x =&gt; x.Name));
Show(lookupByCV[<span class="literal">"神谷浩史"</span>].Select(x =&gt; x.Name));
</code></pre>


<pre class="console" title="実行結果">
新谷良子
後藤邑子
新井智恵 糸色倫 糸色交
糸色望 糸色命
</pre>


ToLookup を使えば、例えば、名前の1文字目を使ったインデックスを作ったりといったことも出来ます。

<pre class="source" title="ToLookup の例2" lang="">
<code><span class="reserved">var</span> lookupByFirstChar = list.Select(x =&gt; x.Name).ToLookup(x =&gt; x[0]);
Show(lookupByFirstChar[<span class="literal">'糸'</span>]);
Show(lookupByFirstChar[<span class="literal">'小'</span>]);
</code></pre>


<pre class="console" title="実行結果">
糸色望 糸色景 糸色命 糸色倫 糸色交
小節あびる 小森霧
</pre>



##### <a id="sec-generated-title-10"></a>要素の型変換

OfType、Cast で要素の型を変換できます。
Cast はすべての要素のキャストを試みます。
キャストに失敗した場合は例外が発生します。
一方、OfType は、変換可能な要素だけを抽出します。

<pre class="source" title="Cast, OfType の例" lang="">
<code><span class="reserved">var</span> numList = <span class="reserved">new object</span>[] {
  1, 1.1, 2, 2.2, 3, 3.3
};

<span class="reserved">var</span> miscList = <span class="reserved">new object</span>[] {
  0, <span class="literal">"test 1"</span>, 1, 3.14, <span class="literal">"test 2"</span>, 2.72,
  <span class="reserved">new</span> List&lt;<span class="reserved">int</span>&gt;(),
  <span class="reserved">new</span> Stack&lt;<span class="reserved">int</span>&gt;(),
  <span class="reserved">new</span> Queue&lt;<span class="reserved">int</span>&gt;()
};

Show(numList.Cast&lt;<span class="reserved">int</span>&gt;());
<span class="comment">// Show(miscList.Cast&lt;int&gt;()); // 例外発生</span>

Show(numList.OfType&lt;<span class="reserved">int</span>&gt;());
Show(miscList.OfType&lt;IEnumerable&lt;<span class="reserved">int</span>&gt;&gt;().Select(x =&gt; x.GetType().Name));
</code></pre>


<pre class="console" title="実行結果">
1 1 2 2 3 3
1 2 3
List`1 Stack`1 Queue`1
</pre>


<code>.OfType&lt;T&gt;()</code> は
<code>.Where(x =&gt; x is T).Cast&lt;T&gt;()</code> と同じ結果になります。


## <a id="sec-generated-title-11"></a> <a id="equal"></a>等価演算子

SequenceEqual で、2つのシーケンスの中身が（順序も含めて）一致するかどうかを調べられます。

<pre class="source" title="SequenceEqualの例" lang="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span>[] { 0, 3, 1, 2 };
<span class="reserved">var</span> y = <span class="reserved">new</span>[] { 0, 3, 1, 2 };
<span class="reserved">var</span> z = <span class="reserved">new</span>[] { 1, 2, 3 };

Console.Write(<span class="literal">"{0}\n"</span>, x.SequenceEqual(y));
Console.Write(<span class="literal">"{0}\n"</span>, y.SequenceEqual(z));
Console.Write(<span class="literal">"{0}\n"</span>, z.SequenceEqual(x));
</code></pre>


<pre class="console" title="実行結果">
True
False
False
</pre>



## <a id="sec-generated-title-12"></a> <a id="element"></a>要素演算子

シーケンスの中から特定の要素を1つ取り出すため、
First、FirstOrDefault、Last、LastOrDefault、Single、SingleOrDefault、ElementAt、ElementAtOrDefault、DefaultIfEmpty という演算子が用意されています。

<table summary="">

	<tr>
		<td markdown="1">First、FirstOrDefault</td>
		<td markdown="1">条件を満たす最初の要素を返します。</td>
	</tr>
	<tr>
		<td markdown="1">Last、LastOrDefault</td>
		<td markdown="1">条件を満たす最後の要素を返します。</td>
	</tr>
	<tr>
		<td markdown="1">Single、SingleOrDefault</td>
		<td markdown="1">条件を満たす唯一の要素を返します。もし、条件を満たす要素が複数あった場合、例外を発生させます。</td>
	</tr>
	<tr>
		<td markdown="1">ElementAt、ElementAtOrDefault</td>
		<td markdown="1">n 番目の要素を返します。</td>
	</tr>
	<tr>
		<td markdown="1">DefaultIfEmpty</td>
		<td markdown="1">もしシーケンスが空の場合、デフォルトの値が1つだけ入ったシーケンスを返します。</td>
	</tr>
</table>


OrDefault が付かないもの、
もし条件を満たす要素が1つもなければ例外を発生させます。
一方、OrDefault が付くものは、
もし条件を満たす要素が1つもなければ規定値
（例えば、数値型なら 0、参照型なら null）を返します。

<pre class="source" title="要素演算子の例" lang="">
<code><span class="reserved">var</span> list = <span class="reserved">new</span>[] {
  <span class="reserved">new</span> { X = 0, Y = 0 },
  <span class="reserved">new</span> { X = 0, Y = 1 },
  <span class="reserved">new</span> { X = 0, Y = 2 },
  <span class="reserved">new</span> { X = 1, Y = 0 },
  <span class="reserved">new</span> { X = 1, Y = 1 },
  <span class="reserved">new</span> { X = 1, Y = 2 },
  <span class="reserved">new</span> { X = 2, Y = 0 },
};

Console.Write(<span class="literal">"{0}\n"</span>, list.First(p =&gt; p.X == 0));
<span class="comment">// Console.Write("{0}\n", list.First(p =&gt; p.X == 3)); // 例外発生</span>
Console.Write(<span class="literal">"{0}\n"</span>, list.Last(p =&gt; p.X == 1));
Console.Write(<span class="literal">"{0}\n"</span>, list.Single(p =&gt; p.X == 2));
<span class="comment">// Console.Write("{0}\n", list.Single(p =&gt; p.X == 0)); // 例外発生</span>
</code></pre>


<pre class="console" title="実行結果">
{ X = 0, Y = 0 }
{ X = 1, Y = 2 }
{ X = 2, Y = 0 }
</pre>


First、Last、Single には引数を持たないバージョンもあって、
その場合、First、Last はシーケンス全体の中の最初・最後の要素を返します。
引数なしの Single は、シーケンスがただ1つの要素からなるときにはその要素の値を返し、
そうでなければ例外を発生させます。

<pre class="source" title="引数なしの Single" lang="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span>[] { 0 }.Single();    <span class="comment">// x == 0</span>
<span class="reserved">var</span> y = <span class="reserved">new</span>[] { 0, 1 }.Single(); <span class="comment">// 例外発生</span>
</code></pre>



## <a id="sec-generated-title-13"></a> <a id="generate"></a>生成演算子

シーケンスに対するフィルタリングではなく、
シーケンスそのものを生成するような演算子が3つあります。

<table summary="">

	<tr>
		<td markdown="1">Range</td>
		<td markdown="1">ある範囲の整数列を生成します。</td>
	</tr>
	<tr>
		<td markdown="1">Repeat</td>
		<td markdown="1">同じ値を指定回数繰り返すシーケンスを生成します。</td>
	</tr>
	<tr>
		<td markdown="1">Empty</td>
		<td markdown="1">空のシーケンスを生成します。</td>
	</tr>
</table>


<pre class="source" title="生成演算子の例" lang="">
<code>Show(Enumerable.Range(5, 3));
Show(Enumerable.Repeat(<span class="literal">"abc"</span>, 3));
Show(Enumerable.Empty&lt;<span class="reserved">int</span>&gt;());
</code></pre>


<pre class="console" title="実行結果">
5 6 7
abc abc abc

</pre>


例えば、Range を使って任意個数の乱数列を生成したりできます。

<pre class="source" title="乱数列生成の例" lang="">
<code>Random rnd = <span class="reserved">new</span> Random();
<span class="reserved">var</span> randomSeq = Enumerable.Range(0, 100).Select(x =&gt; rnd.NextDouble());
</code></pre>



## <a id="sec-generated-title-14"></a> <a id="quantifier"></a>限定子

Any、All、Contains は、
シーケンスがある条件を満たすかどうかを調べるための演算子（限定子（quantifier））です。

<table summary="">

	<tr>
		<td markdown="1">Any</td>
		<td markdown="1">条件を満たす要素がシーケンス中に1つでもあれば true を返す。</td>
	</tr>
	<tr>
		<td markdown="1">All</td>
		<td markdown="1">シーケンス中の全ての要素が条件を満たせば true を返す。</td>
	</tr>
	<tr>
		<td markdown="1">Contains</td>
		<td markdown="1">シーケンス中に要素が含まれるかどうかを調べる。</td>
	</tr>
</table>


<pre class="source" title="限定子の例" lang="">
<code>Func&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; isEven = x =&gt; (x &amp; 1) == 0;

Console.Write(<span class="literal">"{0}\n"</span>, a.Any(isEven)); <span class="comment">// a は偶数も含むので true</span>
Console.Write(<span class="literal">"{0}\n"</span>, b.Any(isEven)); <span class="comment">// b は偶数を含むので true</span>

Console.Write(<span class="literal">"{0}\n"</span>, a.All(isEven)); <span class="comment">// a は奇数を含むので false</span>
Console.Write(<span class="literal">"{0}\n"</span>, b.All(isEven)); <span class="comment">// b は全て偶数なので true</span>

Console.Write(<span class="literal">"{0}\n"</span>, a.Contains(0)); <span class="comment">// a は 0 を含むので true</span>
</code></pre>



## <a id="sec-generated-title-15"></a> <a id="aggregate"></a>集計演算子

シーケンス中の要素の個数、和、平均値などを集計するための演算子が7つあります。

<table summary="">

	<tr>
		<td markdown="1">Count</td>
		<td markdown="1">要素の個数を返します。</td>
	</tr>
	<tr>
		<td markdown="1">LongCount</td>
		<td markdown="1">要素の個数を long 型で返します。</td>
	</tr>
	<tr>
		<td markdown="1">Sum</td>
		<td markdown="1">要素の和を求めます。</td>
	</tr>
	<tr>
		<td markdown="1">Min</td>
		<td markdown="1">要素の最小値を求めます。</td>
	</tr>
	<tr>
		<td markdown="1">Max</td>
		<td markdown="1">要素の最大値を求めます。</td>
	</tr>
	<tr>
		<td markdown="1">Average</td>
		<td markdown="1">要素の平均値を求めます。</td>
	</tr>
	<tr>
		<td markdown="1">Aggregate</td>
		<td markdown="1">より一般的な集計処理を行います。</td>
	</tr>
</table>


list.Aggregate(func); は、以下のコードと同じ結果を得ます。

<pre class="source" title="Aggregate の処理内容" lang="">
<code><span class="reserved">static</span> T Aggregate&lt;T&gt;(IEnumerable&lt;T&gt; list, Func&lt;T, T, T&gt; func)
{
  <span class="reserved">var</span> acc = list.First();
  <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> list.Skip(1))
  {
    acc = func(acc, x);
  }
  <span class="reserved">return</span> acc;
}
</code></pre>


したがって、
<code>list.Aggregate((s, x) =&gt; s + x);</code>
で
<code>list.Sum();</code>
と同じ意味になります。

他の集計演算子もほぼ同様の動作をしています。
なので、
例えば、以下のようなコードを書くと、<em>foreach ループを5回まわすことになります</em>。

<pre class="source" title="集計演算子の例" lang="">
<code><span class="reserved">var</span> num = a.Count();
<span class="reserved">var</span> min = a.Min();
<span class="reserved">var</span> max = a.Max();
<span class="reserved">var</span> ave = a.Average();
<span class="reserved">var</span> sum = a.Sum();
</code></pre>


そのため、
以下のようなコードと比べると、圧倒的に動作速度が遅くなります。
（筆者の環境では約10倍の差。）

<pre class="source" title="自力で集計。ループを1つに。" lang="">
<code><span class="reserved">var</span> num = 0;
<span class="reserved">var</span> min = <span class="reserved">int</span>.MaxValue;
<span class="reserved">var</span> max = <span class="reserved">int</span>.MinValue;
<span class="reserved">var</span> sum = 0;

<span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> a)
{
  ++num;
  <span class="reserved">if</span> (min &gt; x) min = x;
  <span class="reserved">if</span> (max &lt; x) max = x;
  sum += x;
}
<span class="reserved">double</span> ave = sum / (<span class="reserved">double</span>)num;
</code></pre>
