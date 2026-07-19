---
title: "ピックアップRoslyn 3/18"
source_url: "https://ufcpp.net/blog/2016/3/pickuproslyn0318/"
content_type: "BlogEntry"
published_at: "2016-03-18T10:43:23"
updated_at: "2016-03-18T10:43:23"
tags: []
umbraco_id: 1880
parent_id: 1877
sort_order: 2
aliases: []
---

# ピックアップRoslyn 3/18

## C# 7に取り込む範囲、ある程度決まったみたい

作業リストが更新されてた。

- [C# 7 Work List of Features #2136](https://github.com/dotnet/roslyn/issues/2136)

前はあって今消えてるものは単に「7からは外す」という意味。
まだ最終決定でもないんで、ここからまた増減はあるはず。

個人的な印象としては、現状でもそこそこ動いているものだけ残してる感じ。
たぶん、「年に1回リリース」、つまり、今年中のC# 7リリースがかなり現実味ありそう。

ということで、まだちょっと検討が要りそうなものはリストから抜けました。
待ち遠しいけども先送られたのは「非同期シーケンス」と「非null参照型」あたりですかね。

- async sequences ([#261](https://github.com/dotnet/roslyn/issues/261), [#5383](https://github.com/dotnet/roslyn/issues/5383))
- non-null reference type ([#227](https://github.com/dotnet/roslyn/issues/227), [#7445](https://github.com/dotnet/roslyn/issues/7445))

## レコード型の仕様書

レコード型の仕様書がやっとできてた。future ブランチにはマージ済み。

- [Records for C#](https://github.com/dotnet/roslyn/blob/future/docs/features/records.md)

## expression-bodied な set/get、コンストラクター

set, get 個別に、`=>` を使った短縮形のプロパティ定義を書けるようにしてほしいとか、そういう要望が前々から上がっていたわけですが。

- [Expression body syntax for accessors, ctor and finalize #8594](https://github.com/dotnet/roslyn/pull/8594)
- [Proposal: Expression bodied get and set methods #7881](https://github.com/dotnet/roslyn/issues/7881)

C# 6の時にも検討されたものの、「要望があるのはわかるけど、そこまで需要高くないし後回し」扱いされていたものです。

それに対して、C# チームの外から pull request が来て、C# チーム的にも「それをベースに実装するか」という流れになっている模様。

## サロゲートペア識別子対応

日本人的に、ほしければ「Add your reaction」で +1 だけでもつけといた方がよさそうなものが。

- [Compiler does not correctly interpret surrogate pairs when used in an identifier #9731](https://github.com/dotnet/roslyn/issues/9731)

サロゲートペアになってる文字でも、文字カテゴリーが letter になってるんだったら識別子に使えるべきじゃないかという話。

要するに「𩸽」(ほっけ。U+29E3D)とかの話。

C# 的には、letter系のカテゴリーに含まれている文字を識別子に使えるという仕様になっています。
ただ、.NETが内部的に16ビット(UTF16)で文字列を持ってるせいで、サロゲートペアになってる文字はカテゴリーを正しく判定してもらえない。

要するに、以下のようなコードを実行すると、結果はSurrogateになります。

<pre class="source" title="">
<code><reserved></span><span class="reserved">char</span>.GetUnicodeCategory(<span class="string">"𩸽"</span>[0])
</code></pre>

とはいえ、以下のように、`char`じゃなくて`string`を受け付けて、サロゲートペアな文字でも使えるオーバーロードに書き直せばちゃんと判定できます。
𩸽の場合はOtherLetter。

<pre class="source" title="">
<code><reserved></span><span class="reserved">char</span>.GetUnicodeCategory(<span class="string">"𩸽"</span>, 0)
</code></pre>

ってことで、

- C# 実装なRoslyn的に、実は判定自体はできる
- でも、2文字見ないといけなくなってそれなりにコンパイラーに負担が掛かる
- それで使えるようになる文字というと、日本語的には𩸽みたいなレアな文字
  - このissueページで言われてるのは「シュメール語の文字を使いたい」みたいな要望

という感じ。

それでも、この手の文字を識別子に使いたいですか？使いたいなら、reactionしておきましょう。
