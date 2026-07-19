---
title: ".NET 5 の Target Framework と Optional SDK Workload"
source_url: "https://ufcpp.net/blog/2020/3/net5optionalworkload/"
content_type: "BlogEntry"
published_at: "2020-03-25T23:03:08"
updated_at: "2020-03-25T23:03:08"
tags: []
umbraco_id: 2285
parent_id: 2283
sort_order: 1
aliases: []
---

# .NET 5 の Target Framework と Optional SDK Workload

まだ提案ドキュメントが出たばかりな段階ですが、 .NET 5 の方向性を決めるドキュメントが2件出ています。

- [Target Framework Names in .NET 5](https://github.com/terrajobst/designs/blob/master/accepted/2020/net5/net5.md) (割かし最近 Merge されたて)
- [.NET Optional SDK Workloads](https://github.com/dotnet/designs/pull/100) (Pull Request 出たて)

ちなみに、Target Framework Name と言いつつ略称が TFM なのは最後が Moniker (あだ名)だからです。
.NET Core に対して `netcoreapp`、 .NET Standard に対して `netstandard` みたいな記号的な名前を振っていたわけですが、これが Moniker です。

## 標準ライブラリ統合

まず背景おさらい的な話になりますが、
紆余曲折経て、 .NET が単一系統に統合されます。

.NET Framework (Windows 専用)ででしかできないことはほとんどなくなり、こちらは保守モードに入って、 .NET Framework 4.8 が最終バージョンになります。

ランタイムとしては .NET Core 系と Xamarin (Mono)系がまだ残っていますが、
少なくとも標準ライブラリのコードは統合する方向で動いています。

そのため、かつて混とんを極めた Portable Class Library の頃の `portable-net40+sl5+win8` (サポートしたいターゲットを全列挙)みたいなものも、
.NET Standard (.NET Core, .NET Framework, Xamarin の最大公約数を取ったもの)みたいなものも必要なくなる予定です。

そこで、名称も「.NET 5」(Core も Framework も Standard も付かず、バージョン番号は 5)となりますし、
TargetFramework 名もシンプルに `net5.0` を使う予定です。

(提案されたばかりなので今はまだ実装されていません。
今現在出ている .NET 5 Preview 1 ではまだ `netcoreapp5.0` という TFM を使っています。)

## TargetFramework 名

ということで、どのプラットフォームでも使える基礎的な部分を指して `net5.0` という TargetFramework を使います。
これまででいう `netstandard2.1` みたいなものが無印の `netX.Y` みたいな形式になります。

(.NET Framework の方では `.` を入れずに `net472` とか書いていたので、 .NET 10 が来たら(6年は先の話ですが)ちょっとだけ混乱を生みそうですが、
おそらく `net10` だけを 1.0 使いする特殊処理(10にしたければ `net10.0` と書かないとダメ)が入りそうです。)

一方で、 .NET Core 3.1 でも WPF や Windows Forms は Windows 限定の機能ですし、
Xamarin を統合するということは Android 限定機能や iOS 限定機能が入ることになります。

これら特定プラットフォーム限定機能向けに、`net5.0-android`, `net5.0-ios`, `net5.0-windows` みたいな TargetFramwork 名を与えるみたいです。
ちゃんと `-` の前後で分離して解釈するそうで、例えば、`net5.0` (.NET 5.0 汎用)なライブラリを、`net6.0-android` (.NET 6.0 + Android 限定機能)から参照できます。
さすがに。PCL の時のようにはならないように。

ちなみに、`-` の後ろのプラットフォーム名の方にもバージョン番号を指定できるそうです。
例えば、`net5.0-ios14.0` みたいな書き方で「 .NET 5.0 で、 iOS のバージョン 14.0」という意味になります。
プラットフォームの方のバージョンがないときは「その .NET がサポートしているもっとも低いバージョン」扱いになるみたいです。

## Optional SDK Workload

.NET SDK も分割したいようです。
今でも、「コンソール アプリしか使わないのに」、「WPF しか使わないのに」、「ASP.NET しか使わないのに」みたいに思うことは多いと思います。
ここに Xamarin.Android や Xamarin.iOS も加わるわけで、そろそろ、必要な分だけを追加インストールする仕組みが欲しい段階です。

ということで、特定プラットフォーム向けの SDK 機能を workload と呼んでいて、
workload は optional (オプション指定しない限りインストールされない)な状態にしたいそうです。

さすがに時代的に、最初からゴールとして「できる限り CI/CD フレンドリーにしたい」というものが含まれています。
「使う分だけインストールすればいい」というサイズ削減の意味でも分けた方が CI/CD 的にやさしいですし、
インストールをできる限り簡単にしたいという目標もあるみたいです。
最終的には、`dotnet bundle install [bundle name]` とか、`dotnet bundle restore [project name]` みたいな1コマンドで Optional SDK Workload をインストールできるようにしたいそうです。
(同様の仕組みは workload よりも広い用途に使えるかもしれないので、サブコマンド名はより広い名前、まだ決めかねてるそうですが仮に bundle と呼んでいます。)

ここで、`dotnet bundle restore` の方は、前節で説明したような TargetFramework を見て、
例えば、`net5.0-android` なら Android 向けの、`net5.0-ios` なら iOS 向けの workload (bundle)の復元できるようにしたいそうです。

要は、今現在、ライブラリ参照のために NuGet を使ってやっているのと同程度の手軽さで、SDK workload の参照ができるようにしたいそうです。

### .NET 5 次点での目標

.NET 5 次点では、
workload のインストールは Visual Studio (for Windows と for Mac)でだけになるかもとのこと。
`dotnet bundle restore` みたいなコマンドは一部分だけ。

また、 .NET 5 から追加されることになる Xamarin 系統(要するに Android、iOS、Web Assembly など)の workload が Optional 提供になる予定です。
(まだこの時点では ASP.NET や WPF などの分離作業は手付かず。)

### もっと先の目標

最終的には、
`dotnet` コマンドラインツールのフル機能を提供したいし、
apt-get などの Linux 向けインストール ツールの類を使った workload インストールができるようにもしたいみたいです。

また、 .NET Core 3.1 次点ではモノリシックな SDK 提供になってしまっている ASP.NET や WPF、Windows Forms などの workload も Optional 提供に変更したいそうです。
