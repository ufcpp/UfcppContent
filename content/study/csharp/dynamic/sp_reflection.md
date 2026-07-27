---
title: "実行時型情報"
source_url: "https://ufcpp.net/study/csharp/dynamic/sp_reflection/"
content_type: "Article"
published_at: "2015-05-06T14:11:35"
updated_at: "2016-05-08T03:27:39"
tags: []
umbraco_id: 1313
parent_id: 1312
sort_order: 0
aliases:
  - "/study/csharp/sp_reflection.html"
---

# 実行時型情報

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

クラスを他のプログラムから利用できるようにするため、
プログラムやライブラリ中にはクラス名やメンバー名、それらのアクセスレベル等の情報が格納されています。
これらの情報は<strong id="metadata" class="keyword">メタデータ</strong>と呼ばれ、
プログラムの実行時にメタデータを取り出すための機能を<strong id="reflection" class="keyword">リフレクション</strong>（reflection）と呼びます。

(プログラムが自分自身の情報を調べることができる機能なので、reflection(鏡映、反射)と呼ぶわけです。)

さらに、C# ではクラスのインスタンスから実行時に型情報を取得したり、
リフレクション機能を利用して型情報からメタデータを取り出したりする機能が用意されています。
このようにして実行時に得られる型に関する情報を<strong id="rtti" class="keyword">実行時型情報</strong>（runtime type information）といいます。


##### <a id="sec-generated-title-2"></a>ポイント

* 構造体/クラス名やメンバー名などの情報は、プログラムを実行するだけなら不要な情報です。

* ですが、C# には、クラス名やメンバー名などの情報を実行時に取り出したり、 あるいは、クラス名の文字列からクラスのインスタンスを動的に生成したりする機能（リフレクション）があります。



## <a id="sec-generated-title-3"></a> <a id="gettype"></a>実行時型情報とは

実は、プログラムを実行するだけなら、実行時型情報は不要な情報です。

例えば、以下のような構造体を考えます。

```csharp
struct Rect
{
  public int Width;
  public int Height;
}
```


で、以下のように、Rect 構造体のメンバーにアクセスすることを考えます。

```csharp
Rect x = new Rect();
x.Width = 3;
x.Height = 4;
```


で、Rect 構造体は、コンピュータ内部では、表1のようなレイアウトになっています。
（実際にどうなるかは環境依存なんですが、32ビット CPU の場合は大体表1のようになります。）

<table summary="Rect 構造体のレイアウト">
	<caption>
		Rect 構造体のレイアウト
	</caption>
	<tr>
		<th>オフセット</th>
		<th>メンバー名</th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1" rowspan="4">Width</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<td markdown="1">4</td>
		<td markdown="1" rowspan="4">Height</td>
	</tr>
	<tr>
		<td markdown="1">5</td>
	</tr>
	<tr>
		<td markdown="1">6</td>
	</tr>
	<tr>
		<td markdown="1">7</td>
	</tr>
</table>


で、実行時には、Width とか Height とかのメンバー名を知る必要はなく、
このレイアウトさえ分かっていればメンバーにアクセスできます。
x.Width にアクセスしたければ変数 x の格納されている場所の先頭を、
x.Height ならば x から4バイト目を見ればいいことになります。

要するに、Rect 構造体のメンバーへのアクセスは、
実行時には、以下のような（C 言語風の）コードと同じような扱いになっています。

```csharp
// Rect x;
char x[8];

// x.Width = 3;
*((int *)(x + 0)) = 3;

// y.Height = 4;
*((int *)(x + 4)) = 4;
```


実行時型情報をサポートしない言語では、
実行時に不要な
「Rect 構造体は Width とか Height という名前のメンバーを持っている」
というような情報は削除されてしまいます。

一方、C# などのような、実行時型情報をサポートする言語では、
実行には直接不要ではあっても、
「Rect 構造体は Width とか Height という名前のメンバーを持っている」
という情報を持っていて、
実行時にこういった情報を引き出せるようになっています。


