---
title: "【C# 12 候補】IEnumerable 向けリスト パターン"
source_url: "https://ufcpp.net/blog/2023/1/list-pattern-enumerable/"
content_type: "BlogEntry"
published_at: "2023-01-14T16:21:28"
updated_at: "2023-01-14T16:21:28"
tags: []
umbraco_id: 2451
parent_id: 2449
sort_order: 1
aliases: []
---

# 【C# 12 候補】IEnumerable 向けリスト パターン

C# vNext (12 候補)紹介シリーズ。

今日はリスト パターンがらみ。

提案ドキュメント:

* [List patterns on enumerables](https://github.com/dotnet/csharplang/blob/main/proposals/list-patterns-enumerables.md) (この当時から、リスト パターンの文法には結構変更あり)
* [直近の Language Design Meeting ノート](https://github.com/dotnet/csharplang/blob/main/meetings/2022/LDM-2022-10-19.md)

## C# 11 のときの話

[C# 11 でリスト パターン](../../../../study/csharp/cheatsheet/ap_ver11.md#list)が入りました。

`is []` みたいに、`[]` を使って配列とか `List<T>` に対するパターン マッチを行います。
ただ、C# 11 時点では、

* countable: `Length` もしくは `Count` で長さを取れる
* indexable: `[int index]` (整数引数のインデクサー)で i 番目の要素を取れる
* sliceable: `[Range range]` や `Slice(int start, int length)` でスライスを作れる

みたいな割と厳し目な条件を満たす型に対してだけリスト パターンを使えました。
以下の例ではリスト パターンとその展開結果をコメントに書いていますが、
見ての通り、 `Length` や `[]` を使ったコードと等価です。

<pre class="source" title="リスト パターンの展開例">
<span class="reserved">using</span> <span class="reserved">static</span> System<span class="operator">.</span><span class="type"><span class="static">Console</span></span>;

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">m</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>)
{
    <span class="method"><span class="static">WriteLine</span></span>(<span class="variable local">x</span> <span class="reserved">is</span> []); <span class="comment">// x.Length == 0</span>
    <span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">x</span> <span class="reserved">is</span> [<span class="number">1</span>]); <span class="comment">// x.Length == 1 &amp;&amp; x[0] == 1</span>
    <span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">x</span> <span class="reserved">is</span> [<span class="number">1</span>, ..]); <span class="comment">// x.Length &gt;= 1 &amp;&amp; x[0] == 1</span>
    <span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">x</span> <span class="reserved">is</span> [<span class="reserved">_</span>, .. <span class="reserved">var</span> y]); <span class="comment">// y = x[1..]</span>
    <span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">x</span> <span class="reserved">is</span> [<span class="reserved">_</span>, .. <span class="reserved">var</span> z, <span class="reserved">_</span>, <span class="reserved">_</span>]); <span class="comment">// y = x[1..^2]</span>
}
</pre>

## 対 IEnumerable

提案当初(コミュニティ提案だったりします)では、
リスト パターンは `IEnumerable` に対しても使える提案がありました。
別にリジェクトされたわけでもないんですが、countable, indexable, sliceable に対するものと比べると課題が多いので「後回し」にされています。

まあ、元々提案にあったものなので、引き続き検討しようかという感じで C# vNext 候補です。
元々の提案では、何らかのヘルパー クラスを間に挟んで、
`x is [0, 1, ..]` みたいなコードを以下のような感じで展開することを考えています。

<pre class="source" title="IEnumerable に対するリスト パターンの展開例">
<span class="reserved">var</span> <span class="variable">helper</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">ListPatternHelper</span>(<span class="variable">x</span>, <span class="number">2</span>, <span class="number">0</span>);

<span class="variable">helper</span><span class="operator">.</span><span class="method">TryGetStartElement</span>(<span class="variable local">index</span>: <span class="number">0</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">element0</span>) <span class="operator">&amp;&amp;</span> <span class="variable">element0</span> <span class="reserved">is</span> <span class="number">0</span> <span class="operator">&amp;&amp;</span>
<span class="variable">helper</span><span class="operator">.</span><span class="method">TryGetStartElement</span>(<span class="number">1</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">element1</span>) <span class="operator">&amp;&amp;</span> <span class="variable">element1</span> <span class="reserved">is</span> <span class="number">1</span>
</pre>

## 課題

リスト パターンを `IEnumerable` でも使えるようにしようとすると、スライスが絡むときが難しそうです。
例えば、 `x is [0, 1]` だとそんなに問題はなくて、
「最初の2個分 `MoveNext` → `Current` するだけ」になるんですが。
一方で `x is [.., 1]` だと、 LINQ でいうところの `Last` になるわけで、
LINQ でもそうなんですけども、無限シーケンスで困ります。

<pre class="source" title="">
<span class="comment">// 普通、無限シーケンスは Take(有限の値) とかで一部分だけ取り出して使う。</span>
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="method"><span class="static">m</span></span>()<span class="operator">.</span><span class="method">Take</span>(<span class="number">100</span>);

<span class="comment">// 無限なものの Last があるわけなく、永久ループになる。まずい。</span>
<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="static"><span class="method">m</span></span>()<span class="operator">.</span><span class="method">Last</span>();

<span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method"><span class="static">m</span></span>()
{
    <span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>;
    <span class="control">while</span> (<span class="reserved">true</span>) <span class="control">yield</span> <span class="control">return</span> <span class="variable">i</span><span class="operator">++</span>; <span class="comment">// whiel(true) なので永久に途切れない</span>
}
</pre>

この辺りを中心に、[LDM](https://github.com/dotnet/csharplang/blob/main/meetings/2022/LDM-2022-10-19.md)で検討:

### スライスの後ろにパターンがある場合

前述のように `x is [.., 1, 2]` とかになっているパターンをどうするか:

* 展開結果がちょっと複雑
    * → 手書きするよりはコンパイラーに頑張ってもらう方がマシ
* 無限シーケンスとかみたいに footgun (勢い余って自分の足を打ち抜いちゃいそうな道具)になりそう
    * → 元から。LINQ もそうだし、なんだったら .NET Framework 1.0 の頃からこの手の footgun はある
* LINQ よりもパフォーマンスが落ちる可能性
    * LINQ to Object の場合、内部的に `is IList` 分岐とかで「indexable ならそれ前提のコードを使う」みたいな最適化をしてる
    * この問題は大きいと思っている。「パターンを使うとパフォーマンスが悪くなる」という状況は避けたい
    * `IEnumerable` 向けリスト パターンでもその手の最適化がかかるようにしないといけない

まあとりあえず、最初の実装としては `x is [1, 2, ..]` みたいな「スライスの前」だけを認めて、
`x is [.., 1, 2]` みたいな「スライスの後」は後々改めて検討するのでもいいかも見たいな雰囲気です。

「スライスの後」の方は、
結局は、「最初に紹介した `ListPatternHelper` みたいなヘルパー クラスの中で、LINQ と比べてパフォーマンス悪化させないような最適化がかかってほしい」ということになるんですが、「BCL チームと連携して作る」とのこと。

## .. 部分をキャプチャ

リスト パターンでは、`x is [1, 2, ..var y]` みたいに書いて、`y = x[2..]` みたいなスライスをキャプチャすることもできます。
とうことで、「スライスをキャプチャ」は、「[配列とかリストのスライス](../../../../study/csharp/data/dataranges.md#indexer)」と密接に紐づいています。

で、このスライスなんですが、推奨としては「`Slice` メソッドの戻り値は元の型と同じにするべき」ということになっています。
配列であれば `x[i..j]` の結果も配列、
`List<T>` であれば `x[i..j]` の結果も `List<T>`、
`Span<T>` であれば `x[i..j]` の結果も `Span<T>` ということです。

(その結果、配列や `List<T>` に対して `[..]` を使うとコピーが発生してパフォーマンスはそんなによくないですが、「型が同じ」の方が驚きは少ないだろう、パフォーマンスが必要なら `Span` を使えばいいだろうということになっています。)

ところが、`IEnumerable` の場合、スライスを具体的に何の型にすればいいのかが決まらないので困ると。

これも結局、「よいヘルパー クラスができてから改めて考える」みたいな空気感で終わっています。
