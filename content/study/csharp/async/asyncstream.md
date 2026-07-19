---
title: "非同期ストリーム"
source_url: "https://ufcpp.net/study/csharp/async/asyncstream/"
content_type: "Article"
published_at: "2019-06-09T00:00:00"
updated_at: "2021-01-03T19:17:48"
tags: []
umbraco_id: 2248
parent_id: 1326
sort_order: 11
aliases:
  - "/csharp/async/asyncstream/"
---

# 非同期ストリーム

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、[非同期メソッド](sp5_async.md)が大幅に拡張されます。

一連のデータ(data stream)を、非同期に生成(イテレーター)して非同期に消費(`foreach`)する機能なので、これらを合わせて非同期ストリーム(async stream)と呼ばれます。

同期的な処理であれば、これまでも[イテレーター](../data/sp2_iterator.md)と[`foreach`](../data/sp_foreach.md)という機能がありました。
非同期ストリームはこれらの非同期版([非同期メソッド](sp5_awaitable.md)との混在)になります。

## <a id="sec-generated-title-2"></a> <a id="iasyncenumerable"></a>IAsyncEnumerable

[イテレーター](../data/sp2_iterator.md)と[`foreach`](../data/sp_foreach.md)では、[`IEnumerable<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.ienumerable-1)インターフェイス(`System.Collections.Generic`名前空間)が中心的な役割を担います。

- イテレーターの戻り値は`IEnumerable<T>`もしくは[`IEnumerator<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.ienumerator-1)である必要がある
- `foreach`は[パターン ベース](../misc/miscpatternbased.md)で、
「`IEnumerable<T>`と同じメソッドを持つ」というのが満たすべきパターン

C# 8.0 ではこれらの非同期版が入るわけですが、
同期版と同じく中心的な役割を担うインターフェイスがあり、
それが[`IAsyncEnumerable<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.iasyncenumerable-1)インターフェイス(`System.Collections.Generic`名前空間)です。
以下のような構造になっています。

<pre class="source" title="IAsyncEnumerable の構造">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
{
    <span class="type">IAsyncEnumerator</span>&lt;<span class="type">T</span>&gt; <span class="method">GetAsyncEnumerator</span>(<span class="type">CancellationToken</span> <span class="variable">cancellationToken</span> = <span class="reserved">default</span>);
}
 
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt; : <span class="type">IAsyncDisposable</span>
{
    <span class="type">T</span> Current { <span class="reserved">get</span>; }
    <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; <span class="method">MoveNextAsync</span>();
}
 
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncDisposable</span>
{
    <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>();
}
</code></pre>

インターフェイス名とメソッド名に`Async`が付いたのと、一部のメソッドの戻り値が[`ValueTask<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.valuetask-1)になっているくらいで、ほとんど同期版と同じ構造です。

同期版と同じく、非同期ストリームと以下のような関わりがあります。

- 非同期イテレーターの戻り値は`IAsyncEnumerable<T>`もしくは`IAsyncEnumerator<T>`である必要がある
- 非同期`foreach`は[パターン ベース](../misc/miscpatternbased.md)で、
「`IAsyncEnumerable<T>`と同じメソッドを持つ」というのが満たすべきパターン

ちなみに、同期版とは違って、非ジェネリックな`IAsyncEnumerable`、`IAsyncEnumerator`インターフェイスはありません。
(非ジェネリックな`IEnumerable`は[ジェネリクス](../oop/sp2_generics.md)導入前の名残で、互換性のためだけに残されているものです。)

## <a id="sec-generated-title-3"></a> <a id="await-foreach"></a>非同期foreach

仕組みが単純なので、データの消費側(非同期`foreach`)の方を先に説明します。
以下のように、`await foreach` と書くことで、
`IAsyncEnumerable<T>` (と同じパターンを持つ型)の列挙ができます。

<pre class="source" title="非同期 foreach">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncForeach</span>(<span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">items</span>)
{
    <em><span class="reserved">await</span> <span class="control">foreach</span></em> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable">items</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">item</span>);
    }
}
</code></pre>

