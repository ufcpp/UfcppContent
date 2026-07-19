---
title: "ピックアップ Roslyn 6/10"
source_url: "https://ufcpp.net/blog/2015/6/pickuproslyn0610/"
content_type: "BlogEntry"
published_at: "2015-06-10T08:23:48"
updated_at: "2015-07-07T19:34:01"
tags:
  - "ピックアップRoslyn"
umbraco_id: 1748
parent_id: 1745
sort_order: 2
aliases: []
---

# ピックアップ Roslyn 6/10

## Numeric literals (binary and digit separators) #2950 

[https://github.com/dotnet/roslyn/pull/2950](https://github.com/dotnet/roslyn/pull/2950)

[先月くらいにちょこっと書いた](../../5/pickuproslyn20150514/index.md)やつですが、2進リテラルの実装。

pull-req 出してる人がマイクロソフトにインターン中の学生っぽく、インターンの課題かなぁというやつ。マージがうまくいかなかったのか、一度Closeして別のpull-reqが出しなおされてますが、最近になって急にレビューがつくようになって、今、「LGTM」(Looks Good To Me: 私はいいと思う)コメントが付いたところ。

C# 6.0/Visual Studio 2015 RTMには乗らないやつですが、futureブランチとかに取りこまれていくのかな、たぶん。

## Local functions #3372 

[https://github.com/dotnet/roslyn/pull/3372](https://github.com/dotnet/roslyn/pull/3372)

もう1個、同じインターンの学生さんのタスクっぽい新機能実装。

ローカル関数(メソッド内に、そのメソッド中だけのスコープでメソッドを定義できる)を実装したみたい。こちらは今日、レビューを終えて、「[local-functions](https://github.com/dotnet/roslyn/tree/features/local-functions)」って名前のブランチにマージされた模様。大きな問題が出なければ、このままC# 7.0にとりこまれるのかな。

## [C# Feature Request]Multilingual XML Document Comment #3371 

[https://github.com/dotnet/roslyn/issues/3371](https://github.com/dotnet/roslyn/issues/3371)

XMLドキュメント コメントに翻訳入れさせてくれという要望。

これ、前々から言ってるんですよねぇ。[Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/)とかには多言語対応(日本語記事と英語記事をちゃんと両方書いて、同じIDに紐づけておける)入れてるんだから、ソースコードにも多言語対応できる仕組み入れてって。

現状、GitHubとかで公開して、それをできる限り多くの人に使ってほしかったら、全コメント英語で書くのが無難になってしまい。まあ、あきらめて英語で書くんですけど。

## async sequences

もう2週間前の話になってしまうんですが、async sequences に関する議論の中で、proof of conceptにasync iteratorsの実装書いてみたよって人が。

[https://github.com/dotnet/roslyn/issues/261#issuecomment-105960240](https://github.com/dotnet/roslyn/issues/261#issuecomment-105960240)

今のところC#チームの中の人っぽい応答は0。代わりに[dsyme](https://github.com/dsyme) (MSリサーチのF#の人)が食いついてたりはしますが。
