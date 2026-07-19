---
title: "タプル要素名の大文字・小文字"
source_url: "https://ufcpp.net/blog/2018/12/tupleelementcase/"
content_type: "BlogEntry"
published_at: "2018-12-02T15:55:43"
updated_at: "2018-12-02T15:58:33"
tags: []
umbraco_id: 2179
parent_id: 2177
sort_order: 1
aliases: []
---

# タプル要素名の大文字・小文字

タプルの要素名は`(int x, int y)` みたいに camelCase (先頭小文字) で書くべきか、
`(int X, int Y)` みたいに PascalCase (先頭大文字) で書くべきか、
割かし最近、この問題が再燃してたりしました。

## 背景1: C# のコーディング規約

大体のプログラミングでは、別に大文字・小文字に意味があるわけではなく、
`x` と書こうが `X` と書こうが原理的には自由です。
しかし、実践的には、「その言語の標準ライブラリ辺りに合わせる」というのが一般的かと思います。
現在の C# であれば、「[Naming Guidelines](https://docs.microsoft.com/ja-jp/dotnet/standard/design-guidelines/naming-guidelines)」辺りに合わせるのが無難でしょう。

(ちなみに、上記の規約は public な部分にしか言及していません。
割かし「private なところは自由にしたらいいんじゃないか」な文化です。
[corefx](https://github.com/dotnet/corefx)なんかは独自に[C# Coding Style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md)を持っていたりしますが、
これはあくまで corefx に pull request を出すときに気を付けるべき規約であって、
全ての C# 利用者に対して強制するものではありません。
)

今日の主題は casing/capitalization (変数などの先頭を大文字にするか小文字にするか)になります。
今日の話に関係するのは以下の2つ。

- メンバー名は大体 PascalCase
  - フィールドであっても、public なものは PascalCase
- 引数名・ローカル変数名は camelCase

<pre class="source">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Class</span>
{
    <span class="comment">// フィールドも、public なものは大文字始まり</span>
    <span class="reserved">public</span> <span class="reserved">int</span> PublicField;

    <span class="comment">// private なものについては割かし自由</span>
    <span class="comment">// ほとんどの人は小文字始まり。</span>
    <span class="comment">// 先頭に _ を付けるかどうかは好みが分かれるものの、 _ を付ける人の方がちょっと多い印象。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> _privateField;

    <span class="comment">// 引数は小文字始まり</span>
    <span class="reserved">public</span> <span class="reserved">void</span> M(<span class="reserved">int</span> parameter)
    {
        <span class="comment">// ローカル変数は小文字始まり</span>
        <span class="reserved">int</span> localVariable = parameter;
    }
}
</code></pre>

## 背景2: タプル戻り値

やりたかったことは、要するに LINQ の `Zip` メソッドにオーバーロードを足したいというもの。
今あるオーバーロードだと、以下のような書き方をよくやると思います。

<pre class="source" title="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span>[] { 1, 2, 3 };
<span class="reserved">var</span> y = <span class="reserved">new</span>[] { <span class="string">"one"</span>, <span class="string">"two"</span>, <span class="string">"three"</span> };
<span class="reserved">var</span> zip = x.Zip(y, (i, s) =&gt; (i, s));
</code></pre>

今の `Zip` は最後の引数で必ずデリゲートを1個渡す必要がありますが、
大抵の場合はこの例のように「単にタプルで返したい」で十分です。
そこで、以下のようなオーバーロードを足す流れになりました。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Enumearble</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> IEnumerable&lt;(TFirst First, TSecond Second)&gt; Zip&lt;<span class="type">TFirst</span>, <span class="type">TSecond</span>&gt;(
        <span class="reserved">this</span> IEnumerable&lt;TFirst&gt; first, IEnumerable&lt;TSecond&gt; second)
        =&gt; first.Zip(second, (x, y) =&gt; (x, y));
        <span class="comment">// 実際には効率化のためにもうちょっと複雑な実装。返す値自体はこれと同じ。</span>
}
</code></pre>

## 問題: タプル戻り値の casing

先ほどの `Zip` の新オーバーロードですが、戻り値が
`IEnumerable<(TFirst First, TSecond Second)>`です。
問題となるのは `First`、`Second` の部分。

タプルの要素名の casing をどうするべきかは結構意見が割れます。
なぜかというと、

- タプルは構造体の一種なんで、各要素は public なフィールドである
  - だとすると、一般的な規約では PascalCase (先頭大文字)
