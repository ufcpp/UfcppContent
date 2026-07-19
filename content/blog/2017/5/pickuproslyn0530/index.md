---
title: "ピックアップRoslyn 5/30: C# 7.1など(de:code 2017)"
source_url: "https://ufcpp.net/blog/2017/5/pickuproslyn0530/"
content_type: "BlogEntry"
published_at: "2017-05-30T19:57:07"
updated_at: "2017-05-30T19:57:07"
tags: []
umbraco_id: 2069
parent_id: 2059
sort_order: 3
aliases: []
---

# ピックアップRoslyn 5/30: C# 7.1など(de:code 2017)

先週、[de:code 2017](https://www.microsoft.com/ja-jp/events/decode/2017/)で登壇してきたわけですが。
資料、公開しました。

<div style="width: 608px; max-width: 100%; margin-bottom:5px;"><a href="https://docs.com/iwanaga-nobuyuk/3751" title="C#の現状と今後" target="_blank" style="font-family: 'Segoe UI'; font-size: 13px; text-decoration: none; margin-left:18px ">C#の現状と今後</a><span style="font-family: 'Segoe UI'; font-size: 13px ">—</span><a href="https://docs.com/iwanaga-nobuyuk" target="_blank" style="font-family: 'Segoe UI'; font-size: 13px; text-decoration: none ">Iwanaga Nobuyuki</a></div><iframe src="https://docs.com/d/embed/D25190616-0290-4835-4550-001218788925%7eMd2f0fde0-d68b-9095-2ec5-841305bd4fb1" frameborder="0" scrolling="no" width="608px" height="377px" style="max-width:100%" allowfullscreen="True"></iframe>

トラック オーナーの方に「日本の第一人者」とかいう煽りタイトルを付けられてしまったわけですが。
なんかネタなタイトル(このすば)を冗談で言ってみたら採用されてしまい、「MVPはどいつもこいつも…」とか思われてそうで怖いわけですが。
ネタはタイトルだけです。

## C# 7.1

で、本題。
de:code参加者の中には気づいた方もいらっしゃったみたいですけども、
この資料、de:code参加者向けの事前配布版と、当日発表でちょっと内容が変わっています。

変わったというか、C# 7.1のところのボリュームが単純に減ってるんですが。

つい最近、[C# 7.1候補マイルストーン](https://github.com/dotnet/csharplang/milestone/5)に並んでいる項目がごそっと減りました。
それを反映して、de:code登壇資料も削ったという感じです。
候補として残っているのは以下の4つ。

- [default 式](https://github.com/dotnet/csharplang/issues/102)
- [tuple projection](https://github.com/dotnet/csharplang/issues/415)
- [ジェネリクスに対してのパターン マッチング](https://github.com/dotnet/csharplang/issues/154)
- [Main メソッドを非同期(`async Task`)にできるように](https://github.com/dotnet/csharplang/issues/97)

要は、リリースまでの時間が短いなら、リリースする機能は減ってしかるべき。
項目が減ったということは、早めのタイミングでのリリースが決まった。
ということかなぁと思われます。

[先日プレビュー版が出た Visual Studio 2017 Update 3](../vs15_3preview/index.md)に、
C# 7.1も一部実装が載ったわけですが。
上記リリース タイミングの話もあり、
おそらくはVisual Studio 2017 Update 3と同時にC# 7.1もリリースなんじゃないかという状況です。

[.NET Conf](http://www.dotnetconf.net/)が今年は7月19～21日にやるみたいなので、この辺りでの動きを期待したいところ。
