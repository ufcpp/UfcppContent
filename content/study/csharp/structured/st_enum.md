---
title: "列挙型"
source_url: "https://ufcpp.net/study/csharp/structured/st_enum/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2008-01-05T00:00:00"
tags: []
umbraco_id: 1241
parent_id: 1217
sort_order: 13
aliases:
  - "/study/csharp/st_enum.html"
---

# 列挙型

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# では、<strong id="enum" class="keyword">列挙型</strong>（enumeration type）と呼ばれるものを利用することで、曜日などの特定の値しかとらないデータを表現することが出来ます。


##### <a id="sec-generated-title-2"></a>ポイント

* 列挙型: 特定の値しか取らないようなもの（例えば曜日など）に対して使う型

* enum DayOfWeek { Monday, Tuesday, ... }



## <a id="sec-generated-title-3"></a> <a id="about"></a>列挙型とは

例えば、曜日は月・火・水・木・金・土・日の7つの値しか取りませんし、
英語の月は January, February, March, April, May, June, July, August, September, October, November, December の12個の値しか取りません。
その他にも、
飛行機の乗車クラス(エコノミー・ビジネス・ファースト)、
日本の年号(明治・大正・昭和・平成)、
性別(男・女)など、特定の値しか取らないものはたくさんあります。
C# で、このような特定の値しか取らない型を表現するためには列挙型というものを使います。

列挙型は以下のようにして定義します。

```csharp {title="列挙型の定義"}
enum 列挙型名
{
  メンバー1, メンバー2, …, メンバーn
}
```


列挙型を利用する側では以下のようにします。

```csharp {title="列挙型の利用"}
列挙型名.メンバー名
```


また、列挙型の値を <code>Console.Write</code> などに渡して表示すると、
メンバー名がそのまま表示されます。
例えば、和暦の年号を列挙型として定義すると以下のようになります。

```csharp {title="列挙型の例(年号)"}
using System;

enum 年号
{
  明治, 大正, 昭和, 平成
}

class EnumSample
{
  /// <summary>
  /// 和暦を西暦に変換する
  /// </summary>
  static void Main()
  {
    年号[] era = new 年号[5]{年号.昭和, 年号.大正, 年号.明治, 年号.平成, 年号.昭和};
    int[] j_year = new int[5]{33, 12, 20, 10, 54};
    int[] year = new int[5];

    Console.Write("和暦      西暦\n");
    for(int i=0; i<5; ++i)
    {
      switch(era[i])
      {
      case 年号.明治: year[i] = j_year[i] + 1863; break;
      case 年号.大正: year[i] = j_year[i] + 1911; break;
      case 年号.昭和: year[i] = j_year[i] + 1925; break;
      case 年号.平成: year[i] = j_year[i] + 1988; break;
      }

      Console.Write("{0}{1:d2}年  {2:d4}年\n", era[i], j_year[i], year[i]);
    }
  }
}
```


```console
和暦      西暦
昭和33年  1958年
大正12年  1923年
明治20年  1883年
平成10年  1998年
昭和54年  1979年
```



## <a id="sec-generated-title-4"></a> <a id="value"></a>列挙型の値

列挙型はプログラムの内部では整数として扱われていて、
整数型に変換することでその値を取り出すことが出来ます。
特に値や型を指定しなければ、列挙型は <code>int</code> として扱われ、
各メンバーは宣言した順番に 0, 1, 2, …, n となります。

例えば以下のような列挙型を定義すると、
<code>Mon, Tue, Wed, Thu, Fri, Sat, Sun</code>
の値はそれぞれ 0, 1, 2, 3, 4, 5, 6 になります。

```csharp {title="曜日をあらわす列挙型"}
enum DayOfWeek
{
  Mon, Tue, Wed, Thu, Fri, Sat, Sun
}
```


列挙型の型や値は以下のようにすることで明示的に指定することも出来ます。

