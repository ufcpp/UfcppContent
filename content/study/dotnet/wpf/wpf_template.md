---
title: "テンプレート（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_template/"
content_type: "Article"
published_at: "2007-06-17T00:00:00"
updated_at: "2015-05-06T14:14:38"
tags: []
umbraco_id: 1402
parent_id: 1393
sort_order: 8
aliases:
  - "/dotnet/wpf/wpf_template/"
  - "/dotnet/wpf_template"
  - "/dotnet/wpf_template.html"
  - "/study/dotnet/wpf_template"
  - "/study/dotnet/wpf_template.html"
---

# テンプレート（WPF）

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「[スタイル](wpf_xamladv.md#style)」で説明したように、
WPF では、
HTML に対する CSS と同じ要領で UI 要素のスタイルを指定できます。

スタイルに加えて、
コントロール（ボタンやラベル、リストボックスなど）に対しては、
テンプレートと機能を使って、さらに柔軟なカスタマイズが可能です。
テンプレートを使えば、
背景色や文字サイズどころか、
コントロールの表示方法そのものを変更することが可能です。


##<a id="sec-generated-title-2"></a> <a id="ControlTemplate"></a>コントロールテンプレート
Contorl クラス（Button などの親クラス）は Template という名前のプロパティ（ControlTemplate 型）を持っています。
この Template プロパティを設定することで、コントロールの表示方法を変更することができます。

例えば、以下のように書くことで、ボタンの見た目を四角と丸に変化させることができます。


<pre class="xsource" title="ControlTemplate">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.Template</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">TargetType</span><span class="attvalue">="Button"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Grid</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Fill</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Fill</span><span class="attvalue">="#ff8080"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.Template</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![コントロールテンプレート](../../../../assets/media/ufcpp2000/dotnet/fig/template01.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template01.png)
	<figcaption>コントロールテンプレート</figcaption>
</figure>


このような機能を<strong id="ControlTemplate" class="keyword">コントロールテンプレート</strong>（ControlTemplate）といいます。

Button や Label など、
多くのコントロールは中身（Content）を持っています。
上の例では、ボタンの中身である "test1" が表示されていません。
これを表示させるためには、ControlTemplate 中に、ContentPresenter というものを書き加えます。


<pre class="xsource" title="ContentPresenter">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.Template</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">TargetType</span><span class="attvalue">="Button"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Grid</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Fill</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Fill</span><span class="attvalue">="#ff8080"</span>/<span class="bracket">&gt;</span>
<em>          <span class="bracket">&lt;</span><span class="element">ContentPresenter</span> <span class="attribute">HorizontalAlignment</span><span class="attvalue">="Center"</span>
                            <span class="attribute">VerticalAlignment</span><span class="attvalue">="Center"</span>/<span class="bracket">&gt;</span></em>
        <span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.Template</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![ContentPresenter](../../../../assets/media/ufcpp2000/dotnet/fig/template02.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template02.png)
	<figcaption>ContentPresenter</figcaption>
</figure>


ControlTemplate は、リソース中に 書くこともできます。


<pre class="xsource" title="リソース中に ControlTemplate">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">x:Key</span><span class="attvalue">="buttonTemplate"</span> <span class="attribute">TargetType</span><span class="attvalue">="Button"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Grid</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Fill</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Fill</span><span class="attvalue">="#ff8080"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ContentPresenter</span> <span class="attribute">HorizontalAlignment</span><span class="attvalue">="Center"</span>
                          <span class="attribute">VerticalAlignment</span><span class="attvalue">="Center"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span>
    <span class="attribute">Template</span><span class="attvalue">="{StaticResource buttonTemplate}"</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
全てのボタンに対して一律テンプレートを適用したければ、スタイルと併用します。


<pre class="xsource" title="全てのボタンにテンプレートを適用">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">x:Key</span><span class="attvalue">="buttonTemplate"</span> <span class="attribute">TargetType</span><span class="attvalue">="Button"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Grid</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Fill</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Fill</span><span class="attvalue">="#ff8080"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ContentPresenter</span> <span class="attribute">HorizontalAlignment</span><span class="attvalue">="Center"</span>
                          <span class="attribute">VerticalAlignment</span><span class="attvalue">="Center"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Button}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Template"</span> <span class="attribute">Value</span><span class="attvalue">="{StaticResource buttonTemplate}"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![全てのボタンにテンプレートを適用](../../../../assets/media/ufcpp2000/dotnet/fig/template03.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template03.png)
	<figcaption>全てのボタンにテンプレートを適用</figcaption>
</figure>


また、テンプレートの適用先のプロパティ値をテンプレートに反映させるためには、
TemplateBinding マークアップ拡張を用います。


