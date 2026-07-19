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
  - "/csharp/dynamic/sp4_dynamic/"
  - "/csharp/sp4_dynamic"
  - "/csharp/sp4_dynamic.html"
  - "/study/csharp/sp4_dynamic"
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

<pre class="source" title="dynamic 型" lang="">
<code><span class="reserved">var</span> sx = 1;     <span class="comment">// sx の型は int 型</span>
<span class="reserved">dynamic</span> dx = 1; <span class="comment">// dx の型は dynamic 型</span>
</code></pre>


通常、C# （3.0 以前）のような静的型付け言語では、
オブジェクトがどういう名前のプロパティやメソッドを持っているかをコンパイル時に知っておく必要があります。

例えば、以下のようなコードを書くと、
「'object' に 'X' の定義が含まれていません」というようなエラーが生じます。

<pre class="source" title="object 型には X というプロパティはありません" lang="">
<code><span class="reserved">static object</span> GetX(<span class="reserved">object</span> obj)
{
  <span class="reserved">return</span> obj.X;
}
</code></pre>


実際に obj 変数に何の型が入っているかには関係なく、キャストで型を変えない限り、
obj は object 型のメンバーにしかアクセスできません。

一方、C# 4.0 では、dynamic 型を使うことで、以下のようなコードが書けるようになりました。

<pre class="source" title="dynamic 型なら、" lang="">
<code><span class="reserved">static dynamic</span> GetX(<span class="reserved">dynamic</span> obj)
{
  <span class="reserved">return</span> obj.X;
}
</code></pre>


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

<pre class="source" title="lib.cs" lang="">
<code><span class="reserved">public class</span> Calculator
{
    <span class="reserved">public int</span> Add(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { <span class="reserved">return</span> x + y; }
    <span class="reserved">public int</span> Sub(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { <span class="reserved">return</span> x - y; }
    <span class="reserved">public int</span> Mul(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { <span class="reserved">return</span> x * y; }
    <span class="reserved">public int</span> Div(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { <span class="reserved">return</span> x / y; }
}
</code></pre>


<pre class="console" title="lib.cs のコンパイル">
<span class="prompt">&gt; </span>csc /t:library lib.cs
</pre>


このライブラリを使って、以下のようなプログラム（sample.cs）を作ったとします。

<pre class="source" title="sample.cs" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Program
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> calc = <span class="reserved">new</span> Calculator();
        Console.WriteLine(calc.Add(1, 2));
    }
}
</code></pre>


このコードをコンパイルするためには、以下のように、/r オプションで DLL の参照を行う必要があります。

<pre class="console" title="lib.cs のコンパイル">
<span class="prompt">&gt; </span>csc /t:exe /r:lib.dll sample.cs
</pre>


これに対して、遅延バインドというのは、必要になるまで DLL をロードしないことを言います。
3.0 以前の C# では、遅延バインドをしようと思うと、「[リフレクション](sp_reflection.md#reflection)」を使って、
以下のようなまどろっこしい書き方が必要でした。

<pre class="source" title="late.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Reflection;

<span class="reserved">class</span> Program
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> lib = Assembly.LoadWithPartialName(<span class="literal">"lib"</span>);
        <span class="reserved">var</span> type = lib.GetType(<span class="literal">"Calculator"</span>);
        <span class="reserved">var</span> calc = Activator.CreateInstance(type);

        <span class="reserved">var</span> add = type.GetMethod(<span class="literal">"Add"</span>);
        Console.WriteLine(<em>add.Invoke(calc, <span class="reserved">new object</span>[] { 1, 2 })</em>);
    }
}
</code></pre>


こうすると、コンパイル時に lib.dll を参照する必要がなく、
以下のようにしてコンパイル可能です。

<pre class="console" title="lib.cs のコンパイル">
<span class="prompt">&gt; </span>csc /t:exe sample.cs
</pre>


（ただし、実行時に、プログラム本体と同じフォルダに lib.dll が置いてある必要があります。）

3.0 までのリフレクションを使った書き方に対して、
C# 4.0 の dynamic を使うと、add.Invoke の部分を簡素化できて、以下のように書けます。

<pre class="source" title="late.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Reflection;

