---
title: "【C# 10.0 関連】引数なしコンストラクターの Activator バグ"
source_url: "https://ufcpp.net/blog/2021/12/activator-net48/"
content_type: "BlogEntry"
published_at: "2021-12-07T22:01:23"
updated_at: "2021-12-07T22:02:32"
tags: []
umbraco_id: 2378
parent_id: 2375
sort_order: 1
aliases: []
---

# 【C# 10.0 関連】引数なしコンストラクターの Activator バグ

そういえば[ライブ配信(8月)](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/43#issuecomment-903272324)とか Twitter では話しているものの、ちゃんとこのサイト内には書いていなかったなと言う話。

C# 10.0 で[構造体の引数なしコンストラクター](../../../../study/csharp/cheatsheet/ap_ver10.md#struct-parameterless-ctor)が書けるようになりました。

<pre class="source" title="構造体の引数なしコンストラクター">
<code><span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="type">A</span>() =&gt; X = 1; <span class="comment">// ←要はこういうの</span>
}
</code></pre>

[今年2月にブログで書いてるんですが](../../2/parameterlessstructctor/index.md)、これ、C# 6.0 の時に一度採用しようとしたものの、[`Activator.CreateInstance`](https://docs.microsoft.com/ja-jp/dotnet/api/system.activator.createinstance) にバグがあって、いくつかの場面でまっとうに動かないということで延期されたという経緯があります。

で、それは直したし、直っていない頃の古いランタイムはサポート外にしていいだろうということで、晴れて C# 10.0 で採用されました。

ところが、「バグを直したと思ったら別のバグが残ってるランタイムが現存している」と言うことが後から発覚…
実は、 .NET Framework で実行すると引数なしコンストラクターがちゃんと動かいことがあったりします。

## 旧バグ

C# の構造体は `new T()` と `default(T)` が同じ「0初期化」を表してた時期が長かったので、`Activator.CreateInstance` がコンストラクターを呼んでくれず、単に0初期化した値(要するに `default(T)`)を返してくるというものでした。

`Activator.CreateInstance` を直接呼ぶことはまああんまりないでしょうが、ジェネリック型の `new()` 制約は内部的に `Activator.CreateInstance` を使っていて、間接的に影響を受ける人は結構多いと思います。

例えば、先ほどの、引数なしコンストラクターで `X` を 1 に初期化しているはずの構造体 `A` を使って以下のようなコードを書いたとします。

<pre class="source" title="引数なしコンストラクターが呼ばれなかったコードの例">
<code><span class="reserved">var</span> <span class="variable">a</span> = <span class="method">New</span>&lt;<span class="type">A</span>&gt;();

<span class="comment">// 古いランタイムだとこれで a.X == 0 に</span>
<span class="comment">// 1 になるはずなのに…</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span>.X);

<span class="reserved">static</span> <span class="type">T</span> <span class="method">New</span>&lt;<span class="type">T</span>&gt;()
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>()
    =&gt; <span class="reserved">new</span> <span class="type">T</span>();
</code></pre>

直接 `new A()` すればちゃんと `X` が 1 に初期化されるんですが、
`New<T>` メソッドを介すると 0 になっていました。

C# 6.0 当時の話なので確か、
「.NET Framework 4.5 で問題が発覚して、4.6 では直した(つもり)」
とかだったと思います。
当時のポリシーだと古いランタイムのサポートを切れないのでお蔵入り。

## 現バグ

直した**つもり**。

実際、.NET Core では(.NET 5, .NET 6 も)ちゃんと直っています。
問題は .NET Framework の方でして、現行の最新版である .NET Framework 4.8 で別のバグり方をしています。

`CreateInstance` を呼ぶのが1回目なら正しく引数なしコンストラクターが呼ばれるんですが、
2回目以降は `default(T)` を返してしまうという内容。

察しは付くと思いますが、キャッシュ関連のバグです。
何らかのキャッシュを持たせて `CreateInstance` を高速化する最適化は後から追加されたものなので、「1回直したはずのものが再発」という状態です。

先ほどと同じ構造体 `A` と `New<T>` メソッドを使った場合、
.NET Framework 4.8 で実行すると「2度目がおかしい」という状態になります。

<pre class="source" title=".NET Framework 限定 Activator バグ">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;<span class="type">A</span>&gt;().X); <span class="comment">// 1回目は大丈夫。ちゃんと 1。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;<span class="type">A</span>&gt;().X); <span class="comment">// 2回目以降なぜか 0 に… (.NET Framework 限定のバグ)</span>
</code></pre>

## TargetFramework net4.8 じゃなくてもバグる

これ、コンパイル時(C# コンパイラー側)の問題ではなくて、
実行時(.NET Framework ランタイム側)の問題なので、
例えばの話、

1. netstandard2.0 なライブラリで以下のようなコードを書く (LangVersion 指定で明示的に C# のバージョンを 10.0 に上げる)

<pre class="source" title="netstandard2.0 で C# 10.0 を有効化して書く">
<code><span class="reserved">namespace</span> ClassLibrary1;

<span class="reserved">public struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="type">A</span>() =&gt; X = 1;
}
</code></pre>

2. 以下のようなアプリ コードを書く (これは C# 7.3 でも動く)

<pre class="source" title="アプリ コード">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// ジェネリックな new() は、内部的には CreateInstance&lt;T&gt;() と一緒</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;ClassLibrary1.<span class="type">A</span>&gt;().X);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;ClassLibrary1.<span class="type">A</span>&gt;().X);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;ClassLibrary1.<span class="type">A</span>&gt;().X);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;ClassLibrary1.<span class="type">A</span>&gt;().X);
    }

    <span class="reserved">static</span> <span class="type">T</span> <span class="method">New</span>&lt;<span class="type">T</span>&gt;()
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>()
        =&gt; <span class="reserved">new</span> <span class="type">T</span>();
}</code></pre>

とやると、アプリ側、 netcoreapp1.0 とか net5.0 とかで動かす分には問題なく 1, 1, 1, 1 という結果になるんですが、
これを .NET Framework 4.8 で実行すると、1, 0, 0, 0 という結果になります。

## 影響範囲

現在の C# は「[TargetFramework に応じて言語バージョンを自動選択](../../../../study/csharp/cheatsheet/langversionoption.md#new-options)」という方針になっていて、
「古いランタイムで最新の C# 構文を使う」というのは「わかってる人だけがやってくれ」と言うことになっています。

通常 .NET Framework 4.8 で使える C# は C# 7.3 で、
C# 10.0 の新機能が正しく動かなくても概ね問題は起こさないはず…

なんですが、そこで問題になるのが、
先ほどの「ライブラリ側で C# 10.0 にして引数なしコンストラクターを使っている」という場合。

1. ライブラリ側が netstandard2.0 で、引数なしコンストラクターを使っている
2. アプリ側が net48 で、`new()` 制約越しにジェネリックなメソッドで `new T()` している
3. それを Windows 上で .NET Framework 4.8 で実行する

みたいな状況で問題を起こします。

レアな条件ではあるんですが、
「わかっている人が書いたライブラリを、わかっていない人が参照する」というものなので、
「ありえなくもない」くらいには警戒が必要です。

### おまけ: Unity

ちなみに、TargetFramework が net48 であっても、
[Unity](https://unity.com/ja) ([Mono](https://ja.wikipedia.org/wiki/Mono_(%E3%82%BD%E3%83%95%E3%83%88%E3%82%A6%E3%82%A7%E3%82%A2)))で実行する分には問題を起こしません。
あくまでランタイムが .NET Framework の時にだけ起こります。
