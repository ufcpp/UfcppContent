---
title: ".NET 6 Preview 7 & Visual Studio 2020 Preview 3"
source_url: "https://ufcpp.net/blog/2021/8/net6p7/"
content_type: "BlogEntry"
published_at: "2021-08-13T20:31:31"
updated_at: "2021-08-13T20:31:31"
tags: []
umbraco_id: 2355
parent_id: 2354
sort_order: 0
aliases: []
---

# .NET 6 Preview 7 & Visual Studio 2020 Preview 3

一昨日くらいに来てました。

* [Visual Studio 2022 Preview 3 now available!](https://devblogs.microsoft.com/visualstudio/visual-studio-2022-preview-3-now-available/)
* [Announcing .NET 6 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/)
* [Preview Features in .NET 6 – Generic Math](https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/)
* [String Interpolation in C# 10 and .NET 6](https://devblogs.microsoft.com/dotnet/string-interpolation-in-c-10-and-net-6/)
* [新しい C# テンプレート](https://docs.microsoft.com/ja-jp/dotnet/core/tutorials/top-level-templates)
* [ASP.NET Core updates in .NET 6 Preview 7](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-7/)
* [Announcing .NET MAUI Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-maui-preview-7/)

当日、このネタでライブ配信:

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/5m2qiJ24tqI" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

「一気に情報が来ても小一時間では話しきれない」って感じで極々一部しか話せませんでしたが。

「Visual Studio 2020 Preview 3 の方が CDN トラブルで配信が1日延期」というトラブルに見舞われ、
「SDK だけを先に .NET 6 Preview 7 に上げてしまうと、標準のテンプレートがコンパイル エラーを起こす」という事件もありましたが、1日経って問題は解消済みです。

とりあえず、ブログとしては「今回入った C# 10.0 機能」の話を書こうと思います。
ちなみに、今回の更新でほぼ C# 10.0 の全機能が入っています。
(1個だけまだなものがあるけども、「10.0 リリース時点で preview 機能として残る」判定を受けている機能なので、非 preview な 10.0 機能は全部 merge 済み。)

(全機能一覧は[トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/342) を立ててるので現状そちらを見ていただけると。)

## .NET 6 Preview 7 での C# 10.0 新機能

[Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md)で Merged into 17.0 と 17.0p3 になっているやつが今回入っています。
(17.0 になってる2つはもっと前に入ってた疑惑ちょっとあり。 Visual Studio 2020 Preview 2.1 のときかも。)

以下の6つ。

* [Improved Definite Assignment](#definite-assignment)
* [Extended property patterns](#property-pattern)
* [Interpolated string improvements](#interpolated-string)
* [File-scoped namespace](#file-scoped-namespace)
* [Parameterless struct constructors](#parameterless-ctor)
* [Caller expression attribute](#caller-expression)

あと、[Lambda improvements](#lambda) も1個前の Preview では動いていなかった機能が増えているので、合計7つ。

### <a id="definite-assignment">Improved Definite Assignment</a>

C# には元々、確実な代入ルールってのがあって、「未初期化変数から未定義な値を取り出す」みたいなことはできない仕様になっています。

```csharp {title="未初期化変数を触らせない"}
int x;

Console.WriteLine(x); // コンパイルエラー

if (int.TryParse(Console.ReadLine(), out x))
{
    // ここでは x が初期化済みな保証があるのでエラーが消える。
    Console.WriteLine(x);
}
```

これのためのフロー解析に改善の余地があることが周知の事実で長らく手つかずだったんですが、それが C# 10.0 でちょっと改善します。

これまで [`?.`](../../../../study/csharp/resource/rm_nullusage.md#null-conditional) とか [`??`](../../../../study/csharp/resource/rm_nullusage.md#null-coalesce) とか [`? : `](../../../../study/csharp/start/st_operator.md#condition) が絡むときの解析が甘くて、過剰にエラーになっていました。
それが緩和されて、例えば、以下のようなコードがコンパイルできるようになっています。

```csharp {title="?. が絡むときの確実な代入判定の改善例"}
using System.Diagnostics.CodeAnalysis;

m(null);
m(new R<string>(null));
m(new R<string>("abc"));

void m(R<string>? x)
{
    if (x?.TryGetValue(out var v) == true) // ここの var v の definite assignment 判定が改善された。
    {
        Console.WriteLine(v.Length); // 前までこの行がエラーになってた(C# 10.0 から OK に)。
    }
    else
    {
        Console.WriteLine("null");
    }
}

record class R<T>(T? Value)
{
    public bool TryGetValue([NotNullWhen(true)] out T value)
    {
        if(Value is { } v)
        {
            value = v;
            return true;
        }
        else
        {
            value = default!;
            return false;
        }
    }
}
```

### <a id="property-pattern"> Extended property patterns</a>

[プロパティ パターン](../../../../study/csharp/datatype/patterns.md#property)で、
多段のメンバーを `.` でつないでマッチングできるようになりました。

```csharp {title="多段プロパティ パターン" highlight-text="X.Value.Length"}
var x = new A(new B("a"));

if (x is A { X.Value.Length: 1 })
{
    Console.WriteLine("len 1");
}

record A(B X);
record B(string Value);
```

### <a id="interpolated-string"> Interpolated string improvements</a>

[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)のパフォーマンスが大幅に向上します。

以下のようなコードがあったとして、

```csharp {title="文字列補間の例"}
Console.WriteLine(m(1, 2, 3, 4));

string m(int a, int b, int c, int d) => $"{a}.{b}.{c}.{d}";
```

これまでは `string.Format("{0}.{1}.{2}.{3}", new object[] { a, b, c, d })` に展開されていました。
それが、所定の条件を満たせば(普通にやってれば .NET 6 をターゲットにして C# 10.0 でコンパイルすると)、以下のようなコードに変化します。

```csharp {title="パフォーマンス改善結果"}
var h = new System.Runtime.CompilerServices.DefaultInterpolatedStringHandler(3, 4);
h.AppendFormatted(a);
h.AppendLiteral(".");
h.AppendFormatted(b);
h.AppendLiteral(".");
h.AppendFormatted(c);
h.AppendLiteral(".");
h.AppendFormatted(d);
return h.ToStringAndClear();
```

ちなみに、C# コンパイラーのレベルで頑張っていることなので再コンパイルが必要です。
これに関しては「既存のコンパイル済みプログラムを .NET 6 で動かすだけで速くなる」みたいなことはないです。

### <a id="file-scoped-namespace"> File-scoped namespace</a>

いままで:

```csharp {title="{} 名前空間"}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class A
    {
    }
}
```

これから:

```csharp {title="1行名前空間"}
namespace ConsoleApp1;

class A
{
}
```

「たかが1インデント」と言われてたやつなんですが…
まあ確かにこの1インデントが深い言語の方が、今となっては少なく。

### <a id="parameterless-ctor"> Parameterless struct constructors</a>

[さかのぼること C# 6.0 の時に、`Activator` のバグでできなかったやつ](../../2/parameterlessstructctor/index.md)、再チャレンジ(成功)。

構造体のフィールドでも非 null 保証とかがやりやすくなります。

```csharp {title="構造体の引数なしコンストラクターの例"}
struct A
{
    public string S { get; } = "abc"; // 前まで初期化子を書けなかった
}

struct B
{
    public int[] Array { get; }
    public B() => Array = new int[4]; // 前まで B() を書けなかった
}
```

まあ、`default` からは逃げられないんですが…

```csharp {title="参照型の null 問題と同程度にやっかいな default 問題"}
// これは大丈夫。引数なしコンストラクターで new int[] されてる。
Array4 a = new();
Console.WriteLine(a[0]);

// default は引数なしコンストラクターを呼ばない。
a = default;
Console.WriteLine(a[0]); // ぬるぽ

struct Array4
{
    private readonly int[] _array;
    public Array4() => _array = new int[4];
    public int this[int index] => _array[index];
}
```

### <a id="caller-expression"> Caller expression attribute</a>

[CallerInfo 系の属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)に新しい仲間が増えました。

`CallerArgumentExpression` 属性で、「引数に渡した式」を取れるようになります。

```csharp {title="CallerArgumentExpression の例"}
using System.Runtime.CompilerServices;

m(2 * 3 * 4); // 2 * 3 * 4 = 24

var (x, y, z) = (1, 2, 3);
m(x + y + z); // x + y + z = 6

static void m(int result, [CallerArgumentExpression("result")] string? expression = null)
{
    Console.WriteLine($"{expression} = {result}");
}
```

主にロギング用途になると思います。

### <a id="lambda"> Lambda improvements</a>

.NET 6 Preview 6 時点で以下のようなコードは書けていたんですが。

```csharp {title="Delegate にラムダ式を代入"}
Delegate f = int (int x) => x * x;
```

Prevew 7 から以下のようなコードも書けるようになりました。

```csharp {title="ラムダ式の自然な型を自動決定"}
var f = int (int x) => x * x;
```

この場合、`f` の型は `Func<int, int>` になります。
`System.Action` か `System.Func` が使える場合にはそれを、
使えない場合には internal なデリゲート型をコンパイラー生成して使うそうです。

デリゲートの仕様上、以下のような挙動をするのでその点には注意が必要です。

```csharp {title="ラムダ式の自然な型の罠の例"}
// これは target-typed 型決定で、Predicate<int> になる(コンパイル可)。
m(x => x == 0);

// 一方で、これは f の型が Func<int, bool> になる。
var f = (int x) => x == 0;
m(f); // Func<int, bool> を Predicate<int> に変換でしません(コンパイル エラー)。

static void m(Predicate<int> f) { }
```
