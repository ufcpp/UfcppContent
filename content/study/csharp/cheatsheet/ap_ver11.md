---
title: "C# 11.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver11/"
content_type: "Article"
published_at: "2022-05-04T00:00:00"
updated_at: "2022-09-22T00:00:00"
tags: []
umbraco_id: 2423
parent_id: 1174
sort_order: 16
aliases: []
---

# C# 11.0 の新機能

<div class="version version11">Ver. 11.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2022/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 7.0</li>
<li>Visual Studio 2022 17.4</li>
</td>
</tr>
</table>

執筆予定: [C# 11.0 トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/387)

## <a id="sec-generated-title-1"></a> <a id="utf8-literal"></a>UTF-8 リテラル

`"abc"u8` みたいに、文字列リテラルの後ろに u8 接尾辞を付けることで、UTF-8 な byte 列を文字列リテラルの形で書けるようになりました。

```csharp {title="u8 リテラルの例"}
ReadOnlySpan<byte> hex = "0123456789ABCDEF"u8;
```

以下のような byte 列とほぼ同じ意味になります。

```csharp {title="u8 リテラルの展開結果の例"}
ReadOnlySpan<byte> s = new byte[] { 97, 98, 99 };
```

詳しくは「[UTF-8 リテラル](../start/st_string.md#utf8-literal)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="raw-string">生文字列リテラル</a>

C# 11 で、3つ以上の連続した `"` を使うことで、「一切エスケープが必要ない文字列リテラル」を書けるようになりました。

```csharp {title="raw string literal"}
// """ から始まる文字列リテラル(raw string, 生文字列)。
var quote = """
    " はそのまま " として使われて、
    \ も \ のままの意味。
    \\ は \ が2個。
    {} とかも特別な解釈はされない。
    """;
```

この `"""` を使った書き方で、さらに文字列補間をすることもできます。

```csharp {title="$ を2個にすれば、{ 1個はエスケープなしで書ける"}
Console.WriteLine(format(123, "abc"));

static string format(int id, string name) => $$"""
    {
      "id": {{id /* ここは補間 */ }},
      "name": "{{name /* ここも補間 */}}"
    }
    """;
```

詳しくは「[生文字列リテラル](../start/st_string.md#raw-string)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="required">required メンバー</a>

プロパティとフィールドに対する `required` 修飾子というものが追加されました。
これを使うと、[オブジェクト初期化子](../oop/oo_construct.md#member_initializer)で何らかの値を代入することを義務付けられます。
例えば以下のようなコードを書いたとき、`a1` 以外の `new A` はエラーになります。
(警告ではなくエラーにします。)

```csharp {title="required 修飾子" highlight-ranges="sha256:b2ab16f5c01f49b3579d37408c360d5558ab23736f76afb1e0feede18550d81f;9:12-9:20,10:12-10:20"}
var a1 = new A { X = "abc", Y = 123 };

var a2 = new A { X = "abc" }; // Y を代入していないのでエラー。
var a3 = new A { Y = 123 };   // X を代入していないのでエラー。
var a4 = new A();             // X も Y も代入していないのでエラー。

class A
{
    public required string X { get; init; }
    public required int Y;
}
```

詳しくは「[required メンバー](../oop/oo_property.md#required)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="list">リスト パターン</a>

C# 11で、`[]` を使ってリスト(配列や `List<T>` など)に対するパターン マッチングができるようになりました。
例えば以下のような `switch` を書けます。

```csharp {title="リスト パターンの例"}
static ReadOnlySpan<byte> removeBom(ReadOnlySpan<byte> utf8)
    => utf8 is [0xEF, 0xBB, 0xBF, .. var noBom] ? noBom : utf8;

static bool palindrome(ReadOnlySpan<int> list) => list switch
{
    [] or [_] => true,
    [var first, .. var rest, var last] => first == last && palindrome(rest),
};
```

詳しくは「[リスト パターン](../datatype/patterns.md#list)」で説明します。

## <a id="sec-generated-title-5"></a> <a id="generic-math">Generic Math</a>

インターフェイスの静的メンバーを仮想・抽象にできるようになりました。

この機能の一番の用途は、数値型(`int` や `float` など)に対するアルゴリズムを[ジェネリクス](../oop/sp2_generics.md)を使って書けるようにすることです。
この最大用途にちなんで、
インターフェイスの静的メンバーなどを含む一連の機能を「generic math」と呼んだりしていました。
(コンセプト的な呼び名で、具体的に generic math という名前の文法やライブラリが追加されたわけではありません。)

generic math 関連で、数値型の演算子関連で3つ新機能が追加されています。

* [符号なし右シフト](#unsigned-right-shift)
* [checked 演算子オーバーロード](#checked-operator-overload)
* [シフト演算子の右オペランドの制限撤廃](#relaxing-shift)

### <a id="sec-generated-title-6"></a> <a id="static-abstract">インターフェイスの静的抽象メンバー</a>

まず、インターフェイスの静的メンバーについてですが、
例えば以下のようなコードが書けるようになりました。

```csharp {title="ジェネリックな Sum メソッド"}
using System.Numerics;

// よくある「和を取るコード」なものですら、これまでだとジェネリックに書く手段がなかった。
// C# 11 で可能に。
static T Sum<T>(IEnumerable<T> items)
    where T : INumber<T>
{
    var sum = T.Zero;
    foreach (var x in items) sum += x;
    return sum;
}

// いろんな型に対して sum<T> を呼ぶ。
Console.WriteLine(Sum(new byte[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(Sum(new int[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(Sum(new float[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(Sum(new double[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(Sum(new decimal[] { 1, 2, 3, 4, 5 }));
```

(詳しくは「[インターフェイスの静的抽象メンバー](../oop/oo_interface.md#static-abstract)」で説明します。)

### <a id="sec-generated-title-7"></a> <a id="unsigned-right-shift">符号なし右シフト</a>

符号付き整数(`int` とか `sbyte` とか)でも符号なし整数(`uint` とか `byte` とか)でも無関係に、
常に「符号なし右シフト(論理シフト)」をするための `>>>`演算子 (`>` の数が3つ)が追加されました。

```csharp {title="C# にも符号なし右シフト演算子を導入" highlight-ranges="sha256:6292cfbfd01ca537fe9b8029d5dd6dc3ffa82531dca0d931cdbb2d2886d575f4;16:10-16:13"}
using System.Numerics;

sbyte s = -1;

// ちゃんと符号なし右シフトに。
// FF → 7F → 3F → 1F → F → 7 → 3 → 1
for (int i = 0; i < 8; i++)
{
    Console.WriteLine($"{s:X}");
    s = LogicalRightShift(s, 1);
}

// >>> でどの型に対しても符号なし右シフト。
static T LogicalRightShift<T>(T s, int bits)
    where T : IShiftOperators<T,T>
    => s >>> bits;
```

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#unsigned-right-shift)」で説明します。

### <a id="sec-generated-title-8"></a> <a id="checked-operator-overload">checked 演算子オーバーロード</a>

`operator` キーワードの後ろに `checked` を付けることで、
「`checked` 演算子」を定義できるようになりました。
これにより、ユーザー定義の演算子オーバーロードでも `checked`(オーバーフロー時に例外を投げる)と `unchecked` (オーバーフローしても例外を投げない)を切り替えられるようになります。

```csharp {title="checked 演算子オーバーロードの例" highlight-text="checked"}
readonly struct Int2Bit
{
    public readonly byte Value;
    public Int2Bit(int value) => Value = (byte)(value & 0b11);
    public override string ToString() => Value.ToString();

    public static Int2Bit Checked(int value) => value is < 2 and >= 0 ? new(value) : throw new OverflowException();

    public static Int2Bit operator +(Int2Bit x, Int2Bit y) => new(x.Value + y.Value);
    public static Int2Bit operator checked +(Int2Bit x, Int2Bit y) => Checked(x.Value + y.Value);
}
```

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#checked-operator-overload)」で説明します。

### <a id="sec-generated-title-9"></a> <a id="relaxing-shift">シフト演算子の右オペランドの制限撤廃</a>

シフト演算子の右オペランドに `int` 以外の型を使えるようになりました。

```csharp {title="C# 11 で operator &lt;&lt;(A x, A y) とかが書けるように" highlight-text="A y"}
struct A
{
    // C# 10 以前でも書けるオーバーロード。
    public static A operator <<(A x, int y) => default;

    // C# 11 以降でだけ書けるオーバーロード。
    public static A operator <<(A x, A y) => default;
}
```

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#relaxing-shift)」で説明します。

## <a id="sec-generated-title-10"></a> <a id="file-local"></a>file ローカル型

`file` という修飾子を使って「書いたファイル内からだけアクセスできる型」を作れるようになりました。

```csharp {title="file 修飾付きの型を使う例" highlight-text="file"}
1.M();

file static class Extensions
{
    public static void M(this int x) => Console.WriteLine(x);
}
```

これと同じプロジェクト内の別のファイルに以下のようなコードを書いてもエラーにはなりません。

```csharp {title="別のファイルに同名の file 修飾付きの型を定義" highlight-ranges="sha256:bd3266b004cefb39a4c35ea1400844736d98b67545f72862bcc78688a36870d9;1:1-1:5"}
file static class Extensions
{
    public static void M(this int x) => Console.WriteLine("別ファイルの file-local Extensions");
}
```

詳しくは「[file ローカル型](../misc/file-local.md)」で説明します。

## <a id="sec-generated-title-11"></a> <a id="ref-field">ref フィールド</a>

[ref 構造体](../resource/refstruct.md#key-refstruct)のフィールドを [`ref` (参照渡し)](../resource/sp_ref.md#byref)で持てるようになりました。

ref フィールドの書き方は参照引数や参照戻り値と同じく、型の前に `ref` 修飾を付けます。

```csharp {title="ref フィールド"}
ref struct ByReference<T>
{
    public ref T Value;
}
```

詳しくは「[ref フィールド](../resource/refstruct.md#ref-field)」で説明します。

## <a id="sec-generated-title-12"></a> <a id="others">その他</a>

### <a id="sec-generated-title-13"></a> <a id="span">ReadOnlySpan に対するパターンマッチ</a>

C# 11 で、`ReadOnlySpan<char>` に対して[文字列リテラルによる定数パターン](../datatype/patterns.md#span)が使えるようになりました。

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

### <a id="sec-generated-title-14"></a> <a id="nameof-parameter"></a>nameof(引数) のスコープ変更

[`nameof`](../start/st_string.md#nameof-parameter) にちょっとだけ変更が掛かりました。
以下のように、メソッドに対する属性の中で、そのメソッドの引数の名前が参照できるようになりました。

```csharp {title="nameof(引数名)"}
using System.Diagnostics.CodeAnalysis;

// C# 10 までこの属性、 NotNullIfNotNull("x") と書かないといけなくて割かしつらかった。
[return: NotNullIfNotNull(nameof(x))]
static string? m(string? x) => x;
```

### <a id="sec-generated-title-15"></a> <a id="auto-default">構造体のフィールドの既定値初期化</a>

C# 11 では、構造体でもフィールドの明示的な初期化が不要になりました。
クラスと同じく、明示的に代入しなかったフィールド・自動プロパティには既定値が入ります。

```csharp {title="構造体のフィールドが自動的に 0 初期化されるように"}
struct Sample
{
    int _x;
    int _y;
    int _z;

    public Sample(int x, int y)
    {
        M(); // C# 11 では初期化よりも先に読んでも平気。_x, _y にもこの時点でいったん 0 が入ってる。

        _x = x;
        _y = y;
        // C# 11 では _z に 0 が自動で入る。
    }

    void M() => Console.WriteLine($"{_x}, {_y}, {_z}");
}
```

詳しくは「[構造体](../resource/rm_struct.md#auto-default)」や「[既定値](../resource/rm_default.md#auto-default)」で説明します。


### <a id="sec-generated-title-16"></a> <a id="generic-attribute">ジェネリックな属性</a>

[属性をジェネリック クラスにできるようになりました](../dynamic/sp_attribute.md#generic-attribute
)。

```csharp {title="C# 11 以降"}
// 属性クラスをジェネリックにできるように。
class TypeConverter<T> : Attribute { }

// <> で型引数を指定できる。
[TypeConverter<MyConverter>]
class MyClass { }
```

### <a id="sec-generated-title-17"></a> <a id="newline-in-interpolation">文字列補間中の改行</a>

[文字列補間](../start/st_string.md#string-interpolation)で、以下のようなコードが書けるようになりました
(`{}` の中で改行を入れれるようになりました)。

```csharp {title="{} の中の改行"}
var a = 1;
var b = 2;
var s = $"a: {
    a // ここで改行できるのは C# 11 から
    }, b: {b}";
```

ちなみに、以下のように、`$@` (文字列補間、かつ、逐語的文字列リテラル)を使う場合には C# 10.0 以前でも以下のようなコードが普通に書けました。

```csharp {title="$@ なら10以前でもOK"}
var a = 1;
var b = 2;
var s = $@"a: {
    a // $@ の場合は C# 10.0 以前でも OK
    }, b: {b}";
```

「`$""` の場合だけダメだった理由は今となっては思い出せない」というレベルだそうで、
仕様漏れ・バグ修正の類にギリギリの「新機能」になります。

### <a id="sec-generated-title-18"></a> <a id="numeric-intptr">Numeric IntPtr</a>

「C# の新機能」と言っていいのかどうか微妙なラインですが、
[`nint`](ap_ver9.md#nint) に関してちょっとした変更がありました。

C# 9.0 の頃には、`IntPtr`、`UIntPtr` 型に算術演算子の定義がなく、
`nint`、`nuint` に対する演算は C# コンパイラーが特別扱いすることで実装していました。
そのため、
「`nint`、`nuint` は内部的には `IntPtr`、`UIntPtr` としてコンパイルするけども、
`NativeInteger` 属性を付けて `nint`、`nuint` か `IntPtr`、`UIntPtr` を区別する」
みたいなことをしていました。

ところが、 .NET 7 (C# 11 と同世代)では、[generic math](#generic-math) 導入に伴って、
`IntPtr`、`UIntPtr` にも算術演算子が導入されました。
その結果、C# 9.0 時代のような「特別扱い」が不要になったそうです。
そこで C# 11 では、

* .NET 7 移行をターゲットにした場合、`NativeInteger` 属性を付けない
    * 正確に言うと、[`RuntimeFeature` クラス](../../../blog/2018/12/runtimefeature/index.md)を見て分岐
* `NativeInteger` 属性がなくても `nint`、`nuint` と同じ扱いをする

みたいな変更が掛かっています。

一応これが既存のコードに対する破壊的変更になる可能性があって、
例えば、以下のようなコードはこれまで例外が絶対に出なかったのが、C# 11 以降は例外が出る可能性があります。

```csharp {title="Numeric IntPtr 関連の破壊的変更"}
unsafe void M(void* x, int y)
{
    var p = checked((IntPtr)x); // unsigned → singed 変換扱い
    var z = checked(p + y);
}
```

### <a id="sec-generated-title-19"></a> <a id="cache-static-method-group">静的メソッドをデリゲート化するときのキャッシュ化</a>

[Numeric IntPtr](#numeric-intptr) の話以上に「C# の新機能と言っていいのかどうか微妙」な話(文法的には何も変わっていないし、挙動も大差ない)ですが、
`Func<int, int> f = Method;` みたいな書き方をしたときに、デリゲートのインスタンスをキャッシュするようになりました。

例えば以下のようなコードを考えます。

```csharp {title="ラムダ式と、メソッド グループからのデリゲート化の例"}
// この X と
int X(int[] data) => data.Sum(x => x * x);

// この Y、やってることは一緒。
int Y(int[] data) => data.Sum(square);
static int square(int x) => x * x;
```

C# 10 までは、おおむね以下のようなコードに展開されていました。

```csharp {title="C# 10 までの展開結果"}
// ラムダ式だと導入当初からキャッシュが効いてた。
Func<int, int>? _anonymous1 = null;

int X(int[] data)
{
    // こんな感じのコードに展開されてて、 new Func<int, int>() のアロケーションは1回限り。
    _anonymous1 ??= new Func<int, int>(x => x * x);
    return data.Sum(_anonymous1);
}

// ところが、メソッド グループを直接渡した場合、都度 new Func<int, int>() してた(C# 10 まで)。
int Y(int[] data)
{
    // おおむねこういうコードと同じ。
    var f = new Func<int, int>(square);
    return data.Sum(f);
}
static int square(int x) => x * x;
```

メソッド グループをデリゲート化するとき(`Y` の側)、常に `new Func<int, int>()` のコストがかかっていました。
これが、C# 11 からは以下のような感じのコードに展開されます。

```csharp {title="C# 11 からの展開結果"}
// C# 11 で、メソッド グループの場合でも、static なものはキャッシュするようになった。
Func<int, int>? _square = null;

int Y(int[] data)
{
    // この類のコードになった。ラムダ式の場合のものと一緒。
    _square ??= new Func<int, int>(square);
    return data.Sum(_square);
}
static int square(int x) => x * x;
```

### <a id="sec-generated-title-20"></a> <a id="CS9029">補足: required, scoped, file キーワードと型名</a>

これまでずっと、C# に新しいキーワードを足したいときには、文脈キーワード(特定の状況下でだけキーワード扱いを受ける)にしてきました。

C# 11 で追加される `required`, `scoped`, `file` の3つも文脈キーワードです。
ただ、これらのキーワードは型名と競合しやすい位置に書くことになるので、
型名として使えてしまうと文脈からの弁別が難しくなるようで、
型名として使えなくしたようです。
以下のようにコンパイル エラーになります。

```csharp {title="文脈キーワードな型名"}
// 古めの文脈キーワードはクラス名にしても警告にしかならない。
// 警告の出方も、古いやつは「小文字始まり ASCII のみの型名はやめて欲しい」の CS8981
class async { }
class await { }
class dynamic { }

// record に関しては専用の警告。CS8860。
// 今となっては、これもエラーでよかった説はある。
class record { }

// 最近の文脈キーワードはクラス名にするとエラーにするようにしたみたい。
class required { }
class scoped { }
class file { }

// ちなみに、この辺りのクラス名をあえて使いたいときは @ を付けとけば OK。
// (警告にもならない。)
class @required { }

// まあ、@ を付ければ、文脈によらない通常キーワードですら名前に使えるので。
class @class { }
```

### <a id="sec-generated-title-21"></a> <a id="pointer-of-managed-types">マネージ型のポインター</a>

C# 11 から、マネージ型のポインターを使えるようになりました。

```csharp {title="マネージ型のポインター型/アドレス取得"}
unsafe
{
    string s = "";
    Span<byte> x = stackalloc byte[4];

    // 以下のような型、アドレス取得はこれまではエラーになっていた。
    // (C# 11 以降も警告にはなる。多少の緩和があった。)
    string* ps = &s;
    Span<byte>* px = &x;
}
```

詳しくは「[unsafe](../interop/sp_unsafe.md#pointer-of-managed-types)」で説明します。
