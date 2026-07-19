---
title: "nameof(T<>)"
source_url: "https://ufcpp.net/blog/2025/1/nameof-unbound-generic-types/"
content_type: "BlogEntry"
published_at: "2025-01-05T11:20:36"
updated_at: "2025-01-05T11:20:36"
tags: []
umbraco_id: 2510
parent_id: 2506
sort_order: 3
aliases: []
---

# nameof(T<>)

「Rosly の [Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) に並んでいるもののうち、すでに preview 提供済みのものシリーズ第3段。

* field キーワード
* First-class Span
* nameof(T<>) ← 今日はこれ

すでに今、[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) に `preview` を指定すれば利用可能です。

今日は最後の1個の `nameof(T<>)` の話です。
当初「3つまとめて1ブログにする予定」だった原因。
こいつだけ対して書くことがなく…

### <a id="unbound-generic-nameof">nameof(T<>)</a>

今日のやつは Visual Studio 17.13.0 Preview 2 (.NET 9 の正式リリースの次のアプデ)で merge 済みです。
[`nameof` 演算子の中に unbound な型を書けるようになりました](https://github.com/dotnet/csharplang/blob/main/proposals/unbound-generic-types-in-nameof.md)。
unbound (未束縛)というのは、`List<>` みたいに、型実引数を渡してなくて(`<>` の中に何も書かず)、具体的な型が決定していない状態のジェネリック型のことを言います。

例えば以下のような感じ。

```csharp
var name = nameof(List<>.Count);

Console.WriteLine(name); // Count
```

元々、`nameof(T<int>)` とか書いても、結果の文字列は `T` だけで、型引数は何にも影響しません。
メンバー参照でも、`nameof(T.X)` でも `nameof(T<int>.X)` でも `nameof(T<string>.X)` でも、得られる文字列は `X` です。
つまり、`nameof` に取って型引数は全くの無意味でした。

それでもこれまでは unbound な型は掛けず、何か適当なダミーの型実引数を渡す必要がありました。
上記の例であれば、適当に `object` なり `int` なりを渡して、
`nameof(List<int>.Count)` とか書いていました。

`typeof` の場合は `typeof(T<>)` (unbound な型の `Type` 型インスタンスが取れる)とか書けるわけで、
`nameof` でも `nameof(T<>)` と書けてもいいじゃないかと前々から言われていました。

まあ、別に特に問題があってできなかったわけではなくて「それなりに実装コストがかかるから後回し」みたいな感じで放置されていただけです。
`typeof(T<>)` と共通のコードでできそうに見えるかもしれませんが、
`typeof(T<>)` の方では `typeof(T<>.X)` とメンバー参照することはないので、
`nameof` では「似て非なるものの再実装」が必要とのことです。

```csharp
// unbound でメンバー参照(特にインスタンス メンバーの参照)をするのは nameof だけ。
var name = nameof(List<>.Count);

// 入れ子の型なら参照することはあるけども、
List<int>.Enumerator e1 = default;

// unbound はあり得ない。
List<>.Enumerator e2 = default;

// まして、インスタンス メンバー参照はあり得ない。
_ = List<>.Count;

// 入れ子の型は unboud な typeof ができるけど、
var t1 = typeof(List<>.Enumerator);

// メンバー参照はあり得ない。
var m1 = typeof(List<>.Count);
```

一応、「理由なく掛かっていた制限を取り払った」以上の意味もありまして、
これまでは「型制約の関係でどうやっても `nameof` を使いにくい」という場面がありえました。
一例として、以下のような場面があり得ます。

```csharp
var name1 = nameof(A<_>); // これは書けるけど、
var name2 = nameof(B<_>); // これは書けない。


// 「無意味な nameof 型引数のためのダミーはこの型を使う」みたいな規約でやってたとして…
// 型制約によっては規約を守れない。
class _;

class A<T> where T : class;
class B<T> where T : struct;
```

この例はまだ「規約が守れない」程度の話ですが、
型制約が複雑になるにつれ、「そもそも `nameof` が使えない」みたいなことも起こりえるそうです。

とうことで、優先度は低くて放置はされていたものの、ようやく unbound な `nameof(T<>)` を認める実装が merge されました。

## おまけ: typeof がらみを定数扱いする特殊処理

おまけでもう1個似たような話。

`nameof` から取れる名前はかなり限られています。
`nameof(T<Arg1, Arg2, Arg3>)` から取れるのは `T` だけですし、
`nameof(A.B.C.D<E, F>.G)` から取れるのは `G` だけです。

これに対して、

* フルネームを取りたい
* 型引数も含めて取りたい

みたいなこともなくはないらしく。
一時は [`fullnameof`](https://github.com/dotnet/csharplang/discussions/701) みたいな提案も出たことがあるくらいです。

これに対する解決案として、`typeof` で取った `Type` 型のプロパティ `Name` と `FullName` を特殊処理で定数扱いしてはどうか？というものも一瞬提案されたりしてました。

* [typeof string constants #8505](https://github.com/dotnet/csharplang/issues/8505)

まあ、余りにもニッチで役立つ場面が少なすぎるということでリジェクトされて終わりましたが…
