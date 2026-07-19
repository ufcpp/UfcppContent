---
title: "【C# 12 候補】ラムダ式のデフォルト引数と params 引数"
source_url: "https://ufcpp.net/blog/2023/1/lambda-default/"
content_type: "BlogEntry"
published_at: "2023-01-11T22:10:34"
updated_at: "2023-02-09T21:04:22"
tags: []
umbraco_id: 2450
parent_id: 2449
sort_order: 0
aliases: []
---

# 【C# 12 候補】ラムダ式のデフォルト引数と params 引数

そろそろ、C# vNext 候補で上がってるものをちらほら紹介していこうかと。

今日は割かし確度高そうなものとして、ラムダ式がらみの話。
ラムダ式でもデフォルト引数と params への対応を考えているそうです。

提案ドキュメント:

* [Optional and parameter array parameters for lambdas and method groups](https://github.com/dotnet/csharplang/blob/main/proposals/lambda-method-group-defaults.md)

※追記: 後から気づきましたが、この機能は Visual Studio 17.5 Preview 2 (2022年12月中旬)の時点ですでに使えてたっぽいです。
(未確認。少なくとも Preview 3 (2023年1月中旬) では使えます。[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) preview 必要。)

## C# 10 のときの話

[C# 10 のときにラムダ式の改善](../../../../study/csharp/cheatsheet/ap_ver10.md#lambda-improvement)がいくつか入りました。
以下のように、Web アプリがシンプルに書けるようになります。

<pre class="source" title="C# 10 のラムダ式の改善">
<span class="reserved">var</span> <span class="variable">builder</span> <span class="operator">=</span> <span class="type">WebApplication</span><span class="operator">.</span><span class="static"><span class="method">CreateBuilder</span></span>(<span class="reserved">args</span>);
<span class="reserved">var</span> <span class="variable">app</span> <span class="operator">=</span> <span class="variable">builder</span><span class="operator">.</span><span class="method">Build</span>();

<span class="comment">// MapGet の引数は System.Delegate 型。</span>
<span class="comment">// Delegate に対してラムダ式が使える。</span>
<span class="comment">// 自然な型決定が働いて、この場合は Func&lt;string&gt; になる。</span>
<span class="variable">app</span><span class="operator">.</span><span class="method">MapGet</span>(<span class="string">&quot;/&quot;</span>, () <span class="operator">=&gt;</span> <span class="string">&quot;Hello World!&quot;</span>);

<span class="variable">app</span><span class="operator">.</span><span class="method">Run</span>();
</pre>

この機能の延長で、

* ラムダ式の引数にデフォルト値を与えられるように
* ラムダ式の引数を params にできるように

の2つが追加で提案されています。

## これまでのラムダ式の引数

C# 9 までの状態だと、
ラムダ式にデフォルト引数/params 引数が書けても役に立ちませんでした。

メソッドを使った例で説明すると、
以下のように、デフォルト引数/params 引数はデリゲート化する際に一切紛失します。

<pre class="source" title="デフォルト引数/params 引数はデリゲート化すると紛失">
<span class="static"><span class="method">m</span></span>();

<span class="comment">// m() と呼べるのに、 Action には代入できない。</span>
<span class="type">Action</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="static"><span class="error" title="CS0123"><span class="method">m</span></span></span>;

<span class="comment">// Action&lt;int, int[]&gt; には代入できるけど、</span>
<span class="type">Action</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>[]&gt; <span class="variable">a2</span> <span class="operator">=</span> <span class="method"><span class="static">m</span></span>;

<span class="comment">// Action&lt;int, int[]&gt; 越しには () では呼べない。</span>
<span class="variable"><span class="error" title="CS7036">a2</span></span>();

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">m</span></span>(<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>, <span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">y</span>) { }
</pre>

デリゲートに代入して使うことが前提のラムダ式では、
そもそもデフォルト引数/params 引数を書けても全く役に立たないということになります。

そんな中、[C# 10 ではラムダ式への属性指定ができるようになった](../../../../study/csharp/functional/fun_localfunctions.md#lambda-csharp10)わけですが、静的な型情報からは消えるという意味ではこの属性も同様だったりします。
ただ、属性は、静的な情報としては紛失したとしても、
リフレクションを使って属性を取る前提であれば意味があります。

<pre class="source" title="リフレクションで取る情報としては意味があり、ラムダ式に属性を付ける意義はある">
<span class="reserved">using</span> System<span class="operator">.</span>Reflection;
<span class="reserved">using</span> Microsoft<span class="operator">.</span>AspNetCore<span class="operator">.</span>Mvc;

<span class="comment">// f の型 (Func&lt;string, string&gt;) に FromBody 属性が反映されるわけではな。</span>
<span class="type">Func</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>&gt; <span class="variable">f</span> <span class="operator">=</span> ([<span class="type">FromBody</span>] <span class="reserved">string</span> <span class="variable local">name</span>) <span class="operator">=&gt;</span> <span class="string">&quot;Hello World!&quot;</span>;

<span class="comment">// リフレクションで MethodInfo から引数や戻り値を取れば、それについてる属性を調べられる。</span>
<span class="reserved">var</span> <span class="variable">p</span> <span class="operator">=</span> <span class="variable">f</span><span class="operator">.</span><span class="property">Method</span><span class="operator">.</span><span class="method">GetParameters</span>()[<span class="number">0</span>];

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">a</span> <span class="control">in</span> <span class="variable">p</span><span class="operator">.</span><span class="method">GetCustomAttributes</span>())
{
    <span class="comment">// FromBodyAttribute</span>
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span><span class="operator">.</span><span class="method">GetType</span>()<span class="operator">.</span><span class="property">Name</span>);
}

</pre>

「リフレクションで」というのであれば、
デフォルト引数と params 引数も同様のはずです。

<pre class="source" title="リフレクションでデフォルト引数/params 引数を調べる例">
<span class="reserved">using</span> System<span class="operator">.</span>Reflection;
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="type">Delegate</span> <span class="variable">f</span> <span class="operator">=</span> <span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>;

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">p</span> <span class="control">in</span> <span class="variable">f</span><span class="operator">.</span><span class="property">Method</span><span class="operator">.</span><span class="method">GetParameters</span>())
{
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">p</span><span class="operator">.</span><span class="property">Name</span>);
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">p</span><span class="operator">.</span><span class="method">GetCustomAttribute</span>&lt;<span class="type">OptionalAttribute</span>&gt;());   <span class="comment">// x のときに取れる</span>
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">p</span><span class="operator">.</span><span class="method">GetCustomAttribute</span>&lt;<span class="type">ParamArrayAttribute</span>&gt;()); <span class="comment">// y のときに取れる</span>
}

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>, <span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">y</span>) { }
}
</pre>

