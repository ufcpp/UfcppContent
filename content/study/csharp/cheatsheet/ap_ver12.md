---
title: "C# 12.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver12/"
content_type: "Article"
published_at: "2023-06-21T00:00:00"
updated_at: "2025-01-01T18:49:40"
tags: []
umbraco_id: 2467
parent_id: 1174
sort_order: 17
aliases: []
---

# C# 12.0 の新機能

<div class="version version12">Ver. 12.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2023/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 8.0</li>
</td>
</tr>
</table>

## <a id="sec-generated-title-1"></a> <a id="collection-expression">コレクション式</a>

`[]` 記号を使って配列などの初期化ができるようになりました。
配列だけではなく、コレクション(`List<T>` 型など)、`Span<T>` なども全く同じ書き方で初期化できます。
これをコレクション式(collection expression)と言います。

```csharp {title="コレクション式" highlight-ranges="sha256:f910249d2a264e3d542e6b64cb3d32585edd0d2a69bea57c15c964163f3ebd18;3:15-3:24,4:18-4:27,5:18-5:27,6:25-6:34,7:33-7:42"}
using System.Collections.Immutable;

int[] array = [1, 2, 3];
List<int> list = [1, 2, 3];
Span<int> span = [1, 2, 3];
ReadOnlySpan<int> ros = [1, 2, 3];
ImmutableArray<int> immutable = [1, 2, 3];
```

また、コレクション式中では、`..` を使うことで「別のコレクションの中身の展開」ができます。
これを スプレッド (spread)演算子と言います。

```csharp {highlight-ranges="sha256:69f2146a3dd2229ba74d69539a785a324542af8ee84d16dd1e8f9e876feb4108;5:22-5:24,5:32-5:34"}
int[] array1 = [1, 2, 3];
int[] array2 = [4, 5, 6];

// 0, 1, 2, 3, 4, 5, 6, 7
int[] combined = [0, ..array1, ..array2, 7];
```

詳しくは「[コレクション式](../datatype/collection-expression.md)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="primary-constructor">プライマリ コンストラクター</a>

通常のクラス、構造体に対してプライマリ コンストラクターが使えるようになりました。

```csharp
class A(int x)
{
    public int X { get; } = x;
}
```

レコード型の方を先に実装してしまったがために混乱があるんですが、
通常クラス・構造体の場合はプライマリ コンストラクター引数からプロパティを自動生成する機能はありません。

また、これに伴い、`class C;` というように、メンバーを1つも持たないでいい場合に `{}` を書く必要がなくなりました。

詳しくは「[プライマリ コンストラクター](../oop/oo_construct.md#primary-constructor)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="using-any-type">using エイリアスに任意の型を書けるように</a>

C# 11 ではエラーになっていた以下のようなコードをコンパイルできるようになりました。

```csharp {title="C# 12 から書ける using エイリアス"}
using Primitive = int;
using Array = int[];
using Nullable = int?;
using Tuple = (int, int);
```

詳しくは「[任意の型に対する using エイリアス](../structured/sp_namespace.md#using-any-type)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="lambda-default">ラムダ式のデフォルト引数</a>

ラムダ式の引数に[オプション引数](../structured/sp4_optional.md#optional)にできる(既定値を与えられる)ようになりました。
また、[params 引数](../structured/sp_params.md)も使えるようになりました。

```csharp {title="ラムダ式の引数の既定値と params 引数"}
// オプション引数(既定値値指定)。
var f1 = (int x = 1) => 0;

// params 引数。
var f2 = (params int[] x) => 0;

// 混在も OK。
var f3 = (int x = 1, params int[] y) => 0;
```

詳しくは「[ラムダ式のオプション引数(既定値)と params 引数](../functional/fun_localfunctions.md#lambda-default)」で説明します。

## <a id="sec-generated-title-5"></a> <a id="ref-readonly-param">ref readonly 引数</a>

ref 引数、in 引数の亜種として、
「書き換えはしないけども、右辺値は受け付けたくない」ということを表す ref readonly 引数というものを導入しました。

```csharp {title="ref readonly 引数" warning-ranges="sha256:433efe44affde8b8f29e8515c895292f8386c8cb7d64e005c29ae60f8b1cb693;4:3-4:5,8:3-8:8,11:3-11:4" warning-diagnostics="sha256:433efe44affde8b8f29e8515c895292f8386c8cb7d64e005c29ae60f8b1cb693;CS9193@4:3-4:5,CS9193@8:3-8:8,CS9192@11:3-11:4"}
// in 引数の代わりに ref readonly 引数。
void m(ref readonly int x) { }

m(10); // リテラルは警告に。

var a = 1;
var b = 2;
m(a + b); // 式も警告に。

// in や ref を付けないのも警告。
m(a);

// in を付けると警告が出ない。
m(in a);

// in 引数と違って、ref 修飾でも OK。
m(ref a);
```

ちなみに、呼び出し側の書き方が変わる以外に差はなく、コンパイル結果の挙動は in 引数と全く同じです。
呼び出し側の差は以下の通りです。

| 呼び方 | in | ref readonly |
| --- | --- | --- |
| `m(ref x)` | 警告 | OK |
| `m(in x)`  | OK | OK |
| `m(x)`, `m(x + y)`, `m(123)`     | OK | 警告 |

詳しくは「[ref readonly 引数](../resource/sp_ref.md#ref-readonly-param)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="other"></a>その他

### <a id="sec-generated-title-7"></a> <a id="inline-array">InlineArray</a>

.NET 8 で、[`InlineArray` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.inlinearrayattribute) (`System.Runtime.CompilerServices` 名前空間) というものが入って、「値型の固定長配列」みたいなものを作れるようになりました。

```csharp {title="InlineArray 属性"}
using System.Runtime.CompilerServices;

// この属性を付けると、 .NET ランタイムが特別扱いして、構造体のサイズを拡大する。
// (コンストラクター引数で Length 指定。)
[InlineArray(3)]
struct FixedBuffer<T>
{
    private T _value;
}
```

基本的には .NET ランタイム側の機能ですが、
いくつか、C# 側にもこの `InlineArray` 向けの特殊対応が入っています。

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

詳しくは「[[雑記] InlineArray](../datatype/inline-array.md)」で説明します。

### <a id="sec-generated-title-8"></a> <a id="nameof-instance-menbers"></a>nameof の微修正

[`nameof` 演算子](../start/st_string.md#nameof-operator)にちょっとした修正が入りました。

C# 11 以前だと、以下の例の最後の行のように、
静的メンバー内から「インスタンス メンバーのインスタンス メンバー」みたいな名前の参照ができなかったようです。

```csharp {title="C# 11 まではエラーになっていたコードの例" error-ranges="sha256:15785c8e6a478bc26b230251be71613ff8ccd4975b105f3324500ef2dc1dfa38;11:46-11:54" error-diagnostics="sha256:15785c8e6a478bc26b230251be71613ff8ccd4975b105f3324500ef2dc1dfa38;CS0120@11:46-11:54"}
class A
{
    public string? Instance { get; }

    // これは元から行けた。
    public string InstanceM() => nameof(Instance.Length);
    public static string StaticM1() => nameof(string.Length);
    public static string StaticM2() => nameof(Instance);

    // これが今までダメだったらしい。
    public static string StaticM() => nameof(Instance.Length);
}
```

これが、C# 12 ではコンパイルできるようになりました。

正直、バグ修正扱い(最新コンパイラーを使うと C# 11 以下でもコンパイルが通るようになる)でもいいレベルだとは思いますが、一応は C# 12 以上限定です。
