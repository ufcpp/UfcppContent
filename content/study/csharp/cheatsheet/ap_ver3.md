---
title: "C# 3.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver3/"
content_type: "Article"
published_at: "2015-05-06T14:06:43"
updated_at: "2025-01-01T18:43:41"
tags:
  - "Ver. 3.0"
umbraco_id: 1179
parent_id: 1174
sort_order: 5
aliases:
  - "/study/csharp/ap_ver3.html"
---

# C# 3.0 の新機能

## <a id="sec-generated-title-1"></a> <a id="ver3"></a>C# 3.0

<div class="version version3">Ver. 3.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2007/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2008</li>
<li>.NET Framework 3.5</li>
<li>Visual Basic 9</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>LINQ</li>
</ul>
</td>
</tr>
</table>

2005年9月、C# 2.0 の正式出荷を目前にして、
C# の次世代拡張 C# 3.0 の言語仕様が公開されました。

C# 3.0（そして、同時に発表された VB 9.0）の目玉となる機能は、
Language Integrated Query、略して LINQ と呼ばれるもので、
リレーショナルデータベースや XML に対する操作をプログラミング言語に統合するものです。
（データベースや XML 操作用のライブラリと、
プログラミング言語中にSQL 風の問い合わせ構文を埋め込めるようにする言語拡張から成ります。）

C# 3.0 に追加された機能の多くは、基本的にこの LINQ を使うために必要な機能、
あるいは、より便利に LINQ を使うための機能になります。

ちなみに、詳しくは「[小さな機能の組み合わせ](../data/datamodulararchitecture.md)」で説明していますが、
LINQという大きな目的を、小さな機能の組み合わせで実現しています。


## <a id="sec-generated-title-2"></a> <a id="functional"></a>関数型言語・動的言語的機能

C# 3.0 の新機能には、関数型言語や動的言語が由来と思われる機能がいくつかあります。

といっても、C# を関数型言語や動的言語にしようという話ではなくて、
後述する LINQ のために使えそうな機能を輸入したという感じです。

詳細は「[関数型言語・動的言語的な機能](../functional/sp3_functional.md)」で説明します。


##### <a id="sec-generated-title-3"></a>暗黙的型付け

var キーワードを用いて、
暗黙的に型付けされたローカル変数（Implicitly typed local variables）を定義できるようになりました。

```csharp
var n = 1;
var x = 1.0;
var s = "test";
```



##### <a id="sec-generated-title-4"></a> <a id="extension"></a>拡張メソッド

以下のような構文で、
クラスやインターフェースに対してインスタンスメソッドを擬似的に追加できるようになりました。

```csharp
static class StringExtensions
{
  public static string ToggleCase(this string s)
  中身省略
}
```


このようにして定義したメソッドは、
通常通り、静的メソッドとして呼び出すこともできますが、
あたかも string 型のインスタンスメソッドであるかのように呼び出せるようになります。

```csharp
string s = "This Is a Test String.";
string s1 = StringExtensions.ToggleCase(s); // 通常の呼び出し方。
string s1 = s.ToggleCase();                 // 拡張メソッド呼び出し。
```



##### <a id="sec-generated-title-5"></a>ラムダ式

関数型言語でよく使うような記法で匿名メソッドを定義できるようになりました。
この機能をラムダ式と呼びます。

```csharp
Func<int, bool> p = n => n > 0;
```


この式は、以下のような匿名メソッド同じ意味になります。

```csharp
delegate(int n)
{
  return n > 0;
}
```


さらに、ラムダ式は式木データとしても利用可能です。
ラムダ式をデリゲートに代入すると匿名メソッド（実行コード）として、
Expression 型に代入すると式木データとしてコンパイルされます。

```csharp
Expression<Func<int, bool>> e = n => n > 0;
BinaryExpression lt = (BinaryExpression)e.Body;
ParameterExpression en = (ParameterExpression)lt.Left;
ConstantExpression zero = (ConstantExpression)lt.Right;
```



