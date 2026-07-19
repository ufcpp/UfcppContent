---
title: "WPF のコンテナ（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_container/"
content_type: "Article"
published_at: "2007-05-02T00:00:00"
updated_at: "2015-05-06T14:14:29"
tags: []
umbraco_id: 1398
parent_id: 1393
sort_order: 4
aliases:
  - "/dotnet/wpf/wpf_container/"
  - "/dotnet/wpf_container"
  - "/dotnet/wpf_container.html"
  - "/study/dotnet/wpf_container"
  - "/study/dotnet/wpf_container.html"
---

# WPF のコンテナ（WPF）

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
WPF では、
コントロール（ボタンやテキストボックス）などの配置を容易にするために、
配置制御のためのコンテナがいくつか用意されています。


##<a id="sec-generated-title-2"></a> <a id="Canvas"></a>Canvas
まず、一番分かりやすいのは Canvas でしょうか。
Canvas では、
Canvas の左上からの相対座標を直接指定して子要素を配置します。

座標は、以下のように、Canvas.Left, Canvas.Top を使って指定します。


<pre class="xsource" title="Canvas の例">
<code><span class="bracket">&lt;</span><span class="element">Canvas</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span>
  <span class="attribute">Background</span><span class="attvalue">="LightGray"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span>
    <span class="attribute">Canvas.Left</span><span class="attvalue">="5"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span>
    <span class="attribute">Text</span><span class="attvalue">="text 1"</span> <span class="attribute">Background</span><span class="attvalue">="#ffffcc"</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span>
    <span class="attribute">Canvas.Left</span><span class="attvalue">="5"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="105"</span>
    <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span>
    <span class="attribute">Text</span><span class="attvalue">="text 2"</span> <span class="attribute">Background</span><span class="attvalue">="#ffccff"</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span>
    <span class="attribute">Canvas.Left</span><span class="attvalue">="105"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span>
    <span class="attribute">Text</span><span class="attvalue">="text 3"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffff"</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span>
    <span class="attribute">Canvas.Left</span><span class="attvalue">="105"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="105"</span>
    <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span>
    <span class="attribute">Text</span><span class="attvalue">="text 4"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffcc"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Canvas</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![Canvas の例](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_canvas.png)](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_canvas.png)
	<figcaption>Canvas の例</figcaption>
</figure>



##<a id="sec-generated-title-3"></a> <a id="StackPanel"></a>StackPanel
StackPanel による配置はいたってシンプルで、
上から順に、幅いっぱいに詰め込んでいくだけです。


<pre class="xsource" title="StackPanel の例">
<code><span class="bracket">&lt;</span><span class="element">StackPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span>
  <span class="attribute">Background</span><span class="attvalue">="LightGray"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="1"</span> <span class="attribute">Background</span><span class="attvalue">="#ffffcc"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="2"</span> <span class="attribute">Background</span><span class="attvalue">="#ffccff"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="3"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffff"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="4"</span> <span class="attribute">Background</span><span class="attvalue">="#ffcccc"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="5"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffcc"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="6"</span> <span class="attribute">Background</span><span class="attvalue">="#ccccff"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![StackPanel の例](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_stackpanel.png)](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_stackpanel.png)
	<figcaption>StackPanel の例</figcaption>
</figure>


<code>Orientation="Horizontal"</code> という属性値を入れると、
左から右に並べることもできます。


<pre class="xsource" title="StackPanel（左から右） の例">
<code><span class="bracket">&lt;</span><span class="element">StackPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span>
  <span class="attribute">Background</span><span class="attvalue">="LightGray"</span> <span class="attribute">Orientation</span><span class="attvalue">="Horizontal"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="1"</span> <span class="attribute">Background</span><span class="attvalue">="#ffffcc"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="2"</span> <span class="attribute">Background</span><span class="attvalue">="#ffccff"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="3"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffff"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="4"</span> <span class="attribute">Background</span><span class="attvalue">="#ffcccc"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="5"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffcc"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="6"</span> <span class="attribute">Background</span><span class="attvalue">="#ccccff"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![StackPanel（左から右） の例](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_stackpanelh.png)](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_stackpanelh.png)
	<figcaption>StackPanel（左から右） の例</figcaption>
</figure>



##<a id="sec-generated-title-4"></a> <a id="WrapPanel"></a>WrapPanel
WrapPanel は、HTML みたいに、
左詰で子要素を配置していき、
右端で折り返します。
WrapPanel がリサイズされた場合、
折り返し位置が変化します。


