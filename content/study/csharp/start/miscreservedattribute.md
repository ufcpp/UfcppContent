---
title: "[雑記] コンパイル結果に影響を及ぼす属性"
source_url: "https://ufcpp.net/study/csharp/start/miscreservedattribute/"
content_type: "Article"
published_at: "2021-09-12T00:00:00"
updated_at: "2021-10-31T17:36:19"
tags: []
umbraco_id: 2361
parent_id: 1190
sort_order: 19
aliases: []
---

# \[雑記\] コンパイル結果に影響を及ぼす属性

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

「[属性](../dynamic/sp_attribute.md)」の説明を「[動的な処理](../index.md#dynamic)」に並べているように、
多くの場合(特に属性を自作する場合)、
属性は[リフレクション](../dynamic/sp_reflection.md#reflection)を使って実行時に型情報から読みだして使うものです。
(動的な処理自体、うちのサイト内では結構後半での説明なので、属性自体についての説明も後々になります。)

ところが、いくつかの属性は C# コンパイラー自体が解釈して、
コンパイル結果に影響を及ぼします。

## <a id="sec-generated-title-2"></a> <a id="reserved-attribute">予約属性</a>

この手の、(動的・実行時ではなく) 静的・コンパイル時に影響を及ぼす属性として、以下のようなものがあります。

* `AttributeUsage` (`System`名前空間) : 属性の用途を指定します
* `Obsolete` (`System`名前空間) : 時代遅れでもう使ってほしくない(別のクラスやメソッドに移行してほしい)ものに付けて、警告を発します
* `Conditional` (`System.Diagnostics` 名前空間) : 特定の条件下でのみ実行されるメソッドを定義できるようにします
* 呼び出し元情報(CallerInfo)属性: メソッドの呼び出し元に関する情報を得るために使います

[後述するように](#new-syntax)、裏でこっそりとコンパイル結果に影響を及ぼす属性は他にももっとたくさんあるんですが、
明示的な属性指定でコンパイル結果が変わるものは以上です。
これらの属性の事を <strong id="key-reserved-attribute" class="keyword">予約属性</strong> (reserved attribute)と呼びます。

`AttributeUsage`, `Obsolete`, `Conditional` は C# 1.0 の頃からあるもので、当時はこの3つだけが予約属性でした。
その後、C# 5.0 で[呼び出し元情報属性](../cheatsheet/ap_ver5.md#CallerInfo)の `CallerFilePath`, `CallerLineNumber`, `CallerMemberName` 属性(いずれも `System.Runtime.CompilerServices` 名前空間)が追加されました。
また、C# 10.0 で、呼び出し元情報属性に `CallerArgumentExpression` が追加されました。

### <a id="sec-generated-title-3"></a> <a id="AttributeUsage">AttributeUsage</a>

`AttributeUsage` 属性(`System`名前空間)では、
[属性を自作](../dynamic/sp_attribute.md#userdefine)する際に、属性の使い方(名前通り、attribute usage)を指定します。

以下のように、指定した以外の属性の使い方をするとコンパイル エラーを起こします。

```csharp {title="AttributeUsage の利用例"}
[AttributeUsage(AttributeTargets.Class)]
class ForClass : Attribute { }

[ForClass] // これは OK。
class A
{
    [ForClass] // これは「ターゲットが合わない」というエラーになる。
    public void M() { }
}
```

### <a id="sec-generated-title-4"></a> <a id="Obsolete">Obsolete</a>

`Obsolete` 属性(`System`名前空間)は、もう廃止(obsolete)したいクラスやメソッドに付けて、そのクラスやメソッドの利用者側コードに警告やエラーを出します。
通常、廃止理由や移行先に関する情報を書いておきます。

```csharp {title="Obsolete 属性の利用例"}
HighPerformance.AlgorithmA(); // 警告が出る
Cryptograph.AlgorithmA();     // エラーになる

class HighPerformance
{
    [Obsolete("遅いので AlgorithmB に移行してほしい")]
    public static void AlgorithmA() { }
    public static void AlgorithmB() { }
}

class Cryptograph
{
    [Obsolete("セキュリティ強度が低いので AlgorithmB に移行してほしい", error: true)]
    public static void AlgorithmA() { }
    public static void AlgorithmB() { }
}
```

### <a id="sec-generated-title-5"></a> <a id="Conditional">Conditional</a>

`Conditional` 属性 (`System.Diagnostics` 名前空間)を付けると、特定の条件下でのみ実行されるメソッド(conditional method: 条件付きメソッド)、特定の条件下でのみ認識される属性(conditional attribute: 条件付き属性)を定義できるようにします。

([条件付き属性](../cheatsheet/ap_ver2.md#conditional)は C# 2.0 からの機能です。)

条件付きメソッドは、
[`#if` ディレクティブなどを使った条件付きコンパイル](../misc/sp_preprocess.md#conditional)を使ったコードと一緒で、
[`#define` ディレクティブ](../misc/sp_preprocess.md#symbol)などでシンボル定義があるときだけ実行されるメソッドになります。

一番多い用途は「デバッグ時にのみ実行」で、例えば標準ライブラリ中の [`Debug.Assert` メソッド](https://docs.microsoft.com/ja-jp/dotnet/api/system.diagnostics.debug.assert)には `Conditional` 属性が付いています。

```csharp {title="標準ライブラリの Debug.Assert メソッド(宣言部分のみ)"}
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics
{
    public static class Debug
    {
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition);
    }
}
```

これを呼びだすコードはデバッグビルド時にのみ実行されます。

```csharp {title="Debug.Assert の利用例"}
using System.Diagnostics;

void M(int x)
{
    // デバッグ実行時にだけ x > 0 判定が残る。
    // 「リリースまでにはこの条件に当てはまらない呼び出し元は絶対に残さない」、
    // 「だったらリリース時にこの条件判定が残るのはパフォーマンス的にもったいない」
    // みたいなときに使う。
    Debug.Assert(x > 0);
}
```

### <a id="sec-generated-title-6"></a> <a id="CallerInfo">呼び出し元情報(caller info)</a>

<h5 class="version version5">Ver. 5</h5>
<h5 class="version version10">Ver. 10</h5>

以下の4つの属性を使って、メソッドの呼び出し元に関する情報を得ることができます
(いずれも `System.Runtime.CompilerServices` 名前空間)。
通称、CallerInfo (呼び出し元の情報)属性と言います。

* `CallerFilePath`: 呼び出し元のファイル名
* `CallerLineNumber`: 呼び出し元の行番号
* `CallerMemberName`: 呼び出し元のメンバー名（メソッド名、プロパティ名、イベント名等々）
* `CallerArgumentExpression`: 呼び出し元で、特定の引数に渡した式

このうち、前3つは C# 5.0 から、最後の `CallerArgumentExpression` は C# 10.0 から使える属性です。
(それ以前のコンパイラーは単にこの属性を無視します。)

これらは、以下のように、[オプション引数](../structured/st_function.md#default-parameter)になっている引数に属性を付ける形で使います。

```csharp {title="CallerInfo 属性の利用例"}
static void M(
    int x,
    [CallerLineNumber] int line = 0,
    [CallerFilePath] string? file = null,
    [CallerMemberName] string? member = null,
    [CallerArgumentExpression("x")] string? arg = null)
{
    Console.WriteLine($@"{file} の {line} 行目
{member} から呼ばれていて
{arg} という式を引数に渡している
(実際の値は {x})
");
}
```

これを、例えば以下のようなコードから呼び出したとします。

```csharp {title="CallerInfo 属性を使ったメソッドを呼び出す例"}
using System.Runtime.CompilerServices;

class Program
{
    static void Main()
    {
        M(2 * 3 * 5);
    }
}
```

すると、省略したオプション引数の部分に、行番号、ファイルのフルパス、呼び出し元のメンバー名(この場合 `Main` メソッド)、引数に渡した式などの整数/文字列が挿入されます。
この例の場合、(ファイル名は環境によって変わりますが)以下のような出力が得られます。

```console {title="CallerInfo 属性を使ったメソッドを呼び出す例"}
C:\Users\ufcpp\source\repos\ConsoleApp1\ConsoleApp1\Program.cs の 7 行目
Main から呼ばれていて
2 * 3 * 5 という式を引数に渡している
(実際の値は 30)
```

主な用途はデバッグ、ログ用です。

他に面白い使い方としては、「null 判定で、何の変数が null だったかを知らせるために使う」と言うようなこともできます。
.NET 6 (C# 10.0) で導入された [`ThrowIfNull` メソッド](https://docs.microsoft.com/ja-jp/dotnet/api/system.argumentnullexception.throwifnull)がまさにこの機能を使っています。
この `ThrowIfNull` は以下のような宣言になっています。

```csharp {title="ThrowIfNull (宣言部分のみ)" highlight-text="[CallerArgumentExpression(&quot;argument&quot;)]"}
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
    public class ArgumentNullException
    {
        public static void ThrowIfNull(
            [NotNull] object? argument,
            [CallerArgumentExpression("argument")] string? paramName = null);
    }
}
```

このメソッドは以下のような使い方をします。

```csharp {title="ThrowIfNull を使ったメソッドの利用例" highlight-text="ThrowIfNull(myArgument)"}
M(null);

void M(string? myArgument)
{
    ArgumentNullException.ThrowIfNull(myArgument);
}
```

この場合、「null だったら例外」なメソッドにわざと null を渡しているので例外が発生します。
投げられる例外にはちゃんと「何が null だったか」の情報が渡っていて、
以下のようなメッセージが表示されるはずです。

```console {title="ThrowIfNull を使ったメソッドの利用例" highlight-text="'myArgument'"}
Unhandled exception. System.ArgumentNullException: Value cannot be null. (Parameter 'myArgument')
```

ちなみに、これらの数値/文字列はコンパイル時[定数](sp_const.md#constant)になります。
直接数値/文字列を書く場合と比べて追加のコストは掛かりません。

また、これらの属性は拡張メソッドとかでもちゃんと動きます。

```csharp {title="拡張メソッドで CallerArgumentExpression 属性を使う例"}
using System.Runtime.CompilerServices;

(from x in new[] { 1, 2, 3 } select x * x).Sum().M();

static class Extensions
{
    public static void M(this int x, [CallerArgumentExpression("x")] string? ex = null)
    {
        Console.WriteLine($"{ex} = {x}");
    }
}
```

```console {title="拡張メソッドで CallerArgumentExpression 属性を使う例"}
(from x in new[] { 1, 2, 3 } select x * x).Sum() = 14
```

## <a id="sec-generated-title-7"></a> <a id="new-syntax">属性を使った新機能</a>

C# の新機能のうち結構な割合のものが、

* 既存の構文で書けるコードに属性を付けたものが生成される
* その属性が付いている場合、コンパイラーが特殊対応する

というような実装方法になっています。

比較的新しいものでいうと、例えば C# 8.0 で導入された [null 許容参照型](../cheatsheet/ap_ver8.md#nullable-reference-type)は `Nullable` 属性、`NullableContext` 属性を使ったコードに展開されます。

例えば nullable enable な場所で以下のようなコードを書いた場合、

```csharp {title="null 許容参照型の例" highlight-ranges="sha256:c655c3bb9e3740e93f7916edd5a44b9db8d4f5b1104df4912cd09af89a15362d;3:26-3:27,4:26-4:27"}
public class C
{
    public void M1(string? x) { }
    public void M2(string? x, string y, string z) { }
}
```

旧来の(nullable disable な場所での)コードでいうところの以下のようなコードが得られます。

```csharp {title="null 許容参照型の展開結果の例" highlight-ranges="sha256:9acd3f64bad6ba8cf24a453972d527545a817e92c5cf887b8c9619a037405e81;5:5-5:25,10:5-10:25,11:27-11:40"}
using System.Runtime.CompilerServices;

public class C
{
    [NullableContext(2)]
    public void M1(string x)
    {
    }

    [NullableContext(1)]
    public static void M2([Nullable(2)] string x, string y, string z)
    {
    }
}
```

逆に古くからあるものだと拡張メソッドがそうで、以下の2つのコードが同じ意味になります。

```csharp {title="拡張メソッドの例" highlight-text="this"}
public static class C
{
    public static void M(this string x) { }
}
```

```csharp {title="拡張メソッドの展開結果の例" highlight-text="[Extension]"}
using System.Runtime.CompilerServices;

public static class C
{
    [Extension]
    public static void M(string x) { }
}
```

ただし、対応するバージョン以降
(今あげた例でいうと、null 許容参照型は C# 8.0、拡張メソッドは C# 3.0)では、
これらの属性を手書きで使うことはできません。
「直接使うな、拡張メソッド構文を使え」と言うようなコンパイル エラーになります。

## <a id="sec-generated-title-8"></a> <a id="can-be-internal"></a>internal 属性

昔は、この手の属性は public である必要がありました。
C# 3.0 の頃はまさにそうで、`Extension` 属性は public です。

ところが最近は「internal でもいい」と言うことになっています。
例えば以下のような状況を想定しています。
(public であることを義務付けてしまうと最後の「被り」が解消できなくて困る。)

* `CallerArgumentExpressionAttribute` という名前の属性さえあれば、古いバージョンの .NET ランタイム上でも使える
* `CallerArgumentExpressionAttribute` が標準ライブラリ入りするのは .NET 6 (C# 10.0 と同世代)から
* .NET 5 (C# 9.0 と同世代)でも C# 10.0 にしてこの属性を使いたいので自前で同じ名前の属性を用意
* その .NET 5 なコードを、.NET 6 な別のライブラリやアプリから参照したい
* 自前定義の属性と標準ライブラリ中の属性が被って困る

## <a id="sec-generated-title-9"></a> <a id="compiler-generated"></a>コンパイラー生成属性

さらに言うと、最近は標準ライブラリ中に定義された属性を参照するのではなく、
コンパイラーが属性自体をコンパイル時生成していることが多いです。

例えば[前述の null 許容参照型]の `Nullable`, `NullableContext` 属性はコンパイラー生成です。
標準ライブラリにこれらの属性が定義されているわけではなく、
コンパイル結果に以下のような属性が追加されて、それが使われます。

```csharp {title="コンパイラー生成の null 許容参照型関連属性"}
using System.Runtime.CompilerServices;

namespace Microsoft.CodeAnalysis
{
    [CompilerGenerated]
    [Embedded]
    internal sealed class EmbeddedAttribute : Attribute { }
}

namespace System.Runtime.CompilerServices
{
    [CompilerGenerated]
    [Microsoft.CodeAnalysis.Embedded]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
    internal sealed class NullableAttribute : Attribute
    {
        public readonly byte[] NullableFlags;
        public NullableAttribute(byte P_0) => NullableFlags = new byte[1] { P_0 };
        public NullableAttribute(byte[] P_0) => NullableFlags = P_0;
    }
    [CompilerGenerated]
    [Microsoft.CodeAnalysis.Embedded]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    internal sealed class NullableContextAttribute : Attribute
    {
        public readonly byte Flag;
        public NullableContextAttribute(byte P_0) => Flag = P_0;
    }
}
```

他にも例えば、C# 7.3 の [`unmanaged` 制約](../interop/sp_unsafe.md#unmanaged-constraints)も `IsUnmanaged` 属性がコンパイラー生成されています。

```csharp {title="IsUnmanaged 属性"}
namespace System.Runtime.CompilerServices
{
    [CompilerGenerated]
    [Microsoft.CodeAnalysis.Embedded]
    internal sealed class IsUnmanagedAttribute : Attribute { }
}
```
