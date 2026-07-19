---
title: "ピックアップRoslyn 10/25: Records、Static lambdas"
source_url: "https://ufcpp.net/blog/2019/10/pickuproslyn1025/"
content_type: "BlogEntry"
published_at: "2019-10-25T21:09:17"
updated_at: "2019-10-25T21:09:17"
tags: []
umbraco_id: 2271
parent_id: 2268
sort_order: 2
aliases: []
---

# ピックアップRoslyn 10/25: Records、Static lambdas

Design Meeting でちょっと Records がらみの話があったみたいです。
あと、ついでのように「static ラムダ式」の話。

- [C# Language Design Meeting for Oct. 21](https://github.com/dotnet/csharplang/issues/2903)

## Records

Records がらみは何か新アイディアが出たわけではなくて、直近、何から手を付けるか的な意思決定っぽいです。
[8月にブログに書いた](../../8/pickuproslyn0803/index.md)通り、元々 Records と呼ばれていた機能は今はいくつかの小さな機能に分割する流れになっています。

- Automatic structural equality
  - 「メンバーごとの等値判定」で `Equals` メソッドを自動生成
  - 今出てる案では `data class DataType { ... }` みたいに `data` 修飾子を付けると `Equals` の自動生成をするようになる
  - キーワードは変更の可能性あり
- With-er
  - `obj with { X = newValue }` みたいな書き方で一部のメンバーだけ値を差し替える構文
  - 「`with` で使う用の特別なコンストラクター」みたいなのを用意しておく想定
- Object initializers for readonly
  - get-only なプロパティ `X` を `new DataType { X = 1 }` みたいな初期化子で上書きできるようにする案
  - プロパティに `initonly` 修飾を付けることでこの初期化を有効化
  - (今はコンストラクター内での初期化が必須なので、コンストラクター引数にする必要あり)
  - 「初期化子を呼ぶタイミングまでは書き換え可能で、その後は読み取り専用」みたいな仕組みを追加予定
- Primary constructors + deconstruction
  - `class DataType(int X, int Y);` みたいな書き方からプロパティと、その初期化・分解(コンストラクター、`Deconstruct`メソッド)を自動生成

それぞれの機能が後々衝突しないように考えながら作らないといけないものの、
一応は個別に実装して行けるようには分割してあります。

いずれも、原則として、コンパイラーが自動生成しなくてもやろうと思えばユーザーが手書きできる範疇の機能にとどめたいそうです。

また、今回のミーティングでは、まず割かし仕様が固まってきてる primary constructors と structural equality の2つから実装していきたいという話をしています。

### init-only

init-only (前節の Object initializers for readonly のこと)の懸念点についても軽くディスカッションがあったそうです。

基本的には、`initonly T X { get; }` みたいに書いたとき、実際にはプロパティ `X` に `set` も生成して、それを呼べる場所をコンストラクター内と初期化子だけに制限するという仕様。

懸念は以下のようなもの。

- 基底クラスのコンストラクターでの初期化
  - 今のところ特にそれを禁止する理由は思い当たらない
- required (初期化が必須)のメンバー
  - null 許容参照型があったりするわけで、未初期化(自動的に null になる)のはできれば避けたい
  - ただ、「初期化子での初期化をしていないとエラー」ってやってしまうと、後からプロパティを追加したときに既存コードを壊す
  - 特に、`initonly` は C# ソースコードのレベルでの機能になる予定で、コンパイル済みのメタデータには反映されない予定
      - 後から required な `initonly` プロパティを足しても、以前からその型を使っているコードは未初期化のまま素通しになってしまう

## static ラムダ式

C# 8.0 で[静的ローカル関数](../../../../study/csharp/functional/fun_localfunctions.md#static-local-function)が入ったわけですが、ラムダ式にも同じ制限が考えられます。

`static` 修飾子を付けることで外の変数・引数をキャプチャできなくする機能で、
パフォーマンスへの配慮です。
キャプチャが発生してしまうと発生してないときよりもちょっとパフォーマンスが落ちるので、
意図せずキャプチャしてしまうことを避けるために、
キャプチャの必要がないならこの修飾子を付けておけという機能。

というか、C# 8.0 の時点でも「ラムダ式でも同様に」という話はあったんですが、
スケジュールの都合でお蔵入りしていました。
それが改めて検討に上がった状態。

検討に上がったというか、[最近、すでにプロトタイプ実装があったり](https://github.com/dotnet/roslyn/pull/39121)。
元々ラムダ式に `async` 修飾子を付けれるわけで、追加で `static` を認めるのはそんなに難しくない様子。

ミーティングで出た議題としては、
`static` を付けたラムダ式は、生成結果的に静的メソッドであるべきかどうかという点。
詳しくは「[デリゲートの内部](../../../../study/csharp/functional/miscdelegateinternal.md)」で書いたことがありますが、
実のところ、デリゲートとして使う場合、静的メソッドよりもインスタンス メソッドの方が高速だったりします。
なので、`static` を付けたラムダ式であっても、実体としてはインスタンス メソッドに「なってもよい」という余地を残す方が最適化が掛けやすいです。

ということで、仕様としては「`static` キーワードを付けても必ずしも静的メソッドが生成されるわけではない」ということにしておきたいそうです。
