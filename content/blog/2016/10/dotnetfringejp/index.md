---
title: ".NET Fringe Japan 2016で登壇してきました"
source_url: "https://ufcpp.net/blog/2016/10/dotnetfringejp/"
content_type: "BlogEntry"
published_at: "2016-10-02T06:54:15"
updated_at: "2016-10-02T06:54:15"
tags: []
umbraco_id: 1964
parent_id: 1963
sort_order: 0
aliases: []
---

# .NET Fringe Japan 2016で登壇してきました

[.NET Fringe Japan 2016](http://dotnetfringe-japan.connpass.com/event/35659/)やりました。

[.NET Fringe](http://dotnetfringe.org/)っていう本家イベント(アメリカのポートランドで開催)があって、本家の人からそれの日本版をやりませんか？という話が来てそれに応じた形での開催だったりします。

アメリカのイベントって朝からがっつりやるんですよね。本家.NET Fringeは9時開始。日本でもたまにはがっつりやるかって感じで10時開始になりました。
まあ、逆に、アメリカのイベントは17時前に終わるんですが…10時～20時半はさすがにきつかったかなぁ。という感じも。

## C#言語機能の作り方

で、僕のセッション。

<div style="width: 608px; max-width: 100%; margin-bottom:5px;"><a href="https://docs.com/iwanaga-nobuyuk/1121" title="C#言語機能の作り方" target="_blank" style="font-family: 'Segoe UI'; font-size: 13px; text-decoration: none; margin-left:18px ">C#言語機能の作り方</a><span style="font-family: 'Segoe UI'; font-size: 13px ">—</span><a href="https://docs.com/iwanaga-nobuyuk" target="_blank" style="font-family: 'Segoe UI'; font-size: 13px; text-decoration: none ">Iwanaga Nobuyuki</a></div><iframe src="https://docs.com/d/embed/D25192700-1492-1911-6360-000919169843%7eMd2f0fde0-d68b-9095-2ec5-841305bd4fb1" frameborder="0" scrolling="no" width="608px" height="378px" style="max-width:100%" allowfullscreen="True"></iframe>

作り方とかいいつつ、むしろ全力で止めにかかってる予防接種セッション。

⚠使用上の注意

- 「独自に言語を作ろう・拡張しよう」みたいな話はハシカみたいなもんです
  - 早めに免疫つけといた方がいい
  - 歳食ってから罹ると重篤なことが

という。

とはいえ、やるだけ無駄とかやっちゃダメって話ではなく。むしろ、早めに罹っとけって話です。予防接種も、弱毒化したウィルスにあらかじめ罹ることで免疫付ける手法ですしね。

良し悪しとかどのくらい大変かとかはある程度の勘所がないと、いざ責任ある立場になって大きなことやれるようになった時に下手打つと命に係わるぞと。
この辺りは別にプログラミング言語作りに限らず一般論ですね。
いろいろ試しにやってみて、いろいろ勘をつかんでおくのは大事です。

しいて言うなら、なんかプログラミング言語作りでは、この「どのくらい大変か」を過少見積もりして重篤化する人がなんか一定割合いるっぽく、予防接種スライド作っとくか、と。
わざわざ大変なものに手を出さなくても罹った気分になっておくためのものが、僕が今回やったようなセッションですかね。

「ハシカみたいなもんだから」ってセリフ、いろんな方面に結構ぐさぐさ刺さったみたいですがｗ
@kekyo さん、「新しい opcode 作るよ」って話([スライド](http://www.slideshare.net/kekyo/beachhead-implements-new-opcode-on-clr-jit)、[ニコ生放送](http://live.nicovideo.jp/watch/lv277167391)の8時間目くらいから)をしてて「ハシカですね…」って言ってましたけども、
これは割かし良い具合にワクチン(弱毒化したウィルス)なんじゃないですかね。
最小限に手を入れてみるってのは勘所つかむためのかなり良い題材。
まあ、セッションの後、「予防接種ってのはそれなりに流行る可能性のあるものだから必要なんであって、これはハシカと言えるのか(そんなにやりたがる人いるのか)」とかいう鋭い突っ込みもあったんですが。

あと、試しにやってみているだけでも、@neueccさん([スライド](http://www.slideshare.net/neuecc/what-why-how-create-oss-libraries-30coss)、[ニコ生放送](http://live.nicovideo.jp/watch/lv277167391)の7時間目くらいから)が言ってたみたいに、公開するつもりでやる/実際に公開する方が身につく度合いが高いと思います。
まず、人に見せる/人に見られることを意識した方が断然勉強になります。
それにどうせお試しで実用性皆無なものであっても、公開していると「予防接種済み」の査証になりますし。結構価値あるんじゃないかと。
