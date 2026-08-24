---
title: "ref 構造体のインターフェイス実装 / 型引数での使用"
source_url: "https://ufcpp.net/blog/2024/2/ref-struct-interface/"
content_type: "BlogEntry"
published_at: "2024-02-11T11:53:20"
updated_at: "2024-02-11T11:53:20"
tags: []
umbraco_id: 2483
parent_id: 2480
sort_order: 2
aliases: []
---

# ref 構造体のインターフェイス実装 / 型引数での使用

[ref 構造体](../../../../study/csharp/resource/refstruct.md)で説明しているように、
[`Span<T>`](../../../../study/csharp/resource/span.md) 型など一部の型は「スタック上にないといけない」という強い制約があります。

この制約を守るため、これまで、ref 構造体は

* インターフェイスを実装できなかった
* ジェネリック型引数に使えなかった

という制限が掛かっていました。

C# 13 では、この制限を緩和するため、
ジェネリック型引数に「`allows ref struct`」という「アンチ制約」を追加する予定です。

こういう案自体は [ref フィールド](../../../../study/csharp/resource/refstruct.md#ref-field)が追加された C# 11 (2022年)の頃から温められてはいたんですが、
いよいよ C# 13 で本格的に取り組むみたいです。
.NET 8/C# 12 がリリースされた後くらいからちらほら提案ドキュメントの更新あり。

* [Add draft for demonstrating ref-struct-constraint soundness](https://github.com/dotnet/csharplang/pull/7769)
* [Update ref struct interfaces based on LDM discussions](https://github.com/dotnet/csharplang/pull/7865)
* [ref struct interfaces updates](https://github.com/dotnet/csharplang/pull/7911)

ちなみに、ランタイム側はその2022年頃に対応すでに入っているみたいです。

* [Design to support ByRefLike types in Generics](https://github.com/dotnet/runtime/pull/67129)
* [Support ByRefLike types as Generic parameters](https://github.com/dotnet/runtime/pull/67783)

## ref 構造体の制限緩和の要求

わかりやすい例でいうと、`Span<T>` は `IEnumerable<T>` であってほしいというものです。
C# 12 時点だと、以下のような2重実装を余儀なくされています。

```csharp {title="C# 12 時点では IEnumerable と Span の2重実装が必須"}
List<int> list = [1, 2, 3, 4, 5];
ReadOnlySpan<int> span = [1, 2, 3, 4, 5];

Console.WriteLine(MyMath.Sum(list));
Console.WriteLine(MyMath.Sum(span));

static class MyMath
{
    public static int Sum(IEnumerable<int> numbers)
    {
        var sum = 0;
        foreach (var x in numbers) sum+= x;
        return sum;
    }

    // メソッドの中身全く同じ。
    // Span/ReadOnlySpan が IEnumerable じゃないので別メソッドでの実装が必須。
    public static int Sum(ReadOnlySpan<int> numbers)
    {
        // 実装的に、numbers をボックス化したり、ref フィールドを外に漏らしたりもしてない。
        // IEnumerable に対する実装をそのまま使って何も問題ない。
        var sum = 0;
        foreach (var x in numbers) sum += x;
        return sum;
    }
}
```

ref 構造体にインターフェイス実装を持たせること自体はそこまで問題ではありません。
問題は、以下のように、「インターフェイス型の変数に直接代入してしまうとボックス化を起こしてまずい」という点です。

```csharp {title="Span をインターフェイス型変数に代入しちゃダメ" error-ranges="sha256:22cfd56643190261f8c3b27fa6cb6e13b99b9edbe68552837722dc0ccdd90efe;6:22-6:26" error-diagnostics="sha256:22cfd56643190261f8c3b27fa6cb6e13b99b9edbe68552837722dc0ccdd90efe;CS0029@6:22-6:26"}
Span<int> span = [1, 2, 3, 4, 5];

// たとえ、Span が IEnumerable<T> を実装していたとしても、
// 以下のようなコードを書くとこの時点でボックス化が起きる。
// span がヒープに漏れてしまうのでまずい。
IEnumerable<int> e = span;
```

じゃあどうすべきかというと、ジェネリクスを介します。

```csharp {title="ジェネリクスを介すればいい" error-text="Sum&lt;int, Span&lt;int&gt;&gt;" error-diagnostics="sha256:34c0a06f6a606e392234f6865596b2ed0cf032ede1634f9ba1b013124a564c63;CS0306@14:1-14:20"}
Span<int> span = [1, 2, 3, 4, 5];

// ジェネリクスを介すれば、ボックス化を起こさずにインターフェイスのメンバーを呼べる。
// (前述の問題はクリア。)
static T Sum<T, TEnumerable>(TEnumerable list)
    where TEnumerable : IEnumerable<T>
{
    // 省略
    return default!; // 仮
}

// なので残る問題はこっち。
// ref 構造体を型引数に渡したい。
Sum<int, Span<int>>(span);
```

ということで次節で説明する「アンチ制約」が必要になります。

## アンチ制約

ジェネリック型制約(`where T :` みたいなやつ)は、普通、制限を掛けることで、

* メソッド内で `T`に対して できること(呼べるメソッドとか)が増える
* その代わり、呼び出し側で `T` に対して渡せる型が減る

というものになります。

```csharp {title="型制約" error-text="M2&lt;object&gt;" error-diagnostics="sha256:a48b3621af7d96138e66c786d69242b33aea988fc86fe7f02b90b9e84c8aa436;CS0311@19:1-19:11"}
// 制限なし。
static void M1<T>() { }

// 何の型でも渡せる。
M1<int>();
M1<string>();
M1<object>();

// 制限あり。
static void M2<T>() where T:ISpanParsable<T>
{
    // 呼べるメソッドが増える。
    T value = T.Parse("123", null);
}

// 渡せる型が減る。
M2<int>();
M2<string>();
M2<object>(); // コンパイルエラー。
```

ところが今回、「ref 構造体を渡せるようにしたい」という逆の要件なので、「制約」ではなく「アンチ制約(制約の撤回)」が必要になります。

[2年くらい前のブログ](../../../2022/2/ref-generic-arguments/index.md)でちょこっと触れていますが、
逆のことをするのに `where T : ref struct` とは書かせたくないようで、ちょっと別文法を模索していました。
当初案だと `allow T : ref struct` とかも検討されていたんですが、
結局は `where T : allows ref struct` (where はそのまま。制約の前に allows)になりそうです。

```csharp
// allows で制限を緩める。
static void M3<T>(T x)
    where T : allows ref struct // アンチ制約。
{
    // メソッド内でできることが減る。
    object obj = x; // box 化ダメ。エラーにする予定。
}

// 渡せる型が増える。
M3<int>();
M3<string>();
M3<object>();
M3<Span<int>>(); // allows ref struct がないと呼べない。
```

ちなみに、`where T : IDisposable, allows ref struct` みたいに、制約とアンチ制約は並べて書けます。
