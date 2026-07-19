---
title: ".NET 6 Preview 7 & Visual Studio 2020 Preview 3"
source_url: "https://ufcpp.net/blog/2021/8/net6p7/"
content_type: "BlogEntry"
published_at: "2021-08-13T20:31:31"
updated_at: "2021-08-13T20:31:31"
tags: []
umbraco_id: 2355
parent_id: 2354
sort_order: 0
aliases: []
---

# .NET 6 Preview 7 & Visual Studio 2020 Preview 3

一昨日くらいに来てました。

* [Visual Studio 2022 Preview 3 now available!](https://devblogs.microsoft.com/visualstudio/visual-studio-2022-preview-3-now-available/)
* [Announcing .NET 6 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/)
* [Preview Features in .NET 6 – Generic Math](https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/)
* [String Interpolation in C# 10 and .NET 6](https://devblogs.microsoft.com/dotnet/string-interpolation-in-c-10-and-net-6/)
* [新しい C# テンプレート](https://docs.microsoft.com/ja-jp/dotnet/core/tutorials/top-level-templates)
* [ASP.NET Core updates in .NET 6 Preview 7](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-7/)
* [Announcing .NET MAUI Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-maui-preview-7/)

当日、このネタでライブ配信:

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/5m2qiJ24tqI" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

「一気に情報が来ても小一時間では話しきれない」って感じで極々一部しか話せませんでしたが。

「Visual Studio 2020 Preview 3 の方が CDN トラブルで配信が1日延期」というトラブルに見舞われ、
「SDK だけを先に .NET 6 Preview 7 に上げてしまうと、標準のテンプレートがコンパイル エラーを起こす」という事件もありましたが、1日経って問題は解消済みです。

とりあえず、ブログとしては「今回入った C# 10.0 機能」の話を書こうと思います。
ちなみに、今回の更新でほぼ C# 10.0 の全機能が入っています。
(1個だけまだなものがあるけども、「10.0 リリース時点で preview 機能として残る」判定を受けている機能なので、非 preview な 10.0 機能は全部 merge 済み。)

(全機能一覧は[トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/342) を立ててるので現状そちらを見ていただけると。)

## .NET 6 Preview 7 での C# 10.0 新機能

[Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md)で Merged into 17.0 と 17.0p3 になっているやつが今回入っています。
(17.0 になってる2つはもっと前に入ってた疑惑ちょっとあり。 Visual Studio 2020 Preview 2.1 のときかも。)

以下の6つ。

* [Improved Definite Assignment](#definite-assignment)
* [Extended property patterns](#property-pattern)
* [Interpolated string improvements](#interpolated-string)
* [File-scoped namespace](#file-scoped-namespace)
* [Parameterless struct constructors](#parameterless-ctor)
* [Caller expression attribute](#caller-expression)

あと、[Lambda improvements](#lambda) も1個前の Preview では動いていなかった機能が増えているので、合計7つ。

### <a id="definite-assignment">Improved Definite Assignment</a>

C# には元々、確実な代入ルールってのがあって、「未初期化変数から未定義な値を取り出す」みたいなことはできない仕様になっています。

<pre class="source" title="未初期化変数を触らせない">
<code><span class="reserved">int</span> x;

<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="error">x</span>); <span class="comment">// コンパイルエラー</span>

<span class="control">if</span> (<span class="reserved">int</span>.<span class="method">TryParse</span>(<span class="type">Console</span>.<span class="method">ReadLine</span>(), <span class="reserved">out</span> x))
{
    <span class="comment">// ここでは x が初期化済みな保証があるのでエラーが消える。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(x);
}
</code></pre>

これのためのフロー解析に改善の余地があることが周知の事実で長らく手つかずだったんですが、それが C# 10.0 でちょっと改善します。

これまで [`?.`](../../../../study/csharp/resource/rm_nullusage.md#null-conditional) とか [`??`](../../../../study/csharp/resource/rm_nullusage.md#null-coalesce) とか [`? : `](../../../../study/csharp/start/st_operator.md#condition) が絡むときの解析が甘くて、過剰にエラーになっていました。
それが緩和されて、例えば、以下のようなコードがコンパイルできるようになっています。

<pre class="source" title="?. が絡むときの確実な代入判定の改善例">
<code><span class="reserved">using</span> System.Diagnostics.CodeAnalysis;

m(<span class="reserved">null</span>);
m(<span class="reserved">new</span> R&lt;<span class="reserved">string</span>&gt;(<span class="reserved">null</span>));
m(<span class="reserved">new</span> R&lt;<span class="reserved">string</span>&gt;(<span class="string">"abc"</span>));

<span class="reserved">void</span> m(R&lt;<span class="reserved">string</span>&gt;? x)
{
    <span class="reserved">if</span> (x?.TryGetValue(<span class="reserved">out</span> var v) == <span class="reserved">true</span>) <span class="comment">// ここの var v の definite assignment 判定が改善された。</span>
    {
        Console.WriteLine(v.Length); <span class="comment">// 前までこの行がエラーになってた(C# 10.0 から OK に)。</span>
    }
    <span class="reserved">else</span>
    {
        Console.WriteLine(<span class="string">"null"</span>);
    }
}

<span class="reserved">record</span> <span class="reserved">class</span> <span class="type">R</span>&lt;<span class="type">T</span>&gt;(T? Value)
{
    <span class="reserved">public</span> <span class="reserved">bool</span> TryGetValue([NotNullWhen(<span class="reserved">true</span>)] <span class="reserved">out</span> T value)
    {
        <span class="reserved">if</span>(Value <span class="reserved">is</span> { } v)
        {
            value = v;
            <span class="reserved">return</span> <span class="reserved">true</span>;
        }
        <span class="reserved">else</span>
        {
            value = <span class="reserved">default</span>!;
            <span class="reserved">return</span> <span class="reserved">false</span>;
        }
    }
}
</code></pre>

### <a id="property-pattern"> Extended property patterns</a>

[プロパティ パターン](../../../../study/csharp/datatype/patterns.md#property)で、
多段のメンバーを `.` でつないでマッチングできるようになりました。

<pre class="source" title="多段プロパティ パターン">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">A</span>(<span class="reserved">new</span> <span class="type">B</span>(<span class="string">"a"</span>));

<span class="control">if</span> (x <span class="reserved">is</span> <span class="type">A</span> { <em>X.Value.Length</em>: 1 })
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"len 1"</span>);
}

<span class="reserved">record</span> <span class="type">A</span>(<span class="type">B</span> X);
<span class="reserved">record</span> <span class="type">B</span>(<span class="reserved">string</span> Value);
</code></pre>

### <a id="interpolated-string"> Interpolated string improvements</a>

[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)のパフォーマンスが大幅に向上します。

以下のようなコードがあったとして、

<pre class="source" title="文字列補間の例">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">m</span>(1, 2, 3, 4));

<span class="reserved">string</span> m(<span class="reserved">int</span> a, <span class="reserved">int</span> b, <span class="reserved">int</span> c, <span class="reserved">int</span> d) =&gt; <span class="string">$"</span>{a}<span class="string">.</span>{b}<span class="string">.</span>{c}<span class="string">.</span>{d}<span class="string">"</span>;
</code></pre>

これまでは `string.Format("{0}.{1}.{2}.{3}", new object[] { a, b, c, d })` に展開されていました。
それが、所定の条件を満たせば(普通にやってれば .NET 6 をターゲットにして C# 10.0 でコンパイルすると)、以下のようなコードに変化します。

<pre class="source" title="パフォーマンス改善結果">
<code><span class="reserved">var</span> h = <span class="reserved">new</span> System.Runtime.CompilerServices.<span class="type">DefaultInterpolatedStringHandler</span>(3, 4);
h.<span class="method">AppendFormatted</span>(a);
h.<span class="method">AppendLiteral</span>(<span class="string">"."</span>);
h.<span class="method">AppendFormatted</span>(b);
h.<span class="method">AppendLiteral</span>(<span class="string">"."</span>);
h.<span class="method">AppendFormatted</span>(c);
h.<span class="method">AppendLiteral</span>(<span class="string">"."</span>);
h.<span class="method">AppendFormatted</span>(d);
<span class="reserved">return</span> h.<span class="method">ToStringAndClear</span>();
</code></pre>

ちなみに、C# コンパイラーのレベルで頑張っていることなので再コンパイルが必要です。
これに関しては「既存のコンパイル済みプログラムを .NET 6 で動かすだけで速くなる」みたいなことはないです。

### <a id="file-scoped-namespace"> File-scoped namespace</a>

いままで:

<pre class="source" title="{} 名前空間">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;
<span class="reserved">using</span> System.Text;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">namespace</span> ConsoleApp1
{
    <span class="reserved">class</span> <span class="type">A</span>
    {
    }
}
</code></pre>

これから:

<pre class="source" title="1行名前空間">
<code><span class="reserved">namespace</span> ConsoleApp1;

<span class="reserved">class</span> <span class="type">A</span>
{
}
</code></pre>

「たかが1インデント」と言われてたやつなんですが…
まあ確かにこの1インデントが深い言語の方が、今となっては少なく。

### <a id="parameterless-ctor"> Parameterless struct constructors</a>

[さかのぼること C# 6.0 の時に、`Activator` のバグでできなかったやつ](../../2/parameterlessstructctor/index.md)、再チャレンジ(成功)。

構造体のフィールドでも非 null 保証とかがやりやすくなります。

<pre class="source" title="構造体の引数なしコンストラクターの例">
<code><span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> S { <span class="reserved">get</span>; } = <span class="string">"abc"</span>; <span class="comment">// 前まで初期化子を書けなかった</span>
}

<span class="reserved">struct</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span>[] Array { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">B</span>() =&gt; Array = <span class="reserved">new</span> <span class="reserved">int</span>[4]; <span class="comment">// 前まで B() を書けなかった</span>
}
</code></pre>

まあ、`default` からは逃げられないんですが…

<pre class="source" title="参照型の null 問題と同程度にやっかいな default 問題">
<code><span class="comment">// これは大丈夫。引数なしコンストラクターで new int[] されてる。</span>
Array4 a = <span class="reserved">new</span>();
Console.WriteLine(a[0]);

<span class="comment">// default は引数なしコンストラクターを呼ばない。</span>
a = <span class="reserved">default</span>;
Console.WriteLine(a[0]); <span class="comment">// ぬるぽ</span>

<span class="reserved">struct</span> <span class="type">Array4</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span>[] _array;
    <span class="reserved">public</span> <span class="type">Array4</span>() =&gt; _array = <span class="reserved">new</span> <span class="reserved">int</span>[4];
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; _array[index];
}
</code></pre>

### <a id="caller-expression"> Caller expression attribute</a>

[CallerInfo 系の属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)に新しい仲間が増えました。

`CallerArgumentExpression` 属性で、「引数に渡した式」を取れるようになります。

<pre class="source" title="CallerArgumentExpression の例">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="method">m</span>(2 * 3 * 4); <span class="comment">// 2 * 3 * 4 = 24</span>

<span class="reserved">var</span> (x, y, z) = (1, 2, 3);
<span class="method">m</span>(x + y + z); <span class="comment">// x + y + z = 6</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">int</span> result, [<span class="type">CallerArgumentExpression</span>(<span class="string">"result"</span>)] <span class="reserved">string</span>? expression = <span class="reserved">null</span>)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$"</span>{expression}<span class="string"> = </span>{result}<span class="string">"</span>);
}
</code></pre>

主にロギング用途になると思います。

### <a id="lambda"> Lambda improvements</a>

.NET 6 Preview 6 時点で以下のようなコードは書けていたんですが。

<pre class="source" title="Delegate にラムダ式を代入">
<code><span class="type">Delegate</span> f = <span class="reserved">int</span> (<span class="reserved">int</span> x) =&gt; x * x;
</code></pre>

Prevew 7 から以下のようなコードも書けるようになりました。

<pre class="source" title="ラムダ式の自然な型を自動決定">
<code><span class="reserved">var</span> f = <span class="reserved">int</span> (<span class="reserved">int</span> x) =&gt; x * x;
</code></pre>

この場合、`f` の型は `Func<int, int>` になります。
`System.Action` か `System.Func` が使える場合にはそれを、
使えない場合には internal なデリゲート型をコンパイラー生成して使うそうです。

デリゲートの仕様上、以下のような挙動をするのでその点には注意が必要です。

<pre class="source" title="ラムダ式の自然な型の罠の例">
<code><span class="comment">// これは target-typed 型決定で、Predicate&lt;int&gt; になる(コンパイル可)。</span>
m(x =&gt; x == 0);

<span class="comment">// 一方で、これは f の型が Func&lt;int, bool&gt; になる。</span>
<span class="reserved">var</span> f = (<span class="reserved">int</span> x) =&gt; x == 0;
m(<span class="error">f</span>); <span class="comment">// Func&lt;int, bool&gt; を Predicate&lt;int&gt; に変換でしません(コンパイル エラー)。</span>

<span class="reserved">static</span> <span class="reserved">void</span> m(Predicate&lt;<span class="reserved">int</span>&gt; f) { }
</code></pre>
