---
title: "ピックアップRoslyn 1/3: Regex パーサー / C# 7.2 fixes"
source_url: "https://ufcpp.net/blog/2018/1/pickuproslyn0103/"
content_type: "BlogEntry"
published_at: "2018-01-03T18:52:36"
updated_at: "2018-01-03T18:59:52"
tags: []
umbraco_id: 2126
parent_id: 2125
sort_order: 0
aliases: []
---

# ピックアップRoslyn 1/3: Regex パーサー / C# 7.2 fixes

なんかこう、GitHub リポジトリのwatchとかしてると、
だいたいアメリカのホリデーシーズンに合わせて自分まで「もう休み」な気分になってしまい。
要するに、12月20日前後にはもう休み気分で。
代わりに1月は2日から仕事に復帰してる感じの人が多く、休みあけた気分に。

## Regex パーサー

事の発端は、「[C# に Regex リテラルを入れてくれ](https://github.com/dotnet/roslyn/pull/23984)」とかいう、まあ、芽がない提案なんですが。
issue 自体はだいぶ前からあるものなんですが、ホリデー前後あたりからなぜか再燃。

「芽がない」ってのは、この手の「C# に別言語を埋め込みたい」系の提案が通った試しがないからでして。
今回も、まあ、C# チームの中の人の1人が、

- [Analyzer 作るのではダメな理由ある？](https://github.com/dotnet/csharplang/issues/371#issuecomment-290542407)
- [TypeScript で Regex リテラルのサポートに関わってたけど、あれ、割かし簡単じゃない](https://github.com/dotnet/csharplang/issues/371#issuecomment-350517894)
- [言語の変更なしで、IDE 機能として実現できる(それは大きな利益になると思う)](https://github.com/dotnet/csharplang/issues/371#issuecomment-350517894)

などなど言語機能としては完全否定。

代わりに、IDE機能としては割かし乗り気だったわけですけども、
「休み中の飛行機の中で暇だったから作った」的なノリで、
Regex パーサーを書いちゃったみたいです。

- [Add in a regex parser so that the IDE can provide services around regex editing. #23984](https://github.com/dotnet/roslyn/pull/23984)

持っている機能は以下のようなもの。

- VirtualCharService: `"\\1`、`@"\1"`、`"\\\u0031"` みたいな同じ文字列の別表現みたいなのの差を吸収する層
- `new Regex("")` の中身に対して働く正規表現パーサー
- 正規表現が間違っていたら編集時にエラーを検出する
- 正規表現の構文を見て色付け

まあ、現状、1開発者が休暇中に個人的に作ってみたって感じなので、今後どうなるかはわかりませんが。

あと、「正規表現が間違っていたらエラーに」の方は[Microsoft.CodeAnalysis](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp/)だからいいとして、
「構文色付け」の方は[Microsoft.VisualStudio.LanguageServices](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServices/)なんですよね…
Visual Studio Code とか for Mac で動くのか…

### raw string リテラル

ということで、結局、特別な言語機能なしで Analyzer を書く感じに収まりそうなんですが。
Regex だけの特別扱いというのは今後も芽はないとして。
とはいえ、汎用な機能として C# に追加したい課題はいくつかあったりはします。

1つは、["raw" string リテラル](https://github.com/dotnet/csharplang/issues/89)。
`@`付きのやつ([逐語的文字列リテラル](../../../../study/csharp/start/st_embeddedtype.md#verbatim-string))よりもさらに、「そのまんま」な文字列リテラルが欲しいという話。
文字列中に `\` が頻出することは Regex に限らず結構ありますし。
`@""` でも、文字列中の`"` がやっぱり`@"""abc"""`みたいな感じでかなり嫌ですし。

てことで、[C++ 11 の raw string](https://qiita.com/_meki/items/53db5976a041546eb8c4)みたいに、開始・終了のパターンを任意に変えれるようにしたいという話に(これも前々からある提案の再燃)。

### コンパイル時処理

.NET の `Regex` クラスには、正規表現を解析して、[内部的に IL コード生成してキャッシュ](http://smdn.jp/programming/netfx/regex/1_regex_cached_compiled/)しておくような機能もありまして。
動的コンパイル。
初回はやたらと遅いものの、何度も同じパターンを調べる場合は圧倒的に速くなります。

でもその、動的にやっちゃうがゆえに初回やたら遅いとか、
[AOT](https://ja.wikipedia.org/wiki/%E4%BA%8B%E5%89%8D%E3%82%B3%E3%83%B3%E3%83%91%E3%82%A4%E3%83%A9) を見越して動的コンパイルが使えない環境で困るとか、
結構悩ましいものがあります。

てことで、C# コンパイルの途中の処理をフックしてIL生成できるプラグイン機構を追加してほしいなんて話も度々出たりはするんですが。
Regex の中身のコード生成とかかなり複雑そうで、そういう機構があっても結構きつそう…

## C# 7.2 fixes

おまけ。

[Visual Studio 2017 15.5をリリース](../../../2017/12/vs15_5/index.md)して割とすぐにホリデーシーズンに入ってしまい、C# 関連も一気に動きが鈍くなったわけですが。
一応、15.6に関する計画的なドキュメントはちょこっと更新されていたり。

[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)に、
「C# 7.2 fixes」ってセクションが増えています。
おそらくこれが 15.6 でのリリースになります。
今出ている 15.6 のプレビュー版には入っていませんが、次のプレビュー辺りからちらほら入り始めるはず。

まあ、名前通り、基本的にはバグ修正です。
[先月書いたやべーやつ](../../../2017/12/バグ報告祭り/index.md)も直るはずです。
で、ちょこっと文法にも追加？が入ります。

- [Relax ordering constraints for parameter modifiers #23643](https://github.com/dotnet/roslyn/pull/23643)
- [Prefer by-val methods over in methods in overload resolution #23122](https://github.com/dotnet/roslyn/pull/23122)

前者は、[参照渡しの拡張メソッド](../../../../study/csharp/cheatsheet/ap_ver7_2.md#ref-extensions)の`ref`と`this`の順序とか、
[`ref`構造体](../../../../study/csharp/cheatsheet/ap_ver7_2.md#span-safety)の`ref`と`partial`の順序とか、
15.5 の時点では `ref this`、`ref partial` の順でないといけなかったのを、どういう語順でもよくするという話。

後者は、通常の引数と[`in`引数](../../../../study/csharp/cheatsheet/ap_ver7_2.md#ref-readonly)でオーバーロードを作ってしまった場合(例えば、`void M(T x)` と `void M(in T x)`)、`M(T x)` の方を呼び出す手段がなかったという問題の解消です。`M(value)` と呼べば `M(T x)` を、`M(in value)` と呼べば `M(in T x)` を呼ぶようになります。

いずれも、まあ既存コードは壊しませんし、今までできなかったことができるようになるだけなので、
「バグ修正みたいな物」として扱うみたいです。
要するに、
「`ref this` の語順でないとコンパイル エラーになる 15.5 の頃の挙動をあえて選びたい」みたいなことは 15.6 以降ではできなると思われます。
