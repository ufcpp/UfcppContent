---
title: "パターン マッチング"
source_url: "https://ufcpp.net/study/csharp/datatype/patterns/"
content_type: "Article"
published_at: "2018-11-24T00:00:00"
updated_at: "2021-09-20T00:00:00"
tags: []
umbraco_id: 2176
parent_id: 1940
sort_order: 3
aliases:
  - "/study/csharp/datatype/patternmatching"
  - "/study/csharp/datatype/patternmatching/"
---

# パターン マッチング

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

[前項](typeswitch.md)で説明した通り、C# 7.0で、`is`演算子と`swtich`ステートメントが拡張されて、`is`/`case`の後ろにパターンを書けるようになりました。
パターンには以下のようなものがあります。

| パターン | バージョン | 概要 | 例 |
| --- | --- | --- | ------------- |
| [型パターン](#declaration) | C# 7.0 | 型の判定 | `int i`、`string s` |
| [定数パターン](#constant) | C# 7.0 | 定数との比較 | `null`、`1` |
| [var パターン](#var) | C# 7.0 | 何にでもマッチ・変数で受け取り | `var x` |
| [破棄パターン](#discard) | C# 8.0 | 何にでもマッチ・無視 | `_` |
| [位置パターン](#positional) | C# 8.0 | [分解](deconstruction.md)と同じ要領で、再帰的にマッチングする | `(1, var i, _)` |
| [プロパティ パターン](#property) | C# 8.0 | プロパティに対して再帰的にマッチングする | `{ A: 1, B: var i }` |
| [パターンの組み合わせ](#pattern-combintor) | C# 9.0 | `and` や `or` などでパターンの組み合わせができる | `int x and (x is 0 or 1)` |
| [関係演算パターン](#relational-patterns) | C# 9.0 | `<` や `>` などで数値の範囲を指定してマッチングする | `<= 0 and < 10` |
| [リスト パターン](#list) | C# 11.0 | 配列やリストなどにマッチ | `[]`, `[_, ..]` |

サンプル コード: [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Data/Patterns](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Data/Patterns)

## <a id="sec-generated-title-2"></a> <a id="nonrecursive"></a>非再帰パターン

<h5 class="version version7">Ver. 7.0</h5>

C# の文法上の区別する意味はないんですが、
パターンのうち、C# 7.0 で入ったものと 8.0 で入ったものの一番の差は再帰があるかどうかです。
C# 7.0 からあるパターンは1層限り、8.0 で追加されたパターンは再帰的に何層もマッチできます。
(再帰がある方が難しいので後からの追加になりました。)

ここではまず、文法が簡単な再帰のないパターンから説明していきます。

### <a id="sec-generated-title-3"></a> <a id="declaration"></a>型パターン (宣言パターン)

C# 6.0以前から元々あった [`is` 演算子](../oop/oo_polymorphism.md#is-operator)の自然な拡張になっているのが型パターン(type pattern)です。
以下のように、型の後ろに続けて、マッチした結果を変数で受け取れます。

```csharp
static void M(object x)
{
    if (x is int i) Console.WriteLine("int " + i);
    else if (x is string s) Console.WriteLine("string " + s);
}
```

`is` や `case` の後ろで変数宣言をしているような形なので、宣言パターン(declaration pattern)とも呼びます。
(というか、C# 8.0以降は宣言パターンの方が正式な呼び方に変わっていそうです。)

型パターンは、旧来からある `is` 演算子や `as` 演算子とほぼ同じ挙動です。
上記の例は、概ね以下のコードと同じ動作になります。

```csharp
if (x is int)
{
    var i = (int)x;
    Console.WriteLine("int " + i);
}
else
{
    string s = x as string;
    if (s != null)
    {
        Console.WriteLine("string " + s);
    }
}
```

`as` + `!= null` になっていることからわかる通り、
型パターンは null にはマッチしません。
(以下のように、たとえ変数の型が一致していたとしても、null にはマッチしません。)

```csharp
static void Main()
{
    M("abc"); // matched abc
    M(null);  // 何も表示されない
}
 
static void M(string x)
{
    if (x is string s) Console.WriteLine("matched " + s);
}
```

#### <a id="sec-generated-title-4"></a> <a id="simplified-type-pattern"></a>型パターンの簡単化

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 で型パターンがちょっとだけシンプルになりました。

型パターンは元々 C# 1.0 からある `is` 演算子の延長として作られています。
ところが、`is` の場合は `x is T` と書けるのに、`switch` では `T _` のように変数宣言か `_` (破棄) を伴う必要がありました。
これが C# 9.0 で改善されています。

```csharp
int Is(object x)
{
    if (x is string)
    {
        return 1;
    }
    return 0;
}
 
int Switch(object x)
{
    switch (x)
    {
        // C# 8.0 までは string _ と書く必要あり
        case string: return 1;
    }
    return 0;
}
 
int SwitchExpr(object x) => x switch
{
    // C# 8.0 までは string _ と書く必要あり
    string => 1,
    _ => 0,
};
```

C# 9.0 時点でこれが書けたなかったのは次節の[定数パターン](#constant)との混同を避けるためです。
例えば C# 9.0 では以下のようなコードが書けます。
こんなコードを書くこと自体少ないと思いますが、`is`の場合と`switch`の場合で、型と定数、どちらが優先されるかが違うので注意が必要です。

```csharp
class X { }
 
class Program1
{
    static int M(object x) => x switch
    {
        X => 1, // これは x の型がクラス X
        _ => 0,
    };
}
 
class Program2
{
    const int X = 1;
 
    static int M1(object x) => x switch
    {
        X => 1, // これは定数 1
        _ => 0,
    };
 
    static bool M2(object x) => x is X; // でもこれはクラス X (C# 8.0 以前との互換性のため)
}
```

### <a id="sec-generated-title-5"></a> <a id="constant"></a>定数パターン

`is`や`case`の後ろには定数も書けます。これを定数パターン(constant pattern)と言います。
単体で見ると普通に `==` を使えば済むことも多いわけですが、
定数パターンであれば他のパターンとの混在ができます。

```csharp
switch (x)
{
    // 定数パターン
    case 0: return 0;
    // 型パターン
    case string s: return s.Length;
    default: return -1;
}
```

名前通り定数しか使えません。
変数との値比較がしたければ、`when`句を使うなどが必要です。

```csharp
static int M(object x, int comparand)
{
    switch (x)
    {
        // case comparand: とは書けない。
        // 型パターン + when 句を使う。
        case int i when i == comparand: return 0;
        default: return -1;
    }
}
```

ちなみに、定数パターンでは、[ユーザー定義演算子](../oop/oo_operator.md#udo)を見ません。
以下のように、`==`と`is`で挙動が違う場合があります。

```csharp
using System;
 
class X
{
    // 全てのインスタンスが等しいという挙動。
    // 当然、x == null も常に true。
    public static bool operator ==(X a, X b) => true;
    public static bool operator !=(X a, X b) => false;
}
 
class Program
{
    static void Main()
    {
        var x = new X();
 
        // なんでも true なので、== null も true
        Console.WriteLine(x == null);
 
        // ユーザー定義の == は見ない。x が本当に null かどうかを見て、false になる
        Console.WriteLine(x is null);
    }
}
```

#### <a id="sec-generated-title-6"></a> <a id="pointer-null"></a>ポインターの null 比較

<h5 class="version version8">Ver. 8.0</h5>

細かい修正ですが、C# 8.0 からポインターに対してもパターン マッチングが使えるようになりました。
といってもプロパティや `Deconstruct` メソッドを持っているわけではないので、実質的には `is null` チェック用です。

```csharp
static unsafe void M(int* p)
{
    // 元々 OK。
    Console.WriteLine(p == null);
 
    // C# 8.0 から OK。
    Console.WriteLine(p is null);
}
```

#### <a id="sec-generated-title-7"></a> <a id="span">ReadOnlySpan に対するパターンマッチ</a>

<h5 class="version version11">Ver. 11</h5>

C# 11 で、`ReadOnlySpan<char>` に対して文字列リテラルによる定数パターンが使えるようになりました。

```csharp
// string を渡せたところには ReadOnlySpan<char> を渡せるように。
ReadOnlySpan<char> s = Console.ReadLine();

// is も
if (s is "a") { }

// switch ステートメントも
switch (s)
{
    case "b":
        break;
}

// switch 式も OK。
var x = s switch
{
    "c" => 1,
    _ => 2,
};
```

文字列処理に対して `ReadOnlySpan<char>` を使う機会が多くなってきたので特殊対応したそうです。

(パターンに書かれているのは `""` みたいな「定数」ですが、
そこに `string` から `ReadOnlySpan<char>` の変換が挟まっていて定数とは言い切れない状態です。
C# チーム自身はそれほど実装に乗り気ではなく、外部からのコントリビューションで実装された機能になります。)

### <a id="sec-generated-title-8"></a> <a id="var"></a>var パターン

型パターンと似ていますが、具体的な型名の代わりに `var` キーワードを使うと、
任意の型にマッチするパターンになります。
これを var パターン (var pattern)と言います。

`switch` の最後に書いて「その他全部」な分岐に使ったりします。

```csharp
static int M(object x)
{
    switch(x)
    {
        case 0: return 0;
        case string s: return s.Length;
        case var other: return other.GetHashCode();
        // あるいは、変数で受け取る必要がないときは _ にしておけば破棄の意味なる
        // case var _:
    }
}
```

あと、少し悪用気味ではありますが、式中での変数宣言に使えたりします。

```csharp
while (Console.ReadLine() is var line && !string.IsNullOrEmpty(line))
{
    Console.WriteLine(line);
}
```

1つ注意が必要な点として、var パターンは型パターンと違って、null にもマッチします。

```csharp
string s = null;
Console.WriteLine(s is string x); // false
Console.WriteLine(s is var y);    // true
```

null をはじきたい場合は、var ではなく、後述するプロパティ パターンを使って`x is {} nonNull`と書いたりします。

### <a id="sec-generated-title-9"></a> <a id="discards"></a><a id="discard"></a>破棄パターン

<h5 class="version version8">Ver. 8.0</h5>

何にでもマッチして、マッチ結果を受け取る必要がない場合、`_` を使って値を破棄できます。これを破棄パターン(discard pattern)と言います。

再帰はしないんですが、`switch`式の中と、再帰パターン内でしか使えないので C# 8.0 での実装になります。
`is`やステートメントの方の`switch`の`case`の後ろでは`var _`と書く必要がありますが、`switch`式の場合は`_`だけで値を破棄します。

```csharp
static int M(object x)
    => x switch
    {
        0 => 0,
        string s => s.Length,
        _ => -1
    };
```

### <a id="sec-generated-title-10"></a> <a id="breaking-change-in-discard"></a>余談: 破棄パターンが C# 8.0 からな理由

ちなみに、`is` や `switch`ステートメント内で `_` だけでの値の破棄ができないのは既存コードとの互換性のためです。
普通書かないようなコードですが、一応、以下のようなコードが元々合法なため、意味を変えることができませんでした。

```csharp
using System;
 
class _Type
{
    class _ { }
 
    static void M(object x)
    {
        Console.WriteLine(x is _); // class _ とのマッチ
    }
}
 
class _Constant
{
    const int _ = 0;
 
    static void M(object x)
    {
        switch (x)
        {
            case _: // 定数 _ とのマッチ
                break;
        }
    }
}
```

(あまりにも紛らわしいので、このコードを C# 8.0 でコンパイルすると警告が出ます。)

<!-- original-page-break -->

## <a id="sec-generated-title-11"></a> <a id="recursive"></a>再帰パターン

<h5 class="version version8">Ver. 8.0</h5>

C# 7.0 の範囲で使えるものは、「パターン」と呼ぶのが仰々しいくらい単純なものでした。
C# 8.0 で、再帰的に使えるパターンが追加されて、ようやくパターン マッチングらしくなりました。

例えば以下のような感じです。

```csharp
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x = 0, int y = 0) => (X, Y) = (x, y);
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
 
class Program
{
    static int M(object obj)
        => obj switch
    {
        0 => 1,
        int i => 2,
        Point (1, _) => 4, // new!
        Point { X: 2, Y: var y } => y, // new!
        _ => 0
    };
}
```

### <a id="sec-generated-title-12"></a> <a id="positional"></a>位置パターン

位置パターン (positional pattern)は、
[分解](deconstruction.md)と同じ要領で再帰的なマッチングを行うパターンです。

分解と同様、`Deconstruct`メソッドを呼んでメンバーを取り出した上で、
それぞれのメンバーの値に対してマッチングを行います。
例えば、先ほど例として使った`Point`クラスを引き続き使うとして、以下のように書けます。

```csharp
static int M(Point p)
    => p switch
{
    (1, 2) => 0,
    (var x, _) when x > 0 => x,
    _ => -1
};
```

このコードは概ね以下のような意味になります。

```csharp
p.Deconstruct(out var x, out var y);
if (x is 1 && y is 2) return 0;
if (x > 0) return x;
return -1;
```

サブパターンの順序に意味があるため「位置」パターンという呼び名になっています。

上記の例では元々型が`Point`だとわかっているので型名を省略していますが、
型の明示もできます。

```csharp
static int M(object obj)
    => obj switch
{
    int i => i,
    string s => s.Length,
    Point(var x, var y) => 0,
    _ => -1
};
```

また、後述しますが、プロパティ パターンとの混在や、
型パターンのように変数を付け足すこともできます。

```csharp
obj switch
{
    Point (var x, _) { Y: var y } p => x * y
};
```

位置パターンとか言いつつ、名前付き引数のノリで、名前付きなパターン マッチングもできます。

```csharp
static int NamedPattern(Point p)
    => p switch
{
    (x: 1, y: 2) => 0,
    (x: var x, y: _) when x > 0 => x,
    _ => -1
};
```

#### <a id="sec-generated-title-13"></a> <a id="constructor-vs-positional"></a>補足: コンストラクター呼び出しの逆

位置パターンは、コンストラクター呼び出し(`new`)の逆に当たる構文です。
書き方も、コンストラクターと対になっています。

```csharp
// 位置指定で構築できるんなら、位置指定でマッチングできるべき
var p1 = new Point(1, 2);
var r1 = p1 is Point (1, 2);
 
// 名前指定で構築できるんなら、名前指定でマッチングできるべき
var p2 = new Point(x: 1, y: 2);
var r2 = p2 is Point (x: 1, y: 2);
 
// 型推論が効く場合に new の後ろの型名は省略可能(になる予定)なら
// 型が既知なら型名を省略してマッチングできるべき
Point p3 = new (1, 2);
var r3 = p3 is (1, 2);
 
// 階層的に new できるんなら、階層的にマッチングできるべき
var line = new Line(new Point(1, 2), new Point(3, 4));
var r4 = line is ((1, 2), (3, 4));
```

#### <a id="sec-generated-title-14"></a> <a id="how-to-deconstruct"></a>分解方法

位置パターンは [分解](deconstruction.md)と同じ要領でメンバーの値を取り出します。
分解もそうなんですが、[タプル](tuples.md)(C# のタプル構文を使って作る `ValueTuple` 構造体の値)の場合とそうでない場合で内部的な挙動が少し変わります。

まず、タプルの場合、コンパイラーの最適化によって、タプルのフィールドを直接参照するようなコードが生成されます。
例えば以下のようなコードを書いた場合、

```csharp
public bool TupleSyntax((int a, int b) x) => x is (1, 2);
```

以下のようなコードと同じような挙動をします。

```csharp
// ValueTuple の場合は直接フィールドを参照する。
public bool TupleSyntax((int a, int b) x)
{
    return x.a == 1 && x.b == 2;
}
```

そうでない場合、まずはコンパイル時に `Deconstruct` メソッドを探します。
見つかった場合は、それを使うコードが生成されます。
例として以下のようなクラスを用意します。

```csharp
using System.Runtime.CompilerServices;

class X : ITuple
{
    public object this[int index] => index;
    public int Length => 2;
    public void Deconstruct(out int a, out int b) => (a, b) = (0, 1);
}
```

この型に対して以下のようなコードを書いた場合、

```csharp
public bool Deconstruct(X x) => x is (1, 2);
```

以下のようなコードと同じような挙動をします。

```csharp
// コンパイル時に Deconstruct メソッドが見つかる場合はそれを使って分解。
public bool Deconstruct(X x)
{
    x.Deconstruct(out var a, out var b);
    return a == 1 && b == 2;
}
```

分解代入や分解変数宣言とは違って、位置パターンの場合はコンパイル時に `Deconstruct` メソッドが見つからない場合があります。
この場合、`ITuple`インターフェイス(`System.Runtime.CompilerServices`名前空間)を使って分解を試みます。
例えば以下のように`object`で値を渡すコードを書いた場合、

```csharp
public bool Object(object x) => x is (1, 2);
```

以下のようなコードと同じような挙動をします。

```csharp
// コンパイル時の解決ができない場合、ITuple を実装しているかどうかを見る。
// Length とインデクサーを使ってマッチング。
public bool Object(object x)
{
    return x is ITuple t
        && t.Length == 2
        && t[0] is int a && a == 1
        && t[1] is int b && b == 1
        ;
}
```

#### <a id="sec-generated-title-15"></a> <a id="tuple-switch"></a>タプル switch

位置パターンに伴って、`switch`ステートメントの `()` の中に、複数の値を `,` 区切りで書けるようになりました。

```csharp
int Compare(int? a, int? b)
{
    switch (a, b)
    {
        case (null, null): return 0;
        case (int _, null): return -1;
        case (null, int _): return -1;
        case (int a1, int b1): return a1.CompareTo(b1);
    }
}
```

このコードは、まず `(a, b)` というタプルを作って、それを `switch` ステートメントに掛ける挙動になります。`case` の後ろに書かれているのは位置パターンです。

要するに、意味としては `switch ((a, b))` と書くのと同じです。
なので実体としては「複数の値に対する`switch`」というより、「タプルに限り、`()` を一段省略できる」という機能です。

#### <a id="sec-generated-title-16"></a> <a id="zero-or-one"></a>0、1要素の分解

タプル構築や分解代入・分解宣言では0、1要素のもの( `()` や `(x)`) は認められていませんが、
位置パターンでは認められるようになりました。
それぞれ、0、1引数の`Deconstruct`メソッドが調べられます。

```csharp
using System;
 
class X
{
    public void Deconstruct() { }
    public void Deconstruct(out int a) => a = 0;
}
 
class Program
{
    static void Main() => M(new X());
 
    static void M(X x)
    {
        // 0 引数の位置パターン。
        // Deconstruct() を持っていることが使える条件。
        if (x is ()) Console.WriteLine("Deconstruct()");
 
        // 1 引数の位置パターン。
        // Deconstruct(out T) を持っていることが使える条件。
        // ただ、キャストの () との区別が難しいらしく、素直に x is (int a) とは書けない。
        // 前後に余計な var や _ を付ける必要あり。
        if (x is (int a) _) Console.WriteLine($"Deconstruct({a})");
    }
}
```

0引数のものは単に `()` で OK です。

一方で、1引数のものは、キャストの `()` との区別が難しいそうで、
素直に `(constant)` とか `(T variable)` とかは書けません。
`var (subpattern)` とか `(subpattern) _` とか、前後に余計なものを付けることでキャストと区別します。

### <a id="sec-generated-title-17"></a> <a id="remove-deconstruct"></a>最適化での Deconstruct 削除

位置パターンでは、コンパイラーの最適化によって `Deconstruct` メソッドの呼び出しが消えることがあります。
以下のように、すべて `_` で値を破棄してしまう場合には `Deconstruct` メソッドを呼び出す必要がなく、
実際、呼び出しが消えてなくなります。

```csharp
using System;
 
class X
{
    // Deconstruct に副作用を持たせる
    public void Deconstruct() => Console.WriteLine("Deconstruct()");
    public void Deconstruct(out int a)
    {
        Console.WriteLine("Deconstruct(out int a)");
        a = 0;
    }
    public void Deconstruct(out int a, out int b)
    {
        Console.WriteLine("Deconstruct(out int a, out int b)");
        (a, b) = (0, 0);
    }
}
 
class Program
{
    static void Main()
    {
        var x = new X();
 
        // Deconstruct() がないとコンパイル エラーになるけど、
        // Deconstruct() は呼ばれない。
        Console.WriteLine(x is ());
 
        // Deconstruct(out int) がないとコンパイル エラーになるけど、
        // Deconstruct(out int) は呼ばれない。
        Console.WriteLine(x is var (_));
 
        // Deconstruct(out int, out int) がないとコンパイル エラーになるけど、
        // Deconstruct(out int, out int) は呼ばれない。
        Console.WriteLine(x is (_, _));
    }
}
```

また、引数の数が同じ位置パターンをいくつか並べた際にも、`Deconstruct` メソッドの呼び出しは1回にまとめられます。

```csharp
class X
{
    public int Value { get; }
    public X(int value) => Value = value;
    public void Deconstruct(out int value) => value = Value;
}
 
class Program
{
    static int M(X x)
        => x switch
    {
        // 引数の数が同じ位置パターンを3回。
        // この場合、Deconstruct(out int) の呼び出しは1回にまとめられる。
        (0) _ => 1,
        (1) _ => 2,
        (2) _ => 0,
        _ => x.Value
    };
}
```

ちなみに、仕様上は「必ず消える」という保証もないです(「消えることがある」という仕様)。
なので、`Deconstruct` メソッドは副作用を起こさないように作ることが推奨されます。

### <a id="sec-generated-title-18"></a> <a id="property"></a>プロパティ パターン

プロパティ パターン(property pattern)は、プロパティに対して再帰的なマッチングを行うパターンです。
(プロパティ パターンという名前に反して、フィールドも使えます。)

書き方は、`{ PropertyName: SubPattern, ... }` というように、
プロパティ名と、そのプロパティに対して掛けたいパターンを `:` でつなぎます。
複数のプロパティに対して使う場合はそれぞれを `,` で区切ります。
位置パターンとは違って、名前の省略はできません。

再び `Point` クラス(`int` 型の2つのプロパティ `X`、`Y` を持つ)を例に挙げます。
以下のような書き方ができます。

```csharp
static int M(Point p)
    => p switch
{
    { X: 1, Y: 2 } => 0,
    { X: var x, Y: _ } when x > 0 => x,
    _ => -1
};
```

このコードは概ね以下のような意味になります。

```csharp
var x = p.X;
var y = p.Y;
if (x is 1 && y is 2) return 0;
if (x > 0) return x;
return -1;
```

位置パターンと同様、型の明示もできます。

```csharp
static int M(object obj)
    => obj switch
{
    int i => i,
    string s => s.Length,
    Point { X: 0, Y: 0 } => 0,
    Point (_, _) => 1,
    _ => -1
};
```

ちなみに、プロパティ パターンと言いつつ、フィールドも参照できます。

```csharp
using System;
 
class X
{
    // (外から見て) get-only なプロパティ
    public int GetOnly { get; private set; }
 
    // get/set 可能なプロパティ
    public int GetSet { get; set; }
 
    // フィールド
    public int Field;
 
    // set-only なプロパティ
    public int SetOnly { set => GetOnly = value; }
}
 
class Program
{
    public static void Main()
    {
        // オブジェクト初期化子では、set が public なプロパティか readonly ではないフィールドを指定可能
        var x = new X { GetSet = 1, Field = 2, SetOnly = 3 };
 
        // プロパティ パターンでは、get が public なプロパティかフィールドを指定可能
        Console.WriteLine(x is { GetOnly: 3, GetSet: 1, Field: 2 });
    }
}
```

#### <a id="sec-generated-title-19"></a> <a id="initializer-vs-property"></a>オブジェクト初期化子の逆

「位置パターンはコンストラクター呼び出しの逆」という話をしましたが、
同様に、プロパティ パターンは[オブジェクト初期化子](../oop/oo_construct.md#member_initializer)と対になるものです。

```csharp
// 初期化子でプロパティ指定できるんなら、プロパティ指定でマッチングできるべき
var p1 = new Point { X = 1, Y = 2 };
var r1 = p1 is { X: 1, Y: 2 };
 
// 混在で構築できるんなら、混在でマッチングできるべき
var p2 = new Point(x: 1) { Y = 2 };
var r2 = p2 is (1, _) { Y: 2 };
```

ただ、`=` は代入の意味なのでパターンとしては使えず、代わりに `:` になっています。
`:` を使っているのは、位置パターンと構文を共通化できて実装が楽だからだそうです。

#### <a id="sec-generated-title-20"></a> <a id="no-order"></a>位置パターンとプロパティ パターンの順序

位置パターンとプロパティ パターンを混在して使う場合、
`Deconstruct`メソッドとプロパティのアクセサーの呼び出し順序には<em>保証がない</em>そうです。

残念ながら、以下のようなコードには動作保証がないそうです。

```csharp
using System;
 
enum Type { A, B }
 
class X
{
    public Type Type { get; }
    public X(Type type) => Type = type;
 
    // それぞれ Type が一致しているときだけ値を取り出せ、そうでなければ例外
    public int A => Type == Type.A ? 1 : throw new InvalidOperationException();
    public int B => Type == Type.B ? 2 : throw new InvalidOperationException();
 
    // 分解でタイプ判定
    public void Deconstruct(out Type t) => t = Type;
}
 
class Program
{
    static void Main()
    {
        Console.WriteLine(M(new X(Type.A)));
        Console.WriteLine(M(new X(Type.B)));
    }
 
    // 以下のコードはたまたま動く可能性はあるものの、C# の言語使用としては保証がない。
    // Deconstruct よりも先にプロパティのアクセスがあると例外が出ることがある。
    static int M(X x) => x switch
    {
        (Type.A) { A: var a } => a,
        (Type.B) { B: var b } => b,
        _ => 0
    };
}
```

#### <a id="sec-generated-title-21"></a> <a id="non-null"></a>非 null マッチング

プロパティ パターンは、暗黙的にnullチェックが挟まって、非 null であることが保証されます。
しかも、`x is { }` というように、中身が空っぽであっても null チェックだけは挿入されるので、 `x is { }`を「`x`はnullではない」の意味で使えます。

C# 7.0 までのパターンだと、null チェックを楽に書く手段がなかったです。

```csharp
struct LongLongNamedStruct { }
 
void M1(LongLongNamedStruct? x)
{
    // こういう書き方だと null チェックになる。
    if (x is LongLongNamedStruct nonNull)
    {
        // obj が null じゃない時だけここが実行される。
        // でも、x の型が既知なのに、長いクラス名をわざわざ書くのはしんどい…
    }
}
 
void M2(LongLongNamedStruct? x)
{
    // が、var パターンは null にもマッチしちゃう。
    // (var は「何にでもマッチ」。null でも true になっちゃう。)
    if (x is var nullable)
    {
        // obj が null でもここが実行される。
    }
}
```

単に null チェックだけなら `!(x is null)` とか `x.HasValue` だけでいいんですけども、 値を使いたければその後ろで `var nonNull = x.GetValueOrDefault();` とかが必要で、何を使うにしても微妙に長くなりがちでした。

そこで先ほどの `x is { }` を使います。
以下のような書き方で、null 許容型の null チェックをしつつ、値を変数に受け取れます。

```csharp
void M3(LongLongNamedStruct? x)
{
    // (C# 8.0) プロパティ パターンであれば、null チェックを含む。
    if (x is { } nonNull)
    {
        // obj が null じゃない時だけここが実行される。
    }
}
```

#### <a id="sec-generated-title-22"></a> <a id="sub-pattern-name"></a>プロパティ パターンの拡張(入れ子のメンバー参照)

<h5 class="version version10">Ver. 10</h5>

C# 10.0 で、以下のように、入れ子のプロパティ・フィールド参照でプロパティ パターンを書けるようになりました。

```csharp
m(null);
m(new X { Name = "" });
m(new X { Name = "a" });
m(new X { Name = "abc" });

static void m(X? x)
{
    if (x is { Name.Length: 1 })
    {
        Console.WriteLine("single-char Name");
    }
}

class X
{
    public string? Name { get; set; }
}
```

この例でいう `{ Name.Length: 1 }` の部分は、`{ Name: { Length: 1 } }` と全く同じ意味になります。

ここで注意点というか、1つ、一瞬迷いそうな点として、
`Name.Length` と言う書き方でも `Name` の null チェックを含んでいます。
`{ Name: { Length: 1 } }` をさらに展開すると、以下のようなコードとほぼ同じ意味になります。

```csharp
    if (x is not null)
    {
        var name = x.Name;
        if (name is not null)
        {
            var length = name.Length;
            if (length == 1)
            {
                Console.WriteLine("single-char Name");
            }
        }
    }
```

### <a id="sec-generated-title-23"></a> <a id="list">リスト パターン</a>

<h5 class="version version11">Ver. 11</h5>

C# 11で、`[]` を使ってリスト(配列や `List<T>` など)に対するパターン マッチングができるようになりました。
例えば以下のような `switch` を書けます。

```csharp
var array = new[] { 1, 2 };

Console.WriteLine(array switch
{
    [] => "空の配列",
    [1] => "長さ1で、1要素目が1",
    [_] => "長さ1の配列",
    [1, 2] => "長さ2で、1要素目が1、2要素目が2",
    [1, _] => "長さ2で、1要素目が1",
});
```

このような `[]` を使ったパターンを<strong id="key-list-pattern" class="keyword">リスト パターン</strong>(list pattern)と言います。

#### <a id="sec-generated-title-24"></a> <a id="square-bracket">注意: 角カッコ</a>

C# で新文法を追加する際には、既存の文法と比べて違和感がないような選択をすることが多いです。

そういう意味ではリスト パターンの `[]` は珍しくちょっと見慣れない感じの選択でした。
これまで `[]` を使う文法というと、配列作成の `new T[N]` か、インデクサーの `x[i]` な分けですが、
これらはの場合 `[]` の内側には「個数」や「何番目か」の数値が入ります。
リスト パターンの `[]` の中に入るのは「要素に対するパターン」で、ちょっと方針が異なります。

初期案では、配列初期化子 `new[] { a, b, c }` からの類推ができるよう、リスト パターンには `{}` を使おうかという話もありました。
ただ、`is {}` だと[プロパティ パターン](#property)との弁別が難しかったようです。

これに対して、(C# 11 では入らなかったんですが、将来) 「コレクション リテラル」みたいな文法で `[]` を使う事を考えたりもしているようです。

```csharp
// (C# 11 時点で提案段階)
using System.Collections.Immutable;

int[] array = [ 1, 2 ];
Span<int> span = [ 1, 2 ];
ReadOnlySpan<int> ros = [ 1, 2 ];
List<int> list = [ 1, 2 ];
ImmutableArray<int> immutable = [1, 2];
```

これが入れば、初期化・生成側と、パターン マッチ・分解側の間の違和感が緩和されるかと思います。

#### <a id="sec-generated-title-25"></a> <a id="slice-pattern">.. (スライス パターン)</a>

パターンに対して `[a, b]` と書く場合、2要素ピッタリのリスト出ないとマッチしません。

```csharp
var array = new[] { 1, 2 };

Console.WriteLine(array is [1, 2]); // true
Console.WriteLine(array is [1]);    // false。部分一致ではダメ。
```

部分一致させたい場合、余る部分に `..` を置けばマッチさせることができます。
例えば、以下のようなコードで、「1, 2 で始まって、長さ2以上のリスト」にマッチできます。

```csharp
var array = new[] { 1, 2 };

match(new[] { 1 }); // false
match(new[] { 1, 2 }); // true (ちょうどでもOK)
match(new[] { 1, 2, 3 }); // true (過剰でもOK)
match(new[] { 1, 2, 3, 4, 5 }); // true

static void match(int[] array)
    => Console.WriteLine(array is [1, 2, ..]);
```

このような `..` を<strong id="key-slice-pattern" class="keyword">スライス パターン</strong>(slice pattern)と言います。

ちなみに、スライス パターンはリスト パターンの `[]` の内側にだけ書けます。
例えば `array is ..` みたいな書き方は認められていません。

`..` は先頭や中間にも書けます。

```csharp
var a1 = new[] { 1, 2 };
var a2 = new[] { 1, 2, 2 };
var a3 = new[] { 1, 2, 1, 2 };

// 1で始まって2で終わる(長さは任意)。
Console.WriteLine(a1 is [1, .., 2]); // true
Console.WriteLine(a2 is [1, .., 2]); // true
Console.WriteLine(a3 is [1, .., 2]); // true

// 末尾が 1, 2で終わる(長さは任意)。
Console.WriteLine(a1 is [.., 1, 2]); // true
Console.WriteLine(a2 is [.., 1, 2]); // false
Console.WriteLine(a3 is [.., 1, 2]); // true
```

ちなみに、2か所以上に `..` を置いてしまうとコンパイル エラーになります。

```csharp
var array = new[] { 1, 2 };

Console.WriteLine(array is [.., ..]);
```

#### <a id="sec-generated-title-26"></a> <a id="sub-pattern">リスト パターンの再帰</a>

[リスト パターン](#list)はカテゴライズするなら[再帰パターン](#recursive)の一種です。
`[]` の中の各要素には任意のパターンを書くことができます。

```csharp
using System.Numerics;

static bool match1(int[] array)
    => array is [0, _, > 0, < 0, var x, ..] && (x % 2) == 1;
// 前から順に、
// 0 だけにマッチ(定数パターン)
// 任意 (破棄パターン)
// 0 より大きい(関係演算パターン)
// 0 より小さい(関係演算パターン)
// 任意 (var パターン)
// 残り読み飛ばし (スライス パターン)

static bool match2((int x, int y)[] points)
    => points is [(1, 2), (x: 3, y: 4), { x: 5, y: 6 }];
// 前から順に
// 位置パターン
// 位置パターン(名前付き)
// プロパティ パターン
```

また、スライス パターンも、`..` の後ろに続けてパターンを書くことができます。

```csharp
static bool match1(ReadOnlySpan<int> span) => span switch
{
    [> 0, .. var rest] => match1(rest), // 先頭が正の数で、残りを再帰的に判定
    [] => true,
    _ => false,
};

static bool match2(int[] array)
    => array is [1, ..[2, 3]]; // あまり意味はなくて、[1, 2, 3] と同じ結果にしかならない
```

よく使いそうな例でいうと、「先頭数バイトが特定のパターンの時に読み飛ばし」みたいなことができます。

```csharp
var utf8 = File.ReadAllBytes("a.txt");

foreach (var b in removeBom(utf8))
{
    Console.WriteLine($"{b:X}");
}

static ReadOnlySpan<byte> removeBom(ReadOnlySpan<byte> utf8)
    => utf8 is [0xEF, 0xBB, 0xBF, .. var noBom] ? noBom : utf8;
```

#### <a id="sec-generated-title-27"></a> <a id="list-pattern-lowering">リスト パターンの展開結果</a>

リスト パターンやスライス パターンは、
割かしべたに長さ (`Length` もしくは `Count` プロパティ)、インデックス (`a[i]`) やスライス (`a[..]`) に展開されます。
例えば以下のようなリスト パターンを書いた場合、

```csharp
Console.WriteLine(palindrome(new int[0]));              // true
Console.WriteLine(palindrome(new[] { 1 }));             // true
Console.WriteLine(palindrome(new[] { 1, 2 }));          // false
Console.WriteLine(palindrome(new[] { 1, 2, 2 }));       // false
Console.WriteLine(palindrome(new[] { 1, 2, 1 }));       // true
Console.WriteLine(palindrome(new[] { 1, 2, 1, 2, 1 })); // true
Console.WriteLine(palindrome(new[] { 1, 2, 1, 2, 2 })); // false

static bool palindrome(ReadOnlySpan<int> list) => list switch
{
    [] or [_] => true,
    [var first, .. var rest, var last] => first == last && palindrome(rest),
};
```

以下のようなコードとほぼ同じ意味になります。

```csharp
static bool palindrome(ReadOnlySpan<int> list) => list.Length switch
{
    0 or 1 => true,
    >= 2 => list[0] == list[^1] && palindrome(list[1..^1]),
};
```

`a[^i]` や `a[i..j]` が使えることが、そのままリスト パターンを使える条件になります。
(詳しい条件に付いては「[インデックス/範囲](../data/dataranges.md)」を参照。)

また、`list is [_, .. var rest, _]` みたいなものが `list[1..^1]` に展開される都合上、
`list[i..j]` がパフォーマンス的にいまいちなコードになっている場合、
リスト パターンも非効率になります。

```csharp
static void m1(int[] array)
{
    // 配列に対するスライスは新しい配列を作っちゃう(= 遅い)。
    var slice = array[1..^1];

    // その影響で、以下のコードも新しい配列がいちいち作られて遅い。
    // (string でも同じようなことが起きる)。
    Console.WriteLine(array is [_, ..var rest, _]);
}

static void m2(ReadOnlySpan<int> span)
{
    // Span の場合はそんな非効率な事は起きないので、
    var slice = span[1..^1];

    // 以下のコードも遅くはならない。
    // (string に対しては ReadOnlySpan<char> にすると速い)。
    Console.WriteLine(span is [_, .. var rest, _]);
}
```

### <a id="sec-generated-title-28"></a> <a id="usage"></a>再帰パターンの利用例

「[型スイッチの用途](typeswitch.md#usage)」と同じ題材で、再帰パターンの利用例も挙げておきます。

使った題材は、数式を扱うようなクラスです。
要するに、例えば、「<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mi>x</mi><mo>+</mo><mn>1</mn></math>」というような式を、以下のようなクラスで表します。

```csharp
public abstract class Node
{
    public static readonly Node X = new Var();
    public static implicit operator Node(int value) => new Const(value);
    public static Node operator +(Node left, Node right) => new Add(left, right);
    public static Node operator *(Node left, Node right) => new Mul(left, right);
}
 
public class Var : Node { public override string ToString() => "x"; }
 
public class Const : Node
{
    public int Value { get; }
    public Const(int value) { Value = value; }
    public void Deconstruct(out int value) => value = Value;
    public override string ToString() => Value.ToString();
}
 
public class Add : Node
{
    public Node Left { get; }
    public Node Right { get; }
    public Add(Node left, Node right) => (Left, Right) = (left, right);
    public void Deconstruct(out Node left, out Node right) => (left, right) = (Left, Right);
    public override string ToString() => $"({Left.ToString()} + {Right.ToString()})";
}
 
public class Mul : Node
{
    public Node Left { get; }
    public Node Right { get; }
    public Mul(Node left, Node right) => (Left, Right) = (left, right);
    public void Deconstruct(out Node left, out Node right) => (left, right) = (Left, Right);
    public override string ToString() => $"{Left.ToString()} * {Right.ToString()}";
}
```

こいつに対して「式の簡約化」をやってみます。
要は、
「<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>+</mo><mn>0</mn></math>を<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi></math>に、
<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mn>1</mn></math>を<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi></math>に、
<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mn>0</mn></math>を<math xmlns="http://www.w3.org/1998/Math/MathML"><mn>0</mn></math>に直す」みたいなやつ。

こういう処理は、`switch`式と位置パターンを使って以下のように書けます。
(コード全体: [Expressions/Program.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Data/Patterns/Expressions/Program.cs))

```csharp
public static Node Simplify(this Node n)
    => n switch
{
    Add (var l, var r) => (l.Simplify(), r.Simplify()) switch
    {
        // 0 を足しても変わらない
        (Const(0), var r1) => r1,
        (var l1, Const(0)) => l1,
        // 他
        (var l1, var r1) => new Add(l1, r1)
    },
    Mul (var l, var r) => (l.Simplify(), r.Simplify()) switch
    {
        // 0 を掛けたら 0
        (Const(0) c, _) => c,
        (_, Const(0) c) => c,
        // 1 を掛けても変わらない
        (Const(1), var r1) => r1,
        (var l1, Const(1)) => l1,
        // 他
        (var l1, var r1) => new Mul(l1, r1)
    },
    _ => n
};
```

C# 7.3 までだと、この処理は以下のように書くことになります。

```csharp
public static Node ClassicSimplify(this Node n)
{
    if (n is Add a)
    {
        var (l, r) = a;
        var l1 = l.Simplify();
        var r1 = r.Simplify();
 
        { if (l1 is Const c && c.Value == 0) return r1; }
        { if (r1 is Const c && c.Value == 0) return l1; }
        return new Add(l1, r1);
    }
    if (n is Mul m)
    {
        var (l, r) = m;
        var l1 = l.Simplify();
        var r1 = r.Simplify();
 
        {
            if (l1 is Const c)
            {
                if (c.Value == 0) return c;
                if (c.Value == 1) return r1;
            }
        }
        {
            if (r1 is Const c)
            {
                if (c.Value == 0) return c;
                if (c.Value == 1) return l1;
            }
        }
        return new Mul(l1, r1);
    }
    return n;
}
```

<!-- original-page-break -->

## <a id="sec-generated-title-29"></a> <a id="pattern-combintor"></a>パターンの組み合わせ

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 で `and` や `or` などのキーワードを使ってパターンの組み合わせ(pattern combinators)ができるようになりました。

- `and`: 論理積パターン (conjunctive patterns)。両辺に書いたパターンの両方にマッチすることを求める
- `or`: 論理和パターン (disjunctive patterns)。両辺に書いたパターンの少なくとも一方にマッチすることを求める
- `not`: 否定パターン (negated patterns)。後ろに書いたパターンの否定を取る
- `()`: 括弧付きパターン (parenthesized patterns)。`and`, `or` などの結合優先度を指定するためにパターンを `()` でくくる

### <a id="sec-generated-title-30"></a> <a id="and-pattern"></a>and パターン

2つのパターンを `and` キーワードでつなぐことで、両方のパターンにマッチしたときだけマッチした扱いになります。
(論理積パターン(conjunctive patterns)と言ったりもします。)

例えば、複数のインターフェイスをすべて実装しているかを判定するとかに使えます。

```csharp
int M(object x) => x switch
{
    // 2つのインターフェイスを両方実装している場合にマッチ。
    // この時、パターン中で宣言した a, b にはちゃんと両方「初期化済み」判定を受ける。
    IA a and IB b => a.A * b.B,
    _ => 0,
};
 
interface IA { int A { get; } }
interface IB { int B { get; } }
```

その他、後述する関係演算パターンと組み合わせて、「0～10まで」みたいな数値の範囲を表すことができます。

```csharp
int M(byte x) => x switch
{
    >= 0 and < 10 => 0,
    >= 10 and < 100 => 1,
    >= 100 => 2,
};
```

### <a id="sec-generated-title-31"></a> <a id="or-pattern"></a>or パターン

2つのパターンを `or` キーワードでつなぐことで、少なくともいずれか片方のパターンにマッチしたときにマッチした扱いになります。
(論理和パターン(disjunctive patterns)と言ったりもします。)

単純に複数の値にマッチさせたり、複数の型にマッチさせることができます。

```csharp
bool IsSmallPrime(int x) => x is 2 or 3 or 5 or 7;
 
bool IsTrue(bool? x) => x switch
{
    true => true,
    // _ (true 以外)と差はないものの、あり得る値を網羅していることがチェックできるという点で
    // true, false, null の3つの値を並べる意味はなくはない。
    false or null => false,
};
```

また、複数の型にマッチさせたりもできます。

```csharp
bool IsByte(object x) => x is byte or sbyte;
```

`and` と同様、後述する関係演算パターンとの組み合わせでも使えます。

```csharp
int Triangular(int x) => x switch
{
    < -1 or > 1 => 0,
    _ => 1 - Math.Abs(x),
};
```

#### <a id="sec-generated-title-32"></a> <a id="conditional-keyward-and-or"></a>文脈キーワードの and, or

C# のキーワード追加では恒例行事ですが、
既存コードをなるべく壊さないように、後付けな `and`、`or` などは[文脈キーワード](../appendix/ap_reserved.md#context)になっています。

例えば、あまり意味のあるコードではないものの以下のようなコードは有効な C# コードになります。

```csharp
// 水色の部分は型名の or, and。青色の部分はキーワードの or, and。
bool M(object x) => x is or or and and and;
 
class and { }
class or { }
```

### <a id="sec-generated-title-33"></a> <a id="not-pattern"></a>not パターン

パターンの前に `not` キーワードを置くことで、元のパターンの成否を反転させることができます。
(否定パターン(negated patterns)と言ったりもします。)

おそらく一番使い道があるのは `not null` だと思います。

```csharp
using System;
 
#nullable enable
 
void M(string? s)
{
    if (s is not null)
    {
        Console.WriteLine(s.Length);
    }
}
```

`string` 相手だと `x != null` と大差ないですが、[場合によってはパフォーマンスがよくなることもあります](../../../blog/2020/12/isnull/index.md)。
また、`!` の視認性があまりよくないので `!=` よりも `is not` の方を好む人もいるようです。

あと、いわゆる early return に使えます。
以下のように、特定条件を満たさないときに早々に `return` ステートメントで関数を抜けてしまうときに `not` パターンが使えます。

```csharp
using System;
 
void PositivePattern(object x)
{
    if (x is string s)
    {
        Console.WriteLine(s.Length);
    }
}

// ↑のメソッドを early return で書き直したもの。
void EarlyReturn(object x)
{
    // if の中に限り、not + 型パターンで変数宣言可能。
    if (x is not string s) return;
 
    // この場合、if 中(not string の時) には s が使えず、
    // その後ろ(string の時)でだけ s が使える。
 
    Console.WriteLine(s.Length);
}
```

### <a id="sec-generated-title-34"></a> <a id="parenthesized-patterns"></a>括弧付きパターン

`not`, `and`, `or` の結合順位は `!`, `&&`, `||` と同じで、`not` → `and` → `or` の順です。

例えば以下のような書き方をすると、`and` の結合が優先されます。

```csharp
bool IsAsciiLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
```

`&&` と `||` でもよくある話ですが、優先度がわかりにくくて読むときにつらかったりします。
また、`or` の方を優先したいことも当然あります。

そこで、パターンを `()` で囲んで結合優先度を明示することができるようになりました。
(括弧付きパターン(parenthesized patterns)と言ったりもします。)
先ほどの `IsAsciiLetter` の例は以下のようにも書けます。

```csharp
// () を付けて優先度を明示。
bool IsAsciiLetter(char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');
```

前述の「複数のインターフェイスをすべて実装しているかを判定」と「`not` パターンを使った early return」の組み合わせもできます。

```csharp
using System;
 
void M(object x)
{
    if (x is not (IA a and IB b)) return;
 
    // a, b ともに使える。
    Console.WriteLine(a.A * b.B);
}
 
interface IA { int A { get; } }
interface IB { int B { get; } }
```

## <a id="sec-generated-title-35"></a> <a id="relational-patterns"></a>関係演算パターン

<h5 class="version version9">Ver. 9.0</h5>

`<`, `<=`, `>`, `>=` の4つの関係演算子を使って数値の大小をパターンの中に書けます。
(関係演算パターン(relational patterns)と言ったりします。)

```csharp
int M(byte x) => x switch
{
    < 10 => 1, // 0～9
    >= 10 and <= 99 => 2, // 10～99
    > 99 => 3, // 100～255
};
```

初期の案では、C# 8.0 で[範囲アクセス用に `..` 演算子を導入](../cheatsheet/ap_ver8.md#range)したのに対して、「範囲パターン」も用意したいというものでした。
ただ、`x..y` みたいな範囲パターンだと、両端(この場合 `x`と`y`)を含むかどうかがわかりにくくて困るだろうということで不採用になっていました。
( `..` 演算子は[インデックス用途](../data/dataranges.md#index-usage)に絞ったことで、先頭`x`は含む、末尾`y`は含まないというルールにできましたが、「範囲パターン」の場合はあまり用途を絞れないので同じルールだと使いにくいという問題があります。)

他のプログラミング言語だと、範囲を表すために `<..`, `=..`, `..<`, `..=` など `..` の前後に `<` や `=` を付けることで両端の含む・含まない問題を解決していたりします。
しかし、C# ではもういっそ、`<`, `<=`, `>`, `>=` と `and` パターンの組み合わせで範囲を表そうということになりました。

<!-- original-page-break -->

## <a id="sec-generated-title-36"></a> <a id="compile-time-validation"></a>コンパイル時検査

パターン マッチングでは、値の網羅性を満たしているかどうかと、書いたパターンが重複していないかをコンパイル時に検査してくれる仕組みがあります。

### <a id="sec-generated-title-37"></a> <a id="exhaustive"></a>網羅性チェック

いくつかの型は決まった値しかとりません。例えば `bool` なら `true` か `false` の2値ですし、
`bool?` でも `true`, `false`, `null` の3値だけです。
`byte` も高々256個の値しか持ちません。
[型スイッチのページにも書いていますが](typeswitch.md#exhaustive)、パターン マッチングではこれらの値をすべて網羅しているかどうか(exhaustiveness: 網羅性)の検査をしてくれます。

```csharp
// 無警告
int A(bool x) => x switch
{
    true => 1,
    false => 0,
};
 
// 警告あり(CS8655: 条件に null が足りていない)
int B(bool? x) => x switch
{
    true => 1,
    false => 0,
};
 
// 無警告
int C(bool? x) => x switch
{
    true => 1,
    false or null => 0,
};
```

また、数値型に対しては、[関係演算パターン](#relational-patterns)を使って「100未満」と「100以上」というように相補的に値を網羅しているかを検査できます。
例えば以下のコードには条件漏れがあって警告を起こします。

```csharp
// 警告を起こす
int M(byte x) => x switch
{
    < 10 => 1,
    >= 10 and < 100 => 2,
    // < 100 と > 100 (どちらも 100 は含まない)しかないので、実は 100 が漏れてる
    > 100 => 3,
};
```

値パターンや `or` パターンとの組み合わせでも網羅性の検査がかかります。

```csharp
// 整数の場合は値パターンとかその or パターン、関係演算パターンの組み合わせでも網羅性検査がかかる
int M(uint x) => x switch
{
    0 or 2 or 4 or 6 or 8 => 0,
    1 or 3 or 5 or 7 or 9 => 1,
    >= 10 => -1, // この行がなかったり、条件が > 10 とかだったりすると警告
};
```

一般の型に対しても、「null か非 null か」みたいな条件が相補的で、これに対しても網羅性の検査がかかります。

```csharp
// null か非 null かで網羅性検査がかかっていて、どれか1行でも欠けていると警告
int M(int? x, int? y) => (x, y) switch
{
    (null, null) => 0,
    ({ }, null) => -1,
    (null, { }) => 1,
    ({ } x1, { } y1) => x1.CompareTo(y1),
};
```

### <a id="sec-generated-title-38"></a> <a id="case-duplicate"></a>条件の重複チェック

`switch` ステートメント/`switch` 式中に絶対に到達できない条件があるとき、
ある程度はコンパイル時に検知してコンパイル エラーにしてもらえます。

パターンを使った `switch` の条件は[上から逐次判定](typeswitch.md#sequential)なので、要するに、上の方に下にある条件の上位互換な条件があるとコンパイル エラーになります。

一番わかりやすいのは[破棄パターン](#discard)で、これは「何にでも一致するパターン」なので、その下に何かを書くとエラーになります。

```csharp
int M(object obj) => obj switch
{
    _ => 0,
    string _ => 1,
};
```

当然ですが、全く同じ条件が2つ以上ある場合にも、1つ目以外には絶対に到達しないのでエラーになります。

```csharp
int M(object obj) => obj switch
{
    string s => s.Length,
    string _ => 1,
};
```

ちなみに、[`when`句](typeswitch.md#switch)だと重複チェックが漏れることがあります。
一方、同じような条件でも、[再帰パターン](#recursive)を使うとチェックが働きやすいです。

```csharp
int M1(object obj) => obj switch
{
    // when 句を使うと「同じ条件」判定ができなくなる。コンパイルできてしまう。
    string s when s.Length == 0 => 0,
    string s when s.Length == 0 => 1,
    _ => -1,
};
 
int M2(object obj) => obj switch
{
    // 同じことを再帰パターンでやるとちゃんと重複チェックが掛かる。2つ目でコンパイル エラーに。
    string { Length: 0 } => 0,
    string { Length: 0 } => 1,
    _ => -1,
};
```

また、前節の[網羅性](#exhaustive)とも関連して、
全ての値を網羅済みのところの後ろに条件を足しても、その行には絶対に来ないのでエラーにできます。
例えば以下のコードはコンパイル エラーになります。

```csharp
int M(bool a, bool b) => (a, b) switch
{
    (false, false) => 0,
    (true, false) => 1,
    (false, true) => 2,
    (true, true) => 3,
    // bool の場合上記4つ以外は絶対にないことがわかるので、この行でコンパイル エラーになる。
    _ => 4,
};
```
