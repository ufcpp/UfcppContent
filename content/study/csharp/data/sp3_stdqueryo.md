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

```csharp
var a = new[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
var b = new[] { 0, 2, 4, 6, 8, 10, 12 };
```


また、結果の出力用に、以下のような補助関数を使います。

```csharp
static void Show<T>(IEnumerable<T> a)
{
  foreach (var x in a)
    Console.Write("{0} ", x);
  Console.Write("\n");
}
```



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

```csharp
Show(a.Take(5));
Show(a.Skip(5));
Show(a.TakeWhile(x => x != 2));
Show(a.SkipWhile(x => x != 2));
```


```console
0 0 1 1 2
2 3 3 4 4
0 0 1 1
2 2 3 3 4 4
```



## <a id="sec-generated-title-4"></a> <a id="concat"></a>連結演算子

Concat で、2つのシーケンスを連結できます。

```csharp
Show(a.Concat(b));
```


```console
0 0 1 1 2 2 3 3 4 4 0 2 4 6 8 10 12
```


ちなみに、Concat や、後述する Union などは拡張メソッドなので、
<code>Concat(a, b)</code> という書き方も可能です。
<code>a.Concat(b)</code> と書いて a と b の間の2項演算とみなすか、
後者の書き方をして英語的に concatenate a and b と読むか、
ちょっと悩みますが、お好きな方をご利用ください。


## <a id="sec-generated-title-5"></a> <a id="reverse"></a>順序付け演算子

Reverse で、シーケンスの中身の順序を真逆にできます。

```csharp
Show(a.Reverse());
```


```console
4 4 3 3 2 2 1 1 0 0
```



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


```csharp
Show(a.Distinct());
Show(a.Union(b));
Show(a.Intersect(b));
Show(a.Except(b));
```


```console
0 1 2 3 4
0 1 2 3 4 6 8 10 12
0 2 4
1 3
```


注： 数学的な意味での集合は要素の重複を認めません。
セット演算子の結果は重複が除かれたものになります。


## <a id="sec-generated-title-7"></a> <a id="cast"></a>変換演算子

型の変換のための演算子がいくつかあります。


##### <a id="sec-generated-title-8"></a>シーケンス → シーケンス

まず、AsEnumerable、ToArray、ToList は、
シーケンスをそれぞれ、
IEnumeragle&lt;T&gt;、配列、List&lt;T&gt; に変換します。

```csharp
var a = new[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
IEnumerable<int> a1 = a.Distinct().AsEnumerable();
int[] a2            = a.Distinct().ToArray();
List<int> a3        = a.Distinct().ToList();
```


AsEnumerable は、この例のような場合だとあまり役に立ちませんが、
IQueryable（LINQ to SQL などで使う）を IEnumerable に変換したりする場合に使います。

AsEnumerable が as なのに、ToArray や ToList が to を使っているのには理由があって、
as の方は遅延評価、to の方はその場での評価になります。
例えば、以下のようなコードを実行したとします。

```csharp
Func<int, int> hook = x =>
{
  Console.Write("{0}", x);
  return x;
};

Console.Write("AsEnumerable\n");
Console.Write("before ");
var a1 = a.Select(hook).AsEnumerable();
Console.Write(" middle ");
foreach (var x in a1) ;
Console.Write(" after\n\n");

Console.Write("ToList\n");
Console.Write("before ");
var a2 = a.Select(hook).ToList();
Console.Write(" middle ");
foreach (var x in a2) ;
Console.Write(" after\n\n");
```


上半分と下半分は、AsEnumerable と ToList の部分以外はほぼ同じコードですが、
実行結果は以下のように変わります。
前者は foreach の行で初めて hook が実行され、
後者は ToList の時点で実行されます。

```console
AsEnumerable
before  middle 0011223344 after

ToList
before 0011223344 middle  after
```



##### <a id="sec-generated-title-9"></a>シーケンス → 辞書

