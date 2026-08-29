---
title: "[雑記] InlineArray"
source_url: "https://ufcpp.net/study/csharp/datatype/inline-array/"
content_type: "Article"
published_at: "2023-09-20T00:00:00"
updated_at: "2025-02-15T15:49:32"
tags: []
umbraco_id: 2472
parent_id: 1940
sort_order: 7
aliases: []
---

# \[雑記\] InlineArray

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

<h5 class="version version12">Ver. 12</h5>

.NET 8 で、
[`InlineArray` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.inlinearrayattribute) (`System.Runtime.CompilerServices` 名前空間) というものが入りました。

基本的には .NET ランタイム側の機能ですが、
いくつか、C# 側にもこの `InlineArray` 向けの特殊対応が入っています。

ちなみに、この機能は現状、
[コレクション式](https://github.com/ufcpp/UfcppSample/issues/447)の内部実装にこそ使っていますが、
本稿で書いているようなコードを直接書く必要はほぼありません。
(実質、本稿はコレクション式の内部実装(の一部)の説明みたいなものです。)

## <a id="sec-generated-title-2"></a> <a id="inline-array-attribute">InlineArray 属性</a>

.NET 8 から、
以下のように、構造体に属性を付けると構造体のサイズが変わります。

```csharp {title="InlineArray 属性"}
using System.Runtime.CompilerServices;

// この属性を付けると、 .NET ランタイムが特別扱いして、構造体のサイズを拡大する。
// (コンストラクター引数で Length 指定。)
[InlineArray(3)]
struct FixedBuffer<T>
{
    // フィールドを1個だけ書く。
    // (2個以上書くとコンパイル エラーになる。)
    // 構造体のサイズが sizeof(T) × Length になる。
    private T _value;
}
```

inline array という名前通り、「埋め込み配列」として使います。
(長さ N の配列代わりに、長さ N 個分のサイズを持った構造体を作ります。
C# の配列はヒープに割り当てられるのに対して、この inline array であればスタック上に値を持てます。)

要は、以下のような「N 個のフィールドを並べる」みたいな構造体を、ランタイム側で自動的に作ってくれる機能です。

```csharp {title="N 個のフィールドを手書きで並べた例" warning-ranges="8:15-8:22,9:15-9:22" warning-diagnostics="CS0169@8:15-8:22,CS0169@9:15-9:22"}
using System.Runtime.InteropServices;

struct FixedBuffer3<T>
{
    // 所望の個数フィールドを書く。
    // (3要素くらいならいいけども、数十とか数百になるときつい。)
    private T _value0;
    private T _value1;
    private T _value2;

    // 変換とかも自前で書く。
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref _value0, 3);

    public ref T this[int index] => ref AsSpan()[index];
}
```

## <a id="sec-generated-title-3"></a> <a id="vs-stackalloc">stackalloc との違い</a>

これまでも [`stackalloc`](../interop/sp_unsafe.md#safe-stackalloc) という機能を使えば、
一応、スタック上に配列上のデータを置くことはできました。
ただ、`stackalloc` には結構強い制限があって使いづらいです。

一番きつい制限は、参照型、もしくは、参照を含む型に対して使えないことです
(これを認めようとすると[ガベコレ](../../computer/essential-software/memorymanagement.md#garbage-collection)の負担が上がって、パフォーマンス的にかえって不利になるそうです)。
例えば以下のコードでは、`string` 以下の型に対してコンパイル エラーになります。

```csharp {title="参照を含むときには stackalloc は使えない" error-ranges="7:29-7:35,11:39-11:54,12:6-12:22,12:40-12:56" error-diagnostics="CS0208@7:29-7:35,CS0208@11:39-11:54,CS0306@12:6-12:22,CS0208@12:40-12:56" warning-ranges="16:19-16:25,21:20-21:23" warning-diagnostics="CS0649@16:19-16:25,CS0649@21:20-21:23"}
// 構造体に対しては使える。
Span<int> i = stackalloc int[100];
Span<DateTimeOffset> d = stackalloc DateTimeOffset[100];

// クラスに対しては使えない。
// (コンパイル エラーになる。)
Span<string> s = stackalloc string[100];

// クラスや参照を含む構造体に対しても使えない。
// (コンパイル エラーになる。)
Span<ContainsRefType> r1 = stackalloc ContainsRefType[100];
Span<ContainsRefField> r2 = stackalloc ContainsRefField[100];

struct ContainsRefType
{
    public string String;
}

ref struct ContainsRefField
{
    public ref int Ref;
}
```

また、`stackalloc` で確保したスタック領域は、メソッドを抜けるまで解放されません。
このせいで、ループの内側で間違って `stackalloc` を使ってしまうと簡単にスタック オーバーフロー(要はメモリ不足)を引き起こします
(一般に、スタックはヒープよりもだいぶサイズが小さいです。Windows の場合は 1MB 程度)。
例えば以下のコードを Windows で実行するとスタック オーバーフローします
(1000 とか 200 とか、そこまで大きくない数字ですら簡単にスタック オーバーフローになります)。

```csharp
for (int i = 0; i < 1000; i++)
{
    _ = stackalloc long[200];
}
```

## <a id="sec-generated-title-4"></a> <a id="special-syntax">C# 側特殊対応</a>

一応、C# 側にもこの InlineArray に対する特殊対応が入っています。
(一応、C# 12 の新機能。)

まず、属性を付けた型に対するチェックが働いています。
すでに前述の例でも書いていますが、
`InlineArray` 属性を付けた型にフィールドが2つ以上あるとコンパイル エラーになります。

```csharp {title="InlineArray 属性を付けた型に対するチェック"}
using System.Runtime.CompilerServices;

[InlineArray(3)]
struct FixedBuffer<T>
{
    // フィールドを2個以上書くとコンパイル エラーになるのは一応「C# の新機能」。
    private T _value;
}
```

また、この型を使う側に、以下のような特殊対応が入っています。

* インデクサーを直接書ける
* `Span<T>`/`ReadOnlySpan<T>` に暗黙的に変換できる
* `foreach` で列挙できる

```csharp {title="InlineArray 型利用側の特殊対応"}
FixedBuffer<string> buffer = new();

// InlineArray に対して直接インデクサーを書ける。
buffer[0] = "zero";
buffer[1] = "one";

// Span/ReadOnlySpan に暗黙的に変換できる。
Span<string> span = buffer;
span[2] = "two";

// foreach で列挙できる。
foreach (var x in buffer)
{
    Console.WriteLine(x);
}
```

## <a id="sec-generated-title-5"></a> <a id="collection-expressions">コレクション式と InlineArray</a>

前述の通り、
`InlineArray` 属性には `[EditorBrowsable(Never)]` が付いていて、
開発者が直接使う想定はあまりありません。

ただ、この機能は C# 12 時点で、コレクション式の最適化のために使われています。
`Span<T>` や `ReadOnlySpan<T>` 型に対してコレクション式を使うと、
`InlineArray` に展開されます。
例えば以下のようなコードの場合、

```csharp {title="Span/ReadOnlySpan に対するコレクション式の例"}
Span<int> i = [1, 2, 3, 4, 5];

ReadOnlySpan<string> s = ["a", "abc", ""];
```

以下のようなコードとほぼ同じ挙動になります。

```csharp {title="上記のコレクション式は InlineArray に展開される"}
using System.Runtime.CompilerServices;

var i0 = new FixedArray5<int>();
i0[0] = 1;
i0[1] = 2;
i0[2] = 3;
i0[3] = 4;
i0[4] = 5;
Span<int> i = i0;

var s0 = new FixedArray3<string>();
s0[0] = "a";
s0[1] = "abc";
s0[2] = "";
ReadOnlySpan<string> s = s0;

[InlineArray(3)]
struct FixedArray3<T>
{
    private T _value;
}

[InlineArray(5)]
struct FixedArray5<T>
{
    private T _value;
}
```

## <a id="sec-generated-title-6"></a> <a id="future">将来展望</a>

現状では、先ほどの例でいうと `FixedArray3<T>` と `FixedArray5<T>` があるように、
長さごとに別の型を用意せざるを得ない状態です。
「N 個のフィールドを並べる」コードを手書きするよりはマシですが、
まだ一時しのぎ的な実装になっていることは否めません。

根本的に大工事して型システムを改善するなら、
例えば、以下のように「整数型引数」を導入して、これを使って `InlineArray` を作りたいという話もなくはないです。

```csharp {title="「整数型引数」で InlineArray"}
// ※仮定の文法
namespace System;

public struct InlineArray<T, int N>;
```

こういう「public にできる(一時しのぎではないちゃんとした) `InlineArray` 型」があるのなら、
C# 側でももう少し踏み込んだ文法を導入したかったみたいです。
候補として挙がっていたのは、`int[N]` という書き方で「長さ N の `InlineArray`」を書けるようにするというものです。

```csharp {title="T[N]"}
// ※仮定の文法
var c = new C();

int[3] values = c.Values;

class C
{
    private int[3] _values;
    public int[3] Values => _values;
}
```

前述の `InlineArray<T, int N>` みたいな書き方をできるようにするのは結構大変で、
短期的には実現しそうになく、
それに依存しそうな `int[N]` という書き方も残念ながらしばらく実現の見込みはありません。
