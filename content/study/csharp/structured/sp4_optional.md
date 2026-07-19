---
title: "オプション引数・名前付き引数"
source_url: "https://ufcpp.net/study/csharp/structured/sp4_optional/"
content_type: "Article"
published_at: "2009-05-24T00:00:00"
updated_at: "2017-10-22T00:00:00"
tags:
  - "Ver. 4.0"
umbraco_id: 1238
parent_id: 1217
sort_order: 10
aliases:
  - "/csharp/sp4_optional"
  - "/csharp/sp4_optional.html"
  - "/csharp/structured/sp4_optional/"
  - "/study/csharp/sp4_optional"
  - "/study/csharp/sp4_optional.html"
---

# オプション引数・名前付き引数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version4">Ver. 4.0</h5>

C# 4.0 でオプション引数と名前付き引数が追加されました。


##### <a id="sec-generated-title-2"></a>ポイント

* オプション引数と規定値：<code>int Sum(int x = 0, int y = 0) { return x + y; }</code>とか書けるようになった

* オプション引数の省略：<code>Sum(); Sum(1); Sum(1, 2);</code>

* 名前付き引数：<code>Sum(x: 1, y: 2); Sum(y:1, x: 2); Sum(y: 1);</code>

* 引数の規定値も、引数名も、public なものは後から変更してはいけない(利用側コードを壊す)ので要注意。



## <a id="sec-generated-title-3"></a> <a id="optional"></a>オプション引数

オプション引数は C++ にもある機能ですね。
これは、メソッドのオーバーロードで似たようなことが可能なので、
今まで C# では敬遠し続けてきたようです。

まず、C++ 同様、
以下のように規定値(default value)を持ったメソッドを定義します。

```csharp
static int Sum(int x = 0, int y = 0, int z = 0)
{
  return x + y + z;
}
```


すると、以下のように、引数の一部もしくは全てを省略可能になります。
省略可能ということで、オプション引数（optional parameter）と呼びます。

```csharp
int s1 = Sum();     // Sum(0, 0, 0); と同じ意味。
int s2 = Sum(1);    // Sum(1, 0, 0); と同じ意味。
int s3 = Sum(1, 2); // Sum(1, 2, 0); と同じ意味。
```


この記法で省略可能になるのは、後ろの引数のみです。
この例でいうところの、z だけをオプションにして x と y だけを省略することはできません。
定義側でも、以下のようなコードはコンパイルエラーになります。
（z のところで「オプション引数の後ろに必須引数を置いちゃダメ」みたいなエラーが出ます。）

```csharp
static int Sum(int x = 0, int y = 0,
  int z) // コンパイル エラー。後ろの引数に既定値がない
{
  return x + y + z;
}
```


ただし、オプション引数の後ろに params（「[可変長引数](sp_params.md)」参照）を続けることは可能です。

```csharp
static int Sum(int x, int y, int z = 0, params int[] rest)
{
    var sum = x + y + z;
    foreach (var v in rest) sum += v;
    return sum;
}
```


