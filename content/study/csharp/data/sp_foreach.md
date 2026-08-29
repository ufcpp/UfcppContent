---
title: "foreach"
source_url: "https://ufcpp.net/study/csharp/data/sp_foreach/"
content_type: "Article"
published_at: "2002-11-03T00:00:00"
updated_at: "2008-01-05T00:00:00"
tags: []
umbraco_id: 1299
parent_id: 1298
sort_order: 0
aliases:
  - "/study/csharp/sp_foreach.html"
---

# foreach

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

foreachとは、コレクションのすべての要素を1回ずつ読み出すための構文です。


##### <a id="sec-generated-title-2"></a>ポイント

* 配列みたいに for (int i = 0; i &lt; array.Length; ++i) { array[i] ... } という形で要素の列挙ができないようなコレクションも、foreach なら列挙可能。

* foreach (変数 in コレクション) { ... }



## <a id="sec-generated-title-3"></a> <a id="collection"></a>コレクション

<em>コレクション</em>(「コンテナ」ともいいます)とは配列やリスト、辞書などの複数の要素をひとつにまとめるクラスのことです。
複数の要素をまとめておく方法にはさまざまな方法があり、
その方法によって呼び名が変わります。
以下にコレクションの例とその簡単な説明を列挙します。

<table summary="">

	<tr>
		<td markdown="1"></td>
		<th>データ格納方式</th>
		<th>長所</th>
		<th>欠点</th>
	</tr>
	<tr>
		<th>配列</th>
		<td markdown="1">要素を単純に横に並べて置いておく。</td>
		<td markdown="1">処理の効率もメモリの使用効率もよい。また、任意の場所にある要素にいつでもアクセスできる。</td>
		<td markdown="1">末尾以外の場所に要素を挿入することが出来ない(出来ても効率が悪い)。</td>
	</tr>
	<tr>
		<th>連結リスト</th>
		<td markdown="1">セルと呼ばれる要素を入れておく箱を繋げていく。</td>
		<td markdown="1">任意の場所の要素の追加・削除が効率的に行える。</td>
		<td markdown="1">配列と比べ効率が落ちる。また、配列と違って前から順に要素をたどっていくことしか出来ない。</td>
	</tr>
	<tr>
		<th>探査木</th>
		<td markdown="1">左右に枝の伸びる木構造にデータを格納。 「左側の枝には小さな値、右側の枝には大きな値を格納する」といった条件をつけておく。</td>
		<td markdown="1">要素の検索・挿入・削除が効率的に行える。</td>
		<td markdown="1">要素を挿入した順序が意味を成さなくなる。</td>
	</tr>
</table>


ここでは詳細には触れませんが、
当サイト上にある「[C++ STL](../../stl/index.md)」や「[アルゴリズムとデータ構造](../../algorithm/index.md)」でもコレクションについて簡単な説明がありますので、興味のある方はそちらをご覧ください。
また、コレクションについてより詳しく知りたい方は検索エンジンで「データ構造 アルゴリズム」などをキーワードにして検索してみてください。

ここでは例として連結リストを示します。
あくまで例として示すだけなので、単純な実装方法を取っています。
(本来はもう少しちゃんとした実装の仕方をしないとだめ。)

```csharp {title="連結リストの例"}
using System;
using System.IO;

/// <summary>
/// リストのノード
/// </summary>
class Node
{
  public int elem;
  public Node next;

  public Node() : this(0, null){}

  public Node(int val, Node next)
  {
    this.elem = val;
    this.next = next;
  }
}

/// <summary>
/// 連結リストクラス
/// </summary>
class List
{
  public Node head;

  public List()
  {
    head = null;
  }

  /// <summary>
  /// リストに新しい要素を追加する。
  /// </summary>
  /// <param name="val">追加する値</param>
  public void Add(int val)
  {
    Node node = new Node(val, this.head);
    this.head = node;
  }
}
```



