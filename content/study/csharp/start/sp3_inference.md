---
title: "型推論(暗黙的型付け)と匿名型"
source_url: "https://ufcpp.net/study/csharp/start/sp3_inference/"
content_type: "Article"
published_at: "2009-04-29T00:00:00"
updated_at: "2021-09-22T16:44:12"
tags:
  - "Ver. 3.0"
umbraco_id: 1215
parent_id: 1190
sort_order: 15
aliases:
  - "/study/csharp/sp3_inference.html"
---

# 型推論(暗黙的型付け)と匿名型

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

（※修正予定: 
型推論だけに絞って、「変数と式」の直後にでも移動。
匿名型の話は「クラス」の辺りか、「メソッド指向」か「データ処理」の辺りに移動。

<h5 class="version version3">Ver. 3.0</h5>

C# 2.0 以前、「静的型付け言語は冗長な記述が多くてめんどくさい」などと言われることがありました。
例えば、以下の例について考えてみてください。

```csharp
System.Collections.Generic.List<int> list =
  new System.Collections.Generic.List<int>();
```


「なんでこんな長ったらしい型名を左辺と右辺の両方で書かなきゃいけないんだ、
どっちか片方書けば、もう片方は推論できるだろう」という話です。

これに対して、C# 3.0 では、可能な限り型推論を行うような構文が追加されています。


##### <a id="sec-generated-title-2"></a>ポイント

* var： 変数の型を推論してくれる。<code>var x = 1;</code>なら x は int になる。

* 暗黙的配列：<code>new int[] { 1, 2, 3 }</code>を<code>new[] { 1, 2, 3 }</code>と書けるようになりました。

* 匿名型：<code>var anonymous = new { X = 1, Y = 2 };</code>みたいに、匿名のクラスを作ることができるようになりました。



## <a id="sec-generated-title-3"></a> <a id="implicit"></a>変数の型推論(変数の暗黙的型付け)

var キーワードを用いて、<strong id="type-inference" class="keyword">型推論</strong>（type inference）して、
暗黙的に型付けされたローカル変数（Implicitly typed local variables）を定義できるようになりました。

```csharp {title="var"}
var n = 1;
var x = 1.0;
var s = "test";
```


var を用いる際には、必ず初期値を伴う必要があります。
そして、初期値から、変数の型を自動判別（型推論）してくれます。
上記の例では、
<code>n</code> は <code>int</code>、
<code>x</code> は <code>double</code>、
<code>s</code> は <code>string</code> 型の変数になります。

注意すべき点は、
あくまで型の自動判別・推論であって、
<em>任意の型の値を代入できる万能な変数を作れるわけではない</em>ということです。
したがって、以下のように、初期値を伴わない宣言は（型の推論ができないので）エラーになります。

```csharp {title="var（間違い）"}
var n; // エラー。初期値が必要。
```


<code>TypeName x = new TypeName();</code> というように、
式の両辺に型名を書かないといけないのは冗長ではあります。
var は、この冗長さを省くため、左辺側の型名を省略できる機能だと思ってください。

ただし、冗長性がエラー耐性（2か所とも間違っていないとコンパイル エラーになって間違いに気付く）になっている場合もあるので、
<code>TypeName x = new TypeName();</code> という冗長な書き方も悪いことばかりではありません。


## <a id="sec-generated-title-4"></a> <a id="anonymous"></a>匿名型

C# 3.0 では<strong id="anonytype" class="keyword">匿名型</strong>（anonymous type）を作成できるようになりました。
匿名型の作り方は以下の通りです。

```csharp {title="匿名型"}
var x = new { FamilyName = "糸色", FirstName="望"};
```


このようなコードから、自動的に、以下のような型が生成されます。

