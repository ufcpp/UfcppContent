---
title: "並列処理ライブラリ"
source_url: "https://ufcpp.net/study/csharp/lib/lib_parallel/"
content_type: "Article"
published_at: "2009-05-24T00:00:00"
updated_at: "2015-05-06T14:13:06"
tags: []
umbraco_id: 1358
parent_id: 1350
sort_order: 6
aliases:
  - "/study/csharp/lib_parallel.html"
---

# 並列処理ライブラリ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version4">Ver. 4.0</h5>

マルチコア CPU の普及に伴って、並列処理の重要性が増しています。
この時代背景に合わせるかのように、.NET Framework 4で並列処理用のライブラリが追加されました。


## <a id="sec-generated-title-2"></a> <a id="parallel"></a>Parallel クラス

まずは、制御フロー（「[制御フロー](../structured/st_control.md)」参照）の並列化です。
Parallel クラス（System.Threading.Tasks 名前空間）を使うことで、
通常の for 文や foreach 文に非常に似た書き方で並列処理を行えます。

Parallel クラスは Invoke、For、ForEach の3つの静的メソッドを持っています。

<table summary="Parallel クラスを使った制御フローの並列化">
	<caption>
		Parallel クラスを使った制御フローの並列化
	</caption>
	<tr>
		<th>メソッド</th>
		<th>逐次処理版</th>
		<th>並列処理版</th>
	</tr>
	<tr>
		<td markdown="1">Invoke</td>
		<td markdown="1">
<pre class="source" title="3つのメソッドを逐次呼び出し" lang=""><code class="language-csharp">A();
B();
C();</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="3つのメソッドを並列呼び出し" lang=""><code class="language-csharp">Parallel.Invoke(A, B, C);</code></pre>

</td>
	</tr>
	<tr>
		<td markdown="1">For</td>
		<td markdown="1">
<pre class="source" title="0～N まで逐次処理" lang=""><code class="language-csharp">for (int i = 0; i &lt; N; i++)
{
    Console.WriteLine(i * i);
}</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="0～N まで並列処理" lang=""><code class="language-csharp">Parallel.For(0, N, i =&gt;
{
    Console.WriteLine(i * i);
});</code></pre>

</td>
	</tr>
	<tr>
		<td markdown="1">ForEach</td>
		<td markdown="1">
<pre class="source" title="data の要素を逐次列挙" lang=""><code class="language-csharp">var data = Enumerable.Range(0, N);
 
foreach (var x in data)
{
    Console.WriteLine(x * x);
}</code></pre>

</td>
		<td markdown="1">
<pre class="source" title="data の要素を並列列挙" lang=""><code class="language-csharp">var data = Enumerable.Range(0, N);
 
Parallel.ForEach(data, x =&gt;
{
    Console.WriteLine(x * x);
});</code></pre>

</td>
	</tr>
</table>


逐次処理とほとんど同じ書き方で並列処理ができます。

ただし、複数のスレッドから同じデータを読み書きする場合には「[排他制御](../async/sp_thread.md#exclusive)」が必要なので注意してください。
例えば、以下のような処理は、単に foreach 文を Parallel.ForEach メソッドに置き換えるだけでなく、
ロックが必要です。

```csharp
var data = Enumerable.Range(0, N);
 
var sum = 0;
foreach (var x in data)
{
    sum += x;
}
Console.WriteLine(sum);
```


以下のように、sum += x の部分にロックを掛けます。

```csharp
var data = Enumerable.Range(0, N);
 
var sum = 0;
Parallel.ForEach(data, x =>
{
    lock (data) sum += x;
});
Console.WriteLine(sum);
```


ロック自体がそれなりにオーバーヘッドのかかる処理なので、
この例の場合、並列化するとかえって遅くなる可能性があります。


## <a id="sec-generated-title-3"></a> <a id="plinq"></a>Parallel LINQ

「[LINQ](../data/sp3_linq.md#linq)」 に対する並列化の仕組みも用意されています。
System.Linq 名前空間に ParallelEnumerable というクラスが追加されていて、
このクラスで定義されている AsParallel 拡張メソッドを使えば、LINQ クエリを並列化できます。
（データ ソースに対して .AsParallel() を付けるだけです。）

```csharp
var data = Enumerable.Range(0, N);
var sqSum = data.AsParallel().Sum(x => x * x);
Console.WriteLine(sqSum);
```


必要な「[排他制御](../async/sp_thread.md#exclusive)」は適宜ライブラリ内で行ってくれるので、
こちらはロックが不要です。
なので、データ ストリーム（「[ストリームとパイプライン](../data/da_about.md#stream)」参照）に対する並列処理は、
Parallel クラスを使うよりも、こちらを使う方がおすすめです。

ただし、多少の工夫が必要な場合もあります。
例えば、1つ前の要素を参照したいというような場合、
以下のように書いてしまいがちです。

```csharp
// 1つ前の値を保存しておく
var prev = data.First();
var max = int.MinValue;
foreach (var x in data.Skip(1))
{
    // 階差の最大値
    max = Math.Max(x - prev, max);
    prev = x;
}
```


並列化したい場合、必ずしも順序の保証がないので、prev = x; では「1つ前の要素を保存」という処理になりません。
以下のような工夫が必要になります。

```csharp
// 1項ずらしたデータ ストリームと Zip
var difference = data.Zip(data.Skip(1), (i, j) => j - i);
// そのあと、AsParallel
var max = difference.AsParallel().Max();
```
