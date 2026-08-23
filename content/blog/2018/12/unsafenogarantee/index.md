---
title: "Unsafe クラス(保証外)"
source_url: "https://ufcpp.net/blog/2018/12/unsafenogarantee/"
content_type: "BlogEntry"
published_at: "2018-12-27T09:53:16"
updated_at: "2018-12-27T09:53:16"
tags: []
umbraco_id: 2210
parent_id: 2177
sort_order: 27
aliases: []
---

# Unsafe クラス(保証外)

今日は `Unsafe` クラスがらみの話で、
特にきわどい(動作保証外)やつ。
.NET Core 2.0 ～ 2.1 くらいでは動くことを確認していますが、
仕様として保証がなく、古いランタイムや将来、また、Mono などの他の .NET 環境で動く保証がないものです。

## メモリレイアウトが同じもの

まず、元々 unsafe コードを使ってできるし、
`Unsafe`クラスを使っても動作保証があるものから説明。

ポインターを使ったり、`Unsafe.As`メソッドを使うと、
全然違う型・C# では本来変換できない型同士の間で強制変換ができます。
強制しているだけなので、それがちゃんと意味あるコードになるかどうかは unsafe、
すなわち、書いている人の責任の範疇になります。

どういう場合なら大丈夫かというと、要するに、
メモリ上でのフィールドなどのレイアウトが同じ場合です。
例えば、以下のような、サイズが同じで参照型を含まない構造体同士は強制変換しても大丈夫です。

```csharp {title="レイアウトがわかっている構造体に対して Unsafe"}
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
 
// 構造体サイズが4バイトになるようにフィールドを並べる
// この場合は別に StructLayout 属性がなくても4バイトになるものの、
// サイズをピッタリ調整したい場合には明示した方がいいかも。
[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct A
{
    public byte X;
    public byte Y;
    public short Z;
}
 
// int 1個なので当然4バイト。
struct B
{
    public int X;
}
 
public class Program
{
    static void Main()
    {
        // サイズが同じで参照型を含まない構造体間での強制変換は、
        // 普通にポインターを使ってできる操作なので
        // unsafe ではあってもまだ動作保証はある。
        B b = new B { X = 0x01020304 };
        A a = Unsafe.As<B, A>(ref b);
 
        // 4, 3, 102
        Console.WriteLine($"{a.X}, {a.Y}, {a.Z:x}");
    }
}
```

## 保証外な利用方法

`Unsafe.As`メソッドを使うと、
こういった強制型変換を参照型に対しても行えます。

ただ、これは動作保証がないようです。
少なくとも .NET Core 2.1 では動いているんですが、
将来にわたってもそのまま動くかと言われると何も保証されていません。

### 共変クラス

C# の[変性](../../../../study/csharp/oop/sp4_variance.md)はインターフェイスとデリゲートに対してしか働かないわけですが、それを強制的にクラスに対しても適用できたりします。

```csharp {title="共変クラス(動作保証なし)"}
// string → object の代入が合法なんだったら…
string s = "abc";
object o = s;
 
// Task<string> → Task<object> も OK にしてほしい
Task<string> ts = Task.FromResult("abc");
 
// 実際は無理
// Task<object> to = ts;
 
// が、Unsafe.As ならできてしまう。
Task<object> to = Unsafe.As<Task<string>, Task<object>>(ref ts);
 
// await でちゃんと "abc" が取れる
var result = await to;
Console.WriteLine(result);
```

ただ、これは `Task<TResult>`クラス(`System.Threading.Tasks`名前空間)の`TResult`が戻り値にしか使われていないから大丈夫なのであって、
例えば以下のように、読み書き両方できるとまずいです。

```csharp {title="ほんとに挙動が壊れるダメな Unsafe"}
using System;
using System.Runtime.CompilerServices;
 
class Box<T> { public T Value; }
 
public class Program
{
    static void Main()
    {
        // string → object の代入が合法なんだったら…
        Box<string> s = new Box<string> { Value = "abc" };
        Box<object> o = Unsafe.As<Box<string>, Box<object>>(ref s);
 
        // 読み出しはまだ大丈夫。"abc" が表示される。
        Console.WriteLine(o.Value);
 
        // 書き込みはアウト。
        o.Value = 10;
        // ダメなことをやっちゃったあとなので、何か動作がおかしい。
        // 最悪の場合死に至るのでダメ、絶対！
        Console.WriteLine(o.Value);
    }
}
```

また、`string` → `object` が大丈夫だから `Task<string>` → `Task<object>` も大丈夫だったのであって、互換性がない型同士での `Task<T>` 間の変換はもちろんダメです。

