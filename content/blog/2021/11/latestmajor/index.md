---
title: "TargetFramework net5.0 なコードを .NET 6 ランタイムで動かす"
source_url: "https://ufcpp.net/blog/2021/11/latestmajor/"
content_type: "BlogEntry"
published_at: "2021-11-14T21:47:39"
updated_at: "2021-11-14T21:47:39"
tags: []
umbraco_id: 2370
parent_id: 2363
sort_order: 3
aliases: []
---

# TargetFramework net5.0 なコードを .NET 6 ランタイムで動かす

## .NET のアップデート

昔の C# アプリ (例えば去年作った TargetFramework net5.0 なアプリ)をそのまま最新のランタイム(例えば .NET 6 ランタイム)で動かすことを考えます。

.NET は API レベルでの破壊的変更はめったにないので、
「API が合わなくてロードできない」みたいな根本的な問題はほぼ起こりません。

一方、挙動レベルでは時々破壊的変更があるんで、確実に動く保証はなかったりします。
(それでも、体感、9割方は動きますが。)

ここ数バージョンであった影響がありそうな変更でいうと、

* .NET Core 3.0 の頃同期 I/O が例外を出すようになったものがちらほらある
  * ネットワークなどを介する場合、非同期でないとパフォーマンスが出ないので
* .NET 5 で、国際化対応に [ICU](https://icu.unicode.org/) を使うようになった
  * 文字列の紹介順や、`IndexOf` の挙動がちょっと変わった
* .NET 6 で、[`FileStream`](https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/filestream-file-locks-unix) とか [`CryptoStream`](https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/partial-byte-reads-in-streams) の挙動がちょっと変わった

とかがあって、時々、最新のランタイム上で動かそうとしてもうまくいかないことがあります。

## アップデート手順

そういうのは「わかってる人だけやって欲しい」ということらしく、 .NET Core 3.1 以降の .NET は「バイナリは昔のままで最新のランタイム上で動かす」という操作に対してかなり保守的です。

例えば、オプション指定なしだと、.NET 5 向けに作った C# アプリを .NET 6 ランタイム上で動かせません。

手間をかけられるなら、

* プロジェクトの TargetFramwork を net6.0 に上げる
* SDK/ランタイムも .NET 6 に上げる
* その状態で単体テストが通るようにコードを書き換える

という3つを同時にやってくれと言うことだと思います。
(実際、挙動の破壊的変更がまれにある以上、そうするしかアップデート後の動作保証は取れないはずですが。)

とはいえ、できることからコツコツやっていきたいことだってよくあるわけで、

* CI 環境の .NET SDK を先にバージョンアップしておく
* それに合わせてできるプロジェクトからちょっとずつ TargetFramework を変更していく

みたいなことをしたい方も結構多いんじゃないかと思います。

## Roll Forward

まあ、API レベルでの破壊的変更がほとんどなく、挙動レベルでも9割方は動くものに対して常に最近に追従する作業が必要かという話もあり。
ちゃんと、「古いバイナリを最新のランタイムでそのまま動かす」というオプションがあります。

正確に言うと、「バージョン不一致のどき、どのくらいずれてても OK か」を指定するためのオプションがあって、これを [roll forward](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-core-3-0#major-version-runtime-roll-forward) と言います。

デフォルトが Minor で、これがなかなか厳しい…
(メジャー バージョンが一致するものがないと実行できない。)

こうことで、先ほどの「.NET SDK を先にバージョンアップしておく」シナリオをやるなら roll forward 設定の変更が必要になります。
Major か LatestMajor なら動かせるはずですが、
僕みたいな「常に最新の SDK/ランタイムに追従」ポリシーの人はとりあえず LatestMajor で大丈夫です。

### Roll Forward オプションの指定方法

Roll Forward オプションの指定の仕方はいくつかあるみたいです。
dotnet コマンドに[直接オプションを書く](https://docs.microsoft.com/ja-jp/dotnet/core/tools/dotnet#runtime-options)方法もありますし、

```console
dotnet run --roll-forward LatestMajor
```

(ただ、`dotnet run` はこのオプションを受け付けるものの、`dotnet test` は受け付けないらしい？)

[global.json に書いておく](https://docs.microsoft.com/ja-jp/dotnet/core/tools/global-json?tabs=netcore3x#rollforward)のでもいいそうですし、

```json
{
  "sdk": {
    "version": "6.0.100",
    "rollForward": "latestMajor"
  }
}
```

[環境変数で `DOTNET_ROLL_FORWARD` を設定しておく](https://docs.microsoft.com/ja-jp/dotnet/core/tools/dotnet-environment-variables#dotnet_roll_forward)のでもいいそうです。

今回のシナリオ(CI)だと、たぶん環境変数を設定するのがよくて、
例えば GitHub Actions の build.yml に以下の行を足せばいいということになります。

```csharp
env:
  DOTNET_ROLL_FORWARD: latestMajor
```

### 実例

昔、[GitHub Actions を試してみるためだけに空っぽのライブラリに空っぽの単体テストを定義したリポジトリ](https://github.com/ufcpp/TryGithubActions)があったので、
それを .NET 5 から .NET 6 にアップデートしてみました。

CI が失敗:

* [TargetFramework だけアップデート](https://github.com/ufcpp/TryGithubActions/pull/6)
  * The current .NET SDK does not support targeting .NET 6.0.
* [CI 設定だけアップデート (Roll Forward なし)](https://github.com/ufcpp/TryGithubActions/pull/5)
  * version '5.0.0' (x64) was not found.
  * (今回のシナリオだとこれがつらい)

CI が成功:

* [CI 設定と TargetFramework を同時にアップデート](https://github.com/ufcpp/TryGithubActions/pull/4)
* [CI 設定だけアップデート (Roll Forward あり)](https://github.com/ufcpp/TryGithubActions/pull/7)
