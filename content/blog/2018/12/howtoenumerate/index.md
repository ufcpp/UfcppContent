---
title: "foreach の掛け方いろいろ"
source_url: "https://ufcpp.net/blog/2018/12/howtoenumerate/"
content_type: "BlogEntry"
published_at: "2018-12-20T10:06:39"
updated_at: "2018-12-20T10:06:39"
tags: []
umbraco_id: 2202
parent_id: 2177
sort_order: 19
aliases: []
---

# foreach の掛け方いろいろ

[IEnumerator の別実装](../fastenumerator/index.md)で、
インターフェイス越しの `foreach` には仮想呼び出しのコストが結構掛かっているという話を書きました。
(そちらでの主題は「なので、`MoveNext`/`Current`の2つに分かれているのはちょっともったいない」という話でした。
もちろん、それを気にしないといけないのは大体パフォーマンス最優先のエクストリームな状況だけです。)

あと、[配列のインデクサー](../arrayindexer/index.md)では、配列と[`Span<T>`構造体](../../../../study/csharp/resource/span.md)の列挙には C# のレベルでも JIT のレベルでも最適化が掛かっていて、かなり速いという話をしました。

今回はその辺りを踏まえて、列挙の仕方をいろいろ比較。

参考コード: [ArrayEnumeration](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/PerformanceTips/ArrayEnumeration)

## 内部的には配列なコレクション

`List<T>` (`System.Collections.Generic` 名前空間)とか、
`ImmutableArray<T>` (`System.Collections.Immutable` 名前空間)とか、
内部的に配列を持っていて、その上に何か機能を重ねたり(あるいは逆に書き換えを制限したり)している型は結構あります。
今回はその手の型の列挙について考えます。

とりあえず以下のような型を用意。参考にするために、配列を生列挙するコードも書いておきます。

```csharp {title="配列の生列挙"}
using BenchmarkDotNet.Attributes;
 
public partial struct ArrayWrapper<T>
{
    // 比較のために生列挙をしたいので public (本来は不要というかむしろダメ)
    public readonly T[] Array;
    public ArrayWrapper(T[] array) => Array = array;
}
 
public partial class ArrayEnumerationBenchmark
{
    public ArrayWrapper<int> _array;

    // 比較のための生列挙。
    [Benchmark(Baseline = true)]
    public int RawEnumeration()
    {
        var sum = 0;
        foreach (var x in _array.Array) sum += x;
        return sum;
    }
}
```

とりあえず、結果:

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |

## IEnumerable の実装

`foreach`で使いたいというのが主題なので、とりあえず先ほどの型に `IEnumerable<T>` インターフェイスを実装してみます。

とはいえ、インターフェイスを介した `GetEnumerator`/`MoveNext`/`Current` はちょっとオーバーヘッドが掛かるので、以下のような作りにします。
(`List<T>` なんかはまさにこの作りになっています。)

```csharp {title="IEnumerable 化"}
public partial struct ArrayWrapper<T> : IEnumerable<T>
{
    // 専用の型を作って、それを具象型のまま公開する
    public Enumerator GetEnumerator() => new Enumerator(Array);
 
    // インターフェイスは明示的実装にして別実装
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new EnumeratorObject(Array);
    IEnumerator IEnumerable.GetEnumerator() => new EnumeratorObject(Array);
}
```

### 専用実装(構造体)

まずは専用実装の方。
配列の全要素を列挙するような `IEnumerator<T>` 実装は以下のようになります。
無駄なアロケーションが発生しないように構造体製。

```csharp {title="構造体で専用実装"}
public partial struct ArrayWrapper<T>
{
    // 「仮想呼び出しは遅い」ということがわかっているわけで、
    // こんな感じで具象型を返す GetEnumerator を作った方が高速。
    // 構造体にした方が最適化が効く。
    public Enumerator GetEnumerator() => new Enumerator(Array);
 
    public struct Enumerator : IEnumerator<T>
    {
        private readonly T[] _array;
        private int _i;
        internal Enumerator(T[] array) => (_array, _i) = (array, -1);
 
        public T Current => _array[_i];
        public bool MoveNext() => ((uint)++_i) < (uint)_array.Length;
        // 残りは省略
    }
}
 
public partial class ArrayEnumerationBenchmark
{
    // 構造体の Enumerator 越しの列挙
    // 構造体で返してるとほんとにきっちり最適化が効くみたいで、
    // ほぼ配列生列挙と同じ速度が出る。
    [Benchmark]
    public int StructEnumeration()
    {
        var sum = 0;
        foreach (var x in _array) sum += x;
        return sum;
    }
}
```

