---
title: "JIT Intrinsics"
source_url: "https://ufcpp.net/blog/2018/12/jitintrinsics/"
content_type: "BlogEntry"
published_at: "2018-12-23T11:16:04"
updated_at: "2018-12-23T11:16:04"
tags: []
umbraco_id: 2205
parent_id: 2177
sort_order: 22
aliases: []
---

# JIT Intrinsics

.NET Core 2.1 では、いくつか、JIT 時の特殊対応によるパフォーマンス改善を行っています。

そういう「特殊対応」を intrinsic (固有の、内在的な、内因的な、本質的な)と呼びます。
「JIT 時の特殊対応」であれば「JIT intrinsic expansions」(固有展開)とか「JIT intrinsics」(s が付くことで名詞化してる。economics とかの s と同じ)と言います。

## Intrinsic 属性

JIT 時特殊対応をしているクラスやメソッドには [`Intrinsic` 属性](https://source.dot.net/#System.Private.CoreLib/shared/System/Runtime/CompilerServices/IntrinsicAttribute.cs,0b1553fdd9183e62)が付いています。
この属性を参照しているものを検索することで、どこで特殊対応が行われているかを追うことができます。

### `Vector`

記憶にある限り、`Intrinsic` 属性が最初に使われたのは
[`Vector<T>` 構造体](https://source.dot.net/#System.Private.CoreLib/shared/System/Numerics/Vector.cs)(`System.Numerics`名前空間)です。

この型は SIMD 演算を行うためのもので、

- SIMD に対応している環境では、SIMD 命令を使った実装に差し替える
- そうでない場合、`vector1.X * vector2.X + vector1.Y * vector2.Y + ...` というような通常の C# コードを使う

と言うような処理をしています。

## .NET Core 2.1 での最適化

.NET Core 2.1 でいくつか intrinsic なものが増えています。
そのうちいくつかを紹介。

### EqualityComparer.Default

[Devirtualize 処理](../devirtualization/index.md)で説明した通り、.NET Core 2.1 では`EqualityComparer<T>.Default`に対して具象型を返す用ような最適化が入っています。

なので、例えば、`EqualityComparer<int>.Default.Equals(1, 2)`みたいなコードは、.NET Core 2.0 よりも、2.1 の方が1桁高速です。

### Enum.HasFlag

`Enum.HasFlag` も .NET Core 2.1 で intrinsic な最適化が掛かったものの1つです。

`HasFlag`相当の処理は、具体的な列挙型がわかっていれば以下のような書き方ができます。
ただの `&` と0比較なので、かなり高速です。

<pre class="source">
<code><span class="reserved">static</span> <span class="reserved">bool</span> HasFlag(<span class="type">A</span> x, <span class="type">A</span> y) =&gt; (((<span class="reserved">int</span>)x) &amp; ((<span class="reserved">int</span>)y)) != 0;

</code></pre>

ところが、任意の列挙型に対して使えるようにしようとすると途端に面倒になります。
`Enum`クラス(`System`名前空間)の`HasFlag`メソッドで機能としては提供されているんですが、
この`HasFlag`メソッドはむちゃくちゃ遅いです。

そこで、.NET Core 2.1ではJIT時に特殊対応するようにしました。
`HasFlag`メソッドを見たら単なる`&`に置き換える処理が掛かっています。
「[Enum.HasFlag でのボックス化](../../../../study/csharp/oop/miscimplictinherit.md#enum-hasflag)」で説明しているように、
.NET Core 2.0以前と2.1以降で実行速度に20倍以上の差があります。

### Span のインデクサー

[`Span<T>`構造体](../../../../study/csharp/resource/span.md)のインデクサーにも JIT 時特殊対応が入っています。

これは、配列の`array[i]`と同じような最適化です。
「[配列のインデクサー](../arrayindexer/index.md)」で説明したように、配列のインデクサーには

- 何もしなければ、`array[i]` のところに暗黙的な範囲チェックを追加する
- 明示的な範囲チェックがあれば、余計な範囲チェックの追加はしない

と言うような処理が掛かっています。
これと同じことを、.NET Core 2.1では`Span<T>`のインデクサーに対しても行っています。
特殊対応なしだと、`Span<T>`のインデクサーは常に範囲チェックを必要としますが、
.NET Core 2.1 以降だと、必要に応じて範囲チェックの削除が行われます。
