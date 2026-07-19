---
title: "インデクサー"
source_url: "https://ufcpp.net/study/csharp/oop/oo_indexer/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2015-02-16T00:00:00"
tags: []
umbraco_id: 1261
parent_id: 1248
sort_order: 8
aliases:
  - "/csharp/oo_indexer"
  - "/csharp/oo_indexer.html"
  - "/csharp/oop/oo_indexer/"
  - "/study/csharp/oo_indexer"
  - "/study/csharp/oo_indexer.html"
---

# インデクサー

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# で利用できる基本型の1つに配列があります。
配列では i 番目の要素を読み書きする際、
<code>a[i]</code> というように <code>[]</code> を用います。

「[演算子のオーバーロード](oo_operator.md)」でも述べましたが、
ユーザー定義型の理想は、組込み型とまったく同じように扱えることです。
そこで、C# では、
ユーザー定義型が配列型と同様に <code>[]</code> を用いた要素の読み書きが行えるように<strong id="indexer" class="keyword">インデクサー</strong>という仕組みが用意されています。

インデクサーを定義することで、ユーザー定義型のオブジェクトでも、
配列と同じような <code>a[i]</code> という形での要素の読み書きができるようになります。


##### <a id="sec-generated-title-2"></a>ポイント

* 組み込み型（int や string など）とユーザー定義型（クラスや構造体）の区別をなくそう。

* ユーザー定義型にも、配列っぽく<code>[]</code>を使ったインデックスアクセスを定義できます（インデクサー）。

* 書き方は、T this [int index] { set { ... } get { ... } }



## <a id="sec-generated-title-3"></a> <a id="definition"></a>インデクサーの定義

インデクサーは以下のようにして定義します。

```csharp
アクセスレベル 戻り値の型 this[添字の型 添字]
{
  set
  {
    // setアクセサ
    //  ここに値の変更時の処理を書く。
    //  value という名前の変数に代入された値が格納される。
    //  添字が使える以外はプロパティと同じ。
  }
  get
  {
    // getアクセサ
    //  ここに値の取得時の処理を書く。
    //  メソッドの場合と同様に、値はreturnキーワードを用いて返す。
    //  こっちも添字が使える以外はプロパティと同じ。
  }
}
```


インデクサーの定義の仕方はプロパティの定義の仕方に似ています。
プロパティ名の代わりに <code>this[]</code> を使うことと、
添字が使えること以外はプロパティと同じです。

例えば、以下のように添字の下限と上限の両方を指定できる配列を作ることが出来ます。

```csharp
using System;

/// <summary>
/// 添字の下限と上限を指定できる配列。
/// </summary>
class BoundArray
{
    int[] array;
    int lower;   // 配列添字の下限

    public BoundArray(int lower, int upper)
    {
        this.lower = lower;
        array = new int[upper - lower + 1];
    }

    public int this[int i]
    {
        set { this.array[i - lower] = value; }
        get { return this.array[i - lower]; }
    }
}

class Program
{
    static void Main()
    {
        BoundArray a = new BoundArray(1, 9);

        for (int i = 1; i <= 9; ++i)
            a[i] = i;

        for (int i = 1; i <= 9; ++i)
            Console.Write("a[{0}] = {1}\n", i, a[i]);
    }
}
```


```console
a[1] = 1
a[2] = 2
a[3] = 3
a[4] = 4
a[5] = 5
a[6] = 6
a[7] = 7
a[8] = 8
a[9] = 9
```


インデクサーの添字は1つである必要はなく、
複数の添字を利用することが出来ます。

```csharp
using System;

/// <summary>
/// jagged array を使った行列。
/// rectangular array のように[i, j]という添字で要素の読み書き可能。
/// </summary>
class Matrix
{
  int[][] array;

  public Matrix(int rows, int cols)
  {
    this.array = new int[rows][];
    for(int i=0; i<rows; ++i)
      this.array[i] = new int[cols];
  }

  public int this[int i, int j]
  {
    set{this.array[i][j] = value;}
    get{return this.array[i][j];}
  }
}

class IndexerSample
{
  static void Main()
  {
    Matrix a = new Matrix(4, 4);

    for(int i=0; i<4; ++i)
      for(int j=0; j<4; ++j)
        a[i, j] = (i+1) * (j+3);

    for(int i=0; i<4; ++i)
    {
      for(int j=0; j<4; ++j)
        Console.Write("{0,4}", a[i, j]);
      Console.Write("\n");
    }
  }
}
```


```console
   3   4   5   6
   6   8  10  12
   9  12  15  18
  12  16  20  24
```


また、添字の型は整数型である必要はありません。
例えば、以下のように添字が <code>string</code> 型のインデクサーを持つ辞書クラスを作ることも出来ます。