<pre class="xsource" title="TemplateBinding マークアップ拡張">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">x:Key</span><span class="attvalue">="buttonTemplate"</span> <span class="attribute">TargetType</span><span class="attvalue">="Button"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Grid</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Fill</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Ellipse</span> <em><span class="attribute">Fill</span><span class="attvalue">="{TemplateBinding Background}"</span></em>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ContentPresenter</span> <span class="attribute">HorizontalAlignment</span><span class="attvalue">="Center"</span>
                          <span class="attribute">VerticalAlignment</span><span class="attvalue">="Center"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Button}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Template"</span> <span class="attribute">Value</span><span class="attvalue">="{StaticResource buttonTemplate}"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span> <em><span class="attribute">Background</span><span class="attvalue">="#80ff80"</span></em>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span> <em><span class="attribute">Background</span><span class="attvalue">="#ffff80"</span></em>
    <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="test2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span> <em><span class="attribute">Background</span><span class="attvalue">="#80ffff"</span></em>
    <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span> <span class="attribute">Content</span><span class="attvalue">="test1"</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![TemplateBinding マークアップ拡張](../../../../assets/media/ufcpp2000/dotnet/fig/template04.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template04.png)
	<figcaption>TemplateBinding マークアップ拡張</figcaption>
</figure>


サンプル→

[VistaLikeButton.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/VistaLikeButton.xaml)
。
Windows Vista ライクなボタンの見た目にする。
XP で実行しても Vista っぽい見た目になるはず。


##<a id="sec-generated-title-3"></a> <a id="ItemsPanelTemplate"></a>アイテムコントロールのテンプレート
中身（Content）のないコントロールか、
中身が1つだけのコントロール（ContentControl）に加えて、
ListBox や ComboBox のように、複数の項目を一覧表示するためのコントロール（ItemsControl）もあります。

