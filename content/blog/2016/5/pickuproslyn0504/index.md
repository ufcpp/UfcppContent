---
title: "ピックアップRoslyn 5/4"
source_url: "https://ufcpp.net/blog/2016/5/pickuproslyn0504/"
content_type: "BlogEntry"
published_at: "2016-05-04T09:07:34"
updated_at: "2016-05-04T09:07:34"
tags: []
umbraco_id: 1891
parent_id: 1890
sort_order: 0
aliases: []
---

# ピックアップRoslyn 5/4

4月に紹介した[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)、だんだんはっきりとC# 7、VB 15に入りそうな範囲が結構絞られてきた感じ。

最近は、タプル型がらみに注力してる感じがします。

## パターン マッチングも部分的に実装

- [Split the features/patterns branch into two branches for subfeatures in/out C# 7 #10866](https://github.com/dotnet/roslyn/issues/10866)

これまで出てたアイディア全部を一気にC# 7に入れるんじゃなくて、将来的な拡張を阻害しないように気を付けつつ、部分的に実装しようという感じになってる様子。

上記リンクのうち、「Part 1 (targeting future)」になってる方がC# 7に入りそうなやつで、「Part 2 (to remain in features/patterns)」に入ってる方がそれより後のバージョンになりそうなもの。

要するに、C# 7としては、いったん、単純な型による分岐だけを実装するみたい。以下のものはその後の予定。

- 再帰的なオブジェクト分解の構文
- let ステートメント
- match 式
- throw 式

## C# Design Notes (タプル型関連)

- [C# Design Notes for Apr 12-22, 2016 #11031](https://github.com/dotnet/roslyn/issues/11031)

4月中にあったC# Designミーティングの議事録。タプル型関連の議題が多かったみたいです(あと、out varの話が少し)。以下のような内容。

- `ValueTuple`以外の型
  - C#のタプル型の実装に使われる`ValueTuple`構造体の他に、既存の`Tuple`クラスとか、あと、`KeyValuePair`なんかも性質としてはタプルっぽい。これらを統一的に扱いたい
- タプル型の分解(代入、宣言、パターン マッチング)用の構文案
  - `(int x, int y) = Get();`的なのか、`(int, int) (x, y) = Get();` 的なのか、いくつか候補あり。今のところ前者が有力
- `(byte, short)`から`(int, int)`みたいな、メンバー単位で暗黙の型変換がある場合の、タプル型間の変換
  - 認める方向で検討中。ただ、コンパイラーが頑張ることになるし、C# 6と7でオーバーロード解決ルールとかが変わっちゃう問題がある
- (C#の構文糖衣としての)タプル型と、構造体の`ValueType`
  - null許容型が`int?`でも`Nullable<int>`でも使えるように、タプル型も`(int, int)`でも`ValueTuple<int, int>`でも使えるようにしたい
- パターン マッチングとの兼ね合い

ちなみに、その`ValueTuple`構造体ですが、ついに[corefx](https://github.com/dotnet/corefx)の方に入ったみたい。

- [ValueTask.cs](https://github.com/dotnet/corefx/blob/master/src/System.Threading.Tasks.Extensions/src/System/Threading/Tasks/ValueTask.cs)

(ちょっと前までは、[roslyn](https://github.com/dotnet/roslyn)リポジトリ内でだけ実装してた)

## 非同期メソッドの戻り値を任意の型に

- [C# feature proposal: arbitrary async returns](https://github.com/ljw1004/roslyn/blob/features/async-return/docs/specs/feature%20-%20arbitrary%20async%20returns.md)
- [C# design rationale and alternatives: arbitrary async returns](https://github.com/ljw1004/roslyn/blob/features/async-return/docs/specs/feature%20-%20arbitrary%20async%20returns%20-%20discussion.md)
- [Discussion thread for arbitrary async returns #10902](https://github.com/dotnet/roslyn/issues/10902)

今、`Task`クラスしか返せない非同期メソッドに、任意の型を返せるように拡張したいという話。

あと、非同期ストリーム(`await`と`yield`を両方含めて、`IObservable`とか`IAsyncEnumerable`のような戻り値を返せるもの)の実装もまとめてディスカッション中。

これは、[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)の「(C# 7.0 and VB 15) + 1」にすら並んでないんで、さらにもうちょっと先になりそうなんですかね。
