---
title: "ピックアップRoslyn 7/14: Roles, extension interfaces, and static interface members"
source_url: "https://ufcpp.net/blog/2018/7/pickuproslyn0714/"
content_type: "BlogEntry"
published_at: "2018-07-14T16:44:31"
updated_at: "2018-07-14T16:51:11"
tags: []
umbraco_id: 2162
parent_id: 2159
sort_order: 2
aliases: []
---

# ピックアップRoslyn 7/14: Roles, extension interfaces, and static interface members

ここ数日、C# 8.0 (すぐ次のバージョン)を目標にした内容が多かったものの、今日のはもうちょっと先の話。

- [Exploration: Roles, extension interfaces and static interface members #1711](https://github.com/dotnet/csharplang/issues/1711)

タイトルに exploration って入っている通り、まだ「吟味・調査」的な段階のものです。
(特に、Roles の呼び名とかは結構不評。あくまで今現在そう呼んでるだけ。)

昔、[Shapes](https://github.com/dotnet/csharplang/issues/164)とか[Concept](https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md)とか言う案もあったんですが、
この辺りと狙いは同じ。

その狙いを、[extension everything](https://github.com/dotnet/roslyn/issues/11159)の延長として、以下の3つの要素の組み合わせで実現しようという話になります。

- Roles: 既存の型に対して、第三者がメンバーを追加するためのラッパー的なものを作る仕組み
- Extensions: 拡張メソッドの延長で、プロパティとか演算子とか、何でも「拡張」できるようにするもの。これに、Roles を組み合わせて、拡張でインターフェイスも実装できるようにしたい
- Static interface members: インターフェイスに、静的メソッドも含めるようにしたいというもの。単に(実装のある)静的メソッドを持つという話ではなく、静的な「抽象定義」(実装するクラス・構造体ごとに別の定義を持てる)を実現したい

Roles と Extensions は似ているものの、Roles は「あるインスタンスをキャストして使う」みたいな感じで、Extensionsは現状の拡張メソッドと同様「一定のスコープ内で、特定の型のインスタンス全部を拡張する」みたいな感じ。

このアイディアは、.NET ランタイム自体の改修が必要になります。
(現状の .NET の型システムの上に、C# コンパイラーによる構文糖衣で作ろうとするとちょっと問題がありそう。)
