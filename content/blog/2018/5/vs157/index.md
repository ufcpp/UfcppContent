---
title: "Visual Studio 15.7 リリース など"
source_url: "https://ufcpp.net/blog/2018/5/vs157/"
content_type: "BlogEntry"
published_at: "2018-05-08T11:37:51"
updated_at: "2018-05-08T11:37:51"
tags: []
umbraco_id: 2152
parent_id: 2150
sort_order: 1
aliases: []
---

# Visual Studio 15.7 リリース など

[Micorsoft Build](https://developer.microsoft.com/en-us/events/build)に合わせて、Visual Studioの新バージョンが正式リリースしたみたいですね。その他、.NET/C# 関連をいくつか。

告知ブログ:

- [Microsoft Build 2018: New releases for Visual Studio, Visual Studio for Mac, .NET Core and Xamarin.Forms](https://blogs.msdn.microsoft.com/visualstudio/2018/05/07/microsoft-build-2018-new-releases-for-visual-studio-visual-studio-for-mac-net-core-and-xamarin-forms/)

正式リリース:

- [Visual Studio 2017 15.7](https://blogs.msdn.microsoft.com/visualstudio/2018/05/07/visual-studio-2017-version-15-7-and-version-15-8-preview-1/)
  - [C# 7.3](../../../../study/csharp/cheatsheet/ap_ver7_3.md)も含まれています
- [Visual Studio for Mac, version 7.5](https://blogs.msdn.microsoft.com/visualstudio/2018/05/07/visual-studio-for-mac-version-7-5-and-beyond/)
- [ML.NET](https://github.com/dotnet/machinelearning)
  - .NET 実装のオープンソースな機械学習ライブラリ
  - バージョン 0.1 ですが

リリース候補版:

- [.NET Core 2.1 RC](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/announcing-net-core-2-1-rc-1/)
  - Go Live (自己責任だけど、もう実運用環境で使ってもいいよ)サポートに
  - Alpine Linux サポート
  - ARM プロセッサ サポート
  - [SourceLink](https://github.com/dotnet/sourcelink)

プレビュー版:

- [Visual Studio Live Share](https://www.visualstudio.com/services/live-share/)
  - パブリックに(これまで登録制のプライベート プレビューだった)
- [Visual Studio IntelliCode](https://blogs.msdn.microsoft.com/visualstudio/2018/05/07/introducing-visual-studio-intellicode/)
  - AIでインテリセンスを賢くする的なものらしい

ロードマップ公開:

- [.NET Core 3](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/net-core-3-and-support-for-windows-desktop-applications/)
  - 今年後半にプレビュー公開、正式版は2019年リリース予定

## C# 7.3

地味な更新なのでほとんど触れられてませんが、C# 7.3も正式リリースっぽいです。

一応なんとかリリースまでに全機能網羅できてんで、
詳しくは「[C# 7.3 の新機能](../../../../study/csharp/cheatsheet/ap_ver7_3.md)」を参照してください。

「C# 7.2の延長」みたいな機能とか、
相変わらず[refがらみ](../../../../study/csharp/resource/sp_ref.md)(一般的なユーザーにはそこまで使われないかも)とか、
[`Span<T>`がらみ](../../../../study/csharp/resource/span.md)(言語機能だけは先にあるけど、`Span<T>`自体はまだリリース候補版の状態)とかが多いですけども。
以下の2つとかは結構面白いかも。

- [オーバーロード解決の改善](../../../../study/csharp/cheatsheet/ap_ver7_3.md#overload-resolution)
- [ジェネリック型引数に対する Enum, Delegate 制約](../../../../study/csharp/cheatsheet/ap_ver7_3.md#constraints)

## .NET Core for Desktop

[.NET Core 3](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/net-core-3-and-support-for-windows-desktop-applications/)のロードマップでは、「Windowsデスクトップも .NET Coreの上で動くようにするよ」とのこと。

ちなみに、あくまでWindows限定です。
「WPFとかもクロスプラットフォームにする」的な意味ではなく、
「Windowsでしか動かしようのないものもCore上に載せる」という意味です。

メリットは以下のような感じ。

- ユーザー視点: Windows限定機能も、.NET Coreのメリットを享受できる
  - side by side アップデート可能
      - 同一マシン内に複数バージョンの .NET Core ランタイムをインストールして、アプリごとに選べる
      - 最新ランタイムの機能を享受しやすい
      - 「最新版でしか動かない」みたいなものは少ないものの、「最新版にするだけで速くなる」みたいなのは常にある
      - 今後は[「最新版でしか動かない」もあり得る](https://github.com/dotnet/csharplang/issues/52)
  - [App-local デプロイ](https://docs.microsoft.com/ja-jp/dotnet/core/deploying/#self-contained-deployments-scd)ができる
      - アプリ自体に依存ライブラリを全部含めてデプロイできる
- .NET チーム視点: ようやく Core に一本化できそう
  - .NET Frameworkにしかない機能がある限り、.NET Frameworkの開発は止めれない
