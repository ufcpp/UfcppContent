---
title: "dynamic"
source_url: "https://ufcpp.net/study/csharp/dynamic/sp4_dynamic/"
content_type: "Article"
published_at: "2009-05-24T00:00:00"
updated_at: "2009-06-13T00:00:00"
tags:
  - "Ver. 4.0"
umbraco_id: 1316
parent_id: 1312
sort_order: 3
aliases:
  - "/study/csharp/sp4_dynamic.html"
---

# dynamic

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version4">Ver. 4.0</h5>

.NET Framework 4.0 では、クラスライブラリに Dynamic Language Runtime （DLR）が追加されました。
DLR は、.NET Framework 上で Python や Ruby のような動的言語を動かすためのライブラリで、
これに伴い、C# 4.0 にも、動的言語との連携を強化するための仕組みが追加されました。

C# 4.0 で導入された、動的言語との連携の仕組みの1つが動的型付け変数（dynamic キーワード）です。
動的型付け変数を使うことで、動的な（コンパイル時にメンバー情報がわからない型の）メンバーアクセスが可能になります。


## <a id="sec-generated-title-2"></a> <a id="dynamic"></a>動的型付け変数

dynamic キーワードを使うことで、動的型付け変数を定義できます。
使い方としては、<code>dynamic x;</code> というように、変数宣言の型のところに dynamic キーワードを入れます。
（「dynamic 型」という型が C# に追加されたと考えて OK。）

使い方としては var （C# 3.0 で追加された型推論）と似ています。
しかしながら、あくまで型推論である var と違って、dynamic で宣言した変数の型は「動的型」になります。

```csharp {title="dynamic 型"}
var sx = 1;     // sx の型は int 型
dynamic dx = 1; // dx の型は dynamic 型
```


通常、C# （3.0 以前）のような静的型付け言語では、
オブジェクトがどういう名前のプロパティやメソッドを持っているかをコンパイル時に知っておく必要があります。

例えば、以下のようなコードを書くと、
「'object' に 'X' の定義が含まれていません」というようなエラーが生じます。

```csharp {title="object 型には X というプロパティはありません"}
static object GetX(object obj)
{
  return obj.X;
}
```


実際に obj 変数に何の型が入っているかには関係なく、キャストで型を変えない限り、
obj は object 型のメンバーにしかアクセスできません。

一方、C# 4.0 では、dynamic 型を使うことで、以下のようなコードが書けるようになりました。

```csharp {title="dynamic 型なら、"}
static dynamic GetX(dynamic obj)
{
  return obj.X;
}
```


obj が本当に X という名前のプロパティを持っているかどうかは、
コンパイル時ではなく、実行時に調べられます。


## <a id="sec-generated-title-3"></a> <a id="how"></a>dynamic の仕組み

dynamic の機能は、動的コード生成を使って実現されています（プログラム実行時に新たにコード生成される）。
dynamic 型の変数に格納されたインスタンスの型に応じて、以下のいずれかのコードが生成されます。

* IDynamicObject インターフェースを実装した型の場合、TryGetMember などのメソッド呼び出し

* COM オブジェクトの場合、COM Interop コード

