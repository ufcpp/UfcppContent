---
title: ".NET Core 2.1 の JIT 最適化の話"
source_url: "https://ufcpp.net/blog/2018/3/jitoptimization2_1/"
content_type: "BlogEntry"
published_at: "2018-03-18T00:59:53"
updated_at: "2018-03-18T01:01:24"
tags: []
umbraco_id: 2138
parent_id: 2134
sort_order: 2
aliases: []
---

# .NET Core 2.1 の JIT 最適化の話

## 唐突ですが問題

とある構造体、例えば以下のようなものがあったとします。

```csharp
struct X : IDisposable
{
    public bool IsDisposed;
    void IDisposable.Dispose() => IsDisposed = true;
}
```

この構造体 `X` の `Dispose` メソッドを呼び出すにあたって、
以下の3つのうち、一番高速なのはどれでしょう。

```csharp
    // (1) インターフェイス引数で受け取って呼ぶ
    public static void Interface(IDisposable x) => x.Dispose();

    // (2) X のまま受け取って、メソッド内でインターフェイスにキャストして呼ぶ
    public static void NonGeneric(X x) => ((IDisposable)x).Dispose();

    // (3) ジェネリックなメソッドで受け取って呼ぶ
    public static void Generic<T>(T x) where T : IDisposable => x.Dispose();
```

## 解答

基本的に、というか、これまでは、(3) が最速です。

(1) は、構造体のインスタンスを引数に渡そうとした時点で[ボックス化](../../../../study/csharp/resource/rmboxing.md#boxing)が発生。
ヒープ確保と値のコピーが発生するので、結構な遅さになります。

(2) は、結局`((IDisposable)x)`と書いた時点でボックス化が発生するので、(1)と大差ない速度になっていました。
過去形なのは、.NET Core 2.1でこいつに関する最適化が入ったみたいで、これが実は今日の本題。

(3) だと、ボックス化は発生しません。
.NET の[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)は構造体に対しては型1つ1つに対してそれぞれメソッドを展開するような最適化を行います。
そんな大変なことをやる設計にしたのは、こういうボックス化を避けるためです。
大変なことした甲斐あって、(1)の数倍高速です。
(手元で取ったベンチマークだと6倍くらい高速。)

ベンチマークはGistに置いてあります:

- [構造体のインターフェイス明示的実装の devirtualize 問題](https://gist.github.com/ufcpp/fef52f31e11e0bb11ccbd22638e2dfc0)

## devirtualize最適化

すでに少し触れていますが、この(2)が、.NET Core 2.1で実行すると(3)と同程度の速度が出るようになったみたいです。

要するに、ジェネリックなメソッドの時にやっているのと同じような最適化を、単に`((IDispose)x)`と書いた時にも行うようになったみたいです。

- [JIT: after devirtualization try undoing box and invoking unboxed entry #14698](https://github.com/dotnet/coreclr/pull/14698)

devirtualize (脱・仮想化)ってのは、要は、「`Dispose`は仮想メソッドだから本来は仮想呼び出しされるべき。それを、直接的な呼び出しに戻す」と言うような感じ。
構造体に対してこれが効くと、単に仮想呼び出しのコストがなくなるだけじゃなくて、
ボックス化自体が消滅する(結果、ヒープ確保がなくなる)ので、相当効果の大きい最適化になります。

### また「ref」を忘れてる…

Merge 日時を見ての通り、まあ、結構前からだったみたいなんですけども。
気付いたのは、以下のプルリクを見て。

- [JIT: remove boxing for interface call to shared generic struct #17006](https://github.com/dotnet/coreclr/pull/17006)

どうも、値渡しの時にしか上記の最適化が効いていなくて、「参照渡しでも同じこと効くようにしないとダメだろ」っていう不具合報告が入ったみたいでして。

```csharp
    // これは devirtualize される(去年の10月から)
    public static void NonGeneric(X x) => ((IDisposable)x).Dispose();
    // これは devirtualize し忘れてた(今出てるプルリクで治る)
    public static void NonGeneric(ref X x) => ((IDisposable)x).Dispose();
```

[ref](../../../../study/csharp/resource/sp_ref.md#sec-byref) がらみ、
「動作確認が漏れてて後から気付いて修正」みたいなのかなり多いんですよねぇ…

### そういうとこだぞ

ジェネリックな場合に対してこの最適化が掛かっていたのを見ての通り、
この devirtualize っていう手法は大昔から知られている最適化です。
が、まあ、.NET はなんか「JITが頑張らなくてもジェネリクス使えば速いよ？」(実際、ちゃんと書けばほんとに速い)みたいなところがありまして。
JIT は案外さぼってることが結構ありました。

.NET Core 2.1 だと、JIT でも頑張ろうという感じの動きが結構見られます。

もちろんその分、JIT 自体が遅くなったりはするんですが…
そこは、.NET にも [Tiered JIT](http://mattwarren.org/2017/12/15/How-does-.NET-JIT-a-method-and-Tiered-Compilation/) (Java でいう[HotSpot](https://ja.wikipedia.org/wiki/HotSpot)みたいな、段階最適化)を導入することにしたみたいです。

ちなみに、JIT が頑張れば必ず速くなるってものでもないんですけども。
[Escape Analysis](../../../2016/12/tipssmallheapallocation/index.md)みたいに、「.NET だと構造体を使うのが普通で、JIT が頑張る余地が大して残らない」、「頑張った分のコストでかえって損」的なこともあり得たりします。
