---
title: "抽象メソッド、抽象クラス"
source_url: "https://ufcpp.net/study/csharp/oop/oo_abstract/"
content_type: "Article"
published_at: "2015-05-06T14:09:53"
updated_at: "2021-02-21T18:01:58"
tags: []
umbraco_id: 1267
parent_id: 1248
sort_order: 15
aliases:
  - "/study/csharp/oo_abstract.html"
---

# 抽象メソッド、抽象クラス

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

抽象メソッドとは、実装を持たず、メソッドの意味（規約）だけを定義したメソッドです。
抽象メソッドの実装は基底クラスでは行わず、派生クラスで行います。

また、抽象クラスとは、
インスタンスを生成出来ないクラスのことで、
継承して使うことを前提としたクラスのことです。


##### <a id="sec-generated-title-2"></a>ポイント

* 抽象メソッド: 基底クラスでは実装せず、メソッドの意味（規約）だけを定義して、派生クラスで具体的な実装を行うようなメソッド。

* （C++ では純粋仮想関数と呼ばれていたものです。）

* 抽象メソッドを1個でも持つクラス（抽象クラス）は、インスタンスを生成することができません。

* クラスやメソッドの前に abstract キーワードを付ける。



## <a id="sec-generated-title-3"></a> <a id="abstraction"></a>抽象化

「[多態性](oo_polymorphism.md)」で、
仮想メソッドの利用例として <code>Person</code> クラスを挙げました。
この <code>Person</code> 基底クラスには、
<code>Age</code> というプロパティがありますが、
このプロパティ自体は意味のある値を返さず、
実装は派生クラスの <code>Age</code> プロパティで行っていました。

```csharp {title="人間の基底クラス"}
class Person
{
  // ここではあんまり関係ないんで name は省略。
  protected int age;

  public Person(int age){this.age  = age;}

  public virtual int Age
  {
    // 基底クラスでは特に意味のない値を返す。
    // 意味のある実装は派生クラスで行います。
    get
    {
      return 0;
    }
  }
}
```


しかし、<code>Person</code> クラスのように、
意味のない値を返すメソッドを持つクラスのインスタンスが生成されてしまうというのはあまり好ましいことではありません。

この問題を解決するためには2つの方法があります。
1つは基底クラスにデフォルトの動作を定める方法です。
すなわち、
性善説を信じて <code>Person</code> がデフォルトで正直な答えを返すようにするか、
性悪説を信じて <code>Person</code> がデフォルトで鯖を読むようにするか、
とにかく、<code>Person</code> の <code>Age</code> プロパティが何らかの意味を持つ値を返すようにします。

```csharp {title="性善説を信じた人間クラス"}
class Person
{
  protected int age;

  public Person(int age){this.age  = age;}

  public virtual int Age
  {
    // 性善説を信じてみる。
    // 普通の人はみんな正直に年齢を答えてくれるに違いない。
    get
    {
      return this.age;
    }
  }
}
```


そして、もう1つの方法は、<code>Person</code> クラスのインスタンスを生成出来ないようにすることです。
例えば、<code>Person</code> クラスのコンストラクタを protected にしてしまえば、<code>Person</code> クラスのインスタンスは外部から生成できなくなります。

```csharp {title="Person クラスのインスタンスを生成不能に" highlight-ranges="sha256:4d3bf539bb3def74623e5a19e5145d5f597cb55af9633b72ab0be961a131d804;7:3-7:12"}
class Person
{
  protected int age;

  // ↓ protected なので外部からコンストラクタを呼べない。
  //    Person は継承して使う専用のクラスになります。
  protected Person(int age){this.age  = age;}

  public virtual int Age{get{return 0;}}
}
```


これで <code>Person</code> クラスのインスタンスが作られることはなくなるんですが、
まだ <code>Person</code> クラスに意味のないメソッドの実装が残っています。
これは意味のないものをわざわざ書かなくてはいけないので無駄になりますし、
サブクラスでちゃんとオーバーライドしなければ無意味な値が返されてしまうという問題があります。

この問題を解決するため、
C# にはインスタンスを作成できないクラスや、
実装のない(派生クラスで必ずオーバーライドしなければならない)メソッドを定義するための構文が用意されています。

インスタンスを作成できないクラスは<strong id="abclass" class="keyword">抽象クラス</strong>（abstract class）と呼ばれています。
抽象クラスを作成するには、クラスの定義時に <em>
        <code>abstract</code>
      </em> 修飾子を付けます。

```csharp {title="抽象クラスの定義" highlight-text="abstract"}
abstract class Person
{
  protected int age;

  // 抽象クラスなので、コンストラクタが public であってもインスタンスは生成できない。
  public Person(int age){this.age  = age;}

  public virtual int Age{get{return 0;}}
}
```


また、実体を持たず、意味だけを定義し、実装は派生クラスで行うメソッドは<strong id="abmethod" class="keyword">抽象メソッド</strong>（abstract method）と呼ばれています。
抽象メソッドを作成するには、メソッドの定義時に <code>abstract</code> 修飾子を付けます。
抽象メソッドは抽象クラス中でしか定義できません。

ちなみに、「[プロパティ](oo_property.md#property)」も、内部的に見るとメソッドのようなものなので、
abstract を付けて抽象プロパティにすることができます。

```csharp {title="抽象メソッドの定義" highlight-ranges="sha256:f585a65a72130778627f57f2078515994a15c1590613c008a461eb78db928a3b;7:10-7:18"}
abstract class Person
{
  protected int age;

  public Person(int age){this.age  = age;}

  public abstract int Age{get;} // 抽象メソッドや抽象プロパティには定義は要らない
}
```



##### <a id="sec-generated-title-4"></a>サンプル

いままで例に挙げてきた <code>Person</code> クラスの最終形です。

```csharp
using System;

abstract class Person
{
  protected string name;
  protected int age;

  public Person(string name, int age)
  {
    this.name = name;
    this.age  = age;
  }

  public string Name{get{return this.name;}}
  public abstract int Age{get;} // 抽象メソッドには定義は要らない
}

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
      // 「おいおい」って突っ込み入れてあげてね。
      return 17;
    }
  }
}

class PolymorphismTest
{
  static void Main()
  {
    Introduce(new Truepenny  ("Ky Kiske"  , 24)); //正直者のカイさん24歳。
    Introduce(new Liar       ("Axl Low"   , 24)); //嘘つきのアクセルさん24歳。
    Introduce(new Equivocator("Sol Badguy", 24)); //いい加減なソルさん24歳。
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


## <a id="exercise"></a>演習問題

### <a id="exercise-abst1"></a>問題 1


[多態性](oo_polymorphism.md)の[問題 1](oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスを抽象クラス化せよ。


#### 解答例 1


必要な箇所（Shape クラスの部分）だけ抜粋。

```csharp {title="Shape"}
/// <summary>
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// </summary>
abstract class Shape
{
  public abstract double GetArea();
  public abstract double GetPerimeter();
}
```
