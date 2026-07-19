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

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[標準クエリ演算子（クエリ式関係）](../data/sp3_stdquery.md)」などで説明しているように、
LINQ クエリ式はメソッド（あるいは拡張メソッド）呼び出しに変換されます。
例えば、以下のような式は、

```csharp
var q =
  from x in list
  where x > 0
  select x;
```


以下のようなメソッド呼び出しに変換されます。

```csharp
var q = list.Where(x => x > 0);
```


ここでは、この逆をやってみようという話をします。
すなわち、
list.Where(...) から from x in list where ... というクエリ式を再構築します。


## <a id="sec-generated-title-2"></a> <a id="cause"></a>ことの発端

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

```csharp
// 1～5
static IEnumerable<int> five = Enumerable.Range(1, 5);

// x の要素に重複がないとき true
static bool Distinct(params int[] x)
{
  return x.Distinct().Count() == x.Length;
}

// x, y が隣り合う数字でないとき true
static bool Discrete(int x, int y)
{
  return Math.Abs(checked(x - y)) != 1;
}
```


これを使って先ほどの問題を解くクエリ式を書くと、以下のような感じ。

```csharp
var answers1 =
  from baker in five
  from cooper in five
  from fletcher in five
  from miller in five
  from smith in five
  where Distinct(baker, cooper, fletcher, miller, smith)
  where baker != 5
  where cooper != 1
  where fletcher != 1 && fletcher != 5
  where miller > cooper
  where Discrete(smith, fletcher)
  where Discrete(fletcher, cooper)
  select new { baker, cooper, fletcher, miller, smith };
```



## <a id="sec-generated-title-3"></a> <a id="multifrom"></a>余談：多重 from の展開結果

ここでちょっと話がそれますが、
前節で書いたクエリ式は、以下のようなメソッド呼び出しに展開されます。

```csharp
var answers0 = five
  .SelectMany(x => five, (baker, cooper) => new { baker, cooper })
  .SelectMany(x => five, (x, fletcher) => new { x, fletcher })
  .SelectMany(x => five, (x, miller) => new { x, miller })
  .SelectMany(x => five, (x, smith) => new { x, smith })
  .Where(x => Distinct(x.x.x.x.baker, x.x.x.x.cooper, x.x.x.fletcher, x.x.miller, x.smith))
  .Where(x => x.x.x.x.baker != 5)
  .Where(x => x.x.x.x.cooper != 1)
  .Where(x => x.x.x.fletcher != 1 && x.x.x.fletcher != 5)
  .Where(x => x.x.miller > x.x.x.x.cooper)
  .Where(x => Discrete(x.smith, x.x.x.fletcher))
  .Where(x => Discrete(x.x.x.fletcher, x.x.x.x.cooper))
  .Select(x => new { x.x.x.x.baker, x.x.x.x.cooper, x.x.x.fletcher, x.x.miller, x.smith });
```


