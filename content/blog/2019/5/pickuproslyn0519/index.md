---
title: "ピックアップRoslyn 5/19: dotnet-try, .NET Core 3.0 パフォーマンス、null 許容参照型の仕様改善"
source_url: "https://ufcpp.net/blog/2019/5/pickuproslyn0519/"
content_type: "BlogEntry"
published_at: "2019-05-19T22:54:19"
updated_at: "2019-05-19T23:12:41"
tags: []
umbraco_id: 2243
parent_id: 2241
sort_order: 1
aliases: []
---

# ピックアップRoslyn 5/19: dotnet-try, .NET Core 3.0 パフォーマンス、null 許容参照型の仕様改善

この1週間ほど、[build](https://www.microsoft.com/en-us/build) で発表したことを改めてブログ化したものが投稿されたりとか、build が終わって落ち着いたところで本業に戻ったと思われる投稿とかがたくさんありました。

そのうち3つほど紹介

- dotnet-try
- .NET Core 3.0 でのパフォーマンス改善
- C# Design Notes 2件追加(どちらも null 許容参照型がらみ)

## dotnet-try

- [Introducing the Try .NET Global Tool - interactive in-browser documentation and workshop creator](https://www.hanselman.com/blog/IntroducingTheTryNETGlobalToolInteractiveInbrowserDocumentationAndWorkshopCreator.aspx)
- [Create Interactive .NET Documentation with Try .NET](https://devblogs.microsoft.com/dotnet/creating-interactive-net-documentation/)

以下のようなコマンドで簡単にインストールできるツール([.NET Global Tool](https://docs.microsoft.com/ja-jp/dotnet/core/tools/global-tools) という仕組み)として、 dotnet-try というものが公開されました。

```console
dotnet tool install --global dotnet-try
```

C# でいわゆる interactive workshop (ドキュメント中に直接コードが埋め込まれてて、実行結果が見れるような講習資料)を簡単に作れるようにしようというもの。
以下のような仕組み。

- 普通に dotnet コマンドでコンパイルできるプロジェクト一式を書く
- Markdown 中の  ```cs 行に、その C# コードを参照するオプションを付ける
- プロジェクトを置いたフォルダー中で dotnet try ツールを起動する
- dotnet try 自体が Web サーバーになっていて、ブラウザーが起動して localhost アクセスで書いたドキュメントが表示される
- ドキュメント中の C# コードは構文ハイライト表示されるし、実行ボタンがあって結果を出力できる
  - [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/client)を使ってるっぽい

### Try .NET Online

2017年以降、 .NET 関連のドキュメントは[docs.microsoft.com 上にある](https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/intro-to-csharp/)わけですが、
その中ではサンプルの C# コードをブラウザー中で実行して結果を見れる機能があります。
これを指して「Try .NET Online」と言っています。

初期はほとんどの処理をサーバー上でやっていて常に通信してコードや実行結果を表示していました。
それを徐々に Blazor と Web Assembly 化しているそうで、
今はだいぶクライアント上での処理になっているみたいです。
(まだ完全にオフラインではなくて、コンパイルはサーバー側でやっていそう。)

### Try .NET Offline

今回リリースされた dotnet-try ツールは、
「Try .NET Online」と同じドキュメントをローカル環境で書いて試せるという仕組みで、
「Try .NET Offline」と言っています。

普通の C# プロジェクトを作ってそれを参照する仕組みなので、
ちゃんとコンパイルできることが確認を取れているコードの一部分を参照して、
その部分の実行結果が正しく表示されます。

全部オープンソースです。

- [dotnet/try](https://github.com/dotnet/try)

現状、とりあえずアルファ リリースみたいです。フィードバック募集中。

そういう段階のツールなので、利便性はまだまだあまりよくありません。
作った interactive workshop の共有みたいなところまでは至っていなくて、
現状だと作る側も自前で GitHub ででも公開してもらって、
見る側も自前で以下のようにコマンドラインでツール起動してもらう作り。


```console
git clone 公開先
dotnet try cloneしてきたリポジトリ
```

## .NET Core 3.0 のパフォーマンス向上

- [Performance Improvements in .NET Core 3.0](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-3-0/)

[.NET Core 2.0](https://blogs.msdn.microsoft.com/dotnet/2017/06/07/performance-improvements-in-net-core/)や[.NET Core 2.1](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1)の時にもブログがありましたが、.NET Core 3.0 でも改めて「こんな最適化をやったよ」というまとめブログが上がりました。

今回はまたものすごい長大な内容…

長くてまじめに読むのは大変ですが、概ね、以下のような感じ。

- [`Span<T>`](../../../../study/csharp/resource/span.md)とか`Memory<T>`自体を最適化した
- `Span<T>`、`Memory<T>` を使うものを引き続き増やした
- [Hardware Intrinsics](../../../2018/12/hdintrinsic/index.md)を全面的に適用開始
  - バイナリ操作とか文字列操作は本当に2～4倍高速化
  - ただし、現状は Intel CPU のみ([AVX2](https://ja.wikipedia.org/wiki/%E3%82%B9%E3%83%88%E3%83%AA%E3%83%BC%E3%83%9F%E3%83%B3%E3%82%B0SIMD%E6%8B%A1%E5%BC%B5%E5%91%BD%E4%BB%A4) が有効な環境でだけ2～4倍高速化)
  - [ARM 系はまだまだこれから](https://github.com/dotnet/corefx/issues/37199)
- 非同期処理の最適化
  - [`IValueTaskSource`](../../../2018/3/pickuproslyn0323/index.md)の適用範囲を増やしたり
- その他、細かい最適化も大量
  - `T?` から `Value` で値を取ってたところを `GetValueOrDefault` に置き換えて回ったり
  - `new T[0]` を `Array.Empty<T>()` に置き換えて回ったり

## Design Notes 2件(null 許容参照型がらみ)

- [Added: LDM Notes for May 13, 2019 #2527](https://github.com/dotnet/csharplang/issues/2527)
- [Added: LDM Notes for May 15, 2019 #2536](https://github.com/dotnet/csharplang/issues/2536)

どちらも [null 許容参照型](../../../2018/12/cs8nrt/index.md)がらみで、
だいぶ具体的な話。
先日、「[そろそろ C# 8.0 に入れる機能を決めるタイムリミット](../build2019/index.md)」という話をしましたが、
これもその一環だと思います。
そろそろ null 許容参照型の仕様も固めないとまずい。

### #nullable の書き方

null 許容参照型(破壊的変更にならないように opt-in)を有効化するにあたって、
以下の2つの視点があります。

- annotations: ライブラリを提供する側として、null の許容・非許容のアノテーションを公開するかどうか
- warnings: ライブラリを使う側として、コード解析をして null 参照に対して警告を出すかどうか

移行段階としては、どちらか片方だけを有効化したいことがあります。

- 差し当たってアノテーションだけは付けたいけど、中身の警告を全部消す作業まで手が回らない
- 差し当たって警告は出してほしいけど、自分が公開している API にまでは責任を持てないのでアノテーションは付けたくない

結果、指定できるオプションは以下のようにまとめたいとのこと。

```csharp
#nullable (enable | disable | restore) [ warnings | annotations ]
```

- enable、disable で有効・無効を切り替え
  - restore は1つ前のディレクティブの状態に復元
  - その後ろに何も書かなければ annotations も warnings もまとめて切り替え
- その後ろに warnings、 annotations を付けることで、どちらか片方だけを切り替え

ソースコード中の `#nullable` ディレクティブ(その行移行の局所的に作用)と同じように、
`csproj` 中に書くタグ(プロジェクト全体に作用)も、以下のようにしたいみたいです。

- タグ名はシンプルに `Nullable` にする
  - 今は `NullableContextOptions` とかいう長い名前
- オプションの値も enable、disable、annotations、warnings にする

### アノテーションの付け方

前節の `#nullable enable` を指定した状態では、基本的に以下のようなアノテーションの付け方になります。

- 参照型 `T` に対して単に `T` と書くと非 null
- `T?` と書くと null 許容

ただ、これだとどうしてもうまくいかない場合があって、
そういうとき用に属性でのアノテーションを足したいという話。

#### 単純な事前・事後条件

`T` と書いていても null 許容に、`T?` と書いていても非 null に変えたい場合があります。

- [制約](../../../../study/csharp/oop/sp2_generics.md#where)未指定の `T` がジェネリック型引数の場合、値型と参照型で `T?` の意味が違うせいで `T?` と書けないので属性に頼るしかない
- プロパティで、get は非 null だけど set は null 許容とかにしたい
- [参照引数](../../../../study/csharp/resource/sp_ref.md#sec-byref-param)で、in/out 片側だけを null 許容にしたい
  - null を渡してもいいけど、メソッドを呼んだあとはその変数が null じゃなくなる保証あり
  - null を受け取れないけど、メソッド内で null を書き込む可能性あり

そこで、以下の属性を用意

- 事前条件
  - `[AllowNull]`: `T` と書いていても、入力として null を受け付ける
  - `[DisallowNull]`: `T?` と書いていても、入力として null を受け付けない
- 事後条件
  - `[MaybeNull]`: `T` と書いていても、出力として null を返す可能性がある
  - `[NotNull]`: `T?` と書いていても、出力として null を返さない

#### 相互依存のある事後条件

`TryParse` とか `IsNullOrEmpty` とか、何かメソッドを呼んだ結果 null かどうかが確定するものがあります。
こういうとき用に、以下のような属性を用意。

- `[MaybeNullWhen(bool)]`: 戻り値が true/false の時に限り、指定した引数が null になる可能性がある
- `[NotNullWhen(bool)]`: 戻り値が true/false の時に限り、指定した引数が null でない保証がある

「引数が null のときだけ戻り値も null」みたいなこともあります。
そのための属性もあり。

- `[NotNullIfNotNull(string)]`: 指定した引数の nullability と戻り値の nullability が一致

#### コンパイラーによる特殊対応

`x == y` とか書くと、`x` の nullability が `y` に伝搬します。
`x` が非 null なことがわかっているなら、`y` も非 null で確定。

`==` はいいとして、それと同様の効果があるメソッドがいくつかあります。

- `Object.ReferenceEquals`
- `Object.Equals`
- `IEqualityComparer<T>.Equals`
- `EqualityComparer<T>.Equals`
- `IEquatable<T>.Equals`

こいつらは、数が限られているし、これら以外のメソッドで等値判定をすることはほとんどないので、コンパイラーにハードコードで実現したいそうです。
ちゃんと、`x.Equals(y)` で nullability が伝搬するものの、それは `Object.Equals` を特別扱いすることで実装。

同じく、`Interlocked.CompareExchange` も特別扱いで nullability 伝搬するそうです。

#### フローがらみの属性

今ある「[確実な初期化ルール](../../../../study/csharp/resource/rm_struct.md#definite-assignment)」でもそうなんですが、到達できない場所のフロー解析はされません。

```csharp
using System;
 
class Program
{
    static void Main()
    {
        int x;
        return;

        // 本来「x を初期化せずに使っちゃダメ」と怒られるようなコード。
        // 別にエラーにならない。その代わり、「return のせいでここには絶対来ないよ」警告が出る。
        Console.WriteLine(x);
    }
}
```

nullability のフロー解析もこれと同じ挙動になります。

そこで困るのが、以下のような状況。

```csharp
using System;
 
class Program
{
    // 例外を出すのでこのメソッドより後ろは絶対に実行されない。
    static void Throw() => throw new Exception();
 
    static void Main()
    {
        int x;
 
        // 絶対に戻ってこない。
        Throw();
 
        // 以下の2行もアプリケーション クラッシュになるのでここより後ろは実行されない。
        System.Diagnostics.Debug.Assert(false);
        Environment.FailFast("fatal error");
 
        // でも、現状だとコンパイラーがそれを知るすべがな。
        // なので「到達できない」警告じゃなく「未初期化」エラーに。
        Console.WriteLine(x);
    }
}
```

ということで、以下のような属性も用意。

- `[DoesNotReturn]`: メソッドを呼んだ時点でそこより後ろは実行されない
- `[DoesNotReturnIf(bool)]`: 指定した引数が true/false の時、そこより後ろは実行されない