## <a id="sec-generated-title-4"></a> <a id="iEnumerable"></a>IEnumerable インターフェース

ここで1つ問題があります。
データの格納方式が違えば、当然データの読み出し方も変わってくるということです。
例えば、配列の場合、以下のようにすれば全ての要素を読み出せます。

```csharp {title="配列のデータ読み出し"}
int[] a = new int[]{1, 3, 5, 7};
for(int i=0; i<a.Length; ++i)
  Console.Write("{0}\n", a[i]);
```


しかし、上述の例に挙げたリストクラスに対して同じ操作を行おうとすると以下のようになります。

```csharp
List list = new List();
list.Add(7);
list.Add(5);
list.Add(3);
list.Add(1);
for(Node n=list.head; n!=null; n=n.next)
{
  Console.Write("{0}\n", n.elem);
}
```


同じ「コレクション内のすべての要素を1回ずつ読み出す」という操作なのに全然違うコードを書く必要があります。
コレクションごとにコードを変更するのは面倒ですし、
仕様の変更に柔軟に対応できないなどの問題があります。

そこで、コレクションクラスは共通のインターフェースを実装するという決まりを作り、
要素へのアクセスはこのインターフェースを通して行うのが一般的です。
そのためのクラスとして .NET Framework には <em>
        <code>IEnumerable</code>
      </em> というインターフェースが用意されています。
もちろん、C# の配列は <code>IEnumerable</code> インターフェースを実装しています。

<code>IEnumerable</code> インターフェースの実装の仕方については後ほど述べることにして、
ここでは <code>IEnumerable</code> インターフェースを介した要素へのアクセスの仕方のみを説明します。
<code>IEnumerable</code> インターフェースを介した要素へのアクセスは以下のようにします。

```csharp {title="IEnamerable インターフェースを介したコレクションのアクセス"}
int[] array = new int[]{1, 3, 5, 7};

IEnumerator e = array.GetEnumerator();
while(e.MoveNext())
{
  int val = (int)e.Current;
  Console.Write("{0}\n", val);
}
```


ここで、<code>IEnumerator</code> とは<em>列挙子</em>と呼ばれるクラスを作るためのインターフェースです。
<code>IEnumerator</code> インターフェースについては後ほど説明します。


## <a id="sec-generated-title-5"></a> <a id="foreach"></a>foreach文とは

<strong id="foreach" class="keyword">foreach 文</strong>を用いるとこで <code>IEnumerable</code> インターフェースを介した要素へのアクセスを簡単化することが出来ます。
以下のように、foreachを使うことでコレクションのすべての要素を1回ずつ読み出すことができます。

```csharp {title="foreachの使い方"}
foreach(型名 変数 in コレクション)
  文
```


このコードは以下のように展開されます。

```csharp {title="foreachの実態"}
try
{
  IEnumerator e = array.GetEnumerator();
  while(e.MoveNext())
  {
    型名 変数 = (型名)e.Current;
    文
  }
} 
finally
{
  Dispose処理
}
```

「Dispose処理」の部分は、コンパイル時点で`IDisposable`なことがわかっている型かどうかで実際に生成されるコードが変わります。
コンパイル時点で`IDisposable`なことがわかる場合は以下の通り。

```csharp {title="foreachのDispose処理(コンパイル時点でわかっている場合)"}
    ((IDisposable)e).Dispose();
```

逆に、わからない場合は以下のようになります。

```csharp {title="foreachのDispose処理(コンパイル時点でわかっている場合)"}
    IDisposable d = e as IDisposable;
    if (d != null) d.Dispose();
```

例えば、<code>int</code>型の配列の要素を読み出して画面に表示するには以下のようにします。

```csharp {title="foreachの例" highlight-lines="3"}
int[] array = new int[10]{1, 2, 4, 8, 16, 32, 64, 128, 256, 512};

foreach(int n in array)
{
  Console.Write(n + " ");
}
```


