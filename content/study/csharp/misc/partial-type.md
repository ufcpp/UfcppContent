---
title: "型の分割定義 (partial)"
source_url: "https://ufcpp.net/study/csharp/misc/partial-type/"
content_type: "Article"
published_at: "2024-08-31T00:00:00"
updated_at: "2025-09-21T19:49:26"
tags: []
umbraco_id: 2500
parent_id: 1338
sort_order: 0
aliases:
  - "/csharp/misc/partial-type/"
---

# 型の分割定義 (partial)

##<a id="sec-generated-title-1"></a> <a id="abstract">概要</a>
<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 で、`partial` 修飾子を付けることで、クラスや構造体、インターフェイスを複数のファイルに分けて型を定義できるようになりました。
この `partial` によるファイルの分割は、
「片方のファイルを手書き、もう片方のファイルを開発ツールなどによって自動生成」みたいな状況を想定しています。

(それ以外の用途でむやみに複数のファイル分けると、どのファイルに何のメソッドがあるのか探しにくくなるので、
通常は、むしろ、クラス定義を複数のファイルに分割しない方がいいです。)

##<a id="sec-generated-title-2"></a> <a id="tool-generated-code">背景: ツール生成のソースコード</a>
Visual Studio などの統合開発環境を利用していると分かると思いますが、
ソースファイルの一部分はプログラマーの手書きではなく、
開発ツールが自動的に生成してくれる部分があります。

例えば、データベースのテーブル定義から C# のクラスを生成するみたいなツールがあります。
仮に、`Id` と `Name` の2つの列がある `Entity` という名前のテーブルから、以下のようなクラスをツール生成したとします。

<pre class="source" title="データベース テーブルの Id, Name 列からのプロパティ生成">
<span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Id</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Name</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

これに対して、プロパティの値の書き換え時に処理を挟みたいとします。
一応、ツール生成のソースコードを書き換えれば目的を達成することは可能ではあります。

