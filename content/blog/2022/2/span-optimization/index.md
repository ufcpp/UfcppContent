---
title: "【C# 11 候補】 ReadOnlySpan 最適化"
source_url: "https://ufcpp.net/blog/2022/2/span-optimization/"
content_type: "BlogEntry"
published_at: "2022-02-01T22:13:09"
updated_at: "2022-02-01T22:13:09"
tags: []
umbraco_id: 2412
parent_id: 2411
sort_order: 0
aliases: []
---

# 【C# 11 候補】 ReadOnlySpan 最適化

[dotnet/runtime](https://github.com/dotnet/runtime)のコミット履歴とかにうっすら痕跡が見て取れるんですが、
去年の10月中旬頃、
「low level hackathon」とかいう Microsoft 社内イベントをやっていたみたいです。

今、[C# 7.2](../../../../study/csharp/cheatsheet/ap_ver7_2.md)とかの頃に [`Span<T>` 構造体](../../../../study/csharp/resource/span.md)が追加されて以来の4年ぶりくらいの動きになりますが、
.NET ランタイムの低層に手を入れてパフォーマンス改善を図りたい流れになっているみたいです。

その後の様子を見るに、これは昨年10月の hackathon 時だけの短期的なブームというわけでもなくて、割かし .NET 7 目標にちゃんと動き出している雰囲気です。

ということで、今日の分のブログから数回はこの手の low level なパフォーマンス改善系の話をしていこうかと思います。

## 定数配列

今日は、以下のような、
全要素が定数の配列を書いたときの最適化の話になります。

```csharp {title="全要素が定数の配列"}
ReadOnlySpan<int> data = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
```

例えば以下のような2つのメソッドを比べてみましょう。

```csharp {title="ReadOnlySpan int と sbyte"}
static int M1(int i)
{
    ReadOnlySpan<int> table = new[] { 1, 0, -1, 0 }; // 差はこの行だけ
    return table[i % 4];
}

static int M2(int i)
{
    ReadOnlySpan<sbyte> table = new sbyte[] { 1, 0, -1, 0 }; // 差はこの行だけ
    return table[i % 4];
}
```

4要素の定数テーブルを引いているだけのシンプルなコードです。
`M1` と `M2` の差はテーブルが `int` か `sbyte` かという点だけになりますが、
(少なくとも .NET 6 / C# 10.0 では) この差だけでパフォーマンスが数倍違います。

[ベンチマーク用コード](https://gist.github.com/ufcpp/6326920e1fefe48a91c6f11a05ae9b6e)

一番大きな差は、

* `int` の方は普通に配列が `new` されている (newarr 命令が発行されてる)
* `sbyte` の方は 生データが直接参照されて、`new ReadOnlySpan(void*, int)` が呼ばれている

という点になります。
その結果、配列のヒープ アロケーションが発生するかどうかでパフォーマンスに大きな差が出ます。
(`int` の方がだいぶ遅い。)

### エンディアン

C# 10.0 な現状、この手の最適化は `byte` と `sbyte` に対してしか掛からないという制限があります。

理由は主にエンディアンで、
一応、.NET ランタイムはビッグエンディアンにも対応しているので、
`new[] { 1, 2, ... }` と書いてバイト列としてデータを記録するとき、
`0, 0, 0, 1, 0, 0, 0, 2, ...` と並べるか、
`1, 0, 0, 0, 2, 0, 0, 0, ...` と並べるかという問題があります。

とはいえ、これは別に今までの「要素が全部定数の配列」でも同じ問題はあって、

* DLL 中にデータが埋め込まれる場合、.NET はリトルエンディアン
* 埋め込みデータから配列を作るときに `RuntimeHelpers.InitializeArray` メソッドを呼ぶ
* `InitializeArray` の中で、ビッグエンディアン環境だったらエンディアンをひっくり返す処理が入っている

みたいな動作をしているみたいです。

## CreateSpan

ならまあ、やるべきことは割かしわかりやすいわけでして。
埋め込みデータから直接 `ReadOnlySpan` を作る部分を `InitializeArray` と同様のヘルパー メソッドにして、
ビッグエンディアン環境だったらひっくり返す処理を挟めばいいということになります。

それがこちら:

[Add non-intrinsic implementation for `CreateSpan<T>`.`](https://github.com/dotnet/runtime/pull/60451)


Roslyn (C# コンパイラー)側の対応:

[RuntimeHelpers.CreateSpan optimization for stackalloc](https://github.com/dotnet/roslyn/pull/57123)

dotnet/runtime 内で既存コードに対してこれを前提にした最適化を掛けたもの:

[Experiment with Roslyn optimization for `ROS<T>` in assembly data section](https://github.com/dotnet/runtime/pull/60327)

これが正式に採用されれば、
最初に例に挙げた `M1` メソッドと `M2` メソッドのパフォーマンス差はもう少し縮まるはずです。
