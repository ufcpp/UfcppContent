---
title: "ピックアップRoslyn 5/20: 5月の Design Notes"
source_url: "https://ufcpp.net/blog/2018/5/pickuproslyn0520/"
content_type: "BlogEntry"
published_at: "2018-05-20T16:11:02"
updated_at: "2018-05-20T16:11:02"
tags: []
umbraco_id: 2154
parent_id: 2150
sort_order: 3
aliases: []
---

# ピックアップRoslyn 5/20: 5月の Design Notes

5月の Language Design Notes が2件ほど追加されました。

- [C# Language Design Notes for May 2, 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-05-02.md)
- [C# Language Design Notes for May 14, 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-05-14.md)

さらっと抜粋。

## switch 式

[先週書いた](../cs80preview/index.md)通り、現状のプレビュー版では、以下のような文法で「式」としてswitchを書けます。

<pre class="source" title="">
<code><span class="reserved">var</span> s = x <span class="reserved">switch</span>
{
    1 =&gt; <span class="string">"one"</span>,
    2 =&gt; <span class="string">"two"</span>,
    3 =&gt; <span class="string">"three"</span>,
    _ =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">IndexOutOfRangeException</span>()
};
</code></pre>

今のところ、`=>` で実装されているんですが、これに関して:

- `:`、`->`、`~>` なんかも考えはした
- まだ決めかねてる。今の実装はとりあえず `=>` になってるけども
- `=>` はこれはこれで、ラムダ式と混ざったりデメリットもありそう

など。

## ranges

これも[先週書いた](../cs80preview/index.md)通り。`1..^1`みたいな書き方で「`1` ～ `Length - 1` の直前まで」(= 最初と最後、1要素ずつ削ったもの)を表す。

アプローチとしては大筋はよさそう。完全に認められたわけでもないけども、「害悪」とまでは思われてない。`^`って文字を使うのはC#にとってはちょっと馴れないけども、しょせん「馴れてない」程度の話。

## nullable reference types

参照型に対して `T` なら null が来ない、`T?` なら null があり得る、だけだと不十分で、
いくつか、属性を使ったアノテーションを実装し始めたみたい。

`string.IsNullOrEmpty`みたいなメソッドでは、「戻り値が false だったらそれ以降、引数は null ではない」とかいう挙動なわけですが、それ用に`NotNullWhenFalse`属性を導入。




<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">static</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">bool</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">IsNullOrEmpty</span>([<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">NotNullWhenFalse</span>] <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">string</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">s</span>) { }
</code></pre>

