---
title: "ピックアップRoslyn 3/23: no-allocation非同期メソッド、最近の Utf8String"
source_url: "https://ufcpp.net/blog/2018/3/pickuproslyn0323/"
content_type: "BlogEntry"
published_at: "2018-03-23T00:36:34"
updated_at: "2018-03-23T00:41:09"
tags: []
umbraco_id: 2140
parent_id: 2134
sort_order: 4
aliases: []
---

# ピックアップRoslyn 3/23: no-allocation非同期メソッド、最近の Utf8String

[昨日のDesign Notes祭り](../pickuproslyn0321/index.md)とはまた別件なんですが、こんな提案が。

## メソッド単位でAsyncMethodBuilder属性

- [Proposal: Allow [AsyncMethodBuilder(...)] on methods #1407](https://github.com/dotnet/csharplang/issues/1407)

[corefx](https://github.com/dotnet/corefx/)/[coreclr](https://github.com/dotnet/coreclr/)方面でガッチガチのパフォーマンス改善をやりまくってる人からの提案。

タイトルからは内容がちょっとわかりにくいんですが、非同期メソッドの際に必要になるヒープ確保量を0 (no-allocation)にするためにこの機能が必要とのこと。

背景としては、

- C# 6.0: 非同期メソッドの戻り値は常に`Task` → `Task` のヒープ確保が必須でつらい → C# 7.0: [`ValueTask<T>`を認めた](../../../../study/csharp/async/sp5_async.md#task-like)よ
- `ValueTask<T>`、同期で終わるときにはヒープ確保0だけど、実際に非同期だとどうしても`Task`が発生 → [最近Mergeされたコミット](https://github.com/dotnet/coreclr/pull/16618): `IValueTaskSource`ってインターフェイスを使うことで、`Task`を介さず非同期処理できるようにしたよ。うまく使えば`Task`分のヒープ確保がなくなる
- `IValueTaskSource`で、`await`毎のヒープ確保はなくせるけども、AsyncStateMachine の1インスタンスはどうしても残った → そのインスタンスをキャッシュして持ちたい

という感じ。

最終的に、ヒープ確保せざるを得ないインスタンスをキャッシュして、プログラム中で1個だけ(メソッド呼び出しのたびには新規ヒープ確保しない)とかにしたいわけですけども、キャッシュはうまくやらないと効率が悪くて困ることになります。

なので、メソッド単位でどういう非同期メソッド生成するかをカスタマイズできたり、AsyncStateMachine に`this`を渡せたりしないとこれ以上の最適化が厳しいみたいで、やっと提案のタイトルにつながります。

`AsyncMethodBuilder`属性をメソッド単位で指定して、メソッドごとに非同期メソッドの挙動をカスタマイズできるようにしたいっていう話は昔からあったんですけども、需要が低いということで実装されずにいたんですけども。
こういう方面から改めて需要が出てくるとは…

## UTF8文字列がらみ

- [Introducing System.Rune #24093 (Comment)](https://github.com/dotnet/corefx/issues/24093#issuecomment-375300540)

去年の9月くらいに[Go (Unicode Code Point のことを rune って呼んでる)に倣って Rune 型を作らない？](../../../2017/12/fastutf8string/index.md)みたいな提案が立っていましたが。
Rune って名前の評判悪すぎて、まあ、別の名前に落ち着きそう。

あと、C# のキーワードも用意したいみたいです。

| C# キーワード | 実際の型名 | サイズ(Byte) | 概要 |
| --- | --- | --- |
| `ubyte` | `System.CodeUnit` | 1 | UTF-8エンコーディングされてる文字列の1バイト1バイト |
| `uchar` | `System.CodePoint` | 4 | Unicode のコードポイント  |
| `ustring` | `System.Utf8String` |  | 内部的に UTF-8 でデータを持つ文字列クラス |
| `uspan` | `System.Utf8Span` |  | `ReadOnlySpan<ubyte>`で、`ustring`の一定区間を参照する構造体 |

まあまだ正式な API の提案になってる段階ではないので、まだ変わると思いますが。
