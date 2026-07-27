---
title: "レコード型"
source_url: "https://ufcpp.net/study/csharp/datatype/record/"
content_type: "Article"
published_at: "2021-05-02T00:00:00"
updated_at: "2023-03-19T17:14:13"
tags: []
umbraco_id: 2348
parent_id: 1940
sort_order: 5
aliases: []
---

# レコード型

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

C# 9.0 で、レコード型(records)という新しい種類の型が追加されました。
(また、C# 10.0 では構造体版レコード型(record structs)が追加されました。)

record (記録)という名前通り、データの読み書きに使うことを意図した型です。
例えば以下のような書き方で、「`Name` という文字列と `Birthday` という日付」を読み書きできます。

```csharp
using System;
 
record Person(string Name, DateTime Birthday);
```

## <a id="sec-generated-title-2"></a> <a id="data-centric">データが主役のプログラミング</a>

プログラミングをしていると、データが主役・データが中心になる場面がちらほらあります。
「データが主役」(data centric)というのは、例えば以下のように、「`Name` という文字列と、`Birthday` という日付を持っている」というような「何の型がどういうデータを記録しているか」という情報が強い意味を持つような場面です。

```csharp
using System;
 
class Person
{
    public string Name;
    public DateTime Birthday;
}
```

[オブジェクト指向](../oop/oo_about.md)ではこれと真逆の考え方をしたりします。
「[実装の隠ぺい](../oop/oo_conceal.md)」などで書いていますが、
オブジェクト指向では「内部的にどういうデータを持っているか」はあまり重要ではなく、
「外から見てどうふるまうか」の方が主役(behavior centric)になります。
例えば上記の例に当てはめるなら、`Birthday` (誕生日)はなんだったら `int` (単なる整数。例えば独自の基準日からの経過日数で表すなど)でデータを記録していてもよくて、「外から見て `DateTime` 型で `Birthday` を取れれば中身は何でもいい」みたいに考えます。

ところが、データを保存・復元したり、ネットワーク越しに送受信したり、GUI で表示・編集する場合、結局「どういうデータをどういう形式で持っているか」という内部的な情報がそのまま必要になったりします。
例えば上記の `Person` 型であれば、以下のような JSON 形式で保存して、これを読み書きしたりすることが結構あると思います。

```json
{
  "name": "天馬飛雄",
  "birthday": "2003/04/07"
}
```

### <a id="sec-generated-title-3"></a> <a id="data-boilerplate">ボイラープレートなコード</a>

上記の例に挙げた `Person` 型の作りは、話を単純化するために簡素化して書いたものですが、
これを「C# のお作法的に好ましい書き方」で書こうとすると実は結構なコード量を書く必要があります。
例えば以下のようなコードになります。

```csharp
class Person : IEquatable<Person>
{
    public string Name { get; init; }
    public DateTime Birthday { get; init; }
 
    public Person(string name, DateTime birthday)
    {
        Name = name;
        Birthday = birthday;
    }
 
    public bool Equals(Person? other)
        => other is { } person &&
            Name == person.Name &&
            Birthday == person.Birthday;
 
    public override bool Equals(object? obj)
        => obj is Person person &&
            Name == person.Name &&
            Birthday == person.Birthday;
 
    public override int GetHashCode()
        => HashCode.Combine(Name, Birthday);
 
    public override string ToString()
        => $"Person {{ Name = {Name}, Birthday = {Birthday} }}";
}
```

このコードで書いているのは以下のようなものです。

- immutable (1度作ったデータは書き換え不可能にする)にした方がいい
  - C# (8.0 以前)で immutable なデータ型を作ろうと思うと[コンストラクター](../oop/oo_construct.md)が必要になる
  - そうなると、コンストラクターの引数と、プロパティへの代入で同じ名前を何度も書く必要がある
- [参照型](../resource/oo_reference.md#reftype)であっても値的にふるまう(value semantics を持つ)ようにしたい
  - 2つのインスタンスが「全て同じプロパティ値を持つなら等しい」という判定にする
  - 等値判定(`Equals` メソッドや `GetHashCode` メソッド)を書く必要がある
- 必須ではないものの、`ToString` で中身のデータが見れると便利

これは、どんな「データ中心の型」でもほぼ同じものを書く必要があり、かなり定型コードです。
あまりにも同じものを繰り返し書かないといけないので「ボイラープレート」(boilerplate: 焼き型製品。タイ焼きとかみたいな同じ形状のものを大量生産するやつ)と呼ばれたりします。

当然、毎度毎度同じようなコードを繰り返し書かないといけないのはしんどいです。
「お作法的に好ましいものを書こうとするとボイラープレートがしんどい」という状態はあまり好ましくなく、
簡潔に書ける専用の文法が求められていました。

## <a id="sec-generated-title-4"></a> <a id="record">レコード型</a>

このボイラープレート問題を解決するために、データ中心の型向けの新しい構文として導入されたのが<strong id="key-record" class="keyword">レコード型</strong>です。
最も短い書き方をすると、例えば以下のようになります。

```csharp
record Person(string Name, DateTime Birthday);
```

クラスとの差は以下の2点です。

- `class` キーワードの代わりに `record` キーワードを使う
- 型名の後ろに直接コンストラクター引数を書ける(プライマリ コンストラクター)
  - この場合、コンストラクター引数と同名のプロパティが自動的に作られる

ちなみに、「プライマリ コンストラクター」(後述)は使わずに、以下のように書くこともできます。

```csharp
record Person
{
    public string Name { get; init; }
    public DateTime Birthday { get; init; }
}
```

どちらの書き方でも、`record` キーワードを使って定義した型に対しては、
以下のようなものが自動生成されます。

- `Equals` や `GetHashCode` や `==` などの等値判定メソッド
  - `IEquatable<T>` インターフェイスの実装も含む
- `ToString` メソッド
- 後述する `with` 式で使うクローン メソッド
- 後述する派生クラス判別のための `EqualityContract` プロパティ

要するに、前述した「お作用的に好ましい」とされるコードを一通りコンパイラーが用意してくれます。

### <a id="sec-generated-title-5"></a> <a id="record-to-class">レコード型とクラス</a>

ちなみに、コンパイラーが色々と自動生成してくれる以外は通常のクラスと同じというか、
内部的には実際にただのクラスとしてコンパイルされます。
(※ C# 9.0 の `record` の場合。C# 10.0 で導入された `record struct` は構造体としてコンパイルされます。)

コンパイラー生成のコードをそのまま書くと以下のような感じになります。
(一部、実際には手書きの C# コードでは書けないメソッド名で生成されていたりしますし、
C# コンパイラーのバージョンによって微妙に異なるコードになったりはしますが、
意味的にはほぼこのままのコードになります。)

```csharp
class Person : IEquatable<Person>
{
    protected virtual Type EqualityContract => typeof(Person);
 
    public string Name { get; init; }
    public DateTime Birthday { get; init; }

    public Person(string Name, DateTime Birthday)
    {
        this.Name = Name;
        this.Birthday = Birthday;
    }
    
    public void Deconstruct(out string Name, out DateTime Birthday)
    {
        Name = this.Name;
        Birthday = this.Birthday;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Person");
        stringBuilder.Append(" { ");
        if (PrintMembers(stringBuilder))
        {
            stringBuilder.Append(" ");
        }
        stringBuilder.Append("}");
        return stringBuilder.ToString();
    }
 
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Name");
        builder.Append(" = ");
        builder.Append((object)Name);
        builder.Append(", ");
        builder.Append("Birthday");
        builder.Append(" = ");
        builder.Append(Birthday.ToString());
        return true;
    }
 
    public static bool operator !=(Person r1, Person r2) => !(r1 == r2);
    public static bool operator ==(Person r1, Person r2) => (object)r1 == r2 || (r1 is not null && r1.Equals(r2));
 
    public override int GetHashCode()
        => (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295
        + EqualityComparer<string>.Default.GetHashCode(Name)) * -1521134295
        + EqualityComparer<DateTime>.Default.GetHashCode(Birthday);
 
    public override bool Equals(object obj) => Equals(obj as Person);
 
    public virtual bool Equals(Person other)
        => (object)other != null
        && EqualityContract == other.EqualityContract
        && EqualityComparer<string>.Default.Equals(Name, other.Name)
        && EqualityComparer<DateTime>.Default.Equals(Birthday, other.Birthday);
 
    public virtual Person Clone() => new Person(this);
 
    protected Person(Person original)
    {
        Name = original.Name;
        Birthday = original.Birthday;
    }
}
```

### <a id="sec-generated-title-6"></a> <a id="equality">等値判定</a>

前節で示した通り、レコード型を書くと自動的に `==` や `Equals` などの等値判定用のメソッド・演算子が追加されます。

ここで1点注意なんですが、レコード型から作られる `==` や `Equals` は `EqualityComparer<T>.Default` を経由して、最終的にはプロパティごとに `Equals` メソッドを呼ぶことになります。
通常、`==` と `Equals` の結果が違うなんてことは求められないので、
そんな実装になっていることはめったにはありませんが…
悪名高いものとして、[`NaN`](https://docs.microsoft.com/ja-jp/dotnet/api/system.double.nan) ([Not a Number](https://ja.wikipedia.org/wiki/NaN))は `==` と `Equals` の結果が違ったりするので、
「`double` で直接比較」と「レコード型で1段包んで比較」の結果が変わったりします。

```csharp
using System;
 
double nan = double.NaN;
 
Console.WriteLine(nan == nan); // 常に false を返す。
Console.WriteLine(nan.Equals(nan)); // こっちは true だったりする。
 
var recordNan = new Double(nan);
 
Console.WriteLine(recordNan == recordNan); // true
Console.WriteLine(recordNan.Equals(recordNan)); // true
 
record Double(double Value);
```

ちなみに、等値判定を `EqualityComparer<T>.Default` 越しにやっている都合で、レコード型のプロパティやフィールドに型引数にできない型(例えば[ポインター](../interop/sp_unsafe.md#pointer)や[ref 構造体](../resource/refstruct.md)など)は使えません。`unsafe record R(int* P);` はコンパイル エラーになります。

### <a id="sec-generated-title-7"></a> <a id="record-struct">record class と record struct</a>

<h5 class="version version10">Ver. 10</h5>

C# 9.0 (レコード型の最初のバージョン)では、レコード型は常に[参照型](../resource/oo_reference.md#reftype)(クラスと同系統の型)になります。
これに対して C# 10.0 では[値型](../resource/oo_reference.md#valtype)も選べるようにしました。
そのため、以下のように、`record class` と `record struct` というキーワードで書き分けができるようになりました。

```csharp
record class Reference(int X, int Y); // record だけ書いた場合こちらと同じ意味
record struct Value(int X, int Y);
```

ちなみに、C# 9.0 の頃からある `record` だけを使う書き方と、C# 10.0 で追加された `record class` という書き方は全く同じ意味になります。
(レコード型の思想としては、一番よく使う書き方を極力短く書けるようにしたいというものがあって、
「クラスの方がよく使うから、`record` という短い書き方は `record class` の意味で使う」という判断があるようです。)

#### <a id="sec-generated-title-8"></a> <a id="record-struct-specific"></a>record struct の特徴

`record class` と `record struct` の差は、ほぼ[クラス(参照型)と構造体(値型)の差](../resource/oo_reference.md)そのままなんですが、
ちょっとだけ細かい差があります。
(といっても、それも参照型と値型の用途の違いからくるものです。)

まず、構造体の場合は[継承](../oop/oo_inherit.md)がありません。
なので、継承に関係して必要になる [`EqualityContract` プロパティ](#inheritance)は生成されません。

また、構造体の場合、mutable (最初に作ったタイミング以外でも好きなタイミングでメンバーを書き換え可能)であっても `record class` ほど問題は起こしません。
なので、`record struct` (だけ書く)だと、mutable なプロパティが生成されます。

```csharp
// class の場合、X, Y から生成されるプロパティは
// public int X { get; init; }
// public int Y { get; init; }
record class RecordClass(int X, int Y);

// struct の場合は、
// public int X { get; set; }
// public int Y { get; set; }
record struct RecordStruct(int X, int Y);
```

構造体には [`readonly` 修飾](../resource/readonlyness.md#readonly-struct)を付けることができるので、immutable (書き換え不能)な `record struct` を作りたければ `readonly record struct` と書きます。
この場合は `record class` と同じようなプロパティが生成されます。

```csharp
// readonly struct の場合は、
// public int X { get; init; }
// public int Y { get; init; }
readonly record struct ReadOnlyRecordStruct(int X, int Y);
```

### <a id="sec-generated-title-9"></a> <a id="anonymous-type">レコード型と匿名型</a>

導入順序的な問題で、C# 的に言うとレコード型は「名前のある[匿名型](../start/sp3_inference.md#anonymous)」みたいな感じ(トゲナシトゲトゲっぽいやつ)だったりはします。
(匿名型が C# 3.0 で導入された機能なのに対して、レコード型は C# 9.0 です。)

C# は既存機能と新機能の整合性を極力取るように頑張って機能追加をしていて、
レコード型追加の際には匿名型や[タプル](tuples.md#key-tuple)との整合性を結構気にして設計しています。

例えば以下のようなコードがあったとします。
これは「とりあえず型名は付けずに匿名(タプル)でコードを書いてみた」みたいな状態です。

```csharp
using System;
 
var p = (X: 1, Y: 2); // これはタプル
var (x, y) = p;
Console.WriteLine($"{x} * {y} = {x * y}");
```

ここで、`X, Y` のペアに `Point` という名前を付けたいとして、そのためにレコード型を使ったとします。

```csharp
record Point(int X, int Y);
```

先ほどのコードには、`new Point` を足すだけで「匿名の型から名前付きの型に移行」ができます。

```csharp
using System;
 
var p = new Point(X: 1, Y: 2); // これで名前付きになった
var (x, y) = p;
Console.WriteLine($"{x} * {y} = {x * y}");
```

### <a id="sec-generated-title-10"></a> <a id="vs-normal-struct">レコード型と構造体</a>

プロパティごとの比較で等値判定したり、プロパティごとの代入でクローン(後述の [`with` 式](#with)を使う)を作ったりを取ったりする操作は、構造体であれば元々できる操作です。
ある意味、C# 9.0 で導入されたレコード型は「構造体(値型)的な扱いができるクラス(参照型)を作れるようにする」と言うものです。
(こういう操作ができることを「値セマンティクス(value semantics)を持つ」という用語で呼んだりします。)

一方で、C# 10.0 では `record struct` が追加されました。
「構造体的な扱いができるクラスの構造体版」みたいなものができてしまったわけですが…

当初、「通常の構造体にレコード型と同程度の機能性を持たせるように仕様を変えようか」という案も上がっていました。
しかし、その案だと、
既存コードへの影響が大きすぎたり、
コンパイラーによる自動生成物が増えることが好ましくない場面もあったりで、
デメリットもそれなりにあります。
なので結局、通常の構造体(`struct`)とは別に「構造体版レコード」(`record struct`)が追加されました。

大まかな違いとしては、次節のプライマリ コンストラクターを持てる点と、`IEquatable<T>` インターフェイス実装がコンパイラー生成される点です。

* プライマリ コンストラクターを持てる
  * その引数からプロパティや [`Deconstruct` メソッド](deconstruction.md#arbitrary-types)がコンパイラー生成される
* `IEquatable<T>` インターフェイス実装がコンパイラー生成される
  * 通常の構造体よりも `Equals` のパフォーマンスがいい
  * ただし、コンパイル結果のプログラム サイズがちょっと大きくなる
  * インターフェイスを実装する都合上、[ref 構造体](../resource/refstruct.md) と両立しない (`ref record struct` とは書けない)

## <a id="sec-generated-title-11"></a> <a id="primary-constructor">プライマリ コンストラクター</a>

先ほど、レコード型の最も短い書き方として以下のような例を挙げました。

```csharp
record Person(string Name, DateTime Birthday);
```

この、型名の直後に `()` でコンストラクター引数を並べる書き方を<strong id="key-primary-constructor" class="keyword">プライマリ コンストラクター</strong>と言います。

名前にコンストラクターと入っている通り、この構文からコンストラクターがコンパイラー生成されます。
それだけではなく、引数名と同名の [init-only プロパティ](../oop/oo_property.md#init-only)が生成されます。
[前述](#record-to-class)の「レコード型から生成されるクラスの例」で挙げたコードのうち、
以下のものは「プライマリ コンストラクターからの生成物」になります。

```csharp
class Person
{
    public string Name { get; init; }
    public DateTime Birthday { get; init; }

    public Person(string Name, DateTime Birthday)
    {
        this.Name = Name;
        this.Birthday = Birthday;
    }
    
    public void Deconstruct(out string Name, out DateTime Birthday)
    {
        Name = this.Name;
        Birthday = this.Birthday;
    }
}
```

残りの `Equals` や `PrintMember` などについては、プライマリ コンストラクターの有無にかかわらず、
レコード型であれば常にコンパイラー生成されます。
(public な get アクセサーを持ったすべてのプロパティが「生成元」になります。)

ちなみに、プライマリ コンストラクターの引数は、以下のように、
他のメンバーの初期化子や、メソッドの中身で参照することもできます。

```csharp
record X(int Value)
{
    public int Squared { get; } = Value * Value;
    public int Cubed { get; } = Value * Value * Value;
    public double GetSqrt() => Math.Sqrt(Value);
}
```

また「プライマリ」(primary: 一番の、最優先の)の名前の通り、
このコンストラクターは特別というか、「手書きで他のコンストラクターを書き足す場合、必ずプライマリ コンストラクターが呼ばれるようにしなければならない」という強い制約が掛かります。
例えば以下のコードはコンパイル エラーを起こします。

```csharp
record Person(string Name, DateTime Birthday)
{
    // プライマリ コンストラクターを呼んでいないのでコンパイル エラーになる。
    public Person() { }
}
```

以下のように `this` 初期化子を足せばコンパイルできるようになります。

```csharp
record Person(string Name, DateTime Birthday)
{
    // これならエラーにはならない。
    // ("" や default をとりあえず入れてしまう行為の良し悪しは置いておいて…)
    public Person() : this("", default(DateTime)) { }
}
```

ちなみに、プライマリ コンストラクターの引数に [`in`](../resource/sp_ref.md#in) と [`params`](../structured/sp_params.md) を付けることはできます(その引数からプロパティの生成もされます)が、[`ref`](../resource/sp_ref.md#sec-byref) と [`out`](../resource/sp_ref.md#out) は付けれません。

```csharp
// in と params は受け付ける。
public record Record(in int X, params int[] Y);

// ちなみに、 ref と out はダメ。
public record Record2(ref int X, out int Y);
```

### <a id="sec-generated-title-12"></a> <a id="primary-constructor-attribute">プライマリ コンストラクター引数への属性付与</a>

プライマリ コンストラクターからはプロパティがコンパイラー生成されます。
また、[プロパティからもさらにフィールドが生成](../oop/oo_property.md#auto)されています。
ということで、プライマリ コンストラクターの引数には3重の意味(引数、プロパティ、フィールド)が含まれていたりします。

必要となる場面はそれほど多くはないと思いますが、
コンパイラー生成されるプロパティやフィールドに対しても、
以下のような書き方で属性を付けることができます。

```csharp
using System;
using System.Reflection;
 
var t = typeof(X);
 
// parameter
Console.WriteLine(t.GetConstructor(new[] { typeof(int) }).GetParameters()[0].GetCustomAttribute<A>().Name);
 
// property
Console.WriteLine(t.GetProperty("Value").GetCustomAttribute<A>().Name);
 
// field
Console.WriteLine(t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)[0].GetCustomAttribute<A>().Name);
 
record X(
    [A("parameter")]
    [property: A("property")]
    [field: A("field")]
    int Value);
 
class A : Attribute
{
    public string Name { get; }
    public A(string name) => Name = name;
}
```

## <a id="sec-generated-title-13"></a> <a id="manual-override">生成物の上書き</a>

レコード型の目標の1つは「お作法として最も好ましいものを最も短い書き方で書ける」というものです。

一方で、わかった上でお作法から外れたい場合や、ちょっとしたカスタマイズをしたい場合もあります。
そこで、C# のレコード型では、コンパイラー生成されるであろう物と同名のプロパティやメソッドを手書きすると、
手書きした方のコードが優先される仕様になっています。

例えば、文字列の比較で大文字・小文字を無視したい場合、`Equals` メソッドなどを以下のように書き加えることでできます。
(例と言うことで `Equals` のみを書きますが、実際は `GetHashCode` などの書き足しも必要です。)

```csharp
record Person(string Name, DateTime Birthday)
{
    bool IEquatable<Person>.Equals(Person? other)
        => other is not null
        && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)
        && Birthday == other.Birthday;
}
```

他の例として、本来あまり好ましくはないんですが、プロパティを書き換え可能にしてしまいたい場合、
以下のようにプロパティを手書きで足してしまうことでできます。

```csharp
record Person(string Name, DateTime Birthday)
{
    public string Name { get; set; } = Name;
    public DateTime Birthday { get; set; } = Birthday;
}
```

ちなみに、この場合、「`Name` 引数を `Name` プロパティに自動代入する」みたいな処理は掛からないので注意が必要です。
上記の例で `Name { get; } = Name;` としているように、明示的な初期化が必要になります。

### <a id="sec-generated-title-14"></a> <a id="manual-override-field">フィールドでの生成物の上書き</a>

<h5 class="version version10">Ver. 10</h5>

C# 9.0 時点では、プライマリ コンストラクター引数からの生成物の上書きはプロパティでしかできませんでした。

これが C# 10.0 ではフィールドでも上書きできるようになりました。
例えば以下のコードは C# 10.0 から有効なコードになります。
(C# 9.0 時代は「(生成物の)プロパティ `X` と(手書きの)フィールド `X` が名前衝突してる」というエラーになっていました。)

```csharp
record R(int X)
{
    public int X = X;
}
```

`record struct` を導入するにあたって、
「クラスと違って、構造体では public フィールドを使うことがそれなりに需要としてある」
という背景からこの仕様が入りました。
プロパティ生成を抑止して、フィールドに変えたいときにこの機能を使います。

とはいえ、別に `record class` で使えてしまって困る機能でもないので、クラスか構造体かは問わずこの機能を使えます。

## <a id="sec-generated-title-15"></a> <a id="inheritance">レコード型の継承</a>

レコード型はクラスと同じく[継承](../oop/oo_inherit.md)ができます。
例えば以下のように書けます。

```csharp
record Base;
record A(int N) : Base;
record B(string S) : Base;
```

ちなみに、基底クラスがプライマリ コンストラクターを持っている場合、
派生クラスからそのプライマリ コンストラクターの呼び出しが必要です。
以下のように、基底クラス名の後ろに `()` を付けることで呼び出せます。

```csharp
record Base(int X);
 
// Base(int X) を呼んでいないのでエラーになる。
record Error1(int X) : Base;
 
// コンパイルできる例1: 引数を伝搬。
record Ok1(int X) : Base(X);
 
// コンパイルできる例2: 引数に何らかの既定値を与える。
record Ok2() : Base(1);
 
// コンパイルできる例3: 引数を伝搬しつつ、プロパティ Y を追加。
// (この場合、X は Base.X が優先されて、Ok3.X はコンパイラー生成されない。)
record Ok3(int X, int Y) : Base(X);
 
// Ok2 と似たようなものだけど、これはコンパイルできない。
// コンパイラーの実装都合で () が必要。
record Error2 : Base(1);
```

また、将来的なことを言うと、以下のような話もあります。

- C# 9.0 時点ではレコード型の派生はレコード型同士でしかできない
  - 将来的にはクラスからレコード型、レコード型からクラスの派生も認めるかもしれない
- `record struct` が入るとすると、派生できるのは `record class` のみ
  - (単に `record` と書くと `record class` の意味なので派生できる)

ちなみに、「[レコード型とクラス](#record-to-class)」で書いたコンパイラー生成物に `EqualityContract` というプロパティがありますが、
これは派生クラスを弁別するためにあります。

例えば以下のコードの出力結果は false です。

```csharp
using System;
 
// 同じ値を持っていても型が違うと「不一致」扱い。
Console.WriteLine(new Base(1) == new Derived(1)); // false
 
record Base(int X);
record Derived(int X) : Base(X);
```

コンパイラー生成物の `Equals` 等が `EqualityContract == other.EqualityContract` みたいなコードを含んでいるのはこのためです。
ちなみに、ちゃんと `new Base(x).Equals(new Derived(y))` と `new Derived(y).Equals(new Base(x))` の結果が一致するように作られています。
(これが成り立つようにするのは意外と手間で、レコード型からのコンパイラー生成物は結構なコード量になっていますし、手書きメンバー追加でのカスタマイズは結構大変です。)

逆に、型は無視してプロパティの一致だけで等価判定してほしい場合、以下のように `EqualityContract` を手書き追加すればできます。

```csharp
using System;
 
// EqualityContract で typeof(Base) を返すことで「一致」扱いに変わる。
Console.WriteLine(new Base(1) == new Derived(1)); // true
 
record Base(int X);
record Derived(int X) : Base(X)
{
    protected override Type EqualityContract => typeof(Base);
}
```

## <a id="sec-generated-title-16"></a> <a id="with">with 式</a>

「お作法として好ましいのは immutable (書き換え不可)」と言いましたし、
レコード型もそのお作法に則って(手書きでカスタマイズしない限り) immutable な型を生成します。

immutable なデータに対しても、「データの一部分だけを書き換えたい」という要件はよくあるんですが、
この場合「元のデータを書き換えず、クローンして一部分書き換えたデータを新規作成」という手順を踏みます。
C# 8.0 以前の書き方でいうと、以下のようなコードを書くことになります。
(が、1つ問題があって、実際には C# 8.0 で完全にこれと同じことは実現できません。)

```csharp
using System;
 
var p1 = new Person("天馬飛雄", new(2003, 4, 7));
 
var p2 = p1.Clone();
p2.Name = "鉄腕アトム";
 
record Person(string Name, DateTime Birthday);
```

このコードなんですが、(immutable というお作法上)「`Name` は書き換え不能」で作られているはずです。
その書き換え不能プロパティを書き換えないといけないので、C# 8.0 でこれと同様のコードは書けません。
これが [init-only プロパティ](../oop/oo_property.md#init-only)という機能の導入の動機で、C# コンパイラーが「クローン直後」だけを特別扱いして書き換えを認めています。

C# 9.0 ではどうしたかと言うと、`with` 式という構文を追加しました。
これが内部的に上記の「まずクローンして、クローン直後のインスタンスの一部分を書き換える」という処理に展開されます。

```csharp
var p1 = new Person("天馬飛雄", new(2003, 4, 7));
var p2 = p1 with { Name = "鉄腕アトム" };
```

とりあえず以下のように覚えてください。

- immutable なデータに対して一部分だけの書き換えをしたいときには `with` 式を使う
- 内部的にやっていることはクローンしてからの部分書き換え

ちなみに、`with { } ` だけ書く(「一部書き換え」はしない)のでもクローンになります。

```csharp
using System;
 
var p1 = new Point(1, 2);
var p2 = p1 with { }; // p1 のクローンが作られる
 
Console.WriteLine(p1 == p2); // 持ってる値的には等しいので true。
Console.WriteLine(ReferenceEquals(p1, p2)); // クローン(別インスタンス)ができてるのでこちらは false。
 
record Point(int X, int Y);
```

また、C# 9.0 時点ではクローン用のメソッドを `with` 式以外から呼ぶ手段はありません。
(`<Clone>$` みたいな通常の C# コードでは書けないような変な名前でクローン メソッドが生成されています。
逆に、この自動生成のクローンと紛らわしくならないようにするため、
レコード型に自前で `Clone` という名前のメソッドを定義することは認められていません。)
将来的には認める可能性もあるものの、現状は「クローン方法のカスタマイズ」もできません
(`Equals` 等と違って、`<Clone>$` メソッドを「手書きで上書き」はできません)。

また、このクローン用メソッドを手書きできない都合上、C# 9.0 時点では `with` 式はレコード型専用の構文になります。
(ただし、C# 10.0 で `struct` に対しては `with` 式を使えるようになる予定です。
構造体は元からクローン相当の機能を持っているので、それを使う予定です。)

### <a id="sec-generated-title-17"></a> <a id="anonymous-type">匿名型と構造体に対する with 式</a>

<h5 class="version version10">Ver. 10</h5>

C# 9.0 時点では、`with` 式はレコード型に対してしか使えません。
ただ、`with` 式に求められる要件は「クローンメソッドを持っていて、`set` もしくは `init` 持ちのプロパティがある」という点だけです。
前述の通り、「クローン方法のカスタマイズ」を提供することで任意の型に対して `with` 式を使えるようにするという提案も出ていますが、その一方で、C# 9.0 時点でも `with` 式が使える条件を満たせるけどもスケジュールの都合で C# 10.0 での実装になったものもあります。

ということで C# 10.0 では、匿名型と、任意の構造体に対して `with` 式を使えるようになりました。
以下のコードは C# 9.0 ではコンパイルできませんが、C# 10.0  ではコンパイルできるます。

[匿名型](../start/sp3_inference.md#anonymous):

```csharp
var p1 = new { X = 1, Y = 2 };
var p2 = p1 with { X = 3 };
```

任意の[構造体](../resource/rm_struct.md):

```csharp
var p1 = new Point { X = 1, Y = 2 };
var p2 = p1 with { X = 3 };
 
readonly struct Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
```