オプション引数や可変長引数を使った場合の「[オーバーロード](st_function.md#overload)」の優先順位ですが、
オプションなし ＞ オプションあり ＞ 可変長引数 の順で優先されます。

```csharp
static void Main(string[] args)
{
    Sum(1);
    Sum(1, 2);
    Sum(1, 2, 3);
    Sum(1, 2, 3, 4);
}

static int Sum(int x)
{
    Console.WriteLine("Sum(x)");
    return x;
}

static int Sum(int x, int y = 0, int z = 0) // 引数2つ以上でないと呼ばれない
{
    Console.WriteLine("Sum(x, y, z)");
    return x + y + z;
}

static int Sum(params int[] rest) // 引数4つ以上でないと呼ばれない
{
    Console.WriteLine("Sum(rest)");
    var sum = 0;
    foreach (var v in rest) sum += v;
    return sum;
}
```


```console
Sum(x)
Sum(x, y, z)
Sum(x, y, z)
Sum(rest)
```



## <a id="sec-generated-title-4"></a> <a id="named"></a>名前付き引数

で、もう1つ、
こちらも VB には昔からある機能なんですが、
名前付き引数（named parameter）が使えるようになりました。

先ほど定義した引数の規定値付きのメソッドを、以下のような構文で呼び出せます。

```csharp
int s1 = Sum(x: 1, y: 2, z: 3); // Sum(1, 2, 3); と同じ意味。
int s2 = Sum(y: 1, z: 2, x: 3); // Sum(3, 1, 2); と同じ意味。
int s3 = Sum(y: 1);             // Sum(0, 1, 0); と同じ意味。
```


名前付き引数の場合、引数の順序は自由に書けます。
また、任意の箇所を省略可能になります。

1つ気をつけないといけないのは、引数の名前を指定するのに = じゃなくて : を使うところです。
C# の場合、以下のような構文が許されているので、間違えて = と書いてしまわないよう気をつけましょう。

```csharp
static void Main(string[] args)
{
    int x = 0;
    Console.WriteLine(Square(x = 2)); // 単なる代入。名前付き引数ではない
    // ↑これは↓と同じ意味。
    // x = 2;
    // Console.WriteLine(Square(x));
}

static int Square(int x)
{
    return x * x;
}
```

また、C# 7.1 以前では、通常の(位置指定の)引数と名前付き引数を混在させる場合、名前付きにできるのは後ろの方の引数だけです。

```csharp
static void Order()
{
    // OK: 前の方は位置指定、後ろの方は名前指定
    Sum(1, z: 2, y: 3);

    // コンパイル エラー: 前の方の引数を名前指定するのはダメ
    Sum(1, x: 2, y: 3);
}

static int Sum(int x = 0, int y = 0, int z = 0) => x + y + z;
```

### <a id="sec-generated-title-5"></a> <a id="non-trailing-named"></a>非末尾名前付き引数 (前の方の引数を名前付きに)

<h5 class="version version7_1">Ver. 7.2</h5>

C# 7.2で、前の方の引数を名前付きにできるようになりました。
例えば、以下のような書き方が許されるようになりました。

```csharp
// C# 7.2
// 末尾以外でも名前を書けるように
Sum(x: 1, 2, 3);
```

ただし、この場合、順序の変更は認められておらず、通常(位置指定)と同じ順で引数を書く必要があります。

```csharp
// C# 7.2 でもダメなやつ
// 末尾以外の引数を名前付きにしたい場合、順序は厳守する必要あり
Sum(2, 3, x: 1);
```

要するに、引数の省略や順序変更を目的としているのではなく、
単に「どの実引数が何の意味か」が名前からわかるようにしたいときに使うものです。

例えば、よくある話だと、「`Copy(a, b, length)`では、`a`と`b`のどちらがコピー元でどちらがコピー先かがわからなくて困る」といった問題があったりします。
この際に、以下のように書ければ便利だろうということで名前付き引数の制限が緩和されました。

```csharp
var a = new[] { 1, 2, 3, 4, 5 };
var b = new int[3];
Array.Copy(sourceArray: a, destinationArray: b, 3);
```

## <a id="sec-generated-title-6"></a> <a id="implementation"></a>内部実装

##### <a id="sec-generated-title-7"></a>オプション引数（メソッド定義側）

オプション引数の仕組みは、今までの VB.NET と同じ実装方法で実現されていて、
実体は Optional 属性と DefaultParameterValue 属性になっています。
例えば、以下のようなコードを書くと、

```csharp
static int Sum(int x = 0, int y = 0, int z = 0)
{
    return x + y + z;
}
```


以下のようなコードと同じコンパイル結果になります。
（Optional, DefaultParameterValue はいずれも System.Runtime.InteropServices 名前空間内に定義されている属性です。）

```csharp
static int Sum(
    [Optional, DefaultParameterValue(0)] int x,
    [Optional, DefaultParameterValue(0)] int y,
    [Optional, DefaultParameterValue(0)] int z)
{
    return ((x + y) + z);
}
```



##### <a id="sec-generated-title-8"></a>名前付き引数（メソッド定義側）

C# （や、VB など、.NET 上の言語）では、元々、コンパイル結果にメソッドの引数名に関する情報が残っています。
名前付き引数はこの情報に基づいて実装されています。


##### <a id="sec-generated-title-9"></a>メソッド呼び出し側

オプション引数や名前付き引数を使ったメソッド呼び出しでは、
コンパイル時に値が全て展開された状態になります。

例えば、先ほどの Sum メソッドに対して、以下のようなコードは、

```csharp
Sum();
Sum(1);
Sum(1, 2);
Sum(x: 1, y: 2, z: 3);
Sum(y: 1, z: 2, x: 3);
```


以下のようなコードと完全に同じコンパイル結果になります。

```csharp
Sum(0, 0, 0); // 元々 0 を渡していたのか、オプション引数で 0 になったのかはわからない
Sum(1, 0, 0);
Sum(1, 2, 0);
Sum(1, 2, 3); // x, y, z 等の引数名に関する情報は残らない
Sum(3, 1, 2);
```



## <a id="sec-generated-title-10"></a> <a id="fyi"></a>余談： なんでいまさら？

引数の規定値は C++ にもあるし、
VB はオプション引数・名前付き引数ともにかなり前から実装していました。
C# でも、かなり初期の頃からずっと、オプション引数・名前付き引数が欲しいという要望はたびたび出ていました。
にもかかわらず、C# 4.0 でようやくの実装になります。

というのも、名前付き引数や引数の規定値には多少のリスクも伴うからです。
一番の問題は、簡単に言うと、後から名前や値を変えにくい(変えると利用側コードを壊す)という点です。
それから、仮想メソッドに対して規定値を与える場合には特に注意が必要になります。


### <a id="sec-generated-title-11"></a> <a id="const-issue"></a>規定値の変更

1つ目は、引数の規定値は定数扱いになっていて、コンパイル結果に直接埋め込まれるということです。
「[const メンバー](../start/sp_const.md#const_member)」で説明している定数の問題と同様に、利用側でも再コンパイルが必要という問題があります。
(定数と同様、問題になるのは public な場合です。internal や private の場合には問題になりません。)

例えば、ライブラリ内で以下のようなコードを書いたとして、

```csharp
static int Sum(int x = 0, int y = 0, int z = 0)
{
    return x + y + z;
}
```


このライブラリを使う以下のようなコードを書いたとしてます。

```csharp
Sum();
```


この <code>Sum()</code> は <code>Sum(0, 0, 0)</code> と解釈されます。

この後、ライブラリを以下のように更新したとします。

```csharp
static int Sum(int x = 1, int y = 2, int z = 3)
{
    return x + y + z;
}
```


当然、<code>Sum()</code> の部分は <code>Sum(1, 2, 3)</code> になって欲しいわけですが、
利用側を再コンパイルするまで、<code>Sum(0, 0, 0)</code> のままになります。
すなわち、「ライブラリだけコンパイルしなおして再配布」とかやろうとすると問題を起こす可能性があります。

なので、C# では今まで、引数の規定値を導入する代わりに、
メソッドのオーバーロードを使った以下のような実装方法を推奨していました。
この場合は、利用側の再コンパイルは必要なくなります。

```csharp
static int Sum(int x, int y, int z)
{
    return x + y + z;
}
static int Sum()
{
    return Sum(0, 0, 0);
}
```



### <a id="sec-generated-title-12"></a> <a id="name-issue"></a>名前も public

2つ目は名前付き引数に関して。

メソッドの定義側で引数名を変更した場合、
利用側も、名前付き引数構文で呼び出ししている場合には修正が必要になります。

例のごとく、以下のような Sum メソッドがあって、

```csharp
static int Sum(int x, int y, int z)
{
    return x + y + z;
}
```


これを以下のような名前付き引数を使って呼び出しているとします。

```csharp
Sum(x: 0, y: 1, z: 2);
```


この時、Sum の定義側を以下のように変更すると、

```csharp
static int Sum(int a, int b, int c)
{
    return a + b + c;
}
```


呼び出している側で、「そんな名前の引数はないよ」というエラーになります。
一般に、メソッド名などは変更すると利用側にも影響があるので、名前の変更には慎重になるはずです。
要するに、名前付き引数を使うと、引数名もメソッド名と同程度に、変更に慎重になる必要が出ます。

まあ、名前付き引数が入るまでもなく、引数名は元々「表に出ているもの」（ドキュメントコメント等にも残る情報。誰からでも見えている。）なので、
元々そんなに軽々しく変更するものではないんですが、
名前付き引数を使うなら特に注意が必要です。


### <a id="sec-generated-title-13"></a> <a id="compile-time"></a>コンパイル時に決定

3つ目は、仮想メソッドと一緒に使うと少しわかりにくい挙動をするという問題です。

どの規定値が使われるかは、変数の型を見て決定されます。
以下の例のように、
仮想メソッドの場合は(変数の型ではなく、その中身の)インスタンスの型に基づいて呼び出し先が変わるにも関わらず、
規定値だけは変数の型の方を見て決まるので、多少わかりづらい挙動になります。

```csharp
using System;

class Base { public virtual void X(string s = "base") => Console.WriteLine(s + " in base"); }
class Derived : Base { public override void X(string s = "derived") => Console.WriteLine(s + " in derived"); }

class Program
{
    static void Main()
    {
        Base x = new Base();
        x.X(); // base in base

        Derived y = new Derived();
        y.X(); // derived in derived

        Base z = new Derived();
        z.X(); // base in derived
    }
}
```



### <a id="sec-generated-title-14"></a> <a id="d57e944"></a>とはいえ便利

このようないくつかの問題はあるものの、名前付き引数は非常に便利です。
まとめると要するに以下の点にだけ気を付ければいいので、そこまで及び腰になる必要もないでしょう。

* 引数名や規定値は後から変えると影響でかい。

* 仮想メソッドに対して規定値を与えると混乱の元。


ちなみに、C# に引数の規定値(オプション引数と名前付き引数)が導入されたのは、C#４.0 からなわけですが、
4.0 では同時に、COM 相互運用強化がありました。
COM (詰まるところ90年代からあるレガシー資産)では、オプション引数や名前付き引数がないとかなり煩雑な処理を書く必要があります。
ある意味、この COM 相互運用強化の一環として引数の規定値が導入されたと考えられます。

実際、C# 3.0 以前では、例えば C# から Excel の機能を（COM 経由で）呼び出そうとすると、以下のような悲惨なコードになることがありました。

```csharp
Workbook workbook = excelApp.Workbooks.Open(
    "sample.xsl", Type.Missing, true, Type.Missing, Type.Missing,
    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
```


Type.Missing というのは、
オプション引数をサポートしていない言語からオプション引数を利用するための苦肉の策です。

（ちなみに、
この馬鹿みたいにいっぱいある引数は、どういうモードでワークブックを開くかです。
例えば、ワークブックを読み取り専用で開いたりとかを指定するためにある。）

で、これが C# 4.0 なら以下のようにシンプルに書けるようになります。

```csharp
Workbook workbook = excelApp.Workbooks.Open("sample.xsl", ReadOnly: true);
```
