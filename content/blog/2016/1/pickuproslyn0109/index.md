---
title: "ピックアップRoslyn 1/9: structural typing"
source_url: "https://ufcpp.net/blog/2016/1/pickuproslyn0109/"
content_type: "BlogEntry"
published_at: "2016-01-09T04:03:19"
updated_at: "2016-09-19T06:21:07"
tags: []
umbraco_id: 1848
parent_id: 1841
sort_order: 2
aliases: []
---

# ピックアップRoslyn 1/9: structural typing

C#にstructural typing(構造的型付け)を入れようという案。

- [[Proposal][Roslyn] Add structural typing support #7844](https://github.com/dotnet/roslyn/issues/7844)

まあ、「何人かでちょっとディスカッションしたよ」というくらいの段階みたい。やりたいことの説明や、構文の案(3案ほど)が出ている程度(実装方法に関する言及あまりなし)。

以下、structural typingがどういうものなのかについて簡単に説明。

Goのインターフェイスがそうなんですが、明示的な実装を必要とせず、「同じシグネチャ(名前+引数一致)さえ持っていればなんでもOK」という方針で代入可能な型システムをstructural typingと言います。

Javaとか.NETのインターフェイスやクラスの継承・実装、C++のクラス継承なんかは、以下のように、明示的な参照・実装が必要になります。こういうのはnominative(指名的、任命的)な型付けといいます。

![.NETのインターフェイス(nominative typing)][1]

一方で、Goのインターフェイスは、(C#で提案されている案の1つを使って類似機能を書くと)以下のように、シグネチャだけ見て暗黙的に「実装されている」ことにしてくれます(これが、structural)。

![structural typing][2]

  [1]: ../../../../../assets/media/1048/interface.png
  [2]: ../../../../../assets/media/1049/structural.png

もちろん良し悪しあって、以下のような感じ

1. nominativeな方が若干実行性能がいい
1. ただ、structuralの方はほんの小さなコストで、結構大きな自由度が手に入る
1. structuralの方は、実装クラス側が知らず知らずのうちに合わない構造に修正される可能性がある

3番目の「知らず知らずのうちに修正」が怖いので、Javaとか.NETとかみたいな動的リンクが基本のプログラミング言語ではstructuralのデメリットが大きいんですが。Goの場合は静的リンクだからインターフェイスをstructuralにできたというのもあるはず。

とはいえ、別にどっちか片方だけでないといけないということもないわけです。C#では、普通のインターフェイスはnominativeだけど、追加でstructuralな何かを加えて使い分けたってかまわないはず。「知らず知らずのうちに修正」問題をどう対処するかだけ決まればC#でもstructural typingの便利さを享受できるはずです。
