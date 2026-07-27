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

```csharp
// 同名のメソッドを持っていれば型は問わない。
class Awatable
{
    public Awaiter GetAwaiter() { }
}

// 同上、同名のメソッドを持っていれば型は問わない。
struct Awaiter
{
    public bool IsCompleted { get; }
    public void OnCompleted(Action continuation) { }
    public T GetResult() { }
}
```


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

```csharp
public class BlockingAwaitable<T>
{
    private BlockingAwaiter<T> _awaiter;

    public BlockingAwaitable(Task<T> task) { _awaiter = new BlockingAwaiter<T>(task); }

    public BlockingAwaiter<T> GetAwaiter() { return _awaiter; }
}

public class BlockingAwaiter<T>
{
    private Task<T> _task;

    public BlockingAwaiter(Task<T> task) { _task = task; }

    public bool IsCompleted { get { return true; } }

    public void OnCompleted(Action continuation) { }

    public T GetResult()
    {
        _task.Wait();
        return _task.Result;
    }
}

public static class BlockingAwaitableExtensions
{
    public static BlockingAwaitable<T> ToBlocking<T>(this Task<T> task)
    {
        return new BlockingAwaitable<T>(task);
    }
}
```


以下のように利用します。

```csharp
varresult = await task.ToBlocking();
```



## <a id="sec-generated-title-5"></a> <a id="statemachine"></a>状態機械生成

それでは、この awaitable/awaiter が実際にどのように利用されているのかを見てみましょう。
仕組みとしては、「[イテレーター](../data/sp2_iterator.md#iterator)」と似ていて、
一種の状態機械（state machine）の生成となっています。

イテレーターの場合には、yield return の部分が以下のようなコードに置き換えられます。

```csharp
state = State1; // 次に復帰するときのための状態の記録
Current = x;    // 戻り値を Current に保持
return true;    // いったん処理終了
case State1:    // 次に呼ばれたときに続きから処理するためのラベル
```


処理はいったん中断し、次に呼ばれたときには state の値に応じた switch や goto によって、
続きの処理を再開します。

非同期メソッドの場合には、await の部分が以下のようなコードに置き換えられます。

```csharp
state = State1;                  // 次に復帰するときのための状態の記録
var task = RunAsync();
var awaiter = task.GetAwaiter();
if (!awaiter.IsCompleted)
{
    awaiter.OnCompleted(a);      // タスクが未完の場合だけ、継続登録して一度 return
    return;
}
case State1:                     // 次に呼ばれたときに続きから処理するためのラベル
var y = awaiter.GetReslt();      // タスクの結果を受け取り
awaiter = default(T);            // ガベージ コレクションが働きやすくなるように null 代入
```


このコードはラムダ式で囲われていて、
（BeginAwait の引数となっている）Action 型の変数 a に代入されているものと思ってください。
結果として、タスクの継続として自分自身が呼ばれ、state に応じた switch や goto によって続きの処理が行われます。

ちなみに、awaitable/awaiter を介さない単純な実装に展開するなら、以下のようになります。
（実際には、await は Task クラス以外にも使えますし、単純に ContinueWith を呼ぶより少しだけ複雑な処理（後述の SynchronizationContext を利用）を行っています。）

```csharp
state = State1;                  // 次に復帰するときのための状態の記録
var task = AnotherTaskAsync();
if (!task.IsCompleted)
{
    // 他のタスクの完了待ちに入って、いったん処理中止
    task.ContinueWith(a);
    return;
}
// ただし、タスクがすでに完了済みだったら処理続行
case State1:                     // 次に呼ばれたときに続きから処理するためのラベル
var y = task.Result;             // タスクの結果を受け取り
```

##### <a id="sec-generated-title-6"></a>サンプル

（参考： [サンプルの PseudoAsync プロジェクト](http://code.msdn.microsoft.com/C-Async-3185c2e8/sourcecode?itemId=105659)。）

例えば、以下のような非同期メソッドを考えてみましょう。
要は、複数の URL から文字列をダウンロードしてきて表示するプログラムです（ShowTitle の実装については割愛）。

```csharp
private static async void RunTaskAsync(params string[] uriList)
{
    var client = new WebClient();

    foreach (var uri in uriList)
    {
        var html = await client.DownloadStringTaskAsync(uri);
        ShowTitle(html);
    }
}
```


非同期メソッドがイテレーターと似たようなコード生成をしているということは、
イテレーターを使って似たようなことができなくもないです。
上記の例は、イテレーターを使って書くと以下のようになります。

```csharp
private static void RunPseudoAsync(params string[] uriList)
{
    AsyncHelper(RunIterator(uriList));
}

private static IEnumerable<Task> RunIterator(params string[] uriList)
{
    var client = new WebClient();

    foreach (var uri in uriList)
    {
        //↓ここから
        var task = client.DownloadStringTaskAsync(uri);
        if (!task.IsCompleted)
        {
            yield return task;
        }
        var html = task.Result;
        //↑ここまでが await 相当の処理

        ShowTitle(html);
    }

    yield return null;
}

private static void AsyncHelper(IEnumerable<Task> asyncTask)
{
    var e = asyncTask.GetEnumerator();

    Action a = null;

    a = () =>
    {
        if (e.MoveNext() && e.Current != null)
        {
            e.Current.ContinueWith(t => a());
        }
    };

    a();
}
```


さらに、イテレーター相当の処理も展開すると以下のようになります。

```csharp
private static void RunAsyncInside(IEnumerable<string> uriList)
{
    Action a = null;
    var e = uriList.GetEnumerator();
    int state = 0;
    WebClient client = null;
    Task<string> task = null;

    a = () =>
    {
        switch(state)
        {
            case 0: goto State0;
            case 1: goto State1;
        }

        State0:
        client = new WebClient();

        // goto の都合上、ループは if goto とか if return に置き換わる。
        if (!e.MoveNext()) return;

        //↓ここから
        state = 1;
        task = client.DownloadStringTaskAsync(e.Current);
        if (!task.IsCompleted)
        {
            task.ContinueWith(t => a);
            return;
        }
        State1:
        var html = task.Result;
        //↑ここまでが await 相当の処理

        ShowTitle(html);
    };

    a();
}
```

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
```text
・ディスパッチャーを呼ぶ仕組み
WPF とか Silverlight の場合、継続がディスパッチャー経由で呼ばれる。
SynchronizationContext.Post 経由。

（標準提供の TaskAwaiter がこういう挙動してる。
  気に入らなければ Awaiter の自作で回避可能。）

詰まるところ、いくら await しても UI スレッドに処理戻ってくる。
当然、そこで重たい処理したら UI フリーズするので注意。
（一番向いてる処理は、IO 待ち）


・もし、重たい処理が必要なら

await Task.Run(() =>
{
    // 重たい処理
    // ここは別スレッドで動いてる
}

// SynchronizationContext 経由で UI スレッドに戻る

// UI スレッドで実行しないといけない処理

と書く。
```
