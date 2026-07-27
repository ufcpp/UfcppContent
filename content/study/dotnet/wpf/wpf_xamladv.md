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

```csharp
object val = GetValue(DependencyPropertyIdentifier);
SetValue(DependencyPropertyIdentifier, val);
```


DependencyPropertyIdentifier の部分は、
DependencyProperty クラスのインスタンスを渡します。
この DependencyProperty クラスのインスタンスは、
static readonly なメンバー変数としてクラス中に定義します。
例えば、TextBlock という名前のクラス中に Text という名前の依存プロパティを作りたければ、以下のようにします。

```csharp
public class TextBlock
{
  public static readonly DependencyProperty TextProperty =
    DependencyProperty.Register("Text", typeof(string), typeof(TextBlock));
}
```


通常、利便性のために、
同名の CLR プロパティも用意しておきます。

```csharp
public class TextBlock : DependencyObject
{
  public static readonly DependencyProperty TextProperty =
    DependencyProperty.Register("Text", typeof(string), typeof(TextBlock));

  public string Text
  {
    get { return (string)this.GetValue(TextProperty); }
    set { this.SetValue(TextProperty, value); }
  }
}
```


これで、XAML 中で、


```xml
<TextBlock Name="textBlock" Text="テキスト" />
```
と書けば、（通常の Text プロパティよりも、依存プロパティの TextProperty が優先されて、）
以下のコードと同じ効果が得られます。

```csharp
textBlock.SetValue(TextProperty, "テキスト");
```



## <a id="sec-generated-title-2"></a> <a id="attached"></a>添付プロパティ

