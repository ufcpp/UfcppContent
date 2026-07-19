---
title: "ピックアップ Roslyn 9/9"
source_url: "https://ufcpp.net/blog/2016/9/pickuproslyn0909/"
content_type: "BlogEntry"
published_at: "2016-09-09T04:37:57"
updated_at: "2016-09-09T04:37:57"
tags: []
umbraco_id: 1955
parent_id: 1948
sort_order: 3
aliases: []
---

# ピックアップ Roslyn 9/9

## タプルの分解の最適化

タプル構築と同時に分解(要するに多値代入というか)する場合は、タプルを作らない最適化かけたいって。

- [Optimize deconstruction of tuple literal to not construct a ValueTuple #13631](https://github.com/dotnet/roslyn/issues/13631)

要するに、例えば以下みたいなswapコード書いたとして、

<pre class="source" title="タプルと分解を使ったswap">
<code><reserved></span><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
(x, y) = (y, x);
</code></pre>

今だとこうなる。

<pre class="source" title="ValueTupleのコンストラクター呼び出しが発生">
<code><reserved></span><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
<span class="reserved">var</span> v = <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(x, y);
x = v.Item1;
y = v.Item2;
</code></pre>

これ、実のところこの時点でタプルの特別扱いが掛かってます。
分解の仕様上は、以下のようなコードになるべきところを、タプルでまでそれをやるのは無駄だってことで、`Item1`、`Item2`の直参照に。

<pre class="source" title="本来の分解構文の展開結果">
<code><reserved></span><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
<span class="reserved">var</span> v = <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(x, y);
v.Deconstruct(<span class="reserved">out</span> x, <span class="reserved">out</span> y);
</code></pre>

`Deconstruct`を最適化で消すんだったら、`new ValueTuple`の方も消していいんじゃない？という感じ。
なのでたぶん、以下のような感じのコードに展開されるのではないかと。
ほぼ、普通のswapコードに。

<pre class="source" title="最適化でnew ValueTupleを消去">
<code><reserved></span><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
<span class="reserved">var</span> tempX = x;
<span class="reserved">var</span> tempY = y;
x = tempY;
y = tempX;
</code></pre>

## Recap of async streams

[Lucian](https://github.com/ljw1004) (C#チーム非同期担当の開発者)が夏休みから帰ってきて、現状のまとめを投稿。

- [Recap of async streams](https://github.com/dotnet/roslyn/issues/261#issuecomment-244971681)

まあ、本当にまとめのみ。基本、過去にブログで取り上げた内容なので詳細割愛。

あと、[Stephen](https://github.com/stephentoub) (.NETチーム内の人。活動を見るに.NETのパフォーマンス改善系の作業をしてるっぽい)がパフォーマンスがらみの懸念を投稿。

- [261#issuecomment-245616932](https://github.com/dotnet/roslyn/issues/261#issuecomment-245616932)

現状の`IEnumerable`の問題でもあるものの、`MoveNext`と`Current`にメソッドが分かれてると、スレッド安全に作りようがなくて困る。
`IAsyncEnumerable`も同じような構造で作られると、async streamをチャネル的に(Go言語のgoroutine/channelみたいに)使いにくい/使えないので、スレッド安全にできる実装を考えてほしいとのこと。
