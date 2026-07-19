---
title: "ピックアップRoslyn 2/3: csharplang リポジトリ"
source_url: "https://ufcpp.net/blog/2017/2/pickuproslyn0203/"
content_type: "BlogEntry"
published_at: "2017-02-03T20:58:30"
updated_at: "2017-02-03T20:58:30"
tags: []
umbraco_id: 2040
parent_id: 2036
sort_order: 1
aliases: []
---

# ピックアップRoslyn 2/3: csharplang リポジトリ

ようやく、言語設計に関するリポジトリを、[Roslyn](https://github.com/dotnet/roslyn)から分離する流れに。

- [C# Language Design](https://github.com/dotnet/csharplang)
- [Visual Basic .NET Language Design](https://github.com/dotnet/vblang)

ここまではOK。

Roslynリポジトリはコンパイラー実装に関するリポジトリなわけで、issueはバグ報告とかで埋まります。
今現在、3000件以上のissueがあって、そのうちかなりの割合がバグ報告なので、まあ、そりゃそんな場所で言語設計に関する話はできないですよね、というのは仕方がない話。
ユーザーからもリポジトリを分けてくれっていう要望は出ていますし、何よりC#チームがかなりGitHub issueの物量に参っているようです。
ということで、新たにcsharplangリポジトリが出来てみんな幸せに…？！

## メーリングリスト…

問題はここから。

曰く、

> Design Process
> 
> C# is designed by the C# Language Design Team (LDT).
> 
> 1. To submit, support, and discuss ideas please subscribe to the language design <em>mailing list</em>.

えっ、メーリングリスト！？

ということで、もちろん炎上中。

- [Language Design Process Moving (again) #16905](https://github.com/dotnet/roslyn/issues/16905)

反応はおおむね、

- リポジトリを分けるのには賛成
- メーリングリストはやめてくれ
- GitHub issueがつらい気持ちもわからなくはないけど、メーリングリスト、てめぇはダメだ

な感じです。

あまりの炎上っぷりにさすがにC#チームの中の人も再三の説明は繰り返していて、

- まず最初に、分離先にメーリングリストを使うという判断に関して、チーム内だけで決めたことは謝りたい
- C#チームの運用上GitHub issueはつらいものがある
  - 「何百もコメントが付くし、後から編集できるから後から事実を追うのもつらいし」などなど
- もちろんその代案がメーリングリストというのは、チーム内でも揉めた
- けど、どの案にも善し悪しあって、とりあえず当面、メーリングリストでの運用を容赦してはもらえないだろうか

という感じのようです。

## 私的な意見として

まあ、僕個人の意見としては、やっぱり「いろいろと気持ちはわかる。でも、メーリングリスト、てめぇはダメだ」ですねぇ…

もう、メールって言うメディアがダメ。
Windows付属のメールアプリとか、もうとてもじゃないけど使うのつらいじゃないですか。
メール ベースで何かやられたら、もうそんな場所に参加するだけで苦痛。

これはもう、「別のもっと良いアプリに移行すべき」とかそういうレベルの話ではなくて。
もう付属アプリが良くなる当てがない(そこに投資してもMicrosoftもユーザーも大して幸せにならない)という現実があるわけで。
Eメール自体に将来性がない。

そして、そういう将来性のないメディアでディスカッションしているプログラミング言語を使いたいかという問題になるんですが、
僕としては嫌。
使いたくない。

[去年の夏ごろにこのブログでも取り上げました](../../../2016/8/rickuproslyn0827/index.md)けど、その頃、
Mads (C#言語設計のPM)が[「What’s New in C# 7.0」っていうブログ記事](https://blogs.msdn.microsoft.com/dotnet/2016/08/24/whats-new-in-csharp-7-0/)のコメント欄で、

> C# needs to be among the greatest programming languages today, or it won’t be among them tomorrow.

> C#は今この時「最高のプログラミング言語」の1つでなければならない。 さもなくば、明日にはそうではなくなってしまう。

って言ってるわけですよ。
それが、もう、メールなんてメディア使ってる時点でだいぶ「今この時最高」からはずれる気分。

## そして、それを伝えたいけど…

今のGitHub issue運用がきついって言うのはかなりわかるし、かといってその代案としても決め手に欠けているってのもわかるので、あんまり非難はしたくないんですけども。
なので、今の苦労をねぎらいつつも、「メールだけは勘弁してくれ、C#っていう言語のブランディングに関わる」って話をしたいんですが…

これを、まあ、つたない英語力で英訳すると、

> Mailing lists are f***'in

の1行に縮まりかねないわけでして…
バグ報告とかの類と比べて難易度高い…

### おまけ

そんな感じのことをぼやいてたら、英ペの勇さんがRTしてたので、きっと翻訳してくれるんだと思います。

[![英ペの勇さん](../../../../../assets/media/1125/eipe.png)](https://twitter.com/ufcpp/status/827337737812340736)
