---
title: "構造体"
source_url: "https://ufcpp.net/study/csharp/resource/rm_struct/"
content_type: "Article"
published_at: "2015-07-15T00:00:00"
updated_at: "2017-11-04T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 6.0"
umbraco_id: 1773
parent_id: 1286
sort_order: 1
aliases: []
---

# 構造体

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[データの構造化](../structured/st_struct.md)」で少し触れて以来、ずっとクラスだけを使って説明してきましたが、ここで、C#における「もう1つの複合型」である構造体について説明します。

根本的な差は、次項で説明する「[値型](oo_reference.md#valtype)」か「[参照型](oo_reference.md#reftype)」かの違いに起因します。構造体は値型で、そのためにクラスと比べていくつか用途に制限がかかります。

## <a id="sec-generated-title-2"></a> <a id="restriction"></a>構造体の制限

とりあえず、クラスと構造体を並べて書いてみましょう。

#### <a id="sec-generated-title-3"></a>構造体

```csharp
public struct SampleStruct : InterfaceA, InterfaceB
{
    public int A { get; }
    public int B { get; }

    public SampleStruct(int a, int b) { A = a; B = b; }

    public static SampleStruct operator-(SampleStruct x)
        => new SampleStruct(-x.A, -x.B);
}

public interface InterfaceA { int A { get; } }
public interface InterfaceB { int B { get; } }
```

#### <a id="sec-generated-title-4"></a>クラス

```csharp
public sealed class SampleClass : BaseClass, InterfaceA, InterfaceB
{
    public int A { get; set; }
    public int B { get; set; }

    public SampleClass() { }
    public SampleClass(int a, int b) { A = a; B = b; }

    public static SampleClass operator -(SampleClass x)
        => new SampleClass(-x.A, -x.B);

    public override void X() { }

    ~SampleClass() { }
}

public class BaseClass
{
    public virtual void X() { }
}

public static class StaticClass
{
    public static string Hex(int x) => x.ToString("X");
}
```

単純に、クラスの方ができることは多いです。

クラスにしかできないことは以下の通り。

- 他のクラスから派生(他のクラスを継承)する
  - 継承に関連する修飾子(`abstract`, `sealed`, `virtual`, `override`など)を使えるもクラスだけ
- 静的クラス
- 引数なしのコンストラクターの定義(C# 9.0 まで)
- ファイナライザーの定義

一方、クラス・構造体どちらでもできることは以下のとおりです。

- 引数なしコンストラクターとファイナライザー以外のメンバー定義
- インターフェイスの実装(複数も可)
- (構造体自身には`static`修飾子を付けれないものの)静的メンバーの定義自体は可能

## <a id="sec-generated-title-5"></a> <a id="usage"></a>構造体の用途

「できること」でいうと、構造体はクラスの完全下位互換で、メリットがないように見えます。構造体の利点を理解するには、次項の[値型と参照型](oo_reference.md)についての知識が必要になります。

おおまかに言うと、

- クラスと構造体ではメモリの使い方が違う
- 小さなデータ構造に対しては構造体が有利
  - 使い方にもよりますが、大まかなガイドラインとしては16バイト程度が境界と言われています

というものです。

この性質と、前節で説明した制限とを併せて考えると、構造体の利用を検討するのは、

- データ構造が16バイト未満
- 継承が必要ない

という状況下になります。

## <a id="sec-generated-title-6"></a> <a id="default"></a>構造体の既定値

これも[値型](oo_reference.md#valtype)の性質になりますが、
クラス(`new`するまでメモリ領域を確保しない)と違って、
構造体は宣言した時点でデータを記録するためのメモリ領域が確保されます。

クラス型のフィールドの場合は、`new`するなり他のインスタンスを代入するなりして初期化するまでの間、
`null` (何のインスタンスも指していない状態)が入ります。

一方、構造体の場合、いわゆる「0初期化」状態になっています。
全てのメンバーに対して、0、もしくはそれに類する以下のような値が入ります。

- 数値型(`int`, `double`など)の場合は0
  - 列挙型も、0 に相当する値
- `bool` 型の場合は `false`
- 参照型(`string`、配列、クラス、デリゲートなど)や[Null許容型](sp2_nullable.md#nullableType)の場合は `null`

これら、0初期化状態にある値を、<em>構造体の既定値</em>(default value)と呼びます。

```csharp
using System;

struct Sample
{
    public int I;
    public double D;
    public bool B;
    public string S;
}

public class Program
{
    static Sample s;

    static void Main(string[] args)
    {
        Console.WriteLine(s.I);
        Console.WriteLine(s.D);
        Console.WriteLine(s.B);
        Console.WriteLine(s.S);
    }
}
```

```console
0
0
False
```

## <a id="sec-generated-title-7"></a> <a id="parameterless-ctor"></a>引数なしコンストラクター

C# 9.0 まで、構造体のメンバーとして引数なしのコンストラクターを書くことはできませんでした。
これは、`new T()`を[既定値](rm_default.md#default-keyword)(0初期化)として使っていたせいです。

例えば以下のコードでは、`Point`クラスには引数なしのコンストラクターを定義していませんが、
`new Point()`という書き方で 0 初期化を行っています。

```csharp
using System;

struct Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
    public override string ToString() => $"({X}, {Y})";
}

public class Program
{
    static void Main(string[] args)
    {
        var p1 = new Point(); // 既定値、つまり、「XもYも0に初期化」という意味で使われる
        var p2 = new Point(10, 20);
        var p3 = default(Point); // C# 2.0～9.0 まで、p1と同じ意味

        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
    }
}
```

```console
(0, 0)
(10, 20)
(0, 0)
```

<h5 class="version version2">Ver. 2.0</h5>

ちなみに、C# 2.0 以降では、構造体の既定値は、`new T()`という書き方の他に、`default(T)`という書き方もできます。
(主に[ジェネリック](../oop/sp2_generics.md)のために導入された構文です。)

既定値について、詳しくは別項「[既定値](rm_default.md#default-constructor)」で説明します。

<h5 class="version version10">Ver. 10</h5>

C# 2.0 で `default(T)` を使った既定値(0初期化)ができるようになって、
「`new T()` と書く場合は引数なしコンストラクターを呼ぶ」という仕様に変えたい
(構造体にも引数なしコンストラクターを書けるようにして、`new T()` と `default(T)` を区別する)
という話は前々からありました。

C# 10.0 で、ついにその案が採用されることになり、
引数なしコンストラクターを書けるようになりました。
例えば以下のようなコードが書けるようになります。

```csharp {title="構造体の引数なしコンストラクターの例"}
struct A
{
    public int X;
    public A() => X = 1;
}
```

これで、`new A()` で `X` が1になります。

### <a id="sec-generated-title-8"></a> <a id="new-or-default"></a>new() と default

背景説明の通り、`new()` と `default` の意味が変わったので注意が必要です。
この例の構造体 `A` の場合、以下のような挙動になります。

```csharp {title="new A() と default(A)"}
Console.WriteLine(new A().X); // コンストラクターが呼ばれて、X == 1 になってる。
Console.WriteLine(default(A).X); // コンストラクターも呼ばれず 0 初期化で、X == 0 になってる。
```

C# 7.1/9.0 で、`new()` や `default` に[ターゲット型からの推論](../start/misctyperesolution.md#target-type)が働くようになったので、以下のようにも書けます。

```csharp {title="new() と default"}
A a = new();
Console.WriteLine(a.X); // 1

a = default;
Console.WriteLine(a.X); // 0
```

`default` を書く以外に、配列の要素も既定値(0初期化)になるので注意が必要です。

```csharp {title=" 配列の要素は暗黙的に default"}
// 配列の要素は暗黙的に default…
Console.WriteLine((new A[1])[0].X); // default(A) と同じ扱いで、X == 0 になってる。
```

ちなみに、ジェネリクス越しでも `new()` と `default` の呼び分けが掛かります。

```csharp {title="ジェネリクス越しの new() と default"}
Console.WriteLine(New<A>().X); // 1
Console.WriteLine(Default<A>().X); // 0

static T New<T>() where T : new() => new();
static T? Default<T>() => default;
```

また、これまで `default` と同じ意味だった `new()` が、引数なしコンストラクターの有無で違う意味になるのでこの点にも注意が必要です。
例えば、一般の構造体で[オプション引数](../structured/st_function.md#default-parameter)を使いたい場合、
既定値しか使えません。
引数なしコンストラクターがない場合には `new()` も既定値扱いですが、
ある場合には `new()` を渡せなくなります。

```csharp {title="引数なしコンストラクターの有無で new() の意味が変わる例" error-ranges="sha256:4745e94e5aef9c5830d88316092e886dd59da434b8b15644fda591073966c4c0;4:15-4:20"}
void m(
    NoCtor n1 = new(),
    NoCtor n2 = default,
    Ctor c1 = new(), // この行だけコンパイル エラー
    Ctor c2 = default
    )
{ }

struct NoCtor { }
struct Ctor { public Ctor() { } }
```

### <a id="sec-generated-title-9"></a> <a id="field-initialize"></a>フィールド初期化子

C# 10.0 で構造体に引数なしコンストラクターが使えるようになったことに伴って、
フィールド初期化子も使えるようになりました。
以下のようなコードは C# 10.0 から書けるようになります。

```csharp {title="構造体のフィールド初期化子の例"}
struct FieldInitializer
{
    public int X = 1;
    public int Y = 2;

    public FieldInitializer() { }
}
```

`new()` だけで、`X`、`Y` の値がそれぞれ1、2に初期化されます。

```csharp {title="引数なしコンストラクターでフィールド初期化子が呼ばれる例"}
var f = new FieldInitializer();
Console.WriteLine(f.X); // 1
Console.WriteLine(f.Y); // 2
```

(※ 初期案では、明示的なコンストラクター定義もなしでフィールド初期化子を書けるようにする予定でした。
この際、フィールド初期化子を書くとコンパイラーが引数なしコンストラクターを生成していました。
C# 10 リリース当初はその案に基づいた実装になっていましたが、
ちょっと問題があって撤回され、明示的にコンストラクターを書かなければならなくなりました。)

### <a id="sec-generated-title-10"></a> <a id="accessibility"></a>引数なしコンストラクターのアクセシビリティ

`new()` が `default` と同じ意味になるのか、
それとも引数なしコンストラクターの呼び出しになるのか紛らわしくなるので、
構造体の引数なしコンストラクターは public 以外を認めていません。

```csharp {title="private、internal な引数なしコンストラクターはエラーになる" error-ranges="sha256:cb370ea3a6c337eb01e7cca59fc4ceac5286995025a630a786a91824fcf0d82b;4:13-4:14,10:14-10:15"}
struct A
{
    public int X;
    private A() => X = 0; // エラー
}

struct B
{
    public int X;
    internal B() => X = 0; // エラー
}
```

## <a id="sec-generated-title-11"></a> <a id="definite-assignment"></a>確実な初期化

※ C# 10 までの仕様になります。

`new T()` や`default(T)`で作る「既定値」とは違って、
引数付きのコンストラクターを使う場合は、コンストラクター内で全てのメンバーをきっちり自分の手で初期化する必要がありました。

例えば、以下のコードは、コンストラクター内で `_z` の初期化を忘れているのでコンパイル エラーになっていました。

```csharp {title="_z の初期化忘れ"}
struct Sample
{
    int _x;
    int _y;
    int _z;

    public Sample(int x, int y)
    {
        _x = x;
        _y = y;
        // C# 10 以前はコンパイル エラー
    }
}
```

(クラスの場合はこういう制限はなく、明示的に初期化しなかったフィールドは既定値(0)で初期化されます。)

また、全てのフィールドを初期化するまで、プロパティやメソッドなどの関数メンバーを呼べないという制約もありました。

```csharp
struct Sample
{
    int _x;
    int _y;

    public Sample(int x, int y)
    {
        M(); // エラー: _x, _y の初期化より前に呼んじゃダメ。
        _x = x;
        _y = y;
        M(); // この順ならOK。
    }

    void M() { }
}
```

(同じくクラスの場合は制限はなし。既定値(0)が使われるだけ。)

### <a id="sec-generated-title-12"></a> <a id="auto-default">構造体のフィールドの既定値初期化</a>

<h5 class="version version11">Ver. 11.0</h5>

C# 11 では、構造体でもフィールドの明示的な初期化が不要になりました。
(クラスと構造体の差が1つなくなりました。)

前節のコードとほぼ同じですが、 C# 11 にすれば以下のようなコードがコンパイルできるようになります。

```csharp {title="構造体のフィールドが自動的に 0 初期化されるように"}
struct Sample
{
    int _x;
    int _y;
    int _z;

    public Sample(int x, int y)
    {
        M(); // C# 11 では初期化よりも先に読んでも平気。_x, _y にもこの時点でいったん 0 が入ってる。

        _x = x;
        _y = y;
        // C# 11 では _z に 0 が自動で入る。
    }

    void M() => Console.WriteLine($"{_x}, {_y}, {_z}");
}
```


### <a id="sec-generated-title-13"></a> <a id="auto-property"></a>自動プロパティの扱い変更

<h5 class="version version6">Ver. 6</h5>

前節の「確実な初期化」と絡んで、C# 5.0までのC#では、自動プロパティの初期化が非常に面倒でした。

C# 5.0 以前の場合、以下のコードはコンパイル エラーを起こします。

```csharp {title="C# 5.0まではエラーになるコード"}
public struct Point
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Point(int x, int y)
    {
        // C# 5.0 まではエラーになる
        X = x;
        Y = y;
    }
}
```

エラーを起こす原因は、以下の組み合わせのせいです。

- 自動プロパティを定義すると、コンパイラーが対応するフィールド(バック フィールド)を作る
- 構造体の制約のせいで、バック フィールドが初期化されるまで、プロパティの読み書きできない
- でも、自動プロパティの場合、プロパティを介さずにバック フィールドを初期化する方法がない

このせいで、構造体と自動プロパティは相性が悪く、以下のように、自動プロパティを使わない書き方に書き換える必要がありました。

```csharp {title="C# 5.0までで正しくコンパイルできるようにするには"}
public struct Point
{
    private int _x;
    public int X { get { return _x; } }

    private int _y;
    public int Y { get { return _y; } }

    public Point(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
```

これに対して、C# 6では、最初のコードがコンパイルできるようになっています。
C#の仕様書に以下の1文が追加されたことによります。

- 自動プロパティを型の中から使う場合、そのバック フィールドに対する読み書きに置き換える

この仕様が追加されたことで、先ほどのコードはバック フィールドの初期化と見なされ、構造体の制約に引っかからなくなりました。

ちなみに、C# 6の場合は get のみの自動プロパティ(get-only auto-property)という構文が追加されて、先ほどのコードはさらに、以下のように書けるようになりました。

```csharp {title="C# 6のget-only自動プロパティ"}
public struct Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

## <a id="sec-generated-title-14"></a> <a id="memberwise"></a>メンバー毎コピー、メンバー毎比較

構造体の変数への代入は、全メンバーのコピーになります。
また、構造体には自動的に`Equals`メソッドが実装されて、メンバー毎の比較(全メンバー一致の場合に一致)になります。

```csharp
using System;

public class Program
{
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) { X = x; Y = y; }
        public override string ToString() => $"({X}, {Y})";
    }

    static void Main(string[] args)
    {
        var x = new Point(1, 2);
        var y = x;

        Console.WriteLine(y); // x のメンバー毎コピー = (1, 2)

        // メンバー毎比較(全メンバー一致なら一致)
        Console.WriteLine(x.Equals(new Point(1, 2))); // true
        Console.WriteLine(x.Equals(new Point(1, 3))); // false
    }
}
```

## <a id="sec-generated-title-15"></a> <a id="struct-modifier"></a>構造体に対する特別な修飾子

ここでは紹介だけになりますが、構造体にだけ付けることができる特別な修飾子があります。

- [readonly](readonlyness.md#readonly-struct)
- [ref](refstruct.md)

詳細についてはそれぞれリンク先を参照してください。

ちなみに、現状では、`ref` には語順に制約があって、
`struct`もしくは`partial`の直前に来る必要があります(緩和も検討されています)。
要するに、`readonly ref struct`はOKですが、`ref readonly struct`はエラーになります。

いくつか実例を挙げます。

```csharp {title="ref の語順の例" error-ranges="sha256:79c7a395c6a4bac5e2bd5c64e4bce17724f56b315129eea7a0fc6c421edc810b;7:14-7:20,8:14-8:20,9:17-9:24,9:25-9:28,10:21-10:28,11:12-11:19,11:20-11:28"}
// OK
readonly public ref struct Ok1 { }
readonly public ref partial struct Ok2 { }
public readonly ref partial struct Ok3 { }

// コンパイル エラー
ref readonly struct Ng1 { }
readonly ref public struct Ng2 { }
readonly public partial ref struct Ng3 { }
public ref readonly partial struct Ng4 { }
public ref partial readonly struct Ng5 { }
```

おそらく、以下のような型の入れ子とメソッド定義の区別を楽にするための制限(あくまでコンパイラー都合)と思われます。

```csharp {title="ref の語順に制限がある理由" error-text="struct"}
class Sample
{
    // 以下のエラー行、エラー内容は「readonly の後ろには型名が必要」になる
    ref readonly struct InnerStruct { }
    ref readonly int Method(in int x) => ref x;
}
```