ToDictionary と ToLookup は、シーケンスを辞書（キーと値のペア）化します。
ToDictionary は Dictionary（1つのキーに対して1つの値を持つ）を、
ToLookup は Lookup型（1つのキーに対して複数の値（1つの IEnumerable）を持つ辞書）の値を返します。

```csharp
var list = new[] {
  new { Name = "糸色望", CV = "神谷浩史" },
  new { Name = "風浦可符香", CV = "野中藍" },
  new { Name = "大草麻菜実", CV = "井上喜久子" },
  new { Name = "音無芽留", CV = "？？？？" },
  new { Name = "加賀愛", CV = "後藤沙緒里" },
  new { Name = "木津千里", CV = "井上麻里奈" },
  new { Name = "木村カエレ", CV = "小林ゆう" },
  new { Name = "小節あびる", CV = "後藤邑子" },
  new { Name = "小森霧", CV = "谷井あすか" },
  new { Name = "関内・マリア・太郎", CV = "沢城みゆき" },
  new { Name = "常月まとい", CV = "真田アサミ" },
  new { Name = "日塔奈美", CV = "新谷良子" },
  new { Name = "藤吉晴美", CV = "松来未祐" },
  new { Name = "三珠真夜", CV = "谷井あすか" },
  new { Name = "久藤准", CV = "水島大宙" },
  new { Name = "新井智恵", CV = "矢島晶子" },
  new { Name = "臼井影郎", CV = "上田陽司" },
  new { Name = "隣の女子大生", CV = "野中藍" },
  new { Name = "万世橋わたる", CV = "上田陽司" },
  new { Name = "甚六先生", CV = "上田陽司" },
  new { Name = "糸色景", CV = "子安武人" },
  new { Name = "糸色命", CV = "神谷浩史" },
  new { Name = "糸色倫", CV = "矢島晶子" },
  new { Name = "糸色交", CV = "矢島晶子" },
};

var dicByName = list.ToDictionary(x => x.Name);
Console.Write("{0}\n", dicByName["日塔奈美"].CV);
Console.Write("{0}\n", dicByName["小節あびる"].CV);

var lookupByCV = list.ToLookup(x => x.CV);
Show(lookupByCV["矢島晶子"].Select(x => x.Name));
Show(lookupByCV["神谷浩史"].Select(x => x.Name));
```


```console
新谷良子
後藤邑子
新井智恵 糸色倫 糸色交
糸色望 糸色命
```


ToLookup を使えば、例えば、名前の1文字目を使ったインデックスを作ったりといったことも出来ます。

```csharp
var lookupByFirstChar = list.Select(x => x.Name).ToLookup(x => x[0]);
Show(lookupByFirstChar['糸']);
Show(lookupByFirstChar['小']);
```


```console
糸色望 糸色景 糸色命 糸色倫 糸色交
小節あびる 小森霧
```



##### <a id="sec-generated-title-10"></a>要素の型変換

OfType、Cast で要素の型を変換できます。
Cast はすべての要素のキャストを試みます。
キャストに失敗した場合は例外が発生します。
一方、OfType は、変換可能な要素だけを抽出します。

```csharp
var numList = new object[] {
  1, 1.1, 2, 2.2, 3, 3.3
};

var miscList = new object[] {
  0, "test 1", 1, 3.14, "test 2", 2.72,
  new List<int>(),
  new Stack<int>(),
  new Queue<int>()
};

Show(numList.Cast<int>());
// Show(miscList.Cast<int>()); // 例外発生

Show(numList.OfType<int>());
Show(miscList.OfType<IEnumerable<int>>().Select(x => x.GetType().Name));
```


