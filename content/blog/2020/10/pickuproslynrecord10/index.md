---
title: "ピックアップRoslyn: C# 10.0 でのレコード話"
source_url: "https://ufcpp.net/blog/2020/10/pickuproslynrecord10/"
content_type: "BlogEntry"
published_at: "2020-10-07T23:43:24"
updated_at: "2020-10-08T21:31:06"
tags:
  - "C# 10.0"
umbraco_id: 2312
parent_id: 2311
sort_order: 0
aliases: []
---

# ピックアップRoslyn: C# 10.0 でのレコード話

[先月書いた](../../9/csharp9final/index.md)通り、C# 9.0 がらみはほぼ確定(バグ修正レベルの変更しかしない状態)になっています。

(そういえば[ライブ配信](https://youtu.be/VQLtwak8W0U)はやったもののブログ化していなかった話題として、.NET 5.0 の RC 1 到達というのもあります。
RC (リリース候補)が付くと、もう大きな変更はできません。
あと、[.NET Conf](https://www.dotnetconf.net/) のページに「.NET Conf 2020 は11月10日開始」、「.NET 5 launch」の文字が入ったので、.NET 5.0 のリリース日も決まりました。アメリカ西海岸時間で11月10日なので、日本だと11月11日に朝起きたらリリースされているくらいのタイミング。)

そうなると、今デザイン作業が行われているのはすでにその次、C# 10.0 の話になります。

ということでここ2週間ほどの C# の Language Design Meeting はC# 10.0 がらみが議題になります。

- [C# Language Design Meeting for September 23rd, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-23.md)
- [C# Language Design Meeting for September 28th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-28.md)
- [C# Language Design Meeting for September 30th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-30.md)
- [C# Language Design Meeting for October 5th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-05.md)

10/8 に1件、10/7 日付の議事録の話を追記:

- [C# Language Design Meeting for October 7th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-07.md)

一番大きな話題は C# 9.0 で導入されるレコードに関するものです。
C# 9.0 時点では仕様を詰め切れていなくて「9.0 リリース後に再検討」となっていた項目がいくつかあって、
この2週ほどのミーティングではまさにその再検討な話が結構な割合を占めています。

ちょっと長くなりそうなので、今日はこのレコード関連の話だけを書こうかと思います。

他に、構造体(特に ref フィールド、ref 構造体の改善など)の話題とか、細かいトリアージ作業とかもあったりするんですが、この辺りは後日改めて。

## 前提知識: C# 9.0 レコード型

レコード型は、以下のように `record` キーワードを使って宣言する新しい型で、

```csharp {title="record 型宣言"}
record Point(int X, int Y);
```

内部的には以下のようなクラスの生成になります。

```csharp {title="Point レコードからのクラス生成"}
class Point
{
    public int X { get; init; }
    public int Y { get; init; }
    public Point(int X, int Y) // X, Y プロパティに代入
    public void Deconstruct(out int X, out int Y) // X, Y プロパティから値取得
    public override bool Equals(object? obj) // X, Y の値の比較
    public override int GetHashCode() // X, Y からハッシュ値生成
    public override string ToString() // Point { X = ... } の書式で文字列化
    public Point Clone() // shallow コピー (実際には通常の C# から参照できない名前で生成)
}
```

いくつかのとらえ方がありますが、以下のようなものとして説明されます。

- プレーンなデータを簡潔に書けるようにするための型
- [匿名型](../../../../study/csharp/start/sp3_inference.md#anonymous) (`new { X = 1, Y = 2 }` みたいなやつ)の名前付き版
- value semantics (値による比較やコピー生成)を持つクラス
    - C# の場合は構造体が元から value semantics 的な挙動を持っているので、「レコードは構造体的な性質を持つ参照型」ともいえる

ちなみに、この手の型は immutable にしないとまずかったりします。
わかりやすくまずいのは例えば以下のような場合。
ハッシュ値が変わってしまうことで `HashSet` や `Dictionary` の挙動を壊します。

```csharp {title="値比較を持っている参照型は immutable でないとまずい"}
using System;
using System.Collections.Generic;
 
var p = new Point { X = 1, Y = 2 };
 
// HashSet (ハッシュ値で等値比較してる)にインスタンスを渡す
HashSet<Point> set = new();
set.Add(p);
 
// その後、値を書き換え
p.X = 3;
 
// ハッシュ値が変わってしまってるので判定が狂う
Console.WriteLine(set.Contains(p)); // false
 
// Remove もできなくなる
set.Remove(p);
Console.WriteLine(set.Count); // Remove できてないので 1 が返る
 
class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool Equals(Point other) => (X, Y) == (other.X, other.Y);
    public override bool Equals(object? obj) => obj is Point other && Equals(other);
    public override int GetHashCode() => X ^ Y;
}
```

レコード型から生成されるクラスの例に `init` というキーワードが入っていますが、
これも C# 9.0 の新機能で、プロパティがオブジェクト初期化子までは書き換え可能、その後は書き換え不能になるという機能です。
既存のプロパティと比べて、

- set 可能プロパティ(`int X { get; set; }` みたいなの): どこでも書き換えできる
- get-only プロパティ (`int X { get; }` みたいなの): コンストラクター内でだけ書き換えできる
- init プロパティ (`int X { get; init; }` みたいなの): コンストラクター内とオブジェクト初期化子でだけ書き換えできる

```csharp {title="init プロパティ" error-ranges="sha256:f839a54c8868ab4dc7210dae09850471efcec5bfac36ea6fd0879e1baeb835cd;4:5-4:12,9:1-9:10,10:1-10:7"}
var p = new Point
{
    Settable = 1, // OK
    GetOnly = 1,  // ✖
    Init = 1,     // OK
};
 
p.Settable = 1; // OK
p.GetOnly = 1; // ✖
p.Init = 1; // ✖
 
class Point
{
    public int Settable { get; set; }
    public int GetOnly { get; }
    public int Init { get; init; }
 
    public Point()
    {
        Settable = 1; // OK
        GetOnly = 1;  // OK
        Init = 1;     // OK
    }
}
```

というものです。
C# の場合、[初期化子](../../../../study/csharp/oop/oo_construct.md#member_initializer)が C# 3.0 からの後付けなせいでちょっと使いにくかったんですが、その改善案になります。

immutable なデータを書き換えて使いたい場合、
shallow コピーを作ってからそのコピーの方を書き換えるというのが推奨される方式になります。
これに関しても C# 9.0 で「`with` 式」という新しい文法が追加されていて、
以下のような書き方でコピー＆ `init` プロパティの書き換えができます。

```csharp {title="with 式で immutable データの書き換え(コピー後にプロパティ書き換え)"}
using System;
 
var p1 = new Point(1, 2);
var p2 = p1 with { X = 3 };
 
Console.WriteLine(p1); // Point { X = 1, Y = 2 } (元のまま)
Console.WriteLine(p2); // Point { X = 3, Y = 2 } (新インスタンスで X が書き換わってる)
 
record Point(int X, int Y);
```

## C# 10.0 に持ち越されたレコード関連議題

いくつか、レコードにはいくつか議題が残っていて、「10.0 で改めて検討」となっているものがあります。

- レコードは参照型である
  - 値型版 (仮称 record struct)をどうするか
- 既存の構造体との兼ね合い
  - 「record struct」を新設すべきなのか、既存の構造体に手を入れるべきなのか
  - プロパティ生成とかはしないとしても、既存の構造体の時点で `with` 式を使える条件はそろってるはず
- プライマリ コンストラクター
  - 通常のクラスにも `class Point(int X, int Y)` みたいな書き方を認めたい
  ‐ その場合、単にコンストラクターの簡易記法であってプロパティなどのコンパイラー生成はしない

以下、
 [9/30](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-30.md)と[10/5](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-05.md)と[10/7](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-07.md)での検討事項。

### 構造体の等値比較

レコードでは「クラスに対して `Equals` メソッドをコンパイラー生成する」という仕組みで「値比較」を実現しています。

構造体の場合、`object.Equals(object)` の中で、.NET ランタイムが「値比較」に相当する処理を行っています。
なので、挙動としてはレコードと構造体の `Equals` はどちらも同じ「値比較」なんですが、
現状の構造体の `Equals` はちょっとパフォーマンスが悪いです。
これは、`Equals(object)`を介しているせいで[ボックス化](../../../../study/csharp/resource/rmboxing.md)が起こるのと、.NET ランタイム内での処理が[リフレクション](../../../../study/csharp/dynamic/sp_reflection.md#reflection)的になっているからです。

そこで、レコードと同じく構造体に対してもコンパイラー生成で「値比較」の `Equals` メソッドを生成すべきかどうかというのが議題になっていました。

これに関しては以下のような結論。

- コンパイラー生成の `Equals` は作らない方がいい
  - コンパイラー生成してしまうとコンパイル結果のバイナリ サイズが膨らむ
  - 既存の構造体に対して `Equals` 生成すると既存コードを壊す
  ‐ かといって、新しい「record struct (仮)」だけが高パフォーマンスみたいな状態になると、既存の構造体が忌むべきものになってしまう(それは望まない)
- 既存の構造体と record struct (仮)は明確に別
  - プライマリ コンストラクターからのプロパティ生成、型付きの `==` 演算子・`Equals`メソッド生成、`IEquatable<T>` 実装するのは record struct (仮)だけ
- .NET ランタイムのレベルで構造体の `Equals(object)` を最適化すべき
  - [.NET Core  2.1 のときに `enum` とかに対してやったの](../../../2018/12/jitintrinsics/index.md)と同じで、 .NET ランタイムが構造体の `Equals` を特別扱いして最適化できるはず

### 構造体に対する with 式

構造体は元から shallow コピーを持っている(単に代入するだけでコピー発生。[.NET の中間言語](../../../../study/il/index.md)的にも dup 命令ってのを持ってて、1命令でコピーになる)ので、`with` 式を使える条件を満たしています。

また、C# 9.0 で入るレコード(クラスで生成されるやつ)は、現状、コピーのカスタマイズ性がない(通常の C# からは参照できない(`$Clone<>`みたいな)名前でコピー メソッドが生成されていて、手書きでの上書きができないようにしてある)状態です。
これは「将来改めて検討する」ということになっていて、とりあえず、カスタマイズ性がある状態からない状態には戻せないけれど、できないものをできるようにすることは簡単だからいったん「ない」仕様にしてあります。

これに対して、C# 10.0 では以下のような方針(決定ではない)で進めていきたいようです。

- すべての構造体は `with` 式利用可能にする
- ただ、既存の構造体は `with` 時のコピーのカスタマイズ性は提供しない
  - デフォルト動作の「dup 命令でコピー」を常に使う
- record struct (仮) の場合は、レコード (9.0 で入るクラスのやつ)と合わせて再検討することになるけども…
  ‐ record struct (仮)のコピーのカスタマイズは認めない方がよさそう
  - でないと、ジェネリック型引数で `where T : struct` なものの挙動がおかしくなりそう

### プライマリ コストラクター

C# 9.0 のレコードでは、プライマリ コンストラクターの引数(`record Point(int X, int Y)` の `X`、`Y`)から public な init プロパティ(`public int X { get; init; }` とか)が生成されます。

C# 10.0 で検討している record struct (仮) でも同様であるべきかという話があります。

- record (参照型のレコード) と record struct (値型のレコード)という見方をすると、
同じ挙動であった方がいい
- 「record は名前付きの匿名型」に対して、「[タプル](../../../../study/csharp/datatype/tuples.md)の名前付き版」が欲しいという話もあって、record struct (仮) をその位置に据えたいという見方もある
  - この場合、タプルのメンバーは public フィールドになっているので、record struct (仮)は public フィールドを生成した方が合う
- immutable でないと問題を起こすのは参照型だけ
  - 値型の場合は代入で常にコピーが作られるので前述の `HashSet` みたいな問題を起こさない

そして検討の結果、現状、record struct (仮)に関しては以下のような方向性になりそうみたいです。

- デフォルトで public で <em>mutable</em> なプロパティ(`public int X { get; set;}` みたいなの)を生成する
- 手書きでカスタマイズ可能なので必要であれば `public int X { get; init; } を自分で足してもらう
- あるいは構造体の場合元から[readonly struct](../../../../study/csharp/cheatsheet/ap_ver7_2.md#readonly-struct)があるので、immutable にしたければ `readonly record struct Point(int X, int Y)` みたいに書いてもらう
- C# 9.0 時点のレコードには「プロパティかフィールドか」のカスタマイズ権はない(プロパティでないとダメ)ので、フィールドでも上書きできるように変更する

## record struct

10/8 追記:

record struct (仮) と、(仮) を付けて書いていたのは、具体的な文法をどうするかが決まっていなかったからです。
10/5 のものまでは、原文でも theoretical (理論上の)とか hypothetical (仮説上の)という前置き付きで record structs と言っていました。

[10/7](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-07.md) で具体的な文法が検討されて、以下のような方向性になりました。

- `record struct` (構造体に `record` 修飾)と `struct record` (レコードに `struct` 修飾)だと前者の方を選ぶメンバーが多かった
- となると、`record` 修飾子と考える方が自然
- `record class` も認めたい
- `record` は、そのうちよく使う方の `record class` の短縮形だという風に考えたい
- なので、`record struct` という文法を足すと同時に、`record class` も認める

### data メンバー

プライマリ コンストラクター (`record Point(int X, int Y)` みたいなの)からのプロパティ生成は、
常にコンストラクター生成がセットで、コンストラクターでの初期化が前提になります。

必然的に以下のような書き方になって、コンストラクター呼び出しには引数順序に意味があるので、
これを「位置によるレコード」(positional record)と呼んだりします。

```csharp {title="positional record"}
var p = new Point(1, 2);
```

これに対して、init プロパティだけを書いて、

```csharp {title="プライマリ コンストラクターは使わず init プロパティを定義"}
record Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
```

オブジェクト初期化子を前提にした書き方をすることもできます。
こちらはプロパティ名指定が必須で、逆に順序には意味がなくなるので、「名前によるレコード」(nominal record)と呼んだりします。

```csharp {title="nominal record"}
var p = new Point { X = 1, Y = 2 };
```

これはこれで便利なんですが、レコード型の「プレーンなデータを簡潔に書けるようにする」という目的からすると、
`public int X { get; init; } `という書き方はちょっと煩雑過ぎます。

そこで提案されているのが `data` メンバーで、以下のようなコードから `public int X { get; init; } ` をコンパイラー生成したいというものです。

```csharp {title="data キーワードで nominal record を定義"}
record Point
{
    data int X;
    data int Y;
}
```

この案自体はちょっと前からあって、単純に案が出たのがギリギリ過ぎて C# 9.0 には入れなかったという状態です。

ここで改めて record struct (仮)が議題になるんですが、

- プライマリ コンストラクターからのプロパティ生成の仕方が違うけど、data メンバーの場合はどうするべきか
  - C# 9.0 の参照型レコードは immutable (`get; init;`)
  - record struct (仮) は mutable (`get; set;`)

という問題があります。

ここはまだだいぶ悩んでいるようで、以下の3案が全部まだ候補だそうです。

- positional に合わせるべきで、値型の場合は mutable (`get; set;`)、参照型の場合は immutable (`get; init;`)
- `data` の挙動は一致しているべきで、値型だろうと参照型だろうと immutable (`get; init;`)
- `data` メンバーという提案自体をあきらめる

10/8 追記:

[10/7](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-07.md) では「`public int X { get; init; }` を `data int X` に縮める」という案が本当に有効かどうか、具体的なシナリオを検討したみたいです。

- [discriminated union](https://github.com/dotnet/csharplang/discussions/2962) を考えるとき、例1みたいなのには魅力を感じるけども、例2みたいなのはいまいちで、だったら `data` メンバーはそんなに「求めていたもの」じゃない

```csharp {title="単一行 discriminated union"}
// 例1: discriminated union (仮) として単一行メンバーなら書きたいモチベーションになる
record Union
{
    A;
    B(int X);
}
```

```csharp {title="単一行 discriminated union"}
// 例2: data メンバーを使って書く場合複数行に。これは魅力的か？
record Union
{
    A;
    B
    {
        data int X;
    }
}
```

- [required property](https://github.com/dotnet/csharplang/issues/3630) を考えるとき、`data` メンバーが required (オブジェクト初期化子で値を渡すことが必須)かどうかを変更できる追加のキーワードが必要(なので、記述が短くならないか、もしくは、`data` とは別のさらに追加のキーワードが必要)
- 通常のクラスにもプライマリ コンストラクターを定義できるように当たって、`class X(int X, data int Y)` みたいな書き方で、「`X` は単なるパラメーター、`Y` はレコードと同じくパラメーターからのプロパティなどの生成を行う」みたいなキーワードにしたいという案もある

この辺りが悩ましくて、C# 10.0 のタイミングでは `data` メンバーはやめておこうという雰囲気みたいです。
discriminated union、required property、通常クラスのプライマリ コンストラクターの3つはいずれも C# 10.0 で検討している機能で、それが具体的に決まった後でないと、`data` キーワードの有効性がどうなるかわからないということで、C# 10.0 の後に再検討した方がいいだろうという感じ。
