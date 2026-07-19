---
title: "小ネタ 同期コンテキストを拾わないTask型"
source_url: "https://ufcpp.net/blog/2016/12/tipscontextfreetask/"
content_type: "BlogEntry"
published_at: "2016-12-09T00:16:14"
updated_at: "2016-12-27T14:32:41"
tags: []
umbraco_id: 1988
parent_id: 1969
sort_order: 8
aliases: []
---

# 小ネタ 同期コンテキストを拾わないTask型

今日も小ネタというかC# 7ネタというか、C# 7に合わせて1個ライブラリ書いたというか。

勢いで、こんなライブラリ1個作ってしまいました。
C# 7向けです(半分くらいはC# 5.0でも使えるものですが)。

- [ContextFreeTask](https://github.com/ufcpp/ContextFreeTask)

ということで、C# 7の機能の1つについて説明。
C# 7で以下のような機能が入ります。

- [非同期メソッドの戻り値に任意の型を使えるように](../../../../study/csharp/cheatsheet/ap_ver7.md#tasklike)

ほぼ、`ValueTask`のために入った機能なんですが、まあ、せっかくなので他でも使ってみようというのが今日の話。

## 同期コンテキスト

C#に限らずいろんなプログラミング言語で、非同期処理の後にメイン スレッドに戻ってこないといけないという制約があったりします。
特に、GUIプログラムの開発環境だとたいてい、UIがらみのクラスはメイン スレッド(UI末ラッド)からしか触れないとかそういう制約があります。

こういう、「メイン スレッドに戻らないといけない」とか、その場その場にある文脈を同期コンテキストと言います。
C#で`await`を使って非同期処理をする場合、
`await`した時点で同期コンテキストを持っていたら、
それを拾って元のコンテキストに戻ってくるようになっています。

同期コンテキストに関する説明、参考URLを探そうとしたものの…
意外とこの時期、真面目に自分のサイトを更新してなくて「書きかけ」ばっかり…
一番真面目に書いてあるのが@ITで書いたSilverlightの記事という…
(XamarinとかASP.NET Coreあたりで書き直したい気もしつつ。)

- [避けて通れない「非同期処理」を克服しよう](http://www.atmarkit.co.jp/fdotnet/chushin/introsl_04/introsl_04_02.html)
- [[雑記] GUI と非同期処理](../../../../study/csharp/async/misc_uithread.md)

とにかく、C#には同期コンテキストってものがあって、通常、`await`するとそのコンテキスト拾って、スレッド プールからメイン スレッドとかに自動的に戻ってきてくれる仕組みが入っています。

### コンテキスト拾いすぎ

とはいえ、これはアプリのレイヤーのためにある機能であって、
逆に、ライブラリの中でコンテキストを拾っちゃうとまずかったりします。
意図しないタイミングでメイン スレッドを止めてしまって、デッドロックを起こしたりします。

ということで、ライブラリ作者は、同期コンテキストを拾わないようにするために、以下のようなコードを書くことを強要されます。

```csharp
// ConfigureAwait で同期コンテキストを拾うかどうか設定できる
// 引数を false にすると拾わない
await FAsync().ConfigureAwait(false);
```

ライブラリを書く側の人は毎度毎度、これで苦労します。
正直に言って結構うざい…

## コンテキストを拾わない Task

ってことで作ったのが `ContextFreeTask`。
コンテキストを拾わない`Task`です。

冒頭の通り、C# 7では非同期メソッドの戻り値の型を任意に変えれるようになったので、自作してみました。

```csharp
// Task の代わりに ContextFreeTask を非同期メソッドの戻り値にできる
// この中にある await は同期コンテキストを一切拾わない
private async ContextFreeTask FAsync()
{
    // この時点でどんなコンテキストで動いていようと…
    await Task.Delay(100);
    // コンテキストは拾われないので、元のコンテキストには戻らない
    await Task.Delay(100);
    // 同上、戻らない
}

private async Task GAsync()
{
    // ContextFreeTask に対する await もできる
    // この await も同期コンテキストを拾わない

    await FAsync();
    // コンテキストは拾われない
}
```

概ね、以下のコードと同じ挙動になります。

```csharp
private async ContextFreeTask FAsync()
{
    await Task.Delay(100).ConfigureAwait(false);
    await Task.Delay(100).ConfigureAwait(false);
}

private async Task GAsync()
{
    await FAsync().ConfigureAwait(false);
}
```

戻り値があるとき用、すなわち、`Task<TResult>` の代わりの `ContextFreeTask<T>` もあります。

```csharp
private async ContextFreeTask<string> HAsync(int n)
{
    await Task.Delay(100);
    return n.ToString();
}
```

### 中身

`Task` 1個だけ持つ薄いラッパー構造体で、
ほとんどの処理は`Task`や、そのawaiter、async method builderへの丸投げです。
その手前に`ConfigureAwait(false)`や`SetSynchronizationContext(null);`を挟んでいるだけ。

```csharp
public struct ContextFreeTask<T>
{
    public Task<T> Task { get; }
}

public struct ContextFreeTaskAwaiter : ICriticalNotifyCompletion
{
    private readonly Task _value;
    public void OnCompleted(Action continuation) => _value.ConfigureAwait(false).GetAwaiter().OnCompleted(continuation);
}

public struct AsyncContextFreeTaskMethodBuilder
{
    private AsyncTaskMethodBuilder _methodBuilder;

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        SynchronizationContext.SetSynchronizationContext(null);
        _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
    }
}
```

## やってみて

とりあえず小ネタというか、単にライブラリ紹介だったわけですが…

まあ、この、任意の型を非同期メソッドの戻り値に使える機能、
「[C# 7の新機能紹介](../../../../study/csharp/cheatsheet/ap_ver7.md)」でもどう取り扱うかは結構悩みまして。
何せ、実用例がほんとに少ない。
なので、どうしても「ほぼ [`ValueTask`](../../../../study/csharp/async/sp5_async.md#valuetask) 専用です」的な書き口に。
(`ValueTask` だけで十分価値は高いんですが。)

一応小ネタっぽい話もすると、この機能、C#チームからも

> We estimate that in the eventual C# ecosystem maybe 5 people will write tasklike types that get mainstream adoption.

> 我々の見積もりでは、最終的に C# エコシステム内において、たぶんせいぜい5人くらいが、メインストリームに採用される tasklike 型を書くことになるだろう。

とか言われています(参考: [C# LDM notes from 2016.08.24](https://github.com/dotnet/roslyn/issues/10902#issuecomment-242428870))。
せいぜい5人。

たぶん確実に使われそうなのとしては、

- 主目的たる`ValueTask`
- [WinRTの`IAsyncOperation`](https://msdn.microsoft.com/ja-jp/library/windows/apps/br206598.aspx)との相互運用
- [Rx](https://www.nuget.org/packages/System.Reactive/)

の3つは確定。
残りせいぜい2個ですか。

まあ、`ContextFreeTask`も、作ったはいいけど、大々的に使うかどうかはちょっと悩ましかったり。
誤用(逆に使うとまずいアプリのレイヤーで使われたり。ライブラリでも、publicなところで使ってしまうと、アプリ側でコンテキストを拾い損ねる事態になりそう)がちょっと怖そうですし。