<pre class="source" title="ツール生成物をもし手で書き換えたとすると…">
<span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_id</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Id</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_id</span>; <span class="reserved">set</span> { <span class="control">if</span> (<span class="field">_id</span> <span class="operator">!=</span> <span class="reserved">value</span>) { <span class="field">_id</span> <span class="operator">=</span> <span class="reserved">value</span>; <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">true</span>; } } }

    <span class="reserved">private</span> <span class="reserved">string</span><span class="operator">?</span> <span class="field">_name</span>;
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Name</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_name</span>; <span class="reserved">set</span> { <span class="control">if</span> (<span class="field">_name</span> <span class="operator">!=</span> <span class="reserved">value</span>) { <span class="field">_name</span> <span class="operator">=</span> <span class="reserved">value</span>; <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">true</span>; } } }

    <span class="reserved">private</span> <span class="reserved">bool</span> <span class="field">_changed</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Flush</span>() <span class="operator">=&gt;</span> <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">false</span>;
}
</pre>

ここで、データベースのテーブルに列 `X` を追加したので、ツール生成の C# コードも更新したいとします。
「手書きで書き方部分は残して新たに追加したものだけをソースコード生成」なんてことは難しく、普通はすべて上書きされます。

<pre class="source" title="残念ながら、手書きで書き換えた分は紛失する">
<span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Id</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Name</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

ソースコードの一部分を「ここはツール生成だから書き換えないで」領域にすることもできなくはないですが、なかなかに危険です。
例えば、WinForms (C# 1.1 時代からある GUI フレームワーク)開発がまさにそういう方式で、
WinForms アプリを作ると、ソースコード中に以下のような領域ができます。

<pre class="source" title="WinForms アプリで、Visual Studio が生成するコード">
<span class="reserved">namespace</span> WinFormsApp1;

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Form1</span>
{
    <span class="comment">// 前略</span>
    <span class="comment">// この部分は手書きで書き換える想定。</span>

    <span class="preprocess">#</span><span class="preprocess">region</span> Windows Form Designer generated code

    <span class="comment">///</span><span class="comment"> </span><span class="comment">&lt;</span><span class="comment">summary</span><span class="comment">&gt;</span>
    <span class="comment">///</span><span class="comment">  Required method for Designer support - do not modify</span>
    <span class="comment">///</span><span class="comment">  the contents of this method with the code editor.</span>
    <span class="comment">///</span><span class="comment"> </span><span class="comment">&lt;/</span><span class="comment">summary</span><span class="comment">&gt;</span>
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">InitializeComponent</span>()
    {
        <span class="reserved">this</span><span class="operator">.</span><span class="field">components</span> <span class="operator">=</span> <span class="reserved">new</span> System<span class="operator">.</span>ComponentModel<span class="operator">.</span><span class="type">Container</span>();
        <span class="reserved">this</span><span class="operator">.</span>AutoScaleMode <span class="operator">=</span> System<span class="operator">.</span>Windows<span class="operator">.</span>Forms<span class="operator">.</span>AutoScaleMode<span class="operator">.</span>Font;
        <span class="reserved">this</span><span class="operator">.</span>ClientSize <span class="operator">=</span> <span class="reserved">new</span> System<span class="operator">.</span>Drawing<span class="operator">.</span><span class="type struct">Size</span>(<span class="number">800</span>, <span class="number">450</span>);
        <span class="reserved">this</span><span class="operator">.</span>Text <span class="operator">=</span> <span class="string">&quot;Form1&quot;</span>;
    }

    <span class="preprocess">#</span><span class="preprocess">endregion</span>
}
</pre>

まさに「書き換えないで」(do not modify)と書かれていますし、
実際、書き換えても Visual Studio によって元に戻されたりします。
手で書き換える想定のコードとツール生成コードが1ファイルに混ざっていることで、
例えば、「ファイル内で一斉置換」みたいな作業をしたときにツール生成部分を壊したりといった事故もありました。

##<a id="sec-generated-title-3"></a> <a id="partial-class">型の分割</a>
このように、手書きとツール生成の混在は危険なので、別ファイルに分かれている方が安心です。
そこで、C# 2.0 では、クラス定義時に `partial` というキーワードを付けることで、
クラス定義を複数に分割することができるようになりました。
これを <strong id="partial_class" class="keyword">部分クラス</strong>(partial class)と言います。

例えば前節のツール生成例に `partial` を付けてみましょう。

<pre class="source" title="partial の例">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Id</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Name</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

この型に手で何かコードを足したい場合、別ファイルに以下のような感じでコードを書きます。

<pre class="source" title="partial を使えば、別ファイルでクラスに処理を足せる">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">private</span> <span class="reserved">bool</span> <span class="field">_changed</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Flush</span>() <span class="operator">=&gt;</span> <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">false</span>;
}
</pre>

(プロパティ `Id` や `Name` の中の処理を変更したい場合の話は次節の「[メソッドの実装の分離](#partial_method)」で説明します。)

`partial` はクラスの他に、構造体、インターフェイスにもつけれます。

<pre class="source" title="構造体、インターフェイスにも partial">
<span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type struct">S</span> { }
<span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type struct">S</span> { }

<span class="reserved">partial</span> <span class="reserved">interface</span> <span class="type">I</span> { }
<span class="reserved">partial</span> <span class="reserved">interface</span> <span class="type">I</span> { }

<span class="reserved">partial</span> <span class="reserved">record</span> <span class="type">R</span> { }
<span class="reserved">partial</span> <span class="reserved">record</span> <span class="type">R</span> { }

<span class="reserved">partial</span> <span class="reserved">record</span> <span class="reserved">class</span> <span class="type">RC</span> { }
<span class="reserved">partial</span> <span class="reserved">record</span> <span class="reserved">class</span> <span class="type">RC</span> { }

<span class="reserved">partial</span> <span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">RS</span> { }
<span class="reserved">partial</span> <span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">RS</span> { }
</pre>

ただ、部分クラスにしたい場合、すべての型定義に `partial` 修飾子を付ける必要があります。
これは、「ファイルを分けるつもりがなかったのに、他の誰かに勝手に部分定義を足された」みたいなことを避けるためです。

<pre class="source" title="partial はすべての型定義に付ける必要あり">
<span class="comment">// 片方に partial が付いてないとエラー。</span>
<span class="reserved">class</span> <span class="type"><span class="error" title="CS0260">C</span></span> { }

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span> { }
</pre>

ちなみに、`partial` 以外の修飾子に関しては、複数ある型定義についてる修飾子すべてを統合したものになります。
例えば、以下のように「片方が `public`、もう片方が `static`」な場合、この型は `public static` 扱いです。

<pre class="source" title="修飾子は統合される">
<span class="comment">// この型は public static class C 扱い。</span>
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span> { }
<span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type"><span class="static">C</span></span> { }
</pre>

ここで実例として、WPF アプリを紹介します。
WinForms とは違って、WPF (C# 3.0 世代の GUI フレームワーク)は `partial` を使ってツール生成のコードと手書きコードを分けています。
WPF アプリでは、手での書き換えを前提とした以下のようなコードを書く一方で、

<pre class="source" title="WPF で手書きするべきコード例">
<span class="reserved">using</span> System<span class="operator">.</span>Windows;

<span class="reserved">namespace</span> WpfApp1;

<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">MainWindow</span> : <span class="type">Window</span>
{
    <span class="reserved">public</span> <span class="type">MainWindow</span>()
    {
        <span class="method">InitializeComponent</span>();
    }
}
</pre>

ツール生成で以下のようなコードが作られます(実物はだいぶ長いので一部抜粋)。

<pre class="source" title="WPF で、XAML から生成されるコード例">
<span class="comment">//------------------------------------------------------------------------------</span>
<span class="comment">// &lt;auto-generated&gt;</span>
<span class="comment">//     This code was generated by a tool.</span>
<span class="comment">//     Runtime Version:4.0.30319.42000</span>
<span class="comment">//</span>
<span class="comment">//     Changes to this file may cause incorrect behavior and will be lost if</span>
<span class="comment">//     the code is regenerated.</span>
<span class="comment">// &lt;/auto-generated&gt;</span>
<span class="comment">//------------------------------------------------------------------------------</span>

<span class="reserved">using</span> System;
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics;
<span class="reserved">using</span> System<span class="operator">.</span>Windows;
<span class="comment">// 中略</span>

<span class="reserved">namespace</span> WpfApp1
{
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">MainWindow</span> : System<span class="operator">.</span>Windows<span class="operator">.</span><span class="type">Window</span>, System<span class="operator">.</span>Windows<span class="operator">.</span>Markup<span class="operator">.</span><span class="type">IComponentConnector</span>
    {
        <span class="reserved">private</span> <span class="reserved">bool</span> <span class="field">_contentLoaded</span>;

        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">InitializeComponent</span>()
        {
            <span class="control">if</span> (<span class="field">_contentLoaded</span>)
            {
                <span class="control">return</span>;
            }
            <span class="field">_contentLoaded</span> <span class="operator">=</span> <span class="reserved">true</span>;
            System<span class="operator">.</span><span class="type">Uri</span> <span class="variable">resourceLocater</span> <span class="operator">=</span> <span class="reserved">new</span> System<span class="operator">.</span><span class="type">Uri</span>(<span class="string">&quot;/WpfApp1;V1.0.0.0;component/mainwindow.xaml&quot;</span>, System<span class="operator">.</span><span class="type">UriKind</span><span class="operator">.</span>Relative);

<span class="preprocess">#</span><span class="preprocess">line</span> <span class="number">1</span> <span class="string">&quot;..\..\..\MainWindow.xaml&quot;</span>
            System<span class="operator">.</span>Windows<span class="operator">.</span>Application<span class="operator">.</span>LoadComponent(<span class="reserved">this</span>, <span class="variable">resourceLocater</span>);
        }
<span class="comment">// 中略</span>
    }
}
</pre>

こちらのツール生成のコードは、通常、どこに生成されたのかすら意識せず、中身を覗くこともほとんどありません。

##<a id="sec-generated-title-4"></a> <a id="partial_method">メソッドの実装の分離</a>
<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 で
<strong id="partial_method" class="keyword">部分メソッド</strong>（partial method）という機能も追加されました。

どういうものかというと、
[部分クラス](../oop/oo_class.md#partial_class)内限定で、
メソッドに `partial` を付けることでメソッドの宣言と定義を分けれるというものです。

定義の仕方と、制限事項は以下の通り。

* `partial` 修飾子を付けてメソッドを宣言する。

* 必ず部分クラス内になければならない。

* [アクセシビリティ](../oop/oo_conceal.md#level)の指定はできない(自動的に必ず `private` 扱い)。

* 戻り値は `void` 以外不可。

* 引数は自由に取れる。`ref`, `this`, `params` も利用可能。ただし、`out` 引数は不可。

* 静的メソッド（`static`）でもインスタンス メソッド（非 `static`）でも OK。

例として、前節の `Entity` の例で出てきた
「プロパティ `Id` や `Name` の中の処理を変更したい場合の話」をしましょう。

用途的に、「プロパティの値が変わったときに何かはしたい」、
「ただ、何をするかはアプリごとに異なる」
みたいなことがあります。
そういった場合、「プロパティの値が変わった」のタイミングを拾えるよう、
以下のように、部分メソッドを含むコードをツール生成してもらっておきます。

<pre class="source" title="ツール生成で partial メソッドを生成">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_id</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Id</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_id</span>; <span class="reserved">set</span> { <span class="control">if</span> (<span class="field">_id</span> <span class="operator">!=</span> <span class="reserved">value</span>) { <span class="field">_id</span> <span class="operator">=</span> <span class="reserved">value</span>; <span class="method">OnIdChanged</span>(); } } }
    <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">OnIdChanged</span>();

    <span class="reserved">private</span> <span class="reserved">string</span><span class="operator">?</span> <span class="field">_name</span>;
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Name</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_name</span>; <span class="reserved">set</span> { <span class="control">if</span> (<span class="field">_name</span> <span class="operator">!=</span> <span class="reserved">value</span>) { <span class="field">_name</span> <span class="operator">=</span> <span class="reserved">value</span>; <span class="method">OnNameChanged</span>(); } } }
    <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">OnNameChanged</span>();
}
</pre>

`OnIdChanged` と `OnNameChanged` が部分メソッドです。
このまま何も手書きコードを足さなければ、これらのメソッドは何もしません。
(空のメソッドが呼ばれるとかですらなく、メソッドを呼んだ痕跡すらも完全に消えます。)
（さらにいうと、メタデータすら残さず、完全に消えます。
[`Conditional` 属性](../dynamic/sp_attribute.md#compiler_attribute)でも似たようなことができますが、こちらは少なくともメタデータは残ります。）

一方、手書きコードで処理を足したければ以下のような感じのコードを書きます。

<pre class="source" title="partial メソッドに実装を足す例">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Entity</span>
{
    <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">OnIdChanged</span>() <span class="operator">=&gt;</span> <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">true</span>;
    <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">OnNameChanged</span>() <span class="operator">=&gt;</span> <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">true</span>;

    <span class="reserved">private</span> <span class="reserved">bool</span> <span class="field">_changed</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Flush</span>() <span class="operator">=&gt;</span> <span class="field">_changed</span> <span class="operator">=</span> <span class="reserved">false</span>;
}
</pre>

こうすると、追加した `OnIdChanged` や `OnNameChanged` の実装が呼び出されるようになります。

###<a id="sec-generated-title-5"></a> <a id="partial_method-side-effect">部分メソッドの引数で副作用を起こす場合</a>
「不要な場合は完全削除」という仕様には、1つ奇妙な動作を招く点があります。
問題が起こり得るのは、部分メソッドの呼び出しの際に引数で副作用を起こす場合です。

例えば、以下のコードの実行結果はどうなるでしょう。

<pre class="source" title="">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Main</span></span>(<span class="reserved">string</span>[] <span class="variable local">args</span>)
    {
        <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
        <span class="method"><span class="static">A</span></span>(<span class="variable">x</span> <span class="operator">=</span> <span class="number">2</span>);
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">Write</span></span>(<span class="string">&quot;{0}\n&quot;</span>, <span class="variable">x</span>);
    }

    <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="static"><span class="method">A</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>);
}
</pre>

`A` の実装がある場合には 2 に、ない場合には 1 になります。
部分クラスなので、当然、実装は別ファイルにあってもかまいません。
自分以外の誰かがどこか別のところで実装を書くかもしれませんし、
誰も書かないかもしれません。
要するに、自分の知らないところで実行結果が変えられてしまう可能性がある。

まあ、メソッド呼び出しの `()` 内で代入なんてするなよって話ではあるんですが。
こういう副作用があることも覚えておいてください。
（あるいは、この副作用を積極的に利用したトリッキーなコードも書けるでしょうが・・・。
個人的には非推奨。）

##<a id="sec-generated-title-6"></a> <a id="extended_partial_method">部分メソッドの拡張</a>
<h5 class="version version9">Ver. 9</h5>

[前節で説明した部分メソッド](#partial_method)は「開発ツールが生成したコードが先にあって、そこに手書き処理を足したいときに使う物」です。

一方、C# 9.0 世代では[ソースコード生成機能](analyzer-generator.md)が入ったことでこの逆があり得ます。
すなわち、「ソースコード生成してもらう前提で、手書きでは不完全な C# コードを書きたい」という場面が出てきました。

C# 9.0 では、そのための「不完全なメソッド」を書く方法として `partial` キーワードを再利用することにしました。
旧来の部分メソッドとの文法上の差は[アクセシビリティ](../oop/oo_conceal.md#level)修飾子(`public` とか `private` とか)を持つかどうかです。

<pre class="source" title="新旧・部分メソッド">
<span class="comment">// (1) ツールが事前に生成する想定のコード</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">PreGeneratedMethod</span>()
    {
        <span class="method">OnPreGeneratedMethod</span>();
 
        <span class="comment">// ツール生成のコード</span>
    }
 
    <span class="comment">// ツール生成のコードの前に何か手書き処理を足したければこのメソッドの中身を書く</span>
    <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">OnPreGeneratedMethod</span>();
}
 
<span class="comment">// (2) 手書き前提のコード</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
<span class="inactive">#if</span> DEBUG
    <span class="comment">// ツール生成コード前に、Debug 時のみログを仕込む。</span>
    <span class="comment">// これを書かなければ OnPreGeneratedMethod は呼ばれる痕跡すら残らない。</span>
    <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">OnPreGeneratedMethod</span>()
    {
        System.<span class="type">Console</span>.<span class="method">WriteLine</span>(
            <span class="string">&quot;PreGeneratedMethod が呼ばれた直後&quot;</span>
            + <span class="method">WantSourceGenerated</span>());
    }
<span class="inactive">#endif</span>
 
    <span class="comment">// 手書き C# コードが先にあって、これを元にソースコード生成してほしいメソッド。</span>
    <span class="reserved"><em>private</em></span> <span class="reserved">partial</span> <span class="reserved">string</span> <span class="method">WantSourceGenerated</span>();
}
 
<span class="comment">// (3) C# からのソースコード生成が前提のコード</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">private</span> <span class="reserved">partial</span> <span class="reserved">string</span> <span class="method">WantSourceGenerated</span>() =&gt; <span class="string">&quot;手書きはしづらしくて、ソースコード生成なら楽な文字列&quot;</span>;
}
</pre>

[コード解析・コード生成の利用](analyzer-generator.md#usage)で紹介している [StringLiteralGenerator](https://github.com/ufcpp/StringLiteralGenerator) はこの新しい部分メソッドを使っています。

ちなみに、コード生成と手書きの期待される順序が逆になっただけ(しかも文法上は非常に小さな差)ですが、結果的には結構できること・できないことが変わります。
以下の表に違いをまとめます。

<table>
<caption>新旧・部分メソッドの比較</caption>
<tr>
  <th>旧(アクセス修飾子なし)</th>
  <th>新(アクセス修飾子あり)</th>
</tr>
<tr>
  <td>アクセシビリティの指定は不可</td>
  <td>アクセシビリティの指定が必須(private 含む)</td>
</tr>
<tr>
  <td>戻り値は void のみ</td>
  <td>任意の戻り値を使える</td>
</tr>
<tr>
  <td>ref 引数、out 引数を持てない</td>
  <td>ref 引数、out 引数を持てる</td>
</tr>
<tr>
  <td>本体を持っていなくてもいい。なければ完全に消える。</td>
  <td>どこか1か所で本体を持たないとダメ。なければコンパイル エラー。</td>
</tr>
</table>

アクセシビリティ修飾子の有無だけでここまで差があることには少し抵抗があって、
文法をどうするかは C# チームも結構迷ったようです。
ただ、最終的には、下手にキーワードを追加したり全然違う文法を導入するよりはマシという判断が下りました。

#### <a id="sec-generated-title-7"></a>シグネチャの一致
部分メソッドは、宣言側と実装側でシグネチャ(引数リスト、戻り値の型、修飾子)が一致している必要があります。

アクセシビリティ、`static`, `readonly`, `ref` の有無が違うとエラーになります。

<pre class="source" title="シグネチャが合わなくてエラーになる例">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="static"><span class="method">M0</span></span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="static"><span class="method">M1</span></span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="method"><span class="static">M2</span></span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="static"><span class="method">M3</span></span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="static"><span class="method">M4</span></span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="static"><span class="method">M5</span></span>();
}

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="comment">// 全部一致。これは大丈夫。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="method"><span class="static">M0</span></span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();

    <span class="comment">// 戻り値の型が違うのは当然ダメ。エラー。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">byte</span> <span class="static"><span class="error" title="CS8817"><span class="method">M1</span></span></span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();

    <span class="comment">// 以下、修飾子のどこかが違う。全部エラー。</span>
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="method"><span class="static"><span class="error" title="CS8799">M2</span></span></span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="error" title="CS0763"><span class="method">M3</span></span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="method"><span class="static"><span class="error" title="CS8818"><span class="warning" title="CS8826">M4</span></span></span></span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="error" title="CS8818"><span class="static"><span class="method"><span class="warning" title="CS8826">M5</span></span></span></span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
}
</pre>

タプル要素名の差もエラーになります。

<pre class="source" title="タプル要素名が違うとエラー">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">public</span> <span class="reserved">partial</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="method">M0</span>((<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="variable local">t</span>);
    <span class="reserved">public</span> <span class="reserved">partial</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="method">M1</span>((<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="variable local">t</span>);
    <span class="reserved">public</span> <span class="reserved">partial</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="method">M2</span>((<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="variable local">t</span>);
}

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="comment">// 全部一致。これは大丈夫。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="method">M0</span>((<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="variable local">t</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="comment">// タプル要素名が違うとエラーに。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="method"><span class="error" title="CS8142">M1</span></span>((<span class="reserved">int</span> a, <span class="reserved">int</span> b) <span class="variable local">t</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
    <span class="reserved">public</span> <span class="reserved">partial</span> (<span class="reserved">int</span> a, <span class="reserved">int</span> b) <span class="error" title="CS8142"><span class="method">M2</span></span>((<span class="reserved">int</span> x, <span class="reserved">int</span> y) <span class="variable local">t</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span>;
}
</pre>


引数名、[null 許容参照型](../resource/nullablereferencetype.md)のアノテーションの差は警告になります。

<pre class="source" title="引数名、nullability の差は警告に">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">M0</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">y</span>);
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">y</span>);
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">y</span>);
}

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="comment">// 全部一致。これは大丈夫。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">M0</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">y</span>) { }

    <span class="comment">// 引数名が違う。警告。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method"><span class="warning" title="CS8826">M1</span></span>(<span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">y</span>) { }

    <span class="comment">// nullability が違う。警告。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="warning" title="CS8611"><span class="warning" title="CS8826"><span class="method">M2</span></span></span>(<span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">string</span> <span class="variable local">y</span>) { }
}
</pre>

