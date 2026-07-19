---
title: "ピックアップRoslyn 5/20: 5月の Design Notes"
source_url: "https://ufcpp.net/blog/2018/5/pickuproslyn0520/"
content_type: "BlogEntry"
published_at: "2018-05-20T16:11:02"
updated_at: "2018-05-20T16:11:02"
tags: []
umbraco_id: 2154
parent_id: 2150
sort_order: 3
aliases: []
---

# ピックアップRoslyn 5/20: 5月の Design Notes

5月の Language Design Notes が2件ほど追加されました。

- [C# Language Design Notes for May 2, 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-05-02.md)
- [C# Language Design Notes for May 14, 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-05-14.md)

さらっと抜粋。

## switch 式

[先週書いた](../cs80preview/index.md)通り、現状のプレビュー版では、以下のような文法で「式」としてswitchを書けます。

```csharp
var s = x switch
{
    1 => "one",
    2 => "two",
    3 => "three",
    _ => throw new IndexOutOfRangeException()
};
```

今のところ、`=>` で実装されているんですが、これに関して:

- `:`、`->`、`~>` なんかも考えはした
- まだ決めかねてる。今の実装はとりあえず `=>` になってるけども
- `=>` はこれはこれで、ラムダ式と混ざったりデメリットもありそう

など。

## ranges

これも[先週書いた](../cs80preview/index.md)通り。`1..^1`みたいな書き方で「`1` ～ `Length - 1` の直前まで」(= 最初と最後、1要素ずつ削ったもの)を表す。

アプローチとしては大筋はよさそう。完全に認められたわけでもないけども、「害悪」とまでは思われてない。`^`って文字を使うのはC#にとってはちょっと馴れないけども、しょせん「馴れてない」程度の話。

## nullable reference types

参照型に対して `T` なら null が来ない、`T?` なら null があり得る、だけだと不十分で、
いくつか、属性を使ったアノテーションを実装し始めたみたい。

`string.IsNullOrEmpty`みたいなメソッドでは、「戻り値が false だったらそれ以降、引数は null ではない」とかいう挙動なわけですが、それ用に`NotNullWhenFalse`属性を導入。




```csharp
static bool IsNullOrEmpty([NotNullWhenFalse] string? s) { }
```

また、以下のように `EnsuresNotNull` 属性で、「このメソッドを呼んだら、引数は null ではないことを確認済み」

```csharp
static void AssertNotNull<T>([EnsuresNotNull] T? t) where T : class { }
```

`AssertNotNull` だと「null だったらそこで例外」みたいな挙動だけども、別に例外でなくても、
「メソッド内部で null でない値に上書き」とかもあり得る。

```csharp
static void EnsureNotNull([EnsuresNotNull] ref string? s) { if (s is null) s = ""; }
```

`==` 以外での null チェックもできるように、「`Equals` の類のメソッドです。null 解析に使ってください」を表す `NullableEquals` 属性も。

```csharp
class Object
{
    [NullableEquals] public static bool ReferenceEquals(object? x, object? y) { }
    [NullableEquals] public static bool Equals(object? x, object? y) { }
    [NullableEquals] public virtual bool Equals(object? other) { }
    [NullableEquals] public static bool operator==(object? x, object? y) { }
}
```

既存のコードでこの属性が付いてない場合に備えて、
「外からアノテーションを足す」みたいな機能も欲しい。
(属性だと、そのメソッドを書いた人にしか付けれない。)

## インターフェイスのデフォルト メソッド

インターフェイスの中に実装を置くことに対して、やっぱりいくらかの人が反対してる。
多くの人は、インターフェイスを自分で書いて自分で使ってる。
この状況だと、「インターフェイスにメンバーを追加したら破壊的変更」と言うのが問題になりにくい。

でも、public な API を作っている人にとってはデフォルト メソッド(= インターフェイスに後からメソッドを追加しても破壊的変更にならなくできる)は非常に重要。
また、Swift や Java との相互運用(主に Xamarin 用)には必要。

なので重要な機能だと考える。

## discriminated unions

### switch の網羅性

以下のような感じで、「Animal の派生クラスは Dog と Cat しか認めない」みたいな状態を作ったとして

```csharp
abstract class Animal
{
    private Animal() { }
    sealed class Dog : Animal { }
    sealed class Cat : Animal { }
}
```

switch の網羅性(考えうるケースを網羅してたら `_` や `default` を警告なしで省略できるようにしたい)はどう考えるべきか。
以下のコードだとダメ。

```csharp
int M(Animal a)
{
    return a switch
    {
        Cat c => 1,
        Dog d => 2
    }
}
int M(Box<Animal> b)
{
    return b switch
    {
        Box (Cat c) => 1,
        Box (Dog d) => 2
    }
}
```

実際には以下のように書かないと網羅的じゃない。

```csharp
int M(Animal a)
{
    return a switch
    {
        Cat c => 1,
        Dog d => 2,
        null => 3
    }
}
int M(Box<Animal> b)
{
    return b switch
    {
        Box (Cat c) => 1,
        Box (Dog d) => 2,
        Box (null) => 3
        null => 3
    }
}
```

### struct unions

上記のようなクラスを使った discriminated unions の実装(F# の discriminated unions はこんな感じのクラスに展開されてる)の他に、構造体を使った実装もありえる。
`int` と `(short, short)` みたいな、小さい型のどちらかだけを使いたいみたいな場合はある(特にパフォーマンスを求める場面で)。
ただ、これを実現するには .NET ランタイムのレベルでの対応が必要。
