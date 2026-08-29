---
title: "Extensions (拡張型)"
source_url: "https://ufcpp.net/blog/2024/3/extensions/"
content_type: "BlogEntry"
published_at: "2024-03-20T19:51:16"
updated_at: "2024-03-20T19:51:16"
tags: []
umbraco_id: 2495
parent_id: 2490
sort_order: 4
aliases: []
---

# Extensions (拡張型)

C# 3.0 から[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)が使えるわけですが、
もうちょっといろんな「拡張」をしたいという話が前々からあります。
例えば以下のような要求。

* 既存の型に静的メンバーも足したい
* プロパティや演算子も足したい
* インターフェイスの後付けもしたい

今では Extensions とか呼ばれていまして、以下の issue でトラッキング中。

* [Exploration: Shapes and Extensions #164](https://github.com/dotnet/csharplang/discussions/164)

ここからさかのぼって、かつては Extension everything とか呼ばれていたり、
個別に「インターフェイスを実装したい」「演算子を拡張したい」など個別の issue がありました。

* [Extension Everything](https://github.com/dotnet/roslyn/issues/11159)
  * [Extension classes with Interfaces](https://github.com/dotnet/roslyn/issues/3357)
  * [Extension operators](https://github.com/dotnet/roslyn/issues/4945)

2015年([Roslyn](https://github.com/dotnet/roslyn) が GitHub での公開に切り替わった年)にはすでにそんな話が出ています。

結構大きな機能なのでしり込みしていたみたいですが、
去年くらいから Working Group (この機能の追加を推進するメンバーを割り当てて、定期的にミーティング)を設けて作業を始めました。

* [Extensions がらみの議事録](https://github.com/dotnet/csharplang/tree/3cd77d5664281f6df4785a35d4b778c88ec3aa98/meetings/working-groups/roles)

うちのブログでも去年、1度取り上げています。

* [【C# 12 候補】 Extensions](../../../2023/3/extensions/index.md)

もう9年も経ってしまい、C# 12 でも入らなかったわけですが、
ついに今年、C# 13 には一部入りそう(インターフェイスの後付けだけは無理そう)な雰囲気になっています。

最近の話題のうちいくつかを取り上げると、以下のような話が出ています。

* [段階的に実装していく](https://github.com/dotnet/csharplang/blob/3cd77d5664281f6df4785a35d4b778c88ec3aa98/meetings/working-groups/roles/extensions-2024-01-25.md#scope-and-priorities-for-c13)
  * 静的メンバー → インスタンス メンバー → 継承のサポート (C# 13 でやれそうなのはここまで) → インターフェイスの後付け
* [普通の構造体でラッパー型を作って、利用時に Unsafe.As で変換してメンバーを呼ぶ](https://github.com/dotnet/csharplang/issues/7771)
  * 型消去な実装
* [インターフェイス実装は大変そう](https://github.com/dotnet/csharplang/blob/7a3990a2bec382871de3a0615746d274ae924b6b/proposals/extension-interfaces.md)
* [メンバーのルックアップ](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-28.md)
  * クラスの継承時の挙動に準ずる
  * 旧拡張メソッドと新 Extensions は優先度をつけない(どちらにも同名メソッドがあった場合、コンパイル エラーにする)案が今のところ優勢

## extension 構文

ということで、改めて Extensions の話を。
今、以下のような構文を足そうとしています。

```csharp {title="extension 構文"}
// 拡張の構文例。
implicit extension SomeExtension for SomeClass : IEquatable<SomeExtension>
{
    // 追加したいメンバーを書く。

    // 1. 静的メンバーも書ける。
    public static int Y => X * X;

    // 2. メソッド以外も書ける。
    public int Property
    {
        get => GetValue();
        set => SetValue(value);
    }

    public int this[int index] => GetValue(index);

    // 3. インターフェイスの実装を持てる。
    public bool Equals(SomeExtension? other) => Property == other?.Property;
}

// 拡張の対象の例。
class SomeClass
{
    // (中身は適当。)
    public static int X = 123;

    private int _value;

    public int GetValue() => _value;
    public void SetValue(int value) => _value = value;
    public int GetValue(int index) => _value * index;
}
```

ちなみに、「インターフェイスの実装を持つ」には少し難題があって、
C# 13 時点では入らない可能性がかなり高いです。

## 普通の構造体 + Unsafe.As

拡張はラッパー構造体を使った実装になりそうです。
一時期は以下のような ref struct を使った実装になりそうだったんですが、
この案は結局没になりました。

```csharp {title="ref struct 案"}
var value = new SomeStruct();
var extension = new SomeExtension(ref value);

// 拡張プロパティを呼び出す。
extension.Property = 123;

// ちゃんと元インスタンスに値が反映。
Console.WriteLine(value.GetValue());

ref struct SomeExtension(ref SomeStruct @this)
{
    ref SomeStruct @this = ref @this;

    public int Property
    {
        get => @this.GetValue(); // ref で持ってるので、引数でもらった構造体に書き換えが反映される。
        set => @this.SetValue(value);
    }
}

// デモ用に構造体に変更。
struct SomeStruct
{
    private int _value;

    public int GetValue() => _value;
    public void SetValue(int value) => _value = value;
    public int GetValue(int index) => _value * index;
}
```

この案に変わって、普通の構造体 + Unsafe.As を使う路線で考えているそうです。

```csharp {title="Unsafe.As 案"}
using System.Runtime.CompilerServices;

var value = new SomeStruct();

// Unsafe.As を使って、value 値が入っているの場所を無理やり SomeExtension で解釈。
ref var extension = ref Unsafe.As<SomeStruct, SomeExtension>(ref value);

// 拡張プロパティを呼び出す。
extension.Property = 123;

// extension の参照先が value なので、ちゃんと value が書き変わる。
Console.WriteLine(value.GetValue());

// 普通の構造体。
struct SomeExtension
{
    private SomeStruct @this;

    public int Property
    {
        get => @this.GetValue();
        set => @this.SetValue(value);
    }
}

// SomeStruct は先ほどと同じ。
```

## 型消去

Extensions は普通の型と同じように使えたりします。
(特に、`explicit` を付けた Extensions はむしろ「型を明示しないと使えない」状態になります。)
なのでこれを拡張型(extension types)と呼んだりもします。

で、前節の通り Extensions のコンパイル結果はラッパー構造体だったりするわけですが、
このラッパー構造体への変換(Unsafe.As)はあくまでメンバー参照のタイミングで行われます。
メソッドの引数などに拡張型を書くと、実際には「元の型 + 属性」(いわゆる「型消去」方式)になる予定です。
例えば、以下のようなメソッドを書いたとして、

```csharp {title="拡張型を引数に書く例"}
static int Sum(SomeExtension a, List<SomeExtension> b)
{
    var sum = a.Property;
    foreach (var x in b) sum += x.Property;
    return sum;
}
```

以下のような類のコードに置き換わる予定です。

```csharp {title="拡張型を引数に書く例の展開結果"}
static int Sum(
    // SomeExtension は属性の中にしか残らない。
    // 元の、 SomeStruct に置き換わる。
    [Extension(typeof(SomeExtension))] SomeStruct a,
    [Extension(typeof(SomeExtension))] List<SomeStruct> b)
{
    // メンバーアクセスするところで Unsafe.As
    var sum = Unsafe.As<SomeStruct, SomeExtension>(ref a).Property;
    foreach (var x in b) sum += Unsafe.As<SomeStruct, SomeExtension>(ref Unsafe.AsRef(in x)).Property;
    return sum;
}
```

[変性](../../../../study/csharp/oop/sp4_variance.md)を持っていない `List<T>` で、
`List<SomeStruct>` を `List<SomeExtension>` に変換する手段は通常全くありません。
型消去で `List<SomeExtension>` が `List<SomeStruct>` に置き換わることで、
`List<SomeStruct>` 型の変数を `List<SomeExtension>` 型の引数に渡せるようになっています。

## メンバーのルックアップ(継承)

拡張型は元となる型との間には、クラスの継承関係と似た関係が成り立ちます。
なので、メンバーのルックアップのルールも「クラスの継承に準ずる」で行きたいそうです。
例えば、派生クラスから基底クラスのメンバーを何の修飾もなしで(`this.` とか `base.` が必須ではなく)参照できるように、
拡張型から元となる型のメンバーも修飾なしで参照できます。

おさらい的に、「継承があるときのルックアップ」の例をいくつか紹介しておきます。
(拡張型中で元となる型と同名のメンバーを書くとこれに準ずることになると思われます。)

近い側優先:

```csharp {title="基底クラスと同名のメンバー参照"}
class Base
{
    public void M(int x) { }
}

class Derived : Base
{
    public new void M(int x) { }

    public void M()
    {
        // 近い側優先なので、Derived.M が呼ばれる。
        M(1);
    }
}
```

もうちょっとわかりにくい例:

```csharp {title="基底クラスと同名で、引数の型が違うメンバー参照" warning-ranges="8:21-8:22" warning-diagnostics="CS0109@8:21-8:22"}
class Base
{
    public void M(int x) { }
}

class Derived : Base
{
    public new void M(object x) { }

    public void M()
    {
        // わかりにくいけども、Derived.M(object) の方が呼ばれる。
        // 引数の型を考えると Base.M(int) が呼ばれそうに見えるけども、そうはならない。
        // (「元々はなかったけど後から Base の方に M(int) が追加された」みたいな状況で破壊的変更にならないようにするため。)
        M(1);
    }
}
```

## メンバーのルックアップ(拡張同士)

あと、既存の拡張メソッドには以下のような優先度があります。

```csharp {title="インスタンス メソッド優先"}
namespace Ex1
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("Extension in Ex1");
    }
}

namespace App1
{
    class A
    {
        public void M() => Console.WriteLine("Instance");
    }

    class Program
    {
        public static void Main()
        {
            // インスタンス メソッド優先。
            new A().M(); // Instance
        }
    }
}
```

```csharp {title="同じ名前空間内の拡張メソッド優先"}
namespace Ex1
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("Extension in Ex1");
    }
}

namespace App1
{
    class A;

    static class AExtension
    {
        public static void M(this A _) => Console.WriteLine("Extension in App1");
    }

    class Program
    {
        public static void Main()
        {
            // 同じ名前空間内の拡張メソッド優先。
            new A().M(); // in App1
        }
    }
}
```

```csharp {title="内側で using した方優先"}
using Ex1;

namespace Ex1
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("Extension in Ex1");
    }
}

namespace Ex2
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("Extension in Ex1");
    }
}

namespace App1
{
    using Ex2;

    class A;

    class Program
    {
        public static void Main()
        {
            // 内側で using した方優先。
            new A().M(); // in Ex2
        }
    }
}
```

```csharp {title="優劣がない場合はコンパイル エラー" error-ranges="29:21-29:22" error-diagnostics="CS0121@29:21-29:22"}
namespace Ex1
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("Extension in Ex1");
    }
}

namespace Ex2
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("Extension in Ex1");
    }
}

namespace App1
{
    using Ex1;
    using Ex2;

    class A;

    class Program
    {
        public static void Main()
        {
            // 優劣がない場合はコンパイル エラー。
            new A().M();
        }
    }
}
```

新しい拡張型でも同様のルールになると思われます。

一方で、旧「拡張メソッド」と新「拡張型」に優劣をつけるかという議題もありますが、
現状は「優劣つけない」という方向で検討されています。
というか、新旧混在した時点でコンパイル エラーにしようかという話もあるみたいです。

```csharp {title="優劣がない場合はコンパイル エラー" error-ranges="30:21-30:22" error-diagnostics="CS0121@30:21-30:22"}
namespace Ex1
{
    static class AExtension
    {
        public static void M(this App1.A _) => Console.WriteLine("old extension method");
    }
}

namespace Ex2
{
    implicit extension AExtension for A
    {
        public void M() => Console.WriteLine("new extension type");
    }
}

namespace App1
{
    using Ex1; // これが外にあってもエラーにする案もあり
    using Ex2;

    class A;

    class Program
    {
        public static void Main()
        {
            // 優劣を付けない(コンパイル エラーになる)。
            // 何なら新旧混在している時点でコンパイル エラーにする可能性濃厚。
            new A().M();
        }
    }
}
```

## インターフェイス実装

ここまでの話は割かし C# 13 で入りそうな話なんですが、
最後に1つ、13では入らなさそうなのがインターフェイス実装の後付けです。

これまでの話どおり、ラッパー構造体を作る方針で少し考えてみましょう。

インターフェイス実装に関する部分だけ残して、以下のようにしたとします。

```csharp {title="拡張型でインターフェイス実装"}
var value = new SomeClass { Value = 1 };
SomeExtension extension = value;

extension.Equals(new SomeClass { Value = 1 });

explicit extension SomeExtension for SomeClass : IEquatable<SomeExtension>
{
    public bool Equals(SomeExtension? other) => Value == other?.Value;
}

class SomeClass
{
    public int Value;
}
```

ラッパー構造体で展開するとしたら以下のようになります。

```csharp {title="ラッパー構造体で展開"}
using System.Runtime.CompilerServices;

var value = new SomeClass { Value = 1 };
ref var extension = ref Unsafe.As<SomeClass, SomeExtension>(ref value);

var temp = new SomeClass { Value = 1 };

// こういう風に直接インターフェイス メンバーを呼ぶ分には特に問題なさげ。
extension.Equals(Unsafe.As<SomeClass, SomeExtension>(ref temp));

struct SomeExtension : IEquatable<SomeExtension>
{
    private SomeClass Value;
    public bool Equals(SomeExtension other) => Value.Value == other.Value?.Value;
}

class SomeClass
{
    public int Value;
}
```

この例はインターフェイス実装しているといっても、そもそもメンバーを直接呼んでいるので問題がないだけです。
問題は以下の状況。

* インターフェイス型や `object` 型の変数で受けてボックス化する場合
* ジェネリック メソッドに渡す場合

まず、インターフェイス型の変数で受けてみましょう。
`ReferenceEquals` や `is` 判定であまり期待通りとは言えない挙動を起こします。

```csharp {title="インターフェイス型の変数で受けてみる"}
using System.Runtime.CompilerServices;

var value = new SomeClass { Value = 1 };
ref var extension = ref Unsafe.As<SomeClass, SomeExtension>(ref value);

// インターフェイスに渡そうとすると、この実装だとボックス化が発生。
IEquatable<SomeExtension> boxedExtension = extension;

// インスタンスが一致しなくなる。
Console.WriteLine(ReferenceEquals(value, boxedExtension)); // false

// ダウンキャストが失敗する。
Console.WriteLine(boxedExtension is SomeClass); // false
```

ジェネリク メソッドでは、以下のように、元の型と拡張型の両方の型情報を使う必要がでてきます。

```csharp {title="ジェネリク メソッドに拡張型を渡す" error-ranges="5:18-5:27,10:33-10:37,10:39-10:44" error-diagnostics="CS0119@5:18-5:27,CS1503@10:33-10:37,CS1503@10:39-10:44"}
var value = new SomeClass { Value = 1 };
List<SomeClass> list = [new() { Value = 2 }, new() { Value = 1 }, new() { Value = 0 }];

// SomeClass のままだと IEquatable 制約を満たさなくて呼べない。
var i1 = IndexOf<SomeClass>>(list, value);

// これなら呼べるようになるはず。
// ただ、list は List<SomeClass> なので、やっぱり型消去が必要。
// 型引数が暗黙的に SomeClass と SomeExtension の2つに増えるような処理が必要。
var i2 = IndexOf<SomeExtension>(list, value);

static int IndexOf<T>(List<T> list, T value)
    where T : IEquatable<T>
{
    // 今の型システムだと T が通常の型か拡張型かを知るすべはなく、Unsafe.As 展開ができない。
    for (int i = 0; i < list.Count; i++)
        if (list[i].Equals(value))
            return i;
    return -1;
}
```

いずれも、C# コンパイラー上のトリックでは問題を解消できなさそうで、
.NET ランタイムの型システムに手を入れる必要が出てきそうです。
型システムに手を入れるとなると結構大ごとなので、C# 13 で実現する見込みは残念ながらほぼありません。