[`await`](sp5_async.md#async)演算子と同じく、
非同期メソッド(`async` 修飾が付いたメソッド)内でだけ使えます。

このコードは、同期版の`foreach`と似たような感じで、以下のように展開されます。 同期版と比べて、`MoveNext`と`Dispose`が非同期になっただけです。

<pre class="source" title="非同期foreachの展開結果">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncForeach</span>(<span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">items</span>)
{
    <span class="reserved">var</span> <span class="variable">e</span> = <span class="variable">items</span>.<span class="method">GetAsyncEnumerator</span>();
    <span class="control">try</span>
    {
        <span class="control">while</span> (<span class="reserved">await</span> <span class="variable">e</span>.<span class="method">MoveNextAsync</span>())
        {
            <span class="reserved">int</span> <span class="variable">item</span> = <span class="variable">e</span>.Current;
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">item</span>);
        }
    }
    <span class="control">finally</span>
    {
        <span class="control">if</span> (<span class="variable">e</span> != <span class="reserved">null</span>)
        {
            <span class="reserved">await</span> <span class="variable">e</span>.<span class="method">DisposeAsync</span>();
        }
    }
}
</code></pre>

同期版と同じく、`finally`内の処理にはいくつかバリエーションがあります。

- enumerator (上記の例で言う `e`)が構造体なら null チェックは挟まらない
- `DisposeAsync` を持っていない場合は`finally`内の処理自体消える

### <a id="sec-generated-title-4"></a> <a id="pattern-based-await-foreach"></a>パターン ベース

パターン ベースなので、インターフェイスを実装していなくても、
所定のメソッドさえ持っていれば非同期`foreach`で使えます。
以下はその一例です。

<pre class="source" title="パターン ベースで非同期foreachに対応する型の例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="comment">// このメソッドが「Enumerable」の必須要件。</span>
    <span class="comment">// この例では自分自身を返している(それでもOK)ものの、通常は別の型を作って返す。</span>
    <span class="reserved">public</span> <span class="type">A</span> <span class="method">GetAsyncEnumerator</span>() =&gt; <span class="reserved">this</span>;
 
    <span class="comment">// 以下の2つが「Enumerator」の必須要件。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Current =&gt; 0;
    <span class="reserved">public</span> <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; <span class="method">MoveNextAsync</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;MoveNextAsync&quot;</span>);
        <span class="control">return</span> <span class="reserved">new</span> <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt;(<span class="reserved">false</span>);
    }
 
    <span class="comment">// DisposeAsync はなくてもいい。なければ呼ばれないだけ。</span>
    <span class="reserved">public</span> <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;DisposeAsync&quot;</span>);
        <span class="control">return</span> <span class="reserved">default</span>;
    }
 
    <span class="comment">// 同期の Dispose は定義してあっても呼ばれないので注意。</span>
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">A</span>()) ;
    }
}
</code></pre>

この例では`ValueTask`型を使っていますが、これすらもパターン ベースで大丈夫です。
要は、`await`可能であれば型は問いません。
また、[通常の `foreach` と同じく](../data/sp_foreach.md#extension-getenumerator)、C# 9.0 から拡張メソッドも受け付けるようになりました。

また、後から追加された構文だけあって、同期版の`foreach`よりもパターンの条件が緩いです。以下のように、オプション引数や可変長引数が付いていても平気です(同期版はダメ)。

<pre class="source" title="非同期foreachは求められるパターンが緩い">
<code><span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="comment">// 可変長引数があってもいい</span>
    <span class="reserved">public</span> <span class="type">A</span> <span class="method">GetAsyncEnumerator</span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable">dummy</span>) =&gt; <span class="reserved">this</span>;
 
    <span class="reserved">public</span> <span class="reserved">int</span> Current =&gt; 0;
 
    <span class="comment">// オプション引数があってもいい。</span>
    <span class="reserved">public</span> <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; <span class="method">MoveNextAsync</span>(<span class="type">CancellationToken</span> <span class="variable">token</span> = <span class="reserved">default</span>) =&gt; <span class="reserved">default</span>;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">A</span>()) ;
    }
}
</code></pre>

## <a id="sec-generated-title-5"></a> <a id="await-using"></a>非同期using

前節の非同期`foreach`の展開結果には`DisposeAsync`の呼び出しが含まれていました。
また、`IAsyncEnumerator<T>`は`IAsyncDisposable`から派生しています。

