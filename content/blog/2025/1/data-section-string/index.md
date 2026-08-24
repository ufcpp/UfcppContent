---
title: "文字列リテラルを data セクションに UTF-8 で書き込む案"
source_url: "https://ufcpp.net/blog/2025/1/data-section-string/"
content_type: "BlogEntry"
published_at: "2025-01-12T00:16:03"
updated_at: "2025-01-12T12:10:57"
tags: []
umbraco_id: 2511
parent_id: 2506
sort_order: 4
aliases: []
---

# 文字列リテラルを data セクションに UTF-8 で書き込む案

ここ数回のブログ(
[その1](../field-keyword/index.md)、
[その2](../first-class-span/index.md)、
[その3](../nameof-unbound-generic-types/index.md)
)、[Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) に最近かかった更新のうち、すでに実装されたものの紹介をしていたわけですが。

「その [Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) を見てると、何やら見慣れないものもちらほら増えてない？」みたいな話題も出ておりまして、今回からその辺りに触れていきたいと思います。

今日は「String literals in data section as UTF8」というやつを。

## 概要

普通にやると、C# の文字列リテラルや定数はアセンブリ(exe や dll)中の UserString セクションというところに UTF-16 で記録されます。
今回取り上げる話は、オプション追加で、特定の条件を満たす文字列は data セクションというところに UTF-8 で記録できるようにしたいという話です。

おおむね、例えば以下のコードを、

```csharp {title="なんか長い文字列リテラルを持ってるコード例"}
string s = "Some very very long looo...ooong string!!!!!";
```

特定条件下では以下のようなコード扱いでコンパイルするというもの。

```csharp {title="なんか u8 リテラルみたいな状態で記録して、デコードして使いたい"}
string s = System.Text.Encoding.UTF8.GetString(
    "Some very very long looo...ooong string!!!!!"u8
    );
// ※ 実際にはこの GetString 結果を静的にキャッシュ
```

ちなみにこれから説明していきますが、一番の目的は「data セクションに書くこと」で、
UTF-8 化(その結果、dll サイズが大体縮む)は副産物だそうです。

## 24ビット制限

PE ファイル(.NET の exe とか dll のファイル形式)中のメタデータの仕様上の問題なんですが、
C# の文字列リテラルって合計サイズで24ビット長(約 1,600万バイト、800万文字(以下 8M と表記))を超えれないそうです。

どうも理屈としては、

* 文字列リテラルは UserString セクションというところに書き込む
* UserString セクション中の文字列を表すハンドル値は4バイト
* そのうち1バイトは「テーブル番号」
* 残り3バイト(24ビット)が「文字列の先頭までのオフセット(バイト数)」
* 文字列の合計長が 16M バイト(8M 文字)を超えると、その次の文字列のオフセットが24ビットを超えてオーバーフロー

とのこと。

なので、これを超えると C# コンパイラーも [CS8103](https://github.com/dotnet/roslyn/blob/b7e891b8a884be1519a709edc7121140c5a1fac2/src/Compilers/CSharp/Portable/Errors/ErrorCode.cs#L1338) エラーを出すんですって。

しかも「8M を超えたその次の文字列リテラルがあるとダメ」というのが曲者で、
「一番最後のファイルの、一番後ろのメソッド中の、一番最後の文字列リテラルがでかい」という状況だと合計サイズ 8M 超でもコンパイルできちゃうそうです(最後の1個だけはセーフ)。
その状況で、「それより後ろに1個、短かろうが別の文字列リテラルが増えた」となると突然にアウト。

## 制限超過

まあ、8M 文字よ？
原稿用紙2万枚(日本人にしか通じない単位)。
太宰治の「人間失格」が8万文字くらいっぽいので、それ100冊分。
皆様方、「1プロジェクトでそんなの超えるやついるの？」と疑うかと思われます。

そう疑う方はぜひ、「CS8103」で Web 検索してみてください。
案外たくさん踏んでるやついる…