<span class="reserved">class</span> Program
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> lib = Assembly.LoadWithPartialName(<span class="literal">"lib"</span>);
        <span class="reserved">var</span> type = lib.GetType(<span class="literal">"Calculator"</span>);
        <span class="reserved">dynamic</span> calc = Activator.CreateInstance(type);

        Console.WriteLine(<em>calc.Add(1, 2)</em>);
    }
}
</code></pre>



## <a id="sec-generated-title-6"></a> <a id="dlr"></a>DLR 連携

遅延バインドは、動的言語との連携で特に威力を発揮します。
.NET Framework 4 では、動的言語との連携の仕組みとして、DLR（Dynamic Language Runtime）というライブラリが追加されました。

dynamic 型を使うことで、IronPython などの、DLR 上に実装されたスクリプト言語との連携がやりやすくなります。
以下、Visual Studio 2010 付属のサンプルに含まれる IronPython との連携サンプルから抜粋。

例えば、以下のような Python コードを書いて、
helloworld.py という名前で保存したとします。

<pre class="source" title="helloworld.py （Python で Hello World）" lang="">
<code>def welcome(name):
	return "Hello '" + name + "' from IronPython"      
</code></pre>


この Python コードを呼び出すための C# コードは以下のようになります。

<pre class="source" title="C# から Python コードを呼び出す" lang="">
<code>ScriptRuntime py = Python.CreateRuntime();
<span class="reserved">dynamic</span> helloworld = py.UseFile(<span class="literal">"helloworld.py"</span>);

<span class="reserved">var</span> ret = (<span class="reserved">string</span>)helloworld.welcome(<span class="literal">"ufcpp"</span>);

Console.WriteLine(ret);
</code></pre>


<pre class="console" title="実行結果">
Hello 'ufcpp' from IronPython
</pre>


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

<pre class="source" title="X と Y を持つ構造体" lang="">
<code><span class="reserved">struct</span> Point2D
{
    <span class="reserved">public int</span> X, Y;

    <span class="reserved">public override string</span> ToString()
    {
        <span class="reserved">return string</span>.Format(<span class="literal">"2D: ({0}, {1})"</span>, X, Y);
    }
}

<span class="reserved">struct</span> Point3D
{
    <span class="reserved">public int</span> X, Y, Z;

    <span class="reserved">public override string</span> ToString()
    {
        <span class="reserved">return string</span>.Format(<span class="literal">"3D: ({0}, {1}, {2})"</span>, X, Y, Z);
    }
}
</code></pre>


プロパティ Z は Point3D しか持っていませんが、
X と Y なら両方のクラスが持っています。
また、<code>new { X = 1, Y = 2 }</code> というように匿名型を作っても、
同様に X, Y というプロパティを持つオブジェクトが作れます。

ということで、これらの型を使って以下のようなことができます。

<pre class="source" title="X, Y という名前のメンバーさえ持っていれば型を問わない" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Program
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        Console.WriteLine(Sum(<span class="reserved">new</span> Point2D { X = 1, Y = 2 }));
        Console.WriteLine(Sum(<span class="reserved">new</span> Point3D { X = 1, Y = 2, Z = 3 }));
        Console.WriteLine(Sum(<span class="reserved">new</span> { X = 1, Y = 2 }));
    }

    <span class="reserved">static int</span> Sum(<span class="reserved">dynamic</span> obj)
    {
        <span class="reserved">return</span> (<span class="reserved">int</span>)(obj.X + obj.Y);
    }
}
</code></pre>


<pre class="console" title="実行結果">
3
3
3
</pre>


<pre class="source" title="同じ名前のメンバーを持つ型から値をコピー" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Program
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        Console.WriteLine(Convert(<span class="reserved">new</span> Point3D { X = 1, Y = 2, Z = 3 }));
        Console.WriteLine(Convert(<span class="reserved">new</span> { X = 1, Y = 2 }));
        Console.WriteLine(Convert(<span class="reserved">new</span> { X = 1, Y = 2, Z = 3 }));
    }

    <span class="reserved">static</span> Point2D Convert(<span class="reserved">dynamic</span> obj)
    {
        <span class="reserved">return new</span> Point2D
        {
            X = obj.X,
            Y = obj.Y,
        };
    }
}
</code></pre>


<pre class="console" title="実行結果">
2D: (1, 2)
2D: (1, 2)
2D: (1, 2)
</pre>



## <a id="sec-generated-title-8"></a> <a id="data"></a>データ連携

