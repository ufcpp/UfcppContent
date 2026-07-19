---
title: "[雑記] 匿名関数のコンパイル結果"
source_url: "https://ufcpp.net/study/csharp/functional/sp2_anonymousmethod/"
content_type: "Article"
published_at: "2008-03-09T00:00:00"
updated_at: "2017-12-03T00:00:00"
tags:
  - "Ver. 2.0"
umbraco_id: 1279
parent_id: 1275
sort_order: 5
aliases:
  - "/csharp/functional/sp2_anonymousmethod/"
  - "/csharp/sp2_anonymousmethod"
  - "/csharp/sp2_anonymousmethod.html"
  - "/study/csharp/sp2_anonymousmethod"
  - "/study/csharp/sp2_anonymousmethod.html"
---

# \[雑記\] 匿名関数のコンパイル結果

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

本項では、
[匿名関数](sp_delegate.md#anonymous)が内部的にどう実現されているかについて説明します。

匿名関数は、C# コンパイラーによって普通のメソッドに展開されます。
単にメソッドが1つ生成されるだけの場合もあれば、クラスを丸ごと生成する場合もあります。

## <a id="sec-generated-title-2"></a> <a id="compile_anonymous"></a>匿名関数のコンパイル結果

例えば、以下のようなコードは、

```csharp
class Program
{
  static void Main(string[] args)
  {
      Func<int> f1 = () => 0;
      f1();
  }
}
```

以下のコードと同じ意味になります。
(※ 古いC#コンパイラーの実装の場合だけです。現在は、静的メソッドの場合もう少し複雑なコード生成にした方がパフォーマンスがいいらしく、変換結果が変わっています。現在の実装については[後述](#static))

```csharp
class Program
{
    static int AnonymousMethod1()
    {
        return 0;
    }

    static void Main(string[] args)
    {
        Func<int> f1 = AnonymousMethod1;
        f1();
    }
}
```

この例の場合は、クラスのフィールドの使わず、ローカル変数の捕獲もしていないので、静的メソッドに変換されます。

ただし、`AnonymousMethod1` の部分は、
実際には `<Main>b__0` とかいうような、
C# では通常記述できないような特殊な名前になっていて、
プログラマが明示的に参照することはできません。


### <a id="sec-generated-title-3"></a> <a id="instance-member"></a>メンバー変数を参照する場合

匿名関数内で、クラスのメンバー変数を参照するような場合には、
インスタンス メソッド（非 static なメソッド）が自動生成されます。

例えば、以下のようなコードは、

```csharp
class Program
{
  int member = 0;

  void Method()
  {
    // 2. メンバー変数を参照する匿名関数
    Func<int> f2 = () => this.member;
    f2();
  }
}
```

以下のように展開されます。

```csharp
class Program
{
  int AnonymousMethod2()
  {
    return this.member;
  }

  int member = 0;

  void Method()
  {
    Func<int> f2 = AnonymousMethod2;
    f2();
  }
}
```

### <a id="sec-generated-title-4"></a> <a id="closure"></a>クロージャ(ローカル変数を参照する)の場合

ローカル変数を参照するような匿名関数(クロージャ)を書いた場合、
クラスまで自動生成されます。

例えば、以下のようなコードは、

```csharp
class Program
{
  static void Main(string[] args)
  {
    // 3. ローカル変数を参照する匿名関数
    int x = 0;
    Func<int> f3 = () => ++x;
    f3();

    Console.Write(x);
  }
}
```

コンパイル時に以下のようなクラスを生成したうえで、実行時にそのインスタンスが作られます。

```csharp
class Program
{
  class AnonymousClass
  {
    public int x;

    public int AnonymousMethod()
    {
      return ++this.x;
    }
  }

  static void Main(string[] args)
  {
    var temp = new AnonymousClass();
    temp.x = 0;
    Func<int> f3 = temp.AnonymousMethod;
    f3();

    Console.Write(temp.x);
  }
}
```

ローカル変数の変わりに、自動生成されたクラスのメンバー変数アクセスになっています。

呼び出し元とクロージャ側とで、ローカル変数`x`の書き換え結果が共有される(実行結果で 1 が表示される)のは、このコード生成のおかげです。
例えば以下のように、ローカル変数を書き換えるコードを書いたとします。

```csharp
class Program
{
  static void Main(string[] args)
  {
    int x = 0;
    Action f = () => Console.Write(x);

    x = 1;
    f();
  }
}
```

このコードは以下のように展開されます。

```csharp
class Program
{
  class AnonymousClass
  {
    public int x;

    public void AnonymousMethod()
    {
      Console.Write(this.x);
    }
  }

  static void Main(string[] args)
  {
    var temp = new AnonymousClass();
    temp.x = 0;
    Action f = temp.AnonymousMethod;

    temp.x = 1;
    f();
  }
```

すなわち、元々のコードでローカル変数だったものは、クラスのフィールドになっています。
これを、「ローカル変数がフィールドに昇格(elevate)した」と言ったりします。
「昇格」と言っても、えらくなったわけでなくて、むしろ、実行性能上はペナルティになります。
クラスのインスタンスが1つ余計に作られる分、ちょっとした負担が発生しています。

### <a id="sec-generated-title-5"></a> <a id="closure-local-function"></a>ローカル関数かつクロージャの場合

前述の通り、クロージャにはローカル変数の昇格と、それに伴う余計なインスタンス生成が伴います。
これに対して、状況が許せばその余計なインスタンス生成を避けるような最適化ができます。
最適化できる状況は、以下の通りです。

- ローカル関数でクロージャを作っている(匿名関数ではない)
- デリゲートに代入したりせず、直接関数呼び出ししている

```csharp
static void M1(int m, int n)
{
    // 最適化できる状況: ローカル関数を直接呼出し
    int f(int x, int y) => m * x + n * y;
    var r = f(3, 4);
}

static void M2(int m, int n)
{
    // できない状況1: デリゲート越しに使っている
    int f(int x, int y) => m * x + n * y;
    Func<int, int, int> func = f;
    var r2 = func(3, 4);
}

static void M3(int m, int n)
{
    // できない状況2: 匿名関数を使っている
    Func<int, int, int> f3 = (x, y) => m * x + n * y;
    var r3 = f3(3, 4);
}
```

最適化できる状況、例えばこの例の`M1`の場合、以下のようなコードに展開されます。

```csharp
struct State
{
    public int m;
    public int n;
}

static int Anonymous(int x, int y, ref State state)
{
    return state.m * x + state.n * y;
}

static void M1(int m, int n)
{
    // 最適化できる状況: ローカル関数を直接呼出し
    var state = new State { m = m, n = n };
    var r = Anonymous(3, 4, ref state);
}
```

この違いは構造体とクラス(値型と参照型)の差によります。
詳しくは「[値型と参照型](../resource/oo_reference.md)」で説明していますが、
参照型を使うとヒープの確保という少し重たい処理が必要になります。
状況が許すなら値型を使って性能改善ができる場合があり、本節で説明しているクロージャの最適化はまさにその場合に当てはまります。

## <a id="sec-generated-title-6"></a> <a id="static"></a>補足: 静的メソッドにできる場合でも静的メソッドにしない

冒頭の例のように、
インスタンス メンバーもローカル変数使っていないような場合、匿名関数は静的メソッドとして実装してもよいはずです。
実際、昔の C# コンパイラーは静的メソッドを生成していました。

しかし、C# 6.0の頃から、静的メソッドは使わなくなりました。
例えば、冒頭の例を改めて使いますが、以下の例の場合、

```csharp
class Program
{
  static void Main(string[] args)
  {
      Func<int> f1 = () => 0;
      f1();
  }
}
```

C# 5.0までは静的メソッドが生成されていましたが、
現在は以下のように展開されます。

```csharp
class Program
{
    class AnonymousClass
    {
        public static readonly AnonymousClassSingleton = new AnonymousClass();
        public static Func<int> Cache1;

        internal int AnonymousMethod1()
        {
            return 0;
        }
    }

    static void Main(string[] args)
    {
        if (AnonymousClass.Cache1 == null)
        {
            AnonymousClass.Cache1 = AnonymousClass.Singleton.AnonymousMethod1;
        }
        Func<int> f1 = AnonymousClass.Cache1;
        f1();
    }
}
```

変更の理由は、この方がパフォーマンスがいいからです。
これでパフォーマンスが改善する理由は主に以下の2つです。

- キャッシュ: 作ったデリゲートはキャッシュできる
- デリゲートの性質: デリゲートがそもそも静的メソッドに対してパフォーマンスが良くない

##### <a id="sec-generated-title-7"></a>キャッシュ

デリゲートに対して`Action f = temp.AnonymousMethod;` と言うようにメソッドを代入するとき、
実際には`Action f = new Action(temp.AnonymousMethod);` というような`new`が挟まります。
この`new`の負担は大したものではないですが、なくて済むならない方がいい程度には、無視できない負担になります。

インスタンスが毎回変わる場合には、デリゲートも毎回`new`する必要がありますが、
ここで説明している例の場合は常に同じインスタンス(`AnonymousClass.Singleton`)が相手なので、
デリゲートも1インスタンスあれば十分です。

そこで、デリゲート自体をキャッシュする(`AnonymousClass.Cache`に持つ)ことでパフォーマンスが向上します。

##### <a id="sec-generated-title-8"></a>デリゲートの性質

詳細は「[[雑記] デリゲートの内部](miscdelegateinternal.md#static-method)」で説明しますが、
デリゲートの内部の仕組み上、
静的メソッドからデリゲートを作るとそれだけで遅かったりします。

これは、匿名クラスのインスタンスが1つ余計に作られる負担を差し引いてもおつりが来るくらい遅いです。
したがって、匿名関数の生成結果はインスタンス メソッドにした方が速くなります。

## <a id="sec-generated-title-9"></a> <a id="multiple-functions"></a>補足: 同じスコープに複数の匿名関数がある場合

同じスコープに複数の匿名関数がある場合、1つのクラスにまとめてメソッドが生成されます。
例えば以下のコードの場合、

```csharp
using System;

class Program
{
    static void F(int m)
    {
        // ローカル関数かラムダ式か匿名デリゲート式かは無関係
        void a(int x) => Console.WriteLine("A " + m * x);
        Action<int> b = x => Console.WriteLine("B " + m * x);
        Action<int> c = delegate (int x) { Console.WriteLine("C " + m * x); };

        Invoke(a, b, c);
    }

    static void Invoke(params Action<int>[] list)
    {
        foreach (var item in list) item(1);
    }
}
```

以下のように展開されます。

```csharp
using System;

// a, b, c いずれも1つの型にまとまる
class AnonymousClass
{
    public int m;
    internal void A(int x) => Console.WriteLine("A " + x);
    internal void B(int x) => Console.WriteLine("B " + x);
    internal void C(int x) => Console.WriteLine("C " + x);
}

class Program
{
    static void F(int m)
    {
        // 作られるインスタンスは1つだけ
        var anonymous = new AnonymousClass();
        anonymous.m = m;

        Action<int> a = anonymous.A;
        Action<int> b = anonymous.B;
        Action<int> c = anonymous.C;

        Invoke(a, b, c);
    }

    static void Invoke(params Action<int>[] list)
    {
        foreach (var item in list) item(1);
    }
}
```

コンパイラーによって作られるインスタンスが1つで済むという意味ではこの作りはお得です。

その一方で、この作りには、キャプチャしたローカル変数の寿命が一蓮托生になるという欠点があります。
寿命を変えるべきものは、同じスコープでキャプチャしないようにしましょう。
例えば以下のようなコードを書いてしまうと、
短寿命でガベージ コレクションされて欲しい大きなデータがいつまでたっても回収されないという問題が起こります。

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // この2つの配列の寿命は一蓮托生になる
        var smallData = new int[5];
        var bigData = new int[10000];

        // 小さいデータしか握っていないので長寿でもそこまで問題のないデリゲート
        Func<int, int> f1 = i => smallData[i];

        // 大きめのデータを握っていて、長寿だと問題の出るデリゲート
        Func<int, int> f2 = i => bigData[i];

        // f1, f2 を使う何か
        f1(0);
        f2(0);

        // f2 の寿命が長いと問題なので用が済み次第消す
        f2 = null;

        await Task.Delay(TimeSpan.FromHours(10));

        // f1 は後で使いたい
        // f1 が生きている限り、f2 を消しても結局 bigData は残る
        f1(0);
    }
}
```
