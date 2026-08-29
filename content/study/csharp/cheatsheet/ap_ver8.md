---
title: "C# 8.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver8/"
content_type: "Article"
published_at: "2019-03-02T00:00:00"
updated_at: "2019-10-22T00:00:00"
tags: []
umbraco_id: 2232
parent_id: 1174
sort_order: 13
aliases: []
---

# C# 8.0 の新機能

<div class="version version8">Ver. 8.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2019/9</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2019 16.3</li>
<li>.NET Core 3.0</li>
<li>.NET Standard 2.1</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>堅牢性向上</li>
</ul>
</td>
</tr>
</table>

C# 8.0 にはいろいろな新機能が含まれていますが、
主要なものは堅牢性向上を目的としたものになります。
プログラマーの人的ミスを避け、より堅牢なプログラムを書けるようにしたいというものです。

## <a id="sec-generated-title-1"></a> <a id=""></a>補足

### <a id="sec-generated-title-2"></a> <a id="langversion"></a>バージョン指定

ちなみに、C# 8.0 世代の C# コンパイラーから、[バージョンの指定方法](langversionoption.md#langversion)に `preview` というオプションが追加されました。
このオプションを指定することで、正式リリース前の機能をある程度先取りして試してみることができます。
例えば、C# 8.0 がデフォルトで有効になるのは Visual Studio 2019 16.3 からですが、
`preview` 指定であれば 16.0 の頃から使えました。
(名前通りプレビュー状態なので、正式リリースまでに変更が掛かる可能性が高いので注意は必要です。)

### <a id="sec-generated-title-3"></a> <a id="targetframework"></a>ターゲット フレームワーク

C# 8.0 の全ての機能を一切の小細工なしで満足に使えるのは .NET Core 3.0/.NET Standard 2.1 以降です。
古いターゲット フレームワークで C# 8.0 を使うには[バージョンの明示的な指定](langversionoption.md#new-options)が必要です。

ちなみに、以下の機能にはライブラリ依存があって、古いターゲット フレームワーク上で素では動きません。

