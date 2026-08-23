---
title: "複合型の分解"
source_url: "https://ufcpp.net/study/csharp/datatype/deconstruction/"
content_type: "Article"
published_at: "2016-08-22T00:00:00"
updated_at: "2021-09-20T15:39:50"
tags: []
umbraco_id: 1944
parent_id: 1940
sort_order: 1
aliases:
  - "/study/csharp/data/deconstruction"
  - "/study/csharp/data/deconstruction/"
---

# 複合型の分解

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<h5 class="version version7">Ver. 7</h5>

[タプル](tuples.md#key-tuple)から値を取り出す際には、メンバーを直接、それぞれバラバラに受け取りたくなることがあります。

「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
メンバー名だけ見ればその型が何を意味するか分かるからこそ型に名前が付かないわけです。
このとき、その型を受け取る変数にも、よい名前が浮かばなくなるはずです。

そこでC# 7では、タプルと同時に、分解(deconstruction)のための構文が追加されました。

## <a id="sec-generated-title-2"></a> <a id="deconstruction"></a>分解

以下のような、整数列の個数(count)と和(sum)を同時に計算するメソッドがあったとします。
「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
戻り値の型として「個数と和」みたいな名前(`CountAndSum`とか)しか思い浮かばないようなものです。

```csharp {title="個数と和を返すメソッド"}
static (int count, int sum) Tally(IEnumerable<int> items)
{
    var count = 0;
    var sum = 0;
    foreach (var x in items)
    {
        sum += x;
        count++;
    }

    return (count, sum);
}
```

そうなると、この結果を受け取る変数名も、「個数と和」以上の名前はつかないでしょう。
通常、ローカル変数であれば適当な名前でもそこまで問題ではないので、
`x`とか`y`とか、本当に意味がない名前を付けることになると思います。

```csharp {title="個数と和の受け取り"}
var x = Tally(new[] { 1, 2, 3, 4, 5 });
Console.WriteLine(x.count);
Console.WriteLine(x.sum);
```

実際にほしい名前は`count`と`sum`だけです。
であれば、最初から`count`変数と`sum`変数に分解して受け取りたいと思うでしょう。
要するに、以下のようなことを1行で書ける構文がほしいです。

```csharp {title="タプルの分解"}
// この3行に相当する構文がほしい
var x = Tally(new[] { 1, 2, 3, 4, 5 });
var count = x.count;
var sum = x.sum;
// 以後、もう x は使わない

Console.WriteLine(count);
Console.WriteLine(sum);
```

タプルのような名前の決まらない型は、この例のように分解して使うのが前提と言えます。

そこで、C# 7では、タプルと一緒に、以下のような分解のための構文を追加しました。

```csharp {title="分解代入構文"}
(var count, var sum) = Tally(new[] { 1, 2, 3, 4, 5 });
Console.WriteLine(count);
Console.WriteLine(sum);
```

ちなみに、この分解構文は、タプルか、後述する`Deconstruct`メソッドを持つ任意の型に対して使えます。

### <a id="sec-generated-title-3"></a> <a id="deconstruction-declaration"></a>分解宣言

以下のような書き方で、分解と同時に変数を宣言できます。
これを分解宣言(deconstruction declaration)と言います。

```csharp {title="分解宣言"}
// count, sum を宣言しつつ、タプルを分解
(int count, int sum) = Tally(items);

// ↓こう書くとタプル型の変数の宣言
// (int count, int sum) t = Tally(items);
```

この例の後半のコメントのように、分解宣言はタプルの型宣言の書き方によく似ています。
ただ、タプルの型宣言と違って、型推論の`var`が使えます。

```csharp {title="var での型推論付きの分解宣言"}
// 型推論で count, sum を宣言しつつ、タプルを分解
(var count, var sum) = Tally(items);

// ↓タプルだと var は使えない。これはコンパイル エラー
// (var count, var sum) t = Tally(items);
```

このとき、部分的に型推論(`var`)を使うこともできます。

```csharp {title="部分的に var を使う"}
// 部分的に var を使う
(var count, long sum) = Tally(items);
```

一方で、宣言したいすべての変数を型推論する場合であれば、先頭に1つだけ `var` キーワードを書く以下のような書き方もできます。

```csharp {title="var + 変数リスト"}
// 「var + 変数リスト」でタプルを分解
var (count, sum) = Tally(items);
```

この書き方は、`foreach`、`for`などでの変数宣言でも使えます。

```csharp {title="foreachやforの中で分解宣言"}
(int x, int y)[] array = new[] { (1, 2), (3, 4) };

foreach (var (x, y) in array)
{
    Console.WriteLine($"{x}, {y}");
}

for ((int i, int j) = (0, 0); i < 10; i++, j--)
{
    Console.WriteLine($"{i}, {j}");
}
```

(仕様書状はクエリ式の`let`、`from` でも使えることになっているものの、プレビュー版である現在は未実装。)

### <a id="sec-generated-title-4"></a> <a id="deconstruction-assignment"></a>分解代入

既存の変数を使って分解することもできます。
こちらは分解代入(deconstruction assignment)といいます。

```csharp {title="分解代入"}
int x, y;

// 既存の変数を使って分解
(x, y) = Tally(items);
```

文法説明のために簡素化したものとはいえ、この例では分解宣言で十分で、
再代入(既存の変数`x`、`y`の書き換え)の必要性があまりありません。
実際は、以下の例のように、ループで書き換えたりすることになるでしょう。

```csharp {title="分解代入で変数を書き換え"}
var x = 1.0;
var y = 5.0;

for (int i = 0; i < 100; i++)
{
    (x, y) = ((x + y) / 2, Math.Sqrt(x * y));
}
```

分解代入の左辺には、書き換え可能なものであれば何でも書けます。
例えば、配列アクセスや参照戻り値などを分解代入の左辺に書けます。

```csharp {title="配列アクセスや参照戻り値を使って分解代入"}
private static void DeconstractionAssingment()
{
    var a = new[] { 1, 2 };

    // 配列アクセス
    var b = new int[a.Length];
    (b[1], b[0]) = (a[0], a[1]);

    // 参照戻り値
    var c = new int[a.Length];
    (Mod(c, 5), Mod(c, 8)) = (a[0], a[1]);

    Console.WriteLine(string.Join(", ", b));
    Console.WriteLine(string.Join(", ", c));
}

static ref T Mod<T>(T[] array, int index) => ref array[index % array.Length];
```

フィールドに対しても使えるので、
例えば以下のように、コンストラクターを記述を簡素にできたりもします。

```csharp {title="分解代入を使ったコンストラクターの簡素化の例"}
struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y) => (X, Y) = (x, y);
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
```

### <a id="sec-generated-title-5"></a> <a id="deconstruction-expression"></a>タプル構築と分解の混在

タプルを作る構文と分解代入の構文は似ているわけですが、これらは、以下のようにつなげて書くこともできます。

```csharp {title="分解、かつ、タプル構築"}
int x, y;
var t = (x, y) = (1, 2);
```

これは、以下のように、分解後に改めてタプルを作るのと同じ意味になります。

```csharp {title="分解 → タプル構築"}
int x, y;
(x, y) = (1, 2); // 分解代入
var t = (x, y);  // 改めてタプルを構築
```

### <a id="sec-generated-title-6"></a> <a id="mixed-deconstruction"></a>分解宣言と分解代入の混在

<h5 class="version version10">Ver. 10</h5>

C# 10.0 では以下のように、分解代入と分解宣言の混在もできるようになりました。

```csharp {title="分解宣言と分解代入の混在" highlight-text="var u"}
int x;
(x, var u) = (1, 2);
```

ただし、式の途中に分解宣言 (var 付きの宣言) が来るようなコードは C# 10.0 でも書けません。

```csharp {title="ただし、分解宣言は式の途中には書けない"}
int x, y;
(x, var u) = (var v, y) = (1, 2);
```

## <a id="sec-generated-title-7"></a> <a id="conversion"></a>分解時の型変換

分解時には、[タプル間の型変換](tuples.md#conversion)と同じルールで暗黙の型変換が働きます。
すなわち、宣言位置で分解されます(メンバー名は見ない)し、メンバーごとに暗黙的型変換が効くなら分解でも暗黙的型変換が効きます。

```csharp {title="分解時の型変換"}
// Tally の戻り値は (count, sum) の順
var t = Tally(new[] { 1, 2, 3, 4, 5 });

// sum = t.count, count = t.sum の意味になるので注意が必要
(int sum, int count) = t;
Console.WriteLine(sum);   // 5
Console.WriteLine(count); // 15

// int → object も int → long も暗黙的に変換可能
// なので、分解もでもこの変換が暗黙的に可能
(object x, long y) = t;
```

## <a id="sec-generated-title-8"></a> <a id="arbitrary-types"></a>任意の型を分解

C#の言語機能としてのタプルの他にも、
タプルに類する型はあります。
すなわち、意味のある変数が作れず、分解して使う前提の型です。

代表例は`KeyValuePair`構造体(`System.Collections.Generic`名前空間)でしょう。
`key`と`value`という変数で分解して受け取りたいです。

また、C#の構文としてタプルが導入される以前に使っていた型ですが、
`Tuple`クラス(`System`名前空間)というものがあります。
メンバー名まで紛失してしまうので使い勝手はよくありませんが、
「型名がうまく付けられない時に使う型」です。

これらの型に対しても分解構文を使いたいです。
そこで、C# 7では、`Deconstruct`という名前のインスタンス メソッド、もしくは、拡張メソッドさえ持っていれば、
どんな型でも分解構文使えるようにしました。
例として`KeyValuePair`と`Tuple`に対する`Deconstruct`の書き方を示しましょう。
以下のような拡張メソッドがあれば分解できます。

```csharp {title="KeyValuePairとTupleの分解用のDeconstructメソッド"}
static class Extensions
{
    public static void Deconstruct<T, U>(this KeyValuePair<T, U> pair, out T key, out U value)
    {
        key = pair.Key;
        value = pair.Value;
    }

    public static void Deconstruct<T1, T2>(this Tuple<T1, T2> x, out T1 item1, out T2 item2)
    {
        item1 = x.Item1;
        item2 = x.Item2;
    }
}
```

(ちなみに、.NET Core 2.0 以降か .NET Standard 2.1 以降であれば、`KeyValuePair` にはインスタンス メソッドとして標準で`Deconstruct`メソッドが追加されています。
`Tuple` の方は .NET Standard 2.0 以降であれば拡張メソッドとして`Deconstruct`メソッドがあります。)

これで、`KeyValuePair`と`Tuple`に対して分解構文が使えます。以下のようなコードが書けます。

```csharp {title="任意の型に対する分解宣言"}
var pair = new KeyValuePair<string, int>("one", 1);
var (k, v) = pair;
// 以下のようなコードに展開される
// string k;
// int v;
// pair.Deconstruct(out k, out v);

var tuple = Tuple.Create("abc", 100);
var (x, y) = tuple;
// 以下のようなコードに展開される
// string x;
// int y;
// tuple.Deconstruct(out x, out y);
```

### <a id="sec-generated-title-9"></a> <a id="deconstruct-overload"></a>引数の数が同じオーバーロード不可

分解構文では、引数の数が同じ`Deconstruct`メソッドを呼び分けることができません。
例えば以下の例のように、引数の型が`double, double`のものと、`double, Radian`のものという2つの`Deconstruct`メソッドを定義してしまうと、2変数の分解ができなくなります。

```csharp {title="Deconstructメソッドの呼び分けができない(引数の数が同じ)例"}
using static System.Math;

struct Radian
{
    public double Value { get; }
    public Radian(double value) => Value = value;
}

struct Vector2D
{
    public double X { get; }
    public double Y { get; }

    // コンストラクターは当然、個数が同じでも、型が違えば呼び分けができる
    public Vector2D(double x, double y) => (X, Y) = (x, y);
    public Vector2D(double radius, Radian angle)
        : this(radius * Cos(angle.Value), radius * Sin(angle.Value)) { }

    // 引数の数が同じ Deconstruct が2つある
    // 片方だけならいいけど、2つあると分解ができなくなる
    public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
    public void Deconstruct(out double radius, out Radian angle)
        => (radius, angle) = (Sqrt(X * X + Y * Y), new Radian(Atan2(Y, X)));
}

class Program
{
    static void Main()
    {
        // コンストラクターの呼び分け
        var p = new Vector2D(1, 2);
        var q = new Vector2D(10, new Radian(PI / 5));

        // 分解は呼び分けできない
        (double x, double y) = q; // コンパイル エラー
        (double r, Radian a) = p; // コンパイル エラー
    }
}
```

一方で、引数の数が違えば複数の`Deconstruct`メソッドがあっても大丈夫です。
例えば以下のようなコードであれば、ちゃんと分解が使えます。

```csharp {title="Deconstructメソッドの呼び分けができる(引数の数が違う)例"}
struct Vector3D
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public Vector3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);

    // 引数の数が違えば大丈夫
    public void Deconstruct(out double x, out double y, out double z) => (x, y, z) = (X, Y, Z);
    public void Deconstruct(out double first, out Vector2D rest) => (first, rest) = (X, new Vector2D(Y, Z));
}

class Program
{
    static void Main()
    {
        var p = new Vector3D(1, 2, 3);

        // 分解可能
        var (first, rest) = p;
        var (x, y, z) = p;
    }
}
```

### <a id="sec-generated-title-10"></a> <a id="tuple-optimization"></a>タプルの構築や分解の最適化

分解構文は、基本的には`Deconstruct`メソッドの呼び出しに展開されます。
しかし、タプルに対しては、`Deconstruct`メソッドやコンストラクター呼び出しをなくす最適化が掛かります。

例えば以下のようなコード(いわゆるSwap処理)を書いたとします。

```csharp {title="タプル構築後にすぐに分解する例(swap)"}
var x = 1;
var y = 2;
(x, y) = (y, x);
```

もしタプルが一般の型と同列に扱われるのなら、
「[ValueTuple構造体への展開](tuples.md#tuple-ValueTuple)」で説明した内容や、
前述の`Deconstruct`に展開される仕様を考えると、
これは以下のような意味にとることができます。

```csharp {title="(一般の型の分解と同列に考える場合の)タプル構築と分解の展開結果"}
var t = new ValueTuple<int, int>(y, x);
t.Deconstruct(out x, out y);
```

しかし、タプルに限り、単なる一時変数の追加やメンバーアクセスに展開され得ます<sup>※</sup>。
上記の `(x, y) = (y, x)` は、以下のように展開できます。

```csharp {title="タプルの場合は構築も分解も最適化で消える"}
var t1 = y; // この t1 の方はさらに最適化で消える可能性あり
var t2 = x;
x = t1;
y = t2;
```

<sup>※</sup>実際にどこまで最適化されるかは実装依存です。
例えば、C# 7.0の頃には `new ValueTuple<int, int>(x, y)` が一度作られていましたし、
現在の実装では `t1` も消えて `var t = x; x = y; y = t;` 相当のコードが出力されます。

### <a id="sec-generated-title-11"></a> <a id="ValueTuple"></a>余談: System.ValueTuple 構造体を要求される

タプルによる分解を使う場合、C# コンパイラーは常に`ValueTuple`構造体を要求します([System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/)パッケージの参照が必要)。

「常に」というところが少し曲者です。
例えば以下のような2つのステートメントを考えます。

```csharp {title="タプル構築と、タプル構築＋分解"}
// タプルの仕様上、ValueTuple<int, int> 構造体が作られる
var t = (1, 2);

// 前述の通り、最適化が掛かるので ValueTuple は不要なはず
var (x, y) = (1, 2);
```

前者は実際に`ValueTuple`構造体を必要としているので問題はありません。必要なものの参照を要求しているだけです。
一方、後者は`ValueTuple`構造体を使わないにも関わらず、C# コンパイラーは`ValueTuple`構造体の参照を求めます。

このコードから「すぐに分解するから最適化で消える」というの判定するのはコンパイラーにとっては意外と大変らしく、
「頑張っても見合わない」とのことで、この仕様を変えるつもりは今のところないようです。

## <a id="sec-generated-title-12"></a> <a id="evaluation"></a>分解の評価のされ方

分解構文では、メンバーごとにそれぞれ代入するような結果を生みます。
このとき、以下のようなルールが働きます。

- メンバーの評価は左から順
- メンバーの書き換えは同時に起こる

単純な場合、例えば`(a, b) = (x, y);`のような時にはこんなルールを気にするまでもなく、`a = x; b = y;`と同じ結果になります。
ここで、もう少し複雑な場合を考えてみましょう。

まず、左右で同じ変数が出てくる場合についてです。
分解構文では、各メンバーへの代入が同時に行われるかのような結果を生みます。
例えば、`x`と`y`という2つの変数の値を入れ替え(swap)ようとするとき、逐次実行であれば、以下のような書き方は間違いです。

```csharp {title="逐次実行でのswap"}
var x = 1;
var y = 2;

y = x;
x = y; // 上の行で y が書き換わっているので、値の入れ替えにはならない

Console.WriteLine(x); // 1
Console.WriteLine(y); // 1

// 正しくは以下のように書く
// var temp = y;
// y = x;
// x = temp;
```

これが、分解代入を使って以下のように書くと、正しく値が入れ替わります。

```csharp {title="分解代入を使ったswap"}
var x = 1;
var y = 2;

// 分解代入であれば、値の書き換えは同時に起こる
(y, x) = (x, y);

Console.WriteLine(x); // 2
Console.WriteLine(y); // 1
```

値が並行して同時に書き換わっているような結果を得るために、一時変数が挟まります。

```csharp {title="実際の評価のされ方"}
// 左辺の (y, x) を受け取る一時変数をまず用意
var t1 = y;
var t2 = x;
// 一時変数から改めて代入
x = t1;
y = t2;
```

さらに複雑になるのは、式が副作用を持つ場合です。
例として、分解代入の両辺に、悪名高いインクリメント演算を混ぜてみましょう。
各メンバーは、左から順に評価されます。

```csharp {title="分解代入の両辺にインクリメントを混ぜてみる"}
var a = new[] { 0, 1, 2, 3 };
var i = 0;

(a[i++], a[i++]) = (a[i++], a[i++]);

Console.WriteLine(string.Join(", ", a)); // 2, 3, 2, 3
// つまり、以下の評価を受けてる
// (a[0], a[1]) = (a[2], a[3]);
```

これと同じ動作をタプルと分解なしで書くと、以下のようなコードになります。

```csharp {title="左から順に評価するため、一時変数が挟まる"}
var a = new[] { 0, 1, 2, 3 };
var i = 0;

ref var l1 = ref a[i++];
ref var l2 = ref a[i++];
var r1 = a[i++];
var r2 = a[i++];

l1 = r1;
l2 = r2;
```
２
