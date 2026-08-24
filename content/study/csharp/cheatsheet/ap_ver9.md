---
title: "C# 9.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver9/"
content_type: "Article"
published_at: "2020-05-09T00:00:00"
updated_at: "2021-05-02T00:00:00"
tags: []
umbraco_id: 2294
parent_id: 1174
sort_order: 14
aliases: []
---

# C# 9.0 の新機能

<div class="version version9">Ver. 9.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2020/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 5.0</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>レコード型</li>
</ul>
</td>
</tr>
</table>

## <a id="sec-generated-title-1"></a> <a id="record"></a>レコード型

C# 9.0 で、レコード型(records)という新しい種類の型が追加されました。
record (記録)という名前通り、データの読み書きに使うことを意図した型です。
例えば以下のような書き方で、「`Name` という文字列と `Birthday` という日付」を読み書きできます。

```csharp {title="record の例"}
using System;
 
record Person(string Name, DateTime birthday);
```

詳しくは「[レコード型](../datatype/record.md)」で説明します。

### <a id="sec-generated-title-2"></a> <a id="init-only"></a>init-only プロパティ

以下のように `init` という新しいアクセサーを使って、「オブジェクト初期化子までは書き換え可能で、それ以降は書き換えできないプロパティ」を作れるようになりました。

```csharp {title="オブジェクト初期化子でだけ書き換え可能" highlight-ranges="sha256:16786cdd6f117b8e9bba8406daabae0a7c4ee1ad66ea4ff0406968d188743910;6:25-6:29,7:25-7:29" error-text="p.X"}
var p = new Point { X = 1, Y = 2 };
p.X = 3; // ダメ。
 
class Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
```