これは同期版の頃([`foreach`の展開結果](../data/sp_foreach.md#foreach))からある仕様で、同期版にも`Dispose`の呼び出しが含まれています。
この処理は、[`using`ステートメント](../resource/oo_dispose.md#using)がやっていることと同じです。
すなわち、`foreach`は`using`を兼ねています。

ということで、
非同期`foreach`が追加するのであれば、
同時に非同期`using`も追加するのが妥当です。
そこで実際、C# 8.0で非同期`using`が追加されています。

非同期`foreach`と同様`await using`という書き方をします。

<pre class="source" title="非同期using">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncUsing</span>(<span class="type">IAsyncDisposable</span> <span class="variable">d</span>)
{
    <em><span class="reserved">await</span> <span class="reserved">using</span></em> (<span class="variable">d</span>)
    {
        <span class="comment">// d を破棄する前にやっておきたい処理</span>
    }
}
</code></pre>

これも非同期`foreach`と同様に、非同期メソッド(async 修飾が付いたメソッド)内でだけ使えます。

展開結果は、同期版で`Dispose()`呼び出しだった部分が`await DisposeAsync()`に変わっているだけです。
上記のコードは以下のように展開されます。

<pre class="source" title="非同期usingの展開結果">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncUsing</span>(<span class="type">IAsyncDisposable</span> <span class="variable">d</span>)
{
    <span class="control">try</span>
    {
        <span class="comment">// d を破棄する前にやっておきたい処理</span>
    }
    <span class="control">finally</span>
    {
        <span class="reserved">await</span> <span class="variable">d</span>.<span class="method">DisposeAsync</span>();
    }
}
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="pattern-based-await-using"></a>パターン ベース

「[パターン ベースな構文](../misc/miscpatternbased.md#converse)」で説明していますが、同期版の`using`は数少ない「インターフェイス実装が必須な構文」です。

一方、非同期`using`はパターン ベースになっています。
以下のように、`IAsyncDisposable`インターフェイスを実装せず、
単に`DisposeAsync`メソッドを持っていれば`await using`で使えます。

<pre class="source" title="パターン ベースな非同期using">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="comment">// 非同期 using は別に IAsyncDisposable インターフェイスの実装を求めない。</span>
<span class="reserved">class</span> <span class="type">AsyncDisposable</span>
{
    <span class="comment">// ちゃんと await using のブロックの最後で呼ばれる。</span>
    <span class="comment">// 戻り値の型が Task や ValueTask である必要もない。</span>
    <span class="reserved">public</span> <span class="type">MyAwaitable</span> <span class="method">DisposeAsync</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;disposed async&quot;</span>);
        <span class="control">return</span> <span class="reserved">default</span>;
    }
}
 
<span class="reserved">struct</span> <span class="type">MyAwaitable</span> { <span class="reserved">public</span> <span class="type">ValueTaskAwaiter</span> <span class="method">GetAwaiter</span>() =&gt; <span class="reserved">default</span>; }
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">await</span> <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">AsyncDisposable</span>())
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;inside using&quot;</span>);
        }
    }
}
</code></pre>

見ての通り、`DisposeAsync`の戻り値は`await`可能でさえあれば何でも構いません。

また、オプション引数や可変長引数があっても構いません。

<pre class="source" title="オプション引数などがあってもawait using可能">
<code><span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>(<span class="reserved">int</span> <span class="variable">dummy</span> = 0) =&gt; <span class="reserved">default</span>;
}
 
<span class="reserved">struct</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable">dummy</span>) =&gt; <span class="reserved">default</span>;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">await</span> <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">A</span>()) { }
        <span class="reserved">await</span> <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">B</span>()) { }
    }
}
</code></pre>

制限と言えば、インスタンス メソッドしか受け付けない(拡張メソッドは使えない)くらいです。

一方で、同期版と違って、[`as`演算子](../oop/misc_as.md)を使った動的な型チェックはしません。
以下のように、直接的には`IAsyncDisposable`インターフェイスを実装していなくて、
パターンも満たさない型に対して`await using`を使うとコンパイル エラーになります。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">class</span> <span class="type">A</span> { }
 
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>, <span class="type">IAsyncDisposable</span>
{
    <span class="reserved">public</span> <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>() =&gt; <span class="reserved">default</span>;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="comment">// A は IAsyncDisposable じゃないけど、</span>
        <span class="comment">// 派生クラスの B は IAsyncDisposable を実装。</span>
        <span class="reserved">await</span> <span class="method">AsyncUsing</span>(<span class="reserved">new</span> <span class="type">B</span>());
    }
 
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncUsing</span>(<span class="type">A</span> <span class="variable">a</span>)
    {
        <span class="comment">// これはコンパイル エラーになる。</span>
        <span class="comment">// A が直接 IAsyncDisposable を実装しているか、パターンを満たしている必要がある。</span>
        <span class="reserved">await</span> <span class="reserved">using</span> (<span class="error"><span class="variable">a</span></span>) { }
    }
}
</code></pre>

