---
title: "Devirtualization (脱仮想化)"
source_url: "https://ufcpp.net/blog/2018/12/devirtualization/"
content_type: "BlogEntry"
published_at: "2018-12-17T09:22:52"
updated_at: "2018-12-17T09:32:12"
tags: []
umbraco_id: 2199
parent_id: 2177
sort_order: 16
aliases: []
---

# Devirtualization (脱仮想化)

今日は一般論として「仮想メソッドは避けれるなら避けたい」という話と、
.NET Core 2.1 で仮想メソッドの「devirtualization」(脱仮想化)のための最適化が入っているという話。

## 仮想メソッドのコスト

多くのプログラミング言語で、
いわゆる[多態的な動作](../../../../study/csharp/oop/oo_polymorphism.md)の実現のために、
仮想呼び出し(virtual call)という機能が提供されています。
仮想呼び出しは、[仮想関数テーブル](../../../../study/csharp/oop/oo_vftable.md)を使った実装が一般的です。

テーブルを使った実装には以下のような利点があります。

- 後からクラスが増えても呼び出し側コードには変更が必要ない
- 分岐の数がどれだけ増えても呼び出しにかかる時間が一定

ただし、仮想呼び出しには以下のようなコストがかかります。

- テーブルを引くために間接参照が増える
  - ちなみに、今の .NET Coreの実装だとテーブルは2段構造になってるらしく、間接参照は2回
- [インライン展開](../../../../study/csharp/structured/miscinlining.md)が効かなくなる

特に、インライン展開が効くか効かないかで、だいぶパフォーマンスが変わります。

例えば以下のようなコードを考えます。

```csharp
interface IValue { int Value { get; } }
class Impl : IValue { public int Value => 0; }
 
public class VirtualCallBanchmark
{
    // インターフェイス越し
    IValue A { get; } = new Impl();
 
    // クラスを直公開
    Impl B { get; } = new Impl();
 
    [Benchmark]
    public int Interface() => A.Value;
 
    [Benchmark]
    public int Class() => B.Value;
}
```

`Interface`メソッドの方は`IValue`インターフェイス越しに`Value`プロパティを参照(仮想呼び出しになるので遅い)で、
`Class`メソッドの方は具象型である`Impl`クラスを直接参照しています(仮想呼び出しが必要ないので速い)。

このベンチマークを実行すると、`Class`メソッドの方は「計測できないくらい何もしてない」と言われるくらい「ほぼ0時間」です。
一方、`Interface`メソッドの方には、手元の環境では 1.4ns くらいの時間が掛かりました。

この差はインライン展開によるもので、`Class`の方は、`int Class() => 0;` という単に定数を返すだけのメソッドに最適化されて、ほとんど何もコードが残りません。

1.4ns も微々たる時間ですが、このコストすらも避けたいことはよくあります。
それに、多態動作が特に必要ないなら仮想呼び出しはただ無駄なコストなので、できれば消したいものです。
なので、仮想メソッドであっても、必要がなければただのメソッド呼び出しに変換して、
インライン展開が効くようにする最適化を「devirtualization」(脱仮想化)と呼びます。

## .NET Core 2.1 の devirtualize 最適化

ということで、.NET Core 2.1 から devirtulization 最適化が入っているみたいです。
(単純な最適化がいくつか 2.0 に対してマージされていて、ドキュメント上も 2.0 的な記述を見かけるんですが、
ベンチマークを取ってみてる感じは 2.0 ではあんまり有効じゃなさそう…)

以下のようなコードを書いた時、 .NET Core 2.0 以前と 2.1 以降で実行時間が大きく変わります。

```csharp
public interface IX { void M(); }
public class X : IX { public void M() { } }

public class Program
{
    public void M()
    {
        IX x = new X();
        x.M();
    }
}
```

基本的には、この例のように、メソッド内でさかのぼれば具体的な型がわかる場合にだけ devirtualization 最適化が掛かります。
(この例の場合、`new X()`を呼んでいるところがすぐに見つかるので、`M`は`IX.M`の仮想呼び出しではなく、`X.M`を直接呼び出します。)

.NET Core 2.1 以降なら、このコードは devirtualization された結果、インライン展開も効きます。`X.M()` の中身が空っぽなので何も残らず、実行時間が完全に0になります。

また、.NET Core 2.1 では devirtualization に関係する特殊な最適化もいくつか入っています。

### 値型のインターフェイス明示的実装

devirtualization が掛かって特にうれしいのは構造体です。
構造体に対してインターフェイスを介したメソッド呼び出しをする場合、
仮想呼び出しのコストに加えて、[ボックス化](../../../../study/csharp/resource/rmboxing.md)のコストも掛かります(こっちは仮想呼び出しよりもさらにはるかに高コスト)。
devirtualization が掛かれば、ボックス化のコストももろとも最適化で消せます。

なので、以前にも書いたんですが、[.NET Core 2.1 で、構造体のインターフェイス メソッド呼び出しに最適化が掛かりました](../../3/jitoptimization2_1/index.md)。

以下のような構造体があったとします。

```csharp
struct X : IDisposable
{
    public bool IsDisposed;
    void IDisposable.Dispose() => IsDisposed = true;
}
```

で、この `Dispose` メソッドを以下のように呼び出してみます。

```csharp
// (1) インターフェイス引数で受け取って呼ぶ
public static void Interface(IDisposable x) => x.Dispose();
 
// (2) X のまま受け取って、メソッド内でインターフェイスにキャストして呼ぶ
public static void NonGeneric(X x) => ((IDisposable)x).Dispose();
 
// (3) ジェネリックなメソッドで受け取って呼ぶ
public static void Generic<T>(T x) where T : IDisposable => x.Dispose();
```

(1)と(3)に関しては今も昔も変わらず、(1)が遅くて(3)が速いです。(2)についてが .NET Core 2.0 以前か 2.1 以降かで変わります。

.NET Core 2.0 以前だと、(2)の呼び出し方にはボックス化が掛かって遅かったんですが、
これが、.NET Core 2.1 からは devirtualization されて、(3) と同じ速度が出るようになりました。

### EqualityComparer.Default

ジェネリックな型のインスタンスに対して等値比較する際、
`EqualityComparer<T>.Default`をよく使います。

```csharp
public abstract class EqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
{
    public static EqualityComparer<T> Default { get; } // ← こいつ
    public abstract bool Equals(T x, T y);
    public abstract int GetHashCode(T obj);
}
```

`EqualityComparer<T>.Default.Equals(x, y)`の呼び出しは、
冒頭で出した`IValue.Value`の呼び出し同様、
仮想呼び出しのコストが掛かります
(インターフェイス越しの呼び出しと同様に、抽象クラス越しの呼び出しも、仮想呼び出しが必須になります)。

こういう抽象クラスで戻り値を返しているものは、
通常、devirtualization 最適化の対象になりません。
これに対して、.NET Core 2.1 では、
「`EqualityComparer<T>.Default`を見かけたら、抽象クラスの `EqualityComparer<T>`ではなくて、具象クラスに差し替えて処理する」というような特殊処理が入っていて、
その結果、devirtualization が掛かります。

例えば以下のようなベンチマークは、手元の環境で、
.NET Core 2.0 では 0.95ns、
.NET Core 2.1 では 0.05ns と、1桁実行速度が違います。

```csharp
public class EqualityComparerDefaultBenchmark
{
    [Benchmark]
    public bool IntEquals() => EqualityComparer<int>.Default.Equals(1, 2);
}
```
