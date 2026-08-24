---
title: "IList とかを IReadOnlyList とかから派生させたい"
source_url: "https://ufcpp.net/blog/2024/3/ilist-inherites-from-ireadonlylist/"
content_type: "BlogEntry"
published_at: "2024-03-03T00:18:50"
updated_at: "2024-03-03T00:18:50"
tags: []
umbraco_id: 2492
parent_id: 2490
sort_order: 1
aliases: []
---

# IList とかを IReadOnlyList とかから派生させたい

.NET が長らく抱えている「なぜ `IList<T>` は `IReadOnlyList<T>` ではないのか」問題、 .NET 9 で解消するかもしれないみたい。

ちなみに、問題を抱えるに至った原因は `IReadOnlyList<T>` が後付けということです。
1から作り直すのであれば、誰がどう考えても `IList<T>` は `IReadOnlyList<T>` から派生させるのが自然です。
それがかえって、`IReadOnlyList<T>` 導入以降に .NET 利用を始めた人に混乱を招いているというのが現状になります。

## 当初設計: インターフェイスは増やしすぎない

インターフェイスを増やすというのは、
型情報で DLL サイズが増えるとか、
実行時にインターフェイスを検索するコストが増えるとか、
多少なりともコストを生じます。

一方で、.NET Framework の最初のβ版が出たのは2000年ごろ、正式版で2002年なわけですが、
この頃は read-only であることの重要性が過小評価されていたと思います。
なので、重要でない(と当時は思われていた)ものにコストは掛けたくないという話に。

(この辺りのことは「<a target="_blank" href="https://www.amazon.co.jp/dp/4296080040?&_encoding=UTF8&tag=cunflc-22&linkCode=ur2&linkId=c74d95cede34616d4c468ff921d42544&camp=247&creative=1211">.NETのクラスライブラリ設計</a>」で触れられていたりします。
ちなみにこの本、要は「.NET の初期設計に関する懺悔本」です。)

そこで当時の設計としては「read-only / writeable なインターフェイスを1個用意して、`IsReadOnly` プロパティで書き込み出来るかどうかを調べる」という作りでした。

```csharp {title="read-only かどうかはプロパティで調べる"}
namespace System.Collections;

public interface IList : ICollection, IEnumerable
{
    object this[int index] { get; set; }
    bool IsReadOnly { get; } // ← これ
    void Add(object value);
    // 以下略
}
```

.NET Framework 2.0 (2005年)に[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)が導入されてもまだこの思想は引き継がれます。
まあ、旧来インターフェイスとジェネリック インターフェイスで思想が違うのも混乱しそうですし。

```csharp {title="ジェネリック ICollection でも IsReadOnly プロパティ"}
namespace System.Collections.Generic;

public interface ICollection<T> : IEnumerable<T>, IEnumerable
{
    int Count { get; }
    bool IsReadOnly { get; } // ← これ
    void Add(T value);
    // 以下略
}
```

問題になり始めたのは C# 4.0 (2010年)で共変性を得てからでして。
読み書き両方できてしまう `IList<T>` や `ICollection<T>` では、以下のような共変な代入ができません。

```csharp {title="書き込みがあると共変にできない" error-ranges="sha256:bc73001ec0c56aa563e17371f59f3af9086f9a2d2545e1b82c0068865e770da8;2:21-2:24" error-diagnostics="sha256:bc73001ec0c56aa563e17371f59f3af9086f9a2d2545e1b82c0068865e770da8;CS0266@2:21-2:24"}
IList<string> str = new List<string>();
IList<object> obj = str; // ダメ。

// そりゃ、こういうコード書かれたらまずいので当然。
obj.Add(1);
```

そこで .NET Framework 4.5 (2012年)では read-only 系のインターフェイスが導入されます。

```csharp {title="read-only 系インターフェイスは共変"}
IReadOnlyList<string> str = new List<string> { "abc" };
IReadOnlyList<object> obj = str; // read-only なら共変。

// obj.Add(1); とか書かれる心配がない。
// 読むだけなら安全。
Console.WriteLine(obj[0]);
```

## インターフェイスへの親インターフェイスの追加・メンバー移動は破壊的変更

