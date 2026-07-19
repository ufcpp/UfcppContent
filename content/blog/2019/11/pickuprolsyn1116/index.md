---
title: "ピックアップRoslyn 11/16: Discriminated Union, Enhancing Common Type, Type pattern, Interpolated String Const など"
source_url: "https://ufcpp.net/blog/2019/11/pickuprolsyn1116/"
content_type: "BlogEntry"
published_at: "2019-11-16T22:00:28"
updated_at: "2019-11-16T22:23:19"
tags: []
umbraco_id: 2274
parent_id: 2273
sort_order: 0
aliases: []
---

# ピックアップRoslyn 11/16: Discriminated Union, Enhancing Common Type, Type pattern, Interpolated String Const など

10月末のと、今週の Desing Notes が3件ほど。

- [LDM notes for Oct. 30th, 2019 #2950](https://github.com/dotnet/csharplang/issues/2950)
- [LDM Notes for Nov 11, 2019](https://github.com/dotnet/csharplang/issues/2953)
- [LDM Notes for Nov. 13, 2019](https://github.com/dotnet/csharplang/issues/2968)

なんか結構一気に、C# 9.0向けと思われる議題が上がっています。

## Function pointer syntax

C# で関数ポインター的なものというと[デリゲート](../../../../study/csharp/functional/sp_delegate.md)なわけですけども、
こいつはクラスになっていて Managed なものです。
で、.NET の IL 仕様上は、デリゲートの他に生の関数ポインターもあったりします。
基本的にはネイティブ コードとの相互運用のためにあるもので、これまでは C# から直接使う方法はありませんでした。

関数ポインターを C# あらも直接触れるようにしようという話は前々から上がっていて、C# 9.0 向けの機能として作業も始まっていました。
今回は、作業を始めてみたら元々考えていた構文だと問題があったので変えたいという話です。

元々の案だと以下のような構文を考えていたんですが、これだと他の構文と不明瞭で、結構先までソースコードを先読みしないといけないのが負担になりすぎるとのこと。

```csharp
func*(int, int)
```

なので、以下のような構文にしたいとのこと。
既存の文法では `*<` が連続することがないので、これなら構文解析が楽というのが主な理由。

```csharp
func*<void>
```

あと、この機能はネイティブ相互運用のためのものなので、関数の呼び出し規約
(引数や戻り値をメモリ上やレジスター中にどう並べて受け渡しするか)
の指定の仕方をどうするかも今回の議題に。

## Enhancing Common Type Specification

ここでいう Common Type (共通型)っていうのは、[条件演算子](../../../../study/csharp/start/st_operator.md#condition)、[配列初期化子](../../../../study/csharp/structured/st_array.md#key-initializer)、['switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)などで、異なる型が同列に並んでいるときに、結果の型をどうするかという話です。

特に要望として大きいのが整数と null を混在させたときに `int?` などとして扱ってほしいというもの。

```csharp
// 以下のいずれも int? になってほしいけど、現状はコンパイル エラーに
var x = b ? 1 : null;
var y = b switch { true => 1, false => null };
var z = new[] { 1, null };
```

他にも、共通の基底クラスから派生している型 `A : Base` と `B : Base` があったとき、これらを並べたら `Base` として扱ってほしいという話もあります。
で、C# 9.0 では、ちゃんと 1 と null の共通型を `int?` と認識できるようにするつもりで作業が進んでいます。

この問題に対する解決策としては「Common Type」の他に、Target-Typed 型推論というものもあります。
C# 8.0 で入りたての `swtich` 式だけはこの Target-Typed 型推論を行っていて、たとえば以下のコードはコンパイル可能です。

```csharp
// switch 式に限り、以下の書き方はコンパイルできる。
// 変数の型から推論(terget-typed 型推論)
int? targetTyped = b switch { true => 1, false => null };
```

で、今上がっている議題は以下のような感じ。

- Common Type 解決を改善するのは決定事項
- Target-Typed 型推論の方と衝突するけども…
  - Common Type 解決の方を優先したい
  - Common Type 解決に失敗した場合のフォールバックとして、条件演算子や配列初期化子にも Target-Typed 型推論を入れたい
  - すでに Target-Typed 型推論を持っている `switch` 式は破壊的変更になる
      - `switch` 式だけ「Target-Typed 型推論を優先して、フォールバック先を Common Type 解決にする」という選択肢もなくはないものの、それはそれで混乱のもと
      - このくらいの破壊的変更は許容したい(上記混乱よりはマシ)

## [MaybeNull]T

C# 8.0 だと、以下のようなコードを書くと null 警告が出ます。

```csharp
#nullable enable
[return: MaybeNull]
T M<T>() => default;
```

[`MaybeNull` 属性](../../../../study/csharp/resource/nullablereferencetype.md#annotation-attributes)を付けると「たとえ非 null であるとされる型であっても null 許容に上書き」みたいな挙動になるものです。
ただ、現状、この属性は「メソッドの外向け」にしか機能していなくて、
メソッドの内側、この例でいうと `=> default` の部分に対しては効力が及んでいません。

現状これは「仕様」なんですが、頻繁に「バグ報告」を受けています。
好ましい挙動でもないので「積みタスク」にはなっているんですが、C# 9.0 で直す方向で動いています。

ちなみに、構文解析上の問題じゃなくて、[フロー解析に新しいステートを足さないとダメ](https://github.com/dotnet/csharplang/issues/2946)な模様。
`MaybeNullEvenIfNotNullable `ですって…

こんな感じなので、「他の属性も全部一斉に対応」ってのは実は難しいらしく、とりあえず `MaybeNull` だけ対応する(`NotNullIfNotNull` とかは 9.0 でも相変わらず `default` が警告を起こす)方向で考えるそうです。

##  Allow interpolated string constant

[`const`](../../../../study/csharp/start/sp_const.md#const) な文字列リテラルだけを使って[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)をする場合、
その結果も `const` 扱いできるようにしようという話。

```csharp
const string A = "abc";
const string B = $"{A}123"; // 今はエラーに。これを認めたい。
```

ちなみに、数値の場合は書式によってはカルチャーの影響を受ける(小数点がコンマだったり、3桁区切りがピリオドだったりする言語が結構ある)ので、
`{}` 内に書けるのは `const string` だけになりそう。

## Type patterns

[`is` 演算子](../../../../study/csharp/datatype/typeswitch.md#is) の場合は `x is Type` とか書けるのに、
['switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression) とかのパターンの場合は `x switch { Type => ... }` とは書けないのが思った以上にストレスなので「このパターン足すわ」という話。

今だと、[宣言パターン](../../../../study/csharp/datatype/patterns.md#declaration)を使えば同じようなことはできます。
`x switch { Type _ => ... }` というように、型名の後ろに [discard](../../../../study/csharp/datatype/patterns.md#discard) を付ければいいんですが、これも結構ストレスになるということで、
単に `Type` だけを書けばいい「型パターン」を追加したいとのこと。

今までこれができなかった理由は、型パターンと定数パターンが競合するからで、優先度を決めないといけません。
例えば以下のように、型名にも `X` があって、定数にも `X` があるとき、パターン中の `X` はどちらになるかという話。

```csharp
class X { }
class A
{
    const int X = 0;

    int M(object x) => x switch
    {
        X => 0,
        _ => 1,
    };
}
```

既存コードを壊さないようにするには以下のようにするしかなく、ちょっと残念感はあるものの、これで行くことになりそう。

- `is` では型優先
- `switch` では定数優先

## Name lookup with target type

Target-Typed な推論は、型の決定だけじゃなくて、メンバー名のルックアップにも使えるんじゃないかという話。
要するに、以下のようなコードを認めるようにしようという話です。

```csharp
enum E { A, B }

class Program
{
    int M(E e) => e switch
    {
        A => 1, // 今だと、E.A と書かないとダメ
        B => 2, // 同じく、E.B
        _ => 3,
    };
}
```

enum だけじゃなくて、クラスの静的メンバーのルックアップでも同様。
特に、同じく C# 9.0 で Discriminated Union (後述)も考えているので、そのためにもこの機能は役立ちそうとのこと。

## Discriminated Union

F# に同名の機能があるやつ。
他の言語で言うと `Ether` 型とか [`oneof`](https://developers.google.com/protocol-buffers/docs/proto3#oneof)とかがありますけど、
要するに「いくつかの型のうちのいずれか」みたいなやつです。

「いずれかの型」は、オブジェクト指向言語としては利便性を抜きにすれば単に「派生クラス」でできたりするんですが。

```csharp
// Base 型は A もしくは B のいずれかである
class Base { }
class A : Base { }
class B : Base { }
```

問題は2つ

- 単純に書くのが煩雑
- 網羅性 (A もしくは B 「だけ」を保証したい)が取れない

煩雑さに関してはそもそもクラス自体が煩雑というのがあり、
その解決のために C# 9.0 で検討されているのが[Records](../../10/pickuproslyn1025/index.md)になります。

ということで、その Records に合わせた文法で、網羅性も考慮に入れた構文として、以下のような案が出ています。
enum class ですって。

```csharp
enum class Shape
{
    Rectangle(float Width, float Length),
    Circle(float Radius),
}
```

ちなみに、以下のような感じで解釈されます。
(`data class` は Records で提案されている構文。)

```csharp
partial abstract class Shape
{
    public data class Rectangle(float Width, float Length) : Shape,
    public data class Circle(float Radius) : Shape
}
```

まあ、ほぼ F# の Discriminated Union 総統の機能です。

「構造体な Discriminated Union が欲しい」みたいな話もあって、検討には上がってるんですが、
とりあえず C# 9.0 のスケジュールではクラスだけを考えているみたいです。

ベースが Records なので、普通のクラスのメンバーを追加で差し込んだりもできるみたいです。
