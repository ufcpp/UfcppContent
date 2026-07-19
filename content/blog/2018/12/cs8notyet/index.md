---
title: "C# 8.0 その他 (Preview 1での未実装機能)"
source_url: "https://ufcpp.net/blog/2018/12/cs8notyet/"
content_type: "BlogEntry"
published_at: "2018-12-12T09:39:14"
updated_at: "2018-12-12T09:39:14"
tags: []
umbraco_id: 2193
parent_id: 2177
sort_order: 11
aliases: []
---

# C# 8.0 その他 (Preview 1での未実装機能)

これまで紹介してきたもの以外にも、C# 8.0での導入が予定されている機能はいくつかあります。
ただ、Visual Studio 2019 Preview 1でまだ実装されていない機能・ちゃんと動いていない機能はまとめて軽く紹介して終わりにしようかと思います。
次以降のPreviewで実装されたらまた改めて紹介します。

## インターフェイスのデフォルト実装

インターフェイス中のメソッドに実装を持てるようになります。
これに関しては昔書いた記事があるのでそちらを参照:

- [「インターフェースのデフォルト実装」の導入（前編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/013)
- [「インターフェースのデフォルト実装」の導入（中編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/014)
- [「インターフェースのデフォルト実装」の導入（後編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/015)

先日「[RuntimeFeature クラス](../runtimefeature/index.md)」で紹介した通り、
ランタイムの修正が必須の機能です。

## pattern-base な using/foreach

[前に1度書いていますが](../../7/pickuproslyn0711/index.md)、C# には単なるメソッド呼び出しに置き換えるような、シンタックスシュガーな文法が結構あります。
例えば、クエリ式の場合、以下の2行は全く同じ意味になります。

<pre class="source" title="">
<code><span class="reserved">var</span> q1 =
    <span class="reserved">from</span> x <span class="reserved">in</span> source
    <span class="reserved">where</span> x &gt; 5
    <span class="reserved">select</span> x * x;
 
<span class="reserved">var</span> q2 = source
    .Where(x =&gt; x &gt; 5)
    .Select(x =&gt; x * x);
</code></pre>

問題はここから先。
クエリ式の場合は、この`Where`や`Select`メソッドにかなり自由が効きます。

- 特にインターフェイスの実装等は必要なく、所定のパターンを満たしていれば何でもいい
- インスタンス メソッドでも拡張メソッドでもいい
- オプション引数や params があってもいい

一方で、`foreach`の場合だと以下の制限が掛かります。

- インスタンス メソッドでないとダメ
- オプション引数や params があるとダメ

さらに、`using`ステートメントに至ってはもっと厳しい制限が掛かっています。

- `IDisposable`インターフェイスを実装していないとダメ

これに対して、C# 8.0 では、`foreach`と`using`でもクエリ式と同程度の緩さで「パターンでの(pattern-based)実装」が認められるようになります。
[昨日](../cs8asyncstreams/index.md)紹介した非同期版の `foreach` も同様です。

ちなみに、提案では「enhanced using」と呼ばれていて、
次節の「using declarationとセット」、かつ、「`using`の方が主役で`foreach`の方はおまけ」です。

## using declaration

`using`ステートメントに対して、以下のような要望は多いです。

- `using`のネストがしんどい、
- `Dispose`したいタイミングはほとんどの場合、変数のスコープと同じ

ということで、以下のように、変数に対する修飾子として`using`を書くことで、
その変数のスコープから抜けるときに`Dispose`を呼ぶという機能を追加する予定です。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">void</span> Dispose() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">&quot;A Disposed&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">using</span> <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">A</span>();
        <span class="reserved">using</span> <span class="reserved">var</span> b = <span class="reserved">new</span> <span class="type">A</span>();
 
        {
            <span class="reserved">using</span> <span class="reserved">var</span> c = <span class="reserved">new</span> <span class="type">A</span>();
            <span class="comment">// c のスコープはここまでなので、ここで c.Dispose()</span>
        }
 
        <span class="comment">// ここで b.Dispose(); a.Dispose();</span>
        <span class="comment">// ちなみに、宣言とは逆順で呼ばれる</span>
    }
}
</code></pre>

## Target-typed new

C# 7.1で入った[`default`式](../../../../study/csharp/cheatsheet/ap_ver7_1.md#default-expr)と同様に、`new`に対しても左辺からの型推論が効くようになります。

<pre class="source" title="">
<code><span class="comment">// これは 右→左 の推論。C# 3.0 の頃から使える。</span>
<span class="reserved">var</span> a1 = <span class="reserved">new</span> <span class="type">A</span>(1, 2);
 
<span class="comment">// C# 8.0 では、左→右 の推論が入る。</span>
<span class="type">A</span> a2 = <span class="reserved">new</span>(1, 2);
</code></pre>

## caller expression attribute

C# 5.0で、[Caller Info 属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)というものがいくつか入っています。
以下のように、コンパイラーによって呼び出し元のメソッド名などを挿入してもらう機能です。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> M([<span class="type">CallerMemberName</span>]<span class="reserved">string</span> callerName = <span class="reserved">null</span>)
        =&gt; <span class="type">Console</span>.WriteLine(callerName);
 
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// M には何も引数を渡していないものの、</span>
        <span class="comment">// CallerMemberName が付いているので null ではなく、呼び出し元のメソッド名</span>
        <span class="comment">// (この場合は &quot;Main&quot;)がコンパイラーによって挿入される。</span>
        M();
    }
}
</code></pre>

C# 8.0で、この手の属性が1つ増えます。
`CallerArgumentExpression`属性を付けることで、
引数に渡した式全体を受け取れます。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x, [<span class="type">CallerArgumentExpression</span>(<span class="string">&quot;x&quot;</span>)]<span class="reserved">string</span> xExpression = <span class="reserved">null</span>)
        =&gt; <span class="type">Console</span>.WriteLine(xExpression);
 
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        M(1 + 2 + 3); <span class="comment">// &quot;1 + 2 + 3&quot; が xExpression に渡る</span>
        M(2 * 3);     <span class="comment">// 同上、&quot;2 * 3&quot;</span>
    }
}
</code></pre>

わかりやすい用途は、例えば`XUnit.Assert`とかです。
単体テストが失敗したときに、失敗の原因になった式をログに表示できます。

## generic attributes

属性にジェネリックなクラスを使えるようになります。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">MyAttribute</span>&lt;<span class="type">T</span>&gt; : <span class="type">Attribute</span> { }
 
[My&lt;<span class="reserved">int</span>&gt;]
<span class="reserved">class</span> <span class="type">Target</span> { }
</code></pre>

## 機能一覧

ここで紹介したのは、roslyn リポジトリにある[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)を元に選んだものです。
一方で、csharplang の方の [8.0 candidate](https://github.com/dotnet/csharplang/milestone/8) マイルストーンの方には他にもいくつか並んでいます。

7.0 の時の経験からいうと、
基本的には[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)に並んでいるものが実装されていきますが、
多少の入れ替わりはあったりします。
急にLanguage Feature Statusに追加されるものもあれば、
今並んでいても8.xに回されることもあります。

例えば、実装状況を見るに、以下の2つなんかはLanguage Feature Statusに並んでいませんが、8.0 に入るんじゃないかという感じがします。

- [Champion: Unmanaged constructed types #1744](https://github.com/dotnet/csharplang/issues/1744)
- [Champion "Negated-condition if statement" #882](https://github.com/dotnet/csharplang/issues/882)
