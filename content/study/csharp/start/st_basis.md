---
title: "C#のプログラムの基本構造"
source_url: "https://ufcpp.net/study/csharp/start/st_basis/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2021-09-05T00:00:00"
tags: []
umbraco_id: 1191
parent_id: 1190
sort_order: 0
aliases:
  - "/csharp/st_basis"
  - "/csharp/st_basis.html"
  - "/csharp/start/st_basis/"
  - "/study/csharp/st_basis"
  - "/study/csharp/st_basis.html"
---

# C#のプログラムの基本構造

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
##### <a id="sec-generated-title-2"></a>ポイント
* C# プログラムは Main から始まります。

* 「[クラス](../oop/oo_class.md#class)」とか「[名前空間](../structured/sp_namespace.md#namespace)」とかは、今のところおまじない（後々説明）。



##<a id="sec-generated-title-3"></a> <a id="sample"></a>C#の簡単なプログラム例
まずは C# を用いて書かれた簡単なプログラムを見てみましょう。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

<pre class="source" title="最も簡単なC#プログラム" lang="C#">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
<em>    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="comment">// 初めてC#を学ぶ方々にご挨拶</span>
        <span class="type">Console</span>.WriteLine(<span class="literal">"皆様、はじめまして"</span>);
    }</em>
}
</code></pre>


</div>
<div>

