---
title: "C# 2.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver2/"
content_type: "Article"
published_at: "2015-05-06T14:06:40"
updated_at: "2025-01-01T12:53:56"
tags:
  - "Ver. 2.0"
umbraco_id: 1178
parent_id: 1174
sort_order: 4
aliases:
  - "/csharp/ap_ver2"
  - "/csharp/ap_ver2.html"
  - "/csharp/cheatsheet/ap_ver2/"
  - "/study/csharp/ap_ver2"
  - "/study/csharp/ap_ver2.html"
---

# C# 2.0 の新機能

## <a id="sec-generated-title-1"></a> <a id="ver2"></a>C# 2.0

<div class="version version2">Ver. 2.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2005/10</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2005</li>
<li>.NET Framework 2.0</li>
<li>Visual Basic 8</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>ジェネリック</li>
<li>イテレーター</li>
</ul>
</td>
</tr>
</table>

2005年、Visual Studio 2005 の発売に合わせ、
.NET Framework および C# がメジャーバージョンアップして、
.NET Framework 2.0、C# 2.0 になりました。

バージョン 1.0 から 1.1 へのバージョンアップはほとんどバグフィックスのみで、
機能的な変更は小さなものでした。
これに対し、2.0 へのメジャーバージョンアップでは、
少し大きな機能追加がありました。

ここでは、その追加機能、すなわち、C# 2.0 の新機能について説明します。


## <a id="sec-generated-title-2"></a> <a id="generics"></a>Generics

C++ で言うところの template。
（ただし、template とは実装の方式が違います。）
以下のような感じで、“型をパラメータに持つ型”を作ることが出来ます。

```csharp
public class Stack<T>
{
	T[] items;
	int count;
	public void Push(T item) {...}
	public T Pop() {...}
}
```


Generics には、
コンパイル時に型のチェックが可能、ボクシング・ダウンキャストが不要などという利点があり、
開発の効率およびプログラムの実行効率が期待できます。

Generics はクラス、構造体、インターフェース、デリゲート、メソッドに対して適用可能です。

詳細は「[ジェネリック](../oop/sp2_generics.md)」にて説明します。


## <a id="sec-generated-title-3"></a> <a id="anonymous"></a>匿名メソッド

匿名メソッドとは、インラインに(コード中に直に)メソッドを記述できる機能です。
例えば、今までなら、イベントハンドラを定義するときに、
以下のように1度メソッドを定義してからデリゲートにそのメソッドを渡していました。

```csharp
class InputForm: Form
{
  ...
  public InputForm()
  {
    ...
    addButton.Click += new EventHandler(AddClick);
  }

  void AddClick(object sender, EventArgs e)
  {
    listBox.Items.Add(textBox.Text);
  }
}
```


それに対して、匿名メソッドを使った書き方では、
以下のようにコード中に直接メソッドを書くことが出来ます。

```csharp
class InputForm: Form
{
  ...
  public InputForm()
  {
    ...
    addButton.Click += delegate
    {
      listBox.Items.Add(textBox.Text);
    };
    // ↑デリゲートの型は自動的に判別されます。
    /*
     * 引数つきの匿名デリゲートも定義できます。
     * ↑の例の場合、最初の行は
     * addButton.Click += delegate(object sender, EventArgs e)
     * と書くことも出来ます。
     */
  }
}
```


それから、匿名メソッドとは直接関係のない話ですが、
以下のように、メソッドを暗黙的にデリゲートに変換することが出来るようになりました。

```csharp
  static double[] Apply(double[] a, Function f) { ... }

    Apply(a, new Function(Math.Sin)); // 今までの書き方
    Apply(a, Math.Sin);               // Ver. 2.0 から
```


