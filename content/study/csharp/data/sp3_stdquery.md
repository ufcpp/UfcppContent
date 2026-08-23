---
title: "標準クエリ演算子（クエリ式関係）"
source_url: "https://ufcpp.net/study/csharp/data/sp3_stdquery/"
content_type: "Article"
published_at: "2007-09-01T00:00:00"
updated_at: "2008-08-15T00:00:00"
tags:
  - "Ver. 3.0"
umbraco_id: 1304
parent_id: 1298
sort_order: 5
aliases:
  - "/study/csharp/sp3_stdquery.html"
---

# 標準クエリ演算子（クエリ式関係）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version3">Ver. 3.0</h5>

「[LINQ](sp3_linq.md)」で、

* 構文の追加（クエリ式）： C# 3.0 で、SQL 風の問い合わせ構文が書けるようになった。

* メソッド群の追加（標準クエリ演算子）： クエリ式は、Where、Select などのメソッド呼び出しに変換される。（これらのメソッドを標準クエリ演算子と呼ぶ。）


という話をしました。

ここでは、
どういう C# クエリ式がどういう標準クエリ演算子（メソッド呼び出し）に変換されるかを説明しつつ、
クエリ式に関係する標準クエリ演算子を紹介します。

（ちなみに、標準クエリ演算子の中には、クエリ式で表せない
（メソッド呼び出しの形でしか使えない）ものも多数あります。
次節の「[標準クエリ演算子（その他）](sp3_stdqueryo.md)」で説明。）


## <a id="sec-generated-title-2"></a> <a id="sampledata"></a>サンプルデータ

次節以降の説明では、例として以下のようなデータを使います。

```csharp {title="クエリ式説明のためのデータ例"}
var a = new[]
{
  new { X = 0, Y = 10, Z = new[]{ 1, 2, 3} },
  new { X = 1, Y = 11, Z = new[]{ 4, 5, 6} },
  new { X = 2, Y = 12, Z = new[]{ 7, 8, 9} },
  new { X = 3, Y = 13, Z = new[]{ 0, 1, 2} },
  new { X = 4, Y = 14, Z = new[]{ 3, 4, 5} },
};
```



## <a id="sec-generated-title-3"></a> <a id="queryex"></a>クエリ式の構成要素

C# 3.0 で導入されたクエリ式の構成要素は以下のとおりです。

