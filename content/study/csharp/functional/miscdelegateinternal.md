---
title: "[雑記] デリゲートの内部"
source_url: "https://ufcpp.net/study/csharp/functional/miscdelegateinternal/"
content_type: "Article"
published_at: "2017-12-03T17:17:26"
updated_at: "2017-12-03T17:17:26"
tags: []
umbraco_id: 2111
parent_id: 1275
sort_order: 3
aliases: []
---

# \[雑記\] デリゲートの内部

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

[デリゲート](sp_delegate.md)は、内部実装的には「インスタンスと関数ポインターをペアで管理しているクラス」になっています。

ここではデリゲートの内部挙動と、
それを踏まえたパフォーマンス上の注意点を説明します。

## <a id="sec-generated-title-2"></a> <a id="delegate-internal"></a>デリゲートの内部

デリゲートは .NET ランタイム内で特殊な扱いをされていて、
デリゲート内部で起こっていることをそのまま C# で書くことはできないので、
ここでの説明は疑似コード的なものになります。

### <a id="sec-generated-title-3"></a> <a id="delegate-type"></a>型定義

例えば、以下のようなデリゲートがあったとします。

```csharp {title="例として使うデリゲート"}
delegate int F(int x);
```

これは内部的には以下のような扱いになっています。
概ね、インスタンスと関数ポインターのペアです。

```csharp {title="デリゲートの内部的な扱い"}
class F : System.Delegate
{
    object Target;
    IntPtr FunctionPointer;
    // 実際には Delegate クラスのメンバー
    // あと、object がもう1個と、IntPtr がもう1個ある

    public F(object target, IntPtr fp) => (Target, FunctionPointer) = (target, fp);

    public virtual int Invoke(int x)
    {
        // return FunctionPointer(Target, x); 的な処理
    }
}
```

