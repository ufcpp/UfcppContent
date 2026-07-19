---
title: "field キーワード"
source_url: "https://ufcpp.net/blog/2025/1/field-keyword/"
content_type: "BlogEntry"
published_at: "2025-01-02T22:47:21"
updated_at: "2025-01-02T22:47:21"
tags: []
umbraco_id: 2508
parent_id: 2506
sort_order: 1
aliases: []
---

# field キーワード

「Rosly の [Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) にこの1・2か月で結構更新かかったね」という話題もたびたびあり、その辺りの話を。

Language Feature Status に並んでいるもののうち、いくつかは preview として現時点でもうすでに取り込まれています。

* field キーワード ← 今日はこれ
* First-class Span
* nameof(T<>)

今(執筆時、Visual Studio 17.13.0 Preview 2.1)の時点でも、
[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) に `preview` を指定すれば利用可能です。

最初は3つまとめて1ブログにしようかと思ってたんですが、
案外長くなったので個別に。
今日は field キーワードの話になります。
([昔のブログ](../../../2023/1/semi-auto-property/index.md)を参照して「やっと入ったよ」だけ書いて終わりかと思ったら案外新規に書くことがあり。)

### <a id="field">field キーワード</a>

プロパティ内において [`field` をキーワード扱い](https://github.com/dotnet/csharplang/blob/main/proposals/field-keyword.md)して、
「プロパティのバッキング フィールドを表す変数」にしようという案があって、
今は機能名としても「field キーワード」と呼ばれています。

<pre class="source" title="field キーワード">
<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="comment">// (既存の)自動プロパティ。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X1</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="comment">// X1 と同じ意味になる「field キーワード持ち」のプロパティ。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X2</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }

    <span class="comment">// 片方を自動、片方を field 持ちにもできる。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X3</span>
    {
        <span class="reserved">get</span>; <span class="comment">// 自動。</span>
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>; <span class="comment">// field 持ち。</span>
    }

    <span class="comment">// 自動プロパティでできたことは一通りこっちでもできる。</span>
    <span class="comment">// (イニシャライザーも持てたり、get-only とか init とかも。)</span>
    <span class="comment">// (というか、扱いは完全に自動プロパティと同じ。)</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X4</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>; } <span class="operator">=</span> <span class="variable local">x</span>;

    <span class="comment">// get 省略形の =&gt; 内でも field が使える。</span>
    <span class="comment">// int X5 { get; } と全く同じ。(コンストラクターで初期化可能。)</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X5</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>;
}
</pre>

[2年前にすでに「場合によっては C# 11 に入っていたかも」と言っていたもの](../../../2023/1/semi-auto-property/index.md)がようやく C# 14 で入ります。
当時は「半自動プロパティ」(semi-auto properties)とか呼んでいましたが、
結局「field キーワード」で行こうという感じになっているみたいです。

Visual Studio 17.12 Preview 3 / .NET 9 RC 2 の頃にはすでに merge されています。
つまり、C# 13 正式リリース(.NET 9)よりも前に、
すでに C# 14 の preview 機能が取り込まれている状態。
結構長いこと検討していて実装もあるものの、いくつか懸念があって延びに延びていて、
ようやく preview として世に出すことに。

懸念の1つは、これが「そこそこありえる」頻度の破壊的変更になることです。
「`field` という名前のフィールドがあって、`this.` は付けずに、プロパティの中で参照している」という状況が破壊的変更になります。
(「そこまで多くはないけど、まあそういう人も一定数いる」レベル。)

<pre class="source" title="field キーワードにはそこそこありえる破壊的変更">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">int</span> <span class="field">field</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Field</span>
    {
        <span class="comment">// C# 13 まで: field フィールドの参照。</span>
        <span class="comment">// C# 14 から: field キーワード。</span>
        <span class="comment">//             field フィールドはノータッチになる。</span>
        <span class="comment">//             field フィールドを参照したければ @field とか this.field にする。</span>
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">field</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }
}
</pre>

このコードは一応、C# 14 では警告になる予定です。
「`field` キーワードが `field` フィールドを隠してるけども意図通りか？」と怒られて、`@field` への書き換えを推奨されます。
もしかすると、今年のうち(C# 13 の間)に、「今のうちから `@field` に書き換えておいてくれ」アナライザーが提供されるかもしれません。

ちなみに、プロパティ内において、`field` は完全にキーワードになっています。
当初は「既存のコードを壊さない限りにキーワード扱いする」みたいな努力をするかどうかという話もあったんですが、複雑すぎるので断念しています。
例えば、`field` という名前のローカル変数があったとしてもキーワード扱いです。

<pre class="source" title="ローカル変数があっても field はキーワード扱い">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span>
        {
            <span class="reserved">var</span> <span class="variable"><span class="warning" title="CS0219">field</span></span> <span class="operator">=</span> <span class="number">1</span>;
            <span class="control">return</span> <span class="reserved"><span class="warning" title="CS9258">field</span></span>; <span class="comment">// これは field キーワード。フィールドの場合と同じく警告あり。</span>
        }
    }
}
</pre>

