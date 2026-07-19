---
title: "C# 8.0 パターン マッチング"
source_url: "https://ufcpp.net/blog/2018/12/cs8patterns/"
content_type: "BlogEntry"
published_at: "2018-12-10T10:00:09"
updated_at: "2018-12-10T10:05:09"
tags: []
umbraco_id: 2191
parent_id: 2177
sort_order: 9
aliases: []
---

# C# 8.0 パターン マッチング

今日はパターン マッチングの話。
昨日の[`switch`式](../cs8switchexpr/index.md)に引き続き、
真っ先に実装されてそうなものなのに Preview 1 には入っていなかったやつ。
というか、`switch`式自体、このパターン マッチングの一部として提案されているものです。

## パターン マッチング “完全版”

パターン マッチングは、元々は C# 7.0 で検討されていたものの、
結局、一部分だけが C# 7.0 に入り、複雑なものは C# 8.0 に回りました。

| パターン | C# のバージョン | 概要 | 例 |
| --- | --- | --- | ------------- |
| discard | C# 7.0 | 何にでもマッチ・無視 | `_` |
| var | C# 7.0 | 何にでもマッチ・引数で受け取り | `var x` |
| 定数パターン | C# 7.0 | 定数との比較 | `null`、`1` |
| 型パターン | C# 7.0 | 型の判定 | `int i`、`string s` |
| 位置パターン | C# 8.0 | [分解](../../../../study/csharp/datatype/deconstruction.md)と同じ要領で、`Deconstruct`を元に(引数の位置に応じて)再帰的にマッチングする | `(1, var i, _)` |
| プロパティ パターン | C# 8.0 | プロパティに対して再帰的にマッチングする | `{ A: 1, B: var i }` |

要するに、再帰的に使える下の2つが C# 8.0 での新機能になります。

まあ、C# 7.0 のやつだと「“パターン”って言うほど複雑なマッチングしてない」感がありました。
(実際、なので C# 7.0 リリース当時は「型スイッチ」みたいな呼び方もされていました。
結局、まあ、C# 8.0 を見越してあくまで「パターン マッチングのうち、型パターンだけは先にリリース」みたいな感じでアナウンスされています。)

例えば以下のような感じ。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (X, Y);
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> M(<span class="reserved">object</span> obj)
        =&gt; obj <span class="reserved">switch</span>
        {
            0 =&gt; 1,
            <span class="reserved">int</span> i =&gt; 2,
            <span class="type">Point</span>(1, _) =&gt; 4, <span class="comment">// new!</span>
            <span class="type">Point</span> { X: 2, Y: <span class="reserved">var</span> y } =&gt; y, <span class="comment">// new!</span>
            _ =&gt; 0
        };
}
</code></pre>

## 何に使うかと言われると

まあ、再帰パターンなわけで、再帰的なデータ構造相手なら便利そうではあります。
(再起データ構造自体どのくらいの頻度で使われるかという話を置いておけば…)

例えば、`x + 1` みたいな式を `Add(Variable(x), Const(1))` みたいなツリー構造で表す奴とか。
そのツリーに対して、`x + 0` は `x` と等しいとか、`x * 1` は `x` と等しいとかその手の簡単化をするやつは、以下のように書けるようになります。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Node</span> Simplify(<span class="reserved">this</span> <span class="type">Node</span> n)
    =&gt; n <span class="reserved">switch</span>
        {
            <span class="type">Add</span>(<span class="reserved">var</span> l, <span class="reserved">var</span> r) =&gt; (l.Simplify(), r.Simplify()) <span class="reserved">switch</span>
            {
                (<span class="type">Const</span>(0), <span class="reserved">var</span> r1) =&gt; r1,
                (<span class="reserved">var</span> l1, <span class="type">Const</span>(0)) =&gt; l1,
                (<span class="reserved">var</span> l1, <span class="reserved">var</span> r1) =&gt; <span class="reserved">new</span> <span class="type">Add</span>(l1, r1)
            },
            <span class="type">Mul</span>(<span class="reserved">var</span> l, <span class="reserved">var</span> r) =&gt; (l.Simplify(), r.Simplify()) <span class="reserved">switch</span>
            {
                (<span class="type">Const</span>(0) c, _) =&gt; c,
                (_, <span class="type">Const</span>(0) c) =&gt; c,
                (<span class="type">Const</span>(1), <span class="reserved">var</span> r1) =&gt; r1,
                (<span class="reserved">var</span> l1, <span class="type">Const</span>(1)) =&gt; l1,
                (<span class="reserved">var</span> l1, <span class="reserved">var</span> r1) =&gt; <span class="reserved">new</span> <span class="type">Mul</span>(l1, r1)
            },
            _ =&gt; n
        };
