---
title: "Visual Studio 2015でのC#/VBの文字コード"
source_url: "https://ufcpp.net/blog/2015/08/roslynencoding/"
content_type: "BlogEntry"
published_at: "2015-08-04T02:52:40"
updated_at: "2015-08-04T02:57:34"
tags: []
umbraco_id: 1776
parent_id: 1775
sort_order: 0
aliases: []
---

# Visual Studio 2015でのC#/VBの文字コード

VSサポートチームのブログ記事が上がったことによって少々話題になっていますが、文字コード問題。

* [Visual Studio 2015 で Shift JIS エンコーディングされたソース ファイルをビルドするとコンパイラ エラーが発生する場合がある](http://blogs.msdn.com/b/jpvsblog/archive/2015/08/03/vs2015-shift-jis-encoding.aspx)

## 症状

Shift JIS とか、Unicode 以前の各国文字コードで保存した C#/VB ファイルがコンパイルできない、できても文字列リテラルが狂うなどの問題が出ます。

日本に限らず、中国語圏とかでも話題に。

## 原因

[Roslyn](https://github.com/dotnet/roslyn)のせいではあります。

ぶっちゃけ、「by design (仕様です)」の類ではあったんですが。

クロスプラットフォーム対応のために`Encoding.Default`を使うのをやめたというのがこの問題の原因です。

## 経緯を追って

### CoreFx 対応

最初に`Encoding.Default`撤廃をやったのは以下のpull request。CoreFx対応。

* [More CoreFx porting work #1623](https://github.com/dotnet/roslyn/pull/1623)

日付的には3月のものです。つまるところ、CTP5とかRC版とかですでにこの現象起きてたはず。

### Windows-1252 特殊対応

で、欧米圏ではこの時点で騒がれていました。
ASCII文字圏だから大丈夫かと思いきや、結構、
[Windows-1252](https://ja.wikipedia.org/wiki/Windows-1252)で書かれたコードがあるそうで。
œ みたいな合成文字や、À みたいなアクセント付きの文字、¿ (スペイン語で疑問文の頭につける記号)とか。

ちなみに、どのレベルでコンパイルできないかというと、Š (`LF`+128の位置)が`LF`と誤認識されて `//` コメント中に Š を書くとその後ろがコメント扱いされなくなるレベル。

これに対して、結局、4月にRoslyn チームが折れて、Windows-1252に特殊対応を入れたのがこちら。

* [Use CodePage1252 Encoding if available, Latin1 if not. #2151](https://github.com/dotnet/roslyn/pull/2151)

当初はWindows-1252に対する`Encoding`を自前実装して解決しようとしたり。
結局最終的には、一度`Encoding.GetEncoding(codepage: 1252)`してみて、例外が出たら別のコードページに切り替えるという方式に。

### リリース後に改めて問題に

Visual Studio 2015が7/20にリリースされたわけですが。
見事に、7/21に以下の報告が。

* [.NET compiler produces incorrect string constants in MSIL when C# source files encoded with non-UTF-8 encoding #4022](https://github.com/dotnet/roslyn/issues/4022)

そして続くようにぞろぞろと。

* [VS2015 (MSBuild/14) compiler can't detect file ecoding correctly #4222](https://github.com/dotnet/roslyn/issues/4222)
* [Chinese string is compiled to garbage characters #4255](https://github.com/dotnet/roslyn/issues/4255)
* [Roslyn can't detect 932 encoding #4264](https://github.com/dotnet/roslyn/issues/4264)

まあなんというか…

* 中国語に関する問題報告はVS 2015リリースから結構後
* 日本語に至っては、Roslyn Issueページへの直接の報告なくて、おそらくは別の口から入ったフィードバックをもとに、Microsoftの中の人が報告

ということで、しょうがないから修正pull requestが上がったみたいです。

* [Use default OS encoding to decode non-Unicode/non-UTF8 text, if available, instead of unconditionally falling back to cp1252 or Latin1. #4303](https://github.com/dotnet/roslyn/pull/4303)

結局、shim (くさび、詰め木)ライブラリを挟んで、`Encoding.Default`が使える環境では`Encoding.Default`を使うように変更したみたいです。

## 非Unicode問題

ちなみに、僕はRCの頃に気づいてたというか、実際、[大昔(C# 1.0時代)に書いたコード](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/SoundLibrary)がコンパイルできなくなってましたが、
普通に文字コードを変更してスルーしてました。

まあ、非Unicodeファイルとかちょっと。

そりゃリリースされてから問題になりますわね。アーリーアダプターやるような人の感覚だとむしろ非Unicodeファイルが残ってることの方が大問題。「欧米の連中、Windows-1252に対する特殊対応とかやめろよ、マジで」くらいに思っておりました。

ちなみに、今のVisual StudioはC#/VBファイルを作った時点で[BOM](https://ja.wikipedia.org/wiki/%E3%83%90%E3%82%A4%E3%83%88%E3%82%AA%E3%83%BC%E3%83%80%E3%83%BC%E3%83%9E%E3%83%BC%E3%82%AF)が入ってて、UTF8で保存されるはずなんですよね。
C#/VBファイルが`Encoding.Default`(日本だと要するにShift JIS)になってたのって、Visual Studio 2002時代だけのはず。

この時代に書かれてて、その後保守されてなくて、それでもたまに使われるようなファイルがどれくらいあるんだという話。
とはいえ、自分のところにもそれ([C# 1.0時代に書いたコード](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/SoundLibrary)がサンプルに残ってて、たまにダウンロードしていく人がいる)があったわけですが…

あと、Visual Studio の外、例えば今僕は仕事で[Unity](http://japan.unity3d.com/)使ってるわけですが、Windows環境でUnity使ってると、Unity上で作られるC#ファイルは`Encoding.Default`でできちゃうんですよね。
大変うざく(なのでチーム的には「C#ファイルはVisual Studio側で作って」運用をしていたり)。
