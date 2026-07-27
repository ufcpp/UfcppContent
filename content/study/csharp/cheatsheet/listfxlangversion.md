---
title: "C#の言語バージョンと.NETバージョン"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/listfxlangversion/"
content_type: "Article"
published_at: "2015-01-04T00:00:00"
updated_at: "2019-10-22T00:00:00"
tags: []
umbraco_id: 1183
parent_id: 1174
sort_order: 21
aliases: []
---

# C#の言語バージョンと.NETバージョン

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C#の言語機能のバージョンと.NET のバージョン([ターゲット フレームワーク](langversionoption.md#targetframework)。実行環境には大して手が入っていないので、おおむね標準ライブラリのバージョンのこと。どのクラス・どのメソッドが使えるか)は、
基本的には独立しています。なので、C#の新機能の多くは古い .NET 上でも動きます。

「多くは」であって、「全て」ではないわけですが。
ここが今日の主題。
どういう機能は、どういう理由で動かないか。
どうやっても動かないのか、それとも、動かしようがあるのか。
ということについて書いていきます。

## <a id="sec-generated-title-2"></a> <a id="background"></a>具体例

例えば、諸事情あってWindows XP (標準インストールの状態で .NET Framework 2.0)から抜けられないという場合でも、Visual Studio 2015を使って、C# 6で開発したアプリを動かすこともできます。

しかし、いくつかの機能は.NETのライブラリに依存しています。
例えば、C# 5.0の[async/await](../async/sp5_async.md)の実行には、
.NET Framework 4で追加された`Task`クラス(`System.Threading.Tasks`名前空間)と、
.NET Framework 4.5で追加された`AsyncTaskMethodBuilder`(`System.Runtime.CompilerServices`名前空間)などの一連のクラスが必要です。

そして、残念なことに .NET Framework 4～4.8 くらいの時代でも、
だいぶ長い間 .NET Framework 3.5は現役でした
(5～10年くらい前の環境が現役稼働することはよくあります)。
一方で、async/awaitの需要は極めて高くて、.NET Framework 3.5の上でも動かしたいという要求は結構あります。

こういう場合でも、.NET 4や4.5で追加された`Task`クラスなどを、.NET 3.5向けに移植(バックポーティング: backporting)してしまうという手を使えば、
古いバージョンの.NET上でもasync/awaitが使えます。

もちろん、機能毎にバックポーティングの難易度が違ってきますので、
両手放しに「どんな機能でも古いバージョンで動く」とは言い難かったりします。

##### <a id="sec-generated-title-3"></a>サンプル

