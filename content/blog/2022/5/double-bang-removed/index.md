---
title: "引数 null チェックの !!、取りやめ"
source_url: "https://ufcpp.net/blog/2022/5/double-bang-removed/"
content_type: "BlogEntry"
published_at: "2022-05-04T22:37:57"
updated_at: "2022-05-04T22:37:57"
tags: []
umbraco_id: 2425
parent_id: 2424
sort_order: 0
aliases: []
---

# 引数 null チェックの !!、取りやめ

`!!` を使った引数の null チェック、なくなるって。

## 引数 null チェック

[2月にブログに書きましたが](../../2/parameter-null-check/index.md)、
Visual Studio 17.1 Preview 3の頃、C# 11 候補として「引数の null チェック」構文が入っていました。

```csharp
m(null); // ArgumentNull 例外が出る。

void m(string x!!) { }
```

今現在(VS 17.2 Preview 5)でもこの構文は生きているんですが、次(たぶん、17.2正式リリースでも17.3 Preview 1でも)でいったん取りやめになるそうです。

## 取りやめの経緯

C# チームとしては、今、Preview リリースをしてみて反応を見てその後どうするかを決めたりしているわけですが。
[LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#new-options) があるのはそのためです。

とはいえ、普通に考えて、Preview 機能まで追いかけている人がそんなに多いわけもなく、
正式リリースされるまでどんな機能が追加されているのか知らない人の方が多数派でしょうね。
ところが今回は大変目立つ Pull Request が1個ありまして。

* [1,232ファイル、+4,540行、-21,372行の Pull Request]

こういうので急に話題になると、大体燃えます。
以下のような質問 Discussion が立ったんですが、コメントが荒れる荒れる。

* [質問: `!!` はどう使われることを想定しているの？](https://github.com/dotnet/csharplang/discussions/5735)

というか、null チェックがらみはいつも荒れるんですが…

この荒れ具合を受けて、4月6日に再検討:

* [LDM-2022-04-06](https://github.com/dotnet/csharplang/blob/main/meetings/2022/LDM-2022-04-06.md)

この日の検討では、

* やるべきじゃない？
    * → やる価値はあると信じてる
* もっと完全な契約プログラミングにする？
    * → null チェック以外の価値は微小
* NRT で自動的に実行時 null チェックも入れる？
    *  → 破壊的変更(急に例外出るようになる)の度合いが大きすぎるし、無条件の null チェック挿入の実行時コストはちょっと許容しかねる
* 構文再考: `!!` 以外を考える？
    * → `!` はない。かといって他の案(T parameter not null みたいなの)は長ったらしかったりできつい

みたいな話に。
で、次の4月13日に`!!` 以外の構文について色々検討。

* [LDM-2022-04-13](https://github.com/dotnet/csharplang/blob/main/meetings/2022/LDM-2022-04-13.md#c-language-design-meeting-for-april-11th-2022)

とはいえ、しっくりこなかったみたいで「C# 11からは取り下げて、後日改めて検討する」ということに決定。

そして .NET Runtime 側、
`!!`構文をやめて、[`ArgumentNullException.ThrowIfNull`](https://docs.microsoft.com/ja-jp/dotnet/api/system.argumentnullexception.throwifnull)に書き換える Pull Request が改めて出ました。

* [Remove usage of !! from dotnet/runtime](https://github.com/dotnet/runtime/pull/68178)

## 荒れた理由

いくつか私見。

### 突然の登場

一般的な C# 開発者に取って唐突に出て来た機能ではあったと思います。

前述の「1,232ファイル Pull Request」を見ての通り、
.NET Runtime チームにとっては結構強く求められる機能で、
要するに「内需」です。
内需の場合、割かしあっという間に C# チームに需要が伝わって、あっという間に実装されてリリースにこぎつけたりするので。

「外部開発者が知らないとこで“中の人”で勝手に話進めやがって」感があって、「これだからお前のとこの会社は」みたいに思われたんでしょうね。

### マイノリティのための構文

null チェック機能は「.NET Runtime みたいに、大規模に使われているライブラリ作者にとっては有用なものの、ライブラリを使う側にとっては大して必要のない機能」です。

よっぽどかっちりライブラリを書くのでなければ、
「引数の null チェックして自分で `throw new ArgumentNullException()` する必要あるの？」
「自分でチェックしなくてもどうせすぐに `NullReferenceException` 出るし、大差ないじゃない？」
「どうせ `catch` もせず、デバッガーで例外が出たところに飛んで直すだけだし」
とか思いますし。

なので、「なんでそんな必要のない機能を先に実装するの？」みたいな反発を買っていそうな雰囲気があります。
(ライブラリは作る側よりも使う側の人の方が圧倒的多数なので、人口ベースで見るとマイノリティのための機能になります。)

### null 理想論

この機能は「不完全な [NRT](../../../../study/csharp/resource/nullablereferencetype.md) のしりぬぐい」感もあるんですよね。

NRT のフロー解析が完璧ならそもそもとして「null であってはいけないところに null が渡ること自体がない」はずで、
だったら「null チェック」も「`ArgumentNullException`」も本来起こりえないので不要なものなわけで。

じゃあどこから null が漏れてくるかというと、

* NRT が不完全で、構造体の [`default`](../../../../study/csharp/resource/rm_default.md) とかで簡単に「null チェックをすり抜けてくる null」がいる
* 他の言語との相互運用、NRT 導入前の古い C# のコードなど、解析しようがないところから来る null がいる

という辺り。
この辺りのしりぬぐいとして、「フロー解析に加えて、実行時チェックして `throw new ArgumentNullException()`」とか、ある種の冗長な処理をやっているわけです。

結果、NRT に対する理想を抱く人ほど、`!!` を気持ちが悪がってる感じがあります。

## 個人的な意見

大多数の人間にとってはどっちでもよくて、
.NET Runtime チームにとってはそれなりに必要なんだから、
多少みっともない構文でも入れちゃっていいと思うんですけどね。

とはいえ、構文がみっともない以外にも多少、はまりそうなポイントがあったりはします。
例えば、[`record`](../../../../study/csharp/datatype/record.md) が絡むとどうなの？とか。
以下のようなコードの挙動を見ると、構文の問題以上にもうちょっと見当が必要かもなー、とかは思います。

```csharp
// これは例外を出してもらえる。
var r1 = new R(null);

// でもこれは例外が出ない。
// init アクセッサーにも同種の null チェックを備えられるようにすべきではないか？
var r2 = new R("") { X = null };

record R(string X!!);
```
