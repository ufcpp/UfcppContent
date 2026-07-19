---
title: "名前空間"
source_url: "https://ufcpp.net/study/csharp/structured/sp_namespace/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2023-07-29T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 6.0"
umbraco_id: 1244
parent_id: 1217
sort_order: 16
aliases:
  - "/csharp/sp_namespace"
  - "/csharp/sp_namespace.html"
  - "/csharp/structured/sp_namespace/"
  - "/study/csharp/sp_namespace"
  - "/study/csharp/sp_namespace.html"
---

# 名前空間

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
<strong id="namespace" class="keyword">名前空間</strong>（name space）とは、
ファイルを種類ごとにフォルダに分けて管理するのと同じように、
クラスを種類ごとに分けて管理するための機構です。


##### <a id="sec-generated-title-2"></a>ポイント
* namespace キーワードで名前空間を定義します。

* フォルダを掘ってファイルを整理するような感覚で、名前空間を作ってクラスを整理します。

* 例： namespace SampleNameSpace { class SampleClass {} }



##<a id="sec-generated-title-3"></a> <a id="about"></a>名前空間とは
名前空間は、ファイル整理のためにフォルダ分けすることに例えられます。

例えば、ウェブページを作成する場合、コンテンツごとにフォルダに分けて管理すると、サイトの管理がしやすくなります。
例えば、うちのサイトの場合、以下のようなフォルダ構成になっています。
（注：今は構成が変わっています。昔はこういう構成でした。）

<pre class="source" title="うちのサイトの階層構造" lang="">
<code>/--+-- memo           <span class="comment">(ブログ的な何か)</span>
   |
   +-- csharp         <span class="comment">(このコーナー)</span>
   |
   +-- study-------+  <span class="comment">(院試勉強まとめ用)</span>
                   |
                   +-- em      <span class="comment">(電磁理論)</span>
                   |
                   +-- math    <span class="comment">(数学)</span>
</code></pre>


そして各フォルダの中にhtmlや画像ファイルがあります。
このようにコンテンツごとに分けることで、どこにどのファイルがあるのかが分かりやすくなりますし、
それぞれのフォルダに同じ名前のファイル(例えばindex.htmlやback.png)があっても問題はおきません。

プログラムを作成する場合でも、プログラムの規模が大きくなってきて、クラスの数が多くなってくると、
クラスを関連性のあるもの同士まとめて管理するような仕組みが必要になってきます。
そのような、クラスを階層的に分類するための機構が<em>名前空間</em>です。

例として、.NET frameworkの標準クラスライブラリを見てみましょう。
.NET frameworkの標準クラスライブラリ中のクラスの大半は<code>System</code>という名前空間に属しています。
<code>System</code>名前空間の下に、<code>Text</code>、<code>IO</code>、<code>Drawing</code>などの名前空間があります。
以下に、名前空間の階層構造と、各名前空間の説明および名前空間に属するクラスの一部を簡単に示します。

<pre class="source" title="System名前空間の階層構造の例" lang="">
<code>System --+
         |
         +-- IO
         |   <span class="comment">(ファイル入出力。File や Directory などが属する。)</span>
         +-- Text -----+  <span class="comment">(文章処理。Encoding などが属する。)</span>
         |             |
         |             +-- RegularExpressions
         |                 <span class="comment">(正規表現。Regex や Match などが属する。)</span>
         |
         +-- Drawing --+  <span class="comment">(GUI処理。Image や Font や Icon などが属する。)</span>
                       |
                       +-- Imaging
                       |   <span class="comment">(画像処理。ImageFormat や Encoder などが属する。)</span>
                       +-- Printing
                           <span class="comment">(印刷。PrintController などが属する。)</span>
</code></pre>


