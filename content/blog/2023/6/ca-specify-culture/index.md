---
title: ".NET の文字列比較でカルチャー未指定を検知する"
source_url: "https://ufcpp.net/blog/2023/6/ca-specify-culture/"
content_type: "BlogEntry"
published_at: "2023-06-20T22:11:29"
updated_at: "2023-06-24T22:02:38"
tags: []
umbraco_id: 2466
parent_id: 2463
sort_order: 0
aliases: []
---

# .NET の文字列比較でカルチャー未指定を検知する

[先日の C# 配信](https://www.youtube.com/watch?v=M5weHOCzJ6E)で、
「これはブログに書いておくと助かる人がいるんじゃないか」と言われたものをブログ化。

## 背景: カルチャー依存問題再び

うちのブログでも何回か書いてるんですが、 .NET の文字列比較は、カルチャー依存比較するものと Ordinal (文字コード通り)比較するものが混在していて、なかなかにやばいです。

* [.NET のカルチャー依存 API 問題](../../../2021/8/invariantculture/index.md)
* [忘れがちなカルチャー依存問題](../../3/string-order/index.md)

[例えば以下のようなやつ](https://gist.github.com/ufcpp/071785157dfb8402af27b443427f8b90)。

<pre class="source" title="正気とは思えない ContaisKey">
<span class="reserved">using</span> <span class="reserved">static</span> System<span class="operator">.</span><span class="static"><span class="type">Console</span></span>;

<span class="comment">// 正規化すると同じ文字になる、文字コード的には別の文字。</span>
<span class="reserved">var</span> <span class="variable">s1</span> <span class="operator">=</span> <span class="string">&quot;a\u0301&quot;</span>; <span class="comment">// á = a + ́</span>
<span class="reserved">var</span> <span class="variable">s2</span> <span class="operator">=</span> <span class="string">&quot;\u00e1&quot;</span>; <span class="comment">// á</span>

<span class="comment">// これは false。Ordinal 比較。</span>
<span class="method"><span class="static">WriteLine</span></span>(<span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; { { <span class="variable">s1</span>, <span class="number">0</span> } }<span class="operator">.</span><span class="method">ContainsKey</span>(<span class="variable">s2</span>));

<span class="comment">// これは true。CurrentCulture 比較。</span>
<span class="method"><span class="static">WriteLine</span></span>(<span class="reserved">new</span> <span class="type">SortedDictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; { { <span class="variable">s1</span>, <span class="number">0</span> } }<span class="operator">.</span><span class="method">ContainsKey</span>(<span class="variable">s2</span>));
</pre>

なんでこんなことになるかというと、

* `Dictionary` は `EqualityComparer` 依存
    * `EqualityComparer` は `Ordinal` 比較
    * `Ordinal` 比較だと、文字コード的に別の文字は一致しない
* `SortedDictionary` は `Comparer` 依存
    * `Comparer` は `CurrentCulture` 比較
    * `CurrentCulture` 比較だと、たいていのカルチャーで `a\u0301` と `\u00e1` を同一視

という仕組み。

やばい。

前述のブログで一応の解決策として、
[InvariantGlobalization](https://learn.microsoft.com/ja-jp/dotnet/core/runtime-config/globalization) モード指定してしまうという案も書いたんですが、
このモード変更は影響範囲が結構大きいので、
保守しているコードベースがでかいとなかなか踏み切れない方も多いと思います。

## コード解析

このカルチャー依存文字列比較問題は .NET の中の人も把握していて、
.NET 5 の頃にいろいろと対策をしました。
その対策の1つに、[NetAnalyzers](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/overview?tabs=net-7) の提供があります。

NetAnalyzers は要するに、「.NET SDK 付属の公式コード解析」です。
例えば Visual Studio からなら、Dependencis → Analyzers → Microsoft.CodeAnalysis.NetAnalysers のところで確認できます。

![Visual Studio の Solution Exprorer で NetAnalyzers の内容を確認](../../../../../assets/media/1216/netanalyzers.png)

この中で、カルチャー依存 API 対策になっているのは以下の項目。

* CA1304: Specify CultureInfo
* CA1305: Specify IFormatProvider
* CA1307: Specify StringComparison for clarity
* CA1310: Specify StringComparison for correctness

こいつら、デフォルトでは Silent なんですよね…
(Silent = 何も表示しない。エラーや警告はおろか、サジェストのアイコンすら出ない。)

カルチャー依存 API のやばさのわりに Silent。
まあ、 .NET Framework 1.0 から .NET 5 までの十数年、ずっとそうでしたからね…

ということで、このコード解析の警告・エラー レベルを上げてしまった方がいいかもしれません。
.editorconfig に以下のような行を足せばエラーにできます。

<pre class="source">
<span class="type">[*.cs]</span>
<span class="reserved">dotnet_diagnostic.CA1304.severity</span> = error
<span class="reserved">dotnet_diagnostic.CA1305.severity</span> = error
<span class="reserved">dotnet_diagnostic.CA1307.severity</span> = error
<span class="reserved">dotnet_diagnostic.CA1310.severity</span> = error
</pre>

例えば以下のようなメソッドを警告にできます。

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Resources;

<span class="reserved">static</span> <span class="reserved">string</span><span class="operator">?</span> <span class="method"><span class="static">M</span></span>(<span class="type">ResourceManager</span> <span class="variable local">m</span>) <span class="operator">=&gt;</span> <span class="error" title="CA1304"><span class="variable local">m</span><span class="operator">.</span><span class="method">GetString</span>(<span class="string">&quot;&quot;</span>)</span>; <span class="comment">// CA1304</span>
<span class="error" title="CA1305"><span class="type struct">DateTime</span><span class="operator">.</span><span class="property"><span class="static">Now</span></span><span class="operator">.</span><span class="method">ToString</span>()</span>; <span class="comment">// CA1305</span>
<span class="error" title="CA1307"><span class="string">&quot;&quot;</span><span class="operator">.</span><span class="method">IndexOf</span>(<span class="string">' '</span>)</span>; <span class="comment">// CA1307</span>
<span class="error" title="CA1310"><span class="string">&quot;abc&quot;</span><span class="operator">.</span><span class="method">StartsWith</span>(<span class="string">&quot;abc&quot;</span>)</span>; <span class="comment">// CA1310</span>
</pre>