.x だらけで泣けてきます。
こういう、元のクエリ式には存在しない（コンパイラによって生成された）変数を「[透過識別子](../data/sp3_stdquery.md#transparent)」と言います。

ここでは見やすさ優先で x で書いていますが、
実際にコンパイラが生成する変数名はもっと長ったらしい変な名前です。
結構な悲惨さで、難読化ツールなんて使わなくても十分難読。

多少なりとも透過識別子を整理すると以下のような感じ。

```csharp
var answers01 = five
  .SelectMany(x => five, (baker, cooper) => new { baker, cooper })
  .SelectMany(x => five, (x, fletcher) => new { x.baker, x.cooper, fletcher })
  .SelectMany(x => five, (x, miller) => new { x.baker, x.cooper, x.fletcher, miller })
  .SelectMany(x => five, (x, smith) => new { x.baker, x.cooper, x.fletcher, x.miller, smith })
  .Where(x => Distinct(x.baker, x.cooper, x.fletcher, x.miller, x.smith))
  .Where(x => x.baker != 5)
  .Where(x => x.cooper != 1)
  .Where(x => x.fletcher != 1 && x.fletcher != 5)
  .Where(x => x.miller > x.cooper)
  .Where(x => Discrete(x.smith, x.fletcher))
  .Where(x => Discrete(x.fletcher, x.cooper));
```


幾分かはマシに。
多重 from が必要な場合、クエリ式のありがたみが身にしみます。


## <a id="sec-generated-title-4"></a> <a id="iterator"></a>イテレータに置き換え

C# 3.0 のクエリ式は、ラムダ式とかIQueryableを駆使して色々と面白いことができるんで、
高いポテンシャルを秘めてるんですけども、ここで話すのはもうちょっと単純な場合について考えてみます。

クエリ式は、LINQ to object（単純な IEnumerable に対するクエリ）に話を限定して、
from, where select くらいしか使わないような単純な場合には、
foreach, if, yield return ですべて置き換え可能なんですよね。

例えば、以下のような単純なクエリ式を考えてみます。

```csharp
var points =
  from x in Enumerable.Range(0, 100)
  from y in Enumerable.Range(0, 100)
  where x % 2 != 0
  where y % 3 != 0
  select new { x, y };
```


このクエリ式は、「[イテレーター](../data/sp2_iterator.md#iterator)」構文を使って、
以下のように書き換えることができます。

```csharp
static IEnumerable<Point> Points()
{
  foreach (var x in Enumerable.Range(0, 100)
  foreach (var y in Enumerable.Range(0, 100)
  if (x % 2 != 0)
  if (y % 3 != 0)
  yield return new Point(x, y);
}
```


イテレータは匿名メソッドで書けない（＝ クロージャにできないし、匿名型を使えない）っていう欠点はありますが、
パフォーマンス的にはほんのちょっとだけ良くなったりします。
ほんの数％でもいいからパフォーマンスが欲しければ、
イテレータを使って書き変えましょう。


## <a id="sec-generated-title-5"></a> <a id="order"></a>クエリの順序変更

IEnumerable に対する単純なクエリ式が、foreach, if, yield return を使って書き変えれることがわかったところで、
パフォーマンスに関して古くから言われている以下の格言を思い出してみましょう。

<blockquote markdown="1">
条件分岐はループの外に出せ。

</blockquote>
要するに、

```csharp
static IEnumerable<Point> Points()
{
  foreach (var x in Enumerable.Range(0, 100)
  foreach (var y in Enumerable.Range(0, 100)
  if (x % 2 != 0)
  if (y % 3 != 0)
  yield return new Point(x, y);
}
```


というコードよりも、

```csharp
static IEnumerable<Point> Points()
{
  foreach (var x in Enumerable.Range(0, 100)
  if (x % 2 != 0)
  foreach (var y in Enumerable.Range(0, 100)
  if (y % 3 != 0)
  yield return new Point(x, y);
}
```


と書く方がパフォーマンスがよくなります。
たいていの場合、これだけで数割ほどパフォーマンスよくなったりします。
ループがもっと深いと下手すると何倍もよくなることも。

で、多重 from のあるクエリ式も、多重 foreach ループと似たようなもので、
from と where の順序を入れ替えるだけでパフォーマンスがよくなります。
例えば、

```csharp
var answers1 =
  from baker in five
  from cooper in five
  from fletcher in five
  from miller in five
  from smith in five
  where Distinct(baker, cooper, fletcher, miller, smith)
  where baker != 5
  where cooper != 1
  where fletcher != 1 && fletcher != 5
  where miller > cooper
  where Discrete(smith, fletcher)
  where Discrete(fletcher, cooper)
  select new { baker, cooper, fletcher, miller, smith };
```


と書くよりも、

```csharp
var answers2 =
  from baker in five
  where baker != 5
  from cooper in five
  where cooper != 1
  from fletcher in five
  where fletcher != 1 && fletcher != 5
  where Discrete(fletcher, cooper)
  from miller in five
  where miller > cooper
  from smith in five
  where Discrete(smith, fletcher)
  where Distinct(baker, cooper, fletcher, miller, smith)
  select new { baker, cooper, fletcher, miller, smith };
```


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


## <a id="sec-generated-title-6"></a> <a id="reconst"></a>クエリ式の再構築

ということで、クエリ式の順序最適化をしたいなぁと思うわけですが、
そのためにはまず、
クエリ式をデータとして扱える必要があります。

「[ラムダ式](../functional/sp3_lambda.md#lambda)」を使えば、
匿名デリゲートと同じ記法で「[式木](../dynamic/sp3_expression.md#expressiontree)」が作れます。
ですが、クエリ式の場合には、
C# の仕様上、メソッド呼び出しに変換されてしまうわけです。
要するに、以下のようなクエリ式は、

```csharp
var q =
  from x in list
  where x > 0
  select x;
```


以下のようなメソッド呼び出しに変換されます。

```csharp
var q = list.Where(x => x > 0);
```


ということで、メソッド呼び出しの形になっている式木から、
元のクエリ式を復元するようなプログラムを書いてみました。

* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/ExpressionTree.zip)



ソース中の QueryExpression.cs が件の処理をしてる部分。

以下のようなコードで、

```csharp
var q0 = Make.Expression((IEnumerable<int> five) =>
  from baker in five
  from cooper in five
  from fletcher in five
  from miller in five
  from smith in five
  where Distinct(baker, cooper, fletcher, miller, smith)
  where baker != 5
  where cooper != 1
  where fletcher != 1 && fletcher != 5
  where miller > cooper
  where Discrete(smith, fletcher)
  where Discrete(fletcher, cooper)
  select new { baker, cooper, fletcher, miller, smith }
  );

var q = new QueryExpression(q0);

foreach (var l in q.Queries)
{
  Console.Write("{0}\n", l);
}
```


以下のような出力を得ます。

```console
from baker in five
from cooper in five
from fletcher in five
from miller in five
from smith in five
where Distinct(new [] {baker, cooper, fletcher, miller, smith})
where (baker != 5)
where (cooper != 1)
where ((fletcher != 1) && (fletcher != 5))
where (miller > cooper)
where Discrete(smith, fletcher)
where Discrete(fletcher, cooper)
select new <>f__AnonymousType5`5(baker, cooper, fletcher, miller, smith)
```
