---
title: "ピックアップRoslyn 2/13"
source_url: "https://ufcpp.net/blog/2017/2/pickuproslyn0213/"
content_type: "BlogEntry"
published_at: "2017-02-13T20:34:18"
updated_at: "2017-02-13T20:34:18"
tags: []
umbraco_id: 2044
parent_id: 2036
sort_order: 3
aliases: []
---

# ピックアップRoslyn 2/13

[2/10のブログ](../pickuproslyn0110/index.md)の補足。

[csharplangリポジトリ](https://github.com/dotnet/csharplang)内にいくつか提案ドキュメントが上がり始めたというものの中で、2点ほど取り上げて紹介。この2個だけ、ちょっと以前からの進展があったものです。

## 読み取り専用参照

- [Readonly references](https://github.com/dotnet/csharplang/blob/master/proposals/readonly-ref.md)

最近、C#でも構造体を使ったパフォーマンス改善をいろいろやろうとしているわけですが。
[参照戻り値](../../../../study/csharp/resource/sp_ref.md#ref-returns)とかはそのための機能ですし、
[タプル](../../../../study/csharp/datatype/tuples.md)は内部的にmutable(書き換え可能)な構造体になっています(パフォーマンス的にはそれが一番いい)。

ただ、大き目の構造体の受け渡しは、値渡し(コピーが発生)の負担が大きいです。
なので、例えば以下のように、参照引数を使ったりします。

```csharp {title="大き目の構造体を参照渡し"}
static void AddTo(ref Matrix4x4 x, ref Matrix4x4 y)
{
    x.M11 += x.M12;
    // 後略
    // 4×4行列なので15行ほど同じようなの
}
```

ここで問題は、このコード、`x`と`y`のどちらが書き換わるのかわからないところ。
`x`の方は書き換える前提で参照渡しをしていますが、
`y`の方は書き換えるつもりがない。
でも、値渡しするとコピー負荷が高いんで、やむなく参照渡しにしている。
という状態。

こういうのは、意図を明示できるべきだし、もし意図に反して`y`を書き換えようとしたらコンパイル エラーになるべきです。
そこで、「読み取り専用参照」が提案されていました。
C++ならよくやるやつです。`const T&`。
C#に対するこれまでの提案では`readonly ref`なんかが上がっていたんですが、
今回`in`キーワードを使うのはどうだろうという話になりました。

```csharp {title="in参照渡し" highlight-text="in"}
static void AddTo(ref Matrix4x4 x, in Matrix4x4 y)
{
    x.M11 += x.M12;
    // 後略
}
```

前々から`readonly ref`だと長すぎて嫌だしという話はでていまして。
要するに、

- 参照渡しの特殊形として[`out`](../../../../study/csharp/resource/sp_ref.md#out)引数があるんなら、その逆の`in`引数があってもいいじゃない
- `in`なら、`foreach`やジェネリックの[反変性](../../../../study/csharp/oop/sp4_variance.md#in_out)で使ってて今もキーワードだし(破壊的変更になりにくい)

ということで、`in`参照引数にしてはどうかということになっているみたいです。

## `IntPtr`に対する演算子

- [Operators should be exposed for System.IntPtr and System.UIntPtr](https://github.com/dotnet/csharplang/blob/master/proposals/intptr-operators.md)

去年の年末に「[小ネタ](../../../2016/12/tipsprimitives/index.md)」で書きましたが、`System.IntPtr`と`System.UIntPtr`はプリミティブ型です。
要するに、専用の[IL](../../../../study/il/summary/il_about.md)を持っていて、高速な計算ができる型です。
というか、`IntPtr`、`UIntPtr`は、ILの内部的にはnative int, native unsigned intという名前の型になっています。
CPUのバスサイズ(32ビットCPUであれば32ビット、64ビットCPUであれば64ビット)の、実行環境依存の整数型。

最近だと、C#もいろんな場所で動かすようになってきました。
Xamarinや.NET Coreのおかげでクロスプラットフォームになっています。
その結果、このnative intをC#上でも使う機会が増えています。
(まあ、クロスプラットフォームを意識したnative相互運用をしたりといった限られた用途ですけども。
以前よりは必要性が高まっているのは確かです。)

ところが、C#上では、`IntPtr`、`UIntPtr`に対する演算子が一切使えない。
IL的には持っている専用命令を一切活用できない。
`#if`でプラットフォームごとに`int`もしくは`long`にしてから計算とかが必要でした。
ということで、`IntPtr`、`UIntPtr`に対して、一通りの演算子を使えるようにしたという話が出ているようです。
