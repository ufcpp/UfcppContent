---
title: "ピックアップRoslyn 4/27 & Visual Studio 16.1 Preview 2"
source_url: "https://ufcpp.net/blog/2019/4/pickuproslyn0427/"
content_type: "BlogEntry"
published_at: "2019-04-27T19:11:38"
updated_at: "2019-04-27T19:11:38"
tags: []
umbraco_id: 2240
parent_id: 2236
sort_order: 3
aliases: []
---

# ピックアップRoslyn 4/27 & Visual Studio 16.1 Preview 2

## Visual Studio 16.1 Preview  2

[Visual Studio 16.1 Preview 2](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes-preview#16.1.0-pre.2.0) が来てますね。

C# 8.0、Preview 1 からの差分だと、以下の2つの機能が追加されました。

- [インターフェイスのデフォルト実装](https://github.com/dotnet/csharplang/blob/master/proposals/default-interface-methods.md)
- [readonly 関数メンバー](https://github.com/dotnet/csharplang/issues/1710)

### インターフェイスのデフォルト実装

インターフェイスに、実装を持った関数メンバー(メソッド、プロパティ、インデクサーなど)を持てるようになりました。

どちらかというとライブラリ実装者向けの機能で、多くの開発者(大体はライブラリは使う側)にとってはそこまで大きなインパクトのある機能ではないんですが。
[先日のイベント登壇](https://www.slideshare.net/ufcpp/c-80-preview-in-visual-studio-2019-160/27)でも話した通り、この機能の重要な点は、「.NET ランタイム側の修正が必須」という部分です。

古いランタイムでは動かない機能追加というのがいつ以来かというと、[C# 2.0](../../../../study/csharp/cheatsheet/ap_ver2.md)、つまり、2005年以来14年ぶりの出来事です。

状況的に言うと、

- .NET Core が単なる「.NET Framework からの移行期」をやっと超えた
- .NET Core なら新しいランタイムを採用しやすい
  - side by side インストールができるので、新旧ランタイムの混在ができる
  - インストーラーも小さい
- WPF や WinForsm など、Windows 限定機能も .NET Core 3.0 で動くようになって、 .NET Framework にこだわる必要性が減った(少なくとも新規案件では採用理由がない)

という感じで、ランタイム側の修正がやりやすい状況がやっと来ました。
この3・4年の間、「それは C# コンパイラーだけでは無理だから採用できない」と言われて実装されてこなかったような機能が、今後は採用される可能性が高まっています。

### readonly 関数メンバー

「[in 引数を使ってもコピーが発生する場合](../../../../study/csharp/resource/sp_ref.md#in-copy)」で説明している「隠れたコピー」(hidden copy)を回避するための機能です。

関数メンバーに readonly 修飾を付けることで、その実装内でフィールドを書き換えていないという保証をします。結果的に、「書き換わったら困るので防衛的にコピーしてからメソッドを呼ぶ」みたいな挙動がなくなって、パフォーマンスが向上します。

とりあえず、フィールドの書き換えをしていないメソッドとかには readonly を付けておけばいいです。
特に、構造体に対してはこの隠れたコピーがよく問題を起こすので、意外と重要な機能です。

ただ、[ref readonly](../../../../study/csharp/resource/sp_ref.md#ref-readonly) と似てて、ちょっとした語順の差で別の機能になったりするので注意が必要です。
`readonly ref T M<T>()` なら readonly 関数メンバーで、`ref readonly T M<T>()` なら参照戻り値です。

## Design Notes

また2件、C# Design Notes 追加。

- [Added: LDM Notes for April 22 and April 24, 2019 #2471](https://github.com/dotnet/csharplang/issues/2471)

null 許容参照型(NRT)がらみの話3件と、switch 式の「target-typed」の話、非同期イテレーターへの `CancellationToken` の渡し方の話。

### null 許容参照型がらみ

- [finally ブロック](../../../../study/csharp/structured/oo_exception.md#try)内での null チェックの結果は、その後ろでも有効かどうか
  - 有効であるべきなんだけど、フロー解析が複雑になる(= コンパイル時間が増える)
  - 簡単にできる範囲でフロー解析に手を加えることに
- [partial 定義](../../../../study/csharp/oop/oo_class.md#partial)で nullability が違う指定をしてしまったとき、どう扱うべきか
  - 違う nullability  指定をしてたらエラーにする
- ジェネリック型引数では `MaybeNull` 属性が必要そう
  - `T FirstOrDefault<T>(IEnumerable<T>)` の戻り値が代表例で、制約なしのジェネリックなので `T?` とは書けないけど、null を返す可能性があるものに付ける

### switch 式がらみ

[switch 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)に関しては、

```csharp {title="terget-typed switch 式"}
byte M(bool b) => b switch { false => 0, true => 1 };
```

みたいに書いたとき、この 0, 1 をちゃんと `byte` と判定できるようにしたいという話。
(現在(16.1 Preview 1時点)では、switch の戻り値が `int` になって、`int` から `byte` への暗黙の型変換はダメと怒られる。)
戻り値側からの型推論なので「target-typed」。

同じことは条件演算子 `b ? 0 : 1` でもやりたいけども、既存のコードを壊さないようにするために、switch 式ほどは自由度効かなさそう(それでも、影響を最小限に抑えられそうな範囲で検討中)とのこと。

### 非同期イテレーターがらみ

[ちょっと前](https://github.com/dotnet/csharplang/issues/2447)からの決定事項なんですが、
非同期イテレーター(`await`と`yield return`の混在)に対する `CancellationToken` の渡し方は、引数に属性を付けることでやりたい、(属性名は本決定してないけど仮に)以下のような書き方をするという話があります。

```csharp {title="非同期イテレーターへの CancellationToken の渡し方"}
async IAsyncEnumerable<T> M<T>([DefaultCancellation] CancellationToken cancellationToken)
{
    ...
}
```

これに関して、

- 非同期イテレーターになっている実装自体でないものに属性が付いていると警告を出す
  - 要するに、abstract で `IAsyncEnumerable<T>` を返しているメソッドとかに属性が付いていると警告になる
- 複数の `CancellationToken` 引数に属性が付いていても警告は出さない
- 1つも`CancellationToken` 引数に属性が付いていない場合は警告を出す

みたいにしたいみたい。