実際にはこの他に2つのフィールドがあると書いていますが、
1つは[マルチキャスト](sp_delegate.md#malticast)用、
もう1つは[後述](#static-method)する静的メソッドのために使うフィールドです。

### <a id="sec-generated-title-4"></a> <a id="new-delegate"></a>デリゲートのインスタンス生成

C# では(C# 2.0 以降)、以下のように、デリゲート型の変数に対してメソッドを直接渡すような形でデリゲートを作ります。

```csharp {title="デリゲートの作り方(C# 2.0 以降)"}
// インスタンス メソッドから生成
var x = new Sample();
F i = x.Instance;

// 静的メソッドから生成
F s = Sample.Static;
```

これは省略形で、省略せずに書くなら以下のように、デリゲート型のインスタンスを`new`します
(C# 1.0 時代はこの書き方しかできない)。
```csharp
// インスタンス メソッドから生成
var x = new Sample();
F i = new F(x.Instance);

// 静的メソッドから生成
F s = new F(Sample.Static);
```

ここで、先ほど説明した通り、デリゲート`F`のコンストラクターは内部的には`F(object, IntPtr)`という形になっています。
そして、上記のコードは、実際にはこのコンストラクターを呼ぶように展開されます。

まずインスタンス メソッドの方は以下のような処理に展開されます。

- インスタンス`x` を読み込む
- メソッド `Instance` の関数ポインターを読み込む([IL](../../il/index.md) にはそのための `ldftn` という命令がある。)
- `F`のコンストラクター`F(object, IntPtr)`を呼び出す

静的メソッドの場合にも同じコンストラクターを呼びます。
`object target`には null が渡ります。
すなわち、以下のような処理に展開されます。

- nullを読み込む。
- メソッド `Static` の関数ポインターを読み込む
- `F`のコンストラクター`F(object, IntPtr)`を呼び出す

ただし、JIT 時の最適化でコンストラクター呼び出しの部分が書き換えられて、
最終的にはインスタンス メソッド・静的メソッドそれぞれ専用の別処理が呼ばれるようです。
静的メソッドの場合には、後述する「ちょっとしたトリック」のための追加の処理が掛かります。

### <a id="sec-generated-title-5"></a> <a id="invoke-delegate"></a>呼び出し側(Invokeの中身)

デリゲートの呼び出しは以下のように書きます。

```csharp {title="デリゲートの呼び出し"}
i(10);
s(20);
```

これも省略形みたいもので、省略せずに書くと`Invoke`メソッドの呼び出しになっています。

```csharp {title="デリゲートの呼び出し(Invoke を明示的に呼ぶ)"}
i.Invoke(10);
s.Invoke(20);
```

ただし、JIT 時の最適化で`Invoke`メソッド呼び出しの部分が書き換えられて、
最終的には以下のような処理が残ります。

- デリゲートの `Target` フィールドを読み込む
- 引数の`int` (上記の例の 10 や 20)を読み込む
- デリゲートの `FunctionPointer` に格納してあるアドレスにジャンプ

## <a id="sec-generated-title-6"></a> <a id="static-method"></a>静的メソッドを渡すと遅い

インスタンス メソッドと静的メソッドは、内部的には実のところだいぶ異なる引数の受け取り方をしています。
インスタンス メソッドは、以下のように、静的メソッドよりも暗黙的に1引数多く受け取っています。

```csharp {title="インスタンス メソッドと静的メソッドの引数の受け取り方"}
class Sample
{
    static void StaticMethod(int x)
    {
        // 静的メソッドの場合は正真正銘、引数は x の1つだけ
    }

    void InstanceMethod(int x)
    {
        // 引数が1つだけに見えて…

        // 実は暗黙的に this を受け取っている
        Console.WriteLine(this);
    }

    // ということで ↑の InstanceMethod は、以下のような静的メソッドと同じ引数の受け取り方をしてる
    static void InstanceLikeMethod(Sample @this, int x)
    {
        Console.WriteLine(@this);
    }
}
```

このことを踏まえた上で、
前節の最後で説明したデリゲート呼び出しの手順を改めてみてみます。

1. デリゲートの `Target` フィールド(静的メソッドの時には null が入っている)を読み込む
1. 引数の`int` を読み込む
1. デリゲートの `FunctionPointer` に格納してあるアドレスにジャンプ

デリゲートはインスタンス メソッドを参照していることもあれば、
静的メソッドを参照していることもあります。
しかし、呼び出し側では(インスタンス/静的によらず)常にこの手順で引数を渡しています。
すなわち、インスタンス メソッドの場合には素直に呼び出せるんですが、
静的メソッドの場合には内部的にちょっとしたトリックが働いています。

デリゲートに対して静的メソッドを渡すと、`FunctionPointer`には以下のような処理をする別のメソッドが入ります。

- 1. で読み込んだインスタンスを無視して、引数の `int` だけを並べ直す
- 改めて、本来の静的メソッドにジャンプする

この処理は意外と負担が高くて、デリゲートに対して静的メソッドを渡した場合、その呼び出しはかなり遅いです
(参考: [計測コード](https://gist.github.com/ufcpp/b2e64d8e0165746effbd98b8aa955f7e))。

要するに、デリゲートはインスタンス メソッドの時に処理が単純で高速になるように作られていて、
その代わりに静的メソッドが低速です。
C# ではインスタンス メソッドの方が圧倒的に利用頻度が高いので、
インスタンス メソッドに対して最適化した方が、全体としてのパフォーマンスは上がります。

## <a id="sec-generated-title-7"></a> <a id="curried-delegate"></a>カリー化デリゲート

前節の「静的メソッドに対するトリック」を回避して、
デリゲート越しの静的メソッドの呼び出しを速くする方法が1つあります。
「[拡張メソッドのデリゲートへの代入](sp3_extension.md#delegate)」で説明しているカリー化デリゲートという手段を使うと、インスタンス メソッドと同じコストで静的メソッドを呼べます。

拡張メソッドは、実体としては以下のように、第1引数でインスタンスを受け取る構造になっていて、
これがインスタンス メソッドの暗黙的な `this` 引数と同じ受け取り方になります。

```csharp {title="インスタンス メソッドと拡張メソッドの引数の受け取り方"}
class Sample
{
    public void InstanceMethod(int x)
    {
        // 引数が1つだけに見えて、実は暗黙的に this を受け取っている
    }

    // ということで ↑の InstanceMethod は、以下のような静的メソッドと同じ引数の受け取り方をしてる
    static void InstanceLikeMethod(Sample @this, int x)
    {
    }
}

static class SampleExtensions
{
    // であれば、こういう拡張メソッドも InstanceMethod と同じ引数の受け取り方になる
    public static void ExtensionMethod(this Sample @this, int x)
    {
    }
}
```

そこで、C# では、以下のように拡張メソッドに対して、インスタンス メソッドと同じようなデリゲートの作り方を認めています
(`x.E` のような書き方を、カリー化デリゲートと呼びます)。

```csharp {title="拡張メソッドからデリゲートを作る"}
var x = new Sample();

Action<int> i = x.InstanceMethod;

// 拡張メソッドに対して、インスタンス メソッドと同じようなデリゲートの作り方を認めてる
Action<int> e = x.ExtensionMethod;
```

`i`の方も`e`の方のどちらも、以下のように扱われます。

- インスタンス`x` を読み込む
- メソッド `InstanceMethod`/`ExtensionMethod` の関数ポインターを読み込む
- `Action<int>`のコンストラクター`Action<int>(object, IntPtr)`を呼び出す

通常の静的メソッドの場合と違って前述のトリックのための別処理への分岐も掛からず、
内部的にも完全に同じ処理になります。
呼び出しの際にもインスタンス メソッドと同じ処理になるため、
カリー化デリゲートは呼び出しは高速になっています。

### <a id="sec-generated-title-8"></a> <a id="optimization-static"></a>(最適化手法1) 普通の静的メソッドを拡張メソッドに置き換え

ちなみに、こういう内部挙動の結果、
以下のように、静的メソッドに対してダミー引数を1つ増やしてわざわざ拡張メソッド化する高速化手法が使えたりします。

```csharp {title="カリー化デリゲートにすることで静的メソッドのデリゲートを高速化する例"}
using System;

static class Program
{
    // 普通の静的メソッド
    static int F(int x) => 2 * x;

    // わざわざ使いもしない第1引数を増やして、拡張メソッドに変更
    static int F(this object dummy, int x) => 2 * x;

    static void Main()
    {
        // 静的メソッドからデリゲート作成
        Func<int, int> s = F;

        // わざわざ null を使ってカリー化デリゲートにする
        Func<int, int> e = default(object).F;

        // 以下の2つの呼び出しでは、e (カリー化デリゲート)の方が圧倒的に高速
        s(10);
        e(10);
    }
}
```

### <a id="sec-generated-title-9"></a> <a id="optimization-static"></a>(最適化手法2) 匿名関数を拡張メソッドに置き換え

ちょっとした変換処理などに対して、匿名関数を使うよりも拡張メソッドを挟んだ方が速くなることもあります。

単純な例として、あるインスタンスを返すだけのラムダ式を、
拡張メソッドに置き換えることで高速化してみましょう。
以下のように書けます。

```csharp {title="拡張メソッドを介することでちょっと高速化する例"}
using System;

class Program
{
    // Func 越しに何かのインスタンスを取りたい
    static void M(Func<string> factory)
    {
        Console.WriteLine(factory());
    }

    static void Main()
    {
        // でも、呼ぶ側としては単に何かインスタンスを1個渡したいだけ
        string s = Console.ReadLine();

        // そこで、ラムダ式で1段覆って、string から Func<string> を作る
        // これだと、匿名関数の仕様から、匿名のクラスが作られて、その new のコストが余計にかかる
        M(() => s);

        // 一方で、以下のように、拡張メソッドを介することで、カリー化デリゲート(速い)になる
        M(s.Identity);
    }
}

static class TrickyExtension
{
    // 素通しするだけの拡張メソッドを用意
    public static T Identity<T>(this T x) => x;
}
```

この例の「素通し」よりもう少し複雑な場合でも同様です。
いくつか例を挙げると、以下のような場合にも同様の手法が使えます。

- [文字列から`Length`を抜き出す`Func<string, int>`を作る](https://gist.github.com/ufcpp/c6bca9e382c579b25f618bf9befbefae#file-stringgetlength-cs)
- [`Func<int>`から`Func<long>`を作る](https://gist.github.com/ufcpp/c6bca9e382c579b25f618bf9befbefae#file-makeprimitivecovariant-cs)
- [`Action<long>`から`Action<int>`を作る](https://gist.github.com/ufcpp/c6bca9e382c579b25f618bf9befbefae#file-makeprimitivecontravariant-cs)
- [自作の型でも、引数の型違いの`Action`の変換](https://gist.github.com/ufcpp/9e476a8a4bfaf04e3e1256bedfd83881)

匿名関数(特にラムダ式)と比べるとはるかに手間がかかる書き方なので使い勝手はかなり悪いですが、
よっぽど「速度最優先」な場合には有効です。