これを使って `foreach (var x in _array)` とすると、
`MoveNext`も`Current`もインライン展開されて、
最適化でほとんど配列の生列挙と同じコードに展開されます。
誤差の範囲内で生列挙と同じ速度が出ます。


|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |

### インターフェイス実装

構造体実装なものだけでは `IEnumerable<T>` インターフェイスの要件を満たさないので、
別途明示的実装を足します。

このとき、実装要件を満たすだけなら `IEnumerator<T> IEnumerable<T> GetEnumerator() => GetEnumerator();` (構造体実装の `GetEnumerator` を素通しするだけ)でも構いません。
ただ、構造体をインターフェイス化して使うとかえって遅くて、
少しでもパフォーマンスを上げたりならクラスで作り直す方がよかったりします。

```csharp {title="インターフェイス実装"}
public partial struct ArrayWrapper<T> : IEnumerable<T>
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new EnumeratorObject(Array);
    IEnumerator IEnumerable.GetEnumerator() => new EnumeratorObject(Array);
 
    // 構造体の Enumerator と中身は全く同じで、ただクラスになってるだけ。
    // 構造体をインターフェイス越しに返すとかえって遅くなるので、こんなクラスが別途必要に…
    public class EnumeratorObject : IEnumerator<T>
    {
        private readonly T[] _array;
        private int _i;
        internal EnumeratorObject(T[] array) => (_array, _i) = (array, -1);
 
        public T Current => _array[_i];
        public bool MoveNext() => ((uint)++_i) < (uint)_array.Length;
 
        object IEnumerator.Current => Current;
        public void Dispose() { }
        public void Reset() => throw new NotImplementedException();
    }
}
 
public partial class ArrayEnumerationBenchmark
{
    // インターフェイス越し列挙になるように、IEnumerable<T> にキャストして使ってる。
    // びっくりするくらい遅い。
    // StructEnumeration とかに比べて10倍遅い。
    [Benchmark]
    public int InterfaceEnumeration()
    {
        var sum = 0;
        foreach (var x in (IEnumerable<int>)_array) sum += x;
        return sum;
    }
}
```

構造体/具象型実装が配列生列挙とそん色ないのに対して、
こいつは10倍以上遅いです。
それでも、別途クラスで実装した方がちょっとだけマシ。

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |
|           InterfaceEnumeration | 4,407.3 ns | 14.790 ns | 13.8350 ns | 11.43 |    0.05 |

## 出来合いの型

おまけで、出来合いの型を被せて返すのもやっておきます。

### ReadOnlyCollection

配列を生で返したくない状況の1つが、書き換えを認めたくない場合です。
そういう場合、
`ReadOnlyCollection<T>` クラス(`System.Collections.ObjectModel` 名前空間)を使ったりします。

ただ、このクラス、`IList<T>` 向けなので、
配列だけでいいときには余計(繰り返しますが、インターフェイス越しは遅い)ですし、
.NET Framework 2.0 時代からあってパフォーマンスへの考慮はあんまりない型です。
要するに、遅い…

```csharp {title="ReadOnlyCollection 越しの列挙"}
public partial struct ArrayWrapper<T>
{
    public ReadOnlyCollection<T> AsReadOnlyCollection() => new ReadOnlyCollection<T>(Array);
}
 
public partial class ArrayEnumerationBenchmark
{
    // ReadOnlyCollection<T> 列挙。
    // InterfaceEnumeration 以上に遅い。とにかく遅い。
    // ReadOnlyCollection<T> は内部的に IList<T> 越しに配列アクセスするので、それがほんとに遅い。
    [Benchmark]
    public int ReadOnlyCollectionEnumeration()
    {
        var sum = 0;
        foreach (var x in _array.AsReadOnlyCollection()) sum += x;
        return sum;
    }
}
```

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |
|           InterfaceEnumeration | 4,407.3 ns | 14.790 ns | 13.8350 ns | 11.43 |    0.05 |
|  ReadOnlyCollectionEnumeration | 5,199.8 ns | 21.591 ns | 20.1960 ns | 13.48 |    0.07 |

### Span

