---
title: "ラムダ式"
source_url: "https://ufcpp.net/study/csharp/functional/sp3_lambda/"
content_type: "Article"
published_at: "2009-04-29T00:00:00"
updated_at: "2023-10-25T21:55:23"
tags:
  - "Ver. 3.0"
umbraco_id: 1280
parent_id: 1275
sort_order: 6
aliases:
  - "/study/csharp/sp3_lambda.html"
---

# ラムダ式

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version3">Ver. 3.0</h5>

<strong id="lambda" class="keyword">ラムダ式</strong>（lambda expression）と言うのは、
関数型言語と呼ばれるような種類のプログラミング言語における用語なのですが、
関数（メソッド）を整数などの変数と全く同列に扱う手法のことです。

C# 3.0 で導入されたラムダ式は、
以下のようなものだと思ってください。

1. 「[デリゲート](sp_delegate.md#delegate)」に対して代入すると、「[匿名メソッド式](sp_delegate.md#anonymous)」と同じ扱いになる。

2. Expression 型の変数に代入すると、式木（expression tree）データになる。



##### <a id="sec-generated-title-2"></a>ポイント

* C# 3.0 で導入されたラムダ式には、2通りの意味があります。
    * 匿名メソッドを 2.0 の頃の記法より簡単に書ける。

    * 上述の匿名メソッドと同じ記法で式木を作れる。



* 例：<code>Func&lt;int, int&gt; square = x =&gt; x * x;</code>



## <a id="sec-generated-title-3"></a> <a id="anonymous"></a>匿名メソッドの記法の簡略化

まず、1つ目。
ラムダ式は、
C# 2.0 の匿名メソッドをさらに簡便な記法で書けるようにするものとして使われます。
先に概要を書いてしまうと、以下のような感じ。

* 匿名メソッドの定義から、delegate とか { return } とかの記述を省略できる。

* 型推論機構が働く。


（2.0 の）匿名メソッド構文でできることはラムダ式でもできます。
C# 3.0 の開発者も、「もし、ラムダ式が先に導入されていれば、匿名メソッドの構文は不要だった」と言っています。

C# 2.0 までの匿名メソッドは、例えば、以下のような書き方をしていました。

```csharp {title="C# 2.0 の匿名メソッド"}
delegate(int n)
{
  return n > 0;
}
```


この匿名メソッドをラムダ式を使って書き直すと、以下のようになります。

```csharp {title="ラムダ式"}
(int n) => { return n > 0; };
```


{} の中身が単文の場合には、{} と return も省略できます。

```csharp {title="ラムダ式"}
(int n) => n > 0;
```


要するに、記法としては、以下のようになります。

```csharp {title="ラムダ式の記法"}
引数リスト => 式
```


ちなみに、文脈的に引数の型が明らかな場合、
型は省略できます。
（var と似たような型推定機能が働く。）
例えば、以下のようなデリゲートがあるとき、

```csharp {title="デリゲート Pred"}
delegate bool Pred(int n);
```


このデリゲートに対する代入式中にラムダ式を書く場合、
引数の型は int であることが明らかなので、
int を省略して以下のように書くことができます。
（n の型はコンパイラが推論してくれます。）

```csharp {title="引数の型推定" highlight-text="n =&gt; n &gt; 0"}
Pred p = n => n > 0;
```


あと、いちいちデリゲートを定義するのは面倒なので、
.NET Framework 3.5 では、Func という名前のデリゲートが標準で用意されています。
Func は「[ジェネリック](../oop/sp2_generics.md#generics)」を使って定義されていて、
例えば、上述の例の Pred デリゲートのように、
int 型の引数を1つとって、bool 型を返すようなデリゲートを以下のように表現できます。

```csharp {title="Func デリゲート"}
Func<int, bool>
```


式が複数になる場合は省略せずに {} でくくります。
（この場合は、{} の中身は匿名デリゲートと同じ書き方をする。return も書く必要あり。）

```csharp {title="ラムダ式（複文）"}
Func<int, int, int> f =
  (x, y) =>
  {
    int sum = x + y;
    int prod = x * y;
    return sum * prod;
  };
```



## <a id="sec-generated-title-4"></a> <a id="expression"></a>式木

一方、2つ目に関してですが、こちらは完全に新機能で、ラムダ式特有のものです。
匿名メソッドと違って、
ラムダ式は本当に<strong id="exp_tree" class="keyword">式木</strong>（expression tree）データとして扱うこともできます。

上述の例の
<code>Pred p = n =&gt; n &gt; 0;</code> のように、
デリゲートに代入する場合には、
ラムダ式は匿名メソッドと同じ扱い、
すなわち、コンパイル後には実行コードの状態になっています。

これに対して、ラムダ式を
Expression 型の変数に代入すると、式木データとして扱うことができ、
以下のように式中の項を取り出したりといった操作が可能です。

```csharp {title="ラムダ式をデータとして扱う"}
Expression<Func<int, bool>> e = n => n > 0;
BinaryExpression lt = (BinaryExpression)e.Body;
ParameterExpression en = (ParameterExpression)lt.Left;
ConstantExpression zero = (ConstantExpression)lt.Right;
```


インタプリタ型の関数型言語には、実行コードとデータを区別しないものがあって、
ラムダ式をあるときには実行コードとして、またあるときにはデータとして利用するということができました。
C# では「実行コードとデータを区別しない」というわけにはいかないですし、
デリゲートに代入するか Expression 型に代入するかによってコンパイル結果を変えることで、
関数型言語と似たような動作を実現しています。

ただし、ラムダ式をデリゲートに代入する場合と違って、
式木には少し制約があります。
式木にできるのは、単文の（{} を使わない）ラムダ式だけです。
以下の例では、1つ目のラムダ式はコンパイル可能ですが、2つ目はエラーになります。

```csharp {title="{} を使って書いたラムダ式は式木にできない"}
// ↓ これは OK
Expression<Func<int, bool>> p = n => n > 0;

// ↓ これは「式木に変換できません」と怒られる
Expression<Func<int, int, int>> f =
  (x, y) =>
  {
      int sum = x + y;
      int prod = x * y;
      return sum * prod;
  };
```


要するに、単文で書けるものしか式木にできません。
したがって、四則演算やメソッドコールは式木にできるんですが、
for や while などの制御構文は式木にできません。
（Expression 型にも、for や while に相当するノードはない。）

ちなみに、LINQ to SQL では、
このラムダ式を式木として扱う機能を使って、LINQ クエリ式の条件式などを式木データとして受け取って、
それを SQL クエリに変換してデータベースに問い合わせをかけるというようなことをしているようです。

例えば、以下のようなクエリ式を書いたとすると、

```csharp {title="LINQ to SQL の例"}
var q =
  from c in db
  where c.City == "London"
  select new {c.City};

foreach (var city in q)
  ...
```


db.Where や db.Select では、
データベースサーバに対して以下のような SQL を発行するしくみになっています。

```sql {title="上述のクエリ式から作られる SQL 文"}
SELECT TOP 1 [t0].[City]
FROM [Customers] AS [t0]
WHERE [t0].[City] = @p0
```


こういう動作は、<code>c.City == "London"</code> の部分をデリゲート（要するに実行コード）として受け取っていてはできません。式木データとして受け取って、その中身を見ながら SQL 文を作ります。


## <a id="sec-generated-title-5"></a> <a id="init"></a>初期化子

### <a id="sec-generated-title-6"></a> <a id="object-initializer"></a>オブジェクト初期化子

C# 3.0 では、オブジェクトの初期化を以下のような記法でできるようになりました。
このような記法を<strong id="objectinit" class="keyword">オブジェクト初期化子</strong> （object initializer）と呼びます。

```csharp {title="オブジェクト初期化子"}
Point p = new Point{ X = 0, Y = 1 };
```


ちなみに、このコードの実行結果は以下のようなコードと等価です。

```csharp {title="オブジェクト初期化子"}
Point p = new Point();
p.X = 0;
p.Y = 1;
```


この等価なコードを見ればわかると思いますが、
オブジェクト初期化子で指定できるのは public なメンバー変数またはプロパティのみです。
（初期化子を書く場所によっては protected や internal も可。
とにかく、初期化子を書いた場所からアクセスできる変数・プロパティのみ。）

ただし、初期化子を使うと、
プロパティへの値の代入を単文で書けるようになります。
これで何が嬉しいかというと、<em>クラスのメンバー変数の初期化や、式木への代入が可能になります</em>。

```csharp {title="式木への代入時のオブジェクト初期化子"}
Expression<Func<Point>> f = () => new Point { X = 0, Y = 0 };
// ↑式木には単文のラムダ式しか代入できない。

// 要するに、以下のような書き方はコンパイルエラーになる。
Expression<Func<Point>> f = () =>
{
  var p = new Point();
  p.X = 0;
  p.Y = 0;
  return p;
}
```


```csharp {title="クラスのメンバー変数初期化時のオブジェクト初期化子"}
class Triangle
{
    public Point A = new Point { X = 0, Y = 0 };
    public Point B = new Point { X = 1, Y = 0 };
    public Point C = new Point { X = 0, Y = 1 };
    // ↑メンバー変数の初期化に複文は書けないの。
}
```

#### <a id="sec-generated-title-7"></a> <a id="trailing-comma"></a>末尾コンマ

オブジェクト初期化子では、[配列の初期化子](../structured/st_array.md#use)と同様に、末尾のコンマはあってもなくてもかまいません。
以下の2行は同じ意味になります。

```csharp {title="初期化子の末尾コンマ" highlight-ranges="2:34-2:35"}
var p1 = new Point { X = 0, Y = 1 };
var p2 = new Point { X = 0, Y = 1, };
```

これは、後述するコレクション初期化子やインデックス初期化子でも同様です。

### <a id="sec-generated-title-8"></a> <a id="collection-initializer"></a>コレクション初期化

また、コレクションの初期化を以下のような記法でできるようになりました。
こちらは<strong id="collectioninit" class="keyword">コレクション初期化子</strong>（collection initializer）と呼びます。

```csharp {title="コレクション初期化子"}
List<int> list = new List<int> {1, 2, 3};
```


要するに、配列と同じような初期化記法を、任意のコレクションクラス（System.Collections.IEnumerable インターフェースを実装していて、Add メソッドを持つクラス）に対して行うことができます。
ちなみに、このコードは以下のようなコードと等価です。

```csharp {title="コレクション初期化子"}
List<int> list = new List<int>();
list.Add(1);
list.Add(2);
list.Add(3);
```


このようなリスト型のコレクションだけでなく、
IDictionary&lt;TKey,TValue&gt; のような辞書クラスに対しても、
以下のような記法で初期化ができます。
（この場合、2引数の Add メソッドが呼ばれます。）

```csharp {title="コレクション初期化子"}
var map = Dictionary<string, int>
{
  { "One", 1 },
  { "Two", 2 },
  { "Three", 3 },
  { "Four", 4 },
};
```

<h5 class="version version12">Ver. 12</h5>

C# 12 からはコレクション初期化子に代わって、以下のようにコレクションを作ることができるようになりました。
これをコレクション式といいます。

```csharp
int[] a = [1, 3, 5, 7, 9];
```

コレクション初期化子との差や、コレクション式のメリットなどは「[コレクション式](../datatype/collection-expression.md)」で説明します。



### <a id="sec-generated-title-9"></a> <a id="index-initializer"></a>インデックス初期化

<h5 class="version version6">Ver. 6.0</h5>

C# 6.0 から、[オブジェクト初期化子](#object-initializer)に、インデクサーを混ぜれるようになりました。
これを<strong id="key-index-initializer" class="keyword">インデックス初期化子</strong>(index initializer)といいます。

例えば `Dictionary`(`System.Collections.Generic`名前空間)に対して以下のような書き方ができます。

```csharp {title="インデックス初期化子の例"}
var dic = new Dictionary<string, int>
{
    ["one"] = 1,
    ["two"] = 2,
};
```

プロパティへの代入とインデクサーへの代入を混在させることもできます。

```csharp {title="初期化子内でのプロパティとインデクサーの混在"}
class Sample
{
    public string Name { get; set; }
 
    public int this[string key]
    {
        get { return 0; }
        set { }
    }
}
 
class Program
{
    static void Main()
    {
        var s = new Sample
        {
            Name = "sample",
            ["X"] = 1,
            ["Y"] = 2,
        };
    }
}
```

### <a id="sec-generated-title-10"></a> <a id="recursive"></a>再帰初期化

ちなみに、再帰的な構造を持ったクラスの初期化もできます。

```csharp {title="再帰的なオブジェクト初期化子"}
using System.Collections.Generic;

class Point
{
    public double X { get; set; }
    public double Y { get; set; }
}

class Color
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
}

class Geometry
{
    public List<Point> Vertices = new List<Point>();
    public List<int> Indices = new List<int>();
}

class Model
{
    public Geometry Geometry = new Geometry();
    public Color Color = new Color();
}

class Program
{
    static void Main()
    {
        Model m = new Model
        {
            Color = { R = 128, G = 128, B = 128 },
            Geometry =
            {
                Vertices =
                {
                    new Point { X = 0, Y = 0},
                    new Point { X = 1, Y = 0},
                    new Point { X = 1, Y = 1},
                    new Point { X = 0, Y = 1},
                },
                Indices = { 0, 1, 2, 0, 2, 3 },
            },
        };

        //Model m = new Model();
        //m.Color.R = 128;
    }
}
```


ただし、再帰的な初期化をするためには、メンバーが参照型（class）である必要があります。
例えば、上記の例で、Color が class ではなく struct だった場合、
コンパイルエラーになります。

また、この記法ででの初期化は、以下のようなコードと等価で、Color、Geometry、Indices などに対してインスタンスを new してくれたりはしないので注意が必要です。
コンストラクターもしくはメンバー初期化子での初期化が必要です。

```csharp {title="再起初期化子の解釈結果"}
        Model m = new Model();
        // ↓ m = new Model() の時点で Color が初期化されていないと NullReferenceException。
        m.Color.R = 128;
        m.Color.G = 128;
        m.Color.B = 128;
        m.Geometry.Vertices.Add(new Point { X = 0, Y = 0 });
        m.Geometry.Vertices.Add(new Point { X = 1, Y = 0 });
        m.Geometry.Vertices.Add(new Point { X = 1, Y = 1 });
        m.Geometry.Vertices.Add(new Point { X = 0, Y = 1 });
        m.Geometry.Indices.Add(0);
        m.Geometry.Indices.Add(1);
        m.Geometry.Indices.Add(2);
        m.Geometry.Indices.Add(0);
        m.Geometry.Indices.Add(2);
        m.Geometry.Indices.Add(3);
```


さもなくば、以下のように、おとなしく new を書きましょう。

```csharp {title="おとなしく new を明示的に書く"}
        Model m = new Model
        {
            Color = new Color { R = 128, G = 128, B = 128 },
            Geometry = new Geometry
            {
                Vertices = new List<Point>
                {
                    new Point { X = 0, Y = 0},
                    new Point { X = 1, Y = 0},
                    new Point { X = 1, Y = 1},
                    new Point { X = 0, Y = 1},
                },
                Indices = new List<int> { 0, 1, 2, 0, 2, 3 },
            },
        };
```