一方で、属性は統合されます。

<pre class="source" title="属性は統合">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    [<span class="type">A</span>] <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">M</span>([<span class="type">A</span>] <span class="reserved">int</span> <span class="variable local">x</span>);
}

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    [<span class="type">B</span>] <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">void</span> <span class="method">M</span>([<span class="type">B</span>] <span class="reserved">int</span> <span class="variable local">x</span>) { }
}

<span class="comment">// [A, B] public partial void M([A, B] int x) { } と書いたのと同じになる。</span>

<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Attribute</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Attribute</span> { }
</pre>

###<a id="sec-generated-title-8"></a> <a id="partial_property">部分プロパティ</a>
<h5 class="version version13">Ver. 13</h5>

[C# 3.0 からある方の部分メソッド](#partial_method) は「戻り値なし(`void`)でないとダメ」という制約があって、
メソッド以外では元々役に立ちません。

一方、[C# 9.0 で拡張された方の部分メソッド](#extended_partial_method) は、
制約的にも用途的にも、[プロパティ](../oop/oo_property.md)や[インデクサー](../oop/oo_indexer.md)でも使えるはずです。
実際、工数の問題で後回しになっていただけで、C# 13 でめでたく部分プロパティ・部分インデクサーが実装されました。

<pre class="source" title="">
<span class="comment">// 元コード。</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="comment">// 部分プロパティ。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="property">PartialProprty</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="comment">// 部分インデクサー。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="comment">// コード生成で作ってもらう前提のコード。</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_field</span>;
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="property">PartialProprty</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_field</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_field</span> <span class="operator">=</span> <span class="reserved">value</span>; }

    <span class="reserved">private</span> <span class="reserved">int</span>[] <span class="field">_array</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">10</span>];
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_array</span>[<span class="variable local">index</span>]; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_array</span>[<span class="variable local">index</span>] <span class="operator">=</span> <span class="reserved">value</span>; }
}
</pre>