```console
1 1 2 2 3 3
1 2 3
List`1 Stack`1 Queue`1
```


<code>.OfType&lt;T&gt;()</code> は
<code>.Where(x =&gt; x is T).Cast&lt;T&gt;()</code> と同じ結果になります。


## <a id="sec-generated-title-11"></a> <a id="equal"></a>等価演算子

SequenceEqual で、2つのシーケンスの中身が（順序も含めて）一致するかどうかを調べられます。

```csharp
var x = new[] { 0, 3, 1, 2 };
var y = new[] { 0, 3, 1, 2 };
var z = new[] { 1, 2, 3 };

Console.Write("{0}\n", x.SequenceEqual(y));
Console.Write("{0}\n", y.SequenceEqual(z));
Console.Write("{0}\n", z.SequenceEqual(x));
```


```console
True
False
False
```



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

```csharp
var list = new[] {
  new { X = 0, Y = 0 },
  new { X = 0, Y = 1 },
  new { X = 0, Y = 2 },
  new { X = 1, Y = 0 },
  new { X = 1, Y = 1 },
  new { X = 1, Y = 2 },
  new { X = 2, Y = 0 },
};

Console.Write("{0}\n", list.First(p => p.X == 0));
// Console.Write("{0}\n", list.First(p => p.X == 3)); // 例外発生
Console.Write("{0}\n", list.Last(p => p.X == 1));
Console.Write("{0}\n", list.Single(p => p.X == 2));
// Console.Write("{0}\n", list.Single(p => p.X == 0)); // 例外発生
```


```console
{ X = 0, Y = 0 }
{ X = 1, Y = 2 }
{ X = 2, Y = 0 }
```


First、Last、Single には引数を持たないバージョンもあって、
その場合、First、Last はシーケンス全体の中の最初・最後の要素を返します。
引数なしの Single は、シーケンスがただ1つの要素からなるときにはその要素の値を返し、
そうでなければ例外を発生させます。

```csharp
var x = new[] { 0 }.Single();    // x == 0
var y = new[] { 0, 1 }.Single(); // 例外発生
```



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


```csharp
Show(Enumerable.Range(5, 3));
Show(Enumerable.Repeat("abc", 3));
Show(Enumerable.Empty<int>());
```


```console
5 6 7
abc abc abc
```


例えば、Range を使って任意個数の乱数列を生成したりできます。

```csharp
Random rnd = new Random();
var randomSeq = Enumerable.Range(0, 100).Select(x => rnd.NextDouble());
```



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


```csharp
Func<int, bool> isEven = x => (x & 1) == 0;

Console.Write("{0}\n", a.Any(isEven)); // a は偶数も含むので true
Console.Write("{0}\n", b.Any(isEven)); // b は偶数を含むので true

Console.Write("{0}\n", a.All(isEven)); // a は奇数を含むので false
Console.Write("{0}\n", b.All(isEven)); // b は全て偶数なので true

Console.Write("{0}\n", a.Contains(0)); // a は 0 を含むので true
```



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

```csharp
static T Aggregate<T>(IEnumerable<T> list, Func<T, T, T> func)
{
  var acc = list.First();
  foreach (var x in list.Skip(1))
  {
    acc = func(acc, x);
  }
  return acc;
}
```


したがって、
<code>list.Aggregate((s, x) =&gt; s + x);</code>
で
<code>list.Sum();</code>
と同じ意味になります。

他の集計演算子もほぼ同様の動作をしています。
なので、
例えば、以下のようなコードを書くと、<em>foreach ループを5回まわすことになります</em>。

```csharp
var num = a.Count();
var min = a.Min();
var max = a.Max();
var ave = a.Average();
var sum = a.Sum();
```


そのため、
以下のようなコードと比べると、圧倒的に動作速度が遅くなります。
（筆者の環境では約10倍の差。）

```csharp
var num = 0;
var min = int.MaxValue;
var max = int.MinValue;
var sum = 0;

foreach (var x in a)
{
  ++num;
  if (min > x) min = x;
  if (max < x) max = x;
  sum += x;
}
double ave = sum / (double)num;
```
