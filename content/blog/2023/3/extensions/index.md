---
title: "【C# 12 候補】 Extensions"
source_url: "https://ufcpp.net/blog/2023/3/extensions/"
content_type: "BlogEntry"
published_at: "2023-03-05T22:39:12"
updated_at: "2023-03-05T22:39:12"
tags: []
umbraco_id: 2458
parent_id: 2457
sort_order: 0
aliases: []
---

# 【C# 12 候補】 Extensions

今日は「拡張」(拡張メソッド的なものの改良)の話。
(今日のこれは、C# 12 で全て実装されるかどうか怪しく、
一部 13 以降になる可能性も結構高いです。)

* 提案ドキュメント: [Extension types](https://github.com/dotnet/csharplang/blob/main/proposals/extensions.md)
* Working Group 議事録
    * [2022/11/10](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2022-11-10.md)
    * [2023/1/23](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2023-01-23.md)
    * [2023/1/25](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2023-01-25.md)
    * [2023/2/15](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2023-02-15.md)

結構昔から、

* [Extension everything](https://github.com/dotnet/roslyn/issues/11159): 拡張メソッドと同じような仕組みでプロパティ、インデクサー、演算子などを「拡張」したい
* [Roles](https://github.com/dotnet/csharplang/blob/main/meetings/2022/LDM-2022-09-26.md#roles--extensions): 「拡張」をある種の「型」扱いしたい

みたいな案があったんですが、結局、この Roles をベースに、Extensions とか Extension types という名称で実装が進みそうです。

原案で「Roles/Extensions」と呼ばれていたものは、「Explicit /Implicit extensions」となります。

## extension キーワード

提案されている現状の文法では、新たに `extension` キーワードを使った「型定義」できるようにするみたいです。

例えば、`int` に対する「拡張」を書くのなら、以下のような書き方をします。

```csharp {title="int に対する extension"}
implicit extension Ex for int
{
}
```

### なんでも拡張

現状の[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)の仕様では、名前通り、メソッドしか定義できません。
プロパティなどを「拡張」したいという要望は長らくあるんですが、
今の拡張メソッドの文法がプロパティなどに向いていなさ過ぎて、導入できずにいます。
また、静的メンバーにも対応していません。

```csharp {title="プロパティに向かない文法、静的メンバーにも未対応" error-ranges="sha256:00dbcf8fe3f817dcad6049dd4b81f15fb2b877c831066c1811a1764704ddc0d2;8:23-8:31,11:23-11:27,14:32-14:33" error-diagnostics="sha256:00dbcf8fe3f817dcad6049dd4b81f15fb2b877c831066c1811a1764704ddc0d2;CS0548@8:23-8:31,CS0720@11:23-11:27,CS0548@11:23-11:27,CS0106@11:23-11:27,CS1534@14:32-14:33,CS0715@14:32-14:33,CS0161@14:32-14:33"}
static class Extensions
{
    // x.Method() と呼べる。
    // 第1引数を特別扱いしてる都合上…
    public static void Method(this int x) { }

    // 引数のないプロパティとか、
    public static int Property { }

    // インデクサーはどうするか悩ましい。
    public static int this[int index] { }

    // 元が static なものを拡張する手段もない。
    public static int operator +() { }
}
```

`extension` を使った定義では、インスタンス フィールドと[自動プロパティ](../../../../study/csharp/oop/oo_property.md#auto)・[自動イベント](../../../../study/csharp/functional/sp_event.md#auto-event)(暗黙的にフィールドが必要)を除いて、どのメンバーでも使えます。

```csharp {title="プロパティやインデクサー、静的メンバーにも対応"}
implicit extension Ex for int
{
    public void Method() { }
    public int Property => this;
    public int this[int index] => index;

    public static void StaticMethod() { }
    public static Ex operator+ (Ex x) => x;
}
```

ちなみに、インターフェイスも実装できる予定です。
既存の(第3者が作っていて自分では手を入れられない)型にインターフェイスを後挿しできます。

```csharp {title="拡張インターフェイス実装"}
implicit extension Ex for bool : IFormattable
{
    public void ToString(string? format, IFormatProvider? formatProvider) => this ? "true" : "false";
}
```

これで、以下のような呼び出しができるようになる予定です。

```csharp {title="拡張定義したプロパティ、インデクサー、静的メソッドを呼び出し"}
int x = 0;

x.Method();
_ = x.Property;
_ = x[1];
int.StaticMethod();

IFormattable f = true;
```

### 拡張「型」

既存の拡張メソッドでも起こるんですが、
複数の拡張があるとき、同名のメソッドが被ってどちらを呼ぶべきか解決できない時があります。

```csharp {title="名前被りで解決できない拡張メソッド" error-ranges="sha256:1779b09f107c42074b5fe896cab0b89662381930fde7f017c0061a8711997b42;4:3-4:9" error-diagnostics="sha256:1779b09f107c42074b5fe896cab0b89662381930fde7f017c0061a8711997b42;CS0121@4:3-4:9"}
int x = 0;

// 2つ同名のメソッドがあって優先度解決できないのでコンパイル エラー。
x.Method();

// 解決するためには途端に「普通の静的メソッド」呼びに戻る。
Ex1.Method(x);
Ex2.Method(x);

static class Ex1
{
    public static void Method(this int x) { }
}

static class Ex2
{
    public static void Method(this int x) { }
}
```

また、拡張メソッドは元々あるインスタンス メソッドよりも優先度が低いので、
同名のメソッドで「上書き」することもできません。

```csharp {title="拡張メソッドでは同名インスタンス メソッドの上書きはできない"}
int x = 0;

// インスタンス メソッドの方が優先度が高く、この書き方で Ex1.ToString は呼べない。
x.ToString();

// 「普通の静的メソッド」呼びで一応解決は可能。
Ex1.ToString(x);

static class Ex1
{
    public static void ToString(this int x) => x.ToString("X2");
}
```

これらの例の通り、
名前被り時の解決方法は「普通の静的メソッドとして呼ぶ」という手段です。

一方、`extension` では、以下のように、キャスト的な文法で解決します。

```csharp {title="キャストで拡張を使う"}
int x = 0;

// 「暗黙」にやろうとすると、extension を使ったやり方でも解決不能・元々あるメソッド優先。
x.Method();   // これは解決不能。
x.ToString(); // これは int.ToString が呼ばれる。

// キャスト構文で解決可能。
((Ex1)x).Method();   // Ex1.Method。
((Ex2)x).Method();   // Ex2.Method。
((Ex2)x).ToString(); // Ex1.ToString。

// 「拡張型」の変数で1度受けるのでも解決可能。
// この場合は int のメソッドよりも extension のメソッドの方が優先。
Ex1 ex = x;
ex.Method();
ex.ToString();

implicit extension Ex1 for int
{
    public void Method(this int x) { }
    public void ToString(this int x) => x.ToString("X2");
}

implicit extension Ex2 for int
{
    public void Method(this int x) { }
}
```

### 実際に型として使える

`Ex1 ex` みたいな変数を定義できることからもわかる通り、
`extension` は普通に「型」という扱いです。
なので、拡張型 (extension types)と呼びます。

変数だけではなく、引数、型引数などにも使えます。

```csharp {title="拡張型引数"}
using System.Collections;

int x = 0;

// int → Ex1 の暗黙の変換。
M1(x);

// IEnumerable<int> → IEnumerable<Ex1> の暗黙の変換。
M2(new[] { 1, 2, 3 });

// 引数に拡張型を使う。
static void M1(Ex1 x) => Console.WriteLine(x);

// 型引数に拡張型を使う。
static void M2(IEnumerable<Ex1> x)
{
    foreach (var item in x) Console.WriteLine(item);
}

implicit extension Ex1 for int
{
}
```

### explicit extension

これまで説明なしで `implicit extension` という書き方をしてきましたが、
そこから察していただける通り、`explicit extension` もあります。
名前通り型の明示が必須になって、
`int` などの元の型のままでメンバーを呼ぶことができなくなります。

```csharp {title="explicit exntension" highlight-ranges="sha256:b259e4cf19f2e4fbcece9e3b080b7673955cefe8d83523c810a8ad5e88a871aa;10:1-10:9"}
// (implicit なら呼べるけど) explicit extension では呼べない。
1.Method();
int.StaticMethod();

// こんな風に、型を明示して呼ぶ想定。
Ex ex = 1;
ex.Method();
Ex.StaticMethod();

explicit extension Ex for int
{
    public void Method() { }
    public static void StaticMethod() { }
}
```

「`1.Method()` みたな呼び方ができないものが『extension』なのか？」みたいな話はあります。
なので、元々は role, view, shape (同じデータの別の役割・見え方・輪郭)みたいな言葉を使おうかという話も出ていました。
ただ、変に用語を増やすよりは、「暗黙的拡張」、「明示的拡張」と呼び分ける方がいいのではないかということになって、こちらにも `extension` を使おうという流れになっています。

ちなみに、同じ型に対する別の extension はお互い型変換させるつもりはないそうです。

```csharp {title="2つの異なる explicit exntension"}
// 基となる型から extension への変換は暗黙 OK。
Ex1 ex1 = 1;
Ex2 ex2 = 2;

// extension 同士の変換はダメ。
Ex2 ex3 = ex1;

explicit extension Ex1 for int { }
explicit extension Ex2 for int { }
```

要は、strong-typedef 的なものに使えます。
(この辺りが「それは extension なのか？」と言われるゆえんです。
拡張するメンバーが一切なくても使い道があります。)

### 細かい文法話

extension は別の extension からの派生もOKで、
多重継承も認めるそうです。

インターフェイス実装もできるわけで、
`:` の後ろには他の extension とインターフェイスが並びます。
例えば以下のような感じ。
(`T` は通常の型、`I` 始まりのものがインターフェイス、`X` 始まりのものが extension。)

```csharp {title="extension 定義(文法まとめ)"}
implicit extension X for T : XA, XB, IA, IB
{
}
```

ちなみに、ここでいう `T` (`for` の後ろの型)のことを「基になる型」(underlying type: 根底にある型、基礎となる型)と言います。
(C# 的には、`enum` なんかの `enum E : int { }` とかの `int` の部分も underlying type と言います。Microsoft の和訳では undelying type = 基になる型。)

クラスの場合は基底クラスとインターフェイスをあまり区別せず、`class Derived : Base, IA, IB` と書ける(ただし、基底クラスは先頭である必要あり)わけですが、
extension の場合は `for` を使って `:` とは分ける方向で考えているみたいです。
基底型をいくつも持てるし、ただでさえ基底型とインターフェイスの混在があるのに、さらに基になる型 `T` も並べた時に、「同じ `:` を使って、一番先頭という縛りを設ける」というのはいささか不安だったそうです。
特に、`partial` を認めるつもりなので、その場合に「一番先頭」があやふやになるのを懸念したみたいです。

```csharp {title="partial extension"}
implicit partial extension X for T : XA, IA
{
}

implicit partial extension X : XB, IB
{
}
```

また、既存の拡張メソッドがトップレベルの型での定義以外を認めていないのに対して、
新しい extension は入れ子を認めるそうです。

```csharp {title="partial extension"}
using static Ex;
using static C;

// ちゃんと呼べる。
1.M1();
2.M2();

implicit extension Ex for T
{
    implicit extension NextedEx for int
    {
        void M1() { }
    }
}

class C
{
    implicit extension NextedEx for int
    {
        void M2() { }
    }
}
```

さらに、ジェネリックにもできるそうです。

```csharp {title="generic extension"}
implicit extension X<T> for T : XA, IA
    where T : IT
{
}
```

派生 extension を作る際には、
基となる型の条件を強める方向でなら、基となる型の変更もできるみたいです。

```csharp {title="基となる型の変更"}
implicit extension XBase for IEnumerable<object>
{
}

// IEnumerable<object> から IEnumerable<string> への変更はOK。
// (逆だとダメ。)
implicit extension XDerived1 for IEnumerable<string> : XBase
{
}

// ちなみに、基となる型に変更がないなら for は省略可。
implicit extension XDerived2 : XBase
{
}
```

## 実装方法

現状、文法面をどうするかが議論の中心で、
あんまり実装方法に関する決定はないみたいなんですが、
案として挙がっているのは以下のような方向性です。

例えば、前述の(以下に再掲) extension に対して、

```csharp {title="前述の extension"}
implicit extension Ex for int
{
    public void Method() { }
    public int Property => int;
    public int this[int index] => index;

    public static void StaticMethod() { }
    public static Ex operator+ (Ex x) => x;
}
```

以下のようなラッパー構造体を作るのはどうかという案になっています。

```csharp
ref struct Ex
{
    private ref int @this;
    public Ex(ref int @this) => this.@this = ref @this;

    public void Method() { }
    public int Property => @this;
    public int this[int index] => index;

    public static void StaticMethod() { }
    public static Ex operator +(Ex x) => x;
}
```

[ref 構造体](../../../../study/csharp/resource/refstruct.md)、[ref フィールド](../../../../study/csharp/resource/refstruct.md#ref-field)を使う想定なので、
別途以下のような機能(C# 11 時点で認められていない)が必要になります。

* ref 構造体の ref フィールドを持てるようにする
* ref 構造体をジェネリック型引数にする
* ref 構造体でインターフェイスを実装する

```csharp {title="C# 11 で無理なものの、extension の実装に欲しいもの" error-ranges="sha256:b3151fa0ebd57923d85c1a4223fade83105e771639bc9805e683a018f1c66147;2:16-2:32,5:5-5:10,5:11-5:16,8:20-8:28" error-diagnostics="sha256:b3151fa0ebd57923d85c1a4223fade83105e771639bc9805e683a018f1c66147;CS0535@2:16-2:32,CS0535@2:16-2:32,CS8343@2:16-2:32,CS9050@5:5-5:10,CS0523@5:11-5:16,CS0306@8:20-8:28" warning-text="_refS" warning-diagnostics="sha256:b3151fa0ebd57923d85c1a4223fade83105e771639bc9805e683a018f1c66147;CS0169@5:11-5:16"}
// 現状、ref 構造体はインターフェイス実装を持てない。
ref struct S : IEnumerable<int>
{
    // 現状、ref 構造体の ref フィールドはダメ。
    ref S _refS;

    // 現状、ref 構造体を型引数に渡せない。
    IEnumerable<S> GetItems()
    {
        yield return default;
    }
}
```

## 実装フェーズ

冒頭に「C# 12 で全て実装されるかどうか怪しい」という話をしましたが、
具体的には以下のような3つのフェーズに分かれています。

1. 静的メンバーの拡張だけ認める
2. インスタンス メンバーも認める
3. インターフェイス実装を認める

前節で説明したように、ref フィールドを使った実装にする可能性が濃厚なわけで、
これら3フェーズは要するに、

* 静的メンバー: 現状でもできる
* インスタンス メンバー: ref 構造体の ref フィールドを認めた上でやりたい
* インターフェイス実装: ref 構造体のインターフェイス実装を認めた上でやりたい

という区分だったりします。

1と2を分けるのは少々気持ち悪いので実際にはこの2つは同時に提供されるかもしれませんが、
実装都合でいうと結構な難易度の隔たりがあるそうです。

ちなみに、「[静的メソッドの拡張をしたい、既存の型に静的メソッドを追加したい](https://github.com/dotnet/csharplang/discussions/2505)」という要望もそれなりに昔からあるので、
1だけ先行実装というのもそこまで不自然でもないかもしれません。
