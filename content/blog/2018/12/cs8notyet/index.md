---
title: "C# 8.0 その他 (Preview 1での未実装機能)"
source_url: "https://ufcpp.net/blog/2018/12/cs8notyet/"
content_type: "BlogEntry"
published_at: "2018-12-12T09:39:14"
updated_at: "2018-12-12T09:39:14"
tags: []
umbraco_id: 2193
parent_id: 2177
sort_order: 11
aliases: []
---

# C# 8.0 その他 (Preview 1での未実装機能)

これまで紹介してきたもの以外にも、C# 8.0での導入が予定されている機能はいくつかあります。
ただ、Visual Studio 2019 Preview 1でまだ実装されていない機能・ちゃんと動いていない機能はまとめて軽く紹介して終わりにしようかと思います。
次以降のPreviewで実装されたらまた改めて紹介します。

## インターフェイスのデフォルト実装

インターフェイス中のメソッドに実装を持てるようになります。
これに関しては昔書いた記事があるのでそちらを参照:

- [「インターフェースのデフォルト実装」の導入（前編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/013)
- [「インターフェースのデフォルト実装」の導入（中編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/014)
- [「インターフェースのデフォルト実装」の導入（後編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/015)

先日「[RuntimeFeature クラス](../runtimefeature/index.md)」で紹介した通り、
ランタイムの修正が必須の機能です。

## pattern-base な using/foreach

[前に1度書いていますが](../../7/pickuproslyn0711/index.md)、C# には単なるメソッド呼び出しに置き換えるような、シンタックスシュガーな文法が結構あります。
例えば、クエリ式の場合、以下の2行は全く同じ意味になります。

```csharp
var q1 =
    from x in source
    where x > 5
    select x * x;
 
var q2 = source
    .Where(x => x > 5)
    .Select(x => x * x);
```

問題はここから先。
クエリ式の場合は、この`Where`や`Select`メソッドにかなり自由が効きます。

- 特にインターフェイスの実装等は必要なく、所定のパターンを満たしていれば何でもいい
- インスタンス メソッドでも拡張メソッドでもいい
- オプション引数や params があってもいい

一方で、`foreach`の場合だと以下の制限が掛かります。

- インスタンス メソッドでないとダメ
- オプション引数や params があるとダメ

さらに、`using`ステートメントに至ってはもっと厳しい制限が掛かっています。

- `IDisposable`インターフェイスを実装していないとダメ

これに対して、C# 8.0 では、`foreach`と`using`でもクエリ式と同程度の緩さで「パターンでの(pattern-based)実装」が認められるようになります。
[昨日](../cs8asyncstreams/index.md)紹介した非同期版の `foreach` も同様です。

ちなみに、提案では「enhanced using」と呼ばれていて、
次節の「using declarationとセット」、かつ、「`using`の方が主役で`foreach`の方はおまけ」です。

## using declaration

`using`ステートメントに対して、以下のような要望は多いです。

- `using`のネストがしんどい、
- `Dispose`したいタイミングはほとんどの場合、変数のスコープと同じ

ということで、以下のように、変数に対する修飾子として`using`を書くことで、
その変数のスコープから抜けるときに`Dispose`を呼ぶという機能を追加する予定です。

```csharp
struct A
{
    void Dispose() => Console.WriteLine("A Disposed");
}
 
class Program
{
    static void Main()
    {
        using var a = new A();
        using var b = new A();
 
        {
            using var c = new A();
            // c のスコープはここまでなので、ここで c.Dispose()
        }
 
        // ここで b.Dispose(); a.Dispose();
        // ちなみに、宣言とは逆順で呼ばれる
    }
}
```

## Target-typed new

C# 7.1で入った[`default`式](../../../../study/csharp/cheatsheet/ap_ver7_1.md#default-expr)と同様に、`new`に対しても左辺からの型推論が効くようになります。

```csharp
// これは 右→左 の推論。C# 3.0 の頃から使える。
var a1 = new A(1, 2);
 
// C# 8.0 では、左→右 の推論が入る。
A a2 = new(1, 2);
```

## caller expression attribute

C# 5.0で、[Caller Info 属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)というものがいくつか入っています。
以下のように、コンパイラーによって呼び出し元のメソッド名などを挿入してもらう機能です。

```csharp
using System;
using System.Runtime.CompilerServices;
 
class Program
{
    static void M([CallerMemberName]string callerName = null)
        => Console.WriteLine(callerName);
 
    static void Main()
    {
        // M には何も引数を渡していないものの、
        // CallerMemberName が付いているので null ではなく、呼び出し元のメソッド名
        // (この場合は "Main")がコンパイラーによって挿入される。
        M();
    }
}
```

C# 8.0で、この手の属性が1つ増えます。
`CallerArgumentExpression`属性を付けることで、
引数に渡した式全体を受け取れます。

```csharp
using System;
using System.Runtime.CompilerServices;
 
class Program
{
    static void M(int x, [CallerArgumentExpression("x")]string xExpression = null)
        => Console.WriteLine(xExpression);
 
    static void Main()
    {
        M(1 + 2 + 3); // "1 + 2 + 3" が xExpression に渡る
        M(2 * 3);     // 同上、"2 * 3"
    }
}
```

わかりやすい用途は、例えば`XUnit.Assert`とかです。
単体テストが失敗したときに、失敗の原因になった式をログに表示できます。

## generic attributes

属性にジェネリックなクラスを使えるようになります。

```csharp
using System;
 
class MyAttribute<T> : Attribute { }
 
[My<int>]
class Target { }
```

## 機能一覧

ここで紹介したのは、roslyn リポジトリにある[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)を元に選んだものです。
一方で、csharplang の方の [8.0 candidate](https://github.com/dotnet/csharplang/milestone/8) マイルストーンの方には他にもいくつか並んでいます。

7.0 の時の経験からいうと、
基本的には[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)に並んでいるものが実装されていきますが、
多少の入れ替わりはあったりします。
急にLanguage Feature Statusに追加されるものもあれば、
今並んでいても8.xに回されることもあります。

例えば、実装状況を見るに、以下の2つなんかはLanguage Feature Statusに並んでいませんが、8.0 に入るんじゃないかという感じがします。

- [Champion: Unmanaged constructed types #1744](https://github.com/dotnet/csharplang/issues/1744)
- [Champion "Negated-condition if statement" #882](https://github.com/dotnet/csharplang/issues/882)
