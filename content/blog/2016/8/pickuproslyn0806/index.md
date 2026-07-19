---
title: "ピックアップRoslyn 8/6"
source_url: "https://ufcpp.net/blog/2016/8/pickuproslyn0806/"
content_type: "BlogEntry"
published_at: "2016-08-06T08:38:12"
updated_at: "2016-08-06T08:38:12"
tags: []
umbraco_id: 1936
parent_id: 1932
sort_order: 1
aliases: []
---

# ピックアップRoslyn 8/6

## Source Generatorが間に合わなかった理由

[昨日のブログ](../pickuproslyn0805/index.md)にある通り、C# 7に入る機能リストが更新されて、入るもの・入らないものに変化があったわけですが。

それと関連して:

[https://github.com/dotnet/roslyn/issues/12630#issuecomment-237959967](https://github.com/dotnet/roslyn/issues/12630#issuecomment-237959967)

やっぱり、Source Generatorが外れたのがショックだっていう人が現れて、それに対してC#チームの人が釈明してたりします。

まあ、大体[昨日書いたことの通り](../pickuproslyn0805/index.md)ですね。
IDE側の対応が大変で今回は無理とのこと。

Source Generatorって物自体が、IDE側にとっては結構大きな課題でして。例えば、コード生成によってプロパティなどのメンバーが増えたりするわけですが、その生成されるメンバーをIntelliSenseに出したりしようとすると、文字入力のたびにSource Generatorが走り、構文解析しなおすのかという話になります。IntelliSenseなんかは特にパフォーマンスにシビア（ここがもっさりしているとIDEのユーザーのストレスが非常に大きい）なので、Source Generatorが悩ましい存在になります。

もちろん、間に合ってるC#言語機能(replace/original)だけでも先に入れれないかなんてことも言われています。
けども、今早まって入れても、IDE側も対応できるようになった頃にはもっといいアイディアが出るかもしれない。より良いものがもし入ってしまったら、早まって入れたreplace/original構文が無駄・邪魔になってしまう。そういう状態にはしたくない。という話も。

ってことで、Source Generatorはもう少し成熟を待たないといけないということになります。

## Sprint Summary

最近結構、「C#チームの中の様子が見えない。クローズだ。Roslynプロジェクトはオープンソースなんじゃなくて、オープンダンプ（クローズに作ったものをオープンにコピーして出してるだけ）だ」的なことを言い続けてるtroll（荒らし認定される勢いでしつこい人）がいらっしゃったり。

まあ、Roslynプロジェクトは確かに3000件ものIssueがオープンなまま整理されてなったりしますし、C#チーム内で起きたことも定期的には出てこない（ふと思い出したかのように出てくる）んで、あんまり擁護もできないんですが。

ということで、C#チームの人が重い腰をあげて「スプリントごとの作業概要」を公開。

[Sprint 104 Summary #12974](https://github.com/dotnet/roslyn/issues/12974)

まあ確かに、これくらいはちゃんと定期的に出そうよ…
