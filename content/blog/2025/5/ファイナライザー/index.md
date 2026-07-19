---
title: "ファイナライザー"
source_url: "https://ufcpp.net/blog/2025/5/%E3%83%95%E3%82%A1%E3%82%A4%E3%83%8A%E3%83%A9%E3%82%A4%E3%82%B6%E3%83%BC/"
content_type: "BlogEntry"
published_at: "2025-05-17T17:19:14"
updated_at: "2025-05-17T17:30:04"
tags: []
umbraco_id: 2513
parent_id: 2512
sort_order: 0
aliases: []
---

# ファイナライザー

以下の `~Class1` のこと、（C# の言語機能名として)なんと呼びますか？

<pre class="source" title="~Class1">
<span class="reserved">class</span> <span class="type">Class1</span>
{
    <span class="reserved">public</span> <span class="type">Class1</span>() { }
    <span class="operator">~</span><span class="type">Class1</span>() { }
}
</pre>

すごく今更ながら、8年くらい前からこれの呼び名が変わってたらしいというのを最近気づいたという話になります。

ちなみに結果だけ言うと、旧称がデストラクター(destructor)、今はファイナライザー(finalizer)です。

## 他の言語とかの話

C# がかつてこいつのことをデストラクターと読んでいたのは C++ 由来です。
ただ…

* 文法は確かに同じで、C++ も `~Class1` な文法がある
  * これのことをデストラクターと呼ぶ
  * ただし、挙動は C# のものと違っていて、呼ばれるタイミングは「変数のスコープを抜けるとき」
* C# は文法こそ同じなものの、実際にはそれはファイナライザーだった
  * Java の `finalize` メソッド(ファイナライザーって呼ばれてる)と同じ挙動
    * GC 回収されるタイミングで呼ばれる
  * C# もコンパイル結果的には `object.Finalize` メソッドのオーバーロードとして実装
    * `GC.SuppressFinalize` って名前のメソッドもある
  * .NET の仕様書上も呼び名はファイナライザー

文法だけ同じな C++ 由来の名前(デストラクター)で、意味合いとしては違うもの(ファイナライザー)という地雷。

しかも、「.NET の仕様書はファイナライザーと呼んでいるものを、C# のレベルではデストラクターと呼んでいた」という2個目の地雷。

## Deconstruct との兼ね合い

さかのぼること2016年末、うちのブログでも書いてましたね、これの話。

* [小ネタ 「deconstruct」という単語](../../../2016/12/tipsdeconstruct/index.md)

ここにある通り、発端は C# 7 の頃(20217年3月頃正式リリース)に導入した[分解](../../../../study/csharp/cheatsheet/ap_ver7.md#deconstruction)構文でして。
「Deconstruct って英単語は微妙じゃない？」という話から派生して、
「誰だよ、`~Class1` のことをデストラクターって言ったの」という話題にまで及びまして。

このブログの最後で「デストラクターって呼び名は微妙なので変えることも視野に入ってるみたい」と書いていますが、
実際、その後変更されたみたいですね。
Microsoft Learn (前記のブログの時代は MSDN だったもの)の文章の大元になってる [dotnet/docs](https://github.com/dotnet/docs/tree/main/docs) リポジトリを見てみた感じ、
割と早い段階から finalizer に置き換わっていました。

## ということで直します

前述のブログの話があったので長いこと自分も口頭ではファイナライザーって呼んでたりはしたんですが。
Learn が案外ちゃんとドキュメントの修正を早期に入れていたことには気づいておらず。
気が付いたら8年くらい放置してましたね…

うちのサイトも長らく放置で各所にデストラクターと書いたままなんで、直します…
(すぐにやるとは言っていない。)
