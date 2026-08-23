---
title: "未使用ジェネリック型引数で TypeLoadException が起こる問題"
source_url: "https://ufcpp.net/blog/2022/12/unused-generic-type-parameter/"
content_type: "BlogEntry"
published_at: "2022-12-11T18:17:58"
updated_at: "2023-08-03T22:39:37"
tags: []
umbraco_id: 2444
parent_id: 2438
sort_order: 3
aliases: []
---

# 未使用ジェネリック型引数で TypeLoadException が起こる問題

今日は C# の構造体がらみで、
できそうでできない、
できてもいいはずだけど直されない、
コンパイルまでは通るのに実行時にエラーになってしまう制限の話。

※(2023/8/3追記) .NET 8 で直るそうです。

## 入れ子の構造体

C# で、構造体の中にその構造体自身のフィールドを持つことはできません。
レイアウトの決定が無限再帰を起こすので、これはダメで当然。

```csharp {title="構造体の入れ子はダメ" error-text="_nested"}
struct S { S _nested; }
```

これはそもそもコンパイル エラーになります。
当然。

## 使ってないジェネリック型引数でも TypeLoadExcpetion

問題は以下のような場合。
現在の C# (というか .NET の型システム)では、以下のような型はコンパイルはできるものの、実行してみようとすると実行時例外を起こします。
(構造体 `S` のメンバーに初めて触れた瞬間に `TypeLoadException` が飛ぶ。)

```csharp {title="疑惑の判定"}
struct S { Empty<S> _empty; }
struct Empty<T> { }
```

`Empty<T>` の側が `T` のフィールドを持っていないので
(というか空っぽなので、`T` が何かによらずサイズ1で固定)、
レイアウト決定で無限再帰は起こさないはずです。
実際、これは、

* 原理的にはできてもいい
* C# は禁止していない
* CLI (.NET のランタイム仕様)でも禁止は名言されていない
* 現在の .NET のランタイムの実装が過剰防衛している

という状態。
[C# コンパイラー チームの人がそれを指摘する issue](https://github.com/dotnet/runtime/issues/6924) も立っていたりします。

### 実用例

まあ、`Empty` みたいな無意味なコードは誰も書かないとしても、
例えば、以下のようなシナリオでなら似たようなことをしたくなる人はいるはずです。

まず、以下のように、構造体の配列で木構造を表現する例を考えます。

```csharp {title="配列に Parent と Next を持たせた型を入れて木構造を表現"}
// 配列に Parent と Next を持たせた型を入れて木構造を表現。
// A も B もツリー。
// A からは B も参照。
class Tree
{
    A[] A;
    B[] B;
}

struct A
{
    int Parent;
    int Next;
    int BIndex;
}

struct B
{
    int Parent;
    int Next;
}
```

実際にはさらに、「インデックスとは関係ない別の `int` も持ちたくなったりするはずで、なおのこと「この `int` は何？」みたいになると思います。

```csharp {title="この int は何？"}
struct A
{
    // 木とは別に持ちたいデータ。
    int Value;
    int Length;
    ...

    // 木構造表現用。
    int Parent;
    int Next;

    // 別の木を参照
    int BIndex;
}
```

ということで、`Parent` や `Next` が「配列 `A[]` のインデックス」であることが一目でわかるようにしたくなったりします。
よくやるのが、以下のように「`int` をラップした構造体を用意」みたいな手段。

```csharp {title="「配列 T[] のインデックス」用の int のラッパー構造体"}
struct Index<T>
{
    public int Value { get; }
    public Index(int value) => Value = value;
    public static implicit operator Index<T>(int value) => new(value);
}
```

この型を使って先ほどの `Tree`, `A`, `B` を書き換えると以下のような感じになります。

```csharp {title="Index 構造体の導入"}
// 配列に Parent と Next を持たせた型を入れて木構造を表現。
// A も B もツリー。
// A からは B も参照。
class Tree
{
    A[] A;
    B[] B;
}

struct A
{
    int Value;
    int Length;
    Index<A> Parent;
    Index<A> Next;
    Index<B> BIndex;
}

struct B
{
    Index<B> Parent;
    Index<B> Next;
}
```

**便利！**

と思ったところで、冒頭の `Empty<T>` の例と同じ理屈の過剰防衛で、
`TypeLoadException` を起こします…

## 回避策

ちょっと不格好でもよければ解決方法は簡単で、
1段ダミーのクラスを挟むだけだったり。

```csharp {title="ダミーのクラスを1個用意"}
struct Index<T>
{
    public int Value { get; }
    public Index(int value) => Value = value;
    public static implicit operator Index<T>(int value) => new(value);
}

// Index<Dummy<T>> とか Index<Empty<T>> よりは Index<Of<T>> の方がマシかなと…
class Of<T> { }
```

```csharp {title="やむなく Index&lt;Of&lt;T&gt;&gt;"}
class Tree
{
    A[] A；
    B[] B;
}

struct A
{
    int Value;
    int Length;
    Index<Of<A>> Parent;
    Index<Of<A>> Next;
    Index<Of<B>> BIndex;
}

struct B
{
    Index<Of<B>> Parent;
    Index<Of<B>> Next;
}
```

だいぶ不格好で嫌なので、
[件の issue](https://github.com/dotnet/runtime/issues/6924) の優先度を上げてもらえるように👍を付けまくってもらえたりすると大変うれしかったりは…

この issue は2016年からずっと「Future」(いつかね、いつか)なんですよね。
これ、C# 6.0 (つまり、Roslyn 化/C# への移植)の頃に始めて報告されたというだけで、
実際には .NET Framework が生まれてこの方ずっとかも。

※(2023/8/3追記) .NET 8 で直るそうです。C# にジェネリクスが導入されて以来の20年越しの修正に。
