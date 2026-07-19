---
title: "関数ポインター"
source_url: "https://ufcpp.net/study/csharp/interop/functionpointer/"
content_type: "Article"
published_at: "2023-04-01T00:00:00"
updated_at: "2023-04-01T20:42:18"
tags: []
umbraco_id: 2461
parent_id: 1321
sort_order: 5
aliases:
  - "/csharp/interop/functionpointer/"
---

# 関数ポインター

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

<h5 class="version version9">Ver. 9</h5>

関数ポインターとは、メモリ上でメソッドなどの命令列が入ってるアドレスを指すポインターで、
「そのアドレスにジャンプすることでメソッド呼び出しが実現されている」みたいなものです。

.NET の内部的にはこれまでも関数ポインターがあったんですが、
それを C# から効率的に呼ぶ手段がありませんでした。
これに対して、C# 9 では `delegate*` という記法で関数ポインターを扱えるようになりました。
([unsafe コンテキスト](sp_unsafe.md#unsafe)内限定で使えます。)

## <a id="sec-generated-title-2"></a> <a id="since1.0">以前からある関数ポインター</a>

関数ポインター自体は .NET には昔からあって、
例えば、関数ポインターの値を `IntPtr` (`nint`) で取得する手段は .NET Framework 1.0 (初代。2002年リリース)の頃からありました。

ただ、関数ポインターを使ったメソッド呼び出しの側は、C# には関連機能が一切なく、
一度デリゲート化するひと手間が必要でした。

```csharp
using System.Runtime.InteropServices;

var m = typeof(A).GetMethod("M")!;

// GetFunctionPointer で、メソッド M の関数ポインターが取れる。
nint ptr = m.MethodHandle.GetFunctionPointer();

// かつてはこれを直接呼ぶ手段はなくて、デリゲート化のひと手間が必要だった。
var a = Marshal.GetDelegateForFunctionPointer<Action>(ptr);

// これで A.M を間接的に呼べる。
a();

class A
{
    public static void M() => Console.WriteLine("A.M");
}
```

## <a id="sec-generated-title-3"></a> <a id="pinvoke">ネイティブ コード呼び出し</a>

まあ、C# で完結している分には役に立ちません。
C# で書いたメソッドを C# のデリゲートで受け取るんなら、
直接代入するだけでデリゲート化できます。
前節の例も、単に以下のように書けます。

```csharp
// C# で書いたメソッドを C# のデリゲートで受け取るんなら、単に代入でできるわけで、
// 関数ポインターを介する意味は全くなく。
Action a = A.M;

// これで A.M を間接的に呼べる。
a();

class A
{
    public static void M() => Console.WriteLine("A.M");
}
```

実際に関数ポインターを使う場面があるのは[ネイティブ コード呼び出し](sp_pinvoke.md)になります。

ネイティブ コード呼び出しも、`DllImport` 属性(.NET 7 以降であれば `LibraryImport` 属性)を使えば普通の、安全な C# コードだけで呼び出し可能ではあります。
例えば、`LibraryImport` 属性を使って kernel32.dll 中の `Beep` メソッドを呼ぶコードは以下のように書けます。

```csharp
using System.Runtime.InteropServices;

// 呼び出し側。
Native.Beep(440, 1000);

partial class Native
{
    // こんな感じで属性を付けておけば、 .NET ランタイム内でなんかよろしくやってくれてネイティブ コードを呼べる。
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool Beep(uint frequency, uint duration);
}
```

というか、かつてはネイティブ コードの関数ポインターを取る手段がありませんでした。
(上記の `Native.Beep` に対して `GetFunctionPointer` すると、
取れるのはあくまで「ネイティブ コード呼び出しを内部的によろしくやってくれる C# のメソッド」の関数ポインターになります。)

## <a id="sec-generated-title-4"></a> <a id="NativeLibrary">NativeLibrary クラス</a>

「関数ポインターを取る手段がないから使い道がない」と
「関数ポインターが指す先を呼び出す手段がないから取れてもしょうがない」で卵が先か鶏が先かみたいな話になるんですが、
C# に関数ポインターは必要ありませんでした。

ところが、 .NET Core 3.0 (C# 8.0 と同世代)で、[`NativeLibary`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.nativelibrary) (`System.Runtime.InteropServices` 名前空間)というクラスが入って、
ネイティブ コードの関数ポインターを取得する手段が提供されるようになりました。

```csharp
using System.Runtime.InteropServices;

// DLL のロード。
nint kernel32 = NativeLibrary.Load("kernel32.dll");

// 所望の関数の関数ポインターを取得。
nint p = NativeLibrary.GetExport(kernel32, "Beep");

// ただ、C# 8.0 時点だと呼び出しには一度デリゲート化する必要あり。
var a = Marshal.GetDelegateForFunctionPointer<BeepDelegate>(p);
a(440, 1000);

// ちなみに、 NativeLibrary の利点として、DLL のアンロードが可能。
NativeLibrary.Free(kernel32);

// GetDelegateForFunctionPointer にはジェネリックな型は渡せないらしく、
// Func<uint, uint, int> が使えないので同じ引数・戻り値のデリゲートを定義。
delegate int BeepDelegate(uint frequencey, uint duration);
```

`NativeLibary` は、
`DllImport` や `LibararyImoprt` と比べると煩雑ではありますが、
動的にロード・アンロードしたりといった細やかな制御が可能です。

この例のように、C# 8.0 時点では一度デリゲート化する必要があります。
ただ、このデリゲートを介する部分がペナルティになって、
`DllImport` よりも低速になっていました。

## <a id="sec-generated-title-5"></a> <a id="function-pointer">関数ポインター構文</a>

<h5 class="version version9">Ver. 9</h5>

問題は `IntPtr` (`nint`)でポインターを取れても、
引数や戻り値に関する情報がなくなっていて、
どうやって引数を渡して、どうやって戻り値を受け取ればいいかがわからないことです。

`NativeLibary` クラスも入ったことだし、C# でも関数ポインターを扱える構文が欲しいということになり、C# 9 で実際に導入されることになりました。
記法としては `delegate*` を使います。
先ほどの `NativeLibrary` を使った `Beep` 呼び出しの例を関数ポインターで書き換えると以下のようになります。

```csharp
using System.Runtime.InteropServices;

// 関数ポインターを nint で取得。
nint kernel32 = NativeLibrary.Load("kernel32.dll");
nint p = NativeLibrary.GetExport(kernel32, "Beep");

unsafe
{
    // 「関数ポインター型」にキャストして使う。
    // 構文的には delegate* から初めて、 <> の中に引数を戻り値の型を並べる。
    // (戻り値の型が最後。Func<> 風。)
    var fp = (delegate*<uint, uint, int>)p;
    fp(440, 1000);
}
```

`delegate*` から書き始めて、`<>` の中に引数と戻り値の型を並べます。

`<>` の中身は、最後の1個が必ず戻り値です。
`Func<>` と `Action<>` のように、戻り値の有無で型を分ける必要はなく、
「戻り値がない場合は最後の1個を `void` にする」という仕様です。

```csharp
unsafe
{
    // 引数 int, 戻り値 int
    delegate*<int, int> pf = &f;

    // 引数 int, 戻り値なし(void)
    delegate*<int, void> pa = &a;
}

// 同じようなコードでも、デリゲートだと Func/Action の分岐が必要。
Func<int, int> df = f;
Action<int> da = a;

// (こっちも普通に delegate<int, void> とか書きたい気持ちあるものの、現状、そういう仕様はない。)

static int f(int x) => x * x;
static void a(int x) { }
```

ちなみに、[IL](../../il/index.md) には .NET Framework 1.0 の頃から関数ポインターの仕様がちゃんとあって、「引数が `uint` 2つ、戻り値が `int`」みたいなのを指定して関数ポインターが指す先を呼び出す命令([`calli`](https://learn.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.calli))がありました。
あくまで C# 8 以前には `calli` を出力する能力がなかっただけです。

### <a id="sec-generated-title-6"></a> <a id="and-operator">& 演算子</a>

前節の例ですでに使っていますが、
C# で書いたメソッドに対して `&` 演算子を使えます。
`&` 演算子で、`GetFunctionPointer` などのリフレクション介さずにメソッドから直接関数ポインターを得ることができます。

```csharp
unsafe
{
    // & で A.M の関数ポインターを取得。
    delegate*<void> p = &A.M;

    // ちゃんと呼べる。
    p();
}

class A
{
    public static void M() => Console.WriteLine("A.M");
}
```

ただし、`&` 演算子で関数ポインターを取れるのは静的メソッドだけです。

```csharp
unsafe
{
    // 静的メソッドは OK。
    delegate*<void> p1 = &A.Static;

    // インスタンス メソッドは A.Instance みたいな参照の仕方はできないし、
    delegate*<void> p2 = &A.Instance;

    // デリゲートみたいに「インスタンス.メソッド」での参照も不可。
    delegate*<void> p3 = &new A().Instance;

    var a = new A();
    delegate*<void> p4 = &a.Instance;
}

class A
{
    public static void Static() { }
    public void Instance() { }
}
```

ちなみに、取れる値(関数ポインターが指すアドレス)自体は、`GetFunctionPointer` と同じになります。
ただし、`Type` 型や `MethodInfo` 型を介さなくていい分、`&` 演算子を使う方がパフォーマンスはいいそうです。

```csharp
var p1 = typeof(A).GetMethod("M")!.MethodHandle.GetFunctionPointer();
Console.WriteLine(p1);

unsafe
{
    delegate*<void> p2 = &A.M;

    Console.WriteLine((nint)p2); // p1 と同じ値が取れる。
    Console.WriteLine(p1 == (nint)p2); // true。
}

class A
{
    public static void M() { }
}
```

### <a id="sec-generated-title-7"></a> <a id="arguments">引数・戻り値の型</a>

`delegate*<T>` という、一見するとジェネリック型(`Func<T>` とか `Action<T>` とか)と似たような構文ですが、関数ポインターの `<>` の中に書ける型は、ジェネリック型引数よりもだいぶ制限が緩いです。
現状ではジェネリック型引数には書けない以下のような型も、関数ポインターの `<>` には普通に書けます。

* `ref T`, `out T`, `in T`
* ポインター型 `T*`
* `ref struct` な型
* `void`

```csharp
unsafe
{
    // in, out, ref が書ける
    delegate*<in int, out int, ref string> p1 = null;

    // ポインターが書ける
    delegate*<ref int, int*> p2 = null;

    // ref struct も書けるし、それのさらに ref も書ける
    delegate*<Span<int>, ref Span<int>> p3 = null;

    // 前述のとおり、戻り値がないときは void
    delegate*<void> p4 = null;
}
```

関数ポインターの入れ子も可能です。

```csharp
unsafe
{
    // 入れ子
    delegate*<delegate*<int, void>, int, delegate*<void>> p1 = null;
}
```

ちなみに、書ける型の制限が緩いので、Unsafe クラスですらできないことが関数ポインター使えば書けたり。

### <a id="sec-generated-title-8"></a> <a id="calling-convention">呼び出し規約</a>

複数のプログラミング言語をまたいでやり取りする場合、
呼び出し規約(calling convention)というものを気にする必要があります。

呼び出し規約は、引数や戻り値の受け渡しの仕方を呼ぶ側・呼ばれる側でそろえるためのルールです。
1つのプログラミング言語で完結している分にはコンパイラー任せで大丈夫ですが、
言語をまたぐときには明示が必要になります。
(「C# から Windows API を呼ぶ分にはデフォルトの規約が同じ」みたいな理由で省略可能なことはあります。)

`DllImport` では `CallingConvention` プロパティで、
`LibraryImport` では `UnmanagedCallConv` 属性で指定します。

```csharp
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

LibraryImports.Beep(440, 1000);
DllImports.Beep(440, 1000);

partial class LibraryImports
{
    // LibraryImport では UnmanagedCallConv 属性を付ける。
    [LibraryImport("kernel32.dll")]
    [UnmanagedCallConv(CallConvs  = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool Beep(uint frequency, uint duration);
}

class DllImports
{
    // DllImport では CallingConvention プロパティを指定する。
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Beep(uint frequency, uint duration);
}
```

関数ポインターでは、`delegate*` と `<>` の間に、
`managed` もしくは `unmanaged[]` という修飾を付けます。

```csharp
using System.Runtime.InteropServices;

nint kernel32 = NativeLibrary.Load("kernel32.dll");
nint p = NativeLibrary.GetExport(kernel32, "Beep");

unsafe
{
    // 規約を省略。省略時のデフォルトは managed。
    delegate*<int, int, int> p1 = &A.M;

    // managed 規約。C# で書いた普通のメソッドを呼ぶときに使う。
    // 要は「.NET ランタイム任せ」。
    delegate* managed<int, int, int> p2 = &A.M;

    // unmanaged のみ指定。
    // 呼び出し規約はプラットフォーム依存で、
    // Windows では stdcall、他のプラットフォームでは cdecl になるっぽい。
    var p3 = (delegate* unmanaged<uint, uint, int>)p;

    // unmanaged[] で呼び出し規約を明示。
    var p4 = (delegate* unmanaged[Stdcall]<uint, uint, int>)p;
}

class A
{
    public static int M(int x, int y) => x * y;
}
```
