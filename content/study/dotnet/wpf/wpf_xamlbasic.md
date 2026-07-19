---
title: "XAML の基本構造（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_xamlbasic/"
content_type: "Article"
published_at: "2006-11-17T00:00:00"
updated_at: "2007-07-07T00:00:00"
tags: []
umbraco_id: 1396
parent_id: 1393
sort_order: 2
aliases:
  - "/dotnet/wpf/wpf_xamlbasic/"
  - "/dotnet/wpf_xamlbasic"
  - "/dotnet/wpf_xamlbasic.html"
  - "/study/dotnet/wpf_xamlbasic"
  - "/study/dotnet/wpf_xamlbasic.html"
---

# XAML の基本構造（WPF）

## <a id="sec-generated-title-1"></a> <a id="structure"></a>基本構造

XAML の基本を説明するために、
「[XAML 概要（WPF）](wpf_xaml.md)」で例に出した、
テキストボックスを2つ表示するコードをもう1度見てみましょう。


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Background="White"
  >

  <WrapPanel>
    <TextBox 
      Width = "100" FontSize = "30" Text = "text 1"
      Background = "White" Foreground = "Blue" />
    <TextBox 
      Width = "100" FontSize = "30" Text = "text 2"
      Background = "White" Foreground = "Green" />
  </WrapPanel>
</Page>
```
まず、ルート要素（この例では Page タグ）ですが、
これは、
通常、Windows アプリケーションを作りたいなら Window タグ、
ウェブアプリケーション（ブラウザ上で実行）なら Page、
ユーザコントロール（ボタンやテキストボックスのようなものを自作したい場合）なら UserControl とします。

Page タグ中にある、「xmlns」とか「xmlns:x」などの属性は、
XML の決まりごとです。
「この XML ファイルは XAML です」と言うことを宣言しているようなもので、
よく分からない場合はとりあえずおまじないだと思って、この通りに書いておいてください。

さて、ここからが本題になりますが、
XAML 中で使えるタグ名は、
実は
「[Windows Presentation Foundation](wpf_abst.md#wpf)」
というライブラリ中で定義されたクラスです。
（XAML というのは、要するに、XML のタグと .NET Framework のクラスを結びつける機構。）
&lt;Page&gt; タグには Page というクラスが、
&lt;WrapPanel&gt; には WrapPanel というクラスが対応します。

タグ中の属性（上の例の Page の場合、Background など）は、
そのクラスの「[プロパティ](../../csharp/oop/oo_property.md#property)」です。
（正確には、プロパティ“<em>も</em>”使える。
実際には、単なるプロパティでは実現できない特別な処理を行うために、
ディペンデンシープロパティというものを使うことがあります。）

ということで、この XAML コードは、C# 的に書き直すなら、以下のようになります。
（まんまこの通りになるわけではないです。あくまで概念。）

```csharp
class Page1 : Page
{
  public Page1()
  {
    this.Background = Colors.White;

    WrapPanel panel1 = new WrapPanel();

    this.Content = panel1;

    TextBox textbox1 = new TextBox();
    textbox1.Width = 100;
    textbox1.FontSize = 30;
    textbox1.Background = Colors.White;
    textbox1.Foreground = Colors.Blue;
    textbox1.Text = "text 1";

    panel1.Children.Add(textbox1);

    TextBox textbox2 = new TextBox();
    textbox2.Width = 100;
    textbox2.FontSize = 30;
    textbox2.Background = Colors.White;
    textbox2.Foreground = Colors.Green;
    textbox2.Text = "text 2";

    panel1.Children.Add(textbox2);
  }
}
```



## <a id="sec-generated-title-2"></a> <a id="property"></a>プロパティの設定

これまでにも何度か例に出ていますが、
XAML では、XML の属性として「[プロパティ](../../csharp/oop/oo_property.md#property)」の値を設定できます。


```xml
<TextBox 
  Width = "100" FontSize = "30" Text = "text 1"
  Background = "White" Foreground = "Blue" />
```
この書き方は <strong id="attribute" class="keyword">Attribute Syntax</strong> と言って、
値を文字列で指定できる（文字列そのもの or 文字列から直接変換可能な型）プロパティの場合はこの構文を使うと便利です。

では、もっと複雑な型を持つプロパティの場合にはどうすればいいかと言うと、
XML 要素の子要素としてプロパティの値を設定する Property Element Syntax という構文も用意されています。
例えば、上の例を <strong id="property" class="keyword">Property Element Syntax</strong> で書き直すと以下のようになります。


```xml
<TextBox>
  <TextBox.Width>100</TextBox.Width>
  <TextBox.FontSize>30</TextBox.FontSize>
  <TextBox.Background>White</TextBox.Background>
  <TextBox.Foreground>Blue</TextBox.Foreground>
  <TextBox.Text>text 1</TextBox.Text>
</TextBox>
```
この例では、Background / Foreground の中身は相変わらず文字列からの自動型変換
（文字列 → SolidColorBrush への変換）
に頼っているわけですが、
これも省略せずに書くなら以下のようになります。


```xml
<TextBox>
  <TextBox.Width>100</TextBox.Width>
  <TextBox.FontSize>30</TextBox.FontSize>
  <TextBox.Background><SolidColorBrush Color="White"/></TextBox.Background>
  <TextBox.Foreground><SolidColorBrush Color="Blue"/></TextBox.Foreground>
  <TextBox.Text>text 1</TextBox.Text>
