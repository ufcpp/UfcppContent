---
title: "【C# 10.0】 トップ レベル ステートメントの変更点"
source_url: "https://ufcpp.net/blog/2021/11/top-level-csharp10/"
content_type: "BlogEntry"
published_at: "2021-11-23T15:15:33"
updated_at: "2021-11-23T15:16:09"
tags: []
umbraco_id: 2373
parent_id: 2363
sort_order: 5
aliases: []
---

# 【C# 10.0】 トップ レベル ステートメントの変更点

そういえば、文法的な変更ではないのでどこにも告知は出ていないもの(サイレント修正)なんですが、[トップ レベル ステートメント](../../../../study/csharp/cheatsheet/ap_ver9.md#top-level-statements) (C# 9.0 で追加)に変更点が2つあります。

## 空ステートメント禁止

以下の2つのコードを見比べてください。

1つ目:

<pre class="source" title="Hello World">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
}
</code></pre>

2つ目:

<pre class="source" title=";">
<code>;
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
}
</code></pre>

C# 9.0 当初、2つ目のコードもコンパイルできていました。
そして、実行結果がどうなるかと言うと…

* 1つ目: `Program.Main` が呼ばれて、Hello World! が表示される
* 2つ目: トップ レベル ステートメント扱いされて、何も表示されない

さすがに `;` 1個で挙動が変わっちゃうのはためらわれるというか、
[「2つ目で何も表示されなくなるのはバグだ」と認識しちゃう人もいた](https://github.com/dotnet/roslyn/issues/53472)ので[修正がかかりました](https://github.com/dotnet/roslyn/pull/54385)…

今は、2つ目のコードはコンパイル エラーになります。
空ステートメント1個だけのトップ レベル ステートメントは禁止。

でも、以下のようなコードは今 (C# 10.0) でも認められてるんですよね…

空じゃないステートメントもある:

<pre class="source" title="1つでも空じゃないものがあればOK">
<code>;
<span class="type">Console</span>.<span class="method">WriteLine</span>();
</code></pre>

空ブロック:

<pre class="source" title="; はダメでも {} は OK">
<code>{}
</code></pre>

`;` だけのものが禁止された経緯を考えると、`{}` だけのものも禁止されてもおかしくはないんですけど。
「`Main` を呼ぶかトップ レベル ステートメントを呼ぶか」の分岐条件が緩すぎるんですよね。
今後どうなるか…

## トップ レベル ステートメントを使った時のクラス名

トップ レベル ステートメントを使った時、例えば以下のようなコードを書くと、

<pre class="source" title="トップ レベル ステートメント利用例">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>();
</code></pre>

扱いとしては以下のようなコードに展開されていました。

<pre class="source" title="C# 9.0 時点でのトップ レベル ステートメントの展開結果">
<code><span class="reserved">using</span> System;

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">&lt;Program&gt;$</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">&lt;Main&gt;$</span>(<span class="reserved">string</span>[] <span class="variable">args</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
    }
}
</code></pre>

クラス名、メソッド名がどうなるかは仕様には明記されておらず、実装依存(変更が掛かっても文句は言えない)です。
とりあえず、「通常の C# では書けない名前」(unspeakable name というそうです)になっていました。
実装依存ですが、現在の実装では `<Program>$` みたいに `<>$` を入れて unspeakable にしています。

ところが、クラス名が unspeakable だと、[ASP.NET の単体テストで困ったそう](https://github.com/dotnet/roslyn/issues/54877)です。
ということで、[クラス名だけは speakable な `Program` に変更](https://github.com/dotnet/roslyn/pull/55368)。
C# 10.0 では上記のコードは以下のような展開結果に変更されています。

<pre class="source" title="C# 10.0 時点でのトップ レベル ステートメントの展開結果">
<code><span class="reserved">using</span> System;

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">&lt;Main&gt;$</span>(<span class="reserved">string</span>[] <span class="variable">args</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
    }
}
</code></pre>

トップ レベル ステートメントだけを使っている分には特に影響のない修正のはずなんですが…
例えば以下のようなコードがコンパイル エラーを起こすようになります。

<pre class="source" title="C# 10.0 ではエラーになるコード">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="error"><span class="type">Program</span></span>
{
}
</code></pre>

コンパイラーが生成する `Program` クラスと、コード中に手書きした `Program` クラスが衝突しています。

一方で、現状のこの実装を逆手に取ると、以下のようなコードはコンパイルできるようになります。

<pre class="source" title="現状の実装を逆手に取ったコード">
<code><span class="method">A</span>(); <span class="comment">// Program.A が呼ばれる。</span>

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">A</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
}
</code></pre>

ただ、`Program` というクラス名が仕様書上に明記されているわけではないので、将来もこのコードが有効であるという保証はあんまりできません。その点はご注意ください。
