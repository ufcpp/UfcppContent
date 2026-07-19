---
title: "ピックアップRoslyn 4/19: C# 9.0 機能の仕様ドキュメントいくつか"
source_url: "https://ufcpp.net/blog/2020/4/pickuproslyn0419/"
content_type: "BlogEntry"
published_at: "2020-04-19T11:43:34"
updated_at: "2020-04-19T19:57:00"
tags: []
umbraco_id: 2289
parent_id: 2286
sort_order: 2
aliases: []
---

# ピックアップRoslyn 4/19: C# 9.0 機能の仕様ドキュメントいくつか

今回は先にブログを書いてから、それを追う形でライブ配信(この部分は後からの追記):

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/STKZNnauJjo?start=304" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

ここ1週間くらいで、C# 9.0 で入るであろう機能がちゃんとした仕様ドキュメントに起こされ始めました。

- [Add draft spec for C# pattern-matching changes. #3361](https://github.com/dotnet/csharplang/pull/3361)
- [Add proposal for target-typed conditional expression. #3363](https://github.com/dotnet/csharplang/pull/3363)
- [C# Language Design for April 13, 2020 (Roadmap for records)](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-04-13.md)
  - [Init only proposal document #3367](https://github.com/dotnet/csharplang/pull/3367)

## パターン マッチング v3

パターン マッチングも3世代目になります。
最初 [C# 7.0](../../../../study/csharp/cheatsheet/ap_ver7.md) に入ったころは単に「`is` がちょっと便利になった」、
「`switch` で型分岐ができるようになった」程度の機能でしたが、
3世代目ともなるとずいぶんいろいろなものが増えています。

詳細は動くものが出てきてからちゃんとした記事化しようかと思いますが、以下のようなパターンが追加される予定です。

- 型パターン
  - 今、`int i` とか `int _` とか書かないといけないものを `int` だけで型パターン扱いする修正
- 括弧パターン
  - パターンを `()` でくくれるようにする
  - 下記 `and` や `or` の結合優先度がよくわからなくなるのを防ぐために追加
- conjunctive `and` (論理積) パターン
  - `and` で繋いだ2つのパターンのどちらにもマッチする
- disjunctive `or` (論理和) パターン
  - `or` で繋いだ2つのパターンの少なくともどちらか片方にマッチする
- negated `not` (論理否定) パターン
  - `not` の後ろのパターンを満たさない時にマッチする
- relational (比較) パターン
  - 数値(`int` 系、`float` 系、`decimal`)型に対して、`<`、`>`、`<=`、`>=` で大小比較する

パターン マッチングv3がらみは割とすでに実装が動いてるんですが、
いくつか最近検討されたばかり・まだ検討中の項目もあります。

- `x is byte and < 100` みたいに書くと、`and` の左側の型に合わせて右側の型が決まる ([roslyn #42207](https://github.com/dotnet/roslyn/issues/42207))
  - この場合、`< 100` 判定は `byte` 扱いで比較
- `or` の場合は左右の型の間の暗黙的な型変換(派生型 → 基底型みたいなやつ)だけ考慮 ([roslyn #43419](https://github.com/dotnet/roslyn/pull/43419))
- `x is not string ns` みたいに、`not` パターンを使った場合でもその後 `ns` 変数を使える ([csharplang #3369](https://github.com/dotnet/csharplang/issues/3369))

## target-typed 条件演算子

`Base x = b ? new A() : new B();` みたいな条件演算子を書いた時、
左辺の `Base` 型から型決定して、`A`, `B` 違う型でもこの式が有効になるようにしようという話です。

[`switch` 式](../../../../study/csharp/datatype/typeswitch.md#target-typed)の場合、導入時の C# 8.0 の時点からこの手の型決定機構が働いています。
`switch` 式でだけ有効なのも変な話なので条件演算子でも同様のことをしたいという案はずっとあったんですが、
問題は、以下のような場合に既存のコードを壊してしまうこと。

<pre class="source" title="target-typed 条件演算子の悩みどころ">
<code><span class="comment">// 既存のルールだと long の方が選ばれる。</span>
<span class="comment">// switch 式と同じルールの target-typed を導入すると short が優先されるようになる。</span>
M(b ? 1 : 2);

<span class="reserved">void</span> M(<span class="reserved">short</span> x) { }
<span class="reserved">void</span> M(<span class="reserved">long</span> x) { }
</code></pre>

ということで、

- `switch` 式の場合は target-typed による型決定 → 共通型の判定
- 条件演算子の場合は 共通型の判定 → target-typed による型決定

という不整合は起こすけどしょうがなく、このルールで実装するとのこと。

ちなみに、こういう特殊な実装をしてもなお、`M(b ? 1 : 2, 1);` みたいなメソッド呼び出しに対して破壊的変更になる可能性は残っているけども、これくらいなら許容範囲だろうということで破壊的変更を認める方向だそうです。

## Records のロードマップ

最近何度か書いてますが(例えば [2/3 のブログ](../../2/pickuproslyn0203/index.md))、
Records として検討されている機能は結構たくさんあります。

パターン マッチング同様、最終形を意識しつつも段階的に実装して行こうということで、
何を優先的に実装するかの検討に入ったみたいです。
現状、以下のような順序。

今取り組む(= C# 9.0 時点で入る):

- nominal だけ
  - 要するに、init-only プロパティだけまず実装して、プライマリ コンストラクターは入らない
- 派生は認めない(`object` からの直派生だけにする)
- `with` 式は `Clone` メソッドからの init-only プロパティの書き換えという決め打ち実装だけ認める
- value equality はちゃんと実装する

次の段階として以下のものを検討:

- 派生
- プライマリ コンストラクター
- `with` 式をカスタマイズできるようにする

同時に要検討:

- ファクトリ メソッドの生成
- validator
- 任意のプロパティを Records と同じ value quality/with 等のコード生成に含める
- Records 外でのプライマリ コンストラクターを認めるかどうか

ということで、直近(= C# 9.0)ではまず init-only プロパティってものが主役になりそうです。
(逆に、プライマリ コンストラクターはまた流れました。)
これ単体の提案ドキュメントも上がりました。

- [Init only proposal document #3367](https://github.com/dotnet/csharplang/pull/3367)

内部的には `set` アクセサーに対して `modreq` (ちょっと強制力の強い属性みたいなもの)を付ける方向で実装するみたいです。

(C# 7.2 移行、ちょくちょくこの `modreq` ってやつの話が出てくるので、
そろそろちゃんとこの話も記事化しようかと画策中: [tracking issue](https://github.com/ufcpp/UfcppSample/issues/295)。)