また、以下のように `EnsuresNotNull` 属性で、「このメソッドを呼んだら、引数は null ではないことを確認済み」

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">static</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">void</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">AssertNotNull</span>&lt;<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">T</span>&gt;([<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">EnsuresNotNull</span>] <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">T</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">t</span>) <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">where</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">T</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">class</span> { }
</code></pre>

`AssertNotNull` だと「null だったらそこで例外」みたいな挙動だけども、別に例外でなくても、
「メソッド内部で null でない値に上書き」とかもあり得る。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">static</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">void</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">EnsureNotNull</span>([<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">EnsuresNotNull</span>] <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">ref</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">string</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">s</span>) { <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">if</span> (<span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">s</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">is</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">null</span>) <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">s</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">=</span> <span class="pl-s" style="box-sizing: border-box; color: rgb(3, 47, 98);"><span class="pl-pds" style="box-sizing: border-box; color: rgb(3, 47, 98);">"</span><span class="pl-pds" style="box-sizing: border-box; color: rgb(3, 47, 98);">"</span></span>; }
</code></pre>

`==` 以外での null チェックもできるように、「`Equals` の類のメソッドです。null 解析に使ってください」を表す `NullableEquals` 属性も。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Object</span>
{
    [<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">NullableEquals</span>] <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">static</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">bool</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">ReferenceEquals</span>(<span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">x</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">y</span>) { }
    [<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">NullableEquals</span>] <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">static</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">bool</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Equals</span>(<span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">x</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">y</span>) { }
    [<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">NullableEquals</span>] <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">virtual</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">bool</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">Equals</span>(<span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">other</span>) { }
    [<span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">NullableEquals</span>] <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">static</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">bool</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">operator</span><span class="pl-en" style="box-sizing: border-box; color: rgb(111, 66, 193);">==</span>(<span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">x</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(215, 58, 73);">object</span>? <span class="pl-smi" style="box-sizing: border-box; color: rgb(36, 41, 46);">y</span>) { }
}
</code></pre>

既存のコードでこの属性が付いてない場合に備えて、
「外からアノテーションを足す」みたいな機能も欲しい。
(属性だと、そのメソッドを書いた人にしか付けれない。)

## インターフェイスのデフォルト メソッド

インターフェイスの中に実装を置くことに対して、やっぱりいくらかの人が反対してる。
多くの人は、インターフェイスを自分で書いて自分で使ってる。
この状況だと、「インターフェイスにメンバーを追加したら破壊的変更」と言うのが問題になりにくい。

でも、public な API を作っている人にとってはデフォルト メソッド(= インターフェイスに後からメソッドを追加しても破壊的変更にならなくできる)は非常に重要。
また、Swift や Java との相互運用(主に Xamarin 用)には必要。

なので重要な機能だと考える。

## discriminated unions

### switch の網羅性

以下のような感じで、「Animal の派生クラスは Dog と Cat しか認めない」みたいな状態を作ったとして

<pre class="source" title="">
<code><span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">Animal</span>
{
    <span class="reserved">private</span> Animal() { }
    <span class="reserved">sealed</span> <span class="reserved">class</span> <span class="type">Dog</span> : <span class="type">Animal</span> { }
    <span class="reserved">sealed</span> <span class="reserved">class</span> <span class="type">Cat</span> : <span class="type">Animal</span> { }
}
</code></pre>

switch の網羅性(考えうるケースを網羅してたら `_` や `default` を警告なしで省略できるようにしたい)はどう考えるべきか。
以下のコードだとダメ。

<pre class="source" title="">
<code><span class="reserved">int</span> M(<span class="type">Animal</span> a)
{
    <span class="reserved">return</span> a <span class="reserved">switch</span>
    {
        <span class="type">Cat</span> c =&gt; 1,
        <span class="type">Dog</span> d =&gt; 2
    }
}
<span class="reserved">int</span> M(<span class="type">Box</span>&lt;<span class="type">Animal</span>&gt; b)
{
    <span class="reserved">return</span> b <span class="reserved">switch</span>
    {
        <span class="type">Box</span> (<span class="type">Cat</span> c) =&gt; 1,
        <span class="type">Box</span> (<span class="type">Dog</span> d) =&gt; 2
    }
}
</code></pre>

実際には以下のように書かないと網羅的じゃない。

<pre class="source" title="">
<code><span class="reserved">int</span> M(<span class="type">Animal</span> a)
{
    <span class="reserved">return</span> a <span class="reserved">switch</span>
    {
        <span class="type">Cat</span> c =&gt; 1,
        <span class="type">Dog</span> d =&gt; 2,
        <span class="reserved">null</span> =&gt; 3
    }
}
<span class="reserved">int</span> M(<span class="type">Box</span>&lt;<span class="type">Animal</span>&gt; b)
{
    <span class="reserved">return</span> b <span class="reserved">switch</span>
    {
        <span class="type">Box</span> (<span class="type">Cat</span> c) =&gt; 1,
        <span class="type">Box</span> (<span class="type">Dog</span> d) =&gt; 2,
        <span class="type">Box</span> (<span class="reserved">null</span>) =&gt; 3
        <span class="reserved">null</span> =&gt; 3
    }
}
</code></pre>

### struct unions

上記のようなクラスを使った discriminated unions の実装(F# の discriminated unions はこんな感じのクラスに展開されてる)の他に、構造体を使った実装もありえる。
`int` と `(short, short)` みたいな、小さい型のどちらかだけを使いたいみたいな場合はある(特にパフォーマンスを求める場面で)。
ただ、これを実現するには .NET ランタイムのレベルでの対応が必要。
