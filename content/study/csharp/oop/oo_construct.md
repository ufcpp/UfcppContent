---
title: "コンストラクター"
source_url: "https://ufcpp.net/study/csharp/oop/oo_construct/"
content_type: "Article"
published_at: "2015-05-06T14:09:22"
updated_at: "2020-07-04T00:00:00"
tags: []
umbraco_id: 1252
parent_id: 1248
sort_order: 2
aliases:
  - "/study/csharp/oo_construct.html"
---

# コンストラクター

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

オブジェクトを作成するためには、オブジェクトを正しく初期化してやる必要があります。
そのために、オブジェクトの構築のためのコンストラクターと呼ばれる特殊なメソッドが用意されています。


##### <a id="sec-generated-title-2"></a>ポイント

* コンストラクターで初期化
    * new したときに呼び出される特殊なメソッド。
    * 型名と同じ名前で定義する。
* 例えば、class Person { public Person(string name) { ... } ... }

## <a id="sec-generated-title-3"></a> <a id="ctor"></a>コンストラクター

コンストラクターはインスタンスを正しく初期化するための特別なメソッドです。
コンストラクターは以下のように、型名と同じ名前のメソッドを書くことで定義できます。

```csharp
class SampleClass
{
  // ↓これがコンストラクター
  SampleClass()
  {
    // インスタンスの初期化用のコードを書く
  }
}
```


他のメソッドと異なり、戻り値の型は書きません(コンストラクターは戻り値を返すことは出来ません)。

例えば、名簿作成のために個人情報を表す <code>Person</code> というクラスを作ったとします。
説明を簡単にするために、この名簿では名前と年齢だけを管理することにします。
そのため、<code>Person</code> は <code>name</code> と <code>age</code> という2つのメンバーのみを定義します。

```csharp
class Person
{
  public string name; // 名前
  public int age;     // 年齢
}
```


ここで、<code>Person</code>クラスのインスタンスを生成する際、
名前を <code>""</code> (空の文字列)で、年齢を <code>0</code> で初期化したいとします。
そのためには以下のようなコンストラクターを作成します。

```csharp
class Person
{
  public string name; // 名前
  public int age;     // 年齢

  // ↓これが Person クラスのコンストラクター
  public Person()
  {
    name = "";
    age  = 0;
  }
}
```


コンストラクターは <code>new</code> を用いてインスタンスを作成する際に呼び出されます。
例えば、下記のようなコードを実行した場合、

```csharp
using System;

class Test
{
  public Test()
  {
    Console.Write("Test クラスのコンストラクターが呼ばれました\n");
  }
}

class ConstructorSample
{
  static void Main()
  {
    Console.Write("Main の先頭\n");

    Test t = new Test(); // ここで Test のコンストラクターが呼ばれる

    Console.Write("Main の末尾\n");
  }
}
```


以下のような出力が得られます。

```console
Main の先頭
Sample クラスのコンストラクターが呼ばれました
Main の末尾
```


また、コンストラクターには引数を与えることもできます。
例えば、先ほどの <code>Person</code> クラスで、
インスタンスの作成時に名前と年齢の値を設定したい場合、
以下のようなコンストラクターを作成します。

```csharp
class Person
{
  public string name; // 名前
  public int age;     // 年齢

  // ↓引数つきの Person クラスのコンストラクター
  public Person(string name, int age)
  {
    this.name = name;
    this.age  = age;
  }
}
```


この例で使われている <em>
        <code>this</code>
      </em> というキーワードは、
作成するインスタンス自身を格納する特別な変数です。
そのため、この例では <code>this.name</code> は <code>Person</code> クラス内で定義された <code>name</code> のことになります。
一方、<code>this</code> の付いていない方の <code>name</code> は、コンストラクターの引数として定義した <code>name</code> のことです。

引数つきのコンストラクターを呼び出すためには、<code>new</code> を使ってインスタンスを生成する際に、以下のようにして引数を渡します。

```csharp
型名 変数名 = new 型名(引数リスト);
```

