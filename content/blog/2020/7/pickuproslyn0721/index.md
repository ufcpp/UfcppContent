---
title: "ピックアップRoslyn 7/21: 引き続き C# 10.0 向けトリアージ"
source_url: "https://ufcpp.net/blog/2020/7/pickuproslyn0721/"
content_type: "BlogEntry"
published_at: "2020-07-21T23:33:07"
updated_at: "2020-07-21T23:33:07"
tags: []
umbraco_id: 2306
parent_id: 2302
sort_order: 1
aliases: []
---

# ピックアップRoslyn 7/21: 引き続き C# 10.0 向けトリアージ

[先週に続き](../pickuproslyn0719/index.md)トリアージ。

- [C# Language Design Meeting for July 20th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-07-20.md)

あと、records がらみの残作業整理。

- [Remaining design work in and around records #3707](https://github.com/dotnet/csharplang/issues/3707)

こちらも 9.0 (今年11月リリース)よりも先の話になります。
records の「残作業」というのも「9.0 からは削ったけども」という話です。

## トリアージ

- [C# Language Design Meeting for July 20th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-07-20.md)

### Extend with expression to anonymous type

- 匿名型は record 的な性質がある(immutable で、[オブジェクト初期化子](../../../../study/csharp/oop/oo_construct.md#member_initializer)を使ってインスタンス生成)型なので、record と同じく `with` 式を使いたい
- そもそも「`with` 式の汎用化」自体「records の残作業」なので、それと合わせて C# 10.0 に向けて考えたい

### Required properties

- init プロパティ(`var a = new A { X = 1 }` みたいに初期化子での書き換えはできるけど、その後 `a.X = 2;` みたいな書き換えは認めないプロパティ)に対して、「初期化子での初期化を義務付け」みたいな制約を足したい
- C# 10.0 で考える

### Shebang support

- Shebang = スクリプトでよく見る `#!/bin/sh` みたいなやつのこと(sharp + bang を縮めた造語)
  ‐ シェルに解釈してもらって、「このスクリプトファイルをどのインタープリターに掛けるか」みたいなのの指定に使う
  ‐ インタープリター側からすると単に無視してる
- これまで C# は、ランタイムのバージョンとか、依存するパッケージ、ツールとかの指定は C# ソースコード内には書かず、外部(csproj とか)に置いてた
  - C# のスクリプト用途を次のステップに進めるために、C# でも「単一 cs ファイルでこの手の情報を持てるようにしたい」という話に
- C# 10.0 のタイミングでディスカッションしたい
  ‐ 10.0 に入れれないかもしれないけど、「ディスカッション開始自体に何年もかかる」みたいにはしたくなくて、「何か月かかかる」程度に収めたい

### Private fields in structs with SkipLocalsInit

- [前回](../pickuproslyn0719/index.md)も書いた「private フィールド持ちの構造体の[確実な初期化](../../../../study/csharp/resource/rm_struct.md#definite-assignment)が実は漏れてる」問題、やっぱりまずいという話に
- [SkipLocalsInit](../../4/begginingcs9/index.md)と組み合わさると、セキュリティ的にもガベコレ的にも危ないコードになる
- native compiler (C# 5.0 以前の、C++ 実装の C# コンパイラー)時代からの負の遺産が残っているだけで、ちゃんと未初期化を検知して警告にできる実装にはなってる
  - 警告の追加も破壊的変更になるので、警告ウェーブ(言語バージョンとは別に警告のバージョニングを追加する)とともに有効にする予定だった
-でも、SkipLocalsInit 指定があるときには有無を言わさず警告を出すように修正することにした

## records がらみの残作業

- [Remaining design work in and around records #3707](https://github.com/dotnet/csharplang/issues/3707)

### More initialization functionality

reocrds は、これまでの C# の「immutable なオブジェクトの初期化がとにかくめんどくさい」という問題に対する解決策だったりするので、初期化回りの機能が多いです。
9.0 で入れれなくて引き続き課題になっているものも結構残っていたり。

#### Init fields

- init-only プロパティ(`T Property { get; init; }` みたいなやつ)だけじゃなく、フィールドにも `init` 修飾をつけて、オブジェクト初期化子で書き換え可能にしたい

#### Init members

- init-only プロパティはコンストラクターか他の `init`アクセサーからも書き換えできるけど、9.0 時点ではその他のメソッドからは書き換えできない
- メソッドに `init` 修飾を付けることで、コンストラクターか `init` アクセサー内からだけ呼び出せて、init-only プロパティの書き換えができるメソッドを定義したい

#### Final initializers

- オブジェクト初期化子での初期化が一通り終わったあとに呼ばれるメソッドが欲しい
  - コンストラクターはオブジェクト初期化子よりも前になっちゃう
- 今のところ `init { ... }` みたいな構文を考えてる
- この機能自体は欲しい。初期化が一通り終わったあとに、オブジェクトの状態が不正じゃないかの確認をしたいことは多々ある
- ただ、「オブジェクト初期化子の後に呼ぶ」っていうタイミングは古いコンパイラーには強制できない

#### Required members

トリアージでも出てきてるので省略。

#### Factories

- ファクトリーメソッドも records の一部として考慮
  ‐ 「必ず新しいインスタンスを作って返す」みたいなのを強制する仕組みが要る

#### What about collection initializers?

- init-only なコレクション初期化子は可能？
  - `new T { a, b }` みたいなのは、`var x = new T(); x.Add(a); x.Add(b);` に展開しちゃうし、通常、この `Add` は mutation (状態の書き換え)を起こしちゃう
  ‐ 前述の init members を使って、init-only (初期化子のタイミングまでは呼び出し可能)な `Add` メソッドを作れればいける？

### Generalizing away record magic

今、records (`record` キーワードを使って型を宣言)専用になってしまっている機能が結構あるものの、普通のクラスや構造体に対しても適用できそうな文法も結構あります。
records と他の複合型の差は小さい方が、互いに乗り換えがしやすくて好ましいので、
できる限り「records 専用な構文」は作りたくありません。

#### Allowing users to define cloning

- `with` 式は「クローン → 部分書き換え」を行う構文
- 現状、このクローンは records から生成される通常定義・通常呼び出し不可の専用メソッドでやってる
- 前述の Factories も入れた上で、ユーザー定義の `Clone` メソッドを受け付けるようにしたい

#### Cross-inheritance between records and non-records

- 現状、records は records から、クラスはクラスからしか派生できない
- 「records の基底クラスとして使える条件」みたいなのをしっかりと定義することで、「異種派生」を認められるようにしたい

#### Primary constructors

- `record T(int X, int Y);` みたいな書き方、クラスと構造体でも使えるようにしたい
  ‐ records の場合はこのコンストラクター引数からプロパティ `X`、`Y` の生成までやっちゃってるけど
  ‐ クラス、構造体ではコンストラクターの簡易記法としてだけ使って、プロパティの生成まではやらない

#### Bodies and attributes for primary constructors

- 現状、プライマリ コンストラクターには属性を付けれないし、本体を持てない
- `record T(int X) { T { ... } }` みたいな記法(引数なしコンストラクター)で、プライマリ コンストラクターの本体を与えたい
- Final initializers (`init { ... }`) と用途がちょっと被り気味だけど、完全に一本化できなくて、どっちも必要

#### Automatic with-ing on all structs?

- 構造体は常に `with` 式で使える要件を満たせてはいる
  ‐ 暗黙的にメンバーごとのコピー機能を持ってる
  ‐ これをそのまま使って `with` 式を認めるべき？

### More record functionality

いくつか、records として予定されていた機能は C# 9.0 に間に合ってなくて未実装。

#### Struct records

- 現状、records は参照型
- records のセマンティクスは構造体にインスパイアされてるものなのに、records の機能のいくつかは当の構造体では使えない
  ‐ positional members (プライマリ コンストラクターからのプロパティ生成)とか、strongly-typed な `Equals`/`IEquatable<T>`  生成したりとか
- 「値型の records」みたいなのを定義する構文が別途必要

#### Data properties

- `data string Name` みたいな書き方で `public string Name { get; init; }` を生成したい

### Discriminated unions

[#113](https://github.com/dotnet/csharplang/issues/113) とか [#2962](https://github.com/dotnet/csharplang/issues/2962) とか参照。

- records が落ち着いてからその先の機能として検討するつもりでいた
- その時が来た