- [非同期ストリーム](#async-stream)
- [範囲アクセス](#range)
- [null許容参照型](#nullable-reference-type)の一部([アノテーション属性](../resource/nullablereferencetype.md#annotation-attributes))

ただし、このうち、非同期ストリームは [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces/) という NuGet パッケージを参照することで、.NET Framework 4.6.1/.NET Core 2.0/.NET Standard 2.0 以降でも使えます。

また、[インターフェイスのデフォルト実装](#default-imeplementation-of-interface)は実行環境に手を入れないと実現できない機能で、
.NET Core 3.0/.NET Standard 2.1 以降でなければどうやっても動かすことができません。

## <a id="sec-generated-title-4"></a> <a id="nullable-reference-type"></a>null 許容参照型

参照型でも単に型 `T` と書くと null を受け付けず、`T?` と書いて初めて null 許容になる機能が追加されました。
null 許容参照型と呼びます。
ただ、これまでと型 `T` の意味を変えてしまうので、opt-in (オプションを明示しないと有効にならない)方式になっています。

```csharp {title="null 許容参照型の例" warning-ranges="16:39-16:41"}
// 有効化のためのディレクティブ
#nullable enable
 
class Program
{
    // 参照型でも ? の有無で null を許容するかどうかが変わる。
    string NotNull() => "";
    string? MaybeNull() => null;
 
    int M(string s)
    {
        var s1 = NotNull();
        var s2 = MaybeNull();
 
        // null チェックをしていないので、以下の行の s2 のところに警告が出る。
        return s.Length + s1.Length + s2.Length;
    }
}
```

「ぬるぽ」がなぜかネットスラングとして定着するくらい、「意図しない null によるバグ」は多くていらだたしいものです。
コンパイラーによるフロー解析によってこの手のバグを事前に避けれるようになることで、プログラムの堅牢性が増します。

詳しくは「[null 許容参照型](../resource/nullablereferencetype.md)」で説明します。

## <a id="sec-generated-title-5"></a> <a id="recursive-pattern"></a>再帰パターン

C# 7.0 で部分的に[パターン マッチング](../datatype/patterns.md)が実装されていましたが、C# 8.0 で完全版になります。
C# 8.0 で追加されるパターンは再帰的なマッチングが可能で、「再帰パターン」(recursive pattern)と呼ばれたりもします。

例えば以下のような感じです(new! と書いている行が再帰パターン)。

```csharp {title="再帰パターンの例" highlight-ranges="16:9-16:21,17:9-17:33"}
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x = 0, int y = 0) => (X, Y) = (x, y);
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
 
class Program
{
    static int M(object obj)
        => obj switch
    {
        0 => 1,
        int i => 2,
        Point (1, _) => 4, // new! 位置パターン。
        Point { X: 2, Y: var y } => y, // new! プロパティ パターン。
        _ => 0
    };
}
```

単に短く書けるというだけではなく、以下のように、
コンパイラーによるチェックが掛かりやすく、人的ミスの回避にも貢献します。

```csharp {title="再帰パターンはコンパイラーによるチェックがちょっと賢い" error-ranges="17:14-17:34"}
void M(object obj)
{
    switch (obj)
    {
        case string s when s.Length == 0:
            break;
        // これまでの switch だと、間違えて同じ case を書いていてもエラーにならない。
        case string s when s.Length == 0:
            break;
    }
 
    switch (obj)
    {
        case string { Length: 0 }:
            break;
        // 再帰パターンだと同じ条件があるとコンパイル エラーになる。
        case string { Length: 0 }:
            break;
    }
}
```

詳しくは「[再帰パターン](../datatype/patterns.md)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="switch-expression"></a>switch 式

`switch`を式として書けるようになりました。
また、従来の `switch` ステートメントは C# の前身となるC言語のものの名残を強く残し過ぎていて使いにくいものでしたが、その辺りも解消されて使いやすくなりました。

以下のような書き方ができます。

```csharp {title="switch 式の例" highlight-text="switch"}
public int Compare(int? x, int? y)
    => (x, y) switch
    {
        (int i, int j) => i.CompareTo(j),
        ({ }, null) => 1,
        (null, { }) => -1,
        (null, null) => 0
    };
```

後置きの `switch` キーワードに続けて、`{}` 内に[パターン](../datatype/patterns.md)と返したい値を並べます。

詳しくは「[`switch` 式](../datatype/typeswitch.md#switch-expression)」で説明します。

## <a id="sec-generated-title-7"></a> <a id="range"></a>範囲アクセス

`a[i..j]` という書き方で「i番目からj番目の要素を取り出す」というような操作ができるようになりました。

```csharp {title=".. 構文"}
using System;
 
class Program
{
    static void Main()
    {
        var a = new[] { 1, 2, 3, 4, 5 };
 
        // 前後1要素ずつ削ったもの
        var middle = a[1..^1];
 
        // 2, 3, 4 が表示される
        foreach (var x in middle)
        {
            Console.WriteLine(x);
        }
    }
}
```

この手の範囲指定は、例えば `(a, b)` みたいに書いたときに、「`a` から `b` まで」なのか「`a` から始めて `b` 個」なのかで迷ったり、前者だとすると「`b` は含むのか含まないのか」でで迷ったりします。
言語構文として `a..b` を導入することでこういう不明瞭さを排除して、人的ミスを減らします。

この機能は、実際には以下の3つの機能の組み合わせになっています。

- `^i` で「後ろからi番目」を表す `Index` 型の値を得る
- `i..j` で「i番目からj番目」を表す `Range` 型の値を得る
- 所定の条件を満たす型に対して `Index`/`Range` を渡すと、所定のパターンに展開する

詳しくは「[インデックス/範囲処理](../data/dataranges.md)」で説明します。

## <a id="sec-generated-title-8"></a> <a id="default-imeplementation-of-interface"></a>インターフェイスのデフォルト実装

C# 8.0 (.NET Core 3.0)で、インターフェイスの制限が緩和されました。
以下のようになります。

- メソッド、[プロパティ](../oop/oo_property.md)、[インデクサー](../oop/oo_indexer.md)、[イベント](../functional/sp_event.md)のアクセサーの実装を持てるようになった
- [アクセシビリティ](../oop/oo_conceal.md#level)を明示的に指定できるようになった
- [静的メンバー](../oop/oo_static.md)を持てるようになった

これら指して「インターフェイスのデフォルト実装」(default implementations of interfaces)と呼びます。

```csharp {title="デフォルト実装"}
using System;
 
interface I
{
    void X();
 
    // 後から追加しても、デフォルト実装を持っているので平気
    void Y() { }
}
 
class A : I
{
    // X だけ実装していればとりあえず大丈夫
    public void X() { }
}
 
class B : I
{
    public void X() { }
 
    // Y も実装。I 越しでもちゃんとこれが呼ばれる。
    public void Y() => Console.WriteLine("B");
}
 
class Program
{
    static void Main() => M(new B());
    static void M(I i) => i.Y();
}
```

```console {title="デフォルト実装"}
B
```

機能面で言うと、クラス(特に[抽象クラス](../oop/oo_abstract.md#abclass))との差は「フィールドを持てない代わりに多重継承できる」というくらいに縮まりました。
ただ、
既存機能・既存コードへの影響を最小限にとどめるためであったり、
いくつかの理由からクラスの場合と既定動作などに差があるため注意が必要です。

詳しくは「[インターフェイスのデフォルト実装](../oop/oo_interface.md#dim)」で説明します。

ただし、インターフェイスのデフォルト実装は C# コンパイラー上のトリックだけでは実装できず、 .NET ランタイム側の対応が必要な機能です。
C# 8.0 以降を使っていても、ターゲットとなるランタイム(TargetFramework)が古いと使えません。
詳しくは以前書いたブログ「[RuntimeFeature クラス](../../../blog/2018/12/runtimefeature/index.md)」で説明しています。

## <a id="sec-generated-title-9"></a> <a id="async-stream"></a>非同期ストリーム

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では非同期メソッドが大幅に拡張されました。

- 非同期`foreach`: `await foreach`という書き方で、非同期なデータ列挙ができる([`foreach`ステートメント](../data/sp_foreach.md)の非同期版)
- 非同期`using`: `await using`という書き方で、非同期なリソース破棄ができる([`using`ステートメント](../resource/oo_dispose.md#using)の非同期版)
- 非同期イテレーター: 非同期メソッド内に`yield`を書けるようになる([イテレーター](../data/sp2_iterator.md)の非同期版)

例えば以下のように書けます。

```csharp {title="非同期イテレーターと非同期foreachの例" highlight-ranges="14:9-14:21,23:9-23:22"}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 
public class Program
{
    public static async Task Main()
    {
        await WriteItems(Select(GetData(), x => x * x));
    }
 
    static async IAsyncEnumerable<int> GetData()
    {
        yield return 1;
        await Task.Delay(1);
        yield return 2;
        await Task.Delay(1);
        yield return 3;
    }
 
    static async IAsyncEnumerable<int> Select(IAsyncEnumerable<int> source, Func<int, int> selector)
    {
        await foreach (var item in source)
        {
            yield return selector(item);
        }
    }
 
    static async Task WriteItems(IAsyncEnumerable<int> source)
    {
        await foreach (var item in source)
        {
            Console.WriteLine(item);
        }
    }
}
```

一連のデータ(data stream)を、非同期に生成(イテレーター)して非同期に消費(foreach)する機能なので、これらを合わせて非同期ストリーム(async stream)と呼ばれます。

詳しくは「[非同期ストリーム](../async/asyncstream.md)」で説明します。

## <a id="sec-generated-title-10"></a> <a id="using"></a>using ステートメントの改善

### <a id="sec-generated-title-11"></a> <a id="using-declaration"></a>using 変数宣言

変数宣言に対して `using` 修飾を付けることで、
その変数のスコープに紐づいて `using` ステートメントと同じ効果を得られるようになりました。
これを `using` 変数宣言(using declaration)と呼びます。

```csharp {title="using 変数宣言"}
using System;
 
readonly struct DeferredMessage : IDisposable
{
    private readonly string _message;
    public DeferredMessage(string message) => _message = message;
    public void Dispose() => Console.WriteLine(_message);
}
 
class Program
{
    static void Main()
    {
        // using var で、変数のスコープに紐づいた using になる。
        // スコープを抜けるときに Dispose が呼ばれる。
        using var a = new DeferredMessage("a");
        using var b = new DeferredMessage("b");
 
        Console.WriteLine("c");
 
        // c, b, a の順でメッセージが表示される
    }
}
```

詳しくは「[using 変数宣言](../resource/oo_dispose.md#using-declaration)」で説明します。

### <a id="sec-generated-title-12"></a> <a id="pattern-based-using"></a>パターン ベースな using

[ref 構造体](../resource/refstruct.md)に限るんですが、
パターン ベース(別にインターフェイスを実装していなくても、`Dispose` メソッドさえ持っていればOK)で [`using` ステートメント](../resource/oo_dispose.md#using)を使えるようになりました。

```csharp {title="パターン ベースな using ステートメント"}
// ref 構造体なので IDisposable インターフェイスは実装できない。
ref struct RefDisposable
{
    // けど、Dispose メソッドだけ用意。
    public void Dispose() { }
}
 
class Program
{
    static void Main()
    {
        // C# 7.3 まではコンパイル エラーになっていた。
        // C# 8.0 で OK に。
        using (new RefDisposable()) { }
    }
}
```

ref 構造体だけ対応したのは、需要が高く、既存コードを壊す心配が少ないからです
(既存コードの心配さえなければ任意の型で認めたかったそうです)。

詳しくは「[パターン ベースな using](../resource/oo_dispose.md#pattern-based-using)」で説明します。

## <a id="sec-generated-title-13"></a> <a id="others"></a>その他

こまごまとした修正がいくつかあります。

### <a id="sec-generated-title-14"></a> <a id="null-coalescing-assignment"></a>null 合体代入 (??=)

C# 8.0 では、null合体演算子 (`??`)も複合代入に使えるようになりました(`??=`)。

```csharp {title="null 合体代入" highlight-text="??="}
static void M(string s = null)
{
    s ??= "default string";
    Console.WriteLine(s);
}
```

詳しくは「[null 合体代入 (??=)](../resource/sp2_nullable.md#null-coalescing-assignment)」で説明します。

### <a id="sec-generated-title-15"></a> <a id="static-local-function"></a>静的ローカル関数

C# 8.0 から、外部の変数を捕獲しないことを明示するため、
ローカル関数に `static` 修飾を付けれるようになりました。
この機能を<strong id="key-static-local-function" class="keyword">静的ローカル関数</strong>(static local function)と呼びます。

```csharp {title="静的ローカル関数の例" error-ranges="8:28-8:29"}
void M(int a)
{
    // 外部の変数(引数)を捕獲(クロージャ化)。
    int f(int x) => a * x;
 
    // static を付けて、クロージャ化を禁止。
    // a を使っているところでコンパイル エラーになる。
    static int g(int x) => a * x;
}
```

詳しくは「[静的ローカル関数](../functional/fun_localfunctions.md#static-local-function)」で説明します。
同時に、変数の[シャドーイング](../functional/fun_localfunctions.md#shadowing)も認められるようになりました。

### <a id="sec-generated-title-16"></a> <a id="at-dollar"></a>@$

C# 7.0 では、文字列リテラル`""`の前に`$@`と付けることで、複数行に渡る[文字列補間](../start/st_string.md#multi-line)ができましたが、`$`と`@`の順序は`$@`しか認められていませんでした。

C# 8.0では`@$`の順でも認められるようになりました。

### <a id="sec-generated-title-17"></a> <a id="unmanaged-generic-struct"></a>アンマネージなジェネリック構造体

C# 8.0 では、ジェネックな構造体に対して再帰的にアンマネージ型かどうかの判定するようになりました。
型引数全てがアンマネージであれば、その構造体もアンマネージ扱いを受けるようになります。
```csharp {title="ジェネリックな構造体に対するポインター" highlight-text="KeyValuePair&lt;int, int&gt;* pkv = &amp;kv;"}
using System.Collections.Generic;
 
class Program
{
    unsafe static void Main()
    {
        var kv = new KeyValuePair<int, int>(1, 2);
        KeyValuePair<int, int>* pkv = &kv;
    }
}
```

詳しくは「[アンマネージなジェネリック構造体](../interop/sp_unsafe.md#unmanaged-generic-struct)」で説明します。

### <a id="sec-generated-title-18"></a> <a id="readonly-member"></a>readonly 関数メンバー

C# 8.0 で、[関数メンバー](../structured/st_function.md#sec-function-member)単位で「フィールドを書き換えてない」ということを保証できるようになりました。

```csharp {title="プロパティを readonly 修飾する例"}
struct Point
{
    public float X;
    public float Y;
 
    // readonly 修飾でフィールドを書き換えないことを明示
    public readonly float LengthSquared => X * X + Y * Y;
}
```

「[隠れたコピー](../resource/readonlyness.md#struct-readonly)」問題を避けやすくなります。

詳しくは「[readonly 関数メンバー](../resource/readonlyness.md#readonly-member)」で説明します。

### <a id="sec-generated-title-19"></a> <a id="nested-stackalloc"></a>式中の stackalloc

式中の任意の場所に `stackalloc` を書けるようになりました。
例えば以下のような書き方ができます。

```csharp {title="式中での stackalloc"}
using System;
 
class Program
{
    // Span を受け取る適当なメソッドを用意。
    static int M(Span<byte> buf) => 0;
 
    static void M(int len)
    {
        if (stackalloc byte[1] == stackalloc byte[1]) ;
        M(stackalloc byte[1]);
        M(len > 512 ? new byte[len] : stackalloc byte[len]);
    }
}
```

詳しくは「[式中の stackalloc](../resource/span.md#nested-stackalloc)」で説明します。

### <a id="sec-generated-title-20"></a> <a id="generic-is-null"></a>ジェネリック型に対する is null

ほぼ「バグ修正」レベルですが、
以下のコードがコンパイルできるようになりました。

```csharp {title="ジェネリック型に対する is null"}
static bool M<T>(T x) => x is null;
```

元々 `x == null` であればコンパイルできていたのに、`x is null` がコンパイルできないのは変だということで修正されました。
型引数 `T` が[非 null 値型](../resource/sp2_nullable.md#non-nullable)の時には常に false になります。

### <a id="sec-generated-title-21"></a> <a id="obsolete-accessor"></a>プロパティのアクセサーに Obsolete 指定

プロパティの get/set アクセサーに対して、どちらか片方にだけ `Obsolete` 属性(`System`名前空間)を指定できるようになりました。
以下のコードは C# 7.3 以前ではエラーになっていました。

```csharp {title="set にだけ Obsolete"}
class A
{
    public int X
    {
        get;
        [Obsolete] set;
    }
}
```
