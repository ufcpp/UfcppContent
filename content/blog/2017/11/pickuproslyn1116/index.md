---
title: "ピックアップRoslyn 11/16: Microsoft Connect(); 2017"
source_url: "https://ufcpp.net/blog/2017/11/pickuproslyn1116/"
content_type: "BlogEntry"
published_at: "2017-11-16T13:49:35"
updated_at: "2017-11-16T13:50:51"
tags: []
umbraco_id: 2106
parent_id: 2098
sort_order: 1
aliases: []
---

# ピックアップRoslyn 11/16: Microsoft Connect(); 2017

[Connect](https://www.microsoft.com/ja-jp/events/connect2017.aspx) の1日目がありましたが。

## Live Share

今回一番気になったのは、[Visual Studio Live Share](https://code.visualstudio.com/blogs/2017/11/15/live-share)ですかね。
Visual Studio 2017とVisual Studio Codeで、コーディングやデバッグをリアルタイムに画面共有するコラボツール。

複数のオフィスに分かれるような大企業か、大々的にリモート勤務を推してるような会社でないといきなりは使い道ないでしょうけども。
自分も今務めてるのは1フロアに収まる規模のチームですし、基本的には「開発者はフロアをまたぐと開発に支障が出る」というポリシーなんですけども。
とはいえ、「育休で1年だけリモート」みたいなのは十分あり得るので。

## C# 

まあ、このサイトとしてはこっちが主役。C# 7.2と、その先の話題。

### What's new in C# 7.2

[Visual Studio 15.5 Preview](../../10/visualstudio15_5/index.md)が出た時点からC# 7.2は試せるわけで、
「普段から追っていれば」新しい話題も特にないんですけども。

ただ、最近ドキュメントがらみが後手に回りがちな C# チームが、今回はちゃんとConnectに合わせて仕事しましたよ！

- docs 上に[What's new in C# 7.2](https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-7-2)
- [Welcome to C# 7.2 and Span](https://blogs.msdn.microsoft.com/dotnet/2017/11/15/welcome-to-c-7-2-and-span/)

### null許容参照型

あと、null許容参照型 (参照型でも修飾なしのは「非null」扱いにして、`?` を付けて初めて nullableにするってやつ)のプレビューがアナウンスされました。

- [Introducing Nullable Reference Types in C#](https://blogs.msdn.microsoft.com/dotnet/2017/11/15/nullable-reference-types-in-csharp/)
- [Nullable Reference Types Preview](https://github.com/dotnet/csharplang/wiki/Nullable-Reference-Types-Preview)

短縮URLまで取って: [aka.ms/nullable-preview](https://aka.ms/nullable-preview)

(これも、このドキュメント自体は数週前から着々と準備が進んでたんですけど、正式に。)

こちらはだいぶ先を見た話です。
null許容参照型という機能自体、予定としては C# 8.0 でのリリースを目指している機能で、7.2すらプレビューな現状からするとだいぶ先のものの、かなり早い段階のプレビュー版です。
15.5 Preview を入れれば割かし低リスクで試せる C# 7.2 と違って、
別途インストーラーを実行して Visual Studio (の参照しているコンパイラー)を上書きするタイプなので注意が必要です。
