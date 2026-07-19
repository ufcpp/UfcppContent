---
title: "ローカル関数と匿名関数"
source_url: "https://ufcpp.net/study/csharp/functional/fun_localfunctions/"
content_type: "Article"
published_at: "2016-07-17T00:00:00"
updated_at: "2023-07-29T00:00:00"
tags: []
umbraco_id: 1929
parent_id: 1275
sort_order: 4
aliases:
  - "/csharp/functional/fun_localfunctions/"
---

# ローカル関数と匿名関数

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

C# には、関数内に関数を書く方法として、ローカル関数と匿名関数という2つの機能があります。

いずれも共通して、以下のような性質があります。

- 定義している関数の中でしか使えない
- 周りの(定義している関数側にある)ローカル変数を取り込める

ローカル関数の方ができることは多いですが、書ける場所は少なくなります。
匿名関数はその逆で、できることに少し制限がある代わりに、どこにでも書けます。

サンプル コード: [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Functional/LocalFunctions](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Functional/LocalFunctions)

## <a id="sec-generated-title-2"></a> <a id="local-function"></a>ローカル関数

<h5 class="version version7">Ver. 7</h5>

C# 7では、関数の中で別の関数を定義して使うことができます。
関数の中でしか使えないため、<strong id="key-local">ローカル関数</strong>(local function: その場所でしか使えない関数)と呼びます。

例えば以下のように書けます。

```csharp
using System;

class Program
{
    static void Main()
    {
        // Main 関数の中で、ローカル関数 f を定義
        int f(int n) => n >= 1 ? n * f(n - 1) : 1;

        Console.WriteLine(f(10));
    }
}
```

ローカル関数(この例でいう `f`)は、定義した関数(この例でいう `Main`メソッド)の中でしか使えません。

ローカル関数は、通常のメソッドでできることであれば概ね何でもできます。例えば、以下のようなこともできます。

- 再帰呼び出し
- イテレーター
- 非同期メソッド

