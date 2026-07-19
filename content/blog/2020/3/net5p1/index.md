---
title: ".NET 5 Preview 1 / VS 16.6 Preview 1 公開記念"
source_url: "https://ufcpp.net/blog/2020/3/net5p1/"
content_type: "BlogEntry"
published_at: "2020-03-18T09:38:17"
updated_at: "2020-03-18T09:38:17"
tags: []
umbraco_id: 2284
parent_id: 2283
sort_order: 0
aliases: []
---

# .NET 5 Preview 1 / VS 16.6 Preview 1 公開記念

.NET 5 Preview 1 や、Visual Studio 16.5 正式版、Visual Studio 16.6 Preview 1 がアナウンスされました。

- [Announcing .NET 5 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-1/)
- Visual Studio Release Note(相変わらず ja-jp ページは古そうなので、en-us にリンク)
  - [16.5](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes#16.5.0)
  - [16.6 Preview 1](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.6.0-pre.1.0)

最近、ブログだと、C# 関連に絞って書いていて、
そうすると今回のアナウンスに関しては別に大して書くことはないんですけども。

今回から、ちょっと動画配信してみることにしました。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/a5Qs7u6CoqM" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

## C# 8.0 fix

ちなみに、アナウンスや Release Note 上は全く触れられていませんが、C# 8.0 にサイレントに修正が入っているはず。
昔、Gist に上げてあった以下のような修正が入っています。

- [16.5 で修正される null 許容参照型がらみの問題](https://gist.github.com/ufcpp/5dde1abd877c16c0e8f45e02c3858fe7)

Roslyn チームの中の人が「16.5 では間に合わなくて 16.6 になるよ」みたいなことをツイートしていた気もするんですけど、16.5 の方でも警告が消えていました。
ちゃんと確認できていなくて自信はないんですが、もしかしたらもうちょっと前から治っていたかもしれません。
16.6 Preview 1 を入れたことで .NET Core SDK が更新されて、それで治ったとかもあるかもしれません(未確認)。

## 動画配信

さて、ブログとしては以上。

Visual Studio の新しいリファクタリング機能とか、スクショぽとぺたブログを書くのもしんどいんですよね。
その点に関しては昨日、動画配信をやってよかったかなと思っています。
まあ、配信に使ったアプリ([OBS](https://obsproject.com/ja))が、右クリック メニューとか Quick Action の電球アイコンとかリファクタリング結果のプレビューとか、ポップアップする UI を移してくれなくてちょっと困っていますが…

動画配信はまだまだ黙々と環境を整えたりしている真っ最中で、
昨日も少々見切り発車な感じはあります。
とはいえ、こういうのは見切り発車であっても取り合えず始めてしまうことが大事なタイミングがありまして、うちの場合はそれが昨日の「Preview 1」かなと思って急ぎ配信することにしました。
「動画配信も Preview 1 だ」とか言っちゃえる年に1度のタイミングですからね。

動画配信に関しても「High-Level Goals」みたいなものはあるんですが、
その辺りはまた日を改めて話すと思います。
