---
title: "ピックアップRoslyn 7/11: Design Notes"
source_url: "https://ufcpp.net/blog/2018/7/pickuproslyn0711-2/"
content_type: "BlogEntry"
published_at: "2018-07-11T23:26:30"
updated_at: "2018-07-12T01:06:46"
tags: []
umbraco_id: 2161
parent_id: 2159
sort_order: 1
aliases: []
---

# ピックアップRoslyn 7/11: Design Notes

6月くらいからの C# Design Notes 追加。

- [https://github.com/dotnet/csharplang/pull/1705](https://github.com/dotnet/csharplang/pull/1705)

いくつかは、提案文書の方が先に出てたので、先にブログを書いてあるものの原案みたいなもの。

- [Working with Data](../../6/pickuproslyn0630/index.md)
- [using patterns and declarations](../pickuproslyn0711/index.md)

その他に関して。

## nullable reference types

- [May 30](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-05-30.md)
  - 「アノテーションを何もつけていないと非 null 扱い」(unannotated reference types to be non-nullable; "URTANN")に関して
  - URTANN 動作の opt-out ができないと、C# 7.X 以前のコードが警告だらけになる
      - 現状の Roslyn にそのまま適用すると2000個くらいの警告が出るらしい
  - デフォルト挙動は URTANN(true) にしたい
  - 細かい粒度(特定のファイルだけ、特定のクラスだけ、特定のメソッドだけ)で URTANN の opt-in/out 切り替えができるようにしたい
  - 切り替えはたぶん属性でやる。`NonNullTypesAttribute(bool)`
- [Jun 4](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-06-04.md)
  - ラムダ式にキャプチャした変数の null フロー解析、ちょっと特殊になりそう

## 式ツリー

- [Jun 6](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-06-06.md)

式ツリーで使える文法を増やしたいという話は前々からあるものの、やっと検討が始まったっぽい。

前半は需要のあるシナリオについて。Big data に対するクエリを式ツリーで送りたいとか、機械学習ライブラリ方面で[自動微分](https://ja.wikipedia.org/wiki/%E8%87%AA%E5%8B%95%E5%BE%AE%E5%88%86)とか GPU 上でのコード実行したいとか。

後半は実現方法に関して。

- 既存の、Entity Frameworkとかはノードを追加されても使えない。どのノードが使えるかはライブラリごとに異なる。どう制限するか
  - 案1: [非同期メソッドでやってるみたいに](../../../../study/csharp/async/sp5_async.md#task-like)、builder を介して任意の型でツリー構築できるようにする
  - 案2: アナライザーを使って制限する
- reduction (await をその展開結果である AsyncStateMachine の行動に変換したりとか、そういうノード変形)はやるかどうか
  - あまりしない方がよさそう
- C# の最新機能全部に、式ツリーが常に100%追従しようとは思っていない

とか言う感じ。あと、一応、プロトタイプ実装あり。

- [https://github.com/bartdesmet/ExpressionFutures/](https://github.com/bartdesmet/ExpressionFutures/)

## Target-typed new

- [Jun 25](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-06-25.md)
  - オーバーロード解決には寄与させないつもり
  - `S? s = new ();` みたいに書くとき、`new S()` の意味にする(`new Nullable<S>()`、つまり、null の意味にはしない)
  - `new ()` は型を持たない。`var x = new ();` みたいなのは型が確定しなくてエラー
  - `new ()` とか `new {}` とかは認めるけど、`new` 単体は認めないつもり
  - `dynamic`型がターゲットの`new ()`も認めない

オーバーロードに関しては、例えば以下のような話。

```csharp
struct S1 { public int x; }
struct S2 { }

void M(S1 s1) { }
void M(S2 s2) { }

void X()
{
    M(new() { x = 43 });
    //↑ x を持ってるのは S1 だけだから、S1 と推論できる
    // でも、それをやっちゃうと、S2 に後から x を追加することで破壊的変更が起きちゃう
    // なので、こういうオーバーロード解決はやらない
}
```

## null 条件演算

- [Jul 16](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-07-16.md)

`?.` とか `??` 系統の機能をいくつか追加。

- `x ??= y` で、`x = x ?? y;`
- `await? t` で、`if (t != null) await t;`
- ポインターに対しても`p?[a]`、`p?->a`、`p ?? q`