静的型付け言語では、事前に型の決まっていない（スキーマの緩い）データへのアクセスは苦手です。
この手のデータへのアクセスはダックタイピング的にならざるを得ません。

例えば、スキーマが特に決まっていない XML や JSON など読み書きが少々面倒だったりします。
XML にアクセスするのにも、以下のようなコードが必要になります。

<pre class="source" title="LINQ to XML" lang="">
<code><span class="reserved">var</span> doc = XDocument.Parse(<span class="literal">@"
&lt;Point&gt;
    &lt;X&gt;1&lt;/X&gt;
    &lt;Y&gt;2&lt;/Y&gt;
&lt;/Point&gt;
"</span>);

Console.WriteLine(doc.Element(<span class="literal">"Point"</span>).Element(<span class="literal">"X"</span>).Value);
Console.WriteLine(doc.Element(<span class="literal">"Point"</span>).Element(<span class="literal">"Y"</span>).Value);
</code></pre>


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

<pre class="source" title="dynamic を使った XML の読み出し" lang="">
<code><span class="reserved">dynamic</span> doc = <span class="reserved">new</span> <span class="type">DynamicXml</span>(<span class="type">XDocument</span>.Parse(<span class="literal">@"
&lt;Point&gt;
    &lt;X&gt;1&lt;/X&gt;
    &lt;Y&gt;2&lt;/Y&gt;
&lt;/Point&gt;
"</span>));

<span class="type">Console</span>.WriteLine(<em>doc.X</em>);
<span class="type">Console</span>.WriteLine(<em>doc.Y</em>);
</code></pre>


参考：

* [Creating a dynamic xml reader with C# 4.0](http://tore.vestues.no/2009/01/05/creating-a-dynamic-xml-reader-with-c-40/)

* [DynamicDataTable](http://blogs.msdn.com/curth/archive/2009/05/23/dynamicdatatable-part-1.aspx)



## <a id="sec-generated-title-9"></a> <a id="static"></a>ジェネリクス利用時の静的メソッド呼び出し

C# の「[ジェネリック](../oop/sp2_generics.md#generics)」は、メソッドやプロパティの呼び出しをインターフェースによって行います。
「[[サンプル] ジェネリックな複素数型](../sample/sm_genericop.md)」でも書いていますが、
それで何が問題になるかというと、静的メソッド（特に演算子）が呼べないこと。
例えば、以下のようなコードはどうあがいても実現できません。

<pre class="source" title="ジェネリクスでは普通にやってたら operator を使えない" lang="">
<code>T Sum T (IEnumerable&lt;T&gt; list)
{
    T sum = <span class="reserved">default</span>(T);
    <span class="reserved">foreach</span>(<span class="reserved">var</span> x <span class="reserved">in</span> list)
    sum += x; <span class="comment">// ジェネリック型に対して + は使えない。</span>
    <span class="reserved">return</span> sum;
}
</code></pre>


で、少しキャストとかが必要になりますが、
dynamic を使うと一応、静的メソッド呼び出しが可能になります。

<pre class="source" title="ジェネリクスで operator を使いたい" lang="">
<code>T Sum T (IEnumerable&lt;T&gt; list)
{
    <span class="reserved">dynamic</span> sum = <span class="reserved">default</span>(T);
    <span class="reserved">foreach</span>(<span class="reserved">var</span> x <span class="reserved">in</span> list)
    sum += x; <span class="comment">// ジェネリック型に対して + は使えないけど、1回 dynamic 型に代入すればできる。</span>
    <span class="reserved">return</span> (T)sum;
}
</code></pre>


ただし、dynamic の仕組み上、普通の + 演算子呼び出しと比べると少しパフォーマンスが悪いので、
パフォーマンスが要求される場面での利用には注意が必要です。


## <a id="sec-generated-title-10"></a> <a id="multiple_dispatch"></a>多重ディスパッチ

複数のインスタンスの動的な型情報に基づいて実際に呼び出すメソッドを切り替えることを多重ディスパッチ（multiple dispatch）といいます。
これは要するに、「[仮想メソッド](../oop/oo_polymorphism.md#virtual_method)」 の複数インスタンス版といえます。

dynamic を用いると、多重ディスパッチが割と簡単に実現可能です。
詳しくは「[[雑記] 多重ディスパッチ](sp4_multipledispatch.md)」で解説しています。
