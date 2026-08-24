---
title: "C# の特徴（C++、Java 利用者向け）"
source_url: "https://ufcpp.net/study/csharp/cs4j/ab_csspec/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2019-08-03T19:33:02"
tags: []
umbraco_id: 1374
parent_id: 1372
sort_order: 1
aliases:
  - "/study/csharp/ab_csspec.html"
---

# C# の特徴（C++、Java 利用者向け）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# 1.0 は、第一印象としては「Java のぱくり？」と言った感じに見えるのですが、
実際には「Java と C++ と VB のいいところを集めてきたような言語」と言った感じです。
（また、C# 2.0、3.0、… と進歩するにつれ、既存言語の焼き直しにとどまらない斬新な機能が追加されています。）
ここではその C# の特徴的な機能をいくつか紹介していきます。

注意: C# 1.0 当時の「特徴」です。
2.0 以降の追加機能については、
「[C# 2.0 の新機能](../cheatsheet/ap_ver2.md)」、
「[C# 3.0 の新機能](../cheatsheet/ap_ver3.md)」、
「[C# 4.0 の新機能](../cheatsheet/ap_ver4.md)」、
「[C# 5.0 の新機能](../cheatsheet/ap_ver5.md)」、などを参照してください。


## <a id="sec-generated-title-2"></a> <a id="gc"></a>ガーベジコレクション

<strong id="gc" class="keyword">ガーベジコレクション</strong>(Garbage Collection: ごみ集め) とは不要になったメモリを自動的に破棄するための機構です。

オブジェクト指向プログラミングの特徴である、
ポリモーフィズム(polymorphism: 多態性)やダイナミックバインディング(dynamic binding: 動的結合)を
使おうと思うと、参照(≒ポインター)の管理は避けては通れません。
そのため C++ ではこの煩雑なメモリ管理という作業が必要不可欠でした。

このような背景から、熟練のC++プログラマは
参照カウントなどといった技術を使ってメモリ管理を自動化する機構を自作していました。
さらに、クラスの利用者からはポインターが直接見えないように、
クラスの上にメモリ管理用クラス(ハンドルクラスなどといいます)をかぶせて使うこともしばしばあります。

これに対し、Java や C# ではメモリの管理を自動化する機構が言語に組み込まれています。
煩雑な作業をなくし、本来の目的を達成するためのプログラミングに集中できるわけです。

ガベージ コレクションについては、「[メモリ管理](../../computer/essential-software/memorymanagement.md)」でもう少し詳しい説明もしています。
興味があれば参照してください。

注:
実際には、ガーベジコレクション機能を備えているのは C# 自体ではなく、
.Net Framework です。
.Net Framework 上で動作する言語(現時点でマイクロソフトが提供する言語は
C#、Manage C++、VB.Net、JavaScript の4つ。他にもサードパーティから
Perl、Python、SmallTalk、COBOL などが提供されます。)はすべて
ガーベジコレクション機能を利用できます。


## <a id="sec-generated-title-3"></a> <a id="interface"></a>インターフェースのサポート

オブジェクト指向の特徴の一つに継承というものがあります。
継承はコードの再利用性を高めるなどの利点があるのですが、
一つのクラスが複数のクラスを継承するようになると
いろいろと面倒なことが起こってきます。
「多重継承はオブジェクト指向における goto」とまで言われ、忌み嫌われる事すらあります。


そのため、Java や C# では多重継承は禁止されています。
つまり、すべてのクラスは単一の親からしか派生できません。
その代わりに一つのクラスが複数の振る舞いを継承したい場合に使うのが
<strong id="interface" class="keyword">インターフェース</strong>(interface)です。
インターフェースは、

* public な抽象メソッド(C++ で言うところの純粋仮想関数)しか定義できない

* メンバー変数を持つことができない

* インスタンスを作成できない


という特徴のあるクラスだと思ってください。

（余談ですが、
C++ でも、まずは上にあげたような制限を設けた抽象基底クラス(プロトコルクラス)を
定義して、その派生クラスで実装を行ったほうが何かと都合がいいです。
ただ、C++ ではこのような制限を強制することは出来ず、
あくまでプログラマが自分で約束事を決め、それを遵守するように気を付ける必要があります。）

派生クラスは、単一のクラスしか継承できませんが、インターフェースなら複数継承することができます。
このような制限を課すことで、多重継承の問題点を回避しながら複数の振る舞いを継承することができるのです。


## <a id="sec-generated-title-4"></a> <a id="ref"></a>値型と参照型

C# の型には大きく分けて二通りのものがあります。
ひとつは「値型」と呼ばれ、代入がコピーによって行われるものです。
もうひとつは「参照型」と呼ばれ、こちらは代入が参照渡しによって行われます。

<code>int</code> などの組込み型は値型で、
クラスは参照型であることは Java と同じなのですが、
C# ではさらに <code>class</code> で定義した型は参照型になり、
<code>struct</code> で定義した型は値型になるという特徴があります。

オブジェクト指向言語で多態性と動的結合を使おうとする場合、
オブジェクトは参照渡しにする必要があります。
（「[多態性](../oop/oo_polymorphism.md)」参照）
そのため、Javaではクラスはすべて参照型になっています。
しかし、参照型はガーベジコレクションを行わなくてはならず、
多態性を使用しない型まで参照型にしてしまうと実行効率が悪くなるので、
C# では <code>class</code> と <code>struct</code> という2つのキーワードを用意し、値型と参照型の使い分けを可能にしています。


## <a id="sec-generated-title-5"></a> <a id="auto"></a>変数の自動的な初期化

昔からある些細なミスの1つに、変数の初期化のし忘れがあります。
変数を宣言したのはいいけれど、一度も初期化することなく
(この場合、値は不定になっていることが多い)変数を使ってしまうと、
「ときどきおかしな動作をする」といった開発者泣かせのバグになってしまうこともあります。

（
毎回必ず同じ箇所で起きる不具合ならまだ原因の特定が楽なのですが、
ときどきしか不具合は起きず、不具合の起きる箇所も不定だと原因の特定が
困難になります。
）

そこで、C# では、クラスのメンバー変数は自動的に初期化されます。
コンストラクタで初期化されなかったメンバー変数は、数値型なら 0 に、
参照型なら null に初期化されます。
また、ローカル変数は自動的には初期化されないのですが、
初期化しないまま変数を使おうとするとコンパイラが警告メッセージを表示します。

（
プログラミング初心者がよくコンパイラ時に山ほどエラーを指摘されてびびるという
話がありますが、コンパイル時に検出できるエラーなんて一番かわいいエラーです。
変数の初期化し忘れのような、原因不明のエラーになりかねないものは、
いっそのことコンパイル時にエラーになってくれた方がありがたいわけです。
）


## <a id="sec-generated-title-6"></a> <a id="if"></a>if 文の条件式は bool 値を返さなければならない

もう1つ、昔からある些細なミスに、if 文の条件式で代入を行ってしまうというものもあります。
例えば、

```csharp {highlight-text="=="}
if(a == 0)
```


と書くつもりで、

```csharp {highlight-text="="}
if(a = 0)
```


と書いてしまうというものです。

（
C言語などでは、慣習的に「if 文の条件式の中では定数を先に書け」をいう格言があるくらいだったりします。
つまり、
<code>if(a = 0)</code>
はコンパイルが通ってしまいますが、
<code>if(0 = a)</code>
はコンパイルエラーになるので、 <code>==</code> と <code>=</code> を間違えてもすぐに分かるので、
後者を使えということです。
）

そこで、C# では、if 文の条件式の中身は bool 値以外であってはいけない、
つまり、整数型を暗黙的に bool 値に変換することはできなくなっています。
そのため、

```csharp
if(a = 0)
```


はコンパイル時にエラーとして検出されます。


## <a id="sec-generated-title-7"></a> <a id="overload"></a>演算子のオーバーロードのサポート

Java は C++ をより良くした言語といわれていながら、一方では <code>C++--</code> などと陰口を言われることがあります。
その原因の1つに演算子のオーバーロードをサポートしていないことが挙げられます。

例えば、Java で複素数計算をしたければ、

```csharp
Complex a(1,2), b(2,1), c(0,1);
a.Add(b, c);
a.Mul(a, c);
a.Div(a, 5);
```


とか書く必要があるわけです。
当然、C++ プログラマなら

```cpp
std::complex a(2,1), b(1,2), c(0,1);
a = b + c;
a *= c;
a /= 5;
```


と書けるわけで、直感的ですっきりとした書き方ができます。

C# では演算子のオーバーロードをサポートしています。

また、C# では <code>+, -, *, /</code> などの演算子をオーバーロードすると
自動的に <code>+=, -=, *=, /=</code> などの代入演算子もオーバーロードされます。
（
C++ では <code>+</code> と <code>+=</code> は別個にオーバーロードしていたので、
<code>a = a + b;</code>
と
<code>a += b;</code>
とで動作が食い違う可能性もありました。
C# ではそれはありえなくなったわけです。
）


## <a id="sec-generated-title-8"></a> <a id="multid"></a>多次元配列

Java では、多次元配列は「配列の配列」を用いて表現します。
そして、

```csharp
int[][] array = new int[5][];
for(int i=0; i<5; i++)
{
  array[i] = new int[5];
}
```


と言った感じで使用します。
行ごとに配列の長さが異なる場合もあります。

C# では二通りの多次元配列の定義の仕方があります。
1つは<em>四角い配列</em>(rectangular array)と呼ばれ、
もう1つは<em>ぎざぎざの配列</em>(jagged array)と呼ばれるものです。
ぎざぎざの配列とは、いわゆる「配列の配列」のことで、Java の多次元配列と使い方は同じです。
それに対し、四角い配列とは数学で使う行列の様に行の長さが一定の多次元配列のことです。
(C# でも、rectangular array を単に「多次元配列」と呼び、jagged array は「配列の配列」と呼ぶこともあります。)
こちらは、

```csharp
int[,] array = new int[5,4];
```


と言った感じで使用します。
Java では配列の配列しか使用することはできませんが、
C# ではこの二つを使い分けることができます。

また、Java や C# の配列はオブジェクトであり、配列の長さなどを自分自身で管理しています。
Java では C++ の配列のようにただたんにメモリ上にデータが連続して並んでいるだけの配列は
使用することはできないのですが、
C# では unsafe コード中に限り、単なる連続したデータの並びである配列も使用することができます。

（
ちなみに、C++ 的に表現すると、
jagged array は、

```csharp
class JaggedArray
{
  int** p_;
  int w_;
  int h_;
public:
  JaggedArray(int width, int height) : w_(width), h_(height)
  {
    p_ = new int*[width];
    for(int i=0; i<width; ++i)
      p_[i] = new int[height];
  }

  int& operator() (int x, int y)
  {
    return p_[x][y];
  }
  // 以下省略
}
```


rectangular array は、

```csharp
class RectangularArray
{
  int* p_;
  int w_;
  int h_;
public:
  RectangularArray(int width, int height) : w_(width), h_(height)
  {
    p_ = new int*[width*height];
  }

  int& operator() (int x, int y)
  {
    return p_[x + y * height];
  }
  // 以下省略
}
```


になります。
operator() の中身を見れば分かると思いますが、
jagged の方が1回間接参照が多くて、
rectangular の方は乗算と加算が1回ずつ多いです。

「乗算＋加算」と「間接参照」を比べると、
普通は間接参照の方が実行に時間がかかります。
なので、ランダムアクセスする場合、
jagged array の方が重たくなりそうなんですが、
.NET Framework の IL は1次元配列の要素参照のための専用命令を持っているようで、
この関係で jagged array の方が動作が高速らしいです。
（多次元配列にはその専用命令を使えないけど、
jagged array はいわば、1次元配列の中に1次元配列が入ってるだけなので。）

ちなみに、使用メモリ量的には、
jagged array の方が <code>int*</code> （32bit CPU ならたいてい4バイト）のサイズ× width 分だけ余計に使用します。


## <a id="sec-generated-title-9"></a> <a id="property"></a>プロパティ

クラスの実装が外部からは見えないようにしておくことは大切なことです
(その理由は「[実装の隠蔽](../oop/oo_conceal.md)」をご覧ください)。
そのため、アクセッサ(accessor)と呼ばれるもの(<code>x</code> という変数に対して <code>setX()</code> と <code>getX()</code>という
メソッドを用意して、<code>x</code> を参照する場合は必ずこのメソッドを通して行うというもの)を
定義することになります。

しかし、いちいち`set***()`と`get***()`を用意するというのは
決して美しくはありません。
（
ひどい場合には
「メンバー変数の数だけ`set***()`と`get***()`の組が存在する」
ということもあり得ます。
）
そこで、C# では<strong id="property" class="keyword">プロパティ</strong>（property）と呼ばれるものを使ってこの問題を解決しています。
プロパティとは上で述べたようなアクセッサを簡潔に定義するための機能です。
C++で、

```csharp {highlight-lines="5-7"}
class A
{
  double x; //実装は外部から隠す
public:
  //アクセッサ
  void setX(double xx){x = xx;}
  double geX(){return x;}
};
```


と言った感じでアクセッサ関数を用意してデータの参照を行っていましたが、
C#では、

```csharp {highlight-lines="5-10"}
class A
{
  double x; //やはり実装は外部から隠す

  //これがプロパティ
  public double X
  {
    set{x = value;}
    get{return x;}
  }
}
```


というように、プロパティを用いてデータの参照を行えます。


## <a id="sec-generated-title-10"></a> <a id="delegate"></a>デリゲート

<strong id="delegate" class="keyword">デリゲート</strong>(delegate: 代表、委譲、委任)とはC言語の関数ポインターをオブジェクト指向に適するように拡張したものだと思ってください。

Java では、イベント処理(ボタンが押されたときや、メニューが選択されたときの処理)は
イベントリスナーと呼ばれるインターフェースを用いて行います。
しかし、この方法では static なメソッドをイベントリスナーにすることはできませんし、
ボタンなどのコントロールの数だけリスナークラスを書く必要があり、面倒です。

一方、C# では、イベント処理はデリゲートを用いて行います。

```csharp {highlight-lines="18-20"}
using System.Windows.Forms;
using System.Drawing;
using System;

class EventSample : Form
{
  Button button = new Button();

  EventSample()
  {
    this.Text = "Event Sample";
    this.ClientSize = new Size(300, 200);

    button.Location = new Point(60, 60);
    button.Size = new Size(80, 25);
    button.Text = "OK";

    //ボタンが押されたときの処理を追加
    //このEventHandlerはデリゲート
    button.Click += new EventHandler(OnOKButtonClick);

    Controls.Add(button);
  }

  void OnOKButtonClick(object obj, EventArgs args)
  {
    MessageBox.Show("Event Sample", "Sample");
  }

  static void Main()
  {
    Application.Run(new EventSample());
  }
}
```



## <a id="sec-generated-title-11"></a> <a id="attribute"></a>属性

C++ や Java ではクラスやメソッドに対して public や private などといった<strong id="attribute" class="keyword">属性</strong>(attribute)を付加することができます。
Java の例をあげると、

```csharp {highlight-ranges="sha256:dcac7d2796b67f1aac0bcc595af4097391942644a5df4fb0b462d752fb217ed9;1:1-1:6,3:4-3:10"}
final class MyClass
{
   public int myMethod();
}
```


強調表示してある部分が属性です。
この例では、<code>MyClass</code> はサブクラス化のできない(<code>MyClass</code> の派生クラスは作れない)という属性(<code>final</code>)を持ち、
<code>MyMethod</code> はクラスの外部から参照できるという属性(<code>public</code>)を持ちます。

C# ではこのようなクラスやメソッドに対する属性を自分で定義することができます。
例えば、作成中のクラスがまだテスト段階であるということを明示的に示したいときに、
<code>IsTest</code> という属性を作成して、そのクラスに付けておくことができます。

```csharp {highlight-text="[IsTest]"}
// IsTest 属性を作成
public class IsTestedAttribute : Attribute
{
   public override string ToString()
   {
       return "Is Tested";
   }
}

// 新しく作ったクラスに IsTest 属性を持たせる
[IsTest] class MyClass
{
   // クラスの実装
}
```


新しい属性は <code>Attribute</code> クラスの派生クラスとして作成し、
作成した属性は[属性名]というように
名前を[]でくくって用います。

このようにしてクラスやメソッドに付加した属性は、
コンパイラや統合開発環境への指示などにも利用できます。
例えば、C# コンパイラは、
<code>[Conditional]</code> 属性の付加されたメソッドを
<code>#define</code> 命令により特定のシンボルが定義されているときだけ呼び出します。