* その他の場合、「[リフレクション](sp_reflection.md#reflection)」を使ってメンバーを持っているかどうか調べて、持っているならそのメンバーにアクセスするコードを生成。


このような動的コード生成は、.NET Framework 4.0 から追加された CallSite というクラスを使って行われています。
詳細については、「[dynamic の内部実装](sp4_callsite.md)」にて説明します。

C# の dynamic は、「型が動的」というよりは、「静的な型に対する動的コード生成」と言った方が正確です。
動的に生成したコードはキャッシュされていて、2度目の呼び出しからはかなり効率よく実行されます。
このような手法はインラインメソッドキャッシュ（inline method cache）と呼ばれています。


## <a id="sec-generated-title-4"></a> <a id="what"></a>dynamic で何ができるか

さて、じゃあ、この dynamic を使っていったい何ができるんでしょうか。
先にキーワードだけ挙げると以下のような感じです。

* 「[遅延バインド](#late_binding)」（late binding）
    * 特に、DLR との連携で有効です →「[DLR 連携](#dlr)」



* 「[ダックタイピング](#ducktype)」（duck typing）
    * 特に、XML や JSON などスキーマの緩いデータとの連携で有効です →「[データ連携](#data)」



* 「[ジェネリクス利用時の静的メソッド呼び出し](#static)」

* 「[多重ディスパッチ](#multiple_dispatch)」



## <a id="sec-generated-title-5"></a> <a id="late_binding"></a>遅延バインド

DLL や COM 内のクラス・関数を（必要になったときに、必要な分だけ）実行時に読み込むことを遅延バインド（late binding）と呼びます。

通常、C# では、ライブラリ（DLL）を利用するには、コンパイル時に「アセンブリの参照」というのをします。
（DLL 中でどういう型が定義されているかを参照する。）
参照した DLL はプログラム本体の起動時に同時にロードされます。
（このような動作をアーリーバインド（early binding）と呼びます。）

例えば、以下のようなライブラリコード（lib.cs）を書いたとします。

```csharp {title="lib.cs"}
public class Calculator
{
    public int Add(int x, int y) { return x + y; }
    public int Sub(int x, int y) { return x - y; }
    public int Mul(int x, int y) { return x * y; }
    public int Div(int x, int y) { return x / y; }
}
```


```console {title="lib.cs のコンパイル"}
> csc /t:library lib.cs
```


このライブラリを使って、以下のようなプログラム（sample.cs）を作ったとします。

```csharp {title="sample.cs"}
using System;

class Program
{
    static void Main(string[] args)
    {
        var calc = new Calculator();
        Console.WriteLine(calc.Add(1, 2));
    }
}
```


このコードをコンパイルするためには、以下のように、/r オプションで DLL の参照を行う必要があります。

```console {title="lib.cs のコンパイル"}
> csc /t:exe /r:lib.dll sample.cs
```


これに対して、遅延バインドというのは、必要になるまで DLL をロードしないことを言います。
3.0 以前の C# では、遅延バインドをしようと思うと、「[リフレクション](sp_reflection.md#reflection)」を使って、
以下のようなまどろっこしい書き方が必要でした。

```csharp {title="late.cs" highlight-text="add.Invoke(calc, new object[] { 1, 2 })"}
using System;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        var lib = Assembly.LoadWithPartialName("lib");
        var type = lib.GetType("Calculator");
        var calc = Activator.CreateInstance(type);

        var add = type.GetMethod("Add");
        Console.WriteLine(add.Invoke(calc, new object[] { 1, 2 }));
    }
}
```


こうすると、コンパイル時に lib.dll を参照する必要がなく、
以下のようにしてコンパイル可能です。

```console {title="lib.cs のコンパイル"}
> csc /t:exe sample.cs
```


（ただし、実行時に、プログラム本体と同じフォルダに lib.dll が置いてある必要があります。）

3.0 までのリフレクションを使った書き方に対して、
C# 4.0 の dynamic を使うと、add.Invoke の部分を簡素化できて、以下のように書けます。

```csharp {title="late.cs" highlight-text="calc.Add(1, 2)"}
using System;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        var lib = Assembly.LoadWithPartialName("lib");
        var type = lib.GetType("Calculator");
        dynamic calc = Activator.CreateInstance(type);

        Console.WriteLine(calc.Add(1, 2));
    }
}
```



## <a id="sec-generated-title-6"></a> <a id="dlr"></a>DLR 連携

遅延バインドは、動的言語との連携で特に威力を発揮します。
.NET Framework 4 では、動的言語との連携の仕組みとして、DLR（Dynamic Language Runtime）というライブラリが追加されました。

dynamic 型を使うことで、IronPython などの、DLR 上に実装されたスクリプト言語との連携がやりやすくなります。
以下、Visual Studio 2010 付属のサンプルに含まれる IronPython との連携サンプルから抜粋。

例えば、以下のような Python コードを書いて、
helloworld.py という名前で保存したとします。

```python {title="helloworld.py （Python で Hello World）"}
def welcome(name):
	return "Hello '" + name + "' from IronPython"      
```


この Python コードを呼び出すための C# コードは以下のようになります。

```csharp {title="C# から Python コードを呼び出す"}
ScriptRuntime py = Python.CreateRuntime();
dynamic helloworld = py.UseFile("helloworld.py");

var ret = (string)helloworld.welcome("ufcpp");

Console.WriteLine(ret);
```


```console {title="実行結果"}
Hello 'ufcpp' from IronPython
```


このコードの実行には、IronPython が必要です。
IronPython をインストールした上で、IronPython.dll などの DLL を参照する必要があります。
（
現時点（2009/6/3）では、
[IronPython for .NET 4.0](http://ironpython.codeplex.com/Release/ProjectReleases.aspx?ReleaseId=27320)
っていうバージョンの IronPython をインストールしないと動きません。
）

Python の仕様では、スクリプトファイルが無名のクラスのような扱いになるみたいで、
UseFile("helloworld.py") の実行結果は welcom というメソッドを1個持ったオブジェクトを返します。


## <a id="sec-generated-title-7"></a> <a id="ducktype"></a>ダックタイピング

同じ名前のメンバーを持っている型ならすべて同列に扱うことを「[ダックタイピング](../appendix/ap_term.md#ducktype)」と呼びます。
（「[インターフェース](../oop/oo_interface.md#interface)」等を実装している必要もなく、単純にメンバー名の一致性だけを見ます。）

通常、C# のような静的型付け言語で、
「このメソッドの引数はこういう名前のメンバーを持っていて欲しい」というのを指定するにはインターフェースを使います。
C# 3.0 以前なら、
「何でもいいから X と Y って名前のプロパティを持ってる型なら全部受け付けたい」って時でも、
わざわざ IPoint なり IVector なり、インターフェースを定義して、それを継承したクラスを作る必要がありました。

これに対して、C# 4.0 では、dynamic 型を使うことでダックタイピングが可能になりました。
すなわち、型やインターフェースを問わず、
同じ名前のプロパティやメソッドを持っているなら何でも同列に扱えます。

例えば、以下のような構造体を用意します。

```csharp {title="X と Y を持つ構造体"}
struct Point2D
{
    public int X, Y;

    public override string ToString()
    {
        return string.Format("2D: ({0}, {1})", X, Y);
    }
}

struct Point3D
{
    public int X, Y, Z;

    public override string ToString()
    {
        return string.Format("3D: ({0}, {1}, {2})", X, Y, Z);
    }
}
```


プロパティ Z は Point3D しか持っていませんが、
X と Y なら両方のクラスが持っています。
また、<code>new { X = 1, Y = 2 }</code> というように匿名型を作っても、
同様に X, Y というプロパティを持つオブジェクトが作れます。

ということで、これらの型を使って以下のようなことができます。

```csharp {title="X, Y という名前のメンバーさえ持っていれば型を問わない"}
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Sum(new Point2D { X = 1, Y = 2 }));
        Console.WriteLine(Sum(new Point3D { X = 1, Y = 2, Z = 3 }));
        Console.WriteLine(Sum(new { X = 1, Y = 2 }));
    }

    static int Sum(dynamic obj)
    {
        return (int)(obj.X + obj.Y);
    }
}
```


```console {title="実行結果"}
3
3
3
```


```csharp {title="同じ名前のメンバーを持つ型から値をコピー"}
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Convert(new Point3D { X = 1, Y = 2, Z = 3 }));
        Console.WriteLine(Convert(new { X = 1, Y = 2 }));
        Console.WriteLine(Convert(new { X = 1, Y = 2, Z = 3 }));
    }

    static Point2D Convert(dynamic obj)
    {
        return new Point2D
        {
            X = obj.X,
            Y = obj.Y,
        };
    }
}
```


```console {title="実行結果"}
2D: (1, 2)
2D: (1, 2)
2D: (1, 2)
```



## <a id="sec-generated-title-8"></a> <a id="data"></a>データ連携

静的型付け言語では、事前に型の決まっていない（スキーマの緩い）データへのアクセスは苦手です。
この手のデータへのアクセスはダックタイピング的にならざるを得ません。

例えば、スキーマが特に決まっていない XML や JSON など読み書きが少々面倒だったりします。
XML にアクセスするのにも、以下のようなコードが必要になります。

```csharp {title="LINQ to XML"}
var doc = XDocument.Parse(@"
<Point>
    <X>1</X>
    <Y>2</Y>
</Point>
");

Console.WriteLine(doc.Element("Point").Element("X").Value);
Console.WriteLine(doc.Element("Point").Element("Y").Value);
```


可能なら <code>doc.Point.X</code> というような形式で要素にアクセスしたいところですが、
C# 3.0 以前ではできないことでした。

せいぜい、「[インデクサー](../oop/oo_indexer.md#indexer)」を使って
<code>doc["Point"]["X"]</code> と書けるようにすることはできますが、
[ とか " とかの入力は意外と手間で嫌になります
（少なくとも僕は [ とか " をフォームポジション崩さずにタイピングできないです。）

dynamic を使うと、
（IDynamicObject インターフェースを継承したクラスを作れば、）
ダックタイピング的に XML の要素にアクセスすることも可能です。
例えば、以下のようなクラスを1つ用意すれば、<code>doc.X</code> という書き方が可能になります。

* 
[DynamicXml.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/DynamicXml.cs)



例えば、上述のコードは以下のような書き直すことができます。

```csharp {title="dynamic を使った XML の読み出し" highlight-ranges="sha256:db684f10741cf7a8bda611b432a4b394f86eef72bc088c003373edbffe2de81e;8:19-8:24,9:19-9:24"}
dynamic doc = new DynamicXml(XDocument.Parse(@"
<Point>
    <X>1</X>
    <Y>2</Y>
</Point>
"));

Console.WriteLine(doc.X);
Console.WriteLine(doc.Y);
```


参考：

* [Creating a dynamic xml reader with C# 4.0](http://tore.vestues.no/2009/01/05/creating-a-dynamic-xml-reader-with-c-40/)

* [DynamicDataTable](http://blogs.msdn.com/curth/archive/2009/05/23/dynamicdatatable-part-1.aspx)



## <a id="sec-generated-title-9"></a> <a id="static"></a>ジェネリクス利用時の静的メソッド呼び出し

C# の「[ジェネリック](../oop/sp2_generics.md#generics)」は、メソッドやプロパティの呼び出しをインターフェースによって行います。
「[[サンプル] ジェネリックな複素数型](../sample/sm_genericop.md)」でも書いていますが、
それで何が問題になるかというと、静的メソッド（特に演算子）が呼べないこと。
例えば、以下のようなコードはどうあがいても実現できません。

```csharp {title="ジェネリクスでは普通にやってたら operator を使えない"}
T Sum T (IEnumerable<T> list)
{
    T sum = default(T);
    foreach(var x in list)
    sum += x; // ジェネリック型に対して + は使えない。
    return sum;
}
```


で、少しキャストとかが必要になりますが、
dynamic を使うと一応、静的メソッド呼び出しが可能になります。

```csharp {title="ジェネリクスで operator を使いたい"}
T Sum T (IEnumerable<T> list)
{
    dynamic sum = default(T);
    foreach(var x in list)
    sum += x; // ジェネリック型に対して + は使えないけど、1回 dynamic 型に代入すればできる。
    return (T)sum;
}
```


ただし、dynamic の仕組み上、普通の + 演算子呼び出しと比べると少しパフォーマンスが悪いので、
パフォーマンスが要求される場面での利用には注意が必要です。


## <a id="sec-generated-title-10"></a> <a id="multiple_dispatch"></a>多重ディスパッチ

複数のインスタンスの動的な型情報に基づいて実際に呼び出すメソッドを切り替えることを多重ディスパッチ（multiple dispatch）といいます。
これは要するに、「[仮想メソッド](../oop/oo_polymorphism.md#virtual_method)」 の複数インスタンス版といえます。

dynamic を用いると、多重ディスパッチが割と簡単に実現可能です。
詳しくは「[[雑記] 多重ディスパッチ](sp4_multipledispatch.md)」で解説しています。
