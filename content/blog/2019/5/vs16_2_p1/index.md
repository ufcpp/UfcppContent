---
title: "Visual Studio 16.1.0 & 16.2 Preview 1"
source_url: "https://ufcpp.net/blog/2019/5/vs16_2_p1/"
content_type: "BlogEntry"
published_at: "2019-05-22T20:35:13"
updated_at: "2019-05-23T11:41:56"
tags: []
umbraco_id: 2244
parent_id: 2241
sort_order: 2
aliases: []
---

# Visual Studio 16.1.0 & 16.2 Preview 1

Visual Studio 16.1 のリリースと、16.2 の Preview 1 が来ていますね。

- [Visual Studio 2019 version 16.1](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes#16.1.0)
- [Visual Studio 2019 version 16.2 Preview 1](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.2.0-pre.1.0)

## 16.1

16.1 の方は、[こないだの Preview 3](../build2019/index.md)からそんなに変わってなくて、割かし「リリースされました」という感じ。

[C# 8.0](../../../../study/csharp/cheatsheet/ap_ver8.md) 的には、

- [Ranges](../../../2018/12/cs8ranges/index.md)は たぶん、今の挙動で確定
- [`switch`式](../../../../study/csharp/cheatsheet/ap_ver8.md#switch-expression)
  - 優先度が `+` よりも上になってる
  - [Target-typed switch](https://github.com/dotnet/csharplang/issues/2389) は 8.0 候補になってて、かつ、未実装
- [非同期イテレーター](../../../2018/12/cs8asyncstreams/index.md)
  - `EnumeratorCancellation ` 属性を付けた引数に `CancellationToken` が渡るように
  - 変更が確定していて、16.2 Preview 1 ですでに挙動が違う(後述)
- [インターフェイスのデフォルト実装](../../../../study/csharp/cheatsheet/ap_ver8.md#default-imeplementation-of-interface)も 16.2 Preview 1 で変更あり(後述)
- [null 許容参照型](../../../2018/12/cs8nrt/index.md)は相変わらず作業真っ最中

という感じ。

## 16.2 Preview 1

IDE 的には以下のようなものが増えてるみたいです。

- プロジェクトの新規作成で、作成したいアプリのタイプで検索できるように
- テスト エクスプローラーがだいぶ見やすく

あと、Developer PowerShell (開発ツールがらみの環境変数とかパスが通った状態の PowerShell)が追加されたみたいです。
これまでもあった [Developer Command Prompt](https://docs.microsoft.com/ja-jp/dotnet/framework/tools/developer-command-prompt-for-vs) の PowerShell 版。

<em>ちょっと C# コンパイラーに致命的なバグがありそうなので注意</em>。
[安全な `stackalloc`](../../../../study/csharp/resource/span.md#safe-stackalloc)を使うと不正なコードを生成して、プログラムが起動できなくなります。
(正確に言うと、`stackalloc` を書いたメソッドを呼んだ瞬間、`InvalidProgram`例外発生。)
修正済みっぽいんですけど、16.2 Preview 1 には反映されていない状態。

- [Invalid IL generated for stackalloc assigned to Span #35764](https://github.com/dotnet/roslyn/issues/35764)

C# 8.0 的には、16.1 の方と以下の差があります。

- [非同期イテレーター](../../../2018/12/cs8asyncstreams/index.md)の`EnumeratorCancellation ` 属性の仕様変更
- [`base(T)`](../../../../study/csharp/oop/oo_inherit.md#non-virtual-base-access) 削除
- [stackalloc in nested expressions](https://github.com/dotnet/csharplang/issues/1412) の追加

あと、null許容参照型をプロジェクト単位で有効化するための設定も、`<Nullable>enable</Nullable>` に変わっています(16.1 までは `NullableContextOptions`)。

### 非同期イテレーターの仕様変更

`X(ct1).WithCancellation(ct2)` みたいなのを書いたときの挙動が変わります。

<pre class="source" title="">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">c1</span> = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>();
        <span class="reserved">var</span> <span class="variable">c2</span> = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>();
 
        <span class="comment">// キャンセルなし</span>
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="method">X</span>()) ;
 
        <span class="comment">// AscynEnumerable 生成時に c1 が渡る</span>
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="method">X</span>(<span class="variable">c1</span>.Token)) ;
 
        <span class="comment">// GetAsyncEnumerator 時に c2 が渡る</span>
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="method">X</span>().<span class="method">WithCancellation</span>(<span class="variable">c2</span>.Token)) ;
 
        <span class="comment">// 旧挙動: c2 だけが渡る</span>
        <span class="comment">// 新挙動: c1, c2 の両方が渡る。内部で CreateLinkedTokenSource</span>
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="method">X</span>(<span class="variable">c1</span>.Token).<span class="method">WithCancellation</span>(<span class="variable">c2</span>.Token)) ;
    }
 
    <span class="comment">// 新挙動: EnumeratorCancellation 属性付きの引数は1個に限る</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method">X</span>([<span class="type">EnumeratorCancellation</span>]<span class="type">CancellationToken</span> <span class="variable">ct</span> = <span class="reserved">default</span>)
    {
        <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Yield</span>();
        <span class="control">yield</span> <span class="control">break</span>;
    }
}
</code></pre>

### base(T) 削除

[base(T) アクセス](../../../../study/csharp/oop/oo_inherit.md#non-virtual-base-access)、いったん取りやめになりました。
(書いた記事どうしよう… 消すか、「今後入る予定です」に変えるか…)

C# コンパイラーだけでできる実装方法だと不満だそうで、 .NET Core ランタイム側も合せて修正変更したいそうです。
結果的に C# 8.0 には間に合わず、ランタイム修正ありなものをマイナー リリースするとは思えないので 9.0 以降での実装になります。

### stackalloc in nested expressions

式のど真ん中に `stackalloc` を書けるようになりました。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span>) =&gt; 0;
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 引数にも書けたり</span>
        <span class="method">M</span>(<span class="reserved">stackalloc</span> <span class="reserved">int</span>[1]);
 
        <span class="comment">// 式のどこにでも書ける</span>
        <span class="control">if</span> (<span class="reserved">stackalloc</span> <span class="reserved">int</span>[1] <span class="method">==</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1]) { }
    }
 
    <span class="comment">// フィールド初期化子内にも書けたり</span>
    <span class="reserved">int</span> x = <span class="method">M</span>(<span class="reserved">stackalloc</span> <span class="reserved">int</span>[1]);
 
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Async</span>()
    {
        <span class="comment">// 式中に書くなら、非同期メソッド内でも stackalloc が書ける</span>
        <span class="method">M</span>(<span class="reserved">stackalloc</span> <span class="reserved">int</span>[1]);
 
        <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Yield</span>();
    }
}
</code></pre>

ぶっちゃけ、[再帰パターン](../../../../study/csharp/cheatsheet/ap_ver8.md#recursive-pattern)のついでだそうです。
再帰パターンの導入で[参照として返せるものの判定](../../../../study/csharp/resource/sp_ref.md#flow-analysis)が複雑になったらしく、
ちゃんとした判定に書き換えたらついでに `stackalloc` を書ける場所も増えたとのこと。
