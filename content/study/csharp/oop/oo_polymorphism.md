---
title: "多態性"
source_url: "https://ufcpp.net/study/csharp/oop/oo_polymorphism/"
content_type: "Article"
published_at: "2015-05-06T14:09:43"
updated_at: "2007-10-06T00:00:00"
tags: []
umbraco_id: 1263
parent_id: 1248
sort_order: 12
aliases:
  - "/study/csharp/oo_polymorphism.html"
---

# 多態性

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

多態性(polymorphism: ポリモーフィズム)とは、
同じメソッド呼び出し(オブジェクト指向用語的には「メッセージ」という)に対して異なるオブジェクトが異なる動作をすることを言います。

（
ちなみに、polymorphism は多相性とか多様性と訳す場合もあります。
「poly（多）＋morphism（射：形を変えるみたいな意味） → いろいろな姿を映し出す」という意味。
）

オブジェクト指向プログラミング言語には、
多態性を実現するために、仮想メソッドというものが用意されています。


##### <a id="sec-generated-title-2"></a>ポイント

* オブジェクト指向の中核概念その3: 多態性。

* 同じ名前のメソッドを呼び出しで、異なる振る舞いをすること。

* 特に重要なのは、仮想関数を使った動的多態性。インスタンスの動的な型に応じて異なる振る舞いをする。

* （メソッドのオーバーロードも多態性の一種（静的多態性）。）



## <a id="sec-generated-title-3"></a> <a id="type"></a>静的な型、動的な型

「[継承](oo_inherit.md)」で説明したとおり、
派生クラスのインスタンスは基底クラスの変数に格納することが出来ます。
このとき、変数の型を<strong id="statictype" class="keyword">静的な型</strong>といい、
実際に格納されているインスタンスの型を<strong id="dynamictype" class="keyword">動的な型</strong>といいます。

```csharp {title="派生クラスのインスタンスを基底クラスの変数に格納"}
class Base{}
class Derived : Base{}

class DynamicTypeTest
{
  static void Main()
  {
    // 変数の型
    // ｜         実際に格納するインスタンスの型
    // ｜         ｜
    // ↓         ↓              静的な型, 動的な型
    Base    a = new Base();    // Base    , Base
    Base    b = new Derived(); // Base    , Derived
    Derived c = new Derived(); // Derived , Derived
  }
}
```


ここでいう“静的”とはコンパイル時に型が確定するという意味です。
変数（new で生成されるインスタンスではなく、単なる入れ物）の型は、
宣言時に決まっていますので、静的な型になります。
つまり、実行時に型が変わるということはありません。

静的な型の情報は以下のように <strong id="typeof" class="keyword">typeof 演算子</strong>を用いて取得することが出来ます。
typeof 演算子は <code>System.Type</code> というクラスのインスタンスを返します。

```csharp {title="静的型情報 typeof"}
typeof(クラス名)
```


逆に、“動的”とはコンパイル時には型が確定せず、
実行時に変化する可能性のあるもののことを指します。
（なので、動的な型のことを実行時型（run-time type）とも言います。）
（単なる入れ物である）変数とは異なり、
（実行時に new で生成される）インスタンスの型は実行時に決まります。

動的な型の情報は以下のように <code>GetType</code> メソッドを用いて取得します。

```csharp {title="動的型情報 GetType"}
変数名.GetType()
```



##### <a id="sec-generated-title-4"></a>サンプル

```csharp {title="動的型情報のサンプル"}
using System;

class Base{}
class Derived : Base{}

class DynamicTypeTest
{
  static void Main()
  {
    ShowDynamicType(new Base());
    ShowDynamicType(new Derived());
  }

  // Base 型の変数 b に格納されているインスタンスの動的な型の名前を表示する。
  static void ShowDynamicType(Base b)
  {
    Type t = b.GetType();
    Console.Write(t.Name + "\n");
  }
}
```


```console
Base
Derived
```