## <a id="sec-generated-title-4"></a> <a id="type"></a>実行時型情報の取得

C# では、System.Type クラスというものを使って実行時型情報の取得できます。

例として、以下のようなコードを考えてみます。
（あんまり意味のあるコードではないですけども、例ということで。）

```csharp
Rect x = new Rect();
x.Width = 3;
x.Height = 4;
int w = x.Width;
int h = x.Height;
int area = w * h;

Console.Write("{0} × {1} ＝ {2}\n", x.Width, x.Height, area);
```


これを、リフレクション（実行時型情報の取得）機能を使って同じことをしようと思うと以下のようになります。

```csharp
Type t = Type.GetType("Rect");

object o = Activator.CreateInstance(t);

t.GetField("Width").SetValue(o, 3);
t.GetField("Height").SetValue(o, 4);

int w = (int)t.GetField("Width").GetValue(o);
int h = (int)t.GetField("Height").GetValue(o);
int area = w * h;

Rect x = (Rect)o;
Console.Write("{0} × {1} ＝ {2}\n", x.Width, x.Height, area);
```


見てのとおり、型名 Rect も、メンバー名 Width, Height も、文字列になっています。
実行時に、文字列からインスタンスを動的に生成しています。

この例では Type を文字列から生成していますが、
「[多態性](../oop/oo_polymorphism.md)」のところで説明したように、
GetType() メソッドや typeof 演算子を用いることでも Type型のインスタンスを取得することが出来ます。

リフレクション機能を利用するためのクラスは、
この例にも出てきた Type 型、Activator 型の他にもいろいろあって、
主に System.Reflection 名前空間内に定義されています。
例えば、上記の例の、t.GetField() メソッドの戻り値は System.Reflection.FieldInfo 型です。


##### <a id="sec-generated-title-5"></a>ポインター版

参考までに、
逆に 「[unsafe](../interop/sp_unsafe.md#unsafe)」 機能・ポインターを使って書くと、
以下のようになります。

```csharp
// 環境依存だし、ほんとはこんなコード書いちゃ駄目

byte* p = stackalloc byte[sizeof(Rect)];
*(int*)(p + 0) = 3;
*(int*)(p + 4) = 4;
int w = *(int*)(p + 0);
int h = *(int*)(p + 4);
int area = w * h;

Rect x = *(Rect*)p;
Console.Write("{0} × {1} ＝ {2}\n", x.Width, x.Height, area);
```



##### <a id="sec-generated-title-6"></a>実行速度

リフレクション機能を使うと、
例えば、テキスト形式で書かれた設定ファイルを読み込んで、
動的にインスタンスを生成したりといった面白いこともできるんですが、
実行速度は圧倒的に遅くなります。
例えば、ここで例示したようなコードの場合、
リフレクション版は通常版の数千倍くらい低速です。


##### <a id="sec-generated-title-7"></a>サンプル

リフレクションを使って、XML ファイルから動的に（実行時に）インスタンスを生成する簡単なプログラムを作りました。


[ソース一式](../../../../assets/media/ufcpp2000/csharp/source/Reflection.zip)


以下に、利用例をあげます。

```csharp
using System;
using System.Xml.Linq;

namespace Reflection
{
  class Program
  {
    static void Main(string[] args)
    {
      Load(
@"
<Triangle xmlns='Shape'>
  <P1><Point><X>0</X><Y>0</Y></Point></P1>
  <P2><Point><X>1</X><Y>0</Y></Point></P2>
  <P3><Point><X>0</X><Y>2</Y></Point></P3>
</Triangle>
");

      Load(
@"
<Rectangle xmlns='Shape'>
  <Width>3</Width>
  <Height>4</Height>
</Rectangle>
");

      Load(
@"
<Circle xmlns='Shape'>
  <Radius>2</Radius>
</Circle>
");
    }

    static void Load(string xml)
    {
      var doc = XDocument.Parse(xml);
      var p = (Shape.IShape)Loader.LoadFromXml(doc);
      Console.Write("{0}, {1}\n", p, p.GetArea());
    }
  }
}
```