ジェネリック型引数に対して使う場合にも、`IAsyncDisposable`制約が必要になります。

<pre class="source" title="IAsyncDisposable 制約が必須">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IAsyncDisposable</span> <span class="comment">// この制約がないと await using の行でコンパイル エラーに。</span>
{
    <span class="reserved">await</span> <span class="reserved">using</span> (<span class="variable">x</span>) { }
}

</code></pre>

### <a id="sec-generated-title-7"></a> <a id="await-using-declaration"></a>using変数宣言との併用

[`using`変数宣言](../resource/oo_dispose.md#using-declaration)との併用も可能です。
以下のような書き方ができます。

<pre class="source" title="非同期using変数宣言の例">
<code><span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">struct</span> <span class="type">AsyncDisposable</span>
{
    <span class="reserved">public</span> <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>() =&gt; <span class="reserved">default</span>;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">await</span> <span class="reserved">using</span> <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">AsyncDisposable</span>();
 
        <span class="comment">// このメソッドを抜けるタイミングで DisposeAsync が呼ばれる</span>
    }
}
</code></pre>

### <a id="sec-generated-title-8"></a> <a id="sync-and-async"></a>DisposeとDisposeAsyncの混在

ちなみに、`Dispose`(`IDisposable`インターフェイス)と`DisposeAsync`(`IAsyncDisposable`インターフェイス)の両方を実装をしている場合、それぞれ同期版`using`、非同期`using`でしか呼ばれません。
非同期版が同期版を兼ねたりはしませんし、その逆もまたしかり。

以下の例では、`using`の行では`Dispose`だけが呼ばれますし、
`await using`の行では`DisposeAsync`だけが呼ばれます。

<pre class="source" title="usingとawait usingは独立">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">struct</span> <span class="type">Disposable</span> : <span class="type">IDisposable</span>, <span class="type">IAsyncDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;同期 Dispose&quot;</span>);
    <span class="reserved">public</span> <span class="type">ValueTask</span> <span class="method">DisposeAsync</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;非同期 Dispose&quot;</span>);
        <span class="control">return</span> <span class="reserved">default</span>;
    }
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">d</span> = <span class="reserved">new</span> <span class="type">Disposable</span>();
 
        <span class="comment">// Dispose だけが呼ばれる</span>
        <span class="reserved">using</span> (<span class="variable">d</span>) { }
 
        <span class="comment">// DisposeAsync だけが呼ばれる</span>
        <span class="reserved">await</span> <span class="reserved">using</span> (<span class="variable">d</span>) { }
    }
}
</code></pre>

## <a id="sec-generated-title-9"></a> <a id="async-iterator"></a>非同期イテレーター

[非同期`foreach`](#await-foreach)とは逆の、データの生成側の機能が非同期イテレーターです。
簡単に言うと、[`yield`](../data/sp2_iterator.md#block)と[`await`](sp5_async.md#async)の混在ができるようになりました。

例えば以下のような書き方で、1秒に1回、整数値を生成するイテレーターになります。

<pre class="source" title="非同期イテレーターの例">
<code><span class="reserved">static</span> <em><span class="reserved">async</span></em> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GenerateAsync</span>()
{
    <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; ; <span class="variable">i</span>++)
    {
        <em><span class="control">yield</span> <span class="control">return</span></em> <span class="variable">i</span>;
        <em><span class="reserved">await</span></em> <span class="type">Task</span>.<span class="method">Delay</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(1));
    }
}
</code></pre>

同期版のイテレーター(`yield`)は以下のような条件を満たすものでした。

- 関数(メソッドなど)の本体の中に`yield return`もしくは`yield break`を含む
- 戻り値の型は `IEnumerable`、`IEnumerator`(`System.Collection`名前空間)、もしくは、`IEnumerable<T>`、`IEnumerator<T>`(`System.Collection.Generic`名前空間)のいずれか