2012年に追加された read-only 系インターフェイスですが、元々あったインターフェイスとは独立しています。
残念ながら「`IList<T>` は `IReadOnlyList<T>` ではない」という状態。

```csharp {title="残念ながら完全に別インターフェイス"}
namespace System.Collections.Generic;

public interface IReadOnlyCollection<out T> : IEnumerable<T>
{
    int Count { get; }
}

public interface ICollection<T> : IEnumerable<T>
{
    // IReadOnlyCollection とは独立に Count を持つ。
    int Count { get; }
    // 以下略
}

public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
{
    T this[int index] { get; }
}

public interface IList<T> : ICollection<T>
{
    // IReadOnlyList とは独立に this[int] を持つ。
    T this[int index] { get; set; }
    // 以下略
}
```

普通に考えて、1から作るのであれば以下のようにします。

```csharp {title="1からやり直せるならどう考えても ICollection : IReadOnlyCollection"}
namespace System.Collections.Generic;

public interface IReadOnlyCollection<out T> : IEnumerable<T>
{
    int Count { get; }
}

public interface ICollection<T> : IReadOnlyCollection<T> 
{
    // 以下略
}
```

ところが、後付けでこういうことをするのは破壊的変更になります。

例えば以下のようなコードがあったとします。

```csharp {title="バージョン1"}
// バージョン1

// corelib.dll
interface ICollection
{
    int Count { get; }
}

// corelib とは別のプロジェクトで、別の開発者が保守
// mylib.dll
class C : ICollection
{
    public int Count => 0;
}
```

ここに `IReadOnlyCollection` を「理想的な状態」で導入したくて `Count` を移動させると mylib を壊します。

```csharp {title="Count を IReadOnlyCollection"}
// バージョン2

// corelib.dll
interface IReadOnlyCollection
{
    int Count { get; }
}

interface ICollection : IReadOnlyCollection
{
    // Count は IReadOnlyCollection に移した。
}

// corelib とは別のプロジェクトで、別の開発者が保守
// mylib.dll
class C : ICollection
{
    // 再コンパイルするなら平気。
    // ただ、古い dll のまま使うと「IReadOnlyCollection.Count がない」と怒られる。
    // 再コンパイルするまでは C が持ってるのは ICollection.Count。
    public int Count => 0;
}
```

ということでインターフェイスを独立。
これなら「再コンパイルするまでは `C` は `IReadOnlyCollection` にはならない」というだけなので、
DLL のロードに失敗したりはしません。

```csharp {title="機能がダブるけどもこれで妥協"}
// corelib.dll
interface IReadOnlyCollection
{
    int Count { get; }
}

interface ICollection
{
    int Count { get; } // IReadOnlyCollection と機能がダブってるけど許して
}

// corelib とは別のプロジェクトで、別の開発者が保守
// mylib.dll
class C : ICollection, IReadOnlyCollection // 2個とも実装
{
    public int Count => 0;
}
```

これが .NET のコレクション系インターフェイスの現状になります。

## インターフェイス メソッドのデフォルト実装

