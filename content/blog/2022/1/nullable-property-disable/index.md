---
title: "nullable 警告もみ消し(来年までの我慢)の手段"
source_url: "https://ufcpp.net/blog/2022/1/nullable-property-disable/"
content_type: "BlogEntry"
published_at: "2022-01-28T20:49:46"
updated_at: "2022-01-28T20:50:52"
tags: []
umbraco_id: 2410
parent_id: 2401
sort_order: 5
aliases: []
---

# nullable 警告もみ消し(来年までの我慢)の手段

今日はとあるアンケートの結果を乗せておこう的な話。

## 背景: 非 null プロパティの初期化

[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)の仕様が入って以来、以下のようなコードに警告が出るようになりました。

<pre class="source" title="NRT 警告がどうしても出てしまう例">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">X</span>;
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">Y</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">Z</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</code></pre>

C# 10.0 現在、この警告を回避する唯一の方法は「ちゃんとコンストラクターで初期化すること」です。

<pre class="source" title="C# 10.0 現在の唯一の回避方法">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> X;
    <span class="reserved">public</span> <span class="reserved">string</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> Z { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    <span class="reserved">public</span> <span class="type">A</span>(<span class="reserved">string</span> <span class="variable">x</span>, <span class="reserved">string</span> <span class="variable">y</span>, <span class="reserved">string</span> <span class="variable">z</span>) =&gt; <em>(X, Y, Z) = (<span class="variable">x</span>, <span class="variable">y</span>, <span class="variable">z</span>)</em>;
}
</code></pre>

困るのが、「このプロパティは `new() { X = "", Y = "", Z = "" }` みたいに初期化子で初期化したい」という場面。
結構あると思うんですよね、コンストラクターを定義したくない・できないとき。
今のところいい解決策がない状態です。

### 【将来予定】 required

一応補足。
将来的には解消する予定です。
今のところ C# 11.0 目標で、`required` 修飾子を付けるという案が進められています。

<pre class="source" title="required 修飾子">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved"><em>required</em></span> <span class="reserved">string</span> X;
    <span class="reserved">public</span> <span class="reserved"><em>required</em></span> <span class="reserved">string</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved"><em>required</em></span> <span class="reserved">string</span> Z { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</code></pre>

これが付いていると、オブジェクト初期化子で非 null な値を渡すことを義務付けられるようになるので、クラス定義側には警告が出なくなります。

<pre class="source" title="required の効果">
<code><span class="reserved">var</span> <span class="variable">a1</span> = <span class="reserved">new</span> <span class="type"><span class="warning">A</span></span>(); <span class="comment">// required プロパティ/フィールドに値を与えていないので警告</span>

<span class="reserved">var</span> <span class="variable">a2</span> = <span class="reserved">new</span> <span class="type">A</span>()
{
    X = <span class="reserved"><span class="warning">null</span></span>, <span class="comment">// null を与えたので警告</span>
};

<span class="comment">// required プロパティ/フィールド全てにちゃんと値を与えたのでOKに</span>
<span class="reserved">var</span> <span class="variable">a3</span> = <span class="reserved">new</span> <span class="type">A</span>()
{
    X = <span class="string">&quot;&quot;</span>,
    Y = <span class="string">&quot;&quot;</span>,
    Z = <span class="string">&quot;&quot;</span>,
};
</code></pre>

## 現状の回避策

ということで、`required` によって来年には根本解決の当てがあるわけですが。
そうなると、今現在 `A` の作者が頑張って回避策を取る必要もないよなぁ…
ということになって、
「来年まではやっつけ対処でもみ消ししとこう」という発想になります。

ただ、
こういうやっつけほど具体的にどう対処しようか迷います。
また、もみ消しにいくつかの手段があるのでその点もちょっと迷うポイント。

ということでアンケート。

### 選択肢

選択肢1. 該当行を `nullable disable`

<pre class="source" title="nullable disable">
<code><span class="reserved">class</span> <span class="type">A</span>
{
<span class="preprocess">#</span><span class="preprocess">nullable</span> <span class="preprocess">disable</span> <span class="preprocess">warnings</span>
    <span class="reserved">public</span> <span class="reserved">string</span> X;
    <span class="reserved">public</span> <span class="reserved">string</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> Z { <span class="reserved">get</span>; <span class="reserved">init</span>; }
<span class="preprocess">#</span><span class="preprocess">nullable</span> <span class="preprocess">restore</span> <span class="preprocess">warnings</span>
}
</code></pre>

選択肢2. 該当行を `pragma warning disable`

<pre class="source" title="pragma warning disable">
<code><span class="reserved">class</span> <span class="type">A</span>
{
<span class="preprocess">#</span><span class="preprocess">pragma</span> <span class="preprocess">warning</span> <span class="preprocess">disable</span> CS8618
    <span class="reserved">public</span> <span class="reserved">string</span> X;
    <span class="reserved">public</span> <span class="reserved">string</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> Z { <span class="reserved">get</span>; <span class="reserved">init</span>; }
<span class="preprocess">#</span><span class="preprocess">pragma</span> <span class="preprocess">warning</span> <span class="preprocess">restore</span> CS8618
}
</code></pre>

選択肢3. とりあえず `default!` や `null!` を代入

<pre class="source" title="null!">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> X = <span class="reserved">null</span>!;
    <span class="reserved">public</span> <span class="reserved">string</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; } = <span class="reserved">null</span>!;
    <span class="reserved">public</span> <span class="reserved">string</span> Z { <span class="reserved">get</span>; <span class="reserved">init</span>; } = <span class="reserved">null</span>!;
}
</code></pre>

選択肢4. ノーガード。警告出っぱなしなのをあきらめる

<pre class="source" title="あきらめて警告出っぱなし">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">X</span>;
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">Y</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">Z</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</code></pre>

### 結果

C# 配信中にこの話題になり、
配信真っ最中に Twitter アンケートを作って投票してもらったり。

[https://twitter.com/ufcpp/status/1434168597060853760](https://twitter.com/ufcpp/status/1434168597060853760)

`!` でもみ消し派が35%くらいでちょっと多めですね。
まあ思ったよりは差が広がらず。
