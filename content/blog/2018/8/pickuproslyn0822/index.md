---
title: "ピックアップRoslyn 8/22: ldftn + calli"
source_url: "https://ufcpp.net/blog/2018/8/pickuproslyn0822/"
content_type: "BlogEntry"
published_at: "2018-08-22T13:34:37"
updated_at: "2018-08-22T13:51:23"
tags: []
umbraco_id: 2167
parent_id: 2166
sort_order: 0
aliases: []
---

# ピックアップRoslyn 8/22: ldftn + calli

結構前から提案は出ていて、1週間くらい前にそこそこ仕様が固まったやつがあるんですが。

- [Champion "Compiler intrinsics (calli, ldftn, etc)" #191](https://github.com/dotnet/csharplang/issues/191)

C# で、デリゲートではなく、関数ポインターを生で扱うための仕組み。

C# のデリゲートも、そもそもの関数ポインターって概念も、どっちもちゃんと説明しようと思うと結構大変で、どうかこうか迷っていたものの…

## Bing.com runs on .NET Core 2.1

そんな中、昨日話題になっていたブログ:

- [Bing.com runs on .NET Core 2.1!](https://blogs.msdn.microsoft.com/dotnet/2018/08/20/bing-com-runs-on-net-core-2-1/)

[Bing](https://www.bing.com/) が .NET Core 2.1 化したよという話なんですが、

- .NET Standard 1.x 時代は .NET Framework に対して足りてないものが多くて断念したけど、2.0 で行けた
- [crossgen](https://github.com/dotnet/coreclr/blob/master/Documentation/building/crossgen.md) コマンドを使って、CI 時にネイティブ イメージを作ってデプロイしてる
- .NET Core 2.1 のパフォーマンス改善の恩恵を受けて30%高速化

という感じの内容。で、パフォーマンスに関しては、「.NET Core 2.1 ではこんな改善があった」リストも5個ほど紹介。

そのパフォーマンス改善の1つが「`ldftn` + `calli`」がらみ。これらの命令は、関数ポインターの読み込みと、その呼び出し命令です。
通常は[P/Invoke](../../../../study/csharp/interop/sp_pinvoke.md)でしか出てこないような命令で、
現状の C# では普通にやってこの命令が出力されるようなコードは書けません。

(なのに、Bing チームは動的コード生成([IL](../../../../study/il/index.md) 出力)で`ldftn` + `calli`を使っていて、実際それで高速になっているとのこと。)

という話があったので、いい加減に上記の「[Compiler intrinsics (calli, ldftn, etc)](https://github.com/dotnet/csharplang/issues/191)」の話を書こうかなと。

## デリゲートの中身

とはいえ、まずは C# のデリゲートの中身がどうなっているかの話を。

例えば以下のようなデリゲート型を定義したとします。

```csharp
delegate int A(int x, int y);
```

内部的に、このデリゲートに対して、以下のようなクラスが生成されます。

```csharp
// 実際には MulticastDelegate 型から派生していて、MulticastDelegate 側に処理があったりする。
// C# では書けない処理が入ってて、InternalCall (.NET ランタイム内の特殊処理)になってる。
// 細かいところは端折ってるけど、やらないといけないことは概ねこんな感じ。
class A
{
    // obj.Method みたいに書いた時の obj を渡す
    object _target;

    // obj.Method みたいに書いた時の Method に当たる情報を渡す
    // Method 本体が置かれてるメモリ上のアドレス
    IntPtr _functionPointer;

    // multicast 用
    A[] _invocationList;

    public virtual int Invoke(int x, int y)
    {
        // _target をロード
        // x をロード
        // y をロード
        // _functionPointer をロード
        // calli 命令 (現状の C# では出力できない命令)
        // ret 命令
    }

    // single cast
    public A(object target, IntPtr functionPointer)
    {
        _target = target;
        _functionPointer = functionPointer;
    }

    // multicast
    public A(A[] invocationList)
    {
        _target = this;
        // _functionPointer に MulticastInvoke のアドレスを代入
        _invocationList = invocationList;
    }

    public static A Combine(A a, A b)
    {
        // a, b がそれぞれ _invocationList を持ってたら1つの配列にまとめたりしてるかも
        return new A(new[] { a, b });
    }

    private int MulticastInvoke(int x, int y)
    {
        var ret = 0;
        foreach (var item in _invocationList)
        {
            ret = item.Invoke(x, y);
        }
        // 最後の1個しか戻り値が返らない
        return ret;
    }
}
```

C# では「他の言語でいう関数ポインターのようなもの」としてデリゲートを使うわけですが、
実は、.NET の IL 命令上は生の「関数ポインター」も持っています。
メソッドの中身に当たる IL コードがメモリ上のどこに配置されているか、その先頭アドレスを指すのが関数ポインター(function pointer)です。

で、デリゲートは以下のようなものになっています。

- インスタンスと関数ポインターのペアを持つ<em>クラス</em>になる
- マルチキャスト用に、複数のデリゲートを1つにまとめる機能を持つ
- デリゲート型インスタンス `d` に対して `d(x, y)` みたいな呼び出しをした場合、実際に呼ばれるのは `d.Invoke(x, y)` というメソッド(しかも`Invoke`はvirtual)

これはそこそこ重たい仕組みになっていたりします。

- 常にクラスのインスタンスが `new` される
  - 特に、静的メソッドの時は `_target` が `null` で、関数ポインター1個だけしか情報を持たないにも関わらず、無駄にクラスを作ってる
  - クラスなので、P/Invoke のときに GC されないように[固定](../../../../study/csharp/interop/sp_unsafe.md#fixed)が必要で面倒
- マルチキャストのために結構特殊な処理が入ってる
  - マルチキャスト機能は[イベント](../../../../study/csharp/functional/sp_event.md)のためにあるものの、それ以外の用途でマルチキャストを必要とすることはない

なので、「マルチキャスト不要で静的なメソッドに対しては生の関数ポインターを使わせてほしい」という要望が出てきているようです。

## ldftn と calli

ちなみに、`ldftn`. `calli` は、それぞれ関数ポインターをロードする命令と、その関数を呼び出す命令です。

`ldftn` (load function)は、現状のC#でも、デリゲート型のインスタンスを作るときに使われています。
例えば以下のようなコードを書いた場合、

```csharp
static class Program
{
    static void Main()
    {
        A a = Sum;
    }

    static int Sum(int x, int y) => x + y;
    public delegate int A(int x, int y);
}
```

以下のような IL コードが生成されます。

```cil
ldnull // 静的メソッドなので target が null
ldftn int32 C::Sum(int32, int32) // これが関数ポインター読み込み
newobj instance void A::.ctor(object, native int) // デリゲート型のコンストラクターに関数ポインターを渡す
```

一方、`calli` (call indirect)の方は前節のデリゲート型の`Invoke`メソッドの中や、P/Invokeで使われているくらいで、
大多数の C# コードからは生成されません。

## Compiler intrinsics

で、[当初案](https://github.com/dotnet/roslyn/issues/11475)はどうだったかというと、
「通常の C# コードからは絶対に出力されない`calli`, `ldftn`とかの命令を直接出力するための仕組みが欲しい」というものでした。

昔から、「C# コード内に IL コードを埋め込みたい」(いわゆるインライン アセンブラー)という要望もあったりはします。
でも、それは「C# コンパイラー チームが C# 以外の言語も保守しないといけない」、「C# コンパイラーが過度に複雑になる」などの理由で否定されています。

そこで出たのが Compiler intrinsics という提案。
特定のメソッド(例えば `CompilerIntrinsic` 属性が付いてるもの)の呼び出しがあったら、
それを`calli`, `ldftn`とかの命令に置き換えるというもの。
これであれば、C# の文法的にはただのメソッド呼び出しなので、コストは低く所望の動作が得られるだろうという感じです。

## 関数ポインター intrinsics

とはいえ、結局、ディスカッションの過程で例示された実用途は、
「マルチキャスト不要で静的なメソッドに対しては生の関数ポインターを使わせてほしい」だけでした。

(だったら、最初から、「マルチキャスト不要で静的なメソッド」専用の、light weight なデリゲート構文を追加する方がいいのではないかという話もあって、別途、[Static delegate](https://github.com/dotnet/csharplang/blob/master/proposals/static-delegates.md)っていう提案も出ていたり。)

ということで、結局、Compiler intrinsics は`ldftn` + `calli`専用になりそうです。

- `ldftn` の方は、単にメソッドの前に `&` を付けることで関数ポインターを取れるようにする
- `calli` だけ、`CallIndirect`属性を使ったintrinsics (C# コンパイラーが特別扱いするメソッドを用意)にする
