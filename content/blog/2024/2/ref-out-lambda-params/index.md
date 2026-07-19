---
title: "ラムダ式の引数で、型名を省略して ref, out などだけを指定"
source_url: "https://ufcpp.net/blog/2024/2/ref-out-lambda-params/"
content_type: "BlogEntry"
published_at: "2024-02-25T22:53:14"
updated_at: "2024-02-25T22:53:14"
tags: []
umbraco_id: 2489
parent_id: 2480
sort_order: 8
aliases: []
---

# ラムダ式の引数で、型名を省略して ref, out などだけを指定

ラムダ式で、`ref` 引数などに対して `ref x => { }` みたいに書けるようにしたいという話が出ています。

## ラムダ式での ref 引数、out 引数

ラムダ式は、状況が許すなら、`x => { }` などといったように非常に簡素に書けます。
ところが、[`ref`](../../../../study/csharp/resource/sp_ref.md#sec-byref) や [`out`](../../../../study/csharp/resource/sp_ref.md#out) が絡むとそうもいかなくて、型推論が効く状況でも型名を省略できません。

```csharp
// 通常、ラムダ式は型推論が効く限り、引数の型を省略できる。
Action<int> a = x => { };

// ところが ref, out などの修飾が付いた引数は省略不可。

// これなら OK。
RefAction<int> r = (ref int x) => { };
OutFunc<int> o = (out int x) => x = 1;

// ダメ。CS1676 エラー。
RefAction<int> r1 = x => { };
OutFunc<int> o1 = x => x = 1;

delegate void RefAction<T>(ref T arg);
delegate void OutFunc<T>(out T arg);
```

特に、「他にも引数が多かったり、他の引数のどれかに型名が長くて書きたくない引数がある」みたいな状況では相当に不便です。

```csharp
// 全部の引数に型の明示が必要。
ManyParams a = (int x, int y, int z, ref int r) => { };

// r の型は省略できない。
ManyParams a1 = (x, y, z, r) => { };

// 「部分的に型を明示」というのも書けない。
ManyParams a2 = (x, y, z, ref int r) => { };

delegate void ManyParams(int x, int y, int z, ref int r);
```

```csharp
// 全部の引数に型の明示が必要。
LongTypeName a = (IReadOnlyDictionary<(int x, int y), List<string[,]>> x, ref int r) => { };

// r の型は省略できない。
LongTypeName a1 = (x, r) => { };

// 「部分的に型を明示」というのも書けない。
LongTypeName a2 = (x, ref int r) => { };

delegate void LongTypeName(IReadOnlyDictionary<(int x, int y), List<string[,]>> x, ref int r);
```

これに対して、`ref x => { }` みたいな書き方は認めてもいいんじゃない？という話があります。

```csharp
// 現状ダメ。でも、これくらいはできてもいいのでは？
RefAction<int> r = (ref x) => { };
OutFunc<int> o = (out x) => x = 1;
ManyParams m = (x, y, z, ref r) => { };
LongTypeName l = (x, ref r) => { };

delegate void RefAction<T>(ref T arg);
delegate void OutFunc<T>(out T arg);
delegate void ManyParams(int x, int y, int z, ref int r);
delegate void LongTypeName(IReadOnlyDictionary<(int x, int y), List<string[,]>> x, ref int r);
```

## コミュニティ提案

実際アイディア自体は2015年くらいからずっとあります。

* [Declaration of ref/out parameters in lambdas without typename #338](https://github.com/dotnet/csharplang/issues/338)

`ref` 引数ラムダ式とか自体が使用頻度低めなのでそれほど優先度はついておらず、
ずっと「Any Time」(C# チーム自らはやらず、「コミュニティ貢献お待ちしております」状態)でした。

これに対して、去年くらいに実際、コミュニティからの提案ドキュメントが上がっていました。

* [Declaration of lambda parameters with modifiers without type name](https://github.com/dotnet/csharplang/blob/main/proposals/ref-out-lambda-params.md)

[履歴](https://github.com/dotnet/csharplang/commits/main/proposals/ref-out-lambda-params.md))を見るに、2023年7～8月くらいにコミュニティから提案されていて、C# 12 作業中は進捗なし。
今月に入ってから C# チームの中の人が引き取って検討を始めていそうな感じですね。

そして[数日前の Language Design Meeting](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-21.md) で議題に。
とりあえず提案は承認されたみたいです。

## その他検討事項

Desing Meeting では対案も2点ほど検討されたんですがそちらはリジェクト。
元の提案の方向で受け付けるみたいです。

対案その1は、`x => { }` だけで `ref`/`out` も「推論」してもいいのでは？という案。
ただ、C# の `ref` 引数、`out` 引数は、呼び出し元にも `ref`/`out` の明示を求めるくらいなので、さすがに `x => { }` というような書き方はちょっと C# 的には違和感があります(なのでリジェクト)。

```csharp
RefAction<int> r = (ref int x) => { };
OutFunc<int> o = (out int x) => x = 1;

// ref, out 引数は呼び出し側にも ref, out を書く必要があるくらい明示を求められる。
// 呼び出し先で書き変わるのは明示されないと怖い。
int local;
o(out local);
r(ref local);

// なのでラムダ式側でも ref, out は書かないと違和感。
// 以下のような書き方は今後も乗り気ではない。
RefAction<int> r1 = x => { };
OutFunc<int> o1 = x => x = 1;

delegate void RefAction<T>(ref T arg);
delegate void OutFunc<T>(out T arg);
```

対案その2は前述の `ManyParams` とか `LongTypeName` とかの例で書いたような、「引数の一部分の型名を省略、一部分を明示」です。
ただ、これは「`r` に指定した型から `x` の型を推論」みたいな別の要望が加わるだろうことと、
それをやると[部分型推論](../partial-inference/index.md)の話と同様、
推論を頑張ろうとすると指数的なコンパイル時間になってしまう可能性があってちょっと怖いそうです(なのでリジェクト、やるとしても部分型推論と一緒に)。

```csharp
// ラムダ式引数の部分型指定 + 型引数の推論。
// 結構推論機構が複雑になるはず。
static ManyParams<T> Create<T>(ManyParams<T> a) => a;
var a1 = Create((x, y, z, ref int r) => { });

delegate void ManyParams<T>(T x, T y, T z, ref T r);
```

あと、元の提案に残っていた「属性や、引数のデフォルト値はどうしよう？」という未解決の議題についても「大変そうなわりに需要がない」ということで、やらないことになりそうです。

```csharp
using System.Diagnostics.CodeAnalysis;

// C# 10 と 12 で、こんな感じで属性を付けたりデフォルト値を指定できるようになった。
Func<string, int> f = ([MaybeNull] string s = null) => s?.Length ?? 0;

// これに対して型名省略したい？
// (そんなに需要なさそうな割に、これを実装するのは大変そう。)
Func<string, int> f1 = ([MaybeNull] s = null) => s?.Length ?? 0;
```