</code></pre>

([コード全体はGist上に](https://gist.github.com/ufcpp/37702d99a7c0148b3b0d0f8b82e46414))

ちなみに、単に「複数の値を同時にマッチング」という使い方もできます。
以下のように、`(x, y) switch { }` でスイッチ。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> Compare(<span class="reserved">int</span>? x, <span class="reserved">int</span>? y)
    =&gt; (x, y) <span class="reserved">switch</span>
    {
        (<span class="reserved">null</span>, <span class="reserved">null</span>) =&gt; 0,
        (<span class="reserved">null</span>, _) =&gt; -1,
        (_, <span class="reserved">null</span>) =&gt; 1,
        ({} ix, {} iy) =&gt; ix.CompareTo(iy)
    };
</code></pre>

要するに、このコードは「タプルに対する位置パターン」なんですが、
それが「`x`, `y` に対して多値マッチング」っぽく使えます。

(あと、`{}`は後述しますが、「プロパティ パターン」(の、中身空っぽ)です。)

ちなみに、`switch` ステートメントでも以下のような書き方ができます。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> Compare(<span class="reserved">int</span>? x, <span class="reserved">int</span>? y)
{
    <span class="reserved">switch</span> (x, y)
    {
        <span class="reserved">case</span> (<span class="reserved">null</span>, <span class="reserved">null</span>): <span class="reserved">return</span> 0;
        <span class="reserved">case</span> (<span class="reserved">null</span>, _): <span class="reserved">return</span> -1;
        <span class="reserved">case</span> (_, <span class="reserved">null</span>): <span class="reserved">return</span> 1;
        <span class="reserved">case</span> ({ } ix, { } iy): <span class="reserved">return</span> ix.CompareTo(iy);
        }
    }
}
</code></pre>

先ほど書いた通り、これは実際には「タプルに対する位置パターン」なんですが、
だったら、本来は `switch ((x, y))` という書き方(内側の`()`がタプル構築、外側の`()`が`switch`ステートメントのもの)をする必要があります。
これも C# 8.0 の新機能で、「タプルだったら`()`を1個省略して、多値 `switch` っぽく書けるようにした」というものです。

## 非 null マッチング

ちなみに、プロパティ パターンの `{}` は、
プロパティを調べる前に本体が null ではないことをチェックします。
中身が空っぽのプロパティ パターンでも null チェックだけは挿入されるので、
`x is {}`で、「`x`はnullではない」の意味で使えます。

C# 7.0 までのパターンだと、null チェックを楽に書く手段がなかったです。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">LongLongNamedStruct</span> { }
 
<span class="reserved">void</span> M1(<span class="type">LongLongNamedStruct</span>? x)
{
    <span class="comment">// こういう書き方だと null チェックになる。</span>
    <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="type">LongLongNamedStruct</span> nonNull)
    {
        <span class="comment">// obj が null じゃない時だけここが実行される。</span>
        <span class="comment">// でも、x の型が既知なのに、長いクラス名をわざわざ書くのはしんどい…</span>
    }
}
 
<span class="reserved">void</span> M2(<span class="type">LongLongNamedStruct</span>? x)
{
    <span class="comment">// が、var パターンは null にもマッチしちゃう。</span>
    <span class="comment">// (var は「何にでもマッチ」。null でも true になっちゃう。)</span>
    <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="reserved">var</span> nullable)
    {
        <span class="comment">// obj が null でもここが実行される。</span>
    }
}
</code></pre>

もちろん、単に null チェックだけなら `!(x is null)` とか `x.HasValue` でいいんですけども、
値を使いたければその後ろで `var nonNull = x.GetValueOrDefault();` を書かないと行けないのがしんどく。

そこで、プロパティ パターンが使えます。
以下のように、「空のプロパティ パターン」を書けば、「非 null のときだけ」判定ができます。

