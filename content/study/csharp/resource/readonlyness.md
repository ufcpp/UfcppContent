---
title: "readonly の注意点"
source_url: "https://ufcpp.net/study/csharp/resource/readonlyness/"
content_type: "Article"
published_at: "2017-11-04T00:00:00"
updated_at: "2023-04-01T20:34:20"
tags: []
umbraco_id: 2095
parent_id: 1286
sort_order: 3
aliases: []
---

# readonly の注意点

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[定数](../start/sp_const.md#readonly)」で、読み取り専用のフィールドが作れるという話をしました。
この時点ではまだ[クラス](../oop/oo_class.md)や[構造体](rm_struct.md)、[値型と参照型の違い](oo_reference.md)などについて触れていなかったので`readonly`修飾子の簡単な紹介だけに留めましたが、
本項で改めて`readonly`について説明します。

整数などの基本的な型に対して使う分には特に問題は起きないんですが、構造体やクラスなど、複合型に対して使うときには注意が必要です。

## <a id="sec-generated-title-2"></a> <a id="class-readonly"></a>参照型のフィールドに対して readonly

`readonly`に関して最も注意が必要な点は、`readonly`は再帰的には働かないという点です。
`readonly`を付けたその場所だけが読み取り専用になり、参照先などについては書き換えが可能です。

例えば以下のコードを見てください。`Program`クラスのフィールド`c`には`readonly`が付いていますが、
`c`が普通に書き換え可能なクラスのフィールドなので、クラスの中身は自由に書き換えられます。

```csharp {title="参照型のフィールドに対してreadonlyを付ける例" error-ranges="21:9-21:10"}
// 書き換え可能なクラス
class MutableClass
{
    // フィールドを直接公開
    public int X;

    // 書き換え可能なプロパティ
    public int Y { get; set; }

    // フィールドの値を書き換えるメソッド
    public void M(int value) => X = value;
}

class Program
{
    static readonly MutableClass c = new MutableClass();

    static void Main()
    {
        // これは許されない。c は readonly なので、c 自体の書き換えはできない
        c = new MutableClass();

        // けども、c の中身までは保証してない
        // 書き換え放題
        c.X = 1;
        c.Y = 2;
        c.M(3);
    }
}
```

![参照型のフィールドに対してreadonlyを付ける例](../../../../assets/media/1145/mutableclass.png)

クラスを書き換えできないように作る場合、クラス自体を書き換え不能に作りましょう。
(クラスの方で、フィールドを`readonly`にしたり、プロパティを[get-only](../oop/oo_property.md#get-only)にします。)

## <a id="sec-generated-title-3"></a> <a id="struct-readonly"></a>値型のフィールドに対して readonly

クラス(参照型)とは対照的に、構造体(値型)の場合はデータを直接持ちます。
そのため、構造体のフィールドに対して`readonly`を付けると、構造体の中身も読み取り専用になります。
ただし、メソッドの呼び出しなどを行う際、コピーが発生するという別の注意が必要です。

例えば以下のように、`readonly`が付いたフィールド`c`自体に加えて、`c`のフィールドも書き換えできません。

```csharp {title="値型のフィールドに対してreadonlyを付ける例" error-ranges="22:9-22:10,25:9-25:12"}
using System;

// 書き換え可能な構造体
struct MutableStruct
{
    // フィールドを直接公開
    public int X;

    // フィールドの値を書き換えるメソッド
    public void M(int value) => X = value;
}

class Program
{
    static readonly  MutableStruct c = new  MutableStruct();

    static void Main() => Allowed();

    private static void NotAllowed()
    {
        // これはもちろん許されない。c は readonly なので、c 自体の書き換えはできない
        c = new  MutableStruct();

        // 構造体の場合、フィールドに関しては readonly な性質を引き継ぐ
        c.X = 1;
    }

    private static void Allowed()
    {
        // でも、メソッドは呼べてしまう
        c.M(3); // X を 3 で上書きしているはず？

        Console.WriteLine(c.X); // でも、X は 0 のまま

        //↑のコードは、実はコピーが発生している
        // 以下のコードと同じ意味になる

        var local = c;
        local.M(3);

        Console.WriteLine(c.X); // 書き換わってるのは local (コピー)の方なので、c は書き換わらない(0)

        Console.WriteLine(local.X); // もちろんこっちは書き換わってる(3)
    }
}
```

![値型のフィールドに対してreadonlyを付ける例](../../../../assets/media/1146/mutablestruct.png)

この例の後半を見ての通り、メソッドは呼べてしまいます。
フィールド`X`は書き換えれないはずなのに、その`X`を書き換えているメソッド`M`を呼んでもエラーになりません。
C# では、こういう場合に、`readonly`であることを保証しつつメソッドを呼び出せるように、フィールドを一度コピーしてから、そのコピーに対してメソッドを呼ぶということをしています。

このコピーは、万が一に備えて防衛的にコピー(defensive copy)するものです。
実際にコピーが必要かどうか(実際にメソッド内で書き換えをしているかどうか)に関わらず、常にコピーが発生します。
ソースコード上は目に見えないコピーなので、<strong id="hidden-copy" class="keyword">隠れたコピー</strong>(hidden copy)と呼ばれたりもします。

すなわち、コピーが発生してまずいような場合(例えば構造体のサイズが大きくてコピーにコストが掛かるとか)には、`readonly`なフィールドを使うことで問題が発生することがあります。
この問題は、[`in`引数](sp_ref.md#in)などでも発生しまえます。
後述する[`readonly struct`](#readonly-struct)や[readonly 関数メンバー](#readonly-member)を使えばこの問題は少し緩和するので、そちらも参照してください。

## <a id="sec-generated-title-4"></a> <a id="this-rewrite"></a>構造体の this 書き換え

C# の`readonly`フィールドには少し片手落ちなところがあって、実は、構造体の場合にちょっとした問題を起こせたりします。

構造体のメソッドの中では`this`が「自分自身の参照」の意味なんですが、この`this`参照は書き換えできてしまいます。
そのため、以下のように、`readonly`で一見書き換えができなさそうなフィールドを書き換えてしまうことができます。

```csharp {title="構造体の this 書き換えの例"}
using System;

struct Point
{
    // フィールドに readonly を付けているものの…
    public readonly int X;
    public readonly int Y;

    public Point(int x, int y) => (X, Y) = (x, y);

    // this の書き換えができてしまうので、実は X, Y の書き換えが可能
    public void Set(int x, int y)
    {
        // X = x; Y = y; とは書けない
        // でも、this 自体は書き換えられる
        this = new Point(x, y);
    }
}

class Program
{
    static void Main()
    {
        var p = new Point(1, 2);

        // p.X = 0; とは書けない。これはちゃんとコンパイル エラーになる

        // でも、このメソッドは呼べるし、X, Y が書き換わる
        p.Set(3, 4);

        Console.WriteLine(p.X); // 3
        Console.WriteLine(p.Y); // 4
    }
}
```

わざわざこんな紛らわしいことをしようとは思わないのでめったに問題になることはないんですが、一応は注意が必要です。
また、この問題は、次節で説明する通り、C# 7.2で少し緩和されます。

## <a id="sec-generated-title-5"></a> <a id="readonly-struct"></a>readonly struct

<h5 class="version version7">Ver. 7.2</h5>

C# 7.2で、構造体自体に`readonly`修飾を付けられるようになりました。
`readonly`を付けた構造体は以下のような状態になります。

- 全てのフィールドに対して `readonly` を付けなければならなくなる
  - [get-onlyプロパティ](../oop/oo_property.md#get-only)は使えます(自動生成されるフィールドが`readonly`なので問題ない)
- `this`参照も`readonly`扱いされる

`this`が`readonly`扱いになるので、前節のような`this`書き換えの問題は起きません。

```csharp {title="readonly struct の例" highlight-ranges="4:1-4:9"}
using System;

// 構造体自体に readonly を付ける
readonly struct Point
{
    // フィールドには readonly が必須
    public readonly int X;
    public readonly int Y;

    public Point(int x, int y) => (X, Y) = (x, y);

    // readonly を付けない場合と違って、以下のような this 書き換えも不可
    //public void Set(int x, int y) => this = new Point(x, y);
}

class Program
{
    static void Main()
    {
        var p = new Point(1, 2);

        // p.X = 0; とは書けない。これはちゃんとコンパイル エラーになる
        // p.Set(3, 4); みたいなのもダメ

        Console.WriteLine(p.X); // 1 しかありえない
        Console.WriteLine(p.Y); // 2 しかありえない
    }
}
```

### <a id="sec-generated-title-6"></a> <a id="avoid-copy"></a>readonly struct によるコピー回避

[前述](#struct-readonly)の通り、(無印の)構造体の`readonly`フィールドに対してメソッドを呼ぶと防衛的コピーが発生するという問題があります。
これに対して、`readonly struct`であれば、このコピーを回避できます。

例えば以下のように、ほぼ同じ構造・どちらも書き換え不能な構造体を作ったとして、`readonly struct`になっているかどうかでコピー発生の有無が変わります。

```csharp
using System;

// 作りとしては readonly を意図しているので、何も書き換えしない
// でも、struct 自体には readonly が付いていない
struct NoReadOnly
{
    public readonly int X;
    public void M() { }
}

// NoReadOnly と作りは同じ
// ちゃんと readonly struct
readonly struct ReadOnly
{
    public readonly int X;
    public void M() { }
}

class Program
{
    static readonly NoReadOnly nro;
    static readonly ReadOnly ro;

    static void Main()
    {
        // readonly を付けなかった場合
        // フィールド参照(読み取り)は問題ない
        Console.WriteLine(nro.X);

        // メソッド呼び出しが問題。ここでコピー発生
        // (呼び出し側では、「M の中で特に何も書き換えていない」というのを知るすべがないので、防衛的にコピーが発生)
        nro.M();

        // readonly を付けた場合
        // これなら、M をそのまま呼んでも何も書き換わらない保証があるので、コピーは起きない
        ro.M();
    }

    // これも問題あり(コピー発生)
    // in を付けたので readonly 扱い → M を呼ぶ際にコピー発生
    static void F(in NoReadOnly x) => x.M();

    // こちらも、readonly struct であれば問題なし(コピー回避)
    static void F(in ReadOnly x) => x.M();
}
```

C# 7.2 以降では、書き換えを意図していない構造体に対しては`readonly`修飾を付けるのが無難でしょう。

また、「フィールド直接参照なら大丈夫だけど、メソッドを(プロパティも)呼ぶとコピー発生」という性質上、
書き換えを最初から意図している構造体の場合は、プロパティよりも、フィールドを直接`public`にしてしまう方が都合がいいことがあります。

## <a id="sec-generated-title-7"></a> <a id="ref-readonly"></a>readonly参照と不変性

[`in`引数](sp_ref.md#in)や[`ref readonly`](sp_ref.md#ref-readonly)で、読み取り専用の参照を作れます。
この読み取り専用参照は、「そのメソッド内で書き換えない」、「その引数・変数を通した書き換えをしない」という意思表明としては非常に有用です。
その一方で、「外で書き換わる」、「参照元の値が書き換わる」という意味で、不変性(immutability)の保証はありません。

例えば以下の例を見てください。

```csharp {title="in/ref readonly で保証できる範囲"}
using System;

class Program
{
    static void Main()
    {
        _value = 0;
        ByVal(_value); // 0, 0

        _value = 0;
        ByRef(_value); // 0, 1
    }

    // 書き換えできるフィールド
    static int _value;

    // 値渡し = コピー なので、 _value 書き換えの影響は受けない
    static void ByVal(int value)
    {
        Console.WriteLine(value);
        _value++;
        Console.WriteLine(value);
    }

    // 参照渡しなので、 _value 書き換えの影響を受ける
    // in (ref readonly) であっても、immutable ではない
    // value を通して書き換えない保証があるだけで、別経路で書き換わることに対しては無力
    static void ByRef(in int value)
    {
        Console.WriteLine(value);
        _value++;
        Console.WriteLine(value);
    }
}
```

メソッドの中身としては全く同じメソッドが2つありますが、片方(`ByVal`)は値渡しで、もう片方(`ByRef`)は `in` 引数で整数値を受け取っています。
`ByVal`では、`value`は値のコピーを受け取っているので、元の値の出どころとは無縁になっています。
一方、`ByRef`の方では`value`自身は`in`が付いていて書き換えられませんが、その参照元になっている`_value` の方が書き換わると、`value`の値も一緒に変化します。
書き換え不能(readonly)だからと言って、値の不変性(immutable)の保証はなく、こうして値が変化する場合があります。

## <a id="sec-generated-title-8"></a> <a id="readonly-member"></a>readonly 関数メンバー

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、[関数メンバー](../structured/st_function.md#sec-function-member)単位で「フィールドを書き換えてない」ということを保証できるようになりました。
構造体全体を `readonly struct` にしなくても、[隠れたコピー](#hidden-copy)問題を避けられる機会が増えます。

以下のように、関数メンバーに `readonly` 修飾を付けます。

```csharp {title="readonly 関数メンバーの例" highlight-ranges="19:12-19:20"}
// 構造体自体は readonly にしない。
// フィールドは書き換えたい
struct NonReadOnly
{
    public float X;
    public float Y;
 
    // でも、このプロパティ内ではフィールドを書き換えない
    public float LengthSquared => X * X + Y * Y;
}
 
// NonReadOnly との差は LengthSquared の readonly の有無だけ
struct ReadOnly
{
    public float X;
    public float Y;
 
    // readonly 修飾でフィールドを書き換えないことを明示
    public readonly float LengthSquared => X * X + Y * Y;
}
 
class Program
{
    // こっちは、LengthSquared 内での X, Y の書き換えを恐れて隠れたコピーが発生する。
    static float M(in NonReadOnly x) => x.LengthSquared;
 
    // こっちは、LengthSquared に readonly が付いているのでコピー発生しない。
    static float M(in ReadOnly x) => x.LengthSquared;
 
    static void Main(string[] args)
    {
        M(new NonReadOnly { X = 1, Y = 2 });
        M(new ReadOnly { X = 1, Y = 2 });
    }
}
```

隠れたコピー問題はソースコードの見た目に現れず、気づきにくい問題なので、
関数内でフィールドを書き換えていないなら積極的に `readonly` 修飾を付けておくべきでしょう。

ちなみに、逆に、`readonly` 関数メンバー内から、`readonly` ではないものを触ろうとしても隠れたコピーが発生します。
例えば以下のコードでは、`A`のフィールドを書き換える`Increment`メソッドを、
`readonly` なメソッドとそうでないメソッドから呼び出してみています。

```csharp {title="readonly 関数メンバーから、非 readonly な構造体フィールドに触る"}
using System;
 
struct A
{
    public int Value;
    public void Increment() => Value++;
}
 
struct B
{
    public A A;
 
    // A の非 readonly メンバーを呼ぶ。
    public void Mutable() => A.Increment();
 
    // Mutable との差は readonly 修飾が付いてるだけ。
    // this が書き換わらないように、A のコピーが作られる。A 自体には変化が起きない。
    public readonly void Immutable() => A.Increment();
}
 
class Program
{
    static void Main()
    {
        var b = new B();
        Console.WriteLine(b.A.Value); // 初期状態: 0
 
        b.Mutable();
        Console.WriteLine(b.A.Value); // 意図通りの書き換え: 1
 
        b.Immutable();
        Console.WriteLine(b.A.Value); // 書き換わらない: 1 (Immutable の中で A のコピーが発生)
    }
}
```

### <a id="sec-generated-title-9"></a> <a id="similar-but-different"></a>注意: 似て非なるもの(ref readonly)

この `readonly` 関数メンバーは、構文上、[`ref readonly`](sp_ref.md#ref-readonly)と似ているのでちょっと注意が必要かもしれません。

```csharp {title="readonly ref との兼ね合い"}
struct S
{
    public int[] _value;
 
    // これは、読み取り専用参照を返すという意味。
    // _value 配列の中身が書き換わってもらっては困る。
    public ref readonly int X => ref _value[0];
 
    // これは、S 内のフィールド(この場合 _value) を書き換えないという意味。
    // _value 配列の中身が書き換わろうと知ったことではない。
    public readonly ref int Y => ref _value[0];
 
    // これは、上記2つの両方の意味。
    // _value 自体も書き換わらないし、_value の中身を書き換えてもらっても困るとき用。
    public readonly ref readonly int Z => ref _value[0];
}
```

ちなみに、プロパティの場合は `get`/`set` それぞれ別に `readonly` 指定ができます。
当然ですが、ほとんどの場合は「`get` だけが `readonly`」になると思われます。

```csharp {title="プロパティの get にだけ readonly 修飾"}
struct X
{
    int _value;
 
    public int Value
    {
        readonly get => _value;
        set => _value = value;
    }
}
```