本題は次節の「[データテンプレート](#DataTemplate)」なんですが、
ItemsControl には、「[コントロールテンプレート](#ControlTemplate)」中で ContentPresenter の代わりに ItemsPresenter を使わないといけないというような違いがある他、
ItemsPanelTemplate というテンプレート機構もあるので、
先に軽く説明しておきます。

まず、最初に挙げたように、
ItemsControl の場合、ControlTemplate 中には ContentPresenter ではなく、
ItemsPresenter を記述します。
例えば、角を丸めた ListBox を作りたければ以下のようにします。


<pre class="xsource" title="ItemsPresenter">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">ListBox</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBox.Template</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type ListBox}"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Border</span> <span class="attribute">CornerRadius</span><span class="attvalue">="10"</span> <span class="attribute">BorderBrush</span><span class="attvalue">="#808080"</span> <span class="attribute">BorderThickness</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span>
          <em><span class="bracket">&lt;</span><span class="element">ItemsPresenter</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span></em>
        <span class="bracket">&lt;</span>/<span class="element">Border</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ListBox.Template</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBoxItem</span><span class="bracket">&gt;</span>1<span class="bracket">&lt;</span>/<span class="element">ListBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBoxItem</span><span class="bracket">&gt;</span>2<span class="bracket">&lt;</span>/<span class="element">ListBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBoxItem</span><span class="bracket">&gt;</span>3<span class="bracket">&lt;</span>/<span class="element">ListBoxItem</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">ListBox</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![ItemsPresenter](../../../../assets/media/ufcpp2000/dotnet/fig/template05.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template05.png)
	<figcaption>ItemsPresenter</figcaption>
</figure>


で、この ItemsPresenter の中身そのものの表示方法を変えたければ、
ItemsPanel プロパティ（ItemsPanelTemplate 型）を設定します。
例えば、ListBox の項目を、水平に並べたければ以下のようにします。


<pre class="xsource" title="ItemsPanel">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">ListBox</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBox.Template</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type ListBox}"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Border</span> <span class="attribute">CornerRadius</span><span class="attvalue">="10"</span> <span class="attribute">BorderBrush</span><span class="attvalue">="#808080"</span> <span class="attribute">BorderThickness</span><span class="attvalue">="1"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">ItemsPresenter</span> <span class="attribute">Margin</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">Border</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ListBox.Template</span><span class="bracket">&gt;</span>
<em>    <span class="bracket">&lt;</span><span class="element">ListBox.ItemsPanel</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ItemsPanelTemplate</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Horizontal"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ItemsPanelTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ListBox.ItemsPanel</span><span class="bracket">&gt;</span></em>
    <span class="bracket">&lt;</span><span class="element">ListBoxItem</span><span class="bracket">&gt;</span>1<span class="bracket">&lt;</span>/<span class="element">ListBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBoxItem</span><span class="bracket">&gt;</span>2<span class="bracket">&lt;</span>/<span class="element">ListBoxItem</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ListBoxItem</span><span class="bracket">&gt;</span>3<span class="bracket">&lt;</span>/<span class="element">ListBoxItem</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">ListBox</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![ItemsPanel](../../../../assets/media/ufcpp2000/dotnet/fig/template06.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template06.png)
	<figcaption>ItemsPanel</figcaption>
</figure>



##<a id="sec-generated-title-4"></a> <a id="DataTemplate"></a>データテンプレート
ListBox などの ItemsControl の類のクラスは、
ListBoxItem などを使ってアイテムを表示する方法の他に、
データバインディング機能を使って XML や データベース中のデータを一覧表示する機能も持っています。

ListBoxItem を使う場合、
各項目のテンプレートは、ListBoxItem の Template プロパティに ControlTemplate を指定すればできます。
一方で、データバインディングを使う場合には、
<strong id="DataTemplate" class="keyword">データテンプレート</strong>（DataTemplate）というものを使います。

まずは復習ですが、
ListBox では、
ItemsSource プロパティに XmlDataProvider を指定することで、
XML からデータを読み込んで一覧表示することができます。


<pre class="xsource" title="XML データを ListBox 中に一覧表示">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">XmlDataProvider</span> <span class="attribute">x:Key</span><span class="attvalue">="comics"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">x:XData</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">comics</span> <span class="attribute">xmlns</span>=""<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/2"</span><span class="bracket">&gt;</span>エム×ゼロ 3<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/2"</span><span class="bracket">&gt;</span>銀魂 18<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/8"</span><span class="bracket">&gt;</span>無敵看板娘Ｎ 4<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/17"</span><span class="bracket">&gt;</span>×××HOLIC 11<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/18"</span><span class="bracket">&gt;</span>絶対可憐チルドレン 9<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">comics</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">x:XData</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">XmlDataProvider</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">ListBox</span>
    <span class="attribute">ItemsSource</span><span class="attvalue">="{Binding <span class="attribute">Source</span>={StaticResource comics},
      <span class="attribute">XPath</span>=/comics/item}"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">ListBox</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![XML データを ListBox 中に一覧表示](../../../../assets/media/ufcpp2000/dotnet/fig/template07.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template07.png)
	<figcaption>XML データを ListBox 中に一覧表示</figcaption>
</figure>


ここで、
XML の各項目に対してテンプレートを適用したければ、
以下のように、ItemTemplate プロパティに DataTemplate を設定します。


<pre class="xsource" title="DataTemplate">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">XmlDataProvider</span> <span class="attribute">x:Key</span><span class="attvalue">="comics"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">x:XData</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">comics</span> <span class="attribute">xmlns</span>=""<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/2"</span><span class="bracket">&gt;</span>エム×ゼロ 3<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/2"</span><span class="bracket">&gt;</span>銀魂 18<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/8"</span><span class="bracket">&gt;</span>無敵看板娘Ｎ 4<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/17"</span><span class="bracket">&gt;</span>×××HOLIC 11<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">item</span> <span class="attribute">date</span><span class="attvalue">="2007/5/18"</span><span class="bracket">&gt;</span>絶対可憐チルドレン 9<span class="bracket">&lt;</span>/<span class="element">item</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">comics</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">x:XData</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">XmlDataProvider</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">ListBox</span>
    <span class="attribute">ItemsSource</span><span class="attvalue">="{Binding <span class="attribute">Source</span>={StaticResource comics},
      <span class="attribute">XPath</span>=/comics/item}"</span><span class="bracket">&gt;</span>
<em>    <span class="bracket">&lt;</span><span class="element">ListBox.ItemTemplate</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">DataTemplate</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Horizontal"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="{Binding <span class="attribute">XPath</span>=@date}"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Content</span><span class="attvalue">="{Binding <span class="attribute">XPath</span>=text()}"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">DataTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ListBox.ItemTemplate</span><span class="bracket">&gt;</span></em>
    <span class="bracket">&lt;</span><span class="element">ListBox.Template</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ControlTemplate</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type ListBox}"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">StackPanel</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">StackPanel</span>  <span class="attribute">Orientation</span><span class="attvalue">="Horizontal"</span> <span class="attribute">Background</span><span class="attvalue">="#eeeeff"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Width</span><span class="attvalue">="100"</span> <span class="attribute">Content</span><span class="attvalue">="発売日"</span>/<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Content</span><span class="attvalue">="タイトル"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">ItemsPresenter</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ControlTemplate</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ListBox.Template</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">ListBox</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![XML データを ListBox 中に一覧表示](../../../../assets/media/ufcpp2000/dotnet/fig/template08.png)](../../../../assets/media/ufcpp2000/dotnet/fig/template08.png)
	<figcaption>XML データを ListBox 中に一覧表示</figcaption>
</figure>


見ての通り、
DataTemplate 中で XML 中の何を表示するかは、
Binding マークアップ拡張の XPath 属性を使って指定します。