<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では、「[ラムダ式](../functional/sp3_lambda.md#lambda)」という記法を使って匿名デリゲートを書けるようになりました。
こちらの記法の方が簡便なため、delegate キーワードを使った匿名デリゲートの書き方はもう使われなくなると思われます。


## <a id="sec-generated-title-4"></a> <a id="interator"></a>イテレータ

C# の foreach 構文は、コレクションクラスの利用者側から見ると非常に便利な機能です。
しかしながら、実装側から見た場合、<code>IEnumerable</code>や<code>IEnumerator</code>インターフェース実装する必要があり、結構面倒な作業が必要でした。

この実装側の労力を軽減するために、C# 2.0ではイテレータ構文というものが追加されました。
イテレータ構文は、コレクションクラスから要素を得る(yield: 産出する、利益を生む)ための構文で、以下のような書き方をします。

```csharp
using System.Collections.Generic;
public class Stack<T>: IEnumerable<T>
{
  T[] items;
  int count;
  public void Push(T data) {...}
  public T Pop() {...}
  public IEnumerator<T> GetEnumerator()
  {
    for(int i = count - 1; i >= 0; --i)
      yield return items[i];
  }
}
```


<code>GetEnumerator()</code> メソッド中で、
<code>yield</code> というキーワードを用いて値を返すことで、
自動的に <code>IEnumerator</code> インターフェース実装するクラスを生成してくれます。
また、イテレータは以下のように、<code>IEnumerable</code> を返すメソッド/プロパティとしても定義することが出来ます。

```csharp
  public IEnumerable<T> BottomToTop
  {
    get
    {
      for(int i = 0; i < count; i++)
        yield return items[i];
    }
  }
```


利用者側では以下のようにして使用します。

```csharp
    Stack<int> stack = new Stack<int>();
    for (int i = 0; i < 10; i++) stack.Push(i);
    foreach (int i in stack) Console.Write("{0} ", i);
    Console.WriteLine();
    foreach (int i in stack.BottomToTop) Console.Write("{0} ", i);
    Console.WriteLine();
```


詳細は「[イテレーター](../data/sp2_iterator.md)」にて説明します。


## <a id="sec-generated-title-5"></a> <a id="partial"></a>Partial Type

C# 2.0 では、クラスや構造体などの型を複数のソースファイルに分けて記述できるようになりました。
分けて記述したい型には、以下のように <code>partial</code> キーワードを付けます。

```csharp
public partial class Customer
{
  private int id;
  private string name;
  private string address;
  private List<Order> orders;
  public Customer() { ... }
}
// ↑のクラスと↓のクラスは別ファイルに記述可能。
public partial class Customer
{
  public void SubmitOrder(Order order) { orders.Add(order); }
  public bool HasOutstandingOrders() { return orders.Count > 0; }
}
```


この2つのクラスを記述したファイルを一緒にコンパイルすることで、1つのクラスに結合することが出来ます。
クラスの結合はコンパイル時に行う(DLL 参照時にはできない)ので、Partial Type の全ての部分を一緒にしてコンパイルする必要があります。


## <a id="sec-generated-title-6"></a> <a id="nullable"></a>Nullable 型

Nullable 型は、値型の型名の後ろに <code>?</code> を付ける事で、元の型の値または <code>null</code> の値を取れる型になるというものです。
<code>int</code> 型で例に取ると、以下のような書き方が出来ます。

```csharp
int? x = 123;
int? y = null;
if (x.HasValue) Console.WriteLine(x.Value); 
if (y.HasValue) Console.WriteLine(y.Value);
```


上述の例のよう、<code>int?</code> 型は、
整数値または <code>null</code> 値の代入および、値を持つかどうかの判別が出来る型になります。
また、以下のように、Nullable 型同士の演算を行う際には、値が <code>null</code> かを自動的に判別してくれます。

```csharp
int? x, y;
....
int? z = x + y;
```


上述のようなコードを書いた場合、以下のコードと同じ意味合いになります。

```csharp
int? z = x.HasValue && y.HasValue ? x.Value + y.Value : (int?)null;
```


さらに、Nullable 型に対する特別な演算子として、<code>??</code> 演算子というものが追加されました。
（null coalescing operator といいます。
訳すなら、null 結合演算子。）
<code>??</code> 演算子は、値が <code>null</code> かどうかを判別し、<code>null</code> の場合には別の値を割り当てる演算子です。

```csharp
// x, y は int? 型の変数
int? z = x ?? y; // x != null ? x : y
int i = z ?? -1; // z != null ? z.Value : -1
```


詳細は「[Nullable 型](../resource/sp2_nullable.md)」にて説明します。


## <a id="sec-generated-title-7"></a> <a id="level"></a>アクセサのアクセスレベル

以下に示すように、プロパティの set/get アクセサ別個のアクセスレベルが設定可能になりました。

```csharp
public class A
{
  public int P
  {
    protected set {...}
    get {...}
  }
}
```



## <a id="sec-generated-title-8"></a> <a id="static"></a>static クラス

static メンバーのみを持ち、インスタンスの作成が不可能なクラスを作りたいことがしばしばあります。
C# 1.0 では、private なコンストラクタを持つ sealed クラスとしてこのようなクラスを作成していました。
このような方法で、「インスタンスが作成不可能」という制約は満たすことが出来ますが、
非 static なメンバーを定義することができてしまうという問題がありました。
(決してアクセスすることの出来ない無駄なメンバーになってしまいます。)

それに対して、C# 2.0 では、
クラス定義時に <code>static</code> をつけることで、
static メンバーしか定義できないクラスを作ることが出来ます。


## <a id="sec-generated-title-9"></a> <a id="alias"></a>namespace alias qualifier

C# では、基本的に、名前空間に対しても、クラスに対しても、
<code>using</code> 文で作ったエイリアスに対しても、
全て <code>.</code> 修飾子(qualifier: 限定子とも訳す)を用いて名前を繋いでいました。

```csharp
namespace Namespace
{
  public class A{}
  namespace Namespace2{ public class A{} }
}
public class Class
{
  public class A{}
}

class X
{
  using Alias = Namespace.Namespace2; // エイリアスを付ける。

  Namespace.A a1; // Namespace(名前空間) . A(クラス)
  Class.A     a2; // Class(クラス)       . A(クラス)
  Alias.A     a3; // Alias(エイリアス)   . A(クラス)
                  // ↑全部 .
}
```


もし、同一プロジェクト内の他の場所で Alias という名前のクラスまたは名前空間を追加作成すると、
このコードはエラーを起こしてしまいます。

```csharp
class Alias{ public class A{} }
// ↑プロジェクト内のどこか他の場所にこの1行を追加すると・・・

class X
{
  using Alias = Namespace.Namespace2;

  Alias.A a3; // エラー。クラスの Alias？それともエイリアスの方？
}
```


そこで、C# 2.0 では、<code>::</code> 修飾子というものが追加されました。
<code>::</code> 修飾子は、<code>.</code> 修飾子とは異なり、
using 文もしくは後述する extern alias という構文を使って作成したエイリアスのみを参照します。
したがって、後々になって、プロジェクトにエイリアスと同じ名前を持つクラスや名前空間を追加してもエラーにはなりません。

```csharp
namespace N
{
  public class A {}
  public class B {}
}
namespace N
{
  using A = System.IO; // using 文でエイリアス作成。
  class X
  {
    A.Stream s1;  // エラー。class A なのか A = System.IO なのか分からない。
    A::Stream s2; // OK。using 文で作ったエイリアス(A = System.IO)しか参照しない。
  }
}
```



## <a id="sec-generated-title-10"></a> <a id="extern"></a>extern alias

コンパイル時に、<code>/r</code> オプションで DLL の参照を指定する際に、
<code>/r:X=xxx.dll</code> というようにエイリアスを付けることが出来るようになりました。

```console
csc /r:X=xxx.dll /Y:yyy.dll test.cs
```


```csharp
extern alias X; // コンパイル時にオプションで指定したエイリアス
extern alias Y;
class Test
{
  X::N.A a;  // xxx.dll 内の N.A
  X::N.B b1; // xxx.dll 内の N.B
  Y::N.B b2; // yyy.dll 内の N.B
  Y::N.C c;  // yyy.dll 内の N.C
}
```



## <a id="sec-generated-title-11"></a> <a id="pragma"></a>#pragma

pragma プリプロセッサ命令が追加されました。
warning メッセージの抑止などが出来ます。

```csharp
using System;
class Program
{
  [Obsolete]
  static void Foo() {}
  static void Main() {
// 612番の警告(Obsolete メソッドを使用)を出さないようにする。
#pragma warning disable 612
  Foo();
// 612番の警告を出すように戻す。
#pragma warning restore 612
  }
}
```



## <a id="sec-generated-title-12"></a> <a id="conditional"></a>Conditional 属性

属性クラスに対して Conditional 属性を付けることで、
一定条件化でのみ適用される属性を作ることが可能になりました。

```csharp
#define DEBUG
using System;
using System.Diagnostics;

// ↓属性クラスに対して Conditional 属性を付ける。
[Conditional("DEBUG")]
public class TestAttribute : Attribute {}

// ↓DEBUG シンボルが定義されているときのみ Test 属性が付く。
[Test]
class C {}
```



## <a id="sec-generated-title-13"></a> <a id="fixed"></a>固定長配列

unsafe コード内限定で、
<code>fixed int fixedArray[128];</code> というように固定長配列が使用可能になりました。


## <a id="sec-generated-title-14"></a> <a id="co-contra"></a>デリゲートの Covariance/Contravariance

Covariance … 戻り値の型が、デリゲートの戻り値の型の派生クラスになっていても OK。

```csharp
class Mammal {}       // 哺乳類。
class Dog : Mammal {} // 犬。

class Program
{
  // Mammal（哺乳類）型を返すデリゲートを定義。
  public delegate Mammal MyHandlerMethod();

  public static Mammal MammalHandler(){ return null; }
  public static Dog DogHandler(){ return null; }

  static void Main(string[] args)
  {

    MyHandlerMethod handler_1 = new MyHandlerMethod(MammalHandler);

    // Covariance によって、以下のメソッドもデリゲート化可能。
    // Dog 型の変数を Mammal 型に渡すのは OK なので、戻り値は暗黙的にキャストされる。
    MyHandlerMethod handler_2 = new MyHandlerMethod(DogHandler);

  }
}
```


Contravariance … 引数の型が、デリゲートの引数の型の基底クラスになっていても OK。

```csharp
class Mammal {}       // 哺乳類。
class Dog : Mammal {} // 犬。

class Program
{
  // Dog（犬）型を受け取るデリゲートを定義。
  public delegate void MyHandlerMethod(Dog dog);

  public static void MammalHandler(Mammal elephant){}
  public static void DogHandler(Dog sheepdog){}

  static void Main(string[] args)
  {

    // Contravariance によって、以下のメソッドもデリゲート化可能。
    // Dog 型の変数を Mammal 型に渡すのは OK なので、引数は暗黙的にキャストされる。
    MyHandlerMethod handler_1 = new MyHandlerMethod(MammalHandler);

    MyHandlerMethod handler_2 = new MyHandlerMethod(DogHandler);

  }
}
```


数学用語的には、Covariance … 共変性、Contravariance … 反変性。
プログラミングの分野でもこの訳語で定着したようです。
