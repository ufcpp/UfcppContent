---
title: "配列インデックスは0以上"
source_url: "https://ufcpp.net/blog/2018/12/arrayindex/"
content_type: "BlogEntry"
published_at: "2018-12-22T10:25:16"
updated_at: "2021-07-22T14:57:11"
tags: []
umbraco_id: 2204
parent_id: 2177
sort_order: 21
aliases: []
---

# 配列インデックスは0以上

今日は corefx (.NET の標準ライブラリ)の実装レベルの最適化の話。

.NET Core 2.0 とか 2.1 リリースの頃にブログも出ていましたが、
.NET Core 2.X 世代は結構パフォーマンス改善を頑張っています。

- [Performance Improvements in .NET Core](https://blogs.msdn.microsoft.com/dotnet/2017/06/07/performance-improvements-in-net-core/)
- [Performance Improvements in .NET Core 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1/)

実際、 .NET Framework で動かしていたアプリを、 .NET Core 2.1 で動かすようにするだけで、アプリ側では何もしなくても1～2割くらいは高速化します。

改善の方法としては、[`Span<T>`構造体](../../../../study/csharp/resource/span.md)を使ってアロケーションを減らしたり、[Devirtualize 処理](../devirtualization/index.md)を掛けたりといったなんか上等そうな最適化もたくさんやっています。
でも、今日はそんな高尚なものの話ではなく、「.NET の配列のインデックスは0以上の整数 (`int` なのに負の値は絶対来ない)」と言う前提での細かい最適化の話です。

## 負のインデックス？

corefx/coreclr のプルリクエストで、ここ1年くらいの間、頻出する最適化がありまして。
配列操作で以下のようなコードはよく書くと思います。

```csharp
if (index < 0 || index >= length)
    throw new IndexOutOfRangeException();
```

これを、以下のように書き換えるだけ。

```csharp
if ((uint)index >= (uint)length)
    throw new IndexOutOfRangeException();
```

比較と OR が1回ずつ減っているので速くなるという理屈。
[corefx 内で「uint length」で検索](https://github.com/dotnet/coreclr/search?q=uint+length&unscoped_q=uint+length)してもらえばわかりますけど、割かし大量に出てきます。

これは、でも、配列のインデックスが0開始の`int`だからできる最適化になります。
(元からインデックスが`uint`だった場合、`int.MaxValue`より大きいインデックスの時の挙動が狂う。)

### Array.CreateInstance

.NET の配列のインデックスは必ず0以上。いいね？

ところで、以下のコードはどう思います。

```csharp
using System;
 
public class Program
{
    static void Main()
    {
        var array = Array.CreateInstance(typeof(string),
            lengths: new[] { 4 },
            lowerBounds: new[] { -4 });
 
        array.SetValue("a", -4);
        array.SetValue("b", -3);
        array.SetValue("c", -2);
        array.SetValue("d", -1);
 
        foreach (var x in array)
        {
            Console.WriteLine(x);
        }
    }
}
```

ちゃんと動きます。
VB 6 時代の名残っぽいんですけども、
.NET では、0以外の開始インデックスを持つ配列を作る機能があります。
上記の例は、-4 開始。

ということで、先ほどの「配列長は0以上」を前提にしたコードがいいのかどうか、という話になったりもするんですが。
どうも、`T[]` 型にキャストすることができなくなるから大丈夫みたいです。

```csharp
// 開始インデックス 0 を明示して CreateInstance
// それを string[] にキャスト。
// これは問題なく動きます。
var a1 = (string[])Array.CreateInstance(typeof(string),
    lengths: new[] { 4 },
    lowerBounds: new[] { 0 });
 
// 0 以外を指定した上で、全く同じく string[] にキャスト。
// InvalidCastException が発生。
var a2 = (string[])Array.CreateInstance(typeof(string),
    lengths: new[] { 4 },
    lowerBounds: new[] { -4 });
```

ちなみに、例外メッセージは以下のような感じ。

```txt
Unable to cast object of type 'System.String[*]' to type 'System.String[]'.
```

`String[*]` 型… だと…
つまるところ、開始インデックスが型情報の一部…
(実際には、「0開始のやつ」と「0じゃないやつ」の2つの型しかなくて、1開始と-1開始みたいなやつはどちらも `T[*]` 型になって、同じ型みたいです。)

配列だけ特殊対応してもらってていいなぁ…
C++ の template みたいに、C# のジェネリクスでも型引数に整数を渡したいのに… (できない)

まあ、それはさておき、とりあえず、.NET の `T[]` で書かれる配列のインデックスは0以上の整数と思っていいみたいです。

## 最上位ビットを流用

配列のインデックスをフィールドに持つような型がちらほらあります。

### Memory

例えば、[`Memory<T>`構造体](https://source.dot.net/#q=System.Private.CoreLib%20System.Memory%3CT%3E)がそうなんですが、これの中身は以下のような感じ。
(`_object`には配列、もしくは、`MemoryManager<T>`型が入ります。)

```csharp
public readonly struct Memory<T>
{
    private readonly object _object;
    private readonly int _index;
    private readonly int _length;
}
```

ここで、`_index`と`_length`、つまり、配列のインデックスと長さは絶対に負にならない保証があります。
負にならないということは、`int`の内部構造的には、最上位ビットが常に0です。

ということで、`Memory<T>`構造体では、この最上位ビットを別の用途に流用することで最適化していたりします。
具体的には、`_object`の中に実際に何が入っているかを弁別するために`_length`の最上位ビットを使っていたりします
(配列がpinned済みかどうか([`GCHandleType.Pinned`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.gchandletype)指定で[`GCHandle.Alloc`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.gchandle.alloc)されているかどうか)を記録しています)。

### `Index`

C# 8.0 では、以下のような構文で、
配列の一部分を`Span<T>`として切り出すことができるようになります。

```csharp
int[] array = { 1, 2, 3, 4, 5 };
var sub = array[1..^1]; // 先頭から1 ～ 末尾から1 の範囲
 
// 2, 3, 4
foreach (var x in sub)
{
    Console.WriteLine(x);
}
```

この、インデックスの値が先頭からなのか末尾からなのかを表すために、
以下のような [`Index` 構造体](https://github.com/dotnet/coreclr/blob/3464b60b85c8e10d69d8da86d2eb3f9e7aaa7c4b/src/System.Private.CoreLib/shared/System/Index.cs)が追加されています(主要部分のみ抜き出し)。

```csharp
public readonly struct Index
{
    private readonly int _value;
 
    public Index(int value, bool fromEnd)
    {
        _value = fromEnd ? ~value : value;
    }
 
    public int Value => _value < 0 ? ~_value : _value;
    public bool FromEnd => _value < 0;
}
```

これも、配列のインデックスは0以上という前提で、
「末尾から」の方を負の数で表すことで、追加の bool フラグを持たないようにしています。
