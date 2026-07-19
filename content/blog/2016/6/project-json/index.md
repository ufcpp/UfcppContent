---
title: "「project.json辞めます」の意味"
source_url: "https://ufcpp.net/blog/2016/6/project-json/"
content_type: "BlogEntry"
published_at: "2016-06-28T14:39:34"
updated_at: "2016-06-28T14:39:34"
tags: []
umbraco_id: 1926
parent_id: 1908
sort_order: 2
aliases: []
---

# 「project.json辞めます」の意味

[.NET Core](https://github.com/dotnet/core)といい[C#コンパイラー](https://github.com/dotnet/roslyn)といいオープンソースになったおかげで常に最新の情報を拾えてありがたい限りなわけですが。
緩い段階のものが見えすぎて、いろいろ振り回されたりもします。

最近の話題というと、[「project.json辞めます」騒動](https://www.infoq.com/news/2016/05/project-json/)。

これ、あんまり字面通り受け止めるとダメだと思うんですよね。
話の出どころは「[ASP.NET Community Standup](https://live.asp.net/)」の1コマ、[5月10日放映分](https://www.youtube.com/watch?v=P9HqMZviaMg&list=PL0M0zPgJ3HSftTAAHttA3JQU4vOjXFquF&index=0)での会話らしいんですが。
こういう、会話中で出てきた言葉って、割かし不正確(にとられかねない緩い表現)なわけです。

## ここでいう「project.json辞めます」の意味

.NET Coreは、当初、脱msbuildを目指していたわけです。.NET Coreにとっかかり始めた当時の発想としては、

- msbuildとか、MS製のビルド ツールはC++、COM製で、クロスプラットフォーム化が大変
- ASP.NET に対応できればいい
    - モダンなWebっぽいものを作りたい

というような感じだったはずです。

ところが今は、

- msbuildが.NETでクロスプラットフォーム実装になってる
- UWP、Xamarinなどなど、いろんなものに対応する必要が出てきた

と、状況が変わってる。

なので、実際のところは、「ASP.NET チーム主導で独自に作ってたビルド ツール辞めます」、「msbuild使います」という話になります。

で、「ASP.NET チーム主導で独自に作ってたビルド ツール」に名前がないもんだから、そいつが解釈するプロジェクト設定ファイルであるところの「project.json」を辞めるという言い回しをしてしまっているという状況。

## もう1つのproject.json

.NET Coreのリリースがずるずると伸びてる間に、1年近く早く先にリリース段階に達した別プロダクトがあるわけです。[UWP](https://msdn.microsoft.com/en-us/windows/uwp/get-started/whats-a-uwp)とか[NuGet](https://github.com/nuget) 3とか。

.NET Coreは標準ライブラリの参照でNuGetの仕組みに乗っかってるわけですが、
同じく、NuGetに乗っかりたかったのがUWP。
Windwos 10のリリースに合わせる必要があったので、2015年7月の段階でRTM。
そのために、NuGetも同時期にバージョン アップしました。

このとき、.NET Coreのproject.jsonのうち、パッケージ参照がらみの部分だけを抜き出して使うことになりました。
つまり、NuGet 3では、packages.configを辞めて、project.json形式を使うように。
ここで、project.jsonが2つに分岐したわけです。
この辺りは去年の12月にブログを書いてます。

- [.csproj + project.json](../../../2015/12/csprojprojectjson/index.md)

そしてこちらのproject.jsonは、msbuild に組み込まれています。

先ほど言った通り、.NET Coreチームの言うところの「project.json辞めます」は、実際には「msbuild使います」の意味です。
つまるところ、NuGet側のproject.json (msbuild対応済み)は辞めるどころか、むしろ積極的に使われるようになる立場。

## nuget.json？

で、パッケージ参照設定なのに名前が「project」なの？とかいう問題に至ります。
元々はprojectだったけども。その元々の方がお亡くなりになってしまい…

ってことで、NuGetが使っている方のproject.jsonはnuget.jsonにリネームしようみたいな話もあるみたい？

まあなんか、この「project.json辞めます」騒動のせいでもう、そこいら中から「project.json辞めたんじゃないの？」「UWPのやつどうなるの？」みたいな疑問の声が出まくってるんで、もう、リネーム必須な状況になってしまってるんじゃないかなぁ…という気がします。

## project.json便利だったのに…

.NET Coreチームの言うproject.jsonは、.csprojほどファイルの衝突が起きなくて、ソースコードのバージョン管理的には大変ありがたかったです。

「project.json辞めます」、すなわち、「msbuild使います」というのは、結局、「.csprojに戻ります」という意味でもあります。
ありがたかった機能も一旦なくなるわけですが…

.NET Coreチーム曰く、「msbuild/.csprojの方を改善します」とのこと。csprojでも、「フォルダー中の.csは全部コンパイル対象」みたいなことをできるようにしたいみたいです。
てことで、「将来的に、msbuild/.csprojがもっと便利になる」と捉えるべき。

## まとめ

緩い段階で出てきた話は字面通りに受け止めちゃダメというか。
今時は情報の出どころがものすごい緩いことが多いというか。
みんなタイトルだけ読んでわかった気になるんだから、緩い出どころを元にブログのタイトル付けちゃダメというか。

.NET Coreチームの言う、元々の意味のproject.jsonはどうやらなくなります。
ただし、その利便性をmsbuildに引き継ぎたいそうです。

「project.json辞めます」の本当の意味は「.NET Coreでもmsbuildを使います」です。
project.jsonでやろうとしていたことを、msbuildに取り込んでいきたいそうなので、「msbuildが便利になる」と考えるべきです。

また、NuGetの方のproject.jsonはすでに1年前からmsbuldに組み込み済みなので、なくならないどころか、むしろ今後積極的に使われるようになる側にいます。
ただし、nuget.jsonにリネームはするかも。
