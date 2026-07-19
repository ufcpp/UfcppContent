---
title: "JSON とかの中身確認ツール"
source_url: "https://ufcpp.net/blog/2022/12/binarytool/"
content_type: "BlogEntry"
published_at: "2022-12-30T16:34:58"
updated_at: "2022-12-30T16:43:50"
tags: []
umbraco_id: 2447
parent_id: 2438
sort_order: 6
aliases: []
---

# JSON とかの中身確認ツール

今日は、「主に自分が使う用ツールを Blazor WebAssembly で作って Static Web Apps に置いたよ」系の話を一応ブログ化。

* [ソースコード](https://github.com/ufcpp/StaticWebApps/tree/main/BlazorWasm/BinaryTool)
* [Static Web App](https://ambitious-plant-0ea3d5100.2.azurestaticapps.net/)

<div>
<iframe width="1409" height="794" src="https://www.youtube.com/embed/_oi53peGbnw" title="Binary Tool" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

よくある「JSON とかのデータの中身を確認するツール」です。

しばらく、JSON と MessagePack の読み書きをするコードを書いてて、
デバッグがしんどくなって作ったのがこのツール。

いろんな形式を同時に扱うことがニッチ需要なのであんまり自分の需要にあったツールがなかったんですよね。なら、まあ、自作。

[こないだの C# 配信](https://youtu.be/_f6UA3Vs3Kc?t=5935) で、UTF-8 とか MessagePack バイナリとかを手打ちで入力してたら [@xin9le](https://twitter.com/xin9le), [@okazuki](https://twitter.com/okazuki) 両氏にドン引きされたやつ。

## バイナリ読み込み (Parser)

UTF-8 を `ReadOnlySpan<byte>` のまま扱ってて、
ブレイクポイントを仕掛けてデバッガーで中身を覗くとかやってると、
「97, 98, 99」みたいな数値列しか見れないことが多く。

デバッガーからコピペするとその数値が `"97, 98, 99"` みたいな文字列としてコピーされてしまったりするので、それをバイナリに戻す処理を入れています。

せっかくなので、

* 10進数コンマ区切り `97,98,99`
* 10進数スペース区切り `97 98 99`
* 16進数コンマ区切り `61,62,63`
* 16進数スペース区切り `61 62 63`
* 16進数区切りなし `616263`
* UTF-8 文字列 `abc`
* Base64エンコードされた文字列 `YWJj`
* C# 風 `0b0110_0001, 0x62, 99`
* 上記の中からある程度自動判定

とかを読めるようにしています。

## 文字列化 (Formatter)

読み込んだバイナリを表示する形式も選択式に。
UTF-8 だったらそれをデコードした文字列を見たいことがほとんどですし、
バイナリなら16進数を並べたもの辺りが使い勝手がいいと思います。

その他、「デバッグで使いたいけども、いちいちファイルとかから読むの面倒で、C# コード中に生で埋め込みたい」みたいな時のために、`new byte[] { 0x61, 0x62, 0x63 }` みたいな書式にする機能も持たせています。

## デシリアライズ結果の可視化 (DOM)

用途が「JSON とかの中身確認」だったのでその機能を持っています。

```json
{"abc":0,"ABC":[null,false,true]}
```

みたいなデータを、

* abc: 0
* ABC:
    * 0: null
    * 1: false
    * 2: true

みたいな `<li>` タグにして表示します。

とりあえず JSON と MessagePack に対応。
Protocol Buffers と YAML くらいには対応してもいいかも。

自動判定というか、「エラーなくデシリアライズできた最初の1個を選択」みたいな処理も入れています。
(いかにも「自分用ツール」らしく、エラー処理が甘いんで結構フリーズするんですが…)

## 再シリアライズ (Writer)

再度シリアライズして、改めて Formatter にかける処理も入っています。

「MessagePack なバイナリを、JSON な UTF-8 で表示」みたいなことやると結構デバッグがはかどりまして。
