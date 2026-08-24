---
title: "リソースの破棄"
source_url: "https://ufcpp.net/study/csharp/resource/oo_dispose/"
content_type: "Article"
published_at: "2002-11-02T00:00:00"
updated_at: "2007-06-30T00:00:00"
tags: []
umbraco_id: 1295
parent_id: 1286
sort_order: 13
aliases:
  - "/study/csharp/oo_dispose.html"
---

# リソースの破棄

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

ファイルや周辺機器などのリソース(OSが管理している資源)を使用する場合、
まずリソースを使用する権利を取得し、
リソースに対する操作(ファイルの読み書きなど)を行った後、
リソース使用権を破棄する必要があります。

メモリは .NET Framework の「[ガーベジコレクション](../cs4j/ab_csspec.md#gc)」機能が自動的に管理していて、
プログラマが明示的に破棄してやる必要はないのですが、
ファイルなどは「[ガーベジコレクション](../cs4j/ab_csspec.md#gc)」の管理対象外で、
明示的な破棄が必要です。

リソースの破棄を怠ると操作が正しく完了しなかったり、
他のプログラムからそのリソースを使用できなくなったりします。
（例えば、ファイルにロックが掛かったままになって、ファイルの読み書きがしばらくできなくなったり。）
そのため、リソースの破棄は確実に行う必要があるのですが、
これは意外に面倒な作業だったりします。


##### <a id="sec-generated-title-2"></a>ポイント

* .NET Framework でメモリー管理は自動化されたけど、 管理外のリソース（たとえば、ファイルIO）もある。

* 管理外のリソースは明示的に破棄が必要。

* 例外が発生した場合でも正しくリソース破棄ができるように、try-catch-finally や using を使いましょう。

## <a id="sec-generated-title-3"></a> <a id="ex"></a>リソース破棄の例

例えば、ファイルの読み書きを行う場合、
まずファイルを開いて、読み書きを行った後、ファイルを閉じる必要があります。
以下に簡単な例を示します。

```csharp {title="リソースの破棄の例"}
using System;
using System.IO;

class DisposeTest
{
    static void Main(string[] args)
    {
        FileStream reader = new FileStream(args[0], FileMode.Open);

        // 先頭のNバイトを読み出して画面に表示
        const int N = 32;
        byte[] buf = new byte[N];
        reader.Read(buf, 0, N);
        for (int i = 0; i < N; ++i)
        {
            Console.Write("{0,4}", (int)buf[i]);
            if (i % 8 == 7) Console.Write('\n');
        }

        reader.Close(); // ファイルを閉じる(リソースの破棄)
    }
}
```


この例のようなリソース破棄の仕方には実は問題があります。
この例のコードでは<em>例外が発生したときに <code>Close</code> メソッドが呼ばれない</em>ため、
リソースの開放が出来なくなります。
例外が発生した場合にも <code>Close</code> メソッドが呼ばれるようにするためには、
以下のように <em>try-catch-finally ステートメントを用います</em>。

```csharp {title="finally を用いたリソースの破棄"}
using System;
using System.IO;

class DisposeTest
{
    static void Main(string[] args)
    {
        FileStream reader = new FileStream(args[0], FileMode.Open);

        try
        {
            // 先頭のNバイトを読み出して画面に表示
            const int N = 32;
            byte[] buf = new byte[N];
            reader.Read(buf, 0, N);
            for (int i = 0; i < N; ++i)
            {
                Console.Write("{0,4}", (int)buf[i]);
                if (i % 8 == 7) Console.Write('\n');
            }
        }
        catch (Exception)
        {
            // 例外処理を行う
        }
        finally
        {
            // 例外が発生しようがしまいが finally ブロックは必ず実行される。
            // リソースの破棄は finally ブロックで行う。
            if (reader != null)
                reader.Close();
        }
    }
}
```



## <a id="sec-generated-title-4"></a> <a id="using"></a>using ステートメント

リソースの破棄の手順をまとめると以下のようになります。
(ただし、<code>Resource</code> はリソース管理用クラスで、
<code>Dispose</code> メソッドによりリソースの破棄を行うものとする。)

```csharp {title="リソース破棄の手順"}
Resource r = new Resource();
try
{
  リソースに対する操作
}
finally
{
  if(r != null)
    r.Dispose();
}
```


リソースの破棄は必ずこの手順で行います
（「Dispose パターン」という呼び名もついてる定型パターン）。
しかし、毎回同じ手順を繰り返すのは面倒です。
そこで、C#ではこの手順を自動的に行ってくれる構文が用意されています。
この構文は <strong id="using" class="keyword">using ステートメント</strong>と呼ばれ、以下のようにして用います。

```csharp {title="using ステートメント"}
using(Resource r = new Resource())
{
  リソースに対する操作
}
```


using ステートメントを用いると、
コンパイラが自動的に上述のリソース破棄用のコードに展開してくれます。
ただし、using ステートメントで使うリソース管理用クラスは <em>
        <code>System.IDisposable</code> インターフェース
      </em>を実装している必要があります。
(<code>FileStream</code> などのクラスライブラリ中のクラスは <code>System.IDisposable</code> インターフェースを実装しています。)

using ステートメントを用いて上述の例を書き直したものを以下に示します。

```csharp {title="using を用いたリソースの破棄"}
using System;
using System.IO;

class DisposeTest
{
    static void Main(string[] args)
    {
        using (FileStream reader = new FileStream(args[0], FileMode.Open))
        {
            // 先頭のNバイトを読み出して画面に表示

            const int N = 32;
            byte[] buf = new byte[N];
            reader.Read(buf, 0, N);
            for (int i = 0; i < N; ++i)
            {
                Console.Write("{0,4}", (int)buf[i]);
                if (i % 8 == 7) Console.Write('\n');
            }
        }
    }
}
```



### <a id="sec-generated-title-5"></a> <a id="expression-using"></a>式だけの using ステートメント

ちなみに、using() の中身は変数宣言だけではなく、式にすることもできます。

```csharp {title="using(式)"}
using(式)
{
  リソースに対する操作
}
```


これで、以下のようなコードと同等な処理になります。

```csharp {title="using(式)"}
using(IDisposable r = 式)
{
  リソースに対する操作
}
```


さらに展開すると、以下のような意味です。

```csharp {title="using(式) の解釈"}
Resource r = 式;
try
{
  リソースに対する操作
}
finally
{
  if(r != null)
    r.Dispose();
}
```


用途としては例えば、以下の「[ジェネリック](../oop/sp2_generics.md#generics)」を使ったメソッドのように、
T が IDispose を実装している時だけ Dispose を呼び出したい場合などに便利です。

```csharp {title="IDispose を実装している時だけ Dispose を呼び出し"}
static void GenericMethod<T>(T obj)
{
    using (obj as IDisposable)
    {
       obj に対する操作
    }
}
```

### <a id="sec-generated-title-6"></a> <a id="dtor"></a>Dispose とファイナライザー

ちなみに、確実に解放しなければならないリソースは、`Dispose`メソッドだけでなく、ファイナライザーも使って破棄処理を行うべきです。
詳しくは「[IDisposable インターフェイスの実装](rm_disposable.md#idisposable)」で説明します。

## <a id="sec-generated-title-7"></a> <a id="using-declaration"></a>using 変数宣言

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、変数宣言に対して `using` 修飾を付けることで、
その変数のスコープに紐づいて `using` ステートメントと同じ効果を得られるようになりました。
これを `using` 変数宣言(using declaration)と呼びます。

例えば以下のように書きます。

```csharp {title="using 変数宣言"}
using System;
 
readonly struct DeferredMessage : IDisposable
{
    private readonly string _message;
    public DeferredMessage(string message) => _message = message;
 
    // Dispose 時にメッセージ表示
    public void Dispose() => Console.WriteLine(_message);
}
 
class Program
{
    static void Main()
    {
        // using var で、変数のスコープに紐づいた using になる。
        // スコープを抜けるときに Dispose が呼ばれる。
        using var a = new DeferredMessage("a");
        using var b = new DeferredMessage("b");
 
        Console.WriteLine("c");
 
        // c, b, a の順でメッセージが表示される
    }
}
```

`Main` メソッド内は以下のコードと同じ意味になります。

```csharp {title="using 変数宣言の展開"}
// using var で、変数のスコープに紐づいた using になる。
// スコープを抜けるときに Dispose が呼ばれる。
using (var a = new DeferredMessage("a"))
{
    using (var b = new DeferredMessage("b"))
    {
        Console.WriteLine("c");
    }
}
```

この展開結果からもわかるように、複数の `using` 変数宣言が並んでいた場合、
`Dispose` メソッドの呼び出しは宣言の逆順で行われます。
この例では、変数は `a`、`b` の順で宣言しているので、
`Dispose` は `b`、`a` の順になります。

### <a id="sec-generated-title-8"></a> <a id="using-declaration-pitfall"></a>using 変数宣言の注意点

`using` 変数宣言は両手放し喜べる機能ではありません。
`Dispose` が呼ばれるタイミングを伸ばしてしまって、パフォーマンスに悪影響を及ぼす可能性があります。
例えば以下のコードを考えます。

```csharp {title="using 変数宣言に単純置き換えしない方がいい例"}
using System;
using System.IO;
using System.Threading;
 
class Program
{
    static void Main()
    {
        string content;
        using (var s = new StreamReader("sample.txt"))
        {
            content = s.ReadToEnd();
        }
        // s.Dispose はここで呼ばれる。
 
        // すごく長い処理。ここでは Sleep で代用。
        Thread.Sleep(5000);
 
        Console.WriteLine(content);
    }
}
```

ファイルからの内容読み込み後、少し長い処理が挟まってからその内容を使います。
これを単純に `using` 変数宣言に置き換えたとしましょう。

```csharp
using System;
using System.IO;
using System.Threading;
 
class Program
{
    static void Main()
    {
        using var s = new StreamReader("sample.txt");
        var content = s.ReadToEnd();
 
        // すごく長い処理。ここでは Sleep で代用。
        Thread.Sleep(5000);
 
        Console.WriteLine(content);
        // s.Dispose はここで呼ばれる。
    }
}
```

この例では、`Dispose` が呼ばれるタイミングが5秒、無駄に遅れることになります。
5秒もファイルを開きっぱなしになるのでだいぶ悪影響があります。
なので、この場合は単純置き換えがダメだということです。

ただ、実用的には、この手の処理は必要な部分だけメソッドに切り出すことが多いです。
この例でも、実際には以下のように書くべきでしょう。
これならまさに `using` 変数宣言がふさわしい書き方です。

```csharp {title="using が必要な範囲だけメソッド抽出"}
using System;
using System.IO;
using System.Threading;
 
class Program
{
    static void Main()
    {
        var content = ReadToEnd("sample.txt");
 
        // すごく長い処理。ここでは Sleep で代用。
        Thread.Sleep(5000);
 
        Console.WriteLine(content);
    }
 
    private static string ReadToEnd(string path)
    {
        using var s = new StreamReader(path);
        return s.ReadToEnd();
        // s.Dispose はここで呼ばれる。
    }
}
```

## <a id="sec-generated-title-9"></a> <a id="interface-required"></a>IDispose インターフェイス必須

C# の構文の多くは、C# コンパイラーによる簡単な置き換え
(いわゆる構文糖衣(syntax sugar))になっています。
例えば、[`foreach`ステートメント](../data/sp_foreach.md#foreach)の場合は `GetEnumerator`、`MoveNext`、`Current` などのメソッド/プロパティ呼び出しに置き換えられます。

この際、C# の大体の構文糖衣は
「ある特定の名前のメソッドさえ持っていれば使える」
という割と緩い条件になっていて、
これをパターン ベース(pattern-based)な構文と呼びます。

そんな中、`using` ステートメントだけは `IDisposable` インターフェイス(`System` 名前空間)の実装が必須です。
[次節で説明する](#pattern-based-using)ように、C# 8.0 で少しだけ条件緩和されましたが、
既存のコードを壊さないようにするためにはかなり限定的にせざるを得なかったらしく、基本的にはインターフェイス実装が必須です。

```csharp {title="using ステートメントの利用には IDisposable インターフェイスの実装が(ほぼ)必須"}
using System;
 
// using で使える型。
class Disposable : IDisposable
{
    public void Dispose() { }
}
 
// 残念ながら IDisposable を実装していないと using で使えない。
class NonDisposable
{
    public void Dispose() { }
}
 
class Program
{
    static void Main()
    {
        // こっちは OK。
        using (new Disposable()) { }
 
        // こっちはコンパイル エラーに。
        using (new NonDisposable()) { }
    }
}
```

一方で、C# 8.0 で新規導入する非同期 `using` ステートメントの場合は、
既存コードのことを心配する必要がないため、元からパターン ベースにしてあります。
すなわち、別に `IAsyncDisposable` インターフェイスの実装は必要ありません。

```csharp {title="非同期 using はパターン ベース"}
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
 
struct MyAwaitable<T> { public ValueTaskAwaiter<T> GetAwaiter() => default; }
struct MyAwaitable { public ValueTaskAwaiter GetAwaiter() => default; }
 
class Program
{
    static async Task Main()
    {
        await using(new AsyncDisposable())
        {
            Console.WriteLine("iside using");
        }
    }
}
```

```console {title="非同期 using はパターン ベース"}
iside using
disposed async
```

### <a id="sec-generated-title-10"></a> <a id="pattern-based-using"></a>パターン ベースな using

<h5 class="version version8">Ver. 8.0</h5>

`using` ステートメントで使うのにインターフェイスの実装が必須となると、
C# 7.2 で導入された[ref 構造体](refstruct.md)で困ることになりました。

ref 構造体を使いたいような場面では `Dispose` したいリソースを握ることもあり、
`using` ステートメントを使いたい動機があります。
しかし、ref 構造体にはインターフェイスが実装できません。

ということで、`using` ステートメントもパターン ベースにしてしまおうということになりました。
ところが、無条件に変更すると既存のコードを壊しかねない懸念があって断念されました。

その結果、C# 8.0 では、ref 構造体に対してだけパターン ベースでの `using` ステートメントを認めることにしました。
以下のようになります。

```csharp {title="ref 構造体に対するパターン ベース using"}
using System;
 
// これまで通り、using で使える型。
struct Disposable : IDisposable
{
    public void Dispose() { }
}
 
// 残念ながら IDisposable を実装していないと using で使えない。
struct NonDisposable
{
    public void Dispose() { }
}
 
// となると、インターフェイスを実装できない ref struct で困っていた。
// ref struct の場合、IDisposable なしでも Dispose メソッドさえあれば using で使えるようになった。
ref struct RefDisposable
{
    public void Dispose() { }
}
 
class Program
{
    static void Main()
    {
        // この行は元々 OK。
        using (new Disposable()) { }
 
        // 残念ながら今でもコンパイル エラーに。
        using (new NonDisposable()) { }
 
        // C# 8.0 で、これは OK になった。
        using (new RefDisposable()) { }
    }
}
```

この変更は、`foreach` ステートメントに対しても適用されます。
`foreach` ステートメントは、列挙対象が `IDisposable` だった場合に `Dispose` メソッドを呼び出す仕様になっています。

```csharp {title="foreach 最後の Dispose 呼び出しがパターン ベースに"}
using System;
 
// GetEnumerator/MoveNext/Current は元々パターン ベース。
// ただ、Dispose の呼び出しだけは IDisposable の実装が必須だった。
// C# 8.0 で、ref struct の場合はパターン ベースで Dispose メソッドを呼んでもらえるように。
ref struct RefEnumerable
{
    public RefEnumerable GetEnumerator() => this;
    public int Current => 0;
    public bool MoveNext() => false;
    public void Dispose() => Console.WriteLine("ref disposed");
}
 
// RefEnumerable と比べて、 ref を取っただけ。
struct BrokenEnumerable
{
    public BrokenEnumerable GetEnumerator() => this;
    public int Current => 0;
    public bool MoveNext() => false;
 
    // この Dispose は呼ばれない。
    // ref struct でない場合、IDisposable インターフェイスの実装が必須。
    public void Dispose() => Console.WriteLine("broken disposed");
}
 
class Program
{
    static void Main()
    {
        // ref disposed は表示される。
        foreach (var _ in new RefEnumerable()) ;
 
        // broken disposed は表示されない。
        // コンパイル エラーにはならないので特に注意。
        foreach (var _ in new BrokenEnumerable()) ;
    }
}
```

```console {title="非同期 using はパターン ベース"}
ref disposed
```

## <a id="sec-generated-title-11"></a> <a id="await-using"></a>非同期using

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0で非同期版の`using`が追加されました。
`await using`という構文で、`IAsyncDisposable`インターフェイス(`System`名前空間)か、
それと同じ[パターン](../async/asyncstream.md#await-foreach)を満たす型の列挙ができます。

```csharp {title="非同期using"}
static async Task AsyncUsing<T>(T x)
    where T : IAsyncDisposable
{
    await using (x)
    {
        // x を破棄する前にやっておきたい処理
    }
}
```

詳しくは「[非同期using](../async/asyncstream.md#await-using)」で説明します。
