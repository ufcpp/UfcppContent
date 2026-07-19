---
title: "ピックアップRoslyn 12/14"
source_url: "https://ufcpp.net/blog/2016/12/pickuproslyn1214/"
content_type: "BlogEntry"
published_at: "2016-12-15T00:22:37"
updated_at: "2016-12-27T14:34:13"
tags: []
umbraco_id: 1997
parent_id: 1969
sort_order: 15
aliases: []
---

# ピックアップRoslyn 12/14

小ネタ休んだついでにピックアップRoslynも。

Visual Studio 2017の正式リリースまではバグ修正くらいしか作業しない段階に来てるんで大したネタはない…
と思っていた時期がありました。

まあ、小ネタ程度の話はあった…

## whileとforのスコープ変更

まあ、既存動作を壊す変更ではないんですが、`while`と`for`の仕様書上の記述を変更しなきゃという話が出ています。

- [Change scoping of expression variables for while, for #15630](https://github.com/dotnet/roslyn/issues/15630)

[型スイッチ](../../../../study/csharp/datatype/typeswitch.md)と[out var](../../../../study/csharp/resource/sp_ref.md#out-var)のせいで、whileやforの条件式や更新式の中で変数を作れるようになりました。
その変数のスコープはどうなるべきかというのを考えたときに、`while`や`for`の展開結果に関する記述をちょっと変更する必要があるっていう話です。

### while

まず`while`。

```csharp
while (<cond>) <body>
```

こういう`while`ステートメントがあったとき、これまでだと、以下のように展開するという仕様になっていました。

```csharp
continueLabel:;
if (!<cond>) goto breakLabel;
{
    <body>
}
goto continueLabel;
breakLabel:;
```

これが、以下のように変わります。`{ }` が1段増える。

```csharp
continueLabel:;
{
    if (!<cond>) goto breakLabel;
    {
        <body>
    }
    goto continueLabel;
}
breakLabel:;
```

要するに、条件式の中で宣言された変数は、`while`の外には漏らさないよというルールの追加です。

(現状のRC版はルール変更前の実装になってる。RTMまでにたぶん変わる。)

### for

同様に、`for`。

```csharp
for (<decl>; <cond>; <incr>) <body>
```

この`for`ステートメントは、以下のような仕様になっていました。

```csharp
{
    <decl>
    while(<cond>)
    {
        <body>
    continueLabel:;
        <incr>
    }
}
```

これが以下のように変更。更新式(`<incr>`のところ)に `{ }`が増えます。

```csharp
{
    <decl>
    while(<cond>)
    {
        <body>
    continueLabel:;
        { <incr> }
    }
}
```

更新式のところで宣言した変数はその中でだけ使えて、`for`ステートメントのbody内でも参照できなくするということのようです。

## Design by Contract のコミュニティ実装

C# チーム的には「メリットの割には構文が煩雑になりすぎる」と、今まだちょっと及び腰になっている Design by Contract がらみの構文ですが、
しびれを切らした人が自前実装を始めた模様。

- [Lightweight design-by-contact for C#6](https://github.com/JamesFaix/Traction)

実装的には、[StackExchange.Precompilation](https://github.com/StackExchange/StackExchange.Precompilation)を使ったビルド時コード書き換えみたいです。なので、実行時コストは高くないはず。

まあ、ビルド時コード書き換えはそこそこはまりどころもあるので、どうしてもDbCが欲しいという人にしかあんまりお勧めはできませんけども…
C#に公式に機能追加されるまでのつなぎとしてはいいかも。