<pre class="source" title="" lang="VB">
<code><span class="reserved">Module</span> <span class="type">Program</span>
 
    <span class="reserved">Sub</span> Main()
        <span class="comment">' 初めてVisual Basicを学ぶ方々にご挨拶</span>
        <span class="type">Console</span>.WriteLine((<span class="literal">"皆様、はじめまして"</span>)
    <span class="reserved">End</span> <span class="reserved">Sub</span>
 
<span class="reserved">End</span> <span class="reserved">Module</span>
</code></pre>


</div>
<div>

<pre class="source" title="" lang="F#">
<code><span class="comment">// 初めてF#を学ぶ方々にご挨拶</span>
<span class="reserved">open</span> System
Console.Write <span class="literal">"皆様、はじめまして"</span>
</code></pre>


</div>
<div>

<pre class="source" title="" lang="C++">
<code><span class="reserved">#include</span> <span class="literal">"stdafx.h"</span>
 
<span class="reserved">using</span> <span class="reserved">namespace</span> System;
 
<span class="reserved">int</span> main(<span class="reserved">array</span>&lt;System::String ^&gt; ^args)
{
  <span class="comment">// 初めてC++/CLIを学ぶ方々にご挨拶</span>
    Console::WriteLine(L<span class="literal">"皆様、はじめまして"</span>);
    <span class="reserved">return</span> 0;
}
</code></pre>


</div>
</div>


これからしばらくの間は <code>using</code> とか <code>class</code> という部分のことは忘れて、
背景色を変えて強調してある部分だけを注目してください。

<em>
        C#のプログラムは、すべてこの<code>Main</code>と書いてある部分から始まります
      </em>。
このプログラムは、画面(DOSプロンプト中)に“皆様、始めまして。”という文字を表示します。
<em>
        <code>Console.Write</code>は文字や数値を画面に出力するためのもの
      </em>で、詳しくは「[ライブラリ](../structured/st_library.md)」で説明します。
また、<em>
        <code>//</code>から始まる行はコメント
      </em>で、プログラムの動作とは関係ありません。詳しくは「[コメント](st_comment.md)」で説明します。

ちなみに、
<code>using</code>は「[名前空間](../structured/sp_namespace.md)」で、
<code>class</code>は「[クラス](../oop/oo_class.md)」で、
<code>public</code>は「[実装の隠蔽](../oop/oo_conceal.md)」で、
<code>static</code>は「[静的メンバー](../oop/oo_static.md)」で、
<code>void</code>は「[関数](../structured/st_function.md)」で説明していきます。

###<a id="sec-generated-title-4"></a> <a id="top-level-statements"></a>C# 9.0 から
<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 からは、上記のコードを以下のように書くことができます。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="comment">// 初めてC#を学ぶ方々にご挨拶</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;皆様、はじめまして&quot;</span>);
</code></pre>

`namespace` とか `class` とかを飛ばして、書きたい処理を直接ファイル直下に書くことができるようになりました。
詳しくは「[トップ レベル ステートメント](../misc/miscentrypoint.md#top-level-statements)」で説明します。

###<a id="sec-generated-title-5"></a> <a id="global-using"></a>C# 10.0 から
<h5 class="version version10">Ver. 10.0</h5>

C# 10.0 からは、さらに、以下のように縮めて書くことができます。

<pre class="source" title="">
<code><span class="comment">// 初めてC#を学ぶ方々にご挨拶</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;皆様、はじめまして&quot;</span>);
</code></pre>

`using` も消えました。
詳しくは「[global using](../structured/sp_namespace.md#global-using)」で説明します。

##<a id="sec-generated-title-6"></a> <a id="gui"></a>GUIプログラム例
C# では GUI (Graphical User Interface: 要するに、Windowsなどのようにボタンやメニューなどをマウスで操作するようなもの)プログラミングも行えます。

GUI プログラムは文字ベース(CUI: Character User Interfaceという)のプログラムに比べて煩雑な処理が多く、難しいので、ここでは例を挙げるにとどめます。

<h5 class="version version3">Ver. 3.0</h5>
ちなみに、この例は、.NET Framework 3.0、C# 3.0 以降で動きます。
詳しくは、「[Windows Presentation Foundation](../../dotnet/index.md#wpf)」で説明します。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
</ul>
<div>

<pre class="source" title="GUI プログラム例（WPF）" lang="C#">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Windows;
<span class="reserved">using</span> System.Windows.Controls;

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    [<span class="type">STAThread</span>]
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> button = <span class="reserved">new</span> <span class="type">Button</span> { Content = <span class="literal">"ここを押せ"</span> };
        button.Click += (sender, e) =&gt; <span class="type">MessageBox</span>.Show(<span class="literal">"ようこそ"</span>);

        <span class="reserved">var</span> win = <span class="reserved">new</span> <span class="type">Window</span>
        {
            Title = <span class="literal">"サンプルプログラム"</span>,
            Width = 300,
            Height = 200,
            Content = button,
        };

        <span class="reserved">var</span> app = <span class="reserved">new</span> <span class="type">Application</span>();
        app.Run(win);
    }
}
</code></pre>


</div>
<div>

<pre class="source" title="" lang="VB">
<code><span class="reserved">Module</span> <span class="type">VBSample</span>

    <span class="reserved">Sub</span> Main()
        <span class="reserved">Dim</span> button = <span class="reserved">New</span> <span class="type">Button</span> <span class="reserved">With</span> {.Content = <span class="literal">"ここを押せ"</span>}
        <span class="reserved">AddHandler</span> <span class="type">button</span>.Click, <span class="reserved">Function</span>(sender, args) {<span class="type">MessageBox</span>.Show(<span class="literal">"ようこそ"</span>)}

        <span class="reserved">Dim</span> win = <span class="reserved">New</span> <span class="type">Window</span> <span class="reserved">With</span>
                  {
                      .Title = <span class="literal">"サンプルプログラム"</span>,
                      .Width = 300,
                      .Height = 200,
                      .Content = button
                  }

        <span class="reserved">Dim</span> app = <span class="reserved">New</span> <span class="type">Application</span>()
        <span class="type">app</span>.Run(win)
    <span class="reserved">End</span> <span class="reserved">Sub</span>

<span class="reserved">End</span> <span class="reserved">Module</span>
</code></pre>


</div>
<div>

<pre class="source" title="" lang="F#">
<code><span class="reserved">open</span> System
<span class="reserved">open</span> System.Windows
<span class="reserved">open</span> System.Windows.Controls
 
<span class="reserved">let</span> button = <span class="reserved">new</span> Button(Content = <span class="literal">"ここを押せ"</span>)
button.Click.Add(<span class="reserved">fun</span> x <span class="reserved">-&gt;</span> MessageBox.Show(<span class="literal">"ようこそ"</span>) |&gt; ignore)

<span class="reserved">let</span> win = <span class="reserved">new</span> Window(
                     Title = <span class="literal">"サンプルプログラム"</span>,
                     Width = 300.0,
                     Height = 200.0,
                     Content = button)

[&lt;STAThread&gt;]
<span class="reserved">do</span>
    <span class="reserved">let</span> app = <span class="reserved">new</span> Application()
    app.Run(win) |&gt; ignore
</code></pre>


</div>
</div>


<figure>
	[![C# 3.0 WPF によるGUIプログラムの例](../../../../assets/media/ufcpp2000/csharp/fig/wpfwelcome.png)](../../../../assets/media/ufcpp2000/csharp/fig/wpfwelcome.png)
	<figcaption>C# 3.0 WPF によるGUIプログラムの例</figcaption>
</figure>


<span class="expand-button" title="展開/折畳">（古いコード（Windows Forms））</span>
<div class="expand-panel" markdown="1" title="（古いコード（Windows Forms））">
      
.NET Framework 2.0 時代の古いコードも残しておきます。
詳しくは、「[GUI アプリケーション](../lib/lib_forms.md)」で説明します。

      
<pre class="source" title="C#によるGUIプログラムの例" lang="">
<code><span class="reserved">namespace</span> CsharpSample
{
  <span class="reserved">using</span> System;
  <span class="reserved">using</span> System.Windows.Forms;
  <span class="reserved">using</span> System.Drawing;

  <span class="comment">/// &lt;summary&gt;
  /// ボタンが1つ付いたウィンドウを作成し、
  /// ボタンを押したときに「ようこそ。」と書かれたメッセージボックスを表示
  /// &lt;/summary&gt;</span>
  <span class="reserved">class</span> WelcomeForm : Form
  {
    Button button;

    WelcomeForm()
    {
      <span class="comment">// ウィンドウ内にボタンをひとつ作成</span>
      <span class="reserved">this</span>.Text       = <span class="literal">"サンプルプログラム"</span>;
      <span class="reserved">this</span>.ClientSize = <span class="reserved">new</span> Size(256, 64);

      <span class="reserved">this</span>.button = <span class="reserved">new</span> Button();
      <span class="reserved">this</span>.button.Location = <span class="reserved">new</span> Point(80, 16);
      <span class="reserved">this</span>.button.Size     = <span class="reserved">new</span> Size(96, 32);
      <span class="reserved">this</span>.button.Text     = <span class="literal">"ここを押せ"</span>;
      <span class="reserved">this</span>.button.Click   += <span class="reserved">new</span> EventHandler(button_Click);
      <span class="reserved">this</span>.Controls.Add(<span class="reserved">this</span>.button);
    }

    <span class="comment">// ボタンが押されたときの処理</span>
    <span class="reserved">private void</span> button_Click(object sender, System.EventArgs e)
    {
      MessageBox.Show(<span class="literal">"ようこそ。"</span>);
    }

    <span class="reserved">static void</span> Main() 
    {
      Application.Run(<span class="reserved">new</span> WelcomeForm());
    }
  }
}
</code></pre>


      
このサンプルプログラムでは、ボタンがひとつあるウィンドウが表示され、
ボタンを押すと“ようこそ。”というメッセージが表示されます。

      
<figure>
	[![C#によるGUIプログラムの例](../../../../assets/media/ufcpp2000/csharp/fig/guiwelcome.png)](../../../../assets/media/ufcpp2000/csharp/fig/guiwelcome.png)
	<figcaption>C#によるGUIプログラムの例</figcaption>
</figure>


      
ちなみに、
Visual Studio でこのソースをコンパイルする場合、
「Windows フォームアプリケーション」プロジェクトにしてください。
また、コマンドラインで csc を使ってコンパイルする場合、
（ソースファイルの名前を WelcomeForm.cs とすると）以下のようなコマンドでコンパイルします。

      
<pre class="console" title="コマンドラインで csc を使ってコンパイルする場合">
csc /r:system.windows.forms.dll /r:system.drawing.dll /t:winexe WelcomeForm.cs
</pre>
   
</div>

##<a id="sec-generated-title-7"></a> <a id="web"></a>Webアプリ例
<h5 class="version version10">Ver. 10</h5>

C# 10.0/ .NET 6 世代では、Webアプリ開発を以下のような十数行のコードから始められるようになりました。

<pre class="source" title="">
<code><span class="reserved">var</span> builder = <span class="type">WebApplication</span>.<span class="method">CreateBuilder</span>(<span class="variable">args</span>);
<span class="reserved">var</span> app = builder.<span class="method">Build</span>();

<span class="control">if</span> (app.Environment.<span class="method">IsDevelopment</span>())
{
    app.<span class="method">UseDeveloperExceptionPage</span>();
}

app.<span class="method">MapGet</span>(<span class="string">"/"</span>, () =&gt; <span class="string">"Hello World!"</span>);

app.<span class="method">Run</span>();
</code></pre>

![.NET 6 からの「最小限の Web アプリ」テンプレートの実行結果の例](../../../../assets/media/1190/dotnet6webapp.png)
