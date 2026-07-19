---
title: "非同期メソッドの内部実装"
source_url: "https://ufcpp.net/study/csharp/async/sp5_awaitable/"
content_type: "Article"
published_at: "2011-05-12T00:00:00"
updated_at: "2018-07-06T20:41:43"
tags:
  - "Ver. 5.0"
  - "Ver. 6.0"
umbraco_id: 1335
parent_id: 1326
sort_order: 8
aliases:
  - "/csharp/async/sp5_awaitable/"
  - "/csharp/sp5_awaitable"
  - "/csharp/sp5_awaitable.html"
  - "/study/csharp/sp5_awaitable"
  - "/study/csharp/sp5_awaitable.html"
---

# 非同期メソッドの内部実装

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version5">Ver. 5.0</h5>

C# はこれまでも一貫して、「言語自体（コンパイラー）に多くのことをさせ過ぎない」、
「可能な限りフレームワーク側（クラス ライブラリ側）に実装を任せる」という方針で機能追加を行っています。
例えば、foreach や LINQ の実装がその例ですが、以下のように、コンパイラーの仕事はメソッド呼び出しへの変換になります。

* 「[foreach](../structured/st_loop.md#foreach)」は、enumrable/enumerator パターンに沿って実装されたクラスなら何でも列挙可能。
    * 単純に、GetEnumerator メソッドや MoveNext, Current などの呼び出しに置き換えられる。



* LINQ「[クエリ式](../data/sp3_linq.md#query)」は、Select や Where という名前のメソッドを持っていれば何でも問い合わせ可能。


非同期メソッドも同様の方針を取っていて、
本項で説明するようなパターンに沿ったクラスなら、なんでも await の対象にできます。


##### <a id="sec-generated-title-2"></a>サンプル

* [C# Async の例](http://code.msdn.microsoft.com/C-Async-3185c2e8)

* [EAPをTAP化するラッパー クラスの自動生成](http://code.msdn.microsoft.com/EAPTAP-bb69ab56)



## <a id="sec-generated-title-3"></a> <a id="awaiter"></a>Awaitable パターン

await の対象にできるのは、
以下のような Awaitable パターンを実装したクラスです。
（インターフェイスなどの実装も不要で、いわゆる「[ダックタイピング](../appendix/ap_term.md#ducktype)」的。）

<pre class="source" title="Awaitable パターン" lang="">
<code><span class="comment">// 同名のメソッドを持っていれば型は問わない。</span>
<span class="reserved">class</span> <span class="type">Awatable</span>
{
    <span class="reserved">public</span> <span class="type">Awaiter</span> GetAwaiter() { }
}

<span class="comment">// 同上、同名のメソッドを持っていれば型は問わない。</span>
<span class="reserved">struct</span> <span class="type">Awaiter</span>
{
    <span class="reserved">public bool</span> IsCompleted { <span class="reserved">get</span>; }
    <span class="reserved">public void</span> OnCompleted(<span class="type">Action</span> continuation) { }
    <span class="reserved">public</span> T GetResult() { }
}
</code></pre>


await 可能な型は、上記の Awaitable クラスのように、Awaiter を返す GetAwaiter メソッド（あるいは拡張メソッドでも OK）を持つ必要があります。
Awaiter は、以下のようなプロパティ/メソッドを持つ必要があります。

* <code>
          <span class="reserved">bool</span> IsCompleted
        </code>プロパティ
    * タスクが完了していれば true を返します。 この場合、後述の<code>OnCompleted</code>メソッドで「[継続](misc_continuation.md#key_continuation)」呼び出しするのではなく、 即座に続きの処理を行います。



* <code>
          <span class="reserved">void</span> OnCompleted
        </code>メソッド
    * タスクが未完（<code>IsCompleted</code>が false）な場合、 引数で与えた continuation を「[継続](misc_continuation.md#key_continuation)」登録（例えば Task&lt;T&gt;.ContinueWith に渡す）します。



* <code>T GetResult()</code>
    * タスクの結果を取り出します。

    * 非同期処理の結果が戻り値を持つ場合 （例えば、 タスクがいわゆる「[先物](misc_continuation.md#key_future)」（ジェネリック版の Task&lt;T&gt; など）の場合）、 結果の値を返します。

    * 非同期処理の結果が戻り値なし（void）の場合、 GetResult メソッドの戻り値も void で、 単にタスクの完了を待ちます。

    * タスク内で例外が発生していた場合、GetResult でその例外を受け取れます（スレッド間の例外の伝搬）。




Task クラスなどに直接 IsCompleted/OnCompleted/GetRusult を持たせるのではなく、
GetAwaiter を挟むことで拡張性を持たせています。
GetAwaiter は拡張メソッドでもいいので、独自実装で挙動を変えるということもしやすくなっています。


##### <a id="sec-generated-title-4"></a>サンプル

（参考： [サンプルの AwaiterPatternSample プロジェクト](http://code.msdn.microsoft.com/C-Async-3185c2e8/sourcecode?itemId=105647)。）

実装例を挙げてみましょう。
せっかくの非同期呼び出しを同期化（処理が終わるまでブロッキング）するという、使い道のない実装ですが、
シンプルなのでサンプルとしては分かりやすいと思います。

<pre class="source" title="awaitable/awaiter の実装例" lang="">
<code><span class="reserved">public class</span> <span class="type">BlockingAwaitable</span>&lt;T&gt;
{
    <span class="reserved">private</span> <span class="type">BlockingAwaiter</span>&lt;T&gt; _awaiter;

    <span class="reserved">public</span> BlockingAwaitable(<span class="type">Task</span>&lt;T&gt; task) { _awaiter = <span class="reserved">new</span> <span class="type">BlockingAwaiter</span>&lt;T&gt;(task); }

    <span class="reserved">public</span> <span class="type">BlockingAwaiter</span>&lt;T&gt; GetAwaiter() { <span class="reserved">return</span> _awaiter; }
}

<span class="reserved">public class</span> <span class="type">BlockingAwaiter</span>&lt;T&gt;
{
    <span class="reserved">private</span> <span class="type">Task</span>&lt;T&gt; _task;

    <span class="reserved">public</span> BlockingAwaiter(<span class="type">Task</span>&lt;T&gt; task) { _task = task; }

    <span class="reserved">public bool</span> IsCompleted { <span class="reserved">get</span> { <span class="reserved">return true</span>; } }

    <span class="reserved">public void</span> OnCompleted(<span class="type">Action</span> continuation) { }

    <span class="reserved">public</span> T GetResult()
    {
        _task.Wait();
        <span class="reserved">return</span> _task.Result;
    }
}

<span class="reserved">public static class</span> <span class="type">BlockingAwaitableExtensions</span>
{
    <span class="reserved">public static</span> <span class="type">BlockingAwaitable</span>&lt;T&gt; ToBlocking&lt;T&gt;(<span class="reserved">this</span> <span class="type">Task</span>&lt;T&gt; task)
    {
        <span class="reserved">return new</span> <span class="type">BlockingAwaitable</span>&lt;T&gt;(task);
    }
}
</code></pre>


以下のように利用します。

<pre class="source" title="awaitable/awaiter の実装例" lang="">
<code><span class="reserved">var</span>result = <span class="reserved">await</span> task.ToBlocking();
</code></pre>



## <a id="sec-generated-title-5"></a> <a id="statemachine"></a>状態機械生成

それでは、この awaitable/awaiter が実際にどのように利用されているのかを見てみましょう。
仕組みとしては、「[イテレーター](../data/sp2_iterator.md#iterator)」と似ていて、
一種の状態機械（state machine）の生成となっています。

イテレーターの場合には、yield return の部分が以下のようなコードに置き換えられます。

<pre class="source" title="yield return の置き換え" lang="">
<code>state = State1; <span class="comment">// 次に復帰するときのための状態の記録</span>
Current = x;    <span class="comment">// 戻り値を Current に保持</span>
<span class="reserved">return</span> true;    <span class="comment">// いったん処理終了</span>
<span class="reserved">case</span> State1:    <span class="comment">// 次に呼ばれたときに続きから処理するためのラベル</span>
</code></pre>


処理はいったん中断し、次に呼ばれたときには state の値に応じた switch や goto によって、
続きの処理を再開します。

非同期メソッドの場合には、await の部分が以下のようなコードに置き換えられます。

<pre class="source" title="await の置き換え" lang="">
<code>state = State1;                  <span class="comment">// 次に復帰するときのための状態の記録</span>
<span class="reserved">var</span> task = RunAsync();
<span class="reserved">var</span> awaiter = task.GetAwaiter();
<span class="reserved">if</span> (!awaiter.IsCompleted)
{
    awaiter.OnCompleted(a);      <span class="comment">// タスクが未完の場合だけ、継続登録して一度 return</span>
    <span class="reserved">return</span>;
}
<span class="reserved">case</span> State1:                     <span class="comment">// 次に呼ばれたときに続きから処理するためのラベル</span>
<span class="reserved">var</span> y = awaiter.GetReslt();      <span class="comment">// タスクの結果を受け取り</span>
awaiter = <span class="reserved">default</span>(T);            <span class="comment">// ガベージ コレクションが働きやすくなるように null 代入</span>
</code></pre>


このコードはラムダ式で囲われていて、
（BeginAwait の引数となっている）Action 型の変数 a に代入されているものと思ってください。
結果として、タスクの継続として自分自身が呼ばれ、state に応じた switch や goto によって続きの処理が行われます。

ちなみに、awaitable/awaiter を介さない単純な実装に展開するなら、以下のようになります。
（実際には、await は Task クラス以外にも使えますし、単純に ContinueWith を呼ぶより少しだけ複雑な処理（後述の SynchronizationContext を利用）を行っています。）

<pre class="source" title="awaitable/awaiter を介さず直接 Task を使うなら" lang="">
<code>state = State1;                  <span class="comment">// 次に復帰するときのための状態の記録</span>
<span class="reserved">var</span> task = AnotherTaskAsync();
<span class="reserved">if</span> (!task.IsCompleted)
{
    <span class="comment">// 他のタスクの完了待ちに入って、いったん処理中止</span>
    task.ContinueWith(a);
    <span class="reserved">return</span>;
}
<span class="comment">// ただし、タスクがすでに完了済みだったら処理続行</span>
<span class="reserved">case</span> State1:                     <span class="comment">// 次に呼ばれたときに続きから処理するためのラベル</span>
<span class="reserved">var</span> y = task.Result;             <span class="comment">// タスクの結果を受け取り</span>
</code></pre>

##### <a id="sec-generated-title-6"></a>サンプル

（参考： [サンプルの PseudoAsync プロジェクト](http://code.msdn.microsoft.com/C-Async-3185c2e8/sourcecode?itemId=105659)。）

例えば、以下のような非同期メソッドを考えてみましょう。
要は、複数の URL から文字列をダウンロードしてきて表示するプログラムです（ShowTitle の実装については割愛）。

<pre class="source" title="非同期メソッドの例" lang="">
<code><span class="reserved">private static async void</span> RunTaskAsync(<span class="reserved">params string</span>[] uriList)
{
    <span class="reserved">var</span> client = <span class="reserved">new</span> <span class="type">WebClient</span>();

    <span class="reserved">foreach</span> (<span class="reserved">var</span> uri <span class="reserved">in</span> uriList)
    {
        <span class="reserved">var</span> html = <span class="reserved">await</span> client.DownloadStringTaskAsync(uri);
        ShowTitle(html);
    }
}
</code></pre>


非同期メソッドがイテレーターと似たようなコード生成をしているということは、
イテレーターを使って似たようなことができなくもないです。
上記の例は、イテレーターを使って書くと以下のようになります。

<pre class="source" title="イテレーターを使って疑似的に非同期メソッド" lang="">
<code><span class="reserved">private static void</span> RunPseudoAsync(<span class="reserved">params string</span>[] uriList)
{
    AsyncHelper(RunIterator(uriList));
}

<span class="reserved">private static</span> <span class="type">IEnumerable</span>&lt;<span class="type">Task</span>&gt; RunIterator(<span class="reserved">params string</span>[] uriList)
{
    <span class="reserved">var</span> client = <span class="reserved">new</span> <span class="type">WebClient</span>();

    <span class="reserved">foreach</span> (<span class="reserved">var</span> uri <span class="reserved">in</span> uriList)
    {
        <span class="comment">//↓ここから</span>
        <span class="reserved">var</span> task = client.DownloadStringTaskAsync(uri);
        <span class="reserved">if</span> (!task.IsCompleted)
        {
            <span class="reserved">yield return</span> task;
        }
        <span class="reserved">var</span> html = task.Result;
        <span class="comment">//↑ここまでが await 相当の処理</span>

        ShowTitle(html);
    }

    <span class="reserved">yield return null</span>;
}

<span class="reserved">private static void</span> AsyncHelper(<span class="type">IEnumerable</span>&lt;<span class="type">Task</span>&gt; asyncTask)
{
    <span class="reserved">var</span> e = asyncTask.GetEnumerator();

    <span class="type">Action</span> a = <span class="reserved">null</span>;

    a = () =&gt;
    {
        <span class="reserved">if</span> (e.MoveNext() &amp;&amp; e.Current != <span class="reserved">null</span>)
        {
            e.Current.ContinueWith(t =&gt; a());
        }
    };

    a();
}
</code></pre>


さらに、イテレーター相当の処理も展開すると以下のようになります。

<pre class="source" title="非同期メソッドの展開結果" lang="">
<code><span class="reserved">private static void</span> RunAsyncInside(<span class="type">IEnumerable</span>&lt;<span class="reserved">string</span>&gt; uriList)
{
    <span class="type">Action</span> a = <span class="reserved">null</span>;
    <span class="reserved">var</span> e = uriList.GetEnumerator();
    <span class="reserved">int</span> state = 0;
    <span class="type">WebClient</span> client = <span class="reserved">null</span>;
    <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; task = <span class="reserved">null</span>;

    a = () =&gt;
    {
        <span class="reserved">switch</span>(state)
        {
            <span class="reserved">case</span> 0: <span class="reserved">goto</span> State0;
            <span class="reserved">case</span> 1: <span class="reserved">goto</span> State1;
        }

        State0:
        client = <span class="reserved">new</span> <span class="type">WebClient</span>();

        <span class="comment">// goto の都合上、ループは if goto とか if return に置き換わる。</span>
        <span class="reserved">if</span> (!e.MoveNext()) <span class="reserved">return</span>;

        <span class="comment">//↓ここから</span>
        state = 1;
        task = client.DownloadStringTaskAsync(e.Current);
        <span class="reserved">if</span> (!task.IsCompleted)
        {
            task.ContinueWith(t =&gt; a);
            <span class="reserved">return</span>;
        }
        State1:
        <span class="reserved">var</span> html = task.Result;
        <span class="comment">//↑ここまでが await 相当の処理</span>

        ShowTitle(html);
    };

    a();
}
</code></pre>

### <a id="sec-generated-title-7"></a> <a id="catch-finally"></a>catch句、finally句内でのawait

<h5 class="version version6">Ver. 6</h5>

C# 6からは、catch句、finally句内にも`await`を書けるようになりました。

これの展開は結構面倒で、ここまでで説明してきたような単純な置き替えルールではできません。追加で、以下のようなことをしています。

- すべての例外を無差別にcatch
- catch句内、finally句内相当の処理を実行
- 例外を再throw

最後の例外の再throwが曲者で、例外の[スタック トレース](../structured/misc_stacktrace.md)を保ったまま例外をthrowし直すのは結構難しかったりします(.NET Frameworkの内部的な機能(internalなメソッド)を使わないとできなかったりします)。



## <a id="sec-generated-title-8"></a> <a id="synchronization"></a>同期コンテキスト

（書きかけ）

（参考： [サンプルの SynchronizationContextSample プロジェクト](http://code.msdn.microsoft.com/C-Async-3185c2e8/sourcecode?itemId=105663)。）

GUI アプリの場合、UI を更新できるのは UI スレッドだけ。
非同期処理の結果を UI スレッドに返す必要あり。
参考: 「[[雑記] GUI と非同期処理](misc_uithread.md)」
<pre>
・ディスパッチャーを呼ぶ仕組み
WPF とか Silverlight の場合、継続がディスパッチャー経由で呼ばれる。
SynchronizationContext.Post 経由。

（標準提供の TaskAwaiter がこういう挙動してる。
  気に入らなければ Awaiter の自作で回避可能。）

詰まるところ、いくら await しても UI スレッドに処理戻ってくる。
当然、そこで重たい処理したら UI フリーズするので注意。
（一番向いてる処理は、IO 待ち）


・もし、重たい処理が必要なら

await Task.Run(() =&gt;
{
    // 重たい処理
    // ここは別スレッドで動いてる
}

// SynchronizationContext 経由で UI スレッドに戻る

// UI スレッドで実行しないといけない処理

と書く。
</pre>
