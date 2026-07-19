---
title: "ピックアップRoslyn 8/31: トリアージ祭り"
source_url: "https://ufcpp.net/blog/2019/8/pickuproslyn0831/"
content_type: "BlogEntry"
published_at: "2019-08-31T23:56:21"
updated_at: "2019-09-01T00:10:29"
tags: []
umbraco_id: 2263
parent_id: 2259
sort_order: 2
aliases: []
---

# ピックアップRoslyn 8/31: トリアージ祭り

.NET Core 3.0/C# 8.0 正式リリースがあるとされている [.NET Conf](https://www.dotnetconf.net/) (9月23日から)も近づいてきて、さすがに最近は C# 8.0 がらみの動きはないというか、そろそろその先を見た動きになってきた感じがします。

ということで、こういう時期恒例のトリアージ祭りが発生中。

(無言でマイルストーンを変えただけのやつも、GitHub の Sort 順序「Recently updated」で浮上するんですね。
「今週何やらトリアージしてそう」って感知してたのを、今調べてなおしてて大変助かってる。)

ちなみに、[Records](https://github.com/dotnet/csharplang/issues/39) は「これまでと変わらず」で 9.0 マイルストーンです。
一応、C# 9.0 はこれが主軸になるはず。6.0 時代から案だけはあったのが延々先延ばされてきましたが、ついに。

## 優先度微ダウン(8.X → 9.0)

- [Champion "Target-typed `new` expression" #100](https://github.com/dotnet/csharplang/issues/100)
- [Champion "Nullable-enhanced common type" #33](https://github.com/dotnet/csharplang/issues/33)
  - ターゲットからの型推論の導入で解決できそうな問題はまとめてやりたそう
  - see also: [#881](https://github.com/dotnet/csharplang/issues/881), [#2460](https://github.com/dotnet/csharplang/issues/2460), [#2473](https://github.com/dotnet/csharplang/issues/2473)
- [Champion "and, or, and not patterns" #1350](https://github.com/dotnet/csharplang/issues/1350)
  - セットでこれも: [Proposal: allow comparison operators in switch case labels C# #812](https://github.com/dotnet/csharplang/issues/812)
- [Champion "defer statement" #1398](https://github.com/dotnet/csharplang/issues/1398)
- [Champion: Switch expression as a statement expression #2632](https://github.com/dotnet/csharplang/issues/2632)
- [Champion "Permit attributes on local functions" #1888](https://github.com/dotnet/csharplang/issues/1888)

この辺りは優先度ダウンしているといっても、9.0 自体のリリースのタイミングの方自体に関心が。

とりあえずわかっていることとして、

- [build での発表](../../5/build2019/index.md)によれば、
  - 来年から .NET Core は毎年11月の定期メジャー リリースになる
  - (今年末の3.1は安定として5以降) Long Term Support (LTS)は偶数番リリースのみ
- C# のリリースは .NET と足並みをそろえたがってる

というのがありまして。
じゃあ、LTS が付いてない奇数番のとき、C# はどうするんだって話でして。
.NET 5 のタイミング、C# も 9.0 になって LTS なしなのか、このタイミングでは 8.X を出すのか…

自分の把握してる範囲ではどっちになるかわからず。どちらになるかによってだいぶ「マイルストーン 9.0」の意味が違いそう。

[8.X](https://github.com/dotnet/csharplang/milestone/12)に残っているものの少なさとか、9.0 に移動されたものがどれも小さ目な機能だとか、「C# 年次リリース」も十分あり得るような気もしつつ。でも、昔、「文法が年1メジャー アップデートするのはやりすぎ」みたいなこと言ってたし、「C# にも LTS ないバージョンがあるの？」的なこともちょっと思ったりはしつつ。

## 優先度アップ(X.0 → 9.0)

- [Champion "Covariant Return Types" #49](https://github.com/dotnet/csharplang/issues/49)
- [Champion "Discriminated Unions" #113](https://github.com/dotnet/csharplang/issues/113)
  - 「後から入れるにしても Records と併せて考えるべき」という口ぶりなので、マイルストーンが 9.0 になったからといって 9.0 に入るとは限らなさそうな感じ
- [Champion "Static Delegates" #302](https://github.com/dotnet/csharplang/issues/302)
- [Champion: Module Initializers #2608](https://github.com/dotnet/csharplang/issues/2608)

## 優先度アップ(未分類 → 9.0)

- [Efficient Params and String Formatting #2302](https://github.com/dotnet/csharplang/issues/2302)

これ、参照型に対する可変長スタック確保(`stackalloc T[n]`)が GC にやさしくないから厳しいって言われてた気がするものの…
以下の関連する作業が .NET 5 マイルストーンで動いてるので整合性は取れてるのかな…

- [API Proposal: Add Variant type to avoid boxing .NET intrinsic types #35806](https://github.com/dotnet/corefx/issues/35806)
- [API Proposal: Add a ValueStringBuilder #28379](https://github.com/dotnet/corefx/issues/28379)
- [Protoype for nonallocating string formatting #2595](https://github.com/dotnet/corefxlab/pull/2595)

## 優先度微ダウン(9.0 → 10.0)

- [Champion "Type Classes (aka Concepts, Structural Generic Constraints)" #110](https://github.com/dotnet/csharplang/issues/110)

## 優先度微アップ(X.0 → 10.0)

- [Proposal: Exponentiation operator #2585](https://github.com/dotnet/csharplang/issues/2585)
  - 「べき乗演算子」が欲しいってやつ。C# が今までこれを考慮に入れてこなかった理由は「`Pow`関数の実装によって結果変わるのが嫌」みたいな空気
  - [.NET Core 3.0 で float がらみが軒並み IEEE コンプライアンス準拠になる](https://github.com/dotnet/docs/blob/master/docs/core/whats-new/dotnet-core-3-0.md)ので、懸念が1つ解決してるので実装してもいい空気になったみたい

## 優先度ダウン(9.0 → X.0)

- [Champion: fixed-sized buffers #1314](https://github.com/dotnet/csharplang/issues/1314)
  - [ref 構造体](../../../../study/csharp/resource/refstruct.md)の寿命トラッキングがちょっと大変そうで厳しいみたい
- [Champion "Allow no-arg constructor and field initializers in struct declarations" #99](https://github.com/dotnet/csharplang/issues/99)
  - 6.0 の頃から候補になってたけど「既存のコードが `new T()` を `default(T)` の意味で使ってる・最適化かけてるから厳しい」って雰囲気のやつ
  - ちなみに、[Non-defaultable value types](https://github.com/dotnet/csharplang/issues/146)はこれとセットであれば考えてくれるらしい

## 優先度アップ？(未分類 → X.0)

- [Proposal: `tail return` recursive calls #2304](https://github.com/dotnet/csharplang/issues/2304)

## 優先度大ダウン(8.X → Any Time)

- [Switching ReadOnlySpan<char> with constant strings #1881](https://github.com/dotnet/csharplang/issues/1881)
- [Champion: "Permit surrogate pairs and wide Unicode-escaped code points in identifiers" #1742](https://github.com/dotnet/csharplang/issues/1742)

ちなみに後者の Unicode なんとかというやつ、
大昔から既知の問題なんですけども、今の C# コンパイラーはサロゲート ペアな文字を識別子として受け付けません。
サロゲート ペアでも、非常用漢字とかヒエログリフとか、 Unicode 文字カテゴリー的には使えていいはずの文字があるんですが、それが使えないのは「バグ」です。

で、まあ、「Any Time」マイルストーンっていうのは概ね「外部からのコントリビューション待ってます」という意味。
やるとしたら自分だろうなぁとか思っていたり。

昔、[corefx に「`GetUnicodeCategory(int codePoint)`を public にしてほしい」と要望を出した](../../../2018/1/getunicodecategory/index.md)のはこれのためでして。
このオーバーロードがないと C# にサロゲート ペア識別子を入れるのは厳しい(きれいで、かつ、パフォーマンスをあまり落とさない手段がない)。

こいつは .NET Core 2.1 で無事に入ったわけですが、C# コンパイラーは今のところターゲットが .NET Standard 2.0 なので、まだこのオーバーロードを使えません。

というかぶっちゃけ、自分が「サロゲート ペアな文字を識別子に使えなくて困ってる人見たことないよ。ジョークでヒエログリフ使いたいって言う人の方が多いくらい。昔勉強会で『欲しい人？』って聞いてみたことあるけど誰もいなかった。それでも使えるようにする pull request 出そうと思ったことがあるけど、`GetUnicodeCategory(int codePoint)` がないのがネックだった。」的な報告をしているので、「Any Time」になったの自体たぶん自分のせい。

## リジェクト

- [Proposal: Dictionary Literals #414](https://github.com/dotnet/csharplang/issues/414)
  - 口ぶりとしては「Dictionary だけを特別扱いして検討する気はないけど、一般的な『データの初期化』に関する関心はある。Records の後で改めて考える」な雰囲気
- [Proposal: allow sizeof in any context #1828](https://github.com/dotnet/csharplang/issues/1828)

## 新規(未分類)

- [Top level statements and member declarations (embrace scripting dialect) #2765](https://github.com/dotnet/csharplang/issues/2765)

今、「C# スクリプティング」が特別なモードとして存在していて、その中でだけトップ レベル(クラス、名前空間の外)にメンバー宣言を書ける状態です。

で、今までは「C# スクリプティング」の需要自体が低かったから特別モードの保守も平気だった(最悪、ちょっと通常の C# よりも遅れてもいい)ものの、
最近、[Try .NET](https://github.com/dotnet/try/)によって需要が急増していてどうしようかという感じ。

(ちなみに、なんか最近この Try .NET、[jupyter](https://jupyter.org/)対応してるんですよ、作業的に。)

なので、トップ レベル メンバーを、特別扱いではなく、通常の C# の仕様にも統合できないかという話になっています。
「`Main`メソッドはおまじない」からの脱却も、言語を学び始めたばかりの人にやさしいですし。

まあ、マイルストーン未分類ですが。