特によく出てくる話だと、[razor ファイル](https://learn.microsoft.com/ja-jp/aspnet/core/mvc/views/razor?view=aspnetcore-9.0)がビルド時に文字列化されて、UserString セクションに書き込まれるせいっぽいです。
でかい ASP.NET プロジェクトだとそこそこ 8M を超えてしまうとか。
(まあこれに関しては ASP.NET 側が「[.razor, .cshtml が生成する文字列、u8 リテラルに変えたら？](https://github.com/dotnet/aspnetcore/issues/50788)」とか言われたりもしますが。)

他にも、「大量の SQL を文字列で埋め込んでる」とか「元々あったでかい CSV ファイルの中身を文字列リテラルで埋め込んだら起きた」とか、
中々に「ほー、それで 8M 行っちゃったかー」と関心()するようなコメントも多数。

## data セクションと UTF-8 リテラル

ところで、C# 11 のときに [UTF-8 リテラル](../../../../study/csharp/cheatsheet/ap_ver11.md#utf8-literal)というものを導入したわけですが、こいつは単なる定数バイト列にコンパイルされます。
で、定数バイト列は data セクションってところに記録されるそうです。

そして、data セクション(というか、UserString セクション以外のもの大体)は[制限サイズが29ビット](https://github.com/dotnet/roslyn/issues/9852)だそうで。
UserString よりも5ビット多く、32倍！ 256M！

しかも、問題を起こしがちなのが .razor とかということは、
多くの部分がマークアップとかスクリプトですからね。
大体 ASCII 文字。
ASCII 文字は、UTF-8 だと1バイト、UTF-16 だと2バイト。
つまり、UTF-8 にするとそれだけで半分のサイズ。
さらに倍(合わせて64倍)の文字数を記録できるはず！512M！

64倍程度の差でどのくらい「突然 CS8103 が出た！」という「バグ報告」(自称)が減るでしょうか…
ムーアの法則的に言うと10年くらいの延命にはなりますかね。

まあ、 .NET ができた当初、
こんなさらっと 8M 文字超えてくるような使われ方も、
バイナリデータ用のセクションに UTF-8 を書き込んで使うやり方も全く想像してなかったでしょうね…

## コンパイラー オプションでやってみる

という話の流れで最初の話題になります。
「1回、試しに C# コンパイラー側でやってみる？」と。
長い文字列を見たら u8 リテラル化して、読み込み時に `UTF8.GetString` を挟む実装。

* 提案文書: [Add spec for the data section string literals feature #76139](https://github.com/dotnet/roslyn/pull/76139)
* 実験的実装: [Emit opted-in string literals into data section as UTF8 #76036](https://github.com/dotnet/roslyn/pull/76036)

これ、あくまで「C# コンパイラー実装」の話であって、
C# 言語の文法上は何一つ影響がないので Roslyn 側にしか作業はありません。
([csharplang](https://github.com/dotnet/csharplang/) 上に関連項目なし。)

そして、まあ、問題起こすのの大部分が ASP.NET だし、
先ほどちょっと触れていますが [ASP.NET 側がコード生成方針変えろという話](https://github.com/dotnet/aspnetcore/issues/50788)もあるので、
本当に C# コンパイラー側でやる価値があるのかどうかがちょっと自信なさげな感じではあります。

なのでとりあえず experimental。
「いったん merge されたものが世に出荷された上で、やっぱり取りやめになって消える」みたいなことがあり得る状態なのでご注意ください。

軽く現状の仕様を紹介しておくと以下のような感じ。

* UTF8 リテラルでやってることを流用するので実装はそんなに難しくはない
* feature フラグ (`/feature` オプションに渡すフラグ名)として `experimental-data-section-string-literals` を用意
  * `experimental-data-section-string-literals=20` みたいに最後に「閾値の文字数」を渡す
  * この文字数を超えた文字列リテラルだけが対象 (`=0` と書けばすべての文字列リテラルが対象)
* パフォーマンスはそこまで悪くならない予想(多少は不利)
  * 静的にキャッシュするので `GetString` は初回のみ
    * しかも最近、`UTF8.GetString` のパフォーマンスは JIT 最適化かかってる
  * `ldstr` (定数文字列のロード)と `ldsfld` (キャッシュした静的フィールドのロード)の命令自体はそんなにパフォーマンス差がない
  * ただ、定数文字列は文字列専用のヒープに書き込まれたり、JIT 時最適化はかかりやすくて、そこは `GetString` を挟むのが不利
  * 静的キャッシュを持つための匿名型が作られるので、型ロードのコストはかかる

## まとめ

というか感想。
「そんなやついるんだ」からの、「そんな対処するんだ」。

まあ「副作用として UTF-8 化すると文字列データのサイズがほぼ半分」の方が結構ありがたい可能性があります。
dll のバイナリサイズが問題になることもちらほらありますしね。
UTF-8 だと日本語なんかは逆にサイズが増えるんですが、
案外、プログラム中に埋め込んでる文字列リテラルは ASCII なもので
(razor 同様マークアップだったりとか、URL だったりとか、ハッシュ値の base64 だったりとか)。