##### <a id="sec-generated-title-6"></a>初期化子

オブジェクトの初期化を以下のような記法でできるようになりました。

```csharp
Point p = new Point{ X = 0, Y = 1 };
```


ちなみに、このコードは以下のようなコードと等価です。

```csharp
Point p = new Point();
p.X = 0;
p.Y = 1;
```


また、コレクションの初期化を以下のような記法でできるようになりました。

```csharp
List<int> list = new List<int> {1, 2, 3};
```


こちらは以下のようなコードと等価です。

```csharp
List<int> list = new List<int>();
list.Add(1);
list.Add(2);
list.Add(3);
```



##### <a id="sec-generated-title-7"></a>匿名型

匿名型（anonymous type）を作成できるようになりました。
匿名型の作り方は以下の通りです。

```csharp
var x = new { FamilyName = "糸色", FirstName="望"};
```



##### <a id="sec-generated-title-8"></a>暗黙型付け配列

new で配列を作成する際、
型を省略できるようになりました。

```csharp
int[] array = new[] {1, 2, 3, 4};
```



## <a id="sec-generated-title-9"></a> <a id="linq"></a>LINQ

C# 3.0 の目玉となる新機能は、
Language Integrated Query、略して LINQ と呼ばれるもので、
リレーショナルデータベースや XML に対する操作をプログラミング言語に統合するものです。

LINQ の導入により、以下のような利点があります。

* オブジェクト指向言語らしい書き方でデータベースへの問い合わせができます。

* in-memory なオブジェクト、XML、リレーショナルデータベースに対して、同じ文法でデータの問い合わせができます。

* 問い合わせ時に、コンパイラによる文法チェックや、IntelliSense のようなツールの補助を受けることができます。


要するに、LINQ とは、
C# 等の言語に SQL ライクなデータベース操作構文を組み込む
（＋ データベースや XML 操作用のライブラリ）
というものです。
例えば、以下のような書き方ができます。

```csharp
var 学生名簿 =
new[] {
  new {学生番号 = 14, 姓 = "風浦", 名 = "可符香"},
  new {学生番号 = 20, 姓 = "小森", 名 = "霧"    },
  new {学生番号 = 22, 姓 = "常月", 名 = "まとい"},
  new {学生番号 = 19, 姓 = "小節", 名 = "あびる"},
  new {学生番号 = 18, 姓 = "木村", 名 = "カエレ"},
  new {学生番号 = 16, 姓 = "音無", 名 = "芽留"  },
  new {学生番号 = 17, 姓 = "木津", 名 = "千里"  },
  new {学生番号 =  8, 姓 = "関内", 名 = "マリア"},
  new {学生番号 = 28, 姓 = "日塔", 名 = "奈美"  },
};

var 学籍番号前半名 =
  from p in 学生名簿
  where p.学生番号 <= 15
  orderby p.学生番号
  select p.名;

foreach(var 名 in 学籍番号前半名)
{
  Console.Write("{0}\n", 名);
}
```


```console
マリア
可符香
```


詳細は「[LINQ](../data/sp3_linq.md)」と「[標準クエリ演算子（クエリ式関係）](../data/sp3_stdquery.md)」で説明します。


## <a id="sec-generated-title-10"></a> <a id="etc"></a>その他

LINQ 関連の機能以外に、
後からさらに追加された新機能があるようです。


### <a id="sec-generated-title-11"></a> <a id="auto"></a>自動プロパティ

