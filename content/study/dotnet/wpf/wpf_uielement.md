---
title: "WPF の GUI 要素（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_uielement/"
content_type: "Article"
published_at: "2007-05-02T00:00:00"
updated_at: "2007-05-04T00:00:00"
tags: []
umbraco_id: 1399
parent_id: 1393
sort_order: 5
aliases:
  - "/dotnet/wpf/wpf_uielement/"
  - "/dotnet/wpf_uielement"
  - "/dotnet/wpf_uielement.html"
  - "/study/dotnet/wpf_uielement"
  - "/study/dotnet/wpf_uielement.html"
---

# WPF の GUI 要素（WPF）

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
WPF で最初から用意されている GUI 要素や機能は膨大で、とても全部を紹介することはできませんが、
代表的なものをいくつか紹介します。


##<a id="sec-generated-title-2"></a> <a id="Controls"></a>コントロール
System.Windows.Controls 名前空間内に、
ボタン、テキストボックス、チェックボックス、ラジオボタンなど、
ユーザからの入力操作を受け付けるためのコントロール類が定義されています。


<pre class="xsource" title="コントロールの例">
<code><span class="bracket">&lt;</span><span class="element">Grid</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Grid.RowDefinitions</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="25"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="25"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="25"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="25"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="25"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">RowDefinition</span> <span class="attribute">Height</span><span class="attvalue">="25"</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Grid.RowDefinitions</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Grid.ColumnDefinitions</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> <span class="attribute">Width</span><span class="attvalue">="80"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> <span class="attribute">Width</span><span class="attvalue">="120"</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Grid.ColumnDefinitions</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Grid.Row</span><span class="attvalue">="0"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span><span class="bracket">&gt;</span>姓<span class="bracket">&lt;</span>/<span class="element">Label</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Grid.Row</span><span class="attvalue">="0"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span>/<span class="element">TextBox</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Grid.Row</span><span class="attvalue">="1"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span><span class="bracket">&gt;</span>名<span class="bracket">&lt;</span>/<span class="element">Label</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Grid.Row</span><span class="attvalue">="1"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span>/<span class="element">TextBox</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Grid.Row</span><span class="attvalue">="2"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span><span class="bracket">&gt;</span>年齢<span class="bracket">&lt;</span>/<span class="element">Label</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">ComboBox</span> <span class="attribute">Grid.Row</span><span class="attvalue">="2"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span> <span class="attribute">SelectedIndex</span><span class="attvalue">="0"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ComboBoxItem</span> <span class="bracket">&gt;</span>～19歳<span class="bracket">&lt;</span>/<span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>20代<span class="bracket">&lt;</span>/<span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>30代<span class="bracket">&lt;</span>/<span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>40代<span class="bracket">&lt;</span>/<span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>それ以上<span class="bracket">&lt;</span>/<span class="element">ComboBoxItem</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">ComboBox</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Grid.Row</span><span class="attvalue">="3"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span><span class="bracket">&gt;</span>性別<span class="bracket">&lt;</span>/<span class="element">Label</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">GroupBox</span> <span class="attribute">Grid.Row</span><span class="attvalue">="3"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">StackPanel</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RadioButton</span> <span class="attribute">Height</span><span class="attvalue">="18"</span><span class="bracket">&gt;</span>男<span class="bracket">&lt;</span>/<span class="element">RadioButton</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RadioButton</span> <span class="attribute">Height</span><span class="attvalue">="18"</span><span class="bracket">&gt;</span>女<span class="bracket">&lt;</span>/<span class="element">RadioButton</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">GroupBox</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">CheckBox</span> <span class="attribute">Grid.Row</span><span class="attvalue">="4"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span>既婚<span class="bracket">&lt;</span>/<span class="element">CheckBox</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Grid.Row</span><span class="attvalue">="5"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span>OK<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Grid.Row</span><span class="attvalue">="6"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span>Cancel<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![コントロールの例](../../../../assets/media/ufcpp2000/dotnet/fig/ui_controls.png)](../../../../assets/media/ufcpp2000/dotnet/fig/ui_controls.png)
	<figcaption>コントロールの例</figcaption>
</figure>



##<a id="sec-generated-title-3"></a> <a id="Shapes"></a>図形
System.Windows.Shapes 名前空間内に、
直線、円、多角形などの図形が定義されています。
これらの図形はベクタグラフィックになっていて、
拡大・縮小してもふちがギザギザになったりしません。


