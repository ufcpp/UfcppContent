---
title: "小ネタ privateメンバーはAPIの一部か"
source_url: "https://ufcpp.net/blog/2016/12/tipsapisurface/"
content_type: "BlogEntry"
published_at: "2016-12-20T15:07:11"
updated_at: "2016-12-22T13:30:27"
tags: []
umbraco_id: 2003
parent_id: 1969
sort_order: 19
aliases: []
---

# 小ネタ privateメンバーはAPIの一部か

## ことの発端

なんかぐらばくさんとこので、[エラーになるはずのコードがPCLなプロジェクトでだけビルド通ってしまって問題になってた](https://twitter.com/Grabacr07/status/808972608309866496)らしい。

要点を抜き出すと以下のような感じ。

```csharp {title="なぜかフィールドの初期化が不要になってしまう例"}
using System;

struct DateTimeWrapper
{
    DateTimeOffset t;

    public DateTimeWrapper(int i)
    {
        // t を初期化しないとコンパイル エラーになるはず
        // でも、なぜか PCL プロジェクトではエラーにならない
    }
}
```

本来ダメなはずのコードが、PCL プロジェクトでだけコンパイルできてしまうという問題。
「ちゃんと初期化しないと怒られるはず」というのが常識のC#でこれをやられると、ほんと見つけられないバグになったりします。

プロジェクトの種類によって挙動が変わる謎の不具合…

csprojの中身を見てみても、どうも最終的に同じコンパイラーを使っていそう。
軽く[Process Explorer](http://forest.watch.impress.co.jp/library/software/prcsxplorer/)を眺めてみても、
ちゃんと同じコンパイラーが動作していそう。
コンパイラーが同じなのに、なぜ同じコードのコンパイル結果が変わってしまうのか、
謎は深まるばかり…

## 原因は参照アセンブリ

で、調べてみたら、どうも、参照しているアセンブリが違うせいみたい。

- [Reference assemblies need to include private struct fields #6185](https://github.com/dotnet/corefx/issues/6185)

ちなみに、再現用のサンプル コード: [PrivateField](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2016/PrivateField)

### 参照アセンブリ

問題の話をする前にまず簡単に、アセンブリの種類について補足。
今、[NuGet](https://www.nuget.org/)とかでライブラリを参照すると、開発時と実行時で別のDLLが参照されたりします。

- 実装アセンブリ: 実際に動くコードが入っているDLL。実行時に参照されるのはこれ。
- 参照アセンブリ: APIサーフェスだけが入っているDLL。開発時にはこっちが参照される。

これは、開発環境と実行環境が違っても問題なく開発できるようにするための処置です。

元々は .NET Framework 3.5の頃に、
クライアント プロファイルっていう、クライアント上では使わない機能を削ったバージョンの .NET Frameworkインストーラーを用意したことが発端で、
「開発環境ではつかえたクラスが、実行に TypeLoadException を起こした」みたいな自体を回避するために作られた仕組みです。
その後、[PCL](https://msdn.microsoft.com/ja-jp/library/gg597391(v=vs.100).aspx)でも同様の手法が使われるようになりました。

要するに、

- 実行環境の数だけ、開発環境にも別バージョンの .NET のインストールが必要になる
- それをすべてインストーラーに同梱していたらインストーラー サイズが大きくなりすぎる
- コンパイルに必要な情報(APIサーフェス)だけ残して、メソッドの中身とかはごっそり削ったバージョンのDLLを用意して、開発環境ではそのDLLを参照する

みたいな仕組み。
ここで言うAPIサーフェスっていうのは「APIとして外に公開されている表層の情報」という意味あいです。
見えない部分は削ってしまえと。

### どこまでが API サーフェスか

ここでちゃんと考えないといけないのが、どこまでを API サーフェスとみなすべきか。
すなわち、「開発時に参照するだけならどこまでの情報を残す必要があって、どこまでを削って大丈夫か」という話です。

publicやprotectedなメンバーはわかりやすくていいでしょう。外から見えるので、当然APIサーフェスに含まれるべきです。

ちょっと微妙なラインがinternalで、本来は外から見えないはずですが、
[`InternalsVisibleTo`属性](https://msdn.microsoft.com/ja-jp/library/bb385840(v=vs.90).aspx)なんてものもあるので、
外から見える可能性が残ります。
なので、APIサーフェスになりえます(`InternalsVisibleTo`属性があるときだけでいいんですが、参照ライブラリに残す必要があります)。

そして、private。
privateメンバーは、外から参照する手段がありません。
(リフレクションを使うと取れたりはしますけども、コンパイル時には関係ない話です。)
なので、APIサーフェスとはみなされない…
はず…？

と思いきや、privateメンバーがコンパイルに影響する場面が1つだけあります。
それが、構造体のprivateフィールド。

### 構造体のprivateフィールド

いくつか、構造体のprivateフィールドがコンパイル結果に影響を及ぼす例を挙げてみましょう。

- [確実な初期化](../../../../study/csharp/resource/rm_struct.md#definite-assignment)ルール
- ポインター型
- 再帰レイアウト

#### 確実な初期化

C#では、構造体のフィールドは、コンストラクター内で必ず初期化しないといけない、初期化するまでは他のメンバーを呼べないという制約があります。
初期化忘れによるバグを防ぐ意図があります。

でも、空っぽの構造体は初期化しなくてもいいらしい。

```csharp {title="空っぽの構造体の初期化は不要"}
struct EmptyStruct { }
struct Integer { private int _x; }

struct DefiniteAssignement
{
    EmptyStruct _e;
    Integer _i;

    DefiniteAssignement(int i)
    {
        // 中身があるものは初期化必須
        _i = new Integer();
        // 一方で、EmptyStruct みたいに空っぽのものは初期化不要
    }
}
```

中身の有無によって挙動が変わります。

#### ポインター型

基本的に、[GC](../../../../study/csharp/resource/rm_gc.md#garbage-collection)管理下のオブジェクトのポインターを作るのは危険です。

そこで、C#では以下の条件を満たす型(非管理型(unmanaged type)と呼びます)でだけポインターを作ることを認めています

- 参照型ではない
- ジェネリックではない
- 上記2条件を再帰的に満たす(フィールドに1つ含まない)

例えば、もし仮にこの条件を満たさない(GC管理下にある)型のポインターを作れたとします。
そうすると、以下のような問題のあるコードが書けてしまいます。
(そうならないように、赤線の部分をコンパイル エラーにしている。)

```csharp {title="managedな型のポインターが作れた場合の問題" error-ranges="sha256:cb4a685be7ce3c7cf96374842e8ce5625a1ead650d3ffae76c21d7e95442885b;16:38-16:53,17:23-17:31"}
using System.Runtime.InteropServices;

// 参照型を含む構造体
struct Wrapper { object _obj; }

class ManagedPointer
{
    public unsafe void X()
    {
        // Wrapper みたいに内部的に参照型のフィールドを持っている型は、本来はポインター化できない
        // sizeof 取得も本来はできない

        // unmanaged なメモリを確保
        // AllocHGlobal で取得したメモリ領域は初期化されている保証がない
        // 実行するたびに違う値が入ってる
        var p = Marshal.AllocHGlobal(sizeof(Wrapper));
        Wrapper a = *(Wrapper*)p;

        // ここで GC が発生したとすると、
        // GC が TaskAwaiter 中の Task のフィールド(未初期化)を参照する
        // 未初期化(= 意味のないランダムな値)な参照先を見に行こうとして死ぬ

        Marshal.FreeHGlobal(p);
    }
}
```

こちらも、メンバーに参照型を含んでいるかどうかを追うのに、構造体の中身を追う必要があります。

#### 再帰レイアウト

構造体の中にそれ自身の型のフィールドを持とうとすると、当然ですが無限再帰を起こします。
無限に再帰する構造体のレイアウトなんて決定できない(オーバーフローする)ので、当然禁止事項です。

```csharp {title="無限に再帰する構造体レイアウト" error-text="_x"}
struct Container<T>
{
    public T Item;
}

struct RecursiveLayout
{
    // 無限再帰するので、この構造体はレイアウトが確定できない
    Container<RecursiveLayout> _x;
}
```

再帰していないかどうかを調べるために、構造体の中身の情報が必要です。

### privateフィールドを残していない問題

この、「構造体は、中身のprivateフィールドの情報も残さないとまずい」というのに気づいたのは、
参照アセンブリの仕組みを導入したのよりもちょっと後です。
リリースまでには気づいてなくて、リリース後に不具合報告を受けて気づいたようで。

PCLプロジェクトから参照しているいくつかの参照アセンブリが、構造体のprivateフィールドまで削除してしまっていて、問題を起こします。

ということで、本題に戻りますが、PCLプロジェクトでだけ起こせる問題の数々。
以下のコード、本来はコンパイル エラーになるべきですが、PCLではコンパイルできてしまします。

1つ目。確実な初期化に漏れるケース。

```csharp {title="本来は未初期化エラーになるはず" error-ranges="sha256:dc54482cfa6aa4a461a7dd013dc870a2363cf0ac5876c50f0b0a451955468801;8:12-8:30"}
using System;

struct DefiniteAssignment
{
    // DateTimeOffset には中身があるはずなのに…
    DateTimeOffset _x;

    public DefiniteAssignment(int n) { } // PCL ではエラーにならない
}
```

2つ目。ポインター化できるかどうかの判定をミスるケース。

```csharp {title="本来はポインター化できない型をポインター化" error-ranges="sha256:63795f5824542e565e452d29494a8fc3a629cd1eb5e03b08fadc969c491112f9;10:38-10:57,13:27-13:39"}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class ManagedPointer
{
    public unsafe void X()
    {
        // TaskAwaiter は内部的に Task クラスのフィールドを1個だけ持っている
        // 本来はポインター化できない
        var p = Marshal.AllocHGlobal(sizeof(TaskAwaiter));

        // PCL ではエラーにならない
        TaskAwaiter a = *(TaskAwaiter*)p;

        // ここで GC が発生したとするとまずい

        Marshal.FreeHGlobal(p);
    }
}
```

3つ目。無限再帰なレイアウトを作れてしまうケース。

```csharp {title="無限再帰レイアウト" error-text="_x"}
using System.Collections.Generic;

struct RecursiveLayout
{
    // 無限再帰するので、この構造体はレイアウトが確定できない
    KeyValuePair<RecursiveLayout, RecursiveLayout> _x; // PCL ではエラーにならない
}
```

どれも結構まずいんですが、今のところ、これがPCLではコンパイルできてしまっています。
`DateTimeOffset`、`KeyValuePair`、`TaskAwaiter`などの構造体で、PCLが参照している参照アセンブリでは中身がごっそり削られているのが原因。

### この問題を踏む可能性

この問題ですが、根本的には「参照アセンブリを作るときに消しちゃいけないところまで消しすぎた」というのが原因なわけで、
参照しているもの次第で起こるかどうかが決まります。

問題が起きるケース:

- PCL を使っていて、上記の`DateTimeOffset` などを参照する
- 同様に、[.NET Standard 向けのライブラリ](https://docs.microsoft.com/ja-jp/dotnet/articles/standard/library) プロジェクトでも、該当する型を参照すると問題が起きる

起きないケース:

- アプリなど、実行アセンブリを直接参照しているもの
- 実装アセンブリを直接提供しているライブラリなら問題が起きない
  - [`ValueTask`](https://www.nuget.org/packages/System.Threading.Tasks.Extensions/)や[`ValueTuple`](https://www.nuget.org/packages/System.ValueTuple/)は実装アセンブリしか提供していないので、こいつらでは問題は起きない

### 問題への対処(検討中)

とりあえず、どこの問題かというと参照アセンブリを作るツールになります。

今は構造体のすべてのprivateフィールドを削ってしまっている挙動を、以下のように変更する必要があります。
(フィールドをすべて残すのではなく、以下のルールにするのは参照アセンブリのサイズ削減のため。
C#コンパイラーが誤動作しないようにするにはこのルールで十分。)

- 1つでも値型のフィールドを持っていれば、`int`型など適当な型のフィールドを1個だけ作って含める
- 1つでも参照型のフィールドを持っていれば、`object`型のフィールドを1個だけ作って含める
- ジェネリックな構造体の場合は、ジェネリック型引数で与えられた型のフィールドは消さずに残す

今は、C# コンパイラー自身が実装アセンブリリと同時に参照アセンブリを作る機能を持っているみたいなので、
基本的にはC# チームの仕事かも。
(昔からそうだったわけではなくて、割かし最近、そういう機能を持った。
それ以前は、オープンになっていないツールで、標準ライブラリの参照アセンブリ作りをしてた。)

とはいえ、現在問題を起こしている参照アセンブリとかを、ちゃんと治ったバージョンのツールで生成しなおして、
パッケージをNuGetサーバーに上げなおす作業は、たぶんC# チームの範疇外。

問題を起こす状況も限られているし、複数のチームが絡んでいるしで、ちょっと修正には時間が掛かりそうな雰囲気…
