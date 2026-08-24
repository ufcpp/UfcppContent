---
title: "【Generic Math】 C# 11 での演算子の新機能"
source_url: "https://ufcpp.net/study/csharp/oop/generic-math-operators/"
content_type: "Article"
published_at: "2022-06-06T00:00:00"
updated_at: "2022-11-25T00:03:03"
tags: []
umbraco_id: 2428
parent_id: 1248
sort_order: 21
aliases: []
---

# 【Generic Math】 C# 11 での演算子の新機能

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

<h5 class="version version11">Ver. 11</h5>

C# 11 で、数値型の演算子関連で3つ新機能が追加されています。

* [符号なし右シフト](#unsigned-right-shift)
* [checked 演算子オーバーロード](#checked-operator-overload)
* [シフト演算子の右オペランドの制限撤廃](#relaxing-shift)

## <a id="sec-generated-title-2"></a> <a id="generic-math">背景: Generic Math</a>

C# 11 / .NET 7 でインターフェイスの静的メンバーを仮想・抽象にできる (static abstract members in interfaces)ようになります。
(この機能自体については「[インターフェイスの静的抽象メンバー](oo_interface.md#static-abstract)」で説明しています。)

この機能の一番の用途は、数値型(`int` や `float` など)に対するアルゴリズムを[ジェネリクス](sp2_generics.md)を使って書けるようにすることです。
例えば、以下のようなコードが書けるようになりました。

```csharp {title="ジェネリックに「和を取る」コードを書けるように"}
using System.Numerics;

// よくある「和を取るコード」なものの、
// これまでだとジェネリックに書く手段がなかった。
// C# 11 で可能に。
// (T.Zero や、T に対する + 演算子の定義ができるように)
static T sum<T>(IEnumerable<T> items)
    where T : INumber<T>
{
    var sum = T.Zero;
    foreach (var x in items) sum += x;
    return sum;
}

// いろんな型に対して sum<T> を呼ぶ。
Console.WriteLine(sum(new byte[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(sum(new int[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(sum(new float[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(sum(new double[] { 1, 2, 3, 4, 5 }));
Console.WriteLine(sum(new decimal[] { 1, 2, 3, 4, 5 }));
```

加減乗除や論理演算はもちろん、`float` などの一部の型は `Math.Sin` などの数学関数も使えます。
コンセプト的に、この新機能を使ったジェネリックな数値処理の事を Generic Math と呼んでいたりします。

また、 .NET 5 以降、数値関連の型がいくつか追加されています。

* [`Half`](https://docs.microsoft.com/ja-jp/dotnet/api/system.half?WT.mc_id=DT-MVP-4028921): 16ビット浮動小数点数
* [`Int128`, `UInt128`](https://github.com/dotnet/runtime/issues/67151): 128ビットの整数
* [`CLong`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.clong?WT.mc_id=DT-MVP-4028921), [`CULong`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.culong?WT.mc_id=DT-MVP-4028921): C/C++ との相互運用のために使う、環境によってビット幅が違う整数
* [`nint`, `nuint`](../cheatsheet/ap_ver9.md#nint): CPU 依存幅の整数

これらの新しい数値型も、Generic Math の対象で、`INumber<T>` などのインターフェイスを実装しています。

この Generic Math と関連して、数値型の演算子関連で細々とした機能がいくつか追加されています。

* 符号なし右シフト
* checked 演算子オーバーロード
* シフト演算子の右オペランドの制限撤廃

## <a id="sec-generated-title-3"></a> <a id="unsigned-right-shift">符号なし右シフト</a>

右シフト演算には符号付き右シフト(算術シフト)と符号なし右シフト(論理シフト)があって、
右シフトしたときに、最上位ビットの 1 が残るかどうかの差になります。

C# の場合、基本的に、

* 符号<em>付き</em>整数の右シフトは符号<em>付き</em>右シフト(算術シフト)
* 符号<em>なし</em>整数の右シフトは符号<em>なし</em>右シフト(論理シフト)

という方式で右シフトの方式を切り替えます。

```csharp {title="右シフトの符号のありなし"}
// 符号なし (unsigned) の 0xFF = 255
byte u = 0xFF;

// 符号付き (signed) の 0xFF = -1
sbyte s = (sbyte)u;

// 符号なしを右シフトすると、左端には 0 が入る。
// FF → 7F → 3F → 1F → F → 7 → 3 → 1
for (int i = 0; i < 8; i++)
{
    Console.WriteLine($"{u:X}");
    u >>= 1;
}

// 符号なしを右シフトすると、左端のビットが残る。
// 元が FF だとずっと FF。
for (int i = 0; i < 8; i++)
{
    Console.WriteLine($"{s:X}");
    s >>= 1;
}
```

右シフトの符号あり/なしを切り替えたい場合、キャストが必要でした。

```csharp {title="byte にキャストしてから右シフトすることで論理シフトに"}
sbyte s = -1;

// LogicalRightShift を呼んでいるので、符号なし右シフトになる。
// FF → 7F → 3F → 1F → F → 7 → 3 → 1
for (int i = 0; i < 8; i++)
{
    Console.WriteLine($"{s:X}");
    s = LogicalRightShift(s, 1);
}

// 右シフトの符号のあり/なしを切り替えたい場合、キャストを挟む。
static sbyte LogicalRightShift(sbyte s, int bits)
    => (sbyte)((byte)s >> bits);
```

この方式は、Generic Math の導入に伴って1つ問題がありました。
「型引数 `T` に対応する符号なしな型」を取得する手段がありません。

```csharp {title="unsinged generic T を取る手段がない"}
// 符号なしシフトにしたかったらどうすれば？？？
static T LogicalRightShift<T>(T s, int bits)
    where T : IShiftOperators<T,T>
    => (T)((/* unsigned T を取得したいけど手段がない */)s >> bits);
```

そこで、C# 11 では普通に「符号なし右シフト演算子」の `>>>` (`>` 3つ)を導入することにしました。
(Java にあるやつです。Java の場合は `uint` などの符号なし整数型がなくて、`>>` か `>>>` で右シフトを切り替えます。)

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

ちなみに、演算子オーバーロードもできます。

```csharp {title="&gt;&gt;&gt; 演算子オーバーロードの例" highlight-ranges="sha256:f44dd9205a1bc2be49384b18ddef4048898d3be6a6ef24c20a418a7fc23de8ce;20:36-20:39"}
for (int i = 0; i < 4; i++)
{
    var x = new Int2Bit(i);

    Console.WriteLine($"for {x}");

    for (int j = 0; j <= 2; j++)
    {
        Console.WriteLine($"{j} bit signed: {x >> j}, unsigned: {x >>> j}");
    }
}

readonly struct Int2Bit
{
    public readonly byte Value;
    public Int2Bit(int value) => Value = (byte)(value & 0b11);
    public override string ToString() => Value.ToString();

    public static Int2Bit operator >>(Int2Bit x, int y) => new(x.Value >> y);
    public static Int2Bit operator >>>(Int2Bit x, int y) => new(ExtendSign(x.Value) >> y);
    private static int ExtendSign(int x) => x is >= 0b10 ? (-4 | x) : x;
}
```

## <a id="sec-generated-title-4"></a> <a id="checked-operator-overload">checked 演算子オーバーロード</a>

C# では、[整数演算のオーバーフロー時に何もしないか、それとも例外を投げるかを選べる機能](../start/sp_checked.md)があります。

* `checked` コンパイラー オプション: プログラム全域でオーバーフローを例外にする
* `checked` ブロック: ブロック中のオーバーフローを例外にする
* `checked` 式: `checked()` の `()` の中に書いた式でオーバーフローを例外にする

いずれにせよ `checked` というオプション名/キーワードを使います。
これが付いている状況を「`checked` コンテキスト」と言い、
`checked` コンテキストでの演算(要するに例外が出る演算)を 「`checked` 演算」と言います。

逆に、`unchecked` というキーワードで、
「例外を出さない」状態に戻せて、これを「`unchecked` コンテキスト」、「`unchecked` 演算」と言います。
(何も指定がない場合の既定動作は `unchecked` コンテキストになります。)

ちなみに、投げられる例外は `OverflowException` 型です。

```csharp {title="checked 演算の例"}
byte x = 128;
byte y = 128;

// unchecked 演算。
// (特にオプション指定がない場合、x + y はこの意味。)
// 128 + 128 = 256 なものの、オーバーフローして 0 に。
var z = unchecked(x + y);

// checked 演算。
// Overflow 例外が出る。
var w = checked(x + y);

Console.WriteLine((w, z));
```

C# 10 以前では、`checked` な演算ができるのは組み込み整数だけでした。
ユーザー定義で `int` などに準ずる型を作ろうとしても、`cheched`/`unchecked` の切り替えはできません。

「`int` などに準ずる型」をどのくらいの頻度で作るかと言われるとあまりなかったりはするんですが…
ちょうど最近(.NET 7 で)、`Int128`/`UInt128` という型が標準ライブラリに追加されています。

また、generic math でも `checked` を使えるようにしたいしたいです。

```csharp {title="generic に checked 演算をやりたい例"}
// 例外が出るべき。
CheckedAdd<byte>(128, 128);

static T CheckedAdd<T>(T x, T y)
    where T : struct, System.Numerics.IAdditionOperators<T, T, T>
{
    // 例外を出したい。
    return checked(x + y);
}
```

これまでのように、組み込み型でだけ例外を出せるということになってしまうと、

* generic に書き換える手段がなくなる
* 「今現在ライブラリ実装なもの(例えば `Int128`)が将来的に組み込み型になる」みたいなことをやりにくくなる

ということになります。

そこで、C# 11 ではユーザー定義の `checked` 演算子オーバーロードを書けるようにしました。
構文としては、

* `operator` キーワードの<em>後ろ</em>に `checked` を付ける
  * `checked` コンテキストで演算子を書いた時に呼ばれる
  * これを便宜上、「checked 演算子」と呼ぶ
* `unchecked` コンテキストで呼ばれて欲しい方には今まで通り何も付けない(`operator` だけ)
  * 「`checked` 演算子」との区別が必要な場合はわざわざ「普通の(regular)演算子」と呼ぶ

となります。

例えば、前節の符号なし右シフトでも使った「2ビット整数」を例に、とりあえず加算演算を書くなら以下のようになります。

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

`checked` 演算子を定義できるのは算術演算系の演算子だけです。
例えば `+` や `-` は `checked` にできますが、`&` や `!` はできません。

「`checked` だけでなく `unchecked` も明示的に書けるようにするかどうか」みたいなことも検討されたんですが、経験上「ほとんどの人が `unchecked` なコードしか書かない」という事がわかっているので、
「`checked` だけ追加して、何も書かない場合(regular)を `unchecked` 扱い」ということになっています。

### <a id="sec-generated-title-5"></a> <a id="checked-only">注意: checked 演算子のみの定義はエラー</a>

ちなみに、通常演算子なしで `checked` 演算子だけを定義することはできません。

```csharp {title="checked のみの定義はコンパイル エラーになる"}
struct A
{
    // OK: 通常演算子のみ
    public static A operator +(A x, A y) => default;

    // OK: 通常演算子、checked 演算子両方
    public static A operator -(A x, A y) => default;
    public static A operator checked -(A x, A y) => default;

    // コンパイル エラー: checked 演算子のみ
    // public static A operator *(A x, A y) => default; // この行もあれば OK。
    public static A operator checked *(A x, A y) => default;
}
```

### <a id="sec-generated-title-6"></a> <a id="checked-cast">注意: キャスト演算</a>

[キャスト](oo_operator.md#cast)も `checked` にできます。
ただし、`explicit` (明示的型変換)のみ OK で、`implicit` (暗黙的型変換)には `checked` は使えません。

```csharp
struct A
{
    // OK: explicit キャスト
    public static explicit operator A(int x) => default;
    public static explicit operator checked A(int x) => default;

    // OK: 通常演算子、checked 演算子両方
    public static implicit operator A(float x) => default; // これは大丈夫
    public static implicit operator checked A(float x) => default; // これはダメ
}
```

### <a id="sec-generated-title-7"></a> <a id="checked-responsibility">注意: あくまでユーザー裁量</a>

あくまでユーザー定義なので、悪意を持って実装すれば「通常演算子で例外を投げて、checked 演算子で投げない」みたいなこともできてしまいます。

```csharp {title="逆に実装"}
struct A
{
    // なぜかこっちが例外を出して
    public static explicit operator A(int x) => throw new OverflowException();

    // こっちが出さない実装をしても別に怒られない…
    public static explicit operator checked A(int x) => default;
}
```

そこの禁止まではしてないので注意してください。


### <a id="sec-generated-title-8"></a>コンパイル結果

(`>>>` のとこにも同様の話を)

通常演算子は `op_Addition` みたいな名前のメソッドになってる。

checked 演算子は `op_AdditionChecked` みたいに、通常演算子の後ろに `Checked` が付いた名前に

## <a id="sec-generated-title-9"></a> <a id="relaxing-shift">シフト演算子の右オペランドの制限撤廃</a>

C# ではこれまで、シフト演算子の右オペランド(何ビットシフトするかを決める方)には `int` しか使えないという制限がありました。
`<<` や `>>` という記号をシフト以外の意味で使わせるつもりはないのと、
であれば、シフトの右オペランドに `int` 以外のものを使いたい場面がほとんどないためです。

例えば、以下のコードはコンパイル エラーになります。
「1.1 ビットのシフト」とか言われても意味が解らないので、まあこれは妥当な制限でしょう。

```csharp {title="右オペランドが整数じゃないのでエラー"}
var x = 1 << 1.1;
```

ただ、以下のようなコードもコンパイル エラーになります。
右オペランドが `uint` や `long` の場合ですら制限されていて、
ちょっと厳しい感じがします。

```csharp {title="U や L がついてもダメ"}
var x = 1 << 1U;
var y = 1 << 1L;
```

必要かと言われると別に要らないので、厳しかろうと誰も文句は言わなかったんですが。

ところがここに来て、generic math が入りました。
generic math で使えるメソッドの中にはシフト演算の右オペランドで使えそうなものがいくつかあったりします。
例えば、`LeadingZeroCount` や `TrailingZeroCount` などが代表例でが、
これらの戻り値は `int` ではなく、`TSelf` (型引数になっている型)です。

```csharp {title="シフト演算の右オペランドに使えそうな値を返す generic math メソッド"}
using System.Numerics;

M<byte>(0x8);
M<byte>(0xF);
M<byte>(0x10);
M<byte>(0x30);

static void M<T>(T x)
    where T : IBinaryInteger<T>
{
    // pop count = 1 になっているビットの個数を求める関数。
    T count = T.PopCount(x);

    // leading zero count = 上位ビットに 0 が何個並んでいるか。
    T leading = T.LeadingZeroCount(x);

    // trailing zero count = 下位ビットに 0 が何個並んでいるか。
    T trailing = T.TrailingZeroCount(x);

    // これらの戻り値が int ではなく T (ジェネリック)。
    // こういう「ビット数」系の値はシフト演算の右オペランドで使うことがある。

    Console.WriteLine((count, leading, trailing));
}
```

これにより、「シフト演算の右オペランドは `int` だけでいい」という前提が崩れました。

まあ、元が厳しすぎたという話なので、C# 11 で制限を撤廃することになりました。
以下のようなコードが認められるようになっています。

```csharp {title="C# 11 で operator &lt;&lt;(A x, A y) とかが書けるように" highlight-text="A y"}
struct A
{
    // C# 10 以前でも書けるオーバーロード。
    public static A operator <<(A x, int y) => default;

    // C# 11 以降でだけ書けるオーバーロード。
    public static A operator <<(A x, A y) => default;
}
```

### <a id="sec-generated-title-10"></a> <a id="shift-guideline">注意: シフト以外の用途で << を使わせたくはない</a>

思想的な話でいうと、
「`<<` や `>>` という記号をシフト以外の意味で使わせるつもりはない」という方針はこれまで通りです。

ただ、構文的な制限はなくなったので、
思想に反するコードも書けるようになっています。
例えば以下のように、悪名高い「`<<` を "write" とか "append" 的な意味で使う」みたいなこともできます。

```csharp {title="某言語的な &lt;&lt;" highlight-text="cout &lt;&lt; &quot;Hellow World!&quot; &lt;&lt; endl"}
using static Iostream;

// C# の思想的には書かせたくないコードの例。
// 書けてしまうように。
_ = cout << "Hellow World!" << endl;

public static class Iostream
{
    public static readonly ConsoleOut cout = new();
    public static readonly ConsoleEndLine endl = new();

    public struct ConsoleOut
    {
        public static ConsoleOut operator <<(ConsoleOut x, string value) { Console.Write(value); return x; }
        public static ConsoleOut operator <<(ConsoleOut x, ConsoleEndLine _) { Console.WriteLine(); return x; }
    }

    public struct ConsoleEndLine { }
}
```

ただ、まあこういうコードは推奨されていないというのは今となっては割と有名な話ですし、
いわゆるガイドラインとかベストプラクティス集みたいなドキュメントで「やるべきではない」と書いておけば十分だろいうという判断が下されました。

なので、今回のシフト演算子の制限緩和でも、「`INumber<T>` インターフェイスを実装した型に限る」みたいな制限は掛けない(緩めるのであれば一切の制限をしない)ことになりました。