この機能の追加で特にうれしいのは [`GeneratedRegex`](https://learn.microsoft.com/ja-jp/dotnet/api/system.text.regularexpressions.generatedregexattribute) の存在でしょう。
C# 12 までは、以下のようにメソッドにする必要がありました。

<pre class="source" title="GeneratedRegex">
<span class="reserved">using</span> System<span class="operator">.</span>Text<span class="operator">.</span>RegularExpressions;

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">MyPatterns</span>
{
    [<span class="type">GeneratedRegex</span>(<span class="string">@&quot;\d{4}&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="type">Regex</span> <span class="static"><span class="method">FourDigits</span></span>();
}
</pre>

この属性を付けると、正規表現 `\d{4}` のマッチ処理を[ソースコード生成](analyzer-generator.md)で作ってくれます。
(普通に `new Regex(@"\d{4}")` と書くよりもだいぶパフォーマンスがよくなります。)

ただ、生成されるコードはメソッドよりもプロパティっぽいコード
(呼ぶたびに何か処理をするのではなく、最初の1回で作ったインスタンスをキャッシュして持っておいて、2回目からはほとんどノーコスト)になっています。
これまでは「部分プロパティがなかったからやむなくメソッドに付けていた」というだけで、
C# 13 と同世代の .NET 9 からはプロパティで同じことができるようになりました。

<pre class="source" title="GeneratedRegex をプロパティに付けれるようになった">
<span class="reserved">using</span> System<span class="operator">.</span>Text<span class="operator">.</span>RegularExpressions;

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">MyPatterns</span>
{
    [<span class="type">GeneratedRegex</span>(<span class="string">@&quot;\d{4}&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="type">Regex</span> <span class="property"><span class="static">FourDigits</span></span> { <span class="reserved">get</span>; } <span class="comment">// プロパティになった。</span>
}
</pre>

####<a id="sec-generated-title-9"></a> <a id="auto-property">プロパティの宣言と自動実装</a>
C# の文法上の紛らわしさなんですが、
プロパティに対して `{ get; set; }` などと書いたときの扱いには2種類あるので注意が必要です。

普通のクラスや構造体で `{ get; set; }` を書くとき、これは[自動実装](../oop/oo_property.md#auto)になります。

<pre class="source" title="自動実装の get; set;">
<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 自動実装の意味。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="comment">// コンパイラーが裏でフィールドを1個作って、</span>
    <span class="comment">// public int X { get =&gt; field; set =&gt; field = value; }</span>
    <span class="comment">// みたいなコードとして扱われる。</span>
}
</pre>

一方、[インターフェイス](../oop/oo_interface.md)や、[抽象メンバー](../oop/oo_abstract.md)の場合、「宣言だけある」という扱いになります。

<pre class="source" title="abstract の get; set;">
<span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="comment">// 宣言(「このプロパティは get も set も持っていてほしい」という意思表示のみ)。</span>
    <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// これも宣言のみ。</span>
    <span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

部分プロパティの場合は後者の意味になります。

<pre class="source" title="partial の get; set;">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 宣言(「partial の片割れで get も set も実装してほしい」という意思表示)。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9248">X</span></span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

「片方が宣言、片方が自動実装」みたいなことにはならないので、
以下のコードはコンパイル エラーを起こします。

<pre class="source" title="実装がいない問題">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 宣言。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9248">X</span></span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// こっちも宣言。</span>
    <span class="comment">// なので、実装がいなくてエラーになる。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">int</span> <span class="error" title="CS0102"><span class="error" title="CS9250"><span class="property">X</span></span></span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

###<a id="sec-generated-title-10"></a> <a id="partial-event">部分イベントと部分コンストラクター</a>
[部分プロパティ](#partial_property) (C# 13)に続いて、
C# 14 では[イベント](../functional/sp_event.md)と[コンストラクター](../oop/oo_construct.md)も部分定義できるようになりました。
(これも「工数の問題で後回しになっていただけ」の類です。)

<pre class="source" title="部分イベントと部分コンストラクターの例">
<span class="comment">// 元コード(手書き想定)。</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="comment">// 部分イベント。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">event</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> PartialEvent;

    <span class="comment">// 部分コンストラクター。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="type">PartialClass</span>();
}

<span class="comment">// コード生成で作ってもらう前提のコード。</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">private</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="field">_partialEvent</span>;
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">event</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> PartialEvent
    {
        <span class="reserved">add</span> <span class="operator">=&gt;</span> <span class="field">_partialEvent</span> <span class="operator">+=</span> <span class="reserved">value</span>;
        <span class="reserved">remove</span> <span class="operator">=&gt;</span> <span class="field">_partialEvent</span> <span class="operator">-=</span> <span class="reserved">value</span>;
    }

    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="type">PartialClass</span>() { }
}
</pre>


##<a id="sec-generated-title-11"></a> <a id="contextual-partial-keyword">partial キーワードの位置</a>
部分クラス・部分メソッドの仕様は C# 2.0 から追加されたものです。
`partial`というキーワードも 2.0 からの後付けなわけで、完全に予約語(変数などの名前に使えない単語)にしてしまうと、1.0 時代に書かれたコードを壊す可能性がありました。

そこで、`partial` は[文脈キーワード](../appendix/ap_reserved.md#context)になっています。
`partial`という単語がキーワード扱いされるのは、`class`、`struct`、`interface`、`void`の直前だけです。
(前節の[拡張部分メソッド](../oop/oo_class.md#extended_partial_method)の場合は戻り値の型の直前だけ。)
その結果、以下のように、語順に制約があります。

<pre class="source" title="partial には語順に制約がある">
<span class="comment">// OK</span>
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Ok1</span> { }
<span class="reserved">static</span> <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Ok2</span> { }

<span class="comment">// コンパイル エラー</span>
<span class="reserved">public</span> <span class="reserved"><span class="error">partial</span></span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Ng1</span> { }
<span class="reserved"><span class="error">partial</span></span> <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Ng2</span> { }
<span class="reserved">static</span> <span class="reserved"><span class="error">partial</span></span> <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Ng3</span> { }

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">X</span>
{
    <span class="comment">// OK</span>
    <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">void</span> Ok();

    <span class="comment">// コンパイル エラー</span>
    <span class="reserved"><span class="error">partial</span></span> <span class="reserved">static</span> <span class="reserved">void</span> Ng();
}
</pre>