このように階層的に名前を管理することで、例えば、<code>System.Text.Encoding</code>クラス(Windowsのファイルシステムではフォルダの区切りに「 <code>\\</code> 」を使いますが、C#の名前空間の区切りには「 <code>.</code> 」を使います)は画像や音声のエンコード形式ではなくテキストの文字コードだと容易に見当が付きます。

C# では、名前空間の定義(= フォルダーを掘るようなものに) `namespace` キーワードを使います。

<pre class="source" title="namespace で名前空間を作る">
<code><span class="reserved">namespace</span> MyNamespace <span class="comment">// ← MyNamespace という名前空間(フォルダーみたいなもの)を掘った状態</span>
{
    <span class="comment">// その中にクラスを置く</span>
    <span class="reserved">class</span> <span class="type">X</span> { }
}
</code></pre>

一方で、「パスを通す」(フルネームで書かなくても `File` や `Regex` だけでクラスなどを参照する)ための構文も持っていて、こちらには `using` キーワードを使います。

<pre class="source" title="using で名前空間の中身を参照する">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="comment">// System.IO の中に Directory がある。</span>
<span class="comment">// フルネームで書くなら System.IO.Directory.GetFiles()</span>
<span class="reserved">var</span> count = <span class="type">Directory</span>.<span class="method">GetFiles</span>(<span class="string">"."</span>).Length;

<span class="comment">// System の中に Console がある。</span>
<span class="comment">// フルネームで書くなら System.Console()</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$"フォルダーの下に </span>{count}<span class="string"> 個のファイルがあります"</span>);
</code></pre>

ちなみに、名前空間に含まれない部分、ソースコードの一番上の部分を<strong id="global-namespace" class="keyword">グローバル名前空間</strong>(global namespace)と呼びます。

<pre class="source" title="グローバル">
<code><span class="comment">// この辺りの事を「グローバル」(global)と呼ぶ。</span>

<span class="reserved">namespace</span> MyNamespace
{
    <span class="comment">// この辺りは「名前空間の中」。</span>
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="use"></a>名前空間の使い方
具体的に名前空間を使う方法を見ていきましょう。
ここでは例として、学校の課題で文字列クラス、リストクラス、可変長配列クラス、画像クラスを作れといわれたとします(これらのものは、標準ライブラリに初めから用意されていますが、プログラムの勉強のためにわざわざ自作してみることになった)。

###<a id="sec-generated-title-5"></a> <a id="namespace-declaration"></a>namespace (名前空間の定義)
まず、課題を出された各人の作ったクラスの名前が重ならないように、それそれ自分の名前を使って名前空間を作ります。
文字列クラス<code>String</code>はそのすぐ下に作りましょう。
そして、リストクラス<code>List</code>と可変長配列クラス<code>Vector</code>は、名前空間<code>Collections</code>を作ってその下に、画像クラス<code>Image</code>は名前空間<code>Drawing</code>を作ってその下に作ることにします。
階層構造は以下のようになります。

<pre class="source" title="課題用の名前空間の階層構造" lang="">
<code>Ufcpp --+-- String                    <span class="comment">(文字列クラス)</span>
        |
        +-- Collections --+-- List    <span class="comment">(リストクラス)</span>
        |                 |
        |                 +-- Vector  <span class="comment">(可変長配列クラス)</span>
        |
        +-- Drawing --------- Image   <span class="comment">(画像クラス)</span>
</code></pre>


このような構造の名前空間を作るためには以下のように書きます。

<pre class="source" title="名前空間の定義の仕方の例" lang="">
<code><span class="reserved">namespace</span> Ufcpp
{
  <span class="reserved">class</span> String{<span class="comment">// String の内容</span>}

  <span class="reserved">namespace</span> Collections
  {
    <span class="reserved">class</span> List{<span class="comment">// List の内容</span>}

    <span class="reserved">class</span> Vector{<span class="comment">// Vector の内容</span>}
  }

  <span class="reserved">namespace</span> Drawing
  {
    <span class="reserved">class</span> Image{<span class="comment">// Image の内容</span>}
  }
}
</code></pre>


名前空間を定義するためには<em>
        <code>namespace</code>
      </em>というキーワードを使います。
そしてその後に続く {} の中で定義したクラスや名前空間はすべてその名前空間に属することになります。
また、以下のように書いてもこれとまったく同じ意味になります。

<pre class="source" title="名前空間の定義の仕方のもう一つの例" lang="">
<code><span class="reserved">namespace</span> Ufcpp
{
  <span class="reserved">class</span> String{<span class="comment">// String の内容</span>}
}

<span class="reserved">namespace</span> Ufcpp.Collections
{
  <span class="reserved">class</span> List{<span class="comment">// List の内容</span>}
}

<span class="reserved">namespace</span> Ufcpp.Collections
{
  <span class="reserved">class</span> Vector{<span class="comment">// Vector の内容</span>}
}

<span class="reserved">namespace</span> Ufcpp.Drawing
{
  <span class="reserved">class</span> Image{<span class="comment">// Image の内容</span>}
}
</code></pre>


つまり、名前空間を2つ以上の場所に分けて書くこともできますし、
「 <code>.</code> 」で区切ることで階層構造を指定できます。

次に、名前空間中に定義したクラスを参照する方法を説明します。
名前空間中に定義したクラスは、以下のように、階層構造を「 <code>.</code> 」で区切って指定することで参照できます。

<pre class="source" title="名前空間中のクラスの参照" lang="">
<code><span class="reserved">class</span> NameSpaceTest
{
  <span class="reserved">static void</span> Main()
  {
    Ufcpp.String str = <span class="reserved">new</span> Ufcpp.String(<span class="literal">"test"</span>);

    Ufcpp.Collections.List list = <span class="reserved">new</span> Ufcpp.Collections.List();
    Ufcpp.Collections.Vector vec = <span class="reserved">new</span> Ufcpp.Collections.Vector();

    Ufcpp.Drawing.Image image = <span class="reserved">new</span> Ufcpp.Drawing.Image(<span class="literal">"back.png"</span>);
  }
}
</code></pre>


<code>Ufcpp.Collections.Vector</code>というように、名前空間をすべて指定した形式の名前を<em>完全修飾名</em>(fully qualified name)と言います。

###<a id="sec-generated-title-6"></a> <a id="file-scoped-namespace"></a>ファイル スコープ namespace
<h5 class="version version10">Ver. 10</h5>

C# 10.0 から `{}` なしの以下のような書き方で名前空間を指定できるようになりました。

<pre class="source" title="C# 10 からできる名前空間の書き方">
<code><span class="reserved">namespace</span> Namespace;

<span class="reserved">class</span> <span class="type">A</span> { }
</code></pre>

これで以下のコードと同じ意味になります。

<pre class="source" title="同じ意味のコード">
<code><span class="reserved">namespace</span> Namespace
{
    <span class="reserved">class</span> <span class="type">A</span> { }
}
</code></pre>

新しい `{}` なしで `;` を書いてしまう書き方はファイル全体を `namespace {}` でくくったのを同じ意味になります。
そういう意味でこの書き方を<strong id="key-file-scoped-namespace" class="keyword">ファイル スコープ名前空間</strong>(file-scoped namespace)と言います。

ファイル スコープ名前空間は1つの C# ファイルにつき1つだけ書けます。例えば以下のコードはコンパイル エラーになります。

<pre class="source" title="複数のファイル スコープ名前空間を書くとエラー">
<code><span class="reserved">namespace</span> Ns1;
<span class="reserved">namespace</span> <span class="error">Ns2</span>;

<span class="reserved">class</span> <span class="type">A</span> { }
</code></pre>

また、ファイル スコープ名前空間はファイルの「ほぼ先頭」に書く必要があります。
ファイル スコープ名前空間よりも前に書けるものはかなり限られていて、

* [コメント](../start/st_comment.md)
* [プリプロセス命令](../misc/sp_preprocess.md#preprocess)
* 次節で説明する[using](#using-directive)
* [外部エイリアス](#extern)
* [assembly、module 対象の属性](../dynamic/sp_attribute.md#target)

くらいです。このうち頻繁に利用するのはコメントと using くらいでしょう。

<pre class="source" title="ファイル スコープ名前空間よりも前に書けるもの">
<code><span class="comment">// コメントと using は namespace よりも前に書ける。</span>
<span class="reserved">using</span> System.Text;

<span class="reserved">namespace</span> Ns1;

<span class="comment">// using は後にも書ける。</span>
<span class="reserved">using</span> System.Text.Encodings;

<span class="reserved">class</span> <span class="type">A</span> { }
</code></pre>

これで以下のコードと同じ意味になります。

<pre class="source" title="同じ意味のコード">
<code><span class="comment">// コメントと using は namespace よりも前に書ける。</span>
<span class="reserved">using</span> System.Text;

<span class="reserved">namespace</span> Ns1
{
    <span class="comment">// using は後にも書ける。</span>
    <span class="reserved">using</span> System.Text.Encodings;

    <span class="reserved">class</span> <span class="type">A</span> { }
}
</code></pre>

「インデントが1段減る」程度の小さなメリットですが、
一方でデメリットも「1ファイルに1つしか書けない」程度で、
ほとんどの人は制限を掛けられなくても最初から「1ファイルに1つしか書かない」ので特に問題にはならないでしょう。

###<a id="sec-generated-title-7"></a> <a id="using-directive"></a>using (名前空間の参照)
また、いちいち完全修飾名を書かなくても済むように、<strong id="using" class="keyword">using ディレクティブ</strong>というものが用意されています。

<pre class="source" title="usingディレクティブの例1" lang="">
<code><span class="reserved">using</span> Ufcpp; <span class="comment">// 名前空間 Ufcpp 内にあるクラスを修飾名なしで使えるようになる</span>

<span class="reserved">class</span> NameSpaceTest
{
  <span class="reserved">static void</span> Main()
  {
    String str = <span class="reserved">new</span> String(<span class="literal">"test"</span>); <span class="comment">// Ufcpp. が要らない</span>

    Drawing.Image image = <span class="reserved">new</span> Drawing.Image(<span class="literal">"back.png"</span>);
  }
}
</code></pre>


<pre class="source" title="usingディレクティブの例1" lang="">
<code><span class="reserved">using</span> Ufcpp;
<span class="reserved">using</span> Ufcpp.Collections;
<span class="reserved">using</span> Ufcpp.Drawing;

<span class="reserved">class</span> NameSpaceTest
{
  <span class="reserved">static void</span> Main()
  {
    String str = <span class="reserved">new</span> String(<span class="literal">"test"</span>);     <span class="comment">// Ufcpp. が要らない</span>

    List list = <span class="reserved">new</span> List();              <span class="comment">// Ufcpp.Collections も要らない</span>
    Vector vec = <span class="reserved">new</span> Vector();

    Image image = <span class="reserved">new</span> Image(<span class="literal">"back.png"</span>); <span class="comment">// Ufcpp.Drawing. も要らない</span>
  }
}
</code></pre>


先頭の<em>
        <code>using</code>
      </em>から始まる行がusingディレクティブです。
このように、usingディレクティブを使うことでコードの入力手間を省くことが出来ます。

ちなみに、 using ディレクティブはほぼファイルの先頭、もしくは、名前空間内の先頭にしか書けません。
(ファイルの先頭も「グローバル名前空間の先頭」という扱いなので、「名前空間内の先頭にだけ書ける」と考えて大丈夫です。)
using ディレクティブよりも前に書けるのは、
コメントや空白のようにプログラムに影響しないものか、
[プリプロセッサー](../misc/sp_preprocess.md)や[extern alias](#extern)などのめったに使わない構文だけです。

<pre class="source" title="using よりも前に書けるものはほとんどない">
<code><span class="comment">// (コメントを除いて) using より前にはほぼ何も書けない。</span>
<span class="reserved">using</span> System;

Console.WriteLine(); <span class="comment">// 何か書いてしまうと…</span>

<span class="error"><span class="reserved">using</span> System.IO;</span> <span class="comment">// この行はコンパイル エラー。</span>
</code></pre>

ただ、名前空間自体が入れ子に書けるので、「名前空間の先頭にしか書けない」といっても using ディレクティブも入れ子で書けます。

<pre class="source" title="入れ子の名前空間と using ディレクティブ">
<code><span class="reserved">using</span> System;

<span class="reserved">namespace</span> Ns1
{
    <span class="reserved">using</span> System.IO;

    <span class="reserved">namespace</span> Ns2
    {
        <span class="reserved">using</span> System.Collections;
    }
}
</code></pre>

また「using しすぎ」にはそこそこ注意が必要です。
名前の衝突を避けるために名前空間を掘っているのに、using するとその「名前空間分け」をなくすことになります。
例えば、以下のように「別名前空間の同名の型」を用意します。

<pre class="source" title="別名前空間の同名の型">
<code><span class="comment">// 名前空間違いで同じ名前のクラスを用意しておく。</span>
<span class="reserved">namespace</span> A
{
    <span class="reserved">class</span> <span class="type">X</span> { }
}

<span class="reserved">namespace</span> B
{
    <span class="reserved">class</span> <span class="type">X</span> { }
}
</code></pre>

ここで、`using A` と `using B` を同時に書いてしまうと「どちらかわからない」というコンパイル エラーを起こします。
(こういうエラーを「名前があいまい」(ambiguous)と言います。)

<pre class="source" title="同列の using でエラーを起こす例">
<code><span class="comment">// A と B の using を同列に並べる。</span>
<span class="reserved">using</span> A;
<span class="reserved">using</span> B;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="type"><span class="error">X</span></span> x; <span class="comment">// A.X か B.X かわからないのでエラー。</span>
}
</code></pre>

ちなみに、[後述しますが](#priority)、
入れ子の場合は内側優先で名前解決します。

###<a id="sec-generated-title-8"></a> <a id="global-using"></a>global using
<h5 class="version version10">Ver. 10</h5>

C# 10.0 から `using` ディレクティブの前に `global` という修飾を付けることで、
[プロジェクト](../package/project.md#project)内全域に対して影響を及ぼす `using` (名前空間の参照)ができるようになりました。
(これを <strong id="key-global-using" class="keyword">global using ディレクティブ</strong>といいます。
俗称としては単に「global using」。)

例えば、プロジェクト内のどこか1つのファイルに以下のようなコードを書いたとします。

<pre class="source" title="global using の例">
<code><span class="reserved">global</span> <span class="reserved">using</span> System.Text.RegularExpressions;
</code></pre>

これで、このプロジェクト内のすべてのファイルで、ファイルの先頭に `using System.Text.RegularExpressions` を書いたのと同じ状態になります。

例えば別のファイルに以下のようなコードを書いたとき、

<pre class="source" title="global using と同じプロジェクト内の別ファイルの例">
<code><span class="reserved">var</span> line = <span class="type">Console</span>.<span class="method">ReadLine</span>();
<span class="reserved">var</span> m = <span class="type">Regex</span>.<span class="method">Match</span>(line, <span class="string">@"\d+"</span>);
<span class="control">if</span> (m.Success)
    <span class="type">Console</span>.<span class="method">WriteLine</span>(m.Value);
</code></pre>

以下のコードと同じ扱いでコンパイルされます。
(この例の場合、`Regex` クラスが `System.Text.RegularExpressions` 名前空間内で定義されいているクラスなので、`using System.Text.RegularExpressions` が必要。)

<pre class="source" title="上記コードと同じ意味のもの">
<code><span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">var</span> line = <span class="type">Console</span>.<span class="method">ReadLine</span>();
<span class="reserved">var</span> m = <span class="type">Regex</span>.<span class="method">Match</span>(line, <span class="string">@"\d+"</span>);
<span class="control">if</span> (m.Success)
    <span class="type">Console</span>.<span class="method">WriteLine</span>(m.Value);
</code></pre>

同じキーワードを流用したため後述する [global エイリアス](#global)と紛らわしいですが別物です。

ちなみに、通常の using ディレクティブに加え、後述する [using static](#using-static) や [using エイリアス](#alias)に対しても同様に `global` 修飾を付けることでプロジェクト全域化できます。

<pre class="source" title="global using static と global using エイリアス">
<code><span class="reserved">global</span> <span class="reserved">using</span> System.Text.RegularExpressions;
<span class="reserved">global</span> <span class="reserved">using</span> <span class="reserved">static</span> System.Linq.<span class="type">Enumerable</span>;
<span class="reserved">global</span> <span class="reserved">using</span> <span class="type">Date</span> = System.<span class="type">DateOnly</span>;
</code></pre>

global using は通常の using ディレクティブの前にしか書けません。
例えば以下のコードはコンパイル エラーになります。

<pre class="source" title="global using は using の前にしか書けない">
<code><span class="reserved">using</span> System;
<span class="error"><span class="reserved">global</span></span> <span class="reserved">using</span> System.Text.RegularExpressions;
</code></pre>

using ディレクティブ自体が、ファイルの中でもかなり先頭の方にしか書けない構文なので、
必然的に global using よりも前に書けるものはほとんどなくなります。
[ファイル スコープ名前空間](#file-scoped-namespace)よりもさらに厳しくて、

* [コメント](../start/st_comment.md)
* [プリプロセス命令](../misc/sp_preprocess.md#preprocess)
* [外部エイリアス](#extern)

しか書けません。

####<a id="sec-generated-title-9"></a> <a id="usage-global-using"></a>global using の用途
前節で「using しすぎ」に注意を促しましたが、プロジェクト全域に影響を及ぼす global using ではなおの事注意が必要です。
基本的には「むやみやたらと使うものではない」という認識でいいと思います。

その一方で、`System` 名前空間(標準ライブラリの名前空間)のように、
世の中の C# コードの過半数が using していて、
「それはさすがに global using しても誰も困らないだろう」というものもあります。

実際、例えば .NET 5 (Visual Studio 2019) 時点で、Visual Studio でテンプレート通りに C# のクラスを作ると、
初期状態で以下のようなコードが作られます。
`System`、`System.Collections.Generic` などの名前空間は「ほぼみんな使う」と判断されていて、初期状態で using が付いてきます。

<pre class="source" title="Visual Studio のテンプレート通りに作ったファイル">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;
<span class="reserved">using</span> System.Text;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">namespace</span> ConsoleApp1
{
    <span class="reserved">class</span> <span class="type">A</span>
    {
    }
}
</code></pre>

これを、[ファイル スコープ名前空間](#file-scoped-namespace)と併せて、
以下のようなコードにまでテンプレートの行数を減らしたいというのが global using の主な目的になります。

<pre class="source" title="Visual Studio のテンプレート通りに作ったファイル">
<code><span class="reserved">namespace</span> ConsoleApp1;

<span class="reserved">class</span> <span class="type">A</span>
{
}
</code></pre>

この場合でも、開発者自らが global using を書くことは少なくて、
実際には「自動的に生成されているもの」なことが多くなると思います。
詳しくはブログの「[最初の C# プログラム](../../../blog/2021/8/newprojecttemplate/index.md)」で説明しています。

##<a id="sec-generated-title-10"></a> <a id="using-static"></a>補足: using static
<h5 class="version version6">Ver. 6</h5>

名前空間関連ではないんですが、名前空間の「[using ディレクティブ](#using)」と似たものなのでここで紹介だけしておきたい機能が、
静的メソッドに対する 「[using static](../oop/oo_static.md#key-using-static)」 です。
以下のように、静的メソッドの呼び出しに対して、クラス名を省略できるようになる機能です(C# 6からの機能)。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
<em><span class="reserved">using static</span> System.<span class="type">Math</span></em>;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="reserved">var</span> pi = 2 * <em>Asin(1)</em>;
        <span class="type">Console</span>.WriteLine(<em>PI</em> == pi);
    }
}
</code></pre>


詳しくは、「[静的メンバー](../oop/oo_static.md)」で説明します。

##<a id="sec-generated-title-11"></a> <a id="alias"></a>using エイリアス
先ほど自作した<code>String</code>のテストのために、比較対象として.NET frameworkに標準で用意されている<code>System.String</code>クラスを同時に使用したいとします。
もちろん、<code>Ufcpp.String</code>というように完全修飾名を用いれば、<code>System.String</code>と共存可能なのですが、<strong id="alias" class="keyword">エイリアス</strong>（alias：別名付け）という機能を使うことでも共存させることが出来ます。

エイリアスは以下のような書き方をします。

<pre class="source" title="エイリアスの付け方" lang="">
<code><span class="reserved">using</span> MyString = Ufcpp.String;
</code></pre>


名前空間の先頭でこのような宣言をすることで、その名前空間中では<code>MyString</code>と書くことで<code>Ufcpp.String</code>を参照することが出来ます。

<pre class="source" title="エイリアスの利用例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> MyString = Ufcpp.String;           <span class="comment">// クラスのエイリアス</span>
<span class="reserved">using</span> MyCollections = Ufcpp.Collections; <span class="comment">// 名前空間のエイリアスも作れる</span>

<span class="reserved">class</span> NameSpaceTest
{
  <span class="reserved">static void</span> Main()
  {
    String str = <span class="reserved">new</span> String(<span class="literal">"test"</span>);
    <span class="comment">//↑ System.String が参照される</span>
    MyString str = <span class="reserved">new</span> MyString(<span class="literal">"test"</span>);
    <span class="comment">//↑ Ufcpp.String が参照される</span>
    MyCollections.List list = <span class="reserved">new</span> MyCollections.List();
    <span class="comment">//↑ Ufcpp.Collections.List が参照される</span>
  }
}
</code></pre>



##### <a id="sec-generated-title-12"></a>サンプル
<pre class="source" title="">
<code><span class="reserved">using</span> System;
 
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
<span class="inactive">///</span><span class="comment"> 自作クラス用の名前空間</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
<span class="reserved">namespace</span> Ufcpp
{
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
    <span class="inactive">///</span><span class="comment"> 数学関数の自作</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Math</span>
    {
        <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
        <span class="inactive">///</span><span class="comment"> sin(x) の値を求める。</span>
        <span class="inactive">///</span><span class="comment"> この実装は甘い。</span>
        <span class="inactive">///</span><span class="comment"> 入力できる値は-0.1～0.1程度で、精度も4桁程度。</span>
        <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">double</span> <span class="method">Sin</span>(<span class="reserved">double</span> <span class="variable">x</span>)
        {
            <span class="reserved">double</span> <span class="variable">xx</span> = -<span class="variable">x</span> * <span class="variable">x</span>;
            <span class="reserved">double</span> <span class="variable">fact</span> = 1;
            <span class="reserved">double</span> <span class="variable">sin</span> = <span class="variable">x</span>;
 
            <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 2; <span class="variable">i</span> &lt; 100;)
            {
                <span class="variable">fact</span> *= <span class="variable">i</span>; ++<span class="variable">i</span>; <span class="variable">fact</span> *= <span class="variable">i</span>; ++<span class="variable">i</span>;
                <span class="variable">x</span> *= <span class="variable">xx</span>;
                <span class="variable">sin</span> += <span class="variable">x</span> / <span class="variable">fact</span>;
            }
            <span class="control">return</span> <span class="variable">sin</span>;
        }
    }
}
 
<span class="reserved">namespace</span> Sample
{
    <span class="reserved">using</span> <span class="type">MyMath</span> = Ufcpp.<span class="type">Math</span>;
 
    <span class="reserved">class</span> <span class="type">NameSpaceSample</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
        {
            <span class="type">Console</span>.<span class="method">Write</span>(<span class="string">&quot;   x, System.Math.Sin(x), Ufcpp.Math.Sin(x)</span><span style="color:#b776fb;">\n</span><span class="string">&quot;</span>);
            <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 10; ++<span class="variable">i</span>)
            {
                <span class="reserved">double</span> <span class="variable">x</span> = 0.01 * <span class="variable">i</span>;
 
                <span class="reserved">double</span> <span class="variable">y</span> = <span class="type">Math</span>.<span class="method">Sin</span>(<span class="variable">x</span>);   <span class="comment">// System.Math.Sin呼び出し</span>
                <span class="reserved">double</span> <span class="variable">z</span> = <span class="type">MyMath</span>.<span class="method">Sin</span>(<span class="variable">x</span>); <span class="comment">// Ufcpp.Math.Sin呼び出し</span>
 
                <span class="type">Console</span>.<span class="method">Write</span>(<span class="string">&quot;{0:f2},           {1:f6},            {2:f6}</span><span style="color:#b776fb;">\n</span><span class="string">&quot;</span>, <span class="variable">x</span>, <span class="variable">y</span>, <span class="variable">z</span>);
            }
        }
    }
}
</code></pre>


<pre class="console" title="">
   x, System.Math.Sin(x), Ufcpp.Math.Sin(x)
0.00,           0.000000,            0.000000
0.01,           0.010000,            0.010000
0.02,           0.019999,            0.019999
0.03,           0.029996,            0.029996
0.04,           0.039989,            0.039989
0.05,           0.049979,            0.049979
0.06,           0.059964,            0.059964
0.07,           0.069943,            0.069943
0.08,           0.079915,            0.079915
0.09,           0.089879,            0.089879
</pre>



###<a id="sec-generated-title-13"></a> <a id="using-any-type">任意の型に対する using エイリアス</a>
<h5 class="version version12">Ver. 12</h5>

C# 12 から以下のようなコードをコンパイルできるようになりました。

<pre class="source" title="C# 12 から">
<span class="reserved">using</span> <span class="type struct">Primitive</span> <span class="operator">=</span> <span class="reserved">int</span>;
<span class="reserved">using</span> <span class="type">Array</span> <span class="operator">=</span> <span class="reserved">int</span>[];
<span class="reserved">using</span> <span class="type struct">Nullable</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">?</span>;
<span class="reserved">using</span> <span class="type struct">Tuple</span> <span class="operator">=</span> (<span class="reserved">int</span>, <span class="reserved">int</span>);
</pre>

要するに以下の2点が改善点です。

* `int` みたいなキーワードをそのまま using エイリアスの右辺に書けるようになった
* [配列](st_array.md)、[nullable 値型](../resource/sp2_nullable.md)、[タプル](../datatype/tuples.md)などを C# の専用構文を使って書けるようになった

C# 11 以前でも以下のように、キーワード・専用構文を使わない書き方はできていました。

<pre class="source" title="C# 11 でもできる書き方">
<span class="reserved">using</span> <span class="type struct">Primitive</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">Int32</span>;
<span class="reserved">using</span> <span class="type struct">Nullable</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">Nullable</span>&lt;System<span class="operator">.</span><span class="type struct">Int32</span>&gt;;
<span class="reserved">using</span> <span class="type struct">Tuple</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">ValueTuple</span>&lt;System<span class="operator">.</span><span class="type struct">Int32</span>, System<span class="operator">.</span><span class="type struct">Int32</span>&gt;;
<span class="comment">//※ 配列を書く手段はなかった</span>
</pre>

また、少々不可解なことに、以下のようなコードも C# 11 以前から書けていました。

<pre class="source" title="C# 11 でもできる書き方(解せぬ)">
<span class="reserved">using</span> <span class="type struct">Primitive</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">ValueTuple</span>&lt;<span class="reserved">int</span>&gt;;
<span class="reserved">using</span> <span class="type struct">Array</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">ValueTuple</span>&lt;<span class="reserved">int</span>[]&gt;;
<span class="reserved">using</span> <span class="type struct">Nullable</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">ValueTuple</span>&lt;<span class="reserved">int</span><span class="operator">?</span>&gt;;
<span class="reserved">using</span> <span class="type struct">Tuple</span> <span class="operator">=</span> System<span class="operator">.</span><span class="type struct">ValueTuple</span>&lt;(<span class="reserved">int</span>, <span class="reserved">int</span>)&gt;;
</pre>

つまり、型引数(ジェネリック型 `X<T>` の `T` の部分)であればこれまでも `int` や `int[]` などが書けました。
C# 12 では、なぜか最上位レベルの時にだけかかっていた謎の制限を取り払ったことになります。
(実際、仕様書・実装ともに微々たる修正だったようです。)

ちなみに、C# 12 ではポインターや関数ポインターに対しても using エイリアスを使えるようになりました。
詳しくは「[unsafe 型に対する using エイリアス](../interop/sp_unsafe.md#unsafe-using)」で説明します。

##<a id="sec-generated-title-14"></a> <a id="alias_sp"></a>エイリアス修飾子
<h5 class="version version2">Ver. 2.0</h5>

前節で説明したとおり、
名前空間にはエイリアス（別名）を付けられます。

例えば、以下のように、ちょっと長めの名前空間名 Ufcpp.Test.Utilities に、
短いエイリアス Util を付けたとします。

<pre class="source" title="エイリアス（これ自体は問題がないけども・・・）" lang="">
<code><span class="reserved">namespace</span> Ufcpp.Test.Utilities
{
  <span class="reserved">class</span> Image {}
}

<span class="reserved">namespace</span> TestNamespace
{
  <em><span class="reserved">using</span> Util = Ufcpp.Test.Utilities;</em> <span class="comment">// エイリアスをつける。</span>

  <span class="reserved">class</span> Program
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      <em>Util.Image</em> img = <span class="reserved">new</span> Util.Image();
    }
  }
}
</code></pre>


このコード自体には特に問題もなく、ちゃんとコンパイルが通ります。
ところが、このプログラムを修正していくうちに、ちょっとした問題が生じる可能性があります。
例えば、複数人で開発しているものとして、
自分以外の誰かが、TestNamespace 内に Util というクラスを作ってしまったとしましょう。

<pre class="source" title="エイリアスが原因で問題発生" lang="">
<code><span class="reserved">namespace</span> Ufcpp.Test.Utilities
{
  <span class="reserved">class</span> Image {}
}

<span class="reserved">namespace</span> TestNamespace
{
  <span class="reserved">using</span> Util = Ufcpp.Test.Utilities;

  <span class="reserved">class</span> Program
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      Util.Image img = <span class="reserved">new</span> Util.Image();
    }
  }

  <em><span class="reserved">class</span> Util {}</em> <span class="comment">// Util クラスを追加。エラーになる。</span>
}
</code></pre>


たったこれだけでこのコードはコンパイルエラーを起こします。
（エイリアス Util がクラス Util と衝突しましたと怒られるか、
Util と言う名前は既に存在しますと怒られるはず。）

この問題を緩和するため、C# 2.0 では、エイリアス修飾子というものが追加されました。
エイリアス修飾子は、<code>Alias.Class</code> という書き方の代わりに、
<code>Alias::Class</code> と言うように、<code>:</code> を2つ付けます。
このエイリアス修飾子 <code>::</code> は、基本的には <code>.</code> と同じ結果を生みますが、
ただ、エイリアスの後ろにしか付けられないという制限があります。
このため、<code>::</code> の付いている部分の直前はエイリアスであることが確定し、
エイリアスと同名のクラスが追加されても混乱が起こりません。

<pre class="source" title="エイリアス修飾子" lang="">
<code><span class="reserved">namespace</span> Ufcpp.Test.Utilities
{
  <span class="reserved">class</span> Image {}
}

<span class="reserved">namespace</span> TestNamespace
{
  <span class="reserved">using</span> Util = Ufcpp.Test.Utilities;

  <span class="reserved">class</span> Program
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      <em>Util::Image</em> img = <span class="reserved">new</span> Util::Image();
      <span class="comment">//↑ この Util はエイリアスの Util とみなされる。</span>
    }
  }

  <span class="reserved">class</span> Util {} <span class="comment">// Util と同名のクラスがあっても OK。</span>
}
</code></pre>

###<a id="sec-generated-title-15"></a> <a id="global"></a>global 名前空間エイリアス
<h5 class="version version2">Ver. 2.0</h5>

名前の付け方次第では、完全修飾名で書いても参照できない場合があります。
以下のように、名前空間の階層に同名の識別子がある場合です。

<pre class="source" title="完全修飾名で参照できなくなる場合">
<code><reserved></span><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">namespace</span> X.Y
{
    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="comment">// 単に Y って書くと、名前空間 X.Y の方の意味になる</span>
            <span class="type">Y</span>.F(); <span class="comment">// コンパイル エラー。名前空間 Y に F がいない</span>
        }
    }
}

<span class="reserved">class</span> <span class="type">Y</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"class Y"</span>); }
</code></pre>

階層違いで同名のものがあることが原因なので、必ず最上位(グローバル名前空間)からたどる手段があれば解決します。
そのために使うのが、`global`名前空間エイリアスです。
以下のように、`global::`から書き始めれば、最上位から名前をたどれます。

<pre class="source" title="global エイリアスを使って解決">
<code><reserved></span><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">namespace</span> X.Y
{
    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="comment">// global エイリアスを使えば、最上位から名前をたどれる</span>
            <span class="reserved">global</span>::<span class="type">Y</span>.F();
        }
    }
}

<span class="reserved">class</span> <span class="type">Y</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"class Y"</span>); }
</code></pre>

`global`は、`::`の前でだけキーワード扱いされる文脈キーワードです。
その他の場面では、`global`クラスを作ったり、`global`という名前の名前空間を作ったり、参照したりもできます。

##<a id="sec-generated-title-16"></a> <a id="extern"></a>外部エイリアス
<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 では、using を使ってエイリアスを定義する代わりに、
コンパイルオプションでエイリアスを付けることが可能になりました（外部エイリアス）。

外部エイリアスを使うにはまず、
ソースファイル中に extern alias という宣言を書きます。

<pre class="source" title="外部エイリアス" lang="">
<code><em><span class="reserved">extern alias</span> X;</em>

<span class="reserved">class</span> Program
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    X::A a = <span class="reserved">new</span> X::A();
  }
}
</code></pre>


そして、ソースファイルのコンパイル時に、
以下のようなオプションを追加します。

<pre class="console" title="外部エイリアス（コンパイルオプション）">
csc <em>/r:X=Ufcpp.dll</em> Test.cs
</pre>


これで、Ufcpp.dll というライブラリ中で定義された <code>A</code> というクラスを、
<code>X::A</code> という名前で参照できるようになります。

Visual Studio 上では、図1のように、参照しているライブラリのプロパティを開いて、エイリアス(aliases)の行を編集します。

<figure>
	[![Visual Studio 上での外部エイリアス設定。](../../../../assets/media/ufcpp2000/csharp/fig/ExternAliasInVs.png)](../../../../assets/media/ufcpp2000/csharp/fig/ExternAliasInVs.png)
	<figcaption>Visual Studio 上での外部エイリアス設定。</figcaption>
</figure>


サンプル: [ExternAliasConsoleApplication](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/StructuredProgramming/ExternAliasConsoleApplication)

この外部エイリアスを使うと、2つの異なるライブラリに、完全に同名前空間・同名のクラスがあっても、参照し分けることができます。
例えば、上記のサンプルは以下のようなシナリオを想定したものです。

* .NET 2.0 で LINQ を使うために、Enumerable クラスや Extension 属性を自作した(BackportEnumerable.dll)

* その BackportEnumerable のテストのために、標準の LINQ と自作の LINQ を両方使って、実行結果を比べたい(ExternAliasConsoleApplication.exe)


以下のようなコードで呼び分けできます。

<pre class="source" title="" lang="">
<code><span class="reserved">namespace</span> UsingStandard
{
    <span class="reserved">using</span> System.Linq;

    <span class="reserved">class</span> <span class="type">Sample</span>
    {
        <span class="reserved">public static void</span> Run()
        {
            <span class="reserved">var</span> x = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
            <span class="reserved">var</span> y = x.Where(i =&gt; (i &amp; 1) != 0).Select(i =&gt; i * i); <span class="comment">// 標準の LINQ</span>
            <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="literal">", "</span>, y));
        }
    }
}

<span class="reserved">namespace</span> UsingBackport
{
    <span class="reserved">extern alias</span> Backport; <span class="comment">// コンパイル オプションで BackportEnumerable.dll を指定</span>
    <span class="reserved">using</span> Backport::System.Linq;

    <span class="reserved">class</span> <span class="type">Sample</span>
    {
        <span class="reserved">public static void</span> Run()
        {
            <span class="reserved">var</span> x = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
            <span class="reserved">var</span> y = x.Where(i =&gt; (i &amp; 1) != 0).Select(i =&gt; i * i); <span class="comment">// 自作のパックポート LINQ</span>
            <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="literal">", "</span>, y));
        }
    }
}
</code></pre>

##<a id="sec-generated-title-17"></a> <a id="priority"></a>名前解決の優先度
名前空間によって、同じ名前のものを複数作れます。
その同じ名前のものを使い分けたければ、ちゃんと完全修飾名を使う方のが一番ですが、
一応、`using`を並べた場合の優先度についても説明しておきます。

まず、`using`の使い過ぎなどでどちらか判別できない状況になると、コンパイル エラーになります。

<pre class="source" title="判別できずにコンパイル エラー">
<code><reserved></span><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;
<span class="reserved">using</span> A;
<span class="reserved">using</span> B;

<span class="reserved">namespace</span> MyApp
{
    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="type">Lib</span>.F(); <span class="comment">// コンパイル エラー。A, B 区別つかない</span>
        }
    }
}

