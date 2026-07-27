---
title: "関数"
source_url: "https://ufcpp.net/study/csharp/structured/st_function/"
content_type: "Article"
published_at: "2001-11-17T00:00:00"
updated_at: "2016-10-25T00:00:00"
tags:
  - "Ver. 4.0"
  - "Ver. 6.0"
umbraco_id: 1233
parent_id: 1217
sort_order: 5
aliases:
  - "/study/csharp/st_function.html"
---

# 関数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

同じプログラムコードを複数の場所で何度も利用したい場合があります。
例えば、今まで説明してきた中で、たびたび「入力を促すメッセージを出力して、整数を入力してもらう」という場面が出てきました。
そのために何度も同じようなソースコードを書いてきました。

「[反復処理](st_loop.md)」のところでも説明しましたが、
同じコードを複数の箇所に書くのはプログラムを管理していく上で好ましくありません。
そこで、こういう頻繁に使われる機能をまとめて、何度も呼び出せるようにしたのが<strong id="function" class="keyword">関数</strong>（function）です。

(追記したい)
何度も出てくる処理でなくても、
処理に名前が付く単位で関数化すべき。
明確な名前を付ける(名前が付く単位で区切る)のがよいコードを書くコツ。


##### <a id="sec-generated-title-2"></a>ポイント

* 何度も出てくる処理は関数化する。

* 数学の「関数」から取った名前。プログラミング用語的には、他に、サブルーチン、プロシージャ（手続き）、メソッド等といった呼び方がある。

* C# の機能の呼び名としては、このページで説明しているものは正確には「メソッド」という。
    * とりあえず現時点では、メソッド ＝ 関数 と思っておいて OK。

    * C# の場合、関数的な挙動をするものがいくつかあって、そのうち、一番「関数らしい関数」がメソッド。





##### <a id="sec-generated-title-3"></a>サンプル