「[イベント](../functional/sp_event.md#event)」は、
「[デリゲート](../functional/sp_delegate.md#delegate)」に対する「[プロパティ](../oop/oo_property.md#property)」のようなものなわけですが、
イベントの場合、
add/remove などの定義部分を省略して書けば、
コンパイラが自動的に add/remove に相当するものを生成してくれていました。

（2006年11月にひっそりと追加されたみたいなんですが）
これに対して、
C# 3.0 では、
プロパティの get/set の中身の省略もできるようになりました。

例えば、

```csharp
public string Name { get; set; }
```


というように、
<code>get; set;</code> とだけ書いておくと、

```csharp
private string __name;
public string Name
{
  get { return this.__name; }
  set { this.__name = value; }
}
```


というようなコードに相当するものが自動的に生成されます。
（<code>__name</code> という変数名はプログラマが参照できるものではありません。）


### <a id="sec-generated-title-12"></a> <a id="partial_method"></a>パーシャルメソッド

もう1つ、
（VS 2008 β2（2007年5月公開）で追加されたみたいなんですが）
<strong id="partial_method" class="keyword">パーシャルメソッド</strong>（partial method）という機能があります。

どういうものかというと、
「[パーシャルクラス](../oop/oo_class.md#partial_class)」内限定で、
メソッドに partial を付けることでメソッドの宣言と定義を分けれるというものです。
（ただし、private メソッド限定。戻り値も void 以外不可。）

例えば、まずクラスの部分定義で以下のようなコードを書いたとします。

```csharp
partial class Program
{
  static void Main(string[] args)
  {
    OnBeginProgram();

    Console.Write("program body\n");

    OnEndProgram();
  }

  static partial void OnBeginProgram();
  static partial void OnEndProgram();
}
```


この状態でプログラムをコンパイル → 実行すると、「program body」の文字だけが表示されます。

ここで、以下のような部分定義を追加して、
パーシャルメソッドに実装を与えます。

```csharp
partial class Program
{
  static partial void OnBeginProgram()
  {
    Console.Write("check pre-condition\n");
  }

  static partial void OnEndProgram()
  {
    Console.Write("check post-condition\n");
  }
}
```


すると、OnBeginProgram、OnEndProgram が呼ばれるようになります。
実行結果は以下の通り。

```console
check pre-condition
program body
check post-condition
```


利用場面としては、
宣言側は人手で書いて、
定義側はツールで自動生成というようなものを想定しているようです。

「[メソッドの実装の分離](../oop/oo_class.md#partial_method)」にもう少し詳しく書いていますが、
この機能は制限が多くて利用場面が限られていますし、
あまり好ましくない副作用もあります。
あくまで、人手での記述とツールでの自動生成の混在開発で使うものだと思ったほうがいいです。


## <a id="sec-generated-title-13"></a> <a id="summary"></a>まとめ

C# 3.0 には LINQ（Language Integrated Query）に関連した新機能がいくつか追加されました。
大別すると、以下のような機能です。

* SQL ライクな問い合わせ構文の統合

* 関数型言語・動的言語的機能
    * 型の省略・推論機構

    * 拡張メソッド

    * ラムダ式

C# 3.0 は、
問い合わせ構文の統合により、
データベースとオブジェクトの間の溝を埋めてくれます。
すなわち、
オブジェクト指向プログラミングにより実現される多彩なデータ構造を使って、
データベースの構築・問い合わせが可能になります。

ただし、
C# 3.0 では、言語そのものがデータベースへのアクセス機能を持ったわけではなく、
問い合わせ構文を適当なメソッド/拡張メソッド呼び出しに変換することで実現しています。
逆に言うと、適切なメソッドさえ定義されていれば、
何でも問い合わせ構文の対象となりえます。
SQL Server などのデータベースサーバーに対しても、
配列などで確保したメモリ上のデータに対しても、
また、XML データに対しても、
全く同じ構文で問い合わせが可能です。

型の省略・推論機構は、
型名を省略（あるいは var キーワードだけ記述）しても、
コンパイラが自動で適切な型を選択してくれるというものです。
これは、C# が動的型付け言語や型付けのゆるい言語になったということではありません。
型推論機構の導入によって、
C# の厳格な型付けを守ったまま、
型付けのゆるい言語の利便性に歩み寄ったものといえます。

ラムダ式などの機能追加も、
C# が関数型言語になったというわけではなく、
関数型言語から便利そうな構文をいくつか拝借したというものです。
