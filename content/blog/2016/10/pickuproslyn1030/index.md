---
title: "ピックアップRoslyn 10/30: 分解、宣言式、ワイルドカードの内部仕様変更"
source_url: "https://ufcpp.net/blog/2016/10/pickuproslyn1030/"
content_type: "BlogEntry"
published_at: "2016-10-30T03:22:22"
updated_at: "2016-10-30T03:22:22"
tags: []
umbraco_id: 1965
parent_id: 1963
sort_order: 1
aliases: []
---

# ピックアップRoslyn 10/30: 分解、宣言式、ワイルドカードの内部仕様変更

[Roslynリポジトリ](https://github.com/dotnet/roslyn)のブランチやマイルストーンの動きを見るに、次のVisual Studio 15 PreviewはもうRCなようですが。
RC (リリース候補)の段階で大きな変更をするわけもなく、[最近のC#チームの動き](https://github.com/dotnet/roslyn/issues/14804)は大体「ただひたすらバグ修正」なわけです。

そんな中、仕様変更の話が。

- [Proposed changes for deconstruction, declaration expressions, and wildcards #14794](https://github.com/dotnet/roslyn/issues/14794)

まあ、内部挙動的な話なので、C# 7の見栄え上はほとんど変わらないはず。[分解](../../../../study/csharp/datatype/deconstruction.md)や[出力変数宣言](../../../../study/csharp/resource/sp_ref.md#out-var)で、`(A < B, C > D)`みたいな解釈の難しい書き方がどう解釈されるか、みたいな細かい挙動が変わるだけです。

ただ、プログラミング言語としてのC# 7的には大した変化じゃないですが、CodeAnalysis API(いわゆる、Compiler as a Service。C#コンパイラーの中身を誰でも見れるようにするって意味でのC# 7 API)的にはもちろん目立つ変更になります。要は、`SyntaxNode`の種類が変わります。

## ワイルドカード

ワイルドカード(wildcard)は、仮引数、分解、出力変数などの値を受け取る側で、別にその値を使わない場合に無視する記法がほしいというやつ。
以下の、`*`みたいなやつ。

```csharp {title="Wildcardの例"}
using System;
using System.Diagnostics;

class Base
{
    public event EventHandler<string> E;
    protected virtual void M(int x, int y) { }
    public void Deconstruct(out int x, out int y)
    {
        x = 1;
        y = 2;
    }
}

class X : Base
{
    public X()
    {
        E += (*, *) => Debug.WriteLine("log message");

        Deconstruct(out *, out var y);
    }

    protected override void M(int *, int *)
    {
    }
}
```

案としては `_`、`*`、`?` なんかが出ていました。これまで、`*` が最有力だったんですが、`_` の方にするかも、とのこと。

- `_`
  - 今現在、`_` 1文字はC#の識別子として有効なものなので、`_` をワイルドカードに使うのは破壊的変更になりかねない
  - 一応、`_`を使っている場所があれば識別子として、使ってなければワイルドカードとして扱うみたいな処理を入れれば破壊的変更にはならない
  - ただ、それやるとユーザー的には混乱しないかという話はある。まあ、現状すでに、多くのユーザーが`_`を「使わない引数」として使ってるので大丈夫ではないか
- `*`、`?`
  - 破壊的変更は起こしにくい
  - ただ、`int *`みたいな書き方が、「`int`型をワイルドカードで無視」なのか、「ポインター型`int*`」なのかという不明瞭さがある
  - `?`も、null許容型の`int?`との不明瞭さがある
  - この不明瞭さの解決の方が、`_`の方の問題よりも難しそうという意見に傾いてきた

## 宣言式

元々の計画としては、式の途中のどこにでも変数宣言を書ける「宣言式」(declaration expressions)という機能が検討されていました。
現在でも、将来的には宣言式を追加する見込みは非常に高いですが、C# 7では入りません。

一方、C# 7に入る[出力変数宣言](../../../../study/csharp/resource/sp_ref.md#out-var)は、この宣言式のサブセットみたいなものです。
なので、内部的には(CodeAnalysis API的には)、出力変数宣言は最初から「declaration expression」という名前になっています。

## 分解

一方、現状では、[分解](../../../../study/csharp/datatype/deconstruction.md)は、分解専用のステートメントとして実装されています。
これが、将来的に宣言式を入れたときに邪魔になりそうなわけです。
分解も、宣言式を使って表現した方がいいだろうということで、C# 8の際に破壊的変更にならないように、今から変えてしまいたいとのこと。

例えば、現状、C# 7では書けないものの、将来的には認められそうな書き方として、以下のようなものがあります。

```csharp {title="C# 7では書けないけども、将来的には認められそうな書き方"}
var tuple = (1, 2);

// タプル構築 + 分解(しかも、新しい変数 x, y を宣言しながら)
var t = (int x, int y) = tuple;
```

この、`int x`、`int y`の部分は、まさに、宣言式であるべきだろうという話です。

付随して、タプル、分解、出力変数宣言などでモデルをそろえておかないと、細かい挙動で差が出て気持ち悪いというのもあります。
例えば、以下のようなコードはどう評価されるべきか。

```csharp
(A < B, C > D) = F(A < B, C > D = value);
```

以下の2つの解釈ができます。

- `A < B`と`C > D`という2つの式を持つ
- `A<B, C>`というジェネリック型の変数`D`を宣言式で宣言している

これが、現状の、宣言式と分解で異なる実装をしてる状態だと、それぞれ逆の解釈になるそうで、まずい。
今のうちに統一挙動にしてしまいたいということに。

## foreach の変更

C# 7的に、分解が書けるのは以下の3カ所。

- 分解ステートメント: `var (x, y) = t;`
- `for`ステートメント: `for (var (x, y) = t; cond; iter)`
- `foreach`ステートメント: `foreach (var (x, y) in list)`

このうち、`for`は現在の文法構造で、この「分解も宣言式で定義する」という変更に耐えれるそうです。

一方、`foreach`には変更が必要そう。

## まとめ

宣言式とワイルドカードっていう、C# 7では入らない/入らなさそうな機能に関する話ではあるけども、
C# 7の時点からちゃんと考えて作っておかないと後から困りそう。
なので今から、宣言式とワイルドカードを前提とした作りにしておきたい、という話。
