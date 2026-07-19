---
title: "C# 7.0リリース(もう2週間くらい経過したけども)"
source_url: "https://ufcpp.net/blog/2017/3/csharp7rtm/"
content_type: "BlogEntry"
published_at: "2017-03-21T20:53:51"
updated_at: "2017-03-21T20:53:51"
tags:
  - "C# 7思い出話"
umbraco_id: 2049
parent_id: 2047
sort_order: 1
aliases: []
---

# C# 7.0リリース(もう2週間くらい経過したけども)

気が付けば、Visual Studio 2017がリリースされてから2週間くらい経ってしまっているわけですが…

マイクロソフト公式の[Release Celebration](https://connpass.com/event/50910/)でLT登壇したり、
自分主催の[リリース記念勉強会](https://csugjp.connpass.com/event/50930/)やったりとかで、
すっかり力尽きていました。

そんな感じでしたが、2点ほどそれの事後的な話。

- [リリース記念勉強会](https://csugjp.connpass.com/event/50930/)の動画上げました
- [C# によるプログラミング入門](../../../../study/csharp/index.md)、リリース版対応しました

## 動画

[Visual Studio 2017 リリース記念勉強会](https://csugjp.connpass.com/event/50930/)、
当日にストリーミング配信とかはやらなかったんですが、
一応動画に撮っていたりはしまして。

許可が取れたものは、YouTube にアップロードしていってたりします。
C#ユーザー会でチャネルを作ったので、こちらで公開中です。

- [Japan C# User Group](https://www.youtube.com/channel/UCWNxu4tBmihABhzJjPiHMVw)

## リリース版対応

[C# によるプログラミング入門](../../../../study/csharp/index.md)内から、C# 7.0がらみは「正式版ではこうなります」、「予定」、「仮」的な文面全部なくしたはず。
ついに正式リリースですよ。

ちなみに、RCからRTMの間で、C# 7.0の実装にも微妙に変更もありました。

1つ目。タプルに対する拡張メソッドの呼び出しで、オーバーロード解決時にメンバーごとの型変換を考慮してくれるようになりました。
[1月25日のブログ](../../1/pickuproslyn0125/index.md)の最後の方にちょっと書いたやつです。
「『今やる』か『今後もうできないか』の2択なので、もう時間も限られているけど頑張って今やってみる」っていう状態になっていた機能が、無事に入りました。

<div>
<script src="https://gist.github.com/ufcpp/168dc0519f4f58ffa6c5b146246d9f77.js"></script>
</div>

2つ目。`while` の条件式中で宣言した変数のスコープ変更(while内のみになった)。
[去年の12月](../../../2016/12/pickuproslyn1214/index.md)にちょっと書いてたやつ。
これ、直前のRC版でもまだ`while`の外にスコープ漏れてて、リリース後に確認してみたらちゃんと`while`内のみに変更掛かっていることを確認。
うちの記事にも反映させておきました。

<div>
<script src="https://gist.github.com/ufcpp/5984fee9bbbc0600c0314baa9a438ba3.js"></script>
</div>

そういや、最近、すっかりC#のバージョン番号の書式が「C# 7.0」(.0が付く)なんですよね。
一時期は、[C# 6, C# 7](../../../2016/6/versionnumber/index.md)だったんですけども。
しれっと、いろいろとやっぱり .0 が付くように変わっていたり。
うちのサイト内の表記、どうしようかな…
