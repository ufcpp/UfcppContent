---
title: "cszip、nuget.org に上げました"
source_url: "https://ufcpp.net/blog/2018/3/upload-cszip/"
content_type: "BlogEntry"
published_at: "2018-03-04T15:21:09"
updated_at: "2018-03-04T15:21:37"
tags: []
umbraco_id: 2135
parent_id: 2134
sort_order: 0
aliases: []
---

# cszip、nuget.org に上げました

こないだ .NET Global Tools を試すのに作ってみた [cszip](https://www.nuget.org/packages/cszip/) と [csunzip](https://www.nuget.org/packages/csunzip/)、[nuget.org](https://www.nuget.org/) に上げてみといた。

以下のコマンドでインストール可能な状態になっています。

```shell
dotnet instal tool -g cszip
dotnet instal tool -g csunzip
```

以下のように適当にもほどがあるんで nuget.org に上げるかどうか迷っていたものの。

- readme の類一切ない
- ほんとに内部的に [CreateFromDirectory](https://docs.microsoft.com/ja-jp/dotnet/api/system.io.compression.zipfile.createfromdirectory?view=netframework-4.7.1)、[ExtractToDirectory](https://docs.microsoft.com/ja-jp/dotnet/api/system.io.compression.zipfile.extracttodirectory?view=netframework-4.7.1) を呼んでるだけ
  - 例外処理全然してない (不正な引数を渡したら .NET の例外メッセージがそのまま出る)
  - オプション指定とかもできない (常に「圧縮率優先」「UTF8」)
- 専用のリポジトリ持ってなくて、[UfcppSample](https://github.com/ufcpp/UfcppSample)のDemoフォルダー以下にある

名前的にも、以下のような点で悩んだものの。

- C# で書いているというだけで、別に「C# 向け」ではないけど cs~
- こんな手抜き実装なもので cszip とかいう名前を取っちゃっていいのか

あと、当初目的([前述](../../2/dotnettoolspkgs/index.md)の通り、クロスプラットフォーム ビルドの面倒をマシにしたい)を考えるとLinuxとかMacでの動作確認しないといけないわけですけどもそれもやっておらず…

まあ、やっちゃってから考えるかと。