```console
1 2 4 8 16 32 64 128 256 512 
```


foreach文の実態は<code>IEnumerable</code> インターフェースを介した要素へのアクセスですから、
<code>IEnumerable</code> インターフェースを実装しているならどんなコレクションクラスの要素でも読み出すことが出来ます。
例えば、.NET Framework標準ライブラリの<code>ArrayList</code>クラスは<code>IEnumrable</code>インターフェースを実装していますので、以下のようにforeach文を使ってコレクション内の要素を列挙することが出来ます。

```csharp {title="ArrayListに対してforeachを使う" highlight-lines="8" highlight-ranges="1:1-1:10"}
ArrayList list = new ArrayList();

for(int i=0; i<10; ++i)
{
  list.Add(i * (i + 1) / 2);
}

foreach(int s in list)
{
  Console.Write(s + " ");
}
```


```console
0 1 3 6 10 15 21 28 36 45 
```



### <a id="sec-generated-title-6"></a> <a id="pattern-based"></a>余談： パターン ベース

余談になりますが、
foreach で使うコレクションは、実は IEnumerable を実装している必要はなくて、
GetEnumerator という名前のメソッドを持っていればどんな型でもよかったりします。
（要するに、「[パターン ベース](../misc/miscpatternbased.md)」。） 

### <a id="sec-generated-title-7"></a> <a id="extension-getenumerator"></a>拡張メソッドでの GetEnumerator 実装

<h5 class="version version9">Ver. 9</h5>

C# 8.0 まではパターン ベースと言っても、`GetEnumerator` メソッドはインスタンス メソッドである必要がありました。
これが C# 9.0 で緩和されて、[拡張メソッド](../functional/sp3_extension.md)での実装が認められました。

例えば、C# 8.0 で入った [`Range`](dataranges.md#range) に対して以下のような拡張メソッドを書くことで、`foreach (var i in x..y)` みたいな書き方ができるようになります。

```csharp
using System;
 
foreach (var i in 5..10)
{
    Console.WriteLine(i); // 5, 6, 7, 8, 9
}
 
static class RangeExtension
{
    public static RangeEnumerator GetEnumerator(this Range r) => new(r);
 
    public struct RangeEnumerator
    {
        private int _i;
        private int _end;
 
        public RangeEnumerator(Range r)
        {
            _i = r.Start.Value - 1;
            _end = r.End.Value;
        }
 
        public bool MoveNext() => ++_i < _end;
        public int Current => _i;
    }
}
```

