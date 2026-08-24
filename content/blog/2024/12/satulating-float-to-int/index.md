---
title: ".NET 9 の破壊的変更の1つを踏んだ話"
source_url: "https://ufcpp.net/blog/2024/12/satulating-float-to-int/"
content_type: "BlogEntry"
published_at: "2024-12-18T21:09:06"
updated_at: "2024-12-18T21:09:06"
tags: []
umbraco_id: 2502
parent_id: 2501
sort_order: 0
aliases: []
---

# .NET 9 の破壊的変更の1つを踏んだ話

かなりのレアケースを踏んだので酒の肴程度にその話を。

## 破壊的変更の内容: 浮動小数点数 → 整数の飽和変換

破壊的変更の告知ページ:

* [Floating point-to-integer conversions are saturating](https://learn.microsoft.com/ja-jp/dotnet/core/compatibility/jit/9.0/fp-to-integer)

最小再現コードは以下の通り。

```csharp {title=".NET 8 と 9 で挙動が違うコードの例"}
var x = int.MaxValue;
var y = (float)x;
var z = (int)y;
Console.WriteLine(z);
```

`z` の値は、
.NET 8 では `-2147483648` (`int.MinValue`) になって、
.NET 9 では `2147483647` (`int.MaxValue`) になります。

(注意: `float` の精度の問題で、`y` の値は `int.MaxValue` よりも大きい扱いを受けていそうです。
`double` では2行目を `var y = (double)x + 1;` にすると再現。)

`int` の範囲に収まらない `float` の値を `int` に変換した時の挙動が変わりました。

* 古い挙動: `x < int.MinValue` もしくは `x > int.MaxValue` のとき、`(int)x` は `int.MinValue` になる
* 新しい挙動: `x < int.MinValue` のとき `(int)x` は `int.MinValue`、`x > int.MaxnValue` のとき `(int)x` は `int.MaxValue`

どう見ても新しい挙動の方が自然…

## 破壊的変更をした理由

変な挙動であっても、変更するメリットがそれなりにないと破壊的変更が認められることはほとんどありません。

今回の場合何があったのかというと、AVX512 命令を使いたかったということみたいです。

* [Scalar/Packed conversions for floating point to integer #97529](https://github.com/dotnet/runtime/pull/97529)

AVX512 には `double`, `float` を `int`, `uint` とかに変換するための vfixupimmsd などの命令があるそうで。
ハードウェア命令を持っているんなら、ソフトウェア計算するよりもこの命令を使った方がパフォーマンスがよくなります。
当然、積極的に活用したいんですが、どうも、この命令の挙動が前節の「新しい挙動」になるみたいです。

「AVX512 命令が使える場合だけ挙動が変わる」みたいなことになるとかえってまずいので、だったら、
AVX512 が使えないとき向けのソフトウェア実装の挙動も改めて、
破壊的変更の告知を出してしまおうという流れに。

## 破壊的変更の踏み方

「`int.MaxValue` を超える値を `int` に変換して使う」とか普通はやらないし、
「こんなの影響する人いないでしょ」と高を括っていたものの…

自社のコードに .NET 8 から 9 に変更したら永久ループを起こすコードがありました。
すごく簡素化して書くと以下のようなコードがあったせい。

```csharp {title="飽和変換の仕様変更が永久ループになる例"}
// .NET 9 でだけ永久ループ…
M((int)Math.Floor(float.MaxValue), (int)Math.Floor(float.MaxValue));

Console.WriteLine("done.");

static void M(int x, int y)
{
    // y が int.MaxValue だと、i++ がオーバーフローして永久ループになる。
    for (int i = x; i <= y; i++) ;
}
```

.NET 8 では、`M` の引数に渡る値は `int.MinValue` でした。
それが、.NET 9 になると `int.MaxValue` に変わります。

で、以下のコードは「`for` の中を1回だけ実行」になりますが、

```csharp {title="&lt;= MinVavlue"}
for (int i = int.MinValue; i <= int.MinValue; i++) ;
```

以下のコードは永久ループです。

```csharp {title="&lt;= MaxVavlue"}
// i++ がオーバーフローするので i <= int.MaxValue が false になることはない。
for (int i = int.MaxValue; i <= int.MaxValue; i++) ;
```

背景としては以下のような感じ。

* `record struct Point(float X, float Y)` で座標管理している
* 「無効な座標」が必要になったものの、工数的に `Point?` に書き換える余裕はなかった
* 「すごく遠い座標」を与えてみて、テストプレイしてみたところ所望の動作になったので、`float.MaxValue` の座標に飛ばしてた
* 経路探索ロジックが、パフォーマンスのため、内部的に整数で計算していた
* `for (int i = x; i <= y; i++)` に `(int)float.MaxValue` が来る
* `for` は、`i` が `int.MinValue` だったら何も起きないようなコードだった(ループ内を1回通ってたけども特に問題は起きなかった)

だいぶレアな不幸が重なった感じ…

以下の条件が重ならないと起きないですからね。

* `float.MaxValue` (`int.MaxValue` を超える値)を特殊な意味に使っちゃった
* `<=` でループしてる
* `int.MinValue` のときにはたまたま問題を起こさないコードだった

問題を特定できた時、かなりびっくりしました。
