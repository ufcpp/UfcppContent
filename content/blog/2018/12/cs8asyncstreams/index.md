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

```csharp
// 非同期 foreach … IAsyncEnumerable からの列挙
static async Task AsyncForeach(IAsyncEnumerable<int> items)
{
    await foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
```

これまでの`await`同様、これが書けるのは非同期メソッド(`async`修飾付きのメソッド)内だけです。

こいつは、同期版の`foreach`と似たような感じで、以下のように展開されます。
同期版と比べて、`MoveNext`と`Dispose`が非同期になっただけです。

```csharp
private static async Task AsyncForeach(IAsyncEnumerable<int> items)
{
    IAsyncEnumerator<int> e = items.GetAsyncEnumerator();
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

## 非同期イテレーター

続いて生成側の非同期イテレーター。
要は、`await`と`yield`を混在できる機能です。

非同期メソッドと同様に `async`修飾が必須で、
戻り値は`IAsyncEnumerable<T>`である必要があります。

```csharp
// 非同期イテレーター … await/yield混在
static async IAsyncEnumerable<int> AsyncIterator()
{
    await Task.Delay(1);
    yield return 1;
    await Task.Delay(1);
    yield return 2;
}
```

非同期イテレーターから生成されるコードは、
やっぱり[同期版のイテレーター](../../../../study/csharp/data/sp2_iterator.md#complied)と[非同期メソッド](../../../../study/csharp/async/sp5_awaitable.md)を組み合わせたようなコードになります。
イテレーターも非同期メソッド元々結構複雑なので、非同期イテレーターはもっと複雑です。

後述するバグのせいで今のところコンパイルが通らないので、詳細はバグが治ったら(Preview 2？)改めて書こうかと思います。

## IAsyncEnumerable

非同期`foreach`でも非同期イテレーターでも、`IAsyncEnumerable<T>`インターフェイス(`System.Collections.Generic`名前空間)が出てきます。
これも、割と素直に「`IEnumerable<T>`の非同期版」という感じのインターフェイスになりました。

以下のようなインターフェイスになる予定です。
(割かし最近変更があって、Preview 1 の時点では `CancellationToken` を受け取る引数がまだないです。)

```csharp
using System.Threading;
using System.Threading.Tasks;
 
namespace System.Collections.Generic
{
    public interface IAsyncEnumerable<out T>
    {
        IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
    }
    public interface IAsyncEnumerator<out T> : IAsyncDisposable
    {
        T Current { get; }
        ValueTask<bool> MoveNextAsync();
    }
}
```

[前にちょっと書きましたが](../../10/pickuproslyn1014/index.md)、
以下のような構造もちょっと検討されました。

```csharp
public interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    ValueTask<bool> WaitForNextAsync();
    T TryGetNext(out bool success);
}
```

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
