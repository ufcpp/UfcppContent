---
title: "ピックアップRoslyn 10/4: C# 9.0, パターン追加、switch 式ステートメント、共変戻り値"
source_url: "https://ufcpp.net/blog/2019/10/pickuproslyn1004/"
content_type: "BlogEntry"
published_at: "2019-10-04T22:36:29"
updated_at: "2019-10-04T22:43:31"
tags: []
umbraco_id: 2269
parent_id: 2268
sort_order: 0
aliases: []
---

# ピックアップRoslyn 10/4: C# 9.0, パターン追加、switch 式ステートメント、共変戻り値

何件か、C# 9.0 向けに提案されている機能のドラフト仕様が出てきました。

- [Proposed changes for Pattern Matching in C# 9.0 - Draft Specification #2850](https://github.com/dotnet/csharplang/issues/2850)
- [Draft Spec for Switch Expression as a Statement Expression in C# 9.0 #2860](https://github.com/dotnet/csharplang/issues/2860)
- [Covariant Return Types - Draft Specification #2844](https://github.com/dotnet/csharplang/issues/2844)

## パターン マッチ

- [Pattern Matching - Draft Specification #2850](https://github.com/dotnet/csharplang/issues/2850)

[C# 8.0 でもずいぶんとパターンが増えました](../../../../study/csharp/cheatsheet/ap_ver8.md#recursive-pattern)が、9.0 でも追加が出そうです。

- 複数のパターンを `and` や `or` でつないだり、`!(x is pattern)` と書かなくても `x is not pattern` と書けるようにしたり
- `and` や `or` があるなら優先度を付けるために、パターンを `()` で囲えるようにしたり
- `x is >= min and <= max` みたいに、比較パターンを入れたり

## switch 式を式ステートメントに

- [Switch Expression as a Statement Expression #2860](https://github.com/dotnet/csharplang/issues/2860)

メソッド呼び出しなど、いくつかの式は、式を単体で `M();` みたいに書いてステートメント化できます。(こういうのを「式ステートメントと言います。)

[`switch` 式](../../../../study/csharp/cheatsheet/ap_ver8.md#switch-expression)でも、以下のような書き方に需要があるので、式ステートメント化をしたいという話は前々からあります。

```csharp
static void A() => Console.WriteLine("A");
static void B() => Console.WriteLine("B");
static void C() => Console.WriteLine("C");
 
static void M(bool? state)
{
    state switch
    {
        true => A(),
        false => B(),
        null => C(),
    };
}
```

C# 8.0 には間に合わなかったので、9.0 での提案に。

### 共変戻り値

- [Covariant Return Types - Draft Specification #2844](https://github.com/dotnet/csharplang/issues/2844)

これは要するに以下のような奴。

```csharp
class Base { }
class Derived : Base { }
 
class A
{
    public virtual Base M() => null;
}
 
class B
{
    // 戻り値が Base じゃなくて Derived。
    // 原理的には問題ないはずだけど、今までの .NET ではできなかった。
    public override Derived M() => null;
}
```

これはずっと「C# 上の構文糖衣ではなく、ランタイムに手を入れた方がいいので難しめ」ということでなかなか手付かずだったやつです。

C# 上の構文糖衣で何とかごまかせないかという検討もしていたんですが、
結局、ランタイム(.NET の型システム自体)の修正込みでやろいうという流れになっています。

[インターフェイスのデフォルト実装](../../../../study/csharp/cheatsheet/ap_ver8.md#default-imeplementation-of-interface)に続く2例目の「ランタイムを選ぶ新機能」になります。