`readonly` の制限が厳しすぎるので、問題ない範囲でちょっとだけ制限を緩めたもです。
(歴史的経緯で `init` という新キーワードが使われていますが、もし C# をフルスクラッチで作り直せるなら `readonly` が最初から `init` 相当の仕様になっていたと思います。)

詳しくは「[init-only プロパティ](../oop/oo_property.md#init-only)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="top-level-statements"></a>トップ レベル ステートメント

トップ レベル(top-leve: クラスや名前空間よりも外側、ファイル直下)に[ステートメント](../start/st_variable.md#statement)を直接書けるようになりました。

例えばよくある「Hello World」であれば、単に以下のように書けるようになります。

```csharp {title="トップ レベルに直接「Hello World」"}
using System;
Console.WriteLine("Hello World!");
```

詳しくは「[トップ レベル ステートメント](../misc/miscentrypoint.md#top-level-statements)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="pattern-v3"></a>パターンの追加

[C# 7.0](ap_ver7.md)から脈々と改善されてきた[パターン マッチング](../datatype/patterns.md)ですが、
C# 9.0 でもいくつかのパターンが追加されています。

```csharp {title="C# 9.0 でのパターン追加"}
// not, and, or や、 <, <=, >, >= などのパターンが増えた。
int M(uint x) => x switch
{
    0 or 2 or 4 or 6 or 8 => 0,
    1 or 3 or 5 or 7 or 9 => 1,
    >= 10 => -1,
};
```

3世代かけてようやく当初予定(C# に追加すること自体は最初から決めていた機能)が全て入りました。
当初から予定に入ってたのは、既存のいくつかのプログラミング言語が同様の文法を持っていて、
[網羅性](../datatype/patterns.md#exhaustive)や[重複](../datatype/patterns.md#case-duplicate)の検査を含めて実装手段が既知で、検討コストが低いからです。
それでも、需要が高いものから少しずつ実装した結果、3世代に分かれました。
3世代目なことを指して「パターン v3」(patterns v3)という俗称があったりもします。

詳しくは「[パターン マッチング](../datatype/patterns.md)」で説明します。
C# 9.0 で追加されているのは以下の3つです。

- [型パターンの簡単化](../datatype/patterns.md#simplified-type-pattern)
- [パターンの組み合わせ](../datatype/patterns.md#pattern-combintor)
- [関係演算パターン](../datatype/patterns.md#relational-patterns)

## <a id="sec-generated-title-5"></a> <a id="target-typed-inference"></a>ターゲット型推論の強化

### <a id="sec-generated-title-6"></a> <a id="target-typed-new"></a>ターゲットからの new 型推論

ターゲット型からの推論が効く場合に、`new T()` の `T` の部分を省略できるようになりました。
(target-typed new とか呼ばれたりします。)

特に、[`var`](../start/sp3_inference.md#type-inference) が使えず、
型名が長い特に便利です。

```csharp {title="フィールド初期化子で特に便利"}
using System.Collections.Generic;
 
class Sample
{
    // フィールドに対しては var が使えない。
    // 代わりに new 型推論を使うと便利なことがある(特に、型名が長い時)。
    Dictionary<string, List<(int x, int y)>> _cache = new();
}
```

詳しくは「[ターゲットからの new 型推論](../oop/oo_construct.md#target-typed-new)」で説明します。

### <a id="sec-generated-title-7"></a> <a id="target-typed-conditional"></a>条件演算子のターゲット型推論

条件演算子の第2・第3項がターゲット型からの型推論するようになりました。


```csharp
void M(bool b)
{
    // int? を明示するとコンパイルできる(var だとダメ)。
    int? i = b ? 1 : null;
 
    // Base を明示するとコンパイルできる(var だとダメ)
    Base c = b ? new A() : new B();
}
 
class Base { }
class A : Base { }
class B : Base { }
```

詳しくは「[条件演算子のターゲット型推論](../structured/st_branch.md#terget-typed-conditional)」で説明します。
「[型の決定](../start/misctyperesolution.md)」も参考にしてください。

## <a id="sec-generated-title-8"></a> <a id="class-covariant-returns"></a>クラスの共変戻り値

仮想メソッドの戻り値に共変性が認められるようになりました。
(機能名の俗称としては、「クラスの共変戻り値」と言ったりします。)

例えば以下のようなコードを書けるようになります。

```csharp {title="仮想メソッド戻り値の共変性"}
class Base
{
    public virtual Base Clone() => new Base();
}
 
class Derived : Base
{
    // これの戻り値が Base じゃなくてもよくなった。
    // Derived は常に Base に安全に変換可能なので、 Base Clone() の override として Derived Clone() を使える。
    public override Derived Clone() => new Derived();
}
```

詳しくは「[多態性/戻り値の共変性](../oop/oo_polymorphism.md#covariance)」で説明します。

## <a id="sec-generated-title-9"></a> <a id="unsafe"></a>unsafe/ネイティブ相互運用向け機能

[C# 7.2](ap_ver7_2.md)の辺りから、
言語の方向性として生産性や安全性を優先する C# でも、
パフォーマンス改善を目的とするような言語機能が結構増えてきました。

また、 クロスプラットフォーム化が進んだことで、ネイティブ相互運用関連の機能も増えています。

この手の機能は一般的な開発者が直接触れることは少ないですが、
.NET ランタイム自体や、大規模に使われているライブラリのパフォーマンス改善につながり、
間接的にすべての C# 開発者が恩恵を受けるものになります。

### <a id="sec-generated-title-10"></a> <a id="skip-locals-init"></a>ローカル変数の0初期化抑止

`/unsafe` オプション指定時限定ですが、ローカル変数の0初期化を抑止できるようになりました。

```csharp {title="SkipLocalsInit 属性で0初期化抑止"}
using System;
using System.Runtime.CompilerServices;
using System.Text.Unicode;
 
m("aあ😀");
 
// この属性を付けると stackalloc の要素の0初期化がなくなる。
[SkipLocalsInit]
static void m(string s)
{
    // UTF-16 の文字数に大して、UTF-8 のバイト数は最大でも3倍以内。
    Span<byte> buffer = stackalloc byte[s.Length * 3];
    Utf8.FromUtf16(s, buffer, out _, out var bytesWritten);
 
    // FromUtf16 の仕様上、bytesWritten バイト目までは必ず上書きされる。
    // 上書きされた部分だけを使う分には0初期化は「余計なお世話」。
    var written = buffer[..bytesWritten];
 
    foreach (var b in written)
    {
        Console.WriteLine(b);
    }
}
```

詳しくは「[ローカル変数の0初期化抑止](../interop/sp_unsafe.md#skip-locals-init)」で説明します。

### <a id="sec-generated-title-11"></a> <a id="function-pointer"></a>関数ポインター

C# で関数ポインターを書けるようになりました。

.NET の内部的にはこれまでも関数ポインターがあったんですが、 それを C# から効率的に呼ぶ手段がありませんでした。 これに対して、C# 9 では delegate* という記法で関数ポインターを扱えるようになりました。

```csharp {title="関数ポインター構文の例"}
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

詳しくは「[関数ポインター](../interop/functionpointer.md)」で説明します。

## <a id="sec-generated-title-12"></a> <a id="nint"></a>native int

`nint` と `nuint` というキーワードで、「CPU 依存の一番高速に扱える整数」が使えるようになりました。
`nint` が符号付、`nuint` が符号なしです。

```csharp {title="CPU 依存幅整数"}
nint x = 0x1_0000;
x = x * x;

// 32ビット CPU だと 0、64ビット CPU だと 100000000 になるはず。
Console.WriteLine($"{x:X}");

unsafe
{
    // 32ビット CPU だと 4、64ビット CPU だと 8 になるはず。
    Console.WriteLine(sizeof(nint));
}
```

ちなみに、内部的には `IntPtr`、`UIntPtr` (いずれも `System` 名前空間)にコンパイルされています。
そのため、以下のようなコードはコンパイル エラーになります(引数の型が同じ同名のメソッドが2つあるため)。

```csharp {title="IntPtr と nint でのオーバーロードはできない"}
class Sample
{
    void M(IntPtr x) { }
    void M(nint x) { }
}
```

ただ、単純に 「`IntPtr`、`UIntPtr` に別名が付いた」というわけではなく、`+` などの演算子の挙動が違います。
(※ C# 10 までの話。 [C# 11](ap_ver11.md#numeric-intptr) 以降は「`nint`、`nuint` は `IntPtr`、`UIntPtr` の別名 」という扱いになりました。)

`IntPtr` の場合は `operator +(IntPtr pointer, int offset)` しか持っていませんが、
`nint` の場合は普通に整数としての四則演算が全て行えます。 
ちなみに、コンパイラーが `IntPtr` と `nint` を区別するため、`nint` だった場合は `NativeInteger` 属性(`System.Runtime.CompilerServices` 名前空間)が付きます。

今更こんな機能が入った背景には、パフォーマンス改善やネイティブ相互運用の強化があります。
例えば、以下のような場面で `nint` を使っています。

* [ネイティブ コード側が CPU 依存幅の整数になっている場合の相互運用](https://github.com/dotnet/runtime/blob/7984b32774916c98ab7c85c244c9e40581e4cdf5/src/libraries/Common/src/Interop/OSX/Interop.libobjc.cs#L11-L17)
* [配列のインデックス アクセスは `nint` を使った方が速い](https://github.com/dotnet/runtime/blob/4017327955f1d8ddc43980eb1848c52fbb131dfc/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Char.cs#L30) (C++ でいう `size_t` な処理)

## <a id="sec-generated-title-13"></a> <a id="other"></a>その他

### <a id="sec-generated-title-14"></a> <a id="nrt"></a>null 許容参照型の改善

C# 8.0 で入った [null 許容参照型](../resource/nullablereferencetype.md)に対してちょっと改善が入っています。
主に以下の2点です。

- [制約なしジェネリック型引数に `?` を付けれるようになった](../resource/nullablereferencetype.md#unconstrained-generics)
- [アノテーション属性](../resource/nullablereferencetype.md#annotation-attributes)に `MemberNotNull` と `MemberNotNullWhen` が増えた

### <a id="sec-generated-title-15"></a> <a id="lambda-discard"></a>ラムダ式の引数を破棄

ラムダ式の引数で、`_` を使った値の破棄ができるようになりました。

```csharp {title="ラムダ式の引数で _ を破棄扱い"}
static void Subscribe(INotifyPropertyChanged source)
{
    // _ を破棄扱いして、2個以上並べられる
    source.PropertyChanged += (_, _) => { };
}
```

詳細は「[値の破棄 - ラムダ式の引数](../datatype/declarationexpressions.md#lambda-discard)」で説明します。

### <a id="sec-generated-title-16"></a> <a id="static-anonymous-function"></a>静的匿名関数

匿名関数に対しても `static` 修飾子を付けれるようになりました。
「外部の変数を捕獲しない」という意味になります。

```csharp {title="静的匿名関数" highlight-ranges="sha256:b04aded3c3417ac1f26cf6166f2498c20c12d1b4f71170587adfa7ff9b32bd35;6:21-6:27,9:21-9:27" error-ranges="sha256:b04aded3c3417ac1f26cf6166f2498c20c12d1b4f71170587adfa7ff9b32bd35;9:33-9:34"}
using System;
 
int a = 0;
 
// 以下の行は自身の引数しか使っていないので static にしても怒られない。
Func<int, int> ok = static x => x * x;
 
// 以下の行は外側のローカル変数 a を使ってしまったのでコンパイル エラー。
Func<int, int> ng = static x => a * x;
```

詳しくは「[静的匿名関数](../functional/fun_localfunctions.md#static-local-function)」で説明します。

### <a id="sec-generated-title-17"></a> <a id="local-function-attribute"></a>ローカル関数への属性適用

[ローカル関数](../functional/fun_localfunctions.md#local-function)に属性を付けられるようになりました。

```csharp {title="ローカル関数に属性を付ける"}
using System;
using System.Diagnostics.CodeAnalysis;
 
m("", "");
 
static void m(string? a, string? b)
{
    // C# 9.0 からローカル関数に属性を付けれる。
    // C# 8.0 の null 許容参照型がらみで特に有用。
    [return: NotNullIfNotNull("s")]
    string? toLower(string? s) => s?.ToLower();
 
    if (a is not null && b is not null)
    {
        // a, b の null 許容性が、NotNullIfNotNull 属性のおかげで al, bl に伝搬。
        string al = toLower(a);
        string bl = toLower(b);
 
        // a, b が非 null なので、al, bl は非 null で確定済み。改めてのチェック不要。
        Console.WriteLine(al.GetHashCode());
        Console.WriteLine(bl.GetHashCode());
    }
}
```

### <a id="sec-generated-title-18"></a> <a id="extension-getenumerator"></a>拡張メソッドでの GetEnumerator 実装

[パターン ベース](../misc/miscpatternbased.md#index)な [`foreach`](../data/sp_foreach.md#extension-getenumerator)、[`await foreach`](../async/asyncstream.md#await-foreach)で、拡張メソッドによる実装ができるようになりました。

## <a id="sec-generated-title-19"></a> <a id="source-generator"></a>ソースコード生成

正確には C# という言語の機能ではなく、「C# 9.0 と同時期に実装された」というだけですが、
C# 9.0 世代の C# コンパイラーにはソースコード生成(source generator)プラグインの作成機能が追加されました。
詳細は「[コード解析とコード生成](../misc/analyzer-generator.md)」で説明しています。

これと同時に、ソースコード生成を前提とした文法もいくつか実装されました。

### <a id="sec-generated-title-20"></a> <a id="extended_partial_method"></a>部分メソッドの拡張

[ソースコード生成](../misc/analyzer-generator.md)では、手書きでは不完全な C# コードを書いて、
それをソースコード生成で埋めてもらうという状況があり得ます。
C# 9.0 ではそのための文法として、[`partial` キーワード](../oop/oo_class.md#partial_method)を再利用することにしました。

```csharp {title="ソースコード生成で埋めてもらう前提の不完全なメソッドの例"}
// (1) 手書き前提のコード
partial class PartialClass
{
    public void M()
    {
        System.Console.WriteLine(
            "PreGeneratedMethod が呼ばれた直後"
            + WantSourceGenerated());
    }
 
    // C# コードが先にあって、これを元にソースコード生成してほしいメソッド。
    private partial string WantSourceGenerated();
}
 
// (2) C# からのソースコード生成が前提のコード
partial class PartialClass
{
    private partial string WantSourceGenerated() => "手書きはしづらしくて、ソースコード生成なら楽な文字列";
}
```

C# 2.0 の頃からある部分メソッドとの差は[アクセシビリティ](../oop/oo_conceal.md#level)修飾子の有無です。
`private` などを付けるかどうかで「コード生成が先」か「手書きが先」かの用途が逆になります。

詳しくは「[部分メソッドの拡張](../oop/oo_class.md#extended_partial_method)」で説明します。

### <a id="sec-generated-title-21"></a> <a id="module-initializer"></a>モジュール初期化子

プログラムの実行時、最初に1回だけ呼び出したい処理が必要になることがあります。
「[静的コンストラクター](../oop/oo_static.md#ctor)」で説明しているように、この静的コンストラクターという機能を使っても「最初に1回だけ呼ばれる」ということができますが、C# 9.0 ではモジュール初期化子という書き方もできるようになりました。

以下のように、`ModuleInitilizer` 属性(`System.Runtime.CompilerServices` 名前空間)を付けた[静的メソッド](../oop/oo_static.md#stmethod)を書くと、それが必ず1回呼び出されるようになります。

```csharp {title="ModuleInitialize 属性"}
using System;
using System.Runtime.CompilerServices;
 
class Sample
{
    [ModuleInitializer]
    public static void Init()
    {
        Console.WriteLine("必ず1回だけ呼ばれる");
    }
}
```

これをモジュール初期化子(module initializer)と呼びます。

ソースコード生成と組み合わせて、これまでなら[リフレクション](../dynamic/sp_reflection.md)に頼らざるを得なかったような処理を、コンパイル時コード生成に置き換えたりできます。
(他にも使い道はありますが、モジュール初期化子導入の最大のモチベーションはこれです。)

詳しくは「[モジュール初期化子](../oop/moduleinitializer.md)」で説明します。
