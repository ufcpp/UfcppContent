---
title: "C# 13 でのコレクション式 - 制限の緩和の話"
source_url: "https://ufcpp.net/blog/2024/3/colexpr13-relax-restriction/"
content_type: "BlogEntry"
published_at: "2024-03-09T21:31:10"
updated_at: "2024-03-09T21:31:10"
tags: []
umbraco_id: 2493
parent_id: 2490
sort_order: 2
aliases: []
---

# C# 13 でのコレクション式 - 制限の緩和の話

## C# 13 でのコレクション式 - 制限の緩和の話

C# 12 で[コレクション式](../../../../study/csharp/cheatsheet/ap_ver12.md#collection-expression)が入ったわけですが、
スケジュールの都合で「C# 12 後に改めて検討する」ということになった機能がたくさんあります。
C# 12 リリース(2023/11)直後から再検討が始まっていて、先月にはある程度まとまった計画が出ています。

* [[Proposal]: Collection Expressions Next (C#13 and beyond)](https://github.com/dotnet/csharplang/issues/7913)

量が多いのでちょっとずつ取り上げ…

* ディクショナリ式
* 自然な型
* インラインなコレクション式
* コレクションに対する拡張メソッド
* 現状でコレクション式に対応してない型
* 非ジェネリックなコレクションのサポート
* 制限の緩和 ← 今日はこれ

## 制限の緩和

今、コレクション式の要素の型は `IEnumerable<T>` の `T` で判定しています。

<pre class="source" title="iteration type を元に型判定してる">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">A</span>()) ; <span class="comment">// この x は int</span>

<span class="comment">// Add(int) だけあればよさそうに見えるのに、</span>
<span class="comment">// 実際には IEnumerable&lt;int&gt; をみて「int のコレクション」と判断してる。</span>
<span class="type">A</span> <span class="variable">a</span> <span class="operator">=</span> [<span class="number">1</span>];

<span class="comment">// foreach すると int を列挙する型。</span>
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt; <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span><span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
}
</pre>

<pre class="source" title="インターフェイス実装を消したらエラー">
<span class="comment">// foreach はインターフェイスがなくても GetEnumerator っていう名前のメソッドさえ持っていれば OK なのに。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">A</span>()) { }

<span class="comment">// これはダメになる。</span>
<span class="type">A</span> <span class="variable">a</span> <span class="operator">=</span> <span class="error" title="CS9174">[<span class="number">1</span>]</span>;

<span class="comment">// インターフェイスを削るとコレクション式で使えなくなる。</span>
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
}
</pre>

<pre class="source" title="コレクション初期化子は使えるのに…">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="comment">// foreach なんとか OK。</span>
<span class="comment">// non-generic な GetEnumerator が呼ばれてるので object を介してるけど…</span>
<span class="control">foreach</span> (<span class="reserved">int</span> <span class="variable">x</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">A</span>()) { }

<span class="comment">// 旧来のコレクション初期化子は使えるのに…</span>
<span class="type">A</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span>() { <span class="number">1</span> };

<span class="comment">// コレクション式はダメになる。</span>
<span class="type">A</span> <span class="variable">a2</span> <span class="operator">=</span> <span class="error" title="CS9215"><span class="error" title="CS1503">[<span class="number">1</span>]</span></span>;

<span class="comment">// non-generic インターフェイスに変えると？</span>
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IEnumerable</span>
{
    <span class="reserved">public</span> <span class="type">IEnumerator</span> <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
}
</pre>

ちなみに、この「`IEnumerable<T>` の `T`」以外は受け付けなかったりします。
これも、コレクション初期化子時代はできたこと。

<pre class="source" title="コレクション初期化子は使えるのに… (再)">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="comment">// 旧来のコレクション初期化子は string を受け付けるのに…</span>
<span class="type">A</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span>() { <span class="number">1</span>, <span class="string">&quot;2&quot;</span> };

<span class="comment">// コレクション式はダメになる。</span>
<span class="type">A</span> <span class="variable">a2</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="string"><span class="error" title="CS0029">&quot;2&quot;</span></span>];

<span class="comment">// Add だけは string 受付。</span>
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">string</span> <span class="variable local">x</span>) { }

    <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt; <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span><span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
}
</pre>

これが、非ジェネリックな `IEnumerable` を使うと object のみ受け付けるようになるみたいです。
しかもこれ、 Visual Studio 17.10 以前であれば受け付けていたコードがコンパイル エラーになるというひと悶着あり。

* [False positive for CS1503 with MSBuild 17.10, but not dotnet build #72098](https://github.com/dotnet/roslyn/issues/72098)

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="comment">// 旧来のコレクション初期化子は string を受け付けるのに…</span>
<span class="type">A</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span>() { <span class="number">1</span>, <span class="string">&quot;2&quot;</span> };

<span class="comment">// これ、ちょっと前まで受け付けていたらしい。</span>
<span class="comment">// Visual Studio 17.10 Preview 1 だとエラー。</span>
<span class="type">A</span> <span class="variable">a2</span> <span class="operator">=</span> <span class="error" title="CS9215"><span class="error" title="CS1503">[<span class="number">1</span>, <span class="string">&quot;2&quot;</span>]</span></span>;

<span class="comment">// non-generic なインターフェイスを実装。</span>
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">string</span> <span class="variable local">x</span>) { }

    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span><span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
}
</pre>

[意図した破壊的変更](https://github.com/dotnet/roslyn/blob/main/docs/compilers/CSharp/Compiler%20Breaking%20Changes%20-%20DotNet%208.md#collection-expression-target-type-must-have-constructor-and-add-method) (たぶん、[1/8 の LDM での決定](https://github.com/dotnet/csharplang/discussions/7832))だそうですが、
本当にこの変更をしてよかったのかどうか。
こういう非ジェネリック `IEnumerable` だけ実装して、`Add` でちゃんとした型を指定しているクラス、
WPF とか WinForms には結構あって、それが突然コンパイルできなくなったものでちょっとした混乱が起きています。

ちなみに、この変更の理由は、こうしておかないと [`params` コレクション](../params-collections/index.md)を使った時のオーバーロード解決のコストが高くなるからだそうです。
制限を緩めるとして、もしかしたら「コレクション式では使えるけども `params` コレクションでは使えない」みたいな状況が増えるかもしれません。

一方、そもそもとして `IEnumerable` 実装は必要なのかという問題が。
何せ、コレクションを作る時点では `GetEnumerator` は要らず、`CollectionBuilder` 属性で指定した `Create` メソッドだけあれば事足ります。
例えば、型によっては「別のコレクションを作るための足掛かりにするもので、直接列挙はしない」みたいなものがあります。
(実際、Roslyn チーム自身が1件そういう問題を踏んだりしています: [CSharpTestSource](https://github.com/dotnet/roslyn/blob/026c96327b02c5ce4d3208f821e02d2ffa825312/src/Compilers/Test/Utilities/CSharp/CSharpTestSource.cs#L22)。`SyntaxTree[]` を作るために使っていて、この型自体からの列挙はしない)。

ということで、`CollectionBuilder` 属性指定のコレクション型の場合、
`Create` メソッドの引数の `ReadOnlySpan<T>` から要素の型を決めようという提案が出ています。

* [Open issue: relax requirement that type be enumerable to participate in collection expressions #7744](https://github.com/dotnet/csharplang/issues/7744)
