---
title: "条件分岐"
source_url: "https://ufcpp.net/study/csharp/structured/st_branch/"
content_type: "Article"
published_at: "2015-05-06T14:08:20"
updated_at: "2021-01-02T00:00:00"
tags: []
umbraco_id: 1220
parent_id: 1217
sort_order: 2
aliases:
  - "/csharp/st_branch"
  - "/csharp/st_branch.html"
  - "/csharp/structured/st_branch/"
  - "/study/csharp/st_branch"
  - "/study/csharp/st_branch.html"
---

# 条件分岐

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

プログラム中で、ある条件を満たしたときだけ処理の流れを変えたい場合があります。
このような処理を<strong id="branch" class="keyword">条件分岐</strong>といい、
C#では条件分岐のために <code>if</code>、<code>else</code>、<code>switch</code> などのキーワードを用意しています。


##### <a id="sec-generated-title-2"></a>ポイント

* if(条件式) 真のとき

* if(条件式) 真のとき else 偽のとき

* switch(条件) { case 値: ... }

* goto Label;



## <a id="sec-generated-title-3"></a> <a id="if"></a>if 文

<strong id="if" class="keyword">if</strong> 文は以下のような書き方をします。

```csharp
if(条件式)
  文1 // 条件式が真のときに実行される
else
  文2 // 条件式が偽のときに実行される
```


英文法に近い書き方ですね。
if A, B, else C （もし A ならば B、さもなくば C）。

if 文は <code>if</code> の後の括弧内に書かれた条件式の真偽によって処理の流れを変えます。
条件式が真のときには 文1 が、偽のときには 文2 が実行されます。
また、<code>else</code> から後ろの部分は省略することができます。


##### <a id="sec-generated-title-4"></a>サンプル

```csharp
using System;

class IfSample
{
  static void Main()
  {
    // 整数を入力してもらう
    int x;
    Console.Write("整数を入力してください : ");
    x = int.Parse(Console.ReadLine());

    if(x == 0)
    {
      // 0が入力された場合、エラーメッセージだけ表示
      Console.Write("0が入力されました");
    }
    else
    {
      // 0以外が入力された場合、入力された数値の逆数を求めて表示
      double x_inv = 1.0 / x;
      Console.Write("1/{0} = {1}", x, x_inv);
    }
  }
}
```


```console
整数を入力してください : 4
1/4 = 0.25
```


```console
整数を入力してください : 0
0が入力されました
```

### <a id="sec-generated-title-5"></a> <a id="conditional-operator"></a>条件演算子

