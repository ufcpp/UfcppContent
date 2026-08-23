---
title: "bool 型の false, true, それ以外"
source_url: "https://ufcpp.net/blog/2019/1/falsetrueother/"
content_type: "BlogEntry"
published_at: "2019-01-28T14:50:18"
updated_at: "2019-01-28T15:02:17"
tags: []
umbraco_id: 2219
parent_id: 2216
sort_order: 2
aliases: []
---

# bool 型の false, true, それ以外

これまで(C# 7.3 まで)、C# の `switch` ステートメントで `bool` 型を使う場合、以下のように、`default` 句が必須になることが多々ありました。

```csharp {title="true, false, default..."}
static int X(bool b)
{
    switch (b)
    {
        case false: return 0;
        case true: return 1;
        default: return -1;
    }
}
```

`bool` 型には `false` と `true` しかないはずなのにこれはおかしいと言われ続けていたんですが、C# 8.0 では `default` 句が要らなくなるというか、`default` 句を絶対に通らなくなるよう、コード生成の仕方を変更するみたいです。

今日はこの辺りの、要するに「`false` でも `true` でもない `bool` 値」の話。

サンプルコード: [BoolExhaustiveness](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2019/BoolExhaustiveness)

## bool とは

### ドキュメント上

まず、ドキュメント上、`bool` がどうなっているかというと…

- [C# Language Reference での `bool` の説明](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/bool)
  - `System.Boolean` 型のエイリアスで、真偽値、すなわち、`true` か `false` を格納できる
- [`Boolean` 構造体の説明](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)
  - `true` か `false` のいずれかの2値を取れる型

大体は2つの値だけを取れる型として説明されています。

### 実装上: Boolean 構造体

その `Boolean` 構造体(`System` 名前空間)の内部実装がどうなっているかというと、

- 1バイトの構造体
- `true` の内部表現は 1
- `false` の内部表現は 0

です。
1バイトだけども0と1しか必要としないので、残り254個の値は基本的には使われません。

## 0 でも 1 でもない bool を作る

普通にリテラルの `true`, `false` や、`==` などの条件式から `bool` 値を得る限り、本当に0と1以外の値は発生しません。

ただ、C# は unsafe な手段を使って任意に値を書き換えれちゃうので、無理やりやると 0 でも 1 でもない `bool` 値を作れます。

具体的にはいくつか書き方があるんですが、1つ目は素直にポインターを使うもの。

```csharp {title="ポインターを使って変な bool を作る"}
unsafe bool toBool(byte b) => *((bool*)&b);
Console.WriteLine(toBool(2));
```

もう1つは、[`Unsafe` クラス](../../../2018/12/unsafe/index.md)を使う書き方。
これもまあ、書き方が違うだけでポインターと大差ないです。

```csharp {title="Unsafe クラスを使って変な bool を作る"}
bool toBool(byte b) => Unsafe.As<byte, bool>(ref b);
Console.WriteLine(toBool(2));
```

最後に、`StructLayout` を使う(C 言語の union 風な使い方する)方法。
`LayoutKind.Explicit` は、ポインター並みに変なことができちゃう機能なので、
そもそも unsafe コードなしで使えること自体が疑問視されていたりもします。
要するに、実質 unsafe。

```csharp {title="LayoutKind.Explicit を使って変な bool を作る"}
static void Main()
{
    bool toBool(byte b)
    {
        Union u = default;
        u.Byte = b;
        return u.Boolean;
    }

    Console.WriteLine(toBool(2));
}

[StructLayout(LayoutKind.Explicit)]
private struct Union
{
    [FieldOffset(0)]
    public byte Byte;
    [FieldOffset(0)]
    public bool Boolean;
}
```

## 0 でも 1 でもない bool を使うとどうなるか

x86 などの CPU では、条件分岐命令が以下のような方法で実現されています。

- 直前の命令の結果が 0 になったら立つフラグが CPU 内に存在する
- そのフラグを見て分岐する

要するに、「0 かどうか」しか見ません。
この意味では、「true とは 0 以外の全ての値を指す」と言えます。

### C# の if ステートメント

.NET の中間言語もそういう挙動をします。
[brtrue 命令](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.brtrue)ってのを持ってるんですが、
こいつは「value が 0 でなければ分岐」という挙動。

C# の `if` ステートメントはこの命令(もしくはその逆の [brfalse](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.brfalse))に変換されるので、
「0 以外の値」は全て `true` 扱いになります。
実際、前述の方法で作った「中身が2の`bool`値」を `if` に渡すと `true` 側に分岐します。

```csharp {title="中身が2のboolは、if 中では true 扱い"}
using System;

class Pointer
{
    static void Main()
    {
        unsafe bool toBool(byte b) => *((bool*)&b);

        Branch(false);     // if (false)
        Branch(true);      // if (true)
        Branch(toBool(2)); // if (true)
    }

    static void Branch(bool b)
    {
        if (b) Console.WriteLine("if (true)");
        else Console.WriteLine("if (false)");
    }
}
```

```console {title="中身が2のboolは、if 中では true 扱い"}
if (false)
if (true)
if (true)
```

### C# 7.3 までの switch ステートメント

問題はここからなんですが…

`if` ステートメントとは違って、(C# 7.3 までの) `switch` ステートメントは中身の値を見ます。
すなわち、普通の `true` と、「中身が2の`bool`値」は別の値という扱い。

これが、冒頭のコードで `default` 句が必須になる理由です。
[実際、`case true` を通らないようなコード](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2019/BoolExhaustiveness/BoolOtherThan01/Program.cs)を書けます。

```csharp {title="case false も case true も通らない bool 値"}
static void Main()
{
    // 0 → false
    // 1 → true
    // それ以外 → if (b) は通るんだけど、switch (b) { case true: } は通らない(C# 7.3 までは)変な値になる。
    for (byte i = 0; i < 3; i++)
    {
        Console.WriteLine($"value = {i}");
        Branch(Pointer(i));
        Branch(UnsafeAs(i));
        Branch(UnionStruct(i));
    }
}

/// <summary>
/// false (0) の時は何も表示されない。
/// true (1) の時は if(b) switch(b) の両方が表示される。
/// 「それ以外の値」を作って渡すと、if(b) だけが表示される。
/// </summary>
static void Branch(bool b)
{
    if (b) Console.WriteLine("    if(b)");
    switch (b) { case true: Console.WriteLine("    switch(b)"); break; }
}
```

### 型 switch

ちなみにこの「中身の値を見て分岐」挙動は、`case` が全部定数の場合(= 古き良き昔からある `switch`) の場合だけの挙動です。

C# 7.0 から入った、[パターン マッチングを使った `switch`](../../../../study/csharp/datatype/typeswitch.md#switch)(いやゆる「型 switch」)の場合には brtrue 命令が使われるようになって、[`if` ステートメントと同じ挙動になります](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2019/BoolExhaustiveness/BoolOtherThan01/TypeSwitch.cs)。

```csharp {title="型 switch は brtrue と同じ挙動" highlight-text="when true"}
using System;

class TypeSwitch
{
    static void Main()
    {
        Branch(0);
        Branch(1);
        Branch(2);
    }

    static unsafe void Branch(byte x)
    {
        var b = *((bool*)&x);

        Console.WriteLine($"value = {x}");
        Console.Write("    traditional switch: ");
        switch (b)
        {
            case false:
                Console.WriteLine("false");
                break;
            case true:
                Console.WriteLine("true");
                break;
            default:
                // 0でも1でもないbool値の時にここに来る
                Console.WriteLine("other");
                break;
        }

        Console.Write("    type switch: ");
        switch (b)
        {
            case false when true:
                Console.WriteLine("false");
                break;
            case true:
                Console.WriteLine("true");
                break;
            default:
                // 絶対ここは通らない
                Console.WriteLine("other");
                break;
        }
    }
}
```

```console {title="型 switch は brtrue と同じ挙動" highlight-text="other"}
value = 0
    traditional switch: false
    type switch: false
value = 1
    traditional switch: true
    type switch: true
value = 2
    traditional switch: other
    type switch: true
```


### マーシャリング

ちなみに、[P/Invoke](../../../../study/csharp/interop/sp_pinvoke.md#pinvoke)を使う際には、マーシャリング時に「0でも1でもない`bool`値」を`true`(内部的に1の`bool`値)に置き換える処理が掛かるみたいです。

例えば、以下のような Rust コードを lib.dll 中で定義しておいて、

```rust {title="8ビット整数値を素通しする Rust 関数"}
#[no_mangle]
pub extern fn id(x: i8) -> i8 { x }
```

これを C# 側から以下のように呼び出します。

```csharp {title="id 関数を C# から呼び出し"}
using System;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        // 素通し。当然、2。
        byte a = Id(2);
        Console.WriteLine(a);

        // 素通しじゃなくて、bool で値を受け取り。true。
        bool b = ToBool(2);
        Console.WriteLine(b);

        unsafe
        {
            // 内部表現を見てみると、1 になってる。
            byte b1 = *(byte*)&b;
            Console.WriteLine(b1);
        }
    }

    /// <summary>
    /// rust 側の id 関数は i8 を素通しするだけ。
    /// それを DllImport で呼んでるので、このメソッドも素通し。
    /// </summary>
    [DllImport("lib.dll", EntryPoint = "id")]
    private static extern byte Id(byte x);

    /// <summary>
    /// マーシャリングで、byte な戻り値を bool で受け取ることができる。
    /// ただ、この場合、素通しではなくて、ちゃんと 戻り値 != 0 で bool に変換されているみたい。
    /// </summary>
    [DllImport("lib.dll", EntryPoint = "id")]
    private static extern bool ToBool(byte x);
}
```

`id`関数の戻り値は `i8` (C# でいう `sbyte`)ですが、マーシャリング時に `bool` への変換をしてくれます。
変換の仕方は、`!= 0` になっているみたいで、「0 でない値」だったら普通の `true` (内部的に1の`bool`値)が返ってきます。

### C# 8.0 での switch ステートメントの変更

まあ、要するに、`switch` ステートメントだけがきもいです。

たびたび「`case false` と `case true` があれば `default` 要らないだろ」と言われ続け、
そのたびに「内部的に `false` でも `true` でもない値があり得るから」という回答が返って来続けていたんですが。

この度、「ドキュメント上も 『`true` と `false` の2値』と明記されているんだから、それ以外の値を想定して非効率なコードを生成するのはおかしいだろ」という突っ込みがあって、「それは確かに」的な空気になったみたいです。

また、C# 8.0 では [`switch` 式](../../../2018/12/cs8switchexpr/index.md)も入るので、網羅性のチェック(「`true` と `false` で全パターン網羅している」という判定)をしたい需要が高まったので、ついに折れて、`bool` に対する `switch` の挙動を変えることにしたみたいです。

```csharp {title="bool に対する switch の仕様変更"}
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(X(false)); // -1
        Console.WriteLine(X(true)); // 1

        unsafe
        {
            byte x = 2;
            bool y = *(bool*)&x;
            Console.WriteLine(X(y)); // C# 7.0 までは 0 だった。C# 8.0 で 1 になるように。
        }
    }

    static int X(bool b)
    {
        switch (b)
        {
            case false: return -1;
            case true: return 1;
            default: return 0;     // C# 7.0 までは何も言われなかった。C# 8.0 で「到達できないコード」警告出るように。
        }
    }
}
```

内部的には `if` 相当のコードへの置き換えです。

ちなみに、Visual Studio 2019 Preview 2だと、「[LangVersion を 7.3 以下にしてても新しい方の挙動になってしまう](https://github.com/dotnet/roslyn/issues/32806)」というバグがあったりします。
バグ認定はされていて、正式版までには「C# 8.0 以上にした場合だけ新しい挙動になる」に変更されるはずです。
