---
title: "共変配列事故"
source_url: "https://ufcpp.net/blog/2022/11/covariantarrayincident/"
content_type: "BlogEntry"
published_at: "2022-11-24T23:50:02"
updated_at: "2022-11-24T23:50:02"
tags: []
umbraco_id: 2435
parent_id: 2434
sort_order: 0
aliases: []
---

# 共変配列事故

またちょっと Gist に書き捨ててたコードが増えてきたので供養ブログをしばらく書いていこうかと。

(今年はまだ少な目。一人アドベントカレンダーな量にはならず。)

## 配列の共変性

悪名高いんですが、C# のというか、.NET の配列は[共変](../../../../study/csharp/oop/sp4_variance.md#covariant-array)だったりします。

```csharp
// ↓.NET 的に許されていはいるものの、 items[0] = new Base(); が例外を起こすので今となってはあんまり使いたくない機能。
// 意図的に使うことはめったにないものの…
Base[] items = new Derived[1];

// これは問題ない
items[0] = new Derived();

// これも問題ない。 Base に Derived を代入するのは安全。
Base item = items[0];

// これがダメ。
// 実行時例外が出る。
items[0] = new Base();

class Base { }
class Derived : Base { }
```

実行時例外出ることわかってるんだからコンパイル時に禁止しろと…
(みんな言ってる。何度でも言ってる。)

`IEnumerable<T>` や `ReadOnlySpan<T>` がある現在では本当に意味不明な仕様なんですが、
まあ、 .NET の最初期(.NET Framework 1.0)の頃はジェネリクスすらなかったので、
やむなくこんな仕様を入れたんだと思います。

ちなみに、実のところ Java も配列が共変で、.NET はそれに右に倣えな感じは多少あります(初期にジェネリクスがなかったのも共通)。

## 事故発生

まあ、この仕様は昔の名残丸出しの気持ち悪い仕様なので、意図的に使うことはほとんどないんですが。
時々事故るんですよねぇ。

`Base[] items = new Derived[1];` とかいうわかりやすいコードならやらないのであって、
型推論が絡むと時々間違っちゃう。

```csharp
// 配列の型推論はソース側(右辺側)からしかやらない。
// となると…
Base[] items = new[] { new Derived() };

// 1. new[]{} の中身が Derived である
// 2. 中身からの型推論で、右辺の型は Derived[] になる
// 3. Base[] に Derive[] を代入(共変)している

// はい、アウト。実行時例外が出る。
items[0] = new Base();

class Base { }
class Derived : Base { }
```

数年に1度はやっちゃう…

ちなみに今年やったのはもうちょっと複雑で、要点を抜き出すと以下のようなコードでした。

```csharp
var testData = new[]
{
    // たくさん new A() が並んでる。
    new A
    {
        Child = new()
        {
            Items = new[] // これの推論結果は Base[] なのでセーフ。
            {
                new Base(), new Derived(),
            },
        },
    },
    new A
    {
        Child = new()
        {
            Items = new[] // これが Derived[] になってアウト。
            {
                new Derived(), new Derived(),
            },
        },
    },
    // たくさん new A() が並んでる。
};

// いろいろあって最終的に B.Items が Deserialize に渡る。
// こっちは平気だけど…
Serializer.Deserialize(testData[0].Child!.Items!);

// こっちは実行時例外起こす。
Serializer.Deserialize(testData[1].Child!.Items!);

class Serializer
{
    public static void Deserialize<T>(T[] value)
    {
        // ちなみに、共変配列が来てるとここの Span へのキャストのタイミングで実行時例外。
        Deserialize((Span<T>)value);
    }

    public static void Deserialize<T>(Span<T> value)
    {
        foreach (ref var x in value)
        {
            // x = ...
        }
    }
}

class A { public B? Child; }
class B { public Base[]? Items; }

class Base { }
class Derived : Base { }
```

来年には入るかもと目されている[コレクション リテラル](../../../2021/12/collection-literal/index.md)ではこんな問題起こさないように設計されていそうで。
この時ばかりはかなり本気で、一刻も早くコレクション リテラルに来てほしいと思いました。
