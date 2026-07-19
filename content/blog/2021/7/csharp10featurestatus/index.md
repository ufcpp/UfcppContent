---
title: "C# 10.0 に入れるかどうか確定させる時期が来たようです"
source_url: "https://ufcpp.net/blog/2021/7/csharp10featurestatus/"
content_type: "BlogEntry"
published_at: "2021-07-14T23:32:43"
updated_at: "2021-07-14T23:32:43"
tags: []
umbraco_id: 2352
parent_id: 2351
sort_order: 0
aliases: []
---

# C# 10.0 に入れるかどうか確定させる時期が来たようです

今年もそろそろ、どの機能を C# 10.0 にして、どの機能を "Next" のまま(11 以降に先送り)にするかを決めないといけない時期が来ましたと言う話。

## マージ済み機能

まず、[Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) が更新されました。

- [roslyn#54728](https://github.com/dotnet/roslyn/pull/54728)

「C# 10.0」の方に移ったのが以下の4つ。
(17.0p2 が Visual Studio 17 Preview 2、
17.0p3 が Preview 3。)

| 機能 | Merge 先 |
| ---- | ---- |
| [Lambda improvements](https://github.com/dotnet/csharplang/blob/main/proposals/lambda-improvements.md) | 17.0p2 |
| [Static Abstract Members In Interfaces](https://github.com/dotnet/csharplang/issues/4436) ※ | 17.0p2 |
| [Interpolated string improvements](https://github.com/dotnet/csharplang/issues/4487) | 17.0p3 |
| [File-scoped namespace](https://github.com/dotnet/csharplang/issues/137) | 17.0p3 |

※ これだけ「Preview」です(後述)。

Visual Studio 17 Preview 1 が出てからそろそろ1か月くらいですし、
このリストに「p3」(次の次)の文字が並び始めたんで、そろそろ Preview 2 が出るんでしょうね。

## 10.0 には間に合わせるリスト

それとは別に、[7月12日の LDM](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-07-12.md) では残りの "Next" について、10.0 に入れるべきかどうかの話があったみたいです。それによれば、

* [Parameterless struct constructors](https://github.com/dotnet/csharplang/issues/99)
    * record structs の一部みたいなものだし入る。p3 にマージ予定
* [nameof(parameter)](https://github.com/dotnet/csharplang/issues/373)
    * 需要あるのはわかってるけど10には間に合わなさそう
* [Parameter null-checking](https://github.com/dotnet/csharplang/issues/2145)
    * 11 入りを目指す
* [Relax ordering of ref and partial modifiers](https://github.com/dotnet/csharplang/issues/946)
    * 進捗ないです。"Next" からも消した方がいいか
* [Caller expression attribute](https://github.com/dotnet/csharplang/issues/287)
    * ほぼできてる。たぶん 10 入り
* [Generic attributes](https://github.com/dotnet/csharplang/issues/124)
    * できてる。たぶん 10 入り
* [List patterns](https://github.com/dotnet/csharplang/issues/3435)
    * 11 入りを目指す

だそうです。

あと、ひっそりと、[raw string literals](../../2/rawstringliteral/index.md) の実装コードの中に「[C# 11.0](https://github.com/dotnet/roslyn/pull/54789/commits/9d7b780045316eb3429538ca4b8a91dc9a2bc114#diff-3ac66c25866171b5ebdd5ff2622127096c9b78b7f724d7936b64872faa977894R1986)」の文字が
(コンセプト検証用のコードですけど、「既成事実化」を狙ってそうな匂いが多少)。

## ※ Preview

これまでだいたい、Preview というと、

* 1月～7月くらいまでの間、[LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) 指定必須で機能提供
    * .NET 6 Preview を使っていても LangVersion preview 指定が必須
* 8月くらいから、時期 .NET SDK と同期して LangVersion default 扱い
    * .NET 6 Preview を使うと LangVersion default が C# 10.0 になる
* 11月の .NET SDK リリースに合わせて言語機能もリリース
    * .NET 6 のリリースと同時に C# 10.0 としてリリース

みたいなものしかありませんでした。

ただ、今回、 .NET 6 リリース時点でも Preview として残りそうな機能が1個あります。

* [Static Abstract Members In Interfaces](https://github.com/dotnet/csharplang/issues/4436)

こいつだけは、 Visual Studio 17 Preview 2 時点で動く物が世に出るものの、.NET 6 リリース時点でも LangVersion preview 必須になりそうです。
要するに、Preview である期間を十分長く取りたいくらいチャレンジ度合いの高い機能です。

C# 側だけでなく、 .NET ランタイム側にも Preview オプション指定での実行が必要だし、`RequiresPreviewFeatures` 属性が付いていて「ランタイム側の Preview 機能を使う前提」のライブラリでしか使えないようにアナライザーでチェックを行うみたいです。
