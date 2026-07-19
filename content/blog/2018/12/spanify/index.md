---
title: "Span<T> 利用による最適化"
source_url: "https://ufcpp.net/blog/2018/12/spanify/"
content_type: "BlogEntry"
published_at: "2018-12-25T09:27:42"
updated_at: "2018-12-25T09:27:42"
tags: []
umbraco_id: 2208
parent_id: 2177
sort_order: 25
aliases: []
---

# Span<T> 利用による最適化

このブログではたびたび「.NET Core 2.1 上で動かすだけで、アプリ側には何も手を加えなくても 2.0 の頃より1・2割高速になる」みたいな話をしています。

今月に入ってからは、[Devirtualization](../devirtualization/index.md)みたいなJIT時の最適化手法や、
逆にもっと[小手先の細かな最適化](../arrayindex/index.md)の話も書いてきました。
.NET Core 2.1 ではこういういろいろな最適化が入っているんですが、
その中でも一番パフォーマンス改善に効いていそうなのが[`Span<T>`構造体](../../../../study/csharp/resource/span.md)の導入です。

`Span<T>`構造体自体の説明は何度かしていますが、
`Span<T>`を使ってどういう修正をしているかについてはあんまり書いていないので、
今日は実例をいくつか挙げていこうかと。

## ヒープ使用量の削減

`Span<T>` を使うと速くなる理由は単純で、
ヒープの使用量を減らせるからです。

- `string.Substring` などで新しい文字列を作らなくて済む
- `stackallock` で、一時バッファーにヒープを使わなくて済む
- ネイティブ メモリを直接読めるようになったことで、マネージ配列にコピーしなくて済む

いずれも、unsafe コードでポインターを使えばこれまでも十分に実現できたものです。
しかし、安全性・生産性を犠牲にしたコードは書くのも使うのも神経を使うので大規模には導入しにくですし、
[ガベコレ都合](../unsafe/index.md)の制限もあって、
`Span<T>` なしでは難しい最適化です。

`Span<T>` もいろいろと制限の掛かった特殊な型([ref struct](../../../../study/csharp/resource/refstruct.md))ですが、それでもポインターよりは適用可能な範囲が広いです。

## Substring

.NET の `string.Substring` は、新しい `string` 型インスタンス(もちろんヒープを使う)を作ってそれを戻り値に返します。

[下手に仮想呼び出しが増えるよりは、無駄にヒープを使っちゃう方が高速](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2018/SpanPerformance/SubstringBenchmark/AbstractString.cs)だからそういう作りなんですが、
`Span<char>` があればヒープを使わず似たようなことができます。

ということで、`Substring`を`AsSpan`にちまちま変更していくようなプルリクエストが。

- [Avoid substring allocations in WebUtility.HtmlDecode #29402](https://github.com/dotnet/corefx/pull/29402)
- [Replace easy Substrings with AsSpan/Slices #17916](https://github.com/dotnet/coreclr/pull/17916)

`Substring` に限らず文字列操作がらみはかなり`Span<T>`の恩恵を受けていて、
倍以上速くなったメソッド何かもあるみたいです。

## stackalloc

極々短い範囲で、小さいデータを持っておくだけの一時バッファーを必要とすることは結構あります。
そんな時、これまでだと配列(ヒープを使う)を使っていたんですが、
`Span<T>` があれば [`stackalloc`が安全になる](../../../../study/csharp/resource/span.md#safe-stackalloc)ので、
ヒープ利用を避けることができます。

以下の修正では、固定長で2文字の`char`のために`new char[2]`していたものを`stackalloc`に置き換えています。

- [Remove char[] allocation in CheckIriUnicodeRange #33641](https://github.com/dotnet/corefx/pull/33641)

<pre class="source" title="配列を stackalloc に置き換え">
<code><span class="comment">//before</span>
<span class="reserved">char</span>[] chars = <span class="reserved">new</span> <span class="reserved">char</span>[2] { highSurr, lowSurr };
 
<span class="comment">//after</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; chars = <span class="reserved">stackalloc</span> <span class="reserved">char</span>[2] { highSurr, lowSurr };
</code></pre>

ただし、.NET の実装では、メモリのスタック領域は固定長で 1MB くらい(確か)なので、
あんまり大きなデータをスタックに置こうとすると簡単に stack overflow を起こしたりします。
先ほどのような固定長で短いデータはいいんですが、可変長の場合にはひと工夫必要です。

具体的には、要するに「一定サイズ以下の時にだけ`stackalloc`を使う」という分岐を挟むだけなんですが。
以下のプルリクエストなんかはわかりやすいです。

- [Add datetime read span path for netcore #31044](https://github.com/dotnet/corefx/pull/31044)
- [Improve performance of BigInteger.ToString("x") #25353](https://github.com/dotnet/corefx/pull/25353)

以下のような条件演算子は結構頻出です。

<pre class="source" title="データが短い時だけ stackalloc">
<code><span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; datetimeBuffer = ((<span class="reserved">uint</span>)length &lt;= 16) ? <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[16] : <span class="reserved">new</span> <span class="reserved">byte</span>[length];
</code></pre>

ちなみに、以下のような型もあります(今のところ internal ですが)。
[`StringBuilder`](https://source.dot.net/#System.Private.CoreLib/shared/System/Text/StringBuilder.cs,adf60ee46ebd299f)相当の処理を、
初期バッファーを`stackalloc`、その後容量を増やすときには[`ArrayPool`](https://source.dot.net/#System.Private.CoreLib/shared/System/Buffers/ArrayPool.cs,87992df74cbf00ad)を使う実装。
これも、「一定サイズ以下の時にだけ`stackalloc`を使う」最適化の一種です。

- [ValueStringBuilder](https://github.com/dotnet/corefx/blob/df442de5b68264d7d129f3a11a265f88edef3fb0/src/Common/src/CoreLib/System/Text/ValueStringBuilder.cs)

## ネイティブ メモリを直接

`Span<T>` を使うと、は配列でも、`stackalloc` で確保したスタック領域でも、
ネイティブ メモリでも共通処理が書けます。
なので、[ネイティブ相互運用](../../../../study/csharp/interop/sp_pinvoke.md)時に、
C# 側で一時配列を確保してネイティブ コードにポインターを渡す以外に、
ネイティブ側からポインターを返してもらってそれを C# 側で`Span<T>` を介して処理するということもできます。

これも、一時バッファーの確保が不要になるのでパフォーマンス改善につながったりします。

- [Use FORMAT_MESSAGE_ALLOCATE_BUFFER with FormatMessage](https://github.com/dotnet/corefx/commit/e118304d264163f03e79965e4f2ab1d5c1a43961#diff-f302191909b0568b3dca0f6f6b2f7de1R62)

最近だと、[ML.NET](https://github.com/dotnet/machinelearning)内で、
[TensorFlorとの相互運用](https://github.com/dotnet/machinelearning/blob/e2e1aa8a2b43aa0b5ea5d8b3851b6a0d175f7916/src/Microsoft.ML.TensorFlow/TensorflowTransform.cs)でもネイティブ メモリの読み書きに `Span<T>` を使っていたりします。
