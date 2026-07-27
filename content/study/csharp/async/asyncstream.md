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
aliases: []
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

```csharp
public interface IAsyncEnumerable<out T>
{
    IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
}
 
public interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    T Current { get; }
    ValueTask<bool> MoveNextAsync();
}
 
public interface IAsyncDisposable
{
    ValueTask DisposeAsync();
}
```

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

```csharp
static async Task AsyncForeach(IAsyncEnumerable<int> items)
{
    await foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
```

[`await`](sp5_async.md#async)演算子と同じく、
非同期メソッド(`async` 修飾が付いたメソッド)内でだけ使えます。

このコードは、同期版の`foreach`と似たような感じで、以下のように展開されます。 同期版と比べて、`MoveNext`と`Dispose`が非同期になっただけです。

```csharp
static async Task AsyncForeach(IAsyncEnumerable<int> items)
{
    var e = items.GetAsyncEnumerator();
    try
    {
        while (await e.MoveNextAsync())
        {
            int item = e.Current;
            Console.WriteLine(item);
        }
    }
    finally
    {
        if (e != null)
        {
            await e.DisposeAsync();
        }
    }
}
```

同期版と同じく、`finally`内の処理にはいくつかバリエーションがあります。

- enumerator (上記の例で言う `e`)が構造体なら null チェックは挟まらない
- `DisposeAsync` を持っていない場合は`finally`内の処理自体消える

### <a id="sec-generated-title-4"></a> <a id="pattern-based-await-foreach"></a>パターン ベース

パターン ベースなので、インターフェイスを実装していなくても、
所定のメソッドさえ持っていれば非同期`foreach`で使えます。
以下はその一例です。

```csharp
using System;
using System.Threading.Tasks;
 
struct A
{
    // このメソッドが「Enumerable」の必須要件。
    // この例では自分自身を返している(それでもOK)ものの、通常は別の型を作って返す。
    public A GetAsyncEnumerator() => this;
 
    // 以下の2つが「Enumerator」の必須要件。
    public int Current => 0;
    public ValueTask<bool> MoveNextAsync()
    {
        Console.WriteLine("MoveNextAsync");
        return new ValueTask<bool>(false);
    }
 
    // DisposeAsync はなくてもいい。なければ呼ばれないだけ。
    public ValueTask DisposeAsync()
    {
        Console.WriteLine("DisposeAsync");
        return default;
    }
 
    // 同期の Dispose は定義してあっても呼ばれないので注意。
}
 
public class Program
{
    public static async Task Main()
    {
        await foreach (var x in new A()) ;
    }
}
```

この例では`ValueTask`型を使っていますが、これすらもパターン ベースで大丈夫です。
要は、`await`可能であれば型は問いません。
また、[通常の `foreach` と同じく](../data/sp_foreach.md#extension-getenumerator)、C# 9.0 から拡張メソッドも受け付けるようになりました。

また、後から追加された構文だけあって、同期版の`foreach`よりもパターンの条件が緩いです。以下のように、オプション引数や可変長引数が付いていても平気です(同期版はダメ)。

```csharp
using System.Threading;
using System.Threading.Tasks;
 
struct A
{
    // 可変長引数があってもいい
    public A GetAsyncEnumerator(params int[] dummy) => this;
 
    public int Current => 0;
 
    // オプション引数があってもいい。
    public ValueTask<bool> MoveNextAsync(CancellationToken token = default) => default;
}
 
public class Program
{
    public static async Task Main()
    {
        await foreach (var x in new A()) ;
    }
}
```

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

```csharp
static async Task AsyncUsing(IAsyncDisposable d)
{
    await using (d)
    {
        // d を破棄する前にやっておきたい処理
    }
}
```

これも非同期`foreach`と同様に、非同期メソッド(async 修飾が付いたメソッド)内でだけ使えます。

展開結果は、同期版で`Dispose()`呼び出しだった部分が`await DisposeAsync()`に変わっているだけです。
上記のコードは以下のように展開されます。

```csharp
static async Task AsyncUsing(IAsyncDisposable d)
{
    try
    {
        // d を破棄する前にやっておきたい処理
    }
    finally
    {
        await d.DisposeAsync();
    }
}
```

### <a id="sec-generated-title-6"></a> <a id="pattern-based-await-using"></a>パターン ベース

「[パターン ベースな構文](../misc/miscpatternbased.md#converse)」で説明していますが、同期版の`using`は数少ない「インターフェイス実装が必須な構文」です。

一方、非同期`using`はパターン ベースになっています。
以下のように、`IAsyncDisposable`インターフェイスを実装せず、
単に`DisposeAsync`メソッドを持っていれば`await using`で使えます。

```csharp
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
 
// 非同期 using は別に IAsyncDisposable インターフェイスの実装を求めない。
class AsyncDisposable
{
    // ちゃんと await using のブロックの最後で呼ばれる。
    // 戻り値の型が Task や ValueTask である必要もない。
    public MyAwaitable DisposeAsync()
    {
        Console.WriteLine("disposed async");
        return default;
    }
}
 
struct MyAwaitable { public ValueTaskAwaiter GetAwaiter() => default; }
 
class Program
{
    static async Task Main()
    {
        await using (new AsyncDisposable())
        {
            Console.WriteLine("inside using");
        }
    }
}
```

見ての通り、`DisposeAsync`の戻り値は`await`可能でさえあれば何でも構いません。

また、オプション引数や可変長引数があっても構いません。

```csharp
using System.Threading.Tasks;
 
struct A
{
    public ValueTask DisposeAsync(int dummy = 0) => default;
}
 
struct B
{
    public ValueTask DisposeAsync(params int[] dummy) => default;
}
 
public class Program
{
    public static async Task Main()
    {
        await using (new A()) { }
        await using (new B()) { }
    }
}
```

制限と言えば、インスタンス メソッドしか受け付けない(拡張メソッドは使えない)くらいです。

一方で、同期版と違って、[`as`演算子](../oop/misc_as.md)を使った動的な型チェックはしません。
以下のように、直接的には`IAsyncDisposable`インターフェイスを実装していなくて、
パターンも満たさない型に対して`await using`を使うとコンパイル エラーになります。

```csharp
using System;
using System.Threading.Tasks;
 
class A { }
 
class B : A, IAsyncDisposable
{
    public ValueTask DisposeAsync() => default;
}
 
public class Program
{
    public static async Task Main()
    {
        // A は IAsyncDisposable じゃないけど、
        // 派生クラスの B は IAsyncDisposable を実装。
        await AsyncUsing(new B());
    }
 
    static async Task AsyncUsing(A a)
    {
        // これはコンパイル エラーになる。
        // A が直接 IAsyncDisposable を実装しているか、パターンを満たしている必要がある。
        await using (a) { }
    }
}
```

ジェネリック型引数に対して使う場合にも、`IAsyncDisposable`制約が必要になります。

```csharp
static async Task M<T>(T x)
    where T : IAsyncDisposable // この制約がないと await using の行でコンパイル エラーに。
{
    await using (x) { }
}
```

### <a id="sec-generated-title-7"></a> <a id="await-using-declaration"></a>using変数宣言との併用

[`using`変数宣言](../resource/oo_dispose.md#using-declaration)との併用も可能です。
以下のような書き方ができます。

```csharp
using System.Threading.Tasks;
 
struct AsyncDisposable
{
    public ValueTask DisposeAsync() => default;
}
 
public class Program
{
    public static async Task Main()
    {
        await using var x = new AsyncDisposable();
 
        // このメソッドを抜けるタイミングで DisposeAsync が呼ばれる
    }
}
```

### <a id="sec-generated-title-8"></a> <a id="sync-and-async"></a>DisposeとDisposeAsyncの混在

ちなみに、`Dispose`(`IDisposable`インターフェイス)と`DisposeAsync`(`IAsyncDisposable`インターフェイス)の両方を実装をしている場合、それぞれ同期版`using`、非同期`using`でしか呼ばれません。
非同期版が同期版を兼ねたりはしませんし、その逆もまたしかり。

以下の例では、`using`の行では`Dispose`だけが呼ばれますし、
`await using`の行では`DisposeAsync`だけが呼ばれます。

```csharp
using System;
using System.Threading.Tasks;
 
struct Disposable : IDisposable, IAsyncDisposable
{
    public void Dispose() => Console.WriteLine("同期 Dispose");
    public ValueTask DisposeAsync()
    {
        Console.WriteLine("非同期 Dispose");
        return default;
    }
}
 
public class Program
{
    public static async Task Main()
    {
        var d = new Disposable();
 
        // Dispose だけが呼ばれる
        using (d) { }
 
        // DisposeAsync だけが呼ばれる
        await using (d) { }
    }
}
```

## <a id="sec-generated-title-9"></a> <a id="async-iterator"></a>非同期イテレーター

[非同期`foreach`](#await-foreach)とは逆の、データの生成側の機能が非同期イテレーターです。
簡単に言うと、[`yield`](../data/sp2_iterator.md#block)と[`await`](sp5_async.md#async)の混在ができるようになりました。

例えば以下のような書き方で、1秒に1回、整数値を生成するイテレーターになります。

```csharp
static async IAsyncEnumerable<int> GenerateAsync()
{
    for (int i = 0; ; i++)
    {
        yield return i;
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
```

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

```csharp
_state = State1;             // 次に復帰するときのための状態の記録
Current = x;                 // 戻り値を Current に保持
_taskSource.SetResult(true); // MoveNextAsync の戻り値で返した Task を完了させる
return;                      // 一旦処理終了
case: State1:                // 時宜に呼ばれたときに続きから処理するためのラベル
```

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

```csharp
static void M()
{
    var yield = 2;
    var await = 3;
    Console.WriteLine(yield * await);
}
```

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

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
 
public class Program
{
    static async Task Main()
    {
        var cts = new CancellationTokenSource();
 
        var enumerable = GenerateAsync();
 
        // ここで引数に渡したトークンが、GenerateAsync の ct 引数にわたる。
        var enumerator = enumerable.GetAsyncEnumerator(cts.Token);
 
        // キャンセル前なので値が取れるはず。
        await enumerator.MoveNextAsync();
        Console.WriteLine(enumerator.Current);
 
        cts.Cancel();
 
        // キャンセルしたので止まるはず。
        if (!await enumerator.MoveNextAsync())
            Console.WriteLine("終了");
    }
 
    // キャンセルが掛かるまでずっと、1秒に1個値を生成。
    static async IAsyncEnumerable<int> GenerateAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        var i = 0;
        while (!ct.IsCancellationRequested)
        {
            yield return i;
            await Task.Delay(TimeSpan.FromSeconds(1));
            ++i;
        }
    }
}
```

ちなみに、非同期`foreach`で使いたい場合、`WithCancellation`拡張メソッドが使えます。
`WithCancellation` の引数で渡した`CancellationToken`が`GetAsyncEnumerator`に伝搬し、
最終的に`GenerateAsync`の`ct`引数に渡ります。

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
 
public class Program
{
    static async Task Main()
    {
        // 5秒後にキャンセルが掛かる。
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
 
        // WithCancellation に渡したトークンが GenerateAsync まで伝搬する。
        await foreach (var i in GenerateAsync().WithCancellation(cts.Token))
        {
            Console.WriteLine(i);
        }
    }
 
    // キャンセルが掛かるまでずっと、1秒に1個値を生成。
    static async IAsyncEnumerable<int> GenerateAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        var i = 0;
        while (!ct.IsCancellationRequested)
        {
            yield return i;
            await Task.Delay(TimeSpan.FromSeconds(1));
            ++i;
        }
    }
}
```

引数越しに受け取る仕様なので、
以下のように、呼び出し側で引数に直接渡すのと、`WithCancellation`越しに渡すので、
2重に`CancellationToken`を渡せます。
この場合、2個のうちどちらか片方でも`Cancel`が掛かった時点でキャンセル扱いになります。
(正確に言うと、[CreateLinkedTokenSource ](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.cancellationtokensource.createlinkedtokensource)を使って新たに作った`CancellationToken`が渡ります。)

```csharp
// CancellationToken を2個用意。
var ct1 = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
var ct2 = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
 
// 引数に直接渡せるし、WithCancellation でも渡せる。
// この場合、どちらか片方でも Cancel された時点でキャンセル扱い。
// (GenerateAsync には CreateLinkedTokenSource(ct1, ct2) した新しいトークンが渡る。)
await foreach (var i in GenerateAsync(ct1).WithCancellation(ct2))
{
    Console.WriteLine(i);
}
```

<!-- original-page-break -->

## <a id="sec-generated-title-13"></a> <a id="usage"></a>利用例

(予定)

具体例いくつか挙げる

[producer/consumer 的なの](https://github.com/dotnet/try/blob/master/Samples/csharp8/ExploreCsharpEight/AsyncStreams.cs)


```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 
public class Program
{
    static async IAsyncEnumerable<int> GenerateAsync()
    {
        var r = new Random();
 
        for (int i = 0; ; i++)
        {
            yield return i;
            await Task.Delay(TimeSpan.FromSeconds(r.NextDouble()));
        }
    }
 
    static async Task ConsumeAsync(IAsyncEnumerable<int> source)
    {
        var r = new Random();
 
        await foreach (var x in source)
        {
            Console.WriteLine(x);
            await Task.Delay(TimeSpan.FromSeconds(r.NextDouble()));
        }
    }
}
```

LINQ to Object 非同期版
(LINQ は今の実装だとパフォーマンス チューニングの結果イテレーター使わなくなったけど。
少なくとも初期実装はイテレーターだったし、
今でも「パフォーマンスよりもコードのきれいさ重視」だったらイテレーターを使うべき。)

```csharp
static async IAsyncEnumerable<TResult> GenerateAsync<TSource, TResult>(IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
{
    // (お作法的には引数の null チェックすべき)
    await foreach (var x in source)
    {
        yield return selector(x);
    }
}
```

非同期にまとまった単位のデータを読んで、データを1つ1つ列挙

```csharp
static async IAsyncEnumerable<string> ReadLinesAsync(string directoryPath)
{
    foreach (var filePath in Directory.GetFiles(directoryPath, "*.txt"))
    {
        var lines = await File.ReadAllLinesAsync(filePath);
 
        foreach (var line in lines)
        {
            yield return line;
        }
    }
}
```
