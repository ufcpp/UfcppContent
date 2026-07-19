---
title: "Visual Studio 2019 Preview 3"
source_url: "https://ufcpp.net/blog/2019/2/vs2019p3/"
content_type: "BlogEntry"
published_at: "2019-02-14T21:27:29"
updated_at: "2019-02-14T21:27:29"
tags: []
umbraco_id: 2222
parent_id: 2220
sort_order: 1
aliases: []
---

# Visual Studio 2019 Preview 3

[Visual Studio 2019 Preview 3](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#VS2019_Preview3) 出てますね。

C# がらみは特にアナウンスもないんですが、[Roslyn の 16.0.P3 マイルストーン](https://github.com/dotnet/roslyn/milestone/37?closed=1)を見るに、大体は IDE がらみと null 許容参照型がらみを中心としたバグ修正っぽいです。

Preview 2 からあんまり期間が開いていませんし、元からバグ修正のみな予定だったのかも。

## switch 式のバグ

その割に、[`switch` 式を書くと IntelliSense が狂って最終的に Visual Studio がフリーズするバグ](https://github.com/dotnet/roslyn/issues/33378)が増えちゃってるみたいですが…
(コンパイルはできる。あくまで IDE だけの問題。)

ちょうど最近、「[C# によるプログラミング入門](../../../../study/csharp/index.md)」以下に C# 8.0 の話を書き足し始めていて、今週[パターンの話](../../../../study/csharp/datatype/patterns.md)を書き終えて、次は`switch`式かなぁとか思っていたところなんで、このフリーズはタイミングが悪すぎる…

## その他細かい修正

まあ、バグ修正の範囲内ですが、C# の言語機能にも多少変更がありました。

- [`switch` 式の最後の項の後ろに余計な `,` を付けても平気になった](https://github.com/dotnet/roslyn/issues/32292)
  - ついでに、[プロパティ パターン](../../../../study/csharp/datatype/patterns.md#property)でも末尾 `,` を受け付けるようになったらしい
- [`foreach` ステートメントで pattern-based な `Dispose` 呼び出しが働くようになった](https://github.com/dotnet/roslyn/pull/32640)
- [pattern-based な `await using` で、`DisposeAsync` メソッドの戻り値が `ValueTask` 以外の awaitable も受け付けるようになった](https://github.com/dotnet/roslyn/issues/32707)

サンプル: [Csharp80/Preview3](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2019/Csharp80/Preview3)
