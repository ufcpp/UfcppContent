---
title: ".NET のカルチャー依存 API 問題"
source_url: "https://ufcpp.net/blog/2021/8/invariantculture/"
content_type: "BlogEntry"
published_at: "2021-08-22T20:38:16"
updated_at: "2021-08-22T20:38:16"
tags: []
umbraco_id: 2359
parent_id: 2354
sort_order: 2
aliases: []
---

# .NET のカルチャー依存 API 問題

以下のコード、実行環境によって出力結果が変わります。

```csharp {title="DateTime の ToString がカルチャー依存"}
Console.WriteLine(new DateTime(2021, 8, 22));
```

日本語 Windows 環境だと `2021/08/22 0:00:00` と表示されると思いますが、
OS 設定でカルチャーを変更すると別の書式になります。
例えば、en-US カルチャーにすると `8/22/2021 12:00:00 AM` になります。
要するに、`DateTime.ToString` は OS のカルチャー依存になっています。

問題点はいくつかあるんですが…

* `ToString` みたいなよく使うメソッドの既定動作がカルチャー依存
* [WebAssembly](https://docs.microsoft.com/ja-jp/aspnet/core/blazor/host-and-deploy/webassembly?view=aspnetcore-5.0) みたいな、カルチャー情報を使いたくない環境がある
* カルチャー非依存にしたければ北米カルチャーを強要されがち
* 北米カルチャーが思った以上に世界から浮いてる

今日はこの辺りの話を書きたいと思います。

## <a id="why-now">なぜ今</a>

国際化対応をしたことがある人ならたぶん、 .NET Framework 1.0 リリース当初であったり、
さらに言うと C# のリリース前、Windows アプリは Visual Basic (無印)や C++ を使って書いていた頃からこの手の問題には悩まされていたと思います。

ところが最近(去年くらいから)、別に国際化対応をしなくても悩むことが出てきました。

### <a id="dotnet-icu">ICU 化</a>

元々、(Windows 向けの) .NET Framework では NLS (National Language Support)という Windows 組み込みの多言語対応データを使って国際化対応をしていました。

一方、マルチプラットフォームの .NET Core (.NET 5 以降は単に「.NET」と呼ぶようになったやつ)が出たことで、
Windows 以外では ICU (International Components for Unicode) という Unicode 標準に基づくライブラリを使うことになりました。
多くの Unix 系 OS では標準搭載、あるいは、割かし簡単な手間で ICU を組み込むことができるようになっています。
Windows 10 でも、2017年のアップデート以降、標準で ICU が入るようになりました。

.NET Core 3.1 までは、実は、Windows は NLS を、他の OS では ICU を使っていたことで、
実行環境によって微妙に実行結果が変わることがありました。
この挙動があまり望ましくないということで、
.NET 5 (2020年11月リリース)からは [Windows でも ICU を使うように変更されました](https://docs.microsoft.com/ja-jp/dotnet/core/extensions/globalization-icu)。

その結果、.NET Core 3.1 から .NET 5 にアップデートすると、微妙に文字列処理に変化があったりします。
(NLS に戻すオプションもあるので、まずそうならそのオプションを指定することになります。)

[軽く騒動もあった](https://github.com/dotnet/runtime/issues/43736)んですが、
そこで初めて、「えっ、このメソッドもカルチャー依存だったんだ…」みたいな事実に気づいたという方も多いんじゃないかともいます。

ちなみに日本語でも、「[IgnoreCase を付けると "つ" と "っ" が Equals 扱いになった](https://github.com/dotnet/runtime/issues/54987)」みたいなことが起きています。
(.NET 5 リリースのから今年7月まで気づかれてなかったっぽい。)

### <a id="no-icu-env">OS 搭載の ICU に頼れない環境</a>

ICU のデータはフルに持つと結構なでかさになります。
.NET アプリの実行に必要な分だけ抜き出しても 1.4MB くらいあります。

まあ 1.4MB くらいのサイズであれば、サーバー OS ではそれほどきついサイズではないので、「たいてい OS が持ってる」を前提にしても問題ありませんでした。
問題は iOS や WebAssembly 実行で、
これらの環境ではアプリごとに ICU データを同梱して配布する必要があります。
WebAssembly なんかはブラウザーでダウンロードして実行する前提なので、
1.4MB もバカにならないサイズになります。
(だいたいはブラウザー自身、iOS であれば WebKit が ICU 依存なので、データとしては ICU を持っているはずなんですけどね… そのデータをアプリ開発者が参照する手段はないです。)

ということで、.NET も [Blazor WebAssembly](https://docs.microsoft.com/ja-jp/aspnet/core/blazor/host-and-deploy/webassembly?view=aspnetcore-5.0) とかに注力している昨今、
「カルチャー依存をなくしたい」という要望も強くなってきました。

Blazor では、

* EFIGS (西欧向けのフランス語、イタリア語、ドイツ語、スペイン語だけを持つ)のデータだけを持つ
* CJK (データがでかくなりがちな日中韓)のデータだけ抜く
* 完全にカルチャーを抜く([`CurrentCulture`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.cultureinfo.currentculture) を取ろうとしても [`InvariantCulture`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.cultureinfo.invariantculture) が返ってくる)

みたいなモードが選べるようになっていたりします。
完全にカルチャーを抜くモードは Blazor に限らず、[`InvariantGlobalization` というオプション](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md)を指定することでどのタイプの .NET アプリでも同じモードで実行できます。

## <a id="culture-dependent-apis">謎のカルチャー依存</a>

ということで今になってカルチャー依存問題を踏んでいるわけですが。
ここでもう1個問題になるのが、「えっ、このメソッドもカルチャー依存だったの？」みたいな意外さ。

冒頭の例でも書きましたが、ただの `ToString` すらカルチャー依存です。
.NET が Windows 限定で、しかもアプリと言えば Windows GUI アプリだった時代にはこれでもよかったんですが…

当然ですが、Web アプリだとこの挙動は結構イラっとします。
クラウド インスタンスを新規で立ててカルチャー設定を忘れて `ToString` の結果が変わるみたいなやつ。

先ほど「[軽く騒動もあった](https://github.com/dotnet/runtime/issues/43736)」と紹介したやつなんて結構ひどくて、「[`IndexOf`](https://docs.microsoft.com/ja-jp/dotnet/api/system.string.indexof) と [`Contains`](https://docs.microsoft.com/ja-jp/dotnet/api/system.string.contains) で結果が違う」という状態です。
なんでこんなことになるかというと、`IndexOf` は `CurrentCulture` 依存で、`Contains` は [Ordinal](https://docs.microsoft.com/ja-jp/dotnet/api/system.stringcomparison) 比較だからです。
混ざってるのはさすがにつらい…

古くからある API ほどカルチャー依存で、新しめの API は `Ordinal` もしくは `InvariantCulture` 利用に変わっています。

ちなみに、「実行環境によって結果が変わる」という問題がある他にも、
パフォーマンス上の差もあります。
そもそも `CurrentCulture` 取得がそこそこ負担が掛かる処理というのもありますし、
`Ordinal` (文字コード通りに単なる数値比較するだけ)と比べてカルチャー依存処理("a" と "à" や、"か" と "が" 等の関係性を考慮)する方が重たいに決まっています。

「[カルチャー依存の API を呼ぶときは明示的にカルチャー指定しろよ](https://docs.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/quality-rules/ca1304)」というアナライザーがあったりするので、できればこの設定は有効にしておいた方がいいかもしれません。

## <a id="invariant-culture">Invariant とは…</a>

で、まあ、`Ordinal` でいいものは `Ordinal` にするとして。
どうしてもカルチャー依存なものは `InvariantCulture` にするとして。
次の問題は `InvariantCulture` が invariant (不変な) という名前を名乗っているくせに北米基準という点。

以下のようなコードを書くと、北米フォーマットになります。

```csharp {title="Invariant = 北米"}
using System.Globalization;
Console.WriteLine(new DateTime(2021, 8, 22).ToString(CultureInfo.InvariantCulture));
```

90年代の IT 業界にはあるあるだったんですけどね、「北米がデフォルト」な動作。
Java なんかでも、`println(new Date())` すると `Sun Aug 22 08:18:22 UTC 2021` とかになりますよね、たぶん。

元をたどるとこの「未設定なら北米準拠にする」みたいなの、
「"C" ロケール」とか言われてるみたいですね。
古い C 言語多言語対応ライブラリがそういう挙動だったからという。

一方の2000年代言語だと yyyy-MM-dd フォーマットなことが多いんですが(Go とか Rust とか)…

まあ、日本人にとって困るのはたぶん日付のフォーマットくらいだと思います。
小数点は北米と同じなので。
ところがまあ、小数点・桁区切り記号に . を使うか , を使うかは、[結構世界で2分されているみたい](https://twitter.com/ufcpp/status/1426474277541322760)なので注意が必要です。

## <a id="usa">北米フォーマット</a>

日本にいると「欧米」なんていう区切りで一括りにしてしまいがちなんですが…
アメリカ合衆国、結構異端ですからね。

* [ヤードポンド利用国](https://twitter.com/ufcpp/status/1426473850632491009)
* [ファーレンハイト利用国](https://twitter.com/ufcpp/status/1426473940453494786)
* [MM/dd/yyyy 利用国](https://twitter.com/ufcpp/status/1426474093096833027)

英語圏の中でも [Arbitrary Retarded Rollercoaster](https://imgur.com/MQ7LVC3)(勝手で遅れたジェットコースター)とか言われてるネタ画像出てきますからね、これ。
ちなみに、「アメリカはおかしい」って主張に対して「違うよ、アメリカの中でも合衆国だけだよ、一緒にしないで」までがセット。

それを見て「さすがに言いすぎじゃない？」って思って調べるとマジで1国だけ浮いているという。
特に日付。 dd-MM-yyyy (リトル エンディアン)と yyyy-MM-dd (ビッグ エンディアン)はどちらも分からなくはないものの、
さすがに MM-dd-yyyy (ミドル エンディアン？？？)はちょっと…

もちろん元々イギリスの文化なんですけども、当のイギリスはメートル法に改宗済み。
(といっても、法律上メートル法に変わってはいても、人が急に変われるわけもなく街中にはヤードポンドが残ってるそうですが。)
日付はぶれ気味(当人たちも混乱する)なので、Aug 22 みたいに書かないと MM-dd なのか dd-MM なのかわからなくなるから避けるみたいです。

という感じなので、そんな異端児をもって Invariant とか言われましても困ります… という感じになります。

最近、[C# 配信](https://www.youtube.com/channel/UCY-z_9mau6X-Vr4gk2aWtMQ)をしていて、文字列処理の話をするたびに
「ヤードポンドの国なので」という話になるのはこういう背景から。
実害を受けるのは日付の MM-dd-yyyy 書式だけなんですけども、 MM-dd-yyyy の国 ≒ ヤードポンドの国 ≒ ファーレンハイトの国 という。

## <a id="">ISO 8601 フォーマット</a>

と言うことで最近至った結論として、文字列処理は、

* `Ordinal` にできるならそれを、何らかのカルチャーに依存するなら `InvariantCulture` にする
* その上で、日付だけはフォーマットを明示的に指定

でやらないと事故る。

そこでまあ、ちょうど C# 10.0 で [`$"{X}"`](../../../../study/csharp/start/st_string.md#string-interpolation) の[カスタマイズをパフォーマンスを落とさずにできる仕様が入る](https://github.com/ufcpp/UfcppSample/issues/355)ので、
それを使って `InvariantCulture` 指定、かつ、日付だけは `O` 書式([ISO 8601](https://ja.wikipedia.org/wiki/ISO_8601)形式)を常に指定するコードを書いてみたり。

* [Iso8601.Format](https://github.com/ufcpp/UfcppSample/commit/930db8d2430417b412dfcdbefce6fa5b4323a4ee)

ちょうどカルチャー依存で困る時期だったので、ちょうど C# 10.0 でこの機能が入ったのは助かるかも。
