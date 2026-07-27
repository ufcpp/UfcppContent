---
title: "[サンプル] ジェネリックな複素数型"
source_url: "https://ufcpp.net/study/csharp/sample/sm_genericop/"
content_type: "Article"
published_at: "2008-03-14T00:00:00"
updated_at: "2015-05-06T14:13:20"
tags: []
umbraco_id: 1366
parent_id: 1359
sort_order: 6
aliases:
  - "/study/csharp/sm_genericop.html"
---

# \[サンプル\] ジェネリックな複素数型

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/GenericOperator.zip)



事の発端は↓から。

* [あらあらあら - 東方算程譚](http://blogs.wankuma.com/episteme/archive/2009/02/04/167390.aspx)

* [コンプレックスと戦う - Garbage Collection](http://blogs.wankuma.com/izmktr/archive/2009/02/05/167489.aspx)

* [複素数型を作る。 - HIRASE CONNECTION WK](http://blogs.wankuma.com/hirase/archive/2009/02/05/167505.aspx)

* [Expression trees と .NET 風メタプログラミング - NyaRuRuの日記](http://d.hatena.ne.jp/NyaRuRu/20090205/p1)


ちょうど「[ジェネリック](../oop/sp2_generics.md)」に「[C++ や Java の template/generics との違い](../oop/sp2_generics.md#compare)」を足した時にこういう話題を見かけたので食いついてみた。

ちょっと考えてみた結果、以下のようなクラスを作ると便利なんじゃないかという考えに至る。

```csharp
using System;
using System.Linq.Expressions;

namespace GenericOperator
{
    using Binary = Func<ParameterExpression, ParameterExpression, BinaryExpression>;
    using Unary = Func<ParameterExpression, UnaryExpression>;

    /// <summary>
    /// 動的にジェネリック型 T の加減乗除関数を作る。
    /// </summary>
    /// <typeparam name="T">対象となる型。</typeparam>
    public static class Operator<T>
    {
        static readonly ParameterExpression x = Expression.Parameter(typeof(T), "x");
        static readonly ParameterExpression y = Expression.Parameter(typeof(T), "y");

        public static readonly Func<T, T, T> Add = Lambda(Expression.Add);
        public static readonly Func<T, T, T> Subtract = Lambda(Expression.Subtract);
        public static readonly Func<T, T, T> Multiply = Lambda(Expression.Multiply);
        public static readonly Func<T, T, T> Divide = Lambda(Expression.Divide);
        public static readonly Func<T, T> Plus = Lambda(Expression.UnaryPlus);
        public static readonly Func<T, T> Negate = Lambda(Expression.Negate);

        public static Func<T, T, T> Lambda(Binary op)
        {
            return Expression.Lambda<Func<T, T, T>>(op(x, y), x, y).Compile();
        }

        public static Func<T, T> Lambda(Unary op)
        {
            return Expression.Lambda<Func<T, T>>(op(x), x).Compile();
        }
    }
}
```



## <a id="sec-generated-title-2"></a> <a id="template"></a>C++ template と C# genrics の違い

「[C++ や Java の template/generics との違い](../oop/sp2_generics.md#compare)」に書いたように、
C++ の template と C# のジェネリクスには色々違いがありますが、
このページで問題にするのは以下の点。

* C++ の template では、 メソッド呼び出しは「[ダックタイピング](../appendix/ap_term.md#ducktype)」で行う。

* C# のジェネリクスでは、 メソッド呼び出しはインターフェースを使った型制約で行う。


C# の generics において、メソッド呼び出しにインターフェースを使うということは、
以下のような制約が生じます。

* 型引数がどういうインターフェースを実装しているべきか、コンパイル時に分かっている必要がある。

* 静的メソッドを呼べない。 ということは、operator を使えない。


特に、operator を使えないというのが結構問題で、複素数クラスのジェネリック版を作るのにすら苦労することになります。

例えば、C++ ならば以下のような書き方ができます。

```cpp
template<typename T>
Complex<T> operator +(Complex<T> x, Complex<T> y)
{
  //↓ T の型がなんであれ、+ 演算子を持っているものならコンパイル可能。
  T re = x.re + y.re;
  T im = x.im + y.im;
  return Complex<T>(re, im);
}
```


ところが、同じことを C# でやろうとすると、コンパイルエラーになります。

```csharp
public static Complex<T> operator +(Complex<T> x, Complex<T> y)
{
  //↓ エラー： 演算子 '+' を 'T' と 'T' 型のオペランドに適用することはできません。
  T re = x.re + y.re;
  T im = x.im + y.im;
  return new Complex<T>(re, im);
}
```


（
C# のジェネリクスは、
異なる型引数で（例えば List&lt;int&gt; と List&lt;string&gt; で）可能な限り生成されるコードを共通化する方針を取ったためこういう仕様になっています。
C++ の方針の方が自由は効きますが、こちらはこちらで別の問題も抱えているのでどちらがいいとも言えないです。
）


## <a id="sec-generated-title-3"></a> <a id="howto"></a>C# ではどうすればいいか

C# ジェネリクスでできないことも、動的コード生成などを使うことで何とかできることが多いです。

1. <h5 class="version version4">Ver. 4.0</h5>C# 4.0 で導入される dynamic キーワードを使う。

2. 「[リフレクション](../dynamic/sp_reflection.md#reflection)」を使う。

3. 「[式木](../functional/sp3_lambda.md#exp_tree)」を使う。


1つ目の dynamic を使えば、まさに「[ダックタイピング](../appendix/ap_term.md#ducktype)」が可能です。
でも、「キャスト不要」とか「実行効率がいい」というようなジェネリクスの利点とは相反するものなので、
あまりジェネリクスと組み合わせて使いたいものではないです。

2つ目のリフレクションを使えば、例えば以下のようなことができます。

```csharp
using System;
using System.IO;
using System.Text;

class Test
{
    static void Main()
    {
        var mem = new MemoryStream();
        var writer = new BinaryWriter(mem);

        Serializer.Serialize(writer, 10);
        Serializer.Serialize(writer, 10.0);
        Serializer.Serialize(writer, (byte)10);
        Serializer.Serialize(writer, "10");

        mem.Seek(0, SeekOrigin.Begin);

        var reader = new BinaryReader(mem);

        Console.WriteLine(Serializer.Deserialize<int>(reader));
        Console.WriteLine(Serializer.Deserialize<double>(reader));
        Console.WriteLine(Serializer.Deserialize<byte>(reader));
        Console.WriteLine(Serializer.Deserialize<string>(reader));

    }
}

static class Serializer
{
    public static void Serialize<T>(BinaryWriter writer, T value)
    {
        // string だけ特殊処理。
        if (typeof(T) == typeof(string))
        {
            var s = value as string;
            var b = Encoding.UTF8.GetBytes(s);
            writer.Write(b.Length);
            writer.Write(b);
            return;
        }

        // BinaryWriter.Write のオーバーロードがあるものはこれを呼び出す。
        var write = typeof(BinaryWriter).GetMethod("Write", new[] { typeof(T) });
        System.Diagnostics.Debug.Assert(write != null);
        write.Invoke(writer, new object[] { value });
    }

    public static T Deserialize<T>(BinaryReader reader)
    {
        // string だけ特殊処理。
        if (typeof(T) == typeof(string))
        {
            var count = reader.ReadInt32();
            var b = reader.ReadBytes(count);
            return (T)(object)Encoding.UTF8.GetString(b);
        }

        // BinaryReader.Read*** があるものはこれを呼び出す。
        var read = typeof(BinaryReader).GetMethod("Read" + typeof(T).Name, new Type[0]);
        System.Diagnostics.Debug.Assert(read != null);
        return (T)read.Invoke(reader, new object[0]);
    }
}
```


これも、特殊な用途で使うものであって、
キャストが必要だし、実行効率もあまりよくないです。
（この場合、ジェネリクスを使う理由は型推論の利用であって、実行効率ではない。）

それに、この方法だと、int などの組み込み数値型に対する四則演算は呼び出せなかったりします。
（C# では組み込み数値型の演算は特別扱いされてて、リフレクションでは呼び出せない。）

で、今回問題になっている operator 呼び出しには、3つ目の式木を使うのがいいと思います。


## <a id="sec-generated-title-4"></a> <a id="expression"></a>式木で動的コード生成

ということで、前節で出てきた3つ目、式木を使った動的コード生成について説明します。

.NET Framework 3.0 で導入された式木を使うと、以下のようなことができます。

```csharp
using System;
using System.Linq.Expressions;

class Test
{
    static void Main()
    {
        var add = CreateAdder<int>();

        Console.WriteLine(add(10, 20)); // 10 + 20 で 30 が表示される。
    }

    /// <summary>
    /// (T x, T y) => x + y; に相当する匿名デリゲートを生成する。
    /// </summary>
    /// <typeparam name="T">オペランドの型。</typeparam>
    /// <returns>加算デリゲート。</returns>
    static Func<T, T, T> CreateAdder<T>()
    {
        var x = Expression.Parameter(typeof(T), "x");
        var y = Expression.Parameter(typeof(T), "y");

        var expression = Expression.Lambda<Func<T, T, T>>(
            Expression.Add(x, y),
            x, y);

        return expression.Compile();
    }
}
```


汎用化するために、このページの冒頭で載せたようなクラスを定義。
要点だけ抜粋すると、以下のような感じ。

```csharp
using Binary = Func<ParameterExpression, ParameterExpression, BinaryExpression>;

public static class Operator<T>
{
    public static readonly Func<T, T, T> Add = Lambda(Expression.Add);

    public static Func<T, T, T> Lambda(Binary op)
    {
        return Expression.Lambda<Func<T, T, T>>(
            op(x, y),
            x, y).Compile();
    }
}
```


これを使えば、ジェネリックな複素数の加算を以下のような感じで作れます。

```csharp
static T Add(T x, T y) { return Operator<T>.Add(x, y); }

public static Complex<T> operator +(Complex<T> x, Complex<T> y)
{
    T re = Add(x.re, y.re);
    T im = Add(x.im, y.im);
    return new Complex<T>(re, im);
}
```


<code>x.re + y.re</code> と書けなくて不格好なのと、
デリゲート呼び出しが1段挟まって多少まだ効率が悪いので、
C++ の template には少々及びませんが、
これでジェネリックな複素数クラスを作るという目的は果たせそうです。

完成品は以下の通り。

* 
[Operator&lt;T&gt; クラス](../../../../assets/media/ufcpp2000/csharp/source/Operator.cs)


* 
[Complex&lt;T&gt; クラス](../../../../assets/media/ufcpp2000/csharp/source/Complex.cs)


* 
[下記サンプルで使っている有理数クラス](../../../../assets/media/ufcpp2000/csharp/source/Rational.cs)


* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/GenericOperator.zip)



利用例としては以下のような感じ。

```csharp
static void Main(string[] args)
{
    ShowFourOperations(
        4 .I(5),
        2 .I(4));

    ShowFourOperations(
        4.0 .I(5.0),
        2.0 .I(4.0));

    ShowFourOperations(
        1.Over(2) .I(1.Over(3)),
        2.Over(3) .I(3.Over(4)));
}

static void ShowFourOperations<T>(Complex<T> x, Complex<T> y)
    where T: IComparable<T>
{
    Console.WriteLine(typeof(T).Name);
    Console.WriteLine("({0}) + ({1}) = {2}", x, y, x + y);
    Console.WriteLine("({0}) - ({1}) = {2}", x, y, x - y);
    Console.WriteLine("({0}) * ({1}) = {2}", x, y, x * y);
    Console.WriteLine("({0}) / ({1}) = {2}", x, y, x / y);
}
```


```console
Int32
(4 + i5) + (2 + i4) = 6 + i9
(4 + i5) - (2 + i4) = 2 + i1
(4 + i5) * (2 + i4) = -12 + i26
(4 + i5) / (2 + i4) = 0 + i0
Double
(4 + i5) + (2 + i4) = 6 + i9
(4 + i5) - (2 + i4) = 2 + i1
(4 + i5) * (2 + i4) = -12 + i26
(4 + i5) / (2 + i4) = 1.75 - i0.375
Rational
((1/2) + i(1/3)) + ((2/3) + i(3/4)) = (7/6) + i(13/12)
((1/2) + i(1/3)) - ((2/3) + i(3/4)) = (-1/6) - i(5/12)
((1/2) + i(1/3)) * ((2/3) + i(3/4)) = (1/12) + i(43/72)
((1/2) + i(1/3)) / ((2/3) + i(3/4)) = (7/12) - i(11/72)
```