<table summary="クエリ式の構成要素">
	<caption>
		クエリ式の構成要素
	</caption>
	<tr>
		<th>要素</th>
		<th>概要</th>
		<th>関連する標準クエリ演算子</th>
	</tr>
	<tr>
		<td markdown="1">「[from 句](#from)」</td>
		<td markdown="1">データの取り出し元を指定します。</td>
		<td markdown="1">Select, SelectMany, Cast</td>
	</tr>
	<tr>
		<td markdown="1">「[where 句](#where)」</td>
		<td markdown="1">データを与えられた条件に沿ってフィルタリングします。</td>
		<td markdown="1">Where</td>
	</tr>
	<tr>
		<td markdown="1">「[select 句](#select)」</td>
		<td markdown="1">データの出力形式を指定します。（要するに、データを加工する。）</td>
		<td markdown="1">Select</td>
	</tr>
	<tr>
		<td markdown="1">「[group ... by 句](#groupby)」</td>
		<td markdown="1">キーを指定して、キーの値の等しいものをグループ化します。</td>
		<td markdown="1">GroupBy</td>
	</tr>
	<tr>
		<td markdown="1">into</td>
		<td markdown="1">join 句、group 句、select 句の後ろにさらにクエリを続ける場合に使います。</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1">「[orderby 句](#orderby)」</td>
		<td markdown="1">データシーケンスを整列します。</td>
		<td markdown="1">OrderBy, OrderByDescending, ThenBy, ThenByDescending</td>
	</tr>
	<tr>
		<td markdown="1">「[join 句](#join)」</td>
		<td markdown="1">2つのデータシーケンスを1つに結合します。</td>
		<td markdown="1">Join, GroupJoin</td>
	</tr>
	<tr>
		<td markdown="1">「[let 句](#let)」</td>
		<td markdown="1">クエリ式中で計算した値を変数に格納します。</td>
		<td markdown="1">Select</td>
	</tr>
</table>



## <a id="sec-generated-title-4"></a> <a id="from"></a>クエリ変換と from

C# 3.0 のクエリ式は <strong id="from" class="keyword">from 句</strong>から始まります。
（SQL と違って、from が一番最初に来るのは、上から順番にクエリ式を解釈できるようにするため。
あと、from が一番上にないと、Visual Studio のインテリセンスとの相性が悪かったらしい。）

また、クエリ式は select 句または group 句で終わります。
select, group については次節以降で説明します。

```csharp {title="from"}
var b =
  from p in a
  select p.X;
```


繰り返しになりますが、C# 3.0 自体がクエリの解釈能力を持っているわけではなく、
この式は以下のようなメソッド呼び出しに変換されます。

```csharp {title="from の変換結果"}
var b = a.Select(p => p.X);
```


このように、from p in a と書くなら、
a に対する標準クエリ演算子メソッド呼び出しに変換されます。
また、p はラムダ式の仮引数になります。


## <a id="sec-generated-title-5"></a> <a id="basis"></a>基本

基本的には、クエリ式は上から順番に、句単位でメソッド呼び出しに変換されます。
要するに、例えば、

```csharp {title="クエリ式の例"}
var b =
  from p in a
  where p.Y < 12
  select p.X;
```


というクエリ式の場合、
上から順に、表1のように変換されます。

<table summary="変換例">
	<caption>
		変換例
	</caption>
	<tr>
		<th>クエリ式</th>
		<th>変換結果</th>
	</tr>
	<tr>
		<td markdown="1"><code>
            from p in a
          </code></td>
		<td markdown="1"><code>
            a
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>
            where p.Y &lt; 12
          </code></td>
		<td markdown="1"><code>
            .Where(p =&gt; p.Y &lt; 12)
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>
            select p.X
          </code></td>
		<td markdown="1"><code>
            .Select(p =&gt; p.X)
          </code></td>
	</tr>
</table>


ただし、
末尾の Select は省略されることがあります。
例えば、以下のようなクエリ式は、
<code>a.Where(p =&gt; p.Y &lt; 12);</code> だけになります。
（要するに、Select の中身が <code>.Select(x =&gt; x)</code> みたいに、素通しになる場合。）

```csharp {title="末尾の Select が省略される場合" highlight-ranges="sha256:15f683cdfe258c152c0f4dc0ef55e5262d0c7771b627ba3e0867235b695d529b;2:8-2:9,4:10-4:11"}
var b =
  from p in a
  where p.Y < 12
  select p;
```


select や group by の後ろにさらにクエリを続けたい場合には、
select / group by 句の後ろに into をつけます。
例えば、

```csharp {title="select into"}
var b =
  from p in a
  select p.X into x
  where x > 2
  select x;
```


は、以下のように変換されます。

```csharp {title="select into 変換結果"}
var b = a
  .Select(p => p.X)
  .Where(x => x > 2);
```



## <a id="sec-generated-title-6"></a> <a id="cast"></a>Cast

from 句では、from の直後に型を指定することができます。

```csharp {title="from 句で型を指定"}
var a = new[,] { { 1, 2 }, { 3, 4 } };

var b =
  from int p in a
  select p;
```

これは、以下のように Cast 演算子に変換されます。

```csharp {title="Cast 演算子への変換"}
var b = 
  from p in a.Cast<int>()
  select p;
```


この場合、select 句で何もしていない（素通し）ので、
最終的には以下のように解釈されます。

```csharp {title="最終的な変換結果"}
var b = a.Cast<int>();
```

ちなみに、非ジェネリック版の`IEnumerable`インターフェイス(`System.Collections`名前空間)に対して使えるLINQ標準演算子は、
この`Cast<T>`メソッドと、あともう1つ、`OfType<T>`メソッドの2つだけです。`Cast<T>`が`T`に変換できない要素があった場合は`InvalidCastException`を投げるのに対して、`OfType<T>`は`T`に変換できたものだけを通します。

[多次元配列](../structured/st_array.md#multid)はなぜか、非ジェネリック`IEnumerable`しか実装していません。その他、一部の古くからある型には、ジェネリック導入前との互換性維持のために、非ジェネリック`IEnumerable`のままのものがあります。例えば、正規表現ライブラリの`Regex`クラス(`System.Text.RegularExpressions`名前空間)の`Matches`メソッドなどは、非ジェネリック`IEnumerable`を返します。これらに対して、`from`直後の型指定や、`Cast<T>`メソッドが有効です。


## <a id="sec-generated-title-7"></a> <a id="select"></a>Select

Select 演算子（射影演算子、projection）は、どういう形式でデータを出力するかを選択します。

クエリ式中の <strong id="select" class="keyword">select 句</strong>は Select 演算子に変換されます。
以下に、select の例、その出力結果、および、Select 演算子への変換結果を示します。

```csharp {title="select" highlight-text="select p.X;"}
var b =
  from p in a
  select p.X; // X だけ取り出す。

foreach (var p in b)
  Console.Write("{0} ", p);
```


```console {title="select"}
0 1 2 3 4 
```


```csharp {title="select の変換結果"}
var b = a.Select(p => p.X);
```


ちなみに、クエリ式では通常、select 句を書くとそこで処理が終了します。
select の後にさらにクエリを続けたい場合
（標準クエリ演算子で書くなら、a.Select(...).Where(...); のようなことをしたい場合）、
以下のように select ... into を利用します。

```csharp {title="select ... into の例"}
var b =
  from p in a
  select new { p.X, p.Y } into x
  where x.X > 2
  select x;

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```csharp {title="select ... into の変換結果"}
var b =
  a.Select(p => new { p.X, p.Y })
  .Where(p => p.X > 2);
```


ちなみに、この into は、以下のような2段クエリに変換されていると考えてもいいようです。

```csharp {title="into と2段クエリ"}
  from x in
    from p in a
    select new { p.X, p.Y }
  where x.X > 2
  select x;
```



## <a id="sec-generated-title-8"></a> <a id="let"></a>透過識別子と let

<strong id="let" class="keyword">let 句</strong>を使うことで、
クエリ式中で計算した値を変数に格納しておくことができます。

```csharp {title="let"}
var b =
  from p in a
  let sumZ = p.Z.Sum()
  select new { p.X, sumZ };

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="let"}
{ X = 0, sumZ = 6 }
{ X = 1, sumZ = 15 }
{ X = 2, sumZ = 24 }
{ X = 3, sumZ = 3 }
{ X = 4, sumZ = 12 }
```


このクエリ式は、以下のような2重クエリと同じ意味になります。

```csharp {title="let と2重クエリ"}
var b =
  from p2 in
    from p in a
    select new { p, sumZ = p.Z.Sum() }
  select new { p2.p.X, p2.sumZ };
```


これはまた、以下のような select into 句と同じ意味です。

```csharp {title="let と select into"}
var b =
  from p in a
  select new { p, sumZ = p.Z.Sum() } into p2
  select new { p2.p.X, p2.sumZ };
```


さらに、以下のように Select 演算子に変換されます。

```csharp {title="let の変換結果"}
var b = a
  .Select(p => new { p, SumZ = p.Z.Sum() })
  .Select(p2 => new { p2.p.X, p2.SumZ });
```


この式では、
元のクエリ式と比べて余計なダミーの変数 p2 が増えています。
まあ、実際は逆で、元のクエリ式の方で「ダミーの変数を省略して書ける」というというのが正しいです。
この、元のクエリ式中では見えていない（省略されている）変数を<strong id="transparent" class="keyword">透過識別子</strong>（transparent identifier）といったりするようです。


## <a id="sec-generated-title-9"></a> <a id="where"></a>Where

Where 演算子（制限演算子、restriction）は、指定した条件を満たすデータのみを取り出します。

クエリ式中の <strong id="where" class="keyword">where 句</strong>は Where 演算子に変換されます。
以下に、where の例、その出力結果、および、Where 演算子への変換結果を示します。

```csharp {title="where" highlight-text="where p.X &gt; 2"}
var b = 
  from p in a
  where p.X > 2 // この条件を満たすものだけ取り出す
  select p;

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="where"}
{ X = 3, Y = 13, Z = System.Int32[] }
{ X = 4, Y = 14, Z = System.Int32[] }
```


```csharp {title="where の変換結果"}
var b = a.Where(p => p.Y > 0);
```



## <a id="sec-generated-title-10"></a> <a id="selectmany"></a>SelectMany

SelectMany 演算子は、1対多の射影を行います。

例えば、select を使って Z プロパティ（int 型の配列）を射影すると、
結果は「int 型の配列のリスト（IEnumerable&lt;int[]&gt;）」になります。

```csharp {title="select を使うと"}
var b =
  from p in a
  where p.X > 2
  select p.Z;

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="select 時"}
System.Int32[]
System.Int32[]
```


一方、SelectMany を使うと、「配列のリスト」が1本のリスト（正確には IEnumerable）に展開されます。

```csharp {title="SelectMany を使うと"}
var b = a.Where(p => p.X > 2).SelectMany(p => p.Z);
foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="select 時"}
0
1
2
3
4
5
```


クエリ式では、from 句を2重に使った場合に SelectMany に変換されます。
以下に、2重の from 句の例、その出力、および、SelectMany への変換結果を示します。

```csharp {title="2重の from"}
var b =
  from p in a
  where p.X > 2
  from q in p.Z
  select new { p.X, Z = q };

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="2重 from の結果"}
{ X = 3, Z = 0 }
{ X = 3, Z = 1 }
{ X = 3, Z = 2 }
{ X = 4, Z = 3 }
{ X = 4, Z = 4 }
{ X = 4, Z = 5 }
```


```csharp {title="SelectMany への変換結果"}
var b =
  a.Where(p => p.X > 2)
  .SelectMany(p => p.Z, (p, q) => new {p, q})
  .Select(pq => new {pq.p.X, Z = pq.q});

foreach (var p in b)
  Console.Write("{0}\n", p);
```


let に引き続き、
ここでも「[透過識別子](#transparent)」（元のクエリ式中では省略されているダミーの変数）pq が出てきます。
SelectMany に限らず、クエリ式が2重以上になっているものを標準クエリ演算子呼び出しに変換する場合、
透過識別子が必要になることが多いです。


## <a id="sec-generated-title-11"></a> <a id="join"></a>Join、GroupJoin

Join および GroupJoin 演算子（結合演算子）は、2つのデータシーケンスを1つに結合します。

クエリ式では、
Join 演算子は <strong id="join" class="keyword">join 句</strong>、
GroupJoin 演算子は join ... into 句に相当します。

例えば、この節のはじめに定義したデータ a に加えて、
以下のようなデータ a2 を用意します。

```csharp {title="2つのデータシーケンス"}
var a = new[] {
  new { X = 0, Y = 10, Z = new[]{ 1, 2, 3} },
  new { X = 1, Y = 11, Z = new[]{ 4, 5, 6} },
  new { X = 2, Y = 12, Z = new[]{ 7, 8, 9} },
  new { X = 3, Y = 13, Z = new[]{ 0, 1, 2} },
  new { X = 4, Y = 14, Z = new[]{ 3, 4, 5} },
};
var a2 = new[] {
  new { X = 0, W = 1 },
  new { X = 0, W = 2 },
  new { X = 1, W = 3 },
  new { X = 1, W = 4 },
};
```


それぞれ X をキーとして結合する（それぞれの X の値が等しい行をくっつける）と、
Join （join 句）の場合には、

```csharp
from p in a
join q in a2 on p.X equals q.X
```


というクエリ式で

```console {title="Join 後のデータの模式図"}
{ p = { X = 0, Y = 10, Z = { 1, 2, 3} }, q = { X = 0, W = 1 } }
{ p = { X = 0, Y = 10, Z = { 1, 2, 3} }, q = { X = 0, W = 2 } }
{ p = { X = 1, Y = 11, Z = { 4, 5, 6} }, q = { X = 1, W = 3 } }
{ p = { X = 1, Y = 11, Z = { 4, 5, 6} }, q = { X = 1, W = 4 } }
```


というようなデータシーケンスを、
GroupJoin の場合には、

```csharp
from p in a
join q in a2 on p.X equals q.X into r
```


というクエリ式で

```console {title="GroupJoin 後のデータの模式図"}
{
  p = { X = 0, Y = 10, Z = { 1, 2, 3} },
  r = { { X = 0, W = 1 }, { X = 0, W = 2 } }
}
{
  p = { X = 1, Y = 11, Z = { 4, 5, 6} },
  r = { { X = 1, W = 3 }, { X = 1, W = 4 } }
{
  p = { X = 2, Y = 12, Z = { 7, 8, 9} },
  r = {}
}
{
  p = { X = 3, Y = 13, Z = { 0, 1, 2} },
  r = {}
}
{
  p = { X = 4, Y = 14, Z = { 3, 4, 5} },
  r = {}
}
```


というようなデータシーケンスを作ります。

以下に、join 句の例と、その出力結果、および、Join 演算子への変換結果を示します。

```csharp {title="join 句の例"}
var b =
  from p in a
  join q in a2 on p.X equals q.X
  select new { p.X, p.Y, q.W };

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="join 句の出力結果"}
{ X = 0, Y = 10, W = 1 }
{ X = 0, Y = 10, W = 2 }
{ X = 1, Y = 11, W = 3 }
{ X = 1, Y = 11, W = 4 }
```


```csharp {title="Join 演算子への変換結果"}
var b =
  a.Join(a2, p => p.X, q => q.X,
    (p, q) => new { p.X, p.Y, q.W });
```


以下に、join ... into 句の例と、その出力結果、および、GroupJoin 演算子への変換結果を示します。

```csharp {title="join ... into 句の例"}
var b =
  from p in a
  join q in a2 on p.X equals q.X into r
  select new { p.X, p.Y, SumW = r.Sum(q => q.W) };

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="join ... into 句の出力結果"}
{ X = 0, Y = 10, SumW = 3 }
{ X = 1, Y = 11, SumW = 7 }
{ X = 2, Y = 12, SumW = 0 }
{ X = 3, Y = 13, SumW = 0 }
{ X = 4, Y = 14, SumW = 0 }
```


```csharp {title="GroupJoin 演算子への変換結果"}
var b =
  a.GroupJoin(a2, p => p.X, q => q.X,
    (p, r) => new { p.X, p.Y, SumW = r.Sum(q => q.W) });
```



## <a id="sec-generated-title-12"></a> <a id="orderby"></a>OrderBy、ThenBy

OrderBy、OrderByDescending、ThenBy、ThenByDescending 演算子（順序付け演算子、ordering）でデータシーケンスを整列させることができます。

これらはクエリ式の <strong id="orderby" class="keyword">orderby 句</strong>に相当します。
orderby 句には複数の整列キーを指定できますが、
1つ目のキーに対する整列は OrderBy / OrderByDescending  に、
2つ目以降のキーは ThenBy / ThenByDescending に変換されます。

orderby 句でキーを複数指定した場合、
前に書いたキーほど優先されます。
例えば、<code>orderby p.X, p.Y</code> と書いた場合、
X の値が等しいところでのみ Y の値の大小関係を使って整列されます。

OrderBy、ThenBy は昇順（小さい値 → 大きい値）に、
OrderByDescending、ThenByDescending は降順（大きい値 → 小さい値）に整列します。
orderby 句で何も指定しないか ascending を指定すると昇順（OrderBy / ThenBy）に、
descending を指定すると降順（OrderByDescending / ThenByDescending）になります。

以下に、orderby の例、その出力結果、および、コンパイラによる問い合わせ構文の変換結果を示します。

```csharp {title="orderby 句の例"}
var a = new[] {
  new { X = 1, Y = 0, Z = 1 },
  new { X = 0, Y = 1, Z = 0 },
  new { X = 1, Y = 2, Z = 3 },
  new { X = 2, Y = 0, Z = 2 },
  new { X = 0, Y = 0, Z = 7 },
  new { X = 1, Y = 1, Z = 5 },
  new { X = 2, Y = 0, Z = 1 },
};

var b =
  from p in a
  orderby p.X, p.Y descending, p.Z ascending
  select p;

foreach (var p in b)
  Console.Write("{0}\n", p);
```


```console {title="orderby"}
{ X = 0, Y = 1, Z = 0 }
{ X = 0, Y = 0, Z = 7 }
{ X = 1, Y = 2, Z = 3 }
{ X = 1, Y = 1, Z = 5 }
{ X = 1, Y = 0, Z = 1 }
{ X = 2, Y = 0, Z = 1 }
{ X = 2, Y = 0, Z = 2 }
```


```csharp {title="orderby の変換結果"}
var b =
  a.OrderBy(p => p.X)
  .ThenByDescending(p => p.Y)
  .ThenBy(p => p.Z);
```


ちなみに、OrderBy 演算子は、
LINQ to Object（IEnumerable 実装クラスに対する LINQ）で使ってもあまり効率はよくないです。
（どうせ内部的に一度リスト化される。）
LINQ to Object で、特に安定性が必要ない場合には、
ToList() などを使って一度 List に変換してから、
List.Sort メソッドで整列する方が実行速度がはるかによさそうです。

ただ、List.Sort と違って、
「[安定](../../algorithm/sort/sort.md#stable)」なソートになるようです。
（挙動的に、おそらくマージソート？
実行時間が List.Sort（おそらくクイックソート）の4倍程度。）


## <a id="sec-generated-title-13"></a> <a id="groupby"></a>GroupBy

GroupBy 演算子（グループ化演算子、grouping）は、キーを指定して、値の等しい物をグループ化します。

例えば、

```csharp {title="グループ化したいデータの例"}
var a2 = new[] {
  new { X = 0, W = 1 },
  new { X = 0, W = 2 },
  new { X = 1, W = 3 },
  new { X = 1, W = 4 },
};
```


というデータシーケンスを X でグループ化するなら、

```console {title="グループ化後のデータの構造"}
Key = 0, { 1, 2 }
Key = 1, { 3, 4 }
```


というような（キー付きのシーケンスの）シーケンスが得られます。

クエリ式中の <strong id="groupby" class="keyword">group ... by 句</strong>は GroupBy 演算子に変換されます。
以下に、group ... by の例、その出力結果、および、GroupBy 演算子への変換結果を示します。

```csharp {title="group ... by の例"}
var b =
  from p in a2
  group p.W by p.X;

foreach (var p in b)
{
  Console.Write("{0} -> ( ", p.Key);
  foreach (var q in p)
    Console.Write("{0} ", q);
  Console.Write(")\n");
}
```


```console {title="group by の出力結果"}
0 -> ( 1 2 )
1 -> ( 3 4 )
```


```csharp {title="GroupBy への変換結果"}
var b = a2.GroupBy(p => p.X, p => p.W);
```


ちなみに、
select 句と同様に、
group ... by 句も後ろにクエリを続けることはできません。
group ... by の後にさらにクエリを続ける場合、
group ... by ... into 句を使います。

```csharp {title="group ... by ... into 句の例"}
var b =
  from p in a2
  group p.W by p.X into g
  where g.Sum(q => q) > 5
  select g;

foreach (var p in b)
{
  Console.Write("{0} -> ( ", p.Key);
  foreach (var q in p)
    Console.Write("{0} ", q);
  Console.Write(")\n");
}
```


```console {title="group by into の例の出力結果"}
1 -> ( 3 4 )
```


```csharp {title="GroupBy への変換結果"}
var b =
  a2.GroupBy(p => p.X, p => p.W)
  .Where(g => g.Sum(q => q) > 5);
```



## <a id="sec-generated-title-14"></a> <a id="summary"></a>まとめ

C# 3.0 のクエリ式では、以下のようなクエリが可能です。

<table summary="クエリ式と標準クエリ演算子">
	<caption>
		クエリ式と標準クエリ演算子
	</caption>
	<tr>
		<th>説明</th>
		<th>クエリ式中の句</th>
		<th>標準クエリ演算子</th>
	</tr>
	<tr>
		<td markdown="1">射影演算</td>
		<td markdown="1">select、select ... into</td>
		<td markdown="1">Select</td>
	</tr>
	<tr>
		<td markdown="1">制限演算</td>
		<td markdown="1">where</td>
		<td markdown="1">Where</td>
	</tr>
	<tr>
		<td markdown="1">連結演算</td>
		<td markdown="1">join,、join ... into</td>
		<td markdown="1">Join、GroupJoin</td>
	</tr>
	<tr>
		<td markdown="1">順序付け演算</td>
		<td markdown="1">orderby</td>
		<td markdown="1">OrderBy、OrderByDescending、ThenBy、ThenByDescending</td>
	</tr>
	<tr>
		<td markdown="1">グループ化演算</td>
		<td markdown="1">group ... by、group ... by ... into</td>
		<td markdown="1">GroupBy</td>
	</tr>
</table>


また、クエリ式としては利用できない
（メソッド呼び出しとしてだけ利用できる）
標準クエリ演算子として、Take、Skip などもあります。

最後に、これまでに説明してきたクエリ式を使ったサンプルを1つ。


##### <a id="sec-generated-title-15"></a>サンプル

```csharp {title="クエリ式総まとめ"}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      // データテーブルを2つほど定義。
      var studentList = new[] {
        new {id =  0, 姓 = "糸色", 名 = "望"    },
        new {id = 13, 姓 = "風浦", 名 = "可符香"},
        new {id = 20, 姓 = "小森", 名 = "霧"    },
        new {id = 22, 姓 = "常月", 名 = "まとい"},
        new {id = 19, 姓 = "小節", 名 = "あびる"},
        new {id = 18, 姓 = "木村", 名 = "カエレ"},
        new {id = 14, 姓 = "音無", 名 = "芽留"  },
        new {id = 17, 姓 = "木津", 名 = "千里"  },
        new {id =  8, 姓 = "関内", 名 = "マリア"},
        new {id = 28, 姓 = "日塔", 名 = "奈美"  },
        new {id =  6, 姓 = "久藤", 名 = "准"    },
        new {id = 29, 姓 = "藤吉", 名 = "晴美"  },
        new {id = 30, 姓 = "三珠", 名 = "真夜"  },
        new {id = 16, 姓 = "加賀", 名 = "愛"    },
        new {id = 15, 姓 = "大草", 名 = "麻菜実"},
      };
      var remarks = new[] {
        new {id =  0, 備考="超ネガティブ"},
        new {id = 13, 備考="超ポジティブ"},
        new {id = 20, 備考="ひきこもり"},
        new {id = 22, 備考="超恋愛体質"},
        new {id = 22, 備考="ストーカー"},
        new {id = 19, 備考="しっぽ好き"},
        new {id = 19, 備考="被DV疑惑"},
        new {id = 18, 備考="人格バイリンガル"},
        new {id = 14, 備考="毒舌メール"},
        new {id = 17, 備考="几帳面"},
        new {id = 17, 備考="粘着質"},
        new {id =  8, 備考="不法入国"},
        new {id =  8, 備考="難民"},
        new {id = 28, 備考="普通"},
        new {id =  6, 備考="天才ストーリーテラー"},
        new {id = 29, 備考="耳好き"},
        new {id = 29, 備考="カップリング中毒"},
        new {id = 30, 備考="見たまま少女"},
        new {id = 16, 備考="加害妄想少女"},
        new {id = 15, 備考="主婦女子高生"},
        new {id = 15, 備考="多重債務者"},
      };

      // 2つのテーブルをくっつけてみる。
      var remarksWithName =
        from s in studentList
        join r in remarks on s.id equals r.id
        orderby s.id
        select new { 姓名 = s.姓 + s.名, r.備考 } into t1
        group t1.備考 by t1.姓名 into t2
        select new { 姓名 = t2.Key, 備考 = t2 };

      // 結果の表示。
      foreach (var s in remarksWithName)
      {
        Console.Write("{0} : ", s.姓名);
        foreach (var r in s.備考)
          Console.Write("{0} ", r);
        Console.Write("\n");
      }
    }
  }
}
```


```console {title="クエリ式総まとめ"}
糸色望 : 超ネガティブ
久藤准 : 天才ストーリーテラー
関内マリア : 不法入国 難民
風浦可符香 : 超ポジティブ
音無芽留 : 毒舌メール
大草麻菜実 : 主婦女子高生 多重債務者
加賀愛 : 加害妄想少女
木津千里 : 几帳面 粘着質
木村カエレ : 人格バイリンガル
小節あびる : しっぽ好き 被DV疑惑
小森霧 : ひきこもり
常月まとい : 超恋愛体質 ストーカー
日塔奈美 : 普通
藤吉晴美 : 耳好き カップリング中毒
三珠真夜 : 見たまま少女
```
