---
title: "UnsafeAccessor"
source_url: "https://ufcpp.net/study/csharp/misc/unsafeaccessor/"
content_type: "Article"
published_at: "2026-02-15T17:23:26"
updated_at: "2026-02-15T17:23:26"
tags: []
umbraco_id: 2518
parent_id: 1338
sort_order: 11
aliases: []
---

# UnsafeAccessor

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

.NET 8 から、[リフレクション](../dynamic/sp_reflection.md)なしで[アクセシビリティ](../oop/oo_conceal.md#level)を無視した(private や internal なメンバーにアクセス可能な)仕組みとして UnsafeAccessor というものが追加されました。

用途が狭いですし、unsafe と付く名前通り割とデメリットもある機能なので、使い心地はそれほどよくありません。
「全くできないと困るので口だけは用意した」系の機能になります。
例えば、フィールド1個にアクセスするだけでも以下のような書き方になります。

```csharp
using System.Runtime.CompilerServices;

A a = new();

X.RefValue(a) = 1; // private であることを無視して a._value = 1; 相当の処理を実行。
Console.WriteLine(a); // A(1)

static class X
{
    // A._value にアクセスするためのメソッド。
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_value")]
    public static extern ref int RefValue(A @this);
}

class A
{
    private int _value;
    public override string ToString() => $"A({_value})";
}
```

`UnsafeAccessor` 属性(`System.Runtime.CompilerServices`)を付けた extern メソッドを書くと、.NET Runtime が静的に(JIT 時に) `_value` フィールド直参照と同等のコードを生成します。

## <a id="sec-generated-title-2"></a> <a id="ignore-accessibility">アクセシビリティ無視とリフレクション</a>

これまでアクセシビリティを無視したい場合、
たとえ型情報が静的に既知であってもリフレクションを使っていました。
例えば冒頭の例同様に `a._value = 1;` するためだけに、以下のようなコードが必要になっていました。

```csharp
A a = new();

// a._value = 1; 相当のコードをリフレクションでやるとこうなる。
typeof(A)
    .GetField("_value", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
    .SetValue(a, 1);

// Type 型インスタンスが作られて、
// FieldInfo 型インスタンスが作られて、
// 動的な処理で a._value = 1; 相当のコードを実行。
// しかも、int 型の 1 は object にボックス化されるコストもかかる。

Console.WriteLine(a); // A(1)

class A
{
    private int _value;
    public override string ToString() => $"A({_value})";
}
```

コード中のコメントに書きましたが、この処理には様々なオーバーヘッドがかかります。
本当に動的な処理がしたい(本当に実行時まで型に関する情報を知らない)なら必要なコストですが、
この例の `A` のように型が既知の場合だいぶもったいないです。

すなわち、リフレクションの持つ以下の2つの側面があるわけですが、

1. 実行時までわからない型のメンバーを読み書きする
2. 本来アクセスできないメンバーを読み書きする

このうち後者だけを切り出したものが UnsafeAccessor です。

UnsafeAccessor を使った冒頭のコードなら、
パフォーマンス的には `a._value = 1;` と同水準になります。

##### <a id="sec-generated-title-3"></a> <a id="unsafe">余談: 「unsafe」</a>

名前に unsafe の文字が入っていますが、
メモリ安全性や型安全性の保証はあります(この意味では普通に safe)。
バッファー オーバー ランのようなメモリ脆弱性を起こせるような機能ではないですし、
型が合わないようなコードを書くと JIT 時にエラーになります。

UnsafeAccessor の「unsafe」は「private や internal な物に触れるので変更されても文句が言えない」くらいの意味です。

また、C# コンパイラーのレベルでは型チェックできない(JIT 時チェックになるので実行してみる必要はある)という不利益もあります。


## <a id="sec-generated-title-4"></a> <a id="motivation">利用場面</a>

UnsafeAccessor の主な用途は以下のようなものです。

1. シリアライザーや [DI](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection/overview) などをリフレクションから Source Generator に移行したい
2. 元々あった大きな[アセンブリ](../package/project.md)を複数に分ける際などにやむを得ず使う
3. 単体テスト用途

元々の想定利用場面は 1. になります。
シリアライザーでは private なフィールドへの読み書きをリフレクションで行うことが多かったです。
ところがこれは、
リフレクションを避けたい実行環境
([AOT](https://learn.microsoft.com/ja-jp/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8) や、[Web Assembly](https://ja.wikipedia.org/wiki/WebAssembly) など)で困りました。
シリアライザーの場合まさに「型は静的に既知」な場合が多く、
リフレクションを使った動的コード生成から、
[Source Generator](analyzer-generator.md) を使った静的なコード生成への移行が進んでいます
(例: [System.Text.Json のソース生成利用](https://learn.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json/source-generation))。
これがまさに UnsafeAccessor で想定する場面になります。

ただ、多くの人にとって、
Source Generator は使う側にはなる一方で、作る側になることは希少です。
そのため 1. の用途では、UnsafeAccessor も「ライブラリで内部的に使われている」であって、直接の利用はあまりないでしょう。

続いて 2. ですが、
これもよほど大規模でよほど歴史のあるコードでしか見られないものです。
例えば .NET の標準ライブラリ内で、
[System.Net.Security](https://source.dot.net/#System.Net.Security/src/libraries/Common/src/System/Net/Http/X509ResourceClient.cs) から [System.Net.Http](https://source.dot.net/#System.Net.Http/System/Net/Http/GlobalHttpSettings.cs) 内の internal 型への参照が残っています。
元々大きな1つのアセンブリとして提供していたものを「Http」と「Security」で分けたときに困ったものと思われます。

結局のところ、多くの人にとっては 3. の単体テスト用途で使うことが多くなると思われます。
「private メンバー、internal メンバーの単体テストはすべきかどうか」ということ自体議論が分かれる問題ですが、まあ「private アクセスしたくなったことがある」くらいであれば多くの人が通ったことがある道だと思います。

例えば以下のようなライブラリ コードがあったとして、
「`Dispose` 後は中身が null になっていてほしい」というテストを書くみたいな用途が考えられます。

```csharp
namespace Lib;

public class Class1(object? resource) : IDisposable
{
    private object? _someResource = resource;

    public void Dispose()
    {
        _someResource = null;
    }
}
```

```csharp
using Lib;
using System.Runtime.CompilerServices;

namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void Dispose後は中身がnull()
    {
        var x = new Class1("");

        using (x)
        {
        }

        Assert.Null(GetSomeResourceRef(x));
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_someResource")]
    private static extern ref object? GetSomeResourceRef(Class1 class1);
}
```

こういう単体テストがいいのかどうかという話はありますが…
(仕様に応じて「`Dispose` 後に何らかの操作をしても何も起きない」とか「`ObjectDisposedException` が出る」とか、別の形でのテストが望ましい可能性あり。)
「やりたいことは時々ある」の範疇かと思われます。

単体テストの場合、ライブラリと単体テストでプロジェクトは分かれているものの、同じ人がコードを書くことが非常に多いです。
そのため「private なものは変更されても文句が言えない」問題が軽微(変更して単体テストが失敗するようになっても、修正する義務を同じ人が負うのですぐに直すことになるだけ)なので、UnsafeAccessor とは相性がいいです。

## <a id="sec-generated-title-5"></a> <a id="how-to-use">UnsafeAccessor の書き方</a>

すでにいくつかの例を書いていますが、UnsafeAccessor を使うには
`UnsafeAccessor` 属性(`System.Runtime.CompilerServices` 名前空間)を付けた extern メソッドを書きます。

`UnsafeAccessor` 属性の第1引数に渡す `UnsafeAccessorKind` には以下の5つの値があります
(ほぼ名前通り):

* `Constructor`: コンストラクター
* `Method`: インスタンス メソッド
* `StaticMethod`: 静的メソッド
* `Field`: インスタンス フィールド
* `StaticField`: 静的フィールド

プロパティ、インデクサーや演算子などは「.NET の型システム上はメソッドになっている」という仕様があるので、`Method` か `StaticMethod` を使ってそのメソッドを呼び出すことになります。

ちなみに、型自体は public という場面では `A Accessor()` とか `void Accessor(A x)` みたいな素直な書き方で `A` の private メンバーにアクセスできるんですが、
型自体が internal の時はさらにちょっと面倒な書き方が必要になります。
この場合は、`UnsafeAccessorType` というもう1つの属性を使って、文字列で型名を指定することになります。

### <a id="sec-generated-title-6"></a> <a id="constructor">コンストラクター</a>

コンストラクターへのアクセスは以下のように書きます。

```csharp
using System.Runtime.CompilerServices;

Console.WriteLine(X.CreateA());
Console.WriteLine(X.CreateA(1));

static class X
{
    // new A() 相当。
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    public static extern A CreateA();

    // new A(value) 相当。
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    public static extern A CreateA(int value);
}

class A
{
    private A() { }
    private A(int value) => Value = value;

    private int Value { get; }
    public override string ToString() => $"A({Value})";
}
```

「何の型のコンストラクターを呼ぶか」は戻り値の型を見ます。
メソッドの引数の型はアクセス先のコンストラクターの引数と一致させます。
(引数名は違っていても平気です。一致の必要があるのは型のみ。)

この機能は構造体に対しても使えます。

```csharp
static class X
{
    // 構造体に対しても使えて、書き方はクラスの場合と同じ。
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    public static extern A CreateA(int value);
}

struct A
{
    private A(int value) => Value = value;
    private int Value { get; }
    public override string ToString() => $"A({Value})";
}
```

### <a id="sec-generated-title-7"></a> <a id="method">インスタンス メソッド</a>

インスタンス メソッドへのアクセスは以下のように、
「先頭に1つ引数を足す」書き方をします。

```csharp
using System.Runtime.CompilerServices;

var a = new A();
X.Add(a, 1);
Console.WriteLine(a); // A(1)

var b = new B();
X.Add(ref b, 1);
Console.WriteLine(b); // B(1)

static class X
{
    // 第1引数をアクセス先の型にする。
    // (拡張メソッドと同じ感覚。)
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    public static extern void Add(A a, int value);

    // 構造体のインスタンス メソッドを呼びたい場合は ref 引数にする。
    // このメソッドの名前とアクセス先のメソッドの名前が違う場合は、Name で明示的に指定する。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "PrivateAdd")]
    public static extern void Add(ref B a, int value);
}

class A
{
    private int _value;
    private void Add(int value) => _value += value;
    public override string ToString() => $"A({_value})";
}

struct B
{
    private int _value;
    private void PrivateAdd(int value) => _value += value;
    public override readonly string ToString() => $"B({_value})";
}
```

2つ目以降の引数の型はアクセス先のメソッドの引数と一致させます。
(コンストラクター同様、型のみの一致で OK。)
また、戻り値の型が一致している必要もあります。

アクセス先の型が構造体の場合、第1引数は `ref` もしくは `in` でないとアクセスできなくなります。
(実行時エラーを起こします。)

また、`UnsafeAccessor` 属性をつけるメソッドの名前とアクセス先のメソッドの名前が異なる場合には `Name` プロパティの明示が必要になります。

### <a id="sec-generated-title-8"></a> <a id="field">フィールド</a>

インスタンス フィールドへのアクセスは以下のように、
「`ref` 戻り値、引数なしのメソッド」で書きます。

```csharp
using System.Runtime.CompilerServices;

var a = new A();
X.Value(a) = 1;
Console.WriteLine(a); // A(1)

static class X
{
    // 引数なし、ref 戻り値なメソッドでアクセス。
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_value")]
    public static extern ref int Value(A a);
}

class A
{
    private int _value;
    public override string ToString() => $"A({_value})";
}
```

フィールドとメソッドでは命名規約が違う(`_value` と `Value` になる)ので、
自然な書き方をしようとすると `Name` の明示が必要になるかと思いますが、
メソッド名が変でもいいなら以下のように `Name` の省略もできます。

```csharp
static class X
{
    // あまりメソッドに _ 始まりの名前を付けないものの、気にしないのであればこれでも OK。
    [UnsafeAccessor(UnsafeAccessorKind.Field)]
    public static extern ref int _value(A a);
}
```

### <a id="sec-generated-title-9"></a> <a id="static">静的メソッド、静的フィールド</a>

静的メソッド、静的フィールドへのアクセスには `UnsafeAccessorKind` の `StaticMethod` と `StaticField` を使います。
インスタンス メソッド、インスタン フィールドの時と同様、引数の先頭にアクセス先の型を足します(静的メンバーなのでこの第1引数は使われず、ダミー引数になります)。

```csharp
using System.Runtime.CompilerServices;

X.Value(null) = 2;

Console.WriteLine(X.M(null, 3));

static class X
{
    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "_value")]
    public static extern ref int Value(A? _);

    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod)]
    public static extern int M(A? _, int x);
}

class A
{
    private static int _value;
    private static int M(int x) => x * _value;
}
```

### <a id="sec-generated-title-10"></a> <a id="property">プロパティ</a>

`UnsafeAccessorKind` にはプロパティを指定する方法はありませんが、
C# のプロパティは内部的にはメソッドになっているので、
そのメソッドに対する UnsafeAccessor を作ることでプロパティにアクセスできます。

プロパティ `T P` があったとすると、
`get`/`set` アクセサーにはそれぞれ
`T get_P()`、 `void set_P(T value)` というメソッドが対応します。
(元のプロパティ名 + `get_`/`set_` 接頭辞。)

```csharp
using System.Runtime.CompilerServices;

var a = new A();
X.SetValue(a, 1);

Console.WriteLine(a); // A(1)
Console.WriteLine(X.GetValue(a)); // 1

static class X
{
    // Value プロパティの get アクセサーは get_Value。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Value")]
    public static extern int GetValue(A a);

    // Value プロパティの set アクセサーは set_Value。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_Value")]
    public static extern void SetValue(A a, int value);
}

class A
{
    private int Value{ get; set; }
    public override string ToString() => $"A({Value})";
}
```

ちなみにこの[「プロパティ名 + `get_`/`set_` 接頭辞」ルールは C# の言語仕様で決まっています](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#153102-member-names-reserved-for-properties)。
少なくとも C# コンパイラーで作ったプロパティの場合は必ずこのルールに基づくメソッド名になっています。
([IL アセンブラー](https://learn.microsoft.com/ja-jp/dotnet/framework/tools/ilasm-exe-il-assembler)を使えばこのルールを破れるはずですが、そんなことをやっている人は見たことがありません。)

### <a id="sec-generated-title-11"></a> <a id="indexer">インデクサー</a>

[プロパティ](#property)同様です。
[C# の仕様](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#153104-member-names-reserved-for-indexers)上、インデクサーからは `get_Item` / `set_Item` というメソッドが作られているはずなので、これを経由してアクセスします。

```csharp
using System.Runtime.CompilerServices;

var a = new A(2);
X.SetValue(a, 0, 1);
X.SetValue(a, 1, -1);

Console.WriteLine(a); // A([1, -1])
Console.WriteLine(X.GetValue(a, 0)); // 1
Console.WriteLine(X.GetValue(a, 1)); // -1

static class X
{
    // インデクサーの get アクセサーは get_Item。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Item")]
    public static extern int GetValue(A a, int i);

    // インデクサーの set アクセサーは set_Item。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_Item")]
    public static extern void SetValue(A a, int i, int value);
}

class A(int length)
{
    private readonly int[] _items = new int[length];
    private int this[int i]{ get => _items[i]; set => _items[i] = value; }
    public override string ToString() => $"A([{string.Join(", ", _items)}])";
}
```

(ちなみにこの `Item` の部分は `IndexerName` 属性を使って変更できたりします。
実際、`string` 型のインデクサーは `Chars` という名前です。
この場合、作られているメソッドの名前も `get_Chars` です。)


### <a id="sec-generated-title-12"></a> <a id="operator">演算子</a>

C# のユーザー定義の演算子は public でないといけない仕様なので、わざわざ UnsafeAccessor を使う場面はより一層少ないですが、一応触れておきます。
(後述する[型自体が internal な場合](#unsafe-accessor-type)に対して使えなくはないです。)

ユーザー定義の演算子も、内部的にはただのメソッドになります。
`+` 演算子の場合は `op_Addition` など、演算子ごとに名前が決まっています。
(参考: [演算子とメソッド名の対応関係](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#153106-method-names-reserved-for-operators))

```csharp
using System.Runtime.CompilerServices;

var a = new A(1);

X.Add(a, 1); // a += 1;

Console.WriteLine(a); // A(2)

a = X.Add(null, a, 1); // a = a + 1;

Console.WriteLine(a); // A(3)

static class X
{
    // + は op_Addition。
    // 静的メソッドなのでダミーの第1引数が必要。
    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "op_Addition")]
    public static extern A Add(A? _, A a, int x);

    // += は op_AdditionAssignment。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "op_AdditionAssignment")]
    public static extern void Add(A a, int x);
}

class A(int value)
{
    private int _value = value;

    public static A operator +(A a, int x) => new(a._value + x);

    public void operator +=(int x) => _value += x;

    public override string ToString() => $"A({_value})";
}
```


### <a id="sec-generated-title-13"></a> <a id="extension">拡張メンバーを UnsafeAccessor にする</a>

UnsafeAccessor の「静的メソッドにして第1引数を足す」という仕様が拡張メソッドと相性がよく、
拡張メソッドをそのまま UnsafeAccessor にすることができます。

```csharp
using System.Runtime.CompilerServices;

var a = new A(1);

// 拡張メソッド X.Add(A, int) を通して private な A.Add(int) を呼ぶ。
a.Add(1);

Console.WriteLine(a); // A(2)

static class X
{
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    public static extern void Add(this A a, int x);
}

class A(int value)
{
    private int Value { get; set; } = value;
    private void Add(int x) => Value += x;
    public override string ToString() => $"A({Value})";
}
```

これは C# 14 で入る `extension` ブロック形式の拡張メンバーでもできて、
以下のような書き方で UnsafeAccessor を書けたりします。

```csharp
using System.Runtime.CompilerServices;

var a = new A(1);

// 拡張メソッド X.Add を経由して、private な A.Add を呼ぶ。
a.Add(1);

Console.WriteLine(a); // A(2)

// 拡張プロパティ X.Value を経由して、private な A.Value を取得・設定する。
a.Value = 3;
Console.WriteLine(a.Value); // 3

static class X
{
    extension(A a)
    {
        // コンパイル結果は void Add(this A, int x) と一緒。
        // そのまま UnsafeAccessor にできる。
        [UnsafeAccessor(UnsafeAccessorKind.Method)]
        public extern void Add(int x);

        // 拡張プロパティも通常のプロパティと同じ get_ / set_ という名前でメソッドを作る仕様で、
        // int get_Value(A) / void set_Value(A, int) というメソッドができてる。
        // 属性も伝搬されてて、 A.Value の UnsafeAccessor にできる。
        public extern int Value
        {
            [UnsafeAccessor(UnsafeAccessorKind.Method)]
            get;

            [UnsafeAccessor(UnsafeAccessorKind.Method)]
            set;
        }
    }
}

class A(int value)
{
    private int Value { get; set; } = value;
    private void Add(int x) => Value += x;
    public override string ToString() => $"A({Value})";
}
```

### <a id="sec-generated-title-14"></a> <a id="generics">ジェネリックな型やメンバーへのアクセス</a>

.NET 9 からはジェネリックな型に対して UnsafeAccessor を書けるようになりました。
型引数は以下のように「型は型に、メソッドはメソッドに」というルールで書けば大丈夫です。

```csharp
using System.Runtime.CompilerServices;

var a = new A<int>(1);

X<int>.M(a); // 1
X<int>.M(a, "abc"); // 1 abc

// 型の型引数は型に。
// A<T> の T はここに書く。
static class X<T>
{
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    public static extern void M(A<T> a);

    // メソッドの型引数はメソッドに。
    // A<T>.M<U> の U はここに書く。
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    public static extern void M<U>(A<T> a, U x);
}

class A<T>(T value)
{
    private void M() => Console.WriteLine(value);
    private void M<U>(U x) => Console.WriteLine($"{value} {x}");
}
```

型引数の付け方は制限がかかっていて、
上記の例とは違う書き方、
例えば「クラス `X` にメソッド `M<T>` や `M<T, U>` を書く」みたいなことをすると実行時エラーになります。

### <a id="sec-generated-title-15"></a> <a id="unsafe-accessor-type">型自体が internal な場合</a>

.NET 10 から「型自体が internal で参照できない」という場合に対して UnsafeAccessor を使う手段が提供されるようになりました。

これまでの例で `A` 型の引数や戻り値を書いていた場所をとりあえず `object` 型にして、
その代わり、引数・戻り値に `UnsafeAccessorType` 属性を付けます。
`UnsafeAccessorType` 属性の引数に文字列で型名を書くことで、internal な型を参照できます。

例えばまず、`ClassLibrary1` という名前のプロジェクトに以下のようなクラスを用意したとします。

```csharp
namespace Lib;

// ClassLibrary1 というプロジェクト内にあるものとする。
internal class A
{
    private int _value;
    public override string ToString() => $"A({_value})";
}
```

この型を別プロジェクトから参照するには以下のように書きます。

```csharp
using System.Runtime.CompilerServices;

var a = X.CreateA(); // var a = new A();
X._value(a) = 1;   // a._value = 1;

Console.WriteLine(a);

static class X
{
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    [return: UnsafeAccessorType("Lib.A, ClassLibrary1")]
    public static extern object CreateA();

    [UnsafeAccessor(UnsafeAccessorKind.Field)]
    public static extern ref int _value([UnsafeAccessorType("Lib.A, ClassLibrary1")] object @this);
}
```

型名はフルネーム(`Lib.A`)で書き、
`,` でつなげて アセンブリ名(通常、プロジェクト名がそのままアセンブリ名になります。今回は `ClassLibrary1`)を書きます。

`UnsafeAccessorType` 属性は拡張メンバーでも使えます。
(`object` 型インスタンスに対する拡張になってしまって誤用が怖いという問題はあり。)

```csharp
using System.Runtime.CompilerServices;

var a = X.CreateA(); // var a = new A();

// 拡張メソッド呼び。
a._value() = 1;   // a._value = 1;

Console.WriteLine(a);

// 拡張プロパティ呼び。
a.Value = 2;   // a._value = 2;

Console.WriteLine(a);

static class X
{
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    [return: UnsafeAccessorType("Lib.A, ClassLibrary1")]
    public static extern object CreateA();

    // 拡張メソッドでも使える。
    [UnsafeAccessor(UnsafeAccessorKind.Field)]
    public static extern ref int _value([UnsafeAccessorType("Lib.A, ClassLibrary1")] this object @this);

    // extension ブロックでも使える。
    extension([UnsafeAccessorType("Lib.A, ClassLibrary1")] object @this)
    {
        public extern ref int Value
        {
            [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_value")]
            get;
        }
    }
}
```

[ジェネリックな型](#generics)の場合は `` `1 `` みたいな語尾をつける必要があります。
例として `ClassLibrary1` 側のクラス `A` を以下のようにジェネリック クラスにしてみます。

```csharp
namespace Lib;

// ClassLibrary1 というプロジェクト内にあるものとする。
internal class A<T>
{
    private T _value;
    public override string ToString() => $"A({_value})";
}
```

これを参照するためには以下のような書き方になります。

```csharp
using System.Runtime.CompilerServices;

var a = X<int>.CreateA(); // var a = new A();
X<int>._value(a) = 1;   // a._value = 1;

Console.WriteLine(a);

static class X<T>
{
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    [return: UnsafeAccessorType("Lib.A`1, ClassLibrary1")]
    public static extern object CreateA();

    [UnsafeAccessor(UnsafeAccessorKind.Field)]
    public static extern ref T _value([UnsafeAccessorType("Lib.A`1, ClassLibrary1")] object @this);
}
```

UnsafeAccessor 定義側のクラス(この例だと `X<T>`)の書き方は[前節](#generics)と同じです。
一方、 `UnsafeAccessorType` 属性に渡す型名は、
[`Type` 型](https://learn.microsoft.com/ja-jp/dotnet/api/system.type) の [`FullName` プロパティ](https://learn.microsoft.com/ja-jp/dotnet/api/system.type.fullname)で得られる文字列と同じです。
`A<T>` であれば `` Lib.A`1 `` になります。
`` ` `` はバッククオートで、日本語キーボードの場合 shift + `@` で入力するやつです。
また、`1` の部分は型引数の個数で、
これが `A<T1, T2>` の場合だと `` `2 ``、
`A<T1, T2, T3>` の場合だと `` `3 `` になります。


### <a id="sec-generated-title-16"></a> <a id="compiler-generated-field">コンパイラー生成のフィールド</a>

ここからは C# の言語仕様にはない話になります。
(現在の Roslyn と呼ばれる C# コンパイラーの実装ではそうなっているけども、
将来もずっと同じ実装が続くかとかの保証はない話です。)

[自動プロパティ](../oop/oo_property.md#auto)
([`field` キーワード](../oop/oo_property.md#field-keyword)を使ったものも含む)からはフィールドが生成されています。
現在の Roslyn の場合、このフィールドの命名ルールは「プロパティ `P` に対して `<P>k__BackingField`」みたいになります。
この挙動を使えばプロパティのバッキング フィールドを読み書きすることができます。

```csharp
using System.Runtime.CompilerServices;

var a = new A();
X.RefValue(a) = 1;

Console.WriteLine(a); // A(1)

static class X
{
    // これで Value プロパティのバッキング フィールドを参照できる。
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Value>k__BackingField")]
    public static extern ref int RefValue(A a);
}

class A
{
    public int Value { get; set; }
    public override string ToString() => $"A({Value})";
}
```

また、[プライマリ コンストラクター引数のキャプチャ](../oop/oo_construct.md#capture)で作られるフィールドは、引数 `x` に対して `<x>P` という名前になります。

```csharp
using System.Runtime.CompilerServices;

var a = new A(0);
X.RefValue(a) = 1; // 書き換え。

Console.WriteLine(a); // A(1)

static class X
{
    // プライマリ コンストラクター引数から作られるフィールドは <>P。
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<value>P")]
    public static extern ref int RefValue(A a);
}

class A(int value)
{
    public override string ToString() => $"A({value})";
}
```
