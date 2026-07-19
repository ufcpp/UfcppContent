---
title: ".NET Core 3.0 Preview 3"
source_url: "https://ufcpp.net/blog/2019/3/netcore3p3/"
content_type: "BlogEntry"
published_at: "2019-03-07T20:11:50"
updated_at: "2019-03-07T20:20:45"
tags: []
umbraco_id: 2234
parent_id: 2233
sort_order: 0
aliases: []
---

# .NET Core 3.0 Preview 3

[こないだ出た Visual Studio 2019 RC1 / Preview 4](../../2/vs2019rc/index.md)に対応したバージョンの .NET Core 3 Preview が出たみたいですね。

- [Announcing .NET Core 3 Preview 3](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-3/)

.NET Core 3.0、最近なんかずっと、Visual Studio より結構遅れてのリリースですよね…
おかげで、C# 8.0 のコードがエラーになってたり(今回は、`Range`構造体の仕様変更が原因)。

## Preview 3 での更新点

さらっと内容まとめると、

- .NET Core 3.0 の正式リリースは今年後半っぽい。詳しくは今年の [build](https://www.microsoft.com/en-us/build) で
- 細かいインストールは上書きになる。パッチ バージョンの百のけたが同じものを同一視
  - 3.0.100 があるときに 3.0.101 を入れると 100 は消える
  - 3.0.200 を入れると 3.0.100 とか 3.0.101 とかは残る
- Docker がらみ、おかしかったの/不便だったの修正
- Visual Studio 2019 RC1 / Preview 4 の C# 8.0 の `Range` がちゃんと動くように
- .NET Standard 2.1 を使えるように
- F# 4.6 に。dotnet fsi コマンドで F# Intaractive 起動
- `AssemblyDependencyResolver` とか `NativeLibrary` とか、native 相互運用してる人にはうれしそうなもの追加
- WPF/WinForms とか Entity Framework とか更新
- 他は、[Preview 1のとき](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/)と[Preview 2のとき](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-2/)の告知を見て

とか。

## C# 8.0

ちなみに、`TargetFramework` を `netcoreapp3.0` か `netstandard2.1` にすると、
デフォルトの状態で C# 8.0 になるみたいです。
`TargetFramework` だけで、ビルド ツールも .NET Core 3.0 のものに切り替わる模様。
(.NET Core 3.0 が正式リリースされた暁には C# 8.0 も正式リリースになる予定なので、
.NET Core SDK 3 にとっては C# 8.0 がデフォルトになります。)

ということで、ターゲットが .NET Core 3.0 か .NET Standard 2.1 のときは、特に `<LangVersion>preview</LangVersion>` とか書かなくてもよくなります。
(netcoreapp2.2 とかがターゲットだとやっぱりこのタグが必要。あくまで新しいターゲットの時だけの切り替わり。)

RC1 の方だと「[プレビューの .NET Core SDK を使用](../../2/vs2019rc/index.md#add0301)」オプションをオンにしないといけないのだけはご注意を。

## .NET Standard 2.1

やっと、 .NET Standard も 2.1 になったわけですが。詳細は以下のページで見れます。

- [.NET Standard 2.1](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.1.md)

大体は、「.NET Core の方に追いついた」って感じの API 追加です。

.NET Core 3.0 がらみも結構ちゃんと入っています。
C# 8.0 に必要な `Range`/`Index` や、`IAsyncEnumerable`/`IAsyncDisposable` も含む。
でも、.NET Standard 的にはバージョン 2.1。

ただ、[Hardware Intrinsics](../../../2018/12/hdintrinsic/index.md)は入れないとのこと。
なんせ、Intrinsics はランタイムのサポートがあって初めて成り立つものなので、「どのランタイムで動かすかわからない .NET Standard にとっては意味がないから」とのこと。

ちなみに、追加が多いのは以下の辺り。

- [`Span<T>`](../../../../study/csharp/resource/span.md) がらみ
  - `System` とか `System.IO` とかの更新点は半分くらいは `Span` がらみ
- `System.MathF` とか、浮動小数点数がらみの追加結構あり
- これまで NuGet 提供だったものがいくつか標準入り
  - [System.Memory](https://www.nuget.org/packages/System.Memory/), [System.Buffers](https://www.nuget.org/packages/System.Buffers/), [System.Numerics.Vectors](https://www.nuget.org/packages/System.Numerics.Vectors/), [System.Threading.Tasks.Extensions](https://www.nuget.org/packages/System.Threading.Tasks.Extensions/) 辺り
  - 要は、`Span<T>`, `ArrayPool<T>`, `Vector`, `Matrix`, `ValueTask` とか
- 動的コード生成(`System.Reflection.Emit`) の辺りは丸々新規追加
  - もちろん、動的コード生成できないプラットフォームはあるわけで、その辺りは[`RuntimeFeature` クラス](../../../2018/12/runtimefeature/index.md)の`IsDynamicCodeCompiled`, `IsDynamicCodeSupported` で判定
- `System.Drawing` にやたらと差分が多いのは、たぶん `SystemColors` をサポートしたせい
  - enum 値1個1個が「追加 API」にカウントされてそう
