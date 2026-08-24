---
title: "ピックアップRoslyn 2/3: Records総まとめ、トップ レベル ステートメント"
source_url: "https://ufcpp.net/blog/2020/2/pickuproslyn0203/"
content_type: "BlogEntry"
published_at: "2020-02-03T21:29:57"
updated_at: "2020-03-09T11:13:02"
tags: []
umbraco_id: 2282
parent_id: 2281
sort_order: 0
aliases: []
---

# ピックアップRoslyn 2/3: Records総まとめ、トップ レベル ステートメント

2件ほど。

- [Top-level statements and functions #3117](https://github.com/dotnet/csharplang/issues/3117)
- [Records as a collection of features #3137](https://github.com/dotnet/csharplang/issues/3137)

どちらも、散発的にアイディアが出てたもののまとめであるとか、現状報告的なものです。

## Top-level statements and functions

まず短い方から。

- [Top-level statements and functions #3117](https://github.com/dotnet/csharplang/issues/3117)

前々から、[普通の C# の文法と、スクリプト向けの文法を統合したい](https://github.com/dotnet/csharplang/issues/2765)みたいな話はって、それの再考というか、
シナリオの整理とどのシナリオを優先するかみたいな話。

ちなみに、まだマイルストーンも決まっていないので、おそらく C# 9.0 よりは先の話になると思います。

要は、以下のような「いつものおまじない」なしでいきなり(トップレベル、あるいは、名前空間直下のレベルに)ステートメントとかメソッドを書きたいという話になります。

```csharp {title="いつものおまじない"}
class Program
{
    static void Main()
    {
    }
}
```

主たる目的として3つのシナリオが上がっています。

1つ目は単にプログラムをシンプルに書きたいというもの。
普通に「おまじない」でボイラープレートなコードを減らしたい。
この意味では、トップレベルに書いたコードは `Program.Main` メソッドの中に自動的に組み込まれてほしい。

2つ目はグローバル関数的なものを定義したいという話。
`Math.Sin` みたいなものは元々「グローバルでも特に問題はないけど、名前の衝突を避けるために `Math` クラス配下にまとめらている」みたいなものです。
単に名前分けなら、名前空間直下に関数を書けても別にいいはず。
この意味では、何かラッパークラスを1個作って、その中の静的メソッドに変換して、自動的に  [`using`](../../../../study/csharp/structured/sp_namespace.md) される扱いすればいい。

3つ目はスクリプト用途。
今現在、[Microsoft.CodeAnalysis.CSharp.Scripting](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp.Scripting/)で提供されているやつで、微妙に[通常の C# 文法と違う文法](../../../../study/csharp/cheatsheet/apscripting.md)を受け付けます。
通常の文法と統合したいといいつつ、1つ目のシナリオとは競合します。
スクリプトの場合、実行するたびに別の状態を持たせたく、静的な `Main` メソッド内に展開されるような方式よりは、クラスを1個作ってそのメンバー扱い(ローカル変数のように書いたものが、実際にはフィールド扱い)する方が好ましかったりします。

今回の決定では、1つ目のシナリオ、要するに「トップレベルのステートメントを `Main` メソッドに自動的に組み込む」という方向で行きたいとのこと。
トップレベルのメソッドも、`Main` メソッド内のローカル関数扱いしようという感じみたいです。

結局、「通常モード」と「スクリプト モード」の統合はあきらめていて、
「2つのモードの差をあまり開かないようにしたい」くらいの方針。

## Records as a collection of features

- [Records as a collection of features #3137](https://github.com/dotnet/csharplang/issues/3137)

ここ数か月くらい散発的には話題に上がっていましたが([10/25](../../../2019/10/pickuproslyn1025/index.md)、[11/16](../../../2019/11/pickuprolsyn1116/index.md)、[12/21](../../../2019/12/pickuproslyn1221/index.md))、Records がらみの総まとめ。

こちらは C# 9.0 向け。なのでそろそろ具体性を帯びてきています。

まだ全部の提案に Strawman (藁人形。C# リポジトリ内では「いろいろ叩かれることを前提に、まずは C# チーム内で決めた案を公開」くらいの意味)という言葉が入っているので最終決定からはまだ遠い段階ですが、今までの中では一番まとまっていて、一番具体的な文法が出ています。

タイトルの通り、Records をいくつかの機能の組み合わせに分割したいという話なんですが、
いくつかは不可分みたいな話もしています。

### Value-based equality

値による比較(value-based equality)を楽に書きたいという要望が常々あります。

[12/21](../../../2019/12/pickuproslyn1221/index.md)のブログでは「`key` 修飾子」案が出ていましたが、
今回は `value` 修飾子になっています。
意味的には12/21の頃と同じ。この修飾子を付けたフィールドの値比較を持って、その型の `Equals` メソッドや `==` 演算子を生成したいというものです。

#### value members

差分としては、`EqualityContract` というプロパティも生成して、以下のような比較をした方がいいだろうという話が増えています。

例えば以下のような `Point` クラスがあったとして

```csharp {title="value 修飾が付いたプロパティを元に Equals を生成"}
public class Point
{
    public value int X { get; set; }
    public value int Y { get; set; }
}
```

以下のようなコード扱いしたいそうです。

```csharp {title="value 修飾からの生成結果"}
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    protected virtual Type EqualityContract => typeof(Point);
    public override bool Equals(object? other) =>
        other is Point that
        && this.EqualityContract == that.EqualityContract
        && this.X == that.X
        && this.Y == that.Y;
    public override int GetHashCode() => ... X ... Y ... ;
}
```

`EqualityContract` プロパティを用意しているのは、
対称性の確保のため。
このプロパティがないと、基底クラスのインスタンス `b` と派生クラスのインスタンス `d` があるとき、`b.Equals(d)` は true だけど `d.Equals(b)` は false みたいなことがあり得ます。

単に `GetType()` メソッドで型判定しないのは、
以下のような、追加のメンバーを持っていない派生クラスは互いに一致判定できるようにです。

```csharp {title="派生クラスの一致"}
class Base
{
    public value int Id { get; }
}

// 以下の2つの型は特に追加で value 修飾の付いたメンバーを持っていないので、
// Id さえ一致していれば互いに Equals 判定できる。
// EqualityContract はどちらも typeof(Base) を返す。
class Derived1 : Base { }
class Derived2 : Base { }
```

#### value types

また、`value class Point` みたいに型自体に value 修飾を付けることで、全メンバーに value 修飾を付けたのと同じ扱いにするという話も。

上記 `Derived1` と `Derived2` を、「`Id` が同じでも型が違えば `Equals` は false にしてほしい」(ようするに discriminated union 的な挙動)にしたいときはそれぞれ `value class Derived1`、`value class Derived2` と書く(逆に、true にしたいときは value 修飾を付けない)という話もあります。

### Removing construction boilerplate

長らく、以下のようなコードの冗長性が嫌だという話がずっと言われ続けています。

```csharp {title="冗長なコード"}
// プロパティ、コンストラクター引数、代入の左右の4か所で同じ名前を書くのが冗長
public abstract class Person
{
    public string Name { get; }
    public Person(string name)
    {
        Name = name;
    }
}
```

これを、最終的には `class Person(string Name);` くらいまで縮めたいというのが Records の肝なんですが、これも、いくつかの段階に分けて考えようとしているみたいです。

#### direct constructor parameters

まず、direct constructor parameters という案。
以下のように、コンストラクター引数に対応するプロパティだけを書くという方式。

```csharp {title="direct constructor parameters"}
public abstract class Person
{
    public string Name { get; }
    public Person(Name) // 型名なしで、プロパティ名だけ指定
    {
        // this.Name = Name 的なコードが追加される
 
        // 追加で、値の検証コードとか書くのは自由にできる
        if (Name is null) throw new ArgumentNullException(nameof(Name));
    }
}
```

#### primary constructors

次が primary constructors で、以下のように、クラス宣言の行に直接引数を書けるようにするもの。
検証コードの類は「`()` なしのコンストラクター」みたいな構文が提案されています。

```csharp {title="primary constructors"}
public abstract class Person(string name)
{
    public string Name { get; } = name;
 
    public Person // () なしのコンストラクター構文
    {
        // primary constructor に対する検証コードはここに書く
        if (name is null) throw new ArgumentNullException(nameof(Name));
    }
}
```

primary constructors は先ほどの direct constructor parameters と相乗効果あり。

```csharp {title="primary constructors + direct constructor parameters"}
public abstract class Person(Name) // primary constructors + direct constructor parameters
{
    public string Name { get; }
 
    public Person
    {
        if (Name is null) throw new ArgumentNullException(nameof(Name));
    }
}
```

#### primary constructor member declarations

プロパティと direct constructor parameters の重複も避けたいということで、さらに踏み込んだ文法として primary constructor member declarations があります。
primary constructor の引数の部分に直接メンバー宣言を書いてしまうもの。

```csharp {title="direct constructor parameters"}
public abstract class Person(public string Name { get; });
```

### Improvements for object inititalizers

`new Point { X = 1, Y = 2 }` みたいな初期化の方法を[オブジェクト初期化子](../../../../study/csharp/oop/oo_construct.md#member_initializer)と呼びます。
ただ、現状だと mutable なフィールド、もしくは、プロパティにしか使えないので、
immutable が重宝されるこのご時世にはつらいと言われています。
それに対する改善案がいくつか。

#### Strawman: Init-only properties

オブジェクト初期化子では書き換えられるけど、それ以外の場所では書き換え不能という意味で、set の代わりに init アクセサーを持つプロパティ(init-only properties)を認めようというもの。

```csharp {title="init アクセサー"}
public class Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
 
var p = new Point { X = 5, Y = 3 }; // OK
p.Y = 7; // エラー。初期化子以外での Y の書き換えは認めない
```

#### validation accessors for auto-properties

「get だけ[自動実装](../../../../study/csharp/oop/oo_property.md#auto)して、set 内の検証コードは普通に書きたい」ということがあるので、それを認めようかという話。

```csharp {title="validation accessors"}
public string Name
{
    // get の実装を省略
    get;
 
    // set には検証コードだけ書く
    set { if (value is null) throw new ArgumentNullException(nameof(Name)); }
}
```

前述の init アクセサーでも同様。

#### object initializers for direct constructor parameters

前節の direct constructor parameters を持っている場合には、
オブジェクト初期化子の構文(`new Point { X = 1, Y = 2 }` みたいなの)をコンストラクター呼び出し(`new Point(1, 2)`)に置換しようかという案。

[匿名型](../../../../study/csharp/start/sp3_inference.md#implicit)に対してはこういう類の変換をすることで immutable を実現しているので、匿名型と名前付きの型の不整合をなくそうという話になります。

### Non-destructive mutation and data classes

immutable な型のインスタンスに対して、非破壊な書き換え(non-destructive mutation)、すなわち、「コピーを作って一部のメンバーだけ書き換えたインスタンスを作りたい」ということが結構あります。
これに対して、以下のような with 構文を導入したいという話は前々からありました。

```csharp {title="with 構文"}
var p2 = p1 with { X = 4 };
```

問題は、この with 構文をどう解釈(どうコード生成)すべきかという点です。

#### withers through virtual factories

with は、以下のような `With` メソッドとそれの呼び出しに展開しようという案になっています。
`With` メソッドの生成トリガーにするために、クラスには data 修飾を求めようという話も。

```csharp {title="With メソッドの生成元には data 修飾を付ける"}
public data class Point(X, Y)
{
    public int X { get; }
    public int Y { get; }
}
var p2 = p1 with { Y = 2 };
```

以下のように展開されます。

```csharp {title="data class, with 構文の展開結果"}
public class Point(X, Y)
{
    public int X { get; }
    public int Y { get; }
 
    public virtual Point With(int X, int Y) => new Point(X, Y);
}
var p2 = p1.With(p1.X, 2);
```

この案では、どのプロパティがどのコンストラクター引数と対応しているのかがわかっていないといけないので、data class には前述の primary constructor が必須みたいです。

virtual なファクトリ メソッドを必要とするのは、以下のように、派生型のメンバーのコピーがちゃんと働くようにするためです。

```csharp {title="data class の派生"}
public data class Person(Name)
{
    public string Name { get; }
}
public data class Student(ID) : Person
{
    public int ID { get; }
}
```

以下のように展開されます。

```csharp {title="data class の派生の展開結果"}
public abstract class Person(Name)
{
    public string Name { get; }
    public virtual Person With(string Name) => new Person()
}
public class Student(ID) : Person
{
    public int ID { get; }
 
    public sealed override Person With(string Name) => With(Name, this.ID);
    public virtual Student With(string Name, int ID) => new Student(Name, ID);
}
```

#### Auto-generated deconstructors

data class では「どのプロパティがどのコンストラクター引数と対応しているのかがわかっていないといけない」、「primary constructor 必須」なので、
だったら[分解](../../../../study/csharp/datatype/deconstruction.md)用の `Deconstruct` メソッドも(プロパティと引数の結び付け、コンストラクターの逆パターンなので)自動生成できる状況になります。

`With` メソッドだけ、`Deconstruct` メソッドだけをそれぞれ別々に生成したいという要件はあまり重い浮かばず、「data 修飾を付ければ `With` も `Deconstruct` も生成」でいいだろうというような雰囲気。

#### Abbreviated data members

with 構文に data 修飾と primary constructor の引数が必須なのであれば、
data class はもう常に前述の「primary constructor member declarations」的な挙動をするという扱いでよさそうです。

要するに、以下のような書き方で、

```csharp {title="data class + primary constructor member declarations"}
public data class Point(int X, int Y);
```

プロパティ `public int X { get; }` と `public int Y { get; }` を生成したいという話に。

### data classes as value classes

value 修飾(値による比較、`Equals` の生成が目的)と data 修飾(非破壊な書き換え、`With`/`Deconstruct` の生成が目的)の2つの案が出たわけですが、割と似て非なる感があります。ただ、必ずしも同じではない。

とはいえ、data と value の2個の修飾子を常に両方書かないといけないというのが快適化というと微妙な感じ。

ただ、「value class は常に data class か」と言われるとおそらく違います。
data class の方が「primary constructor 必須」とかの制約が強くて、
値による比較だけが欲しくて使いにくいという場面は十分想定されます。
with 構文が求めている「インスタンスのコピー」自体を禁止したい場合もあると思います。

逆に、「data class は常に value class か」の方はたぶんその方が都合がよさそうです。

### 結論

Records がらみを小さな機能の集まりに分けたいという話でいろいろと検討していますが、
結局、いくつかの機能は不可分(data class には primary constructor が必須だったり、`With` 生成と `Deconstruct` 生成は常にセットだったり)なところはあります。
それでも、抜き出せる部分はちゃんと抜き出して個別の機能としたいし、特に、値による比較(value class)は個別に切り出すことが有用そうです。

まだ詳細を詰めないといけない部分は残っていますが、今回挙げた案で Records として求めらているものは大筋実現できそうな感じにはなっていると思います。
