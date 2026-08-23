---
title: "stackalloc の自然な型"
source_url: "https://ufcpp.net/blog/2022/12/stackalloc-natural-type/"
content_type: "BlogEntry"
published_at: "2022-12-05T22:24:10"
updated_at: "2022-12-05T22:24:10"
tags: []
umbraco_id: 2442
parent_id: 2438
sort_order: 1
aliases: []
---

# stackalloc の自然な型

今日は
`stackalloc T[N]` と `(stackalloc T[N])` に差があるとか、
`(stackalloc T[N]).M()` が許されるとか、
そんな感じの話。

## <a id="natural-type">ターゲット型推論と自然な型</a>

C# の文法の中には、「基本的にはターゲットを見て型決定するけども、別にターゲットがなくても型決定できる」ような文法がいくつかあります。
例えば整数リテラルがそうなんですが、以下のように、ターゲット(左辺)の型が決まっていても決まっていなくても大丈夫です。

```csharp {title="ターゲットからの型推論もできるし、推論できなかった時の自然な型も決まってる"}
// ターゲット(左辺)の型に合わせて「100」の型を決めてる。
byte x = 100;
short y = 100;
int z = 100;

// 一方で、var だとターゲットからは型決定できない。
// そういう場合の 100 は int になる。
var v = 100;
```

ちなみに、`var v = 100;` みたいに「普段ターゲットから型を決めている式が、決めれないときにデフォルトで何の型になるか」を指して「自然な型」(natural type)と言います。
上述の場合、「整数リテラルの自然な型は `int` 」ということになります。

他だと、補間文字列リテラルも「ターゲット型推論 + 自然な型持ち」です。

```csharp {title="補間文字列の自然な型"}
using System.Runtime.CompilerServices;

var x = 100;

// ターゲット(左辺)の型に合わせて「$"abc{x}"」の型を決めてる。
string s = $"abc{x}";
IFormattable f = $"abc{x}";
DefaultInterpolatedStringHandler h = $"abc{x}";

// 一方で、こちらはターゲットからは型決定できない。
// そういう場合の $"abc{x}" は string になる。
var v = $"abc{x}";
```

## <a id="stackalloc">stackalloc</a>

`stackalloc` は元々 [`unsafe`](../../../../study/csharp/interop/sp_unsafe.md) 限定機能で、
当然利用者も少ない機能でした。

ところが C# 7.2 で [`Span<T>` 構造体](../../../../study/csharp/resource/span.md)とか[安全な `stackalloc`](../../../../study/csharp/interop/sp_unsafe.md#safe-stackalloc)とか、
安全性を犠牲にせずにパフォーマンスを向上させれる文法が追加されて、
利用範囲が急に広がりました。

そして、安全な `stackalloc` の方が後入りなのもあって、`stackalloc` の自然な型はポインターのままです。

```csharp {title="stackalloc は基本的にはポインター" error-ranges="sha256:e5f080b94f6cde1b0a07fe76ec9b2151a52b3ac7b4221e2b9529572af5b3c49a;18:9-18:26"}
unsafe
{
    // stackalloc の昔からの用法。
    // 元々がこういう文法なので、 stackalloc の結果は T* (ポインター)。
    int* i1 = stackalloc int[4];

    // 型推論でも T* 扱い。
    // ↓の i2 は int* になる。
    var i2 = stackalloc int[4];
}

// C# 7.2 から
// ターゲットが Span のときに限り、safe コンテキストで stackalloc が使える。
Span<int> s = stackalloc int[4];

// ところが、stackalloc の自然な型はポインターのまま。
// 以下の行は「safe コンテキストでポインターは使えません」エラー。
var p = stackalloc int[4];
```

