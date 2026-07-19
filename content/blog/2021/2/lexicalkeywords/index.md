---
title: "ピックアップRoslyn: 文脈キーワードの複雑さ低減"
source_url: "https://ufcpp.net/blog/2021/2/lexicalkeywords/"
content_type: "BlogEntry"
published_at: "2021-02-28T19:51:58"
updated_at: "2021-02-28T20:14:19"
tags: []
umbraco_id: 2335
parent_id: 2327
sort_order: 3
aliases: []
---

# ピックアップRoslyn: 文脈キーワードの複雑さ低減

ここ数回やってる「C# Language Design Meeting 議事録が1か月分くらいたまったので1個1個機能紹介」シリーズに見せかけて、もうちょっと別枠。
Design Meeting の場に上がっていなくて、まだ単体の提案ドキュメントが出ただけの状態のものです。

- [[Proposal]: Only Allow Lexical Keywords in the Language #4460](https://github.com/dotnet/csharplang/issues/4460)

[文脈キーワード](../../../../study/csharp/misc/ap_compatibility.md#contextual-keyword)の判定方法をもうちょっと単純にしたいという話になります。

## 文脈キーワード

C# は極力互換性を保つ(破壊的変更になるものを避ける)ように進化を続けてきました。
なので、「`var` というキーワードを使った新機能を追加したいけど、元々 `var` という名前の型がいた場合は型名を優先して、キーワード扱いしない」みたいなことを毎度やっています。
こういう文脈次第でキーワード扱いになるものを指して[文脈キーワード](../../../../study/csharp/misc/ap_compatibility.md#contextual-keyword) (contextual keyword) と言います。

ということで、以下のようなコードを書いた場合、C# 9.0 現在、1単語目の `var` はクラスの `var` になります。

<pre class="source" title="1単語目の var はクラスの var">
<code><span class="type">var</span> <span class="variable">var</span> = <span class="reserved">new</span> <span class="type">var</span>();
<span class="reserved">class</span> <span class="type">var</span> { }
</code></pre>

一方で、以下のコードの場合、1単語目はキーワードの `var` です。
(先ほどのコードとの差はクラス名が大文字始まりの `Var` な点だけ。)

<pre class="source" title="1単語目はキーワードの var">
<code><span class="reserved">var</span> <span class="variable">var</span> = <span class="reserved">new</span> <span class="type">Var</span>();
<span class="reserved">class</span> <span class="type">Var</span> { }
</code></pre>

## 文脈キーワードのわかりにくさ

先ほどの `var var = new var();` は割かし悪名高くて、
「互換性を保つため」という理由がなければどう考えても悪い文法です。
IDE 上は型名とキーワードで色が違うのでギリギリ判別は付きますが、
色なしのテキストになったら読めないと思います。

このブログは Visual Studio で書いた結果から型名かキーワードかの「色」を取って書いているので大丈夫なんですが、
そこまでマメなブログがどれだけあるか…
例えば単に Markdown 記法で以下のようなコードを書いたとき、
大体の環境で、`var` は無条件にキーワード扱いされると思います。

<pre class="source" title="1単語目はキーワードの var">
<code>```cs
var var = new var();
```
</code></pre>

「文脈を見る」みたいなこと自体学習ハードルを上げてしまうものですし、
挙句、インターネットで調べものをしていて変な色付けで表示されるとなると、
かなり初心者に優しくない状態になっています。

## record

ところで、C# 9.0 で導入された[レコード型](../../../2020/6/record0609/index.md)についてはちょっと条件が違ったりします。
以下のコードはコンパイル エラー。

<pre class="source" title="record は型名よりもキーワードの優先度が高い">
<code><span class="reserved">record</span> <span class="type"><span class="warning">record</span></span> <span class="error">= <span class="reserved">new</span> <span class="type">record</span>();</span>
<span class="reserved">record</span> <span class="error"><span class="type">record</span></span> { }
</code></pre>

1単語目の `record` がキーワード扱いされていて、結果的に、以下の警告・エラーが出ます。

- 警告: 型名に `record` は避けてほしい
- エラー: レコード型の宣言に `=` は書けない
- エラー: レコード型の宣言が2つある

ここで、2行目を `class` に書き換えて、あと、C# 8.0 の文法として成立するように1行目をクラスで覆ってみます。
C# 8.0 ではこれは有効な C# コードでした。

<pre class="source" title="C# 8.0 で普通にコンパイルできるコード">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="type">record</span> record = <span class="reserved">new</span> <span class="type">record</span>();
}
 
<span class="reserved">class</span> <span class="type">record</span> { }
</code></pre>

これを C# 9.0 でコンパイルすると以下のようになります。
色付けが変わって、コンパイル エラーが出ます。

<pre class="source" title="さっきと同じコード。C# 9.0 では無効">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">record</span> <span class="type"><span class="warning">record</span></span> <span class="error">=</span> <span class="reserved">new</span> <span class="type"><span class="error">record</span></span>();
}
 
<span class="reserved">class</span> <span class="type"><span class="warning">record</span></span> { }
</code></pre>

ということで、実は `record` キーワードの追加は破壊的変更を起こしています。

ちなみに、文脈キーワードじゃなくなっているわけではないです。
例えば以下のコードはコンパイル可能。
1単語目だけ `@` を付けて「キーワードではない」ことを明示しています。
`new` の後ろなら`@` なしでも型名。

<pre class="source" title="@ を付ければキーワードじゃなくなる">
<code><span class="type">@record</span> <span class="variable">record</span> = <span class="reserved">new</span> <span class="type">record</span>();
<span class="reserved">class</span> <span class="type">record</span> { }
</code></pre>

あと、以下の書き方でも大丈夫です。
`global::` (`global` はキーワード)の後ろなので型名であることが明白。

<pre class="source" title="global:: の後ろは常に型名">
<code><span class="reserved">global</span>::<span class="type">record</span> <span class="variable">record</span> = <span class="reserved">new</span> <span class="type">record</span>();
<span class="reserved">class</span> <span class="type">record</span> { }
</code></pre>

`record record = new record();` を書けるようにしようと思ったら判定がかなり複雑になるのであきらめたみたいです。

そしてもう1点、互換性に対するポリシーもだいぶ今と昔で違っています。

## 互換性に関するポリシー

まあ、現実的に、C# で `var` とか `record` とかを型名として使うことはありません。
C# では型名は大文字始まりにする文化なので、同じ単語を使うにしても `Var` とか `Record`とかを使います。

それでも、昔だったら「世の中にはどんなコードがあるかわからないので、`var` 型・`record` 型が存在する前提で考える」となったと思います。
一方で、今は、「GitHub でコードを探してみたけど9割9分使ってないよ」みたいなのが判断材料になるようです。
実際まあ、この文章みたいに「変なことができる」という指摘をするためのコード以外ではほぼ出てこないと思います。

あと、[言語バージョンとターゲット フレームワークのバージョン](../../../../study/csharp/cheatsheet/langversionoption.md)に関するポリシーも変わりました。
昔は、「古い .NET Framework がターゲットの時でも最新の C# を使えるようにサポートする」という方針でした。
一方で今は、「既定動作では .NET のバージョンによって、それと同世代の C# バージョンに固定される」、
「明示的に C# バージョンを変えることもできるけども、わかってる人だけにやってほしくて、『サポートする』とは言わない」という方針。
基本的には、「C# のバージョンを上げるときには色々と書き換えが発生する覚悟があるはず」、
「保守モードなコードさえ壊さなければまあ許す」という前提になります。

## 字句解析で文脈判定

話を戻しますが、`record` キーワードの話。
文脈を見てはいるんですよね。`record` という名前の型があっても大丈夫です。
ただ、`record` の場合は以下のような判定をしています。

- ステートメントとかメンバー宣言の先頭で出てきたらキーワード
- それ以外の、`::` とか `new` とか他の型名の後ろに出てきたら識別子(型名、変数名、メソッド名等々)

`var` と何が違うかというと、「型名として使われているかどうか」みたいな情報を必要としていない点。
字句解析の段階(lexical analysis: コンパイラーの仕事のかなり初期の段階)でキーワードかどうかの判定ができます。

(注:
原文が lexical analysis と書いているのでここでもそう書いているものの、
正しくは構文解析(syntax analysis)での判定な気もする。
「その名前の型がある」みたいな情報(semantics: 意味解析)は必要としないものの、
「`new` の後ろ」とか「`::` の後ろ」みたいな情報(syntax)は必要としてそう。
lexical というと普通は `new` とか `::` とかの「単語の切り出し」のこと。)

字句解析だけで判定できるということは、前述の「Markdown が誤判定する問題」も回避できます。
1ファイルだけとか、コードの一部分だけを切り出したものとかを読んでも「キーワードなのか型名なのか」の判別を間違えません。

## 既存の文脈キーワードも字句解析時の判定に

で、`record` キーワードの追加で破壊的変更をしたわけですが、それで困った人いる？

さらにいうと、今更 `var var = new var();` が書けなくなって困る人いる？

ということで、以下の文脈キーワードの判定方法を変えたい(`record` と同じく字句解析時に判定)という話が出ています。

- `var`: [型推論](../../../../study/csharp/start/misctyperesolution.md#source-type)
- `nameof`: [nameof 演算子](../../../../study/csharp/start/st_string.md#nameof-operator)
- `dynamic`: [dynamic 型](../../../../study/csharp/dynamic/sp4_dynamic.md)
- `_`: [値の破棄](../../../../study/csharp/datatype/declarationexpressions.md#discards)

これらも完全にキーワードになるわけじゃなく、`record` と同じく例えば `@var var = new var();` なら有効なコードになります。

多分、`var`、`nameof`、`dynamic` については異論は出ないと思います。
`var` と `dynamic` はクラス名と、
`nameof` はメソッド名と競合するんですが、クラス名もメソッド名も普通は大文字開始で書くので。
影響を受けるコードは実用的にはほとんど現れないし、
影響を受けるとしても「`@` を足す程度なら許容できる」という範囲に収まる思います。

迷うとすると `_` で、`_ => _.Name` みたいな[ラムダ式](../../../../study/csharp/functional/fun_localfunctions.md#anonymous-function)は「聞かないこともない」程度には存在しています。
なのでこいつだけは現状維持の可能性はあります。

## まとめ

もう `var var = new var();` 書けなくてもいいんじゃない？という話。

メリットとして、GitHub の Markdown などに「コードの一部分だけ記載」とかをやっても型名かキーワードかの誤判定しなくて済むようになります。
(あくまで実装依存ですが、現状の semantics 依存な文法よりはだいぶ実装者にやさしくなります。)

一方で、文脈キーワード自体がなくなるわけではなくて、`@record record = new record();` は普通に有効なコードです。
生誕20年のプログラミング言語の「負債の返却」作業としては悪くない落としどころかなと思います。

個人的にはやってほしい変更なので、このブログを読んだ方にぜひとも[提案 issue](https://github.com/dotnet/csharplang/issues/4460) への 👍 をお願いしたかったりします。
(C# チームはある程度 👍👎 の数を見ています。)