「[組み込み演算子](../start/st_operator.md#condition)」で紹介した条件演算子`?:`は、「`if`文の[式](miscexpressions.md#term)版」とも言える機能です。
式なので戻り値が必須ですが、以下のように、条件を満たすときと満たさないときの両方で同じ型の値を返す場合には条件演算子を使った方がすっきり書けることが多いです。

```csharp
using System;
 
class Program
{
    static void Main()
    {
        var num = int.Parse(Console.ReadLine());
 
        // if で偶奇判定
        string parity1;
        if (num % 2 == 1) parity1 = "odd";
        else parity1 = "even";
 
        // 条件演算子で偶奇判定
        var parity2 = num % 2 == 1 ? "odd" : "even";
    }
}
```

#### <a id="sec-generated-title-6"></a> <a id="terget-typed-conditional"></a>条件演算子のターゲット型推論

<h5 class="version version9">Ver. 9</h5>

C# 9.0 から条件演算子に[ターゲット型](../start/misctyperesolution.md#target-type)からの型推論が働くようになりました。

これまで、条件演算子の結果は第2項・第3項から判別できる共通の型で決めていました。
(ソース型からの推論(例えば [`var`](../start/sp3_inference.md#implicit) など)とターゲット型からの推論は両立できないので、`var` などを使う場合には C# 9.0 でもこれまで通り、共通の型で決まります。)
共通の型が判別できないときにはコンパイル エラーになります。

例えば以下のようなコードはコンパイルできません。

```csharp
void M(bool b)
{
    // C# では整数型と null の共通型判定ができない。
    // 自動的に int? になってくれたりはしない(int? が後入り機能なせい)。
    var i = b ? 1 : null;
 
    // C# では「共通の基底クラスを探す」とかの処理はやらない。
    // インターフェイスは多重継承が可能で、共通基底を探す処理はかなりの時間を要することがあって、意図的に避けている。
    var c = b ? new A() : new B();
}
 
class Base { }
class A : Base { }
class B : Base { }
```

これが、C# 9.0 から、ターゲット型を指定することでコンパイルできるようになります。

```csharp
void M(bool b)
{
    // var をやめて、int? を明示。
    int? i = b ? 1 : null;
 
    // var をやめて、Base を明示。
    Base c = b ? new A() : new B();
}
```

## <a id="sec-generated-title-7"></a> <a id="switch"></a>switch 文

<strong id="switch" class="keyword">switch</strong> 文は以下のような書き方をします。

```csharp
switch(変数)
{
  case 値1:
    いくつかの文1 // 変数の値 == 値1 のとき実行される
    break;
  case 値2:
    いくつかの文2 // 変数の値 == 値2 のとき実行される
    break;
      ・
      ・
      ・
  default:
    いくつかの文 // 変数の値がどの値とも異なるとき実行される
    break;
}
```


<code>switch</code> の後ろの括弧に書かれた変数の値によって処理の流れを変えます。
<code>switch</code> 中で使える変数は、整数型もしくは文字列型の変数のみです。

そして、<code>case</code> の後ろに条件となる値を書きます。
変数の値が <code>case</code> で指定されたどの値とも異なる場合、
<code>default</code> というラベルのついた場所に処理の流れが移ります。
<code>break</code> は switch 文から抜けるために使います。

### <a id="sec-generated-title-8"></a> <a id="type-switch"></a>型による分岐

<h5 class="version version7">Ver. 7</h5>

C# 6までは、`case`に書ける条件は値のみでした。その値と一致したときにだけ、`case`以下の文が実行されます。

一方、C# 7からは、型による分岐ができるようになりました。例えば以下のような書き方ができます。

```csharp
static void TypeSwitch(object obj)
{
    switch (obj)
    {
        case int n:
            Console.WriteLine("整数 " + n);
            break;
        case string s:
            Console.WriteLine("文字列 " + s);
            break;
        default:
            Console.WriteLine("その他");
            break;
    }
}
```

ちなみに、この書き方の場合、各`case`に対してさらに`when`句で条件を付けることができます。
この書き方では条件が被ることもありますが、そのときは書いた順に上から調べて最初に条件を満たした`case`が実行されます。

```csharp
static int TypeSwitch(object obj)
{
    switch (obj)
    {
        case int n when n < 1: return 0;
        case int n when n < 10: return 1;
        case int n when n < 100: return 2;
        case int n when n < 1000: return 3;
        case int n: return (int)Math.Log10(n);
        case int[] a: return a.Length;
        default: return -1;
    }
}
```

詳しくは「[型スイッチ](../datatype/typeswitch.md#switch)」で説明します。

### <a id="sec-generated-title-9"></a> <a id="tuple-switch"></a>複数の値で switch

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 からは、以下のように、複数の値をまとめて `switch` 文に掛けれるようになりました。

```csharp
static string Color(bool r, bool g, bool b)
{
    switch (r, g, b)
    {
        case (false, false, false): return "black";
        case (true, false, false): return "red";
        case (false, true, false): return "green";
        case (false, false, true): return "blue";
        case (false, true, true): return "cyan";
        case (true, false, true): return "magenta";
        case (true, true, false): return "yellow";
        case (true, true, true): return "white";
    }
}
```

正確に言うと、これは「[タプル](../datatype/tuples.md)に対する[位置パターン](../datatype/patterns.md#positional)」だったりします。
詳しくは「[タプル switch](../datatype/patterns.md#tuple-switch)」で説明します。

### <a id="sec-generated-title-10"></a> <a id="fallthrough"></a>フォールスルーの禁止

C# の先祖に当たる C 言語や C++ 言語では、
以下のようなコードが許されていました。

```cpp
swicth(x)
{
case 1:
  printf("x == 1 のときに実行される\n"); // (1)
case 2:
  printf("x == 1 でも x == 2 でも実行される\n"); // (2)
}
```


変数 <code>x</code> が 1 のとき、(1) と (2) の両方の行が実行されます。
<code>x</code> が 2 のときには (2) だけが実行され、
それ以外の場合は何も実行されません。
すなわち、C/C++ では、switch 文中の case ラベルを超えてコードが実行され、
このような動作を<em>フォールスルー</em>（fall through）と呼びます。

ですが、実際にプログラムを作る際、多くの場合では、
<code>x</code> が 1 のときと 2 のときで、
全く別の処理をしたい、すなわち、
フォールスルーして欲しくない場合がほとんどで、
以下のように、<code>braek</code> を挿入して、
case ラベルを超えてコードが実行されないようにします。

```cpp
swicth(x)
{
case 1:
  printf("x == 1 のときだけ実行される\n");
  break;
case 2:
  printf("x == 2 のときだけ実行される\n");
  break;
}
```


で、C/C++ では、
「フォールスルーして欲しくないのに、ついうっかり break を忘れる」
というバグが結構頻繁に起こりました。
そのため、<em>C# ではフォールスルーを禁止しています</em>。
すなわち、C# では、
case ラベル毎に必ず、break, 「[goto](#goto)」, 「[戻り値return](st_function.md#return)」 のいずれかを記述する必要があります。

毎回いちいち break を書くのが面倒ですが、必ず書く必要があります。
C/C++ 時代の名残です。

（「どうせフォールスルーできないんだから、break を明示的に書かなくてもフォールスルーしない仕様にして欲しい」という要望も多かったりします。
C# は C/C++ からの移行を意識して作られたので、
C/C++ プログラマの混乱を避けるために break を付ける構文になったんだと思います。
最初から C# でプログラミングを学び始める人もかなり出きた今となっては少々気持ち悪い構文です。
）

ただし、C# でも、以下のように、case ラベルが連続している場合に限りフォールスルー可能で、
break 等が必須ではありません。

```csharp
switch(x)
{
  case 1:
  case 2:
    Console.Write("x == 1 か x == 2 のときに実行される\n");
    break;
    // case ラベルが連続している場合のみ OK。
    // case 1: と case 2: の間にコードを書いては駄目。
}
```



##### <a id="sec-generated-title-11"></a>サンプル

```csharp
using System;

class SwitchSample
{
  static void Main()
  {
    // 整数を2つ入力してもらう
    int x, y;
    Console.Write("1つ目の整数を入力してください : ");
    x = int.Parse(Console.ReadLine());
    Console.Write("2つ目の整数を入力してください : ");
    y = int.Parse(Console.ReadLine());

    // + - / * のいずれかを入力してもらう
    char op;
    Console.Write("行いたい操作を入力してください(+ - / *) : ");
    op = Console.ReadLine()[0];

    switch(op)
    {
      case '+':
        Console.Write("{0} + {1} = {2}", x, y, x+y);
        break;
      case '-':
        Console.Write("{0} - {1} = {2}", x, y, x-y);
        break;
      case '*':
        Console.Write("{0} × {1} = {2}", x, y, x*y);
        break;
      case '/':
        if(y != 0)
          Console.Write("{0} ÷ {1} = {2} … {3}", x, y, x/y, x%y);
        break;
      default:
        Console.Write("対応していない操作です");
        break;
    }
  }
}
```


```console
1つ目の整数を入力してください : 5
2つ目の整数を入力してください : 7
行いたい操作を入力してください(+ - / *) : +
5 + 7 = 12
```


```console
1つ目の整数を入力してください : 11
2つ目の整数を入力してください : 3
行いたい操作を入力してください(+ - / *) : /
11 ÷ 3 = 3 … 2
```


```console
1つ目の整数を入力してください : 1
2つ目の整数を入力してください : 1
行いたい操作を入力してください(+ - / *) : 0
対応していない操作です
```

### <a id="sec-generated-title-12"></a> <a id="switch-expression"></a>switch 式

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0では、`switch`の[式](miscexpressions.md#term)版が追加されました。
以下のような書き方をします。

```csharp
変数 switch
{
    パターン1 => 式1,
    パターン2 => 式2,
      ・
      ・
      ・
}
```

詳しくは「[`switch` 式](../datatype/typeswitch.md#switch-expression)」で説明します。

## <a id="sec-generated-title-13"></a> <a id="goto"></a>goto 文

<strong id="goto" class="keyword">goto</strong> 文は if 文や switch 文と異なり、無条件に処理の流れを変えるものです。
例えば以下のように使います。

```csharp
START: // ジャンプ先を示すラベル
Console.Write("gotoの例");
goto START;// START: というラベルのある位置に処理の流れを移す
```


この例では、<code>Console.Write("gotoの例");</code>が何度も繰り返し実行されます。
(プログラムを強制終了するしか止める方法がないので注意)

goto 文を使用するとプログラムの処理の流れを追いづらくなるので、あまり使うのは好ましくないとされています。
そのため、通常は goto 文を使うのは以下のような場合に限られます。

1つは以下のように、switch 文で、<code>x</code> の値が1のときも2の時も同じ処理を行いたいといった場合に使います。

```csharp
switch(x)
{
  case 1:
    goto case 2; // gotoを使って処理を移す
  case 2:
    // x の値が1か2だった場合の処理
    break;
  case 3:
    // x の値が3だった場合の処理
    break;
  default:
    // そのほかの場合の処理
    break;
}
```


もう1つ、以下のように多重ループ(ループについては「[反復処理](st_loop.md)」で説明します)から抜け出すときにも使います。

```csharp
while(x != 0)
{
  while(y != 0)
  {
    // 繰り返し行いたい処理

    if(x == y)
      goto LOOPEND; // break では while(y != 0) の方のループしか抜けられない
  }
}
LOOPEND:
;
```
## <a id="exercise"></a>演習問題

### <a id="exercise-brancheo"></a>問題 1


ユーザから入力された整数が奇数か偶数か判定するプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("整数を入力してください: ");
    int n = int.Parse(Console.ReadLine());

    if (n % 2 == 0) Console.Write("{0} は偶数です\n", n);
    else            Console.Write("{0} は奇数です\n", n);
  }
}
```



### <a id="exercise-branch0"></a>問題 2


[組込み演算子](../start/st_operator.md)の[問題 5](../start/st_operator.md#exercise-ope5)のプログラムを修正し、
BMI 値から体型(やせ型、普通、やや肥満、肥満、高度肥満)を判定し、
表示するプログラムを作成せよ。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("身長[cm]: ");
    double height = double.Parse(Console.ReadLine()) * 0.01;
    Console.Write("体重[kg]: ");
    double weight = double.Parse(Console.ReadLine());

    double bmi = weight / (height * height);
    Console.Write("BMI = {0}\n", bmi);

    if(bmi < 19.8)      Console.Write("やせ型");
    else if(bmi < 24.2) Console.Write("普通");
    else if(bmi < 26.4) Console.Write("やや肥満（過体重）");
    else if(bmi < 35.0) Console.Write("肥満");
    else                Console.Write("高度肥満（要治療）");
    Console.Write("です\n");
  }
}
```



### <a id="exercise-branch1"></a>問題 3


switch 文を使って150以下の平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。

ヒント：
要するに、ユーザからの入力が 1, 4, 9, 16, ・・・になっているかどうかを switch 文で判別します。


#### 解答例 1


```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("整数を入力してください: ");
    int n = int.Parse(Console.ReadLine());

    switch (n)
    {
      case 1:
        Console.Write("{0} は平方数です。\n", n);
        break;
      case 2 * 2: goto case 1;
      case 3 * 3: goto case 1;
      case 4 * 4: goto case 1;
      case 5 * 5: goto case 1;
      case 6 * 6: goto case 1;
      case 7 * 7: goto case 1;
      case 8 * 8: goto case 1;
      case 9 * 9: goto case 1;
      case 10 * 10: goto case 1;
      case 11 * 11: goto case 1;
      case 12 * 12: goto case 1;
      default:
        Console.Write("{0} は平方数ではないか、150以上です\n", n);
        break;
    }
  }
}
```



### <a id="exercise-branch2"></a>問題 4


数値を3つ入力してもらい、
その3つの値の中の最大値、最小値を求めるプログラムを作成せよ。


#### 解答例 1


単純な条件分岐による方法。

```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("値1: ");
    double x = double.Parse(Console.ReadLine());
    Console.Write("値2: ");
    double y = double.Parse(Console.ReadLine());
    Console.Write("値3: ");
    double z = double.Parse(Console.ReadLine());

    if (x > y)
    {
      if (x > z)
      {
        if (y > z) Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", x, y, z);
        else       Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", x, z, y);
      }
      else Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", z, x, y);
    }
    else
    {
      if (y > z)
      {
        if (x > z) Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", y, x, z);
        else       Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", y, z, x);
      }
      else Console.Write("最大 {0}, 中間 {1}, 最小 {2}\n", z, y, x);
    }
  }
}
```



#### 解答例 2


3つの数値をあらかじめ整列してしまう方法。

```csharp
using System;

class Exercise
{
  static void Main()
  {
    Console.Write("値1: ");
    double x = double.Parse(Console.ReadLine());
    Console.Write("値2: ");
    double y = double.Parse(Console.ReadLine());
    Console.Write("値3: ");
    double z = double.Parse(Console.ReadLine());

    double tmp;

    if (y < z) { tmp = y; y = z; z = tmp; }
    if (x < y) { tmp = x; x = y; y = tmp; }
    if (y < z) { tmp = y; y = z; z = tmp; }

    Console.Write("最大 {0}, 中間{1}, 最小 {2}\n", x, y, z);
  }
}
```
