---
title: "ピックアップRoslyn 6/9: record"
source_url: "https://ufcpp.net/blog/2020/6/record0609/"
content_type: "BlogEntry"
published_at: "2020-06-09T21:21:48"
updated_at: "2020-06-09T21:21:48"
tags:
  - "C# 9.0"
umbraco_id: 2299
parent_id: 2297
sort_order: 1
aliases: []
---

# ピックアップRoslyn 6/9: record

先月くらいからじわじわと、C# Language Design Meeting で Records がらみの議題が上がっています。
最近やっとまとまってきた感じがするのでまとめて紹介。

- [LDM notes for May 4](https://github.com/dotnet/csharplang/issues/3443)
- [LDM notes for May 11](https://github.com/dotnet/csharplang/issues/3470)
- [LDM Notes for May 27](https://github.com/dotnet/csharplang/issues/3526)
- [LDM notes for June 1](https://github.com/dotnet/csharplang/issues/3529)

## record 型の新設

まず、基本方針として、record は class/struct に対する修飾子ではなくて、enum とか delegate とかと同じく1種の型みたいな扱いにしたみたいです。
なので、以下のような書き方に。

<pre class="source" title="record 型">
<code><span class="reserved">record</span> <span class="type">Point</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
</code></pre>

とりあえず初期実装としては結構やることを絞るみたいで、

- record は参照型
  - 値型なものは既存の struct に手を入れるか、"record struct" を新設するかになると思うもののまだ未定
- プライマリ コンストラクターを持てるのは record だけ
  - `class Point(int X, int Y)` とか `struct Point(int X, int Y)` とかは未実装
  - 検討はされてるものの、record と同じコード生成をすべきかどうかでまだ迷ってそう
      - record の場合はプライマリ コンストラクター引数から `public int X { get; init; }` プロパティを作ることが決まってる
      - 通常の class, struct の場合はプロパティまでは作らない、キャプチャが掛からない限りフィールドにすらしないという案あり

みたいな実装のようです。

この辺りは issue のコメントでの反発も結構大きいんですが…
修飾子じゃなくて型のカテゴリーの新設な点とか、当初実装に値型版がない点とか…

## 構造体との一貫性

今はいったん未定な状態になってるんですが、
仮に、普通の class/struct にもプライマリ コンストラクターを持てて、
record のものと近いコード生成をすることになったとします
(1案としてはそういう実装も考えられます)。

じゃあ、class と record の本質的な差は何になるかと言うと、

- メンバーごとの(shallow な)比較による `Equasl`/`GetHashCode` が生成される
- メンバーごとの(shallow な)コピーによる clone メソッドが生成される

という点になります。
で、この2つ、struct の場合は標準で作られます。

<pre class="source" title="struct には自動的に Equals が作られてる">
<code><span class="reserved">using</span> System;
 
<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">int</span> Y;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> p1 = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
        <span class="reserved">var</span> p2 = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
        <span class="type">Console</span>.<span class="method">WriteLine</span>(p1.<span class="method">Equals</span>(p2)); <span class="comment">// true</span>
 
        p2.X = 3;
        <span class="type">Console</span>.<span class="method">WriteLine</span>(p1.<span class="method">Equals</span>(p2)); <span class="comment">// false</span>
    }
}
</code></pre>

ということで、コンセプト上は、「record は struct のような振る舞いを持つ参照型」みたいに考えることもできます。
なので、今の struct の挙動とあまりに違うものにはしたくないし、
今の struct が非効率な実装になっちゃってる部分は record に合わせて struct の方にも改善を入れてもいいかもとか、
そういう感じの話は出ています。

## data 修飾子

プライマリ コンストラクター前提の構文は「positional record」と呼ばれています。
引数の並びに意味があって、`new Point(1, 2)` みたいに、positional(位置指定) で初期化ができるためこう呼びます。

一方で、プロパティを元にして、`new Point { X = 1, Y = 2 }` みたいに書く想定のものを「nominal record」と呼びます。
nominal record のために、data 修飾子も用意する流れのようです。
以下のような書き方ができます。一見、data 修飾子を付けたフィールドっぽい書き方ですが、`get; init;` な public プロパティが生成されます。

<pre class="source" title="data 修飾子による「nominal record」">
<code><span class="reserved">record</span> <span class="type">Point</span>
{
    <span class="reserved">data</span> <span class="reserved">int</span> X;
    <span class="reserved">data</span> <span class="reserved">int</span> Y;
}
</code></pre>

## base 呼び出しとか、プライマリ コンストラクター引数のスコープとか

あとは細かい話。
record 型は派生もできるんですが、その場合、以下のような書き方ができます。

<pre class="source" title="record の派生">
<code><span class="reserved">record</span> <span class="type">Person</span>(<span class="reserved">string</span> FirstName, <span class="reserved">string</span> LastName)
{
    <span class="reserved">public</span> <span class="reserved">string</span> Fullname =&gt; <span class="string">$&quot;</span>{FirstName}<span class="string"> </span>{LastName}<span class="string">&quot;</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; <span class="string">$&quot;</span>{FirstName}<span class="string"> </span>{LastName}<span class="string">&quot;</span>;
}
 
<span class="reserved">record</span> <span class="type">Student</span>(<span class="reserved">string</span> FirstName, <span class="reserved">string</span> LastName, <span class="reserved">int</span> Id)
    : <span class="type">Person</span>(FirstName, LastName)
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; <span class="string">$&quot;</span>{FirstName}<span class="string"> </span>{LastName}<span class="string"> (</span>{ID}<span class="string">)&quot;</span>;
}
</code></pre>

このとき、以下のような点が検討に上がっています。

- コンストラクター引数に対して、それと同名のプロパティと、引数からプロパティへの代入コードが自動生成される
  - 代入のタイミングは base コンストラクターより前であるべきか後であるべきか
  - 今のところ「前」案優勢
- 基底クラスのコンストラクターを呼んでいる部分(この例だと `Person(FirstName, LastName)` の引数の部分のスコープはどうなるべきか
  - クラス内の全メンバーがスコープ
  - ただ、通常コンストラクターの[base アクセス](../../../../study/csharp/oop/oo_inherit.md#base-access)と同様に、インスタンス メンバーに触わろうとするとエラー
- 自動生成されるのと同名のメンバーを手書きすると、手書きの方を優先して使う
  - Equals とか
  - その手書き Equals とかが sealed だったりするとエラーにする
- `object.Equals(object)` じゃなくて `Equals(T)` は作るべきか？ → そうする予定だし、`IEquatable<T>` の実装も需要が高いことは認識してて検討の範囲内
