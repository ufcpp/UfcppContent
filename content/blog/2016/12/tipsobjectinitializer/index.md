---
title: "小ネタ オブジェクト初期化子"
source_url: "https://ufcpp.net/blog/2016/12/tipsobjectinitializer/"
content_type: "BlogEntry"
published_at: "2016-12-05T01:10:05"
updated_at: "2016-12-05T01:10:05"
tags: []
umbraco_id: 1982
parent_id: 1969
sort_order: 4
aliases: []
---

# 小ネタ オブジェクト初期化子

今日の小ネタは、オブジェクト初期化子について、意外と知られてないらしい話。

## 問い

唐突ですが問題です。以下の3つのコードはそれぞれどういう意味でしょう。

<pre class="source" title="オブジェクト初期化子パターン1">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
{
    A = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 },
    B = <span class="reserved">new</span> <span class="type">Point</span> { X = 3, Y = 4 },
};
</code></pre>

<pre class="source" title="オブジェクト初期化子パターン2">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
{
    A = { X = 1, Y = 2 },
    B = { X = 3, Y = 4 },
};
</code></pre>

<pre class="source" title="オブジェクト初期化子パターン3">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
{
    A = <span class="reserved">new</span> { X = 1, Y = 2 },
    B = <span class="reserved">new</span> { X = 3, Y = 4 },
};
</code></pre>

ついでに、将来的に認められるようになるかもしれないパターンをもう1つ。

<pre class="source" title="オブジェクト初期化子パターン4 (将来OKになるかも)">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
{
    A = <span class="reserved">new</span>() { X = 1, Y = 2 },
    B = <span class="reserved">new</span>() { X = 3, Y = 4 },
};
</code></pre>

## 答え合わせの前に

答えを説明する前に、コード中に出ていた2つの型、`Point`、`Line`について。

まず、`Point`の方は、どのパターンであっても以下のような感じである必要があります。

<pre class="source" title="Point の例">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

構造体でもいいんですが、その場合、`Line`側に参照戻り値が必要になります。

`Line`の方は、2パターンあります。
1つは、プロパティが書き換え可能なもの。

<pre class="source" title="書き換え可能な Line の例">
<code><span class="reserved">class</span> <span class="type">Line</span>
{
    <span class="reserved">public</span> <span class="type">Point</span> A { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="type">Point</span> B { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

もう1つは、getのみのプロパティに対して、コンストラクター、もしくは、プロパティ初期化子で初期値を与えているものです。

<pre class="source" title="get のみな Line の例">
<code><span class="reserved">class</span> <span class="type">Line</span>
{
    <span class="reserved">public</span> <span class="type">Point</span> A { <span class="reserved">get</span>; } = <span class="reserved">new</span> <span class="type">Point</span>();
    <span class="reserved">public</span> <span class="type">Point</span> B { <span class="reserved">get</span>; } = <span class="reserved">new</span> <span class="type">Point</span>();
}
</code></pre>

## 答え

### パターン1

パターン1のやつは、一番シンプルというか、多くの方がこれのつもりでオブジェクト初期化子を使っているのではないかと思います。

<pre class="source" title="パターン1の答え">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Q()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
    {
        A = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 },
        B = <span class="reserved">new</span> <span class="type">Point</span> { X = 3, Y = 4 },
    };
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> A()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>();
    x.A = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
    x.B = <span class="reserved">new</span> <span class="type">Point</span> { X = 3, Y = 4 };
}
</code></pre>

展開結果を見ての通り、`x.A`や`x.B`に対する代入が発生するので、`A`, `B` は set アクセサーを持つ必要があります。

その結果、書き換え可能な方の `Line` 実装に対してならこの構文を使えますが、
getのみの方の `Line` 実装には使えません。

### パターン2

意外と知られてないのはこいつですね。

<pre class="source" title="パターン2の答え">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Q()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
    {
        A = { X = 1, Y = 2 },
        B = { X = 3, Y = 4 },
    };
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> A()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>();
    x.A.X = 1;
    x.A.Y = 2;
    x.B.X = 3;
    x.B.Y = 4;
}
</code></pre>

オブジェクト初期化子は再帰的に書けます。
その場合、この例の `x.A.X` というように、全部展開されて、そこに代入が行われます。

ここで注意が必要なのは、`x.A` の初期化は外からは行われないということです。
もしも、`Line` のコンストラクター内で `A` を初期化していなければ、当然のようにぬるぽります。

つまり、getのみの方の `Line` に対しても使える代わりに、
書き換え可能な方の `Line` 実装みたいにコンストラクター内での初期化をしていないものに対してこの書き方を使うと実行時エラーを起こします。

### パターン3

並べられると、似たようなもので全然違う結果になるので気持ち悪くなりますが、
まあ、個別によく見るとそんなに不思議なものではないと思います。

パターン3は、単に匿名型を代入しているだけ。

<pre class="source" title="パターン3の答え">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Q()
{
    <span class="comment">// 実はコンパイル エラー</span>
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
    {
        A = <span class="reserved">new</span> { X = 1, Y = 2 },
        B = <span class="reserved">new</span> { X = 3, Y = 4 },
    };
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> A()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>();
    <span class="comment">// この new { } は匿名型。</span>
    <span class="comment">// A, B は Point 型なので、匿名型だと型があってない。</span>
    <span class="comment">// つまり、コンパイル エラー: 匿名型を暗黙的に Point に変換できません</span>
    x.A = <span class="reserved">new</span> { X = 1, Y = 2 };
    x.B = <span class="reserved">new</span> { X = 3, Y = 4 };
}
</code></pre>

C#だと、コンパイル時にエラーなことがわかるんでそんなに問題はないと思うんですが。
もしも実行してみないとこの差がわからないとか言われたらちょっと殺意を覚えますね…

### パターン4

パターン4は将来の話。今現在はコンパイル エラーになります。
どういう構文が追加されそうかというと、左辺からの型推論です。

<pre class="source" title="パターン4の答え">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Q()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>
    {
        A = <span class="reserved">new</span>() { X = 1, Y = 2 },
        B = <span class="reserved">new</span>() { X = 3, Y = 4 },
    };
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> A()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Line</span>();
    <span class="comment">// new() って書き方で、左辺から型推論してくれる構文が入りそう。</span>
    <span class="comment">// この場合、A, B が Point なので、new () は new Point() の意味。</span>
    x.A = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
    x.B = <span class="reserved">new</span> <span class="type">Point</span> { X = 3, Y = 4 };
}
</code></pre>

ものすごいほしい型推論機能です。早く実装されないかな…

とはいえ、まあ、こういう、並べると気持ち悪いコードが書けますよ、と。
`{ }`と`new { }`と`new() { }`で全部意味が違うという。
