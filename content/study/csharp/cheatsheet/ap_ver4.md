---
title: "C# 4.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver4/"
content_type: "Article"
published_at: "2008-11-14T00:00:00"
updated_at: "2015-02-16T00:00:00"
tags:
  - "Ver. 4.0"
umbraco_id: 1180
parent_id: 1174
sort_order: 6
aliases:
  - "/study/csharp/ap_ver4.html"
---

# C# 4.0 の新機能

## <a id="sec-generated-title-1"></a> <a id="ver4"></a>C# 4.0

<div class="version version4">Ver. 4.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2010/4</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2010</li>
<li>.NET Framework 4</li>
<li>Visual Basic 10</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>dynamic</li>
<li>相互運用</li>
</ul>
</td>
</tr>
</table>


2008年10月、3年ぶりに C# バージョンアップに関する情報が公開されました。

C# 4.0 で追加される機能は以下の3つ。

* 動的型付け変数

* オプション引数・名前付き引数

* ジェネリクスの共変性・反変性


（
注 2008/11/14: 
2008年10月に発表された内容としてはこの3つですが、今後の機能追加に関して。
C# 4.0 CTP の顧客フィードバックサイトの掲示板議論によれば、
「C# 4.0 の仕様は現状で確定ではない。ただし、これから大幅な変更は入れたくはない。」ということらしい。
C# 3.0 の時には、後から追加された機能は自動プロパティとpartialメソッドだけで、
「小さな変更」だったので、今回も同程度の「小さな変更」くらいしか追加されないはず。
）


## <a id="sec-generated-title-2"></a> <a id="dynamic"></a>動的型付け変数

dynamic キーワードを使うことで、動的型付け変数を定義できます。

dynamic 型を使うことで、
コンパイル時に確定しないプロパティアクセス・メソッド呼び出しが可能です。
（スクリプト言語との連携や、DLL の遅延バインディングのために使います。）

使い方としては var （C# 3.0 で追加された型推論）と似ています。
しかしながら、あくまで型推論である var と違って、dynamic で宣言した変数の型は「動的型」になります。

```csharp
var sx = 1;     // sx の型は int 型
dynamic dx = 1; // dx の型は dynamic 型
```


通常、C# （3.0 以前）のような静的型付け言語では、
オブジェクトがどういう名前のプロパティやメソッドを持っているかをコンパイル時に知っておく必要があります。

例えば、以下のようなコードを書くと、
「'object' に 'X' の定義が含まれていません」というようなエラーが生じます。

```csharp
static object GetX(object obj)
{
  return obj.X;
}
```


object 型が X という名前のプロパティを持っていないので、静的言語の世界ではエラーが出て当たり前。

一方、C# 4.0 では、dynamic 型を使うことで、以下のようなコードが書けるようになりました。

```csharp
static dynamic GetX(dynamic obj)
{
  return obj.X;
}
```


obj が本当に X という名前のプロパティを持っているかどうかは、
コンパイル時ではなく、実行時に調べられます。

詳細は「[dynamic](../dynamic/sp4_dynamic.md)」で説明します。


## <a id="sec-generated-title-3"></a> <a id="optional"></a>オプション引数・名前付き引数

C# 4.0 で、C++ や VB にあるような、オプション引数と名前付き引数が追加されました。

まず、以下のように規定値(default value)を持ったメソッドを定義します。

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
この例でいうところの、x や y だけを省略することはできません。

で、もう1つ、
名前付き引数（named parameter）が使えるようになりました。

先ほど定義した規定値を持つメソッドを、以下のような構文で呼び出せます。

```csharp
int s1 = Sum(x: 1, y: 2, z: 3); // Sum(1, 2, 3); と同じ意味。
int s2 = Sum(y: 1, z: 2, x: 3); // Sum(3, 1, 2); と同じ意味。
int s3 = Sum(y: 1);             // Sum(0, 1, 0); と同じ意味。
```


名前付き引数の場合、引数の順序は自由に書けます。
また、任意の箇所を省略可能になります。

詳細は「[オプション引数・名前付き引数](../structured/sp4_optional.md)」で説明します。


## <a id="sec-generated-title-4"></a> <a id="variance"></a>ジェネリックの共変性・反変性

C# 4.0 で、ジェネリクスの型引数に共変性・反変性を持たせることが可能になりました。
（共変性・反変性という言葉の意味は「[covariance と contravariance](../functional/sp_delegate.md#co-contra)」参照。）

ジェネリクスの共変性・反変性実現のために、ジェネリクスの型引数に対して in/out を修飾子を指定します。

出力（戻り値、get）でしか使わない型には out という修飾子を付けることで、共変性が認められます。

```csharp
public interface IEnumerable<out T> { ... }
```


```csharp
IEnumerable<string> strings = new[] {"aa", "bb", "cc"};
IEnumerable<object> objs = strings;
// foreach (object x in strings) ってやっても問題ないんだから、
// objs に strings を代入しても OK。
```


一方、入力（引数、set）でしか使わない型には in という修飾子を付けることで、反変性が認められます。

```csharp
public delegate void Action<in T> (T arg);
```


```csharp
Action<object> objAction = x => { Console.Write(x); };
Action<string> strAction = objAction;
// objAction("string"); ってやっても問題ないんだから、
// strAction に objAction を代入しても OK。
```


詳細は「[ジェネリクスの共変性・反変性](../oop/sp4_variance.md)」で説明します。


## <a id="sec-generated-title-5"></a> <a id="ComInterop"></a>COM 相互運用時の特別処理

.NET Framework には COM 相互運用機能があって、COM のクラスをあたかも .NET のクラスであるかのように扱うことができます。
ただ、COM が主流だった時代と今とでは大分設計思想に差があって、
.NET 的には不要だけども、COM 相互運用をする上では欲しい機能というのがいくつかあります。

そこで、C# 4.0 では、COM 相互運用用のクラス
（Runtime Callable Wrapper といいます。.NET ランタイムから COM を呼び出せるようにしたラッパークラス）に対してだけ特別な処理をするようになりました。
COM への特別処理は以下の2点。

* ref 引数（「[引数の参照渡し](../resource/sp_ref.md)」参照）に対して、ref キーワードを付けなくても呼び出せるようになった。

* <code>get_X(index)</code>、<code>set_X(index, value)</code>というメソッドに対して、 インデックス付きプロパティ構文（<code>X[index]</code>という書き方）が使えるようになった。


ちなみに、前者に関しては、「引数の参照渡しでは、呼び出し側からも参照渡しであることが一目でわかるべき」
というのが C# の流儀なので、ref キーワードの省略はあまりいい構文ではありません。
ですが、COM の場合、参照渡しにする必要のないようなものにまでやたらと ref が付きまくるので、
やむなく ref キーワードの省略を認めるようです。

後者に関しても、C# は「インデックス付きプロパティじゃなくて、インデクサー持ちの型のプロパティを作れ」という設計思想です。
COM の時代にはそういう思想がなくて、インデックス付きプロパティだらけなので、これもやむなく認めるようになりました。
（あくまで、COM クラスの get_X() を X[] で参照できるだけ。
C# でインデックス付きプロパティが定義できるようになるわけではない。
それどころか、VB.NET で作ったインデックス付きプロパティは、C# からは get_X という書き方をする必要があります。）
