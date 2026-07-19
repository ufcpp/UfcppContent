---
title: "ピックアップ Roslyn 6/30"
source_url: "https://ufcpp.net/blog/2017/6/pickuproslyn0630/"
content_type: "BlogEntry"
published_at: "2017-06-30T14:13:39"
updated_at: "2017-06-30T14:13:39"
tags: []
umbraco_id: 2074
parent_id: 2070
sort_order: 1
aliases: []
---

# ピックアップ Roslyn 6/30

[csharplang](https://github.com/dotnet/csharplang)に出てる提案の整理をしたみたいで、
いくつかの提案に「Proposal Champion」ラベルが付き始めたようです。

「Proposal Champion」は、Proposal(提案が出てるだけ)とChanpion(C#チームの誰かがオーナーになって進めることが決まった段階)の間くらいのつもりっぽく。
「いつかは取り組むけども、今すぐとはいかない」くらいみたいです。

以下、いくつか紹介。

## 末尾以外で名前付き引数

- [Champion "Non-trailing named arguments" #570](https://github.com/dotnet/csharplang/issues/570)

末尾以外の場所にある引数でも名前付き引数を使いたいという話。

どうもこれはプロトタイプ実装がすでに始まってるっぽい。

<pre class="source" title="">
<code><reserved></span><span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="comment">// よく言われる話で、bool なフラグがたくさん並ぶとどれがどれかわからない</span>
    M(<span class="reserved">true</span>, <span class="reserved">true</span>, <span class="reserved">true</span>);

    <span class="comment">// 名前付き引数には「オプションな引数を省略可能にする」という意味もあるけども</span>
    M(isC: <span class="reserved">true</span>);

    <span class="comment">// どのフラグが何だったかを明記する意味でも使う</span>
    M(isA: <span class="reserved">true</span>, isB: <span class="reserved">true</span>, isC: <span class="reserved">true</span>);

    <span class="comment">// (これまで) 末尾の引数だけを名前付きにすることならできた</span>
    M(<span class="reserved">true</span>, <span class="reserved">true</span>, isC: <span class="reserved">true</span>);

    <span class="comment">// (提案) それ以外の位置でも、一部分だけ名前付きにできるようにしたい</span>
    M(isA: <span class="reserved">true</span>, <span class="reserved">true</span>, <span class="reserved">true</span>);
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">bool</span> isA = <span class="reserved">false</span>, <span class="reserved">bool</span> isB = <span class="reserved">false</span>, <span class="reserved">bool</span> isC = <span class="reserved">false</span>)
{
}
</code></pre>

## どこででも拡張メソッドを定義

- [Champion "Support extension methods everywhere" #301](https://github.com/dotnet/csharplang/issues/301)

これまでだと拡張メソッドは、トップレベル(名前空間直下)の静的クラスの中でしか定義できませんでした。

でも、「あるクラスの中でだけ使う拡張メソッドのために、外に1クラス作るのは嫌」というような話はたびたび出てくるわけで、
「静的でないクラスでも拡張メソッドを定義したい」とか、
「入れ子のクラス内で拡張メソッドを定義したい」とかいう要望はよく見ます。

ということで、制限を緩めようという話がついに検討に上がったみたいです。

## 0b, 0x の直後に _ 区切り

- [Champion "allow digit separator after 0b or 0x" #65](https://github.com/dotnet/csharplang/issues/65)

C# 7で、数値リテラルの数字と数字の間を `_` で区切れるようになりました
([数字区切り文字](../../../../study/csharp/start/stnumber.md#digit-separator))。

で、数字区切り(digit separator)の名前通り、ほんとに数字と数字の間にしか `_` を書けません。

<pre class="source" title="">
<code><reserved></span><span class="reserved">var</span> a = 1_2_3; <span class="comment">// OK</span>
<span class="reserved">var</span> b = 0b1111_1111; <span class="comment">// OK</span>
<span class="reserved">var</span> c = 0b_1111_1111; <span class="comment">// エラー。0b の直後に _ を書くのはダメ</span>
<span class="reserved">var</span> d = 0xab_cd; <span class="comment">// OK</span>
<span class="reserved">var</span> e = 0x_ab_cd; <span class="comment">// エラー。0x の直後に _ を書くのはダメ</span>
</code></pre>

元々、`0b` や `0x` の直後にも `_` を書きたいという要望は多々あったんですが、
「名前通りにしたい」、「数字区切りなのに数字でないところで区切れるようにはあんまりしたくない」みたいな雰囲気で、
C# 7ではこういう仕様になっています。

まあでも、やっぱり要望が大きく、`0b` や `0x` の直後の `_` (上記サンプルの`c`、`e`みたいな書き方)も認めようという流れになりつつあるみたいです。

## 分解にタプルは要らないのではないか

- [Surprising System.ValueTuple requirement for deconstruction #18629](https://github.com/dotnet/roslyn/issues/18629)

「提案」ってわけではないんですが。

C# 7で[分解](../../../../study/csharp/datatype/deconstruction.md)構文(deconstruction)が入りましたが、例えば以下のような場合を考えてみます。

<pre class="source" title="">
<code><reserved></span><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (X, Y);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> (x, y) = (1, 2);

        <span class="comment">// ↑現状は一度 ValueTuple を作ってから、それぞれ x, yに代入するようなコードに展開される</span>
        <span class="comment">//var t = new ValueTuple&lt;int, int&gt;(1, 2);</span>
        <span class="comment">//var x = t.Item1;</span>
        <span class="comment">//var y = t.Item2;</span>

        <span class="comment">// でも、仕様上は以下のような状態に展開する最適化を認めてるし、実際将来的にそういうコード生成する可能性がある</span>
        <span class="comment">//var x = 1;</span>
        <span class="comment">//var y = 2;</span>

        <span class="comment">// だったら、var (x, y) = (1, 2); に ValueTuple は不要なはず</span>

        <span class="reserved">var</span> (a, b) = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);

        <span class="comment">// ↑この場合は完全にタプルは要らないはず</span>
        <span class="comment">// 以下のようなコードに展開される(どこにも ValueTuple は出てこない)</span>
        <span class="comment">//var p = new Point(1, 2);</span>
        <span class="comment">//int a, b;</span>
        <span class="comment">//p.Deconstruct(out a, out b);</span>

        <span class="comment">// にもかかわらず、現状、分解を使っただけで ValueTuple (.NET 4.7以上、もしくは、System.ValueTuple パッケージの参照)が求められる</span>
    }
}
</code></pre>

現状だと、不要なはずの `ValueTuple` が求められます。

これは、将来的に以下のような構文を認めるために、分解とタプル構築の内部表現を統一したのの余波です。

<pre class="source" title="">
<code><reserved></span><span class="reserved">var</span> t = (<span class="reserved">var</span> x, <span class="reserved">var</span> y) = (1, 2);

<span class="comment">// ↑これは、↓これと同じ意味</span>
<span class="comment">// (var x, var y) = (1, 2);</span>
<span class="comment">// var t = (x, y);</span>
</code></pre>

ですが、まあ、利用者としては不要なはずのパッケージ参照が必要になるというのは気分がいいものではありません。
実際「バグ報告」扱いで報告が入ってしまっている状況。

で、まあ、修正するようです。
本当に `ValueTuple` が必要になるまでは、パッケージ参照を求めないよう修正済み。
C# 7.2辺りに入れる予定みたいです。