<span class="reserved">namespace</span> A
{
    <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"A"</span>); }
}
<span class="reserved">namespace</span> B
{
    <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"B"</span>); }
}
</code></pre>

`using`や型定義を書く場所によって優先度が付いています。
優先度違いのものであれば、優先度が高い方が選ばれ、コンパイルできます。
逆に、同優先度のものがあるとエラーになります。

優先度ですが、以下のように、使う場所に近いほど優先、直接的なものほど優先です。

<pre class="source" title="名前参照の優先度">
<code><reserved></span><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;
<span class="reserved">using</span> A;

<span class="comment">// using よりは、直接定義されているものの方が優先 A &lt; C, global</span>
<span class="comment">// エイリアスと型定義は同列 C = global</span>
<span class="reserved">using</span> <span class="type">Lib</span> = C.<span class="type">Lib</span>;
<span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"global"</span>); }

<span class="reserved">namespace</span> MyApp
{
    <span class="reserved">using</span> B; <span class="comment">// 内側に using を書くと、外より優先 A, C, global &lt; B</span>

    <span class="comment">// 同一名前空間内にあるものは1番高い優先度 B &lt; MyApp</span>
    <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"MyApp"</span>); }

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="comment">// Lib は5つある</span>
            <span class="comment">// この場合 MyApp.Lib が使われる</span>
            <span class="comment">// 優先度 高 MyApp &gt; B &gt; global = C &gt; A 低</span>
            <span class="type">Lib</span>.F();

            <span class="comment">// ちゃんと呼び分けたければフルネームで書く</span>
            A.<span class="type">Lib</span>.F();
            B.<span class="type">Lib</span>.F();
            C.<span class="type">Lib</span>.F();
            MyApp.<span class="type">Lib</span>.F();
            <span class="reserved">global</span>::<span class="type">Lib</span>.F();
        }
    }
}

<span class="reserved">namespace</span> A
{
    <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"A"</span>); }
}
<span class="reserved">namespace</span> B
{
    <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"B"</span>); }
}
<span class="reserved">namespace</span> C
{
    <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F() =&gt; WriteLine(<span class="string">"C"</span>); }
}
</code></pre>
