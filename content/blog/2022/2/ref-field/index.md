---
title: "【C# 11候補】 ref field"
source_url: "https://ufcpp.net/blog/2022/2/ref-field/"
content_type: "BlogEntry"
published_at: "2022-02-11T21:53:43"
updated_at: "2022-02-11T21:53:43"
tags: []
umbraco_id: 2415
parent_id: 2411
sort_order: 3
aliases: []
---

# 【C# 11候補】 ref field

今日は「low level」関連4個目。

* 1個目: [【C# 11 候補】 ReadOnlySpan 最適化](../span-optimization/index.md)
* 2個目: [【C# 11 候補】 params Span](../params-span/index.md)
* 3個目: [【C# 11 候補】 ref 型引数](../ref-generic-arguments/index.md)

今日は ref フィールドとか、構造体を使ったパフォーマンス改善系の話。

* [Low Level Struct Improvements](https://github.com/dotnet/csharplang/blob/main/proposals/low-level-struct-improvements.md)

昨年10月の low level hackathon で何かプロトタイプ実装があったわけじゃないですし、提案自体は2020年からあります。
ただ、昨年10月頃から本腰を入れて動き始めているみたいで、
.NET 7 / C# 11 でのリリースに向けて割かし前向きみたいです。

## Span 構造体の中身

[C# 7.2](../../../../study/csharp/cheatsheet/ap_ver7_2.md) の頃に入った [`Span<T>` 構造体](../../../../study/csharp/resource/span.md)ですが、理屈上は以下のような構造体です。

```csharp {title="Span&lt;T&gt; の理屈上の中身" highlight-text="ref T _field"}
readonly ref struct Span<T>
{
    private readonly ref T _field;
    private readonly int _length;
}
```

配列とか、`stackalloc` で確保したメモリ領域の先頭を `ref` で持っています。

「理屈上は」と前置きしているのは、この当時 (というか C# 10.0 現在でも)、C# には `ref` をフィールドに持つ機能がありません。

ちょこっと背景的な話をすると、

* 初期プロトタイプ時点では、`Span<T>` の中身はポインター (unmanaged なやつ)で実装していた
* それで劇的なパフォーマンス改善が得られることが実証された
* その後、やっぱりポインター(ガベージ コレクションのトラッキング対象にならない)だとダメで、マネージ参照(要するにガベコレ対象にしたい)が必要という話になった
* `ref` フィールドの追加は負担が大きいので、`ByReference<T>` という特殊な internal 構造体を用意して、それを .NET ランタイム内で特別扱いしてしのいだ

という経緯があります。

ということで、C# 10.0 / .NET 6 時点での `Span<T>` の中身は概ね以下のようになっています。

```csharp {title=".NET 6 時点の Span&lt;T&gt; の理屈上の中身"}
public readonly ref struct Span<T>
{
    private readonly ByReference<T> _pointer;
    private readonly int _length;
}

internal readonly ref struct ByReference<T>
{
    // 形式上こんな定義が入っているものの、ランタイム内で特殊処理して ref T に置き換えてる。
    private readonly IntPtr _value;

    [Intrinsic]
    public ref T Value => throw new PlatformNotSupportedException();
}
```

## ref がらみの改善

この `Span<T>` を導入した当時から、
`ByReference<T>` の特別扱いがだいぶ「やっつけ」っぽいことは百も承知です。
「いつかは直すべきだが、差し当たって一番需要が高い `Span<T>` をリリースすることの方が先決」という判定です。

そしてその「いつか」が今ついに来たというのが冒頭で紹介したこの提案。
`Span<T>` / C# 7.2 が2017年末のことなので、実に5年ぶりの low level の機運。

* [Low Level Struct Improvements](https://github.com/dotnet/csharplang/blob/main/proposals/low-level-struct-improvements.md)

この提案には複数の機能・目標が含まれていて、以下のようなものがあります。

* `ref` 構造体に `ref` フィールドを持てるようにする
* `ByReference<T>` の特別扱いをやめて、`Span<T>` などを普通に `ref T` を使った実装に置き換えれるようにする
* `ref` 構造体が `this` 参照を [`ref` 戻り値](../../../../study/csharp/resource/sp_ref.md#ref-returns)で返せるようにする
* 現在認められていない `new Span<T>(ref T reference, int length)` みたいなものを、unsafe なしで作れるようにする
* 固定長バッファーを unsafe なしで作れるようにする

`Span<T>` を先にリリースして、`ref` フィールドが後なので、
互換性のために、エスケープ解析が多少複雑になっている感じはありますが…

ちなみに、C# コンパイラー側だけじゃなく、
ラインタイム側の作業も結構必要になります。
以下のものがトラッキング用の issue。
今日の時点でも結構完了済み。

* [ref field support in .NET runtimes](https://github.com/dotnet/runtime/issues/63768)
