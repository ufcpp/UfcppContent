---
title: "ピックアップRoslyn: C# 9.0での、パターン、records、switch、null チェックの改善"
source_url: "https://ufcpp.net/blog/2019/12/pickuproslyn1221/"
content_type: "BlogEntry"
published_at: "2019-12-21T17:21:07"
updated_at: "2019-12-21T17:21:07"
tags: []
umbraco_id: 2277
parent_id: 2276
sort_order: 0
aliases: []
---

# ピックアップRoslyn: C# 9.0での、パターン、records、switch、null チェックの改善

この1週間で C# Design Notes が4件立て続けにアップロードされました。

- [Added: LDM Notes for Nov. 18, 2019 #3032](https://github.com/dotnet/csharplang/issues/3032)
- [Added: LDM notes for Dec. 11, 2019 #3027](https://github.com/dotnet/csharplang/issues/3027)
- [Added: LDM Notes for Dec. 16, 2019 #3036](https://github.com/dotnet/csharplang/issues/3036)
- [Added: LDM notes for Dec. 18, 2019 #3039](https://github.com/dotnet/csharplang/issues/3039)

あと、1件、提案ドキュメント追加:

- [Unconstrained type parameter annotation proposal #3035](https://github.com/dotnet/csharplang/pull/3035)

## Nov. 18: パターン マッチ

C# 9.0 でいくつか「[パターン](../../../../study/csharp/datatype/patterns.md)」の追加が検討されています。

- パターンの組み合わせ: `not`, `and`, `or` とかで、パターンの組み合わせたり条件を反転させたり
- 括弧つきパターン: `x is (pattern)` みたいなやつ。`and` とかの優先順位の明確化用(`x is (pattern1) and (pattern2)` とか)
- 関係パターン: `x is < 3` みたいな感じで大小比較

Nov. 18 の Design Meeting ではこの辺りに関する検討があったみたいです。

- 括弧つきパターンは 1-tuple (1要素のタプル パターン)と区別がつくか
  - 1-tuple の方を `x is (Item1: var tupleVal)` みたいに名前(この例でいうと `Item1`)必須にすれば区別はつく
- 関係パターン
  - 正直、構文として `x is >= 3 and <= 5' みたいなのはきもい
      - 2項演算子を単項的に使う構文になれてなさすぎる
  - でも、`x is int 3..5` みたいな「range パターン」と違って「両端を含むかどうか」で悩む必要がない明瞭さがある
  - ユーザー定義演算子をどうするかという問題もあり
      - `x is null` みたいな「定数パターン」ではユーザー定義演算子は呼ばれない
      - でも、`x is < 3` と `x < 3` で挙動が違うのはいいのか
  - `x is < 3` とかは暗黙的に `x is int i && i < 3` みたいな型チェックが含まれる
      - `x is not <3` と `x is >= 3` が同じ意味にならない
- パターンの組み合わせ
  - 今の提案では、今後、パターン中での `not`, `and, `or` がキーワードになる
  - これは既存のコードを壊す可能性がある
  - どこまでこの breaking change を許容できるものか要検討

## Dec. 11: Records

### key 修飾(records の等価比較)

Dec. 11 は Records がらみの検討があったみたいです。
[以前も書いた通り](../../10/pickuproslyn1025/index.md)、「Records」と呼ばれる機能は独立した小さな機能に分解して実装が検討されています。
この日は、等価比較 (`Equals` メソッドや `==` 演算子)の生成に関して、`key` 修飾子を追加しようかという話です。

以下のように、`key` 修飾子を付けることで、そのメンバーの比較によってクラス自体の等価性を判定(する `Equals` メソッドなどを生成)しようという話。

```csharp {title="key 修飾子" highlight-text="key"}
class C
{
    public key string Item1 { get; }
    public string Item2 { get; }
}
```

検討事項としては、これ単体では Records 的な機能としては足りていない(コンストラクターを書かなきゃいけないようでは「何度も同じ名前を書かないといけなくてつらい」問題の対処にならない)とか、
クラスの場合に mutable なプロパティで `Equals` を実装してしまうと `Dictionary`/`HashSet` の動作を狂わせてしまうので避けたいとか、
そういう問題にどう取り組もうかという感じ。
構文がこれでいいか(`key' を使うかとか、どうつけるかとか)は置いておいて、議題としては前向きに取り組みたいという様子。 

### nominal vs. positional

nominal っていうのは `new C { Item1 = 1 }` みたいなやつで、positional っていうのは `new C(1)` みたいなやつのこと。
[8月頃に原案が出ていたので紹介していますが](../../8/pickuproslyn0803/index.md)、
それぞれ init-only、primary constructor という提案が出ています。

今回はこれら再検討というか、それぞれバラバラに提供できるか、混ぜて使って使用感はどうか、
「primary constructor は既存の戦略(普通にコンストラクター呼び出し)の延長線上なのでいいが、init-only は大丈夫か」とかそういう話でした。

## Dec. 16: switch

Dec. 16 は `switch` の改善が議題に。

旧来の C# の [`switch` ステートメント](../../../../study/csharp/structured/st_branch.md#switch)は、C++ の影響を色濃く受けていて、使い勝手的にいまいちだとよく言われています。
それに対して、C# 8.0 では [`switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)を導入したわけですが、
C# 8.0 時点で未解決の問題として、戻り値がない場合をどうするかというものがありました。

単に、`switch` 式の各枝に void なものを認めようという提案も出ていましたが、「末尾に `;` が必須になるのがちょっと嫌」とのこと。

代わりに、2つ提案が出ています。

- [Proposal: Enhanced switch statements #3038](https://github.com/dotnet/csharplang/issues/3038)
  - ステートメントの方の `switch` でも、`=>` を使った「枝」の簡素化(`case` ラベル撤廃＆`break` なしで OK にする)をしたい
- [Proposal: Block-bodied switch expression arms #3037](https://github.com/dotnet/csharplang/issues/3037)
  - 式の方の `switch` で、各枝に `{ statement1; statement2; ... }` みたいな複文を認めたい

### トリアージ

Dec. 16 ではその他ちょこっとトリアージがあったみたいです。

- private なフィールドに対する[確実な初期化](../../../../study/csharp/resource/rm_struct.md#definite-assignment)判定
    - 先に warning waves (警告出すかどうかの選択権)が必要。それ以降ならいつでも
- `catch` 付きの `try` ブロック内に `yield` を書けない問題
    - 今もうこの制限がある意味はあまりない(`await` 導入時に解決済み)ので緩和したい
    - いつでも
    - やるなら合わせて[非同期イテレーター](../../../../study/csharp/async/asyncstream.md#async-iterator)でもやる
- [generic operators](https://github.com/dotnet/csharplang/issues/813)
  - [ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)がらみは「型推論に失敗するときでも、型の明示で解決できる」状態にあってほしい
       - `M(x)` で型解決できない書けないときに、`M<T>(x)` と書けばいいみたいな
  - 演算子の場合、これに対するいい解決案が思い浮かばないのでリジェクト
- 引数に対する `nameof`
  - 引数のスコープは「メソッド内」なので、メソッドに付けてる属性中で参照できない問題がある
      - `[return: NotNullIfNotNull(nameof(x)] T M(T x)` みたいなのが書けない
  - 認めたい。いつでも

## Dec.18: nullable issue

Dec. 18 は [null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)がらみ。

### pure null チェック

パターン マッチで、以下のような書き方は暗黙的に null チェックを含みます。

- `x is { }`
- `x is object`

`x is null` や `x == null` と書くとそれ以降は「`x` は null ではない」判定が働くのに、これら `x is { }` や `x is object` などでは現状は判定が通っていません。
これらも null 判定に含めたいとのこと。

### var?

型推論はしたいけど、null 許容性だけを変えたいということがあります。

```csharp
var current = myLinkedList.Head; // 連結リストの先頭とかで、not-null 指定あり
while (current is object)
{
    ...
    current = current.Next; // not-null と推論された current に null が渡る可能性あり。警告
}
```

こういう時のために、`var?` という書き方を導入したらどうかという案が以前に検討されています。
それを再検討。

## Unconstrained type parameter annotation

[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)で一番困るのが制約なしの[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)です。
LINQ の `FirstOrDefault` みたいなやつの戻り値をどう扱うかで、そのために `T??` という記法を導入したいとのこと。

- ジェネリック型引数にだけ付けれる
- 意味としては `default` を返す/受け付ける
  - 結果、参照型に対しては null があり得て、非 null 値型には null があり得ない
