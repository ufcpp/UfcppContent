---
title: "【C# 11 候補】 トップ レベル ステートメントの Main に属性を付ける"
source_url: "https://ufcpp.net/blog/2022/1/main-attribute/"
content_type: "BlogEntry"
published_at: "2022-01-25T21:27:32"
updated_at: "2022-01-25T21:27:32"
tags: []
umbraco_id: 2409
parent_id: 2401
sort_order: 4
aliases: []
---

# 【C# 11 候補】 トップ レベル ステートメントの Main に属性を付ける

ちょっと体調崩し気味だったので軽いネタに逃げる感じでわかりやすい C# 11 候補を1つ。

[トップ レベル ステートメント(が作る `Main` メソッド)に属性を付けたい](https://github.com/dotnet/csharplang/issues/5045)という話があります。

もう、割かし以下の利用例1個で説明終わりな感じ。

<pre class="source" title="main: 属性">
<code>[<span class="reserved"><em>main</em></span>: <span class="type">STAThread</span>]

<span class="reserved">using</span> System.Windows;

<span class="type">Clipboard</span>.<span class="method">SetData</span>(<span class="type">DataFormats</span>.Text, <span class="type">Environment</span>.OSVersion.<span class="method">ToString</span>());
</code></pre>

今、これと同じことをしようと思ったら、これだけのために `class Program { static void Main() { } }` が必要です。

とはいえ、`Main` メソッドに付けたい属性って `STAThread` 以外に何かありますかね？

という意味でニッチな需要ではあるんですけど、まあ、実装コストも低そうなので割かしやる気みたいです。
