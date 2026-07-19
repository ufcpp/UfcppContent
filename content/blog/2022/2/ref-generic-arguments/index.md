---
title: "【C# 11 候補】 ref 型引数"
source_url: "https://ufcpp.net/blog/2022/2/ref-generic-arguments/"
content_type: "BlogEntry"
published_at: "2022-02-07T23:34:12"
updated_at: "2022-02-07T23:34:12"
tags: []
umbraco_id: 2414
parent_id: 2411
sort_order: 2
aliases: []
---

# 【C# 11 候補】 ref 型引数

今日は「low level」系統の話3個目。

* 1個目: [【C# 11 候補】 ReadOnlySpan 最適化](../span-optimization/index.md)
* 2個目: [【C# 11 候補】 params Span](../params-span/index.md)

## ref 構造体の制限

今日もさかのぼること C# 7.2 の頃、[`Span<T>` 構造体](../../../../study/csharp/resource/span.md)が入ったときの話から。

`Span<T>` 構造体は内部に `ref` フィールド的なものを持っていて、
変なところ(例えばもう解放したあとの不正な場所)を参照したりしないよう、ヒープ上にコピーできないという制限が掛かっています。
(詳しくは[`ref` 構造体](../../../../study/csharp/resource/refstruct.md)で説明しています。)

その制限が守られているかどうかはコンパイラーがちゃんとチェックしているので安全に使えます。
ただ、C# 7.2 時点ではコンパイラーのチェックがまだ貧弱で、
過剰防衛気味になっています。
すなわち、コンパイラーや .NET ランタイムがもう少し頑張れば、
ref 構造体に掛かっている制限は多少緩めることができます。

その「過剰防衛」のうちで深刻なのが以下の2つ。

* [ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)の型引数に使えない
* インターフェイスを実装できない

## デリゲートの型引数に ref T

で、昨年10月にあった low level hachathon でその制限を緩めるプロトタイプ実装の1つとして、以下のような pull request が出ていました。

* [Support 'ref' type arguments in delegates](https://github.com/dotnet/roslyn/pull/57135)

デリゲートはフィールドを一切何も持っていないことがわかっているので、
型引数に `ref` 構造体を与えても実は問題を起こさないだろうことがわかっています。

そして .NET の型システムは C# のレベルの型システムよりも元からチェックが甘いので、元から `ref` 構造体や `ref T` を型引数として与えることができるようです。

ということで、デリゲートに対して `ref T` を型引数として与えられるようにしたのが上記 pull request。
以下のようなコードが書けるようになります。

```csharp
Func<ref int, ref int> f = (ref int x) => ref x;
```

デリゲートだけ特別扱いと言うのがちょっと気持ち悪くはあるんですが…

C# 10.0 の[デリゲートの自然の型](../../../../study/csharp/functional/sp_delegate.md#natural-type)の仕様も十分気持ち悪いですからね…
「`Action` や `Func` で表現できないものは匿名の型を作る」みたいなことをしていて、これはこれで微妙です。

```csharp
// C# 10.0 時点では Func<ref int, ref int> というデリゲートは作れないので、
// しょうがないので delegate ref int Anonymous(ref int x) という匿名のデリゲート型を作ってる。
var f = (ref int x) => ref x;

int x = 10;
ref var y = ref f(ref x);
```

## ジェネリック型引数に ref 構造体

デリゲートだけ特別扱いではやっぱりまだ制限が厳しすぎるわけですが。

具体的には、以下のような事をしたいという需要が結構高いです。

* `Span<Span<T>>` みたいな、入れ子 `Span` を作りたい
* `Span<T>` に `ISpanFormattable` インターフェイスを実装させたい

ということで、これを認めるべく、提案ドキュメントが上がっています。

* [Support for ref struct as generic arguments](https://github.com/dotnet/csharplang/pull/5689)

これで、例えば以下のようなコードを書けるようになります。

```csharp
class Writer
{
    void Write<T>(T value)
        where T : ref struct, ISpanFormattable
    {
        Span<char> buffer = stackalloc char[100];

        // Constrained interface call which does not box
        if (value.TryFormat(buffer, out var written, default, null))
        {
            this.Write(buffer);
        }
    }

    void Write(ReadOnlySpan<char> data) { /* 省略 */ }
}
```

ここでちょっと気持ち悪い点が1つあるんですが…
`where T : ref struct` は「アンチ制約」になっています。

普通、`where` で制約を掛けると「何も付けないときよりも渡せる型が減る」という状態になります。
例えば `where T : class` と書くと参照型しか渡せなくなります。

一方、`where T : ref struct` の場合は、「無制約だと渡せなかった `ref` 構造体を渡せるようになる」ということで、「何も付けないときよりも渡せる型が増える」ということになります。
制約が増えてるのではなく減っているので「アンチ制約」。

なので、もしかしたら `where` とは逆の単語、例えば `allow` とかを新キーワードとして追加すべきなのかもという話もあったりします。
