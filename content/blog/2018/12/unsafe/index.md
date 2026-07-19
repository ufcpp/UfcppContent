---
title: "System.Runtime.CompilerServices.Unsafe"
source_url: "https://ufcpp.net/blog/2018/12/unsafe/"
content_type: "BlogEntry"
published_at: "2018-12-14T09:35:46"
updated_at: "2018-12-14T09:35:46"
tags: []
umbraco_id: 2195
parent_id: 2177
sort_order: 13
aliases: []
---

# System.Runtime.CompilerServices.Unsafe

昨日から始まった在庫一掃処分セール的なブログなんですが、結構な頻度で「`Unsafe` クラス」ってのが出てきます。

以下のパッケージに含まれているもので、こいつをを参照すれば、通常の C# では書けないようなどぎつい unsafe な真似がし放題になります。

- [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)

これの登場はもう結構前なんですけども、そういえばちゃんとした説明をしたことなかったなと。

## .NET の IL は意外とやりたい放題

上記パッケージにはパッケージ名と同じ`Unsafe`というクラスが入っています。
この`Unsafe`クラス、ソースコードはこんな感じ:

- [System.Runtime.CompilerServices.Unsafe.il](https://github.com/dotnet/corefx/blob/64c6d9fe5409be14bdc3609d73ffb3fea1f35797/src/System.Runtime.CompilerServices.Unsafe/src/System.Runtime.CompilerServices.Unsafe.il)

ILアセンブリ実装です。

C# では書けなくても、IL なら何も特別なことをしなくてもやりたい放題。
要するに、.NET における「安全」は、結構 C# のレベルで保証しています。

とはいえ、unsafe でもいいので、C# でできないのは困るということで提供されるようになったのがこの`Unsafe`クラスです。
C# の文法を拡張するよりは、こういう IL 実装なクラスを提供する方が手っ取り早かったのでこんなことになりました。

## ポインターの方がまだマシ疑惑

とはいえ、この`Unsafe`クラスをフル活用すると、こんなコードになります。

```csharp
using System.Runtime.CompilerServices;
 
class Program
{
    static int UnsafeClass(int[] array)
    {
        var sum = 0;
        ref var begin = ref array[0];
        ref var p = ref Unsafe.As<int, byte>(ref begin);
        var length = array.Length * 4;
        for (int i = 0; i < length; i++, p = ref Unsafe.Add(ref p, 1))
            sum += p;
        return sum;
    }
}
```

ちなみに、普通に C# で unsafe コードを使って同じものを書くと以下のようになります。

```csharp
unsafe static int UnsafeContext(int[] array)
{
    var sum = 0;
    fixed (int* begin = &array[0])
    {
        var p = (byte*)begin;
        var length = array.Length * 4;
        for (int i = 0; i < length; i++, p++)
            sum += *p;
    }
    return sum;
}
```

見た目に関しては、ポインターを使った後者の方がまだマシなんじゃないでしょうか。

だったら素直に unsafe コードを使う方がいいんじゃないかという話になるとは思いますが、
いくつか、`Unsafe`クラスでしかできないことがあります。

- ポインターの代わりに [`ref`](../../../../study/csharp/resource/sp_ref.md) で操作できる
- ジェネリックな型をポインター化できる

### ポンターの代わりに ref

ポインターと `ref` は内部的には似たようなものです。
大体同じ命令を使って間接参照します。
ですが、
1つ決定的に違うのが、`ref`なら[ガベコレ](../../../../study/csharp/resource/rm_gc.md#garbage-collection)が追えるという点があります。

```csharp
// ref 戻り値ならこんなコードを書いても平気。
// 戻り値が「参照」されている限り、配列自体の参照がガベコレにトラッキングされる。
ref int X()
{
    var array = new int[1];
    return ref array[0];
}
 
// 一方、これはダメ。
// ガベコレが走ったら、もはやポインターが有効な場所を指さなくなる。
unsafe int* Y()
{
    var array = new int[1];
    fixed (int* p = array)
        return p;
}
```

ということで、`ref`を使って unsafe なことをしたいときに使うのが
`Unsafe` クラスです。

例としては[`Span<T>`構造体](../../../../study/csharp/resource/span.md)があります。
(というか、`Unsafe`クラスを導入するに至った最初の動機は`Span<T>`構造体を作るためでした。)

`Span<T>`は、以下のように、配列でもポインターでも統一的に扱える型です。

```csharp
using System;
using System.Runtime.InteropServices;
 
class Program
{
    static void Main()
    {
        // 配列
        Span<int> array = new int[8].AsSpan().Slice(2, 3);
 
        // 文字列
        ReadOnlySpan<char> str = "abcdefgh".AsSpan().Slice(2, 3);
 
        // スタック領域
        Span<int> stack = stackalloc int[8];
 
        unsafe
        {
            // ガベコレ管理外メモリ
            var p = Marshal.AllocHGlobal(sizeof(int) * 8);
            Span<int> unmanaged = new Span<int>((int*)p, 8);
 
            // 他の言語との相互運用
            var q = malloc((IntPtr)(sizeof(int) * 8));
            Span<int> interop = new Span<int>((int*)q, 8);
 
            Marshal.FreeHGlobal(p);
            free(q);
        }
    }
 
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern IntPtr malloc(IntPtr size);
 
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void free(IntPtr ptr);
}
```

こういう型を作ろうと思うと、通常なら unsafe コードだらけ・ポインターだらけになるんですが、
`Span<T>`構造体はその代わりに [`Unsafe` クラスだらけ・`ref`だらけ](https://source.dot.net/#System.Private.CoreLib/shared/System/Span.Fast.cs,d2517139cac388e8)です。

### ジェネリックな型をポインター化

C# の unsafe コードの仕様では、ジェネリックな型はポインター化できません。
とはいえ、この制限は実はちょっと厳しすぎです。

```csharp
// 値型しか含まない構造体はポインター化 (A*) できる。
struct A
{
    public int X;
}
 
// 1つでも参照型を含んでいる場合、ポインター化されるとガベコレが追えなくなって困る。
// なので、ポインター化できない仕様もやむなし。
struct B
{
    public string X;
}
 
// ならこのジェネリックな場合はどうか。
// T に値型を渡したとき、値型しか含まない構造体になり得る。
// T 次第でポインター化できるかどうか変えてもよかったのではないか。
// (現状は無条件にポインター化 (C<int>* とかも) 不可)
struct C<T>
{
    T X;
}
```

C# 7.3 で [unmanaged 制約](../../../../study/csharp/interop/sp_unsafe.md#unmanaged-types)が入って、
多少は制限が緩和したんですが、いまだこの例の `C<T>` のような型はポインター化できません。
(C# 8.0 で緩和される可能性あり。遅くとも C# 8.x の間には緩和されると思われます。)

が、`Unsafe`クラスを使えば(今でも)そんな制限をガン無視できます。

```csharp
using System;
using System.Runtime.CompilerServices;
 
struct C<T>
{
    public T X;
}
 
public class Program
{
    unsafe static void Main()
    {
        var c = new C<int>();
        int* p = (int*)Unsafe.AsPointer(ref c);
        *p = 1;
        Console.WriteLine(c.X); // 1
    }
}
```

## Unsafe クラスを safe なところから呼べる

もちろん、`Unsafe` クラス悪用すると、unsafe コード以上に unsafe になります。

にもかかわらず、`Unsafe`クラスのメソッドの引数・戻り値は大半が `ref` になっているので、unsafe コードなしで呼び出せます。
ある意味、これが一番の欠陥で、言語機能の不足を感じます
(「ポインターは使っていないけども unsafe コードからしか呼べない」みたいな制約を付けれる機能が欲しい)。
(実際、corefx/coreclr 内でも度々そういう話題は上がっています。
そもそも利用頻度が低いクラスなので需要はあんまりありませんが…)
