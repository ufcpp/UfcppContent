---
title: "RuntimeFeature クラス"
source_url: "https://ufcpp.net/blog/2018/12/runtimefeature/"
content_type: "BlogEntry"
published_at: "2018-12-04T10:13:34"
updated_at: "2018-12-04T10:21:12"
tags: []
umbraco_id: 2182
parent_id: 2177
sort_order: 3
aliases: []
---

# RuntimeFeature クラス

先日 [C# 8.0 予告なブログ](../../11/cs80_net48/index.md)で書いた通り、
C# 8.0 で入る[インターフェイスのデフォルト実装](https://github.com/dotnet/csharplang/issues/52)は .NET ランタイム側の修正が必要な機能です。

今日は、そういう「ランタイム側機能」についての話を少し。

## ランタイム側機能

C# の言語機能は、C# コンパイラーがちょこっと頑張ってよい具合にコード生成して、
古い .NET Framework ランタイム上でも動くものが多いです。
「古いランタイム上では動かない新機能」というと、実は .NET Framework 2.0 での[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)の導入まで遡ります。
.NET Framework 2.0 は2005年リリースですし、
C# 8.0 には実に13年ぶりにランタイムの方に修正を求める機能が入ったことになります。

契機となったのはやっぱり .NET Core の存在です。
.NET Core は、オープンソースで開発ペースも速く、
side by side (1台のPCに複数バージョンを同時にインストール可能)なのでランタイムの更新がしやすいという利点があります。
ランタイム更新しやすいからこその、13年ぶりの新機能を追加です。

登場直後の .NET Core は .NET Framework の下位互換のような存在でしたが、
.NET Core 2.0 くらいから互換性が増してきて、
.NET Core 3.0 ではついに Windows 限定な WPF や UWP などの GUI フレームワークも .NET Core 上で動くようになりました。
一応、完全新規の案件であれば .NET Framework の方をわざわざ使う理由がないくらいにはなっています。
ようやく、次のステップに進む段階に入ったといえます。
これも、このタイミングで13年ぶりの新機能追加に至った要因でしょう。

## RuntimeFeature クラス

しかし、ランタイムの新旧によって使えない機能があるとなると、
それを検知・コンパイル時にエラーにする仕組みが必要になります。
コンパイルできたはいいけど、実際に動かそうとした段階で無理だったというのでは困ります。

その検知機構として用意されたのが、`RuntimeFeature`クラス(`System.Runtime.CompilerServices`名前空間)です。
以下のようなクラスになっていて、`const string`なメンバーが存在するかどうかで、その機能を使えるかどうかを判定します。

```csharp {title="RuntimeFeature クラス"}
public static partial class RuntimeFeature
{
#if FEATURE_DEFAULT_INTERFACES
    public const string DefaultImplementationsOfInterfaces = "DefaultImplementationsOfInterfaces";
#endif
    public const string PortablePdb = "PortablePdb";
    public static bool IsSupported(string feature);
}
```

今のところ生えているメンバーは、

- `DefaultImplementationsOfInterfaces` … 今回追加されたインターフェイスのデフォルト実装の可否
- `PortablePdb` … 動的コード生成で Portable PDB を解釈できるかどうか

の2つです。

### PortablePdb

ちなみに、`PortablePdb`の方について補足。
まず、PDB はデバッグ情報が掛かれたファイルで、
Visual C++ の頃から同名の拡張子のファイルは作られていました。
C# でも、ビルド時に dll や exe と一緒に拡張子が pdb のファイルが作られていると思います。

拡張子自体はずっと同じ pdb ですが、内部の形式については最近ガラッと変わりました。
以前の pdb は、仕様がオープンになっておらず、
pdb を読み込めるデバッガーが Windows 依存でした(なので、通称 "Windows PDB")。
そこで、.NET Core では、せっかくなので仕様自体をオープンにした Portable な pdb 形式を作ることにしたそうです。
それが「Portable PDB」。

PDB は基本的に C# コンパイラーが生成するものなので、`RuntimeFeature` (ランタイム側)とは無関係そうに見えます。
では `PortablePdb` は何のためにあるかと言うと、動的コード生成です。
例えば C# スクリプト実行であっても、内部的にはちゃんと PDB を生成して、
デバッグ情報が取れるようにしてあります。
このとき、生成する PDB を Portable PDB にしていいか、クラシックな Windows PDB でないとダメかを判別するための機構がないと困るので、`RuntimeFeature.PortablePdb`があります。

## その他の RuntimeFeature 機能

その他に、`RuntimeFeature`クラスには
(.NET Standard 2.1 から)以下のような bool 型の静的プロパティもあります。

```csharp {title="DynamicCode"}
public static partial class RuntimeFeature
{
    public static bool IsDynamicCodeSupported { get; }
    public static bool IsDynamicCodeCompiled { get; }
}
```

先ほどの2つとは違って、こちらは実行時に値を確認して使う用みたいです。
そのランタイムで、

- `IsDynamicCodeSupported` … そもそも動的コード実行は可能か
- `IsDynamicCodeCompiled` … 動的コード実行はコンパイルされているか(= インタープリター実行ではないか = パフォーマンスよく実行できるか)

の判別に使います。
例えば、動的コンパイルができない環境で[式ツリー](../../../../study/csharp/dynamic/sp3_expression.md)の `Compile` メソッドを使ったりすると、
高速化のためにやってることなのにかえって破滅的に遅いコードになってしまいます。
それを避けるために分岐に使うのがこれらのプロパティ。

## 今後入るかもしれないランタイム側機能

とりあえず、`DefaultImplementationsOfInterfaces` は最初の一歩です。
これからは定期的に、こういう .NET ランタイム側の修正が必要な機能が追加されていくものと思われます。
(おそらく、基本的にはメジャー バージョンアップのタイミングでの追加。
そんなに高頻度で追加はしないと思われます。)

例えば、以下のような issue ページがあります。
.NET ランタイム(CLR)に修正を入れれるなら実現できそうな C# 機能の一覧。

- [What language proposals would benefit from CLR changes?](https://github.com/dotnet/csharplang/issues/317)

挙がっている内容は、以下のようなものです。

### CLR unification of types across assemblies

別アセンブリで定義された「同名で同じメンバーを含む型」を同一視したいというもの。

DI 用途でほしかったり。
あと、匿名クラス(内部的に匿名クラスを生成してる)が public なところで使えない理由にもなっているので回避方法が欲しいという要望があります。

### Make void a first-class type

要するに、`Func<void>`とか`Task<void>`と書かせろ、
`Action`や非ジェネリック`Task`との分岐がめんどい、というあれ。

### Covariance and contravariance for classes

現状、[変性](../../../../study/csharp/oop/sp4_variance.md)が認められているのはインターフェイスとデリゲートだけなわけですが、どうにかしてクラスでも認めてほしいというやつ。

`Task<object>` に `Task<string>` を代入したいとか、そういうやつ。

### |, &, and ~ operators on a type parameter with the enum constraint

`where T : Enum` が付いてるとき、その型の変数に対してビット演算したいというやつ。
今、列挙型とジェネリクスの相性が悪すぎてつらく。

### Union and intersection types

最近 TypeScript に入ったあれ。
`string | int` で「`string` か `int` のどちらか」みたいな型を作ったり、
`IA & IB` で「2つのインターフェイス`IA`と`IB`の両方を実装した(両方のメソッドを使える)型」を作ったり。

ある程度は C# コンパイラーのレベルでできるんですが、
ランタイムに手を入れて型システムのレベルで対応した方がよいという話。

### Support generic indexers

インデクサーに型引数を取りたいと。
↓みたいな。(今は、この`T`がどうやっても使えない。)

```csharp {title="インデクサーに型引数"}
public interface IOptions
{
    T this<T>[OptionKey<T> key]
    {
        get;
        set;
    }
}
```

### Higher-kinded polymorphism

ジェネリクスの型制約に複雑な条件を付けたいというやつ。
例として挙がってるのは↓みたいなコード。

```csharp {title="複雑な型制約"}
public static T<A> To<T, A>(this IEnumerable<A> xs)
    where T : <>, new(), ICollection<>
{
    var ta = new T<A>();
    foreach (var x in xs)
    {
        ta.Add(x);
    }
    return ta;
}
```

### methods in enums

列挙型に直接メソッドを持ちたいというやつ。
拡張メソッドでの実装だとちょっとつらいこともあり。
