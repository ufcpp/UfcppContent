---
title: "Connect() での発表、.NET Core"
source_url: "https://ufcpp.net/blog/2015/11/netcoreconnect/"
content_type: "BlogEntry"
published_at: "2015-11-19T14:42:36"
updated_at: "2015-11-19T14:43:44"
tags: []
umbraco_id: 1825
parent_id: 1810
sort_order: 3
aliases: []
---

# Connect() での発表、.NET Core

[Connect(); // 2015](https://channel9.msdn.com/)がありましたね。

昨日、Connect() 初日の基調講演に合わせて、日本では深夜ニコ生放送「[深夜に盛り上がれ　MS開発者イベント『Connect(); // 2015』生放送](http://live.nicovideo.jp/watch/lv241697499)」をやってたわけですが。

「どのくらい出番あるかわからないけどいて」とか言われて待機してたはずが、結局丸ごと画面に映っていたような…

まあ、全体の様子は[亀淵さんのとこ](https://buchizo.wordpress.com/2015/11/19/microsoft-connect-2015-day-1-azure-update%E3%82%82%E3%81%82%E3%82%8B%E3%82%88/)でも見ていただいて。

## .NET Core RC

C#/.NET的に影響があるのは、.NET CoreのRC提供開始。

RC(バグ修正とかのぞいてもう変更しない)でGo Live(製品に使っても構わない状態)になりました。オープンソースでソースコードがMITライセンスだからって、自前でソースコードとってきて、問題出たら自分で直すなんてそうそうやれないわけで、「ビルド済みバイナリをマイクロソフトがサポートします」的な意味で、RCをもってGo Live。

## --native オプション

Connect() キーノートのデモの中で、

```shell
dotnet compile -o output --native
```

とかコマンドを打ってました。--nativeオプション。

事前コンパイル(AOT)する機能も提供するみたいですね。

ちなみに、まあそもそもJIT自体の説明してなかったなぁと思って記事追加:

- [JITコンパイル](../../../../study/csharp/framework/fwjitcompilation.md)

Windows デスクトップだとそれなりにJITのメリット高くてずっとJITだけ提供してきたわけですが、Linux上とかだとAOTの需要があるだろうってことでしょうか。

ちなみに、[.NET Native](https://ufcpp.wordpress.com/2014/04/03/net-native/)とは別系統。

- .NET Native
  - 単にAOTだけじゃなくて、いろいろUniversal Windows Apps向けにガチガチに最適化
    - シリアライズとかP/Invokeのコードをビルド時に生成(通常は動的にやる)
    - XAML中の動的な処理を自動的に判別して必要な型情報を残す
    - アセンブリをまたいだ最適化までする
  - Visual C++コンパイラー(要するに、マイクロソフトが長年Windows向けに最適化しまくってる高性能コンパイラー)技術を使ってネイティブ コード生成
- .NET Coreの --native オプション
  - [RyuJIT](http://blogs.msdn.com/b/dotnet/archive/2013/09/30/ryujit-the-next-generation-jit-compiler.aspx)や[LLILC](https://github.com/dotnet/llilc)が持ってるネイティブ コード生成を事前に掛けるだけ

みたいな感じのはず。たぶん。
