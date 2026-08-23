---
title: "拡張メソッド"
source_url: "https://ufcpp.net/study/csharp/functional/sp3_extension/"
content_type: "Article"
published_at: "2008-08-15T00:00:00"
updated_at: "2018-03-25T00:00:00"
tags:
  - "Ver. 3.0"
umbraco_id: 1284
parent_id: 1275
sort_order: 10
aliases:
  - "/study/csharp/sp3_extension.html"
---

# 拡張メソッド

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

拡張メソッドは、静的メソッドをインスタンスメソッドと同じ形式で呼び出せるようにできるものです。
すなわち、
今までなら、

```csharp {title="静的メソッド"}
int x = int.Parse("1");      
```


と書いていたものを、

```csharp {title="拡張メソッドの定義"}
static class Extensions
{
    public static int Parse(this string str)
    {
        return int.Parse(str);
    }
}
```


というような静的メソッドを用意することで、
以下のような構文で呼び出せるようになります。

```csharp {title="拡張メソッドの利用"}
int x = "1".Parse();
```



##### <a id="sec-generated-title-2"></a>ポイント

* 拡張メソッド： 静的メソッドをインスタンスメソッドと同じ書式で呼び出せるようにすることで、 あたかもクラスに新しいメソッドを追加したかのように見せかける仕組みです。

* 単に、静的メソッドを後置き記法で呼び出せるようになっただけとも考えることができます。

* 定義側： 第1引数の前に this を付けます。

* 利用側： インスタンスメソッドと同じ書き方をします。



## <a id="sec-generated-title-3"></a> <a id="extension"></a>拡張メソッド

C# 2.0 までの常識で言うと、
既存のクラスの機能拡張（＝メソッドの追加）をしたければ、
そのクラスを継承したりなどして、新しいクラスを作るしかありませんでした。

これに対して、C# 3.0 では、後述する方法で、
既存のクラスにメソッドを追加できます。
（正確には、インスタンスメソッドの“ようなもの”。インスタンスメソッドと同じ構文で呼べるだけ。）
このような、後から追加するメソッドのことを<strong id="exmethod" class="keyword">拡張メソッド</strong>（extension method）と呼びます。

