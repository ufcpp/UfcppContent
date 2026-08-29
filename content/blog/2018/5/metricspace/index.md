---
title: "C# にも型クラス(Shapes)が欲しい… 距離空間上のアルゴリズム実装"
source_url: "https://ufcpp.net/blog/2018/5/metricspace/"
content_type: "BlogEntry"
published_at: "2018-05-03T16:46:26"
updated_at: "2018-05-04T04:10:15"
tags: []
umbraco_id: 2151
parent_id: 2150
sort_order: 0
aliases: []
---

# C# にも型クラス(Shapes)が欲しい… 距離空間上のアルゴリズム実装

今日は C# で「距離の計算」を汎用的に、かつ、高パフォーマンスでやりたいという話。
というか、やりたいのはやまやまなんだけど、高パフォーマンスを目指すとなかなか大変なことになるという話。
[Shapes](../../../2017/2/pickuproslyn0223/index.md)が来れば楽になるはずだけども、計画上だいぶ先の話なので、待っていると厳しいので大変なのを我慢したという話でもあります。

サンプル コードの全体像: [MetricSpace](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/MetricSpace)

## <a id="distance">距離</a>

2つの何らかの情報の距離を求めたいことは結構あります。

- [近いもの同士でグルーピングしたい](https://ja.wikipedia.org/wiki/%E3%83%87%E3%83%BC%E3%82%BF%E3%83%BB%E3%82%AF%E3%83%A9%E3%82%B9%E3%82%BF%E3%83%AA%E3%83%B3%E3%82%B0)
- [最短経路を求めたい](https://ja.wikipedia.org/wiki/%E6%9C%80%E7%9F%AD%E7%B5%8C%E8%B7%AF%E5%95%8F%E9%A1%8C)
- [一定範囲に入っているものだけを取り出したい](https://docs.unity3d.com/jp/current/Manual/OcclusionCulling.html)

汎用性が要らないなら簡単な話で、以下のようなコードで書けます。

```csharp {title="float の配列に対するユークリッド距離"}
class Euclidean
{
    // a と b の長さが同じとか、いくつか前提を置いちゃってるけども、最低限のコード
    // a, b を N 次元空間上の点とみなして、その間の距離の2乗、
    // 要するに「差の2乗和」を求める。
    public static float DistanceSquared(float[] a, float[] b)
    {
        var d = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            var dif = b[i] - a[i];
            d += dif * dif;
        }
        return d;
    }
}
```

ここで、汎用性を気にすると以下のような要望が出てきます。

- 数値の型: `float`以外にも使いたい
- 数値の「組」の型: 配列じゃなくしたい
- 距離計算の方法: 2乗和(いわゆる[ユークリッド距離](https://ja.wikipedia.org/wiki/%E3%83%A6%E3%83%BC%E3%82%AF%E3%83%AA%E3%83%83%E3%83%89%E8%B7%9D%E9%9B%A2) )だけが距離じゃない

## <a id="arithmetic">数値の型</a>

距離計算に使う演算は、和、差、積程度です。
あと、「同じ点かどうか」は知りたいことが多いので、等値比較はしたいでしょう。
距離を計算したあと、「一定範囲に収まっているかどうか」を判定することが多いので、大小比較くらいは必要です。

とはいえ、`float`に限らず、数値型ならどの型でもこの条件くらいは満たします。
実際例えば、「四角いマス目の上の点」みたいなのを考えると`float`よりも`int`が使いたくなります。
精度が必要な場合には`double`や`decimal`を使いたくなるでしょうし、省メモリ都合で`short`を使いたい場合もあるでしょう。

となったときに問題になるのが、C# では、数値の四則演算を素直にジェネリックにできないこと。
以下のコードはコンパイルできません。

```csharp {title="ジェネリックな型には演算子が使えない" error-ranges="7:15-7:16,10:28-10:29"}
// int や double でも使いたいからと言って、以下のようには書けない。
// ジェネリックな型 T には +, -, * が定義されていない。
class Euclidean<T>
{
    public static T DistanceSquared(T[] a, T[] b)
    {
        T d = 0;
        for (int i = 0; i < a.Length; i++)
        {
            var dif = b[i] - a[i];
            d += dif * dif;
        }
        return d;
    }
}
```

しょうがなく、以下のように書いたりします。

```csharp
interface IArithmetic<T>
{
    T Zero { get; }
    T Add(T a, T b);
    T Subtract(T a, T b);
    T Multiply(T a, T b);
}

class Euclidean<T>
{
    // 四則演算用のインターフェイスを外からもらう
    IArithmetic<T> _arithmetic;
    public Euclidean(IArithmetic<T> arithmetic) => _arithmetic = arithmetic;


    // static にするのはあきらめる
    public T DistanceSquared(T[] a, T[] b)
    {
        var arith = _arithmetic;
        // IArithmetic<T> 越しに 0 をもらったり、四則演算したり
        var d = arith.Zero;
        for (int i = 0; i < a.Length; i++)
        {
            var dif = arith.Subtract(b[i], a[i]);
            var sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        return d;
    }
}
```

が、これだと、

- `IArithmetic<T>`や`Euclidean<T>`のインスタンスを持ちまわすのが大変面倒
- 仮想呼び出しのせいで[インライン化](../../../../study/csharp/structured/miscinlining.md)が効かなくなってものすごく遅い

という問題があります。

で、ちょっとしたトリックなんですが、[値型ジェネリックを使うとインライン化が効く](../../../../study/csharp/oop/sp2_generics.md#pseudo-static)という黒魔術がありまして。
以下のように書けば倍は速くなります。

```csharp {title="値型ジェネリックで四則演算"}
class Euclidean<T, TArithmetic>
    // 構造体にして、型引数で受け取る
    where TArithmetic : struct, IArithmetic<T>
{
    public static T DistanceSquared(T[] a, T[] b)
    {
        // default を使って IArithmetic<T> を作る
        var arith = default(TArithmetic);
        // あとは先ほどと同じ
        var d = arith.Zero;
        for (int i = 0; i < a.Length; i++)
        {
            var dif = arith.Subtract(b[i], a[i]);
            var sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        return d;
    }
}

struct FloatArithmetic : IArithmetic<float>
{
    public float Zero => 0;
    public float Add(float a, float b) => a + b;
    public float Multiply(float a, float b) => a - b;
    public float Subtract(float a, float b) => a * b;
}

// IntArithmetic, DoubleArithmetic, ...
// 使いたい型の分だけ同じ IArithmetic<T> を書く

class Program
{
    static void Main()
    {
        // FloatArithmetic の時点で T は float で確定なんだけど、残念ながら型推論はされない
        // 常にこの2つの型引数をペアで渡さないといけない
        Euclidean<float, FloatArithmetic>.DistanceSquared(new[] { 1f, 2f }, new[] { 3f, 4f });
    }
}
```

一応これで、最初の `float` 専用で書いたコードに近いパフォーマンスになります。
まあ、面倒も多々あって、特に大変なのが、型引数を常にペアで渡さないと行けなくなる部分です。
この先、さらにどんどん面倒になって行くんですが、もうすでにこの時点で相当面倒です…

## <a id="fixed-array">数値の「組」の型</a>

次の課題は、配列を避けたいという点。
前述の例でも、メソッド呼び出しの際に `new[] { 1f, 2f }` とか書いていますが、
配列は[ヒープ](../../../../study/computer/essential-software/memorymanagement.md#heap)を使ってしまうので、
今回のような用途ではパフォーマンス上、あまり好ましくありません。

今回の用途だと、

- 要素数が常に固定
- しかも、よく使うのはせいぜい2次元か3次元

ということで、配列の代わりに以下のような構造体を使いたくなったりします。

```csharp
struct Array1<T>
{
    public T Item1;
}

struct Array2<T>
{
    public T Item1;
    public T Item2;
}

struct Array3<T>
{
    public T Item1;
    public T Item2;
    public T Item3;
}

// 以下、必要なだけ ArrayN を用意
```

で、以下の理由から、こいつに対しても先ほどと同様の「値型ジェネリックを使ったトリック」が必要になります。

- 固定長の配列なんだから、長さを静的に取得したい
- 構造体は、自身のフィールドを `ref` 戻り値で返せない

```csharp {error-ranges="11:59-11:64"}
struct Array2<T>
{
    public T Item1;
    public T Item2;

    // これをジェネリックに使いたければトリックが必要
    public static int Length => 2;

    // ただでさえ、safe にインデックス アクセスを実現する方法はないんだけど…
    // そもそも、C# の構造体は ref Item1 したものを、ref 戻り値では返せない仕様
    public ref T this[int index] => ref Unsafe.Add<T>(ref Item1, index);
}
```

その結果、行きつく先は以下のようなコードになります。

```csharp {title="値型ジェネリックを使った固定長配列"}
// 配列自体用。これは大して意味は持ってない。誤用防止程度
public interface IFixedArray<T> { }

// 値型ジェネリック トリック用
public interface IFixedArrayAccessor<T, TArray>
    where TArray : struct, IFixedArray<T>
{
    TArray New();
    ref T At(ref TArray array, int i);
    int Length { get; }
}

public struct Fixed2<T> : IFixedArrayAccessor<T, Fixed2<T>.Array>
{
    public struct Array : IFixedArray<T>
    {
        public T Item1; public T Item2;
        public Array(T item1, T item2) => (Item1, Item2) = (item1, item2);
        public static implicit operator Array((T, T) value) => new Array(value.Item1, value.Item2);
    }

    public Array New() => default;
    public int Length => 2;
    public unsafe Span<T> AsSpan(ref Array array) => new Span<T>(Unsafe.AsPointer(ref array.Item1), 2);
    public ref T At(ref Array array, int i) => ref AsSpan(ref array)[i];
    // 範囲チェックをさぼる(危険でいい)なら以下の書き方でも OK
    //public ref T At(ref Array array, int i) => ref Unsafe.Add(ref array.Item1, i);
}
```

この時点で結構悩ましいコードですが、されにこれを距離計算に組み込むと以下のようになります。

```csharp {title="固定長配列を距離計算に組み込み"}
class Euclidean<T, TArithmetic, TArray, TArrayAccessor>
    where TArithmetic : struct, I/OArithmetic<T>
    where TArray : struct, IFixedArray<T>
    where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
{
    public static T DistanceSquared(TArray a, TArray b)
    {
        var arith = default(TArithmetic);
        var accessor = default(TArrayAccessor);
        var d = arith.Zero;
        for (int i = 0; i < accessor.Length; i++)
        {
            var dif = arith.Subtract(accessor.At(ref b, i), accessor.At(ref a, i));
            var sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        return d;
    }
}

class Program
{
    static void Main()
    {
        // これも、Fixed2<float> を使う時点で残りの型引数確定なんだけど、残念ながら型推論はされない
        // 常にこの4つの型引数が必要
        Euclidean<float, FloatArithmetic, Fixed2<float>.Array, Fixed2<float>>.DistanceSquared((1, 2), (3, 4));
    }
}
```

型引数が4つに増えました。
しかし、実際のところ意味がある情報は、`float`、「2次元」の2つだけです。
気持ち的には `Euclidean<float, 2>` とだけ書きたいですが、C# では叶いません。

## <a id="metric">距離計算の方法</a>

最後に、距離の計算自体も汎用化してみましょう。

距離にもいろいろあります。
ぶっちゃけ、「非負」「三角不等式が成り立つ」の2点だけ満たしていれば何でも距離です。
[ユークリッド距離](https://ja.wikipedia.org/wiki/%E3%83%A6%E3%83%BC%E3%82%AF%E3%83%AA%E3%83%83%E3%83%89%E8%B7%9D%E9%9B%A2)以外でそこそこよく使うものだと以下のようなものがあります。

- [マンハッタン距離](https://ja.wikipedia.org/wiki/%E3%83%9E%E3%83%B3%E3%83%8F%E3%83%83%E3%82%BF%E3%83%B3%E8%B7%9D%E9%9B%A2)
  - 絶対値の和
  - 京都やマンハッタンの街みたいに碁盤の目になっている都市での2点間の距離
  - 「一定距離にある点」をつなぐと、ダイアモンド型になる
- [チェビシェフ距離](https://ja.wikipedia.org/wiki/%E3%83%81%E3%82%A7%E3%83%93%E3%82%B7%E3%82%A7%E3%83%95%E8%B7%9D%E9%9B%A2)
  - 絶対値の最大値
  - チェスや将棋みたいに、斜めにも動ける駒にとっての盤面の距離
  - 「一定距離にある点」をつなぐと、四角になる

これも、汎用化するだけならインターフェイスを1個用意するだけなんですが、
パフォーマンスを考えると値型ジェネリックを使うことになります。
行きつく先が以下のようなコード。

```csharp {title="距離もジェネリック化"}
interface IMetric<T, TArray>
    where TArray : struct, IFixedArray<T>
{
    T DistanceSquared(TArray a, TArray b);
}

struct EuclideanMetric<T, TArithmetic, TArray, TArrayAccessor> : IMetric<T, TArray>
    where TArithmetic : struct, IArithmetic<T>
    where TArray : struct, IFixedArray<T>
    where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
{
    public T DistanceSquared(TArray a, TArray b)
    {
        var arith = default(TArithmetic);
        var accessor = default(TArrayAccessor);
        var d = arith.Zero;
        for (int i = 0; i < accessor.Length; i++)
        {
            var dif = arith.Subtract(accessor.At(ref b, i), accessor.At(ref a, i));
            var sq = arith.Multiply(dif, dif);
            d = arith.Add(d, sq);
        }
        return d;
    }
}

// Manhattan とか Chebychev とかも同様に作る

class Program
{
    // 近い方の点を求める
    static TArray Nearest<T, TArray, TMetric>(TArray origin, TArray a, TArray b)
        where T : IComparable<T>
        where TArray : struct, IFixedArray<T>
        where TMetric : struct, IMetric<T, TArray>
    {
        var metric = default(TMetric);

        var da = metric.DistanceSquared(origin, a);
        var db = metric.DistanceSquared(origin, b);

        return da.CompareTo(db) <= 0 ? a : b;
    }

    static void Main()
    {
        // 型引数は3つと思いきや、Euclidean がさらに4つ求めるので合計7つ
        // 常にこの7つの型引数が必要
        var n = Nearest<float, Fixed2<float>.Array, EuclideanMetric<float, FloatArithmetic, Fixed2<float>.Array, Fixed2<float>>>(
            (0, 0), (1, 2), (3, 4));

        Console.WriteLine((n.Item1, n.Item2));
    }
}
```

型引数だけで画面の横幅目いっぱい使うようなメソッドができました…
もちろん、意味がある部分は`float`, `2`, `Euclidean`だけで、残りは冗長です。

## <a id="instantiation">ごまかし</a>

[というようなコードを書くことに最近迫られまして](https://github.com/ufcpp/KdTree/)。
(元々公開されていたリポジトリからフォークして、上記のようなトリックを仕込んでパフォーマンス向上する作業をした。)
汎用化を捨てたり、パフォーマンスをあきらめてもよかったんですが。
なんとなくきっちりやっちゃいまして。

最初はしょうがなく7つの冗長な型引数を書いてたんですが、
やっぱりすぐにつらくなって断念。
代わりに、以下のようなごまかしコードを書くことになりました。

```csharp {title="派生でごまかす"}
// ジェネリックな型を1個用意しておいて、派生で型引数を与えておく
// 数値の型
class FloatPoint : Point<float, FloatArithmetic> { }
class DoublePoint : Point<double, DoubleArithmetic> { }
class IntPoint : Point<int, IntArithmetic> { }
class ShortPoint : Point<short, ShortArithmetic> { }

class Point<T, TArithmetic>
    where T : IComparable<T>
    where TArithmetic : struct, IArithmetic<T>
{
    // 数値の「組」の型
    public class _1 : Dimension<Fixed1<T>.Array, Fixed1<T>> { }
    public class _2 : Dimension<Fixed2<T>.Array, Fixed2<T>> { }
    public class _3 : Dimension<Fixed3<T>.Array, Fixed3<T>> { }
    public class _4 : Dimension<Fixed4<T>.Array, Fixed4<T>> { }

    public class Dimension<TArray, TArrayAccessor>
        where TArray : struct, IFixedArray<T>
        where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
    {
        // 距離計算の方法
        public class Euclidean : Metric<EuclideanMetric<T, TArithmetic, TArray, TArrayAccessor>> { }
        public class Manhattan : Metric<ManhattanMetric<T, TArithmetic, TArray, TArrayAccessor>> { }
        public class Chebyshev : Metric<ChebyshevMetric<T, TArithmetic, TArray, TArrayAccessor>> { }

        public class Metric<TMetric>
            where TMetric : struct, IMetric<T, TArray>
        {
            public static TArray Nearest(TArray origin, TArray a, TArray b)
            {
                var metric = default(TMetric);

                var da = metric.DistanceSquared(origin, a);
                var db = metric.DistanceSquared(origin, b);

                return da.CompareTo(db) <= 0 ? a : b;
            }

            // その他、距離空間に対するアルゴリズムをこの中に書く
        }
    }
}

class Program
{
    static void Main()
    {
        // 使う側に関してはだいぶ短く書けた
        var n = FloatPoint._2.Euclidean.Nearest(
            (0, 0), (1, 2), (3, 4));

        Console.WriteLine((n.Item1, n.Item2));
    }
}
```

一応、使う側のコードはだいぶ短くなり、許容範囲になったかなと思います。
ただ、これはこれで、以下のような問題があって、妥協的です。

- 使いたい型、固定長配列の次元、距離計算の方法が増えるたびに、この派生クラスも追加しないといけない
- クラスの中に入っているので、他のアセンブリで型を追加できない

根本解決できるようなプログラミング言語を求めるなら、
ジェネリック型引数の推論をもっと頑張ってもらうとか、`float`に対する`FloatArithmetic`みたいなもののペアリングとかの仕様が必要になります。

型の推論は、かなり頑張っている言語もあって、そういう言語ではもうちょっと手短にコードを書けるんですが、
その代わりにコンパイル時間が指数的に跳ね上がったり、
コンパイル エラーが出たときにエラー メッセージが読めた代物じゃなくなったりという弊害があったりします…

`float`に対する`FloatArithmetic`、
`TArray`に対する`TArrayAccessor`みたいなものは、ShapeとかType Classとか呼ばれたりするんですが、
こいつは[将来的に C# に入りそうな雰囲気](../../../2017/2/pickuproslyn0223/index.md)があります。

これが来てくれればだいぶこの手の作業は楽になるんですが。
現状は明確なマイルストーンが切られておらず、「7.X はおろか、8.X でも無理」という扱いです。
最短で、9.0 とかで入ると仮定しても2年以上は先でしょうか…
なので待ってられないので、しょうがなくこんなコードを書くことに…
