---
title: "ref/ref struct 変数を非同期メソッド中で使えるように"
source_url: "https://ufcpp.net/blog/2024/4/ref-in-async/"
content_type: "BlogEntry"
published_at: "2024-04-04T23:30:21"
updated_at: "2024-04-04T23:30:21"
tags: []
umbraco_id: 2498
parent_id: 2496
sort_order: 1
aliases: []
---

# ref/ref struct 変数を非同期メソッド中で使えるように

[前回の `Lock` クラスの話](../lock-class/index.md)を見てから、とりあえず以下のコードを見てほしい。

```csharp {title="非同期メソッド中でエラーに" error-ranges="21:15-21:24" error-diagnostics="CS9217@21:15-21:24"}
using System.Runtime.Versioning;

[module: RequiresPreviewFeatures]

class MultiThreadCode
{
    private static readonly object _syncObj = new();
    private static readonly Lock _syncLock = new();

    public static IEnumerable<object?> MIterator()
    {
        lock (_syncObj) { } // 旧来 lock。
        lock (_syncLock) { } // 新しい lock (VS 17.10p2 以降)。

        yield return null;
    }

    public static async ValueTask MAsync()
    {
        lock (_syncObj) { }
        lock (_syncLock) { } // これだけダメ(VS 17.10p2 以降)。

        await Task.Yield();
    }
}
```

おそらく C# 13 正式リリースまでには直ると思うんですが、
どうしてこうなるのかと、どう対処する予定なのかという話になります。

ちなみに、単に `Lock` クラスに対して特殊処理をするという話ではなく、
もう少し汎用に「非同期メソッド中で ref ローカル変数を使えるようにする」という対処になります。


## lock の展開

[前回の話]で、今回関係するのは、`Lock` インスタンスに対する `lock` ステートメントが `using (x.EnterScope())` み化けるという点。
で、さらにいうと、`using` は以下のように展開されます。

```csharp {title="lock → using → try-finally"}
class MultiThreadCode
{
    private static readonly Lock _syncLock = new();

    // 元コード。
    public static void A()
    {
        lock (_syncLock) { }
    }

    // lock → using。
    public static void B()
    {
        using (_syncLock.EnterScope()) { }
    }

    // using → try-finally。
    public static void C()
    {
        Lock.Scope scope = _syncLock.EnterScope();
        try
        {
        }
        finally
        {
            scope.Dispose();
        }
    }
}
```

ここで、`Lock.Scope` は [ref struct](../../../../study/csharp/resource/refstruct.md) になっています。
これが先ほどのコードで非同期メソッド中の `lock (_syncLock)` がエラーになる原因です。
問題の本質としては以下のようなコードと同じ。

```csharp {title="非同期メソッド中では ref struct を使えない" error-ranges="15:9-15:18" error-diagnostics="CS4012@15:9-15:18"}
class A
{
    public static IEnumerable<object?> MIterator()
    {
        // イテレーター中では ref strcut を使える。
        // (ただし、yield をまたがない場合のみ。)
        Span<int> span = stackalloc int[1];

        yield return null;
    }

    public static async ValueTask MAsync()
    {
        // こちらはダメ。
        Span<int> span = stackalloc int[1];

        await Task.Yield();
    }
}
```

イテレーターと非同期メソッドって、仕組みがかなり似ていて、「イテレーターでできて非同期メソッドでできない」ということは原理的にはあまりないんですが。
実際、上記の挙動は単に実装都合で、コストさえかければ「非同期メソッド中でも ref struct のローカル変数を書けるようにする」というのは可能です。

## イテレーターの中断と再開

