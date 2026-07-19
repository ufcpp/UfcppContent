---
title: ".NET Global Tools"
source_url: "https://ufcpp.net/blog/2018/2/dotnettoolspkgs/"
content_type: "BlogEntry"
published_at: "2018-02-28T20:54:15"
updated_at: "2018-03-25T09:27:27"
tags: []
umbraco_id: 2133
parent_id: 2132
sort_order: 0
aliases: []
---

# .NET Global Tools

[.NET Core 2.1 の Preview 1 が公開されたそうで](https://blogs.msdn.microsoft.com/dotnet/2018/02/27/announcing-net-core-2-1-preview-1/)。

以前から、daily build の不安定な奴で良ければ試せていたんですが、オフィシャルにアナウンスがあったということは、作業が一区切りしたということでしょう。

実際、今回の主題の Global Tools は、以前、daily build で試したときには全然動いていませんでした。
ということで、今日、やっと動いたので試してみたという話。

## Global Tools

.NET Core 2.1 の、`dotnet` コマンドの新機能の1つです。

[NPM global tools](https://docs.npmjs.com/getting-started/installing-npm-packages-globally)にインスパイアされて作ったよ、というもの。

要するに、`dotnet`コマンドを使って、NuGet 越しにインストール可能なコマンドラインツールを提供するための仕組み。

試しに作ってみたものがこちら:

- [DotNetGlobalTools](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/DotNetGlobalTools)

## ツールの作り方

`csproj` の `PropertyGroup` に、[以下のような行](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2018/DotNetGlobalTools/cszip/cszip.csproj#L6)を足せばいいらしい。

```
    <PackAsTool>true</PackAsTool>
```

ただ、現状、Visual Studio の方はまだ対応していなくて、これを入れたプロジェクトをビルドしてもそのままだとパッケージは作られません。`dotnet pack`コマンドを手打ちする必要があります。

```
dotnet pack -c release cszip/cszip.csproj
```

詳しくは、[公式サンプル: dotnetsay](https://github.com/dotnet/core/blob/master/samples/dotnetsay/README.md)を参照。

## ツールのインストールの仕方

`dotnet install tool` コマンドでインストールできます。

```
dotnet install tool -g cszip --configfile .\nuget.config
```

NuGet を使ってパッケージを取ってきて、ユーザー フォルダーの下の `.dotnet/toolspkgs` と `.dotnet/tools` 以下にもってきて使うようです。
作ったツールは[nuget.org](https://www.nuget.org/)にアップロードするもよし、
ローカル フォルダーを [NuGet パッケージ ソースに指定](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2018/DotNetGlobalTools/nuget.config#L5)するもよし。

上記の例では、設定ファイルを指定して、ローカルのフォルダーから作ったツールをインストールしています。

## ツールの使い方

`.dotnet/tools` にはパスが通っているので、作ったコマンド ライン アプリがどこからでも呼べるようになっています。
普通にコマンドをたたけば呼べます。

```
cszip packages sample.zip
```

## 試しに作ってみたもの

- [cszip](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/DotNetGlobalTools/cszip): `ZipFile.CreateFromDirectory` を呼んでるだけ
- [csunzip](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/DotNetGlobalTools/csunzip): `ZipFile.ExtractToDirectory` を呼んでるだけ
- [xstatic](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/DotNetGlobalTools/xstatic): .NET 標準ライブラリ中の任意の静的メソッドを呼び出せる

割と最近、社内で C# で書いたコマンド ライン ツールを、
他社の方に使って貰わないといけない事案がありまして。
その時のやり取り:

- 手元でビルドしたバイナリを毎回手渡しするのしんどいです
- 先方も、Mac だけど dotnet コマンドは使えるのよね？ .NET Core SDK はインストールしてもらえてて。ソースコードは共有してるんだし、向こうで `dotnet build` してもらったら？
- 試してみてもらったんですが、PostBuild イベントで呼んでるコマンドが PowerShell なので Mac で呼べませんでした

呼んでるのは Compress-Archive でした。
Cygwin でも入れて zip コマンドに変えれば Mac でも動きそうなものの。
Windows だと bat を書いて、Mac とかだと sh を書いてとかもできはしますが、書くのはいいけど動作確認が大変で。
あと、最近は [PowerShell も .NET Core 化](https://docs.microsoft.com/ja-jp/powershell/scripting/whats-new/what-s-new-in-powershell-core-60?view=powershell-6)してるのでこれを入れてもらうという手もなくはないものの。
そこまでやるか…と悩んだ結果、手渡し運用を続行。

ということで、
`dotnet` コマンドでインストールできて、`dotnet` コマンドで実行できるものがあるならそれを使くて、
Global Tools の仕組みを使ってみようかということで作ったのが cszip と csunzip。

で、zip/unzip でコマンド分けるべきか、
分けるの面倒ではないか、
というかむしろいっそのこと任意の静的メソッド呼べるようにしてやろうか。
等々、遊んでみてた結果が xstatic の方です。

インスパイア元の NPM Global Tools も似たような動機ですよね、きっと。
node.js でインストールできて、node.js で実行できるツールが欲しいという。
