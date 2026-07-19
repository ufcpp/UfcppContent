---
title: "ピックアップRoslyn 4/10"
source_url: "https://ufcpp.net/blog/2016/4/pickuproslyn0410/"
content_type: "BlogEntry"
published_at: "2016-04-10T10:26:59"
updated_at: "2016-04-10T10:27:59"
tags: []
umbraco_id: 1886
parent_id: 1885
sort_order: 0
aliases: []
---

# ピックアップRoslyn 4/10

「C# 7」に入る範囲がそろそろ決まってきた感じ。

## C# Design Notes for Apr 6, 2016 (Tuples, Recursive patterns)

久々に言語デザインミーティングの議事録が。

- [C# Design Notes for Apr 6, 2016 #10429](https://github.com/dotnet/roslyn/issues/10429)

タプル型と、再帰的なパターン(位置指定パターン・プロパティ パターン)の詳細がだいぶ決まってきたっぽい。

タプル型は↓こんな感じ。

- メンバーの数と型が一致していたら、名前は無視して同一視する(代入可能)
  - box化・unboxしても同様の変換ルール適用可能
  - メンバーが int → long みたいな暗黙的型変換できる場合でも、タプル型の自動変換はいったんしないことにする(将来はわからない)
- 無名タプルも作れる(メンバー名指定なし)
- 匿名型の `new { p.X, p.Y }`みたいに、名前の射影(X, Yという名前を引き継ぐ)あり
- タプル型に対する拡張メソッドも作れる
- タプル型引数の規定値も指定できるようにしたいけど、そのためには何か新しい属性が必要になる(ので、いったん据え置きになりそう？)
- 0-tuples, 1-tuplesも検討したい
  - それぞれ、C#チーム内ではnuples, wonples (null+tuple, one+tupleの造語？)と呼んでるみたい
  - nuplesは、要するに「ユニット型」。`()`で書き表せそう
  - wonplesが難しそう。`(Type)`も`(value)`も現在のC#で有効な構文になっちゃってるんで
- タプル型戻り値を、out引数みたいな感じで、メンバー名で代入可能にはしない。return使え

再帰的なパターンの方は、もしかしたら全部やC# 7ではやらないかも。段階的に便利にしていくのでもいいだろうし、後からの追加に耐えれるようにC# ７を作れそう、とのこと。

## Language Feature Status

C# 7として出したいもの、だいぶ絞られてきたみたい。状況のまとめページができてます。

- [Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)

概ね、こないだのbuildでデモでプロトタイプが動いてたものに絞った感じ。リリース時期が大まかには決まってきたのかな？この絞り方(今ある程度動いてるものだけをまず出す)の感じからすると、Windows 10のレッドストーンと同時期？

## Source Generators

メタプログラミングがらみ、ソースコード ジェネレーターのドキュメントができてた。

- [Source Generators](https://github.com/dotnet/roslyn/blob/features/generators/docs/features/generators.md)

これはほんと一刻も早く試したい…

## C# as a High-Performance Language

C# vNextのパフォーマンス改善系の提案まとめページが。

- [State / Direction of C# as a High-Performance Language #10378](https://github.com/dotnet/roslyn/issues/10378)

[Build Insiderでやってる連載](http://www.buildinsider.net/column/iwanaga-nobuyuki)の次回のネタ、パフォーマンスにしようかなぁ。

Roslyn自体がパフォーマンス面に関して相当シビアだったり、ゲーム方面でC#が注目されてたり、.NET Coreがベンチマーク向上を頑張ってる真っ最中だったり、今、結構こういう機能が求められています。
