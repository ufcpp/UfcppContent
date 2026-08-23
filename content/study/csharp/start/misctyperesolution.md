---
title: "[雑記] 型の決定"
source_url: "https://ufcpp.net/study/csharp/start/misctyperesolution/"
content_type: "Article"
published_at: "2019-12-08T00:00:00"
updated_at: "2021-01-02T00:00:00"
tags: []
umbraco_id: 2275
parent_id: 1190
sort_order: 18
aliases: []
---

# \[雑記\] 型の決定

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

C# の[変数](st_variable.md#variable)や[式](st_variable.md#expression)はそれぞれが「型」を持っています。
例えば `int x;` として宣言した変数 `x` は `int` 型になりますし、`"abc"` という式(文字列[リテラル](st_variable.md#literal)も式の一種)は `string` 型になります。

そして、代入(`=`)などの処理では、左右両辺の型が一致しないとコンパイル時にエラーを起こします。
例えば以下のコードはコンパイルできません。

```csharp {title="型の不一致でのコンパイル エラー" error-text="&quot;abc&quot;"}
int x = "abc";
```

どうせ左右で型を合わせる必要があるわけで、片方からもう片方の型を自動決定する構文もいくつかあります。
[ローカル変数の型推論(`var` 変数宣言)](sp3_inference.md#type-inference)が代表例で、
例えば以下のような書き方をすると、「右辺の型に合わせて `x` の型が自動的に `string` になる」という挙動になります。

```csharp {title="ローカル変数の型推論"}
var x = "abc";
```

逆に、反対側の辺を見ないと型が決定できないようなものもいくつかあります。
[デリゲート](../functional/sp_delegate.md)や[匿名関数](../functional/fun_localfunctions.md#anonymous-function)が代表例で、
例えば以下のコードは「型が決定できなくてコンパイル エラー」になります。

```csharp {title="左辺の型が必須な構文" error-ranges="sha256:727915d81048d1638424807fc8439278f386bf0d3a9785838daff9c510e36d3a;7:13-7:21,8:13-8:26"}
using System;
 
class Program
{
    static void Main()
    {
        var m = Main;
        var f = () => { };
 
        // 以下の書き方ならコンパイル可能。左辺の型が必須。
        Action m1 = Main;
        Action f1 = () => { };
    }
}
```

本項では、こういった「型の決定」について説明していきます。

## <a id="sec-generated-title-2"></a> <a id="source-target"></a>型決定の「向き」

型の決定には「向き」があります。
概要で話した通り、型決定の代表例は代入処理で、`=` 演算子の左右を指して「左辺」(left hand side)、「右辺」(right hand side)と言ったりします。
ただ、同様の型決定は、必ずしも「左右」になっていない構文でも発生します。
例えば、メソッド呼び出し([オーバーロード解決](../structured/miscoverloadresolution.md))の場合は「左右」というよりは「内外」といった方がいいかもしれません。

```csharp {title="型決定の「左右」、「内外」"}
using System;
 
class Program
{
    static void Main()
    {
        // 右辺の "abc" から左辺の s の型が string に決定。
        var s = "abc";
 
        // 内側の 1 から外側の X (の引数)の型が int に決定(X(int x) が呼ばれる)。
        X(1);
 
        // 左辺の Action から右辺の () => { } の型が決定。
        Action a = () => { };
 
        // 外側の Y (の引数の Action)から内側の () => { } の型が決定。
        Y(() => { });
    }
 
    static void X(int x) { }
    static void X(string x) { }
 
    static void Y(Action a) { }
}
```

どちらの場合でも、「値の出所」と「値を受け取る側」に分かれます。
そして、出所の方を<strong id="source-type" class="keyword">ソース型</strong>(source type)、受け取る側を<strong id="target-type" class="keyword">ターゲット型</strong>(target type)と言います。

| 型の「向き」 | 例 |
| --- | --- |
| ソース | 代入の右辺、メソッド呼び出しの実引数 |
| ターゲット | 代入の左辺、メソッドの仮引数 |

元々 C# ではソース型の方を明示的に指定して、ターゲット型の方を自動決定することが多いです。
なので、単に推論(inference)とか解決(resolution)という場合、この向き(ソース型からの決定)なことが多いです。

```csharp {title="ソース型からの型決定"}
using System;
 
class Program
{
    static void Main()
    {
        // 変数の型推論(type inference)はソース型からの型決定
        var s = "abc";
 
        // オーバーロード解決(overload resolution)はソース型からの型決定。
        X(1);
    }
 
    static void X(int x) { }
    static void X(string x) { }
 
    static void Y(Action a) { }
}
```

しかし徐々にターゲット型の方を明示的に指定する構文が増えています。
後入りな構文が多いせいか、こちらは「ターゲット型からの(target typed)」という形容をすることが多いです。
例えば C# 7.1 で入った [`default` 式](../resource/rm_default.md#default-expr)は「target typed default」などと呼ばれることがあります。
また、C# 8.0 で入った [`switch` 式はターゲットからの型決定](../datatype/typeswitch.md#target-typed)をしていますが、
こちらも「target typed switch」と言われたりします。

```csharp {title="ターゲット型からの型決定"}
static void M(bool b)
{
    // 以前の C# では default(DateTime) と書く必要があった。
    // C# 7.1 から、ターゲットからの型推論で default だけで書けるようになった。
    DateTime t = default;
 
    // C# の型決定機構では「1 と null の共通型が何かわからない」ということでコンパイルできなかった。
    // switch 式ではターゲット型(この場合 int?)を見て、switch の型を決めて、1 と null を受け付けできるようにした。
    int? x = b switch
    {
        true => 1,
        false => null
    };
}
```

##### <a id="sec-generated-title-3"></a> <a id="source-typed"></a>ソース型からの決定

ソース型によって挙動が決まる構文として以下のようなものがあります。

- [ローカル変数の型推論](sp3_inference.md#implicit)
- [配列の暗黙的型付け](sp3_inference.md#impl_array)
- [オーバーロード解決](../structured/miscoverloadresolution.md)
  - 特に、[ジェネリック型引数の推論](../oop/sp2_generics.md#method)

##### <a id="sec-generated-title-4"></a> <a id="source-typed"></a>ターゲット型からの決定

ターゲット型によって挙動が決まる構文として以下のようなものがあります。

単に「ターゲットを見るまで型が確定しない」程度のものもあります。
暗黙的型変換の一種として考えることができます(未確定の型から確定した型への変換)。

- [整数リテラル](st_embeddedtype.md#intl)
- [null](../oop/oo_class.md#null)
- [`default` 式](../resource/rm_default.md#default-expr)
- [`new` 演算子](../oop/oo_construct.md#target-typed-new)
- [デリゲート](../functional/sp_delegate.md)

もう少し積極的にターゲット型の情報を使う構文もあります。

- [`switch` 式](../datatype/typeswitch.md#target-typed)
- [条件演算子](../structured/st_branch.md#terget-typed-conditional)

また、ターゲットの型によってまるっきり異なる挙動になるものもあります。

 - [文字列補間](st_string.md#string-interpolation)
  - [`IFormattable` 型に渡すとき](st_string.md#FormattableString)
 - [ラムダ式](../functional/sp3_lambda.md)
  - [`Expression` 型に渡すとき](../functional/sp3_lambda.md#expression)

ちなみに、組み合わせも行けます。
以下のように、`switch` 式中の条件演算子中の `new` みたいな入れ子になった状況でもターゲット型推論が働きます。

```csharp {title="ターゲット型推論の入れ子"}
using System;
 
// target-typed switch 式中の
// target-typed 条件演算子中の
// target-typed new 式。
TimeSpan X(int i, bool b) => i switch
{
    < 0 => b ? new(0) : new(1),
    0 => b ? new(2) : new(6),
    > 0 => b ? new(4) : new(5),
};
```

## <a id="sec-generated-title-5"></a> <a id="history"></a>自動型決定の歴史

ソース/ターゲットのいずれか一方だけの型を指定して他方を自動決定するというのは、
2000年代頃から増え始めたものです。
20世紀の(1990年代以前の)プログラミング言語では少数派でしたし、
C# でも、C# 2.0 や 3.0 から導入された構文が多いです。
例えば、C# 1.0 (2000年発表、2002年正式リリース)時代には以下のような書き方はできませんでした。

```csharp {title="C# 1.0 時代"}
static void Main()
{
    // C# 2.0 から。
    // C# 1.0 時代は Action m = new Action(Main); と書く必要あり。
    Action m = Main;
 
    // C# 3.0 から。
    // C# 1.0 時代は string s = "abc"; と書く必要あり。
    var s = "abc";
}
```

ただ、明確に型推論(type inference)という言葉が出始めたのは C# 3.0 の頃からですが、
それ以前でも、推論に類するものはありました。
例えばメソッドのオーバーロード解決は「ソース型からの型決定」に類するものですし、
整数リテラルや null などは実は「ターゲット型からの型決定」をしています
(正確に言うと「暗黙的型変換」なんですが、いずれにせよターゲット型が決まるまで解釈が確定しません)。

```csharp {title="整数リテラルの型決定" error-ranges="sha256:f3644c0841c905c1ea92bde78bbda4bb8ebb15cc7208f24ea51baa790f8fb10d;7:5-7:6,10:5-10:8,20:12-20:26"}
// byte リテラルや short リテラルは存在していなくて、「整数リテラル」の暗黙的型変換で代用している。
byte a = 1;  // この 1 は byte (に代入可能)
short b = 1; // この 1 は short (に代入可能)

// 変数だと int から byte や short への暗黙的変換は認められていない。コンパイル エラーに。
int i = 1;
a = i;
 
// ちゃんと精度チェックが入る。byte に代入できない大きさの整数リテラルはコンパイル エラーを起こす。
a = 256;

// ただし、var に対して使った時は int 扱い。
var c = 1; // c の型は int
 
// 配列初期化子はターゲット型を見ているのでこの書き方は OK。
byte[] d = { 1, 2 }; // この 1, 2 は byte 扱い
 
// でも、配列の型推論はソース型からの型決定なので NG。
// 右辺は int[] 扱いで、左辺の byte[] と型が合わなくてコンパイル エラーになる。
byte[] e = new[] { 1, 2 };
```

## <a id="sec-generated-title-6"></a> <a id="conflict"></a>自動型決定の競合

ソースから型決定する構文とターゲットから型決定する構文は、当然ですが両立はできません。
どちらもあいまいでは型決定できません。片方は明示的な型指定が必要になります。

```csharp {title="推論に推論は重ねられない" error-ranges="sha256:e93f90077f537cb40225a52c4bdfb8aafa342ba2b2411942ec499a9df596af51;12:9-12:10"}
class Program
{
    static void Main()
    {
        // OK。引数の側の型を明示。
        X(out int a);
 
        // OK。メソッドの側の型を明示。
        X<int>(out var b);
 
        // NG。(型引数の)推論と(ローカル変数の)推論が重なって型決定不可。
        X(out var c);
    }
 
    static void X<T>(out T x) => x = default;
}
```

ちなみに、型推論は「後から」競合を起こす原因になり得たりもします。
例えば以下のようなコードはコンパイルできるコードなんですが、

```csharp {title="有効な型推論"}
using System;
 
class Program
{
    static void M(DateTime? x) { }
 
    static void Main()
    {
        M(null);
        M(default);
        M(new());
    }
}
```

ここに1行、オーバーロードを増やすとどちらを呼ぶべきか決定できなくてコンパイル エラーになります。

```csharp {title="オーバーロードの追加が破壊的変更になり得る" error-ranges="sha256:ddf753605b881e4c907ee0b5f1108536b6aa45f27f143b8394d396c8b443d3c0;10:9-10:10,11:9-11:10,12:9-12:10"}
using System;
 
class Program
{
    static void M(int? x) { } // この行を追加
    static void M(DateTime? x) { }
 
    static void Main()
    {
        M(null);
        M(default);
        M(new());
    }
}
```

## <a id="sec-generated-title-7"></a> <a id="priority"></a>優先度付きのターゲットからの型決定

ターゲットの型を見て挙動が変わりはするものの、
未指定の場合の既定の挙動が決まっていて、ソース型からの型推論と競合しないものもあります。

整数リテラルなどがそうで、整数リテラルはターゲットの型によって挙動を変えますが、
型推論に対しては `int` 扱いになりますし、
オーバーロード解決では `int` が最優先になります。

```csharp {title="整数リテラルの既定の型は int"}
// ターゲットの型を見ている。
byte a = 1;  // この 1 は byte 扱い
short b = 1; // この 1 は short 扱い
 
// ターゲット型が決まっていない場合は int になる。
var c = 1; // 1 は int で、そこからの型推論で c の型は int

void f<T>(T x) { }
f(1); // 型引数の推論でも int 扱い
```

[文字列補間](st_string.md#string-interpolation)にも優先度があります。
文字列補間は `IFormattable` 型よりも `string` が優先です。

```csharp {title="文字列補間は IFormattable より string が優先"}
using System;
 
class Program
{
    static void M(int x)
    {
        // var に対して文字列補間を使うと string 扱い。
        var s = $"abc {x}";
 
        // M(string) が優先的に呼ばれる。
        M($"abc {x}");
 
        // M(IFormattable) の方を呼びたければキャストが必要。
        M((IFormattable)$"abc {x}");
    }
 
    static void M(string s) { }
    static void M(IFormattable s) { }
}
```

## <a id="sec-generated-title-8"></a> <a id="cost"></a>自動型決定のコスト

ソースとターゲットのどちらか片方から他方を決定できるといっても、
その推論が低コストなものと、意外と高コストなものがあったりします。

例えば、ローカル変数の型推論はほとんどコストがかからないそうです。
なんせ左辺と右辺が1対1ですし、元々「型が合うかどうか」の判定のために左右どちらにも明確な型を求めています。
単に片方を他方に伝搬させるだけなので低コストです。

一方、オーバーロード解決は結構高コストです。
多数の候補の中から1つを選ばないといけないので単純に検索コストがかかります。
例えば、以下のようなコードでは `Parse("")` で呼び出せるメソッドの候補が4つあります。

```csharp {title="複数の候補があるメソッド呼び出しの例"}
// この1行によって DateTime.Parse(string) がオーバーロード解決候補に入る。
using static System.DateTime;
 
class Base
{
    // 基底クラスにも同名のメソッド。
    public void Parse(string x) { }
}
 
class Derived : Base
{
    // Parse("") で呼び出せる候補が複数。
    public void Parse(object x) { }
    public static void Parse<T>(T x) { }
 
    void M()
    {
        Parse("abc");
    }
}
```

オーバーロードの数や引数の数が多くなれば多くなるほど複雑になることは容易に想像できるかと思います。
標準ライブラリ中にも多数のオーバーロードを持つメソッドは多く、容易に複雑化します。
実は、C# ソースコードのコンパイル時間のうち数割程度はオーバーロード解決が占めているといわれています。

```csharp {title="いかにも複雑そうなオーバーロード解決の例"}
// 以下の5つの Parse が候補に
// int Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
// int Parse(string s)
// int Parse(string s, NumberStyles style)
// int Parse(string s, NumberStyles style, IFormatProvider provider)
// int Parse(string s, IFormatProvider provider)
using static System.Int32;
 
// 以下の4つの Parse が候補に
// DateTime Parse(ReadOnlySpan<char> s, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)
// DateTime Parse(string s)
// DateTime Parse(string s, IFormatProvider provider)
// DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
using static System.DateTime;
 
class Program
{
    static void Main()
    {
        // 2引数で、第2引数が NumberStyles なものは1個しかないので、
        // int Parse(string, NumberStyles)
        Parse(null, System.Globalization.NumberStyles.HexNumber);
 
        // stackalloc によって第1引数の型が ReadOnlySpan<char> に決定
        // 第3引数が DateTimeStyles なので、
        // DateTime Parse(ReadOnlySpan<char>, IFormatProvider, DateTimeStyles)
        Parse(stackalloc char[0], null, System.Globalization.DateTimeStyles.None);
    }
}
```

ちなみに、C# は、ローカル変数に対しては型推論(`var` 変数宣言)を認めていますが、
メンバー(フィールド、プロパティやメソッド)の引数・戻り値に対しては認めていません。
これは、作法的な問題(メンバーの型は明示すべきという思想)もありますが、
簡単に高コストになりうるから認められないという問題もあるそうです。

```csharp {title="メンバーの型推論は認めない"}
// 以下、仮定的な構文。C# では認めていない(おそらく今後も認めない)。
 
// 再帰しているので当然型決定が不可能。
// この例はまだ単純なものの、「再帰の検知」も十分複雑になりえる。
static var a = b;
static var b = a;
 
// ただでさえ複雑なオーバーロード解決と組み合わせると悲惨なことに…
static (T, U) M<T, U>(T t, U u) => (t, u);
static short M(int x, string y) => 0;
static float M(double x, string y) => 0;
 
static var A() => M(B(), C());
static var B() => M(D(), E());
static var C() => M(F(), "");
static var D() => M(1, 1.2);
static var E() => M(1, "");
static var F() => M(1.2, new object());
```

## <a id="sec-generated-title-9"></a> <a id="nest"></a>入れ子

いくつかの構文では、多段に中身を追って型決定してくれます。
例えば、以下のように、多重のラムダ式からオーバーロード解決することができます。
(ただし制限あり。)

```csharp {title="多重ラムダ式からのオーバーロード解決"}
using System;
 
class Program
{
    static void Main()
    {
        // 非ジェネリックなオーバーロードなら、多段ラムダ式でも解決可能。
        // (ただし、この解決ができるのは C# 6.0 以降)
        M(() => () => 1);   // M(Func<Func<int>> x)
        M(() => () => 1.0); // M(Func<Func<double>> x)
 
        // ただ、ジェネリックなものについては無理。コンパイル エラーに。
        // M<string> は呼んでもらえない。
        M(() => () => "");
    }
 
    static void M(Func<Func<int>> x) { }
    static void M(Func<Func<double>> x) { }
    static void M<T>(Func<Func<T>> x) { }
}
```

暗黙の型変換の類は[タプル](../datatype/tuples.md)の中でも働きますし、この場合、タプルが入れ子になっていても大丈夫です。

```csharp {title="入れ子のタプル"}
// 整数リテラルはターゲット型を見て暗黙的に変換がかかる。
// たとえ入れ子のタプルになっていてもこの仕組みは働く。
(byte, (short, long)) t = (1, (2, 3));
 
// ちなみに、以下のコードだとコンパイル エラー。
// リテラル以外では、int から byte, short への変換は暗黙的にできない。
(int, (int, int)) i = (1, (2, 3));
t = i;
```

[target-typed な`switch` 式](../datatype/typeswitch.md#target-typed)も、入れ子になっていても平気です。

```csharp {title="入れ子の swtich 式"}
static byte? M(object obj) => obj switch
{
    string s => s.Length switch
    {
        0 => 1,    // 2重の switch の中でも byte に変換できる
        _ => null, // 同、null を byte? 扱いできる
    },
    byte i => i,
    _ => null,
};
```

一方で、条件演算子や配列の型推論、ジェネリック型引数などは、ターゲット型からの型推論に対応していなくて、
以下のコードはコンパイル エラーになります。
(ただし、条件演算子については C# 9.0 でターゲット型推論を導入する予定があります。)

```csharp {title="ターゲット型からの型推論を持っていない構文" error-ranges="sha256:7b990bd17fe7bc23c311a1213df87061bfa1e865b9be94f1135676685ddd3723;4:15-4:30,5:28-5:32,6:20-6:24"}
static void Fail()
{
    // 以下のいずれもコンパイル エラー。
    byte? a = true ? 1 : null;
    byte?[] b = new[] { 1, null };
    byte? c = M(1, null);
}
 
static void Success()
{
    // ターゲット型推論に頼らない書き方が求められる。
    // 以下の書き方ならソースからだけで型決定できる。
    byte? a = true ? (byte?)1 : null;
    byte?[] b = new[] { (byte?)1, null };
    byte? c = M((byte?)1, null);
}
 
static T M<T>(T x, T y) => x;
```

## <a id="sec-generated-title-10"></a> <a id="common-type"></a>共通型

`switch` 式や条件演算子など、いくつかの「枝」を持つ構文では、枝ごとの型の「共通の型」(common type)を探す作業を一応行います。
ただ、C# 8.0 時点では制約がきつく、「枝のうちいずれか1つ」しか選ばれません。

ちょっとわかりにくいと思うので具体例を挙げます。
まず、以下のようなクラスを用意します。

```csharp {title="型階層の例"}
class Base { }
class A : Base { }
class B : Base { }
```

このクラスと、あと、int を使って共通型を決定できるかどうかの例を示します。

```csharp {title="共通型の決定" error-ranges="sha256:986d3328318848c2e430333804e05c1ef6756247bbdb076e1191c161ca0331bb;2:11-2:32,5:11-5:23"}
// 型の候補は A, B。それぞれお互いには変換不可なので、共通型の決定不可。
var ng1 = x ? new A() : new B();
 
// 型の候補は int。null は int に変換不可なので、共通型の決定不可。
var ng2 = x ? 1 : null;
 
// 型の候補は Base, A。A から Base に変換可能なので、共通型は Base に決定。
var ok1 = x ? new Base() : new A();
 
// 型の候補は int?。null は int? に変換可能なので、共通型は int?。
var ok2 = x ? (int?)1 : null;
 
// 型の候補は int, int?。int は int? に変換可能なので、共通型は int?。
var ok3 = x ? 1 : default(int?);
```

`int` と null の共通型は `int?` だとわかりそうなものですが、少なくとも C# 8.0 ではそういう自動判定はしません。
枝のいずれか1つが `int?` でないと共通型判定できません。

ちなみに、「提案が出ている」という程度の状態で実現するかはわかりませんが、
値型 `T`と null が並んでいた場合、共通の型として `T?` を選ぶようにするという案もあります。

また、クラスの場合も共通の基底クラス(`A` と `B` の場合 `Base`)を共通型として選ぶかどうかも検討されています。
こちらは「基底クラスに限る」(共通インターフェイスの場合は相変わらず)という条件付きです。
インターフェイスが絡むと以下のように多段派生があったり複雑なのでおそらく認められません。

```csharp {title="共通型の決定が難しい例"}
// 型 D と F の「共通型」といわれると何？
// インターフェイス J？ それともクラス A？
interface I { }
interface J { }
class A { }
class B : A, I { }
class C : A { }
class D : B, J { }
class E : B { }
class F : C, J { }
```
