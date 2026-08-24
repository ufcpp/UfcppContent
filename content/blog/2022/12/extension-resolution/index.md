---
title: "拡張メソッドは暗黙型変換を見ない"
source_url: "https://ufcpp.net/blog/2022/12/extension-resolution/"
content_type: "BlogEntry"
published_at: "2022-12-08T23:07:13"
updated_at: "2022-12-08T23:07:13"
tags: []
umbraco_id: 2443
parent_id: 2438
sort_order: 2
aliases: []
---

# 拡張メソッドは暗黙型変換を見ない

こないだ、C# で [`(stackalloc T[N]).M()`](../stackalloc-natural-type/index.md) とか書けるという話を書いたわけですが。
その過程で出てきた「そういえばこんなのも」話をもう1個。

[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)の拡張メソッド呼びがちょっと変という話になります。

## 拡張メソッドの解決

[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)の存在意義は、
「語順を変更して、`x.M().N()` みたいな呼び出しができる」という点です。
ほとんどの場合は本当に「語順」だけの問題で、通常のメソッド呼び出しの形でも同じコードが書けます。

```csharp {title="同じメソッドを通常のメソッド呼び出しと拡張メソッド呼び出しの両方の書き方をする"}
// 拡張メソッド呼び。
1.M();

// 同じものを通常のメソッド呼び出しで書く。
Ex.M(1);

static class Ex
{
    public static void M(this int _) { }
}
```

ただ、まあ、通常のメソッド呼びと拡張メソッド呼びでは、ちょっとだけ「解決ルール」みたいなやつが違ったりします。

無変換の場合、つまり、
「`1` を `int` 引数に渡す」とか「`""` を `string` 引数に渡す」みたいなときには変な挙動はしないんですが、
問題は型変換が絡む場合です。

## 解決できる例

先に、大丈夫な例から行きます。

親クラスや、実装するインターフェイスへの変換は問題なく行けて、
拡張メソッド呼び出しもできます。

```csharp {title="親クラスや、実装しているインターフェイスへの変換"}
// 親クラスや、実装しているインターフェイスへの変換は、拡張メソッド呼び出しできる。
1.Object();
1.Interface();

Ex.Object(1);
Ex.Interface(1);

static class Ex
{
    public static void Object(this object _) { }
    public static void Interface(this IComparable _) { }
}
```

オーバーロードがあるときには
「階層が近い方優先」で、これも通常メソッド呼び・拡張メソッド呼びで共通です。

```csharp {title="オーバーロードは階層が近い方優先"}
// どっちも IComparable の方が呼ばれる。
1.M();
Ex.M(1);

static class Ex
{
    public static void M(this object _) => Console.WriteLine("object");
    public static void M(this IComparable _) => Console.WriteLine("IComparable");
}
```

## ユーザー定義の型変換

拡張メソッドの解決時、ユーザー定義の型変換はみません。
一方で、通常のメソッド解決の時には見るので、
「拡張メソッド呼びだけできない」みたいなことがあります。

標準ライブラリでいうと、`DateTime` → `DateTimeOffset` とか、
`Span<T>` → `ReadOnlySpan<T>` とか、
`string` → `ReadOnlySpan<char>` とかがあります。

```csharp {title="拡張メソッド呼びできない例: ユーザー定義の型変換"}
// 通常のメソッドとしてなら呼べる。
Ex.M("");                 // string → ReadOnlySpan<char>
Ex.M(stackalloc char[1]); // Span<char> → ReadOnlySpan<char>
Ex.M(DateTime.Now);       // DateTime → DateTimeOffset

// 拡張メソッドでは呼べない…
"".M();
(stackalloc char[1]).M();
DateTime.Now.M();

static class Ex
{
    public static void M(this ReadOnlySpan<char> _) { }
    public static void M(this DateTimeOffset _) {}
}
```

これがまあ、[こないだのブログ](../stackalloc-natural-type/index.md)とのつながりでして。
「`(stackalloc char[1]).M()` が呼べない？そうだっけ？」からの、
「`ReadOnly` を削ったら呼べた」ということがありました。

## ターゲットからの型推論

ターゲットからの型推論系の処理も、拡張メソッドでは働きません。

`new()`、`default` 辺りはダメです。

```csharp
// 通常のメソッドとしてなら呼べる。
Ex.M(new());      // new object()
Ex.M(default); // null

// 拡張メソッド前に型推論は働かない。
// エラーに。
new().M();
default.M();

static class Ex
{
    public static void M(this object? _) {}
}
```

## ターゲットからの型推論 + 自然な型

ターゲットからの型推論は効かないものの、
自然な型を持っているやつはどうなるかというと…

