---
title: "ピックアップRoslyn 2/28: Milestones 7.X"
source_url: "https://ufcpp.net/blog/2017/2/pickuproslyn0228/"
content_type: "BlogEntry"
published_at: "2017-02-28T01:35:47"
updated_at: "2017-02-28T01:35:47"
tags: []
umbraco_id: 2046
parent_id: 2036
sort_order: 5
aliases: []
---

# ピックアップRoslyn 2/28: Milestones 7.X

なんか、[csharplang にマイルストーンが切られてた](https://github.com/dotnet/csharplang/milestones)。

ちなみに、リリース時期を邪推されないように、期日は2070年とかのむちゃくちゃな日付になっています。
7.0が2070年、7.1が2071年、…みたいな。

## 7.0

- [7.0](https://github.com/dotnet/csharplang/milestone/4)

3/7のリリースを考えるととっくに実装終わってるはずのもの。

[Roslynリポジトリ](https://github.com/dotnet/roslyn)側からの移植と、あと、ドキュメントがまだないのでそれが残タスクっぽいです。(最近、ほんとにドキュメントが後…)

## 7.1

- [7.1](https://github.com/dotnet/csharplang/milestone/5)

C#的には初の「小数点リリース」計画なわけですが、これは、Visual Studio とか .NET ランタイムのリリース スケジュールとは合わせないって意味で使いたさそうな雰囲気。
ライブラリ、ランタイムやIDE支援が要らない機能を主に入れていく感じ。

7.1は、みるからに7.0の積み残し。時間的な都合で入れなかっただけで、いつでも実装可能なやつ。

[private protected](https://github.com/dotnet/csharplang/issues/37)なんて、7.0どころか6.0の積み残しだった李。これの話とか見事なもんで、「[機能的には大したことがなくていつでも実装可能。あとはキーワードを決めるだけ。キーワードが決まらなくて延期](http://www.buildinsider.net/column/iwanaga-nobuyuki/001)」って状態だったのに、この期に及んで「それでも私は別キーワードがいい」ってコメントを付けてる人がいたり。
「もう多数決では決まらないからC#チームがprivate protectedに決めた。このキーワードでやるか、さもなくばまったくやらないからの二択」と説明されてなお、あれやこれやコメントを付けてる人がいたり…

## 7.2

- [7.2](https://github.com/dotnet/csharplang/milestone/6)

なんか、パフォーマンス改善系の世代っぽい。
もしかしたら、この辺りで[System.Memory](../../../2016/12/tipscorefxlab/index.md)とか、[Utf8String](http://www.buildinsider.net/language/csharpunicode/02)とかのライブラリを実用段階に持っていきたいのかも。
それと関連しそうなものがちらほら並んでいます。

## 7.X

- [7.X](https://github.com/dotnet/csharplang/milestone/7)

その他、7.0の延長、ライブラリやIDEの機能追加が要らない感じのもの。

[フル機能版のパターン マッチング](https://github.com/dotnet/csharplang/issues/45)はここに入ったっぽいんですよねぇ。
確かにまあ、[型パターン](../../../../study/csharp/datatype/typeswitch.md)が入るだけでそこそこ満足度高いですし。
再帰パターンの類は実装大変な割には用途が狭く。

## 8.0

- [8.0](https://github.com/dotnet/csharplang/milestone/8)

メジャーリリースなので、でかい機能。時間も掛かりそうかなぁ…

[レコード型](../../../../study/csharp/misc/misclanguageevolution.md#record-type)とか、
[null許容参照型](http://www.buildinsider.net/column/iwanaga-nobuyuki/012)とか、
[非同期シーケンス](http://www.buildinsider.net/column/iwanaga-nobuyuki/010)とかはここに。

## X

- [X.X](https://github.com/dotnet/csharplang/milestone/9)
- [X.0](https://github.com/dotnet/csharplang/milestone/10)

ほんとに未定。
「何かいいアイディアがあれば」とか、
「.NET ランタイム自体に機能追加が必要で、そっち次第」とか、そんな感じのもの。
逆にいうと、ふといいアイディアが降りてきて急に実装されたり、
.NETチームとかVisual Studio IDEチームとかのスケジュール次第では早くも遅くもなりそうな。

[コード生成拡張](https://github.com/dotnet/csharplang/issues/107)(replace/original)とかはほんと、IDE側の対応次第って感じのものなんですよね…
これ、ほんと一刻も早くほしかったりするんですけど。