[https://github.com/ufcpp/UfcppSample/tree/master/Chapters/StructuredProgramming/Function](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/StructuredProgramming/Function)


## <a id="sec-generated-title-4"></a> <a id="sec-function-member"></a>補足: 関数メンバー

C# の場合、このページで説明するような「関数」的な動作、
すなわち、何らかの値を受け取って、処理して、結果の値を返すような挙動をするものがいくつかあります。
具体的には、以下のようなものがあります。

* メソッド
    * 拡張メソッド(参考:「[拡張メソッド](../functional/sp3_extension.md)」)



* コンストラクター(参考:「[コンストラクター](../oop/oo_construct.md)」)

* プロパティ(参考:「[プロパティ](../oop/oo_property.md)」)

* インデクサー(参考:「[インデクサー](../oop/oo_indexer.md)」)

* イベント(参考:「[イベント](../functional/sp_event.md)」)

* 演算子(参考:「[演算子のオーバーロード](../oop/oo_operator.md)」)

* ユーザー定義の型変換(参考:「[演算子のオーバーロード](../oop/oo_operator.md)」)

* ファイナライザー(参考: 「[ファイナライザー](../resource/rm_destructor.md)」)


これらを合わせて、<strong id="function-member" class="keyword">関数メンバー</strong>(function member)と呼びます。
このページで説明しているものは、C# の機能名としては、正確には<strong id="method" class="keyword">メソッド</strong>といいます。

また、数学の関数を引き合いに出して説明していますが、
数学の関数のイメージと一番合うのは、「静的メソッド」というものです
(メソッドにもインスタンス メソッドと静的メソッドの2種類があります)。
これについては、「[静的メンバー](../oop/oo_static.md)」で説明します。

このページでの説明(つまり、メソッドに対する説明)の多くは、メソッド以外の関数メンバーにも当てはまります。
引数の書き方、関数本体の書き方、戻り値の返し方などは、すべての関数メンバーで共通です。
(引数や戻り値が指定できない関数メンバーもありますが、指定できる場合には、書き方が同じです。)


## <a id="sec-generated-title-5"></a> <a id="definition"></a>関数定義

C# では、以下のようにして関数(C# 用語としては、正確にはメソッド)を定義します。

```csharp
戻り値の型 関数名(引数一覧)
{
    関数本体(具体的な処理)
}
```


<h5 class="version version6">Ver. 6</h5>
また、C# 6 では、関数本体の部分が1つの式だけからなる場合、以下のような書き方をすることができるようになりました。
これを、expression-bodied (本体が式の)関数と呼びます(詳細は後述)。

```csharp
戻り値の型 関数名(引数一覧) => 関数本体の式
```


関数という名前は、数学用語の関数からきています。
数学の関数は、ある値を入力すると、一定のルールに従った出力が得られます。
また、入力することの出来る値の範囲や、出力として得られる値の範囲はしっかりと決められています。

C#の関数も同じように、入力出来る値と、出力される値の型をあらかじめ決めておかなければ行けません。
例えば、実数(浮動小数点数)を入力して、その値のsinを求めるような関数を作りたい場合、
以下のようにして関数を作ることが出来ます。

```csharp
// sin x を求める関数。
// テイラー展開を利用。
// かなり適当に作ってるので、この方法ではそんなに精度はよくない。
double Sin(double x)
{
  double xx = -x * x;
  double fact = 1;
  double sin = x;
  for(int i=1; i<100;)
  {
    fact *= i; ++i; fact *= i; ++i;
    x *= xx;
    sin += x / fact;
  }
  return sin;
}
```


まず、コメントの部分を除いて1番最初の、「<code>
                <span class="reserved">double</span> Sin(<span class="reserved">double</span> x)
            </code>」の部分を細かく見ていくと、
先頭の「<code>
                <span class="reserved">double</span>
            </code>」が関数の出力(これを<strong id="return" class="keyword">戻り値</strong>という)の型、
次の「<code>Sin</code>」が関数の名前、
<code>()</code>の中にある「<code>
                <span class="reserved">double</span> x
            </code>」が入力の型と入力された値を保持するための変数(これを<strong id="paramter" class="keyword">引数</strong>(paramter)といいます)です。

その後に続く<code>{}</code>の中身が関数の内部で行う処理です。
そして、出力にしたい値(これを<strong id="return-value" class="keyword">戻り値</strong>(return value)といいます)は<strong id="return" class="keyword">return</strong>というキーワードの後ろに書きます。

作成した関数を呼び出すには、

```csharp
変数 = 関数名(入力)
```


というように書きます。
以下に関数を呼び出す例を挙げます。


##### <a id="sec-generated-title-6"></a>サンプル

```csharp
using System;

class SinSample
{
  static void Main()
  {
    for(int i=0; i<10; ++i)
    {
      double x = 0.01 * i;

      double y = Sin(x); // 関数呼び出し

      Console.Write("sin({0:f2}) = {1:f6}\n", x, y);
    }
  }

  /// <summary>
  /// sin(x) の値を求める。
  /// 実装は割りと適当。
  /// </summary>
  static double Sin(double x) // 関数定義
  {
    double xx = -x * x;
    double fact = 1;
    double sin = x;
    for(int i=1; i<100;)
    {
      fact *= i; ++i; fact *= i; ++i;
      x *= xx;
      sin += x / fact;
    }
    return sin;
  }
}
```


```console
sin(0.00) = 0.000000
sin(0.01) = 0.010000
sin(0.02) = 0.019999
sin(0.03) = 0.029996
sin(0.04) = 0.039989
sin(0.05) = 0.049979
sin(0.06) = 0.059964
sin(0.07) = 0.069943
sin(0.08) = 0.079915
sin(0.09) = 0.089879
```


<code>Sin</code> 関数の定義の部分の前についている <code>
                <span class="reserved">static</span>
            </code> というキーワードについては、
「[静的メンバー](../oop/oo_static.md)」で説明します。

もう一つ違う例を挙げて見ましょう。
今まで、実数の入力は以下のようにして行っていました。

```csharp
Console.Write("ユーザーに入力を促すメッセージ");
x = double.Parse(Console.ReadLine());
```


実数を入力する必要のある場面ごとにこのようなコードを書くのは面倒ですし、
これを関数化して見ましょう。
まず、単純に関数化した結果を以下に示します。

```csharp
double GetDouble(string message)
{
  Console.Write(message);
  double x = double.Parse(Console.ReadLine());
  return x;
}
```


今までずっと無視してきていたのですが、実はこのままでは、実数に出来ない文字列を入力してしまうとエラーが発生して、以下のようなエラーメッセージを表示してプログラムが途中で止まってしまいます。

```console
未処理の例外 : System.FormatException: 入力文字列の形式が正しくありません。
  at System.Number.ParseDouble(String s, NumberStyles style, NumberFormatInfo info)
  at System.Double.Parse(String s, NumberStyles style, IFormatProvider provider)
  at StatementSample2.Main()
```


本当は例外処理(後述)というものを行ってこのようなエラーが出たときの対処を行わないといけません。
そこで、この例外処理を行うように変更を加えてみましょう。
(例外処理については「[例外処理](oo_exception.md)」参照。
<code>try</code>と<code>catch</code>は例外処理を行うための構文です。)

```csharp
double GetDouble(string message)
{
  double x;
  while(true)
  {
    try
    {
      // 入力を促すメッセージを表示して、値を入力してもらう
      Console.Write(message);
      x = double.Parse(Console.ReadLine());
    }
    catch(Exception)
    {
      // 不正な入力が行われた場合の処理
      Console.Write(
        "error : 正しい値が入力されませんでした\n入力しなおしてください\n");
      continue;
    }
    break;
  }
  return x;
}
```


この修正した関数を用いて「[変数と式](../start/st_variable.md)」の最後で示したサンプルを書き換えてみましょう。


##### <a id="sec-generated-title-7"></a>サンプル

```csharp
using System;

class StatementSample2
{
  static void Main()
  {
    double x, y, z; // 変数を宣言。

    // x, y にユーザーの入力した値を代入。
    x = GetDouble("input x : ");
    y = GetDouble("input y : ");

    // 入力された値を元に計算
    z = x * x + y * y; // z に x と y の二乗和を代入
    x /=  z;           // x =  x / z; と同じ。
    y /= -z;           // y = -y / z; と同じ。

    // 計算結果を出力
    Console.Write("({0}, {1})", x, y);
  }

  /// <summary>
  /// 入力を促すメッセージを表示して、実数を入力してもらう。
  /// 正しく実数として解釈できる文字が入力されるまで繰り返す。
  /// <param name="message"> 入力を促すメッセージ </param>
  /// <return> 入力された値 </return>
  /// </summary>
  static double GetDouble(string message)
  {
    double x;
    while(true)
    {
      try
      {
        // 入力を促すメッセージを表示して、値を入力してもらう
        Console.Write(message);
        x = double.Parse(Console.ReadLine());
      }
      catch(Exception)
      {
        // 不正な入力が行われた場合の処理
        Console.Write(
          "error : 正しい値が入力されませんでした\n入力しなおしてください\n");
        continue;
      }
      break;
    }
    return x;
  }
}
```

### <a id="sec-generated-title-8"></a> <a id="return-statement"></a>returnの場所・数

これまでの例ではすべて、関数の最後に1つだけ`return`を書いていますが、C# には別にそういう縛りはありません。
`return` は関数の途中にも書けますし、1つの関数内に複数書けます。

複数個の`return`を書きたくなる1番の例は[条件分岐](st_branch.md)でしょう。
以下のように、条件を満たすときと満たさない時で別の値を返したい場合などです。

```csharp
static int Max(int x, int y)
{
    if (x > y) return x;
    else return y;
}
```

分岐なしで関数の途中に`return`を書くこともできますが、この場合は、`return`よりも後ろは実行されません。

```csharp
static int F(int x)
{
    Console.WriteLine("ここは実行される");
    return x;

    Console.WriteLine("ここは実行されない");
}
```

## <a id="sec-generated-title-9"></a> <a id="arity"></a>引数・戻り値の数

引数や戻り値は、なくてもよかったり、複数書けたりします。

### <a id="sec-generated-title-10"></a> <a id="multiple-params"></a>引数が複数ある関数、

数学の関数では、例えば「f(x, y、z) = x<sup>2</sup>+y<sup>2</sup>+z<sup>2</sup>」といったように、入力が複数ある場合があります。
C#の関数でもこのように引数が複数ある関数を作れます。
引数を複数使いたい場合、数学の関数と同じように、関数を定義する際に、以下のように複数の引数を <code>,</code> で区切って並べます。
このように、引数を <code>,</code> で区切って並べたものを<em>引数リスト</em>といいます。

```csharp
double Norm(double x, double y, double z)
{
  // ノルムの計算
  return x*x + y*y + z*z;
}
```

### <a id="sec-generated-title-11"></a> <a id="no-param"></a>引数のない関数

数学ではあまり考えられませんが、C#では引数のない関数も定義できます。
引数のない関数は、以下のように、引数リストを空にして定義します。

```csharp
ulong seed = 4275646295673547UL;
ulong Random()
{
  // 線形合同法による疑似乱数の生成
  unchecked{seed = seed * 1566083941UL + 1;}
  return seed;
}
```

### <a id="sec-generated-title-12"></a> <a id="void"></a>戻り値のない関数

同様に戻り値のない関数も定義できます。
戻り値のない関数は、以下のように、戻り値の型を <em>
                <code>void</code>
            </em> (「空の、何もない」という意味)というものにしておきます。

```csharp
void WriteArray(int[] array)
{
  Console.Write("{");
  for(int i=0; i<array.Length-1; ++i)
  {
    Console.Write("{0}, ", array[i]);
  }
  Console.Write(array[array.Length-1] + "}\n");
}
```

戻り値のない物でも「関数」と呼ぶのはC言語やC++言語から受け継いだ習慣です。
その他の言語では、
戻り値のないものは「<em>サブルーチン</em>」とか「<em>プロシージャ</em>」といって関数と区別する場合もあります。

ちなみに、戻り値がない(`void`)の場合、`return`は書けますが、`return`の後ろには何も値を書かず、関数を途中で抜ける意味だけ持ちます。

```csharp
static void F(int x)
{
    if (x <= 0) return;

    Console.WriteLine("x が正の時だけ実行される");
}
```

### <a id="sec-generated-title-13"></a> <a id="unit"></a>補足1: void だけ特別扱いは不便

戻り値が`void`の関数は`return`の後ろに値を書けません。
これは`void`の場合だけの特別な書き方になります。
そして、特別に書き方を変えないといけないというのが面倒になることがあります。

例えば、C# では関数を変数に格納して使うことができるんですが、
戻り値がある場合は`Func`、ない場合は`Action`と、別の型に代入して使うことになります。

```csharp
using System;

class Program
{
    static void Main()
    {
        Action a1 = A1; // Func<void> とは書けない
        Action<int> a2 = A2;
        Func<int> f1 = F1; // Action と Func が別
        Func<int, int> f2 = F2;
    }

    static void A1() { } // 戻り値がないと、=> 記法も使えない
    static void A2(int x) { }
    static int F1() => 0;
    static int F2(int x) => x;
}
```

そこで、以下のように、空っぽの値を用意して、`void`の代わりに使うことですべて「戻り値あり」で統一する手法を時々使ったりします。

```csharp
using System;

// 空っぽの型を1個用意
struct Unit { }

class Program
{
    static void Main()
    {
        // void の代わりに Unit を使うことで、全部 Func に統一
        Func<Unit> a1 = A1;
        Func<int, Unit> a2 = A2;
        Func<int> f1 = F1;
        Func<int, int> f2 = F2;
    }

    static Unit A1() => default(Unit); // 空っぽの値を返しておく
    static Unit A2(int x) => default(Unit);
    static int F1() => 0;
    static int F2(int x) => x;
}
```

不格好なので積極的に使うものでもありませんが、統一のためにやむを得ないこともあったりします。

#### <a id="sec-generated-title-14"></a> <a id="why-unit"></a>Unitという名前

ちなみに、先ほどの例では、空っぽの型の名前をunit (単位元)にしていますが、
一応意味があってこの名前を使っています。
プログラミング用語しても使われるので、C#に限らず、たまに目にする言葉かもしれません。

この名前は数学用語に由来します。
数学では、以下のような表現をすることがあります。

- 0 = { }  … 0とは空っぽ(0要素)の集合である
- 1 = { 0 } … 1とは0のみを持つ(1要素の)集合である

unitというのはこの意味での「1」を指します。
先ほどの例では`defautl(Unit)`という意味のない値を返していますが、`Unit`型は、この意味のない値を1つだけ持つ型ということになります。

### <a id="sec-generated-title-15"></a> <a id="tuple"></a>補足2: 複数の戻り値(タプル)

C#では、基本的には戻り値は1つだけ返せます。

複数の値(多値)を返したいこともありますが、その場合、C# 6以前では[複合型](st_struct.md#about)を1つ作って返していました。

```csharp
struct SumCount
{
    public int sum;
    public int count;
}

static SumCount Tally(int[] items)
{
    var sum = 0;
    var count = 0;
    foreach (var x in items)
    {
        sum += x;
        count++;
    }
    return new SumCount { sum = sum, count = count };
}
```

<h5 class="version version7">Ver. 7</h5>

この型に本当にちゃんとした意味があればいいんですが、この例の場合は見るからに大した意味がありません。
「和(sum)と個数(count)を表す`SumCount`型」なんていわれなくても、`sum`と`count`を見ればわかります。

そこで、C# 7では、以下のように書けるようになりました。

```csharp
static (int sum, int count) Tally(int[] items)
{
    var sum = 0;
    var count = 0;
    foreach (var x in items)
    {
        sum += x;
        count++;
    }
    return (sum, count);
}
```

複数の戻り値を返しているような書き心地になります。

これは実際には、`(int sum, int count)`という「名前のない型」(これをタプルと呼びます)を1つ作って、その値を返しています。
詳細は[名前のない複合型](st_anonymoustype.md)で説明します。

##### <a id="sec-generated-title-16"></a>サンプル

```csharp
using System;

class FunctionSample
{
  static void Main()
  {
    int[] array = new int[3];

    // 乱数を使って値を生成
    for(int i=0; i<array.Length; ++i)
    {
      array[i] = (int)(Random() >> 58); // [0,63] の整数乱数生成
    }

    // ノルムを計算
    double norm = Norm(array[0], array[1], array[2]);

    // 値の出力
    WriteArray(array);
    Console.Write("norm = {0}\n", norm);
  }

  static ulong seed = 4275646293455673547UL;
  /// <summary>
  /// 線形合同法による乱数の生成
  /// </summary>
  static ulong Random()
  {
    unchecked{seed = seed * 1566083941UL + 1;}
    return seed;
  }

  /// <summary>
  /// 入力した3つの値のノルムを計算
  /// <summary>
  static double Norm(double x, double y, double z)
  {
    return x*x + y*y + z*z;
  }

  /// <summary>
  /// 配列を , で各要素を区切って、{}で括った形式で出力
  /// <summary>
  static void WriteArray(int[] array)
  {
    Console.Write("{");
    for(int i=0; i<array.Length-1; ++i)
    {
      Console.Write("{0}, ", array[i]);
    }
    Console.Write(array[array.Length-1] + "}\n");
  }
}
```


```console
{40, 31, 39}
norm = 4082
```



### <a id="sec-generated-title-17"></a> <a id="state"></a>補足: 状態

この例の `Random` は、数学の関数と違って、「状態」(state)を持っています。
一般に、数学の関数は、引数と戻り値の関係だけを説明していて、同じ引数を与えた場合、常に同じ戻り値が返ります。
一方、この例は、関数の外にある `seed` という変数に値を記録・書き換え(これを「状態を持つ」という)していて、呼ぶたびに状態が変わり、違う戻り値を返します。

ちなみに、この関数の外にある変数のことは、C# 的にはフィールド(field)と呼びます。詳しくは「[データの構造化](st_struct.md)」で説明します。


## <a id="sec-generated-title-18"></a> <a id="parameter"></a>補足: 引数

引数についてはいくつか補足があります。


### <a id="sec-generated-title-19"></a> <a id="default-parameter"></a>オプション引数と名前付き引数

<h5 class="version version4">Ver. 4.0</h5>

C# 4 から、引数に規定値(default value)を与えて、呼び出し時に省略できたり(optional)、名前付き(named)で引数を書けるようになりました。

```csharp
class OptionalParameterSample
{
    public static void Sample()
    {
        // 引数の省略(optional parameter)
        var s1 = Sum();     // Sum(0, 0, 0); と同じ意味。
        var s2 = Sum(1);    // Sum(1, 0, 0); と同じ意味。
        var s3 = Sum(1, 2); // Sum(1, 2, 0); と同じ意味。

        // 名前付きで引数を与える(named parameter)
        var s4 = Sum(x: 1, y: 2, z: 3); // Sum(1, 2, 3); と同じ意味。
        var s5 = Sum(z: 3);             // Sum(0, 0, 3); と同じ意味。
    }

    static int Sum(int x = 0, int y = 0, int z = 0)
    {
        return x + y + z;
    }
}
```


詳しくは、「[オプション引数・名前付き引数](sp4_optional.md)」で説明します。

リンク先の余談、「[余談： なんでいまさら？](sp4_optional.md#fyi)」 で説明していますが、引数の規定値には「後から値を変えにくい」という問題があるので、使用する際には注意が必要です。
とはいえ、後から値を変えることもそう多くなく、便利に使える機能です。


### <a id="sec-generated-title-20"></a> <a id="actual-formal"></a>実引数と仮引数

引数という言葉は、「引数として渡す値」と「引数を受け取るための変数」という2つの意味で使われます。
区別が必要な場合、前者を<strong id="actual-parameter" class="keyword">実引数</strong>(actual parameter)、後者を<strong id="formal parameter" class="keyword">仮引数</strong>(formal parameter)と呼びます。

例えば、先ほどの例で言うと、以下のように、Norm 関数に渡す 3, 4, 5 などの数値が実引数、
Norm 関数の定義側にある x, y, z などの変数が仮引数です。

```csharp
static void Main()
{
    var norm = Norm(3, 4, 5); // 3, 4, 5 が実引数
}

static double Norm(double x, double y, double z) // x, y, z が仮引数
{
    return x * x + y * y + z * z;
}
```



##### <a id="sec-generated-title-21"></a>余談: paramter と argument

少し余談になりますが、引数は、英語だと parameter 以外に、argument (ここでは「独立変数」の意味。数学用語としての argument)という単語を使うこともあります。
(ちなみに、parameter も、数学用語としては「媒介変数」という訳語になります。)

流儀によっては、実引数の意味で argument、仮引数の意味で parameter といように単語で呼び分けることもあるようです。
(どちらがどちらだったかわからなくなるので、あまり推奨はされません。結局、actual argument, formal argument という言い方もします。)


### <a id="sec-generated-title-22"></a> <a id="special-case"></a>特殊な引数

C# には、いくつか特殊な引数があります。
詳しくは別項で説明しています。

* ref, out:「[引数の参照渡し](../resource/sp_ref.md)」

* params:「[可変長引数](sp_params.md)」

* this:「[拡張メソッド](../functional/sp3_extension.md)」



## <a id="sec-generated-title-23"></a> <a id="overload"></a>関数のオーバーロード

関数を作る際、関数の名前が同じで引数リストだけが異なる関数を複数作ることが出来ます。
例えば、以下のように同じ名前の関数を作成することが出来ます。

```csharp
void WriteTypeAndValue(string s)
{
  Console.Write("文字列 : {0}\n", s);
}

void WriteTypeAndValue(int n)
{
  Console.Write("整数   : {0}\n", n);
}

void WriteTypeAndValue(double x)
{
  Console.Write("実数   : {0}\n", x);
}
```


このように、引数リストだけが異なる関数を作ることを関数の<strong id="overload" class="keyword">オーバーロード</strong>(overload : 過負荷、上積み)といいます。

##### <a id="sec-generated-title-24"></a>サンプル

```csharp
using System;

class OverloadSample
{
  static void Main()
  {
    WriteTypeAndValue("サンプル"); // WriteTypeAndValue(string) が呼ばれる
    WriteTypeAndValue(13);         // WriteTypeAndValue(int)    が呼ばれる
    WriteTypeAndValue(3.14159265); // WriteTypeAndValue(double) が呼ばれる
  }

  /// <summary>
  /// 型名と値を出力する(string 版)。
  /// </summary>
  static void WriteTypeAndValue(string s)
  {
    Console.Write("文字列 : {0}\n", s);
  }

  /// <summary>
  /// 型名と値を出力する(int 版)。
  /// </summary>
  static void WriteTypeAndValue(int n)
  {
    Console.Write("整数   : {0}\n", n);
  }

  /// <summary>
  /// 型名と値を出力する(double 版)。
  /// </summary>
  static void WriteTypeAndValue(double x)
  {
    Console.Write("実数   : {0}\n", x);
  }
}
```


```console
文字列 : サンプル
整数   : 13
実数   : 3.14159265
```

### <a id="sec-generated-title-25"></a> <a id="non-ovarloadable"></a>オーバーロードできない例

C# のメソッドのオーバーロードにはいくつか制限があります。

引数の型違いのオーバーロードはできますが、引数名だけが違うオーバーロードは作れません。

```csharp
// F は、引数の型が違うので大丈夫
static void F(int x) { }
static void F(string x) { }

// G は、引数の型まで一緒で、名前だけ違う。これはコンパイル エラー
static void G(int x) { }
static void G(int y) { }
```

また、戻り値だけ違うオーバーロードも作れません。

```csharp
// H は、引数が一致していて、戻り値だけ違う。これもコンパイル エラー
static int H() => 1;
static string H() => "";
```

あと、「C# としては区別しているように見えるけども、内部的には同じ扱いになっていて区別できないのでオーバーロードにも使えない」という型がいくつかあります。

- [`dynamic`](../dynamic/sp4_dynamic.md)は[内部的には`object`扱い](../dynamic/sp4_callsite.md)
- [`in`、`ref`、`out`は内部的には同じ扱い](../resource/sp_ref.md#ref-in-out)

例えば以下のようなオーバーロードは作れません。

```csharp
void D(object x) { }
void D(dynamic x) { }
```

```csharp
void F(ref int x) { }
void F(in int x) { }

void G(ref int x) { }
void G(out int x) => x = 0;

void H(in int x) { }
void H(out int x) => x = 0;
```

### <a id="sec-generated-title-26"></a> <a id="signature"></a>シグネチャ

オーバーロードがある以上、関数は、複数ある関数のうちのどれを呼ぶか、名前だけ特定することができません。
特定には、関数名と、引数の型が必要になります。
こういう、関数の呼び分けに必要な情報のことを<strong id="key-signature" class="keyword">シグネチャ</strong>(signature: 署名、サイン)と呼びます。

前述の通り、C#の場合は引数名や戻り値の型はオーバロード解決には使えないので、これらはシグネチャには含まれません。
例えば、`int F(int x, int y)`というようなメソッドがあった場合、このメソッド`F`のシグネチャは`F(int, int)`です(引数名と戻り値の型が消える)。

参考までに他の言語の例を上げておくと、
C++やJavaはC#と同様です。
C言語やGoは、関数のオーバーロード自体認めていない(呼び分けには名前自体を変えないといけない)ので、関数名 = シグネチャです。
Swiftでは、引数名違いや戻り値の型違いのオーバーロードができるので、`func x(x: Int) -> Int`というような関数があった場合、`x(x: Int) -> Int`全体がシグネチャです。

### <a id="sec-generated-title-27"></a> <a id="method-group"></a>メソッド グループ

関数の中でも[メソッド](#method)の場合は、[デリゲート](../functional/sp_delegate.md)への代入の際に `Action a = M;` みたいな引数なしな書き方ができます。

この引数なしな書き方では、`M`だけではなくてその周りまで見ないと「どの`M`か」が確定しません。
すなわち、「いくつかあるメソッド`M`のうちのいずれか」という状態です。
この状態の`M`を<strong id="key-method-group" class="keyword">メソッド グループ</strong>(method group)と呼びます。

```csharp
using System;

class Program
{
    // M(int) という「メソッド」
    static void M(int x) { }

    // M(string) という「メソッド」
    static void M(string x) { }

    static void Main()
    {
        // 右辺だけ見ると M は「M(int) か M(string) のどちらか」という状態
        // この状態の M をメソッド グループという
        // 左辺の Action<int> を見て初めてどちらなのかが確定する
        Action<int> a = M;
    }
}
```

## <a id="sec-generated-title-28"></a> <a id="sec-expression-bodied"></a>expression-bodied な関数

<h5 class="version version6">Ver. 6</h5>
C# 6 では、関数本体の部分が1つの式だけからなる場合、 `=>` 記号を使って以下のような書き方をすることができるようになりました。
これを、<strong id="expression-bodied" class="keyword">expression-bodied (本体が式の)関数</strong>(expression-bodied function)と呼びます。

例えば、先ほど例に出した2つの関数、Random と Norm は以下のように書くこともできます。

```csharp
static ulong Random() => unchecked(seed = seed * 1566083941UL +  1 );

static double Norm(double x, double y, double z) => x * x + y * y + z * z;
```

C# 6時点では、メソッド、演算子、プロパティとインデクサー(get-only)を `=>` 記号で書けます。

```csharp
class Csharp6
{
    // メソッド
    int Method(int x) => x * x;

    // 演算子
    public static Csharp6 operator +(Csharp6 x) => x;

    // プロパティ(get-only)
    int X => 0;

    // インデクサー(get-only)
    int this[int index] => index;
}
```

また、C# 7では、コンストラクター、ファイナライザー、プロパティとインデクサー(get/set それぞれ)、イベント(add/removeそれぞれ)も `=>` 記号で書けるようになりました。

```csharp
class Csharp7
{
    static int x;

    // コンストラクター
    Csharp7() => x++;

    // ファイナライザー
    ~Csharp7() => x--;

    // プロパティ(get/set)
    int X
    {
        get => x++;
        set => x--;
    }

    // インデクサー(get/set)
    int this[int index]
    {
        get => x += index;
        set => x -= index;
    }

    // イベント(add/remove)
    event Action E
    {
        add => x++;
        remove => x--;
    }
}
```

## <a id="sec-generated-title-29"></a> <a id="sec-local"></a>ローカル関数

<h5 class="version version7">Ver. 7</h5>

C# 7では、関数の中で別の関数を定義して使うことができます。
関数の中でしか使えないため、<strong id="key-local">ローカル関数</strong>(local function: その場所でしか使えない関数)と呼びます。

例えば以下のように書けます。

```csharp
using System;

class Program
{
    static void Main()
    {
        // Main 関数の中で、ローカル関数 f を定義
        int f(int n) => n >= 1 ? n * f(n - 1) : 1;

        Console.WriteLine(f(10));
    }
}
```

詳細は「[ローカル関数と匿名関数](../functional/fun_localfunctions.md)」で説明しています。

## <a id="sec-generated-title-30"></a> <a id="anonymous"></a>匿名関数

<h5 class="version version2">Ver. 2.0</h5>

もう1つ、関数の中に関数を書く方法として、<strong id="key-anonymous" class="keyword">匿名関数</strong>(anonymous function)というものがあります。

以下のような書き方をします。

```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        var input = new[] { 1, 2, 3, 4, 5 };
        var output = input
            .Where(n => n > 3)
            .Select(n => n * n);

        foreach (var x in output)
        {
            Console.WriteLine(x);
        }
    }
}
```

強調表示している部分が匿名関数です。
通常の関数もローカル関数も名前を持っていますが、匿名関数は、その名前通り、無名です。

匿名関数は、ローカル関数と比べて制限も多いですが、その代わり、どこにでも書けるという利点があります。
(正確には、[式](../start/st_variable.md#expression)が書ける場所ならどこにでも書けます。
一方、ローカル関数は[ステートメント](../start/st_variable.md#statement)です。)

詳細は「[ローカル関数と匿名関数](../functional/fun_localfunctions.md)」で説明しています。
## <a id="exercise"></a>演習問題

### <a id="exercise-func0"></a>問題 1


int 型の配列に格納されている値の最大値、最小値および平均値を求める関数をそれぞれ作成せよ。

```csharp
/// <summary>
/// 配列中の値の最大値を求める。
/// </summary>
/// <param name="a">対象の配列</param>
/// <returns>最大値</returns>
static int Max(int[] a)

/// <summary>
/// 配列中の値の最小値を求める。
/// </summary>
/// <param name="a">対象の配列</param>
/// <returns>最小値</returns>
static int Min(int[] a)

/// <summary>
/// 配列中の値の平均値を求める。
/// </summary>
/// <param name="a">対象の配列</param>
/// <returns>平均値</returns>
static double Average(int[] a)
```



#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    // 配列長の入力
    Console.Write("配列の長さ: ");
    int n = int.Parse(Console.ReadLine());

    // 配列の値の入力
    int[] a = new int[n];
    for (int i = 0; i < n; ++i)
    {
      Console.Write("{0}: ", i);
      a[i] = int.Parse(Console.ReadLine());
    }

    Console.Write(
@"
最大値: {0}
最小値: {1}
平均値: {2}
"
    , Max(a), Min(a), Average(a));
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最大値</returns>
  static int Max(int[] a)
  {
    int max = int.MinValue;

    for (int i = 0; i < a.Length; ++i)
    {
      if (max < a[i]) max = a[i];
    }

    return max;
  }

  /// <summary>
  /// 配列中の値の最小値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最小値</returns>
  static int Min(int[] a)
  {
    int min = int.MaxValue;

    for (int i = 0; i < a.Length; ++i)
    {
      if (min > a[i]) min = a[i];
    }

    return min;
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>平均値</returns>
  static double Average(int[] a)
  {
    double average = 0;

    for (int i = 0; i < a.Length; ++i)
    {
      average += a[i];
    }

    return average / a.Length;
  }
}
```



### <a id="exercise-func1"></a>問題 2


double 型の値 x の整数冪を求める関数 Power を作成せよ。

```csharp
/// <summary>
/// x の整数冪を求める。
/// </summary>
/// <param name="x">仮数 x</param>
/// <param name="n">指数 n</param>
/// <returns>x の n 乗</returns>
static double Power(
  double x,
  int n)
```



#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    const double x = 3;
    Console.Write("{0}\n", Power(x, 4));
    Console.Write("{0}\n", Power(x, -1));
    Console.Write("{0}\n", Power(x, -2));
    Console.Write("{0}\n", Power(x, 0));
  }

  /// <summary>
  /// x の整数冪を求める。
  /// </summary>
  /// <param name="x">仮数 x</param>
  /// <param name="n">指数 n</param>
  /// <returns>x の n 乗</returns>
  static double Power(
    double x,
    int n)
  {
    if (n == 0)
      return 1;

    if (n < 0)
    {
      x = 1.0 / x;
      n = -n;
    }

    double y = x;
    while (--n > 0)
    {
      y *= x;
    }

    return y;
  }
}
```



### <a id="exercise-func2"></a>問題 3


配列に格納されている値の最大値を求める関数を、
<code>int[]</code> に対するものと
<code>double[]</code> に対するものの2種類作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    int[]    ai = new int[]    { 1, 3, 9, 2, 5, 6, 4 };
    double[] ad = new double[] { 1, 3, 9, 2, 5, 6, 4 };

    Console.Write("{0}, {1}\n", Max(ai), Max(ad));
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最大値</returns>
  static int Max(int[] a)
  {
    int max = int.MinValue;
    for (int i = 0; i < a.Length; ++i)
    {
      if (max < a[i]) max = a[i];
    }
    return max;
  }

  /// <summary>
  /// 配列中の値の最大値を求める。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <returns>最大値</returns>
  static double Max(double[] a)
  {
    double max = int.MinValue;
    for (int i = 0; i < a.Length; ++i)
    {
      if (max < a[i]) max = a[i];
    }
    return max;
  }
}
```


見ての通り、型が変わっただけで、処理自体は全く同じものになっています。
このように、型と無関係に同じ処理で実現できるものは、
「[ジェネリック](../oop/sp2_generics.md#generics)」を使うことで実装の手間を軽減できます。