```csharp {title="列挙型の型と値の指定"}
enum 列挙型名 : 内部的な型
{
  メンバー1 = メンバー1の値,
  メンバー2 = メンバー2の値,
   …,
  メンバーn = メンバーnの値
}
```


また、1つ目のメンバーだけに値を指定すると、残りのメンバーの値は1つ目のメンバーの値から1ずつ増加した値になります。

例えば、<code>byte</code> 型で、値が1から始まる列挙型を定義したければ以下のようにします。

```csharp {title="列挙型の型と値を指定する例" highlight-ranges="1:11-1:18,3:10-3:14"}
enum Month : byte
{
  January = 1, February, March, April,
  May, June, July, August,
  September, October, November, December
}

class EnumSample
{
  static void Main()
  {
    for(int i=1; i<12; ++i)
      Console.Write("{0}月  {1}\n", i, (Month)i);
  }
}
```


```console
1月  January
2月  February
3月  March
4月  April
5月  May
6月  June
7月  July
8月  August
9月  September
10月  October
11月  November
```


ちなみに、この例から分かるように、
列挙型を文字列化（ToString）すると、列挙型のメンバー名が表示されます。


## <a id="sec-generated-title-5"></a> <a id="flag"></a>フラグ

ときには、以下のような定数を定義したい場合もあります。

* 条件が n 個ある（例えば X, Y, Z の3つ）

* 「X かつ Y」とか「Y かつ Z」というような条件もありうる


こういう場合、列挙型を以下のように使って実現したりします。

```csharp {title="フラグとしての列挙体"}
enum Xyz
{
  X = 1, // 001
  Y = 2, // 010
  Z = 4, // 100
}

class Program
{
  static void Main(string[] args)
  {
    Xyz xy = Xyz.X | Xyz.Y; // 011
    ...
  }
```


列挙型の値を2の累乗にして、OR 演算をとります。

ただし、このままだと、Console.Write を使って表示するときに少し困ります。
以下の例の場合、X | Y は 3 になるわけですが、値が3のメンバーは Xyz 列挙型には定義されていないので、
表示結果は数値の3がそのまま表示されます。

```csharp {title="X | Y"}
enum Xyz
{
  X = 1, // 001
  Y = 2, // 010
  Z = 4, // 100
}

class Program
{
  static void Main(string[] args)
  {
    Console.Write("{0}\n", Xyz.X);
    Console.Write("{0}\n", Xyz.Y);
    Console.Write("{0}\n", Xyz.Z);

    Xyz xy = Xyz.X | Xyz.Y;
    Console.Write("{0}\n", xy);
  }
}
```


```console {title="結果"}
X
Y
Z
3
```


これに対して、列挙型に Flags 属性を付けると、以下のような表示結果が得られるようになります。
（属性に関しては「[属性](../dynamic/sp_attribute.md)」を参照。）

```csharp {title="Flags 属性を付ける"}
[Flags]
enum Xyz
{
  X = 1, // 001
  Y = 2, // 010
  Z = 4, // 100
}

class Program
{
  static void Main(string[] args)
  {
    Xyz xy = Xyz.X | Xyz.Y;
    Console.Write("{0}\n", xy);

    Xyz yz = Xyz.Y | Xyz.Z;
    Console.Write("{0}\n", yz);

    Xyz zx = Xyz.Z | Xyz.X;
    Console.Write("{0}\n", zx);

    Xyz xyz = Xyz.X | Xyz.Y | Xyz.Z;
    Console.Write("{0}\n", xyz);
  }
}
```


```console {title="結果"}
X, Y
Y, Z
X, Z
X, Y, Z
```



## <a id="sec-generated-title-6"></a> <a id="plan"></a>追加予定

System.Enum クラスから派生。
特別扱い（他の値型同様）。

いくつかメソッド紹介。
インスタンス: ToString, HasFlag。
静的: IsDefined, GetName, GetNames, GetValues, TryParse