</TextBox>
```
文字列からの自動変換に頼らず、ちゃんとブラシを指定するなら、
グラデーションの掛かった柄（LinearGradientBrush、RadialGradientBrush）や、
画像（ImageBrush）などを背景・前景色に指定する事もできます。

ちなみに、「[Windows Presentation Foundation](wpf_abst.md#wpf)」 でよく使う型には、文字列からの変換関数が標準で用意されているので、
複雑な型でなければたいていは Attribute Syntax が利用できます。


## <a id="sec-generated-title-3"></a> <a id="content"></a>コンテントプロパティ

基本的に、XAML 中のある要素（例えば &lt;Button&gt;）の子は、
その要素に対応するクラス（&lt;Button&gt; の場合、Button クラス）のプロパティになります。


```html
<Button>
  <Button.Background>Gray</Button.Background>
  <Button.Foreground>White</Button.Foreground>
  <Button.Content>ここを押して</Button.Content>
</Button>
```
ただ、
<strong id="content" class="keyword">コンテントプロパティ</strong>（content property）という物に指定されているプロパティに限っては省略が可能になっています。
Button クラスでは、Content がコンテントプロパティに指定されていて、
上の例の &lt;Button.Content&gt; タグは省略可能になり、以下のように書けます。


```html
<Button>
  <Button.Background>Gray</Button.Background>
  <Button.Foreground>White</Button.Foreground>
  ここを押して
</Button>
```
どのプロパティがコンテントプロパティかは、
ContentProperty 「[属性](../../csharp/dynamic/sp_attribute.md#attribute)」で指定されています。
例えば、ContentControl クラスには、
<code>[ContentProperty("Content")] </code> 属性が付いているので、
ContentControl のサブクラスに当たる Button や Label などは、
Button.Content や Label.Contet がコンテントプロパティになります。
また、
Panel クラスには
<code>[ContentProperty("Children")] </code> 属性が付いていて、
そのサブクラスの WrapPanel や StackPanel クラスなどは、
Children がコンテントプロパティになります。
TextBox の場合、Text がコンテントプロパティです。

それと、
コンテントプロパティは、他のプロパティより前か、後ろにまとまっている必要があります。
例えば、以下のような書き方はエラーになります。


```html
<Button>
  他の要素よりも前にテキスト
  <Button.Background>Gray</Button.Background>
  <Button.Foreground>White</Button.Foreground>
  かつ、後ろにもテキスト
</Button>
```

## <a id="sec-generated-title-4"></a> <a id="collection"></a>プロパティがコレクションの場合

「[基本構造](#structure)」で挙げた例で、
WrapPanel の直下に TextBox タグが書けるのも実はコンテントプロパティによる省略です。


```xml
<WrapPanel>
  <TextBox Foreground = "Blue" Text = "text 1" />
  <TextBox Foreground = "Green" Text = "text 2" />
</WrapPanel>
```
これも、コンテントプロパティを省略せずに書くと以下のようになります。


```xml
<WrapPanel>
  <WrapPanel.Children>
    <TextBox Foreground = "Blue" Text = "text 1" />
    <TextBox Foreground = "Green" Text = "text 2" />
  <WrapPanel.Children>
</WrapPanel>
```
この例では、実はもう1つ省略しているものがあります。
WrapPanel の「[コンテントプロパティ](#content)」は Children なんですが、
この Children の型は UIElementCollection です。
なので、省略せずに書くなら、
上の例は以下のようになります。


```xml
<WrapPanel>
  <WrapPanel.Children>
    <UIElementCollection>
      <TextBox Foreground = "Blue" Text = "text 1" />
      <TextBox Foreground = "Green" Text = "text 2" />
    </UIElementCollection>
  </WrapPanel.Children>
</WrapPanel>
```
要するに、コレクション（IList、IDictionary を実装するクラスか、配列）の場合、タグを1レベル省略することが可能です。


## <a id="sec-generated-title-5"></a> <a id="xmlns"></a>XML 名前空間

今までさらっと流していましたが、
XAML 中で使う XML 名前空間について説明します。
（「XML 名前空間」の概念自体は XML の入門サイトを探してもらうことにして、
XAML に関する話だけを。）

「[基本構造](#structure)」で説明したように、
XAML は、XML タグと .NET Framework クラスを結びつけるための機構です。
なので、XAML の仕様自体が持っているタグと、
「[WPF](wpf_abst.md#wpf0)」 のライブラリ中で定義されているタグ（＝ クラス群）の2種類のタグがあります。

これまでの例では、
最上位のタグに以下のような xmlns 属性が付いていました。


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  >
```
このうち、
http://schemas.microsoft.com/winfx/2006/xaml/presentation の方が WPF で定義されたタグ（クラス群）、
http://schemas.microsoft.com/winfx/2006/xaml の方が XAML の仕様自体に含まれるタグをあらわす XML 名前空間です。

また、XAML では、任意の .NET Framework クラスを XML タグと結びつけることが出来ます。
以下のように、
xmlns:c="clr-namespace:My.Namespace" みたいな書き方をすることで、
My.Namespace 名前空間中のクラス名を XML タグとして利用可能になります。
（コンパイル必須。「[Loose XAML](wpf_xaml.md#loose)」では無理。）


```xml
<Page
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  xmlns:sys="clr-namespace:System"
  >
  <Page.Resources>
    <sys:DateTime x:Key="date"/>
  </Page.Resources>
</Page>
```
ちなみに、WPF と同様に、Silverlight も XAML を利用するわけですが、
Silverlight の場合には、
以下のような XML 名前空間を使います。


```xml
<Canvas
  xmlns="http://schemas.microsoft.com/client/2007"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  >
```
http://schemas.microsoft.com/winfx/2006/xaml の方は XAML の仕様自体のものなので、WPF でも Silverlight でも共通です。
