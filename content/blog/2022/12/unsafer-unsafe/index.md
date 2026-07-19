---
title: "Unsafe クラスの敗北 (関数ポインター)"
source_url: "https://ufcpp.net/blog/2022/12/unsafer-unsafe/"
content_type: "BlogEntry"
published_at: "2022-12-18T22:11:56"
updated_at: "2022-12-18T22:11:56"
tags: []
umbraco_id: 2445
parent_id: 2438
sort_order: 4
aliases: []
---

# Unsafe クラスの敗北 (関数ポインター)

[Gist](https://gist.github.com/ufcpp) に書き捨ててたコードの供養ブログ シリーズ、
今日のは特に人を選ぶやつ。

今日は C# 9 で入った [関数ポインター](../../../../study/csharp/cheatsheet/ap_ver9.md#function-pointer) がらみの話です。

## <a id="unsafe-class">Unsafe クラス</a>

C# の unsafe 機能、例えばポインターとかは、なかなか制限がきついです。
そのため、「実は .NET の型システム的にはできる」というものでも、
C# で書くことはできないことが結構あります。

それに対して、
.NET Core 以降、
[`Unsafe`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.unsafe?view=net-7.0) (`System.Runtime.CompilerServices` 名前空間)とかいう名前からして unsafe なクラスがあって、
内部的に IL を使ったり、
runtime intrinsics (JIT コンパイラーの特別扱い)で実装したりして、
元々 C# では書けなかったようなコードを、普通の C# で書けるようにしました。

この `Unsafe` クラスは、
[unsafe コンテキスト](../../../../study/csharp/interop/sp_unsafe.md)なしで、
普通の unsafe コードよりもよっぽど unsafe なことができちゃうという意味で良くも悪くも凶悪です。

ということで、皆様ご存じの通り<sup>[※](#as-you-all-known)</sup>、
`Unsafe` クラスを使えば C# でも C++ 的な遊びがいろいろと楽しめます。

```csharp
using System.Runtime.CompilerServices;

var a = new A(123);

// readonly struct なので、↓はエラー。
//a.Value = 999;

// Unsafe.As を使えば、
// C++ でいう reinterpret_cast 的に何でもかんでも変換可能。
// (メモリレイアウトが想定通りかは利用者の自己責任。)
ref var x = ref Unsafe.As<A, int>(ref a);
x = 999;

// a.Value が 999 に書き変わってる。
// A { Value = 999 }
Console.WriteLine(a);

readonly record struct A(int Value);
```

<sup><a id="as-you-all-known">※</a></sup>どの方面に向かって「皆」と言っているのかは不明。

`Unsafe` クラスを使ったこの手の処理、
誤用すると盛大にクラッシュさせれるくらい安全性皆無になるので利用には注意が必要ですが、
パフォーマンス改善につながることが多くて、
一部界隈では結構多用されます。

## <a id="ref-struct">ref struct の制限</a>

ところが、`Unsafe` クラスでもできないことがありまして。
というか、`Unsafe` クラスはおろか、現状では[ポインター](../../../../study/csharp/interop/sp_unsafe.md#pointer)を使っても解決できないものがありまして。

というのも、[ref struct](../../../../study/csharp/resource/refstruct.md)はジェネリック型引数にもできないし、ポインターにもできません。

なので、先ほどと同じノリで ref struct に対して `Unsafe.As` (とか、それ相当の unsafe コード)を書こうとしてもうまくいきません。

```csharp
using System.Runtime.CompilerServices;

var span = (stackalloc int[] { 0xDE, 0xAD, 0xBE, 0xEF });
var a = new A(span);

var spanFromA = Unsafe.As<A, Span<int>>(ref a);

ref struct A
{
    // private なので通常、この _span を取り出す方法ない。
    // なんならリフレクションを使っても無理。
    private Span<int> _span;
    public A(Span<int> span) => _span = span; 
}
```

## <a id="function-pointer">関数ポインター</a>

そんな時には[関数ポインター](../../../../study/csharp/cheatsheet/ap_ver9.md#function-pointer)を使えばいいらしいですよ。

C# 9 で入った関数ポインター、
`delegate*<T1, T2, ...>` みたいな、ジェネリクスに似た記法を使う割に、
この `T1` とか `T2` のところには `ref` も書けるし ref struct も書けるしで、相当自由みたいです。

要するに、`Span<T>` (ref struct)に対して、
`Span<T>*` (直接その型のポインター)は書けないし、
`Unsafe.As<A, Span<T>>` (型引数)も書けませんが、
`delegate*<ref A, ref Span<T>>` (関数ポインターの引数)なら書けます。

これを使えば、以下のように、ref struct に対しても `Unsafe.As` 的なことができるようになったりします。

```csharp
var span = (stackalloc int[] { 0xDE, 0xAD, 0xBE, 0xEF });
var a = new A(span);

unsafe
{
    // function pointer の引数なら ref RefStruct も行ける。
    var f = (delegate*<ref A, ref Span<int>>)(delegate*<nint, nint>)&id;

    // 晴れて A の中から _span を抜き出し。
    var spanFromA = f(ref a);

    // span と同じ内容。
    foreach (var x in spanFromA) Console.Write($"{x:X2}");
    Console.WriteLine();

    // span が書き変わる。
    spanFromA[0] = 1;
    spanFromA[1] = 2;
    spanFromA[2] = 3;
    spanFromA[3] = 4;
}

// 上書きされた 01020304。
foreach (var x in span) Console.Write($"{x:X2}");
Console.WriteLine();

// nint 素通しメソッド。
// nint = unsafe コンテキスト内なら任意のポインター、任意の ref T を通せる。
static nint id(nint x) => x;

ref struct A
{
    // private なので通常、この _span を取り出す方法ない。
    // なんならリフレクションを使っても無理。
    private Span<int> _span;
    public A(Span<int> span) => _span = span;
}
```

native interop でしか使い道がないと思っていた関数ポインター、
こんなところで「これでしかできないこと」があるとは…