```csharp {title="匿名型によって自動生成されるクラス"}
// ↓この __Anonymous という名前はプログラマが参照できるわけではない。
class __Anonymous1
{
  private string f1;
  private string f2;
  
  public __Anonymous1(string f1, string f2)
  {
    this.f1 = f1;
    this.f2 = f2;
  }

  public string FamilyName
  {
    get { return this.f1}
  };
  public string FirstName
  {
    get { return this.f2}
  };
  
  // あと、Equals, GetHashCode, ToString も実装
}
```


そして、変数 x に対して、
2つのプロパティ FamilyName と FirstName が使えます。

```csharp {title="匿名型の変数" highlight-text="x.FamilyName, x.FirstName"}
var x = new { FamilyName = "糸色", FirstName="望"};

Console.Write("{0}\n", x.FamilyName, x.FirstName);
```



##### <a id="sec-generated-title-5"></a>不変性

自動生成されたクラスを見てのとおり、自動実装されたプロパティには set アクセサーがありません。
要するに、読み取り専用（immutable: 不変）になります。

通常の「[オブジェクト初期化子](../functional/sp3_lambda.md#objectinit)」では、public な set アクセサーを持つプロパティしか初期化できませんでしたが、
匿名型の場合には、コンストラクター呼び出しに置き換えられます。

```csharp
var p = new Point { X = 1, Y = 2 };
// Point p = new Point();
// p.X = 1;
// p.Y = 2;
// と同じ意味。

var anonymous = new { X = 1, Y = 2 };
// __Anonymous anonymous = new __Anonymous(1, 2);
// みたいなコードが生成される。
```



##### <a id="sec-generated-title-6"></a>プロパティ名の省略

ちなみに、以下のように、他のクラスのプロパティを初期化子に渡す場合には、
「プロパティ名 =」の部分を省略することもできます。
（初期化子で渡したプロパティの名前がそのまま匿名クラスでも使われます。）

```csharp {title="プロパティ名の省略" highlight-text="var b = new { a.X, a.Y };"}
struct A
{
  public int X { set; get; }
  public int Y { set; get; }
  public int Z { set; get; }
}

class Program
{
  static void Main(string[] args)
  {
    A a = new A { X = 0, Y = 1, Z = 2};
    var b = new { a.X, a.Y };
    //↑ new { X = a.X, Y = a.Y } と同じ意味。
    Console.Write("{0}, {1}\n", b.X, b.Y);
  }
}
```



##### <a id="sec-generated-title-7"></a>LINQ との組み合わせ

まあ、匿名クラスは、その場限りの使い捨てなクラスになるわけで、
普通はあまり使うような機能ではありません。
基本的には、「[LINQ](../data/sp3_linq.md#linq)」 のための機能だと思っていいでしょう。
例えば、後述するクエリ式中で、以下のように利用します。

```csharp {title="匿名型の利用" highlight-text="select new { p.FamilyName, p.FirstName }"}
var list1 =
  from p in list
  where p.id <= 15
  orderby p.id
  select new { p.FamilyName, p.FirstName };
```



## <a id="sec-generated-title-8"></a> <a id="impl_array"></a>暗黙型付け配列

new で配列を作成する際、
型を省略できるようになりました。

```csharp {title="配列の暗黙的型付け" highlight-text="new[] {1, 2, 3, 4}"}
int[] array = new[] {1, 2, 3, 4};
```


見ての通り、
new の後ろの型を省略しています。
配列の型は、{} の中身の型から推定されます。
この例の場合、中身が 1, 2, 3, 4 といずれも int 型なので、
配列は int[] 型になります。

まあ、これだけだと、
ちょっとタイピングをサボれる程度ですが、
var および「[匿名型](#anonytype)」と組み合わせることによって、
真価が発揮されます。

```csharp {title="var と匿名型との組み合わせ"}
var array = new[]
  {
    new {X =  0, Y =  1},
    new {X =  3, Y = -1},
    new {X =  7, Y =  3},
    new {X = 13, Y = -5},
  };

foreach(var p in array) Console.Write("{0}\n", p);
```


配列宣言の中身が匿名なんだから、
new の後ろにどういう型名を書いたらいいかわかるはずがないですからね。
