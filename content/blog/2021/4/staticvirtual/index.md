---
title: "ピックアップRoslyn 4/4: static virtual/abstract members"
source_url: "https://ufcpp.net/blog/2021/4/staticvirtual/"
content_type: "BlogEntry"
published_at: "2021-04-04T17:07:36"
updated_at: "2021-04-04T17:07:36"
tags: []
umbraco_id: 2347
parent_id: 2346
sort_order: 0
aliases: []
---

# ピックアップRoslyn 4/4: static virtual/abstract members

インターフェイスの静的メソッドを virtual/abstract 指定できるようにする話が出ています。

- [[Proposal]: Static abstract members in interfaces #4436](https://github.com/dotnet/csharplang/issues/4436)

主な用途は、

- ファクトリ
- 比較 (`Equatable` とか `Comparable`)
- 数値計算

とかになると思います。
一番求められている用途は数値計算で、要は [NumPy](https://ja.wikipedia.org/wiki/NumPy) みたいなことを C# でも苦痛なく、かつ、パフォーマンスを損なうことなく実現したいというものです。

## <a id="factory">ファクトリ</a>

数値計算に特化した仕様かと言うとそんなこともないので、先に他の用途について触れておきます。

ジェネリックなメソッドを作るとき、`new()` 制約を付けることで引数なしのコンストラクターなら呼び出せるんですが…

```csharp {title="new() 制約"}
void m<T>() where T : new()
{
    var x = new T(); // OK
}
```

ところが、この `new` には引数を渡せません。

```csharp {title="new(X) は書けない"}
void m<T>(int i)
    where T : new(int) // こう書きたい(ダメ)
{
    var x = new T(i); // ダメ
}
```

これを例えば以下のように書けるようにすることで代替できるようになります。

```csharp {title="new(X) の代替で T.New(X)"}
void m<T>(int i)
    where T : IConvartibleFromInt // 普通のインターフェイス制約
{
    var x = T.New(i); // こう書けるようにする
}

interface IConvartibleFromInt
{
    public static abstract IConvartibleFromInt New(int i);
}
```

## <a id="generic-math">generic math</a>

たびたび出てくる要望として、 `+`, `-`, `*`, `/` をジェネリックな型で使いたいというものがあります。
わかりやすい例だと「[`Enumerable.Sum`](https://source.dot.net/#System.Linq/System/Linq/Sum.cs,17ae8142727f08ee) の実装何個あるんだ」って話で。
中身はほぼ定型文で、以下のようなコードのコピペが何個も並んでいます。

```csharp {title="Sum"}
foreach (int v in source)
{
    sum += v;
}
```

コピペせざるを得ないのはジェネリックな型に対して `+` を使えないからです。

業務アプリ開発とかでは大体 `int` か `double`、せいぜい `decimal` を使っておけばいいのでジェネリックじゃなくてもそこまで困らないんですが、
汎用数学ライブラリみたいなのを作ろうとすると結構困ります。
[NumPy](https://ja.wikipedia.org/wiki/NumPy) みたいなものの利用者を取り込みたいし、この問題を解決したいという流れ。

現状の C# で汎用数学処理を書こうとするとどうなるかと言うと、以下のような感じ(3年位前のブログ):

- [C# にも型クラス(Shapes)が欲しい…](../../../2018/5/metricspace/index.md)

ブログタイトルが「型クラス」となっていますが、まあ、それが今回出ている「[static virtual 提案](https://github.com/dotnet/csharplang/issues/4436)」の原型。

- [Shapes and Extensions](../../../2017/2/pickuproslyn0223/index.md)

この「Shapes」というやつは結構込み入った仕様なんですが、
いったんこのうちの一部分というか、既存の文法からそう大きく外れない範囲でできるものが
「インターフェイスの [static](../../../../study/csharp/oop/oo_static.md) メソッドに [virtual](../../../../study/csharp/oop/oo_polymorphism.md#virtual)/[abstract](../../../../study/csharp/oop/oo_abstract.md) を認めよう」というものです。

上記の `Sum` であれば、「0 を取得」と「足し算」の2つがあれば書けるので、まず以下のようなインターフェイスを用意。

```csharp {title="static virtual / static abstract の宣言"}
interface IAddable<T> where T : IAddable<T>
{
    static virtual T Zero { get; } => default(T);
    static abstract T operator +(T t1, T t2);
}
```

これが入るのであれば、標準の `int` 型(`Int32` 構造体(`System` 名前空間))に以下のような実装も足されることになります。

```csharp {title="static virtual / static abstract の実装"}
struct Int32 : …, IAddable<Int32>
{
    static Int32 I.operator +(Int32 x, Int32 y) => x + y; // Explicit
    public static int Zero => 0;                          // Implicit
}
```

これを使って `Sum` メソッドを書くと以下のようになります。

```csharp {title="static virtual / static abstract の利用"}
public static T Sum<T>(T[] ts) where T : IAddable<T>
{
    T result = T.Zero;                   // Call static operator
    foreach (T t in ts) { result += t; } // Use `+`
    return result;
}
```

これ、下手な実装をするとパフォーマンスを著しく損ねます。
`+` なんてネイティブコード化されると CPU の1命令だったりするわけですが、
そこに、インターフェイスが挟まることで仮想関数呼び出しが挟まったり、インライン展開阻害が起きたりして数倍～1桁遅くなります。

とはいえ、[前述の3年前のブログ](../../../2018/5/metricspace/index.md)でやっているような「値型ジェネリクスを使った黒魔術」でパフォーマンスは解決できるんですが、型引数が余分に1個増えたり、演算子を使えなかったり、だいぶ使い勝手は悪いです。

```csharp {title="これまでの黒魔術的な回避策"}
public static T Sum<T, TAddable>(T[] ts) where TAddable : IAddable<T>
{
    T result = default(TAddable).Zero;
    foreach (T t in ts) { result = default(TAddable).Add(result, t); }
    return result;
}
```

## <a id="type-param">型引数による分岐</a>

普通の、既存の virtual/abstract メソッドの場合、
実際にどのメソッドが呼び出されるかはインスタンスの実行時の型によって決まります。

```csharp {title="通常の virtual/abstract は実行時の型によって呼び出し先が決定される"}
using System;
 
// 型引数が何だろうと、インスタンスが A なので表示されるのは "A"。
m<I>(new A());
m<A>(new A());
 
// 型引数が何だろうと、インスタンスが B なので表示されるのは "B"。
m<I>(new B());
m<A>(new B());
m<B>(new B());
 
void m<T>(T x) where T : I => x.M();
 
interface I
{
    void M();
}
 
class A : I
{
    public virtual void M() => Console.WriteLine("A");
}
 
class B : A
{
    public override void M() => Console.WriteLine("B");
}
```

一方、static virtual/abstract の場合は型引数を見ます。
コンパイル時に決定済み。
abstract なままのもの(実態がないもの)を使うとコンパイル自体できません。

```csharp {title="static virtual/abstract はコンパイル時に渡した型引数で決定される"}
using System;
 
// static virtual/abstract の場合は型引数の方で呼び出し先が決まる。
m<I>(new A()); // コンパイル エラー。 I.M に実装がない。
m<A>(new A()); // "A"
 
m<I>(new B()); // コンパイル エラー。 I.M に実装がない。
m<A>(new B()); // "A"
m<B>(new B()); // "B"
 
void m<T>(T x) where T : I => T.M();
 
interface I
{
    public abstract static void M();
}
 
class A : I
{
    public override static void M() => Console.WriteLine("A");
}
 
class B : A
{
    public override static void M() => Console.WriteLine("B");
}
```

## <a id="runtime-mod">型システムの修正</a>

これ、C# コンパイラーのレベルで実現しようと思うと、多分前述の「黒魔術的な構造体ジェネリクス」みたいなコードを生成することになります。
さすがにちょっと「コンパイラーが裏でこっそり生成するコード」にするのもためらわれる(型引数の個数が変わるとかだいぶつらい)レベルです。

なので、.NET ランタイムの型システム自体に手を入れる必要がありました。
実際、型システムに手を入れる(.NET 5 以前では使えない機能になる)方向で実装を進めるそうです。

C# 8.0 くらいから、こういう「古いランタイムでは動かない機能」がちらほら入ってきています。

- C# 8.0 の[インターフェイスのデフォルト実装](../../../../study/csharp/oop/oo_interface.md#dim): .NET Core 3.0 以降でだけ動く
- C# 9.0 の[共変戻り値](../../../../study/csharp/oop/oo_polymorphism.md#covariance): .NET 5.0 以降でだけ動く

(ちなみに、この辺りの一定バージョン以上のランタイムでしか動かない機能については「[RuntimeFeature](../../../2018/12/runtimefeature/index.md)」でちょっと書いています。)

とはいえ、デフォルト実装とか共変戻り値と比べても、static virtual/abstract は実装が難しめの機能になります。

結構な大事なんですが、[Miguel de Icaza](https://github.com/migueldeicaza) ([Mono](https://ja.wikipedia.org/wiki/Mono_(%E3%82%BD%E3%83%95%E3%83%88%E3%82%A6%E3%82%A7%E3%82%A2)) 創設者)が[プロトタイプ](https://github.com/partydonk/partydonk/)を作っていて、これをベースに話が進んでいるみたいです。
