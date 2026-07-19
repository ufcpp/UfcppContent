---
title: "XAML の高度な機能（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_xamladv/"
content_type: "Article"
published_at: "2006-11-19T00:00:00"
updated_at: "2007-06-17T00:00:00"
tags: []
umbraco_id: 1397
parent_id: 1393
sort_order: 3
aliases:
  - "/dotnet/wpf/wpf_xamladv/"
  - "/dotnet/wpf_xamladv"
  - "/dotnet/wpf_xamladv.html"
  - "/study/dotnet/wpf_xamladv"
  - "/study/dotnet/wpf_xamladv.html"
---

# XAML の高度な機能（WPF）

## <a id="sec-generated-title-1"></a> <a id="dependency"></a>依存プロパティ

「[基本構造](wpf_xamlbasic.md#structure)」では、
XAML 中の XML タグの属性や子要素を通して、
クラスの「[プロパティ](../../csharp/oop/oo_property.md#property)」の値を設定できると説明しました。
これはより正確にいうと、
「普通のプロパティ<em>も</em>使える」となります。

WPF では、通常のプロパティでは実現できない機能を実装するために、
<strong id="dependency" class="keyword">依存プロパティ</strong>（dependency property）というものを用意しています。
XAML では、
通常のプロパティと同名の依存プロパティがある場合、
依存プロパティの方が優先されます。
（依存プロパティと区別する目的で、
通常のプロパティを「CLR プロパティ」と呼んだりもします。）

依存プロパティを使いたいクラスは DependencyObject クラスを継承する必要があります。
DependencyObject は、SetValue と GetValue というメソッドを持っていて、
以下のようにして依存プロパティの取得・設定を行います。

<pre class="source" title="DependencyObject の GetValue / SetValue" lang="">
<code><span class="reserved">object</span> val = GetValue(<span class="input">DependencyPropertyIdentifier</span>);
SetValue(<span class="input">DependencyPropertyIdentifier</span>, val);
</code></pre>


DependencyPropertyIdentifier の部分は、
DependencyProperty クラスのインスタンスを渡します。
この DependencyProperty クラスのインスタンスは、
static readonly なメンバー変数としてクラス中に定義します。
例えば、TextBlock という名前のクラス中に Text という名前の依存プロパティを作りたければ、以下のようにします。

<pre class="source" title="依存プロパティ識別子の定義" lang="">
<code><span class="reserved">public class</span> TextBlock
{
  <span class="reserved">public static readonly</span> DependencyProperty TextProperty =
    DependencyProperty.Register(<span class="literal">"Text"</span>, <span class="reserved">typeof</span>(<span class="reserved">string</span>), <span class="reserved">typeof</span>(TextBlock));
}
</code></pre>


通常、利便性のために、
同名の CLR プロパティも用意しておきます。

<pre class="source" title="同名のプロパティ定義" lang="">
<code><span class="reserved">public class</span> TextBlock : DependencyObject
{
  <span class="reserved">public static readonly</span> DependencyProperty TextProperty =
    DependencyProperty.Register(<span class="literal">"Text"</span>, <span class="reserved">typeof</span>(<span class="reserved">string</span>), <span class="reserved">typeof</span>(TextBlock));

  <span class="reserved">public string</span> Text
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> (<span class="reserved">string</span>)<span class="reserved">this</span>.GetValue(TextProperty); }
    <span class="reserved">set</span> { <span class="reserved">this</span>.SetValue(TextProperty, value); }
  }
}
</code></pre>


これで、XAML 中で、


<pre class="xsource" title="XAML から依存プロパティの値を設定">
<code><span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Name</span><span class="attvalue">="textBlock"</span> <span class="attribute">Text</span><span class="attvalue">="テキスト"</span> /<span class="bracket">&gt;</span>
</code></pre>
と書けば、（通常の Text プロパティよりも、依存プロパティの TextProperty が優先されて、）
以下のコードと同じ効果が得られます。

<pre class="source" title="同名のプロパティ定義" lang="">
<code>textBlock.SetValue(TextProperty, <span class="literal">"テキスト"</span>);
</code></pre>



## <a id="sec-generated-title-2"></a> <a id="attached"></a>添付プロパティ

