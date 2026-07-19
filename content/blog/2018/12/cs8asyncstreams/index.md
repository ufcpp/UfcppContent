---
title: "C# 8.0 Async streams"
source_url: "https://ufcpp.net/blog/2018/12/cs8asyncstreams/"
content_type: "BlogEntry"
published_at: "2018-12-11T07:54:51"
updated_at: "2018-12-11T22:10:06"
tags: []
umbraco_id: 2192
parent_id: 2177
sort_order: 10
aliases: []
---

# C# 8.0 Async streams

一応、Preview 1で実装されてはいるんですが、ちょっと不具合があって動かない機能が1つあったりします。

非同期ストリーム(async streams)と呼ばれていて、具体的には以下の2つの機能からなります。

- 非同期イテレーター … 戻り値を`IAsyncEnumerable<T>`インターフェイスにすることで、`await`と`yield`を混在させることができる
- 非同期 `foreach` … `await foreach`という書き方で、`IAsyncEnumerable<T>`から値を列挙できる

要は、一連のデータ(data stream)を、非同期に生成(イテレーター)して非同期に消費(foreach)する機能です。

## 非同期 foreach

消費側の方が簡単なので先に非同期 `foreach` の方を。
`IEnumerable<T>`の非同期版である`IAsyncEnumerable<T>`に対して要素の列挙ができる機能です。
(実際には同名のメソッドを持っていればインターフェイスの実装は不問なところも、同期版`foreach`と一緒。)

文法の候補は `async foreach`、`foreach async`、`foreach await`など他にもあったんですが、
現状は以下のような`await foreach`が採用されました。

<pre class="source" title="">
<code><span class="comment">// 非同期 foreach … IAsyncEnumerable からの列挙</span>
<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> AsyncForeach(<span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; items)
{
    <span class="reserved">await</span> <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> items)
    {
        <span class="type">Console</span>.WriteLine(item);
    }
}
</code></pre>

これまでの`await`同様、これが書けるのは非同期メソッド(`async`修飾付きのメソッド)内だけです。

こいつは、同期版の`foreach`と似たような感じで、以下のように展開されます。
同期版と比べて、`MoveNext`と`Dispose`が非同期になっただけです。

<pre class="source" title="">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> AsyncForeach(<span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; items)
{
    <span class="type">IAsyncEnumerator</span>&lt;<span class="reserved">int</span>&gt; e = items.GetAsyncEnumerator();
    <span class="reserved">try</span>
    {
        <span class="reserved">while</span> (<span class="reserved">await</span> e.MoveNextAsync())
        {
            <span class="reserved">int</span> item = e.Current;
            <span class="type">Console</span>.WriteLine(item);
        }
    }
    <span class="reserved">finally</span>
    {
        <span class="reserved">if</span> (e != <span class="reserved">null</span>)
        {
            <span class="reserved">await</span> e.DisposeAsync();
        }
    }
}

</code></pre>

## 非同期イテレーター

続いて生成側の非同期イテレーター。
要は、`await`と`yield`を混在できる機能です。

非同期メソッドと同様に `async`修飾が必須で、
戻り値は`IAsyncEnumerable<T>`である必要があります。

<pre class="source" title="">
<code><span class="comment">// 非同期イテレーター … await/yield混在</span>
<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; AsyncIterator()
{
    <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
    <span class="reserved">yield</span> <span class="reserved">return</span> 1;
    <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
    <span class="reserved">yield</span> <span class="reserved">return</span> 2;
}
</code></pre>

非同期イテレーターから生成されるコードは、
やっぱり[同期版のイテレーター](../../../../study/csharp/data/sp2_iterator.md#complied)と[非同期メソッド](../../../../study/csharp/async/sp5_awaitable.md)を組み合わせたようなコードになります。
イテレーターも非同期メソッド元々結構複雑なので、非同期イテレーターはもっと複雑です。

後述するバグのせいで今のところコンパイルが通らないので、詳細はバグが治ったら(Preview 2？)改めて書こうかと思います。

## IAsyncEnumerable

非同期`foreach`でも非同期イテレーターでも、`IAsyncEnumerable<T>`インターフェイス(`System.Collections.Generic`名前空間)が出てきます。
これも、割と素直に「`IEnumerable<T>`の非同期版」という感じのインターフェイスになりました。

以下のようなインターフェイスになる予定です。
(割かし最近変更があって、Preview 1 の時点では `CancellationToken` を受け取る引数がまだないです。)

<pre class="source" title="">
<code><span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">namespace</span> System.Collections.Generic
{
    <span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;
    {
        <span class="type">IAsyncEnumerator</span>&lt;<span class="type">T</span>&gt; GetAsyncEnumerator(<span class="type">CancellationToken</span> cancellationToken = <span class="reserved">default</span>);
    }
    <span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt; : <span class="type">IAsyncDisposable</span>
    {
        <span class="type">T</span> Current { <span class="reserved">get</span>; }
        <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; MoveNextAsync();
    }
}
</code></pre>

[前にちょっと書きましたが](../../10/pickuproslyn1014/index.md)、
以下のような構造もちょっと検討されました。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAsyncEnumerator</span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt; : <span class="type">IAsyncDisposable</span>
{
    <span class="type">ValueTask</span>&lt;<span class="reserved">bool</span>&gt; WaitForNextAsync();
    <span class="type">T</span> TryGetNext(<span class="reserved">out</span> <span class="reserved">bool</span> success);
}
</code></pre>

こちらの没案の方が、うまく使えばパフォーマンスがよくなります。
ただ、ちょっと使いにくい構造なので、ちょっと複雑なことをしようと思うと、パフォーマンスの良いコードを書くのが結構大変になったりします。
なので、「シンプルさにこだわりたい」とのことで、結局、現在の素直な構造になったみたいです。

## Preview 1 でのバグ

非同期 foreach の方はPreview 1でも問題なく動きます。
一方で、非同期イテレーターの方は、文法上はエラーなく解釈できるんですが、
実行ファイルを生成する段階で「`ManualResetValueTaskSourceLogic`構造体が存在しない」というエラーを起こします。

どうも、Preview 1としてリリースするブランチが、Roslyn側とcoreclr側で食い違っているみたいです。
非同期イテレーターが内部的に使う型があって、
その型の仕様は最近ちょっと変更されています。
元々は`ManualResetValueTaskSourceLogic`という名前で実装されていたんですが、
名前も`ManualResetValueTaskSourceCore`に変更されました。
そして、Roslynの方は変更前のままで、corefxの方は変更後のブランチでPreview 1をリリースしてしまったみたいです。

ソースコードを取ってきて名前だけ"Logic"に戻して動くなら良かったんですが、
ちょっと実装も変わっていて、無理やり動かすのもそこそこ面倒そうでした。
まあ、Preview 2では治っていると思うので、治ったら本気出します。