## ラムダ式のデフォルト引数/params 引数を認める

ということで、C# 10 の時に属性を認めたのと同じく、
ラムダ式のデフォルト引数/params 引数を認めたいという話になりました。

そこから、もう1歩進めた提案もあって、
自然な型決定で、デフォルト引数/params 引数付きのデリゲートを作るという話もあります。

<pre class="source" title="ラムダ式のデフォルト引数/params 引数">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">m</span></span>(<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>, <span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">y</span>) { }

<span class="comment">// 今までだったら Action&lt;int, int[]&gt; になってた。</span>
<span class="comment">// これを、 delegate void Anonymous(int x = 1, params int[] y) で生成したい。</span>
<span class="reserved">var</span> <span class="variable">f</span> <span class="operator">=</span> <span class="static"><span class="method">m</span></span>;

<span class="comment">// 今まででも、↓なら呼べる。</span>
<span class="variable">f</span>(<span class="number">1</span>, <span class="reserved">new</span>[] { <span class="number">2</span> });

<span class="method"><span class="static">m</span></span>(<span class="number">1</span>, <span class="number">2</span>);
<span class="method"><span class="static">m</span></span>(<span class="variable local">x</span>: <span class="number">1</span>);
<span class="method"><span class="static">m</span></span>(<span class="variable local">y</span>: <span class="number">2</span>);

<span class="comment">// ↓はこれまではダメで、C# 12 でできるようにしたい。</span>
<span class="variable">f</span>(<span class="number">1</span>, <span class="number"><span class="error" title="CS1503">2</span></span>);
<span class="variable">f</span>(<span class="error" title="CS1746">x</span>: <span class="number">1</span>);
<span class="variable">f</span>(<span class="error" title="CS1746">y</span>: <span class="number">2</span>);

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">f</span><span class="operator">.</span><span class="method">GetType</span>());
</pre>

割かし実装も進んでいるはずなので、これは近いうちにプレビューが来ると思われます。