「[イテレーターのコンパイル結果](../../../../study/csharp/data/sp2_iterator.md#complied)」辺りで書いてるんですが、
イテレーターは「中断と再開」をするようなコードが生成されます。

例えば以下のようなコードを書いたとき、

```csharp {title="イテレーターの例"}
foreach (var x in M())
{
    Console.WriteLine(x);
}

IEnumerable<int> M()
{
    var x = 1;
    yield return x * x;

    // 式は適当。
    // ここで重要なのは、y は yield をまたがないということ。
    var y = ++x * x;
    y *= y;

    yield return y;

    // 同、z は yield をまたがない。
    var z = ++x;
    z *= (2 * x + 1);

    yield return z;
}
```

おおむね、以下のようなクラスが生成されます。
(簡単化のためちょこっとさぼっています。要点のみ。)

```csharp {title="上記イテレーターの解釈結果"}
var e = new MImpl();
while (e.MoveNext())
{
    Console.WriteLine(e.Current);
}

class MImpl
{
    private int _i = 0;
    private int _x = 1;

    public int Current { get; private set; }

    public bool MoveNext()
    {
        if (_i == 0)
        {
            Current = _x * _x;
        }
        else if (_i == 1)
        {
            var y = ++_x * _x;
            y *= y;
            Current = y;
        }
        else if (_i == 2)
        {
            var z = ++_x;
            z *= (2 * _x + 1);
            Current = z;
        }
        else
        {
            return false;
        }

        _i++;
        return true;
    }
}
```

ここで重要なのは以下の点。

* `yield` をまたいで使う変数はフィールドに昇格する
* そうでないものはローカル変数のまま

つまり、「`yield` さえまたがなければ、ローカル変数に制限を掛ける必要はない」ということになります。
ここではイテレーターで話しましたが、非同期メソッドもほぼ同様で、
「`await` さえまたがなければ、ローカル変数に制限を掛ける必要はない」といえたりします。

ただまあ、これはあくまで「原理的には」という話であって、じゃあ、現在の実装がどうなっているかというと…
C# 12 時点では以下のような感じ。

```csharp {title="C# 12 時点での、ref/ref struct のイテレーター/非同期メソッド中での挙動" error-ranges="19:9-19:16,22:17-22:18,29:9-29:18,31:16-31:18,32:9-32:16,35:17-35:18" error-diagnostics="CS8344@19:9-19:16,CS8176@22:17-22:18,CS4012@29:9-29:18,CS9104@31:16-31:18,CS8344@32:9-32:16,CS8177@35:17-35:18"}
class A
{
    public static void M()
    {
        RefStruct rs = new();

        using (rs) { }
        foreach (var _ in rs) ;

        int x = 1;
        ref int r = ref x;
    }

    public static IEnumerable<object?> MIterator()
    {
        RefStruct rs = new();

        using (rs) { }
        foreach (var _ in rs) ; // ダメ。

        int x = 1;
        ref int r = ref x; // ダメ。

        yield return null;
    }

    public static async ValueTask MAsync()
    {
        RefStruct rs = new(); // 非同期メソッドだとこの時点でダメ。

        using (rs) { } // ダメ。
        foreach (var _ in rs) ; // ダメ。

        int x = 1;
        ref int r = ref x; // ダメ。

        await Task.Yield();
    }
}

ref struct RefStruct
{
    public void Dispose() { }

    public RefStruct GetEnumerator() => this;
    public int Current => 0;
    public bool MoveNext() => false;
}
```

どれも、「ref struct のローカル変数が認められるのであれば書けてもいいはずのコード」になります。
ところが、大丈夫なものとコンパイル エラーになるものがまちまち。

## ref/ref struct 変数を非同期メソッド中で使えるように

まあ既知の問題ではあったんですが。
これまで、需要がそこまでないからか、ずっと放置されていました。
ところが、今回「`Lock` クラスに対する `lock` ステートメント」問題が出たからか、急に対処することになったみたいです。

* [Add proposal for "ref and unsafe in iterators and async" #7994](https://github.com/dotnet/csharplang/pull/7994)

先ほどの、以下のようなコード、すべて「`yield`/`await` さえまたがなければ認める」ということになりそうです。

```csharp {title="C# 12 時点での、ref/ref struct のイテレーター/非同期メソッド中での挙動"}
RefStruct rs = new();

using (rs) { }
foreach (var _ in rs) ;

int x = 1;
ref int r = ref x;
```

* ref ローカル変数
* ref struct のローカル変数
  * ref struct に対する `using` ステートメント
  * ref struct に対する `foreach` ステートメント

付随して、同じく「`yield`/`await` さえまたがなければ認める」という条件で、
[`unsafe` ブロック](../../../../study/csharp/interop/sp_unsafe.md#unsafe)も認めるそうです。

### lock 中の yield

逆に、「これまで書けちゃっていたけども、実はまずかった」というものに警告を出そうという話もあります。
それが「`lock` ステートメント中の `yield`」です。

```csharp {title="まずそうなコード: lock 中の yield" error-text="await Task.Yield()" error-diagnostics="CS1996@19:13-19:31"}
class MultiThreadCode
{
    private static readonly object _syncObj = new();

    public static IEnumerable<object?> MIterator()
    {
        lock (_syncObj)
        {
            // これが書けちゃう。使い方によってはまずい。
            yield return null;
        }
    }

    public static async ValueTask MAsync()
    {
        lock (_syncObj)
        {
            // 非同期メソッドの場合、コンパイル エラーになるので大丈夫。
            await Task.Yield();
        }
    }
}
```

.NET の実装では、
「ロックの開始と終了(内部的には `Monitor.Enter` と `Monitor.Exit`)は同じスレッドでやらないといけない」という制限がありまして。
非同期メソッドの方はわかりやすく「`await` をまたぐと別スレッド」感があるのでコンパイルの時点でエラーにしています。

で、イテレーターの方も使い方によっては「`yield` をまたぐと別スレッドになることがある」という意味では危険で、
例えば、以下のようなコードを書くと実行時に `SynchronizationLockException` 例外が出ます。

```csharp {title="lock 中 yield で例外を起こす例"}
object syncObj = new();

IEnumerable<object?> M()
{
    lock (syncObj)
    {
        // これが書けちゃう。使い方によってはまずい。
        yield return null;
    }
}

foreach (var _ in M())
{
    // M 内に非同期コードがなくても、利用側が非同期だった時点でアウト。
    await Task.Yield();
}
```

ということで、この「`lock` 中での `yield`」も警告を足すことになりそうです。
(いきなりエラーにすると破壊的変更になるのでとりあえず警告。
何バージョンかかけてエラーに変更する可能性はあり。)

(※ 「スレッドをまたいだ `lock` を書けるようにする」みたいなことはしません。)
