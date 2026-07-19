---
title: ".NET Coreへの移植"
source_url: "https://ufcpp.net/blog/2016/2/porting-to-net-core/"
content_type: "BlogEntry"
published_at: "2016-02-13T14:05:38"
updated_at: "2016-02-15T00:51:40"
tags: []
umbraco_id: 1875
parent_id: 1873
sort_order: 1
aliases: []
---

# .NET Coreへの移植

twitterで流れてきてて、気になったやつ。

- [Porting to .NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/)

内容的には、

- フィードバック募集中なのでお願いします
- .NET Coreは今、ASP.NET、UWP、コンソール アプリに使えるけども、.NET Frameworkからの移植のモチベーションはそれぞれ何か
- .NET Framework と .NET Coreの関係・差分
- 意図して.NET Coreには取り込まなかったものがちらほらあるから注意
- .NET Core化するかどうか、単純に時間的な不足で検討してないものもちらほら
  - 特にフィードバックほしいのはここ。ほしいかどうか、優先度付けしたい
- 移植にあたってのコツ

みたいなの。

以下、さらっと概要。

## 何を移植するか

### ASP.NET

移植する理由:

- .NET Coreならクロスプラットフォーム。MacやLinuxで動く
- マシン全体に対するインストールじゃないんで、デプロイが楽(管理者権限とかも不要でバージョン変えれる)

移植するとよい候補:

- MVC/Web API使ったものなら移植しやすいのでお勧め

あまり移植に適してないもの:

- WebFormsは .NET Coreに移植してないので、WebFormsを使っているものは以降大変。ほぼ再実装に
  - もちろん、そろそろWebForms自体をMVCとかに置き換えたいと思っているなら話は別

### UWP

移植する理由:

- (.NET Coreへの移植というか、既存GUIフレームワークからUWPへのという意味で)PC、tablet、phablet、phone、Xbox、IoTデバイスと、広範囲のデバイスで統一的なアプリ開発ができる

移植するとよい候補:

- Windows 8/Windows Phone 8.1アプリは最初から.NET Core
  - .NET Coreの前身というか、いずれ.NET Coreが出ることを前提として設計されたというか、ほぼ Windows 8アプリ = .NET Core + Windows UIフレームワーク
- Silverlightアプリも、開発モデルはUWPとかなり近い

あまり移植に適してないもの:

- Win32なデスクトップ機能をフルに使ったWindows ForsmアプリやWPFアプリは
  - ただし、WPFアプリのXAMLを使った開発スタイルはUWPとかなり近いので多少楽

### コンソール アプリ

移植する理由:

- これも、クロスプラットフォーム対応
- .NET Native (事前に必要な部分だけネイティブ化した単一実行ファイルを作ってしまう機能)を使いたい場合

移植するとよい候補/あまり移植に適してないもの:

- 単純に依存しているライブラリが.NET Coreに対応してるかどうか次第
  - ASP.NETやUWPと比べるとハードル低いはず
- 例えばCOMとか使いまくったWindowsとかOfficeの自動化アプリは移植できないだろうし移植の意味もなさそう

## .NET Coreと.NET Framework

.NET Coreは、ある程度.NET Frameworkのサブセットになるように作られてるというか、Visual Studio 2015時点くらいまではそうなるようにある程度頑張ってたはず。でも、ずっとそうなるように維持するつもりはないみたい。現状でも.NET Core側だけにある機能がちらほらあるし、今後はまず.NET Core向けに機能実装されてくことになるはずだし、それが.NET Framework側にバックポーティングされる保証はない。

といっても、かなりの部分は.NET Core向けと.NET Framework (4以降)向けでコード共有できるはず。

差が生じる理由は大まかにいうと以下の3点に由来:

1. NuGetベース
  - .NET Frameworkはシステム全体に対するインストールが必要
  - .NET CoreはNuGetパッケージ マネージャーを使ってモジュールごとにバージョン管理可能
  - なので、大体において、.NET Coreの方が新機能を早く使える
1. きれいにレイヤー分け
  - .NET Frameworkではレイヤー分かれず低層から高層まで丸ごと含んでたようなライブラリが結構ある
  - .NET Coreではその辺りを整理して分割したものがある
1. 問題ある技術の除外
  - .NET Framework 15年の歴史から、問題がはっきりしてて、新規実装の際には取り入れたくないものがある

## 変化の激しいもの: リフレクション

リフレクションは結構APIが変わっていて困るもの代表で。理由は:

- レイヤー分けの問題: `object` が `Type` に依存することで、`System.Reflection`過剰に参照されかねない
  - `Type`には型識別(型の一致判定とか、型の名前を取得したりとか)だけ残す
  - 動的実行や動的コード生成に必要な情報は`TypeInfo`に分離
- そもそもリフレクション非推奨
  - .NET Native (事前ネイティブ化)とは相性悪すぎる

## .NET Coreには実装しないもの

### AppDomain

理由: .NET Coreみたいな層でサポートするにはコストが高すぎる。.NET Nativeでは使えない

代替技術: コード分離(isolation)はプロセスとかコンテナーとかの技術使ってほしい。動的なDLL読み込みのためには`AssemblyLoadContext`っていう新しいクラスがある

### リモーティング

理由: RPC(remote procedure call)自体がいまいちと認識されてる。AppDomain間の通信みたいなものなので、非常に高コスト。

代替技術: プロセス間通信だったらpipeとかmemory mapped fileとかを使うべきだし、マシン間通信だったらHTTPとか使ったネットワーク ベースの技術使うべき。

### バイナリ シリアライズ

理由: privateなデータまでアクセスしちゃうし、互換性を取る上で重荷でしかない。

代替技術: 要件によって最適なシリアライザーは変わるだろうけど、それを要件に合わせて選んでほしい。候補としては、[data contract serializer](https://msdn.microsoft.com/en-us/library/ms731072.aspx)、[XMLシリアライザー](https://msdn.microsoft.com/en-us/library/182eeyhh.aspx)、[JSON.NET](http://www.newtonsoft.com/json)、[protobuf-net](https://github.com/mgravell/protobuf-net)など

### サンドボックス化

理由: これも.NET Coreみたいな層でサポートするにはコストが高すぎる。正しくサンドボックス化するのはかなり難しくて、信頼も置けない。

代替技術: OSレベルでのセキュリティ境界を使ってほしい。

## 移植も考えているもの

現状の.NET Coreにないものはもっといろいろあるものの、移植しないと決まったわけじゃなくて、単純に時間が足りないものもある。これに関しては、移植の必要性がどのくらいあるか、特にフィードバックがほしいみたい。

- `System.Data`
- `System.DirectoryServices`
- `System.Drawing`
- `System.Transactions`
- `System.Xml.Xsl `と`System.Xml.Schema `
- `System.Net.Mail`
- `System.IO.Ports`
- `System.Workflow`
- `System.Xaml`

## 移植にあたって

既存の.NET Framework向けのアセンブリがどのくらい移植しやすそうか解析するツールがあるみたい:

- [.NET Compatibility Diagnostic Tools](http://dotnetstatus.azurewebsites.net/)

で移植の流れとしては、

- とりあえず .NET Framework 4.6.1にしとくと.NET Coreとの差が少ない。
- 元のコードは .NET Framework ターゲットのまま、このツールを掛けつつ修正してとか繰り返すのがおすすめ。
- .NET Coreへの完全移行じゃなくて、Core/Framework両方で使うとかもあるだろうからその場合は[Shared SourceとかPCLとか](https://blogs.msdn.microsoft.com/dotnet/2014/04/21/sharing-code-across-platforms/)を活用

とかそんな感じ。