「[依存プロパティ](#dependency)」にできて通常のプロパティにできないことの最たるものが、
<strong id="attached" class="keyword">添付プロパティ</strong>（attached property）です。

添付プロパティの例を示すのに、Grid を使ってみましょう。
Grid は、子要素をテーブル状にレイアウトするためのものです。
子要素を何行何列目に配置するかは、以下のように、
Grid.Row と Grid.Column を使って指定します。


<pre class="xsource" title="Grid と添付プロパティ">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Grid</span> <span class="attribute">Width</span><span class="attvalue">="120"</span> <span class="attribute">Height</span><span class="attvalue">="120"</span> <span class="attribute">Background</span><span class="attvalue">="Black"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Grid.ColumnDefinitions</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ColumnDefinition</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Grid.ColumnDefinitions</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Grid.RowDefinitions</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RowDefinition</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RowDefinition</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Grid.RowDefinitions</span><span class="bracket">&gt;</span>

    <span class="bracket">&lt;</span><span class="element">Border</span> <em><span class="attribute">Grid.Row</span><span class="attvalue">="0"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span></em>
      <span class="attribute">Width</span><span class="attvalue">="50"</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> <span class="attribute">Background</span><span class="attvalue">="Red"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Border</span> <em><span class="attribute">Grid.Row</span><span class="attvalue">="0"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span></em>
      <span class="attribute">Width</span><span class="attvalue">="50"</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> <span class="attribute">Background</span><span class="attvalue">="Green"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Border</span> <em><span class="attribute">Grid.Row</span><span class="attvalue">="1"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="0"</span></em>
      <span class="attribute">Width</span><span class="attvalue">="50"</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> <span class="attribute">Background</span><span class="attvalue">="Blue"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Border</span> <em><span class="attribute">Grid.Row</span><span class="attvalue">="1"</span> <span class="attribute">Grid.Column</span><span class="attvalue">="1"</span></em>
      <span class="attribute">Width</span><span class="attvalue">="50"</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> <span class="attribute">Background</span><span class="attvalue">="Yellow"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Grid</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
この例では、4色の四角形（Border）が2×2のテーブル状に配置されます。
見てのとおり、
4つの Border では、親要素である Grid の依存プロパティが設定されています。

このように、自分自身のクラス中のプロパティではなく、
親要素で定義されたプロパティ値を設定することを添付プロパティと呼びます。
添付プロパティは（通常のプロパティは使えず、）依存プロパティでなければいけません。
（というか、通常のプロパティではこのような仕組みは実現できません。）

「依存プロパティ」という名前は、添付プロパティの例のように、
「他のクラスとの依存関係を持つことができるプロパティ」というような意味合いです。


## <a id="sec-generated-title-3"></a> <a id="resource"></a>リソース

「[Attribute Syntax](wpf_xamlbasic.md#attribute)」 を使うにしろ、
「[Property Element Syntax](wpf_xamlbasic.md#property)」 を使うにしろ、
普通に値を設定した場合、
プロパティごと新しいインスタンスが作られます。

ところで、新しいインスタンスではなく、
既にあるインスタンスを参照したい場合もあります。
（例えば、どこかで1度定義したブラシを、複数のコントロールの背景で使いまわすとか。）
このような場合、
<strong id="resource" class="keyword">リソース</strong>というものを使います。

まず、リソースを定義する側ですが、
リソースは、どこかの要素（通常、リソースを使う要素自体か、ルート要素中）の
Resources プロパティ中で定義します。

例えば、&lt;Page&gt; 中でグラデーションブラシをリソース化する場合、
以下のようにします。


<pre class="xsource" title="リソースの例">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span><span class="bracket">&gt;</span>
  <em><span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span></em>
    <span class="bracket">&lt;</span><span class="element">LinearGradientBrush</span> <span class="attribute">x:Key</span><span class="attvalue">="brush1"</span>
      <span class="attribute">StartPoint</span><span class="attvalue">="0, 0"</span> <span class="attribute">EndPoint</span><span class="attvalue">="1, 1"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="Violet"</span> <span class="attribute">Offset</span><span class="attvalue">="0.0"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="Coral"</span> <span class="attribute">Offset</span><span class="attvalue">="1.0"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
  <em><span class="bracket">&lt;</span>/<span class="element">Page.Resources</span><span class="bracket">&gt;</span></em>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
リソースを参照するときのことを考えて、
x:Key 属性をつけておきます。

リソースを参照する側では、
「[Attribute Syntax](wpf_xamlbasic.md#attribute)」 の場合には、


<pre class="xsource" title="リソースの参照方法1">
<code><span class="bracket">&lt;</span><span class="element">object</span> <span class="attribute">property</span><span class="attvalue">="{StaticResource key}"</span> .../<span class="bracket">&gt;</span>
</code></pre>
「[Property Element Syntax](wpf_xamlbasic.md#property)」 の場合には、


<pre class="xsource" title="リソースの参照方法2">
<code><span class="bracket">&lt;</span><span class="element">object</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">object.property</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">StaticResource</span> <span class="attribute">ResourceKey</span><span class="attvalue">="key"</span> .../<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">object.property</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">object</span><span class="bracket">&gt;</span>
</code></pre>
というように書きます。
先ほどの例のグラデーションブラシをテキストブロックの背景として参照したい場合には、
以下のように書きます。


<pre class="xsource" title="リソースの参照">
<code><span class="bracket">&lt;</span><span class="element">TextBlock</span> <em><span class="attribute">Background</span><span class="attvalue">="{StaticResource brush1}"</span></em> <span class="attribute">Text</span><span class="attvalue">="textblock 1"</span>/<span class="bracket">&gt;</span>
</code></pre>

##### <a id="sec-generated-title-4"></a>サンプル

<pre class="xsource" title="リソースの例">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">LinearGradientBrush</span> <span class="attribute">x:Key</span><span class="attvalue">="brush1"</span>
      <span class="attribute">StartPoint</span><span class="attvalue">="0, 0"</span> <span class="attribute">EndPoint</span><span class="attvalue">="1, 1"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="Violet"</span> <span class="attribute">Offset</span><span class="attvalue">="0.0"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="Coral"</span> <span class="attribute">Offset</span><span class="attvalue">="1.0"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">LinearGradientBrush</span> <span class="attribute">x:Key</span><span class="attvalue">="brush2"</span>
      <span class="attribute">StartPoint</span><span class="attvalue">="0, 0"</span> <span class="attribute">EndPoint</span><span class="attvalue">="1, 1"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="Turquoise"</span> <span class="attribute">Offset</span><span class="attvalue">="0.0"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="Gainsboro"</span> <span class="attribute">Offset</span><span class="attvalue">="1.0"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Page.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Background</span><span class="attvalue">="{StaticResource brush1}"</span> <span class="attribute">Text</span><span class="attvalue">="textblock 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Background</span><span class="attvalue">="{StaticResource brush2}"</span> <span class="attribute">Text</span><span class="attvalue">="textblock 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span>    <span class="attribute">Background</span><span class="attvalue">="{StaticResource brush1}"</span> <span class="attribute">Content</span><span class="attvalue">="button 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBox</span>   <span class="attribute">Background</span><span class="attvalue">="{StaticResource brush2}"</span> <span class="attribute">Text</span><span class="attvalue">="textbox 1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>

### <a id="sec-generated-title-5"></a> <a id="extern_resource"></a>外部リソース

Resources プロパティの型は ResourceDictionary なんですが、
ResourceDictionary の Source プロパティを指定することで、
外部の XAML からリソースを読みこむことができます。

例えば、まず、以下のような XAML を StyleForLabel.xaml という名前で保存して、


<pre class="xsource" title="StyleForLabel.xaml">
<code><span class="bracket">&lt;</span><span class="element">ResourceDictionary</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Label}"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Background"</span> <span class="attribute">Value</span><span class="attvalue">="#eeeeff"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">ResourceDictionary</span><span class="bracket">&gt;</span>
</code></pre>
以下のような XAML を書くと、
StyleForLabel.xaml 中の設定が反映されます。


<pre class="xsource" title="ResourceDictionary の Source プロパティ">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ResourceDictionary</span> <span class="attribute">Source</span><span class="attvalue">="StyleForLabel.xaml"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Content</span><span class="attvalue">="label 1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Content</span><span class="attvalue">="label 2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Content</span><span class="attvalue">="label 3"</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
（スタイルに関しては、「[スタイル](#style)」で説明します。）

複数の外部リソースをマージしたければ、
以下のように、MergedDictionaries プロパティを設定します。


<pre class="xsource" title="MergedDictionaries">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ResourceDictionary</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ResourceDictionary.MergedDictionaries</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ResourceDictionary</span> <span class="attribute">Source</span><span class="attvalue">="StyleForButton.xaml"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ResourceDictionary</span> <span class="attribute">Source</span><span class="attvalue">="StyleForLabel.xaml"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">ResourceDictionary.MergedDictionaries</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">ResourceDictionary</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Content</span><span class="attvalue">="label 1"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Content</span><span class="attvalue">="label 2"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 3"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Label</span> <span class="attribute">Content</span><span class="attvalue">="label 3"</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
実物 →

[ResourceDictionary.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/ResourceDictionary.xaml)
。
リソースファイル →

[StyleForButton.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/StyleForButton.xaml)
、

[StyleForLabel.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/StyleForLabel.xaml)
。
（今のところ説明していない機能使いまくってます。
詳細は、「[アニメーション（WPF）](wpf_xamlani.md)」などで説明します。）


## <a id="sec-generated-title-6"></a> <a id="extension"></a>マークアップ拡張

「[プロパティの設定](wpf_xamlbasic.md#property)」で説明したとおり、
「[Attribute Syntax](wpf_xamlbasic.md#attribute)」 を使ってプロパティの値を設定する場合、
属性の値は文字列もしくは文字列から直接変換可能な型として扱われます。
このような単純な仕様に加えて、
XAML では、<strong id="extension" class="keyword">マークアップ拡張</strong>（markup extension）という高度な仕組みを持っています。

実は、前節ですでにマークアップ拡張を使っています。
マークアップ拡張を使って実現できるもっとも簡単な例は、リソースの参照、
すなわち、StaticResource です。
実は、StaticResource の仕組みは、
StaticResourceExtension というクラスによって提供されています。
例えば、前節の例、


<pre class="xsource" title="リソースの参照">
<code><span class="bracket">&lt;</span><span class="element">TextBlock</span>
  <span class="attribute">Name</span><span class="attvalue">="textblock1"</span>
  <span class="attribute">Background</span><span class="attvalue">="{StaticResource brush1}"</span>
  <span class="attribute">Text</span><span class="attvalue">="textblock 1"</span>/<span class="bracket">&gt;</span>
</code></pre>
の場合、以下のようなコードと同じような意味合いになります。

<pre class="source" title="StaticResourceExtension" lang="">
<code>StaticResourceExtension ex = <span class="reserved">new</span> StaticResourceExtension();
ex.ResourceKey = <span class="literal">"brush1"</span>;

TextBlock textblock1 = <span class="reserved">new</span> TextBlock();
textblock1.Background = (Brush)ex.ProvideValue(serviceProvider);
textblock1.Text       = <span class="literal">"textblock 1"</span>;

</code></pre>


StaticResourceExtension クラスは、
リソース辞書（Page.Resources 中などで定義されたリソース一覧）の中から、
brush1 というキーを持つリソースを捜してくる機構を持っているわけです。

ちなみに、StaticResourceExtension クラスは、
プロパティが ResourceKey の1つだけなので、
本来は {StaticResource ResourceKey=brush1} と書くべき所を、
{StaticResource brush1} と省略できます。

StaticResource 以外にも、
DynamicResource や Binding などのマークアップ拡張がありますが、
これらはすべて、MarkupExtension クラスのサブクラスです。
MarkupExtension クラスのサブクラスを実装すれば、
マークアップ拡張の自作も可能です。
（「マークアップ拡張」という名前がそもそも、「XAML の構文をユーザが拡張できる」という意味。）

マークアップ拡張は
StaticResource や Binding 以外にもたくさん標準で用意されています。
どのようなマークアップ拡張が標準で用意されているのかは WPF のヘルプを参照してください。


## <a id="sec-generated-title-7"></a> <a id="style"></a>スタイル

コントロールごとに Foreground や Background プロパティの値を設定することで、
個々に文字色などを変更することができました。
これに対して、
「全てのテキストボックスを一律同じ文字色に変更したい」
などといった要望もあると思います。
このような要望は、<strong id="style" class="keyword">スタイル</strong>（style）というものを使って実現することができます。

XAML のスタイルは、HTML に対する CSS のような感じで設定できます。
HTML の場合、&lt;p&gt; タグに対して一律フォントサイズを 18pt にしたければ、
CSS 中に p {font-size: 18pt;} などと記述しました。
また、特定の &lt;p&gt; タグに対してだけスタイルを適用したければ、
CSS の方で p.footnote {font-size: 10pt;} などというようにクラス名を付けて定義し、
HTML の方では &lt;p class="footnote"&gt; というようにします。

XAML のスタイルでもほぼ同様のことが実現できます。
まず、一律でスタイルを指定する方法ですが、
以下のようにします。


<pre class="xsource" title="TextBlock に一律スタイルを適用">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">FontSize</span><span class="attvalue">="18pt"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
<em>    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Foreground"</span> <span class="attribute">Value</span><span class="attvalue">="Blue"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontFamily"</span> <span class="attribute">Value</span><span class="attvalue">="Times New Roman"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontStyle"</span> <span class="attribute">Value</span><span class="attvalue">="Italic"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span></em>
  <span class="bracket">&lt;</span>/<span class="element">Page.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 3"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
スタイルは、リソース中に Style 要素を記述することで定義できます。
Style の TargetType プロパティには型名を指定します。
このとき、Style 要素に x:Key 属性を付けなければ、
指定した型の要素全てにスタイルが適用されるようになります。

Style 中には、Setter というものを並べて、
この中でどのプロパティにどういう値を設定するかを設定します。
この例の場合、全ての TextBlock が、Times New Roman のイタリック体の青色文字になります。

一方で、以下のように、Style 要素に x:Key を指定すると、
特定の要素にのみスタイルが適用されるようになります。
適用したい要素には、Style 属性を付けます。


<pre class="xsource" title="特定の TextBlock にスタイルを適用">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">FontSize</span><span class="attvalue">="18pt"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span> <em><span class="attribute">x:Key</span><span class="attvalue">="em"</span></em><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Foreground"</span> <span class="attribute">Value</span><span class="attvalue">="Blue"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontFamily"</span> <span class="attribute">Value</span><span class="attvalue">="Times New Roman"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontStyle"</span> <span class="attribute">Value</span><span class="attvalue">="Italic"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Page.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <em><span class="attribute">Style</span><span class="attvalue">="{StaticResource em}"</span></em> <span class="attribute">Text</span><span class="attvalue">="text block 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 3"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
もちろん、
特定の要素にだけ特別なスタイルを適用しつつ、
残り全ての要素にもスタイルを適用することもできます。


<pre class="xsource" title="">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">FontSize</span><span class="attvalue">="18pt"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontFamily"</span> <span class="attribute">Value</span><span class="attvalue">="Times New Roman"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontStyle"</span> <span class="attribute">Value</span><span class="attvalue">="Italic"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span> <span class="attribute">x:Key</span><span class="attvalue">="em"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Foreground"</span> <span class="attribute">Value</span><span class="attvalue">="Blue"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontWeight"</span> <span class="attribute">Value</span><span class="attvalue">="Bold"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Page.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Style</span><span class="attvalue">="{StaticResource em}"</span> <span class="attribute">Text</span><span class="attvalue">="text block 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 3"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
この場合、「text block 1」だけが青色太字に、
残りのテキストブロックは Times New Roman のイタリック体になります。

スタイルは別のスタイルを継承・拡張する形で定義することもできます。
Style 要素に対して BasedOn 属性を指定します。
BasedOn 属性には、x:Key 名か TargetType で指定した型を参照するようにします。


<pre class="xsource" title="">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">FontSize</span><span class="attvalue">="18pt"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontFamily"</span> <span class="attribute">Value</span><span class="attvalue">="Times New Roman"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontStyle"</span> <span class="attribute">Value</span><span class="attvalue">="Italic"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <em><span class="attribute">BasedOn</span><span class="attvalue">="{StaticResource {x:Type TextBlock}}"</span></em>
      <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span> <span class="attribute">x:Key</span><span class="attvalue">="em"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="FontWeight"</span> <span class="attribute">Value</span><span class="attvalue">="Bold"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <em><span class="attribute">BasedOn</span><span class="attvalue">="{StaticResource em}"</span></em>
      <span class="attribute">TargetType</span><span class="attvalue">="TextBlock"</span> <span class="attribute">x:Key</span><span class="attvalue">="emred"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Foreground"</span> <span class="attribute">Value</span><span class="attvalue">="Red"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Page.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Style</span><span class="attvalue">="{StaticResource em}"</span> <span class="attribute">Text</span><span class="attvalue">="text block 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 1"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Style</span><span class="attvalue">="{StaticResource emred}"</span> <span class="attribute">Text</span><span class="attvalue">="text block 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="button 2"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <span class="attribute">Text</span><span class="attvalue">="text block 3"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
この例では、
特に指定のないテキストブロックは Times New Roman イタリック、
「text block 1」は Times New Roman イタリックに加えて太字、
「text block 2」は Times New Roman 太字イタリックにさらに赤文字になります。
