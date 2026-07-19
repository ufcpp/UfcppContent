---
title: "ジェネリック型引数の部分型推論"
source_url: "https://ufcpp.net/blog/2024/2/partial-inference/"
content_type: "BlogEntry"
published_at: "2024-02-17T15:43:51"
updated_at: "2024-02-17T15:43:51"
tags: []
umbraco_id: 2486
parent_id: 2480
sort_order: 5
aliases: []
---

# ジェネリック型引数の部分型推論

C# のジェネリック型引数の推論を賢くしたいという話は、issue として記録されている分に限っても5年くらい前からあります。

* [Champion: "Partial Type Inference"](https://github.com/dotnet/csharplang/issues/1349)

現状の C# の型推論は割と "All or Nothing" で、
`new()` みたいに型全体の省略はできても、`new List<>()` みたいな「一部分だけ省略」ができません。

```csharp
// 型全体の推論は可能。
// 左辺から型が決定されて、new() は new List<int>() と解釈される。
List<int> x1 = new();

// 一方、型引数だけの省略というのができない。
List<int> x2 = new List<>(); // 要は Java のダイヤモンド演算子みたいなのとか、
List<int> x3 = new List<_>(); // あるいは「ここは推論して」を表すキーワードを用意したい。

// 特にこういう「部分推論」ができないと困るのは以下のような場面。
IList<int> x4 = new List<_>(); // 左辺がインターフェイス、右辺が具象型なので new() とは書けない。
```

この issue はずいぶん前から「Champion」(C# チーム内の誰かが興味を持って推進している状態)にはなっていたんですが、しばらく動きはありませんでした。

それが去年くらいから動きあり。
具体的な提案が出ました。

* [Add proposal for partial type inference #7582](https://github.com/dotnet/csharplang/pull/7582)

現状有力な案は「推論したいところに `_` を入れる」文法です。
先日の「[C# での破壊的変更の今後の扱い](../breaking-changes/index.md)」の話とも関連していますね。
discard と同じく `_` を利用するとなると、`class _ { }` が邪魔なので。

そして最近これが Language Design Meeting でも取り上げられたみたいです。

* [C# Language Design Meeting for February 7th, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-07.md)

これによれば以下のような感じ。

* コミュニティ貢献。作者は修士論文として取り組んでいるらしい
* C# チームも乗り気でサポート中
  * 特に、「型推論が面倒だからインターフェイス使わない」(`IEnumerable<T>` じゃなくて `List<T>` を使う方を好む)みたいな問題を減らせる点を評価
* 出ている提案はパフォーマンスも考慮していて、「コンパイル時間が指数的に爆発する」みたいな問題は避けれそう
  * いくつか制限することで、(型推論アルゴリズムとして有名な) Hindley-Milner ほど複雑にはならず、指数的な処理は避けれる
* ポリシーとして「`var` は使わない」とか「型が明白な場合にのみ `var` を使う」みたいなことがあり、現在の `var` と同じく部分型推論にもその手のアナライザー提供の可能性あり
  * ただ、部分型推論にとって「型が明白」とはどういうものかは要検討
* C# 13 には入らなさそう(原文 "not going to" なので結構な確度で入らない)なものの、取り組みには前向き
