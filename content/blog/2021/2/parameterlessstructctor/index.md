---
title: "ピックアップRoslyn: 構造体の引数なしコンストラクター"
source_url: "https://ufcpp.net/blog/2021/2/parameterlessstructctor/"
content_type: "BlogEntry"
published_at: "2021-02-27T19:01:53"
updated_at: "2021-02-27T19:01:53"
tags: []
umbraco_id: 2334
parent_id: 2327
sort_order: 2
aliases: []
---

# ピックアップRoslyn: 構造体の引数なしコンストラクター

[前回の続き](../rawstringliteral/index.md)。
というかしばらく「C# Language Design Meeting 議事録が1か月分くらいたまったので1個1個機能紹介」シリーズ。

議事録(前回と比べて2/24議事録が増えてます):

[1/27](https://github.com/dotnet/csharplang/blob/master/meetings/2021/LDM-2021-01-27.md)、[2/3](https://github.com/dotnet/csharplang/blob/master/meetings/2021/LDM-2021-02-03.md)、[2/8](https://github.com/dotnet/csharplang/blob/master/meetings/2021/LDM-2021-02-08.md)、[2/10](https://github.com/dotnet/csharplang/blob/master/meetings/2021/LDM-2021-02-10.md)、[2/22](https://github.com/dotnet/csharplang/blob/master/meetings/2021/LDM-2021-02-22.md)、[2/24](https://github.com/dotnet/csharplang/blob/master/meetings/2021/LDM-2021-02-24.md)

今日は「構造体の引数なしコンストラクター」の話。

## 概要: 構造体の引数なしコンストラクター

現状ではコンパイル エラーになる以下のコードを書けるようにしようという話です。

```csharp {title="構造体の引数なしコンストラクターとフィールド初期化子"}
struct S1
{
    public int X;
    public int Y;
 
    public S1() // これとか
    {
        X = 1;
        Y = 2;
    }
}
 
struct S2
{
    public int X = 1;
    public int Y = 2; // この2行とか
}
```

C# 6.0 の頃に一度採用しようとしたものの、`Activator.CreateInstance` のバグを踏んでしまって取りやめになっていました。
(6・7年前の話ですが、まあ、覚えている方も中にはいらっしゃるかも。)

上記議事録上は直接出てきてはいないんですが、record structs と関連して「やっぱり必要だよね」みたいな空気になっていて、改めて提案が出ました。

- [Add proposal for explicit parameterless struct constructors #4459](https://github.com/dotnet/csharplang/pull/4459)

[Language Feature Status の C# Next のところ](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#user-content-c-next)にも並んだので C# 10.0 に内定。

## 背景: default(T) と new T()

さかのぼること C# 1.0。
最初のバージョンの C# には [`default` 式](../../../../study/csharp/resource/rm_default.md#default-keyword)がありませんでした。
でも、(既定値の作成」(0 埋め)は必要で、それを構造体の場合、単に `new T()` と書いていました。

結果的に、C# の仕様は以下のようになりました。

- 構造体には明示的に引数なしコンストラクターを書くことはできない
- 構造体は暗黙的な引数なしコンストラクターがあるかのようなふるまいをしていた
  - 構造体に対して `new T()` と書くと既定値(すべてのフィールドを0埋め)を作る
  - [` : this()`](../../../../study/csharp/oop/oo_construct.md#initializer) も0埋め処理になる

その後、C# 2.0 では[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)の導入に伴って `default(T)` という書き方で既定値を作れるようになりました。
この時点で実は、「構造体の `new T()` は既定値を作るために使う」という必要性はなくなっています。
ただ、あくまで「変えても大丈夫になった」というだけで、実際に変えようという話になったのは C# 6.0 が初出だし、実現しそうなのは 10.0 です。

つまり現状、

- `new T()` と `default(T)` は全く同じ意味
  - どちらも「規定値の作成」で0初期化

で、これを、 C# 10.0 で、

- 構造体に明示的な引数なしコンストラクターを書けるようにする
- 明示的な引数なしコンストラクターを書いた場合に限り、`new T()` と `default(T)` が別の意味になる
  - `new T()` はコンストラクター呼び出し
  - `default(T)` は規定の作成

にしようとしています。

## Activator バグ

C# 6.0 でこの話が出た時、なんで即座に実装できなかったというと、[`Activator.CreateInstance`](https://docs.microsoft.com/ja-jp/dotnet/api/system.activator.createinstance?WT.mc_id=DT-MVP-4028921) の実装に問題が発覚したからだそうです。

前述の通り、元々、構造体に対して `new T()` は `default(T)` と同じ意味で使っていました。
`Activator.CreateInstance` はその前提で実装されていたそうで、
構造体に引数なしコンストラクターを書けるようにしたのにコンストラクターを呼んでもらえないという状態になったそうです。
当時、構造体に対する `CreateInstance<T>()` は常に既定値(`default(T)`)を返す実装になっていました。
(ちなみに、 .NET の型システム上は元々構造体に引数なしコンストラクターを持たせられるにも関わらず、です。)

「`Activator.CreateInstance` なんて使ったことないし、そんなに問題なの？」と思う方もいらっしゃるかと思いますが、
実際には多分、無意識に使っています。
と言うのも、ジェネリクスの `new()` 制約を付けた型を実際に `new T()` すると、内部的に `Activator.CreateInstance<T>()` が呼ばれます。

```csharp {title="new() 制約付きの型 T に対する new T()"}
class C<T>
    where T : new()
{
    public static T M() => new T(); // これが実は Activator.CreateInstanct<T>() になってる。
}
```

C# 6.0 でこの問題に気づいたあとすぐに `CreateInstance` は修正されて、
今はちゃんと「構造体でも引数なしコンストラクターがある場合はそれを呼ぶ」という実装に変更されています。

この修正は .NET Framework 4.6 の時に入っていて、となると基本的に「サポートがまだ切れていない .NET ランタイムで `CreateInctansce` がバグっているものはもうほとんどない」という状態。
(時代的背景があって .NET Framework 3.5 SP 1 とかが実はまだサポート期間中だったりしますが、
さすがに .NET Framework 3.5 で C# 10.0 を使いたいという需要はほとんどないと思われます。)

## そして実装へ (C# 10.0)

C# 9.0 で追加された[レコード型](../../../2020/6/record0609/index.md)(まだちゃんと [C# によるプログラミング入門](../../../../study/csharp/index.md)内に書いてない…)は参照型(クラスと一緒)になります。
で、レコードと同じことを値型でもやりたいという話は 9.0 の頃から当然あって、
単に「後からの追加でも問題ないし、9.0 に間に合わないから 10.0 でやる」という話になっていました。
それが record struct。
基本的にはほぼ「C# 9.0 でレコード型に対してやったことをほぼそのまま構造体に対してもやる」というものです。

C# 9.0 のレコード型では、例えば以下のような書き方ができます。
型名の直後の `()` はプライマリ コンストラクターとか呼ばれています。

```csharp {title="プライマリ コンストラクターの例"}
// プライマリ コンストラクター
record A(int x)
{
    public int X { get; init; } = x;
}
 
// 引数なしプライマリ コンストラクター
record B() : A(1)
{
    public int Y { get; init; } = 2;
}
```

これと同じようなことをしたいんだから、自然と、record struct でも以下のような書き方もできてほしくなります。

```csharp {title="record struct でも引数なしプライマリ コンストラクター"}
record struct S()
{
    public int X { get; init; } = 1;
}
```

まあ、record struct は単なる契機であって、元から以下のような書き方をしたいという要望はずっと昔からあります。

```csharp {title="普通の構造体でもフィールド初期化子"}
struct S
{
    public int X = 1;
}
```

はい、いいタイミングなのでやりましょう(いまここ)。

## 踏みそうな問題

まあ概ね「今までできなかったことの方が不自然」レベルの機能なので、そんなに説明が必要な部分はないと思います。
「フィールド初期化子があると暗黙的に引数なしコンストラクターが作られる」とか「フィールド初期化子は上から順に呼ばれる」とか、大体は「クラスと一緒」の一言で終わりそうな仕様。

いくつかだけ注意点を紹介:

- 引数なしコンストラクターのアクセシビリティは、フィールドとして含んでいる構造体のアクセシビリティ以上でないとダメ
- `where T : struct` と `where T : new()` の入れ子に注意
- [オプション引数](../../../../study/csharp/structured/sp4_optional.md)に注意

### アクセシビリティ

以下のようなコードはダメだそうです。

```csharp {title="アクセシビリティの問題"}
internal struct Internal { }
 
public struct PublicContainsInternal
{
    private Internal _internal;
 
    // このコンストラクターが Internal 構造体よりも広いアクセシビリティなのでダメ。
    // internal とか private なら OK。
    public PublicContainsInternal()
    {
        _internal = new();
    }
}
```

引数なしコンストラクターの有無で `new T()` の意味が変わるので、既存の型への引数なしコンストラクター追加は破壊的変更になります。
なので、より広い範囲に公開されてしまうコンストラクターがあると問題を起こしかねないので禁止とのこと。

### where T : struct

これまで、構造体は無条件に `new T()` できていました。
なので、以下のようなメソッドを書いて、`CreateStruct<T>()` を呼んで実行できないケースは全くありませんでした。

```csharp {title="C# 9.0 までなら絶対に大丈夫なコード"}
static T CreateNew<T>() where T : new() => new T();
static T CreateStruct<T>() where T : struct => CreateNew<T>();
```

一方で、`Activator.CreateInstance<T>()` は `T` 型の引数なしコンストラクターが public でないと例外を起こします。
ということで、もし、C# 10.0 で非 public な引数なしコンストラクターを定義した構造体に対して上記の `CreateStruct<T>()` を呼ぶと実行時に `MissingMethod` 例外が出るようになります。
(さすがに、上記 `CreateNew<T>()` 呼び出しの方をコンパイル エラーにする変更はできなさそう。)

### オプション引数

C# のオプション引数で、構造体な引数は `default(T)` だけを既定値設定できます。
例えば以下のようなコードは `new TimeSpan(0)` のところだけコンパイル エラーになります。

```csharp {title="オプション引数に default(T) は渡せても new T(...) は渡せない"}
void M(
    int x = 1, // 組み込み型の場合は const にできるもの何でも OK
    CancellationToken c = default, // default だけは渡せる。この行も OK。
    TimeSpan t = new TimeSpan(0) // これはダメ。一見定数にできそうに見えてもダメ。
    )
{
}
```

ここで問題になるのは、昔は `new T()` と `default(T)` は全く同じ意味だったという点。
ということで、以下のコードは有効な C# コードになります。

```csharp {title="new T() と default(T) が同じ意味なので OK"}
void M(
    CancellationToken c = new() // new T() と default(T) が同じ意味なので OK。
    )
{
}
```

で、C# 10.0 では「引数なしコンストラクターを持っている構造体に対しては `new T()` の意味が変わるので…
以下のような状態になります。

```csharp {title="引数なしコンストラクターを追加すると破壊的変更になる例"}
void M(S s = new()) // S に引数なしコンストラクターを足したらコンパイル エラーになる。
{
}
 
struct S
{
    int x;
    public S() => x = 1; // この行の有無で M がコンパイルできるかどうか変わる。
}
```