```csharp
using System;

/// <summary>
/// Dictionary クラスの項目。
/// </summary>
internal class Item
{
  public string key;
  public string value;
  public Item next;

  public Item(string key, string value, Item next)
  {
    this.key = key;
    this.value = value;
    this.next = next;
  }
}

/// <summary>
/// 辞書クラス。
/// </summary>
class Dictionary
{
  Item head;

  public Dictionary()
  {
    this.head = new Item(null, null, null);
  }

  public string this[string key]
  {
    set
    {
      for(Item item = this.head.next; item != null; item =item.next)
        if(item.key == key)
        {
          item.value = value;
          return;
        }
      this.head.next = new Item(key, value, this.head.next);
    }
    get
    {
      for(Item item = this.head.next; item != null; item =item.next)
        if(item.key == key)
          return item.value;
      return null;
    }
  }
}

class IndexerSample
{
  static void Main()
  {
    Dictionary dic = new Dictionary();

    dic["ﾊｧ"]    = "( ﾟДﾟ)？";
    dic["ﾊｧﾊｧ"]  = "(;´Д｀)";
    dic["ﾎﾟｶｰﾝ"] = "( ﾟдﾟ)";
    dic["ｵﾏｴﾓﾅ"] = "(´∀｀)";

    Console.Write(dic["ﾊｧﾊｧ"]);
  }
}
```


```console
(;´Д｀)
```



## <a id="sec-generated-title-4"></a> <a id="level"></a>set/get で異なるアクセスレベルを設定

<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 では、
「[プロパティ](oo_property.md#property)」と同様に、
インデクサーの set/get アクセサそれぞれ異なるアクセスレベルを設定できるようになりました。

```csharp
int[] x;
public int this[int i]
{
    get { return x[i]; }
    private set { x[i] = value; }
}
```



## <a id="sec-generated-title-5"></a> <a id="indexed"></a>余談: VB のインデックス付きプロパティ

余談なんですが、
VB.NET なんかにはインデックス付きプロパティというものもあります。
（VB.NET の他、Delphi には配列プロパティという名前で同様な機能が。）
VB.NET にあって C# にない機能の代表格としてよく挙がるんですが、
C# にこの機能がない理由についてちょっと考えてみます。

（先に「[foreach](../data/sp_foreach.md)」とか「[コレクション](../lib/lib_other.md#collection)」、「[コレクション概要](../../algorithm/collection/collection.md)」の辺りを読んでおくといいかも。）

VB.NET のプロパティの構文は以下のような感じです。
（例として整数型のプロパティを作るなら）

```vbnet
Public Property X() As Integer
  Get
    Return x_
  End Get
  Set
    x_ = value
  End Set
End Property
```


```vbnet
obj.X = 0
```


で、VB の場合はプロパティが引数を取れます。

```vbnet
Public Property X(i As Integer) As Integer
  Get
    Return x_(i)
  End Get
  Set
    x_(i) = value
  End Set
End Property
```


```vbnet
obj.X(0) = 0
```


この構文、ある意味「名前付きインデクサー」ともいえます。
C# のインデクサーの構文は、なんか意味の分からない所に this が入って、
以下のような書き方をするわけですが、

```csharp
int[] x;

public int this[int i]
  {
    get{return this.x[i];}
  }
}
```


この、this の部分に自由な名前を書けるのが VB.NET のインデックス付きプロパティだと思ってください。
this を使うときには（要するにインデクサー） obj[i] という記法で、
名前を付けたときには obj.Name[i] という記法でインデクサーを使える。

まあ、便利そうな機能ではあるんですけど、
なぜか C# にはない。
「これだけは VB.NET が便利」なんてことも言われるんですが、
C# がインデックス付きプロパティ（あるいは、名前付きインデクサー）を採用しなかった理由ってのも、
想像付かなくはないんですよね。

多分なんですけど、
C# の言語設計者的には、
インデックス付きプロパティにするよりも、
コレクションクラス（配列とかのこと）を返すプロパティを使って欲しいんだと思います。
要するに、以下のような。

```csharp
int[] x;

public int[] X
  {
    get{return this.x;}
  }
}
```


単なる配列じゃなくて、もうちょっと細かい挙動をちゃんと書きたければ、
自分で ICollection なり IList なりを実装した内部クラスを書けと。

結構面倒なんですけど、
C# 設計者がそうして欲しかった理由は、
名前付きインデクサーだと foreach とかが使えないから。
例えば、以下のような利用側コードは、
コレクションを返すプロパティなら OK なんですが、
インデックス付きプロパティではできない。
（参考： 「[foreach](../data/sp_foreach.md)」。）

```csharp
foreach(int val in obj.X)
{
  Console.Write("{0}\n", val);
}
```


ちなみに、インデックス付きプロパティではなくて、
C# 2.0 で導入されたイテレータ構文を使ったプロパティなら、
簡単に作れて、かつ、foreach で使えます。
（参考： 「[イテレーター](../data/sp2_iterator.md)」。）

また勝手な推測が交じるんですが、
多分、C# の開発者は、C# 1.0 の設計段階からイテレータのアイディアをなんとなく持っていて、
そのためにあえてインデックス付きプロパティを C# に導入しなかったんじゃないかと。