`nameof(field)` もエラーになります。
`nameof(int)` とかがエラーなのと同じ。

<pre class="source" title="nameof(field) はダメ">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property"><span class="warning" title="CS9264">X</span></span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">nameof</span>(<span class="reserved"><span class="error" title="CS8081">field</span></span>); <span class="comment">// ダメ。</span>
    }
}
</pre>

(余談で、[`value` もキーワードに変えちゃうか](../../../2024/2/value-as-context-keyword/index.md)という話もあったんですが、これは没になりました。)

これと関連して、以下のようなコードを書くと、タプル要素名のやつだけエラーを起こします。

<pre class="source" title="タプル要素名とか、匿名型のプロパティとか">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span>
        {
            <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> (<span class="reserved">field</span><span class="error" title="CS1002"><span class="error" title="CS1513"><span class="error" title="CS1026">:</span></span></span> <span class="number">1</span><span class="error" title="CS1002"><span class="error" title="CS1513">,</span></span> <span class="number">2</span><span class="error" title="CS1002"><span class="error" title="CS1513">)</span></span>; <span class="comment">// タプル要素名 (これだけコンパイル エラー)</span>
            <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="reserved">new</span> { <span class="property">field</span> <span class="operator">=</span> <span class="number">1</span> }; <span class="comment">// 匿名型のプロパティ</span>
            <span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Foo</span>() { <span class="field">field</span> <span class="operator">=</span> <span class="number">1</span> }; <span class="comment">// オブジェクト初期化子でのフィールド/プロパティ参照</span>
            <span class="control">if</span> (<span class="variable">y</span> <span class="reserved">is</span> { <span class="property">field</span>: <span class="number">1</span> }) { } <span class="comment">// プロパティ パターンでのフィールド/プロパティ参照</span>

            <span class="control">return</span> <span class="reserved">field</span>;
        }
    }

    <span class="reserved">class</span> <span class="type">Foo</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">field</span>;
    }
}
</pre>

最後にもう1つ、[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)のフロー解析の問題があります。
プロパティが `T` のとき、そのバッキング フィールド(`field` キーワードの実体)は `T` であるべきか、`T?` であるべきか。

例えば以下のような `??=` を使った遅延初期化コードはよく書くと思います。

<pre class="source" title="??= で遅延初期化">
<span class="reserved">class</span> <span class="type">A</span>(<span class="type">Type</span> <span class="variable local">type</span>)
{
    <span class="comment">// Type.Name のキャッシュ。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property"><span class="warning" title="CS9264">Name</span></span>
    {
        <span class="comment">// 遅延初期化にしたいので field ??= で代入。</span>
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??=</span> <span class="variable local">type</span><span class="operator">.</span><span class="property">Name</span>;
    }
}
</pre>

現状(Visual Studio 17.13.0 Preview 2.1 時点)、「プロパティが `T` なら `field` も `T`」です。
この例の場合、`string` (not null)。
「not null なフィールドがあるのに、コンストラクターで初期化していない」という警告が出ます。

[解決策](https://github.com/dotnet/csharplang/blob/main/proposals/field-keyword.md#nullability)は検討さいれているんですが、短期的には `MaybeNull` 属性を使って回避してくれと言われています。

<pre class="source" title="当面、MaybeNull で回避">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">class</span> <span class="type">A</span>(<span class="type">Type</span> <span class="variable local">type</span>)
{
    [<span class="reserved">field</span>: <span class="type">MaybeNull</span>] <span class="comment">// この属性によって、field が string? 扱いになる。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Name</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??=</span> <span class="variable local">type</span><span class="operator">.</span><span class="property">Name</span>;
    }
}
</pre>

上記[解決策](https://github.com/dotnet/csharplang/blob/main/proposals/field-keyword.md#nullability)が間に合うなら、
「いったん `field` が `T?` と仮定してフロー解析して nullable 警告を起こすかどうか」をみてバッキング フィールドが `T` か `T?` かを決定するとのこと。
これが入れば `MaybeNull` を付ける前のコードでも警告が出なくなる予定です。
