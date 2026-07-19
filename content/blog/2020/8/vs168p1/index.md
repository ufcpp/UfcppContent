---
title: "Visual Studio 16.7 & 16.8 Preview 1 リリース / C# 9.0 の新機能3つ(module initializers, static lambda, target-typed conditional)"
source_url: "https://ufcpp.net/blog/2020/8/vs168p1/"
content_type: "BlogEntry"
published_at: "2020-08-10T21:02:46"
updated_at: "2020-08-10T21:39:50"
tags: []
umbraco_id: 2308
parent_id: 2307
sort_order: 0
aliases: []
---

# Visual Studio 16.7 & 16.8 Preview 1 リリース / C# 9.0 の新機能3つ(module initializers, static lambda, target-typed conditional)

5日に、Visual Studio 2019 の 16.7 と、16.8 Preview 1 がリリースされました。

- [Visual Studio 2019 v16.7 and v16.8 Preview 1 Release Today!](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-v16-7-releases/)

ということで、先週、ライブ配信もしていました。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/-_QGGvT5FEw" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

16.7 が正式リリースになった記念に、Preview の頃に触れてた話題を改めてちょこっと振り返ったのと、16.8 Preview 1 で新たに追加された C# 9.0 の3つの機能の話でした。

C# 9.0 に今回追加されたのは以下の3つです。

- [Module Initializers](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-9.0/module-initializers.md)
- [Static anonymous functions](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-9.0/static-anonymous-functions.md)
- [Target-Typed Conditional Expression](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-9.0/target-typed-conditional-expression.md)

今日は主にこの3つについて説明。

## Module Initializers

モジュール(exe (アプリ)や dll (ライブラリ))が読み込まれた時点で必ず1回呼ばれるメソッドを書けるようになりました。

以下のように、`ModuleInitializer` 属性を付けた静的メソッドが、モジュール読み込み時に呼ばれます。

```csharp
using System;
using System.Runtime.CompilerServices;

class Init
{
    [ModuleInitializer]
    internal static void M1() => Console.WriteLine("Init.M1");

    [ModuleInitializer]
    internal static void M2() => Console.WriteLine("Init.M2");
}
```

[静的コンストラクター](../../../../study/csharp/oop/oo_static.md#ctor)でも近いことができるんですが、

- 静的コンストラクター
  - そのクラスのメンバーに触れた時点で初めて呼ばれる
      ‐ 1度も使っていないクラスの静的コンストラクターは結局呼ばれない
  - 1つのクラスに1つ限り
- Module Initializers
  ‐ クラスのメンバーを使っていようが使っていまいが、モジュール読み込み時に必ず呼ばれる
  ‐ 1クラスに複数持てる

みたいな差があります。
確実に、確定タイミングで呼ばれるというのもメリットですし、個別に静的コンストラクターを持つよりはちょっとだけパフォーマンス的にも都合がいいみたいです。

今、[Source Generator](../../5/sourcegenerator/index.md)って機能の実装も進められていて、これが入ると、たぶん「各クラスについて1回限り走らせたい処理」みたいなものは結構あると思います。
例えば自分が必要に迫られているものだと、[リフレクションが使えない環境で自前でリフレクションに代わる型情報を持つ](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/12#issuecomment-669958700)みたいなコードなんですけども、
これが「確実に、確定タイミング」になってくれるのは結構ありがたかったりします。

## Static anonymous functions

[匿名関数](../../../../study/csharp/functional/fun_localfunctions.md#anonymous-function) ([ラムダ式](../../../../study/csharp/functional/sp_delegate.md#lambda)と[匿名メソッド式](../../../../study/csharp/functional/sp_delegate.md#anonymous-method))に対して `static` 修飾を付けて、[キャプチャ](../../../../study/csharp/functional/sp2_anonymousmethod.md#closure)の抑止ができるようになりました。

```csharp
using System;

// OK
Action staticLambda = static () => { };
Action staticAnonymousMethod = static delegate () { };

// コンパイル エラー
int local = 1;
Action badStaticLambda = static () => Console.WriteLine(local);
Action badStaticAnonymousMethod = static delegate () { Console.WriteLine(local); };
```

これは割かし、「工数的な問題で 8.0 に入らなかっただけ」系の機能です。
C# 8.0 時点で[ローカル関数に関しては同様の機能](../../../../study/csharp/functional/fun_localfunctions.md#static-local-function)が入っていて、
匿名関数でも同様の需要があることはわかっていましたが、
文法的にちょっとめんどくさいので後回しになっていたものです。

## Target-Typed Conditional Expression

条件演算子 (`? :`)で、第2項と第3項で共通の型を決められないときに、[ターゲット型](../../../../study/csharp/start/misctyperesolution.md#target-type)を見て型を決定できるようになりました。

```csharp
void targetTypedConditional(bool x)
{
    // target-typed で、1 : null の部分がちゃんと int? になる。
    int? v1 = x ? 1 : null;

    // あくまで target-typed で判定してるので、以下のような推論は働かない(コンパイル エラー)。
    // 1 と null の「共通型」は確定できない。
    //var v2 = x ? 1 : null;
}
```

[`switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)の場合には C# 8.0 時点であった機能です。新しい文法である `switch` 式と違って、既存の文法に手を入れるのはリスクもある(というか、実際、ちょっと破壊的変更を起こしてる)ので 8.0 には間に合わなかった機能です。
