---
title: ".NET Core 3.0 をもって .NET Framework からの移植作業は完結"
source_url: "https://ufcpp.net/blog/2019/10/netcore3concludesfxporting/"
content_type: "BlogEntry"
published_at: "2019-10-15T19:50:14"
updated_at: "2019-10-15T19:50:14"
tags: []
umbraco_id: 2270
parent_id: 2268
sort_order: 1
aliases: []
---

# .NET Core 3.0 をもって .NET Framework からの移植作業は完結

corefx で以下のようなアナウンスが。

- [.NET Core 3.0 concludes the .NET Framework API porting project](https://github.com/dotnet/corefx/issues/41769)

[build](../../5/build2019/index.md)の時点で .NET を .NET Core ベースに一本化、.NET Framework は 4.8 をもって最後にするという話があったわけですが、
改めてというか、総括的なアナウンスです。

## API 数

まず、.NET Framework から .NET Core に移植してきた API 数の総括。
メソッドのオーバーロード1個1個を「1 API」とカウントしてるんだともいますが、以下のような数字が書かれています。

- .NET Core 1.0 時点では1.8万個
- .NET Standard 2.0 では .NET Framework、.NET Core、Xamarin の共通部分として3.8万個
- Windows 限定な機能も [Windows Compatibility Pack](https://devblogs.microsoft.com/dotnet/announcing-the-windows-compatibility-pack-for-net-core/) として提供して、これが2.1万個
  - .NET Standard 2.0 と合わせて6万個
- .NET Core 3.0 では WPF と WinForms を移植して、これが12万個
  - .NET Framework の API の約半分
- 依然として6.2万個の API が未移植なものの、8割の API が移植済み

## 移植しないもの

これも build の時点でアナウンス済みですが、AppDomain、remoting、Web Forms、WCF、Workflow は .NET Core への移植をしません。

この辺りはレガシー扱いで、
モダンなアプリ開発に必要なテクノロジーは .NET Core 3.0 をもって一通りそろったといえる段階に達したと判断されています。
今後はもう、レガシー移植よりも、新しいテクノロジーに開発リソースを割いていきます。

.NET Framework のコード自体は[Microsoft .NET Reference Source](https://github.com/microsoft/referencesource) として MIT ライセンスで GitHub 上にあるので、
Microsoft が保守を止めてしまった部分も、コミュニティ ベースで保守していくことはできます。
(実際、[Core WF](https://github.com/UiPath/corewf)や[Core WCF](https://github.com/CoreWCF/CoreWCF)など、いくつかのコミュニティ プロジェクトがすでにあります。)

ちなみに、[Reference Source](https://github.com/microsoft/referencesource)は本当にソースコードの公開のみで、ビルド基盤をもう保守していないので、
ビルドして動くものが欲しければ[Core WCF](https://github.com/CoreWCF/CoreWCF)などのコミュニティ プロジェクトを参照(なければ立ち上げ)してほしいそうです。

## 移植の要望 issue を close

「このテクノロジーを移植してほしい」という要望 issue があって、
これまで「どれを優先的に移植するか」の判断材料として使ってきました。
しかし、.NET Core 3.0 をもってもう移植作業は打ち切りということで、issue も一通り close したそうです。

ただ、打ち切ったといってもあくまで WCF であるとか Web Forms であるとかのテクノロジー単位での話で、
例えば「移植済みのクラスのこのオーバーロードがないみたいだけども、あった方が便利じゃない？」くらいのものであれば追加される可能性はあります。

「.NET Framework にあるから」という理由だけで .NET Core に追加されることはなくなりますが、同時に、「.NET Framework にある(けどまだ .NET Core にない)から」というだけで絶対にその API が移植されないということもありません。
