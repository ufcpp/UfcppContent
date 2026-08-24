---
title: "【C# 11 候補】コレクション リテラル"
source_url: "https://ufcpp.net/blog/2021/12/collection-literal/"
content_type: "BlogEntry"
published_at: "2021-12-31T19:42:44"
updated_at: "2021-12-31T19:42:44"
tags: []
umbraco_id: 2399
parent_id: 2375
sort_order: 12
aliases: []
---

# 【C# 11 候補】コレクション リテラル

今日は[リスト パターンの回でちょこっと出て来た `[]` リテラルの話](../list-pattern/index.md#collection-literal)。

逆に、リスト パターン側でも `{}` ではなく `[]` を使う決断に至った理由でもあります。

もう実装があるリスト パターンと違って、こちらはまだ案が出たてで、
もしかしたら C# 11 よりもさらに後になるかもしれないです。

## <a id="collection-literal">[] リテラルの導入</a>

元々、C# よりも後に世に出たり、大幅改修したことがあるプログラミング言語には結構「コレクション リテラル」系の文法があります。
で、多くの場合、`[ 1, 2, 3 ]` みたいに角括弧を利用。

そして現在の C# には `new[] { 1, 2, 3 }` みたいな書き方はあるにはあるものの、いろんなコレクション型があって、それぞれ書き方に統一感がない状態。

```csharp {title="C# のコレクションあれこれ"}
// 型を明示、かつ、配列の時に限り {} だけで OK。
int[] array1 = { 1, 2, 3 };

// 型推論を使いたければ new[] {}。
var array2 = new[] { 1, 2, 3 };

// Target-typed new + コレクション初期化子。 () は省略不可。
List<int> list1 = new() { 1, 2, 3 };

// 通常の new + コレクション初期化子。こっちの場合は () 省略 OK。
var list2 = new List<int> { 1, 2, 3 };

// Span にはまあ、new で配列を割り当ててもいいものの、
// パフォーマンス的には stackalloc を使った方が大体の場合有利。
Span<int> span = stackalloc int[] { 1, 2 };

// ReadOnlySpan も同様。
// あと、stackalloc の後ろは型推論で省略可能。
ReadOnlySpan<int> ros = stackalloc[] { 1, 2, 3 };

// new() もコレクション初期化子も使えないかわいそうな型あり。
var immutable = System.Collections.Immutable.ImmutableArray.Create(1, 2, 3);
```

C# でももう少し統一感あるコレクション リテラルがあった方がいいし、
だったら他の言語に倣って `[]` を使った新文法を導入でいいのではないかという話になります。

```csharp {title="[] をもっていろんなコレクションを初期化したい"}
// ぜんぶ [] にしたい。
int[] array1 = [ 1, 2, 3 ];
List<int> list1 = [ 1, 2, 3 ];
Span<int> span = [ 1, 2, 3 ];
ReadOnlySpan<int> ros = [ 1, 2 ];
System.Collections.Immutable.ImmutableArray<int> immutable = [ 1, 2, 3 ];
```

そしてこっち(リテラル側)でも `[]` を使うのであれば、
[パターンの方](../list-pattern/index.md)で `{}` (プロパティ パターンと区別が付かない)とか `[]{}` (`new[]{}` との対称性はいいかもしれないもののキモい)とか考えず、そっちも素直に `[]` を使えばいいということに。

## <a id="spread">[] リテラル中の .. (spread 演算)</a>

[パターンの方](../list-pattern/index.md)で「`[1, ..[2, 3, 4], 5]` と `[1, 2, 3, 4, 5]` が同じ意味になる」と書きましたが、コレクション リテラル中でも同じく「入れ子のコレクションを展開」みたいな仕様があります。

```csharp {title=".. で入れ子のコレクションを展開"}
int[] array = [ 1, 2, 3 ];
List<int> list = [ 0, ..array, 4 ]; // 0, 1, 2, 3, 4
```

他の言語で unpacking とか splat (* 記号が一部の人にそう呼ばれていて、この機能に * を使ってる言語ではこう呼ぶ)とか spread (拡散)演算子とか呼ばれているやつです。

C# ではまあ、LINQ の `Concat`, `Append`, `Prepend` とかを使って同様のものは書けていましたが、煩雑、かつ、パフォーマンスはいまいちでした。

```csharp {title="Concat, Append, Prepend"}
int[] array1 = { 1, 2, 3 };
int[] array2 = { 4, 5, 6 };

// enumerator のインスタンスが余計に new されたりで遅い。
var linq = array1.Concat(array2).Prepend(0).Append(7);

// 列挙も結構遅い。
foreach (var x in linq)
{
    Console.WriteLine(x);
}

// LINQ のよりも速い実装になる予定(後述)。
// かつ、Preapend よりはだいぶわかりやすい。
var spread = [ 0, .. array1, .. array2, 7 ];
```

## <a id="brace">おまけ: {} 案</a>

一時期はパターンの方も `is {}` にしたいみたいな話もあったんですが。
元々配列初期化子が `{}` ですし、コレクション初期化子も `{}` になる案もなくはなかったです。

ただ、`{}` の用途としては他に [Expression blocks](https://github.com/dotnet/csharplang/issues/3086) という提案も出ていて、それとの弁別が無理そうということで没。

## <a id="lowering">展開結果</a>

展開結果、基本的には「前から順に詰める」です。
配列の場合だと割かしシンプルで、例えば以下のような感じ。

```csharp {title="配列に対するコレクション リテラルの展開結果"}
int[] array1 = { 1, 2, 3 };
int[] array2 = { 4, 5, 6 };

// var spread = [ 0, .. array1, .. array2, 7 ];

var len = 1 + array1.Length + array2.Length + 1;
var spread = new int[len];

var i = 0;
spread[i++] = 0;
for (int j = 0; j < array1.Length; j++, i++) spread[i] = array1[j];
for (int j = 0; j < array2.Length; j++, i++) spread[i] = array2[j];
spread[i] = 7;
```

`Span<T>` の場合には `new T[]` のところを `stackalloc T[]` に変更。
`ReadOnlySpan<T>` の場合はいったん `Span<T>` と同じ処理でデータを書き込んでから、最後に `ReadOnlySpan<T>` に変換。

それ以外の型については「所定のパターンを満たすコンストラクターと `Init` メソッドを呼ぶ」と言うことになっています。

* `capacity` という名前の引数があるコンストラクターがある場合はそれを、なければ引数なしコンストラクターを呼ぶ
* `void Init(T1)` があって、`T1` が `T[]` なら `new[]` で、`T1` が `Span<T>`, `ReadOnlySpan<T>` なら `stackalloc[]` で一時バッファーを作ってから `Init` メソッドに渡す

例えば `Init(int[])` だけ持っている型だと以下のような感じ。

```csharp {title="一時 new int[] が作られるパターン"}
// A a = [ 1, 2, 3 ];
int[] tempA = { 1, 2, 3 };
A a = new();
a.Init(tempA);

class A
{
    public void Init(int[] items) { }
}
```

`capacity` コンストラクターと `Init(ReadOnlySpan<int>)` を持つ型だと以下のような感じ。

```csharp {title="一時 stackalloc int[] が作られるパターン"}
// A a = [ 1, 2, 3 ];
ReadOnlySpan<int> tempA = stackalloc[] { 1, 2, 3 };
A a = new(3);
a.Init(tempA);

class A
{
    public A(int capacity) { }
    public void Init(ReadOnlySpan<int> items) { }
}
```

## immutable コレクション初期化

ちょっと別の機能追加も必要なのでさらに不透明なんですが、
この `[]` リテラルは前に話した [`ImmutableArray` の初期化問題](../immutable-array-init/index.md)の解決策としても期待されています。

とりあえず、`ImmutableArray` についても前節と同じルールで初期化を掛けることを考えます。

```csharp {title="ImmutableArray.Init"}
using System.Collections.Immutable;

// ImmutableArray<int> a = [ 1, 2, 3 ];
ReadOnlySpan<int> tempA = stackalloc[] { 1, 2, 3 };
ImmutableArray<int> a = new();
a.Init(tempA); // こういうメソッドを足したいという話。今はない。
```

こういう `Init` メソッドを足せればいいわけですが、
immutable を名乗る以上、`new()` とは別に呼ばれるとまずいという話になります。

で、そこは[init-only プロパティ](../../../../study/csharp/oop/oo_property.md#init-only-internal)と同じ方式で乗り切りたいとのこと。

任意のメソッドに対して、`new()` 中、もしくは、直後にしか呼ばない・呼ばれない保証をコンパイラーがするような仕様(メソッドに対する `init` 修飾)があればいいわけで、そういう仕様も模索中とのこと。

```csharp {title="init 修飾子"}
struct ImmutableArray<T>
{
    readonly T[] _items;

    // init 修飾を付けたメソッドは new() 内、もしくは、直後でしか呼べないように、
    // コンパイラーが呼び出し箇所をチェックする。
    public init void Init(ReadOnlySpan<T> items)
    {
        // 本来、コンストラクター内でしか書き換えてはいけないはずのフィールドを、
        // init 修飾子が付いたメソッド内に限り書き換え可能にする。
        _items = items.ToArray();
    }
}
```
