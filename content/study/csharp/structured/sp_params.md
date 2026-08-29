---
title: "可変長引数"
source_url: "https://ufcpp.net/study/csharp/structured/sp_params/"
content_type: "Article"
published_at: "2015-05-06T14:08:53"
updated_at: "2024-08-16T00:00:00"
tags: []
umbraco_id: 1237
parent_id: 1217
sort_order: 9
aliases:
  - "/study/csharp/sp_params.html"
---

# 可変長引数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# では <em>
        <code>params</code>
      </em> キーワードを用いることでメソッドの引数の数を可変にすることが出来ます。


##### <a id="sec-generated-title-2"></a>ポイント

* 定義側の例：<code>int Sum(params int[] args) { ... }</code>

* 利用側の例：<code>Sum(1, 2, 3, 4, 5);</code>… これで、<code>Sum(new int[] { 1, 2, 3, 4, 5 });</code>と同じ意味。



## <a id="sec-generated-title-3"></a> <a id="params"></a>params キーワード

例えば、可変個の整数のうち最大の整数を求めるメソッドを作りたいとします。
可変長引数を使わずにメソッドを実装すると以下のようになるでしょう。

```csharp {title="最大値を求めるメソッド"}
using System;

class ParamsTest
{
  static void Main()
  {
    int a = 314, b = 159, c = 265, d = 358, e  = 979;
    // ↑こいつらの最大値を探したいとき、

    int[] tmp = new int[]{a, b, c, d, e};
    // ↑こんな風に一度配列に格納してから

    int max = Max(tmp);
    // ↑Max メソッドを呼び出す必要がある。

    Console.Write("{0}\n", max);
  }

  static int Max(int[] a)
  {
    int max = a[0];
    for(int i=1; i<a.Length; ++i)
    {
      if(max < a[i])
        max = a[i];
    }
    return max;
  }
}
```


この方法では、1度値を配列に格納してからメソッドを呼び出すという操作が必要になります。
このメソッドを呼び出すたびに1時的に配列を作成して、
値を格納してという作業を行うのは面倒です。
そこで、この作業を自動化しようというのが C# の可変長引数の考え方です。

C# では <code>params</code> というキーワードを使って可変個の引数を取るメソッドを定義することが出来ます。
例えば、上の例を <code>params</code> キーワードを使って書き直すと以下のようになります。

```csharp {title="最大値を求めるメソッド(可変長引数版)" highlight-ranges="10:19-10:32,16:18-16:24"}
using System;

class ParamsTest
{
  static void Main()
  {
    int a = 314, b = 159, c = 265, d = 358, e  = 979;
    // ↑こいつらの最大値を探したいとき、

    int max = Max(a, b, c, d, e);
    // ↑こうすると、自動的に配列を作って値を格納してくれる。

    Console.Write("{0}\n", max);
  }

  static int Max(params int[] a)
  {
    int max = a[0];
    for(int i=1; i<a.Length; ++i)
    {
      if(max < a[i])
        max = a[i];
    }
    return max;
  }
}
```


メソッド定義側の変更点は引数 <code>int[] a</code> の前に <code>params</code> キーワードが付いただけです。
呼び出し側では、手動で配列を用意して値を格納しなくても、
可変個の引数を与えてメソッドを呼び出すことが出来ます。


##### <a id="sec-generated-title-4"></a>サンプル

今まで何気なく <code>Console.Write("(x, y) = ({0}, {1})\n", x, y)</code> というような書き方をしていましたが、この Console.Write メソッドは可変長引数の機構を使っています。

ここでは、params の例として、
かなり簡略化したものですが、Console.Write もどきを作ってみます。

```csharp {title="Console.Write もどき"}
using System;

class TestParams
{
  static void Main(string[] args)
  {
    double x = 3.14;
    int    n = 99;
    string s = "test string";
    bool   b = true;

    Write("x = {0}, n = {1}, s = {2}, b = {3}\n", x, n, s, b);
  }

  /// <summary>
  /// Console.Write もどき。
  /// {0:d5} のような書式指定は出来ません。
  /// </summary>
  /// <param name="format">書式指定文字列</param>
  /// <param name="args">format を使用して書き込むオブジェクトの配列</param>
  static void Write(string format, params object[] args)
  {
    for(int i=0; i<args.Length; ++i)
    {
      format = format.Replace("{" + i.ToString() + "}", args[i].ToString());
    }
    Console.Write(format);
  }
}
```