また、メソッド内に限らず、[関数メンバー](../structured/st_function.md#sec-function-member)ならどれの中でも定義できます。

```csharp
class Sample
{
    public Sample()
    {
        int f(int n) => n * n;
    }

    public int Property
    {
        get
        {
            int f(int n) => n * n;
            return f(10);
        }
    }

    public static Sample operator+(Sample x)
    {
        int f(int n) => n * n;
        return null;
    }
}
```

### <a id="sec-generated-title-3"></a> <a id="local-function-attribute"></a>ローカル関数への属性適用

<h5 class="version version9">Ver. 9.0</h5>

ローカル関数の追加当初、ローカル関数には属性を付けれなかったんですが、C# 9.0 でできるようになりました。

```csharp
using System;
using System.Diagnostics.CodeAnalysis;
 
m("", "");
 
static void m(string? a, string? b)
{
    // C# 9.0 からローカル関数に属性を付けれる。
    // C# 8.0 の null 許容参照型がらみで特に有用。
    [return: NotNullIfNotNull("s")]
    string? toLower(string? s) => s?.ToLower();
 
    if (a is not null && b is not null)
    {
        // a, b の null 許容性が、NotNullIfNotNull 属性のおかげで al, bl に伝搬。
        string al = toLower(a);
        string bl = toLower(a);
 
        // a, b が非 null なので、al, bl は非 null で確定済み。改めてのチェック不要。
        Console.WriteLine(al.GetHashCode());
        Console.WriteLine(bl.GetHashCode());
    }
}
```

ローカル関数が追加された C# 7.0 時点で特に属性を付けれない積極的な理由はなく、
9.0 で入ったのは単に実装都合です。
(メソッド本体(`{}` の中身)内で属性を使えるような文法がこれまで全くなくて、
新たに書かないといけないコードが案外多く、
単純な割には実装コストが高くて後回しになっていただけ。
C# 8.0 の [null 許容参照型](../resource/nullablereferencetype.md)がらみでローカル関数にも属性を付けたい需要が急激に増えたので実装優先度が上がったみたいです。)

### <a id="sec-generated-title-4"></a> <a id="local-function-usage"></a>ローカル関数の使い道

ローカル関数を使いたくなる一番の動機は、定義した関数内からだけ使えるというになるでしょう。

あるメソッド`M`の中から、その`M`でしか使わないメソッドを呼び出したい場面が時々あります。
このとき、ローカル関数を使わないと、`M`でしか使わないメソッドに`MInternal`など、あまり意味のない名前を付ける羽目になり、不格好です。

```csharp
static void M()
{
    // 何らかの前準備とか
    MInternal();
}

static void MInternal()
{
    // 実際の処理はこちらで
}
```

名前が不格好な程度ならそれほど大きな問題ではないんですが、
この`MInternal`は、`M`以外のメソッドからも呼べてしまうという問題が発生します。
こういう場合に、ローカル関数を使えば、以下のように書くことができ、呼びたい場所からだけ呼べるようになります。

```csharp
static void M()
{
    // 何らかの前準備とか

    void m()
    {
        // 実際の処理はこちらで
    }

    m();
}
```

#### <a id="sec-generated-title-5"></a> <a id="iterator"></a>例1: イテレーターの引数チェック

例えば、[イテレーター](../data/sp2_iterator.md#iterator)の引数チェックではこういうコードが必要になりがちです。

例として、標準ライブラリ中の処理を1つ自作してみましょう。`Enumerable`クラス(`System.Linq`名前空間)の`Where`メソッドをまねてみます。
まず、単純な書き方をしてみましょう。この書き方には、コメントに書いてあるように、少し欠陥があります。

```csharp
using System;
using System.Collections.Generic;

static class MyEnumerable
{
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        // イテレーター中のコードは、最初に列挙した(foreach などに渡す)時に初めて実行される
        // このメソッドを呼んだ時点では、↓この引数チェックが働かない
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        foreach (var x in source)
            if (predicate(x))
                yield return x;
    }
}
```

コメント中に「メソッドを呼んだ時点では引数チェックが働かない」とありますが、使う側のコードも書いてみると問題がよりはっきりするでしょう。
以下のように、期待されるのと異なるタイミングで例外が起きます。

```csharp
using Iterator1;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        IEnumerable<string> input = null;

        // input が null なので例外を投げてほしい
        // 多くの人がそれを期待する
        var output = input.Where(x => x.Length < 10);

        Console.WriteLine("ここが表示されるとおかしい"); // でも表示される

        foreach (var x in output) // 実際に例外が出るのはこの行
        {
            Console.WriteLine(x);
        }
    }
}
```

そこで、よく以下のような書き方をします。

```csharp
using System;
using System.Collections.Generic;

static class MyEnumerable
{
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        // イテレーターではなくなった(イテレーターなのは WhereInternal の方)ので、ちゃんと呼ばれた時点でチェックが走る
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return WhereInternal(source, predicate);
    }

    private static IEnumerable<T> WhereInternal<T>(IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var x in source)
            if (predicate(x))
                yield return x;
    }
}
```

こういう場面こそ、ローカル関数の出番です。
以下のように書き直すことができます。

```csharp
using System;
using System.Collections.Generic;

static class MyEnumerable
{
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        // イテレーターではなくなった(イテレーターなのは WhereInternal の方)ので、ちゃんと呼ばれた時点でチェックが走る
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        IEnumerable<T> f()
        {
            foreach (var x in source)
                if (predicate(x))
                    yield return x;
        }

        return f();
    }
}
```

#### <a id="sec-generated-title-6"></a> <a id="ToArray"></a>例2: イテレーターをToArrayしてから返す

[イテレーター](../data/sp2_iterator.md#iterator)を使って書きたいものの、
遅延実行(foreachで列挙されて初めて実行される)ではなく即座に実行するために、`ToArray`メソッド(`System.Enumerable`クラスの拡張メソッド)を掛けてから返したい場合があります。

この場合も、1つのメソッドからしか呼ばれないメソッドが作られがちです。
例えば以下のようなコードになります。

```csharp
using System.Collections.Generic;
using System.Linq;

static class MyEnumerable
{
    public static U[] SelectToArray<T, U>(this T[] array, Func<T, U> selector)
    {
        return Select(array, selector).ToArray();
    }

    // SelectToArray からしか呼ばれない
    private static IEnumerable<U> Select<T, U>(IEnumerable<T> source, Func<T, U> selector)
    {
        foreach (var x in source)
            yield return selector(x);
    }
}
```

これも、以下のように書き直せます。

```csharp
using System.Collections.Generic;
using System.Linq;

static class MyEnumerable
{
    public static U[] SelectToArray<T, U>(this T[] array, Func<T, U> selector)
    {
        IEnumerable<U> inner()
        {
            foreach (var x in array)
                yield return selector(x);
        }

        return inner().ToArray();
    }
}
```

#### <a id="sec-generated-title-7"></a> <a id="async-task"></a>例3: 非同期メソッドのキャッシュ

最後の例は、非同期メソッドで作った`Task`のキャッシュです。

非同期メソッドを呼び出すと、呼び出すたびに`Task`クラス(`System.Threading.Tasks`名前空間)のインスタンスが作られます。
しかし、これを、1度だけ呼んで、2度目以降はキャッシュして持っている`Task`を返したいことがあります。

```csharp
static async Task MainAsync()
{
    // 何度か呼ぶけども、キャッシュされているので通信は1回きり
    Console.WriteLine(await LoadAsync());
    Console.WriteLine(await LoadAsync());
    Console.WriteLine(await LoadAsync());
}

static Task<string> LoadAsync()
{
    _loadCache = _loadCache ?? LoadAsyncInternal();
    return _loadCache;
}
static Task<string> _loadCache;

static async Task<string> LoadAsyncInternal()
{
    var c = new HttpClient();
    var res = await c.GetAsync("http://ufcpp.net");
    var content = await res.Content.ReadAsStringAsync();

    return Regex.Match(content, @"\<title\>(.*?)\<").Groups[1].Value;
}
```

これも、以下のように書き直せます。

```csharp
static Task<string> LoadAsync()
{
    async Task<string> inner()
    {
        var c = new HttpClient();
        var res = await c.GetAsync("http://ufcpp.net");
        var content = await res.Content.ReadAsStringAsync();

        return Regex.Match(content, @"\<title\>(.*?)\<").Groups[1].Value;
    }

    _loadCache = _loadCache ?? inner();
    return _loadCache;
}
static Task<string> _loadCache;
```

## <a id="sec-generated-title-8"></a> <a id="anonymous-function"></a>匿名関数 (ラムダ式)

<h5 class="version version2">Ver. 2.0</h5>
<h5 class="version version3">Ver. 3.0</h5>

C# 2.0では[匿名メソッド式](sp_delegate.md#anonymous-method)、C# 3.0では[ラムダ式](sp_delegate.md#lambda)という構文が入り、これらを合わせて<strong id="key-anonymous" class="keyword">匿名関数</strong>と呼びます。

(ラムダ式は匿名メソッド式のほぼ上位互換です。
C#開発者も、「ラムダ式が最初からあれば、匿名メソッド式の構文はC#には不要だった」と言っています。
そのため、匿名メソッド式はC# 2.0時代の互換性を保つためだけの機能だと考えて差し支えないです。
こういう背景から、匿名関数という名前が使われることはあまりなく、
<strong id="key-lambda" class="keyword">ラムダ式</strong>(lambda expression)という言葉の方がよく目にすることになると思います。
本節でも、以下の説明はラムダ式でのみ行います。)

ラムダ式は、以下の例のように、引数リストと関数本体を `=>`でつないで書きます。

```csharp
(int x) =>
{
    var sum = 0;
    for (int i = 0; i < x; i++)
        sum += i;
    return sum;
}
```

この例を見ての通り、関数名がありません。これが「匿名」と呼ばれる理由です。

`=>` は、矢印のように見えることからアロー演算子(arrow operator)と呼ばれたり、
その矢印を「行先」に見立ててgoes to演算子と呼ばれたりします。
(実際、`x => 2 * x`を x goes to 2x (xが2xに行く)と読むと、英語的に案外しっくり来るそうです。)

`=>` の後ろの関数本体の部分は、式が1つだけの場合、`{}`と`return`を省略して、以下のように書くことができます。

```csharp
(int x) => x * x
```

また、`=>`の前の引数リストでは、引数の型を推論できる場合には型を省略できます。
このとき、引数が1つだけであれば、`()`も省略できます。

```csharp
(x, y) => x * y
```

```csharp
x => x * x
```

例えば、以下のような使い方ができます。

```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        var input = new[] { 1, 2, 3, 4, 5 };
        var output = input
            .Where(n => n > 3)
            .Select(n => n * n);

        foreach (var x in output)
        {
            Console.WriteLine(x);
        }
    }
}
```

強調表示している部分が匿名関数です。
匿名関数の引数(`n`)の型は、渡す先（`Where`や`Select`)から推論されます。

## <a id="sec-generated-title-9"></a> <a id="pros-cons"></a>ローカル関数と匿名関数のそれぞれの利点

前節の例のように、匿名関数は式(この例では`Where`メソッドや`Select`メソッドの引数)の中に書くことができます。
ここがローカル関数との最大の違いになります。
ローカル関数の場合は、関数(この場合`Main`メソッド)直下にしか書けません。

匿名関数はどこにでも書けるという利点がある一方で、以下のような制限があります。

- 再帰呼び出しが素直にはできない
- イテレーターにできない
- ジェネリックにできない
- 引数の既定値を持てない

```csharp
// ローカル関数は素直に再帰を書ける
int f1(int n) => n >= 1 ? n * f1(n - 1) : 1;

// 匿名関数はひと手間必要
Func<int, int> f2 = null;
f2 = n => n >= 1 ? n * f2(n - 1) : 1;
```

```csharp
// ローカル関数ならイテレーターにできる
IEnumerable<int> g1(IEnumerable<int> items)
{
    foreach (var x in items)
        yield return 2 * x;
}

// 匿名関数ではコンパイル エラー
Func<IEnumerable<int>, IEnumerable<int>> g2 = items =>
{
    foreach (var x in items)
        yield return 2 * x;
}
```

```csharp
// ローカル関数ならジェネリックに使える
bool eq1<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) == 0;
Console.WriteLine(eq1(1, 2));
Console.WriteLine(eq1("aaa", "aaa"));

// 匿名関数はジェネリックにならない
// Func<T, T, bool> の時点でコンパイル エラー
// where 制約を付ける構文もない
Func<T, T, bool> eq2 = (x, y) => x.CompareTo(y) == 0;
// 当然、呼べない
Console.WriteLine(eq2(1, 2));
Console.WriteLine(eq2("aaa", "aaa"));
```

```csharp
// ローカル関数の引数には既定値を与えられる
int f1(int n = 0) => 2 * n;
Console.WriteLine(f1());
Console.WriteLine(f1(5));

// 匿名関数は無理
// この時点でコンパイル エラー
Func<int, int> f2 = (int n = 0) => 2 * n;
// 当然、呼べない
Console.WriteLine(f2());
Console.WriteLine(f2(5));
```

すなわち、以下のことが言えます。

- ローカル関数は書ける場所が限られるものの、機能的には通常のメソッドと同程度に何でも書ける
- 逆に、匿名関数はどこにでも書ける代わりに、いくつか機能的に制限がある

また、詳しくは「[[雑記] 匿名関数のコンパイル結果](sp2_anonymousmethod.md#closure-local-function)」で説明しますが、
多少、実行性能にも差があります。
呼び出し方次第ではありますが、ローカル関数の方が高速になる場合があります。

### <a id="sec-generated-title-10"></a> <a id="background-local-function"></a>余談: 経緯

ちなみに、C# 7でローカル関数が導入されるに至った経緯としては、匿名関数の制限を緩和してほしいという要望から始まっています。
すなわち、前述の、「匿名関数はイテレーター化できない、再帰呼び出しが大変」という問題の解決策がローカル関数です。

書ける場所にも違いがあるので、この要望が完全に満たされたわけではありません。
しかし、「イテレーター化」あるいは「再帰呼び出し」をしたい場面を改めて考えてみたところ、
「別に式中に書きたいわけじゃない」、「ローカル関数で十分」、「ローカル関数の方が実行性能的にお得になる場面もある」となったみたいです。

## <a id="sec-generated-title-11"></a> <a id="capture-local"></a>ローカル変数の捕獲

ローカル関数でも匿名関数でも、周りの(定義している関数内の)ローカル変数や引数を取り込んで使うことができます。
例えば以下のようなコードが書けます。

```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // ユーザーからの入力をローカル変数に記録
        var m = int.Parse(Console.ReadLine());
        var n = int.Parse(Console.ReadLine());

        var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // ユーザーの入力 m よりも大きいか判定
        bool filter(int x) => x > m;

        var output = input
            .Where(filter)
            .Select(x => n * x); // ユーザーの入力 n を掛ける

        foreach (var x in output)
        {
            Console.WriteLine(x);
        }
    }
}
```

こういう処理を、ローカル変数の捕獲(capture)と言います(カタカナ言葉で「キャプチャする」ともよく言います)。
また、ローカル変数を捕獲しているローカル関数や匿名関数を<strong id="closure" class="keyword">クロージャ</strong>(closure: 囲い込み)と呼んだりします。

捕獲したローカル変数は書き換えることもできます。

```csharp
using System;

class Program
{
    static void Main()
    {
        var x = 1;

        // ローカル関数内で変数xを書き換え
        void f(int n) => x = n;

        Console.WriteLine(x); // 1

        f(2);
        Console.WriteLine(x); // 2
    }
}
```

注意点として、詳しくは「[[雑記] 匿名関数のコンパイル結果](sp2_anonymousmethod.md#closure)」で説明しますが、
ローカル変数の取り込みには少々ペナルティがかかります。
実行性能への要求が極めて高い場合には、避けれるなら避けるべきです
(ペナルティは小さいので、ボトルネックになっていない場所でまで無理に頑張る必要はありません)。

## <a id="sec-generated-title-12"></a> <a id="avoid-capture"></a>ローカル変数捕獲の禁止

前節での説明の通り、外部の変数を捕獲してしまうと少々ペナルティが掛かります。
意図してやっているのならいいんですが、無自覚にやってしまうのは避けたいです。

そこで、C# 8.0 では静的ローカル関数、C# 9.0 では静的匿名関数という仕様が入りました。

### <a id="sec-generated-title-13"></a> <a id="static-local-function"></a>静的ローカル関数

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 から、外部の変数を捕獲しないことを明示するため、
ローカル関数に `static` 修飾を付けれるようになりました。
この機能を<strong id="key-static-local-function" class="keyword">静的ローカル関数</strong>(static local function)と呼びます。

```csharp
void M(int a)
{
    // 外部の変数(引数)を捕獲(クロージャ化)。
    int f(int x) => a * x;
 
    // static を付けて、クロージャ化を禁止。
    // a を使っているところでコンパイル エラーになる。
    static int g(int x) => a * x;
}
```

「[匿名関数のコンパイル結果](sp2_anonymousmethod.md#compile_anonymous)」で説明していますが、
こういう何も捕獲していないローカル関数は、静的メソッドに展開されます。
なので、`static` 修飾子を使って、静的ローカル関数と呼びます。

ちなみに、「静的」の名前が示す通り、インスタンス メンバーの参照もできません。

```csharp
class LocalFunction
{
    public static int StaticProperty { get; set; }
    public int InstanceProperty { get; set; }
 
    public void M()
    {
        // これは OK。
        static int f1() => StaticProperty;
 
        // これはコンパイル エラー。
        static int f2() => InstanceProperty;
    }
}
```

ちなみに、定数や `nameof` であれば外側のスコープにあるものに触ることができます。
例えば以下のコードはコンパイルできます。

```csharp
using System;
 
const string s = "bc";
int a = 0;
 
// a を使っているように見えて、nameof(a) は単に "a" に展開されるのでセーフ。
static string m() => nameof(a) + s;
 
Console.WriteLine(m());
```

### <a id="sec-generated-title-14"></a> <a id="static-anonymous-function"></a>静的匿名関数

<h5 class="version version9">Ver. 9.0</h5>

同様に、C# 9.0 では匿名関数に対しても `static` 修飾子を付けれるようになりました。
意味的には[静的ローカル関数](#static-local-function)と全く同じで、「外部の変数を捕獲しない」という宣言になります。
ラムダ式、匿名メソッド式ともに、式の前に `static` を付けます。

```csharp
using System;
 
int a = 0;
 
// 以下の2行は自身の引数しか使っていないので static にしても怒られない。
Func<int, int> ok1 = static x => x * x;
Func<int, int> ok2 = static delegate (int x) { return x * x; };
 
// 以下の2行は外側のローカル変数 a を使ってしまったのでコンパイル エラー。
Func<int, int> ng1 = static x => a * x;
Func<int, int> ng2 = static delegate (int x) { return a * x; };
```

静的ローカル関数がある時点で匿名関数でも同様のことができてしかるべきもので、
ただちょっと構文解析が大変なので後回しになっていたものです。
順当に「1バージョン遅れで実装」となりました。

### <a id="sec-generated-title-15"></a> <a id="not-pure"></a>注意: 純粋関数(副作用なしのメソッド)ではない

静的ローカル関数にしても静的匿名関数にしても、ローカル変数の捕獲(によるパフォーマンスのペナルティ)は避けることができますが、静的フィールドの読み書きは普通にできます。
例えば以下のコードは有効な C# 8.0 コードになります。

```csharp
class StaticLocalFunction
{
    private static int _count;
 
    public void M()
    {
        // ローカル関数内から外の変数を読み書きしてる。
        // _count が static なのでコンパイル可能。
        static int local() => ++_count;
 
        System.Console.WriteLine(local());
    }
}
```

`static` を付けてもいわゆる純粋関数(pure function、同じ引数で呼べば必ず同じ戻り値が得られる)にはならないので注意してください。

## <a id="sec-generated-title-16"></a> <a id="shadowing"></a>変数のシャドーイング

<h5 class="version version8">Ver. 8.0</h5>

前節の静的ローカル関数に伴って新たに認められた機能に、変数の<strong id="key-shadowing" class="keyword">シャドーイング</strong>(shadowing)というものがあります。
ローカル関数内で、外側にすでに存在している変数や引数と同じ名前で、
新たに変数・引数を定義できる機能です。
外側のものを「影で覆い隠す」という意味で shadowing と呼びます。

```csharp
void M(int a)
{
    int x;
 
    int f(int a) // この a は M(int a) の a とは別物
    {
        var x = a; // この x も外側の x とは別物
        return x;
    }
}
```

C# 8.0 以降であれば、普通のローカル関数でも使えます。
ただ、外側の変数を捕獲したものなのか、ローカル関数側でシャドーイングしたものなのかの区別がわかりにくくなるという問題があるので、静的ローカル関数と同時(C# 8.0)に認められました。
静的ローカル関数でだけ認めるのも気持ち悪く、普通のローカル関数でも認めるようにしたそうです。

## <a id="sec-generated-title-17"></a> <a id="lambda-csharp10"></a>ラムダ式の戻り値の明示と属性

<h5 class="version version10">Ver. 10</h5>

C# 10.0 で、ラムダ式の戻り値を明示できるようになりました。
また、属性も付けられるようになりました。
例えば以下のようなコードが書けます。

```csharp
Func<int, int, int> f =
    [A]
    int (int x, int y)
    => x + y;

class AAttribute : Attribute { }
```

これだけ見るとあまり使い道がなさそうな機能ですが、
同時に入る[デリゲートの自然な型決定](sp_delegate.md#natural-type)と併せるとそれなりに意味を持ちます。
「[自然な型](sp_delegate.md#natural-type)」の方でも書いていますが、
 .NET 6.0 (C# 10.0 と同世代)の Web アプリ テンプレートで作られるコードは以下のようになっています。

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

`MapGet` にラムダ式を渡すことで Web API を簡潔に書けるようになりました。
この書き方がそのまま大規模開発に向いているかというと微妙ですが、
少なくとも入門用のコードとしてはこれくらいの簡潔さが求められています。

この例では、HTTP GET で `/` (Web サイトのルート)にアクセスすると、`Hellow World!` という文字列を返します。
ここで、`/` アクセス時に色々と凝ったことをしようと思うと、属性や戻り値の型を指定したくなります。

### <a id="sec-generated-title-18"></a> <a id="lambda-explicit-return"></a>戻り値の型の明示

ラムダ式に戻り値の型を明示できるようになりました。
戻り値の型は、引数の `()` の前に書きます。
例えば以下のような書き方ができます。

```csharp
// 新文法。
// ラムダ式に戻り値の型を明示。
// (引数も明示。)
Func<int, int> f1 = int (int x) => x;

// 元々の文法。
// 引数の型の方を明示。
Func<int, int> f2 = (int x) => x;

// 新文法。
// 戻り値の型だけ明示。 () が必要。
Func<int, int> f3 = int (x) => x;

// これはエラーになる。
// int が引数に掛かっているのか戻り値に掛かっているのか不明瞭。
Func<int, int> f4 = int x => x;
```

たとえ[自然な型決定](sp_delegate.md#natural-type)と組み合わせたとしても、
たいていの場合は引数だけ型を明示すれば戻り値の型は決定できたりするので、
必要になる場面はそう多くないかもしれません。
以下のようなコード(右辺のラムダ式の部分は C# 9.0 でも有効)でも問題なく自然な型決定ができます。

```csharp
// 引数の int から、戻り値の型が int に決定する。
// その後、ラムダ式の型は Func<int, int> として決定できる。
var f = (int x) => x;
```

(おそらく、後述する属性のついでで実装された(ついででやれたから手間が掛かっていない)機能だと思われます。)

戻り値の型の明示が有効なのは、
例えば、
ラムダ式の中身自体がターゲット型推論に依存している場合などです。
サンプル コードとして[条件演算子のターゲット型推論](../cheatsheet/ap_ver9.md#target-typed-conditional)を使いますが、以下のような式は後者のみ有効になります。

```csharp
// 条件演算子だけでは int と null の共通型が決定できなくて、戻り値の型が決まらない。
// (条件演算子の後方互換性のために掛かってる制限。)
var f1 = (bool x, int y) => x ? y : null;

// 一方で、これなら、戻り値の型からのターゲット型推論で条件演算子を書ける。
// f2 の自然な型決定もできるようになる (Func<bool, int, int?> になる)。
var f2 = int? (bool x, int y) => x ? y : null;
```

ちなみに、[静的匿名関数](#static-local-function)の `static` と併用する場合、戻り値の型を書く場所は `static` の後ろです。
(通常のメソッドと同じ。)

```csharp
// 戻り値の型を各場所は static の後ろ。
var f = static int (int x) => x;
```

また、明示した戻り値の型からラムダ式の引数の型を推論することはできません。

```csharp
// 戻り値の型から引数の型の推論はできない。
// 結果的に、Func<T, int> への代入はできても、自然な型決定(var などへの代入)はできない。
var f6 = int (x) => x;
```

### <a id="sec-generated-title-19"></a> <a id="lambda-attribute"></a>属性

同じくラムダ式に属性を付けれるようになりました。

```csharp
var f =
    [A]
    [return: B]
    static int ([C] int x)
    => x;

[AttributeUsage(AttributeTargets.Method)]
class AAttribute : Attribute { }

[AttributeUsage(AttributeTargets.ReturnValue)]
class BAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter)]
class CAttribute : Attribute { }
```

属性を書く位置は通常のメソッドと同じです。
ラムダ式(メソッド)自体、引数、戻り値が対象になります。
また、([静的匿名関数](#static-local-function)と併用するなら)ラムダ式全体(メソッド全体)や戻り値に属性を付けたい場合は `static` よりも前に書きます。

これも、 .NET 6 の新しい Web テンプレートで使います。
`MapGet` などのメソッドでは、引数などに属性を付けて Web API の挙動をカスタマイズできます。
例えば以下のような書き方ができます。

```csharp
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// テンプレに1行を追加。DI 用。
builder.Services.AddSingleton<Counter>();

var app = builder.Build();

// テンプレを1行書き換え。引数を DI で受け取ったり、クエリ文字列から受け取ったり。
// counter: ページをリロードするたびに +1。
// value: クエリ文字列で数値を指定。
// その2つの値から何らかの計算して返す。
app.MapGet("/", ([FromServices] Counter counter, [FromQuery] int? value) => counter.Count * (value ?? 1));

app.Run();

// テンプレに1クラス追加。上記 DI で渡すデモ用の型。
class Counter
{
    private int _count;
    public int Count { get => _count++; }
}
```

## <a id="sec-generated-title-20"></a> <a id="lambda-default">ラムダ式のオプション引数(既定値)と params 引数</a>

<h5 class="version version12">Ver. 12</h5>

C# 12 でラムダ式の引数に[オプション引数](../structured/sp4_optional.md#optional)にできる(既定値を与えられる)ようになりました。
また、[params 引数](../structured/sp_params.md)も使えるようになりました。

```csharp
// オプション引数(既定値値指定)。
var f1 = (int x = 1) => 0;

// params 引数。
var f2 = (params int[] x) => 0;

// 混在も OK。
var f3 = (int x = 1, params int[] y) => 0;
```

前節の[戻り値の型の指定や属性付与](#lambda-csharp10)と同様、
[デリゲートの自然な型](sp_delegate.md#natural-type)との併用で使ったり、
リフレクションを使って情報を取得するために使います。

自然な型決定(要するに `var` への代入)した場合、
匿名デリゲート型が生成されて、既定値や params の情報が残ります。
例えば `(int x = 1) => x` であれば `delegate int F(int x = 1)` 相当の匿名デリゲート型が生成されます。

```csharp
// 引数にデフォルト値指定。
// delegate int <anonymous>(int x = 1); みたいな匿名デリゲート型になる。
var f1 = (int x = 1) => 0;

f1(); // f1(1) と同じ。

// params 引数。
// delegate int <anonymous>(params int[] x); みたいな匿名デリゲート型になる。
var f2 = (params int[] x) => 0;

f2(1, 2, 3); // f2(new int[] { 1, 2, 3 }) と同じ。
```

一方で、既定値違い、params 違いのデリゲート型への代入もできてしまいます。
この場合、既定値などの情報は消えます。
(ちょっと罠なので、一応、警告はしてくれます。)

```csharp
// 既定値の情報がないデリゲート型に代入。
Action<int> f1 = (int x = 1) => { };

f1(); // エラー。 f1(1) と書かないとダメ。

// params の情報がないデリゲート型に代入。
Action<int[]> f2 = (params int[] x) => { };

f2(1, 2, 3); // エラー。 f2(new int[] { 1, 2, 3 }) と書かないとダメ。
```

この点についてもう少し踏み込んで注意すると、
ラムダ式の側とデリゲート型の側で異なる既定値を与えたとき、
リフレクションで値を取るときに変なことが起きたりもします。
`Delegate.Method` で取る情報(ラムダ式側)と、`Type.GetMethod` で取る情報(デリゲート型型)が食い違います。

```csharp
using System.Reflection;

// ラムダ式としては既定値 2。
// ちゃんと警告にはなるものの、無視してしまうと…
A a = (int x = 2) => { };

MethodInfo m1 = a.Method; // ラムダ式側の情報が取れる。
MethodInfo m2 = a.GetType().GetMethod("Invoke")!; // デリゲート型側の情報が取れる。

Console.WriteLine(m1.GetParameters()[0].DefaultValue); // 2
Console.WriteLine(m2.GetParameters()[0].DefaultValue); // 1

// デリゲート型としては既定値 1。
delegate void A(int x = 1);
```

## <a id="sec-generated-title-21"></a> <a id="simple-param-with-modifier">修飾子付きの引数の型名省略</a>

<h5 class="version version14">Ver. 14.0</h5>

ラムダ式には導入以来ずっと、「型を推論できる限り、引数の型は省略できる」という仕様があります。
ところが、`ref` や `out` などの修飾子が必須の引数の場合、C# 13 までは型名省略できませんでした。
これが C# 14 で改善されました。

例えば、`int` などをはじめ多くの型が以下のような `TryParse` メソッドを持っています。


```csharp
using System.Diagnostics.CodeAnalysis;

readonly struct Int32
{
    public static bool TryParse([NotNullWhen(true)] string? s, out int result)
    {
        // ...
    }
}
```

以下のように、この類の処理用のデリゲート型があったとして、

```csharp
delegate bool TryParse<T>(string text, out T result);
```

C# 13 までは以下のように書くことができませんでした。

```csharp
// C# 13 までは書けなかった。
TryParse<int> m = (text, out result) => { result = 0; return true; };

// out や ref がないならこう書けるのに…
Func<string, int, int> f = (text, x) => 0;

// out が付いた瞬間、型名が必須だった(これなら C# 13 でもコンパイル可能)。
TryParse<int> m13 = (string text, out int result) => { result = 0; return true; };

delegate bool TryParse<T>(string text, out T result);
```

これが C# 14 では認められます。

```csharp
// C# 14 で可能に。
TryParse<int> m = (text, out result) => { result = 0; return true; };
```

対象となる修飾子は `out`、`ref`、`in`、`ref readonly`、`scoped` などです。

```csharp
M m = (in a, ref b, out c, ref readonly d, scoped e) => c = 0;

// C# 13 以前だと以下のように書く必要あり。
M m13 = (in int a, ref int b, out int c, ref readonly int d, scoped Span<int> e) => c = 0;

delegate void M(in int a, ref int b, out int c, ref readonly int d, scoped Span<int> e);
```

ちなみに、これが認められているのは引数リストを `()` でくくっている場合だけです。
ラムダ式は、引数が1つだけの時は `x => { }` というように引数リストの `()` も省略できるわけですが、
この場合は `ref x => { }` みたいな書き方はできません(というか元々、`int x => { }` みたいな型名指定も許されていません)。

```csharp
// 修飾子をつけたい場合、() は必須。
In m1 = (in int a) => { };
In m2 = (in a) => { };

// () 省略不可でエラーに。
In m3 = in int a => { };
In m4 = in a => { };

// ちなみに、in を抜こうとすると型が合わなくてエラーになる。
In m5 = (int a) => { };
In m6 = (a) => { };
In m7 = a => { };

// 参考: 修飾子がない場合:
Value v1 = a => { };
Value v2 = (a) => { };
Value v3 = (int a) => { };
// Value v4 = int a => { }; はこっちでもダメ。コンパイル エラーに。

delegate void Value(int a);
delegate void In(in int a);
```