```csharp {title="ヤバい(ヤバい)"}
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
 
// 無関係のクラス
class C1 { }
class C2 { }
 
// A, B は同じ4バイト
// C は1バイト
struct A { public int X; }
struct B { public short X, Y; }
struct C { public byte X; }
 
public class Program
{
    static void Main()
    {
        // ヤバい(無関係のクラス)
        Task<C1> c1 = Task.FromResult<C1>(null);
        Task<C2> c2 = Unsafe.As<Task<C1>, Task<C2>>(ref c1);
 
        // 保証外だけどギリ動く(サイズが同じ)
        Task<A> a = Task.FromResult(new A());
        Task<B> b = Unsafe.As<Task<A>, Task<B>>(ref a);
 
        // ヤバい(サイズが違う)
        Task<C> c = Unsafe.As<Task<A>, Task<C>>(ref a);
    }
}
```

### シグネチャが同じデリゲート

デリゲートは、引数・戻り値の型が完全に一致していても、
別個に定義したものは別の型扱いを受けます。

そして、引数・戻り値の型が完全に一致しているデリゲート型は山ほどあります。
例えば以下のような。

- [`IValueTaskSource`](https://source.dot.net/#System.Runtime/System.Runtime.cs,e847f170291a7c6f,references) … `Action<object>` を使用。`object`引数、`void`戻り値。
- [`Timer`](https://source.dot.net/#System.Private.CoreLib/src/System/Threading/Timer.cs,814) … `TimerCallback` を使用。`object`引数、`void`戻り値。
- [`SynchronizationContext`](https://source.dot.net/#System.Private.CoreLib/src/System/Threading/SynchronizationContext.cs,98) … `SendOrPostCallback` を使用。`object`引数、`void`戻り値。

そして、これらのデリゲート間の変換では、以下のように `new` が挟まってしまって、無駄にメモリを食います。

```csharp {title="デリゲートの残念さ"}
using System;
using System.Threading;
using System.Threading.Tasks.Sources;
 
class MyValueTaskSource : IValueTaskSource
{
    private SynchronizationContext _context;
    public void GetResult(short token) { }
    public ValueTaskSourceStatus GetStatus(short token) => ValueTaskSourceStatus.Succeeded;
    public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
    {
        // こういう書き方は無理。
        // _context.Post(continuation, state);
 
        // こうなる。
        _context.Post(continuation.Invoke, state);
 
        // ↑これは意味的には↓と同じ。1段 new が挟まってて、ヒープも確保される。
        // _context.Post(new SendOrPostCallback(continuation.Invoke), state);
    }
}
```

でも、`Unsafe.As`メソッドを使えば無駄な `new` なしで強制変換できます。

```csharp {title="Unsafe で無理やり変換(動作保証なし)"}
// でも、これで行けたりする。
_context.Post(Unsafe.As<Action<object>, SendOrPostCallback>(ref continuation), state);
```

引数・戻り値の型が一致している限りには、
少なくとも .NET Core 2.1 とかでは動きます
(再三いうけども動作保証があるわけじゃない)。

```csharp {title="Unsafe で無理やり変換(動作保証なし)"}
using System;
using System.Runtime.CompilerServices;
using System.Threading;
 
public class Program
{
    static void Main()
    {
        Action<object> action = x => Console.WriteLine(x);
        SendOrPostCallback callback = Unsafe.As<Action<object>, SendOrPostCallback>(ref action);
 
        callback("abc"); // ちゃんと Console.WriteLine("abc") が呼ばれる
    }
}
```

## 静的な型と動的な型

ちなみに、`Unsafe.As`メソッドでの共生型変換を、互いに無関係なクラスでやってしまうと結構変な動作になります。
以下のように、全然無関係なメソッドが呼ばれてしまうことがあり得ます。
([仮想呼び出し](../../../../study/csharp/oop/oo_vftable.md)が狂います。本来参照すべきものと違う仮想テーブルをひいちゃうので当然。)

```csharp {title="Unsafe.As の強制変換をすると仮想呼び出しが狂う"}
using System;
using System.Runtime.CompilerServices;
 
class A
{
    public void M() => Console.WriteLine("A non-virtual M");
    public virtual void X() => Console.WriteLine("A virtual X");
}
 
class B
{
    public void M() => Console.WriteLine("B non-virtual M");
 
    // 仮想テーブル的に、A.X と同じ場所にポインターが入る
    public virtual void Y() => Console.WriteLine("B virtual Y");
}
 
public class Program
{
    static void Main()
    {
        A a = new A();
        B b = Unsafe.As<A, B>(ref a);
 
        // non-virtual なメソッドは静的な型(B)に基づいて呼ばれる。
        // なので、これは普通に B.M が呼ばれる
        b.M(); // B non-virtual M
 
        // virtual なメソッドは動的な型(A)に基づいて呼ばれる。
        // 型の強制変換のせいで変な挙動に。
        // 仮想テーブル上、B.Y と同じ位置に A.X のポインターがあるので、
        // B.Y を呼んだつもりが A.X が呼ばれる。
        b.Y(); // A virtual X
    }
}
```

この例ではたまたまクラッシュせずに動作しますが
(というか、ならないように気を使って書いています)、
無神経にやるとまずクラッシュします。