まあ、今なら、特に .NET Core を使えるのであれば、
`ReadOnlySpan<T>` 構造体 (`System` 名前空間)を使うのがいいと思います。
`Span<T>` と同様最適化が掛かるので、
書き換えを防止しつつ、配列の生列挙とそん色ない速度が出ます。

```csharp {title="ReadOnlySpan 越しの列挙"}
public partial struct ArrayWrapper<T>
{
    // インデクサーも使いたいとき用、その2。
    // Span<T> を介してみる。
    // パフォーマンスに焦点が当たってた .NET Core 2.1 世代の型だけあって、かなり速い。
    public ReadOnlySpan<T> AsSpan() => Array;
}
 
public partial class ArrayEnumerationBenchmark
{
    // Span<T> 列挙
    // こいつも配列生列挙とほぼ同じ性能。速い。
    [Benchmark]
    public int SpanEnumeration()
    {
        var sum = 0;
        foreach (var x in _array.AsSpan()) sum += x;
        return sum;
    }
}
```

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |
|           InterfaceEnumeration | 4,407.3 ns | 14.790 ns | 13.8350 ns | 11.43 |    0.05 |
|  ReadOnlyCollectionEnumeration | 5,199.8 ns | 21.591 ns | 20.1960 ns | 13.48 |    0.07 |
|                SpanEnumeration |   385.0 ns |  1.612 ns |  1.5079 ns |  1.00 |    0.00 |

## `Span<T>` の利用率アップ

ここまでの説明の通り下手すると10倍性能が違ったりするので、
.NET Core 2.1 が出て以降、
`Span<T>`や`ReadOnlySpan<T>`を引数に取るAPIが増えていたりします。
`IEnumerable<T>`や`IList<T>`が減って。

そうなると、既存のコレクションに対しても、「可能なもの(内部的に配列とか連続したデータになってるやつ)は`Span`で取りたい」という要求がかなり高くなっています。

が、既存の型を改修してもらえるまで待てないという人も… 
`Unsafe` な手段で無理やり中身の配列を取得して、
無理やり `Span` にしてしまったり…

- [Add Span and Memory conversion methods for ImmutableArray](https://github.com/dotnet/roslyn/pull/31785)

過渡的な手段とはいえ、結構邪悪です。
以下のようなコード。

```csharp {title="ImmurableArray を無理やり配列に変換"}
[StructLayout(LayoutKind.Sequential)]
private struct ImmutableArrayProxy<T>
{
    internal T[] MutableArray;
}
 
internal static T[] DangerousGetUnderlyingArray<T>(this ImmutableArray<T> array)
     => Unsafe.As<ImmutableArray<T>, ImmutableArrayProxy<T>>(ref array).MutableArray;
```

また、`Span`を使うと、中身を参照で外に漏らしちゃうことになるので、
ちょっと変な挙動をすることがあります。
以下のようなコードには注意を。

```csharp {title="Span で中身を返す場合の注意点"}
using System;
 
// System.Collections.Generic.List<T> と同じような実装 + AsSpan
class List<T>
{
    private T[] _buffer;
    private int _count;
    public List(int capacity) => _buffer = new T[capacity];
 
    public ref T this[int index] => ref _buffer[index];
    public ReadOnlySpan<T> AsSpan() => _buffer.AsSpan(0, _count);
 
    public void Add(T item)
    {
        if(_count == _buffer.Length)
        {
            var newBuffer = new T[_buffer.Length * 2];
            _buffer.AsSpan().CopyTo(newBuffer);
            _buffer = newBuffer;
        }
        _buffer[_count++] = item;
    }
}
 
public class Program
{
    static void Main()
    {
        var list = new List<int>(2);
        list.Add(1);
        list.Add(2);
        // この時点で容量満杯
 
        // Span 取得してから…
        var span = list.AsSpan();
 
        list.Add(3);  // Add で内部バッファーの再確保が発生
        list[0] = 99; // 新しいバッファーへの書き込み
 
        Console.WriteLine(span[0]); // 古いバッファーを参照してるので 1 のまま
        Console.WriteLine(list[0]); // 新しいバッファーを参照してるので 99
    }
}
```

## まとめ

- `GetEnumerator` を実装するなら、専用の構造体をまず考える
- それとは別に、`IEnumerable<T>.GetEnumerator` を明示的実装
- `Span<T>`/`ReadOnlySpan<T>` 速い
  - ので、パフォーマンス的には `Span` を使いたい
  - でも、参照が外に漏れるので注意
