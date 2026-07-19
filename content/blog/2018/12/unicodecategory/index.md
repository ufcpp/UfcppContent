---
title: "書記素分割/Unicode カテゴリー判定"
source_url: "https://ufcpp.net/blog/2018/12/unicodecategory/"
content_type: "BlogEntry"
published_at: "2018-12-24T20:39:22"
updated_at: "2018-12-24T20:49:07"
tags: []
umbraco_id: 2207
parent_id: 2177
sort_order: 24
aliases: []
---

# 書記素分割/Unicode カテゴリー判定

なんか、昔作った[GraphemeSplitter](https://github.com/ufcpp/GraphemeSplitter)が[C++方面のUnicodeがらみのブログ](https://qiita.com/yumetodo/items/54e1a8230dbf513ea85b)から参照されてたので、ちょっと補足。

## UNICODE TEXT SEGMENTATION

「書記素って何？」って話は詳しくは[昔書いた記事](https://www.buildinsider.net/language/csharpunicode/01)でも見てもらうとして。
とりあえず、「人間が見て1文字と思うようなもの」を指して書記素(grapheme)といいます。複数の Unicode コードポイントが結合しまくるので、可変長。

いつも例に出すのが家族絵文字([👩🏻‍👦🏼👨🏽‍👦🏾‍👦🏿👩🏼‍👨🏽‍👦🏼‍👧🏽👩🏻‍👩🏿‍👧🏼‍👧🏾](http://ufcppfree.azurewebsites.net/Grapheme?s=%F0%9F%91%A9%F0%9F%8F%BB%E2%80%8D%F0%9F%91%A6%F0%9F%8F%BC%F0%9F%91%A8%F0%9F%8F%BD%E2%80%8D%F0%9F%91%A6%F0%9F%8F%BE%E2%80%8D%F0%9F%91%A6%F0%9F%8F%BF%F0%9F%91%A9%F0%9F%8F%BC%E2%80%8D%F0%9F%91%A8%F0%9F%8F%BD%E2%80%8D%F0%9F%91%A6%F0%9F%8F%BC%E2%80%8D%F0%9F%91%A7%F0%9F%8F%BD%F0%9F%91%A9%F0%9F%8F%BB%E2%80%8D%F0%9F%91%A9%F0%9F%8F%BF%E2%80%8D%F0%9F%91%A7%F0%9F%8F%BC%E2%80%8D%F0%9F%91%A7%F0%9F%8F%BE)とか)ですが、1書記素で11コードポイント、UTF-8で41バイトになったりします。

で、問題は、書記素の機械的な判定方法。
コンピューター上でもちゃんと書記素単位で処理してくれないと、人間の感覚からすると「backspace/delete を押すたびに文字が変わる」みたいな変な感じになります。

Unicode 標準としては、「あくまで参考。もっといいアルゴリズムにしてもらってもいいけど」という但し書き付きですが、以下のようなドキュメントがあります。

- [Unicode® Standard Annex #29 UNICODE TEXT SEGMENTATION](https://www.unicode.org/reports/tr29/)

書記素の区切り(grapheme cluster boundary)だけじゃなくて、単語区切り(word boundary)や文区切り(sentence boundary)についても言及。

基本的には、「このカテゴリーのコードポイントの後ろにこのカテゴリーが来たら繋げろ(あるは、そこで区切れ)」というルールが示されていて、そのルール自体は割と単純です。カテゴリーさえわかっていれば。
[自分が書いた実装](https://github.com/ufcpp/GraphemeSplitter/blob/master/GraphemeSplitter/StringSplitter.Grapheme.cs)でも、コメント・空行を除けば16行。

## コードポイントのカテゴリー

真の問題はカテゴリー判定。

Unicode では、コードポイント1つ1つにいろいろな属性が定義されています。
例えば、C# で[`GetUnicodeCategory`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.charunicodeinfo.getunicodecategory)で取れるやつは「[general category](http://www.unicode.org/reports/tr44/#General_Category_Values)」というやつで、
「[UnicodeData.txt](https://www.unicode.org/Public/11.0.0/ucd/UnicodeData.txt)」(`;`区切り)の3列目に定義があります。

UnicodeData.txt の中身を見れば何がきついかわかっていただけると思います。
こいつ、(Version 11 時点で)32292行もあります。
何らかの計算式があるとかではなく、愚直にテーブル。
そりゃまあ、それしかやりようがないのはわかりますが…

UnicodeData.txt 1個でもでかいのに、さらに追加で別の定義ファイルを参照せざるを得ない処理なんかもあったりします。
書記素区切りはその1つで、[GraphemeBreakProperty.txt](https://www.unicode.org/Public/11.0.0/ucd/auxiliary/GraphemeBreakProperty.txt)内のデータが必要になったり。

この問題は別に絵文字とか書記素分割だけのものでもなくて、
例えば `ToLower`/`ToUpper`の実装とかでも問題になります。

### テーブルの引き方(自分の実装)

`GetUnicodeCategory`で取れるカテゴリーだけで判別できるんだったら楽なんですけどね。
[自分が書いたコードが3千行近くなった](https://github.com/ufcpp/GraphemeSplitter/blob/master/GraphemeSplitter/Character.GetGraphemeBreakPropertyV10.cs)理由は、GraphemeBreakProperty.txt で定義されたカテゴリーが必要だったからです。

当たり前ですけど、こんなのコード生成で作ってます。

実行速度とか生成されるDLLサイズとかを比較するために数パターンのコード生成をやってみていて、全部 [switch case に展開したやつ(約2万行)](https://github.com/ufcpp/GraphemeSplitter/blob/master/GraphemeBreakPropertyCodeGeneratorTest/Benchmark.switch.cs)とかもあったりします(コンパイルするだけで1分くらいかかります。)。結局、まあ、二分探索でやるパターンを採用したのが上記の3千行近いコード。

### テーブルの引き方(.NET Core の実装)

.NET 標準の[`GetUnicodeCategory`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.charunicodeinfo.getunicodecategory) とか [`GetNumericValue`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.charunicodeinfo.getnumericvalue)とかも、
やっぱりテーブルを引く実装になっています。
テーブルのデータは以下のコード中にあり。

- [CharUnicodeInfoData.cs](https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/shared/System/Globalization/CharUnicodeInfoData.cs)

13万文字以上もあるものを愚直にテーブル化するわけにもいかないので、11:5:4ビットに区切った3段テーブルになっています。
(同じカテゴリーが連続していることが多いので、こういう分け方をするとデータ量が減る。
それでも23KBほどのサイズ。)

もちろんこいつもコード生成。
UnicodeData.txt からこのテーブルを生成するコードも coreclr 内にあります。

- [GenUnicodeProp](https://github.com/dotnet/coreclr/tree/master/src/System.Private.CoreLib/Tools/GenUnicodeProp)

## バージョン

テーブル実装のなお悪いところは、バージョンが変わるとテーブル自体を作り直すしかないところでして。

例えば先ほどの CharUnicodeInfoData.cs ですが、Unicode 11 にアップデートした時のプルリクエストがこちら:

- [Get the real update for Unicode 11 data #20589](https://github.com/dotnet/coreclr/pull/20589)

まあ、「Files changed」で差分を見てみてください。結構な分量。

しかも、Unicode、ほとんどの場合は「追加」なんですが、
たまーに破壊的変更もやるんですよね。
Unicode 標準に追従すると、そのフレームワークも破壊的変更を起こすことが。

[Java](http://d.hatena.ne.jp/masanobuimai/20140623/1403530870)も[C#](../../../../study/csharp/start/misc_unicode.md#katakana-middle-dot)やられてますが、Unicode のカテゴリー変更のあおりを受けています。

そうなると、指定したバージョンの Unicode 文字カテゴリーを取れる API も欲しいところなんですが…
1バージョン辺り23KBとかのサイズになるわけで、それを10以上あるバージョンすべてで持つのも結構な負担です。

## char と CharUnicodeInfo

ちなみに、[`char.GetUnicodeCategory`](https://docs.microsoft.com/ja-jp/dotnet/api/system.char.getunicodecategory)と[`CharUnicodeInfo.GetUnicodeCategory`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.charunicodeinfo.getunicodecategory) で結果が違うという邪悪なおまけつき。

どうも、昔からある `char` の方の実装は Unicode 4.0 がベース、
`CharUnicodeInfo` は Unicode 5.0 がベース(最近 11.0 ベースに更新)だそうです。

`char` の方を「破壊的変更になるし変えれない」とか中途半端にやった結果こうなったとか。
しかも、完全に Unicode 4.0 のままなんじゃなくて、
[Latin-1 の文字だけ 4.0 の時のカテゴリーのままで、残りは更新されていそうという](https://gist.github.com/ufcpp/1573a1a453bce1827b6b5025f79ed18a)。

## International Components for Unicode

そんな感じで、Unicode のカテゴリー判定は結構つらい作業です。
なので、OS に [ICU](http://site.icu-project.org/home) が入ってることを期待して、それを参照するのがいいのかも…
(自前で ICU のバイナリを同梱しようとすると20MBを超えます。)

[書記素、単語、文、行の区切り](http://userguide.icu-project.org/boundaryanalysis)の実装もあります。

ちなみに、[Windows 10 には標準で ICU が組み込まれてる](https://docs.microsoft.com/en-us/windows/desktop/intl/international-components-for-unicode--icu-)そうです。
