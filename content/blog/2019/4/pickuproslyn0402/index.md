---
title: "ピックアップRoslyn 4/2: corefx 側とのミーティング"
source_url: "https://ufcpp.net/blog/2019/4/pickuproslyn0402/"
content_type: "BlogEntry"
published_at: "2019-04-02T20:22:37"
updated_at: "2019-04-02T20:22:37"
tags: []
umbraco_id: 2237
parent_id: 2236
sort_order: 0
aliases: []
---

# ピックアップRoslyn 4/2: corefx 側とのミーティング

3/25 に、corefx 側の design review チームと C# チームで C# 8.0 絡みの折衝をしていたみたいです。その議事録が上がりました。

- [C# Design Review Notes for Mar 25, 2019](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-03-25.md)

当然、corefx (.NET の基本ライブラリ)依存があったり、corefx 側での作業が必要になる機能の話になるので、話題は以下の3つ

- [null 許容参照型](../../../2018/12/cs8nrt/index.md) … ライブラリ側にどうやって null 許容注釈をつけて、どうやってリリースしていくか
- [Index/Range](../../../2018/12/cs8ranges/index.md) … ライブラリ側に `Index`/`Range` 型引数のオーバーロードを足すんじゃなくて、パターン ベースに変えたい
- [非同期ストリーム](../../../2018/12/cs8asyncstreams/index.md) … `async foreach` で、`GetAsyncEnumerator` にどうやって `CancellationToken` を渡すか

## null 許容参照型

null 許容参照型は後付けで検証を足すので、オプション指定したり `#nullable` ディレクティブを書いたりしない限り、検証が有効になりません(でないと既存コードを壊す)。

で、検証をオンにして初めて「null 許容かどうかの注釈」(nullability annotation)が入るわけですが、この null 許容注釈をどうやってリリースしていくかが問題になります。

かつて、案としては「sidecar 配布」って呼ばれる方式も検討されていました。
これはバイクにサイドカーをくっつけるみたいに、既存の dll/exe に後付けで(別ファイルで)注釈を足せるようにしたいという方式です。
でも、そういう特別な処理を保守するのは、得られるメリットよりもコストの方が大きいだろうということで、検討から外れたみたいです。

となると、corefx 自体にちまちまと `#nullable` ディレクティブを足して、ちょっとずつ注釈を入れていくことになります。
corefx みたいな大規模なもの対してに一斉作業はできないわけで、
.NET Core のアップデートのたびにちょっとずつ注釈が増えて、使ってる側にはちょっとずつ警告が増えます。

null 許容参照型が普及しきるまでの過渡期には、この「ちょっとずつ警告が増える」を覚悟しておく必要があります。
(その覚悟ができないなら、null 許容参照型を有効化するオプションは指定できない。
というのを、C# チームや .NET チームが周知しておかないとまずい。)

## Index/Range

これは[以前にも取り上げている](../../2/pickuproslyn0210/index.md)んですが、

- 今まで
  - C# 側ではあくまで、`^i` みたいな書き方から `Index` 構造体を、`i..j` みたいな書き方から `Range` 構造体を作るだけ
  - `Index` や `Range` を受け取つけるオーバーロードはライブラリ側の責任で作る
- これから/変えたい内容
  - パターン ベースで、`int`のインデクサーや `Slice` メソッドに展開
  - `Index` 型の `i` に対して `x[i]` みたいなのを、`x[i.GetOffset(x.Length)]` に
  - `Range` 型の `r` に対して `x[r]` みたいなのを、`var (offset, length) = r.GetOffsetAndLength(x.Length); x.Slice(offset, length)` に

みたいな話。
csharplang 側と corefx 側にそれぞれ issue が立っていたりしますが、この日に決まった話みたいです。

## 非同期ストリーム

`await foreach` の展開結果では `GetAsyncEnumeartor(CancellationToken cancellationToken)` メソッドが呼ばれるようになります。
これまで、この、引数の `CancellationToken` に値を渡す手段がなくて、問答無用に `default` (キャンセルはしない)が渡っていました。

で、結局、プロパティの `set` やイベントの `add`/`remove` の `value` 変数(暗黙的に定義済みになってる)と同様に、`cancellationToken` という名前の暗黙的な変数を用意して、それを介して渡そうかという流れになっているみたいです。
