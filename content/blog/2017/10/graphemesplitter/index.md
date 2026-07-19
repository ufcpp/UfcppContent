---
title: "絵文字の連結と、書記素クラスター判定"
source_url: "https://ufcpp.net/blog/2017/10/graphemesplitter/"
content_type: "BlogEntry"
published_at: "2017-10-29T16:40:16"
updated_at: "2017-10-29T16:43:02"
tags: []
umbraco_id: 2092
parent_id: 2084
sort_order: 3
aliases: []
---

# 絵文字の連結と、書記素クラスター判定

[みんな絵文字好きすぎだろ…](https://twitter.com/ufcpp/status/923379224223793152)

というRT状況なわけですが。

## emoji zwj sequences

元々、👪 (U+1F46A)という、1文字で家族を表す絵文字があったわけですが。

「白人の絵しかないのはおかしい」とか「LGBT に配慮しろ。なぜ男女ペアしかないんだ」とかいろいろと地雷になってしまった結果、
[合字で解決しよう](http://www.atmarkit.co.jp/ait/articles/1604/27/news051.html)とかいう仕様が Unicode に入ってしまって今に至っているわけですが。

ちなみに、単に合字になるというだけじゃなくて、
「合字は1文字として扱え」という仕様も決まっています。
仕様は以下のページにあり。

- [Unicode® Standard Annex #29 UNICODE TEXT SEGMENTATION](http://unicode.org/reports/tr29/)

まあ、仕様があるといってもそれにアプリが対応しているかどうかというとまちまち。
以下の動画でのChromeの動作みたいに、「ページ中ではちゃんと1文字として扱う」、「アドレスバーでは家族1人1人がバラバラに」という挙動になったり。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/9SCONCSjMnU" frameborder="0" gesture="media" allowfullscreen></iframe>
</div>

で、この[Annex #29](http://unicode.org/reports/tr29/)の仕様では、
家族どころか、絵文字を ZWJ (zero width joiner: ゼロ幅接合子)で繋げば何人でもつながります。
これが、emoji zwj sequences というやつ。

そして、ZWJ でいくらでもつながるんですけど、それをグリフ的にどう描画すべきかというとまたこれが難しくて、その結果が[冒頭のツイート](https://twitter.com/ufcpp/status/923379224223793152)の内容。

![emoji zwj sequences](https://pbs.twimg.com/media/DNCAUS3VQAAY3yV.jpg:large)

## 「合字は1文字として扱え」仕様 … 書記素判定

ちなみに、「グリフ的に1つにくっつくから、ユーザーから見ると1文字に見える」、
「なので、カーソル移動や削除の際には1文字として扱え」というものを書記素(grapheme)といいます。

[Annex #29](http://unicode.org/reports/tr29/)は「書記素の区切りはどこか」(と、単語の区切り、センテンスの区切り)を判定するためのルールを決めたものです。
先ほどの動画を見ての通り環境次第ではあるんですが、最近のOSは結構ちゃんとこの「書記素の区切り判定」を実装しています。

## C# で書記素判定するライブラリ

でも、この辺りは割かし新しい仕様な上に、Unicode のバージョン依存が激しく、あんまり実装したい類の処理ではなく。
その結果、プログラミングに使えるライブラリとしてはあんまり提供されてなかったり。

標準ライブラリで書記素判定が提供されているものは少ないですし、
あっても「Unicode 8.0の仕様」、「家族絵文字や肌色には対応できない」とかだったりもします。

ということで、ないものは自分で作ったのがこちら。

- [GraphemeSplitter](https://github.com/ufcpp/GraphemeSplitter)

書記素判定の C# 実装です。
一応、Unicode 10.0 相当。

Web とかゲームとかでこの手の文字数判定をやりたくなることもあるでしょうし。
.NET Framework 3.5 にも対応してあるので、もしなんなら [Unity](https://unity3d.com/) でもお使いいただけると思います。

### おまけ

結構いやいやではあるんですけど。
どのくらい嫌かは、この2万行近い switch ステートメントを見てもらえれば伝わるかと思います…

- [Character.GetGraphemeBreakPropertyV10.cs](https://github.com/ufcpp/GraphemeSplitter/blob/master/GraphemeSplitter/Character.GetGraphemeBreakPropertyV10.cs)

要するに、どの文字がどういうカテゴリーなのか、1文字1文字テーブルを引く必要があって、
このテーブルがとにかく巨大です。
しかも Unicode のバージョン依存。

ちなみに、この2万行近い switch ステートメントは、[unicode.org が提供しているテーブル データ](http://www.unicode.org/Public/10.0.0/ucd/auxiliary/GraphemeBreakProperty.txt)からコード生成で作っています。
C# コンパイラーが結構いい感じに最適化してくれるんで割かし高速なコードが生成されるんですけども、
その代わり、この1ファイルがあるだけでコンパイル時間がものすごい伸びます…