基本的に、自然な型の時だけは拡張メソッド呼びもできます。

```csharp {title="自然な型の時なら拡張メソッドが呼べる"}
// 通常のメソッドとして、当然呼べる。
Ex.M(1);
Ex.M($"{1}");

// 整数リテラルの自然な型は int で、 int の拡張メソッドなら呼べる。
1.M();

// 同、string の拡張メソッドなら呼べる。
$"{1}".M();

static class Ex
{
    public static void M(this int _) { }
    public static void M(this string _) {}
}
```

例えば整数リテラルは `short` や `byte` 型に変換できますし、
文字列補間 `$""` は `IFormattable` や文字列補間ハンドラーに変換できます。
ところが、こういう場合は拡張メソッド呼びできません。

```csharp {title="ターゲットからの型判定がかかるような例では拡張メソッドは呼べない"}
using System.Runtime.CompilerServices;

// 通常のメソッドとしてなら呼べる。
Ex.M(1);
Ex.M($"{1}");

// ターゲットからの型判定がかかるような例では拡張メソッドは呼べない。
1.M();
$"{1}".M();

static class Ex
{
    public static void M(this byte _) { }
    public static void M(this DefaultInterpolatedStringHandler _) {}
}
```

### ラムダ式の自然な型

ちなみに、自然な型決定できるようになったにもかかわらず、
ラムダ式は自然な型に対しても拡張メソッド呼びはできません。
これは意図的で、`() => {}.M()` みたいな文法を認めたくなかったみたいです。
`(() => {}).M()` でもダメ。 

```csharp {title="ラムダ式のときは自然な型に対しても拡張メソッド呼び不可"}
// これは行ける。
// 何なら Delegate とか object 引数相手でもこう書ける。
Ex.M(() => { });

// これはダメ。
// 自然な型は Action なはずだけど。
(() => { }).M();

static class Ex
{
    public static void M(this Action _) {}
}
```

### 特殊なオーバーロード解決順序

文字列補間のオーバーロード解決順序はちょっと特殊です。

C# 10 でパフォーマンス改善のために[ハンドラー パターン](../../../../study/csharp/start/improvedinterpolatedstring.md)を導入したわけですが、
その時に検討された内容:

* たいていのクラスがすでに `string` のオーバーロードを持っている
* 普通に考えれば `$""` の自然な型は `string` で、オーバーロード解決でも `string` 引数が優先されるべき
* ところが `string` オーバーロードが呼ばれたらパフォーマンス改善されない
* 何なら C# 6 で[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)を導入したときにも、`IFormattable` オーバーロードが呼ばれなくて困った

このような背景がありまして。
結果的に、「文字列補間ハンドラーがあれば、それを優先的に使う」という特殊処理が挟まっています。

```csharp
using System.Runtime.CompilerServices;

// ハンドラー優先の特殊処理が働く。
Ex.M($"{1}"); // interpolation の方が呼ばれる

static class Ex
{
    public static void M(string _) => Console.WriteLine("string");
    public static void M(DefaultInterpolatedStringHandler _) => Console.WriteLine("interpolation");
}
```

これは本当に特殊処理です。
例えば、整数リテラルの場合は普通に int が優先されます。

```csharp {title="int 優先"}
// 普通に考えれば「自然な型」優先。
// 実際、整数リテラルは int 優先。
Ex.M(1); // int が呼ばれる

// int におさまらない桁のリテラルを書くと long リテラルになって、long オーバーロードが呼ばれるのに。
Ex.M(0x1_0000_0000); // long が呼ばれる。

static class Ex
{
    public static void M(int _) => Console.WriteLine("int");
    public static void M(byte _) => Console.WriteLine("byte");
    public static void M(long _) => Console.WriteLine("long");
}
```

で、この `$""` に対する特殊処理が、拡張メソッド解決の際には働かないということは…
以下のように、通常メソッド呼びと拡張メソッド呼びで呼ばれるオーバーロードが変わるという症状を起こします。

```csharp {title="$&quot;&quot; の拡張メソッド呼び"}
using System.Runtime.CompilerServices;

// ハンドラー優先の特殊処理が働く。
Ex.M($"{1}"); // interpolation の方が呼ばれる

// そしてその特殊処理は、拡張メソッド解決時には働かない！
$"{1}".M(); // string の方が呼ばれる！

static class Ex
{
    public static void M(this string _) => Console.WriteLine("string");
    public static void M(this DefaultInterpolatedStringHandler _) => Console.WriteLine("interpolation");
}
```

特殊処理が挟まった背景を知らないと意味が分からない仕様ですよね。
一応、バグじゃなくて仕様通りです。