その後、C# 8.0 で、式の途中に `stackalloc` を書けるようになりました。
(C# 8.0 未満では、ここまで上げてきた例のように、変数に直接代入する場所にしか書けませんでした。)

```csharp {title="式の途中で stackalloc"}
// C# 8.0 未満でも書けた書き方:
Span<int> s = stackalloc int[4];

static void M(Span<int> s) { }

// こういう書き方は C# 8.0 以降でだけ書ける。
M(stackalloc int[4]);
```

こういう歴史的な流れから、現状の `stackalloc` がどうなっているかというと…

## <a id="stackalloc-anywhere">式の途中の stackalloc</a>

C# 8.0 のとき、「式の途中に `stackalloc` を書いた場合に限り、自然な型を `Span<T>` にする」という決定をしていたりします。

例えば、以下のようなコードを書くと、`M(int*)` と `M(Span<int>)` の呼び分けが掛かります。

```csharp {title="式の途中かどうかで自然な型が違う stackalloc"}
unsafe
{
    // こちらは昔ながらの型決定で、 stackalloc の自然な型はポインター。
    var p = stackalloc int[4]; // int* 扱い。
    C.M(p); // M(int*) の方が呼ばれる。

    // こちらは「式の途中」ということで、C# 8.0 以降のルールで、自然な型が Span<T> に。
    C.M(stackalloc int[4]); // M(Span<int>) の方が呼ばれる。(なので実は unsafe 不要。)
}

class C
{
    public static unsafe void M(int* _) { }
    public static void M(Span<int> _) { }
}
```

で、この「式の途中なら `Span<T>`」な仕様を使うと、以下のようなこともできたりします。

* `var` + `stackalloc` の自然な型を `Span<T>` にする
* `stackalloc` に対して拡張メソッドを呼ぶ

### <a id="natural-type-span">var + stackalloc を Span に</a>

式の途中なら自然な型が `Span<T>` になるということは…
実は `()` の有無で自然な型を変えれます。
`()` を付ければ safe。

```csharp {title="() を付ければ safe" error-ranges="sha256:156c5e2f6dc54bd9f5d67116df7028e8dc1c5c5906c72f1490db6de4c4ec7441;3:9-3:26"}
// 前述のとおり、自然な型が int* で、unsafe 必須。
// (今は unsafe を付けていないのでコンパイル エラー。)
var p = stackalloc int[4];

// こっちは自然な型が Span<int>。
// var に対して使っても Span<int> になるので safe。
var s = (stackalloc int[4]);
```

そしてまあ、型推論推進派(左辺と右辺で2度同じ型名を書きたくない)にとっては、
安全な `stackalloc` を使いつつも型推論を掛けるための回避策になります。

```csharp {title="左右に同じ型名を2度も書きたくない"}
// こう書いてもいいけども…
Span<int> s1 = stackalloc int[4];

// こっちの方が短いという。
var s2 = (stackalloc int[4]);

// まして、型名が長いときは… だいぶ差が大きい。
Span<LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm> s3 = stackalloc LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm[4];
var s4 = (stackalloc LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm[4]);

struct LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm { }
```

### <a id="extension-method">stackalloc に対して拡張メソッドを呼ぶ</a>

そして、拡張メソッドも呼べるみたいですよ。

```csharp {title="(stackalloc) なら拡張メソッドも呼べる"}
var x = (stackalloc int[4]).M(123);

static class C
{
    public static ReadOnlySpan<T> M<T>(this Span<T> span, T value)
    {
        span.Fill(value);
        return span;
    }
}
```

できる気はしていたものの、ほんとにできた…

というか、以下のようなコードを書いててふと思いつき。

```csharp {title="&quot;&quot;u8 拡張メソッド"}
using System.Text;

// u8 リテラルの自然な型は ReadOnlySpan<byte> だったはず。
// なら拡張メソッド M も呼べるはず。
"abcあいう"u8.M();

// そういや stackalloc にも自然な型あるはずよな…?

static class C
{
    public static void M(this ReadOnlySpan<byte> span)
    {
        foreach (var x in span)
        {
            Console.Write($"{x:X2} ");
        }
        Console.WriteLine();
        Console.WriteLine(Encoding.UTF8.GetString(span));
    }
}
```

ちなみに、拡張メソッド解決の仕様的に、以下のようなコードだとダメ(コンパイル エラー)だったりします。
`Span<T>` から `ReadOnlySpan<T>` への暗黙の型変換は、拡張メソッド解決の際には使われません。

```csharp {title="ReadOnlySpan の拡張メソッドに対しては使えない" error-text="stackalloc byte[4]"}
using System.Text;

// これは呼べない。
// Span<byte> → ReadOnlySpan<byte> には暗黙の型変換があるものの、
// 拡張メソッド解決の際に暗黙の型変換を挟むことは許容していない。
(stackalloc byte[4]).M();

static class C
{
    public static void M(this ReadOnlySpan<byte> span)
    {
        foreach (var x in span)
        {
            Console.Write($"{x:X2} ");
        }
        Console.WriteLine();
        Console.WriteLine(Encoding.UTF8.GetString(span));
    }
}
```
