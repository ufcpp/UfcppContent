---
title: "イテレーター"
source_url: "https://ufcpp.net/study/csharp/data/sp2_iterator/"
content_type: "Article"
published_at: "2005-09-19T00:00:00"
updated_at: "2010-11-03T00:00:00"
tags:
  - "Ver. 2.0"
umbraco_id: 1300
parent_id: 1298
sort_order: 1
aliases:
  - "/study/csharp/sp2_iterator.html"
---

# イテレーター

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# の foreach 構文は、コレクションクラスの利用者側から見ると非常に便利な機能です。
しかしながら、実装側から見た場合、<code>IEnumerable</code>や<code>IEnumerator</code>インターフェースを実装する必要があり、結構面倒な作業が必要でした。

この実装側の労力を軽減するために、C# 2.0ではイテレーター構文というものが追加されました。


##### <a id="sec-generated-title-2"></a>ポイント

* イテレーター構文： IEnumerator を簡単に実装するための機能。

* return の代わりに yield return



## <a id="sec-generated-title-3"></a> <a id="block"></a>イテレーター ブロック

メソッドやプロパティのgetアクセサーなどを定義する際、ブロック中に`return`の代わりに`yield return`もしくは`yield break`を書くことで、通常のメソッドやプロパティとは違った動作が得られます。この、`yield return`もしくは`yield break`を含むブロックのことを<strong id="iterator" class="keyword">イテレーター</strong> ブロック（iterator block）と言いいます。