.NET Core 3.0 (2019年)に[デフォルト実装](../../../../study/csharp/cheatsheet/ap_ver8.md#default-imeplementation-of-interface)というものが導入されて、インターフェイスへのメンバー追加での破壊的変更を避けれるようになりました。

この機能を使えば先ほどの「既存クラスが `IReadOnlyCollection.Count` を実装していない」問題は解消できます。
(親インターフェイスの追加は、「メンバー追加」の一種なのでデフォルト実装で対処できます。)

```csharp {title="デフォルト実装で解決"}
// corelib.dll
interface IReadOnlyCollection
{
    int Count { get; }
}

interface ICollection : IReadOnlyCollection
{
    new int Count { get; } // IReadOnlyCollection.Count とは別の Count にはなっちゃう。

    // IReadOnlyCollection のことを知らない既存クラスのために、
    // 既存クラスに代わって ICollection 内で IReadOnlyCollection.Count を実装。
    int IReadOnlyCollection.Count => Count;
}

// corelib とは別のプロジェクトで、別の開発者が保守
// mylib.dll
class C : ICollection
{
    // 再コンパイルするまではあくまで ICollection.Count。
    // それでも、ICollection 側で IReadOnlyCollection.Count を実装してくれているので平気。
    //
    // ちなみに、再コンパイルするとこの Count をもって
    // ICollection.Count と IReadOnlyCollection.Count の両方を実装。
    public int Count => 0;
}
```

ということで、インターフェイスのデフォルト実装の導入後、
ついに `ICollection<T>` が `IReadOnlyCollection<T>` 派生に、
`IList<T>` が `IReadOnlyList<T>` 派生にできるのではないかと多くの期待が寄せられています。
実際、2019年に提案あり:

* [Make mutable generic collection interfaces implement read-only collection interfaces
](https://github.com/dotnet/runtime/issues/31001)

ただ、厳密にはこれも破壊的変更を起こす可能性はあったりします。
というのも、デフォルト実装には「ダイアモンド継承」問題というものがあります。
以下のような感じで、「分かれ道からの合流がある継承」をやると問題を起こすことがあります。

```csharp {title="ダイアモンド継承問題" error-ranges="sha256:15b2b7c885e01422d4030048a9ded63869c52b927ef349dbc23a2b6a1d575e17;18:11-18:13" error-diagnostics="sha256:15b2b7c885e01422d4030048a9ded63869c52b927ef349dbc23a2b6a1d575e17;CS8705@18:11-18:13"}
interface IA
{
    int M();
}

interface IB : IA
{
    int IA.M() => 1; // デフォルト実装持ち
}

interface IC : IA
{
    int IA.M() => 2; // デフォルト実装持ち
}

// IA.M の実装をデフォルト実装に頼るとして、
// IB の実装と IC の実装のどちらを使えばいいか不明瞭。
class C : IB, IC
{
}
```

まあ、前述の `ICollection` に「分かれ道」はないので誰しもがこの問題を踏むわけではないんですが。
1段自作のインターフェイスとかを挟んでいると問題を踏む可能性が出てきます。
例えば以下のような感じ。

```csharp {title="IReadOnlyCollection.Count でダイアモンド継承問題を踏む例" error-ranges="sha256:ab8a0f485eec389b11b937978b05ff0416765203971dedc0e5fc7e2669a93a34;11:11-11:22" error-diagnostics="sha256:ab8a0f485eec389b11b937978b05ff0416765203971dedc0e5fc7e2669a93a34;CS8705@11:11-11:22"}
// corelib とは別のプロジェクトで、別の開発者が保守
// anotherlib.dll
interface ICustomReadonlyList : IReadOnlyCollection
{
    // 何らかのデフォルト実装持ち
    int IReadOnlyCollection.Count => 0;
}

// corelib とも anotherlib とも別のプロジェクトで、別の開発者が保守
// mylib.dll
class C : ICollection, ICustomReadonlyList
{
    // ICollection 更新前: 
    //   ICollection.Count は明示的に実装
    //   IReadOnlyCollection.Count は ICustomReadonlyList 側のデフォルト実装を使用
    //
    // ICollection 更新後: 
    //   ICollection.Count は明示してるから平気
    //   IReadOnlyCollection.Count は ICustomReadonlyList と ICollection のどちらのデフォルト実装を使えばいいかわからない
    //   (ソースコードも修正しないと再コンパイルも失敗)
    int ICollection.Count => 1;
}
```

この辺りの懸念もあって、しばらく塩漬けが続きます。

## ついに動きが

そして時は流れること4年、ついに動きが。
.NET 9 でこの作業をやろうという検討に入ったみたいです。

* [API レビューをやった報告コメント](https://github.com/dotnet/runtime/issues/31001#issuecomment-1811013088)
  * 実験を試みる準備が整った
* [.NET チームのプロダクト マネージャーのコメント](https://github.com/dotnet/runtime/issues/31001#issuecomment-1813159725)
  * 指摘されている破壊的変更はレアケースで、考えを変えるものではないと思う
  * どの程度の破壊的変更になるか(許容できる範囲かどうか)、.NET 9 の初期に実験してみるのは十分妥当
* [修正 Pull Request](https://github.com/dotnet/runtime/pull/95830)
