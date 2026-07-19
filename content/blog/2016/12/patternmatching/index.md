---
title: "パターン マッチング"
source_url: "https://ufcpp.net/blog/2016/12/patternmatching/"
content_type: "BlogEntry"
published_at: "2016-12-16T00:06:23"
updated_at: "2016-12-15T15:06:27"
tags: []
umbraco_id: 1996
parent_id: 1969
sort_order: 14
aliases: []
---

# パターン マッチング

小ネタ休んだついでに、C# 7思い出話。

そういや、[タプル](../../8/tuples/index.md)の辺りまでは、[C# 7思い出話](../../../../search.md#blog-category-f6548b1308f6)とか称して、仕様が固まるまでにあった流れなんかもブログに残そうとしてたのを思い出したり。

[C# 7の紹介](../../../../study/csharp/cheatsheet/ap_ver7.md)、後半は一気に埋めちゃったのもあって、思い出話をどう書こうか考えてるうちに流れてしまったというか。
[ちょっと書くのに苦労した原稿](http://www.buildinsider.net/language/csharpunicode/01)とか、グロサミ参加とか、色々忙しくて忘れていたというか。

とりあえず、今日はパターン マッチングの話。

## パターン マッチングを小分けに

元々予定されていたパターン マッチングの全機能はC# 7の時点では入れれない、段階的に実装していくという話は結構早い段階で決まっていました。

ちなみに、その、予定されている全機能で言うと、以下のような書き方とかもできます。

```csharp
static int Calculate(Node n, int x)
{
    switch (n)
    {
        // この2行はC# 7の時点で書ける
        case Variable v: return x;
        case Constant c: return c.Value;
        // この2行は先送り
        case Add { Le is var l, Right is var r }: return Calculate(l, x) + Calculate(r, x);
        case Mul(var l, var r): return Calculate(l, x) * Calculate(r, x);
    }
}
```

一方で、どこまでをC# 7に入れるかは、徐々に、スケジュールと相談しつつ決めていたみたいで最近まで全然確定していません。

初期は、本当に[型スイッチ](../../../../study/csharp/datatype/typeswitch.md)くらいでした。

続いて、まあタプルが入るんなら[分解](../../../../study/csharp/datatype/deconstruction.md)くらいは要るだろうとなったのか、こいつが実装されます。

[throw式](../../../../study/csharp/structured/oo_exception.md#throwexpr)もパターン マッチングからの派生です。パターン マッチングのお供として「switch 式」みたいなやつを入れることを考えると、「どのパターンにもマッチしなかったら例外を投げる」という処理が必要で、そのための throw 式です。

[分解](../../../../study/csharp/datatype/deconstruction.md)や[out var](../../../../study/csharp/resource/sp_ref.md#out-var)が入るならdiscards (wildcards)もほしいわけですが、これは、`_` を使うか `*` を使うか、既存の文脈でも discards を使えるようにするかどうかとかで悩んでいたみたいで、
本当につい最近実装されています。
[先日書いた](../vs2017rc2/index.md)通り、Visual Studio 2017 RCの初期リリースでは実装されていなくて、Update での実装。

気が付いてみれば、「再帰的な分解」と「switch 式」以外は一通り実装されたのかなぁという感じです。
(再帰的っていうのは、最初に挙げた例でいう`Add { Left is var l, Right is var r }`とか`Mul(var l, var r)`とかみたいなパターンです。)

まあ、それぞれを見ると、細かく「先送り」になっているものもあるんですが。例えば以下のコードは、計画上はできることになっているんですが、現状ではコンパイルエラーになります。

```csharp
// 計画上は、クエリ式の let での分解も予定あり
var q = from x in new [] { 1, 2, 3, 4, 5 }.Select((x, i) => (x, i))
        let (y, z) = x
        select y * z;

// 計画上は、ラムダ式とかの既存の文法にも discards 導入の予定あり
Func<int, int, int> f = (_, _) => 1;

// 計画上は、ラムダ式でも throw 式を書ける予定あり
Action a = () => throw new Exception();
```

もしかしたら、まだリリースまでにこれらの構文も対応するかもしれませんが、断定はできなさそう。