[https://github.com/ufcpp/UfcppSample/tree/master/LanguageAndFrameworkVersion](https://github.com/ufcpp/UfcppSample/tree/master/LanguageAndFrameworkVersion)

## <a id="sec-generated-title-4"></a>C# 8.0 以降の default バージョン

「[言語バージョンの指定](langversionoption.md#new-options)」で説明していますが、
C# 8.0 以降、規定動作(オプション未指定時の動作)では、ターゲット フレームワークに合わせて C# のバージョンが自動選択されるようになっています。

わざわざ古い .NET 上で動くプログラムを最新の C# で書く人というのはそれなりに知識のある人のはずなので、
カジュアルにサポートはされません。


## <a id="sec-generated-title-5"></a> <a id="feature-table"></a>古いバージョンで動くかどうか

まず、バックポーティングなしでも動く機能について。
一覧表で、C# のどの機能がどのバージョンの .NET Framework 上で動くのかを示しましょう。

ちなみに、.NET Framework 1.0/1.1 はもうサポートが切れているので調査対象外です(Visual Studio上でもターゲット フレームワークに選べないので)。
つまり、以下の表で、「2.0で動く」となっているものは実質的に「どこでも動くもの」になります。


### <a id="sec-generated-title-6"></a> <a id="v3"></a>C# 3.0

C# 3.0 = Visual Studio 2008, .NET Framework 3.5 と同時期リリース。Windows 7 よりちょっと前。

<table summary="">

	<tr>
		<th>機能</th>
		<th>どのバージョンで動くか</th>
	</tr>
	<tr>
		<td markdown="1">変数の型推論</td>
		<td markdown="1" rowspan="9">2.0</td>
	</tr>
	<tr>
		<td markdown="1">配列の型推論</td>
	</tr>
	<tr>
		<td markdown="1">オブジェクト初期化子</td>
	</tr>
	<tr>
		<td markdown="1">コレクション初期化子</td>
	</tr>
	<tr>
		<td markdown="1">プロパティの自動実装</td>
	</tr>
	<tr>
		<td markdown="1">ラムダ式</td>
	</tr>
	<tr>
		<td markdown="1">クエリ式†</td>
	</tr>
	<tr>
		<td markdown="1">部分メソッド</td>
	</tr>
	<tr>
		<td markdown="1">匿名型</td>
	</tr>
	<tr>
		<td markdown="1">拡張メソッド</td>
		<td markdown="1">3.5<sup>※</sup></td>
	</tr>
</table>


※ … 2.0で動かすすべあり

† … 補足あり


### <a id="sec-generated-title-7"></a> <a id="v4"></a>C# 4.0

C# 4.0 = Visual Studio 2010, .NET Framework 4 と同時期リリース。

<table summary="">

	<tr>
		<th>機能</th>
		<th>どのバージョンで動くか</th>
	</tr>
	<tr>
		<td markdown="1">引数の規定値と名前付き引数</td>
		<td markdown="1" rowspan="2">2.0</td>
	</tr>
	<tr>
		<td markdown="1">変性(genericのin/out型注釈)†</td>
	</tr>
	<tr>
		<td markdown="1">dynamic</td>
		<td markdown="1">4<sup>×</sup></td>
	</tr>
</table>


† … 補足あり

× … dynamicは古いバージョンの.NET Frameworkで動かすのがほぼ無理臭い唯一の機能かも。


### <a id="sec-generated-title-8"></a> <a id="v5"></a>C# 5.0

C# 5.0 = Visual Studio 2012, .NET Framework 4.5と同時期リリース。Windows 8 ともほぼ同時期。

<table summary="">

	<tr>
		<th>機能</th>
		<th>どのバージョンで動くか</th>
	</tr>
	<tr>
		<td markdown="1">async/await</td>
		<td markdown="1">4.5<sup>※1</sup></td>
	</tr>
	<tr>
		<td markdown="1">Caller Info</td>
		<td markdown="1">4.5<sup>※2</sup></td>
	</tr>
</table>


※1 … 4で動かすすべあり

※2 … 2.0で動かすすべあり

### <a id="sec-generated-title-9"></a> <a id="v6"></a>C# 6

C# 6 = Visual Studio 2015, .NET Framework 4.6と同時期リリース。Windows 10とも同時期。

<table summary="">
	<tr>
		<th>機能</th>
		<th>どのバージョンで動くか</th>
	</tr>
	<tr>
		<td markdown="1">自動実装プロパティに対する初期化子</td>
		<td markdown="1" rowspan="9">2.0</td>
	</tr>
	<tr>
		<td markdown="1">get-onlyの自動実装プロパティ</td>
	</tr>
	<tr>
		<td markdown="1">式形式の関数メンバー定義</td>
	</tr>
	<tr>
		<td markdown="1">using static</td>
	</tr>
	<tr>
		<td markdown="1">null条件演算子</td>
	</tr>
	<tr>
		<td markdown="1">インデックス初期化子</td>
	</tr>
	<tr>
		<td markdown="1">nameof演算子</td>
	</tr>
	<tr>
		<td markdown="1">例外フィルター</td>
	</tr>
	<tr>
		<td markdown="1">構造体の引数なしコンストラクター</td>
	</tr>
	<tr>
		<td markdown="1">文字列補間</td>
		<td markdown="1">2.0<sup>※1</sup></td>
	</tr>
	<tr>
		<td markdown="1">拡張メソッドを使ったコレクション初期化子</td>
		<td markdown="1">3.5<sup>※2</sup></td>
	</tr>
	<tr>
		<td markdown="1">catch句/finally句内でのawait演算子利用</td>
		<td markdown="1">4.5<sup>※3</sup></td>
	</tr>
</table>


※1 … 書き方によっては4.6でないと動かなくなる

※2 … 拡張メソッドの制限そのまま。拡張メソッドを2.0で動かす方法はあるので、それを使えば2.0

※3 … 同様に、await演算子の制限。4で動かすすべあり

### <a id="sec-generated-title-10"></a> <a id="v7"></a>C# 7

C# 7 = Visual Studio 2017と同時期リリース。

<table summary="">
	<tr>
		<th>機能</th>
		<th>どのバージョンで動くか</th>
	</tr>
	<tr>
		<td markdown="1">パターン マッチング(型スイッチ)</td>
		<td markdown="1" rowspan="9">2.0</td>
	</tr>
	<tr>
		<td markdown="1">複合型の分解</td>
	</tr>
	<tr>
		<td markdown="1">出力変数宣言</td>
	</tr>
	<tr>
		<td markdown="1">ローカル関数</td>
	</tr>
	<tr>
		<td markdown="1">参照戻り値</td>
	</tr>
	<tr>
		<td markdown="1">2進数リテラル</td>
	</tr>
	<tr>
		<td markdown="1">数字セパレーター</td>
	</tr>
	<tr>
		<td markdown="1">throw式</td>
	</tr>
	<tr>
		<td markdown="1">式形式の関数メンバー定義(拡充)</td>
	</tr>
	<tr>
		<td markdown="1">一般化非同期戻り値</td>
		<td markdown="1">2.0<sup>※1</sup></td>
	</tr>
	<tr>
		<td markdown="1">タプル型</td>
		<td markdown="1">4.5<sup>※2</sup></td>
	</tr>
</table>

※1 … 仕組み自体は2.0で動く。ただし、この機能の最大の目的である`ValueTask`構造体が使えるのは .NET 4.5以降。

※2 … 2.0で動かすすべあり

## <a id="sec-generated-title-11"></a> <a id="how-to"></a>具体的な方法・補足

前節の表で、2.0で動くとなっているものは、要は、ライブラリ依存がなくて、単純にC#コンパイラーだけの仕事で実現できる機能です。

逆に、特定のバージョンに依存しているものは、そのバージョンで追加されたクラスに依存しています。

しかし、具体例で少し触れた通り、最新のライブラリからバックポーティングしてくれば、
古いバージョンの.NET Frameworkでも最新のC#機能を使えます。
(あるクラスに依存するといっても厳密なチェックをしているわけではなく、同じ名前・同じ機能のクラスを自前で実装すれば動きます。)
一応は、バックポーティングさえすればどの機能でも「動かすすべがある」ということになります。

とは言え、C#機能ごとに、その依存するクラスを実装する大変さが全然違うので、現実的には全部とはいかないでしょう。

### <a id="sec-generated-title-12"></a> <a id="attribute"></a>拡張メソッド、Caller Info

かなり簡単なのがこの2つ。この2つは、単にメソッドや引数に属性が付くだけのものです。

* 拡張メソッド → ExtensionAttribute(System.Runtime.CompilerServices名前空間)

* Caller Info → CallerFilePathAttribute, CallerLineNumberAttribute, CallerMemberNameAttribute(System.Runtime.CompilerServices名前空間)


属性の実装は簡単。例えば以下のようなコードで終わり。

```csharp
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}
```



### <a id="sec-generated-title-13"></a> <a id="query-expression"></a>クエリ式

System.Linq名前空間のクラスがなくて動くの？と一瞬思うかもしれませんが、SelectとかWhereとかを自前実装すれば使えます。そんなに難易度も高くないものなので割とちょろい。
例えば、Select の実装は以下のような感じです(本物と比べるとちょっとだけ手抜き実装。引数の null チェックがない)。

```csharp
using System.Collections.Generic;

namespace System.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<TResult> Select<TSource, TResult>( this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (var item in source)
                yield return selector(item);
        }
    }
}
```


あと、SelectとかWhereは、普通のインスタンス メソッドでも拡張メソッドでもどちらでもOKです。


### <a id="sec-generated-title-14"></a> <a id="variance"></a>変性(genericのin/out型注釈)

インターフェイスやデリゲートを定義する側は何の問題もなく、.NET Framework 3.5以前の上で動くんですが…

残念なの点としては、標準ライブラリ(BCL: Base Class Library)中のクラスにちゃんとした変性注釈がついたのは.NET Framework 4 からということです。
つまり、以下のようなコードを書けるのは、結局、.NET Framework 4 以降のみ。

```csharp
List<string> x = new List<string> { "one", "two", "three" };
IEnumerable<object> y = x;
```


いくら、C# 側が変性に対応したからといって、IEnumerable インターフェイスにその注釈がついていなければ恩恵を受けれません。
(インターフェイスを自作する分には恩恵があるけども、標準ライブラリを使う範囲では 4 以降でないと恩恵がない。)


### <a id="sec-generated-title-15"></a> <a id="dynamic"></a>dynamic

dynamicが使っているのはSystem.Dynamic名前空間以下のクラスなんですが… こいつの自前実装はさすがにちょっと…

実質的に、古い.NET Framework上で動かしようがない唯一の機能かも。もちろん、monoの実装をとってきて使うなどすれば、無理ではないはず。


### <a id="sec-generated-title-16"></a> <a id="async"></a>async/await

これも、.NET Framework 4.5で入ったTaskクラスに対する拡張が必須になります。
自前で実装できる規模も超えていると思います。

ただ、こちらには多少救いがあって、マイクロソフトの.NETチームは、.NET 4と.NET 4.5の差分にあたるものを、NuGetパッケージとして公開してくれています。
その中にはasync/awaitに関するものもあって、Microsoft.Bcl.Asyncという名前のパッケージになっています。
これを使えば、async/awaitを.NET Framework 4の上で動かすことはできます。

しかし、マイクロソフトが公式に提供しているのは4から4.5の間の差分で、3.5以前には対応できません。
一応、async/awaitの実行に必要なクラスのソースコードはオープンになっています。

- マイクロソフトの公式実装: [Reference Source](http://referencesource.microsoft.com/#mscorlib/system/runtime/compilerservices/AsyncMethodBuilder.cs)

この辺りからバックポーティングすればなんとか3.5以前のバージョンにも対応できます。
実際、コミュニティ ベースのバックポーティング プロジェクトがあったりします。

- [Rackspace Threading Library for .NET](https://github.com/rackerlabs/dotnet-threading)

### <a id="sec-generated-title-17"></a> <a id="string-interpolation"></a>string interpolation

(現行の Visual Studio 2015 Preview (2014/11 版)とは文法が変わることが確定しているんですが、新しい方の(2014/11 版では動かない)文法で説明します)
(日本語での呼称も決まってないので string interpolation。文字列補間？文字列挿入？)

文字列補間は、普通に使う分にはただのstring.Formatへの展開なので、.NET Framework 2.0上でも動きます。
例えば、以下のような構文なんですが、

```csharp
var s = $"{x}, {y}";
```


この s はstring型で、この行は以下のように展開されます。

```csharp
var s = string.Format("{0}, {1}", x, y);
```


ただ、この文法だとカルチャー指定付きでのFormatなどができません。
参考: 「[文字列挿入](../start/st_string.md#string-interpolation)」、「[書式とカルチャー](../../dotnet/bcl/bcl_format.md#culture)」。
そこで、別途、以下のように書けるバージョンが追加される予定です(string ではなく、IFormattable 型の変数で受け取る)。

```csharp
IFormattable s = $"{x}, {y}";
```


この場合、以下のように展開されます。

```csharp
IFormattable s = new System.Runtime.CompilerServices.FormattableString("{0}, {1}", x, y);
```


この、`FormattableString`クラスは、.NET Framework 4.6で追加される予定のクラスです。
それほど複雑ではなく、自作は簡単でしょう。以下のような内容です。

```csharp
namespace System.Runtime.CompilerServices
{
    public struct FormattableString : System.IFormattable
    {
        private readonly String format;
        private readonly object[] args;
        public FormattableString(String format, params object[] args)
        {
            this.format = format;
            this.args = args;
        }
        public String Format => this.format;
        public object[] Args => this.args;
        string IFormattable.ToString(string ignored, IFormatProvider formatProvider)
        {
            return String.Format(formatProvider, format, args);
        }
    }
}
```

## <a id="sec-generated-title-18"></a> <a id="ValueTuple"></a>タプル

タプルは、`ValueTuple`構造体(`System`名前空間)と`TupleElementNames`属性(`System.Runtime.CompilerServices`名前空間)という型に依存しています。

これらの型は、.NET 4.5 以降であれば、
[System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/)パッケージを参照することで利用可能です。
(ちなみに、標準ライブラリに含まれるのは .NET 4.7 以降の予定。)

これらの型は[ほんの数ファイルほどで実装されている](https://github.com/dotnet/corefx/tree/master/src/System.ValueTuple/src/System)くらい単純なものなので、.NET 2.0 向けのバックポーティングもそれほど難しくありません。

## <a id="sec-generated-title-19"></a> <a id="closing"></a>最後に

あれこれ書きましたが
まあ、さすがに最近(2014年末)、.NET Framework 2.0 案件の話は聞くこと減って来たかなぁ…

Windows 7/Windows Server 2008 R2の標準インストールでのバージョンが.NET Framework 3.5.1なので、割かし、最低ラインがここ。
また、Windows 8/Windows Server 2012の場合は.NET Framework 4.5。
なので、3.5と4.5の場合だけ大まかに覚えておけばいいと思います。

* .NET Framework 3.5 = Windows 7 … async/awaitとdynamic以外は平気(Caller InfoとValueTupleは、やりよう次第)

* .NET Framework 4.5 = Windows 8 … 全部平気


ということで、本稿の内容は、実質的に、
「Windows Server 2003 R2をまだお使いの方、かつ、サーバー側開発なのになぜか.NET Frameworkのバージョンを2.0から上げれない方」
を対象にした非常にニッチな話な気ということになります。

一応、言語のバージョンとフレームワークのバージョンは独立で、
C# の新機能は結構古い .NET でも動くということをお伝えしたものでした。
