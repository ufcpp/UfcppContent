---
title: "C# 14 の破壊的変更点(First-class Span)"
source_url: "https://ufcpp.net/blog/2025/10/first-class-span-breaking-change/"
content_type: "BlogEntry"
published_at: "2025-10-22T21:33:30"
updated_at: "2025-10-22T21:33:30"
tags: []
umbraco_id: 2517
parent_id: 2516
sort_order: 0
aliases: []
---

# C# 14 の破壊的変更点(First-class Span)

C# 14 で導入された [First-class Span](../../../../study/csharp/resource/span.md#first-class-span) は破壊的変更を伴っています。

例えば標準ライブラリの範囲内の拡張メソッド呼び出しでも以下のような差が生じます。

```csharp {title="C# 14 にすると挙動が変わる拡張メソッド呼び出しの例"}
int[] array = [1, 2, 3, 4];

// C# 13 まで: System.Linq.Enumerable.Contains (IEnumerable 引数) が呼ばれる
// C# 14 から: System.MemoryExtensions.Contains (ReadOnlySpan 引数) が呼ばれる
Console.WriteLine(array.Contains(2));
```

ほとんどの場合、「パフォーマンスが上がるだけで得られる結果は同じ」な実装ばかりなのでそんなに問題にはならないだろうということで、
「許容できる範囲内」・「破壊的変更を受け入れるメリットの方が大きい」という判定を受けています。
公式の[.NET 10 での破壊的変更に関するドキュメント](https://learn.microsoft.com/ja-jp/dotnet/core/compatibility/core-libraries/10.0/csharp-overload-resolution)では、
「式ツリーで使っていた場合に問題が起きうる」くらいしか書かれていません。

ただ、これのせいで問題を起こしそうだった拡張メソッドとして `Reverse` があったりします。

```csharp {title="Reverse は危うかった" error-text="array.Reverse()"}
int[] array = [1, 2, 3, 4];

// C# 13 まで: Enumerable.Reverse だったから問題なし。
// C# 14 から: MemoryExtensions.Reverse を呼んじゃいそう…
foreach (var x in array.Reverse())
{
}

// デモ用に同じシグネチャの拡張メソッドをローカル実装。
static class A
{
    // System.Linq.Enumerable にあるのは「新しい IEnumerable インスタンスを作って返す」タイプ。
    public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source) => null!;

    // MemoryExtensions にあるのは「Span に対する自己書き換え」タイプ。
    public static void Reverse<T>(this Span<T> span) { }
}
```

ちなみに、この問題は「[`TSource[]` 引数の `Reverse`](https://learn.microsoft.com/ja-jp/dotnet/api/system.linq.enumerable.reverse?view=net-10.0#system-linq-enumerable-reverse-1(-0())) を足す」という方法で解決しています。
`Span<T>` (first-class とはいえ型変換を挟む)よりも `T[]` (無変換)の方が優先度が高いので、
`array.Reverse()` は `Reverse(T[])` が優先的に呼ばれます。

ここまではあくまで「標準ライブラリの範囲内で」の話。
「自作の LINQ もどき」とかを持っているともう少しいろいろと問題を踏みます。
というか、自分が踏んだという話…
それを2つほど紹介。

## Where(Span)

1個目は以下のような `Where` メソッドです。

```csharp {title="Span 自己書き換え Where"}
static class InPlaceLinq
{
    // Reverse の例と同様、「Span 相手は自己書き換えでいいだろ」的なメソッド。
    public static Span<T> Where<T>(this Span<T> span, Func<T, bool> predicate)
    {
        int count = 0;
        for (int i = 0; i < span.Length; i++)
        {
            if (predicate(span[i]))
            {
                span[count++] = span[i];
            }
        }
        return span[..count];
    }
}
```

先ほどの `Reverse` の例同様、自己書き換え。

自己書き換えな時点で用途はかなり限定的で、
本来は以下のような利用を想定しています。

```csharp {title="本来の「自己書き換え Where」利用"}
struct SomeItem
{
    public bool Flag { get; }
}

class SomeRepository
{
    public SomeItem[] Filter(Func<SomeItem, bool> predicate)
    {
        // 個数の上限がある程度わかってる & 小さいので stackalloc でバッファー確保。
        Span<SomeItem> buffer = stackalloc SomeItem[32];

        // 一覧を取る時点では Where 出来ず一度バッファーに書き込みが必要なメソッドを呼ぶ。
        var written = GetItems(buffer);

        // 最終的には ToArray して返す。
        return [.. buffer[..written].Where(predicate)];
    }

    private int GetItems(Span<SomeItem> destination)
    {
        // 本来は以下の類のコード
        // destination[count++] = ...
        // return count;
        return 0;
    }
}
```

ところが、first-class Span が入ったことで、配列に対して `Enumerable.Where` よりも優先度が高くなってしまい…
意図しないところで呼ばれてしまうことに…

```csharp {title="意図せず「自己書き換え Where」の方が呼ばれてしまった例"}
int[] array = [1, 2, 3, 4];

// C# 13 まで: System.Linq.Enumerable.Where が呼ばれる。
// C# 14 から: InPlaceLinq.Where が呼ばれる。自己書き換え…
var result = array.Where(x => x % 2 == 0);

Console.WriteLine("result");
foreach (var x in result)
{
    Console.WriteLine(x);
}

// C# 14 だと自己書き換えやっちゃってるんで当然…
Console.WriteLine("array");
foreach (var x in array)
{
    Console.WriteLine(x); // 2, 4, 3, 4 になっちゃう。
}
```

この例の特に厳しいところは、コンパイル エラーにはならずに実行できてしまうものの、
実行結果が破滅的に意図しない挙動になるところです…

一応、`Reverse` の例同様、`Where(TSource[], Func<TSource, bool>)` オーバーロードを足して `Enumerable.Where` に流すようにしてしまえば解決できるはずです。

また、自己書き換えな拡張メソッドが非破壊なものと同名なのが問題だったという反省もあり、
メソッド名を変更してしまうべきとう気もします。
(実際、この路線で修正。`WhereInPlace` というあえての長ったらしい名前に変更。)

## Index(ReadOnlySpan)

もう1個は以下のような `Index` メソッド。

```csharp
static class SpanExtensions
{
    // 要は「Span 相手にも Enumerable.Index みたいなものが欲しい」。
    public static IndexEnumerable<T> Index<T>(this ReadOnlySpan<T> span) => new(span);

    public readonly ref struct IndexEnumerable<T>(ReadOnlySpan<T> span)
    {
        private readonly ReadOnlySpan<T> _span = span;
        public IndexEnumerator<T> GetEnumerator() => new(_span);
    }

    public ref struct IndexEnumerator<T>(ReadOnlySpan<T> span)
    {
        private readonly ReadOnlySpan<T> _span = span;
        private int _index = -1;
        public bool MoveNext() => ++_index < _span.Length;
        public readonly (int Index, T Item) Current => (_index, _span[_index]);
    }
}
```

こちらは `Enumerable.Index(this IEnumerable<T>)` と同じ挙動を `ReadOnlySpan<T>` 引数で実装したものです。

まあ、`Span`/`ReadOnlySpan` を使いたいくらいパフォーマンスを気にする場面なら `for (var i = 0; i < span.Length; ++i)` を使えという話はありつつも…

同じ挙動なので、配列に対して呼ばれても問題ないはずでめでたしめでたし(?)

```csharp
int[] array = [1, 2, 3, 4];

// C# 13 まで: System.Linq.Enumerable.Index が呼ばれる。
// C# 14 から: 自作の SpanExtensions.Index が呼ばれる。
//
// 挙動は同じなので特に問題ない。
// むしろパフォーマンス上がるのではないかと。
foreach (var (index, item) in array.Index())
{
    Console.WriteLine($"{index}: {item}");
}
```

問題は `IndexEnumerable` が `ref struct` な点で、
`foreach` 中に `yield` や `await` があるとエラーになります。

```csharp {error-lines="4"}
int[] array = [1, 2, 3, 4];

// C# 14 で 自作の SpanExtensions.Index が呼ばれようになると…
foreach (var (index, item) in array.Index())
{
    // さっきとの差は await を含んでることだけ。
    await Task.Delay(1);
    Console.WriteLine($"{index}: {item}");
}
```

「 'SpanExtensions.IndexEnumerator&lt;int&gt;' 型のインスタンスは、'await' または 'yield' 境界を越えて保持することはできません。」というコンパイル エラーが出るはずです。

これはまあ、ほとんどの場合は `Index(ReadOnlySpan)` が呼ばれた方が好ましい中、
少数の `yield`/`await` を含むケースでだけ問題になるので、
コンパイル エラーが出た場所を `Enumerable.Index` (拡張メソッドをやめて静的メソッド呼び)に書き換えるなどで対処しました。
(それか、名前空間の内側に `using static System.Linq.Enumerable;` を書いて `Enumerable.Index` の優先度を上げるという手もあります。)

あとこれも一応、配列用のオーバーロード追加でも問題解消するはずです。
