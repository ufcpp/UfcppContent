---
title: "ピックアップRoslyn 9/12: nullable参照型、target-typed new、Index 演算子など"
source_url: "https://ufcpp.net/blog/2018/9/pickuproslyn0912/"
content_type: "BlogEntry"
published_at: "2018-09-12T18:30:11"
updated_at: "2018-09-12T18:30:11"
tags: []
umbraco_id: 2169
parent_id: 2168
sort_order: 0
aliases: []
---

# ピックアップRoslyn 9/12: nullable参照型、target-typed new、Index 演算子など

夏休みな時期を抜けたからか、7月末辺りからの Design Notes が何件かアップロードされました。

- [Added: LDM notes for August 20, 2018 #1836](https://github.com/dotnet/csharplang/issues/1836)
- [Added: LDM notes for August 22, 2018 #1848](https://github.com/dotnet/csharplang/issues/1848)
- [Added: LDM notes for September 5, 2018 #1849](https://github.com/dotnet/csharplang/issues/1849)

どれも、C# 8.0に向けて実装している機能の、割と細かい課題とそれに対してどうするかの決定についての話。

## ! 演算子(nullチェックの抑止演算子)

nullable(もしくは C# 7.X 以前のコード由来)な変数だけど、実際には null が来ないはずという時に使うのが`!`演算子。単に null チェック警告を出さないようにするもの。

### 入れ子の場合

[入れ子の場合](https://github.com/dotnet/roslyn/issues/29642) (`List<string>`から`List<string?>`に代入するときとか)のチェックをどうするかが議題に。

キャストと `!` 演算子は似た性質のものだけども、こういう場合、キャストなら警告を出して、`!`なら出さないべき。

### 意味のない !

元々非nullなものに `!` を付けたり、2重に`!!`とか付けた場合をどうするか。

コンパイラーとしては何もしない(警告を出さない)。チェックするとしたらIDEのリファクタリング機能として提供。

## nullable 型の dereference

ここでいう dereference (脱参照。参照先の値を得る操作)は、`x` に対して `x.Substring(1)` とか、メンバー呼び出しすることを指してる。

以下のように、nullable なものに対して何らかのメソッドを呼んだ場合、
`x` が null だったら例外が出るはずで、だったら、呼び出して以降は null チェック済みと考えていいはず。

```csharp
string? x = y;
var z = x.Substring(1);
...
```

実際そう扱うみたい。
正確には、`try`-`catch` があった場合、`try`句内のdereference以降は非 null として、
`catch` 句内では nullable として扱う。

## null コントラクト属性

[5/22 に書いた、属性でnullチェックの有無を指示する方式](../../5/pickuproslyn0520/index.md)のこと。

現在の候補一覧: [Recognize annotations on methods that affect nullability analysis #26761](https://github.com/dotnet/roslyn/issues/26761)

とりあえず今上がってる候補でプロトタイプ実装を初めて見ようみたいな空気。

## 実行的チェック コードの挿入

nullable 参照型は、現状、コンパイル時にだけ使うアノテーションになってる。
古いコンパイラーを使ったりでチェックをすり抜けたとしても、特に実行時チェックは働いてない。

それに対して、実行時チェックするコードを挿入するような機能もあってもいい？
例えば、`void M(string s!) { }` みたいに、引数の後ろに `!` を付けたら、引数の null チェック コードを挿入するとか。

とりあえず正式な提案ドキュメントを書いてみる。

## `Nullable<T>`

nullable 参照型に対するフロー解析、一部は `Nullable<T>` 構造体の `.Value` にも使えるはず。

## `class?` 制約

`void M<T>(T? t) where T : class?` は認める？

`T?` は、`T` が非 null の時にだけ付けれるようにしたい。`T : class?` の時点で`T` が nullable なので、`T?` は認めない。

## `List<T>.FirstOrDefault()`

`List<T>` の `T` が制約なしの場合、`FirstOrDefault()` の戻り値はどうなるべき？

`MaybeNull`扱いに。

## nullable参照型のgeneric制約

```csharp
void M<T, U>()
    where T : class
    where U : T
{ }

interface I { }
void M<T>() where T : I { }
```

みたいなのの、`T` は非 null？

非 null 扱いにする予定。

## target-typed new

左辺からの型推論で、[`new ()` だけで`new`できる](https://github.com/dotnet/csharplang/blob/master/proposals/target-typed-new.md)やつに関して。

### タプルに関して

今、タプルに関しては `new (int, int)(0, 0)` みたいな書き方を認めてないけども。
`new (0, 0)` の場合は認めるべき？
7要素以上のタプルの場合、実際に作られるのは`new ValueTuple<..., ValueTuple<...>>` みたいな入れ子の`ValueTuple`だけども。

`new (0, 0)` は認める予定。この場合はタプル構文(8要素以上も大丈夫。上記の入れ子の`ValueTuple`を構築)ではなく、単なる`ValueTuple`構造体のコンストラクター呼び出しという扱い(なので8要素以上の時には使えない/入れ子が必要)になる。

### throw new()

`throw new()` って書いた時、暗黙的に `new Exception()` 扱いすべき？

しないことに。

### ユーザー定義の比較、算術演算で new()

`x == new ()` みたいなの、認める。

## Index 演算子

`^1` って書き方で、「配列の末尾から1要素目」みたいな扱いにするための `^` 演算子のこと。

- 構文的に、単項演算子扱い？ → そう
- ユーザー定義演算子？ → なしで。今のところ int 以外のオーバーロードの需要をあんまり感じない
- 優先順位は？ → 単項演算子扱いになったので、他の単項演算子と同列
- `^^1` とかは認める？ → 単なる単項演算子扱い + int 以外認めない + `^` の結果は `Index` 型なので、`^^1` は必然的にエラーに
- `..` についても同様 → `..` の方は2項演算子ではなく、専用の構文扱いにする

## Compiler intrinsics

[8/22に書いた、ldftn + calli のやつ](../../8/pickuproslyn0822/index.md)。

この ldftn + calli を出力できる仕組み自体は取り組みたい。ただ、8/22 時点の提案からはちょっと修正が必要そう。例えば:

- インスタンスに対して `&instance.M` みたいな書き方は認めない
- `calli` は、`_calli_stdcall(args, f)`みたいな特殊キーワードでやる方がいいかも
- 呼び出し規約をどうするかは呼び出し側に書きたい
