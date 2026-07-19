---
title: "Pickup Roslyn 1/21"
source_url: "https://ufcpp.net/blog/2018/1/pickuproslyn0121/"
content_type: "BlogEntry"
published_at: "2018-01-21T19:43:07"
updated_at: "2018-01-21T19:43:07"
tags: []
umbraco_id: 2131
parent_id: 2125
sort_order: 3
aliases: []
---

# Pickup Roslyn 1/21

今日は coreclr, corefxlab, designs から1件ずつ、計3つ。

## C# スクリプトの実用

- [Update clr-configuration-knobs.md, add new C# csi.exe based script for xplat document generation #15858](https://github.com/dotnet/coreclr/pull/15858)

なんかドキュメント生成系のスクリプトを1個、sh から C# スクリプトに置き換えてみるのを試したいらしい。
曰く、

- 典型的なスクリプト作業がどの程度効率化するか知りたい
- あんまり重要でないものでとりあえずドッグフーディングを始めたい
- [m4](https://ss64.com/bash/m4.html)への依存を減らしたい
- C#ベースのスクリプト利用の強み・弱みの知見を得たい

とのこと。

あと、今の (.NET Core 向けのは) Regex が遅すぎてやってらんないからこのプルリクでは Regex クラスの利用を避けてるらしい。

## priority queue

- [Solved priority queue design! #1850](https://github.com/dotnet/corefxlab/pull/1850)

今更ながら、[priority queue](../../../../study/algorithm/collection/col_heap.md) の実装するみたい。
とりあえず、corefxlab でお試し実装を提案中。

割かし「なんで .NET にはないんだろう？」と言われ続けてるデータ構造筆頭。

なんかさらっと見てる感じ、priority queue に mutable なデータを入れたあと、優先度が変わるような変化を書けたときがまずそうな感じ。

## .NET Core Runtime と .NET Core SDKのバージョン

- [Add plan for .NET Core SDK version numbers #29](https://github.com/dotnet/designs/pull/29)

.NET Coreって、今、Runtime (.NET 製プログラムを動かすための実行環境)とSDK (コンパイラーとかを含む)のバージョン番号がずれてて本当にわかりにくく。

前々からそれに文句を言ってた人が、「上2つの数字(メジャーバージョンとマイナーバージョン)くらいは揃えよう」っていう提案文書を提出。
まあ、全くもってその通りで。

ちなみに、まあ、SDK には「C# コンパイラーのバグ修正のみのリリース」みたいなのがあるので、Runtime と SDK のバージョンを完全に足並みそろえるってのはできないそうです。
なので、上2つのみの統合。

あと、SDK の方の3つ目のは、「基本、100単位でバージョンを上げる。バグ・セキュリティ ホール修正のサービス リリースは1ずつ上げる」みたいなのを提案。
