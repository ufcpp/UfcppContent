---
title: "IEnumerator の別実装"
source_url: "https://ufcpp.net/blog/2018/12/fastenumerator/"
content_type: "BlogEntry"
published_at: "2018-12-19T10:00:28"
updated_at: "2018-12-19T10:05:44"
tags: []
umbraco_id: 2201
parent_id: 2177
sort_order: 18
aliases: []
---

# IEnumerator の別実装

[Devirtualization 最適化](../devirtualization/index.md)の話で仮想呼び出しのコストの話もしました。
そこでもう1つ思い出してほしいのが、[C# 8.0 Async streams](../cs8asyncstreams/index.md)で書いた、
`IAsyncEnumerator<T>`インターフェイスの話。
最終的な決定としては以下のような API を持っています。

```csharp {title="最終決定時の IAsyncEnumerator"}
public interface IAsyncEnumerator<out T>
{
    T Current { get; }
    ValueTask<bool> MoveNextAsync();
}
```

一方で、検討段階では以下のような API も考えられていました。

```csharp {title="検討段階の IAsyncEnumerator"}
public interface IAsyncEnumerator<out T>
{
    ValueTask<bool> WaitForNextAsync();
    T TryGetNext(out bool success);
}
```

今日はこの後者のメリットについての話。

参考コード: [FastEnumeration](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/PerformanceTips/FastEnumeration)

## IEnumerator 版

同様の話は実は `IEnumerator<T>` にも言えます。
正確に言えば `IAsyncEnumerator<T>` の方が `WaitForNextAsync` を持っている分だけ複雑なんですが、とりあえず単純化のために `IEnumerator<T>` で話を進めます。

`IEnumerator<T>` インターフェイス(`System.Collections.Generic`名前空間)は、
歴史的経緯から非ジェネリックな `IEnumerator` インターフェイス(`System.Collections`名前空間)からの派生になっていますが、
そういう歴史的経緯を抜いて考えれば以下のような API を持つインターフェイスです。

```csharp {title="IEnumerator インターフェイスの本質"}
public interface IEnumerator<out T>
{
    bool MoveNext();
    T Current { get; }
}
```

一方、冒頭で上げた検討段階の `IAsyncEnumerator<T>` と同じ考え方で、
以下のような API での実装が考えられます。
(区別のために名前をちょっと変えて、`IFastEnumerator` にしています。)

```csharp {title="IEnumerator の構造はこっちの方がよかったかもしれない"}
interface IFastEnumerator<out T>
{
    T TryMoveNext(out bool success);
}
```

要するに、`MoveNext`と`Current`に分かれている機能を、1つのメソッドにまとめた方がいいのではないかという話です。

(.NET のジェネリクスでは、[out 引数](../../../../study/csharp/resource/sp_ref.md#out)を[共変](../../../../study/csharp/oop/sp4_variance.md#covariance)にできないという嫌な制限があるので、`success` の方が out 引数になっています。
可能であれば `bool TryMoveNext(out T current)` にしたいものです。
とりあえずそこは今回関係なく、あくまで「メソッドを1つにしたい」というところが今回の話の本質です。)

## 仮想呼び出しのコスト

まあ、冒頭で仮想呼び出しのコストの話を振っているのでどういう問題なのか察してもらえると思います。
単純に、そこそこコストが掛かる仮想呼び出しを2回に分けたくないという話です。

`foreach` が含まれるメソッドはたいてい[インライン展開](../../../../study/csharp/structured/miscinlining.md)されません。
そうなると devirtualization 最適化は大体かからなくなるんですが、
なので、`MoveNext`/`Current` には普通に仮想呼び出しのコストが掛かります。
結果、仮想呼び出しが2回。
一方の `IFastEnumerator<T>` であれば、仮想呼び出しは `TryMoveNext` の1回だけです。

ということで、どのくらい変わるか[ベンチマーク](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/FastEnumeration/FastEnumeration)を用意しました。

配列の中身を列挙するだけのクラスを2つ用意します。
片方は `IEnumerator<T>` 実装(本題に関係するところだけ抜き出し)。

```csharp {title="IEnumerator 実装"}
class NormalEnumerator : IEnumerator<int>
{
    private readonly int[] _data;
    private int _i = -1;
 
    public int Current => _data[_i];
    public bool MoveNext() => ++_i < _data.Length;
}
```

もう一方は `IFastEnumerator<T>` 実装。

```csharp {title="IFastEnumerator 実装"}
class FastEnumerator : IFastEnumerator<int>
{
    private readonly int[] _data;
    private int _i = -1;
 
    public int TryMoveNext(out bool success)
    {
        var i = ++_i;
        var data = _data;
        if ((uint)i < (uint)data.Length)
        {
            success = true;
            return data[i];
        }
        else
        {
            success = false;
            return default;
        }
    }
}
```

これに対して以下のようなループを回します。
(`IEnumerator` 版の方は、まんま、`foreach` の展開結果です。)

```csharp {title="インターフェイスを介して呼ぶと MoveNext/Current のコストが気になる"}
static int VirtualSum(IEnumerator<int> e)
{
    var sum = 0;
    while (e.MoveNext())
    {
        var x = e.Current;
        sum += x;
    }
    return sum;
}

// IFastEnumerator の方が1.5倍くらい速い。
static int VirtualSum(IFastEnumerator<int> e)
{
    var sum = 0;
    while (true)
    {
        var x = e.TryMoveNext(out var success);
        if (!success) break;
        sum += x;
    }
    return sum;
}
```

これの結果は、`IFastEnumerator` 版の方が1.5倍くらい高速です。
`TryMoveNext` の方が複雑なコードになりがちなので最適化も効きにくいんですが、
それ以上に仮想呼び出しのコストが高くて、`TryMoveNext` の方が速くなります。

## インターフェイスをやめてしまえば…

ちなみに、あくまでこれは仮想呼び出しのコストの問題なので、
以下のように、インターフェイスを介さず具象クラスで呼ぶと、
むしろ `MoveNext`/`Current` 型の方が速くなります。

```csharp {title="具象型で呼ぶと別に MoveNext/Current のコストは気にならない"}
// さっきとの違いは引数の型だけ。
// IEnumerator インターフェイスだったのを、NormalEnumerator クラスに変えただけ。
// この場合は普通にこっちの方が速い。
static int NonVirtualSum(NormalEnumerator e)
{
    var sum = 0;
    while (e.MoveNext())
    {
        var x = e.Current;
        sum += x;
    }
    return sum;
}
 
// 同じく、IFastEnumerator インターフェイスを FastEnumerator クラスに変えただけ。
static int NonVirtualSum(FastEnumerator e)
{
    var sum = 0;
    while (true)
    {
        var x = e.TryMoveNext(out var success);
        if (!success) break;
        sum += x;
    }
    return sum;
}
```

とはいえ、汎用性がなくなるので具象クラスで受け渡しするのはちょっとつらいです。
ジェネリクスを使えば多少緩和はされるんですが…

```csharp {title="ジェネリック版"}
// ジェネリクスを使えば、構造体の時には仮想呼び出しが消える。
// (構造体限定。クラスの時は別に仮想呼び出しは消えない。)
static int GenericSum<T>(T e)
    where T : IEnumerator<int>
{
    var sum = 0;
    while (e.MoveNext())
    {
        var x = e.Current;
        sum += x;
    }
    return sum;
}
```

速くなる(仮想呼び出しが消える)のは構造体限定です。
しかもなお悪いことに、`foreach` は、`GetEnumerator` を介する構造なので、
普通にやるとどうやっても仮想呼び出しが消えません。

```csharp {title="foreach はどうやっても仮想呼び出しが残る"}
static int Sum<T>(T items)
    where T : IEnumerable<int>
{
    var sum = 0;
    foreach (var x in items) sum += x;
    return sum;
} 
// ↑は↓みたいに展開される
static int Sum_<T>(T items)
    where T : IEnumerable<int>
{
    var sum = 0;
 
    // この GetEnumerator の仮想呼び出しは消える可能性があるものの…
    IEnumerator<int> e = items.GetEnumerator();
    try
    {
        // 結局ここの MoveNext/Current はインターフェイス越し。
        // 必ず仮想呼び出しになる。
        while (e.MoveNext())
        {
            var x = e.Current;
            sum += x;
        }
    }
    finally
    {
        if (e is IDisposable d) d.Dispose();
    }
    return sum;
}
```

ということで、`foreach` 中の `MoveNext`/`Current` の仮想呼び出しはなかなか消せなかったりします。
なのでなおのこと、1回で済む `TryMoveNext` 型のインターフェイスがよかったかもしれない、という話になったりします。