## <a id="sec-generated-title-5"></a> <a id="downcast"></a>ダウンキャスト

基底クラスの変数に派生クラスの変数を渡すことを<strong id="upcast" class="keyword">アップキャスト</strong>（upcast）と呼び、
それとは逆に、
派生クラスの変数に基底クラスの変数を渡すことを<strong id="downcast" class="keyword">ダウンキャスト</strong>（downcast）と呼びます。

基底クラスの変数に派生クラスのインスタンスを格納することは何の問題もありませんので、
アップキャストは常に安全に行うことが出来ます。
ところが、ダウンキャストの場合は必ずしも安全には行うことが出来ません。
以下に危険なダウンキャストの例を挙げます。

```csharp
class Base{}
class Derived1 : Base{}
class Derived2 : Base{}

class DowncastTest
{
  static void Main()
  {
    Derived1 d1 = new Derived1(); // 当然、合法。
    Derived2 d2 = new Derived2(); // 同じく、合法。

    Base b;
    Derived1 d;

    b = d1;          // アップキャストは常に合法。明示的なキャスト不要。
    d = (Derived1)b; // ダウンキャストは明示的なキャストが必要。
    // Derived1 の変数に Derived1 のインスタンスを格納しているので、これはOK。

    b = d2;          // 同じ事を今度は d2 の方で繰り返す。
    d = (Derived1)b;
    // Derived1 の変数に Derived2 のインスタンスを格納しているので、これは問題あり。
    // コンパイルは通るが、実行時エラーになる。
  }
}
```


このプログラムを実行すると <code>InvalidCastException</code> という例外が発生します。
(例外については「[例外処理](../structured/oo_exception.md)」で説明します。)

このような問題があるため、ダウンキャストを行う際には動的な型情報を取得する必要があります。
そのための構文として C# には <strong id="is-operator" class="keyword">is 演算子</strong>と <strong id="as-operator" class="keyword">as 演算子</strong>があります。

is 演算子はキャスト可能かどうかを調べるための演算子で以下のようにして使用します。

```csharp {title="is 演算子"}
変数名 is 型名
```


is 演算子を適用した結果は bool 型になり、
左辺の変数が右辺の型にキャスト可能ならば true を、不能ならば false を返します。

```csharp {title="is 演算子の例" highlight-ranges="sha256:ad777eb5165d3ac4a9c8aba50ce68131796cc98070574f205af24cf3e1bb3710;14:8-14:21,18:8-18:21"}
using System;

class Base{}
class Derived1 : Base{}
class Derived2 : Base{}

class DowncastTest
{
  static void Main()
  {
    Base b;

    b = new Derived1();
    if(b is Derived1)
      Console.Write("b = new Derived1();\n");

    b = new Derived2();
    if(b is Derived1)
      Console.Write("b = new Derived2();\n");
  }
}
```


```console
b = new Derived1();
```


as 演算子はキャストと同じような働きをする演算子で、以下のようにして使用します。

```csharp {title="as 演算子"}
変換先の変数 = 変換元の変数 as 型名
```


キャストとの違いは、
もし型変換が出来ない場合には結果が null になるということです。

```csharp {highlight-ranges="sha256:1478b6a7e13160662a467bb15a56fe14ba02b8cda32166be50931ce59d5ed7f6;15:9-15:22,20:9-20:22"}
using System;

class Base{}
class Derived1 : Base{}
class Derived2 : Base{}

class DowncastTest
{
  static void Main()
  {
    Base b;
    Derived1 d;

    b = new Derived1();
    d = b as Derived1;
    if(d != null)
      Console.Write("b = new Derived1();\n");

    b = new Derived2();
    d = b as Derived1;
    if(d != null)
      Console.Write("b = new Derived2();\n");
  }
}
```


```console
b = new Derived1();
```


### <a id="sec-generated-title-6"></a> <a id="type-switch"></a>is演算子の拡張