- タプルは「変数をペアリングしたもの」あるいは「引数リストの一般化」である
  - だとすると、camelCase (先頭小文字)

前者の立場(構造体のフィールド派)は以下のような感じ。

<pre class="source" title="">
<code><span class="comment">// 立場1: 構造体のフィールド派</span>
<span class="reserved">static</span> <span class="reserved">void</span> A()
{
    <span class="comment">// 以下の2行の差は「匿名か、名前付きか」だけ。</span>
    <span class="reserved">var</span> p = <span class="reserved">new</span> Point(1, 2);
    <span class="reserved">var</span> q = (1, 2);

    <span class="comment">// タプルは「名前がない型」なだけで、各要素はフィールドみたいなもの。</span>

    <span class="comment">// 大体、上記のように要素名を省略した場合、Item1, Item2 と、PacalCase な名前で値を参照することになる。</span>
    System.Console.WriteLine(q.Item1);

    <span class="comment">// 以下のように、PascalCase にしておいた方が「名前付き」の場合とそろってていい。</span>
    <span class="comment">// このタプル(無名の型)から、自動リファクタリングで Point 型みたいなものを生成することもあり得えて、その場合 PascalCase の方が自然</span>
    <span class="reserved">var</span> r = (X: 1, Y: 2);
    <span class="reserved">var</span> x1 = p.X;
    <span class="reserved">var</span> x2 = r.X;
}
</code></pre>

後者の立場(引数の一般化派)は以下の通り。

<pre class="source" title="">
<code><span class="comment">// 立場2: 引数の一般化派</span>
<span class="reserved">static</span> <span class="reserved">void</span> B()
{
    <span class="reserved">var</span> p = <span class="reserved">new</span> Point(1, 2);
    <span class="reserved">var</span> q = (1, 2);

    <span class="comment">// タプルは「引数リストだけがむき出しになっている」という状態。</span>

    <span class="comment">// 「名前付き引数」を使うと以下のような書き方になるわけで、それに合わせる方が自然。</span>
    <span class="reserved">var</span> r = <span class="reserved">new</span> Point(x: 1, y: 2);
    <span class="reserved">var</span> s = (x: 1, y: 2);

    <span class="comment">// s.x という書き方も、「変数 x, y が s によってペアリングされてる」程度の認識。</span>
    <span class="reserved">var</span> sum = s.x + s.y;
}

<span class="comment">// 引数リストとタプル戻り値がほぼ同じ書き方。</span>
<span class="comment">// タプルは引数の対となるもの。</span>
<span class="reserved">static</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) Swap(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (y, x);
</code></pre>

## どちらが有力か

C# チームは「引数の一般化派」です。
なので、タプル(C# 7.0)がリリースされてからの2年ほど、
大抵の場所で camelCase が使われてきました。

それに対して、今回、2年越しで corefx 方面に「構造体のフィールド派」が多かったみたいです。
その結果、前述の`Zip`の戻り値は PascalCase で作られて、
一度それの pull request はマージされました。

ここで、両者の齟齬が発覚。
もめ始めて、

- いったん `Zip` のやつは revert
- camelCase か PascalCase かちゃんと決めて、規約に書いとこうぜ
- 決めかねる。というか、タプルを public なところに使うこと自体よくない
  - `Zip` みたいに「2要素で確定」みたいなものならともかく、通常は、要素(フィールド)を増やすだけで破壊的変更になってしまうようなものは public には使いにくい
- 規約に書くなら「タプルは public なところに使うものじゃない」がいい

みたいな流れに。

結局、前述の `Zip` 新オーバーロードは、以下のような構造体を返す作りにおそらく変更されることになりそうです。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">ZipResult</span>&lt;<span class="type">TFirst</span>, <span class="type">TSecond</span>&gt;
{
    <span class="reserved">public</span> ZipResult(<span class="type">TFirst</span> first, <span class="type">TSecond</span> second) =&gt; (First, Second) = (first, second);
    <span class="reserved">public</span> <span class="type">TFirst</span> First { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">TSecond</span> Second { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="type">TFirst</span> first, <span class="reserved">out</span> <span class="type">TSecond</span> second) =&gt; (first, second) = (First, Second);
}
</code></pre>

規約に関しては、タプル要素名の casing についてはやっぱり両論あって決まらなさそう。
その代わり、corefx 内では「DO NOT: タプルは public API で使うな」規約を足しそうな雰囲気になっています。
(これももちろん、あくまで corefx 内の話です。corefx は「破壊的変更を起こしそうなもの」を特に強く避ける文化なのでかなり保守的。)
