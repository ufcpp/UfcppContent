---
title: "【C# 12 候補】ラムダ式のデフォルト引数と params 引数"
source_url: "https://ufcpp.net/blog/2023/1/lambda-default/"
content_type: "BlogEntry"
published_at: "2023-01-11T22:10:34"
updated_at: "2023-02-09T21:04:22"
tags: []
umbraco_id: 2450
parent_id: 2449
sort_order: 0
aliases: []
---

# 【C# 12 候補】ラムダ式のデフォルト引数と params 引数

そろそろ、C# vNext 候補で上がってるものをちらほら紹介していこうかと。

今日は割かし確度高そうなものとして、ラムダ式がらみの話。
ラムダ式でもデフォルト引数と params への対応を考えているそうです。

提案ドキュメント:

* [Optional and parameter array parameters for lambdas and method groups](https://github.com/dotnet/csharplang/blob/main/proposals/lambda-method-group-defaults.md)

※追記: 後から気づきましたが、この機能は Visual Studio 17.5 Preview 2 (2022年12月中旬)の時点ですでに使えてたっぽいです。
(未確認。少なくとも Preview 3 (2023年1月中旬) では使えます。[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) preview 必要。)

## C# 10 のときの話

[C# 10 のときにラムダ式の改善](../../../../study/csharp/cheatsheet/ap_ver10.md#lambda-improvement)がいくつか入りました。
以下のように、Web アプリがシンプルに書けるようになります。

```csharp {title="C# 10 のラムダ式の改善"}
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// MapGet の引数は System.Delegate 型。
// Delegate に対してラムダ式が使える。
// 自然な型決定が働いて、この場合は Func<string> になる。
app.MapGet("/", () => "Hello World!");

app.Run();
```

この機能の延長で、

* ラムダ式の引数にデフォルト値を与えられるように
* ラムダ式の引数を params にできるように

の2つが追加で提案されています。

## これまでのラムダ式の引数

C# 9 までの状態だと、
ラムダ式にデフォルト引数/params 引数が書けても役に立ちませんでした。

メソッドを使った例で説明すると、
以下のように、デフォルト引数/params 引数はデリゲート化する際に一切紛失します。

```csharp {title="デフォルト引数/params 引数はデリゲート化すると紛失"}
m();

// m() と呼べるのに、 Action には代入できない。
Action a1 = m;

// Action<int, int[]> には代入できるけど、
Action<int, int[]> a2 = m;

// Action<int, int[]> 越しには () では呼べない。
a2();

static void m(int x = 1, params int[] y) { }
```

デリゲートに代入して使うことが前提のラムダ式では、
そもそもデフォルト引数/params 引数を書けても全く役に立たないということになります。

そんな中、[C# 10 ではラムダ式への属性指定ができるようになった](../../../../study/csharp/functional/fun_localfunctions.md#lambda-csharp10)わけですが、静的な型情報からは消えるという意味ではこの属性も同様だったりします。
ただ、属性は、静的な情報としては紛失したとしても、
リフレクションを使って属性を取る前提であれば意味があります。

```csharp {title="リフレクションで取る情報としては意味があり、ラムダ式に属性を付ける意義はある"}
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

// f の型 (Func<string, string>) に FromBody 属性が反映されるわけではな。
Func<string, string> f = ([FromBody] string name) => "Hello World!";

// リフレクションで MethodInfo から引数や戻り値を取れば、それについてる属性を調べられる。
var p = f.Method.GetParameters()[0];

foreach (var a in p.GetCustomAttributes())
{
    // FromBodyAttribute
    Console.WriteLine(a.GetType().Name);
}
```

「リフレクションで」というのであれば、
デフォルト引数と params 引数も同様のはずです。

```csharp {title="リフレクションでデフォルト引数/params 引数を調べる例"}
using System.Reflection;
using System.Runtime.InteropServices;

Delegate f = C.M;

foreach (var p in f.Method.GetParameters())
{
    Console.WriteLine(p.Name);
    Console.WriteLine(p.GetCustomAttribute<OptionalAttribute>());   // x のときに取れる
    Console.WriteLine(p.GetCustomAttribute<ParamArrayAttribute>()); // y のときに取れる
}

class C
{
    public static void M(int x = 1, params int[] y) { }
}
```

## ラムダ式のデフォルト引数/params 引数を認める

ということで、C# 10 の時に属性を認めたのと同じく、
ラムダ式のデフォルト引数/params 引数を認めたいという話になりました。

そこから、もう1歩進めた提案もあって、
自然な型決定で、デフォルト引数/params 引数付きのデリゲートを作るという話もあります。

```csharp {title="ラムダ式のデフォルト引数/params 引数"}
static void m(int x = 1, params int[] y) { }

// 今までだったら Action<int, int[]> になってた。
// これを、 delegate void Anonymous(int x = 1, params int[] y) で生成したい。
var f = m;

// 今まででも、↓なら呼べる。
f(1, new[] { 2 });

m(1, 2);
m(x: 1);
m(y: 2);

// ↓はこれまではダメで、C# 12 でできるようにしたい。
f(1, 2);
f(x: 1);
f(y: 2);

Console.WriteLine(f.GetType());
```

割かし実装も進んでいるはずなので、これは近いうちにプレビューが来ると思われます。