<pre class="source" title="">
<code><span class="reserved">void</span> M3(<span class="type">LongLongNamedStruct</span>? x)
{
    <span class="comment">// (C# 8.0) プロパティ パターンであれば、null チェックを含む。</span>
    <span class="reserved">if</span> (x <span class="reserved">is</span> {} nonNull)
    {
        <span class="comment">// obj が null じゃない時だけここが実行される。</span>
    }
}
</code></pre>

ちょっと「知ってないと使えない仕様」ですけども…
覚えておくと便利です。

## 対称性

C# 7.0 の時、タプルとか分解の構文を決めるにあたって、C# チームは結構「対称性」を気にしていました。

まず、タプルは「引数と対になるもの」として考えられています。

<pre class="source" title="">
<code><span class="comment">// タプル型宣言と引数宣言は同じような見た目。</span>
(<span class="reserved">int</span> x, <span class="reserved">int</span> y) tup0;
<span class="reserved">int</span> method(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; x + y;
 
<span class="comment">// タプル構築はメソッド呼び出しみたいな書き方になる。</span>
<span class="comment">// 位置指定:</span>
<span class="reserved">var</span> tup1 = (1, 2);
<span class="reserved">var</span> ret1 = method(1, 2);
 
<span class="comment">// 名前指定:</span>
<span class="reserved">var</span> tup2 = (x: 1, y: 2);
<span class="reserved">var</span> ret2 = method(x: 1, y: 2);
 
<span class="comment">// タプル戻り値は、引数と同じような書き方に。</span>
(<span class="reserved">int</span> x, <span class="reserved">int</span> y) swap(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (y, x);
</code></pre>

また、分解は「コンストラクターと対になるもの」です。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
 
    <span class="comment">// 複数の値を組み合わせて1つの型にまとめるのが構築(construct)。</span>
    <span class="reserved">public</span> Point(<span class="reserved">int</span> x = 0, <span class="reserved">int</span> y = 0) =&gt; (X, Y) = (x, y);
 
    <span class="comment">// 1つにまとまっている値をバラバラに戻すのが分解(deconstruct)。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (X, Y);
}
</code></pre>

<pre class="source" title="">
<code><span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2); <span class="comment">// contruct</span>
<span class="reserved">var</span> (x, y) = p;          <span class="comment">// deconstruct</span>
</code></pre>

C# 8.0 の再帰パターンもこの話の延長にあります。

<pre class="source" title="">
<code><span class="comment">// 位置指定で構築できるんなら、位置指定でマッチングできるべき</span>
<span class="reserved">var</span> p1 = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
<span class="reserved">var</span> r1 = p1 <span class="reserved">is</span> (1, 2);
 
<span class="comment">// 名前指定で構築できるんなら、名前指定でマッチングできるべき</span>
<span class="reserved">var</span> p2 = <span class="reserved">new</span> <span class="type">Point</span>(x: 1, y: 2);
<span class="reserved">var</span> r2 = p2 <span class="reserved">is</span> (x: 1, y: 2);
 
<span class="comment">// 初期化子でプロパティ指定できるんなら、プロパティ指定でマッチングできるべき</span>
<span class="reserved">var</span> p3 = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
<span class="reserved">var</span> r3 = p3 <span class="reserved">is</span> { X: 1, Y: 2 };
 
<span class="comment">// 混在構築できるんなら、混在マッチングできるべき</span>
<span class="reserved">var</span> p4 = <span class="reserved">new</span> <span class="type">Point</span>(x: 1) { Y = 2 };
<span class="reserved">var</span> r4 = p4 <span class="reserved">is</span> (1, _) { Y: 2 };
</code></pre>

## 最近の変更

冒頭で言ったように、再帰パターンも元々は C# 7.0 で考えられていました。
それに、C# 8.0 機能の中では相当早い段階から実装済みで、
確か今年の初めくらいにはすでに実装がありました。