<pre class="xsource" title="図形の例">
<code><span class="bracket">&lt;</span><span class="element">Canvas</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="100"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="10"</span>
    <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span> <span class="attribute">Fill</span><span class="attvalue">="#ffcccc"</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="30"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="120"</span>
    <span class="attribute">Width</span><span class="attvalue">="60"</span> <span class="attribute">Height</span><span class="attvalue">="60"</span> <span class="attribute">Fill</span><span class="attvalue">="#ccccff"</span>/<span class="bracket">&gt;</span>
    
  <span class="bracket">&lt;</span><span class="element">Polygon</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="10"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="10"</span>
    <span class="attribute">Points</span><span class="attvalue">="20 10 70 20 80 40 60 70 10 50 0 30"</span>
    <span class="attribute">Fill</span><span class="attvalue">="#ccffcc"</span>
  /<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Line</span> <span class="attribute">Stroke</span><span class="attvalue">="#aaaaaa"</span> <span class="attribute">StrokeThickness</span><span class="attvalue">="3"</span>
    <span class="attribute">X1</span><span class="attvalue">="120"</span> <span class="attribute">Y1</span><span class="attvalue">="120"</span> <span class="attribute">X2</span><span class="attvalue">="180"</span> <span class="attribute">Y2</span><span class="attvalue">="180"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Line</span> <span class="attribute">Stroke</span><span class="attvalue">="#aaaaaa"</span> <span class="attribute">StrokeThickness</span><span class="attvalue">="3"</span>
    <span class="attribute">X1</span><span class="attvalue">="180"</span> <span class="attribute">Y1</span><span class="attvalue">="120"</span> <span class="attribute">X2</span><span class="attvalue">="120"</span> <span class="attribute">Y2</span><span class="attvalue">="180"</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Canvas</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![図形の例](../../../../assets/media/ufcpp2000/dotnet/fig/ui_shapes.png)](../../../../assets/media/ufcpp2000/dotnet/fig/ui_shapes.png)
	<figcaption>図形の例</figcaption>
</figure>



##<a id="sec-generated-title-4"></a> <a id="Media"></a>メディア
System.Windows.Media 名前空間内には多彩な機能が用意されています。

まず、
コントロールや図形の背景にグラデーションをかけたり画像を表示したり、
回転・拡大・平行移動などの変形を施す機能があります。

また、System.Windows.Shapes で定義されている基本的な図形に加えて、
ベジエ曲線等を用いた複雑な図形の描写機能があります。

さらに、静止画、音声、動画などを再生・表示する機能があります。


<pre class="xsource" title="グラデーションの例">
<code><span class="bracket">&lt;</span><span class="element">Canvas</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#808080"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="5"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="5"</span> <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#aaaaff"</span> <span class="attribute">Offset</span><span class="attvalue">="0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#aaffff"</span> <span class="attribute">Offset</span><span class="attvalue">="1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="105"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="5"</span> <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RadialGradientBrush</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffaaaa"</span> <span class="attribute">Offset</span><span class="attvalue">="0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffffaa"</span> <span class="attribute">Offset</span><span class="attvalue">="1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">RadialGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="5"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="105"</span> <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RadialGradientBrush</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffffff"</span> <span class="attribute">Offset</span><span class="attvalue">="0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffaaff"</span> <span class="attribute">Offset</span><span class="attvalue">="1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">RadialGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="105"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="105"</span> <span class="attribute">Width</span><span class="attvalue">="90"</span> <span class="attribute">Height</span><span class="attvalue">="90"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#aaffaa"</span> <span class="attribute">Offset</span><span class="attvalue">="0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#aaaaaa"</span> <span class="attribute">Offset</span><span class="attvalue">="1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Canvas</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![グラデーションの例](../../../../assets/media/ufcpp2000/dotnet/fig/ui_gradation.jpg)](../../../../assets/media/ufcpp2000/dotnet/fig/ui_gradation.jpg)
	<figcaption>グラデーションの例</figcaption>
</figure>



<pre class="xsource" title="回転・拡大・傾斜・平行移動の例">
<code><span class="bracket">&lt;</span><span class="element">Canvas</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#808080"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Line</span> <span class="attribute">X1</span><span class="attvalue">="100"</span> <span class="attribute">Y1</span><span class="attvalue">="0"</span> <span class="attribute">X2</span><span class="attvalue">="100"</span> <span class="attribute">Y2</span><span class="attvalue">="200"</span> <span class="attribute">Stroke</span><span class="attvalue">="Black"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Line</span> <span class="attribute">X1</span><span class="attvalue">="0"</span> <span class="attribute">Y1</span><span class="attvalue">="100"</span> <span class="attribute">X2</span><span class="attvalue">="200"</span> <span class="attribute">Y2</span><span class="attvalue">="100"</span> <span class="attribute">Stroke</span><span class="attvalue">="Black"</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="10"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="10"</span> <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ScaleTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="45"</span> <span class="attribute">CenterY</span><span class="attvalue">="45"</span> <span class="attribute">ScaleX</span><span class="attvalue">="0.5"</span> <span class="attribute">ScaleY</span><span class="attvalue">="0.5"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
    button 1
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="110"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="10"</span> <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">TranslateTransform</span> <span class="attribute">X</span><span class="attvalue">="-10"</span> <span class="attribute">Y</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
    button 2
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="10"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="110"</span> <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">SkewTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="45"</span> <span class="attribute">CenterY</span><span class="attvalue">="45"</span> <span class="attribute">AngleX</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
    button 3
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="110"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="110"</span> <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RotateTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="45"</span> <span class="attribute">CenterY</span><span class="attvalue">="45"</span> <span class="attribute">Angle</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.RenderTransform</span><span class="bracket">&gt;</span>
    button 4
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Canvas</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![回転・拡大・傾斜・平行移動の例](../../../../assets/media/ufcpp2000/dotnet/fig/ui_transform.jpg)](../../../../assets/media/ufcpp2000/dotnet/fig/ui_transform.jpg)
	<figcaption>回転・拡大・傾斜・平行移動の例</figcaption>