また、非同期メソッドは以下のようなものです。

- メソッドに`async`修飾子が付いている
  - この場合に限り、メソッド内に`await`演算子を書ける

非同期イテレーターはこれらの組み合わせなので、以下のようなものになります。

- 関数(メソッドなど)の本体の中に`yield return`もしくは`yield break`を含む
- 戻り値の型は `IAsyncEnumerable<T>`、`IAsyncEnumerator<T>`(`System.Collection.Generic`名前空間)のいずれか
- メソッドに`async`修飾子が付いている
  - メソッド内に`await`演算子を書ける

### <a id="sec-generated-title-10"></a> <a id="compiled"></a>非同期イテレーターのコンパイル結果

非同期イテレーターの仕組みは、同期版のイテレーターや非同期メソッドの延長線上にあります。
それぞれについては以下のページで説明しています。

- [イテレーターのコンパイル結果](../data/sp2_iterator.md#complied)
- [非同期メソッドの内部実装](sp5_awaitable.md#statemachine)

これら2つは原理的には非常に似ています。
というより、非同期メソッド自体、イテレーターから着想を得て作られた機能です。
なので、パフォーマンス チューニングなど細かい点を除けば、組み合わせることはそれほど難しくはありません。
(ただ、いずれも元々相当複雑なコード生成になるので、
組み合わせた上でパフォーマンスにも配慮すると結構難解なコード生成になります。)

原理だけ簡単に説明すると、非同期イテレーター中に`yield return x`と書くと、
概ね以下のようなコードが生成されます。

<pre class="source" title="yield return の置き換え">
<code>_state = State1;             <span class="comment">// 次に復帰するときのための状態の記録</span>
Current = x;                 <span class="comment">// 戻り値を Current に保持</span>
_taskSource.<span class="method">SetResult</span>(<span class="reserved">true</span>); <span class="comment">// MoveNextAsync の戻り値で返した Task を完了させる</span>
<span class="control">return</span>;                      <span class="comment">// 一旦処理終了</span>
<span class="reserved">case</span>: State1:                <span class="comment">// 時宜に呼ばれたときに続きから処理するためのラベル</span>
</code></pre>

([同期版での説明](../data/sp2_iterator.md#complied)と同様、疑似コードです。実際の C# では case に変数は使えないので、 「これに相当する goto が生成される」くらいのものだと思って読んでください。)

`_taskSource`は、現状の実装では[`ManualResetValueTaskSourceCore`](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.sources.manualresetvaluetasksourcecore-1)という型を使っています。
既存の型で言うと[`TaskCompletionSource<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.taskcompletionsource-1)と似た型というか、用途的には完全に同じで、
パフォーマンス最適化のために導入された構造体です。
(パフォーマンスはいいですが、使い勝手は少し煩雑になります。)

### <a id="sec-generated-title-11"></a> <a id="contextual-keyword"></a>余談: 文脈キーワード

[C# は後方互換性を非常に重要視する言語](../misc/ap_compatibility.md)なので、
`yield`や`await`は[文脈キーワード](../misc/ap_compatibility.md#contextual-keyword)になっています。
例えば、以下のようなコードでは`yield`や`await`がキーワード扱いされず、
普通に変数として使えています。

<pre class="source" title="yield変数とawait変数">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>()
{
    <span class="reserved">var</span> <span class="variable">yield</span> = 2;
    <span class="reserved">var</span> <span class="variable">await</span> = 3;
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">yield</span> * <span class="variable">await</span>);
}
</code></pre>

ただ、この2つは文脈の作り方が異なります。

- `yield`は、`yield return`もしくは`yield break`というように、2単語が並んだ場合だけキーワード扱いされる
  - このキーワードを含んだ時点でイテレーター扱いされる
- `await`は、メソッド自体に`async`修飾子が付いているときだけキーワード扱いされる
  - `async`修飾子は「`await`がキーワードになるかどうか」の目印的な意味しかない

非同期イテレーターの導入にあたって、
方式が異なる2つのものを混ぜることに対する懸念もありました。
`yield`の「含んだ時点でイテレーターになる」というのはコンパイラーにとって結構負担があるらしく、[匿名関数](../functional/fun_localfunctions.md#anonymous-function)をイテレーターに出来ないという問題があったりもします。
そのため、「イテレーターにも`iterator`修飾子みたいなものを足そうか」という話が出たこともあります。
しかし、「同じ用途の別文法」を作ってしまう混乱を起こしてまで実現する課題ではないという判断になり、結局2方式の混在が採用されました。

### <a id="sec-generated-title-12"></a> <a id="EnumeratorCancellation"></a>キャンセル

`IAsyncEnumerable<T>`インターフェイスの`GetAsyncEnumerator`メソッドには`CancellationToken`を渡せるようになっていて、これを使って非同期処理の途中キャンセルをする想定になっています。

非同期イテレーターでは、以下のように、引数に`EnumeratorCancellation`属性(`System.Runtime.CompilerServices`名前空間)を付けることでこの`CancellationToken`を受け取れるようになります。

<pre class="source" title="非同期イテレーターへのCancellationTokenの渡し方">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">cts</span> = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>();
 
        <span class="reserved">var</span> <span class="variable">enumerable</span> = <span class="method">GenerateAsync</span>();
 
        <span class="comment">// ここで引数に渡したトークンが、GenerateAsync の ct 引数にわたる。</span>
        <span class="reserved">var</span> <span class="variable">enumerator</span> = <span class="variable">enumerable</span>.<span class="method">GetAsyncEnumerator</span>(<em><span class="variable">cts</span>.Token</em>);
 
        <span class="comment">// キャンセル前なので値が取れるはず。</span>
        <span class="reserved">await</span> <span class="variable">enumerator</span>.<span class="method">MoveNextAsync</span>();
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">enumerator</span>.Current);
 
        <span class="variable">cts</span>.<span class="method">Cancel</span>();
 
        <span class="comment">// キャンセルしたので止まるはず。</span>
        <span class="control">if</span> (!<span class="reserved">await</span> <span class="variable">enumerator</span>.<span class="method">MoveNextAsync</span>())
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;終了&quot;</span>);
    }
 
    <span class="comment">// キャンセルが掛かるまでずっと、1秒に1個値を生成。</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GenerateAsync</span>([<span class="type">EnumeratorCancellation</span>] <span class="type">CancellationToken</span> <span class="variable">ct</span> = <span class="reserved">default</span>)
    {
        <span class="reserved">var</span> <span class="variable">i</span> = 0;
        <span class="control">while</span> (!<span class="variable">ct</span>.IsCancellationRequested)
        {
            <span class="control">yield</span> <span class="control">return</span> <span class="variable">i</span>;
            <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Delay</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(1));
            ++<span class="variable">i</span>;
        }
    }
}
</code></pre>

ちなみに、非同期`foreach`で使いたい場合、`WithCancellation`拡張メソッドが使えます。
`WithCancellation` の引数で渡した`CancellationToken`が`GetAsyncEnumerator`に伝搬し、
最終的に`GenerateAsync`の`ct`引数に渡ります。

<pre class="source" title="WithCancellation での CancellationToken 伝搬">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="comment">// 5秒後にキャンセルが掛かる。</span>
        <span class="reserved">var</span> <span class="variable">cts</span> = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(5));
 
        <span class="comment">// WithCancellation に渡したトークンが GenerateAsync まで伝搬する。</span>
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="control">in</span> <span class="method">GenerateAsync</span>().<span class="method">WithCancellation</span>(<span class="variable">cts</span>.Token))
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">i</span>);
        }
    }
 
    <span class="comment">// キャンセルが掛かるまでずっと、1秒に1個値を生成。</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GenerateAsync</span>([<span class="type">EnumeratorCancellation</span>] <span class="type">CancellationToken</span> <span class="variable">ct</span> = <span class="reserved">default</span>)
    {
        <span class="reserved">var</span> <span class="variable">i</span> = 0;
        <span class="control">while</span> (!<span class="variable">ct</span>.IsCancellationRequested)
        {
            <span class="control">yield</span> <span class="control">return</span> <span class="variable">i</span>;
            <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Delay</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(1));
            ++<span class="variable">i</span>;
        }
    }
}
</code></pre>

引数越しに受け取る仕様なので、
以下のように、呼び出し側で引数に直接渡すのと、`WithCancellation`越しに渡すので、
2重に`CancellationToken`を渡せます。
この場合、2個のうちどちらか片方でも`Cancel`が掛かった時点でキャンセル扱いになります。
(正確に言うと、[CreateLinkedTokenSource ](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.cancellationtokensource.createlinkedtokensource)を使って新たに作った`CancellationToken`が渡ります。)

<pre class="source" title="CancellationToken が2個渡る例">
<code><span class="comment">// CancellationToken を2個用意。</span>
<span class="reserved">var</span> <span class="variable">ct1</span> = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(3)).Token;
<span class="reserved">var</span> <span class="variable">ct2</span> = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(5)).Token;
 
<span class="comment">// 引数に直接渡せるし、WithCancellation でも渡せる。</span>
<span class="comment">// この場合、どちらか片方でも Cancel された時点でキャンセル扱い。</span>
<span class="comment">// (GenerateAsync には CreateLinkedTokenSource(ct1, ct2) した新しいトークンが渡る。)</span>
<span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="control">in</span> <span class="method">GenerateAsync</span>(<span class="variable">ct1</span>).<span class="method">WithCancellation</span>(<span class="variable">ct2</span>))
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">i</span>);
}
</code></pre>

<!-- original-page-break -->

## <a id="sec-generated-title-13"></a> <a id="usage"></a>利用例

(予定)

具体例いくつか挙げる

[producer/consumer 的なの](https://github.com/dotnet/try/blob/master/Samples/csharp8/ExploreCsharpEight/AsyncStreams.cs)


<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GenerateAsync</span>()
    {
        <span class="reserved">var</span> <span class="variable">r</span> = <span class="reserved">new</span> <span class="type">Random</span>();
 
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; ; <span class="variable">i</span>++)
        {
            <span class="control">yield</span> <span class="control">return</span> <span class="variable">i</span>;
            <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Delay</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(<span class="variable">r</span>.<span class="method">NextDouble</span>()));
        }
    }
 
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">ConsumeAsync</span>(<span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">source</span>)
    {
        <span class="reserved">var</span> <span class="variable">r</span> = <span class="reserved">new</span> <span class="type">Random</span>();
 
        <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">source</span>)
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
            <span class="reserved">await</span> <span class="type">Task</span>.<span class="method">Delay</span>(<span class="type">TimeSpan</span>.<span class="method">FromSeconds</span>(<span class="variable">r</span>.<span class="method">NextDouble</span>()));
        }
    }
}

</code></pre>

LINQ to Object 非同期版
(LINQ は今の実装だとパフォーマンス チューニングの結果イテレーター使わなくなったけど。
少なくとも初期実装はイテレーターだったし、
今でも「パフォーマンスよりもコードのきれいさ重視」だったらイテレーターを使うべき。)

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="type">TResult</span>&gt; <span class="method">GenerateAsync</span>&lt;<span class="type">TSource</span>, <span class="type">TResult</span>&gt;(<span class="type">IAsyncEnumerable</span>&lt;<span class="type">TSource</span>&gt; <span class="variable">source</span>, <span class="type">Func</span>&lt;<span class="type">TSource</span>, <span class="type">TResult</span>&gt; <span class="variable">selector</span>)
{
    <span class="comment">// (お作法的には引数の null チェックすべき)</span>
    <span class="reserved">await</span> <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">source</span>)
    {
        <span class="control">yield</span> <span class="control">return</span> <span class="variable">selector</span>(<span class="variable">x</span>);
    }
}
</code></pre>

非同期にまとまった単位のデータを読んで、データを1つ1つ列挙

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">string</span>&gt; <span class="method">ReadLinesAsync</span>(<span class="reserved">string</span> <span class="variable">directoryPath</span>)
{
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">filePath</span> <span class="control">in</span> <span class="type">Directory</span>.<span class="method">GetFiles</span>(<span class="variable">directoryPath</span>, <span class="string">&quot;*.txt&quot;</span>))
    {
        <span class="reserved">var</span> <span class="variable">lines</span> = <span class="reserved">await</span> <span class="type">File</span>.<span class="method">ReadAllLinesAsync</span>(<span class="variable">filePath</span>);
 
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">line</span> <span class="control">in</span> <span class="variable">lines</span>)
        {
            <span class="control">yield</span> <span class="control">return</span> <span class="variable">line</span>;
        }
    }
}
</code></pre>