(これまでは単に C# 1.0 時代からある文法に下手に手を入れるのが怖くて認められていなかっただけです。)


## <a id="sec-generated-title-8"></a> <a id="ownmaking"></a>コレクションクラスの自作

<code>IEnumrable</code>インターフェースを実装することで、foreach文で利用できるコレクションクラスを自作できます。

<code>IEnumrable</code>インターフェースには<code>GetEnumerator</code>メソッドがあり、このメソッドは<code>IEnumerator</code>インターフェースを返します。
コレクションクラスを自作する場合、この<code>IEnumerator</code>インターフェースを実装する<em>列挙子</em>も自作する必要があります。

<code>IEnumerator</code>インターフェースには<code>Current</code>というプロパティと<code>MoveNext</code>、<code>Reset</code>という2つのメソッドがあります。
<code>Current</code>プロパティはコレクション内の現在の要素を取得するためのもので、
<code>MoveNext</code>メソッドは列挙子をコレクションの次の要素に進めます。
また、<code>Reset</code>メソッドは列挙子を初期位置、つまりコレクションの最初の要素の前に戻します。

```csharp {title="コレクションクラスと列挙子の自作の例" highlight-ranges="7:20-7:31,25:7-25:22,27:40-27:51,41:19-41:26,49:17-49:25,64:17-64:22,88:22-88:35"}
using System;
using System.Collections;

/// <summary>
/// 片方向連結リストクラス
/// </summary>
class LinearList : IEnumerable
{
  /// <summary>
  /// 連結リストのセル
  /// </summary>
  private class Cell
  {
    public object value;
    public Cell next;

    public Cell(object value, Cell next)
    {
      this.value = value;
      this.next = next;
    }
  }

  /// <summary>
  /// LinearList の列挙子
  /// </summary>
  private class LinearListEnumerator : IEnumerator
  {
    private LinearList list;
    private Cell current;

    public LinearListEnumerator(LinearList list)
    {
      this.list = list;
      this.current = null;
    }

    /// <summary>
    /// コレクション内の現在の要素を取得
    /// </summary>
    public object Current
    {
      get{return this.current.value;}
    }

    /// <summary>
    /// 列挙子をコレクションの次の要素に進める
    /// </summary>
    public bool MoveNext()
    {
      if(this.current == null)
        this.current = this.list.head;
      else
        this.current = this.current.next;

      if(this.current == null)
        return false;
      return true;
    }

    /// <summary>
    /// 列挙子を初期位置に戻す
    /// </summary>
    public void Reset()
    {
      this.current = null;
    }
  }

  private Cell head;

  public LinearList()
  {
    head = null;
  }

  /// <summary>
  /// リストに新しい要素を追加
  /// </summary>
  public void Add(object value)
  {
    head = new Cell(value, head);
  }

  /// <summary>
  /// 列挙子を取得
  /// </summary>
  public IEnumerator GetEnumerator()
  {
    return new LinearListEnumerator(this);
  }
}

class ForeachSample
{
  static void Main()
  {
    LinearList list = new LinearList();

    for(int i=0; i<10; ++i)
    {
      list.Add(i * (i + 1) / 2);
    }

    foreach(int s in list)
    {
      Console.Write(s + " ");
    }
  }
}
```


```console
45 36 28 21 15 10 6 3 1 0 
```


<h5 class="version version2">Ver. 2.0</h5>

このようなコレクションクラスを自作する作業は結構面倒なんですが、
C# 2.0 ではこの作業を簡単化するための「[イテレーター](sp2_iterator.md#iterator)」という機能が追加されました。
詳しくは、「[イテレーター](sp2_iterator.md)」で説明します。


## <a id="sec-generated-title-9"></a> <a id="performance"></a>foreach 文のパフォーマンス

「[foreach文とは](#foreach)」で説明したように、
一般には、foreach 文は以下のようなコードに展開されます。
（IDispose を実装しない場合。
IDispose を実装するクラスの場合には、
さらに「[using ステートメント](../resource/oo_dispose.md#using)」で囲ったのと同じ扱いになります。）

```csharp {title="foreachの実態"}
IEnumerator e = array.GetEnumerator();
while(e.MoveNext())
{
  型名 変数 = (型名)e.Current;
  文
}
```


このコードだと、
MoveNext() や Current などのメソッド呼び出しのオーバーヘッドが結構大きくて、
<code>for(int i; i &lt; array.Length; ++i) 文;</code>
というようなコードに比べると少し実行効率が悪くなります。

ただ、配列に対して foreach を使った場合、
最適化がかかって for 文相当のコードに変換されるようで、
そこまで大きな差はなくなるようです。

## <a id="sec-generated-title-10"></a> <a id="await-foreach"></a>非同期 foreach

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0で非同期版の`foreach`が追加されました。
`await foreach` (`foreach`の前に`await`を付ける)という構文で、
[`IAsyncEnumerable<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.iasyncenumerable-1)インターフェイス(`System.Collections.Generic`名前空間)か、それと同じ[パターン](../misc/miscpatternbased.md)を満たす型の列挙ができます。

```csharp {title="非同期 foreach" highlight-text="await foreach"}
static async Task AsyncForeach(IAsyncEnumerable<int> items)
{
    await foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
```

詳しくは「[非同期foreach](../async/asyncstream.md#await-foreach)」で説明します。
