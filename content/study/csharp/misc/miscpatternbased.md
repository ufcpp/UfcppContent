---
title: "パターン ベースな構文"
source_url: "https://ufcpp.net/study/csharp/misc/miscpatternbased/"
content_type: "Article"
published_at: "2019-06-09T00:00:00"
updated_at: "2024-08-31T17:24:48"
tags: []
umbraco_id: 2249
parent_id: 1338
sort_order: 7
aliases:
  - "/csharp/misc/miscpatternbased/"
---

# パターン ベースな構文

##<a id="sec-generated-title-1"></a> <a id="abctract"></a>概要
C# の言語機能のいくつか(というか結構多くのもの)は、「所定のパターンを満たしている任意の型に使える」というものになっています。
そういう構文を指して「パターン ベース」(pattern-based)な構文と言ったりします。

本項では、パターン ベースにすることのメリットや、
実際にパターン ベースになっている構文について紹介します。

##<a id="sec-generated-title-2"></a> <a id="pattern-based"></a>パターン ベース
例えば C# 3.0 の[クエリ式](../data/sp3_linq.md#query)がパターン ベースな構文の代表例です。
以下のような書き方をした場合、

<pre class="source" title="クエリ式の例">
<code><span class="reserved">from</span> x <span class="reserved">in</span> <span class="variable">source</span>
<span class="reserved">where</span> x &lt; 10
<span class="reserved">select</span> x * x;
</code></pre>

C# コンパイラーが以下のようなメソッド呼び出しに展開します。

<pre class="source" title="クエリ式の展開結果の例">
<code><span class="variable">source</span>
    .<span class="method">Where</span>(<span class="variable">x</span> =&gt; <span class="variable">x</span> &lt; 10)
    .<span class="method">Select</span>(<span class="variable">x</span> =&gt; <span class="variable">x</span> * <span class="variable">x</span>);
</code></pre>

C# コンパイラーは「`select`句を見たら`Select`メソッドに置き換える」というルールだけを提供していて、
`Select`メソッドをどう実装するかは自由にできます。

クエリ式に関しては本当にかなり自由度が高く、以下のように、だいぶ緩い条件で使えます。

- (`Select` メソッドなどを定義した)インターフェイス不要
- 戻り値の型には何の制約もない
- インスタンス メソッドでも[拡張メソッド](../functional/sp3_extension.md)でもいい

要するに、
「所定のパターンを満たしている任意の型に使える」ということです。
このこと、特に1番目の「インターフェイス不要」を指して、「<strong id="key-pattern-based" class="keyword">パターン ベース</strong>」(pattern-based)と言います。

(同じ単語が入っているのでちょっと紛らわしいですが、
[パターン マッチング](../datatype/patterns.md)とは無関係です。)

###<a id="sec-generated-title-3"></a> <a id="converse"></a>パターン ベースな構文の対極
逆に、インターフェイスの実装が必須の構文が1つだけあって、[`using` ステートメント](../resource/oo_dispose.md#using)がそうです。
(ただし、C# 8.0 で、[`ref struct` に対してだけは緩和されています](../resource/oo_dispose.md#pattern-based-using)。)

<pre class="source" title="using ステートメントはインターフェイス必須">
<code><span class="reserved">using</span> System;
 
<span class="reserved">struct</span> <span class="type">Disposable</span>
    <span class="comment">// インターフェイス実装が必須。</span>
    <span class="comment">// 以下の行をコメントアウトするとコンパイル エラーになる。</span>
    : <span class="type">IDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">using</span> (<span class="reserved">var</span> <span class="variable">d</span> = <span class="reserved">new</span> <span class="type">Disposable</span>()) ;
    }
}
</code></pre>