<pre class="xsource" title="WrapPanel の例">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span>
  <span class="attribute">Background</span><span class="attvalue">="LightGray"</span> <span class="attribute">Orientation</span><span class="attvalue">="Horizontal"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="1"</span> <span class="attribute">Background</span><span class="attvalue">="#ffffcc"</span>
    <span class="attribute">Width</span><span class="attvalue">="30"</span> <span class="attribute">Height</span><span class="attvalue">="20"</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="2"</span> <span class="attribute">Background</span><span class="attvalue">="#ffccff"</span>
    <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="3"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffff"</span>
    <span class="attribute">Width</span><span class="attvalue">="40"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="4"</span> <span class="attribute">Background</span><span class="attvalue">="#ffcccc"</span>
    <span class="attribute">Width</span><span class="attvalue">="50"</span> <span class="attribute">Height</span><span class="attvalue">="30"</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="5"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffcc"</span>
    <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="6"</span> <span class="attribute">Background</span><span class="attvalue">="#ccccff"</span>
    <span class="attribute">Width</span><span class="attvalue">="20"</span> <span class="attribute">Height</span><span class="attvalue">="60"</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![WrapPanel の例](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_wrappanel.png)](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_wrappanel.png)
	<figcaption>WrapPanel の例</figcaption>
</figure>



##<a id="sec-generated-title-5"></a> <a id="DockPanel"></a>DockPanel
DockPanel を使うと、
<code>Dock</code> 属性で指定した方向に子要素を貼り付けることができます。
例えば、<code>Dock="Top"</code> とすれば上側に張り付いて、左右いっぱいに表示されます。
DockPanel がリサイズされた場合、
<code>Dock</code> で指定した方向に張り付いたまま、子要素も自動的にリサイズされます。

例えば、左側にメニュー、
右上に広告欄、右下に本文を表示したりといったようなレイアウトにしたいときに使います。


<pre class="xsource" title="DockPanel の例">
<code><span class="bracket">&lt;</span><span class="element">DockPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span>
  <span class="attribute">Background</span><span class="attvalue">="LightGray"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="1"</span> <span class="attribute">Background</span><span class="attvalue">="#ffffcc"</span> <span class="attribute">DockPanel.Dock</span><span class="attvalue">="Top"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="2"</span> <span class="attribute">Background</span><span class="attvalue">="#ffccff"</span> <span class="attribute">DockPanel.Dock</span><span class="attvalue">="Left"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="3"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffff"</span> <span class="attribute">DockPanel.Dock</span><span class="attvalue">="Right"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="4"</span> <span class="attribute">Background</span><span class="attvalue">="#ffcccc"</span> <span class="attribute">DockPanel.Dock</span><span class="attvalue">="Bottom"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="5"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffcc"</span> <span class="attribute">DockPanel.Dock</span><span class="attvalue">="Left"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="6"</span> <span class="attribute">Background</span><span class="attvalue">="#ccccff"</span> <span class="attribute">DockPanel.Dock</span><span class="attvalue">="Right"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">DockPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![DockPanel の例](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_dockpanel.png)](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_dockpanel.png)
	<figcaption>DockPanel の例</figcaption>
</figure>



##<a id="sec-generated-title-6"></a> <a id="Grid"></a>Grid
Grid を使うと、
行の高さ、列の幅を指定して、
テーブル上に子要素を配置できます。

行や列の定義は <code>Grid.RowDefinitions</code>, <code>Grid.ColumnDefinitions</code> で行います。
どの子要素を何行何列目に置くかは、
<code>Grid.Row</code>, <code>Grid.Column</code> 属性で指定します（行、列の番号は 0 から始まる）。


<pre class="xsource" title="Grid の例">
<code><span class="bracket">&lt;</span><span class="element">Grid</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span>
  <span class="attribute">Background</span><span class="attvalue">="LightGray"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Grid.ColumnDefinitions</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> <span class="attribute">Width</span><span class="attvalue">="60"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> <span class="attribute">Width</span><span class="attvalue">="60"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> <span class="attribute">Width</span><span class="attvalue">="60"</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Grid.ColumnDefinitions</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Grid.RowDefinitions</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="60"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="60"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="60"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Grid.RowDefinitions</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="1"</span> <span class="attribute">Background</span><span class="attvalue">="#ffffcc"</span>
    <span class="attribute">Grid.Row</span><span class="attvalue">="1"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="2"</span> <span class="attribute">Background</span><span class="attvalue">="#ffccff"</span>
    <span class="attribute">Grid.Row</span><span class="attvalue">="0"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="3"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffff"</span>
    <span class="attribute">Grid.Row</span><span class="attvalue">="2"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="4"</span> <span class="attribute">Background</span><span class="attvalue">="#ffcccc"</span>
    <span class="attribute">Grid.Row</span><span class="attvalue">="0"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span> <span class="attribute">Grid.RowSpan</span><span class="attvalue">="2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="5"</span> <span class="attribute">Background</span><span class="attvalue">="#ccffcc"</span>
    <span class="attribute">Grid.Row</span><span class="attvalue">="2"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span> <span class="attribute">Grid.ColumnSpan</span><span class="attvalue">="2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Text</span><span class="attvalue">="6"</span> <span class="attribute">Background</span><span class="attvalue">="#ccccff"</span>
    <span class="attribute">Grid.Row</span><span class="attvalue">="1"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="2"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![Grid の例](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_grid.png)](../../../../assets/media/ufcpp2000/dotnet/fig/wpf_grid.png)
	<figcaption>Grid の例</figcaption>
</figure>



##<a id="sec-generated-title-7"></a> <a id="other"></a>その他
UniformGrid や TabPanel、ToolbarPanel なんてのもあるみたい。
