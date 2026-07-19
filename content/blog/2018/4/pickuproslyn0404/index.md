---
title: "ピックアップRoslyn 4/4 C# 8.0辺りの検討事項"
source_url: "https://ufcpp.net/blog/2018/4/pickuproslyn0404/"
content_type: "BlogEntry"
published_at: "2018-04-04T23:19:36"
updated_at: "2018-04-04T23:23:06"
tags: []
umbraco_id: 2145
parent_id: 2144
sort_order: 0
aliases: []
---

# ピックアップRoslyn 4/4 C# 8.0辺りの検討事項

[Design Notes の一斉アップロード祭り](../../3/pickuproslyn0321/index.md)がらみ、今日でやっと最後。
[先月28日に1つ追加の Note もあります](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-03-28.md)。

とりあえずこれまでの分:

- [C# 8.0よりさらに先を見越して、いくつかの機能の採用が決定](../../3/pickuproslyn0321/index.md)
- [一番分量が多かった Ranges (C# 8.0 予定)の話](../../3/ranges/index.md)
- [C# 7.3 で入りそうな機能の話](../../3/pickuproslyn0331/index.md)

で、今日は最後に、C# 8.0 辺りで入りそうな機能の話になります。

## Caller Expression Attribute

[Caller Info 属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)に1種類追加。
`M(2 * x, y * y, Sin(z));` みたいなメソッド呼び出しをした時に、`2 * x, y * y, Sin(z)` の部分をそのまま受け取るというデバッグ用の機能です。
主な用途は Assert 系の API。

で、既存の Assert API がこの機能を活用しようと思ったら、
`Equal(T expected, T actual)` みたいなメソッドを
`Equal(T expected, T actual, [CallerExpression]string expression = null)` みたいなシグネチャに変える必要があります。

こいつの呼び出し側は、元のまま `Assert.Equal(x, y);` で呼べばいいんですが…

- 既存の `Equal(T expected, T actual)` を残すと、こっちの方が優先度が高いのでこっちしか呼ばれない
- 既存の `Equal(T expected, T actual)` を消すと、ソースコード互換はあるけどバイナリ互換はない(一応、破壊的変更)
    - ソースコードの再コンパイルなしで、Assert ライブラリの DLL だけ更新すると実行できなくなる

という問題が。
XUnit は昔この手の引数追加での破壊的変更を1度やっているらしく、まあ、許容できると言えばできそう。


## null 許容参照型

このブログでもたびたび紹介していますが、参照型も `T` (null なし)と`T?` (null あり)の区別がつくようになるやつ。

待望の機能ですし、すでにプロトタイプ実装も始まっていますが、やっぱ細々と検討事項が出てきます。

### 明示的なキャストの扱い

`(T)x` とか `(T?)x` とか、キャストをどう扱うかというのも案外自明ではないみたい。
特に、既存コードからの移行の都合と、新たに書き始めるのであれば追求したい「正しさ」との間のバランスで悩むとか。

- 既存コードには `(string)null` とかで「型指定ありの null」とかは結構書くけども
    - これは自動的に`string?`扱いすべきか(既存コード優先)
    - 警告すべきか(正しさ優先)

`M((string)x)` と `M((string?)x)` みたいなキャストは、ジェネリック メソッドの呼び分けでははっきりとキャストが意味を持ってる。
例えば`T M<T>(T x)` に対して、
`var y = M((string)x)` とすると `y` も `string` (nullなし)になるし、
`var y = M((string?)x)` とすると `y` が `string?` (nullあり)になる。

一方で、`M`が旧時代のコード(`T` と `T?` の区別をしない時代のコードでは、`T` から null が帰ってくることがあるし、
コンパイラーもそのつもりで null チェックをする)だった場合、
`M((string)x)` はどう解釈すべきか。
`x` が新時代(`T?`あり)コードなら警告でいいけども、
`x` も旧時代コードならこの`string`はどう扱うべきか。

### 警告の種類

古いコードをアップグレードするとき、一気にはコードを修正できないので `#pragma warning` などを使って警告を無視したいことがあり得る。
この「警告オフ」作業を煩雑にしないために、null チェック絡みの警告の種類はまとめられる限りまとめたい。

実際、「null リテラルを `T` に代入」と、「`T?` 型変数の値を `T` に代入」は統合したみたいです。

### 型推論

`T?` 型の変数であっても、null チェックがあったらそれ以降は「null ではない」という扱いになります
(コンパイラーがそういう風に判断して警告の有無が決まる)。

例えば、以下のような判定になったりします。

```csharp
string? ns = ...
if (ns is null) return; // null だったらここから下にはいかない
var s = ns; // なので、s は「非 null」
```

ここで問題なのは、じゃあ、この `s` は「`string?`だけどnullチェック済み」という判定なのか、
「`string`型として推論されてる」という状態なのか。
ジェネリック メソッドに `s` を渡した時のオーバーロード解決とかにも絡んでくる。

### var?

`var s1 ="Hello";` とか書いたとき(`string`扱い？)、`s1` に後から `null` を代入したい場面ではどうするべきか。

`var s2 = (string?)"Hello";` は、「`var`と書くと非nullとして推論」として認めないようにするか、`string?`として推論すべきか。

「`var`と書くと非null」の対として、`var? s3 = "Hello";` みたいな書き方を用意する？

## Records

C# 6.0の頃から案だけはあるものの、気が付けば C# 7.X でも入らず 8.0 に伸びてる Record 型。
`class Record(int X, int Y);` とか書くと、プロパティ `X`、`Y` とか `==`、`Deconstruct` を自動で実装してくれるというやつです。

先月の [Microsoft MVP Global Summit](https://mvp.microsoft.com/ja-jp/Summit) でやっぱ [MVP](https://www.microsoft.com/ja-jp/communities/mvp/) からさんざん突っ込みが入ったそうで。それに対する回答:

- 重要性はわかっているけども、現実主義的に行きたい(要するに、課題も多くて完成には及ばず)
- [Discriminated Unions](https://github.com/dotnet/csharplang/issues/113)と併せて取り組みたい。別機能ではあるものの、Records と一緒に取り組むとより良くなる
- いくつか「用途が狭い」と判定された提案は「[コード生成でやってくれ](https://github.com/dotnet/csharplang/issues/341)」と言ってきてるが、Records に関して言うとコード生成の領分ではない。ちゃんと言語機能として取り組むべき

## Ranges

[28日に追加分](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-03-28.md)でも Ranges に関する記述あり。

### プロトタイプ提供するにあたって

[Ranges](../../3/ranges/index.md) 構文はあくまで `Range` 型を作るという機能であって、`Span<T> span = array[1..3];` みたいな書き方をするためには、配列や`List`が`Range`型を受け付けるインデクサーを持っていないといけない。

で、この機能のプロトタイプを提供するにあたって、こういう「`Range`型を受け付けるインデクサー」も同時にプロトタイプ提供したい。
特に、配列とか`string`とかの型でこれを使いたいわけだけど、そのために .NET Core ランタイム側で対応作業をしてもらう必要があるとなると、
プロジェクト間の依存が大きすぎる。

`Span<T> this[Range range]` インデクサーを被せるだけのラッパー構造体を作ってそれをプロトタイプ提供すべき？

それか、一時的に「拡張インデクサー」を提供してしまう？
(拡張インデクサーは、[Shapes](https://github.com/dotnet/csharplang/issues/164)って機能の一部として提供したけども、
こいつは 8.0 よりもさらに先で検討されてる。Ranges の方が先に実装される。なので、本来 Ranges と同時期には提供しないはずの機能を「一時的に」「仮実装」で提供。)

今のところ「一時的な拡張インデクサー」の提供を考えてる。

### どこまで提供するか

結局どこまでやるか。

- `m..n` で「m から n まで」みたいな開始・終了インデックス型だけを提供
- 「m から len 要素」みたいなやつや、「配列の末尾から n 要素目」みたいなのを考えて、`Index`型と`^`演算子みたいなのも提供

前者なら実装は楽だけどできることの制限が強い。
まずはこの「実装が楽な部分」だけを実装してみて、ユーザーの反応を見てみるという手もある。
でも、ちょっとこの制限はきついと思っていて、たぶん高機能な後者の方の需要はある。

## switch 式の網羅性

「`switch` 式」では網羅性(exhaustiveness)のチェックをしたいという話がある。
例えば `bool` なら(変なことをしなければ) `true` と `false` の2値しか持っていないわけだから、
`x switch { case true: a; case false: b; }` みたいな式は「すべての値を網羅している」と言えるはず。

これをコンパイラーがチェック(網羅されていれば「default 句なし」を認める)すべき？
チェックするとして、(いくつか、コンパイラーの静的チェックに漏れるケースがあり得るけど、その時のために)実行時チェックもする？

今のところ、網羅性をチェックして、網羅できていなければ警告にする。
静的チェックに漏れた場合の実行時チェックも入れて、例外を投げるようにすると思う。

### nested stackalloc

現状、[stackalloc](../../../../study/csharp/resource/span.md#safe-stackalloc)は非同期メソッド内では使えないという制限があります。
まあ、`await` をまたいで使うことは原理的にできない機能ではあります。

```csharp
async Task M()
{
    Span<int> span = stackalloc int[10];

    await Task.Delay(1);

    span[0] = 1; // これは本当に不正。原理的に無理
}
```

でも、`await` さえまたがなければ、例えば以下のような書き方であれば安全に使えるはずです。

```csharp
async Task M()
{
    {
        Span<int> span = stackalloc int[10];
        span[0] = 1;
    }

    // ブロックでくくったので、span がここに漏れることはない
    await Task.Delay(1);
    // ここから下で使えなければ span は安全
}
```

この、「ブロックで囲った(nested) `stackalloc`」を認めたいという感じになっています。