(なので、[sharplab.io](https://sharplab.io/#v2:EYLgZgpghgLgrgJwgZwLRIMaOQSwG4SoAOsMECAdsgD4ACADAAS0CMA3ALABQ3tAzMxYA2ZgCZGAYW4Bvbo3nMBrETgoxJUADZZNsCAAoYACxzJGAOQD2AEwiMKAGkar1ADwCUchd4C8APntGZAB3HBgMIy9veVkuaPjGADUoBEY8Rn9GVwcohPkJSyp1DAyAjAA6ZM04CBy4vIUAQWtrfTwUxk0ndtSEd1LO8oktHT19D0YAakYEIZG4XTJx9zqGhQBZBbaOrrSOvoHNOe0FsYmAKhnj0aWPVbXGAH0B4wRLYPsID4BJCnbNHDWADyRHIsBwhQAoq4MBAiDAIRR9J56vEAL6cHioxSCERWWyMADKOAAtkQAWAAJ6GExmfF2CgovKZChBULhSLY7yxB6MZqtHqdbr7fqZfRHYlkinUlZXSXknBU5H9EJhCK5PI83kKfQFIr6eiywUIFiigIm+7axjbVKaFhOPXIGAG9xmzr2jUPG3u4W9U0DChfPktcX2mamz3oy0NTaab27Y1u8XleXS5FOWapxUylXs9VchJaq26wpOl2MDBOR5uyuRhr6R4O0vOw0VmvR3kl/Wm33ht0Wut5eNhx3O01uu0dr2Cyd7P1uwMfWOhjMRgtRwcKZ4susY7i5ZTMAAsjHWUFUyNyReigtcGQsNgg5QAGpi8qwAJzjRiXO/TU1vgkn76CBLA/lkUyMEwv79JcpopqSCpKq6gHxMBIF/owoiwdamF8K61ynEs46oYwaLcORWL8IwUDAE6CBQBg6i0OI9IyAeSjCDM0DWIUmiUg+BLPvei5JCkyKkdRh6IQCGBhIwliggxMCWKk9L6C4ezVBAC5BqO2zae4kmcXij4KUpsCqZM+j0p0EBgDATi2QgOAAOZGDAukfPy4r2Y5MxuR5Rkcbigl2IpYIqQg5w2WZmh+U5Zkue5nkBkGy7xQ5GaBZ5mKUbwSjiMkqQgGFjDSDilgEAgLkEqwTAACqWISMAuRQrnIgMABErhdWwZH7lwUniKOjClWxXDXtRmlVDU5WMK5EAwP1lHeNR+maf8NT9BVs12D4Wk1CtIW0CeAAiEAYM2CBwEx+iWHA6ibVo20DFt+1idpxkKdVtV2PVjBNS1bUdW6e3lEDrWqKDeWDcNwbWGNYXsdi1G2QAMn582LctA2owItkAEo5djS3HfjCOxQSmX+c5OVJpjWWMMTKWitaNPZaz32nYwF1XUUN13Q96i2RzCmPWVyVBQMvlM1LqUHfojP+SzQXfVV5B/YIjXNVD7WdZkAAkXX6NIysQ7rIPImikHSKrMAW8D0PW+4fUUXDhWngsSMTVNBNmcrpO46tCho2Z9tB+Ta0CMuosJZL9My8rTj22zsv+fLwUUzzfPXbdzrC2VYuF3TrMy2Lmf3kr8ep+rv2Av9LA607+tusbZt+Y7eugzblx2zlXdW+4aJu1waJAA==)で割かし安定して試せたりします。
Visual Studio 2019 Preview 1 で実装されていなくても割と細かくブログを書けるのはこれのおかげ。)

個人的には「C# 7.4 があってもよかったんじゃ… 再帰パターンだけのリリース」とかもちょっと思ったり。
C# チーム的には「マイナー リリースで出すほど小さい機能ではない」とのことで、C# 8.0 での追加になります。

ということで、大半の機能はだいぶ前から試せる状態にあったんですが、
割と最近にもいくつか細かい追加・変更がありました。

- `switch (x, y)` の「`()` を1段省略」は割と最近の採用
- プロパティ パターンの構文は `{ X is pattern }` か `{ X = pattern }` か `{ X: pattern }` のどれがいいか
  - `:` になったのは割と最近
- `var (x)` みたいな、「1引数 `Deconstruct`」
  - キャストや、「`(1)`は単なる`1`と同じ意味」という既存の構文との弁別の問題があるものの、`var`の後ろなら弁別できるので認めようということに最近なった
- 同じく、「0引数`Desonctruct`」に対する`var ()`パターンも
