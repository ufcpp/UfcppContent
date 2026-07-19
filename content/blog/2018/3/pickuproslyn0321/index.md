---
title: "ピックアップRoslyn 3/21: Design Notes一斉アップロード祭り"
source_url: "https://ufcpp.net/blog/2018/3/pickuproslyn0321/"
content_type: "BlogEntry"
published_at: "2018-03-22T00:13:56"
updated_at: "2018-03-22T00:51:10"
tags: []
umbraco_id: 2139
parent_id: 2134
sort_order: 3
aliases: []
---

# ピックアップRoslyn 3/21: Design Notes一斉アップロード祭り

昨日なんですけども、2018年に入ってからのC# Language Design Meetingの議事録(design notes)が一斉にアップロードされました。

- [C# Language Design Notes for 2018](https://github.com/dotnet/csharplang/tree/master/meetings/2018)

読むの大変だった… 春分の日でよかった…

一通りなんとなくは目を通したんですけど、ブログ1回の内容じゃなさすぎるので、少しずつネタにしていこうかと。

## ここ数時の状況

2週間前に[Visual Studio 15.6が正式リリース](../vs15_6/index.md)されて、
その後ほどなくして15.7のプレビュー1もリリースされたわけですけども。

このプレビュー1の時点では 15.7 に C# 7.3 は入っていなかったわけですけども、
[roslynリポジトリの15.7マイルストーン](https://github.com/dotnet/roslyn/milestone/32)を見るとだいぶC# 7.3がらみの作業がマージされている状況です。
作業進捗を表す[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)のページもつい5日前に更新されて、C# 7.3のところの大半の機能が Merged になりました。

要するに、15.7向けのC# 7.3の最低限の作業が完了したんでしょうね。
あとは正式リリースに向けてバグ出し・バグ修正するフェーズに。
おそらく近々15.7プレビューにC# 7.3対応が来るのではないかと思われます。

ちなみに、[roslynのナイトリービルド](https://dotnet.myget.org/gallery/roslyn)に挙がっているVSIXやNuGetパッケージをインストールすれば、[結構ちゃんとC# 7.3が使えていました](https://gist.github.com/ufcpp/b4b505077f589235afb169652e10ee8d)。

そして、作業が落ち着いたタイミングで毎度やってくる「一斉投稿」が昨日来たと…

## 最近採用が決まった提案

とりあえず今日はこの話題のみ。
[3/19のDesign Note](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-03-19.md)で今後取り組む作業の選別をしたようで、
いろんな提案issueが新たにChampioned(将来取り組むこと自体は決定)に昇格しています。

取り組み時期はたいてい「8.X」。「8.0ですらなくさらにその後」という意味で、実際のところ「未定」と大差ないやつです。
そんな状態のものなので、具体的な文法はこれからまだだいぶ変わると思います。

### default in deconstruction

- [Champion: allow 'default' in deconstruction #1394](https://github.com/dotnet/csharplang/issues/1394)

↓みたいな書き方を認めてほしいというもの。

<pre class="source" title="">
<code><span class="reserved">int</span> x;
<span class="reserved">int</span> y;
(x, y) = <span class="reserved">default</span>; <span class="comment">// x = default; y = default; と同じ意味</span>
</code></pre>

### and, or, and not パターン

- [Champion "and, or, and not patterns" #1350](https://github.com/dotnet/csharplang/issues/1350)

↓みたいに、パターン マッチングで条件のところに and, or, not を書けるようにしたいとのこと。

<pre class="source" title="">
<code><span class="reserved">switch</span> (o)
{
    <span class="reserved">case</span> 1 <span class="reserved">or</span> 2:
    <span class="reserved">case</span> <span class="type">Point</span>(0, 0) <span class="reserved">or</span> <span class="reserved">null</span>:
    <span class="reserved">case</span> <span class="type">Point</span>(<span class="reserved">var</span> x, <span class="reserved">var</span> y) <span class="reserved">and var</span> p:
    <span class="reserved">case</span> <span class="reserved">not</span> <span class="reserved">string</span> _:
}
</code></pre>

### 型引数の部分的な型推論

- [Champion: "Partial Type Inference" #1349](https://github.com/dotnet/csharplang/issues/1349)

いくつか文法案は出ているものの、そのうちの1つで書くと、↓みたいな感じ。

<pre class="source" title="">
<code>M&lt;<span class="reserved">int</span>, &gt;(args); <span class="comment">// 2個目の型引数だけは args から推論できて、1個目は無理な時、こう書けるようにしたい</span>
</code></pre>

### 制約なしの型引数に対して is null を認めたい

- [Champion "permit `t is null` for unconstrained type parameter" #1284](https://github.com/dotnet/csharplang/issues/1284)

ちょっと説明しにくいんですけど、以下のような感じ。`where T : class`なしの`T t`に対して、`t is null`を認めたい。

<pre class="source" title="">
<code><span class="reserved">void</span> M(<span class="reserved">string</span> s)
{
    <span class="reserved">if</span> (s <span class="reserved">is</span> <span class="reserved">null</span>) { } <span class="comment">// OK。クラスだし、null チェックしたい</span>
}

<span class="reserved">void</span> M(<span class="reserved">int</span> x)
{
    <span class="reserved">if</span> (x == <span class="reserved">null</span>) { } <span class="comment">// 警告は出るけど別にエラーにはならない。常にfalse</span>
    <span class="comment">// ↑あんまり良い話ではないけど、default とかジェネリクスがなかった頃の名残っぽい</span>
}

<span class="reserved">void</span> M1&lt;<span class="type">T</span>&gt;(<span class="type">T</span> t)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>
{
    <span class="reserved">if</span> (t <span class="reserved">is</span> <span class="reserved">null</span>) { } <span class="comment">// OK。クラス制約あるし。</span>
}

<span class="reserved">void</span> M2&lt;<span class="type">T</span>&gt;(<span class="type">T</span> t)
{
    <span class="reserved">if</span> (t <span class="reserved">is</span> <span class="reserved">null</span>) { } <span class="comment">// 今は NG。</span>
    <span class="comment">// とはいえ、構造体の == null が OK なんだから別にこれも認めていいでしょ。常にfalseで</span>
}
</code></pre>

### 暗黙的なスコープのusingステートメント

- [Champion "Implicitly scoped using statement" #1174](https://github.com/dotnet/csharplang/issues/1174)

以下のような、`using`したいリソースがたくさんあるときのネスト問題への対処。

<pre class="source" title="">
<code><span class="reserved">using</span> (<span class="reserved">var</span> d = SomeDisposable())
{
    <span class="comment">// ここのネストが1段深くなるのがしんどい時がある</span>
}

<span class="comment">// 特に、多段の時。最後の1個以外は {} を省略できるとはいえ</span>
<span class="reserved">using</span> (<span class="reserved">var</span> d1 = SomeDisposable())
<span class="reserved">using</span> (<span class="reserved">var</span> d2 = SomeDisposable())
<span class="reserved">using</span> (<span class="reserved">var</span> d3 = SomeDisposable())
<span class="reserved">using</span> (<span class="reserved">var</span> d4 = SomeDisposable())
<span class="reserved">using</span> (<span class="reserved">var</span> d5 = SomeDisposable())
{
}
</code></pre>

以下のような書き方を予定。

<pre class="source" title="">
<code>{
    <span class="comment">// 変数宣言の前に using を付けることで、その変数のスコープを using のスコープにする</span>
    <span class="reserved">using</span> <span class="reserved">var</span> d1 = SomeDisposable();
    <span class="reserved">using</span> <span class="reserved">var</span> d2 = SomeDisposable();
    <span class="reserved">using</span> <span class="reserved">var</span> d3 = SomeDisposable();
    <span class="reserved">using</span> <span class="reserved">var</span> d4 = SomeDisposable();
    <span class="reserved">using</span> <span class="reserved">var</span> d5 = SomeDisposable();

    <span class="comment">// Dispose が走るのは、変数がスコープを抜ける時</span>
    <span class="comment">// = このブロックから抜けるとき</span>
}
</code></pre>

### defer ステートメント

- [Champion "defer statement" #1398](https://github.com/dotnet/csharplang/issues/1398)

Swift にあるやつ。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="reserved">defer</span>
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"関数を抜ける時に呼ばれる"</span>); <span class="comment">// 例外があっても常に</span>
    }

    <span class="type">Console</span>.WriteLine(<span class="string">"こっちの方が先に表示される"</span>);
}
</code></pre>

「一回り外側のブロックに影響する」っていう点が気持ち悪くて据え置きになっていたんですが…
前節の`using var`を認めてしまった以上、それを理由にリジェクトできなくなった感じ。

前節の`using var`を使って、以下のように代用できないこともないんですが、
これだとラムダ式のオーバーヘッド(デリゲートのヒープ確保とインライン展開の阻害)が掛かるのが嫌だそうです。

<pre class="source" title="">
<code>    <span class="reserved">using</span> <span class="reserved">var</span> d = <span class="reserved">new</span> <span class="type">ActionDisposable</span>(() =&gt;
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"関数を抜ける時に呼ばれる"</span>);
    });

    <span class="type">Console</span>.WriteLine(<span class="string">"こっちの方が先に表示される"</span>);
</code></pre>

### ユーザー定義の位置指定パターン(positional patterns)

- [Champion "User-defined Positional Patterns" #1047](https://github.com/dotnet/csharplang/issues/1047)

[パターン マッチング](https://github.com/dotnet/csharplang/blob/master/proposals/patterns.md)、C# 7.0時点では「型パターン」とか「定数パターン」とか、一部分だけが実装されました。
残りの大部分は、現状、C# 8.0で提供する予定になっています。

そんな中、さらに C# 8.0からも外れて「8.X」にしようという風に外されたのがこいつ。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Cartesian</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> Cartesian(<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// こいつを使った positional パターンは C# 8.0 で入る予定</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">double</span> x, <span class="reserved">out</span> <span class="reserved">double</span> y) =&gt; (x, y) = (X, Y);
}

<span class="reserved">class</span> <span class="type">Polar</span>
{
    <span class="comment">// こんな感じの定義を書くことで、Cartesian p を p is Polar(var r, var t) みたいなパターンに掛けることができる仕様がある</span>
    <span class="comment">// が、こいつは C# 8.0 では入らない</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> <span class="reserved">is</span>(<span class="type">Cartesian</span> p, <span class="reserved">out</span> <span class="reserved">double</span> radius, <span class="reserved">out</span> <span class="reserved">double</span> theta)
    {
        radius = <span class="type">Math</span>.Sqrt(p.X * p.X + p.Y * p.Y);
        theta = <span class="type">Math</span>.Atan2(p.Y, p.X);
    }
}
</code></pre>
