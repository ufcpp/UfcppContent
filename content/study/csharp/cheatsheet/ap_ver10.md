---
title: "C# 10.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver10/"
content_type: "Article"
published_at: "2021-07-24T00:00:00"
updated_at: "2021-10-24T00:00:00"
tags: []
umbraco_id: 2353
parent_id: 1174
sort_order: 15
aliases:
  - "/csharp/cheatsheet/ap_ver10/"
---

# C# 10.0 の新機能

<div class="version version10">Ver. 10.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2021/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 6.0</li>
<li>Visual Studio 2022</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li></li>
</ul>
</td>
</tr>
</table>

※一部、まだ記事化(めったに使わない機能や細かい修正の紹介)が完了していません:

* <a id="async-builder">Async method builder override</a>
* Enhanced `#line` directive

執筆予定: [C# 10.0 トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/342)

## <a id="sec-generated-title-1"></a> <a id="record-struct">record struct</a>

C# 9.0 (レコード型の最初のバージョン)では、レコード型は常に[参照型](../resource/oo_reference.md#reftype)(クラスと同系統の型)になります。
これに対して C# 10.0 では[値型](../resource/oo_reference.md#valtype)も選べるようにしました。
そのため、以下のように、`record class` と `record struct` というキーワードで書き分けができるようになりました。

```csharp
record class Reference(int X, int Y); // record だけ書いた場合こちらと同じ意味
record struct Value(int X, int Y);
```

詳しくは 「[レコード型](../datatype/record.md)」のページ内に色々と追記しました。

## <a id="sec-generated-title-2"></a> <a id="struct-parameterless-ctor"></a>構造体の引数なしコンストラクター

構造体に引数なしコンストラクターとかフィールド初期化子を書けるようになりました。

```csharp
struct A
{
    public int X;
    public A() => X = 1;
}
```

これで、`new A()` で `X` が1になります。
詳しくは「[引数なしコンストラクター](../resource/rm_struct.md#parameterless-ctor)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="string-interpolation"></a>文字列補間

[文字列補間](../start/st_string.md#string-interpolation)に2点ほど改善が入りました。

### <a id="sec-generated-title-4"></a> <a id="improved-string-interpolation"></a>パフォーマンス改善

`string.Format` を使った実装ではどうしてもパフォーマンス上の改善が難しく、
別の型を使って結構複雑なコードに変換する最適化が入りました。
条件を満たす場合、

```csharp
var formatted = $"({x}, {y})";
```

このコードは `string.Format` ではなく、以下のようなコードに展開されます。

```csharp
DefaultInterpolatedStringHandler handler = new DefaultInterpolatedStringHandler(4, 2);
handler.AppendLiteral("(");
handler.AppendFormatted(x);
handler.AppendLiteral(", ");
handler.AppendFormatted(y);
handler.AppendLiteral(")");
string s = handler.ToStringAndClear();
```

詳しくは「[C# 10.0 の補間文字列の改善](../start/improvedinterpolatedstring.md)」で説明します。

### <a id="sec-generated-title-5"></a> <a id="improved-string-interpolation"></a>const 文字列補間

[文字列補間](../start/st_string.md#string-interpolation)でも、`{}` の中身が `const` 文字列な場合に限り、補完結果も `const` にできます。
例えば以下のような `const` 文字列を作れます。

```csharp
const string A = "Abc";
const string B = "Xyz";
const string C = $"{nameof(A)}: {A}, {nameof(B)}: {B}"; // "A: Abc, B: Xyz"
```

詳しくは「[const 文字列補間](../start/sp_const.md#constant-string-interpolation)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="CallerArgumentExpression"></a>CallerArgumentExpression 属性

`CallerArgumentExpression` 属性を使って、メソッド呼び出し元でどの引数にどういう式を渡したかを文字列として取れるようになりました。

```csharp
using System.Runtime.CompilerServices;

m(2 * 3 * 5);

static void m(
    int x,
    [CallerArgumentExpression("x")] string? expression = null)
{
    Console.WriteLine($"{expression} = {x}");
}
```

```console
2 * 3 * 5 = 30
```

詳しくは「[呼び出し元情報(caller info)](../start/miscreservedattribute.md#CallerInfo)」で説明します。

## <a id="sec-generated-title-7"></a> <a id="simple-program"></a>シンプル プログラム

C# 9.0 の[トップ レベル ステートメント](ap_ver9.md#top-level-statements)に続いて、シンプルなプログラムであればシンプルなソースコードで書けるようになる機能が増えています。

これらの機能によって、いわゆる [Hello World プログラム](https://ja.wikipedia.org/wiki/Hello_world)を以下の1行で書けるようになりました。

```csharp
Console.WriteLine("Hello, World!");
```

実際、 .NET 6 からはコンソール アプリのプロジェクト テンプレートがこの1ファイル、1行だけのものになっています。

また、Web アプリ用のテンプレートも以下のような1ファイルのコードになりました。

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

参考: 「[最初の C# プログラム](../../../blog/2021/8/newprojecttemplate/index.md)」

これらを実現するために、C# の文法にもいくつかの新機能が追加されました。

### <a id="sec-generated-title-8"></a> <a id="file-scoped-namespace"></a>ファイル スコープ名前空間

C# 10.0 から `{}` なしの以下のような書き方で名前空間を指定できるようになりました。

```csharp
namespace Namespace;

class A { }
```

これで以下のコードと同じ意味になります。

```csharp
namespace Namespace
{
    class A { }
}
```

詳しくは「[ファイル スコープ namespace](../structured/sp_namespace.md#file-scoped-namespace)」で説明します。

### <a id="sec-generated-title-9"></a> <a id="global-using"></a>global using

`using` ディレクティブの前に `global` という修飾を付けることで、
[プロジェクト](../package/project.md#project)内全域に対して影響を及ぼす `using` (名前空間の参照)ができるようになりました。

例えば、プロジェクト内のどこか1つのファイルに以下のようなコードを書いたとします。

```csharp
global using System;
```

これで、このプロジェクト内のすべてのファイルで、ファイルの先頭に `using System;` を書いたのと同じ状態になります。

詳しくは「[global using](../structured/sp_namespace.md#global-using)」で説明します。

### <a id="sec-generated-title-10"></a> <a id="lambda-improvement"></a>ラムダ式の改善(自然な型決定、戻り値明示、属性指定)

Web アプリ用テンプレートの `MapGet` を実現するために、
ラムダ式とデリゲートに以下の3つの機能が追加されました。

* 自然な型決定
* ラムダ式の戻り値の明示
* ラムダ式への属性

これらにより、ラムダ式やデリゲートを以下のように書けるようになりました。

```csharp
var f =
    [A]
    [return: B]
    static int ([C] int x)
    => x;
```

詳しくは「[デリゲートの自然な型](../functional/sp_delegate.md#natural-type)」と「[ラムダ式の戻り値の明示と属性](../functional/fun_localfunctions.md#lambda-csharp10)」で説明します。

## <a id="sec-generated-title-11"></a> <a id="others"></a>その他

### <a id="sec-generated-title-12"></a> <a id="sub-pattern-name"></a>プロパティ パターンの拡張(入れ子のメンバー参照)

入れ子のプロパティ・フィールド参照でプロパティ パターンを書けるようになりました。

```csharp
    if (x is { Name.Length: 1 })
    {
        Console.WriteLine("single-char Name");
    }
```

詳しくは「[プロパティ パターン](../datatype/patterns.md#sub-pattern-name)」で説明します。

### <a id="sec-generated-title-13"></a> <a id="mixed-deconstruction"></a>分解宣言と分解代入の混在

分解代入と分解宣言の混在もできるようになりました。

```csharp
int x;
(x, var u) = (1, 2);
```

ただし、式の途中に分解宣言 (var 付きの宣言) が来るようなコードは C# 10.0 でも書けません。

```csharp
int x, y;
(x, var u) = (var v, y) = (1, 2);
```

### <a id="sec-generated-title-14"></a> <a id="definite-assignment"></a>明確な初期化ルールの改善

明確な初期化ルール(未初期化のまま変数から値を読めないようにするフロー解析)に関する改善がありました。
これまでは `?.` や `??` が絡んだ時の判定があまり賢くなかったんですが、C# 10 で改善しました。

例えば以下のコードは C# 10 以降でだけコンパイルできます。

```csharp
// C# 10 から大丈夫な例: ?. == true。
void m(Dictionary<int, int>? d)
{
    if (d?.TryGetValue(123, out var x) == true)
    {
        // C# 10 から大丈夫になった。
        // (前までは ?. からの == true は判定漏れでエラー。)
        Console.WriteLine(x);
    }
}
```

「[[雑記] 明確な代入ルール](../start/definiteassignment.md)」で説明しています。