<h5 class="version version7">Ver. 7</h5>

C# 7では、`is`演算子で以下のような書き方ができるようになりました。

```csharp {title="is 演算子の拡張"}
変数名 is 型名 新しい変数名
```

演算子の結果はこれまで通り`bool`で、左辺の変数の中身が右辺の型にキャストできるなら`true`、できないなら`false`を返します。
そして、キャストできるとき、そのキャスト結果が新しい変数に入ります。
例えば、以下のような書き方ができます。

```csharp {title="C# 7の新しいis演算子の例"}
static void TypeSwitch(object obj)
{
    // C# 7での新しい書き方
    if (obj is string s)
    {
        Console.WriteLine("string #" + s.Length);
    }
}
```

詳しくは、「[型スイッチ](../datatype/typeswitch.md#is)」で説明します。

## <a id="sec-generated-title-7"></a> <a id="virtual"></a>仮想メソッド

C# では、何も指定しない通常のメソッド呼び出し時、
基底クラスと派生クラスに同名のメソッドがある場合、
どちらのメソッドが呼び出されるかは静的な型によって決定されます。

```csharp {title="静的型情報に基づいたメソッドが呼び出し"}
using System;

class Base
{
  public void Test(){Console.Write("Base.Test()\n");}
}

class Derived : Base
{
  public new void Test(){Console.Write("Derived.Test()\n");}
}

class NonVirtualTest
{
  static void Main()
  {
    Base    a = new Base();
    a.Test(); // Base の Test が呼ばれる。

    Base    b = new Derived();
    b.Test(); // Base の Test が呼ばれる。

    Derived c = new Derived();
    c.Test(); // Derived の Test が呼ばれる。
  }
}
```


```console
Base.Test()
Base.Test()
Derived.Test()
```


しかし、動的な型に基づいて呼び出されるメソッドを決定したい場合があります。
（というより、ほとんどの場合、メソッド呼び出しは動的に決定した方が都合がいい。）
動的な型に基づいて呼び出されるメソッドを選びたい場合、
以下のように、
メソッドに <em>virtual</em> という修飾子を付けます。

```csharp {title="動的型情報に基づいたメソッドが呼び出し" highlight-ranges="sha256:aae84d9edcc4f46f43bb91d76c5d95468916f3cbadf09a66003e6a56fe025b29;5:10-5:17,10:10-10:18"}
using System;

class Base
{
  public virtual void Test(){Console.Write("Base.Test()\n");}
}

class Derived : Base
{
  public override void Test(){Console.Write("Derived.Test()\n");}
}

class VirtualTest
{
  static void Main()
  {
    Base    a = new Base();
    a.Test(); // Base の Test が呼ばれる。

    Base    b = new Derived();
    b.Test(); // Derived の Test が呼ばれる。

    Derived c = new Derived();
    c.Test(); // Derived の Test が呼ばれる。
  }
}
```


```console
Base.Test()
Derived.Test()
Derived.Test()
```


このような virtual 修飾子をつけたメソッドのことを<strong id="virtual_method" class="keyword">仮想メソッド</strong>（virtual method）と呼びます。

また、仮想メソッドを派生クラスで再定義することをメソッドの<strong id="override" class="keyword">オーバーライド</strong>(override: 上に重なる)と言います。
オーバーロード(「[関数](../structured/st_function.md)」のところにある「関数のオーバーロード」を参照)と混乱しそうになる名前ですが、別物です。

さらに、C#では、「[基底クラスのメンバーの隠蔽](oo_inherit.md#conceal)」と同様に、
プログラマの意図しないところでメソッドがオーバーライドされてしまうのを防ぐため、
メソッドをオーバーライドする際には <em>
        <code>override</code> 修飾子
      </em>を明示的に付ける必要があります。


## <a id="sec-generated-title-8"></a> <a id="usage"></a>仮想メソッドの利用例

仮想メソッド、すなわち、メソッドの動的呼び出しを用いると、
どのようなことが出来るのかを説明します。

ここではまた、例として Person クラスを使いましょう。
人間と一口に言ってもいろいろな人がいます。
例えば、年齢を聞いても、
正直に答える人、
鯖を読む人、
大体の年齢しか答えない人とさまざまなタイプの人がいます。

このようなさまざまなタイプの人をクラスで表現してみましょう。
まずは共通部分をまとめた基底クラス(<code>Person</code>)を定義します。
年齢を取得するプロパティ <code>Age</code> は、virtual にしておいて、
とりあえず意味のない値を返しておきます。

```csharp {title="人間の基底クラス"}
class Person
{
  protected string name;
  protected int age;

  public Person(string name, int age)
  {
    this.name = name;
    this.age  = age;
  }

  public string Name{get{return this.name;}}
  public virtual int Age{get{return 0;}} // 基底クラスでは特に意味のない値を返す。
}
```


次に正直者を表すクラス(<code>Truepenny</code>)を定義します。
<code>Truepenny</code> の <code>Age</code> プロパティでは実年齢をそのまま返します。

```csharp {title="正直者クラス"}
/// <summary>
/// 正直者。
/// 年齢を偽らない。
/// </summary>
class Truepenny : Person
{
  public Truepenny(string name, int age) : base(name, age){}

  public override int Age
  {
    get
    {
      // 実年齢をそのまま返す。
      return this.age;
    }
  }
}
```


次は嘘つき(<code>Liar</code>)クラスの定義です。
<code>Liar</code> の <code>Age</code> プロパティでは、
歳を取るにつれ大幅に鯖を読んだ値を返します。

```csharp {title="嘘つきクラス"}
/// <summary>
/// 嘘つき。
/// 鯖を読む(しかも、歳取るにつれ大幅に)。
/// </summary>
class Liar : Person
{
  public Liar(string name, int age) : base(name, age){}

  public override int Age
  {
    get
    {
      // 年齢を偽る。
      if(this.age < 20) return this.age;
      if(this.age < 25) return this.age - 1;
      if(this.age < 30) return this.age - 2;
      if(this.age < 35) return this.age - 3;
      if(this.age < 40) return this.age - 4;
      return this.age - 5;
    }
  }
}
```


次はいい加減な人(<code>Equivocator</code>)クラスの定義です。
<code>Equivocator</code> の <code>Age</code> プロパティでは、
実年齢を四捨五入した値を返します。

```csharp {title="いい加減な人のクラス"}
/// <summary>
/// いいかげん。
/// 大体の歳しか答えない。
/// </summary>
class Equivocator : Person
{
  public Equivocator(string name, int age) : base(name, age){}

  public override int Age
  {
    get
    {
      // 年齢を四捨五入した値を返す。
      return ((this.age + 5) / 10) * 10;
    }
  }
}
```


おまけで永遠の17歳。

```csharp {title="永遠の17歳"}
/// <summary>
/// いくつになったって気持ちは17歳。
/// </summary>
class Seventeenist : Person
{
  public Seventeenist(string name, int age) : base(name, age) { }

  public override int Age
  {
    get
    {
      return 17;
    }
  }
}
```


最後に、これらのクラスを利用したプログラムを作ってみます。
以下の例では、<code>Person</code> クラスを引数とし、
その人の自己紹介文を画面に表示するメソッドを用意し、
正直者、嘘つき、いい加減な人のそれぞれに自己紹介をしてもらいます。

```csharp {title="Person クラスとその派生クラスの利用例"}
using System;

class PolymorphismTest
{
  static void Main()
  {
    Introduce(new Truepenny   ("Ky Kiske"  , 24)); // 正直者のカイさん24歳。
    Introduce(new Liar        ("Axl Low"   , 24)); // 嘘つきのアクセルさん24歳。
    Introduce(new Equivocator ("Sol Badguy", 24)); // いい加減なソルさん24歳。
    Introduce(new Seventeenist("Ino"       , 24)); // 時空を超えるイノさん24歳。
  }

  /// <summary>
  /// p さんの自己紹介をする。
  /// </summary>
  static void Introduce(Person p)
  {
    Console.Write("My name is {0}.\n", p.Name);
    Console.Write("I'm {0} years old.\n\n", p.Age);
  }
}
```


```console
My name is Ky Kiske.
I'm 24 years old.

My name is Axl Low.
I'm 23 years old.

My name is Sol Badguy.
I'm 20 years old.

My name is Ino.
I'm 17 years old.
```


正直者、嘘つき、いい加減な人はいずれも実年齢24歳にしてあります。
しかし、画面に表示される自己紹介文では異なる年齢が表示されています。

<code>Introduce</code> メソッド中では、
<code>Person</code> の <code>Age</code> プロパティが呼び出されていますが、
実際には、動的型情報に基づき、
<code>Truepenny</code>、<code>Liar</code>、<code>Equivocator</code> の
<code>Age</code> プロパティが呼び出されます。


## <a id="sec-generated-title-9"></a> <a id="polymorphism"></a>多態性とは

仮想メソッドの利用例のところで示したとおり、
仮想メソッドを用いると、同じメソッドを呼び出しても、
変数に格納されているインスタンスの型によって異なる動作をします。
このように、同じメッセージ(メソッド呼び出し)に対し、
異なるオブジェクトが異なる動作をすることを<strong id="polymorphism" class="keyword">多態性</strong>（polymorphism: ポリモーフィズム）と呼びます。

仮想メソッド呼び出しの他にも、
メソッドのオーバーロード
(同じ名前のメソッドでも、引数が異なれば動作も異なる)
なども多態性の一種であると考えられます。
しかし、メソッドのオーバーロードはその動作がコンパイル時に決定しますが、
仮想メソッド呼び出しの動作は実行時に決定するという違いがあります。
(前者を静的多態性、後者を動的多態性と言って区別する場合もあります。)

## <a id="sec-generated-title-10"></a> <a id="covariance">戻り値の共変性</a>

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 (.NET 5.0)から、仮想メソッドの戻り値に共変性が認められるようになりました。
(機能名の俗称としては、「クラスの共変戻り値」と言ったりします。)

例えば以下のようなコードを書けるようになります。

```csharp {title="仮想メソッド戻り値の共変性"}
class Base
{
    public virtual Base Clone() => new Base();
}
 
class Derived : Base
{
    // これの戻り値が Base じゃなくてもよくなった。
    // Derived は常に Base に安全に変換可能なので、 Base Clone() の override として Derived Clone() を使える。
    public override Derived Clone() => new Derived();
}
```

get のみのプロパティでも同様に、共変なオーバーライドができます。

```csharp {title="get のみのプロパティの共変戻り値"}
class Base
{
    public virtual Base P { get; }
}
 
class Derived : Base
{
    // get のみの時は OK。
    // set を書いちゃうとコンパイル エラー。
    public override Derived P { get; }
}
```

### <a id="sec-generated-title-11"></a> <a id="runtime-feature">ランタイム側の修正</a>

[デリゲート](../functional/sp_delegate.md#co-contra)や[ジェネリクス](sp4_variance.md)では元々できていたことなので、今までできなかったことの方が不思議なくらいです。
(実際、似たような言語でいうと、Java は JDK 5.0 以降で共変戻り値をサポートしています。)

[インターフェイスのデフォルト実装](oo_interface.md#runtime-feature)が C# 8.0 でやっと実装されたのと同様で、 .NET ランタイム側の修正が必要なためこれまで未実装でした。

ランタイム側の修正が必要ということは、古いランタイムでは動かせません。
[言語バージョン](../cheatsheet/langversionoption.md)で `LangVersion` 9.0 を明示的に指定していても、ターゲット フレームワークが .NET 5.0 (`net5.0`)以降でないとコンパイルできません。

ランタイム側の修正に関しては、以前書いたブログ「[RuntimeFeature クラス](../../../blog/2018/12/runtimefeature/index.md)」で説明しています。
(.NET 5.0 で `RuntimeFeature` クラスに `CovariantReturnsOfClasses` が追加されています。)

### <a id="sec-generated-title-12"></a> <a id="interface-covariant-returns">注意: インターフェイスの共変戻り値(C# 9.0 時点で未対応)</a>

C# 9.0 時点では共変戻り値を使えるのはクラスの仮想メソッド・仮想プロパティのみです。
将来的にはインターフェイスに対しても共変戻り値のサポートを考えているようですが、後回しにしたそうです。

例えば以下のようなコードはおそらく書きたい意図とは異なる挙動になると思います。

```csharp {title="インターフェイスの共変戻り値は C# 9.0 時点ではないという例1" warning-ranges="sha256:7f17aae47ab1ba285f2c844bb04fa169d54f0bad165390dcaeb885b346f3c70c;10:8-10:9"}
interface IA
{
    IA M();
}
 
interface IB : IA
{
    // 以下の行は override 扱いを受けない。
    // 「IA.M を隠してしまう(別メソッド扱いされる)」という警告が出る。
    IB M();
}
```

以下のようなコードはコンパイル エラーになります。

```csharp {title="インターフェイスの共変戻り値は C# 9.0 時点ではないという例2" error-ranges="sha256:2235d994b0ca3def2faed32a1b7068fcd3d5571a74d313b70c50392e30516398;9:11-9:12"}
interface IA
{
    public IA M() => null;
}
 
interface IB : IA
{
    // コンパイル エラー(IA.M と一致しない)
    IB IA.M() => null;
}
```

以下のような実装クラスもコンパイル エラーになります。

```csharp {title="インターフェイスの共変戻り値は C# 9.0 時点ではないという例3" error-ranges="sha256:cad49684ff30da2d00092a4851ec9f4f201243e02d767bebfa78dba3a8d53a44;6:16-6:18"}
interface IA
{
    IA M();
}
 
class ImpleA : IA
{
    // コンパイル エラー(IA.M を実装していない)
    public ImpleA M() => this;
}
```
## <a id="exercise"></a>演習問題

### <a id="exercise-polim1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Triangle</code> クラスを元に、
以下のような継承構造を持つクラスを作成せよ。

まず、三角形や円等の共通の基底クラスとなる <code>Shape</code> クラスを以下のように作成。

```csharp {title="Shape"}
/// <summary>
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// </summary>
class Shape
{
  virtual public double GetArea() { return 0; }
  virtual public double GetPerimeter() { return 0; }
}
```


そして、<code>Shape</code> クラスを継承して、
三角形 <code>Triangle</code> クラスと
円 <code>Circle</code> クラスを作成。

```csharp {title="Triangle"}
/// <summary>
/// 2次元空間上の三角形をあらわすクラス
/// </summary>
class Triangle : Shape
```


```csharp {title="Circle"}
/// <summary>
/// 2次元空間上の円をあらわすクラス
/// </summary>
class Circle : Shape
```



#### 解答例 1


```csharp {title="Shape、Triangle、Circle"}
using System;

/// <summary>
/// 2次元の点をあらわす構造体
/// </summary>
struct Point
{
  double x; // x 座標
  double y; // y 座標

  #region 初期化

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

  #endregion
  #region プロパティ

  /// <summary>
  /// x 座標。
  /// </summary>
  public double X
  {
    get { return this.x; }
    set { this.x = value; }
  }

  /// <summary>
  /// y 座標。
  /// </summary>
  public double Y
  {
    get { return this.y; }
    set { this.y = value; }
  }

  #endregion
  #region 演算子

  /// <summary>
  /// ベクトル和
  /// </summary>
  /// <param name="a">点A</param>
  /// <param name="b">点B</param>
  /// <returns>和</returns>
  public static Point operator +(Point a, Point b)
  {
    return new Point(a.x + b.x, a.y + b.y);
  }

  /// <summary>
  /// ベクトル差
  /// </summary>
  /// <param name="a">点A</param>
  /// <param name="b">点B</param>
  /// <returns>和</returns>
  public static Point operator -(Point a, Point b)
  {
    return new Point(a.x - b.x, a.y - b.y);
  }

  #endregion

  /// <summary>
  /// A-B 間の距離を求める。
  /// </summary>
  /// <param name="a">点A</param>
  /// <param name="b">点B</param>
  /// <returns>距離AB</returns>
  public static double GetDistance(Point a, Point b)
  {
    double x = a.x - b.x;
    double y = a.y - b.y;
    return Math.Sqrt(x * x + y * y);
  }

  public override string ToString()
  {
    return "(" + x + ", " + y + ")";
  }
}

/// <summary>
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// </summary>
class Shape
{
  virtual public double GetArea() { return 0; }
  virtual public double GetPerimeter() { return 0; }
}

/// <summary>
/// 2次元空間上の円をあらわすクラス
/// </summary>
class Circle : Shape
{
  Point center;
  double radius;

  #region 初期化

  /// <summary>
  /// 半径を指定して初期化。
  /// </summary>
  /// <param name="r">半径。</param>
  public Circle(Point center, double r)
  {
    this.center = center;
    this.radius = r;
  }

  #endregion
  #region プロパティ

  /// <summary>
  /// 円の中心。
  /// </summary>
  public Point Center
  {
    get { return this.center; }
    set { this.center = value; }
  }

  /// <summary>
  /// 円の半径。
  /// </summary>
  public double Radius
  {
    get { return this.radius; }
    set { this.radius = value; }
  }

  #endregion
  #region 面積・周

  /// <summary>
  /// 円の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public override double GetArea()
  {
    return Math.PI * this.radius * this.radius;
  }

  /// <summary>
  /// 円の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public override double GetPerimeter()
  {
    return 2 * Math.PI * this.radius;
  }

  #endregion

  public override string ToString()
  {
    return string.Format(
      "Circle (c = {0}, r = {1})",
      this.center, this.radius);
  }
}

/// <summary>
/// 2次元空間上の三角形をあらわすクラス
/// </summary>
class Triangle : Shape
{
  Point a;
  Point b;
  Point c;

  #region 初期化

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

  #endregion
  #region プロパティ

  /// <summary>
  /// 頂点A。
  /// </summary>
  public Point A
  {
    get { return a; }
    set { a = value; }
  }

  /// <summary>
  /// 頂点B。
  /// </summary>
  public Point B
  {
    get { return b; }
    set { b = value; }
  }

  /// <summary>
  /// 頂点C。
  /// </summary>
  public Point C
  {
    get { return c; }
    set { c = value; }
  }

  #endregion
  #region 面積・周

  /// <summary>
  /// 三角形の面積を求める。
  /// </summary>
  /// <returns>面積</returns>
  public override double GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    return 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  /// <summary>
  /// 三角形の周の長さを求める。
  /// </summary>
  /// <returns>周</returns>
  public override double GetPerimeter()
  {
    double l = Point.GetDistance(this.a, this.b);
    l += Point.GetDistance(this.a, this.c);
    l += Point.GetDistance(this.b, this.c);
    return l;
  }

  #endregion

  public override string ToString()
  {
    return string.Format(
      "Circle (a = {0}, b = {1}, c = {2})",
      this.a, this.b, this.c);
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

    Circle c = new Circle(
      new Point(0, 0), 3);

    Show(t);
    Show(c);
  }

  static void Show(Shape f)
  {
    Console.Write("{0}\n", f);
    Console.Write("{0}\n", f.GetArea());
    Console.Write("{0}\n", f.GetPerimeter());
  }
}
```
