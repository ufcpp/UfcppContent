---
title: "静的な typeof/sizeof"
source_url: "https://ufcpp.net/blog/2018/12/statictypeof/"
content_type: "BlogEntry"
published_at: "2018-12-24T10:43:48"
updated_at: "2018-12-24T10:43:48"
tags: []
umbraco_id: 2206
parent_id: 2177
sort_order: 23
aliases: []
---

# 静的な typeof/sizeof

[JIT Intrinsics](../jitintrinsics/index.md)で少し触れましたが、
.NET Core 2.1では`Enum.HasFlag`に対する最適化が掛かります。
.NET Core 2.0と2.1で`Enum.HasFlag`の実行速度が1桁違うわけですが、
古いランタイムでも何とかする手段がなくもないです(ただし、`Unsafe`)。

今日はそんな、.NET Core 2.0以前でも使える最適化の話。

## 定数最適化

例えば、以下のようなコードを考えます。

```csharp
static int X()
{
    if (true) return 1;
    else return 0;
}
```

`if` の条件式が定数なので、これは C# のコンパイル時に最適化が掛かって、 `return 1`だけが残ります。`if`相当のコードは出力されません。
このように、コンパイル時に確定している値や条件分岐などは、きれいさっぱり消えることがあります。

## JIT 時定数

中には、C# コンパイル結果としては定数にならないものの、
JIT のタイミングでは定数と判明して、最適化が掛かるものがあります。

ジェネリック型引数に対する`typeof(T)`や`sizeof(T)`はまさにそういう「JIT 時に定数になるもの」です。
例えば以下のようなコードは C# コンパイラーは条件分岐を生成しますが、
 JIT 時の最適化が掛かって、条件式が一致している行だけを残して消えてくれます。

```csharp
static long MaxValue<T>()
{
    if (typeof(T) == typeof(byte)) return byte.MaxValue;
    else if (typeof(T) == typeof(short)) return short.MaxValue;
    else if (typeof(T) == typeof(int)) return int.MaxValue;
    else if (typeof(T) == typeof(long)) return long.MaxValue;
    // お好みで、sbyte, ushort, uint, ulong もどうぞ
    else throw new InvalidOperationException();
}
```

## Enum.HasFlag の代わり

ということで、この手の分岐を書いて、ジェネリックな `HasFlag` を書いてみましょう。

```csharp
using System;
using System.Runtime.CompilerServices;
 
public static class EnumExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool UnsafeHasFlag<T>(T x, T y)
        where T : unmanaged, Enum
    {
        if (Unsafe.SizeOf<T>() == 1) return (Unsafe.As<T, byte>(ref x) & Unsafe.As<T, byte>(ref y)) != 0;
        else if (Unsafe.SizeOf<T>() == 2) return (Unsafe.As<T, ushort>(ref x) & Unsafe.As<T, ushort>(ref y)) != 0;
        else if (Unsafe.SizeOf<T>() == 4) return (Unsafe.As<T, uint>(ref x) & Unsafe.As<T, uint>(ref y)) != 0;
        else if (Unsafe.SizeOf<T>() == 8) return (Unsafe.As<T, ulong>(ref x) & Unsafe.As<T, ulong>(ref y)) != 0;
        else { Throw(); return default; }
    }
 
    private static void Throw() => throw new InvalidOperationException();
}
```

このコードで、非ジェネリックな場合の、

```csharp
static bool HasFlag(A x, A y) => (((int)x) & ((int)y)) != 0;
```

みたいなコードとそこまで差がない性能が出ます。
さすがに最適化をちょっと阻害されるみたいで全く同じとは行きませんが、
少なくとも `Enum.HasFlag` みたいに1桁遅くなることはありません。
せいぜい数割差です。

`Unsafe.SizeOf<T>()` は内部的に `sizeof(T)` を呼んでいるだけです。
単にジェネリック型引数に対して掛けるようにしただけ。
(C# 7.3 移行で unsafe コードであれば、普通にジェネリック型引数に対しても `sizeof(T)` を掛けるようになりました。一方、C# 7.2 以前だと `Unsafe.SizeOf<T>` メソッドが必須です。)

先ほどの説明の通り、`sizeof(T)`はJIT時定数になるので、
この`UnsafeHasFlag`メソッドは、ちゃんと1行だけ残して残りのサイズが違うコードはきれいさっぱり消えます。
この最適化は結構昔から掛かっているものなので、.NET Core 2.0以前でも働きます。
(と言っても、[`Unsafe`クラス](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)が対応している必要があるので、.NET Framework 3.5とかでは動かせません。
`Unsafe.As`相当のILコードを自分で書けば使えますが…)

ちなみに、`Unsafe.As<T, byte>(ref x)` の方は変数の型を無理やり変更するもので、通常の C# ではどうやっても書けません。
メソッドの中身は IL で書かれています。
(また、`Intrinsic`属性が付いているので、おそらく .NET Core 2.1移行ではJITレベルでの最適化も何か掛けていそうです。)
