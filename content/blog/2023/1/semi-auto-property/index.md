---
title: "【C# 12 候補】半自動プロパティ"
source_url: "https://ufcpp.net/blog/2023/1/semi-auto-property/"
content_type: "BlogEntry"
published_at: "2023-01-16T22:15:04"
updated_at: "2023-01-16T22:15:04"
tags: []
umbraco_id: 2452
parent_id: 2449
sort_order: 2
aliases: []
---

# 【C# 12 候補】半自動プロパティ

今日は半自動プロパティの話。

* 提案 issue: [Proposal: Semi-Auto-Properties; field keyword #140](https://github.com/dotnet/csharplang/issues/140)

[約1年前にも書いてる](../../../2021/12/semi-auto-property/index.md)通り、場合によっては C# 11 で入っていたかもしれないものです。

需要はそれなりに高いんですが、
案外課題があって結局スケジュール的に11からははずれ、「その後どうなったの？」とか思われていそうな機能です。
(12候補としては結構有力。)

半自動プロパティの話自体は[去年度](../../../2021/12/semi-auto-property/index.md)にしているので、
今日書くのはその「課題」をつらつらと。

## 半自動プロパティ概要

去年の繰り返しになるので概要のみ。
要は、手動で書く通常のプロパティ(以下、手動プロパティ)と自動プロパティの中間で、
バッキング フィールドのアクセスに `field` というキーワードを使おうというものです。

```csharp {title="手動、(全)自動、半自動プロパティ" highlight-ranges="18:27-18:32,18:41-18:46"}
class A
{
    // 手動プロパティ (manual property)
    // (と、自前で用意したフィールド)。
    // こういう、プロパティからほぼ素通しで値を記録しているフィールドを「バッキング フィールド」(backing field)という。
    private int _x;
    public int X { get => _x; set => _x = value; }

    // 自動プロパティ (auto-property)。
    // 前述の X とほぼ一緒。
    // バッキング フィールドの自動生成。
    public int Y { get; set; }

    // 【C# 12 候補】 半自動プロパティ (semi-auto-property)。
    // バッキング フィールドは自動生成。
    // 全自動の方と違って、バッキング フィールドの使い方は自由にできる。
    // field キーワードでバッキング フィールドを読み書き。
    public int Z { get => field; set => field = value; }
}
```

## field の “キーワード性”

半自動プロパティに類する提案は他にもありつつも、
現状はとりあえず「`field` キーワード」案で話が進んでいます。
キーワード追加。

ところが、この世に出ている C# コードの中には「`field` という名前のフィールドや変数」がそれなりにあって(オープンソースになっているコードとかを検索すると相当量出てくるそうで)、さすがに「`field` を文脈抜きに無条件にキーワード扱い」とかやるのは、破壊的変更としては許容できるレベルを超えていて、現実的ではないです。
`field` という単語は、最近提案されている新機能の中では断トツで(`record` や `required` すら霞むくらい)影響力が大きいかもしれません。

一方で、文脈キーワードの仕様はなかなかに複雑になりがちで、
今、[ちょっと単純化したいという話もあるくらい](../../../2021/2/lexicalkeywords/index.md)です。
そんな中、半自動プロパティでは早速苦戦しそうな雰囲気。

半自動プロパティの `field` は、極限まで突き詰めて「有効な時だけキーワード扱い」をやろうとすると `var` とか `record` とかよりもだいぶ難しいみたいです。
一例として挙がっているのは以下のようなコード。

```csharp {title="field の有効性の循環"}
unsafe struct S
{
    object Prop
    {
        get
        {
            S s = new();

            // このステートメントは「構造体 S が unmanaged のときだけ有効」
            // 言い換えると、「構造体 S が参照型のフィールドを持たないときだけ有効」
            // (C# 11 からは警告のみになったものの、元々はエラー。)
            var ptr = &s;

            // field が「S とは無関係な定数とか」だと &s が有効。
            // ところが、field がキーワードで、バッキング フィールドが自動的に作られると &s が無効になる。
            // 「&s が無効にならないようにこれは認めない」みたいなことまでやるのは解析が「循環」してしまう。
            return field;
        }
    }
}
```

なのであんまり正確にやるのはやめておいた方がいいとして、
簡素化した案でいうと以下のようなものがあります。

* セマンティクスを見るのは「`field` という名前のクラスと、`field` という名前のフィールドがあるかどうか」だけ
* あとは、構文的にだけ解析して、「スコープ内に `field` という名前の識別子がいるかどうか」で判定

簡素化するために「スコープを無視して解析」みたいな案もあるみたいなんですが、
結局は、以下のように「スコープも考慮に入れる」、「内側のスコープやローカル関数でのシャドーイングは認める」という予定だそうです。

```csharp {title="field キーワード/識別子のスコープ"}
object Prop
{
    get
    {
        {
            // この field は {} 内でだけ有効。
            int field = 1;
        }

        // このフィールドは m の内側でだけ有効。
        static void m(int field) { } 

        // {} とかローカル関数の外側には "field" がいないので、
        // ここの field はキーワード。
        return field;
    }
}
```

というのも、同スコープ内の解析に限っても、それなりに解析が大変そうな文法がいくつかあって、「労力は変わらない」とのこと。

```csharp {title="field のキーワード性の解析が大変そうなやつら"}
class C
{
    int Prop
    {
        get
        {
            var x = (field: 1, 2); // タプル要素名
            var y = new { field = 1 }; // 匿名型のプロパティ
            var z = new Foo() { field = 1 }; // オブジェクト初期化子でのフィールド/プロパティ参照
            if (x is { field: 1 }) { } // プロパティ パターンでのフィールド/プロパティ参照

            // 上記の field はいずれも、field という名前の変数が新たに導入されたりはしない。
            // このスコープ内に "field" はいないので、ここの field はキーワードでいいはず。
            return field;
        }
    }
}

class Foo { public int field; }
```

## 初期化子の挙動

C# の構造体には「すべてのフィールドを初期化しきるまで関数メンバー(メソッドやプロパティ)を呼べない」という仕様がありました。
(ただし、[C# 11 で緩和されました](../../../../study/csharp/cheatsheet/ap_ver11.md#auto-default)。)

```csharp {title="すべてのフィールドの初期化が必須" error-ranges="10:9-10:10" error-diagnostics="CS0188@10:9-10:10"}
struct S
{
    int _x;

    public void M() { }

    public S()
    {
        // C# 10 まではコンパイル エラーになってた。
        M(); // _x の初期化より前
        _x = 0;
    }
}
```

そんな中、C# 6 で[ get-only プロパティ](../../../../study/csharp/cheatsheet/ap_ver6.md#getter-only)の導入とともに、
「[コンストラクター内での自動プロパティへの代入は、それのバッキング フィールドへの直接代入への最適化を認める](../../../../study/csharp/cheatsheet/ap_ver6.md#struct-property-init)」という仕様も入っています。

```csharp {title="バッキング フィールドへの代入に展開"}
struct Point
{
    public int X { get; private set; }

    public Point(int x)
    {
        // C# 5.0まではエラーに。
        X = x;

        // これを認めるために、X = x の部分は「Xのバッキングフィールド = x」に展開される。
    }
}
```

その流れで、プロパティ初期化子も「バッキング フィールドへの代入に展開」されます。
例えば以下のようなコードを書いたとします。

```csharp {title="プロパティ初期化子"}
struct S
{
    public int X { get; private set; } = 1;
    public S() { }
}

record struct R(int X)
{
    public int X { get; private set; } = X;
}
```

このコードは、以下のようなコードとほぼ同じ挙動になります。

```csharp {title="バッキング フィールドへの代入に展開"}
struct S
{
    private int _x;
    public int X { get => _x; private set => _x = value; }
    public S()
    {
        _x = 1; // X = 1 ではなくて、_x = 1
    }
}

struct R
{
    private int _x;
    public int X { get => _x; private set => _x = value; }

    public R(int X)
    {
        _x = X; // this.X = X ではなくて、_x = 1
    }
}
```

という背景の中、半自動プロパティの場合はどうしようかという問題があります。
例えば以下のようなコードを認めたいんですが、
じゃあ、初期化時に `OnXChanged` は呼ばれるのかどうか。

```csharp {title="半自動プロパティのプロパティ初期化子"}
struct S
{
    // 流れ的にはこういうプロパティ初期化子も認めたい。
    public int X
    {
        get => field;
        private set
        {
            field = value;
            OnXChanged();
        }
    } = 1;

    public S() { }

    public void OnXChanged()
    {
        Console.WriteLine("何か副作用起こす");
    }
}
```

C# 11 での変更前は「自動プロパティと同様にせざるを得ない」と言われていました。
つまるところ、プロパティ初期化子はバッキング フィールドへの直代入に展開されて、
結果的に、`OnXChanged` は呼ばれないということになります。

C# 11 でこの要件は必然ではなくなったわけですが、
それでも「自動プロパティと同様」の仕様(`OnXChanged` は呼ばれない)になりそうな雰囲気です。

## override

override したときの挙動をどうしようかという問題もあります。
というのも、例えば以下のコードを考えます。

```csharp {title="自動プロパティの override"}
class Base
{
    // 自動プロパティなので、バッキング フィールドが作られる。
    public virtual int Prop { get; set; }
}

class Derived : Base
{
    // override してる時点で Base.Prop とは別物。
    // それをまた自動プロパティにすると、Base.Prop のものとは別に追加でバッキング フィールドができる。
    public override int Prop { get; set; }
}
```

自動プロパティの作るバッキング フィールドは `Base` と `Derived` で独立しています。
さらに、virtual なプロパティは「`get` だけ override」みたいなことができます。

```csharp {title="get だけ override"}
var x = new Derived { Prop = 2 }; // set は base.Prop のものがそのまま呼ばれる。
Console.WriteLine(x.Prop);        // get は Derived.Prop が呼ばれて、4 になる。

class Base
{
    public virtual int Prop { get; set; }
}

class Derived : Base
{
    // get だけ override して、base のものの二乗を返す。
    public override int Prop { get => base.Prop * base.Prop; }
}
```

そんな中、半自動プロパティでの override はどうしよう？という話になります。

```csharp {title="半自動プロパティでの override"}
var x = new Derived { Prop = 2 };
Console.WriteLine(x.Prop);

class Base
{
    public virtual int Prop { get; set; }
}

class Derived : Base
{
    // get だけ override して(全)自動プロパティというのはできない。
    // じゃあ、get だけ "半"自動プロパティは？
    // これは Base.Prop とは別のバッキング フィールドになる？
    public override int Prop { get => field * field; }
}
```

これはさすがにどう転んでもわかりにくいので、
いっそのこと、「半自動プロパティでの override はすべてのアクセサー(get/set 両方)の override が必須」とするそうです。

## nullability

半自動プロパティの導入の動機の1つに遅延初期化、
すなわち、以下のようなコードを書きたいというものがあります。

```csharp {title="遅延初期化目的の半自動プロパティ"}
public class LazyInit
{
    public string Value => field ??= ComputeValue();
    private static string ComputeValue() { /*...*/ }
}
```

この用途の場合、バッキング フィールドの型は `string?` であるべきなんですよね。

ところが、現状は「半自動プロパティから作られるバッキング フィールドの型はプロパティの型と同じ」という仕様なので、`string` になります。

参照型に関しては元から [`?` の有無はフロー解析](../../../../study/csharp/resource/nullablereferencetype.md)の差だけなのでそこまで問題ではないんですが、
値型の場合は困ります。

```csharp
public class LazyInit
{
    // field も int なので、 ?? が意味をなさない。
    public int Value => field ??= ComputeValue();
    private static int ComputeValue() { /*...*/ }
}
```

これは、「`field` キーワード」路線でやる以上は解決しようがなさそうで、
それとは別に「[プロパティ スコープ フィールド](https://github.com/dotnet/csharplang/issues/133)」(半自動プロパティと同じ要件に対する別案)が必要かもしれません。
とはいえ、とりあえず「`field` キーワード」優先で、
プロパティ スコープ フィールドはやるとしてもその後ということになっています。