```console {title="Console.Write もどき"}
x = 3.14, n = 99, s = test string, b = True
```

## <a id="sec-generated-title-5"></a> <a id="params-collections">params コレクション</a>

<h5 class="version version13">Ver. 13</h5>

<!-- 昔この id のセクションがあったのでアンカーだけは残す -->
<a id="params-IEnumerable"></a>

C# 13 で、配列以外にも `params` にできる型が増えました。
[コレクション式](../datatype/collection-expression.md)で使える型であれば何でも `params` にできます。
例えば、以下のコードの `M1`～`M4` のようなコードを書けます。

```csharp {title="任意のコレクションに対して params を付ける例"}
static void M1(params List<int> x) { }
static void M2(params IEnumerable<int> x) { }
static void M3(params Span<int> x) { }
static void M4(params ReadOnlySpan<int> x) { }

M1(1, 2);
M2(1, 2);
M3(1, 2);
M4(1, 2);
```

俗称として、このような機能を「`params` コレクション」と言います。

昔から `IEnumerable<T>` を使いたいという要望は多くありました。
また、[C# 7.2](../cheatsheet/ap_ver7_2.md#span-safety) 以降では [`Span<T>` や `ReadOnlySpan<T>`](../resource/span.md) を使いたいという要望も出てきました。
どちらも、「具体的に何の型のインスタンスを作って渡すのがいいか」を決めかねていたり、
オーバーロード解決をどうするかという課題があって、今の今まで実装されてきませんでした。
ただ、これらの課題はコレクション式でも全く同じものを抱えます。
つまるところ、コレクション式(当然課題を解決済み)が [C# 12](../cheatsheet/ap_ver12.md#collection-expression) で入ったのであれば、「コレクション式と同じ解決方法をとる」だけで `params` コレクションを実装できます。

そうなると今度は、「コレクション式だけもう十分なのでは？」という話になります。
なんせ、コレクション式のおかげで、`params` がなくても `[]` のたった2文字の追加だけでほぼ同様のことができます。

```csharp {title="params の価値とは…"}
static void A(params ReadOnlySpan<int> x) { }
static void B(ReadOnlySpan<int> x) { }

// params
A(1, 2, 3);

// params がなくても、[] を足すだけ。
// params の価値とは…
B([1, 2, 3]);
```

なので、「パフォーマンス的に明らかに有利な `params ReadOnlySpan<T>` 以外は要らないのではないか」という話も出ました。
実際、需要があるのはこれと、あとはせいぜい `params Span<T>` くらいな可能性があります。
あくまで、「コレクション式が先に実装されている以上、あえて `ReadOnlySpan<T>` だけに制限する理由がない」という感じです。

既存の `params T[]` なメソッドがあったとして、
このメソッドを `params ReadOnlySpan<T>` に置き換えれば、
メソッドを呼んでいる側のコードは書き換えることなく、パフォーマンスが改善します。

例えば元コードとして以下のようなものがあったとします。

```csharp {title="params T[] な元コード"}
// 初期状態。
static void A(params int[] x) { }

// これはコンパイル結果的には
// A(new int[] { 1, 2, 3 });
// になる。
// この new int[3] がそこそこ重たい。
A(1, 2, 3);
```

これが、以下のように、メソッド定義側だけの書き換えで、利用側はノータッチでパフォーマンス改善が見込めます。

```csharp {title="params ReadOnlySpan に書き換え"}
// メソッド定義側だけ ReadOnlySpan に変更。
static void A(params ReadOnlySpan<int> x) { }

// 呼び出し側はノータッチ。
// (C# 13 で再コンパイルだけ必要。)
// 何もせず、 new int[3] のアロケーションが消える。
A(1, 2, 3);
```

利用個所が非常に多い場合、
「コレクション式があるから `[]` の2文字を足して回るだけ」というのもそんなに簡単な話ではないので、
`params ReadOnlySpan<T>` にはそれなりの需要が出てきます。

実際、 .NET 9 では、`string.Join` や `Task.WhenAll` などのメソッドに
`params ReadOnlySpan<T>` なオーバーロードが増えています。

```csharp {title="params ReadOnlySpan オーバーロードが増えている例"}
// .NET 8 以前なら Join(string, string[])
// .NET 9 以降なら Join(string, ReadOnlySpan<string>)
var joiend = string.Join(",", "a", "b", "c");
```

ちなみに、この理屈でのパフォーマンス改善のためには、
コンパイラーを C# 13 にアップグレードした後、1度は再コンパイルが必要です。
再コンパイルしないままだと(以前コンパイルした dll のまま .NET 9 環境に持って行って動かしても)、`params T[]` の方を参照したままになります。

また、こういう「以前コンパイルした dll をそのまま使う」という利用形態がある以上、
`params T[]` なオーバーロードを消すことは破壊的変更になるためためらわれます。
メソッド作者と利用者が同じなら `params T[]` を単に `params ReadOnlySpan<T>` に書き換えてもいいですが、
誰が利用するかわからないメソッドの場合には実質的には `params ReadOnlySpan<T>` オーバーロードの追加(`params T[]` も残す)しかできません。

幸い、[コレクション式の時点でこの辺りは考慮していて](../datatype/collection-expression.md#priority)、`params` でも同様に配列よりも `ReadOnlySpan<T>` (パフォーマンス的に有利)の方が優先度が高い仕様になっています。

```csharp {title="ReadOnlySpan が優先"}
// ReadOnlySpan の方が呼ばれる。
A.M(1, 2, 3);

class A
{
    // int[] と ReadOnlySpan<int> の両方ある。
    public static void M(params int[] x) { }
    public static void M(params ReadOnlySpan<int> x) { }
}
```

こういった背景から、基本的に、コレクション式と `params` コレクションでは、どちらからも生成されるコードはほぼ同じになります。

```csharp {title="コレクション式と params コレクション"}
static void M1(params int[] x) { }

// どちらで呼んでも new int[] { 1 } 生成。
M1(1, 2);
M1([1, 2]);

static void M2(params Span<int> x) { }

// どちらで呼んでも InlineArray に展開。
M2(1, 2);
M2([1, 2]);

static void M3(params ReadOnlySpan<int> x) { }

// どちらで呼んでも静的データ最適化が掛かる。
M3(1, 2);
M3([1, 2]);
```

#### <a id="sec-generated-title-6"></a> <a id="diff-from-collection-expr">余談:  コレクション式との差</a>

ただ、実装都合でどうしても「全く同じ」にはできないこともあるそうで、ちょっとだけ差があります。
例えば以下のようなコードの場合、`[]` の有無で呼ばれるオーバーロード解決ルールが変わるそうです。

```csharp {title="コレクション式と params コレクション利用時で結果がちょっと変わる珍しい例" error-ranges="1:3-1:4" error-diagnostics="CS0121@1:3-1:4"}
A.M([1, 2, 3]); // こちらは解決できなくてエラーに。
A.M(1, 2, 3); // こちらは int[] 側に解決。

class A
{
    public static void M(params int[] _) { }
    public static void M(params long[] _) { }
}
```

#### <a id="sec-generated-title-7"></a> <a id="params-ref-struct">余談:  params ref 構造体</a>

ref 構造体 (`Span<T>` や `ReadOnlySpan<T>` など)に `params` を付けた場合、
暗黙的に [`scoped`](../resource/refstruct.md#scoped-modifier) 扱い(`scoped` 修飾子を付けた場合と同じルールで解析)になるそうです。

```csharp {title="params を付けた場合、暗黙的に scoped" error-ranges="8:71-8:72,12:71-12:72" error-diagnostics="CS8352@8:71-8:72,CS8352@12:71-12:72"}
class A
{
    // 普通の ReadOnlySpan 引数は、戻り値に素通し可能。
    public static ReadOnlySpan<int> M1(ReadOnlySpan<int> x) => x;

    // scoped を付けると外に漏らせなくなる。
    // 戻り値に返そうとするとコンパイル エラー。
    public static ReadOnlySpan<int> M2(scoped ReadOnlySpan<int> x) => x;

    // params を付けると自動的に scoped 扱い。
    // 戻り値に返そうとするとコンパイル エラー。
    public static ReadOnlySpan<int> M3(params ReadOnlySpan<int> x) => x;
}
```

`scoped` が付いていると、メソッド定義側での自由が減る代わりに、呼び出し側の自由が増えます。
`params` の用途的に、定義側が `scoped` 困ることもなく、呼び出し側は `scoped` でないと困ることがありそうということでこういう仕様になりました。


## <a id="sec-generated-title-8"></a> <a id="no-param"></a>余談: 可変長引数を引数なしで呼ぶ

可変長引数にしたメソッドは、引数なしで呼ぶこともできます。
この場合、呼び出された側のメソッドには、空配列(長さ0の配列)が渡ります。

```csharp {title="可変長引数メソッドを引数なしで呼ぶと空配列が渡る"}
using System;

class Program
{
    static void Main(string[] args)
    {
        var x = Sum();
        Console.WriteLine(x); // 0
    }

    static int Sum(params int[] source)
    {
        // 引数なしで呼ばれた場合、source には空配列が入る
        // source が null にはならない
        var sum = 0;
        foreach (var x in source) sum += x;
        return sum;
    }
}
```

ちなみに、空配列の作られ方ですが、
.NET Frameworkのバージョンによって変化します。
.NET Framework 4.6以降/.NET Coreでは、`Array.Empty`という空配列を作るためのメソッドが用意されています。
これがある(つまり、.NET Framework 4.6以降か、.NET Coreがターゲットになっている)場合、このメソッドが呼ばれます。
なければ、`new T[0]`で空配列を作ります。

つまり、上記の`var x = Sum()`は、.NET Framework 4.5以前であれば以下のように解釈されます。

```csharp {title=".NET 4.5以前での空配列の作り方"}
// .NET Framework 4.5 以前はこういう扱い
var x = Sum(new int[0]);
```

一方、.NET Framework 4.6以降であれば以下のように解釈されます。

```csharp {title=".NET 4.6以降での空配列の作り方"}
// .NET Framework 4.6 以降はこういう扱い
var x = Sum(Array.Empty<int>());
```

これらの差・変更の理由は単純で、`Array.Empty`を使う方がパフォーマンスが良いです。
`new int[0]`だと、メソッド呼び出しのために新しい配列のインスタンスが作られますが、
`Array.Empty`は最初に作った1つのインスタンスをキャッシュしてずっと使いまわします。

一応、昔からあるプログラムの挙動が変わる可能性がある破壊的変更なので注意してください。
狙ってやらないと起こせないような珍しい問題ですが、
例えば以下のようなコードの挙動は、.NET Framework のバージョンによって変化します。

```csharp {title="破壊的変更になる例"}
using System;

class Program
{
    static void Main(string[] args)
    {
        var x = IsCached();
        Console.WriteLine(x);
        var y = IsCached();
        Console.WriteLine(y); // ターゲットによって結果が変わる
    }

    static int[] prev;

    static bool IsCached(params int[] source)
    {
        // .NET 4.5 以前だと、毎回違う配列がnewされて渡ってくる
        // .NET 4.6 以降だと、毎回同じインスタンスが使いまわされる
        if (prev == source) return true;

        prev = source;
        return false;
    }
}
```

## <a id="sec-generated-title-9"></a> <a id="arglist"></a>__arglist

ちなみに、仕様書にない隠し機能ではあるんですが、
マイクロソフト製や、Mono 製の C# コンパイラーには、可変長引数のための構文として、もう1つ、__arglist というものがあります。
詳しくは「[型付き参照](../interop/sp_makeref.md)」で説明します。

この隠し機能は主に C# 以外のプログラミング言語との相互運用にあるためのものです。
実際のところ、あまり性能はよくない(params を使ったものと比べると1桁は余裕で遅い)ので、わざわざ使うものではないでしょう。
