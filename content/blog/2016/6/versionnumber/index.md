---
title: "C# 6, C# 7"
source_url: "https://ufcpp.net/blog/2016/6/versionnumber/"
content_type: "BlogEntry"
published_at: "2016-06-11T03:46:53"
updated_at: "2016-06-11T03:50:30"
tags: []
umbraco_id: 1910
parent_id: 1908
sort_order: 1
aliases: []
---

# C# 6, C# 7

最近、C#がらみの原稿仕事を依頼されたとき、よくある校正が「C# 6になってますけども、6.0ですよね？」。
回答は「いえ、C# 6が正式です」。

C#のバージョン番号、6以降は .0 を付けないのが正式っぽいんですよね。実は。

- C# 1.0
- C# 1.1 (`#line`ディレクティブと`/** doc comment */`が追加されただけ。1.1なのか1.2なのかも割かしよくわからず)
- C# 2.0
- C# 3.0
- C# 4.0
- C# 5.0
- C# 6
- C# 7

という感じ。

実質的に小数点以下使ってない(1.1とか存在感ない)し、要らないですよね。という感じ。

これに気づいたの、結構後になってからなんで、うちの[C#によるプログラミング入門](../../../../study/csharp/index.md)にも「C# 6.0」って表記が結構残ってたんですが、今しがた整理。ブログ以外からは6.0が消えたはず。

## 実はよくわからない

ただ、C# 7の次がC# 7.1とかになる可能性は0ではないです。「TypeScriptとか0.1刻みの細かいリリースしてるよね」みたいな話題もあるし、C#も1年1回リリースくらいのペースに速めようなんて話が出てますし。パターン マッチングの構文を、7で追加するものと、その先に回すものを分けたとかいうのもありますし。

.NET Frameworkも、「.NET 4」ってブランド名にしたら、その次からが「.NET 4.5」、「.NET 4.5.1」、「.NET 4.6」、…とかだったなんて話もあります。

あと、C#チームは現体制([GitHub上で公開](https://github.com/dotnet/roslyn)、[Mads](https://github.com/MadsTorgersen)がPM)になってから、この手のアナウンスが手抜きなんですよね。気が付いたら「あれ？そうだったの？」みたいなことが割とちらほら。

単に、「6.0と6に何の差もないだろう」的な発想なのかも。例えば以下のページなんて、C# 1, C# 2, C# 3, ...表記。

- [What's New for Visual C#](https://msdn.microsoft.com/en-us/library/hh156499.aspx)

ってことで、自分も全部C# 1, C# 2, C# 3, ...表記に変えたい気持ちありつつ。まあ、一応過去への遡及はしないことにします… 6以降だけ、.0を取る方針で。

## おまけ: VB

Visual Basicのバージョンもなかなかよくわからないみたいですね。

C#は、言語のバージョンとIDEのバージョンを分けて表記してますけど、「Visual Basic」っていうIDE製品があった頃の名残で、言語とIDEの区別をしないみたい。その結果何が起こるかというと…

まあ、以下のページを見てください。

- [What's New for Visual Basic](https://msdn.microsoft.com/en-us/library/we86c8x2.aspx)

書かれてるのは以下の通り。

- Visual Basic / Visual Studio .NET 2002 (内部バージョン7)
- Visual Basic / Visual Studio .NET 2003 (7.1)
- Visual Basic / Visual Studio .NET 2005 (8)
- Visual Basic / Visual Studio .NET 2008 (9)
- Visual Basic / Visual Studio .NET 2010 (10)
- Visual Basic / Visual Studio .NET 2012 (11)
- Visual Basic / Visual Studio .NET 2013 (12)
  - このとき、コンパイラーは全く更新してません
- Visual Basic / Visual Studio .NET 2015 (14)
  - 13を嫌って、内部バージョン1つ飛ばして14

で、VBのバージョンとしては、Visual Studioの内部バージョンで(2002みたいな年号表記じゃなくて、7みたいなの)表します。
問題は、2013。VBのコンパイラーは一切変更してないのに、こいつのことをVB 12って言うみたいです。
しかもその後、13っていう忌み数を避けて内部バージョンを14にしたもんだから、VBもVB 14に。
