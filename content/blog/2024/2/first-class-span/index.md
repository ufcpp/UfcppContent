---
title: "ファーストクラスな Span 型"
source_url: "https://ufcpp.net/blog/2024/2/first-class-span/"
content_type: "BlogEntry"
published_at: "2024-02-13T23:12:43"
updated_at: "2024-02-15T10:16:24"
tags: []
umbraco_id: 2485
parent_id: 2480
sort_order: 4
aliases: []
---

# ファーストクラスな Span 型

今日は「`Span<T>`、`ReadOnlySpan<T>` をコンパイラーで特別扱いしたい」という話。

C# 7.2 の頃、[`Span<T>` 型](../../../../study/csharp/resource/span.md)が追加されて、
安全性を損なわずに unsafe コード並みにパフォーマンスのよいコードが書けるようになりました。
それ以来、.NET の標準ライブラリでもいろんな場面で`Span<T>` 型が活用されています。

いまや結構重要なポジションを担う型なわけですが、
現状の扱いはあくまで「普通の構造体の1つ」です。
そのため微妙にオーバーロード解決とかで困り気味。

例えば直近では、C# 12 でコレクション式を導入するにあたって「[普通にやってたら使い勝手が悪いので `Span` を特別扱い](../../../../study/csharp/datatype/collection-expression.md#priority)」みたいなことをやっています。

```csharp {title="コレクション式の Span 特別対応" error-ranges="2:18-2:19" error-diagnostics="CS0121@2:18-2:19"}
// 普通にやると IEnumerable と Span の優先度はつかなくてコンパイルエラー。
EnumerableVsSpan.M(new int[5]);

// コレクション式は Span を優先する。
EnumerableVsSpan.M([1, 2, 3]);

// Span を優先しちゃう(パフォーマンス的に好ましくない)。
SpanVsReadOnlySpan.M(new int[5]);

// ReadOnlySpan を優先するよう特別扱い。
SpanVsReadOnlySpan.M([1, 2, 3]);

class EnumerableVsSpan
{
    public static void M(IEnumerable<int> _) { }
    public static void M(Span<int> _) { }
}

class SpanVsReadOnlySpan
{
    public static void M(ReadOnlySpan<int> _) { }
    public static void M(Span<int> _) { }
}
```

また、いわゆる共変性の辺りが微妙だったりします。
以下のように、コンパイルできてほしくないのに実行時エラーになるのが1件、
コンパイルできてほしいのにできないのが1件。

```csharp {title="ReadOnlySpan の共変性" error-ranges="21:32-21:39" error-diagnostics="CS0029@21:32-21:39"}
var strArray = new string[5];

// 行ける。 Span に implicit operator が定義されているので。
Span<string> strSpan = strArray;

// なぜか行ける…
// 配列に共変性(object[] objArray = strArray; が合法という負の遺産)があるせい。
// が、実行時例外起こす。
Span<object> objSpan = strArray;

// 行ける。
ReadOnlySpan<string> strRos1 = strArray;
ReadOnlySpan<string> strRos2 = strSpan;

// これも行ける。
ReadOnlySpan<object> objRos1 = objSpan;
ReadOnlySpan<object> objRos2 = strArray;

// ダメ…
// (できても問題ないけど、ReadOnlySpan を特別扱いしないとコンパイラーにはそれがわからない。)
ReadOnlySpan<object> objRos3 = strSpan;
```

ということで、まあ、
「`Span<T>`、`ReadOnlySpan<T>` をコンパイラーで特別扱いしたい」という話になります。

* ドキュメント追加の PR: [Add first-class span types proposal](https://github.com/dotnet/csharplang/pull/7904)
* トラッキング issue: [[Proposal]: First-Class Span Types](https://github.com/dotnet/csharplang/issues/7905)

「配列から `IEnumerable<T>` への変換」とかが元からそうなんで、その辺りに並べて `Span<T>`、`ReadOnlySpan<T>` がらみの仕様を入れるとのこと。

提案は[2月5日の Language Design Meeting であっさり了承されてる](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-05.md)し、
割かしコレクション式の取り組みからの流れっぽい感じもするので C# 13 に入りそうな感じがしますね。
懸念として、ちょっとした(めったに起こらなさそうな)破壊的変更があり得るので、
そのリスクがどう評価されるか次第。
