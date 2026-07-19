---
title: "C# 9.0 最終版"
source_url: "https://ufcpp.net/blog/2020/9/csharp9final/"
content_type: "BlogEntry"
published_at: "2020-09-13T09:41:23"
updated_at: "2020-09-13T09:41:23"
tags:
  - "C# 9.0"
umbraco_id: 2310
parent_id: 2309
sort_order: 0
aliases: []
---

# C# 9.0 最終版

いくつかライブ配信では言ってたんですが、C# 9.0 がそろそろ機能確定しそうな感じ。
11月リリースと言ってるわけなので、まあ、時期的にもこの辺りで確定していないとまずいでしょう。

ということで、先日、 What's new in C# 9.0 もドキュメント化されて docs 上に公開されました。

- [What's new in C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9?WT.mc_id=DT-MVP-4028921)

見出しに載るようなレベルでの機能の増減はもうありません。

Records とか Function pointers とか、一部の機能はまだちょっと修正が入るかと思います。
それに関しては9月9日の Design Meeting 議事録にまとまっています。
(同日の議題には C# 10.0 の話題というか、C# 10.0 に流れてしまったものの話もあり。)

- [C# Language Design Meeting for September 9th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-09.md)

ちなみに先日のライブ配信:

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/nJ1TMNfmllA" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

## Visual Studio 16.8 Preview 2

[16.8 Preview 2 が出た時点でのライブ配信](https://youtu.be/Uc04x0eZBBM)では気づいてなかったんですが、
以下の2つの機能、この時点で入っていました。
(これらが最後の C# 9.0 機能です。)

- Covariant return types
- Extension GetEnumerator

### Covariant return types

いわゆる共変戻り値。virtual メソッドの override 側で、戻り値の型を共変にできるようになりました。
要するに、以下のようなやつです。

<pre class="source" title="クラスの共変戻り値">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Base</span>
{
    <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">virtual</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Base</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Clone</span>() <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">=&gt;</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">new</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Base</span>();
}

<span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Derived</span> : <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Base</span>
{
    <span class="comment">// これの戻り値、C# 8.0 までは Base でないとダメだった</span>
    <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">override</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Derived</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Clone</span>() <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">=&gt;</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">new</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Derived</span>();
}
</code></pre>

デリゲートや、`out` 修飾付きのジェネリック型引数などではこれまでもできていたことですし、
認めてまずいことは何1つありません。
これができないことは割かしずっと問題として認識はされていて、
今になってようやく実装されたのは単に優先度の問題です。

C# 9.0 の機能のメジャーな機能の中では唯一、 .NET Runtime 側の修正が必須
(C# コンパイラーによる小手先のトリックだけでは実現不能)な機能です。
要するに、「.NET Core への移行だけで手一杯(C# 7.0 付近)」 → 「[インターフェイスのデフォルト実装](../../../../study/csharp/oop/oo_interface.md#dim)の方が優先(C# 8.0)」 → 「共変戻り値に着手(今ここ)」という感じ。

(インターフェイスのデフォルト実装同様、というかそれよりさらにだいぶ昔から、Java にはこの機能があったり。
Android での Java との相互運用のためもあって、.NET Core と Xamarin (Mono) との統合を目指している今このタイミングで共変戻り値も採用になりました。)

### Extension GetEnumerator

`GetEnumerator` が拡張メソッドであっても [`foreach` ステートメント](../../../../study/csharp/data/sp_foreach.md)で使えるようになりました。

例えば以下のような拡張メソッドを用意することで、2-[tuple](../../../../study/csharp/datatype/tuples.md) に対する `foreach` が使えます。

<pre class="source" title="2-tuple を foreach するための拡張メソッド">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
 
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="control">in</span> (1, 2))
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">i</span>);
}
 
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">TupleExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Tuple2Enumerator</span>&lt;<span class="type">T</span>&gt; <span class="method">GetEnumerator</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> (<span class="type">T</span>, <span class="type">T</span>) <span class="variable">t</span>) =&gt; <span class="reserved">new</span>(<span class="variable">t</span>);
 
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Tuple2Enumerator</span>&lt;<span class="type">T</span>&gt; : <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt;
    {
        <span class="reserved">private</span> <span class="reserved">int</span> _i;
        <span class="reserved">private</span> (<span class="type">T</span>, <span class="type">T</span>) _tuple;
 
        <span class="reserved">public</span> <span class="type">Tuple2Enumerator</span>((<span class="type">T</span>, <span class="type">T</span>) <span class="variable">tuple</span>)
        {
            _i = 0;
            _tuple = <span class="variable">tuple</span>;
        }
 
        <span class="reserved">public</span> <span class="type">T</span> Current =&gt; _i <span class="control">switch</span>
        {
            1 =&gt; _tuple.Item1,
            2 =&gt; _tuple.Item2,
            <span class="reserved">_</span> =&gt; <span class="reserved">default</span>!,
        };
 
        <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() =&gt; ++_i &lt; 3;
 
        <span class="reserved">object</span> System.Collections.<span class="type">IEnumerator</span>.Current =&gt; Current!;
        <span class="reserved">void</span> System.Collections.<span class="type">IEnumerator</span>.<span class="method">Reset</span>() =&gt; <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
        <span class="reserved">void</span> <span class="type">IDisposable</span>.<span class="method">Dispose</span>() { }
    }
}
</code></pre>

まあ、実用途があるかというとそこまで有益な使い道は思いつかないんですが…

配信ではしゃべってるんですが、
[タプルに対しては arity ごとに別拡張メソッドが必要だったり](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/14#issuecomment-687748403)、
[Range に対しては inclusive/exclusive 問題がやっぱりだいぶ混乱しそうとか](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/14#issuecomment-687749388)あり。

これは、他の新しめの文法との一貫性を取るためです。
[パターン ベースな構文一覧](../../../../study/csharp/misc/miscpatternbased.md#index)にある通り、
[クエリ式](../../../../study/csharp/data/sp3_stdquery.md)とか[分解](../../../../study/csharp/datatype/deconstruction.md#arbitrary-types)、
[await](../../../../study/csharp/async/sp5_awaitable.md#awaiter) では認めていることなので、
それと揃えたいという話が前々からありました。

(確かそれも、実用性が低めということで着手されず、最終的にはコミュニティ貢献(C# チーム外の人の実装)だったと思います。)

### null 許容参照型の改善

[#3297](https://github.com/dotnet/csharplang/issues/3297)のうち、たぶん、制約なしジェネリック型に対する `T?` は 16.8 Preview 2 で入ったはず。

<pre class="source" title="制約なし T?">
<code><span class="reserved">class</span> <span class="type">C</span>&lt;<span class="type">T</span>&gt;
<span class="comment">//where T :class // これがあれば前からOK</span>
<span class="comment">//where T :struct // これがあれば前からOK</span>
<span class="comment">// 制約なしは今回から初めてOK</span>
{
    <span class="comment">// これだとエラー。 </span>
    <span class="comment">// T? と言いつつ、C&lt;int&gt; とかを渡すと int。int? ではない</span>
    <span class="comment">//public static T? M() =&gt; null;</span>
 
    <span class="comment">// 実は nullable じゃなくて、defaultable</span>
    <span class="comment">// LINQ の FirstOrDefault 的な奴</span>
    <span class="comment">// あまりにきもいから、当初 T?? にしようという案もあった</span>
    <span class="comment">// ? になったのは、 x ?? y の ?? と区別つかなくて困ったかららしい</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span>? <span class="method">M</span>() =&gt; <span class="reserved">default</span>;
}
</code></pre>

ただこれ、少々クセはありまして。
上記コメントにもありますが、この場合の `T?` は nullable じゃなくて「defaultable」と呼んだ方がいいかもしれないようなものです。
以下のように、型引数として非 null 値型を渡すと nullable にはなりません。

<pre class="source" title="defaultable な T?">
<code><span class="reserved">string</span>? <span class="variable">x1</span> = <span class="type">C</span>&lt;<span class="reserved">string</span>?&gt;.<span class="method">M</span>();
<span class="reserved">string</span>? <span class="variable">x2</span> = <span class="type">C</span>&lt;<span class="reserved">string</span>&gt;.<span class="method">M</span>(); <span class="comment">// 順当に string?</span>
<span class="reserved">int</span>?    <span class="variable">x3</span> = <span class="type">C</span>&lt;<span class="reserved">int</span>?&gt;.<span class="method">M</span>();   <span class="comment">// 順当に int?</span>
<span class="reserved">int</span>     <span class="variable">x4</span> = <span class="type">C</span>&lt;<span class="reserved">int</span>&gt;.<span class="method">M</span>();    <span class="comment">// これの戻り値は int? にならない。default(int)、つまり、0 が返る。</span>
</code></pre>

「実は nullable じゃなくて defaultable」という挙動が気持ち悪すぎて C# 8.0 時点では見送られたし、
9.0 でも `T??` みたいな他の文法が検討されたりしたんですが、他の文法にもいろいろ問題があって、
結局単に「制約なしの `T?` は defaultable」ということになったみたいです。

## C# 9.0 最終トリアージ

[C# Language Design Meeting for September 9th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-09.md)は、C# 9.0 のタイミングでやる作業の最終判断みたいな感じになっています。

冒頭で言った通り、
「[What's new in C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9?WT.mc_id=DT-MVP-4028921)」みたいな記事が docs に並ぶ時点でもう、大きな変更はないんですけども、
いくつか細かい議題が。

とりあえず今日は 9.0 の残作業の話のみ。

「10.0 行き」みたいな感じで分類されているものも、
言い回しとしては「10.0 までの期間で再検討」みたいなふわっとしたものが多いので、
もうちょっと固まってきたら改めて。
その他、「Anytime (いつやるか不明。相当低優先度)」行きなものも省略。

### ! と .? の組み合わせがおかしい

[#3393](https://github.com/dotnet/csharplang/issues/3393)

現在、`a?.b.c!.d.e` が `(a?.b.c)!.d.e` として解釈されてしまうという問題があります。

[null 条件演算子 `?.`](../../../../study/csharp/resource/rm_nullusage.md#key-null-conditional)のショートサーキットの性質上、`!` の有無によって挙動が変わります。
[null 抑止演算子 `!`](../../../../study/csharp/resource/nullablereferencetype.md#null-forgiving)の理念としては、`!` の有無で挙動は変えたくないそうで、これは完全に想定外の仕様バグです。

ただ、C# 8.0 で1度この仕様で実装してしまったものはしょうがないので、「破壊的変更を許容してでも直すべきバグかどうか」が争点にはなっていました。
まあ、それでも「9.0 で直す」判定になりそうです。

### インターフェイスの静的メソッドが共変注釈をちゃんと見てない

[#3275](https://github.com/dotnet/csharplang/issues/3275)

これもほぼバグ。こっちは破壊的変更をするわけでもなく、単に深刻度が低くて優先度が低い状態。
Pull Request はすでに出ていて間に合うかどうかだけの問題で、
一応まだ C# 9.0 候補だそうです。

### record がらみ

[#3226](https://github.com/dotnet/csharplang/issues/3226)、
[#3213](https://github.com/dotnet/csharplang/issues/3213)、
[#3137](https://github.com/dotnet/csharplang/issues/3137) など

結構、10.0 行きになった機能はあります。

ただ、いくつかは 16.8 Preview 2 時点で実装されていないけども正式リリースまでに実装されるということになっているものがあります。

- `ToString` で単に型名だけじゃなく、`Point { X = 1, Y = 2 }` みたいな文字列化されるようになる
- `==` 演算子が生成されるようになる
  - reference equal じゃなくて、`Equals` メソッド呼び出しの「値による比較」になる
- `Equals` と `GetHashCode` (コンパイラーが自動生成してくれるものの、手書きで挙動を上書き可能)のうち、片方だけ手動上書きすると警告になる