「[依存プロパティ](#dependency)」にできて通常のプロパティにできないことの最たるものが、
<strong id="attached" class="keyword">添付プロパティ</strong>（attached property）です。

添付プロパティの例を示すのに、Grid を使ってみましょう。
Grid は、子要素をテーブル状にレイアウトするためのものです。
子要素を何行何列目に配置するかは、以下のように、
Grid.Row と Grid.Column を使って指定します。


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  >
  <Grid Width="120" Height="120" Background="Black">
    <Grid.ColumnDefinitions>
      <ColumnDefinition />
      <ColumnDefinition />
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
      <RowDefinition />
      <RowDefinition />
    </Grid.RowDefinitions>

    <Border Grid.Row="0" Grid.Column="0"
      Width="50" Height="50" Background="Red"/>
    <Border Grid.Row="0" Grid.Column="1"
      Width="50" Height="50" Background="Green"/>
    <Border Grid.Row="1" Grid.Column="0"
      Width="50" Height="50" Background="Blue"/>
    <Border Grid.Row="1" Grid.Column="1"
      Width="50" Height="50" Background="Yellow"/>
  </Grid>
</Page>
```
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


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
  <Page.Resources>
    <LinearGradientBrush x:Key="brush1"
      StartPoint="0, 0" EndPoint="1, 1">
      <GradientStop Color="Violet" Offset="0.0" />
      <GradientStop Color="Coral" Offset="1.0" />
    </LinearGradientBrush>
  </Page.Resources>
</Page>
```
リソースを参照するときのことを考えて、
x:Key 属性をつけておきます。

リソースを参照する側では、
「[Attribute Syntax](wpf_xamlbasic.md#attribute)」 の場合には、


```html
<object property="{StaticResource key}" .../>
```
「[Property Element Syntax](wpf_xamlbasic.md#property)」 の場合には、


```html
<object>
  <object.property>
    <StaticResource ResourceKey="key" .../>
  </object.property>
</object>
```
というように書きます。
先ほどの例のグラデーションブラシをテキストブロックの背景として参照したい場合には、
以下のように書きます。


```xml
<TextBlock Background="{StaticResource brush1}" Text="textblock 1"/>
```

##### <a id="sec-generated-title-4"></a>サンプル

```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
  <Page.Resources>
    <LinearGradientBrush x:Key="brush1"
      StartPoint="0, 0" EndPoint="1, 1">
      <GradientStop Color="Violet" Offset="0.0" />
      <GradientStop Color="Coral" Offset="1.0" />
    </LinearGradientBrush>
    <LinearGradientBrush x:Key="brush2"
      StartPoint="0, 0" EndPoint="1, 1">
      <GradientStop Color="Turquoise" Offset="0.0" />
      <GradientStop Color="Gainsboro" Offset="1.0" />
    </LinearGradientBrush>
  </Page.Resources>

  <StackPanel Orientation="Vertical">
    <TextBlock Background="{StaticResource brush1}" Text="textblock 1"/>
    <TextBlock Background="{StaticResource brush2}" Text="textblock 2"/>
    <Button    Background="{StaticResource brush1}" Content="button 1"/>
    <TextBox   Background="{StaticResource brush2}" Text="textbox 1"/>
  </StackPanel>
</Page>
```

### <a id="sec-generated-title-5"></a> <a id="extern_resource"></a>外部リソース

Resources プロパティの型は ResourceDictionary なんですが、
ResourceDictionary の Source プロパティを指定することで、
外部の XAML からリソースを読みこむことができます。

例えば、まず、以下のような XAML を StyleForLabel.xaml という名前で保存して、


```xml
<ResourceDictionary
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  >
  <Style TargetType="{x:Type Label}">
    <Setter Property="Background" Value="#eeeeff"/>
  </Style>
</ResourceDictionary>
```
以下のような XAML を書くと、
StyleForLabel.xaml 中の設定が反映されます。


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  >
  <WrapPanel.Resources>
    <ResourceDictionary Source="StyleForLabel.xaml"/>
  </WrapPanel.Resources>

  <Label Content="label 1"/>
  <Label Content="label 2"/>
  <Label Content="label 3"/>
</WrapPanel>
```
（スタイルに関しては、「[スタイル](#style)」で説明します。）

複数の外部リソースをマージしたければ、
以下のように、MergedDictionaries プロパティを設定します。


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  >
  <WrapPanel.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="StyleForButton.xaml"/>
        <ResourceDictionary Source="StyleForLabel.xaml"/>
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </WrapPanel.Resources>

  <Button Content="button 1"/>
  <Label Content="label 1"/>
  <Button Content="button 2"/>
  <Label Content="label 2"/>
  <Button Content="button 3"/>
  <Label Content="label 3"/>
</WrapPanel>
```
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


```xml
<TextBlock
  Name="textblock1"
  Background="{StaticResource brush1}"
  Text="textblock 1"/>
```
の場合、以下のようなコードと同じような意味合いになります。

```csharp
StaticResourceExtension ex = new StaticResourceExtension();
ex.ResourceKey = "brush1";

TextBlock textblock1 = new TextBlock();
textblock1.Background = (Brush)ex.ProvideValue(serviceProvider);
textblock1.Text       = "textblock 1";
```


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


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  FontSize="18pt">
  <Page.Resources>
    <Style TargetType="TextBlock">
      <Setter Property="Foreground" Value="Blue" />
      <Setter Property="FontFamily" Value="Times New Roman"/>
      <Setter Property="FontStyle" Value="Italic"/>
    </Style>
  </Page.Resources>

  <StackPanel Orientation="Vertical">
    <TextBlock Text="text block 1"/>
    <Button Content="button 1"/>
    <TextBlock Text="text block 2"/>
    <Button Content="button 2"/>
    <TextBlock Text="text block 3"/>
  </StackPanel>
</Page>
```
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


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  FontSize="18pt">
  <Page.Resources>
    <Style TargetType="TextBlock" x:Key="em">
      <Setter Property="Foreground" Value="Blue" />
      <Setter Property="FontFamily" Value="Times New Roman"/>
      <Setter Property="FontStyle" Value="Italic"/>
    </Style>
  </Page.Resources>

  <StackPanel Orientation="Vertical">
    <TextBlock Style="{StaticResource em}" Text="text block 1"/>
    <Button Content="button 1"/>
    <TextBlock Text="text block 2"/>
    <Button Content="button 2"/>
    <TextBlock Text="text block 3"/>
  </StackPanel>
</Page>
```
もちろん、
特定の要素にだけ特別なスタイルを適用しつつ、
残り全ての要素にもスタイルを適用することもできます。


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  FontSize="18pt">
  <Page.Resources>
    <Style TargetType="TextBlock">
      <Setter Property="FontFamily" Value="Times New Roman"/>
      <Setter Property="FontStyle" Value="Italic"/>
    </Style>
    <Style TargetType="TextBlock" x:Key="em">
      <Setter Property="Foreground" Value="Blue" />
      <Setter Property="FontWeight" Value="Bold" />
    </Style>
  </Page.Resources>

  <StackPanel Orientation="Vertical">
    <TextBlock Style="{StaticResource em}" Text="text block 1"/>
    <Button Content="button 1"/>
    <TextBlock Text="text block 2"/>
    <Button Content="button 2"/>
    <TextBlock Text="text block 3"/>
  </StackPanel>
</Page>
```
この場合、「text block 1」だけが青色太字に、
残りのテキストブロックは Times New Roman のイタリック体になります。

スタイルは別のスタイルを継承・拡張する形で定義することもできます。
Style 要素に対して BasedOn 属性を指定します。
BasedOn 属性には、x:Key 名か TargetType で指定した型を参照するようにします。


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  FontSize="18pt">
  <Page.Resources>
    <Style TargetType="TextBlock">
      <Setter Property="FontFamily" Value="Times New Roman"/>
      <Setter Property="FontStyle" Value="Italic"/>
    </Style>
    <Style BasedOn="{StaticResource {x:Type TextBlock}}"
      TargetType="TextBlock" x:Key="em">
      <Setter Property="FontWeight" Value="Bold" />
    </Style>
    <Style BasedOn="{StaticResource em}"
      TargetType="TextBlock" x:Key="emred">
      <Setter Property="Foreground" Value="Red" />
    </Style>
  </Page.Resources>

  <StackPanel Orientation="Vertical">
    <TextBlock Style="{StaticResource em}" Text="text block 1"/>
    <Button Content="button 1"/>
    <TextBlock Style="{StaticResource emred}" Text="text block 2"/>
    <Button Content="button 2"/>
    <TextBlock Text="text block 3"/>
  </StackPanel>
</Page>
```
この例では、
特に指定のないテキストブロックは Times New Roman イタリック、
「text block 1」は Times New Roman イタリックに加えて太字、
「text block 2」は Times New Roman 太字イタリックにさらに赤文字になります。
