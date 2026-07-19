---
title: "ライブラリ"
source_url: "https://ufcpp.net/study/csharp/structured/st_library/"
content_type: "Article"
published_at: "2015-05-06T14:08:59"
updated_at: "2020-09-13T11:35:44"
tags: []
umbraco_id: 1240
parent_id: 1217
sort_order: 12
aliases:
  - "/csharp/st_library"
  - "/csharp/st_library.html"
  - "/csharp/structured/st_library/"
  - "/study/csharp/st_library"
  - "/study/csharp/st_library.html"
---

# ライブラリ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="library" class="keyword">ライブラリ</strong>(library)とは、一言で言うと便利な機能をまとめておいて、他のプログラムから呼び出せる形にしたものです。

.NET framework SDK をインストールすると、コンパイラと一緒にさまざまなライブラリが初めからインストールされます。
C# に限らず、このようにコンパイラとセットで必ず提供されるライブラリのことを<strong id="stdlib" class="keyword">標準ライブラリ</strong>(standard library)などと呼んだりもします。

自分でライブラリを作成することも出来るのですが、ライブラリの自作や、自作したライブラリの利用方法は後ほど説明することにして、ここでは標準ライブラリについて少し説明したいと思います。


##### <a id="sec-generated-title-2"></a>ポイント

* ライブラリ: よくつかわれる機能をひとまとめにしたもの

* C# （というか、.NET Framework）には標準で多種多様なライブラリが付属します



## <a id="sec-generated-title-3"></a> <a id="lib"></a>.NET framework の標準ライブラリ

「[.NET Framework とは](../abstract/ab_dotnet.md)」で説明したように、
.NET Framework では、.NET Framework 上に実装された言語すべてから呼び出せるような共通ライブラリが用意されています。

.NET Frameworkの標準ライブラリの内容をここですべて説明するわけには行きませんので、
ライブラリの簡単な利用方法と、今後このページのサンプルで利用しそうな機能に焦点を当てて説明していきます。


##### <a id="sec-generated-title-4"></a>標準ライブラリの利用方法

C# ではライブラリを利用する際、プログラムのソースには特に何も手を加える必要はありません。
(C言語のように <code>#include</code> でヘッダーファイルを読み込む必要はないし、Java のように <code>import</code> も行う必要はない。)
ライブラリは、コンパイラに対して <code>/r</code> オプションでライブラリの入っている DLL ファイルを指定するだけで利用できます。
（Visual Studio を使う場合には、
ソリューションエクスプローラーに「参照」という項目が表示されているはずなので、
それを右クリックして「参照の追加」を行います。）

また、標準ライブラリの中でも特によく利用されるものに関しては、
自動的に参照先の DLL ファイルを見つける設定がなされているので <code>/r</code> オプションを指定する必要もありません。


##### <a id="sec-generated-title-5"></a>クラスライブラリ

.NET Framework の標準ライブラリはすべてクラス化されています。
クラスに関しては「[クラス](../oop/oo_class.md)」で解説します。


##### <a id="sec-generated-title-6"></a>名前空間

「[名前空間](sp_namespace.md)」で説明しますが、C# の標準ライブラリはすべて名前空間によって分類されています。
例えば、数学関連の機能を利用するためには <code>Math</code> というクラスを用いますが、
<code>Math</code> クラスは <code>System</code> という名前空間に属しています。
そのため、<code>Math</code> クラスを利用するには、以下のように完全修飾名で書くか、

<pre class="source" title="完全修飾名でクラスを利用" lang="">
<code><span class="reserved">class</span> LibrarySample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">for</span>(<span class="reserved">double</span> x=0; x&lt;1; x+=0.1)
      <em>System.Console</em>.Write(<span class="literal">"sin({0}) = {1}\n"</span>, x, <em>System.Math</em>.Sin(x));
  }
}
</code></pre>


以下のように using ディレクティブを利用します。
(using ディレクティブに関しても「[名前空間](sp_namespace.md)」で説明します。)

<pre class="source" title="using を使ってクラスを利用" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> LibrarySample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">for</span>(<span class="reserved">double</span> x=0; x&lt;1; x+=0.1)
      <em>Console</em>.Write(<span class="literal">"sin({0}) = {1}\n"</span>, x, <em>Math</em>.Sin(x));
  }
}
</code></pre>



## <a id="sec-generated-title-7"></a> <a id="class"></a>このページのサンプルで利用しそうなクラス

今後、このページのサンプルで利用しそうなクラスを以下に列挙します。

<table summary="">

	<tr>
		<th>属する名前空間</th>
		<th>クラス名</th>
		<th>機能</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="2"><code>System</code></td>
		<td markdown="1"><code>Console</code></td>
		<td markdown="1">コンソール(MS DOSプロンプト)に対する入出力</td>
	</tr>
	<tr>
		<td markdown="1"><code>Math</code></td>
		<td markdown="1">数学関連の機能(絶対値、三角関数、指数対数、円周率など)</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="4"><code>System.IO</code></td>
		<td markdown="1"><code>Directory</code></td>
		<td markdown="1">ディレクトリ(フォルダ)操作</td>
	</tr>
	<tr>
		<td markdown="1"><code>File</code></td>
		<td markdown="1">ファイル操作</td>
	</tr>
	<tr>
		<td markdown="1"><code>StreamReader</code></td>
		<td markdown="1">ファイルなどからテキストを読み込む</td>
	</tr>
	<tr>
		<td markdown="1"><code>StreamWriter</code></td>
		<td markdown="1">ファイルなどにテキストを書き出す</td>
	</tr>
	<tr>
		<td markdown="1"><code>System.Text</code></td>
		<td markdown="1"><code>Encoding</code></td>
		<td markdown="1">文字コードの指定</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2"><code>System.Collections.Generic</code></td>
		<td markdown="1"><code>List</code></td>
		<td markdown="1">可変長配列</td>
	</tr>
	<tr>
		<td markdown="1"><code>Dictionary</code></td>
		<td markdown="1">辞書(連想配列)</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2"><code>System.Drawing</code></td>
		<td markdown="1"><code>Bitmap</code></td>
		<td markdown="1">ビットマップの読み書き</td>
	</tr>
	<tr>
		<td markdown="1"><code>Graphics</code></td>
		<td markdown="1">GDI+ 描画サーフェス</td>
	</tr>
	<tr>
		<td markdown="1"><code>System.Windows.Forms</code></td>
		<td markdown="1"><code>Form</code></td>
		<td markdown="1">Windows アプリケーションフォーム</td>
	</tr>
</table>


これらの説明は「[標準ライブラリ](../index.md#lib)」で行います。