(後述しますが、[C# 9.0 からは `new` の後ろの型名を省略できることがあります](#target-typed-new)。)

例えば、先ほど定義した<code>Person</code>クラスのコンストラクターを呼び出すためには以下のようにします。

```csharp
Person p = new Person("ビスケット・クルーガー", 57);
Console.Write(p.age); // 57 と表示される
```


また、コンストラクターはオーバーロードすることができます。
例えば、<code>Person</code> クラスに、名前と年齢を引数として与えるコンストラクターと、何も引数を与えないコンストラクターの両方を定義することができます。

```csharp
class Person
{
  public string name; // 名前
  public int age;     // 年齢

  // ↓引数なしの Person クラスのコンストラクター
  public Person()
  {
    this.name = "";
    this.age  = 0;
  }

  // ↓引数つきの Person クラスのコンストラクター
  public Person(string name, int age)
  {
    this.name = name;
    this.age  = age;
  }
}
```



##### <a id="sec-generated-title-4"></a>サンプル

```csharp
using System;

/// <summary>
/// 名簿用の個人情報記録用のクラス。
/// とりあえず、名前と年齢のみ。
/// </summary>
class Person
{
  // public なフィールド
  public string name; // 氏名
  public int    age;  // 年齢

  // 定数
  const int UNKNOWN = -1;
  const string DEFAULT_NAME = "デフォルトの名無しさん";

  /// <summary>
  /// 名前と年齢を初期化
  /// 与えられた年齢が負のときは年齢不詳とみなす
  /// </summary>
  /// <param name="name">氏名</param>
  /// <param name="age">年齢</param>
  public Person(string name, int age)
  {
    this.name = name;
    this.age  = age > 0 ? age : UNKNOWN;
  }

  /// <summary>
  /// 名前のみを初期化
  /// 年齢は不詳とする
  /// </summary>
  /// <param name="name">氏名</param>
  public Person(string name) : this(name, UNKNOWN)
  {
  }

  /// <summary>
  /// デフォルトコンストラクター
  /// 氏名・年齢ともに不詳
  /// </summary>
  public Person() : this(null, UNKNOWN)
  {
  }

  /// <summary>
  /// 文字列化
  /// 氏名が不詳のときには NONAME に設定された名前を返す
  /// 年齢が不詳の時には名前のみを返す
  /// 氏名・年齢が分かっているときには「名前(xx歳)」という形の文字列を返す
  /// </summary>
  public override string ToString()
  {
    if(name == null)
      return DEFAULT_NAME;

    if(age == UNKNOWN)
      return name;

    return name + "(" + age + "歳)";
  }
}//class Person

//----------------------------------------------------
// メインプログラム
class ConstructorSample
{
  static void Main()
  {
    Person p1 = new Person("ちゆ", 12);
    Person p2 = new Person("澪");
    Person p3 = new Person();

    Console.Write("{0}\n{1}\n{2}\n", p1, p2, p3);
  }
}
```


```console
ちゆ(12歳)
澪
デフォルトの名無しさん
```


## <a id="sec-generated-title-5"></a> <a id="variable-initializer"></a>フィールド初期化子

フィールドに初期値を与えるだけなら、
コンストラクターを使わなくても、以下の様な書き方で初期化できます。

```csharp
class Person
{
    public string name = "";
    public int age = 0;
}
```

こういう書き方をフィールド初期化子（variable initializer）と言います。フィールド初期化子は、フィールドと定数に対して付けることができます。

説明は後程になりますが、[プロパティ](oo_property.md#get-only)に対しても同様の初期化を行うことができ、こちらは「プロパティ初期化子」と呼びます。
(初期化する対象の名前が違うだけで、ほぼ同じものです。)


## <a id="sec-generated-title-6"></a> <a id="initializer"></a>コンストラクター初期化子

場合によっては、あるコンストラクターから別のコンストラクターを呼びだしたいことがあります。
このような場合に、以下のような書き方で、別のコンストラクターを呼び出すことができます。

```csharp
class Person
{
    public string name;
    public int age;

    public Person()
        : this("", 0) // ↓のPerson(string, int) が呼ばれる。
    {
    }

    public Person(string name, int age)
    {
        this.name = name;
        this.age = age;
    }
}
```


この書き方をコンストラクター初期化子（constructor initializer）と言います。
（[別項](oo_inherit.md#base_ctor)で説明する`base`と区別してthis初期化子と言うこともあります。）

### <a id="sec-generated-title-7"></a> <a id="initializer-order">初期化子の呼ばれる順序</a>

ちなみに、フィールド初期化子やコンストラクターの実行順序は以下のようになります。

1. コンストラクター初期化子に渡す引数の評価
2. フィールド初期化子
    * フィールドが複数ある場合、上から順
3. 呼び先のコンストラクター
4. 呼び元のコンストラクター

```csharp
// コンストラクターを空呼び。
_ = new A();

class A
{
    // 呼び出される順序を確認するために呼ぶメソッド。
    private static int M(string message)
    {
        Console.WriteLine(message);
        return 0;
    }

    private int _member1 = M("フィールド初期化子 1");
    private int _member2 = M("フィールド初期化子 2");

    public A() : this(M("コンストラクター初期化子引数"))
    {
        M("コンストラクター()");
    }

    public A(int _)
    {
        M("コンストラクター(int)");
    }
}
```

```console
コンストラクター初期化子引数
フィールド初期化子 1
フィールド初期化子 2
コンストラクター(int)
コンストラクター()
```

この初期化の順序との兼ね合いで、フィールド初期化子ではインスタンス メソッドを呼ぶことができません。
例えば以下のようなコードを認めてしまうと、「まだ初期化していないフィールドを読んでしまう」問題が起きます。

```csharp
class C
{
    // ここで M を呼べてしまうと、未初期化の _otherField を読んでしまう。
    private int _someField = M();

    private int _otherField;
    private int M() => _otherField;
}
```

## <a id="sec-generated-title-8"></a> <a id="member_initializer"></a>オブジェクト初期化子

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 から、以下のような記法でメンバーを初期化できるようになりました。

```csharp
Point p = new Point{ X = 0, Y = 1 };
```


ちなみに、このコードの実行結果は以下のようなコードと等価です。

```csharp
Point p = new Point();
p.X = 0;
p.Y = 1;
```


詳細は「[初期化子](../functional/sp3_lambda.md#init)」で説明します。

## <a id="sec-generated-title-9"></a> <a id="dtor"></a>コンストラクターの逆操作

詳しくは後々説明していきますが、コンストラクターと逆の操作を行うものが2つあります。

1つは、ファイナライザー(destructor)です。
プログラムを書く上で、「確保したら必ず後片付けが必要なリソース」と言うものが存在します。
コンストラクターでリソースを確保したら、セットで後片付けを書く場所がファイナライザーです。

```csharp
using System.Buffers;

class Resource
{
    private byte[] _rentalArray;

    // コンストラクターで「借りてくる」
    public Resource() => _rentalArray = ArrayPool<byte>.Shared.Rent(100);

    // 借りたものは返さないといけない。そのために使うのがファイナライザー
    ~Resource() => ArrayPool<byte>.Shared.Return(_rentalArray);
}
```

詳しくは「[ファイナライザー](../resource/rm_destructor.md)」で説明します。

もう1つは、分解(deconstruct)です。
コンストラクターは複数の値を1つの複合型にまとめる操作でもあります。
この意味でのコンストラクターにあたるのが分解処理です。

```csharp
class Point
{
    public int X;
    public int Y;

    // 複数の値を組み合わせる
    public Point(int x, int y) => (X, Y) = (x, y);

    // 複数の値にばらす
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}

static class Program
{
    static void Main()
    {
        // 組み合わせる
        var p = new Point(1, 2);

        // ばらす
        var (x, y) = p;
    }
}
```

詳しくは「[複合型の分解](../datatype/deconstruction.md)」で説明します。


## <a id="sec-generated-title-10"></a> <a id="target-typed-new"></a>ターゲットからの new 型推論

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 から、状況によっては `new 型名()` の `型名` の部分を省略できるようになりました。
[ターゲット型](../start/misctyperesolution.md#target-type)からの推論が効くことが条件で、
例えば、以下のような書き方をできます。
(この機能を target-typed new と呼んだりします)。

```csharp
// new Person(17, new DateTime(1964, 9, 25)) と同じ意味
Person p = new(17, new(1964, 9, 25));
 
record Person(int Age, DateTime Birthday);
```

1つ目の `new` は左辺の `Person p` から、2つ目の `new` はコンストラクター引数の `DateTime Birthday` から型を推論できるので、自動的に `Person`、`DateTime` に型を決定します。

ローカル変数の場合には [`var`](../start/sp3_inference.md#type-inference) が使えるのでそれほど便利ではないんですが、[フィールド初期化子](#variable-initializer)やメソッドの引数などでは便利です。

```csharp
using System.Collections.Generic;
 
class Sample
{
    // フィールドに対しては var が使えない。
    // 代わりに new 型推論を使うと便利なことがある(特に、型名が長い時)。
    Dictionary<string, List<(int x, int y)>> _cache = new();
}
```

```csharp
using System.Collections.Generic;
 
static void m(Dictionary<string, string> options) { }
 
m(new()
{
    { "define", "DEBUG" },
    { "o", "true" },
    { "w", "4" },
});
```

型名の省略をできるだけの機能で、
元々 `new T(a, b, ...)` みたいに書けて、型 `T` を推論できるのであれば、`new(a, b, ...)` と書くことができます。

```csharp
using System.Globalization;
 
// new UnicodeCategory() とは元々書けるので、new() と省略可能。
UnicodeCategory c1 = new();
 
// new UnicodeCategory(1) とは元々書けないので、new(1) もダメ。
UnicodeCategory c2 = new(1);
 
// new (int x, int y)(1, 2) とは書けないんだけど、
// new ValueTuple<int, int>(1, 2) とは書けて、new(1, 2) はこの意味になる。
(int x, int y) t = new(1, 2);
 
// 配列とか dynamic は元々 new int[]() とか new dynamic() と書けないので、new() もダメ
int[] a = new();
dynamic d = new();
```

ちなみに、[null 許容型](../resource/sp2_nullable.md) に対する `new()` は、元となる型(`T?` に対する `T` 型) の方の意味になります。

```csharp
using System;
 
void m(DateTime? d) => Console.WriteLine(d);
 
m(default); // これは null の意味になる。何も表示されない。
m(new()); // これは new DateTime() の意味になる。 0001/01/01/ 0:00:00
```

また、`throw new()` は `throw new Exception()` の意味になったりします。

## <a id="sec-generated-title-11"></a> <a id="primary-constructor">プライマリ コンストラクター</a>

<h5 class="version version12">Ver. 12</h5>

C# 12 から、クラス名の直後に `()` を付けることでコンストラクターを簡素に書けるようになりました。
これを<strong id="key-primary-constructor" class="keyword">プライマリ コンストラクター</strong>(primary constructor: 主要な、第1のコンストラクター)と言います。


例えば、これまで以下のように書いていたコードがあったとします。

```csharp
class Person
{
    public string Name;
    public int Age;

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
```

これをプライマリ コンストラクターを使って書きなおすと以下のようになります。

```csharp
class Person(string name, int age)
{
    public string Name = name;
    public int Age = age;
}
```

プライマリ コンストラクターは、
名前にプライマリ(主要、第1)と付く程度には特別な地位にあります。
構文的に1つしか持てないのはもちろんのこと、
他のコンストラクターから必ず呼び出す必要があります。

例えば以下のコードはコンパイル エラーになりますが、

```csharp
class Person(string name, int age)
{
    public string Name = name;
    public int Age = age;

    // プライマリ コンストラクター以外にもコンストラクターを書けるものの、
    // : this(...) でプライマリ コンストラクターを呼び出す必要がある。
    public Person() { } // このコードでは呼んでいないのでコンパイル エラーを起こす。
}
```

以下のようなコードなら大丈夫です。

```csharp
class Person(string name, int age)
{
    public string Name = name;
    public int Age = age;

    const int UNKNOWN = -1;
    const string DEFAULT_NAME = "デフォルトの名無しさん";

    public Person() : this(DEFAULT_NAME, UNKNOWN) { }
    public Person(string name) : this(name, UNKNOWN) { }
}
```

### <a id="sec-generated-title-12"></a> <a id="vs-record">補足: レコード型との差</a>

C# 9 で[レコード型](../datatype/record.md)が導入された際、
普通のクラスや構造体よりも先にレコード型に対してだけプライマリ コンストラクターが書けました。
順序的に紛らわしくなっていますが、
プロパティの自動生成をしてくれるのはレコード型だけです。

例えば以下のような(通常の)クラスとレコードがあったとして、

```csharp
class Class(int X, int Y);

record Record(int X, int Y);
```

これらの型は以下のような感じに展開されます。

```csharp
class Class
{
    // 空っぽのコンストラクターができるだけ(引数未使用)。
    public Class(int X, int Y) { }
}

class Record
{
    // レコード型の場合はコンパイラーがいろいろと生成する。
    public int X { get; init; }
    public int Y { get; init; }

    public Record(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    // その他、Equals などもコンパイラーが生成。
}
```

### <a id="sec-generated-title-13"></a> <a id="empty-body">括弧省略</a>

レコード型では、以下のように `{}` を省略可能でした。
(ただし、その場合、`;` を付ける必要があります。)

```csharp
// プライマリ コンストラクターだけ持つレコード。
// 「X 以外にメンバーは不要」みたいなことは多々あり、{} 省略にはそれなりの需要あり。
record R1(int X);

// プライマリ コンストラクターは引数なしでも OK。
// なんならプライマリ コンストラクターすらなくても {} 省略可能。
// あんまり使わないとしても、わざわざ禁止する理由もないので。
record R2();
record R3;
```

C# 12 で、普通のクラスに対してもプライマリ コンストラクターを書けるようにするにあたって、
この `{}` を省略できる仕様も引き継がれました。
そして、コンストラクターを必要とするクラスと構造体だけではなく、
インターフェイスと列挙型に対しても同様に `{}` 省略を認めることになりました。
(`{}` が `;` に変わるだけなのでたかだか1文字差ですが。)

```csharp
// クラス、構造体、インターフェイス、列挙型で {} 省略が可能に。
class C;
struct S;
interface I;
enum E;
```

レコード型と比べると用途は少ないですが、例えば、
コード生成前提で「手書きでは何も書くものがない」というような場合に使えなくもないです。
実際例えば、[`JsonSerializable` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.text.json.serialization.jsonserializableattribute)を使うときにそういうコードになったりします。

```csharp
using System.Text.Json.Serialization;

// JsonSerializable 属性を付けていると、シリアライズ処理に必要なメンバーをコード生成する。
// 手書きでは何もする必要がないので空っぽ。
[JsonSerializable(typeof(Person))]
partial class MyJsonContext : JsonSerializerContext;

record Person(string FirstName, string LastName);
```

### <a id="sec-generated-title-14"></a> <a id="pc-paramter">プライマリ コンストラクター引数</a>

プライマリ コンストラクターの引数は、クラス内の全域で参照できます。

```csharp
class C(int x)
{
    public int Fiedl = x; // フィールド初期化子で使う。
    public int Property { get; } = x; // プロパティ初期化子で使う。

    // どこでも、何度でも使える。
    public int X2 = x * x;
    public int X3 = x * x * x;
}
```

なんなら [`partial`](oo_class.md#partial)で複数のファイルに分割されていても参照できます。

```csharp
// C1.cs
partial class C(int x)
{
}

// C2.cs
partial class C
{
    public int X = x; // OK
}
```

プライマリ コンストラクターの引数を初期化にだけ使っている分には、
通常のコンストラクター引数とほぼ同じです。

例えば、以下のコードの `C1` と `C2` には差がありません。
(クラスの継承が絡まない限りは同じで、[継承があった場合でも初期化の実行順](misc_construct.md#primary-constructor)にちょっと影響があるだけです。)

```csharp
class C1(int x)
{
    private readonly int _x = x;
}

class C2
{
    private readonly int _x;

    public C2(int x)
    {
        _x = x;
    }
}
```

#### <a id="sec-generated-title-15"></a> <a id="capture">キャプチャ</a>

プライマリ コンストラクター引数を初期化時以外でも使う場合には少し事情が変わってきます。

例えば以下のように、メソッドや[プロパティ](oo_property.md)の中で参照した場合、
コンパイラーがフィールドを生成します。

```csharp
class C(int x)
{
    // = (代入)じゃなくて => (式形式のプロパティ)。
    public int Count => x;

    // 他に、メソッドの中でも参照。
    public void Increment() => ++x;
}
```

こういう操作をキャプチャ(capture: 捕獲)と言います。
「`Count` プロパティや `Increment` メソッドに引数 `x` が捕まる」という意味です。

この例の場合、以下のようなコードと同じ意味になります。

```csharp
class C
{
    // コンパイラー生成のフィールドは実際には <x>P みたいな、通常の C# では書けない名前になる。
    // かつ、この名前はコンパイラーのバージョンによって変わる可能性あり。
    private int _x;

    public C(int x) => _x = x;

    public int Count => _x;

    public void Increment() => ++_x;
}
```

#### <a id="sec-generated-title-16"></a> <a id="double-field">注意: 2重フィールド生成</a>

ちょっと注意が必要なのは、以下のようなコードを書いてしまうと(おそらく意図せず)フィールドが2重に生成されることがあるという点です。

```csharp
class C(int x)
{
    // こちらは「キャプチャ」。
    public int X1 => x;

    // こちらは自動プロパティの初期化。
    public int X2 { get; } = x;
}
```

(ちゃんと警告が出るようになっています。`X2` の行の `= x` のところに警告が出ます。)

このコードはおおむね以下のような意味になります。

```csharp
class C
{
    // キャプチャに対応するため、「引数 x に対応するフィールド」を生成。
    private int _x;

    // 自動プロパティに対応するため、「プロパティ X2 に対応するフィールド」を生成。
    private int _x2;

    public C(int x)
    {
        _x = x;
        _x2 = x;
    }

    // 「キャプチャ」だったもの。
    public int X1 => _x;

    // 「自動プロパティ」だったもの。
    public int X2 { get => _x2; set => _x2 = value; }
}
```

こんな風にフィールドが2個できることは望ましくないので、ちゃんと警告は取りましょう。

#### <a id="sec-generated-title-17"></a> <a id="mutable">注意: 書き換え可能</a>

プライマリ コンストラクターの引数は、
あくまで引数です。
キャプチャが発生すると実質的にはフィールドみたいなものですが、
それでも扱いとしては引数です。

現状、C# には引数を[readonly](../start/sp_const.md#readonly)にする手段がないので、
プライマリ コンストラクター引数は常に書き換え可能です。

```csharp
partial class C(int x)
{
    public int X => ++x; // x を書き換え放題。
}

// 別ファイル
partial class C
{
    public void M() => x = 0; // 何だったらだいぶ遠い場所で書き換え可能。
}
```

これが嫌なら、一度 readonly フィールドで受けましょう。

```csharp
partial class C(int x)
{
    // フィールドで受け取る。
    private readonly int _x = x;
}

// 別ファイル
partial class C
{
    public void M1() => x = 0; // これは「2重フィールド警告」が出る(警告を取れば問題を避けれる)。

    public void M2() => _x = 0; // これは「readonly フィールドを書き換えちゃダメ」エラーになる。
}
```
## <a id="exercise"></a>演習問題

### <a id="exercise-str1"></a>問題 1


前節[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスに、
以下のようなコンストラクターを追加せよ。

```csharp
/// <summary>
/// 座標値 (x, y) を与えて初期化。
/// </summary>
/// <param name="x">x 座標値</param>
/// <param name="y">y 座標値</param>
public Point(double x, double y)
```


```csharp
/// <summary>
/// 3つの頂点の座標を与えて初期化。
/// </summary>
/// <param name="a">頂点A</param>
/// <param name="b">頂点B</param>
/// <param name="c">頂点C</param>
public Triangle(Point a, Point b, Point c)
```



#### 解答例 1


```csharp
using System;

/// <summary>
/// 2次元の点をあらわす構造体
/// </summary>
struct Point
{
  public double x; // x 座標
  public double y; // y 座標

  /// <summary>
  /// 座標値 (x, y) を与えて初期化。
  /// </summary>
  /// <param name="x">x 座標値</param>
  /// <param name="y">y 座標値</param>
  public Point(double x, double y)
  {
    this.x = x;
    this.y = y;
  }

  public override string ToString()
  {
    return "(" + x + ", " + y + ")";
  }
}

/// <summary>
/// 2次元空間上の三角形をあらわす構造体
/// </summary>
class Triangle
{
  public Point a;
  public Point b;
  public Point c;

  /// <summary>
  /// 3つの頂点の座標を与えて初期化。
  /// </summary>
  /// <param name="a">頂点A</param>
  /// <param name="b">頂点B</param>
  /// <param name="c">頂点C</param>
  public Triangle(Point a, Point b, Point c)
  {
    this.a = a;
    this.b = b;
    this.c = c;
  }

  /// <summary>
  /// 三角形の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public double GetArea()
  {
    double abx, aby, acx, acy;
    abx = b.x - a.x;
    aby = b.y - a.y;
    acx = c.x - a.x;
    acy = c.y - a.y;
    return 0.5 * Math.Abs(abx * acy - acx * aby);
  }
}

/// <summary>
/// Class1 の概要の説明です。
/// </summary>
class Class1
{
  static void Main()
  {
    Triangle t = new Triangle(
      new Point(0, 0),
      new Point(3, 4),
      new Point(4, 3));

    Console.Write("{0}\n", t.GetArea());
  }
}
```