イテレーター ブロックを使うことで、「[foreach 文](sp_foreach.md#foreach)」で利用可能なコレクションを返すメソッドやプロパティを簡単に実装することができます。

```csharp
using System.Collections.Generic;

class TestEnumerable
{
  // ↓これがイテレーター ブロック。IEnumerable を実装するクラスを自動生成してくれる。
  static public IEnumerable<int> FromTo(int from, int to)
  {
    while(from <= to)
      yield return from++;
  }

  static void Main(string[] args)
  {
    // ↓こんな感じで使う。
    foreach(int i in FromTo(10, 20))
    {
      Console.Write("{0}\n", i);
    }
  }
}
```


ちなみに、yield という単語は「譲る」という意味です
（車文化のアメリカでは、「車線を譲る」（他の車を通すために速度を落としたり、脇道に止めたり）の意味でよく使われます）。
イテレーター ブロックの場合、"yield control to another method"（制御フローを他の処理に譲る）というような意味合いになります。

通常のブロック(メソッドやプロパティgetアクセサーの本体)との違いは以下の通りです。

* 戻り値の型が以下のうちのいずれか
    * System.Collections.IEnumerator

    * System.Collections.Generic.IEnumerator&lt;T&gt;

    * System.Collections.IEnumerable

    * System.Collections.Generic.IEnumerable&lt;T&gt;



* return の変わりに yield return というキーワードを使う。

* break の変わりに yield break というキーワードを使う。


上述の例の通り、
イテレーター ブロック中で、yield return 文が呼ばれるたびに、
foreach 文中で使われる値を1つ得ます。
for 文や while 文を使わず、ベタに yield return を並べても OK です。

```csharp
static public IEnumerable GetEnumerable(int from, int to)
{
  yield return 1;
  yield return 3.14;
  yield return "文字列";
  yield return new System.Drawing.Point(1, 2);
  yield return 1.0f;
}
```


また、yield break を記述した行まで処理が進むと、イテレーターの処理をそこで終了します。

イテレーター ブロックは静的（static）なものでもインスタンス（非 static）でも、
どちらでも定義できます。
また、プロパティ風の記述も可能です。
上述の例は static なメソッドですが、以下のような非 static なプロパティ風の定義も可能です。

```csharp
class FromTo
{
  int from, to;
  public FromTo(int from, int to){this.from = from; this.to = to;}

  public IEnumerable<int> Enumerable
  {
    get
    {
      while(from <= to)
        yield return from++;
    }
  }

  static void Main(string[] args)
  {
    foreach(int i in new FromTo(10, 20).Enumerable)
    {
      Console.Write("{0}\n", i);
    }
  }
}
```

## <a id="sec-generated-title-4"></a> <a id="restriction"></a>イテレーターの制限

イテレーター ブロックは、戻り値を返せるような関数メンバー(メソッド、演算子、プロパティのget、インデクサーのget)なら基本的には何に対してでも使えます。

ただし、いくつか制限があります。

まず、以下のような制限があります。

- [unsafe](../interop/sp_unsafe.md)にはできない。
  - 関数メンバーにunsafe修飾子は付けれない。
  - イテレーター ブロック内にunsafeステートメントは書けない<sup>※</sup>
- 引数を[ref, out](../resource/sp_ref.md)にはできない。
- [ref ローカル変数](../resource/sp_ref.md#ref-returns)を書けない<sup>※</sup>

(<sup>※</sup> このうち unsafe ステートメントと ref ローカル変数は、[C# 13 で書けるように](../cheatsheet/ap_ver13.md#ref-in-async)なりました。)

また、以下の場所には`yield return`、`yield break`共に書けません。

- finally 句内
- [匿名関数](../functional/sp_delegate.md#anonymous)の中
  - 匿名なイテレーター ブロック自体作れません。

そして、以下の場所には`yield return`を書けません。

- catch 句を持つ try 句内
  - (finally 句のみを持つ try 句内には`yield return`を書けます)
- catch 句内

## <a id="sec-generated-title-5"></a> <a id="GetEnum"></a>GetEnumerator

「[コレクションクラスの自作](sp_foreach.md#ownmaking)」で説明したように、
通常、foreach 文で利用できるコレクションクラスを自作するには、
IEnumerable インターフェースを継承し、
GetEnumerator メソッドをオーバーライドします。

C# 2.0 ではこのような方法の他に、
GetEnumerator と言う名前のイテレーター ブロックを定義することでも
コレクションクラスを作成できます。
ここでは、「[ジェネリック](../oop/sp2_generics.md)」で例に挙げた Stack クラスにイテレーターを追加してみましょう。

```csharp
class Stack<Type>
{
  Type[] buf;
  int top;
  public Stack(int max) { this.buf = new Type[max]; this.top = 0; }
  public void Push(Type item) { this.buf[this.top++] = item; }
  public Type Pop() { return this.buf[--this.top]; }

  public IEnumerator<Type> GetEnumerator()
  {
    for (int i = this.top - 1; i >= 0; --i)
      yield return buf[i];
  }
}
```



##### <a id="sec-generated-title-6"></a>サンプル

「[foreach](sp_foreach.md)」で挙げた例を、
ジェネリックスとイテレーターを用いて書き直してみます。

```csharp
using System;
using System.Collections.Generic;

/// <summary>
/// 片方向連結リストクラス
/// </summary>
class LinearList<T>
{
  /// <summary>
  /// 連結リストのセル
  /// </summary>
  private class Cell
  {
    public T value;
    public Cell next;

    public Cell(T value, Cell next)
    {
      this.value = value;
      this.next = next;
    }
  }

  private Cell head;

  public LinearList()
  {
    this.head = null;
  }

  /// <summary>
  /// リストに新しい要素を追加
  /// </summary>
  public void Add(T value)
  {
    this.head = new Cell(value, head);
  }

  /// <summary>
  /// 列挙子を取得
  /// </summary>
  public IEnumerator<T> GetEnumerator()
  {
    for(Cell c = this.head; c != null; c = c.next)
    {
      yield return c.value;
    }
  }
}

class ForeachSample
{
  static void Main()
  {
    LinearList<int> list = new LinearList<int>();

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



## <a id="sec-generated-title-7"></a> <a id="complied"></a>イテレーターのコンパイル結果

イテレーターは、
コレクションクラスを実装する際の手間が大幅に削減できる、
非常に便利な機能です。
ですが、少々抽象度が高く、イテレーター ブロックのコンパイル結果がどうなるのか、
ちょっと想像しづらいと思います。

中には、
中身の分からないものを使うのが怖いという方もいらっしゃるでしょうし、
怖いとまでは言わないものの、少しでもプログラムの効率をよくするために、
コンパイル結果がどうなるかを知りたいと言う方は多いと思います。
なので、イテレーター ブロックのコンパイル結果について少し触れておきます。
（ちなみに、C# 2.0 の仕様書中にも、このコンパイル結果に関する記事があります。）

イテレーターのコンパイル結果ですが、コンパイラが頑張ってくれていて、
結構凄いことをしています。
一種の状態機械（state machine）を自動生成していて、
例えば、先ほど例に挙げた Stack なら以下のようなコードと等価になるそうです。

```csharp
using System;
using System.Collections.Generic;
using System.Collections;

class Stack<T> : IEnumerable<T>
{
  T[] buf;
  int top;
  public Stack(int max) { this.buf = new T[max]; this.top = 0; }
  public void Push(T item) { this.buf[this.top++] = item; }
  public T Pop() { return this.buf[--this.top]; }

  public IEnumerator<T> GetEnumerator() {
    return new __Enumerator1(this);
  }
  class __Enumerator1: IEnumerator<T>, IEnumerator
  {
    int __state;
    T __current;
    Stack<T> __this;
    int i;

    public __Enumerator1(Stack<T> __this)
    {
      this.__this = __this;
    }

    public T Current
    {
      get { return __current; }
    }

    object IEnumerator.Current
    {
      get { return __current; }
    }

    public bool MoveNext()
    {
      switch (__state)
      {
        case 1: goto __state1;
        case 2: goto __state2;
      }
      i = __this.top - 1;

    __loop:
      if (i < 0) goto __state2;
      __current = __this.buf[i];
      __state = 1;
      return true;

    __state1:
      --i;
      goto __loop;

    __state2:
      __state = 2;
      return false;
    }
    public void Dispose()
    {
      __state = 2;
    }

    void IEnumerator.Reset()
    {
      throw new NotSupportedException();
    }
  }
}
```


C# 2.0 コンパイラは、
イテレーター ブロック内の for 文を、
この MoveNext メソッド内のようなコードに展開してくれるそうです。
やっていることを簡単に言うと、<code>yield return x;</code> の部分を以下のように置き換えています。

```csharp
state = State1; // 次に復帰するときのための状態の記録
Current = x;    // 戻り値を Current に保持
return true;    // いったん処理終了
case State1:    // 次に呼ばれたときに続きから処理するためのラベル
```

(疑似コードです。実際の C# では `case` に変数は使えないので、
「これに相当する `goto` が生成される」くらいのものだと思って読んでください。)

そして、最後に、これを switch 文で囲う(に相当する`goto`が挿入される)ことで、
処理の一時中断と再開を実現します。

ちなみに、このコードを見ての通り、
イテレーター ブロックによって得た IEnumerator は、
実は Reset メソッドをサポートしていません。
Reset を呼ぼうとすると NotSupportedException がスローされます。


## <a id="sec-generated-title-8"></a> <a id="dispose"></a>リソースの破棄

「[リソースの破棄](../resource/oo_dispose.md)」で説明したように、
ファイルなどの、.NET Framework の「[ガーベジコレクション](../cs4j/ab_csspec.md#gc)」の管理対象外のリソースは明示的な破棄が必要です。

リソースの破棄は、Dispose() メソッドなどを直接呼び出すことでもできますが、
以下のように、イテレーター ブロック中で Dispose() を呼び出しても、
正しく呼び出されない場合があります。

```csharp
static IEnumerable<string> Lines(string path)
{
  System.IO.StreamReader sr = new System.IO.StreamReader(path);

  string line;
  while ((line = sr.ReadLine()) != null)
  {
    yield return line;
  }

  sr.Dispose(); // この行は呼ばれないことがある
}
```

利用側の`foreach`ループに`break`などを書くと、`yield return`から後ろが実行されなくなります。
以下の例のように、`break`を1つ追加するだけで、イテレーター ブロック内の最後の1行が実行されなくなります。

```csharp
static IEnumerable<int> Iterator()
{
    Console.Write("1の前 ");
    yield return 1;
    Console.Write("1の後 ");
}

static void Foreach()
{
    var items = Iterator();

    // こちらのループの結果: 1の前 1を消費 1の後
    foreach (var item in items)
    {
        Console.Write($"{item}を消費 ");
    }

    // こちらのループの結果: 1の前 1を消費
    // (yield return より後ろ(1の後)は実行されない)
    foreach (var item in items)
    {
        Console.Write($"{item}を消費");
        break; // break 1つで挙動が変わる
    }
}
```

正しく sr.Dispose(); が呼ばれるようにしたければ、
イテレーター ブロック内で「[try-catch-finally 文](../structured/oo_exception.md#try)」や「[using ステートメント](../resource/oo_dispose.md#using)」を使います。

```csharp
static IEnumerable<string> Lines(string path)
{
  using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
  {
    string line;
    while ((line = sr.ReadLine()) != null)
    {
      yield return line;
    }
  }
}
```