</figure>



###<a id="sec-generated-title-5"></a> <a id="Media3D"></a>3次元モデル
特に、
System.Windows.Media.Media3D 名前空間内には、
3次元モデルの表示機能があります。

カメラの向きを設定して、
光源を置いて、
3次元モデルを置く感じで、割と簡単に作れます。

3次元モデルの作り方は、
いわゆるメッシュ（多面体の頂点と、頂点のつなぎ方を指定して物体を作る）構造がメインのようです。
（以下の例では、頂点の座標を全部 XAML 中に打っていますが、
3次元モデル生成アプリで作ったデータを読んだりもできるようです。）

以下の例では、
正8面体を作って、3方向から指向性の光を当てています。
（本当は手抜きしてて、8面体の表から見える側だけ作ってる。）


<pre class="xsource" title="3次元モデル表示の例">
<code><span class="bracket">&lt;</span><span class="element">Canvas</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="Black"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Viewport3D</span> <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span><span class="bracket">&gt;</span>
    <span class="comment">&lt;!-- カメラ --&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Viewport3D.Camera</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">PerspectiveCamera</span> <span class="attribute">Position</span><span class="attvalue">="0,0,15"</span> <span class="attribute">FieldOfView</span><span class="attvalue">="10"</span>
        <span class="attribute">LookDirection</span><span class="attvalue">="0,0,-1"</span> <span class="attribute">UpDirection</span><span class="attvalue">="0, 1, 0"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Viewport3D.Camera</span><span class="bracket">&gt;</span>

    <span class="comment">&lt;!-- 物体 --&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ModelVisual3D</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ModelVisual3D.Content</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GeometryModel3D</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">GeometryModel3D.Geometry</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">MeshGeometry3D</span>
              <span class="attribute">Positions</span><span class="attvalue">="1 0 0, 0 1 0, -1 0 0, 0 -1 0, 0 0 1"</span>
              <span class="attribute">TriangleIndices</span><span class="attvalue">="0 1 4, 1 2 4, 2 3 4, 3 0 4"</span>
              /<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">GeometryModel3D.Geometry</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">GeometryModel3D.Material</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DiffuseMaterial</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">DiffuseMaterial.Brush</span><span class="bracket">&gt;</span>
                <span class="bracket">&lt;</span><span class="element">SolidColorBrush</span> <span class="attribute">Color</span><span class="attvalue">="White"</span>/<span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span>/<span class="element">DiffuseMaterial.Brush</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span>/<span class="element">DiffuseMaterial</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">GeometryModel3D.Material</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">GeometryModel3D</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ModelVisual3D.Content</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ModelVisual3D</span><span class="bracket">&gt;</span>

    <span class="comment">&lt;!-- 光源 --&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ModelVisual3D</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ModelVisual3D.Content</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Model3DGroup</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">AmbientLight</span> <span class="attribute">Color</span><span class="attvalue">="#404040"</span> /<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">DirectionalLight</span> <span class="attribute">Color</span><span class="attvalue">="#ff0000"</span> <span class="attribute">Direction</span><span class="attvalue">="-1,-1,0"</span> /<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">DirectionalLight</span> <span class="attribute">Color</span><span class="attvalue">="#0000ff"</span> <span class="attribute">Direction</span><span class="attvalue">="1,0,0"</span> /<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">DirectionalLight</span> <span class="attribute">Color</span><span class="attvalue">="#00ff00"</span> <span class="attribute">Direction</span><span class="attvalue">="1,-1,0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">Model3DGroup</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ModelVisual3D.Content</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ModelVisual3D</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Viewport3D</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Canvas</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![3次元モデル表示の例](../../../../assets/media/ufcpp2000/dotnet/fig/ui_viewport3d.jpg)](../../../../assets/media/ufcpp2000/dotnet/fig/ui_viewport3d.jpg)
	<figcaption>3次元モデル表示の例</figcaption>
</figure>


その他のサンプル →

[viewport3d.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/viewport3d.xaml)
。
正8面体を6個置いて、カメラを回して写しています。
