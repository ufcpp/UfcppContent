---
title: "MemberNotNull (値型) 判定"
source_url: "https://ufcpp.net/blog/2022/1/member-not-null/"
content_type: "BlogEntry"
published_at: "2022-01-11T20:34:58"
updated_at: "2022-01-11T20:34:58"
tags: []
umbraco_id: 2408
parent_id: 2401
sort_order: 3
aliases: []
---

# MemberNotNull (値型) 判定

こないだ、[null フロー解析]と似たノリで、[構造体の default フロー解析が必要](../defaultable/index.md)という話をしました。

まあ、難航しそうではあるんですが…

とはいえ実は現在でも、「null チェックといいつつ、構造体に対しても働くフロー解析」があったりします。

## MemberNotNull

[nullable enable](../../../../study/csharp/resource/nullablereferencetype.md#opt-in) のとき、
非 null 参照型のフィールドやプロパティは、
コンストラクター内でちゃんと初期化する必要があります。

例えば以下のコードはプロパティ定義の行に警告。

<pre class="source" title="非 null 参照型のプロパティが未初期化">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">S</span> { <span class="reserved">get</span>; } <span class="comment">// CS8618 警告</span>
}
</code></pre>

以下のようにコンストラクターを足すと、今度はコンストラクターの行に警告。

<pre class="source" title="コンストラクター内でも非 null 参照型のプロパティが未初期化">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> S { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type"><span class="warning">C</span></span>() { } <span class="comment">// CS8618 警告</span>
}
</code></pre>

以下のように書くと警告は消えるんですが、

<pre class="source" title="ちゃんと非 null 値で初期化したので OK に">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> S { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">C</span>()
    {
        S = <span class="string">&quot;値は適当&quot;</span>; <span class="comment">// これで警告が消える。</span>
    }
}
</code></pre>

これをメソッド抽出してしまうと再び警告が出ます。

<pre class="source" title="初期化コードはコンストラクターに直にないとダメ">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> S { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="type"><span class="warning">C</span></span>() <span class="comment">// 再び CS8618</span>
    {
        <span class="method">Initialize</span>();
    }

    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Initialize</span>()
    {
        S = <span class="string">&quot;値は適当&quot;</span>;
    }
}
</code></pre>

null 許容参照型の初期リリースではこの問題を回避する手段はなかったんですが、後々、[`MemberNotNull`](../../../../study/csharp/resource/nullablereferencetype.md#MemberNotNull) という属性が追加されていて、
以下のように書けば警告をなくすことができるようになりました。

<pre class="source" title="MemberNotNull 属性">
<code><span class="reserved">using</span> System.Diagnostics.CodeAnalysis;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> S { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="type">C</span>()
    {
        <span class="method">Initialize</span>();
    }

    [<span class="type">MemberNotNull</span>(<span class="reserved">nameof</span>(S))]
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Initialize</span>()
    {
        S = <span class="string">&quot;値は適当&quot;</span>; <span class="comment">// 逆に、この行を消すと CS8774 警告。</span>
    }
}
</code></pre>

## 値型に対して MemberNotNull

そしてここでようやく本題。

`MemberNotNull` なんて名前をしていますが、
実際には「値を代入したかどうか」を見ているようで、
値型に対しても使えたりします。

<pre class="source" title="MemberNotNull(値型プロパティ)">
<code><span class="reserved">using</span> System.Diagnostics.CodeAnalysis;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="type">DateOnly</span> D { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="type">C</span>() = &gt; <span class="method">Initialize</span>();

    [<span class="type">MemberNotNull</span>(<span class="reserved">nameof</span>(D))]
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Initialize</span>()
    {
    <span class="warning">}</span> <span class="comment">// CS8774</span>
    <span class="comment">// member not &quot;null&quot; と言いつつ、非 null が確定している値型に対してもフロー解析してる。</span>
}
</code></pre>

「代入したかどうか」しか調べてない雰囲気？

代入さえされていれば `D = default;` でも警告が消えたりします。
(C# 10.0 時点では。)

<pre class="source" title="D = default でもよかったりする">
<code><span class="reserved">using</span> System.Diagnostics.CodeAnalysis;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="type">DateOnly</span> D { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="type">C</span>() =&gt; <span class="method">Initialize</span>();

    [<span class="type">MemberNotNull</span>(<span class="reserved">nameof</span>(D))]
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Initialize</span>()
    {
        D = <span class="reserved">default</span>; <span class="comment">// これでも OK。</span>
    }
}
</code></pre>

ということで、[defaultable value type](../defaultable/index.md) の仕様が入るまではまだ機能不足ではあるんですが。
とりあえず、`MemberNotNull` に対して値型のプロパティを渡せなくするみたいな処理をあえて入れたりはしていないようです。
将来的に defaultable value type のフロー解析もあるだろう見込みがあるのではじかないようにしてあるんじゃないかなぁと思います。
