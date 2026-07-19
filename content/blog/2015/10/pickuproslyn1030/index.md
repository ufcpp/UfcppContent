---
title: "ピックアップRoslyn 10/30"
source_url: "https://ufcpp.net/blog/2015/10/pickuproslyn1030/"
content_type: "BlogEntry"
published_at: "2015-10-30T12:10:25"
updated_at: "2015-10-30T12:10:25"
tags: []
umbraco_id: 1809
parent_id: 1800
sort_order: 4
aliases: []
---

# ピックアップRoslyn 10/30

## Proposal: extension everything #6136

[Proposal: extension everything #6136](https://github.com/dotnet/roslyn/issues/6136)

コンセプト自体は前々から出ているやつの具体的な文法案。拡張メソッド以外にも、プロパティとかその他のメンバーも拡張で足せたり、静的メソッドとかの追加もできたりさせたいという話。

結構争点多くてまだまだまとまらなさそう。

- `extension class A` みたいに拡張したいクラスだけ指定するか、`extension class AExtension : A` みたいに、拡張メソッドを定義するクラス名を書くか
- 拡張側にインスタンスメンバーを持てるべきかどうか。持てるようにするには`ConditionalWeakTable`みたいなパフォーマンスに悪影響のある仕組みを使わざるを得なくて微妙
- 既存の拡張メソッドみたいに、静的メソッドの引数として明示的にインスタンスを渡す構文が好きなんだけど、同じように明示的な感じにはできないか

## Proposal: Sequence Expressions #6182

[Proposal: Sequence Expressions #6182](https://github.com/dotnet/roslyn/issues/6182)

`(var x = GetX(); x * x)` みたいな感じに、`()`内にステートメントを並べて、最後の1式の結果を返す式を提案。

パターンマッチングに対応したswitchステートメントを作るのに使いそう。

`,`区切りじゃなくて`;`区切りなのは、メソッドの引数リストと区別がつかない場合があり得るから。

## Proposal: out var declaration #6183

[Proposal: out var declaration #6183](https://github.com/dotnet/roslyn/issues/6183)

out 引数のところに、`M(out var x);` みたいに変数宣言混ぜれるようにする話。

一瞬C# 6.0に入りかけて、「もうちょっとしっかり考えたいから先送り」になってたやつ。

これも、パターンマッチング構文で使う前提。

## Proposal: Destructuring assignment for tuples and other types #6400

[Proposal: Destructuring assignment for tuples and other types #6400](https://github.com/dotnet/roslyn/issues/6400)

コンパイル時に確定でマッチするようなパターンに対して、コンパイル時チェック付きでパターンマッチングできるようにするやつ。今のところ、これのためにletキーワードを導入しようという雰囲気。

これで、タプルの分解とかの、いわゆる「Destructuring」ができる。`let (var x, var y) = tuple;` みたいなの。

タプル以外にも、任意のパターンマッチングを使ってDestructuring可能。`let Destination { Place is Coordinates { Longitude is var longitude } }` みたいな書き方。
