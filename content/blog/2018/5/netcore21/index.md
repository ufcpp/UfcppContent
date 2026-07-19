---
title: ".NET Core 2.1 正式リリース"
source_url: "https://ufcpp.net/blog/2018/5/netcore21/"
content_type: "BlogEntry"
published_at: "2018-05-31T14:43:45"
updated_at: "2018-05-31T14:43:45"
tags: []
umbraco_id: 2156
parent_id: 2150
sort_order: 5
aliases: []
---

# .NET Core 2.1 正式リリース

.NET Core 2.1 が正式リリースされたみたいですね。

- [Announcing .NET Core 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/05/30/announcing-net-core-2-1/)

内部的にかなりパフォーマンス改善してるとか、
[.NET Global Tools](../../2/dotnettoolspkgs/index.md)が使えるとか、
[SourceLink](https://github.com/dotnet/sourcelink)に対応したらしいとかいろいろありますが。
C# 的に直接的に関わってくるのは[`Span<T>`](../../../../study/csharp/resource/span.md)構造体のリリースでしょうか。

[C# 7.2](../../../../study/csharp/cheatsheet/ap_ver7_2.md)で、`Span<T>`がらみの言語機能がいろいろ入っているんですが、
肝心の`Span<T>`自体がプレビュー状態でした。
([.NET の基本ライブラリ](https://github.com/dotnet/corefx)自体がC#で書かれてる部分が多いので、
先に言語機能が入ってくれないと`Span<T>`周りの最適化がやりにくいので。)
それが今日、正式リリースとなりました。

[`Span<T>`](../../../../study/csharp/resource/span.md)構造体は以下の環境で使えます。

- .NET Core 2.1 では標準で使える
- [System.Memory パッケージ](https://www.nuget.org/packages/System.Memory/) を参照すれば、古い .NET ランタイム上でも使える
  - .NET Standard 1.1、.NET Framework 4.5とかにも対応
- .NET Core 2.1 上で実行すると特にパフォーマンスがいい([fast Span](../../../../study/csharp/resource/span.md#fast-span))
