---
title: "C# 13 でのコレクション式 - 制限の緩和の話"
source_url: "https://ufcpp.net/blog/2024/3/colexpr13-relax-restriction/"
content_type: "BlogEntry"
published_at: "2024-03-09T21:31:10"
updated_at: "2024-03-09T21:31:10"
tags: []
umbraco_id: 2493
parent_id: 2490
sort_order: 2
aliases: []
---

# C# 13 でのコレクション式 - 制限の緩和の話

## C# 13 でのコレクション式 - 制限の緩和の話

C# 12 で[コレクション式](../../../../study/csharp/cheatsheet/ap_ver12.md#collection-expression)が入ったわけですが、
スケジュールの都合で「C# 12 後に改めて検討する」ということになった機能がたくさんあります。
C# 12 リリース(2023/11)直後から再検討が始まっていて、先月にはある程度まとまった計画が出ています。

* [[Proposal]: Collection Expressions Next (C#13 and beyond)](https://github.com/dotnet/csharplang/issues/7913)

量が多いのでちょっとずつ取り上げ…

* ディクショナリ式
* 自然な型
* インラインなコレクション式
* コレクションに対する拡張メソッド
* 現状でコレクション式に対応してない型
* 非ジェネリックなコレクションのサポート
* 制限の緩和 ← 今日はこれ

## 制限の緩和

今、コレクション式の要素の型は `IEnumerable<T>` の `T` で判定しています。

```csharp {title="iteration type を元に型判定してる"}
using System.Collections;

foreach (var x in new A()) ; // この x は int

// Add(int) だけあればよさそうに見えるのに、
// 実際には IEnumerable<int> をみて「int のコレクション」と判断してる。
A a = [1];

// foreach すると int を列挙する型。
class A : IEnumerable<int>
{
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    public void Add(int x) { }
}
```

```csharp {title="インターフェイス実装を消したらエラー"}
// foreach はインターフェイスがなくても GetEnumerator っていう名前のメソッドさえ持っていれば OK なのに。
foreach (var x in new A()) { }

// これはダメになる。
A a = [1];

// インターフェイスを削るとコレクション式で使えなくなる。
class A
{
    public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
    public void Add(int x) { }
}
```

```csharp {title="コレクション初期化子は使えるのに…"}
using System.Collections;

// foreach なんとか OK。
// non-generic な GetEnumerator が呼ばれてるので object を介してるけど…
foreach (int x in new A()) { }

// 旧来のコレクション初期化子は使えるのに…
A a1 = new() { 1 };

// コレクション式はダメになる。
A a2 = [1];

// non-generic インターフェイスに変えると？
class A : IEnumerable
{
    public IEnumerator GetEnumerator() => throw new NotImplementedException();
    public void Add(int x) { }
}
```

ちなみに、この「`IEnumerable<T>` の `T`」以外は受け付けなかったりします。
これも、コレクション初期化子時代はできたこと。

```csharp {title="コレクション初期化子は使えるのに… (再)"}
using System.Collections;

// 旧来のコレクション初期化子は string を受け付けるのに…
A a1 = new() { 1, "2" };

// コレクション式はダメになる。
A a2 = [1, "2"];

// Add だけは string 受付。
class A : IEnumerable<int>
{
    public void Add(int x) { }
    public void Add(string x) { }

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
```

これが、非ジェネリックな `IEnumerable` を使うと object のみ受け付けるようになるみたいです。
しかもこれ、 Visual Studio 17.10 以前であれば受け付けていたコードがコンパイル エラーになるというひと悶着あり。

* [False positive for CS1503 with MSBuild 17.10, but not dotnet build #72098](https://github.com/dotnet/roslyn/issues/72098)

```csharp
using System.Collections;

// 旧来のコレクション初期化子は string を受け付けるのに…
A a1 = new() { 1, "2" };

// これ、ちょっと前まで受け付けていたらしい。
// Visual Studio 17.10 Preview 1 だとエラー。
A a2 = [1, "2"];

// non-generic なインターフェイスを実装。
class A : IEnumerable
{
    public void Add(int x) { }
    public void Add(string x) { }

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
```

[意図した破壊的変更](https://github.com/dotnet/roslyn/blob/main/docs/compilers/CSharp/Compiler%20Breaking%20Changes%20-%20DotNet%208.md#collection-expression-target-type-must-have-constructor-and-add-method) (たぶん、[1/8 の LDM での決定](https://github.com/dotnet/csharplang/discussions/7832))だそうですが、
本当にこの変更をしてよかったのかどうか。
こういう非ジェネリック `IEnumerable` だけ実装して、`Add` でちゃんとした型を指定しているクラス、
WPF とか WinForms には結構あって、それが突然コンパイルできなくなったものでちょっとした混乱が起きています。

ちなみに、この変更の理由は、こうしておかないと [`params` コレクション](../params-collections/index.md)を使った時のオーバーロード解決のコストが高くなるからだそうです。
制限を緩めるとして、もしかしたら「コレクション式では使えるけども `params` コレクションでは使えない」みたいな状況が増えるかもしれません。

一方、そもそもとして `IEnumerable` 実装は必要なのかという問題が。
何せ、コレクションを作る時点では `GetEnumerator` は要らず、`CollectionBuilder` 属性で指定した `Create` メソッドだけあれば事足ります。
例えば、型によっては「別のコレクションを作るための足掛かりにするもので、直接列挙はしない」みたいなものがあります。
(実際、Roslyn チーム自身が1件そういう問題を踏んだりしています: [CSharpTestSource](https://github.com/dotnet/roslyn/blob/026c96327b02c5ce4d3208f821e02d2ffa825312/src/Compilers/Test/Utilities/CSharp/CSharpTestSource.cs#L22)。`SyntaxTree[]` を作るために使っていて、この型自体からの列挙はしない)。

ということで、`CollectionBuilder` 属性指定のコレクション型の場合、
`Create` メソッドの引数の `ReadOnlySpan<T>` から要素の型を決めようという提案が出ています。

* [Open issue: relax requirement that type be enumerable to participate in collection expressions #7744](https://github.com/dotnet/csharplang/issues/7744)