また、パターン ベースの逆という意味では、
C# コンパイラーだけではできない言語機能もあります。
[ジェネリクス](../oop/sp2_generics.md)や[インターフェイスのデフォルト実装](../oop/oo_interface.md#dim)がそうで、
これらは新しい .NET ランタイム(ジェネリクスは .NET Framework 2.0 以降、インターフェイスのデフォルト実装は .NET Core 3.0 以降)が必要になります。

ただ、パターン ベースでは実現不可能でも、「.NET ランタイム的には昔から機能を持っていて、C# 上認められていなかっただけ」というものあります。
例えば C# 4.0 で[オプション引数](../cheatsheet/ap_ver4.md#optional)と[ジェネリクスの共変・反変性](../cheatsheet/ap_ver4.md#variance)という機能が入りましたが、
ランタイム側はもっと昔から(それぞれ .NET Framework 1.0、2.0 時点で)対応していました。
なので、C# コンパイラーだけの更新で実現可能でした。

##<a id="sec-generated-title-4"></a> <a id="advantage"></a>パターン ベースの利点
新しい構文をパターン ベースに実装するのには2つの利点があります。

- C# コンパイラーだけでできる/古い .NET ランタイム上でも動く
- [仮想呼び出し](../oop/oo_vftable.md)が挟まらない

###<a id="sec-generated-title-5"></a> <a id="syntax-sugar"></a>C# コンパイラーだけでできる
パターン ベースな置き換えは C# コンパイラーだけでできる仕事になります。

「[.NET プログラム](../start/st_compile.md#dotnet)」で説明しているように、
C# は、

- C# コンパイラーは、C# ソースコードを中間言語(IL)と呼ばれる汎用的な命令に翻訳する
- .NET ランタイムが IL を CPU 依存で高速な命令に置き換える

という2段階に分けてプログラムを実行しています。
C# コンパイラーがしている仕事の方が比較的楽で、
C# コンパイラーだけの修正で済むなら実装コストがだいぶ低いです。

実装コストの問題だけでなく、
古いランタイムでも動くというメリットもあります。
例えば、2017年リリースの C# 7.0 の機能(例えば[分解](../datatype/deconstruction.md))を使ったプログラムが、
2002年 リリースの .NET Framework 1.0 上でも動かせたりします。

(.NET Framework 1.0  は 2017年時点ですでにサポートも切れています。
サポート外のランタイム上ですら、新しい言語機能を使えたりします。)

###<a id="sec-generated-title-6"></a> <a id="non-virtual"></a>仮想呼び出しを避ける
詳細は「[[雑記] 仮想関数テーブル](../oop/oo_vftable.md)」で説明していますが、
[`virtual` なメソッド](../oop/oo_polymorphism.md#virtual)には、一段階テーブルをはさむコストが発生します。

また、付随して以下のようなコストがかかる場合があります。

- [インライン化](../structured/miscinlining.md)を阻害する
- [値型](../resource/oo_reference.md#valtype)の場合、[ボックス化](../resource/rmboxing.md)が発生することがある

例えば以下のような型があったとします。

<pre class="source" title="分解可能な型の例">
<code><span class="reserved">interface</span> <span class="type">IDeconstructibleTo2Ints</span>
{
    <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">y</span>);
}
 
<span class="reserved">struct</span> <span class="type">Point</span> : <span class="type">IDeconstructibleTo2Ints</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (X, Y) = (<span class="variable">x</span>, <span class="variable">y</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) = (X, Y);
}
</code></pre>

この型は[分解](../datatype/deconstruction.md)構文を使えるように作ってあります。
(分解は `Deconstruct` メソッドの呼び出しに展開されます。)
分解もパターン ベースなので、インターフェイスは必須ではありません。

この型に対して、以下のような2つのメソッドを考えます。
どちらも `Deconstruct` メソッドに展開されますが、
`Sum1` はパターン ベース(`Point`構造体の`Deconstruct`が直接呼ばれる)、
`Sum2` はインターフェイスを介しています。

<pre class="source" title="分解がインターフェイスを介するかどうか">
<code><span class="comment">// Point を直接分解。</span>
<span class="comment">// 最終的にインライン展開が働いて、単なる p.X + p.Y に展開される(ものすごく高速)。</span>
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Sum1</span>(<span class="type">Point</span> <span class="variable">p</span>)
{
    <span class="reserved">var</span> (<span class="variable">x</span>, <span class="variable">y</span>) = <span class="variable">p</span>;
    <span class="control">return</span> <span class="variable">x</span> + <span class="variable">y</span>;
}
 
<span class="comment">// インターフェイスを介して分解。</span>
<span class="comment">// インライン展開が効かず、ボックス化も起きてるので遅い。</span>
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Sum2</span>(<span class="type">IDeconstructibleTo2Ints</span> <span class="variable">p</span>)
{
    <span class="reserved">var</span> (<span class="variable">x</span>, <span class="variable">y</span>) = <span class="variable">p</span>;
    <span class="control">return</span> <span class="variable">x</span> + <span class="variable">y</span>;
}
</code></pre>

[ベンチマークを取ってみれば](https://gist.github.com/ufcpp/a09030dd049f20d10e2504edf3711926)わかるんですが、
`Sum1`は最適化によってほとんど消えることすらあって、計測できない(誤差しか残らない)くらい高速です。
一方、`Sum2`はボックス化で24バイトのゴミが発生しますし、実行に数ミリ秒要します。

パターン ベースである(インターフェイスを要求しない)ことで、このくらいの速度差が生じます。

##<a id="sec-generated-title-7"></a> <a id="flexibility"></a>パターンの自由度
ということで、C# の構文の多くがパターン ベースな実装になっています。
ただ、実装された時期によってどのくらい自由が利くかに差があったりします。
(基本的には新しいものほど自由が利く。ただ、新しいものでも、他の構文との兼ね合いで制限が掛かる場合がある。)

一番自由が利くのはクエリ式です。
例えば、クエリ式(の`where`と`select`)を使える最低限のコードを書くと以下のようになります。
(意味のあることはしていません。単に、クエリ式で使えるというだけです。)

<pre class="source" title="クエリ式を使うための最低限の型の例">
<code><span class="reserved">using</span> System;
 
<span class="reserved">struct</span> <span class="type">Queryable</span>
{
    <span class="reserved">public</span> <span class="type">Queryable</span> <span class="method">Where</span>(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; <span class="variable">f</span>) =&gt; <span class="reserved">this</span>;
    <span class="reserved">public</span> <span class="type">Queryable</span> <span class="method">Select</span>(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span>) =&gt; <span class="reserved">this</span>;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">q</span> =
            <span class="reserved">from</span> x <span class="reserved">in</span> <span class="reserved">new</span> <span class="type">Queryable</span>()
            <span class="reserved">where</span> x &lt; 10
            <span class="reserved">select</span> x * x;
    }
}
</code></pre>

これに対して、融通が利くポイントが2つあります。

- [オプション引数](../cheatsheet/ap_ver4.md#optional)や[可変長引数](../structured/sp_params.md)が付いていてもいい
- [拡張メソッド](../functional/sp3_extension.md)でもいい

例えば、上記のコードは以下のように書き換えてもコンパイルできます。

<pre class="source" title="拡張メソッドやオプション引数の許容">
<code><span class="reserved">using</span> System;
 
<span class="reserved">struct</span> <span class="type">Queryable</span>
{
    <span class="reserved">public</span> <span class="type">Queryable</span> <span class="method">Where</span>(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; <span class="variable">f</span>, <em><span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable">dummy</span></em>) =&gt; <span class="reserved">this</span>;
}
 
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">QueryableExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Queryable</span> <span class="method">Select</span>(<em><span class="reserved">this</span></em> <span class="type">Queryable</span> <span class="variable">q</span>, <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span>, <em><span class="reserved">int</span> <span class="variable">dummy</span> = 0</em>) =&gt; <span class="variable">q</span>;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">q</span> =
            <span class="reserved">from</span> x <span class="reserved">in</span> <span class="reserved">new</span> <span class="type">Queryable</span>()
            <span class="reserved">where</span> x &lt; 10
            <span class="reserved">select</span> x * x;
    }
}
</code></pre>

##<a id="sec-generated-title-8"></a> <a id="index"></a>パターン ベースな構文一覧
以下に、パターン ベースになっている構文の一覧を示します。

| 構文 | 拡張メソッド可 | オプション引数可 |
| ---- | ---- | ---- |
| [クエリ式](../data/sp3_stdquery.md)  |  〇 | 〇  |
| [コレクション初期化子](../functional/sp3_lambda.md#collectioninit)  |  〇<sup>※1</sup> | 〇 |
| [分解](../datatype/deconstruction.md#arbitrary-types)  | 〇 | × |
| [await](../async/sp5_awaitable.md#awaiter)  | 〇 | × |
| [await foreach](../async/asyncstream.md#await-foreach)  | 〇<sup>※4</sup> | 〇 |
| [await using](../async/asyncstream.md#await-using)  | × | 〇 |
| [foreach](../data/sp_foreach.md)<sup>※2</sup> | 〇<sup>※4</sup> | × |
| [fixed](../interop/sp_unsafe.md#custom-fixed)  | × | × |
| [using](../resource/oo_dispose.md#using)<sup>※3</sup>  | × | × |

<sup>※1</sup> [拡張メソッド可になったのは C# 6.0 から](../cheatsheet/ap_ver6.md#add-extensions)

<sup>※2</sup> ループの最後に `Dispose` が呼ばれるためにはインターフェイスの実装、もしくは、[`ref struct` である必要があります](../resource/oo_dispose.md#pattern-based-using)。

<sup>※3</sup> [`ref struct` 限定](../resource/oo_dispose.md#pattern-based-using)でパターン ベース。クラスや通常の構造体の場合はインターフェイスの実装が必須。

<sup>※4</sup> [拡張メソッド可になったのは C# 9.0 から](../data/sp_foreach.md#extension-getenumerator)

`foreach` と `await foreach`、`using` と `await using` の差は実装時期の差によるものです。
非同期版(`await` 付き)の方が C# 8.0 での実装と新しく、制限が緩和されています。
C# 8.0 の計画段階では既存の(同期の) `foreach` や `using` の制限緩和も検討されましたが、
破壊的変更になりそうとのことで断念されました。
[`ref struct`](../resource/refstruct.md)に対してだけは、
そもそも `ref struct` 自体が新しくて破壊的変更の影響が少ないことと、
ないと困るという理由で[制限が緩和されています](../resource/oo_dispose.md#pattern-based-using)。