まず、拡張メソッドの定義の仕方ですが、
以下のように、
<em>「[静的クラス](../oop/oo_static.md#stclass)」中に、
      第一引数に this キーワードを修飾子として付けた static メソッドを書きます</em>。

```csharp {title="拡張メソッドの定義" highlight-text="this"}
static class StringExtensions
{
  public static string ToggleCase(this string s)
  中身省略
}
```


このようにして定義したメソッドは、
通常通り、静的メソッドとして呼び出すこともできますが、
あたかも string 型のインスタンスメソッドであるかのように呼び出せるようになります。

```csharp {title="拡張メソッドの呼び出し" highlight-text="s.ToggleCase()"}
string s = "This Is a Test String.";
string s1 = StringExtensions.ToggleCase(s); // 通常の呼び出し方。
string s1 = s.ToggleCase();                 // 拡張メソッド呼び出し。
```


上述のような拡張メソッドの利用例のソース全てを以下に示します。

```csharp {title="拡張メソッドの例"}
using System;

namespace ConsoleApplication1
{
  static class StringExtensions
  {
    /// <summary>
    /// 文字列の大文字と小文字を入れ替える。
    /// </summary>
    /// <param name="s">変換元</param>
    /// <returns>変換結果</returns>
    public static string ToggleCase(this string s)
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach(char c in s)
      {
        if(char.IsUpper(c))
          sb.Append(char.ToLower(c));
        else if(char.IsLower(c))
          sb.Append(char.ToUpper(c));
        else
          sb.Append(c);
      }
      return sb.ToString();
    }
  }

  class ExtensionMethodTest
  {
    static void Main(string[] args)
    {
      string s = "This Is a Test String.";
      Console.Write(s.ToggleCase());
    }
  }
}
```


```console {title="拡張メソッドの例"}
tHIS iS A tEST sTRING.
```



## <a id="sec-generated-title-4"></a> <a id="using"></a>using ディレクティブによる拡張メソッドのインポート

通常、静的メソッドは「クラス名.メソッド名」という記法で呼び出します。
ところが、拡張メソッドでは、「クラス名」の部分をさぼって書けるようになっています。

じゃあ、どうやって「どのメソッドが呼ばれるか」を決定しているかというと、
<em>「[using ディレクティブ](../structured/sp_namespace.md#using)」で指定した名前空間中のにある拡張メソッドが参照される</em>ようになっています。

そのため、同じ名前空間内に2つ以上同名の拡張メソッドを定義してはいけません。

```csharp {title="同名の拡張メソッドがあるせいでエラーに"}
namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            Console.Write(1.Square()); // エラーになる
        }
    }

    static class Extensions1
    {
        public static int Square(this int x)
        {
            return x * x;
        }
    }

    static class Extensions2
    {
        public static int Square(this int x) // エラーの原因
        {
            return x * x;
        }
    }
}
```


同名の拡張メソッドが定義されている名前空間を同時に using するのもご法度です。

```csharp {title="using でどのメソッドが呼ばれるかが決まる"}
using System;

namespace ConsoleApplication1
{
    using NamespaceA;
    //using NamespaceB;
    // ↑
    // ここのコメントを外してもやっぱりエラー。
    // using NamespaceA をコメントアウトして、
    // 代りに using NamespaceB するなら OK（表示結果が変わる）。

    class Program   
    {
        static void Main()
        {
            1.WriteToConsole();
            // ↑
            // NamespaceA.Extensions.WriteToConsole が呼ばれる
        }
    }
}

namespace NamespaceA
{
    static class Extensions
    {
        public static void WriteToConsole(this int x)
        {
            Console.Write("A {0}", x);
        }
    }
}

namespace NamespaceB
{
    static class Extensions
    {
        public static void WriteToConsole(this int x)
        {
            Console.Write("B {0}", x);
        }
    }
}
```



## <a id="sec-generated-title-5"></a> <a id="priority"></a>優先順位

拡張メソッドのせいで、
同じ名前のメソッドがいくつか同時に定義されてしまう可能性があります。
その場合、どのメソッドが呼ばれるか優先順位が決まっています。

### <a id="sec-generated-title-6"></a> <a id="instance-over-extension"></a>インスタンス メソッド優先

まず、拡張メソッドよりも通常のインスタンスメソッドの方が優先されます。

```csharp {title="インスタンスメソッド優先"}
using System;

class Program
{
    static void Main()
    {
        Console.Write(1.ToString());
        // ↑
        // Extensions.ToString ではなく、
        // int.ToString が呼ばれる。
    }
}

static class Extensions
{
    public static string ToString(this int x)
    {
        return "dummy data";
    }
}
```

#### <a id="sec-generated-title-7"></a> <a id="overload"></a>オーバーロード解決ルールより、インスタンス メソッド優先が強い

通常、オーバーロードが複数ある場合は一番引数の一致度が高いものが呼ばれます。
例えば、以下のコードの場合は、`object`引数のものより`string`引数のものがまず優先、`string`に合わない場合だけ`object`のものが呼ばれます。

```csharp {title="オーバーロード解決"}
using static System.Console;

class X
{
    public void F(object x) => WriteLine($"object {x}");
    public void F(string x) => WriteLine($"string {x}");
}

class Program
{
    static void Main(string[] args)
    {
        var x = new X();
        x.F("abc"); // string のが呼ばれる
        x.F(10);    // int のオーバーロードがないので object のが呼ばれる
    }
}
```
```console {title="実行結果"}
string abc
object 10
```

ここで、`int`引数の拡張メソッドを足してみましょう。
しかし、拡張メソッドよりもインスタンス メソッドの方が優先的に呼ばれます。
引数の一致度が高くても、拡張メソッドの方は呼ばれません。

```csharp {title="インスタンス メソッドと拡張メソッドの混在"}
using static System.Console;

class X
{
    public void F(object x) => WriteLine($"object {x}");
    public void F(string x) => WriteLine($"string {x}");
}

static class XExtensions
{
    public static void F(this X @this, int x) => WriteLine($"int {x}");

}

class Program
{
    static void Main(string[] args)
    {
        var x = new X();
        x.F("abc"); // string のが呼ばれる
        x.F(10);    // int な拡張が増えたものの、インスタンス メソッド優先で object のが呼ばれる
    }
}
```
```console {title="実行結果"}
string abc
object 10
```


### <a id="sec-generated-title-8"></a> <a id="namespace"></a>名前空間の優先度

名前空間違いで複数の拡張メソッドを定義することもできます。
この場合、優先度付けは名前空間の仕様に準じます:

- [名前空間 > 名前解決の優先度](../structured/sp_namespace.md#priority)

特に、拡張メソッドを拡張メソッドとして呼びたい場合、完全修飾名は使えません。
上記ページの優先度付けが唯一の呼び分け手段になります。
以下のように、使う場所に近いほど優先、直接的なものほど優先で呼べます。
同優先度のものが複数ある場合はコンパイル エラーになります。

```csharp {title="複数の名前空間にある拡張メソッドの呼び分け"}
using static System.Console;
using A;

using Lib = C.Lib;
static class Lib { public static void F(this int x) => WriteLine("global"); }

namespace MyApp
{
    using B;

    static class Lib { public static void F(this int x) => WriteLine("MyApp"); }

    class Program
    {
        static void Main()
        {
            // F 拡張メソッドは5つある
            // この場合 MyApp.Lib.F が使われる
            // 優先度 高 MyApp > B > global = C > A 低
            10.F();

            // ちゃんと呼び分けたければ拡張メソッドとして使うことをあきらめる
            // 完全修飾名を使って、普通の静的メソッドとして呼ぶ
            A.Lib.F(10);
            B.Lib.F(10);
            C.Lib.F(10);
            MyApp.Lib.F(10);
            global::Lib.F(10);
        }
    }
}

namespace A
{
    static class Lib { public static void F(this int x) => WriteLine("A"); }
}
namespace B
{
    static class Lib { public static void F(this int x) => WriteLine("B"); }
}
namespace C
{
    static class Lib { public static void F(this int x) => WriteLine("C"); }
}
```


## <a id="sec-generated-title-9"></a> <a id="interface"></a>インターフェースに拡張メソッドを追加

拡張メソッドでは、1つ、通常のインスタンスメソッドにはできないことができます。
それは、「[インターフェース](../oop/oo_interface.md#interface)」に対して、
インスタンスメソッド風のメソッドを定義できると言うことです。

通常、「[インターフェース](../oop/oo_interface.md#interface)」は、メソッドの外部仕様のみを定義でき、
実装は定義できません。
しかしながら、拡張メソッドを利用することで、
インスタンスメソッド定義っぽいことが実現できます。

```csharp {title="インターフェースに対する拡張メソッド定義"}
using System;
using System.Collections;

static class Extensions
{
  public static IEnumerable<T> Duplicate<T>(this IEnumerable<T> list)
  {
    foreach (var x in list)
    {
      yield return x;
      yield return x;
    }
  }
}

class Program
{
  static void Main(string[] args)
  {
    IEnumerable<int> data = new int[]{ 1, 2, 3 };

    // ↓インターフェースに対してメソッドを追加できる
    data = data.Duplicate();

    foreach (var x in data)
      Console.Write("{0}\n", x);
  }
}
```


C# 3.0 では、IEnumerable インターフェースなどに、
拡張メソッドとして Where や Select などのメソッド（「[標準クエリ演算子](../data/sp3_linq.md#std_query_op)」）が定義されています。


## <a id="sec-generated-title-10"></a> <a id="problem"></a>拡張メソッドの問題点

ちなみに、インスタンス メソッドでも拡張メソッドでもどちらでもいい場合、拡張メソッドの濫用は避けた方がいいでしょう。
拡張メソッドの濫用には不便な点もありますし、
いくつか問題を起こす可能性があります。

##### <a id="sec-generated-title-11"></a>実体はあくまで静的メソッド

拡張メソッドは、
呼び出し側だけ見ると、一見、クラスにメソッドが追加されたように思えますが、
その実態はあくまで静的メソッドです。
それも、元のクラス中ではなく、別の静的クラスの中で定義された静的メソッドです。

元のクラスからみれば当然「外部」なので、
拡張メソッドから private / protected メンバーにアクセスすることはできません。


##### <a id="sec-generated-title-12"></a>定義場所がどこかわからなくなる

クラス本体と別の場所にメソッド定義があるため、
定義された場所を探すのに苦労する可能性があります。

しかも、using 文を使ってインポートするため、
using 文1つでどの静的メソッドが呼ばれるのかが切り替わって、
なおのことどこに定義があるのかわかりにくくなっています。


## <a id="sec-generated-title-13"></a> <a id="significance"></a>拡張メソッドの意義

前節の通り、実を言うと、拡張メソッドは両手ばなしによろこべる機能ではなかったりします。
インスタンス メソッドでの実装が可能ならば素直にクラスのインスタンス メソッドとして定義すべきです。

拡張メソッドは、「クラスを作った人とは全くの別人がメソッドを足せる」という点が最大のメリットです。
このメリットは、特にインターフェイスに対して需要があります。
多くの場合、インターフェイスを作る人と、そのインターフェイスを使った処理を書く人は別です。
通常、この「インターフェイスを使った処理」は静的メソッドになりがちです。
そして、拡張メソッドの真骨頂は「<em>（本来は前置き記法である）静的メソッドを後置き記法で書ける</em>」という部分にあると思っています。

例えば、下図のような、データ列に対するパイプライン処理を考えてみます。

<figure>

[![パイプライン処理](../../../../assets/media/ufcpp2000/csharp/fig/extension01.png)](../../../../assets/media/ufcpp2000/csharp/fig/extension01.png)

<figcaption>パイプライン処理</figcaption>
</figure>


まず、条件付けや値の加工のために以下のような静的メソッドを用意します。

```csharp {title="データ列の選択・加工用のメソッド"}
static class Extensions
{
    public static IEnumerable<int> Where(this IEnumerable<int> array, Func<int, bool> pred)
    {
        foreach (var x in array)
            if (pred(x))
                yield return x;
    }

    public static IEnumerable<int> Select(this IEnumerable<int> array, Func<int, int> filter)
    {
        foreach (var x in array)
            yield return filter(x);
    }
}
```


これを、静的メソッド呼び出しの構文で書くと以下のようになります。

```csharp {title="静的メソッドによるデータ列のパイプライン処理"}
var input = new[] { 8, 9, 10, 11, 12, 13 };

var output =
    Extensions.Select(
        Extensions.Where(
            input,
            x => x > 10),
        x => x * x);
```


やりたいパイプライン処理の順序と、語順が逆になります。
また、「Where とそれに対する条件式 x &gt; 10」や
「Select とそれに対する加工式 x * x」の位置が離れてしまいます。

これに対して、拡張メソッド構文を使うと、以下のようになります。

```csharp {title="拡張メソッドによるデータ列のパイプライン処理"}
var input = new[] { 8, 9, 10, 11, 12, 13 };

var output = input
    .Where(x => x > 10)
    .Select(x => x * x);
```


ただ語順が違うだけなんですが、
こちらの方がやりたいことの意図が即座に伝わります。
すなわち、パイプライン処理（フィルタリング処理）は、
後置きの語順が好ましい処理です。

というように、
語順的に後置きの方がしっくりくる場合に
（というか、むしろその場合のみに）、
静的メソッドを拡張メソッド化することをお勧めします。


## <a id="sec-generated-title-14"></a> <a id="delegate"></a>拡張メソッドのデリゲートへの代入

拡張メソッドは、インスタンスメソッドと同じ構文で静的メソッドを呼べるものなわけですが、
デリゲートへの代入時にも、インスタンスメソッドと同じ構文で書けたりします。
（ただし、少々制約あり。）

すなわち、以下のようなコードは合法です。

```csharp {title="拡張メソッドのデリゲートへの代入"}
using System;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            Func<string> f = "test".Duplicate;
            // ↑
            // 実行結果的には
            // Func<string> f = () => Extensions.Duplicate("test");
            // と同じ。
            // コンパイル結果的には、こんな余計な匿名デリゲートはできないらしい。
            // 直接 f に Extensions.Duplicate("test") が代入されるようなイメージ。
        }
    }

    static class Extensions
    {
        public static string Duplicate(this string x)
        {
            return x + x;
        }
    }
}
```

こういうように、メソッドの引数を何らかの値で束縛して、新しいデリゲートを作ることをカリー化（currying）といいます。
また、上述のようなデリゲートの作り方をカリー化デリゲート（curried delegate）というそうです
（curry は人名に由来する単語らしくて、他に意味はない）。
詳細は「[カリー化デリゲート](miscdelegateinternal.md#curried-delegate)」で説明します。

ただし、カリー化デリゲートが作れるのは参照型の変数のみです。
値型の場合にはエラーになります。

## <a id="sec-generated-title-15"></a> <a id="ref-extensions"></a>参照渡しの拡張メソッド

<h5 class="version version7">Ver. 7.2</h5>

C# 7.2 から、拡張メソッドの第1引数(`this`が付いている引数)を参照渡し([`ref`](../resource/sp_ref.md#sec-byref)もしくは[`in`](../resource/sp_ref.md#in))で渡せるようになりました。
(ただし、構造体に対してのみです。クラスの場合は今まで通り、値渡ししかできません。)

以下のように書けます。`ref`引数の拡張メソッドで構造体の書き換えができたり、コピー除けのために`in`引数が使えます。

```csharp {title="参照渡しの拡張メソッドの例" highlight-ranges="sha256:7f04dc66ff248c81ab7f966ee3b9b61d4a481bf56cc0ffd359206755b79b3dcb;4:34-4:42,14:37-14:44"}
public static class QuaternionExtensions
{
    // 構造体の書き換えを拡張メソッドでやりたい場合に ref 引数が使える
    public static void Conjugate(ref this Quaternion q)
    {
        var norm = q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z;
        q.W = q.W / norm;
        q.X = -q.X / norm;
        q.Y = -q.Y / norm;
        q.Z = -q.Z / norm;
    }

    // コピーを避けたい場合に in 引数が使える
    public static Quaternion Rotate(in this Quaternion p, in Quaternion q)
    {
        var qc = q;
        qc.Conjugate();
        return q * p * qc;
    }
}

public struct Quaternion
{
    public double W;
    public double X;
    public double Y;
    public double Z;
    public Quaternion(double w, double x, double y, double z) => (W, X, Y, Z) = (w, x, y, z);

    public static Quaternion operator *(in Quaternion a, in Quaternion b)
        => new Quaternion(
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
}
```

ちなみに、
古いバージョンの[コンパイラー](https://www.nuget.org/packages/Microsoft.Net.Compilers/)(バージョン2.6)では、
修飾子の順序が`ref this`、`in this`の順でないと受け付けないという挙動でした。
2.7 以降では逆(`this ref`、`this in`)の順でも大丈夫です。

### <a id="sec-generated-title-16"></a> <a id="only-struct"></a>補足: 構造体のみ

すでに触れてはいますが、参照渡しで拡張メソッドを作れるのは[構造体](../resource/rm_struct.md)(値型)だけです。
以下のように、クラスではできません。また、ジェネリックな型の場合、[`struct`制約](../oop/sp2_generics.md#where)が必要です(ただし、それでも`in`引数は不可)。

```csharp {title="参照渡しの拡張メソッドを作れるのは構造体だけ"}
static class Extensions
{
    // 構造体(値型)は OK
    public static void M(ref this int x) { }
    public static void MI(in this int x) { }

    // クラス(参照型)はダメ。コンパイル エラー
    public static void M(ref this string x) { }

    // 制約が付いていないとダメ。コンパイル エラー
    public static void M1<T>(ref this T x) { }

    // ref の場合、struct 制約が付いていれば OK
    public static void M2<T>(ref this T x) where T : struct { }

    // in の場合、struct 制約が付いてもダメ
    public static void M3<T>(in this T x) where T : struct { }
}
```

こういう仕様になっている理由ですが、
まず、クラスについては拡張メソッドの中で参照を書き換えられることを心配してのことだそうです。
通常の[参照引数](../resource/sp_ref.md#sec-byref)の場合は呼ぶ側で`M(ref s)`と言うように`ref`を付ける必要があるので、
`s`が書き換わる可能性があることが呼ぶ側でもわかりやすいです。
一方で、拡張メソッドの場合は`ref`を付けない仕様なので、知らないうちに書き換わる可能性があり、これを禁止したかったわけです。

```csharp {title="クラスの引数を ref this にできない理由"}
// (もしもこれをコンパイル エラーにしなかった場合)
public static void M(ref this string s)
{
    // 拡張メソッドの中で参照を書き換える
    s = null;
}

static void Main()
{
    var s = "abc";
    s.M(); // M の中で s = null される
    Console.WriteLine(s); // null になってる
}
```

`in`引数では`struct`制約付きのジェネリック型も認めていない理由については、
コピー発生を避けることができなくて、`in`引数である意味が全くなくなるからだそうです。
詳しくは「[参照渡し](../resource/sp_ref.md#in-copy)」の項で説明しますが、
`in`引数はパフォーマンス改善を目的とした機能ですが、
正しく使わないとかえってパフォーマンスを損ねます。
ジェネリックな構造体に対する`in`引数はまさにパフォーマンスを損ねるため、最初から禁止することにしました。

```csharp {title="ジェネリックな構造体を in this にできない理由"}
// (もしもこれをコンパイル エラーにしなかった場合)
public static void M<T>(in this T s)
    where T : IDisposable
{
    // 結局、この Dispose 呼び出しのところでコピーが起こる
    // コピーを避けるためには T が readonly struct でないとダメ
    // インターフェイス越しなので readonly struct かどうかの判定が不可能
    s.Dispose();
    // しかも、メソッドを呼ぶたびにコピー
    s.Dispose();
}
```

### <a id="sec-generated-title-17"></a> <a id="struct-field"></a>構造体のフィールドの参照

「[参照渡し](../resource/sp_ref.md#struct-this)」で振れていますが、構造体のインスタンス メソッドでは、その構造体のフィールドの参照を返せません
(その方が都合のいい場面がある)。

この制約に対する救済策として、`ref`引数の拡張メソッドが使えます。
例えば以下のように、インスタンス メソッドではコンパイル エラーになる`ref`戻り値が、拡張メソッドではコンパイルできます。

```csharp {title="拡張メソッドならフィールドを ref で返せる"}
using System;

struct Point
{
    public int X;
    public int Y;
    public int Z;

    public ref int At(int index)
    {
        switch (index)
        {
            // インスタンス メソッド(プロパティ、インデクサー)では以下の ref が認められていない(コンパイル エラー)
            case 0: return ref X;
            case 1: return ref Y;
            case 2: return ref Z;
            default: throw new IndexOutOfRangeException();
        }
    }
}

static class PointExtensions
{
    public static ref int At(ref this Point p, int index)
    {
        switch (index)
        {
            // インスタンス メソッド版とやっていることは同じでも、こちらは OK
            case 0: return ref p.X;
            case 1: return ref p.Y;
            case 2: return ref p.Z;
            default: throw new IndexOutOfRangeException();
        }
    }
}
```
