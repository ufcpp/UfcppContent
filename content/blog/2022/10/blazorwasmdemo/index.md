---
title: "Blazor Wasm 実動作デモはじめました"
source_url: "https://ufcpp.net/blog/2022/10/blazorwasmdemo/"
content_type: "BlogEntry"
published_at: "2022-10-31T22:14:02"
updated_at: "2022-10-31T22:14:02"
tags: []
umbraco_id: 2433
parent_id: 2432
sort_order: 0
aliases: []
---

# Blazor Wasm 実動作デモはじめました

昔、うちのサイトのページ内に iframe で張り付けとくような実動作デモを[いろいろと Silverlight 作ってた](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/csharp/source)んですが、
Silverlight のサポート終了後、移行先がなくてほったらかしになっていました。

「[その時が来たら本気出す](https://github.com/ufcpp/UfcppSample/labels/%E3%81%9D%E3%81%AE%E6%99%82%E3%81%8C%E6%9D%A5%E3%81%9F%E3%82%89%E6%9C%AC%E6%B0%97%E5%87%BA%E3%81%99)」とかいう雑なタグをつけて放置してたんですが、
そろそろ [Blazor WebAssembly](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/#blazor-webassembly) 化でもしてみようかという感じで数年越しに作業する気になり。

とりあえず、ソートのページで使っていたソートの可視化プログラムを移植。

* [ソート概要](../../../../study/algorithm/sort/sort.md#demo)
* [バブルソート](../../../../study/algorithm/sort/sort_bubble.md#abstract)
* [クイックソート](../../../../study/algorithm/sort/sort_quick.md#abstract)
* (他、一通りのソートのページに)

移植というか、もう完全に忘れてるし、なんだったら思い立った瞬間には昔のコードをどこに置いたかわからなくなっていたので1から作ったんですが。

* ソースコード: [SortVisualizer](https://github.com/ufcpp/StaticWebApps/tree/main/BlazorWasm/SortVisualizer)
* [Static Web App](https://azure.microsoft.com/ja-jp/products/app-service/static/) のデプロイ先: [https://black-ocean-009cb0000.2.azurestaticapps.net/](https://black-ocean-009cb0000.2.azurestaticapps.net/)

実物 iframe (クイックソート単品):

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=quick&i=0&s=0&w=300" width="304" height="332"></iframe></div>

実物 iframe (一覧):

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?i=0&s=0&w=150" width="780" height="500"></iframe></div>

`<span style="witdh: ...; height: ..." />` とかでバーを表示するという雑なことやっても、
スマホとかで表示しても結構ちゃんと動いていてほんと富豪的…

まあさらっとやってさらっと動いたので、
他にも何かしらこの手の実動作デモがページ内にあるとよさげなものがあれば作ろうかなという気分になっています。
(何かいいものがあれば。)
