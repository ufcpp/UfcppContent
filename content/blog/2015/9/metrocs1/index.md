---
title: "Roslyn メタプログラミング & Code-Aware ライブラリ"
source_url: "https://ufcpp.net/blog/2015/9/metrocs1/"
content_type: "BlogEntry"
published_at: "2015-09-17T01:48:59"
updated_at: "2015-09-18T00:53:31"
tags: []
umbraco_id: 1799
parent_id: 1787
sort_order: 4
aliases: []
---

# Roslyn メタプログラミング & Code-Aware ライブラリ

昨日の [Metro.cs #1](https://roommetro.doorkeeper.jp/events/30482) にて。

<div style="width: 608px; max-width: 100%; margin-bottom:5px;"><a href="https://docs.com/iwanaga-nobuyuk/4439/roslyn" title="Roslynメタプログラミング" target="_blank" style="font-family: 'Segoe UI'">Roslynメタプログラミング</a><span style="font-family: 'Segoe UI Light'">—</span><a href="https://docs.com/iwanaga-nobuyuk" target="_blank" style="font-family: 'Segoe UI'">Iwanaga Nobuyuki</a></div><iframe src="https://docs.com/d/embed/D25195984-8847-6785-4430-000426915410%7eMd2f0fde0-d68b-9095-2ec5-841305bd4fb1" frameborder="0" scrolling="no" width="608px" height="378px" style="max-width:100%"></iframe>

デモ用ソースコード: [https://github.com/ufcpp/UfcppSample/tree/master/Demo/2015/MyRoslynAnalyzers](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2015/MyRoslynAnalyzers)

要するに、「実際作ってみた結果の感想など」。

作ってみたもの自体は大したことはないんで面白くもないと思いますが、感想の部分は他の Analyzer, Code Fix, Code-Aware ライブラリを作ったりする際にも役立つだろうというつもり。

作ってみたものに関しては、結構、「C# 7.0 が来れば解決しそうなもの」に対する「それまでの間のつなぎ」的なものだったりします。つなぎだし、そんなに頑張らず適当に作ろうとかいう軽いノリができるのも、気軽に作って気軽に配布できるようになったことの強みかなぁとか思います。

## supersedes

スライド中に、一昨日公開されたばかりの[Design Notes 9/2](https://github.com/dotnet/roslyn/issues/5234)の話を少し含めましたが。

今日、supersedes 機能専用の Issue ページも立ってた。

- [[Proposal] add supersede modifier to enable more tool generated code scenarios. #5292](https://github.com/dotnet/roslyn/issues/5292)

supersedes(「取って代わる」という意味の単語)でできること自体は Python のデコレーターみたいなの。Javaだとannotation processing tool (APT)使ってできたりすること。.NETでもIL書き替えであれば[PostSharp](https://www.postsharp.net/)とか使ってやれるもの。
要するに、あるメソッドやプロパティに対して、その前後にツール生成コードとかで処理を追加する機能。

それを、C# → C# コード生成で、静的に実現しようというのがこの supersedes 構文。
Java の APT や PostSharp を使ってできるような便利さを実現しつつ、実行効率を落とさず、実際に動いているのがどういうコードなのかC#コードが目で見える。

構文自体は結構シンプルなものですが、いくつか課題があることもわかっているし、C# 的にはVisual StudioとかのIDEのデバッグやリファクタリングの機能がちゃんと働く形で実現しないといけないし、それなりに大変なはずです。

一応マイルストーンは C# 7.0 に入っていますが。順調に行ってくれるといいなぁ。

## once a year

そうそう、こんなコメントが。

[https://github.com/dotnet/roslyn/issues/996#issuecomment-140605428](https://github.com/dotnet/roslyn/issues/996#issuecomment-140605428)

この前後の話の流れ的には、

- その機能、まあ少なくとも C# 7.0とか8.0では取り組まないと思うよ
- 9.0？C# って2・3年に1回リリースだし、2022年とかくらいまで待たせるの？
- ロードマップ示せるわけじゃないけど、これからは「年に1回リリース」とかやれるといいなと思ってる

みたいなの。

あくまで目標であって確約はされないものの、もしかしたらタプルとかのC# 7.0予定機能、来年には使えるようになっているかも。
