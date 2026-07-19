---
title: "Visual Studio 16.2 Preview 2 & .NET Core 3.0 Preview 6"
source_url: "https://ufcpp.net/blog/2019/6/vs16_2_p2/"
content_type: "BlogEntry"
published_at: "2019-06-13T22:37:08"
updated_at: "2019-06-13T22:57:18"
tags: []
umbraco_id: 2251
parent_id: 2250
sort_order: 0
aliases: []
---

# Visual Studio 16.2 Preview 2 & .NET Core 3.0 Preview 6

[Visual Studio 16.2 Preview 2](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.2.0-pre.2.0) と [.NET Core 3.0 Preview 6](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-6/) が来てますね。

Visual Studio 16.2 Preview 2 の方は、自分が気になったのだと、`switch`ステートメントを[`switch`式](../../../../study/csharp/cheatsheet/ap_ver8.md#switch-expression)に書き換えてくれるリファクタリング機能が入ったとかあるみたいです。

.NET Core 3.0 Preview 6 は、パフォーマンス カウンターで GC とかスレッド周りの詳細な情報が取れるようになったり、AOT シナリオで「使ってなさそうなコードを消す」系の最適化が増えたり、HTTP/2 サポートが入ったみたいです。

[WPFのオープンソース化](https://github.com/dotnet/wpf)(リポジトリができた時点ではスカスカだった)も完了したとのこと。

※追記: .NET Core 3.0 Preview 6 をインストールすると、[WPF アプリ(の .NET Core 版)が英語ロケールでしか動かなくなるみたいです](https://twitter.com/ufcpp/status/1139167755242881029)。ご注意を。

あと、[null許容参照型に関連する属性](https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/Diagnostics/CodeAnalysis/NullableAttributes.cs)が標準で入ったみたいです。
ただ、これは型として存在しているだけで、C# コンパイラー側が解釈できるようになるのは 16.2 Preview 3 以降随時みたいです。

## C# 8.0 in 16.2 Preview 2

C# 的には、[Preview 1の頃](../../5/vs16_2_p1/index.md)にあったやばいバグは治りました。
(`stackalloc`を使っただけで不正な IL を生成して実行できなくなる問題。)
これで今度こそ、気兼ねなく[式中の stackalloc](../../../../study/csharp/cheatsheet/ap_ver8.md#nested-stackalloc)を試せます。

あと、ひそかに今回から入ったのが、

- [Support re-abstraction of interface members in derived interfaces #35756](https://github.com/dotnet/roslyn/pull/35756)

です。
以下のようなやつ。一度デフォルト実装を持ったメソッドを、もう1度抽象メソッドに変えて、派生側での実装を必須に変える機能。

<div>
<script src="https://gist.github.com/ufcpp/26d6eda8b1bdf91cc785bd7478d95b89.js"></script>
</div>

前述の通り[null許容参照型に関連する属性](https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/Diagnostics/CodeAnalysis/NullableAttributes.cs)は .NET Core 3.0 側には入ったわけですが、
C#コンパイラーが対応し出すのは次の Preview 以降みたいです。
スケジュール感は以下のページに書かれている通り。

- [Nullable Reference Type Changes #35816](https://github.com/dotnet/roslyn/issues/35816)

6/10 で終わっている作業はたぶん 16.2 Preview 3 で入ります。

ちなみに、以下のような issue もあります。

- [Change C# 8 to be the default, non-experimental language version #36140](https://github.com/dotnet/roslyn/issues/36140)

要するに、16.3 で C# 8.0 の preview が外れ、default が 8.0 になるという予告。
[build](https://www.microsoft.com/en-us/build)で「7月にRC、9月にGA」という話をしていたんで、それがそれぞれ 16.2 (で C# 8.0 も RC)、16.3 (で GA)ということなんだと思います。
